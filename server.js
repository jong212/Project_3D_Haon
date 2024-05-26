const express = require('express');
const cors = require('cors');
const sql = require('mssql');
const bcrypt = require('bcrypt');
require('dotenv').config();

const app = express();
const port = process.env.PORT || 3000;

app.use(cors());
app.use(express.json());

const dbConfig = {
  user: process.env.DB_USER,
  password: process.env.DB_PASSWORD,
  server: process.env.DB_SERVER,
  database: process.env.DB_DATABASE,
  options: {
    encrypt: true,
    trustServerCertificate: false, // true로 설정하면 자체 서명된 SSL 인증서 사용 가능
  },
};
sql.connect(dbConfig, (err) => {
    if (err) {
        console.error('데이터베이스 연결 실패:', err);
    } else {
        console.log('데이터베이스 연결 성공');
    }
});

// 회원가입 엔드포인트
app.post('/api/register', async (req, res) => {
    const transaction = new sql.Transaction();
    try {
        const { Username, Password, PlayerName } = req.body;

        console.log('Starting transaction...');
        await transaction.begin();
        console.log('Transaction started.');
        let request = new sql.Request(transaction);

        // 사용자 아이디 중복 확인
        const userCheck = await request
            .input('CheckUsername', sql.NVarChar, Username)
            .query('SELECT COUNT(*) as count FROM Users WHERE Username = @CheckUsername');

        if (userCheck.recordset[0].count > 0) {
            console.log('Username already exists.');
            await transaction.rollback();
            return res.status(409).json({ errorCode: 'USERNAME_EXISTS', message: 'Username already exists' });
        }

        // PlayerName 중복 확인
        request = new sql.Request(transaction);
        const playerNameCheck = await request
            .input('CheckPlayerName', sql.NVarChar, PlayerName)
            .query('SELECT COUNT(*) as count FROM Players WHERE PlayerName = @CheckPlayerName');

        if (playerNameCheck.recordset[0].count > 0) {
            console.log('Player name already exists.');
            await transaction.rollback();
            return res.status(409).json({ errorCode: 'PLAYERNAME_EXISTS', message: 'Player name already exists' });
        }

        // 비밀번호 해싱
        const hashedPassword = await bcrypt.hash(Password, 10);

        // Users 테이블에 사용자 정보 삽입
        request = new sql.Request(transaction);
        const userResult = await request
            .input('InsertUsername', sql.NVarChar, Username)
            .input('InsertPasswordHash', sql.NVarChar, hashedPassword)
            .input('InsertPlayerNameUser', sql.NVarChar, PlayerName)
            .query('INSERT INTO Users (Username, PasswordHash, PlayerName) VALUES (@InsertUsername, @InsertPasswordHash, @InsertPlayerNameUser); SELECT SCOPE_IDENTITY() AS NewUserID');

        const newUserId = userResult.recordset[0].NewUserID;

        // Players 테이블에 사용자 정보 삽입
        request = new sql.Request(transaction);
        await request
            .input('InsertNewUserID', sql.Int, newUserId)
            .input('InsertPlayerNamePlayer', sql.NVarChar, PlayerName)
            .input('InsertUsername', sql.NVarChar, Username) 
            .input('InsertGems', sql.Int, 50)
            .input('InsertCoins', sql.Int, 50)
            .input('InsertMaxHealth', sql.Int, 500)
            .input('InsertHealthEnhancement', sql.Int, 0)
            .input('InsertAttackPower', sql.Int, 40)
            .input('InsertAttackEnhancement', sql.Int, 0)
            .input('InsertWeaponEnhancement', sql.Int, 0)
            .input('InsertArmorEnhancement', sql.Int, 0)
            .query(`
        INSERT INTO Players (UserID, Username, PlayerName, Gems, Coins, MaxHealth, HealthEnhancement, AttackPower, AttackEnhancement, WeaponEnhancement, ArmorEnhancement)
        VALUES (@InsertNewUserID, @InsertUsername, @InsertPlayerNamePlayer, @InsertGems, @InsertCoins, @InsertMaxHealth, @InsertHealthEnhancement, @InsertAttackPower, @InsertAttackEnhancement, @InsertWeaponEnhancement, @InsertArmorEnhancement)
      `);

        // 트랜잭션 커밋
        await transaction.commit();
        console.log('Transaction committed.');

        // 새로 생성된 사용자 정보 반환
        request = new sql.Request();
        const newUser = await request
            .input('SelectNewUserID', sql.Int, newUserId)
            .query('SELECT UserID, Username, PlayerName FROM Users WHERE UserID = @SelectNewUserID');

        res.status(201).json(newUser.recordset[0]);
    } catch (err) {
        console.error('회원가입 오류:', err);
        if (transaction._aborted) {
            console.log('Transaction already aborted.');
        } else if (transaction._begun) {
            console.log('Transaction will be rolled back.');
            try {
                await transaction.rollback();
            } catch (rollbackErr) {
                console.error('Rollback error:', rollbackErr);
            }
        } else {
            console.log('Transaction was not begun.');
        }
        res.status(500).json({ errorCode: 'SERVER_ERROR', message: 'An error occurred during registration' });
    } finally {
        if (transaction._begun) {
            try {
                await transaction.release();
                console.log('Transaction released.');
            } catch (releaseErr) {
                console.error('Release error:', releaseErr);
            }
        }
    }
});

