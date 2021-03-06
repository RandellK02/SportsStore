﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace SportsStore.Domain.Entities
{
    public class ShippingDetails
    {
        [Required(ErrorMessage = "Please enter a name")]
        public String Name { get; set; }

        [Required(ErrorMessage = "Please enter the first address line")]
        [Display(Name = "Line1")]
        public String Line1 { get; set; }
        [Display( Name = "Line 2" )]
        public String Line2 { get; set; }
        [Display( Name = "Line 3" )]
        public String Line3 { get; set; }

        [Required(ErrorMessage = "Please enter a city name")]
        public String City { get; set; }

        [Required(ErrorMessage = "Please enter a state name")]
        public String State { get; set; }

        [Required(ErrorMessage = "Please enter a country name")]
        public String Country { get; set; }

        public bool GiftWrap { get; set; }
        public String Zip { get; set; }

    }
}
