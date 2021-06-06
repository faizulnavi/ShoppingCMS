using ShoppingCMS.Models.Data;
using ShoppingCMS.Models.ViewModels.Shop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShoppingCMS.Areas.Admin.Controllers
{
    public class ShopController : Controller
    {
        // GET: Admin/Shop/Categories
        public ActionResult Categories()
        {
            //Declare a list of Models
            List<CategoryVM> categoryVMList;
            using (Db db = new Db())
            {
                //Init the list
                categoryVMList = db.Categories
                                 .ToArray()
                                 .OrderBy(x => x.Sorting)
                                 .Select(x => new CategoryVM(x))
                                 .ToList();
            }
            //return the list
            return View(categoryVMList);
        }
    }
}