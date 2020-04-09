﻿using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace IdentityServer.Areas.Admin.Pages.Resources
{
    public static class ResourcesNavPages
    {
        public static string Index => "Index";

        public static string Clients => "Clients";

        public static string Apis => "Apis";

        public static string IndexNavClass(ViewContext viewContext) => PageNavClass(viewContext, Index);

        public static string ClientsNavClass(ViewContext viewContext) => PageNavClass(viewContext, Clients);

        public static string ApisNavClass(ViewContext viewContext) => PageNavClass(viewContext, Apis);

        private static string PageNavClass(ViewContext viewContext, string page)
        {
            var activePage = viewContext.ViewData["ActivePage"] as string
                ?? System.IO.Path.GetFileNameWithoutExtension(viewContext.ActionDescriptor.DisplayName);
            
            return string.Equals(activePage, page, StringComparison.OrdinalIgnoreCase) ? "active" : null;
        }
    }
}