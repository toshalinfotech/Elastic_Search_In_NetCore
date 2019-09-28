using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ElasticSearchInNetCore.Web.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public string SearchString { get; set; }

        [BindProperty(SupportsGet = true)]
        [TempData]
        public string ResultData { get; set; }

        public void OnGet()
        {
            //OrderHelper.Insert();
        }

        public void OnPost()
        {
            if (!ModelState.IsValid)
            {
                return;
            }
            ResultData = OrderHelper.GetOrders(SearchString);
        }
    }
}
