using IntroSE.Kanban.Backend.DataAccessLayer;
using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.RegularExpressions;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    public class User
    {
        public bool isLogin { get; set; }
        public string _Email { get; set; }
        public string _Password { get; set; }
        public UserDTO UserDTO { get; set; }

        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);


        public User(string email, string password)
        {
            _Password = password;
            _Email = email;
            isLogin = true;
            UserDTO = new UserDTO(email, password, isLogin);

            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            log.Info("Starting new Task log!");
        }

        public User(UserDTO userDto)
        {
            _Password = userDto._Password;
            _Email = userDto._Email;
            isLogin = userDto._IsLogin;
            UserDTO = userDto;
        }

        /// <summary>
        /// Logs in a user to the system
        /// </summary>
        /// <param name="password">User's password</param>
        /// <returns></returns>
        public void Login(string password)
        {
            if (!_Password.Equals(password))
                throw new Exception("Incorrect password");
            isLogin = true;
            UserDTO._IsLogin = true;
            log.Info("User logged in succesfully");
        }
        /// <summary>
        /// Sets the isLogin values to false.
        /// </summary>
        public void LogOut()
        {
            isLogin = false;
            UserDTO._IsLogin = false;
            log.Info("User logged out succesfully");
        }
        /// <summary>
        /// verify that a user has logged in.
        /// </summary>
        public void verifyLogin()
        {
            if (!isLogin)
                throw new Exception($"The user (with the email: {_Email}) is not logged in");
        }

    }
}