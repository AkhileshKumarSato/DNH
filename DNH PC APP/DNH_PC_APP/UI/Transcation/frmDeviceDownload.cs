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
    public partial class frmDeviceDownload : Form
    {
        #region Variable
        private BL_PICKING_UPLOAD_DOWNLOAD _blObj = null;
        private PL_PICKING_UPLOAD_DOWNLOAD _plObj = null;
        #endregion

        #region Method
        private DataTable dataTableFileFormate(DataSet dataSet)
        {
            DataTable dataTable = new DataTable();
            try
            {
                dataTable.Columns.Add("NAMES");
                for (int iColumns = 0; iColumns < dataSet.Tables[0].Rows.Count; iColumns++)
                {
                    dataTable.Columns.Add(dataSet.Tables[0].Rows[iColumns][0].ToString());

                }
                for (int iRows = 0; iRows < dataSet.Tables[1].Rows.Count; iRows++)
                {
                    DataRow dataRow = dataTable.NewRow();
                    dataRow[0] = dataSet.Tables[1].Rows[iRows][0].ToString();
                    dataTable.Rows.Add(dataRow);
                    dataTable.AcceptChanges();
                }
            }
            catch (Exception)
            {

                throw;
            }
            return dataTable;
        }
        private void ConvertTXTtoDataTable(string strFilePath, string sPicklistNo, ref DataTable dataTable)
        {
            DataRow dr = null;
            using (StreamReader sr = new StreamReader(strFilePath))
            {
                while (!sr.EndOfStream)
                {
                    string[] rows = sr.ReadLine().Trim().Split(',');
                    if (rows[0].ToString().Trim() == "Barcode") { continue; }
                    dr = dataTable.NewRow();
                    dr[0] = sPicklistNo;
                    dr[1] = rows[0].Split('~')[0];
                    dr[2] = rows[0];
                    dr[3] = rows[1];
                    dr[4] = GlobalVariable.mSatoAppsLoginUser;
                    dr[5] = DateTime.Now.ToString();
                    dataTable.Rows.Add(dr);
                }

            }

        }

        private void SaveDownloadData()
        {
            try
            {
                DataTable dtFinal = new DataTable();
                if (rbPicking.Checked)
                {
                    dtFinal.Columns.Add("PicklistNo");
                    dtFinal.Columns.Add("PartNo");
                    dtFinal.Columns.Add("Barcode");
                    dtFinal.Columns.Add("Qty");
                    dtFinal.Columns.Add("PickedBy");
                    dtFinal.Columns.Add("PickedOn");
                }
                else
                {
                    dtFinal.Columns.Add("PicklistNo");
                    dtFinal.Columns.Add("PartNo");
                    dtFinal.Columns.Add("Barcode");
                    dtFinal.Columns.Add("Qty");
                    dtFinal.Columns.Add("DispatchBy");
                    dtFinal.Columns.Add("DispatchOn");
                }
                if (string.IsNullOrWhiteSpace(txtBrowseFilePath.Text.Trim()))
                {
                    GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, "File Path can't be blank!!", 3);
                    txtBrowseFilePath.Focus();
                    txtBrowseFilePath.SelectAll();
                    return;
                }
                DirectoryInfo arrPiclistNo = new DirectoryInfo(txtBrowseFilePath.Text.Trim());
                FileInfo[] Files = arrPiclistNo.GetFiles("*.TXT"); //Getting Text files
                if (Files.Length == 0)
                {
                    GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, "File Not Exist!!!", 3);
                    return;
                }
                foreach (FileInfo file in Files)
                {
                    if (!file.Name.ToString().StartsWith("PKL")) { continue; }
                    if (rbDispatch.Checked)
                    {
                        if (!txtBrowseFilePath.Text.Trim().Contains("DISPATCH"))
                        {
                            GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, "Invalid Dispatch File Path!!!", 3);
                            return;
                        }
                    }
                    if (rbPicking.Checked)
                    {
                        if (!txtBrowseFilePath.Text.Trim().Contains("PICKING"))
                        {
                            GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, "Invalid Picking File Path!!!", 3);
                            return;
                        }
                    }
                    GlobalVariable.mOutputFileName = file.Name.ToString().Trim();
                    string filePath = txtBrowseFilePath.Text.Trim() + "//" + GlobalVariable.mOutputFileName;
                    if (File.Exists(filePath) == true)
                    {
                        ConvertTXTtoDataTable(filePath, file.Name.ToString().Trim().Replace(".TXT", ""), ref dtFinal);
                    }
                }
                if (dtFinal.Rows.Count > 0)
                {
                    _plObj = new PL_PICKING_UPLOAD_DOWNLOAD();
                    _blObj = new BL_PICKING_UPLOAD_DOWNLOAD();
                    if (rbDispatch.Checked)
                    {
                        _plObj.DbType = "SAVE_DISPATCH_DATA";
                    }
                    else
                    {
                        _plObj.DbType = "SAVE_PICKING_DATA";
                    }

                    _plObj.CreatedBy = GlobalVariable.mSatoAppsLoginUser;
                    DataTable dtSave = dtFinal;
                    _plObj.dtPickingDataDownload = dtSave.AsDataView().ToTable(true, "PicklistNo", "PartNo", "Barcode", "Qty");
                    DataTable dataTable = _blObj.BL_ExecuteTask(_plObj);
                    if (dataTable.Rows.Count > 0)
                    {
                        if (dataTable.Rows[0]["RESULT"].ToString() == "Y")
                        {
                            GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, "Data saved  successfully!!", 1);
                        }
                        else
                        {
                            GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, dataTable.Rows[0][0].ToString(), 2);
                        }
                    }
                }
                dgv.DataSource = dtFinal;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        #region Constructor
        public frmDeviceDownload()
        {
            InitializeComponent();

        }
        #endregion

        #region Event
        private void frmTargetMaster_Load(object sender, EventArgs e)
        {
            //this.MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea;
            this.FormBorderStyle = FormBorderStyle.None;
            this.WindowState = FormWindowState.Maximized;
            rbPicking_CheckedChanged(null, null);
        }

        private void picBrowse_Click(object sender, EventArgs e)
        {
            try
            {
                FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog();
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    txtBrowseFilePath.Text = folderBrowserDialog.SelectedPath;
                }

            }
            catch (Exception ex)
            {
                GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, ex.Message, 3);

            }
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                SaveDownloadData();

            }
            catch (Exception ex)
            {

                GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, ex.Message, 3);
            }
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            for (int i = dgv.Rows.Count - 1; i >= 0; i--)
            {
                dgv.Rows.RemoveAt(i);
            }

            txtBrowseFilePath.Text = "";
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
        private void rbPicking_CheckedChanged(object sender, EventArgs e)
        {
            if (!rbPicking.Checked)
            {
                dgv.Columns.Clear();
                DataGridViewTextBoxColumn PicklistNo;
                DataGridViewTextBoxColumn Barcode;
                DataGridViewTextBoxColumn DispatchQty;
                DataGridViewTextBoxColumn DispatchBy;
                DataGridViewTextBoxColumn DispatchOn;
                PicklistNo = new DataGridViewTextBoxColumn();
                Barcode = new DataGridViewTextBoxColumn();
                DispatchQty = new DataGridViewTextBoxColumn();
                DispatchBy = new DataGridViewTextBoxColumn();
                DispatchOn = new DataGridViewTextBoxColumn();
                dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
                dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                PicklistNo,
                Barcode,
                DispatchQty,
                DispatchBy,
                DispatchOn
               });
                // 
                // DispatchOn
                // 
                DispatchOn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
                DispatchOn.DataPropertyName = "DispatchOn";
                DispatchOn.HeaderText = "Dispatch On";
                DispatchOn.Name = "DispatchOn";
                DispatchOn.ReadOnly = true;
                DispatchOn.DataPropertyName = "DispatchOn";
                // 
                // DispatchBy
                // 
                DispatchBy.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
                DispatchBy.DataPropertyName = "DispatchBy";
                DispatchBy.HeaderText = "Dispatch By";
                DispatchBy.Name = "DispatchBy";
                DispatchBy.ReadOnly = true;
                DispatchBy.Width = 160;
                DispatchBy.DataPropertyName = "DispatchBy";
                // 
                // DispatchQty
                // 
                DispatchQty.DataPropertyName = "Qty";
                DispatchQty.HeaderText = "Dispatch Qty";
                DispatchQty.Name = "DispatchQty";
                DispatchQty.ReadOnly = true;
                // 
                // Barcode
                // 
                Barcode.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
                Barcode.DataPropertyName = "Barcode";
                Barcode.HeaderText = "Barcode";
                Barcode.Name = "Barcode";
                Barcode.ReadOnly = true;
                Barcode.Width = 500;
                // 
                // PicklistNo
                // 
                PicklistNo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
                PicklistNo.DataPropertyName = "PicklistNo";
                PicklistNo.HeaderText = "Picklist No";
                PicklistNo.Name = "PicklistNo";
                PicklistNo.ReadOnly = true;
                PicklistNo.Width = 160;




            }
            else
            {
                dgv.Columns.Clear();
                DataGridViewTextBoxColumn PicklistNo;
                DataGridViewTextBoxColumn Barcode;
                DataGridViewTextBoxColumn PickedQty;
                DataGridViewTextBoxColumn PickedBy;
                DataGridViewTextBoxColumn PickedOn;
                PicklistNo = new DataGridViewTextBoxColumn();
                Barcode = new DataGridViewTextBoxColumn();
                PickedQty = new DataGridViewTextBoxColumn();
                PickedBy = new DataGridViewTextBoxColumn();
                PickedOn = new DataGridViewTextBoxColumn();
                dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
                dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                PicklistNo,
                Barcode,
                PickedQty,
                PickedBy,
                PickedOn
               });
                // 
                // PickedOn
                // 
                PickedOn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
                PickedOn.DataPropertyName = "CreatedOn";
                PickedOn.HeaderText = "Picked On";
                PickedOn.Name = "PickedOn";
                PickedOn.ReadOnly = true;
                PickedOn.DataPropertyName = "PickedOn";
                // 
                // PickedBy
                // 
                PickedBy.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
                PickedBy.DataPropertyName = "CreatedBy";
                PickedBy.HeaderText = "Picked By";
                PickedBy.Name = "PickedBy";
                PickedBy.ReadOnly = true;
                PickedBy.Width = 160;
                PickedBy.DataPropertyName = "PickedBy";
                // 
                // PickedQty
                // 
                PickedQty.DataPropertyName = "Qty";
                PickedQty.HeaderText = "Picked Qty";
                PickedQty.Name = "PickedQty";
                PickedQty.ReadOnly = true;

                // Barcode
                // 
                Barcode.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
                Barcode.DataPropertyName = "Barcode";
                Barcode.HeaderText = "Barcode";
                Barcode.Name = "Barcode";
                Barcode.ReadOnly = true;
                Barcode.Width = 500;
                // 
                // PicklistNo
                // 
                PicklistNo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
                PicklistNo.DataPropertyName = "PicklistNo";
                PicklistNo.HeaderText = "Picklist No";
                PicklistNo.Name = "PicklistNo";
                PicklistNo.ReadOnly = true;
                PicklistNo.Width = 160;



            }
        }

        #endregion

        private void rbDispatch_CheckedChanged(object sender, EventArgs e)
        {
            if (!rbPicking.Checked)
            {
                dgv.Columns.Clear();
                DataGridViewTextBoxColumn PicklistNo;
                DataGridViewTextBoxColumn Barcode;
                DataGridViewTextBoxColumn DispatchQty;
                DataGridViewTextBoxColumn DispatchBy;
                DataGridViewTextBoxColumn DispatchOn;
                PicklistNo = new DataGridViewTextBoxColumn();
                Barcode = new DataGridViewTextBoxColumn();
                DispatchQty = new DataGridViewTextBoxColumn();
                DispatchBy = new DataGridViewTextBoxColumn();
                DispatchOn = new DataGridViewTextBoxColumn();
                dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
                dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                PicklistNo,
                Barcode,
                DispatchQty,
                DispatchBy,
                DispatchOn
               });
                // 
                // DispatchOn
                // 
                DispatchOn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
                DispatchOn.DataPropertyName = "DispatchOn";
                DispatchOn.HeaderText = "Dispatch On";
                DispatchOn.Name = "DispatchOn";
                DispatchOn.ReadOnly = true;
                DispatchOn.DataPropertyName = "DispatchOn";
                // 
                // DispatchBy
                // 
                DispatchBy.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
                DispatchBy.DataPropertyName = "DispatchBy";
                DispatchBy.HeaderText = "Dispatch By";
                DispatchBy.Name = "DispatchBy";
                DispatchBy.ReadOnly = true;
                DispatchBy.Width = 160;
                DispatchBy.DataPropertyName = "DispatchBy";
                // 
                // DispatchQty
                // 
                DispatchQty.DataPropertyName = "Qty";
                DispatchQty.HeaderText = "Dispatch Qty";
                DispatchQty.Name = "DispatchQty";
                DispatchQty.ReadOnly = true;
                // 
                // Barcode
                // 
                Barcode.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
                Barcode.DataPropertyName = "Barcode";
                Barcode.HeaderText = "Barcode";
                Barcode.Name = "Barcode";
                Barcode.ReadOnly = true;
                Barcode.Width = 500;
                // 
                // PicklistNo
                // 
                PicklistNo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
                PicklistNo.DataPropertyName = "PicklistNo";
                PicklistNo.HeaderText = "Picklist No";
                PicklistNo.Name = "PicklistNo";
                PicklistNo.ReadOnly = true;
                PicklistNo.Width = 160;




            }
            else
            {
                dgv.Columns.Clear();
                DataGridViewTextBoxColumn PicklistNo;
                DataGridViewTextBoxColumn Barcode;
                DataGridViewTextBoxColumn PickedQty;
                DataGridViewTextBoxColumn PickedBy;
                DataGridViewTextBoxColumn PickedOn;
                PicklistNo = new DataGridViewTextBoxColumn();
                Barcode = new DataGridViewTextBoxColumn();
                PickedQty = new DataGridViewTextBoxColumn();
                PickedBy = new DataGridViewTextBoxColumn();
                PickedOn = new DataGridViewTextBoxColumn();
                dgv.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
                dgv.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
                PicklistNo,
                Barcode,
                PickedQty,
                PickedBy,
                PickedOn
               });
                // 
                // PickedOn
                // 
                PickedOn.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
                PickedOn.DataPropertyName = "CreatedOn";
                PickedOn.HeaderText = "Picked On";
                PickedOn.Name = "PickedOn";
                PickedOn.ReadOnly = true;
                PickedOn.DataPropertyName = "PickedOn";
                // 
                // PickedBy
                // 
                PickedBy.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
                PickedBy.DataPropertyName = "CreatedBy";
                PickedBy.HeaderText = "Picked By";
                PickedBy.Name = "PickedBy";
                PickedBy.ReadOnly = true;
                PickedBy.Width = 160;
                PickedBy.DataPropertyName = "PickedBy";
                // 
                // PickedQty
                // 
                PickedQty.DataPropertyName = "Qty";
                PickedQty.HeaderText = "Picked Qty";
                PickedQty.Name = "PickedQty";
                PickedQty.ReadOnly = true;
                // 
                // Barcode
                // 
                Barcode.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
                Barcode.DataPropertyName = "Barcode";
                Barcode.HeaderText = "Barcode";
                Barcode.Name = "Barcode";
                Barcode.ReadOnly = true;
                Barcode.Width = 500;
                // 
                // PicklistNo
                // 
                PicklistNo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None;
                PicklistNo.DataPropertyName = "PicklistNo";
                PicklistNo.HeaderText = "Picklist No";
                PicklistNo.Name = "PicklistNo";
                PicklistNo.ReadOnly = true;
                PicklistNo.Width = 160;



            }
        }
    }

}
