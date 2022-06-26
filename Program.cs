using System;

namespace Import
{
    class Program
    {
        static void Main(string[] args)
        {
            //check if input an argument is supplied.
            if (args is null)
            {
                Console.WriteLine("Please enter an argument, and try again.");
                return;
            }
            else
            {
                Console.WriteLine($"Argument is: {args} ");
            }
        }
    }
}
