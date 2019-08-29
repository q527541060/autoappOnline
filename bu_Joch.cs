using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Xml;

namespace AutoAPP
{
    class bu_Joch
    {
        private ThreadProcess runnerDeleteData;
        private ThreadProcessEXFO runnerEXFO;

        private static readonly string RS_FORMAT_DATETIME = "yyyy-MM-dd HH:mm:ss";
        public static readonly string RS_Format_DateTimeFileName = "yyyyMMddHHmmss";
        private string ThreadName = "DeleteData";


        /// <summary>
        ///  连接测试
        /// </summary>
        /// <param name="strMySQLConnect"></param>
        /// <param name="log"></param>
        /// <returns></returns>
        public string TestConnection(string strMySQLConnect)
        {
            try
            {
                string strReturn = "连接测试成功！";
                MySqlConnection conn = new MySqlConnection(strMySQLConnect);
                MySqlCommand cmd = null;
                try
                {
                    if (conn.State != System.Data.ConnectionState.Open)
                    {
                        conn.Open();
                    }
                    cmd = conn.CreateCommand();
                    // cmd.CommandText = "  SELECT * from tbpadmeasure  LIMIT 1  ";
                    cmd.CommandText = "  SELECT TRIGGER_NAME from  information_schema.TRIGGERS where TRIGGER_NAME ='TBBoard_delete_Before_trigger';   ";
                    object objScalar = cmd.ExecuteScalar();
                    if (objScalar == DBNull.Value || objScalar == null)
                    {
//                        cmd.CommandText = @"create trigger TBBoard_delete_Before_trigger
//                                           BEFORE delete on TBBoard for each row
//                                           begin
//                                             DELETE from TBPadMeasure  where PCBID = old.PCBID; 
//                                             DELETE FROM TBBarCode WHERE PCBID = old.PCBID;
//                                           end; ";
//                        cmd.ExecuteNonQuery();
                    }
                    else
                    {
                        //add by peng 20190319 delete trigger ;
                        cmd.CommandText = "DROP TRIGGER TBBoard_delete_Before_trigger ;";
                        cmd.ExecuteScalar();
                    }
                }
                catch (System.Exception ex)
                {
                    strReturn = strMySQLConnect.ToString() + "连接出错：" + ex.Message.ToString();
                    //log.WriteErr("错误 ! " + strReturn, "Main");
                    AppLogHelp.WriteError(LogFileFormate.SQL, "TestConnection " + strReturn);
                }
                finally   // moditied by peng 20190319
                {
                    conn.Close();
                    cmd.Dispose();
                    conn.Dispose();
                }

                //log.WriteLog(strReturn, "Main");
                return strReturn;

            }
            catch (System.Exception ex)
            {
                throw ex;
            }
        }

        public string TestConnectionFox(string strMysqlConn)
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
                    //log.WriteErr("错误 ! " + strReturn, "Main");
                    AppLogHelp.WriteLog(LogFileFormate.MES, "TestConnectionFox" + strReturn);
                }
                finally
                {
                    if (conn.State == ConnectionState.Open)
                    {
                        conn.Close();
                        strReturn = "测试链接成功!";
                    }
                }
                AppLogHelp.WriteLog(LogFileFormate.MES, "TestConnectionFox" + strReturn);
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
        public void AutoStart(DateTime dtStart, DateTime dtEnd, bool blnBackUpData, string strBackFilePath, bool blnDeleteDataEveryHour, int iPCBLimit)
        {
            try
            {
                
                runnerDeleteData = new ThreadProcess(dtStart, dtEnd, blnBackUpData, strBackFilePath, blnDeleteDataEveryHour, iPCBLimit);
                runnerDeleteData.Run();
            }
            catch (Exception ex)
            {
                 
                AppLogHelp.WriteLog(LogFileFormate.Delete, ThreadName + ex.ToString());
            }
        }


        public void AutoStart(DateTime dtEndTime, bool blnBackUpData, string strBackFilePath, bool blnDeleteDataEveryHour, int iPCBLimit)
        {
            try
            {
                runnerDeleteData = new ThreadProcess(dtEndTime, blnBackUpData, strBackFilePath, blnDeleteDataEveryHour, iPCBLimit);
                runnerDeleteData.Run();
            }
            catch (Exception ex)
            {
                //log.WriteErr("错误 ! " + ex.ToString(), ThreadName);
                AppLogHelp.WriteError(LogFileFormate.Delete, ThreadName + " 错误 ! :" + ex.ToString());
                // moditied by peng 20190314
                throw ex;
            }

        }

