using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

using System.Web.Mvc;
using Npgsql;
using System.Security.Cryptography;
using System.Text;
using System.Data;
using System.Data.Entity;


namespace SMS_ass1.Models
{
    [Table("tasks", Schema = "public")]
    public class Task
    {
        [Key]
        public int id { get; set; }
        public int user_id { get; set; }
        public string title { get; set;  }
        public string description { get; set; }
        public string category { get; set; }
        public DateTime due { get; set; }
        public string notes { get; set; }
        public string priority { get; set; }
        public string re_dates { get; set; }
        public string status { get; set; }
        public DateTime rec_date { get; set; }
        public string zid { get; set; }
        public DateTime inserted_at { get; set; }
        public DateTime updated_at { get; set; }
        public int parent_task { get; set; }



        public int SaveDetails()
        {


            var conn = new NpgsqlConnection();
            conn.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnectionString"].ConnectionString;
            conn.Open();
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "INSERT INTO tasks(title, status, description, category, due, rec_date, notes, priority,re_dates, user_id, parent_task, zid, inserted_at, updated_at) values ('" + title + "','" + status+ "','" + description + "','" + category + "','" + due + "','" + rec_date + "','"  + notes + "','" + priority + "','" + re_dates+ "','" + user_id + "','" + parent_task + "','" + zid + "','" + inserted_at + "','" + updated_at + "')";
            cmd.ExecuteNonQuery();
            conn.Close();
            return 1;
        }
        public int UpdateDetails()
        {


            var conn = new NpgsqlConnection();
            conn.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnectionString"].ConnectionString;
            conn.Open();
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "Update tasks set re_dates = '" + re_dates + "', title = '" + title + "', status = '" + status + "', description = '" + description + "', category = '" + category + "', due = '" + due + "',  notes = '" + notes + "', priority = '" + priority + "', updated_at = '" + updated_at + "' where id = " + id + " ";

            //cmd.CommandText = "Update tasks set title = '" + title + "',  priority = " + priority + " where id = 1 ";

            cmd.ExecuteNonQuery();
            conn.Close();
            return 1;
        }
        
    }

}