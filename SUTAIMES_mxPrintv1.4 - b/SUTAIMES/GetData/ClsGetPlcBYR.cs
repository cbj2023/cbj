
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Data;
using System.Collections;
using System.Reflection;//PropertyInfo引用
using SUTAIMES;
using SUTAIMES.Common;
using System.Xml;
using System.IO;
using System.Windows.Forms;

namespace SUTAIMES.GetData
{
    /// <summary>
    /// 模板尺寸信息带小数点
    /// </summary>
    class ClsGetPlcBYR
    {
        Program tSystem;
        Thread mThreadGetPlcData;
        public bool mPlcStartGet = false;

        public S7Client[] PlcClient;


        public int mMBOutCmdFlow = 0;

  
        public ClsGetPlcBYR(Program tSys)
        {
            tSystem = tSys;
            mMBOutCmdFlow = SettingsMC.Default.MBOutCmdFlow;
            for (int i = 0; i < tEnportCmdtime.Length; i++)
            {
                tEnportCmdtime[i] = DateTime.Now.AddSeconds(-5);
            }

        }
        public string OpenThread(int ConnCount)
        {
            int PlcResult = 9999; string PlcResultString = "";
            PlcClient = new S7Client[ConnCount];/////连接PLC
            for (int i = 0; i < PlcClient.Length; i++)
            {
                try
                {
                    PlcResult = 99;
                    PlcClient[i] = new S7Client();
                    PlcResult = PlcClient[i].ConnectTo(clsMyPublic.mPLCIPBY , 0, 1);// IP  机架号  插槽号  ///192.168.1.10   192.168.11.40  192.168.2.9
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message + " " + ex.StackTrace);
                }
                if (PlcResult == 0)
                {
                    PlcResultString = PlcResultString + "0";

                }
                else
                {
                    PlcResultString = PlcResultString + "1";
                }
            }

