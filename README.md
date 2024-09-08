# CoindeskApi
面試作業



create schema coindesk
go

create table Currency
(
Id   int identity,
Code varchar(3) not null,
Lang varchar(10) not null,
CurrencyName nvarchar(10)
)
go
