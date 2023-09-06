using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.Reflection;
using System.IO;

namespace IntroSE.Kanban.Backend.DataAccessLayer
{
    public abstract class DTO
    {
        protected DalController _controller;
        public DalController _Controller { get => _controller; set { _controller = value; } }
        public DTO(DalController controller)
        {
            _Controller = controller;
        }
    }
}
