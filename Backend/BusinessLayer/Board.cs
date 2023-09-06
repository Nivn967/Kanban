using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using IntroSE.Kanban.Backend.DataAccessLayer;
using log4net;
using log4net.Config;

namespace IntroSE.Kanban.Backend.BusinessLayer

{
    public class Board
    {

        public Dictionary<int, Task> _backlog { get; set; }
        public Dictionary<int, Task> _inProgress { get; set; }
        public Dictionary<int, Task> _done { get; set; }
        public int[] _columnsLimit { get; set; }
        public string _boardName { get; set; }
        public int _boardId { get; set; }
        public string _owner { get; set; }
        public List<string> _members { get; set; }
        public int taskCounter { get; set; }

        public BoardDTO boardDTO;

        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        public Board(string boardName, int boardId, string owner, UserController uc)
        {
            if (owner == null)
                throw new Exception("Email cannot be null");
            _columnsLimit = new int[3];
            for (int i = 0; i < _columnsLimit.Length; i++)
            {
                _columnsLimit[i] = -1; //no limit by default
            }
            _backlog = new Dictionary<int, Task>();
            _inProgress = new Dictionary<int, Task>();
            _done = new Dictionary<int, Task>();
            _boardName = boardName;
            _owner = owner;
            taskCounter = 0;
            _members = new List<string>();
            _boardId = boardId;
            boardDTO = new BoardDTO(boardId, boardName, owner, -1, -1, -1);

            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            log.Info("Starting new board log!");
        }

        public Board(BoardDTO boardDTO1)
        {
            _members = new List<string>();
            _columnsLimit = new int[3];
            _backlog = new Dictionary<int, Task>();
            _inProgress = new Dictionary<int, Task>();
            _done = new Dictionary<int, Task>();

            this._boardId = boardDTO1.BoardId;
            this._boardName = boardDTO1.BoardName;
            this._owner = boardDTO1.Owner;
            this._columnsLimit[0] = boardDTO1.BacklogLimit;
            this._columnsLimit[1] = boardDTO1.InProgressLimit;
            this._columnsLimit[2] = boardDTO1.DoneLimit;
            boardDTO = boardDTO1;
        }

        /// <summary>
        /// Advances task in a board
        /// if the maximum number of tasks in Backlog column is reached, then the task is not added.
        /// </summary>
        /// <param name="taskId">Task's id.</param>
        /// <param name="columnOrdinal">Task's Column ordinal</param>
        /// <param name="email"> User's email </param>
        /// <returns></returns>
        public void AdvanceTask(int taskId, int columnOrdinal, string email)
        {
            if (columnOrdinal == 0)
            {
                if (_backlog.ContainsKey(taskId))
                {
                    Task task = _backlog[taskId];

                    if (task.assignee != null && task.assignee.Equals(email))
                    {
                        _backlog.Remove(taskId);
                        _inProgress[taskId] = task;
                        task.columnOrdinal = columnOrdinal + 1;
                        task.TaskDTO.ColumnOrdinal = task.columnOrdinal;
                        log.Info("Task advanced successfully");
                    }
                    else
                    {
                        log.Error("The advance failed, Only the assignee can move tasks");
                        throw new Exception("Only the assignee can move tasks");
                    }

                }
                else
                {
                    log.Error("The given task dosent exist in this backlog column");
                    throw new Exception($"The given task (task id {taskId}) does not exist in backlog column");
                }
            }
            else if (columnOrdinal == 1)
            {
                if (_inProgress.ContainsKey(taskId))
                {
                    Task task = _inProgress[taskId];

                    if (task.assignee != null && task.assignee.Equals(email))
                    {
                        _inProgress.Remove(taskId);
                        _done[taskId] = task;
                        task.columnOrdinal = columnOrdinal + 1;
                        task.TaskDTO.ColumnOrdinal = task.columnOrdinal;
                        log.Info("Task advanced successfully");
                    }
                    else
                    {
                        log.Error("The advance failed, Only the assignee can move tasks");
                        throw new Exception("Only the assignee can move tasks");
                    }
                }
                else
                {
                    log.Error("The given task dosent exist in this backlog column");
                    throw new Exception($"The given task (task id {taskId}) does not exist in backlog column");
                }
            }
            else if (columnOrdinal == 2)
            {
                if (_done.ContainsKey(taskId))
                {
                    log.Error("The given task is already done!");
                    throw new Exception($"taskId {taskId} has already done! no movement is allowed");
                }
                else
                {
                    log.Error("The given task is already done!");
                    throw new Exception($"The given task (task id {taskId}) does not exist in done column");
                }
            }
            else
            {
                log.Error("The given column number is illegal");
                throw new Exception("The given column does not exist (columnOrdinal can only get values between 0-2)");
            }

        }

