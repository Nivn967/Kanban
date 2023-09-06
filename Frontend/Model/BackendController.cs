using System;
using System.Collections.Generic;
using System.Windows;
using IntroSE.Kanban.Backend.ServiceLayer;
using System.Text.Json;

namespace Frontend.Model
{
    public class BackendController
    {
        private  ServiceFactory serviceFactory { get; set; }
        public BackendController(ServiceFactory service)
        {
            this.serviceFactory = service;
        }

        public BackendController()
        {
            this.serviceFactory = new ServiceFactory();
            serviceFactory.LoadData();
        }

        /// <summary>
        /// Logs in a user to the system
        /// </summary>
        /// <param name="username">User's email</param>
        /// <param name="password">User's password</param>
        /// <returns>UserModel</returns>
        public UserModel Login(string username, string password)
        {
            string user = serviceFactory.userService.Login(username, password);

            Response<string> res1 = JsonSerializer.Deserialize<Response<string>>(user);

            if (res1.ErrorOccured)
            {
                throw new Exception(res1.ErrorMessage);
            }
            return new UserModel(this, username);
        }

        /// <summary>
        /// Logs out a user from the system
        /// </summary>
        /// <param name="username">User's email</param>
        /// <param name="password">User's password</param>
        /// <returns></returns>
        public void LogOut(string username)
        {
            string user = serviceFactory.userService.LogOut(username);

            Response<string> res1 = JsonSerializer.Deserialize<Response<string>>(user);

            if (res1.ErrorOccured)
            {
                throw new Exception(res1.ErrorMessage);
            }
        }

        /// <summary>
        /// Registers a user to the system
        /// </summary>
        /// <param name="username">User's email</param>
        /// <param name="password">User's password</param>
        /// <returns></returns>
        internal void Register(string username, string password)
        {
            string user = serviceFactory.userService.Register(username, password);
            Response res = JsonSerializer.Deserialize<Response>(user);
            if (res.ErrorOccured)
            {
                throw new Exception(res.ErrorMessage);
            }
        }


        /// <summary>
        /// Returns all user's board to the system
        /// </summary>
        /// <param name="email">User's email</param>
        /// <returns>List<int></returns>
        internal List<int> GetAllBoardsIds(string email)
        {
            string jsonBoardsIds = serviceFactory.boardService.GetUserBoards(email);
            Response<List<int>> res1 = JsonSerializer.Deserialize<Response<List<int>>>(jsonBoardsIds);
            if (res1.ErrorOccured)
            {
                throw new Exception(res1.ErrorMessage);
            }

            List<int> boardsIds = (List<int>)res1.ReturnValue;
            return boardsIds;
        }


        /// <summary>
        /// Returns a user's board to the system
        /// </summary>
        /// <param name="boardId">User's boardID</param>
        /// <returns>(int Id, string BoardName)</returns>
        internal (int Id, string BoardName) GetBoard(int boardId)
        {
            string ans = serviceFactory.boardService.GetBoardName(boardId);
            Response<string> res1 = JsonSerializer.Deserialize<Response<string>>(ans);
            if (res1.ErrorOccured)
            {
                throw new Exception(res1.ErrorMessage);
            }

            string boardName = (string)res1.ReturnValue;
            return (boardId, boardName);
        }


        /// <summary>
        /// Returns all board tasks
        /// </summary>
        /// <param name="email">User's email</param>
        /// <param name="boardName">Board name</param>
        /// <param name="columnOrdinal">Column ordinal</param>
        /// <returns>List<int></returns>
        internal List<int> GetAllTasksIds(string email, string boardName, int columnOrdinal)
        {
            string jsonTasks0 = serviceFactory.boardService.getColumnTaskIds(email, boardName, columnOrdinal);
            Response<List<int>> res0 = JsonSerializer.Deserialize<Response<List<int>>>(jsonTasks0);

            if (res0.ErrorOccured)
            {
                throw new Exception(res0.ErrorMessage);
            }
            List < int > taskIds = (List < int >)res0.ReturnValue;
         
            return taskIds;
        }


        /// <summary>
        /// Returns all board tasks
        /// </summary>
        /// <param name="email">User's email</param>
        /// <param name="_boardName">Board name</param>
        /// <param name="columnOrdinal">Column ordinal</param>
        /// <param name="taskId">taskID</param>
        /// <returns>(int Id, string Title, string Descreption)</returns>
        internal (int Id, string Title, string Descreption) GetTask(string email, string _boardName, int taskId, int columnOrdinal)
        {
            string ansId = serviceFactory.taskService.GetTaskId(email, _boardName, columnOrdinal, taskId);
            string ansTitle = serviceFactory.taskService.GetTaskTitle(email, _boardName, columnOrdinal, taskId);
            string ansDescription = serviceFactory.taskService.GetTaskDescription(email, _boardName, columnOrdinal, taskId);

            Response<int> res1 = JsonSerializer.Deserialize<Response<int>>(ansId);
            if (res1.ErrorOccured)
            {
                throw new Exception(res1.ErrorMessage);
            }
            Response<string> res2 = JsonSerializer.Deserialize<Response<string>>(ansTitle);
            if (res2.ErrorOccured)
            {
                throw new Exception(res2.ErrorMessage);
            }
            Response<string> res3 = JsonSerializer.Deserialize<Response<string>>(ansDescription);
            if (res3.ErrorOccured)
            {
                throw new Exception(res3.ErrorMessage);
            }

            int taskId1 = (int)res1.ReturnValue;
            string taskTitle = (string)res2.ReturnValue;
            string taskDesc = (string)res3.ReturnValue;

            return (taskId1, taskTitle, taskDesc);
        }

       

    }
}
