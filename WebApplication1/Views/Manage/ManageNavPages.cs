using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Microsoft.WebApplication1.Views.Manage
{
    public static class ManageNavPages
    {
        // this class must be static by convention
        public static string activePageKey => "ActivePage";
        public static string Index => "Index";
        public static string IndexNavClass(ViewContext viewContext) => PageNavClass(viewContext, Index);

        public static string PageNavClass(ViewContext viewContext, string page)
        { // static method that checks if selected page is different from current active page
            var activePage = viewContext.ViewData["ActivePage"] as string;

            return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "active" : null;
        }

        public static void AddActivePage(this ViewDataDictionary viewData, string activePage)
        {
            // static method that sets the current active page 
            viewData[activePageKey] = activePage;
        }
  
    
    }


}
