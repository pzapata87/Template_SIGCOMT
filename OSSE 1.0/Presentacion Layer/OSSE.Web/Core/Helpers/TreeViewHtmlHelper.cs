using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace OSSE.Web.Core.Helpers
{
    public static class TreeViewHtmlHelper
    {
        /// <summary>
        /// Create a TreeView of nodes starting from a root element
        /// </summary>
        /// <param name="html"></param>
        /// <param name="treeId">The ID that will be used when the ul is created</param>
        /// <param name="rootItems">The root nodes to create</param>
        /// <param name="childrenProperty">A lambda expression that returns the children nodes</param>
        /// <param name="parentContent"></param>
        /// <param name="itemContent">A lambda expression defining the content in each tree node</param>
        public static string TreeView<T>(this HtmlHelper html, string treeId, IEnumerable<T> rootItems, Func<T, IEnumerable<T>> childrenProperty, 
            Func<T, string> parentContent, Func<T, string> itemContent)
        {
            return html.TreeView(treeId, string.Empty, rootItems, childrenProperty, parentContent, itemContent, true, string.Empty, null);
        }

        /// <summary>
        /// Create a TreeView of nodes starting from a root element
        /// </summary>
        /// <param name="html"></param>
        /// <param name="treeId">The ID that will be used when the ul is created</param>
        /// <param name="rootItems">The root nodes to create</param>
        /// <param name="childrenProperty">A lambda expression that returns the children nodes</param>
        /// <param name="parentContent"></param>
        /// <param name="itemContent">A lambda expression defining the content in each tree node</param>
        /// <param name="includeJavaScript">If true, output will automatically render the JavaScript to turn the ul into the treeview</param>    
        public static string TreeView<T>(this HtmlHelper html, string treeId, IEnumerable<T> rootItems, Func<T, IEnumerable<T>> childrenProperty, 
            Func<T, string> parentContent, Func<T, string> itemContent, bool includeJavaScript)
        {
            return html.TreeView(treeId, string.Empty, rootItems, childrenProperty, parentContent, itemContent, includeJavaScript, string.Empty, null);
        }

        /// <summary>
        /// Create a TreeView of nodes starting from a root element
        /// </summary>
        /// <param name="html"></param>
        /// <param name="treeId">The ID that will be used when the ul is created</param>
        /// <param name="label"></param>
        /// <param name="rootItems">The root nodes to create</param>
        /// <param name="childrenProperty">A lambda expression that returns the children nodes</param>
        /// <param name="parentContent"></param>
        /// <param name="itemContent">A lambda expression defining the content in each tree node</param>
        /// <param name="classUl"></param>
        /// <param name="emptyContent">Content to be rendered when the tree is empty</param>
        /// <param name="includeJavaScript">If true, output will automatically into the JavaScript to turn the ul into the treeview</param>    
        public static string TreeView<T>(this HtmlHelper html, string treeId, string label, IEnumerable<T> rootItems, 
            Func<T, IEnumerable<T>> childrenProperty, Func<T, string> parentContent, Func<T, string> itemContent, 
            bool includeJavaScript, string classUl, string emptyContent)
        {
            var sb = new StringBuilder();

            sb.AppendFormat("<ul id='{0}' class='{1}'>\r\n", treeId, classUl);

            if (!string.IsNullOrEmpty(label))
            {
                sb.AppendFormat("<li><span style='border-top: 0px !important;'>{0}", label);
                sb.AppendFormat("<ul>\r\n");
            }

            var listaEnumeradaRootItems = rootItems as IList<T> ?? rootItems.ToList();

            if (!listaEnumeradaRootItems.Any())
            {
                sb.AppendFormat("<li>{0}</li>", emptyContent);
            }

            foreach (T item in listaEnumeradaRootItems)
            {
                var children = childrenProperty(item);

                RenderLi(sb, item, children.Any() ? parentContent : itemContent);
                AppendChildren(sb, item, childrenProperty, parentContent, itemContent);
            }

            sb.AppendLine(!string.IsNullOrEmpty(label) ? "</li></ul>" : "</ul>");

            if (includeJavaScript)
            {
                sb.AppendFormat(
                    @"<script type='text/javascript'>
                    $(document).ready(function() {{
                        $('#{0}').treeview({{ animated: 'fast' }});
                    }});
                </script>", treeId);
            }

            return sb.ToString();
        }

        private static void AppendChildren<T>(StringBuilder sb, T root, Func<T, IEnumerable<T>> childrenProperty, Func<T, string> parentContent, 
            Func<T, string> itemContent)
        {
            var children = childrenProperty(root);
            var listaEnumeradaChildren = children as IList<T> ?? children.ToList();

            if (!listaEnumeradaChildren.Any())
            {
                sb.AppendLine("</li>");
                return;
            }

            sb.AppendLine("\r\n<ul>");
            foreach (T item in listaEnumeradaChildren)
            {
                children = childrenProperty(item);

                RenderLi(sb, item, children.Any() ? parentContent : itemContent);
                AppendChildren(sb, item, childrenProperty, parentContent, itemContent);
            }

            sb.AppendLine("</ul></li>");
        }

        private static void RenderLi<T>(StringBuilder sb, T item, Func<T, string> itemContent)
        {
            sb.AppendFormat("<li>{0}", itemContent(item));
        }
    }
}