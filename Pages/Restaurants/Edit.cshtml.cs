using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using RestaurantViewer.Core;
using RestaurantViewer.Data;

namespace RestaurantViewer.Pages.Restaurants
{
    public class EditModel : PageModel
    {
        private readonly IRestaurantData restaurantData;
        private readonly IHtmlHelper htmlHelper;

        [BindProperty]
        public Restaurant Restaurant { get; set; }

        public IEnumerable<SelectListItem> Cuisines { get; set; }

        public EditModel(IRestaurantData restaurantData, IHtmlHelper htmlHelper)
        {
            this.restaurantData = restaurantData;
            this.htmlHelper = htmlHelper;
        }

        public IActionResult OnGet(int? restaurantId)
        {

            Cuisines = htmlHelper.GetEnumSelectList<Cuisine>();
            if(restaurantId.HasValue)
            {
                Restaurant = restaurantData.GetById(restaurantId.Value);
            }
            else
            {
                Restaurant = new Restaurant();
            }
            
            if (Restaurant == null)
            {
                return RedirectToPage("./NotFound");
            }
            return Page();
        }

        public IActionResult OnPost()
        {
            

            if (!ModelState.IsValid)
            {
                Cuisines = htmlHelper.GetEnumSelectList<Cuisine>();
                return Page();
                
            }

            if(Restaurant.Id > 0)
            {
                restaurantData.Update(Restaurant);
            }
            else
            {
                restaurantData.Add(Restaurant); 
            }
           
            restaurantData.Commit();
            TempData["Message"] = "Restaurant Saved!";
            return RedirectToPage("./Detail", new { restaurantId = Restaurant.Id });
        }
    }
}
