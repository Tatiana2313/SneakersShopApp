-- Создание контейнера базы данных Sneakers
create database Sneakers
go

-- Активизация контейнера базы данных Sneakers
use Sneakers
go

--создание доменов--
create type cod from smallint
go

create type nume from varchar(50) not null
go

create type amount from smallint
go

--создаем родительские таблицы --
create table Providerr ( cod_provider cod primary key check(cod_provider > 0 and cod_provider < 21),
                        provider_name nume not null,
						addres varchar(70) not null,
						phone_fax char(12) not null,
						rascet_scet char(20) not null )
go

create table Kind_of_sport ( cod_kind_of_sport cod primary key check(cod_kind_of_sport > 10 and cod_kind_of_sport < 21),
                             name_sport nume )
go

--создаем родительско-дочерние таблицы--
create table Sneakers ( cod_sneakers cod primary key check(cod_sneakers > 1000 and cod_sneakers < 4000),
                        Sneakers_name nume,
						Vendor_code varchar(8) not null,
						brand varchar(20) not null,
						Producing_country nume,
						cod_kind_of_sport cod foreign key references Kind_of_sport(cod_kind_of_sport) check(cod_kind_of_sport > 10 and cod_kind_of_sport < 21) not null,
						material varchar(40) not null,
						gender char(1) check(gender ='F' or gender ='M') not null,
						size smallint check(size >= 30 and size <= 48) not null,
						color varchar(20) not null,
						unit_price decimal(7,2) not null,
						photo varchar(70) not null )
go

--создаем дочерние таблицы--
create table TTN ( num_doc cod check(num_doc > 500 and num_doc < 1000) not null,
                   date_post date not null,
				   cod_provider cod foreign key references Providerr(cod_provider) check(cod_provider > 0 and cod_provider < 21) not null,
				   cod_sneakers cod foreign key references Sneakers(cod_sneakers) check(cod_sneakers > 1000 and cod_sneakers < 2000) not null,
				   number_of_sneakers amount check(number_of_sneakers > 0) not null )
go

create table Chek ( check_number cod check(check_number > 200 and check_number < 1000) not null,
                    date_of_sale date default GETDATE(),
					sale_time time default convert(smalldatetime, GETDATE()),
					cod_sneakers cod foreign key references Sneakers(cod_sneakers) check(cod_sneakers > 1000 and cod_sneakers < 2000) not null,
					number_of_sneakers amount check(number_of_sneakers > 0) default 1 )
go


create table Jurnal_uceta ( month_name varchar(9) not null,
                            cod_sneakers cod foreign key references Sneakers(cod_sneakers) check(cod_sneakers > 1000 and cod_sneakers < 2000) not null,
							NumbeOfDeliveredSneakers amount check(NumbeOfDeliveredSneakers >= 0) not null,
							NumberOfSneakersSold amount check(NumberOfSneakersSold >= 0) not null )
go

create table Users ( id_user cod not null,
					 name_role varchar(7) check(name_role = 'admin' or name_role = 'user' or name_role = 'seller') not null,
					 login_user varchar(20) unique not null,
					 password_user char(60) not null )
go

--внесение информации--
insert into Providerr (cod_provider, provider_name, addres, phone_fax, rascet_scet)
values ( 1, 'Life', 'Puskina 45', '+37368190324', '20098129480200078012'),
       ( 2, 'SG', 'Decebal 12', '+37369002819', '10986789012387492910'),
	   ( 3, 'Only shoes', 'Lesova 19', '+37379080108', '90005770682071100910'),
	   ( 4, 'All in quality', 'Stefan Cel Mare 18', '+37342695812', '90000846127094546278'),
	   ( 5, 'GoaBay', 'Sarmigetuza 12', '+37333612978', '56789194351000490330'),
	   ( 6, 'Top Village', 'Dacia 28', '+37385647120', '10000028590004728190'),
	   ( 7, 'International TexTrade', 'Decebal 15/2', '+37379182634', '90000768930190304768'),
	   ( 8, 'Vesto Italiano Cros', 'Kannam 33', '+37379030292', '19628354607870090410'),
	   ( 9, 'Sports With Sneakers', 'Namsang 11', '+37368409870', '10203548795040546688'),
	   ( 10, 'Veila', 'Albisoara 11', '+37369870650', '30002718456319901220')
go

insert into Kind_of_sport (cod_kind_of_sport, name_sport)
values ( 11, 'Basketball'),
	   ( 12, 'Tennis'),
	   ( 13, 'Running'),
	   ( 14, 'Volleyball'),
	   ( 15, 'Football'),
	   ( 16, 'Fitness')
go

