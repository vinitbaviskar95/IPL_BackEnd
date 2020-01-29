using FinalProject.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Cors;

namespace FinalProject.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class RoleController : ApiController
    {
        FinalProject_DBEntities dalobj = new FinalProject_DBEntities();
       
        public RoleController()
        {
            dalobj.Configuration.ProxyCreationEnabled = false;
        }
        // GET: api/Role
        public IEnumerable<T_Roles> Get()
        {
            return (dalobj.T_Roles.ToList());
        }

        // GET: api/Role/5
        public T_Roles Get(int id)
        {
            return (dalobj.T_Roles.Find(id));
        }

        // POST: api/Role
        public void Post([FromBody]T_Roles r)
        {
            dalobj.proc_AddRole(r.RoleName);
            dalobj.SaveChanges();
        }

        // PUT: api/Role/5
        public void Put(int id, [FromBody]T_Roles r)
        {
            dalobj.proc_ModifyRole(id, r.RoleName);
        }

        // DELETE: api/Role/5
        public void Delete(int id)
        {
            dalobj.proc_RemoveRole(id);
            dalobj.SaveChanges();
        }
    }
}
