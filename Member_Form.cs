using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Views.Layout;
using GesAMFC.AMFC_Methods;
using System;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Windows.Forms;

namespace GesAMFC
{
    /// <summary>AMFC Members Admin</summary>
    /// <author>Valter Lima</author>
    /// <creation>08-03-2017(v0.0.1.9)</creation>
    /// <versions>09-12-2017(GesAMFC-v1.0.0.1)</versions>
    public partial class Member_Form : DevExpress.XtraEditors.XtraForm
    {
        public Library_AMFC_Methods LibAMFC;

        public AMFCMember Member  { get; set; }

        public Library_AMFC_Methods.MemberOperationType Operation { get; set; }
        
        #region     Constructor

        /// <versions>05-05-2017(v0.0.2.56)</versions>
        public Member_Form(AMFCMember objMember, Library_AMFC_Methods.MemberOperationType eOperation)
        {
            LibAMFC = new Library_AMFC_Methods();
            try
            {
                InitializeComponent();

                Program.Member_Added  = false;
                Program.Member_Edited = false;

                if (eOperation == Library_AMFC_Methods.MemberOperationType.EDIT && (objMember == null || objMember.NUMERO < 0 || String.IsNullOrEmpty(objMember.NOME))) //....
                    return;
                Member = new AMFCMember();
                if (eOperation == Library_AMFC_Methods.MemberOperationType.EDIT)
                    Member = objMember;
                Operation = eOperation;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        #endregion  Constructor

        #region     Events

        /// <versions>05-05-2017(v0.0.2.56)</versions>
        private void Member_Form_Load(object sender, EventArgs e)
        {
            try
            {
                this.WindowState = FormWindowState.Maximized;
                this.Update();
                if (this.Operation == Library_AMFC_Methods.MemberOperationType.EDIT && (this.Member == null || this.Member.NUMERO < 0 || String.IsNullOrEmpty(this.Member.NOME))) //....
                    return;
                MemberForm_LoadLayoutView(true, true, true, false);
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>20-03-2017(v0.0.1.12)</versions>
        private void LayoutView_Member_Form_ShowingEditor(object sender, System.ComponentModel.CancelEventArgs e)
        {
            try
            {
                LayoutView view = sender as LayoutView;
                if (view.FocusedColumn.FieldName == new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMERO))
                    e.Cancel = true;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>20-03-2017(v0.0.1.12)</versions>
        private void LayoutView_Member_Form_CustomDrawCardFieldValue(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
        {
            try
            {
                //Int32 iMemberNumber = Convert.ToInt32((sender as LayoutView).GetRowCellValue(e.RowHandle, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMERO)));
                //if (e.Column.FieldName == new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMERO))
                //    return;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>20-03-2017(v0.0.1.12)</versions>
        private void LayoutView_Member_Form_CustomFieldValueStyle(object sender, DevExpress.XtraGrid.Views.Layout.Events.LayoutViewFieldValueStyleEventArgs e)
        {
            try
            {
                //e.Appearance.BackColor = Color.White;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>20-03-2017(v0.0.1.12)</versions>
        private void LayoutView_Member_Form_FocusedColumnChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedColumnChangedEventArgs e)
        {
            try
            {
                if (e.FocusedColumn != null)
                {
                    e.FocusedColumn.AppearanceCell.ForeColor = Color.DarkBlue;
                    e.FocusedColumn.AppearanceCell.BackColor = Color.White;
                    e.FocusedColumn.AppearanceCell.BackColor2 = Color.White;
                    e.FocusedColumn.AppearanceCell.BorderColor = Color.Green;
                }
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>21-02-2017(v0.0.1.6)</versions>
        private void Member_Form_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {

        }

        /// <versions>05-05-2017(v0.0.2.56)</versions>
        private void SimpleButton_Member_Save_Click(object sender, EventArgs e)
        {
            try
            {
                Int32 iFocusedRowHandle = this.LayoutView_Member_Form.FocusedRowHandle;
                if (iFocusedRowHandle < 0)
                    return;
                AMFCMember objMember = new AMFCMember();
                #region     Member Data
                objMember.NOME          = Convert.ToString(this.LayoutView_Member_Form.GetRowCellValue(iFocusedRowHandle,   new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NOME)));
                objMember.MORADA        = Convert.ToString(this.LayoutView_Member_Form.GetRowCellValue(iFocusedRowHandle,   new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.MORADA)));
                objMember.CPOSTAL       = Convert.ToString(this.LayoutView_Member_Form.GetRowCellValue(iFocusedRowHandle,   new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.CPOSTAL)));
                objMember.TELEFONE      = Convert.ToString(this.LayoutView_Member_Form.GetRowCellValue(iFocusedRowHandle,   new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.TELEFONE)));
                objMember.TELEMOVEL     = Convert.ToString(this.LayoutView_Member_Form.GetRowCellValue(iFocusedRowHandle,   new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.TELEMOVEL)));
                objMember.CC = Convert.ToString(this.LayoutView_Member_Form.GetRowCellValue(iFocusedRowHandle, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.CC)));
                objMember.NIF = Convert.ToString(this.LayoutView_Member_Form.GetRowCellValue(iFocusedRowHandle, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NIF)));
                objMember.EMAIL         = Convert.ToString(this.LayoutView_Member_Form.GetRowCellValue(iFocusedRowHandle,   new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.EMAIL)));
                objMember.MORLOTE       = Convert.ToString(this.LayoutView_Member_Form.GetRowCellValue(iFocusedRowHandle,   new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.MORLOTE)));
                objMember.NUMLOTE       = Convert.ToString(this.LayoutView_Member_Form.GetRowCellValue(iFocusedRowHandle,   new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMLOTE)));
                objMember.AREALOTE      = Convert.ToInt32(this.LayoutView_Member_Form.GetRowCellValue(iFocusedRowHandle,    new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.AREALOTE)).ToString().Trim());
                objMember.PROFISSAO     = Convert.ToString(this.LayoutView_Member_Form.GetRowCellValue(iFocusedRowHandle,   new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.PROFISSAO)));
                objMember.DATAADMI      = Convert.ToString(this.LayoutView_Member_Form.GetRowCellValue(iFocusedRowHandle,   new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.DATAADMI)));
                objMember.OBSERVACAO    = Convert.ToString(this.LayoutView_Member_Form.GetRowCellValue(iFocusedRowHandle,   new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.OBSERVACAO)));
                objMember.SECTOR        = Convert.ToString(this.LayoutView_Member_Form.GetRowCellValue(iFocusedRowHandle,   new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.SECTOR)));
                objMember.NUMFOGOS      = Convert.ToString(this.LayoutView_Member_Form.GetRowCellValue(iFocusedRowHandle,   new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMFOGOS)));
                objMember.NUMFILHOS     = Convert.ToString(this.LayoutView_Member_Form.GetRowCellValue(iFocusedRowHandle,   new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMFILHOS)));
                objMember.AGREFAMIL     = Convert.ToString(this.LayoutView_Member_Form.GetRowCellValue(iFocusedRowHandle,   new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.AGREFAMIL)));
                objMember.LADOMAIOR     = Convert.ToString(this.LayoutView_Member_Form.GetRowCellValue(iFocusedRowHandle,   new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.LADOMAIOR)));
                objMember.CASA          = Convert.ToBoolean(this.LayoutView_Member_Form.GetRowCellValue(iFocusedRowHandle,  new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.CASA)));
                objMember.GARAGEM       = Convert.ToBoolean(this.LayoutView_Member_Form.GetRowCellValue(iFocusedRowHandle,  new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.GARAGEM)));
                objMember.MUROS         = Convert.ToBoolean(this.LayoutView_Member_Form.GetRowCellValue(iFocusedRowHandle,  new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.MUROS)));
                objMember.POCO          = Convert.ToBoolean(this.LayoutView_Member_Form.GetRowCellValue(iFocusedRowHandle,  new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.POCO)));
                objMember.FURO          = Convert.ToBoolean(this.LayoutView_Member_Form.GetRowCellValue(iFocusedRowHandle,  new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.FURO)));
                objMember.SANEAMENTO    = Convert.ToBoolean(this.LayoutView_Member_Form.GetRowCellValue(iFocusedRowHandle,  new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.SANEAMENTO)));
                objMember.ELECTRICID    = Convert.ToBoolean(this.LayoutView_Member_Form.GetRowCellValue(iFocusedRowHandle,  new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.ELECTRICID)));
                objMember.PROJECTO      = Convert.ToBoolean(this.LayoutView_Member_Form.GetRowCellValue(iFocusedRowHandle,  new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.PROJECTO)));
                objMember.ESCRITURA     = Convert.ToBoolean(this.LayoutView_Member_Form.GetRowCellValue(iFocusedRowHandle,  new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.ESCRITURA)));
                objMember.FINANCAS      = Convert.ToBoolean(this.LayoutView_Member_Form.GetRowCellValue(iFocusedRowHandle,  new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.FINANCAS)));
                objMember.RESIDENCIA    = Convert.ToBoolean(this.LayoutView_Member_Form.GetRowCellValue(iFocusedRowHandle,  new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.RESIDENCIA)));
                objMember.GAVETO        = Convert.ToBoolean(this.LayoutView_Member_Form.GetRowCellValue(iFocusedRowHandle,  new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.GAVETO)));
                objMember.QUINTINHA     = Convert.ToBoolean(this.LayoutView_Member_Form.GetRowCellValue(iFocusedRowHandle,  new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.QUINTINHA)));
                objMember.MAIS1FOGO     = Convert.ToBoolean(this.LayoutView_Member_Form.GetRowCellValue(iFocusedRowHandle,  new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.MAIS1FOGO)));
                objMember.HABCOLECT     = Convert.ToBoolean(this.LayoutView_Member_Form.GetRowCellValue(iFocusedRowHandle,  new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.HABCOLECT)));
                objMember.ARECOMERC     = Convert.ToBoolean(this.LayoutView_Member_Form.GetRowCellValue(iFocusedRowHandle,  new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.ARECOMERC)));
                #endregion  Member Data
                switch (Operation)
                {
                    case Library_AMFC_Methods.MemberOperationType.ADD:
                        objMember.NUMERO = Convert.ToInt64(this.LayoutView_Member_Form.GetRowCellValue(iFocusedRowHandle, objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMERO)).ToString().Trim());
                        Int32 iAddStatus = DBF_AMFC_Add_Member(objMember);
                        if (iAddStatus == 1)
                        {
                            String sInfo = "Sócio " + objMember.NOME + " (Nº: " + objMember.NUMERO + ") criado com sucesso.";
                            MessageBox.Show(sInfo, "Sócio adicionado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            Program.Member_Added = true;
                            this.Close();
                        }
                        else if (iAddStatus == -1)
                        {
                            String sError = "Ocorreu um erro na criação do Sócio " + objMember.NOME + " (Nº: " + objMember.NUMERO + ")!";
                            MessageBox.Show(sError, "Erro Criação Sócio", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Set_Member_List_Details(objMember, true);
                            Program.Member_Added = false;
                        }
                        else if (iAddStatus == 0)
                            return;
                        break;
                    case Library_AMFC_Methods.MemberOperationType.EDIT:
                        objMember.NUMERO = this.Member.NUMERO;
                        Int32 iEditStatus = DBF_AMFC_Edit_Member(objMember);
                        if (iEditStatus == 1)
                        {
                            String sInfo = "Sócio " + objMember.NOME + " (Nº: " + objMember.NUMERO + ") editado com sucesso.";
                            MessageBox.Show(sInfo, "Sócio editado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            Program.Member_Edited = true;
                            this.Close();
                        }
                        else if (iEditStatus == -1)
                        {
                            String sError = "Ocorreu um erro na edição do Sócio " + objMember.NOME + " (Nº: " + objMember.NUMERO + ")!";
                            MessageBox.Show(sError, "Erro Edição Sócio", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Set_Member_List_Details(objMember, false);
                            Program.Member_Edited = false;
                        }
                        else if (iEditStatus == 0)
                            return;
                        break;
                }
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return;
            }
        }

        /// <versions>21-02-2017(v0.0.1.6)</versions>
        private void SimpleButton_Member_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <versions>21-04-2017(v0.0.1.27)</versions>
        private void SimpleButton_Member_Reload_Click(object sender, EventArgs e)
        {
            try
            {
                switch (Operation)
                {
                    case Library_AMFC_Methods.MemberOperationType.EDIT:
                        Set_Member_List_Details(this.Member, false);
                        break;
                    case Library_AMFC_Methods.MemberOperationType.ADD:
                        #region     Get Members Max Number
                        Int32 iMemberMaxNumber = DBF_AMFC_Members_GetMaxNumber();
                        if (iMemberMaxNumber < 1)
                        {
                            String sWarning = "Não foi possivel obter o número máximo dos Sócios! Por favor, contacte o programador!";
                            MessageBox.Show(sWarning, "Erro: Sócio Máximo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                        #endregion  Get Members Max Number
                        AMFCMember objMember = new AMFCMember();
                        objMember.NUMERO = iMemberMaxNumber + 1;
                        Set_Member_List_Details(objMember, true);
                        break;
                }
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return;
            }
        }

        #endregion  Events

        #region     Methods

        /// <versions>23-04-2017(v0.0.1.31)</versions>
        private Boolean Set_Member_List_Details(AMFCMember objMember, Boolean bLoadNumber)
        {
            try
            {
                if (bLoadNumber)
                {
                    this.LayoutView_Member_Form.SetFocusedRowCellValue(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMERO), objMember.NUMERO.ToString());
                    this.LayoutView_Member_Form.Columns[new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMERO)].Visible = true;
                    this.LayoutView_Member_Form.Columns[new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMERO)].OptionsColumn.ReadOnly = false;
                    this.LayoutView_Member_Form.Columns[new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMERO)].OptionsColumn.AllowEdit = true;
                    this.LayoutView_Member_Form.Columns[new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMERO)].OptionsColumn.AllowFocus = true;
                }
                else
                {
                    //this.LayoutView_Member_Form.SetFocusedRowCellValue(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMERO), objMember.NUMERO.ToString());
                    this.LayoutView_Member_Form.Columns[new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMERO)].Visible = false;
                    this.LayoutView_Member_Form.Columns[new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMERO)].OptionsColumn.ReadOnly = true;
                    this.LayoutView_Member_Form.Columns[new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMERO)].OptionsColumn.AllowEdit = false;
                    this.LayoutView_Member_Form.Columns[new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMERO)].OptionsColumn.AllowFocus = false;
                }
                this.LayoutView_Member_Form.SetFocusedRowCellValue(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NOME),        objMember.NOME);
                this.LayoutView_Member_Form.SetFocusedRowCellValue(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.MORADA),      objMember.MORADA);
                this.LayoutView_Member_Form.SetFocusedRowCellValue(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.CPOSTAL),     objMember.CPOSTAL);
                this.LayoutView_Member_Form.SetFocusedRowCellValue(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.TELEFONE),    objMember.TELEFONE);
                this.LayoutView_Member_Form.SetFocusedRowCellValue(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.TELEMOVEL),   objMember.TELEMOVEL);
                this.LayoutView_Member_Form.SetFocusedRowCellValue(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.CC), objMember.CC);
                this.LayoutView_Member_Form.SetFocusedRowCellValue(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NIF), objMember.NIF);
                this.LayoutView_Member_Form.SetFocusedRowCellValue(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.EMAIL),       objMember.EMAIL);
                this.LayoutView_Member_Form.SetFocusedRowCellValue(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.MORLOTE),     objMember.MORLOTE);
                this.LayoutView_Member_Form.SetFocusedRowCellValue(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMLOTE),     objMember.NUMLOTE);
                this.LayoutView_Member_Form.SetFocusedRowCellValue(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.AREALOTE),    objMember.AREALOTE.ToString());
                this.LayoutView_Member_Form.SetFocusedRowCellValue(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.PROFISSAO),   objMember.PROFISSAO);
                this.LayoutView_Member_Form.SetFocusedRowCellValue(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.DATAADMI),    objMember.DATAADMI);
                this.LayoutView_Member_Form.SetFocusedRowCellValue(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.OBSERVACAO),  objMember.OBSERVACAO);
                this.LayoutView_Member_Form.SetFocusedRowCellValue(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.SECTOR),      objMember.SECTOR);
                this.LayoutView_Member_Form.SetFocusedRowCellValue(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.CASA),        objMember.CASA);
                this.LayoutView_Member_Form.SetFocusedRowCellValue(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.GARAGEM),     objMember.GARAGEM);
                this.LayoutView_Member_Form.SetFocusedRowCellValue(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.MUROS),       objMember.MUROS);
                this.LayoutView_Member_Form.SetFocusedRowCellValue(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.POCO),        objMember.POCO);
                this.LayoutView_Member_Form.SetFocusedRowCellValue(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.FURO),        objMember.FURO);
                this.LayoutView_Member_Form.SetFocusedRowCellValue(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.SANEAMENTO),  objMember.SANEAMENTO);
                this.LayoutView_Member_Form.SetFocusedRowCellValue(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.ELECTRICID),  objMember.ELECTRICID);
                this.LayoutView_Member_Form.SetFocusedRowCellValue(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.PROJECTO),    objMember.PROJECTO);
                this.LayoutView_Member_Form.SetFocusedRowCellValue(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.ESCRITURA),   objMember.ESCRITURA);
                this.LayoutView_Member_Form.SetFocusedRowCellValue(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.FINANCAS),    objMember.FINANCAS);
                this.LayoutView_Member_Form.SetFocusedRowCellValue(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.RESIDENCIA),  objMember.RESIDENCIA);
                this.LayoutView_Member_Form.SetFocusedRowCellValue(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.AGREFAMIL),   objMember.AGREFAMIL);
                this.LayoutView_Member_Form.SetFocusedRowCellValue(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMFILHOS),   objMember.NUMFILHOS);
                this.LayoutView_Member_Form.SetFocusedRowCellValue(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.GAVETO),      objMember.GAVETO);
                this.LayoutView_Member_Form.SetFocusedRowCellValue(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.QUINTINHA),   objMember.QUINTINHA);
                this.LayoutView_Member_Form.SetFocusedRowCellValue(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.LADOMAIOR),   objMember.LADOMAIOR);
                this.LayoutView_Member_Form.SetFocusedRowCellValue(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.MAIS1FOGO),   objMember.MAIS1FOGO);
                this.LayoutView_Member_Form.SetFocusedRowCellValue(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.HABCOLECT),   objMember.HABCOLECT);
                this.LayoutView_Member_Form.SetFocusedRowCellValue(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMFOGOS),    objMember.NUMFOGOS);
                this.LayoutView_Member_Form.SetFocusedRowCellValue(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.ARECOMERC),   objMember.ARECOMERC);
                
                return true;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return false;
            }
        }

        /// <versions>20-03-2017(v0.0.1.12)</versions>
        private void MemberForm_LoadLayoutView(Boolean bSetCols, Boolean bClearSorting, Boolean bClearFilters, Boolean bWriteLog)
        {
            try
            {
                #region     Operation Label
                this.Label_Member_Form.Text = "Adicionar/Editar Sócio";
                switch (Operation)
                {
                    case Library_AMFC_Methods.MemberOperationType.ADD:
                        this.Text = "Criar Sócio";
                        this.Label_Member_Form.Text = "Novo Sócio";
                        break;

                    case Library_AMFC_Methods.MemberOperationType.EDIT:
                        this.Text = "Editar Sócio";
                        this.Label_Member_Form.Text = "Sócio Nº: " + this.Member.NUMERO;
                        break;
                }
                #endregion  Operation Label

                MemberForm_CleanGridLayoutView(this.GridControl_Member_Form, this.LayoutView_Member_Form, bSetCols, bClearSorting, bClearFilters);
                this.GridControl_Member_Form.Visible = false;
                this.Update();

                Boolean bLoadDatasource = MemberForm_SetDataSource(this.GridControl_Member_Form);
                if (bLoadDatasource && bSetCols)
                {
                    Add_Member_LayoutView_Columns_Options();
                }

                //this.LoadingBox_Brands.Hide();
                this.GridControl_Member_Form.Visible = true;
                this.Update();
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>05-10-2017(v0.0.4.6)</versions>
        private Boolean MemberForm_SetDataSource(GridControl objGridControl)
        {
            String sErrorMsg = String.Empty;
            try
            {
                if (this.Operation == Library_AMFC_Methods.MemberOperationType.EDIT && (this.Member == null || this.Member.NUMERO < 0 || String.IsNullOrEmpty(this.Member.NOME)))
                    return false;

                #region     Debug
                //int i = 0;
                //if (objMember.NUMERO != 105)
                //    i = 1;
                #endregion  Debug

                #region     TreeDataSet
                DataTable objDataTable_Members_List = new DataTable("Members_Form");
                #region     Data Columns Creation
                DataColumn objDataColumn_Member_NUMERO      = new DataColumn(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMERO),       typeof(String));
                DataColumn objDataColumn_Member_NOME        = new DataColumn(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NOME),         typeof(String));
                DataColumn objDataColumn_Member_MORADA      = new DataColumn(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.MORADA),       typeof(String));
                DataColumn objDataColumn_Member_CPOSTAL     = new DataColumn(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.CPOSTAL),      typeof(String));
                DataColumn objDataColumn_Member_TELEFONE    = new DataColumn(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.TELEFONE),     typeof(String));
                DataColumn objDataColumn_Member_TELEMOVEL   = new DataColumn(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.TELEMOVEL),    typeof(String));
                DataColumn objDataColumn_Member_CC = new DataColumn(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.CC), typeof(String));
                DataColumn objDataColumn_Member_NIF = new DataColumn(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NIF), typeof(String));
                DataColumn objDataColumn_Member_EMAIL       = new DataColumn(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.EMAIL),        typeof(String));
                DataColumn objDataColumn_Member_MORLOTE     = new DataColumn(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.MORLOTE),      typeof(String));
                DataColumn objDataColumn_Member_NUMLOTE     = new DataColumn(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMLOTE),      typeof(String));
                DataColumn objDataColumn_Member_AREALOTE    = new DataColumn(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.AREALOTE),     typeof(String));
                DataColumn objDataColumn_Member_PROFISSAO   = new DataColumn(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.PROFISSAO),    typeof(String));
                DataColumn objDataColumn_Member_DATAADMI    = new DataColumn(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.DATAADMI),     typeof(String));
                DataColumn objDataColumn_Member_OBSERVACAO  = new DataColumn(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.OBSERVACAO),   typeof(String));
                DataColumn objDataColumn_Member_SECTOR      = new DataColumn(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.SECTOR),       typeof(String));
                DataColumn objDataColumn_Member_CASA        = new DataColumn(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.CASA),         typeof(Boolean));
                DataColumn objDataColumn_Member_GARAGEM     = new DataColumn(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.GARAGEM),      typeof(Boolean));
                DataColumn objDataColumn_Member_MUROS       = new DataColumn(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.MUROS),        typeof(Boolean));
                DataColumn objDataColumn_Member_POCO        = new DataColumn(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.POCO),         typeof(Boolean));
                DataColumn objDataColumn_Member_FURO        = new DataColumn(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.FURO),         typeof(Boolean));
                DataColumn objDataColumn_Member_SANEAMENTO  = new DataColumn(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.SANEAMENTO),   typeof(Boolean));
                DataColumn objDataColumn_Member_ELECTRICID  = new DataColumn(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.ELECTRICID),   typeof(Boolean));
                DataColumn objDataColumn_Member_PROJECTO    = new DataColumn(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.PROJECTO),     typeof(Boolean));
                DataColumn objDataColumn_Member_ESCRITURA   = new DataColumn(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.ESCRITURA),    typeof(Boolean));
                DataColumn objDataColumn_Member_FINANCAS    = new DataColumn(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.FINANCAS),     typeof(Boolean));
                DataColumn objDataColumn_Member_RESIDENCIA  = new DataColumn(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.RESIDENCIA),   typeof(Boolean));
                DataColumn objDataColumn_Member_AGREFAMIL   = new DataColumn(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.AGREFAMIL),    typeof(String)); //String
                DataColumn objDataColumn_Member_NUMFILHOS   = new DataColumn(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMFILHOS),    typeof(String)); //String
                DataColumn objDataColumn_Member_GAVETO      = new DataColumn(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.GAVETO),       typeof(Boolean));
                DataColumn objDataColumn_Member_QUINTINHA   = new DataColumn(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.QUINTINHA),    typeof(Boolean));
                DataColumn objDataColumn_Member_LADOMAIOR   = new DataColumn(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.LADOMAIOR),    typeof(String)); //String
                DataColumn objDataColumn_Member_MAIS1FOGO   = new DataColumn(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.MAIS1FOGO),    typeof(Boolean));
                DataColumn objDataColumn_Member_HABCOLECT   = new DataColumn(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.HABCOLECT),    typeof(Boolean));
                DataColumn objDataColumn_Member_NUMFOGOS    = new DataColumn(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMFOGOS),     typeof(String)); //String
                DataColumn objDataColumn_Member_ARECOMERC   = new DataColumn(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.ARECOMERC),    typeof(Boolean));
                #endregion  Data Columns Creation
                #region     Data Table Add Columns

                if (Operation == Library_AMFC_Methods.MemberOperationType.ADD)
                    objDataTable_Members_List.Columns.Add(objDataColumn_Member_NUMERO);

                objDataTable_Members_List.Columns.AddRange(
                                            new DataColumn[] {
                                                objDataColumn_Member_NOME,
                                                objDataColumn_Member_MORADA,
                                                objDataColumn_Member_CPOSTAL,
                                                objDataColumn_Member_TELEFONE,
                                                objDataColumn_Member_TELEMOVEL,
                                                objDataColumn_Member_CC,
                                                objDataColumn_Member_NIF,
                                                objDataColumn_Member_EMAIL,
                                                objDataColumn_Member_MORLOTE,
                                                objDataColumn_Member_NUMLOTE,
                                                objDataColumn_Member_AREALOTE,
                                                objDataColumn_Member_PROFISSAO,
                                                objDataColumn_Member_DATAADMI,
                                                objDataColumn_Member_OBSERVACAO,
                                                objDataColumn_Member_SECTOR,
                                                objDataColumn_Member_CASA,
                                                objDataColumn_Member_GARAGEM,
                                                objDataColumn_Member_MUROS,
                                                objDataColumn_Member_POCO,
                                                objDataColumn_Member_FURO,
                                                objDataColumn_Member_SANEAMENTO,
                                                objDataColumn_Member_ELECTRICID,
                                                objDataColumn_Member_PROJECTO,
                                                objDataColumn_Member_ESCRITURA,
                                                objDataColumn_Member_FINANCAS,
                                                objDataColumn_Member_RESIDENCIA,
                                                objDataColumn_Member_AGREFAMIL,
                                                objDataColumn_Member_NUMFILHOS,
                                                objDataColumn_Member_GAVETO,
                                                objDataColumn_Member_QUINTINHA,
                                                objDataColumn_Member_LADOMAIOR,
                                                objDataColumn_Member_MAIS1FOGO,
                                                objDataColumn_Member_HABCOLECT,
                                                objDataColumn_Member_NUMFOGOS,
                                                objDataColumn_Member_ARECOMERC
                                            }
                                        );
                #endregion  Data Table Add Columns
                #region     Data Columns Dispose
                objDataColumn_Member_NUMERO.Dispose();
                objDataColumn_Member_NOME.Dispose();
                objDataColumn_Member_MORADA.Dispose();
                objDataColumn_Member_CPOSTAL.Dispose();
                objDataColumn_Member_TELEFONE.Dispose();
                objDataColumn_Member_TELEMOVEL.Dispose();
                objDataColumn_Member_CC.Dispose();
                objDataColumn_Member_NIF.Dispose();
                objDataColumn_Member_EMAIL.Dispose();
                objDataColumn_Member_MORLOTE.Dispose();
                objDataColumn_Member_NUMLOTE.Dispose();
                objDataColumn_Member_AREALOTE.Dispose();
                objDataColumn_Member_PROFISSAO.Dispose();
                objDataColumn_Member_DATAADMI.Dispose();
                objDataColumn_Member_OBSERVACAO.Dispose();
                objDataColumn_Member_SECTOR.Dispose();
                objDataColumn_Member_CASA.Dispose();
                objDataColumn_Member_GARAGEM.Dispose();
                objDataColumn_Member_MUROS.Dispose();
                objDataColumn_Member_POCO.Dispose();
                objDataColumn_Member_FURO.Dispose();
                objDataColumn_Member_SANEAMENTO.Dispose();
                objDataColumn_Member_ELECTRICID.Dispose();
                objDataColumn_Member_PROJECTO.Dispose();
                objDataColumn_Member_ESCRITURA.Dispose();
                objDataColumn_Member_FINANCAS.Dispose();
                objDataColumn_Member_RESIDENCIA.Dispose();
                objDataColumn_Member_AGREFAMIL.Dispose();
                objDataColumn_Member_NUMFILHOS.Dispose();
                objDataColumn_Member_GAVETO.Dispose();
                objDataColumn_Member_QUINTINHA.Dispose();
                objDataColumn_Member_LADOMAIOR.Dispose();
                objDataColumn_Member_MAIS1FOGO.Dispose();
                objDataColumn_Member_HABCOLECT.Dispose();
                objDataColumn_Member_NUMFOGOS.Dispose();
                objDataColumn_Member_ARECOMERC.Dispose();
                #endregion  Data Columns Dispose
                #endregion  TreeDataSet

                #region     Binding Data

                #region  Set Member Row Data
                DataRow objDataRow = objDataTable_Members_List.NewRow();
                switch (Operation)
                {
                    case Library_AMFC_Methods.MemberOperationType.EDIT:
                        #region     Edit Member
                        LayoutViewField_layoutViewColumn_Member_Form_Number.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                        objDataRow[this.Member.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NOME)]        = this.Member.NOME;
                        objDataRow[this.Member.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.MORADA)]      = this.Member.MORADA;
                        objDataRow[this.Member.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.CPOSTAL)]     = this.Member.CPOSTAL;
                        objDataRow[this.Member.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.TELEFONE)]    = this.Member.TELEFONE;
                        objDataRow[this.Member.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.TELEMOVEL)]   = this.Member.TELEMOVEL;
                        objDataRow[this.Member.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.CC)] = this.Member.CC;
                        objDataRow[this.Member.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NIF)] = this.Member.NIF;
                        objDataRow[this.Member.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.EMAIL)]       = this.Member.EMAIL;
                        objDataRow[this.Member.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.MORLOTE)]     = this.Member.MORLOTE;
                        objDataRow[this.Member.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMLOTE)]     = this.Member.NUMLOTE.ToString();
                        objDataRow[this.Member.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.AREALOTE)]    = this.Member.AREALOTE;
                        objDataRow[this.Member.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.PROFISSAO)]   = this.Member.PROFISSAO;
                        objDataRow[this.Member.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.DATAADMI)]    = this.Member.DATAADMI;
                        objDataRow[this.Member.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.OBSERVACAO)]  = this.Member.OBSERVACAO;
                        objDataRow[this.Member.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.SECTOR)]      = this.Member.SECTOR;
                        objDataRow[this.Member.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.CASA)]        = this.Member.CASA;
                        objDataRow[this.Member.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.GARAGEM)]     = this.Member.GARAGEM;
                        objDataRow[this.Member.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.MUROS)]       = this.Member.MUROS;
                        objDataRow[this.Member.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.POCO)]        = this.Member.POCO;
                        objDataRow[this.Member.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.FURO)]        = this.Member.FURO;
                        objDataRow[this.Member.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.SANEAMENTO)]  = this.Member.SANEAMENTO;
                        objDataRow[this.Member.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.ELECTRICID)]  = this.Member.ELECTRICID;
                        objDataRow[this.Member.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.PROJECTO)]    = this.Member.PROJECTO;
                        objDataRow[this.Member.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.ESCRITURA)]   = this.Member.ESCRITURA;
                        objDataRow[this.Member.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.FINANCAS)]    = this.Member.FINANCAS;
                        objDataRow[this.Member.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.RESIDENCIA)]  = this.Member.RESIDENCIA;
                        objDataRow[this.Member.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.AGREFAMIL)]   = this.Member.AGREFAMIL;
                        objDataRow[this.Member.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMFILHOS)]   = this.Member.NUMFILHOS;
                        objDataRow[this.Member.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.GAVETO)]      = this.Member.GAVETO;
                        objDataRow[this.Member.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.QUINTINHA)]   = this.Member.QUINTINHA;
                        objDataRow[this.Member.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.LADOMAIOR)]   = this.Member.LADOMAIOR;
                        objDataRow[this.Member.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.MAIS1FOGO)]   = this.Member.MAIS1FOGO;
                        objDataRow[this.Member.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.HABCOLECT)]   = this.Member.HABCOLECT;
                        objDataRow[this.Member.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMFOGOS)]    = this.Member.NUMFOGOS;
                        objDataRow[this.Member.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.ARECOMERC)]   = this.Member.ARECOMERC;
                        #endregion  Edit Member
                        break;
                    case Library_AMFC_Methods.MemberOperationType.ADD:
                        LayoutViewField_layoutViewColumn_Member_Form_Number.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                        #region     Add Member
                        #region     Get Members Max Number
                        Int32 iMemberMaxNumber = DBF_AMFC_Members_GetMaxNumber();
                        if (iMemberMaxNumber < 1)
                        {
                            String sWarning = "Não foi possivel obter o número máximo dos Sócios! Por favor, contacte o programador!";
                            MessageBox.Show(sWarning, "Erro: Sócio Máximo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return false;
                        }
                        #endregion  Get Members Max Number
                        objDataRow[this.Member.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMERO)]      = (iMemberMaxNumber + 1).ToString();
                        objDataRow[this.Member.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NOME)]        = String.Empty;
                        objDataRow[this.Member.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.MORADA)]      = String.Empty;
                        objDataRow[this.Member.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.CPOSTAL)]     = String.Empty;
                        objDataRow[this.Member.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.TELEFONE)]    = String.Empty;
                        objDataRow[this.Member.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.TELEMOVEL)]   = String.Empty;
                        objDataRow[this.Member.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.CC)] = String.Empty;
                        objDataRow[this.Member.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NIF)] = String.Empty;
                        objDataRow[this.Member.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.EMAIL)]       = String.Empty;
                        objDataRow[this.Member.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.MORLOTE)]     = String.Empty;
                        objDataRow[this.Member.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMLOTE)]     = String.Empty;
                        objDataRow[this.Member.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.AREALOTE)]    = 0;
                        objDataRow[this.Member.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.PROFISSAO)]   = String.Empty;
                        objDataRow[this.Member.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.DATAADMI)]    = DateTime.Today.ToString(Program.Date_Format_String, CultureInfo.CurrentCulture);
                        objDataRow[this.Member.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.OBSERVACAO)]  = String.Empty;
                        objDataRow[this.Member.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.SECTOR)]      = String.Empty;
                        objDataRow[this.Member.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.CASA)]        = false;
                        objDataRow[this.Member.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.GARAGEM)]     = false;
                        objDataRow[this.Member.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.MUROS)]       = false;
                        objDataRow[this.Member.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.POCO)]        = false;
                        objDataRow[this.Member.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.FURO)]        = false;
                        objDataRow[this.Member.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.SANEAMENTO)]  = false;
                        objDataRow[this.Member.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.ELECTRICID)]  = false;
                        objDataRow[this.Member.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.PROJECTO)]    = false;
                        objDataRow[this.Member.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.ESCRITURA)]   = false;
                        objDataRow[this.Member.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.FINANCAS)]    = false;
                        objDataRow[this.Member.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.RESIDENCIA)]  = false;
                        objDataRow[this.Member.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.AGREFAMIL)]   = String.Empty; //
                        objDataRow[this.Member.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMFILHOS)]   = String.Empty; //
                        objDataRow[this.Member.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.GAVETO)]      = false;
                        objDataRow[this.Member.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.QUINTINHA)]   = false;
                        objDataRow[this.Member.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.LADOMAIOR)]   = String.Empty; //
                        objDataRow[this.Member.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.MAIS1FOGO)]   = false;
                        objDataRow[this.Member.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.HABCOLECT)]   = false;
                        objDataRow[this.Member.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMFOGOS)]    = String.Empty; //
                        objDataRow[this.Member.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.ARECOMERC)]   = false;

                        #if DEBUG
                        objDataRow[this.Member.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMERO)]      = 555;
                        objDataRow[this.Member.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NOME)]        = "Valter Sousa Lima";
                        objDataRow[this.Member.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.MORADA)]      = "Praceta Cidade de Chaves Lote 11A, Redondos";
                        objDataRow[this.Member.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.CPOSTAL)]     = "2865-696 Fernão Ferro";
                        objDataRow[this.Member.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.TELEFONE)]    = "931618502";
                        objDataRow[this.Member.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.TELEMOVEL)]   = "931618502";
                        objDataRow[this.Member.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.CC)] = "11556686 4ZY7";
                        objDataRow[this.Member.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NIF)] = "225174472";
                        objDataRow[this.Member.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.EMAIL)]       = "valtersousalima@gmail.com";
                        objDataRow[this.Member.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.MORLOTE)]     = "Praceta Cidade de Chaves Lote 11A, Redondos";
                        objDataRow[this.Member.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMLOTE)]     = "11A";
                        objDataRow[this.Member.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.AREALOTE)]    = 1000;
                        objDataRow[this.Member.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.PROFISSAO)]   = "Programdor Software";
                        objDataRow[this.Member.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.DATAADMI)]    = DateTime.Today.ToString(Program.Date_Format_String, CultureInfo.CurrentCulture);
                        objDataRow[this.Member.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.OBSERVACAO)]  = String.Empty;
                        objDataRow[this.Member.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.SECTOR)]      = String.Empty;
                        objDataRow[this.Member.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.CASA)]        = false;
                        objDataRow[this.Member.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.GARAGEM)]     = false;
                        objDataRow[this.Member.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.MUROS)]       = false;
                        objDataRow[this.Member.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.POCO)]        = false;
                        objDataRow[this.Member.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.FURO)]        = false;
                        objDataRow[this.Member.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.SANEAMENTO)]  = false;
                        objDataRow[this.Member.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.ELECTRICID)]  = false;
                        objDataRow[this.Member.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.PROJECTO)]    = false;
                        objDataRow[this.Member.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.ESCRITURA)]   = false;
                        objDataRow[this.Member.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.FINANCAS)]    = false;
                        objDataRow[this.Member.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.RESIDENCIA)]  = false;
                        objDataRow[this.Member.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.AGREFAMIL)]   = String.Empty; //
                        objDataRow[this.Member.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMFILHOS)]   = String.Empty; //
                        objDataRow[this.Member.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.GAVETO)]      = false;
                        objDataRow[this.Member.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.QUINTINHA)]   = false;
                        objDataRow[this.Member.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.LADOMAIOR)]   = String.Empty; //
                        objDataRow[this.Member.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.MAIS1FOGO)]   = false;
                        objDataRow[this.Member.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.HABCOLECT)]   = false;
                        objDataRow[this.Member.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMFOGOS)]    = String.Empty; //
                        objDataRow[this.Member.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.ARECOMERC)]   = false;
                        #endif

                        #endregion  Add Member
                        break;
                }
                objDataTable_Members_List.Rows.Add(objDataRow);
                #endregion  Set Member Row Data

                objGridControl.DataSource = objDataTable_Members_List;

                return true;
                #endregion  Binding Data
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return false;
            }
        }

        /// <versions>20-03-2017(v0.0.1.12)</versions>
        public void MemberForm_CleanGridLayoutView(GridControl objGridControl, LayoutView objLayoutView, Boolean bClearCols, Boolean bClearSorting, Boolean bClearFilters)
        {
            try
            {
                objGridControl.Visible = true;
                objGridControl.Show();
                objGridControl.DataSource = null;
                ////objLayoutView.OptionsView.ShowFooter = false;
                //objLayoutView.BeginUpdate();
                //if (bClearCols)
                //    objLayoutView.Columns.Clear();
                //if (bClearSorting)
                //    objLayoutView.ClearSorting();
                //if (bClearFilters)
                //{
                //    objLayoutView.OptionsFilter.Reset();
                //    objLayoutView.ActiveFilterEnabled = false;
                //}
                ////if (bClearGrouping)
                ////    objLayoutView.ClearGrouping();
                //objLayoutView.EndUpdate();
                objGridControl.Update();
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>22-04-2017(v0.0.1.29</versions>
        public void Add_Member_LayoutView_Columns_Options()
        {
            try
            {
                LibAMFC.Set_LayoutView_Column_Options(this.LayoutView_Member_Form, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMERO),     new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMERO),        "Número de sócio",  true, 1,    60,     false, true, true, false, false);
                LibAMFC.Set_LayoutView_Column_Options(this.LayoutView_Member_Form, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NOME),       new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.NOME),          "Nome do sócio",    true, 2,    60,     false, true, true, false, false);
                LibAMFC.Set_LayoutView_Column_Options(this.LayoutView_Member_Form, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.MORADA),     new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.MORADA),        "Morada do sócio",  true, 3,    60,     false, true, true, false, false);
                LibAMFC.Set_LayoutView_Column_Options(this.LayoutView_Member_Form, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.CPOSTAL),    new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.CPOSTAL),       "CPOSTAL",          true, 4,    60,     false, true, true, false, false);
                LibAMFC.Set_LayoutView_Column_Options(this.LayoutView_Member_Form, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.TELEFONE),   new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.TELEFONE),      "TELEFONE",         true, 5,    60,     false, true, true, false, false);
                LibAMFC.Set_LayoutView_Column_Options(this.LayoutView_Member_Form, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.TELEMOVEL),  new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.TELEMOVEL),     "TELEMOVEL",        true, 6,    60,     false, true, true, false, false);
                LibAMFC.Set_LayoutView_Column_Options(this.LayoutView_Member_Form, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.CC), new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.CC), "CC", true, 6, 60, false, true, true, false, false);
                LibAMFC.Set_LayoutView_Column_Options(this.LayoutView_Member_Form, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NIF), new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.NIF), "NIF", true, 6, 60, false, true, true, false, false);
                LibAMFC.Set_LayoutView_Column_Options(this.LayoutView_Member_Form, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.EMAIL),      new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.EMAIL),         "EMAIL",            true, 7,    60,     false, true, true, false, false);
                LibAMFC.Set_LayoutView_Column_Options(this.LayoutView_Member_Form, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.MORLOTE),    new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.MORLOTE),       "Morada do lote",   true, 8,    60,     false, true, true, false, false);
                LibAMFC.Set_LayoutView_Column_Options(this.LayoutView_Member_Form, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMLOTE),    new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMLOTE),       "Lote de sócio",    true, 9,    60,     false, true, true, false, false);
                LibAMFC.Set_LayoutView_Column_Options(this.LayoutView_Member_Form, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.AREALOTE),   new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.AREALOTE),      "Área do lote",     true, 10,   60,     false, true, true, false, false);
                LibAMFC.Set_LayoutView_Column_Options(this.LayoutView_Member_Form, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.PROFISSAO),  new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.PROFISSAO),     "PROFISSAO",        true, 11,   60,     false, true, true, false, false);
                LibAMFC.Set_LayoutView_Column_Options(this.LayoutView_Member_Form, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.DATAADMI),   new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.DATAADMI),      "DATAADMI",         true, 12,  100,     false, true, true, false, false);
                LibAMFC.Set_LayoutView_Column_Options(this.LayoutView_Member_Form, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.OBSERVACAO), new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.OBSERVACAO),    "OBSERVACAO",       true, 13,   60,     false, true, true, false, false);
                LibAMFC.Set_LayoutView_Column_Options(this.LayoutView_Member_Form, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.SECTOR),     new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.SECTOR),        "SECTOR",           true, 14,   60,     false, true, true, false, false);
                LibAMFC.Set_LayoutView_Column_Options(this.LayoutView_Member_Form, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.CASA),       new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.CASA),          "CASA",             true, 15,   10,     false, true, true, false, true);
                LibAMFC.Set_LayoutView_Column_Options(this.LayoutView_Member_Form, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.GARAGEM),    new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.GARAGEM),       "GARAGEM",          true, 16,   10,     false, true, true, false, true);
                LibAMFC.Set_LayoutView_Column_Options(this.LayoutView_Member_Form, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.MUROS),      new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.MUROS),         "MUROS",            true, 17,   10,     false, true, true, false, true);
                LibAMFC.Set_LayoutView_Column_Options(this.LayoutView_Member_Form, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.POCO),       new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.POCO),          "POCO",             true, 18,   10,     false, true, true, false, true);
                LibAMFC.Set_LayoutView_Column_Options(this.LayoutView_Member_Form, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.FURO),       new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.FURO),          "FURO",             true, 19,   10,     false, true, true, false, true);
                LibAMFC.Set_LayoutView_Column_Options(this.LayoutView_Member_Form, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.SANEAMENTO), new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.SANEAMENTO),    "SANEAMENTO",       true, 20,   10,     false, true, true, false, true);
                LibAMFC.Set_LayoutView_Column_Options(this.LayoutView_Member_Form, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.ELECTRICID), new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.ELECTRICID),    "ELECTRICID",       true, 21,   10,     false, true, true, false, true);
                LibAMFC.Set_LayoutView_Column_Options(this.LayoutView_Member_Form, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.PROJECTO),   new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.PROJECTO),      "PROJECTO",         true, 22,   10,     false, true, true, false, true);
                LibAMFC.Set_LayoutView_Column_Options(this.LayoutView_Member_Form, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.ESCRITURA),  new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.ESCRITURA),     "ESCRITURA",        true, 23,   10,     false, true, true, false, true);
                LibAMFC.Set_LayoutView_Column_Options(this.LayoutView_Member_Form, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.FINANCAS),   new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.FINANCAS),      "FINANCAS",         true, 24,   10,     false, true, true, false, true);
                LibAMFC.Set_LayoutView_Column_Options(this.LayoutView_Member_Form, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.RESIDENCIA), new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.RESIDENCIA),    "RESIDENCIA",       true, 25,   10,     false, true, true, false, true);
                LibAMFC.Set_LayoutView_Column_Options(this.LayoutView_Member_Form, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.AGREFAMIL),  new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.AGREFAMIL),     "AGREFAMIL",        true, 26,   10,     false, true, true, false, false);//
                LibAMFC.Set_LayoutView_Column_Options(this.LayoutView_Member_Form, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMFILHOS),  new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMFILHOS),     "NUMFILHOS",        true, 27,   10,     false, true, true, false, false);//
                LibAMFC.Set_LayoutView_Column_Options(this.LayoutView_Member_Form, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.GAVETO),     new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.GAVETO),        "GAVETO",           true, 28,   10,     false, true, true, false, true);
                LibAMFC.Set_LayoutView_Column_Options(this.LayoutView_Member_Form, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.QUINTINHA),  new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.QUINTINHA),     "QUINTINHA",        true, 29,   10,     false, true, true, false, true);
                LibAMFC.Set_LayoutView_Column_Options(this.LayoutView_Member_Form, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.LADOMAIOR),  new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.LADOMAIOR),     "LADOMAIOR",        true, 30,   10,     false, true, true, false, false);//
                LibAMFC.Set_LayoutView_Column_Options(this.LayoutView_Member_Form, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.MAIS1FOGO),  new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.MAIS1FOGO),     "MAIS1FOGO",        true, 31,   10,     false, true, true, false, true);
                LibAMFC.Set_LayoutView_Column_Options(this.LayoutView_Member_Form, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.HABCOLECT),  new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.HABCOLECT),     "HABCOLECT",        true, 32,   10,     false, true, true, false, true);
                LibAMFC.Set_LayoutView_Column_Options(this.LayoutView_Member_Form, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMFOGOS),   new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMFOGOS),      "NUMFOGOS",         true, 33,   10,     false, true, true, false, false);//
                LibAMFC.Set_LayoutView_Column_Options(this.LayoutView_Member_Form, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.ARECOMERC),  new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.ARECOMERC),     "ARECOMERC",        true, 34,   10,     false, true, true, false, true);

            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>28-04-2017(v0.0.2.48)</versions>
        private Int32 DBF_AMFC_Members_GetMaxNumber()
        {
            try
            {
                using (Library_AMFC_SQL obj_AMFC_SQL = new Library_AMFC_SQL())
                {
                    return obj_AMFC_SQL.Get_Member_Max_Number();
                }
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return -1;
            }
        }

        /// <versions>28-04-2017(v0.0.2.48)</versions>
        private Boolean DBF_AMFC_Member_Already_Exist(AMFCMember objMember)
        {
            try
            {
                using (Library_AMFC_SQL obj_AMFC_SQL = new Library_AMFC_SQL())
                {
                    return obj_AMFC_SQL.Member_Already_Exist(objMember, Operation);
                }
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return false;
            }
        }

        /// <versions>28-04-2017(v0.0.2.48)</versions>
        private Int32 DBF_AMFC_Add_Member(AMFCMember objMemberNew)
        {
            String sQueryString = String.Empty;
            try
            {
                #region     Check for Valid Member Info
                Int32 iCheckMemberDetailsStaturs = CheckMemberIsValid(objMemberNew);
                switch (iCheckMemberDetailsStaturs)
                {
                    case 1:
                        break;
                    case 0:
                        return 0;
                    case -1:
                        return -1;
                }
                #endregion  Check for Valid Member Info

                using (Library_AMFC_SQL obj_AMFC_SQL = new Library_AMFC_SQL())
                { 
                    this.Member = objMemberNew;
                    return obj_AMFC_SQL.Add_Member(objMemberNew, Operation);
                }
            }
            catch (Exception ex)
            {
                
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return -1;
            }
        }

        /// <versions>28-04-2017(v0.0.2.48)</versions>
        private Int32 DBF_AMFC_Edit_Member(AMFCMember objMemberEdited)
        {
            String sQueryString = String.Empty;
            try
            {
                #region     Check for Valid Member Info
                Int32 iCheckMemberDetailsStaturs = CheckMemberIsValid(objMemberEdited);
                switch (iCheckMemberDetailsStaturs)
                {
                    case 1:
                        break;
                    case 0:
                        return 0;
                    case -1:
                        return -1;
                }
                #endregion  Check for Valid Member Info

                using (Library_AMFC_SQL obj_AMFC_SQL = new Library_AMFC_SQL())
                {
                    this.Member = objMemberEdited;
                    return obj_AMFC_SQL.Edit_Member(objMemberEdited, Operation);
                }
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return -1;
            }
        }

        /// <versions>23-04-2017(v0.0.1.31)</versions>
        private Int32 CheckMemberIsValid(AMFCMember objMember)
        {
            try
            {
                #region     Nº Sócio
                if (objMember.NUMERO < 1 || objMember.NUMERO > objMember.MaxNumber)
                {
                    String sWarning = "O Nº de Sócio (" + objMember.NUMERO + ") não é válido! Por favor, insira um número válido < " + objMember.MaxNumber;
                    MessageBox.Show(sWarning, objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMERO) + " Invalído", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.LayoutView_Member_Form.FocusedRowHandle = 0;
                    this.LayoutView_Member_Form.FocusedColumn = this.LayoutView_Member_Form.Columns[new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMERO)];
                    this.LayoutView_Member_Form.ShowEditor(); // - use to activate the editor if necessary
                    this.LayoutView_Member_Form.Focus();
                    return 0;
                }
                #endregion  Nº Sócio

                #region     Nome
                if (String.IsNullOrEmpty(objMember.NOME))
                {
                    String sWarning = "O Nome do Sócio (" + objMember.NOME + ") não é válido! Por favor, modifique.";
                    MessageBox.Show(sWarning, "Nome Sócio" + " Invalído", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.LayoutView_Member_Form.FocusedRowHandle = 0;
                    this.LayoutView_Member_Form.FocusedColumn = this.LayoutView_Member_Form.Columns[new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NOME)];
                    this.LayoutView_Member_Form.ShowEditor(); // - use to activate the editor if necessary
                    this.LayoutView_Member_Form.Focus();
                    return 0;
                }
                #endregion  Nome

                #region     Morada
                if (String.IsNullOrEmpty(objMember.MORADA))
                {
                    String sWarning = "A morada do Sócio (" + objMember.MORADA + ") não é válida! Por favor, modifique.";
                    MessageBox.Show(sWarning, objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.MORADA) + " Invalída", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.LayoutView_Member_Form.FocusedRowHandle = 0;
                    this.LayoutView_Member_Form.FocusedColumn = this.LayoutView_Member_Form.Columns[new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.MORADA)];
                    this.LayoutView_Member_Form.ShowEditor(); // - use to activate the editor if necessary
                    this.LayoutView_Member_Form.Focus();
                    return 0;
                }
                #endregion  Morada

                #region     CPOSTAL
                if (String.IsNullOrEmpty(objMember.CPOSTAL))
                {
                    String sWarning = "O CPOSTAL do Sócio (" + objMember.CPOSTAL + ") não é válido! Por favor, modifique.";
                    MessageBox.Show(sWarning, "CPOSTAL Sócio" + " Invalído", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.LayoutView_Member_Form.FocusedRowHandle = 0;
                    this.LayoutView_Member_Form.FocusedColumn = this.LayoutView_Member_Form.Columns[new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.CPOSTAL)];
                    this.LayoutView_Member_Form.ShowEditor(); // - use to activate the editor if necessary
                    this.LayoutView_Member_Form.Focus();
                    return 0;
                }
                #endregion  CPOSTAL

                #region     MORLOTE
                if (String.IsNullOrEmpty(objMember.MORLOTE))
                {
                    String sWarning = "A morada do Lote (" + objMember.MORLOTE + ") não é válida! Por favor, modifique.";
                    MessageBox.Show(sWarning, objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.MORLOTE) + " Invalída", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.LayoutView_Member_Form.FocusedRowHandle = 0;
                    this.LayoutView_Member_Form.FocusedColumn = this.LayoutView_Member_Form.Columns[new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.MORLOTE)];
                    this.LayoutView_Member_Form.ShowEditor(); // - use to activate the editor if necessary
                    this.LayoutView_Member_Form.Focus();
                    return 0;
                }
                #endregion  MORLOTE

                #region     Lote
                if (String.IsNullOrEmpty(objMember.NUMLOTE))
                {
                    String sWarning = "O Lote do Sócio (" + objMember.NUMLOTE + ") não é válido! Por favor, modifique.";
                    MessageBox.Show(sWarning, "Lote " + " Invalído", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.LayoutView_Member_Form.FocusedRowHandle = 0;
                    this.LayoutView_Member_Form.FocusedColumn = this.LayoutView_Member_Form.Columns[new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMLOTE)];
                    this.LayoutView_Member_Form.ShowEditor(); // - use to activate the editor if necessary
                    this.LayoutView_Member_Form.Focus();
                    return 0;
                }
                #endregion  Lote

                #region     Lote
                if (objMember.AREALOTE < 1)
                {
                    String sWarning = "A Área do Lote (" + objMember.AREALOTE + ") não é válida! Por favor, insira um valor inteiro válido (> 0).";
                    MessageBox.Show(sWarning, "Área do Lote" + " Invalída", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.LayoutView_Member_Form.FocusedRowHandle = 0;
                    this.LayoutView_Member_Form.FocusedColumn = this.LayoutView_Member_Form.Columns[new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.AREALOTE)];
                    this.LayoutView_Member_Form.ShowEditor(); // - use to activate the editor if necessary
                    this.LayoutView_Member_Form.Focus();
                    return 0;
                }
                #endregion  Lote
                
                return 1;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return -1;
            }
        }

        #endregion  Methods
    }
}