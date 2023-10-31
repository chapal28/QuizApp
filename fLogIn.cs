using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;



namespace QuizApp
{
    public partial class fLogIn : Form
    {
        string myServer = string.Empty;
        string appMyServer = string.Empty;

        public fLogIn()
        {
            InitializeComponent();
           
        }

        private void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtLoginID.Text == "" || txtPassword.Text == "")
                {
                    lblError.Text = "Please Enter LoginID and Password...";
                }
                else
                {
                         List<User> users=new UserDA().Get();
                        User oUser = users.FirstOrDefault(o => o.UserName == txtLoginID.Text);
                        //var oUser= UserService.Get(txtLoginID.Text, true);

                        if (oUser == null)
                        {
                            lblError.Text = "Invalid LoginID";
                        }
                        else if (oUser.UserPassword != txtPassword.Text)
                        {
                            lblError.Text = "Invalid Password";
                        }
                        else
                        {
                            frmMain oFMainForm = new frmMain();
                                //FMainForm oFMainForm = new FMainForm();
                               // oFMainForm.lblUser.Text = txtLoginID.Text;
                                User.CurrentUser = oUser;
                               // this.Close();
                                oFMainForm.ShowDialog();
                                
                               //this.Hide();
                                //DialogResult = DialogResult.OK;
                                //this.Close();

                          
                        }
                    }
                }
            
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void btnCLose_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void fLogIn_Load(object sender, EventArgs e)
        {
            txtLoginID.Focus();
        }

    }
}
