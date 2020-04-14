using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace EWebStore.Models
{
    public class Vehicles
    {
        public string brand { get; set; }
        public string model { get; set; }
        public string category { get; set; }
        public int yearmax { get; set; }
        public int yearmin { get; set; }
        public double pricemax { get; set; }
        public double pricemin { get; set; }
    }
}