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
    internal class BoardUsersController : DalController
    {
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private const string boardsUsersTableName = "BoardUsers";

        /// <summary>
        /// the constructor of DalBoardController
        /// </summary>
        public BoardUsersController() : base(boardsUsersTableName)
        {
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            log.Info("Starting new BoardUsersController log!");
        }


        ///<summary>
        ///insert board member to the db.
        ///</summary>
        ///<param name="board">the BoardUsersDTO.</param>
        /// <returns>true if inserted successfully, false otherwise</returns>
        public bool Insert(BoardUsersDTO boardUser)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {boardsUsersTableName} ({BoardUsersDTO.boardIdColumn} ,{BoardUsersDTO.emailColumn}) " +
                        $"VALUES (@idVal,@emailVal);";

                    SQLiteParameter idParam = new SQLiteParameter(@"idVal", boardUser.BoardId);
                    SQLiteParameter mailParam = new SQLiteParameter(@"emailVal", boardUser.Email);

                    command.Parameters.Add(idParam);
                    command.Parameters.Add(mailParam);
                    command.Prepare();
                    res = command.ExecuteNonQuery();
                    log.Info($"Successfully inserted new board member to the db (boardId:{boardUser.BoardId}, email:{boardUser.Email})");
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
        ///select all the board members.
        ///</summary>
        /// <returns>return list of DMmeber</returns>
        public List<BoardUsersDTO> SelectAllBoardUsers()
        {
            List<BoardUsersDTO> result = Select().Cast<BoardUsersDTO>().ToList();
            log.Info("successfully selected all board members");
            return result;
        }


        /// <summary>
        /// convert reader values to BoardDTO fields 
        /// </summary>
        /// <param name="reader">SQLiteDataReader reader</param>
        /// <returns>return the BoardUsersDTO</returns>
        protected override DTO ConvertReaderToObject(SQLiteDataReader reader)
        {
            BoardUsersDTO result = new BoardUsersDTO(reader.GetInt32(0), reader.GetString(1));
            log.Info("successfully convert reader to BoardUsersDTO object");
            return result;
        }


        ///<summary>
        ///loads all the board members
        ///</summary>
        /// <returns>return list of BoardUsersDTO</returns>
        public List<BoardUsersDTO> LoadBoardMembers()
        {
            List<BoardUsersDTO> result = Select().Cast<BoardUsersDTO>().ToList();
            log.Info("Successfully loaded all board members");
            return result;
        }

        ///<summary>
        ///Delete board member from the db.
        ///</summary>
        ///<param name="board">the BoardUsersDTO.</param>
        /// <returns>true if the board members was deleted, false otherwise</returns>
        public bool Delete(BoardUsersDTO boardUser)
        {
            int res = -1;

            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"delete from {_tableName} where {BoardUsersDTO.boardIdColumn}={boardUser.BoardId} AND {BoardUsersDTO.emailColumn}=\"{boardUser.Email}\""
                };
                try
                {
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    log.Info($"successfully deleted board member (boardId: {boardUser.BoardId},email: {boardUser.Email}) from the db");
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

    }
}
