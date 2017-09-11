using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;


namespace QCHelperNew
{
    //SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["ForeClosure"].ConnectionString);
    //SqlDataAdapter da;
    //DataTable dt;
    //SqlCommand cmd;


    public partial class Default1 : System.Web.UI.Page
    {
        DAL dal = new DAL();


        System.Threading.Thread thrd_BindCount_1 = null;
        System.Threading.Thread thrd_BindCount_2 = null;
        System.Threading.Thread thrd_BindCount_3 = null;
        System.Threading.Thread thrd_BindCount_4 = null;
        System.Threading.Thread thrd_bindCSV = null;
        System.Threading.Thread thrd_BindRobotLog = null;

        static object obj_QCCount1_1to11 = null;
        static object obj_QCCount2_12_TotalUpdatesForAllFields = null;
        static object obj_QCCount3_12_FRADARupdatesbyotherRobots = null;
        static object obj_QCCount4_13_FRADARupdatesFromotherRobots = null;

        static object obj_CSVCreated1 = null;
        static object obj_CSVCreated2 = null;

        static object obj_RobotLog = null;


        protected void Page_Load(object sender, EventArgs e)
        {
            if (!base.IsPostBack)
            {
                MakeAllNull();

                this.txtDate.Text = DateTime.Now.Date.ToString("yyyy/MMM/dd");
                this.timerDisp();
                //

                LoadAllValues();
            }
            //this.ucQCCounts.qcDate = this.txtDate.Text;
            //this.ucQCCsv.qcDate = this.txtDate.Text;

        }

        private void MakeAllNull()
        {
            obj_QCCount1_1to11 = null;
            obj_QCCount2_12_TotalUpdatesForAllFields = null;
            obj_QCCount3_12_FRADARupdatesbyotherRobots = null;
            obj_QCCount4_13_FRADARupdatesFromotherRobots = null;

            obj_CSVCreated1 = null;
            obj_CSVCreated2 = null;

            obj_RobotLog = null;
        }

        private void MakeAllThreadNull()
        {
            try
            {
                if (thrd_BindCount_1 != null)
                {
                    thrd_BindCount_1.Abort();
                }
                if (thrd_BindCount_2 != null)
                {
                    thrd_BindCount_2.Abort();
                }
                if (thrd_BindCount_3 != null)
                {
                    thrd_BindCount_3.Abort();
                }
                if (thrd_BindCount_4 != null)
                {
                    thrd_BindCount_4.Abort();
                }
                if (thrd_bindCSV != null)
                {
                    thrd_bindCSV.Abort();
                }
                if (thrd_BindRobotLog != null)
                {
                    thrd_BindRobotLog.Abort();
                }
            }
            catch { }
        }


        protected void btnShowQC_Click(object sender, EventArgs e)
        {
            //this.ucQCCounts.qcDate = this.txtDate.Text;
            //this.ucQCCsv.qcDate = this.txtDate.Text;
        }

        private void LoadAllValues()
        {
            if (thrd_BindCount_1 == null)
            {
                thrd_BindCount_1 = new System.Threading.Thread(() => this.BindCount_1());
                thrd_BindCount_1.Start();
            }
            if (thrd_BindCount_2 == null)
            {
                thrd_BindCount_2 = new System.Threading.Thread(() => this.BindCount_2());
                thrd_BindCount_2.Start();
            }
            if (thrd_BindCount_3 == null)
            {
                thrd_BindCount_3 = new System.Threading.Thread(() => this.BindCount_3());
                thrd_BindCount_3.Start();
            }
            if (thrd_BindCount_4 == null)
            {
                thrd_BindCount_4 = new System.Threading.Thread(() => this.BindCount_4());
                thrd_BindCount_4.Start();
            }
            if (thrd_bindCSV == null)
            {
                thrd_bindCSV = new System.Threading.Thread(() => this.bindCSV());
                thrd_bindCSV.Start();
            }
            if (thrd_BindRobotLog == null)
            {
                thrd_BindRobotLog = new System.Threading.Thread(() => this.BindRobotLog());
                thrd_BindRobotLog.Start();
            }
        }




