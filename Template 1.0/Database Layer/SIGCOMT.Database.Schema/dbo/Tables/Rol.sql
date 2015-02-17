CREATE TABLE [dbo].[Rol] (
    [Id]     INT            IDENTITY (1, 1) NOT NULL,
    [Nombre] NVARCHAR (100) NOT NULL,
    [Estado] INT            NOT NULL,
    CONSTRAINT [PK_dbo.Rol] PRIMARY KEY CLUSTERED ([Id] ASC)
);

