using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Customers.Web.Models
{
    public partial class Customer
    {
        [NotMapped]
        public bool IsChecked { get; set; }

        [NotMapped]
        [Display(Name = "Name")]
        public string FullName => $"{FirstName} {LastName}";

        [NotMapped]
        [Display(Name = "Active")]
        public bool IsActive => !IsDisabled;
    }
}