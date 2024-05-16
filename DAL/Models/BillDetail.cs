using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class BillDetail
    {
        public int Id { get; set; }
        public int BillId { get; set; }
        public virtual Bill Bill { get; set; }
        public int ProductId { get; set; }
        public virtual Product Product { get; set; }
        public int Quantity { get; set; }
        public double Price { get; set; }
       public byte Status { get; set; }
    }
}
