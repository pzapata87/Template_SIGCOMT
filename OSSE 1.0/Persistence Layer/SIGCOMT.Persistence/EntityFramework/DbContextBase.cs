using System;
using System.Configuration;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.Entity.Validation;
using System.Linq;

namespace SIGCOMT.Persistence.EntityFramework
{
    public class DbContextBase : DbContext
    {
        public DbContextBase()
            : base(PersistenceConfigurator.ConnectionStringKey)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            string schema = ConfigurationManager.AppSettings["SchemaDB"] ?? "dbo";
            modelBuilder.HasDefaultSchema(schema);
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            new AutoMapper().Apply(new DbModelBuilderWrapper(modelBuilder));
        }

        public override int SaveChanges()
        {
            try
            {
                return base.SaveChanges();
            }
            catch (DbEntityValidationException dbEx)
            {
                string msg = dbEx.EntityValidationErrors.Aggregate(
                    dbEx.Message, (current1, validationErrors) =>
                        validationErrors.ValidationErrors.Aggregate(
                            current1, (current, validationError) =>
                                current + (" " +
                                           string.Format("Class: {0}, Property: {1}, Error: {2}",
                                               validationErrors.Entry.Entity.GetType().FullName,
                                               validationError.PropertyName,
                                               validationError.ErrorMessage))));

                throw new Exception(msg);
            }
        }
    }
}