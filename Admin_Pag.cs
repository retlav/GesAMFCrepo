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
using System.Drawing;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;

namespace GesAMFC
{
    /// <summary>Members Entidades Admin</summary>
    /// <author>Valter Lima</author>
    /// <creation>08-12-2017(GesAMFC-v0.0.5.3)</creation>
    /// <versions>23-03-2018(GesAMFC-v1.0.1.0)</versions>
    public partial class Admin_Pag : DevExpress.XtraEditors.XtraForm
    {
        public Library_AMFC_Methods LibAMFC;

        #region    Form Fields

        #region     Entity
        private PAG_ENTIDADE Selected_Entidade;
        private PAG_ENTIDADE PreviousEntityState;
        private EntityTypeConfigs Entidade_Configs;
        #endregion  Entity

        private AMFCMember  Entidade_Member;

        private AMFCMemberLotes ListMemberLotes;
        private AMFCMemberLote Selected_Lote;

        public AMFCYear YearSelected;
        public AMFCMonth MonthSelected;
        //public  AMFCPeriod  PeriodSelected;

        private enum GridDatasourceType { UNDEF = -1, ALL = 1, MEMBER = 2, ID = 3, YEAR = 4, YEARMONTH = 5 }
        private GridDatasourceType Grid_Datasouce_Type;
        private String _GridColId       = "Id";
        private String _GridColSocio    = "MemberNumber";
        private Int32   LoadingGridPanelWaitTime = 500;
        #endregion  Grid Columns

        #region     Form Constructor 

