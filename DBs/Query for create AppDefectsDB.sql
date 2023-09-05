use master

CREATE DATABASE AppDefectsDB

--=USE и таблицу создавать только после первых двух строк=--
USE AppDefectsDB
CREATE TABLE AppDefects
(
    Id int PRIMARY KEY IDENTITY(1,1),
    Description NVARCHAR(250) NOT NULL,
    CreatedDate DATETIME  NOT NULL,
    CreatedBy NVARCHAR(50)  NOT NULL,
    CompanyName NVARCHAR(50)  NOT NULL,
    Status BIT  NOT NULL,
);