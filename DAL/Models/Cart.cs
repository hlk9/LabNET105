using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public Guid AccountId { get; set; }
        public virtual Account Account { get; set; }
        public virtual ICollection<CartItem> CartItems { get; set; }
        public byte Status { get; set; }
       
    }
}
