CREATE PROCEDURE testproc as
declare @id int
insert into test(name) values('name1')
set @id=IDENT_CURRENT('test')
--select @id
select IDENT_CURRENT('test')
