using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraLayout.Utils;
using GesAMFC.AMFC_Methods;
using System;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Forms;

namespace GesAMFC
{
    /// <summary>Admin Caixa Pagamentos</summary>
    /// <author>Valter Lima</author>
    /// <creation>16-06-2017(v0.0.4.1)</creation>
    /// <versions>23-03-2018(GesAMFC-v1.0.0.3)</versions>
    public partial class Admin_Caixa : DevExpress.XtraEditors.XtraForm
    {
        //public enum Library_AMFC_Methods.OperationType { UNDEFINED = -1, OPEN = 1, ADD = 2, EDIT = 3, DEL = 4 }

        public Library_AMFC_Methods LibAMFC;
        public Library_AMFC_SQL     Lib_AMFC_SQL;

        private AMFCMember          PaymentMember;
        private AMFCCashPayment     Payment;
        private Library_AMFC_Methods.OperationType PaymentOperationType;

        //private Boolean _CanEdit = false;

        #region     Form Constructor 

        /// <versions>16-06-2017(v0.0.4.1)</versions>
        public Admin_Caixa()
        {
            LibAMFC = new Library_AMFC_Methods();
            Lib_AMFC_SQL = new Library_AMFC_SQL();
            try
            {
                Payment = new AMFCCashPayment();
                PaymentMember = new AMFCMember();
                PaymentOperationType = Library_AMFC_Methods.OperationType.UNDEFINED;

                InitializeComponent();

                LayoutControlGroup_Pay_Admin.Visibility         = LayoutVisibility.Never;
                LayoutControlItem_Pay_Add.Visibility            = LayoutVisibility.Never;
                LayoutControlItem_Pay_Edit.Visibility           = LayoutVisibility.Never;
                LayoutControlItem_Pay_Del.Visibility            = LayoutVisibility.Never;
                //LayoutControlGroup_Receipt_Buttons.Visibility   = LayoutVisibility.Never;
                LayoutControlGroup_Pay_Add.Visibility           = LayoutVisibility.Never;
                LayoutControlGroup_Pay_Edit.Visibility          = LayoutVisibility.Never;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        #endregion  Form Constructor

        #region     Events

        #region     Form Events

        /// <versions>16-06-2017(v0.0.4.1)</versions>
        private void Admin_Form_Load(object sender, EventArgs e)
        {
            try
            {
                #region     Tem de ser feito aqui senão crasha -> No futuro, depois de ter o menu, comentar !!!
                this.WindowState = FormWindowState.Maximized;
                this.Update();
                #endregion

                #region     Set DateEdits Calendars
                Program.SetCalendarControl(DateEdit_Folha_Caixa);
                Program.SetCalendarControl(DateEdit_DATA);
                Program.SetCalendarControl(DatetEdit_ALTERADO);
                Program.SetCalendarControl(DateEdit_DASSOCJOIA);
                Program.SetCalendarControl(DateEdit_DASSOCQUOT);
                Program.SetCalendarControl(DateEdit_DASSOCINFR);
                Program.SetCalendarControl(DateEdit_DASSOCCEDE);
                Program.SetCalendarControl(DateEdit_DASSOCESGO);
                Program.SetCalendarControl(DateEdit_DASSOCRECO);
                Program.SetCalendarControl(DateEdit_DASSOCOUTR);
                #endregion  Set DateEdits Calendars

                #region     Set Currency Euro Edit Values
                Program.SetPayEditValues(TextEdit_VALOR_Total);
                Program.SetPayEditValues(TextEdit_JOIAVAL);
                Program.SetPayEditValues(TextEdit_QUOTASVAL);
                Program.SetPayEditValues(TextEdit_INFRAVAL);
                Program.SetPayEditValues(TextEdit_CEDENCVAL);
                Program.SetPayEditValues(TextEdit_ESGOTVAL);
                Program.SetPayEditValues(TextEdit_RECONVAL);
                Program.SetPayEditValues(TextEdit_OUTROSVAL);
                #endregion  Set Currency Euro Edit Values

                SetSingleMultipleVisibleOptions(AMFCCashPayment.PaymentTypes.MULTIPLE, Library_AMFC_Methods.PaymentOptions.MULTIPLE);
                SetSingleMultipleEnabledOptions(AMFCCashPayment.PaymentTypes.MULTIPLE, Library_AMFC_Methods.PaymentOptions.MULTIPLE);

                LibAMFC.GridConfiguration(this.Grid_Control, this.Grid_View, true, false, true, true, true);
                Load_Grid(true, true, true, true, true, -1, -1, -1, new DateTime());
                this.Grid_View.FocusedRowHandle = -1;

                Load_Details(-1);

                Load_All_Members_Payments();
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        #endregion  Form Events

        #region     Grid Events

        /// <versions>16-06-2017(v0.0.4.1)</versions>
        private void Grid_View_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                Int32 iRowHandle = e.FocusedRowHandle;

                #region     Prevent the selectionchanged event
                if (this.Grid_View.IsGroupRow(iRowHandle))
                {
                    Clear_Details(true, true);
                    Set_Editable_Details(false, false);
                    return;
                }
                #endregion  Prevent the selectionchanged event

                Int32 iMinRowHandle = 0;
                GridView_FocusedRow(iRowHandle, iMinRowHandle);
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>16-06-2017(v0.0.4.1)</versions>
        private void Grid_View_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                GridView view = (GridView)sender;
                GridHitInfo hi = view.CalcHitInfo(e.Location);

                Int32 iRowHandle = hi.RowHandle;

                #region     Prevent the selectionchanged event
                if (this.Grid_View.IsGroupRow(iRowHandle))
                {
                    Clear_Details(true, true);
                    Set_Editable_Details(false, false);
                    return;
                }
                #endregion  Prevent the selectionchanged event

                #region     Check if is a valid Data Row Handle
                if (!hi.InRowCell || iRowHandle == GridControl.InvalidRowHandle || !this.Grid_View.IsDataRow(iRowHandle) || this.Grid_View.IsGroupRow(iRowHandle))
                {
                    this.Grid_View.ClearSelection();
                    return;
                }
                #endregion  Check if is a valid Data Row Handle

                GridView_Mouse_Down(iRowHandle);
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>16-06-2017(v0.0.4.1)</versions>
        private void Grid_View_RowStyle(object sender, RowStyleEventArgs e)
        {
            try
            {
                if (e.RowHandle >= 0 && this.Grid_View.IsDataRow(e.RowHandle) && !this.Grid_View.IsGroupRow(e.RowHandle))
                {
                    if (e.RowHandle >= 0 && this.Grid_View.FocusedRowHandle >= 0 && e.RowHandle == this.Grid_View.FocusedRowHandle)
                        e.Appearance.BackColor = Program.FocusedRowBgColor;
                    else
                    {
                        //Boolean bAssociated = Convert.ToBoolean(this.Grid_View.GetRowCellValue(e.RowHandle, this.Grid_View.Columns[""]));
                        //e.Appearance.BackColor = Program.BlueRowBgColor;
                        String sState = Convert.ToString(this.Grid_View.GetRowCellValue(e.RowHandle, this.Grid_View.Columns[Lib_AMFC_SQL.Payments_Col_ESTADO]));
                        if (sState.Trim().Substring(0, 1).ToUpper() == new AMFCCashPayment().GetPaymentStateDesc(AMFCCashPayment.PaymentState.FINALIZADO).Substring(0, 1).ToUpper())
                            e.Appearance.BackColor = Program.GreenRowBgColor;
                        else
                        {
                            if (sState.Trim().Substring(0, 1).ToUpper() == new AMFCCashPayment().GetPaymentStateDesc(AMFCCashPayment.PaymentState.ABERTO).Substring(0, 1).ToUpper())
                                e.Appearance.BackColor = Program.YelloRowBgColor;
                            else
                                e.Appearance.BackColor = System.Drawing.Color.Transparent;
                        }
                    }
                }
                else
                    e.Appearance.BackColor = System.Drawing.Color.Transparent;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        #endregion  Grid Events

        #region     Action Buttons Events

        /// <versions>16-06-2017(v0.0.4.1)</versions>
        private void Button_Member_Find_Click(object sender, EventArgs e)
        {
            Find_Member();
        }

        /// <versions>29-10-2017(GesAMFC-v0.0.4.29)</versions>
        private void Button_Load_All_Members_Click(object sender, EventArgs e)
        {
            Load_All_Members_Payments();
        }

        /// <versions>29-10-2017(GesAMFC-v0.0.4.29)</versions>
        private void Load_All_Members_Payments()
        {
            try
            {
                PaymentMember = new AMFCMember();
                Load_Grid(false, true, true, false, true, -1, -1, -1, new DateTime());
                this.Grid_View.FocusedRowHandle = -1;
                Load_Details(-1);
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>29-10-2017(GesAMFC-v0.0.4.29)</versions>
        private void Button_Pay_Add_Click(object sender, EventArgs e)
        {
            try
            {
                PaymentOperationType = Library_AMFC_Methods.OperationType.ADD;
                Int64 lMemberNumber = -1;
                if (PaymentMember != null && PaymentMember.NUMERO > 0 && PaymentMember.NUMERO < PaymentMember.MaxNumber)
                    lMemberNumber = PaymentMember.NUMERO;
                GetMemberToAddPay(lMemberNumber);
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, false, false);
            }
}

        /// <versions>29-10-2017(GesAMFC-v0.0.4.29)</versions>
        private void Button_Pay_Add_Save_Click(object sender, EventArgs e)
        {
            Button_Add_Save_Click();
        }

        /// <versions>29-10-2017(GesAMFC-v0.0.4.29)</versions>
        private void Button_Pay_Add_Cancel_Click(object sender, EventArgs e)
        {
            Button_Add_Cancel_Click();
        }

        /// <versions>29-10-2017(GesAMFC-v0.0.4.29)</versions>
        private void Button_Pay_Add_Repor_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.PaymentMember != null && this.PaymentMember.NUMERO > 0 && this.PaymentMember.NUMERO < this.PaymentMember.MaxNumber)
                    Button_Add_Pay_Action(this.PaymentMember);
                else
                    Clear_Details(false, false);
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, false, false);
            }
        }

        /// <versions>29-10-2017(GesAMFC-v0.0.4.29)</versions>
        private void Button_Pay_Edit_Click(object sender, EventArgs e)
        {
            PaymentOperationType = Library_AMFC_Methods.OperationType.EDIT;
            Button_Edit_Pay_Action();
        }

        /// <versions>29-10-2017(GesAMFC-v0.0.4.29)</versions>
        private void Button_Pay_Edit_Save_Click(object sender, EventArgs e)
        {
            Button_Edit_Save_Click();
        }

        /// <versions>29-10-2017(GesAMFC-v0.0.4.29)</versions>
        private void Button_Pay_Edit_Cancel_Click(object sender, EventArgs e)
        {
            Button_Edit_Cancel_Click();
        }

        /// <versions>29-10-2017(GesAMFC-v0.0.4.29)</versions>
        private void Button_Pay_Edit_Repor_Click(object sender, EventArgs e)
        {
            AMFCCashPayment objPayment = GetPayDetailsToAdd(this.PaymentMember);
            if (objPayment == null || objPayment.SOCIO < 1 || objPayment.SOCIO > new AMFCMember().MaxNumber || objPayment.ID < 1)
            {
                Clear_Details(true, true);
                XtraMessageBox.Show("Não foi possível obter o " + "Pagamento" + " selecionado!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Load_Grid(false, false, false, false, true, -1, -1, -1, new DateTime());
                return;
            }
            Reload_Pay_Details(objPayment, Library_AMFC_Methods.OperationType.EDIT);
        }

        /// <versions>29-10-2017(GesAMFC-v0.0.4.29)</versions>
        private void Button_Pay_Del_Click(object sender, EventArgs e)
        {
            PaymentOperationType = Library_AMFC_Methods.OperationType.DEL;
            Button_Del_Click();
        }

        /// <versions>26-11-2017(GesAMFC-v0.0.4.37)</versions>
        public void Button_PayClose_Click(object sender, EventArgs e)
        {
            try
            {
                AMFCCashPayment objPayment = GetCurrentPayDetails();
                this.PaymentOperationType = Library_AMFC_Methods.OperationType.EDIT;
                Button_Pay_Close(objPayment);
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return;
            }
        }

        #endregion  Action Buttons Events

        #region     Date Edits

        /// <versions>12-11-2017(GesAMFC-v0.0.4.32)</versions>
        private void DateEdit_DASSOCJOIA_Click(object sender, EventArgs e)
        {
            try
            {
                if (DateEdit_DASSOCJOIA.Properties.ReadOnly)
                    return;
                Set_Date_JOIA();
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, false, false);
            }
        }

        /// <versions>12-11-2017(GesAMFC-v0.0.4.32)</versions>
        private void DateEdit_DASSOCJOIA_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (DateEdit_DASSOCJOIA.Properties.ReadOnly)
                    return;
                if (!Program.IsValidDateTime(DateEdit_DASSOCJOIA.DateTime))
                    return;
                if (Program.IsValidCurrencyEuroValue(TextEdit_JOIAVAL.Text.Trim()))
                {
                    Set_Desc_JOIA();
                    Set_Date_JOIA();
                    AMFCCashPayment objMemberPay = GetPayDetailsToEdit();
                    Set_Value_JOIA(objMemberPay, true, false);
                }
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, false, false);
            }
        }

        /// <versions>21-11-2017(GesAMFC-v0.0.4.35)</versions>
        private void DateEdit_DATA_QueryPopUp(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DateEdit dateEdit = sender as DateEdit;
            Program.DateEdit_QueryPopUp_Event(dateEdit, e);
        }

        /// <versions>21-11-2017(GesAMFC-v0.0.4.35)</versions>
        private void DatetEdit_ALTERADO_QueryPopUp(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DateEdit dateEdit = sender as DateEdit;
            Program.DateEdit_QueryPopUp_Event(dateEdit, e);
        }

        /// <versions>21-11-2017(GesAMFC-v0.0.4.35)</versions>
        private void DateEdit_DASSOCJOIA_QueryPopUp(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DateEdit dateEdit = sender as DateEdit;
            Program.DateEdit_QueryPopUp_Event(dateEdit, e);
        }

        /// <versions>21-11-2017(GesAMFC-v0.0.4.35)</versions>
        private void DateEdit_DASSOCQUOT_QueryPopUp(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DateEdit dateEdit = sender as DateEdit;
            Program.DateEdit_QueryPopUp_Event(dateEdit, e);
        }

        /// <versions>21-11-2017(GesAMFC-v0.0.4.35)</versions>
        private void DateEdit_DASSOCINFR_QueryPopUp(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DateEdit dateEdit = sender as DateEdit;
            Program.DateEdit_QueryPopUp_Event(dateEdit, e);
        }

        /// <versions>21-11-2017(GesAMFC-v0.0.4.35)</versions>
        private void DateEdit_DASSOCCEDE_QueryPopUp(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DateEdit dateEdit = sender as DateEdit;
            Program.DateEdit_QueryPopUp_Event(dateEdit, e);
        }

        /// <versions>21-11-2017(GesAMFC-v0.0.4.35)</versions>
        private void DateEdit_DASSOCESGO_QueryPopUp(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DateEdit dateEdit = sender as DateEdit;
            Program.DateEdit_QueryPopUp_Event(dateEdit, e);
        }

        /// <versions>21-11-2017(GesAMFC-v0.0.4.35)</versions>
        private void DateEdit_DASSOCRECO_QueryPopUp(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DateEdit dateEdit = sender as DateEdit;
            Program.DateEdit_QueryPopUp_Event(dateEdit, e);
        }

        /// <versions>21-11-2017(GesAMFC-v0.0.4.35)</versions>
        private void DateEdit_DASSOCOUTR_QueryPopUp(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DateEdit dateEdit = sender as DateEdit;
            Program.DateEdit_QueryPopUp_Event(dateEdit, e);
        }

        #endregion  Date Edits

        #region     Text Box Edits

        /// <versions>29-10-2017(GesAMFC-v0.0.4.29)</versions>
        private void TextEdit_PayValue_Click(object sender, EventArgs e)
        {
            if (TextEdit_VALOR_Total.Properties.ReadOnly)
                Pay_Edit_Action();
        }

        /// <versions>12-11-2017(GesAMFC-v0.0.4.32)</versions>
        private void TextEdit_JOIADESC_Click(object sender, EventArgs e)
        {
            try
            {
                if (TextEdit_JOIADESC.Properties.ReadOnly)
                    return;
                Set_Desc_JOIA();
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, false, false);
            }
        }