insert into Sneakers (cod_sneakers, Sneakers_name, Vendor_code, brand, Producing_country, cod_kind_of_sport, material, gender, size, color, unit_price, photo)
values ( 1301, 'Nike STAR RUNNER 3', 'W8192049', 'Nike', 'Vietnam', 13, 'synthetics, textiles', 'F', 40, 'blue', 1200.20, 'nike_star_runner3.jpg'),
       ( 1101, 'Nike RENEW ELEVATE III', 'M6120098', 'Nike', 'Great Britain', 11, 'synthetics, textiles', 'M', 45, 'black', 1400.89, 'NIKE_RENEW_ELEVATE_III.jpg'),
	   ( 1501, 'Demix GOAL FG', 'M7800910', 'Demix', 'China', 15, 'synthetic leather', 'M', 43, 'black-red', 990.29, 'Demix_GOAL_FG.jpg'),
	   ( 1601, 'FILA Krypton', 'W9100254', 'FILA', 'USA', 16, 'polyester, synthetic leather', 'F', 40, 'lilac', 1500.79, 'FILA_Krypton.jpg'),
	   ( 1201, 'Puma Mapf1 Rs-Fast Me', 'W0901654', 'Puma', 'Vietnam', 12, 'textiles, artificial leather', 'F', 39, 'black-blue', 3995.10, 'Puma_Mapf1_Rs-Fast_Me.jpg'),
	   ( 1401, 'Puma Rs-Fast Limiter BsiW', 'M1189538', 'Puma', 'Vietnam', 14, 'textiles, artificial leather', 'M', 43, 'black', 4000.00, 'Puma_Rs-Fast_Limiter_BsiW.jpg'),
	   ( 1102, 'JORDAN XXXV WIP', 'W8901267', 'JORDAN', 'Vietnam', 11, 'textiles, artificial leather', 'F', 40, 'blue', 6000.00, 'JORDAN_XXXV_WIP.png'),
	   ( 1103, 'Nike KD Trey 5 X', 'M7799001', 'Nike', 'France', 11, 'synthetics, textiles', 'M', 44, 'red and white', 6600.90, 'KD_TREY_5X.jpg'),
	   ( 1602, 'Reebok Flexagon Energy Train 3', 'M8870124', 'Reebok', 'Indonesia', 16, 'textiles, synthetics', 'M', 43, 'pink/white', 5500.10, 'Reebok_Flexagon_Energy_Train3.jpg'),
	   ( 1502, 'Adidas X Speedflow', 'W0190376', 'Adidas', 'Great Britain', 15, 'synthetic leather', 'F', 41, 'yellow', 6300.45, 'AdidasX_Speedflow.jpg')
go

insert into Sneakers (cod_sneakers, Sneakers_name, Vendor_code, brand, Producing_country, cod_kind_of_sport, material, gender, size, color, unit_price, photo)
values ( 1104, 'JORDAN DIAMOND LOW PF', 'M1209002', 'JORDAN', 'Vietnam', 11, 'textiles, artificial leather', 'M', 40, 'black', 2500.80, 'JORDAN_DIAMOND_LOW_PF.jpg')

insert into TTN (num_doc, date_post, cod_provider, cod_sneakers, number_of_sneakers)
values ( 501, '2023-09-20', 1, 1301, 25),
       ( 502, '2023-10-01', 2, 1101, 30),
	   ( 503, '2023-10-03', 3, 1501, 70),
	   ( 504, '2023-11-13', 4, 1601, 100),
	   ( 505, '2023-11-13', 5, 1201, 69),
	   ( 506, '2023-11-23', 6, 1401, 110),
	   ( 507, '2023-11-28', 7, 1102, 290),
	   ( 508, '2023-10-13', 8, 1103, 290),
	   ( 509, '2023-12-04', 9, 1602, 100),
	   ( 510, '2023-12-15', 10, 1502, 100),
	   ( 511, '2023-01-17', 4, 1104, 170)
go

insert into TTN (num_doc, date_post, cod_provider, cod_sneakers, number_of_sneakers)
values ( 512, '2023-11-13', 1, 1301, 30)

insert into Chek (check_number, date_of_sale, sale_time, cod_sneakers, number_of_sneakers)
values ( 201, '2023-09-26', '13:23', 1301, 1),
       ( 202, '2023-02-01', '12:10', 1101, 2),
	   ( 203, '2023-10-12', '15:00', 1102, 1),
	   ( 204, '2023-10-12', '17:00', 1301, 2),
	   ( 205, '2023-11-23', '13:25', 1201, 1),
	   ( 206, '2023-01-30', '14:00', 1102, 1),
	   ( 207, '2023-01-18', '11:30', 1602, 1),
	   ( 208, '2023-01-18', '18:45', 1103, 1),
	   ( 209, '2023-02-11', '12:25', 1502, 1),
	   ( 210, '2023-02-24', '13:50', 1103, 2)
go

insert into Chek (check_number, date_of_sale, sale_time, cod_sneakers, number_of_sneakers)
values ( 211, '2023-01-05', '11:28', 1101, 1),
	   ( 212, '2023-01-16', '17:12', 1502, 2),
	   ( 213, '2023-01-20', '18:00', 1602, 1)

