using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace diplom
{
    internal class Logger
    {

        private static string logFilePath = "encryption_log.txt";

        public static void Log(string message, RichTextBox logTextBox = null)
        {
            string logMessage = $"[{DateTime.Now}] {message}";

            // Запись в файл
            File.AppendAllText(logFilePath, logMessage + Environment.NewLine);

            // Вывод в RichTextBox (если передан)
            if (logTextBox != null)
            {
                logTextBox.Invoke((MethodInvoker)(() =>
                {
                    logTextBox.AppendText(logMessage + Environment.NewLine);
                }));
            }
        }
    }
}



