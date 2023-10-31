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
    public partial class frmQuestionSets : Form
    {
        List<QuestionSet> _oQuestionSets = null;
        List<People> _oPeoples = null;
        QuestionSetDA _QuestionSetDA = new QuestionSetDA();
        public frmQuestionSets()
        {
            InitializeComponent();
        }
        private void frmQuestionSet_Load(object sender, EventArgs e)
        {
            //_oPeoples = new DAPeople().GetPeoples();
            RefreshList();
        }
        private void RefreshList()
        {
            _oQuestionSets = new List<QuestionSet>();
            _oQuestionSets = _QuestionSetDA.Get();
            Random rdm = new Random();
            ListViewItem li = null;
            lsvList.Items.Clear();
            //foreach (QuestionSet item in _oQuestionSets)
            //{
                
                li = new ListViewItem();
                int count = _oQuestionSets.Count - 1;
                li.Text = rdm.Next(count).ToString();
                //People oPeople = _oPeoples.Where(p => p.PeopleID == item.PeopleID).SingleOrDefault();
                //li.SubItems.Add(oPeople.PeopleName);
                //li.SubItems.Add(item.RentAmount.ToString("#,###"));
                //li.SubItems.Add(item.MeterNo);
                //li.Tag = item;
                lsvList.Items.Add(li);
            //}
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            frmQuestionSet ffm = new frmQuestionSet();
            ffm.ShowDlg(new QuestionSet());
            RefreshList();
        }

        private void btnEdit_Click(object sender, EventArgs e)
        {
            if (lsvList.SelectedItems.Count > 0)
            {
                frmQuestionSet ffm = new frmQuestionSet();
                QuestionSet fm = (QuestionSet)lsvList.SelectedItems[0].Tag;
                ffm.ShowDlg(fm);
                RefreshList();
            }
            else
                MessageBox.Show("Please select an item.", "Select", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (lsvList.SelectedItems.Count > 0)
            {
                if (MessageBox.Show("Do you want to delete?", "Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                {
                    _QuestionSetDA.DeleteQuestionSets((QuestionSet)lsvList.SelectedItems[0].Tag);
                    MessageBox.Show("Data Deleted Successfully.", "Deleted", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    RefreshList();
                }
            }
            else
                MessageBox.Show("Please select an item.", "Select", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