insert into Jurnal_uceta (month_name, cod_sneakers, NumbeOfDeliveredSneakers, NumberOfSneakersSold)
values ( 'September', 1301, 25, 24),
       ( 'October', 1101, 30, 28),
	   ( 'October', 1501, 70, 69),
	   ( 'November', 1601, 100, 100),
	   ( 'November', 1201, 69, 69),
	   ( 'November', 1401, 110, 100),
	   ( 'November', 1102, 290, 200),
	   ( 'October', 1103, 290, 120),
	   ( 'December', 1602, 100, 90),
	   ( 'December', 1502, 100, 91),
	   ( 'January', 1102, 170, 1),
	   ( 'November', 1301, 30, 2)
go 

insert into Users (id_user, name_role, login_user, password_user)
values ( 100, 'admin', 'admin001', '$2a$12$xMqtHp23eF8PeaLzlZr04.PzHJwGMZkmwzrBOzWNpcXcbbBNDmSv2'),
	   ( 101, 'user', 'userRPWP', '$2a$12$SWrgMacJxFN4PUkJk/8P1.TXT7qOWEdpqVVLr5WNmt1T7Vb2kO3oK'),
	   ( 102, 'seller', 'MariaS', '$2a$12$fmZoA90/8BVc26xQghM48.OSR86yry03FP68myPHM62y09lYw1Y4i'),
	   ( 103, 'user', 'userIvan', '$2a$12$1gmmZsZFkeUbg0Ahfs4OnOW0noBcGvqKl0hwD5/ISiSea0pT0/liG')
go

select * from Jurnal_uceta

--создание индексов--
create index indx_sneakers on Sneakers(Sneakers_name)
go

create index indx_brand on Sneakers(brand)
go

create index indx_sport on Kind_of_sport(name_sport)
go

create index indx_provider on Providerr(provider_name)
go


--информация, которая может изменяться - это цена спортивных кроссовок--
UPDATE Sneakers SET unit_price = unit_price + 50.7
				Where  Sneakers_name = 'Nike STAR RUNNER 3'

select * From Sneakers
go

--представления--

GO
/*создаем представление таблиц Kind_of_sport и Sneakers для того, чтобы иметь полную информацию о спорт. кроссовках и эффективного поиска
  информации в запросах, в которых используются данные из этих таблиц*/
CREATE VIEW ViewSneakersInfo AS
SELECT cod_sneakers, Sneakers_name, Vendor_code, brand, Producing_country, name_sport, material, gender, size, color, unit_price, photo
FROM Kind_of_sport INNER JOIN Sneakers ON Kind_of_sport.cod_kind_of_sport = Sneakers.cod_kind_of_sport
GO

GO
/*создаем представление представления ViewSneakersInfo и таблицы Jurnal_uceta для того, чтобы иметь  
  информацию о спорт. кроссовках и их продаж.
  Также для быстрого и эффективного поиска информации в запросах, в которых используются данные из этих таблиц*/
CREATE VIEW ViewSneakersJurnal AS
SELECT month_name, Sneakers_name, brand, NumbeOfDeliveredSneakers, NumberOfSneakersSold, NumbeOfDeliveredSneakers - NumberOfSneakersSold AS remainder
FROM  ViewSneakersInfo INNER JOIN Jurnal_uceta ON ViewSneakersInfo.cod_sneakers = Jurnal_uceta.cod_sneakers
GO

GO
/*создаем представление представления ViewSneakersInfo и таблицы Chek для того, чтобы иметь полную информацию о покупке и  
  о спорт. кроссовках, которые купили. Также для быстрого и эффективного поиска информации
  в запросах, в которых используются данные из этих таблиц*/
CREATE VIEW ViewSneakersChek AS
SELECT check_number, date_of_sale, sale_time, Sneakers_name, brand, unit_price, number_of_sneakers, unit_price * number_of_sneakers AS total_Price
FROM ViewSneakersInfo INNER JOIN Chek ON ViewSneakersInfo.cod_sneakers = Chek.cod_sneakers
GO

SELECT * 
FROM ViewSneakersTTN

GO
/*создаем вспомогательное представление таблиц Providerr и TTN для того, чтобы соединить в одну таблицу отношения Providerr и Sneakers */
CREATE VIEW ViewProviderTTN AS
SELECT num_doc, date_post, provider_name, cod_sneakers, number_of_sneakers
FROM Providerr INNER JOIN TTN ON Providerr.cod_provider = TTN.cod_provider
GO

GO
/*создаем представление представлений ViewSneakersInfo и ViewProviderTTN для того, чтобы иметь полную информацию о поставках и  
  о спорт. кроссовках, которые поставили. Также для быстрого и эффективного поиска информации
  в запросах, в которых используются данные из этих таблиц*/
