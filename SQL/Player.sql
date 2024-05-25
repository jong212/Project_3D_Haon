CREATE TABLE Users (
    UserID INT PRIMARY KEY IDENTITY(1,1),
    Username NVARCHAR(50) NOT NULL UNIQUE,
    PasswordHash NVARCHAR(256) NOT NULL,
    PlayerName NVARCHAR(50) NOT NULL
);

CREATE TABLE Players (
    PlayerID INT PRIMARY KEY IDENTITY(1,1),
    UserID INT NOT NULL FOREIGN KEY REFERENCES Users(UserID),
    PlayerName NVARCHAR(50) NOT NULL,
    Gems INT DEFAULT 0,
    Coins INT DEFAULT 0,
    MaxHealth INT DEFAULT 500 NOT NULL,
    HealthEnhancement INT DEFAULT 0,
    AttackPower INT DEFAULT 40 NOT NULL,
    AttackEnhancement INT DEFAULT 0,
    WeaponEnhancement INT DEFAULT 0,
    ArmorEnhancement INT DEFAULT 0
);

CREATE TABLE Monsters (
    MonsterID INT PRIMARY KEY IDENTITY(1,1),
    Name NVARCHAR(50) NOT NULL,
    Health INT NOT NULL,
    AttackPower INT NOT NULL
);
