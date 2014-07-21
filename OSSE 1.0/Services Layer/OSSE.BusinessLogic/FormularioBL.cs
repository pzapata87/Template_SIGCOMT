using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using OSSE.Aspects;
using OSSE.BusinessLogic.Interfaces;
using OSSE.Common;
using OSSE.Common.Enum;
using OSSE.Domain;
using OSSE.Repository;

namespace OSSE.BusinessLogic
{
    [TryCatch(ExceptionTypeExpected = typeof(Exception), RethrowException = true)]
    public class FormularioBL : IFormularioBL
    {
        private readonly IFormularioRepository _formularioRepository;
        private readonly IPermisoRolRepository _permisoRolRepository;

        public FormularioBL(IFormularioRepository formularioRepository, IPermisoRolRepository permisoRolRepository)
        {
            _formularioRepository = formularioRepository;
            _permisoRolRepository = permisoRolRepository;
        }
        
        public List<Formulario> Formularios(Usuario usuario)
        {
            var formularios = new List<Formulario>();
            if (usuario != null)
            {
                const int estadoActivo = (int) TipoEstado.Activo;
                const int permisoMostrar = (int) TipoPermiso.Mostrar;

                var listaRolesUsuario = usuario.RolUsuarioList.Where(p => p.Estado == estadoActivo).Select(p => p.RolId);

                formularios = _permisoRolRepository.FindAll(p => listaRolesUsuario.Any(q => q == p.RolId)
                                                                 && p.TipoPermiso == permisoMostrar).Select(p => p.Formulario).ToList();
            }

            return formularios.ToList();
        }
        
        public Formulario GetById(int id)
        {
            return _formularioRepository.FindOne(id);
        }
        
        public IList<Formulario> FindAll(Expression<Func<Formulario, bool>> where)
        {
            return _formularioRepository.FindAll(where).ToList();
        }
        
        public int Count(Expression<Func<Formulario, bool>> where)
        {
            return _formularioRepository.Count(where);
        }

        public IQueryable<Formulario> GetAll(JQGridParameters<Formulario> parameters)
        {
            return _formularioRepository.FindAllPaging(parameters);
        }
    }
}
