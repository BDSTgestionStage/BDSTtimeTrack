USE BDST_TimeTrack
GO
CREATE PROCEDURE GetUserByAuth
    @Auth NVARCHAR(255)
AS
BEGIN
    SELECT UTI_ID, UTI_Nom, UTI_Prenom, UTI_Auth, UTI_Role
    FROM Utilisateur
    WHERE UTI_Auth = @Auth;
END
GO

