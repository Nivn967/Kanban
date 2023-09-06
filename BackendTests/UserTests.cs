using IntroSE.Kanban.Backend.ServiceLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace BackendTests
{

    public class UserTests
    {
        private readonly User user1;

        public UserTests(User user1)
        {
            this.user1 = user1;
        }
        public void Tests()
        {

            RegisterTest();
            LoginLogOutTest();

        }

        /// <summary>
        /// This function tests register option for a new user.
        /// </summary>
        /// /// <returns></returns>
        public void RegisterTest()
        {
            // register new user
            string register1 = user1.Register("niv@gmail.com", "Niv123");
            Response res1 = JsonSerializer.Deserialize<Response>(register1);

            if (res1.ErrorOccured)
            {
                Console.WriteLine(res1.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Created user successfully"); //should succeed 
            }

            // register new user
            string register2 = user1.Register("tomer@gmail.com", "Tomer123");
            Response res2 = JsonSerializer.Deserialize<Response>(register2);

            if (res2.ErrorOccured)
            {
                Console.WriteLine(res2.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Created user successfully"); //should succeed 
            }


            // verify new user can't use an email that already exist. 
            string register3 = user1.Register("niv@gmail.com", "Niv1234");
            Response res3 = JsonSerializer.Deserialize<Response>(register3);

            if (res3.ErrorOccured)
            {
                Console.WriteLine(res3.ErrorMessage); //should return an error message
            }
            else
            {
                Console.WriteLine("Created user successfully");
            }

            // Verify a valid password must in the length 6 to 20. 
            string register4 = user1.Register("tomer@gmail.com", "Niv");
            Response res4 = JsonSerializer.Deserialize<Response>(register4);

            if (res4.ErrorOccured)
            {
                Console.WriteLine(res4.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Created user successfully"); //should return an error message
            }

            // Verify a valid password must include a number, at least one uppercase letter and one small character. 
            string register5 = user1.Register("tomer@gmail.com", "tomer1");
            Response res5 = JsonSerializer.Deserialize<Response>(register5);

            if (res5.ErrorOccured)
            {
                Console.WriteLine(res5.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Created user successfully"); //should return an error message
            }


            // Verify a user must register with a valid email.
            string register6 = user1.Register("tomer@", "Tomer1");
            Response res6 = JsonSerializer.Deserialize<Response>(register6);

            if (res6.ErrorOccured)
            {
                Console.WriteLine(res6.ErrorMessage);//should return an error message
            }
            else
            {
                Console.WriteLine("Created user successfully");
            }

            // register new user.
            string register7 = user1.Register("nitzki@gmail.com", "Nitzki012");
            Response res7 = JsonSerializer.Deserialize<Response>(register7);

            if (res7.ErrorOccured)
            {
                Console.WriteLine(res7.ErrorMessage);// should succeed.
            }
            else
            {
                Console.WriteLine("Created user successfully");
            }

        }


        /// <summary>
        /// This function tests logging in and logging out of a user.
        /// </summary>
        /// /// <returns></returns>
        public void LoginLogOutTest()
        {

            //Verify that a user can login twice
            string login1 = user1.Login("niv@gmail.com", "Niv123");
            Response res1 = JsonSerializer.Deserialize<Response>(login1);

            if (res1.ErrorOccured)
            {
                Console.WriteLine(res1.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Login user successfully");  //should succeed
            }

            // logging out a logged in user.
            string logout1 = user1.LogOut("niv@gmail.com");
            Response res2 = JsonSerializer.Deserialize<Response>(logout1);

            if (res2.ErrorOccured)
            {
                Console.WriteLine(res2.ErrorMessage); //should succeed
            }
            else
            {
                Console.WriteLine("Logout user successfully");
            }

            // can't logging out a user that have been logged out.
            string logout2 = user1.LogOut("niv@gmail.com");
            Response res3 = JsonSerializer.Deserialize<Response>(logout2);

            if (res3.ErrorOccured)
            {
                Console.WriteLine(res3.ErrorMessage); // should return an error message.
            }
            else
            {
                Console.WriteLine("Logout user successfully");
            }


            //try to login with wrong password.
            string login2 = user1.Login("niv@gmail.com", "Niv12345");
            Response res4 = JsonSerializer.Deserialize<Response>(login2);

            if (res4.ErrorOccured)
            {
                Console.WriteLine(res4.ErrorMessage); // should return an error message.
            }
            else
            {
                Console.WriteLine("Login user successfully");
            }

            //logging in a user that is logged out.
            string login3 = user1.Login("niv@gmail.com", "Niv123");
            Response res5 = JsonSerializer.Deserialize<Response>(login3);

            if (res5.ErrorOccured)
            {
                Console.WriteLine(res5.ErrorMessage); //should succeed
            }
            else
            {
                Console.WriteLine("Login user successfully");
            }


            // try to loging out an email that have not registered.
            string logout3 = user1.LogOut("lior@gmail.com");
            Response res6 = JsonSerializer.Deserialize<Response>(logout3);

            if (res6.ErrorOccured)
            {
                Console.WriteLine(res6.ErrorMessage); // should return an error message.
            }
            else
            {
                Console.WriteLine("Logout user successfully");
            }

            // trying to logout with unvalid email. 
            string logout4 = user1.LogOut("nitz2&");
            Response res7 = JsonSerializer.Deserialize<Response>(logout4);

            if (res7.ErrorOccured)
            {
                Console.WriteLine(res7.ErrorMessage); //should return an error message
            }
            else
            {
                Console.WriteLine("Logout user successfully");
            }

        }
    }

}
