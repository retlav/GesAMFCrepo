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
    public partial class Admin_CC_CEDEN : DevExpress.XtraEditors.XtraForm
    {
        public Library_AMFC_Methods LibAMFC;

        private AMFCMember Member;
        private AMFCMemberLotes ListMemberLotes;
        private EntityTypeConfigs Entidade_Configs;

        private Double OneEuroToEscudos = 200.482;

        #region     Form Constructor 

        /// <versions>05-05-2017(v0.0.3.2)</versions>
        public Admin_CC_CEDEN()
        {
            LibAMFC = new Library_AMFC_Methods();
            try
            {
                InitializeComponent();

                LayoutControl_Global.Visible = false;

                this.Member = new AMFCMember();
                this.ListMemberLotes = new AMFCMemberLotes();

                this.Entidade_Configs = new PAG_ENTIDADE().GetEntityTypeConfigs(PAG_ENTIDADE.EntityTypes.CEDEN);
                if (this.Entidade_Configs == null)
                {
                    XtraMessageBox.Show("Tipo de Pagamento Inválido!", "Erro [" + "Tipo de Pagamento" + "]", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
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

        #region     Events CEDEN

        /// <versions>02-03-2018(GesAMFC-v1.0.0.3)</versions>
        private void ComboBoxEdit_Select_Lote_CEDEN_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (ComboBoxEdit_Select_Lote_CEDEN.SelectedIndex < 0)
                    return;
                Set_Lote_CEDEN();
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>04-03-2018(GesAMFC-v1.0.0.3)</versions>
        private void Button_CC_Guardar_CEDEN_Click(object sender, EventArgs e)
        {
            Save_Member_Lote_CEDEN();
        }

        #endregion  Events CEDEN

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

                ComboBoxEdit_Select_Lote_CEDEN.SelectedIndex = 0;

                Clear_Ceden_Controls();

                LayoutControl_Global.Visible = false;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>08-03-2018(GesAMFC-v1.0.0.3)</versions>
        private void Clear_Ceden_Controls()
        {
            try
            {
                TextEdit_Selected_Lote_Number_CEDEN.Text = String.Empty;
                TextEdit_Selected_Lote_ID_CEDEN.Text = String.Empty;
                TextEdit_Selected_Lote_Address_CEDEN.Text = String.Empty;
                TextEdit_Selected_Lote_Area_Pagar_CEDEN.Text = String.Empty;
                TextEdit_Selected_Lote_Preco_M2_CEDEN.Text = String.Empty;
                TextEdit_Selected_Lote_Total_Pagar_CEDEN.Text = String.Empty;
                
                SpinEdit_Pag_Reg_Num_Pagamentos_CEDEN.Value = 1;
                TextEdit_Pag_Reg_Metros_CEDEN.Text = String.Empty;
                TextEdit_Pag_Reg_Valor_CEDEN.Text = String.Empty;

                DateEdit_Total_Pago_Data_CEDEN.DateTime = Program.Default_Date;
                TextEdit_Selected_Lote_Percentagem_Ceder_CEDEN.Text = String.Empty;
                TextEdit_Total_Pago_Preco_Metro_CEDEN.Text = String.Empty;
                TextEdit_Total_Pago_Metros_CEDEN.Text = String.Empty;
                TextEdit_Total_Pago_Valor_Escudos_CEDEN.Text = String.Empty;
                TextEdit_Total_Pago_Valor_CEDEN.Text = String.Empty;
                TextEdit_Total_Pago_Notas_CEDEN.Text = String.Empty;
                
                TextEdit_Falta_Pagar_Preco_Metro_CEDEN.Text = String.Empty;
                TextEdit_Falta_Pagar_Metros_CEDEN.Text = String.Empty;
                TextEdit_Falta_Pagar_Valor_CEDEN.Text = String.Empty;
                TextEdit_Falta_Pagar_Notas_CEDEN.Text = String.Empty;

                ComboBoxEdit_Estado_Liquidacao_CEDEN.SelectedIndex = 0;
                if (this.PictureBox_Estado_Liquidacao_CEDEN.Image != null)
                {
                    this.PictureBox_Estado_Liquidacao_CEDEN.Image.Dispose();
                    this.PictureBox_Estado_Liquidacao_CEDEN.Image = null;
                }
                TextEdit_Liquidacao_Notas_CEDEN.Text = String.Empty;
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

                Set_List_Lotes_CEDEN(ListMemberLotes);
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

        #region     CEDEN

        /// <versions>02-03-2018(GesAMFC-v1.0.0.3)</versions>
        private void Set_List_Lotes_CEDEN(AMFCMemberLotes objListLotes)
        {
            try
            {
                LayoutControlGroup_CEDEN.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                LayoutControlGroup_CEDEN.Expanded = true;

                Int32 iSelecteIndex = 0;
                Int32 iIndex = 0;
                ComboBoxEdit_Select_Lote_CEDEN.Properties.Items.Clear();
                foreach (AMFCMemberLote objLote in objListLotes.Lotes)
                {
                    ComboboxItem objComboBoxItem = new ComboboxItem(Convert.ToInt32(objLote.IDLOTE), objLote.NUMLOTE);
                    ComboBoxEdit_Select_Lote_CEDEN.Properties.Items.Add(objComboBoxItem);
                    iIndex++;
                }
                ComboBoxEdit_Select_Lote_CEDEN.SelectedIndex = iSelecteIndex;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>04-03-2018(GesAMFC-v1.0.0.3)</versions>
        private void Set_Lote_CEDEN()
        {
            try
            {
                Clear_Ceden_Controls();
                Set_Lote_Info_CEDEN();
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>04-03-2018(GesAMFC-v1.0.0.3)</versions>
        private void Set_Lote_Info_CEDEN()
        {
            try
            {
                if (ComboBoxEdit_Select_Lote_CEDEN.SelectedIndex < 0)
                    return;
                ComboboxItem objItemLote = (ComboBoxEdit_Select_Lote_CEDEN.SelectedItem as ComboboxItem);
                Int64 lLoteId = Convert.ToInt64(objItemLote.GetValue());
                if (lLoteId < 1)
                    return;
                AMFCMemberLote objLote = this.ListMemberLotes.GetLoteById(lLoteId);
                if (objLote == null || objLote.IDLOTE < 1 || String.IsNullOrEmpty(objLote.NUMLOTE) )
                    return;
                TextEdit_Selected_Lote_Number_CEDEN.Text = objLote.NUMLOTE; 
                TextEdit_Selected_Lote_ID_CEDEN.Text = objLote.IDLOTE.ToString();
                TextEdit_Selected_Lote_Address_CEDEN.Text = objLote.MORLOTE;

                Set_Lote_Payments_CEDEN(objLote);
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>04-03-2018(GesAMFC-v1.0.0.3)</versions>
        private void Set_Lote_Payments_CEDEN(AMFCMemberLote objLote)
        {
            try
            {
                AMFC_ContaCorrente_CEDEN objCCCEDEN = Get_DBF_Member_Lote_CEDEN(this.Member.NUMERO, objLote.IDLOTE);
                if (objCCCEDEN == null)
                    return;
                
                Double dbAREA = (objCCCEDEN.AREA > 0) ? objCCCEDEN.AREA : objLote.AREAPAGAR;
                Double dbCEDEN_Valor_Metro = 0;
                Double dbCEDEN_CederPercentagem = 0;
                Double dbCEDEN_CederArea = 0;
                Double dbTotal_Pagar_CEDEN = 0;
                if (objCCCEDEN.CEDERPERC > 0 && objCCCEDEN.AREAPAGAR > 0)
                {
                    dbCEDEN_CederPercentagem = objCCCEDEN.CEDERPERC;
                    dbCEDEN_CederArea = objCCCEDEN.AREAPAGAR;
                }
                else
                {
                    dbCEDEN_CederPercentagem = objCCCEDEN.GetCederPercentagem(dbAREA);
                    dbCEDEN_CederArea = objCCCEDEN.GetCederTotalAreaCeder(dbAREA);
                }
                if (objCCCEDEN.PRECOM2 > 0)
                    dbCEDEN_Valor_Metro = objCCCEDEN.PRECOM2;
                else
                    dbCEDEN_Valor_Metro = Program.Get_Current_Parameter_CEDENC_Valor_Metro();

                if (objCCCEDEN.VALORPAGAR > 0)
                    dbTotal_Pagar_CEDEN = objCCCEDEN.VALORPAGAR;
                else
                    dbTotal_Pagar_CEDEN = Convert.ToDouble(Decimal.Multiply(Convert.ToDecimal(dbCEDEN_CederArea), Convert.ToDecimal(dbCEDEN_Valor_Metro)));

                Double dbCEDEN_CederPercentagem_Text = Convert.ToDouble(Decimal.Multiply(Convert.ToDecimal(dbCEDEN_CederPercentagem), 100));
                
                TextEdit_Selected_Lote_Percentagem_Ceder_CEDEN.Text = Convert.ToInt32(dbCEDEN_CederPercentagem_Text).ToString();
                TextEdit_Selected_Lote_Area_Pagar_CEDEN.Text = Program.SetAreaDoubleStringValue(dbCEDEN_CederArea);
                TextEdit_Selected_Lote_Preco_M2_CEDEN.Text = Program.SetPayDoubleStringValue(dbCEDEN_Valor_Metro);
                TextEdit_Selected_Lote_Total_Pagar_CEDEN.Text = Program.SetPayDoubleStringValue(dbTotal_Pagar_CEDEN);
                
                Int32 i_Lote_Total_Pays_Reg = Get_DBF_Member_Lote_Total_Payments_Reg_CEDEN(this.Member.NUMERO, objLote.IDLOTE);
                SpinEdit_Pag_Reg_Num_Pagamentos_CEDEN.Value = (i_Lote_Total_Pays_Reg > 0) ? i_Lote_Total_Pays_Reg : 0;
                Double db_Lote_Total_Area_Reg = Get_DBF_Member_Lote_Total_Area_Reg_CEDEN(this.Member.NUMERO, objLote.IDLOTE);
                TextEdit_Pag_Reg_Metros_CEDEN.Text = (db_Lote_Total_Area_Reg > 0) ? Program.SetAreaDoubleStringValue(db_Lote_Total_Area_Reg) : Program.Default_Area_Double_String;
                Double db_Lote_Total_Valor_Reg = Get_DBF_Member_Lote_Total_Valor_Reg_CEDEN(this.Member.NUMERO, objLote.IDLOTE);
                TextEdit_Pag_Reg_Valor_CEDEN.Text = (db_Lote_Total_Valor_Reg > 0) ? Program.SetPayDoubleStringValue(db_Lote_Total_Valor_Reg) : Program.Default_Pay_Double_String;
                
                DateEdit_Total_Pago_Data_CEDEN.DateTime = Program.SetDateTimeValue(objCCCEDEN.DATA, -1, -1);
                TextEdit_Total_Pago_Preco_Metro_CEDEN.Text = Program.SetPayDoubleStringValue(objCCCEDEN.PRECOM2P);
                TextEdit_Total_Pago_Metros_CEDEN.Text = Program.SetAreaDoubleStringValue(objCCCEDEN.AREAPAGO);
                TextEdit_Total_Pago_Valor_Escudos_CEDEN.Text = (objCCCEDEN.VALORESCUD > 0) ? String.Format(Program.FormatString_Double3_DecimalPlaces, objCCCEDEN.VALORESCUD) : Program.Default_Pay_Double_String;
                TextEdit_Total_Pago_Valor_CEDEN.Text = Program.SetPayDoubleStringValue(objCCCEDEN.VALORPAGO);
                TextEdit_Total_Pago_Notas_CEDEN.Text = objCCCEDEN.NOTASPAGO;

                TextEdit_Falta_Pagar_Preco_Metro_CEDEN.Text = Program.SetPayDoubleStringValue(objCCCEDEN.PRECOM2F);
                TextEdit_Falta_Pagar_Metros_CEDEN.Text = Program.SetAreaDoubleStringValue(objCCCEDEN.AREAFALTA);
                TextEdit_Falta_Pagar_Valor_CEDEN.Text = Program.SetPayDoubleStringValue(objCCCEDEN.VALORFALTA);
                TextEdit_Falta_Pagar_Notas_CEDEN.Text = objCCCEDEN.NOTASFALTA;

                if (!String.IsNullOrEmpty(objCCCEDEN.ESTADOLIQ.Trim()))
                {
                    switch (objCCCEDEN.ESTADOLIQ.Trim())
                    {
                        default:
                        case "Totalidade em Dívida":
                            ComboBoxEdit_Estado_Liquidacao_CEDEN.SelectedIndex = 0;
                            break;
                        case "Pagamento Parcial":
                            ComboBoxEdit_Estado_Liquidacao_CEDEN.SelectedIndex = 1;
                            break;
                        case "Totalalidade Pago":
                            ComboBoxEdit_Estado_Liquidacao_CEDEN.SelectedIndex = 2;
                            break;
                    }
                }
                else
                    ComboBoxEdit_Estado_Liquidacao_CEDEN.SelectedIndex = 0;
                TextEdit_Liquidacao_Notas_CEDEN.Text = objCCCEDEN.NOTASLIQ.Trim();
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>04-03-2018(GesAMFC-v1.0.0.3)</versions>
        private AMFC_ContaCorrente_CEDEN Get_DBF_Member_Lote_CEDEN(Int64 lMemberNumber, Int64 lMemberLoteId)
        {
            try
            {
                using (Library_AMFC_SQL obj_AMFC_SQL = new Library_AMFC_SQL())
                    return obj_AMFC_SQL.Get_Member_ContaCorrente_CEDEN(lMemberNumber, lMemberLoteId);
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return null;
            }
        }

        /// <versions>22-03-2018(GesAMFC-v1.0.0.3)</versions>
        private Int32 Get_DBF_Member_Lote_Total_Payments_Reg_CEDEN(Int64 lMemberNumber, Int64 lMemberLoteId)
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
        private Double Get_DBF_Member_Lote_Total_Valor_Reg_CEDEN(Int64 lMemberNumber, Int64 lMemberLoteId)
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
        private Double Get_DBF_Member_Lote_Total_Area_Reg_CEDEN(Int64 lMemberNumber, Int64 lMemberLoteId)
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
        private Boolean Save_Member_Lote_CEDEN()
        {
            try
            {
                if (this.Member == null || this.Member.NUMERO < this.Member.MinNumber || this.Member.NUMERO > this.Member.MaxNumber)
                    return false;
                if (ComboBoxEdit_Select_Lote_CEDEN.SelectedIndex < 0)
                    return false;
                ComboboxItem objItemLote = (ComboBoxEdit_Select_Lote_CEDEN.SelectedItem as ComboboxItem);
                Int64 lLoteId = Convert.ToInt64(objItemLote.GetValue());
                if (lLoteId < 1)
                    return false;
                AMFCMemberLote objLote = this.ListMemberLotes.GetLoteById(lLoteId);
                if (objLote == null || objLote.IDLOTE < 1 || String.IsNullOrEmpty(objLote.NUMLOTE) )
                    return false;

                AMFC_ContaCorrente_CEDEN objCCCEDEN = new AMFC_ContaCorrente_CEDEN();

                objCCCEDEN.SOCNUM = this.Member.NUMERO;
                objCCCEDEN.SOCNOME = this.Member.NOME;
                objCCCEDEN.IDLOTE = objLote.IDLOTE;
                objCCCEDEN.NUMLOTE = objLote.NUMLOTE;

                objCCCEDEN.AREA = objLote.AREALOTES;
                Double dbCEDEN_CederPercentagem_Text = Program.SetAreaDoubleValue(TextEdit_Selected_Lote_Percentagem_Ceder_CEDEN.Text);
                Double dbCEDEN_CederPercentagem_Decimal = Convert.ToDouble(Decimal.Divide(Convert.ToDecimal(dbCEDEN_CederPercentagem_Text), 100));
                objCCCEDEN.CEDERPERC = dbCEDEN_CederPercentagem_Decimal;
                objCCCEDEN.AREAPAGAR = Program.SetAreaDoubleValue(TextEdit_Selected_Lote_Area_Pagar_CEDEN.Text);
                objCCCEDEN.PRECOM2 = Program.SetPayCurrencyEuroDoubleValue(TextEdit_Selected_Lote_Preco_M2_CEDEN.Text);
                objCCCEDEN.VALORPAGAR = Program.SetPayCurrencyEuroDoubleValue(TextEdit_Selected_Lote_Total_Pagar_CEDEN.Text);

                objCCCEDEN.DATA = DateEdit_Total_Pago_Data_CEDEN.DateTime;
                objCCCEDEN.PRECOM2P = Program.SetPayCurrencyEuroDoubleValue(TextEdit_Total_Pago_Preco_Metro_CEDEN.Text);
                objCCCEDEN.AREAPAGO = Program.SetAreaDoubleValue(TextEdit_Total_Pago_Metros_CEDEN.Text);
                objCCCEDEN.VALORESCUD = Convert.ToDouble(TextEdit_Total_Pago_Valor_Escudos_CEDEN.Text);
                objCCCEDEN.VALORPAGO = Program.SetPayCurrencyEuroDoubleValue(TextEdit_Total_Pago_Valor_CEDEN.Text);
                objCCCEDEN.NOTASPAGO = TextEdit_Total_Pago_Notas_CEDEN.Text.Trim();

                objCCCEDEN.PRECOM2F = Program.SetPayCurrencyEuroDoubleValue(TextEdit_Falta_Pagar_Preco_Metro_CEDEN.Text);
                objCCCEDEN.AREAFALTA = Program.SetAreaDoubleValue(TextEdit_Falta_Pagar_Metros_CEDEN.Text);
                objCCCEDEN.VALORFALTA = Program.SetPayCurrencyEuroDoubleValue(TextEdit_Falta_Pagar_Valor_CEDEN.Text);
                objCCCEDEN.NOTASFALTA = TextEdit_Falta_Pagar_Notas_CEDEN.Text.Trim();

                if (ComboBoxEdit_Estado_Liquidacao_CEDEN.SelectedIndex > -1)
                    objCCCEDEN.ESTADOLIQ = ComboBoxEdit_Estado_Liquidacao_CEDEN.SelectedText;
                objCCCEDEN.NOTASLIQ = TextEdit_Liquidacao_Notas_CEDEN.Text.Trim();

                if (!Set_DBF_Member_Lote_CEDEN(objCCCEDEN))
                    return false;

                String sMessageOK = "Pagamentos de " + "Cedências" +" guardados para o Lote Nº: " + objCCCEDEN.NUMLOTE + " (ID=" + objCCCEDEN.IDLOTE + ")" + " do Sócio: " + objCCCEDEN.SOCNOME + " Nº: " + objCCCEDEN.SOCNUM;
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
        private Boolean Set_DBF_Member_Lote_CEDEN(AMFC_ContaCorrente_CEDEN objCCCEDEN)
        {
            try
            {
                using (Library_AMFC_SQL obj_AMFC_SQL = new Library_AMFC_SQL())
                    return obj_AMFC_SQL.Save_DB_Lote_CEDEN(objCCCEDEN);
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return false;
            }
        }

        #endregion  CEDEN

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
        private void TextEdit_Total_Pago_Valor_Escudos_CEDEN_EditValueChanged(object sender, EventArgs e)
        {
            try
            {
                Double dbEscudos = Program.SetAreaDoubleValue(this.TextEdit_Total_Pago_Valor_Escudos_CEDEN.Text);
                if (dbEscudos <= 0)
                    return;
                Double dbEuros = Convert.ToDouble(Decimal.Divide(Convert.ToDecimal(dbEscudos), Convert.ToDecimal(this.OneEuroToEscudos)));
                this.TextEdit_Total_Pago_Valor_CEDEN.Text = String.Format(Program.FormatString_Double3_DecimalPlaces, dbEuros);
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, false, false);
            }
        }
    }
}