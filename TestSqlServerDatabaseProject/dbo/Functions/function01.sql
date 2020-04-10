-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION function01
(
)
RETURNS int
AS
BEGIN
	-- Declare the return variable here
	DECLARE @result int=1

	-- Add the T-SQL statements to compute the return value here
	SELECT @result+=10

	-- Return the result of the function
	RETURN @Result

END