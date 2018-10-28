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
    /// <creation>08-10-2017(v0.0.4.8)</creation>
    /// <versions>08-10-2017(v0.0.4.8)</versions>
    public partial class Find_Member_Add_Joia : DevExpress.XtraEditors.XtraForm
    {
        public Library_AMFC_Methods LibAMFC;

        public AMFCMembers  AllDbMembers;
        
        public AMFCMembers MembersFindResult;
        public AMFCMember   MemberSelected;
        public Boolean Member_Found;
        public Boolean AllJoiasPaid;

        private Color _FocusedMemberRowBgColor;

        #region Constructor

        /// <versions>08-10-2017(v0.0.4.8)</versions>
        public Find_Member_Add_Joia(AMFCMembers objAllDbMembers)
        {
            this.LibAMFC = new Library_AMFC_Methods();
            try
            {
                InitializeComponent();

                this.MembersFindResult = new AMFCMembers();
                this.MemberSelected = new AMFCMember();
                this.AllJoiasPaid = false;
                this.Member_Found = false;

                Boolean bLoadOk = false;

                this.AllDbMembers = new AMFCMembers();
                if (objAllDbMembers != null && objAllDbMembers.Members != null)
                    this.AllDbMembers = objAllDbMembers;
                else
                {
                    using (Library_AMFC_SQL obj_AMFC_SQL = new Library_AMFC_SQL())
                    {
                        AMFCMembers objMembers = obj_AMFC_SQL.Get_List_Members_Joia_Not_Paid();
                        if (objMembers != null && objMembers.Members != null)
                        {
                            this.AllDbMembers = objMembers;
                            bLoadOk = Load_Member_Grid_Results(objMembers);
                        }
                        else
                        {
                            XtraMessageBox.Show("Lista de Sócios vazia!", "Erro [Get_Selected_Member]", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }
                    }
                }
                if (bLoadOk)
                    _FocusedMemberRowBgColor = Color.FromArgb(242, 180, 82);
                else
                {
                    this.MembersFindResult = new AMFCMembers();
                    this.MemberSelected = new AMFCMember();
                    this.AllJoiasPaid = false;
                    this.Member_Found = false;
                }
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        #endregion Constructor

        #region Events

        #region     Form Events

        /// <versions>08-10-2017(v0.0.4.8)</versions>
        private void FindMember_Load(object sender, EventArgs e)
        {

        }

        /// <versions>08-10-2017(v0.0.4.8)</versions>
        private void FindMember_Activated(object sender, EventArgs e)
        {
            
        }

        /// <versions>08-10-2017(v0.0.4.8)</versions>
        private void FindMember_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        /// <versions>08-10-2017(v0.0.4.8)</versions>
        private void FindMember_FormClosed(object sender, FormClosedEventArgs e)
        {

        }

        #endregion  Form Events

        #region     Buttons Events

        /// <versions>08-10-2017(v0.0.4.8)</versions>
        private void Button_Select_Member_Cancel_Click(object sender, EventArgs e)
        {
            this.MembersFindResult = null;
            this.MemberSelected = null;
            this.Member_Found = false;
            this.AllJoiasPaid = false;
            this.Close();
        }

        /// <versions>08-10-2017(v0.0.4.8)</versions>
        private void Button_Select_Member_Ok_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion  Buttons Events

        #region     Find Results Grid Events

        /// <versions>08-10-2017(v0.0.4.8)</versions>
        private void GridView_Find_Member_Click(object sender, EventArgs e)
        {

        }

        /// <versions>08-10-2017(v0.0.4.8)</versions>
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

        /// <versions>08-10-2017(v0.0.4.8)</versions>
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

        /// <versions>08-10-2017(v0.0.4.8)</versions>this.MemberSelected = null;
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

        /// <versions>08-10-2017(v0.0.4.8)</versions>
        private Boolean Load_Member_Grid_Results(AMFCMembers objListMembersFound)
        {
            try
            {
                if (objListMembersFound == null || objListMembersFound.Members.Count == 0)
                {
                    DialogResult objResult = XtraMessageBox.Show("Não existem sócios com joias por pagar!", "Todas as joias pagas", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    if (objResult == DialogResult.OK)
                        this.Close();
                    this.MembersFindResult = null;
                    this.MemberSelected = null;
                    this.Member_Found = false;
                    this.AllJoiasPaid = true;
                    return false;
                }
                this.MembersFindResult = objListMembersFound;

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

        #region     Find Results Grid Methods

        /// <versions>08-10-2017(v0.0.4.8)</versions>
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
                #endregion  Config Grids Options

                return true;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return false;
            }
        }

        /// <versions>08-10-2017(v0.0.4.8)</versions>
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

        /// <versions>08-10-2017(v0.0.4.8)</versions>
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

        /// <versions>08-10-2017(v0.0.4.8)</versions>
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
                DataColumn objDataColumn_Member_NUMERO      = new DataColumn(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMERO),    typeof(Int64)); //Int64
                DataColumn objDataColumn_Member_NOME        = new DataColumn(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NOME),      typeof(String));
                DataColumn objDataColumn_Member_DATAADMI    = new DataColumn(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.DATAADMI),  typeof(String));
                #endregion  Data Columns Creation
                #region     Data Table Add Columns
                objDataTableResults.Columns.AddRange(
                                            new DataColumn[] {
                                                objDataColumn_Member_NUMERO,
                                                objDataColumn_Member_NOME,
                                                objDataColumn_Member_DATAADMI
                                            }
                                        );
                #endregion  Data Table Add Columns
                #region     Data Columns Dispose
                objDataColumn_Member_NUMERO.Dispose();
                objDataColumn_Member_NOME.Dispose();
                objDataColumn_Member_DATAADMI.Dispose();
                #endregion  Data Columns Dispose
                #endregion  TreeDataSet

                #region     Binding Data
                foreach (AMFCMember objMember in objListResults.Members)
                {
                    if (objMember == null || objMember.NUMERO < 1 || String.IsNullOrEmpty(objMember.NOME))
                        continue;
                    #region     Set Member Row Data
                    DataRow objDataRow = objDataTableResults.NewRow();
                    objDataRow[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMERO)]     = objMember.NUMERO.ToString();
                    objDataRow[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NOME)]       = objMember.NOME;
                    objDataRow[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.DATAADMI)]   = objMember.DATAADMI;
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

        /// <versions>08-10-2017(v0.0.4.8)</versions>
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

        /// <versions>08-10-2017(v0.0.4.8)</versions>
        private void Set_Results_Columns_Editability()
        {
            try
            {
                LibAMFC.SetGridColumnOptions(this.GridControl_Find_Member,  this.GridView_Find_Member,  new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMERO),     new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMERO),    "Número de sócio",  true, 1, 60,  true, false, true, false);
                LibAMFC.SetGridColumnOptions(this.GridControl_Find_Member,  this.GridView_Find_Member,  new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NOME),       new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.NOME),      "Nome do sócio",    true, 2, 250, true, false, true, false);
                LibAMFC.SetGridColumnOptions(this.GridControl_Find_Member,  this.GridView_Find_Member,  new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.DATAADMI),   new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.DATAADMI),  "Data admissão",    true, 3, 100, true, false, true, false);
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>08-10-2017(v0.0.4.8)</versions>
        private void Set_Results_Columns_Options_Filter()
        {
            try
            {
                LibAMFC.SetGridColumOptionsFilter(this.GridView_Find_Member, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMERO),    false, false, AutoFilterCondition.Contains, 8.0f);
                LibAMFC.SetGridColumOptionsFilter(this.GridView_Find_Member, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NOME),      false, false, AutoFilterCondition.Contains, 8.0f);
                LibAMFC.SetGridColumOptionsFilter(this.GridView_Find_Member, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.DATAADMI),  false, false, AutoFilterCondition.Contains, 8.0f);
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>08-10-2017(v0.0.4.8)</versions>
        private Boolean GridView_MembersFocusedRow(Int32 iRowHandle)
        {
            return GridView_Members_Focused_Row(iRowHandle, 0);
        }

        /// <versions>08-10-2017(v0.0.4.8)</versions>
        private Boolean GridView_MembersFocusedRow(Int32 iRowHandle, Int32 iMinRowHandle)
        {
            return GridView_Members_Focused_Row(iRowHandle, iMinRowHandle);
        }

        /// <versions>08-10-2017(v0.0.4.8)</versions>
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
                
                this.MemberSelected = objMember;
                if (this.MembersFindResult != null)
                    this.MembersFindResult.SelectedMember = objMember;
                this.Member_Found = true;
                this.AllJoiasPaid = false;
                #endregion  Get Focused Member

                return true;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return false;
            }
        }

        /// <versions>08-10-2017(v0.0.4.8)</versions>
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
    }
}