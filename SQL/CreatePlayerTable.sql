CREATE TABLE Players (
    PlayerID INT PRIMARY KEY IDENTITY(1,1),
    Username NVARCHAR(50) NOT NULL,
    Gems INT DEFAULT 0,
    Coins INT DEFAULT 0
);

CREATE TABLE PlayerStats (
    PlayerID INT PRIMARY KEY,
    MaxHealth INT DEFAULT 500 NOT NULL,
    MaxHealthEnhancement INT DEFAULT 0,
    AttackPower INT DEFAULT 40 NOT NULL,
    AttackPowerEnhancement INT DEFAULT 0,
    WeaponEnhancement INT DEFAULT 0,
    ArmorEnhancement INT DEFAULT 0,
    FOREIGN KEY (PlayerID) REFERENCES Players(PlayerID)
);
