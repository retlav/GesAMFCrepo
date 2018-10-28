using DevExpress.XtraEditors;
using DevExpress.XtraLayout.Utils;
using GesAMFC.AMFC_Methods;
using System;
using System.Globalization;
using System.Windows.Forms;

namespace GesAMFC
{
    /// <summary>Form Caixa Pagamentos</summary>
    /// <creation>16-06-2017(v0.0.4.1)</creation>
    /// <versions>07-12-2017(GesAMFC-v0.0.5.1)</versions>
    public partial class Form_Caixa : DevExpress.XtraEditors.XtraForm
    {
        public Library_AMFC_Methods LibAMFC;
        public Library_AMFC_SQL Lib_AMFC_SQL;

        public Library_AMFC_Methods.OperationType PaymentOperationType;
        public Library_AMFC_Methods.PaymentOptions EntitiesPaymentType;

        public AMFCMember PaymentMember;
        public AMFCCashPayment  CurrentPayment;
        public Boolean PaymentOk;

        #region     Form Constructor 

        /// <versions>16-06-2017(v0.0.4.1)</versions>
        public Form_Caixa()
        {
            LibAMFC = new Library_AMFC_Methods();
            Lib_AMFC_SQL = new Library_AMFC_SQL();
            try
            {
                this.PaymentOperationType = Library_AMFC_Methods.OperationType.UNDEFINED;
                this.PaymentMember = new AMFCMember();
                this.CurrentPayment = new AMFCCashPayment();
                this.EntitiesPaymentType = Library_AMFC_Methods.PaymentOptions.SINGLE;
                this.PaymentOk = false;
                
                InitializeComponent();
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>25-11-2017(GesAMFC-v0.0.4.37)</versions>
        private void ComboBoxEdit_ESTADO_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                //if (PaymentOperationType == Library_AMFC_Methods.OperationType.ADD || PaymentOperationType == Library_AMFC_Methods.OperationType.EDIT)
                //{
                //    if (ComboBoxEdit_ESTADO.SelectedIndex == 1)
                //    {
                //        DialogResult objResult_MemberMoneyDelivered = XtraMessageBox.Show("O sócio vai entregar neste momento o valor de: " + TextEdit_VALOR_Total.Text + " ?", "Finalizar pagamento ?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                //        if (objResult_MemberMoneyDelivered == DialogResult.No)
                //            ComboBoxEdit_ESTADO.SelectedIndex = 0;
                //    }
                //}
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return;
            }
        }

        #endregion  Form Constructor

        #region     Events

        #region     Form Events

        /// <versions>16-06-2017(v0.0.4.1)</versions>
        private void Form_Caixa_Load(object sender, EventArgs e)
        {
            try
            {
                this.StartPosition = FormStartPosition.CenterParent;
                this.Focus();
                this.Update();
                this.CenterToParent();
                this.PaymentOk = false;

                SetSingleMultipleVisibleOptions(CurrentPayment.Payment_Type, EntitiesPaymentType);
                SetSingleMultipleEnabledOptions(AMFCCashPayment.PaymentTypes.MULTIPLE, Library_AMFC_Methods.PaymentOptions.MULTIPLE);

                if (EntitiesPaymentType == Library_AMFC_Methods.PaymentOptions.SINGLE)
                    this.Size = new System.Drawing.Size(700, 550);
                else
                    this.Size = new System.Drawing.Size(700, 850);

                switch (PaymentOperationType)
                {
                    case Library_AMFC_Methods.OperationType.OPEN:
                        Button_Open_Pay_Action(PaymentMember, CurrentPayment, Library_AMFC_Methods.OperationType.OPEN);
                        break;
                    case Library_AMFC_Methods.OperationType.ADD:
                        Button_Add_Pay_Action(PaymentMember, CurrentPayment);
                        break;
                    case Library_AMFC_Methods.OperationType.EDIT:
                        Button_Edit_Pay_Action();
                        break;
                    default:
                        //Dialog erro:
                        return;
                }
                
                Load_Payment_Details(CurrentPayment);

                #region     Set DateEdits Calendars
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
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        #endregion  Form Events

        #region     Buttons Events

        /// <versions>29-10-2017(GesAMFC-v0.0.4.29)</versions>
        private void Button_Pay_Add_Save_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Button_Add_Save_Click())
                    return;
                this.PaymentOk = true;
                this.Close();
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return;
            }
        }

