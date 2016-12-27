using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using Customers.Web.DAL;

namespace Customers.Web.Models
{
    public class CustomerCreateUpdateViewModel : CustomerViewModel
    {
        public CustomerCreateUpdateViewModel()
        { }

        public CustomerCreateUpdateViewModel(CustomerEntity customer) : base(customer)
        {
            CustomerRoles = Roles.GetRolesForUser(Login);

            var allRoles = RoleNames.GetAllRoles().Select(r => new {
                RoleId = RoleNames.GetRoleIdByName(r),
                RoleName = r
            }).ToList();

            var currentRoles = CustomerRoles.Select(r => new {
                RoleId = RoleNames.GetRoleIdByName(r),
                RoleName = r
            }).ToList();

            RolesMultiSelectList = new MultiSelectList(allRoles, "RoleId", "RoleName", currentRoles);
        }

        [NotMapped]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }

        [NotMapped]
        [Required]
        [Display(Name = "Roles")]
        public IEnumerable<string> CustomerRoles { get; set; }

        [NotMapped]
        [Required]
        [Display(Name = "Roles")]
        public MultiSelectList RolesMultiSelectList { get; set; }

        public static explicit operator CustomerCreateUpdateViewModel(CustomerEntity entity)
        {
            return new CustomerCreateUpdateViewModel(entity);
        }
    }
}