        #region Timer DIsplay Area
        protected void timerDisp()
        {
            DateTime now = DateTime.Now;
            this.lblAppharborTime.Text = TimeZoneInfo.ConvertTimeToUtc(now, TimeZoneInfo.Local).ToString("dddd, MMM-dd-yyyy, HH:mm:ss");
            TimeZoneInfo destinationTimeZone = TimeZoneInfo.FindSystemTimeZoneById("Pacific Standard Time");
            this.lblPsttime.Text = TimeZoneInfo.ConvertTimeFromUtc(now.ToUniversalTime(), destinationTimeZone).ToString("dddd, MMM-dd-yyyy, HH:mm:ss");
            TimeZoneInfo info2 = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
            this.lblIST.Text = TimeZoneInfo.ConvertTimeFromUtc(now.ToUniversalTime(), info2).ToString("dddd, MMM-dd-yyyy, HH:mm:ss");
        }


        #endregion

        private DataTable GetTestData(string countQueryName)
        {
            DataTable dtable = new DataTable();
            dtable.Columns.Add(countQueryName);
            //dtable.Columns.Add("");
            dtable.Columns.Add("...");

            dtable.Rows.Add(new string[] { "***Retrieving data****", DateTime.Now.ToString() });

            //dtable.Rows.Add(new string[] { "2", "B", DateTime.Now.ToString() });
            
            return dtable;
        }


        protected void tmr_Tick(object sender, EventArgs e)
        {
            //obj_QCCount1_1to11 = null;
            if (obj_QCCount1_1to11 != null)
            {
                this.dxGrdCount.DataSource = (DataTable)obj_QCCount1_1to11;
                this.dxGrdCount.DataBind();
            }
            else
            {
                this.dxGrdCount.DataSource = GetTestData("QCCount1_1to11");
                this.dxGrdCount.DataBind();
            }
            //obj_QCCount2_12_TotalUpdatesForAllFields
            if (obj_QCCount2_12_TotalUpdatesForAllFields != null)
            {
                this.dxGrdCount_2.DataSource = (DataTable)obj_QCCount2_12_TotalUpdatesForAllFields;
                this.dxGrdCount_2.DataBind();
            }
            else
            {
                this.dxGrdCount_2.DataSource = GetTestData("QCCount2_12_TotalUpdatesForAllFields");
                this.dxGrdCount_2.DataBind();
            }
            //obj_QCCount3_12_FRADARupdatesbyotherRobots
            if (obj_QCCount3_12_FRADARupdatesbyotherRobots != null)
            {
                this.dxGrdCount_3.DataSource = (DataTable)obj_QCCount3_12_FRADARupdatesbyotherRobots;
                this.dxGrdCount_3.DataBind();
            }
            else
            {
                this.dxGrdCount_3.DataSource = GetTestData("QCCount3_12_FRADARupdatesbyotherRobots");
                this.dxGrdCount_3.DataBind();
            }
            //obj_QCCount4_13_FRADARupdatesFromotherRobots
            if (obj_QCCount4_13_FRADARupdatesFromotherRobots != null)
            {
                this.dxGrdCount_4.DataSource = (DataTable)obj_QCCount4_13_FRADARupdatesFromotherRobots;
                this.dxGrdCount_4.DataBind();
            }
            else
            {
                this.dxGrdCount_4.DataSource = GetTestData("QCCount4_13_FRADARupdatesFromotherRobots");
                this.dxGrdCount_4.DataBind();
            }


            //obj_CSVCreated1
            if (obj_CSVCreated1 != null)
            {
                this.dxGrdQCCSV1.DataSource = (DataTable)obj_CSVCreated1;
                this.dxGrdQCCSV1.DataBind();
            }
            else
            {
                this.dxGrdQCCSV1.DataSource = GetTestData("CSVCreated1");
                this.dxGrdQCCSV1.DataBind();
            }

            //obj_CSVCreated2
            if (obj_CSVCreated2 != null)
            {
                this.dxGrdQCCSV2.DataSource = (DataTable)obj_CSVCreated2;
                this.dxGrdQCCSV2.DataBind();
            }
            else
            {
                this.dxGrdQCCSV2.DataSource = GetTestData("CSVCreated2");
                this.dxGrdQCCSV2.DataBind();
            }

            if (obj_RobotLog != null)
            {
                this.dxGrdRobotLog.DataSource = (DataTable)obj_RobotLog;
                this.dxGrdRobotLog.DataBind();
            }
            else
            {
                this.dxGrdRobotLog.DataSource = GetTestData("obj_RobotLog");
                this.dxGrdRobotLog.DataBind();
            }


            timerDisp();
        }

