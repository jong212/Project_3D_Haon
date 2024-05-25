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
            .input('InsertGems', sql.Int, 0)
            .input('InsertCoins', sql.Int, 0)
            .input('InsertMaxHealth', sql.Int, 500)
            .input('InsertHealthEnhancement', sql.Int, 0)
            .input('InsertAttackPower', sql.Int, 40)
            .input('InsertAttackEnhancement', sql.Int, 0)
            .input('InsertWeaponEnhancement', sql.Int, 0)
            .input('InsertArmorEnhancement', sql.Int, 0)
            .query(`
        INSERT INTO Players (UserID, PlayerName, Gems, Coins, MaxHealth, HealthEnhancement, AttackPower, AttackEnhancement, WeaponEnhancement, ArmorEnhancement)
        VALUES (@InsertNewUserID, @InsertPlayerNamePlayer, @InsertGems, @InsertCoins, @InsertMaxHealth, @InsertHealthEnhancement, @InsertAttackPower, @InsertAttackEnhancement, @InsertWeaponEnhancement, @InsertArmorEnhancement)
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
            .query('SELECT UserID, Username, PasswordHash FROM Users WHERE Username = @LoginUsername');

        if (userResult.recordset.length === 0) {
            return res.status(404).json({ message: 'Invalid username or password' });
        }

        const user = userResult.recordset[0];

        // 비밀번호 확인
        const isPasswordValid = await bcrypt.compare(Password, user.PasswordHash);
        if (!isPasswordValid) {
            return res.status(404).json({ message: 'Invalid username or password' });
        }

        res.status(200).json({ message: 'Login successful', userID: user.UserID });
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

// 플레이어 정보 업데이트 엔드포인트
app.put('/api/players/:id', async (req, res) => {
    try {
        const { id } = req.params;
        const { PlayerName, Gems, Coins, MaxHealth, HealthEnhancement, AttackPower, AttackEnhancement, WeaponEnhancement, ArmorEnhancement } = req.body;
        const pool = await sql.connect(dbConfig);

        await pool.request()
            .input('UpdatePlayerID', sql.Int, id)
            .input('UpdatePlayerName', sql.NVarChar, PlayerName) // 파라미터 이름 변경
            .input('UpdateGems', sql.Int, Gems) // 파라미터 이름 변경
            .input('UpdateCoins', sql.Int, Coins) // 파라미터 이름 변경
            .input('UpdateMaxHealth', sql.Int, MaxHealth) // 파라미터 이름 변경
            .input('UpdateHealthEnhancement', sql.Int, HealthEnhancement) // 파라미터 이름 변경
            .input('UpdateAttackPower', sql.Int, AttackPower) // 파라미터 이름 변경
            .input('UpdateAttackEnhancement', sql.Int, AttackEnhancement) // 파라미터 이름 변경
            .input('UpdateWeaponEnhancement', sql.Int, WeaponEnhancement) // 파라미터 이름 변경
            .input('UpdateArmorEnhancement', sql.Int, ArmorEnhancement) // 파라미터 이름 변경
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

        res.status(200).send('플레이어 정보 업데이트 성공');
    } catch (err) {
        console.error('데이터 업데이트 오류:', err);
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