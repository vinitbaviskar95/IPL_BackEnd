using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Web.Http;
using System.Web;
using System.Net.Http.Formatting;
using FinalProject.Models;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Diagnostics;
using MyLoggerLib;
using MySecurityLib;
using FinalProject.Controllers;


namespace FinalProject.Controllers
{
    
    public class UserController : ApiController
    {
        FinalProject_DBEntities obj = new FinalProject_DBEntities();
        Response res = new Response();



        public UserController()
        {
            obj.Configuration.ProxyCreationEnabled = false;
        }

        [Route("api/User/UserList")]
        public Response Get()
        {
            FileLogger logger = FileLogger.GetLogger();
            logger.Log("Get() from User Controller Called");
            Response response = new Response();
            var listuser = obj.T_Users.ToList();
            var user = (from u in listuser
                        where u.RoleId==2
                        select u).ToList();
            if (user != null)
            {
                response.Data = user;
                response.Error = null;
                response.Status = "Success";
                return response;
            }
            else
            {
                response.Error = null;
                response.Status = "No User Found..";
                return response;
            }
        }

        // GET: api/User/5
        public Response Get(int id)
        {
            Response response = new Response();
            T_Users u = obj.T_Users.Find(id);
            if (u != null)
            {
                response.Data = u;
                response.Error = null;
                response.Status = "Success";
                return response;
            }
            else
            {
                response.Error = null;
                response.Status = "Enter valid Id ...";
                return response;
            }
        }


        [HttpPost]
        [Route("api/User/Registration")]
        public Response Registration([FromBody]T_Users u)
        {
           

            u.RoleId = 2;
            Response response = new Response();
            var pass = u.Password;
            string encryptpass = null;
            MySecurityLib.Security.Encrypt(pass, out encryptpass);
            u.Password = encryptpass;
            var result = obj.T_Users.Add(u);
            obj.SaveChanges();
            if (result != null)
            {
                FileLogger logger = FileLogger.GetLogger();
                logger.Log("User Registerted " + u.Name + " for Online IPL Auction");
                Email mail = new Email();
                mail.to = u.EmailId;
                mail.subject = "Online IPL Auctions";
                mail.body = "Congratulations...! You are successfully Registered as Owner in Online IPL Auctions...  Your Password is :" + pass;
                AccountController acc = new AccountController();
                acc.SendEmail(mail);
                //obj.T_Users.Add(u);
                ////obj.proc_AddRole(u.EmailId, u.Password, u.MobileNo, u.Name);
                //obj.SaveChanges();
                response.Error = null;
                response.Status = "Success";
                return response;

            }
            else
            {
                response.Error = null;
                response.Status = "Please enter valid data..";
                return response;
            }

        }


        [HttpPost]
        [Route("api/User/Login")]
        public Response Login([FromBody]T_Users u)
        {
            Response response = new Response();

            string enc = null;
            MySecurityLib.Security.Encrypt(u.Password, out enc);

            if (u.EmailId != null && u.Password != null)
            {
                var listuser = obj.T_Users.ToList();
                T_Users validuser = (from user in listuser
                                     where user.EmailId == u.EmailId &&
                                     user.Password == enc
                                     select user).SingleOrDefault();
                if (validuser != null)
                {
                    FileLogger logger = FileLogger.GetLogger();
                    logger.Log("User " + validuser.Name + " Logged in Online IPL Auction");
                    statusonline(validuser.UserId);
                    response.Data = validuser;
                    response.Error = null;
                    response.Status = "Success";
                    return response;
                }
                else
                {
                    response.Error = null;
                    response.Status = "Incorrect Credintial";
                    return response;
                }
            }
            else
            {
                response.Error = null;
                response.Status = "Field are empty";
                return response;
            }
        }

        // PUT: api/User/5
        public void Put(int id, [FromBody]T_Users u)
        {
            T_Users u1 = obj.T_Users.Find(id);
            u1.Name = u.Name;
            u1.EmailId = u.EmailId;
            u1.Password = u.Password;
            u1.MobileNo = u.MobileNo;
            u1.IsOnline = u.IsOnline;
            u1.IsLocked = u.IsLocked;
            u1.RoleId = u.RoleId;

            FileLogger logger = FileLogger.GetLogger();
            logger.Log("User " + u.Name + "Updated Details inside Online IPL Auction");
            obj.SaveChanges();
        }

        // DELETE: api/User/5
        public Response Delete(int id)
        {
            Response response = new Response();

            obj.T_Users.Remove(obj.T_Users.Find(id));
            obj.SaveChanges();
            response.Error = null;
            response.Status = "User Deleted Successfully...";
            return response;

        }

        [Route("api/User/OnlineUsers")]
        public Response get()
        {
            Response response = new Response();

            var UserList = obj.T_Users.ToList();
            var List = (from u in UserList
                        where u.IsOnline == true
                        select u).ToList();
            if (List != null)
            {
                response.Data = List;
                response.Error = null;
                response.Status = "Success";
                return response;

            }
            else
            {
                response.Status = "Fail";
                return response;

            }
        }

        public void statusonline(int UserId)
        {

            T_Users user = obj.T_Users.Find(UserId);
            user.IsOnline = true;
            obj.SaveChanges();

        }


        [HttpPut]
        [Route("api/User/statusoffline")]
        public Response statusoffline([FromBody]dynamic value)
        {
            Response response = new Response();
            var userlist = obj.T_Users.ToList();


            var User = (from u in userlist
                        where u.EmailId == value.EmailId.ToString()
                        select u).SingleOrDefault();

            if (User != null)
            {

                User.IsOnline = false;
                obj.SaveChanges();
                response.Status = "Success";
                response.Error = null;
                response.Data = User;
                return response;
            }
            else
            {

                response.Status = "Fail";
                response.Error = null;
                response.Data = null;
                return response;
            }
        }


        

    }
}