CREATE VIEW ViewSneakersTTN AS
SELECT num_doc, date_post, provider_name, Sneakers_name, brand, unit_price, number_of_sneakers, unit_price * number_of_sneakers AS total
FROM ViewSneakersInfo INNER JOIN ViewProviderTTN ON ViewSneakersInfo.cod_sneakers = ViewProviderTTN.cod_sneakers
GO


--запросы с применением различных типов соединений

--отобразить все покупки и включить те спорт. кроссовки, которые никто не покупал.
SELECT Sneakers_name, check_number, date_of_sale, sale_time, unit_price, number_of_sneakers
FROM ViewSneakersInfo LEFT OUTER JOIN Chek ON ViewSneakersInfo.cod_sneakers = Chek.cod_sneakers

--Вывести информацию о спорт. кроссовках, которые были в магазине, но их не купили
SELECT ViewSneakersInfo.Sneakers_name, ViewSneakersInfo.Vendor_code, ViewSneakersInfo.brand, ViewSneakersInfo.Producing_country, 
       ViewSneakersInfo.name_sport, ViewSneakersInfo.material, ViewSneakersInfo.gender, ViewSneakersInfo.size, ViewSneakersInfo.color, ViewSneakersInfo.unit_price
FROM ViewSneakersInfo LEFT OUTER JOIN ViewSneakersChek ON ViewSneakersInfo.Sneakers_name = ViewSneakersChek.Sneakers_name
WHERE ViewSneakersChek.Sneakers_name IS NULL

--вывести прибыль, которую получил магазин за каждый бренд спорт. кроссовок
SELECT brand, SUM(unit_price * number_of_sneakers) AS Earnings
FROM ViewSneakersChek
GROUP BY brand

--запросы на группировку и агрегацию информации

--вывести название вида спорта и количество кроссовок каждого вида 
SELECT name_sport, COUNT(*) AS CountOfSneakers
FROM ViewSneakersInfo
GROUP BY name_sport

--вывести средную  цену мужских кроссовок 
SELECT AVG(unit_price) AS AVGPrice
FROM ViewSneakersInfo
WHERE gender = 'M'

--вывести прибыль, которую получил магазин за '2023-01-18'
SELECT SUM(number_of_sneakers * unit_price) AS Earnings
FROM ViewSneakersChek
WHERE date_of_sale = '2023-01-18'

--вывести модель спорт. кроссовок, количество проданных единиц и общую сумму продаж для каждой модели спорт. кроссовок, проданных в период с 01-01-2023 по 28-02-2023
SELECT Sneakers_name, SUM(number_of_sneakers) AS sold_units, SUM(unit_price * number_of_sneakers) AS total_sales
FROM ViewSneakersChek
WHERE date_of_sale BETWEEN '2023-01-01' AND '2023-02-28'
GROUP BY Sneakers_name

--вывести количество моделей спорт. кроссовок в материале, которых есть 'textiles'
SELECT COUNT(cod_sneakers) AS CountSneakers
FROM ViewSneakersInfo
WHERE material LIKE '%textiles%' 

--подзапросы

--вывести модель самых дорогих кроссовок бренда Nike
SELECT brand, Sneakers_name
FROM Sneakers
WHERE unit_price = ( SELECT MAX(unit_price)
					 FROM Sneakers 
					 WHERE brand = 'Nike')

--вывести самую дешевую модель баскетбольных кроссовок 40 размера
SELECT Sneakers_name
FROM ViewSneakersInfo
WHERE unit_price = ( SELECT MIN(unit_price)
				     FROM ViewSneakersInfo
					 WHERE name_sport = 'Basketball' AND size = 40)

--вывести самую(ые) продаваемую(ые) модель(и) кроссовок
SELECT Sneakers_name, SUM(number_of_sneakers) AS TotalSales
FROM ViewSneakersChek
GROUP BY Sneakers_name
HAVING SUM(number_of_sneakers) = ( SELECT MAX(SneakersSales)
									 FROM 
										( SELECT SUM(number_of_sneakers) AS SneakersSales
										  FROM ViewSneakersChek
										  GROUP BY Sneakers_name ) AS Sales )

--Вывести информацию о  баскетбольных кроссовках, цена которых меньше, чем у любой модели кроссовок бренда 'JORDAN'
SELECT Sneakers_name, Vendor_code, brand, Producing_country, name_sport, material, gender, size, color, unit_price
FROM ViewSneakersInfo
WHERE name_sport = 'Basketball' AND unit_price < ALL ( SELECT unit_price
													   FROM ViewSneakersInfo
													   WHERE brand = 'JORDAN' )

--Вывести модели кроссовок, которые были поставлены '2022-11-13' и их цена меньше, чем средняя цена любых футбольных кроссовок
SELECT Sneakers_name
FROM ViewSneakersTTN
WHERE date_post = '2022-11-13' AND unit_price < ALL ( SELECT AVG(unit_price) AS sredn_unit_price
													  FROM ViewSneakersInfo
													  WHERE name_sport = 'Football' )

