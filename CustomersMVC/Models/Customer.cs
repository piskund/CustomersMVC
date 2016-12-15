using System.Collections.Generic;

namespace CustomersMVC.Models
{
    public class Customer
    {
        public int ID { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string PhoneNumner { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public bool IsDisabled { get; set; }

        public virtual ICollection<CustomerRole> CustomerRoles { get; set; }
    }
}