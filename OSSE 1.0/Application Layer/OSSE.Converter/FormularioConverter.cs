using System.Collections.Generic;
using System.Linq;
using OSSE.Domain;
using OSSE.DTO;

namespace OSSE.Converter
{
    public class FormularioConverter
    {
        public static List<FormularioDto> DomainToDto(List<Formulario> formularioDomain)
        {
            var modulos = formularioDomain.Where(p => p.FormularioParentId == null);

            return modulos.Select(moduloDomain => new FormularioDto
            {
                Id = moduloDomain.Id, 
                Nombre = moduloDomain.ItemTablaFormularioList.First().Nombre, 
                Icono = moduloDomain.Direccion, 
                Operaciones = MakeOperacionesDtos(moduloDomain)
            }).ToList();
        }

        private static List<OperacionDto> MakeOperacionesDtos(Formulario moduloDomain)
        {
            if (moduloDomain.FormulariosHijosList != null)
            {
                return moduloDomain.FormulariosHijosList.Select(operacionHija => new OperacionDto
                {
                    Id = operacionHija.Id, 
                    Controlador = operacionHija.Controlador, 
                    Nombre = operacionHija.ItemTablaFormularioList.First().Nombre, 
                    Operaciones = MakeOperacionesDtos(operacionHija)
                }).ToList();
            }

            return null;
        }
    }
}
