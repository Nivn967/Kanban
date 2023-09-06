using IntroSE.Kanban.Backend.DataAccessLayer;
using log4net;
using log4net.Config;
using System;
using System.IO;
using System.Reflection;
using System.Text.Json.Serialization;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    public class Task
    {
        public int Id { get; set; }
        public DateTime CreationTime { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime DueDate { get; set; }
        [JsonIgnore]
        public int columnOrdinal { get; set; }
        [JsonIgnore]
        public string assignee { get; set; }
        [JsonIgnore]
        private TaskDTO taskDTO;
        [JsonIgnore]
        public TaskDTO TaskDTO { get => taskDTO; set { taskDTO = value; } }
        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);


        public Task(int taskId, int columnOrdinal, DateTime dueDate, string title, string description, string assignee)
        {
            if (DateTime.Compare(dueDate, DateTime.Now) < 0)
                throw new Exception("The due date given is invalid");

            if (String.IsNullOrWhiteSpace(title) || title.Equals(""))
                throw new Exception("Title cant be null or empty");
            else if (title.Length > 50)
                throw new Exception("The given title has more than 50 characters");

            if (description != null && description.Length > 300)
                throw new Exception("The given descreption has more than 300 characters");

            this.Id = taskId;
            this.columnOrdinal = columnOrdinal;
            this.DueDate = dueDate;
            this.Title = title;
            this.Description = description;
            this.CreationTime = DateTime.Now;
            this.assignee = assignee;

            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            log.Info("Starting new Task log!");
        }

        public Task(TaskDTO taskDTO)
        {
            this.Id = taskDTO.TaskId;
            this.columnOrdinal = taskDTO.ColumnOrdinal;
            this.DueDate = taskDTO.DueDate;
            this.Title = taskDTO.Title;
            this.Description = taskDTO.Description;
            this.CreationTime = taskDTO.CreationTime;
            this.assignee = taskDTO.Assignee;
            this.TaskDTO = taskDTO;
        }

        /// <summary>
        /// Edits task descreption
        /// </summary>
        /// <param name="description">Task's descreption. assumed to be initially valid description</param>
        /// <param name="asigneeEmail">Asignee's email. assumed to be initially valid title</param>
        /// <returns>Boolean value if operation succeed</returns>
        public bool EditTaskDescription(string description, string asigneeEmail)
        {
            if (this.assignee == null)
                throw new Exception("No one is assigned to this task");
            if (!asigneeEmail.Equals(assignee))
                throw new Exception("Only the asignee can edit the task");
            if (this.columnOrdinal == 2)
                throw new Exception("The task is done so it cant be edited");
            if (description.Length > 300)
                throw new Exception("The given descreption has more than 300 characters");
            else
            {
                this.Description = description;
                taskDTO.Description = description;
                log.Info("Edited task successfully");
            }
            return true;
        }

        /// <summary>
        /// Edits task DueDate
        /// </summary>
        /// <param name="dueDate">Task's duedate. assumed to be initially valid Duedate</param>
        /// <param name="asigneeEmail">Asignee's email. assumed to be initially valid title</param>
        /// <returns>Boolean value if operation succeed</returns>
        public bool EditTaskDueDate(DateTime dueDate, string asigneeEmail)
        {
            if (this.assignee == null)
                throw new Exception("No one is assigned to this task");
            if (!asigneeEmail.Equals(assignee))
                throw new Exception("Only the asignee can edit the task");
            if (this.columnOrdinal == 2)
                throw new Exception("The task is done so it cant be edited");
            if (DateTime.Compare(dueDate, DateTime.Now) < 0)
                throw new Exception("The due date given is invalid");
            else
            {
                this.DueDate = dueDate;
                taskDTO.DueDate = dueDate;
                log.Info("Edited task successfully");
            }
            return true;
        }

        /// <summary>
        /// Edits task title
        /// </summary>
        /// <param name="title">Task's title. assumed to be initially valid title</param>
        /// <param name="asigneeEmail">Asignee's email. assumed to be initially valid title</param>
        /// <returns>Boolean value if operation succeed</returns>
        public bool EditTaskTitle(string title, string asigneeEmail)
        {
            if (this.assignee == null)
                throw new Exception("No one is assigned to this task");
            if (!asigneeEmail.Equals(assignee))
                throw new Exception("Only the asignee can edit the task");
            if (this.columnOrdinal == 2)
                throw new Exception("The task is done so it cant be edited");
            if (title == null || title.Equals(""))
                throw new Exception("Title can't be empty");
            else if (title.Length > 50)
                throw new Exception("The given title has more than 50 characters");
            else
            {
                this.Title = title;
                taskDTO.Title = title;
                log.Info("Edited task successfully");
            }
            return true;

        }
    }
}