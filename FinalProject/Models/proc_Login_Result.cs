//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace FinalProject.Models
{
    using System;
    
    public partial class proc_Login_Result
    {
        public int UserId { get; set; }
        public string EmailId { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string MobileNo { get; set; }
        public bool IsOnline { get; set; }
        public bool IsLocked { get; set; }
        public int RoleId { get; set; }
    }
}