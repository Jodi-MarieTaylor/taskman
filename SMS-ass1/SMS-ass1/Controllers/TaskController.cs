using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Npgsql;
using System.Security.Cryptography;
using System.Text;
using System.Data;
using System.Data.Entity;

using SMS_ass1.Models;

namespace SMS_ass1.Controllers
{
    public class TaskController : Controller
    {
        // GET: Task


        PGDbContext _context;

        public TaskController()
        {
            _context = new PGDbContext();
        }

        [HttpPost]
        public ActionResult create_task()
        {
            DateTime now = DateTime.Now;
            SMS_ass1.Models.Task umodel = new SMS_ass1.Models.Task();
            umodel.title = HttpContext.Request.Form["title"].ToString();
            umodel.description = HttpContext.Request.Form["description"].ToString();
            umodel.category = HttpContext.Request.Form["category"].ToString();
            umodel.notes = HttpContext.Request.Form["notes"].ToString();
            umodel.priority = HttpContext.Request.Form["priority"];
            umodel.re_dates = HttpContext.Request.Form["re_dates"];
            // DateTime.ParseExact("2012-12-12, "yyyy-mm-dd", null)
            DateTime due;
            DateTime.TryParse(HttpContext.Request.Form["due"], out due);
            umodel.due = due;
            umodel.zid = Guid.NewGuid().ToString();
            umodel.inserted_at = now;
            umodel.updated_at = now;
            
           umodel.parent_task = Int32.Parse(HttpContext.Request.Form["parent_task"].ToString());



            umodel.rec_date = get_recurrence(HttpContext.Request.Form["re_dates"]);
            umodel.user_id = Int32.Parse(Session["tmid"].ToString());
            umodel.status = HttpContext.Request.Form["status"]; 

            int result = umodel.SaveDetails();
            if (result > 0)
            {
                ViewBag.Result = "Task Created and Saved Successfully";
            }
            else
            {
                ViewBag.Result = "Something Went Wrong";
            }
            return View("~/Views/Home/Index.cshtml");


        }


        public ActionResult create()
        {
            return View();



        }
        public ActionResult view_tasks()
        {
            List<Task> model = new List<Task>();
            var conn = new NpgsqlConnection();
            conn.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnectionString"].ConnectionString;
            conn.Open();
            var sql = "Select * from tasks where user_id = " + Session["tmid"] + " ";
            NpgsqlCommand cmd = new NpgsqlCommand(sql, conn);
                NpgsqlDataReader reader = cmd.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        model.Add(
                           new Task()
                           {
                               title = reader["title"].ToString(),
                               description = reader["description"].ToString(),
                               notes = reader["notes"].ToString(),
                               category = reader["category"].ToString(),
                               priority = reader["priority"].ToString(),
                               inserted_at = Convert.ToDateTime(reader["inserted_at"].ToString()),
                               status = reader["status"].ToString(),
                               due = Convert.ToDateTime(reader["due"].ToString()),
                               id = Int32.Parse(reader["id"].ToString())



                           });
                    }
                }
                catch (Exception e)
                {

                }

