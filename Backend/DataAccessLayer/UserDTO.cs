using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    public class UserDTO : DTO
    {
        public const string UserEmailColumnName = "Email";
        public const string UserPasswordColumnName = "Password";
        public const string UserIsLoginColumnName = "IsLogin";



        private string Email;
        public string _Email { get => Email; }
        private string Password;
        public string _Password { get => Password; }
        private bool IsLogin;
        public bool _IsLogin { get => IsLogin; set { IsLogin = value; ((UserDalController)_Controller).Update(_Email, UserIsLoginColumnName, value); } }

        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public UserDTO(string email, string password, bool isLogin) : base(new UserDalController())
        {
            Email = email;
            Password = password;
            IsLogin = isLogin;
        }
        /// <summary>
        /// Insert this UserDTO to the DataBase.
        /// </summary>
        public bool InsertUser()
        {
            if (((UserDalController)_Controller).InsertUser(this, UserEmailColumnName, UserPasswordColumnName, UserIsLoginColumnName))
            {
                log.Info($"Inserted user to DB");
                return true;
            }
            throw new AggregateException($"error when try to insert user to 'Users' database.  email:'{Email}'");

        }
    }
}
