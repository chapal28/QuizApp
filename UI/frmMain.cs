using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace QuizApp
{
    public partial class frmMain : Form
    {
        public frmMain()
        {
            InitializeComponent();
            lblDate.Text ="Date : " + System.DateTime.Now.ToString("dd MMM yyyy");
        }

        private void entryToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

       

        private void createQuestionSetToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmQuestionSets fQuestionSet = new frmQuestionSets();
            fQuestionSet.ShowDialog();
        }

        private void entryMonthlyBillToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //frmMonthlyBills fBill = new frmMonthlyBills();
            //fBill.ShowDialog();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

      

        private void frmMain_Load(object sender, EventArgs e)
        {
            if (User.CurrentUser.UserType == (int)EnumUserType.Admin)
            {
               uploadToolStripMenuItem.Visible = true;
             
            }
            else
            {
                uploadToolStripMenuItem.Visible = false;
            }
        }

        private void balanceSheetToolStripMenuItem_Click(object sender, EventArgs e)
        {

            
        }

        private void quizToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmQuiz fBill = new frmQuiz();
            fBill.ShowDialog();
        }

        private void logoutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            this.Hide();
            fLogIn frm = new fLogIn();
            frm.ShowDialog();
            frm.txtLoginID.Focus();
        }

        private void uploadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmUploader fBill = new frmUploader();
            fBill.ShowDialog();
        }
    }
}
