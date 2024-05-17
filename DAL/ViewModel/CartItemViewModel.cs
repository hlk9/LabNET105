﻿using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.ViewModel
{
    public class CartItemViewModel
    {
        public Product Product { get; set; }


       
   
        public int Quantity { get; set; }

        public double TotalPrice {  get; set; }

    }
}
