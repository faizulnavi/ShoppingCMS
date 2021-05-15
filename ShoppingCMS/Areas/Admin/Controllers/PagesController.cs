using ShoppingCMS.Models.Data;
using ShoppingCMS.Models.ViewModels;
using ShoppingCMS.Models.ViewModels.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShoppingCMS.Areas.Admin.Controllers
{
    public class PagesController : Controller
    {
        // GET: Admin/Pages
        public ActionResult Index()
        {
            //Declare List of PageVM
            List<PageVM> pagesList;

            using (Db db = new Db())
            {
                //Init the list
                pagesList = db.Pages.ToArray().OrderBy(x => x.Sorting).Select(x => new PageVM(x)).ToList();
            }

            //Return the view List
            return View(pagesList);
        }
        // GET: Admin/Pages/AddPage
        [HttpGet]
        public ActionResult AddPage()
        {
            return View();
        }
        // Post: Admin/Pages/AddPage
        [HttpPost]
        public ActionResult AddPage(PageVM model)
        {
            //Check Model state
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            using (Db db = new Db())
            {

                //Declare slug
                string slug;

                //Init PageDTO
                PageDTO dto = new PageDTO();

                //DTO Title
                dto.Title = model.Title;

                //Check for and set slug if need be
                if (string.IsNullOrWhiteSpace(model.Slug))
                {
                    slug = model.Title.Replace(" ", "-").ToLower();
                }
                else
                {
                    slug = model.Slug.Replace(" ", "-").ToLower();
                }

                //Make sure title and slug are unique
                if(db.Pages.Any(x => x.Title == model.Title) || db.Pages.Any (x => x.Slug == model.Slug))
                {
                    ModelState.AddModelError("", "Title or Slug already exist");
                    return View(model);
                }
                //DTO the rest
                dto.Slug = slug;
                dto.Body = model.Body;
                dto.HasSidebar = model.HasSidebar;
                dto.Sorting = 100;

                //Save DTO
                db.Pages.Add(dto);
                db.SaveChanges();
            }

            //Save TempData Message
            TempData["SM"] = "You have added new Page";
            //Redirect
            return RedirectToAction("AddPage");
        }
        // Get: Admin/Pages/EditPage/1
        [HttpGet]
        public ActionResult EditPage(int id)
        {
            // Declare PageVM
            PageVM model;
            
            using (Db db = new Db())
            {
                //Get the Page
                PageDTO dto = db.Pages.Find(id);
                //Confirm the page exist
                if (dto == null)
                {
                    return Content("The Page doesn't not exists.");
                }
                //Init Page
                model = new PageVM(dto);
            }

            //Return Page with View Model
            return View(model);
        }
        // Post: Admin/Pages/EditPage/1
        [HttpPost]
        public ActionResult EditPage(PageVM model)
        {
            //Check Model state
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (Db db = new Db())
            {
                //Get Page Id
                int id = model.Id;

                //Declare Slug
                string slug="home";

                //Get the Page
                PageDTO dto = db.Pages.Find(id);
                //DTO the title
                dto.Title = model.Title;

                //Check for the slug and set it if need be
                if (model.Slug != "home")
                {
                    if (string.IsNullOrWhiteSpace(model.Slug))
                    {
                        slug = model.Title.Replace(" ", "-").ToLower();
                    }
                    else
                    {
                        slug = model.Slug.Replace(" ", "-").ToLower();
                    }

                }
                //Make sure title and Slug are unique
                if (db.Pages.Where(x => x.Id != id).Any(x => x.Title == model.Title) || db.Pages.Where(x => x.Id != id).Any(x => x.Slug == slug))
                {
                    ModelState.AddModelError("", "That title or slug already exist");
                    return View(model);
                }
                //DTO the rest
                dto.Slug = slug;
                dto.Body = model.Body;
                dto.HasSidebar = model.HasSidebar;

                //Save the DTO
                db.SaveChanges();
            }

            //Set tempdata messaeg
            TempData["SM"] = "Changes have been saved of Pages.";
            //Redirect
            return RedirectToAction("EditPage");
        }
        // GET: Admin/Pages/PageDetails/1
        public ActionResult PageDetails(int id)
        {
            //Declare PageVM
            PageVM model;
            using (Db db = new Db())
            {
                //Get the Page
                PageDTO dto = db.Pages.Find(id);
                //Confirm the Page exists
                if(dto == null)
                {
                    return Content("The Page does not exist");
                }
                //Init PageVM
                model = new PageVM(dto);
            }

            return View(model);
        }
        // GET: Admin/Pages/DeletePage/1
        public ActionResult DeletePage(int id)
        {
            using (Db db = new Db())
            {
                //Get the Page
                PageDTO dto = db.Pages.Find(id);
                //Remove the page
                db.Pages.Remove(dto);
                //Save
                db.SaveChanges();
            }
            //Redirect
            return RedirectToAction("Index");
        }
        // Post: Admin/Pages/ReorderPages/
        [HttpPost]
        public void ReorderPages(int [] id)
        {
            using (Db db = new Db())
            {
                //set initial count
                int count = 1;

                //Declare PageDTO
                PageDTO dto;

                //Set sorting for each page
                foreach(var pageId in id)
                {
                    dto = db.Pages.Find(pageId);
                    dto.Sorting = count;
                    db.SaveChanges();
                    count++;
                }
            }
        }
        // GET: Admin/Pages/EditSidebar
        [HttpGet]
        public ActionResult EditSidebar()
        {
            //Declare Model
            SidebarVM model;

            using (Db db= new Db())
            {
                //Get the DTO
                SidebarDTO dto = db.Sidebar.Find(1);
                //Init Model
                model = new SidebarVM(dto);
            }



            //Return View with Model
            return View(model);
        }
        // Post: Admin/Pages/EditSidebar
        [HttpPost]
        public ActionResult EditSidebar(SidebarVM model)
        {
           using (Db db = new Db())
            {
                //Get the DTO
                SidebarDTO dto = db.Sidebar.Find(1);
                //DTO the body
                dto.Body = model.Body;
                //Save
                db.SaveChanges();
            }
            //Save Temp Data Message
            TempData["SM"] = "Sidebar has been edited";
            //Redirect 
            return RedirectToAction("EditSidebar");
        }
    }
}