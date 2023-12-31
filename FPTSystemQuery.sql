
use FPTSystem
go

create table AccountDB
(accID int primary key identity(1,1) not null,
username nvarchar(100) not null unique,
password nvarchar(500) not null
)

create table PermitDB
(perID int primary key identity(1,1) not null,
type_acc nvarchar(50) not null
)

create table InfoDetailsDB
(detailsID int primary key identity(1,1) not null,
date_birthday varchar(10) default '1990-01-01',
workplace nvarchar(200) default '',
fullname nvarchar(200) not null,
job nvarchar(200) default '',
telephone varchar(12) default null,
email nvarchar(200) not null unique,
toeic_score varchar(200) default '5.0',
ex_or_in varchar(200),
main_pr_lg nvarchar(200),
location nvarchar(200) default 'Vietnam'
)

create table InfoAccDB
(infoID int primary key identity(1,1) not null,
accID int unique not null,
perID int unique not null,
detailsID int unique not null,
couD_ID int default null
)
go

--Insert

insert into AccountDB(username, password) values ('admin', '202CB962AC59075B964B07152D234B70') --(SELECT CONVERT(NVARCHAR(32),HashBytes('MD5', 'email@dot.com'),2))
insert into PermitDB(type_acc) values ('admin')
insert into InfoDetailsDB(fullname, telephone, email) values ('Trung Nhan', '03578435', 'admin@gmail.com')
insert into InfoAccDB(accID, perID , detailsID) values (1,1,1)
go
insert into AccountDB(username, password) values ('staff', '202CB962AC59075B964B07152D234B70') --(SELECT CONVERT(NVARCHAR(32),HashBytes('MD5', 'email@dot.com'),2))
insert into PermitDB(type_acc) values ('staff')
insert into InfoDetailsDB(fullname, telephone, email) values ('Nhan Vien', '0781223', 'staff@gmail.com')
insert into InfoAccDB(accID, perID , detailsID) values (2,2,2)
go
insert into AccountDB(username, password) values ('trainer', '202CB962AC59075B964B07152D234B70') --(SELECT CONVERT(NVARCHAR(32),HashBytes('MD5', 'email@dot.com'),2))
insert into PermitDB(type_acc) values ('trainer')
insert into InfoDetailsDB(fullname, telephone, email) values ('Super Hero', '01234234', 'trainer@gmail.com')
insert into InfoAccDB(accID, perID , detailsID) values (3,3,3)
go
insert into AccountDB(username, password) values ('trainee', '202CB962AC59075B964B07152D234B70') --(SELECT CONVERT(NVARCHAR(32),HashBytes('MD5', 'email@dot.com'),2))
insert into PermitDB(type_acc) values ('trainee')
insert into InfoDetailsDB(fullname, telephone, email) values ('Toi la sinh vien', '788123', 'trainee@gmail.com')
insert into InfoAccDB(accID, perID , detailsID) values (4,4,4)

----insert into AccountDB(username, password) values ('student', '202CB962AC59075B964B07152D234B70') --(SELECT CONVERT(NVARCHAR(32),HashBytes('MD5', 'email@dot.com'),2))
----insert into PermitDB(type_acc) values ('trainee')
----insert into InfoDetailsDB(fullname, telephone, email) values ('New Student', '0789342', 'student@gmail.com')
----insert into InfoAccDB(accID, perID , detailsID, couD_ID) values (5,5,5,1)

--select * from AccountDB
--select * from PermitDB
--select * from InfoDetailsDB
--select * from InfoAccDB

--delete AccountDB where accID = 5
--delete PermitDB where perID = 5

--Update InfoAccDB set couD_ID = 1 where infoID=1
--Course

create table TopicDB
(topID int primary key identity(1,1) not null,
name nvarchar(200) not null,
description nvarchar(500)
)

create table CourseDB
(couID int primary key identity(1,1) not null,
name nvarchar(200) not null,
description nvarchar(500)
)

create table CategoryDB
(cateID int primary key identity(1,1) not null,
name nvarchar(200) not null,
description nvarchar(500)
)

create table CourseDetailsDB
(couD_ID int primary key identity(1,1) not null,
couID int default  null,
cateID int default  null,
topID int default  null
)
go

--insert into TopicDB(name, description) values ('2021','')
--insert into CourseDB(name, description) values ('Networking','About internet security')
--insert into CategoryDB(name, description) values ('Security','Security on PC')

--insert into CourseDetailsDB(topID, couID, cateID) values (1,1,1)

--insert into CourseDetailsDB( couID) values (2)


--select * from TopicDB
--select * from CourseDB
--select * from CategoryDB
--select * from CourseDetailsDB

--Rang buoc

alter table InfoAccDB
add constraint FK_InfoAccDB_1 foreign key(accID) references AccountDB(accID)


alter table InfoAccDB
add constraint FK_InfoAccDB_2 foreign key(perID) references PermitDB(perID)

alter table InfoAccDB
add constraint FK_InfoAccDB_3 foreign key(detailsID) references InfoDetailsDB(detailsID)

alter table CourseDetailsDB
add constraint FK_CourseDetailsDB_1 foreign key(couID) references CourseDB(couID)

alter table CourseDetailsDB
add constraint FK_CourseDetailsDB_2 foreign key(cateID) references CategoryDB(cateID)

alter table CourseDetailsDB
add constraint FK_CourseDetailsDB_3 foreign key(topID) references TopicDB(topID)



--go
--Lien ket 2 bang chinh

alter table InfoAccDB
add constraint FK_CourseDetailsDB foreign key(couD_ID) references CourseDetailsDB(couD_ID)

--go
