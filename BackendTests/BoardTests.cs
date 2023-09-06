using IntroSE.Kanban.Backend.ServiceLayer;
using System;
using System.Text.Json;

namespace IntroSE.Kanban.Frontend
{
    class BoardTests
    {
        private readonly Board board;
        public string workingEmail;
        public string workingEmail2;
        public string workingEmail3;
        public string board1;
        public string board2;

        public BoardTests(Board board)
        {
            this.board = board;
            workingEmail = "niv@gmail.com"; //Board1 //0 //owner
            workingEmail2 = "tomer@gmail.com"; //Board1 //0
            workingEmail3 = "nitzki@gmail.com"; //Board2 //2
            board1 = "Board1";
            board2 = "Board2";
        }
        public void RunTests()
        {
            AddBoardTest();  //implemented
            RemoveBoardTest();  //implemented
            LimitColumnTest();  //implemented
            JoinBoardTest();  //implemented
            TransferOwnershipTest();
            LeaveBoardTest();
        }

        /// <summary>
        /// Verify the ability to add boards 
        /// </summary>
        /// <returns></returns>
        public void AddBoardTest()
        {
            //Verify adding new board by niv@gmail.com
            string res1 = board.AddBoard(workingEmail, board1);
            Response r1 = JsonSerializer.Deserialize<Response>(res1);

            if (r1.ErrorOccured)
            {
                Console.WriteLine(r1.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Board added succesfully"); //should succeed
            }

            //Verify adding new board by tomer@gmail.com (two different users can have the same boards names)
            string res2 = board.AddBoard(workingEmail2, board1);
            Response r2 = JsonSerializer.Deserialize<Response>(res2);

            if (r2.ErrorOccured)
            {
                Console.WriteLine(r2.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Board added succesfully"); //should succeed
            }

            //Verify adding new board by nitzki@gmail.com
            string res3 = board.AddBoard(workingEmail3, board2);
            Response r3 = JsonSerializer.Deserialize<Response>(res3);

            if (r3.ErrorOccured)
            {
                Console.WriteLine(r3.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Board added succesfully"); //should succeed
            }

            //Verify user cannot have boards with same names
            string res4 = board.AddBoard(workingEmail3, board2);
            Response r4 = JsonSerializer.Deserialize<Response>(res4);

            if (r4.ErrorOccured)
            {
                Console.WriteLine(r4.ErrorMessage); //should return an error message
            }
            else
            {
                Console.WriteLine("Board added succesfully");
            }

            //Verify board name cannot be null or empty
            string res5 = board.AddBoard(workingEmail3, null);
            Response r5 = JsonSerializer.Deserialize<Response>(res5);

            if (r5.ErrorOccured)
            {
                Console.WriteLine(r5.ErrorMessage);  //should return an error message
            }
            else
            {
                Console.WriteLine("Board added succesfully");
            }

        }

        /// <summary>
        /// Verify the ability to remove board
        /// </summary>
        /// <returns></returns>
        public void RemoveBoardTest()
        {
            //Verify tomer@gmail.com can remove existing board
            string res1 = board.RemoveBoard(workingEmail2, board1);
            Response r1 = JsonSerializer.Deserialize<Response>(res1);

            if (r1.ErrorOccured)
            {
                Console.WriteLine(r1.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Board removed successfully"); //should succeed
            }

            //Verify tomer@gmail.com can't remove unexisting board
            string res2 = board.RemoveBoard(workingEmail2, board1);
            Response r2 = JsonSerializer.Deserialize<Response>(res2);

            if (r2.ErrorOccured)
            {
                Console.WriteLine(r2.ErrorMessage);    //should return error message
            }
            else
            {
                Console.WriteLine("Board removed successfully");
            }

            //Verify unexisting email cant remove existing board
            string res3 = board.RemoveBoard("notExistingEmail@gmail.com", board1);
            Response r3 = JsonSerializer.Deserialize<Response>(res3);

            if (r3.ErrorOccured)
            {
                Console.WriteLine(r3.ErrorMessage);    //should return error message
            }
            else
            {
                Console.WriteLine("Board removed successfully");
            }
        }

        /// <summary>
        /// Verify the ability to limit columns on a board 
        /// </summary>
        /// <returns></returns>
        public void LimitColumnTest()
        {
            //Verify "backlog" column support limiting the maximum number of its tasks. 
            string res1 = board.LimitColumn(workingEmail, board1, 10, 0);
            Response r1 = JsonSerializer.Deserialize<Response>(res1);

            if (r1.ErrorOccured)
            {
                Console.WriteLine(r1.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Limited Column successfully"); //should succeed
            }


            //Verify "in Progress" column support limiting the maximum number of its tasks. 
            string res2 = board.LimitColumn(workingEmail, board1, 40, 1);
            Response r2 = JsonSerializer.Deserialize<Response>(res2);

            if (r2.ErrorOccured)
            {
                Console.WriteLine(r2.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Limited Column successfully"); //should succeed
            }


            //Verify "Done" column support limiting the maximum number of its tasks. 
            string res3 = board.LimitColumn(workingEmail3, board2, 19, 2);
            Response r3 = JsonSerializer.Deserialize<Response>(res3);

            if (r3.ErrorOccured)
            {
                Console.WriteLine(r3.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Limited Column successfully"); //should succeed
            }

            //Verify negative limit is not supportod
            string res4 = board.LimitColumn(workingEmail2, board1, -5, 2);
            Response r4 = JsonSerializer.Deserialize<Response>(res4);

            if (r4.ErrorOccured)
            {
                Console.WriteLine(r4.ErrorMessage); //should return an error message
            }
            else
            {
                Console.WriteLine("Limited Column successfully");
            }

            //Verify limit unexisiting column get failed
            string res5 = board.LimitColumn(workingEmail2, board1, 5, 4);
            Response r5 = JsonSerializer.Deserialize<Response>(res5);

            if (r5.ErrorOccured)
            {
                Console.WriteLine(r5.ErrorMessage); //should return an error message
            }
            else
            {
                Console.WriteLine("Limited Column successfully");
            }
        }

        /// <summary>
        /// Verify the ability to Join to a board 
        /// </summary>
        /// <returns></returns>
        public void JoinBoardTest()
        {
            //Verify tomer@gmail.com join to boardid 0
            string res1 = board.JoinBoard(workingEmail2, 0);
            Response r1 = JsonSerializer.Deserialize<Response>(res1);

            if (r1.ErrorOccured)
            {
                Console.WriteLine(r1.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Joined board successfully");  //should succeed
            }

            //Verify email cannot join twice to the same board
            string res2 = board.JoinBoard(workingEmail2, 0);
            Response r2 = JsonSerializer.Deserialize<Response>(res2);

            if (r2.ErrorOccured)
            {
                Console.WriteLine(r2.ErrorMessage);   //should return an error message
            }
            else
            {
                Console.WriteLine("Joined board successfully");
            }

            //Verify unexisting email cannot join to a board
            string res3 = board.JoinBoard("notExistingEmail@gmail.com", 0);
            Response r3 = JsonSerializer.Deserialize<Response>(res3);

            if (r3.ErrorOccured)
            {
                Console.WriteLine(r3.ErrorMessage);   //should return an error message
            }
            else
            {
                Console.WriteLine("Joined board successfully");
            }

            //Verify email can to join a board
            string res4 = board.JoinBoard(workingEmail3, 0);
            Response r4 = JsonSerializer.Deserialize<Response>(res4);

            if (r4.ErrorOccured)
            {
                Console.WriteLine(r4.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Joined board successfully");  //should succeed
            }
        }

        /// <summary>
        /// Verify the ability to transfer ownership on a board 
        /// </summary>
        /// <returns></returns>
        public void TransferOwnershipTest()
        {

            //Verify owner can transfer ownership
            string res1 = board.TransferOwnership(workingEmail, workingEmail2, board1);
            Response r1 = JsonSerializer.Deserialize<Response>(res1);

            if (r1.ErrorOccured)
            {
                Console.WriteLine(r1.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Ownership transfered successfully");  //should succeed
            }

            //Verify owner can transfer ownership
            string res2 = board.TransferOwnership(workingEmail2, workingEmail, board1);
            Response r2 = JsonSerializer.Deserialize<Response>(res2);

            if (r2.ErrorOccured)
            {
                Console.WriteLine(r2.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Ownership transfered successfully");  //should succeed
            }

            //Verify that someone who isnt owner cant transfer ownership
            string res3 = board.TransferOwnership(workingEmail2, workingEmail, board1);
            Response r3 = JsonSerializer.Deserialize<Response>(res3);

            if (r3.ErrorOccured)
            {
                Console.WriteLine(r3.ErrorMessage); //should return an error message
            }
            else
            {
                Console.WriteLine("Ownership transfered successfully");
            }
        }

        public void LeaveBoardTest()
        {
            //Verify someone in the board can leave board
            string res1 = board.LeaveBoard(workingEmail3, 0);
            Response r1 = JsonSerializer.Deserialize<Response>(res1);

            if (r1.ErrorOccured)
            {
                Console.WriteLine(r1.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Left board successfully");  //should succeed
            }

            //Verify someone not in board cant perform leaving
            string res2 = board.LeaveBoard(workingEmail3, 0);
            Response r2 = JsonSerializer.Deserialize<Response>(res2);

            if (r2.ErrorOccured)
            {
                Console.WriteLine(r2.ErrorMessage);
            }
            else
            {
                Console.WriteLine("Left board successfully");  //should succeed
            }

            //Verify unexisting email cant preform leaving
            string res3 = board.LeaveBoard("notExistingEmail@gmail.com", 0);
            Response r3 = JsonSerializer.Deserialize<Response>(res3);

            if (r3.ErrorOccured)
            {
                Console.WriteLine(r3.ErrorMessage);  //should return an error message
            }
            else
            {
                Console.WriteLine("Left board successfully");
            }
        }
    }
}
