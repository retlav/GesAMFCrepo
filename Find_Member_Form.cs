using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using GesAMFC.AMFC_Methods;
using System;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace GesAMFC
{
    /// <summary>Find Member Form</summary>
    /// <creation>20-04-2017(v0.0.1.23)</creation>
    /// <versions>30-11-2017(GesAMFC-v0.0.4.40)</versions>
    public partial class Find_Member_Form : DevExpress.XtraEditors.XtraForm
    {
        public Library_AMFC_Methods LibAMFC;

        public AMFCMember   MemberSelected;
        public AMFCMembers  MembersFindResult;

        public Int64 _MemberNumber = -1;

        private Color _FocusedMemberRowBgColor;

        public Int64 MemberNumber
        {
            get { return _MemberNumber; }
            set
            {
                if (value < new AMFCMember().MinNumber || value > new AMFCMember().MaxNumber)
                    return;
                _MemberNumber = value;
                this.TextEdit_Member_Number.Text = _MemberNumber.ToString();
            }
        }

        #region Constructor

        /// <versions>30-11-2017(GesAMFC-v0.0.4.40)</versions>
        public Find_Member_Form()
        {
            this.LibAMFC = new Library_AMFC_Methods();
            try
            {
                InitializeComponent();

                if (Program.AllDbMembers == null || Program.AllDbMembers.Members == null || Program.AllDbMembers.Members.Count == 0)
                {
                    using (Library_AMFC_SQL obj_AMFC_SQL = new Library_AMFC_SQL())
                    {
                        AMFCMembers objMembers = obj_AMFC_SQL.Get_List_Members();
                        if (objMembers != null && objMembers.Members != null)
                            Program.AllDbMembers = objMembers;
                    }
                }

                this.MemberSelected     = new AMFCMember();
                this.MembersFindResult  = new AMFCMembers();
                Program.Member_Found    = false;

                _MemberNumber = -1;


                _FocusedMemberRowBgColor = Color.FromArgb(242, 180, 82);

                SplitContainer_01.Panel2Collapsed = true;

                SplitContainer_02.Panel1Collapsed = true;

                SplitContainer_03.Panel2Collapsed = true;

                SplitContainer_04.SplitterDistance = 270;
                SplitContainer_04.IsSplitterFixed = true;

                SplitContainer_05.Panel2Collapsed = true;

                SplitContainer_06.Panel1Collapsed = true;

                SplitContainer_07.Panel2Collapsed = true;

                SplitContainer_08.Panel2Collapsed = true;

                SplitContainer_09.Panel1Collapsed = true;

                SplitContainer_10.SplitterDistance = 385;
                SplitContainer_10.IsSplitterFixed = true;
                
                this.MinimumSize = new Size(500, 200);
                this.MaximumSize = new Size(1450, 800);
                this.Size = this.MinimumSize;

                Panel_Find_Member_Form.MinimumSize = new Size(460, 160);
                Panel_Find_Member_Form.MaximumSize = new Size(460, 280);

                LayoutControl_Member_Find_Form.MinimumSize = new Size(460, 160);
                LayoutControl_Member_Find_Form.MaximumSize = new Size(460, 280);

                this.Update();

                ShowPanelFindMember(false);
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        #endregion Constructor

        #region Events

        #region     Form Events

        /// <versions>30-11-2017(GesAMFC-v0.0.4.40)</versions>
        private void Find_Member_Load(object sender, EventArgs e)
        {
            this.Focus();
            this.TextEdit_Member_Number.Focus();
        }

        /// <versions>30-11-2017(GesAMFC-v0.0.4.40)</versions>
        private void FindMember_Activated(object sender, EventArgs e)
        {
            this.Focus();
            this.TextEdit_Member_Number.Focus();
        }

        /// <versions>30-11-2017(GesAMFC-v0.0.4.40)</versions>
        private void FindMember_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        /// <versions>30-11-2017(GesAMFC-v0.0.4.40)</versions>
        private void FindMember_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        #endregion  Form Events

        #region     TextBoxes Events

        /// <versions>20-04-2017(v0.0.1.23)</versions>
        private void TextEdit_Member_Number_KeyPress(object sender, KeyPressEventArgs e)
        {
            try { TextEdit_KeyPress(this.TextEdit_Member_Number, ((Int32)e.KeyChar == 13), ((Int32)e.KeyChar == 27)); }
            catch (Exception ex) { Program.WriteLog(ex.TargetSite.Name, ex.Message, true, true, true, true); }
        }

        /// <versions>20-04-2017(v0.0.1.23)</versions>
        private void TextEdit_Member_Name_KeyPress(object sender, KeyPressEventArgs e)
        {
            try { TextEdit_KeyPress(this.TextEdit_Member_Number, ((Int32)e.KeyChar == 13), ((Int32)e.KeyChar == 27)); }
            catch (Exception ex) { Program.WriteLog(ex.TargetSite.Name, ex.Message, true, true, true, true); }
        }

        /// <versions>05-05-2017(v0.0.2.54)</versions>
        private void TextEdit_Member_Address_KeyPress(object sender, KeyPressEventArgs e)
        {
            try { TextEdit_KeyPress(this.TextEdit_Member_Address, ((Int32)e.KeyChar == 13), ((Int32)e.KeyChar == 27)); }
            catch (Exception ex) { Program.WriteLog(ex.TargetSite.Name, ex.Message, true, true, true, true); }
        }

        /// <versions>05-05-2017(v0.0.2.54)</versions>
        private void TextEdit_Member_ZipCode_KeyPress(object sender, KeyPressEventArgs e)
        {
            try { TextEdit_KeyPress(this.TextEdit_Member_ZipCode, ((Int32)e.KeyChar == 13), ((Int32)e.KeyChar == 27)); }
            catch (Exception ex) { Program.WriteLog(ex.TargetSite.Name, ex.Message, true, true, true, true); }
        }

        /// <versions>05-05-2017(v0.0.2.54)</versions>
        private void TextEdit_Member_Lot_KeyPress(object sender, KeyPressEventArgs e)
        {
            try { TextEdit_KeyPress(this.TextEdit_Member_Lot, ((Int32)e.KeyChar == 13), ((Int32)e.KeyChar == 27)); }
            catch (Exception ex) { Program.WriteLog(ex.TargetSite.Name, ex.Message, true, true, true, true); }
        }

        #endregion  TextBoxes Events

        #region     Buttons Events

        /// <versions>20-04-2017(v0.0.1.23)</versions>
        private void SimpleButton_Find_Member_Cancel_Click(object sender, EventArgs e)
        {
            Program.Member_Found = false;
            this.Close();
        }

        /// <versions>20-04-2017(v0.0.1.23)</versions>
        private void SimpleButton_Find_Member_Ok_Click(object sender, EventArgs e)
        {
            Find_DB_Member();
        }

        /// <versions>20-04-2017(v0.0.1.24)</versions>
        private void SimpleButton_Member_Result_Cancel_Click(object sender, EventArgs e)
        {
            ShowPanelFindMember(Button_Show_Find_Member_Other_Fields.Image.Tag.ToString() == "hide_more");
        }

        /// <versions>20-04-2017(v0.0.1.24)</versions>
        private void SimpleButton_Member_Result_Ok_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <versions>26-04-2017(v0.0.2.40)</versions>
        private void Button_Show_Find_Member_Other_Fields_Click(object sender, EventArgs e)
        {
            Set_Button_Show_Other_Fields();
        }

        #endregion  Buttons Events

        #region     Find Results Grid Events

        /// <versions>03-05-2017(v0.0.2.52)</versions>
        private void GridView_Find_Member_Click(object sender, EventArgs e)
        {

        }
        
        /// <versions>03-05-2017(v0.0.2.52)</versions>
        private void GridView_Find_Member_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                Int32 iRowHandle = e.FocusedRowHandle;
                Int32 iMinRowHandle = 0;
                GridView_MembersFocusedRow(iRowHandle, iMinRowHandle);
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>03-05-2017(v0.0.2.52)</versions>
        private void GridView_Find_Member_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                StackFrame objStackFrame = new StackFrame();
                try
                {
                    GridView view = (GridView)sender;
                    GridHitInfo hi = view.CalcHitInfo(e.Location);

                    Int32 iRowHandle = hi.RowHandle;

                    #region     Check if is a valid Data Row Handle
                    if (iRowHandle == GridControl.InvalidRowHandle || !this.GridView_Find_Member.IsDataRow(iRowHandle))
                    {
                        this.GridView_Find_Member.ClearSelection();
                        return;
                    }
                    #endregion  Check if is a valid Data Row Handle

                    #region     Prevent the selectionchanged event
                    if (!hi.InRowCell || hi.Column == null)
                    {
                        (e as DevExpress.Utils.DXMouseEventArgs).Handled = true;
                        return;
                    }
                    #endregion  Prevent the selectionchanged event

                    if (iRowHandle > -1)
                        GridView_MembersFocusedRow(iRowHandle);
                }
                catch (Exception ex) { Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false); }
                finally { objStackFrame = null; }
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>03-05-2017(v0.0.2.52)</versions>
        private void GridView_Find_Member_RowStyle(object sender, DevExpress.XtraGrid.Views.Grid.RowStyleEventArgs e)
        {
            try
            {
                if (e.RowHandle >= 0 && this.GridView_Find_Member.FocusedRowHandle >= 0 && e.RowHandle == this.GridView_Find_Member.FocusedRowHandle)
                {
                    //this.GridView_Find_Member.Appearance.FocusedCell.BackColor = objColor;
                    //this.GridView_Find_Member.Appearance.FocusedRow.BackColor = objColor;
                    e.Appearance.BackColor = _FocusedMemberRowBgColor;
                }
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        #endregion  Find Results Grid Events

        #endregion  Events

        #region     Private Methods

        /// <versions>20-04-2017(v0.0.1.23)</versions>
        private void TextEdit_KeyPress(TextEdit objTextEdit, Boolean bEnterKeyPressed, Boolean bEscKeyPressed)
        {
            try
            {
                //SetToolTipHint(false, objTextEdit);
                if (bEnterKeyPressed) 
                    Find_DB_Member();
                else if (bEscKeyPressed)
                    this.Close();
                //else SetToolTipHint(true, objTextEdit);
            }
            catch (Exception ex) { Program.WriteLog(ex.TargetSite.Name, ex.Message, true, true, true, true); }
        }

        /// <versions>20-04-2017(v0.0.1.23)</versions>
        //private void SetToolTipHint(Boolean bShowHint, TextEdit objTextBox)
        //{
        //    try
        //    {
        //        String sCapsOnWarning = "CAPS LOCK ligado!";
        //        if (bShowHint) { if (Console.CapsLock) ToolTipController_FindMember.ShowHint(sCapsOnWarning, objTextBox, ToolTipLocation.RightCenter); }
        //        else
        //            ToolTipController_FindMember.HideHint();
        //    }
        //    catch (Exception ex) { Program.WriteLog(ex.TargetSite.Name, ex.Message, true, true, true, true); }
        //}

        /// <versions>01-05-2017(v0.0.2.49)</versions>
        private Boolean Find_DB_Member()
        {
            try
            {
                if (!Find_Member())
                {
                    XtraMessageBox.Show("Não foi possível encontrar o sócio! Por favor, altere os parâmetros de procura.", "[Sócio não existe]", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Focus();
                    ShowPanelFindMember(Button_Show_Find_Member_Other_Fields.Image.Tag.ToString() == "hide_more");
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                Program.WriteLog(ex.TargetSite.Name, ex.Message, true, true, true, true);
                return false;
            }
        }

        /// <versions>07-05-2017(v0.0.3.3)</versions>
        private Boolean Find_Member()
        {
            try
            {
                #region     Check Fields
                if (String.IsNullOrEmpty(this.TextEdit_Member_Number.Text.Trim()) && ((String.IsNullOrEmpty(TextEdit_Member_Name.Text.Trim()) && String.IsNullOrEmpty(TextEdit_Member_Address.Text.Trim()) && String.IsNullOrEmpty(TextEdit_Member_ZipCode.Text.Trim()) && String.IsNullOrEmpty(TextEdit_Member_Lot.Text.Trim()))))
                {
                    XtraMessageBox.Show("Por favor, preencha os campos de procura!", "Campos vazios", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Focus();
                    this.TextEdit_Member_Number.Focus();
                    return false;
                }
                #endregion  Check Fields

                #region     Find Member by Number
                if (!String.IsNullOrEmpty(this.TextEdit_Member_Number.Text.Trim()))
                {
                    try
                    {
                        Int64 lMemberNumber = -1;
                        if (Int64.TryParse(this.TextEdit_Member_Number.Text.Trim().TrimStart('0'), out lMemberNumber))
                        {
                            AMFCMember objMemberFound = Get_DBF_AMFC_Member_By_Number(lMemberNumber);
                            if (objMemberFound != null && objMemberFound.NUMERO >= objMemberFound.MinNumber && objMemberFound.NUMERO <= objMemberFound.MaxNumber && !String.IsNullOrEmpty(objMemberFound.NOME) && !String.IsNullOrEmpty(objMemberFound.MORADA) && !String.IsNullOrEmpty(objMemberFound.NUMLOTE))
                            {
                                this.MemberSelected = objMemberFound;
                                Program.Member_Found = true;
                                this.Close();
                                return true;
                            }
                        }
                        else
                        {
                            XtraMessageBox.Show("Insira um número de sócio inteior válido!", "ERRO [Nº Sócio Inválido]", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            this.Focus();
                            this.TextEdit_Member_Number.Focus();
                            return false;
                        }
                    }
                    catch (Exception ex1)
                    {
                        XtraMessageBox.Show("Não foi possível encontrar o sócio! Por favor, introduza um Número de Sócio inteiro válido.", "ERRO [Procurar Sócio]", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Focus();
                        this.TextEdit_Member_Number.Focus();
                        Program.WriteLog(ex1.TargetSite.Name, ex1.Message, true, true, true, true);
                        return false;
                    }
                }
                #endregion  Find Member by Number

                #region     Find Member by Other Fields
                else
                {
                    if (!String.IsNullOrEmpty(TextEdit_Member_Name.Text.Trim()) || !String.IsNullOrEmpty(TextEdit_Member_Address.Text.Trim()) || !String.IsNullOrEmpty(TextEdit_Member_ZipCode.Text.Trim()) || !String.IsNullOrEmpty(TextEdit_Member_Lot.Text.Trim()))
                    {
                        AMFCMember objMember = new AMFCMember();
                        if (!String.IsNullOrEmpty(TextEdit_Member_Name.Text.Trim()))
                            objMember.NOME = TextEdit_Member_Name.Text.Trim();
                        if (!String.IsNullOrEmpty(TextEdit_Member_Address.Text.Trim()))
                            objMember.MORADA = TextEdit_Member_Address.Text.Trim();
                        if (!String.IsNullOrEmpty(TextEdit_Member_ZipCode.Text.Trim()))
                            objMember.CPOSTAL = TextEdit_Member_ZipCode.Text.Trim();
                        if (!String.IsNullOrEmpty(TextEdit_Member_Lot.Text.Trim()))
                            objMember.NUMLOTE = TextEdit_Member_Lot.Text.Trim();
                        AMFCMembers objListMembersFound = Get_DBF_AMFC_Members_By_Fields(objMember);
                        if (objListMembersFound == null || objListMembersFound.Members.Count == 0)
                        {
                            this.TextEdit_Member_Number.Text    = String.Empty;
                            this.TextEdit_Member_Name.Text      = String.Empty;
                            this.TextEdit_Member_Address.Text   = String.Empty;
                            this.TextEdit_Member_ZipCode.Text   = String.Empty;
                            this.TextEdit_Member_Lot.Text       = String.Empty;
                            XtraMessageBox.Show("Não foi possível encontrar o sócio! Por favor, altere os campos de procura e tente novamente.", "Sócio Não Encontrado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            this.Focus();
                            this.TextEdit_Member_Number.Focus();
                            return false;
                        }
                        else if (objListMembersFound.Members.Count == 1 && objListMembersFound.Members[0].NUMERO >= objListMembersFound.Members[0].MinNumber && objListMembersFound.Members[0].NUMERO <= objListMembersFound.Members[0].MaxNumber && !String.IsNullOrEmpty(objListMembersFound.Members[0].NOME) && !String.IsNullOrEmpty(objListMembersFound.Members[0].NUMLOTE))
                        {
                            this.MemberSelected = objListMembersFound.Members[0];
                            Program.Member_Found = true;
                            this.Close();
                            return true;
                        }
                        else
                        {
                            return Load_Member_Grid_Results(objListMembersFound);
                        }
                    }
                }
                #endregion     Find Member by Other Fields

                return false;
            }
            catch (Exception ex)
            {
                Program.WriteLog(ex.TargetSite.Name, ex.Message, true, true, true, true);
                return false;
            }
        }

        /// <versions>03-05-2017(v0.0.2.51)</versions>
        private Boolean Load_Member_Grid_Results(AMFCMembers objListMembersFound)
        {
            try
            {
                if (objListMembersFound == null || objListMembersFound.Members.Count == 0)
                    return false;
                this.MembersFindResult = objListMembersFound;

                ShowPanelGridResults();
                this.Text += " " + "[" + "Total Sócios Encontrados: " + objListMembersFound.Members.Count + "]";
                
                if (!Load_Grid_Members(objListMembersFound))
                    return false;

                return true;
            }
            catch (Exception ex)
            {
                Program.WriteLog(ex.TargetSite.Name, ex.Message, true, true, true, true);
                return false;
            }
        }

        /// <versions>05-05-2017(v0.0.2.54)</versions>
        public void ShowPanelFindMember(Boolean bShowAll)
        {
            try
            {
                this.Text = "Procurar Sócio";
                Panel_Find_Member_Form.Visible = true;
                SplitContainer_04.Panel1Collapsed = false;
                Panel_Find_Member_Grid.Visible = false;
                SplitContainer_04.Panel2Collapsed = true;

                Set_Button_Fields(bShowAll);
                
                TextEdit_Member_Number.Focus();
#if DEBUG
                //TextEdit_Member_Number.Text = "105"; //Debug: Manuel Leitão
                //Find_DB_Member();
#endif
            }
            catch (Exception ex)
            {
                Program.WriteLog(ex.TargetSite.Name, ex.Message, true, true, true, true);
            }
        }

        /// <versions>05-05-2017(v0.0.2.54)</versions>
        public void ShowPanelGridResults()
        {
            try
            {
                this.Text = "Selecione o sócio";
                Panel_Find_Member_Form.Visible = false;
                SplitContainer_04.Panel1Collapsed = true;
                Panel_Find_Member_Grid.Visible = true;
                SplitContainer_04.Panel2Collapsed = false;
                this.GridControl_Find_Member.Dock = DockStyle.Fill;
                this.Size = this.MaximumSize;
                this.Update();
                //this.StartPosition = FormStartPosition.CenterScreen;
                this.CenterToParent();
                this.Focus();
                this.BringToFront();
            }
            catch (Exception ex)
            {
                Program.WriteLog(ex.TargetSite.Name, ex.Message, true, true, true, true);
            }
        }

        /// <versions>05-05-2017(v0.0.2.54)</versions>
        private void Set_Button_Show_Other_Fields()
        {
            try
            {
                if (Button_Show_Find_Member_Other_Fields.Image.Tag.ToString() == "hide_more")
                    Set_Button_Fields(false);
                else
                    Set_Button_Fields(true);
            }
            catch (Exception ex)
            {
                Program.WriteLog(ex.TargetSite.Name, ex.Message, true, true, true, true);
            }
        }

        /// <versions>05-05-2017(v0.0.2.54)</versions>
        private void Set_Button_Fields(Boolean bShowAll)
        {
            try
            {
                if (bShowAll)
                {
                    Button_Show_Find_Member_Other_Fields.Image = GesAMFC.Properties.Resources.hide_more;
                    Button_Show_Find_Member_Other_Fields.Image.Tag = "hide_more";
                    Button_Show_Find_Member_Other_Fields.ToolTip = "esconder outros campos de procura ...";
                    LayoutControlGroup_Find_Member_Other_Fields.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                    LayoutControlGroup_Find_Member_Other_Fields.Expanded = true;
                    LayoutControlGroup_Member_Find_Form.Update();
                    LayoutControl_Member_Find_Form.Size = this.LayoutControl_Member_Find_Form.MaximumSize;
                    Panel_Find_Member_Form.Size = Panel_Find_Member_Form.MaximumSize;
                    this.Size = new Size(Panel_Find_Member_Form.MaximumSize.Width + 20, Panel_Find_Member_Form.MaximumSize.Height + 20);
                    this.Update();
                    this.StartPosition = FormStartPosition.CenterScreen;
                    this.Focus();
                    this.BringToFront();
                }
                else
                {
                    Button_Show_Find_Member_Other_Fields.Image = GesAMFC.Properties.Resources.show_more;
                    Button_Show_Find_Member_Other_Fields.Image.Tag = "show_more";
                    Button_Show_Find_Member_Other_Fields.ToolTip = "mostrar mais campos de procura ...";
                    LayoutControlGroup_Find_Member_Other_Fields.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    LayoutControlGroup_Find_Member_Other_Fields.Expanded = false;
                    LayoutControlGroup_Member_Find_Form.Update();
                    LayoutControl_Member_Find_Form.Size = this.LayoutControl_Member_Find_Form.MinimumSize;
                    Panel_Find_Member_Form.Size = Panel_Find_Member_Form.MinimumSize;
                    this.Size = this.MinimumSize;
                    this.Update();
                    this.StartPosition = FormStartPosition.CenterScreen;
                    this.Focus();
                    this.BringToFront();
                }
            }
            catch (Exception ex)
            {
                Program.WriteLog(ex.TargetSite.Name, ex.Message, true, true, true, true);
            }
        }

        /// <versions>28-04-2017(v0.0.2.48)</versions>
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

        /// <versions>04-05-2017(v0.0.2.53)</versions>
        private AMFCMembers Get_DBF_AMFC_Members_By_Fields(AMFCMember objMember)
        {
            try
            {
                using (Library_AMFC_SQL obj_AMFC_SQL = new Library_AMFC_SQL())
                {
                    return obj_AMFC_SQL.Find_Members(Program.AllDbMembers, objMember);
                }
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return null;
            }
        }

        #region     Find Results Grid Methods

        /// <versions>04-05-2017(v0.0.2.53)</versions>
        private Boolean Load_Grid_Members(AMFCMembers objListMembersFound)
        {
            try
            {
                Boolean bSetCols = true, bClearSorting = true, bClearFilters = true, bClearGrouping = true;

                Grid_Results_Clear_Selection();
                
                if (!Load_Results_Grid(objListMembersFound, bSetCols, bClearSorting, bClearFilters, bClearGrouping, true))
                    return false;

                #region     Config Grids Options
                this.GridView_Find_Member.OptionsSelection.EnableAppearanceFocusedRow = true;
                this.GridView_Find_Member.OptionsSelection.EnableAppearanceFocusedCell = false;
                this.GridView_Find_Member.OptionsSelection.EnableAppearanceHideSelection = false;
                this.GridView_Find_Member.OptionsSelection.UseIndicatorForSelection = true;
                this.GridView_Find_Member.ClearSelection();
                this.GridView_Find_Member.OptionsFilter.AllowFilterEditor = false;
                this.GridView_Find_Member.OptionsFilter.AllowColumnMRUFilterList = false;
                this.GridView_Find_Member.OptionsFilter.AllowMRUFilterList = false;
                this.GridView_Find_Member.OptionsFind.AllowFindPanel = false;
                this.GridView_Find_Member.OptionsFind.AlwaysVisible = false;
                this.GridView_Find_Member.OptionsView.ShowAutoFilterRow = false;
                this.GridView_Find_Member.OptionsView.ShowGroupPanel = false;
                this.GridView_Find_Member.OptionsView.ShowGroupedColumns = false;
                this.GridView_Find_Member.OptionsView.ShowIndicator = true;
                this.GridView_Find_Member.OptionsView.ShowPreview = false;
                this.GridView_Find_Member.OptionsView.AllowCellMerge = false;
                this.GridView_Find_Member.OptionsView.ShowFooter = true;
                //this.GridControl_Find_Member.show pager ??
                this.GridView_Find_Member.OptionsSelection.UseIndicatorForSelection = true;
                this.GridView_Find_Member.ColumnPanelRowHeight = 30;
                #endregion  Config Grids Options

                return true;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return false;
            }
        }

        /// <versions>03-05-2017(v0.0.2.52)</versions>
        private Boolean Grid_Results_Clear_Selection()
        {
            StackFrame objStackFrame = new StackFrame();
            try
            {
                this.GridView_Find_Member.ClearSelection();
                //for (int iRow = 0; iRow < this.GridView_Find_Member.DataRowCount; iRow++)
                //    this.GridView_Find_Member.SetRowCellValue(iRow, this.GridView_Find_Member.Columns[_SelectedColName], false);
                return true;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return false;
            }
            finally { objStackFrame = null; }
        }

        /// <versions>03-05-2017(v0.0.2.52)</versions>
        private Boolean Load_Results_Grid(AMFCMembers objListMembersFound, Boolean bSetCols, Boolean bClearSorting, Boolean bClearFilters, Boolean bClearGrouping, Boolean bWriteLog)
        {
            try
            {
                LibAMFC.CleanGrid(this.GridControl_Find_Member, this.GridView_Find_Member, bSetCols, bClearSorting, bClearFilters, bClearGrouping);
                this.GridControl_Find_Member.Visible = false;
                //this.LoadingBox_Brands.Dock = DockStyle.Fill;
                //this.LoadingBox_Brands.Show();
                this.Update();

                if (!Set_Results_DataSource(this.GridControl_Find_Member, objListMembersFound))
                    return false;

                if (bSetCols)
                    Set_Results_Columns();

                //this.LoadingBox_Brands.Hide();
                this.GridControl_Find_Member.Visible = true;
                this.Update();

                return true;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return false;
            }
        }

        /// <versions>03-05-2017(v0.0.2.52)</versions>
        private Boolean Set_Results_DataSource(GridControl objGridControl, AMFCMembers objListResults)
        {
            StackFrame objStackFrame = new StackFrame();
            String sErrorMsg = String.Empty;
            try
            {
                #region     Results Datasource
                if (objListResults == null || objListResults.Members == null || objListResults.Members.Count == 0)
                {
                    sErrorMsg = "Não foi possível obter Sócios para esta procura!";
                    Program.HandleError(objStackFrame.GetMethod().Name, sErrorMsg, Program.ErroType.ERROR, true, false);
                    return false;
                }
                #endregion  Results Datasource

                #region     TreeDataSet
                DataTable objDataTableResults = new DataTable("Table_Find_Members_Resuls");
                #region     Data Columns Creation
                DataColumn objDataColumn_Member_NUMERO = new DataColumn(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMERO), typeof(Int64)); //Int64
                DataColumn objDataColumn_Member_NOME = new DataColumn(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NOME), typeof(String));
                DataColumn objDataColumn_Member_MORADA = new DataColumn(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.MORADA), typeof(String));
                DataColumn objDataColumn_Member_CPOSTAL = new DataColumn(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.CPOSTAL), typeof(String));
                DataColumn objDataColumn_Member_MORLOTE = new DataColumn(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.MORLOTE), typeof(String));
                DataColumn objDataColumn_Member_NUMLOTE = new DataColumn(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMLOTE), typeof(String));
                #endregion  Data Columns Creation
                #region     Data Table Add Columns
                objDataTableResults.Columns.AddRange(
                                            new DataColumn[] {
                                                objDataColumn_Member_NUMERO,
                                                objDataColumn_Member_NOME,
                                                objDataColumn_Member_MORADA,
                                                objDataColumn_Member_CPOSTAL,
                                                objDataColumn_Member_MORLOTE,
                                                objDataColumn_Member_NUMLOTE
                                            }
                                        );
                #endregion  Data Table Add Columns
                #region     Data Columns Dispose
                objDataColumn_Member_NUMERO.Dispose();
                objDataColumn_Member_NOME.Dispose();
                objDataColumn_Member_MORADA.Dispose();
                objDataColumn_Member_CPOSTAL.Dispose();
                objDataColumn_Member_MORLOTE.Dispose();
                objDataColumn_Member_NUMLOTE.Dispose();
                #endregion  Data Columns Dispose
                #endregion  TreeDataSet

                #region     Binding Data
                foreach (AMFCMember objMember in objListResults.Members)
                {
                    if (objMember == null || objMember.NUMERO < 1 || String.IsNullOrEmpty(objMember.NOME))
                        continue;
                    #region     Set Member Row Data
                    DataRow objDataRow = objDataTableResults.NewRow();
                    objDataRow[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMERO)] = objMember.NUMERO.ToString();
                    objDataRow[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NOME)] = objMember.NOME;
                    objDataRow[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.MORADA)] = objMember.MORADA;
                    objDataRow[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.CPOSTAL)] = objMember.CPOSTAL;
                    objDataRow[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.MORLOTE)] = objMember.MORLOTE;
                    objDataRow[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMLOTE)] = objMember.NUMLOTE;
                    #endregion  Set Member Row Data
                    objDataTableResults.Rows.Add(objDataRow);
                }
                objGridControl.DataSource = objDataTableResults;
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

        /// <versions>03-05-2017(v0.0.2.52)</versions>
        private void Set_Results_Columns()
        {
            try
            {
                Set_Results_Columns_Editability();
                Set_Results_Columns_Options_Filter();
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>03-05-2017(v0.0.2.52)</versions>
        private void Set_Results_Columns_Editability()
        {
            try
            {
                LibAMFC.SetGridColumnOptions(this.GridControl_Find_Member,  this.GridView_Find_Member,  new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMERO),     new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMERO).ToUpper(),    "Número de sócio",  true, 1, 100,  true, false, true, false);
                LibAMFC.SetGridColumnOptions(this.GridControl_Find_Member,  this.GridView_Find_Member,  new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NOME),       new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.NOME).ToUpper(),      "Nome do sócio",    true, 2, 300, true, false, true, false);
                LibAMFC.SetGridColumnOptions(this.GridControl_Find_Member,  this.GridView_Find_Member,  new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.MORADA),     new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.MORADA).ToUpper(),    "Morada do sócio",  true, 4, 300, true, false, true, false);
                LibAMFC.SetGridColumnOptions(this.GridControl_Find_Member,  this.GridView_Find_Member,  new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.CPOSTAL),    new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.CPOSTAL).ToUpper(),   "CPOSTAL",          true, 5, 250, true, false, true, false);
                LibAMFC.SetGridColumnOptions(this.GridControl_Find_Member,  this.GridView_Find_Member,  new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.MORLOTE),    new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.MORLOTE).ToUpper(),   "Morada do lote",   true, 7, 300, true, false, true, false);
                LibAMFC.SetGridColumnOptions(this.GridControl_Find_Member,  this.GridView_Find_Member,  new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMLOTE),    new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMLOTE).ToUpper(),   "Lote de sócio",    true, 8, 100, true, false, true, false);
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>03-05-2017(v0.0.2.52)</versions>
        private void Set_Results_Columns_Options_Filter()
        {
            try
            {
                LibAMFC.SetGridColumOptionsFilter(this.GridView_Find_Member, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMERO),    false, false, AutoFilterCondition.Contains, 9.0f);
                LibAMFC.SetGridColumOptionsFilter(this.GridView_Find_Member, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NOME),      false, false, AutoFilterCondition.Contains, 9.0f);
                LibAMFC.SetGridColumOptionsFilter(this.GridView_Find_Member, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.MORADA),    false, false, AutoFilterCondition.Contains, 9.0f);
                LibAMFC.SetGridColumOptionsFilter(this.GridView_Find_Member, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.CPOSTAL),   false, false, AutoFilterCondition.Contains, 9.0f);
                LibAMFC.SetGridColumOptionsFilter(this.GridView_Find_Member, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.MORLOTE),   false, false, AutoFilterCondition.Contains, 9.0f);
                LibAMFC.SetGridColumOptionsFilter(this.GridView_Find_Member, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMLOTE),   false, false, AutoFilterCondition.Contains, 9.0f);
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>03-05-2017(v0.0.2.52)</versions>
        private Boolean GridView_MembersFocusedRow(Int32 iRowHandle)
        {
            return GridView_Members_Focused_Row(iRowHandle, 0);
        }

        /// <versions>03-05-2017(v0.0.2.52)</versions>
        private Boolean GridView_MembersFocusedRow(Int32 iRowHandle, Int32 iMinRowHandle)
        {
            return GridView_Members_Focused_Row(iRowHandle, iMinRowHandle);
        }

        /// <versions>03-05-2017(v0.0.2.52)</versions>
        private Boolean GridView_Members_Focused_Row(Int32 iRowHandle, Int32 iMinRowHandle)
        {
            try
            {
                StackFrame objStackFrame = new StackFrame();
                #region     Check if is a valid Data Row Handle
                if (iRowHandle < iMinRowHandle || iRowHandle == GridControl.InvalidRowHandle || !this.GridView_Find_Member.IsDataRow(iRowHandle))
                {
                    //this.GridView_Find_Member.ClearSelection();
                    return false;
                }
                #endregion  Check if is a valid Data Row Handle

                #region     Get Focused Member
                AMFCMember objMember = Get_Selected_Member(iRowHandle);
                if (objMember == null || objMember.NUMERO < 1)
                {
                    XtraMessageBox.Show("Erro", objStackFrame.GetMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                Program.Member_Found = true;
                this.MemberSelected = objMember;
                this.MembersFindResult.SelectedMember = objMember;
                #endregion  Get Focused Member

                return true;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return false;
            }
        }

        /// <versions>03-05-2017(v0.0.2.52)</versions>
        private AMFCMember Get_Selected_Member(Int32 iRowHandle)
        {
            try
            {
                AMFCMember objMember = null;
                Int32 iFocusedRowHandle = -1;
                Int64 lFocusedMemberNumber = -1;

                if (this.MembersFindResult.Members.Count == 0)
                {
                    XtraMessageBox.Show("Lista de Sócios vazia!", "Erro [Get_Selected_Member]", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }

                if (iRowHandle > -1)
                    iFocusedRowHandle = iRowHandle;
                else
                    iFocusedRowHandle = this.GridView_Find_Member.FocusedRowHandle;
                if (iFocusedRowHandle < 0 || iFocusedRowHandle == GridControl.InvalidRowHandle || !this.GridView_Find_Member.IsDataRow(iFocusedRowHandle))
                {
                    XtraMessageBox.Show("Não foi possível obter os detalhes do sócio selecionado!", "Erro [Sócio Selecionado]", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
                lFocusedMemberNumber = Convert.ToInt64(this.GridView_Find_Member.GetRowCellValue(iFocusedRowHandle, this.GridView_Find_Member.Columns[new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMERO)]));

                objMember = this.MembersFindResult.GetMemberByNumber(lFocusedMemberNumber);
                if (objMember == null || objMember.NUMERO < 1 || String.IsNullOrEmpty(objMember.NOME))
                {
                    XtraMessageBox.Show("Não foi possível obter os detalhes do sócio selecionado!", "Erro [Sócio Selecionado]", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }


                return objMember;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return null;
            }
        }

        #endregion  Find Results Grid Methods

        #endregion  Private Methods

        private void Panel_Members_List_Header_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}