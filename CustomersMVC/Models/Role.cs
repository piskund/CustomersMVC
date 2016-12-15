using System.Collections.Generic;

namespace CustomersMVC.Models
{
    public class Role
    {
        public int ID { get; set; }

        public string Name { get; set; }

        public virtual ICollection<CustomerRole> CustomerRoles { get; set; }
    }
}