        /// <summary>
        /// Add a new task to a board
        /// if the maximum number of tasks in Backlog column is reached, then the task is not added.
        /// </summary>
        /// <param name="email"> User's email </param>
        /// <param name="columnOrdinal">Task's Column ordinal</param>
        /// <param name="dueDate"> Task's due date </param>
        /// <param name="title"> Task's title </param>
        /// <param name="description"> Task's description </param>
        /// <returns></returns>
        public void AddTask(string email, int columnOrdinal, DateTime dueDate, string title, string description)
        {
            if (!_owner.Equals(email))
            {
                if (!_members.Contains(email))
                {
                    log.Error("Only a board member can add a new task!");
                    throw new Exception("A new task can be added by one of the board members only!");
                }
            }

            if (columnOrdinal != 0)
            {
                log.Error("A new task can be added to Backlog column only!");
                throw new Exception("A new task can be added to Backlog column only!");
            }

            Task task = new Task(taskCounter, columnOrdinal, dueDate, title, description, null);
            task.TaskDTO = new TaskDTO(_boardId, taskCounter, task.CreationTime, title, description, dueDate, columnOrdinal, null);

            if (_backlog.Count < _columnsLimit[0] | _columnsLimit[0] == -1)
            {
                _backlog[taskCounter] = task;
                task.TaskDTO.Insert();
                log.Info("Task added successfully");
            }
            else
                throw new Exception("You have reached the maximum number of tasks in Backlog column! no more tasks can be added");

            taskCounter++;

        }

        /// <summary>
        /// Adssign a user in a board to a task
        /// if the maximum number of tasks in Backlog column is reached, then the task is not added.
        /// </summary>
        /// <param name="email">Assigning user's email </param>
        /// <param name="columnOrdinal">Task's Column ordinal</param>
        /// <param name="taskID"> Task's ID </param>
        /// <param name="emailAssignee"> Assignee user's email </param>
        /// <returns></returns>
        public void AssignTask(string email, int columnOrdinal, int taskID, string emailAssignee)
        {
            Task task;

            if (!_members.Contains(emailAssignee) && !_owner.Equals(emailAssignee))
                throw new Exception("Task can be assigned to board member only");

            if (columnOrdinal == 0)
            {
                if (_backlog.ContainsKey(taskID))
                    task = _backlog[taskID];
                else
                {
                    log.Error($"The given task (task id {taskID}) does not exist in backlog column");
                    throw new Exception($"The given task (task id {taskID}) does not exist in backlog column");
                }
            }
            else if (columnOrdinal == 1)
            {
                if (_inProgress.ContainsKey(taskID))
                    task = _inProgress[taskID];
                else
                {
                    log.Error($"The given task (task id {taskID}) does not exist in inprogress column");
                    throw new Exception($"The given task (task id {taskID}) does not exist in in Progress column");
                }
            }
            else if (columnOrdinal == 2)
            {
                if (_done.ContainsKey(taskID))
                    throw new Exception($"taskId {taskID} has already done! no movement is allowed");
                else
                {
                    log.Error($"The given task (task id {taskID}) does not exist in done column");
                    throw new Exception($"The given task (task id {taskID}) does not exist in done column");
                }
            }
            else
                throw new Exception("The given column does not exist (columnOrdinal can only get values between 0-2)");

            if (task.assignee != null)
            {
                if (task.assignee.Equals(email))
                {
                    task.assignee = emailAssignee;
                    task.TaskDTO.Assignee = emailAssignee;
                    log.Info("Task asignee updated successfully");
                }
                else
                    throw new Exception("An assinged task can be changed by its assignee only");
            }
            else if (_members.Contains(email) || _owner.Equals(email))
            {
                task.assignee = emailAssignee;
                task.TaskDTO.Assignee = emailAssignee;
                log.Info("Task asignee updated successfully");
            }
            else
            {
                log.Error("An unassigned task can changed by board member only!");
                throw new Exception("An unassigned task can changed by board member only!");
            }
        }


