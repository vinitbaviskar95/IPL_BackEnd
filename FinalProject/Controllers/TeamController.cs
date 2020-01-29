using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using FinalProject.Models;
using MyLoggerLib;

namespace FinalProject.Controllers
{
    public class TeamController : ApiController
    {
        FinalProject_DBEntities obj = new FinalProject_DBEntities();


        public TeamController()
        {
            obj.Configuration.ProxyCreationEnabled = false;
        }

        [Route("api/Team/TeamList")]
        public Response Get()
        {
            Response response = new Response();
            var teamlist = obj.T_Teams.ToList();
            if (teamlist != null)
            {
                response.Data = teamlist;
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

        //get api/Team/id
        public Response Get(int id)
        {
            Response response = new Response();
            T_Teams t = obj.T_Teams.Find(id);
            if (t != null)
            {
                response.Data = t;
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
        [Route("api/Team/AddTeam")]
        public Response AddPlayers([FromBody]T_Teams t)
        {
            Response response = new Response();
            if (t != null)
            {
                obj.T_Teams.Add(t);
                FileLogger logger = FileLogger.GetLogger();
                logger.Log("Team : " + t.TeamName + " added Successfully inside Online IPL Auction");
                obj.SaveChanges();
                response.Error = null;
                response.Status = "Success";
                return response;
            }
            else
            {
                response.Error = null;
                response.Status = "Error";
                return response;
            }
        }

        [HttpPut]
        //Get api/Team/
        public Response Put([FromBody]T_Teams u)
        {
            Response res = new Response();
            T_Teams u1 = obj.T_Teams.Find(u.TeamId);
            if (u1 != null)
            { 
                u1.TeamName = u.TeamName;
                u1.UserId = u.UserId;
                obj.SaveChanges();
                res.Data = u;
                res.Error = null;
                res.Status = "Success";
                return res;
            }
            else
            {
                res.Error = null;
                res.Status = "Enter valid Id ...";
                return res;
            }
        }

        [HttpDelete]
        //Get api/Team/id
        public Response Delete(int id)
        {
            Response response = new Response();

            obj.T_Teams.Remove(obj.T_Teams.Find(id));
            obj.SaveChanges();
            response.Error = null;
            response.Status = "Success";
            return response;
        }
    }
}