        /// <versions>29-10-2017(GesAMFC-v0.0.4.29)</versions>
        private void Button_Pay_Add_Cancel_Click(object sender, EventArgs e)
        {
            try
            {
                Button_Add_Cancel_Click();
                this.Close();
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return;
            }
        }

        /// <versions>29-10-2017(GesAMFC-v0.0.4.29)</versions>
        private void Button_Pay_Add_Repor_Click(object sender, EventArgs e)
        {
            Reload_Pay_Details(CurrentPayment, Library_AMFC_Methods.OperationType.ADD);
        }

        /// <versions>29-10-2017(GesAMFC-v0.0.4.29)</versions>
        private void Button_Pay_Edit_Save_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Button_Edit_Save_Click())
                    return;
                this.PaymentOk = true;
                this.Close();
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return;
            }
        }

        /// <versions>29-10-2017(GesAMFC-v0.0.4.29)</versions>
        private void Button_Pay_Edit_Cancel_Click(object sender, EventArgs e)
        {
            try
            {
                Button_Edit_Cancel_Click(CurrentPayment);
                this.Close();
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return;
            }
        }

        /// <versions>29-10-2017(GesAMFC-v0.0.4.29)</versions>
        private void Button_Pay_Edit_Repor_Click(object sender, EventArgs e)
        {
            Reload_Pay_Details(CurrentPayment, PaymentOperationType);
        }

        /// <versions>26-11-2017(GesAMFC-v0.0.4.37)</versions>
        private void Button_DelOpenPay_Click(object sender, EventArgs e)
        {
            try
            {
                Button_Del_Click();
                this.PaymentOk = false;
                this.CurrentPayment.Payment_State = AMFCCashPayment.PaymentState.CANCELED;
                this.Close();
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return;
            }
        }

        #endregion  Buttons Events

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
                    Set_Value_JOIA(objMemberPay, true, true);
                    Set_Value_JOIA(objMemberPay, true, true);
                }
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, false, false);
            }
        }

        #endregion  Text Box Edits

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

        #endregion  Events

        #region     Payment Methods

        #region     Payment Entities Visibility

        /// <versions>26-11-2017(GesAMFC-v0.0.4.37)</versions>
        public Library_AMFC_Methods.PaymentOptions GetMultipleSinglePaymentType()
        {
            try
            {
                Library_AMFC_Methods.PaymentOptions ePayEntType = Get_Payment_Type(CurrentPayment);
                if (ePayEntType == Library_AMFC_Methods.PaymentOptions.MULTIPLE)
                    CurrentPayment.Payment_Type = AMFCCashPayment.PaymentTypes.MULTIPLE;
                return ePayEntType;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return Library_AMFC_Methods.PaymentOptions.SINGLE;
            }
        }

        /// <versions>26-11-2017(GesAMFC-v0.0.4.37)</versions>
        public static Library_AMFC_Methods.PaymentOptions Get_Payment_Type(AMFCCashPayment objPayment)
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
        public void SetVisibility_RECONV( Boolean bVisibility)
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
                    DatetEdit_ALTERADO.DateTime = DateTime.Today;
                
                ComboBoxEdit_ESTADO.SelectedIndex = (objPayment.Payment_State != AMFCCashPayment.PaymentState.UNDEFINED) ? objPayment.GetPaymentStateIdx() : -1;

                if (this.PaymentOperationType == Library_AMFC_Methods.OperationType.OPEN && objPayment.Payment_State == AMFCCashPayment.PaymentState.FINALIZADO)
                    LayoutControlGroup_Receipt_Buttons.Visibility = LayoutVisibility.Always;
                else
                    LayoutControlGroup_Receipt_Buttons.Visibility = LayoutVisibility.Never;

                LayoutControlItem_DelOpenPay.Visibility = (PaymentOperationType == Library_AMFC_Methods.OperationType.EDIT) ? LayoutVisibility.Always : LayoutVisibility.Never;
                
                switch (CurrentPayment.Payment_Type)
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

                TextEdit_VALOR_Total.Text = Program.SetPayCurrencyEuroStringValue(objPayment.VALOR);
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

                LayoutControlItem_DelOpenPay.Visibility = (PaymentOperationType == Library_AMFC_Methods.OperationType.EDIT) ? LayoutVisibility.Always : LayoutVisibility.Never;

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

                #region     CEDENCIAS
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
                #endregion  CEDENCIAS

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
                    objMemberPay.DASSOCINFR = Program.SetDateTimeValue(DateEdit_DASSOCINFR.DateTime, -1, -1);
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

                LayoutControlItem_DelOpenPay.Visibility = (eOpType == Library_AMFC_Methods.OperationType.EDIT) ? LayoutVisibility.Always : LayoutVisibility.Never;

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

                #region     CEDENCIAS
                TextEdit_CEDENCDESC.Text = Program.SetTextString(objMemberPay.CEDENCDESC, eDefaultStringTextType);
                DateEdit_DASSOCCEDE.DateTime = Program.SetDateTimeValue(objMemberPay.DASSOCCEDE, -1, -1);
                TextEdit_CEDENCVAL.Text = Program.SetPayCurrencyEuroStringValue(objMemberPay.CEDENCVAL);
                #endregion  CEDENCIAS

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
                if (eOpType == Library_AMFC_Methods.OperationType.UNDEFINED)
                    return;

                #region     Load Pay Details
                if (CurrentPayment != null)
                {
                    if (!SetPayDetails(CurrentPayment, eOpType))
                    {
                        Clear_Details(true, true);
                        XtraMessageBox.Show("Não foi possível carregar os detalhes do " + "Pagamento" + "!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        /// <versions>27-11-2017(GesAMFC-v0.0.4.37)</versions>
        public Boolean Set_Pay_Close(AMFCCashPayment objPayment, Library_AMFC_Methods.OperationType PaymentOperationType)
        {
            try
            {
                if (PaymentOperationType == Library_AMFC_Methods.OperationType.ADD || PaymentOperationType == Library_AMFC_Methods.OperationType.EDIT)
                {
                    //DialogResult objResult_MemberMoneyDelivered = XtraMessageBox.Show("Deseja finalizar este pagamento? Ou seja, o sócio entregou o valor de: " + TextEdit_VALOR_Total.Text + " ?", "Finalizar pagamento ?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    //if (objResult_MemberMoneyDelivered == DialogResult.Yes)
                        return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return false;
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
                LayoutControlGroup_Pay_Add.Visibility = LayoutVisibility.Never;
                LayoutControlGroup_Pay_Edit.Visibility = LayoutVisibility.Never;
                LayoutControlItem_DelOpenPay.Visibility = (PaymentOperationType == Library_AMFC_Methods.OperationType.EDIT) ? LayoutVisibility.Always : LayoutVisibility.Never;
                LayoutControlGroup_Receipt_Buttons.Visibility = LayoutVisibility.Never;
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
        public void Button_Add_Pay_Action(AMFCMember PaymentMember, AMFCCashPayment CurrentPayment)
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
                Set_Editable_Details(true, false);
                LayoutControlGroup_Pay_Add.Visibility = LayoutVisibility.Always;
                LayoutControlGroup_Pay_Edit.Visibility = LayoutVisibility.Never;
                LayoutControlItem_DelOpenPay.Visibility = (PaymentOperationType == Library_AMFC_Methods.OperationType.EDIT) ? LayoutVisibility.Always : LayoutVisibility.Never;
                LayoutControlGroup_Receipt_Buttons.Visibility = LayoutVisibility.Never;
                #endregion  Set Controls to Add

                #region     Get Member Pay Info to Add
                if (CurrentPayment == null)
                    CurrentPayment = GetPayDetailsToAdd(PaymentMember);
                if (CurrentPayment == null)
                {
                    String sError = "Ocorreu um erro a obter os dados do " + "Pagamento" + " " + CurrentPayment.NOME + " (Nº: " + CurrentPayment.SOCIO + ")!";
                    MessageBox.Show(sError, "Erro Criação Sócio", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                #endregion  Get Member Pay Info to Add

                #region     Set Member Pay Info to Add
                if (!SetPayDetails(CurrentPayment, Library_AMFC_Methods.OperationType.ADD))
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
        public Boolean Button_Add_Save_Click()
        {
            try
            {
                #region     Member Pay Info
                AMFCCashPayment objMemberPay = GetPayDetailsEdited();
                if (objMemberPay == null)
                {
                    String sError = "Ocorreu um erro a obter os dados do " + "Pagamento" + " do Sócio " + objMemberPay.NOME + " (Nº: " + objMemberPay.SOCIO + ")!";
                    MessageBox.Show(sError, "Erro Criação Sócio", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                #endregion  Member Pay Info

                #region     Validate Data 
                if (CheckPayIsValid(objMemberPay, true) < 1)
                    return false;
                #endregion  Validate Data

                #region     Set Pay Close
                if (Set_Pay_Close(objMemberPay, Library_AMFC_Methods.OperationType.ADD))
                {
                    ComboBoxEdit_ESTADO.SelectedIndex = 1;
                    objMemberPay.Payment_State = AMFCCashPayment.PaymentState.FINALIZADO;
                    this.CurrentPayment = objMemberPay;
                    if (!DBF_AMFC_Add_Pay(objMemberPay, Library_AMFC_Methods.OperationType.ADD))
                    {
                        objMemberPay = GetPayDetailsEdited();
                        this.CurrentPayment = objMemberPay;
                        return false;
                    }
                    if (!DBF_AMFC_Set_Entities_Payment_Closed(objMemberPay))
                    {
                        String sError = "Ocorreu um erro no processo de fechar o " + "Pagamento" + " do Sócio " + objMemberPay.NOME + " (Nº: " + objMemberPay.SOCIO + ")!";
                        MessageBox.Show(sError, "Erro Fechar Pagamento", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
                else
                {
                    //ComboBoxEdit_ESTADO.SelectedIndex = 0;
                    //objMemberPay.Payment_State = AMFCCashPayment.PaymentState.ABERTO;
                    ComboBoxEdit_ESTADO.SelectedIndex = 1;
                    objMemberPay.Payment_State = AMFCCashPayment.PaymentState.FINALIZADO;

                    LayoutControlGroup_Receipt_Buttons.Visibility = LayoutVisibility.Never;
                    this.CurrentPayment = objMemberPay;
                    if (!DBF_AMFC_Add_Pay(objMemberPay, Library_AMFC_Methods.OperationType.ADD))
                    {
                        objMemberPay = GetPayDetailsEdited();
                        this.CurrentPayment = objMemberPay;
                        return false;
                    }
                }
                #endregion  Set Pay Close

                Set_Editable_Details(false, false);
                LayoutControlGroup_Pay_Add.Visibility = LayoutVisibility.Never;

                return true;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return false;
            }
        }

        /// <versions>26-11-2017(GesAMFC-v0.0.4.37)</versions>
        public void Button_Add_Cancel_Click()
        {
            try
            {
                Clear_Details(true, true);
                Set_Editable_Details(false, false);
                LayoutControlGroup_Pay_Add.Visibility = LayoutVisibility.Never;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return;
            }
        }

        /// <versions>26-11-2017(GesAMFC-v0.0.4.37)</versions>
        public void Button_Add_Repor_Click(AMFCCashPayment CurrentPayment, Library_AMFC_Methods.OperationType PaymentOperationType)
        {
            Reload_Pay_Details(CurrentPayment, PaymentOperationType);
        }

        /// <versions>06-12-2017(GesAMFC-v0.0.5.0)</versions>
        //public Boolean Add_Pay()
        //{
        //    return DBF_AMFC_Add_Pay(this.CurrentPayment, Library_AMFC_Methods.OperationType.ADD);
        //}

        /// <versions>26-11-2017(GesAMFC-v0.0.4.37)</versions>
        public Boolean DBF_AMFC_Add_Pay(AMFCCashPayment objMemberPay, Library_AMFC_Methods.OperationType PaymentOperationType)
        {
            try
            {
                using (Library_AMFC_SQL obj_AMFC_SQL = new Library_AMFC_SQL())
                {
                    Int64 lOpStatus = -1;
                    lOpStatus = obj_AMFC_SQL.Add_Payment(objMemberPay);
                    if (lOpStatus == 1)
                    {
                        String sInfo = "Pagamento" + " do Sócio " + objMemberPay.NOME + " (Nº: " + objMemberPay.SOCIO + ") adicionado com sucesso.";
                        MessageBox.Show(sInfo, "Pagamento" + " adicionada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        return true;
                    }
                    else if (lOpStatus == -1)
                    {
                        String sError = "Ocorreu um erro na introdução do " + "Pagamento" + " do Sócio " + objMemberPay.NOME + " (Nº: " + objMemberPay.SOCIO + ")!";
                        MessageBox.Show(sError, "Erro Criação Sócio", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        if (!SetPayDetails(objMemberPay, PaymentOperationType))
                        {
                            Clear_Details(true, true);
                            XtraMessageBox.Show("Não foi possível obter carregar detalhes d do " + "Pagamento" + "  !", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        return false;
                    }
                    else if (lOpStatus == 0) //Já efetuado pagamento na mesma data, com os mesmos valores e designação
                    {
                        TextEdit_DESIGNACAO.Focus();
                        //if (!SetPayDetails(objMemberPay, PaymentOperationType))
                        //{
                        //    Clear_Details(true, true);
                        //    XtraMessageBox.Show("Carregar Detalhes do " + "Pagamento" + "  ", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        //}
                        return false; //Ja existe
                    }
                    else if (lOpStatus == -2) //Pagamento em aberto
                    {
                        AMFCCashPayment objMemberPayOpen = obj_AMFC_SQL.Get_Member_Payment_Open(objMemberPay.SOCIO);
                        if (objMemberPayOpen != null && objMemberPayOpen.ID > 0)
                        {
                            CurrentPayment = objMemberPayOpen;
                            Load_Payment_Details(objMemberPay);
                        }
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
        public void Button_Edit_Pay_Action()
        {
            try
            {
                #region     Set Controls to Edit
                Set_Editable_Details(true, true);
                LayoutControlGroup_Pay_Add.Visibility = LayoutVisibility.Never;
                LayoutControlGroup_Pay_Edit.Visibility = LayoutVisibility.Always;
                LayoutControlItem_DelOpenPay.Visibility = (PaymentOperationType == Library_AMFC_Methods.OperationType.EDIT) ? LayoutVisibility.Always : LayoutVisibility.Never;
                LayoutControlGroup_Receipt_Buttons.Visibility = LayoutVisibility.Never;
                #endregion  Set Controls to Edit

                #region     Get Member um " + "Pagamento" + "  Info to Edit
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
        public Boolean Button_Edit_Save_Click()
        {
            try
            {
                Set_Editable_Details(false, false);
                LayoutControlGroup_Pay_Edit.Visibility = LayoutVisibility.Never;
                LayoutControlItem_DelOpenPay.Visibility = (PaymentOperationType == Library_AMFC_Methods.OperationType.EDIT) ? LayoutVisibility.Always : LayoutVisibility.Never;

                #region     Member Pay Info
                AMFCCashPayment objMemberPay = GetPayDetailsEdited();
                if (objMemberPay == null)
                {
                    String sError = "Ocorreu um erro a obter os dados do " + "Pagamento" + " do Sócio " + objMemberPay.NOME + " (Nº: " + objMemberPay.SOCIO + ")!";
                    MessageBox.Show(sError, "Erro Editar " + "Pagamento", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                #endregion  Member Pay Info

                #region     Validate Data 
                if (CheckPayIsValid(objMemberPay, true) < 1)
                    return false;
                #endregion  Validate Data

                #region     Set Pay Close
                if (Set_Pay_Close(objMemberPay, Library_AMFC_Methods.OperationType.EDIT))
                {
                    ComboBoxEdit_ESTADO.SelectedIndex = 1;
                    objMemberPay.Payment_State = AMFCCashPayment.PaymentState.FINALIZADO;
                    this.CurrentPayment = objMemberPay;
                    if (!DBF_AMFC_Edit_Pay(objMemberPay))
                    {
                        objMemberPay = GetPayDetailsEdited();
                        this.CurrentPayment = objMemberPay;
                        return false;
                    }
                    if (!DBF_AMFC_Set_Entities_Payment_Closed(objMemberPay))
                    {
                        String sError = "Ocorreu um erro no processo de fechar o " + "Pagamento" + " do Sócio " + objMemberPay.NOME + " (Nº: " + objMemberPay.SOCIO + ")!";
                        MessageBox.Show(sError, "Erro Fechar Pagamento", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
                else
                {
                    //ComboBoxEdit_ESTADO.SelectedIndex = 0;
                    //objMemberPay.Payment_State = AMFCCashPayment.PaymentState.ABERTO;
                    ComboBoxEdit_ESTADO.SelectedIndex = 1;
                    objMemberPay.Payment_State = AMFCCashPayment.PaymentState.FINALIZADO;

                    LayoutControlGroup_Receipt_Buttons.Visibility = LayoutVisibility.Never;
                    this.CurrentPayment = objMemberPay;
                    if (!DBF_AMFC_Edit_Pay(objMemberPay))
                    {
                        objMemberPay = GetPayDetailsEdited();
                        this.CurrentPayment = objMemberPay;
                        return false;
                    }
                }
                #endregion  Set Pay Close

                Set_Editable_Details(false, false);
                LayoutControlGroup_Pay_Add.Visibility = LayoutVisibility.Never;

                return true;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return false;
            }
        }

        /// <versions>26-11-2017(GesAMFC-v0.0.4.37)</versions>
        public void Button_Edit_Cancel_Click(AMFCCashPayment CurrentPayment)
        {
            try
            {
                Reload_Pay_Details(CurrentPayment, Library_AMFC_Methods.OperationType.EDIT);
                Set_Editable_Details(false, false);
                LayoutControlGroup_Pay_Edit.Visibility = LayoutVisibility.Never;
                LayoutControlItem_DelOpenPay.Visibility = (PaymentOperationType == Library_AMFC_Methods.OperationType.EDIT) ? LayoutVisibility.Always : LayoutVisibility.Never;
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
                        TextEdit_DESIGNACAO.Focus();
                        //if (!SetPayDetails(objMemberPay, Library_AMFC_Methods.OperationType.EDIT))
                        //{
                        //    Clear_Details(true, true);
                        //    XtraMessageBox.Show("Carregar Detalhes d do " + "Pagamento" + "  ", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        //}
                        return false; //Ja existe
                    }
                    else if (lOpStatus == -2) //Pagamento em aberto
                    {
                        AMFCCashPayment objMemberPayOpen = obj_AMFC_SQL.Get_Member_Payment_Open(objMemberPay.SOCIO);
                        if (objMemberPayOpen != null && objMemberPayOpen.ID > 0)
                        {
                            this.CurrentPayment = objMemberPayOpen;
                            Load_Payment_Details(this.CurrentPayment);
                        }
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
                if (TextEdit_JOIADESC.Properties.ReadOnly)
                    return;
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
                if (DateEdit_DASSOCJOIA.Properties.ReadOnly)
                    return;
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
                if (TextEdit_JOIAVAL.Properties.ReadOnly)
                    return null;

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

        public Boolean Set_Entities_Payment_Closed()
        {
            return DBF_AMFC_Set_Entities_Payment_Closed(this.CurrentPayment);
        }

        /// <versions>26-11-2017(GesAMFC-v0.0.4.37)</versions>
        public Boolean DBF_AMFC_Set_Entities_Payment_Closed(AMFCCashPayment objPayment)
        {
            try
            {
                using (Library_AMFC_SQL obj_AMFC_SQL = new Library_AMFC_SQL())
                {
                    #region     JOIAS
                    AMFCMemberJoias objListJoias = obj_AMFC_SQL.Get_Joias_Pay_Open(this.PaymentMember.NUMERO);
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
                    AMFC_Entities objListQuotas = obj_AMFC_SQL.Get_QUOTAS_Pay_Open(this.PaymentMember.NUMERO, -1);
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

        #endregion  Payment Methods

        /// <versions>06-12-2017(GesAMFC-v0.0.5.0)</versions>
        private void Button_Create_Receipt_Click(object sender, EventArgs e)
        {
            try
            {
                if (this.CurrentPayment == null || this.CurrentPayment.SOCIO < 1 || this.CurrentPayment.SOCIO > new AMFCMember().MaxNumber || this.CurrentPayment.ID < 1)
                {
                    XtraMessageBox.Show("Não foi possível obter os detalhes do " + "Pagamento" + " selecionado!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (this.PaymentMember == null || this.PaymentMember.NUMERO <= 0 || this.PaymentMember.NUMERO > this.PaymentMember.MaxNumber)
                {
                    XtraMessageBox.Show("Não foi possível obter os Dados do Sócio do " + "Pagamento" + " selecionado!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                Form_Payment_Receipt objReceipt = new Form_Payment_Receipt();
                objReceipt.Set_Member_Pay_Receipt();
                if (objReceipt != null)
                {
                    #region     Receipt Properties
                    objReceipt.Receipt_Number = this.CurrentPayment.ID;
                    objReceipt.Receipt_Value = this.CurrentPayment.VALOR;
                    objReceipt.Receipt_Member_Name = this.PaymentMember.NOME;
                    objReceipt.Receipt_Member_Number = this.PaymentMember.NUMERO;
                    objReceipt.Receipt_Member_Lote = this.PaymentMember.NUMLOTE;
                    objReceipt.Receipt_Member_Quantia = Program.QuantiaToExtenso(this.CurrentPayment.VALOR);
                    #region     Get Payment Receipt Designacao
                    String sDesignacao = this.CurrentPayment.DESIGNACAO;
                    if (!String.IsNullOrEmpty(this.CurrentPayment.JOIADESC.Trim()))
                    {
                        if (sDesignacao.Trim().Length + this.CurrentPayment.JOIADESC.Trim().Length <= objReceipt.Receipt_Desginação_Max_Chars)
                            sDesignacao += ": " + this.CurrentPayment.JOIADESC.Trim();
                    }
                    if (!String.IsNullOrEmpty(this.CurrentPayment.QUOTASDESC.Trim()))
                    {
                        String sQuotasDesc = this.CurrentPayment.QUOTASDESC.Trim();
                        if (sDesignacao.Trim().Length + sQuotasDesc.Length <= objReceipt.Receipt_Desginação_Max_Chars)
                            sDesignacao += ": " + sQuotasDesc;
                    }
                    if (!String.IsNullOrEmpty(this.CurrentPayment.INFRADESC.Trim()))
                    {
                        if (sDesignacao.Trim().Length + this.CurrentPayment.INFRADESC.Trim().Length <= objReceipt.Receipt_Desginação_Max_Chars)
                            sDesignacao += ": " + this.CurrentPayment.INFRADESC.Trim();
                    }
                    if (!String.IsNullOrEmpty(this.CurrentPayment.CEDENCDESC.Trim()))
                    {
                        if (sDesignacao.Trim().Length + this.CurrentPayment.CEDENCDESC.Trim().Length <= objReceipt.Receipt_Desginação_Max_Chars)
                            sDesignacao += ": " + this.CurrentPayment.CEDENCDESC.Trim();
                    }
                    if (!String.IsNullOrEmpty(this.CurrentPayment.ESGOTDESC.Trim()))
                    {
                        if (sDesignacao.Trim().Length + this.CurrentPayment.ESGOTDESC.Trim().Length <= objReceipt.Receipt_Desginação_Max_Chars)
                            sDesignacao += ": " + this.CurrentPayment.ESGOTDESC.Trim();
                    }
                    if (!String.IsNullOrEmpty(this.CurrentPayment.RECONDESC.Trim()))
                    {
                        if (sDesignacao.Trim().Length + this.CurrentPayment.RECONDESC.Trim().Length <= objReceipt.Receipt_Desginação_Max_Chars)
                            sDesignacao += "; " + this.CurrentPayment.RECONDESC.Trim();
                    }
                    if (!String.IsNullOrEmpty(this.CurrentPayment.OUTROSDESC.Trim()))
                    {
                        if (sDesignacao.Trim().Length + this.CurrentPayment.OUTROSDESC.Trim().Length <= objReceipt.Receipt_Desginação_Max_Chars)
                            sDesignacao += ": " + this.CurrentPayment.OUTROSDESC.Trim();
                    }
                    #endregion  Get Payment Receipt Designacao
                    objReceipt.Receipt_Member_Designacao = sDesignacao;
                    objReceipt.Receipt_Member_Data = DateTime.Today.ToString("D", CultureInfo.CurrentCulture);
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

        /// <versions>27-03-2018(GesAMFC-v1.0.1.2)</versions>
        private void DateEdit_DATA_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                switch (CurrentPayment.Payment_Type)
                {
                    case AMFCCashPayment.PaymentTypes.JOIA:
                        DateEdit_DASSOCJOIA.DateTime = Program.SetDateTimeValue(DateEdit_DATA.DateTime, -1, -1);
                        break;

                    case AMFCCashPayment.PaymentTypes.QUOTAS:
                        DateEdit_DASSOCQUOT.DateTime = Program.SetDateTimeValue(DateEdit_DATA.DateTime, -1, -1);
                        break;

                    case AMFCCashPayment.PaymentTypes.INFRAS:
                        DateEdit_DASSOCINFR.DateTime = Program.SetDateTimeValue(DateEdit_DATA.DateTime, -1, -1);
                        break;

                    case AMFCCashPayment.PaymentTypes.CEDENC:
                        DateEdit_DASSOCCEDE.DateTime = Program.SetDateTimeValue(DateEdit_DATA.DateTime, -1, -1);
                        break;

                    case AMFCCashPayment.PaymentTypes.ESGOT:
                        DateEdit_DASSOCESGO.DateTime = Program.SetDateTimeValue(DateEdit_DATA.DateTime, -1, -1);
                        break;

                    case AMFCCashPayment.PaymentTypes.RECONV:
                        DateEdit_DASSOCRECO.DateTime = Program.SetDateTimeValue(DateEdit_DATA.DateTime, -1, -1);
                        break;

                    case AMFCCashPayment.PaymentTypes.OUTRO:
                        DateEdit_DASSOCOUTR.DateTime = Program.SetDateTimeValue(DateEdit_DATA.DateTime, -1, -1);
                        break;

                    case AMFCCashPayment.PaymentTypes.MULTIPLE:
                        DateEdit_DASSOCJOIA.DateTime = Program.SetDateTimeValue(DateEdit_DATA.DateTime, -1, -1);
                        DateEdit_DASSOCQUOT.DateTime = Program.SetDateTimeValue(DateEdit_DATA.DateTime, -1, -1);
                        DateEdit_DASSOCINFR.DateTime = Program.SetDateTimeValue(DateEdit_DATA.DateTime, -1, -1);
                        DateEdit_DASSOCCEDE.DateTime = Program.SetDateTimeValue(DateEdit_DATA.DateTime, -1, -1);
                        DateEdit_DASSOCESGO.DateTime = Program.SetDateTimeValue(DateEdit_DATA.DateTime, -1, -1);
                        DateEdit_DASSOCRECO.DateTime = Program.SetDateTimeValue(DateEdit_DATA.DateTime, -1, -1);
                        DateEdit_DASSOCOUTR.DateTime = Program.SetDateTimeValue(DateEdit_DATA.DateTime, -1, -1);
                        break;
                }
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }
    }
}