using FinalProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Mail;
using System.Web.Http;


namespace FinalProject.Controllers
{
    
    public class LoginController : ApiController
    {
        FinalProject_DBEntities Logindalobj = new FinalProject_DBEntities();
        Response res = new Response();
        public LoginController()
        {
            Logindalobj.Configuration.ProxyCreationEnabled = false;
        }


        [HttpGet]
        [Route("api/Login/PasswordHistory")]
        public IEnumerable<T_PasswordHistoryLog> Get()
        {
            return (Logindalobj.T_PasswordHistoryLog.ToList());
        }

        // POST: api/Login
        public Response Post([FromBody] T_Users login)
        {
            Response response = new Response();
            T_Users Validuser=null;
            if(login.EmailId != null && login.Password != null)
            {
                var UserDetails = Logindalobj.T_Users.ToList();
                 Validuser = (from user in UserDetails
                                    where user.EmailId == login.EmailId 
                                    && user.Password == login.Password
                                    select user).SingleOrDefault();

                if (Validuser != null)
                {
                    response.Data = Validuser;
                    response.Error = null;
                    response.Status = "Success";
                    return response;
                }
                else
                {
                    response.Error = null;
                    response.Status = "InCorrect Data";
                    return response;
                }
                
            }
            else
            {
                response.Error = null;
                response.Status = "Invalid Data";
                return response;
            }
        }

       

    }


}

