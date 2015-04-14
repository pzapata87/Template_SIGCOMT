using System;
using System.Reflection;
using log4net;

namespace SIGCOMT.DomainValidation.Core
{
    public class ErrorException : Exception
    {
        protected static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public ErrorException(string message) : base(message)
        {

        }
    }
}