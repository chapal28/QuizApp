using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
//using System.Threading.Tasks;
using System.Windows.Forms;

namespace QuizApp
{
    public partial class frmUploader : Form
    {
        string filePath = string.Empty;
        string fileName = string.Empty;
        DataSet _uploadData = null;
        private DataTable _uplodedData;
        List<string> SheetCollection = new List<string>();
        List<QuestionSet> _sets = new List<QuestionSet>();
        List<Question> _questions = new List<Question>();
        List<Answer> _answers = new List<Answer>();
        public frmUploader()
        {
            InitializeComponent();
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            openFileDialog.FileName = string.Empty;
            openFileDialog.Filter = "Excel files(*.xls, *.xlsx)|*.xls;*.xlsx;";
            openFileDialog.Multiselect = false;
            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                fileName = openFileDialog.SafeFileName;
                filePath = openFileDialog.FileName;
                txtFileName.Text = filePath;
            }
            else
                return;
        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            try
            {
                Cursor.Current = Cursors.WaitCursor;
                _uploadData = new DataSet();
                SheetCollection.Add("Sets");
                SheetCollection.Add("Questions");
                SheetCollection.Add("Answers");
                foreach (string oSt in SheetCollection)
                {
                    _uplodedData = LoadExcelData(txtFileName.Text, oSt);

                    switch (oSt)
                    {
                        case "Sets":

                            PrepareSets();
                            break;
                        case "Questions":

                            PrepareQuestions();
                            break;
                        case "Answers":

                            PrepareAnswers();
                            break;


                        default:
                            break;
                    }


                }
                QuestionSetDA qset = new QuestionSetDA();
                qset.SaveforUpload(_sets,_questions,_answers);
                Cursor.Current = Cursors.Default;
                MessageBox.Show("Data Upload Successfull...");
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
        }

        private void PrepareAnswers()
        {
           
            Answer ans = null;
            foreach (DataRow item in _uplodedData.Rows)
            {
                ans = new Answer();
                ans.AnswerID = Convert.ToInt32(item["AnswerID"].ToString());
                ans.QuestionID = Convert.ToInt32(item["QuestionID"].ToString());
                Question temp = _questions.FirstOrDefault(x => x.QuestionID == ans.QuestionID);
                ans.SetID = temp!=null?temp.SetID:0;
                ans.Description = item["Description"].ToString();
                ans.AnswerNo = item["AnswerNo"].ToString();
                ans.IsCorrect = item["IsCorrect"].ToString()=="1"?true:false;
                _answers.Add(ans);
            }
          
               
        }

        private void PrepareQuestions()
        {
            Question qsn = null;
            foreach (DataRow item in _uplodedData.Rows)
            {
                qsn = new Question();
                qsn.QuestionID = Convert.ToInt32(item["QuestionID"].ToString());
                qsn.SetID = Convert.ToInt32(item["SetID"].ToString());
                qsn.Description = item["Description"].ToString();
               
                _questions.Add(qsn);
            }
        }

        private void PrepareSets()
        {
            QuestionSet qset = null;
            foreach (DataRow item in _uplodedData.Rows)
            {
                qset = new QuestionSet();
                qset.SetID = Convert.ToInt32(item["SetID"].ToString());
                qset.Name = item["Name"].ToString();
                qset.Category = (EnumCategory)Enum.Parse(typeof(EnumCategory), item["Category"].ToString(), true);
                _sets.Add(qset);
            }

        }
        public DataTable LoadExcelData(string fileName, string sheetName)
        {
            string connectionString = string.Empty;

            if (fileName.EndsWith("xls"))
                //Connection string for office 2003
                connectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};Extended Properties=Excel 8.0;", fileName);
            else
                //Connection string for office 2007
                connectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=Excel 12.0 Xml;", fileName);

            DataTable dt = new DataTable(sheetName);

            try
            {
                OleDbConnection connection = new OleDbConnection(connectionString);
                connection.Open();
                DataTable excelTable = connection.GetSchema("Tables");
                string tableName;

                if (excelTable.Rows.Count > 0)
                {
                    tableName = Convert.ToString(excelTable.Rows[0]["TABLE_NAME"]);
                    OleDbDataAdapter dataAdapter = new OleDbDataAdapter(string.Format("SELECT * FROM [{0}]", sheetName + "$"), connection);
                    //dt.TableName = sheetName;
                    dataAdapter.Fill(dt);
                    dataAdapter = null;
                }

                connection.Close();
                connection = null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            return dt;


        }
    }
}
