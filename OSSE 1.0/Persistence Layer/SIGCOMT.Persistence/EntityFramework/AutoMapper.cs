using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using SIGCOMT.Domain.Core;

namespace SIGCOMT.Persistence.EntityFramework
{
    public class AutoMapper
    {
        private readonly IList<Type> _alreadyMappedEntities = new List<Type>();

        public void Apply(IDbModelBuilder modelBuilder)
        {
            if (PersistenceConfigurator.MappingsAssembly != null)
            {
                MapEntitiesFromMappingConfigurations(modelBuilder);
            }
            if (PersistenceConfigurator.EntititesAssembly != null)
            {
                MapEntitiesByDefaultConventions(modelBuilder);
            }
        }

        private void MapEntitiesByDefaultConventions(IDbModelBuilder modelBuilder)
        {
            List<Type> list =
                (from type in
                    PersistenceConfigurator.EntititesAssembly.GetExportedTypes()
                        .Where(p => p.Namespace != "SIGCOMT.Domain.Core" && p.Namespace != "SIGCOMT.Domain.Reporte")
                    where (type.BaseType != null &&
                           (type.BaseType.IsGenericType &&
                            (type.BaseType.GetGenericTypeDefinition() == typeof (Entity<>) ||
                             type.BaseType.GetGenericTypeDefinition() == typeof (EntityWithTypedId<>) ||
                             type.BaseType.GetGenericTypeDefinition() == typeof (EntityExtension<>) ||
                             type.BaseType.GetGenericTypeDefinition() == typeof (EntityBase))))
                    select type).ToList<Type>();
            foreach (Type type in list)
            {
                if (!_alreadyMappedEntities.Contains(type))
                {
                    modelBuilder.AddEntity(type);
                }
            }
        }

        private void MapEntitiesFromMappingConfigurations(IDbModelBuilder modelBuilder)
        {
            List<Type> list =
                (from type in
                    PersistenceConfigurator.MappingsAssembly.GetTypes()
                        .Where(p => p.Namespace != "SIGCOMT.Domain.Core" && p.Namespace != "SIGCOMT.Domain.Reporte")
                    where
                        type.BaseType != null &&
                        (type.BaseType.IsGenericType && (type.BaseType.GetGenericTypeDefinition() == typeof (EntityTypeConfiguration<>)))
                    select type).ToList<Type>();
            foreach (Type type in list)
            {
                modelBuilder.AddConfiguration(type);
                Type baseType = type.BaseType;
                _alreadyMappedEntities.Add(baseType.GetGenericArguments()[0]);
            }
        }
    }
}