using DNH_BL;
using DNH_COMMON;
using DNH_PL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DNH_PC_APP
{
    public partial class frmPicklistView : Form
    {
        #region Variable
        private BL_PICKLIST _blObj = null;
        private PL_PICKLIST _plObj = null;
        private DataTable dtPickilistData = null;
        #endregion



        #region Constructor
        public frmPicklistView(string piclistNo)
        {
            InitializeComponent();
            lblPicklistNo.Text = "********************";
            lblPicklistNo.Text =  piclistNo;
            BindDataView();
        }
        #endregion

        #region Method
        private void BindDataView()
        {
            try
            {
                _plObj = new PL_PICKLIST();
                _blObj = new BL_PICKLIST();
                _plObj.DbType = "BIND_PICKLIST_VIEW";
                _plObj.Picklist_No = lblPicklistNo.Text.Trim() ;
                 DataTable dtPiclist = null;
                dtPiclist = _blObj.BL_ExecuteTask(_plObj);
                dgv.DataSource = null;
                if (dtPiclist.Rows.Count > 0)
                {
                    dgv.DataSource = dtPiclist;
                }

            }
            catch (Exception ex)
            {
                GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, ex.Message, 3); ;
            }
        }
    
      
       
        #endregion
        #region Event
        private void frmDeviceUpload_Load(object sender, EventArgs e)
        {
            //this.MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea;
            this.FormBorderStyle = FormBorderStyle.None;
            //this.WindowState = FormWindowState.Maximized;
           
        }

       

        private void btnReset_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        

        #endregion


    }

}
