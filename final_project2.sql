#create schema postoffice;
use postoffice;
/*
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
    PRIMARY KEY (`ID`)
);

INSERT INTO `__EFMigrationsHistory` (`MigrationId`, `ProductVersion`)
VALUES ('20180101070011_inittest', '2.0.1-rtm-125');


drop table if exists AspNetRoles;
CREATE TABLE `AspNetRoles` (
    `Id` varchar(100) NOT NULL,
    `ConcurrencyStamp` text NULL,
    `Name` varchar(100) NULL,
    `NormalizedName` varchar(100) NULL,
    PRIMARY KEY (`Id`)
);

drop table if exists AspNetUsers;
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

*/

drop table if exists `Newspaper`;
create table Newspaper(
	#报纸编号
	pno int,
    #报纸发行号
    pno_number varchar(20) primary key,
	#报纸名称
	pna varchar(100) not null,
	#报纸单价 每一年
	ppr float not null,
	#出版单位
	pdw varchar(50),
	#发刊类型 #1 日刊 2 周刊 3 半月刊 4 月刊 5 季刊 6 年刊 7 旬刊 8 双月刊
	ptype varchar(10),
	#订单量
	total_sell_out int default 0,
    #报纸类型 报纸 null 1 商业/财经/营销 2 教育/育儿/外语 3 建筑/地理/园林 
    #4 时尚/娱乐/服饰 5 文体/艺术/广告 6 旅游/摄影/数码 7 时政/军事/咨讯
    #8 汽车/生活/百科 9 医学/美食/酒店 10 电影/动漫/影视 11 科技/会展/互联网
    labels varchar(20)
)engine = InnoDB;

drop table if exists `customer`;
create table customer(
	#用户编码 
	gno varchar(100) unique references aspnetroles(UserId),
	#用户姓名
	gna varchar(20) not null,
	#用户电话 固定为11位 
	gte char(11) not null,
	#用户地址
	gad varchar(50),
	#邮政编码 固定为6位 
	gpo char(6)
)engine = InnoDB;

drop table if exists `can_update_address`;
create table can_update_address(
	#用户编码
    gno varchar(50) not null primary key
)engine = InnoDB;

drop table if exists `orders`;
create table orders(
	#订单编号 
	onumber int auto_increment primary key,
	#订报编号 
	ona varchar(10),
	foreign key(ona) references newspaper(pno_number),
	#订报数量 
	ofen int not null,#完整性约束 
	#订报人编号 
	opeople varchar(50),
	foreign key(opeople) references customer(gno),
	#订报地址 
	oaddress varchar(200) not null,
	#订单日期 
	ostart_year int not null,#完整性约束 >= 现在的年份 
	
	#订单价格 
	oprice float not null,#完整性约束 
	#订单有效时间 
	last_time int not null,#完整性约束 持续年数 
	#支付方式 
	payway int,
	#是否已支付 
	boolpay bool
)auto_increment = 1 engine = InnoDB;

#建立索引，方便查找
create unique index newspaper_the_number on newspaper(pno);
create unique index customer_number on customer(gno);
create unique index order_number on orders(ona);
create unique index newspaper_public_number on newspaper(pno_number);
create index newspaper_labels on newspaper(labels);

#建立发刊类型验证函数，验证为1-8的类型 
drop function if exists `newspaper_type_islegal`;
/*
delimiter //
create function `newspaper_type_islegal`(newspaper_type int) returns int
begin
	declare result int default 0;
    if (newspaper_type >= 1 and newspaper_type <= 8) then 
		return 1;
	else
		return 0;
	end if;
end//
delimiter ;
*/
#建立报纸类型验证函数，验证为0-11的类型 
drop function if exists `newspaper_type2_islegal`;
/*
delimiter //
create function `newspaper_type2_islegal`(newspaper_labels int) returns int
begin
	declare result int default 0;
    if (newspaper_labels >= 0 and newspaper_labels <= 11) then 
		return 1;
	else
		return 0;
	end if;
end//
delimiter ;
*/
#建立根据编号查找的函数，返回报刊的名称 
drop function if exists `seek_for_name`;
delimiter //
create function `seek_for_name` (newspaper_number int) returns varchar(50)
begin
	declare result varchar(50);
	select newspaper.pna into result
	from newspaper
	where newspaper.pno = newspaper_number;
    return result;
end//
delimiter ;