        /// <versions>12-11-2017(GesAMFC-v0.0.4.32)</versions>
        private void TextEdit_JOIAVAL_Click(object sender, EventArgs e)
        {
            try
            {
                if (TextEdit_JOIAVAL.Properties.ReadOnly)
                    return;
                AMFCCashPayment objMemberPay = GetPayDetailsToEdit();
                Set_Value_JOIA(objMemberPay, true, false);
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, false, false);
            }
        }

        /// <versions>12-11-2017(GesAMFC-v0.0.4.32)</versions>
        private void TextEdit_JOIADESC_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (TextEdit_JOIADESC.Properties.ReadOnly)
                    return;
                if (Program.IsValidCurrencyEuroValue(TextEdit_JOIAVAL.Text.Trim()))
                {
                    Set_Desc_JOIA();
                    Set_Date_JOIA();
                    AMFCCashPayment objMemberPay = GetPayDetailsToEdit();
                    Set_Value_JOIA(objMemberPay, true, false);
                }
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, false, false);
            }
        }

        /// <versions>12-11-2017(GesAMFC-v0.0.4.32)</versions>
        private void TextEdit_JOIAVAL_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if (DateEdit_DASSOCJOIA.Properties.ReadOnly)
                    return;
                if (Program.IsValidCurrencyEuroValue(TextEdit_JOIAVAL.Text.Trim()))
                {
                    Set_Desc_JOIA();
                    Set_Date_JOIA();
                    AMFCCashPayment objMemberPay = GetPayDetailsToEdit();
                    Set_Value_JOIA(objMemberPay, true, false);
                }
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, false, false);
            }
        }

        #endregion  Text Box Edits

        #endregion  Events

        #region     Methods

        /// <versions>17-06-2017(v0.0.4.1)</versions>
        private void Find_Member()
        {
            try
            {
                Find_Member_Form objFindMemberForm = new Find_Member_Form();
                objFindMemberForm.FormClosing += delegate
                {
                    if (Program.Member_Found)
                    {
                        PaymentMember = objFindMemberForm.MemberSelected;
                        if (PaymentMember != null && PaymentMember.NUMERO > 0 && PaymentMember.NUMERO < PaymentMember.MaxNumber)
                            Load_Grid(false, false, false, false, true, -1, -1, PaymentMember.NUMERO, new DateTime());
                    }
                    else
                        this.Grid_View.FocusedRowHandle = -1;
                };
                objFindMemberForm.Show();
                objFindMemberForm.StartPosition = FormStartPosition.CenterParent;
                objFindMemberForm.Focus();
                objFindMemberForm.BringToFront();
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return;
            }
        }

        /// <versions>26-11-2017(GesAMFC-v0.0.4.37)</versions>
        private void GetMemberToAddPay(Int64 lMemberNumber)
        {
            try
            {
                AMFCMember objMember = new AMFCMember();

                Find_Member_Form objFindMemberForm = new Find_Member_Form();
                objFindMemberForm.MemberNumber = lMemberNumber;
                objFindMemberForm.FormClosing += delegate
                {
                    if (Program.Member_Found)
                    {
                        AMFCMember objMemberSelected = objFindMemberForm.MemberSelected;
                        if (objMemberSelected != null && objMemberSelected.NUMERO > 0 && objMemberSelected.NUMERO < objMemberSelected.MaxNumber)
                        {
                            PaymentMember = objMemberSelected;
                            Button_Add_Pay_Action(objMemberSelected);
                        }
                    }
                    else
                        this.Grid_View.FocusedRowHandle = -1;
                };
                objFindMemberForm.Show();
                objFindMemberForm.StartPosition = FormStartPosition.CenterParent;
                objFindMemberForm.Focus();
                objFindMemberForm.BringToFront();
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return;
            }
        }

        #region     Grid Methods

        /// <versions>17-06-2017(v0.0.4.1)</versions>
        private AMFCCashPayment Get_Selected(Int32 iRowHandle, Int64 lItemId)
        {
            try
            {
                AMFCCashPayment objPayment = null;
                Int32 iFocusedRowHandle = -1;
                Int64 lFocusedItemId = -1;

                if (lItemId < 1)
                {
                    if (iRowHandle > -1)
                        iFocusedRowHandle = iRowHandle;
                    else
                        iFocusedRowHandle = this.Grid_View.FocusedRowHandle;
                    if (iFocusedRowHandle < 0 || iFocusedRowHandle == GridControl.InvalidRowHandle || !this.Grid_View.IsDataRow(iFocusedRowHandle))
                    {
                        XtraMessageBox.Show("Não foi possível obter os detalhes do pagamento selecionado!", "Erro [Pagamento Selecionado]", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return null;
                    }
                    lFocusedItemId = Convert.ToInt64(this.Grid_View.GetRowCellValue(iFocusedRowHandle, this.Grid_View.Columns[Lib_AMFC_SQL.Payments_DB_Col_ID]));
                }
                else
                    lFocusedItemId = lItemId;

                if (lFocusedItemId == this.Payment.ID)
                    return this.Payment;
                objPayment = Lib_AMFC_SQL.Get_Cash_Payment_By_Id(lFocusedItemId);

                if (objPayment == null || objPayment.ID < 1 || objPayment.SOCIO < 1 || String.IsNullOrEmpty(objPayment.NOME))
                {
                    XtraMessageBox.Show("Não foi possível obter os detalhes do pagamento selecionado!", "Erro [Pagamento Selecionado]", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
                this.Payment = objPayment;
                return objPayment;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return null;
            }
        }

        /// <versions>16-06-2017(v0.0.4.1)</versions>
        private Boolean IsTheSameSelected(Int32 iRowHandle, Int64 lItemId)
        {
            try
            {
                Int32 iFocusedRowHandle = -1;
                Int64 lFocusedItemId = -1;

                if (lItemId < 1)
                {
                    if (iRowHandle > -1)
                        iFocusedRowHandle = iRowHandle;
                    else
                        iFocusedRowHandle = this.Grid_View.FocusedRowHandle;
                    if (iFocusedRowHandle < 0 || iFocusedRowHandle == GridControl.InvalidRowHandle || !this.Grid_View.IsDataRow(iFocusedRowHandle))
                    {
                        XtraMessageBox.Show("Não foi possível obter os detalhes do pagamento selecionado!", "Erro [Pagamento Selecionado]", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    lFocusedItemId = Convert.ToInt64(this.Grid_View.GetRowCellValue(iFocusedRowHandle, this.Grid_View.Columns[Lib_AMFC_SQL.Payments_DB_Col_ID]));
                }
                else
                    lFocusedItemId = lItemId;
                if (lFocusedItemId == this.Payment.ID)
                    return true;

                return false;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return false;
            }
        }

        /// <versions>17-06-2017(v0.0.4.1)</versions>
        private void Load_Grid(Boolean bSetCols, Boolean bClearSorting, Boolean bClearFilters, Boolean bClearGrouping, Boolean bForceDetailsDataLoad, Int32 iFocusedRowHandle, Int64 lPaymentId, Int64 lMemberNumber, DateTime dtDate)
        {
            try
            {
                this.Grid_View.ClearSelection();

                Load_Grid_Datasource(bSetCols, bClearSorting, bClearFilters, bClearGrouping, true, lMemberNumber, dtDate);

                #region     Config Grids Options
                this.Grid_View.OptionsSelection.EnableAppearanceFocusedRow = true;
                this.Grid_View.OptionsSelection.EnableAppearanceFocusedCell = false;
                this.Grid_View.OptionsSelection.EnableAppearanceHideSelection = true;
                this.Grid_View.OptionsSelection.UseIndicatorForSelection = false;
                this.Grid_View.ClearSelection();
                #endregion  Config Grids Options

                #region     Set Focused Member
                if (lPaymentId > 0)
                {
                    Int32 iRowHandle = this.Grid_View.LocateByValue(Lib_AMFC_SQL.Payments_DB_Col_ID, lPaymentId);
                    if (iRowHandle != DevExpress.XtraGrid.GridControl.InvalidRowHandle)
                        this.Grid_View.FocusedRowHandle = iRowHandle;
                }
                else if (iFocusedRowHandle > -1)
                {
                    if (iFocusedRowHandle != DevExpress.XtraGrid.GridControl.InvalidRowHandle)
                        this.Grid_View.FocusedRowHandle = iFocusedRowHandle;
                }
                #endregion  Set Focused Member

                return;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>17-06-2017(v0.0.4.1)</versions>
        private void Load_Grid_Datasource(Boolean bSetCols, Boolean bClearSorting, Boolean bClearFilters, Boolean bClearGrouping, Boolean bWriteLog, Int64 lMemberNumber, DateTime dtDate)
        {
            try
            {
                LibAMFC.CleanGrid(this.Grid_Control, this.Grid_View, bSetCols, bClearSorting, bClearFilters, bClearGrouping);
                this.Grid_Control.Visible = false;
                this.Update();

                Boolean bLoadDatasource = Set_Grid_Data_Source(this.Grid_Control, lMemberNumber, dtDate);

                if (bLoadDatasource)
                {
                    if (bSetCols)
                    {
                        Set_Grid_Columns();
                        Set_Grid_Cols_Totals_Summaries();
                    }
                    Set_Grid_Global_Totals_Summaries();
                }

                this.Grid_Control.Visible = true;
                this.Update();
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>28-10-2017(v0.0.4.28)</versions>
        private Boolean Set_Grid_Data_Source(GridControl objGridControl, Int64 lMemberNumber, DateTime dtDate)
        {
            StackFrame objStackFrame = new StackFrame();
            String sErrorMsg = String.Empty;
            try
            {
                #region         Members Datasource
                AMFCCashPayments objAMFCCashPayments = new AMFCCashPayments();
                if (lMemberNumber < 1 || lMemberNumber > new AMFCMember().MaxNumber)
                    objAMFCCashPayments = Lib_AMFC_SQL.Get_All_Cash_Payments(dtDate);
                else
                    objAMFCCashPayments = Lib_AMFC_SQL.Get_Cash_Payments_By_MemberNumber(lMemberNumber, dtDate);
                if (objAMFCCashPayments == null)
                {
                    sErrorMsg = "Não foi possível obter a Lista de Pagamentos relativamente ao Sócio com o Nº: " + lMemberNumber;
                    Program.HandleError(objStackFrame.GetMethod().Name, sErrorMsg, Program.ErroType.ERROR, true, false);
                    return false;
                }
                if (objAMFCCashPayments.Payments.Count == 0)
                {
                    String sWarningMsg = "Não existem registos na Caixa de Pagamentos!";
                    MessageBox.Show(sWarningMsg, "Sem Pagamentos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    objAMFCCashPayments = Lib_AMFC_SQL.Get_All_Cash_Payments(new DateTime());
                    if (objAMFCCashPayments.Payments.Count == 0)
                        return false;
                }
                #endregion  Members Datasource

                #region     TreeDataSet
                DataTable objDataTableSource = new DataTable("AMFC_Cash_Payments");
                #region     Data Columns Creation
                DataColumn objDataColumn_ID             = new DataColumn(Lib_AMFC_SQL.Payments_Col_ID,              typeof(Int64));
                DataColumn objDataColumn_SOCIO          = new DataColumn(Lib_AMFC_SQL.Payments_Col_SOCIO,           typeof(Int64));
                DataColumn objDataColumn_NOME           = new DataColumn(Lib_AMFC_SQL.Payments_Col_NOME,            typeof(String));
                DataColumn objDataColumn_DESIGNACAO     = new DataColumn(Lib_AMFC_SQL.Payments_Col_DESIGNACAO,      typeof(String));
                DataColumn objDataColumn_DATA_Str       = new DataColumn(Lib_AMFC_SQL.Payments_Col_DATA_Str,        typeof(String));
                DataColumn objDataColumn_ALTERADO_Str   = new DataColumn(Lib_AMFC_SQL.Payments_Col_ALTERADO_Str,    typeof(String));
                DataColumn objDataColumn_DATAYear       = new DataColumn(Lib_AMFC_SQL.Payments_Col_DATAYear,        typeof(String));
                DataColumn objDataColumn_DATAMonth      = new DataColumn(Lib_AMFC_SQL.Payments_Col_DATAMonth,       typeof(String));
                DataColumn objDataColumn_VALOR          = new DataColumn(Lib_AMFC_SQL.Payments_Col_VALOR,           typeof(Double));
                //DataColumn objDataColumn_DATAMonthYear  = new DataColumn(Lib_AMFC_SQL.Payments_Col_DATAMonthYear,   typeof(String));
                DataColumn objDataColumn_NOME_PAG       = new DataColumn(Lib_AMFC_SQL.Payments_Col_NOME_PAG,        typeof(String));
                DataColumn objDataColumn_NOTAS          = new DataColumn(Lib_AMFC_SQL.Payments_Col_NOTAS,           typeof(String));
                DataColumn objDataColumn_LISTARECNU   = new DataColumn(Lib_AMFC_SQL.Payments_Col_LISTARECNU,        typeof(String));
                DataColumn objDataColumn_ESTADO = new DataColumn(Lib_AMFC_SQL.Payments_Col_ESTADO, typeof(String));

                #endregion  Data Columns Creation
                #region     Data Table Add Columns
                objDataTableSource.Columns.AddRange(
                                            new DataColumn[] {
                                                objDataColumn_ID,
                                                objDataColumn_SOCIO,
                                                objDataColumn_NOME,
                                                objDataColumn_DESIGNACAO,
                                                objDataColumn_DATA_Str,
                                                objDataColumn_ALTERADO_Str,
                                                objDataColumn_DATAYear,
                                                objDataColumn_DATAMonth,
                                                //objDataColumn_DATAMonthYear,
                                                objDataColumn_VALOR,
                                                objDataColumn_NOME_PAG,
                                                objDataColumn_NOTAS,
                                                objDataColumn_LISTARECNU,
                                                objDataColumn_ESTADO
                                            }
                                        );
                #endregion  Data Table Add Columns
                #region     Data Columns Dispose
                objDataColumn_ID.Dispose();
                objDataColumn_SOCIO.Dispose();
                objDataColumn_NOME.Dispose();
                objDataColumn_DESIGNACAO.Dispose();
                objDataColumn_DATA_Str.Dispose();
                objDataColumn_ALTERADO_Str.Dispose();
                objDataColumn_DATAYear.Dispose();
                objDataColumn_DATAMonth.Dispose();
                //objDataColumn_DATAMonthYear.Dispose();
                objDataColumn_VALOR.Dispose();
                objDataColumn_NOME_PAG.Dispose();
                objDataColumn_NOTAS.Dispose();
                objDataColumn_LISTARECNU.Dispose();
                objDataColumn_ESTADO.Dispose();
                #endregion  Data Columns Dispose
                #endregion  TreeDataSet

                #region     Binding Data
                foreach (AMFCCashPayment objPayment in objAMFCCashPayments.Payments)
                {
                    if (objPayment == null || objPayment.ID < 1 || objPayment.SOCIO < 1 || objPayment.SOCIO > new AMFCMember().MaxNumber || objPayment.VALOR < 0)
                        continue;
                    #region     Set Member Row Data
                    DataRow objDataRow = objDataTableSource.NewRow();
                    objDataRow[Lib_AMFC_SQL.Payments_Col_ID]                = objPayment.ID;
                    objDataRow[Lib_AMFC_SQL.Payments_Col_SOCIO]             = objPayment.SOCIO;
                    objDataRow[Lib_AMFC_SQL.Payments_Col_NOME]              = objPayment.NOME;
                    objDataRow[Lib_AMFC_SQL.Payments_Col_DESIGNACAO]        = objPayment.DESIGNACAO;
                    objDataRow[Lib_AMFC_SQL.Payments_Col_DATA_Str]          = objPayment.DATA_Str;
                    objDataRow[Lib_AMFC_SQL.Payments_Col_ALTERADO_Str]      = objPayment.ALTERADO_Str;
                    objDataRow[Lib_AMFC_SQL.Payments_Col_DATAYear]          = objPayment.DATAYear;
                    objDataRow[Lib_AMFC_SQL.Payments_Col_DATAMonth]         = objPayment.DATAMonth;
                    //objDataRow[Lib_AMFC_SQL.Payments_Col_DATAMonthYear]     = objPayment.DATAMonthYear;
                    objDataRow[Lib_AMFC_SQL.Payments_Col_VALOR]             = objPayment.VALOR; ;
                    objDataRow[Lib_AMFC_SQL.Payments_Col_NOME_PAG]          = objPayment.NOME_PAG;
                    objDataRow[Lib_AMFC_SQL.Payments_Col_NOTAS]             = objPayment.NOTAS;
                    objDataRow[Lib_AMFC_SQL.Payments_Col_LISTARECNU]        = objPayment.LISTARECNU;
                    objDataRow[Lib_AMFC_SQL.Payments_Col_ESTADO] = objPayment.Payment_State;
                    #endregion  Set Member Row Data
                    objDataTableSource.Rows.Add(objDataRow);
                }
                objGridControl.DataSource = objDataTableSource;
                return true;
                #endregion  Binding Data
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return false;
            }
            finally { objStackFrame = null; }
        }

        /// <versions>16-06-2017(v0.0.4.1)</versions>
        public void Set_Grid_Columns()
        {
            try
            {
                Set_Grid_Columns_Editability();
                Set_Grid_Columns_OptionsFilter();
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>09-07-2017(v0.0.4.3)</versions>
        public void Set_Grid_Cols_Totals_Summaries()
        {
            try
            {
                this.Grid_View.GroupFooterShowMode = GroupFooterShowMode.VisibleAlways;

                GridGroupSummaryItem objGroupTotal = new GridGroupSummaryItem();
                objGroupTotal.SummaryType = DevExpress.Data.SummaryItemType.Count;
                objGroupTotal.FieldName = Lib_AMFC_SQL.Payments_Col_VALOR;
                this.Grid_View.GroupSummary.Add(objGroupTotal);

                GridGroupSummaryItem objGroupTotalValues = new GridGroupSummaryItem();
                objGroupTotalValues.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                objGroupTotalValues.FieldName = Lib_AMFC_SQL.Payments_Col_VALOR;
                objGroupTotalValues.DisplayFormat = "Total = " + Program.FormatString_Double3_Euro;
                objGroupTotalValues.ShowInGroupColumnFooter = this.Grid_View.Columns[Lib_AMFC_SQL.Payments_Col_VALOR];
                this.Grid_View.GroupSummary.Add(objGroupTotalValues);

                this.Grid_View.OptionsView.ShowFooter = true;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>09-07-2017(v0.0.4.3)</versions>
        public void Set_Grid_Global_Totals_Summaries()
        {
            try
            {
                this.Grid_View.GroupFooterShowMode = GroupFooterShowMode.VisibleAlways;

                this.Grid_View.OptionsView.ShowFooter = true;

                this.Grid_View.Columns[Lib_AMFC_SQL.Payments_Col_VALOR].Summary.Clear();

                GridColumnSummaryItem objTotalGlobalValues = new GridColumnSummaryItem();
                objTotalGlobalValues.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                objTotalGlobalValues.FieldName = Lib_AMFC_SQL.Payments_Col_VALOR;
                objTotalGlobalValues.DisplayFormat = "Total Global = " + Program.FormatString_Double3_Euro;
                this.Grid_View.Columns[Lib_AMFC_SQL.Payments_Col_VALOR].Summary.Add(objTotalGlobalValues);
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>22-10-2017(v0.0.4.22)</versions>
        private void Set_Grid_Columns_Editability()
        {
            try
            {
                LibAMFC.SetGridColumnOptions(this.Grid_Control, this.Grid_View, Lib_AMFC_SQL.Payments_Col_ID,               "Nº Pagamento",         "Id do pagamento",                 false,  -1,   80, true, false, true,     false,      HorzAlignment.Center,   VertAlignment.Center,      HorzAlignment.Far,       VertAlignment.Center);
                LibAMFC.SetGridColumnOptions(this.Grid_Control, this.Grid_View, Lib_AMFC_SQL.Payments_DB_Col_SOCIO,         "Nº Sócio",             "Número de sócio",                  true,   2,   80, true, false, true,     false,      HorzAlignment.Far,      VertAlignment.Center,      HorzAlignment.Far,       VertAlignment.Center);
                LibAMFC.SetGridColumnOptions(this.Grid_Control, this.Grid_View, Lib_AMFC_SQL.Payments_Col_NOME,             "Nome",                 "Nome do sócio",                    true,   3,  270, true, false, true,     false,      HorzAlignment.Near,     VertAlignment.Center,      HorzAlignment.Near,      VertAlignment.Center);
                LibAMFC.SetGridColumnOptions(this.Grid_Control, this.Grid_View, Lib_AMFC_SQL.Payments_Col_DESIGNACAO,       "Designação Pagamento", "Descrição do pagamento",           true,   4,  270, true, false, true,     false,      HorzAlignment.Center,   VertAlignment.Center,      HorzAlignment.Near,      VertAlignment.Center);
                LibAMFC.SetGridColumnOptions(this.Grid_Control, this.Grid_View, Lib_AMFC_SQL.Payments_Col_DATA_Str,         "Data Pagamento",       "Data de Pagamento",                true,   5,  140, true, false, true,     true,       HorzAlignment.Center,   VertAlignment.Center,      HorzAlignment.Center,    VertAlignment.Center);
                //LibAMFC.SetGridColumnOptions(this.Grid_Control, this.Grid_View, Lib_AMFC_SQL.Payments_Col_DATAMonthYear,    "Mês/Ano",              "Mês/Ano de Pagamento",             true,   6,   55, true, false, true,     false,      HorzAlignment.Center,  VertAlignment.Center,      HorzAlignment.Center,  VertAlignment.Center);
                LibAMFC.SetGridColumnOptions(this.Grid_Control, this.Grid_View, Lib_AMFC_SQL.Payments_Col_VALOR,            "Valor Total",          "Valor total do pagamento",         true,   7,  180, true, false, true,     true,       HorzAlignment.Center,   VertAlignment.Center,      HorzAlignment.Far,       VertAlignment.Center, true);
                LibAMFC.SetGridColumnOptions(this.Grid_Control, this.Grid_View, Lib_AMFC_SQL.Payments_Col_DATAYear,         "Ano",                  "Ano de Pagamento",                 true,   8,   80, true, false, true,     true, 1,    HorzAlignment.Center,   VertAlignment.Center,      HorzAlignment.Center,    VertAlignment.Center);
                LibAMFC.SetGridColumnOptions(this.Grid_Control, this.Grid_View, Lib_AMFC_SQL.Payments_Col_DATAMonth,        "Mês",                  "Mês de Pagamento",                 true,   9,  140, true, false, true,     true, 1,    HorzAlignment.Center,   VertAlignment.Center,      HorzAlignment.Center,    VertAlignment.Center);
                LibAMFC.SetGridColumnOptions(this.Grid_Control, this.Grid_View, Lib_AMFC_SQL.Payments_Col_ESTADO,           "Estado",               "Estado do Pagamento",              true,  10,   90, true, false, true,     true,       HorzAlignment.Center,   VertAlignment.Center,       HorzAlignment.Center,   VertAlignment.Center);
                LibAMFC.SetGridColumnOptions(this.Grid_Control, this.Grid_View, Lib_AMFC_SQL.Payments_Col_NOME_PAG,         "Pagou",                "Pagamento realizado por",          false, -1,  280, true, false, true,     false,      HorzAlignment.Center,   VertAlignment.Center,      HorzAlignment.Center,    VertAlignment.Center);
                LibAMFC.SetGridColumnOptions(this.Grid_Control, this.Grid_View, Lib_AMFC_SQL.Payments_Col_NOTAS,            "Notas",                "Notas",                            false, -1,  100, true, false, true,     false,      HorzAlignment.Center,   VertAlignment.Center,      HorzAlignment.Center,    VertAlignment.Center);
                LibAMFC.SetGridColumnOptions(this.Grid_Control, this.Grid_View, Lib_AMFC_SQL.Payments_Col_LISTARECNU,       "Recibos",              "Lista de recibos associados",      false, -1,  100, true, false, true,     false,      HorzAlignment.Center,   VertAlignment.Center,      HorzAlignment.Center,    VertAlignment.Center);
                LibAMFC.SetGridColumnOptions(this.Grid_Control, this.Grid_View, Lib_AMFC_SQL.Payments_Col_ALTERADO_Str,     "Data Alterado",        "Data de Alteração do Pagamento",   false, -1,   90, true, false, true,     true,       HorzAlignment.Center,   VertAlignment.Center,      HorzAlignment.Center,    VertAlignment.Center);
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>22-10-2017(v0.0.4.22)</versions>
        private void Set_Grid_Columns_OptionsFilter()
        {
            try
            {
                LibAMFC.SetGridColumOptionsFilter(this.Grid_View, Lib_AMFC_SQL.Payments_Col_ID,             true, true, AutoFilterCondition.Equals,     8.0f);
                LibAMFC.SetGridColumOptionsFilter(this.Grid_View, Lib_AMFC_SQL.Payments_DB_Col_SOCIO,       true, true, AutoFilterCondition.Equals,     8.0f);
                LibAMFC.SetGridColumOptionsFilter(this.Grid_View, Lib_AMFC_SQL.Payments_Col_DESIGNACAO,     true, true, AutoFilterCondition.Contains,   9.0f);
                LibAMFC.SetGridColumOptionsFilter(this.Grid_View, Lib_AMFC_SQL.Payments_Col_NOME,           true, true, AutoFilterCondition.Contains,   8.5f);
                LibAMFC.SetGridColumOptionsFilter(this.Grid_View, Lib_AMFC_SQL.Payments_Col_VALOR,          true, true, AutoFilterCondition.Contains,   8.5f);
                LibAMFC.SetGridColumOptionsFilter(this.Grid_View, Lib_AMFC_SQL.Payments_Col_DATA_Str,       true, true, AutoFilterCondition.Contains,   8.5f);
                LibAMFC.SetGridColumOptionsFilter(this.Grid_View, Lib_AMFC_SQL.Payments_Col_DATAYear,       true, true, AutoFilterCondition.Equals,     8.5f);
                LibAMFC.SetGridColumOptionsFilter(this.Grid_View, Lib_AMFC_SQL.Payments_Col_DATAMonth,      true, true, AutoFilterCondition.Equals,     8.5f);
                LibAMFC.SetGridColumOptionsFilter(this.Grid_View, Lib_AMFC_SQL.Payments_Col_DATAMonthYear,  true, true, AutoFilterCondition.Equals,     8.5f);
                LibAMFC.SetGridColumOptionsFilter(this.Grid_View, Lib_AMFC_SQL.Payments_Col_NOME_PAG,       true, true, AutoFilterCondition.Contains,   8.5f);
                LibAMFC.SetGridColumOptionsFilter(this.Grid_View, Lib_AMFC_SQL.Payments_Col_NOTAS,          true, true, AutoFilterCondition.Contains,   8.5f);
                LibAMFC.SetGridColumOptionsFilter(this.Grid_View, Lib_AMFC_SQL.Payments_Col_ESTADO,         true, true, AutoFilterCondition.Contains,   8.5f);
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>16-06-2017(v0.0.4.1)</versions>
        private Boolean GridView_FocusedRow(Int32 iRowHandle)
        {
            return Get_Grid_Focused_Row(iRowHandle, 0);
        }

        /// <versions>16-06-2017(v0.0.4.1)</versions>
        private Boolean GridView_Mouse_Down(Int32 iRowHandle)
        {
            try
            {
                StackFrame objStackFrame = new StackFrame();
                try
                {
                    if (iRowHandle < 0)
                        return false;
                    return GridView_FocusedRow(iRowHandle);
                }
                catch (Exception ex) { Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false); }
                finally { objStackFrame = null; }
                return false;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return false;
            }
        }

        /// <versions>16-06-2017(v0.0.4.1)</versions>
        private Boolean GridView_FocusedRow(Int32 iRowHandle, Int32 iMinRowHandle)
        {
            return Get_Grid_Focused_Row(iRowHandle, iMinRowHandle);
        }

        /// <versions>16-06-2017(v0.0.4.1)</versions>
        private Boolean Get_Grid_Focused_Row(Int32 iRowHandle, Int32 iMinRowHandle)
        {
            try
            {
                StackFrame objStackFrame = new StackFrame();

                LayoutControlGroup_Pay_Admin.Visibility         = LayoutVisibility.Never;
                LayoutControlItem_Pay_Add.Visibility            = LayoutVisibility.Never;
                LayoutControlItem_Pay_Edit.Visibility           = LayoutVisibility.Never;
                LayoutControlItem_Pay_Del.Visibility            = LayoutVisibility.Never;
                //LayoutControlGroup_Receipt_Buttons.Visibility   = LayoutVisibility.Never;
                LayoutControlGroup_Pay_Add.Visibility           = LayoutVisibility.Never;
                LayoutControlGroup_Pay_Edit.Visibility          = LayoutVisibility.Never;
                

                #region     Check if is a valid Data Row Handle
                if (iRowHandle < iMinRowHandle || iRowHandle == GridControl.InvalidRowHandle || !this.Grid_View.IsDataRow(iRowHandle))
                {
                    Clear_Details(true, true);
                    Set_Editable_Details(false, false);
                    return false;
                }
                #endregion  Check if is a valid Data Row Handle

                #region     Get Focused Item
                Boolean bIsTheSameMember = IsTheSameSelected(iRowHandle, -1);
                AMFCCashPayment objPayment = null;
                if (!bIsTheSameMember)
                    objPayment = Get_Selected(iRowHandle, -1);
                else
                    objPayment = this.Payment;
                if (objPayment == null || objPayment.ID < 1 || objPayment.SOCIO < 1)
                {
                    XtraMessageBox.Show("Erro", objStackFrame.GetMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                #endregion  Get Focused Item

                #region     Load Member Item Details
                if (!bIsTheSameMember)
                    Load_Payment_Details(objPayment);
                #endregion  Load Member Item Details

                LayoutControlGroup_Pay_Admin.Visibility         = LayoutVisibility.Never;
                LayoutControlItem_Pay_Add.Visibility            = LayoutVisibility.Never; 
                LayoutControlItem_Pay_Edit.Visibility           = LayoutVisibility.Never;
                
                //LayoutControlItem_Pay_Del.Visibility          = (objPayment.Payment_State == AMFCCashPayment.PaymentState.ABERTO || objPayment.Payment_State == AMFCCashPayment.PaymentState.FINALIZADO) ? LayoutVisibility.Always : LayoutVisibility.Never;
                LayoutControlItem_Pay_Del.Visibility            = LayoutVisibility.Never;

                LayoutControlGroup_Pay_Set_Close.Visibility     = (objPayment.Payment_State == AMFCCashPayment.PaymentState.ABERTO) ? LayoutVisibility.Always : LayoutVisibility.Never;

                LayoutControlGroup_Pay_Add.Visibility           = LayoutVisibility.Never;
                LayoutControlGroup_Pay_Edit.Visibility          = LayoutVisibility.Never;

                //LayoutControlGroup_Receipt_Buttons.Visibility = (objPayment.Payment_State == AMFCCashPayment.PaymentState.FINALIZADO) ? LayoutVisibility.Always : LayoutVisibility.Never;
                LayoutControlGroup_Receipt_Buttons.Visibility = LayoutVisibility.Always; //debug

                return true;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return false;
            }
        }

        #endregion  Grid Methods

        #region     Payment Entities Visibility

        /// <versions>26-11-2017(GesAMFC-v0.0.4.37)</versions>
        public Library_AMFC_Methods.PaymentOptions Get_Payment_Type(AMFCCashPayment objPayment)
        {
            try
            {
                Library_AMFC_Methods.PaymentOptions ePayEntType = Library_AMFC_Methods.PaymentOptions.SINGLE;

                Int32 iCurrentPayEntities = 0;
                if (objPayment.JOIAVAL > 0)
                    iCurrentPayEntities++;
                if (objPayment.QUOTASVAL > 0)
                    iCurrentPayEntities++;
                if (objPayment.INFRAVAL > 0)
                    iCurrentPayEntities++;
                if (objPayment.CEDENCVAL > 0)
                    iCurrentPayEntities++;
                if (objPayment.ESGOTVAL > 0)
                    iCurrentPayEntities++;
                if (objPayment.RECONVAL > 0)
                    iCurrentPayEntities++;
                if (objPayment.OUTROSVAL > 0)
                    iCurrentPayEntities++;

                if (objPayment.EntidadeVAL > 0)
                    iCurrentPayEntities++;

                if (iCurrentPayEntities > 1)
                    ePayEntType = Library_AMFC_Methods.PaymentOptions.MULTIPLE;
                else
                    ePayEntType = Library_AMFC_Methods.PaymentOptions.SINGLE;

                return ePayEntType;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return Library_AMFC_Methods.PaymentOptions.SINGLE;
            }
        }

        /// <versions>26-11-2017(GesAMFC-v0.0.4.37)</versions>
        public void SetSingleMultipleVisibleOptions(AMFCCashPayment.PaymentTypes ePaymentType, Library_AMFC_Methods.PaymentOptions EntitiesPaymentType)
        {
            try
            {
                #region     Set Single/Multiple Visisble Options
                if (EntitiesPaymentType == Library_AMFC_Methods.PaymentOptions.SINGLE)
                {
                    switch (ePaymentType)
                    {
                        case AMFCCashPayment.PaymentTypes.JOIA:
                            SetVisibility_JOIA(true);
                            SetVisibility_QUOTAS(false);
                            SetVisibility_INFRA(false);
                            SetVisibility_CEDENC(false);
                            SetVisibility_ESGOT(false);
                            SetVisibility_RECONV(false);
                            SetVisibility_OUTROS(false);
                            SetVisibility_TOTAL(true);
                            break;
                        case AMFCCashPayment.PaymentTypes.QUOTAS:
                            SetVisibility_JOIA(false);
                            SetVisibility_QUOTAS(true);
                            SetVisibility_INFRA(false);
                            SetVisibility_CEDENC(false);
                            SetVisibility_ESGOT(false);
                            SetVisibility_RECONV(false);
                            SetVisibility_OUTROS(false);
                            SetVisibility_TOTAL(true);
                            break;
                        case AMFCCashPayment.PaymentTypes.INFRAS:
                            SetVisibility_JOIA(false);
                            SetVisibility_QUOTAS(false);
                            SetVisibility_INFRA(true);
                            SetVisibility_CEDENC(false);
                            SetVisibility_ESGOT(false);
                            SetVisibility_RECONV(false);
                            SetVisibility_OUTROS(false);
                            SetVisibility_TOTAL(true);
                            break;
                        case AMFCCashPayment.PaymentTypes.CEDENC:
                            SetVisibility_JOIA(false);
                            SetVisibility_QUOTAS(false);
                            SetVisibility_INFRA(false);
                            SetVisibility_CEDENC(true);
                            SetVisibility_ESGOT(false);
                            SetVisibility_RECONV(false);
                            SetVisibility_OUTROS(false);
                            SetVisibility_TOTAL(true);
                            break;
                        case AMFCCashPayment.PaymentTypes.ESGOT:
                            SetVisibility_JOIA(false);
                            SetVisibility_QUOTAS(false);
                            SetVisibility_INFRA(false);
                            SetVisibility_CEDENC(false);
                            SetVisibility_ESGOT(true);
                            SetVisibility_RECONV(false);
                            SetVisibility_OUTROS(false);
                            SetVisibility_TOTAL(true);
                            break;
                        case AMFCCashPayment.PaymentTypes.RECONV:
                            SetVisibility_JOIA(false);
                            SetVisibility_QUOTAS(false);
                            SetVisibility_INFRA(false);
                            SetVisibility_CEDENC(false);
                            SetVisibility_ESGOT(false);
                            SetVisibility_RECONV(true);
                            SetVisibility_OUTROS(false);
                            SetVisibility_TOTAL(true);
                            break;
                        case AMFCCashPayment.PaymentTypes.OUTRO:
                            SetVisibility_JOIA(false);
                            SetVisibility_QUOTAS(false);
                            SetVisibility_INFRA(false);
                            SetVisibility_CEDENC(false);
                            SetVisibility_ESGOT(false);
                            SetVisibility_RECONV(false);
                            SetVisibility_OUTROS(true);
                            SetVisibility_TOTAL(true);
                            break;
                        case AMFCCashPayment.PaymentTypes.MULTIPLE:
                            SetVisibility_JOIA(true);
                            SetVisibility_QUOTAS(true);
                            SetVisibility_INFRA(true);
                            SetVisibility_CEDENC(true);
                            SetVisibility_ESGOT(true);
                            SetVisibility_RECONV(true);
                            SetVisibility_OUTROS(true);
                            SetVisibility_TOTAL(true);
                            break;
                    }
                }
                else
                {
                    SetVisibility_JOIA(true);
                    SetVisibility_QUOTAS(true);
                    SetVisibility_INFRA(true);
                    SetVisibility_CEDENC(true);
                    SetVisibility_ESGOT(true);
                    SetVisibility_RECONV(true);
                    SetVisibility_OUTROS(true);
                    SetVisibility_TOTAL(true);
                }

                #endregion  Set Single/Multiple Visisble Options
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>26-11-2017(GesAMFC-v0.0.4.37)</versions>
        public void SetVisibility_JOIA(Boolean bVisibility)
        {
            try
            {
                LayoutVisibility eLayoutVisibility = bVisibility ? LayoutVisibility.Always : LayoutVisibility.Never;
                LayoutControlGroup_Pay_JOIA.Visibility = eLayoutVisibility;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>26-11-2017(GesAMFC-v0.0.4.37)</versions>
        public void SetVisibility_QUOTAS(Boolean bVisibility)
        {
            try
            {
                LayoutVisibility eLayoutVisibility = bVisibility ? LayoutVisibility.Always : LayoutVisibility.Never;
                LayoutControlGroup_Pay_QUOTAS.Visibility = eLayoutVisibility;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>26-11-2017(GesAMFC-v0.0.4.37)</versions>
        public void SetVisibility_INFRA(Boolean bVisibility)
        {
            try
            {
                LayoutVisibility eLayoutVisibility = bVisibility ? LayoutVisibility.Always : LayoutVisibility.Never;
                LayoutControlGroup_Pay_INFRA.Visibility = eLayoutVisibility;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>26-11-2017(GesAMFC-v0.0.4.37)</versions>
        public void SetVisibility_CEDENC(Boolean bVisibility)
        {
            try
            {
                LayoutVisibility eLayoutVisibility = bVisibility ? LayoutVisibility.Always : LayoutVisibility.Never;
                LayoutControlGroup_Pay_CEDENC.Visibility = eLayoutVisibility;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>26-11-2017(GesAMFC-v0.0.4.37)</versions>
        public void SetVisibility_ESGOT(Boolean bVisibility)
        {
            try
            {
                LayoutVisibility eLayoutVisibility = bVisibility ? LayoutVisibility.Always : LayoutVisibility.Never;
                LayoutControlGroup_Pay_ESGOT.Visibility = eLayoutVisibility;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>26-11-2017(GesAMFC-v0.0.4.37)</versions>
        public void SetVisibility_RECONV(Boolean bVisibility)
        {
            try
            {
                LayoutVisibility eLayoutVisibility = bVisibility ? LayoutVisibility.Always : LayoutVisibility.Never;
                LayoutControlGroup_Pay_RECONV.Visibility = eLayoutVisibility;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>26-11-2017(GesAMFC-v0.0.4.37)</versions>
        public void SetVisibility_OUTROS(Boolean bVisibility)
        {
            try
            {
                LayoutVisibility eLayoutVisibility = bVisibility ? LayoutVisibility.Always : LayoutVisibility.Never;
                LayoutControlGroup_Pay_OUTROS.Visibility = eLayoutVisibility;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>26-11-2017(GesAMFC-v0.0.4.37)</versions>
        public void SetVisibility_TOTAL(Boolean bVisibility)
        {
            try
            {
                LayoutVisibility eLayoutVisibility = bVisibility ? LayoutVisibility.Always : LayoutVisibility.Never;
                LayoutControlGroup_Pay_TOTAL.Visibility = eLayoutVisibility;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        #endregion  Payment Entities Visibility

        #region     Payment Entities Enability

        /// <versions>26-11-2017(GesAMFC-v0.0.4.37)</versions>
        public void SetSingleMultipleEnabledOptions(AMFCCashPayment.PaymentTypes ePaymentType, Library_AMFC_Methods.PaymentOptions EntitiesPaymentType)
        {
            try
            {
                #region     Set Single/Multiple Visisble Options
                if (EntitiesPaymentType == Library_AMFC_Methods.PaymentOptions.SINGLE)
                {
                    switch (ePaymentType)
                    {
                        case AMFCCashPayment.PaymentTypes.JOIA:
                            SetEnability_JOIA(false);
                            SetEnability_QUOTAS(false);
                            SetEnability_INFRA(false);
                            SetEnability_CEDENC(false);
                            SetEnability_ESGOT(false);
                            SetEnability_RECONV(false);
                            SetEnability_OUTROS(false);
                            SetEnability_TOTAL(false);
                            break;
                        case AMFCCashPayment.PaymentTypes.QUOTAS:
                            SetEnability_JOIA(false);
                            SetEnability_QUOTAS(false);
                            SetEnability_INFRA(false);
                            SetEnability_CEDENC(false);
                            SetEnability_ESGOT(false);
                            SetEnability_RECONV(false);
                            SetEnability_OUTROS(false);
                            SetEnability_TOTAL(false);
                            break;
                        case AMFCCashPayment.PaymentTypes.INFRAS:
                            SetEnability_JOIA(false);
                            SetEnability_QUOTAS(false);
                            SetEnability_INFRA(false);
                            SetEnability_CEDENC(false);
                            SetEnability_ESGOT(false);
                            SetEnability_RECONV(false);
                            SetEnability_OUTROS(false);
                            SetEnability_TOTAL(false);
                            break;
                        case AMFCCashPayment.PaymentTypes.CEDENC:
                            SetEnability_JOIA(false);
                            SetEnability_QUOTAS(false);
                            SetEnability_INFRA(false);
                            SetEnability_CEDENC(false);
                            SetEnability_ESGOT(false);
                            SetEnability_RECONV(false);
                            SetEnability_OUTROS(false);
                            SetEnability_TOTAL(false);
                            break;
                        case AMFCCashPayment.PaymentTypes.ESGOT:
                            SetEnability_JOIA(false);
                            SetEnability_QUOTAS(false);
                            SetEnability_INFRA(false);
                            SetEnability_CEDENC(false);
                            SetEnability_ESGOT(false);
                            SetEnability_RECONV(false);
                            SetEnability_OUTROS(false);
                            SetEnability_TOTAL(false);
                            break;
                        case AMFCCashPayment.PaymentTypes.RECONV:
                            SetEnability_JOIA(false);
                            SetEnability_QUOTAS(false);
                            SetEnability_INFRA(false);
                            SetEnability_CEDENC(false);
                            SetEnability_ESGOT(false);
                            SetEnability_RECONV(false);
                            SetEnability_OUTROS(false);
                            SetEnability_TOTAL(false);
                            break;
                        case AMFCCashPayment.PaymentTypes.OUTRO:
                            SetEnability_JOIA(false);
                            SetEnability_QUOTAS(false);
                            SetEnability_INFRA(false);
                            SetEnability_CEDENC(false);
                            SetEnability_ESGOT(false);
                            SetEnability_RECONV(false);
                            SetEnability_OUTROS(true);
                            SetEnability_TOTAL(false);
                            break;
                        case AMFCCashPayment.PaymentTypes.MULTIPLE:
                            SetEnability_JOIA(false);
                            SetEnability_QUOTAS(false);
                            SetEnability_INFRA(false);
                            SetEnability_CEDENC(false);
                            SetEnability_ESGOT(false);
                            SetEnability_RECONV(false);
                            SetEnability_OUTROS(false);
                            SetEnability_TOTAL(false);
                            break;
                    }
                }
                else
                {
                    SetEnability_JOIA(false);
                    SetEnability_QUOTAS(false);
                    SetEnability_INFRA(false);
                    SetEnability_CEDENC(false);
                    SetEnability_ESGOT(false);
                    SetEnability_RECONV(false);
                    SetEnability_OUTROS(false);
                    SetEnability_TOTAL(false);
                }
                #endregion  Set Single/Multiple Enabled Options
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>26-11-2017(GesAMFC-v0.0.4.37)</versions>
        public void SetEnability_JOIA(Boolean bEnability)
        {
            LayoutControlGroup_Pay_JOIA.Enabled = bEnability;
        }

        /// <versions>26-11-2017(GesAMFC-v0.0.4.37)</versions>
        public void SetEnability_QUOTAS(Boolean bEnability)
        {
            LayoutControlGroup_Pay_QUOTAS.Enabled = bEnability;
        }

        /// <versions>26-11-2017(GesAMFC-v0.0.4.37)</versions>
        public void SetEnability_INFRA(Boolean bEnability)
        {
            LayoutControlGroup_Pay_INFRA.Enabled = bEnability;
        }

        /// <versions>26-11-2017(GesAMFC-v0.0.4.37)</versions>
        public void SetEnability_CEDENC(Boolean bEnability)
        {
            LayoutControlGroup_Pay_CEDENC.Enabled = bEnability;
        }

        /// <versions>26-11-2017(GesAMFC-v0.0.4.37)</versions>
        public void SetEnability_ESGOT(Boolean bEnability)
        {
            LayoutControlGroup_Pay_ESGOT.Enabled = bEnability;
        }

        /// <versions>26-11-2017(GesAMFC-v0.0.4.37)</versions>
        public void SetEnability_RECONV(Boolean bEnability)
        {
            LayoutControlGroup_Pay_RECONV.Enabled = bEnability;
        }

        /// <versions>26-11-2017(GesAMFC-v0.0.4.37)</versions>
        public void SetEnability_OUTROS(Boolean bEnability)
        {
            LayoutControlGroup_Pay_OUTROS.Enabled = bEnability;
        }

        /// <versions>26-11-2017(GesAMFC-v0.0.4.37)</versions>
        public void SetEnability_TOTAL(Boolean bEnability)
        {
            LayoutControlGroup_Pay_TOTAL.Enabled = bEnability;
        }

        #endregion  Payment Entities Enability

        #region     Payment Action Buttons Methods

        /// <versions>27-11-2017(GesAMFC-v0.0.4.38)</versions>
        public AMFCCashPayment GetCurrentPayDetails()
        {
            try
            {
                AMFCCashPayment objMemberPay = new AMFCCashPayment();

                if (Convert.ToInt64(TextEdit_ID.Text.Trim()) > 0)
                    objMemberPay.ID = Convert.ToInt64(TextEdit_ID.Text.Trim());

                if (Convert.ToInt64(TextEdit_SOCIO.Text.Trim()) > 0 && Convert.ToInt64(TextEdit_SOCIO.Text.Trim()) < new AMFCMember().MaxNumber)
                    objMemberPay.SOCIO = Convert.ToInt64(TextEdit_SOCIO.Text.Trim());

                if (Program.IsValidTextString(TextEdit_NOME.Text))
                    objMemberPay.NOME = TextEdit_NOME.Text.Trim();

                if (Program.IsValidCurrencyEuroValue(TextEdit_VALOR_Total.Text))
                    objMemberPay.VALOR = Program.SetPayCurrencyEuroDoubleValue(TextEdit_VALOR_Total.Text.Trim());

                objMemberPay.DATA = Program.SetDateTimeValue(DateEdit_DATA.DateTime, -1, -1);

                if (Program.IsValidTextString(DatetEdit_ALTERADO.Text))
                    objMemberPay.ALTERADO = Program.ConvertToValidDateTime(DatetEdit_ALTERADO.Text.Trim());

                if (Program.IsValidTextString(TextEdit_NOME_PAG.Text))
                    objMemberPay.NOME_PAG = TextEdit_NOME_PAG.Text.Trim();
                else if (Program.IsValidTextString(TextEdit_NOME.Text))
                    objMemberPay.NOME_PAG = TextEdit_NOME.Text.Trim();
                else
                    objMemberPay.NOME_PAG = objMemberPay.NOME;

                if (Program.IsValidTextString(TextEdit_DESIGNACAO.Text))
                    objMemberPay.DESIGNACAO = TextEdit_DESIGNACAO.Text.Trim();

                if (ComboBoxEdit_ESTADO.SelectedIndex > -1)
                    objMemberPay.Payment_State = objMemberPay.GetPaymentStateType(ComboBoxEdit_ESTADO.SelectedIndex + 1);

                #region     JOIA
                if (Program.IsValidCurrencyEuroValue(TextEdit_JOIAVAL.Text.Trim()))
                {
                    objMemberPay.HasJOIA = true;
                    objMemberPay.JOIADESC = TextEdit_JOIADESC.Text;
                    objMemberPay.JOIAVAL = Program.SetPayCurrencyEuroDoubleValue(TextEdit_JOIAVAL.Text);
                    objMemberPay.DASSOCJOIA = Program.SetDateTimeValue(DateEdit_DASSOCJOIA.DateTime, -1, -1);
                }
                #endregion  JOIA

                #region     QUOTAS
                if (Program.IsValidCurrencyEuroValue(TextEdit_QUOTASVAL.Text.Trim()))
                {
                    objMemberPay.HasQUOTAS = true;
                    objMemberPay.QUOTASDESC = TextEdit_QUOTASDESC.Text;
                    objMemberPay.QUOTASVAL = Program.SetPayCurrencyEuroDoubleValue(TextEdit_QUOTASVAL.Text);
                    objMemberPay.DASSOCQUOT = Program.SetDateTimeValue(DateEdit_DASSOCQUOT.DateTime, -1, -1);
                }
                #endregion  QUOTAS

                #region     INFRAESTRUTURAS
                if (Program.IsValidCurrencyEuroValue(TextEdit_INFRAVAL.Text.Trim()))
                {
                    objMemberPay.HasINFRAEST = true;
                    objMemberPay.INFRADESC = TextEdit_INFRADESC.Text;
                    objMemberPay.INFRAVAL = Program.SetPayCurrencyEuroDoubleValue(TextEdit_INFRAVAL.Text);
                    objMemberPay.DASSOCCEDE = Program.SetDateTimeValue(DateEdit_DASSOCINFR.DateTime, -1, -1);
                }
                #endregion  INFRAESTRUTURAS

                #region     CEDENCIAS
                if (Program.IsValidCurrencyEuroValue(TextEdit_CEDENCVAL.Text.Trim()))
                {
                    objMemberPay.HasCEDENCIAS = true;
                    objMemberPay.CEDENCDESC = TextEdit_CEDENCDESC.Text;
                    objMemberPay.CEDENCVAL = Program.SetPayCurrencyEuroDoubleValue(TextEdit_CEDENCVAL.Text);
                    objMemberPay.DASSOCCEDE = Program.SetDateTimeValue(DateEdit_DASSOCCEDE.DateTime, -1, -1);
                }
                #endregion  CEDENCIAS

                #region     ESGOT
                if (Program.IsValidCurrencyEuroValue(TextEdit_ESGOTVAL.Text.Trim()))
                {
                    objMemberPay.HasESGOT = true;
                    objMemberPay.ESGOTDESC = TextEdit_ESGOTDESC.Text;
                    objMemberPay.ESGOTVAL = Program.SetPayCurrencyEuroDoubleValue(TextEdit_ESGOTVAL.Text);
                    objMemberPay.DASSOCESGO = Program.SetDateTimeValue(DateEdit_DASSOCESGO.DateTime, -1, -1);
                }
                #endregion  ESGOT

                #region     RECONVERSAO URBANISTICA
                if (Program.IsValidCurrencyEuroValue(TextEdit_RECONVAL.Text.Trim()))
                {
                    objMemberPay.HasRECONV = true;
                    objMemberPay.RECONDESC = TextEdit_RECONDESC.Text;
                    objMemberPay.RECONVAL = Program.SetPayCurrencyEuroDoubleValue(TextEdit_RECONVAL.Text);
                    objMemberPay.DASSOCRECO = Program.SetDateTimeValue(DateEdit_DASSOCRECO.DateTime, -1, -1);
                }
                #endregion  RECONVERSAO URBANISTICA

                #region     OUTROS
                if (Program.IsValidCurrencyEuroValue(TextEdit_OUTROSVAL.Text.Trim()))
                {
                    objMemberPay.HasOUTRO = true;
                    objMemberPay.OUTROSDESC = TextEdit_OUTROSDESC.Text;
                    objMemberPay.OUTROSVAL = Program.SetPayCurrencyEuroDoubleValue(TextEdit_OUTROSVAL.Text);
                    objMemberPay.DASSOCOUTR = Program.SetDateTimeValue(DateEdit_DASSOCOUTR.DateTime, -1, -1);
                }
                #endregion  OUTROS

                return objMemberPay;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return null;
            }
        }

        /// <versions>26-11-2017(GesAMFC-v0.0.4.37)</versions>
        public void Load_Details(Int64 lPaymentId)
        {
            try
            {
                if (lPaymentId < 1)
                {
                    Clear_Details(true, true);
                    Set_Editable_Details(false, false);
                    return;
                }

                AMFCCashPayment objPayment = Lib_AMFC_SQL.Get_Cash_Payment_By_Id(lPaymentId);
                if (objPayment == null)
                {
                    Clear_Details(true, true);
                    Set_Editable_Details(false, false);
                    return;
                }

                #region     Load Details
                Clear_Details(true, true);
                Set_Editable_Details(false, false);
                Load_Payment_Details(objPayment);
                #endregion  Load Details

                return;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>26-11-2017(GesAMFC-v0.0.4.37)</versions>
        public void Load_Payment_Details(AMFCCashPayment objPayment)
        {
            try
            {
                TextEdit_ID.Text = objPayment.ID.ToString();
                TextEdit_SOCIO.Text = objPayment.SOCIO.ToString();

                TextEdit_NOME.Text = objPayment.NOME.Trim();

                if (Program.IsValidTextString(objPayment.NOME_PAG))
                    TextEdit_NOME_PAG.Text = objPayment.NOME_PAG.Trim();
                else
                    TextEdit_NOME_PAG.Text = objPayment.NOME.Trim();

                TextEdit_DESIGNACAO.Text = objPayment.DESIGNACAO.Trim();

                DateEdit_DATA.DateTime = Program.SetDateTimeValue(objPayment.DATA, -1, -1);

                if (objPayment.ALTERADO != null)
                    DatetEdit_ALTERADO.DateTime = objPayment.ALTERADO;
                else
                    DatetEdit_ALTERADO.DateTime = Program.Default_Date;

                TextEdit_VALOR_Total.Text = Program.SetPayCurrencyEuroStringValue(objPayment.VALOR);

                ComboBoxEdit_ESTADO.SelectedIndex = (objPayment.Payment_State != AMFCCashPayment.PaymentState.UNDEFINED) ? objPayment.GetPaymentStateIdx() : -1;

                switch (objPayment.Payment_Type)
                {
                    case AMFCCashPayment.PaymentTypes.JOIA:
                        TextEdit_JOIADESC.Text = objPayment.JOIADESC;
                        DateEdit_DASSOCJOIA.DateTime = Program.SetDateTimeValue(objPayment.DASSOCJOIA, -1, -1);
                        TextEdit_JOIAVAL.Text = Program.SetPayCurrencyEuroStringValue(objPayment.JOIAVAL);
                        break;
                    case AMFCCashPayment.PaymentTypes.QUOTAS:
                        TextEdit_QUOTASDESC.Text = objPayment.QUOTASDESC;
                        DateEdit_DASSOCQUOT.DateTime = Program.SetDateTimeValue(objPayment.DASSOCQUOT, -1, -1);
                        TextEdit_QUOTASVAL.Text = Program.SetPayCurrencyEuroStringValue(objPayment.QUOTASVAL);
                        break;
                    case AMFCCashPayment.PaymentTypes.INFRAS:
                        TextEdit_INFRADESC.Text = objPayment.INFRADESC;
                        DateEdit_DASSOCINFR.DateTime = Program.SetDateTimeValue(objPayment.DASSOCINFR, -1, -1);
                        TextEdit_INFRAVAL.Text = Program.SetPayCurrencyEuroStringValue(objPayment.INFRAVAL);
                        break;
                    case AMFCCashPayment.PaymentTypes.CEDENC:
                        TextEdit_CEDENCDESC.Text = objPayment.CEDENCDESC;
                        DateEdit_DASSOCCEDE.DateTime = Program.SetDateTimeValue(objPayment.DASSOCCEDE, -1, -1);
                        TextEdit_CEDENCVAL.Text = Program.SetPayCurrencyEuroStringValue(objPayment.CEDENCVAL);
                        break;
                    case AMFCCashPayment.PaymentTypes.ESGOT:
                        TextEdit_ESGOTDESC.Text = objPayment.ESGOTDESC;
                        DateEdit_DASSOCESGO.DateTime = Program.SetDateTimeValue(objPayment.DASSOCESGO, -1, -1);
                        TextEdit_ESGOTVAL.Text = Program.SetPayCurrencyEuroStringValue(objPayment.ESGOTVAL);
                        break;
                    case AMFCCashPayment.PaymentTypes.RECONV:
                        TextEdit_RECONDESC.Text = objPayment.RECONDESC;
                        DateEdit_DASSOCRECO.DateTime = Program.SetDateTimeValue(objPayment.DASSOCRECO, -1, -1);
                        TextEdit_RECONVAL.Text = Program.SetPayCurrencyEuroStringValue(objPayment.RECONVAL);
                        break;
                    case AMFCCashPayment.PaymentTypes.OUTRO:
                        TextEdit_OUTROSDESC.Text = objPayment.OUTROSDESC;
                        DateEdit_DASSOCOUTR.DateTime = Program.SetDateTimeValue(objPayment.DASSOCOUTR, -1, -1);
                        TextEdit_OUTROSVAL.Text = Program.SetPayCurrencyEuroStringValue(objPayment.OUTROSVAL);
                        break;
                    case AMFCCashPayment.PaymentTypes.MULTIPLE:
                    default:
                        TextEdit_JOIADESC.Text = objPayment.JOIADESC;
                        DateEdit_DASSOCJOIA.DateTime = Program.SetDateTimeValue(objPayment.DASSOCJOIA, -1, -1);
                        TextEdit_JOIAVAL.Text = Program.SetPayCurrencyEuroStringValue(objPayment.JOIAVAL);

                        TextEdit_QUOTASDESC.Text = objPayment.QUOTASDESC;
                        DateEdit_DASSOCQUOT.DateTime = Program.SetDateTimeValue(objPayment.DASSOCQUOT, -1, -1);
                        TextEdit_QUOTASVAL.Text = Program.SetPayCurrencyEuroStringValue(objPayment.QUOTASVAL);

                        TextEdit_INFRADESC.Text = objPayment.INFRADESC;
                        DateEdit_DASSOCINFR.DateTime = Program.SetDateTimeValue(objPayment.DASSOCINFR, -1, -1);
                        TextEdit_INFRAVAL.Text = Program.SetPayCurrencyEuroStringValue(objPayment.INFRAVAL);

                        TextEdit_CEDENCDESC.Text = objPayment.CEDENCDESC;
                        DateEdit_DASSOCCEDE.DateTime = Program.SetDateTimeValue(objPayment.DASSOCCEDE, -1, -1);
                        TextEdit_CEDENCVAL.Text = Program.SetPayCurrencyEuroStringValue(objPayment.CEDENCVAL);

                        TextEdit_ESGOTDESC.Text = objPayment.ESGOTDESC;
                        DateEdit_DASSOCESGO.DateTime = Program.SetDateTimeValue(objPayment.DASSOCESGO, -1, -1);
                        TextEdit_ESGOTVAL.Text = Program.SetPayCurrencyEuroStringValue(objPayment.ESGOTVAL);

                        TextEdit_RECONDESC.Text = objPayment.RECONDESC;
                        DateEdit_DASSOCRECO.DateTime = Program.SetDateTimeValue(objPayment.DASSOCRECO, -1, -1);
                        TextEdit_RECONVAL.Text = Program.SetPayCurrencyEuroStringValue(objPayment.RECONVAL);

                        TextEdit_OUTROSDESC.Text = objPayment.OUTROSDESC;
                        DateEdit_DASSOCOUTR.DateTime = Program.SetDateTimeValue(objPayment.DASSOCOUTR, -1, -1);
                        TextEdit_OUTROSVAL.Text = Program.SetPayCurrencyEuroStringValue(objPayment.OUTROSVAL);
                        break;
                }
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>26-11-2017(GesAMFC-v0.0.4.37)</versions>
        public void Set_Editable_Details(Boolean bCanEdit, Boolean bEditMember)
        {
            try
            {
                Boolean bIsReadOnly = !bCanEdit;

                TextEdit_ID.Properties.ReadOnly = true;
                if (bEditMember)
                {
                    TextEdit_SOCIO.Properties.ReadOnly = bIsReadOnly;
                    TextEdit_NOME.Properties.ReadOnly = bIsReadOnly;
                }
                else
                {
                    TextEdit_SOCIO.Properties.ReadOnly = true;
                    TextEdit_NOME.Properties.ReadOnly = true;
                }
                TextEdit_NOME_PAG.Properties.ReadOnly = bIsReadOnly;
                TextEdit_DESIGNACAO.Properties.ReadOnly = bIsReadOnly;
                DateEdit_DATA.Properties.ReadOnly = bIsReadOnly;
                DatetEdit_ALTERADO.Properties.ReadOnly = bIsReadOnly;
                TextEdit_VALOR_Total.Properties.ReadOnly = bIsReadOnly;

                ComboBoxEdit_ESTADO.Properties.ReadOnly = bIsReadOnly;

                TextEdit_JOIADESC.Properties.ReadOnly = bIsReadOnly;
                DateEdit_DASSOCJOIA.Properties.ReadOnly = bIsReadOnly;
                TextEdit_JOIAVAL.Properties.ReadOnly = bIsReadOnly;

                TextEdit_QUOTASDESC.Properties.ReadOnly = bIsReadOnly;
                DateEdit_DASSOCQUOT.Properties.ReadOnly = bIsReadOnly;
                TextEdit_QUOTASVAL.Properties.ReadOnly = bIsReadOnly;

                TextEdit_INFRADESC.Properties.ReadOnly = bIsReadOnly;
                DateEdit_DASSOCINFR.Properties.ReadOnly = bIsReadOnly;
                TextEdit_INFRAVAL.Properties.ReadOnly = bIsReadOnly;

                TextEdit_CEDENCDESC.Properties.ReadOnly = bIsReadOnly;
                DateEdit_DASSOCCEDE.Properties.ReadOnly = bIsReadOnly;
                TextEdit_CEDENCVAL.Properties.ReadOnly = bIsReadOnly;

                TextEdit_ESGOTDESC.Properties.ReadOnly = bIsReadOnly;
                DateEdit_DASSOCESGO.Properties.ReadOnly = bIsReadOnly;
                TextEdit_ESGOTVAL.Properties.ReadOnly = bIsReadOnly;

                TextEdit_RECONDESC.Properties.ReadOnly = bIsReadOnly;
                DateEdit_DASSOCRECO.Properties.ReadOnly = bIsReadOnly;
                TextEdit_RECONVAL.Properties.ReadOnly = bIsReadOnly;

                TextEdit_OUTROSDESC.Properties.ReadOnly = bIsReadOnly;
                DateEdit_DASSOCOUTR.Properties.ReadOnly = bIsReadOnly;
                TextEdit_OUTROSVAL.Properties.ReadOnly = bIsReadOnly;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>26-11-2017(GesAMFC-v0.0.4.37)</versions>
        public void Clear_Details(Boolean bClearId, Boolean bClearMember)
        {
            try
            {
                if (bClearId)
                    TextEdit_ID.Text = String.Empty;
                if (bClearMember)
                {
                    TextEdit_SOCIO.Text = String.Empty;
                    TextEdit_NOME.Text = String.Empty;
                }
                TextEdit_NOME_PAG.Text = String.Empty;
                TextEdit_DESIGNACAO.Text = String.Empty;
                DateEdit_DATA.DateTime = Program.Default_Date;
                DatetEdit_ALTERADO.Text = String.Empty;

                TextEdit_VALOR_Total.Text = Program.Default_Pay_String;

                ComboBoxEdit_ESTADO.SelectedIndex = -1;

                TextEdit_JOIADESC.Text = String.Empty;
                DateEdit_DASSOCJOIA.DateTime = Program.Default_Date;
                TextEdit_JOIAVAL.Text = Program.Default_Pay_String;

                TextEdit_QUOTASDESC.Text = String.Empty;
                DateEdit_DASSOCQUOT.DateTime = Program.Default_Date;
                TextEdit_QUOTASVAL.Text = Program.Default_Pay_String;

                TextEdit_INFRADESC.Text = String.Empty;
                DateEdit_DASSOCINFR.DateTime = Program.Default_Date;
                TextEdit_INFRAVAL.Text = Program.Default_Pay_String;

                TextEdit_CEDENCDESC.Text = String.Empty;
                DateEdit_DASSOCCEDE.DateTime = Program.Default_Date;
                TextEdit_CEDENCVAL.Text = Program.Default_Pay_String;

                TextEdit_ESGOTDESC.Text = String.Empty;
                DateEdit_DASSOCESGO.DateTime = Program.Default_Date;
                TextEdit_ESGOTVAL.Text = Program.Default_Pay_String;

                TextEdit_RECONDESC.Text = String.Empty;
                DateEdit_DASSOCRECO.DateTime = Program.Default_Date;
                TextEdit_RECONVAL.Text = Program.Default_Pay_String;

                TextEdit_OUTROSDESC.Text = String.Empty;
                DateEdit_DASSOCOUTR.DateTime = Program.Default_Date;
                TextEdit_OUTROSVAL.Text = Program.Default_Pay_String;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>26-11-2017(GesAMFC-v0.0.4.37)</versions>
        public Int32 CheckPayIsValid(AMFCCashPayment objMemberPay, Boolean bCheckPayId)
        {
            try
            {
                #region     Pay ID
                if (bCheckPayId)
                {
                    if (objMemberPay.ID < 1)
                    {
                        String sError = "O " + "Nº de " + "Pagamento" + ": " + objMemberPay.ID + " não é válido! Por favor, modifique.";
                        MessageBox.Show(sError, "Nº " + "Pagamento" + "  Inválido", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return -1;
                    }
                }
                #endregion  Pay ID

                #region     Nº Sócio
                if (objMemberPay.SOCIO < 1 || objMemberPay.SOCIO > new AMFCMember().MaxNumber)
                {
                    String sError = "O " + "Nº de Sócio: " + objMemberPay.SOCIO + " não é válido! Por favor, modifique.";
                    MessageBox.Show(sError, "Sócio Inválido", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return -1;
                }
                #endregion  Nº Sócio

                #region     Nome
                if (!Program.IsValidTextString(objMemberPay.NOME))
                {
                    String sWarning = "O " + "Nome do Sócio" + " (" + objMemberPay.NOME.Trim() + ") não é válido! Por favor, modifique.";
                    MessageBox.Show(sWarning, "Nome Sócio" + " Invalído", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return -1;
                }
                #endregion  Nome

                #region     Pay VALOR
                if (objMemberPay.VALOR < 0)
                {
                    String sWarning = "O " + "Valor Total" + " do " + "Pagamento" + " (" + objMemberPay.VALOR.ToString() + ") não é válido! Por favor, modifique.";
                    MessageBox.Show(sWarning, "Valor Total" + " do " + "Pagamento" + " " + " Invalído", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return -1;
                }
                #endregion  Pay VALOR

                #region     Pay Date
                if (!Program.IsValidTextString(objMemberPay.DATA_Str))
                {
                    String sWarning = "A " + "data do " + "pagamento" + " do Sócio (" + objMemberPay.DATA_Str.Trim() + ") não é válida! Por favor, modifique.";
                    MessageBox.Show(sWarning, "Data de " + "Pagamento" + " Invalída", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return -1;
                }
                #endregion  Pay Date

                #region     Pay Person
                if (!Program.IsValidTextString(objMemberPay.NOME_PAG))
                {
                    String sWarning = "O " + "nome da pessoa que efetuou o pagamento" + " (" + objMemberPay.NOME_PAG.Trim() + ") não é válido! Por favor, modifique.";
                    MessageBox.Show(sWarning, "" + "Nome de que pagou invalído", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return -1;
                }
                #endregion Pay Person

                return 1;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return -1;
            }
        }

        /// <versions>26-11-2017(GesAMFC-v0.0.4.37)</versions>
        public Boolean SetPayDetails(AMFCCashPayment objMemberPay, Library_AMFC_Methods.OperationType eOpType)
        {
            try
            {
                if (eOpType == Library_AMFC_Methods.OperationType.UNDEFINED)
                    return false;

                Boolean bPayPaid = (objMemberPay.ID > 0 && objMemberPay.VALOR > 0);

                TextEdit_ID.Text = objMemberPay.ID.ToString();

                TextEdit_SOCIO.Text = objMemberPay.SOCIO.ToString();

                TextEdit_NOME.Text = Program.SetTextString(objMemberPay.NOME, Program.DefaultStringTextTypes.EMPTY);

                TextEdit_VALOR_Total.Text = Program.SetPayCurrencyEuroStringValue(objMemberPay.VALOR);

                DateEdit_DATA.DateTime = Program.SetDateTimeValue(objMemberPay.DATA, -1, -1);

                TextEdit_DESIGNACAO.Text = Program.SetTextString(objMemberPay.DESIGNACAO, Program.DefaultStringTextTypes.EMPTY);

                if (Program.IsValidTextString(objMemberPay.NOME_PAG.Trim()))
                    TextEdit_NOME_PAG.Text = objMemberPay.NOME_PAG.Trim();
                else
                    TextEdit_NOME_PAG.Text = objMemberPay.NOME.Trim();

                if (Program.IsValidDateTime(objMemberPay.ALTERADO))
                    DatetEdit_ALTERADO.Text = objMemberPay.ALTERADO_Str;
                else
                    DatetEdit_ALTERADO.Text = Program.Default_Date.ToString(Program.Date_Format_String, CultureInfo.CurrentCulture);

                ComboBoxEdit_ESTADO.SelectedIndex = (objMemberPay.Payment_State != AMFCCashPayment.PaymentState.UNDEFINED) ? objMemberPay.GetPaymentStateIdx() : -1;

                Program.DefaultStringTextTypes eDefaultStringTextType = (eOpType == Library_AMFC_Methods.OperationType.ADD) ? Program.DefaultStringTextTypes.EMPTY : Program.DefaultStringTextTypes.DEFAULT;

                #region     JOIA
                TextEdit_JOIADESC.Text = Program.SetTextString(objMemberPay.JOIADESC, eDefaultStringTextType);
                DateEdit_DASSOCJOIA.DateTime = Program.SetDateTimeValue(objMemberPay.DASSOCJOIA, -1, -1);
                TextEdit_JOIAVAL.Text = Program.SetPayCurrencyEuroStringValue(objMemberPay.JOIAVAL);
                #endregion  JOIA

                #region     QUOTAS
                TextEdit_QUOTASDESC.Text = Program.SetTextString(objMemberPay.QUOTASDESC, eDefaultStringTextType);
                DateEdit_DASSOCQUOT.DateTime = Program.SetDateTimeValue(objMemberPay.DASSOCQUOT, -1, -1);
                TextEdit_QUOTASVAL.Text = Program.SetPayCurrencyEuroStringValue(objMemberPay.QUOTASVAL);
                #endregion  QUOTAS

                #region     INFRAESTRUTURAS
                TextEdit_INFRADESC.Text = Program.SetTextString(objMemberPay.INFRADESC, eDefaultStringTextType);
                DateEdit_DASSOCINFR.DateTime = Program.SetDateTimeValue(objMemberPay.DASSOCINFR, -1, -1);
                TextEdit_INFRAVAL.Text = Program.SetPayCurrencyEuroStringValue(objMemberPay.INFRAVAL);
                #endregion  INFRAESTRUTURAS

                #region     CEDENC
                TextEdit_CEDENCDESC.Text = Program.SetTextString(objMemberPay.CEDENCDESC, eDefaultStringTextType);
                DateEdit_DASSOCCEDE.DateTime = Program.SetDateTimeValue(objMemberPay.DASSOCCEDE, -1, -1);
                TextEdit_CEDENCVAL.Text = Program.SetPayCurrencyEuroStringValue(objMemberPay.CEDENCVAL);
                #endregion  CEDENC

                #region     ESGOT
                TextEdit_ESGOTDESC.Text = Program.SetTextString(objMemberPay.ESGOTDESC, eDefaultStringTextType);
                DateEdit_DASSOCESGO.DateTime = Program.SetDateTimeValue(objMemberPay.DASSOCESGO, -1, -1);
                TextEdit_ESGOTVAL.Text = Program.SetPayCurrencyEuroStringValue(objMemberPay.ESGOTVAL);
                #endregion  ESGOT

                #region     RECONVERSAO URBANISTICA
                TextEdit_RECONDESC.Text = Program.SetTextString(objMemberPay.RECONDESC, eDefaultStringTextType);
                DateEdit_DASSOCRECO.DateTime = Program.SetDateTimeValue(objMemberPay.DASSOCRECO, -1, -1);
                TextEdit_RECONVAL.Text = Program.SetPayCurrencyEuroStringValue(objMemberPay.RECONVAL);
                #endregion  RECONVERSAO URBANISTICA

                #region     OUTROS
                TextEdit_OUTROSDESC.Text = Program.SetTextString(objMemberPay.OUTROSDESC, eDefaultStringTextType);
                DateEdit_DASSOCOUTR.DateTime = Program.SetDateTimeValue(objMemberPay.DASSOCOUTR, -1, -1);
                TextEdit_OUTROSVAL.Text = Program.SetPayCurrencyEuroStringValue(objMemberPay.OUTROSVAL);
                #endregion  OUTROS

                //.TextEdit_Pay_NOTAS.Text = objMemberPay.NOTAS;
                return true;
            }
            catch (Exception ex)
            {

                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return false;
            }
        }

        /// <versions>26-11-2017(GesAMFC-v0.0.4.37)</versions>
        public void Reload_Pay_Details(AMFCCashPayment CurrentPayment, Library_AMFC_Methods.OperationType eOpType)
        {
            try
            {
                AMFCCashPayment objMemberPay = null;

                #region     Get Focused Pay
                if (this.Payment != null && this.Payment.ID > 1)
                {
                    if (this.Grid_View.FocusedRowHandle > 0)
                    {
                        objMemberPay = Get_Selected(this.Grid_View.FocusedRowHandle, -1);
                        if (objMemberPay == null || objMemberPay.SOCIO < 1 || objMemberPay.SOCIO > new AMFCMember().MaxNumber || objMemberPay.ID < 1)
                        {
                            Clear_Details(true, true);
                            XtraMessageBox.Show("Não foi possível obter o " + "Pagamento" + " selecionado!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Load_Grid(false, false, false, false, true, -1, -1, -1, new DateTime());
                            return;
                        }
                    }
                }
                #endregion  Get Focused Pay

                #region     Load Pay Details
                if (objMemberPay != null)
                {
                    if (!SetPayDetails(objMemberPay, eOpType))
                    {
                        Clear_Details(true, true);
                        XtraMessageBox.Show("Não foi possível carregar os detalhes do " + "Pagamento" + "!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Load_Grid(false, false, false, false, true, -1, -1, -1, new DateTime());
                        return;
                    }
                }
                #endregion  Load Pay Details
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return;
            }
        }

        /// <versions>26-11-2017(GesAMFC-v0.0.4.37)</versions>
        public void Button_Pay_Close(AMFCCashPayment objPayment)
        {
            try
            {
                DialogResult objResult_MemberMoneyDelivered = XtraMessageBox.Show("Deseja finalizar este pagamento? Ou seja, o sócio entregou o valor de: " + TextEdit_VALOR_Total.Text + " ?", "Finalizar pagamento ?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (objResult_MemberMoneyDelivered == DialogResult.No)
                {
                    ComboBoxEdit_ESTADO.SelectedIndex = 0;
                    objPayment.Payment_State = AMFCCashPayment.PaymentState.ABERTO;
                }
                else if (objResult_MemberMoneyDelivered == DialogResult.Yes)
                {
                    ComboBoxEdit_ESTADO.SelectedIndex = 1;
                    objPayment.Payment_State = AMFCCashPayment.PaymentState.FINALIZADO;
                    if (PaymentOperationType == Library_AMFC_Methods.OperationType.ADD)
                        Button_Add_Save_Click();
                    else if (PaymentOperationType == Library_AMFC_Methods.OperationType.EDIT)
                        Button_Edit_Save_Click();
                    if (!DBF_AMFC_Set_Entities_Payment_Closed(objPayment))
                    {
                        String sError = "Ocorreu um erro no processo de fechar o " + "Pagamento" + " do Sócio " + objPayment.NOME + " (Nº: " + objPayment.SOCIO + ")!";
                        MessageBox.Show(sError, "Erro Fechar Pagamento", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return;
            }
        }

        #region     Payment Open Pay

        /// <versions>26-11-2017(GesAMFC-v0.0.4.37)</versions>
        public void Button_Open_Pay_Action(AMFCMember PaymentMember, AMFCCashPayment CurrentPayment, Library_AMFC_Methods.OperationType PaymentOperationType)
        {
            try
            {
                #region     Get Member to Add
                if (PaymentMember == null || PaymentMember.NUMERO < 1 || PaymentMember.NUMERO > PaymentMember.MaxNumber)
                {
                    String sError = "Ocorreu um erro na obtenção do sócio!";
                    MessageBox.Show(sError, "Erro Sócio", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                #endregion  Get Member to Add

                #region     Set Controls to Add
                Clear_Details(true, false);
                Set_Editable_Details(false, false);
                LayoutControlGroup_Pay_Admin.Visibility         = LayoutVisibility.Never;
                LayoutControlItem_Pay_Add.Visibility            = LayoutVisibility.Never;
                LayoutControlItem_Pay_Edit.Visibility           = LayoutVisibility.Never;
                LayoutControlItem_Pay_Del.Visibility            = LayoutVisibility.Never;
                //LayoutControlGroup_Receipt_Buttons.Visibility   = LayoutVisibility.Never;
                LayoutControlGroup_Pay_Add.Visibility           = LayoutVisibility.Always;
                LayoutControlGroup_Pay_Edit.Visibility          = LayoutVisibility.Never;
                #endregion  Set Controls to Add

                #region     Set Member Pay Info
                if (!SetPayDetails(CurrentPayment, PaymentOperationType))
                {
                    Clear_Details(true, true);
                    XtraMessageBox.Show("Não foi possível obter carregar detalhes do " + "Pagamento" + "!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                #endregion  Set Member Pay Info
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return;
            }
        }

        #endregion  Payment Open Pay

        #region     Payment Add Pay

        /// <versions>26-11-2017(GesAMFC-v0.0.4.37)</versions>
        public AMFCCashPayment GetPayDetailsToAdd(AMFCMember objMember)
        {
            try
            {
                AMFCCashPayment objMemberPay = new AMFCCashPayment();
                #region     Get Pay Max ID
                Int32 iPayMaxId = DBF_AMFC_Members_GetMaxPayId();
                if (iPayMaxId < 1)
                    return null;
                #endregion  Get Pay Max ID
                objMemberPay.ID = Convert.ToInt64(iPayMaxId) + 1;
                if (objMember.NUMERO > 0 && objMember.NUMERO < new AMFCMember().MaxNumber)
                    objMemberPay.SOCIO = objMember.NUMERO;
                if (Program.IsValidTextString(objMember.NOME))
                    objMemberPay.NOME = objMember.NOME.Trim();
                objMemberPay.VALOR = 0;
                objMemberPay.DATA = Program.Default_Date;
                objMemberPay.ALTERADO = Program.Default_Date;

                if (Program.IsValidTextString(objMember.NOME))
                    objMemberPay.NOME_PAG = objMember.NOME.Trim();

                if (Program.IsValidTextString(TextEdit_DESIGNACAO.Text))
                    objMemberPay.DESIGNACAO = TextEdit_DESIGNACAO.Text.Trim();

                if (ComboBoxEdit_ESTADO.SelectedIndex > -1)
                    objMemberPay.Payment_State = objMemberPay.GetPaymentStateType(ComboBoxEdit_ESTADO.SelectedIndex + 1);

                #region     JOIA
                try
                {
                    if (Program.IsValidCurrencyEuroValue(TextEdit_JOIAVAL.Text.Trim()))
                    {
                        objMemberPay.HasJOIA = true;
                        objMemberPay.JOIADESC = TextEdit_JOIADESC.Text;
                        objMemberPay.SetItemPayment(AMFCCashPayment.PaymentTypes.JOIA, Program.SetPayCurrencyEuroDoubleValue(TextEdit_JOIAVAL.Text.Trim()), false, false);
                        objMemberPay.DASSOCJOIA = Program.SetDateTimeValue(DateEdit_DASSOCJOIA.DateTime, -1, -1);
                    }
                }
                catch { }
                #endregion  JOIA

                #region     QUOTAS
                try
                {
                    if (Program.IsValidCurrencyEuroValue(TextEdit_QUOTASVAL.Text.Trim()))
                    {
                        objMemberPay.HasQUOTAS = true;
                        objMemberPay.QUOTASDESC = TextEdit_QUOTASDESC.Text;
                        objMemberPay.SetItemPayment(AMFCCashPayment.PaymentTypes.QUOTAS, Program.SetPayCurrencyEuroDoubleValue(TextEdit_QUOTASVAL.Text.Trim()), false, false);
                        objMemberPay.DASSOCQUOT = Program.SetDateTimeValue(DateEdit_DASSOCQUOT.DateTime, -1, -1);
                    }
                }
                catch { }
                #endregion  QUOTAS

                #region     INFRAESTRUTURAS
                try
                {

                    if (Program.IsValidCurrencyEuroValue(TextEdit_INFRAVAL.Text.Trim()))
                    {
                        objMemberPay.HasINFRAEST = true;
                        objMemberPay.INFRADESC = TextEdit_INFRADESC.Text;
                        objMemberPay.SetItemPayment(AMFCCashPayment.PaymentTypes.INFRAS, Program.SetPayCurrencyEuroDoubleValue(TextEdit_INFRAVAL.Text.Trim()), false, false);
                        objMemberPay.DASSOCINFR = Program.SetDateTimeValue(DateEdit_DASSOCINFR.DateTime, -1, -1);
                    }
                }
                catch { }
                #endregion  INFRAESTRUTURAS

                #region     CEDENC
                try
                {
                    if (Program.IsValidCurrencyEuroValue(TextEdit_CEDENCVAL.Text.Trim()))
                    {
                        objMemberPay.HasCEDENCIAS = true;
                        objMemberPay.CEDENCDESC = TextEdit_CEDENCDESC.Text;
                        objMemberPay.SetItemPayment(AMFCCashPayment.PaymentTypes.CEDENC, Program.SetPayCurrencyEuroDoubleValue(TextEdit_CEDENCVAL.Text.Trim()), false, false);
                        objMemberPay.DASSOCCEDE = Program.SetDateTimeValue(DateEdit_DASSOCCEDE.DateTime, -1, -1);
                    }
                }
                catch { }
                #endregion  CEDENC

                #region     ESGOT
                try
                {
                    if (Program.IsValidCurrencyEuroValue(TextEdit_ESGOTVAL.Text.Trim()))
                    {
                        objMemberPay.HasESGOT = true;
                        objMemberPay.ESGOTDESC = TextEdit_ESGOTDESC.Text;
                        objMemberPay.SetItemPayment(AMFCCashPayment.PaymentTypes.ESGOT, Program.SetPayCurrencyEuroDoubleValue(TextEdit_ESGOTVAL.Text.Trim()), false, false);
                        objMemberPay.DASSOCESGO = Program.SetDateTimeValue(DateEdit_DASSOCESGO.DateTime, -1, -1);
                    }
                }
                catch { }
                #endregion  ESGOT

                #region     RECONVERSAO URBANISTICA
                try
                {
                    if (Program.IsValidCurrencyEuroValue(TextEdit_RECONVAL.Text.Trim()))
                    {
                        objMemberPay.HasRECONV = true;
                        objMemberPay.RECONDESC = TextEdit_RECONDESC.Text;
                        objMemberPay.SetItemPayment(AMFCCashPayment.PaymentTypes.RECONV, Program.SetPayCurrencyEuroDoubleValue(TextEdit_RECONVAL.Text.Trim()), false, false);
                        objMemberPay.DASSOCRECO = Program.SetDateTimeValue(DateEdit_DASSOCRECO.DateTime, -1, -1);
                    }
                }
                catch { }
                #endregion  RECONVERSAO URBANISTICA
                
                #region     OUTROS
                try
                {
                    if (Program.IsValidCurrencyEuroValue(TextEdit_OUTROSVAL.Text.Trim()))
                    {
                        objMemberPay.HasOUTRO = true;
                        objMemberPay.OUTROSDESC = TextEdit_OUTROSDESC.Text;
                        objMemberPay.SetItemPayment(AMFCCashPayment.PaymentTypes.OUTRO, Program.SetPayCurrencyEuroDoubleValue(TextEdit_OUTROSVAL.Text.Trim()), false, false);
                        objMemberPay.DASSOCOUTR = Program.SetDateTimeValue(DateEdit_DASSOCOUTR.DateTime, -1, -1);
                    }
                }
                catch { }
                #endregion  OUTROS

                //objMemberPay.NOTAS = String.Empty;

                return objMemberPay;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return null;
            }
        }

        /// <versions>26-11-2017(GesAMFC-v0.0.4.37)</versions>
        public Int32 DBF_AMFC_Members_GetMaxPayId()
        {
            try
            {
                using (Library_AMFC_SQL obj_AMFC_SQL = new Library_AMFC_SQL())
                {
                    Int32 iPayMaxId = obj_AMFC_SQL.Get_Pay_Max_Number();
                    if (iPayMaxId < 1)
                    {
                        String sWarning = "Não foi possivel obter o número máximo de ID das Pays! Por favor, contacte o programador!";
                        MessageBox.Show(sWarning, "Erro: Nº de " + "Pagamento" + "  Máximo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return -1;
                    }
                    return iPayMaxId;
                }
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return -1;
            }
        }

        /// <versions>26-11-2017(GesAMFC-v0.0.4.37)</versions>
        public void Button_Add_Pay_Action(AMFCMember objMember)
        {
            try
            {
                #region     Get Member to Add
                if (objMember == null || objMember.NUMERO < 1 || objMember.NUMERO > objMember.MaxNumber)
                {
                    String sError = "Ocorreu um erro na obtenção do sócio!";
                    MessageBox.Show(sError, "Erro Sócio", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                #endregion  Get Member to Add

                #region     Set Controls to Add
                Clear_Details(true, false);
                Set_Editable_Details(true, false);
                LayoutControlGroup_Pay_Admin.Visibility         = LayoutVisibility.Never;
                LayoutControlItem_Pay_Add.Visibility            = LayoutVisibility.Never;
                LayoutControlItem_Pay_Edit.Visibility           = LayoutVisibility.Never;
                LayoutControlItem_Pay_Del.Visibility            = LayoutVisibility.Never;
                //LayoutControlGroup_Receipt_Buttons.Visibility   = LayoutVisibility.Never;
                LayoutControlGroup_Pay_Add.Visibility           = LayoutVisibility.Always;
                LayoutControlGroup_Pay_Edit.Visibility          = LayoutVisibility.Never;
                #endregion  Set Controls to Add

                #region     Get Member Pay Info to Add
                AMFCCashPayment objPayment = GetPayDetailsToAdd(objMember);
                if (objPayment == null)
                {
                    String sError = "Ocorreu um erro a obter os dados do " + "Pagamento" + " " + objPayment.NOME + " (Nº: " + objPayment.SOCIO + ")!";
                    MessageBox.Show(sError, "Erro Criação Sócio", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                #endregion  Get Member Pay Info to Add

                #region     Set Member Pay Info to Add
                if (!SetPayDetails(objPayment, Library_AMFC_Methods.OperationType.ADD))
                {
                    Clear_Details(true, true);
                    XtraMessageBox.Show("Não foi possível obter carregar detalhes do " + "Pagamento" + "!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                #endregion  Set Member Pay Info to Add
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return;
            }
        }

        /// <versions>26-11-2017(GesAMFC-v0.0.4.37)</versions>
        public void Button_Add_Save_Click()
        {
            try
            {
                #region     Member Pay Info
                AMFCCashPayment objMemberPay = GetPayDetailsEdited();
                if (objMemberPay == null)
                {
                    String sError = "Ocorreu um erro a obter os dados do " + "Pagamento" + " do Sócio " + objMemberPay.NOME + " (Nº: " + objMemberPay.SOCIO + ")!";
                    MessageBox.Show(sError, "Erro Criação Sócio", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                #endregion  Member Pay Info

                #region     Validate Data 
                if (CheckPayIsValid(objMemberPay, true) < 1)
                    return;
                #endregion  Validate Data

                #region     Add Operation
                if (!DBF_AMFC_Add_Pay(objMemberPay))
                    return;
                #endregion  Add Operation

                Set_Editable_Details(false, false);
                LayoutControlGroup_Pay_Admin.Visibility         = LayoutVisibility.Never;
                LayoutControlItem_Pay_Add.Visibility            = LayoutVisibility.Never;
                LayoutControlItem_Pay_Edit.Visibility           = LayoutVisibility.Never;
                LayoutControlItem_Pay_Del.Visibility            = LayoutVisibility.Never;
                //LayoutControlGroup_Receipt_Buttons.Visibility   = LayoutVisibility.Never;
                LayoutControlGroup_Pay_Add.Visibility           = LayoutVisibility.Never;
                LayoutControlGroup_Pay_Edit.Visibility          = LayoutVisibility.Never;
                Load_Grid(true, true, true, true, true, -1, objMemberPay.ID, objMemberPay.SOCIO, new DateTime());
                Load_Details(objMemberPay.ID);
                this.Grid_View.Focus();
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return;
            }
        }

        /// <versions>26-11-2017(GesAMFC-v0.0.4.37)</versions>
        public void Button_Add_Cancel_Click()
        {
            try
            {
                Clear_Details(true, true);
                Set_Editable_Details(false, false);
                LayoutControlGroup_Pay_Admin.Visibility         = LayoutVisibility.Never;
                LayoutControlItem_Pay_Add.Visibility            = LayoutVisibility.Never;
                LayoutControlItem_Pay_Edit.Visibility           = LayoutVisibility.Never;
                LayoutControlItem_Pay_Del.Visibility            = LayoutVisibility.Never;
                //LayoutControlGroup_Receipt_Buttons.Visibility   = LayoutVisibility.Never;
                LayoutControlGroup_Pay_Add.Visibility           = LayoutVisibility.Never;
                LayoutControlGroup_Pay_Edit.Visibility          = LayoutVisibility.Never;
                Load_Grid(true, true, true, true, true, -1, -1, this.PaymentMember.NUMERO, new DateTime());
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return;
            }
        }

        /// <versions>26-11-2017(GesAMFC-v0.0.4.37)</versions>
        public void Button_Add_Repor_Click(AMFCCashPayment objPayment, Library_AMFC_Methods.OperationType PaymentOperationType)
        {
            Reload_Pay_Details(objPayment, PaymentOperationType);
        }

        /// <versions>26-11-2017(GesAMFC-v0.0.4.37)</versions>
        public Boolean DBF_AMFC_Add_Pay(AMFCCashPayment objPayment)
        {
            try
            {
                using (Library_AMFC_SQL obj_AMFC_SQL = new Library_AMFC_SQL())
                {
                    Int64 lOpStatus = -1;
                    lOpStatus = obj_AMFC_SQL.Add_Payment(objPayment);
                    if (lOpStatus == 1)
                    {
                        String sInfo = "Pagamento" + " do Sócio " + objPayment.NOME + " (Nº: " + objPayment.SOCIO + ") adicionado com sucesso.";
                        MessageBox.Show(sInfo, "Pagamento" + " adicionada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return true;
                    }
                    else if (lOpStatus == -1)
                    {
                        String sError = "Ocorreu um erro na introdução do " + "Pagamento" + " do Sócio " + objPayment.NOME + " (Nº: " + objPayment.SOCIO + ")!";
                        MessageBox.Show(sError, "Erro Criação Sócio", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        if (!SetPayDetails(objPayment, Library_AMFC_Methods.OperationType.ADD))
                        {
                            Clear_Details(true, true);
                            XtraMessageBox.Show("Não foi possível obter carregar detalhes d do " + "Pagamento" + "  !", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        return false;
                    }
                    else if (lOpStatus == 0)
                    {
                        TextEdit_DESIGNACAO.Focus();
                        if (!SetPayDetails(objPayment, Library_AMFC_Methods.OperationType.ADD))
                        {
                            Clear_Details(true, true);
                            XtraMessageBox.Show("Carregar Detalhes d do " + "Pagamento" + "  ", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        return false; //Ja existe
                    }
                    else if (lOpStatus == -2) //Pagamento em aberto
                    {
                        AMFCCashPayment objMemberPayOpen = obj_AMFC_SQL.Get_Member_Payment_Open(objPayment.SOCIO);
                        if (objMemberPayOpen != null && objMemberPayOpen.ID > 0)
                            Load_Payment_Details(objPayment);
                        return false;
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return false;
            }
        }

        #endregion  Payment Add Pay

        #region     Payment Edit Pay

        /// <versions>26-11-2017(GesAMFC-v0.0.4.37)</versions>
        public void Pay_Edit_Action()
        {
            try
            {
                DialogResult objDialogResult = XtraMessageBox.Show("Deseja editar o " + "Pagamento" + "?", "Editar " + "Pagamento" + "? ", MessageBoxButtons.OK, MessageBoxIcon.Question);
                if (objDialogResult == DialogResult.OK)
                    Button_Edit_Pay_Action();
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return;
            }
        }

        /// <versions>26-11-2017(GesAMFC-v0.0.4.37)</versions>
        public AMFCCashPayment GetPayDetailsToEdit()
        {
            try
            {
                AMFCCashPayment objMemberPay = new AMFCCashPayment();

                if (!String.IsNullOrEmpty(TextEdit_ID.Text.Trim()) && Convert.ToInt64(TextEdit_ID.Text.Trim()) > 0)
                    objMemberPay.ID = Convert.ToInt64(TextEdit_ID.Text.Trim());

                if (!String.IsNullOrEmpty(TextEdit_SOCIO.Text.Trim()) && Convert.ToInt64(TextEdit_SOCIO.Text.Trim()) > 0 && Convert.ToInt64(TextEdit_SOCIO.Text.Trim()) < new AMFCMember().MaxNumber)
                    objMemberPay.SOCIO = Convert.ToInt64(TextEdit_SOCIO.Text.Trim());

                if (Program.IsValidTextString(TextEdit_NOME.Text))
                    objMemberPay.NOME = TextEdit_NOME.Text.Trim();

                if (Program.IsValidCurrencyEuroValue(TextEdit_VALOR_Total.Text.Trim()))
                    objMemberPay.VALOR = Program.SetPayCurrencyEuroDoubleValue(TextEdit_VALOR_Total.Text.Trim());

                objMemberPay.DATA = Program.SetDateTimeValue(DateEdit_DATA.DateTime, -1, -1);

                if (Program.IsValidDateTime(DatetEdit_ALTERADO.DateTime))
                    objMemberPay.ALTERADO = Program.ConvertToValidDateTime(DatetEdit_ALTERADO.Text.Trim());
                else
                    objMemberPay.ALTERADO = Program.Default_Date;

                if (Program.IsValidTextString(TextEdit_NOME_PAG.Text))
                    objMemberPay.NOME_PAG = TextEdit_NOME_PAG.Text.Trim();
                else if (Program.IsValidTextString(TextEdit_NOME.Text))
                    objMemberPay.NOME_PAG = TextEdit_NOME.Text.Trim();

                if (Program.IsValidTextString(TextEdit_DESIGNACAO.Text))
                    objMemberPay.DESIGNACAO = TextEdit_DESIGNACAO.Text.Trim();

                if (ComboBoxEdit_ESTADO.SelectedIndex > -1)
                    objMemberPay.Payment_State = objMemberPay.GetPaymentStateType(ComboBoxEdit_ESTADO.SelectedIndex + 1);

                #region     JOIA
                if (Program.IsValidCurrencyEuroValue(TextEdit_JOIAVAL.Text.Trim()))
                {
                    objMemberPay.HasJOIA = true;
                    if (Program.IsValidTextString(TextEdit_JOIADESC.Text))
                        objMemberPay.JOIADESC = TextEdit_JOIADESC.Text;
                    objMemberPay.SetItemPayment(AMFCCashPayment.PaymentTypes.JOIA, Program.SetPayCurrencyEuroDoubleValue(TextEdit_JOIAVAL.Text.Trim()), false, false);
                    objMemberPay.DASSOCJOIA = Program.SetDateTimeValue(DateEdit_DASSOCJOIA.DateTime, -1, -1);
                }
                #endregion  JOIA

                #region     QUOTAS
                if (Program.IsValidCurrencyEuroValue(TextEdit_QUOTASVAL.Text.Trim()))
                {
                    objMemberPay.HasQUOTAS = true;
                    if (Program.IsValidTextString(TextEdit_QUOTASDESC.Text))
                        objMemberPay.QUOTASDESC = TextEdit_QUOTASDESC.Text;
                    objMemberPay.SetItemPayment(AMFCCashPayment.PaymentTypes.QUOTAS, Program.SetPayCurrencyEuroDoubleValue(TextEdit_QUOTASVAL.Text.Trim()), false, false);
                    objMemberPay.DASSOCQUOT = Program.SetDateTimeValue(DateEdit_DASSOCQUOT.DateTime, -1, -1);
                }
                #endregion  QUOTAS

                #region     INFRAESTRUTURAS
                if (Program.IsValidCurrencyEuroValue(TextEdit_INFRAVAL.Text.Trim()))
                {
                    objMemberPay.HasINFRAEST = true;
                    if (Program.IsValidTextString(TextEdit_INFRADESC.Text))
                        objMemberPay.INFRADESC = TextEdit_INFRADESC.Text;
                    objMemberPay.SetItemPayment(AMFCCashPayment.PaymentTypes.INFRAS, Program.SetPayCurrencyEuroDoubleValue(TextEdit_INFRAVAL.Text.Trim()), false, false);
                    objMemberPay.DASSOCINFR = Program.SetDateTimeValue(DateEdit_DASSOCINFR.DateTime, -1, -1);
                }
                #endregion  INFRAESTRUTURAS

                #region     CEDENCIAS
                if (Program.IsValidCurrencyEuroValue(TextEdit_CEDENCVAL.Text.Trim()))
                {
                    objMemberPay.HasCEDENCIAS = true;
                    if (Program.IsValidTextString(TextEdit_CEDENCDESC.Text))
                        objMemberPay.CEDENCDESC = TextEdit_CEDENCDESC.Text;
                    objMemberPay.SetItemPayment(AMFCCashPayment.PaymentTypes.CEDENC, Program.SetPayCurrencyEuroDoubleValue(TextEdit_CEDENCVAL.Text.Trim()), false, false);
                    objMemberPay.DASSOCCEDE = Program.SetDateTimeValue(DateEdit_DASSOCCEDE.DateTime, -1, -1);
                }
                #endregion  CEDENCIAS

                #region     ESGOT
                if (Program.IsValidCurrencyEuroValue(TextEdit_ESGOTVAL.Text.Trim()))
                {
                    objMemberPay.HasESGOT = true;
                    if (Program.IsValidTextString(TextEdit_ESGOTDESC.Text))
                        objMemberPay.ESGOTDESC = TextEdit_ESGOTDESC.Text;
                    objMemberPay.SetItemPayment(AMFCCashPayment.PaymentTypes.ESGOT, Program.SetPayCurrencyEuroDoubleValue(TextEdit_ESGOTVAL.Text.Trim()), false, false);
                    objMemberPay.DASSOCESGO = Program.SetDateTimeValue(DateEdit_DASSOCESGO.DateTime, -1, -1);
                }
                #endregion  ESGOT

                #region     RECONVERSAO URBANISTICA
                if (Program.IsValidCurrencyEuroValue(TextEdit_RECONDESC.Text.Trim()))
                {
                    objMemberPay.HasRECONV = true;
                    if (Program.IsValidTextString(TextEdit_RECONDESC.Text))
                        objMemberPay.RECONDESC = TextEdit_RECONDESC.Text;
                    objMemberPay.SetItemPayment(AMFCCashPayment.PaymentTypes.RECONV, Program.SetPayCurrencyEuroDoubleValue(TextEdit_RECONVAL.Text.Trim()), false, false);
                    objMemberPay.DASSOCRECO = Program.SetDateTimeValue(DateEdit_DASSOCRECO.DateTime, -1, -1);
                }
                #endregion  RECONVERSAO URBANISTICA

                #region     OUTROS
                if (Program.IsValidCurrencyEuroValue(TextEdit_OUTROSVAL.Text.Trim()))
                {
                    objMemberPay.HasOUTRO = true;
                    if (Program.IsValidTextString(TextEdit_OUTROSDESC.Text))
                        objMemberPay.OUTROSDESC = TextEdit_OUTROSDESC.Text;
                    objMemberPay.SetItemPayment(AMFCCashPayment.PaymentTypes.OUTRO, Program.SetPayCurrencyEuroDoubleValue(TextEdit_OUTROSVAL.Text.Trim()), false, false);
                    objMemberPay.DASSOCOUTR = Program.SetDateTimeValue(DateEdit_DASSOCOUTR.DateTime, -1, -1);
                }
                #endregion  OUTROS

                return objMemberPay;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return null;
            }
        }

        /// <versions>26-11-2017(GesAMFC-v0.0.4.37)</versions>
        public AMFCCashPayment GetPayDetailsEdited()
        {
            return GetCurrentPayDetails();
        }

        /// <versions>26-11-2017(GesAMFC-v0.0.4.37)</versions>
        public void Button_Edit_Pay_Action()
        {
            try
            {
                #region     Check Selected Pay
                if (this.Grid_View.FocusedRowHandle < 0)
                {
                    String sInfo = "Selecione um " + "Pagamento" + "  na grelha para editar!";
                    MessageBox.Show(sInfo, "Pagamento" + " não selecionado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                #endregion  Check Selected Pay


                #region     Set Controls to Edit
                Set_Editable_Details(true, true);
                LayoutControlGroup_Pay_Admin.Visibility         = LayoutVisibility.Never;
                LayoutControlItem_Pay_Add.Visibility            = LayoutVisibility.Never;
                LayoutControlItem_Pay_Edit.Visibility           = LayoutVisibility.Never;
                LayoutControlItem_Pay_Del.Visibility            = LayoutVisibility.Never;
                //LayoutControlGroup_Receipt_Buttons.Visibility   = LayoutVisibility.Never;
                LayoutControlGroup_Pay_Add.Visibility           = LayoutVisibility.Never;
                LayoutControlGroup_Pay_Edit.Visibility          = LayoutVisibility.Always;
                #endregion  Set Controls to Edit

                #region     Get Member Info to Edit
                AMFCCashPayment objPayment = GetPayDetailsToEdit();
                if (objPayment == null)
                {
                    String sError = "Ocorreu um erro a obter os dados do " + "Pagamento" + " do Sócio " + objPayment.NOME + " (Nº: " + objPayment.SOCIO + ")!";
                    MessageBox.Show(sError, "Erro Criação Sócio", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                #endregion  Get Member Pay Info to Edit

                #region     Set Member Pay Info to Edit
                if (!SetPayDetails(objPayment, Library_AMFC_Methods.OperationType.EDIT))
                {
                    Clear_Details(true, true);
                    XtraMessageBox.Show("Não foi possível obter carregar detalhes do " + "Pagamento" + "!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                #endregion  Set Member Pay Info to Edit
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return;
            }
        }

        /// <versions>26-11-2017(GesAMFC-v0.0.4.37)</versions>
        public void Button_Edit_Save_Click()
        {
            try
            {
                Set_Editable_Details(false, false);
                LayoutControlGroup_Pay_Admin.Visibility         = LayoutVisibility.Never;
                LayoutControlItem_Pay_Add.Visibility            = LayoutVisibility.Never;
                LayoutControlItem_Pay_Edit.Visibility           = LayoutVisibility.Never;
                LayoutControlItem_Pay_Del.Visibility            = LayoutVisibility.Never;
                //LayoutControlGroup_Receipt_Buttons.Visibility   = LayoutVisibility.Never;
                LayoutControlGroup_Pay_Add.Visibility           = LayoutVisibility.Never;
                LayoutControlGroup_Pay_Edit.Visibility          = LayoutVisibility.Never;

                #region     Member Pay Info
                AMFCCashPayment objMemberPay = GetPayDetailsEdited();
                if (objMemberPay == null)
                {
                    String sError = "Ocorreu um erro a obter os dados do " + "Pagamento" + " do Sócio " + objMemberPay.NOME + " (Nº: " + objMemberPay.SOCIO + ")!";
                    MessageBox.Show(sError, "Erro Editar " + "Pagamento", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                #endregion  Member Pay Info

                #region     Validate Data 
                if (CheckPayIsValid(objMemberPay, true) < 1)
                    return;
                #endregion  Validate Data

                #region     Edit Operation
                if (!DBF_AMFC_Edit_Pay(objMemberPay))
                    return;
                #endregion  Edit Operation

                Set_Editable_Details(false, false);
                LayoutControlGroup_Pay_Admin.Visibility         = LayoutVisibility.Never;
                LayoutControlItem_Pay_Add.Visibility            = LayoutVisibility.Never;
                LayoutControlItem_Pay_Edit.Visibility           = LayoutVisibility.Never;
                LayoutControlItem_Pay_Del.Visibility            = LayoutVisibility.Never;
                //LayoutControlGroup_Receipt_Buttons.Visibility   = LayoutVisibility.Never;
                LayoutControlGroup_Pay_Add.Visibility           = LayoutVisibility.Never;
                LayoutControlGroup_Pay_Edit.Visibility          = LayoutVisibility.Never;

                Load_Grid(false, false, false, false, true, -1, objMemberPay.ID, objMemberPay.SOCIO, new DateTime());
                Load_Details(objMemberPay.ID);
                this.Grid_View.Focus();
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return;
            }
        }

        /// <versions>26-11-2017(GesAMFC-v0.0.4.37)</versions>
        public void Button_Edit_Cancel_Click()
        {
            try
            {
                AMFCCashPayment objPayment = GetPayDetailsToAdd(this.PaymentMember);
                if (objPayment == null || objPayment.SOCIO < 1 || objPayment.SOCIO > new AMFCMember().MaxNumber || objPayment.ID < 1)
                {
                    Clear_Details(true, true);
                    XtraMessageBox.Show("Não foi possível obter o " + "Pagamento" + " selecionado!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Load_Grid(false, false, false, false, true, -1, -1, -1, new DateTime());
                    return;
                }
                Reload_Pay_Details(objPayment, Library_AMFC_Methods.OperationType.EDIT);
                Set_Editable_Details(false, false);

                LayoutControlGroup_Pay_Admin.Visibility         = LayoutVisibility.Never;
                LayoutControlItem_Pay_Add.Visibility            = LayoutVisibility.Never;
                LayoutControlItem_Pay_Edit.Visibility           = LayoutVisibility.Never;
                LayoutControlItem_Pay_Del.Visibility            = LayoutVisibility.Never;
                //LayoutControlGroup_Receipt_Buttons.Visibility   = LayoutVisibility.Never;
                LayoutControlGroup_Pay_Add.Visibility           = LayoutVisibility.Never;
                LayoutControlGroup_Pay_Edit.Visibility          = LayoutVisibility.Never;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return;
            }
        }

        /// <versions>26-11-2017(GesAMFC-v0.0.4.37)</versions>
        public void Button_Edit_Repor_Click(AMFCCashPayment CurrentPayment)
        {
            try
            {
                Reload_Pay_Details(CurrentPayment, Library_AMFC_Methods.OperationType.EDIT);
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return;
            }
        }

        /// <versions>26-11-2017(GesAMFC-v0.0.4.37)</versions>
        public Boolean DBF_AMFC_Edit_Pay(AMFCCashPayment objMemberPay)
        {
            try
            {
                using (Library_AMFC_SQL obj_AMFC_SQL = new Library_AMFC_SQL())
                {
                    Int64 lOpStatus = -1;
                    lOpStatus = obj_AMFC_SQL.Edit_Payment(objMemberPay);
                    if (lOpStatus == 1)
                    {
                        String sInfo = "Pagamento" + " do Sócio " + objMemberPay.NOME + " (Nº: " + objMemberPay.SOCIO + ") alterado com sucesso.";
                        MessageBox.Show(sInfo, "Pagamento" + " editada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return true;
                    }
                    else if (lOpStatus == -1)
                    {
                        String sError = "Ocorreu um erro na alteração do " + "Pagamento" + " do Sócio " + objMemberPay.NOME + " (Nº: " + objMemberPay.SOCIO + ")!";
                        MessageBox.Show(sError, "Erro Criação Sócio", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        if (!SetPayDetails(objMemberPay, Library_AMFC_Methods.OperationType.EDIT))
                        {
                            Clear_Details(true, true);
                            XtraMessageBox.Show("Não foi possível obter carregar detalhes d do " + "Pagamento" + "  !", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        return false;
                    }
                    else if (lOpStatus == 0)
                    {
                        if (!SetPayDetails(objMemberPay, Library_AMFC_Methods.OperationType.EDIT))
                        {
                            Clear_Details(true, true);
                            XtraMessageBox.Show("Carregar Detalhes d do " + "Pagamento" + "  ", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        return false; //Ja existe
                    }
                    else if (lOpStatus == -2) //Pagamento em aberto
                    {
                        AMFCCashPayment objMemberPayOpen = obj_AMFC_SQL.Get_Member_Payment_Open(objMemberPay.SOCIO);
                        if (objMemberPayOpen != null && objMemberPayOpen.ID > 0)
                            Load_Payment_Details(objMemberPayOpen);
                        return false;
                    }
                    return true;
                }
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return false;
            }
        }

        #endregion  Payment Edit Pay

        #region     Payment Del Pay

        /// <versions>26-11-2017(GesAMFC-v0.0.4.37)</versions>
        public void Button_Del_Click()
        {
            try
            {
                #region     Get Member Pay ID
                Int64 lPayId = -1;
                if (Convert.ToInt64(TextEdit_ID.Text.Trim()) > 0)
                    lPayId = Convert.ToInt64(TextEdit_ID.Text.Trim());
                if (lPayId < 1)
                {
                    String sError = "Nº de " + "Pagamento" + "  inválido: " + lPayId;
                    MessageBox.Show(sError, "Nº Inválido", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                #endregion  Get Member Pay ID
                
                #region     Del Confirmation
                DialogResult objDialogResult = XtraMessageBox.Show("Deseja eliminar  do " + "Pagamento" + " " + "Nº: " + lPayId + " ?", "Eliminar " + "Pagamento" + " ?", MessageBoxButtons.OK, MessageBoxIcon.Question);
                if (objDialogResult != DialogResult.OK)
                    return;
                #endregion  Del Confirmation

                #region     Delete Operation
                if (!DBF_AMFC_Del_Pay(lPayId))
                {
                    String sError = "Nº de " + "Pagamento" + "  inválido: " + lPayId;
                    MessageBox.Show(sError, "Nº Inválido", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                #endregion  Delete Operation

                Load_Grid(false, false, false, false, true, -1, -1, -1, new DateTime());
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return;
            }
        }

        /// <versions>26-11-2017(GesAMFC-v0.0.4.37)</versions>
        public Boolean DBF_AMFC_Del_Pay(Int64 lPayId)
        {
            try
            {
                Int64 lOpStatus = -1;
                using (Library_AMFC_SQL obj_AMFC_SQL = new Library_AMFC_SQL())
                    lOpStatus = obj_AMFC_SQL.Del_Payment(lPayId);
                if (lOpStatus == 1)
                {
                    String sInfo = "Nº de " + "Pagamento" + "  = " + lPayId + " eliminado com sucesso.";
                    MessageBox.Show(sInfo, "Pagamento" + " Eliminado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return true;
                }
                else
                {
                    String sError = "Ocorreu um erro na eliminação  do " + "Pagamento" + "  Nº = " + lPayId + "!";
                    MessageBox.Show(sError, "Erro Eliminação  do " + "Pagamento", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Clear_Details(true, true);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return false;
            }
        }

        #endregion  Payment Del Pay

        #region     Payment Details Methods

        /// <versions>26-11-2017(GesAMFC-v0.0.4.37)</versions>
        public void Set_Desc_JOIA()
        {
            try
            {
                if (Program.IsValidTextString(TextEdit_JOIADESC.Text))
                    TextEdit_JOIADESC.Text = "Pagamento da Joia";
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, false, false);
            }
        }

        /// <versions>26-11-2017(GesAMFC-v0.0.4.37)</versions>
        public void Set_Date_JOIA()
        {
            try
            {
                if (!Program.IsValidDateTime(DateEdit_DASSOCJOIA.DateTime))
                    DateEdit_DASSOCJOIA.DateTime = Program.Default_Date;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>26-11-2017(GesAMFC-v0.0.4.37)</versions>
        public AMFCCashPayment Set_Value_JOIA(AMFCCashPayment objMemberPay, Boolean bCheckTotals, Boolean bSetTotal)
        {
            try
            {
                if (!Program.IsValidCurrencyEuroValue(TextEdit_JOIAVAL.Text.Trim()))
                    return null;

                if (objMemberPay == null)
                    objMemberPay = new AMFCCashPayment();

                if (Program.IsValidCurrencyEuroValue(TextEdit_JOIAVAL.Text.Trim()))
                    objMemberPay.VALOR = Program.SetPayCurrencyEuroDoubleValue(TextEdit_VALOR_Total.Text.Trim());

                objMemberPay.SetItemPayment(AMFCCashPayment.PaymentTypes.JOIA, Program.SetPayCurrencyEuroDoubleValue(TextEdit_JOIAVAL.Text.Trim()), true, false);
                if (!Program.IsValidCurrencyEuroValue(TextEdit_VALOR_Total.Text.Trim()))
                    TextEdit_VALOR_Total.Text = Program.SetPayCurrencyEuroStringValue(objMemberPay.VALOR);

                if (bSetTotal)
                {
                    Double dTotal_VALOR = objMemberPay.GetAllPaymentsTotal();
                    TextEdit_VALOR_Total.Text = Program.SetPayCurrencyEuroStringValue(dTotal_VALOR);
                }
                return objMemberPay;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return null;
            }
        }

        #endregion  Payment Details Methods

        #endregion  Payment Action Buttons Methods

        #region Payment Close Methdos
        
        /// <versions>26-11-2017(GesAMFC-v0.0.4.37)</versions>
        public Boolean DBF_AMFC_Set_Entities_Payment_Closed(AMFCCashPayment objPayment)
        {
            try
            {
                Int64 lSocioNumber = -1;
                if (this.PaymentMember != null && this.PaymentMember.NUMERO > 0 && this.PaymentMember.NUMERO < this.PaymentMember.MaxNumber)
                    lSocioNumber = this.PaymentMember.NUMERO;
                else if (objPayment.SOCIO > 0)
                    lSocioNumber = objPayment.SOCIO;
                else
                    lSocioNumber = Convert.ToInt64(TextEdit_SOCIO.Text.Trim());

                using (Library_AMFC_SQL obj_AMFC_SQL = new Library_AMFC_SQL())
                {
                    #region     JOIAS
                    AMFCMemberJoias objListJoias = obj_AMFC_SQL.Get_Joias_Pay_Open(lSocioNumber);
                    if (objListJoias != null || objListJoias.Joias != null)
                    {
                        foreach (AMFCMemberJoia objJoia in objListJoias.Joias)
                        {
                            if (objPayment.ID < 1 || !objJoia.ContainsCaixaId(objPayment.ID))
                                return false;

                            objJoia.ValuePaid = objPayment.JOIAVAL;
                            objJoia.Pay_State = AMFCMemberJoia.PayState.SIM;

                            if (!DBF_AMFC_Close_Pay_JOIA(objJoia, false))
                                return false;
                        }
                    }
                    #endregion  JOIAS

                    #region     QUOTAS
                    AMFC_Entities objListQuotas = obj_AMFC_SQL.Get_QUOTAS_Pay_Open(lSocioNumber, -1);
                    if (objListQuotas != null || objListQuotas.Entidades != null)
                    {
                        foreach (AMFC_Entity objQuota in objListQuotas.Entidades)
                        {
                            if (objPayment.ID < 1 || !objQuota.ContainsCaixaId(objPayment.ID))
                                return false;

                            objQuota.ValuePaid = objQuota.Value;
                            objQuota.Pay_State = AMFC_Entity.PayState.SIM;

                            if (!DBF_AMFC_Close_Pay_QUOTA(objQuota, false))
                                return false;
                        }
                    }
                    #endregion  QUOTAS

                    return true;
                }
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return false;
            }
        }

        /// Colocar no Program
        /// <versions>03-12-2017(GesAMFC-v0.0.4.41)</versions>
        private Boolean DBF_AMFC_Close_Pay_JOIA(AMFCMemberJoia objMemberJoia, Boolean bShowMessageDialog)
        {
            try
            {
                Int64 lOpStatus = -1;
                using (Library_AMFC_SQL obj_AMFC_SQL = new Library_AMFC_SQL())
                {
                    lOpStatus = obj_AMFC_SQL.Edit_Joia(objMemberJoia);
                    if (lOpStatus == 1)
                    {
                        if (bShowMessageDialog)
                        {
                            String sInfo = "O Pagamento da " + "Joia" + " do Sócio " + objMemberJoia.MemberName + " (Nº: " + objMemberJoia.MemberNumber + ") foi concluído com sucesso.";
                            MessageBox.Show(sInfo, "Pagamento" + " da " + "Joia" + " " + "Fechado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return false;
            }
        }

        /// Colocar no Program
        /// <versions>03-12-2017(GesAMFC-v0.0.4.41)</versions>
        private Boolean DBF_AMFC_Close_Pay_QUOTA(AMFC_Entity objEntity, Boolean bShowMessageDialog)
        {
            try
            {
                Int64 lOpStatus = -1;
                using (Library_AMFC_SQL obj_AMFC_SQL = new Library_AMFC_SQL())
                {
                    lOpStatus = obj_AMFC_SQL.Edit_QUOTA(objEntity);
                    if (lOpStatus == 1)
                    {
                        if (bShowMessageDialog)
                        {
                            String sInfo = "O Pagamento da " + Program.Entity_QUOTA_Desc_Single + " do Sócio " + objEntity.MemberName + " (Nº: " + objEntity.MemberNumber + ") foi concluído com sucesso.";
                            MessageBox.Show(sInfo, "Pagamento" + " da " + Program.Entity_QUOTA_Desc_Single + " " + "Fechado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        return true;
                    }
                    return false;
                }
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return false;
            }
        }

        #endregion  Payment Close Methdos

        #endregion  Methods

        /// <versions>06-12-2017(GesAMFC-v0.0.5.0)</versions>
        private void Button_Create_Receipt_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.Payment == null || this.Payment.SOCIO < 1 || this.Payment.SOCIO > new AMFCMember().MaxNumber || this.Payment.ID < 1)
                {
                    if (this.Grid_View.FocusedRowHandle > 0)
                    {
                        this.Payment = Get_Selected(this.Grid_View.FocusedRowHandle, -1);
                        if (this.Payment == null || this.Payment.SOCIO < 1 || this.Payment.SOCIO > new AMFCMember().MaxNumber || this.Payment.ID < 1)
                        {
                             XtraMessageBox.Show("Não foi possível obter os detalhes do " + "Pagamento" + " selecionado!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Load_Grid(false, false, false, false, true, -1, -1, -1, new DateTime());
                            return;
                        }
                    }
                    else
                        return;
                }

                this.PaymentMember = Get_DBF_AMFC_Member_By_Number(this.Payment.SOCIO);
                if (this.PaymentMember == null || this.PaymentMember.NUMERO <= 0 || this.PaymentMember.NUMERO > this.PaymentMember.MaxNumber)
                {
                    XtraMessageBox.Show("Não foi possível obter os Dados do Sócio do " + "Pagamento" + " selecionado!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Load_Grid(false, false, false, false, true, -1, -1, -1, new DateTime());
                    return;
                }

                Form_Payment_Receipt objReceipt = new Form_Payment_Receipt();
                objReceipt.Set_Member_Pay_Receipt();
                if (objReceipt != null)
                {
                    #region     Receipt Properties
                    objReceipt.Receipt_Number               = this.Payment.ID;
                    objReceipt.Receipt_Value                = this.Payment.VALOR;
                    objReceipt.Receipt_Member_Name          = this.PaymentMember.NOME;
                    objReceipt.Receipt_Member_Number        = this.PaymentMember.NUMERO;
                    objReceipt.Receipt_Member_Lote          = this.PaymentMember.NUMLOTE;
                    objReceipt.Receipt_Member_Quantia       = Program.QuantiaToExtenso(this.Payment.VALOR);
                    #region     Get Payment Receipt Designacao
                    String sDesignacao = this.Payment.DESIGNACAO;
                    if (!String.IsNullOrEmpty(this.Payment.JOIADESC.Trim()))
                    {
                        if (this.Payment.Payment_Type == AMFCCashPayment.PaymentTypes.MULTIPLE)
                        {
                            if (sDesignacao.Trim().Length + this.Payment.JOIADESC.Trim().Length <= objReceipt.Receipt_Desginação_Max_Chars)
                                sDesignacao += ": " + this.Payment.JOIADESC.Trim();
                        }
                    }
                    if (!String.IsNullOrEmpty(this.Payment.QUOTASDESC.Trim()))
                    {
                        String sQuotasDesc = this.Payment.QUOTASDESC.Trim().Replace(Program.Entity_QUOTA_Desc_Plural, "").Trim().Trim(':').Trim().Replace(Program.Entity_QUOTA_Desc_Single, "").Trim().Trim(':').Trim();
                        if (sDesignacao.Trim().Length + sQuotasDesc.Length <= objReceipt.Receipt_Desginação_Max_Chars)
                            sDesignacao += ": " + sQuotasDesc;
                    }
                    if (!String.IsNullOrEmpty(this.Payment.INFRADESC.Trim()))
                    {
                        if (sDesignacao.Trim().Length + this.Payment.INFRADESC.Trim().Length <= objReceipt.Receipt_Desginação_Max_Chars)
                            sDesignacao += ": " + this.Payment.INFRADESC.Trim();
                    }
                    if (!String.IsNullOrEmpty(this.Payment.CEDENCDESC.Trim()))
                    {
                        if (sDesignacao.Trim().Length + this.Payment.CEDENCDESC.Trim().Length <= objReceipt.Receipt_Desginação_Max_Chars)
                            sDesignacao += ": " + this.Payment.CEDENCDESC.Trim();
                    }
                    if (!String.IsNullOrEmpty(this.Payment.ESGOTDESC.Trim()))
                    {
                        if (sDesignacao.Trim().Length + this.Payment.ESGOTDESC.Trim().Length <= objReceipt.Receipt_Desginação_Max_Chars)
                            sDesignacao += ": " + this.Payment.ESGOTDESC.Trim();
                    }
                    if (!String.IsNullOrEmpty(this.Payment.RECONDESC.Trim()))
                    {
                        if (sDesignacao.Trim().Length + this.Payment.RECONDESC.Trim().Length <= objReceipt.Receipt_Desginação_Max_Chars)
                            sDesignacao += "; " + this.Payment.RECONDESC.Trim();
                    }
                    if (!String.IsNullOrEmpty(this.Payment.OUTROSDESC.Trim()))
                    {
                        if (sDesignacao.Trim().Length + this.Payment.OUTROSDESC.Trim().Length <= objReceipt.Receipt_Desginação_Max_Chars)
                            sDesignacao += ": " + this.Payment.OUTROSDESC.Trim();
                    }
                    #endregion  Get Payment Receipt Designacao
                    objReceipt.Receipt_Member_Designacao    = sDesignacao;
                    objReceipt.Receipt_Member_Data          = DateTime.Today.ToString("D", CultureInfo.CurrentCulture);
                    #endregion  Receipt Properties

                    objReceipt.Set_Member_Pay_Receipt();

                    objReceipt.FormClosing += delegate
                    {
                        //...
                    };
                    objReceipt.Show();
                    objReceipt.StartPosition = FormStartPosition.CenterParent;
                    objReceipt.Focus();
                    objReceipt.BringToFront();
                    objReceipt.Update();
                }
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);

            }
        }

        /// <versions>06-12-2017(GesAMFC-v0.0.5.0)</versions>
        private AMFCMember Get_DBF_AMFC_Member_By_Number(Int64 lMemberNumber)
        {
            try
            {
                using (Library_AMFC_SQL obj_AMFC_SQL = new Library_AMFC_SQL())
                {
                    return obj_AMFC_SQL.Get_Member_By_Number(lMemberNumber);
                }
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return null;
            }
        }

        /// <versions>23-03-2018(GesAMFC-v1.0.0.3)</versions>
        private void Button_Folha_Caixa_Click(object sender, EventArgs e)
        {
            Load_Year_Month_Folha_Caixa();
        }

        /// <versions>23-03-2018(GesAMFC-v1.0.0.3)</versions>
        //private void Load_Year_Month_Folha_Caixa()
        //{
        //    try
        //    {
        //        DateTime dtCurrentDate = DateTime.Today;
        //        Form_Year_Month objYearMonth = new Form_Year_Month(new AMFCYear(dtCurrentDate.Year), new AMFCMonth(dtCurrentDate.Month));
        //        if (objYearMonth != null)
        //        {
        //            objYearMonth.FormClosing += delegate
        //            {
        //                if (objYearMonth.IsYearMonthSelected)
        //                {
        //                    if (Program.IsValidYear(objYearMonth.YearSelected.Value) && Program.IsValidMonth(objYearMonth.MonthSelected.Value))
        //                    {
        //                        //if (objYearMonth.EveryYear)
        //                        //{
        //                        //    this.YearSelected = objYearMonth.YearSelected;
        //                        //    this.Grid_Datasouce_Type = GridDatasourceType.YEAR;
        //                        //}
        //                        //else
        //                        //{
        //                        //    this.YearSelected = objYearMonth.YearSelected;
        //                        //    this.MonthSelected = objYearMonth.MonthSelected;
        //                        //    this.Grid_Datasouce_Type = GridDatasourceType.YEARMONTH;
        //                        //}
        //                        //this.Entidade_Member = new AMFCMember();
        //                        //Load_Grid(true, true, true, true, true, -1, -1, -1, dtFolhaCaixaDate);
        //                        //this.LoadingGridPanelWaitTime = 500;

        //                        Create_Folha_Caixa(objYearMonth.YearSelected.Value, objYearMonth.MonthSelected.Value);
        //                    }
        //                }
        //            };
        //            objYearMonth.Show();
        //            objYearMonth.StartPosition = FormStartPosition.CenterParent;
        //            objYearMonth.Focus();
        //            objYearMonth.BringToFront();
        //            objYearMonth.Update();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
        //    }
        //}

        private void Load_Year_Month_Folha_Caixa()
        {
            try
            {
                DateTime dtFolhaCaixaDate = DateTime.Today;
                if (Program.IsValidDateTime(this.DateEdit_Folha_Caixa.DateTime))
                    dtFolhaCaixaDate = this.DateEdit_Folha_Caixa.DateTime;

                Load_Grid(true, true, true, true, true, -1, -1, -1, dtFolhaCaixaDate);

                Create_Folha_Caixa(dtFolhaCaixaDate);
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>23-03-2018(GesAMFC-v1.0.0.3)</versions>
        //private void Create_Folha_Caixa(Int32 iYear, Int32 iMonth)
        private void Create_Folha_Caixa(DateTime dtDateSelected)
        {
            try
            {
                //if (!Program.IsValidYear(iYear) || !Program.IsValidMonth(iMonth))
                //    return;
                if (!Program.IsValidDateTime(dtDateSelected))
                    return;
                //Form_Folha_Caixa objFormReciboQuotas = new Form_Folha_Caixa(new AMFCYear(iYear), new AMFCMonth(iMonth));
                Form_Folha_Caixa objFormReciboQuotas = new Form_Folha_Caixa(dtDateSelected);
                if (objFormReciboQuotas != null)
                {
                    objFormReciboQuotas.FormClosing += delegate
                    {

                    };
                    //objFormReciboQuotas.WindowState = FormWindowState.Maximized;
                    objFormReciboQuotas.Show();
                    objFormReciboQuotas.StartPosition = FormStartPosition.CenterParent;
                    objFormReciboQuotas.Focus();
                    objFormReciboQuotas.BringToFront();
                    objFormReciboQuotas.Update();
                }
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return;
            }
        }
    }
}