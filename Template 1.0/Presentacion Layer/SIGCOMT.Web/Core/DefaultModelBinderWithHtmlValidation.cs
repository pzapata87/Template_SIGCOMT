using System;
using System.Diagnostics;
using System.Resources;
using System.Web;
using System.Web.Mvc;
using log4net;
using SIGCOMT.Common.Constantes;
using SIGCOMT.Resources;

namespace SIGCOMT.Web.Core
{
    public class DefaultModelBinderWithHtmlValidation : DefaultModelBinder
    {
        protected static readonly ILog Logger = LogManager.GetLogger(string.Empty);

        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            try
            {
                return base.BindModel(controllerContext, bindingContext);
            }
            catch (HttpRequestValidationException)
            {
                var mensaje = string.Format("Se intentó registrar caracteres inválidos en el campo {0}",
                    bindingContext.ModelMetadata.DisplayName ?? bindingContext.ModelName);

                Logger.ErrorFormat(mensaje);

                var resourceManager = new ResourceManager(typeof(Master));
                var formatoMensaje = resourceManager.GetString(MasterConstantes.CaracteresInvalidosValidation);

                Debug.Assert(formatoMensaje != null, "formatoMensaje != null");

                bindingContext.ModelState.AddModelError(bindingContext.ModelName,
                    string.Format(formatoMensaje,
                        bindingContext.ModelMetadata.DisplayName ?? bindingContext.ModelName));
            }
            
            var provider = bindingContext.ValueProvider as IUnvalidatedValueProvider;
            if (provider == null) return null;
           
            var result = provider.GetValue(bindingContext.ModelName, true);
            return result.AttemptedValue;
        }
    }
}