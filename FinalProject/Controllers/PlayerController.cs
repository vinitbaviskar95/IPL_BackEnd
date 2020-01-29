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
    public class PlayerController : ApiController
    {
      
        FinalProject_DBEntities obj = new FinalProject_DBEntities();


            [HttpGet]
            [Route("api/Player/PlayerList")]
            public Response Get()
            {
                Response response = new Response();
                var playerlist = obj.T_Players.ToList();
                if (playerlist != null)
                {
                    response.Data = playerlist;
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

            //get api/Player/id
            public Response Get(int id)
            {
                Response response = new Response();
                T_Players t = obj.T_Players.Find(id);
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
            [Route("api/Player/AddPlayer")]
            public Response AddPlayers([FromBody]T_Players p)
            {

                Response response = new Response();
                if (p != null)
                {

                    obj.T_Players.Add(p);
                    FileLogger logger = FileLogger.GetLogger();
                    logger.Log("Player :"+p.Name +" added Successfully inside Online IPL Auction");
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

            //Get api/Player/
            [HttpPut]
    
            public Response Put([FromBody]T_Players t)
            {
                Response res = new Response();
                T_Players t1 = obj.T_Players.Find(t.PlayerId);

                if(t1 !=null)
                {       
                    t1.Name = t.Name;
                    t1.Age = t.Age;
                    t1.Type = t.Type;
                    t1.Matches = t.Matches;
                    t1.Runs = t.Runs;
                    t1.Wickets = t.Wickets;
                    t1.BasePrice = t.BasePrice;
                    obj.SaveChanges();
                
                    res.Data = t;
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

            //Get api/Player/id
            public Response Delete(int id)
            {
                Response response = new Response();

                obj.T_Players.Remove(obj.T_Players.Find(id));
                obj.SaveChanges();
                response.Error = null;
                response.Status = "Success";
                return response;
            }
     }
}