SELECT brand, SUM(total_Price) AS TotalProfit
FROM ViewSneakersChek
GROUP BY brand

--функции

--функция для генерации первичного ключа
GO
CREATE FUNCTION PrimIdProvider()
RETURNS SMALLINT AS
BEGIN
   DECLARE @LNIdProvider SMALLINT
   SET @LNIdProvider = (SELECT MAX(cod_provider) + 1 FROM Providerr)
   RETURN @LNIdProvider
END
GO

--функция для генерации первичного ключа
GO
CREATE FUNCTION PrimIdSneakers(@code_sport SMALLINT)
RETURNS SMALLINT AS
BEGIN
   DECLARE @LNIdSneakers SMALLINT,
		   @LNcountS SMALLINT
   SET @LNcountS = ( SELECT COUNT(cod_sneakers) FROM Sneakers
				     WHERE cod_kind_of_sport = @code_sport )
   SET @LNIdSneakers = @code_sport * 100 + @LNcountS + 1
   RETURN @LNIdSneakers
END
GO

--функция для генерации первичного ключа
GO
CREATE FUNCTION PrimIdSport()
RETURNS SMALLINT AS
BEGIN
   DECLARE @LNIdSport SMALLINT
   SET @LNIdSport = (SELECT MAX(cod_kind_of_sport) + 1 FROM Kind_of_sport)
   RETURN @LNIdSport
END
GO

--функция для генерации номера документа
GO
CREATE FUNCTION GenerateTTNNumDoc()
RETURNS SMALLINT AS
BEGIN
   DECLARE @LNNumDoc SMALLINT
   SET @LNNumDoc = (SELECT MAX(num_doc) + 1 FROM TTN)
   RETURN @LNNumDoc
END
GO

--функция для генерации номера чека
GO
CREATE FUNCTION GenerateCheckNum()
RETURNS SMALLINT AS
BEGIN
   DECLARE @LNCheckNum SMALLINT
   SET @LNCheckNum = (SELECT MAX(check_number) + 1 FROM Chek)
   RETURN @LNCheckNum
END
GO

--функция для генерации первичного ключа
GO
CREATE FUNCTION PrimIdUsers()
RETURNS SMALLINT AS
BEGIN
   DECLARE @LNIdUser SMALLINT
   SET @LNIdUser = (SELECT MAX(id_user) + 1 FROM Users)
   RETURN @LNIdUser
END
GO

-- процедуры для добавления информации
GO
CREATE PROCEDURE AddKindSport
   @name_sport varchar(50)
AS
BEGIN
  BEGIN TRY
    BEGIN TRANSACTION
      INSERT INTO Kind_of_sport OUTPUT INSERTED.cod_kind_of_sport VALUES (dbo.PrimIdSport(), @name_sport)
      COMMIT TRANSACTION
  END TRY
  BEGIN CATCH
    IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        THROW 50000, 'Не удалось добавить новую запись в таблицу.', 1;
  END CATCH
END
GO
drop procedure AddKindSport
EXEC AddKindSport 'gg'

GO
CREATE PROCEDURE AddSneakers
    @Sneakers_name varchar(50),
    @Vendor_code varchar(8),
    @brand varchar(20),
    @Producing_country varchar(50),
    @cod_kind_of_sport smallint,
    @material varchar(40),
    @gender char(1),
	@size smallint,
	@color varchar(20),
    @unit_price decimal(7, 2),
	@photo varchar(70)
AS
BEGIN
  BEGIN TRY
    BEGIN TRANSACTION
      INSERT INTO Sneakers OUTPUT INSERTED.cod_sneakers VALUES (dbo.PrimIdSneakers(@cod_kind_of_sport), @Sneakers_name, @Vendor_code, @brand, @Producing_country, 
								   @cod_kind_of_sport, @material, @gender, @size, @color, @unit_price, @photo)
      COMMIT TRANSACTION
  END TRY
  BEGIN CATCH
    IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        THROW 50000, 'Не удалось добавить новую запись в таблицу.', 1;
  END CATCH
END
GO

EXEC AddSneakers 'JORDAN DIAMOND LOW PF', 'M1209002', 'JORDAN', 'Vietnam', 11, 'textiles, artificial leather', 'M', 40, 'black', 2500.80, 'bb'

GO
CREATE PROCEDURE AddProviders
    @provider_name varchar(50),
    @addres varchar(70),
    @phone_fax char(12),
    @rascet_scet char(20)
AS
BEGIN
  BEGIN TRY
    BEGIN TRANSACTION
      INSERT INTO Providerr OUTPUT INSERTED.cod_provider VALUES (dbo.PrimIdProvider(), @provider_name, @addres, @phone_fax, @rascet_scet)
      COMMIT TRANSACTION
  END TRY
  BEGIN CATCH
    IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        THROW 50000, 'Не удалось добавить новую запись в таблицу.', 1;
  END CATCH
