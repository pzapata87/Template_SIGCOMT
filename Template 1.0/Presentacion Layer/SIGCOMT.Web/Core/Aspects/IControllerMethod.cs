using log4net;
using PostSharp.Aspects;

namespace SIGCOMT.Web.Core.Aspects
{
    public interface IControllerMethod
    {
        void Procesar(MethodInterceptionArgs args, ILog log);
    }
}