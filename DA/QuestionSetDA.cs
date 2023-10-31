using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Matrix;

namespace QuizApp
{
    public class QuestionSetDA
    {
        private string sQuery = "";
        public QuestionSet MapQuestionSet(DataRow dr)
        {
            QuestionSet oSet = new QuestionSet();
            oSet.SetID = Convert.ToInt32(dr["SetID"].ToString());
            oSet.Name = dr["Name"].ToString();
            oSet.Category = (EnumCategory)Convert.ToInt16(dr["Category"].ToString());
            return oSet;
        }
        public List<QuestionSet> QuestionSets(DataTable dt)
        {
            List<QuestionSet> items = new List<QuestionSet>();
            foreach (DataRow dr in dt.Rows)
            {
                items.Add(MapQuestionSet(dr));
            }
            return items;
        }
        public void Save(QuestionSet oItem)
        {
            Transaction tc = new Transaction();
            try
            {
                tc.Start();
                if (oItem.SetID == 0)
                {
                    oItem.SetID = tc.GenerateID("QuestionSets", "SetID");
                    sQuery = SQLGenerator.GenerateQuery("Insert Into QuestionSets(SetID,Name,Category) values(%n,%s,%n)", oItem.SetID, oItem.Name, (int)oItem.Category);
                }
                else
                {
                    sQuery = SQLGenerator.GenerateQuery("UPDATE QuestionSets SET Name=%s,Category=%n WHERE SetID=%n", oItem.Name, oItem.Category, oItem.SetID);
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

        public void SaveforUpload(List<QuestionSet> sets,List<Question> questions,List<Answer> answers)
        {
            Transaction tc = new Transaction();
            try
            {
                tc.Start();
                sQuery = SQLGenerator.GenerateQuery("DELETE  from Answers");
                tc.ExecuteNonQuery(sQuery);
                sQuery = SQLGenerator.GenerateQuery("DELETE  from Questions");
                tc.ExecuteNonQuery(sQuery);
                sQuery = SQLGenerator.GenerateQuery("DELETE  from QuestionSets");
                tc.ExecuteNonQuery(sQuery);

                foreach (QuestionSet item in sets)
                {
                    sQuery = SQLGenerator.GenerateQuery("Insert Into QuestionSets(SetID,Name,Category) values(%n,%s,%n)", item.SetID, item.Name, (int)item.Category);

                    tc.ExecuteNonQuery(sQuery);
                }

                foreach (Question item in questions)
                {

                    sQuery = SQLGenerator.GenerateQuery("Insert Into Questions(QuestionID,Description,SetID) values(%n,%s,%n)", item.QuestionID, item.Description, item.SetID);
                    tc.ExecuteNonQuery(sQuery);
                }
                foreach (Answer item in answers)
                {

                    sQuery = SQLGenerator.GenerateQuery("Insert Into Answers(AnswerID,AnswerNo,Description,SetID,QuestionID,IsCorrect) values(%n,%s,%s,%n,%n,%b)", item.AnswerID,item.AnswerNo, item.Description, item.SetID,item.QuestionID,item.IsCorrect);
                    tc.ExecuteNonQuery(sQuery);
                }
                tc.Finish();
            }
            catch (Exception exp)
            {
                tc.Finish();
                throw new Exception(exp.Message);
            }
        }
        public List<QuestionSet> Get()
        {
            List<QuestionSet> oQuestionSets = new List<QuestionSet>();
            Transaction tc = new Transaction();
            tc.Start();
            DataTable dt = tc.ExecuteReader("Select * from QuestionSets");
            if (dt != null)
                oQuestionSets = QuestionSets(dt);
            tc.Finish();
            return oQuestionSets;
        }
        public List<QuestionSet> Get(long nID)
        {
            List<QuestionSet> oQuestionSets = new List<QuestionSet>();
            Transaction tc = new Transaction();
            tc.Start();
            DataTable dt = tc.ExecuteReader(SQLGenerator.GenerateQuery("Select * from QuestionSets where SetID=%n", nID));
            if (dt != null)
                oQuestionSets = QuestionSets(dt);
            tc.Finish();
            return oQuestionSets;
        }

        public List<QuestionSet> Get(EnumCategory category)
        {
            List<QuestionSet> oQuestionSets = new List<QuestionSet>();
            Transaction tc = new Transaction();
            tc.Start();
            DataTable dt = tc.ExecuteReader(SQLGenerator.GenerateQuery("Select * from QuestionSets where category=%n", (int)category));
            if (dt != null)
                oQuestionSets = QuestionSets(dt);
            tc.Finish();
            return oQuestionSets;
        }
        public void DeleteQuestionSets(QuestionSet fm)
        {
            Transaction tc = new Transaction();
            try
            {
                tc.Start();
                sQuery = SQLGenerator.GenerateQuery("DELETE  from QuestionSets WHERE SetID=%n", fm.SetID);
                tc.ExecuteNonQuery(sQuery);
                tc.Finish();
            }
            catch (Exception exp)
            {
                tc.Finish();
                throw new Exception(exp.Message);
            }
        }
        public bool IsExists(string sName)
        {
            bool sts = false;
            Transaction tc = new Transaction();
            tc.Start();
            sQuery = SQLGenerator.GenerateQuery("Select COUNT(*) from QuestionSets WHERE Name=%s", sName);
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
