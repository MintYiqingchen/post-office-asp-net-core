#create database postoffice;
#create user postmaster identified by 'wangxiaoxi';
#grant all on postoffice.* to postmaster with grant option;
use postoffice;
CREATE TABLE `__EFMigrationsHistory` (
    `MigrationId` varchar(150) NOT NULL,
    `ProductVersion` varchar(32) NOT NULL,
    PRIMARY KEY (`MigrationId`)
);


CREATE TABLE `Movie` (
    `ID` int NOT NULL AUTO_INCREMENT,
    `Genre` text NULL,
    `Price` decimal(18, 2) NOT NULL,
    `ReleaseDate` datetime NOT NULL,
    `Title` text NULL,
	Img_url varchar(256) null,
    PRIMARY KEY (`ID`)
);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20180101070011_inittest', '2.0.1-rtm-125');

