using System.Data.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SIGCOMT.Domain;
using SIGCOMT.IoC;
using SIGCOMT.Persistence;
using SIGCOMT.Persistence.EntityFramework;

namespace SIGCOMT.RepositoryTest.Core
{
    [TestClass]
    public class InitializeTest
    {
        [AssemblyInitialize]
        public static void GenerarSnapShotDB(TestContext testContext)
        {
            Database.SetInitializer(new ContextInitializer());
            Database.SetInitializer<DbContextBase>(null);
            PersistenceConfigurator.Configure("SIGCOMT", typeof (Usuario).Assembly, typeof (ConnectionFactory).Assembly);

            StructuremapMvc.Start();

            var contextDB = new DbContextBase();
            contextDB.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction, @"
                IF DB_ID('TemplateDB_Snap') IS NOT NULL 
                    DROP DATABASE TemplateDB_Snap;

                CREATE DATABASE TemplateDB_Snap ON
                    ( NAME = TemplateDB, FILENAME = 'D:\Temp\TemplateDB_Snapshot.ss' )
                 AS SNAPSHOT OF TemplateDB;
            ");
        }

        [AssemblyCleanup]
        public static void DeleteSnapshot()
        {
            var contextDB = new DbContextBase();
            contextDB.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction, @"
                USE master                
                DROP DATABASE TemplateDB_Snap;
            ");
        }
    }
}