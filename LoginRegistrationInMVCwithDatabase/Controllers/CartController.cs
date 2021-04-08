using LoginRegistrationInMVCwithDatabase.Models;
using LoginRegistrationInMVCwithDatabase.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LoginRegistrationInMVCwithDatabase.Controllers
{
    public class CartController : Controller
    {
        LoginRegistrationInMVCEntities1 entities = new LoginRegistrationInMVCEntities1();
        // GET: Cart
        public ActionResult AddToCart()
        {
            return View();
        }

            [HttpPost]
        public ActionResult AddToCart(int productid)
        {
            if (Session["cart"] == null)
            {
                List<Item> cart = new List<Item>();
                var product = entities.Product_.Find(productid);
                cart.Add(new Item
                {
                    Product = product,
                    Quantity = 1
                });
                Session["cart"] = cart;
            }
            else
            {
                var cart = (List<Item>)Session["cart"];
                var product = entities.Product_.Find(productid);
                
                foreach (var item in cart)
                {
                    if (item.Product.ID == productid)
                    {
                        int prevQty = item.Quantity;
                        cart.Remove(item);
                        cart.Add(new Item()
                        {
                            Product = product,
                            Quantity = prevQty + 1
                        });
                        break;
                    }
                    else
                    {
                      
                            cart.Add(new Item()
                            {
                                Product = product,
                                Quantity = 1
                            });
                        break;
                    }
                }
                Session["cart"] = cart;
            }
             
            return View();
        }
    }
}