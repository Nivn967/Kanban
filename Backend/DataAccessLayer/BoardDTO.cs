using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using log4net;
using log4net.Config;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    public class BoardDTO : DTO
    {
        public const string BoardNameColumn = "BoardName";
        public const string BackLogLimitColumn = "BackLogLimit";
        public const string InProgressLimitColumn = "InProgressLimit";
        public const string DoneLimitColumn = "DoneLimit";
        public const string BoardIdColumn = "BoardId";
        public const string BoardOwnerColumn = "BoardOwner";

        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private int boardId;
        public int BoardId { get => boardId; }

        private string boardName;
        public string BoardName { get => boardName; }
        private string owner;
        public string Owner { get => owner; set { owner = value; ((BoardControllerDTO)_controller).Update(BoardId, BoardOwnerColumn, value); } }
        private int backlogLimit;
        public int BacklogLimit { get => backlogLimit; set { backlogLimit = value; ((BoardControllerDTO)_controller).Update(BoardId, BackLogLimitColumn, value); } }

        private int inProgressLimit;
        public int InProgressLimit { get => inProgressLimit; set { inProgressLimit = value; ((BoardControllerDTO)_controller).Update(BoardId, InProgressLimitColumn, value); } }
        private int doneLimit;
        public int DoneLimit { get => doneLimit; set { doneLimit = value; ((BoardControllerDTO)_controller).Update(BoardId, DoneLimitColumn, value); } }


        /// <summary>the BoardDTO constructor</summary>
        public BoardDTO(int boardId, string boardName, string owner, int backlogLimit, int inProgressLimit, int doneLimit) : base(new BoardControllerDTO())
        {
            this.boardId = boardId;
            this.boardName = boardName;
            this.owner = owner;
            this.backlogLimit = backlogLimit;
            this.inProgressLimit = inProgressLimit;
            this.doneLimit = doneLimit;

            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            log.Info("Starting new BoardDTO log!");
        }


        /// <summary>
        /// insert new board to the db.
        /// </summary>
        /// <exception cref="System.ArgumentException">Thrown if error accured when try to insert board to DB.</exception>
        /// <returns>true if board inserted, false otherwise</returns>
        public bool InsertBoard()
        {
            if (((BoardControllerDTO)_controller).Insert(this))
            {
                log.Info($"Successfully inserted the board to the db (board id: {boardId})");
                return true;
            }
            log.Error($"Error: can't add new board to the table (board id: {boardId})");
            throw new ArgumentException($"Error: can't add new board to the table (board id: {boardId})");
        }


        /// <summary>
        /// delete board from db.
        /// </summary>
        /// <exception cref="System.ArgumentException">Thrown if error accured when try to delete board from DB.</exception>
        /// <returns>true if board deleted, false otherwise</returns>
        public bool DeleteBoard()
        {
            if (((BoardControllerDTO)_controller).Delete(this))
            {
                log.Info($"Successfully deleted board from db");
                return true;
            }
            log.Error($"Error: can't delete board from the table (board id: {boardId})");
            throw new ArgumentException($"Error: can't delete board from the table (board id: {boardId})");
        }

    }
}
