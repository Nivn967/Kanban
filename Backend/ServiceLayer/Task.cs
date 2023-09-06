using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using log4net;
using log4net.Config;
using System.IO;
using System.Reflection;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class Task
    {
        public BusinessLayer.User user;
        public BusinessLayer.Board board;
        public BusinessLayer.Task task;
        public BusinessLayer.UserController userController;
        public BusinessLayer.BoardController boardController;
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public Task(BusinessLayer.UserController userController, BusinessLayer.BoardController boardController)
        {
            this.userController = userController;
            this.boardController = boardController;
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            log.Info("Starting new board log!");
        }

        /// <summary>
        /// Edits task descreption
        /// </summary>
        /// <param name="email">the users email</param>
        /// <param name="boardName">the task board name</param>
        /// <param name="description">the task description</param>
        /// <param name="taskId">the task ID</param>
        /// <param name="columnOrdinal">the task status</param>
        /// <returns>json string of response object</returns>
        public string EditTaskDescription(string email, string boardName, string description, int taskId, int columnOrdinal)
        {
            Response res;
            string objectAsJson;
            JsonSerializerOptions options1 = new JsonSerializerOptions { WriteIndented = true };

            try
            {
                user = userController.getUser(email);
                user.verifyLogin();
                boardController.verifyBoardExists(boardName, email);
                board = boardController.getBoard(boardName, email);
                task = board.getTask(taskId, columnOrdinal);
                task.EditTaskDescription(description, email);
                log.Info("Task description edited successfully");
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
        /// Edits task title
        /// </summary>
        /// <param name="email">the users email</param>
        /// <param name="boardName">the task board name</param>
        /// <param name="title">the task title</param>
        /// <param name="taskId">the task ID</param>
        /// <param name="columnOrdinal">the task status</param>
        /// <returns>json string of response object</returns>
        public string EditTaskTitle(string email, string boardName, string title, int taskId, int columnOrdinal)
        {
            Response res;
            string objectAsJson;
            JsonSerializerOptions options1 = new JsonSerializerOptions { WriteIndented = true };

            try
            {
                user = userController.getUser(email);
                user.verifyLogin();
                boardController.verifyBoardExists(boardName, email);
                board = boardController.getBoard(boardName, email);
                task = board.getTask(taskId, columnOrdinal);
                task.EditTaskTitle(title, email);
                log.Info("Task title edited successfully");
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

        public string GetTaskId(string email, string boardName, int columnOrdinal, int taskID)
        {
            string objectAsJson;
            JsonSerializerOptions options1 = new JsonSerializerOptions { WriteIndented = true };
            BusinessLayer.Task task;

            try
            {
                user = userController.getUser(email);
                user.verifyLogin();
                boardController.verifyBoardExists(boardName, email);
                board = boardController.getBoard(boardName, email);
                task = board.getTask(taskID, columnOrdinal);
                log.Info("Task returned successfully");
            }
            catch (Exception e)
            {
                Response res = new Response(e.Message, null);
                log.Error(e.Message);
                objectAsJson = JsonSerializer.Serialize(res, options1);
                return objectAsJson;
            }

            Response<int> res2 = new Response<int>(null, task.Id);
            objectAsJson = JsonSerializer.Serialize(res2, options1);
            return objectAsJson;
        }

        public string GetTaskTitle(string email, string boardName, int columnOrdinal, int taskID)
        {
            string objectAsJson;
            JsonSerializerOptions options1 = new JsonSerializerOptions { WriteIndented = true };
            BusinessLayer.Task task;

            try
            {
                user = userController.getUser(email);
                user.verifyLogin();
                boardController.verifyBoardExists(boardName, email);
                board = boardController.getBoard(boardName, email);
                task = board.getTask(taskID, columnOrdinal);
                log.Info("Task returned successfully");
            }
            catch (Exception e)
            {
                Response res = new Response(e.Message, null);
                log.Error(e.Message);
                objectAsJson = JsonSerializer.Serialize(res, options1);
                return objectAsJson;
            }

            Response<string> res2 = new Response<string>(null, task.Title);
            objectAsJson = JsonSerializer.Serialize(res2, options1);
            return objectAsJson;
        }

        public string GetTaskDescription(string email, string boardName, int columnOrdinal, int taskID)
        {
            string objectAsJson;
            JsonSerializerOptions options1 = new JsonSerializerOptions { WriteIndented = true };
            BusinessLayer.Task task;

            try
            {
                user = userController.getUser(email);
                user.verifyLogin();
                boardController.verifyBoardExists(boardName, email);
                board = boardController.getBoard(boardName, email);
                task = board.getTask(taskID, columnOrdinal);
                log.Info("Task returned successfully");
            }
            catch (Exception e)
            {
                Response res = new Response(e.Message, null);
                log.Error(e.Message);
                objectAsJson = JsonSerializer.Serialize(res, options1);
                return objectAsJson;
            }

            Response<string> res2 = new Response<string>(null, task.Description);
            objectAsJson = JsonSerializer.Serialize(res2, options1);
            return objectAsJson;
        }

        /// <summary>
        /// Edits task due date
        /// </summary>
        /// <param name="email">the users email</param>
        /// <param name="boardName">the task board name</param>
        /// <param name="dueDate">the task title</param>
        /// <param name="taskId">the task ID</param>
        /// <param name="columnOrdinal">the task status</param>
        /// <returns>json string of response object</returns>
        public string EditTaskDueDate(string email, string boardName, DateTime dueDate, int taskId, int columnOrdinal)
        {
            Response res;
            string objectAsJson;
            JsonSerializerOptions options1 = new JsonSerializerOptions { WriteIndented = true };

            try
            {
                user = userController.getUser(email);
                user.verifyLogin();
                boardController.verifyBoardExists(boardName, email);
                board = boardController.getBoard(boardName, email);
                task = board.getTask(taskId, columnOrdinal);
                task.EditTaskDueDate(dueDate, email);
                log.Info("Task due date edited successfully");
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
        /// Adds new task to the board
        /// </summary>
        /// <param name="email">the users email</param>
        /// <param name="boardName">the task board name</param>
        /// <param name="dueDate">the task title</param>
        /// <param name="columnOrdinal">the task status</param>
        /// <param name="dueDate">the task status</param>
        /// <param name="title">the task title</param>
        /// <param name="description">the task description</param>
        /// <returns>json string of response object</returns>
        public string AddTask(string email, string boardName, int columnOrdinal, DateTime dueDate, string title, string description)
        {
            Response res;
            string objectAsJson;
            JsonSerializerOptions options1 = new JsonSerializerOptions { WriteIndented = true };

            try
            {
                user = userController.getUser(email);
                user.verifyLogin();
                boardController.verifyBoardExists(boardName, email);
                board = boardController.getBoard(boardName, email);
                board.AddTask(email, columnOrdinal, dueDate, title, description);
                log.Info("Task added successfully");
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
        /// Advances task status
        /// </summary>
        /// <param name="boardName">the task board name</param>
        /// <param name="taskId">the task ID</param>
        /// <param name="columnOrdinal">the task status</param>
        /// <param name="email">the users email</param>
        /// <returns>json string of response object</returns>
        public string AdvanceTask(string boardName, int taskId, int columnOrdinal, string email)
        {
            Response res;
            string objectAsJson;
            JsonSerializerOptions options1 = new JsonSerializerOptions { WriteIndented = true };

            try
            {
                user = userController.getUser(email);
                user.verifyLogin();
                boardController.verifyBoardExists(boardName, email);
                board = boardController.getBoard(boardName, email);
                board.AdvanceTask(taskId, columnOrdinal, email);
                log.Info("Task advanced successfully");
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
        /// Edits task asignee
        /// </summary>
        /// <param name="email">the task old asignee</param>
        /// <param name="boardName">the board name</param>
        /// <param name="columnOrdinal">the task status</param>
        /// <param name="taskID">the task ID</param>
        /// <param name="emailAssignee">the new asignee email</param>
        /// <returns>json string of response object</returns>
        public String AssignTask(string email, string boardName, int columnOrdinal, int taskID, string emailAssignee)
        {
            Response res;
            string objectAsJson;
            JsonSerializerOptions options1 = new JsonSerializerOptions { WriteIndented = true };

            try
            {
                user = userController.getUser(email);
                user.verifyLogin();
                boardController.verifyBoardExists(boardName, email);
                board = boardController.getBoard(boardName, email);
                board.AssignTask(email, columnOrdinal, taskID, emailAssignee);
                log.Info("Task assigned successfully");
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