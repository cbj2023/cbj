//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace SUTAIMES.Common
//{
//    class Csflie
//    {
//    }
//}



using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using SUTAIMES;

namespace SUTAIMES.Common
{
    

    public class ClsFile
    {

        /// <summary>用来存放来自各class中的Log</summary>
        private System.Collections.Queue mQueue;
        public bool mDeLogQueueFlg;
        /// <summary>用来处理Log的Thread</summary>
        private Thread mLogThread;

        public Int32 mLogLength = 500;
        private Program rUsunMain;
        public string mLogPath;
        // <summary>用來存放目前最即時的Log. </summary>
        private System.Collections.ArrayList mLastNewLog = new System.Collections.ArrayList();



        public ClsFile(Program System)
        {
            rUsunMain = System;
            mLogPath = Application.StartupPath;

            mLogThread = new Thread(DeLogQueue);
            mDeLogQueueFlg = true;
            mLogThread.Start();

            mQueue = new System.Collections.Queue();

        }


        /// <summary>将Log写入档案</summary>
        /// <param name="LogType">LOG 型态</param>
        /// <param name="sttCode">Log代码</param>
        /// <param name="FunctionName">执行到的函式名称</param>
        /// <param name="LogMsg">Log内容</param>
        public Boolean  WriteLog(string FunctionName, string LogMsg)
        {
            Boolean stt = true;

            string strDt = "";

            try
            {
                string Log = "";
                strDt = DateTime.Now.ToString("HH:mm:ss"); // ComLib.GetNowDateTime(
                //        DateTimeType.ShortDateLongTimeMs);

                //  <2011-12-15 23:59:59:99 [GetData] - System Error : test msg>                      
                Log = strDt + LogMsg; ;//LogType.ToString() +
                //LogHeadChar +
                //"<" + strDt + " [" + FunctionName + "] - " + strSttContent + " : " +
                //LogMsg + ">";

                lock (mQueue)
                {
                    mQueue.Enqueue(Log);
                    //AddLogToCurrLogAry(Log);
                }
            }
            catch (Exception ex)
            {
                stt = false ;
                MessageBox.Show(ex.Message);
            }

            return stt;
        }




        /// <summary>将Log记录到Array中.</summary>
        private void AddLogToCurrLogAry(string Log)
        {
            try
            {
                if (mLastNewLog.Count >= mLogLength)
                {
                    mLastNewLog.RemoveAt(0);
                }

                mLastNewLog.Add(Log);

            }
            catch (Exception ex)
            {

                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }
        }

        /// <summary> 将Log从Queue中取出来</summary>
        public void DeLogQueue()
        {
            try
            {
                do
                {
                    if (mQueue != null)
                    {
                        lock (mQueue)
                        {
                            if (mQueue.Count > 0)
                            {
                                string strTmp = "";
                                strTmp = Convert.ToString(mQueue.Dequeue());
                                WriteLog2File(strTmp);
                            }
                        }
                    }
                    Thread.Sleep(100);
                    System.Windows.Forms.Application.DoEvents();
                } while (mDeLogQueueFlg);
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.StackTrace);
            }
        }

        /// <summary>将Log写入txt档案 </summary>
        private Boolean  WriteLog2File(string Log)
        {
            Boolean  stt = true;

            try
            {
                string LogFileName = "";
                string tLogTpe = "";
                if (Log.Trim().Length > 0)
                {
                }
                else
                {
                    tLogTpe = "E";
                }
                if (Log.Length > 0)
                {
                    tLogTpe = Log.Substring(Log.Length - 1, 1);
                }
                stt = GetLogFileName(ref LogFileName, tLogTpe);
                StreamWriter sw = new StreamWriter(LogFileName, true);
                Log = Log.Substring(0, Log.Length - 1);
                sw.WriteLine(Log);
                sw.Close();
            }
            catch (Exception ex)
            {
                stt = false;
                MessageBox.Show(ex.Message);
            }

            return stt;

        }
        /// <summary>取得Log档的名称,包含日期部份</summary>
        public Boolean  GetLogFileName(ref string FileName, string LT)
        {
            Boolean  stt = true;
            string strFileName = "";
            try
            {

                strFileName = @"C:\\STLog\\";
                strFileName += DateTime.Now.Year.ToString().Substring(2, 2) + DateTime.Now.Month.ToString("00") + "\\";
                switch (LT.ToUpper())
                {
                    case "A":
                        strFileName += @"原片仓储\\";
                        break;
                    case "B":

                        break;
                    case "C":
                        strFileName += @"切割上片\\";
                        break;
                    case "D":
                        strFileName += @"切割上片\\";
                        break;
                    case "E":
                        strFileName += @"切割对比\\";
                        break;
                    case "F":
                        strFileName += @"出理片\\";
                        break;
                    case "G":

                        break;
                    case "H":
                        strFileName += @"磨边仓入库\\";
                        break;
                    case "I":
                        strFileName += @"磨边仓储出库\\";
                        break;
                    case "J":
                        strFileName += @"进钢化\\";
                        break;
                    case "K":
                        strFileName += @"钢化后复核\\";
                        break;
                    case "L":

                        strFileName += @"中空仓入库\\";
                        break;
                    case "M":

                        strFileName += @"中空仓出库\\";
                        break;
                    case "N":

                        strFileName += @"ERP\\";
                        break;
                    case "Y":
                        strFileName += @"原片仓\\";
                        break;
                    case "W":
                        strFileName += @"原片仓裁切暂存\\";
                        break;
                    default:
                        break;
                }
                DirMk(strFileName);

 


                strFileName += DateTime.Now.ToString("yyyyMMdd");
            
            }
            catch (Exception ex)
            {
                stt = false;
                MessageBox.Show("GetLogFileName" + ex.Message);
              
            }

            FileName = strFileName;

            return stt;
        }
        public static int DirMk(string dirPathName)///delete file
        {
            int stt = 0;
            try
            {
                if (!Directory.Exists(dirPathName)) { Directory.CreateDirectory(dirPathName); }
            }
            catch (Exception ex)
            {
                stt = -1;
                System.Diagnostics.Debug.WriteLine(ex.Message + ex.StackTrace);
            }

            return stt;

        }



    }
}

