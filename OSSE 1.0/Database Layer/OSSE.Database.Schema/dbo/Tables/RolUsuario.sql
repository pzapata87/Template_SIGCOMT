CREATE TABLE [dbo].[RolUsuario] (
    [UsuarioId] INT NOT NULL,
    [RolId]     INT NOT NULL,
    [Estado]    INT NOT NULL,
    CONSTRAINT [PK_dbo.RolUsuario] PRIMARY KEY CLUSTERED ([UsuarioId] ASC, [RolId] ASC),
    CONSTRAINT [FK_dbo.RolUsuario_dbo.Rol_RolId] FOREIGN KEY ([RolId]) REFERENCES [dbo].[Rol] ([Id]),
    CONSTRAINT [FK_dbo.RolUsuario_dbo.Usuario_UsuarioId] FOREIGN KEY ([UsuarioId]) REFERENCES [dbo].[Usuario] ([Id])
);


GO
CREATE NONCLUSTERED INDEX [IX_RolId]
    ON [dbo].[RolUsuario]([RolId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_UsuarioId]
    ON [dbo].[RolUsuario]([UsuarioId] ASC);

