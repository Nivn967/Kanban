using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Reflection;
using System.IO;
using log4net;
using log4net.Config;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    public abstract class DalController
    {
        protected readonly string _connectionString;
        protected readonly string _tableName;
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>the DalController constructor</summary>
        public DalController(string tableName)
        {
            string path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "kanban.db"));
            this._connectionString = $"Data Source={path}; Version=3;";
            this._tableName = tableName;

            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            log.Info("Starting new DalController log!");

        }

        ///<summary>
        ///Select all from table.
        ///</summary>
        /// <returns>true if selected successfully, false otherwise</returns>
        protected List<DTO> Select()
        {
            List<DTO> results = new List<DTO>();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"select * from {_tableName};";
                SQLiteDataReader dataReader = null;
                try
                {
                    connection.Open();
                    dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        results.Add(ConvertReaderToObject(dataReader));

                    }
                    log.Info($"Successfully selected all from table {_tableName}");
                }
                catch (Exception e)
                {
                    log.Error(e.ToString());
                    throw new ArgumentException(e.ToString());
                }
                finally
                {
                    if (dataReader != null)
                    {
                        dataReader.Close();
                    }

                    command.Dispose();
                    connection.Close();
                }

            }
            return results;
        }

        ///<summary>
        ///Deleted all from table.
        ///</summary>
        /// <returns>true if deleted successfully, false otherwise</returns>
        public bool DeleteAll()
        {
            int res = -1;

            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteDataReader dataReader = null;
                var command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"Delete from {_tableName}"
                };
                try
                {
                    connection.Open();
                    dataReader = command.ExecuteReader();
                    if (!dataReader.HasRows)
                        return true;
                    res = command.ExecuteNonQuery();
                    log.Info($"Successfully deleted all from table {_tableName}");
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

        /// <summary>
        /// convert reader to BoardDTO object (abstract)
        /// </summary>
        /// <param name="reader">SQLiteDataReader reader</param>
        protected abstract DTO ConvertReaderToObject(SQLiteDataReader reader);
    }
}
