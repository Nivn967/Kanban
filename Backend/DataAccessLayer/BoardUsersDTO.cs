using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    public class BoardUsersDTO : DTO
    {
        public const string emailColumn = "email";
        public const string boardIdColumn = "boardId";

        private string email;
        public string Email { get { return email; } }

        private int boardId;
        public int BoardId { get { return boardId; } }

        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>the BoardUsersDTO constructor</summary>
        public BoardUsersDTO(int boardId, String email) : base(new BoardUsersController())
        {
            this.email = email;
            this.boardId = boardId;

            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            log.Info("Starting new BoardUsersDTO log!");
        }

        /// <summary>
        /// insert board member to the db.
        /// </summary>
        /// <exception cref="System.ArgumentException">Thrown if error accured when try to insert member to DB.</exception>
        /// <returns>true if member inserted successfully, false otherwise</returns>
        public bool InsertUser()
        {
            if (((BoardUsersController)_controller).Insert(this))
            {
                log.Info($"BoardUsersDTO inserted successfully, (board id:{boardId}, email-{email})");
                return true;
            }
            log.Error($"Error: cant insert user (email: {email}) to the table (board id:{boardId})");
            throw new ArgumentException($"Error: cant insert user (email: {email}) to the table (board id:{boardId})");
        }


        ///<summary>
        ///delete board member from the db.
        ///</summary>
        ///<exception cref="System.ArgumentException">Thrown if error accured when try to delete member from DB.</exception>
        /// <returns>true if member was deleted successfully, false otherwise</returns>
        public bool DeleteUser()
        {
            if (((BoardUsersController)_controller).Delete(this))
            {
                log.Info($"successfully deleted BoardUsersDTO from DB (board id-{boardId},userEmail:{Email})");
                return true;
            }
            log.Error($"Error: cant Delete user (email: {email}) to the table (board id:{boardId})");
            throw new ArgumentException($"Error: cant Delete user (email: {email}) to the table (board id:{boardId})");
        }

    }
}
