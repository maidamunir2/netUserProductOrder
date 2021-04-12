using LoginRegistrationInMVCwithDatabase.Models;
using LoginRegistrationInMVCwithDatabase.ViewModel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LoginRegistrationInMVCwithDatabase.Controllers
{
    public class CartController : Controller
    {
        LoginRegistrationInMVCEntities entities = new LoginRegistrationInMVCEntities();
        // GET: Cart
        public ActionResult AddToCart()
        {
            return View();
        }
        
        public ActionResult ProceedToOrder()
        {
            int customerId = entities.RegisterUsers.Where(x => x.Email == System.Web.HttpContext.Current.User.Identity.Name).FirstOrDefault().Id;

            string query = "SELECT * FROM [dbo].[Order] WHERE Customer_ID = '" + customerId + "' ORDER BY Order_id";
            List<Order> orders = new List<Order>();
            string constr = System.Configuration.ConfigurationManager.ConnectionStrings["LRI"].ConnectionString;
            using (SqlConnection con = new SqlConnection(constr))
            {
                using (SqlCommand cmd = new SqlCommand(query))
                {
                    cmd.CommandType = CommandType.Text;
                    cmd.Connection = con;
                    con.Open();
                    using (SqlDataReader sdr = cmd.ExecuteReader())
                    {
                        while (sdr.Read())
                        {

                            orders.Add(new Order
                            {
                                Order_id =Convert.ToInt32(sdr["Order_id"]),
                                OrderDate = Convert.ToDateTime(sdr["OrderDate"]),
                                Customer_ID = Convert.ToInt32(sdr["Customer_ID"]),
                                Name = sdr["Name"].ToString(),
                                Price = Convert.ToInt32(sdr["Price"])

                            });

                        }
                    }
                    con.Close();
                }

                
            }
            
            return View(orders);
        }
        public ActionResult ordersList()
        {
            int customerId = entities.RegisterUsers.Where(x => x.Email == System.Web.HttpContext.Current.User.Identity.Name).FirstOrDefault().Id;
            if (Session["cart"] != null)
            {

                var cart = (List<Item>)Session["cart"];
                foreach (var item in cart)
                {

                    entities.Orders.Add(new Order
                    {
                        OrderDate = DateTime.Now,
                        Customer_ID = customerId,
                        Name = item.Product.Name,
                        Price = item.Product.Price

                    });

                    entities.SaveChanges();
                }
                
            }
            return RedirectToAction("ProceedToOrder");
           
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