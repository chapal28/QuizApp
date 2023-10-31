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
    public partial class frmResult : Form
    {

       public frmResult()
        {
            InitializeComponent();
        }

       public void ShowDlg(Result oResult)
       {
           lblName.Text = oResult.CandidateName;
           lblInstitution.Text = oResult.Institution;
           lblMobile.Text = oResult.MobileNo;
           lblEmail.Text = oResult.Email;
           lblCorrect.Text = oResult.NoofCurrect.ToString();
           lblIncorrect.Text = oResult.NoofWrong.ToString();
           lblTotal.Text = oResult.Score.ToString();
           ShowDialog();
       }
    }
}
