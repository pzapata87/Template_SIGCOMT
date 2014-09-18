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
using System.Web.Security;
using OSSE.BusinessLogic.Interfaces;
using OSSE.Persistence.EntityFramework;
using OSSE.Repository;
using OSSE.Repository.SqlServer;
using OSSE.Service;
using OSSE.Service.Interfaces;
using StructureMap;
using StructureMap.Web;

namespace OSSE.IoC.DependencyResolution
{
    public static class IoC
    {
        public static IContainer Initialize()
        {
            ObjectFactory.Initialize(x =>
            {
                x.Scan(scan =>
                {
                    scan.AssemblyContainingType<IUsuarioRepository>();
                    scan.AssemblyContainingType<UsuarioRepository>();
                    scan.AssemblyContainingType<IUsuarioBL>();
                    scan.WithDefaultConventions();
                });

                x.For<DbContext>().HybridHttpOrThreadLocalScoped().Use<DbContextBase>();

                x.For(typeof(IFormsAuthenticationService)).Use(typeof(FormsAuthenticationService));
                x.For<IMembershipService>().Use<AccountMembershipService>();
                //x.For<MembershipProvider>().Use(Membership.Provider);
                x.For<MembershipProvider>().Use(Membership.Providers["DbMembershipProvider"]);
            });

            return ObjectFactory.Container;
        }
    }
}