END
GO

GO
CREATE PROCEDURE AddTTN
    @date_post date,
    @cod_provider smallint,
    @cod_sneakers smallint,
    @number_of_sneakers smallint
AS
BEGIN
  BEGIN TRY
    BEGIN TRANSACTION
      INSERT INTO TTN VALUES (dbo.GenerateTTNNumDoc(), @date_post, @cod_provider, @cod_sneakers, @number_of_sneakers)
      COMMIT TRANSACTION
  END TRY
  BEGIN CATCH
    IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        THROW 50000, 'Не удалось добавить новую запись в таблицу.', 1;
  END CATCH
END
GO

GO
CREATE PROCEDURE AddCheck
    @cod_sneakers smallint,
    @number_of_sneakers smallint
AS
BEGIN
  BEGIN TRY
    BEGIN TRANSACTION
      INSERT INTO Chek(check_number, cod_sneakers, number_of_sneakers) VALUES (dbo.GenerateCheckNum(), @cod_sneakers, @number_of_sneakers)
      COMMIT TRANSACTION
  END TRY
  BEGIN CATCH
    IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        THROW 50000, 'Не удалось добавить новую запись в таблицу.', 1;
  END CATCH
END
GO
select * from Chek
exec AddCheck 1101, 1

GO
CREATE PROCEDURE AddJurnalInfo
    @month_name VARCHAR(20),
    @cod_sneakers SMALLINT,
    @NumbeOfDeliveredSneakers SMALLINT,
    @NumberOfSneakersSold SMALLINT
AS
BEGIN
  BEGIN TRY
    BEGIN TRANSACTION
      INSERT INTO Jurnal_uceta VALUES (@month_name, @cod_sneakers, @NumbeOfDeliveredSneakers, @NumberOfSneakersSold)
      COMMIT TRANSACTION
  END TRY
  BEGIN CATCH
    IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        THROW 50000, 'Не удалось добавить новую запись в таблицу.', 1;
  END CATCH
END
GO

GO
CREATE PROCEDURE AddUser
	@name_role VARCHAR(7),
    @login_user VARCHAR(20),
    @password_user VARCHAR(60)
AS
BEGIN
  BEGIN TRY
    BEGIN TRANSACTION
      INSERT INTO Users OUTPUT INSERTED.id_user VALUES (dbo.PrimIdUsers(), @name_role, @login_user, @password_user)
      COMMIT TRANSACTION;
  END TRY
  BEGIN CATCH
    IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        THROW 50000, 'Не удалось добавить нового пользователя.', 1;
  END CATCH
END
GO

-- процедуры для удаления информации
GO
CREATE PROCEDURE DeleteRecordSport
    @recordID SMALLINT
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;
        IF EXISTS (SELECT 1 FROM Kind_of_sport WHERE cod_kind_of_sport = @recordID)
		BEGIN
            DELETE FROM Kind_of_sport WHERE cod_kind_of_sport = @recordID
            COMMIT TRANSACTION;
        END
        ELSE
        BEGIN;
            THROW 50000, 'Запись не существует в базе данных', 1;
        END
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        THROW 50000, 'Нельзя удалить запись, так как она связана с другими таблицами.', 1;
    END CATCH
END
GO

GO
CREATE PROCEDURE DeleteRecordsProvider
    @recordID SMALLINT
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;
        IF EXISTS (SELECT 1 FROM Providerr WHERE cod_provider = @recordID)
		BEGIN
            DELETE FROM Providerr WHERE cod_provider = @recordID
            COMMIT TRANSACTION;
        END
        ELSE
        BEGIN;
            THROW 50000, 'Запись не существует в базе данных', 1;
        END
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        THROW 50000, 'Нельзя удалить запись, так как она связана с другими таблицами.', 1;
    END CATCH
END
GO

EXEC DeleteRecordsProvider @recordID = 1003;
select  * from Providerr

GO
CREATE PROCEDURE DeleteRecordsSneakers
    @recordID SMALLINT
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;
        IF EXISTS (SELECT 1 FROM Sneakers WHERE cod_sneakers = @recordID)
		BEGIN
            DELETE FROM Sneakers WHERE cod_sneakers = @recordID
            COMMIT TRANSACTION;
        END
        ELSE
        BEGIN;
            THROW 50000, 'Запись не существует в базе данных', 1;
        END
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        THROW 50000, 'Нельзя удалить запись, так как она связана с другими таблицами.', 1;
    END CATCH
END
GO

