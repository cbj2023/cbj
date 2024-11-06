using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;



namespace SUTAIMES
{
    public class Program
    {
        public Common.ClsFile mFile;
        public static Program mMain;

        public Common.ClsDbAcc mClsDBac;
        public Common.ClsDbAcc mClsDBUPdate ;
        public Common.ClsDbAcc mClsDBUPdateZK;
        public Common.ClsDbAcc mClsDBYu;
        public FrmMain mFrmMain;

        public Http.HttpServer httpServer;

        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            bool isRuned;

            Mutex mutex = new Mutex(true, "WCSData242", out isRuned);
            if (isRuned)
            {
                mMain = new Program();
                mMain.mFile = new Common.ClsFile(mMain);

                clsMyPublic.gIniSysParam();

                mMain.mClsDBac = new Common.ClsDbAcc(mMain);
                mMain.mClsDBUPdate = new Common.ClsDbAcc(mMain);
                mMain.mClsDBUPdateZK = new Common.ClsDbAcc(mMain);

                mMain.mClsDBYu = new Common.ClsDbAcc(mMain);
                mMain.mClsDBYu.mIndex = 2;
                if (clsMyPublic.mYuanDB == "1")
                {
                    mMain.mClsDBYu.DBopen();
                }

                mMain.mClsDBac.DBopen();

                if (clsMyPublic.mHttpIF == "1")
                {
                    mMain.httpServer = new Http.MyHttpServer(8080, mMain);
                    Thread thread = new Thread(new ThreadStart(mMain.httpServer.listen));
                    thread.Start();
                }
                
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                mMain.mFrmMain = new FrmMain(mMain);
                mMain.mFrmMain.Show();
                Application.Run();
            }
        }



        private static string GetGuid()
         {
             System.Guid guid = new Guid();
             guid = Guid.NewGuid();
             return guid.ToString();
         }
         /// <summary>  
         /// 根据GUID获取16位的唯一字符串  
         /// </summary>  
         /// <param name=\"guid\"></param>  
         /// <returns></returns>  
         public static string GuidTo16String()
         {
             long i = 1;
             foreach (byte b in Guid.NewGuid().ToByteArray())
                 i *= ((int)b + 1);
             return string.Format("{0:x}", i - DateTime.Now.Ticks);
         }
         /// <summary>  
         /// 根据GUID获取19位的唯一数字序列  
         /// </summary>  
         /// <returns></returns>  
         public static long GuidToLongID()
         {
             byte[] buffer = Guid.NewGuid().ToByteArray();
             return BitConverter.ToInt64(buffer, 0);
         }  

        //
    }
}
