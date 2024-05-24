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
    const { Username, Password } = req.body;

    await transaction.begin();

    const request = new sql.Request(transaction);

    // 사용자 아이디 중복 확인
    const userCheck = await request
      .input('Username', sql.NVarChar, Username)
      .query('SELECT COUNT(*) as count FROM Users WHERE Username = @Username');
    
    if (userCheck.recordset[0].count > 0) {
      await transaction.rollback();
      return res.status(400).json({ message: 'Username already exists' });
    }

    // 비밀번호 해싱
    const hashedPassword = await bcrypt.hash(Password, 10);

    // Users 테이블에 사용자 정보 삽입
    const userResult = await request
      .input('Username', sql.NVarChar, Username)
      .input('PasswordHash', sql.NVarChar, hashedPassword)
      .query('INSERT INTO Users (Username, PasswordHash) VALUES (@Username, @PasswordHash); SELECT SCOPE_IDENTITY() AS UserID');
    
    const userId = userResult.recordset[0].UserID;

    // Players 테이블에 사용자 정보 삽입
    await request
      .input('UserID', sql.Int, userId)
      .input('PlayerName', sql.NVarChar, Username) // 기본적으로 Username을 PlayerName으로 사용
      .input('Gems', sql.Int, 0) // 기본값 설정
      .input('Coins', sql.Int, 0) // 기본값 설정
      .input('MaxHealth', sql.Int, 500) // 기본값 설정
      .input('HealthEnhancement', sql.Int, 0) // 기본값 설정
      .input('AttackPower', sql.Int, 40) // 기본값 설정
      .input('AttackEnhancement', sql.Int, 0) // 기본값 설정
      .input('WeaponEnhancement', sql.Int, 0) // 기본값 설정
      .input('ArmorEnhancement', sql.Int, 0) // 기본값 설정
      .query(`
        INSERT INTO Players (UserID, PlayerName, Gems, Coins, MaxHealth, HealthEnhancement, AttackPower, AttackEnhancement, WeaponEnhancement, ArmorEnhancement)
        VALUES (@UserID, @PlayerName, @Gems, @Coins, @MaxHealth, @HealthEnhancement, @AttackPower, @AttackEnhancement, @WeaponEnhancement, @ArmorEnhancement)
      `);

    // 트랜잭션 커밋
    await transaction.commit();

    // 새로 생성된 사용자 정보 반환
    const newUser = await request
      .input('UserID', sql.Int, userId)
      .query('SELECT UserID, Username FROM Users WHERE UserID = @UserID');

    res.status(201).json(newUser.recordset[0]);
  } catch (err) {
    await transaction.rollback();
    console.error('회원가입 오류:', err);
    res.status(500).send('서버 오류');
  } finally {
    transaction.release();
  }
});

app.post('/api/login', async (req, res) => {
  try {
    const { Username, Password } = req.body;
    const pool = await sql.connect(dbConfig);

    // 사용자 정보 조회
    const userResult = await pool.request()
      .input('Username', sql.NVarChar, Username)
      .query('SELECT UserID, Username, PasswordHash FROM Users WHERE Username = @Username');
    
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
      .input('UserID', sql.Int, id)
      .query('SELECT UserID, Username FROM Users WHERE UserID = @UserID');
    
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
      .input('id', sql.Int, id)
      .query('SELECT * FROM Players WHERE PlayerID = @id');
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
      .input('PlayerID', sql.Int, id)
      .input('PlayerName', sql.NVarChar, PlayerName)
      .input('Gems', sql.Int, Gems)
      .input('Coins', sql.Int, Coins)
      .input('MaxHealth', sql.Int, MaxHealth)
      .input('HealthEnhancement', sql.Int, HealthEnhancement)
      .input('AttackPower', sql.Int, AttackPower)
      .input('AttackEnhancement', sql.Int, AttackEnhancement)
      .input('WeaponEnhancement', sql.Int, WeaponEnhancement)
      .input('ArmorEnhancement', sql.Int, ArmorEnhancement)
      .query(`
        UPDATE Players SET 
          PlayerName = @PlayerName,
          Gems = @Gems,
          Coins = @Coins,
          MaxHealth = @MaxHealth,
          HealthEnhancement = @HealthEnhancement,
          AttackPower = @AttackPower,
          AttackEnhancement = @AttackEnhancement,
          WeaponEnhancement = @WeaponEnhancement,
          ArmorEnhancement = @ArmorEnhancement
        WHERE PlayerID = @PlayerID
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