namespace SIGCOMT.DomainValidation.Core
{
    public class ServiceLocator
    {
        private static ServiceResolver _instance;
        private static readonly object Lock = new object();

        /// <summary>
        /// Gets a shared instance of the ServiceResolver.
        /// </summary>
        /// <returns>Returns the shared instance of a ServiceResolver, or a new one if it has not yet been accessed.</returns>
        private static ServiceResolver GetInstance()
        {
            lock (Lock)
            {
                return _instance ?? (_instance = new ServiceResolver());
            }
        }

        /// <summary>
        /// Binds an abstract type to a concrete implementation.
        /// </summary>
        /// <typeparam name="TFrom">The abstract type or interface to use as a key.</typeparam>
        /// <typeparam name="TTo">The implementation type to use as a value.</typeparam>
        public static void Register<TFrom, TTo>()
        {
            GetInstance().Register<TFrom, TTo>();
        }

        public static IValidation Resolve(object objeto)
        {
            return GetInstance().Resolve(objeto);
        }
    }
}