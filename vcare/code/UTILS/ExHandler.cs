using System;
using System.Text;
using System.IO;
namespace UTILS
{
    public class ExHandler
    {
        /// <summary>
        /// Handle Exception and write to log file
        /// </summary>
        /// <param name="ex">Exception object</param>
        public static void Handle(Exception ex)
        {
            Handle(ex, String.Empty, String.Empty);
        }
        /// <summary>
        /// Handle Exception and write to log file
        /// </summary>
        /// <param name="ex">Exception object</param>
        /// <param name="className">Name of class, where the error occurred.</param>
        public static void Handle(Exception ex, string className)
        {
            Handle(ex, className, String.Empty);
        }
        /// <summary>
        /// Handle Exception and write to log file
        /// </summary>
        /// <param name="ex">Exception object</param>
        /// <param name="className">Name of class, where the error occurred.</param>
        /// <param name="functionName">Name of function, where the error occurred.</param>
        public static void Handle(Exception ex, string className, string functionName)
        {
            //MessageBox.Show(ex.Message + ",class=" + className + ",func = " + functionName);
            var sb = new StringBuilder();
            sb.Append("Time: ");
            sb.Append(DateTime.Now);
            sb.Append("| class: ");
            sb.Append(className);
            sb.Append("| functionName: ");
            sb.Append(functionName);
            sb.Append("| Error: ");
            sb.Append(ex.Message);
            sb.AppendLine();
            sb.Append("---------------------");
            AppendToTextFile(sb.ToString(), Config.LogErrorFolder, DateTime.Now.ToString("yyyyMMdd") + "_LogErrors.txt");
            //throw ex;
        }

        /// <summary>
        /// hàm nối một đoạn text vào cuối văn bản
        /// </summary>
        /// <param name="strContent"></param>
        /// <param name="uploadPath"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        public static bool AppendToTextFile(string strContent, string uploadPath, string fileName)
        {
            var result = true;

            try
            {
                var byteArray = Encoding.UTF8.GetBytes(strContent);
                var stream = new MemoryStream(byteArray);
                var filePath = Path.Combine(uploadPath, fileName);
                var dir = Path.GetDirectoryName(filePath);

                if (dir != null && !Directory.Exists(dir))
                    Directory.CreateDirectory(dir);

                if (!File.Exists(filePath))
                {
                    var fs = File.Create(filePath);
                    fs.Close();
                }

                var sw = File.AppendText(filePath);
                sw.WriteLine(strContent);
                // Writing a string directly to the file
                sw.Close();
            }
            catch
            {
                result = false;
            }

            return result;
        }
    }
}
