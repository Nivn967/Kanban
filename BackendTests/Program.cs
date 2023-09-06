using System;
using System.Text.Json;
using BackendTests;

using IntroSE.Kanban.Backend.ServiceLayer;
using IntroSE.Kanban.Backend.BusinessLayer;


namespace IntroSE.Kanban.Frontend
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome!\n ");
            Backend.ServiceLayer.ServiceFactory s = new ServiceFactory();

            Console.WriteLine("User test:");
            new UserTests(s.userService).Tests();

            Console.WriteLine("\nBoard test:");
            new BoardTests(s.boardService).RunTests();

            Console.WriteLine("\nTask test:");
            new TaskTests(s.taskService).RunTests();
        }
    }

}
