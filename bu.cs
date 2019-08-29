using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Xml;

namespace AutoAPP
{
    class bu
    {
        private Log log = new Log();
        private ThreadProcess runner ;
        private ThreadProcessEXFO runnerEXFO;
        private ThreadProcessFoxconn runnerFoxconn;
        private ThreadProcessJUFEI runnerJUEFEI;  // add by Peng 20180321
        private ThreadProcessLHFO runnerLHFO;     // add by Peng 20180321
        private ThreadProcessCDFO runnerCDFO  ;     // add by Peng 20180325
        


        private static readonly string RS_FORMAT_DATETIME = "yyyy-MM-dd HH:mm:ss";
        public static readonly string RS_Format_DateTimeFileName = "yyyyMMddHHmmss";
        private string ThreadName = "DeleteData";

        /// <summary>
        /// 
        /// </summary>
        public class ZZFoxPDB
        {
            public static readonly string RS_EXPRT_TITLE = " Date,Time,Acount,GoodAcount,FileAcount,CapacityOfHour,PreGood,RunTime,WaitTime,cyclesTime,BadTime";
            public static readonly string RS_DATE_FORMAT = "yyyyMMdd";
            public static readonly string RS_TIME_FORMAT = "HHmmss";
            public static readonly string RS_SPIT = ",";
            public static readonly string RS_UNLINE = "\r\n";
            public static readonly string RS_DATE_FORMAT_COV = "yyyy-MM-dd";
            public static readonly string lineUnder = "\r\n";
            public static readonly string RS_STR_FORMAT_4F = "0.0000";
            public static readonly string RS_ZERO = "0";
            public static readonly string RS_PER = "%";
        }

