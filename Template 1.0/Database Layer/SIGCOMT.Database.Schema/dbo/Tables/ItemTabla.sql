CREATE TABLE [dbo].[ItemTabla] (
    [Id]          INT            IDENTITY (1, 1) NOT NULL,
    [Nombre]      NVARCHAR (200) NULL,
    [Descripcion] NVARCHAR (500) NULL,
    [Valor]       NVARCHAR (10)  NOT NULL,
    [TablaId]     INT            NOT NULL,
    [Estado]      INT            NOT NULL,
    CONSTRAINT [PK_dbo.ItemTabla] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.ItemTabla_dbo.Tabla_TablaId] FOREIGN KEY ([TablaId]) REFERENCES [dbo].[Tabla] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_TablaId]
    ON [dbo].[ItemTabla]([TablaId] ASC);

