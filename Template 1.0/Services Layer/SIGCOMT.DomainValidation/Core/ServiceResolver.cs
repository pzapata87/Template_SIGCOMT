using System;
using System.Collections.Generic;
using System.Linq;

namespace SIGCOMT.DomainValidation.Core
{
    public class ServiceResolver
    {
        private readonly Dictionary<Type, object> _store;
        private readonly Dictionary<Type, Type> _bindings;

        /// <summary>
        /// Default constructor; instantiates a new ServiceResolver object.
        /// </summary>
        public ServiceResolver()
        {
            _store = new Dictionary<Type, object>();
            _bindings = new Dictionary<Type, Type>();
        }

        public IValidation Resolve(object objeto)
        {
            var tipoDestino =  (from tipo in _bindings.Keys where tipo.IsInstanceOfType(objeto) select _bindings[tipo]).FirstOrDefault();
            if (tipoDestino == null)
                return null;

            // check for already requested object
            if (_store.ContainsKey(tipoDestino))
                return (IValidation)_store[tipoDestino];

            // create a new instance of this type
            var obj = (IValidation)Activator.CreateInstance(tipoDestino);

            // add to store for future use
            _store.Add(tipoDestino, obj);

            return obj;
        }

        /// <summary>
        /// Registers a type with its corresponding implementation type.
        /// </summary>
        /// <typeparam name="TFrom">The abstract type or interface to use as a key.</typeparam>
        /// <typeparam name="TTo">The implementation type to use as a value.</typeparam>
        public void Register<TFrom, TTo>()
        {
            _bindings.Add(typeof(TFrom), typeof(TTo));
        }
    }
}