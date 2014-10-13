CREATE TABLE [dbo].[ItemTablaFormulario] (
    [Id]           INT           IDENTITY (1, 1) NOT NULL,
    [FormularioId] INT           NOT NULL,
    [Nombre]       NVARCHAR (50) NOT NULL,
    [ItemTablaId]  INT           NOT NULL,
    [Estado]       INT           NOT NULL,
    CONSTRAINT [PK_dbo.ItemTablaFormulario] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.ItemTablaFormulario_dbo.Formulario_FormularioId] FOREIGN KEY ([FormularioId]) REFERENCES [dbo].[Formulario] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.ItemTablaFormulario_dbo.ItemTabla_ItemTablaId] FOREIGN KEY ([ItemTablaId]) REFERENCES [dbo].[ItemTabla] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_ItemTablaId]
    ON [dbo].[ItemTablaFormulario]([ItemTablaId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_FormularioId]
    ON [dbo].[ItemTablaFormulario]([FormularioId] ASC);

