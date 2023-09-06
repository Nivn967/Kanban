using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using IntroSE.Kanban.Backend.DataAccessLayer;
using log4net;
using log4net.Config;

namespace IntroSE.Kanban.Backend.BusinessLayer
{
    public class BoardController
    {
        public Dictionary<int, Board> _boards { get; set; }

        private readonly UserController _uc;
        public int boardCounter { get; set; }

        private static readonly ILog log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public BoardController(UserController uc)
        {
            _boards = new Dictionary<int, Board>();
            _uc = uc;
            boardCounter = 0;
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));
            log.Info("Starting new boardController log!");
        }

        /// <summary>
        /// Add a new board by a user
        /// </summary>
        /// <param name="email"> User's email </param>
        /// <param name="boardName">boards name</param>
        /// <returns></returns>
        public void AddBoard(string email, string boardName)
        {
            if (email == null)
                throw new ArgumentNullException("email cannot be null");

            User u1 = _uc.getUser(email);

            if (!u1.isLogin)
                throw new Exception($"The user did not log in yet");

            if (String.IsNullOrWhiteSpace(boardName) || boardName.Length == 0)
                throw new Exception("Board name cannot be null or empty");

            if (getBoard(boardName, email) != null)
                throw new Exception($"A board with the board name {boardName} is already exists for the user {email}.");

            Board board = new Board(boardName, boardCounter, email, _uc);
            board.boardDTO.InsertBoard();
            _boards.Add(boardCounter, board);
            boardCounter++;
            log.Info("Added board successfully");
        }

        /// <summary>
        /// Removes a board by a user
        /// </summary>
        /// <param name="email"> User's email </param>
        /// <param name="boardName">board's name</param>
        /// <returns></returns>
        public void RemoveBoard(string email, string boardName)
        {
            User u1 = _uc.getUser(email);

            if (!u1.isLogin)
                throw new Exception($"The user did not log in yet");

            Board board = getBoard(boardName, email);

            if (board == null)
                throw new Exception($"Board with boardName {boardName} does not exist for the user {email}");

            if (!board._owner.Equals(email))
                throw new Exception("Only board owner can remove boards");

            //update taskDTO
            foreach (Task t in board._backlog.Values)
                t.TaskDTO.Delete();

            foreach (Task t in board._inProgress.Values)
                t.TaskDTO.Delete();

            foreach (Task t in board._done.Values)
                t.TaskDTO.Delete();

            //update BoardUsersDTO 
            foreach (string email1 in board._members)
            {
                BoardUsersDTO member = new BoardUsersDTO(board._boardId, email1);
                member.DeleteUser();
            }

            board.boardDTO.DeleteBoard();

            board._backlog = null;
            board._inProgress = null;
            board._done = null;

            _boards.Remove(board._boardId);

            log.Info("Removed board successfully");
        }

        /// <summary>
        /// Returns a board by a user
        /// </summary>
        /// <param name="email"> User's email </param>
        /// <param name="boardName">board's name</param>
        /// <returns></returns>
        public Board getBoard(string boardName, string email)
        {
            //verify if a user has a board with the same board name.
            if (boardName == null)
                throw new Exception($"the board name cannot be null");

            foreach (int boardId in _boards.Keys)
            {
                if (boardName.Equals(_boards[boardId]._boardName))
                {
                    Board board = _boards[boardId];
                    if (board._members.Contains(email) || board._owner.Equals(email))
                    {
                        log.Info("Returned board successfully");
                        return board;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Returns all board by a user
        /// </summary>
        /// <param name="email"> User's email </param>
        /// <returns></returns>
        public Dictionary<int, Board> GetUserBoards(string email)
        {
            if (!_uc.userExists(email))
            {
                throw new Exception($"the user does not exist.");
            }

            Dictionary<int, Board> user_boards = new Dictionary<int, Board>();

            foreach (int boardId in _boards.Keys)
            {
                Board board = _boards[boardId];
                if (board._members.Contains(email) || board._owner.Equals(email))
                    user_boards.Add(boardId, board);
            }
            log.Info("Returned boards successfully");
            return user_boards;
        }

        /// <summary>
        /// Verify if a board is existing in the system
        /// </summary>
        /// <param name="email"> User's email </param>
        /// <param name="boardName">board's name</param>
        /// <returns></returns>
        public void verifyBoardExists(string boardName, string email)
        {
            if (getBoard(boardName, email) == null)
                throw new Exception($"Board with boardName {boardName} does not exist for the user {email}");
        }

        /// <summary>
        /// Returnes a boards name
        /// </summary>
        /// <param name="boardId"> boards's ID</param>
        /// <returns></returns>
        public string GetBoardName(int boardId)
        {
            if (_boards.ContainsKey(boardId))
            {
                log.Info("Returned board name successfully");
                return _boards[boardId]._boardName;
            }
            else
                throw new Exception($"Board (with boardId {boardId}) does not exist in the system");
        }

        /// <summary>
        /// Joins a user to a board
        /// </summary>
        /// <param name="email"> users email</param>
        /// <param name="boardID"> boards's ID</param>
        /// <returns></returns>
        public void JoinBoard(string email, int boardID)
        {
            User u1 = _uc.getUser(email);
            if (!u1.isLogin)
                throw new Exception($"The user did not log in yet");

            if (_boards.ContainsKey(boardID))
            {
                if (getBoard(_boards[boardID]._boardName, email) != null)
                    throw new Exception($"user (email: {email}) cannot join the board as he already have board with the same board name");
                if (_boards[boardID]._members.Contains(email) | _boards[boardID]._owner.Equals(email))
                    throw new Exception($"user (email: {email}) is already a member of the board");

                BoardUsersDTO member = new BoardUsersDTO(boardID, email);
                member.InsertUser();
                _boards[boardID]._members.Add(email);
                log.Info("Joined to board successfully");
            }
            else
                throw new Exception($"Board (with Id {boardID}) does not exist in the system");
        }

        /// <summary>
        /// Deleting a user from a board
        /// </summary>
        /// <param name="email"> users email</param>
        /// <param name="boardID"> boards's ID</param>
        /// <returns></returns>
        public void LeaveBoard(string email, int boardID)
        {
            User u1 = _uc.getUser(email);
            if (!u1.isLogin)
                throw new Exception($"The user did not log in yet");

            if (_boards.ContainsKey(boardID))
            {
                if (getBoard(_boards[boardID]._boardName, email) == null)
                    throw new Exception($"user (email: {email}) is not a member in the board (Id: {boardID})");

                if (_boards[boardID]._owner.Equals(email))
                    throw new Exception("A board owner cannot leave the board");

                foreach (Task task in _boards[boardID]._backlog.Values)
                {
                    if (task.assignee.Equals(email))
                    {
                        task.TaskDTO.Assignee = null;
                        task.assignee = null;

                    }
                }

                foreach (Task task in _boards[boardID]._inProgress.Values)
                {
                    if (task.assignee.Equals(email))
                    {
                        task.TaskDTO.Assignee = null;
                        task.assignee = null;
                    }
                }

                BoardUsersDTO member = new BoardUsersDTO(boardID, email);
                member.DeleteUser();

                _boards[boardID]._members.Remove(email);

                log.Info("Left board successfully");

            }
            else
                throw new Exception($"Board (with Id {boardID}) does not exist in the system");
        }

        /// <summary>
        /// return all the tasks that at InProgress column and the user is assigned to
        /// </summary>
        /// <param name="email">the user email</param>
        /// <returns>return all the tasks that at InProgress column and the user is assigned to</returns>
        public List<Task> getAssigneeInProgressTasks(string email)
        {
            Dictionary<int, Board> Boards = GetUserBoards(email);
            List<Task> AssigneeInProgressTasks = new List<Task>();

            foreach (int boardId in Boards.Keys)
            {
                Board board = Boards[boardId];
                foreach (Task task in board._inProgress.Values)
                {
                    if (task.assignee.Equals(email))
                        AssigneeInProgressTasks.Add(task);
                }
            }
            log.Info("Returned inProgress tasks successfully");
            return AssigneeInProgressTasks;
        }

        /// <summary>
        /// load all the boards in the db, calling LoadMembers, LoadTasks
        /// </summary>
        public void LoadBoards()
        {
            BoardControllerDTO BCD = new BoardControllerDTO();
            List<BoardDTO> BoardDTO_lst = BCD.LoadBoards();
            foreach (BoardDTO board_dto in BoardDTO_lst)
            {
                Board board = new Board(board_dto);

                _boards.Add(board_dto.BoardId, board);
            }

            log.Info($"inserted to businessLayer BoardController");

            LoadBoardMembers();
            LoadTasks();
        }

        /// <summary>
        /// load all the board's members in the db
        /// </summary>
        private void LoadBoardMembers()
        {
            List<BoardUsersDTO> members = new BoardUsersController().LoadBoardMembers();
            foreach (BoardUsersDTO member in members)
            {
                Board board = _boards[member.BoardId];
                board._members.Add(member.Email);
            }
            log.Info("inserted board to businessLayer BoardController");
        }

        /// <summary>
        /// load all the tasks in the db
        /// </summary>
        public void LoadTasks()
        {
            foreach (KeyValuePair<int, Board> board in _boards)
            {
                board.Value.LoadBoardTasks();
            }
            log.Info("all the tasks inserted to businessLayer");
        }


        /// <summary>
        /// delete all the board,members and tasks in the db
        /// </summary>
        public void DeleteAllBoards()
        {
            if (!new BoardUsersController().DeleteAll())
            {
                log.Error("cant delete from DAL");
                throw new ArgumentException("Unable to delete all board members");
            }
            else
            {
                foreach(Board b in _boards.Values) 
                    b._members.Clear();
            }
            if (!new TaskControllerDTO().DeleteAll())
            {
                log.Error("cant delete from DAL");
                throw new ArgumentException("Unable to delete all boards");
            }
            else
            {
                foreach(Board b in _boards.Values)
                {
                    b._backlog.Clear(); 
                    b._inProgress.Clear();  
                    b._done.Clear();    
                }
            }
            log.Info("Deleted all boards from DAL");
            if (!new BoardControllerDTO().DeleteAll())
            {
                log.Error("cant delete from DAL");
                throw new ArgumentException("Unable to delete all boards");
            }
            else
            {
                _boards.Clear();    
            }
        }
    }
}
