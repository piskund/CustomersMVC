﻿namespace Customers.Web.Models
{
    public class Customer 
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Login { get; set; }

        public string Password { get; set; }

        public bool IsDisabled { get; set; }
    }
}