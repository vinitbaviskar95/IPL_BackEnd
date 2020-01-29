using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Net.Mail;

using FinalProject.Models;
using System.Data.SqlClient;
using MyLoggerLib;

namespace FinalProject.Controllers
{
    public class BidDetails
    {
        public int PlayerId { get; set; }
        public String  PlayerName { get; set; }
        public int UserId { get; set; }
        public String UserName { get; set; }
        public double Amount { get; set; }

    }

    public class BidController : ApiController
    {
        FinalProject_DBEntities obj = new FinalProject_DBEntities();

        BidController()
        {
            obj.Configuration.ProxyCreationEnabled = false;
        }


        // GET: api/Bid
        //http://localhost:52276/api/Bid
        public IEnumerable<BidDetails> Get()
        {
            string query = "select T_Players.PlayerId,T_Players.Name as PlayerName,T_Users.UserId,T_Users.Name as UserName, Amount from T_Users,T_Bid,T_Players where T_Bid.PlayerId = T_Players.PlayerId AND T_Users.UserId = T_Bid.UserId order by Amount DESC";
            
            var result = obj.Database.SqlQuery<BidDetails>(query).ToList();
            return result;
            
        }

        //GET: api/Bid/5
        public Response Get(int id)
        {
            Response response = new Response();
            var bidPlayer = obj.T_Bid.Find(id);
            if (bidPlayer != null)
            {
                response.Data = bidPlayer;
                response.Error = null;
                response.Status = "Success";
            }
            else
            {
                response.Data = null;
                response.Error = null;
                response.Status = "Failed";

            }

            return response;
        }

        // POST: api/Bid
        public Response Post([FromBody]T_Bid bid)
        {
            var player = obj.T_Players.Find(bid.PlayerId);
            if (player != null)
            {
                player.Status = true;
                obj.SaveChanges();
            }

            Response response = new Response();
            if (bid != null)
            {
                var Team = obj.T_Bid.Add(bid);
                obj.SaveChanges();

                var Userdetails = obj.T_Users.ToList();
                var bidusers = (from u in Userdetails
                                where u.UserId == bid.UserId
                                select u).SingleOrDefault();

                Email em = new Email();
                em.to = bidusers.EmailId;
                em.subject = "Confirmation Mail";
                em.body = "Hello "+ bidusers.Name + " You have Successfully Purchased Player with PlayerId : " +  Team.PlayerId;
                FileLogger logger = FileLogger.GetLogger();
                logger.Log(bidusers.Name + " have Successfully purchased in Online IPL Auction");
                obj.SaveChanges();
                var mailresult = SendInvoice(em);

                response.Status = "Success";
                response.Error = null;
                response.Data = Team;
                return response;

            }
            else
            {
                response.Status = "Failed";
                response.Error = null;
                response.Data = null;
                return response;
            }
        }

        // PUT: api/Bid/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Bid/5
        public void Delete(int id)
        {
        }

        [HttpPost]
        [Route("api/Bid/SendInvoice")]
        public Response SendInvoice(Email e)
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



