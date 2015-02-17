CREATE TABLE [dbo].[Tabla] (
    [Id]          INT            NOT NULL,
    [Nombre]      NVARCHAR (40)  NOT NULL,
    [Descripcion] NVARCHAR (250) NULL,
    [Estado]      INT            NOT NULL,
    CONSTRAINT [PK_dbo.Tabla] PRIMARY KEY CLUSTERED ([Id] ASC)
);