            mThreadGetPlcData = new Thread(GetPlcData);
            mThreadGetPlcData.Start();
            mPlcStartGet = true;
            return PlcResultString;
        }
        int[] BYPutUPloadFlow = new int[3] { 0, 0, 0 };//上片
        int[] BYUpdateFlow = new int[4] { 0, 0, 0, 0 };//切割抓片
        int[] BYCarFinish = new int[3] { 0, 0, 0 };//索车完成
        int[] BYCarFlow = new int[3] { 0, 0, 0 };//索车运行状态
        int[] BYDownFlow = new int[4] { 0, 0, 0,0 };//卸片站位

     
        int tFurFlow = 0;
        int mEnportTag = 0;
        DateTime[] tEnportCmdtime = new DateTime[3];
        private void GetPlcData()//5
        {
            int[] tPlcFlow = new int[23] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            int[] tFruNOFlow = new int[5] { 0, 0, 0, 0, 0 };
            do
            {
                if (mPlcStartGet == true)
                {

                    byte[] tbufferValue = new byte[332];
                    int[] tValue = new int[83];
                    int readResult = PlcClient[0].DBRead(700, 0, tbufferValue.Length, tbufferValue);
                    if (readResult != 0)
                    {
                        PlcClient[0].Disconnect();
                        Thread.Sleep(100);
                        PlcClient[0].ConnectTo(clsMyPublic.mPLCIPA, 0, 1);//// IP  机架号  插槽号 
                    }
                    else
                    {
                        

                        byte[] tbufferValueData = new byte[880];
                        int[] tValueData = new int[220];
                        int readResultData = PlcClient[0].DBRead(600, 0, tbufferValueData.Length, tbufferValueData);
                       
                        for (int i = 0; i < tValueData.Length; i++)
                        {
                            tValueData[i] = S7.GetDIntAt(tbufferValueData, i * 4);//转换成数据
                        }

                        int Start = 0; int Pos_id = 0; List<PositionObj> tListPos = new List<PositionObj>();
                        while (Start < tValueData.Length)
                        {
                            PositionObj tPosObj = new PositionObj();
                            Pos_id = Pos_id + 1;
                            tPosObj.Pos = Pos_id;
                            tPosObj.PlcStatus = tValueData[Start];
                            tPosObj.Matid = tValueData[Start + 1];
                            tPosObj.PlcFlow = tValueData[Start + 2];
                            tPosObj.Count = tValueData[Start + 3];
                            tPosObj.Rev = tValueData[Start + 4]; ;//备用
                            tListPos.Add(tPosObj);
                            Start = Start + 5;
                        }
                        DataTable tDtPos = ToDataTable<PositionObj>(tListPos);

                        for (int i = 0; i < tValue.Length; i++)
                        {
                            tValue[i] = S7.GetDIntAt(tbufferValue, i * 4);//转换成数据
                        }


                        if (tValue[0] > 0 & tValue[1] > 0 & BYPutUPloadFlow[0] != tValue[0] & tValue[0]>0
                             //& 1 == 2
                            )//上片 ok  1号上片
                        {
                         
                            BYPutUPloadFlow[0] = tValue[0];
                            ClsObj clsObj1 = new ClsObj();
                            ClsPutLoad clsPutLoad = new ClsPutLoad();
                            
                            clsPutLoad.Enport = 1;
                            clsPutLoad.Flow = tValue[0];
                            clsPutLoad.Status = tValue[1];
                            clsPutLoad.Obj = tDtPos;//获取当前库位状态
                            clsObj1.obj = clsPutLoad;
                            clsObj1.Index = 10010;//上片
                            Dispose(clsObj1);
                            //Thread thread1 = new Thread(new ParameterizedThreadStart(Dispose));
                            //thread1.Start(clsObj1);
                        }

                        if (tValue[74] > 0 & tValue[75] > 0 & BYPutUPloadFlow[1] != tValue[74] & tValue[74] > 0
                            //& 1 == 2
                            )//上片 ok  2号上片
                        {

                            BYPutUPloadFlow[1] = tValue[74];
                            ClsObj clsObj1 = new ClsObj();
                            ClsPutLoad clsPutLoad = new ClsPutLoad();

                            clsPutLoad.Enport = 2;
                            clsPutLoad.Flow = tValue[074];
                            clsPutLoad.Status = tValue[75];
                            clsPutLoad.Obj = tDtPos;//获取当前库位状态
                            clsObj1.obj = clsPutLoad;
                            clsObj1.Index = 10010;//上片
                            Dispose(clsObj1);
                            //Thread thread1 = new Thread(new ParameterizedThreadStart(Dispose));
                            //thread1.Start(clsObj1);
                        }

                        if (tValue[42] > 0 & tValue[43]>0 & BYUpdateFlow[0]!=tValue[43]
                            //& 1 == 2
                            )//一号1位完成一片  每片取片
                        {
                            BYUpdateFlow[0] = tValue[43];
                            ClsObj clsObj2 = new ClsObj();
                            CutStartObj clsCutStartObj = new CutStartObj();
                            clsCutStartObj.Pos = 131;
                            clsCutStartObj.Status = tValue[42];
                            clsCutStartObj.SingleFlow = tValue[43];
                            clsCutStartObj.Flow = tValue[44];
                            clsCutStartObj.Matid = tValue[45];
                            clsCutStartObj.LCount = tValue[46];
                            clsCutStartObj.Count = tValue[47];
                            clsCutStartObj.Num = 1;
                            clsObj2.Index = 10030;
                            clsObj2.obj = clsCutStartObj;
                            Dispose(clsObj2);
                            //Thread thread1 = new Thread(new ParameterizedThreadStart(Dispose));
                            //thread1.Start(clsObj2);
                        }
                        if (tValue[50] > 0 & tValue[51] > 0 & BYUpdateFlow[1] != tValue[51]
                            //& 1 == 2
                            )//一号2位完成一片  每片取片
                        {
                            BYUpdateFlow[1] = tValue[51];
                            ClsObj clsObj3 = new ClsObj();
                            CutStartObj clsCutStartObj = new CutStartObj();
                            clsCutStartObj.Pos = 132;
                            clsCutStartObj.Status = tValue[50];
                            clsCutStartObj.SingleFlow = tValue[51];
                            clsCutStartObj.Flow = tValue[52];
                            clsCutStartObj.Matid = tValue[53];
                            clsCutStartObj.LCount = tValue[54];
                            clsCutStartObj.Count = tValue[55];
                            clsCutStartObj.Num = 2;
                            clsObj3.Index = 10030;
                            clsObj3.obj = clsCutStartObj;
                            Dispose(clsObj3);
                            //Thread thread1 = new Thread(new ParameterizedThreadStart(Dispose));
                            //thread1.Start(clsObj3);
                        }
                        if (tValue[58] > 0 & tValue[59] > 0 & BYUpdateFlow[2] != tValue[59]
                            //& 1 == 2
                            )//二号1位完成一片  每片取片
                        {
                            BYUpdateFlow[2] = tValue[59];
                            ClsObj clsObj4 = new ClsObj();
                            CutStartObj clsCutStartObj = new CutStartObj();
                            clsCutStartObj.Pos = 231;
                            clsCutStartObj.Status = tValue[58];
                            clsCutStartObj.SingleFlow = tValue[59];
                            clsCutStartObj.Flow = tValue[60];
                            clsCutStartObj.Matid = tValue[61];
                            clsCutStartObj.LCount = tValue[62];
                            clsCutStartObj.Count = tValue[63];
                            clsCutStartObj.Num = 3;
                            clsObj4.Index = 10030;
                            clsObj4.obj = clsCutStartObj;
                            Dispose(clsObj4);
                            //Thread thread1 = new Thread(new ParameterizedThreadStart(Dispose));
                            //thread1.Start(clsObj4);
                        }
                        if (tValue[58] > 0 & tValue[59] > 0 & BYUpdateFlow[3] != tValue[59]
                            //& 1 == 2
                            )//二号2位完成一片 每片取片
                        {
                            BYUpdateFlow[3] = tValue[59];
                            ClsObj clsObj5 = new ClsObj();
                            CutStartObj clsCutStartObj = new CutStartObj();
                            clsCutStartObj.Pos = 232;
                            clsCutStartObj.Status = tValue[66];
                            clsCutStartObj.SingleFlow = tValue[67];
                            clsCutStartObj.Flow = tValue[68];
                            clsCutStartObj.Matid = tValue[69];
                            clsCutStartObj.LCount = tValue[70];
                            clsCutStartObj.Count = tValue[71];
                            clsCutStartObj.Num = 4;
                            clsObj5.Index = 10030;
                            clsObj5.obj = clsCutStartObj;
                            Dispose(clsObj5);
                            //Thread thread1 = new Thread(new ParameterizedThreadStart(Dispose));
                            //thread1.Start(clsObj5);
                        }
                        if (tValue[24] > 0 & tValue[25] > 0 & BYCarFinish[0] != tValue[25]
                            //& 1 == 2
                            )//索车完成
                        {
                            BYCarFinish[0] = tValue[25];
                            ClsObj clsObj6 = new ClsObj();
                            CarFisishObj carFisishObj = new CarFisishObj();
                            carFisishObj.CarNO = 1;
                            carFisishObj.Status = tValue[24];
                            carFisishObj.Cmd = tValue[25];
                            carFisishObj.Flow = tValue[26];
                            carFisishObj.Matid = tValue[27];
                            carFisishObj.ToPos = tValue[28];
                            carFisishObj.FromPos = tValue[29];
                            clsObj6.Index = 10021;
                            clsObj6.obj = carFisishObj;
                            Dispose(clsObj6);
                            //Thread thread1 = new Thread(new ParameterizedThreadStart(Dispose));
                            //thread1.Start(clsObj6);
                        }
                        if(tValue[10]>0 & BYCarFlow[0]==0
                            //& 1 == 2 
                            )//索车运行状态
                        {
                            BYCarFlow[0] = 1;
                            ClsObj clsObj7 = new ClsObj();
                            CarObj carObj = new CarObj();
                            carObj.CarNO = 1;
                            carObj.RunStatus = tValue[10]; //牵引车状态
                            carObj.CarPos = tValue[11]; //牵引车位置
                            carObj.toPos = tValue[12]; //目的存储位置
                            carObj.CmdNO = tValue[13]; //指令序号
                            carObj.height = tValue[14]; //同步
                            carObj.width =tValue[15]; //同步
                            carObj.Ply = tValue[16];//厚度
                            carObj.Matid = tValue[17]; //玻璃品种编号
                            carObj.Count = tValue[18]; //数量
                            carObj.StatusIF = tValue[19];
                            carObj.Plcflow = tValue[20];

                            carObj.obj = tDtPos;
                            clsObj7.Index = 10020;
                            clsObj7.obj = carObj;
                            Dispose(clsObj7);
                            //Thread thread1 = new Thread(new ParameterizedThreadStart(Dispose));
                            //thread1.Start(clsObj7);
                        }

                        /////切割卸片
                        if (tValue[34] > 0 & tValue[35] > 0 & BYDownFlow[0] != tValue[35]
                            //& 1 == 2
                            )//卸片一 1  1
                        {
                            BYDownFlow[0] = tValue[35];
                            ClsObj clsObj8 = new ClsObj();
                            DownPosObj downPosObj = new DownPosObj();
                            downPosObj.Pos = 131;
                            downPosObj.Status = tValue[34];
                            downPosObj.Matid = tValue[35];
                            downPosObj.Num = 1;
                            downPosObj.Obj = tDtPos;//获取当前库位状态
                            clsObj8.Index = 10040;
                            clsObj8.obj = downPosObj;
                            Dispose(clsObj8);
                        }
                        if (tValue[36] > 0 & tValue[37] > 0 & BYDownFlow[1] != tValue[37]
                            //& 1 == 2
                            )//卸片一 2  2
                        {
                            BYDownFlow[1] = tValue[37];
                            ClsObj clsObj9 = new ClsObj();
                            DownPosObj downPosObj = new DownPosObj();
                            downPosObj.Pos = 132;
                            downPosObj.Status = tValue[36];
                            downPosObj.Matid = tValue[37];
                            downPosObj.Num = 2;
                            downPosObj.Obj = tDtPos;//获取当前库位状态
                            clsObj9.Index = 10040;
                            clsObj9.obj = downPosObj;
                            Dispose(clsObj9);
                        }
                       

                        if (tValue[38] > 0 & tValue[39] > 0 & BYDownFlow[2] != tValue[39]
                            //& 1 == 2
                            )//卸片二 1  1
                        {
                            BYDownFlow[2] = tValue[39];
                            ClsObj clsObj10 = new ClsObj();
                            DownPosObj downPosObj = new DownPosObj();
                            downPosObj.Pos = 231;
                            downPosObj.Status = tValue[38];
                            downPosObj.Matid = tValue[39];
                            downPosObj.Num = 3;
                            downPosObj.Obj = tDtPos;//获取当前库位状态
                            clsObj10.Index = 10040;
                            clsObj10.obj = downPosObj;
                            Dispose(clsObj10);
                        }
                        if (tValue[40] > 0 & tValue[41] > 0 & BYDownFlow[3] != tValue[41]
                            //& 1 == 2
                           )//卸片二 2  2
                        {
                            BYDownFlow[3] = tValue[41];
                            ClsObj clsObj10 = new ClsObj();
                            DownPosObj downPosObj = new DownPosObj();
                            downPosObj.Pos = 232;
                            downPosObj.Status = tValue[40];
                            downPosObj.Matid = tValue[41];
                            downPosObj.Num = 4;
                            downPosObj.Obj = tDtPos;//获取当前库位状态
                            clsObj10.Index = 10040;
                            clsObj10.obj = downPosObj;
                            Dispose(clsObj10);
                        }
                        //else if (tValue[42] > 0)
                        //{
                        //    MCInFinishFlow = tValue[43];
                        //    ClsObj clsObj14 = new ClsObj();
                        //    ClsMCFinish clsMCFinishcls14 = new ClsMCFinish();
                        //    clsObj14.Index = 14;//
                        //    clsMCFinishcls14.Flow = tValue[43];
                        //    clsMCFinishcls14.Status = tValue[42];
                        //    clsObj14.obj = clsMCFinishcls14;
                        //    Thread thread1 = new Thread(new ParameterizedThreadStart(Dispose));
                        //    thread1.Start(clsObj14);
                        //}




                        //List<ClsMachineData> tListData = new List<ClsMachineData>(); ClsMachineData tclsMachineData; int tPlcTag = 0;
                        //for (int j = 1; j <= 5; j++)
                        //{
                        //    if (tFruNOFlow[j - 1] != tValue[64 + j])//65
                        //    {
                        //        tPlcTag = 1;
                        //        tFruNOFlow[j - 1] = tValue[64 + j];
                        //        tclsMachineData = new ClsMachineData(); tclsMachineData.ID = j; tclsMachineData.Plc_Flow = tValue[64 + j]; tListData.Add(tclsMachineData);
                        //    }


                        //}
                        //for (int j = 21; j <= 36; j++)
                        //{
                        //    if (tPlcFlow[j - 15] != tValue[34 + j])
                        //    {
                        //        tPlcTag = 1;
                        //        tPlcFlow[j - 15] = tValue[34 + j];
                        //        tclsMachineData = new ClsMachineData(); tclsMachineData.ID = j; tclsMachineData.Plc_Flow = tValue[34 + j]; tListData.Add(tclsMachineData);
                        //    }
                        //}

                  
                    }





                   


                }
                EnTag1 = 0; EnTag2 = 0; EnTag3 = 0;
                for (int i = 0; i < CHMBTag.Length; i++)
                {
                    if (DateTime.Now.Subtract(CHMBtagtime[i]).TotalMilliseconds > 1200)
                    {
                        CHMBTag[i] = 0;
                    }
                }

                Thread.Sleep(777);
                System.Windows.Forms.Application.DoEvents();
            } while (true);

        }

