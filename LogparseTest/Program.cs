using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace LogparseTest
{
    class Program
    {
        static void Main(string[] args)
        {
            Run();
        }

        private static void Run()
        {
            var exePath = ConfigurationManager.AppSettings["e"];
            var a = ConfigurationManager.AppSettings["a"];
            var sql =
                "-i:IISW3C -o:CSV \"SELECT cs-uri-query,COUNT(*) INTO D:\\temps\\logs\\u_ex{0}.csv FROM '" + a + "\\u_ex{0}*.log' WHERE cs-uri-stem = '/__utm.gif' and cs-uri-query LIKE '%source=yt_yy_email&track=%' GROUP BY cs-uri-query ORDER BY cs-uri-query\"";

            var l = new List<DateTime>();
            var s = new DateTime(2013, 6, 3);
            var e = new DateTime(2013, 6, 5);
            while (s <= e)
            {
                l.Add(s);
                s = s.AddDays(1);
            }

            foreach (var d in l)
            {
                var t = String.Format(sql, d.ToString("yyMMdd"));
                Console.WriteLine(d.ToString("yyyy-MM-dd"));
                var r = CallProcessAndReturn(exePath, t);

                Console.WriteLine();
                // var resul = String.Format("日期{0},{1}", d.ToString("yyyy-MM-dd"), r);
                //Console.WriteLine(resul);
            }

            Console.ReadLine();

        }


        private static void CallProcess(string exePath, string fileArgs)
        {
            var startInfo = new ProcessStartInfo
            {
                Arguments = fileArgs,
                FileName = exePath,
                UseShellExecute = false,
                CreateNoWindow = false,
                RedirectStandardOutput = true
            };

            using (var exeProcess = Process.Start(startInfo))//Process.Start(System.IO.Path.Combine(pathImageMagick,appImageMagick),fileArgs))
            {
                exeProcess.WaitForExit();
                exeProcess.Close();
            }
        }

        private static string CallProcessAndReturn(string exePath, string fileArgs)
        {
            string result;
            using (var pro = new Process())
            {
                pro.StartInfo.UseShellExecute = false;
                pro.StartInfo.ErrorDialog = false;
                pro.StartInfo.RedirectStandardError = true;

                pro.StartInfo.FileName = exePath;
                pro.StartInfo.Arguments = fileArgs;

                pro.Start();
                using (var errorreader = pro.StandardError)
                {
                    pro.WaitForExit();

                    result = errorreader.ReadToEnd();
                }
            }

            return result;
        }
    }
}