        /// <summary>
        ///  连接测试
        /// </summary>
        /// <param name="strMySQLConnect"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public string TestConnection(string strMySQLConnect, Log log)
        {
            try
            {
                string strReturn = "连接测试成功！";
                MySqlConnection conn = new MySqlConnection(strMySQLConnect);
                try
                {
                    if (conn.State != System.Data.ConnectionState.Open)
                    {
                        conn.Open();
                    }
                    MySqlCommand cmd = conn.CreateCommand();
                    cmd.CommandText = "  SELECT * from tbpadmeasure  LIMIT 1  ";
                    MySqlDataReader reader = cmd.ExecuteReader();
                }
                catch (System.Exception ex)
                {
                    strReturn = strMySQLConnect.ToString() + "连接出错：" + ex.Message.ToString();
                    log.WriteErr("错误 ! " + strReturn, "Main");
                }

                log.WriteLog(strReturn, "Main");
                return strReturn;

            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public string TestConnectionFox(string strMysqlConn, Log log)
        {
            try
            {
                string strReturn = "测试未链接";
                MySqlConnection conn = new MySqlConnection(strMysqlConn);
                try
                {
                    if (conn.State != System.Data.ConnectionState.Open)
                    {
                        conn.Open();
                    }
                }
                catch (System.Exception ex)
                {
                    strReturn = strMysqlConn.ToString() + "连接出错：" + ex.Message.ToString();
                    log.WriteErr("错误 ! " + strReturn, "Main");
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                        strReturn = "测试链接成功!";
                    }
                }
                log.WriteLog(strReturn, "Main");
                return strReturn;

            }
            catch (System.Exception ex)
            {
                throw ex;
            }

        }
        /// <summary>
        /// 线程开始
        /// </summary>
        /// <param name="dtStart"></param>
        /// <param name="dtEnd"></param>
        /// <param name="log"></param>
        public void AutoStart(DateTime dtStart, DateTime dtEnd, bool blnBackUpData, string strBackFilePath, Log log)
        {
            try
            {
                runner = new ThreadProcess(dtStart, dtEnd, blnBackUpData, strBackFilePath, log);
                runner.Run();
            }
            catch (Exception ex)
            {
                log.WriteErr("错误 ! " + ex.ToString(), ThreadName);
            }
        }

        public void AutoStart(DateTime dtEndTime, bool blnBackUpData, string strBackFilePath, Log log)
        {
            try
            {
                runner = new ThreadProcess(dtEndTime, blnBackUpData, strBackFilePath, log);
                runner.Run();
            }
            catch (Exception ex)
            {
                log.WriteErr("错误 ! " + ex.ToString(), ThreadName);
            }

        }

        /// <summary>
        /// 更新配置文件
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void UpdateConfigKey(string key, string value)
        {

            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(System.Windows.Forms.Application.ExecutablePath + ".config");

            XmlNode xNode;
            XmlElement xElem1;
            XmlElement xElem2;
            XmlElement xElem3;

            xNode = xDoc.SelectSingleNode("//applicationSettings");

            xElem1 = (XmlElement)xNode.SelectSingleNode("//setting[@name='" + key + "']");
            if (xElem1 != null)
                xElem1.FirstChild.InnerText = value;
            //xElem1.SetAttribute("value", value);
            else
            {
                xElem2 = xDoc.CreateElement("setting");
                xElem2.SetAttribute("name", key);
                xElem2.SetAttribute("serializeAs", "String");
                xNode.AppendChild(xElem2);

                xElem3 = xDoc.CreateElement("value");
                xElem3.InnerText = value;
                xElem2.AppendChild(xElem3);
            }
            xDoc.Save(System.Windows.Forms.Application.ExecutablePath + ".config");
        }

        /// <summary>
        /// stop
        /// </summary>
        /// <returns></returns>
        public Boolean Stop()
        {
            Boolean blnReturn = true;
            runnerCDFO = new ThreadProcessCDFO(log);
            runnerLHFO = new ThreadProcessLHFO(log);
            
            try
            {
                if (runner != null)
                    if (runner.Running)
                    {
                        // 确认是否停止
                        //if (MessageBox.Show("后台线程正在执行，是否结束线程并退出？", "确认", MessageBoxButtons.OKCancel) != DialogResult.OK)
                        //{
                        runner.Stop();
                        blnReturn = false;
                        //}
                        //else
                        //{
                        //    blnReturn = true;
                        //}
                    }
                if (runnerEXFO != null)
                    if (runnerEXFO.Running)
                    {
                        runnerEXFO.Stop();
                        blnReturn = false;
                    }
            
                //add by Peng 20180321 
                if(runnerFoxconn !=null)
                    if (runnerFoxconn.Running)
                    {
                        runnerFoxconn.Stop();
                        blnReturn = false;
                    }
                if(runnerJUEFEI != null)
                    if (runnerJUEFEI.Running)
                    {
                        runnerJUEFEI.Stop();
                        blnReturn = false;
                    }
               
                    runnerLHFO.Stop();                                  
                    runnerCDFO.Stop();
                    blnReturn = false;
                                            
                             
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return blnReturn;
        }
        /// <summary>
        /// 删除intDays天前的数据 不删除job信息. 
        ///  删除表TBBoard和TBPadMeasure和TbRealTimeResources三个表数据.
        /// </summary>
        /// <param name="stSPCDeleteData"></param>
        /// <returns></returns>
        public int DeleteAndBackupDataByDays(DateTime dtStartTime, DateTime dtEndTime, int intSPCDataBaseType, string strConnect, bool blnBackUpData, string strBackFilePath, Log log)
        {
            MySqlConnection connMySQL = new MySqlConnection(strConnect);
            MySqlCommand cmdMySQL = new MySqlCommand();
          //  MySqlTransaction tranMySQL;
            MySqlDataAdapter daptMySQL = new MySqlDataAdapter();
            DataTable dtReturn = new DataTable();

            int intReturn = 0;
            string strBackUpPadMeasureSQL = string.Empty;
            string strBackUpBoardSQL = string.Empty;
          //  string strBackUpRealTimeResourcesSQL = string.Empty;
            string strBackUpFile = string.Empty;
            string strPCBID = string.Empty;

            string strDeletePadMeasureSQL = string.Empty;
            string strDeleteBoardSQL = string.Empty;
           // string strDeleteRealTimeResourcesSQL = string.Empty;

            try
            {

                switch (intSPCDataBaseType)
                {
                    case 2:
                        {
                            ////         string strDeleteSimplePadSQL = @" DELETE FROM spidb11.TBSimplePad WHERE padid in (
                            //      SELECT a.PadID FROM spidb11.TBPadMeasure a  INNER JOIN  spidb11.TBBoard b on a.PCBID =b.PCBID 
                            //      WHERE b.StartTime <=  '" + dtEndTime.ToString(RS_FORMAT_DATETIME) + "'  and  b.StartTime >=  '" + dtStartTime.ToString(RS_FORMAT_DATETIME) + "'  );  ";


                            string strSelectPCBID = " SELECT   PCBID  FROM spidb11.TBBoard WHERE StartTime >= '" + dtStartTime.ToString(RS_FORMAT_DATETIME) + "'  and StartTime <= '" + dtEndTime.ToString(RS_FORMAT_DATETIME) + "'";

                            if (connMySQL.State != ConnectionState.Open)
                            {
                                connMySQL.Open();
                            }
                            cmdMySQL = connMySQL.CreateCommand();
                            cmdMySQL.CommandTimeout = Properties.Settings.Default.SQLTimeOut;
                            cmdMySQL.CommandText = strSelectPCBID;
                            daptMySQL.SelectCommand = cmdMySQL;
                            daptMySQL.Fill(dtReturn);




                            foreach (DataRow dr in dtReturn.Rows)
                            {
                                //  strPCBID += dr[0].ToString() + ",";
                                strPCBID = dr[0].ToString() + ",";
                                //   }

                                if (strPCBID.Length > 1)
                            {
                                strPCBID = strPCBID.Remove(strPCBID.Length - 1, 1);
                                strDeletePadMeasureSQL = string.Format("DELETE FROM spidb11.TBPadMeasure WHERE 	PCBID IN ({0});  ", strPCBID);

                                    strDeleteBoardSQL = string.Format(" DELETE FROM	  spidb11.TBBoard  WHERE PCBID IN ({0});  ", strPCBID);
                            }

                                //     strDeleteBoardSQL = @" DELETE FROM	  spidb11.TBBoard WHERE StartTime >=  '" + dtStartTime.ToString(RS_FORMAT_DATETIME) + "'  and  StartTime <=  '" + dtEndTime.ToString(RS_FORMAT_DATETIME) + "'  ;  ";
                              

                                //  strDeleteRealTimeResourcesSQL = @" DELETE FROM spidb11.TbRealTimeResources 
                                //       WHERE InsertTime >= '" + dtStartTime.ToString(RS_FORMAT_DATETIME) + "' and  InsertTime <= '" + dtEndTime.ToString(RS_FORMAT_DATETIME) + "'  ; ";

                                if (blnBackUpData)
                            {

                                    strBackUpPadMeasureSQL = @" SELECT  *  FROM spidb11.TBPadMeasure WHERE 	PCBID IN ({0}) into OUTFILE '{1}'   ;";

                                if (!Directory.Exists(strBackFilePath))
                                    Directory.CreateDirectory(strBackFilePath);

                                strBackUpFile = Path.Combine(strBackFilePath, "TBPadMeasure" + "_" + strPCBID + "_" + DateTime.Now.ToString(RS_Format_DateTimeFileName) + ".backup");

                                if (File.Exists(strBackUpFile))
                                {
                                    strBackUpFile = Path.Combine(strBackFilePath, "TBPadMeasure" + "_" + strPCBID + "_" + DateTime.Now.AddSeconds(1).ToString(RS_Format_DateTimeFileName) + ".backup");
                                }

                                strBackUpFile = strBackUpFile.Replace("\\", "\\\\");
                                log.WriteLog("TBPadMeasure备份文件:" + strBackUpFile, ThreadName);
                                strBackUpPadMeasureSQL = string.Format(strBackUpPadMeasureSQL,strPCBID, strBackUpFile);

                                    // strBackUpBoardSQL = @" SELECT  *  FROM	spidb11.TBBoard   into OUTFILE '{0} '    ;  ";
                                    //strBackUpBoardSQL = @" SELECT  *  FROM	spidb11.TBBoard WHERE StartTime >=  '" + dtStartTime.ToString(RS_FORMAT_DATETIME) + "'  and  StartTime <=  '" + dtEndTime.ToString(RS_FORMAT_DATETIME) + "' into OUTFILE '{0}' ;  ";
                                    strBackUpBoardSQL = @" SELECT  *  FROM	spidb11.TBBoard  WHERE 	PCBID IN ({0})  into OUTFILE '{1}'   ; ";


                                    strBackUpFile = Path.Combine(strBackFilePath, "TBBoard" +"_"+strPCBID+ "_" + DateTime.Now.ToString(RS_Format_DateTimeFileName) + ".backup");

                                if (File.Exists(strBackUpFile))
                                {
                                    strBackUpFile = Path.Combine(strBackFilePath, "TBBoard" + "_" + strPCBID + "_" + DateTime.Now.AddSeconds(1).ToString(RS_Format_DateTimeFileName) + ".backup");
                                }
                                strBackUpFile = strBackUpFile.Replace("\\", "\\\\");
                                log.WriteLog("TBBoard备份文件:" + strBackUpFile, ThreadName);
                                strBackUpBoardSQL = string.Format(strBackUpBoardSQL,strPCBID, strBackUpFile);

                                //strBackUpRealTimeResourcesSQL = @" SELECT  *  FROM spidb11.TbRealTimeResources 
                                //WHERE InsertTime >= '" + dtStartTime.ToString(RS_FORMAT_DATETIME) + "' and  InsertTime <= '" + dtEndTime.ToString(RS_FORMAT_DATETIME) + "' into OUTFILE '{0}'    ; ";

                                //strBackUpFile = Path.Combine(strBackFilePath, "TbRealTimeResources" + "_" + DateTime.Now.ToString(RS_Format_DateTimeFileName) + ".backup");

                                //if (File.Exists(strBackUpFile))
                                //{
                                //    strBackUpFile = Path.Combine(strBackFilePath, "TbRealTimeResources" + "_" + DateTime.Now.AddSeconds(1).ToString(RS_Format_DateTimeFileName) + ".backup");
                                //}
                                //strBackUpFile = strBackUpFile.Replace("\\", "\\\\");
                                //log.WriteLog("TbRealTimeResources备份文件:" + strBackUpFile, ThreadName);
                                //strBackUpRealTimeResourcesSQL = string.Format(strBackUpRealTimeResourcesSQL, strBackUpFile);
                            }

                            if (connMySQL.State != ConnectionState.Open)
                            {
                                connMySQL.Open();
                            }
                            // tranMySQL = connMySQL.BeginTransaction();
                            cmdMySQL = connMySQL.CreateCommand();
                            //  cmdMySQL.Transaction = tranMySQL;
                            cmdMySQL.CommandTimeout = Properties.Settings.Default.SQLTimeOut;
                            try
                            {
                                cmdMySQL.CommandText = " START TRANSACTION;";
                                cmdMySQL.CommandText += strBackUpPadMeasureSQL;
                                cmdMySQL.CommandText += strBackUpBoardSQL;
                               // cmdMySQL.CommandText += strBackUpRealTimeResourcesSQL;

                                //cmdMySQL.CommandText = strDeleteSimplePadSQL;
                                //intReturn += cmdMySQL.ExecuteNonQuery();
                                cmdMySQL.CommandText += strDeletePadMeasureSQL;
                                //   intReturn += cmdMySQL.ExecuteNonQuery();
                                cmdMySQL.CommandText += strDeleteBoardSQL;
                                //     intReturn += cmdMySQL.ExecuteNonQuery();
                             //   cmdMySQL.CommandText += strDeleteRealTimeResourcesSQL;
                                cmdMySQL.CommandText += " COMMIT;";

                                log.WriteLog(cmdMySQL.CommandText, ThreadName);

                                if (connMySQL.State != ConnectionState.Open)
                                {
                                    connMySQL.Open();
                                }
                                intReturn += cmdMySQL.ExecuteNonQuery();
                                //     tranMySQL.Commit();
                                //  "START TRANSACTION;" + SQL + "COMMIT;";

                            }
                            catch (Exception ex)
                            {
                                intReturn = -1;
                                //   if (connMySQL.State != ConnectionState.Open)k
                                //   {
                                //       connMySQL.Open();
                                //   }
                                ////   tranMySQL.Rollback();
                                log.WriteErr("错误 ! " + ex.ToString(), ThreadName);
                            }
                            finally
                            {
                                    log.WriteLog("删除了"+strPCBID+"下的"+intReturn.ToString()+"条记录!");
                                    //   tranMySQL.Dispose();
                                }
                                log.WriteLog("Sleep(1000)");
                                System.Threading.Thread.Sleep(1000);
                        }
                            break;
                        }

                    default:
                        break;
                }
                return intReturn;
            }
            catch (Exception ex)
            {
                log.WriteErr("错误 ! " + ex.ToString(), ThreadName);
                return -1;
            }
            finally
            {
                if (connMySQL.State == ConnectionState.Open)
                    connMySQL.Close();
                cmdMySQL.Dispose();
                daptMySQL.Dispose();
            }
        }


        //LOAD DATA INFILE "/path/to/file" INTO TABLE table_name;
        //SELECT * FROM TBBoard  INTO OUTFILE "d:/1.sql"

        public int LoadDataFromFile(string strTableName, string strBackUpFile, string strConnect, Log log)
        {
            MySqlConnection connMySQL = new MySqlConnection(strConnect);
            MySqlCommand cmdMySQL = new MySqlCommand();
            int intReturn = 0;
            try
            {
                if (File.Exists(strBackUpFile))
                {
                    if (string.IsNullOrEmpty(strTableName))
                    {
                        strTableName = Path.GetFileNameWithoutExtension(strBackUpFile).Split('_')[0];
                    }

                    strBackUpFile = strBackUpFile.Replace("\\", "\\\\");
                    string strSQL = string.Format("LOAD DATA INFILE \"{0}\" INTO TABLE {1};", strBackUpFile, strTableName);

                    connMySQL.Open();
                    cmdMySQL = connMySQL.CreateCommand();
                    cmdMySQL.CommandTimeout = Properties.Settings.Default.SQLTimeOut;
                    cmdMySQL.CommandText = strSQL;
                    intReturn += cmdMySQL.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                log.WriteErr(strBackUpFile + "Load Data Exception :" + ex.ToString());
                throw ex;
            }
            finally
            {
                if (connMySQL.State == ConnectionState.Open)
                    connMySQL.Close();
                cmdMySQL.Dispose();
            }
            return intReturn;

        }



        #region @"爱斯福 MES         " 

        ///// 定期写入对方数据库中指定的表.  表内容字段麻烦工程师跟客户确认下. 看需要什么数据
        /////表结构和用户名密码 我们确定后提供给客户
        /////数据库格式可能会升级, 建议用SQL SERVER 2012 测试.      


        /// <summary>
        /// 开始 爱斯福 MES进程
        /// </summary>
        /// <param name="dtStartTime"></param>
        /// <param name="dtEndTime"></param>
        /// <param name="log"></param>
        public void AutoStartEXFO(DateTime dtStartTime, DateTime dtEndTime, Log log)
        {
            try
            {
                runnerEXFO = new ThreadProcessEXFO(dtStartTime, dtEndTime, log);
                runnerEXFO.Run();
            }
            catch (Exception ex)
            {
                log.WriteErr("错误 ! " + ex.ToString(), ThreadName);
            }

        }

        /// <summary>
        /// 获取SPCData报表数据  
        /// </summary>
        /// <param name="AsDatabaseIP"></param>
        /// <param name="AsDatabaseName"></param>
        /// <param name="AsDataTableName"></param>
        /// <param name="AsUserName"></param>
        /// <param name="AsPassWord"></param>
        /// <param name="dtStartTime"></param>
        /// <param name="dtEndTime"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public DataTable GetSPCDataForEXFO(string AsSQLConnectString, string AsDataTableName, DateTime dtStartTime, DateTime dtEndTime, Log log)
        {

            String strGetSPCDataSQL = @"SELECT
	                    b.JobName,
	                    b.JobVersion,
	                    a.PCBID,
	                    c.BarCode AS PCBBarcode,
	                    a.PadID,
	                    e.Shape as ShapeType,
	                    e.ShapeID,
	                    d.PosXmm AS `PosX(mm)`,
	                    d.PosYmm AS `PosY(mm)`,
	                    d.SizeXmm AS `SizeX(mm)`,
	                    d.SizeYmm AS `SizeY(mm)`,
	                    d.Rotation,
	                    a.ABSArea AS `ABSArea(mm2)`,
	                    a.ABSVolume AS `ABSVolume(mm3)`,
	                    a.ABSHeight AS `ABSHeight(mm)`,
	                    a.PerArea AS `PerArea(%)`,
	                    a.PerVolume AS `PerVolume(%)`,
	                    a.PerHeight AS `PerHeight(%)`,
	                    a.ShiftX AS `ShiftX(mm)`,
	                    a.ShiftX / d.SizeXmm AS `PerShiftX(%)`,
	                    a.ShiftY AS `ShiftY(mm)`,
	                    a.ShiftY / d.SizeYmm AS `PerShiftY(%)`,
	                    a.JudgeRes,
	                    a.DefectType,
	                    1 AS `Check`,
	                    f.CheckArea,
	                    f.AreaU AS `AreaU(%)`,
	                    f.AreaL AS `AreaL(%)`,
	                    f.CheckVol,
	                    f.VolU AS `VolU(%)`,
	                    f.VolL AS `VolL(%)`,
	                    f.CheckHeight,
	                    f.HeightU AS `HeightU(mm)`,
	                    f.HeightL AS `HeightL(mm)`,
	                    f.CheckOffset,
	                    f.ShiftXU AS `ShiftXU(mm)`,
	                    f.ShiftYU AS `ShiftYU(mm)`,
	                    f.CheckShape,
	                    f.ShapeDeltaU AS `ShapeDeltaU(mm)`,
	                    f.CheckStencilHeight,
	                    f.StencilHeight AS `StencilHeight(mm)`,
	                    f.CheckBridge,
	                    f.BridgeWidth AS `BridgeWidth(mm)`,
	                    f.BridgeLength AS `BridgeLength(mm)`,
	                    f.BridgeHeight AS `BridgeHeight(mm)`,
	                    d.ComponentID,
	                    d.PackageType,
	                    d.PinNumber,
	                    d.PadGroup,
	                    d.ArrayID,
	                    g.Squeege AS `Squeege`,
	                    g.SqueegeID AS `SqueegeID`,
	                    g.LotNo AS `LotNo`,
	                    c.ArrayBarCode AS `ArrayBarcode`,
	                    a.LineNo AS `LineNo`,
	                    g.InspectTimeStamp AS `InspectTimeStamp`,
	                    '0' AS `ComponentIDAverageHeight(mm)`
                    FROM 	TBPadMeasure a
                    INNER JOIN TBJobInfo b ON a.JobIndex = b.SerNo
                    LEFT JOIN TBBarCode c ON c.PCBID = a.PCBID
                    INNER JOIN TBSimplePad d ON d.PadID = a.PadID  AND d.IndexSerNo = a.PadIndex  AND d.JobIndex = a.JobIndex
                    INNER JOIN TBShape e ON e.ShapeID = d.ShapeID  AND e.JobIndex = a.JobIndex  AND e.IndexSerNo = d.ShapeIDIndex
                    INNER JOIN TBPadConditionParams f ON f.JobIndex = a.JobIndex  AND f.IndexSerNo = d.PadCndtParamsIndex
                    INNER JOIN TBBoard g ON g.JobIndex = a.JobIndex  AND g.PCBID = a.PCBID  AND g.LineNo = a.LineNo
                    where  1=1 and  g.StartTime >=  '{0}'  and  g.StartTime <=  '{1}';  ";

            DataTable dtReturn = new DataTable();

            try
            {
                dtReturn = GetDataTableFromEXFO(AsSQLConnectString, AsDataTableName, log);
                string strMySQLConn = Properties.Settings.Default.MySQLConnect;
                if (dtReturn != null)
                {
                    strGetSPCDataSQL = string.Format(strGetSPCDataSQL, dtStartTime.ToString(RS_FORMAT_DATETIME), dtEndTime.ToString(RS_FORMAT_DATETIME));
                    MySqlConnection conn = new MySqlConnection(strMySQLConn);

                    MySqlCommand cmd = conn.CreateCommand();
                    MySqlDataAdapter daptMySQL = new MySqlDataAdapter();
                    if (conn.State != ConnectionState.Open)
                    {
                        conn.Open();
                    }
                    cmd = conn.CreateCommand();
                    cmd.CommandTimeout = 600;
                    cmd.CommandText = strGetSPCDataSQL;
                    daptMySQL.SelectCommand = cmd;
                    daptMySQL.Fill(dtReturn);
                }
                return dtReturn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        /// <summary>
        /// 获取SPCData报表结构
        /// </summary>
        /// <param name="AsDatabaseIP"></param>
        /// <param name="AsDatabaseName"></param>
        /// <param name="AsDataTableName"></param>
        /// <param name="AsUserName"></param>
        /// <param name="AsPassWord"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public DataTable GetDataTableFromEXFO(string AsSQLConnectString, string AsDataTableName, Log log)
        {

            string strSQL = "  SELECT * from {0}  where 1=2 ";
            string strReturn = "链接测试成功";
            SqlConnection conn = new SqlConnection();
            SqlCommand cmd = conn.CreateCommand();
            SqlDataAdapter daptMySQL = new SqlDataAdapter();
            DataTable dt = null;
            try
            {
                strSQL = string.Format(strSQL, AsDataTableName);
                conn.ConnectionString = AsSQLConnectString;

                if (!string.IsNullOrEmpty(strSQL))
                {
                    if (conn.State != ConnectionState.Open)
                    {
                        conn.Open();
                    }
                    cmd = conn.CreateCommand();
                    cmd.CommandTimeout = 600;
                    cmd.CommandText = strSQL;
                    daptMySQL.SelectCommand = cmd;
                    dt = new DataTable();
                    dt.TableName = AsDataTableName;
                    daptMySQL.Fill(dt);

                }
                log.WriteErr(strReturn, "链接测试");
                return dt;

            }
            catch (System.Exception ex)
            {

                strReturn = AsSQLConnectString.ToString() + "连接出错：" + ex.Message.ToString();
                log.WriteErr("错误 ! " + strReturn, "链接测试");
                return null;
            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                }
                conn.Dispose();
                cmd.Dispose();
            }
        }

        /// <summary>
        /// SqlBulk 批量插入SQL SERVER  add by joch
        ///  /*  调用方法
        ///  //DataTable dt = Get_All_RoomState_ByHID();
        ///  //SqlBulkCopyColumnMapping[] mapping = new SqlBulkCopyColumnMapping[4];
        ///  //mapping[0] = new SqlBulkCopyColumnMapping("Xing_H_ID", "Xing_H_ID");
        ///  //mapping[1] = new SqlBulkCopyColumnMapping("H_Name", "H_Name");
        ///  //mapping[2] = new SqlBulkCopyColumnMapping("H_sName", "H_sName");
        ///  //mapping[3] = new SqlBulkCopyColumnMapping("H_eName", "H_eName");
        ///  //BulkToDB(dt,  mapping);
        ///  //*/
        /// </summary>
        /// <param name="AsIPAddress"></param>
        /// <param name="AsDBName"></param>
        /// <param name="AsUserName"></param>
        /// <param name="AsPassword"></param>
        /// <param name="Adt"></param>
        /// <param name="mapping"></param>
        public void BulkToDB(string AsSQLConnectString, DataTable Adt, SqlBulkCopyColumnMapping[] mapping)
        {
            string strError = string.Empty;
            try
            {
                if (Adt == null || string.IsNullOrEmpty(Adt.TableName) || Adt.Rows.Count == 0)
                    throw new Exception("DataTable Params Error");

                SqlConnection conn = new SqlConnection(AsSQLConnectString);
                if (conn.State != ConnectionState.Open)
                {
                    conn.Open();
                }

                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(conn))
                {
                    bulkCopy.BulkCopyTimeout = 600;
                    bulkCopy.DestinationTableName = Adt.TableName;
                    bulkCopy.BatchSize = Adt.Rows.Count;

                    if (Adt != null && Adt.Rows.Count != 0)
                    {
                        if (mapping != null && mapping.Length > 0)
                        {
                            for (int i = 0; i < mapping.Length; i++)
                                bulkCopy.ColumnMappings.Add(mapping[i]);
                        }
                        bulkCopy.WriteToServer(Adt);
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }
        #endregion

        #region @"富士康保存CSV文件         " 

        /// <summary>
        /// 开始 爱斯福 MES进程
        /// </summary>
        /// <param name="dtStartTime"></param>
        /// <param name="dtEndTime"></param>
        /// <param name="log"></param>
        public void AutoStartFoxconn(DateTime dtStartTime, DateTime dtEndTime,String strCVSFile, int AintSectionTime, Log log)
        {
            try
            {
                runnerFoxconn = new ThreadProcessFoxconn(dtStartTime, dtEndTime, strCVSFile, AintSectionTime, log);
                runnerFoxconn.Run();
            }
            catch (Exception ex)
            {
                log.WriteErr("错误 ! " + ex.ToString(), ThreadName);
            }

        }

        /// <summary>
        ///  ExportZZFox
        /// </summary>
        /// <param name="AstrDir"></param>
        /// <param name="AstartTime"></param>
        /// <param name="AsendTime"></param>
        /// <param name="AintSectionTime"></param>
        public static void ExportZZFoxconnInformat(
            string AsMySqlConnectionString,
            string AstrDir,
            DateTime AstartTime,   //界面设置导出开始时间
            DateTime AsendTime,    //界面设置导出结束时间
            int AintSectionTime,   //界面设置导出区域时间 单位 /分
            Log log)      
        {
            string sqlFx = "";
            string sqlFb = "", sqlFs = "", sqlFsr = "";
            string C_AOI = "AOI";
            int sec = AintSectionTime;
            string timeFile = AstrDir + "\\" + "timeTmp.txt";
            try
            {
                System.Text.StringBuilder strbld = new System.Text.StringBuilder();
                if (!string.IsNullOrEmpty(AstrDir)
                    && AintSectionTime != 0
                    )
                {                    
                    //间隔多少秒
                    //TimeSpan ts = new TimeSpan(0, 0, AintSectionTime * 60);
                    //开始时间 结束时间差
                    //int es = (int)AstartTime.Subtract(AsendTime).Duration().TotalSeconds;
                    //int multiple = 0;
                    //if (es % (AintSectionTime * 60) == 0) multiple = es / (AintSectionTime * 60);
                    //else multiple = es / (AintSectionTime * 60) + 1;
                    sqlFx = "SELECT t.Customer,t.Factory,t.Floor,t.Line,t.EquipID,t.EquipName,t.Module " +
                            " FROM TBEquipStatus t " +
                            " WHERE t.UpdateTime >=STR_TO_DATE('" + AstartTime.ToString(ZZFoxPDB.RS_DATE_FORMAT + ZZFoxPDB.RS_TIME_FORMAT) + "','%Y%m%d%H%i%S') " +
                            " and t.UpdateTime <=STR_TO_DATE('" + AsendTime.ToString(ZZFoxPDB.RS_DATE_FORMAT + ZZFoxPDB.RS_TIME_FORMAT) + "','%Y%m%d%H%i%S')" +
                            " GROUP BY t.Customer,t.Factory,t.Floor,t.Line;";
                    string sCustomer = "";
                    string sFactory = "";
                    string sFloor = "";
                    string sLine = "";
                    string sEquipID = "";
                    string sFileName = "";
                    string sFilePath = "";
                    string sEquipName = "";
                    string sModule = "";
                    //DBdata获取 设备信息
                    System.Data.DataTable dataTable = getDataTableForZZFox(AsMySqlConnectionString, sqlFx, log);
                    if (dataTable != null && dataTable.Rows.Count > 0)
                    {
                        sCustomer = (string)dataTable.Rows[0][0];
                        sFactory = (string)dataTable.Rows[0][1];
                        sFloor = (string)dataTable.Rows[0][2];
                        sLine = (string)dataTable.Rows[0][3];
                        sEquipID = (string)dataTable.Rows[0][4];
                        sEquipName = (string)dataTable.Rows[0][5];
                        sModule = (string)dataTable.Rows[0][6];
                    }                  
                    DateTime dtStartTime = AstartTime;
                    while (dtStartTime < AsendTime)
                    {
                        DateTime dtEndTime = dtStartTime.AddMinutes(AintSectionTime);
                        if (!string.IsNullOrEmpty(sFactory)
                           && !string.IsNullOrEmpty(sFloor)
                           && !string.IsNullOrEmpty(sLine)
                           && !string.IsNullOrEmpty(sEquipID)
                           )
                        {
                            sFileName = C_AOI + "_" + sFactory + sFloor + sLine + "_" + sEquipID + "_" + dtStartTime.ToString(ZZFoxPDB.RS_DATE_FORMAT);
                            sFilePath = Path.Combine(AstrDir, sFileName + ".csv");
                             
                        }
                        else
                        {
                            return;
                        }
                        bool isNewFile = false;
                        if (!File.Exists(sFilePath))
                        {
                            using (FileStream fsLine1 = new FileStream(sFilePath, FileMode.Create))
                            {
                                ;
                            }
                            isNewFile = true;
                        }
                        #region    "測試總数量报表"
                        //nowTime = AstartTime.AddSeconds(AintSectionTime*60); lastTime = nowTime.AddSeconds(-sec*60); AintSectionTime += sec;
                        sqlFb = "  SELECT t.PCBID,t.Result from TBBoard as t  " +
                                "  WHERE   t.StartTime >= STR_TO_DATE('" + dtStartTime.ToString(ZZFoxPDB.RS_DATE_FORMAT + ZZFoxPDB.RS_TIME_FORMAT) + "','%Y%m%d%H%i%S') " +
                                "  AND t.StartTime < STR_TO_DATE('" + dtEndTime.ToString(ZZFoxPDB.RS_DATE_FORMAT + ZZFoxPDB.RS_TIME_FORMAT) + "','%Y%m%d%H%i%S');";
                        sqlFsr =" SELECT sum(t.EndTime - t.StartTime)  FROM TBBoard as t " +
                                " WHERE  t.StartTime >= STR_TO_DATE('" + dtStartTime.ToString(ZZFoxPDB.RS_DATE_FORMAT + ZZFoxPDB.RS_TIME_FORMAT) + "','%Y%m%d%H%i%S') " +
                                " AND t.EndTime <STR_TO_DATE('" + dtEndTime.ToString(ZZFoxPDB.RS_DATE_FORMAT + ZZFoxPDB.RS_TIME_FORMAT) + "','%Y%m%d%H%i%S'); ";
                        sqlFs = " SELECT sum(Run=1 and error=0) * TimeInterval,sum(Idle=1 and error=0) * TimeInterval,sum(error = 1) * TimeInterval " +
                                " FROM TBEquipStatus as t  " +
                                " WHERE t.UpdateTime >= STR_TO_DATE('" + dtStartTime.ToString(ZZFoxPDB.RS_DATE_FORMAT + ZZFoxPDB.RS_TIME_FORMAT) + "','%Y%m%d%H%i%S') " +
                                " and  t.UpdateTime<STR_TO_DATE('" + dtEndTime.ToString(ZZFoxPDB.RS_DATE_FORMAT + ZZFoxPDB.RS_TIME_FORMAT) + "','%Y%m%d%H%i%S'); ";                        
                        if (!string.IsNullOrEmpty(sFactory)
                            && !string.IsNullOrEmpty(sFloor)
                            && !string.IsNullOrEmpty(sLine)
                            && !string.IsNullOrEmpty(sEquipID)
                            )
                        {
                            if (isNewFile)
                            {                           
                                strbld.Append(sFactory + sFloor + ZZFoxPDB.lineUnder +
                                        sLine + ZZFoxPDB.lineUnder +
                                        "AOI" + ZZFoxPDB.lineUnder +
                                        "HOLLY" + ZZFoxPDB.lineUnder +
                                        sEquipName + ZZFoxPDB.lineUnder +
                                        sEquipID + ZZFoxPDB.lineUnder +
                                        AstartTime.ToString(ZZFoxPDB.RS_DATE_FORMAT) + ZZFoxPDB.lineUnder +
                                        "No Check" + ZZFoxPDB.lineUnder);
                                strbld.Append(ZZFoxPDB.RS_EXPRT_TITLE + ZZFoxPDB.lineUnder);
                                isNewFile = false;
                            }
                            //DBdata获取 runSeconds  badSeconds waitSeconds 运行时间 故障时间 等待时间
                            System.Data.DataTable dl = getDataTableForZZFox(AsMySqlConnectionString,sqlFs,log);
                            string runSeconds = "";
                            string waitSeconds = "";
                            string cycleSeconds = "";
                            string badSeconds = "";
                            
                            if (dl != null && dl.Rows.Count > 0)
                            {
                                if (!string.IsNullOrEmpty(dl.Rows[0][0].ToString()))
                                {
                                    runSeconds = dl.Rows[0][0].ToString();
                                }
                                if (!string.IsNullOrEmpty(dl.Rows[0][1].ToString()))
                                {                                  
                                    waitSeconds = dl.Rows[0][1].ToString();
                                }
                                if (!string.IsNullOrEmpty(dl.Rows[0][2].ToString()))
                                {
                                    badSeconds = dl.Rows[0][2].ToString();
                                }
                            }
                            //DBdata 得到cycleSeconds 循环时间
                            System.Data.DataTable dsr = getDataTableForZZFox(AsMySqlConnectionString, sqlFsr,log);
                            if (dsr != null && dsr.Rows.Count > 0)
                            {
                                for (int m = 0; m < dsr.Rows.Count; m++)
                                {
                                    if (!string.IsNullOrEmpty(dsr.Rows[m][0].ToString()))
                                    {

                                        cycleSeconds = dsr.Rows[m][0].ToString();
                                    }
                                }
                            }
                            //DBdata获取 spc板信息
                            System.Data.DataTable de = getDataTableForZZFox(AsMySqlConnectionString, sqlFb,log);
                            int deAllAcount = 0, deGoodAcount = 0, deNgAcount = 0, dePassAcount = 0;
                            if (de != null && de.Rows.Count > 0)
                            {
                                for (int m = 0; m < de.Rows.Count; m++)
                                {

                                    if (de.Rows[m][1].ToString() == "0")
                                    {
                                        deGoodAcount++;
                                    }
                                    else if (de.Rows[m][1].ToString() == "2") { dePassAcount++; }
                                    else if (de.Rows[m][1].ToString() == "1") { deNgAcount++; }
                                    if (!string.IsNullOrEmpty(de.Rows[m][0].ToString()))
                                    {
                                        deAllAcount++;
                                    }
                                }
                            }
                            float sAcount = 0f;
                            if (deAllAcount == 0)
                            {
                                sAcount = 0f;
                            }
                            else
                            {
                                sAcount = ((float)(deGoodAcount + dePassAcount) / (float)deAllAcount);
                            }
                            if (string.IsNullOrEmpty(runSeconds)) runSeconds = ZZFoxPDB.RS_ZERO;//运行时间(时)                                                                                         
                            if (string.IsNullOrEmpty(waitSeconds)) waitSeconds = ZZFoxPDB.RS_ZERO;//等待时间(时)                                                                                     
                            if (string.IsNullOrEmpty(cycleSeconds)) cycleSeconds = ZZFoxPDB.RS_ZERO;//循环时间 (秒)                          
                            if (string.IsNullOrEmpty(badSeconds)) badSeconds = ZZFoxPDB.RS_ZERO;//故障时间(时)

                            runSeconds = (double.Parse(runSeconds) / (double)3600).ToString(ZZFoxPDB.RS_STR_FORMAT_4F);
                            waitSeconds = (double.Parse(waitSeconds) / (double)3600).ToString(ZZFoxPDB.RS_STR_FORMAT_4F);
                            badSeconds = (double.Parse(badSeconds) / (double)3600).ToString(ZZFoxPDB.RS_STR_FORMAT_4F);
                            //兑换成小时--
                            //TimeSpan tsRun = new TimeSpan(0, 0, 0, (int)(double.Parse(runSeconds)));
                            //runSeconds = tsRun.Hours + ":" + tsRun.Minutes + ":" + tsRun.Seconds + ":" + tsRun.Milliseconds;
                            //TimeSpan tsWait = new TimeSpan(0, 0, 0, (int)(double.Parse(waitSeconds)));
                            //waitSeconds = tsWait.Hours + ":" + tsWait.Minutes + ":" + tsWait.Seconds;
                            //TimeSpan tsBad = new TimeSpan(0, 0, 0, (int)(double.Parse(badSeconds)));
                            //badSeconds = tsBad.Hours + ":" + tsBad.Minutes + ":" + tsBad.Seconds;                          
                            // 
                            // 如果数据都为0则放弃输出
                            //if (   deAllAcount == 0
                            //    && deGoodAcount == 0
                            //    && dePassAcount == 0
                            //    && deNgAcount == 0
                            //    && sAcount == 0
                            //    && ZZFoxPDB.RS_STR_FORMAT_4F.Equals(runSeconds)
                            //    && ZZFoxPDB.RS_STR_FORMAT_4F.Equals(waitSeconds)
                            //    && ZZFoxPDB.RS_STR_FORMAT_4F.Equals(badSeconds)
                            //    && ZZFoxPDB.RS_ZERO.Equals(cycleSeconds)
                            //    )
                            //{
                            //}
                            //else
                            //{
                                strbld.Append(dtStartTime.ToString(ZZFoxPDB.RS_DATE_FORMAT) + ZZFoxPDB.RS_SPIT +
                                           dtStartTime.ToString(ZZFoxPDB.RS_TIME_FORMAT) + ZZFoxPDB.RS_SPIT +
                                           deAllAcount + ZZFoxPDB.RS_SPIT +
                                           (deGoodAcount + dePassAcount) + ZZFoxPDB.RS_SPIT +
                                           deNgAcount + ZZFoxPDB.RS_SPIT +
                                           deAllAcount * (60 / sec) + ZZFoxPDB.RS_SPIT +
                                           sAcount * 100 + ZZFoxPDB.RS_PER + ZZFoxPDB.RS_SPIT +//直通率
                                           runSeconds + ZZFoxPDB.RS_SPIT +
                                           waitSeconds + ZZFoxPDB.RS_SPIT +
                                           cycleSeconds + ZZFoxPDB.RS_SPIT +
                                           badSeconds + ZZFoxPDB.RS_SPIT + ZZFoxPDB.RS_UNLINE
                                 );
                           // }
                            if (!string.IsNullOrEmpty(strbld.ToString()))
                            {
                                FileStream fsLine1 = new FileStream(sFilePath, FileMode.Append);

                                StreamWriter wsLine = new StreamWriter(fsLine1);
                                wsLine.Write(strbld);
                                wsLine.Flush();
                                wsLine.Close();
                                fsLine1.Close();
                                strbld.Clear();
                            }
                        }
                        #endregion
                        dtStartTime = dtStartTime.AddMinutes(AintSectionTime);
                    }
                }
            }
            catch (Exception ex)
            {
                string error = "  function: (ExportZZFoxconnInformat)  数据导出出错 ";
                log.WriteErr("错误 ! " + error + ex.Message );
            }
        }
        public static DataTable getDataTableForZZFox(string AsMySqlConnecionString , string AsMySqlQueryStr,Log log)
        {
            MySqlConnection mysqlCon = new MySqlConnection(AsMySqlConnecionString);
            MySqlCommand cmd = new MySqlCommand();
            MySqlDataAdapter daptMySQL= new MySqlDataAdapter();
            DataTable dt = new DataTable();
            string sResult = string.Empty;
            try
            {                           
                openMySqlConnection(AsMySqlConnecionString, mysqlCon, log);
                cmd.Connection = mysqlCon;
                cmd.CommandText = AsMySqlQueryStr;
                cmd.CommandType = CommandType.Text;
                cmd.CommandTimeout = 500;
                daptMySQL.SelectCommand = cmd;
                daptMySQL.Fill(dt);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (mysqlCon.State == ConnectionState.Open)
                {
                    mysqlCon.Close();                   
                }
                cmd.Dispose();
                daptMySQL.Dispose();
            }
            return dt;
        }
        public static void openMySqlConnection(string AsMySqlConnecionString,MySqlConnection mysqlConnection,Log log)
        {            
            try
            {
                if (string.IsNullOrEmpty(AsMySqlConnecionString))
                {
                    throw new Exception("(openMySqlConnection) error : mySqlConnectionString empty");
                }
                else
                {
                    if (mysqlConnection.State != ConnectionState.Open)
                    {
                        mysqlConnection.Open();
                    }
                }
            }
            catch (Exception ex)
            {
                string error = "  function: (openMySqlConnection)  数据库连接出错 ";
                log.WriteErr("错误 ! " + error + ex.Message  );
            }
        }
        #endregion
    }

    }

