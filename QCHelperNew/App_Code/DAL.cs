using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;

namespace QCHelperNew
{
    public class DAL
    {
        //SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ForeClosure"].ConnectionString);
        //SqlDataAdapter da;
        //DataTable dt;
        //DataSet ds;
        //SqlCommand cmd;

        public void AUDOP(string str)
        {
            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ForeClosure"].ConnectionString);
            SqlCommand cmd;
            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            con.Open();
            cmd = new SqlCommand(str, con);
            cmd.ExecuteNonQuery();
            con.Close();
        }

        public DataSet GetCount(string str)
        {
            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ForeClosure"].ConnectionString);
            SqlDataAdapter da;
            DataSet ds;
            SqlCommand cmd;

            cmd = new SqlCommand(str, con);
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 2700;

            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            con.Open();

            da = new SqlDataAdapter(cmd);
            ds = new DataSet();
            da.Fill(ds);
            con.Close();
            return ds;
        }

        private DataSet GetCount(string str, int index)
        {
            SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["ForeClosure"].ConnectionString);
            SqlDataAdapter da;
            DataSet ds;
            SqlCommand cmd;

            cmd = new SqlCommand(str, con);
            cmd.CommandType = CommandType.Text;
            cmd.CommandTimeout = 2700;

            if (con.State == ConnectionState.Open)
            {
                con.Close();
            }
            con.Open();

            da = new SqlDataAdapter(cmd);
            ds = new DataSet();
            da.Fill(ds);
            con.Close();
            return ds;
        }

        //public DataSet GetQCCounts(CountQueries countQueries, DateTime date)
        public DataSet GetQCCounts(CountQueries countQueries, string date)
        {
            DataSet dsetRetVal = new DataSet();
            string strdate = date;
            if (countQueries == CountQueries.QCCount1_1to11)
            {
                dsetRetVal = GetCount(GetQCCounts_1(strdate), 0);
            }
            else if (countQueries == CountQueries.QCCount2_12_TotalUpdatesForAllFields)
            {
                dsetRetVal = GetCount(GetQCCount2_12_TotalUpdatesForAllFields(strdate), 1);
            }
            else if (countQueries == CountQueries.QCCount3_12_FRADARupdatesbyotherRobots)
            {
                dsetRetVal = GetCount(GetQCCount3_12_FRADARupdatesbyotherRobots(strdate), 2);
            }
            else if (countQueries == CountQueries.QCCount4_13_FRADARupdatesFromotherRobots)
            {
                dsetRetVal = GetCount(GetQCCount4_13_FRADARupdatesFromotherRobots(strdate), 3);
            }
            else if (countQueries == CountQueries.LogData)
            {
                dsetRetVal = GetCount(GetRobotLog(strdate), 4);
            }

            return dsetRetVal;
        }

        public enum CountQueries
        {
            QCCount1_1to11 = 1,
            QCCount2_12_TotalUpdatesForAllFields = 2,
            QCCount3_12_FRADARupdatesbyotherRobots = 3,
            QCCount4_13_FRADARupdatesFromotherRobots = 4,
            LogData = 5
        }

        #region SQLS

