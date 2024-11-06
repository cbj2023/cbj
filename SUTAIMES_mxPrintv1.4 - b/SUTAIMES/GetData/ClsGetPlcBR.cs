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
    class ClsGetPlcBR
    {
        Program tSystem;
        Thread mThreadGetPlcData;
        public bool mPlcStartGet = false;

        public S7Client[] PlcClient;

        public int mZKOutCmdFlow = 0;

        Common.ClsPrintDocument[] tAutoClsPrintDocument=new Common.ClsPrintDocument[3] ;
        public ClsGetPlcBR(Program tSys)
        {
            tSystem = tSys;
            mZKOutCmdFlow = SettingsMC.Default.ZKOutCmdFlow;

            tAutoClsPrintDocument[0] = new ClsPrintDocument(clsMyPublic.autoPrintName[0]);
            tAutoClsPrintDocument[1] = new ClsPrintDocument(clsMyPublic.autoPrintName[1]);
            tAutoClsPrintDocument[2] = new ClsPrintDocument(clsMyPublic.autoPrintName[2]);
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
        int[] MCPutUPloadFlow = new int[3] { 0, 0, 0 };
        int ZKCheckFlow = 0, ZKInFinishFlow1 = 0, ZKInFinishFlow2 = 0, ZKExportFlow = 0, ZKOutFinishFlow1 = 0, ZKOutFinishFlow2 = 0, ZKXiaFlow1 = 0, ZKXiaFlow2 = 0;
        int ZKEnportFlow1 = 0, ZKEnportFlow2 = 0, ZKEnportFlow3 = 0; int[] ZKOutFinishFlow = new int[2] { 0, 0 }; int[] ZKOutFinishFlowExit = new int[3] { 0, 0, 0 };
        private void GetPlcData()//5
        {
            int[] tPlcFlow = new int[35] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

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

                        ////////List<ClsMachineData> tListData = new List<ClsMachineData>(); ClsMachineData tclsMachineData; int tPlcTag = 0;
                        ////////if (tValue[0] > 0)//钢化出数据
                        ////////{
                        ////////    for (int j = 1; j <= 5; j++)
                        ////////    {
                        ////////        if (tFruNOFlow[j - 1] != tValue[j - 1])//65
                        ////////        {
                        ////////            tPlcTag = 1;
                        ////////            tFruNOFlow[j - 1] = tValue[j - 1];
                        ////////            tclsMachineData = new ClsMachineData(); tclsMachineData.ID = j; tclsMachineData.Plc_Flow = tValue[j - 1]; tListData.Add(tclsMachineData);
                        ////////        }
                        ////////    }
                        ////////}




                        if (tValue[0] > 0 & tValue[1] > 0 & MCPutUPloadFlow[0] != tValue[0])//上片 ok  1号上片 连续炉出片上片
                        {
                            MCPutUPloadFlow[0] = tValue[0];
                            ClsObj clsObj1 = new ClsObj();
                            ClsPutLoad clsPutLoad = new ClsPutLoad();
                            clsObj1.Index = 0;//上片
                            clsPutLoad.Enport = 1;
                            clsPutLoad.Flow = tValue[0];
                            clsPutLoad.Status = tValue[1];
                            clsObj1.obj = clsPutLoad;
                            Thread thread1 = new Thread(new ParameterizedThreadStart(Dispose));
                            thread1.Start(clsObj1);
                        }


                        //////////if (tValue[137] > 0 & tValue[138] > 0 & MCPutUPloadFlow[1] != tValue[137])//上片 ok  2号上片 连续炉出片上片
                        //////////{
                        //////////    MCPutUPloadFlow[1] = tValue[137];
                        //////////    ClsObj clsObj1 = new ClsObj();
                        //////////    ClsPutLoad clsPutLoad = new ClsPutLoad();
                        //////////    clsObj1.Index = 0;//上片
                        //////////    clsPutLoad.Enport = 2;
                        //////////    clsPutLoad.Flow = tValue[137];
                        //////////    clsPutLoad.Status = tValue[138];
                        //////////    clsObj1.obj = clsPutLoad;
                        //////////    Thread thread1 = new Thread(new ParameterizedThreadStart(Dispose));
                        //////////    thread1.Start(clsObj1);
                        //////////}



                        if (tValue[21] > 0 & tValue[22] > 0 & ZKInFinishFlow1 != tValue[22])//入库完成1
                        {
                            ZKInFinishFlow1 = tValue[22];
                            ClsObj clsObj341 = new ClsObj();
                            ClsZKFinish clsZKFinishcls341 = new ClsZKFinish();
                            clsObj341.Index = 34;//中空库完成
                            clsZKFinishcls341.TroNO = 1;
                            clsZKFinishcls341.Flow = tValue[22];
                            clsZKFinishcls341.Status = tValue[21];
                            clsObj341.obj = clsZKFinishcls341;
                            Thread thread1 = new Thread(new ParameterizedThreadStart(Dispose));
                            thread1.Start(clsObj341);
                        }

                        if (tValue[42] > 0 & tValue[43] > 0 & ZKInFinishFlow2 != tValue[43])//入库完成2
                        {
                            ZKInFinishFlow2 = tValue[43];
                            ClsObj clsObj342 = new ClsObj();
                            ClsZKFinish clsZKFinishcls342 = new ClsZKFinish();
                            clsObj342.Index = 34;//中空库完成
                            clsZKFinishcls342.TroNO = 2;
                            clsZKFinishcls342.Flow = tValue[43];
                            clsZKFinishcls342.Status = tValue[42];
                            clsObj342.obj = clsZKFinishcls342;
                            Thread thread1 = new Thread(new ParameterizedThreadStart(Dispose));
                            thread1.Start(clsObj342);
                        }


                        if (tValue[98] > 0 & tValue[99] > 0 & ZKOutFinishFlow[0] != tValue[99])//中空仓储索车出库完成1
                        {
                            ZKOutFinishFlow[0] = tValue[99];
                            ClsObj clsObj44 = new ClsObj();
                            clsObj44.Index = 44;//出库完成
                            ClsZKOutFinish clsOutFinish44 = new ClsZKOutFinish();
                            clsOutFinish44.CarNO = 1;
                            clsOutFinish44.Status = tValue[98];
                            clsOutFinish44.OutCmd = tValue[99];

                            clsOutFinish44.Flow1 = tValue[100];
                            clsOutFinish44.Flow2 = tValue[101];
                            clsOutFinish44.Flow3 = tValue[102];
                            clsOutFinish44.Flow4 = tValue[103];

                            clsObj44.obj = clsOutFinish44;
                            Thread threadC = new Thread(new ParameterizedThreadStart(Dispose));
                            threadC.Start(clsObj44);
                        }

                        ////////////////if (tValue[110] > 0 & tValue[111] > 0 & ZKOutFinishFlow[1] != tValue[111])//中空仓储索车出库完成2
                        ////////////////{
                        ////////////////    ZKOutFinishFlow[1] = tValue[111];
                        ////////////////    ClsObj clsObj44 = new ClsObj();
                        ////////////////    clsObj44.Index = 44;//出库完成
                        ////////////////    ClsZKOutFinish clsOutFinish44 = new ClsZKOutFinish();
                        ////////////////    clsOutFinish44.CarNO = 2;
                        ////////////////    clsOutFinish44.Status = tValue[110];
                        ////////////////    clsOutFinish44.OutCmd = tValue[111];

                        ////////////////    clsOutFinish44.Flow1 = tValue[112];
                        ////////////////    clsOutFinish44.Flow2 = tValue[113];
                        ////////////////    clsOutFinish44.Flow3 = tValue[114];
                        ////////////////    clsOutFinish44.Flow4 = tValue[115];

                        ////////////////    clsObj44.obj = clsOutFinish44;
                        ////////////////    Thread threadC = new Thread(new ParameterizedThreadStart(Dispose));
                        ////////////////    threadC.Start(clsObj44);
                        ////////////////}

                        if (tValue[62] > 0 & tValue[63] > 0 & ZKOutFinishFlowExit[0] != tValue[63])//中空仓储出库完成出口1
                        {
                            ZKOutFinishFlowExit[0] = tValue[63];
                            ClsObj clsObj441 = new ClsObj();
                            clsObj441.Index = 441;//出库完成
                            ClsZKOutFinish clsOutFinish441 = new ClsZKOutFinish();
                            clsOutFinish441.CarNO = 1;
                            clsOutFinish441.Status = tValue[62];
                            clsOutFinish441.OutCmd = tValue[63];

                            clsOutFinish441.Flow1 = tValue[64];
                            clsOutFinish441.Flow2 = tValue[65];
                            clsOutFinish441.Flow3 = tValue[66];
                            clsOutFinish441.Flow4 = tValue[67];

                            clsObj441.obj = clsOutFinish441;
                            Thread threadC = new Thread(new ParameterizedThreadStart(Dispose));
                            threadC.Start(clsObj441);
                        }
                        if (tValue[74] > 0 & tValue[75] > 0 & ZKOutFinishFlowExit[1] != tValue[75])//中空仓储出库完成出口2
                        {
                            ZKOutFinishFlowExit[1] = tValue[75];
                            ClsObj clsObj442 = new ClsObj();
                            clsObj442.Index = 441;//出库完成
                            ClsZKOutFinish clsOutFinish442 = new ClsZKOutFinish();
                            clsOutFinish442.CarNO = 2;
                            clsOutFinish442.Status = tValue[74];
                            clsOutFinish442.OutCmd = tValue[75];

                            clsOutFinish442.Flow1 = tValue[76];
                            clsOutFinish442.Flow2 = tValue[77];
                            clsOutFinish442.Flow3 = tValue[78];
                            clsOutFinish442.Flow4 = tValue[79];

                            clsObj442.obj = clsOutFinish442;
                            Thread threadC = new Thread(new ParameterizedThreadStart(Dispose));
                            threadC.Start(clsObj442);
                        }

                        if (tValue[86] > 0 & tValue[87] > 0 & ZKOutFinishFlowExit[2] != tValue[87])//中空仓储出库完成出口3
                        {
                            ZKOutFinishFlowExit[1] = tValue[87];
                            ClsObj clsObj443 = new ClsObj();
                            clsObj443.Index = 441;//出库完成
                            ClsZKOutFinish clsOutFinish443 = new ClsZKOutFinish();
                            clsOutFinish443.CarNO = 3;
                            clsOutFinish443.Status = tValue[86];
                            clsOutFinish443.OutCmd = tValue[87];

                            clsOutFinish443.Flow1 = tValue[88];
                            clsOutFinish443.Flow2 = tValue[89];
                            clsOutFinish443.Flow3 = tValue[90];
                            clsOutFinish443.Flow4 = tValue[91];

                            clsObj443.obj = clsOutFinish443;
                            Thread threadC = new Thread(new ParameterizedThreadStart(Dispose));
                            threadC.Start(clsObj443);
                        }


                        if (tValue[6] > 0 & tValue[7] > 0 & tValue[13] > 0 & tValue[14] > 0 & ZKEnportFlow1 != tValue[7])//入库中空库前测量段1 入库口分配
                        {
                            ZKEnportFlow1 = tValue[7];
                            ClsObj clsObj30 = new ClsObj();
                            ClsEnport clsEnport30 = new ClsEnport();
                            clsObj30.Index = 30;//入库口
                            clsEnport30.Status = tValue[6];
                            clsEnport30.MBNO = 1;// tValue[26];
                            clsEnport30.Long = tValue[13];
                            clsEnport30.Short = tValue[14];
                            int[] tFlowArr = new int[8];
                            tFlowArr[0] = tValue[7]; tFlowArr[1] = tValue[8]; tFlowArr[2] = tValue[9]; tFlowArr[3] = tValue[10];
                            tFlowArr[4] = tValue[11]; tFlowArr[5] = tValue[12]; tFlowArr[6] = tValue[15]; tFlowArr[7] = tValue[16];
                            clsEnport30.FlowArr = tFlowArr;
                            clsObj30.obj = clsEnport30;
                            Thread thread30 = new Thread(new ParameterizedThreadStart(Dispose));
                            thread30.Start(clsObj30);
                        }
                        if (tValue[27] > 0 & tValue[28] > 0 & tValue[34] > 0 & tValue[35] > 0 & ZKEnportFlow2 != tValue[28])//入库中空库前测量段2 入库口分配
                        {
                            ZKEnportFlow2 = tValue[28];
                            ClsObj clsObj30 = new ClsObj();
                            ClsEnport clsEnport30 = new ClsEnport();
                            clsObj30.Index = 30;//入库口
                            clsEnport30.Status = tValue[27];
                            clsEnport30.MBNO = 2;// tValue[26];
                            clsEnport30.Long = tValue[34];
                            clsEnport30.Short = tValue[35];
                            int[] tFlowArr = new int[8];
                            tFlowArr[0] = tValue[28]; tFlowArr[1] = tValue[29]; tFlowArr[2] = tValue[30]; tFlowArr[3] = tValue[31];
                            tFlowArr[4] = tValue[32]; tFlowArr[5] = tValue[33]; tFlowArr[6] = tValue[36]; tFlowArr[7] = tValue[37];
                            clsEnport30.FlowArr = tFlowArr;
                            clsObj30.obj = clsEnport30;
                            Thread thread30 = new Thread(new ParameterizedThreadStart(Dispose));
                            thread30.Start(clsObj30);
                        }
                        ////////////////if (tValue[122] > 0 & tValue[123] > 0 & tValue[129] > 0 & tValue[130] > 0 & ZKEnportFlow3 != tValue[123])//入库中空库前测量段3 入库口分配
                        ////////////////{
                        ////////////////    ZKEnportFlow3 = tValue[123];
                        ////////////////    ClsObj clsObj30 = new ClsObj();
                        ////////////////    ClsEnport clsEnport30 = new ClsEnport();
                        ////////////////    clsObj30.Index = 30;//入库口
                        ////////////////    clsEnport30.Status = tValue[27];
                        ////////////////    clsEnport30.MBNO = 3;// tValue[26];
                        ////////////////    clsEnport30.Long = tValue[129];
                        ////////////////    clsEnport30.Short = tValue[130];
                        ////////////////    int[] tFlowArr = new int[8];
                        ////////////////    tFlowArr[0] = tValue[123]; tFlowArr[1] = tValue[124]; tFlowArr[2] = tValue[125]; tFlowArr[3] = tValue[126];
                        ////////////////    tFlowArr[4] = tValue[127]; tFlowArr[5] = tValue[128]; tFlowArr[6] = tValue[131]; tFlowArr[7] = tValue[132];
                        ////////////////    clsEnport30.FlowArr = tFlowArr;
                        ////////////////    clsObj30.obj = clsEnport30;
                        ////////////////    Thread thread30 = new Thread(new ParameterizedThreadStart(Dispose));
                        ////////////////    thread30.Start(clsObj30);
                        ////////////////}

                        if ((tValue[54] > 0 | tValue[55] > 0) & ZKExportFlow == 0 & (tValue[56] > 0 | tValue[57] > 0 | tValue[58] > 0))//中空仓储出库  ClsZKOutFinish
                        {
                            ZKExportFlow = 1;
                            ClsObj clsObj40 = new ClsObj();
                            ClsExit tclsExit = new ClsExit();
                            clsObj40.Index = 40;//中空仓储出库

                            tclsExit.Status = tValue[54] * 10000 + tValue[55] * 1000 + tValue[56] * 100 + tValue[57] * 10 + tValue[58] * 1;//tValue[35]
                            tclsExit.Exit1 = tValue[56];
                            tclsExit.Exit2 = tValue[57];
                            tclsExit.Exit3 = tValue[58];

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


                        //if (tPlcTag == 1)
                        //{

                        //    ClsObj clsObj112 = new ClsObj();
                        //    clsObj112.Index = 112;//获取PLC流水号
                        //    clsObj112.obj = tListData;
                        //    Thread thread111 = new Thread(new ParameterizedThreadStart(Dispose));
                        //    thread111.Start(clsObj112);
                        //}

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
                case 0://双式
                    ClsPutLoad tClsPutLoad0 = new ClsPutLoad();
                    tClsPutLoad0 = (ClsPutLoad)tobj.obj;
                    PutLoad(1, 1, tClsPutLoad0);
                    MCPutUPloadFlow[tClsPutLoad0.Enport - 1] = 0;
                    break;
                case 1://双式  设置旗滨
                    ClsPutLoad tClsPutLoad = new ClsPutLoad();
                    tClsPutLoad = (ClsPutLoad)tobj.obj;
                    PutLoad(1, 2, tClsPutLoad);
                    MCPutUPloadFlow[tClsPutLoad.Enport -1] = 0;
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
                    MCCheck(1, clsZKCheck28.Status, clsZKCheck28.FlowArr[0], clsZKCheck28.Long, clsZKCheck28.Short, clsZKCheck28.MBNO);
                    ZKCheckFlow = 0;
                    break;
                case 30:
                    ClsEnport clsZKCheck30 = new ClsEnport();
                    clsZKCheck30 = (ClsEnport)tobj.obj;
                    ZKEnportWH(2, clsZKCheck30.Status, clsZKCheck30.Long, clsZKCheck30.Short, clsZKCheck30.MBNO, clsZKCheck30.FlowArr);
                    if (clsZKCheck30.MBNO == 1)
                    {
                        ZKEnportFlow1 = 0;
                    }
                    else if (clsZKCheck30.MBNO == 2)
                    {
                        ZKEnportFlow2 = 0;
                    }
                    else if (clsZKCheck30.MBNO == 3)
                    {
                        ZKEnportFlow3 = 0;
                    }
                    break;
                case 34:
                    ClsZKFinish clsZKFinishcls34 = new ClsZKFinish();
                    clsZKFinishcls34 = (ClsZKFinish)tobj.obj;
                    ZKEnportWHFinish(3, clsZKFinishcls34.Status, clsZKFinishcls34.Flow, clsZKFinishcls34.TroNO);
                    if (clsZKFinishcls34.TroNO == 1)
                    { ZKInFinishFlow1 = 0; }
                    else
                    { ZKInFinishFlow2 = 0; }

                    break;
                case 40:
                    ClsExit tclsExit = new ClsExit();
                    tclsExit = (ClsExit)tobj.obj;
                    ZKWHoutCmd(4, 1, tclsExit.Exit1, tclsExit.Exit2, tclsExit.Exit3, tclsExit.Status);
                    ZKExportFlow = 0;
                    break;
                case 44://索车完成
                    ClsZKOutFinish clsOutFinish44 = new ClsZKOutFinish();
                    clsOutFinish44 = (ClsZKOutFinish)tobj.obj;
                    ZKWHoutFinish(5, clsOutFinish44.OutCmd, clsOutFinish44.Flow1, clsOutFinish44.Flow2, clsOutFinish44.Flow3, clsOutFinish44.Flow4, clsOutFinish44.CarNO);
                    ZKOutFinishFlow[clsOutFinish44.CarNO - 1] = 0;
                    break;
                case 441://出口完成
                    ClsZKOutFinish clsOutFinish441 = new ClsZKOutFinish();
                    clsOutFinish441 = (ClsZKOutFinish)tobj.obj;
                    ZKWHoutFinishExit(6, clsOutFinish441.OutCmd, clsOutFinish441.Flow1, clsOutFinish441.Flow2, clsOutFinish441.Flow3, clsOutFinish441.Flow4, clsOutFinish441.CarNO);
                    ZKOutFinishFlowExit[clsOutFinish441.CarNO - 1] = 0;
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
                    { ZKXiaFlow1 = 0; }
                    else
                    { ZKXiaFlow1 = 0; }

                    break;

            }
        }

        private bool PutLoad(int _Plcline, int _SysType, ClsPutLoad _ClsPutLoad)
        {
            try
            {
                if (_SysType == 1)
                {

                    byte[] tbufferValue = new byte[24];
                    int[] tValue = new int[6];


                    tValue[0] = 2;//状态
                    S7.SetDIntAt(tbufferValue, 0 * 4, tValue[0]);

                    tValue[1] = 0;//1000  长边
                    S7.SetDIntAt(tbufferValue, 1 * 4, tValue[1]);

                    tValue[2] = 0;//1000  短边
                    S7.SetDIntAt(tbufferValue, 2 * 4, tValue[2]);


                    tValue[3] = _ClsPutLoad.Flow;//流水号
                    if (tValue[3] != _ClsPutLoad.Flow) { return false; }
                    S7.SetDIntAt(tbufferValue, 3 * 4, tValue[3]);

                    tValue[4] = 0; ;//长磨边量
                    S7.SetDIntAt(tbufferValue, 4 * 4, tValue[4]);

                    tValue[5] = 0; ;//短磨边量
                    S7.SetDIntAt(tbufferValue, 5 * 4, tValue[5]);
                    if (_ClsPutLoad.Enport == 1)
                    {
                        Write_PLC_Data(_Plcline, 751, 0, tbufferValue);
                    }
                    else if (_ClsPutLoad.Enport == 2)
                    {
                        Write_PLC_Data(_Plcline, 751, 1332, tbufferValue);
                    }

                    return true;
                }
                else
                {

                    ClsDbAcc tClsDB = new ClsDbAcc(tSystem);
                    string tRetData = "", tRetStr = "Y"; int tRows = 0;
                    tClsDB.ExecutePro("Pro_29MBStart", _ClsPutLoad.mStatus, _ClsPutLoad.mFlow.ToString(), _ClsPutLoad.Status, _ClsPutLoad.Flow, _ClsPutLoad.Enport, ref tRetData, ref tRetStr, ref tRows);
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
                            if (_ClsPutLoad.Enport == 1)
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

        private bool GHxiaPian(int _Plcline, int _Status, int _PlcFlow, int _TroNo)
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

                    tSystem.mFile.WriteLog("", "钢化后测量复核 状态" + tValue[0].ToString() + " 长边 " + tValue[1] + "  测" + _Long.ToString() + " 短边 " + tValue[2] + " 测" + _Short + " 流水号 " + tValue[3] + " 磨边方向 " + tValue[4] + " 理片 " + tValue[5] + " 片 " + tValue[6] + " 测量厚度 " + _Enport + " " + tRetData + "K");
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



                        tValue[4] = (int)(float.Parse(tValues[2]) * 1000);
                        S7.SetDIntAt(tbufferValue, 4 * 4, tValue[4]);//长边

                        tValue[5] = (int)(float.Parse(tValues[3]) * 1000);
                        S7.SetDIntAt(tbufferValue, 5 * 4, tValue[5]);//短边

                        tValue[6] = 5;
                        S7.SetDIntAt(tbufferValue, 6 * 4, tValue[6]);

                        tValue[7] = 1;
                        S7.SetDIntAt(tbufferValue, 7 * 4, tValue[7]);//玻璃标识 A B C

                        if (_ExportMB == 1)
                        {
                            Write_PLC_Data(_Plcline, 751, 24, tbufferValue);
                        }
                        else if (_ExportMB == 2)
                        {
                            Write_PLC_Data(_Plcline, 751, 108, tbufferValue);
                        }
                        else if (_ExportMB == 3)
                        {
                            Write_PLC_Data(_Plcline, 751, 1272, tbufferValue);
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
                        tSystem.mFile.WriteLog("", "中空入库 状态" + _Status + " 流水号 " + _Arr[0] + " 测" + _Long + " 短 " + " 测" + _Short + " 线别" + _ExportMB
                              + " 有无 " + _Arr[6] + " 耗时" + stopwatch.Elapsed.TotalSeconds + "  " + tRetData + "L");
                    }
                }
                else if (dataSet.Tables[dataSet.Tables.Count - 1].Rows[0][0].ToString().Contains("未找到库位") == true
                         & dataSet.Tables[dataSet.Tables.Count - 1].Rows[0][0].ToString().Contains(_Arr[0].ToString()) == true
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
                        Write_PLC_Data(_Plcline, 751, 24, tbufferValue);
                    }
                    else if (_ExportMB == 2)
                    {
                        Write_PLC_Data(_Plcline, 751, 108, tbufferValue);
                    }
                    else if (_ExportMB == 3)
                    {
                        Write_PLC_Data(_Plcline, 751, 1272, tbufferValue);
                    }
                }
                else if (dataSet.Tables[dataSet.Tables.Count - 1].Rows[0][0].ToString().Contains(_Arr[0].ToString()) == true
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
                        Write_PLC_Data(_Plcline, 751, 24, tbufferValue);
                    }
                    else if (_ExportMB == 2)
                    {
                        Write_PLC_Data(_Plcline, 751, 108, tbufferValue);
                    }
                    else if (_ExportMB == 3)
                    {
                        Write_PLC_Data(_Plcline, 751, 1272, tbufferValue);
                    }

                }
            }
            mEnportTag = 0;
            return false;

        }

        private bool ZKEnportWHFinish(int _Plcline, int _Status, int _PlcFlow, int _TroNo)
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

            if (_TroNo == 1)
            { Write_PLC_Data(_Plcline, 751, 84, tbufferValue); }
            else
            { Write_PLC_Data(_Plcline, 751, 168, tbufferValue); }
            tSystem.mFile.WriteLog("", "中空入库完成 状态" + tValue[0].ToString() + " 流水号 " + tValue[1] + " 车号 " + _TroNo + "L");
            return true;
        }

        int MBOUTTag = 0;
        DateTime tOutCmdtime = DateTime.Now;
        //using System.Text.RegularExpressions;
        private bool ZKWHoutCmd(int _Plcline, int _Status, int _Exit1, int Exit2, int Exit3, int _OutStatus)
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
                    string[] tCmdPData = System.Text.RegularExpressions.Regex.Split(tRetData, "@@@"); // tRetData.Split('@');
                    string[] tCmdStr = tCmdPData[0].Split('#');
                    if (tCmdStr.Length == 0) { return false; }
                    string[] tValues = tCmdStr[0].Split(',');
                    int tExit_to = 0;
                    if (tValues.Length >= 14)////=15
                    {
                        byte[] tbufferValue = new byte[56 * 3];
                        int[] tValue = new int[14];

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

                        //if ( (tValue[1] == 1 & tValue[4] == 3))
                        //{
                        //    tValue[4] = 2; ;//出口
                        //    S7.SetDIntAt(tbufferValue, 4 * 4, tValue[4]);
                        //}
                        mZKOutCmdFlow = mZKOutCmdFlow + 1;

                        if (mZKOutCmdFlow > 30000)
                        {
                            mZKOutCmdFlow = 1;
                        }

                        SettingsMC.Default.MBOutCmdFlow = mZKOutCmdFlow;
                        SettingsMC.Default.Save();
                        tExit_to = tValue[4];
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

                        tValue[13] = 0;
                        S7.SetDIntAt(tbufferValue, 13 * 4, tValue[13]);//

                        if (tCmdStr.Length > 1)
                        {
                            string[] tValues2 = tCmdStr[1].Split(',');
                            tValue[0] = int.Parse(tValues2[0]);//10整格出
                            S7.SetDIntAt(tbufferValue, 14 * 4, tValue[0]);

                            tValue[1] = int.Parse(tValues2[1]);//仓库编号
                            S7.SetDIntAt(tbufferValue, 15 * 4, tValue[1]);

                            tValue[2] = int.Parse(tValues2[2]);//库格编号
                            S7.SetDIntAt(tbufferValue, 16 * 4, tValue[2]);

                            tValue[3] = int.Parse(tValues2[3]);//出几片
                            S7.SetDIntAt(tbufferValue, 17 * 4, tValue[3]);

                            tValue[4] = int.Parse(tValues2[4]); ;//出口
                            S7.SetDIntAt(tbufferValue, 18 * 4, tValue[4]);

                            mZKOutCmdFlow = mZKOutCmdFlow + 1;

                            if (mZKOutCmdFlow > 30000)
                            {
                                mZKOutCmdFlow = 1;
                            }

                            SettingsMC.Default.MBOutCmdFlow = mZKOutCmdFlow;
                            SettingsMC.Default.Save();
                            tOutCmdtime = DateTime.Now;
                            tValue[5] = int.Parse(tValues2[5]);
                            S7.SetDIntAt(tbufferValue, 19 * 4, tValue[5]);

                            tValue[6] = int.Parse(tValues2[6]);
                            S7.SetDIntAt(tbufferValue, 20 * 4, tValue[6]);

                            tValue[7] = int.Parse(tValues2[7]);
                            S7.SetDIntAt(tbufferValue, 21 * 4, tValue[7]);

                            tValue[8] = int.Parse(tValues2[8]);
                            S7.SetDIntAt(tbufferValue, 22 * 4, tValue[8]);

                            tValue[9] = int.Parse(tValues2[9]);
                            S7.SetDIntAt(tbufferValue, 23 * 4, tValue[9]);

                            tValue[10] = int.Parse(tValues2[10]);
                            S7.SetDIntAt(tbufferValue, 24 * 4, tValue[10]);

                            tValue[11] = int.Parse(tValues2[11]);
                            S7.SetDIntAt(tbufferValue, 25 * 4, tValue[11]);

                            tValue[12] = int.Parse(tValues2[12]);
                            S7.SetDIntAt(tbufferValue, 26 * 4, tValue[12]);

                            tValue[13] = 0;
                            S7.SetDIntAt(tbufferValue, 27 * 4, tValue[13]);//
                        }

                        if (tCmdStr.Length == 3)
                        {
                            string[] tValues3 = tCmdStr[1].Split(',');
                            tValue[0] = int.Parse(tValues3[0]);//10整格出
                            S7.SetDIntAt(tbufferValue, 28 * 4, tValue[0]);

                            tValue[1] = int.Parse(tValues3[1]);//仓库编号
                            S7.SetDIntAt(tbufferValue, 29 * 4, tValue[1]);

                            tValue[2] = int.Parse(tValues3[2]);//库格编号
                            S7.SetDIntAt(tbufferValue, 30 * 4, tValue[2]);

                            tValue[3] = int.Parse(tValues3[3]);//出几片
                            S7.SetDIntAt(tbufferValue, 31 * 4, tValue[3]);

                            tValue[4] = int.Parse(tValues3[4]); ;//出口
                            S7.SetDIntAt(tbufferValue, 32 * 4, tValue[4]);

                            mZKOutCmdFlow = mZKOutCmdFlow + 1;

                            if (mZKOutCmdFlow > 30000)
                            {
                                mZKOutCmdFlow = 1;
                            }

                            SettingsMC.Default.MBOutCmdFlow = mZKOutCmdFlow;
                            SettingsMC.Default.Save();
                            tOutCmdtime = DateTime.Now;
                            tValue[5] = int.Parse(tValues3[5]);
                            S7.SetDIntAt(tbufferValue, 33 * 4, tValue[5]);

                            tValue[6] = int.Parse(tValues3[6]);
                            S7.SetDIntAt(tbufferValue, 34 * 4, tValue[6]);

                            tValue[7] = int.Parse(tValues3[7]);
                            S7.SetDIntAt(tbufferValue, 35 * 4, tValue[7]);

                            tValue[8] = int.Parse(tValues3[8]);
                            S7.SetDIntAt(tbufferValue, 36 * 4, tValue[8]);

                            tValue[9] = int.Parse(tValues3[9]);
                            S7.SetDIntAt(tbufferValue, 37 * 4, tValue[9]);

                            tValue[10] = int.Parse(tValues3[10]);
                            S7.SetDIntAt(tbufferValue, 38 * 4, tValue[10]);

                            tValue[11] = int.Parse(tValues3[11]);
                            S7.SetDIntAt(tbufferValue, 39 * 4, tValue[11]);

                            tValue[12] = int.Parse(tValues3[12]);
                            S7.SetDIntAt(tbufferValue, 40 * 4, tValue[12]);

                            tValue[13] = 0;
                            S7.SetDIntAt(tbufferValue, 41 * 4, tValue[13]);//
                        }


                        int tCarNO = 0;
                        if (1 == 1)
                        {
                            if (tValues[14].Trim() == "1" & tStatusStr.Substring(0, 1) == "1")//索车
                            {
                                Write_PLC_Data(_Plcline, 751, 840, tbufferValue);
                                tCarNO = 1;
                            }
                            else if (tValues[14].Trim() == "2" & tStatusStr.Substring(1, 1) == "1")//索车
                            {
                                Write_PLC_Data(_Plcline, 751, 1008, tbufferValue);
                                tCarNO = 2;
                            }
                            else
                            {
                                if (tValue[4] == 1 & tStatusStr.Substring(0, 1) == "1")//索车
                                {
                                    Write_PLC_Data(_Plcline, 751, 840, tbufferValue);
                                    tCarNO = 11;
                                }
                                else if (tValue[4] == 3 & tStatusStr.Substring(1, 1) == "1")//索车
                                {
                                    Write_PLC_Data(_Plcline, 751, 1008, tbufferValue);
                                    tCarNO = 22;
                                }
                                else if (tValue[4] == 2 & tValue[1] < 5 & tStatusStr.Substring(0, 1) == "1")//索车
                                {
                                    Write_PLC_Data(_Plcline, 751, 840, tbufferValue);
                                    tCarNO = 13;
                                }
                                else if (tValue[4] == 2 & tValue[1] > 4 & tStatusStr.Substring(1, 1) == "1")//索车
                                {
                                    Write_PLC_Data(_Plcline, 751, 1008, tbufferValue);
                                    tCarNO = 24;
                                }
                            }
                        }
                        else  //出口指令下达模式
                        {
                            if (tExit_to == 1)
                            {
                                Write_PLC_Data(_Plcline, 751, 192, tbufferValue);
                            }
                            else if (tExit_to == 2)
                            {
                                Write_PLC_Data(_Plcline, 751, 360, tbufferValue);
                            }
                            else if (tExit_to == 3)
                            {
                                Write_PLC_Data(_Plcline, 751, 528, tbufferValue);

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

                        string[] tPrintData = new string[6]{"","","","","",""};
                        if (tCmdPData.Length > 1)//打印标签
                        {
                            
                            string[] tCmdPrint = System.Text.RegularExpressions.Regex.Split(tCmdPData[1], "@@");
                            if (tCmdPrint.Length > 3)
                            {
                                tPrintData[0] = tCmdPrint[0];
                                tPrintData[1] = tCmdPrint[1];
                                tPrintData[2] = tCmdPrint[2];
                                tPrintData[3] = tCmdPrint[3];
                            }
                            tPrintData[4] = clsMyPublic.autoPrintLine[tValue[4] - 1];
                            tPrintData[5] = clsMyPublic.autoPrintQC[tValue[4] - 1];
                            tAutoClsPrintDocument[tValue[4] - 1].MyAutoprinter(tPrintData);
                        }

                        tSystem.mFile.WriteLog("", string.Concat("中空出库 状态" + tValue[0].ToString(), " 库号 ", tValue[1], " 格号 ", tValue[2], " 片数 ", tValue[3], " 出口 ", tValue[4]
                                 , " 指令编号 ", tValue[5], " 序号1 ", tValue[6], " 序号2 ", tValue[7], " 序号3 ", tValue[8], " 序号4 ", tValue[9]
                                  , " 下格 ", tValue[10], " 再下格 ", tValue[11], "  梭车号 ", tValue[12], "  成品单片ID ", tValues[13], " ", tStatusStr, " 车号" + tCarNO, " ", tRetData, " ", string.Join(" ",tPrintData).Trim(), "M"));
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

        private bool ZKWHoutFinish(int _Plcline, int _CmdFlow, int _Flow1, int _Flow2, int _Flow3, int _Flow4, int _CarNO)//索车出库完成
        {
            ClsDbAcc tClsDB = new ClsDbAcc(tSystem);
            string tRetData = "", tRetStr = ""; int tRows = 0;
            tClsDB.ExecuteProFinishOut("Pro_41FinishCmdOut", 1, _CmdFlow, _Flow1, _Flow2, _Flow3, _Flow4, _CarNO, ref tRetData, ref tRetStr, ref tRows);
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
                Write_PLC_Data(_Plcline, 751, 1176, tbufferValue);
            }
            else if (_CarNO == 2)
            {
                Write_PLC_Data(_Plcline, 751, 1224, tbufferValue);
            }
            tSystem.mFile.WriteLog(" ", string.Concat("中空出库完成 指令编号 " + _CmdFlow.ToString(), "  PLC流水号1  ", _Flow1, "  PLC流水号2 ", _Flow2, " PLC流水号3 ", _Flow3, " PLC流水号4 ", _Flow4, " 车号 ", _CarNO, "M"));
            return false;
        }


        private bool ZKWHoutFinishExit(int _Plcline, int _CmdFlow, int _Flow1, int _Flow2, int _Flow3, int _Flow4, int _CarNO)//出口出库完成
        {
            ClsDbAcc tClsDB = new ClsDbAcc(tSystem);
            string tRetData = "", tRetStr = ""; int tRows = 0;
            tClsDB.ExecuteProFinishOut("Pro_41FinishCmdOutExit", 1, _CmdFlow, _Flow1, _Flow2, _Flow3, _Flow4, _CarNO, ref tRetData, ref tRetStr, ref tRows);
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
                Write_PLC_Data(_Plcline, 751, 696, tbufferValue);
            }
            else if (_CarNO == 2)
            {
                Write_PLC_Data(_Plcline, 751, 744, tbufferValue);
            }
            else if (_CarNO == 2)
            {
                Write_PLC_Data(_Plcline, 751, 792, tbufferValue);
            }
            tSystem.mFile.WriteLog(" ", string.Concat("中空出库出口完成 指令编号 " + _CmdFlow.ToString(), "  PLC流水号1  ", _Flow1, "  PLC流水号2 ", _Flow2, " PLC流水号3 ", _Flow3, " PLC流水号4 ", _Flow4, " 出口 ", _CarNO, "M"));
            return false;
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
            public int TroNO { get; set; }
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
