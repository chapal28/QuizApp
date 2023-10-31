using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Media;
using System.Text;
using System.Windows.Forms;
using System.Configuration;


namespace QuizApp
{
    public partial class frmQuiz : Form
    {
       List<QuestionSet> _oQuestionSets = null;
       QuestionSetDA _QuestionSetDA = new QuestionSetDA();
       List<Question> _Questions = null;
       List<Answer> _Answers = null;
       List<Answer> _TempAnswers = null;
       int _Qindex = 0;
       int timeLeft =Convert.ToInt32(ConfigurationSettings.AppSettings["time"]);

        public frmQuiz()
        {
            InitializeComponent();
        }

        private void UnCheckAll()
        {
            rdoA.Checked = false;
            rdoB.Checked = false;
            rdoC.Checked = false;
            rdoD.Checked = false;

        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            if (timeLeft > 0)
            {
                timeLeft = timeLeft - 1;
                // Display time remaining as mm:ss
                var timespan = TimeSpan.FromSeconds(timeLeft);
                txtTimer.Text = string.Format("{0:mm:ss}", timespan); //timespan.ToString(@"mm\:ss");
                // Alternate method
                //int secondsLeft = timeLeft % 60;
                //int minutesLeft = timeLeft / 60;
            }
            else
            {
                timer1.Stop();
                SystemSounds.Exclamation.Play();
                btnStart.Enabled = false;
                btnNext.Enabled = false;
                btnBack.Enabled = false;
                panel2.Enabled = false;
                MessageBox.Show("Time's up!", "Time has elapsed", MessageBoxButtons.OK);
            }
        }
        private void btnStart_Click(object sender, EventArgs e)
        {
            try
            {
                if (Validation())
                {
                    UnCheckAll();
                    btnBack.Enabled = false;
                    _oQuestionSets = new List<QuestionSet>();
                    _oQuestionSets = _QuestionSetDA.Get(rdoUniversity.Checked ? EnumCategory.University : EnumCategory.School);
                    Random rdm = new Random();
                    int count = _oQuestionSets.Count - 1;
                    int set = rdm.Next(count);
                    //lblSet.Text = _oQuestionSets[set].Name;
                    QuestionDA questionDA = new QuestionDA();

                    _Questions = questionDA.GetBySetID(_oQuestionSets[set].SetID);
                    _Answers = new AnswerDA().GetBySetID(_oQuestionSets[set].SetID);
                    if (_Questions != null && _Questions.Count > 0)
                    {
                        lblQuestion.Text = _Questions[0].Description;


                        if (_Answers != null && _Answers.Count > 0)
                        {
                            _TempAnswers = _Answers.Where(x => x.QuestionID == _Questions[0].QuestionID).OrderBy(z => z.AnswerNo).ToList();
                            if (_TempAnswers != null && _TempAnswers.Count > 0)
                            {
                                lblA.Text = _TempAnswers[0] != null ? _TempAnswers[0].Description : "";
                                lblB.Text = _TempAnswers[1] != null ? _TempAnswers[1].Description : "";
                                lblC.Text = _TempAnswers[2] != null ? _TempAnswers[2].Description : "";
                                lblD.Text = _TempAnswers[3] != null ? _TempAnswers[3].Description : "";
                            }
                        }
                        if (_Qindex == _Questions.Count - 1)
                        {
                            btnNext.Enabled = false;
                            // btnFinish.Visible = true;
                        }
                    }
                    timer1.Start();
                    btnStart.Enabled = false;
                }
               
            }
            catch (Exception)
            {
                
                throw;
            }
        }
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
        private bool Validation()
        {
            if (txtName.Text.Trim().Length < 3)
            {
               
                MessageBox.Show("Please input your name.");
                txtName.Focus();
                return false;
            }
            else if (txtEmail.Text.Trim().Length >0 && !IsValidEmail(txtEmail.Text.Trim()))
            {
                MessageBox.Show("Please input a valid email address.");
                txtEmail.Focus();
                return false;
            }
            else if (txtInstitution.Text.Trim().Length <2)
            {
                MessageBox.Show("Please input your Institution Name.");
                txtInstitution.Focus();
                return false;
            }
            else if (txtMobile.Text.Trim().Length < 11)
            {
                MessageBox.Show("Please input a valid mobile number.");
                txtMobile.Focus();
                return false;
            }
            
            else if (ResultDA.IsDuplicate(txtMobile.Text.Trim(), txtEmail.Text.Trim()))
            {
                MessageBox.Show("Sorry, you have already participated !!!");
                btnStart.Enabled = false;
                btnNext.Enabled = false;
                btnFinish.Enabled = false;
                return false;
            }
            else if (!(rdoUniversity.Checked||rdoSchool.Checked))
            {
                MessageBox.Show("Please select a education level.");
                
                btnNext.Enabled = false;
                btnFinish.Enabled = false;
                return false;
                }
            else
            {
                btnNext.Enabled = true;
                btnFinish.Enabled = true;
                return true;
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            try
            {
                
                btnBack.Enabled = true;
                _Questions[_Qindex].AnswerIndex = GetAnsIndex();
                _Questions[_Qindex].Mark = GetMark(_Questions[_Qindex].AnswerIndex);

                UnCheckAll();
                
                _Qindex += 1;
                CheckRdo(_Questions[_Qindex].AnswerIndex);
                if (_Questions != null && _Questions.Count > 0)
                {
                    lblQuestion.Text = _Questions[_Qindex].Description;
                    
                    if (_Answers != null && _Answers.Count > 0)
                    {
                        _TempAnswers = _Answers.Where(x => x.QuestionID == _Questions[_Qindex].QuestionID).OrderBy(z => z.AnswerNo).ToList();
                        if (_TempAnswers != null && _TempAnswers.Count > 0)
                        {
                            lblA.Text = _TempAnswers[0] != null ? _TempAnswers[0].Description : "";
                            lblB.Text = _TempAnswers[1] != null ? _TempAnswers[1].Description : "";
                            lblC.Text = _TempAnswers[2] != null ? _TempAnswers[2].Description : "";
                            lblD.Text = _TempAnswers[3] != null ? _TempAnswers[3].Description : "";
                        }


                    }
                    if (_Qindex == _Questions.Count - 1)
                    {
                        btnNext.Enabled = false;
                       // btnFinish.Visible = true;
                    }
                }

            }
            catch (Exception)
            {
                
                throw;
            }
        }

        private int GetMark(int p)
        {
            Answer ans = null;
            if (p == 0)
            {
                return 0;
            }
           else if (p == 1 )
            {
                //ans = _TempAnswers.FirstOrDefault(x => x.AnswerNo.ToLower() == "a");
                ans = _TempAnswers.FirstOrDefault();
                return (ans != null && ans.IsCorrect) ? 1 : 0;
            }
            else if (p == 2)
            {
                //ans = _TempAnswers.FirstOrDefault(x => x.AnswerNo.ToLower() == "b");
                ans = _TempAnswers.Skip(1).FirstOrDefault();
                return (ans != null && ans.IsCorrect) ? 1 : 0;
            }
            else if (p == 3)
            {
               // ans = _TempAnswers.FirstOrDefault(x => x.AnswerNo.ToLower() == "c");
                ans = _TempAnswers.Skip(2).FirstOrDefault();
                return (ans != null && ans.IsCorrect) ? 1 : 0;
            }
            else if (p == 4)
            {
               // ans = _TempAnswers.FirstOrDefault(x => x.AnswerNo.ToLower() == "d");
                ans = _TempAnswers.Skip(3).FirstOrDefault();
                return (ans != null && ans.IsCorrect) ? 1 : 0;
            }
            else
            {
                return 0;
            }
        }

        private int GetAnsIndex()
        {
            if (rdoA.Checked)
            {
                return 1;
            }
            else if (rdoB.Checked)
            {
                return 2;
            }
            else if (rdoC.Checked)
            {
                return 3;
            }
            else if (rdoD.Checked)
            {
                return 4;
            }
            else
            {
                return 0;
            }
        }

        private void CheckRdo(int asnindex)
        {
            switch (asnindex)
            {
                case 1:
                    rdoA.Checked = true;
                    break;
                case 2:
                     rdoB.Checked = true;
                    break;
                case 3:
                    rdoC.Checked = true;
                    break;
                case 4:
                    rdoD.Checked = true;
                    break;
                
            }
        }
        private void frmQuiz_Load(object sender, EventArgs e)
        {

        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            try
            {
               // btnFinish.Visible = false;
                btnNext.Enabled = true;
                _Questions[_Qindex].AnswerIndex = GetAnsIndex();
                _Questions[_Qindex].Mark = GetMark(_Questions[_Qindex].AnswerIndex);
                UnCheckAll();
                
                _Qindex -= 1;
                CheckRdo(_Questions[_Qindex].AnswerIndex);
                if (_Questions != null && _Questions.Count > 0)
                {
                    lblQuestion.Text = _Questions[_Qindex].Description;
                   
                    if (_Answers != null && _Answers.Count > 0)
                    {
                        _TempAnswers = _Answers.Where(x => x.QuestionID == _Questions[_Qindex].QuestionID).ToList();
                        if (_TempAnswers != null && _TempAnswers.Count > 0)
                        {
                            lblA.Text = _TempAnswers[0] != null ? _TempAnswers[0].Description : "";
                            lblB.Text = _TempAnswers[1] != null ? _TempAnswers[1].Description : "";
                            lblC.Text = _TempAnswers[2] != null ? _TempAnswers[2].Description : "";
                            lblD.Text = _TempAnswers[3] != null ? _TempAnswers[3].Description : "";
                        }

                    }
                }
                if (_Qindex == 0)
                {
                    btnBack.Enabled = false;
                }
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        private void btnFinish_Click(object sender, EventArgs e)
        {
             timer1.Stop();
            btnFinish.Enabled = false;
            _Questions[_Qindex].AnswerIndex = GetAnsIndex();
            _Questions[_Qindex].Mark = GetMark(_Questions[_Qindex].AnswerIndex);
            int mark=0;
            int nofCurrect=0;
            int nofWrong=0;
            long setid=0;
            if(_Questions!=null && _Questions.Count>0)
            {
            mark=_Questions.Sum(x=>x.Mark);
                nofCurrect=_Questions.Where(x=>x.Mark==1).Count();
                nofWrong=_Questions.Where(x=>x.Mark==0).Count();
                setid=_Questions[0].SetID;
            }
            Result oResult = new Result();
            oResult.CandidateName = txtName.Text;
            oResult.Email = txtEmail.Text;
            oResult.MobileNo = txtMobile.Text;
            oResult.Institution = txtInstitution.Text;
            oResult.IsAwarded = false;
            oResult.NoofCurrect = nofCurrect;
            oResult.NoofWrong = nofWrong;
            oResult.Score = mark;
            oResult.SetID = setid;
            oResult.QuizDate = DateTime.Today;

            ResultDA rsl =new ResultDA();
            rsl.Save(oResult);

            new frmResult().ShowDlg(oResult);
            this.Close();
        }

        private void btn1_Click(object sender, EventArgs e)
        {
            try
            {
                int tempindex = 0;
                // btnFinish.Visible = false;
                
                    btnNext.Enabled = true;
                    _Questions[_Qindex].AnswerIndex = GetAnsIndex();
                    _Questions[_Qindex].Mark = GetMark(_Questions[_Qindex].AnswerIndex);
                   
                    tempindex = _Qindex;
                    _Qindex = Convert.ToInt32(((Button)sender).Text) - 1;
                    if ((_Questions.Count - 1) >= _Qindex)
                    {
                        UnCheckAll();
                        CheckRdo(_Questions[_Qindex].AnswerIndex);
                        if (_Questions != null && _Questions.Count > 0)
                        {
                            lblQuestion.Text = _Questions[_Qindex].Description;

                            if (_Answers != null && _Answers.Count > 0)
                            {
                                _TempAnswers = _Answers.Where(x => x.QuestionID == _Questions[_Qindex].QuestionID).ToList();
                                if (_TempAnswers != null && _TempAnswers.Count > 0)
                                {
                                    lblA.Text = _TempAnswers[0] != null ? _TempAnswers[0].Description : "";
                                    lblB.Text = _TempAnswers[1] != null ? _TempAnswers[1].Description : "";
                                    lblC.Text = _TempAnswers[2] != null ? _TempAnswers[2].Description : "";
                                    lblD.Text = _TempAnswers[3] != null ? _TempAnswers[3].Description : "";
                                }

                            }
                        }
                    }
                    else
                    {
                        _Qindex = tempindex;
                    }


                    if (_Qindex == 0)
                    {
                        btnBack.Enabled = false;
                    }
                    else
                    {
                        btnBack.Enabled = true;
                    }
                if (_Qindex >= _Questions.Count - 1)
                {
                    btnNext.Enabled = false;
                    // btnFinish.Visible = true;
                }

            }
            catch (Exception)
            {

                throw;
            }
        }

      
    }
}