// 로그인 엔드포인트
app.post('/api/login', async (req, res) => {
    try {
        const { Username, Password } = req.body;
        const pool = await sql.connect(dbConfig);

        // 사용자 정보 조회
        const userResult = await pool.request()
            .input('LoginUsername', sql.NVarChar, Username)
            .query('SELECT UserID, Username, PasswordHash, PlayerName FROM Users WHERE Username = @LoginUsername');

        if (userResult.recordset.length === 0) {
            return res.status(404).json({ message: 'Invalid username or password' });
        }

        const user = userResult.recordset[0];

        // 비밀번호 확인
        const isPasswordValid = await bcrypt.compare(Password, user.PasswordHash);
        if (!isPasswordValid) {
            return res.status(404).json({ message: 'Invalid username or password' });
        }

        // 플레이어 정보 조회
        const playerResult = await pool.request()
            .input('UserId', sql.Int, user.UserID)
            .query('SELECT * FROM Players WHERE UserID = @UserId');

        const player = playerResult.recordset[0];

        const characterData = {
            PlayerId: player.Username,
            PlayerName: player.PlayerName,
            Gems: player.Gems,
            Coins: player.Coins,
            MaxHealth: player.MaxHealth,
            HealthEnhancement: player.HealthEnhancement,
            AttackPower: player.AttackPower,
            AttackEnhancement: player.AttackEnhancement,
            WeaponEnhancement: player.WeaponEnhancement,
            ArmorEnhancement: player.ArmorEnhancement
        };

        res.status(200).json({ message: 'Login successful', UserId: user.UserID, Character: characterData });
    } catch (err) {
        console.error('로그인 오류:', err);
        res.status(500).send('서버 오류');
    }
});

// 사용자 정보 조회 엔드포인트
app.get('/api/login/:id', async (req, res) => {
    try {
        const { id } = req.params;
        const pool = await sql.connect(dbConfig);
        const result = await pool.request()
            .input('GetUserID', sql.Int, id)
            .query('SELECT UserID, Username, PlayerName FROM Users WHERE UserID = @GetUserID');

        if (result.recordset.length === 0) {
            return res.status(404).json({ message: 'User not found' });
        }

        res.json(result.recordset[0]);
    } catch (err) {
        console.error('사용자 정보 조회 오류:', err);
        res.status(500).send('서버 오류');
    }
});

// 플레이어 정보 조회 엔드포인트
app.get('/api/players/:id', async (req, res) => {
    try {
        const { id } = req.params;
        const pool = await sql.connect(dbConfig);
        const result = await pool.request()
            .input('GetPlayerID', sql.Int, id)
            .query('SELECT * FROM Players WHERE PlayerID = @GetPlayerID');
        res.json(result.recordset);
    } catch (err) {
        console.error('데이터 조회 오류:', err);
        res.status(500).send('서버 오류');
    }
});

// 모든 플레이어 정보 조회 엔드포인트
app.get('/api/players', async (req, res) => {
    try {
        const pool = await sql.connect(dbConfig);
        const result = await pool.request().query('SELECT * FROM Players');
        res.json(result.recordset);
    } catch (err) {
        console.error('데이터 조회 오류:', err);
        res.status(500).send('서버 오류');
    }
});

// 특정 플레이어 정보 조회 시 Username 포함 엔드포인트
app.get('/api/player-data/:username', async (req, res) => {
    try {
        const { username } = req.params;
        const pool = await sql.connect(dbConfig);
        const result = await pool.request()
            .input('Username', sql.NVarChar, username)
            .query(`
                SELECT p.*, u.Username 
                FROM Players p 
                JOIN Users u ON p.UserID = u.UserID 
                WHERE u.Username = @Username
            `);

        if (result.recordset.length === 0) {
            return res.status(404).json({ message: 'Player data not found' });
        }

        res.json(result.recordset[0]);
    } catch (err) {
        console.error('데이터 조회 오류:', err);
        res.status(500).send('서버 오류');
    }
});

