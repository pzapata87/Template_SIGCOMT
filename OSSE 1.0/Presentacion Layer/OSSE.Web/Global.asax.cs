using System;
using System.Configuration;
using System.Data.Entity;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using log4net.Config;
using OSSE.Common.Constantes;
using OSSE.Domain;
using OSSE.IoC;
using OSSE.Persistence;
using OSSE.Persistence.EntityFramework;
using OSSE.Web.App_Start;

namespace OSSE.Web
{
    // Nota: para obtener instrucciones sobre cómo habilitar el modo clásico de IIS6 o IIS7, 
    // visite http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            Database.SetInitializer(new ContextInitializer());
            Database.SetInitializer<DbContextBase>(null);

            PersistenceConfigurator.Configure("DefaultConnection", typeof(Usuario).Assembly, typeof(ConnectionFactory).Assembly);
            StructuremapMvc.Start();


            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            XmlConfigurator.Configure();
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            Session.Timeout = int.Parse(ConfigurationManager.AppSettings[MasterConstantes.TimeOutSession]);
        }
    }
}