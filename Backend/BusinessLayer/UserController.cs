using IntroSE.Kanban.Backend.DataAccessLayer;
using log4net;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text.RegularExpressions;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    public class UserController
    {
        public readonly Dictionary<string, User> _users;
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public UserController()
        {
            _users = new Dictionary<string, User>();
        }

        /// <summary>
        /// Load all the users from the DataBase.
        /// </summary>
        public void LoadUsers()
        {
            UserDalController duc = new UserDalController();
            List<UserDTO> dusers = duc.LoadUsers();
            foreach (UserDTO DU in dusers)
            {
                User user = new User(DU);
                _users[user._Email] = user;
            }

            log.Info($"Users loaded from db successfully");
        }
        /// <summary>
        /// Delete all the users from the DataBase. 
        /// </summary>
        public void DeleteUsers()
        {
            UserDalController duc = new UserDalController();
            if (duc.DeleteAll())
            {
                _users.Clear();
                log.Info("Users deleted successfully");
            }
            else
                log.Error($"Cant delete users from DAL");
        }
        /// <summary>
        /// Register a new user to the system.
        /// </summary>
        /// <param name="email">The email address of the user</param>
        /// <param name="password">The password of the user</param>
        internal void Register(string email, string password)
        {
            verifyDetails(email, password);
            if (userExists(email))
            {
                throw new Exception($"Email {email} already exists");
            }
            User user = new User(email, password);
            user.isLogin = true;
            _users.Add(email, user); //adding to dictionary of existing users
            InsertUserToDal(user.UserDTO);
            log.Info($"A new user has created: {email} and was inserted to the DB");
        }
        /// <summary>
        /// Insert User to DB
        /// </summary>
        /// <param name="du">The Dal User of User object</param>
        public void InsertUserToDal(UserDTO du)
        {
            try
            {
                if (du == null)
                {
                    log.Error($"Cant insert user to DB");
                    throw new ArgumentException("Try to add to DB a Null Object");
                }
                du.InsertUser();
                log.Info($"User {du._Email} insert to DB");
            }
            catch (Exception e)
            {
                log.Error(e.ToString());
            }
        }
        /// <summary>
        /// verify that the email and the password of a user are valid.
        /// <param name="email">a string with the email of a user.</param>
        /// /// <param name="password">a string with the password of a user.</param>
        /// </summary>
        internal void verifyDetails(string email, string password)
        {
            if (email is null)
                throw new Exception($"Email cannot be null");

            if (password is null)
                throw new Exception("Password cannot be null");

            Regex rx1 = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Regex rx2 = new Regex(@"^\w+([.-]?\w+)@\w+([.-]?\w+)(.\w{2,3})+$");

            Match match1 = rx1.Match(email);
            Match match2 = rx2.Match(email);

            if (!match1.Success || !match2.Success)
                throw new Exception("Email is invalid");

            if (password.Length < 6 || password.Length > 20)
                throw new Exception("A valid password is in the length of 6 to 20 characters");

            var hasNumber = new Regex(@"[0-9]+");
            var hasUpperChar = new Regex(@"[A-Z]+");
            var hasLowerChar = new Regex(@"[a-z]+");

            if (!hasNumber.IsMatch(password) || !hasUpperChar.IsMatch(password) || !hasLowerChar.IsMatch(password))
                throw new Exception("Invalid password. Password must contain at least one uppercase letter, one lowecase letter and a number");

        }

        /// <summary>
        /// checks if the user exist in the users dictionary, which means he have been registered.
        /// <param name="email">The email of the user.</param>
        /// <summary>
        internal bool userExists(string email)
        {
            return _users.ContainsKey(email);
        }
        /// <summary>
        /// if the user exist, the function return the user with the email given.
        /// <param name="email">The email of the user.</param>
        /// <summary>
        internal User getUser(string email)
        {
            if (userExists(email))
                return _users[email];
            throw new Exception($"The user (with the email: {email}) has not registered yet");
        }
    }
}