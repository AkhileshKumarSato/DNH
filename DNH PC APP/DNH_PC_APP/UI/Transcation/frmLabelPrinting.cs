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
    public partial class frmLabelPrinting : Form
    {

        #region Variables

        BL_LABEL_PRINTING _blObj = null;
        PL_LABEL_PRINTIING _plObj = null;

        Common _comObj = null;
        string _packType = string.Empty;
        DataTable dtBindGrid = new DataTable();
        #endregion

        #region Form Methods

        public frmLabelPrinting()
        {
            try
            {
                InitializeComponent();
                _blObj = new BL_LABEL_PRINTING();
                SetColumns();
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
              //  this.MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea;
                this.FormBorderStyle = FormBorderStyle.None;
                this.WindowState = FormWindowState.Maximized;

                if (GlobalVariable.UserGroup == "ADMIN" || GlobalVariable.UserGroup == "SUPERVISOR")
                {
                    gbPrintingParameter.Enabled = true;
                    btnPrint.Enabled = true;
                    btnStartModified.Enabled = false;
                    GlobalVariable.mAccessUser = GlobalVariable.UserName;
                }
                else
                {
                    btnStartModified.Enabled = true;
                    gbPrintingParameter.Enabled = false;
                    btnPrint.Enabled = false;
                }

                BindTotalDaysPrintCount();
                GetPart();
                GetOperator();
                chkBin_CheckedChanged(null, null);
                cbPartNo.Focus();
                
            }
            catch (Exception ex)
            {
                GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, ex.Message, 3);
            }
        }

        #endregion

        #region Button Event
        private void btnMinimize_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        private void btnPrint_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            try
            {
                //if (ValidateInput())
                //{
                bool bCheck = false;
                int iCounter = 0;
                int iRemQty = 0;
                int iActualQty = 0;
                for (int i = dgv.Rows.Count - 1; i >= 0; i--)
                {
                    if (Convert.ToBoolean(dgv.Rows[i].Cells["Select"].Value) == false)
                    { continue; }
                    if (Convert.ToBoolean(dgv.Rows[i].Cells["Select"].Value) == true)
                    {
                        btnPrint.Enabled = false;
                        iCounter = 0;
                        iRemQty = 0;
                        iActualQty = 0;
                        bCheck = true;
                        iActualQty = GetActualPackQty(i,
                           Convert.ToInt32(dgv.Rows[i].Cells["TotalQty"].Value.ToString()),
                           Convert.ToInt32(dgv.Rows[i].Cells["PackQty"].Value.ToString()), ref iCounter, ref iRemQty);
                        for (int j = 1; j <= iCounter; j++)
                        {
                            _comObj = new Common();
                            _plObj = new PL_LABEL_PRINTIING();


                            _plObj.PackType = dgv.Rows[i].Cells["PackType"].Value.ToString();
                            _plObj.PartNo = dgv.Rows[i].Cells["PartNo"].Value.ToString();
                            _plObj.PartName = dgv.Rows[i].Cells["PartName"].Value.ToString();
                            _plObj.PackingDate = dgv.Rows[i].Cells["PackingDate"].Value.ToString();
                            _plObj.PackingDateOnLabel = dgv.Rows[i].Cells["PackingDateOnLabel"].Value.ToString();
                            _plObj.TotalQty = Convert.ToInt32(dgv.Rows[i].Cells["TotalQty"].Value.ToString());

                            if (iRemQty == 0)
                            {
                                _plObj.Qty = Convert.ToInt32(dgv.Rows[i].Cells["PackQty"].Value.ToString());

                            }
                            else
                            {
                                _plObj.Qty = Convert.ToInt32(dgv.Rows[i].Cells["PackQty"].Value.ToString());
                                if (j == iCounter)
                                {
                                    _plObj.Qty = iRemQty;

                                }
                            }
                            _plObj.PackQty = Convert.ToInt32(dgv.Rows[i].Cells["PackQty"].Value.ToString());
                            _plObj.LotNo = dgv.Rows[i].Cells["LotNo"].Value.ToString();
                            _plObj.Barcode = _plObj.PartNo + "~" + _plObj.Qty + "~" + _plObj.LotNo + "~" + _comObj.GetBarcode("LABEL_PRINT");
                            _plObj.AccessUser = GlobalVariable.mAccessUser;
                            _plObj.Operator = dgv.Rows[i].Cells["Operator"].Value.ToString();
                            _plObj.CreatedBy = GlobalVariable.mSatoAppsLoginUser;
                            //If saving data

                            _plObj.DbType = "PRINT_SAVE";
                            DataTable dataTable = _blObj.BL_ExecuteTask(_plObj);
                            if (dataTable.Rows.Count > 0)
                            {
                                if (dataTable.Rows[0]["RESULT"].ToString() == "Y")
                                {
                                    if (_plObj.PackType=="BIN/PACKETS")
                                    {
                                        for (int iPacket = 1; iPacket <= 2; iPacket++)
                                        {
                                            _comObj.LablePrint(_plObj.PartNo.Trim(), _plObj.PartName.Trim(), _plObj.LotNo.Trim()
                                         , _plObj.PackingDateOnLabel.Trim(), _plObj.Qty.ToString(), _plObj.Operator
                                         , _plObj.Barcode);
                                        }
                                    }
                                    else
                                    {
                                        _comObj.LablePrint(_plObj.PartNo.Trim(), _plObj.PartName.Trim(), _plObj.LotNo.Trim()
                                         , _plObj.PackingDateOnLabel.Trim(), _plObj.Qty.ToString(), _plObj.Operator
                                         , _plObj.Barcode);
                                    }
                                    BindTotalDaysPrintCount();
                                    this.Cursor = Cursors.WaitCursor;
                                    Application.DoEvents();
                                    this.Cursor = Cursors.WaitCursor;
                                }
                                else
                                {
                                    GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, dataTable.Rows[0][0].ToString(), 3);
                                }
                            }
                            if (j == iCounter)
                            {
                                dgv.Rows.RemoveAt(i);
                            }
                        }

                    }

                }
                this.Cursor = Cursors.Default;
                if (!bCheck)
                {
                    btnPrint.Enabled = true;
                    GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, "At least one row selected!!!", 2);
                    return;
                }
                btnPrint.Enabled = true;
                GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, "Label print successfully!!", 1);

                frmModelMaster_Load(null, null);
               
                chkSelectAll.Checked = false;
                this.Cursor = Cursors.Default;
            }
            catch (Exception ex)
            {

                GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, ex.Message, 3);

            }
            finally
            {
                this.Cursor = Cursors.Default;
            }
        }
        private void btnReset_Click(object sender, EventArgs e)
        {
            try
            {

                Clear();

                //if (GlobalVariable.UserGroup.ToUpper() != "ADMIN")
                //{
                //    Common common = new Common();
                //    common.SetModuleChildSectionRights(this.Text, _IsUpdate, btnSave, btnDelete);
                //}
            }
            catch (Exception ex)
            {
                GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, ex.Message, 3);
            }
        }
        private void btnStartModified_Click(object sender, EventArgs e)
        {
            try
            {
                btnStartModified.Text = "Un Freeze";
                btnPrint.Enabled = false;
                gbPrintingParameter.Enabled = false;

                frmAccessPassword frmObj = new frmAccessPassword();
                frmObj.ShowDialog();
                if (frmObj.IsCancel == false)
                {
                    btnStartModified.Text = "Un Freeze";
                    btnPrint.Enabled = false;
                    gbPrintingParameter.Enabled = false;
                    btnPrint.Enabled = true;
                    return;
                }
                if (GlobalVariable.mAccessUser != "")
                {
                    if (btnStartModified.Text == "Un Freeze")
                    {
                        btnStartModified.Text = "Freeze";
                        btnPrint.Enabled = true;
                        gbPrintingParameter.Enabled = true;
                    }
                    else
                    {
                        btnStartModified.Text = "Un Freeze";
                        btnPrint.Enabled = true;
                        gbPrintingParameter.Enabled = true;
                    }
                }
                else
                {
                    btnStartModified.Text = "Un Freeze";
                    btnPrint.Enabled = false;
                    gbPrintingParameter.Enabled = false;
                }

            }
            catch (Exception ex)
            {
                GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, ex.Message, 3);
            }
        }
        private void btnAdd_Click(object sender, EventArgs e)
        {

            try
            {
                bool bExisting = false;
                if (ValidateInput())
                {
                    if (Convert.ToInt32(txtTotalQty.Text.Trim()) < Convert.ToInt32(txtPackQty.Text.Trim()))
                    {
                        GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, "Total Qty can't be less than Pack Qty!!", 3);
                        return;
                    }
                    for (int i = 0; i < dgv.Rows.Count; i++)
                    {
                        if (Convert.ToString(dgv.Rows[i].Cells["PartNo"].Value) == cbPartNo.Text.Trim()
                            && Convert.ToString(dgv.Rows[i].Cells["LotNo"].Value.ToString().ToUpper()) == txtLotNo.Text.Trim().ToUpper())
                        {
                            if(!GlobalVariable.mStoCustomFunction.ConfirmationMsg(GlobalVariable.mSatoApps, "Part No. and Lot No. is same you want add Qty ??"))
                            { return; }
                                dgv.Rows[i].Cells["TotalQty"].Value = Convert.ToString(Convert.ToInt32(dgv.Rows[i].Cells["TotalQty"].Value)
                                + Convert.ToInt32(txtTotalQty.Text.Trim()));
                            dgv.Rows[i].Cells["PackQty"].Value = Convert.ToString(Convert.ToInt32(dgv.Rows[i].Cells["PackQty"].Value)
                                + Convert.ToInt32(txtPackQty.Text.Trim()));
                            bExisting = true;
                        }

                    }
                    if (!bExisting)
                    {
                        dgv.Rows.Add(new string[] {"False",_packType, cbPartNo.Text.Trim(), lblDescription.Text,dpPackingDate.Value.ToString("yyyy-MM-dd"),dpPackingDate.Text.Trim(),
                            txtLotNo.Text.Trim().ToUpper(),txtTotalQty.Text.Trim(), txtPackQty.Text.Trim(), cbOperator.Text.Trim() });
                        dgv.Sort(dgv.Columns["LotNo"], ListSortDirection.Ascending);
                    }
                    Clear();
                }

            }
            catch (Exception ex)
            {
                GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, ex.Message, 3);
            }
        }
        private void btnRemove_Click(object sender, EventArgs e)
        {

            try
            {
                if (dgv.Rows.Count > 0)
                {
                    if (!GlobalVariable.mStoCustomFunction.ConfirmationMsg(GlobalVariable.mSatoApps, "Are you sure,You want to remove ??"))
                    { return; }
                    bool bCheck = false;
                    for (int i = dgv.Rows.Count-1; i >=0 ; i--)
                    {

                        if (Convert.ToBoolean(dgv.Rows[i].Cells["Select"].Value) == true)
                        {
                            bCheck = true;
                            dgv.Rows.Remove(dgv.Rows[i]);
                        }
                    }
                    if (bCheck == false)
                    {
                        GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, "Select Atleast on record", 2);
                        return;
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

        private void SetColumns()
        {
            try
            {
                dtBindGrid.Columns.AddRange(new[]
                {
                     new DataColumn("Barcode", typeof(string)),
                     new DataColumn("PartNo", typeof(string)),
                     new DataColumn("LotNo", typeof(string)),
                     new DataColumn("Qty", typeof(int)),
                     new DataColumn("Operator", typeof(string)),
                });

            }
            catch (Exception ex)
            {

                GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, ex.Message, 3);
            }
        }
        private void Clear()
        {
            try
            {
                cbOperator.SelectedIndex = cbPartNo.SelectedIndex = 0;
                cbOperator.SelectedIndex = cbOperator.FindString(GlobalVariable.UserName);
                dpPackingDate.Text =DateTime.Now.ToString( "dd-MMM-yyyy");
                cbOperator.DataSource = cbPartNo.DataSource = null;
                lblDescription.Text = "********************";
                txtPackQty.Text = "**************";
                txtLotNo.Text = "";
                txtTotalQty.Text = "";
                chkPackets.Checked = false;
                //lblTotalDaysPrintCount.Text = "0";

            }
            catch (Exception ex)
            {
                GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, ex.Message, 3);
            }
        }


        private void GetPart()
        {
            try
            {
                Common common = new Common();
                cbPartNo.Items.Clear();
                cbPartNo.Items.Add("--Select--");
                DataTable dt = common.GetPart();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                        cbPartNo.Items.Add(row["PART_NO"].ToString());
                    cbPartNo.SelectedIndex = 0;
                }
                else
                    GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, "Data not found", 3);

            }
            catch (Exception ex)
            {
                GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, ex.Message, 3);
            }
        }
        private void GetPartDisAndQty()
        {
            try
            {
                _plObj = new PL_LABEL_PRINTIING();
                _blObj = new BL_LABEL_PRINTING();
                _plObj.DbType = "GET_PART_DIS_QTY";
                _plObj.PartNo = cbPartNo.SelectedItem.ToString();
                DataTable dt = _blObj.BL_ExecuteTask(_plObj);
                if (dt.Rows.Count > 0)
                {
                    lblDescription.Text = dt.Rows[0]["DESCRIPTION"].ToString();
                    txtPackQty.Text = dt.Rows[0]["QTY"].ToString();
                    if (dt.Rows[0]["PACK_TYPE"].ToString() == "PACKET")
                    {
                        chkBin.Checked = chkPackets.Checked = true;
                    }
                    else
                    {
                        chkBin.Checked = true;
                        chkPackets.Checked = false;
                    }

                    txtLotNo.Focus();
                }
                else
                    GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, "Data not found", 3);

            }
            catch (Exception ex)
            {
                GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, ex.Message, 3);
            }
        }
        private void GetOperator()
        {
            try
            {
                Common common = new Common();
                cbOperator.Items.Clear();
                cbOperator.Items.Add("--Select--");
                DataTable dt = common.GetOperator();
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                        cbOperator.Items.Add(row["UserName"].ToString());
                    cbOperator.SelectedIndex = cbOperator.FindString(GlobalVariable.UserName);
                }
                else
                    GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, "Data not found", 3);

            }
            catch (Exception ex)
            {
                GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, ex.Message, 3);
            }
        }
        private void BindGrid()
        {
            try
            {
                dgv.DataSource = dtBindGrid;
                // lblCount.Text = "Rows Count : " + dgv.Rows.Count;
            }
            catch (Exception ex)
            {
                GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, ex.Message, 3);
            }
        }

        private int GetActualPackQty(int iRowIndex, int iTotalQty, int iPackQty, ref int iCounter, ref int iRefRemQty)
        {
            int iActualPackQty = 0;
            if (dgv.Rows.Count > 0)
            {
                iRefRemQty = 0;
                iCounter = 0;
                int iRemQty = Convert.ToInt32(dgv.Rows[iRowIndex].Cells["TotalQty"].Value) % Convert.ToInt32(dgv.Rows[iRowIndex].Cells["PackQty"].Value);
                if (iRemQty == 0)
                {
                    iCounter = Convert.ToInt32(dgv.Rows[iRowIndex].Cells["TotalQty"].Value) / Convert.ToInt32(dgv.Rows[iRowIndex].Cells["PackQty"].Value);
                    iActualPackQty = Convert.ToInt32(dgv.Rows[iRowIndex].Cells["PackQty"].Value);
                }
                else
                {
                    iCounter = Convert.ToInt32(dgv.Rows[iRowIndex].Cells["TotalQty"].Value) / Convert.ToInt32(dgv.Rows[iRowIndex].Cells["PackQty"].Value);
                    iCounter = iCounter + 1;
                    iRefRemQty = iRemQty;
                    iActualPackQty = Convert.ToInt32(dgv.Rows[iRowIndex].Cells["PackQty"].Value);
                }

            }
            return iActualPackQty;
        }

        private void BindTotalDaysPrintCount()
        {
            try
            {
                _plObj = new PL_LABEL_PRINTIING();
                _blObj = new BL_LABEL_PRINTING();
                _plObj.DbType = "GET_DAYS_TOTAL_PRINT_COUNT";
                DataTable dt = _blObj.BL_ExecuteTask(_plObj);
                if (dt.Rows.Count > 0)
                {
                    lblTotalDaysPrintCount.Text = dt.Rows[0][0].ToString();
                }
                else
                    lblTotalDaysPrintCount.Text = "0";
            }
            catch (Exception ex)
            {
                GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, ex.Message, 3);
            }
        }
        private bool ValidateInput()
        {
            try
            {
                if (cbPartNo.SelectedIndex <= 0)
                {
                    GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, "Part No. can't be blank!!", 3);
                    cbPartNo.Focus();
                    cbPartNo.SelectAll();
                    return false;
                }
                if (txtPackQty.Text == "0")
                {
                    GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, "Pack Qty can't be Zero!!", 3);
                    return false;
                }

                if (txtLotNo.Text.Trim().Length == 0)
                {
                    GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, "Lot No. can't be blank!!", 3);
                    txtLotNo.Focus();
                    return false;
                }
                if (txtTotalQty.Text.Trim().Length == 0)
                {
                    GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, "Total Qty. can't be blank!!", 3);
                    txtTotalQty.Focus();
                    return false;
                }
                if (cbOperator.SelectedIndex <= 0)
                {
                    GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, "Operator can't be blank!!", 3);
                    cbOperator.Focus();
                    cbOperator.SelectAll();
                    return false;
                }
                if (GlobalVariable.mAccessUser == "")
                {
                    GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, "Given access user can't be blank!!", 3);
                    return false;
                }
                if (GlobalVariable.mRevNo == "" || GlobalVariable.mRevName == "" || GlobalVariable.mRevDate == "")
                {
                    GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, "Revision Details Not Found!!", 3);
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


        #endregion

        #region TextBox Event


        private void chkBin_CheckedChanged(object sender, EventArgs e)
        {
            if (chkBin.Checked && chkPackets.Checked == false)
            {
                _packType = "BIN";
            }
            else
            {

                _packType = "BIN/PACKETS";
            }
        }
        private void chkPackets_CheckedChanged(object sender, EventArgs e)
        {
            if (chkPackets.Checked)
            {
                _packType = "BIN/PACKETS";
            }
            else
            {
                _packType = "BIN";
            }
        }
        private void cbPartNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cbPartNo.SelectedIndex > 0)
            {
                GetPartDisAndQty();
            }
        }

        private void txtPackQty_KeyPress(object sender, KeyPressEventArgs e)
        {
            GlobalVariable.allowOnlyNumeric(sender, e);
        }
        private void txtLotNo_KeyPress(object sender, KeyPressEventArgs e)
        {
           
        }
        private void txtTotalQty_KeyPress(object sender, KeyPressEventArgs e)
        {
            GlobalVariable.allowOnlyNumeric(sender, e);
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

        
    }
}
