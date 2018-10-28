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
using System.Windows.Forms;
using System.Threading;

namespace GesAMFC
{
    /// <summary>Members Entidades Admin</summary>
    /// <author>Valter Lima</author>
    /// <creation>16-06-2017(v0.0.4.1)</creation>
    /// <versions>06-12-2017(GesAMFC-v0.0.5.0)</versions>
    public partial class Admin_Quotas : DevExpress.XtraEditors.XtraForm
    {
        public Library_AMFC_Methods LibAMFC;

        #region    Form Fields
        private AMFCMember  Entidade_Member;
        private AMFC_Entity Selected_Entidade;
        public  AMFCYear    YearSelected;
        public  AMFCYears   ListYears;
        public  AMFCPeriodYears  PeriodSelected;
        private enum GridDatasourceType { UNDEF = -1, ALL = 1, MEMBER = 2, ID = 3, YEAR = 4 }
        private GridDatasourceType Grid_Datasouce_Type;
        private String _GridColId       = "Id";
        private String _GridColSocio    = "MemberNumber";
        private Int32   LoadingGridPanelWaitTime = 500;
        #endregion  Grid Columns

        private Double Current_Entity_Value = Program.Get_Current_Parameter_QUOTA_Valor_Ano();

        #region     Entity
        public String Entity_Desc_Single        = Program.Entity_QUOTA_Desc_Single;
        public String Entity_Lower_Single       = Program.Entity_QUOTA_Lower_Single;
        public String Entity_Upper_Single       = Program.Entity_QUOTA_Upper_Single;
        public String Entity_Desc_Plural        = Program.Entity_QUOTA_Desc_Plural;
        public String Entity_Lower_Plural       = Program.Entity_QUOTA_Lower_Plural;
        public String Entity_Upper_Plural       = Program.Entity_QUOTA_Upper_Plural;
        public String Entity_Abbr_Lower         = Program.Entity_QUOTA_Abbr_Lower;
        public String Entity_Abbr_Upper         = Program.Entity_QUOTA_Abbr_Upper;
        #endregion  Entity

        #region     Form Constructor 

