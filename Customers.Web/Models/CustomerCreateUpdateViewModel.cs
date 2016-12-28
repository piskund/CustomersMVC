using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using Customers.Web.DAL;
using Customers.Web.Helpers;

namespace Customers.Web.Models
{
    public class CustomerCreateUpdateViewModel : CustomerViewModel
    {
        public const string TempPassword = "TempPass1.";
        private string _password;

        public CustomerCreateUpdateViewModel()
        {
            var allRoles = RoleNames.GetAllRoleNames().Select(r => new {
                RoleId = RoleNames.GetRoleIdByName(r),
                RoleName = r
            }).ToList();

            RolesList = new MultiSelectList(allRoles, "RoleId", "RoleName", new int[] {});
        }

        public CustomerCreateUpdateViewModel(CustomerEntity customer) : base(customer)
        {
            var customerRoles = Roles.GetRolesForUser(Login);

            var allRoles = RoleNames.GetAllRoleNames().Select(r => new {
                RoleId = RoleNames.GetRoleIdByName(r),
                RoleName = r
            }).ToList();

            var selectedRoles = customerRoles.Select(r => new {
                RoleId = RoleNames.GetRoleIdByName(r),
                RoleName = r
            }).ToList();

            InititallySelectedRoles = selectedRoles.Select(r => r.RoleId).ToArray();
            RolesList = new MultiSelectList(allRoles, "RoleId", "RoleName", InititallySelectedRoles);

            OldHashedPassword = customer.Password;
            // Workaround to deal with hased password that probably don't match pattern.
            _password = TempPassword;
            ConfirmPassword = TempPassword;
        }

        [NotMapped]
        public bool IsPasswordChanged { get; set; }

        public string OldHashedPassword { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(12, MinimumLength = 6, ErrorMessage = "Password must contains from 6 to 12 symbols")]
        [RegularExpression(@"^.*(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@#$%&_\\.]).*$", ErrorMessage = "Make sure that password contains at least 1 lowercase, 1 uppercase, 1 number and 1 special character")]
        public string Password
        {
            get
            {
                return _password;
            }

            set
            {
                _password = value;

                if (value != TempPassword)
                {
                    IsPasswordChanged = true;
                }
            }
        }

        [NotMapped]
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Passwords do not match")]
        public string ConfirmPassword { get; set; }

        [NotMapped]
        [HiddenInput(DisplayValue = false)]
        public int?[] InititallySelectedRoles { get; set; }

        [NotMapped]
        public MultiSelectList RolesList { get; set; }

        [NotMapped]
        public int?[] RoleId { get; set; }
        
        public string GetHashedPassword()
        {
            return IsPasswordChanged ? Password.GetMd5Hash() : OldHashedPassword;
        }

        public static explicit operator CustomerCreateUpdateViewModel(CustomerEntity entity)
        {
            return new CustomerCreateUpdateViewModel(entity);
        }

        public static explicit operator CustomerEntity(CustomerCreateUpdateViewModel model)
        {
            var entity = (CustomerEntity)((CustomerViewModel)model);

            // Transform password to hashed.
            entity.Password = model.GetHashedPassword();

            return entity;
        }
    }
}