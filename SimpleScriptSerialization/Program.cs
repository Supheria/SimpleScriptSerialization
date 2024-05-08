using Hoi4ScriptParser;
using Hoi4ScriptParser.Data;
using System.Text;

namespace SimpleScriptSerialization
{
    internal static class Program
    {
        [System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
        [return: System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.Bool)]
        static extern bool AllocConsole();

        [System.Runtime.InteropServices.DllImport("Kernel32")]
        public static extern void FreeConsole();
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            var analyzer = new Analyzer();
            var tokens = analyzer.Parse("test.md");
            var log = analyzer.ErroLog;
            AllocConsole();   //��������̨
            var message = new StringBuilder();
            foreach (var token in tokens)
                message.Append(token.ToString());
            Console.Write(message);
            Application.Run(new Form1());
        }
    }

    static class Shell
    {
        /// <summary>  
        /// �����Ϣ  
        /// </summary>  
        /// <param name="format"></param>  
        /// <param name="args"></param>  
        public static void WriteLine(string message, ConsoleColor GetConsoleColor)
        {
            Console.ForegroundColor = GetConsoleColor;
            Console.WriteLine(@"[{0}]{1}", DateTimeOffset.Now, message);
        }

        /// <summary>  
        /// �����Ϣ  
        /// </summary>  
        /// <param name="format"></param>  
        /// <param name="args"></param>  
        public static void WriteLine(string format, params object[] args)
        {
            WriteLine(string.Format(format, args));
        }

        /// <summary>  
        /// �����Ϣ  
        /// </summary>  
        /// <param name="output"></param>  
        public static void WriteLine(string output)
        {
            Console.ForegroundColor = GetConsoleColor(output);
            Console.WriteLine(@"[{0}]{1}", DateTimeOffset.Now, output);
        }

        /// <summary>  
        /// ��������ı�ѡ�����̨������ɫ  
        /// </summary>  
        /// <param name="output"></param>  
        /// <returns></returns>  
        private static ConsoleColor GetConsoleColor(string output)
        {
            if (output.StartsWith("����")) return ConsoleColor.Yellow;
            if (output.StartsWith("����")) return ConsoleColor.Red;
            if (output.StartsWith("ע��")) return ConsoleColor.Green;
            return ConsoleColor.Gray;
        }
    }
}