        public void Dispose(object _obj)
        {
          
            ClsObj tobj = new ClsObj();
            tobj = (ClsObj)_obj;
            switch (tobj.Index)//10000以上原片仓储使用
            {
                case 10010://入库口1
                    ClsPutLoad clsPutLoad = new ClsPutLoad();
                    clsPutLoad= (ClsPutLoad)tobj.obj;
                    EnportStart(1, clsPutLoad);
                    BYPutUPloadFlow[clsPutLoad.Enport -1] = 0;
                    break;
                case 10020://索车空闲
                    CarObj carObj = new CarObj();
                    carObj = (CarObj)tobj.obj;
                    GetCarCmd(2, carObj);
                    BYCarFlow[carObj.CarNO - 1] = 0;
                    break;
                case 10021://索车指令完成
                    CarFisishObj carFisishObj = new CarFisishObj();
                    carFisishObj = (CarFisishObj)tobj.obj;
                    CarFinish(3,carFisishObj);
                    BYCarFinish[carFisishObj.CarNO - 1] = 0;
                    break;
                case 10030://磨边机上片完成
                    CutStartObj clsCutStartObj = new CutStartObj();
                    clsCutStartObj = (CutStartObj)tobj.obj;
                    SingleFinish(4, clsCutStartObj);
                    BYUpdateFlow[clsCutStartObj.Num - 1] = 0;
                    break;
                case 10040://
                    DownPosObj downPosObj=new DownPosObj();
                    downPosObj = (DownPosObj)tobj.obj;

                    DownFinish(5, downPosObj);
                    BYDownFlow[downPosObj.Num - 1] = 0;
                    break ;
                 default ://其它

                    break;
         
            }
        }

