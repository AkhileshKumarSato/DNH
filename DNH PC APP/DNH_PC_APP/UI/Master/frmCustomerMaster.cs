using DNH_BL;
using DNH_COMMON;
using DNH_PL;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace DNH_PC_APP
{
    public partial class frmCustomerMaster : Form
    {

        #region Variables

        BL_CUSTOMER_MASTER _blObj = null;
        PL_CUSTOMER_MASTER _plObj = null;
        bool _IsUpdate = false;

        #endregion

        #region Form Methods

        public frmCustomerMaster()
        {
            try
            {
                InitializeComponent();
                _blObj = new BL_CUSTOMER_MASTER();
            }
            catch (Exception ex)
            {
                GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, ex.Message, 3);
            }
        }

        private void frmModelMaster_Load(object sender, EventArgs e)
        {
            try
            {
               // this.MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea;
                this.FormBorderStyle = FormBorderStyle.None;
                this.WindowState = FormWindowState.Maximized;

                btnDelete.Enabled = false;
                if (GlobalVariable.UserGroup.ToUpper() != "ADMIN")
                {
                    Common common = new Common();
                    common.SetModuleChildSectionRights(this.Text, _IsUpdate, btnSave, btnDelete);
                }
                BindGrid();
                txtCustCode.Focus();
            }
            catch (Exception ex)
            {
                GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, ex.Message, 3); ;
            }
        }

        #endregion

        #region Button Event
        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        private void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateInput())
                {
                    _plObj = new PL_CUSTOMER_MASTER();
                    _plObj.Cust_Code = txtCustCode.Text.Trim();
                    _plObj.Cust_Name = txtCustName.Text.Trim();
                    _plObj.Cust_Address = txtCustAddress.Text.Trim();
                    _plObj.Kanban = Convert.ToBoolean(chkKanban.Checked);
                    _plObj.CreatedBy = GlobalVariable.mSatoAppsLoginUser;
                    //If saving data
                    if (_IsUpdate == false)
                    {
                        _plObj.DbType = "INSERT";
                        DataTable dataTable = _blObj.BL_ExecuteTask(_plObj);
                        if (dataTable.Rows.Count > 0)
                        {
                            if (dataTable.Rows[0]["RESULT"].ToString() == "Y")
                            {
                                btnReset_Click(sender, e);
                                GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, "Saved successfully!!", 1);
                                frmModelMaster_Load(null, null);
                            }
                            else
                            {
                                GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, dataTable.Rows[0][0].ToString(), 3);
                            }
                        }
                    }
                    else // if updating data
                    {
                        _plObj.DbType = "UPDATE";
                        DataTable dataTable = _blObj.BL_ExecuteTask(_plObj);
                        if (dataTable.Rows.Count > 0)
                        {
                            if (dataTable.Rows[0]["RESULT"].ToString() == "Y")
                            {
                                btnReset_Click(sender, e);
                                GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, "Updated successfully!!", 1);
                            }
                            else
                            {
                                GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, dataTable.Rows[0][0].ToString(), 3);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Violation of PRIMARY KEY"))
                {
                    GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, "UserId already exist!!", 3);
                }
                else
                {
                    GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, ex.Message, 3);
                }
            }
        }
        private void btnReset_Click(object sender, EventArgs e)
        {
            try
            {

                Clear();
                BindGrid();

                if (GlobalVariable.UserGroup.ToUpper() != "ADMIN")
                {
                    Common common = new Common();
                    common.SetModuleChildSectionRights(this.Text, _IsUpdate, btnSave, btnDelete);
                }
            }
            catch (Exception ex)
            {
                GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, ex.Message, 3); ;
            }
        }
        private void btnDelete_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(txtCustCode.Text))
                {
                    GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, "Customer Code can't be blank!!", 3);
                    return;
                }
                if (GlobalVariable.mStoCustomFunction.ConfirmationMsg(GlobalVariable.mSatoApps, "Äre you sure to delete the record !!"))
                {
                    _plObj = new PL_CUSTOMER_MASTER();
                    _blObj = new BL_CUSTOMER_MASTER();
                    _plObj.Cust_Code = txtCustCode.Text.Trim();
                    _plObj.DbType = "DELETE";
                    DataTable dataTable = _blObj.BL_ExecuteTask(_plObj);
                    if (dataTable.Rows.Count > 0)
                    {
                        if (dataTable.Rows[0][0].ToString().StartsWith("Y"))
                        {
                            btnReset_Click(sender, e);
                            GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, "Deleted successfully!!", 1);
                            frmModelMaster_Load(null, null);
                        }
                        else
                        {
                            GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, dataTable.Rows[0][0].ToString(), 3);
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("conflicted with the REFERENCE constraint"))
                {
                    GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, "This is already in use!!!", 3);
                }
                else
                {
                    GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, ex.Message, 3); ;
                }
            }
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            try
            {
                OpenFileDialog folderBrowserDialog = new OpenFileDialog();
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    txtFilePath.Text = folderBrowserDialog.FileName;
                }

            }
            catch (Exception ex)
            {

                GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, ex.Message, 3);
            }
        }

        private void btnProcess_Click(object sender, EventArgs e)
        {

            try
            {
                if (txtFilePath.Text.Length == 0)
                {
                    GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, "Select browse file!!!", 3);
                    return;
                }
                DataTable dataReturn = null;
                clsODBC oOdbc = new clsODBC();
                oOdbc.DataSource = txtFilePath.Text.Trim();
                if (oOdbc.Connect())
                {
                    string Query = "SELECT * FROM [Sheet1$]";
                    DataTable dtResultSet = oOdbc.GetDataTable(Query);
                    oOdbc.Disconnect();
                    for (int i = 0; i < dtResultSet.Rows.Count; i++)
                    {
                        _plObj = new PL_CUSTOMER_MASTER();
                        _blObj = new BL_CUSTOMER_MASTER();
                        _plObj.DbType = "INSERT";
                        _plObj.Cust_Code = dtResultSet.Rows[i]["Cust_Code"].ToString();
                        _plObj.Cust_Name = dtResultSet.Rows[i]["Cust_Name"].ToString();
                        _plObj.Cust_Address = dtResultSet.Rows[i]["Cust_Address"].ToString();
                        _plObj.Kanban = Convert.ToBoolean( dtResultSet.Rows[i]["Kanban"]);
                        _plObj.CreatedBy = GlobalVariable.mSatoAppsLoginUser;
                        dataReturn = _blObj.BL_ExecuteTask(_plObj);
                        if (dataReturn.Rows.Count > 0)
                        {
                            if (!dataReturn.Rows[0][0].ToString().StartsWith("Y"))
                            {
                                GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, dataReturn.Rows[0][0].ToString(), 3);
                                break;
                            }
                        }
                    }
                    if (dataReturn.Rows[0][0].ToString().StartsWith("Y"))
                    {
                        btnReset_Click(sender, e);
                        GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, "Saved successfully!!", 1);
                        frmModelMaster_Load(null, null);
                    }
                    else
                    {
                        GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, dataReturn.Rows[0][0].ToString(), 3);
                    }
                }
            }
            catch (Exception ex)
            {

                GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, ex.Message, 3);
            }

        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion

        #region Methods

        private void Clear()
        {
            try
            {
                txtCustCode.Text = "";
                txtCustName.Text = "";
                txtCustAddress.Text = "";
                chkKanban.Checked = false;
                txtCustCode.Enabled = true;
                btnDelete.Enabled = false;
                _IsUpdate = false;
            }
            catch (Exception ex)
            {
                GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, ex.Message, 3); ;
            }
        }




        private void BindGrid()
        {
            try
            {
                _plObj = new PL_CUSTOMER_MASTER();
                _blObj = new BL_CUSTOMER_MASTER();
                _plObj.DbType = "SELECT";
                DataTable dt = _blObj.BL_ExecuteTask(_plObj);
                dgv.DataSource = dt;
                lblCount.Text = "Rows Count : " + dgv.Rows.Count;
            }
            catch (Exception ex)
            {
                GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, ex.Message, 3); ;
            }
        }

        private bool ValidateInput()
        {
            try
            {
                if (txtCustCode.Text.Trim().Length == 0)
                {
                    GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, "Customer Code. can't be blank!!", 3);
                    txtCustCode.Focus();
                    txtCustCode.SelectAll();
                    return false;
                }
                if (txtCustName.Text.Trim().Length == 0)
                {
                    GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, "Customer Name can't be blank!!", 3);
                    txtCustName.Focus();
                    txtCustName.SelectAll();
                    return false;
                }

                if (txtCustAddress.Text.Trim().Length == 0)
                {
                    GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, "Customer Address can't be blank!!", 3);
                    txtCustAddress.Focus();
                    return false;
                }

                return true;
            }
            catch (Exception ex) { throw ex; }
        }

        #endregion

        #region Label Event

        #endregion

        #region DataGridView Events
        private void dgv_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                if (e.RowIndex <= -1)
                {
                    return;
                }
                Clear();
                txtCustCode.Text = dgv.Rows[e.RowIndex].Cells["CUST_CODE"].Value.ToString();
                txtCustName.Text = dgv.Rows[e.RowIndex].Cells["CUST_NAME"].Value.ToString();
                txtCustAddress.Text = dgv.Rows[e.RowIndex].Cells["CUST_ADDRESS"].Value.ToString();
                chkKanban.Checked = Convert.ToBoolean(dgv.Rows[e.RowIndex].Cells["KANBAN"].Value.ToString());
                btnDelete.Enabled = true;
                txtCustCode.Enabled = false;
                _IsUpdate = true;
                if (GlobalVariable.UserGroup.ToUpper() != "ADMIN")
                {
                    Common common = new Common();
                    common.SetModuleChildSectionRights(this.Text, _IsUpdate, btnSave, btnDelete);
                }
            }
            catch (Exception ex)
            {
                GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, ex.Message, 3);
            }
        }

        #endregion

        #region TextBox Event

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            (dgv.DataSource as DataTable).DefaultView.RowFilter = string.Format("CUST_NAME LIKE '%{0}%'", txtSearch.Text);
        }




        #endregion

    }
}
