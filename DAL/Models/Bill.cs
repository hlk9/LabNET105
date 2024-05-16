using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.Models
{
    public class Bill
    {
        public int Id { get; set; }
        public Guid AccountId { get; set; }
        public virtual Account Account { get; set; }
        public byte Status { get; set; }
        public virtual ICollection<BillDetail> BillDetails { get; set; }

    }
}