            conn.Close();
            return View(model);
        }

        public ActionResult delete(int? id) {
            using (NpgsqlConnection conn = new NpgsqlConnection())
            {
                conn.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnectionString"].ConnectionString;
                conn.Open();
                NpgsqlCommand cmd = new NpgsqlCommand();
                cmd.Connection = conn;
                cmd.CommandText = "Delete from tasks where id=@ID";
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add(new NpgsqlParameter("@ID", Convert.ToInt32(id)));
                cmd.ExecuteNonQuery();
                cmd.Dispose();
                conn.Close();
                ViewBag.Result = "Task Deleted Successfully";
            }


            return View("~/Views/Home/Index.cshtml");
        }



        public ActionResult update(int? id)
        {
            Task Reports = new Task();
            Task r;
            var conn = new NpgsqlConnection();
            conn.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnectionString"].ConnectionString;
            conn.Open();
            var sql = "Select * from tasks where id = " + id +" " ;
            NpgsqlCommand cmd = new NpgsqlCommand(sql, conn);
            NpgsqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                r = new Task();
                r.title = reader["title"].ToString();
                r.description = reader["description"].ToString();
                r.notes = reader["notes"].ToString();
                r.status = reader["status"].ToString();
                r.re_dates = reader["re_dates"].ToString();

                r.due = Convert.ToDateTime(reader["due"].ToString());
                r.category = reader["category"].ToString();
                r.priority = reader["priority"].ToString();
                r.inserted_at = Convert.ToDateTime(reader["inserted_at"].ToString());
                r.updated_at = Convert.ToDateTime(reader["updated_at"].ToString());
                r.id = Int32.Parse(reader["id"].ToString());
                r.parent_task = Int32.Parse(reader["parent_task"].ToString());
                Reports = r;

                ViewBag.date = Convert.ToDateTime(reader["due"].ToString());
            }
           
           
           
            
           
            return View(Reports);
        }

        [HttpPost]
        public ActionResult update_task()
        {
            DateTime now = DateTime.Now;
            SMS_ass1.Models.Task umodel = new SMS_ass1.Models.Task();
            umodel.id = Int32.Parse(HttpContext.Request.Form["id"]);
            umodel.title = HttpContext.Request.Form["title"].ToString();
            umodel.description = HttpContext.Request.Form["description"].ToString();
            umodel.category = HttpContext.Request.Form["category"].ToString();
            umodel.notes = HttpContext.Request.Form["notes"].ToString();
            umodel.priority = HttpContext.Request.Form["priority"];
            umodel.re_dates = HttpContext.Request.Form["re_dates"];

            DateTime due;
            DateTime.TryParse(HttpContext.Request.Form["due"], out due);
            umodel.due = due;
            umodel.updated_at = now;
            umodel.rec_date = now;
            umodel.status = HttpContext.Request.Form["status"]; 

            int result = umodel.UpdateDetails();
            if (result > 0)
            {
                ViewBag.Result = "Task Updated Successfully";
            }
            else
            {
                ViewBag.Result = "Something Went Wrong";
            }
            return View("~/Views/Home/Index.cshtml");


        }
        public ActionResult details(int? id) {

            Task Reports = new Task();
            Task r;
            var conn = new NpgsqlConnection();
            conn.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnectionString"].ConnectionString;
            conn.Open();
            var sql = "Select * from tasks where id = " + id + " ";
            NpgsqlCommand cmd = new NpgsqlCommand(sql, conn);
            NpgsqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                r = new Task();
                r.title = reader["title"].ToString();
                r.description = reader["description"].ToString();
                r.notes = reader["notes"].ToString();
                r.status = reader["status"].ToString();
                r.re_dates = reader["re_dates"].ToString();
                r.due = Convert.ToDateTime(reader["due"].ToString());
                r.rec_date = Convert.ToDateTime(reader["rec_date"].ToString());
                r.inserted_at = Convert.ToDateTime(reader["inserted_at"].ToString());
                r.updated_at = Convert.ToDateTime(reader["updated_at"].ToString());
                r.category = reader["category"].ToString();
                r.priority = reader["priority"].ToString();
                r.id = Int32.Parse(reader["id"].ToString());
                r.parent_task = Int32.Parse(reader["parent_task"].ToString());
                Reports = r;

                ViewBag.date = Convert.ToDateTime(reader["due"].ToString());
            }

            return View(Reports);
            
        }
        public DateTime get_recurrence(string recurrence)
        {
            DateTime now = DateTime.Now;
            DateTime date;
            
            date = now.AddDays(1);
            return date;

        }

        public ActionResult createsub(int? id) {
            ViewBag.id = id;
            
            return View();
        }

        public ActionResult viewsub(int? id)
        {
            List<Task> model = new List<Task>();
            var conn = new NpgsqlConnection();
            conn.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnectionString"].ConnectionString;
            conn.Open();
            var sql = "Select * from tasks where user_id = " + Session["tmid"] + " and parent_task =  '" + id +"'";
            NpgsqlCommand cmd = new NpgsqlCommand(sql, conn);
            var result = cmd.ExecuteScalar();


            if (result != null)
            {
                NpgsqlDataReader reader = cmd.ExecuteReader();
                try
                {
                    while (reader.Read())
                    {
                        model.Add(
                           new Task()
                           {
                               title = reader["title"].ToString(),
                               description = reader["description"].ToString(),
                               notes = reader["notes"].ToString(),
                               category = reader["category"].ToString(),
                               priority = reader["priority"].ToString(),
                               inserted_at = Convert.ToDateTime(reader["inserted_at"].ToString()),
                               status = reader["status"].ToString(),
                               due = Convert.ToDateTime(reader["due"].ToString()),
                               id = Int32.Parse(reader["id"].ToString())



                           });
                    }
                }
                catch (Exception e)
                {

                }
            } 
            else
            {
                ViewBag.Empty = true;  
            }

            conn.Close();
            ViewBag.id = id;
            return View(model);
           
        }
    }
}