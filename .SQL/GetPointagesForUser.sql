CREATE PROCEDURE GetPointagesForUser
    @UserId INT
AS
BEGIN
    SELECT POINT_HEURE
    FROM Pointage
    WHERE UTI_ID = @UserId;
END
