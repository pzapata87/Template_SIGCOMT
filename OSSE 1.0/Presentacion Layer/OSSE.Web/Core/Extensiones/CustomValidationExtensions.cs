using System;
using System.Collections.Generic;
using System.Text;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace OSSE.Web.Core.Extensiones
{
    public static class CustomValidationExtensions
    {
        public static MvcHtmlString ValidationSummaryJQuery(this HtmlHelper htmlHelper, string message,
                                                            IDictionary<string, object> htmlAttributes)
        {
            if (!htmlHelper.ViewData.ModelState.IsValid)
                return htmlHelper.ValidationSummary(message, htmlAttributes);
            // use default Validation Summary rendering if ModelState is invalid

            var sb = new StringBuilder(Environment.NewLine);

            var divBuilder = new TagBuilder("div");
            divBuilder.MergeAttributes(htmlAttributes);
            divBuilder.AddCssClass(HtmlHelper.ValidationSummaryValidCssClassName); // intentionally add VALID css class

            if (!string.IsNullOrEmpty(message))
            {
                //--------------------------------------------------------------------------------
                // Build an EMPTY error summary message <span style="display:none"> tag
                //--------------------------------------------------------------------------------
                var spanBuilder = new TagBuilder("span");
                //spanBuilder.MergeAttribute("style", "display:none");
                spanBuilder.SetInnerText(message);
                sb.Append(spanBuilder.ToString(TagRenderMode.Normal)).Append(Environment.NewLine);
            }

            divBuilder.InnerHtml = sb.ToString();
            return MvcHtmlString.Create(divBuilder.ToString(TagRenderMode.Normal));
        }
    }
}