GO
CREATE PROCEDURE DeleteRecordsTTN
    @recordID SMALLINT
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;
        IF EXISTS (SELECT 1 FROM TTN WHERE num_doc = @recordID)
		BEGIN
            DELETE FROM TTN WHERE num_doc = @recordID
            COMMIT TRANSACTION;
        END
        ELSE
        BEGIN;
            THROW 50000, 'Запись не существует в базе данных', 1;
        END
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        THROW 50000, 'Нельзя удалить запись, так как она связана с другими таблицами.', 1;
    END CATCH
END
GO

GO
CREATE PROCEDURE DeleteRecordsCheck
    @recordID SMALLINT
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;
        IF EXISTS (SELECT 1 FROM Chek WHERE check_number = @recordID)
		BEGIN
            DELETE FROM Chek WHERE check_number = @recordID
            COMMIT TRANSACTION;
        END
        ELSE
        BEGIN;
            THROW 50000, 'Запись не существует в базе данных', 1;
        END
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        THROW 50000, 'Нельзя удалить запись, так как она связана с другими таблицами.', 1;
    END CATCH
END
GO

GO
CREATE PROCEDURE DeleteRecordsUcet
    @month varchar(20),
    @name varchar(50)
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;
        IF EXISTS (SELECT 1 FROM Jurnal_uceta WHERE month_name = @month AND cod_sneakers IN (SELECT cod_sneakers FROM Sneakers WHERE Sneakers_name = @name))
		BEGIN
            DELETE FROM Jurnal_uceta WHERE month_name = @month AND cod_sneakers IN (SELECT cod_sneakers FROM Sneakers WHERE Sneakers_name = @name)
            COMMIT TRANSACTION;
        END
        ELSE
        BEGIN;
            THROW 50000, 'Запись не существует в базе данных', 1;
        END
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        THROW 50000, 'Нельзя удалить запись, так как она связана с другими таблицами.', 1;
    END CATCH
END
GO

GO
CREATE PROCEDURE DeleteUser
    @UserID SMALLINT
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;
        IF EXISTS (SELECT 1 FROM Users WHERE id_user = @UserID)
    BEGIN
            DELETE FROM Users WHERE id_user = @UserID
            COMMIT TRANSACTION;
        END
        ELSE
        BEGIN;
            THROW 50000, 'Такого пользователя не существует в базе данных', 1;
        END
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        THROW 50000, 'Не удалось удалить пользователя.', 1;
    END CATCH
END
GO

-- процедуры для изменения информации
GO
CREATE PROCEDURE UpdateSport
    @cod_kind_of_sport SMALLINT,
    @name_sport varchar(50)
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;
        UPDATE Kind_of_sport
        SET name_sport = @name_sport
        WHERE cod_kind_of_sport = @cod_kind_of_sport

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        THROW 50000, 'Не удалось изменить информацию.', 1;
    END CATCH
END
GO

GO
CREATE PROCEDURE UpdateSneakers
    @cod_sneakers SMALLINT,
    @Sneakers_name varchar(50),
    @Vendor_code varchar(8),
    @brand varchar(20),
    @Producing_country varchar(50),
    @cod_kind_of_sport SMALLINT,
    @material varchar(40),
    @gender char(1),
	@size SMALLINT,
	@color varchar(20),
    @unit_price DECIMAL(7, 2),
	@photo varchar(70)
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;

        UPDATE Sneakers
        SET 
            Sneakers_name = @Sneakers_name,
            Vendor_code = @Vendor_code,
            brand = @brand,
            Producing_country = @Producing_country,
            cod_kind_of_sport = @cod_kind_of_sport,
            material = @material,
            gender = @gender,
			size = @size,
			color = @color,
            unit_price = @unit_price,
			photo = @photo
        WHERE
            cod_sneakers = @cod_sneakers

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        THROW 50000, 'Не удалось изменить информацию.', 1;
    END CATCH
END
GO

GO
CREATE PROCEDURE UpdateProvider
    @provider_cod SMALLINT,
    @provider_name VARCHAR(50),
    @addres VARCHAR(70),
    @phone_fax CHAR(12),
    @rascet_scet CHAR(20)
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;

        UPDATE Providerr
        SET 
            provider_name = @provider_name,
            addres = @addres,
            phone_fax = @phone_fax,
            rascet_scet = @rascet_scet
        WHERE
            cod_provider = @provider_cod

        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        THROW 50000, 'Не удалось изменить информацию.', 1;
    END CATCH
END
GO

GO
CREATE PROCEDURE UpdateTTN
    @num_doc SMALLINT,
    --@date_post DATE,
    @cod_provider SMALLINT,
    @cod_sneakers SMALLINT,
    @number_of_sneakers SMALLINT
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;
        UPDATE TTN
        SET 
            cod_provider = @cod_provider,
            cod_sneakers = @cod_sneakers,
            number_of_sneakers = @number_of_sneakers
        WHERE
            num_doc = @num_doc
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        THROW 50000, 'Не удалось изменить информацию.', 1;
    END CATCH
END
GO