// 모든 플레이어 정보 조회 시 Username 포함 엔드포인트
app.get('/api/players/with-username', async (req, res) => {
    try {
        const pool = await sql.connect(dbConfig);
        const result = await pool.request()
            .query(`
                SELECT p.*, u.Username 
                FROM Players p 
                JOIN Users u ON p.UserID = u.UserID
            `);
        res.json(result.recordset);
    } catch (err) {
        console.error('데이터 조회 오류:', err);
        res.status(500).send('서버 오류');
    }
});

// 플레이어 정보 업데이트 엔드포인트
app.put('/api/players/:id', async (req, res) => {
    try {
        const { id } = req.params;
        const {
            PlayerName, Gems, Coins, MaxHealth,
            HealthEnhancement, AttackPower, AttackEnhancement,
            WeaponEnhancement, ArmorEnhancement
        } = req.body;

        // 음수 값 검증
        if (Gems < 0 || Coins < 0 || MaxHealth < 0 ||
            HealthEnhancement < 0 || AttackPower < 0 ||
            AttackEnhancement < 0 || WeaponEnhancement < 0 ||
            ArmorEnhancement < 0) {
            return res.status(400).json({ message: 'Negative values are not allowed' });
        }

        const pool = await sql.connect(dbConfig);

        // 플레이어가 존재하는지 확인
        const playerCheckResult = await pool.request()
            .input('PlayerID', sql.Int, id)
            .query('SELECT COUNT(*) AS count FROM Players WHERE PlayerID = @PlayerID');

        if (playerCheckResult.recordset[0].count === 0) {
            return res.status(404).send('플레이어 정보를 찾을 수 없습니다.');
        }

        console.log(`Updating player data for PlayerID: ${id}`);
        console.log(`Request Body: ${JSON.stringify(req.body)}`);

        const result = await pool.request()
            .input('UpdatePlayerID', sql.Int, id)
            .input('UpdatePlayerName', sql.NVarChar, PlayerName)
            .input('UpdateGems', sql.Int, Gems)
            .input('UpdateCoins', sql.Int, Coins)
            .input('UpdateMaxHealth', sql.Int, MaxHealth)
            .input('UpdateHealthEnhancement', sql.Int, HealthEnhancement)
            .input('UpdateAttackPower', sql.Int, AttackPower)
            .input('UpdateAttackEnhancement', sql.Int, AttackEnhancement)
            .input('UpdateWeaponEnhancement', sql.Int, WeaponEnhancement)
            .input('UpdateArmorEnhancement', sql.Int, ArmorEnhancement)
            .query(`
                UPDATE Players SET 
                  PlayerName = @UpdatePlayerName,
                  Gems = @UpdateGems,
                  Coins = @UpdateCoins,
                  MaxHealth = @UpdateMaxHealth,
                  HealthEnhancement = @UpdateHealthEnhancement,
                  AttackPower = @UpdateAttackPower,
                  AttackEnhancement = @UpdateAttackEnhancement,
                  WeaponEnhancement = @UpdateWeaponEnhancement,
                  ArmorEnhancement = @UpdateArmorEnhancement
                WHERE PlayerID = @UpdatePlayerID
            `);

        if (result.rowsAffected[0] > 0) {
            res.status(200).send('플레이어 정보 업데이트 성공');
            console.log('플레이어 정보 업데이트 성공');
        } else {
            res.status(404).send('플레이어 정보를 찾을 수 없습니다.');
            console.log('플레이어 정보를 찾을 수 없습니다.');
        }
    } catch (err) {
        console.error('데이터 업데이트 오류:', err);
        res.status(500).send('서버 오류');
    }
});

// 몬스터 정보 조회 엔드포인트
app.get('/api/monsters', async (req, res) => {
    try {
        const pool = await sql.connect(dbConfig);
        const result = await pool.request().query('SELECT * FROM Monsters');
        res.json(result.recordset);
    } catch (err) {
        console.error('데이터 조회 오류:', err);
        res.status(500).send('서버 오류');
    }
});

app.listen(port, () => {
    console.log(`서버가 http://localhost:${port} 에서 실행 중입니다`);
});