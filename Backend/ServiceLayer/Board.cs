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
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class Board
    {
        public BusinessLayer.User user;
        public BusinessLayer.Board board;
        public BusinessLayer.UserController _uc;
        public BusinessLayer.BoardController _bc;
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public Board(BusinessLayer.UserController _uc, BusinessLayer.BoardController _bc)
        {
            this._uc = _uc;
            this._bc = _bc;
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            log.Info("Starting new board log!");
        }

        /// <summary>
        /// Limit the number of tasks in a column
        /// </summary>
        /// <param name="email">User email</param>
        /// <param name="boardName">The board which the column wil be limited</param>
        /// <param name="limit">Column limitation</param>
        /// <param name="columnOrdinal">Column ordinal</param>
        /// <returns name=>json string of Response object</returns>
        public string LimitColumn(string email, string boardName, int limit, int columnOrdinal)
        {
            Response res;
            string objectAsJson;
            JsonSerializerOptions options1 = new JsonSerializerOptions { WriteIndented = true };

            try
            {
                user = _uc.getUser(email);
                user.verifyLogin();
                _bc.verifyBoardExists(boardName, email);
                board = _bc.getBoard(boardName, email);
                board.LimitColumn(limit, columnOrdinal);
                log.Info("Column limit updated successfully");
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
        /// get column name by it's ordinal number.
        /// </summary>
        /// <param name="email">User's email</param>
        /// <param name="boardName">Boards's name</param>
        /// <param name="columnOrdinal">Task's Column ordinal</param>
        /// <returns>json string of Response object</returns>
        public string getColumnName(string email, string boardName, int columnOrdinal)
        {
            JsonSerializerOptions options1 = new JsonSerializerOptions { WriteIndented = true };

            string objectAsJson;
            string columnName;
            try
            {
                user = _uc.getUser(email);
                user.verifyLogin();
                _bc.verifyBoardExists(boardName, email);
                board = _bc.getBoard(boardName, email);
                columnName = board.getColumnName(columnOrdinal);
                log.Info("Clomun name returned");
            }
            catch (Exception e)
            {
                Response res = new Response(e.Message, null);
                log.Error(e.Message);
                objectAsJson = JsonSerializer.Serialize(res, options1);
                return objectAsJson;
            }

            Response<string> res2 = new Response<string>(null, columnName);
            objectAsJson = JsonSerializer.Serialize(res2, options1);
            return objectAsJson;
        }

        /// <summary>
        /// Returns the limit of a specific column
        /// </summary>
        /// <param name="email">User email</param>
        /// <param name="boardName">The board which the column wil be limited</param>
        /// <param name="columnOrdinal">Column ordinal</param>
        /// <returns name=>json string of Response object</returns>
        public string getColumnLimit(string email, string boardName, int columnOrdinal)
        {
            string objectAsJson;
            int columnLimit;
            JsonSerializerOptions options1 = new JsonSerializerOptions { WriteIndented = true };

            try
            {
                user = _uc.getUser(email);
                user.verifyLogin();
                _bc.verifyBoardExists(boardName, email);
                board = _bc.getBoard(boardName, email);
                columnLimit = board.getColumnLimit(columnOrdinal);
                log.Info("Column limit returned");
            }
            catch (Exception e)
            {
                Response res = new Response(e.Message, null);
                log.Error(e.Message);
                objectAsJson = JsonSerializer.Serialize(res, options1);
                return objectAsJson;
            }

            Response<int> res2 = new Response<int>(null, columnLimit);
            objectAsJson = JsonSerializer.Serialize(res2, options1);
            return objectAsJson;
        }

        /// <summary>
        /// Returnes a list of column tasks
        /// </summary>
        /// <param name="email">User email</param>
        /// <param name="boardName">The board which the column wil be limited</param>
        /// <param name="columnOrdinal">Column ordinal</param>
        /// <returns name=>json string of Response object</returns>
        public string getColumn(string email, string boardName, int columnOrdinal)
        {
            string objectAsJson;
            Dictionary<int, BusinessLayer.Task> column;
            try
            {
                user = _uc.getUser(email);
                user.verifyLogin();
                _bc.verifyBoardExists(boardName, email);
                board = _bc.getBoard(boardName, email);
                column = board.getColumn(columnOrdinal);
                log.Info("Column list returned");
            }
            catch (Exception e)
            {
                JsonSerializerOptions options1 = new JsonSerializerOptions { WriteIndented = true };
                Response res = new Response(e.Message, null);
                log.Error(e.Message);
                objectAsJson = JsonSerializer.Serialize(res, options1);
                return objectAsJson;
            }

            JsonSerializerOptions options2 = new JsonSerializerOptions { IncludeFields = true };
            options2.WriteIndented = true;

            Response<List<BusinessLayer.Task>> res2 = new Response<List<BusinessLayer.Task>>(null, column.Values.ToList());
            objectAsJson = JsonSerializer.Serialize(res2, options2);
            return objectAsJson;
        }

        /// <summary>
        /// Returnes a list of column tasks ids
        /// </summary>
        /// <param name="email">User email</param>
        /// <param name="boardName">The board which the column wil be limited</param>
        /// <param name="columnOrdinal">Column ordinal</param>
        /// <returns name=>json string of Response object</returns>
        public string getColumnTaskIds(string email, string boardName, int columnOrdinal)
        {
            string objectAsJson;
            Dictionary<int, BusinessLayer.Task> column;
            try
            {
                user = _uc.getUser(email);
                user.verifyLogin();
                _bc.verifyBoardExists(boardName, email);
                board = _bc.getBoard(boardName, email);
                column = board.getColumn(columnOrdinal);
                log.Info("Column list returned");
            }
            catch (Exception e)
            {
                JsonSerializerOptions options1 = new JsonSerializerOptions { WriteIndented = true };
                Response res = new Response(e.Message, null);
                log.Error(e.Message);
                objectAsJson = JsonSerializer.Serialize(res, options1);
                return objectAsJson;
            }

            JsonSerializerOptions options2 = new JsonSerializerOptions { IncludeFields = true };
            options2.WriteIndented = true;

            Response<List<int>> res2 = new Response<List<int>>(null, column.Keys.ToList());
            objectAsJson = JsonSerializer.Serialize(res2, options2);
            return objectAsJson;
        }

        /// <summary>
        /// Transfers boards ownership
        /// </summary>
        /// <param name="currentOwnerEmail">Current owner email</param>
        /// <param name="newOwnerEmail">The new owner which will be assigned</param>
        /// <param name="boardName">The board which the owner will be transfered to</param>
        /// <returns name=>json string of Response object</returns>
        public String TransferOwnership(string currentOwnerEmail, string newOwnerEmail, string boardName)
        {
            Response res;
            string objectAsJson;
            JsonSerializerOptions options1 = new JsonSerializerOptions { WriteIndented = true };

            BusinessLayer.Task task;
            try
            {
                user = _uc.getUser(currentOwnerEmail);
                user.verifyLogin();
                _bc.verifyBoardExists(boardName, currentOwnerEmail);
                board = _bc.getBoard(boardName, currentOwnerEmail);
                board.TransferOwnership(currentOwnerEmail, newOwnerEmail);
                log.Info("Owner transfered successfully");
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
        /// Add a new board to a user in the system
        /// </summary>
        /// <param name="email">The user's email</param>
        /// <param name="boardName">The new board name</param>
        /// <returns>json string of Response object</returns>
        public string AddBoard(string email, string boardName)
        {
            Response res;
            string objectAsJson;
            JsonSerializerOptions options1 = new JsonSerializerOptions { WriteIndented = true };


            try
            {
                _bc.AddBoard(email, boardName);
                log.Info("Board added successfully");
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
        /// Remove a board from a user in the system
        /// </summary>
        /// <param name="email">User's email.</param>
        /// <param name="boardName">Boards's name</param>
        /// <returns name=>json string of Response object</returns>
        public string RemoveBoard(string email, string boardName)
        {
            Response res;
            string objectAsJson;
            JsonSerializerOptions options1 = new JsonSerializerOptions { WriteIndented = true };

            try
            {
                _bc.RemoveBoard(email, boardName);
                log.Info("Board removed successfully");
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
        /// Returns board name
        /// </summary>
        /// <param name="boardId">Board ID.</param>
        /// <returns name=>json string of Response object</returns>
        public string GetBoardName(int boardId)
        {
            string boardName;
            string objectAsJson;
            JsonSerializerOptions options1 = new JsonSerializerOptions { WriteIndented = true };

            try
            {
                boardName = _bc.GetBoardName(boardId);
                log.Info("Board name returned successfully");
            }
            catch (Exception e)
            {
                Response res = new Response(e.Message, null);
                log.Error(e.Message);
                objectAsJson = JsonSerializer.Serialize(res, options1);
                return objectAsJson;
            }

            Response<string> res2 = new Response<string>(null, boardName);
            objectAsJson = JsonSerializer.Serialize(res2, options1);
            return objectAsJson;
        }

        /// <summary>
        /// Returns the users members of the board
        /// </summary>
        /// <param name="email">User email</param>
        /// <returns name=>json string of Response object</returns>
        public string GetUserBoards(string email)
        {
            string objectAsJson;
            Dictionary<int, BusinessLayer.Board> user_boards;

            try
            {
                user_boards = _bc.GetUserBoards(email);
                log.Info("Returned user boards");
            }
            catch (Exception e)
            {
                JsonSerializerOptions options1 = new JsonSerializerOptions { WriteIndented = true };
                Response res = new Response(e.Message, null);
                log.Error(e.Message);
                objectAsJson = JsonSerializer.Serialize(res, options1);
                return objectAsJson;
            }

            JsonSerializerOptions options2 = new JsonSerializerOptions { IncludeFields = true };
            options2.WriteIndented = true;

            Response <List<int>>res2 = new Response<List<int>>(null, user_boards.Keys.ToList());
            objectAsJson = JsonSerializer.Serialize(res2, options2);

            return objectAsJson;
        }

        /// <summary>
        /// Remove a board from a user in the system
        /// </summary>
        /// <param name="email">User's email.</param>
        /// <param name="boardID">Boards's ID</param>
        /// <returns name=>json string of Response object</returns>
        public string JoinBoard(string email, int boardID)
        {
            Response res;
            string objectAsJson;
            JsonSerializerOptions options1 = new JsonSerializerOptions { WriteIndented = true };

            try
            {
                _bc.JoinBoard(email, boardID);
                log.Info("User joied the board successfully");
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
        /// Remove a board from a user in the system
        /// </summary>
        /// <param name="email">User's email.</param>
        /// <param name="boardID">Boards's ID</param>
        /// <returns name=>json string of Response object</returns>
        public string LeaveBoard(string email, int boardID)
        {
            Response res;
            string objectAsJson;
            JsonSerializerOptions options1 = new JsonSerializerOptions { WriteIndented = true };

            try
            {
                _bc.LeaveBoard(email, boardID);
                log.Info("User left the board successfully");
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
        /// Return a set of all the tasks of a asignee that are in progress.
        /// </summary>
        /// <param name="email">User's email.</param>
        /// <returns> json string of Response object</returns>
        public string getAssigneeInProgressTasks(string email)
        {
            string objectAsJson;
            Response res;
            List<BusinessLayer.Task> assigneeInProgressTasks;
            JsonSerializerOptions options1 = new JsonSerializerOptions { WriteIndented = true };

            try
            {
                assigneeInProgressTasks = _bc.getAssigneeInProgressTasks(email);
                log.Info("Returned inprogress tasks list");
            }
            catch (Exception e)
            {
                res = new Response(e.Message, null);
                log.Error(e.Message);
                objectAsJson = JsonSerializer.Serialize(res, options1);
                return objectAsJson;
            }

            JsonSerializerOptions options = new JsonSerializerOptions { IncludeFields = true };
            options.WriteIndented = true;

            Response<List<BusinessLayer.Task>> res2= new Response<List<BusinessLayer.Task>>(null, assigneeInProgressTasks);
            objectAsJson = JsonSerializer.Serialize(res2, options);
            return objectAsJson;
        }


        /// <summary>
        /// Loads all the board that in the DataBase
        /// </summary>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public string LoadBoards()
        {
            Response res;
            string objectAsJson;
            JsonSerializerOptions options1 = new JsonSerializerOptions { WriteIndented = true };

            try
            {
                _bc.LoadBoards();
                log.Info("Board Loaded successfully");
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
        /// Deletes All the DataBase
        /// </summary>
        /// <returns>A response object. The response should contain a error message in case of an error</returns>
        public string DeleteAll()
        {
            Response res;
            string objectAsJson;
            JsonSerializerOptions options1 = new JsonSerializerOptions { WriteIndented = true };

            try
            {
                _bc.DeleteAllBoards();
                log.Info("Data deleted successfully");
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