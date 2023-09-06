using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;

namespace Frontend.Model
{
    public class BoardModel : NotifiableModelObject
    {
        private int _id;
        public int Id
        {
            get => _id;
            set
            {
                this._id = value;
                RaisePropertyChanged("Id");
            }
        }
        private string _boardName;
        public string BoardName
        {
            get => _boardName;
            set
            {
                this._boardName = "Name: " + value;
                RaisePropertyChanged("BoardName");
            }
        }


        public UserModel user;
        public ObservableCollection<TaskModel> Tasks_backlog { get; set; }
        public ObservableCollection<TaskModel> Tasks_inProgress { get; set; }
        public ObservableCollection<TaskModel> Tasks_done { get; set; }


        public BoardModel(BackendController controller, int boardId, string boardName, UserModel user) : base(controller)
        {
            this.user = user;
            Tasks_backlog = new ObservableCollection<TaskModel>(controller.GetAllTasksIds(user.Email, boardName, 0).
                Select((c, i) => new TaskModel(controller, controller.GetTask(user.Email, boardName, c, 0),user)).ToList());

            Tasks_inProgress = new ObservableCollection<TaskModel>(controller.GetAllTasksIds(user.Email, boardName, 1).
                Select((d, j) => new TaskModel(controller, controller.GetTask(user.Email, boardName, d, 1), user)).ToList());

            Tasks_done = new ObservableCollection<TaskModel>(controller.GetAllTasksIds(user.Email, boardName, 2).
                Select((e, k) => new TaskModel(controller, controller.GetTask(user.Email, boardName, e, 2), user)).ToList());

            Id = boardId;
            BoardName = boardName;
        }

        public BoardModel(BackendController controller, (int Id, string BoardName) board, UserModel user) : this(controller, board.Id, board.BoardName, user) { }

    }
}
