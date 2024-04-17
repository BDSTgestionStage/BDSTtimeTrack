USE BDST_TimeTrack
GO

CREATE PROCEDURE BDST_AuthenticateUser
    @username NVARCHAR(38),
    @password NVARCHAR(300)
AS
BEGIN
    SELECT UTI_ID, UTI_Role, UTI_Prenom
    FROM Utilisateur
    WHERE UTI_Prenom=@username AND UTI_MotDePasse=@password
END 
GO

