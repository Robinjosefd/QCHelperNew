using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace QCHelperNew
{
    public class clsVisualIndicator
    {
        public bool IsConnectionAvailable(string url)
        {

            System.Net.WebRequest objWebReq = System.Net.WebRequest.Create(url);
            System.Net.WebResponse objResp = default(System.Net.WebResponse);
            try
            {
                objResp = objWebReq.GetResponse();
                objResp.Close();
                objWebReq = null;
                //IsConnected = true;
                return true;
            }
            catch (Exception)
            {
                objResp = null;
                objWebReq = null;
                //IsConnected = false;
                return false;
            }
        }

        //EIS : The appharbor time is 8 hours greater than the clients time
        //So while displayign the time, it should be appharbortime-8
        public DateTime ConvertDateTime(DateTime dt)
        {
            dt = dt.AddHours(-8);
            return dt;
        }


        //Robot  ,  Connection , Last Completed Download , Date of Records Downloaded  , Records Downloaded  ,  Records Updated 

        public DataTable GetVisualIndicatorValues()
        {
            // DataSet dTableRetValue = new DataSet();

            DataTable table = new DataTable();
            table.Columns.Add("SlNo");
            table.Columns.Add("RobotName");
            table.Columns.Add("Connection");
            table.Columns.Add("LastCompletedDownload");
            table.Columns.Add("LastCompletedDownloadTime");
            table.Columns.Add("DateofRecordsDownloaded");
            table.Columns.Add("RecordsDownloaded");
            table.Columns.Add("RecordsAppended");
            table.Columns.Add("RecordsUpdated");

            table.Rows.Add(GetAuctionComInfo("1", "AUCTION.COM", "http://www.auction.com/calendar/all/California"));
            table.Rows.Add(GetAuctionComInfo("2", "FASAP", "https://www.lpsasap.com/"));
            table.Rows.Add(GetAuctionComInfo("3", "FPASSPORT", "http://www.fidelitypassport.com/logon.asp"));
            table.Rows.Add(GetAuctionComInfo("4", "NATIONWIDE", "http://www.nationwideposting.com/"));
            table.Rows.Add(GetAuctionComInfo("5", "REALQUEST", "http://www.realquest.com/jsp/rq.jsp?action=switch&page=main"));
            table.Rows.Add(GetAuctionComInfo("6", "TAC", "http://www.tacforeclosures.com/sales/"));
            table.Rows.Add(GetAuctionComInfo("7", "ZILLOW", "http://www.realquest.com/jsp/rq.jsp?action=switch&page=main"));

            return table;
        }


        public string[] GetAuctionComInfo(string slno, string robotName, string siteUrl)
        {
            string strRobotName = robotName;
            string strConnection = "";
            string strLastCompletedDownload = "";
            string strLastCompletedDownloadTime = "";
            string strDateofRecordsDownloaded = "";
            string strRecordsDownloaded = "";
            string strRecordsAppended = "";
            string strRecordsUpdated = "";

            DAL objvi = new DAL();

            try
            {
                bool con;
                con = IsConnectionAvailable(siteUrl);

                if (con == true)
                {
                    strConnection = "OK";
                }
                else
                {
                    strConnection = "Failed";
                }

                string strdt = string.Empty;
                string DtTm = string.Empty;
                DataSet dsdt = new DataSet();
                DateTime dt = new DateTime();
                string[] spstr = { "" };

                //
                switch (robotName.ToUpper())
                {
                    case "AUCTION.COM":
                        #region ["Auction.com"]

                        DtTm = "select * from LastDownloadDetail WITH(NOLOCK)  where Robot_Name='AuctionCom'";

                        dsdt = objvi.GetCount(DtTm);
                        foreach (DataRow dr in dsdt.Tables[0].Rows)
                        {
                            dt = ConvertDateTime(Convert.ToDateTime(Convert.ToString(dr["LastDownloadedTime"])));
                            strdt = dt.ToString();

                            spstr = strdt.Split(' ');
                            strLastCompletedDownload = spstr[0].ToString();
                            strLastCompletedDownloadTime = spstr[1].ToString() + spstr[2].ToString();
                            strDateofRecordsDownloaded = spstr[0].ToString();
                            strRecordsDownloaded = Convert.ToString(dr["DownloadCount"]);
                        }

                        //updated data
                        string apdt = "select count(*) from RetRequred  WITH(NOLOCK)  where RecordMeter='UpdAuction'and RecDate='" + strLastCompletedDownload + "'";
                        DataSet dsap = new DataSet();
                        dsap = objvi.GetCount(apdt);
                        strRecordsUpdated = dsap.Tables[0].Rows[0][0].ToString();

                        break;
                        #endregion
                    case "FASAP":
                        #region ["FASAP"]

                        int totalcount2, apndcount2, updcount2;
                        DtTm = "select LastDownloadedTime from LastDownloadDetail where Robot_Name='FASAP'";

                        dsdt = objvi.GetCount(DtTm);

                        //DateTime dt = ConvertDateTime(Convert.ToDateTime(dsdt.Tables[0].Rows[0][0].ToString()));
                        dt = ConvertDateTime(Convert.ToDateTime(dsdt.Tables[0].Rows[0][0].ToString()));
                        //strdt = Convert.ToString(dr["LastDownloadedTime"]);
                        strdt = dt.ToString();

                        //string strdt = dsdt.Tables[0].Rows[0][0].ToString();
                        spstr = strdt.Split(' ');
                        strLastCompletedDownload = spstr[0].ToString();
                        strLastCompletedDownloadTime = spstr[1].ToString() + spstr[2].ToString();

                        strDateofRecordsDownloaded = spstr[0].ToString();

                        //total downloaded data
                        string strdwn = "select count(*) from Retrequred where (Datafrom='FASAP' or ASAPUdated='1') and RecDate='" + spstr[0].ToString() + "'";
                        DataSet dsdwn = new DataSet();
                        dsdwn = objvi.GetCount(strdwn);
                        strRecordsDownloaded = dsdwn.Tables[0].Rows[0][0].ToString();

                        totalcount2 = Convert.ToInt16(dsdwn.Tables[0].Rows[0][0].ToString());
                        //appended data

                        apdt = "select count(*) from RetRequred where RecordMeter='InsFasap'and DataFrom='FASAP' and RecDate='" + spstr[0].ToString() + "'";
                        dsap = new DataSet();
                        dsap = objvi.GetCount(apdt);
                        strRecordsAppended = dsap.Tables[0].Rows[0][0].ToString();

                        apndcount2 = Convert.ToInt16(dsap.Tables[0].Rows[0][0].ToString());

                        //updated data
                        updcount2 = totalcount2 - apndcount2;
                        strRecordsUpdated = updcount2.ToString();

                        break;

                        #endregion
                    case "FPASSPORT":
                        #region ["FPASSPORT"]
                        DtTm = "select * from LastDownloadDetail where Robot_Name='FPassport'";
                        dsdt = new DataSet();

                        dsdt = objvi.GetCount(DtTm);
                        dt = ConvertDateTime(Convert.ToDateTime(dsdt.Tables[0].Rows[0][2].ToString()));
                        strdt = dt.ToString();
                        spstr = strdt.Split(' ');
                        strLastCompletedDownload = spstr[0].ToString();
                        strLastCompletedDownloadTime = spstr[1].ToString() + spstr[2].ToString();
                        string strc = "select count(*) from retrequred where recdate ='" + System.DateTime.Now.ToString("MM/dd/yyyy") + "'  and salesstatus like 'cancelled %' and auctionadd2 = 'ventura' ";

                        dsdt.Clear();
                        dsdt = objvi.GetCount(strc);
                        int cancelcount = Convert.ToInt32(dsdt.Tables[0].Rows[0][0].ToString());

                        string str = "select count(*) from retrequred where recdate='" + System.DateTime.Now.ToString("MM/dd/yyyy") + "' and APN is not null and auctionadd2 = 'ventura'  ";

                        dsdt.Clear();
                        dsdt = objvi.GetCount(str);
                        strRecordsDownloaded = "Number of APNs Systemwide:" + Convert.ToString(Convert.ToInt32(dsdt.Tables[0].Rows[0][0].ToString()) - cancelcount);


                        str = "";
                        //str = "select Count(*) from retrequred where apn in (select distinct(mainapn) from realquestmaster rq inner join retrequred rr on rr.apn=rq.mainapn where rr.recdate='" + System.DateTime.Now.ToString("MM/dd/yyyy") + "' ) and recdate='" + System.DateTime.Now.ToString("MM/dd/yyyy") + "' and apn!=''";
                        str = "select Count(*) from retrequred where apn in (select distinct(mainapn) from realquestmaster rq inner join retrequred rr on rr.apn=rq.mainapn where rr.recdate='" + System.DateTime.Now.ToString("MM/dd/yyyy") + "' ) and recdate='" + System.DateTime.Now.ToString("MM/dd/yyyy") + "' and apn!='' and auctionadd2='ventura'";

                        dsdt.Clear();
                        dsdt = objvi.GetCount(str);
                        strRecordsUpdated = "Total Comps Downloaded :" + dsdt.Tables[0].Rows[0][0].ToString();
                        break;
                        #endregion
                    case "NATIONWIDE":
                        #region ["NATIONWIDE"]

                        int totalcount, apndcount, updcount;
                        DtTm = "select LastDownloadedTime from LastDownloadDetail where Robot_Name='Nation'";
                        dsdt = new DataSet();
                        dsdt = objvi.GetCount(DtTm);

                        dt = ConvertDateTime(Convert.ToDateTime(dsdt.Tables[0].Rows[0][0].ToString()));

                        strdt = dt.ToString();

                        spstr = strdt.Split(' ');

                        strLastCompletedDownload = spstr[0].ToString();
                        strLastCompletedDownloadTime = spstr[1].ToString() + spstr[2].ToString();
                        strDateofRecordsDownloaded = spstr[0].ToString();

                        //total downloaded data
                        strdwn = "Select count(*) from   RetRequred where (DataFrom = 'Nation' or UpdatedBy like'%Nation%' or RecordMeter ='InsNation' or RecordMeter='UpdNation' )and RecDate ='" + spstr[0].ToString() + "'";
                        dsdwn = new DataSet();
                        dsdwn = objvi.GetCount(strdwn);
                        totalcount = Convert.ToInt16(dsdwn.Tables[0].Rows[0][0].ToString());
                        strRecordsDownloaded = dsdwn.Tables[0].Rows[0][0].ToString();

                        //appended data
                        //EIS:25-Jan-13 ; #276,Ensure the visual indicator for Nationwide is displaying records appended
                        apdt = "Select count(*) from  RetRequred where (DataFrom = 'Nation' and  RecordMeter ='InsNation' )and RecDate ='" + spstr[0].ToString() + "'";
                        dsap = new DataSet();
                        dsap = objvi.GetCount(apdt);
                        apndcount = Convert.ToInt16(dsap.Tables[0].Rows[0][0].ToString());
                        strRecordsAppended = dsap.Tables[0].Rows[0][0].ToString();

                        //updated data
                        string updt = "select count(*) from RetRequred where RecordMeter='UpdNation' and RecDate='" + spstr[0].ToString() + "'";
                        DataSet dsup = new DataSet();
                        dsup = objvi.GetCount(updt);
                        if (dsup.Tables[0].Rows.Count > 0)
                        {
                            strRecordsUpdated = dsup.Tables[0].Rows[0][0].ToString();
                        }
                        else
                        {
                            strRecordsUpdated = "0";
                        }

                        //updated data
                        updcount = totalcount - apndcount;
                        strRecordsUpdated = updcount.ToString();

                        break;
                        #endregion
                    case "REALQUEST":
                        #region ["REALQUEST"]
                        DtTm = "select * from LastDownloadDetail where Robot_Name='RealQuest'";
                        dsdt = new DataSet();
                        dsdt = objvi.GetCount(DtTm);

                        dt = ConvertDateTime(Convert.ToDateTime(dsdt.Tables[0].Rows[0][2].ToString()));

                        strdt = dt.ToString();

                        spstr = strdt.Split(' ');
                        strLastCompletedDownload = spstr[0].ToString();
                        strLastCompletedDownloadTime = spstr[1].ToString() + spstr[2].ToString();

                        strc = "select count(*) from retrequred where recdate ='" + System.DateTime.Now.ToString("MM/dd/yyyy") + "'  and salesstatus like 'cancelled %' and auctionadd2 != 'ventura'";

                        dsdt.Clear();
                        dsdt = objvi.GetCount(strc);
                        cancelcount = Convert.ToInt32(dsdt.Tables[0].Rows[0][0].ToString());
                        str = "select count(*) from retrequred where recdate='" + System.DateTime.Now.ToString("MM/dd/yyyy") + "' and APN is not null and auctionadd2 != 'ventura' ";

                        dsdt.Clear();
                        dsdt = objvi.GetCount(str);
                        strRecordsDownloaded = "Number of APNs Systemwide:" + Convert.ToString(Convert.ToInt32(dsdt.Tables[0].Rows[0][0].ToString()) - cancelcount);

                        str = "";

                        str = "select COUNT(distinct(mainapn)) from realquestmaster rq inner join retrequred rr on rr.apn=rq.mainapn where rr.recdate='" + System.DateTime.Now.ToString("MM/dd/yyyy") + "' and rr.apn!='' and rr.auctionadd2 != 'ventura'";

                        dsdt.Clear();
                        dsdt = objvi.GetCount(str);
                        strRecordsUpdated = "Total Comps Downloaded :" + dsdt.Tables[0].Rows[0][0].ToString();

                        break;
                        #endregion
                    case "TAC":
                        #region ["TAC"]
                        DtTm = "select LastDownloadedTime from LastDownloadDetail where Robot_Name='Tacfore'";
                        dsdt = new DataSet();
                        dsdt = objvi.GetCount(DtTm);

                        dt = ConvertDateTime(Convert.ToDateTime(dsdt.Tables[0].Rows[0][0].ToString()));

                        strdt = dt.ToString();
                        spstr = strdt.Split(' ');

                        strLastCompletedDownload = spstr[0].ToString();
                        strLastCompletedDownloadTime = spstr[1].ToString() + spstr[2].ToString();
                        strDateofRecordsDownloaded = spstr[0].ToString();

                        //total downloaded data
                        strdwn = "select count(*) from Retrequred where (Datafrom='Tacfore' or updatedby='tacfore robot') and RecDate='" + spstr[0].ToString() + "'";
                        dsdwn = new DataSet();
                        dsdwn = objvi.GetCount(strdwn);
                        strRecordsDownloaded = dsdwn.Tables[0].Rows[0][0].ToString();

                        totalcount2 = Convert.ToInt16(dsdwn.Tables[0].Rows[0][0].ToString());
                        //appended data

                        apdt = "select count(*) from RetRequred where RecordMeter='Instacfore'and DataFrom='tacfore' and RecDate='" + spstr[0].ToString() + "'";
                        dsap = new DataSet();
                        dsap = objvi.GetCount(apdt);
                        strRecordsAppended = dsap.Tables[0].Rows[0][0].ToString();

                        apndcount2 = Convert.ToInt16(dsap.Tables[0].Rows[0][0].ToString());

                        //updated data
                        updcount2 = totalcount2 - apndcount2;
                        strRecordsUpdated = updcount2.ToString();

                        break;
                        #endregion
                    case "ZILLOW":
                        #region ["ZILLOW"]
                        DtTm = "select * from LastDownloadDetail where Robot_Name='Zillow'";
                        dsdt = new DataSet();
                        dsdt = objvi.GetCount(DtTm);

                        foreach (DataRow dr in dsdt.Tables[0].Rows)
                        {
                            dt = ConvertDateTime(Convert.ToDateTime(Convert.ToString(dr["LastDownloadedTime"])));

                            strdt = dt.ToString();

                            spstr = strdt.Split(' ');
                            strLastCompletedDownload = spstr[0].ToString();
                            strLastCompletedDownloadTime = spstr[1].ToString() + spstr[2].ToString();
                            strDateofRecordsDownloaded = spstr[0].ToString();
                        }

                        //Total no of records updated 
                        apdt = "select count(*) from RetRequred where Zestimate is not null and Zestimate != 'Not Available' and Zestimate !='' and recdate ='" + strLastCompletedDownload + "'";

                        //Total no of records not available
                        string apdt2 = "select count(*) from RetRequred where Zestimate like'%Not Available%' and RecDate ='" + strLastCompletedDownload + "'";

                        //Total download Queried in site  
                        //string apdt3 = "select count(*) from RetRequred where Zestimate is not null and recdate ='" + lblZillowlastcompletedDownload.Text + "'";

                        dsap = new DataSet();
                        //dsap = objvi.GetCount(apdt + ";" + apdt2 + ";" + apdt3 );
                        dsap = objvi.GetCount(apdt + ";" + apdt2);

                        strRecordsUpdated = dsap.Tables[0].Rows[0][0].ToString();

                        strRecordsDownloaded = "Records with No data :" + dsap.Tables[1].Rows[0][0].ToString();
                        break;
                        #endregion
                }
                //
            }
            catch { }

            return new string[] { slno, strRobotName, strConnection, strLastCompletedDownload, strLastCompletedDownloadTime ,
            strDateofRecordsDownloaded , strRecordsDownloaded ,  strRecordsAppended , strRecordsUpdated  };
        }

        //public void GetAuctionComInfo()
        //{
        //    System.Collections.Generic.Dictionary<string, string> dictAuction = new System.Collections.Generic.Dictionary<string, string>();

        //    DAL objvi = new DAL();

        //    try
        //    {

        //        bool con;
        //        string strdt = string.Empty;

        //        //con = IsConnectionAvailable("http://www.auction.com/California/Los-Angeles-County/Norwalk-pre-foreclosure-trustee-real-estate-auctions.html");
        //        con = IsConnectionAvailable("http://www.auction.com/calendar/all/California");


        //        if (con == true)
        //        {
        //            dictAuction.Add("lblAuctionConnectionVal", "OK");
        //        }
        //        else
        //        {
        //            dictAuction.Add("lblAuctionConnectionVal", "Failed");
        //        }

        //        string DtTm = "select * from LastDownloadDetail WITH(NOLOCK)  where Robot_Name='AuctionCom'";
        //        DataSet dsdt = new DataSet();
        //        dsdt = objvi.GetCount(DtTm);
        //        foreach (DataRow dr in dsdt.Tables[0].Rows)
        //        {
        //            DateTime dt = ConvertDateTime(Convert.ToDateTime(Convert.ToString(dr["LastDownloadedTime"])));
        //            //strdt = Convert.ToString(dr["LastDownloadedTime"]);
        //            strdt = dt.ToString();

        //            string[] spstr = strdt.Split(' ');

        //            lblAuctionLastdownldDate.Text = spstr[0].ToString();
        //            dictAuction.Add("lblAuctionLastdownldDate", lblAuctionLastdownldDate.Text);

        //            lblAuctionLastdownldTime.Text = spstr[1].ToString() + spstr[2].ToString();
        //            dictAuction.Add("lblAuctionLastdownldTime", lblAuctionLastdownldTime.Text);

        //            lblDateofRecordsDwnldedval.Text = spstr[0].ToString();
        //            dictAuction.Add("lblDateofRecordsDwnldedval", lblDateofRecordsDwnldedval.Text);

        //            lblRecordsDwnldvalue.Text = Convert.ToString(dr["DownloadCount"]);
        //            dictAuction.Add("lblRecordsDwnldvalue", lblRecordsDwnldvalue.Text);
        //        }

        //        //updated data
        //        string apdt = "select count(*) from RetRequred where RecordMeter='UpdAuction'and RecDate='" + lblAuctionLastdownldDate.Text + "'";
        //        DataSet dsap = new DataSet();
        //        dsap = objvi.GetCount(apdt);
        //        lblAuctionRcrdsUpdValue.Text = dsap.Tables[0].Rows[0][0].ToString();
        //        dictAuction.Add("lblAuctionRcrdsUpdValue", lblAuctionRcrdsUpdValue.Text);
        //        //apndcount = Convert.ToInt16(dsap.Tables[0].Rows[0][0].ToString());
        //    }
        //    catch { }
        //}

    }

}