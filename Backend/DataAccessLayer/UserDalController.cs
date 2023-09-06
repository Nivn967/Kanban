using log4net;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Data;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    public class UserDalController : DalController
    {
        private const string UserTableName = "Users";
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public UserDalController() : base(UserTableName)
        {

        }
        ///<summary>
        ///update the isLogin field in the DataBase.
        ///</summary>
        /// <returns>return true or false if update was successful</returns>
        public bool Update(string email, string isLogincolumnName, bool value)
        {

            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                connection.Open();

                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"UPDATE {UserTableName} set {isLogincolumnName}={value} where {UserDTO.UserEmailColumnName}=@emailVal";
                try
                {
                    SQLiteParameter emailVal = new SQLiteParameter("@emailVal", email);
                    command.Parameters.Add(emailVal);
                    command.Prepare();

                    res = command.ExecuteNonQuery();
                    log.Info("Updated field successfully");
                }
                catch(Exception e)
                {
                    log.Error(e.ToString());
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
                return res > 0;
            }
        }
        /// <summary>
        /// Insert a user to the DataBase.
        /// </summary>
        /// <param name="userDTO">the user to insert</param>
        /// <param name="emailColumn">the email column name in the DataBase </param>
        /// <param name="passwordColumn">the password column name in the DataBase </param>
        /// <param name="isLoginColumn">the isLogin column name in the DataBase </param>
        /// <returns>returns true if the user was inserted, else false. </returns>
        public bool InsertUser(UserDTO userDTO, string emailColumn, string passwordColumn, string isLoginColumn)
        {
            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {_tableName} ({emailColumn} , {passwordColumn}, {isLoginColumn}) " +
                        $"VALUES (@emailVal,@passwordVal,{userDTO._IsLogin});";

                    SQLiteParameter emailVal = new SQLiteParameter("@emailVal", userDTO._Email);
                    SQLiteParameter passwordVal = new SQLiteParameter("@passwordVal", userDTO._Password);

                    command.Parameters.Add(emailVal);
                    command.Parameters.Add(passwordVal);
                    command.Prepare();

                    res = command.ExecuteNonQuery();
                    log.Info("Updated field successfully");

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
        /// <summary>
        /// Load all the users from DataBase. 
        /// </summary>
        /// <returns>return a list of all the users from the DataBase.</returns>
        public List<UserDTO> LoadUsers()
        {
            List<UserDTO> result = Select().Cast<UserDTO>().ToList();
            return result;
        }
        /// <summary>
        /// convert the data to a DTO objects , a specific DTO to each DalController.
        /// </summary>
        /// <param name="reader">A sql reader Object from the DataBase.</param>
        /// <returns>the right DTO object. </returns>
        protected override DTO ConvertReaderToObject(SQLiteDataReader reader)
        {
            UserDTO result = new UserDTO(reader.GetString(0), reader.GetString(1), reader.GetString(2)=="1");
            log.Debug("Pulled info successfully");
            return result;
        }
    }
}
