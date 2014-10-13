CREATE TABLE [dbo].[Formulario] (
    [Id]                 INT             IDENTITY (1, 1) NOT NULL,
    [Direccion]          NVARCHAR (4000) NULL,
    [Orden]              INT             NOT NULL,
    [Nivel]              INT             NOT NULL,
    [FormularioParentId] INT             NULL,
    [Controlador]        NVARCHAR (100)  NULL,
    [Estado]             INT             NOT NULL,
    CONSTRAINT [PK_dbo.Formulario] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.Formulario_dbo.Formulario_FormularioParentId] FOREIGN KEY ([FormularioParentId]) REFERENCES [dbo].[Formulario] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_FormularioParentId]
    ON [dbo].[Formulario]([FormularioParentId] ASC);

