﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace LogparseTest
{
    internal class Entry
    {
        public int Id { get; set; }

        public int Amount { get; set; }

        public string Title { get; set; }

        public int TemplateId { get; set; }
    }

    internal class EntryCollection : List<Entry>
    {
        public DateTime DateTime { get; set; }
    }

    class Program
    {
        private static void Init()
        {
            // return Init(new EntryCollection());
        }

        private static List<Entry> GetTitle()
        {
            var e = new List<Entry>
                {
                    new Entry
                        {
                            Id = 10001,
                            Title = "银泰网-成功下单通知"
                        },
                    new Entry
                        {
                            Id = 10002,
                            Title = "欢迎加入银泰网"
                        },
                    new Entry
                        {
                            Id = 10003,
                            Title = "邮箱验证"
                        },
                    new Entry
                        {
                            Id = 10004,
                            Title = "订单发货成功"
                        },
                    new Entry
                        {
                            Id = 10005,
                            Title = "退换货受理通知"
                        },
                    new Entry
                        {
                            Id = 10006,
                            Title = "限时尊抢活动订阅邮件"
                        },
                    new Entry
                        {
                            Id = 20001,
                            Title = "银泰网-找回密码"
                        },
                    new Entry
                        {
                            Id = 20002,
                            Title = "银泰网-未支付提醒"
                        },
                    new Entry
                        {
                            Id = 20003,
                            Title = "银泰网-订单支付成功"
                        },
                    new Entry
                        {
                            Id = 20004,
                            Title = "银泰网-成功下单通知"
                        },
                    new Entry
                        {
                            Id = 20005,
                            Title = "银泰网-发货通知"
                        },
                    new Entry
                        {
                            Id = 20006,
                            Title = "您已成功加入银泰网，重置密码"
                        },
                    new Entry
                        {
                            Id = 20007,
                            Title = "银泰网-邮箱验证"
                        },
                    new Entry
                        {
                            Id = 20008,
                            Title = "银泰网-退换货受理通知（RMA）"
                        },
                    new Entry
                        {
                            Id = 20009,
                            Title = "银泰网-退款通知(RMA)"
                        },
                    new Entry
                        {
                            Id = 20010,
                            Title = "您的好友邀请您加入银泰网"
                        },
                    new Entry
                        {
                            Id = 20011,
                            Title = "银泰网-订单取消通知"
                        },
                    new Entry
                        {
                            Id = 20012,
                            Title = "欢迎您加入银泰网（新）"
                        },
                    new Entry
                        {
                            Id = 20013,
                            Title = "银泰网-成功订购通知"
                        },
                    new Entry
                        {
                            Id = 20014,
                            Title = "会员升级邮件通知（新）"
                        },
                    new Entry
                        {
                            Id = 20015,
                            Title = "银泰网-COD取消通知"
                        },
                    new Entry
                        {
                            Id = 20016,
                            Title = "银泰网-预定到货通知"
                        },
                    new Entry
                        {
                            Id = 20017,
                            Title = "满额返券邮件模板"
                        },
                    new Entry
                        {
                            Id = 20018,
                            Title = "会员升级邮件通知(金卡)"
                        },
                    new Entry
                        {
                            Id = 20019,
                            Title = "银泰网限时尊抢订阅验证"
                        },
                    new Entry
                        {
                            Id = 20020,
                            Title = "银泰网限时尊抢退订验证"
                        },
                    new Entry
                        {
                            Id = 20021,
                            Title = "会员升级邮件通知（白金卡）"
                        },
                    new Entry
                        {
                            Id = 20022,
                            Title = "银泰网-修改邮箱"
                        }
                };


            //2qi


            return e;
        }

        static void Main(string[] args)
        {
            Init();

            Run();
        }

        static readonly string exePath = ConfigurationManager.AppSettings["e"];
        static readonly string logPath = ConfigurationManager.AppSettings["a"];
        static readonly string outPath = ConfigurationManager.AppSettings["o"];

        static readonly string sql = "-i:IISW3C -o:CSV \"SELECT cs-uri-query,COUNT(*) INTO " + outPath + "\\u_ex{0}.csv FROM '" + logPath + "\\u_ex{0}*.log' WHERE cs-uri-stem = '/__utm.gif' and cs-uri-query LIKE '%source=yt_yy_email&track=%' GROUP BY cs-uri-query ORDER BY cs-uri-query\"";

        private static void Run()
        {
            var l = new List<DateTime>();
            var s = new DateTime(2013, 10, 1);
            var e = new DateTime(2013, 11, 30);
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

            var result = Handler(outPath, l);
            Save(result);

            Console.ReadLine();
        }


        private static void Save(List<EntryCollection> datas)
        {
            var title1 = GetTitle();
            var fullPath = outPath + String.Format("\\tongji_{0}.csv", DateTime.Now.ToString("yyyyMMddHH"));
            var fi = new FileInfo(fullPath);

            if (fi.Directory != null && !fi.Directory.Exists)
            {
                fi.Directory.Create();
            }

            var fs = new FileStream(fullPath, System.IO.FileMode.Create, System.IO.FileAccess.Write);
            StreamWriter sw = new StreamWriter(fs, System.Text.Encoding.UTF8);

            StringBuilder sbTitle = new StringBuilder();
            var sbData = new StringBuilder();
            // var sbData2 = new StringBuilder();
            //写出列名称
            sbTitle.Append("日期,");
            foreach (var item in title1)
            {
                sbTitle.AppendFormat("{0},", item.Title);
            }

            //行数据
            foreach (var data in datas)
            {
                sbData.AppendFormat("{0},", data.DateTime.ToString("yy年M月d日"));
                //sbData2.AppendFormat("{0},", data.DateTime.ToString("yy年M月d日"));
                foreach (var t in title1)
                {
                    var item = data.FirstOrDefault(v => v.Id == t.Id);
                    var val = item == null ? "0" : item.Amount.ToString();
                    sbData.AppendFormat("{0},", val);
                    // sbData2.AppendFormat("{0},{1}", t.Title, val);
                }

                sbData.AppendLine();


            }

            sw.WriteLine(sbTitle.ToString());
            sw.WriteLine(sbData);

            //2表

            sw.WriteLine();


            // sw.WriteLine("日期,邮件类型,发送量");

            // sw.WriteLine(sbData2.ToString());
            sw.Close();
            fs.Close();

        }

        private static string FileName(string format, DateTime d)
        {
            return String.Format(format, d.ToString("yyMMdd"));
        }

        private static List<EntryCollection> Handler(string path, IEnumerable<DateTime> fileName)
        {
            var es = new List<EntryCollection>();
            var list = new List<string>();
            foreach (var n in fileName)
            {
                string s;

                //判断文件是否存在
                var p = Path.Combine(path, FileName("u_ex{0}.csv", n));
                var f = new FileInfo(p);
                if (!f.Exists)
                {
                    continue;
                }

                using (var sr = f.OpenText())
                {
                    //放弃第一行
                    if (sr.ReadLine() != null)
                    {

                        while ((s = sr.ReadLine()) != null)
                        {
                            list.Add(s);
                        }
                    }
                }

                var entry = Arrange(n, list);
                es.Add(entry);
                list.Clear();
            }

            return es;
        }

        private static EntryCollection Arrange(DateTime dt, IEnumerable<string> lines)
        {
            var collec = new EntryCollection { DateTime = dt };

            var dic = new Dictionary<int, Entry>();

            foreach (var line in lines)
            {
                if (line.StartsWith("source=yt_yy_email&track=", System.StringComparison.OrdinalIgnoreCase))
                {
                    var t = line.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                    var p = t[0].ToLower().Split(new[] { '&' }, StringSplitOptions.RemoveEmptyEntries);

                    var ppos = p.FirstOrDefault(v => v.StartsWith("track"));

                    if (!String.IsNullOrEmpty(ppos))
                    {
                        var id = ppos.Split(new[] { '=' })[1];
                        int tmpInt;
                        if (!Int32.TryParse(id.Trim(), out tmpInt))
                        {
                            continue;
                        }

                        var i = tmpInt;
                        var entry = new Entry { Id = i, Amount = Int32.Parse(t[1]) };

                        if (dic.ContainsKey(i))
                        {
                            dic[i].Amount += entry.Amount;
                        }
                        else
                        {
                            dic.Add(i, entry);
                        }

                    }
                }
            }

            collec.AddRange(dic.Values);

            return collec;
        }

        private static void CallProcess(string ePath, string fileArgs)
        {
            var startInfo = new ProcessStartInfo
            {
                Arguments = fileArgs,
                FileName = ePath,
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

        private static string CallProcessAndReturn(string ePath, string fileArgs)
        {
            string result;
            using (var pro = new Process())
            {
                pro.StartInfo.UseShellExecute = false;
                pro.StartInfo.ErrorDialog = false;
                pro.StartInfo.RedirectStandardError = true;

                pro.StartInfo.FileName = ePath;
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