        /// <summary>
        /// Limit the number of tasks in a column 
        /// if the given limit is greater than the current amount of tasks in this column, then no limit is made.
        /// </summary>
        /// <param name="limit">Column limitation</param>
        /// <param name="columnOrdinal">Column ordinal</param>
        /// <returns name=></returns>
        public void LimitColumn(int limit, int columnOrdinal)
        {
            int currentTaskAmount;
            if (columnOrdinal == 0)
                currentTaskAmount = _backlog.Count;
            else if (columnOrdinal == 1)
                currentTaskAmount = _inProgress.Count;
            else if (columnOrdinal == 2)
                currentTaskAmount = _done.Count;
            else
            {
                log.Error("The given column does not exist (columnOrdinal can only get values between 0-2)");
                throw new Exception("The given column does not exist (columnOrdinal can only get values between 0-2)");
            }

            if (limit < 0 & limit != -1)
            {
                log.Error("Invalid limit! Please enter a positive value");
                throw new Exception("Invalid limit! Please enter a positive value");
            }

            if (limit >= currentTaskAmount)
            {
                if (columnOrdinal == 0)
                    boardDTO.BacklogLimit = limit;
                else if (columnOrdinal == 1)
                    boardDTO.InProgressLimit = limit;
                else
                    boardDTO.DoneLimit = limit;

                _columnsLimit[columnOrdinal] = limit;
                log.Info("Column limit updated successfully");
            }
            else
            {
                log.Error($"Cant limit {getColumnName(columnOrdinal)} column! the given limit is less than current number of tasks in this column!");
                throw new Exception($"Cant limit {getColumnName(columnOrdinal)} column! the given limit is less than current number of tasks in this column!");
            }
        }

        public int getColumnLimit(int columnOrdinal)
        {
            if (columnOrdinal < 0 | columnOrdinal > 2)
            {
                log.Error("The given column does not exist (columnOrdinal can only get values between 0-2)");
                throw new Exception("The given column does not exist (columnOrdinal can only get values between 0-2)");
            }
            log.Info("Column limit returned successfully");
            return _columnsLimit[columnOrdinal];
        }

        /// <summary>
        /// get column name by it's ordinal number.
        /// </summary>
        /// <param name="columnOrdinal">Task's Column ordinal</param>
        /// <returns></returns>
        public string getColumnName(int columnOrdinal)
        {
            if (columnOrdinal == 0)
            {
                log.Info("Column name returned successfully");
                return "Backlog";
            }
            else if (columnOrdinal == 1)
            {
                log.Info("Column name returned successfully");
                return "In Progress";
            }
            else if (columnOrdinal == 2)
            {
                log.Info("Column name returned successfully");
                return "Done";
            }
            else
            {
                log.Error("The given column does not exist (columnOrdinal can only get values between 0-2)");
                throw new Exception("The given column does not exist (columnOrdinal can only get values between 0-2)");
            }
        }

