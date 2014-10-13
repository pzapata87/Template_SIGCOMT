CREATE TABLE [dbo].[Usuario] (
    [Id]       INT            IDENTITY (1, 1) NOT NULL,
    [UserName] NVARCHAR (100) NOT NULL,
    [Email]    NVARCHAR (50)  NOT NULL,
    [Password] NVARCHAR (100) NOT NULL,
    [Nombre]   NVARCHAR (30)  NULL,
    [Apellido] NVARCHAR (50)  NULL,
    [Telefono] NVARCHAR (50)  NULL,
    [IdiomaId] INT            NOT NULL,
    [Estado]   INT            NOT NULL,
    CONSTRAINT [PK_dbo.Usuario] PRIMARY KEY CLUSTERED ([Id] ASC)
);

