using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Web;
namespace cms.libs
{
    public class ConvertTools
    {
        private HttpContext context = HttpContext.Current;
        private string WorkingDir = null;
        private string AppPath = null;
        public ConvertTools()
        {
            WorkingDir = Config.mediaPath + "resources/flvtools/";
            AppPath = WorkingDir + "convertflv.exe";
        }
        public void ToFlv(string MediaFilePath, string FlvFilePath, string ThumbFilePath)
        {
            ConvertFLV(MediaFilePath, FlvFilePath);
            GenThumbImage(FlvFilePath, ThumbFilePath);
        }

        private void ConvertFLV(string MediaFilePath, string FlvFilePath)
        {

            string sParam;
            sParam = "-i ";
            sParam += MediaFilePath + " ";
            sParam += "-ar 22050 -ab 32 -f flv -s 400x300 -aspect 4:3 -y ";
            sParam += FlvFilePath;

            ProcessStartInfo oInfo = new ProcessStartInfo();

            oInfo.WorkingDirectory = WorkingDir;
            oInfo.FileName = AppPath;
            oInfo.Arguments = sParam;

            oInfo.CreateNoWindow = true;
            oInfo.UseShellExecute = false;

            Process myProcess = new Process();
            myProcess.StartInfo = oInfo;

            myProcess.Start();

            while (myProcess.HasExited == false)
            {
                System.Threading.Thread.Sleep(1000);
            }

        }

        public void GenThumbImage(string FlvFilePath, string ThumbFilePath)
        {

            string sParam;
            sParam = "-i ";
            sParam += FlvFilePath + " ";
            sParam += "-f image2 -t 1 -s 400x300 ";
            //sParam += "-f image2 -t 1 -s 320x240 ";
            sParam += ThumbFilePath;

            ProcessStartInfo oInfo = new ProcessStartInfo();
            oInfo.WorkingDirectory = WorkingDir;
            oInfo.FileName = AppPath;
            oInfo.Arguments = sParam;


            oInfo.CreateNoWindow = true;
            oInfo.UseShellExecute = false;

            Process myProcess = new Process();
            myProcess.StartInfo = oInfo;

            myProcess.Start();

            while (myProcess.HasExited == false)
            {
                System.Threading.Thread.Sleep(1000);
            }

        }
        public void ConvertWMA_WAV_MP3(string MediaFilePath, string FlvFilePath)
        {

            string sParam;
            sParam = " -i ";
            sParam += MediaFilePath + " ";
            sParam += FlvFilePath;

            ProcessStartInfo oInfo = new ProcessStartInfo();

            oInfo.WorkingDirectory = WorkingDir;
            oInfo.FileName = AppPath;
            oInfo.Arguments = sParam;

            oInfo.CreateNoWindow = true;
            oInfo.UseShellExecute = false;

            Process myProcess = new Process();
            myProcess.StartInfo = oInfo;

            myProcess.Start();

            while (myProcess.HasExited == false)
            {
                System.Threading.Thread.Sleep(1000);
            }

        }
    }
}
