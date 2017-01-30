using System;
using System.Drawing;
using WindowsAPI;

namespace WindowsHacks
{

    /// <summary>
    /// Writes directly to the Desktop.
    /// </summary>
    public static class WriteToDesktop
    {
        public static void Run()
        {
            int count = 0;
            Console.WriteLine("Type -1 to exit.");
            while (true)
            {
                Console.Write("Input Text: ");
                string input = Console.ReadLine();
                if (input.Equals("-1")) break;
                Draw.String(input, 200, 200 + count, Color.White, 20);
                count += 22;
            }
        }
    }
}
