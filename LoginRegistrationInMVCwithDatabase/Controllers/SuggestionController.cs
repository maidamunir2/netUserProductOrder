using LoginRegistrationInMVCwithDatabase.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LoginRegistrationInMVCwithDatabase.Controllers
{
    public class SuggestionController : Controller
    {
        LoginRegistrationInMVCEntities1 entities = new LoginRegistrationInMVCEntities1();
        // GET: Suggestion
        public ActionResult Index()
        {
            List<Order> order = Orders();
            List<Product_> pro = null;
            if(order.Count != 0 ){
                var Cat_id = order.Select(x => x.Category_ID.Value).FirstOrDefault();
                List<Product_> product = RandomProductForSuggestion(Cat_id);

                return View(product);
            }
            return View(pro);
        }

        public List<Order> Orders()
        {
            int customerId = entities.RegisterUsers.Where(x => x.Email == System.Web.HttpContext.Current.User.Identity.Name).FirstOrDefault().Id;

            string query = "SELECT * FROM [dbo].[Order] WHERE Customer_ID = '" + customerId + "' ORDER BY Order_id  DESC";
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
                                Order_id = Convert.ToInt32(sdr["Order_id"]),
                                OrderDate = Convert.ToDateTime(sdr["OrderDate"]),
                                Customer_ID = Convert.ToInt32(sdr["Customer_ID"]),
                                Name = sdr["Name"].ToString(),
                                Price = Convert.ToInt32(sdr["Price"]),
                                Category_ID = Convert.ToInt32(sdr["Category_ID"])

                            });

                        }
                    }
                    con.Close();
                }


            }

            return orders;
        }
        public List<Product_> RandomProductForSuggestion(int Cat_id)
        {
            var list = entities.Product_.Where(x =>x.Ctegory_ID == Cat_id).ToList();
            Random number = new Random();
            int index = number.Next(list.Count);
            var selectedList = list[index];
            var id = selectedList.ID;



            string query = "SELECT * FROM [dbo].[Product_] WHERE Ctegory_ID = '" + Cat_id + "' AND ID = '" + id + "'";
            List<Product_> products = new List<Product_>();
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

                            products.Add(new Product_
                            {
                                ID = Convert.ToInt32(sdr["ID"]),
                                Name = sdr["Name"].ToString(),
                                Price = Convert.ToInt32(sdr["Price"]),
                                Image = sdr["Image"].ToString(),
                                Ctegory_ID = Convert.ToInt32(sdr["Ctegory_ID"])

                            });

                        }
                    }
                    con.Close();
                }


            }

            return products;

        }
    }
}