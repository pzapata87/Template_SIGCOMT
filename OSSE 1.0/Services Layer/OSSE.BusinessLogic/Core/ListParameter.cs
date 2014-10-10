using System;
using System.Linq;
using System.Linq.Expressions;
using OSSE.Common;
using OSSE.Common.Enum;
using OSSE.Common.JQGrid;
using OSSE.Domain.Core;
using GridTable = OSSE.Common.DataTable.GridTable;

namespace OSSE.BusinessLogic.Core
{
    public class ListParameter<T, TResult>
        where T : EntityBase
        where TResult : class
    {
        public ListParameter()
        {
            MostrarSoloActivos = true;
        }

        private IPaging<T> _businessLogicClass;
        public IPaging<T> BusinessLogicClass
        {
            get
            {
                return _businessLogicClass;
            }
            set
            {
                _businessLogicClass = value;
                CountMethod = _businessLogicClass.Count;
                ListMethod = _businessLogicClass.GetAll;
            }
        }

        private Expression<Func<T, bool>> _filtrosAdicionales;
        public Expression<Func<T, bool>> FiltrosAdicionales
        {
            get
            {
                return _filtrosAdicionales ?? (p => p.Estado == (int)TipoEstado.Activo);
            }
            set
            {
                _filtrosAdicionales = MostrarSoloActivos ? value.And(p => p.Estado == (int)TipoEstado.Activo) : value;
            }
        }

        public GridTable Grid { get; set; }
        public Func<T, TResult> SelecctionFormat { get; set; }

        public Func<Expression<Func<T, bool>>, int> CountMethod { get; set; }
        public Func<FilterParameters<T>, IQueryable<T>> ListMethod { get; set; }
        public bool MostrarSoloActivos { get; set; }
    }
}
