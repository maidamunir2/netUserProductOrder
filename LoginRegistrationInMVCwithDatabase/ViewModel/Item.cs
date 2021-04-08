using LoginRegistrationInMVCwithDatabase.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LoginRegistrationInMVCwithDatabase.ViewModel
{
    public class Item
    {
        public Product_ Product { get; set; }
        public int Quantity { get; set; }
    }
}