using System;
using System.Data.Entity;

namespace SIGCOMT.Persistence.EntityFramework
{
    public class DbModelBuilderWrapper : IDbModelBuilder
    {
        private readonly DbModelBuilder _modelBuilder;

        public DbModelBuilderWrapper(DbModelBuilder modelBuilder)
        {
            _modelBuilder = modelBuilder;
        }

        public void AddConfiguration(Type entityTypeConfiguration)
        {
            object obj2 = Activator.CreateInstance(entityTypeConfiguration);
            _modelBuilder.Configurations.Add((dynamic) obj2);
        }

        public void AddEntity(Type entityType)
        {
            typeof (DbModelBuilder).GetMethod("Entity").MakeGenericMethod(new[] {entityType}).Invoke(_modelBuilder, null);
        }
    }
}