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
    public partial class frmReprinting : Form
    {

        #region Variables

        private BL_LABEL_REPRINTING _blObj = null;
        private PL_LABEL_REPRINTING _plObj = null;
        private Common _comObj = null;
        private string _packType = string.Empty;
        private DataTable dtBindGrid = new DataTable();
        #endregion

        #region Form Methods

        public frmReprinting()
        {
            try
            {
                InitializeComponent();
                _blObj = new BL_LABEL_REPRINTING();
                _plObj = new PL_LABEL_REPRINTING();
            }
            catch (Exception ex)
            {
                GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, ex.Message, 3);
            }
        }

        private void frmReprinting_Load(object sender, EventArgs e)
        {
            try
            {
                //this.MaximizedBounds = Screen.FromHandle(this.Handle).WorkingArea;
                this.FormBorderStyle = FormBorderStyle.None;
                this.WindowState = FormWindowState.Maximized;
                dpToDate_CloseUp(null, null);

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
        private void btnRePrint_Click(object sender, EventArgs e)
        {
            Cursor.Current = Cursors.WaitCursor;
            try
            {
                if (dgv.Rows.Count == 0)
                {
                    GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, "No data found for Reprint!!!", 2);
                    return;
                }

                bool bCheck = false;
                int iCounter = 0;
                int iRemQty = 0;
                int iActualQty = 0;
                //for (int i = dgv.Rows.Count - 1; i >= 0; i--)
                //{//commented
                for (int i = 0; i < dgv.Rows.Count; i++)
                {
                    if (Convert.ToBoolean(dgv.Rows[i].Cells["Select"].Value) == false)
                    { continue; }
                    if (Convert.ToBoolean(dgv.Rows[i].Cells["Select"].Value) == true)
                    {
                        bCheck = true;
                        _plObj = new PL_LABEL_REPRINTING();
                        _plObj.DbType = "PRINT_DATA";
                        _plObj.Barcode = dgv.Rows[i].Cells["BARCODE"].Value.ToString();
                        DataTable dtPrint = _blObj.BL_ExecuteTask(_plObj);
                        if (dtPrint.Rows.Count > 0)
                        {
                            using (Common common = new Common())
                            {
                                if (dgv.Rows[i].Cells["PackType"].Value.ToString().Trim() == "BIN/PACKET")
                                {
                                    for (int iPacket = 0; iPacket < 2; iPacket++)
                                    {

                                        common.LablePrint(dtPrint.Rows[0]["PART_NO"].ToString()
                                            , dtPrint.Rows[0]["DESCRIPTION"].ToString()
                                            , dtPrint.Rows[0]["LOT_NO"].ToString()
                                            , dtPrint.Rows[0]["DateOfPacking"].ToString()
                                            , dtPrint.Rows[0]["QTY"].ToString()
                                            , dtPrint.Rows[0]["OPERATOR"].ToString()
                                            , dtPrint.Rows[0]["BARCODE"].ToString());

                                    }
                                }
                                else
                                {
                                    common.LablePrint(dtPrint.Rows[0]["PART_NO"].ToString()
                                           , dtPrint.Rows[0]["DESCRIPTION"].ToString()
                                           , dtPrint.Rows[0]["LOT_NO"].ToString()
                                           , dtPrint.Rows[0]["DateOfPacking"].ToString()
                                           , dtPrint.Rows[0]["QTY"].ToString()
                                           , dtPrint.Rows[0]["OPERATOR"].ToString()
                                           , dtPrint.Rows[0]["BARCODE"].ToString());
                                }
                                _plObj = new PL_LABEL_REPRINTING();
                                _blObj = new BL_LABEL_REPRINTING();
                                _plObj.DbType = "RE_PRINT_SAVE";
                                _plObj.PackType = dgv.Rows[i].Cells["PackType"].Value.ToString().Trim();
                                _plObj.PartNo = dtPrint.Rows[0]["PART_NO"].ToString();
                                _plObj.Qty = Convert.ToInt32( dtPrint.Rows[0]["QTY"].ToString());
                                _plObj.LotNo = dtPrint.Rows[0]["LOT_NO"].ToString();
                                _plObj.Barcode = dtPrint.Rows[0]["BARCODE"].ToString();
                                _plObj.CreatedBy= GlobalVariable.mSatoAppsLoginUser;
                                _blObj.BL_ExecuteTask(_plObj);
                            }
                        }


                    }

                }
                if (!bCheck)
                {
                    GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, "At least one row selected!!!", 2);
                    return;
                }

                GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, "Label Re-print successfully!!", 1);
                chkSelectAll.Checked = false;
                btnReset_Click(sender, e);
                dpFromDate.Value = dpToDate.Value = DateTime.Now;
                dpToDate_CloseUp(null, null);
                Cursor.Current = Cursors.Default;
            }
            catch (Exception ex)
            {

                GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, ex.Message, 3);

            }
            finally
            {
                Cursor.Current = Cursors.Default;
            }
        }
        private void btnReset_Click(object sender, EventArgs e)
        {
            try
            {

                Clear();

            }
            catch (Exception ex)
            {
                GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, ex.Message, 3);
            }
        }

        private void btnSearch_Click(object sender, EventArgs e)
        {

            try
            {
                bool bExisting = false;
                if (ValidateInput())
                {

                    _plObj = new PL_LABEL_REPRINTING();
                    _plObj.DbType = "BIND_VIEW";
                    _plObj.FromDate = dpFromDate.Value.ToString("yyyy-MM-dd");
                    _plObj.ToDate = dpToDate.Value.ToString("yyyy-MM-dd");
                    _plObj.PartNo = cmbPartNo.Text.Trim();
                    _plObj.LotNo = cmbLotNo.Text.Trim();
                    _plObj.SerialNo = cmbSerialNo.Text.Trim();
                    DataTable dataTable = _blObj.BL_ExecuteTask(_plObj);
                    dgv.DataSource = dataTable.DefaultView;

                    if (txtFromLabel.Text.Length > 0 && txtToLabel.Text.Length > 0)
                    {
                        dataTable.DefaultView.RowFilter = " SUBSTRING(BARCODE,LEN(BARCODE)-3,4) >= " + txtFromLabel.Text.Trim().PadLeft(4, '0') + " AND SUBSTRING(BARCODE,LEN(BARCODE)-3,4) <= " + txtToLabel.Text.Trim().PadLeft(4, '0') + "  ";
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
                //dpFromDate.Value = dpToDate.Value = DateTime.Now;
                // cmbLotNo.SelectedIndex = cmbPartNo.SelectedIndex = cmbSerialNo.SelectedIndex = 0;
                cmbPartNo.Items.Clear();
                cmbLotNo.Items.Clear();
                cmbSerialNo.Items.Clear();
                cmbPartNo.Text = cmbLotNo.Text = cmbSerialNo.Text = string.Empty;
                cmbLotNo.DataSource = cmbPartNo.DataSource = cmbSerialNo.DataSource = null;
                for (int i = dgv.Rows.Count - 1; i >= 0; i--)
                {
                    dgv.Rows.RemoveAt(i);
                }
                txtFromLabel.Text = txtToLabel.Text = string.Empty;
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
                _plObj = new PL_LABEL_REPRINTING();
                _plObj.DbType = "GET_PART";
                _plObj.FromDate = dpFromDate.Value.ToString("yyyy-MM-dd");
                _plObj.ToDate = dpToDate.Value.ToString("yyyy-MM-dd");
                cmbPartNo.Items.Clear();
                cmbPartNo.Items.Add("--Select--");
                DataTable dt = _blObj.BL_ExecuteTask(_plObj);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        cmbPartNo.Items.Add(row["PART_NO"].ToString());
                    }

                    cmbPartNo.SelectedIndex = 0;
                }
                else
                {
                    GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, "Data not found", 3);
                }
            }
            catch (Exception ex)
            {
                GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, ex.Message, 3);
            }
        }

        private void GetLotNo()
        {
            try
            {
                _plObj = new PL_LABEL_REPRINTING();
                _plObj.DbType = "GET_LOT_NO";
                _plObj.FromDate = dpFromDate.Value.ToString("yyyy-MM-dd");
                _plObj.ToDate = dpToDate.Value.ToString("yyyy-MM-dd");
                _plObj.PartNo = cmbPartNo.Text.Trim();
                cmbLotNo.Items.Clear();
                cmbLotNo.Items.Add("--Select--");
                DataTable dt = _blObj.BL_ExecuteTask(_plObj);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        cmbLotNo.Items.Add(row["Lot_No"].ToString());
                    }

                    cmbLotNo.SelectedIndex = 0;
                }
                else
                {
                    GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, "Data not found", 3);
                }
            }
            catch (Exception ex)
            {
                GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, ex.Message, 3);
            }
        }
        private void GetSerialNo()
        {
            try
            {
                _plObj = new PL_LABEL_REPRINTING();
                _plObj.DbType = "GET_SERIAL";
                _plObj.FromDate = dpFromDate.Value.ToString("yyyy-MM-dd");
                _plObj.ToDate = dpToDate.Value.ToString("yyyy-MM-dd");
                _plObj.PartNo = cmbPartNo.Text.Trim();
                _plObj.LotNo = cmbLotNo.Text.Trim();
                cmbSerialNo.Items.Clear();
                cmbSerialNo.Items.Add("--Select--");
                DataTable dt = _blObj.BL_ExecuteTask(_plObj);
                if (dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        cmbSerialNo.Items.Add(row["Serial_No"].ToString());
                    }

                    cmbSerialNo.SelectedIndex = 0;
                }
                else
                {
                    GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, "Data not found", 3);
                }
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



        private bool ValidateInput()
        {
            try
            {
                //if (dpFromDate.Value > dpToDate.Value)
                //{          
                //    GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, "From date can not be greater than To date!!", 3);
                //    dpToDate.Focus();
                //    return false;
                //}
                //if (cmbPartNo.SelectedIndex <= 0)
                //{
                //    GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, "Part No. can't be blank!!", 3);
                //    cmbPartNo.Focus();
                //    cmbPartNo.SelectAll();
                //    return false;
                //}
                //if (cmbLotNo.SelectedIndex <= 0)
                //{
                //    GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, "Lot No. can't be blank!!", 3);
                //    cmbLotNo.Focus();
                //    cmbLotNo.SelectAll();
                //    return false;
                //}
                //if (cmbSerialNo.SelectedIndex <= 0)
                //{
                //    GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, "Serial No. can't be blank!!", 3);
                //    cmbSerialNo.Focus();
                //    cmbSerialNo.SelectAll();
                //    return false;
                //}
                if (GlobalVariable.mRevNo == "" || GlobalVariable.mRevName == "" || GlobalVariable.mRevDate == "")
                {
                    GlobalVariable.mStoCustomFunction.setMessageBox(GlobalVariable.mSatoApps, "Revision Details Not Found!!", 3);
                    cmbSerialNo.Focus();
                    cmbSerialNo.SelectAll();
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
        private void dpToDate_ValueChanged(object sender, EventArgs e)
        {

        }

        private void dpToDate_CloseUp(object sender, EventArgs e)
        {
            Clear();
            GetPart();
        }
        private void dpFromDate_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }
        private void dpToDate_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = true;
        }
        private void cbPartNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbPartNo.SelectedIndex > 0)
            {
                GetLotNo();
            }
        }

        private void cmbLotNo_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbLotNo.SelectedIndex > 0)
            {
                GetSerialNo();
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
        private void txtFromLabel_KeyPress(object sender, KeyPressEventArgs e)
        {
            GlobalVariable.allowOnlyNumeric(sender, e);
        }

        private void txtToLabel_KeyPress(object sender, KeyPressEventArgs e)
        {
            GlobalVariable.allowOnlyNumeric(sender, e);
        }











        #endregion


    }
}
