using System;
using System.Web.Security;
using Customers.Web.DAL;
using Customers.Web.Helpers;

namespace Customers.Web.Providers
{
    public class CustomMembershipProvider : MembershipProvider
    {
        public override MembershipUser CreateUser(
            string username, string password, 
            string email, string passwordQuestion, 
            string passwordAnswer, bool isApproved, 
            object providerUserKey, 
            out MembershipCreateStatus status)
        {
            var args = new ValidatePasswordEventArgs(username, password, true);
            OnValidatingPassword(args);

            if (args.Cancel)
            {
                status = MembershipCreateStatus.InvalidPassword;
                return null;
            }

            if (RequiresUniqueEmail && !string.IsNullOrEmpty(GetUserNameByEmail(email)))
            {
                status = MembershipCreateStatus.DuplicateEmail;
                return null;
            }

            MembershipUser user = GetUser(username, true);

            if (user != null)
            {
                // User has already exists in db.
                status = MembershipCreateStatus.DuplicateUserName;
                return null;
            }

            using (var db = new CustomerContext())
            {
                var customer = new CustomerEntity()
                {
                    Login = username,
                    Email = email,
                    Password = password.GetMd5Hash()
                };
                db.Customers.Add(customer);
                db.SaveChanges();
            }

            status = MembershipCreateStatus.Success;

            return GetUser(username, true);
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion,
            string newPasswordAnswer)
        {
            throw new NotImplementedException();
        }

        public override string GetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            throw new NotImplementedException();
        }

        public override string ResetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override void UpdateUser(MembershipUser user)
        {
            throw new NotImplementedException();
        }

        public override bool ValidateUser(string username, string password)
        {
            using (var db = new CustomerContext())
            {
                var user = db.FindCustomerByLoginOrEmail(username);
                return user != null && user.Password.IsHashMatchesToPassword(password);
            }
        }

        public override bool UnlockUser(string username)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            throw new NotImplementedException();
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            MembershipUser result = null;

            using (var db = new CustomerContext())
            {
                var customer = db.FindCustomerByLoginOrEmail(username);
                if (customer != null)
                {
                    result = MapCustomerToMemebershipUser(customer);
                }
            }

            return result;
        }

        private static MembershipUser MapCustomerToMemebershipUser(CustomerEntity customer)
        {
            return new MembershipUser(
                providerName: nameof(CustomMembershipProvider),
                name: customer.Login,
                providerUserKey: customer.Id,
                email: customer.Email,
                passwordQuestion: string.Empty,
                comment: string.Empty,
                isApproved: !customer.IsDisabled,
                isLockedOut: false,
                creationDate: DateTime.UtcNow,
                lastLoginDate: DateTime.UtcNow,
                lastActivityDate: DateTime.UtcNow,
                lastPasswordChangedDate: default(DateTime),
                lastLockoutDate: default(DateTime));
        }

        public override string GetUserNameByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override int GetNumberOfUsersOnline()
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            throw new NotImplementedException();
        }

        public override bool EnablePasswordRetrieval { get; }
        public override bool EnablePasswordReset { get; }
        public override bool RequiresQuestionAndAnswer { get; }
        public override string ApplicationName { get; set; }
        public override int MaxInvalidPasswordAttempts { get; }
        public override int PasswordAttemptWindow { get; }
        public override bool RequiresUniqueEmail { get; }
        public override MembershipPasswordFormat PasswordFormat { get; }
        public override int MinRequiredPasswordLength { get; } = 12;
        public override int MinRequiredNonAlphanumericCharacters { get; }
        public override string PasswordStrengthRegularExpression { get; }
    }
}