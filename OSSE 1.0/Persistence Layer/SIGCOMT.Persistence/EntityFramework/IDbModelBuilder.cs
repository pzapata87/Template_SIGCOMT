using System;

namespace OSSE.Persistence.EntityFramework
{
    public interface IDbModelBuilder
    {
        void AddConfiguration(Type entityTypeConfiguration);

        void AddEntity(Type entityType);
    }
}