using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using Matrix;

namespace QuizApp
{
    public class UserDA
    {
        private string sQuery = "";
        public User MapUser(DataRow dr)
        {
            User oUser = new User();
            oUser.UserID = Convert.ToInt32(dr["UserID"].ToString());
            oUser.UserName = dr["UserName"].ToString();
            oUser.UserPassword = dr["UserPassword"].ToString();
            oUser.UserType = Convert.ToInt32(dr["UserType"].ToString());
            return oUser;
        }
        public List<User> Users(DataTable dt)
        {
            List<User> items = new List<User>();
            foreach (DataRow dr in dt.Rows)
            {
                items.Add(MapUser(dr));
            }
            return items;
        }
        public void Save(User oItem)
        {
            Transaction tc = new Transaction();
            try
            {
                tc.Start();
                if (oItem.UserID == 0)
                {
                    oItem.UserID = tc.GenerateID("Users", "UserID");
                    sQuery = SQLGenerator.GenerateQuery("Insert Into Users(UserID,UserName,UserPassword,UserType) values(%n,%s,%s,%n)", oItem.UserID, oItem.UserName, oItem.UserPassword, oItem.UserType);
                }
                else
                {
                    //sQuery = SQLGenerator.GenerateQuery("UPDATE Users SET Score=%n,NoofCurrect=%n,NoofWrong=%n,CandidateName=%s,SetID=%n,IsAwarded=%b,Email=%s,MobileNo=%s,Institution=%s,QuizDate=%d WHERE UserID=%n", oItem.Score, oItem.NoofCurrect, oItem.NoofWrong, oItem.CandidateName, oItem.SetID,
                    //     oItem.IsAwarded, oItem.Email, oItem.MobileNo, oItem.Institution,oItem.QuizDate, oItem.UserID);
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
        public List<User> Get()
        {
            List<User> oUsers = new List<User>();
            Transaction tc = new Transaction();
            tc.Start();
            DataTable dt = tc.ExecuteReader("Select * from Users");
            if (dt != null)
                oUsers = Users(dt);
            tc.Finish();
            return oUsers;
        }
        public List<User> Get(long nID)
        {
            List<User> oUsers = new List<User>();
            Transaction tc = new Transaction();
            tc.Start();
            DataTable dt = tc.ExecuteReader(SQLGenerator.GenerateQuery("Select * from Users where UserID=%n", nID));
            if (dt != null)
                oUsers = Users(dt);
            tc.Finish();
            return oUsers;
        }

        public List<User> Get(string userName)
        {
            List<User> oUsers = new List<User>();
            Transaction tc = new Transaction();
            tc.Start();
            DataTable dt = tc.ExecuteReader(SQLGenerator.GenerateQuery("Select * from Users where userName=%s", userName));
            if (dt != null)
                oUsers = Users(dt);
            tc.Finish();
            return oUsers;
        }
     
        public void DeleteUsers(User fm)
        {
            Transaction tc = new Transaction();
            try
            {
                tc.Start();
                sQuery = SQLGenerator.GenerateQuery("DELETE  from Users WHERE UserID=%n", fm.UserID);
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
            sQuery = SQLGenerator.GenerateQuery("Select COUNT(*) from Users WHERE Description=%s", CandidateName);
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
            string query = SQLGenerator.GenerateQuery("Select COUNT(*) from Users WHERE MobileNo=%s OR Email=%s", mobileno,email);
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
