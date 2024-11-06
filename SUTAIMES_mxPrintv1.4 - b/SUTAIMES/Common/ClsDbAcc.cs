
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;
using System.Configuration;
using System.Collections;
using SUTAIMES;
using System.Windows.Forms;



namespace SUTAIMES.Common
{
    public class ClsDbAcc
    {
        public SqlConnection mSqlConn;
        public SqlDataAdapter mDa;
        public string strConnection = "user id=sa;password=123456;"//    wcs     801020;";Sa123456  ///123456
                      + "initial catalog=STMBDB;Data Source=192.168.11.250; "//192.168.2.212;";//10.71.64.203///   192.168.1.11; 127.0.0.1  192.168.11.240
                      + "Connect Timeout=10"
                      +";pooling=false";
                 
        public Program tSystem;
        public int mOpenIs = 0;
        public int mIndex=0;
        public ClsDbAcc(Program tSys)
        {
            tSystem = tSys;
           
            
            mSqlConn = new SqlConnection();

            if (mIndex == 2)
            {
                strConnection = clsMyPublic.mstrConnectionB;
            }
            else if (clsMyPublic.mstrConnectionA.Length > 10)
            {
                strConnection = clsMyPublic.mstrConnectionA;
            }
            mSqlConn.ConnectionString = strConnection;


            mDa = new SqlDataAdapter();

        }

        public bool DBopen()
        {
            try
            {
                if (mIndex == 2)
                {
                    strConnection = clsMyPublic.mstrConnectionB;
                    mSqlConn.ConnectionString = strConnection;
                }
                else if (clsMyPublic.mstrConnectionA.Length > 10)
                {
                    strConnection = clsMyPublic.mstrConnectionA;
                }

                mSqlConn.Open();
                mOpenIs = 1;
              
                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.StackTrace);
                return false;
            }
        }
        public bool DBClose()
        {
            try
            {
                mSqlConn.Close();
                mOpenIs = 0;

                return true;
            }
            catch (Exception e)
            {
                MessageBox.Show(e.StackTrace);
                return false;
            }
        }
    
        public string Execute_Command(string strstmt)
        {
            SqlTransaction sqlTrans = null;
            try
            {

                if (mSqlConn.State != ConnectionState.Open) mSqlConn.Open();

                SqlCommand sqlComm = new SqlCommand("", mSqlConn, sqlTrans);
                sqlComm.CommandTimeout = 120;
                sqlComm.CommandType = System.Data.CommandType.Text;

                //check 
                sqlComm.CommandText = strstmt;
                sqlComm.ExecuteNonQuery();//执行update  


                //sqlTrans.Commit();
                return "";

            }
            catch (Exception ex)
            {
                DBClose();
                return ex.Message + " :" + strstmt;
                //MessageBox.Show("Execute_Command" + strstmt.ToString() + ex.Message + ex.StackTrace);
                //tSystem.mFile.WriteLog("", "Execute_Command" + strstmt.ToString() + ex.Message + ex.StackTrace);
            }

        }

        public string RetrieveDataTable_from_DB(string strstmt, ref DataTable dt)
        {
            SqlTransaction sqlTrans = null;
            try
            {

                if (mSqlConn.State != ConnectionState.Open) mSqlConn.Open();



                sqlTrans = mSqlConn.BeginTransaction();
                SqlCommand sqlComm = new SqlCommand("", mSqlConn, sqlTrans);
                sqlComm.CommandTimeout = 120;
                sqlComm.CommandType = System.Data.CommandType.Text;

                //check 
                sqlComm.CommandText = strstmt;
                mDa.SelectCommand = sqlComm;
                mDa.Fill(dt);
                sqlTrans.Commit();
                return "";
            }
            catch (Exception ex)
            {
                MessageBox.Show("RetrieveDataTable_from_DB" + strstmt.ToString() + ex.Message + ex.StackTrace);
                return "RetrieveDataTable_from_DB" + strstmt.ToString() + ex.Message + ex.StackTrace;
            }
            
        }


