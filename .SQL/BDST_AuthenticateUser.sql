CREATE PROCEDURE BDST_AuthenticateUser
    @username NVARCHAR(38),
    @password NVARCHAR(300)
AS
BEGIN
    SELECT UTI_Role, UTI_Prenom, UTI_MotDePasse
    FROM Utilisateur
    WHERE UTI_Prenom = @username AND UTI_MotDePasse = @password;
END