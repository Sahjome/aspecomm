using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EWebStore.Models
{
    public class UserDetails
    {
        public int ID { get; set; }
        public string Firstname { get; set; }
        public string Role { get; set; }
        public string Lastname { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public long Phone { get; set; }
        public string Sex { get; set; }
        public string Trans_ID { get; set; }
       
    }
}