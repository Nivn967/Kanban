using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.ServiceLayer
{
    public class ServiceFactory
    {
        private BusinessLayer.UserController userController;
        private BusinessLayer.BoardController boardController;
        public User userService { get; set; }
        public Board boardService { get; set; }
        public Task taskService { get; set; }
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public ServiceFactory()
        {
            userController = new BusinessLayer.UserController();
            boardController = new BusinessLayer.BoardController(userController);
            userService = new User(userController);
            boardService = new Board(userController, boardController);
            taskService = new Task(userController, boardController);
        }
        /// <summary>
        /// loads the data from the db to the ram.
        /// </summary>
        /// <returns> json string of Response object</returns>
        public string LoadData()
        {
            JsonSerializerOptions options1 = new JsonSerializerOptions { WriteIndented = true };
            Response res;
            string objectAsJson;
            try
            {
                userService.LoadUsers();
                boardService.LoadBoards();
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
        /// delete the data that came from the db to the ram.
        /// </summary>
        /// <returns> json string of Response object</returns>
        public string deleteData()
        {
            JsonSerializerOptions options1 = new JsonSerializerOptions { WriteIndented = true };
            Response res;
            string objectAsJson;
            try
            {
                userService.deleteUsers();
                boardService.DeleteAll();
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