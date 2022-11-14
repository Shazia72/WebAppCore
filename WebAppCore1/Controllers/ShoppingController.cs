using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using WebAppCore1.Data;
using WebAppCore1.ViewModels;
using System.Linq;
using WebAppCore1.Models;
using WebAppCore1.Helper;
using System;
using Microsoft.AspNetCore.DataProtection.KeyManagement.Internal;

namespace WebAppCore1.Controllers
{
    public class ShoppingController : Controller
    {
        private readonly ShoppingCartDBAspCoreContext _db;
        private List<ShoppingCartModel> Cart;
        public ShoppingController(ShoppingCartDBAspCoreContext db)
        {
            _db = db;
            Cart= new List<ShoppingCartModel>();
        }
        public ActionResult Shopping()
        {
            List<ShoppingCartViewModel> cartViewModel = (from objItem in _db.Items
                                                         join
                                                         objCat in _db.Categories
                                                         on objItem.CategoryId equals objCat.CategoryId
                                                         select new ShoppingCartViewModel()
                                                         {
                                                             ItemName = objItem.ItemName,
                                                             ImagePath = objItem.ImagePath,
                                                             Description = objItem.Description,
                                                             ItemPrice = objItem.ItemPrice,
                                                             ItemId = objItem.ItemId,
                                                             CategoryId = objCat.CategoryId,
                                                             ItemCode = objItem.ItemCode
                                                         }).ToList();
            return View(cartViewModel);
        }

        [HttpPost]
        public JsonResult AddToCart(string ItemId)
        {

            ShoppingCartModel cartModel = new ShoppingCartModel();
            Items ObjItem = _db.Items.Single(model => model.ItemId.ToString() == ItemId);

            if (HttpContext.Session.GetString("CartCounter") != null)
            {
                Cart = HttpContext.Session.Get<List<ShoppingCartModel>>("CartItem");
            }

            if (Cart.Any(model => model.ItemId == ItemId))
            {
                cartModel = Cart.Single(cart => cart.ItemId == ItemId);
                cartModel.Quantity = cartModel.Quantity + 1;
                cartModel.Total = cartModel.Quantity * cartModel.UnitPrice;
            }
            else
            {
                cartModel.ItemId = ItemId;
                cartModel.ImagePath = ObjItem.ImagePath;
                cartModel.UnitPrice = ObjItem.ItemPrice;
                cartModel.ItemName = ObjItem.ItemName;
                cartModel.Quantity = 1;
                cartModel.Total = ObjItem.ItemPrice;
                Cart.Add(cartModel);
            }
            HttpContext.Session.SetString("CartCounter", Cart.Count.ToString());
            string y = HttpContext.Session.GetString("CartCounter");

            int x = Cart.Count;
            HttpContext.Session.Set<List<ShoppingCartModel>>("CartItem", Cart);
            return Json( Cart.Count );
        }
        public ActionResult ShoppingCart(int t)
        {
            Cart = HttpContext.Session.Get<List<ShoppingCartModel>>("CartItem"); 
            return View(Cart);
        }

        public ActionResult ProceedToCheckOut()
        {
            return View();
        }
        public ActionResult SaveOrder()
        {
            Cart = HttpContext.Session.Get<List<ShoppingCartModel>>("CartItem");// as List<ShoppingCartModel>;
            var order = new Orders()
            {
                OrderDate = DateTime.Now,
                OrderNumber = String.Format("{0:ddmmyyyyHHmmsss}", DateTime.Now)
            };
            _db.Orders.Add(order); // after adding you can get orderId for the recently added order
            _db.SaveChanges();
            var orderId = order.OrderId;
            foreach (var item in Cart)
            {
                OrderDetails orderDetail = new OrderDetails()
                {
                    Quantity = item.Quantity,
                    Total = item.Total,
                    UnitPrice = item.UnitPrice,
                    OrderId = orderId,
                    ItemId = Convert.ToInt32(item.ItemId)
                };
                _db.OrderDetails.Add(orderDetail);
                _db.SaveChanges();
            }
            HttpContext.Session.Clear();
            //HttpContext.Session.SetString("CartCounter", Cart.Count.ToString());
            return RedirectToAction("Shopping");
        }
        public ActionResult ClearCart()
        {
            HttpContext.Session.Clear();
            //HttpContext.Session.Set("CartCounter", null);
            return RedirectToAction("Shopping");
        }
        public JsonResult DeleteItem(string ItemId)
        {
            Cart = HttpContext.Session.Get<List<ShoppingCartModel>>("CartItem");
            var res= Cart.FirstOrDefault(x => x.ItemId == ItemId);
            
            Cart.Remove(res);
            HttpContext.Session.Set<List<ShoppingCartModel>>("CartItem", Cart);
            HttpContext.Session.SetString("CartCounter", Cart.Count.ToString());

            return Json(Cart.Count);
        }
    }
}
