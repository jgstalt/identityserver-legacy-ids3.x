using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer.Legacy.Services.DbContext;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace IdentityServer.Areas.Admin.Pages.Resources
{
    public class IndexModel : AdminPageModel
    {
        private IExportClientDbContext _exportClientDb = null;
        private IExportResourceDbContext _exportResourceDb = null;

        public IndexModel(IExportClientDbContext exportClientDb = null,
                          IExportResourceDbContext exportResourceDb = null)
        {
            _exportClientDb = exportClientDb;
            _exportResourceDb = exportResourceDb;
        }

        public bool HasExportClientDbContext => _exportClientDb != null;
        public string ExportClientDbContextType => _exportClientDb?.GetType().ToString();
        public bool HasExportResourceDbContext => _exportResourceDb != null;
        public string ExportResourceDbContextType => _exportResourceDb?.GetType().ToString();

        public bool HasExports => HasExportClientDbContext || HasExportResourceDbContext;

        public string ExportClientsMessage { get; set; }
        public string ExportResourcesMessage { get; set; }

        public void OnGet(string exportClientsMessage=null, string exportResourcesMessage = null)
        {
            ExportClientsMessage = exportClientsMessage;
            ExportResourcesMessage = exportResourcesMessage;
        }
    }
}
