using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("AccountId")]
        public Guid AccountId { get; set; }
        public virtual Account Account { get; set; }
        public virtual ICollection<CartItem> CartItems { get; set; }
        public byte Status { get; set; }
       
    }
}
