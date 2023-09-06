using IntroSE.Kanban.Backend.ServiceLayer;

namespace Frontend.Model
{
    public class TaskModel : NotifiableModelObject
    {
        private int _id;
        public int Id
        {
            get => _id;
        }
        private string _title;
        public string Title
        {
            get => _title;
        }
        private string _descreption;
        public string Descreption
        {
            get => _descreption;
        }
        private string UserEmail; //storing this user here is an hack becuase static & singletone are not allowed.
        public TaskModel(BackendController controller, int id, string title, string descreption, string user_email) : base(controller)
        {
            _id = id;
            _title = title;
            _descreption = descreption;
            UserEmail = user_email;
        }

        public TaskModel(BackendController controller, (int Id, string Title, string description) task, UserModel user) : this(controller, task.Id, task.Title, task.description, user.Email) { }

    }
}
