CREATE TABLE [dbo].[PermisoRol] (
    [Id]           INT IDENTITY (1, 1) NOT NULL,
    [FormularioId] INT NOT NULL,
    [RolId]        INT NOT NULL,
    [TipoPermiso]  INT NOT NULL,
    [Estado]       INT NOT NULL,
    CONSTRAINT [PK_dbo.PermisoRol] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_dbo.PermisoRol_dbo.Formulario_FormularioId] FOREIGN KEY ([FormularioId]) REFERENCES [dbo].[Formulario] ([Id]) ON DELETE CASCADE,
    CONSTRAINT [FK_dbo.PermisoRol_dbo.Rol_RolId] FOREIGN KEY ([RolId]) REFERENCES [dbo].[Rol] ([Id]) ON DELETE CASCADE
);


GO
CREATE NONCLUSTERED INDEX [IX_RolId]
    ON [dbo].[PermisoRol]([RolId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_FormularioId]
    ON [dbo].[PermisoRol]([FormularioId] ASC);