        private Boolean  EnportStart(int _Plcline,  object _Obj = null)
        {
            try
            {
                ClsDbAcc tClsDB = new ClsDbAcc(tSystem);
                string tRetData = "", tRetStr = ""; int tRows = 0;
                DataSet dataSet = new DataSet();
                ClsPutLoad clsPutLoad = new ClsPutLoad();
                clsPutLoad = (ClsPutLoad)_Obj;
                DataTable tDt = new DataTable();
                string tDataStr = "";
                tDt =(DataTable ) clsPutLoad.Obj;
                dataSet = tClsDB.ExecuteBYuan("BYuan_01InStart", clsPutLoad.Status , clsPutLoad.Flow , tDataStr, tDt, clsPutLoad.Enport.ToString() , ref tRetData, ref tRetStr, ref tRows);

                if (dataSet == null) { mEnportTag = 0; return false; }
                if (dataSet.Tables.Count > 0)
                {
                    if (dataSet.Tables[dataSet.Tables.Count - 1].Rows.Count == 0) { mEnportTag = 0; return false; }
                    if (dataSet.Tables[dataSet.Tables.Count - 1].Rows[0][0].ToString().ToUpper() == "Y" & dataSet.Tables[dataSet.Tables.Count - 1].Rows[0][2].ToString().ToUpper() == "入库口" + clsPutLoad.Enport.ToString())
                    {
                        string[] tValues = dataSet.Tables[dataSet.Tables.Count - 1].Rows[0][1].ToString().Split(',');
                        if (tValues.Length >= 9)
                        {
                            int readResult = 0;
                            byte[] tbufferValue = new byte[36];
                            int[] tValue = new int[9];

                            tValue[0] = int.Parse(tValues[0]);//流水号
                            S7.SetDIntAt(tbufferValue, 0 * 4, tValue[0]);
                            tValue[1] = int.Parse(tValues[1]);//状态
                            S7.SetDIntAt(tbufferValue, 1 * 4, tValue[1]);
                            tValue[2] = int.Parse(tValues[2]);//暂存位
                            S7.SetDIntAt(tbufferValue, 2 * 4, tValue[2]);
                            tValue[3] = int.Parse(tValues[3]);//宽
                            S7.SetDIntAt(tbufferValue, 3 * 4, tValue[3]);
                            tValue[4] = int.Parse(tValues[4]);//高
                            S7.SetDIntAt(tbufferValue, 4 * 4, tValue[4]);
                            tValue[5] = int.Parse(tValues[5]);//厚度
                            S7.SetDIntAt(tbufferValue, 5 * 4, tValue[5]);
                            tValue[6] = int.Parse(tValues[6]);//玻璃编号
                            S7.SetDIntAt(tbufferValue, 6 * 4, tValue[6]);
                            tValue[7] = int.Parse(tValues[7]);//数量量
                            S7.SetDIntAt(tbufferValue, 7 * 4, tValue[7]);
                            tValue[8] = int.Parse(tValues[8]);//指令编号
                            S7.SetDIntAt(tbufferValue, 8 * 4, tValue[8]);

                            if (clsPutLoad.Enport == 1)
                            {

                                readResult = PlcClient[_Plcline].DBWrite(701, 0, tbufferValue.Length, tbufferValue);
                            }
                            else
                            {
                                readResult = PlcClient[_Plcline].DBWrite(701, 296, tbufferValue.Length, tbufferValue);
                            }
                            //Write_PLC_Data(0, 700, 0, tbufferValue);
                            tSystem.mFile.WriteLog("", "原片仓入口 流水号" + tValue[0].ToString() + " 状态 " + tValue[1] + " " + dataSet.Tables[dataSet.Tables.Count - 1].Rows[0][1].ToString() + " 编号 " + tDataStr + "Y");
                            return true;
                        }
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }

        }
        private Boolean  GetCarCmd(int _Plcline,  object _Obj = null)
        {
            try
            {
                ClsDbAcc tClsDB = new ClsDbAcc(tSystem);
                string tRetData = "", tRetStr = ""; int tRows = 0;
                DataSet dataSet = new DataSet();
                DataTable tDt = new DataTable();
                string tDataStr = "";
                CarObj carObj = new CarObj();
                
                carObj =(CarObj)_Obj;
                tDt = (DataTable)carObj.obj;
                tDataStr=
                          carObj.CarNO.ToString() + "_" +      //1
                          carObj.RunStatus.ToString() + "_" +  //2
                          carObj.CarPos.ToString() + "_" +  //3
                          carObj.toPos.ToString() + "_" +  //4
                          carObj.CmdNO.ToString() + "_" +  //5
                          carObj.height.ToString() + "_" +  //6
                          carObj.width.ToString() + "_" +  //7
                          carObj.Ply.ToString() + "_" +  //8
                          carObj.Matid.ToString() + "_" +  //9
                          carObj.Count.ToString() + "_" +  //10
                          carObj.StatusIF.ToString() + "_" +  //11
                          carObj.Plcflow.ToString();  //12
                //数据参数
                //tDt = null;//可以使用
                dataSet = tClsDB.ExecuteBYuan("BYuan_10GetCarCmd", carObj.RunStatus , carObj.Plcflow , tDataStr, tDt, carObj.CarNO.ToString(), ref tRetData, ref tRetStr, ref tRows);

                if (dataSet == null) { mEnportTag = 0; return false; }
                if (dataSet.Tables.Count > 0)
                {
                    if (dataSet.Tables[dataSet.Tables.Count - 1].Rows.Count == 0) { mEnportTag = 0; return false; }
                    if (dataSet.Tables[dataSet.Tables.Count - 1].Rows[0][0].ToString().ToUpper() == "Y" & dataSet.Tables[dataSet.Tables.Count - 1].Rows[0][2].ToString().ToUpper() == "原片索车" + carObj.CarNO.ToString())
                    {
                        string[] tValues = dataSet.Tables[dataSet.Tables.Count - 1].Rows[0][1].ToString().Split(',');
                        if (tValues.Length >= 9)
                        {
                            int readResult = 0;
                            byte[] tbufferValue = new byte[40];
                            int[] tValue = new int[10];

                            tValue[0] = int.Parse(tValues[0]);//状态
                            S7.SetDIntAt(tbufferValue, 0 * 4, tValue[0]);
                            tValue[1] = int.Parse(tValues[1]);//指令序号
                            S7.SetDIntAt(tbufferValue, 1 * 4, tValue[1]);
                            tValue[2] = int.Parse(tValues[2]);//每架玻璃流水号
                            S7.SetDIntAt(tbufferValue, 2 * 4, tValue[2]);
                            tValue[3] = int.Parse(tValues[3]);//源位置
                            S7.SetDIntAt(tbufferValue, 3 * 4, tValue[3]);
                            tValue[4] = int.Parse(tValues[4]);//目的暂存位置
                            S7.SetDIntAt(tbufferValue, 4 * 4, tValue[4]);
                            tValue[5] = int.Parse(tValues[5]);//玻璃长边mm
                            S7.SetDIntAt(tbufferValue, 5 * 4, tValue[5]);
                            tValue[6] = int.Parse(tValues[6]);//玻璃短边mm
                            S7.SetDIntAt(tbufferValue, 6 * 4, tValue[6]);
                            tValue[7] = int.Parse(tValues[7]);//厚度
                            S7.SetDIntAt(tbufferValue, 7 * 4, tValue[7]);
                            tValue[8] = int.Parse(tValues[8]);//玻璃品种编号
                            S7.SetDIntAt(tbufferValue, 8 * 4, tValue[8]);
                            tValue[9] = int.Parse(tValues[9]);//数量
                            S7.SetDIntAt(tbufferValue, 9 * 4, tValue[9]);


                            readResult = PlcClient[_Plcline].DBWrite(701, 40, tbufferValue.Length, tbufferValue);
                            //Write_PLC_Data(0, 700, 0, tbufferValue);
                            tSystem.mFile.WriteLog("", "原片仓执行 状态" + tValue[0].ToString() + " 执行序号 " + tValue[1] + " 编号 " + tDataStr + "Y");
                            return true;
                        }
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }

        }
        private Boolean  CarFinish(int _Plcline,  object _Obj = null)
        {
            try
            {
                ClsDbAcc tClsDB = new ClsDbAcc(tSystem);
                string tRetData = "", tRetStr = ""; int tRows = 0;
                DataSet dataSet = new DataSet();
                DataTable tDt = new DataTable();
                string tDataStr = "";
                CarFisishObj carFisishObj = new CarFisishObj();
                carFisishObj = (CarFisishObj)_Obj;
                tDataStr =
                         carFisishObj.CarNO.ToString() + "_"//1
                       + carFisishObj.Status.ToString()+"_"//2
                       + carFisishObj.Cmd.ToString() + "_"//3
                       + carFisishObj.Flow.ToString() + "_"//4
                       + carFisishObj.Matid.ToString() + "_"//5
                       + carFisishObj.ToPos.ToString() + "_"//6
                       + carFisishObj.FromPos.ToString();//7
                tDt = null;
                dataSet = tClsDB.ExecuteBYuan("BYuan_11CmdFinish", carFisishObj.Status, carFisishObj.Cmd, tDataStr, tDt, carFisishObj.CarNO.ToString(), ref tRetData, ref tRetStr, ref tRows);

                if (dataSet == null) { mEnportTag = 0; return false; }
                if (dataSet.Tables.Count > 0)
                {
                    if (dataSet.Tables[dataSet.Tables.Count - 1].Rows.Count == 0) { mEnportTag = 0; return false; }
                    if (dataSet.Tables[dataSet.Tables.Count - 1].Rows[0][0].ToString().ToUpper() == "Y" & dataSet.Tables[dataSet.Tables.Count - 1].Rows[0][2].ToString().ToUpper() == "原片索车完成"+ carFisishObj.CarNO.ToString())
                    {
                        int readResult = 0;
                        byte[] tbufferValue = new byte[8];
                        int[] tValue = new int[2];

                        tValue[0] = 1;
                        S7.SetDIntAt(tbufferValue, 0 * 4, tValue[0]);

                        tValue[1] = carFisishObj.Cmd;
                        S7.SetDIntAt(tbufferValue, 1 * 4, tValue[1]);
                        readResult = PlcClient[_Plcline].DBWrite(701, 96, tbufferValue.Length, tbufferValue);
                        tSystem.mFile.WriteLog("", "原片仓完成 状态" + tValue[0].ToString() + " 编号 " + tValue[1] + " " + dataSet.Tables[dataSet.Tables.Count - 1].Rows[0][1].ToString() + " 编号 " + tDataStr + "Y");
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }

        }
        private Boolean  SingleFinish(int _Plcline,  object _Obj=null)
        {
            try
            {
                ClsDbAcc tClsDB = new ClsDbAcc(tSystem);
                string tRetData = "", tRetStr = ""; int tRows = 0;
                DataSet dataSet = new DataSet();
                DataTable tDt = new DataTable();
                string tDataStr = "";
                CutStartObj cutStartObj = new CutStartObj();
                cutStartObj = (CutStartObj)_Obj;
                tDataStr =
                         cutStartObj.Pos.ToString() + "_"//1
                       + cutStartObj.Status.ToString() + "_"//2
                       + cutStartObj.SingleFlow.ToString() + "_"//3
                       + cutStartObj.Flow.ToString() + "_"//4
                       + cutStartObj.Matid.ToString() + "_"//5
                       + cutStartObj.LCount.ToString() + "_"//6
                       + cutStartObj.Num.ToString();//7
                dataSet = tClsDB.ExecuteBYuan("BYuan_21SingleFinish", cutStartObj.Status, cutStartObj.SingleFlow, tDataStr, null, cutStartObj.Pos.ToString(), ref tRetData, ref tRetStr, ref tRows);

                if (dataSet == null) { mEnportTag = 0; return false; }
                if (dataSet.Tables.Count > 0)
                {
                    if (dataSet.Tables[dataSet.Tables.Count - 1].Rows.Count == 0) { mEnportTag = 0; return false; }
                    if (dataSet.Tables[dataSet.Tables.Count - 1].Rows[0][0].ToString().ToUpper() == "Y" & dataSet.Tables[dataSet.Tables.Count - 1].Rows[0][2].ToString().ToUpper() == "切割取片" + cutStartObj.Pos.ToString())
                    {
                        int readResult = 0;
                        byte[] tbufferValue = new byte[8];
                        int[] tValue = new int[2];

                        tValue[0] = 1;
                        S7.SetDIntAt(tbufferValue, 0 * 4, tValue[0]);

                        tValue[1] = cutStartObj.SingleFlow;
                        if (cutStartObj.Num == 1)
                        {
                            readResult = PlcClient[_Plcline].DBWrite(701, 168, tbufferValue.Length, tbufferValue);
                        }
                        else if (cutStartObj.Num == 2)
                        {
                            readResult = PlcClient[_Plcline].DBWrite(701, 200, tbufferValue.Length, tbufferValue);
                        }
                        else if (cutStartObj.Num == 3)
                        {
                            readResult = PlcClient[_Plcline].DBWrite(701, 232, tbufferValue.Length, tbufferValue);
                        }
                        else if (cutStartObj.Num == 4)
                        {
                            readResult = PlcClient[_Plcline].DBWrite(701, 264, tbufferValue.Length, tbufferValue);
                        }
                        tSystem.mFile.WriteLog("", "原片仓切割取片 状态" + tValue[0].ToString() + " 编号 " + tValue[1] + " " + dataSet.Tables[dataSet.Tables.Count - 1].Rows[0][1].ToString() + " 编号 " + tDataStr + "W");
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
        private Boolean DownFinish(int _Plcline, object _Obj = null)
        {
            ClsDbAcc tClsDB = new ClsDbAcc(tSystem);
            string tRetData = "", tRetStr = ""; int tRows = 0;
            DataSet dataSet = new DataSet();
            DataTable tDt = new DataTable();
            string tDataStr = "";
            DownPosObj downPosObj = new DownPosObj();
            downPosObj = (DownPosObj)_Obj;
            dataSet = tClsDB.ExecuteBYuan("BYuan_20DownFinish", downPosObj.Status, downPosObj.Matid, tDataStr, null, downPosObj.Pos.ToString(), ref tRetData, ref tRetStr, ref tRows);

                if (dataSet == null) { mEnportTag = 0; return false; }
                if (dataSet.Tables.Count > 0)
                {
                    if (dataSet.Tables[dataSet.Tables.Count - 1].Rows.Count == 0) { mEnportTag = 0; return false; }
                    if (dataSet.Tables[dataSet.Tables.Count - 1].Rows[0][0].ToString().ToUpper() == "Y" & dataSet.Tables[dataSet.Tables.Count - 1].Rows[0][2].ToString().ToUpper() == "切割下载完成" + downPosObj.Pos.ToString())
                    {
                        string[] tValues = dataSet.Tables[dataSet.Tables.Count - 1].Rows[0][1].ToString().Split(',');
                        if (tValues.Length >= 2)
                        {
                            int readResult = 0;
                            byte[] tbufferValue = new byte[8];
                            int[] tValue = new int[2];

                            tValue[0] = int.Parse(tValues[0]);
                            S7.SetDIntAt(tbufferValue, 0 * 4, tValue[0]);

                            tValue[1] = int.Parse(tValues[1]);
                            if (downPosObj.Num == 1)
                            {
                                readResult = PlcClient[_Plcline].DBWrite(701, 136, tbufferValue.Length, tbufferValue);
                            }
                            else if (downPosObj.Num == 2)
                            {
                                readResult = PlcClient[_Plcline].DBWrite(701, 144, tbufferValue.Length, tbufferValue);
                            }
                            else if (downPosObj.Num == 3)
                            {
                                readResult = PlcClient[_Plcline].DBWrite(701, 152, tbufferValue.Length, tbufferValue);
                            }
                            else if (downPosObj.Num == 4)
                            {
                                readResult = PlcClient[_Plcline].DBWrite(701, 160, tbufferValue.Length, tbufferValue);
                            }
                            tSystem.mFile.WriteLog("", "原片仓切割下载完成 状态" + tValue[0].ToString() + " 编号 " + tValue[1] + " " + dataSet.Tables[dataSet.Tables.Count - 1].Rows[0][1].ToString() + " 编号 " + tDataStr + "W");
                        }
                    }
                }

            return true;
        }
        /// <summary>
        /// /原片仓储
        /// </summary>
        /// <param name="_Dt"></param>
        /// <param name="_No"></param>

        public class ClsObj///主对象
        {
            public int Index { get; set; }
            public object obj { get; set; }
        }
        public class ClsPutLoad//上片
        {
            public int Enport { get; set; }
            public int Flow { get; set; }
            public int Status { get; set; }
            public object Obj { get; set; }
        }

        class CarObj//梭车
        {
            public int CarNO { get; set; }
            public int RunStatus { get; set; }//牵引车状态
            public int CarPos { get; set; }//牵引车位置
            public int toPos { get; set; }//目的存储位置
            public int CmdNO { get; set; }//指令序号
            public int height { get; set; }//同步
            public int width { get; set; }//同步
            public int Ply { get; set; }//厚度
            public int Matid { get; set; }//玻璃品种编号
            public int Count { get; set; }//数量
            public int StatusIF { get; set; }
            public int Plcflow { get; set; }
            public object obj { get; set; }
            
        }
        
        class CarFisishObj
        {
            public int CarNO { get; set; }
            public int Status { get; set; }
            public int Cmd { get; set; }
            public int Flow { get; set; }
            public int Matid { get; set; }
            public int ToPos { get; set; }
            public int FromPos { get; set; }
        }

        class CutStartObj//缓存站位抓玻璃
        {
            public int Pos { get; set; }
            public int Status { get; set; }
            public int SingleFlow { get; set; }
            public int Flow { get; set; }
            //public int Use_Count { get; set; }
            //public int height { get; set; }
            //public int width { get; set; }
            //public int Ply { get; set; }
            public int Matid { get; set; }
            public int LCount { get; set; }
            public int Count { get; set; }
            public int Num { get; set; }
        }

        class DownPosObj
        {
            public int Pos { get; set; }
            public int Status { get; set; }
            public int Matid { get; set; }
            public int Num { get; set; }
            public object Obj { get; set; }
        }

        class PositionObj///站位
        {
            public int Pos { get; set; }
            public int PlcStatus { get; set; }
            public int Matid { get; set; }
            public int PlcFlow { get; set; }
            public int Count { get; set; }
            public int Rev { get; set; }

        }



 
        int mMBcheckTag = 0;
        int[] CHMBTag = new int[3] { 0, 0, 0 }; DateTime[] CHMBtagtime = new DateTime[3] { DateTime.Now, DateTime.Now, DateTime.Now };


      
        private bool MCEnportMBFinish(int _Plcline, int _Status, int _PlcFlow, int _Enport)
        {
            ClsDbAcc tClsDB = new ClsDbAcc(tSystem);
            string tRetData = "", tRetStr = ""; int tRows = 0;
            tClsDB.Execute_Command("update TabMB_Data set MBEnport ='" + _Enport.ToString() + "',Status=4 where Status<'10' and  MBPlcFlow ='" + _PlcFlow.ToString() + "'");
            tClsDB.DBClose();
            //if (tRetStr.ToUpper() != "Y")
            //{
            //    return false;
            //}

            byte[] tbufferValue = new byte[8];
            int[] tValue = new int[2];

            tValue[0] = 1;
            S7.SetDIntAt(tbufferValue, 0 * 4, tValue[0]);

            tValue[1] = _PlcFlow;
            S7.SetDIntAt(tbufferValue, 1 * 4, tValue[1]);

            if (_Enport == 1)
            {
                Write_PLC_Data(_Plcline, 751, 124, tbufferValue);
            }
            else if (_Enport == 2)
            {
                Write_PLC_Data(_Plcline, 751, 132, tbufferValue);
            }
            else if (_Enport == 3)
            {
                Write_PLC_Data(_Plcline, 751, 140, tbufferValue);
            }
            tSystem.mFile.WriteLog("", "进磨边完成 状态" + tValue[0].ToString() + " 流水号 " + tValue[1] + " 磨边编号 " + _Enport + "F");
            return true;
        }
        
        public class ClsMachineData
        {
            public int ID { get; set; }
            public string Machine_NO { get; set; }
            public int Plc_Flow { get; set; }
        }

        int EnTag1 = 0, EnTag2 = 0, EnTag3 = 0;
   
        private bool MCEnportWHFinish(int _Plcline, int _Status, int _PlcFlow, int _Enport)
        {
            ClsDbAcc tClsDB = new ClsDbAcc(tSystem);
            string tRetData = "", tRetStr = ""; int tRows = 0;
            tClsDB.ExecutePro("Pro_11FinishCmdIn", 1, _PlcFlow.ToString(), 0, 0, _Enport, ref tRetData, ref tRetStr, ref tRows);

            if (tRetStr.ToUpper() != "Y")
            {
                return false;
            }

            byte[] tbufferValue = new byte[8];
            int[] tValue = new int[2];

            tValue[0] = 1;
            S7.SetDIntAt(tbufferValue, 0 * 4, tValue[0]);

            tValue[1] = _PlcFlow;
            S7.SetDIntAt(tbufferValue, 1 * 4, tValue[1]);

            if (_Enport == 1)
            {
                Write_PLC_Data(_Plcline, 751, 224, tbufferValue);
            }
            else if (_Enport == 2)
            {
                Write_PLC_Data(_Plcline, 751, 488, tbufferValue);
            }

            tSystem.mFile.WriteLog("", "磨边库入库完成 状态" + tValue[0].ToString() + " 流水号 " + tValue[1] + " 车号 " + _Enport + "H");
            return true;
        }
       

      
        private bool Write_PLC_Data(int _Plcline, int _Add, int _Start, byte[] _bufferValue)
        {
            bool tR = true;
            try
            {
                int readResult = PlcClient[_Plcline].DBWrite(_Add, _Start, _bufferValue.Length, _bufferValue);
                if (readResult != 0)
                {
                    PlcClient[_Plcline].Disconnect();
                    Thread.Sleep(100);
                    PlcClient[_Plcline].ConnectTo(clsMyPublic.mPLCIPA, 0, 1);//// IP  机架号  插槽号 
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                tR = false;
            }

            return tR;
        }


        

        

        public class ClsMBCheck//上片
        {
            public int Enport { get; set; }
            public int Status { get; set; }
            public int Long { get; set; }
            public int Short { get; set; }
            public int Flow { get; set; }
            public int LPbinStatus { get; set; }
        }
        public class ClsEnport
        {
            public int Status { get; set; }
            public int Long { get; set; }
            public int Short { get; set; }
            public int MBNO { get; set; }
            public int[] FlowArr { get; set; }
        }
        public class ClsMCFinish//完成
        {
            public int Enport { get; set; }
            public int Flow { get; set; }
            public int Status { get; set; }
        }

        public class ClsOutFinish
        {
            public int CarNO { get; set; }
            public int Status { get; set; }
            public int OutCmd { get; set; }
            public int Flow1 { get; set; }
            public int Flow2 { get; set; }
            public int Flow3 { get; set; }
            public int Flow4 { get; set; }
        }


        /// <summary>
        /// list<T>转成datatable格式
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public static DataTable ToDataTable<T>(IEnumerable<T> collection)
        {
            var props = typeof(T).GetProperties();
            var dt = new DataTable();
            dt.Columns.AddRange(props.Select(p => new DataColumn(p.Name, p.PropertyType)).ToArray());
            if (collection.Count() > 0)
            {
                for (int i = 0; i < collection.Count(); i++)
                {
                    ArrayList tempList = new ArrayList();
                    foreach (PropertyInfo pi in props)
                    {
                        object obj = pi.GetValue(collection.ElementAt(i), null);
                        tempList.Add(obj);
                    }
                    object[] array = tempList.ToArray();
                    dt.LoadDataRow(array, true);
                }
            }
            return dt;
        }
    }
}
