using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    public class TaskDTO : DTO
    {
        private int taskId;
        public int TaskId { get => taskId; }
        private DateTime creationTime;
        public DateTime CreationTime { get => creationTime; }
        private string title;
        public string Title { get => title; set { title = value; ((TaskControllerDTO)_controller).Update(BoardId, TaskId, TaskTitleColumnName, value); } }
        private string description;
        public string Description { get => description; set { description = value; ((TaskControllerDTO)_controller).Update(BoardId, TaskId, TaskDescriptionColumnName, value); } }
        private DateTime dueDate;
        public DateTime DueDate { get => dueDate; set { dueDate = value; ((TaskControllerDTO)_controller).Update(BoardId, TaskId, TaskDueDateColumnName, value); } }
        private int columnOrdinal;
        public int ColumnOrdinal { get => columnOrdinal; set { columnOrdinal = value; ((TaskControllerDTO)_controller).Update(BoardId, TaskId, TaskColumnOrdinalColumnName, value); } }
        private string assignee;
        public string Assignee { get => assignee; set { assignee = value; ((TaskControllerDTO)_controller).Update(BoardId, TaskId, TaskAsigneeColumnName, value); } }
        private int boardId;
        public int BoardId { get => boardId; }
        public TaskControllerDTO Controller;

        public const string TaskBoardIDColumnName = "BoardID";
        public const string TaskIdColumnName = "TaskID";
        public const string TaskCreationTimeColumnName = "CreationTime";
        public const string TaskTitleColumnName = "Title";
        public const string TaskDescriptionColumnName = "Description";
        public const string TaskDueDateColumnName = "DueDate";
        public const string TaskColumnOrdinalColumnName = "ColumnOrdinal";
        public const string TaskAsigneeColumnName = "Asignee";

        public TaskDTO(int boardId, int taskId, DateTime creationTime, string title, string description, DateTime dueDate, int columnOrdinal, string assignee) : base(new TaskControllerDTO())
        {
            this.boardId = boardId;
            this.taskId = taskId;
            this.creationTime = creationTime;
            this.title = title;
            this.description = description;
            this.dueDate = dueDate;
            this.columnOrdinal = columnOrdinal;
            this.assignee = assignee;
        }

        public bool Insert()
        {
            if (((TaskControllerDTO)_controller).Insert(this))
            {
                return true;
            }
            throw new AggregateException("Unable to insert this task to the DB");
        }

        public bool Delete()
        {
            if (((TaskControllerDTO)_controller).Delete(this))
            {
                return true;
            }

            throw new AggregateException("Unable to delete this task to the DB");
        }


    }
}
