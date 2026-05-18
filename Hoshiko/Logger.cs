using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hoshiko
{
    public class Logger
    {
        private static readonly string LogFilePath =
        Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "app.log");

        public void Info(string message)
        {
            try
            {
                var directory = Path.GetDirectoryName(LogFilePath);
                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);

                var line = $"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {message}";

                File.AppendAllLines(LogFilePath, new[] { line });
            }
            catch
            {}
        }
    }
}