        private string GetQCCounts_1(string datetime)
        {
            string strRetVal = @"

declare @date datetime = '" + datetime + @"'
--1. Number of properties for today's auctions
SELECT count(*) AS '1. Number of properties for today''s auctions'
FROM
	RetRequred WITH(NOLOCK)
WHERE
	convert(DATE, RecDate) = convert(DATE, @date)
	OR convert(DATE, AuctionDate) = convert(DATE, @date)or CONVERT(date, RecapDate) = CONVERT(date, @date);
--2. Number of properties postponed (sales status like postponed > current date)
SELECT count(*) AS '2. Number of properties postponed'
FROM
	RetRequred WITH(NOLOCK)
WHERE
	--convert(DATE, substring(SalesStatus, (patindex('%[0-9]%', SalesStatus)), 10)) > convert(DATE, @date)
	CONVERT(DATE,SUBSTRING(SalesStatus, CHARINDEX ('/' ,(SalesStatus)) - 2, LEN(SalesStatus)- CHARINDEX ('/' ,REVERSE(SalesStatus)) - CHARINDEX ('/' ,(SalesStatus)) + 8)) > convert(DATE, @date)
	AND (convert(DATE, RecDate) = convert(DATE, @date)
	OR convert(DATE, AuctionDate) = convert(DATE, @date)or CONVERT(date, RecapDate) = CONVERT(date, @date))
	AND SalesStatus LIKE 'post%' AND len ( SalesStatus)!=len('Postponed');
	
--3. Number of properties cancelled (sales status like cancelled)
SELECT count(*) AS '3. Number of properties cancelled'
FROM
	RetRequred WITH(NOLOCK)
WHERE
	(convert(DATE, RecDate) = convert(DATE, @date)
	OR convert(DATE, AuctionDate) = convert(DATE, @date)or CONVERT(date, RecapDate) = CONVERT(date, @date))
	AND SalesStatus LIKE 'cancel%';
--4. Number of properties sold
SELECT count(*) AS '4. Number of properties sold'
FROM
	RetRequred WITH(NOLOCK)
WHERE
	(convert(DATE, RecDate) = convert(DATE, @date)
	OR convert(DATE, AuctionDate) = convert(DATE, @date)or CONVERT(date, RecapDate) = CONVERT(date, @date))
	AND (SalesStatus LIKE 'sold%') -- OR StatusId = 22);
--StatusId = 22
--5. Number of properties from FRADAR (DataFrom = FRADAR)
--SELECT count(*) AS '5. Number of properties from FRADAR'
--FROM
--	RetRequred
--WHERE
--	(convert(DATE, RecDate) = convert(DATE, @date)
--	OR convert(DATE, AuctionDate) = convert(DATE, @date))
--	AND DataFrom LIKE '%FRADAR%'
---------------------------------------------------------------------------------------------------
---Updated by lima @ eis on 05/05/2014
/* The FRADAR count should be a count of only the records included in the last LA & Ventura FRADAR uploads 
for the selected date. The Appended count should be a count of all the records that were not included in the 
Last LA & Ventura FRADAR uploads for the selected date*/
select
COUNT(RetFlId) AS '5. Number of properties from FRADAR'
FROM RetRequred WITH(NOLOCK) where 
(CONVERT(date, recdate)=CONVERT(date,@date) OR CONVERT(date,AuctionDate)=CONVERT(date,@date)or CONVERT(date, RecapDate) = CONVERT(date, @date))
AND (APN  in (select isnull(param,'') from dbo.fn_MVParam((select top 1 apn from compsmaster WITH(NOLOCK) where CONVERT(date, recdate)=CONVERT(date,@date) and county='LA'
order by compsid desc),',') 
UNION
select isnull(param,'') from dbo.fn_MVParam((select top 1 apn from compsmaster WITH(NOLOCK) where CONVERT(date, recdate)=CONVERT(date,@date) and county='VENTURA'
order by compsid desc),',')))and  APN IS NOT  NULL AND LTRIM(RTRIM(APN))!=''

--6. Number of properties appended (DataFrom != FRADAR)
--SELECT count(*) AS '6. Number of properties appended'
--FROM
--	RetRequred
--WHERE
--	(convert(DATE, RecDate) = convert(DATE, @date)
--	OR convert(DATE, AuctionDate) = convert(DATE, @date))
--	AND DataFrom NOT LIKE '%FRADAR%'
------------------------------------------------------------------------------------------------------------------------------
---Updated by lima @ eis on 05/05/2014
/* The FRADAR count should be a count of only the records included in the last LA & Ventura FRADAR uploads 
for the selected date. The Appended count should be a count of all the records that were not included in the 
Last LA & Ventura FRADAR uploads for the selected date*/
select
COUNT(RetFlId) AS '6. Number of properties appended' 
FROM RetRequred WITH(NOLOCK) where 
(CONVERT(date, recdate)=CONVERT(date,@date) OR CONVERT(date,AuctionDate)=CONVERT(date,@date)or CONVERT(date, RecapDate) = CONVERT(date, @date))
AND (APN not in (select isnull(param,'') from dbo.fn_MVParam((select top 1 apn from compsmaster WITH(NOLOCK) where CONVERT(date, recdate)=CONVERT(date,@date) and county='LA'
order by compsid desc),',') 
UNION
select isnull(param,'') from dbo.fn_MVParam((select top 1 apn from compsmaster WITH(NOLOCK) where CONVERT(date, recdate)=CONVERT(date,@date) and county='VENTURA'
order by compsid desc),',')) or APN IS NULL or LTRIM(RTRIM(APN))='')
--7. Number of properties with OpeningBids
SELECT count(*) AS '7. Number of properties with OpeningBids'
FROM
	RetRequred WITH(NOLOCK)
WHERE
	(convert(DATE, RecDate) = convert(DATE, @date)
	OR convert(DATE, AuctionDate) = convert(DATE, @date)or CONVERT(date, RecapDate) = CONVERT(date, @date))
	AND patindex('%not%', isnull(OpeningBid, 'Not')) = 0
	AND OpeningBid != '0'
	AND OpeningBid != '0.0'
	AND OpeningBid != '0.00'
	AND ltrim(rtrim(OpeningBid))!='' 
--8. Number of properties with Zestimates
SELECT count(*) AS '8. Number of properties with Zestimates'
FROM
	RetRequred WITH(NOLOCK)
WHERE
	(convert(DATE, RecDate) = convert(DATE, @date)
	OR convert(DATE, AuctionDate) = convert(DATE, @date)or CONVERT(date, RecapDate) = CONVERT(date, @date))
	AND patindex('%not%', isnull(Zestimate, 'Not')) = 0
--9. Number of properties with comps (RealQuest + FPassport)
SELECT count(*) AS '9. Number of properties with comps'
FROM
	RetRequred WITH(NOLOCK)
WHERE
	(convert(DATE, RecDate) = convert(DATE, @date)
	OR convert(DATE, AuctionDate) = convert(DATE, @date)or CONVERT(date, RecapDate) = CONVERT(date, @date))
	AND HaveComps = 1
--SELECT count(DISTINCT mainapn) AS '9. Number of properties with comps'
--FROM
--	realquestmaster
--WHERE
--	convert(DATE, recdate) = convert(DATE, @date)

--10. Number of properties that should have comps
SELECT count(apn) AS '10. Number of properties that should have comps(Records having APN)'
FROM
	retrequred WITH(NOLOCK)
WHERE
	(convert(DATE, RecDate) = convert(DATE, @date)
	OR convert(DATE, AuctionDate) = convert(DATE, @date)or CONVERT(date, RecapDate) = CONVERT(date, @date))
	AND isnull(APN,'')! = ''

