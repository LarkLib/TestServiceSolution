using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;
using System.Data.SqlClient;



namespace Elite.Dsd
{
    class SyncInterface
    {
        private string wmsConnectionString = "Data Source=192.168.200.48;Initial Catalog=dsdwms;Persist Security Info=True;User ID=taimao;Password=sd.258963;Asynchronous Processing=true";
        private string teConnectionString = "Data Source=192.168.158.5;Initial Catalog=AMR182002;Persist Security Info=True;User ID=wms;Password=tE182002dsD;Asynchronous Processing=true";

        public string WmsConnectionString
        {
            get { return wmsConnectionString; }
            set { wmsConnectionString = value; }
        }
        public string TEConnectionString
        {
            get { return teConnectionString; }
            set { teConnectionString = value; }
        }

        //耐力数据同步
        public int SyncDataFromETToWms(int deviceId, string taskId, int track_no = 200002)
        {
            var result = -1;
            using (var wmsConnection = new SqlConnection(wmsConnectionString))
            {

                var wmsCmd = wmsConnection.CreateCommand();
                wmsCmd.CommandText = "up_WMS_InquireInsert";
                wmsCmd.CommandType = CommandType.StoredProcedure;

                //耐力上报数据
                if (deviceId == 14001)
                {
                    using (var teConnection = new SqlConnection(TEConnectionString))
                    {
                        var teCmd = teConnection.CreateCommand();
                        teCmd.CommandText = "WMS_Inquire";
                        teCmd.CommandType = CommandType.StoredProcedure;
                        teCmd.Parameters.Add(new SqlParameter("@track_no", SqlDbType.Int) { Value = track_no });

                        teConnection.Open();
                        var reader = teCmd.ExecuteReader();
                        if (reader.Read())
                        {
                            for (int i = 0; i < reader.FieldCount; i++)
                            {
                                var name = string.Format("@{0}", reader.GetName(i));
                                var dbType = reader[i].GetType() == typeof(int) ? SqlDbType.Int : SqlDbType.NVarChar;
                                wmsCmd.Parameters.Add(new SqlParameter(name, dbType) { Value = reader[i] });
                            }
                        }
                        teConnection.Close();
                    }
                }
                //手工上货口上报数据
                else if (deviceId == 14002)
                {
                    wmsCmd.Parameters.Add(new SqlParameter("@sn", SqlDbType.NVarChar, 255) { Value = Guid.NewGuid().ToString() });
                }
                else
                {
                    throw new ArgumentOutOfRangeException("deviceId", "deviceId must be 14001 or 14002");
                }
                wmsCmd.Parameters.Add(new SqlParameter("@deviceId", SqlDbType.NVarChar, 255) { Value = deviceId.ToString() });
                wmsCmd.Parameters.Add(new SqlParameter("@transpottaskid", SqlDbType.NVarChar, 255) { Value = taskId });
                wmsCmd.Parameters.Add(new SqlParameter("@message", SqlDbType.NVarChar, 255) { Direction = ParameterDirection.Output });
                wmsConnection.Open();
                result = wmsCmd.ExecuteNonQuery();
                wmsConnection.Close();
            }
            return result;
        }

        //条码扫描仪上报数据
        public int UploadDataFromScannerToWms(string ScannerId, string palletCode, string taskId)
        {
            //--条码上报
            //--@strInParam0 = '1'		1：条码扫描仪上报
            //--@strInParam1 = '扫描位置' 
            //--@strInParam2 = '条码值'
            //--@strInParam3 = '传送线任务号'
            string strOutParam;
            int intOutParam;
            var parameterDictionary = new Dictionary<string, string>();
            parameterDictionary[WcsInfoReportParameter.strInParam0] = "1";
            parameterDictionary[WcsInfoReportParameter.strInParam1] = ScannerId;
            parameterDictionary[WcsInfoReportParameter.strInParam2] = palletCode;
            parameterDictionary[WcsInfoReportParameter.strInParam3] = taskId;
            return WcsInfoReport(parameterDictionary, out strOutParam, out intOutParam);
        }

