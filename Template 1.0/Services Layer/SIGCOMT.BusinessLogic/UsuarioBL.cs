using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using SIGCOMT.Aspects;
using SIGCOMT.BusinessLogic.Interfaces;
using SIGCOMT.Common;
using SIGCOMT.Common.Enum;
using SIGCOMT.Domain;
using SIGCOMT.Persistence.Aspects;
using SIGCOMT.Repository;

namespace SIGCOMT.BusinessLogic
{
    public class UsuarioBL : IUsuarioBL
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public UsuarioBL(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        [TryCatch(ExceptionTypeExpected = typeof (Exception), RethrowException = true)]
        public Usuario Get(Expression<Func<Usuario, bool>> where)
        {
            return _usuarioRepository.FindOne(where);
        }

        [TryCatch(ExceptionTypeExpected = typeof (Exception), RethrowException = true)]
        [CommitsOperation]
        public void Add(Usuario entity)
        {
            _usuarioRepository.Add(entity);
        }

        [TryCatch(ExceptionTypeExpected = typeof (Exception), RethrowException = true)]
        [CommitsOperation]
        public void Add(Usuario entity, IList<int> listaRolSelected)
        {
            foreach (int item in listaRolSelected)
            {
                var rolUsuario = new RolUsuario {RolId = item, Estado = (int) TipoEstado.Activo};
                if (entity.RolUsuarioList == null) entity.RolUsuarioList = new List<RolUsuario>();

                entity.RolUsuarioList.Add(rolUsuario);
            }

            _usuarioRepository.Add(entity);
        }

        [TryCatch(ExceptionTypeExpected = typeof (Exception), RethrowException = true)]
        [CommitsOperation]
        public void Update(Usuario entity)
        {
            _usuarioRepository.Update(entity);
        }

        [TryCatch(ExceptionTypeExpected = typeof (Exception), RethrowException = true)]
        [CommitsOperation]
        public void Update(Usuario entity, IList<int> listaRoleSelected)
        {
            Usuario usuario = _usuarioRepository.FindOne(entity.Id);
            usuario.RolUsuarioList.ToList().ForEach(p => p.Estado = (int) TipoEstado.Inactivo);

            foreach (int item in listaRoleSelected)
            {
                RolUsuario usuarioRol = usuario.RolUsuarioList.FirstOrDefault(p => p.RolId == item);
                if (usuarioRol != null)
                    usuarioRol.Estado = (int) TipoEstado.Activo;
                else
                {
                    var rolUsuario = new RolUsuario {RolId = item, Estado = (int) TipoEstado.Activo};
                    usuario.RolUsuarioList.Add(rolUsuario);
                }
            }

            List<RolUsuario> listaRolesEliminar = usuario.RolUsuarioList.Where(p => p.Estado == (int) TipoEstado.Inactivo).ToList();
            int cantidadItemsEliminar = listaRolesEliminar.Count();
            int iterator = 0;

            while (iterator < cantidadItemsEliminar)
            {
                usuario.RolUsuarioList.Remove(listaRolesEliminar[iterator++]);
            }

            _usuarioRepository.Update(usuario);
        }

        [TryCatch(ExceptionTypeExpected = typeof (Exception), RethrowException = true)]
        public Usuario GetById(int id)
        {
            return _usuarioRepository.FindOne(p => p.Id == id);
        }

        [TryCatch(ExceptionTypeExpected = typeof (Exception), RethrowException = true)]
        public int Count(Expression<Func<Usuario, bool>> where)
        {
            return _usuarioRepository.Count(where);
        }

        [TryCatch(ExceptionTypeExpected = typeof (Exception), RethrowException = true)]
        public IQueryable<Usuario> GetAll(FilterParameters<Usuario> parameters)
        {
            return _usuarioRepository.FindAllPaging(parameters);
        }

        [TryCatch(ExceptionTypeExpected = typeof (Exception), RethrowException = true)]
        public IQueryable<Usuario> FindAll(Expression<Func<Usuario, bool>> where)
        {
            return _usuarioRepository.FindAll(where);
        }

        [TryCatch(ExceptionTypeExpected = typeof (Exception), RethrowException = true)]
        public Usuario ValidateUser(string username, string password)
        {
            Usuario user = Get(p => p.UserName == username && p.Estado == (int) TipoEstado.Activo);

            if (user == null)
            {
                return null;
            }

            if (user.UserName == null)
            {
                return null;
            }

            string enc = Encriptador.Encriptar(password);

            return enc == user.Password ? user : null;
        }
    }
}