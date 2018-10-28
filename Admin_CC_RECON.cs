using DevExpress.XtraEditors;
using GesAMFC.AMFC_Methods;
using System;
using System.Windows.Forms;

namespace GesAMFC
{
    /// <summary>Admin Conta Corrente</summary>
    /// <author>Valter Lima</author>
    /// <creation>20-01-2018(GesAMFC-v1.0.0.3)</creation>
    /// <versions>22-03-2018(GesAMFC-v1.0.0.3)</versions>
    public partial class Admin_CC_RECON : DevExpress.XtraEditors.XtraForm
    {
        public Library_AMFC_Methods LibAMFC;

        private AMFCMember Member;
        private AMFCMemberLotes ListMemberLotes;
        private EntityTypeConfigs Entidade_Configs;

        private Double OneEuroToEscudos = 200.482;

        #region     Form Constructor 

        /// <versions>05-05-2017(v0.0.3.2)</versions>
        public Admin_CC_RECON()
        {
            LibAMFC = new Library_AMFC_Methods();
            try
            {
                InitializeComponent();

                LayoutControl_Global.Visible = false;

                this.Member = new AMFCMember();
                this.ListMemberLotes = new AMFCMemberLotes();

                this.Entidade_Configs = new PAG_ENTIDADE().GetEntityTypeConfigs(PAG_ENTIDADE.EntityTypes.RECON);
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

        #region     Events RECON

        /// <versions>02-03-2018(GesAMFC-v1.0.0.3)</versions>
        private void ComboBoxEdit_Select_Lote_RECON_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ComboBoxEdit_Select_Lote_RECON.SelectedIndex < 0)
                    return;
                Set_Lote_RECON();
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>04-03-2018(GesAMFC-v1.0.0.3)</versions>
        private void Button_CC_Guardar_RECON_Click(object sender, EventArgs e)
        {
            Save_Member_Lote_RECON();
        }

        #endregion  Events RECON

        #endregion  Events

        #region     Private Methods

        /// <versions>08-03-2018(GesAMFC-v1.0.0.3)</versions>
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

                ComboBoxEdit_Select_Lote_RECON.SelectedIndex = 0;

                Clear_RECON_Controls();

                LayoutControl_Global.Visible = false;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>08-03-2018(GesAMFC-v1.0.0.3)</versions>
        private void Clear_RECON_Controls()
        {
            try
            {
                TextEdit_Selected_Lote_Number_RECON.Text = String.Empty;
                TextEdit_Selected_Lote_ID_RECON.Text = String.Empty;
                TextEdit_Selected_Lote_Address_RECON.Text = String.Empty;
                TextEdit_Selected_Lote_Area_Pagar_RECON.Text = String.Empty;
                TextEdit_Selected_Lote_Preco_M2_RECON.Text = String.Empty;
                TextEdit_Selected_Lote_Total_Pagar_RECON.Text = String.Empty;
                
                SpinEdit_Pag_Reg_Num_Pagamentos_RECON.Value = 1;
                TextEdit_Pag_Reg_Metros_RECON.Text = String.Empty;
                TextEdit_Pag_Reg_Valor_RECON.Text = String.Empty;

                DateEdit_Total_Pago_Data_RECON.DateTime = Program.Default_Date;
                TextEdit_Total_Pago_Preco_Metro_RECON.Text = String.Empty;
                TextEdit_Total_Pago_Metros_RECON.Text = String.Empty;
                TextEdit_Total_Pago_Valor_Escudos_RECON.Text = String.Empty;
                TextEdit_Total_Pago_Valor_RECON.Text = String.Empty;
                TextEdit_Total_Pago_Notas_RECON.Text = String.Empty;
                
                TextEdit_Falta_Pagar_Preco_Metro_RECON.Text = String.Empty;
                TextEdit_Falta_Pagar_Metros_RECON.Text = String.Empty;
                TextEdit_Falta_Pagar_Valor_RECON.Text = String.Empty;
                TextEdit_Falta_Pagar_Notas_RECON.Text = String.Empty;

                ComboBoxEdit_Estado_Liquidacao_RECON.SelectedIndex = 0;
                if (this.PictureBox_Estado_Liquidacao_RECON.Image != null)
                {
                    this.PictureBox_Estado_Liquidacao_RECON.Image.Dispose();
                    this.PictureBox_Estado_Liquidacao_RECON.Image = null;
                }
                TextEdit_Liquidacao_Notas_RECON.Text = String.Empty;
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

                Set_List_Lotes_RECON(ListMemberLotes);
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

        #region     RECON

        /// <versions>02-03-2018(GesAMFC-v1.0.0.3)</versions>
        private void Set_List_Lotes_RECON(AMFCMemberLotes objListLotes)
        {
            try
            {
                LayoutControlGroup_RECON.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                LayoutControlGroup_RECON.Expanded = true;

                Int32 iSelecteIndex = 0;
                Int32 iIndex = 0;
                ComboBoxEdit_Select_Lote_RECON.Properties.Items.Clear();
                foreach (AMFCMemberLote objLote in objListLotes.Lotes)
                {
                    ComboboxItem objComboBoxItem = new ComboboxItem(Convert.ToInt32(objLote.IDLOTE), objLote.NUMLOTE);
                    ComboBoxEdit_Select_Lote_RECON.Properties.Items.Add(objComboBoxItem);
                    iIndex++;
                }
                ComboBoxEdit_Select_Lote_RECON.SelectedIndex = iSelecteIndex;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>04-03-2018(GesAMFC-v1.0.0.3)</versions>
        private void Set_Lote_RECON()
        {
            try
            {
                Clear_RECON_Controls();
                Set_Lote_Info_RECON();
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>04-03-2018(GesAMFC-v1.0.0.3)</versions>
        private void Set_Lote_Info_RECON()
        {
            try
            {
                if (ComboBoxEdit_Select_Lote_RECON.SelectedIndex < 0)
                    return;
                ComboboxItem objItemLote = (ComboBoxEdit_Select_Lote_RECON.SelectedItem as ComboboxItem);
                Int64 lLoteId = Convert.ToInt64(objItemLote.GetValue());
                if (lLoteId < 1)
                    return;
                AMFCMemberLote objLote = this.ListMemberLotes.GetLoteById(lLoteId);
                if (objLote == null || objLote.IDLOTE < 1 || String.IsNullOrEmpty(objLote.NUMLOTE) )
                    return;
                TextEdit_Selected_Lote_Number_RECON.Text = objLote.NUMLOTE; 
                TextEdit_Selected_Lote_ID_RECON.Text = objLote.IDLOTE.ToString();
                TextEdit_Selected_Lote_Address_RECON.Text = objLote.MORLOTE;

                Set_Lote_Payments_RECON(objLote);
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>04-03-2018(GesAMFC-v1.0.0.3)</versions>
        private void Set_Lote_Payments_RECON(AMFCMemberLote objLote)
        {
            try
            {
                AMFC_ContaCorrente_RECON objCCRECON = Get_DBF_Member_Lote_RECON(this.Member.NUMERO, objLote.IDLOTE);
                if (objCCRECON == null)
                    return;
                
                Double dbAREA = (objCCRECON.AREA > 0) ? objCCRECON.AREA : objLote.AREAPAGAR;
                Double dbRECON_Valor_Metro = 0;
                Double dbRECON_Area_Pagar = 0;
                Double dbTotal_Pagar_RECON = 0;
                if (objCCRECON.PRECOM2 > 0)
                    dbRECON_Valor_Metro = objCCRECON.PRECOM2;
                else
                    dbRECON_Valor_Metro = Program.Get_Current_Parameter_RECON_Valor_Metro();
                if (objCCRECON.VALORPAGAR > 0)
                    dbTotal_Pagar_RECON = objCCRECON.VALORPAGAR;
                else
                    dbTotal_Pagar_RECON = Convert.ToDouble(Decimal.Multiply(Convert.ToDecimal(dbRECON_Area_Pagar), Convert.ToDecimal(dbRECON_Valor_Metro)));

                TextEdit_Selected_Lote_Area_Pagar_RECON.Text = Program.SetAreaDoubleStringValue(dbRECON_Area_Pagar);
                TextEdit_Selected_Lote_Preco_M2_RECON.Text = Program.SetPayCurrencyEuroStringValue(dbRECON_Valor_Metro);
                TextEdit_Selected_Lote_Total_Pagar_RECON.Text = Program.SetPayCurrencyEuroStringValue(dbTotal_Pagar_RECON);

                TextEdit_Selected_Lote_Area_Pagar_RECON.Text = Program.SetAreaDoubleStringValue(dbRECON_Area_Pagar);
                TextEdit_Selected_Lote_Preco_M2_RECON.Text = Program.SetPayDoubleStringValue(dbRECON_Valor_Metro);
                TextEdit_Selected_Lote_Total_Pagar_RECON.Text = Program.SetPayDoubleStringValue(dbTotal_Pagar_RECON);
                
                Int32 i_Lote_Total_Pays_Reg = Get_DBF_Member_Lote_Total_Payments_Reg_RECON(this.Member.NUMERO, objLote.IDLOTE);
                SpinEdit_Pag_Reg_Num_Pagamentos_RECON.Value = (i_Lote_Total_Pays_Reg > 0) ? i_Lote_Total_Pays_Reg : 0;
                Double db_Lote_Total_Area_Reg = Get_DBF_Member_Lote_Total_Area_Reg_RECON(this.Member.NUMERO, objLote.IDLOTE);
                TextEdit_Pag_Reg_Metros_RECON.Text = (db_Lote_Total_Area_Reg > 0) ? Program.SetAreaDoubleStringValue(db_Lote_Total_Area_Reg) : Program.Default_Area_Double_String;
                Double db_Lote_Total_Valor_Reg = Get_DBF_Member_Lote_Total_Valor_Reg_RECON(this.Member.NUMERO, objLote.IDLOTE);
                TextEdit_Pag_Reg_Valor_RECON.Text = (db_Lote_Total_Valor_Reg > 0) ? Program.SetPayDoubleStringValue(db_Lote_Total_Valor_Reg) : Program.Default_Pay_Double_String;
                
                DateEdit_Total_Pago_Data_RECON.DateTime = Program.SetDateTimeValue(objCCRECON.DATA, -1, -1);
                TextEdit_Total_Pago_Preco_Metro_RECON.Text = Program.SetPayDoubleStringValue(objCCRECON.PRECOM2P);
                TextEdit_Total_Pago_Metros_RECON.Text = Program.SetAreaDoubleStringValue(objCCRECON.AREAPAGO);
                TextEdit_Total_Pago_Valor_Escudos_RECON.Text = (objCCRECON.VALORESCUD > 0) ? String.Format(Program.FormatString_Double3_DecimalPlaces, objCCRECON.VALORESCUD) : Program.Default_Pay_Double_String;
                TextEdit_Total_Pago_Valor_RECON.Text = Program.SetPayDoubleStringValue(objCCRECON.VALORPAGO);
                TextEdit_Total_Pago_Notas_RECON.Text = objCCRECON.NOTASPAGO;

                TextEdit_Falta_Pagar_Preco_Metro_RECON.Text = Program.SetPayDoubleStringValue(objCCRECON.PRECOM2F);
                TextEdit_Falta_Pagar_Metros_RECON.Text = Program.SetAreaDoubleStringValue(objCCRECON.AREAFALTA);
                TextEdit_Falta_Pagar_Valor_RECON.Text = Program.SetPayDoubleStringValue(objCCRECON.VALORFALTA);
                TextEdit_Falta_Pagar_Notas_RECON.Text = objCCRECON.NOTASFALTA;

                if (!String.IsNullOrEmpty(objCCRECON.ESTADOLIQ.Trim()))
                {
                    switch (objCCRECON.ESTADOLIQ.Trim())
                    {
                        default:
                        case "Totalidade em Dívida":
                            ComboBoxEdit_Estado_Liquidacao_RECON.SelectedIndex = 0;
                            break;
                        case "Pagamento Parcial":
                            ComboBoxEdit_Estado_Liquidacao_RECON.SelectedIndex = 1;
                            break;
                        case "Totalalidade Pago":
                            ComboBoxEdit_Estado_Liquidacao_RECON.SelectedIndex = 2;
                            break;
                    }
                }
                else
                    ComboBoxEdit_Estado_Liquidacao_RECON.SelectedIndex = 0;
                TextEdit_Liquidacao_Notas_RECON.Text = objCCRECON.NOTASLIQ.Trim();
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>04-03-2018(GesAMFC-v1.0.0.3)</versions>
        private AMFC_ContaCorrente_RECON Get_DBF_Member_Lote_RECON(Int64 lMemberNumber, Int64 lMemberLoteId)
        {
            try
            {
                using (Library_AMFC_SQL obj_AMFC_SQL = new Library_AMFC_SQL())
                    return obj_AMFC_SQL.Get_Member_ContaCorrente_RECON(lMemberNumber, lMemberLoteId);
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return null;
            }
        }

        /// <versions>22-03-2018(GesAMFC-v1.0.0.3)</versions>
        private Int32 Get_DBF_Member_Lote_Total_Payments_Reg_RECON(Int64 lMemberNumber, Int64 lMemberLoteId)
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
        private Double Get_DBF_Member_Lote_Total_Valor_Reg_RECON(Int64 lMemberNumber, Int64 lMemberLoteId)
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

        /// <versions>22-03-2018(GesAMFC-v1.0.0.3)</versions>
        private Double Get_DBF_Member_Lote_Total_Area_Reg_RECON(Int64 lMemberNumber, Int64 lMemberLoteId)
        {
            try
            {
                using (Library_AMFC_SQL obj_AMFC_SQL = new Library_AMFC_SQL())
                    return obj_AMFC_SQL.Get_Member_ENTID_Total_Area_Paid(this.Entidade_Configs, lMemberNumber, lMemberLoteId, -1, -1);
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return -1;
            }
        }

        /// <versions>04-03-2018(GesAMFC-v1.0.0.3)</versions>
        private Boolean Save_Member_Lote_RECON()
        {
            try
            {
                if (this.Member == null || this.Member.NUMERO < this.Member.MinNumber || this.Member.NUMERO > this.Member.MaxNumber)
                    return false;
                if (ComboBoxEdit_Select_Lote_RECON.SelectedIndex < 0)
                    return false;
                ComboboxItem objItemLote = (ComboBoxEdit_Select_Lote_RECON.SelectedItem as ComboboxItem);
                Int64 lLoteId = Convert.ToInt64(objItemLote.GetValue());
                if (lLoteId < 1)
                    return false;
                AMFCMemberLote objLote = this.ListMemberLotes.GetLoteById(lLoteId);
                if (objLote == null || objLote.IDLOTE < 1 || String.IsNullOrEmpty(objLote.NUMLOTE) )
                    return false;

                AMFC_ContaCorrente_RECON objCCRECON = new AMFC_ContaCorrente_RECON();

                objCCRECON.SOCNUM = this.Member.NUMERO;
                objCCRECON.SOCNOME = this.Member.NOME;
                objCCRECON.IDLOTE = objLote.IDLOTE;
                objCCRECON.NUMLOTE = objLote.NUMLOTE;

                objCCRECON.AREA = objLote.AREALOTES;
                objCCRECON.AREAPAGAR = Program.SetAreaDoubleValue(TextEdit_Selected_Lote_Area_Pagar_RECON.Text);
                objCCRECON.PRECOM2 = Program.SetPayCurrencyEuroDoubleValue(TextEdit_Selected_Lote_Preco_M2_RECON.Text);
                objCCRECON.VALORPAGAR = Program.SetPayCurrencyEuroDoubleValue(TextEdit_Selected_Lote_Total_Pagar_RECON.Text);

                objCCRECON.DATA = DateEdit_Total_Pago_Data_RECON.DateTime;
                objCCRECON.PRECOM2P = Program.SetPayCurrencyEuroDoubleValue(TextEdit_Total_Pago_Preco_Metro_RECON.Text);
                objCCRECON.AREAPAGO = Program.SetAreaDoubleValue(TextEdit_Total_Pago_Metros_RECON.Text);
                objCCRECON.VALORESCUD = Convert.ToDouble(TextEdit_Total_Pago_Valor_Escudos_RECON.Text);
                objCCRECON.VALORPAGO = Program.SetPayCurrencyEuroDoubleValue(TextEdit_Total_Pago_Valor_RECON.Text);
                objCCRECON.NOTASPAGO = TextEdit_Total_Pago_Notas_RECON.Text.Trim();

                objCCRECON.PRECOM2F = Program.SetPayCurrencyEuroDoubleValue(TextEdit_Falta_Pagar_Preco_Metro_RECON.Text);
                objCCRECON.AREAFALTA = Program.SetAreaDoubleValue(TextEdit_Falta_Pagar_Metros_RECON.Text);
                objCCRECON.VALORFALTA = Program.SetPayCurrencyEuroDoubleValue(TextEdit_Falta_Pagar_Valor_RECON.Text);
                objCCRECON.NOTASFALTA = TextEdit_Falta_Pagar_Notas_RECON.Text.Trim();

                if (ComboBoxEdit_Estado_Liquidacao_RECON.SelectedIndex > -1)
                    objCCRECON.ESTADOLIQ = ComboBoxEdit_Estado_Liquidacao_RECON.SelectedText;
                objCCRECON.NOTASLIQ = TextEdit_Liquidacao_Notas_RECON.Text.Trim();

                if (!Set_DBF_Member_Lote_RECON(objCCRECON))
                    return false;

                String sMessageOK = "Pagamentos de " + "Cedências" +" guardados para o Lote Nº: " + objCCRECON.NUMLOTE + " (ID=" + objCCRECON.IDLOTE + ")" + " do Sócio: " + objCCRECON.SOCNOME + " Nº: " + objCCRECON.SOCNUM;
                MessageBox.Show(sMessageOK, "Cedências" + " Salvas", MessageBoxButtons.OK, MessageBoxIcon.Information);

                return true;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return false;
            }
        }

        /// <versions>04-03-2018(GesAMFC-v1.0.0.3)</versions>
        private Boolean Set_DBF_Member_Lote_RECON(AMFC_ContaCorrente_RECON objCCRECON)
        {
            try
            {
                using (Library_AMFC_SQL obj_AMFC_SQL = new Library_AMFC_SQL())
                    return obj_AMFC_SQL.Save_DB_Lote_RECON(objCCRECON);
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return false;
            }
        }

        #endregion  RECON

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

        private void Label_Admin_CC_RECON_Click(object sender, EventArgs e)
        {

        }

        /// <versions>28-03-2018(GesAMFC-v1.0.1.2)</versions>
        private void TextEdit_Total_Pago_Valor_Escudos_RECON_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                Double dbEscudos = Program.SetAreaDoubleValue(this.TextEdit_Total_Pago_Valor_Escudos_RECON.Text);
                if (dbEscudos <= 0)
                    return;
                Double dbEuros = Convert.ToDouble(Decimal.Divide(Convert.ToDecimal(dbEscudos), Convert.ToDecimal(this.OneEuroToEscudos)));
                this.TextEdit_Total_Pago_Valor_RECON.Text = String.Format(Program.FormatString_Double3_DecimalPlaces, dbEuros);
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, false, false);
            }
        }
    }
}