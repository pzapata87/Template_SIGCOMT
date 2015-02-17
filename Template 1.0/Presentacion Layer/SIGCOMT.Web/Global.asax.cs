using System;
using System.Configuration;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Routing;
using log4net.Config;
using SIGCOMT.BusinessLogic.Interfaces;
using SIGCOMT.Cache;
using SIGCOMT.Common.Constantes;
using SIGCOMT.Common.Enum;
using SIGCOMT.IoC;
using SIGCOMT.Persistence;
using SIGCOMT.Persistence.EntityFramework;
using SIGCOMT.Resources;
using SIGCOMT.Resources.CustomModelMetadata;
using StructureMap;
using Usuario = SIGCOMT.Domain.Usuario;

namespace SIGCOMT.Web
{
    // Nota: para obtener instrucciones sobre cómo habilitar el modo clásico de IIS6 o IIS7, 
    // visite http://go.microsoft.com/?LinkId=9394801
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            Database.SetInitializer(new ContextInitializer());
            Database.SetInitializer<DbContextBase>(null);

            PersistenceConfigurator.Configure("DefaultConnection", typeof (Usuario).Assembly, typeof (ConnectionFactory).Assembly);
            StructuremapMvc.Start();


            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            XmlConfigurator.Configure();

            ModelMetadataProviders.Current = new ConventionalModelMetadataProvider(true, typeof(Master));

            CargarParametrosAplicacion();
        }

        private void CargarParametrosAplicacion()
        {
            // Cargar datos de idiomas.
            var itemTablaBL = ObjectFactory.GetInstance<IItemTablaBL>();
            var listaIdiomasDomain = itemTablaBL.FindAll(p => p.TablaId == (int) TipoTabla.Idioma).ToList();
            GlobalParameters.Idiomas = listaIdiomasDomain.ToDictionary(p => int.Parse(p.Valor), p => p.Descripcion);

            // Cargar datos de permisos para formularios.
            //var formularioBL = ObjectFactory.GetInstance<IFormularioBL>();
            //var formularios = formularioBL.FindAll(p => p.Estado == (int)TipoEstado.Activo).Include(p => p.PermisoList).ToList();
            //GlobalParameters.PermisoFormularioList = FormularioConverter.DomainToDtoPermiso(formularios);
        }

        protected void Session_Start(object sender, EventArgs e)
        {
            Session.Timeout = int.Parse(ConfigurationManager.AppSettings[MasterConstantes.TimeOutSession]);
        }
    }
}