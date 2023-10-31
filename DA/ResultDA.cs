using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Matrix;

namespace QuizApp
{
    public class ResultDA
    {
        private string sQuery = "";
        public Result MapResult(DataRow dr)
        {
            Result oResult = new Result();
            oResult.ResultID = Convert.ToInt32(dr["ResultID"].ToString());
            oResult.Score = Convert.ToInt32(dr["Score"].ToString());
            oResult.NoofCurrect = Convert.ToInt32(dr["NoofCurrect"].ToString());
            oResult.NoofWrong = Convert.ToInt32(dr["NoofWrong"].ToString());
            oResult.CandidateName = dr["CandidateName"].ToString();
            oResult.SetID = Convert.ToInt32(dr["SetID"].ToString());
            oResult.IsAwarded = dr["IsAwarded"].ToString() == "1" ? true : false;
            oResult.Email = dr["Email"].ToString();
            oResult.MobileNo = dr["MobileNo"].ToString();
            oResult.Institution = dr["Institution"].ToString();
            
            return oResult;
        }
        public List<Result> Results(DataTable dt)
        {
            List<Result> items = new List<Result>();
            foreach (DataRow dr in dt.Rows)
            {
                items.Add(MapResult(dr));
            }
            return items;
        }
        public void Save(Result oItem)
        {
            Transaction tc = new Transaction();
            try
            {
                tc.Start();
                if (oItem.ResultID == 0)
                {
                    oItem.ResultID = tc.GenerateID("Results", "ResultID");
                    sQuery = SQLGenerator.GenerateQuery("Insert Into Results(ResultID,Score,NoofCurrect,NoofWrong,CandidateName,SetID,IsAwarded,Email,MobileNo,Institution,QuizDate) values(%n,%n,%n,%n,%s,%n,%b,%s,%s,%s,%d)", oItem.ResultID, oItem.Score, oItem.NoofCurrect, oItem.NoofWrong, oItem.CandidateName, oItem.SetID,
                         oItem.IsAwarded, oItem.Email, oItem.MobileNo, oItem.Institution,oItem.QuizDate);
                }
                else
                {
                    sQuery = SQLGenerator.GenerateQuery("UPDATE Results SET Score=%n,NoofCurrect=%n,NoofWrong=%n,CandidateName=%s,SetID=%n,IsAwarded=%b,Email=%s,MobileNo=%s,Institution=%s,QuizDate=%d WHERE ResultID=%n", oItem.Score, oItem.NoofCurrect, oItem.NoofWrong, oItem.CandidateName, oItem.SetID,
                         oItem.IsAwarded, oItem.Email, oItem.MobileNo, oItem.Institution,oItem.QuizDate, oItem.ResultID);
                }
                tc.ExecuteNonQuery(sQuery);
                tc.Finish();
            }
            catch (Exception exp)
            {
                tc.Finish();
                throw new Exception(exp.Message);
            }
        }
        public List<Result> Get()
        {
            List<Result> oResults = new List<Result>();
            Transaction tc = new Transaction();
            tc.Start();
            DataTable dt = tc.ExecuteReader("Select * from Results");
            if (dt != null)
                oResults = Results(dt);
            tc.Finish();
            return oResults;
        }
        public List<Result> Get(long nID)
        {
            List<Result> oResults = new List<Result>();
            Transaction tc = new Transaction();
            tc.Start();
            DataTable dt = tc.ExecuteReader(SQLGenerator.GenerateQuery("Select * from Results where ResultID=%n", nID));
            if (dt != null)
                oResults = Results(dt);
            tc.Finish();
            return oResults;
        }

        public List<Result> GetBySetID(long SetID)
        {
            List<Result> oResults = new List<Result>();
            Transaction tc = new Transaction();
            tc.Start();
            DataTable dt = tc.ExecuteReader(SQLGenerator.GenerateQuery("Select * from Results where SetID=%n", SetID));
            if (dt != null)
                oResults = Results(dt);
            tc.Finish();
            return oResults;
        }

        public List<Result> GetByQuestion(int questionID)
        {
            List<Result> oResults = new List<Result>();
            Transaction tc = new Transaction();
            tc.Start();
            DataTable dt = tc.ExecuteReader(SQLGenerator.GenerateQuery("Select * from Results where questionID=%n", questionID));
            if (dt != null)
                oResults = Results(dt);
            tc.Finish();
            return oResults;
        }
        public void DeleteResults(Result fm)
        {
            Transaction tc = new Transaction();
            try
            {
                tc.Start();
                sQuery = SQLGenerator.GenerateQuery("DELETE  from Results WHERE ResultID=%n", fm.ResultID);
                tc.ExecuteNonQuery(sQuery);
                tc.Finish();
            }
            catch (Exception exp)
            {
                tc.Finish();
                throw new Exception(exp.Message);
            }
        }
        public bool IsExists(string CandidateName)
        {
            bool sts = false;
            Transaction tc = new Transaction();
            tc.Start();
            sQuery = SQLGenerator.GenerateQuery("Select COUNT(*) from Results WHERE Description=%s", CandidateName);
            DataTable dt = tc.ExecuteReader(sQuery);
            tc.Finish();
            foreach (DataRow dr in dt.Rows)
            {
                if (Convert.ToInt16(dr[0]) > 0)
                    sts = true;
            }
            return sts;
        }

        internal static bool IsDuplicate(string mobileno, string email)
        {
            bool sts = false;
            Transaction tc = new Transaction();
            tc.Start();
            string query = string.Empty;
            if (email != string.Empty)
            {
                query = SQLGenerator.GenerateQuery("Select COUNT(*) from Results WHERE QuizDate<%d AND MobileNo=%s OR Email=%s",DateTime.Today.AddMonths(-1), mobileno, email);

            }
            else
            {
                query = SQLGenerator.GenerateQuery("Select COUNT(*) from Results WHERE QuizDate<%d AND MobileNo=%s", DateTime.Today.AddMonths(-1), mobileno);
            }
                DataTable dt = tc.ExecuteReader(query);
            tc.Finish();
            foreach (DataRow dr in dt.Rows)
            {
                if (Convert.ToInt16(dr[0]) > 0)
                    sts = true;
            }
            return sts;
        }
    }
}
