using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;


namespace SMS_ass1.Controllers
{
    public class SendMailerController : Controller
    {
        //  
        // GET: /SendMailer/   
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ViewResult Index(SMS_ass1.Models.MailModel _objModelMail)
        {
            if (ModelState.IsValid)
            {
                MailMessage mail = new MailMessage();
                mail.To.Add(_objModelMail.To);
                mail.From = new MailAddress(_objModelMail.From);
                mail.Subject = _objModelMail.Subject;
                string Body = _objModelMail.Body;
                mail.Body = Body;
                mail.IsBodyHtml = true;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "smtp.gmail.com";
                smtp.Port = 587;
                smtp.UseDefaultCredentials = false;
               
                smtp.Credentials = new System.Net.NetworkCredential("jodimarietaylor18@gmail.com", "yatidoj1"); // Enter seders User name and password   
               
                smtp.EnableSsl = true;


              smtp.Send(mail);
                return View("Index", _objModelMail);
            }
            else
            {
                return View();
            }
        }
       
    }
}

