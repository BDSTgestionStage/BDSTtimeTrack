USE BDST_TimeTrack
GO

CREATE PROCEDURE UpdateUtilisateur
    @Auth NVARCHAR(50),
    @Nom NVARCHAR(50),
    @Prenom NVARCHAR(50),
    @MotDePasse NVARCHAR(64),
    @Role NVARCHAR(50)
AS
BEGIN
    UPDATE Utilisateur
    SET UTI_Nom = @Nom,
        UTI_Prenom = @Prenom,
        UTI_MotDePasse = @MotDePasse,
        UTI_Role = @Role
    WHERE UTI_Auth = @Auth;
END
GO

