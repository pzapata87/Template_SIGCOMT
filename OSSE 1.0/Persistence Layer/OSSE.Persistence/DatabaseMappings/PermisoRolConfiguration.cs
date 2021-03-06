﻿using System.Data.Entity.ModelConfiguration;
using OSSE.Domain;

namespace OSSE.Persistence.DatabaseMappings
{
    public class PermisoRolConfiguration : EntityTypeConfiguration<PermisoRol>
    {
        public PermisoRolConfiguration()
        {
            HasRequired(p => p.Formulario).WithMany(p => p.PermisoRolList).HasForeignKey(p => p.FormularioId);
            HasRequired(p => p.Rol).WithMany(p => p.PermisoRolList).HasForeignKey(p => p.RolId);
        }
    }
}