using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EWebStore.Models
{
    public class AddressLists
    {
        public int id {get;set;} 
        public string line {get;set;}
        public string city {get;set;}
        public string state {get;set;}
        public int code {get;set;}              
    }
}