        /// <summary>
        /// get task by taskId (if exists in the board). 
        /// </summary>
        /// <param name="taskId">Task's id.</param>
        /// <param name="columnOrdinal">Task's status.</param>
        /// <returns></returns>
        public Task getTask(int taskId, int columnOrdinal)
        {
            if (columnOrdinal == 0)
            {
                if (_backlog.ContainsKey(taskId))
                {
                    log.Info("Task returned successfully");
                    return _backlog[taskId];
                }
                else
                {
                    log.Error($"The given task (task id {taskId}) does not exist in backlog column");
                    throw new Exception($"The given task (task id {taskId}) does not exist in backlog column");
                }
            }
            else if (columnOrdinal == 1)
            {
                if (_inProgress.ContainsKey(taskId))
                {
                    log.Info("Task returned successfully");
                    return _inProgress[taskId];
                }
                else
                {
                    log.Error($"The given task (task id {taskId}) does not exist in in progress column");
                    throw new Exception($"The given task (task id {taskId}) does not exist in in progress column");
                }
            }
            else if (columnOrdinal == 2)
            {
                if (_done.ContainsKey(taskId))
                {
                    log.Info("Task returned successfully");
                    return _done[taskId];
                }
                else
                {
                    log.Error($"The given task (task id {taskId}) does not exist in done column");
                    throw new Exception($"The given task (task id {taskId}) does not exist in done column");
                }
            }
            else
            {
                log.Error("The given column does not exist (columnOrdinal can only get values between 0-2)");
                throw new Exception("The given column does not exist (columnOrdinal can only get values between 0-2)");
            }
        }

        /// <summary>
        /// get boards specific column tasks. 
        /// </summary>
        /// <param name="columnOrdinal">Task's status.</param>
        /// <returns></returns>
        public Dictionary<int, Task> getColumn(int columnOrdinal)
        {
            if (columnOrdinal == 0)
            {
                log.Info("Column returned successfully");
                return _backlog;
            }
            else if (columnOrdinal == 1)
            {
                log.Info("Column returned successfully");
                return _inProgress;
            }
            else if (columnOrdinal == 2)
            {
                log.Info("Column returned successfully");
                return _done;
            }
            else
            {
                log.Error("The given column does not exist (columnOrdinal can only get values between 0-2)");
                throw new Exception("The given column does not exist (columnOrdinal can only get values between 0-2)");
            }

        }
        /// <summary>
        /// Transfer owner to another owner. 
        /// </summary>
        /// <param name="currentOwnerEmail">board's current owner.</param>
        /// <param name="newOwnerEmail">board's new owner.</param>
        /// <returns></returns>
        public void TransferOwnership(string currentOwnerEmail, string newOwnerEmail)
        {
            if (!currentOwnerEmail.Equals(_owner))
            {
                log.Info("only the board owner get transfer the ownership!");
                throw new Exception("only the board owner get transfer the ownership!");
            }

            if (_members.Contains(newOwnerEmail))
            {
                //update BoardUsersDTO
                boardDTO.Owner = newOwnerEmail;
                BoardUsersDTO member1 = new BoardUsersDTO(_boardId, newOwnerEmail);
                member1.DeleteUser();
                BoardUsersDTO member2 = new BoardUsersDTO(_boardId, currentOwnerEmail);
                member2.InsertUser();

                _owner = newOwnerEmail;
                _members.Remove(newOwnerEmail);
                _members.Add(currentOwnerEmail);
                log.Info("Owner transfered successfully");
            }
            else
                throw new Exception("The ownership can be tranffered to one of the board members only");
        }

        /// <summary>
        /// Loads all of boards tasks. 
        /// </summary>
        /// <returns></returns>
        public void LoadBoardTasks()
        {
            TaskControllerDTO TDC = new TaskControllerDTO();
            List<TaskDTO> TaskDTOList = new List<TaskDTO>();
            TaskDTOList = TDC.LoadBoardTasks(_boardId);
            foreach (TaskDTO taskDTO in TaskDTOList)
            {
                Task task = new Task(taskDTO);
                //task.TaskDTO = taskDTO;
                if (taskDTO.ColumnOrdinal == 0)
                    _backlog[task.Id] = task;
                else if (taskDTO.ColumnOrdinal == 1)
                    _inProgress[task.Id] = task;
                else
                    _done[task.Id] = task;
            }
            log.Info("Board tasks loaded successfully");
        }
    }
}