        /// <versions>16-03-2018(GesAMFC-v1.0.0.3)</versions>
        public Admin_Pag(PAG_ENTIDADE.EntityTypes eEntityType)
        {
            LibAMFC = new Library_AMFC_Methods();

            try
            {
                this.Entidade_Member = new AMFCMember();
                this.Selected_Entidade = new PAG_ENTIDADE();
                this.PreviousEntityState = new PAG_ENTIDADE();
                
                this.ListMemberLotes = new AMFCMemberLotes();
                this.Selected_Lote = new AMFCMemberLote();

                this.Entidade_Configs = this.Selected_Entidade.GetEntityTypeConfigs(eEntityType);
                if (this.Entidade_Configs == null)
                {
                    XtraMessageBox.Show("Tipo de Pagamento Inválido!", "Erro [" + "Tipo de Pagamento" + "]", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                this.YearSelected = Program.DefaultYear;
                this.MonthSelected = Program.DefaultMonth;

                //this.PeriodSelected = new AMFCPeriod();
                //this.PeriodSelected.Start = new AMFCPeriodYearMonth();
                //this.PeriodSelected.Start.Year  = new AMFCYear(DateTime.Today.Year);
                //this.PeriodSelected.Start.Month = new AMFCMonth(1);
                //this.PeriodSelected.End = new AMFCPeriodYearMonth();
                //this.PeriodSelected.End.Year = new AMFCYear(DateTime.Today.Year);
                //this.PeriodSelected.End.Month = new AMFCMonth(12);

                Grid_Datasouce_Type = GridDatasourceType.ALL; 
                this.LoadingGridPanelWaitTime = 500;

                InitializeComponent();

                SetEntityControlsLabels();
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        #endregion  Form Constructor

        #region     Events

        #region     Form Events

        /// <versions>16-03-2018(GesAMFC-v1.0.0.3)</versions>
        private void Admin_Load(object sender, EventArgs e)
        {
            try
            {
                this.WindowState = FormWindowState.Maximized;
                this.Update();

                Program.SetCalendarControl(DateEdit_EntidadeData);

                Program.SetAreaEditValues(TextEdit_Area_Paga);
                Program.SetPayEditValues(TextEdit_Preco_Metro);
                Program.SetPayEditValues(TextEdit_Value);

                LibAMFC.GridConfiguration(this.Grid_Control, this.Grid_View, true, false, true, true, true);
                this.Grid_View.OptionsView.RowAutoHeight = true;
                this.Grid_View.OptionsView.AnimationType = DevExpress.XtraGrid.Views.Base.GridAnimationType.AnimateAllContent;
                this.Grid_View.OptionsView.WaitAnimationOptions = WaitAnimationOptions.Panel;
                SplitContainer_Grid.Panel2Collapsed = true;
                this.SplashScreenManager_LoadingGrid = new DevExpress.XtraSplashScreen.SplashScreenManager(this, typeof(global::GesAMFC.WaitFormLoadingPanel), true, true);

                LayoutControl_Details.Visible = false;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        #endregion  Form Events

        #region     Grid Events

        /// <versions>15-06-2017(v0.0.3.17)</versions>
        private void Grid_View_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                Int32 iMinRowHandle = 0;
                Int32 iRowHandle = e.FocusedRowHandle;
                if (iRowHandle < iMinRowHandle)
                    return;

                #region     Prevent the selectionchanged event
                if (this.Grid_View.IsGroupRow(iRowHandle))
                {
                    //(e as DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs)..Handled = true;
                    Clear_Details(true);
                    return;
                }
                #endregion  Prevent the selectionchanged event


                GridView_FocusedRow(iRowHandle, iMinRowHandle);
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>15-06-2017(v0.0.3.17)</versions>
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
                    //(e as DXMouseEventArgs).Handled = true; //não funciona o colapse/expande
                    Clear_Details(true);
                    return;
                }
                #endregion  Prevent the selectionchanged event

                #region     Check if is a valid Data Row Handle
                if (!hi.InRowCell || iRowHandle == GridControl.InvalidRowHandle || !this.Grid_View.IsDataRow(iRowHandle) || this.Grid_View.IsGroupRow(iRowHandle))
                {
                    this.Grid_View.ClearSelection();
                    //(e as DXMouseEventArgs).Handled = true;
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

        /// <versions>15-05-2017(v0.0.3.14)</versions>
        private void Grid_View_RowStyle(object sender, RowStyleEventArgs e)
        {
            try
            {
                if (e.RowHandle >= 0 && this.Grid_View.IsDataRow(e.RowHandle) && !this.Grid_View.IsGroupRow(e.RowHandle))
                {
                    if (e.RowHandle >= 0 && this.Grid_View.FocusedRowHandle >= 0 && e.RowHandle == this.Grid_View.FocusedRowHandle)
                        e.Appearance.BackColor = Program.FocusedRowBgColor;
                }
                else
                {
                    e.Appearance.BackColor = System.Drawing.Color.Transparent;
                }
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        #endregion  Grid Events

        #region     Action Buttons Events

        /// <versions>15-06-2017(v0.0.3.17)</versions>
        private void Button_Member_Find_Click(object sender, EventArgs e)
        {
            Load_Member();
        }

        /// <versions>01-12-2017(GesAMFC-v0.0.4.41)</versions>
        private void Button_Load_Year_Month_Members_Click(object sender, EventArgs e)
        {
            Load_Year_Month_Members();
        }

        /// <versions>02-12-2017(GesAMFC-v0.0.4.41)</versions>
        private void Button_Load_All_Members_Click(object sender, EventArgs e)
        {
            Load_All_Members();
        }

        /// <versions>22-03-2018(GesAMFC-v1.0.0.3)</versions>
        private void Button_Entity_Add_Click(object sender, EventArgs e)
        {
            LayoutControl_Details.Visible = true;
            Button_Add_Click();
        }

        /// <versions>06-10-2017(v0.0.4.8)</versions>
        private void ButtonAdd_Click(object sender, EventArgs e)
        {
            Button_Add_Click();
        }

        /// <versions>06-10-2017(v0.0.4.8)</versions>
        private void ButtonAdd_Save_Click(object sender, EventArgs e)
        {
            Button_Add_Save_Click();
        }

        /// <versions>06-10-2017(v0.0.4.8)</versions>
        private void ButtonAdd_Cancel_Click(object sender, EventArgs e)
        {
            Button_Add_Cancel_Click();
        }

        /// <versions>06-10-2017(v0.0.4.8)</versions>
        private void ButtonAdd_Repor_Click(object sender, EventArgs e)
        {
            Button_Add_Repor_Click();
        }

        /// <versions>06-10-2017(v0.0.4.8)</versions>
        private void ButtonEdit_Click(object sender, EventArgs e)
        {
            Button_Edit_Click();
        }

        /// <versions>06-10-2017(v0.0.4.8)</versions>
        private void ButtonEdit_Save_Click(object sender, EventArgs e)
        {
            Button_Edit_Save_Click();
        }

        /// <versions>06-10-2017(v0.0.4.8)</versions>
        private void ButtonEdit_Cancel_Click(object sender, EventArgs e)
        {
            Button_Edit_Cancel_Click();
        }

        /// <versions>06-10-2017(v0.0.4.8)</versions>
        private void ButtonEdit_Repor_Click(object sender, EventArgs e)
        {
            Button_Edit_Repor_Click();
        }

        /// <versions>08-10-2017(v0.0.4.8)</versions>
        private void TextEdit_Value_Click(object sender, EventArgs e)
        {
            if (TextEdit_Value.Properties.ReadOnly)
                Edit_Action();
        }

        /// <versions>16-03-2018(GesAMFC-v1.0.0.3)</versions>
        private void TextEdit_Area_Paga_Click(object sender, EventArgs e)
        {
            if (TextEdit_Area_Paga.Properties.ReadOnly)
                Edit_Action();
        }

        private void TextEdit_Preco_Metro_Click(object sender, EventArgs e)
        {
            if (TextEdit_Preco_Metro.Properties.ReadOnly)
                Edit_Action();
        }

        private void DateEdit_EntidadeData_Click(object sender, EventArgs e)
        {
            if (TextEdit_Preco_Metro.Properties.ReadOnly)
                Edit_Action();
        }

        /// <versions>08-10-2017(v0.0.4.8)</versions>
        private void TextEdit_EntidadeData_Click(object sender, EventArgs e)
        {
            if (DateEdit_EntidadeData.Properties.ReadOnly)
                Edit_Action();
        }

        /// <versions>08-10-2017(v0.0.4.8)</versions>
        private void TextEdit_PayDate_Click(object sender, EventArgs e)
        {
            if (DateEdit_EntidadeData.Properties.ReadOnly)
                Edit_Action();
        }
        
        /// <versions>08-10-2017(v0.0.4.8)</versions>
        private void TextEditNotas_Click(object sender, EventArgs e)
        {
            if (TextEdit_Notas.Properties.ReadOnly)
                Edit_Action();
        }

        /// <versions>06-10-2017(v0.0.4.8)</versions>
        private void ButtonDel_Click(object sender, EventArgs e)
        {
            Button_Del_Click();
        }

        /// <versions>06-10-2017(v0.0.4.8)</versions>
        //private void Button_Pay_Click(object sender, EventArgs e)
        //{
        //    Pay();
        //}

        private void Button_Show_Pay_Click(object sender, EventArgs e)
        {
            Open();
        }

        #endregion  Action Buttons Events

        #region     DateEdit Events

        /// <versions>21-11-2017(GesAMFC-v0.0.4.35)</versions>
        private void DateEdit_EntidadeData_QueryPopUp(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DateEdit dateEdit = sender as DateEdit;
            Program.DateEdit_QueryPopUp_Event(dateEdit, e);
        }

        #endregion  DateEdit Events

        #endregion  Events

        #region     Methods

        #region     Form Methods

        /// <versions>20-03-2018(GesAMFC-v1.0.0.3)</versions>
        private void SetEntityControlsLabels()
        {
            try
            {
                this.Text = "Gestão de Pagamentos de " + this.Entidade_Configs.Entity_Desc_Plural;
                this.Label_Admin.Text = "GESTÃO DE PAGAMENTOS DE " + this.Entidade_Configs.Entity_Upper_Plural;
                this.Button_Load_All_Members.ToolTip = "Carregar todos os pagamentos de " + this.Entidade_Configs.Entity_Desc_Short_Plural;
                this.Button_Load_Year_Month_Members.ToolTip = "Carregar os pagamentos de " + this.Entidade_Configs.Entity_Desc_Short_Plural + " do ano/mês selecionado(s)";
                this.Button_Member_Find.ToolTip = "Carregar os pagamentos de " + this.Entidade_Configs.Entity_Desc_Short_Plural + " do sócio selecionado";
                this.Label_Table_Header.Text = "LISTA DE PAGAMENTOS DE " + this.Entidade_Configs.Entity_Upper_Plural;
                this.LabelDetails.Text = "DETALHES DO PAGAMENTO DE " + this.Entidade_Configs.Entity_Upper_Single;
                this.Button_Member_Add.ToolTip = "Adicionar pagamento de " + this.Entidade_Configs.Entity_Desc_Short_Plural;
                this.Button_Member_Edit.ToolTip = "Editar pagamento de " + this.Entidade_Configs.Entity_Desc_Short_Plural;
                this.Button_Member_Del.ToolTip = "Apagar pagamento de " + this.Entidade_Configs.Entity_Desc_Short_Plural;
                this.ButtonEdit_Save.ToolTip = "Guardar alterações do pagamento de " + this.Entidade_Configs.Entity_Desc_Short_Plural;
                this.ButtonEdit_Cancel.ToolTip = "Cancelar alterações do pagamento de " + this.Entidade_Configs.Entity_Desc_Short_Plural;
                this.ButtonEdit_Repor.ToolTip = "Recarregar detalhes do pagamento de " + this.Entidade_Configs.Entity_Desc_Short_Plural;
                this.ButtonAdd_Cancel.ToolTip = "Cancelar adição do pagamento de " + this.Entidade_Configs.Entity_Desc_Short_Plural;
                this.ButtonAdd_Save.ToolTip = "Guardar o pagamento de " + this.Entidade_Configs.Entity_Desc_Short_Plural;
                this.ButtonAdd_Repor.ToolTip = "Limpar detalhes do pagamento de " + this.Entidade_Configs.Entity_Desc_Short_Plural;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        #endregion  Form Methods

        #region     Grid Methods

        #region Comum

        /// <versions>05-05-2017(v0.0.3.2)</versions>
        private void Grid_Clear_Selection()
        {
            StackFrame objStackFrame = new StackFrame();
            try
            {
                this.Grid_View.ClearSelection();
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
            finally { objStackFrame = null; }
        }

        /// <versions>14-05-2017(v0.0.3.13)</versions>
        private Boolean GridView_FocusedRow(Int32 iRowHandle)
        {
            return Get_Grid_Focused_Row(iRowHandle, 0);
        }

        /// <versions>14-05-2017(v0.0.3.13)</versions>
        private Boolean GridView_FocusedRow(Int32 iRowHandle, Int32 iMinRowHandle)
        {
            return Get_Grid_Focused_Row(iRowHandle, iMinRowHandle);
        }

        /// <versions>15-06-2017(v0.0.3.17)</versions>
        private Boolean Get_Grid_Focused_Row(Int32 iRowHandle, Int32 iMinRowHandle)
        {
            try
            {
                StackFrame objStackFrame = new StackFrame();

                #region     Check if is a valid Data Row Handle
                if (iRowHandle < iMinRowHandle || iRowHandle == GridControl.InvalidRowHandle || !this.Grid_View.IsDataRow(iRowHandle))
                {
                    //this.GridView_AMFC_Members.ClearSelection();
                    Clear_Details(true);
                    return false;
                }
                #endregion  Check if is a valid Data Row Handle

                LayoutControl_Details.Visible = true;

                #region     Get Focused 
                Boolean bIsTheSameMember = IsTheSameSelected(iRowHandle, -1);
                PAG_ENTIDADE objEnt = null;
                if (!bIsTheSameMember)
                    objEnt = Get_Selected(iRowHandle, -1);
                else
                    objEnt = this.Selected_Entidade;

                if (objEnt == null || objEnt.SOCIO < 1 || objEnt.SOCIO > new AMFCMember().MaxNumber || objEnt.ID < 1)
                {
                    XtraMessageBox.Show(objStackFrame.GetMethod().Name, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                #endregion  Get Focused 

                #region     Load Member Details
                if (!bIsTheSameMember)
                    Load_Entity_Details(objEnt.ID);
                #endregion  Load Member Details

                return true;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return false;
            }
        }

        /// <versions>07-10-2017(v0.0.4.8)</versions>
        private PAG_ENTIDADE Get_Selected(Int32 iRowHandle, Int64 lId)
        {
            try
            {
                PAG_ENTIDADE objEnt = null;
                Int32 iFocusedRowHandle = -1;
                Int64 lFocusedId = -1;

                if (lId < 1)
                {
                    if (iRowHandle > -1)
                        iFocusedRowHandle = iRowHandle;
                    else
                        iFocusedRowHandle = this.Grid_View.FocusedRowHandle;
                    if (iFocusedRowHandle < 0 || iFocusedRowHandle == GridControl.InvalidRowHandle || !this.Grid_View.IsDataRow(iFocusedRowHandle))
                    {
                        XtraMessageBox.Show("Não foi possível obter os detalhes do " + "Pagamento" + " de " + this.Entidade_Configs.Entity_Desc_Short_Single + " do sócio selecionado!", "Erro Seleção", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return null;
                    }
                    lFocusedId = Convert.ToInt64(this.Grid_View.GetRowCellValue(iFocusedRowHandle, this.Grid_View.Columns[_GridColId]));
                }
                else
                    lFocusedId = lId;

                if (lFocusedId == this.Selected_Entidade.ID)
                    return this.Selected_Entidade;
                objEnt = DBF_Get_Member_Pagamento_ById(lFocusedId);

                if(objEnt == null || objEnt.SOCIO < 1 || objEnt.SOCIO > new AMFCMember().MaxNumber || objEnt.ID < 1)
                {
                    XtraMessageBox.Show("Não foi possível obter os detalhes do Pagamento de " + this.Entidade_Configs.Entity_Desc_Short_Single + " selecionado!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
                this.Selected_Entidade = objEnt;
                this.PreviousEntityState = objEnt;
                return objEnt;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return null;
            }
        }
        
        /// <versions>15-06-2017(v0.0.3.17)</versions>
        private Boolean IsTheSameSelected(Int32 iRowHandle, Int64 lId)
        {
            try
            {
                Int32 iFocusedRowHandle = -1;
                Int64 lFocusedId = -1;

                if (lId < 1)
                {
                    if (iRowHandle > -1)
                        iFocusedRowHandle = iRowHandle;
                    else
                        iFocusedRowHandle = this.Grid_View.FocusedRowHandle;
                    if (iFocusedRowHandle < 0 || iFocusedRowHandle == GridControl.InvalidRowHandle || !this.Grid_View.IsDataRow(iFocusedRowHandle))
                    {
                        XtraMessageBox.Show("Não foi possível obter os detalhes de " + this.Entidade_Configs.Entity_Desc_Short_Single + " selecionado!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    lFocusedId = Convert.ToInt64(this.Grid_View.GetRowCellValue(iFocusedRowHandle, this.Grid_View.Columns[_GridColId]));
                }
                else
                    lFocusedId = lId;
                if (lFocusedId == this.Selected_Entidade.ID)
                    return true;

                return false;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return false;
            }
        }

        /// <versions>15-05-2017(v0.0.3.14)</versions>
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

        #endregion Comum

        /// <versions>30-11-2017(GesAMFC-v0.0.4.40)</versions>
        private void Load_Grid_Info(Boolean bSetDatasource, Boolean bClearAll, Boolean bClearTotals, Int32 iFocusedRowHandle, Int64 lId)
        {
            try
            {
                Boolean bSetOptionsSelection = bClearAll, bSetCols = bClearAll, bClearSorting = bClearAll, bClearFilters = bClearAll, bClearGrouping = bClearAll, bSetTotals = bClearTotals;
                Load_Grid(bSetDatasource, bSetOptionsSelection, bSetCols, bClearSorting, bClearFilters, bClearGrouping, bSetTotals, iFocusedRowHandle, lId);
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>30-11-2017(GesAMFC-v0.0.4.40)</versions>
        private void Load_Grid(Boolean bSetDatasource, Boolean bSetOptionsSelection, Boolean bSetCols, Boolean bClearSorting, Boolean bClearFilters, Boolean bClearGrouping, Boolean bSetTotals, Int32 iFocusedRowHandle, Int64 lId)
        {
            try
            {
                Grid_Clear_Selection();

                #region     Grid Data Source
                if (bSetDatasource)
                    Load_Grid_Datasource(bSetCols, bClearSorting, bClearFilters, bClearGrouping, bSetTotals, true);
                #endregion  Grid Data Source

                #region     Config Grids Options
                if (bSetOptionsSelection)
                {
                    this.Grid_View.OptionsSelection.EnableAppearanceFocusedRow = true;
                    this.Grid_View.OptionsSelection.EnableAppearanceFocusedCell = false;
                    this.Grid_View.OptionsSelection.EnableAppearanceHideSelection = true;
                    this.Grid_View.OptionsSelection.UseIndicatorForSelection = false;
                    this.Grid_View.ClearSelection();
                }
                #endregion  Config Grids Options

                #region     Set Focused 
                if (lId > 0)
                {
                    Int32 iRowHandle = this.Grid_View.LocateByValue(_GridColId, lId);
                    if (iRowHandle != GridControl.InvalidRowHandle)
                        this.Grid_View.FocusedRowHandle = iRowHandle;
                }
                else if (iFocusedRowHandle > -1)
                {
                    if (iFocusedRowHandle != GridControl.InvalidRowHandle)
                        this.Grid_View.FocusedRowHandle = iFocusedRowHandle;
                }
                #endregion  Set Focused 

                return;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>01-12-2017(GesAMFC-v0.0.4.41)</versions>
        private void Load_Grid_Datasource(Boolean bSetCols, Boolean bClearSorting, Boolean bClearFilters, Boolean bClearGrouping, Boolean bSetTotals, Boolean bWriteLog)
        {
            try
            {
                SplitContainer_Grid.Panel2Collapsed = false;

                LibAMFC.CleanGrid(this.Grid_Control, this.Grid_View, bSetCols, bClearSorting, bClearFilters, bClearGrouping);
                this.Update();
                this.Grid_View.ShowLoadingPanel();

                #region     Loading Panel While Datasource Bind
                Boolean bLoadDatasource = false;
                this.SplashScreenManager_LoadingGrid.ShowWaitForm();
                try
                {
                    this.SplashScreenManager_LoadingGrid.SetWaitFormCaption("Aguarde");
                    this.SplashScreenManager_LoadingGrid.SetWaitFormDescription("A carregar Lista de " + this.Entidade_Configs.Entity_Desc_Plural + " ...");
                    Thread.Sleep(this.LoadingGridPanelWaitTime);
                    bLoadDatasource = Set_Grid_Data_Source(this.Grid_Control);
                }
                finally
                {
                    this.SplashScreenManager_LoadingGrid.CloseWaitForm();
                }
                #endregion  Loading Panel While Datasource Bind

                SplitContainer_Grid.Panel2Collapsed = true;
                this.Grid_View.HideLoadingPanel();

                if (bLoadDatasource)
                {
                    if (bSetCols)
                        Set_Grid_Columns();
                    if (bSetTotals)
                        Set_Grid_Totals_Summaries();
                }

                this.Update();
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>30-11-2017(GesAMFC-v0.0.4.40)</versions>
        private Boolean Set_Grid_Data_Source(GridControl objGridControl)
        {
            StackFrame objStackFrame = new StackFrame();
            String sErrorMsg = String.Empty;
            try
            {
                #region         Members Datasource
                LIST_PAG_ENTIDADE objPAG_ENTIDADEs = new LIST_PAG_ENTIDADE();
                using (Library_AMFC_SQL obj_AMFC_SQL = new Library_AMFC_SQL())
                {
                    switch (this.Grid_Datasouce_Type)
                    {
                        case GridDatasourceType.ALL:
                            objPAG_ENTIDADEs = obj_AMFC_SQL.Get_All_Member_ENTID(this.Entidade_Configs, -1, -1);
                            this.LoadingGridPanelWaitTime = 500;
                            break;
                        case GridDatasourceType.MEMBER:
                            objPAG_ENTIDADEs = obj_AMFC_SQL.Get_Member_ENTID_ByNbr(this.Entidade_Configs, this.Entidade_Member.NUMERO, this.Selected_Lote.IDLOTE, this.YearSelected.Value, this.MonthSelected.Value);
                            this.LoadingGridPanelWaitTime = 500;
                            break;
                        case GridDatasourceType.YEAR:
                            objPAG_ENTIDADEs = obj_AMFC_SQL.Get_Member_ENTID_ByNbr(this.Entidade_Configs, this.Entidade_Member.NUMERO, this.Selected_Lote.IDLOTE, this.YearSelected.Value, -1);
                            this.LoadingGridPanelWaitTime = 500;
                            break;
                        case GridDatasourceType.YEARMONTH:
                            objPAG_ENTIDADEs = obj_AMFC_SQL.Get_Members_ENTID_ByYearMonth(this.Entidade_Configs, this.YearSelected.Value, this.MonthSelected.Value);
                            this.LoadingGridPanelWaitTime = 500;
                            break;
                        case GridDatasourceType.ID:
                            objPAG_ENTIDADEs = new LIST_PAG_ENTIDADE();
                            objPAG_ENTIDADEs.Add(obj_AMFC_SQL.Get_Member_ENTID_ById(this.Entidade_Configs, this.Selected_Entidade.ID));
                            this.LoadingGridPanelWaitTime = 500;
                            break;
                        default:
                            break;
                    }
                }
                if (objPAG_ENTIDADEs == null)
                {
                    sErrorMsg = "Não foi possível obter a Lista " + this.Entidade_Configs.Entity_Desc_Plural + "!";
                    Program.HandleError(objStackFrame.GetMethod().Name, sErrorMsg, Program.ErroType.ERROR, true, false);
                    return false;
                }
                if (objPAG_ENTIDADEs.List.Count == 0)
                {
                    String sWarningMsg = "Não existem " + this.Entidade_Configs.Entity_Desc_Plural + "!";
                    MessageBox.Show(sWarningMsg, "No results", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    LayoutControl_Details.Visible = true;
                    return false;
                }
                #endregion  Members Datasource

                #region     TreeDataSet
                DataTable objDataTableSource = new DataTable("AMFC_Members_" + this.Entidade_Configs.Entity_Desc_Plural);

                #region     Data Columns Creation
                //DataColumn objDataColumn_EntidadeIndex = new DataColumn("Index", typeof(Int64));
                DataColumn objDataColumn_Id = new DataColumn(_GridColId, typeof(Int64));
                DataColumn objDataColumn_MemberNumber = new DataColumn(_GridColSocio, typeof(Int64));
                DataColumn objDataColumn_MemberName = new DataColumn("MemberName", typeof(String));
                DataColumn objDataColumn_Lote_Numero = new DataColumn("LoteNumero", typeof(String));
                DataColumn objDataColumn_Date = new DataColumn("Date", typeof(String));
                DataColumn objDataColumn_Year = new DataColumn("Year", typeof(String));
                //DataColumn objDataColumn_YearInt = new DataColumn("YearInt", typeof(String));
                DataColumn objDataColumn_Month = new DataColumn("Month", typeof(String));
                //DataColumn objDataColumn_MonthInt = new DataColumn("MonthInt", typeof(String));
                DataColumn objDataColumn_MonthYear = new DataColumn("MonthYear", typeof(String));
                DataColumn objDataColumn_AREAPAGO = new DataColumn("AREAPAGO", typeof(Double)); 
                DataColumn objDataColumn_PRECOM2P = new DataColumn("PRECOM2P", typeof(Double));
                DataColumn objDataColumn_Value = new DataColumn("Value", typeof(Double));               
                DataColumn objDataColumn_PayState = new DataColumn("PayState", typeof(String));
                DataColumn objDataColumn_Notas = new DataColumn("Notas", typeof(String));
                DataColumn objDataColumn_ListaCaixa = new DataColumn("ListaCaixa", typeof(String));
                DataColumn objDataColumn_ListaRecibos = new DataColumn("ListaRecibos", typeof(String));
                DataColumn objDataColumn_Lote_Id = new DataColumn("LoteId", typeof(Int64));


                #endregion  Data Columns Creation
                #region     Data Table Add Columns
                objDataTableSource.Columns.AddRange(
                                            new DataColumn[] {
                                                //objDataColumn_EntidadeIndex,
                                                objDataColumn_Id,
                                                objDataColumn_MemberNumber,
                                                objDataColumn_MemberName,
                                                objDataColumn_Lote_Numero,
                                                objDataColumn_Date,
                                                objDataColumn_Year,
                                                //objDataColumn_YearInt,
                                                objDataColumn_Month,
                                                //objDataColumn_MonthInt,
                                                objDataColumn_MonthYear,
                                                objDataColumn_AREAPAGO,
                                                objDataColumn_Value,
                                                objDataColumn_PRECOM2P,
                                                objDataColumn_PayState,
                                                objDataColumn_Notas,
                                                objDataColumn_ListaCaixa,
                                                objDataColumn_ListaRecibos,
                                                objDataColumn_Lote_Id
                                            }
                                        );
                #endregion  Data Table Add Columns
                #region     Data Columns Dispose
                //objDataColumn_EntidadeIndex.Dispose();
                objDataColumn_Id.Dispose();
                objDataColumn_MemberNumber.Dispose();
                objDataColumn_MemberName.Dispose();
                objDataColumn_Lote_Numero.Dispose();
                objDataColumn_Date.Dispose();
                objDataColumn_Year.Dispose();
                //objDataColumn_YearInt.Dispose();
                objDataColumn_Month.Dispose();
                //objDataColumn_MonthInt.Dispose();
                objDataColumn_MonthYear.Dispose();
                objDataColumn_AREAPAGO.Dispose();
                objDataColumn_PRECOM2P.Dispose();
                objDataColumn_Value.Dispose();
                objDataColumn_PayState.Dispose();
                objDataColumn_Notas.Dispose();
                objDataColumn_ListaCaixa.Dispose();
                objDataColumn_ListaRecibos.Dispose();
                objDataColumn_Lote_Id.Dispose();
                #endregion  Data Columns Dispose
                #endregion  TreeDataSet

                #region     Binding Data
                Int32 iRowIdx = -1;
                foreach (PAG_ENTIDADE objEntidade in objPAG_ENTIDADEs.List)
                {
                    if (objEntidade == null || objEntidade.SOCIO < 1 || objEntidade.SOCIO > new AMFCMember().MaxNumber || objEntidade.ID < 1)
                        continue;

                    #region     Set Row Data
                    iRowIdx++;

                    DataRow objDataRow = objDataTableSource.NewRow();
                    //objDataRow["Index"] = iRowIdx;
                    objDataRow[_GridColId] = objEntidade.ID;
                    objDataRow[_GridColSocio] = objEntidade.SOCIO;
                    objDataRow["MemberName"] = objEntidade.NOME;
                    objDataRow["LoteNumero"] = objEntidade.NUMLOTE;
                    objDataRow["Date"] = objEntidade.DATAPAG.ToString(Program.Date_Format_String, CultureInfo.CurrentCulture);
                    objDataRow["Year"] = objEntidade.ANO;
                    //objDataRow["YearInt"] = objEntidade.ANO.ToString();
                    objDataRow["Month"] = new DateTime(objEntidade.ANO, objEntidade.MES, 1).ToString("MMMM", Program.CurrentCulture); 
                    //objDataRow["MonthInt"] = objEntidade.MES;
                    objDataRow["MonthYear"] = objEntidade.DATAPAG.ToString("MMM", Program.CurrentCulture) + "/" + objEntidade.ANO;
                    objDataRow["AREAPAGO"] = objEntidade.AREAPAGO;
                    objDataRow["PRECOM2P"] = objEntidade.PRECOM2P;
                    objDataRow["Value"] = objEntidade.VALORPAGO;
                    objDataRow["PayState"] = objEntidade.Pay_State.ToString();
                    objDataRow["Notas"] = objEntidade.NOTAS;
                    objDataRow["ListaCaixa"] = objEntidade.LISTACAIXA;
                    objDataRow["ListaRecibos"] = objEntidade.LISTARECNU;
                    objDataRow["LoteId"] = objEntidade.IDLOTE;
                    #endregion  Set Row Data

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

        /// <versions>07-05-2017(v0.0.3.3)</versions>
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

        /// <versions>30-11-2017(GesAMFC-v0.0.4.40)</versions>
        public void Set_Grid_Totals_Summaries()
        {
            try
            {
                this.Grid_View.GroupSummary.Clear();

                this.Grid_View.GroupFooterShowMode = GroupFooterShowMode.VisibleAlways;

                GridGroupSummaryItem objGroupTotal = new GridGroupSummaryItem();
                objGroupTotal.SummaryType = DevExpress.Data.SummaryItemType.Count;
                objGroupTotal.FieldName = "Value";
                this.Grid_View.GroupSummary.Add(objGroupTotal);

                GridGroupSummaryItem objGroupTotalValues = new GridGroupSummaryItem();
                objGroupTotalValues.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                objGroupTotalValues.FieldName = "Value";
                objGroupTotalValues.DisplayFormat = "Total = {0:c2}";
                objGroupTotalValues.ShowInGroupColumnFooter = this.Grid_View.Columns["Value"];
                this.Grid_View.GroupSummary.Add(objGroupTotalValues);

                this.Grid_View.OptionsView.ShowFooter = true;

                GridColumnSummaryItem objTotalGlobalValues = new GridColumnSummaryItem();
                objTotalGlobalValues.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                objTotalGlobalValues.FieldName = "Value";
                objTotalGlobalValues.DisplayFormat = "Total Global = {0:c2}";
                this.Grid_View.Columns["Value"].Summary.Clear();
                this.Grid_View.Columns["Value"].Summary.Add(objTotalGlobalValues);
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>30-11-2017(GesAMFC-v0.0.4.40)</versions>
        private void Set_Grid_Columns_Editability()
        {
            try
            {
                LibAMFC.SetGridColumnOptions(this.Grid_Control, this.Grid_View, _GridColId, "Id" + " " + "Pagamento", "Id do " + "Pagamento", false, -1, 50, true, false, false, false, HorzAlignment.Center, VertAlignment.Center, HorzAlignment.Far, VertAlignment.Center);

                LibAMFC.SetGridColumnOptions(this.Grid_Control, this.Grid_View, "MonthYear", "Ano/Mês", "Ano/Mês do " + "Pagamento", true, 1, 120, true, false, true, true, HorzAlignment.Center, VertAlignment.Center, HorzAlignment.Center, VertAlignment.Center);

                LibAMFC.SetGridColumnOptions(this.Grid_Control, this.Grid_View, _GridColSocio, "Nº Sócio", "Número de sócio", true, 2, 90, true, false, true, false, HorzAlignment.Far, VertAlignment.Center, HorzAlignment.Far, VertAlignment.Center);
                LibAMFC.SetGridColumnOptions(this.Grid_Control, this.Grid_View, "MemberName", "Nome", "Nome do sócio", true, 3, 180, true, false, true, false, HorzAlignment.Near, VertAlignment.Center, HorzAlignment.Near, VertAlignment.Center);

                LibAMFC.SetGridColumnOptions(this.Grid_Control, this.Grid_View, "LoteNumero", "Lote", "Lote do sócio", true, 4, 80, true, false, true, false, HorzAlignment.Center, VertAlignment.Center, HorzAlignment.Center, VertAlignment.Center);
                
                LibAMFC.SetGridColumnOptions(this.Grid_Control, this.Grid_View, "Year", "Ano", "Ano", false, -1, 60, true, false, true, true, 0, HorzAlignment.Center, VertAlignment.Center, HorzAlignment.Center, VertAlignment.Center);
                LibAMFC.SetGridColumnOptions(this.Grid_Control, this.Grid_View, "Month", "Mês", "Mês", false, -1, 60, true, false, true, true, 1, HorzAlignment.Center, VertAlignment.Center, HorzAlignment.Center, VertAlignment.Center);
                
                //LibAMFC.SetGridColumnOptions(this.Grid_Control, this.Grid_View, "YearInt", "Ano", "Ano", true, 10, 60, true, false, true, true, HorzAlignment.Center, VertAlignment.Center, HorzAlignment.Center, VertAlignment.Center);
                //LibAMFC.SetGridColumnOptions(this.Grid_Control, this.Grid_View, "MonthInt", "Mês", "Mês", true, 11, 60, true, false, true, true, HorzAlignment.Center, VertAlignment.Center, HorzAlignment.Center, VertAlignment.Center);

                LibAMFC.SetGridColumnOptions(this.Grid_Control, this.Grid_View, "Date", "Data Pagamento", "Data de Pagamento", true, 12, 140, true, false, false, false, HorzAlignment.Center, VertAlignment.Center, HorzAlignment.Center, VertAlignment.Center);

                LibAMFC.SetGridColumnOptions(this.Grid_Control, this.Grid_View, "Notas", "Notas", "Observações", false, -1, 220, true, false, true, false, HorzAlignment.Center, VertAlignment.Center, HorzAlignment.Center, VertAlignment.Center);

                LibAMFC.SetGridColumnOptions(this.Grid_Control, this.Grid_View, "ListaCaixa", "ListaCaixa", "ListaCaixa", false, -1, 80, true, false, true, true, HorzAlignment.Center, VertAlignment.Center, HorzAlignment.Center, VertAlignment.Center);
                LibAMFC.SetGridColumnOptions(this.Grid_Control, this.Grid_View, "ListaRecibos", "ListaRecibos", "ListaRecibos", false, -1, 80, true, false, true, true, HorzAlignment.Center, VertAlignment.Center, HorzAlignment.Center, VertAlignment.Center);
                LibAMFC.SetGridColumnOptions(this.Grid_Control, this.Grid_View, "PayState", "PayState", "PayState", false, -1,  80, true,  false, true,     true,      HorzAlignment.Center,  VertAlignment.Center,      HorzAlignment.Center,  VertAlignment.Center);

                LibAMFC.SetGridColumnOptions(this.Grid_Control, this.Grid_View, "LoteId", "Id Lote", "Id do Lote", false, -1, 80, true, false, true, true, HorzAlignment.Center, VertAlignment.Center, HorzAlignment.Center, VertAlignment.Center);

                LibAMFC.SetGridColumnOptions(this.Grid_Control, this.Grid_View, "AREAPAGO", "Área Paga [M2]", "Área Paga", true, 13, 130, true, false, true, true, HorzAlignment.Center, VertAlignment.Center, HorzAlignment.Far, VertAlignment.Center, false);
                LibAMFC.SetGridColumnOptions(this.Grid_Control, this.Grid_View, "PRECOM2P", "Preço/M2", "Preço por Metro Quadrado", true, 13, 100, true, false, true, true, HorzAlignment.Center, VertAlignment.Center, HorzAlignment.Far, VertAlignment.Center, true);
                LibAMFC.SetGridColumnOptions(this.Grid_Control, this.Grid_View, "Value", "Valor Pago [€]", "Valores Pagos em Euros", true, 13, 140, true, false, true, true, HorzAlignment.Center, VertAlignment.Center, HorzAlignment.Far, VertAlignment.Center, true);
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>30-11-2017(GesAMFC-v0.0.4.40)</versions>
        private void Set_Grid_Columns_OptionsFilter()
        {
            try
            {
                LibAMFC.SetGridColumOptionsFilter(this.Grid_View, _GridColId, true, true, AutoFilterCondition.Equals, 8.0f);

                LibAMFC.SetGridColumOptionsFilter(this.Grid_View, _GridColSocio, true, true, AutoFilterCondition.Equals, 8.0f);
                LibAMFC.SetGridColumOptionsFilter(this.Grid_View, "MemberName", true, true, AutoFilterCondition.Contains, 8.5f);

                LibAMFC.SetGridColumOptionsFilter(this.Grid_View, "LoteNumero", true, true, AutoFilterCondition.Contains, 8.5f);

                LibAMFC.SetGridColumOptionsFilter(this.Grid_View, "MonthYear", true, true, AutoFilterCondition.Equals, 8.5f);
                LibAMFC.SetGridColumOptionsFilter(this.Grid_View, "Year", true, true, AutoFilterCondition.Equals, 8.5f);
                LibAMFC.SetGridColumOptionsFilter(this.Grid_View, "Month", true, true, AutoFilterCondition.Equals, 8.5f);

                LibAMFC.SetGridColumOptionsFilter(this.Grid_View, "AREAPAGO", true, true, AutoFilterCondition.Contains, 8.5f);
                LibAMFC.SetGridColumOptionsFilter(this.Grid_View, "PRECOM2P", true, true, AutoFilterCondition.Contains, 8.5f);
                LibAMFC.SetGridColumOptionsFilter(this.Grid_View, "Value", true, true, AutoFilterCondition.Contains, 8.5f);

                LibAMFC.SetGridColumOptionsFilter(this.Grid_View, "Date", true, true, AutoFilterCondition.Equals, 8.5f);

                LibAMFC.SetGridColumOptionsFilter(this.Grid_View, "Notas", true, true, AutoFilterCondition.Equals, 8.5f);
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        #endregion  Grid Methods

        #region     Actions Methods

        /// <versions>07-10-2017(v0.0.4.8)</versions>
        private void Load_Member()
        {
            try
            {
                this.PreviousEntityState = new PAG_ENTIDADE();

                Find_Member_Form objFindMemberForm = new Find_Member_Form();
                if (objFindMemberForm != null)
                {
                    objFindMemberForm.FormClosing += delegate
                    {
                        if (Program.Member_Found) //trocar por objFindMemberForm.Member_Found e remover Program.Member_Found no futuro
                        {
                            AMFCMember objMemberSelected = objFindMemberForm.MemberSelected;
                            if (objMemberSelected != null && objMemberSelected.NUMERO > 0 && objMemberSelected.NUMERO < objMemberSelected.MaxNumber)
                            {
                                this.Entidade_Member = objMemberSelected;

                                Get_Member_Lotes_Get_Pagamentos();
                            }
                        }
                        else
                            this.Grid_View.FocusedRowHandle = -1;
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

        /// <versions>15-03-2018(GesAMFC-v1.0.0.3)</versions>
        private void Get_Member_Lotes_Get_Pagamentos()
        {
            try
            {
                if (this.Entidade_Member == null || this.Entidade_Member.NUMERO < this.Entidade_Member.MinNumber || this.Entidade_Member.NUMERO > this.Entidade_Member.MaxNumber)
                    return;

                this.ListMemberLotes = Get_DBF_AMFC_Member_Lotes(this.Entidade_Member.NUMERO);
                if (this.ListMemberLotes == null || this.ListMemberLotes.Lotes.Count == 0)
                {
                    String sErrorMsg = "Não foi possível obter a Lista de Lotes do Sócio Nº: " + this.Entidade_Member.NUMERO;
                    Program.HandleError("", sErrorMsg, Program.ErroType.ERROR, true, true);
                    return;
                }

                if (this.ListMemberLotes.Lotes.Count == 1)
                {
                    this.Selected_Lote = this.ListMemberLotes.Lotes[0];
                    this.Grid_Datasouce_Type = GridDatasourceType.MEMBER;
                    Load_Grid_Info(true, true, true, -1, -1);
                    this.LoadingGridPanelWaitTime = 500;
                }
                else
                    Load_Lote_Get_Pagamentos();
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return;
            }
        }

        /// <versions>15-03-2018(GesAMFC-v1.0.0.3)</versions>
        private void Get_Member_Lotes_Add_Pagamentos()
        {
            try
            {
                if (this.Entidade_Member == null || this.Entidade_Member.NUMERO < this.Entidade_Member.MinNumber || this.Entidade_Member.NUMERO > this.Entidade_Member.MaxNumber)
                    return;

                this.ListMemberLotes = Get_DBF_AMFC_Member_Lotes(this.Entidade_Member.NUMERO);
                if (this.ListMemberLotes == null || this.ListMemberLotes.Lotes.Count == 0)
                {
                    String sErrorMsg = "Não foi possível obter a Lista de Lotes do Sócio Nº: " + this.Entidade_Member.NUMERO;
                    Program.HandleError("", sErrorMsg, Program.ErroType.ERROR, true, true);
                    return;
                }

                if (this.ListMemberLotes.Lotes.Count == 1)
                {
                    this.Selected_Lote = this.ListMemberLotes.Lotes[0];
                    Button_Add_Action(this.Entidade_Member, this.Selected_Lote);
                }
                else
                    Load_Lote_Add_Pagamentos();
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return;
            }
        }

        /// <versions>15-03-2018(GesAMFC-v1.0.0.3)</versions>
        private void Load_Lote_Get_Pagamentos()
        {
            try
            {
                if (this.ListMemberLotes == null || this.ListMemberLotes.Lotes.Count == 0)
                    return;
                Form_Lote objFormLote = new Form_Lote(this.ListMemberLotes);
                if (objFormLote != null)
                {
                    objFormLote.FormClosing += delegate
                    {
                        if (objFormLote.IsLoteSelected)
                        {
                            if (Convert.ToInt64(objFormLote.LoteSelected.Value) > 0)
                            {
                                this.Selected_Lote = this.ListMemberLotes.GetLoteById(Convert.ToInt64(objFormLote.LoteSelected.Value));
                                this.Grid_Datasouce_Type = GridDatasourceType.MEMBER;
                                Load_Grid_Info(true, true, true, -1, -1);
                                this.LoadingGridPanelWaitTime = 500;
                            }
                        }
                        else
                            this.Grid_View.FocusedRowHandle = -1;
                    };
                    objFormLote.Show();
                    objFormLote.StartPosition = FormStartPosition.CenterParent;
                    objFormLote.Focus();
                    objFormLote.BringToFront();
                    objFormLote.Update();
                }
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>15-03-2018(GesAMFC-v1.0.0.3)</versions>
        private void Load_Lote_Add_Pagamentos()
        {
            try
            {
                if (this.ListMemberLotes == null || this.ListMemberLotes.Lotes.Count == 0)
                    return;
                Form_Lote objFormLote = new Form_Lote(this.ListMemberLotes);
                if (objFormLote != null)
                {
                    objFormLote.FormClosing += delegate
                    {
                        if (objFormLote.IsLoteSelected)
                        {
                            if (Convert.ToInt64(objFormLote.LoteSelected.Value) > 0)
                            {
                                this.Selected_Lote = this.ListMemberLotes.GetLoteById(Convert.ToInt64(objFormLote.LoteSelected.Value));
                                Button_Add_Action(this.Entidade_Member, this.Selected_Lote);
                            }
                        }
                    };
                    objFormLote.Show();
                    objFormLote.StartPosition = FormStartPosition.CenterParent;
                    objFormLote.Focus();
                    objFormLote.BringToFront();
                    objFormLote.Update();
                }
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>15-03-2018(GesAMFC-v1.0.0.3)</versions>
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

        /// <versions>01-12-2017(GesAMFC-v0.0.4.41)</versions>
        private void Load_Year_Month_Members()
        {
            try
            {
                Form_Year_Month objYearMonth = new Form_Year_Month(this.YearSelected, this.MonthSelected);
                if (objYearMonth  != null)
                {
                    objYearMonth.FormClosing += delegate
                    {
                        if (objYearMonth.IsYearMonthSelected)
                        {
                            if (Program.IsValidYear(objYearMonth.YearSelected.Value) && Program.IsValidMonth(objYearMonth.MonthSelected.Value))
                            {
                                if (objYearMonth.EveryYear)
                                {
                                    this.YearSelected = objYearMonth.YearSelected;
                                    this.Grid_Datasouce_Type = GridDatasourceType.YEAR;
                                }
                                else
                                {
                                    this.YearSelected = objYearMonth.YearSelected;
                                    this.MonthSelected = objYearMonth.MonthSelected;
                                    this.Grid_Datasouce_Type = GridDatasourceType.YEARMONTH;
                                }
                                this.Entidade_Member = new AMFCMember();
                                Load_Grid_Info(true, true, true, -1, -1);
                                this.LoadingGridPanelWaitTime = 500;
                            }
                        }
                        else
                            this.Grid_View.FocusedRowHandle = -1;
                    };
                    objYearMonth.Show();
                    objYearMonth.StartPosition = FormStartPosition.CenterParent;
                    objYearMonth.Focus();
                    objYearMonth.BringToFront();
                    objYearMonth.Update();
                }
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>02-12-2017(GesAMFC-v0.0.4.41)</versions>
        private void Load_All_Members()
        {
            try
            {
                this.Grid_Datasouce_Type = GridDatasourceType.ALL;
                Load_Grid_Info(true, true, true, -1, -1);
                this.LoadingGridPanelWaitTime = 500;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>03-12-2017(GesAMFC-v0.0.4.41)</versions>
        private void Load_Entity_Details(Int64 lId)
        {
            try
            {
                #region     Validate Member Number
                if (lId < 1)
                {
                    Clear_Details(true);
                    return;
                }
                #endregion  Validate Member Number

                #region     Get Focused Member
                PAG_ENTIDADE objEnt = Get_Selected(-1, lId);
                if (objEnt == null || objEnt.SOCIO < 1 || objEnt.SOCIO > new AMFCMember().MaxNumber || objEnt.ID < 1)
                {
                    Clear_Details(true);
                    XtraMessageBox.Show("Não foi possível obter os dados de " + this.Entidade_Configs.Entity_Desc_Short_Single + "!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                #endregion  Get Focused Member

                #region     Set Details Controls
                Set_Details_Editability(false);
                LayoutControlGroupAdmin.Visibility = LayoutVisibility.Always;
                LayoutControlGroupAdd.Visibility = LayoutVisibility.Never;
                LayoutControlGroupEdit.Visibility = LayoutVisibility.Never;
                #endregion  Set Details Controls

                #region     Load Details
                if (objEnt != null)
                {
                    if (!SetDetails(objEnt))
                    {
                        Clear_Details(true);
                        XtraMessageBox.Show("Não foi possível obter carregar detalhes de " + this.Entidade_Configs.Entity_Desc_Short_Single + "!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                #endregion  Load Details

                return;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>16-03-2018(GesAMFC-v1.0.0.3)</versions>
        private void Clear_Details(Boolean bClearId)
        {
            try
            {
                if (bClearId)
                    this.TextEdit_Id.Text = String.Empty;
                this.TextEdit_MemberNumber.Text = String.Empty;
                this.TextEdit_MemberName.Text = String.Empty;
                this.TextEdit_Area_Paga.Text = Program.Default_Area_String;

                if (this.Entidade_Configs.Entity_Value_Meter > 0)
                    this.TextEdit_Preco_Metro.Text = Program.SetPayCurrencyEuroStringValue(this.Entidade_Configs.Entity_Value_Meter);

                this.TextEdit_Value.Text = Program.Default_Pay_String;
                this.DateEdit_EntidadeData.DateTime = new DateTime();
                this.TextEdit_Notas.Text = String.Empty;

                LayoutControl_Details.Visible = false;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>08-10-2017(v0.0.4.8)</versions>
        private void Set_Details_Editability(Boolean bCanEdit)
        {
            try
            {
                Color cColorReadOnly = Color.FromArgb(207, 221, 238);
                Color cColorEditable = Color.White;

                Boolean bIsReadOnly = !bCanEdit;
                TextEdit_Id.Properties.ReadOnly = true; //always
                TextEdit_MemberNumber.Properties.ReadOnly = true; //always
                TextEdit_MemberName.Properties.ReadOnly = true; //always
                TextEdit_PAGTOTAL.Properties.ReadOnly = true; //always
                TextEdit_PAGNBR.Properties.ReadOnly = true; //always

                TextEdit_Value.Properties.ReadOnly = bIsReadOnly;
                TextEdit_Area_Paga.Properties.ReadOnly = bIsReadOnly;
                TextEdit_Preco_Metro.Properties.ReadOnly = bIsReadOnly;

                DateEdit_EntidadeData.Properties.ReadOnly = bIsReadOnly;
                TextEdit_Notas.Properties.ReadOnly = bIsReadOnly;

                if (bIsReadOnly)
                {
                    TextEdit_Value.BackColor = cColorReadOnly;
                    TextEdit_Area_Paga.BackColor = cColorReadOnly;
                    TextEdit_Preco_Metro.BackColor = cColorReadOnly;
                    DateEdit_EntidadeData.BackColor = cColorReadOnly;
                    TextEdit_Notas.BackColor = cColorReadOnly;
                }
                else
                {
                    TextEdit_Value.BackColor = cColorEditable;
                    TextEdit_Area_Paga.BackColor = cColorEditable;
                    TextEdit_Preco_Metro.BackColor = cColorEditable;
                    DateEdit_EntidadeData.BackColor = cColorEditable;
                    TextEdit_Notas.BackColor = cColorEditable;
                }

            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return;
            }
        }
        
        /// <versions>08-10-2017(v0.0.4.8)</versions>
        private Int32 CheckIsValid(PAG_ENTIDADE objEnt, Boolean bCheckId)
        {
            try
            {
                #region    ID
                if (bCheckId)
                {
                    if (objEnt.ID < 1)
                    {
                        String sError = "O " + "Id do " + "Pagamento " + " de " + this.Entidade_Configs.Entity_Desc_Short_Single + ": " + objEnt.ID + " não é válido!";
                        MessageBox.Show(sError, "Id Inválido", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return -1;
                    }
                }
                #endregion ID

                #region     Nº Sócio
                if (objEnt.SOCIO < 1 || objEnt.SOCIO > new AMFCMember().MaxNumber)
                {
                    String sError = "O " + "Nº de Sócio: " + objEnt.SOCIO + " não é válido! Por favor, modifique.";
                    MessageBox.Show(sError, "Sócio Inválido", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return -1;
                }
                #endregion  Nº Sócio

                #region     ID Lote
                if (objEnt.IDLOTE < 1)
                {
                    String sError = "O " + "ID do Lote: " + objEnt.IDLOTE + " não é válido! Por favor, modifique.";
                    MessageBox.Show(sError, "Lote Inválido", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return -1;
                }
                #endregion  ID Lote

                #region     Nome
                if (!Program.IsValidTextString(objEnt.NOME))
                {
                    String sWarning = "O " + "Nome do Sócio" + " (" + objEnt.NOME.Trim() + ") não é válido! Por favor, modifique.";
                    MessageBox.Show(sWarning, "Nome Sócio" + " Invalído", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return -1;
                }
                #endregion  Nome

                #region    Value
                if (objEnt.VALORPAGO <= 0)
                {
                    String sWarning = "O " + "valor do " + "Pagamento" + " de " + this.Entidade_Configs.Entity_Desc_Short_Single + " (" + objEnt.VALORPAGO.ToString() + ") não é válido! Por favor, modifique.";
                    MessageBox.Show(sWarning, "Valor Invalído", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return -1;
                }
                #endregion Value

                #region    Date Paid
                if (!Program.IsValidDateTime(objEnt.DATAPAG))
                {
                    String sWarning = "A " + "data de pagamento" + " (" + objEnt.DATAPAG.ToString() + ") não é válida! Por favor, modifique.";
                    MessageBox.Show(sWarning, "Data Pagamento" + " Invalída", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return -1;
                }
                #endregion Date Paid

                return 1;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return -1;
            }
        }

        /// <versions>16-03-2018(GesAMFC-v1.0.0.3)</versions>
        private PAG_ENTIDADE GetDetailsToAdd(AMFCMember objMember, AMFCMemberLote objLote)
        {
            try
            {
                #region     Get New ID
                Int64 lId = -1;
                Int32 iMaxId = DBF_AMFC_Members_GetMaxId();
                if (iMaxId < 0)
                    return null;
                lId = Convert.ToInt64(iMaxId) + 1;
                #endregion  Get New ID

                return Get_Entity_ToAdd(objMember, objLote, lId);
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return null;
            }
        }

        /// <versions>08-10-2017(v0.0.4.8)</versions>
        private void Edit_Action()
        {
            try
            {
                DialogResult objDialogResult = XtraMessageBox.Show("Deseja editar o " + "Pagamento" + " de " + this.Entidade_Configs.Entity_Desc_Short_Single + "?", "Editar" + " " + "Pagamento" + "?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (objDialogResult == DialogResult.Yes)
                    Button_Edit_Click();
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return;
            }
        }

        /// <versions>16-03-2018(GesAMFC-v1.0.0.3)</versions>
        private PAG_ENTIDADE GetDetailsToEdit()
        {
            try
            {
                PAG_ENTIDADE objEnt = new PAG_ENTIDADE();

                if (!String.IsNullOrEmpty(TextEdit_Id.Text.Trim()) && Convert.ToInt64(TextEdit_Id.Text.Trim()) > 0)
                    objEnt.ID = Convert.ToInt64(TextEdit_Id.Text.Trim());

                if (!String.IsNullOrEmpty(TextEdit_MemberNumber.Text.Trim()) && Convert.ToInt64(TextEdit_MemberNumber.Text.Trim()) > 0 && Convert.ToInt64(TextEdit_MemberNumber.Text.Trim()) < new AMFCMember().MaxNumber)
                    objEnt.SOCIO = Convert.ToInt64(TextEdit_MemberNumber.Text.Trim());

                if (Program.IsValidTextString(TextEdit_MemberName.Text))
                    objEnt.NOME = TextEdit_MemberName.Text.Trim();

                if (!String.IsNullOrEmpty(TextEdit_Lote_Id.Text.Trim()) && Convert.ToInt64(TextEdit_Lote_Id.Text.Trim()) > 0)
                    objEnt.IDLOTE = Convert.ToInt64(TextEdit_Lote_Id.Text.Trim());

                if (Program.IsValidTextString(TextEdit_Lote_Numero.Text))
                    objEnt.NUMLOTE = TextEdit_Lote_Numero.Text;

                if (!String.IsNullOrEmpty(TextEdit_PAGNBR.Text.Trim()) && Convert.ToInt32(TextEdit_PAGNBR.Text.Trim()) > 0)
                    objEnt.PAGNBR = Convert.ToInt32(TextEdit_PAGNBR.Text.Trim());

                if (!String.IsNullOrEmpty(TextEdit_PAGTOTAL.Text.Trim()) && Convert.ToInt32(TextEdit_PAGTOTAL.Text.Trim()) > 0)
                    objEnt.PAGTOTAL = Convert.ToInt32(TextEdit_PAGTOTAL.Text.Trim());

                if (Program.IsValidAreaValue(TextEdit_Area_Paga.Text.Trim()))
                    objEnt.AREAPAGO = Program.SetAreaDoubleValue(TextEdit_Area_Paga.Text.Trim());

                if (Program.IsValidCurrencyEuroValue(TextEdit_Preco_Metro.Text.Trim()))
                    objEnt.PRECOM2P = Program.SetPayCurrencyEuroDoubleValue(TextEdit_Preco_Metro.Text.Trim());
                
                if (Program.IsValidCurrencyEuroValue(TextEdit_Value.Text.Trim()))
                    objEnt.VALORPAGO = Program.SetPayCurrencyEuroDoubleValue(TextEdit_Value.Text.Trim());

                objEnt.DATAPAG = Program.SetDateTimeValue(DateEdit_EntidadeData.DateTime, -1, -1);
                objEnt.ANO = objEnt.DATAPAG.Year;
                objEnt.MES = objEnt.DATAPAG.Month;

                if (Program.IsValidTextString(TextEdit_Notas.Text))
                    objEnt.NOTAS = TextEdit_Notas.Text.Trim();

                if (Program.IsValidTextString(TextEdit_Hidden_Pay_Caixa_ListIDs.Text))
                    objEnt.LISTACAIXA = TextEdit_Hidden_Pay_Caixa_ListIDs.Text.Trim();

                return objEnt;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return null;
            }
        }

        /// <versions>08-10-2017(v0.0.4.8)</versions>
        private PAG_ENTIDADE GetDetailsEdited()
        {
            try
            {
                PAG_ENTIDADE objEnt = new PAG_ENTIDADE();

                if (Convert.ToInt64(TextEdit_Id.Text.Trim()) > 0)
                    objEnt.ID = Convert.ToInt64(TextEdit_Id.Text.Trim());

                if (Convert.ToInt64(TextEdit_MemberNumber.Text.Trim()) > 0 && Convert.ToInt64(TextEdit_MemberNumber.Text.Trim()) < new AMFCMember().MaxNumber)
                    objEnt.SOCIO = Convert.ToInt64(TextEdit_MemberNumber.Text.Trim());

                if (Program.IsValidTextString(TextEdit_MemberName.Text))
                    objEnt.NOME = TextEdit_MemberName.Text.Trim();

                if (!String.IsNullOrEmpty(TextEdit_Lote_Id.Text.Trim()) && Convert.ToInt64(TextEdit_Lote_Id.Text.Trim()) > 0)
                    objEnt.IDLOTE = Convert.ToInt64(TextEdit_Lote_Id.Text.Trim());

                if (Program.IsValidTextString(TextEdit_Lote_Numero.Text))
                    objEnt.NUMLOTE = TextEdit_Lote_Numero.Text;

                if (!String.IsNullOrEmpty(TextEdit_PAGNBR.Text.Trim()) && Convert.ToInt32(TextEdit_PAGNBR.Text.Trim()) > 0)
                    objEnt.PAGNBR = Convert.ToInt32(TextEdit_PAGNBR.Text.Trim());

                if (!String.IsNullOrEmpty(TextEdit_PAGTOTAL.Text.Trim()) && Convert.ToInt32(TextEdit_PAGTOTAL.Text.Trim()) > 0)
                    objEnt.PAGTOTAL = Convert.ToInt32(TextEdit_PAGTOTAL.Text.Trim());

                if (Program.IsValidAreaValue(TextEdit_Area_Paga.Text.Trim()))
                    objEnt.AREAPAGO = Program.SetAreaDoubleValue(TextEdit_Area_Paga.Text.Trim());

                if (Program.IsValidCurrencyEuroValue(TextEdit_Preco_Metro.Text.Trim()))
                    objEnt.PRECOM2P = Program.SetPayCurrencyEuroDoubleValue(TextEdit_Preco_Metro.Text.Trim());

                if (Program.IsValidCurrencyEuroValue(TextEdit_Value.Text.Trim()))
                    objEnt.VALORPAGO = Program.SetPayCurrencyEuroDoubleValue(TextEdit_Value.Text.Trim());

                objEnt.DATAPAG = Program.SetDateTimeValue(DateEdit_EntidadeData.DateTime, -1, -1);
                objEnt.ANO = objEnt.DATAPAG.Year;
                objEnt.MES = objEnt.DATAPAG.Month;

                if (Program.IsValidTextString(TextEdit_Notas.Text))
                    objEnt.NOTAS = TextEdit_Notas.Text.Trim();

                if (Program.IsValidTextString(TextEdit_Hidden_Pay_Caixa_ListIDs.Text))
                    objEnt.LISTACAIXA = TextEdit_Hidden_Pay_Caixa_ListIDs.Text.Trim();
                
                return objEnt;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return null;
            }
        }

        /// <versions>18-11-2017(GesAMFC-v0.0.4.33)</versions>
        private PAG_ENTIDADE GetDetailsEditedPayment()
        {
            try
            {
                PAG_ENTIDADE objEnt = new PAG_ENTIDADE();

                if (Convert.ToInt64(TextEdit_Id.Text.Trim()) > 0)
                    objEnt.ID = Convert.ToInt64(TextEdit_Id.Text.Trim());

                if (Convert.ToInt64(TextEdit_MemberNumber.Text.Trim()) > 0 && Convert.ToInt64(TextEdit_MemberNumber.Text.Trim()) < new AMFCMember().MaxNumber)
                    objEnt.SOCIO = Convert.ToInt64(TextEdit_MemberNumber.Text.Trim());

                if (Program.IsValidTextString(TextEdit_MemberName.Text))
                    objEnt.NOME = TextEdit_MemberName.Text.Trim();

                if (!String.IsNullOrEmpty(TextEdit_Lote_Id.Text.Trim()) && Convert.ToInt64(TextEdit_Lote_Id.Text.Trim()) > 0)
                    objEnt.IDLOTE = Convert.ToInt64(TextEdit_Lote_Id.Text.Trim());

                if (Program.IsValidTextString(TextEdit_Lote_Numero.Text))
                    objEnt.NUMLOTE = TextEdit_Lote_Numero.Text;

                if (!String.IsNullOrEmpty(TextEdit_PAGNBR.Text.Trim()) && Convert.ToInt32(TextEdit_PAGNBR.Text.Trim()) > 0)
                    objEnt.PAGNBR = Convert.ToInt32(TextEdit_PAGNBR.Text.Trim());

                if (!String.IsNullOrEmpty(TextEdit_PAGTOTAL.Text.Trim()) && Convert.ToInt32(TextEdit_PAGTOTAL.Text.Trim()) > 0)
                    objEnt.PAGTOTAL = Convert.ToInt32(TextEdit_PAGTOTAL.Text.Trim());

                if (Program.IsValidAreaValue(TextEdit_Area_Paga.Text.Trim()))
                    objEnt.AREAPAGO = Program.SetAreaDoubleValue(TextEdit_Area_Paga.Text.Trim());

                if (Program.IsValidCurrencyEuroValue(TextEdit_Preco_Metro.Text.Trim()))
                    objEnt.PRECOM2P = Program.SetPayCurrencyEuroDoubleValue(TextEdit_Preco_Metro.Text.Trim());

                if (Program.IsValidCurrencyEuroValue(TextEdit_Value.Text.Trim()))
                    objEnt.VALORPAGO = Program.SetPayCurrencyEuroDoubleValue(TextEdit_Value.Text.Trim());
                
                objEnt.DATAPAG = Program.SetDateTimeValue(DateEdit_EntidadeData.DateTime, -1, -1);
                objEnt.ANO = objEnt.DATAPAG.Year;
                objEnt.MES = objEnt.DATAPAG.Month;

                if (Program.IsValidTextString(TextEdit_Notas.Text))
                    objEnt.NOTAS = TextEdit_Notas.Text.Trim();

                if (Program.IsValidTextString(TextEdit_Hidden_Pay_Caixa_ListIDs.Text))
                    objEnt.LISTACAIXA = TextEdit_Hidden_Pay_Caixa_ListIDs.Text.Trim();

                return objEnt;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return null;
            }
        }

        /// <versions>18-11-2017(GesAMFC-v0.0.4.33)</versions>
        private void Edit_Payment()
        {
            try
            {
                Set_Details_Editability(false);
                LayoutControlGroupEdit.Visibility = LayoutVisibility.Never;
                LayoutControlGroupAdmin.Visibility = LayoutVisibility.Always; 

                #region     Member Info
                PAG_ENTIDADE objEnt = GetDetailsEditedPayment();
                if (objEnt == null)
                {
                    String sError = "Ocorreu um erro a obter os dados de " + "Pagamento" + " de " + this.Entidade_Configs.Entity_Desc_Short_Single + ". " + "Sócio: " + objEnt.NOME + " (Nº: " + objEnt.SOCIO + ")!";
                    MessageBox.Show(sError, "Erro Pagamento Sócio", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                #endregion  Member Info

                #region     Validate Data 
                if (CheckIsValid(objEnt, true) < 1)
                    return;
                #endregion  Validate Data

                #region     Edit Operation
                if (!DBF_AMFC_Edit(objEnt, true))
                    return;
                #endregion  Edit Operation

                this.Selected_Entidade = objEnt;
                Set_Details_Editability(false);
                LayoutControlGroupAdd.Visibility = LayoutVisibility.Never;
                LayoutControlGroupAdmin.Visibility = LayoutVisibility.Always;
                Load_Grid_Info(false, false, false, -1, objEnt.ID);
                Load_Entity_Details(objEnt.ID);
                this.Grid_View.Focus();
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return;
            }
        }

        /// <versions>16-03-2018(GesAMFC-v1.0.0.3)</versions>
        private Boolean SetDetails(PAG_ENTIDADE objEnt)
        {
            try
            {
                this.TextEdit_Id.Text = objEnt.ID.ToString();
                this.TextEdit_MemberNumber.Text = objEnt.SOCIO.ToString();
                this.TextEdit_MemberName.Text = Program.SetTextString(objEnt.NOME, Program.DefaultStringTextTypes.DEFAULT);

                TextEdit_Lote_Id.Text = objEnt.IDLOTE.ToString();
                TextEdit_Lote_Numero.Text = objEnt.NUMLOTE;

                TextEdit_PAGNBR.Text = objEnt.PAGNBR.ToString();
                TextEdit_PAGTOTAL.Text = objEnt.PAGTOTAL.ToString();

                this.TextEdit_Area_Paga.Text = Program.SetAreaDoubleStringValue(objEnt.AREAPAGO);
                this.TextEdit_Preco_Metro.Text = Program.SetPayCurrencyEuroStringValue(objEnt.PRECOM2P);
                this.TextEdit_Value.Text = Program.SetPayCurrencyEuroStringValue(objEnt.VALORPAGO);

                this.DateEdit_EntidadeData.DateTime = objEnt.DATAPAG;

                this.TextEdit_Notas.Text = objEnt.NOTAS;
                
                LayoutControlItem_Show_Pay.Visibility = (objEnt.VALORPAGO > 0) ? LayoutVisibility.Always : LayoutVisibility.Never;

                this.TextEdit_Hidden_Pay_Caixa_ListIDs.Text = objEnt.LISTACAIXA;

                return true;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return false;
            }
        }

        /// <versions>08-10-2017(v0.0.4.8)</versions>
        private void Reload_Details()
        {
            try
            {
                PAG_ENTIDADE objEnt = null;

                #region     Get Focused 
                if (this.Selected_Entidade != null && this.Selected_Entidade.ID > 1)
                {
                    if (this.Grid_View.FocusedRowHandle > 0)
                    {
                        objEnt = Get_Selected(this.Grid_View.FocusedRowHandle, -1);
                        if (objEnt == null || objEnt.SOCIO < 1 || objEnt.SOCIO > new AMFCMember().MaxNumber || objEnt.ID < 1)
                        {
                            Clear_Details(true);
                            XtraMessageBox.Show("Não foi possível obter o " + "Pagamento" + " de " + this.Entidade_Configs.Entity_Desc_Short_Single + " selecionado!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Load_Grid_Info(false, false, false, -1, -1);
                            return;
                        }
                    }
                }
                #endregion  Get Focused 

                #region     Load Details
                if (objEnt != null)
                {
                    if (!SetDetails(objEnt))
                    {
                        Clear_Details(true);
                        XtraMessageBox.Show("Não foi possível obter carregar detalhes do " + "Pagamento" + " de " + this.Entidade_Configs.Entity_Desc_Short_Single + "!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Load_Grid_Info(false, false, false, -1, -1);
                        return;
                    }
                }
                #endregion  Load Details
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return;
            }
        }

        #region     Add 

        /// <versions>07-10-2017(v0.0.4.8)</versions>
        private Int32 DBF_AMFC_Members_GetMaxId()
        {
            try
            {
                using (Library_AMFC_SQL obj_AMFC_SQL = new Library_AMFC_SQL())
                {
                    Int32 iMaxId = obj_AMFC_SQL.Get_ENTID_Max_Number(this.Entidade_Configs);
                    if (iMaxId < 0)
                    {
                        String sWarning = "Não foi possivel obter o número máximo do ID do Pagamento de " + this.Entidade_Configs.Entity_Desc_Short_Single + "! Por favor, contacte o programador!";
                        MessageBox.Show(sWarning, "Erro: ID Máximo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return -1;
                    }
                    return iMaxId;
                }
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return -1;
            }
        }
       
        /// <versions>15-03-2018(GesAMFC-v1.0.0.3)</versions>
        private void GetMemberToAddEntidade()
        {
            try
            {
                this.PreviousEntityState = new PAG_ENTIDADE();

                Find_Member_Form objFindMemberForm = new Find_Member_Form();
                if (objFindMemberForm != null)
                {
                    objFindMemberForm.FormClosing += delegate
                    {
                        if (Program.Member_Found) //trocar por objFindMemberForm.Member_Found e remover Program.Member_Found no futuro
                        {
                            AMFCMember objMemberSelected = objFindMemberForm.MemberSelected;
                            if (objMemberSelected != null && objMemberSelected.NUMERO > 0 && objMemberSelected.NUMERO < objMemberSelected.MaxNumber)
                            {
                                this.Entidade_Member = objMemberSelected;

                                Get_Member_Lotes_Add_Pagamentos();
                            }
                        }
                        else
                            this.Grid_View.FocusedRowHandle = -1;
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

        /// <versions>08-10-2017(v0.0.4.8)</versions>
        private void Button_Add_Click()
        {
            GetMemberToAddEntidade();
        }

        /// <versions>08-10-2017(v0.0.4.8)</versions>
        private void Button_Add_Action(AMFCMember objMember, AMFCMemberLote objLote)
        {
            try
            {
                #region     Get Member to Add
                if (objMember == null || objMember.NUMERO < 1 || objMember.NUMERO > objMember.MaxNumber)
                {
                    String sError = "Ocorreu um erro na obtenção do sócio!";
                    MessageBox.Show(sError, "Erro Obtenção Sócio", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                #endregion  Get Member to Add

                #region     Set Controls to Add
                Set_Details_Editability(true);
                LayoutControlGroupAdmin.Visibility = LayoutVisibility.Never;
                LayoutControlGroupAdd.Visibility = LayoutVisibility.Always;
                LayoutControlGroupEdit.Visibility = LayoutVisibility.Never;
                #endregion  Set Controls to Add

                #region     Get Member Info to Add
                PAG_ENTIDADE objEnt = GetDetailsToAdd(objMember, objLote);
                if (objEnt == null)
                {
                    String sError = "Ocorreu um erro a obter os dados do Sócio " + objEnt.NOME + " (Nº: " + objEnt.SOCIO + ")!";
                    MessageBox.Show(sError, "Erro Criação Sócio", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                #endregion  Get Member Info to Add

                #region     Set Member Info to Add
                if (!SetDetails(objEnt))
                {
                    Clear_Details(true);
                    XtraMessageBox.Show("Não foi possível obter carregar os detalhes do Pagamento da " + this.Entidade_Configs.Entity_Desc_Short_Single + "!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Load_Grid_Info(false, false, false, -1, -1);
                    return;
                }
                #endregion  Set Member Info to Add
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return;
            }
        }

        /// <versions>08-10-2017(v0.0.4.8)</versions>
        private void Button_Add_Save_Click()
        {
            try
            {
                #region     Member Info
                PAG_ENTIDADE objEnt = GetDetailsEdited();
                if (objEnt == null)
                {
                    String sError = "Ocorreu um erro a obter os dados do pagamento!";
                    MessageBox.Show(sError, "Erro Criação Sócio", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                #endregion  Member Info

                #region     Validate Data 
                if (CheckIsValid(objEnt, true) < 1)
                    return;
                #endregion  Validate Data

                #region     Add Operation
                if (!DBF_AMFC_Add(objEnt, true))
                    return;
                this.Selected_Entidade = objEnt;
                #endregion  Add Operation

                Set_Details_Editability(false);
                LayoutControlGroupAdd.Visibility = LayoutVisibility.Never;
                LayoutControlGroupAdmin.Visibility = LayoutVisibility.Always;

                if (objEnt.VALORPAGO > 0)
                    AddPay();
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return;
            }
        }

        /// <versions>06-10-2017(v0.0.4.8)</versions>
        private void Button_Add_Cancel_Click()
        {
            try
            {
                Clear_Details(true);
                Set_Details_Editability(false);
                LayoutControlGroupAdd.Visibility = LayoutVisibility.Never;
                LayoutControlGroupAdmin.Visibility = LayoutVisibility.Always;
                Load_Grid_Info(false, false, false, 0, -1);
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return;
            }
        }

        /// <versions>06-10-2017(v0.0.4.8)</versions>
        private void Button_Add_Repor_Click()
        {
            try
            {
                if (this.Entidade_Member != null && this.Entidade_Member.NUMERO > 0 && this.Entidade_Member.NUMERO < this.Entidade_Member.MaxNumber && this.Selected_Lote.IDLOTE > 0)
                    Button_Add_Action(this.Entidade_Member, this.Selected_Lote);
                else 
                    Clear_Details(false);
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return;
            }
        }

        /// <versions>08-10-2017(v0.0.4.8)</versions>
        private Boolean DBF_AMFC_Add(PAG_ENTIDADE objEnt, Boolean bShowMessage)
        {
            try
            {
                Int64 lOpStatus = -1;
                using (Library_AMFC_SQL obj_AMFC_SQL = new Library_AMFC_SQL())
                {
                    lOpStatus = obj_AMFC_SQL.Add_ENTID(this.Entidade_Configs, objEnt, bShowMessage);
                    if (lOpStatus == 1)
                    {
                        if (bShowMessage)
                        {
                            String sInfo = "Pagamento" + " de " + this.Entidade_Configs.Entity_Desc_Short_Single + " " + "do Sócio " + objEnt.SOCIO + " (Nº: " + objEnt.SOCIO + ") adicionado com sucesso.";
                            MessageBox.Show(sInfo, "Pagamneto" + " " + "adicionado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        return true;
                    }
                    else if (lOpStatus == -1)
                    {
                        if (bShowMessage)
                        {
                            String sError = "Ocorreu um erro na introdução do " + "Pagamento" + " de " + this.Entidade_Configs.Entity_Desc_Short_Single + " do Sócio " + objEnt.NOME + " (Nº: " + objEnt.SOCIO + ")!";
                            MessageBox.Show(sError, "Erro Criação Sócio", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            if (!SetDetails(objEnt))
                            {
                                Clear_Details(true);
                                XtraMessageBox.Show("Não foi possível obter carregar detalhes do " + "Pagamento" + " de " + this.Entidade_Configs.Entity_Desc_Short_Single + "!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                Load_Grid_Info(false, false, false, 0, -1);
                            }
                        }
                        return false;
                    }
                    else if (lOpStatus == 0)
                    {
                        if (!SetDetails(objEnt))
                        {
                            if (bShowMessage)
                            {
                                Clear_Details(true);
                                XtraMessageBox.Show("Erro a carregar os detalhes do " + "Pagamento" + " de " + this.Entidade_Configs.Entity_Desc_Short_Single, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                Load_Grid_Info(false, false, false, 0, -1);
                            }
                        }
                        return false; //Ja existe
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

        #endregion  Add 

        #region     Edit 

        /// <versions>08-10-2017(v0.0.4.9)</versions>
        private void Button_Edit_Click()
        {
            try
            {
                #region     Check Selected 
                if (this.Grid_View.FocusedRowHandle < 0)
                {
                    String sInfo = "Selecione um " + "Pagamento" + " de " + this.Entidade_Configs.Entity_Desc_Short_Single + " na grelha para editar!";
                    MessageBox.Show(sInfo, "Pagamento" + " de " + this.Entidade_Configs.Entity_Desc_Short_Single + " não selecionado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                #endregion  Check Selected 

                #region     Set Controls to Edit
                Set_Details_Editability(true);
                LayoutControlGroupAdmin.Visibility = LayoutVisibility.Never;
                LayoutControlGroupAdd.Visibility = LayoutVisibility.Never;
                LayoutControlGroupEdit.Visibility = LayoutVisibility.Always;
                #endregion  Set Controls to Edit

                #region     Get Member Info to Edit
                PAG_ENTIDADE objEnt = GetDetailsToEdit();
                if (objEnt == null)
                {
                    String sError = "Ocorreu um erro a obter os dados do " + "Pagamento" + " de " + this.Entidade_Configs.Entity_Desc_Short_Single + " do Sócio " + objEnt.NOME + " (Nº: " + objEnt.SOCIO + ")!";
                    MessageBox.Show(sError, "Erro Criação Sócio", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                #endregion  Get Member Info to Edit

                #region     Set Member Info to Edit
                if (!SetDetails(objEnt))
                {
                    Clear_Details(true);
                    XtraMessageBox.Show("Não foi possível obter carregar detalhes do " + "Pagamento" + " de " + this.Entidade_Configs.Entity_Desc_Short_Single + "!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Load_Grid_Info(false, false, false, -1, -1);
                    return;
                }
                #endregion  Set Member Info to Edit
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return;
            }
        }

        /// <versions>08-10-2017(v0.0.4.9)</versions>
        private void Button_Edit_Save_Click()
        {
            try
            {
                Set_Details_Editability(false);
                LayoutControlGroupEdit.Visibility = LayoutVisibility.Never;
                LayoutControlGroupAdmin.Visibility = LayoutVisibility.Always;


                #region     Member Info
                PAG_ENTIDADE objEnt = GetDetailsEdited();
                if (objEnt == null)
                {
                    String sError = "Ocorreu um erro a obter os dados do " + "Pagamento" + " de " + this.Entidade_Configs.Entity_Desc_Short_Single + " do Sócio " + objEnt.NOME + " (Nº: " + objEnt.SOCIO + ")!";
                    MessageBox.Show(sError, "Erro Criação Sócio", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                #endregion  Member Info

                #region     Validate Data 
                if (CheckIsValid(objEnt, true) < 1)
                    return;
                #endregion  Validate Data

                #region     Edit Operation
                if (!DBF_AMFC_Edit(objEnt, true))
                    return;
                #endregion  Edit Operation

                this.Selected_Entidade = objEnt;
                Set_Details_Editability(false);
                LayoutControlGroupAdd.Visibility = LayoutVisibility.Never;
                LayoutControlGroupAdmin.Visibility = LayoutVisibility.Always;

                if (objEnt.VALORPAGO > 0 && this.PreviousEntityState.VALORPAGO > 0 && this.PreviousEntityState.SOCIO > 0 && objEnt.SOCIO == this.PreviousEntityState.SOCIO && this.PreviousEntityState.VALORPAGO > 0 && this.PreviousEntityState.VALORPAGO != objEnt.VALORPAGO)
                    EditPay();
                else
                {
                    #region     Reload Grid
                    this.Selected_Entidade = objEnt;
                    Set_Details_Editability(false);
                    LayoutControlGroupAdd.Visibility = LayoutVisibility.Never;
                    LayoutControlGroupAdmin.Visibility = LayoutVisibility.Always;
                    Load_Grid_Info(true, true, true, -1, objEnt.ID);
                    Load_Entity_Details(objEnt.ID);
                    this.Grid_View.Focus();
                    #endregion  Reload Grid
                }
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return;
            }
        }

        /// <versions>08-10-2017(v0.0.4.9)</versions>
        private void Button_Edit_Cancel_Click()
        {
            try
            {
                Reload_Details();
                Set_Details_Editability(false);
                LayoutControlGroupEdit.Visibility = LayoutVisibility.Never;
                LayoutControlGroupAdmin.Visibility = LayoutVisibility.Always;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return;
            }
        }

        /// <versions>08-10-2017(v0.0.4.9)</versions>
        private void Button_Edit_Repor_Click()
        {
            Reload_Details();
        }

        /// <versions>08-10-2017(v0.0.4.9)</versions>
        private Boolean DBF_AMFC_Edit(PAG_ENTIDADE objEnt, Boolean bShowMessageDialog)
        {
            try
            {
                Int64 lOpStatus = -1;
                using (Library_AMFC_SQL obj_AMFC_SQL = new Library_AMFC_SQL())
                    lOpStatus = obj_AMFC_SQL.Edit_ENTID(this.Entidade_Configs, objEnt);
                if (lOpStatus == 1 && bShowMessageDialog)
                {
                    String sInfo = "Pagamento" + " de " + this.Entidade_Configs.Entity_Desc_Short_Single + " do Sócio " + objEnt.NOME + " (Nº: " + objEnt.SOCIO + ") alterado com sucesso.";
                    MessageBox.Show(sInfo, "Pagamento" + " de " + this.Entidade_Configs.Entity_Desc_Short_Single + " " + "editado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return true;
                }
                else if (lOpStatus == -1)
                {
                    String sError = "Ocorreu um erro na alteração do " + "Pagamento" + " de " + this.Entidade_Configs.Entity_Desc_Short_Single + " do Sócio " + objEnt.NOME + " (Nº: " + objEnt.SOCIO + ")!";
                    MessageBox.Show(sError, "Erro Criação Sócio", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    if (!SetDetails(objEnt))
                    {
                        Clear_Details(true);
                        XtraMessageBox.Show("Não foi possível obter carregar detalhes do " + "Pagamento" + " de " + this.Entidade_Configs.Entity_Desc_Short_Single + "!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Load_Grid_Info(false, false, false, 0, -1);
                    }
                    return false;
                }
                else if (lOpStatus == 0)
                {
                    if (!SetDetails(objEnt))
                    {
                        Clear_Details(true);
                        XtraMessageBox.Show("Erro a carregar os detalhes do " + "Pagamento" + " de " + this.Entidade_Configs.Entity_Desc_Short_Single, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Load_Grid_Info(false, false, false, 0, -1);
                    }
                    return false; //Ja existe
                }
                return true;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return false;
            }
        }

        #endregion  Edit 

        #region     Del 

        /// <versions>08-10-2017(v0.0.4.8)</versions>
        private void Button_Del_Click()
        {
            try
            {
                #region     Del Confirmation
                DialogResult objDialogResult = XtraMessageBox.Show("Deseja eliminar o " + "Pagamento" + " de " + this.Entidade_Configs.Entity_Desc_Short_Single + "?", "Eliminar" + " " + "Pagamento" + " ?", MessageBoxButtons.OK, MessageBoxIcon.Question);
                if (objDialogResult != DialogResult.OK)
                    return;
                #endregion Del Confirmation

                #region     Check Selected 
                if (this.Grid_View.FocusedRowHandle < 0)
                {
                    String sInfo = "Selecione um " + "Pagamento" + " de " + this.Entidade_Configs.Entity_Desc_Short_Single + " na grelha para eliminar!";
                    MessageBox.Show(sInfo, "Pagamento" + " de " + this.Entidade_Configs.Entity_Desc_Short_Single + " não selecionado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                #endregion  Check Selected 

                #region     Get Member ID
                PAG_ENTIDADE objEnt = GetDetailsToEdit();
                if (objEnt == null)
                {
                    String sError = "Ocorreu um erro a obter os dados do " + "Pagamento" + " de " + this.Entidade_Configs.Entity_Desc_Short_Single + " do Sócio " + objEnt.NOME + " (Nº: " + objEnt.SOCIO + ")!";
                    MessageBox.Show(sError, "Erro Criação Sócio", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (objEnt.ID < 1)
                {
                    String sError = "ID" + " do" + "Pagamento" + " de " + this.Entidade_Configs.Entity_Desc_Short_Single + " inválido: " + objEnt.ID;
                    MessageBox.Show(sError, "ID Inválido", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                #endregion  Get Member ID

                #region     Delete Operation
                if (!DBF_AMFC_Del(objEnt))
                {
                    String sError = "ID" + " do "+ "Pagamento" + " de " + this.Entidade_Configs.Entity_Desc_Short_Single + " inválido: " + objEnt.ID;
                    MessageBox.Show(sError, "ID Inválido", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        /// <versions>08-10-2017(v0.0.4.8)</versions>
        private Boolean DBF_AMFC_Del(PAG_ENTIDADE objEnt)
        {
            try
            {
                Int64 lOpStatus = -1;
                using (Library_AMFC_SQL obj_AMFC_SQL = new Library_AMFC_SQL())
                    lOpStatus = obj_AMFC_SQL.Del_ENTID(this.Entidade_Configs, objEnt);
                if (lOpStatus == 1)
                {
                    String sInfo = "Pagamento" + " de " + this.Entidade_Configs.Entity_Desc_Short_Single + " = " + objEnt.ID + " eliminado com sucesso.";
                    MessageBox.Show(sInfo, "Pagamento" + " " + "Eliminado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Grid_Datasouce_Type = GridDatasourceType.MEMBER;
                    Load_Grid_Info(true, true, true, 0, -1);
                    return true;
                }
                else
                {
                    String sError = "Ocorreu um erro na eliminação do " + "Pagamento" + " de " + this.Entidade_Configs.Entity_Desc_Short_Single + " ID = " + objEnt.ID + "!";
                    MessageBox.Show(sError, "Erro Eliminação ", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Clear_Details(true);
                    Load_Grid_Info(false, false, false, 0, -1);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return false;
            }
        }

        #endregion  Del 

        #region     Pay 

        /// <versions>12-11-2017(GesAMFC-v0.0.4.32)</versions>
        private AMFCCashPayment GetPayDetails(PAG_ENTIDADE objEntidade)
        {
            try
            {
                if (objEntidade == null)
                    return null;

                AMFCCashPayment objMemberPay = new AMFCCashPayment();

                objMemberPay.Payment_Type = this.Entidade_Configs.Pagamento_Tipo;

                #region     Get Pay Max ID
                Int32 iPayMaxId = DBF_AMFC_Members_GetMaxPayId();
                if (iPayMaxId < 1)
                    return null;
                #endregion  Get Pay Max ID

                objMemberPay.ID = Convert.ToInt64(iPayMaxId) + 1;
                if (objEntidade.SOCIO > 0 && objEntidade.SOCIO < new AMFCMember().MaxNumber)
                    objMemberPay.SOCIO = objEntidade.SOCIO;
                if (Program.IsValidTextString(objEntidade.NOME))
                    objMemberPay.NOME = objEntidade.NOME.Trim();

                Double dValor = Program.SetPayCurrencyEuroDoubleValue(objEntidade.VALORPAGO);
                objMemberPay.VALOR = dValor;

                objMemberPay.ALTERADO = Program.SetDateTimeValue(DateTime.Today, -1, -1);
                objMemberPay.DATA = Program.SetDateTimeValue(DateTime.Today, -1, -1);

                objMemberPay.DESIGNACAO = "Pagamento de " + this.Entidade_Configs.Entity_Desc_Plural;

                objMemberPay.Payment_State = AMFCCashPayment.PaymentState.ABERTO;

                #region     Payment Entity
                switch (this.Entidade_Configs.Pagamento_Tipo)
                {
                    case AMFCCashPayment.PaymentTypes.INFRAS:
                        objMemberPay.HasINFRAEST = true;
                        objMemberPay.INFRADESC = "Prestação de " + this.Entidade_Configs.Entity_Desc_Plural + " " + "Nº: " + objEntidade.PAGNBR;
                        objMemberPay.INFRAVAL = dValor;
                        objMemberPay.DASSOCINFR = objMemberPay.DATA;
                        break;
                    case AMFCCashPayment.PaymentTypes.CEDENC:
                        objMemberPay.HasCEDENCIAS = true;
                        objMemberPay.CEDENCDESC = "Prestação de " + this.Entidade_Configs.Entity_Desc_Plural + " " + "Nº: " + objEntidade.PAGNBR;
                        objMemberPay.CEDENCVAL = dValor;
                        objMemberPay.DASSOCCEDE = objMemberPay.DATA;
                        break;
                    case AMFCCashPayment.PaymentTypes.ESGOT:
                        objMemberPay.HasESGOT = true;
                        objMemberPay.ESGOTDESC = "Prestação de " + this.Entidade_Configs.Entity_Desc_Plural + " " + "Nº: " + objEntidade.PAGNBR;
                        objMemberPay.ESGOTVAL = dValor;
                        objMemberPay.DASSOCESGO = objMemberPay.DATA;
                        break;
                    case AMFCCashPayment.PaymentTypes.RECONV:
                        objMemberPay.HasRECONV = true;
                        objMemberPay.RECONDESC = "Prestação de " + this.Entidade_Configs.Entity_Desc_Plural + " " + "Nº: " + objEntidade.PAGNBR;
                        objMemberPay.RECONVAL = dValor;
                        objMemberPay.DASSOCRECO = objMemberPay.DATA;
                        break;
                }
                #endregion  Payment Entity

                objMemberPay.NOTAS += ". " + objEntidade.NOTAS;

                return objMemberPay;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return null;
            }
        }

        /// <versions>07-10-2017(v0.0.4.8)</versions>
        private Int32 DBF_AMFC_Members_GetMaxPayId()
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

        /// <versions>19-03-2018(GesAMFC-v1.0.0.3)</versions>
        private void AddPay()
        {
            try
            {
                using (Library_AMFC_SQL obj_AMFC_SQL = new Library_AMFC_SQL())
                {
                    Form_Caixa objForm_Caixa = new Form_Caixa();
                    if (objForm_Caixa != null)
                    {
                        PAG_ENTIDADE objEntidade = GetDetailsEdited();
                        if (objEntidade == null || objEntidade.SOCIO < 1 || objEntidade.SOCIO > new AMFCMember().MaxNumber || objEntidade.ID < 1)
                            return;

                        this.Entidade_Member = Get_DBF_AMFC_Member_By_Number(objEntidade.SOCIO);
                        if (this.Entidade_Member == null || this.Entidade_Member.NUMERO <= 0 || this.Entidade_Member.NUMERO > this.Entidade_Member.MaxNumber)
                            return;
                        objForm_Caixa.PaymentMember = this.Entidade_Member;

                        AMFCCashPayment objPayment = new AMFCCashPayment();
                        if (obj_AMFC_SQL.Member_Payment_Open_Already_Exist(this.Entidade_Member.NUMERO)) //Pagamento em aberto
                        {
                            objPayment = obj_AMFC_SQL.Get_Member_Payment_Open(this.Entidade_Member.NUMERO);
                            if (objPayment == null || objPayment.ID < 1)
                                return;

                            objPayment = SetEntityPayDetails(objPayment, objEntidade.VALORPAGO, objEntidade);
                            if (objPayment == null || objPayment.ID < 1)
                                return;

                            objPayment.ALTERADO = Program.SetDateTimeValue(DateTime.Today, -1, -1);
                            objPayment.Payment_State = AMFCCashPayment.PaymentState.FINALIZADO;
                            objForm_Caixa.PaymentOperationType = Library_AMFC_Methods.OperationType.EDIT;

                            objForm_Caixa.CurrentPayment = objPayment;
                            objForm_Caixa.EntitiesPaymentType = objForm_Caixa.GetMultipleSinglePaymentType();
                        }
                        else
                        {
                            objPayment = GetPayDetails(objEntidade); //Novo pagamento
                            if (objPayment == null || objPayment.ID < 1)
                                return;

                            objPayment = SetEntityPayDetails(objPayment, objEntidade.VALORPAGO, objEntidade);
                            if (objPayment == null || objPayment.ID < 1)
                                return;

                            objPayment.Payment_State = AMFCCashPayment.PaymentState.FINALIZADO;
                            objForm_Caixa.PaymentOperationType = Library_AMFC_Methods.OperationType.ADD;
                            objForm_Caixa.CurrentPayment = objPayment;
                            objForm_Caixa.EntitiesPaymentType = objForm_Caixa.GetMultipleSinglePaymentType();
                        }
                        objForm_Caixa.FormClosing += delegate
                        {
                            if (objForm_Caixa.PaymentOk)
                            {
                                try
                                {
                                    #region     Update Details
                                    objEntidade = Get_Pay_Details(objEntidade);
                                    if (objForm_Caixa.CurrentPayment.ID > 0)
                                        objEntidade.AddCaixaId(objForm_Caixa.CurrentPayment.ID);
                                    if (objForm_Caixa.CurrentPayment.Payment_State == AMFCCashPayment.PaymentState.FINALIZADO)
                                        objEntidade.Pay_State = PAG_ENTIDADE.PayState.SIM;
                                    else
                                        objEntidade.Pay_State = PAG_ENTIDADE.PayState.EM_PAGAMENTO;
                                    if (objEntidade == null)
                                        return;
                                    this.Selected_Entidade = objEntidade; //Tem de estar antes do Pay_Edit_DB
                                    Pay_Edit_DB(objEntidade, false, false);
                                    #endregion  Update Details

                                    #region     Reload Grid
                                    this.Selected_Entidade = objEntidade;
                                    Set_Details_Editability(false);
                                    LayoutControlGroupAdd.Visibility = LayoutVisibility.Never;
                                    LayoutControlGroupAdmin.Visibility = LayoutVisibility.Always;
                                    Load_Grid_Info(true, true, true, -1, objEntidade.ID);
                                    Load_Entity_Details(objEntidade.ID);
                                    this.Grid_View.Focus();
                                    #endregion  Reload Grid
                                }
                                catch { }
                            }
                            else //Delete Payment or Error 
                            {
                                try
                                {
                                    if (objForm_Caixa.CurrentPayment.Payment_State == AMFCCashPayment.PaymentState.CANCELED)
                                    {
                                        //Delete Pagamento
                                    }
                                }
                                catch { }
                            }
                        };

                        objForm_Caixa.Show();
                        objForm_Caixa.StartPosition = FormStartPosition.CenterParent;
                        objForm_Caixa.Focus();
                        objForm_Caixa.BringToFront();
                        objForm_Caixa.Update();
                    }
                }
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, false, false);
            }
        }

        /// <versions>19-03-2018(GesAMFC-v1.0.0.3)</versions>
        private void EditPay()
        {
            try
            {
                using (Library_AMFC_SQL obj_AMFC_SQL = new Library_AMFC_SQL())
                {
                    Form_Caixa objForm_Caixa = new Form_Caixa();
                    if (objForm_Caixa != null)
                    {
                        PAG_ENTIDADE objEntidade = GetDetailsEdited();
                        if (objEntidade == null || objEntidade.SOCIO < 1 || objEntidade.SOCIO > new AMFCMember().MaxNumber || objEntidade.ID < 1)
                            return;

                        this.Entidade_Member = Get_DBF_AMFC_Member_By_Number(objEntidade.SOCIO);
                        if (this.Entidade_Member == null || this.Entidade_Member.NUMERO <= 0 || this.Entidade_Member.NUMERO > this.Entidade_Member.MaxNumber)
                            return;
                        objForm_Caixa.PaymentMember = this.Entidade_Member;

                        AMFCCashPayment objPayment = new AMFCCashPayment();
                        if (obj_AMFC_SQL.Member_Payment_Open_Already_Exist(this.Entidade_Member.NUMERO)) //Pagamento em aberto
                        {
                            objPayment = obj_AMFC_SQL.Get_Member_Payment_Open(this.Entidade_Member.NUMERO);
                            if (objPayment == null || objPayment.ID < 1)
                                return;

                            objPayment = SetEntityPayDetails(objPayment, objEntidade.VALORPAGO, objEntidade);
                            if (objPayment == null || objPayment.ID < 1)
                                return;

                            objPayment.ALTERADO = Program.SetDateTimeValue(DateTime.Today, -1, -1);
                            objPayment.Payment_State = AMFCCashPayment.PaymentState.FINALIZADO;
                            objForm_Caixa.PaymentOperationType = Library_AMFC_Methods.OperationType.EDIT;
                            objForm_Caixa.CurrentPayment = objPayment;
                            objForm_Caixa.EntitiesPaymentType = objForm_Caixa.GetMultipleSinglePaymentType();
                        }
                        else
                        {
                            objPayment = Get_Entity_Payment();
                            if (objPayment == null || objPayment.ID < 1)
                                return;

                            objPayment = SetEntityPayDetails(objPayment, objEntidade.VALORPAGO, objEntidade);
                            if (objPayment == null || objPayment.ID < 1)
                                return;

                            objPayment.Payment_State = AMFCCashPayment.PaymentState.FINALIZADO;
                            objForm_Caixa.PaymentOperationType = Library_AMFC_Methods.OperationType.EDIT;
                            objForm_Caixa.CurrentPayment = objPayment;
                            objForm_Caixa.EntitiesPaymentType = objForm_Caixa.GetMultipleSinglePaymentType();
                        }
                        objForm_Caixa.FormClosing += delegate
                        {
                            if (objForm_Caixa.PaymentOk)
                            {
                                try
                                {
                                    #region     Update Details
                                    objEntidade = Get_Pay_Details(objEntidade);
                                    if (objForm_Caixa.CurrentPayment.ID > 0)
                                        objEntidade.AddCaixaId(objForm_Caixa.CurrentPayment.ID);
                                    if (objForm_Caixa.CurrentPayment.Payment_State == AMFCCashPayment.PaymentState.FINALIZADO)
                                        objEntidade.Pay_State = PAG_ENTIDADE.PayState.SIM;
                                    else
                                        objEntidade.Pay_State = PAG_ENTIDADE.PayState.EM_PAGAMENTO;
                                    if (objEntidade == null)
                                        return;
                                    Pay_Edit_DB(objEntidade, false, false);  //Tem de estar antes do Pay_Edit_DB
                                    this.Selected_Entidade = objEntidade;
                                    #endregion  Update Details

                                    #region     Reload Grid
                                    this.Selected_Entidade = objEntidade;
                                    Set_Details_Editability(false);
                                    LayoutControlGroupAdd.Visibility = LayoutVisibility.Never;
                                    LayoutControlGroupAdmin.Visibility = LayoutVisibility.Always;
                                    Load_Grid_Info(true, true, true, -1, objEntidade.ID);
                                    Load_Entity_Details(objEntidade.ID);
                                    this.Grid_View.Focus();
                                    #endregion  Reload Grid
                                }
                                catch { }
                            }
                            else //Delete Payment or Error 
                            {
                                try
                                {
                                    if (objForm_Caixa.CurrentPayment.Payment_State == AMFCCashPayment.PaymentState.CANCELED)
                                    {
                                        //Delete Pagamento
                                    }
                                }
                                catch { }
                            }
                        };
                        objForm_Caixa.Show();
                        objForm_Caixa.StartPosition = FormStartPosition.CenterParent;
                        objForm_Caixa.Focus();
                        objForm_Caixa.BringToFront();
                        objForm_Caixa.Update();
                    }
                }
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, false, false);
            }
        }

        private AMFCCashPayment SetEntityPayDetails(AMFCCashPayment objPayment, Double dbValor, PAG_ENTIDADE objEntidade)
        {
            try
            {
                objPayment.Payment_Type = this.Entidade_Configs.Pagamento_Tipo;
                switch (this.Entidade_Configs.Pagamento_Tipo)
                {
                    case AMFCCashPayment.PaymentTypes.INFRAS:
                        objPayment.INFRADESC = "Prestação de " + this.Entidade_Configs.Entity_Desc_Plural + " " + "Nº: " + objEntidade.PAGNBR;
                        objPayment.INFRAVAL = dbValor;
                        objPayment.SetItemPayment(this.Entidade_Configs.Pagamento_Tipo, Program.SetPayCurrencyEuroDoubleValue(objPayment.INFRAVAL), true, false);
                        objPayment.HasINFRAEST = true;
                        objPayment.DASSOCINFR = Program.SetDateTimeValue(DateTime.Today, -1, -1);
                        break;
                    case AMFCCashPayment.PaymentTypes.CEDENC:
                        objPayment.CEDENCDESC = "Prestação de " + this.Entidade_Configs.Entity_Desc_Plural + " " + "Nº: " + objEntidade.PAGNBR;
                        objPayment.CEDENCVAL = dbValor;
                        objPayment.SetItemPayment(this.Entidade_Configs.Pagamento_Tipo, Program.SetPayCurrencyEuroDoubleValue(objPayment.CEDENCVAL), true, false);
                        objPayment.HasCEDENCIAS = true;
                        objPayment.DASSOCCEDE = Program.SetDateTimeValue(DateTime.Today, -1, -1);
                        break;
                    case AMFCCashPayment.PaymentTypes.ESGOT:
                        objPayment.ESGOTDESC = "Prestação de " + this.Entidade_Configs.Entity_Desc_Plural + " " + "Nº: " + objEntidade.PAGNBR;
                        objPayment.ESGOTVAL = dbValor;
                        objPayment.SetItemPayment(this.Entidade_Configs.Pagamento_Tipo, Program.SetPayCurrencyEuroDoubleValue(objPayment.ESGOTVAL), true, false);
                        objPayment.HasESGOT = true;
                        objPayment.DASSOCESGO = Program.SetDateTimeValue(DateTime.Today, -1, -1);
                        break;
                    case AMFCCashPayment.PaymentTypes.RECONV:
                        objPayment.RECONDESC = "Prestação de " + this.Entidade_Configs.Entity_Desc_Plural + " " + "Nº: " + objEntidade.PAGNBR;
                        objPayment.RECONVAL = dbValor;
                        objPayment.SetItemPayment(this.Entidade_Configs.Pagamento_Tipo, Program.SetPayCurrencyEuroDoubleValue(objPayment.RECONVAL), true, false);
                        objPayment.HasRECONV = true;
                        objPayment.DASSOCRECO = Program.SetDateTimeValue(DateTime.Today, -1, -1);
                        break;
                }

                return objPayment;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, false, false);
                return null;
            }
        }

        /// <versions>26-11-2017(GesAMFC-v0.0.4.37)</versions>
        private Int64 Get_Pay_Last_Id(PAG_ENTIDADE objEntidade)
        {
            try
            {
                Int64 lPayId = -1;
                if (objEntidade.ListaCaixaIDs.Count == 0)
                    return -1;
                lPayId = objEntidade.GetLastCaixaId();
                return lPayId;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, false, false);
                return -1;
            }
        }

        /// <versions>06-12-2017(GesAMFC-v0.0.5.0)</versions>
        private void Open()
        {
            try
            {
                AMFCCashPayment objPayment = Get_Entity_Payment();
                if (objPayment == null || objPayment.ID < 1)
                    return;

                this.Entidade_Member = Get_DBF_AMFC_Member_By_Number(objPayment.SOCIO);
                if (this.Entidade_Member == null || this.Entidade_Member.NUMERO <= 0 || this.Entidade_Member.NUMERO > this.Entidade_Member.MaxNumber)
                    return;

                Form_Caixa objForm_Caixa = new Form_Caixa();
                if (objForm_Caixa != null)
                {                      
                    objForm_Caixa.PaymentMember = this.Entidade_Member;
                    objForm_Caixa.CurrentPayment = objPayment;
                    objForm_Caixa.PaymentOperationType = Library_AMFC_Methods.OperationType.OPEN;
                    objForm_Caixa.EntitiesPaymentType = objForm_Caixa.GetMultipleSinglePaymentType();
                    objForm_Caixa.FormClosing += delegate
                    {
                        //...
                    };
                    objForm_Caixa.Show();
                    objForm_Caixa.StartPosition = FormStartPosition.CenterParent;
                    objForm_Caixa.Focus();
                    objForm_Caixa.BringToFront();
                    objForm_Caixa.Update();
                }
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, false, false);
            }
        }

        /// <versions>06-12-2017(GesAMFC-v0.0.5.0)</versions>
        private AMFCCashPayment Get_Entity_Payment()
        {
            try
            {
                AMFCCashPayment objPayment = new AMFCCashPayment();

                Int64 lPayId = -1;
                PAG_ENTIDADE objEntidade = GetDetailsEdited();
                if (objEntidade == null || objEntidade.SOCIO < 1 || objEntidade.SOCIO > new AMFCMember().MaxNumber || objEntidade.ID < 1)
                    return null;

                if (objEntidade.ListaCaixaIDs.Count == 0)
                {
                    String sWarning = "Não foi possivel obter o ID do Pagamento!";
                    MessageBox.Show(sWarning, "Erro: Nº de " + "Pagamento", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
                lPayId = Get_Pay_Last_Id(objEntidade);
                if (lPayId < 1)
                {
                    String sWarning = "Não foi possivel obter o Pagamento!";
                    MessageBox.Show(sWarning, "Erro: " + "Pagamento", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }

                this.Entidade_Member = Get_DBF_AMFC_Member_By_Number(objEntidade.SOCIO);
                if (this.Entidade_Member == null || this.Entidade_Member.NUMERO <= 0 || this.Entidade_Member.NUMERO > this.Entidade_Member.MaxNumber)
                    return null;

                using (Library_AMFC_SQL obj_AMFC_SQL = new Library_AMFC_SQL())
                {
                    objPayment = obj_AMFC_SQL.Get_Member_Payment_By_Id(this.Entidade_Member.NUMERO, lPayId);
                    if (objPayment == null || objPayment.ID < 1)
                        return null;
                }

                return objPayment;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, false, false);
                return null;
            }
        }

        /// <versions>19-11-2017(GesAMFC-v0.0.4.33)</versions>
        private void Pay_Edit_DB(PAG_ENTIDADE objEnt, Boolean bRelaodGrid, Boolean bShowMessageDialog)
        {
            try
            {
                Set_Details_Editability(false);
                LayoutControlGroupEdit.Visibility = LayoutVisibility.Never;
                LayoutControlGroupAdmin.Visibility = LayoutVisibility.Always;

                #region     Member Info
                if (objEnt == null)
                {
                    String sError = "Ocorreu um erro a obter os dados do " + "Pagamento" + " de " + this.Entidade_Configs.Entity_Desc_Short_Single + " do Sócio " + objEnt.NOME + " (Nº: " + objEnt.SOCIO + ")!";
                    MessageBox.Show(sError, "Erro Pagamento Sócio", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                #endregion  Member Info

                #region     Validate Data 
                if (CheckIsValid(objEnt, true) < 1)
                    return;
                #endregion  Validate Data

                #region     Edit Operation
                if (!DBF_AMFC_Edit(objEnt, bShowMessageDialog))
                    return;
                #endregion  Edit Operation

                if (bRelaodGrid)
                {
                    this.Selected_Entidade = objEnt;
                    Set_Details_Editability(false);
                    LayoutControlGroupAdd.Visibility = LayoutVisibility.Never;
                    LayoutControlGroupAdmin.Visibility = LayoutVisibility.Always;
                    Load_Grid_Info(true, true, true, -1, objEnt.ID);
                    Load_Entity_Details(objEnt.ID);
                    this.Grid_View.Focus();
                }
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return;
            }
        }

        /// <versions>18-11-2017(GesAMFC-v0.0.4.33)</versions>
        private PAG_ENTIDADE Get_Pay_Details(PAG_ENTIDADE objEnt)
        {
            try
            {
                if (objEnt.ID <= 1)
                    objEnt.ID = Convert.ToInt64(TextEdit_Id.Text.Trim());

                if (objEnt.SOCIO < 1 || objEnt.SOCIO > new AMFCMember().MaxNumber)
                    objEnt.SOCIO = Convert.ToInt64(TextEdit_MemberNumber.Text.Trim());

                if (!Program.IsValidTextString(objEnt.NOME))
                    objEnt.NOME = TextEdit_MemberName.Text.Trim();

                if (Program.IsValidAreaValue(TextEdit_Area_Paga.Text.Trim()))
                    objEnt.AREAPAGO = Program.SetAreaDoubleValue(TextEdit_Area_Paga.Text.Trim());

                if (Program.IsValidCurrencyEuroValue(TextEdit_Preco_Metro.Text.Trim()))
                    objEnt.PRECOM2P = Program.SetPayCurrencyEuroDoubleValue(TextEdit_Preco_Metro.Text.Trim());

                if (!Program.IsValidCurrencyEuroValue(objEnt.VALORPAGO))
                    objEnt.VALORPAGO = Program.SetPayCurrencyEuroDoubleValue(TextEdit_Value.Text.Trim());

                objEnt.DATAPAG = Program.SetDateTimeValue(DateEdit_EntidadeData.DateTime, -1, -1);
                objEnt.ANO = objEnt.DATAPAG.Year;
                objEnt.MES = objEnt.DATAPAG.Month;

                if (!Program.IsValidTextString(objEnt.NOTAS))
                    objEnt.NOTAS = TextEdit_Notas.Text.Trim();

                if (!Program.IsValidTextString(objEnt.LISTACAIXA))
                    objEnt.LISTACAIXA = TextEdit_Hidden_Pay_Caixa_ListIDs.Text.Trim();

                return objEnt;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return null;
            }
        }

        /// <versions>28-04-2017(v0.0.2.48)</versions>
        private AMFCMember Get_DBF_AMFC_Member_By_Number(Int64 lMemberNumber)
        {
            try
            {
                using (Library_AMFC_SQL obj_AMFC_SQL = new Library_AMFC_SQL())
                    return obj_AMFC_SQL.Get_Member_By_Number(lMemberNumber);
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return null;
            }
        }

        #endregion  Pay 

        #endregion  Actions Methods

        /// <versions>28-11-2017(GesAMFC-v0.0.4.39)</versions>
        public PAG_ENTIDADE DBF_Get_Member_Pagamento_ById(Int64 lId)
        {
            try
            {
                using (Library_AMFC_SQL obj_AMFC_SQL = new Library_AMFC_SQL())
                {
                    Int32 iYear = GetCurrentYear();
                    return obj_AMFC_SQL.Get_Member_ENTID_ById(this.Entidade_Configs, lId);
                }
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return null;
            }
        }

        /// Colocar no Program
        /// <versions>02-12-2017(GesAMFC-v0.0.4.41)</versions>
        public Int32 GetCurrentYear()
        {
            try
            {
                Int32 iYear = -1;
                if (this.YearSelected != null && Program.IsValidYear(this.YearSelected.Value))
                    return this.YearSelected.Value;
                return iYear;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return -1;
            }
        }

        /// <versions>16-03-2018(GesAMFC-v1.0.0.3)</versions>
        private PAG_ENTIDADE Get_Entity_ToAdd(AMFCMember objMember, AMFCMemberLote objLote, Int64 lId)
        {
            try
            {
                #region     Check
                if (objMember.NUMERO < objMember.MinNumber || objMember.NUMERO > objMember.MaxNumber)
                {
                    String sError = "O Nº Sócio não é válido!";
                    MessageBox.Show(sError, "Sócio" + " " + "Inválido", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }

                if (objLote == null || objLote.IDLOTE < 0 || String.IsNullOrEmpty(objLote.NUMLOTE.Trim()))
                {
                    String sError = "O Lote não é válido!";
                    MessageBox.Show(sError, "Lote" + " " + "Inválido", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
                #endregion  Check

                PAG_ENTIDADE objEnt = new PAG_ENTIDADE();
                objEnt.ID = lId;

               objEnt.SOCIO = objMember.NUMERO;

                if (Program.IsValidTextString(objMember.NOME))
                    objEnt.NOME = objMember.NOME.Trim();

                objEnt.IDLOTE = objLote.IDLOTE;
                objEnt.NUMLOTE = objLote.NUMLOTE;

                using (Library_AMFC_SQL obj_AMFC_SQL = new Library_AMFC_SQL())
                {
                    Int32 iNumPags = obj_AMFC_SQL.Get_DB_ENTID_PAGNBR_Max_Number(this.Entidade_Configs, objEnt);
                    Int32 iTotPags = obj_AMFC_SQL.Get_DB_ENTID_PAGTOTAL_Number(this.Entidade_Configs, objEnt);
                    objEnt.PAGNBR = (iNumPags > 0) ? (iNumPags + 1) : 1;
                    objEnt.PAGTOTAL = (iTotPags > 0) ? iTotPags : 0;
                }

                objEnt.AREAPAGO = Program.Default_Area_Value;
                if (this.Entidade_Configs.Entity_Value_Meter > 0)
                    objEnt.PRECOM2P = Program.SetPayCurrencyEuroDoubleValue(this.Entidade_Configs.Entity_Value_Meter);
                objEnt.VALORPAGO = Program.Default_Pay_Value;

                objEnt.DATAPAG = Program.Default_Date;
                objEnt.ANO = objEnt.DATAPAG.Year;
                objEnt.MES = objEnt.DATAPAG.Month;

                objEnt.NOTAS = String.Empty;

                //if (Program.IsValidTextString(TextEdit_Hidden_Pay_Caixa_ListIDs.Text))
                //    objEnt.LISTACAIXA = TextEdit_Hidden_Pay_Caixa_ListIDs.Text.Trim();

                return objEnt;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return null;
            }
        }

        #endregion  Methods

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
    }
}