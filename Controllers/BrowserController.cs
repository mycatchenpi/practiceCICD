using Microsoft.AspNetCore.Mvc;
using Shopping.Data;
using Shopping.Models;
using System.Reflection.Metadata.Ecma335;
using EFDemo.Models;
using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Principal;
using static Login_CA.Filters.LoginFilter;
using Shopping.Constants;

namespace Shopping.Controllers
{
    public class BrowserController : Controller
    {
        public IActionResult Index()
        {
            List<Product> products = ProductData.GetProduct();
            ViewData["product"]=products;

            string? usernameInSession = HttpContext.Session.GetString("username");
            ViewData["username"] = usernameInSession;

            return View();
        }
        
        public IActionResult Search(string query)
        {
            List<Product>products= ProductData.GetProduct();
            List<Product> newproducts=new List<Product>();
            
            foreach(Product p in products)
            {
                if (p.Description.ToLower().Contains(query))
                {
                    newproducts.Add(p);
                }
            }
            ViewData["product"] = newproducts;
            string? usernameInSession = HttpContext.Session.GetString("username");
            ViewData["username"] = usernameInSession;
            return View("Index");
        }

        [MustLogin]
        public IActionResult Purchase()
        {
            var id = HttpContext.Session.GetInt32("id");
            List<CheckOut> check = CheckOutData.GetCheckOut(Convert.ToInt32(id)); //Get the purchase history from the CheckOut Database          

            if (check == null || check.Count == 0)
            {
                ViewBag.message = "Your havn't had purchase histories......";
            }

            for (int i = 0; i < check.Count; i++)
            {
                string ActivationCode = check[i].ActivationCode;
                if (ActivationCode.IndexOf(",") != -1)
                {
                    check[i].ActivationCodeList = ActivationCode.Split(",");
                }
            }

            ViewBag.orderProducts = check;

            return View();
        }

        [HttpPost]
        public JsonResult AddReview(int productId, int starCount)
        {
            try
            {
                using (var context = new MyDbContext(new DbContextOptionsBuilder<MyDbContext>().UseSqlServer(@"Server=DESKTOP-UCVEAQF;Database=Shopping;Integrated Security=true;encrypt=false").Options))
                {
                    // Find the product based on the ID
                    var product = context.Product.Find(productId);

                    // Add the starCount to the product's review count
                    product.ReviewCount += starCount;

                    var clickCount = product.CountClick++;


                    // Save changes to the database
                    context.SaveChanges();


                    // Get the total number of reviews for the product
                    var reviewCount = product.ReviewCount;

                    // Return a JSON object with the new review count
                    return Json(new { count = reviewCount });


                }
            }
            catch (Exception ex)
            {
                // Log the error or handle it in some other way
                return Json(new { error = ex.Message });
            }
        }







        [HttpGet]
        public JsonResult GetReviewCount(int productId)
        {
            try
            {
                using (var context = new MyDbContext(new DbContextOptionsBuilder<MyDbContext>().UseSqlServer(CommonConstants.connectionString).Options))
                {
                    // Find the product based on the ID
                    var product = context.Product.Find(productId);

                    // Get the updated review count for the product
                    var CountClick = product.CountClick;
                    var reviewCount = product.ReviewCount;

                    var averageCount = reviewCount / CountClick;

                    // Return a JSON object with the review count
                    return Json(new { averageCount = averageCount });
                }
            }
            catch (Exception ex)
            {
                // Log the error or handle it in some other way
                return Json(new { error = ex.Message });
            }
        }


    }
}
