// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IoC.cs" company="Web Advanced">
// Copyright 2012 Web Advanced (www.webadvanced.com)
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0

// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------


using System.Data.Entity;
using OSSE.BusinessLogic;
using OSSE.BusinessLogic.Interfaces;
using OSSE.Persistence.Core;
using OSSE.Persistence.EntityFramework;
using OSSE.Repository;
using OSSE.Repository.RepositoryContracts;
using OSSE.Repository.SqlServer;
using StructureMap;
using StructureMap.Configuration.DSL;
using StructureMap.Graph;
using StructureMap.Web;

namespace OSSE.IoC.DependencyResolution
{
    public static class IoC
    {
        public static IContainer Initialize()
        {
            ObjectFactory.Initialize(x => x.Scan(scan =>
            {
                scan.TheCallingAssembly();
                scan.WithDefaultConventions();
            }));

            ObjectFactory.Initialize(x => x.AddRegistry<ControllerRegistry>());

            return ObjectFactory.Container;
        }
    }

    public class ControllerRegistry : Registry
    {
        public ControllerRegistry()
        {
            #region Repository

            For<DbContext>().HybridHttpOrThreadLocalScoped().Use<DbContextBase>();

            For(typeof (IRepository<>)).Use(typeof (Repository<>));
            For(typeof (IRepositoryWithTypedId<,>)).Use(typeof (RepositoryWithTypedId<,>));
            For<IFormularioRepository>().Use<FormularioRepository>();
            For<IItemTablaRepository>().Use<ItemTablaRepository>();
            For<IPermisoRolRepository>().Use<PermisoRolRepository>();
            For<IUsuarioRepository>().Use<UsuarioRepository>();
            For<IRolRepository>().Use<RolRepository>();
            For<ITablaRepository>().Use<TablaRepository>();
            For<IRepositoryQueryExecutor>().Use<RepositoryQueryExecutor>();

            #endregion

            #region Business Logic

            //For(typeof(IFormsAuthenticationService)).Use(typeof(FormsAuthenticationService));

            For<IFormularioBL>().Use<FormularioBL>();
            For<IItemTablaBL>().Use<ItemTablaBL>();
            For<IPermisoRolBL>().Use<PermisoRolBL>();
            For<IRolBL>().Use<RolBL>();
            For<IUsuarioBL>().Use<UsuarioBL>();
            For<ITablaBL>().Use<TablaBL>();
            //For(typeof(IReporteBL<>)).Use(typeof(ReporteBL<>));

            #endregion

            #region Authentication

            //For<IMembershipService>().Use<AccountMembershipService>();
            //For<MembershipProvider>().Use(Membership.Providers["DbMembershipProvider"]);

            #endregion
        }
    }
}