USE BDST_TimeTrack
GO

CREATE PROCEDURE CreerNouvelUtilisateur
    @UTI_ID int,
    @UTI_NOM varchar(38),
    @UTI_PRENOM varchar(38),
    @UTI_AUTH varchar(38),
    @UTI_MOTDEPASSE varchar(300),
    @UTI_ROLE varchar(38)
AS
BEGIN
    -- Créer le nouvel utilisateur en utilisant le rôle fourni
    INSERT INTO Utilisateur (UTI_Nom, UTI_Prenom, UTI_Auth, UTI_MotDePasse, UTI_Role)
    VALUES (@UTI_NOM, @UTI_PRENOM, @UTI_AUTH, @UTI_MOTDEPASSE, @UTI_ROLE);

    PRINT 'Nouvel utilisateur créé avec succès.';
END;
GO

