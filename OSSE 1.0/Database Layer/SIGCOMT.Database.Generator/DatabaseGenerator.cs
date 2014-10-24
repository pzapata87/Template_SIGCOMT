using System.Data.Entity;
using System.Reflection;
using System.Resources;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SIGCOMT.Domain;
using SIGCOMT.Persistence;
using SIGCOMT.Persistence.EntityFramework;
using Usuario = SIGCOMT.DTO.GlobalResources.Usuario;

namespace SIGCOMT.DataBase.Generator
{
    [TestClass]
    public class DataBaseGenerator
    {
        [TestMethod]
        public void CreateDataBaseDesarrollo()
        {
            var rm = new ResourceManager("SIGCOMT.DTO.GlobalResources.Usuario", typeof(Usuario).Assembly);
            string valor = rm.GetString("EmailRequerido");
            //Database.SetInitializer(new DbContextDropCreateDatabaseAlwaysDesarrollo());
            //PersistenceConfigurator.Configure("SIGCOMT", typeof (Usuario).Assembly, typeof (ConnectionFactory).Assembly);
            //var target = new DbContextBase();
            //target.Database.Initialize(true);
        }
    }
}