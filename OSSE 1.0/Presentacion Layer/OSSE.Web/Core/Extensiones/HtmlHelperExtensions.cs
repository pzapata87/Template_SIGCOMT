using System.Web;
using System.Web.Mvc;
using System.Web.WebPages;

namespace OSSE.Web.Core.Extensiones
{
    public static class HtmlHelperExtensions
    {
        public static string ResolveUrl(this HtmlHelper helper, string relativeUrl)
        {
            if (VirtualPathUtility.IsAppRelative(relativeUrl))
            {
                return VirtualPathUtility.ToAbsolute(relativeUrl);
            }

            string curPath = WebPageContext.Current.Page.TemplateInfo.VirtualPath;
            string curDir = VirtualPathUtility.GetDirectory(curPath);
            return VirtualPathUtility.ToAbsolute(VirtualPathUtility.Combine(curDir, relativeUrl));
        }

        public static MvcHtmlString SetImage(this HtmlHelper helper, string url, string nombre, int alto, int ancho)
        {
            string htmlImage = string.Empty;
            url = string.Format("{0}{1}", Utils.AbsoluteWebRoot, url);

            if (alto == 0 && ancho != 0)
            {
                htmlImage = string.Format("<img src='{0}' alt='{1}'  width='{2}' />", url, nombre, ancho);
            }

            if (ancho == 0 && alto != 0)
            {
                htmlImage = string.Format("<img src='{0}' alt='{1}' height ='{2}' />", url, nombre, alto);
            }

            if (alto == 0 && ancho == 0)
            {
                htmlImage = string.Format("<img src='{0}' alt='{1}' />", url, nombre);
            }

            if (alto != 0 && ancho != 0)
            {
                htmlImage = string.Format("<img src='{0}' alt='{1}' width ='{2}' height = '{3}' />", url, nombre, ancho, alto);
            }

            return MvcHtmlString.Create(htmlImage);
        }
    }
}