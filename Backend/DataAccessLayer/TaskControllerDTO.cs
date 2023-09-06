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
    public class TaskControllerDTO : DalController
    {
        private const string taskTableName = "Tasks";
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>the TaskControllerDTO constructor</summary>
        public TaskControllerDTO() : base(taskTableName)
        {
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            log.Info("Starting new TaskControllerDTO log!");
        }

        /// <summary>
        /// convert reader values to TaskDTO fields 
        /// </summary>
        /// <param name="reader">SQLiteDataReader reader</param>
        /// <returns>return the TaskDTO</returns>
        protected override DTO ConvertReaderToObject(SQLiteDataReader reader)
        {
            TaskDTO result = new TaskDTO(reader.GetInt32(0), reader.GetInt32(1), DateTime.Parse(reader.GetString(2)), reader.GetString(3), reader.IsDBNull(4) ? null : reader.GetString(4), DateTime.Parse(reader.GetString(5)), reader.GetInt32(6), reader.IsDBNull(7) ? null : reader.GetString(7));
            log.Info("successfully convert reader to TaskDTO object");
            return result;
        }

        ///<summary>
        ///update task fields in db.
        ///</summary>
        ///<param name="boardId">the board id</param>
        ///<param name="taskId" >task id</param> 
        ///<param name="attributeName">column to update</param>
        ///<param name="attributeValue">the new value</param>
        /// <returns>return true if the update succeed, and false otherwise</returns>
        public bool Update(int boardId, int taskId, string attributeName, string attributeValue)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"update {taskTableName} set [{attributeName}]=@{attributeName} where BoardID={boardId} AND TaskID={taskId}"
                };
                try
                {
                    command.Parameters.Add(new SQLiteParameter(attributeName, attributeValue));
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    log.Info("Successfully update task");
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
        ///update task fields in db.
        ///</summary>
        ///<param name="boardId">the board id</param>
        ///<param name="taskId" >task id</param> 
        ///<param name="attributeName">column to update</param>
        ///<param name="attributeValue">the new value</param>
        /// <returns>return true if the update succeed, and false otherwise</returns>
        public bool Update(int boardId, int taskId, string attributeName, int attributeValue)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"update {taskTableName} set [{attributeName}]=@{attributeName} where BoardID={boardId} AND TaskID={taskId}"
                };
                try
                {
                    command.Parameters.Add(new SQLiteParameter(attributeName, attributeValue));
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    log.Info("Successfully update task");
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
        ///update task fields in db.
        ///</summary>
        ///<param name="boardId">the board id</param>
        ///<param name="taskId" >task id</param> 
        ///<param name="attributeName">column to update</param>
        ///<param name="attributeValue">the new value</param>
        /// <returns>return true if the update succeed, and false otherwise</returns>
        public bool Update(int boardId, int taskId, string attributeName, DateTime attributeValue)
        {
            int res = -1;
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"update {taskTableName} set [{attributeName}]=@{attributeName} where BoardID={boardId} AND TaskID={taskId}"
                };
                try
                {
                    command.Parameters.Add(new SQLiteParameter(attributeName, attributeValue));
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    log.Info("Successfully update task");
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

        //<summary>
        ///insert task to db.
        ///</summary>
        ///<param name="boardId">the board id</param>
        ///<param name="taskId" >task id</param> 
        ///<param name="attributeName">column to update</param>
        ///<param name="attributeValue">the new value</param>
        /// <returns>return true if the update succeed, and false otherwise</returns>
        public bool Insert(TaskDTO newTask)
        {

            using (var connection = new SQLiteConnection(_connectionString))
            {
                int res = -1;
                SQLiteCommand command = new SQLiteCommand(null, connection);
                try
                {
                    connection.Open();
                    command.CommandText = $"INSERT INTO {taskTableName} ({TaskDTO.TaskBoardIDColumnName}, {TaskDTO.TaskIdColumnName}, {TaskDTO.TaskCreationTimeColumnName}, {TaskDTO.TaskTitleColumnName}, {TaskDTO.TaskDescriptionColumnName}, {TaskDTO.TaskDueDateColumnName}, {TaskDTO.TaskColumnOrdinalColumnName}, {TaskDTO.TaskAsigneeColumnName}) " +
                        $"VALUES (@boardIdVal,@taskIdVal,@creationTimeVal,@titleVal,@descriptionVal,@dueDateVal,@columnOrdinalVal,@asigneeVal);";

                    SQLiteParameter boardIdParam = new SQLiteParameter(@"boardIdVal", newTask.BoardId);
                    SQLiteParameter taskIdParam = new SQLiteParameter(@"taskIdVal", newTask.TaskId);
                    SQLiteParameter creationTimeParam = new SQLiteParameter(@"creationTimeVal", newTask.CreationTime);
                    SQLiteParameter titleParam = new SQLiteParameter(@"titleVal", newTask.Title);
                    SQLiteParameter descriptionParam = new SQLiteParameter(@"descriptionVal", newTask.Description);
                    SQLiteParameter dueDateParam = new SQLiteParameter(@"dueDateVal", newTask.DueDate);
                    SQLiteParameter columnOrdinalParam = new SQLiteParameter(@"columnOrdinalVal", newTask.ColumnOrdinal);
                    SQLiteParameter asigneeParam = new SQLiteParameter(@"asigneeVal", newTask.Assignee);



                    command.Parameters.Add(boardIdParam);
                    command.Parameters.Add(taskIdParam);
                    command.Parameters.Add(creationTimeParam);
                    command.Parameters.Add(titleParam);
                    command.Parameters.Add(descriptionParam);
                    command.Parameters.Add(dueDateParam);
                    command.Parameters.Add(columnOrdinalParam);
                    command.Parameters.Add(asigneeParam);
                    command.Prepare();

                    res = command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    throw new Exception(e.ToString());
                }
                finally
                {
                    command.Dispose();
                    connection.Close();
                }
                return res > 0;
            }
        }

        public List<TaskDTO> LoadAllTasks()
        {
            List<TaskDTO> result = Select().Cast<TaskDTO>().ToList();

            return result;
        }

        public List<TaskDTO> LoadBoardTasks(int boardId)
        {
            List<TaskDTO> results = new List<TaskDTO>();
            using (var connection = new SQLiteConnection(_connectionString))
            {
                SQLiteCommand command = new SQLiteCommand(null, connection);
                command.CommandText = $"select * from {_tableName} where {TaskDTO.TaskBoardIDColumnName} = {boardId};";
                SQLiteDataReader dataReader = null;
                try
                {
                    connection.Open();
                    dataReader = command.ExecuteReader();

                    while (dataReader.Read())
                    {
                        results.Add((TaskDTO)ConvertReaderToObject(dataReader));

                    }
                }
                catch (Exception e)
                {
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

        public bool Delete(TaskDTO task)
        {
            int res = -1;

            using (var connection = new SQLiteConnection(_connectionString))
            {
                var command = new SQLiteCommand
                {
                    Connection = connection,
                    CommandText = $"delete from {_tableName} where BoardID={task.BoardId} AND TaskID={task.TaskId}"
                };
                try
                {
                    connection.Open();
                    res = command.ExecuteNonQuery();
                }
                catch (Exception e)
                {
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
