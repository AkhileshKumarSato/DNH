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
    public partial class frmDeviceUpload : Form
    {
        #region Variable
        private BL_PICKING_UPLOAD_DOWNLOAD _blObj = null;
        private PL_PICKING_UPLOAD_DOWNLOAD _plObj = null;
        private DataTable dtPickilistData = null;
        #endregion



        #region Constructor
        public frmDeviceUpload()
        {
            InitializeComponent();

        }
        #endregion

        #region Method
        private void BindPicklistNoForPicking()
        {
            try
            {
                Common common = new Common();
                DataTable dtPiclist = null;
                dtPiclist = common.GetPicklistNoForPicking();
                cbPicklistNo.DataSource = null;
                if (dtPiclist.Rows.Count > 0)
                {
                    GlobalVariable.BindCombo(cbPicklistNo, dtPiclist);
                    cbPicklistNo.SelectedIndex = 0;
                }

            }
            catch (Exception ex)
            {
                GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, ex.Message, 3); ;
            }
        }
        private void BindPicklistNoForDispatch()
        {
            try
            {
                Common common = new Common();
                DataTable dtPiclist = null;
                dtPiclist = common.GetPicklistNoForDispatch();
                cbPicklistNo.DataSource = null;
                if (dtPiclist.Rows.Count > 0)
                {
                    GlobalVariable.BindCombo(cbPicklistNo, dtPiclist);
                    cbPicklistNo.SelectedIndex = 0;
                }

            }
            catch (Exception ex)
            {
                GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, ex.Message, 3); ;
            }
        }
        private void ExportPickingFile(DataTable dataTable)
        {
            try
            {
                string pickInputFilesPath = string.Empty;
                GlobalVariable.mInputFileName = cbPicklistNo.Text + ".TXT";
               
                if (!txtBrowseFilePath.Text.Trim().EndsWith("INPUT_FILES"))
                {
                    GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, "Invalid Selected File Path!!", 3);
                    return;
                }
                else
                {
                    pickInputFilesPath = txtBrowseFilePath.Text.Trim();
                }

                var pickingData = GlobalVariable.DataTableToCsv(dataTable);
                if (File.Exists(pickInputFilesPath + "\\" + GlobalVariable.mInputFileName))
                {
                    File.Delete(pickInputFilesPath + "\\" + GlobalVariable.mInputFileName);
                }
                using (StreamWriter streamWriter = new StreamWriter(pickInputFilesPath + "\\" + GlobalVariable.mInputFileName))
                {
                    streamWriter.WriteLine(pickingData.TrimEnd());
                }
                _plObj = new PL_PICKING_UPLOAD_DOWNLOAD();
                _plObj.DbType = "UPDATE_PICKING_STATUS";
                _plObj.PicklistNo = cbPicklistNo.Text.Trim();
                DataTable dtUpdateStatus = _blObj.BL_ExecuteTask(_plObj);
                GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, "Picking data export successfully !!", 1);

            }
            catch (Exception ex)
            {

                GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, ex.Message, 3);
            }
        }
        private void ExportDispatchFile(DataTable dataTable)
        {
            try
            {
                string dispatchInputFilesPath = string.Empty;
                GlobalVariable.mInputFileName = cbPicklistNo.Text + ".TXT";

                if (!txtBrowseFilePath.Text.Trim().EndsWith("INPUT_FILES"))
                {
                    GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, "Invalid Selected File Path!!", 3);
                    return;
                }
                else
                {
                    dispatchInputFilesPath = txtBrowseFilePath.Text.Trim();
                }

                var pickingData = GlobalVariable.DataTableToCsv(dataTable);
                if (File.Exists(dispatchInputFilesPath + "\\" + GlobalVariable.mInputFileName))
                {
                    File.Delete(dispatchInputFilesPath + "\\" + GlobalVariable.mInputFileName);
                }
                using (StreamWriter streamWriter = new StreamWriter(dispatchInputFilesPath + "\\" + GlobalVariable.mInputFileName))
                {
                    streamWriter.WriteLine(pickingData.TrimEnd());
                }
                _plObj = new PL_PICKING_UPLOAD_DOWNLOAD();
                _plObj.DbType = "UPDATE_DISPATCH_STATUS";
                _plObj.PicklistNo = cbPicklistNo.Text.Trim();
                DataTable dtUpdateStatus = _blObj.BL_ExecuteTask(_plObj);
                GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, "Dispatch data export successfully !!", 1);

            }
            catch (Exception ex)
            {

                GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, ex.Message, 3);
            }
        }
        #endregion
        #region Event
        private void frmDeviceUpload_Load(object sender, EventArgs e)
        {
            //this.MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea;
            this.FormBorderStyle = FormBorderStyle.None;
            //this.WindowState = FormWindowState.Maximized;
            rbPicking_CheckedChanged(null,null);
        }

        private void picBrowse_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                txtBrowseFilePath.Text = folderBrowserDialog.SelectedPath;
            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (cbPicklistNo.SelectedIndex <= 0)
                {
                    GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, "Picklist no can't be blank!!", 3);
                    cbPicklistNo.Focus();
                    return;

                }

                if (string.IsNullOrWhiteSpace(txtBrowseFilePath.Text.Trim()))
                {
                    GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, "Export file path can't be blank!!", 3);
                    txtBrowseFilePath.Focus();
                    return;


                }
                if (rbPicking.Checked)
                {
                    ExportPickingFile(dtPickilistData);
                }
                else
                {
                    ExportDispatchFile(dtPickilistData);

                }

            }
            catch (Exception ex)
            {

                GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, ex.Message, 3);
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            cbPicklistNo.DataSource = null;
            txtBrowseFilePath.Text = "";
            frmDeviceUpload_Load(null, null);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnFileFormat_Click(object sender, EventArgs e)
        {

        }
        private void btnMini_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        private void cbPicklistNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {

                if (cbPicklistNo.SelectedIndex > 0)
                {
                     dtPickilistData = new DataTable();
                    _plObj = new PL_PICKING_UPLOAD_DOWNLOAD();
                    _blObj = new BL_PICKING_UPLOAD_DOWNLOAD();
                    if (rbPicking.Checked)
                    {
                        _plObj.DbType = "PICKING_DATA";
                    }
                    else
                    {
                        _plObj.DbType = "DISPATCH_DATA";
                    }
                    _plObj.PicklistNo = cbPicklistNo.SelectedValue.ToString().Trim();
                    dtPickilistData = _blObj.BL_ExecuteTask(_plObj);
                    if (dtPickilistData.Rows.Count == 0)
                    {
                        GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, "No Data Found !!!", 1);
                    }
                }

            }
            catch (Exception ex)
            {

                GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, ex.Message, 3);
            }
        }
        private void rbPicking_CheckedChanged(object sender, EventArgs e)
        {
            if (rbPicking.Checked)
            {
                BindPicklistNoForPicking();
            }
            else
            {
                BindPicklistNoForDispatch();
            }
        }

        private void rbDispatch_CheckedChanged(object sender, EventArgs e)
        {
            if (rbDispatch.Checked)
            {
                BindPicklistNoForDispatch();
            }
            else
            {
                BindPicklistNoForPicking();
            }
        }

        #endregion


    }

}