GO
CREATE PROCEDURE UpdateCheck
    @check_number SMALLINT,
    @cod_sneakers SMALLINT,
    @number_of_sneakers SMALLINT
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;
        UPDATE Chek
        SET 
            cod_sneakers = @cod_sneakers,
            number_of_sneakers = @number_of_sneakers
        WHERE
            check_number = @check_number
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        THROW 50000, 'Не удалось изменить информацию.', 1;
    END CATCH
END
GO

GO
CREATE PROCEDURE UpdateJurnalUceta
    @month_name VARCHAR(9),
    @cod_sneakers SMALLINT,
    @NumbeOfDeliveredSneakers SMALLINT,
    @NumberOfSneakersSold SMALLINT
AS
BEGIN
    BEGIN TRY
        BEGIN TRANSACTION;
        UPDATE Jurnal_uceta
        SET 
            NumbeOfDeliveredSneakers = @NumbeOfDeliveredSneakers,
            NumberOfSneakersSold = @NumberOfSneakersSold
        WHERE
            month_name = @month_name AND cod_sneakers = @cod_sneakers
        COMMIT TRANSACTION;
    END TRY
    BEGIN CATCH
        IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        THROW 50000, 'Не удалось изменить информацию.', 1;
    END CATCH
END
GO

GO
CREATE PROCEDURE UpdateUser
	@id_user SMALLINT,
	@name_role VARCHAR(7),
    @login_user VARCHAR(20),
    @password_user VARCHAR(60)
AS
BEGIN
  BEGIN TRY
    BEGIN TRANSACTION
      UPDATE Users
        SET 
            name_role = @name_role,
            login_user = @login_user,
			password_user = @password_user
        WHERE id_user = @id_user
      COMMIT TRANSACTION;
  END TRY
  BEGIN CATCH
    IF @@TRANCOUNT > 0
            ROLLBACK TRANSACTION;
        THROW 50000, 'Не удалось добавить нового пользователя.', 1;
  END CATCH
END
GO

--триггеры

--триггер, который обновляет кол-во проданных кроссовок в таблице Jurnal_uceta после добавления новой записи в таблицу Chek
GO
CREATE TRIGGER UpdateSales
ON Chek
AFTER INSERT
AS
BEGIN
    DECLARE @month_name varchar(9)
    DECLARE @sold_sneakers smallint
    DECLARE @cod_sneakers smallint

    SELECT @cod_sneakers = cod_sneakers, @sold_sneakers = number_of_sneakers, @month_name = DATENAME(month, date_of_sale) 
	FROM inserted

    IF EXISTS (SELECT 1
               FROM Jurnal_uceta
               WHERE cod_sneakers = @cod_sneakers AND month_name = @month_name AND (NumbeOfDeliveredSneakers <= @sold_sneakers OR NumbeOfDeliveredSneakers = NumberOfSneakersSold OR @sold_sneakers + NumberOfSneakersSold > NumbeOfDeliveredSneakers))
    BEGIN
        ROLLBACK TRANSACTION;
        THROW 51000, 'Произошла ошибка! Не удается купить столько кроссовок', 1;
    END
    ELSE
    BEGIN
        UPDATE Jurnal_uceta
        SET NumberOfSneakersSold = NumberOfSneakersSold + @sold_sneakers
        WHERE cod_sneakers = @cod_sneakers AND month_name = @month_name
    END
END
GO

--триггер, который обновляет кол-во поставленных кроссовок в таблице Jurnal_uceta после добавления новой записи в таблицу TTN
GO
CREATE TRIGGER UpdateDeliveries
ON TTN
AFTER INSERT
AS
BEGIN
    DECLARE @month_name varchar(9)
    DECLARE @delivered_sneakers smallint
    DECLARE @cod_sneakers smallint

    SELECT @cod_sneakers = cod_sneakers, @delivered_sneakers = number_of_sneakers, @month_name = DATENAME(month, date_post) 
	FROM inserted

    UPDATE Jurnal_uceta
    SET NumbeOfDeliveredSneakers = NumbeOfDeliveredSneakers + @delivered_sneakers
    WHERE cod_sneakers = @cod_sneakers AND month_name = @month_name
END
GO

--триггер, который обновляет кол-во проданных кроссовок в таблице Jurnal_uceta после удаления записи в таблице Chek
GO
CREATE TRIGGER DecreaseSales
ON Chek
AFTER DELETE
AS
BEGIN
    DECLARE @month_name varchar(9)
    DECLARE @sold_sneakers smallint
    DECLARE @cod_sneakers smallint

    SELECT @cod_sneakers = cod_sneakers, @sold_sneakers = number_of_sneakers, @month_name = DATENAME(month, date_of_sale) 
	FROM deleted

    UPDATE Jurnal_uceta
    SET NumberOfSneakersSold = NumberOfSneakersSold - @sold_sneakers
    WHERE cod_sneakers = @cod_sneakers AND month_name = @month_name;
END
GO