        public void ExecuteTvp(string USPName, DataTable tDT, ref string tReString)///导入数据
        {
            try
            {
                int Rows = 0;
                SqlCommand tSqlCmdu = mSqlConn.CreateCommand();
                tSqlCmdu.CommandType = CommandType.StoredProcedure;
                tSqlCmdu.CommandText = USPName;


                SqlParameter[] tSqlParmu = new SqlParameter[3];

                tSqlParmu[0] = new SqlParameter("@tvp_Data", SqlDbType.Structured, 10000);
                //outv

                tSqlParmu[1] = new SqlParameter("@RetData", SqlDbType.VarChar, 500);
                tSqlParmu[2] = new SqlParameter("@RetStr", SqlDbType.VarChar, 500);


                //指明参数是输入还是输出型
                for (int i = 0; i < tSqlParmu.Length - 1; i++)
                {
                    tSqlParmu[i].Direction = ParameterDirection.Input;
                }
                for (int i = 1; i < tSqlParmu.Length; i++)
                {
                    tSqlParmu[i].Direction = ParameterDirection.Output;
                }


                //给参数赋值

                tSqlParmu[0].Value = tDT;


                ////////parm[3].Value = OracleDateTime.Parse(m_dksrq.ToShortDateString());
                /////直接用update语句更新时,需要采用下面的日期格式.

                for (int i = 0; i < tSqlParmu.Length; i++)
                {
                    tSqlCmdu.Parameters.Add(tSqlParmu[i]);
                    //.Add(tSqlParm[i]);
                }


                if (mSqlConn.State != ConnectionState.Open)
                    mSqlConn.Open();

                //OrCmd.Connection = JHOracleConn;
                Rows = tSqlCmdu.ExecuteNonQuery();


                //取出返回值
                tReString = Convert.ToString(tSqlParmu[1].Value).Trim();//res
            }
            catch (Exception Ex)
            {
                MessageBox.Show("ExecuteTvp" + Ex.StackTrace + Ex.Message);
            }



        }

        public void ExecuteTvpIndex(string USPName,string _Index, DataTable tDT, ref string tReString)///导入数据
        {
            try
            {
                int Rows = 0;
                SqlCommand tSqlCmdu = mSqlConn.CreateCommand();
                tSqlCmdu.CommandType = CommandType.StoredProcedure;
                tSqlCmdu.CommandText = USPName;


                SqlParameter[] tSqlParmu = new SqlParameter[4];

                tSqlParmu[0] = new SqlParameter("@Index", SqlDbType.VarChar, 500);
                tSqlParmu[1] = new SqlParameter("@tvp_Data", SqlDbType.Structured, 10000);
                //outv

                tSqlParmu[2] = new SqlParameter("@RetData", SqlDbType.VarChar, 500);
                tSqlParmu[3] = new SqlParameter("@RetStr", SqlDbType.VarChar, 500);


                //指明参数是输入还是输出型
                for (int i = 0; i < tSqlParmu.Length - 1; i++)
                {
                    tSqlParmu[i].Direction = ParameterDirection.Input;
                }
                for (int i = 2; i < tSqlParmu.Length; i++)
                {
                    tSqlParmu[i].Direction = ParameterDirection.Output;
                }


                //给参数赋值

                tSqlParmu[0].Value = _Index;
                tSqlParmu[1].Value = tDT;


                ////////parm[3].Value = OracleDateTime.Parse(m_dksrq.ToShortDateString());
                /////直接用update语句更新时,需要采用下面的日期格式.

                for (int i = 0; i < tSqlParmu.Length; i++)
                {
                    tSqlCmdu.Parameters.Add(tSqlParmu[i]);
                    //.Add(tSqlParm[i]);
                }


                if (mSqlConn.State != ConnectionState.Open)
                    mSqlConn.Open();

                //OrCmd.Connection = JHOracleConn;
                Rows = tSqlCmdu.ExecuteNonQuery();


                //取出返回值
                tReString = Convert.ToString(tSqlParmu[3].Value).Trim();//res
            }
            catch (Exception Ex)
            {
                MessageBox.Show("ExecuteTvpIndex" + Ex.StackTrace + Ex.Message);
            }



        }


