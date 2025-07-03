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
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DNH_PC_APP
{
    public partial class frmPicklist_Generate : Form
    {

        #region Variables

        private BL_PICKLIST _blObj = null;
        private PL_PICKLIST _plObj = null;
        private bool _IsUpdate = false;
        private DataTable dtPicklist = null;

        #endregion

        #region Form Methods

        public frmPicklist_Generate()
        {
            try
            {
                InitializeComponent();
                _blObj = new BL_PICKLIST();
                SetPicklistColumns();
            }
            catch (Exception ex)
            {
                GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, ex.Message, 3);
            }
        }

        private void frmPicklist_Generate_Load(object sender, EventArgs e)
        {
            try
            {
            //    this.MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea;
                this.FormBorderStyle = FormBorderStyle.None;
                this.WindowState = FormWindowState.Maximized;

                if (GlobalVariable.UserGroup.ToUpper() != "ADMIN")
                {
                    Common common = new Common();
                    common.SetModuleChildSectionRights(this.Text, _IsUpdate, btnSave, null);
                }
                BindPicklistNo();
                BindCustomer();
                BindGrid();
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
        private void btnCreatePicklist_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateInput())
                {
                    dtPicklist.Rows.Clear();
                    string sPickistNo = string.Empty;
                    _plObj = new PL_PICKLIST();
                    for (int i = dgv.Rows.Count - 1; i >= 0; i--)
                    {
                        if (Convert.ToBoolean(dgv.Rows[i].Cells["Select"].Value) == true)
                        {
                            if (Convert.ToInt32(dgv.Rows[i].Cells["EnterQty"].Value) > Convert.ToInt32(dgv.Rows[i].Cells["TotalQty"].Value))
                            {
                                GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, "This Part No. (" + dgv.Rows[i].Cells["PartNo"].Value + ") Enter Qty can't be greater than Total Qty!!!", 3);
                                continue;
                            }
                            DataRow dataRow = dtPicklist.NewRow();
                            dataRow["PART_NO"] = dgv.Rows[i].Cells["PartNo"].Value.ToString().Trim();
                            dataRow["PART_NAME"] = dgv.Rows[i].Cells["PartName"].Value.ToString().Trim();
                            dataRow["ENTER_QTY"] = Convert.ToInt32(dgv.Rows[i].Cells["EnterQty"].Value);
                            dtPicklist.Rows.Add(dataRow);
                        }
                    }
                    //If saving data
                    _plObj.DbType = "INSERT";
                    _plObj.Cust_Code = cbCustomer.SelectedValue.ToString().Trim();
                    _plObj.Picklist_Date = dpPicklistDate.Text.Trim();
                    _plObj.CreatedBy = GlobalVariable.mSatoAppsLoginUser;
                    _plObj.dtPicklist = dtPicklist;
                    DataTable dataTable = _blObj.BL_ExecuteTask(_plObj);
                    if (dataTable.Rows.Count > 0)
                    {
                        if (dataTable.Rows[0]["RESULT"].ToString() == "Y")
                        {
                            btnReset_Click(sender, e);
                            GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, "Piclist No (" + dataTable.Rows[0]["PKLIST"].ToString() + ") generated  successfully!!", 1);
                            frmPicklist_Generate_Load(null, null);
                        }
                        else
                        {
                            GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, dataTable.Rows[0][0].ToString(), 2);
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
                    common.SetModuleChildSectionRights(this.Text, _IsUpdate, btnSave, null);
                }
            }
            catch (Exception ex)
            {
                GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, ex.Message, 3); ;
            }
        }
        private void picDeletePicklist_Click(object sender, EventArgs e)
        {
            try
            {
                if (cbPicklistNo.SelectedIndex <=0)
                {

                    GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, "Picklist No. can't be blank!!", 3);
                    cbPicklistNo.Focus();
                    cbPicklistNo.SelectAll();
                    return;

                }
                frmAccessPassword frmObj = new frmAccessPassword();
                frmObj.ShowDialog();
                if (frmObj.IsCancel == true)
                {
                    _plObj = new PL_PICKLIST();
                    _blObj = new BL_PICKLIST();
                    _plObj.DbType = "DELETE_PICKLIST";
                    _plObj.Picklist_No = cbPicklistNo.Text.Trim();
                    DataTable dtPiclist = _blObj.BL_ExecuteTask(_plObj);
                    if (dtPiclist.Rows.Count > 0)
                    {
                        GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, dtPiclist.Rows[0][0].ToString(), 1);
                        frmPicklist_Generate_Load(null, null);
                    }
                }


            }
            catch (Exception ex)
            {
                GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, ex.Message, 3); ;
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
                cbCustomer.DataSource = null;
                lblAddress.Text = "********************";
                dpPicklistDate.Text = DateTime.Now.ToString("dd-MM-yyyy");

            }
            catch (Exception ex)
            {
                GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, ex.Message, 3); ;
            }
        }

        private void SetPicklistColumns()
        {
            dtPicklist = new DataTable();
            dtPicklist.Columns.Add("PART_NO");
            dtPicklist.Columns.Add("PART_NAME");
            dtPicklist.Columns.Add("ENTER_QTY");
        }

        private void BindCustomer()
        {
            try
            {
                Common common = new Common();
                DataTable dt = common.GetCustomer();
                cbCustomer.DataSource = null;
                DataTable dtCust = common.GetCustomer();
                if (dtCust.Rows.Count > 0)
                {
                    GlobalVariable.BindCombo(cbCustomer, dtCust);
                    cbCustomer.SelectedIndex = 0;
                }
                else
                {
                    GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, "Data not found", 3);
                }
            }
            catch (Exception ex)
            {
                GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, ex.Message, 3); ;
            }
        }
        private void BindPicklistNo()
        {
            try
            {
                _plObj = new PL_PICKLIST();
                _blObj = new BL_PICKLIST();
                _plObj.DbType = "BIND_DELETE_PICKLIST";
                DataTable dtPiclist = _blObj.BL_ExecuteTask(_plObj);
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
        private void BindGrid()
        {
            try
            {
                _plObj = new PL_PICKLIST();
                _blObj = new BL_PICKLIST();
                _plObj.DbType = "BIND_PICK_PART";
                DataTable dt = _blObj.BL_ExecuteTask(_plObj);
                dgv.DataSource = dt;

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
                if (dpPicklistDate.Value == null)
                {
                    GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, "Picklist Date can't be blank!!", 3);
                    dpPicklistDate.Focus();

                    return false;
                }
                if (cbCustomer.SelectedIndex <= 0)
                {
                    GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, "Customer can't be blank!!", 3);
                    cbCustomer.Focus();
                    cbCustomer.SelectAll();
                    return false;
                }
                if (dgv.Rows.Count == 0)
                {
                    GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, "Part data not found!!", 3);

                    return false;
                }

                bool bCheck = false;
                for (int i = dgv.Rows.Count - 1; i >= 0; i--)
                {
                    if (Convert.ToBoolean(dgv.Rows[i].Cells["Select"].Value) == false)
                    {
                        continue;
                    }
                    if (Convert.ToBoolean(dgv.Rows[i].Cells["Select"].Value) == true)
                    {
                        bCheck = true;

                    }
                }
                if (bCheck == false)
                {
                    GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, "At least one row selected!!!", 2);
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

            }
            catch (Exception ex)
            {
                GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, ex.Message, 3);
            }
        }
        private void dgv_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (dgv.Columns[e.ColumnIndex].Name == "EnterQty")
            {
                dgv.CurrentCell = dgv.Rows[e.RowIndex].Cells[e.ColumnIndex];
                dgv.BeginEdit(true);
            }
        }
        private void dgv_EditingControlShowing(object sender, DataGridViewEditingControlShowingEventArgs e)
        {
            try
            {
                e.Control.KeyPress += new KeyPressEventHandler(Control_KeyPress);

            }
            catch (Exception ex)
            {
                GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, ex.Message, 3);
            }
        }
        private void dgv_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            try
            {
                int columnindex = e.ColumnIndex;
                int rowindex = e.RowIndex;

                if (dgv.Columns[columnindex].Name == "EnterQty" && (dgv["EnterQty", e.RowIndex].EditedFormattedValue.ToString() == "" || Convert.ToInt32(dgv["EnterQty", e.RowIndex].EditedFormattedValue) == 0))
                {
                    dgv["EnterQty", e.RowIndex].Value = dgv["TotalQty", e.RowIndex].EditedFormattedValue;
                    dgv.RefreshEdit();
                    dgv.BeginEdit(true);
                }
                if (dgv.Columns[columnindex].Name == "EnterQty" && dgv["EnterQty", e.RowIndex].EditedFormattedValue.ToString() != "")
                {
                    if (Convert.ToInt32(dgv["EnterQty", e.RowIndex].EditedFormattedValue) > Convert.ToInt32(dgv["TotalQty", e.RowIndex].Value))
                    {
                        GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, "This Part No. (" + dgv["PartNo", e.RowIndex].Value + ") Enter Qty can't be greater than Total Qty!!!", 3);
                        dgv.CurrentCell = dgv[e.ColumnIndex, e.RowIndex];
                        dgv.Focus();
                        dgv.BeginEdit(true);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, ex.Message, 3);
            }
        }
        private void chkSelectAll_CheckedChanged(object sender, EventArgs e)
        {
            if (chkSelectAll.Checked)
            {
                for (int i = 0; i < dgv.Rows.Count; i++)
                {
                    dgv.Rows[i].Cells["Select"].Value = true;
                }

            }
            else
            {
                for (int i = 0; i < dgv.Rows.Count; i++)
                {
                    dgv.Rows[i].Cells["Select"].Value = false;
                }
            }
        }
        #endregion

        #region TextBox Event
        private void Control_KeyPress(object sender, KeyPressEventArgs e)
        {
            try
            {
                if (dgv.Columns[dgv.CurrentCell.ColumnIndex].Name == "EnterQty")
                {
                    if (!char.IsDigit(e.KeyChar) && e.KeyChar != 8)
                    {
                        e.Handled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void cbCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbCustomer.SelectedIndex > 0)
            {
                try
                {
                    _plObj = new PL_PICKLIST();
                    _blObj = new BL_PICKLIST();
                    _plObj.DbType = "GET_CUST_ADD";
                    _plObj.Cust_Code = Convert.ToString(cbCustomer.SelectedValue);
                    DataTable dt = _blObj.BL_ExecuteTask(_plObj);
                    if (dt.Rows.Count > 0)
                    {
                        lblAddress.Text = dt.Rows[0]["CUST_ADDRESS"].ToString();
                    }

                }
                catch (Exception ex)
                {
                    GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, ex.Message, 3);
                }
            }

        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            (dgv.DataSource as DataTable).DefaultView.RowFilter = string.Format("PART_NO LIKE '%{0}%' or DESCRIPTION LIKE '%{0}%'", txtSearch.Text);
        }
        private void picView_Click(object sender, EventArgs e)
        {
            if (cbPicklistNo.SelectedIndex <= 0)
            {
                GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, "Select Picklist No.!!!", 1);
                return;
            }
            frmPicklistView picklistView = new frmPicklistView(cbPicklistNo.Text.Trim());
            picklistView.ShowDialog();
        }









        #endregion


    }
}
