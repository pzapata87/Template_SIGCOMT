using System;

namespace SIGCOMT.Persistence.EntityFramework
{
    public interface IDbModelBuilder
    {
        void AddConfiguration(Type entityTypeConfiguration);

        void AddEntity(Type entityType);
    }
}