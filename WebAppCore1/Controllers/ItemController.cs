using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WebAppCore1.Data;
using WebAppCore1.Helper;
using WebAppCore1.Models;
using WebAppCore1.ViewModels;

namespace WebAppCore1.Controllers
{
    public class ItemController : Controller
    {
        private readonly ShoppingCartDBAspCoreContext _db;
        private readonly IWebHostEnvironment _env;
        public ItemController(ShoppingCartDBAspCoreContext db, IWebHostEnvironment env)
        {
            _db = db;
            _env = env;
        }
        public ActionResult Index()
        {
            var items = _db.Items.ToList();
            return View(items);
        }

        // GET: ItemController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ItemController/Create
        public ActionResult Create()
        {
            List<Categories> ListOfCategories = new List<Categories>();
            ListOfCategories = (from lst in _db.Categories select lst).ToList();
            ListOfCategories.Insert(0, new Categories { CategoryId = 0, CategoryName = "--Select Category--" });
            // IT IS ALSO POSIIBLE TO GET DATA INTO MODEL THEN USE CODE FOR MODEL IN THE VIEW asp-items="@( new SelectList(Model.Categories, "CategoryId","CategoryName") )"
            ViewBag.message = ListOfCategories;
            return View();
        }

        [HttpPost]
        public JsonResult Create(ItemViewModel item)
        {
            var newImage = Guid.NewGuid().ToString().Substring(0, 4) + Path.GetFileName(item.ImagePath.FileName);
            var images = Path.Combine(_env.WebRootPath, "images");
            var filePath = Path.Combine(images, newImage);
            item.ImagePath.CopyTo(new FileStream(filePath, FileMode.Create));
            try
            {
                if (ModelState.IsValid)
                {
                    var itemDataModel = new Items();
                    itemDataModel.CategoryId = item.CategoryId;
                    itemDataModel.ItemCode = item.ItemCode;
                    itemDataModel.ItemName = item.ItemName;
                    itemDataModel.Description = item.Description;
                    itemDataModel.ItemPrice = item.ItemPrice;
                    itemDataModel.ImagePath = newImage;

                    if(item.ItemId==0)
                    {
                        _db.Items.Add(itemDataModel);
                    }
                    else
                        _db.Items.Update(itemDataModel);

                    _db.SaveChanges();
                }
                return Json(new { Success = true, Message = "Item Added Successfully." });

            }
            catch
            {
                ModelState.AddModelError("ItemName", "Invalid Model State");
                return Json(new { Success = false, Message = "Some Problem." });
            }
        }

        public ActionResult SingleProduct(string itemId)
        {
            //var itemId1 ="84abae2f-b553-4974-8374-7cc6cd587b65";
            var item = _db.Items.Single(x => x.ItemId.ToString() == itemId);
            return View("SingleProduct", item);
        }



        // GET: ItemController/Edit/5
        public ActionResult Edit(int id)
        {
            var item = _db.Items.FirstOrDefault(i => i.ItemId == id);
            List<Categories> ListOfCategories = new List<Categories>();
            ListOfCategories = (from lst in _db.Categories select lst).ToList();
            ListOfCategories.Insert(0, new Categories { CategoryId = 0, CategoryName = "--Select Category--" });
            ViewBag.message = ListOfCategories;
            ViewBag.file=item.ImagePath;
            return View(item);
        }

        // POST: ItemController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Items item, string ImgFileName)
        {
            
            if (item.ItemId != 0)
            {
                if (item.ImagePath == null)
                    item.ImagePath = ImgFileName;
                _db.Items.Update(item);
                _db.SaveChanges();
            }
          
            return RedirectToAction("Index");
        }
        public JsonResult DeleteItem(int ItemId)
        {
           var item = _db.Items.FirstOrDefault(i => i.ItemId == ItemId);
            if(item !=null)
            {
                _db.Items.Remove(item);
                _db.SaveChanges();
            }
            return Json("");
        }
       
    }
}
