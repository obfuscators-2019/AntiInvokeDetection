using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Anti_Invoke_Detection
{
    public partial class ConsoleShort
    {
        public void WriteLine(string text, ConsoleColor color) { Console.ForegroundColor = color; Console.WriteLine(text); Console.ResetColor(); }
        public void Space() { Console.WriteLine(""); }
        public void Pause() { Console.ReadKey(); }
    }
}