#建立用户电话验证函数，验证为11位且每一位都是数字 
drop function if exists `user_phone_islegal`;
delimiter //
create function `user_phone_islegal`(str char(11))returns int
begin
	declare result int default 0;
    if isnull(str) then
		return 0;
    end if;
    select str regexp '^[0-9]*$' into result;
    if (result = 1) then 
		return 1;
    else
		return 0;
    end if;
end//
delimiter ;

#建立用户邮编验证函数，验证为6位且每一位都是数字 
drop function if exists `user_post_islegal`;
delimiter //
create function `user_post_islegal`(str char(6))returns int
begin
	declare result int default 0;
    if isnull(str) then
		return 0;
    end if;
    select str regexp '^[0-9]*$' into result;
    if (result = 1) then 
		return 1;
    else
		return 0;
    end if;
end//
delimiter ;

#建立触发器，每次添加一个订单且支付完成，对应报刊的订阅总量修改 
drop trigger if exists auto_add;
delimiter //
create trigger auto_add after insert on orders
for each row
begin
	update newspaper set total_sell_out = total_sell_out + new.ofen
    where newspaper.pno = new.ona and new.boolpay = 1;
end//
delimiter ;

#建立验证函数 验证月份是否合法 
drop function if exists `month_islegal`;
/*
delimiter //
create function `month_islegal`(month int) returns int
begin
	declare result int default 0;
	if(month >= 1 and month <= 12) then
		return 1;
	end if;
	return 0;
end//
delimiter ;
*/
#建立验证函数 验证持续时间是否合法 
drop function if exists `last_time_islegal`;

delimiter //
create function `last_time_islegal`(last_time int) returns int
begin
	declare result int default 0;
	if(last_time >= 1 and last_time <= 3) then
		return 1;
	end if;
	return 0;
end//
delimiter ;

#建立验证函数 验证开始年份是否合法 
drop function if exists `start_year_islegal`;
delimiter //
create function `start_year_islegal`(start_year int) returns int
begin
	declare result int default 0;
	if(start_year >= 2018) then
		return 1;
	end if;
	return 0;
end//
delimiter ;

#建立验证函数 验证订报数量是否合法 
drop function if exists `ofen_islegal`;
delimiter //
create function `ofen_islegal`(orders_ofen int) returns int
begin
	if(orders_ofen >= 1 and orders_often <= 1000) then
		return 1;
	end if;
    return 0;
end//
delimiter ;

#建立函数，自动计算订单的价格 
drop function if exists `return_orders_price`;
delimiter //
create function `return_orders_price`(newspaper_number varchar(50), last_time int) returns float
begin
	declare result float default 0;
	#先查找报纸的类型和单价 
	declare newspaper_type int default 0;
	declare newspaper_price float default 0;
	select newspaper.ptype, newspaper.ppr into newspaper_type,  newspaper_price
	from newspaper
	where newspaper.pna = newspaper_number;
    /*
	if(newspaper_type = 1) then#日刊
		set result = last_time * 30 * newspaper_price;
		return result;
	end if;
	if(newspaper_type = 2) then#周刊
		set result = last_time * 4 * newspaper_price;
		return result;
	end if;
	if(newspaper_type = 3) then#半月刊
		set result = last_time * 2 * newspaper_price;
		return result;
	end if;
	if(newspaper_type = 4) then#月刊
		set result = last_time * newspaper_price;
		return result;
	end if;
	if(newspaper_type = 5) then#季刊
		set result = floor(last_time / 3) * newspaper_price;
		return result;
	end if;
	if(newspaper_type = 6) then#年刊
		set result = floor(last_time / 12) * newspaper_price;
		return result;
	end if;
    if(newspaper_type = 7) then#旬刊 
		set result = last_time * 3 * newspaper_price;
        return result;
	end if;
    if(newspaper_type = 8) then #双月刊
		set result = last_time * 2 * newspaper_price;
        return result;
        */
	set result = newspaper_price * last_time;
    return result;
end//
delimiter ;

#建立触发器，每次增加、修改报纸清单，检查发刊类型、报刊类型是否合法 
drop trigger if exists check_newspaper1;
/*
delimiter //
create trigger check_newspaper1 after insert on newspaper
for each row
begin
	declare type_islegal int default 0;
    declare type_islegal2 int default 0;
    set type_islegal = newspaper_type_islegal(new.ptype);
    set type_islegal2 = newspaper_type2_islegal(new.labels);
    if(type_islegal = 0 or type_islegal2 = 0) then#illegal
		delete from newspaper where newspaper.pno = new.pno;
	end if;
end//
delimiter ;
*/

