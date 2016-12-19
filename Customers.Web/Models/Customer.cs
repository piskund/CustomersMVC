using System.ComponentModel.DataAnnotations;

namespace Customers.Web.Models
{
    public partial class Customer 
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        [Required]
        public string Email { get; set; }

        [Display(Name = "Phone")]
        public string PhoneNumber { get; set; }

        [Required]
        public string Login { get; set; }

        [Required]
        public string Password { get; set; }

        public bool IsDisabled { get; set; }
    }
}