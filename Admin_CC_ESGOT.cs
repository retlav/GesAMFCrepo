using DevExpress.XtraEditors;
using GesAMFC.AMFC_Methods;
using System;
using System.Windows.Forms;

namespace GesAMFC
{
    /// <summary>Admin Conta Corrente</summary>
    /// <author>Valter Lima</author>
    /// <creation>20-01-2018(GesAMFC-v1.0.0.3)</creation>
    /// <versions>01-03-2018(GesAMFC-v1.0.0.3)</versions>
    public partial class Admin_CC_ESGOT : DevExpress.XtraEditors.XtraForm
    {
        public Library_AMFC_Methods LibAMFC;

        private AMFCMember Member;
        private AMFCMemberLotes ListMemberLotes;
        private EntityTypeConfigs Entidade_Configs;

        private Double OneEuroToEscudos = 200.482;

        #region     Form Constructor 

        /// <versions>05-05-2017(v0.0.3.2)</versions>
        public Admin_CC_ESGOT()
        {
            LibAMFC = new Library_AMFC_Methods();

            try
            {
                InitializeComponent();

                LayoutControl_Global.Visible = false;

                this.Member = new AMFCMember();
                this.ListMemberLotes = new AMFCMemberLotes();

                this.Entidade_Configs = new PAG_ENTIDADE().GetEntityTypeConfigs(PAG_ENTIDADE.EntityTypes.ESGOT);
                if (this.Entidade_Configs == null)
                {
                    XtraMessageBox.Show("Tipo de Pagamento Inválido!", "Erro [" + "Tipo de Pagamento" + "]", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                this.OneEuroToEscudos = Convert.ToDouble(LibAMFC.DBF_AMFC_Euro_To_Escudos.Trim());
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        #endregion  Form Constructor

        #region     Events

        #region     Form Events

        /// <versions>20-01-2018(GesAMFC-v1.0.0.3)</versions>
        private void Admin_Form_Load(object sender, EventArgs e)
        {
            try
            {
                #region     Tem de ser feito aqui senão crasha -> No futuro, depois de ter o menu, comentar !!!
                this.WindowState = FormWindowState.Maximized;
                this.Update();
                #endregion

                Load_Member();
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>28-02-2018(GesAMFC-v1.0.0.3)</versions>
        private void Button_Procurar_Socio_Click(object sender, EventArgs e)
        {
            Load_Member();
        }

        private void Button_Socio_Carregar_Click(object sender, EventArgs e)
        {
            Get_Member_Lotes();
        }

        #endregion  Form Events

        #region     Events ESGOT

        /// <versions>02-03-2018(GesAMFC-v1.0.0.3)</versions>
        private void ComboBoxEdit_Select_Lote_ESGOT_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ComboBoxEdit_Select_Lote_ESGOT.SelectedIndex < 0)
                    return;
                Set_Lote_ESGOT();
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>04-03-2018(GesAMFC-v1.0.0.3)</versions>
        private void Button_CC_Guardar_ESGOT_Click(object sender, EventArgs e)
        {
            Save_Member_Lote_ESGOT();
        }

        #endregion  Events ESGOT

        #endregion  Events

        #region     Private Methods

        /// <versions>28-02-2018(GesAMFC-v1.0.0.3)</versions>
        private void Load_Member()
        {
            try
            {
                Clear_All_Controls();

                Find_Member_Form objFindMemberForm = new Find_Member_Form();
                if (objFindMemberForm != null)
                {
                    objFindMemberForm.FormClosing += delegate
                    {
                        if (Program.Member_Found) //trocar por objFindMemberForm.Member_Found e remover Program.Member_Found no futuro
                        {
                            AMFCMember objMemberSelected = objFindMemberForm.MemberSelected;
                            if (objMemberSelected != null && objMemberSelected.NUMERO >= objMemberSelected.MinNumber && objMemberSelected.NUMERO <= objMemberSelected.MaxNumber)
                            {
                                this.Member = objMemberSelected;
                                TextEdit_Socio_Numero.Text = objMemberSelected.NUMERO.ToString();
                                TextEdit_Socio_Nome.Text = objMemberSelected.NOME;
                                LayoutControl_Global.Visible = true;
                                Get_Member_Lotes();
                            }
                        }
                    };
                    objFindMemberForm.Show();
                    objFindMemberForm.StartPosition = FormStartPosition.CenterParent;
                    objFindMemberForm.Focus();
                    objFindMemberForm.BringToFront();
                    objFindMemberForm.Update();
                }
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return;
            }
        }


        /// <versions>08-03-2018(GesAMFC-v1.0.0.3)</versions>
        private void Clear_All_Controls()
        {
            try
            {
                TextEdit_Socio_Numero.Text = String.Empty;
                TextEdit_Socio_Nome.Text = String.Empty;

                ComboBoxEdit_Select_Lote_ESGOT.SelectedIndex = 0;

                Clear_Esgot_Controls();

                LayoutControl_Global.Visible = false;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>08-03-2018(GesAMFC-v1.0.0.3)</versions>
        private void Clear_Esgot_Controls()
        {
            try
            {
                TextEdit_Selected_Lote_Number_ESGOT.Text = String.Empty;
                TextEdit_Selected_Lote_ID_ESGOT.Text = String.Empty;
                TextEdit_Selected_Lote_Address_ESGOT.Text = String.Empty;
                TextEdit_Selected_Lote_Area_Pagar_ESGOT.Text = String.Empty;
                TextEdit_Selected_Lote_Total_Pagar_ESGOT.Text = String.Empty;

                SpinEdit_Pag_Reg_Num_Pagamentos_ESGOT.Value = 1;
                TextEdit_Pag_Reg_Valor_ESGOT.Text = String.Empty;

                DateEdit_Total_Pago_Data_ESGOT.DateTime = Program.Default_Date;
                TextEdit_Total_Pago_Valor_Escudos_ESGOT.Text = String.Empty;
                TextEdit_Total_Pago_Valor_ESGOT.Text = String.Empty;
                TextEdit_Total_Pago_Notas_ESGOT.Text = String.Empty;

                TextEdit_Falta_Pagar_Valor_ESGOT.Text = String.Empty;
                TextEdit_Falta_Pagar_Notas_ESGOT.Text = String.Empty;

                ComboBoxEdit_Estado_Liquidacao_ESGOT.SelectedIndex = 0;
                if (this.PictureBox_Estado_Liquidacao_ESGOT.Image != null)
                {
                    this.PictureBox_Estado_Liquidacao_ESGOT.Image.Dispose();
                    this.PictureBox_Estado_Liquidacao_ESGOT.Image = null;
                }
                TextEdit_Liquidacao_Notas_ESGOT.Text = String.Empty;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        #region     LOTES

        /// <versions>28-02-2018(GesAMFC-v1.0.0.3)</versions>
        private void Get_Member_Lotes()
        {
            try
            {
                if (this.Member == null || this.Member.NUMERO < this.Member.MinNumber || this.Member.NUMERO > this.Member.MaxNumber)
                {
                    String sErrorMsg = "Não foi obter o Sócio!";
                    Program.HandleError("", sErrorMsg, Program.ErroType.ERROR, true, true);
                    Clear_All_Controls();
                    return;
                }

                ListMemberLotes = Get_DBF_AMFC_Member_Lotes(this.Member.NUMERO);
                if (ListMemberLotes == null || ListMemberLotes.Lotes.Count == 0)
                {
                    String sErrorMsg = "Não foi possível obter a Lista de Lotes do Sócio Nº: " + this.Member.NUMERO;
                    Program.HandleError("", sErrorMsg, Program.ErroType.ERROR, true, true);
                    Clear_All_Controls();
                    return;
                }

                LayoutControl_Global.Visible = true;

                Set_List_Lotes_ESGOT(ListMemberLotes);
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>28-02-2018(GesAMFC-v1.0.0.3)</versions>
        private AMFCMemberLotes Get_DBF_AMFC_Member_Lotes(Int64 lMemberNumber)
        {
            try
            {
                using (Library_AMFC_SQL obj_AMFC_SQL = new Library_AMFC_SQL())
                    return obj_AMFC_SQL.Get_List_Member_Lotes(lMemberNumber);
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return null;
            }
        }

        #endregion  LOTES

        #region     ESGOT

        /// <versions>08-03-2018(GesAMFC-v1.0.0.3)</versions>
        private void Set_List_Lotes_ESGOT(AMFCMemberLotes objListLotes)
        {
            try
            {
                LayoutControlGroup_ESGOT.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                LayoutControlGroup_ESGOT.Expanded = true;

                Int32 iSelecteIndex = 0;
                Int32 iIndex = 0;
                ComboBoxEdit_Select_Lote_ESGOT.Properties.Items.Clear();
                foreach (AMFCMemberLote objLote in objListLotes.Lotes)
                {
                    ComboboxItem objComboBoxItem = new ComboboxItem(Convert.ToInt32(objLote.IDLOTE), objLote.NUMLOTE);
                    ComboBoxEdit_Select_Lote_ESGOT.Properties.Items.Add(objComboBoxItem);
                    iIndex++;
                }
                ComboBoxEdit_Select_Lote_ESGOT.SelectedIndex = iSelecteIndex;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>08-03-2018(GesAMFC-v1.0.0.3)</versions>
        private void Set_Lote_ESGOT()
        {
            try
            {
                Set_Lote_Info_ESGOT();
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>08-03-2018(GesAMFC-v1.0.0.3)</versions>
        private void Set_Lote_Info_ESGOT()
        {
            try
            {
                ComboboxItem objItemLote = (ComboBoxEdit_Select_Lote_ESGOT.SelectedItem as ComboboxItem);
                Int64 lLoteId = Convert.ToInt64(objItemLote.GetValue());
                if (lLoteId < 1)
                    return;
                AMFCMemberLote objLote = this.ListMemberLotes.GetLoteById(lLoteId);
                if (objLote == null || objLote.IDLOTE < 1 || String.IsNullOrEmpty(objLote.NUMLOTE))
                    return;
                TextEdit_Selected_Lote_Number_ESGOT.Text = objLote.NUMLOTE; 
                TextEdit_Selected_Lote_ID_ESGOT.Text = objLote.IDLOTE.ToString();
                TextEdit_Selected_Lote_Address_ESGOT.Text = objLote.MORLOTE;

                Set_Lote_Payments_ESGOT(objLote);
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>08-03-2018(GesAMFC-v1.0.0.3)</versions>
        private void Set_Lote_Payments_ESGOT(AMFCMemberLote objLote)
        {
            try
            {
                AMFC_ContaCorrente_ESGOT objCCESGOT = Get_DBF_Member_Lote_ESGOT(this.Member.NUMERO, objLote.IDLOTE);
                if (objCCESGOT == null)
                    return;

                Double dbAREAPAGAR = (objCCESGOT.AREAPAGAR > 0) ? objCCESGOT.AREAPAGAR : objLote.AREAPAGAR;
                TextEdit_Selected_Lote_Area_Pagar_ESGOT.Text = Program.SetAreaDoubleStringValue(dbAREAPAGAR);
                
                Int32 i_Lote_Total_Pays_Reg = Get_DBF_Member_Lote_Total_Payments_Reg_ESGOT(this.Member.NUMERO, objLote.IDLOTE);
                SpinEdit_Pag_Reg_Num_Pagamentos_ESGOT.Value = (i_Lote_Total_Pays_Reg > 0) ? i_Lote_Total_Pays_Reg : 0;
                Double db_Lote_Total_Valor_Reg = Get_DBF_Member_Lote_Total_Valor_Reg_ESGOT(this.Member.NUMERO, objLote.IDLOTE);
                TextEdit_Pag_Reg_Valor_ESGOT.Text = (db_Lote_Total_Valor_Reg > 0) ? Program.SetPayDoubleStringValue(db_Lote_Total_Valor_Reg) : Program.Default_Pay_Double_String;
                
                DateEdit_Total_Pago_Data_ESGOT.DateTime = Program.SetDateTimeValue(objCCESGOT.DATA, -1, -1);
                TextEdit_Selected_Lote_Total_Pagar_ESGOT.Text = (objCCESGOT.VALORPAGAR > 0) ? Program.SetPayCurrencyEuroStringValue(objCCESGOT.VALORPAGAR) : Program.Default_Pay_Double_String;
                TextEdit_Total_Pago_Valor_Escudos_ESGOT.Text = String.Format(Program.FormatString_Double3_DecimalPlaces, objCCESGOT.VALORESCUD);
                TextEdit_Total_Pago_Valor_ESGOT.Text = Program.SetPayDoubleStringValue(objCCESGOT.VALORPAGO);
                TextEdit_Total_Pago_Notas_ESGOT.Text = objCCESGOT.NOTASPAGO;

                TextEdit_Falta_Pagar_Valor_ESGOT.Text = Program.SetPayDoubleStringValue(objCCESGOT.VALORFALTA);
                TextEdit_Falta_Pagar_Notas_ESGOT.Text = objCCESGOT.NOTASFALTA;

                if (!String.IsNullOrEmpty(objCCESGOT.ESTADOLIQ.Trim()))
                {
                    switch (objCCESGOT.ESTADOLIQ.Trim())
                    {
                        default:
                        case "Totalidade em Dívida":
                            ComboBoxEdit_Estado_Liquidacao_ESGOT.SelectedIndex = 0;
                            break;
                        case "Pagamento Parcial":
                            ComboBoxEdit_Estado_Liquidacao_ESGOT.SelectedIndex = 1;
                            break;
                        case "Totalalidade Pago":
                            ComboBoxEdit_Estado_Liquidacao_ESGOT.SelectedIndex = 2;
                            break;
                    }
                }
                else
                    ComboBoxEdit_Estado_Liquidacao_ESGOT.SelectedIndex = 0;
                TextEdit_Liquidacao_Notas_ESGOT.Text = objCCESGOT.NOTASLIQ.Trim();
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>08-03-2018(GesAMFC-v1.0.0.3)</versions>
        private AMFC_ContaCorrente_ESGOT Get_DBF_Member_Lote_ESGOT(Int64 lMemberNumber, Int64 lMemberLoteId)
        {
            try
            {
                using (Library_AMFC_SQL obj_AMFC_SQL = new Library_AMFC_SQL())
                    return obj_AMFC_SQL.Get_Member_ContaCorrente_ESGOT(lMemberNumber, lMemberLoteId);
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return null;
            }
        }

        /// <versions>22-03-2018(GesAMFC-v1.0.0.3)</versions>
        private Int32 Get_DBF_Member_Lote_Total_Payments_Reg_ESGOT(Int64 lMemberNumber, Int64 lMemberLoteId)
        {
            try
            {
                using (Library_AMFC_SQL obj_AMFC_SQL = new Library_AMFC_SQL())
                    return obj_AMFC_SQL.Get_Member_ENTID_Total_Payments(this.Entidade_Configs, lMemberNumber, lMemberLoteId, -1, -1);
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return -1;
            }
        }

        /// <versions>22-03-2018(GesAMFC-v1.0.0.3)</versions>
        private Double Get_DBF_Member_Lote_Total_Valor_Reg_ESGOT(Int64 lMemberNumber, Int64 lMemberLoteId)
        {
            try
            {
                using (Library_AMFC_SQL obj_AMFC_SQL = new Library_AMFC_SQL())
                    return obj_AMFC_SQL.Get_Member_ENTID_Total_Valor_Paid(this.Entidade_Configs, lMemberNumber, lMemberLoteId, -1, -1);
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return -1;
            }
        }

        /// <versions>08-03-2018(GesAMFC-v1.0.0.3)</versions>
        private Boolean Save_Member_Lote_ESGOT()
        {
            try
            {
                if (this.Member == null || this.Member.NUMERO < this.Member.MinNumber || this.Member.NUMERO > this.Member.MaxNumber)
                    return false;

                ComboboxItem objItemLote = (ComboBoxEdit_Select_Lote_ESGOT.SelectedItem as ComboboxItem);
                Int64 lLoteId = Convert.ToInt64(objItemLote.GetValue());
                if (lLoteId < 1)
                    return false;
                AMFCMemberLote objLote = this.ListMemberLotes.GetLoteById(lLoteId);
                if (objLote == null || objLote.IDLOTE < 1 || String.IsNullOrEmpty(objLote.NUMLOTE) )
                    return false;

                AMFC_ContaCorrente_ESGOT objCCESGOT = new AMFC_ContaCorrente_ESGOT();

                objCCESGOT.SOCNUM = this.Member.NUMERO;
                objCCESGOT.SOCNOME = this.Member.NOME;
                objCCESGOT.IDLOTE = objLote.IDLOTE;
                objCCESGOT.NUMLOTE = objLote.NUMLOTE;

                objCCESGOT.AREAPAGAN = objLote.AREALOTES;
                objCCESGOT.AREAPAGAR = Program.SetAreaDoubleValue(TextEdit_Selected_Lote_Area_Pagar_ESGOT.Text);
                objCCESGOT.VALORPAGAR = Program.SetPayCurrencyEuroDoubleValue(TextEdit_Selected_Lote_Area_Pagar_ESGOT.Text);

                objCCESGOT.DATA = DateEdit_Total_Pago_Data_ESGOT.DateTime;
                objCCESGOT.VALORESCUD = Convert.ToDouble(TextEdit_Total_Pago_Valor_Escudos_ESGOT.Text);
                objCCESGOT.VALORPAGO = Program.SetPayCurrencyEuroDoubleValue(TextEdit_Total_Pago_Valor_ESGOT.Text);
                objCCESGOT.NOTASPAGO = TextEdit_Total_Pago_Notas_ESGOT.Text.Trim();

                objCCESGOT.VALORFALTA = Program.SetPayCurrencyEuroDoubleValue(TextEdit_Falta_Pagar_Valor_ESGOT.Text);
                objCCESGOT.NOTASFALTA = TextEdit_Falta_Pagar_Notas_ESGOT.Text.Trim();

                if (ComboBoxEdit_Estado_Liquidacao_ESGOT.SelectedIndex > -1)
                    objCCESGOT.ESTADOLIQ = ComboBoxEdit_Estado_Liquidacao_ESGOT.SelectedText;
                objCCESGOT.NOTASLIQ = TextEdit_Liquidacao_Notas_ESGOT.Text.Trim();

                if (!Set_DBF_Member_Lote_ESGOT(objCCESGOT))
                    return false;

                String sMessageOK = "Pagamentos de " + "Esgotos" + " guardados para o Lote Nº: " + objCCESGOT.NUMLOTE + " (ID=" + objCCESGOT.IDLOTE + ")" + " do Sócio: " + objCCESGOT.SOCNOME + " Nº: " + objCCESGOT.SOCNUM;
                MessageBox.Show(sMessageOK, "Esgotos" + " Salvas", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return true;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return false;
            }
        }

        /// <versions>08-03-2018(GesAMFC-v1.0.0.3)</versions>
        private Boolean Set_DBF_Member_Lote_ESGOT(AMFC_ContaCorrente_ESGOT objCCESGOT)
        {
            try
            {
                using (Library_AMFC_SQL obj_AMFC_SQL = new Library_AMFC_SQL())
                    return obj_AMFC_SQL.Save_DB_Lote_ESGOT(objCCESGOT);
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return false;
            }
        }

        #endregion  ESGOT

        #endregion  Private Methods

        /// <versions>23-03-2018(GesAMFC-v1.0.1.0)</versions>
        private void Button_Conversor_Esudos_Euros_Click(object sender, EventArgs e)
        {
            Open_Conversor_Escudos_Euros();
        }

        /// <versions>23-03-2018(GesAMFC-v1.0.1.0)</versions>
        private void Open_Conversor_Escudos_Euros()
        {
            try
            {
                Form_Calc_Euro_Escudos objForm_Calc_Euro_Escudos = new Form_Calc_Euro_Escudos();
                if (objForm_Calc_Euro_Escudos != null)
                {
                    objForm_Calc_Euro_Escudos.FormClosing += delegate
                    {
                        //...
                    };
                    objForm_Calc_Euro_Escudos.Show();
                    objForm_Calc_Euro_Escudos.StartPosition = FormStartPosition.CenterParent;
                    objForm_Calc_Euro_Escudos.Focus();
                    objForm_Calc_Euro_Escudos.BringToFront();
                    objForm_Calc_Euro_Escudos.Update();
                }
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, false, false);
            }
        }

        /// <versions>28-03-2018(GesAMFC-v1.0.1.2)</versions>
        private void TextEdit_Total_Pago_Valor_Escudos_ESGOT_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                Double dbEscudos = Program.SetAreaDoubleValue(this.TextEdit_Total_Pago_Valor_Escudos_ESGOT.Text);
                if (dbEscudos <= 0)
                    return;
                Double dbEuros = Convert.ToDouble(Decimal.Divide(Convert.ToDecimal(dbEscudos), Convert.ToDecimal(this.OneEuroToEscudos)));
                this.TextEdit_Total_Pago_Valor_ESGOT.Text = String.Format(Program.FormatString_Double3_DecimalPlaces, dbEuros);
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, false, false);
            }
        }
    }
}