using System;
using System.Linq;
using System.Linq.Expressions;
using OSSE.Common;
using OSSE.Common.DataTable;
using OSSE.Common.Enum;
using OSSE.Domain.Core;

namespace OSSE.BusinessLogic.Core
{
    public class ListParameter<T, TResult>
        where T : EntityBase
        where TResult : class
    {
        private IPaging<T> _businessLogicClass;

        private Expression<Func<T, bool>> _filtrosAdicionales;

        public ListParameter()
        {
            MostrarSoloActivos = true;
        }

        public IPaging<T> BusinessLogicClass
        {
            get { return _businessLogicClass; }
            set
            {
                _businessLogicClass = value;
                CountMethod = _businessLogicClass.Count;
                ListMethod = _businessLogicClass.GetAll;
            }
        }

        public Expression<Func<T, bool>> FiltrosAdicionales
        {
            get { return _filtrosAdicionales ?? (p => p.Estado == (int) TipoEstado.Activo); }
            set { _filtrosAdicionales = MostrarSoloActivos ? value.And(p => p.Estado == (int) TipoEstado.Activo) : value; }
        }

        public GridTable Grid { get; set; }
        public Func<T, TResult> SelecctionFormat { get; set; }

        public Func<Expression<Func<T, bool>>, int> CountMethod { get; set; }
        public Func<FilterParameters<T>, IQueryable<T>> ListMethod { get; set; }
        public bool MostrarSoloActivos { get; set; }
    }
}