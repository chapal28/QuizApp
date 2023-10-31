using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.Reporting.WinForms;

namespace QuizApp
{
    public partial class fReportViewer : Form
    {
        public fReportViewer()
        {
            InitializeComponent();
        }

        private void fReportViewer_Load(object sender, EventArgs e)
        {

            this.rptViewer.RefreshReport();
        }
        List<ReportParameter> _parameters = null;       
        ReportParameter rParam = null;
        string _PATH = string.Empty;

        public ReportViewer ReportViewer
        {
            get { return rptViewer; }
        }
        private List<ReportParameter> reportParameters;
        public List<ReportParameter> ReportParams
        {
            get { return reportParameters; }
            set { reportParameters = value; }
        }

        public void CommonReportView(DataSet dSet, string reportName, List<ReportParameter> parameters,bool paramNeeded)
        {
            //DataSet dSet = new DataSet();

            ReportViewer.Refresh();
            ReportViewer.LocalReport.DataSources.Clear();
            ReportViewer.LocalReport.EnableExternalImages = true;           
            try
            {
                if (dSet != null && dSet.Tables.Count > 0)
                {
                    foreach (DataTable table in dSet.Tables)
                    {
                        ReportViewer.LocalReport.DataSources.Add(new ReportDataSource(table.TableName, table));
                    }
                    ReportViewer.LocalReport.ReportEmbeddedResource = reportName;
                }
                GetParameters();

                if (parameters != null)
                {
                    foreach (ReportParameter parItem in parameters)
                    {
                        _parameters.Add(parItem);
                    }
                }

                this.ReportParams = _parameters;
                if (paramNeeded)
                {
                    if (reportParameters != null)
                        ReportViewer.LocalReport.SetParameters(reportParameters);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            this.ShowDialog();
        }

        public void CommonReportView2(DataSet dSet, string reportName, List<ReportParameter> parameters)
        {
            //DataSet dSet = new DataSet();

            ReportViewer.Refresh();
            ReportViewer.LocalReport.DataSources.Clear();
            ReportViewer.LocalReport.EnableExternalImages = true;
            try
            {
                if (dSet != null && dSet.Tables.Count > 0)
                {
                    foreach (DataTable table in dSet.Tables)
                    {
                        ReportViewer.LocalReport.DataSources.Add(new ReportDataSource(table.TableName, table));
                    }
                    ReportViewer.LocalReport.ReportEmbeddedResource = reportName;
                }
                //GetParameters();

                if (parameters != null)
                {
                    _parameters = new List<ReportParameter>();
                    foreach (ReportParameter parItem in parameters)
                    {
                        _parameters.Add(parItem);
                    }
                }

                this.ReportParams = _parameters;

                if (reportParameters != null)
                    ReportViewer.LocalReport.SetParameters(reportParameters);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            this.ShowDialog();
        }
        public void CommonReportViewWithoutParam(DataSet dSet, string reportName, List<ReportParameter> parameters)
        {
            //DataSet dSet = new DataSet();

            ReportViewer.Refresh();
            ReportViewer.LocalReport.DataSources.Clear();
            ReportViewer.LocalReport.EnableExternalImages = true;
            try
            {
                if (dSet != null && dSet.Tables.Count > 0)
                {
                    foreach (DataTable table in dSet.Tables)
                    {
                        ReportViewer.LocalReport.DataSources.Add(new ReportDataSource(table.TableName, table));
                    }
                    ReportViewer.LocalReport.ReportEmbeddedResource = reportName;
                }                

                //if (parameters != null)
                //{
                //    foreach (ReportParameter parItem in parameters)
                //    {
                //        _parameters.Add(parItem);
                //    }
                //}

                this.ReportParams = parameters;

                if (reportParameters != null)
                    ReportViewer.LocalReport.SetParameters(reportParameters);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex);
            }
            this.ShowDialog();
        }
        public void GetParameters()
        {
            _parameters = new List<ReportParameter>();

            _PATH = Application.StartupPath + @"\sig.png";
            rParam = new ReportParameter("Logo", _PATH);
            _parameters.Add(rParam);           

            string address = "Kha-65/1, South Badda, Gulshan, Dhaka-1212";
            rParam = new ReportParameter("Address", address);
            _parameters.Add(rParam);

           

        }
    }
}
