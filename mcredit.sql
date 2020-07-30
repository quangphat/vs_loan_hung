

ALTER TABLE MCreditProduct
ALTER COLUMN IsCheckCat char(1)

------------------

        go           
alter function fn_MCProduct_ISCheckCat(@productCode varchar(10))
returns int
as begin
declare @isCheckCat bit = 0;

select @isCheckCat  = ISCheckCat from MCreditProduct where Code = @productCode;
if(@isCheckCat = '1')
return 1
return 0
end

----------