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
    public partial class frmQuestionSet : Form
    {
        QuestionSet _oQuestionSet = null;
        People _oPeople = null;
        QuestionSetDA _QuestionSetDA = new QuestionSetDA();
      
        public frmQuestionSet()
        {
            InitializeComponent();
        }
        public void ShowDlg(QuestionSet oQuestionSet)
        {
            _oQuestionSet = oQuestionSet;
            RefreshControls();
            this.ShowDialog();
        }
        
        private void RefreshControls()
        {
            txtName.Text = _oQuestionSet.Name;
            
            
        }
        private int GetComboIndex(ComboBox cbo, string value)
        {
            int index = -1;
            foreach (object item in cbo.Items)
            {
                index++;
                People p = (People)item;
                if (p.PeopleID.ToString() == value)
                    return index;
            }
            return -1;
        }
        private void RefreshObject()
        {
             _oQuestionSet.Name=txtName.Text ;             
//             _oQuestionSet.Category =Convert.ToInt16(cmbGuestName.SelectedValue);

        }
        private bool ValidateInputs()
        {
            bool isValid = true;
            //if (txtName.Text == "")
            //{
            //    MessageBox.Show("Name should not be blank.", "Duplicate", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            //    txtName.Focus();
            //    isValid = false;
            //}
            return isValid;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (!ValidateInputs())
                {
                    return;
                }
                if (_oQuestionSet.SetID == 0)
                {
                    if (txtName.Text.Trim() != "")
                    {
                        if (_QuestionSetDA.IsExists(txtName.Text.Trim()))
                        {
                            MessageBox.Show("Name already exists.", "Duplicate", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                            return;
                        }
                    }
                }
                RefreshObject();
                _QuestionSetDA.Save(_oQuestionSet);
                MessageBox.Show("Data Saved Successfully.", "Successfull", MessageBoxButtons.OK, MessageBoxIcon.Information);
                _oPeople = new People();
                RefreshControls();
                txtName.Focus();
            }
            catch (Exception exp)
            {
                MessageBox.Show("Failed to Save.Because: " + exp.Message, "Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
