using log4net;
using log4net.Config;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    public class BoardControllerDTO : DalController
    {
        private const string boardsTableName = "Boards";
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public BoardControllerDTO() : base(boardsTableName)
        {
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            log.Info("Starting new BoardControllerDTO log!");
        }

        ///<summary>
        ///insert new board to the db.
        ///</summary>
        ///<param name="board">the BoardDTO.</param>
        /// <returns>true if the board was added, false otherwise</returns>
        public bool Insert(BoardDTO board)
        {

            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {boardsTableName} ({BoardDTO.BoardIdColumn} ,{BoardDTO.BoardNameColumn},{BoardDTO.BoardOwnerColumn},{BoardDTO.BackLogLimitColumn},{BoardDTO.InProgressLimitColumn},{BoardDTO.DoneLimitColumn}) " +
                        $"VALUES (@idVal,@boardNameVal,@ownerEmailVal,@BackLogLimitVal,@InProgressLimitVal,@DoneLimitVal);";

                    SQLiteParameter idParam = new SQLiteParameter(@"idVal", board.BoardId);
                    SQLiteParameter NameParam = new SQLiteParameter(@"boardNameVal", board.BoardName);
                    SQLiteParameter creatorMailParam = new SQLiteParameter(@"ownerEmailVal", board.Owner);
                    SQLiteParameter backLogLimitParam = new SQLiteParameter(@"BackLogLimitVal", board.BacklogLimit);
                    SQLiteParameter inProgressLimitParam = new SQLiteParameter(@"InProgressLimitVal", board.InProgressLimit);
                    SQLiteParameter doneLimitParam = new SQLiteParameter(@"DoneLimitVal", board.DoneLimit);

                    command.Parameters.Add(idParam);
                    command.Parameters.Add(NameParam);
                    command.Parameters.Add(creatorMailParam);
                    command.Parameters.Add(backLogLimitParam);
                    command.Parameters.Add(inProgressLimitParam);
                    command.Parameters.Add(doneLimitParam);
                    command.Prepare();

                    res = command.ExecuteNonQuery();
                    log.Info($"successfully insert board to DB (board id: {board.BoardId})");
                }
                catch (Exception e)
                {
                    log.Error(e.ToString());
                    throw new ArgumentException(e.ToString());
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
                return res > 0;
            }
        }

        ///<summary>
        ///select all boards from DB.
        ///</summary>
        /// <returns>return list of BoardDTO</returns>
        public List<BoardDTO> SelectAllBoards()
        {
            List<BoardDTO> result = Select().Cast<BoardDTO>().ToList();
            log.Info("successfully selected all boards");
            return result;
        }

        ///<summary>
        ///update board fields in db.
        ///</summary>
        ///<param name="boardId">the board id</param>
        ///<param name="columnName">column to update</param>
        ///<param name="value">the new value</param>
        /// <returns>return true if the update succeed, and false otherwise</returns>
        public bool Update(int boardId, string columnName, int value)
        {

            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"UPDATE {boardsTableName} set {columnName}={value} where {BoardDTO.BoardIdColumn}={boardId}";
                try
                {
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    log.Info($"successfully updated the db (board id:{boardId}, column name:{columnName}, new value:{value})");
                }
                catch (Exception e)
                {
                    log.Error(e.ToString());
                    throw new ArgumentException(e.ToString());
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
                return res > 0;
            }
        }

        ///<summary>
        ///update board fields in db.
        ///</summary>
        ///<param name="boardId">the BoardId.</param>
        ///<param name="columnName">column to update</param>
        ///<param name="value">new value</param>
        /// <returns>return true if the update succeed, and false otherwise</returns>
        public bool Update(int boardId, string columnName, string value)
        {

            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"UPDATE {boardsTableName} set [{columnName}]=@{value} where {BoardDTO.BoardIdColumn}={boardId}";
                try
                {
                    command.Parameters.Add(new SQLiteParameter(columnName, value));
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    log.Info($"successfully updated the db (board id:{boardId}, column name:{columnName}, new value:{value})");
                }
                catch (Exception e)
                {
                    log.Error(e.ToString());
                    throw new ArgumentException(e.ToString());
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
                return res > 0;
            }
        }

        ///<summary>
        ///Delete board from the db.
        ///</summary>
        ///<param name="board">the BoardDTO.</param>
        /// <returns>true if the board was deleted, false otherwise</returns>
        public bool Delete(BoardDTO board)
        {
            int res = -1;

            using (var connection = new SQLiteConnection(_connectionString))
            {
                var command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"delete from {_tableName} where {BoardDTO.BoardIdColumn}={board.BoardId}"
                };
                try
                {
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    log.Info($"successfully deleted board (boardId: {board.BoardId}) from the db");

                }
                catch (Exception e)
                {
                    log.Error(e.ToString());
                    throw new ArgumentException(e.ToString());
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }

            }
            return res > 0;
        }

        ///<summary>
        ///loads all the boards from the db
        ///</summary>
        /// <returns>return list of Dboard</returns>
        public List<BoardDTO> LoadBoards()
        {
            List<BoardDTO> result = Select().Cast<BoardDTO>().ToList();
            log.Info("successfully loads all boards from the db");
            return result;
        }

        /// <summary>
        /// convert reader values to BoardDTO fields 
        /// </summary>
        /// <param name="reader">SQLiteDataReader reader</param>
        /// <returns>return the Dboard</returns>
        protected override DTO ConvertReaderToObject(SQLiteDataReader reader)
        {
            BoardDTO result = new BoardDTO(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetInt32(3), reader.GetInt32(4), reader.GetInt32(5));
            log.Info("successfully convert reader to BoardDTO object");
            return result;
        }
    }
}