        /// <versions>05-05-2017(v0.0.3.2)</versions>
        public Admin_Quotas()
        {
            LibAMFC = new Library_AMFC_Methods();   

            try
            {
                this.Entidade_Member    = new AMFCMember();
                this.Selected_Entidade  = new AMFC_Entity();

                ListYears = new AMFCYears();
                ListYears.SetYearList();
                
                this.YearSelected   = Program.DefaultYear;
                
                this.PeriodSelected = new AMFCPeriodYears();
                this.PeriodSelected.Start = new AMFCPeriodYear();
                this.PeriodSelected.Start.Year  = new AMFCYear(DateTime.Today.Year);
                this.PeriodSelected.End.Year = new AMFCYear(DateTime.Today.Year);

                Grid_Datasouce_Type = GridDatasourceType.ALL; 
                this.LoadingGridPanelWaitTime = 500;

                InitializeComponent();

                LayoutControlGroup_MemberAdmiDate.Visibility = LayoutVisibility.Never;
                LayoutControlGroup_Data_Insercao.Visibility = LayoutVisibility.Never;
                LayoutControlItem_Value_Paid.Visibility = LayoutVisibility.Never;
                LayoutControlItem_Value_OnPaying.Visibility = LayoutVisibility.Never;
                LayoutControlItem_Value_Missing.Visibility = LayoutVisibility.Never;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        #endregion  Form Constructor

        #region     Events

        #region     Form Events

        /// <versions>07-10-2017(v0.0.4.8)</versions>
        private void Admin_Load(object sender, EventArgs e)
        {
            try
            {
                this.WindowState = FormWindowState.Maximized;
                this.Update();

                #region     Set DateEdits Calendars
                Program.SetCalendarControl(DateEdit_MemberAdmiDate);
                Program.SetCalendarControl(DateEdit_EntidadeData);
                Program.SetCalendarControl(DateEdit_PayDate);
                #endregion  Set DateEdits Calendars

                #region     Set Currency Euro Edit Values
                Program.SetPayEditValues(TextEdit_Value);
                Program.SetPayEditValues(TextEdit_Value_ToPay);
                Program.SetPayEditValues(TextEdit_Value_Paid);
                Program.SetPayEditValues(TextEdit_Value_Missing);
                #endregion  Set Currency Euro Edit Values

                LibAMFC.GridConfiguration(this.Grid_Control, this.Grid_View, true, false, true, true, true);
                this.Grid_View.OptionsView.RowAutoHeight = true;
                this.Grid_View.OptionsView.AnimationType = DevExpress.XtraGrid.Views.Base.GridAnimationType.AnimateAllContent;
                this.Grid_View.OptionsView.WaitAnimationOptions = WaitAnimationOptions.Panel;
                SplitContainer_Grid.Panel2Collapsed = true;
                this.SplashScreenManager_LoadingGrid = new DevExpress.XtraSplashScreen.SplashScreenManager(this, typeof(global::GesAMFC.WaitFormLoadingPanel), true, true);

                SetYearComboList();

                LayoutControlGroup_Payment_Buttons.Visibility = LayoutVisibility.Never;

                //Load_All_Members();
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
                    else
                    {
                        Boolean bPaid = Convert.ToBoolean(this.Grid_View.GetRowCellValue(e.RowHandle, this.Grid_View.Columns["Paid"]));
                        if (bPaid)
                            e.Appearance.BackColor = Program.GreenRowBgColor;
                        else
                        {
                            String sState = Convert.ToString(this.Grid_View.GetRowCellValue(e.RowHandle, this.Grid_View.Columns["PayState"]));
                            if (sState.Trim().Substring(0, 1).ToUpper() == new AMFC_Entity().GetPayStateDesc(AMFC_Entity.PayState.NAO).Substring(0, 1).ToUpper())
                                e.Appearance.BackColor = Program.RedRowBgColor;
                            else
                                if (sState.Trim().Substring(0, 1).ToUpper() == new AMFC_Entity().GetPayStateDesc(AMFC_Entity.PayState.EM_PAGAMENTO).Substring(0, 1).ToUpper())
                                    e.Appearance.BackColor = Program.YelloRowBgColor;
                        }
                    }
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
        private void Button_Load_Year_Members_Click(object sender, EventArgs e)
        {
            Load_Year_Members();
        }

        /// <versions>02-12-2017(GesAMFC-v0.0.4.41)</versions>
        private void Button_Load_All_Members_Click(object sender, EventArgs e)
        {
            Load_All_Members();
        }

        /// <versions>05-12-2017(GesAMFC-v0.0.4.41)</versio
        private void Button_Pay_Period_Click(object sender, EventArgs e)
        {
            Pay_Member_Period();
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

        /// <versions>08-10-2017(v0.0.4.8)</versions>
        private void TextEdit_EntidadeData_Click(object sender, EventArgs e)
        {
            if (DateEdit_EntidadeData.Properties.ReadOnly)
                Edit_Action();
        }

        /// <versions>08-10-2017(v0.0.4.8)</versions>
        private void TextEdit_PayDate_Click(object sender, EventArgs e)
        {
            if (DateEdit_PayDate.Properties.ReadOnly)
                Edit_Action();
        }

        /// <versions>08-10-2017(v0.0.4.8)</versions>
        private void TextEdit_PaidPerson_Click(object sender, EventArgs e)
        {
            if (TextEdit_PaidPerson.Properties.ReadOnly)
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
        private void Button_Pay_Click(object sender, EventArgs e)
        {
            Pay();
        }

        private void Button_Show_Pay_Click(object sender, EventArgs e)
        {
            Open();
        }

        #endregion  Action Buttons Events

        #region     DateEdit Events

        /// <versions>21-11-2017(GesAMFC-v0.0.4.35)</versions>
        private void DateEdit_MemberAdmiDate_QueryPopUp(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DateEdit dateEdit = sender as DateEdit;
            Program.DateEdit_QueryPopUp_Event(dateEdit, e);
        }

        /// <versions>21-11-2017(GesAMFC-v0.0.4.35)</versions>
        private void DateEdit_EntidadeData_QueryPopUp(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DateEdit dateEdit = sender as DateEdit;
            Program.DateEdit_QueryPopUp_Event(dateEdit, e);
        }

        /// <versions>21-11-2017(GesAMFC-v0.0.4.35)</versions>
        private void DateEdit_PayDate_QueryPopUp(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DateEdit dateEdit = sender as DateEdit;
            Program.DateEdit_QueryPopUp_Event(dateEdit, e);
        }

        #endregion  DateEdit Events

        #region     Details Changed Events

        private void TextEdit_Value_TextChanged(object sender, EventArgs e)
        {
            Value_Changed();
        }

        private void TextEdit_Value_ToPay_TextChanged(object sender, EventArgs e)
        {
            Value_ToPay_Changed();
        }

        private void TextEdit_Value_Paid_TextChanged(object sender, EventArgs e)
        {
            Value_Paid_Changed();
        }

        private void Value_Changed()
        {
            try
            {
                AMFC_Entity objEntity = GetDetailsToEdit();
                objEntity.ValueToPay = Program.SetPayCurrencyEuroDoubleValue(objEntity.GetValueToPay());
                TextEdit_Value_ToPay.Text = Program.SetPayCurrencyEuroStringValue(objEntity.ValueToPay);
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, false, false);
            }
        }

        private void Value_ToPay_Changed()
        {
            try
            {
                if (Program.IsValidCurrencyEuroValue(TextEdit_Value_ToPay.Text.Trim()))
                {
                    AMFC_Entity objEntity = GetDetailsToEdit();
                    objEntity.ValueMissing = Program.SetPayCurrencyEuroDoubleValue(objEntity.GetValueMissing());
                    TextEdit_Value_Missing.Text = Program.SetPayCurrencyEuroStringValue(objEntity.ValueMissing);
                }
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, false, false);
            }
        }
        
        private void Value_Paid_Changed()
        {
            try
            {
                if (Program.IsValidCurrencyEuroValue(TextEdit_Value_Paid.Text.Trim()))
                {
                    
                    AMFC_Entity objEntity = GetDetailsToEdit();
                    objEntity.ValueMissing = Program.SetPayCurrencyEuroDoubleValue(objEntity.GetValueMissing());
                    TextEdit_Value_Missing.Text = Program.SetPayCurrencyEuroStringValue(objEntity.ValueMissing);
                }
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, false, false);
            }
        }

        #endregion  Details Changed Events

        #endregion  Events

        #region     Methods

        #region     Form Methods

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

                #region     Get Focused 
                Boolean bIsTheSameMember = IsTheSameSelected(iRowHandle, -1);
                AMFC_Entity objEntity = null;
                if (!bIsTheSameMember)
                    objEntity = Get_Selected(iRowHandle, -1);
                else
                    objEntity = this.Selected_Entidade;
                if (objEntity == null || objEntity.MemberNumber < 1 || objEntity.MemberNumber > new AMFCMember().MaxNumber || objEntity.Id < 1)
                {
                    XtraMessageBox.Show(objStackFrame.GetMethod().Name, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                #endregion  Get Focused 

                #region     Load Member Details
                if (!bIsTheSameMember)
                    Load_Entity_Details(objEntity.Id);
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
        private AMFC_Entity Get_Selected(Int32 iRowHandle, Int64 lId)
        {
            try
            {
                AMFC_Entity objEntity = null;
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
                        XtraMessageBox.Show("Não foi possível obter os detalhes do " + "Pagamento de " + this.Entity_Desc_Plural + " do sócio selecionado!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return null;
                    }
                    lFocusedId = Convert.ToInt64(this.Grid_View.GetRowCellValue(iFocusedRowHandle, this.Grid_View.Columns[_GridColId]));
                }
                else
                    lFocusedId = lId;

                if (lFocusedId == this.Selected_Entidade.Id)
                    return this.Selected_Entidade;
                objEntity = DBF_Get_Member_Quota_ById(lFocusedId);

                if(objEntity == null || objEntity.MemberNumber < 1 || objEntity.MemberNumber > new AMFCMember().MaxNumber || objEntity.Id < 1)
                {
                    XtraMessageBox.Show("Não foi possível obter os detalhes do " + "Pagamento de " + this.Entity_Desc_Plural + " do sócio selecionado!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
                this.Selected_Entidade = objEntity;
                return objEntity;
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
                        XtraMessageBox.Show("Não foi possível obter os detalhes do " + "Pagamento de " + this.Entity_Desc_Plural + " do sócio!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    lFocusedId = Convert.ToInt64(this.Grid_View.GetRowCellValue(iFocusedRowHandle, this.Grid_View.Columns[_GridColId]));
                }
                else
                    lFocusedId = lId;
                if (lFocusedId == this.Selected_Entidade.Id)
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
                    this.SplashScreenManager_LoadingGrid.SetWaitFormDescription("A carregar Lista de " + this.Entity_Desc_Plural + " ...");
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
                AMFC_Entities objAMFC_Entitys = new AMFC_Entities();
                using (Library_AMFC_SQL obj_AMFC_SQL = new Library_AMFC_SQL())
                {
                    switch (this.Grid_Datasouce_Type)
                    {
                        case GridDatasourceType.ALL:
                            objAMFC_Entitys = obj_AMFC_SQL.Get_All_Member_QUOTAS(-1);
                            this.LoadingGridPanelWaitTime = 500;
                            break;
                        case GridDatasourceType.MEMBER:
                            objAMFC_Entitys = obj_AMFC_SQL.Get_Member_QUOTAS_ByNbr(this.Entidade_Member.NUMERO, this.YearSelected.Value);
                            this.LoadingGridPanelWaitTime = 500;
                            break;
                        case GridDatasourceType.YEAR:
                            objAMFC_Entitys = obj_AMFC_SQL.Get_Member_QUOTAS_ByNbr(this.Entidade_Member.NUMERO, this.YearSelected.Value);
                            this.LoadingGridPanelWaitTime = 500;
                            break;
                        case GridDatasourceType.ID:
                            objAMFC_Entitys = new AMFC_Entities();
                            objAMFC_Entitys.Add(obj_AMFC_SQL.Get_Member_QUOTA_ById(this.Selected_Entidade.Id, this.YearSelected.Value));
                            this.LoadingGridPanelWaitTime = 500;
                            break;
                        default:
                            break;
                    }
                }
                if (objAMFC_Entitys == null)
                {
                    sErrorMsg = "Não foi possível obter a Lista " + Entity_Desc_Plural + "!";
                    Program.HandleError(objStackFrame.GetMethod().Name, sErrorMsg, Program.ErroType.ERROR, true, false);
                    return false;
                }
                if (objAMFC_Entitys.Entidades.Count == 0)
                {
                    String sWarningMsg = "Não existem " + Entity_Desc_Plural + "!";
                    MessageBox.Show(sWarningMsg, "No results", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                #endregion  Members Datasource

                #region     TreeDataSet
                DataTable objDataTableSource = new DataTable("AMFC_Members_" + Entity_Desc_Plural);

                #region     Data Columns Creation
                //DataColumn objDataColumn_EntidadeIndex = new DataColumn("Index", typeof(Int64));
                DataColumn objDataColumn_Id = new DataColumn(_GridColId, typeof(Int64));
                DataColumn objDataColumn_MemberNumber = new DataColumn(_GridColSocio, typeof(Int64));
                DataColumn objDataColumn_MemberName = new DataColumn("MemberName", typeof(String));
                //DataColumn objDataColumn_MemberAdmiDate             = new DataColumn("MemberAdmiDate",  typeof(String));
                DataColumn objDataColumn_Date = new DataColumn("Date", typeof(String));
                DataColumn objDataColumn_Year = new DataColumn("Year", typeof(String));
                DataColumn objDataColumn_YearInt = new DataColumn("YearInt", typeof(String));
                DataColumn objDataColumn_Value = new DataColumn("Value", typeof(Double));
                DataColumn objDataColumn_Paid = new DataColumn("Paid", typeof(Boolean));
                DataColumn objDataColumn_PaidOrNot = new DataColumn("PaidOrNot", typeof(String));
                DataColumn objDataColumn_PayState = new DataColumn("PayState", typeof(String));
                DataColumn objDataColumn_PaidPerson = new DataColumn("PaidPerson", typeof(String));
                DataColumn objDataColumn_Notas = new DataColumn("Notas", typeof(String));
                DataColumn objDataColumn_ListaCaixa = new DataColumn("ListaCaixa", typeof(String));
                DataColumn objDataColumn_ListaRecibos = new DataColumn("ListaRecibos", typeof(String));


                #endregion  Data Columns Creation
                #region     Data Table Add Columns
                objDataTableSource.Columns.AddRange(
                                            new DataColumn[] {
                                                //objDataColumn_EntidadeIndex,
                                                objDataColumn_Id,
                                                objDataColumn_MemberNumber,
                                                //objDataColumn_MemberAdmiDate,
                                                objDataColumn_MemberName,
                                                objDataColumn_Date,
                                                objDataColumn_Year,
                                                objDataColumn_YearInt,
                                                objDataColumn_Value,
                                                objDataColumn_Paid,
                                                objDataColumn_PaidOrNot,
                                                objDataColumn_PayState,
                                                objDataColumn_PaidPerson,
                                                objDataColumn_Notas,
                                                objDataColumn_ListaCaixa,
                                                objDataColumn_ListaRecibos
                                            }
                                        );
                #endregion  Data Table Add Columns
                #region     Data Columns Dispose
                //objDataColumn_EntidadeIndex.Dispose();
                objDataColumn_Id.Dispose();
                objDataColumn_MemberNumber.Dispose();
                objDataColumn_MemberName.Dispose();
                //objDataColumn_MemberAdmiDate.Dispose();
                objDataColumn_Date.Dispose();
                objDataColumn_Year.Dispose();
                objDataColumn_YearInt.Dispose();
                objDataColumn_Value.Dispose();
                objDataColumn_Paid.Dispose();
                objDataColumn_PaidOrNot.Dispose();
                objDataColumn_PayState.Dispose();
                objDataColumn_PaidPerson.Dispose();
                objDataColumn_Notas.Dispose();
                objDataColumn_ListaCaixa.Dispose();
                objDataColumn_ListaRecibos.Dispose();
                #endregion  Data Columns Dispose
                #endregion  TreeDataSet

                #region     Binding Data
                Int32 iRowIdx = -1;
                foreach (AMFC_Entity objEntidade in objAMFC_Entitys.Entidades)
                {
                    if (objEntidade == null || objEntidade.MemberNumber < 1 || objEntidade.MemberNumber > new AMFCMember().MaxNumber || objEntidade.Id < 1)
                        continue;

                    #region     Set Row Data
                    iRowIdx++;

                    DataRow objDataRow = objDataTableSource.NewRow();
                    //objDataRow["Index"] = iRowIdx;
                    objDataRow[_GridColId] = objEntidade.Id;
                    objDataRow[_GridColSocio] = objEntidade.MemberNumber;
                    objDataRow["MemberName"] = objEntidade.MemberName;
                    //objDataRow["MemberAdmiDate"] = objEntidade.MemberAdmiDate;
                    objDataRow["Date"] = objEntidade.DatePaid;
                    objDataRow["Year"] = objEntidade.Year;
                    objDataRow["YearInt"] = objEntidade.YearInt.ToString();
                    objDataRow["Value"] = objEntidade.Value;
                    objDataRow["Paid"] = objEntidade.Paid;
                    objDataRow["PaidOrNot"] = objEntidade.PaidOrNot.ToString();
                    objDataRow["PayState"] = objEntidade.Pay_State.ToString();
                    objDataRow["PaidPerson"] = objEntidade.PaidPerson;
                    objDataRow["Notas"] = objEntidade.Notas;
                    objDataRow["ListaCaixa"] = objEntidade.ListaCaixa;
                    objDataRow["ListaRecibos"] = objEntidade.ListaRecibos;
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
                //LibAMFC.SetGridColumnOptions(this.Grid_Control, this.Grid_View, _GridColId, "Index", "Index", false, -1, 100, true, false, false, false, HorzAlignment.Center, VertAlignment.Center, HorzAlignment.Far, VertAlignment.Center);
                LibAMFC.SetGridColumnOptions(this.Grid_Control, this.Grid_View, _GridColId, "ID " + "Pagamento", "ID do " + "Pagamento", false, -1, 80, true, false, false, false, HorzAlignment.Center, VertAlignment.Center, HorzAlignment.Far, VertAlignment.Center);

                LibAMFC.SetGridColumnOptions(this.Grid_Control, this.Grid_View, _GridColSocio, "Nº Sócio", "Número de sócio", true, 2, 85, true, false, true, false, HorzAlignment.Far, VertAlignment.Center, HorzAlignment.Far, VertAlignment.Center);
                LibAMFC.SetGridColumnOptions(this.Grid_Control, this.Grid_View, "MemberName", "Nome", "Nome do sócio", true, 3, 250, true, false, true, false, HorzAlignment.Near, VertAlignment.Center, HorzAlignment.Near, VertAlignment.Center);
                
                //LibAMFC.SetGridColumnOptions(this.Grid_Control, this.Grid_View, "MemberAdmiDate",       "Data Admissão",                "Data Admissão o Sócio",                                                              true,   4,   80, true,  false, true,     true,          HorzAlignment.Center,  VertAlignment.Center,      HorzAlignment.Center,  VertAlignment.Center);

                LibAMFC.SetGridColumnOptions(this.Grid_Control, this.Grid_View, "Year", "Ano", "Ano", true, 7, 60, true, false, true, true, 0, HorzAlignment.Center, VertAlignment.Center, HorzAlignment.Center, VertAlignment.Center);
                
                LibAMFC.SetGridColumnOptions(this.Grid_Control, this.Grid_View, "YearInt", "Ano", "Ano", true, 9, 60, true, false, true, true, HorzAlignment.Center, VertAlignment.Center, HorzAlignment.Center, VertAlignment.Center);

                LibAMFC.SetGridColumnOptions(this.Grid_Control, this.Grid_View, "Date", "Pagamento", "Data de Pagamento", true, 11, 130, true, false, false, false, HorzAlignment.Center, VertAlignment.Center, HorzAlignment.Center, VertAlignment.Center);
                
                LibAMFC.SetGridColumnOptions(this.Grid_Control, this.Grid_View, "PaidOrNot", "Pago / Não Pago", Entity_Desc_Plural + " " + "Pagas" + " / " + Entity_Desc_Plural + " em atraso", false, -1, 80, true, false, false, false, -1, HorzAlignment.Center, VertAlignment.Center, HorzAlignment.Center, VertAlignment.Center);
                LibAMFC.SetGridColumnOptions(this.Grid_Control, this.Grid_View, "Paid", "Pago", this.Entity_Desc_Plural + " Pagas /" + Entity_Desc_Plural + " Não Pagas", false, -1, 80, true, false, true, false, HorzAlignment.Center, VertAlignment.Center, HorzAlignment.Center, VertAlignment.Center);
                LibAMFC.SetGridColumnOptions(this.Grid_Control, this.Grid_View, "PaidPerson", "Pagou", "Pessoa que pagou", false, -1, 220, true, false, true, false, HorzAlignment.Center, VertAlignment.Center, HorzAlignment.Center, VertAlignment.Center);

                LibAMFC.SetGridColumnOptions(this.Grid_Control, this.Grid_View, "Notas", "Notas", "Observações", false, -1, 220, true, false, true, false, HorzAlignment.Center, VertAlignment.Center, HorzAlignment.Center, VertAlignment.Center);

                LibAMFC.SetGridColumnOptions(this.Grid_Control, this.Grid_View, "ListaCaixa", "ListaCaixa", "ListaCaixa", false, -1, 80, true, false, true, true, HorzAlignment.Center, VertAlignment.Center, HorzAlignment.Center, VertAlignment.Center);
                LibAMFC.SetGridColumnOptions(this.Grid_Control, this.Grid_View, "ListaRecibos", "ListaRecibos", "ListaRecibos", false, -1, 80, true, false, true, true, HorzAlignment.Center, VertAlignment.Center, HorzAlignment.Center, VertAlignment.Center);
                LibAMFC.SetGridColumnOptions(this.Grid_Control, this.Grid_View, "PayState", "PayState", "PayState", false, -1,  80, true,  false, true,     true,      HorzAlignment.Center,  VertAlignment.Center,      HorzAlignment.Center,  VertAlignment.Center);

                LibAMFC.SetGridColumnOptions(this.Grid_Control, this.Grid_View, "Value", "Valor Pago", "Valores das " + this.Entity_Desc_Plural, true, 12, 150, true, false, true, true, HorzAlignment.Center, VertAlignment.Center, HorzAlignment.Far, VertAlignment.Center, true);                
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
                //LibAMFC.SetGridColumOptionsFilter(this.Grid_View, "Index", true, true, AutoFilterCondition.Equals, 8.0f);
                LibAMFC.SetGridColumOptionsFilter(this.Grid_View, _GridColId, true, true, AutoFilterCondition.Equals, 8.0f);

                LibAMFC.SetGridColumOptionsFilter(this.Grid_View, _GridColSocio, true, true, AutoFilterCondition.Equals, 8.0f);
                LibAMFC.SetGridColumOptionsFilter(this.Grid_View, "MemberName", true, true, AutoFilterCondition.Contains, 8.5f);

                //LibAMFC.SetGridColumOptionsFilter(this.Grid_View, "MemberAdmiDate",     true, true, AutoFilterCondition.Equals,     8.5f);
                //LibAMFC.SetGridColumOptionsFilter(this.Grid_View, "Date",               true, true, AutoFilterCondition.Contains,   8.5f);

                LibAMFC.SetGridColumOptionsFilter(this.Grid_View, "Year", true, true, AutoFilterCondition.Equals, 8.5f);
                
                LibAMFC.SetGridColumOptionsFilter(this.Grid_View, "YearInt", true, true, AutoFilterCondition.Equals, 8.5f);
                
                LibAMFC.SetGridColumOptionsFilter(this.Grid_View, "Value", true, true, AutoFilterCondition.Contains, 8.5f);
                LibAMFC.SetGridColumOptionsFilter(this.Grid_View, "Date", true, true, AutoFilterCondition.Equals, 8.5f);

                LibAMFC.SetGridColumOptionsFilter(this.Grid_View, "Paid", true, true, AutoFilterCondition.Equals, 8.5f);
                LibAMFC.SetGridColumOptionsFilter(this.Grid_View, "PaidPerson", true, true, AutoFilterCondition.Equals, 8.5f);

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
                                this.Grid_Datasouce_Type = GridDatasourceType.MEMBER;
                                Load_Grid_Info(true, true, true, -1, -1);
                                this.LoadingGridPanelWaitTime = 500;
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

        /// <versions>01-12-2017(GesAMFC-v0.0.4.41)</versions>
        private void Load_Year_Members()
        {
            try
            {
                Form_Year objFormYear = new Form_Year(this.YearSelected);
                if (objFormYear != null)
                {
                    objFormYear.FormClosing += delegate
                    {
                        if (objFormYear.IsYearSelected)
                        {
                            if (Program.IsValidYear(objFormYear.YearSelected.Value))
                            {
                                this.YearSelected = objFormYear.YearSelected;
                                this.Grid_Datasouce_Type = GridDatasourceType.YEAR;
                                this.Entidade_Member = new AMFCMember();
                                Load_Grid_Info(true, true, true, -1, -1);
                                this.LoadingGridPanelWaitTime = 500;
                            }
                        }
                        else
                            this.Grid_View.FocusedRowHandle = -1;
                    };
                    objFormYear.Show();
                    objFormYear.StartPosition = FormStartPosition.CenterParent;
                    objFormYear.Focus();
                    objFormYear.BringToFront();
                    objFormYear.Update();
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
                AMFC_Entity objEntity = Get_Selected(-1, lId);
                if (objEntity == null || objEntity.MemberNumber < 1 || objEntity.MemberNumber > new AMFCMember().MaxNumber || objEntity.Id < 1)
                {
                    Clear_Details(true);
                    XtraMessageBox.Show("Não foi possível obter o " + "Pagamento de " + this.Entity_Desc_Plural+ "!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                #endregion  Get Focused Member

                #region     Set Details Controls
                Set_Details_Editability(false);

                LayoutControlGroup_Payment_Buttons.Visibility = LayoutVisibility.Always;
                LayoutControlGroupAdmin.Visibility = LayoutVisibility.Always;
                LayoutControlGroupAdd.Visibility = LayoutVisibility.Never;
                LayoutControlGroupEdit.Visibility = LayoutVisibility.Never;
                LayoutControlGroup_Payment.Visibility = LayoutVisibility.Always;

                this.LayoutControlItem_Pay.Visibility = (!objEntity.Paid && objEntity.Value > 0) ? LayoutVisibility.Always : LayoutVisibility.Never;
                #endregion  Set Details Controls

                #region     Load Details
                if (objEntity != null)
                {
                    if (!SetDetails(objEntity))
                    {
                        Clear_Details(true);
                        XtraMessageBox.Show("Não foi possível obter carregar detalhes do " + "Pagamento de " + this.Entity_Desc_Plural + "!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        /// <versions>08-10-2017(v0.0.4.8)</versions>
        private void Clear_Details(Boolean bClearId)
        {
            try
            {
                if (bClearId)
                    this.TextEdit_Id.Text = String.Empty;
                this.TextEdit_MemberNumber.Text = String.Empty;
                this.TextEdit_MemberName.Text = String.Empty;
                this.DateEdit_MemberAdmiDate.DateTime = Program.Default_Date;
                this.TextEdit_Value.Text = Program.Default_Pay_String;
                if (this.PictureBox_Payment.Image != null)
                {
                    this.PictureBox_Payment.Image.Dispose();
                    this.PictureBox_Payment.Image = null;
                }
                
                this.DateEdit_EntidadeData.DateTime = new DateTime();
                this.TextEdit_Notas.Text = String.Empty;

                this.TextEdit_Value_ToPay.Text = Program.Default_Pay_String;
                this.TextEdit_Value_Paid.Text = Program.Default_Pay_String;
                this.TextEdit_Value_Missing.Text = Program.Default_Pay_String;

                this.DateEdit_PayDate.DateTime = Program.Default_Date;
                this.TextEdit_PaidPerson.Text = String.Empty;

                ComboBoxEdit_Year.SelectedIndex = -1;
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
                Boolean bIsReadOnly = !bCanEdit;
                TextEdit_Id.Properties.ReadOnly = true; //always
                TextEdit_MemberNumber.Properties.ReadOnly = true; //always
                TextEdit_MemberName.Properties.ReadOnly = bIsReadOnly; //always
                DateEdit_MemberAdmiDate.Properties.ReadOnly = true; //always
                TextEdit_Value.Properties.ReadOnly = bIsReadOnly;
                
                DateEdit_EntidadeData.Properties.ReadOnly = bIsReadOnly;
                TextEdit_Notas.Properties.ReadOnly = bIsReadOnly;

                TextEdit_Value_ToPay.Properties.ReadOnly = true;
                TextEdit_Value_Paid.Properties.ReadOnly = true;
                TextEdit_Value_Missing.Properties.ReadOnly = true;
                DateEdit_PayDate.Properties.ReadOnly = true;
                TextEdit_PaidPerson.Properties.ReadOnly = true;

                ComboBoxEdit_Year.Properties.ReadOnly = bIsReadOnly;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return;
            }
        }
        
        /// <versions>08-10-2017(v0.0.4.8)</versions>
        private Int32 CheckIsValid(AMFC_Entity objEntity, Boolean bCheckId)
        {
            try
            {
                #region    ID
                if (bCheckId)
                {
                    if (objEntity.Id < 1)
                    {
                        String sError = "O " + "ID do " + "Pagamento de " + this.Entity_Desc_Plural + ": " + objEntity.Id + " não é válido! Por favor, modifique.";
                        MessageBox.Show(sError, "ID " + " Inválido", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return -1;
                    }
                }
                #endregion ID

                #region     Nº Sócio
                if (objEntity.MemberNumber < 1 || objEntity.MemberNumber > new AMFCMember().MaxNumber)
                {
                    String sError = "O " + "Nº de Sócio: " + objEntity.MemberNumber + " não é válido! Por favor, modifique.";
                    MessageBox.Show(sError, "Sócio Inválido", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return -1;
                }
                #endregion  Nº Sócio

                #region     Nome
                if (!Program.IsValidTextString(objEntity.MemberName))
                {
                    String sWarning = "O " + "Nome do Sócio" + " (" + objEntity.MemberName.Trim() + ") não é válido! Por favor, modifique.";
                    MessageBox.Show(sWarning, "Nome Sócio" + " Invalído", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return -1;
                }
                #endregion  Nome

                #region     Member Admission Date
                if (objEntity.DtMemberAdmiDate == null)
                {
                    String sWarning = "A " + "data de admissão" + " do Sócio (" + objEntity.MemberAdmiDate.Trim() + ") não é válida! Por favor, modifique.";
                    MessageBox.Show(sWarning, "Data de admissão" + " Invalída", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return -1;
                }
                #endregion  Member Admission Date

                #region    Value
                if (objEntity.Value <= 0)
                {
                    String sWarning = "O " + "valor do " + "Pagamento de " + this.Entity_Desc_Plural + " (" + objEntity.Value.ToString() + ") não é válido! Por favor, modifique.";
                    MessageBox.Show(sWarning, "Valor Invalído", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return -1;
                }
                #endregion Value

                #region    Date Paid
                if (objEntity.DtDatePaid == null)
                {
                    String sWarning = "A " + "data de pagamento" + " (" + objEntity.DatePaid.Trim() + ") não é válida! Por favor, modifique.";
                    MessageBox.Show(sWarning, "Data Pagamento" + " Invalída", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return -1;
                }
                #endregion Date Paid

                #region    Paid Person
                if (!Program.IsValidTextString(objEntity.PaidPerson))
                {
                    String sWarning = "O " + "nome da pessoa que efetuou o pagamento" + " (" + objEntity.PaidPerson.Trim() + ") não é válido! Por favor, modifique.";
                    MessageBox.Show(sWarning, "" + "Nome de que pagou invalído", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return -1;
                }
                #endregion  Paid Person

                return 1;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return -1;
            }
        }

        /// <versions>08-10-2017(v0.0.4.8)</versions>
        private AMFC_Entity GetDetailsToAdd(AMFCMember objMember, Int32 iYear)
        {
            try
            {
                AMFC_Entity objEntity = new AMFC_Entity();
                #region     Get Max ID
                Int32 iMaxId = DBF_AMFC_Members_GetMaxId();
                if (iMaxId < 1)
                    return null;
                #endregion  Get Max ID
                objEntity.Id = Convert.ToInt64(iMaxId) + 1;
                if (objMember.NUMERO > 0 && objMember.NUMERO < new AMFCMember().MaxNumber)
                    objEntity.MemberNumber = objMember.NUMERO;
                if (Program.IsValidTextString(objMember.NOME))
                    objEntity.MemberName = objMember.NOME.Trim();
                if (Program.IsValidTextString(objMember.DATAADMI))
                    objEntity.DtMemberAdmiDate = Program.ConvertToValidDateTime(objMember.DATAADMI.Trim());
                if (this.Current_Entity_Value > 0)
                {
                    objEntity.Value = Program.SetPayCurrencyEuroDoubleValue(this.Current_Entity_Value);
                    objEntity.ValueToPay = Program.SetPayCurrencyEuroDoubleValue(this.Current_Entity_Value);
                    objEntity.ValuePaid = Program.Default_Pay_Value;
                    objEntity.ValueMissing = Program.SetPayCurrencyEuroDoubleValue(this.Current_Entity_Value);
                }

                objEntity.YearInt = iYear;
                objEntity.DtDate = new DateTime(objEntity.YearInt, 1, 1);

                objEntity.DtDatePaid = Program.Default_Date;
                if (Program.IsValidTextString(objMember.NOME))
                    objEntity.PaidPerson = objMember.NOME.Trim();
                objEntity.Notas = String.Empty;

                return objEntity;
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
                DialogResult objDialogResult = XtraMessageBox.Show("Deseja editar do " + "Pagamento Pagamento" + this.Entity_Desc_Plural + "?", "Editar Pagamento" + " ?", MessageBoxButtons.OK, MessageBoxIcon.Question);
                if (objDialogResult == DialogResult.OK)
                    Button_Edit_Click();
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return;
            }
        }

        /// <versions>08-10-2017(v0.0.4.8)</versions>
        private AMFC_Entity GetDetailsToEdit()
        {
            try
            {
                AMFC_Entity objEntity = new AMFC_Entity();

                if (!String.IsNullOrEmpty(TextEdit_Id.Text.Trim()) && Convert.ToInt64(TextEdit_Id.Text.Trim()) > 0)
                    objEntity.Id = Convert.ToInt64(TextEdit_Id.Text.Trim());

                if (!String.IsNullOrEmpty(TextEdit_MemberNumber.Text.Trim()) && Convert.ToInt64(TextEdit_MemberNumber.Text.Trim()) > 0 && Convert.ToInt64(TextEdit_MemberNumber.Text.Trim()) < new AMFCMember().MaxNumber)
                    objEntity.MemberNumber = Convert.ToInt64(TextEdit_MemberNumber.Text.Trim());

                if (Program.IsValidTextString(TextEdit_MemberName.Text))
                    objEntity.MemberName = TextEdit_MemberName.Text.Trim();

                objEntity.DtMemberAdmiDate = Program.SetDateTimeValue(DateEdit_MemberAdmiDate.DateTime, -1, -1);

                if (Program.IsValidCurrencyEuroValue(TextEdit_Value.Text.Trim()))
                    objEntity.Value = Program.SetPayCurrencyEuroDoubleValue(TextEdit_Value.Text.Trim());
                else if (this.Current_Entity_Value > 0)
                    objEntity.Value = Program.SetPayCurrencyEuroDoubleValue(this.Current_Entity_Value);

                objEntity.YearInt = Convert.ToInt32((ComboBoxEdit_Year.SelectedItem as ComboboxItem).GetValue());
                objEntity.DtDate = new DateTime(objEntity.YearInt, 1, 1);

                if (Program.IsValidTextString(TextEdit_Notas.Text))
                    objEntity.Notas = TextEdit_Notas.Text.Trim();

                objEntity.DtDatePaid = Program.SetDateTimeValue(DateEdit_PayDate.DateTime, -1, -1);

                if (Program.IsValidTextString(TextEdit_PaidPerson.Text))
                    objEntity.PaidPerson = TextEdit_PaidPerson.Text.Trim();
                else if (Program.IsValidTextString(TextEdit_MemberName.Text))
                    objEntity.MemberName = TextEdit_MemberName.Text.Trim();

                if (Program.IsValidCurrencyEuroValue(TextEdit_Value_ToPay.Text.Trim()))
                    objEntity.ValueToPay = Program.SetPayCurrencyEuroDoubleValue(TextEdit_Value_ToPay.Text.Trim());

                if (Program.IsValidCurrencyEuroValue(TextEdit_Value_Paid.Text.Trim()))
                    objEntity.ValuePaid = Program.SetPayCurrencyEuroDoubleValue(TextEdit_Value_Paid.Text.Trim());

                if (Program.IsValidCurrencyEuroValue(TextEdit_Value_Missing.Text.Trim()))
                    objEntity.ValueMissing = Program.SetPayCurrencyEuroDoubleValue(TextEdit_Value_Missing.Text.Trim());

                return objEntity;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return null;
            }
        }

        /// <versions>08-10-2017(v0.0.4.8)</versions>
        private AMFC_Entity GetDetailsEdited()
        {
            try
            {
                AMFC_Entity objEntity = new AMFC_Entity();

                if (Convert.ToInt64(TextEdit_Id.Text.Trim()) > 0)
                    objEntity.Id = Convert.ToInt64(TextEdit_Id.Text.Trim());

                if (Convert.ToInt64(TextEdit_MemberNumber.Text.Trim()) > 0 && Convert.ToInt64(TextEdit_MemberNumber.Text.Trim()) < new AMFCMember().MaxNumber)
                    objEntity.MemberNumber = Convert.ToInt64(TextEdit_MemberNumber.Text.Trim());

                if (Program.IsValidTextString(TextEdit_MemberName.Text))
                    objEntity.MemberName = TextEdit_MemberName.Text.Trim();

                objEntity.DtMemberAdmiDate = Program.SetDateTimeValue(DateEdit_MemberAdmiDate.DateTime, -1, -1);

                if (Program.IsValidCurrencyEuroValue(TextEdit_Value.Text.Trim()))
                    objEntity.Value = Program.SetPayCurrencyEuroDoubleValue(TextEdit_Value.Text.Trim());

                objEntity.YearInt = Convert.ToInt32((ComboBoxEdit_Year.SelectedItem as ComboboxItem).GetValue());
                objEntity.DtDate = new DateTime(objEntity.YearInt, 1, 1);

                if (Program.IsValidTextString(TextEdit_Notas.Text))
                    objEntity.Notas = TextEdit_Notas.Text.Trim();

                objEntity.DtDatePaid = Program.SetDateTimeValue(DateEdit_PayDate.DateTime, -1, -1);

                if (Program.IsValidTextString(TextEdit_PaidPerson.Text))
                    objEntity.PaidPerson = TextEdit_PaidPerson.Text.Trim();
                else if (Program.IsValidTextString(TextEdit_MemberName.Text))
                    objEntity.PaidPerson = TextEdit_MemberName.Text.Trim();

                if (Program.IsValidCurrencyEuroValue(TextEdit_Value_ToPay.Text.Trim()))
                    objEntity.ValueToPay = Program.SetPayCurrencyEuroDoubleValue(TextEdit_Value_ToPay.Text.Trim());

                if (Program.IsValidCurrencyEuroValue(TextEdit_Value_Paid.Text.Trim()))
                    objEntity.ValuePaid = Program.SetPayCurrencyEuroDoubleValue(TextEdit_Value_Paid.Text.Trim());

                if (Program.IsValidCurrencyEuroValue(TextEdit_Value_Missing.Text.Trim()))
                    objEntity.ValueMissing = Program.SetPayCurrencyEuroDoubleValue(TextEdit_Value_Missing.Text.Trim());

                if (Program.IsValidTextString(TextEdit_Hidden_Pay_Caixa_ListIDs.Text))
                    objEntity.ListaCaixa = TextEdit_Hidden_Pay_Caixa_ListIDs.Text.Trim();
                
                return objEntity;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return null;
            }
        }

        /// <versions>18-11-2017(GesAMFC-v0.0.4.33)</versions>
        private AMFC_Entity GetDetailsEditedPayment()
        {
            try
            {
                AMFC_Entity objEntity = new AMFC_Entity();

                if (Convert.ToInt64(TextEdit_Id.Text.Trim()) > 0)
                    objEntity.Id = Convert.ToInt64(TextEdit_Id.Text.Trim());

                if (Convert.ToInt64(TextEdit_MemberNumber.Text.Trim()) > 0 && Convert.ToInt64(TextEdit_MemberNumber.Text.Trim()) < new AMFCMember().MaxNumber)
                    objEntity.MemberNumber = Convert.ToInt64(TextEdit_MemberNumber.Text.Trim());

                if (Program.IsValidTextString(TextEdit_MemberName.Text))
                    objEntity.MemberName = TextEdit_MemberName.Text.Trim();

                objEntity.DtMemberAdmiDate = Program.SetDateTimeValue(DateEdit_MemberAdmiDate.DateTime, -1, -1);

                if (Program.IsValidCurrencyEuroValue(TextEdit_Value.Text.Trim()))
                    objEntity.Value = Program.SetPayCurrencyEuroDoubleValue(TextEdit_Value.Text.Trim());

                objEntity.YearInt = Convert.ToInt32((ComboBoxEdit_Year.SelectedItem as ComboboxItem).GetValue());
                objEntity.DtDate = new DateTime(objEntity.YearInt, 1, 1);

                if (Program.IsValidTextString(TextEdit_Notas.Text))
                    objEntity.Notas = TextEdit_Notas.Text.Trim();

                objEntity.DtDatePaid = Program.SetDateTimeValue(DateEdit_PayDate.DateTime, -1, -1);

                if (Program.IsValidTextString(TextEdit_PaidPerson.Text))
                    objEntity.PaidPerson = TextEdit_PaidPerson.Text.Trim();
                else if (Program.IsValidTextString(TextEdit_MemberName.Text))
                    objEntity.PaidPerson = TextEdit_MemberName.Text.Trim();

                if (Program.IsValidCurrencyEuroValue(TextEdit_Value_Paid.Text.Trim()))
                {
                    objEntity.ValuePaid = Program.SetPayCurrencyEuroDoubleValue(TextEdit_Value_Paid.Text.Trim());
                    objEntity.ValueMissing = Program.SetPayCurrencyEuroDoubleValue(objEntity.GetValueMissing());
                    objEntity.ValueToPay = (objEntity.ValueMissing > 0 && objEntity.ValueMissing < objEntity.Value) ? Program.SetPayCurrencyEuroDoubleValue(objEntity.ValueMissing) : Program.SetPayCurrencyEuroDoubleValue(objEntity.GetValueToPay());
                }

                return objEntity;
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
                LayoutControlGroup_Payment.Visibility = LayoutVisibility.Never;

                #region     Member Info
                AMFC_Entity objEntity = GetDetailsEditedPayment();
                if (objEntity == null)
                {
                    String sError = "Ocorreu um erro a obter os dados do " + "Pagamento de " + this.Entity_Desc_Plural + " do Sócio " + objEntity.MemberName + " (Nº: " + objEntity.MemberNumber + ")!";
                    MessageBox.Show(sError, "Erro Pagamento", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                #endregion  Member Info

                #region     Validate Data 
                if (CheckIsValid(objEntity, true) < 1)
                    return;
                #endregion  Validate Data

                #region     Edit Operation
                if (!DBF_AMFC_Edit(objEntity, true))
                    return;
                #endregion  Edit Operation

                this.Selected_Entidade = objEntity;
                Set_Details_Editability(false);
                LayoutControlGroup_Payment.Visibility = LayoutVisibility.Always;
                LayoutControlGroupAdd.Visibility = LayoutVisibility.Never;
                LayoutControlGroupAdmin.Visibility = LayoutVisibility.Always;
                Load_Grid_Info(false, false, false, -1, objEntity.Id);
                Load_Entity_Details(objEntity.Id);
                this.Grid_View.Focus();
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return;
            }
        }

        /// <versions>08-10-2017(v0.0.4.8)</versions>
        private Boolean SetDetails(AMFC_Entity objEntity)
        {
            try
            {
                this.TextEdit_Id.Text = objEntity.Id.ToString();
                this.TextEdit_MemberNumber.Text = objEntity.MemberNumber.ToString();
                this.TextEdit_MemberName.Text = Program.SetTextString(objEntity.MemberName, Program.DefaultStringTextTypes.DEFAULT);
                this.DateEdit_MemberAdmiDate.DateTime = Program.SetDateTimeValue(objEntity.DtMemberAdmiDate, -1, -1);

                this.ComboBoxEdit_Year.SelectedIndex = GeComboListItemIndexByValue(this.ComboBoxEdit_Year, objEntity.YearInt);
                this.DateEdit_EntidadeData.DateTime = Program.SetDateTimeValue(objEntity.DtDate, objEntity.YearInt, 1);

                this.TextEdit_Notas.Text = objEntity.Notas;
                this.TextEdit_Value.Text = Program.SetPayCurrencyEuroStringValue(objEntity.Value);
                this.TextEdit_Value_ToPay.Text = Program.SetPayCurrencyEuroStringValue(objEntity.ValueToPay);
                this.TextEdit_Value_Paid.Text = Program.SetPayCurrencyEuroStringValue(objEntity.ValuePaid);
                this.TextEdit_Value_Missing.Text = Program.SetPayCurrencyEuroStringValue(objEntity.ValueMissing);
                this.DateEdit_PayDate.DateTime = Program.SetDateTimeValue(objEntity.DtDatePaid, -1, -1);
                this.TextEdit_PaidPerson.Text = Program.SetTextString(objEntity.PaidPerson, Program.DefaultStringTextTypes.DEFAULT);

                Set_PictureBox_Payment(objEntity);
                LayoutControlItem_Pay.Visibility = (objEntity.Paid || objEntity.Value <= 0) ? LayoutVisibility.Never : LayoutVisibility.Always;
                LayoutControlItem_Show_Pay.Visibility = (objEntity.Value > 0 && objEntity.Paid && Get_Pay_Last_Id(objEntity) > 0) ? LayoutVisibility.Always : LayoutVisibility.Never;

                Value_Changed();

                this.TextEdit_Hidden_Pay_Caixa_ListIDs.Text = objEntity.ListaCaixa;

                return true;
            }
            catch (Exception ex)
            {

                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return false;
            }
        }

        /// <versions>25-11-2017(GesAMFC-v0.0.4.37)</versions>
        private void Set_PictureBox_Payment(AMFC_Entity objEntidade)
        {
            try
            {
                if (objEntidade.Paid)
                {
                    this.PictureBox_Payment.Image = Properties.Resources.carimbo_pago_small;
                }
                else
                {
                    if (objEntidade.Pay_State == AMFC_Entity.PayState.EM_PAGAMENTO)
                        this.PictureBox_Payment.Image = Properties.Resources.carimbo_em_pagamento_small;
                    else
                        this.PictureBox_Payment.Image = Properties.Resources.carimbo_nao_pago_small;
                }
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return;
            }
        }

        /// <versions>08-10-2017(v0.0.4.8)</versions>
        private void Reload_Details()
        {
            try
            {
                AMFC_Entity objEntity = null;

                #region     Get Focused 
                if (this.Selected_Entidade != null && this.Selected_Entidade.Id > 1)
                {
                    if (this.Grid_View.FocusedRowHandle > 0)
                    {
                        objEntity = Get_Selected(this.Grid_View.FocusedRowHandle, -1);
                        if (objEntity == null || objEntity.MemberNumber < 1 || objEntity.MemberNumber > new AMFCMember().MaxNumber || objEntity.Id < 1)
                        {
                            Clear_Details(true);
                            XtraMessageBox.Show("Não foi possível obter o " + "Pagamento de " + this.Entity_Desc_Plural + "!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Load_Grid_Info(false, false, false, -1, -1);
                            return;
                        }
                    }
                }
                #endregion  Get Focused 

                #region     Load Details
                if (objEntity != null)
                {
                    if (!SetDetails(objEntity))
                    {
                        Clear_Details(true);
                        XtraMessageBox.Show("Não foi possível obter carregar detalhes do " + "Pagamento de " + this.Entity_Desc_Plural + "!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    Int32 iMaxId = obj_AMFC_SQL.Get_QUOTA_Max_Number();
                    if (iMaxId < 1)
                    {
                        String sWarning = "Não foi possivel obter o número máximo de ID do " + "Pagamento de " + this.Entity_Desc_Plural + "! Por favor, contacte o programador!";
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

        /// <versions>03-12-2017(GesAMFC-v0.0.4.41)</versions>
        private void GetMemberToAddEntidade()
        {
            try
            {
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

                                //Load Form_Period_Years
                                Button_Add_Action(objMemberSelected, this.YearSelected.Value);
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
            try
            {
                GetMemberToAddEntidade();
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return;
            }
        }

        /// <versions>08-10-2017(v0.0.4.8)</versions>
        private void Button_Add_Action(AMFCMember objMember, Int32 iYear)
        {
            try
            {
                #region     Get Member to Add
                if (objMember == null || objMember.NUMERO < 1 || objMember.NUMERO > objMember.MaxNumber)
                {
                    String sError = "Ocorreu um erro na obtenção do sócio para adicionar o " + "Pagamento de " + this.Entity_Desc_Plural + "!";
                    MessageBox.Show(sError, "Erro Obtenção Sócio", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                #endregion  Get Member to Add

                #region     Set Controls to Add
                Set_Details_Editability(true);
                LayoutControlGroupAdmin.Visibility = LayoutVisibility.Never;
                LayoutControlGroupAdd.Visibility = LayoutVisibility.Always;
                LayoutControlGroupEdit.Visibility = LayoutVisibility.Never;
                LayoutControlGroup_Payment.Visibility = LayoutVisibility.Never;
                #endregion  Set Controls to Add

                #region     Get Member Info to Add
                AMFC_Entity objEntity = GetDetailsToAdd(objMember, iYear);
                if (objEntity == null)
                {
                    String sError = "Ocorreu um erro a obter os dados do " + "Pagamento de " + this.Entity_Desc_Plural + " do Sócio " + objEntity.MemberName + " (Nº: " + objEntity.MemberNumber + ")!";
                    MessageBox.Show(sError, "Erro Criação Sócio", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                #endregion  Get Member Info to Add

                #region     Set Member Info to Add
                if (!SetDetails(objEntity))
                {
                    Clear_Details(true);
                    XtraMessageBox.Show("Não foi possível obter carregar detalhes do " + "Pagamento de " + this.Entity_Desc_Plural + "!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                AMFC_Entity objEntity = GetDetailsEdited();
                if (objEntity == null)
                {
                    String sError = "Ocorreu um erro a obter os dados do " + "Pagamento de " + this.Entity_Desc_Plural + " do Sócio " + objEntity.MemberName + " (Nº: " + objEntity.MemberNumber + ")!";
                    MessageBox.Show(sError, "Erro Criação Sócio", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                #endregion  Member Info

                #region     Validate Data 
                if (CheckIsValid(objEntity, true) < 1)
                    return;
                #endregion  Validate Data

                #region     Add Operation
                if (!DBF_AMFC_Add(objEntity, true))
                    return;
                this.Selected_Entidade = objEntity;
                #endregion  Add Operation

                Set_Details_Editability(false);
                LayoutControlGroup_Payment.Visibility = LayoutVisibility.Always;
                LayoutControlGroupAdd.Visibility = LayoutVisibility.Never;
                LayoutControlGroupAdmin.Visibility = LayoutVisibility.Always;
                this.Grid_Datasouce_Type = GridDatasourceType.MEMBER;
                Load_Grid_Info(true, true, true, -1, objEntity.Id);
                Load_Entity_Details(objEntity.Id);
                this.Grid_View.Focus();

                if (!objEntity.Paid && objEntity.Value > 0)
                    Pay();
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
                LayoutControlGroup_Payment.Visibility = LayoutVisibility.Always;
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
            if (this.Entidade_Member != null && this.Entidade_Member.NUMERO > 0 && this.Entidade_Member.NUMERO < this.Entidade_Member.MaxNumber)
                Button_Add_Action(this.Entidade_Member, this.YearSelected.Value);
            else 
                Clear_Details(false);
        }

        /// <versions>08-10-2017(v0.0.4.8)</versions>
        private Boolean DBF_AMFC_Add(AMFC_Entity objEntity, Boolean bShowMessage)
        {
            try
            {
                Int64 lOpStatus = -1;
                using (Library_AMFC_SQL obj_AMFC_SQL = new Library_AMFC_SQL())
                {
                    lOpStatus = obj_AMFC_SQL.Add_QUOTA(objEntity, bShowMessage);
                    if (lOpStatus == 1)
                    {
                        if (bShowMessage)
                        {
                            String sInfo = "Pagamento de " + this.Entity_Desc_Plural + " " + "do Sócio " + objEntity.MemberName + " (Nº: " + objEntity.MemberNumber + ") adicionado com sucesso.";
                            MessageBox.Show(sInfo, "Pagamento" + " adicionado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        return true;
                    }
                    else if (lOpStatus == -1)
                    {
                        if (bShowMessage)
                        {
                            String sError = "Ocorreu um erro na introdução do " + "Pagamento de " + this.Entity_Desc_Plural + " do Sócio " + objEntity.MemberName + " (Nº: " + objEntity.MemberNumber + ")!";
                            MessageBox.Show(sError, "Erro Criação Sócio", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            if (!SetDetails(objEntity))
                            {
                                Clear_Details(true);
                                XtraMessageBox.Show("Não foi possível obter os detalhes do " + "Pagamento de " + this.Entity_Desc_Plural + "!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                Load_Grid_Info(false, false, false, 0, -1);
                            }
                        }
                        return false;
                    }
                    else if (lOpStatus == 0)
                    {
                        if (!SetDetails(objEntity))
                        {
                            if (bShowMessage)
                            {
                                Clear_Details(true);
                                XtraMessageBox.Show("Erro a carregar Detalhes do " + "Pagamento de " + this.Entity_Desc_Plural, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                    String sInfo = "Selecione um " + "Pagamento de " + this.Entity_Desc_Plural + " na grelha para editar!";
                    MessageBox.Show(sInfo, "Pagamento" + " não selecionado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                #endregion  Check Selected 

                #region     Set Controls to Edit
                Set_Details_Editability(true);
                LayoutControlGroupAdmin.Visibility = LayoutVisibility.Never;
                LayoutControlGroupAdd.Visibility = LayoutVisibility.Never;
                LayoutControlGroupEdit.Visibility = LayoutVisibility.Always;
                LayoutControlGroup_Payment.Visibility = LayoutVisibility.Never;
                #endregion  Set Controls to Edit

                #region     Get Member Info to Edit
                AMFC_Entity objEntity = GetDetailsToEdit();
                if (objEntity == null)
                {
                    String sError = "Ocorreu um erro a obter os dados do " + "Pagamento de " + this.Entity_Desc_Plural + " do Sócio " + objEntity.MemberName + " (Nº: " + objEntity.MemberNumber + ")!";
                    MessageBox.Show(sError, "Erro Criação Sócio", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                #endregion  Get Member Info to Edit

                #region     Set Member Info to Edit
                if (!SetDetails(objEntity))
                {
                    Clear_Details(true);
                    XtraMessageBox.Show("Não foi possível obter carregar detalhes do " + "Pagamento de " + this.Entity_Desc_Plural + "!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                LayoutControlGroup_Payment.Visibility = LayoutVisibility.Never;


                #region     Member Info
                AMFC_Entity objEntity = GetDetailsEdited();
                if (objEntity == null)
                {
                    String sError = "Ocorreu um erro a obter os dados do " + "Pagamento de " + this.Entity_Desc_Plural + " do Sócio " + objEntity.MemberName + " (Nº: " + objEntity.MemberNumber + ")!";
                    MessageBox.Show(sError, "Erro Criação Sócio", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                #endregion  Member Info

                #region     Validate Data 
                if (CheckIsValid(objEntity, true) < 1)
                    return;
                #endregion  Validate Data

                #region     Edit Operation
                if (!DBF_AMFC_Edit(objEntity, true))
                    return;
                #endregion  Edit Operation

                this.Selected_Entidade = objEntity;
                Set_Details_Editability(false);
                LayoutControlGroup_Payment.Visibility = LayoutVisibility.Always;
                LayoutControlGroupAdd.Visibility = LayoutVisibility.Never;
                LayoutControlGroupAdmin.Visibility = LayoutVisibility.Always;
                this.Grid_Datasouce_Type = GridDatasourceType.MEMBER;
                Load_Grid_Info(true, true, true, -1, objEntity.Id);
                Load_Entity_Details(objEntity.Id);
                this.Grid_View.Focus();

                Value_Changed();
                Value_ToPay_Changed();

                if (!objEntity.Paid && objEntity.Value > 0)
                    Pay();
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
                LayoutControlGroup_Payment.Visibility = LayoutVisibility.Always;
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
            try
            {
                Reload_Details();
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return;
            }
        }

        /// <versions>08-10-2017(v0.0.4.9)</versions>
        private Boolean DBF_AMFC_Edit(AMFC_Entity objEntity, Boolean bShowMessageDialog)
        {
            try
            {
                Int64 lOpStatus = -1;
                using (Library_AMFC_SQL obj_AMFC_SQL = new Library_AMFC_SQL())
                    lOpStatus = obj_AMFC_SQL.Edit_QUOTA(objEntity);
                if (lOpStatus == 1 && bShowMessageDialog)
                {
                    String sInfo = "Pagamento de " + this.Entity_Desc_Plural+ " do Sócio " + objEntity.MemberName + " (Nº: " + objEntity.MemberNumber + ") alterado com sucesso.";
                    MessageBox.Show(sInfo, "Pagamento" + " alterado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return true;
                }
                else if (lOpStatus == -1)
                {
                    String sError = "Ocorreu um erro na alteração do " + "Pagamento de " + this.Entity_Desc_Plural + " do Sócio " + objEntity.MemberName + " (Nº: " + objEntity.MemberNumber + ")!";
                    MessageBox.Show(sError, "Erro Criação Sócio", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    if (!SetDetails(objEntity))
                    {
                        Clear_Details(true);
                        XtraMessageBox.Show("Não foi possível obter carregar od detalhes do " + "Pagamento de " + this.Entity_Desc_Plural + "!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Load_Grid_Info(false, false, false, 0, -1);
                    }
                    return false;
                }
                else if (lOpStatus == 0)
                {
                    if (!SetDetails(objEntity))
                    {
                        Clear_Details(true);
                        XtraMessageBox.Show("Erro a carregar od detalhes do " + "Pagamento de " + this.Entity_Desc_Plural, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
                DialogResult objDialogResult = XtraMessageBox.Show("Deseja eliminar do " + "Pagamento de " + this.Entity_Desc_Plural + "?", "Eliminar " + "Pagamento" + "?", MessageBoxButtons.OK, MessageBoxIcon.Question);
                if (objDialogResult != DialogResult.OK)
                    return;
                #endregion Del Confirmation

                #region     Check Selected 
                if (this.Grid_View.FocusedRowHandle < 0)
                {
                    String sInfo = "Selecione um " + "Pagamento de " + this.Entity_Desc_Plural + " na grelha para eliminar!";
                    MessageBox.Show(sInfo, "Pagamento" + " não selecionado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                #endregion  Check Selected 

                #region     Get Member ID
                AMFC_Entity objEntity = GetDetailsToEdit();
                if (objEntity == null)
                {
                    String sError = "Ocorreu um erro a obter os dados do " + "Pagamento de " + this.Entity_Desc_Plural + " do Sócio " + objEntity.MemberName + " (Nº: " + objEntity.MemberNumber + ")!";
                    MessageBox.Show(sError, "Erro Criação Sócio", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (objEntity.Id < 1)
                {
                    String sError = "ID do " + "Pagamento de " + this.Entity_Desc_Plural + " inválido: " + objEntity.Id;
                    MessageBox.Show(sError, "ID Inválido", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                #endregion  Get Member ID

                #region     Delete Operation
                if (!DBF_AMFC_Del(objEntity))
                {
                    String sError = "Não foi possível eliminar o " + "Pagamento de " + this.Entity_Desc_Plural + " inválido: " + objEntity.Id;
                    MessageBox.Show(sError, "Erro Eliminação", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        private Boolean DBF_AMFC_Del(AMFC_Entity objEntity)
        {
            try
            {
                Int64 lOpStatus = -1;
                using (Library_AMFC_SQL obj_AMFC_SQL = new Library_AMFC_SQL())
                    lOpStatus = obj_AMFC_SQL.Del_QUOTA(objEntity);
                if (lOpStatus == 1)
                {
                    String sInfo = "Pagamento de " + this.Entity_Desc_Plural + "ID = " + objEntity.Id + " eliminado com sucesso.";
                    MessageBox.Show(sInfo, "Pagamento" + " Eliminado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.Grid_Datasouce_Type = GridDatasourceType.MEMBER;
                    Load_Grid_Info(true, true, true, 0, -1);
                    return true;
                }
                else
                {
                    String sError = "Ocorreu um erro na eliminação do " + "Pagamento de " + this.Entity_Desc_Plural + " Nº = " + objEntity.Id + "!";
                    MessageBox.Show(sError, "Erro Eliminação", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
        private AMFCCashPayment GetPayDetails(AMFC_Entity objEntidade)
        {
            try
            {
                if (objEntidade == null)
                    return null;

                AMFCCashPayment objMemberPay = new AMFCCashPayment();

                objMemberPay.Payment_Type = AMFCCashPayment.PaymentTypes.QUOTAS;

                #region     Get Pay Max ID
                Int32 iPayMaxId = DBF_AMFC_Members_GetMaxPayId();
                if (iPayMaxId < 1)
                    return null;
                #endregion  Get Pay Max ID

                objMemberPay.ID = Convert.ToInt64(iPayMaxId) + 1;
                if (objEntidade.MemberNumber > 0 && objEntidade.MemberNumber < new AMFCMember().MaxNumber)
                    objMemberPay.SOCIO = objEntidade.MemberNumber;
                if (Program.IsValidTextString(objEntidade.MemberName))
                    objMemberPay.NOME = objEntidade.MemberName.Trim();

                if (Program.IsValidCurrencyEuroValue(TextEdit_Value_Missing.Text.Trim()))
                    objEntidade.ValueMissing = Program.SetPayCurrencyEuroDoubleValue(TextEdit_Value_Missing.Text.Trim());
                
                Double dValor = (objEntidade.ValueMissing > 0) ? Program.SetPayCurrencyEuroDoubleValue(objEntidade.ValueMissing) : Program.SetPayCurrencyEuroDoubleValue(objEntidade.Value);
                objMemberPay.VALOR = dValor;

                objMemberPay.ALTERADO = Program.SetDateTimeValue(DateTime.Today, -1, -1);
                objMemberPay.DATA = Program.SetDateTimeValue(DateTime.Today, -1, -1);

                if (Program.IsValidTextString(TextEdit_PaidPerson.Text))
                    objMemberPay.NOME_PAG = TextEdit_PaidPerson.Text.Trim();

                objMemberPay.DESIGNACAO = "Pagamento de " + Entity_Desc_Plural + " de " + objEntidade.Year;

                objMemberPay.Payment_State = AMFCCashPayment.PaymentState.ABERTO;

                #region     Payment Entity
                objMemberPay.HasQUOTAS = true;
                objMemberPay.QUOTASDESC = "Quotas" + " " + objEntidade.Year;
                objMemberPay.QUOTASVAL = objEntidade.Value;
                objMemberPay.DASSOCQUOT = objMemberPay.DATA;
                #endregion  Payment Entity

                //objMemberPay.NOTAS = String.Empty;

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

        /// <versions>26-11-2017(GesAMFC-v0.0.4.37)</versions>
        private void Pay()
        {
            try
            {
                using (Library_AMFC_SQL obj_AMFC_SQL = new Library_AMFC_SQL())
                {
                    Form_Caixa objForm_Caixa = new Form_Caixa();
                    if (objForm_Caixa != null)
                    {
                        AMFC_Entity objEntidade = GetDetailsEdited();
                        if (objEntidade == null || objEntidade.MemberNumber < 1 || objEntidade.MemberNumber > new AMFCMember().MaxNumber || objEntidade.Id < 1)
                            return;

                        this.Entidade_Member = Get_DBF_AMFC_Member_By_Number(objEntidade.MemberNumber);
                        if (this.Entidade_Member == null || this.Entidade_Member.NUMERO <= 0 || this.Entidade_Member.NUMERO > this.Entidade_Member.MaxNumber)
                            return;
                        objForm_Caixa.PaymentMember = this.Entidade_Member;

                        AMFCCashPayment objPayment = new AMFCCashPayment();
                        if (obj_AMFC_SQL.Member_Payment_Open_Already_Exist(this.Entidade_Member.NUMERO)) //Pagamento em aberto
                        {
                            objPayment = obj_AMFC_SQL.Get_Member_Payment_Open(this.Entidade_Member.NUMERO);
                            if (objPayment == null || objPayment.ID < 1)
                                return;
                            Double dbCurretnValue = (objEntidade.ValueMissing > 0) ? objEntidade.ValueMissing : objEntidade.Value;
                            if (objPayment.QUOTASVAL <= 0)
                            {
                                objPayment.QUOTASDESC = objEntidade.Year;
                                objPayment.QUOTASVAL = dbCurretnValue;
                            }
                            else
                            {
                                objPayment.QUOTASDESC = Entity_Desc_Plural + ": " + objPayment.QUOTASDESC.Trim() + "; " + objEntidade.Year;
                                objPayment.QUOTASVAL += dbCurretnValue;
                            }
                            objPayment.SetItemPayment(AMFCCashPayment.PaymentTypes.QUOTAS, Program.SetPayCurrencyEuroDoubleValue(objPayment.QUOTASVAL), true, false);
                            objPayment.ALTERADO = Program.SetDateTimeValue(DateTime.Today, -1, -1);
                            objPayment.HasQUOTAS = true;
                            objPayment.DASSOCQUOT = Program.SetDateTimeValue(DateTime.Today, -1, -1);
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
                            objPayment.QUOTASVAL = (objEntidade.ValueMissing > 0) ? objEntidade.ValueMissing : objEntidade.Value;
                            objPayment.SetItemPayment(AMFCCashPayment.PaymentTypes.QUOTAS, Program.SetPayCurrencyEuroDoubleValue(objPayment.QUOTASVAL), true, false);
                            objPayment.Payment_State = AMFCCashPayment.PaymentState.FINALIZADO;
                            objForm_Caixa.PaymentOperationType = Library_AMFC_Methods.OperationType.ADD;
                            objPayment.Payment_Type = AMFCCashPayment.PaymentTypes.QUOTAS;
                            objForm_Caixa.CurrentPayment = objPayment;
                            objForm_Caixa.EntitiesPaymentType = objForm_Caixa.GetMultipleSinglePaymentType();
                        }
                        objForm_Caixa.FormClosing += delegate
                        {
                            if (objForm_Caixa.PaymentOk)
                            {
                                try
                                {
                                    if (objForm_Caixa.CurrentPayment.Payment_State == AMFCCashPayment.PaymentState.FINALIZADO)
                                    {
                                        objEntidade.AddValuePaid(Program.SetPayCurrencyEuroDoubleValue(objEntidade.Value.ToString().Trim()));
                                        TextEdit_Value_ToPay.Text = Program.SetPayCurrencyEuroStringValue(objEntidade.ValueToPay);
                                        TextEdit_Value_Paid.Text = Program.SetPayCurrencyEuroStringValue(objEntidade.ValuePaid);
                                        TextEdit_Value_Missing.Text = Program.SetPayCurrencyEuroStringValue(objEntidade.ValueMissing);
                                        TextEdit_Value_OnPaying.Text = Program.SetPayCurrencyEuroStringValue(objEntidade.ValueOnPaying);
                                    }
                                    else if (objForm_Caixa.CurrentPayment.Payment_State == AMFCCashPayment.PaymentState.ABERTO)
                                    {
                                        objEntidade.ValueOnPaying = Program.SetPayCurrencyEuroDoubleValue(objEntidade.Value.ToString().Trim());
                                        TextEdit_Value_ToPay.Text = Program.SetPayCurrencyEuroStringValue(objEntidade.ValueToPay);
                                        TextEdit_Value_Paid.Text = Program.SetPayCurrencyEuroStringValue(objEntidade.ValuePaid);
                                        TextEdit_Value_Missing.Text = Program.SetPayCurrencyEuroStringValue(objEntidade.ValueMissing);
                                        TextEdit_Value_OnPaying.Text = Program.SetPayCurrencyEuroStringValue(objEntidade.ValueOnPaying);
                                    }
                                    DateEdit_PayDate.DateTime = objForm_Caixa.CurrentPayment.DATA;
                                    TextEdit_PaidPerson.Text = objForm_Caixa.CurrentPayment.NOME_PAG;

                                    #region     Update Details
                                    objEntidade = Get_Pay_Details(objEntidade);
                                    if (objForm_Caixa.CurrentPayment.ID > 0)
                                        objEntidade.AddCaixaId(objForm_Caixa.CurrentPayment.ID);
                                    if (objForm_Caixa.CurrentPayment.Payment_State == AMFCCashPayment.PaymentState.FINALIZADO)
                                        objEntidade.Pay_State = AMFC_Entity.PayState.SIM;
                                    else
                                        objEntidade.Pay_State = AMFC_Entity.PayState.EM_PAGAMENTO;
                                    if (objEntidade == null)
                                        return;
                                    Pay_Edit_DB(objEntidade, true, false);
                                    this.Selected_Entidade = objEntidade;
                                    Set_PictureBox_Payment(objEntidade);
                                    #endregion  Update Details
                                }
                                catch { }
                            }
                            else //Delete Payment or Error 
                            {
                                try
                                {
                                    if (objForm_Caixa.CurrentPayment.Payment_State == AMFCCashPayment.PaymentState.CANCELED)
                                    {
                                        objEntidade.RemoveValuePaid(Program.SetPayCurrencyEuroDoubleValue(objForm_Caixa.CurrentPayment.QUOTASVAL.ToString().Trim()));
                                        TextEdit_Value_ToPay.Text = Program.SetPayCurrencyEuroStringValue(objEntidade.ValueToPay);
                                        TextEdit_Value_Paid.Text = Program.SetPayCurrencyEuroStringValue(objEntidade.ValuePaid);
                                        TextEdit_Value_Missing.Text = Program.SetPayCurrencyEuroStringValue(objEntidade.ValueMissing);
                                        TextEdit_Value_OnPaying.Text = Program.SetPayCurrencyEuroStringValue(objEntidade.ValueOnPaying);
                                    }

                                    objEntidade = Get_Pay_Details(objEntidade);
                                    if (objForm_Caixa.CurrentPayment.ID > 0)
                                        objEntidade.DelCaixaId(objForm_Caixa.CurrentPayment.ID);

                                    if (objEntidade == null)
                                        return;
                                    Pay_Edit_DB(objEntidade, true, false);
                                    this.Selected_Entidade = objEntidade;
                                    Set_PictureBox_Payment(objEntidade);
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

        /// <versions>26-11-2017(GesAMFC-v0.0.4.37)</versions>
        private Int64 Get_Pay_Last_Id(AMFC_Entity objEntidade)
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
                Int64 lPayId = -1;
                AMFC_Entity objEntidade = GetDetailsEdited();
                if (objEntidade == null || objEntidade.MemberNumber < 1 || objEntidade.MemberNumber > new AMFCMember().MaxNumber || objEntidade.Id < 1)
                    return;

                if (objEntidade.ListaCaixaIDs.Count == 0)
                {
                    String sWarning = "Não foi possivel obter o ID do Pagamento!";
                    MessageBox.Show(sWarning, "Erro: Nº de " + "Pagamento", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                lPayId = Get_Pay_Last_Id(objEntidade);
                if (lPayId < 1)
                {
                    String sWarning = "Não foi possivel obter o Pagamento!";
                    MessageBox.Show(sWarning, "Erro: " + "Pagamento", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                using (Library_AMFC_SQL obj_AMFC_SQL = new Library_AMFC_SQL())
                {
                    Form_Caixa objForm_Caixa = new Form_Caixa();
                    if (objForm_Caixa != null)
                    {                      

                        this.Entidade_Member = Get_DBF_AMFC_Member_By_Number(objEntidade.MemberNumber);
                        if (this.Entidade_Member == null || this.Entidade_Member.NUMERO <= 0 || this.Entidade_Member.NUMERO > this.Entidade_Member.MaxNumber)
                            return;
                        objForm_Caixa.PaymentMember = this.Entidade_Member;

                        AMFCCashPayment objPayment = obj_AMFC_SQL.Get_Member_Payment_By_Id(this.Entidade_Member.NUMERO, lPayId);
                        if (objPayment == null || objPayment.ID < 1)
                            return;

                        objPayment.QUOTASVAL = (objEntidade.ValueMissing > 0) ? objEntidade.ValueMissing : objPayment.QUOTASVAL;
                        objPayment.VALOR = 0;
                        objPayment.SetItemPayment(AMFCCashPayment.PaymentTypes.QUOTAS, Program.SetPayCurrencyEuroDoubleValue(objPayment.QUOTASVAL), true, false);

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
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, false, false);
            }
        }

        /// <versions>19-11-2017(GesAMFC-v0.0.4.33)</versions>
        private void Pay_Edit_DB(AMFC_Entity objEntity, Boolean bRelaodGrid, Boolean bShowMessageDialog)
        {
            try
            {
                Set_Details_Editability(false);
                LayoutControlGroupEdit.Visibility = LayoutVisibility.Never;
                LayoutControlGroupAdmin.Visibility = LayoutVisibility.Always;
                LayoutControlGroup_Payment.Visibility = LayoutVisibility.Never;

                #region     Member Info
                if (objEntity == null)
                {
                    String sError = "Ocorreu um erro a obter os dados do " + "Pagamento de " + this.Entity_Desc_Plural + " do Sócio " + objEntity.MemberName + " (Nº: " + objEntity.MemberNumber + ")!";
                    MessageBox.Show(sError, "Erro Pagamento Sócio", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                #endregion  Member Info

                #region     Validate Data 
                if (CheckIsValid(objEntity, true) < 1)
                    return;
                #endregion  Validate Data

                #region     Edit Operation
                if (!DBF_AMFC_Edit(objEntity, bShowMessageDialog))
                    return;
                #endregion  Edit Operation

                if (bRelaodGrid)
                {
                    this.Selected_Entidade = objEntity;
                    Set_PictureBox_Payment(objEntity);
                    Set_Details_Editability(false);
                    LayoutControlGroup_Payment.Visibility = LayoutVisibility.Always;
                    LayoutControlGroupAdd.Visibility = LayoutVisibility.Never;
                    LayoutControlGroupAdmin.Visibility = LayoutVisibility.Always;
                    Load_Grid_Info(true, true, true, -1, objEntity.Id);
                    Load_Entity_Details(objEntity.Id);
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
        private AMFC_Entity Get_Pay_Details(AMFC_Entity objEntity)
        {
            try
            {
                if (objEntity.Id <= 1)
                    objEntity.Id = Convert.ToInt64(TextEdit_Id.Text.Trim());

                if (objEntity.MemberNumber < 1 || objEntity.MemberNumber > new AMFCMember().MaxNumber)
                    objEntity.MemberNumber = Convert.ToInt64(TextEdit_MemberNumber.Text.Trim());

                if (Program.IsValidTextString(objEntity.MemberName))
                    objEntity.MemberName = TextEdit_MemberName.Text.Trim();

                objEntity.DtMemberAdmiDate = Program.SetDateTimeValue(DateEdit_MemberAdmiDate.DateTime, -1, -1);

                if (!Program.IsValidCurrencyEuroValue(objEntity.Value))
                    objEntity.Value = Program.SetPayCurrencyEuroDoubleValue(TextEdit_Value.Text.Trim());

                objEntity.YearInt = Convert.ToInt32((ComboBoxEdit_Year.SelectedItem as ComboboxItem).GetValue());
                objEntity.DtDate = new DateTime(objEntity.YearInt, 1, 1);

                if (Program.IsValidTextString(objEntity.Notas))
                    objEntity.Notas = TextEdit_Notas.Text.Trim();

                objEntity.DtDatePaid = Program.SetDateTimeValue(DateEdit_PayDate.DateTime, -1, -1);

                if (Program.IsValidTextString(objEntity.PaidPerson))
                    objEntity.PaidPerson = TextEdit_PaidPerson.Text.Trim();

                if (Program.IsValidTextString(objEntity.ListaCaixa))
                    objEntity.ListaCaixa = TextEdit_Hidden_Pay_Caixa_ListIDs.Text.Trim();

                return objEntity;
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

        #endregion  Pay 

        #endregion  Actions Methods

        /// <versions>28-11-2017(GesAMFC-v0.0.4.39)</versions>
        public AMFC_Entity DBF_Get_Member_Quota_ById(Int64 lId)
        {
            try
            {
                using (Library_AMFC_SQL obj_AMFC_SQL = new Library_AMFC_SQL())
                {
                    Int32 iYear = GetCurrentYear();
                    return obj_AMFC_SQL.Get_Member_QUOTA_ById(lId, iYear);
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
                //iYear = TExtYear
                return iYear;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return -1;
            }
        }

        /// Colocar no Program
        /// <versions>01-12-2017(GesAMFC-v0.0.4.41)</versions>
        private void SetYearComboList()
        {
            try
            {
                Int32 iSelecteIndex = 0;
                Int32 iIndex = 0;
                ComboBoxEdit_Year.Properties.Items.Clear();
                foreach (AMFCYear objYear in ListYears.List)
                {
                    ComboboxItem objComboBoxItem = new ComboboxItem(objYear.Value, objYear.Description);
                    if (objYear.Value == DateTime.Today.Year)
                        iSelecteIndex = iIndex;
                    ComboBoxEdit_Year.Properties.Items.Add(objComboBoxItem);
                    iIndex++;
                }
                //ComboBoxEdit_Year.SelectedIndex = iSelecteIndex;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// Colocar no Program
        /// <versions>01-12-2017(GesAMFC-v0.0.4.41)</versions>
        private Int32 GeComboListItemIndexByValue(ComboBoxEdit objComboBoxEdit, Int32 iCheckValue)
        {
            try
            {
                for (Int32 iIndex = 0; iIndex < objComboBoxEdit.Properties.Items.Count; iIndex++)
                {
                    ComboboxItem objComboBoxItem = (ComboboxItem)objComboBoxEdit.Properties.Items[iIndex];
                    Int32 iValue = Convert.ToInt32((objComboBoxItem as ComboboxItem).GetValue());
                    if (iValue == iCheckValue)
                        return iIndex;
                }
                return -1;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return -1;
            }
        }

        /// <versions>05-12-2017(GesAMFC-v0.0.4.41)</versions>
        private void Pay_Member_Period()
        {
            try
            {
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
                                Pay_Member_Period_Quotas();
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

        /// <versions>05-12-2017(GesAMFC-v0.0.4.41)</versions>
        private void Pay_Member_Period_Quotas()
        {
            try
            {
                Form_Period_Years objFormPeriod = new Form_Period_Years(this.PeriodSelected);
                if (objFormPeriod != null)
                {
                    objFormPeriod.FormClosing += delegate
                    {
                        if (objFormPeriod.IsPeriodSelected)
                        {
                            if  (
                                    Program.IsValidYear(objFormPeriod.PeriodSelected.Start.Year.Value) 
                                    &&
                                    Program.IsValidYear(objFormPeriod.PeriodSelected.End.Year.Value)
                                )
                            {
                                #region     Loading Panel While Datasource Bind
                                //this.SplashScreenManager_LoadingGrid.ShowWaitForm();
                                //try
                                //{
                                //    this.SplashScreenManager_LoadingGrid.SetWaitFormCaption("Aguarde");
                                //    this.SplashScreenManager_LoadingGrid.SetWaitFormDescription("A Registasr Período de Pagamentos  ...");
                                //    Thread.Sleep(this.LoadingGridPanelWaitTime);

                                    this.PeriodSelected = objFormPeriod.PeriodSelected;
                                    Add_Member_Period_Multiple_Quotas();
                                //}
                                //finally
                                //{
                                //    this.SplashScreenManager_LoadingGrid.CloseWaitForm();
                                //}
                                #endregion  Loading Panel While Datasource Bind
                            }
                        }
                    };
                    objFormPeriod.Show();
                    objFormPeriod.StartPosition = FormStartPosition.CenterParent;
                    objFormPeriod.Focus();
                    objFormPeriod.BringToFront();
                    objFormPeriod.Update();
                }
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>05-12-2017(GesAMFC-v0.0.4.41)</versions>
        private void Add_Member_Period_Multiple_Quotas()
        {
            try
            {
                #region Check
                //this.Entidade_Member =
                //this.PeriodSelected =
                #endregion Check

                #region     Get Max ID
                Int64 lId = DBF_AMFC_Members_GetMaxId();
                if (lId < 1)
                    lId = 1;
                #endregion  Get Max ID

                AMFC_Entities objListEntitiesToAdd = new AMFC_Entities();
                for (Int32 iYear = this.PeriodSelected.Start.Year.Value; iYear <= this.PeriodSelected.End.Year.Value; iYear++)
                {
                    lId++;
                    AMFC_Entity objEntity = Get_Entity_To_Add(iYear, lId);
                    if (CheckIsValid(objEntity, true) < 1)
                        continue;
                    objListEntitiesToAdd.Add(objEntity);
                }

                AMFC_Entities objListEntitiesToPay = new AMFC_Entities();
                foreach (AMFC_Entity objEntity in objListEntitiesToAdd.Entidades)
                {
                    if (!DBF_AMFC_Add(objEntity, false))
                    {
                        //error
                        continue;
                    }
                    objListEntitiesToPay.Add(objEntity);
                    
                }
                this.Selected_Entidade = objListEntitiesToPay.Entidades[0];

                if (objListEntitiesToAdd.Entidades.Count > 0)
                    Pay_Member_Period_Multiple_Quotas(objListEntitiesToAdd);

                this.Grid_Datasouce_Type = GridDatasourceType.MEMBER;
                Load_Grid_Info(true, true, true, -1, this.Selected_Entidade.Id);
                Load_Entity_Details(this.Selected_Entidade.Id);
                this.Grid_View.Focus();                
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return;
            }
        }

        /// <versions>05-12-2017(GesAMFC-v0.0.4.41)</versions>
        private AMFC_Entity Get_Entity_To_Add(Int32 iYear, Int64 lId)
        {
            try
            {
                #region Check
                //this.Entidade_Member =
                //this.PeriodSelected =
                //Int32 iYear, Int64 lId
                #endregion Check

                AMFC_Entity objEntity = new AMFC_Entity();
                objEntity.Id = lId;
                if (this.Entidade_Member.NUMERO > 0 && this.Entidade_Member.NUMERO < new AMFCMember().MaxNumber)
                    objEntity.MemberNumber = this.Entidade_Member.NUMERO;
                if (Program.IsValidTextString(this.Entidade_Member.NOME))
                    objEntity.MemberName = this.Entidade_Member.NOME.Trim();
                if (this.Current_Entity_Value > 0)
                {
                    objEntity.Value = Program.SetPayCurrencyEuroDoubleValue(this.Current_Entity_Value);
                    objEntity.ValueToPay = Program.SetPayCurrencyEuroDoubleValue(this.Current_Entity_Value);
                    objEntity.ValuePaid = Program.Default_Pay_Value;
                    objEntity.ValueMissing = Program.SetPayCurrencyEuroDoubleValue(this.Current_Entity_Value);
                }

                objEntity.YearInt = iYear;
                objEntity.DtDate = new DateTime(objEntity.YearInt, 1, 1);

                objEntity.DtDatePaid = Program.Default_Date;
                if (Program.IsValidTextString(this.Entidade_Member.NOME))
                    objEntity.PaidPerson = this.Entidade_Member.NOME.Trim();
                objEntity.Notas = String.Empty;

                return objEntity;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return null;
            }
        }

        /// <versions>05-12-2017(GesAMFC-v0.0.4.41)</versions>
        private void Pay_Member_Period_Multiple_Quotas(AMFC_Entities objListEntitiesToPay)
        {
            try
            {
                using (Library_AMFC_SQL obj_AMFC_SQL = new Library_AMFC_SQL())
                {
                    Form_Caixa objForm_Caixa = new Form_Caixa();
                    if (objForm_Caixa != null)
                    {
                        objForm_Caixa.PaymentMember = this.Entidade_Member;

                        AMFCCashPayment objPayment = new AMFCCashPayment();
                        objPayment = GetPeriodPayMultipleDetails(objListEntitiesToPay); //Novo pagamento Multi Period Quotas
                        if (objPayment == null || objPayment.ID < 1)
                            return;
                        objPayment.Payment_Type = AMFCCashPayment.PaymentTypes.QUOTAS;


                        //DialogResult objResult_MemberMoneyDelivered = XtraMessageBox.Show("Deseja efetuar o pagamento?", "Finalizar pagamento ?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                        //if (objResult_MemberMoneyDelivered == DialogResult.Yes)
                        //{
                            objForm_Caixa.CurrentPayment.Payment_State = AMFCCashPayment.PaymentState.FINALIZADO;
                        //}

                        objForm_Caixa.PaymentOperationType = Library_AMFC_Methods.OperationType.ADD;
                        objForm_Caixa.CurrentPayment = objPayment;
                        objForm_Caixa.EntitiesPaymentType = objForm_Caixa.GetMultipleSinglePaymentType();

                        objForm_Caixa.FormClosing += delegate
                        {
                            if (objForm_Caixa.PaymentOk)
                            {
                                try
                                {
                                    #region     Update Each Quota Pay Details
                                    foreach (AMFC_Entity objEntityToPay in objListEntitiesToPay.Entidades)
                                    {
                                        objEntityToPay.DtDatePaid = Program.SetDateTimeValue(objForm_Caixa.CurrentPayment.DATA, -1, -1);
                                        objEntityToPay.AddValuePaid(Program.SetPayCurrencyEuroDoubleValue(objEntityToPay.Value.ToString().Trim()));
                                        AMFC_Entity objEntityPaid = objEntityToPay;
                                        if (objForm_Caixa.CurrentPayment.ID > 0)
                                            objEntityPaid.AddCaixaId(objForm_Caixa.CurrentPayment.ID);
                                        objEntityPaid.Pay_State = AMFC_Entity.PayState.SIM;
                                        Pay_Edit_DB(objEntityPaid, false, false);
                                    }
                                    #endregion  Update Each Quota Pay Details

                                    Load_Grid_Info(true, true, true, -1, objListEntitiesToPay.Entidades[0].Id);
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

        /// <versions>05-12-2017(GesAMFC-v0.0.4.41)</versions>
        private AMFCCashPayment GetPeriodPayMultipleDetails(AMFC_Entities objListEntitiesToPay)
        {
            try
            {
                if (objListEntitiesToPay == null || objListEntitiesToPay.Entidades == null || objListEntitiesToPay.Entidades.Count == 0)
                    return null;

                AMFCCashPayment objMemberPay = new AMFCCashPayment();

                objMemberPay.Payment_Type = AMFCCashPayment.PaymentTypes.QUOTAS;

                #region     Get Pay Max ID
                Int32 iPayMaxId = DBF_AMFC_Members_GetMaxPayId();
                if (iPayMaxId < 1)
                    return null;
                #endregion  Get Pay Max ID

                objMemberPay.ID = Convert.ToInt64(iPayMaxId) + 1;
                objMemberPay.SOCIO = this.Entidade_Member.NUMERO;
                objMemberPay.NOME = this.Entidade_Member.NOME;

                Double dValor = 0;
                String sDesc = objListEntitiesToPay.Entidades[0].Year + " a " + objListEntitiesToPay.Entidades[objListEntitiesToPay.Entidades.Count - 1].Year + "." + " ("  + objListEntitiesToPay.Entidades.Count + " " + "anos" + ") ";
                foreach (AMFC_Entity objEntityToPay in objListEntitiesToPay.Entidades)
                {
                    if (objEntityToPay.Value > 0)
                        dValor += objEntityToPay.Value;
                }
                objMemberPay.VALOR = dValor;

                objMemberPay.ALTERADO = Program.SetDateTimeValue(DateTime.Today, -1, -1);
                objMemberPay.DATA = Program.SetDateTimeValue(DateTime.Today, -1, -1);
                objMemberPay.NOME_PAG = this.Entidade_Member.NOME;

                objMemberPay.DESIGNACAO = "Pagamento de " + Entity_Desc_Plural + " de " + sDesc;

                objMemberPay.Payment_State = AMFCCashPayment.PaymentState.ABERTO;

                #region     Payment Entity
                if (dValor > 0 && !String.IsNullOrEmpty(sDesc))
                {
                    objMemberPay.HasQUOTAS = true;
                    objMemberPay.QUOTASDESC = "Quotas" + " " + sDesc;
                    objMemberPay.QUOTASVAL = dValor;
                    objMemberPay.SetItemPayment(AMFCCashPayment.PaymentTypes.QUOTAS, Program.SetPayCurrencyEuroDoubleValue(objMemberPay.QUOTASVAL), true, false);
                    objMemberPay.DASSOCQUOT = objMemberPay.DATA;
                }
                #endregion  Payment Entity

                //objMemberPay.NOTAS = String.Empty;

                return objMemberPay;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return null;
            }
        }

        #endregion  Methods

        private void Button_Create_Receipt_Click(object sender, EventArgs e)
        {
            try
            {
                if (!Get_Member_ByNumber())
                    return;
                Int32 iYear = Convert.ToInt32((ComboBoxEdit_Year.SelectedItem as ComboboxItem).GetValue());
                Form_Recibo_Quotas objFormReciboQuotas = new Form_Recibo_Quotas(this.Entidade_Member, new AMFCYear(iYear));
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

        /// <versions>07-05-2017(v0.0.3.3)</versions>
        private Boolean Get_Member_ByNumber()
        {
            try
            {
                #region     Check Fields
                if (String.IsNullOrEmpty(this.TextEdit_MemberNumber.Text.Trim()))
                {
                    XtraMessageBox.Show("Por favor, selecione a quota na grelha!", "Selcione quota", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return false;
                }
                #endregion  Check Fields

                #region     Find Member by Number
                if (!String.IsNullOrEmpty(this.TextEdit_MemberNumber.Text.Trim()))
                {
                    try
                    {
                        Int64 lMemberNumber = -1;
                        if (Int64.TryParse(this.TextEdit_MemberNumber.Text.Trim().TrimStart('0'), out lMemberNumber))
                        {
                            AMFCMember objMemberFound = Get_DBF_AMFC_Member_By_Number(lMemberNumber);
                            if (objMemberFound != null && objMemberFound.NUMERO >= objMemberFound.MinNumber && objMemberFound.NUMERO <= objMemberFound.MaxNumber && !String.IsNullOrEmpty(objMemberFound.NOME) && !String.IsNullOrEmpty(objMemberFound.NUMLOTE))
                            {
                                this.Entidade_Member = objMemberFound;
                                return true;
                            }
                        }
                        else
                        {
                            XtraMessageBox.Show("Insira um número de sócio inteior válido!", "ERRO [Nº Sócio Inválido]", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            this.Focus();
                            return false;
                        }
                    }
                    catch (Exception ex1)
                    {
                        XtraMessageBox.Show("Não foi possível encontrar o sócio! Por favor, introduza um Número de Sócio inteiro válido.", "ERRO [Procurar Sócio]", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        this.Focus();
                        Program.WriteLog(ex1.TargetSite.Name, ex1.Message, true, true, true, true);
                        return false;
                    }
                }
                #endregion  Find Member by Number

                return false;
            }
            catch (Exception ex)
            {
                Program.WriteLog(ex.TargetSite.Name, ex.Message, true, true, true, true);
                return false;
            }
        }
    }
}