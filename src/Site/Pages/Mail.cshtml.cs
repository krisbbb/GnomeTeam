using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authorization;

namespace GnomeTeam.Site
{
    [Authorize]
    public class MailModel : PageModel
    {
        public void OnGet()
        {
        }
    }
}