﻿using System;
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
    class ClsGetPlcA
    {
        Program tSystem;
        Thread mThreadGetPlcData;
        public bool mPlcStartGet = false;

        public S7Client[] PlcClient;


        public int mMBOutCmdFlow = 0;
        public ClsGetPlcA(Program tSys)
        {
            tSystem = tSys;
            mMBOutCmdFlow = SettingsMC.Default.MBOutCmdFlow;
            for (int i = 0; i < tEnportCmdtime.Length; i++)
            {
                tEnportCmdtime[i] = DateTime.Now;
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
                    PlcResult = PlcClient[i].ConnectTo(clsMyPublic.mPLCIPA, 0, 1);// IP  机架号  插槽号  ///192.168.1.10   192.168.11.40
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
        int MCPutUPloadFlow = 0; int MCLPoutFlow = 0; int MCCheckFlow = 0, MCEnportFlow1 = 0, MCEnportFlow2 = 0, MCEnportFlow3 = 0, MCExportFlow = 0;
        int MCLPOutFinishFlow = 0, MCInFinishFlow = 0, MCOutFinishFlow = 0, MCOutFinishFlow2 = 0;

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

                    byte[] tbufferValue = new byte[280];
                    int[] tValue = new int[70];
                    int readResult = PlcClient[0].DBRead(700, 0, tbufferValue.Length, tbufferValue);
                    if (readResult != 0)
                    {
                        PlcClient[0].Disconnect();
                        Thread.Sleep(100);
                        PlcClient[0].ConnectTo(clsMyPublic.mPLCIPA, 0, 1);//// IP  机架号  插槽号 
                    }
                    else
                    {
                        for (int i = 0; i < tValue.Length; i++)
                        {
                            tValue[i] = S7.GetDIntAt(tbufferValue, i * 4);//转换成数据
                        }
                        if (tValue[0] > 0 & tValue[1] > 0 & MCPutUPloadFlow != tValue[0])//上片 ok
                        {
                            MCPutUPloadFlow = tValue[0];
                            ClsObj clsObj1 = new ClsObj();
                            ClsPutLoad clsPutLoad = new ClsPutLoad();
                            clsObj1.Index = 1;//上片
                            clsPutLoad.Enport = 1;
                            clsPutLoad.Flow = tValue[0];
                            clsPutLoad.Status = tValue[1];
                            clsObj1.obj = clsPutLoad;
                            Thread thread1 = new Thread(new ParameterizedThreadStart(Dispose));
                            thread1.Start(clsObj1);
                        }


                        if (tValue[12] > 0 & MCLPoutFlow == 0)//理片机出片  理片出口空闲 ok
                        {
                            MCLPoutFlow = 1;
                            ClsObj clsObj3 = new ClsObj();
                            clsObj3.Index = 3;//理片机
                            clsObj3.obj = 1;
                            Thread thread3 = new Thread(new ParameterizedThreadStart(Dispose));
                            thread3.Start(clsObj3);

                        }

                        if (tValue[6] > 0 & tValue[7] > 0 & tValue[8] > 0 & tValue[9] > 0 & MCCheckFlow != tValue[9] & CHMBTag[0] == 0)//理片机前匹配 ok
                        {
                            MCCheckFlow = tValue[9];
                            ClsObj clsObj2 = new ClsObj();
                            ClsMBCheck clsMBCheck = new ClsMBCheck();
                            clsObj2.Index = 2;//数据核对
                            clsMBCheck.Enport = 1;
                            clsMBCheck.Status = tValue[6]; clsMBCheck.Long = tValue[7]; clsMBCheck.Short = tValue[8]; clsMBCheck.Flow = tValue[9]; clsMBCheck.LPbinStatus = tValue[10];
                            clsObj2.obj = clsMBCheck;
                            Thread thread2 = new Thread(new ParameterizedThreadStart(Dispose));
                            thread2.Start(clsObj2);
                        }

                        if (tValue[17] > 0 & tValue[18] > 0 & MCLPOutFinishFlow != tValue[18])//理片机出库完成 ok
                        {
                            MCLPOutFinishFlow = tValue[18];
                            ClsObj clsObj4 = new ClsObj();
                            ClsMCFinish clsMCFinishcls4 = new ClsMCFinish();
                            clsObj4.Index = 4;//理片机出库完成
                            clsMCFinishcls4.Enport = 1;
                            clsMCFinishcls4.Flow = tValue[18];
                            clsMCFinishcls4.Status = tValue[17];
                            clsObj4.obj = clsMCFinishcls4;
                            Thread thread1 = new Thread(new ParameterizedThreadStart(Dispose));
                            thread1.Start(clsObj4);
                        }

                        if (tValue[53] > 0 & tValue[54] > 0 & MCOutFinishFlow != tValue[54])//磨边仓储出库完成
                        {
                            MCOutFinishFlow = tValue[54];
                            ClsObj clsObj24 = new ClsObj();
                            clsObj24.Index = 24;//出库完成
                            ClsOutFinish ClsOutFinish24 = new ClsOutFinish();

                            ClsOutFinish24.CarNO = 1;
                            ClsOutFinish24.Status = tValue[53];
                            ClsOutFinish24.OutCmd = tValue[54];

                            ClsOutFinish24.Flow1 = tValue[55];
                            ClsOutFinish24.Flow2 = tValue[56];
                            ClsOutFinish24.Flow3 = tValue[57];
                            ClsOutFinish24.Flow4 = tValue[58];

                            clsObj24.obj = ClsOutFinish24;
                            Thread threadC = new Thread(new ParameterizedThreadStart(Dispose));
                            threadC.Start(clsObj24);
                        }

                        if (tValue[59] > 0 & tValue[60] > 0 & MCOutFinishFlow2 != tValue[60])//磨边仓储出库完成
                        {
                            MCOutFinishFlow2 = tValue[60];
                            ClsObj clsObj24 = new ClsObj();
                            clsObj24.Index = 24;//出库完成
                            ClsOutFinish ClsOutFinish24 = new ClsOutFinish();

                            ClsOutFinish24.CarNO = 2;
                            ClsOutFinish24.Status = tValue[59];
                            ClsOutFinish24.OutCmd = tValue[60];

                            ClsOutFinish24.Flow1 = tValue[61];
                            ClsOutFinish24.Flow2 = tValue[62];
                            ClsOutFinish24.Flow3 = tValue[63];
                            ClsOutFinish24.Flow4 = tValue[64];

                            clsObj24.obj = ClsOutFinish24;
                            Thread threadC = new Thread(new ParameterizedThreadStart(Dispose));
                            threadC.Start(clsObj24);
                        }

                        //else if (tValue[45] > 0)
                        //{
                        //    MCOutFinishFlow = tValue[46];
                        //    ClsObj clsObj24 = new ClsObj();
                        //    clsObj24.Index = 24;//出库完成
                        //    ClsOutFinish ClsOutFinish24 = new ClsOutFinish();
                        //    ClsOutFinish24.Status = tValue[45];
                        //    ClsOutFinish24.OutCmd = tValue[46];

                        //    ClsOutFinish24.Flow1 = tValue[47];
                        //    ClsOutFinish24.Flow2 = tValue[48];
                        //    ClsOutFinish24.Flow3 = tValue[49];
                        //    ClsOutFinish24.Flow4 = tValue[50];

                        //    clsObj24.obj = ClsOutFinish24;
                        //    Thread threadC = new Thread(new ParameterizedThreadStart(Dispose));
                        //    threadC.Start(clsObj24);
                        //}

                        if ((tValue[42] > 0 | tValue[43] > 0) & (tValue[44] > 0 | tValue[45] > 0 | tValue[46] > 0) & MCExportFlow == 0
                            //& 1 == 2
                            )//磨边仓储出库
                        {
                            MCExportFlow = 1;
                            ClsObj clsObj20 = new ClsObj();
                            clsObj20.Index = 20;//磨边仓储出库
                            clsObj20.obj = tValue[42] * 10000 + tValue[43] * 1000 + tValue[44] * 100 + tValue[45] * 10 + tValue[46] * 1;
                            Thread threadC = new Thread(new ParameterizedThreadStart(Dispose));
                            threadC.Start(clsObj20);
                        }

                        if (tValue[49] > 0 & tValue[50] > 0 & MCInFinishFlow != tValue[50])//1入库完成
                        {
                            MCInFinishFlow = tValue[50];
                            ClsObj clsObj14 = new ClsObj();
                            ClsMCFinish clsMCFinishcls14 = new ClsMCFinish();
                            clsObj14.Index = 14;//
                            clsMCFinishcls14.Enport = 1;
                            clsMCFinishcls14.Flow = tValue[50];
                            clsMCFinishcls14.Status = tValue[49];
                            clsObj14.obj = clsMCFinishcls14;
                            Thread thread1 = new Thread(new ParameterizedThreadStart(Dispose));
                            thread1.Start(clsObj14);
                        }
                        if (tValue[51] > 0 & tValue[52] > 0 & MCInFinishFlow != tValue[52])//2入库完成
                        {
                            MCInFinishFlow = tValue[52];
                            ClsObj clsObj14 = new ClsObj();
                            ClsMCFinish clsMCFinishcls14 = new ClsMCFinish();
                            clsObj14.Index = 14;//
                            clsMCFinishcls14.Enport = 2;
                            clsMCFinishcls14.Flow = tValue[52];
                            clsMCFinishcls14.Status = tValue[51];
                            clsObj14.obj = clsMCFinishcls14;
                            Thread thread1 = new Thread(new ParameterizedThreadStart(Dispose));
                            thread1.Start(clsObj14);
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


                        if (tValue[26] > 0 & tValue[31] > 0 & tValue[33] > 0 & tValue[34] > 0 & MCEnportFlow1 != tValue[31] & EnTag1 == 0)//入库
                        {
                            MCEnportFlow1 = tValue[31];
                            ClsObj clsObj10 = new ClsObj();
                            ClsEnport clsEnport10 = new ClsEnport();
                            clsObj10.Index = 10;//入库口
                            clsEnport10.Status = tValue[26];
                            clsEnport10.MBNO = 1;// tValue[26];
                            clsEnport10.Long = tValue[33];
                            clsEnport10.Short = tValue[34];
                            int[] tFlowArr = new int[7];
                            tFlowArr[0] = tValue[31]; tFlowArr[1] = tValue[32]; tFlowArr[2] = tValue[27]; tFlowArr[3] = tValue[28];
                            tFlowArr[4] = tValue[29]; tFlowArr[5] = tValue[30]; tFlowArr[6] = tValue[35];
                            clsEnport10.FlowArr = tFlowArr;
                            clsObj10.obj = clsEnport10;
                            Thread thread10 = new Thread(new ParameterizedThreadStart(Dispose));
                            thread10.Start(clsObj10);
                        }





                        List<ClsMachineData> tListData = new List<ClsMachineData>(); ClsMachineData tclsMachineData; int tPlcTag = 0;
                        for (int j = 1; j <= 5; j++)
                        {
                            if (tFruNOFlow[j - 1] != tValue[64 + j])//65
                            {
                                tPlcTag = 1;
                                tFruNOFlow[j - 1] = tValue[64 + j];
                                tclsMachineData = new ClsMachineData(); tclsMachineData.ID = j; tclsMachineData.Plc_Flow = tValue[64 + j]; tListData.Add(tclsMachineData);
                            }


                        }
                        //for (int j = 21; j <= 36; j++)
                        //{
                        //    if (tPlcFlow[j - 15] != tValue[34 + j])
                        //    {
                        //        tPlcTag = 1;
                        //        tPlcFlow[j - 15] = tValue[34 + j];
                        //        tclsMachineData = new ClsMachineData(); tclsMachineData.ID = j; tclsMachineData.Plc_Flow = tValue[34 + j]; tListData.Add(tclsMachineData);
                        //    }
                        //}
                        //if (tPlcFlow[22] != tValue[71])
                        //{
                        //    tPlcTag = 1;
                        //    tPlcFlow[22] = tValue[71];
                        //    tclsMachineData = new ClsMachineData(); tclsMachineData.ID = 999; tclsMachineData.Plc_Flow = tValue[71]; tListData.Add(tclsMachineData);
                        //}

                        if (tPlcTag == 1)
                        {

                            ClsObj clsObj10 = new ClsObj();
                            clsObj10.Index = 111;//钢化炉入口
                            clsObj10.obj = tListData;
                            Thread thread10 = new Thread(new ParameterizedThreadStart(Dispose));
                            thread10.Start(clsObj10);

                        }
                    }



                    byte[] tbufferValue2 = new byte[144];
                    int[] tValue2 = new int[36];
                    int readResult2 = PlcClient[0].DBRead(701, 0, tbufferValue2.Length, tbufferValue2);
                    if (readResult2 != 0)
                    {
                        PlcClient[0].Disconnect();
                        Thread.Sleep(100);
                        PlcClient[0].ConnectTo(clsMyPublic.mPLCIPA, 0, 1);//// IP  机架号  插槽号 
                    }
                    else
                    {
                        for (int i = 0; i < tValue2.Length; i++)
                        {
                            tValue2[i] = S7.GetDIntAt(tbufferValue2, i * 4);//转换成数据
                        }
                        if (tValue2[0] > 0 & tValue2[1] > 0 & MCPutUPloadFlow != tValue2[0])//上片 ok
                        {
                            MCPutUPloadFlow = tValue2[0];
                            ClsObj clsObj1 = new ClsObj();
                            ClsPutLoad clsPutLoad = new ClsPutLoad();
                            clsObj1.Index = 1;//上片
                            clsPutLoad.Enport = 2;
                            clsPutLoad.Flow = tValue2[0];
                            clsPutLoad.Status = tValue2[1];
                            clsObj1.obj = clsPutLoad;
                            Thread thread1 = new Thread(new ParameterizedThreadStart(Dispose));
                            thread1.Start(clsObj1);
                        }


                        if (tValue2[12] > 0 & MCLPoutFlow == 0)//理片机出片  理片出口空闲 ok
                        {
                            MCLPoutFlow = 1;
                            ClsObj clsObj3 = new ClsObj();
                            clsObj3.Index = 3;//理片机
                            clsObj3.obj = 2;
                            Thread thread3 = new Thread(new ParameterizedThreadStart(Dispose));
                            thread3.Start(clsObj3);

                        }

                        if (tValue2[6] > 0 & tValue2[7] > 0 & tValue2[8] > 0 & tValue2[9] > 0 & MCCheckFlow != tValue2[9] & CHMBTag[1] == 0)//理片机前匹配 ok
                        {
                            MCCheckFlow = tValue2[9];
                            ClsObj clsObj2 = new ClsObj();
                            ClsMBCheck clsMBCheck = new ClsMBCheck();
                            clsObj2.Index = 2;//数据核对
                            clsMBCheck.Enport = 2;
                            clsMBCheck.Status = tValue2[6]; clsMBCheck.Long = tValue2[7]; clsMBCheck.Short = tValue2[8]; clsMBCheck.Flow = tValue2[9]; clsMBCheck.LPbinStatus = tValue2[10];
                            clsObj2.obj = clsMBCheck;
                            Thread thread2 = new Thread(new ParameterizedThreadStart(Dispose));
                            thread2.Start(clsObj2);
                        }

                        if (tValue2[17] > 0 & tValue2[18] > 0 & MCLPOutFinishFlow != tValue2[18])//理片机出库完成 ok
                        {
                            MCLPOutFinishFlow = tValue2[18];
                            ClsObj clsObj4 = new ClsObj();
                            ClsMCFinish clsMCFinishcls4 = new ClsMCFinish();
                            clsObj4.Index = 4;//理片机出库完成
                            clsMCFinishcls4.Enport = 2;
                            clsMCFinishcls4.Flow = tValue2[18];
                            clsMCFinishcls4.Status = tValue2[17];
                            clsObj4.obj = clsMCFinishcls4;
                            Thread thread1 = new Thread(new ParameterizedThreadStart(Dispose));
                            thread1.Start(clsObj4);
                        }

                        if (tValue2[26] > 0 & tValue2[31] > 0 & tValue2[33] > 0 & tValue2[34] > 0 & MCEnportFlow2 != tValue2[31] & EnTag2 == 0)//入库
                        {
                            MCEnportFlow2 = tValue2[31];
                            ClsObj clsObj10 = new ClsObj();
                            ClsEnport clsEnport10 = new ClsEnport();
                            clsObj10.Index = 10;//入库口
                            clsEnport10.Status = tValue2[26];
                            clsEnport10.MBNO = 2;// tValue3[26];
                            clsEnport10.Long = tValue2[33];
                            clsEnport10.Short = tValue2[34];
                            int[] tFlowArr = new int[7];
                            tFlowArr[0] = tValue2[31]; tFlowArr[1] = tValue2[32]; tFlowArr[2] = tValue2[27]; tFlowArr[3] = tValue2[28];
                            tFlowArr[4] = tValue2[29]; tFlowArr[5] = tValue2[30]; tFlowArr[6] = tValue2[35];
                            clsEnport10.FlowArr = tFlowArr;
                            clsObj10.obj = clsEnport10;
                            Thread thread10 = new Thread(new ParameterizedThreadStart(Dispose));
                            thread10.Start(clsObj10);
                        }
                    }


                    ////
                    byte[] tbufferValue3 = new byte[144];
                    int[] tValue3 = new int[36];
                    int readResult3 = PlcClient[0].DBRead(702, 0, tbufferValue3.Length, tbufferValue3);
                    if (readResult3 != 0)
                    {
                        PlcClient[0].Disconnect();
                        Thread.Sleep(100);
                        PlcClient[0].ConnectTo(clsMyPublic.mPLCIPA, 0, 1);//// IP  机架号  插槽号 
                    }
                    else
                    {
                        for (int i = 0; i < tValue3.Length; i++)
                        {
                            tValue3[i] = S7.GetDIntAt(tbufferValue3, i * 4);//转换成数据
                        }
                        if (tValue3[0] > 0 & tValue3[1] > 0 & MCPutUPloadFlow != tValue3[0])//上片 ok
                        {
                            MCPutUPloadFlow = tValue3[0];
                            ClsObj clsObj1 = new ClsObj();
                            ClsPutLoad clsPutLoad = new ClsPutLoad();
                            clsObj1.Index = 1;//上片
                            clsPutLoad.Enport = 3;
                            clsPutLoad.Flow = tValue3[0];
                            clsPutLoad.Status = tValue3[1];
                            clsObj1.obj = clsPutLoad;
                            Thread thread1 = new Thread(new ParameterizedThreadStart(Dispose));
                            thread1.Start(clsObj1);
                        }


                        if (tValue3[12] > 0 & MCLPoutFlow == 0)//理片机出片  理片出口空闲 ok
                        {
                            MCLPoutFlow = 1;
                            ClsObj clsObj3 = new ClsObj();
                            clsObj3.Index = 3;//理片机
                            clsObj3.obj = 3;
                            Thread thread3 = new Thread(new ParameterizedThreadStart(Dispose));
                            thread3.Start(clsObj3);

                        }

                        if (tValue3[6] > 0 & tValue3[7] > 0 & tValue3[8] > 0 & tValue3[9] > 0 & MCCheckFlow != tValue3[9] & CHMBTag[2] == 0)//理片机前匹配 ok
                        {
                            MCCheckFlow = tValue3[9];
                            ClsObj clsObj2 = new ClsObj();
                            ClsMBCheck clsMBCheck = new ClsMBCheck();
                            clsObj2.Index = 2;//数据核对
                            clsMBCheck.Enport = 3;
                            clsMBCheck.Status = tValue3[6]; clsMBCheck.Long = tValue3[7]; clsMBCheck.Short = tValue3[8]; clsMBCheck.Flow = tValue3[9]; clsMBCheck.LPbinStatus = tValue3[10];
                            clsObj2.obj = clsMBCheck;
                            Thread thread2 = new Thread(new ParameterizedThreadStart(Dispose));
                            thread2.Start(clsObj2);
                        }

                        if (tValue3[17] > 0 & tValue3[18] > 0 & MCLPOutFinishFlow != tValue3[18])//理片机出库完成 ok
                        {
                            MCLPOutFinishFlow = tValue3[18];
                            ClsObj clsObj4 = new ClsObj();
                            ClsMCFinish clsMCFinishcls4 = new ClsMCFinish();
                            clsObj4.Index = 4;//理片机出库完成
                            clsMCFinishcls4.Enport = 3;
                            clsMCFinishcls4.Flow = tValue3[18];
                            clsMCFinishcls4.Status = tValue3[17];
                            clsObj4.obj = clsMCFinishcls4;
                            Thread thread1 = new Thread(new ParameterizedThreadStart(Dispose));
                            thread1.Start(clsObj4);
                        }

                        if (tValue3[26] > 0 & tValue3[31] > 0 & tValue3[33] > 0 & tValue3[34] > 0 & MCEnportFlow3 != tValue3[31] & EnTag3 == 0)//入库
                        {
                            MCEnportFlow3 = tValue3[31];
                            ClsObj clsObj10 = new ClsObj();
                            ClsEnport clsEnport10 = new ClsEnport();
                            clsObj10.Index = 10;//入库口
                            clsEnport10.Status = tValue3[26];
                            clsEnport10.MBNO = 3;// tValue3[26];
                            clsEnport10.Long = tValue3[33];
                            clsEnport10.Short = tValue3[34];
                            int[] tFlowArr = new int[7];
                            tFlowArr[0] = tValue3[31]; tFlowArr[1] = tValue3[32]; tFlowArr[2] = tValue3[27]; tFlowArr[3] = tValue3[28];
                            tFlowArr[4] = tValue3[29]; tFlowArr[5] = tValue3[30]; tFlowArr[6] = tValue3[35];
                            clsEnport10.FlowArr = tFlowArr;
                            clsObj10.obj = clsEnport10;
                            Thread thread10 = new Thread(new ParameterizedThreadStart(Dispose));
                            thread10.Start(clsObj10);
                        }
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
            switch (tobj.Index)
            {
                case 1:
                    ClsPutLoad clsPutLoad = new ClsPutLoad();
                    clsPutLoad = (ClsPutLoad)tobj.obj;
                    MCPutUPload(1, clsPutLoad.Status, clsPutLoad.Flow, clsPutLoad.Enport);
                    MCPutUPloadFlow = 0;
                    break;
                case 2://
                    ClsMBCheck clsMBCheck = new ClsMBCheck();
                    clsMBCheck = (ClsMBCheck)tobj.obj;
                    MCCheck(2, clsMBCheck.Status, clsMBCheck.Flow, clsMBCheck.Long, clsMBCheck.Short, clsMBCheck.Enport);
                    MCCheckFlow = 0;
                    break;
                case 3://理片机出片
                    //MCLPoutCmd(3, 1);
                    MCLPoutFlow = 0;
                    break;
                case 4://理片机出库完成
                    ClsMCFinish clsMCFinishcls4 = new ClsMCFinish();
                    clsMCFinishcls4 = (ClsMCFinish)tobj.obj;
                    MCLFOutFinish(4, clsMCFinishcls4.Status, clsMCFinishcls4.Flow, clsMCFinishcls4.Enport);
                    MCLPOutFinishFlow = 0;
                    break;
                case 10:
                    ClsEnport clsEnport10 = new ClsEnport();
                    clsEnport10 = (ClsEnport)tobj.obj;
                    MCEnportWH(5, clsEnport10.Status, clsEnport10.Long, clsEnport10.Short, clsEnport10.MBNO, clsEnport10.FlowArr);//clsEnport10.MBNO入口
                    if (clsEnport10.MBNO == 1)
                    {
                        MCEnportFlow1 = 0;
                    }
                    else if (clsEnport10.MBNO == 2)
                    {
                        MCEnportFlow2 = 0;
                    }
                    if (clsEnport10.MBNO == 3)
                    {
                        MCEnportFlow3 = 0;
                    }
                    break;
                case 14:
                    ClsMCFinish clsMCFinishcls14 = new ClsMCFinish();
                    clsMCFinishcls14 = (ClsMCFinish)tobj.obj;
                    MCEnportWHFinish(6, clsMCFinishcls14.Status, clsMCFinishcls14.Flow, clsMCFinishcls14.Enport);
                    MCInFinishFlow = 0;
                    break;
                case 20://出库
                    int tTemp = (int)tobj.obj;
                    MCWHoutCmd(7, tTemp);
                    MCExportFlow = 0;
                    break;
                case 24:
                    ClsOutFinish ClsOutFinish24 = new ClsOutFinish();
                    ClsOutFinish24 = (ClsOutFinish)tobj.obj;
                    MCWHoutFinish(8, ClsOutFinish24.OutCmd, ClsOutFinish24.Flow1, ClsOutFinish24.Flow2, ClsOutFinish24.Flow3, ClsOutFinish24.Flow4, ClsOutFinish24.CarNO);
                    MCOutFinishFlow = 0;
                    break;
                case 999:
                    List<ClsMachineData> tListData = new List<ClsMachineData>();
                    tListData = (List<ClsMachineData>)tobj.obj;

                    DataTable tDt = new DataTable();
                    tDt = ToDataTable<ClsMachineData>(tListData);
                    MCPlcflow(tDt);
                    break;
                case 111:
                    List<ClsMachineData> tListDataFur = new List<ClsMachineData>();
                    tListDataFur = (List<ClsMachineData>)tobj.obj;

                    DataTable tDtFur = new DataTable();
                    tDtFur = ToDataTable<ClsMachineData>(tListDataFur);
                    MCPlcflowFur(tDtFur, 1);
                    break;
            }
        }


        private void MCPlcflowFur(DataTable _Dt, int _No)
        {
            ClsDbAcc tClsDB = new ClsDbAcc(tSystem);
            DataSet dataSet = new DataSet(); string tRetData = "", tShowStr = "";
            dataSet = tClsDB.ExecuteGetPlcFlowTvp("ProFurNO", 1, _Dt);
            if (dataSet != null)
            {
                for (int i = 0; i < _Dt.Rows.Count; i++)
                {
                    for (int j = 0; j < _Dt.Columns.Count; j++)
                    {
                        tShowStr = tShowStr + " " + _Dt.Rows[i][j].ToString();
                    }
                }
                tSystem.mFile.WriteLog("", "进钢化炉 组号" + tShowStr + "J");
                if (dataSet.Tables.Count > 0)
                {
                    if (dataSet.Tables[dataSet.Tables.Count - 1].Rows.Count == 0) { return; }

                }
            }
        }
        private void MCPlcflow(DataTable _Dt)
        {
            ClsDbAcc tClsDB = new ClsDbAcc(tSystem);
            DataSet dataSet = new DataSet(); string tRetData = "";
            dataSet = tClsDB.ExecuteGetPlcFlowTvp("ProPlcFlow", 1, _Dt);
            if (dataSet.Tables.Count > 0)
            {
                if (dataSet.Tables[dataSet.Tables.Count - 1].Rows.Count == 0) { return; }
                if (dataSet.Tables[dataSet.Tables.Count - 1].Rows[0][0].ToString().ToUpper() == "Y" & dataSet.Tables[dataSet.Tables.Count - 1].Rows[0][2].ToString().ToUpper() == "钢化排版")
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

        private bool MCPutUPload(int _Plcline, int _Status, int _PlcFlow, int _Enport)
        {
            ClsDbAcc tClsDB = new ClsDbAcc(tSystem);
            string tRetData = "", tRetStr = ""; int tRows = 0;
            tClsDB.ExecutePro("Pro_01MBStart", 1, _PlcFlow.ToString(), 0, 0, _Enport, ref tRetData, ref tRetStr, ref tRows);
            if (tRetStr.ToUpper() == "Y")
            {
                string[] tValues = tRetData.Split(',');
                if (tValues.Length == 7)
                {
                    byte[] tbufferValue = new byte[20];
                    int[] tValue = new int[5];

                    tValue[0] = int.Parse(tValues[0]);//状态
                    S7.SetDIntAt(tbufferValue, 0 * 4, tValue[0]);

                    tValue[1] = int.Parse(tValues[2]);
                    S7.SetDIntAt(tbufferValue, 1 * 4, tValue[1] * 1000);

                    tValue[2] = int.Parse(tValues[3]);
                    S7.SetDIntAt(tbufferValue, 2 * 4, tValue[2] * 1000);

                    tValue[3] = int.Parse(tValues[5]);//流水号
                    S7.SetDIntAt(tbufferValue, 3 * 4, tValue[3]);

                    tValue[4] = int.Parse(tValues[4]); ;//磨边走向
                    S7.SetDIntAt(tbufferValue, 4 * 4, tValue[4]);
                    if (_Enport == 1)
                    {
                        Write_PLC_Data(_Plcline, 751, 0, tbufferValue);
                    }
                    else if (_Enport == 2)
                    {
                        Write_PLC_Data(_Plcline, 752, 0, tbufferValue);
                    }
                    else if (_Enport == 3)
                    {
                        Write_PLC_Data(_Plcline, 753, 0, tbufferValue);
                    }

                    tSystem.mFile.WriteLog("", "上片 线" + _Enport + "  状态 " + tValue[0].ToString() + " 长边 " + tValue[1] + " 短边" + tValue[2] + " 流水号" + tValue[3] + "  走向 " + tValue[4] + " WCSID " + tValues[6] + "C");
                    return true;
                }
            }
            return false;
        }
        int mMBcheckTag = 0;
        int[] CHMBTag = new int[3] { 0, 0, 0 }; DateTime[] CHMBtagtime = new DateTime[3] { DateTime.Now, DateTime.Now, DateTime.Now };
        private bool MCCheck(int _Plcline, int _Status, int _PlcFlow, int _Long, int _Short, int _Enport)
        {
            System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
            stopwatch.Start();

            if (mMBcheckTag == 1)
            {
                return false;
            }
            mMBcheckTag = 1;
            ClsDbAcc tClsDB = new ClsDbAcc(tSystem);
            string tRetData = "", tRetStr = ""; int tRows = 0;
            tClsDB.ExecutePro("Pro_02MBCheckNew", _Status, _PlcFlow.ToString(), _Long, _Short, _Enport, ref tRetData, ref tRetStr, ref tRows);
            mMBcheckTag = 0;
            if (tRetStr.ToUpper() == "Y")
            {
                string[] tValues = tRetData.Split(',');
                if (tValues.Length == 8)
                {
                    byte[] tbufferValue = new byte[32];
                    int[] tValue = new int[8];

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

                    tValue[4] = int.Parse(tValues[4]); ;//磨边走向  旗滨改为厚度
                    S7.SetDIntAt(tbufferValue, 4 * 4, tValue[4]);

                    tValue[5] = int.Parse(tValues[6]);//理片机
                    S7.SetDIntAt(tbufferValue, 5 * 4, tValue[5]);

                    tValue[6] = int.Parse(tValues[7]);//第几片
                    S7.SetDIntAt(tbufferValue, 6 * 4, tValue[6]);

                    tValue[7] = 1;//PC控制
                    S7.SetDIntAt(tbufferValue, 7 * 4, tValue[7]);
                    if (_Enport == 1)
                    {
                        Write_PLC_Data(2, 751, 24, tbufferValue);
                    }
                    else if (_Enport == 2)
                    {
                        Write_PLC_Data(2, 752, 24, tbufferValue);

                    }
                    else if (_Enport == 3)
                    {
                        Write_PLC_Data(2, 753, 24, tbufferValue);

                    }
                    stopwatch.Stop();
                    tSystem.mFile.WriteLog("", "磨边核对 状态" + tValue[0].ToString() + " 长边 " + tValue[1] + " 测" + _Long + " 短边 " + tValue[2] + " 测" + _Short + " 流水号 " + tValue[3] + " 磨边方向 " + tValue[4] + " 理片 " + tValue[5] + " 片 " + tValue[6] + " " + " 耗时" + stopwatch.Elapsed.TotalSeconds + "  " + tRetData + "F");
                    return true;
                }
            }
            else if (_Enport > 0 & _Enport < 3)
            {
                CHMBTag[_Enport - 1] = 1;
                CHMBtagtime[_Enport - 1] = DateTime.Now;
            }

            return false;
        }

        private bool MCLPoutCmd(int _Plcline, int _Status, int _Enport)
        {
            ClsDbAcc tClsDB = new ClsDbAcc(tSystem);
            string tRetData = "", tRetStr = ""; int tRows = 0;
            tClsDB.ExecutePro("Pro_02MBCheck", 9999, "", 0, 0, _Enport, ref tRetData, ref tRetStr, ref tRows);
            if (tRetStr.ToUpper() == "Y")
            {

                string[] tValues = tRetData.Split(',');
                if (tValues.Length == 3)
                {
                    byte[] tbufferValue = new byte[12];
                    int[] tValueL = new int[3];
                    tValueL[0] = int.Parse(tValues[0]);
                    S7.SetDIntAt(tbufferValue, 0 * 4, tValueL[0]);
                    tValueL[1] = int.Parse(tValues[1]); ;
                    S7.SetDIntAt(tbufferValue, 1 * 4, tValueL[1]);
                    tValueL[2] = int.Parse(tValues[2]);
                    S7.SetDIntAt(tbufferValue, 2 * 4, tValueL[2]);
                    Write_PLC_Data(_Plcline, 501, 100, tbufferValue);
                    tSystem.mFile.WriteLog("", "理片机出 状态" + tValueL[0].ToString() + " 库层 " + tValueL[1] + " 返回值" + tValueL[2] + "F");
                    return true;
                }
            }
            return false;
        }
        private bool MCLFOutFinish(int _Plcline, int _Status, int _PlcFlow, int _Enport)
        {
            try
            {

                ClsDbAcc tClsDB = new ClsDbAcc(tSystem);
                string tRetData = "", tRetStr = ""; int tRows = 0;
                tClsDB.ExecutePro("Pro_03FinishLFOut", 1, _PlcFlow.ToString(), 0, 0, _Enport, ref tRetData, ref tRetStr, ref tRows);

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


                Write_PLC_Data(_Plcline, 501, 112, tbufferValue);
                tSystem.mFile.WriteLog("", "理片机出完成 状态" + tValue[0].ToString() + " 返回值 " + tValue[1] + "F");
                return true;
            }
            catch
            {
                return false;
            }
        }
        public class ClsMachineData
        {
            public int ID { get; set; }
            public string Machine_NO { get; set; }
            public int Plc_Flow { get; set; }
        }

        int EnTag1 = 0, EnTag2 = 0, EnTag3 = 0;
        private bool MCEnportWH(int _Plcline, int _Status, int _Long, int _Short, int _ExportMB, int[] _Arr)
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
            dataSet = tClsDB.ExecuteProInLocationZK("Pro_10MBInLocation09", _Status, tDataTable, _Long, _Short, _ExportMB, ref tRetData, ref tRetStr, ref tRows);

            if (dataSet == null) { mEnportTag = 0; return false; }
            if (dataSet.Tables.Count > 0)
            {
                if (dataSet.Tables[dataSet.Tables.Count - 1].Rows.Count == 0) { mEnportTag = 0; return false; }
                if (dataSet.Tables[dataSet.Tables.Count - 1].Rows[0][0].ToString().ToUpper() == "Y" & dataSet.Tables[dataSet.Tables.Count - 1].Rows[0][2].ToString().ToUpper() == "磨边入库指令")
                {
                    tRetData = dataSet.Tables[dataSet.Tables.Count - 1].Rows[0][1].ToString();
                    string[] tValues = tRetData.Split(',');
                    if (tValues.Length == 14)
                    {
                        byte[] tbufferValue = new byte[28];
                        int[] tValue = new int[7];

                        tValue[0] = int.Parse(tValues[6]);// 状态
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

                        tValue[4] = int.Parse(tValues[7]);// 订序
                        S7.SetDIntAt(tbufferValue, 4 * 4, tValue[4]);

                        tValue[5] = int.Parse(tValues[2]);
                        S7.SetDIntAt(tbufferValue, 5 * 4, tValue[5] * 1000);//长边

                        tValue[6] = int.Parse(tValues[3]);
                        S7.SetDIntAt(tbufferValue, 6 * 4, tValue[6] * 1000);//短边


                        if (_ExportMB == 1)
                        {
                            Write_PLC_Data(_Plcline, 751, 104, tbufferValue);
                        }
                        else if (_ExportMB == 2)
                        {
                            Write_PLC_Data(_Plcline, 752, 104, tbufferValue);
                        }
                        else if (_ExportMB == 3)
                        {
                            Write_PLC_Data(_Plcline, 753, 104, tbufferValue);
                        }
                        tEnportCmdtime[_ExportMB - 1] = DateTime.Now;
                        stopwatch.Stop();
                        dataSet = tClsDB.ExecuteProInLocationZK("Pro_10MBInLocation", _Status, tDataTable, _Long, _Short, _ExportMB, ref tRetData, ref tRetStr, ref tRows);
                        tSystem.mFile.WriteLog("", "磨边库入库 状态" + tValue[0].ToString() + " 流水号 " + tValue[1] + " 库号 " + tValue[2] + " 格号 " + tValue[3] + " 片数 " + tValue[4] + " 长 " + tValue[5] + " 测" + _Long + " 短 " + tValue[6] + " 测" + _Short + " 线别" + _ExportMB
                           + " 有无 " + _Arr[6] + " 耗时" + stopwatch.Elapsed.TotalSeconds + "  " + tRetData + "H");
                        tSystem.mFile.WriteLog("", "磨边库入库 状态" + tValue[0].ToString() + " 流水号 " + tValue[1] + " 库号 " + tValue[2] + " 格号 " + tValue[3] + " 片数 " + tValue[4] + " 长 " + tValue[5] + " 测" + _Long + " 短 " + tValue[6] + " 测" + _Short + " 线别" + _ExportMB
                           + " 有无 " + _Arr[6] + " 耗时" + stopwatch.Elapsed.TotalSeconds + "  " + tRetData + "G");
                        mEnportTag = 0;
                        return true;

                    }

                }
                else if (dataSet.Tables[dataSet.Tables.Count - 1].Rows[0][0].ToString().Contains("未找到库位") == true
                          & dataSet.Tables[dataSet.Tables.Count - 1].Rows[0][0].ToString().Contains(_Status.ToString() + " " + _Arr[0].ToString()) == true
                          & dataSet.Tables[dataSet.Tables.Count - 1].Rows[0][2].ToString().ToUpper() == "磨边入库指令")
                {
                    byte[] tbufferValue = new byte[8];
                    int[] tValue = new int[2];

                    tValue[0] = 12;
                    S7.SetDIntAt(tbufferValue, 0 * 4, tValue[0]);

                    tValue[1] = _Arr[0];
                    S7.SetDIntAt(tbufferValue, 1 * 4, tValue[1]);
                    if (_ExportMB == 1)
                    {
                        Write_PLC_Data(_Plcline, 751, 104, tbufferValue);
                        EnTag1 = 1;
                    }
                    else if (_ExportMB == 2)
                    {
                        Write_PLC_Data(_Plcline, 752, 104, tbufferValue);
                        EnTag2 = 1;
                    }
                    else if (_ExportMB == 3)
                    {
                        Write_PLC_Data(_Plcline, 753, 104, tbufferValue);
                        EnTag3 = 1;
                    }
                }
                else if (dataSet.Tables[dataSet.Tables.Count - 1].Rows[0][0].ToString().Contains(_Status.ToString() + " " + _Arr[0].ToString()) == true
                      & dataSet.Tables[dataSet.Tables.Count - 1].Rows[0][2].ToString().ToUpper() == "磨边入库指令")
                {
                    byte[] tbufferValue = new byte[8];
                    int[] tValue = new int[2];

                    tValue[0] = 11;
                    S7.SetDIntAt(tbufferValue, 0 * 4, tValue[0]);

                    tValue[1] = _Arr[0];
                    S7.SetDIntAt(tbufferValue, 1 * 4, tValue[1]);
                    if (_ExportMB == 1)
                    {
                        Write_PLC_Data(_Plcline, 751, 104, tbufferValue);
                        EnTag1 = 1;
                    }
                    else if (_ExportMB == 2)
                    {
                        Write_PLC_Data(_Plcline, 752, 104, tbufferValue);
                        EnTag2 = 1;
                    }
                    else if (_ExportMB == 3)
                    {
                        Write_PLC_Data(_Plcline, 753, 104, tbufferValue);
                        EnTag3 = 1;
                    }

                }
            }
            mEnportTag = 0;
            return false;

        }
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
                Write_PLC_Data(_Plcline, 751, 248, tbufferValue);
            }
            else if (_Enport == 2)
            {
                Write_PLC_Data(_Plcline, 751, 256, tbufferValue);
            }

            tSystem.mFile.WriteLog("", "磨边库入库完成 状态" + tValue[0].ToString() + " 流水号 " + tValue[1] + " 车号 " + _Enport + "H");
            return true;
        }
        int tTempA = 0;

        int MBOUTTag = 0;

        DateTime tOutCmdtime = DateTime.Now;
        private bool MCWHoutCmd(int _Plcline, int _Status)
        {
            if (MBOUTTag == 1 | DateTime.Now.Subtract(tOutCmdtime).TotalMilliseconds < 3000)
            {
                return false;
            }
            MBOUTTag = 1;
            ClsDbAcc tClsDB = new ClsDbAcc(tSystem);
            string tRetData = "", tRetStr = ""; int tRows = 0;
            DataTable tDataTable = new DataTable();
            string tStatusStr = _Status.ToString().PadLeft(5, '0');
            List<ClsMachineData> tListData = new List<ClsMachineData>();
            tDataTable = ToDataTable<ClsMachineData>(tListData);
            DataSet dataSet = new DataSet();
            dataSet = tClsDB.ExecuteProInLocationZK("Pro_20MBOutLocation", 1, tDataTable, mMBOutCmdFlow, _Status, 0, ref tRetData, ref tRetStr, ref tRows);

            if (dataSet == null) { MBOUTTag = 0; return false; }
            if (dataSet.Tables.Count > 0)
            {
                if (dataSet.Tables[dataSet.Tables.Count - 1].Rows.Count == 0) { MBOUTTag = 0; return false; }
                if (dataSet.Tables[dataSet.Tables.Count - 1].Rows[0][0].ToString().ToUpper() == "Y" & dataSet.Tables[dataSet.Tables.Count - 1].Rows[0][2].ToString().ToUpper() == "磨边出库指令")
                {
                    tRetData = dataSet.Tables[dataSet.Tables.Count - 1].Rows[0][1].ToString();
                    string[] tValues = tRetData.Split(',');
                    if (tValues.Length == 16)
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

                        //if (tValue[1] < 5 )
                        //{
                        //    tValue[4] = 1; ;//出口
                        //    S7.SetDIntAt(tbufferValue, 4 * 4, tValue[4]);

                        //}
                        //else
                        if ((tValue[1] == 8 & tValue[4] == 1) | (tValue[1] == 1 & tValue[4] == 3))
                        {
                            tValue[4] = 2; ;//出口
                            S7.SetDIntAt(tbufferValue, 4 * 4, tValue[4]);
                        }

                        mMBOutCmdFlow = mMBOutCmdFlow + 1;

                        if (mMBOutCmdFlow > 30000)
                        {
                            mMBOutCmdFlow = 1;
                        }

                        SettingsMC.Default.MBOutCmdFlow = mMBOutCmdFlow;
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

                        tTempA = tTempA + 1;
                        tValue[11] = int.Parse(tValues[11]);
                        S7.SetDIntAt(tbufferValue, 11 * 4, tTempA * 10 + tValue[4]);

                        tValue[12] = int.Parse(tValues[12]);
                        S7.SetDIntAt(tbufferValue, 12 * 4, tTempA * 10 + tValue[4]);
                        //if (tValues[13] == "0")
                        //{
                        //    S7.SetDIntAt(tbufferValue, 13 * 4, tValue[13]);
                        //    tValue[13] = 1;
                        //}
                        //else
                        //{
                        //    tValue[13] = 0;
                        //}

                        byte[] tAbufferValue = new byte[4];
                        int[] tAValue = new int[1];

                        if (tValues[13] == "1")
                        {

                            tAValue[0] = 1;// int.Parse(tValues[5]);
                        }
                        else
                        {
                            tAValue[0] = 0;
                        }
                        S7.SetDIntAt(tAbufferValue, 0 * 4, tAValue[0]);

                        int tCarNO = 0;
                        if (tValues[14].Trim() == "1" & tStatusStr.Substring(0, 1) == "1")
                        {
                            Write_PLC_Data(_Plcline, 751, 144, tbufferValue);
                            tCarNO = 1;

                        }
                        else if (tValues[14].Trim() == "2" & tStatusStr.Substring(1, 1) == "1")
                        {
                            Write_PLC_Data(_Plcline, 751, 196, tbufferValue);
                            tCarNO = 2;
                        }
                        else
                        {
                            if (tValue[4] == 1 & tStatusStr.Substring(0, 1) == "1")
                            {
                                Write_PLC_Data(_Plcline, 751, 144, tbufferValue);
                                tCarNO = 11;

                            }
                            else if (tValue[4] == 3 & tStatusStr.Substring(1, 1) == "1")
                            {
                                Write_PLC_Data(_Plcline, 751, 196, tbufferValue);
                                tCarNO = 22;
                            }
                            else if (tValue[4] == 2 & tValue[1] < 5 & tStatusStr.Substring(0, 1) == "1")
                            {
                                Write_PLC_Data(_Plcline, 751, 144, tbufferValue);
                                tCarNO = 13;

                            }
                            else if (tValue[4] == 2 & tValue[1] > 4 & tStatusStr.Substring(1, 1) == "1")
                            {
                                Write_PLC_Data(_Plcline, 751, 196, tbufferValue);
                                tCarNO = 24;
                            }
                        }

                        if (tCarNO == 1 | tCarNO == 11 | tCarNO == 13)
                        {
                            //Write_PLC_Data(_Plcline, 751, 192, tAbufferValue);
                            Write_PLC_Data(_Plcline, 500, 0, tAbufferValue);
                        }
                        else
                        {
                            Write_PLC_Data(_Plcline, 500, 4, tAbufferValue);
                            //Write_PLC_Data(_Plcline, 751, 244, tAbufferValue);
                        }

                        //else if (tValue[4] == 2 & tValue[1] < 8 & tStatusStr.Substring(0, 1) == "1")
                        //{
                        //    Write_PLC_Data(_Plcline, 751, 144, tbufferValue);
                        //    tCarNO = 1;
                        //}
                        //else if (tValue[4] == 2 & tValue[1] > 1 & tStatusStr.Substring(1, 1) == "1")
                        //{
                        //    Write_PLC_Data(_Plcline, 751, 196, tbufferValue);
                        //    tCarNO = 2;
                        //}

                        //if (tValue[1] < 5 & tStatusStr.Substring(0, 1) == "1")
                        //{
                        //    Write_PLC_Data(_Plcline, 751, 144, tbufferValue);
                        //}
                        //else if (tValue[1] > 4 & tStatusStr.Substring(1, 1) == "1")
                        //{
                        //    Write_PLC_Data(_Plcline, 751, 196, tbufferValue);
                        //}
                        tSystem.mFile.WriteLog("", string.Concat("磨边出库 状态" + tValue[0].ToString(), " 库号 ", tValue[1], " 格号 ", tValue[2], " 片数 ", tValue[3], " 出口 ", tValue[4]
                                 , " 指令编号 ", tValue[5], " 序号1 ", tValue[6], " 序号2 ", tValue[7], " 序号3 ", tValue[8], " 序号4 ", tValue[9]
                                  , " 下格 ", tValue[10], " 再下格 ", tValue[11], "  梭车号 ", tValue[12], "  成品单片ID ", tValues[13], " 状态出库 ", tStatusStr, " 车号" + tCarNO, " 最后一片", tValues[13], ' ', tRetData, "I"));
                        if (tValues[14] == "0")
                        {

                        }
                        MBOUTTag = 0;
                        return true;
                    }
                    else
                    {
                        tSystem.mFile.WriteLog("", string.Concat("磨边出库 错误 状态" + _Status.ToString() + tRetData, "I"));
                    }
                }
            }
            MBOUTTag = 0;
            return false;
        }
        private bool MCWHoutFinish(int _Plcline, int _CmdFlow, int _Flow1, int _Flow2, int _Flow3, int _Flow4, int _CarNO)
        {
            ClsDbAcc tClsDB = new ClsDbAcc(tSystem);
            string tRetData = "", tRetStr = ""; int tRows = 0;
            tClsDB.ExecuteProFinishOut("Pro_21FinishCmdOut", 1, _CmdFlow, _Flow1, _Flow2, _Flow3, _Flow4, _CarNO, ref tRetData, ref tRetStr, ref tRows);
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
                Write_PLC_Data(_Plcline, 751, 264, tbufferValue);
            }
            else if (_CarNO == 2)
            {
                Write_PLC_Data(_Plcline, 751, 272, tbufferValue);
            }
            tSystem.mFile.WriteLog("", "磨边库出库完成 状态" + tValue[0].ToString() + " 流水号 " + tValue[1] + " 车号 " + _CarNO + "I");
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
