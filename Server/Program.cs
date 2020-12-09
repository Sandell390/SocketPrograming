using System;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            var echo = new EchoServer();

            echo.Start();

            Console.WriteLine("Echo server running");
            Console.ReadLine();
        }
    }
}
