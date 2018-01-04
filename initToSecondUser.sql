
CREATE TABLE `AspNetRoles` (
    `Id` varchar(100) NOT NULL,
    `ConcurrencyStamp` text NULL,
    `Name` varchar(100) NULL,
    `NormalizedName` varchar(100) NULL,
    PRIMARY KEY (`Id`)
);

CREATE TABLE `AspNetUsers` (
    `Id` varchar(100) NOT NULL,
    `AccessFailedCount` int NOT NULL,
    `ConcurrencyStamp` text NULL,
    `Email` varchar(100) NULL,
    `EmailConfirmed` bit NOT NULL,
    `LockoutEnabled` bit NOT NULL,
    `LockoutEnd` timestamp NULL,
    `NormalizedEmail` varchar(100) NULL,
    `NormalizedUserName` varchar(100) NULL,
    `PasswordHash` text NULL,
    `PhoneNumber` text NULL,
    `PhoneNumberConfirmed` bit NOT NULL,
    `SecurityStamp` text NULL,
    `TwoFactorEnabled` bit NOT NULL,
    `UserName` varchar(256) NULL,
    PRIMARY KEY (`Id`)
);

CREATE TABLE `AspNetRoleClaims` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `ClaimType` text NULL,
    `ClaimValue` text NULL,
    `RoleId` varchar(100) NOT NULL,
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_AspNetRoleClaims_AspNetRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `AspNetRoles` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `AspNetUserClaims` (
    `Id` int NOT NULL AUTO_INCREMENT,
    `ClaimType` text NULL,
    `ClaimValue` text NULL,
    `UserId` varchar(100) NOT NULL,
    PRIMARY KEY (`Id`),
    CONSTRAINT `FK_AspNetUserClaims_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `AspNetUserLogins` (
    `LoginProvider` varchar(100) NOT NULL,
    `ProviderKey` varchar(256) NOT NULL,
    `ProviderDisplayName` text NULL,
    `UserId` varchar(100) NOT NULL,
    PRIMARY KEY (`LoginProvider`, `ProviderKey`),
    CONSTRAINT `FK_AspNetUserLogins_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `AspNetUserRoles` (
    `UserId` varchar(100) NOT NULL,
    `RoleId` varchar(100) NOT NULL,
    PRIMARY KEY (`UserId`, `RoleId`),
    CONSTRAINT `FK_AspNetUserRoles_AspNetRoles_RoleId` FOREIGN KEY (`RoleId`) REFERENCES `AspNetRoles` (`Id`) ON DELETE CASCADE,
    CONSTRAINT `FK_AspNetUserRoles_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
);

CREATE TABLE `AspNetUserTokens` (
    `UserId` varchar(100) NOT NULL,
    `LoginProvider` varchar(200) NOT NULL,
    `Name` varchar(100) NOT NULL,
    `Value` text NULL,
    PRIMARY KEY (`UserId`, `LoginProvider`, `Name`),
    CONSTRAINT `FK_AspNetUserTokens_AspNetUsers_UserId` FOREIGN KEY (`UserId`) REFERENCES `AspNetUsers` (`Id`) ON DELETE CASCADE
);

CREATE INDEX `IX_AspNetRoleClaims_RoleId` ON AspNetRoleClaims (`RoleId`);

CREATE UNIQUE INDEX `RoleNameIndex` ON AspNetRoles (`NormalizedName`);

CREATE INDEX `IX_AspNetUserClaims_UserId` ON AspNetUserClaims (`UserId`);

CREATE INDEX `IX_AspNetUserLogins_UserId` ON AspNetUserLogins (`UserId`);

CREATE INDEX `IX_AspNetUserRoles_RoleId` ON AspNetUserRoles (`RoleId`);

CREATE INDEX `EmailIndex` ON AspNetUsers (`NormalizedEmail`);

CREATE UNIQUE INDEX `UserNameIndex` ON AspNetUsers (`NormalizedUserName`);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20180101083440_secondUser', '2.0.1-rtm-125');

