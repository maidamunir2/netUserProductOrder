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
    public class UploadProductController : Controller
    {
        // GET: UploadProduct
        LoginRegistrationInMVCEntities1 entities = new LoginRegistrationInMVCEntities1();
        public ActionResult Index()
        {
            List<Category> category = categorySelectList();
            return View(category);
        }


        [HttpPost]
        public ActionResult Index(HttpPostedFileBase postedFile, int price,int Category)
        {
            //Extract Image File Name.
            string fileName = System.IO.Path.GetFileName(postedFile.FileName);

            //Set the Image File Path.
            string filePath = "~/UploadImageFile/" + fileName;

            //Save the Image File in Folder.
            postedFile.SaveAs(Server.MapPath(filePath));

            //Insert the Image File details in Table.
            

            entities.Product_.Add(new Product_
            {
                Name = fileName,
                Image = filePath,
                Price = price,
                Ctegory_ID = Category
            });
         
            entities.SaveChanges();

            //Redirect to Index Action.
            return RedirectToAction("Index");
        }

        public List<Category> categorySelectList()
        {
            string query = "SELECT * FROM Category";
            List<Category> category = new List<Category>();
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

                            category.Add(new Category
                            {
                                Category_ID = Convert.ToInt32(sdr["Category_ID"]),
                                Category_Name = sdr["Category_Name"].ToString()

                            });

                        }
                    }
                    con.Close();
                }

                return category;
            }
        }
    }
}