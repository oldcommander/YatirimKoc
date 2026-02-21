using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YatirimKoc.Application.Features.SiteSettings.Dtos
{
    public class SiteSettingsDto
    {
        public string SiteName { get; set; } = default!;
        public string? LogoUrl { get; set; }
        public bool MaintenanceMode { get; set; }
    }
}
