using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using log4net;
using log4net.Config;
using System.IO;
using System.Reflection;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class User
    {
        public BusinessLayer.User user;
        public BusinessLayer.UserController userController;
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public User(BusinessLayer.UserController userController)
        {
            this.userController = userController;
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            log.Info("Starting new board log!");
        }

        /// <summary>
        /// Load the users from the db to the ram.
        /// </summary>
        /// <returns>json string of Response object</returns>
        public string LoadUsers()
        {
            JsonSerializerOptions options1 = new JsonSerializerOptions { WriteIndented = true };
            Response res;
            string objectAsJson;
            try
            {
                userController.LoadUsers();
                log.Info("Users loaded successfully");
            }
            catch (Exception e)
            {
                res = new Response(e.Message, null);
                log.Error(e.Message);
                objectAsJson = JsonSerializer.Serialize(res, options1);
                return objectAsJson;
            }
            res = new Response(null, null);
            objectAsJson = JsonSerializer.Serialize(res, options1);
            return objectAsJson;
        }

        /// <summary>
        /// delete the users that came from the db.
        /// </summary>
        /// <returns>json string of Response object</returns>
        public string deleteUsers()
        {
            JsonSerializerOptions options1 = new JsonSerializerOptions { WriteIndented = true };
            Response res;
            string objectAsJson;
            try
            {
                userController.DeleteUsers();
                log.Info("Users deleted successfully");
            }
            catch (Exception e)
            {
                res = new Response(e.Message, null);
                log.Error(e.Message);
                objectAsJson = JsonSerializer.Serialize(res, options1);
                return objectAsJson;
            }
            res = new Response(null, null);
            objectAsJson = JsonSerializer.Serialize(res, options1);
            return objectAsJson;
        }


        /// <summary>
        /// Registers a user in the system
        /// </summary>
        /// <param name="email">User's email. Assumed not to be registered yet</param>
        /// <param name="password">User's password</param>
        /// <returns>json string of Response object</returns>
        public string Register(string email, string password)
        {
            JsonSerializerOptions options1 = new JsonSerializerOptions { WriteIndented = true };

            Response res;
            string objectAsJson;
            try
            {
                userController.Register(email, password);
                log.Info("Registered user successfully");
            }
            catch (Exception e)
            {
                res = new Response(e.Message, null);
                log.Error(e.Message);
                objectAsJson = JsonSerializer.Serialize(res, options1);
                return objectAsJson;
            }

            res = new Response(null, null);
            objectAsJson = JsonSerializer.Serialize(res, options1);
            return objectAsJson;
        }

        /// <summary>
        /// Login a user to the system
        /// </summary>
        /// <param name="email">User's email. Assumed to be registered</param>
        /// <param name="password">User's password</param>
        /// <returns> json string of Response object</returns>
        public string Login(string email, string password)
        {
            string objectAsJson;
            JsonSerializerOptions options1 = new JsonSerializerOptions { WriteIndented = true };

            try
            {
                user = userController.getUser(email);
                user.Login(password);
                log.Info("User logged in successfully");
            }
            catch (Exception e)
            {
                Response res = new Response(e.Message, null);
                log.Error(e.Message);
                objectAsJson = JsonSerializer.Serialize(res, options1);
                return objectAsJson;
            }

            Response<string> res2 = new Response<string>(null, user._Email);
            objectAsJson = JsonSerializer.Serialize(res2, options1);
            return objectAsJson;
        }

        /// <summary>
        /// Logout a user from the system.
        /// </summary>
        /// <param name="email">User's email. Assumed to be registered</param>
        /// <returns> json string of Response object</returns>
        public string LogOut(string email)
        {
            Response res;
            string objectAsJson;
            JsonSerializerOptions options1 = new JsonSerializerOptions { WriteIndented = true };

            try
            {
                user = userController.getUser(email);
                user.verifyLogin();
                user.LogOut();
                log.Info("User logged out successfully");
            }
            catch (Exception e)
            {
                res = new Response(e.Message, null);
                log.Error(e.Message);
                objectAsJson = JsonSerializer.Serialize(res, options1);
                return objectAsJson;
            }

            res = new Response(null, null);
            objectAsJson = JsonSerializer.Serialize(res, options1);
            return objectAsJson;
        }
    }
}