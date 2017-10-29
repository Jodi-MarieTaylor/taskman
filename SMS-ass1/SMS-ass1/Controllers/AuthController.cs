using SMS_ass1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Npgsql;
using System.Security.Cryptography;
using System.Text;

namespace SMS_ass1.Controllers
{
    public class AuthController : Controller
    {
        PGDbContext _context;

        public AuthController()
        {
            _context = new PGDbContext();
        }
        public ActionResult signin()
        {
            return View();
        }

        [HttpPost]
        public ActionResult signup2()
        {
            DateTime now = DateTime.Now;
            SMS_ass1.Models.User umodel = new SMS_ass1.Models.User();
            umodel.name = HttpContext.Request.Form["name"].ToString();
            umodel.user_name = HttpContext.Request.Form["user_name"].ToString();
            umodel.phone = HttpContext.Request.Form["phone"].ToString();
            umodel.email = HttpContext.Request.Form["email"].ToString();
            umodel.address = HttpContext.Request.Form["address"].ToString();
            umodel.zid = Guid.NewGuid().ToString();
            umodel.inserted_at = now;
            umodel.updated_at = now;
            string password = HttpContext.Request.Form["password"].ToString();
            MD5 md5Hash = MD5.Create();
            string hash = GetMd5Hash(md5Hash, password);

            umodel.password = hash;
            int result = umodel.SaveDetails();
            if (result > 0)
            {
                ViewBag.Result = "Successfully Registered! Welcome to TaskMan";
            }
            else
            {
                ViewBag.Result = "Something Went Wrong";
            }
            return View("~/Views/Auth/login.cshtml");


        }



        public ActionResult signup()
        {
            ViewBag.Message = "Sign Up now";

            return View();
        }



        public ActionResult login()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }

        [HttpPost]
        public ActionResult login_user()
        {

          
            string email = HttpContext.Request.Form["email"].ToString();
            string password = HttpContext.Request.Form["password"].ToString();
            MD5 md5Hash = MD5.Create();
            string hashOfInput = GetMd5Hash(md5Hash, password);
            bool found = false;
            var conn = new NpgsqlConnection();
            conn.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnectionString"].ConnectionString;
            conn.Open();
            var sql = "Select * from users where email = '" + email + "' and password = '" + hashOfInput +"'";
            
            NpgsqlCommand cmd = new NpgsqlCommand(sql, conn);
             NpgsqlDataReader reader = cmd.ExecuteReader();
            if  (reader.Read())
            {
                found = true;
                Session["tmuser_name"] = reader["user_name"].ToString();
                Session["tmid"] = Int32.Parse(reader["id"].ToString());
                Session["tmzid"] = reader["zid"].ToString();
                ViewBag.Login = true;
                return View("~/Views/Home/Index.cshtml");
            }
            if (found == false)

            {
                ViewBag.Login = false;
                return View("~/Views/Auth/login.cshtml");
            }

            conn.Close();
            return View("~/Views/Auth/login.cshtml");
        }
        public ActionResult logout()
        {
            ViewBag.Message = "Your contact page.";
            Session["tmuser_name"] = null;
            Session["tmzid"] = null;
            //string usrname = Session["username"].ToString();
            //if (Session["username"] == null)
            // Response.Redirect("Login.aspx");
            return View("~/Views/Home/Index.cshtml");
        }
        static string GetMd5Hash(MD5 md5Hash, string input)
        {

            // Convert the input string to a byte array and compute the hash.
            byte[] data = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input));

            // Create a new Stringbuilder to collect the bytes
            // and create a string.
            StringBuilder sBuilder = new StringBuilder();

            // Loop through each byte of the hashed data 
            // and format each one as a hexadecimal string.
            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            // Return the hexadecimal string.
            return sBuilder.ToString();
        }

        static bool VerifyMd5Hash(MD5 md5Hash, string input, string hash)
        {
            // Hash the input.
            string hashOfInput = GetMd5Hash(md5Hash, input);

            // Create a StringComparer an compare the hashes.
            StringComparer comparer = StringComparer.OrdinalIgnoreCase;

            if (0 == comparer.Compare(hashOfInput, hash))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}