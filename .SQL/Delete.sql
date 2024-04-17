USE BDST_TimeTrack
GO

CREATE PROCEDURE DeleteUtilisateur
    @Auth NVARCHAR(50)
AS
BEGIN
    DELETE FROM Utilisateur WHERE UTI_Auth = @Auth;
END;
GO

