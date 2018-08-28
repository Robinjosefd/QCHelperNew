using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;

namespace QCHelperNew
{
    public class clsGetCSV
    {



        public DataSet GetCSV(DateTime qcDate)
        {
            DataSet dsetRetValue  = new DataSet();

            DataTable table = new DataTable();
            table.Columns.Add("SlNo");
            table.Columns.Add("Robot Name");
            table.Columns.Add("CSVCreated?");
            string[] strArray = new string[3];
            int num = 1;
            //base.Session["CsvDate"] = this.qcDate;

            string str = Convert.ToDateTime(qcDate).ToString("MMM-dd-yyyy");
            DataTable subDirectoryList = new DataTable();
            subDirectoryList = clsAmazon.GetSubDirectoryList("CSV/Auction/" + str);
            strArray[0] = num++.ToString();
            strArray[1] = "Auction";
            strArray[2] = (subDirectoryList.Rows.Count > 0) ? "Yes" : "No";
            DataRow row = table.NewRow();
            row.ItemArray = strArray;
            table.Rows.Add(row);
            subDirectoryList = clsAmazon.GetSubDirectoryList("CSV/VAuction/" + str);
            strArray[0] = num++.ToString();
            strArray[1] = "VAuction";
            strArray[2] = (subDirectoryList.Rows.Count > 0) ? "Yes" : "No";
            row = table.NewRow();
            row.ItemArray = strArray;
            table.Rows.Add(row);
            subDirectoryList = clsAmazon.GetSubDirectoryList("CSV/FASAP/" + str);
            strArray[0] = num++.ToString();
            strArray[1] = "FASAP";
            strArray[2] = (subDirectoryList.Rows.Count > 0) ? "Yes" : "No";
            row = table.NewRow();
            row.ItemArray = strArray;
            table.Rows.Add(row);
            subDirectoryList = clsAmazon.GetSubDirectoryList("CSV/VFASAP/" + str);
            strArray[0] = num++.ToString();
            strArray[1] = "VFASAP";
            strArray[2] = (subDirectoryList.Rows.Count > 0) ? "Yes" : "No";
            row = table.NewRow();
            row.ItemArray = strArray;
            table.Rows.Add(row);
            subDirectoryList = clsAmazon.GetSubDirectoryList("CSV/FPassport/" + str);
            strArray[0] = num++.ToString();
            strArray[1] = "FPassport";
            strArray[2] = (subDirectoryList.Rows.Count > 0) ? "Yes" : "No";
            row = table.NewRow();
            row.ItemArray = strArray;
            table.Rows.Add(row);
            subDirectoryList = clsAmazon.GetSubDirectoryList("CSV/NationWidePosting/" + str);
            strArray[0] = num++.ToString();
            strArray[1] = "NationWidePosting";
            strArray[2] = (subDirectoryList.Rows.Count > 0) ? "Yes" : "No";
            row = table.NewRow();
            row.ItemArray = strArray;
            table.Rows.Add(row);            
            subDirectoryList = clsAmazon.GetSubDirectoryList("CSV/VNationWidePosting/" + str);
            strArray[0] = num++.ToString();
            strArray[1] = "VNationWidePosting";
            strArray[2] = (subDirectoryList.Rows.Count > 0) ? "Yes" : "No";
            row = table.NewRow();
            row.ItemArray = strArray;
            table.Rows.Add(row);  
            subDirectoryList = clsAmazon.GetSubDirectoryList("CSV/PriorityPosting/" + str);
            strArray[0] = num++.ToString();
            strArray[1] = "PriorityPosting";
            strArray[2] = (subDirectoryList.Rows.Count > 0) ? "Yes" : "No";
            row = table.NewRow();
            row.ItemArray = strArray;
            table.Rows.Add(row);
            subDirectoryList = clsAmazon.GetSubDirectoryList("CSV/VPriorityPosting/" + str);
            strArray[0] = num++.ToString();
            strArray[1] = "VPriorityPosting";
            strArray[2] = (subDirectoryList.Rows.Count > 0) ? "Yes" : "No";
            row = table.NewRow();
            row.ItemArray = strArray;
            table.Rows.Add(row);
            subDirectoryList = clsAmazon.GetSubDirectoryList("CSV/RPP/" + str);
            strArray[0] = num++.ToString();
            strArray[1] = "RPP";
            strArray[2] = (subDirectoryList.Rows.Count > 0) ? "Yes" : "No";
            row = table.NewRow();
            row.ItemArray = strArray;
            table.Rows.Add(row);
            subDirectoryList = clsAmazon.GetSubDirectoryList("CSV/VRPP/" + str);
            strArray[0] = num++.ToString();
            strArray[1] = "VRPP";
            strArray[2] = (subDirectoryList.Rows.Count > 0) ? "Yes" : "No";
            row = table.NewRow();
            row.ItemArray = strArray;
            table.Rows.Add(row);
            subDirectoryList = clsAmazon.GetSubDirectoryList("CSV/RealQuest/" + str);
            strArray[0] = num++.ToString();
            strArray[1] = "RealQuest";
            strArray[2] = (subDirectoryList.Rows.Count > 0) ? "Yes" : "No";
            row = table.NewRow();
            row.ItemArray = strArray;
            table.Rows.Add(row);
            
            /*
            subDirectoryList = clsAmazon.GetSubDirectoryList("CSV/TacForeclosure/" + str);
            strArray[0] = num++.ToString();
            strArray[1] = "TacForeclosure";
            strArray[2] = (subDirectoryList.Rows.Count > 0) ? "Yes" : "No";
            row = table.NewRow();
            row.ItemArray = strArray;
            table.Rows.Add(row);
            */
            
            dsetRetValue.Tables.Add(table);
            table = new DataTable();
            table.Columns.Add("SlNo");
            table.Columns.Add("Robot Name");
            table.Columns.Add("CSVCreated?");
            subDirectoryList = clsAmazon.GetSubDirectoryList("CSV/Zillow/" + str);
            strArray[0] = num++.ToString();
            strArray[1] = "Zillow";
            strArray[2] = (subDirectoryList.Rows.Count > 0) ? "Yes" : "No";
            row = table.NewRow();
            row.ItemArray = strArray;
            table.Rows.Add(row);
            subDirectoryList = clsAmazon.GetSubDirectoryList("CSV/MigratingDataToRetRequred/" + str);
            strArray[0] = num++.ToString();
            strArray[1] = "Migration Auction";
            strArray[2] = (subDirectoryList.Select("Filename LIKE '%Auction%'").Length > 0) ? "Yes" : "No";
            row = table.NewRow();
            row.ItemArray = strArray;
            table.Rows.Add(row);
            strArray[0] = num++.ToString();
            strArray[1] = "Migration FASAP";
            strArray[2] = (subDirectoryList.Select("Filename LIKE '%FASAP%'").Length > 0) ? "Yes" : "No";
            row = table.NewRow();
            row.ItemArray = strArray;
            table.Rows.Add(row);
            strArray[0] = num++.ToString();
            strArray[1] = "Migration GoogleAddress";
            strArray[2] = (subDirectoryList.Select("Filename LIKE '%GoogleAddress%'").Length > 0) ? "Yes" : "No";
            row = table.NewRow();
            row.ItemArray = strArray;
            table.Rows.Add(row);
            strArray[0] = num++.ToString();
            strArray[1] = "Migration Nation";
            strArray[2] = (subDirectoryList.Select("Filename LIKE '%NationWidePosting%'").Length > 0) ? "Yes" : "No";
            row = table.NewRow();
            row.ItemArray = strArray;
            table.Rows.Add(row);
            strArray[0] = num++.ToString();
            strArray[1] = "Migration Priority";
            strArray[2] = (subDirectoryList.Select("Filename LIKE '%PriorityPosting%'").Length > 0) ? "Yes" : "No";
            row = table.NewRow();
            row.ItemArray = strArray;
            table.Rows.Add(row);
            strArray[0] = num++.ToString();
            strArray[1] = "Migration RPP";
            strArray[2] = (subDirectoryList.Select("Filename LIKE '%RPP%'").Length > 0) ? "Yes" : "No";
            row = table.NewRow();
            row.ItemArray = strArray;
            table.Rows.Add(row);
            strArray[0] = num++.ToString();
            strArray[1] = "Migration RealQuest";
            strArray[2] = (subDirectoryList.Select("Filename LIKE '%RealQuest%'").Length > 0) ? "Yes" : "No";
            row = table.NewRow();
            row.ItemArray = strArray;
            table.Rows.Add(row);
            strArray[0] = num++.ToString();
            strArray[1] = "Migration Tac";
            strArray[2] = (subDirectoryList.Select("Filename LIKE '%TacForeclosure%'").Length > 0) ? "Yes" : "No";
            row = table.NewRow();
            row.ItemArray = strArray;
            table.Rows.Add(row);
            strArray[0] = num++.ToString();
            strArray[1] = "Migration Zillow";
            strArray[2] = (subDirectoryList.Select("Filename LIKE '%Zillow%'").Length > 0) ? "Yes" : "No";
            row = table.NewRow();
            row.ItemArray = strArray;
            table.Rows.Add(row);
            strArray[0] = num++.ToString();
            strArray[1] = "Migration FPassport";
            strArray[2] = (subDirectoryList.Select("Filename LIKE '%FPassport%'").Length > 0) ? "Yes" : "No";
            row = table.NewRow();
            row.ItemArray = strArray;
            table.Rows.Add(row);
            dsetRetValue.Tables.Add(table);
           // base.Session["CsvData"] = set;
            return dsetRetValue;
        }



    }



}
