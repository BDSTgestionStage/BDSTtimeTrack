CREATE PROCEDURE AddPointage
    @Heure DATETIME,
    @UserId INT
AS
BEGIN
    INSERT INTO Pointage (POINT_HEURE, UTI_ID)
    VALUES (@Heure, @UserId);
END
