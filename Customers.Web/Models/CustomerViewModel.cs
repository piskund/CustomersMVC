using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using Customers.Web.DAL;

namespace Customers.Web.Models
{
    public class CustomerViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerEntity"/> class.
        /// Default consturctor.
        /// </summary>
        public CustomerViewModel()
        { }

        /// <summary>
        /// Initializes a new instance of the <see cref="CustomerEntity"/> class.
        /// Copy constructor.
        /// </summary>
        /// <param name="customer">The customer.</param>
        public CustomerViewModel(CustomerEntity customer)
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

        [HiddenInput(DisplayValue = false)]
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        [DataType(DataType.EmailAddress)]
        [StringLength(50)]
        [RegularExpression(@"[A-Za-z0-9._%+-]+@[A-Za-z0-9.-]+\.[A-Za-z]{2,4}", ErrorMessage = "Wrong email format")]
        public string Email { get; set; }

        [Display(Name = "Phone")]
        [StringLength(50)]
        [RegularExpression(@"^[+]\d{1,3}\d{1,3}\d{4,8}$")]
        public string PhoneNumber { get; set; }

        [Required]
        [StringLength(15, MinimumLength = 4, ErrorMessage = "Login must contains from 4 to 15 symbols")]
        [RegularExpression(@"[A-Za-z0-9\\._]{4,15}", ErrorMessage = "Wrong login format")]
        public string Login { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(12, MinimumLength = 6, ErrorMessage = "Password must contains from 6 to 12 symbols")]
        // Validation at least 1 lowercase, 1 uppercase, 1 number and 1 special character. 
        [RegularExpression(@"^.*(?=.*[a - z])(?=.*[A - Z])(?=.*\d)(?=.*[@#$%&_\\.]).*$", ErrorMessage = "Wrong password format")]
        public string Password { get; set; }

        public bool IsDisabled { get; set; }

        public bool IsChecked { get; set; }

        [Display(Name = "Name")]
        public string FullName => $"{FirstName} {LastName}";

        [Display(Name = "Active")]
        public bool IsActive
        {
            get { return !IsDisabled; }
            set { IsDisabled = !value; }
        }

        public static explicit operator CustomerViewModel(CustomerEntity entity)
        {
            return new CustomerViewModel(entity);
        }

        public static explicit operator CustomerEntity(CustomerViewModel model)
        {
            return new CustomerEntity()
            {
                Id = model.Id,
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                PhoneNumber = model.PhoneNumber,
                Login = model.Login,
                Password = model.Password,
                IsDisabled = model.IsDisabled,
            };
        }
    }
}