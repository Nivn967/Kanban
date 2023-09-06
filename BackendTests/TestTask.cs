using IntroSE.Kanban.Backend.ServiceLayer;
using System;
using System.Text.Json;

namespace IntroSE.Kanban.Frontend
{

    class TaskTests
    {
        private readonly Backend.ServiceLayer.Task task1;
        public string workingEmail;
        public string workingEmail2;
        public string notWorkingEmail;
        public string board1;
        public string notExistBoard;

        public TaskTests(Backend.ServiceLayer.Task task1)
        {
            this.task1 = task1;
            workingEmail = "niv@gmail.com";  //tasks (0,1) in board1
            workingEmail2 = "tomer@gmail.com";  //taskid 0 asignee
            notWorkingEmail = "nidsadasdasv@gmail.com";
            board1 = "Board1";
            notExistBoard = "unExistBoard";
        }
        public void RunTests()
        {
            AddTaskTest();
            AssignTaskTest();
            AdvanceTaskTest();
            EditTaskDescreptionTest();
            EditTaskTitleTest();
            EditDueDateTest();
        }

        /// <summary>
        /// Verify the ability to add tasks to boards
        /// </summary>
        /// <returns></returns>
        public void AddTaskTest()
        {
            //Add task to an existing board with valid values. should succeed.
            string test1 = task1.AddTask(workingEmail, board1, 0, DateTime.Now.AddDays(10), "Finish work1", "aaa");
            Response res1 = JsonSerializer.Deserialize<Response>(test1);
            if (res1.ErrorOccured)
            {
                Console.WriteLine(res1.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Task added successfully"); //should succeed
            }

            //Add tasks to an unexisting email. should throw exception.
            string test2 = task1.AddTask(notWorkingEmail, board1, 0, DateTime.Now.AddDays(10), "Finish work1", "aaa");
            Response res2 = JsonSerializer.Deserialize<Response>(test2);
            if (res2.ErrorOccured)
            {
                Console.WriteLine(res2.ErrorMessage);  //should return an error message
            }
            else
            {
                Console.WriteLine("Task added successfully");
            }

            //Add tasks to an unexisting board. should throw exception.
            string test3 = task1.AddTask(workingEmail, notExistBoard, 0, DateTime.Now.AddDays(10), "Finish work1", "aaa");
            Response res3 = JsonSerializer.Deserialize<Response>(test3);
            if (res2.ErrorOccured)
            {
                Console.WriteLine(res3.ErrorMessage); //should return an error message
            }
            else
            {
                Console.WriteLine("Task added successfully");
            }

            //Add task to an existing board with valid values. should succeed.
            string test4 = task1.AddTask(workingEmail, board1, 0, DateTime.Now.AddDays(10), "Finish work1", "aaa");
            Response res4 = JsonSerializer.Deserialize<Response>(test4);
            if (res4.ErrorOccured)
            {
                Console.WriteLine(res4.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Task added successfully"); //should succeed
            }

        }

        /// <summary>
        /// Verify the ability to assign a task to a user
        /// </summary>
        /// <returns></returns>
        public void AssignTaskTest()
        {
            //Assign task 0 into unexisting email. Assign should fail.
            string test = task1.AssignTask(workingEmail2, board1, 2, 0, workingEmail);
            Response res = JsonSerializer.Deserialize<Response>(test);
            if (res.ErrorOccured)
            {
                Console.WriteLine(res.ErrorMessage);  //should return an error message
            }
            else
            {
                Console.WriteLine("Task assigned successfully");
            }

            //Assign task 0 into existing email. Assign should succeed
            string test2 = task1.AssignTask(workingEmail2, board1, 0, 0, workingEmail2);
            Response res2 = JsonSerializer.Deserialize<Response>(test2);
            if (res2.ErrorOccured)
            {
                Console.WriteLine(res2.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Task assigned successfully");  //succeed
            }

            //Assign task 0 into existing email. Assign should succeed
            string test3 = task1.AssignTask(workingEmail2, board1, 0, 1, workingEmail2);
            Response res3 = JsonSerializer.Deserialize<Response>(test3);
            if (res3.ErrorOccured)
            {
                Console.WriteLine(res3.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Task assigned successfully");  //succeed
            }
        }

        /// <summary>
        /// Verify the ability to advance a task status
        /// </summary>
        /// <returns></returns>
        public void AdvanceTaskTest()
        {
            //Advance task 0 to inProgress column in unexisting board. Advance should fail.
            string test = task1.AdvanceTask(notExistBoard, 0, 0, workingEmail2);
            Response res = JsonSerializer.Deserialize<Response>(test);
            if (res.ErrorOccured)
            {
                Console.WriteLine(res.ErrorMessage);  //should return an error message
            }
            else
            {
                Console.WriteLine("Task advanced successfully");
            }
            //Advance task 0 to inProgress column in board board1. Advance should succeed.
            string test1 = task1.AdvanceTask(board1, 0, 0, workingEmail2);
            Response res1 = JsonSerializer.Deserialize<Response>(test1);
            if (res1.ErrorOccured)
            {
                Console.WriteLine(res1.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Task advanced successfully");  //succeed
            }

            //Advance task 0 to done column in board board1. Advance should succeed.
            string test2 = task1.AdvanceTask(board1, 0, 1, workingEmail2);
            Response res2 = JsonSerializer.Deserialize<Response>(test2);
            if (res2.ErrorOccured)
            {
                Console.WriteLine(res2.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Task advanced successfully");  //succeed
            }

            //Advance task 0 from done column in board board1. Advance should fail.
            string test4 = task1.AdvanceTask(board1, 0, 2, workingEmail2);
            Response res4 = JsonSerializer.Deserialize<Response>(test4);
            if (res4.ErrorOccured)
            {
                Console.WriteLine(res4.ErrorMessage);  //should return an error message
            }
            else
            {
                Console.WriteLine("Task advanced successfully");
            }
        }


        /// <summary>
        /// Verify the edit of task's descreption correctly by a user
        /// </summary>
        /// <returns></returns>
        public void EditTaskDescreptionTest()
        {
            //Edit task descreption to an empty descreption. the edit should be succesful 
            string desc1 = "";
            string test1 = task1.EditTaskDescription(workingEmail2, board1, desc1, 1, 0);
            Response res1 = JsonSerializer.Deserialize<Response>(test1);
            if (res1.ErrorOccured)
            {
                Console.WriteLine(res1.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Description edit went succesful");  //succeed
            }

            //Edit task descreption to a descreption with over 300 chars. the edit should be unsuccesful 
            string desc3 = "";
            for (int i = 0; i < 301; i++)
            {
                desc3 = desc3 + "1";
            }
            string test3 = task1.EditTaskDescription(workingEmail2, board1, desc3, 1, 0);
            Response res3 = JsonSerializer.Deserialize<Response>(test3);
            if (res3.ErrorOccured)
            {
                Console.WriteLine(res3.ErrorMessage);  //should return an error message
            }
            else
            {
                Console.WriteLine("Description edit went succesful");
            }
        }

        /// <summary>
        /// Verify the edit of task's title correctly by a user
        /// </summary>
        /// <returns></returns>

        public void EditTaskTitleTest()
        {
            //Edit task title to an empty title. the edit should be unsuccesful 
            string titl1 = "";
            string test4 = task1.EditTaskTitle(workingEmail2, board1, titl1, 1, 0);
            Response res4 = JsonSerializer.Deserialize<Response>(test4);
            if (res4.ErrorOccured)
            {
                Console.WriteLine(res4.ErrorMessage);  //should return an error message
            }
            else
            {
                Console.WriteLine("Title edit went succesful");
            }

            //Edit task title should be successful
            string titl2 = "Milesone0";
            string test5 = task1.EditTaskTitle(workingEmail2, board1, titl2, 1, 0);
            Response res5 = JsonSerializer.Deserialize<Response>(test5);
            if (res5.ErrorOccured)
            {
                Console.WriteLine(res5.ErrorMessage); //should return an error message
            }
            else
            {
                Console.WriteLine("Title edit went succesful");
            }

            //Edit task title to a title with over 50 chars. the edit should be unsuccesful 
            string titl3 = "";
            for (int i = 0; i < 51; i++)
            {
                titl3 = titl3 + "1";
            }
            string test6 = task1.EditTaskTitle(workingEmail2, board1, titl3, 1, 0);
            Response res6 = JsonSerializer.Deserialize<Response>(test6);
            if (res6.ErrorOccured)
            {
                Console.WriteLine(res6.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Title edit went succesful"); //succeed
            }
        }

        /// <summary>
        /// Verify the edit of task's due date correctly by a user
        /// </summary>
        /// <returns></returns>
        public void EditDueDateTest()
        {
            //Edit task due date to a future due date. the edit should be succesful 
            DateTime dD1 = DateTime.Now.AddDays(12);
            string test8 = task1.EditTaskDueDate(workingEmail2, board1, dD1, 1, 0);
            Response res8 = JsonSerializer.Deserialize<Response>(test8);
            if (res8.ErrorOccured)
            {
                Console.WriteLine(res8.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Duedate edit went succesful");  //succeed
            }

            //Edit task due date to a past due date. the edit should be unsuccesful 
            DateTime dD2 = DateTime.Now.AddDays(-12);
            string test9 = task1.EditTaskDueDate(workingEmail2, board1, dD2, 1, 0);
            Response res9 = JsonSerializer.Deserialize<Response>(test9);
            if (res9.ErrorOccured)
            {
                Console.WriteLine(res9.ErrorMessage);  //should return an error message
            }
            else
            {
                Console.WriteLine("Duedate edit went succesful");
            }

            //Edit task due date should be successful. 
            DateTime dD3 = DateTime.Now.AddDays(12);
            string test10 = task1.EditTaskDueDate(workingEmail2, board1, dD3, 1, 0);
            Response res10 = JsonSerializer.Deserialize<Response>(test10);
            if (res10.ErrorOccured)
            {
                Console.WriteLine(res10.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Duedate edit went succesful");  //succeed
            }
        }
    }
}