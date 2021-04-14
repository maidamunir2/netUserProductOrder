using LoginRegistrationInMVCwithDatabase.Models;
using LoginRegistrationInMVCwithDatabase.ViewModel;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace LoginRegistrationInMVCwithDatabase.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        
        LoginRegistrationInMVCEntities1 db = new LoginRegistrationInMVCEntities1();
        public ActionResult Index()
        {

            var products = TempData["products"] as List<Product_>;
            if (products != null) {
                var tupleModel1 = new Tuple<List<Product_>, List<Category>>(products, categorySelectList());

                //sending it to view
                return View(tupleModel1);
            }
            else
            {
                var tupleModel = new Tuple<List<Product_>, List<Category>>(GetData(), categorySelectList());

                //sending it to view
                return View(tupleModel);
            }
            
         
        }
        [HttpPost]
        

        public ActionResult CategoryBaseResult(int category)
        {

            string query = "select * from Product_ where Ctegory_ID = '" + category + "'";

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
                                Price = Convert.ToInt32(sdr["Price"]),
                                Name = sdr["Name"].ToString(),
                                Image = sdr["Image"].ToString(),
                                Ctegory_ID = Convert.ToInt32(sdr["Ctegory_ID"])
                            });

                        }
                    }
                    con.Close();
                }
                TempData["products"] = products;
                return RedirectToAction("Index", "Account");
            }
        }

        //Return Register view
        public ActionResult Register()
        {
            return View();
        }

        //I binded the Register View with Register ViewModel, so we can accept object of Register class as parameter.
        //This object contains all the values entered in the form by the user.
        [HttpPost]
        public ActionResult SaveRegisterDetails(Register registerDetails)
        {
            //We check if the model state is valid or not. We have used DataAnnotation attributes.
            //If any form value fails the DataAnnotation validation the model state becomes invalid.
            if (ModelState.IsValid)
            {
                //create database context using Entity framework 
                using (var databaseContext = new LoginRegistrationInMVCEntities1())
                {
                    //If the model state is valid i.e. the form values passed the validation then we are storing the User's details in DB.
                    RegisterUser reglog = new RegisterUser();

                    //Save all details in RegitserUser object

                    reglog.FirstName = registerDetails.FirstName;
                    reglog.LastName = registerDetails.LastName;
                    reglog.Email = registerDetails.Email;
                    reglog.Password = registerDetails.Password;


                    //Calling the SaveDetails method which saves the details.
                    databaseContext.RegisterUsers.Add(reglog);
                    databaseContext.SaveChanges();
                }

                ViewBag.Message = "User Details Saved";
                return View("Register");
            }
            else
            {

                //If the validation fails, we are returning the model object with errors to the view, which will display the error messages.
                return View("Register", registerDetails);
            }
        }

        //Return Register view
        public ActionResult Login()
        {
            return View();
        }

        //The login form is posted to this method.
        [HttpPost]
        public ActionResult Login(LoginViewModel model)
        {
            //Checking the state of model passed as parameter.
            if (ModelState.IsValid)
            {

                //Validating the user, whether the user is valid or not.
                var isValidUser = IsValidUser(model);

                //If user is valid & present in database, we are redirecting it to Welcome page.
                if (isValidUser != null)
                {
                    FormsAuthentication.SetAuthCookie(model.Email, false);
                    return RedirectToAction("Index");
                }
                else
                {
                    //If the username and password combination is not present in DB then error message is shown.
                    ModelState.AddModelError("Failure", "Wrong Username or password combination !");
                    return View();
                }
            }
            else
            {
                //If model state is not valid, the model with error message is returned to the View.
                return View(model);
            }
        }

        //function to check if User is valid or not
        public RegisterUser IsValidUser(LoginViewModel model)
        {
            using (var dataContext = new LoginRegistrationInMVCEntities1())
            {
                //Retireving the user details from DB based on username and password enetered by user.
                RegisterUser user = dataContext.RegisterUsers.Where(query => query.Email.Equals(model.Email) && query.Password.Equals(model.Password)).SingleOrDefault();
                //If user is present, then true is returned.
                if (user == null)
                    return null;
                //If user is not present false is returned.
                else
                    return user;
            }
        }
        // retriving data from database
        private List<Product_> GetData()
        {

            string query = "SELECT * FROM Product_";
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
                                Price = Convert.ToInt32(sdr["Price"]),
                                Name = sdr["Name"].ToString(),
                                Image = sdr["Image"].ToString()
                        });
                           
                        }
                    }
                    con.Close();
                }

                return products;
            }
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
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            Session.Abandon(); // it will clear the session at the end of request
            return RedirectToAction("Index");
        }
    
    }
}