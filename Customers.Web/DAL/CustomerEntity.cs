namespace Customers.Web.DAL
{
    public class CustomerEntity 
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerEntity"/> class.
        /// Default consturctor.
        /// </summary>
        public CustomerEntity()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerEntity"/> class.
        /// Copy constructor.
        /// </summary>
        /// <param name="customer">The customer.</param>
        public CustomerEntity(CustomerEntity customer)
        {
            Id = customer.Id;
            FirstName = customer.FirstName;
            LastName = customer.LastName;
            Email = customer.Email;
            PhoneNumber = customer.PhoneNumber;
            Login = customer.Login;
            Password = customer.Password;
            IsDisabled = customer.IsDisabled;
        }

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