        /// <summary>
        /// stop
        /// </summary>
        /// <returns></returns>
        public Boolean Stop()
        {
            Boolean blnReturn = true;
            try
            {
                if (runnerDeleteData != null)
                    if (runnerDeleteData.Running)
                    {
                        runnerDeleteData.Stop();
                        blnReturn = false;
                    }

                if (runnerEXFO != null)
                    if (runnerEXFO.Running)
                    {
                        runnerEXFO.Stop();
                        blnReturn = false;
                    }
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
        public int DeleteAndBackupDataByDays(DateTime dtStartTime, DateTime dtEndTime, int intSPCDataBaseType, string strConnect, bool blnBackUpData, string strBackFilePath, bool blnDeleteDataEveryHour, int iPCBLimit)
        {
            MySqlConnection connMySQL = new MySqlConnection(strConnect);
            MySqlCommand cmdMySQL = new MySqlCommand();
            MySqlTransaction tranMySQL;
            MySqlDataAdapter daptMySQL = new MySqlDataAdapter();
            DataTable dtReturn = new DataTable();

            int intReturn = 0;
            string strBackUpPadMeasureSQL = string.Empty;
            string strBackUpBoardSQL = string.Empty;
            string strBackUpBarcodeSQL = string.Empty;
            //string strBackUpRealTimeResourcesSQL = string.Empty;
            string strBackUpFile = string.Empty;
            string strPCBID = string.Empty;

            string strDeletePadMeasureSQL = string.Empty;
            string strDeleteBoardSQL = string.Empty;
            string strDeleteBarcodeSQL = string.Empty;
            // string strDeleteRealTimeResourcesSQL = string.Empty;

            try
            {

                switch (intSPCDataBaseType)
                {
                    case 2:
                        {
                            string strSelectPCBID = string.Empty;

                            if (blnDeleteDataEveryHour)
                            {
                                int iDeletePCBLimtCount = iPCBLimit;
                                if (iDeletePCBLimtCount > 0)
                                {
                                    strSelectPCBID = " SELECT   PCBID  FROM spidb.TBBoard WHERE StartTime >= '" + dtStartTime.ToString(RS_FORMAT_DATETIME) + "'  and StartTime <= '" + dtEndTime.ToString(RS_FORMAT_DATETIME) + "' Limit " + iDeletePCBLimtCount + ";";

                                }
                                else
                                {
                                    strSelectPCBID = " SELECT   PCBID  FROM spidb.TBBoard WHERE StartTime >= '" + dtStartTime.ToString(RS_FORMAT_DATETIME) + "'  and StartTime <= '" + dtEndTime.ToString(RS_FORMAT_DATETIME) + "' Limit 100 ;";
                                }
                            }
                            else
                            {
                                strSelectPCBID = " SELECT   PCBID  FROM spidb.TBBoard WHERE StartTime >= '" + dtStartTime.ToString(RS_FORMAT_DATETIME) + "'  and StartTime <= '" + dtEndTime.ToString(RS_FORMAT_DATETIME) + "';";
                            }

                            if (connMySQL.State != ConnectionState.Open)
                            {
                                //log.WriteLog("select pcb 打开..","查询PCB");
                                connMySQL.Open();
                            }
                            cmdMySQL = connMySQL.CreateCommand();
                            cmdMySQL.CommandTimeout = Properties.Settings.Default.SQLTimeOut;
                            cmdMySQL.CommandText = strSelectPCBID;
                            daptMySQL.SelectCommand = cmdMySQL;
                            daptMySQL.Fill(dtReturn);

                            //if (connMySQL.State == ConnectionState.Open)    //moditied by peng 20190314
                            //{
                            //    connMySQL.Close();
                            //    log.WriteLog("select pcb 关闭..", "查询PCB");
                            //}
                            //cmdMySQL.Dispose();
                            //daptMySQL.Dispose();
                            foreach (DataRow dr in dtReturn.Rows)
                            {
                                int iPCBCount = 0;
                                //  strPCBID += dr[0].ToString() + ",";
                                strPCBID = dr[0].ToString() + ",";
                                //   }
                                if (strPCBID.Length > 1)
                                {
                                    strPCBID = strPCBID.Remove(strPCBID.Length - 1, 1);

                                    strDeletePadMeasureSQL = string.Format("DELETE FROM spidb.TBPadMeasure WHERE PCBID = '{0}';  ", strPCBID);

                                    strDeleteBarcodeSQL = string.Format("DELETE FROM spidb.TBBarCode  WHERE PCBID = '{0}';  ", strPCBID);

                                    strDeleteBoardSQL = string.Format(" DELETE FROM	 spidb.TBBoard  WHERE PCBID = '{0}';  ", strPCBID);

                                    
                                }

                                if (blnBackUpData)
                                {

                                    strBackUpPadMeasureSQL = " SELECT `PCBID`, `PadID`, `LineNo`, `JobIndex`, `PadIndex`, `ABSHeight`, `ABSArea`, `ABSVolume`, `ShiftX`, `ShiftY`, `PerHeight`, `PerArea`, `PerVolume`, `ABSShape`, `BridgeType`, `DefectType`, `JudgeRes`, `BaseType`, `ArrayIDIndex`, `PadArea`  into OUTFILE '{1}' FIELDS TERMINATED BY ',' OPTIONALLY ENCLOSED BY '\"' LINES TERMINATED BY '\n' from spidb.TBPadMeasure   where PCBID in ({0}) ;";

                                    if (!Directory.Exists(strBackFilePath))
                                        Directory.CreateDirectory(strBackFilePath);

                                    strBackUpFile = Path.Combine(strBackFilePath, "TBPadMeasure" + "_" + strPCBID + "_" + DateTime.Now.ToString(RS_Format_DateTimeFileName) + ".backup");

                                    if (File.Exists(strBackUpFile))
                                    {
                                        strBackUpFile = Path.Combine(strBackFilePath, "TBPadMeasure" + "_" + strPCBID + "_" + DateTime.Now.AddSeconds(1).ToString(RS_Format_DateTimeFileName) + ".backup");
                                    }

                                    strBackUpFile = strBackUpFile.Replace("\\", "\\\\");
                                    //log.WriteLog("TBPadMeasure备份文件:" + strBackUpFile, ThreadName);
                                    AppLogHelp.WriteLog(LogFileFormate.Delete, ThreadName + " TBPadMeasure备份文件:" + strBackUpFile);
                                    strBackUpPadMeasureSQL = string.Format(strBackUpPadMeasureSQL, strPCBID, strBackUpFile);

                                    strBackUpBoardSQL = " SELECT* into OUTFILE '{1}' FIELDS TERMINATED BY ',' OPTIONALLY ENCLOSED BY '\"' LINES TERMINATED BY '\n' from TBBoard  where PCBID in ({0}) ; ";

                                    strBackUpFile = Path.Combine(strBackFilePath, "TBBoard" + "_" + strPCBID + "_" + DateTime.Now.ToString(RS_Format_DateTimeFileName) + ".backup");

                                    if (File.Exists(strBackUpFile))
                                    {
                                        strBackUpFile = Path.Combine(strBackFilePath, "TBBoard" + "_" + strPCBID + "_" + DateTime.Now.AddSeconds(1).ToString(RS_Format_DateTimeFileName) + ".backup");
                                    }
                                    strBackUpFile = strBackUpFile.Replace("\\", "\\\\");
                                    //log.WriteLog("TBBoard备份文件:" + strBackUpFile, ThreadName);
                                    AppLogHelp.WriteLog(LogFileFormate.Delete, ThreadName + " TBPadMeasure备份文件:" + strBackUpFile);
                                    strBackUpBoardSQL = string.Format(strBackUpBoardSQL, strPCBID, strBackUpFile);

                                    strBackUpBarcodeSQL = " SELECT * into OUTFILE '{1}' FIELDS TERMINATED BY ',' OPTIONALLY ENCLOSED BY '\"' LINES TERMINATED BY '\n' from TBBarCode  where PCBID in ({0}) ;";
                                    strBackUpFile = Path.Combine(strBackFilePath, "TBBarCode" + "_" + strPCBID + "_" + DateTime.Now.ToString(RS_Format_DateTimeFileName) + ".backup");

                                    if (File.Exists(strBackUpFile))
                                    {
                                        strBackUpFile = Path.Combine(strBackFilePath, "TBBarCode" + "_" + strPCBID + "_" + DateTime.Now.AddSeconds(1).ToString(RS_Format_DateTimeFileName) + ".backup");
                                    }
                                    strBackUpFile = strBackUpFile.Replace("\\", "\\\\");
                                    //log.WriteLog("TBBarCode备份文件:" + strBackUpFile, ThreadName);
                                    AppLogHelp.WriteLog(LogFileFormate.Delete, ThreadName + " " + "TBBarCode备份文件:" + strBackUpFile);
                                    
                                    strBackUpBarcodeSQL = string.Format(strBackUpBarcodeSQL, strPCBID, strBackUpFile);
                                }

                                if (connMySQL.State != ConnectionState.Open)
                                {
                                    connMySQL.Open();
                                }
                                tranMySQL = connMySQL.BeginTransaction();
                                cmdMySQL = connMySQL.CreateCommand();
                                cmdMySQL.Transaction = tranMySQL;
                                cmdMySQL.CommandTimeout = 0;
                                try
                                {
                                    //cmdMySQL.CommandText += " START TRANSACTION;";
                                    cmdMySQL.CommandText += strBackUpBoardSQL;
                                    cmdMySQL.CommandText += strBackUpBarcodeSQL;
                                    cmdMySQL.CommandText += strBackUpPadMeasureSQL;
                                    // cmdMySQL.CommandText += strBackUpRealTimeResourcesSQL;

                                    //cmdMySQL.CommandText = strDeleteSimplePadSQL;
                                    //intReturn += cmdMySQL.ExecuteNonQuery();
                                    cmdMySQL.CommandText = strDeleteBoardSQL;
                                    iPCBCount = cmdMySQL.ExecuteNonQuery();
                                    cmdMySQL.CommandText = strDeleteBarcodeSQL;
                                    cmdMySQL.ExecuteNonQuery();
                                    cmdMySQL.CommandText = strDeletePadMeasureSQL;
                                    cmdMySQL.ExecuteNonQuery();
                                    //intReturn += cmdMySQL.ExecuteNonQuery();                                  
                                    //intReturn += cmdMySQL.ExecuteNonQuery();
                                    //cmdMySQL.CommandText += strDeleteRealTimeResourcesSQL;
                                    //cmdMySQL.CommandText += " COMMIT;";

                                    //log.WriteLog(strDeleteBoardSQL + strDeleteBarcodeSQL+strDeletePadMeasureSQL, ThreadName);
                                    AppLogHelp.WriteLog(LogFileFormate.Delete, ThreadName+" " + strDeleteBoardSQL + strDeleteBarcodeSQL + strDeletePadMeasureSQL);
                                    //if (connMySQL.State != ConnectionState.Open)
                                    //{
                                    //    connMySQL.Open();
                                    //    //log.WriteLog("del pcb 打开..pcbid:" + strPCBID, "del PCB");
                                    //}
                                    //intReturn += cmdMySQL.ExecuteNonQuery();
                                    tranMySQL.Commit();
                                    //  "START TRANSACTION;" + SQL + "COMMIT;";

                                }
                                catch (Exception ex)
                                {
                                    //intReturn = -1;
                                    //   if (connMySQL.State != ConnectionState.Open)k
                                    //   {
                                    //       connMySQL.Open();
                                    //   }
                                    ////   tranMySQL.Rollback();
                                    
                                    connMySQL.Close();
                                    cmdMySQL.Dispose();
                                    tranMySQL.Dispose();
                                   // log.WriteErr("错误 ! " + ex.ToString(), ThreadName);
                                    AppLogHelp.WriteLog(LogFileFormate.Delete, ThreadName +  "错误 ! " + ex.ToString());
                                   
                                    return -1;
                                }
                                finally
                                {
                                    //if (connMySQL.State == ConnectionState.Open)
                                    //{//moditied by peng 20190314
                                        connMySQL.Close();
                                        cmdMySQL.Dispose();
                                        tranMySQL.Dispose();
                                   // }

                                        //log.WriteLog("删除了" + strPCBID + "下的(" + iPCBCount.ToString() + ")条记录!");
                                        AppLogHelp.WriteLog(LogFileFormate.Delete, "删除了" + strPCBID + "下的(" + iPCBCount.ToString() + ")条记录!");
                                        intReturn += iPCBCount;
                                    //   tranMySQL.Dispose();
                                }
                                //log.WriteLog("Sleep(2000)");
                                System.Threading.Thread.Sleep(2000);
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
                connMySQL.Close();
                cmdMySQL.Dispose();
                //log.WriteErr("错误 ! " + ex.ToString(), ThreadName);
                AppLogHelp.WriteError(LogFileFormate.Delete, ThreadName + " " + "错误 ! " + ex.ToString());
                                    
                return -1;
            }
            finally
            {
                //daptMySQL.Dispose();
                //if (connMySQL.State == ConnectionState.Open) //moditied by peng 20190314
                //{
                    connMySQL.Close();
                //}
                cmdMySQL.Dispose();
                daptMySQL.Dispose();
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }


        public int LoadDataFromFile(string strTableName, string strBackUpFile, string strConnect)
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
                    string strSQL = string.Format("LOAD DATA INFILE \"{0}\" INTO TABLE {1} FIELDS TERMINATED BY ',' OPTIONALLY ENCLOSED BY '\"' LINES TERMINATED BY '\n';", strBackUpFile, strTableName);

                    connMySQL.Open();
                    cmdMySQL = connMySQL.CreateCommand();
                    cmdMySQL.CommandTimeout = Properties.Settings.Default.SQLTimeOut;
                    cmdMySQL.CommandText = strSQL;
                    intReturn += cmdMySQL.ExecuteNonQuery();
                }
            }
            catch (Exception ex)
            {
                //log.WriteErr(strBackUpFile + "Load Data Exception :" + ex.ToString());
                AppLogHelp.WriteError(LogFileFormate.Delete, ThreadName + " " + strBackUpFile + "Load Data Exception :" + ex.ToString());
                //throw ex;
                //return;
                return -1;
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
        public void AutoStartEXFO(DateTime dtStartTime, DateTime dtEndTime)
        {
            try
            {
                runnerEXFO = new ThreadProcessEXFO(dtStartTime, dtEndTime);
                runnerEXFO.Run();
            }
            catch (Exception ex)
            {
                //log.WriteErr("错误 ! " + ex.ToString(), ThreadName);
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
        public DataTable GetSPCDataForEXFO(string AsSQLConnectString, string AsDataTableName, DateTime dtStartTime, DateTime dtEndTime)
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
                    where  1=1 and  g.StartTime >=  '{0}'  and  g.EndTime <=  '{1}';  ";

            DataTable dtReturn = new DataTable();
            MySqlConnection conn =null;
            MySqlCommand cmd = null;
            MySqlDataAdapter daptMySQL=null;
            try
            {
                dtReturn = GetDataTableFromEXFO(AsSQLConnectString, AsDataTableName);
                //lin 20190813
                //string strMySQLConn = Properties.Settings.Default.MySQLConnect;
                string strMySQLConn = WSClnt.PubStaticParam._strSPIdbConnectionString;
                if (dtReturn != null)
                {
                    strGetSPCDataSQL = string.Format(strGetSPCDataSQL, dtStartTime.ToString(RS_FORMAT_DATETIME), dtEndTime.ToString(RS_FORMAT_DATETIME));
                    conn = new MySqlConnection(strMySQLConn);

                    cmd = conn.CreateCommand();
                    daptMySQL = new MySqlDataAdapter();
                    if (conn.State != ConnectionState.Open)
                    {
                        conn.Open();
                    }
                    cmd = conn.CreateCommand();
                    cmd.CommandTimeout = 10000;
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
                cmd.Dispose(); daptMySQL.Dispose();
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
                
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
        public DataTable GetDataTableFromEXFO(string AsSQLConnectString, string AsDataTableName)
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
                //log.WriteErr(strReturn, "链接测试");
                return dt;

            }
            catch (System.Exception ex)
            {
                
                strReturn = AsSQLConnectString.ToString() + "连接出错：" + ex.Message.ToString();
                //log.WriteErr("错误 ! " + strReturn, "链接测试");
                return null;
            }
            finally
            {
                if (conn.State != ConnectionState.Closed)
                {
                    conn.Close();
                }
                daptMySQL.Dispose();
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
            SqlConnection conn = null;
            try
            {
                if (Adt == null || string.IsNullOrEmpty(Adt.TableName) || Adt.Rows.Count == 0)
                    throw new Exception("DataTable Params Error");

                 conn = new SqlConnection(AsSQLConnectString);
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
                if (conn.State == ConnectionState.Open)
                {
                    conn.Close();
                }
            }
        }
        #endregion



    }

}