        public int WcsInfoReport(Dictionary<string, string> parameterDictionary, out string strOutParam, out int intOutParam)
        {
            var result = -1;
            using (var wmsConnection = new SqlConnection(wmsConnectionString))
            {
                var wmsCmd = wmsConnection.CreateCommand();
                wmsCmd.CommandText = "WcsInfoReport";
                wmsCmd.CommandType = CommandType.StoredProcedure;
                wmsConnection.Open();
                wmsCmd.Parameters.Add(new SqlParameter(WcsInfoReportParameter.strInParam0, SqlDbType.NVarChar, 100) { Value = parameterDictionary.GetParameter(WcsInfoReportParameter.strInParam0) });
                wmsCmd.Parameters.Add(new SqlParameter(WcsInfoReportParameter.strInParam1, SqlDbType.NVarChar, 100) { Value = parameterDictionary.GetParameter(WcsInfoReportParameter.strInParam1) });
                wmsCmd.Parameters.Add(new SqlParameter(WcsInfoReportParameter.strInParam2, SqlDbType.NVarChar, 100) { Value = parameterDictionary.GetParameter(WcsInfoReportParameter.strInParam2) });
                wmsCmd.Parameters.Add(new SqlParameter(WcsInfoReportParameter.strInParam3, SqlDbType.NVarChar, 100) { Value = parameterDictionary.GetParameter(WcsInfoReportParameter.strInParam3) });
                wmsCmd.Parameters.Add(new SqlParameter(WcsInfoReportParameter.strInParam4, SqlDbType.NVarChar, 100) { Value = parameterDictionary.GetParameter(WcsInfoReportParameter.strInParam4) });
                wmsCmd.Parameters.Add(new SqlParameter(WcsInfoReportParameter.strInParam5, SqlDbType.NVarChar, 100) { Value = parameterDictionary.GetParameter(WcsInfoReportParameter.strInParam5) });
                wmsCmd.Parameters.Add(new SqlParameter(WcsInfoReportParameter.strInParam6, SqlDbType.NVarChar, 100) { Value = parameterDictionary.GetParameter(WcsInfoReportParameter.strInParam6) });
                wmsCmd.Parameters.Add(new SqlParameter(WcsInfoReportParameter.strInParam7, SqlDbType.NVarChar, 100) { Value = parameterDictionary.GetParameter(WcsInfoReportParameter.strInParam7) });
                wmsCmd.Parameters.Add(new SqlParameter(WcsInfoReportParameter.strInParam8, SqlDbType.NVarChar, 100) { Value = parameterDictionary.GetParameter(WcsInfoReportParameter.strInParam8) });
                wmsCmd.Parameters.Add(new SqlParameter(WcsInfoReportParameter.strInParam9, SqlDbType.NVarChar, 100) { Value = parameterDictionary.GetParameter(WcsInfoReportParameter.strInParam9) });
                var strOutParamParameter = new SqlParameter(WcsInfoReportParameter.strOutParam, SqlDbType.NVarChar, 500) { Direction = ParameterDirection.Output };
                var intOutParamParameter = new SqlParameter(WcsInfoReportParameter.intOutParam, SqlDbType.Int) { Direction = ParameterDirection.Output };
                wmsCmd.Parameters.Add(strOutParamParameter);
                wmsCmd.Parameters.Add(intOutParamParameter);
                result = wmsCmd.ExecuteNonQuery();
                strOutParam = (string)strOutParamParameter.Value;
                intOutParam = (int)intOutParamParameter.Value;
                wmsConnection.Close();
            }
            return result;
        }
    }

    static class Extension
    {
        public static string GetParameter(this Dictionary<string, string> dictionary, string key)
        {
            var value = string.Empty;
            if (dictionary != null && dictionary.ContainsKey(key))
            {
                value = dictionary[key];
            }
            return value;
        }
    }

    class WcsInfoReportParameter
    {
        public const string strInParam0 = "@strInParam0";
        public const string strInParam1 = "@strInParam1";
        public const string strInParam2 = "@strInParam2";
        public const string strInParam3 = "@strInParam3";
        public const string strInParam4 = "@strInParam4";
        public const string strInParam5 = "@strInParam5";
        public const string strInParam6 = "@strInParam6";
        public const string strInParam7 = "@strInParam7";
        public const string strInParam8 = "@strInParam8";
        public const string strInParam9 = "@strInParam9";
        public const string strOutParam = "@strOutParam";
        public const string intOutParam = "@intOutParam";
    }
}
