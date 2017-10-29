using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;
using Npgsql;

namespace SMS_ass1.Models
{
    [Table("users", Schema = "public")]
    public class User
    {

        [Key]
        public int id { get; set; }
        public string name { get; set; }
        public string user_name { get; set; }
        public string password { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string address { get; set; }
        public string meta1 { get; set; }
        public string meta2 { get; set; }
        public string zid { get; set; }
        public DateTime inserted_at { get; set; }
        public DateTime updated_at { get; set; }


        public int SaveDetails()
        {


            var conn = new NpgsqlConnection();
            conn.ConnectionString = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnectionString"].ConnectionString;
            conn.Open();
            NpgsqlCommand cmd = new NpgsqlCommand();
            cmd.Connection = conn;
            cmd.CommandText = "INSERT INTO users(name, user_name, password, email, phone, address, zid, inserted_at, updated_at) values ('" + name + "','"  + user_name + "','" + password + "','" + email + "','" + phone +  "','" + address + "','" + zid + "','" + inserted_at + "','" + updated_at + "')";
            cmd.ExecuteNonQuery();
            conn.Close();
            return 1;
        }
    }
}
