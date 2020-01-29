using FinalProject.Models;
using MyLoggerLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Web.Http;

namespace FinalProject.Controllers
{
    public class AccountController : ApiController
    {
        FinalProject_DBEntities dalobj = new FinalProject_DBEntities();

        public AccountController()
        {
            dalobj.Configuration.ProxyCreationEnabled = false;
        }

        [HttpPost]
        [Route("api/Account/OTP")]
        public Response OTP([FromBody]dynamic otpDetails)
        {
            string email = otpDetails.EmailId.ToString();
            var userlist = dalobj.T_Users.ToList();
            var User = (from u in userlist
                        where u.EmailId == email
                        select u).SingleOrDefault();

            string o = otpDetails.OTP.ToString();

            var otpd = dalobj.T_OTP_Details.ToList();
            var vOTP = (from v in otpd
                        where v.UserId == User.UserId && v.OTP == o
                        select v).SingleOrDefault();

            if (vOTP != null)
            {
                Response RC = new Response();
                RC.Status = "success";
                RC.Error = null;
                RC.Data = vOTP;
                return RC;
            }
            else
            {
                Response RC = new Response();
                RC.Status = "fail";
                RC.Error = null;
                RC.Data = false;
                return RC;
            }
        }


        //Change Password
        [HttpPut]
        [Route("api/User/ChangePassword")]
        public Response ChangePassword([FromBody]dynamic value)
        {

            var pass = value.Password.ToString();
            string encpass = null;
            MySecurityLib.Security.Encrypt(pass, out encpass);
            var userlist = dalobj.T_Users.ToList();

            var User = (from u in userlist
                        where u.EmailId == value.EmailId.ToString() && u.Password == encpass
                        select u).SingleOrDefault();
            Response rc = new Response();
            if (User != null)
            {
                var newpass = value.NewPassword.ToString();
                string encnewpass = null;
                MySecurityLib.Security.Encrypt(newpass, out encnewpass);

                User.Password = encnewpass;

                FileLogger logger = FileLogger.GetLogger();
                logger.Log("Users Password Changed Successfully Online IPL Auction");
                dalobj.SaveChanges();
                rc.Status = "Success";
                rc.Error = null;
                rc.Data = User;
                return rc;
            }
            else
            {

                rc.Status = "Fail";
                rc.Error = null;
                rc.Data = null;
                return rc;
            }
        }




        [HttpPost]
        [Route("api/Account/IsExist")]
        public Response IsExist([FromBody]T_Users value)
        {
            var userList = dalobj.T_Users.ToList();
            var User = (from u in userList
                        where u.EmailId == value.EmailId
                        select u).SingleOrDefault();

            if (User != null)
            {
                Response RC = new Response();
                string mails = GetOTP();

                T_OTP_Details otp = new T_OTP_Details();
                otp.UserId = User.UserId;
                otp.GeneratedOn = DateTime.Today;
                otp.ValidTill = DateTime.Now.AddMinutes(5);
                otp.OTP = mails;
                dalobj.T_OTP_Details.Add(otp);

                Email em = new Email();
                em.to = User.EmailId;
                em.subject = "otp";
                em.body = "Your OTP is" + mails;
                dalobj.SaveChanges();
                var mailresult = SendEmail(em);

                RC.Status = "success";
                RC.Error = null;
                RC.Data = mails;
                return RC;
            }
            else
            {
                Response RC = new Response();
                RC.Status = "fail";
                RC.Error = null;
                RC.Data = false;
                return RC;
            }
        }

        [HttpPut]
        [Route("api/Account/UpdatePassword")]
        public Response updatepassword([FromBody]T_Users value)
        {
            var userlist = dalobj.T_Users.ToList();
            var User = (from u in userlist
                        where u.EmailId == value.EmailId
                        select u).SingleOrDefault();

            if (User != null)
            {

                var newpass = value.Password;
                string encnewpass = null;
                MySecurityLib.Security.Encrypt(newpass, out encnewpass);

                User.Password = encnewpass;
                dalobj.SaveChanges();
                Response rc = new Response();
                rc.Status = "success";
                rc.Error = null;
                rc.Data = User;
                return rc;
            }
            else
            {
                Response rc = new Response();
                rc.Status = "fail";
                rc.Error = null;
                rc.Data = null;
                return rc;
            }
        }

        private string GetOTP(string otpType = "1", int len = 5)
        {
            //otptype 1 = alpha numeric
            string alphabets = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string small_alphabets = "abcdefghijklmnopqrstuvwxyz";
            string numbers = "1234567890";
            string characters = numbers;
            if (otpType == "1")
            {
                characters += alphabets + small_alphabets + numbers;
            }
            int length = 5;
            string otp = string.Empty;
            for (int i = 0; i < length; i++)
            {
                string character = string.Empty;
                do
                {
                    int index = new Random().Next(0, characters.Length);
                    character = characters.ToCharArray()[index].ToString();
                } while (otp.IndexOf(character) != -1);
                otp += character;
            }
            return otp;
        }

        [HttpPost]
        [Route("api/Account/SendEmail")]
        public Response SendEmail(Email e)
        {
            Response res = new Response();
            string to = e.to;
            string subject = e.subject;
            string body = e.body;

            string result = "Message Sent Successfully..!!";
            string senderID = "vinit.baviskar95@gmail.com";// use sender’s email id here..
            const string senderPassword = "Vinit7588"; // sender password here…
            try
            {
                SmtpClient smtp = new SmtpClient
                {
                    Host = "smtp.gmail.com", // smtp server address here…
                    Port = 587,
                    EnableSsl = true,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new System.Net.NetworkCredential(senderID, senderPassword),
                    Timeout = 30000,
                };
                MailMessage message = new MailMessage(senderID, to, subject, body);
                smtp.Send(message);
            }
            catch (Exception ex)
            {
                result = "Error sending email.!!!";
                res.Error = ex;
            }
            return res;

        }
    }
}

