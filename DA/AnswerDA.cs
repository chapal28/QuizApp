using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Matrix;

namespace QuizApp
{
    public class AnswerDA
    {
        private string sQuery = "";
        public Answer MapAnswer(DataRow dr)
        {
            Answer oAnswer = new Answer();
            oAnswer.AnswerID = Convert.ToInt32(dr["AnswerID"].ToString());
            oAnswer.QuestionID = Convert.ToInt32(dr["QuestionID"].ToString());
            oAnswer.Description = dr["Description"].ToString();
            oAnswer.SetID = Convert.ToInt32(dr["SetID"].ToString());
            oAnswer.IsCorrect = dr["IsCorrect"].ToString() == "True" ? true : false; 
            oAnswer.AnswerNo = dr["AnswerNo"].ToString(); 
            return oAnswer;
        }
        public List<Answer> Answers(DataTable dt)
        {
            List<Answer> items = new List<Answer>();
            foreach (DataRow dr in dt.Rows)
            {
                items.Add(MapAnswer(dr));
            }
            return items;
        }
        public void Save(Answer oItem)
        {
            Transaction tc = new Transaction();
            try
            {
                tc.Start();
                if (oItem.AnswerID == 0)
                {
                    oItem.AnswerID = tc.GenerateID("Answers", "AnswerID");
                    sQuery = SQLGenerator.GenerateQuery("Insert Into Answers(AnswerID,Description,SetID) values(%n,%s,%n)", oItem.AnswerID, oItem.Description, oItem.SetID);
                }
                else
                {
                    sQuery = SQLGenerator.GenerateQuery("UPDATE Answers SET Description=%s,SetID=%n WHERE AnswerID=%n", oItem.Description, oItem.SetID, oItem.AnswerID);
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
        public List<Answer> Get()
        {
            List<Answer> oAnswers = new List<Answer>();
            Transaction tc = new Transaction();
            tc.Start();
            DataTable dt = tc.ExecuteReader("Select * from Answers");
            if (dt != null)
                oAnswers = Answers(dt);
            tc.Finish();
            return oAnswers;
        }
        public List<Answer> Get(long nID)
        {
            List<Answer> oAnswers = new List<Answer>();
            Transaction tc = new Transaction();
            tc.Start();
            DataTable dt = tc.ExecuteReader(SQLGenerator.GenerateQuery("Select * from Answers where AnswerID=%n", nID));
            if (dt != null)
                oAnswers = Answers(dt);
            tc.Finish();
            return oAnswers;
        }

        public List<Answer> GetBySetID(long SetID)
        {
            List<Answer> oAnswers = new List<Answer>();
            Transaction tc = new Transaction();
            tc.Start();
            DataTable dt = tc.ExecuteReader(SQLGenerator.GenerateQuery("Select * from Answers where SetID=%n", SetID));
            if (dt != null)
                oAnswers = Answers(dt);
            tc.Finish();
            return oAnswers;
        }

        public List<Answer> GetByQuestion(int questionID)
        {
            List<Answer> oAnswers = new List<Answer>();
            Transaction tc = new Transaction();
            tc.Start();
            DataTable dt = tc.ExecuteReader(SQLGenerator.GenerateQuery("Select * from Answers where questionID=%n", questionID));
            if (dt != null)
                oAnswers = Answers(dt);
            tc.Finish();
            return oAnswers;
        }
        public void DeleteAnswers(Answer fm)
        {
            Transaction tc = new Transaction();
            try
            {
                tc.Start();
                sQuery = SQLGenerator.GenerateQuery("DELETE  from Answers WHERE AnswerID=%n", fm.AnswerID);
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
            sQuery = SQLGenerator.GenerateQuery("Select COUNT(*) from Answers WHERE Description=%s", sDescription);
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
