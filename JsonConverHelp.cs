using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoAPP
{
    class JsonConverHelp
    {


    }
    class JsonConverLogin
    {
        public string success { get; set; }
        public string errorCode { get; set; }
        public string message { get; set; }
        public string data { get; set; }
    }
    class JsonConverStatus
    {
        public string success { get; set; }
        public string errorCode { get; set; }
        public string message { get; set; }
        public string data { get; set; }
        public string pageNum { get; set; }
        public string pageSize { get; set; }
        public string total { get; set; }
        public string pages { get; set; }
        public JsonConverStatus()
        {

        }
    }

    class JsonConverErrorData
    {
        public string clientId { get; set; }
        public string clientSecret { get; set; }
        //public string clientId { get; set; }
        public string at { get; set; }
        public string on { get; set; }
        public string sn { get; set; }
        public string sf { get; set; }
        public string error { get; set; }
        public string msg { get; set; }
        public MdInfomation md { get; set; }
        public JsonConverErrorData() { }

    }
    class JsonConverPRDData
    {
        public string clientId { get; set; }
        public string clientSecret { get; set; }
        //public string clientId { get; set; }
        public string at { get; set; }
        public string on { get; set; }
        public string sn { get; set; }
        public string sf { get; set; }
        public string result { get; set; }
        public MdInfomation md { get; set; }
        public JsonConverPRDData() { }
    }
    class MdInfomation
    {
        public string SN { get; set; }
        public string MachineNo { get; set; }
        public string ITEMVALUE { get; set; }
        public string MO { get; set; }
        public string USERNO { get; set; }

        public string jobName { get; set; }
        public string startTime { get; set; }
        public string endTime { get; set; }
        public string totalSec { get; set; }

        public string user { get; set; }
        public string opConfirmed { get; set; }
        public string opConfirmedTime { get; set; }
        public string errorCode { get; set; }

        public MdPadInfo[] mdPadInfos;
        public MdArrayPcbInfo[] mdArrayPcbInfos;
        public MdInfomation()
        {
        }
    }
    
    class MdArrayPcbInfo
    {
        public string arraybarcode { get; set; }
        public string arrayResult { get; set; }
    }
    class MdPadInfo
    {
        public string padId { get; set; }
        public string height { get; set; }
        //public string heightSpcPerH { get; set; }
        //public string heightSpcPerL { get; set; }
        public string vol { get; set; }
        //public string volSpcPerH { get; set; }
        //public string volSpcPerL { get; set; }
        public string area{ get; set; }
        //public string areaSpcPerH { get; set; }
        //public string areaSpcPerL { get; set; }
        public string barcode { get; set; }
        public string padResult { get; set; }
        public string defaultType { get; set; }
    }
}