drop trigger if exists check_newspaper2;
/*
delimiter //
create trigger check_newspaper2 before update on newspaper
for each row
begin
	declare type_islegal int default 0;
	declare type_islegal2 int default 0;
    set type_islegal = newspaper_type_islegal(new.ptype);
    set type_islegal2 = newspaper_type2_islegal(new.labels);
    if(type_islegal = 0) then#illegal
		set new.pno = old.pno;
        set new.pna = old.pna;
        set new.ppr = old.ppr;
        set new.pdw = old.pdw;
        set new.ptype = old.ptype;
        set new.total_sell_out = old.total_sell_out;
        set new.labels = old.labels;
        set new.disciption = old.disciption;
        set new.pno_number = old.pno_number;
        set new.pmi = old.pmi;
	end if;
end//
delimiter ;
*/

#建立触发器，每次增加、修改用户清单，检查用户的电话、邮编
drop trigger if exists check_customer1;
delimiter //
create trigger check_customer1 after insert on customer
for each row
begin
	declare telephone_islegal int default 0;
    declare post_islegal int default 0;
    set telephone_islegal = user_phone_islegal(new.gte);
    set post_islegal = user_post_islegal(new.gpo);
    if(telephone_islegal = 0 or post_islegal = 0) then
		delete from customer where customer.gno = new.gno;
	end if;
end//
delimiter ;

drop trigger if exists check_customer2;
delimiter //
create trigger check_customer2 before update on customer
for each row
begin
	declare telephone_islegal int default 0;
    declare post_islegal int default 0;
    set telephone_islegal = user_phone_islegal(new.gte);
    set post_islegal = user_post_islegal(new.gpo);
    if(telephone_islegal = 0 or post_islegal = 0) then
		set new.gno = old.gno;
		set new.gna = old.gna;
        set new.gte = old.gte;
        set new.gad = old.gad;
        set new.gpo = old.gpo;
	end if;
end//
delimiter ;

#建立触发器，每次增加、修改订单，检查年份、月份、持续时间、订购份数是否合法 
drop trigger if exists check_orders1;
delimiter //
create trigger check_orders1 after insert on orders
for each row
begin
	#declare check_month int default 0;
    declare check_last_time int default 0;
    declare check_start_year int default 0;
    declare check_ofen int default 0;
    #set check_month = month_islegal(new.ostart_month);
    set check_last_time = last_time_islegal(new.last_time);
    set check_start_year = start_year_islegal(new.ostart_year);
    set check_ofen = ofen_islegal(new.ofen);
    if(check_last_time = 0 or check_start_year = 0 or check_ofen = 0) then
		delete from orders where orders.onumber = new.onumber;
    end if;
end//
delimiter ;

drop trigger if exists check_orders2;
delimiter //
create trigger check_orders2 before update on orders
for each row
begin
	#declare check_month int default 0;
    declare check_last_time int default 0;
    declare check_start_year int default 0;
    declare check_ofen int default 0;
    #set check_month = month_islegal(new.ostart_month);
    set check_last_time = last_time_islegal(new.last_time);
    set check_start_year = start_year_islegal(new.ostart_year);
    set check_ofen = ofen_islegal(new.ofen);
    if(check_last_time = 0 or check_start_year = 0 or check_ofen = 0) then
		set new.onumber = old.onumber;
        set new.ona = old.ona;
        set new.ofen = old.ofen;
        set new.opeople = old.opeople;
        set new.oaddress = old.oaddress;
        set new.ostart_year = old.ostart_year;
        #set new.ostart_month = old.ostart_month;
        set new.oprice = old.oprice;
        set new.last_time = old.last_time;
        set new.payway = old.payway;
        set new.boolpay = old.boolpay;
        
    end if;
end//
delimiter ;

drop view if exists all_of_newspaper;
#创建视图 显示所有的报纸信息 
/*
create view all_of_newspaper(pno, pno_number, pna, ppr, pdw, ptype, total_sell_out, labels, disciption, pmi)
as select *
from newspaper;
*/
drop view if exists all_of_orders;
#创建视图 显示所有的订单信息 
/*
create view all_of_orders(onumber, ona, ofen, opeople, oaddress, ostart_year, ostart_month, oprice, last_time, pay_way, boolpay)
as select *
from orders;
*/

