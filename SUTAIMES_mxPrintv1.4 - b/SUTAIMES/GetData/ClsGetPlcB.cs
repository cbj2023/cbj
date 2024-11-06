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
    class ClsGetPlcB
    {
        Program tSystem;
        Thread mThreadGetPlcData;
        public bool mPlcStartGet = false;

        public S7Client[] PlcClient;

        public int mZKOutCmdFlow = 0;
        public ClsGetPlcB(Program tSys)
        {
            tSystem = tSys;
            mZKOutCmdFlow = SettingsMC.Default.ZKOutCmdFlow;
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
                    PlcResult = PlcClient[i].ConnectTo(clsMyPublic.mPLCIPB, 0, 1);// IP  机架号  插槽号  ///192.168.1.10   192.168.11.40
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
        int ZKCheckFlow = 0, ZKInFinishFlow1 = 0, ZKInFinishFlow2 = 0, ZKExportFlow = 0, ZKOutFinishFlow1 = 0, ZKOutFinishFlow2 = 0, ZKXiaFlow1 = 0, ZKXiaFlow2=0;
        int ZKEnportFlow1 = 0, ZKEnportFlow2 = 0, ZKEnportFlow3 = 0;
        private void GetPlcData()//5
        {
            int[] tPlcFlow = new int[35] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0,0,0 };

            int[] tFruNOFlow = new int[5] { 0, 0, 0, 0, 0 };
            do
            {
                if (mPlcStartGet == true)
                {

                    byte[] tbufferValue = new byte[440];
                    int[] tValue = new int[110];
                    int readResult = PlcClient[0].DBRead(700, 0, tbufferValue.Length, tbufferValue);
                    if (readResult != 0)
                    {
                        PlcClient[0].Disconnect();
                        Thread.Sleep(100);
                        PlcClient[0].ConnectTo(clsMyPublic.mPLCIPB, 0, 1);//// IP  机架号  插槽号 
                    }
                    else
                    {
                        for (int i = 0; i < tValue.Length; i++)
                        {
                            tValue[i] = S7.GetDIntAt(tbufferValue, i * 4);//转换成数据
                        }

                        List<ClsMachineData> tListData = new List<ClsMachineData>(); ClsMachineData tclsMachineData; int tPlcTag = 0;
                        if (tValue[0] > 0)//钢化出数据
                        {
                            for (int j = 1; j <= 5; j++)
                            {
                                if (tFruNOFlow[j - 1] != tValue[j - 1])//65
                                {
                                    tPlcTag = 1;
                                    tFruNOFlow[j - 1] = tValue[j - 1];
                                    tclsMachineData = new ClsMachineData(); tclsMachineData.ID = j; tclsMachineData.Plc_Flow = tValue[j - 1]; tListData.Add(tclsMachineData);
                                }
                            }
                        }

                        if (
                            ((tValue[98] == 1 & tValue[99] > 0) | (tValue[98] ==2 & tValue[99] >0))
                            
                            & tValue[100] > 0 & tValue[101] > 0)//1号上片口
                        {
                            ClsObj clsObj1 = new ClsObj();
                            ClsPutLoad tClsPutLoad = new ClsPutLoad();
                            tClsPutLoad.mStatus = tValue[98];
                            tClsPutLoad.mFlow = tValue[99];
                            tClsPutLoad.Status = tValue[100];
                            tClsPutLoad.Flow = tValue[101];
                            tClsPutLoad.Enport = 1;
                            clsObj1.Index = 1;
                            clsObj1.obj = tClsPutLoad;
                            Thread thread1 = new Thread(new ParameterizedThreadStart(Dispose));
                            thread1.Start(clsObj1);

                        }

                        if (((tValue[104] == 1 & tValue[105] > 0) | (tValue[104] == 2 & tValue[105] > 0))
                            & tValue[106] > 0 & tValue[107] > 0)//1号上片口
                        {
                            ClsObj clsObj2 = new ClsObj();
                            ClsPutLoad tClsPutLoad = new ClsPutLoad();
                            tClsPutLoad.mStatus = tValue[104];
                            tClsPutLoad.mFlow = tValue[105];
                            tClsPutLoad.Status = tValue[106];
                            tClsPutLoad.Flow = tValue[107];
                            tClsPutLoad.Enport = 2;
                            clsObj2.Index = 1;
                            clsObj2.obj = tClsPutLoad;
                            Thread thread1 = new Thread(new ParameterizedThreadStart(Dispose));
                            thread1.Start(clsObj2);

                        }

                        if (tValue[28] > 0 & tValue[29] > 0 & ZKXiaFlow1 != tValue[29])//下片完成
                        {
                            ZKXiaFlow1 = tValue[29];
                            ClsObj clsObj341 = new ClsObj();
                            ClsZKFinish clsZKFinishcls341 = new ClsZKFinish();
                            clsObj341.Index = 26;//下片完成
                            clsZKFinishcls341.TroNO = 1;
                            clsZKFinishcls341.Flow = tValue[29];
                            clsZKFinishcls341.Status = tValue[28];
                            clsObj341.obj = clsZKFinishcls341;
                            Thread thread1 = new Thread(new ParameterizedThreadStart(Dispose));
                            thread1.Start(clsObj341);
                        }
                        if (tValue[30] > 0 & tValue[31] > 0 & ZKXiaFlow2 != tValue[31])//下片完成
                        {
                            ZKXiaFlow1 = tValue[31];
                            ClsObj clsObj341 = new ClsObj();
                            ClsZKFinish clsZKFinishcls341 = new ClsZKFinish();
                            clsObj341.Index = 26;//下片完成
                            clsZKFinishcls341.TroNO = 2;
                            clsZKFinishcls341.Flow = tValue[31];
                            clsZKFinishcls341.Status = tValue[30];
                            clsObj341.obj = clsZKFinishcls341;
                            Thread thread1 = new Thread(new ParameterizedThreadStart(Dispose));
                            thread1.Start(clsObj341);
                        }


                        if (tValue[6] > 0 & tValue[7] > 0 & tValue[8] > 0 & tValue[9] > 0 & ZKCheckFlow != tValue[9])//中空库钢化后前测量段
                        {
                            ZKCheckFlow = tValue[9];
                            ClsObj clsObj2 = new ClsObj();
                            ClsZKCheck clsZKCheck = new ClsZKCheck();
                            clsObj2.Index = 28;//数据核对
                            clsZKCheck.Status = tValue[6]; clsZKCheck.Long = tValue[7]; clsZKCheck.Short = tValue[8];
                            int[] tFlowArr = new int[1];
                            tFlowArr[0] = tValue[9];
                            clsZKCheck.FlowArr = tFlowArr;
                            clsZKCheck.MBNO = tValue[13];
                            clsObj2.obj = clsZKCheck;
                            Thread thread2 = new Thread(new ParameterizedThreadStart(Dispose));
                            thread2.Start(clsObj2);
                        }



                        //if (tValue[34] > 0 & tValue[39] > 0 & tValue[42] > 0 & tValue[43] > 0 & ZKCheckFlow != tValue[39])//中空库前测量段 1号口
                        //{
                        //    ZKCheckFlow = tValue[39];
                        //    ClsObj clsObj2 = new ClsObj();
                        //    ClsZKCheck clsZKCheck = new ClsZKCheck();
                        //    clsObj2.Index = 31;//数据核对
                        //    clsZKCheck.Status = tValue[34]; clsZKCheck.Long = tValue[42]; clsZKCheck.Short = tValue[43];
                        //    int[] tFlowArr = new int[1];
                        //    tFlowArr[0] = tValue[9];
                        //    clsZKCheck.FlowArr = tFlowArr;
                        //    clsZKCheck.MBNO = 1;
                        //    clsObj2.obj = clsZKCheck;
                        //    Thread thread2 = new Thread(new ParameterizedThreadStart(Dispose));
                        //    thread2.Start(clsObj2);
                        //}

                        if (tValue[34] > 0 & tValue[39] > 0 & tValue[41] > 0 & tValue[42] > 0 & ZKEnportFlow1 != tValue[39])//入库中空库前测量段 1号口
                        {
                            ZKEnportFlow1 = tValue[39];
                            ClsObj clsObj10 = new ClsObj();
                            ClsEnport clsEnport10 = new ClsEnport();
                            clsObj10.Index = 10;//入库口
                            clsEnport10.Status = tValue[34];
                            clsEnport10.MBNO = 1;// tValue[26];
                            clsEnport10.Long = tValue[41];
                            clsEnport10.Short = tValue[42];
                            int[] tFlowArr = new int[7];
                            tFlowArr[0] = tValue[39]; tFlowArr[1] = tValue[40]; tFlowArr[2] = tValue[35]; tFlowArr[3] = tValue[36];
                            tFlowArr[4] = tValue[37]; tFlowArr[5] = tValue[38]; tFlowArr[6] = tValue[43];
                            clsEnport10.FlowArr = tFlowArr;
                            clsObj10.obj = clsEnport10;
                            Thread thread10 = new Thread(new ParameterizedThreadStart(Dispose));
                            thread10.Start(clsObj10);
                        }


                        if (tValue[78] > 0 & tValue[83] > 0 & tValue[85] > 0 & tValue[86] > 0 & ZKEnportFlow2 != tValue[83])//中空库前测量段 2号口
                        {
                            ZKEnportFlow2 = tValue[83];
                            ClsObj clsObj10 = new ClsObj();
                            ClsEnport clsEnport10 = new ClsEnport();
                            clsObj10.Index = 10;//入库口
                            clsEnport10.Status = tValue[78];
                            clsEnport10.MBNO = 2;// tValue[26];
                            clsEnport10.Long = tValue[85];
                            clsEnport10.Short = tValue[86];
                            int[] tFlowArr = new int[7];
                            tFlowArr[0] = tValue[83]; tFlowArr[1] = tValue[84]; tFlowArr[2] = tValue[79]; tFlowArr[3] = tValue[80];
                            tFlowArr[4] = tValue[81]; tFlowArr[5] = tValue[82]; tFlowArr[6] = tValue[87];
                            clsEnport10.FlowArr = tFlowArr;
                            clsObj10.obj = clsEnport10;
                            Thread thread10 = new Thread(new ParameterizedThreadStart(Dispose));
                            thread10.Start(clsObj10);
                        }
                        if (tValue[88] > 0 & tValue[93] > 0 & tValue[95] > 0 & tValue[96] > 0 & ZKEnportFlow3 != tValue[93])//中空库前测量段 3号口
                        {
                            ZKEnportFlow3 = tValue[93];
                            ClsObj clsObj10 = new ClsObj();
                            ClsEnport clsEnport10 = new ClsEnport();
                            clsObj10.Index = 10;//入库口
                            clsEnport10.Status = tValue[88];
                            clsEnport10.MBNO = 3;// tValue[26];
                            clsEnport10.Long = tValue[95];
                            clsEnport10.Short = tValue[96];
                            int[] tFlowArr = new int[7];
                            tFlowArr[0] = tValue[93]; tFlowArr[1] = tValue[94]; tFlowArr[2] = tValue[89]; tFlowArr[3] = tValue[90];
                            tFlowArr[4] = tValue[91]; tFlowArr[5] = tValue[92]; tFlowArr[6] = tValue[97];
                            clsEnport10.FlowArr = tFlowArr;
                            clsObj10.obj = clsEnport10;
                            Thread thread10 = new Thread(new ParameterizedThreadStart(Dispose));
                            thread10.Start(clsObj10);
                        }

                        if (tValue[57] > 0 & tValue[58] > 0 & ZKInFinishFlow1 != tValue[58])//入库完成1
                        {
                            ZKInFinishFlow1 = tValue[58];
                            ClsObj clsObj341 = new ClsObj();
                            ClsZKFinish clsZKFinishcls341 = new ClsZKFinish();
                            clsObj341.Index = 34;//中空库完成
                            clsZKFinishcls341.TroNO = 1;
                            clsZKFinishcls341.Flow = tValue[58];
                            clsZKFinishcls341.Status = tValue[57];
                            clsObj341.obj = clsZKFinishcls341;
                            Thread thread1 = new Thread(new ParameterizedThreadStart(Dispose));
                            thread1.Start(clsObj341);
                        }
                     
                        if (tValue[59] > 0 & tValue[60] > 0 & ZKInFinishFlow2 != tValue[60])//入库完成2
                        {
                            ZKInFinishFlow2 = tValue[23];
                            ClsObj clsObj342 = new ClsObj();
                            ClsZKFinish clsZKFinishcls342 = new ClsZKFinish();
                            clsObj342.Index = 34;//中空库完成
                            clsZKFinishcls342.TroNO = 2;
                            clsZKFinishcls342.Flow = tValue[60];
                            clsZKFinishcls342.Status = tValue[59];
                            clsObj342.obj = clsZKFinishcls342;
                            Thread thread1 = new Thread(new ParameterizedThreadStart(Dispose));
                            thread1.Start(clsObj342);
                        }
                

                        if (tValue[61] > 0 & tValue[62] > 0 & ZKOutFinishFlow1 != tValue[62])//仓储出库完成1
                        {
                            ZKOutFinishFlow1 = tValue[62];
                            ClsObj clsObj44 = new ClsObj();
                            clsObj44.Index = 44;//出库完成
                            ClsZKOutFinish clsOutFinish44 = new ClsZKOutFinish();
                            clsOutFinish44.CarNO = 1;
                            clsOutFinish44.Status = tValue[61];
                            clsOutFinish44.OutCmd = tValue[62];

                            clsOutFinish44.Flow1 = tValue[63];
                            clsOutFinish44.Flow2 = tValue[64];
                            clsOutFinish44.Flow3 = tValue[65];
                            clsOutFinish44.Flow4 = tValue[66];

                            clsObj44.obj = clsOutFinish44;
                            Thread threadC = new Thread(new ParameterizedThreadStart(Dispose));
                            threadC.Start(clsObj44);
                        }

                        if (tValue[67] > 0 & tValue[68] > 0 & ZKOutFinishFlow1 != tValue[68])//仓储出库完成2
                        {
                            ZKOutFinishFlow1 = tValue[68];
                            ClsObj clsObj44 = new ClsObj();
                            clsObj44.Index = 44;//出库完成
                            ClsZKOutFinish clsOutFinish44 = new ClsZKOutFinish();
                            clsOutFinish44.CarNO = 2;
                            clsOutFinish44.Status = tValue[67];
                            clsOutFinish44.OutCmd = tValue[68];

                            clsOutFinish44.Flow1 = tValue[69];
                            clsOutFinish44.Flow2 = tValue[70];
                            clsOutFinish44.Flow3 = tValue[71];
                            clsOutFinish44.Flow4 = tValue[72];

                            clsObj44.obj = clsOutFinish44;
                            Thread threadC = new Thread(new ParameterizedThreadStart(Dispose));
                            threadC.Start(clsObj44);
                        }




                        if ((tValue[50] > 0 | tValue[51] > 0) & ZKExportFlow == 0 & (tValue[52] > 0 | tValue[53] > 0 | tValue[54] > 0))//中空仓储出库  ClsZKOutFinish
                        {
                            ZKExportFlow = 1;
                            ClsObj clsObj40 = new ClsObj();
                            ClsExit tclsExit = new ClsExit();
                            clsObj40.Index = 40;//中空仓储出库

                            tclsExit.Status = tValue[50] * 10000 + tValue[51] * 1000 + tValue[52] * 100 + tValue[53] * 10 + tValue[54] * 1;
                            tclsExit.Exit1 = tValue[52];
                            tclsExit.Exit2 = tValue[53];
                            tclsExit.Exit3 = tValue[54];

                            clsObj40.obj = tclsExit;

                            Thread threadC = new Thread(new ParameterizedThreadStart(Dispose));
                            threadC.Start(clsObj40);
                        }


                        //if ((tValue[42] > 0 | tValue[43] > 0) & (tValue[44] > 0 | tValue[45] > 0 | tValue[46] > 0) & MCExportFlow == 0
                        //    //& 1 == 2
                        //   )//磨边仓储出库
                        //{
                        //    MCExportFlow = 1;
                        //    ClsObj clsObj20 = new ClsObj();
                        //    clsObj20.Index = 20;//磨边仓储出库
                        //    clsObj20.obj = tValue[42] * 10000 + tValue[43] * 1000 + tValue[44] * 100 + tValue[45] * 10 + tValue[46] * 1;
                        //    Thread threadC = new Thread(new ParameterizedThreadStart(Dispose));
                        //    threadC.Start(clsObj20);
                        //}


                        if (tPlcTag == 1)
                        {

                            ClsObj clsObj112 = new ClsObj();
                            clsObj112.Index = 112;//获取PLC流水号
                            clsObj112.obj = tListData;
                            Thread thread111 = new Thread(new ParameterizedThreadStart(Dispose));
                            thread111.Start(clsObj112);
                        }

                        //List<ClsMachineData> tListData = new List<ClsMachineData>(); ClsMachineData tclsMachineData; int tPlcTag = 0;
                        //for (int j = 1; j <= 34; j++)
                        //{
                        //    if (tPlcFlow[j - 1] != tValue[39 + j])
                        //    {
                        //        tPlcTag = 1;
                        //        tPlcFlow[j - 1] = tValue[39 + j];
                        //        tclsMachineData = new ClsMachineData(); tclsMachineData.ID = j; tclsMachineData.Plc_Flow = tValue[39 + j]; tListData.Add(tclsMachineData);
                        //    }
                        //}
                        //if (tPlcFlow[34] != tValue[39])
                        //{
                        //    tPlcTag = 1;
                        //    tPlcFlow[34] = tValue[39];
                        //    tclsMachineData = new ClsMachineData(); tclsMachineData.ID = 999; tclsMachineData.Plc_Flow = tValue[39]; tListData.Add(tclsMachineData);
                        //}

                        
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
            switch (tobj.Index)
            {
                case 1:
                    ClsPutLoad tClsPutLoad = new ClsPutLoad();
                    tClsPutLoad = (ClsPutLoad)tobj.obj;
                    PutLoad(1,tClsPutLoad);
                    break;
                case 10:
                    ClsEnport clsEnport10 = new ClsEnport();
                    clsEnport10 = (ClsEnport)tobj.obj;
                    ZKEnportWH(4, clsEnport10.Status, clsEnport10.Long, clsEnport10.Short, clsEnport10.MBNO, clsEnport10.FlowArr);//clsEnport10.MBNO入口
                    if (clsEnport10.MBNO == 1)
                    {
                        ZKEnportFlow1 = 0;
                    }
                    else if (clsEnport10.MBNO == 2)
                    {
                        ZKEnportFlow2 = 0;
                    }
                    else if (clsEnport10.MBNO == 3)
                    {
                        ZKEnportFlow3 = 0;
                    }
                    
                    break;
                case 28:
                    ClsZKCheck clsZKCheck28 = new ClsZKCheck();
                    clsZKCheck28 = (ClsZKCheck)tobj.obj;
                    MCCheck(1,  clsZKCheck28.Status, clsZKCheck28.FlowArr[0]  , clsZKCheck28.Long, clsZKCheck28.Short, clsZKCheck28.MBNO);
                    ZKCheckFlow = 0;
                    break;
                case 30:
                    ClsZKCheck clsZKCheck30 = new ClsZKCheck();
                    clsZKCheck30 = (ClsZKCheck)tobj.obj;
                    ZKEnportWH(1, clsZKCheck30.Status, clsZKCheck30.Long, clsZKCheck30.Short, clsZKCheck30.MBNO, clsZKCheck30.FlowArr);
                    ZKCheckFlow = 0;
                    break;
                case 34:
                    ClsZKFinish clsZKFinishcls34 = new ClsZKFinish();
                    clsZKFinishcls34 = (ClsZKFinish)tobj.obj;
                    ZKEnportWHFinish(2, clsZKFinishcls34.Status, clsZKFinishcls34.Flow, clsZKFinishcls34.TroNO);
                    if (clsZKFinishcls34.TroNO == 1)
                    { ZKInFinishFlow1 = 0;  }
                    else
                    { ZKInFinishFlow2 = 0; }

                    break;
                case 40:
                    ClsExit tclsExit = new ClsExit();
                    tclsExit = (ClsExit)tobj.obj;
                    ZKWHoutCmd(3, 1, tclsExit.Exit1, tclsExit.Exit2, tclsExit.Exit3,tclsExit.Status );
                    ZKExportFlow = 0;
                    break;
                case 44:
                    ClsZKOutFinish clsOutFinish44 = new ClsZKOutFinish();
                    clsOutFinish44 = (ClsZKOutFinish)tobj.obj;
                    ZKWHoutFinish(4, clsOutFinish44.OutCmd, clsOutFinish44.Flow1, clsOutFinish44.Flow2, clsOutFinish44.Flow3, clsOutFinish44.Flow4,clsOutFinish44.CarNO );
                    ZKOutFinishFlow1 = 0;
                    break;
                case 999:
                    List<ClsMachineData> tListData = new List<ClsMachineData>();
                    tListData = (List<ClsMachineData>)tobj.obj;

                    DataTable tDt = new DataTable();
                    tDt = ToDataTable<ClsMachineData>(tListData);
                    MCPlcflow(tDt);
                    break;
                case 112:
                    List<ClsMachineData> tListDataFur = new List<ClsMachineData>();
                    tListDataFur = (List<ClsMachineData>)tobj.obj;

                    DataTable tDtFur = new DataTable();
                    tDtFur = ToDataTable<ClsMachineData>(tListDataFur);
                    if (int.Parse(tDtFur.Rows[0][2].ToString()) > 0)
                    {
                        MCPlcflowFur(tDtFur, 1);
                    }
                    break;
                case 26://下片完成
                    ClsZKFinish clsZKFinishcls26 = new ClsZKFinish();
                    clsZKFinishcls26 = (ClsZKFinish)tobj.obj;
                    GHxiaPian(2, clsZKFinishcls26.Status, clsZKFinishcls26.Flow, clsZKFinishcls26.TroNO);
                    if (clsZKFinishcls26.TroNO == 1)
                    { ZKXiaFlow1  = 0;  }
                    else
                    { ZKXiaFlow1 = 0; }
                    
                    break;

            }
        }

        private bool PutLoad(int _Plcline, ClsPutLoad _ClsPutLoad)
        {
            try
            {


                ClsDbAcc tClsDB = new ClsDbAcc(tSystem);
                string tRetData = "", tRetStr = "Y"; int tRows = 0;
                tClsDB.ExecutePro("Pro_29MBStart", _ClsPutLoad.mStatus , _ClsPutLoad.mFlow.ToString() , _ClsPutLoad.Status , _ClsPutLoad.Flow , _ClsPutLoad.Enport , ref tRetData, ref tRetStr, ref tRows);
                if (tRetStr.ToUpper() == "Y")
                {
                    //tRetData = "1,1150,1020,"+_ClsPutLoad.Flow.ToString()+",3,0,0";
                    string[] tValues = tRetData.Split(',');
                    if (tValues.Length == 7)
                    {
                        byte[] tbufferValue = new byte[20];
                        int[] tValue = new int[5];

                        tValue[0] = int.Parse(tValues[0]);//状态
                        S7.SetDIntAt(tbufferValue, 0 * 4, tValue[0]);

                        tValue[1] = int.Parse(tValues[1]);//长边
                        S7.SetDIntAt(tbufferValue, 1 * 4, tValue[1] * 1000);

                        tValue[2] = int.Parse(tValues[2]);//短边
                        S7.SetDIntAt(tbufferValue, 2 * 4, tValue[2] * 1000);

                        tValue[3] = int.Parse(tValues[3]);//流水号
                        if (tValue[3] != _ClsPutLoad.Flow)
                        {
                            return false;
                        }
                        S7.SetDIntAt(tbufferValue, 3 * 4, tValue[3]);

                        tValue[4] = int.Parse(tValues[4]); ;//数量
                        S7.SetDIntAt(tbufferValue, 4 * 4, tValue[4]);
                        if (_ClsPutLoad.Enport  == 1)
                        {
                            Write_PLC_Data(_Plcline, 751, 392, tbufferValue);
                        }
                        else if (_ClsPutLoad.Enport == 2)
                        {
                            Write_PLC_Data(_Plcline, 751, 416, tbufferValue);
                        }
                        

                        //tSystem.mFile.WriteLog("", "上片 线" + _Enport + "  状态 " + tValue[0].ToString() + " 长边 " + tValue[1] + " 短边" + tValue[2] + " 流水号" + tValue[3] + "  走向 " + tValue[4] + " WCSID " + tValues[6] + "C");
                        return true;
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
        private void MCPlcflowFur(DataTable _Dt, int _No)
        {
            ClsDbAcc tClsDB = new ClsDbAcc(tSystem);
            DataSet dataSet = new DataSet(); string tRetData = "";
            dataSet = tClsDB.ExecuteGetPlcFlowTvp("ProFurNO", 100, _Dt);
            if (dataSet.Tables.Count > 0)
            {
                //if (dataSet.Tables[dataSet.Tables.Count - 1].Rows.Count == 0) { return; }
                byte[] tbufferValue = new byte[8];
                int[] tValue = new int[2];
                tValue[0] = 1;
                tValue[1] = int.Parse(_Dt.Rows[0][2].ToString());
                S7.SetDIntAt(tbufferValue, 0, tValue[0]);//玻璃标识 A B C
                S7.SetDIntAt(tbufferValue, 1 * 4, tValue[1]);//玻璃标识 A B C

                Write_PLC_Data(1, 751, 0, tbufferValue);

            }
        }
        private void MCPlcflow(DataTable _Dt)
        {
            ClsDbAcc tClsDB = new ClsDbAcc(tSystem);
            DataSet dataSet = new DataSet(); string tRetData = "";
            dataSet = tClsDB.ExecuteGetPlcFlowTvp("ProPlcFlow", 100, _Dt);
            if (dataSet.Tables.Count > 0)
            {
                if (dataSet.Tables[dataSet.Tables.Count - 1].Rows.Count == 0) { return; }
                if (dataSet.Tables[dataSet.Tables.Count - 1].Rows[0][0].ToString().ToUpper() == "Y" & dataSet.Tables[dataSet.Tables.Count - 1].Rows[0][2].ToString().ToUpper() == "中空")
                {
                    tRetData = dataSet.Tables[dataSet.Tables.Count - 1].Rows[0][1].ToString();
                    string[] tValues = tRetData.Split(',');
                    //convert(varchar,b.WcsID) +','+convert(varchar,b.Single_long) +','+convert(varchar,b.Singel_Short) +','+Single_Tag +','+Single_Str +','+convert(varchar,b.PlcFlow) +','+b.Process_number 

                    if (tValues.Length == 7)
                    {

                    }
                }
            }
        }

        private bool  GHxiaPian(int _Plcline, int _Status, int _PlcFlow, int _TroNo)
        {

            ClsDbAcc tClsDB = new ClsDbAcc(tSystem);
            string tRetData = "", tRetStr = ""; int tRows = 0;
            tClsDB.ExecutePro("Pro_26FinishCmdXIA", 1, _PlcFlow.ToString(), _TroNo, 0, 0, ref tRetData, ref tRetStr, ref tRows);

            if (tRetStr.ToUpper() != "Y")
            {
                return false;
            }
            byte[] tbufferValue = new byte[8];
            int[] tValue = new int[2];

            tValue[0] = _Status;
            S7.SetDIntAt(tbufferValue, 0 * 4, tValue[0]);

            tValue[1] = _PlcFlow;
            S7.SetDIntAt(tbufferValue, 1 * 4, tValue[1]);

            if (_TroNo == 1)
            { Write_PLC_Data(_Plcline, 751, 112, tbufferValue); }
            else
            { Write_PLC_Data(_Plcline, 751, 120, tbufferValue); }

            return true;
        }

        private bool MCCheck(int _Plcline, int _Status, int _PlcFlow, int _Long, int _Short, int _Enport)//钢化后测量入口  _Enporth厚度
        {
            ClsDbAcc tClsDB = new ClsDbAcc(tSystem);
            string tRetData = "", tRetStr = ""; int tRows = 0;
            tClsDB.ExecutePro("Pro_25MBCheckNew", _Status, _PlcFlow.ToString(), _Long, _Short, _Enport, ref tRetData, ref tRetStr, ref tRows);
            if (tRetStr.ToUpper() == "Y")
            {
                string[] tValues = tRetData.Split(',');
                if (tValues.Length == 11)
                {
                    byte[] tbufferValue = new byte[44];
                    int[] tValue = new int[11];

                    tValue[0] = _Status;//状态
                    S7.SetDIntAt(tbufferValue, 0 * 4, tValue[0]);

                    tValue[1] = int.Parse(tValues[2]);
                    S7.SetDIntAt(tbufferValue, 1 * 4, tValue[1] * 1000);

                    tValue[2] = int.Parse(tValues[3]);
                    S7.SetDIntAt(tbufferValue, 2 * 4, tValue[2] * 1000);

                    tValue[3] = int.Parse(tValues[5]);//流水号
                    if (tValue[3] != _PlcFlow)
                    {
                        return false;
                    }
                    S7.SetDIntAt(tbufferValue, 3 * 4, tValue[3]);

                    tValue[4] = int.Parse(tValues[4]); ;//走向  
                    S7.SetDIntAt(tbufferValue, 4 * 4, tValue[4]);

                    tValue[5] = int.Parse(tValues[6]);//理片机
                    S7.SetDIntAt(tbufferValue, 5 * 4, tValue[5]);

                    tValue[6] = int.Parse(tValues[7]);//第几片
                    S7.SetDIntAt(tbufferValue, 6 * 4, tValue[6]);

                    tValue[7] = 1;//PC控制
                    S7.SetDIntAt(tbufferValue, 7 * 4, tValue[7]);
                    tValue[8] = int.Parse(tValues[8]);//厚度
                    S7.SetDIntAt(tbufferValue, 8 * 4, tValue[8]);//旗滨改为厚度
                    tValue[9] = int.Parse(tValues[9]);
                    S7.SetDIntAt(tbufferValue, 9 * 4, tValue[9]);//旗滨 流程卡编号
                    tValue[10] = int.Parse(tValues[10]);
                    S7.SetDIntAt(tbufferValue, 10 * 4, tValue[10]);//旗滨 流程卡数量
                    //if (_Enport == 1)
                    {
                        Write_PLC_Data(_Plcline, 751, 20, tbufferValue);
                    }

                    tSystem.mFile.WriteLog("", "钢化后测量复核 状态" + tValue[0].ToString() + " 长边 " + tValue[1] + "  测" + _Long.ToString() + " 短边 " + tValue[2]+" 测"+_Short  + " 流水号 " + tValue[3] + " 磨边方向 " + tValue[4] + " 理片 " + tValue[5] + " 片 " + tValue[6] + " 测量厚度 " + _Enport + " " + tRetData + "K");
                    return true;
                }
            }
            else if (tRetStr.Trim().Contains("钢化后核对未找到对于流水号数据" + _Status.ToString() + " " + _PlcFlow.ToString()) == true)
            {
                byte[] tbufferValue = new byte[16];
                int[] tValue = new int[4];

                tValue[0] = 11;
                S7.SetDIntAt(tbufferValue, 0 * 4, tValue[0]);

                tValue[1] = 0;
                S7.SetDIntAt(tbufferValue, 1 * 4, tValue[1]);
                tValue[2] = 0;
                S7.SetDIntAt(tbufferValue, 2 * 4, tValue[2]);
                tValue[3] = _PlcFlow;
                S7.SetDIntAt(tbufferValue, 3 * 4, tValue[3]);
                
                Write_PLC_Data(_Plcline, 751, 20, tbufferValue);
                
            }
            return false;
        }



        public class ClsMachineData
        {
            public int ID { get; set; }
            public string Machine_NO { get; set; }
            public int Plc_Flow { get; set; }
        }
        DateTime[] tEnportCmdtime = new DateTime[3] { DateTime.Now, DateTime.Now, DateTime.Now };
        int mEnportTag = 0;
        private bool ZKEnportWH(int _Plcline, int _Status, int _Long, int _Short, int _ExportMB, int[] _Arr)
        {
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();

            if (mEnportTag == 1 | DateTime.Now.Subtract(tEnportCmdtime[_ExportMB - 1]).TotalMilliseconds < 3000)
            {
                return false;
            }
            mEnportTag = 1;
            ClsDbAcc tClsDB = new ClsDbAcc(tSystem);
            string tRetData = "", tRetStr = ""; int tRows = 0;
            DataTable tDataTable = new DataTable(); List<ClsMachineData> tListData = new List<ClsMachineData>(); ClsMachineData tclsMachineData;
            for (int i = 0; i < _Arr.Length; i++)
            {
                tclsMachineData = new ClsMachineData();
                tclsMachineData.ID = i + 1; tclsMachineData.Machine_NO = ""; tclsMachineData.Plc_Flow = _Arr[i]; tListData.Add(tclsMachineData);
            }
            tDataTable = ToDataTable<ClsMachineData>(tListData);
            DataSet dataSet = new DataSet();
            dataSet = tClsDB.ExecuteProInLocationZK("Pro_30ZKInLocation", _Status, tDataTable, _Long, _Short, _ExportMB, ref tRetData, ref tRetStr, ref tRows);
            if (dataSet == null) { mEnportTag = 0; return false; }
            if (dataSet.Tables.Count > 0)
            {
                if (dataSet.Tables[dataSet.Tables.Count - 1].Rows.Count == 0) { mEnportTag = 0; return false; }
                if (dataSet.Tables[dataSet.Tables.Count - 1].Rows[0][0].ToString().ToUpper() == "Y" & dataSet.Tables[dataSet.Tables.Count - 1].Rows[0][2].ToString().ToUpper() == "中空入库指令")
                {
                    tRetData = dataSet.Tables[dataSet.Tables.Count - 1].Rows[0][1].ToString();
                    string[] tValues = tRetData.Split(',');
                    if (tValues.Length == 8)
                    {
                        byte[] tbufferValue = new byte[32];
                        int[] tValue = new int[8];

                        tValue[0] = _Status;
                        S7.SetDIntAt(tbufferValue, 0 * 4, tValue[0]);

                        tValue[1] = int.Parse(tValues[1]);// 流水号
                        if (tValue[1] != _Arr[0])
                        {
                            mEnportTag = 0;
                            return false;
                        }
                        S7.SetDIntAt(tbufferValue, 1 * 4, tValue[1]);

                        tValue[2] = int.Parse(tValues[4]);//仓库编号
                        S7.SetDIntAt(tbufferValue, 2 * 4, tValue[2]);

                        tValue[3] = int.Parse(tValues[5]);//格号
                        S7.SetDIntAt(tbufferValue, 3 * 4, tValue[3]);

                        tValue[4] = 1;
                        S7.SetDIntAt(tbufferValue, 4 * 4, tValue[4]);

                        tValue[5] = int.Parse(tValues[2]);
                        S7.SetDIntAt(tbufferValue, 5 * 4, tValue[5] * 1000);//长边

                        tValue[6] = int.Parse(tValues[3]);
                        S7.SetDIntAt(tbufferValue, 6 * 4, tValue[6] * 1000);//短边

                        tValue[7] = 1;
                        S7.SetDIntAt(tbufferValue, 7 * 4, tValue[7]);//玻璃标识 A B C

                        if (_ExportMB == 1)
                        {
                            Write_PLC_Data(_Plcline, 751, 136, tbufferValue);
                        }
                        else if (_ExportMB == 2)
                        {
                            Write_PLC_Data(_Plcline, 751, 312, tbufferValue);
                        }
                        else if (_ExportMB == 3)
                        {
                            Write_PLC_Data(_Plcline, 751, 352, tbufferValue);
                        }
                        tEnportCmdtime[_ExportMB - 1] = DateTime.Now;
                        stopwatch.Stop();

                        //tSystem.mFile.WriteLog("", string.Concat("中空入库 状态" + tValue[0].ToString(), " PLC流水号 ", tValue[1], " 库号 ", tValue[2], " 格号 ", tValue[3], " 订序 ", tValue[4], " 长边 ", tValue[5]
                        //          , " 长边 ", tValue[6], " 指令 " + tRetData, "L"));
                        tSystem.mFile.WriteLog("", "中空入库 状态" + tValue[0].ToString() + " 流水号 " + tValue[1] + " 库号 " + tValue[2] + " 格号 " + tValue[3] + " 订序 " + tValue[4] + " 长 " + tValue[5] + " 测" + _Long + " 短 " + tValue[6] + " 测" + _Short + " 线别" + _ExportMB
                           + " 有无 " + _Arr[6] + " 耗时" + stopwatch.Elapsed.TotalSeconds + "  " + tRetData + "L");
                        mEnportTag = 0;
                        return true;

                    }
                    else
                    {
                        stopwatch.Stop();
                        tSystem.mFile.WriteLog("", "中空入库 状态" + _Status + " 流水号 " + _Arr[0] + " 测" + _Long + " 短 "+" 测" + _Short + " 线别" + _ExportMB
                              + " 有无 " + _Arr[6] + " 耗时" + stopwatch.Elapsed.TotalSeconds + "  " + tRetData + "L");
                    }
                }
                else if (dataSet.Tables[dataSet.Tables.Count - 1].Rows[0][0].ToString().Contains("未找到库位") == true
                         & dataSet.Tables[dataSet.Tables.Count - 1].Rows[0][0].ToString().Contains(_Status.ToString() + " " + _Arr[0].ToString()) == true
                         & dataSet.Tables[dataSet.Tables.Count - 1].Rows[0][2].ToString().ToUpper() == "中空入库指令")
                {
                    byte[] tbufferValue = new byte[8];
                    int[] tValue = new int[2];

                    tValue[0] = 12;
                    S7.SetDIntAt(tbufferValue, 0 * 4, tValue[0]);

                    tValue[1] = _Arr[0];
                    S7.SetDIntAt(tbufferValue, 1 * 4, tValue[1]);
                    if (_ExportMB == 1)
                    {
                        Write_PLC_Data(_Plcline, 751, 136, tbufferValue);
                    }
                    else if (_ExportMB == 2)
                    {
                        Write_PLC_Data(_Plcline, 751, 312, tbufferValue);
                    }
                    else if (_ExportMB == 3)
                    {
                        Write_PLC_Data(_Plcline, 751, 352, tbufferValue);
                    }
                }
                else if (dataSet.Tables[dataSet.Tables.Count - 1].Rows[0][0].ToString().Contains(_Status.ToString() + " " + _Arr[0].ToString()) == true
                      & dataSet.Tables[dataSet.Tables.Count - 1].Rows[0][2].ToString().ToUpper() == "中空入库指令")
                {
                    byte[] tbufferValue = new byte[8];
                    int[] tValue = new int[2];

                    tValue[0] = 11;
                    S7.SetDIntAt(tbufferValue, 0 * 4, tValue[0]);

                    tValue[1] = _Arr[0];
                    S7.SetDIntAt(tbufferValue, 1 * 4, tValue[1]);
                    if (_ExportMB == 1)
                    {
                        Write_PLC_Data(_Plcline, 751, 136, tbufferValue);
                    }
                    else if (_ExportMB == 2)
                    {
                        Write_PLC_Data(_Plcline, 751, 312, tbufferValue);
                    }
                    else if (_ExportMB == 3)
                    {
                        Write_PLC_Data(_Plcline, 751, 352, tbufferValue);
                    }

                }
            }
            mEnportTag = 0;
            return false;

        }

        private bool ZKEnportWHFinish(int _Plcline, int _Status, int _PlcFlow,int _TroNo)
        {
            ClsDbAcc tClsDB = new ClsDbAcc(tSystem);
            string tRetData = "", tRetStr = ""; int tRows = 0;
            tClsDB.ExecutePro("Pro_31FinishCmdIn", 1, _PlcFlow.ToString(), _TroNo, 0, _TroNo, ref tRetData, ref tRetStr, ref tRows);

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

            if(_TroNo==1)
            { Write_PLC_Data(_Plcline, 751, 280, tbufferValue); }
            else
            { Write_PLC_Data(_Plcline, 751, 288, tbufferValue); }
            tSystem.mFile.WriteLog("", "中空入库完成 状态" + tValue[0].ToString() + " 流水号 " + tValue[1] + " 车号 " + _TroNo + "L");
            return true;
        }

        int MBOUTTag = 0;
        DateTime tOutCmdtime = DateTime.Now;
        private bool ZKWHoutCmd(int _Plcline, int _Status, int _Exit1, int Exit2, int Exit3,int _OutStatus)
        {
            if (MBOUTTag == 1 | DateTime.Now.Subtract(tOutCmdtime).TotalMilliseconds < 3000)
            {
                return false;
            }
            MBOUTTag = 1;
            ClsDbAcc tClsDB = new ClsDbAcc(tSystem);
            string tRetData = "", tRetStr = ""; int tRows = 0;
            DataTable tDataTable = new DataTable();
            string tStatusStr = _OutStatus.ToString().PadLeft(5, '0');

            List<ClsMachineData> tListData = new List<ClsMachineData>();
            tDataTable = ToDataTable<ClsMachineData>(tListData);
            DataSet dataSet = new DataSet();
            dataSet = tClsDB.ExecuteProInLocationZK("Pro_40ZKOutLocation", _Status, tDataTable, mZKOutCmdFlow, _OutStatus, Exit2, ref tRetData, ref tRetStr, ref tRows);
            if (dataSet == null) { MBOUTTag = 0; return false; }
            if (dataSet.Tables.Count > 0)
            {
                if (dataSet.Tables[dataSet.Tables.Count - 1].Rows.Count == 0) { MBOUTTag = 0; return false; }
                if (dataSet.Tables[dataSet.Tables.Count - 1].Rows[0][0].ToString().ToUpper() == "Y" & dataSet.Tables[dataSet.Tables.Count - 1].Rows[0][2].ToString().ToUpper() == "中空出库指令")
                {
                    tRetData = dataSet.Tables[dataSet.Tables.Count - 1].Rows[0][1].ToString();
                    string[] tValues = tRetData.Split(',');
                    if (tValues.Length == 15)
                    {
                        byte[] tbufferValue = new byte[52];
                        int[] tValue = new int[13];

                        tValue[0] = int.Parse(tValues[0]);//10整格出
                        S7.SetDIntAt(tbufferValue, 0 * 4, tValue[0]);

                        tValue[1] = int.Parse(tValues[1]);//仓库编号
                        S7.SetDIntAt(tbufferValue, 1 * 4, tValue[1]);

                        tValue[2] = int.Parse(tValues[2]);//库格编号
                        S7.SetDIntAt(tbufferValue, 2 * 4, tValue[2]);

                        tValue[3] = int.Parse(tValues[3]);//出几片
                        S7.SetDIntAt(tbufferValue, 3 * 4, tValue[3]);

                        tValue[4] = int.Parse(tValues[4]); ;//出口
                        S7.SetDIntAt(tbufferValue, 4 * 4, tValue[4]);

                        if ((tValue[1] == 8 & tValue[4] == 1) | (tValue[1] == 1 & tValue[4] == 3))
                        {
                            tValue[4] = 2; ;//出口
                            S7.SetDIntAt(tbufferValue, 4 * 4, tValue[4]);
                        }
                        mZKOutCmdFlow = mZKOutCmdFlow + 1;

                        if (mZKOutCmdFlow > 30000)
                        {
                            mZKOutCmdFlow = 1;
                        }

                        SettingsMC.Default.MBOutCmdFlow = mZKOutCmdFlow;
                        SettingsMC.Default.Save();
                        tOutCmdtime = DateTime.Now;
                        tValue[5] = int.Parse(tValues[5]);
                        S7.SetDIntAt(tbufferValue, 5 * 4, tValue[5]);

                        tValue[6] = int.Parse(tValues[6]);
                        S7.SetDIntAt(tbufferValue, 6 * 4, tValue[6]);

                        tValue[7] = int.Parse(tValues[7]);
                        S7.SetDIntAt(tbufferValue, 7 * 4, tValue[7]);

                        tValue[8] = int.Parse(tValues[8]);
                        S7.SetDIntAt(tbufferValue, 8 * 4, tValue[8]);

                        tValue[9] = int.Parse(tValues[9]);
                        S7.SetDIntAt(tbufferValue, 9 * 4, tValue[9]);

                        tValue[10] = int.Parse(tValues[10]);
                        S7.SetDIntAt(tbufferValue, 10 * 4, tValue[10]);

                        tValue[11] = int.Parse(tValues[11]);
                        S7.SetDIntAt(tbufferValue, 11 * 4, tValue[11]);

                        tValue[12] = int.Parse(tValues[12]);
                        S7.SetDIntAt(tbufferValue, 12 * 4, tValue[12]);

                        int tCarNO = 0;

                        if (tValues[14].Trim() == "1" & tStatusStr.Substring(0, 1) == "1")
                        {
                            Write_PLC_Data(_Plcline, 751, 176, tbufferValue);
                            tCarNO = 1;
                        }
                        else if (tValues[14].Trim() == "2" & tStatusStr.Substring(1, 1) == "1")
                        {
                            Write_PLC_Data(_Plcline, 751, 228, tbufferValue);
                            tCarNO = 2;
                        }
                        else
                        {
                            if (tValue[4] == 1 & tStatusStr.Substring(0, 1) == "1")
                            {
                                Write_PLC_Data(_Plcline, 751, 176, tbufferValue);
                                tCarNO = 11;
                            }
                            else if (tValue[4] == 3 & tStatusStr.Substring(1, 1) == "1")
                            {
                                Write_PLC_Data(_Plcline, 751, 228, tbufferValue);
                                tCarNO = 22;
                            }
                            else if (tValue[4] == 2 & tValue[1] < 5 & tStatusStr.Substring(0, 1) == "1")
                            {
                                Write_PLC_Data(_Plcline, 751, 176, tbufferValue);
                                tCarNO = 13;
                            }
                            else if (tValue[4] == 2 & tValue[1] > 4 & tStatusStr.Substring(1, 1) == "1")
                            {
                                Write_PLC_Data(_Plcline, 751, 228, tbufferValue);
                                tCarNO = 24;
                            }
                        }
                        //if (tValue[4] == 1 & tStatusStr.Substring(0, 1) == "1")
                        //{
                        //    Write_PLC_Data(_Plcline, 751, 176, tbufferValue);
                        //    tCarNO = 1;
                        //}
                        //else if (tValue[4] == 3 & tStatusStr.Substring(1, 1) == "1")
                        //{
                        //    Write_PLC_Data(_Plcline, 751, 228, tbufferValue);
                        //    tCarNO = 2;
                        //}
                        //else if (tValue[4] == 2 & tValue[1] < 5 & tStatusStr.Substring(0, 1) == "1")
                        //{
                        //    Write_PLC_Data(_Plcline, 751, 176, tbufferValue);
                        //    tCarNO = 1;
                        //}
                        //else if (tValue[4] == 2 & tValue[1] > 4 & tStatusStr.Substring(1, 1) == "1")
                        //{
                        //    Write_PLC_Data(_Plcline, 751, 228, tbufferValue);
                        //    tCarNO = 2;
                        //}
                        //else if (tValue[4] == 2 & tValue[1] < 8 & tStatusStr.Substring(0, 1) == "1")
                        //{
                        //    Write_PLC_Data(_Plcline, 751, 176, tbufferValue);
                        //    tCarNO = 1;
                        //}
                        //else if (tValue[4] == 2 & tValue[1] > 1 & tStatusStr.Substring(1, 1) == "1")
                        //{
                        //    Write_PLC_Data(_Plcline, 751, 228, tbufferValue);
                        //    tCarNO = 2;
                        //}

                        tSystem.mFile.WriteLog("", string.Concat("中空出库 状态" + tValue[0].ToString(), " 库号 ", tValue[1], " 格号 ", tValue[2], " 片数 ", tValue[3], " 出口 ", tValue[4]
                                 , " 指令编号 ", tValue[5], " 序号1 ", tValue[6], " 序号2 ", tValue[7], " 序号3 ", tValue[8], " 序号4 ", tValue[9]
                                  , " 下格 ", tValue[10], " 再下格 ", tValue[11], "  梭车号 ", tValue[12], "  成品单片ID ", tValues[13], " ", tStatusStr, " 车号" + tCarNO," ",tRetData, "M"));
                        MBOUTTag = 0;
                        return true;
                    }
                    else 
                    {
                        tSystem.mFile.WriteLog("", string.Concat("中空出库 错误 状态" + _Status.ToString() + tRetData, "I"));
                    }
                }
            }
            MBOUTTag = 0;
            return false;
        }

        private bool ZKWHoutFinish(int _Plcline, int _CmdFlow, int _Flow1, int _Flow2, int _Flow3, int _Flow4, int _CarNO)
        {
            ClsDbAcc tClsDB = new ClsDbAcc(tSystem);
            string tRetData = "", tRetStr = ""; int tRows = 0;
            tClsDB.ExecuteProFinishOut("Pro_41FinishCmdOut", 1, _CmdFlow, _Flow1, _Flow2, _Flow3, _Flow4,_CarNO, ref tRetData, ref tRetStr, ref tRows);
            if (tRetStr.ToUpper() != "Y")
            {
                return false;
            }
            byte[] tbufferValue = new byte[8];
            int[] tValue = new int[2];

            tValue[0] = 1;
            S7.SetDIntAt(tbufferValue, 0 * 4, tValue[0]);

            tValue[1] = _CmdFlow;
            S7.SetDIntAt(tbufferValue, 1 * 4, tValue[1]);
            if (_CarNO == 1)
            {
                Write_PLC_Data(_Plcline, 751, 296, tbufferValue);
            }
            else if (_CarNO == 2)
            {
                Write_PLC_Data(_Plcline, 751, 304, tbufferValue);
            }
            tSystem.mFile.WriteLog(" ", string.Concat("中空出库完成 指令编号 " + _CmdFlow.ToString(), "  PLC流水号1  ", _Flow1, "  PLC流水号2 ", _Flow2, " PLC流水号3 ", _Flow3, " PLC流水号4 ", _Flow4, " 车号 ", _CarNO, "M"));
            return false;
        }




        private bool Write_PLC_Data(int _Plcline, int _Add, int _Start, byte[] _bufferValue)
        {
            bool tR = true;
            try
            {
                int readResult = PlcClient[_Plcline].DBWrite(_Add, _Start, _bufferValue.Length, _bufferValue);
                if(readResult !=0)
                {
                    PlcClient[_Plcline].Disconnect();
                    Thread.Sleep(100);
                    PlcClient[_Plcline].ConnectTo(clsMyPublic.mPLCIPB, 0, 1);//// IP  机架号  插槽号 
                }
            }
            catch (Exception ex)
            {
               
                tR = false;
            }

            return tR;
        }


        public class ClsObj///主对象
        {
            public int Index { get; set; }
            public object obj { get; set; }
        }
        public class ClsPutLoad//上片
        {
            public int Enport { get; set; }
            public int mFlow { get; set; }
            public int mStatus { get; set; }
            public int Flow { get; set; }
            public int Status { get; set; }
        }

        public class ClsExit
        {
            public int Status { get; set; }
            public int Exit1 { get; set; }
            public int Exit2 { get; set; }
            public int Exit3 { get; set; }
        }
        public class ClsZKCheck//中空复核
        {

            public int Status { get; set; }
            public int Long { get; set; }
            public int Short { get; set; }
            public int MBNO { get; set; }
            public int[] FlowArr { get; set; }
        }
        public class ClsZKFinish//完成
        {
            public int TroNO{ get; set; }
            public int Flow { get; set; }
            public int Status { get; set; }
        }
        public class ClsZKOutFinish
        {
            public int CarNO { get; set; }
            public int Status { get; set; }
            public int OutCmd { get; set; }
            public int Flow1 { get; set; }
            public int Flow2 { get; set; }
            public int Flow3 { get; set; }
            public int Flow4 { get; set; }
        }
        public class ClsEnport
        {
            public int Status { get; set; }
            public int Long { get; set; }
            public int Short { get; set; }
            public int MBNO { get; set; }
            public int[] FlowArr { get; set; }
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
