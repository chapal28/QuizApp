using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Matrix;

namespace QuizApp
{
    public class QuestionDA
    {
        private string sQuery = "";
        public Question MapQuestion(DataRow dr)
        {
            Question oSet = new Question();
            oSet.QuestionID = Convert.ToInt32(dr["QuestionID"].ToString());
            oSet.Description = dr["Description"].ToString();
            oSet.SetID = Convert.ToInt32(dr["SetID"].ToString());
           
            return oSet;
        }
        public List<Question> Questions(DataTable dt)
        {
            List<Question> items = new List<Question>();
            foreach (DataRow dr in dt.Rows)
            {
                items.Add(MapQuestion(dr));
            }
            return items;
        }
        public void Save(Question oItem)
        {
            Transaction tc = new Transaction();
            try
            {
                tc.Start();
                if (oItem.QuestionID == 0)
                {
                    oItem.QuestionID = tc.GenerateID("Questions", "QuestionID");
                    sQuery = SQLGenerator.GenerateQuery("Insert Into Questions(QuestionID,Description,SetID) values(%n,%s,%n)", oItem.QuestionID, oItem.Description, oItem.SetID);
                }
                else
                {
                    sQuery = SQLGenerator.GenerateQuery("UPDATE Questions SET Description=%s,SetID=%n WHERE QuestionID=%n", oItem.Description, oItem.SetID, oItem.QuestionID);
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
        public List<Question> Get()
        {
            List<Question> oQuestions = new List<Question>();
            Transaction tc = new Transaction();
            tc.Start();
            DataTable dt = tc.ExecuteReader("Select * from Questions");
            if (dt != null)
                oQuestions = Questions(dt);
            tc.Finish();
            return oQuestions;
        }
        public List<Question> Get(long nID)
        {
            List<Question> oQuestions = new List<Question>();
            Transaction tc = new Transaction();
            tc.Start();
            DataTable dt = tc.ExecuteReader(SQLGenerator.GenerateQuery("Select * from Questions where QuestionID=%n", nID));
            if (dt != null)
                oQuestions = Questions(dt);
            tc.Finish();
            return oQuestions;
        }

        public List<Question> GetBySetID(long SetID)
        {
            List<Question> oQuestions = new List<Question>();
            Transaction tc = new Transaction();
            tc.Start();
            DataTable dt = tc.ExecuteReader(SQLGenerator.GenerateQuery("Select * from Questions where SetID=%n", SetID));
            if (dt != null)
                oQuestions = Questions(dt);
            tc.Finish();
            return oQuestions;
        }
        public void DeleteQuestions(Question fm)
        {
            Transaction tc = new Transaction();
            try
            {
                tc.Start();
                sQuery = SQLGenerator.GenerateQuery("DELETE  from Questions WHERE QuestionID=%n", fm.QuestionID);
                tc.ExecuteNonQuery(sQuery);
                tc.Finish();
            }
            catch (Exception exp)
            {
                tc.Finish();
                throw new Exception(exp.Message);
            }
        }
        public bool IsExists(string sDescription)
        {
            bool sts = false;
            Transaction tc = new Transaction();
            tc.Start();
            sQuery = SQLGenerator.GenerateQuery("Select COUNT(*) from Questions WHERE Description=%s", sDescription);
            DataTable dt = tc.ExecuteReader(sQuery);
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