        public DataSet ExecuteGetPlcFlowTvp(string USPName, int _Status, DataTable tDT)///导入数据
        {
            try
            {
               
                SqlCommand tSqlCmdu = mSqlConn.CreateCommand();
                tSqlCmdu.CommandType = CommandType.StoredProcedure;
                tSqlCmdu.CommandText = USPName;


                SqlParameter[] tSqlParmu = new SqlParameter[4];

                tSqlParmu[0] = new SqlParameter("@Status", SqlDbType.VarChar, 500);
                tSqlParmu[1] = new SqlParameter("@tvp_Data", SqlDbType.Structured, 10000);
                //outv

                tSqlParmu[2] = new SqlParameter("@RetData", SqlDbType.VarChar, 500);
                tSqlParmu[3] = new SqlParameter("@RetStr", SqlDbType.VarChar, 500);


                //指明参数是输入还是输出型
                for (int i = 0; i < tSqlParmu.Length - 1; i++)
                {
                    tSqlParmu[i].Direction = ParameterDirection.Input;
                }
                for (int i = 2; i < tSqlParmu.Length; i++)
                {
                    tSqlParmu[i].Direction = ParameterDirection.Output;
                }


                //给参数赋值
                tSqlParmu[0].Value = _Status;
                tSqlParmu[1].Value = tDT;


                ////////parm[3].Value = OracleDateTime.Parse(m_dksrq.ToShortDateString());
                /////直接用update语句更新时,需要采用下面的日期格式.

                for (int i = 0; i < tSqlParmu.Length; i++)
                {
                    tSqlCmdu.Parameters.Add(tSqlParmu[i]);
                    //.Add(tSqlParm[i]);
                }


                if (mSqlConn.State != ConnectionState.Open)
                    mSqlConn.Open();

                SqlDataAdapter adapter = new SqlDataAdapter(tSqlCmdu);
                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet);
                
                return dataSet;
            }
            catch (Exception Ex)
            {
                //MessageBox.Show("ExecuteTvp" + Ex.StackTrace + Ex.Message);
                return null;
            }



        }


        public void ExecuteTvpOptimize_batch(string USPName, string _Data, string _Line, ref string tReString, ref string tRetData)///导入数据
        {
            try
            {
                int Rows = 0;
                SqlCommand tSqlCmdu = mSqlConn.CreateCommand();
                tSqlCmdu.CommandType = CommandType.StoredProcedure;
                tSqlCmdu.CommandText = USPName;


                SqlParameter[] tSqlParmu = new SqlParameter[4];

                tSqlParmu[0] = new SqlParameter("@Optimize_batch", SqlDbType.VarChar , 100);
                tSqlParmu[1] = new SqlParameter("@Line", SqlDbType.VarChar, 100);
                //outv

                tSqlParmu[2] = new SqlParameter("@RetData", SqlDbType.VarChar, 500);
                tSqlParmu[3] = new SqlParameter("@RetStr", SqlDbType.VarChar, 500);


                //指明参数是输入还是输出型
                for (int i = 0; i < tSqlParmu.Length - 2; i++)
                {
                    tSqlParmu[i].Direction = ParameterDirection.Input;
                }
                for (int i = 2; i < tSqlParmu.Length; i++)
                {
                    tSqlParmu[i].Direction = ParameterDirection.Output;
                }


                //给参数赋值

                tSqlParmu[0].Value = _Data;
                tSqlParmu[1].Value = _Line;


                ////////parm[3].Value = OracleDateTime.Parse(m_dksrq.ToShortDateString());
                /////直接用update语句更新时,需要采用下面的日期格式.

                for (int i = 0; i < tSqlParmu.Length; i++)
                {
                    tSqlCmdu.Parameters.Add(tSqlParmu[i]);
                    //.Add(tSqlParm[i]);
                }


                if (mSqlConn.State != ConnectionState.Open)
                    mSqlConn.Open();

                //OrCmd.Connection = JHOracleConn;
                Rows = tSqlCmdu.ExecuteNonQuery();


                //取出返回值
                tReString = Convert.ToString(tSqlParmu[3].Value).Trim();//res
                tRetData  = Convert.ToString(tSqlParmu[2].Value).Trim();//res
            }
            catch (Exception Ex)
            {
                MessageBox.Show("ExecuteTvp" + Ex.StackTrace + Ex.Message);
            }



        }

        public DataSet  ExecuteDisposeStatus(string USPName, string _Wcsid, string _Data, ref string _RetData, ref string _RetStr)///处理更新数据
        {
            try
            {
               
                SqlCommand tSqlCmdu = mSqlConn.CreateCommand();
                tSqlCmdu.CommandType = CommandType.StoredProcedure;
                tSqlCmdu.CommandText = USPName;


                SqlParameter[] tSqlParmu = new SqlParameter[4];

                tSqlParmu[0] = new SqlParameter("@WcsID", SqlDbType.VarChar, 100);
                tSqlParmu[1] = new SqlParameter("@Data", SqlDbType.VarChar, 100);
                //outv

                tSqlParmu[2] = new SqlParameter("@RetData", SqlDbType.VarChar, 500);
                tSqlParmu[3] = new SqlParameter("@RetStr", SqlDbType.VarChar, 500);


                //指明参数是输入还是输出型
                for (int i = 0; i < tSqlParmu.Length - 2; i++)
                {
                    tSqlParmu[i].Direction = ParameterDirection.Input;
                }
                for (int i = 2; i < tSqlParmu.Length; i++)
                {
                    tSqlParmu[i].Direction = ParameterDirection.Output;
                }


                //给参数赋值

                tSqlParmu[0].Value = _Wcsid;
                tSqlParmu[1].Value = _Data;

                ////////parm[3].Value = OracleDateTime.Parse(m_dksrq.ToShortDateString());
                /////直接用update语句更新时,需要采用下面的日期格式.

                for (int i = 0; i < tSqlParmu.Length; i++)
                {
                    tSqlCmdu.Parameters.Add(tSqlParmu[i]);
                    //.Add(tSqlParm[i]);
                }


                if (mSqlConn.State != ConnectionState.Open)
                    mSqlConn.Open();

                //////OrCmd.Connection = JHOracleConn;
                ////Rows = tSqlCmdu.ExecuteNonQuery();


                //////取出返回值
                ////RetData = Convert.ToString(tSqlParmu[2].Value).Trim();//res
                ////RetStr = Convert.ToString(tSqlParmu[3].Value).Trim();//res
                SqlDataAdapter adapter = new SqlDataAdapter(tSqlCmdu);
                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet);

                //取出返回值
                _RetData = Convert.ToString(tSqlParmu[2].Value).Trim();//res
                _RetStr = Convert.ToString(tSqlParmu[3].Value).Trim();//res
                return dataSet;
            }
            catch (Exception Ex)
            {
                MessageBox.Show("ExecuteTvp" + Ex.StackTrace + Ex.Message);
                return null;
            }



        }
         


        /// <summary>
        /// 通用的存储过程
        /// </summary>
        /// <param name="_Proname">存储过程名称</param>  
        /// <param name="_Status">状态</param>
        /// <param name="_Plc_Flow"></param>
        /// <param name="_Long"></param>
        /// <param name="_Short"></param>
        /// <param name="RetData"></param>
        /// <param name="RetStr"></param>
        /// <param name="_Rows"></param>
        /// <returns></returns>
        public string ExecutePro(string _Proname, int _Status, string _Plc_Flow, int _Long, int _Short
                               , int _line , ref string RetData, ref string RetStr, ref int _Rows)//
        {
            try
            {
                int Rows = 0;
                SqlCommand tSqlCmdw = mSqlConn.CreateCommand();
                tSqlCmdw.CommandType = CommandType.StoredProcedure;
                tSqlCmdw.CommandText = _Proname;// "[Usp_containerPosition]";


                SqlParameter[] tSqlParmw = new SqlParameter[7];

                tSqlParmw[0] = new SqlParameter("@Status", SqlDbType.Int, 32);
                tSqlParmw[1] = new SqlParameter("@Plc_Flow", SqlDbType.VarChar, 50);
                tSqlParmw[2] = new SqlParameter("@Long", SqlDbType.Int, 32);
                tSqlParmw[3] = new SqlParameter("@Short", SqlDbType.Int, 32);
                tSqlParmw[4] = new SqlParameter("@Line", SqlDbType.VarChar, 32);
                //outv
                tSqlParmw[5] = new SqlParameter("@RetData", SqlDbType.VarChar, 200);
                tSqlParmw[6] = new SqlParameter("@RetStr", SqlDbType.VarChar, 200);


                //指明参数是输入还是输出型
                for (int i = 0; i < tSqlParmw.Length - 2; i++)
                {
                    tSqlParmw[i].Direction = ParameterDirection.Input;
                }
                for (int i = 5; i < tSqlParmw.Length; i++)
                {
                    tSqlParmw[i].Direction = ParameterDirection.Output;
                }


                //给参数赋值
                tSqlParmw[0].Value = _Status;
                tSqlParmw[1].Value = _Plc_Flow;
                tSqlParmw[2].Value = _Long;
                tSqlParmw[3].Value = _Short;
                tSqlParmw[4].Value = _line;

                for (int i = 0; i < tSqlParmw.Length; i++)
                {
                    tSqlCmdw.Parameters.Add(tSqlParmw[i]);

                }


                if (mSqlConn.State != ConnectionState.Open)
                    mSqlConn.Open();


                Rows = tSqlCmdw.ExecuteNonQuery();
                RetData = Convert.ToString(tSqlParmw[5].Value).Trim();
                RetStr = Convert.ToString(tSqlParmw[6].Value).Trim();
                _Rows = Rows;
                return "";

            }
            catch (Exception ex)
            {
                return ex.Message + " @" + ex.StackTrace;
            }






        }

        /// <summary>
        /// 通用的存储过程返回数据集
        /// </summary>
        /// <param name="_Proname">存储过程名称</param>  
        /// <param name="_Status">状态</param>
        /// <param name="_Plc_Flow"></param>
        /// <param name="_Long"></param>
        /// <param name="_Short"></param>
        /// <param name="RetData"></param>
        /// <param name="RetStr"></param>
        /// <param name="_Rows"></param>
        /// <returns></returns>
        public DataSet  ExecuteProDataSet(string _Proname, int _Status, string _Plc_Flow, int _Long, int _Short
                                 , ref string RetData, ref string RetStr, ref int _Rows)//
        {
            try
            {
                int Rows = 0;
                SqlCommand tSqlCmdw = mSqlConn.CreateCommand();
                tSqlCmdw.CommandType = CommandType.StoredProcedure;
                tSqlCmdw.CommandText = _Proname;// "[Usp_containerPosition]";


                SqlParameter[] tSqlParmw = new SqlParameter[6];

                tSqlParmw[0] = new SqlParameter("@Status", SqlDbType.Int, 32);
                tSqlParmw[1] = new SqlParameter("@Plc_Flow", SqlDbType.VarChar, 50);
                tSqlParmw[2] = new SqlParameter("@Long", SqlDbType.Int, 32);
                tSqlParmw[3] = new SqlParameter("@Short", SqlDbType.Int, 32);

                //outv
                tSqlParmw[4] = new SqlParameter("@RetData", SqlDbType.VarChar, 200);
                tSqlParmw[5] = new SqlParameter("@RetStr", SqlDbType.VarChar, 200);


                //指明参数是输入还是输出型
                for (int i = 0; i < tSqlParmw.Length - 2; i++)
                {
                    tSqlParmw[i].Direction = ParameterDirection.Input;
                }
                for (int i = 4; i < tSqlParmw.Length; i++)
                {
                    tSqlParmw[i].Direction = ParameterDirection.Output;
                }


                //给参数赋值
                tSqlParmw[0].Value = _Status;
                tSqlParmw[1].Value = _Plc_Flow;
                tSqlParmw[2].Value = _Long;
                tSqlParmw[3].Value = _Short;

                for (int i = 0; i < tSqlParmw.Length; i++)
                {
                    tSqlCmdw.Parameters.Add(tSqlParmw[i]);

                }


                if (mSqlConn.State != ConnectionState.Open)
                    mSqlConn.Open();


                //Rows = tSqlCmdw.ExecuteNonQuery();
                //RetData = Convert.ToString(tSqlParmw[4].Value).Trim();
                //RetStr = Convert.ToString(tSqlParmw[5].Value).Trim();
                //_Rows = Rows;
                //return "";
                SqlDataAdapter adapter = new SqlDataAdapter(tSqlCmdw);
                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet);
                _Rows = Rows;
                return dataSet;

            }
            catch (Exception ex)
            {
                return null;// ex.Message + " @" + ex.StackTrace;
            }
        }

        /// <summary>
        /// 获取入库指令
        /// </summary>
        /// <param name="_Proname"></param>
        /// <param name="_Status"></param>
        /// <param name="_tvp_Data"></param>
        /// <param name="_Long"></param>
        /// <param name="_Short"></param>
        /// <param name="_Export"></param>
        /// <param name="RetData"></param>
        /// <param name="RetStr"></param>
        /// <param name="_Rows"></param>
        /// <returns></returns>
        public string ExecuteProInLocation(string _Proname, int _Status, DataTable _tvp_Data, int _Long, int _Short, int _Export
                                 , ref string RetData, ref string RetStr, ref int _Rows)//
        {
            try
            {
                int Rows = 0;
                SqlCommand tSqlCmdw = mSqlConn.CreateCommand();
                tSqlCmdw.CommandType = CommandType.StoredProcedure;
                tSqlCmdw.CommandText = _Proname;// "[Usp_containerPosition]";


                SqlParameter[] tSqlParmw = new SqlParameter[7];

                tSqlParmw[0] = new SqlParameter("@Status", SqlDbType.Int, 32);
                tSqlParmw[1] = new SqlParameter("@tvp_Data", SqlDbType.Structured, 500);

                tSqlParmw[2] = new SqlParameter("@Long", SqlDbType.Int, 32);
                tSqlParmw[3] = new SqlParameter("@Short", SqlDbType.Int, 32);
                tSqlParmw[4] = new SqlParameter("@Export", SqlDbType.Int, 32);

                //outv
                tSqlParmw[5] = new SqlParameter("@RetData", SqlDbType.VarChar, 500);
                tSqlParmw[6] = new SqlParameter("@RetStr", SqlDbType.VarChar, 500);


                //指明参数是输入还是输出型
                for (int i = 0; i < tSqlParmw.Length - 2; i++)
                {
                    tSqlParmw[i].Direction = ParameterDirection.Input;
                }
                for (int i = 5; i < tSqlParmw.Length; i++)
                {
                    tSqlParmw[i].Direction = ParameterDirection.Output;
                }


                //给参数赋值
                tSqlParmw[0].Value = _Status;
                tSqlParmw[1].Value = _tvp_Data;
                tSqlParmw[2].Value = _Long;
                tSqlParmw[3].Value = _Short;
                tSqlParmw[4].Value = _Export;

                for (int i = 0; i < tSqlParmw.Length; i++)
                {
                    tSqlCmdw.Parameters.Add(tSqlParmw[i]);

                }


                if (mSqlConn.State != ConnectionState.Open)
                    mSqlConn.Open();


                Rows = tSqlCmdw.ExecuteNonQuery();
                RetData = Convert.ToString(tSqlParmw[5].Value).Trim();
                RetStr = Convert.ToString(tSqlParmw[6].Value).Trim();
                _Rows = Rows;
                return "";

            }
            catch (Exception ex)
            {
                return ex.Message + " @" + ex.StackTrace;
            }






        }

        /// <summary>
        /// 中空出入库返回数据集
        /// </summary>
        /// <param name="_Proname"></param>
        /// <param name="_Status"></param>
        /// <param name="_tvp_Data"></param>
        /// <param name="_Long"></param>
        /// <param name="_Short"></param>
        /// <param name="_Export"></param>
        /// <param name="RetData"></param>
        /// <param name="RetStr"></param>
        /// <param name="_Rows"></param>
        /// <returns></returns>
        public DataSet ExecuteProInLocationZK(string _Proname, int _Status, DataTable _tvp_Data, int _Long, int _Short, int _Export
                                , ref string RetData, ref string RetStr, ref int _Rows)//
        {
            try
            {
                int Rows = 0;
                SqlCommand tSqlCmdw = mSqlConn.CreateCommand();
                tSqlCmdw.CommandType = CommandType.StoredProcedure;
                tSqlCmdw.CommandText = _Proname;// "[Usp_containerPosition]";


                SqlParameter[] tSqlParmw = new SqlParameter[7];

                tSqlParmw[0] = new SqlParameter("@Status", SqlDbType.Int, 32);
                tSqlParmw[1] = new SqlParameter("@tvp_Data", SqlDbType.Structured, 500);

                tSqlParmw[2] = new SqlParameter("@Long", SqlDbType.Int, 32);
                tSqlParmw[3] = new SqlParameter("@Short", SqlDbType.Int, 32);
                tSqlParmw[4] = new SqlParameter("@Export", SqlDbType.Int, 32);

                //outv
                tSqlParmw[5] = new SqlParameter("@RetData", SqlDbType.VarChar, 500);
                tSqlParmw[6] = new SqlParameter("@RetStr", SqlDbType.VarChar, 500);


                //指明参数是输入还是输出型
                for (int i = 0; i < tSqlParmw.Length - 2; i++)
                {
                    tSqlParmw[i].Direction = ParameterDirection.Input;
                }
                for (int i = 5; i < tSqlParmw.Length; i++)
                {
                    tSqlParmw[i].Direction = ParameterDirection.Output;
                }


                //给参数赋值
                tSqlParmw[0].Value = _Status;
                tSqlParmw[1].Value = _tvp_Data;
                tSqlParmw[2].Value = _Long;
                tSqlParmw[3].Value = _Short;
                tSqlParmw[4].Value = _Export;

                for (int i = 0; i < tSqlParmw.Length; i++)
                {
                    tSqlCmdw.Parameters.Add(tSqlParmw[i]);

                }


                if (mSqlConn.State != ConnectionState.Open)
                    mSqlConn.Open();


                //Rows = tSqlCmdw.ExecuteNonQuery();
                //RetData = Convert.ToString(tSqlParmw[5].Value).Trim();
                //RetStr = Convert.ToString(tSqlParmw[6].Value).Trim();
                SqlDataAdapter adapter = new SqlDataAdapter(tSqlCmdw);
                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet);
                _Rows = Rows;
                return dataSet;

            }
            catch (Exception ex)
            {
                return null;
            }
        }
        /// <summary>
        /// 获取出库完成指令
        /// </summary>
        /// <param name="_Proname"></param>
        /// <param name="_Status"></param>
        /// <param name="_OutCmd_Flow"></param>
        /// <param name="_Flow1"></param>
        /// <param name="_Flow2"></param>
        /// <param name="_Flow3"></param>
        /// <param name="_Flow4"></param>
        /// <param name="RetData"></param>
        /// <param name="RetStr"></param>
        /// <param name="_Rows"></param>
        /// <returns></returns>
        public string ExecuteProFinishOut(string _Proname, int _Status, int _OutCmd_Flow, int _Flow1, int _Flow2, int _Flow3, int _Flow4
                                 , int _Long
                                 , ref string RetData, ref string RetStr, ref int _Rows)//
        {
            try
            {
                int Rows = 0;
                SqlCommand tSqlCmdw = mSqlConn.CreateCommand();
                tSqlCmdw.CommandType = CommandType.StoredProcedure;
                tSqlCmdw.CommandText = _Proname;// "[Usp_containerPosition]";


                SqlParameter[] tSqlParmw = new SqlParameter[9];

                tSqlParmw[0] = new SqlParameter("@Status", SqlDbType.Int, 32);
                tSqlParmw[1] = new SqlParameter("@OutCmd_Flow", SqlDbType.Int, 32);
                tSqlParmw[2] = new SqlParameter("@Flow1", SqlDbType.Int, 32);
                tSqlParmw[3] = new SqlParameter("@Flow2", SqlDbType.Int, 32);
                tSqlParmw[4] = new SqlParameter("@Flow3", SqlDbType.Int, 32);
                tSqlParmw[5] = new SqlParameter("@Flow4", SqlDbType.Int, 32);
                tSqlParmw[6] = new SqlParameter("@Line", SqlDbType.Int, 32);
                //outv
                tSqlParmw[7] = new SqlParameter("@RetData", SqlDbType.VarChar, 1000);
                tSqlParmw[8] = new SqlParameter("@RetStr", SqlDbType.VarChar, 200);


                //指明参数是输入还是输出型
                for (int i = 0; i < tSqlParmw.Length - 2; i++)
                {
                    tSqlParmw[i].Direction = ParameterDirection.Input;
                }
                for (int i = 7; i < tSqlParmw.Length; i++)
                {
                    tSqlParmw[i].Direction = ParameterDirection.Output;
                }


                //给参数赋值
                tSqlParmw[0].Value = _Status;
                tSqlParmw[1].Value = _OutCmd_Flow;
                tSqlParmw[2].Value = _Flow1;
                tSqlParmw[3].Value = _Flow2;
                tSqlParmw[4].Value = _Flow3;
                tSqlParmw[5].Value = _Flow4;
                tSqlParmw[6].Value = _Long;

                for (int i = 0; i < tSqlParmw.Length; i++)
                {
                    tSqlCmdw.Parameters.Add(tSqlParmw[i]);

                }


                if (mSqlConn.State != ConnectionState.Open)
                    mSqlConn.Open();


                Rows = tSqlCmdw.ExecuteNonQuery();
                RetData = Convert.ToString(tSqlParmw[7].Value).Trim();
                RetStr = Convert.ToString(tSqlParmw[8].Value).Trim();
                _Rows = Rows;
                return "";

            }
            catch (Exception ex)
            {
                return ex.Message + " @" + ex.StackTrace;
            }
        }





        ////原片仓储
        public DataSet ExecuteBYuan(string _Proname, int _Status,int _PlcFlow,string _DataStr, DataTable _tvp_Data, string  _Line
                                , ref string RetData, ref string RetStr, ref int _Rows)//
        {
            try
            {
                int Rows = 0;
                SqlCommand tSqlCmdw = mSqlConn.CreateCommand();
                tSqlCmdw.CommandType = CommandType.StoredProcedure;
                tSqlCmdw.CommandText = _Proname;// "[Usp_containerPosition]";


                SqlParameter[] tSqlParmw = new SqlParameter[7];

                tSqlParmw[0] = new SqlParameter("@Status", SqlDbType.Int, 32);
                tSqlParmw[1] = new SqlParameter("@Plc_Flow", SqlDbType.VarChar, 32);
                tSqlParmw[2] = new SqlParameter("@DataStr", SqlDbType.VarChar, 200);
                tSqlParmw[3] = new SqlParameter("@tvp_Data", SqlDbType.Structured, 500);
                tSqlParmw[4] = new SqlParameter("@Line", SqlDbType.Int, 32);
                //outv
                tSqlParmw[5] = new SqlParameter("@RetData", SqlDbType.VarChar, 500);
                tSqlParmw[6] = new SqlParameter("@RetStr", SqlDbType.VarChar, 500);


                //指明参数是输入还是输出型
                for (int i = 0; i < tSqlParmw.Length - 2; i++)
                {
                    tSqlParmw[i].Direction = ParameterDirection.Input;
                }
                for (int i = 5; i < tSqlParmw.Length; i++)
                {
                    tSqlParmw[i].Direction = ParameterDirection.Output;
                }


                //给参数赋值
                tSqlParmw[0].Value = _Status;
                tSqlParmw[1].Value = _PlcFlow.ToString()  ;
                tSqlParmw[2].Value = _DataStr ;
                tSqlParmw[3].Value = _tvp_Data;
                tSqlParmw[4].Value = _Line;

                for (int i = 0; i < tSqlParmw.Length; i++)
                {
                    tSqlCmdw.Parameters.Add(tSqlParmw[i]);

                }

                if (mSqlConn.State != ConnectionState.Open)
                    mSqlConn.Open();


                //Rows = tSqlCmdw.ExecuteNonQuery();
                //RetData = Convert.ToString(tSqlParmw[5].Value).Trim();
                //RetStr = Convert.ToString(tSqlParmw[6].Value).Trim();
                SqlDataAdapter adapter = new SqlDataAdapter(tSqlCmdw);
                DataSet dataSet = new DataSet();
                adapter.Fill(dataSet);
                _Rows = Rows;
                return dataSet;

            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }


}
