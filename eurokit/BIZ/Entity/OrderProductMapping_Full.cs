using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BIZ.Entity
{
    public class OrderProductMapping_Full : DATA.OrderProductMapping
    {
        public string ProductImage { get; set; }

        public double ProductWeight { get; set; }
    }
}