	--11. Number of properties  updated 

SELECT count(*) AS '11. Number of properties updated'
FROM
	retrequred WITH(NOLOCK)
WHERE
	(convert(DATE, RecDate) = convert(DATE, @date)
	OR convert(DATE, AuctionDate) = convert(DATE, @date)or CONVERT(date, RecapDate) = CONVERT(date, @date))
	AND (isnull(updatedby, '') != ''
	OR RetUpdateDate >= @date);
";

            return strRetVal;

        }

        private string GetQCCount2_12_TotalUpdatesForAllFields(string datetime)
        {
            string strRetVal = @"
declare @date datetime = '" + datetime + @"'
--12. Number of properties  updates on the following fields: 
--		SalesStatus, SitusStreet, Situs1, Situs2, Situs3, AuctionDate, AuctionTime, 
--		MinBid, AuctionAdd1, AuctionAdd2, OpeningBid, Zestimate, GoogleAddress, HaveComps
SELECT 
(SELECT count(*) FROM Audit WITH(NOLOCK) WHERE convert(DATE, UpdateDate) = convert(DATE, @date) AND lower(Type) = 'update' 
AND (Insert_UpdatedBy NOT LIKE '%FRADAR%' AND Insert_UpdatedBy LIKE 'upd%' 
AND Insert_UpdatedBy NOT LIKE '%EIS%' AND Insert_UpdatedBy NOT LIKE 'UpdBy%' 
 AND Insert_UpdatedBy NOT LIKE 'Ins%' AND Insert_UpdatedBy NOT LIKE 'tgr%' 
 AND Insert_UpdatedBy NOT LIKE 'UpdCls%' AND Insert_UpdatedBy NOT LIKE 'MigrationRobot%'
 AND Insert_UpdatedBy NOT LIKE 'UpdMigration_SetAppend' 
 AND Insert_UpdatedBy NOT LIKE 'UpdMigration_ResetAppend' 
 )
AND FieldName not in('AsapUdated', 'BUPStatus', 'DefaultPer', 'GoogleAddressUpdateDate', 'HaveResearchFiles', 'isGoogleAddressUpdated', 'IsPrinted', 
 'IsUpdated', 'MovedFromNM', 'NationUdated', 'OpeningBidUpdStatus', 'postponecount', 'PpostinUdated', 'PriorityRank', 'reconUdated', 'RecordMeter', 
 'RetUpdateDate', 'RPPUdated', 'UpdatedBy', 'AuctionAddressFormatted', 'AuctionTimeFormatted', 'OpeningBidFormatted', 'TabStatus',
 'PerChange', 'RecapDate', 'RecDate', 'StatusDate', 'StatusId', 'ZestimateUpdatedDate' -- Excluded on 6-August-2014
 )
) AS 'Total Updates for all fields'
    ";
            return strRetVal;

        }

        private string GetQCCount3_12_FRADARupdatesbyotherRobots(string datetime)
        {
            string strRetVal = @"
declare @date datetime = '" + datetime + @"'

--12. Number of FRADAR properties updated by other robots
select count(DISTINCT PK) as '12. Number of FRADAR properties updated by other robots'
FROM Audit  aud  WITH(NOLOCK) where PK  IN (
SELECT '<RetFlId='+convert(NVARCHAR, RetFlId)+'>'  FROM RetRequred WITH(NOLOCK) where
( convert(DATE,AuctionDate)=convert(DATE,@Date) OR convert(DATE,RecDate)=convert(DATE,@Date)or CONVERT(date, RecapDate) = CONVERT(date, @date))
 AND (DataFrom='FRADAR' and convert(date,RetUpdateDate)=convert(DATE, @date)) )
 AND FieldName IN('OpeningBid','MinBid','HaveComps','Zestimate','SalesStatus')
AND convert(DATE , UpdateDate)=convert(DATE , @Date) AND Type='Update'
AND ( Insert_UpdatedBy NOT LIKE 'tgr%' AND Insert_UpdatedBy NOT LIKE 'MigrationRobot%'
 AND Insert_UpdatedBy NOT LIKE 'UpdMigration_SetAppend' 
 AND Insert_UpdatedBy NOT LIKE 'UpdMigration_ResetAppend' 
);
    ";
            return strRetVal;

        }

        private string GetQCCount4_13_FRADARupdatesFromotherRobots(string datetime)
        {
            string strRetVal = @"
declare @date datetime = '" + datetime + @"' 
--13. Number of updates to FRADAR properties from other robots
SELECT  count(*) AS '13. Number of updates to FRADAR properties from other robots' FROM Audit WITH(NOLOCK) where PK IN (
SELECT '<RetFlId='+convert(NVARCHAR, RetFlId)+'>'  FROM RetRequred WITH(NOLOCK) where( 
convert(DATE,AuctionDate)=convert(DATE,@Date) OR convert(DATE,RecDate)=convert(DATE,@Date)or CONVERT(date, RecapDate) = CONVERT(date, @date)) 
AND (DataFrom='FRADAR'and convert(date,RetUpdateDate)=convert(DATE, @date)))
AND FieldName IN('OpeningBid','MinBid','HaveComps','Zestimate','SalesStatus')
AND convert(DATE , UpdateDate)=convert(DATE , @Date) AND Type='Update'
AND ( Insert_UpdatedBy NOT LIKE 'tgr%' AND Insert_UpdatedBy NOT LIKE 'MigrationRobot%' AND Insert_UpdatedBy NOT LIKE 'updClsGridAdjustments'  
AND Insert_UpdatedBy NOT LIKE 'UpdGoogleAddress' AND Insert_UpdatedBy NOT LIKE 'UpdFradar'
AND Insert_UpdatedBy NOT LIKE 'UpdGmail' AND Insert_UpdatedBy NOT LIKE 'UpdResearcher' 
 AND Insert_UpdatedBy NOT LIKE 'UpdMigration_SetAppend' 
 AND Insert_UpdatedBy NOT LIKE 'UpdMigration_ResetAppend' 
)
    ";
            return strRetVal;

        }

        private string GetRobotLog(string datetime)
        {
            string strRetVal = @"
declare @LogDate datetime = '" + datetime + @"'  

SET @LogDate = DATEADD(MINUTE,-10,GETDATE());
	--1-Auction 
   ---------------------------------------------------------------
   SELECT(
	SELECT CASE WHEN COUNT(*) > 0 THEN 'YES' ELSE 'NO' END FROM Log_Auction WITH(NOLOCK) WHERE LogDate > @LogDate
   ) AS 'Auction.Com'
   
   --2-FASAP 
   ---------------------------------------------------------------
   SELECT(
	SELECT CASE WHEN COUNT(*) > 0 THEN 'YES' ELSE 'NO' END FROM Log_FASAP WITH(NOLOCK) WHERE LogDate > @LogDate
   ) AS 'FASAP'
   
   --3-FPassport 
   -------------------------------------------------------------
  SELECT(
	SELECT CASE WHEN COUNT(*) > 0 THEN 'YES' ELSE 'NO' END FROM Log_FPassport WITH(NOLOCK) WHERE LogDate > @LogDate
   ) AS 'Fidelity Passport'
  
   --4-MigratingDataToRetRequred 
   ---------------------------------------------------------------
   SELECT(
	SELECT CASE WHEN COUNT(*) > 0 THEN 'YES' ELSE 'NO' END FROM Log_MigratingDataToRetRequred WITH(NOLOCK) WHERE LogDate > @LogDate
   ) AS 'Migration Agent'
   
   --5-NationWidePosting 
   ---------------------------------------------------------------
   SELECT(
	SELECT CASE WHEN COUNT(*) > 0 THEN 'YES' ELSE 'NO' END FROM Log_NationWidePosting WITH(NOLOCK) WHERE LogDate > @LogDate
   ) AS 'NationWide Posting'
   
   --6-RealQuest 
   ---------------------------------------------------------------
   SELECT(
	SELECT CASE WHEN COUNT(*) > 0 THEN 'YES' ELSE 'NO' END FROM Log_RealQuest WITH(NOLOCK) WHERE LogDate > @LogDate
   ) AS 'RealQuest'
   
  /* --7-Recontrust 
   ---------------------------------------------------------------
   SELECT(
	SELECT CASE WHEN COUNT(*) > 0 THEN 'YES' ELSE 'NO' END FROM Log_MRecontrust WHERE LogDate > @LogDate
   ) AS 'Recontrust'
   
   --8-RPP 
   ---------------------------------------------------------------
   SELECT(
	SELECT CASE WHEN COUNT(*) > 0 THEN 'YES' ELSE 'NO' END FROM Log_RPP WHERE LogDate > @LogDate
   ) AS 'RPP Sales'
   */
   
   /*
   --9-TacForeclosure 
   ---------------------------------------------------------------
   SELECT(
	SELECT CASE WHEN COUNT(*) > 0 THEN 'YES' ELSE 'NO' END FROM Log_TacForeclosure  WITH(NOLOCK) WHERE LogDate > @LogDate
   ) AS 'TAC-Foreclosure'
   */
   
   --10-VFASAP 
   ---------------------------------------------------------------
   SELECT(
	SELECT CASE WHEN COUNT(*) > 0 THEN 'YES' ELSE 'NO' END FROM Log_VFASAP WITH(NOLOCK) WHERE LogDate > @LogDate
   ) AS 'Ventura FASAP'
   
   /*--11-VPriorityPosting 
   ---------------------------------------------------------------
   SELECT(
	SELECT CASE WHEN COUNT(*) > 0 THEN 'YES' ELSE 'NO' END FROM Log_VPriorityPosting WHERE LogDate > @LogDate
   ) AS 'Ventura Priority Posting'
   
   --12-VRPP 
   ---------------------------------------------------------------
   SELECT(
	SELECT CASE WHEN COUNT(*) > 0 THEN 'YES' ELSE 'NO' END FROM Log_VRPP WHERE LogDate > @LogDate
   ) AS 'Ventura RPP Sales'
   
   --13-VRecontrust 
   ---------------------------------------------------------------
   SELECT(
	SELECT CASE WHEN COUNT(*) > 0 THEN 'YES' ELSE 'NO' END FROM Log_MVRecontrust WHERE LogDate > @LogDate
   ) AS 'Ventura Recontrust'
   */
   --14-Zillow 
   ---------------------------------------------------------------
   SELECT(
	SELECT CASE WHEN COUNT(*) > 0 THEN 'YES' ELSE 'NO' END FROM Log_Zillow WITH(NOLOCK)  WHERE LogDate > @LogDate
   ) AS 'Zillow'
   
--15-Gmail 
   ---------------------------------------------------------------
   SELECT(
	SELECT CASE WHEN COUNT(*) > 0 THEN 'YES' ELSE 'NO' END FROM Log_Gmail WHERE LogDate > @LogDate
   ) AS 'Gmail'
   /*
   --16-PriorityPosting 
   ---------------------------------------------------------------
   SELECT(
	SELECT CASE WHEN COUNT(*) > 0 THEN 'YES' ELSE 'NO' END FROM Log_PriorityPosting WHERE LogDate > @LogDate
   ) AS 'Priority Posting'
   */
   --17-VAuction 
   ---------------------------------------------------------------
   SELECT(
	SELECT CASE WHEN COUNT(*) > 0 THEN 'YES' ELSE 'NO' END FROM Log_VAuction WITH(NOLOCK)   WHERE LogDate > @LogDate
   ) AS 'Ventura Auction'
 
 --18-V NationWide Posting 
   ---------------------------------------------------------------
   SELECT(
	SELECT CASE WHEN COUNT(*) > 0 THEN 'YES' ELSE 'NO' END FROM Log_VNationwidePosting WITH(NOLOCK)   WHERE LogDate > @LogDate
   ) AS 'Ventura NationWide Posting'

    ";
            return strRetVal;

        }

        #endregion






    }



}