        #region QCCOunts Area
        private void BindCount_1()
        {
            try
            {
                DataTable table = new DataTable();
                table = cutomizeDataset(dal.GetQCCounts(DAL.CountQueries.QCCount1_1to11, txtDate.Text));
                //Session["QCCount1_1to11"] = table;                 
                obj_QCCount1_1to11 = table;
                this.lblError.Text = "Morning Rundown";
                this.dxGrdCount.DataSource = table;
                this.dxGrdCount.DataBind();
            }
            catch (Exception exception)
            {
                this.lblError.Text = this.lblError.Text + exception.ToString();
            }
        }

        private void BindCount_2()
        {
            try
            {
                DataTable table = new DataTable();
                table = cutomizeDataset(dal.GetQCCounts(DAL.CountQueries.QCCount2_12_TotalUpdatesForAllFields, txtDate.Text));
                //Session["QCCount1_1to11"] = table;                 
                obj_QCCount2_12_TotalUpdatesForAllFields = table;
                this.lblError.Text = "Morning Rundown";
                this.dxGrdCount_2.DataSource = table;
                this.dxGrdCount_2.DataBind();
            }
            catch (Exception exception)
            {
                this.lblError.Text = this.lblError.Text + exception.ToString();
            }
        }

        private void BindCount_3()
        {
            try
            {
                DataTable table = new DataTable();
                table = cutomizeDataset(dal.GetQCCounts(DAL.CountQueries.QCCount3_12_FRADARupdatesbyotherRobots, txtDate.Text));
                //Session["QCCount1_1to11"] = table;                 
                obj_QCCount3_12_FRADARupdatesbyotherRobots = table;
                this.lblError.Text = "Morning Rundown";
                this.dxGrdCount_3.DataSource = table;
                this.dxGrdCount_3.DataBind();
            }
            catch (Exception exception)
            {
                this.lblError.Text = this.lblError.Text + exception.ToString();
            }
        }

        private void BindCount_4()
        {
            try
            {
                DataTable table = new DataTable();
                table = cutomizeDataset(dal.GetQCCounts(DAL.CountQueries.QCCount4_13_FRADARupdatesFromotherRobots, txtDate.Text));
                //Session["QCCount1_1to11"] = table;                 
                obj_QCCount4_13_FRADARupdatesFromotherRobots = table;
                this.lblError.Text = "Morning Rundown";
                this.dxGrdCount_4.DataSource = table;
                this.dxGrdCount_4.DataBind();
            }
            catch (Exception exception)
            {
                this.lblError.Text = this.lblError.Text + exception.ToString();
            }
        }


        private DataTable cutomizeDataset(DataSet ds)
        {
            DataTable table = new DataTable();
            table.Columns.Add("SlNo");
            table.Columns.Add("QCItem");
            table.Columns.Add("QCCount");
            int num = 1;
            foreach (DataTable table2 in ds.Tables)
            {
                foreach (DataColumn column in table2.Columns)
                {
                    string columnName = column.ColumnName;
                    string str2 = table2.Rows[0][columnName].ToString();
                    if ((columnName.Split(new char[] { ' ' }).Length > 3) && !columnName.Contains("sold"))
                    {
                        string[] strArray = new string[] { num++.ToString(), Regex.Replace(columnName, @"[0-9]*\.", ""), str2 };
                        DataRow row = table.NewRow();
                        row.ItemArray = strArray;
                        table.Rows.Add(row);
                    }
                }
            }
            // Session["data"] = table;
            return table;
        }


        #endregion


        #region CSVCreated Area
        protected void bindCSV()
        {
            DataSet dset = new DataSet();
            clsGetCSV clsGetCSV = new clsGetCSV();


            DateTime dttime = Convert.ToDateTime(txtDate.Text);


            dset = clsGetCSV.GetCSV(dttime);
            this.lblCSVCreated.Text = "CSV Files Created";

            obj_CSVCreated1 = dset.Tables[0];
            obj_CSVCreated2 = dset.Tables[1];

            this.dxGrdQCCSV1.DataSource = dset.Tables[0];
            this.dxGrdQCCSV1.DataBind();
            this.dxGrdQCCSV2.DataSource = dset.Tables[1];
            this.dxGrdQCCSV2.DataBind();
        }


        #endregion
        

        #region BindLog
        protected void BindRobotLog()
        {
            try
            {
                DataTable table = new DataTable();
                table = dal.GetQCCounts(DAL.CountQueries.LogData, txtDate.Text).Tables[0];

                obj_RobotLog = table;
                this.lblQClog.Text = "Log Created";
                this.dxGrdRobotLog.DataSource = table;
                this.dxGrdRobotLog.DataBind();
            }
            catch (Exception exception)
            {
                this.lblQClog.Text = exception.ToString();
            }
        }

        #endregion
    }
}
