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

namespace GesAMFC
{
    /// <summary>Members Joias Admin</summary>
    /// <author>Valter Lima</author>
    /// <creation>18-02-2017(v0.0.1.0)</creation>
    /// <versions>21-11-2017(GesAMFC-v0.0.4.35)</versions>
    public partial class Admin_Joias : DevExpress.XtraEditors.XtraForm
    {
        public Library_AMFC_Methods LibAMFC;

        private AMFCMemberJoia  _SelectedJoia;
        private AMFCMember _SelectedMember;

        private String _GridColJoiaId = "JoiaId";
        private String _GridColMemberNumber = "MemberNumber";

        private Double Current_Joia_Value = Program.Get_Current_Parameter_JOIA_Value();

        #region     Form Constructor 

        /// <versions>05-05-2017(v0.0.3.2)</versions>
        public Admin_Joias()
        {
            LibAMFC = new Library_AMFC_Methods();   

            try
            {
                //_MembersJoias = new AMFCMemberJoias();
                _SelectedJoia = new AMFCMemberJoia();
                _SelectedMember = new AMFCMember();

                InitializeComponent();

                #region     User Permissions Set Controls
                //String sPermAdmin = "Permissão de Administrador";
                #endregion  User Permissions Set Controls

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
        private void Admin_Joias_Load(object sender, EventArgs e)
        {
            try
            {
                this.WindowState = FormWindowState.Maximized;
                this.Update();

                #region     Set DateEdits Calendars
                Program.SetCalendarControl(DateEdit_MemberAdmiDate);
                Program.SetCalendarControl(DateEdit_JoiaInsertDate);
                Program.SetCalendarControl(DateEdit_JoiaPayDate);
                #endregion  Set DateEdits Calendars

                #region     Set Currency Euro Edit Values
                Program.SetPayEditValues(TextEdit_JoiaValue);
                Program.SetPayEditValues(TextEdit_Value_ToPay);
                Program.SetPayEditValues(TextEdit_Value_Paid);
                Program.SetPayEditValues(TextEdit_Value_Missing);
                #endregion  Set Currency Euro Edit Values

                LibAMFC.GridConfiguration(this.Grid_Control, this.Grid_View, true, false, true, true, true);

                Int32 iInitRouHandle = 0;
                Load_Grid(true, true, true, true, true, true, iInitRouHandle, -1);
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
                    Clear_Joia_Details(true);
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
                    Clear_Joia_Details(true);
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
                        Boolean bPaid = Convert.ToBoolean(this.Grid_View.GetRowCellValue(e.RowHandle, this.Grid_View.Columns["JoiaPaid"]));
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
            Find_Member();
        }

        /// <versions>06-10-2017(v0.0.4.8)</versions>
        private void Button_Joia_Add_Click(object sender, EventArgs e)
        {
            Button_Add_Joia_Click();
        }

        /// <versions>06-10-2017(v0.0.4.8)</versions>
        private void Button_Joia_Add_Save_Click(object sender, EventArgs e)
        {
            Button_Add_Save_Click();
        }

        /// <versions>06-10-2017(v0.0.4.8)</versions>
        private void Button_Joia_Add_Cancel_Click(object sender, EventArgs e)
        {
            Button_Add_Cancel_Click();
        }

        /// <versions>06-10-2017(v0.0.4.8)</versions>
        private void Button_Joia_Add_Repor_Click(object sender, EventArgs e)
        {
            Button_Add_Repor_Click();
        }

        /// <versions>06-10-2017(v0.0.4.8)</versions>
        private void Button_Joia_Edit_Click(object sender, EventArgs e)
        {
            Button_Edit_Joia_Click();
        }

        /// <versions>06-10-2017(v0.0.4.8)</versions>
        private void Button_Joia_Edit_Save_Click(object sender, EventArgs e)
        {
            Button_Edit_Save_Click();
        }

        /// <versions>06-10-2017(v0.0.4.8)</versions>
        private void Button_Joia_Edit_Cancel_Click(object sender, EventArgs e)
        {
            Button_Edit_Cancel_Click();
        }

        /// <versions>06-10-2017(v0.0.4.8)</versions>
        private void Button_Joia_Edit_Repor_Click(object sender, EventArgs e)
        {
            Button_Edit_Repor_Click();
        }

        /// <versions>08-10-2017(v0.0.4.8)</versions>
        private void TextEdit_JoiaValue_Click(object sender, EventArgs e)
        {
            if (TextEdit_JoiaValue.Properties.ReadOnly)
                Joia_Edit_Action();
        }

        /// <versions>08-10-2017(v0.0.4.8)</versions>
        private void TextEdit_JoiaInsertDate_Click(object sender, EventArgs e)
        {
            if (DateEdit_JoiaInsertDate.Properties.ReadOnly)
                Joia_Edit_Action();
        }

        /// <versions>08-10-2017(v0.0.4.8)</versions>
        private void TextEdit_JoiaPayDate_Click(object sender, EventArgs e)
        {
            if (DateEdit_JoiaPayDate.Properties.ReadOnly)
                Joia_Edit_Action();
        }

        /// <versions>08-10-2017(v0.0.4.8)</versions>
        private void TextEdit_JoiaPaidPerson_Click(object sender, EventArgs e)
        {
            if (TextEdit_JoiaPaidPerson.Properties.ReadOnly)
                Joia_Edit_Action();
        }

        /// <versions>08-10-2017(v0.0.4.8)</versions>
        private void TextEdit_Joia_Notas_Click(object sender, EventArgs e)
        {
            if (TextEdit_Joia_Notas.Properties.ReadOnly)
                Joia_Edit_Action();
        }

        /// <versions>06-10-2017(v0.0.4.8)</versions>
        private void Button_Joia_Del_Click(object sender, EventArgs e)
        {
            Button_Del_Click();
        }

        /// <versions>06-10-2017(v0.0.4.8)</versions>
        private void Button_Pay_Click(object sender, EventArgs e)
        {
            Pay_Joia();
        }

        private void Button_Show_Pay_Click(object sender, EventArgs e)
        {
            Open_Joia();
        }

        #endregion  Action Buttons Events

        #region     Details Changed Events

        private void TextEdit_JoiaValue_TextChanged(object sender, EventArgs e)
        {
            JoiaValue_Changed();
        }

        private void TextEdit_Value_ToPay_TextChanged(object sender, EventArgs e)
        {
            Value_ToPay_Changed();
        }

        private void TextEdit_Value_Paid_TextChanged(object sender, EventArgs e)
        {
            Value_Paid_Changed();
        }

        private void JoiaValue_Changed()
        {
            try
            {
                AMFCMemberJoia objMemberJoia = GetJoiaDetailsToEdit();
                objMemberJoia.ValueToPay = Program.SetPayCurrencyEuroDoubleValue(objMemberJoia.GetValueToPay());
                TextEdit_Value_ToPay.Text = Program.SetPayCurrencyEuroStringValue(objMemberJoia.ValueToPay);
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
                    AMFCMemberJoia objMemberJoia = GetJoiaDetailsToEdit();
                    objMemberJoia.ValueMissing = Program.SetPayCurrencyEuroDoubleValue(objMemberJoia.GetValueMissing());
                    TextEdit_Value_Missing.Text = Program.SetPayCurrencyEuroStringValue(objMemberJoia.ValueMissing);
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
                    AMFCMemberJoia objMemberJoia = GetJoiaDetailsToEdit();
                    objMemberJoia.ValueMissing = Program.SetPayCurrencyEuroDoubleValue(objMemberJoia.GetValueMissing());
                    TextEdit_Value_Missing.Text = Program.SetPayCurrencyEuroStringValue(objMemberJoia.ValueMissing);
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

        /// <versions>07-10-2017(v0.0.4.8)</versions>
        private void Load_Grid(Boolean bSetCols, Boolean bClearSorting, Boolean bClearFilters, Boolean bClearGrouping, Boolean bForceMemberDetailsDataLoad, Boolean bSetTotals, Int32 iJoiaFocusedRowHandle, Int64 lJoiaId)
        {
            try
            {
                //LibAMFC.GridConfiguration(this.Grid_Control, this.Grid_View, true, false, true, true, true);

                Grid_Clear_Selection();

                Load_Grid_Datasource(bSetCols, bClearSorting, bClearFilters, bClearGrouping, bSetTotals, true);

                #region     Config Grids Options
                this.Grid_View.OptionsSelection.EnableAppearanceFocusedRow = true;
                this.Grid_View.OptionsSelection.EnableAppearanceFocusedCell = false;
                this.Grid_View.OptionsSelection.EnableAppearanceHideSelection = true;
                this.Grid_View.OptionsSelection.UseIndicatorForSelection = false;
                this.Grid_View.ClearSelection();
                #endregion  Config Grids Options

                #region     Set Focused Joia
                if (lJoiaId > 0)
                {
                    Int32 iRowHandle = this.Grid_View.LocateByValue(_GridColJoiaId, lJoiaId);
                    if (iRowHandle != GridControl.InvalidRowHandle)
                        this.Grid_View.FocusedRowHandle = iRowHandle;
                }
                else if (iJoiaFocusedRowHandle > -1)
                {
                    if (iJoiaFocusedRowHandle != GridControl.InvalidRowHandle)
                        this.Grid_View.FocusedRowHandle = iJoiaFocusedRowHandle;
                }
                #endregion  Set Focused Joia

                return;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

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

        /// <versions>05-05-2017(v0.0.3.2)</versions>
        private void Load_Grid_Datasource(Boolean bSetCols, Boolean bClearSorting, Boolean bClearFilters, Boolean bClearGrouping, Boolean bSetTotals, Boolean bWriteLog)
        {
            try
            {
                LibAMFC.CleanGrid(this.Grid_Control, this.Grid_View, bSetCols, bClearSorting, bClearFilters, bClearGrouping);
                this.Grid_Control.Visible = false;
                this.Update();

                Boolean bLoadDatasource = Set_Grid_Data_Source(this.Grid_Control);

                if (bLoadDatasource)
                {
                    if (bSetCols)
                        Set_Grid_Columns();
                    if (bSetTotals)
                        Set_Grid_Totals_Summaries();
                }

                this.Grid_Control.Visible = true;
                this.Update();
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>21-10-2017(v0.0.4.17)</versions>
        private Boolean Set_Grid_Data_Source(GridControl objGridControl)
        {
            StackFrame objStackFrame = new StackFrame();
            String sErrorMsg = String.Empty;
            try
            {
                #region         Members Datasource
                AMFCMemberJoias objAMFCMemberJoias = new AMFCMemberJoias();
                using (Library_AMFC_SQL obj_AMFC_SQL = new Library_AMFC_SQL())
                    objAMFCMemberJoias = obj_AMFC_SQL.Get_All_Member_Joias();
                if (objAMFCMemberJoias == null)
                {
                    sErrorMsg = "Não foi possível obter a Lista de Pagamentos de Joias!";
                    Program.HandleError(objStackFrame.GetMethod().Name, sErrorMsg, Program.ErroType.ERROR, true, false);
                    return false;
                }
                if (objAMFCMemberJoias.Joias.Count == 0)
                {
                    String sWarningMsg = "Não existem Pagamentos de Joias!";
                    MessageBox.Show(sWarningMsg, "No results", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                #endregion  Members Datasource

                #region     TreeDataSet
                DataTable objDataTableSource = new DataTable("AMFC_Members_Joias");
                #region     Data Columns Creation
                DataColumn objDataColumn_JoiaId                         = new DataColumn(_GridColJoiaId,                typeof(Int64));
                DataColumn objDataColumn_MemberNumber                   = new DataColumn(_GridColMemberNumber,          typeof(Int64));
                DataColumn objDataColumn_MemberName                     = new DataColumn("MemberName",                  typeof(String));
                DataColumn objDataColumn_MemberAdmiDate                 = new DataColumn("MemberAdmiDate",              typeof(String));
                DataColumn objDataColumn_JoiaDate                       = new DataColumn("JoiaDate",                    typeof(String));
                DataColumn objDataColumn_JoiaYear                       = new DataColumn("JoiaYear",                    typeof(String));
                DataColumn objDataColumn_JoiaMonth                      = new DataColumn("JoiaMonth",                   typeof(String));
                DataColumn objDataColumn_JoiaMonthYear                  = new DataColumn("JoiaMonthYear",               typeof(String));
                DataColumn objDataColumn_JoiaValue                      = new DataColumn("JoiaValue",                   typeof(Double));
                DataColumn objDataColumn_JoiaPaid                       = new DataColumn("JoiaPaid",                    typeof(Boolean));
                DataColumn objDataColumn_PaidOrNot = new DataColumn("PaidOrNot",               typeof(String));
                DataColumn objDataColumn_PayState = new DataColumn("PayState", typeof(String));
                DataColumn objDataColumn_JoiaPaidPerson                 = new DataColumn("JoiaPaidPerson",              typeof(String));
                DataColumn objDataColumn_JoiaNotas                      = new DataColumn("JoiaNotas",                   typeof(String));
                DataColumn objDataColumn_JoiaListaCaixa                 = new DataColumn("JoiaListaCaixa",              typeof(String));
                DataColumn objDataColumn_JoiaListaRecibos               = new DataColumn("JoiaListaRecibos",            typeof(String));
                //DataColumn objDataColumn_MemberAdmiDT                 = new DataColumn("DtMemberAdmiDate",            typeof(DateTime));
                //DataColumn objDataColumn_JoiaDT                       = new DataColumn("DtJoiaDate",                  typeof(DateTime));
                //DataColumn objDataColumn_DtJoiaDataPagamentoAgregado  = new DataColumn("DtJoiaDataPagamentoAgregado", typeof(DateTime));
                //DataColumn objDataColumn_JoiaDataPagamentoAgregado    = new DataColumn("JoiaDataPagamentoAgregado",   typeof(String));


                #endregion  Data Columns Creation
                #region     Data Table Add Columns
                objDataTableSource.Columns.AddRange(
                                            new DataColumn[] {
                                                objDataColumn_JoiaId,
                                                objDataColumn_MemberNumber,
                                                objDataColumn_MemberAdmiDate,
                                                objDataColumn_MemberName,
                                                objDataColumn_JoiaDate,
                                                objDataColumn_JoiaYear,
                                                objDataColumn_JoiaMonth,
                                                objDataColumn_JoiaMonthYear,
                                                objDataColumn_JoiaValue,
                                                objDataColumn_JoiaPaid,
                                                objDataColumn_PaidOrNot,
                                                objDataColumn_PayState,
                                                objDataColumn_JoiaPaidPerson,
                                                objDataColumn_JoiaNotas,
                                                objDataColumn_JoiaListaCaixa,
                                                objDataColumn_JoiaListaRecibos
                                                //objDataColumn_MemberAdmiDT,
                                                //objDataColumn_JoiaDT,
                                                //objDataColumn_DtJoiaDataPagamentoAgregado,
                                                //objDataColumn_JoiaDataPagamentoAgregado,
                                            }
                                        );
                #endregion  Data Table Add Columns
                #region     Data Columns Dispose
                objDataColumn_JoiaId.Dispose();
                objDataColumn_MemberNumber.Dispose();
                objDataColumn_MemberName.Dispose();
                objDataColumn_MemberAdmiDate.Dispose();
                objDataColumn_JoiaDate.Dispose();
                objDataColumn_JoiaYear.Dispose();
                objDataColumn_JoiaMonth.Dispose();
                objDataColumn_JoiaMonthYear.Dispose();
                objDataColumn_JoiaValue.Dispose();
                objDataColumn_JoiaPaid.Dispose();
                objDataColumn_PaidOrNot.Dispose();
                objDataColumn_PayState.Dispose();
                objDataColumn_JoiaPaidPerson.Dispose();
                objDataColumn_JoiaNotas.Dispose();
                objDataColumn_JoiaListaCaixa.Dispose();
                objDataColumn_JoiaListaRecibos.Dispose();
                //objDataColumn_MemberAdmiDT.Dispose();
                //objDataColumn_JoiaDT.Dispose();
                //objDataColumn_DtJoiaDataPagamentoAgregado.Dispose();
                //objDataColumn_JoiaDataPagamentoAgregado.Dispose();
                #endregion  Data Columns Dispose
                #endregion  TreeDataSet

                #region     Binding Data
                foreach (AMFCMemberJoia objJoia in objAMFCMemberJoias.Joias)
                {
                    //if (objJoia == null || objJoia.MemberNumber < 1 || objJoia.MemberNumber > new AMFCMember().MaxNumber || objJoia.JoiaId < 1 || objJoia.JoiaValue < 0)
                    if (objJoia == null || objJoia.MemberNumber < 1 || objJoia.MemberNumber > new AMFCMember().MaxNumber || objJoia.JoiaId < 1)
                        continue;
                    #region     Set Member Row Data
                    DataRow objDataRow = objDataTableSource.NewRow();
                    objDataRow[_GridColJoiaId]                  = objJoia.JoiaId;
                    objDataRow[_GridColMemberNumber]            = objJoia.MemberNumber;
                    objDataRow["MemberName"]                    = objJoia.MemberName;
                    objDataRow["MemberAdmiDate"]                = objJoia.MemberAdmiDate;
                    objDataRow["JoiaDate"]                      = objJoia.JoiaDatePaid;
                    objDataRow["JoiaYear"]                      = objJoia.JoiaYear;
                    objDataRow["JoiaMonth"]                     = objJoia.JoiaMonth;
                    objDataRow["JoiaMonthYear"]                 = objJoia.JoiaMonthYear;
                    objDataRow["JoiaValue"]                     = objJoia.JoiaValue;
                    objDataRow["JoiaPaid"]                      = objJoia.JoiaPaid;
                    objDataRow["PaidOrNot"]                     = objJoia.PaidOrNot;
                    objDataRow["PayState"] = objJoia.Pay_State.ToString();
                    objDataRow["JoiaPaidPerson"]                = objJoia.JoiaPaidPerson;
                    objDataRow["JoiaNotas"]                     = objJoia.JoiaNotas;
                    objDataRow["JoiaListaCaixa"]                = objJoia.JoiaListaCaixa;
                    objDataRow["JoiaListaRecibos"]              = objJoia.JoiaListaRecibos;
                    //objDataRow["DtMemberAdmiDate"]              = objJoia.DtMemberAdmiDate;
                    //objDataRow["DtJoiaDate"]                    = objJoia.DtJoiaDate;
                    //objDataRow["DtJoiaDataPagamentoAgregado"]   = objJoia.DtJoiaDataPagamentoAgregado;
                    //objDataRow["JoiaDataPagamentoAgregado"]     = objJoia.JoiaDataPagamentoAgregado;


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

        /// <versions>15-06-2017(v0.0.3.18)</versions>
        public void Set_Grid_Totals_Summaries()
        {
            try
            {
                this.Grid_View.GroupFooterShowMode = GroupFooterShowMode.VisibleAlways;

                GridGroupSummaryItem objGroupTotalJoias = new GridGroupSummaryItem();
                objGroupTotalJoias.SummaryType = DevExpress.Data.SummaryItemType.Count;
                objGroupTotalJoias.FieldName = "JoiaValue";
                this.Grid_View.GroupSummary.Add(objGroupTotalJoias);

                GridGroupSummaryItem objGroupTotalJoiasValues = new GridGroupSummaryItem();
                objGroupTotalJoiasValues.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                objGroupTotalJoiasValues.FieldName = "JoiaValue";
                objGroupTotalJoiasValues.DisplayFormat = "Total = {0:c2}";
                objGroupTotalJoiasValues.ShowInGroupColumnFooter = this.Grid_View.Columns["JoiaValue"];
                this.Grid_View.GroupSummary.Add(objGroupTotalJoiasValues);

                //---

                this.Grid_View.OptionsView.ShowFooter = true;

                GridColumnSummaryItem objTotalGlobalJoiasValues = new GridColumnSummaryItem();
                objTotalGlobalJoiasValues.SummaryType = DevExpress.Data.SummaryItemType.Sum;
                objTotalGlobalJoiasValues.FieldName = "JoiaValue";
                objTotalGlobalJoiasValues.DisplayFormat = "Total Global = {0:c2}";
                this.Grid_View.Columns["JoiaValue"].Summary.Add(objTotalGlobalJoiasValues);
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>21-10-2017(v0.0.4.17)</versions>
        private void Set_Grid_Columns_Editability()
        {
            try
            {
                LibAMFC.SetGridColumnOptions(this.Grid_Control, this.Grid_View, _GridColJoiaId,                 "Nº Joia",                      "Nº da Joia",                       false, -1,  100, false, false, true,    false,      HorzAlignment.Center,  VertAlignment.Center,      HorzAlignment.Far,     VertAlignment.Center);
                LibAMFC.SetGridColumnOptions(this.Grid_Control, this.Grid_View, _GridColMemberNumber,           "Nº Sócio",                     "Número de sócio",                  true,   2,  120, true,  false, true,    false,      HorzAlignment.Far,     VertAlignment.Center,      HorzAlignment.Far,     VertAlignment.Center);
                LibAMFC.SetGridColumnOptions(this.Grid_Control, this.Grid_View, "MemberName",                   "Nome",                         "Nome do sócio",                    true,   3,  300, true,  false, true,    false,      HorzAlignment.Near,    VertAlignment.Center,      HorzAlignment.Near,    VertAlignment.Center);
                LibAMFC.SetGridColumnOptions(this.Grid_Control, this.Grid_View, "MemberAdmiDate",               "Data Admissão",                "Data Admissão o Sócio",            true,   4,   80, true,  false, true,     true,      HorzAlignment.Center,  VertAlignment.Center,      HorzAlignment.Center,  VertAlignment.Center);
                LibAMFC.SetGridColumnOptions(this.Grid_Control, this.Grid_View, "JoiaDate",                     "Data Pagamento",               "Data de Pagamento da Joia",        true,   5,   87, true,  false, true,     true,      HorzAlignment.Center,  VertAlignment.Center,      HorzAlignment.Center,  VertAlignment.Center);
                LibAMFC.SetGridColumnOptions(this.Grid_Control, this.Grid_View, "JoiaMonthYear",                "Mês/Ano",                      "Mês/Ano de Pagamento da Joia",     true,   6,   58, true,  false, true,     true,      HorzAlignment.Center,  VertAlignment.Center,      HorzAlignment.Center,  VertAlignment.Center);
                LibAMFC.SetGridColumnOptions(this.Grid_Control, this.Grid_View, "JoiaValue",                    "Valores Joia",                 "Valores da Joia de Admissão",      true,   7,  150, true,  false, true,     true,      HorzAlignment.Center,  VertAlignment.Center,      HorzAlignment.Far,     VertAlignment.Center, true);
                LibAMFC.SetGridColumnOptions(this.Grid_Control, this.Grid_View, "PaidOrNot",                "Pago / Não Pago",              "Joias Pagas / Joias em atraso",    true,   8,   80, true,  false, true,     false, -1,   HorzAlignment.Center,  VertAlignment.Center,      HorzAlignment.Center,  VertAlignment.Center);
                LibAMFC.SetGridColumnOptions(this.Grid_Control, this.Grid_View, "JoiaYear",                     "Ano",                          "Ano de Pagamento da Joia",         true,   9,   80, true,  false, true,     true, 0,   HorzAlignment.Center,  VertAlignment.Center,      HorzAlignment.Center,  VertAlignment.Center);
                LibAMFC.SetGridColumnOptions(this.Grid_Control, this.Grid_View, "JoiaMonth",                    "Mês",                          "Mês de Pagamento da Joia",         false, -1,   60, true,  false, true,     true,      HorzAlignment.Center,  VertAlignment.Center,      HorzAlignment.Center,  VertAlignment.Center);
                LibAMFC.SetGridColumnOptions(this.Grid_Control, this.Grid_View, "JoiaPaidPerson",               "Pagou",                        "Pessoa que pagou",                 false, -1,  220, true,  false, true,    false,      HorzAlignment.Center,  VertAlignment.Center,      HorzAlignment.Center,  VertAlignment.Center);
                LibAMFC.SetGridColumnOptions(this.Grid_Control, this.Grid_View, "JoiaPaid",                     "Pago",                         "Joia Paga / Joias Não Paga",       false, -1,   80, true,  false, true,    false,      HorzAlignment.Center,  VertAlignment.Center,      HorzAlignment.Center,  VertAlignment.Center);
                LibAMFC.SetGridColumnOptions(this.Grid_Control, this.Grid_View, "JoiaNotas",                    "Notas",                        "Observações",                      false, -1,  220, true,  false, true,    false,      HorzAlignment.Center,  VertAlignment.Center,      HorzAlignment.Center,  VertAlignment.Center);
                LibAMFC.SetGridColumnOptions(this.Grid_Control, this.Grid_View, "JoiaListaCaixa",               "JoiaListaCaixa",               "JoiaListaCaixa",                   false, -1,   80, true,  false, true,     true,      HorzAlignment.Center,  VertAlignment.Center,      HorzAlignment.Center,  VertAlignment.Center);
                LibAMFC.SetGridColumnOptions(this.Grid_Control, this.Grid_View, "JoiaListaRecibos",             "JoiaListaRecibos",             "JoiaListaRecibos",                 false, -1,   80, true,  false, true,     true,      HorzAlignment.Center,  VertAlignment.Center,      HorzAlignment.Center,  VertAlignment.Center);
                LibAMFC.SetGridColumnOptions(this.Grid_Control, this.Grid_View, "PayState", "PayState", "PayState", false, -1,  80, true,  false, true,     true,      HorzAlignment.Center,  VertAlignment.Center,      HorzAlignment.Center,  VertAlignment.Center);
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>21-10-2017(v0.0.4.17)</versions>
        private void Set_Grid_Columns_OptionsFilter()
        {
            try
            {
                LibAMFC.SetGridColumOptionsFilter(this.Grid_View, _GridColJoiaId,                   true, true, AutoFilterCondition.Equals,     8.0f);
                LibAMFC.SetGridColumOptionsFilter(this.Grid_View, _GridColMemberNumber,             true, true, AutoFilterCondition.Equals,     8.0f);
                LibAMFC.SetGridColumOptionsFilter(this.Grid_View, "MemberName",                     true, true, AutoFilterCondition.Contains,   8.5f);
                LibAMFC.SetGridColumOptionsFilter(this.Grid_View, "MemberAdmiDate",                 true, true, AutoFilterCondition.Equals,     8.5f);
                LibAMFC.SetGridColumOptionsFilter(this.Grid_View, "JoiaDate",                       true, true, AutoFilterCondition.Contains,   8.5f);
                LibAMFC.SetGridColumOptionsFilter(this.Grid_View, "DtJoiaDate",                     true, true, AutoFilterCondition.Equals,     8.5f);
                LibAMFC.SetGridColumOptionsFilter(this.Grid_View, "JoiaMonthYear",                  true, true, AutoFilterCondition.Equals,     8.5f);
                LibAMFC.SetGridColumOptionsFilter(this.Grid_View, "JoiaValue",                      true, true, AutoFilterCondition.Contains,   8.5f);
                LibAMFC.SetGridColumOptionsFilter(this.Grid_View, "JoiaYear",                       true, true, AutoFilterCondition.Equals,     8.5f);
                LibAMFC.SetGridColumOptionsFilter(this.Grid_View, "JoiaMonth",                      true, true, AutoFilterCondition.Equals,     8.5f);
                LibAMFC.SetGridColumOptionsFilter(this.Grid_View, "JoiaPaidPerson",                 true, true, AutoFilterCondition.Equals,     8.5f);
                LibAMFC.SetGridColumOptionsFilter(this.Grid_View, "JoiaNotas",                      true, true, AutoFilterCondition.Equals,     8.5f);
                LibAMFC.SetGridColumOptionsFilter(this.Grid_View, "JoiaPaid",                       true, true, AutoFilterCondition.Equals,     8.5f);
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
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
                    Clear_Joia_Details(true);
                    return false;
                }
                #endregion  Check if is a valid Data Row Handle

                #region     Get Focused Joia
                Boolean bIsTheSameMember = IsTheSameSelected(iRowHandle, -1);
                AMFCMemberJoia objMemberJoia = null;
                if (!bIsTheSameMember)
                    objMemberJoia = Get_Selected(iRowHandle, -1);
                else
                    objMemberJoia = _SelectedJoia;
                if (objMemberJoia == null || objMemberJoia.MemberNumber < 1 || objMemberJoia.MemberNumber > new AMFCMember().MaxNumber || objMemberJoia.JoiaId < 1)
                {
                    XtraMessageBox.Show(objStackFrame.GetMethod().Name, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                #endregion  Get Focused JOia

                #region     Load Member Joia Details
                if (!bIsTheSameMember)
                    Load_Member_Joia_Details(objMemberJoia.JoiaId);
                #endregion  Load Member Joia Details

                return true;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return false;
            }
        }

        /// <versions>07-10-2017(v0.0.4.8)</versions>
        private AMFCMemberJoia Get_Selected(Int32 iRowHandle, Int64 lJoiaId)
        {
            try
            {
                AMFCMemberJoia objMemberJoia = null;
                Int32 iFocusedRowHandle = -1;
                Int64 lFocusedJoiaNumber = -1;

                if (lJoiaId < 1)
                {
                    if (iRowHandle > -1)
                        iFocusedRowHandle = iRowHandle;
                    else
                        iFocusedRowHandle = this.Grid_View.FocusedRowHandle;
                    if (iFocusedRowHandle < 0 || iFocusedRowHandle == GridControl.InvalidRowHandle || !this.Grid_View.IsDataRow(iFocusedRowHandle))
                    {
                        XtraMessageBox.Show("Não foi possível obter os detalhes da joia do sócio selecionado!", "Erro [Joia Selecionada]", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return null;
                    }
                    lFocusedJoiaNumber = Convert.ToInt64(this.Grid_View.GetRowCellValue(iFocusedRowHandle, this.Grid_View.Columns[_GridColJoiaId]));
                }
                else
                    lFocusedJoiaNumber = lJoiaId;

                if (lFocusedJoiaNumber == _SelectedJoia.JoiaId)
                    return _SelectedJoia;
                using (Library_AMFC_SQL obj_AMFC_SQL = new Library_AMFC_SQL())
                    objMemberJoia = obj_AMFC_SQL.Get_Member_Joia_ByJoiaId(lFocusedJoiaNumber);

                if(objMemberJoia == null || objMemberJoia.MemberNumber < 1 || objMemberJoia.MemberNumber > new AMFCMember().MaxNumber || objMemberJoia.JoiaId < 1)
                {
                    XtraMessageBox.Show("Não foi possível obter os detalhes da joia do sócio selecionado!", "Erro [Joia Selecionada]", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
                this._SelectedJoia = objMemberJoia;
                return objMemberJoia;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return null;
            }
        }
        
        /// <versions>15-06-2017(v0.0.3.17)</versions>
        private Boolean IsTheSameSelected(Int32 iRowHandle, Int64 lJoiaId)
        {
            try
            {
                Int32 iFocusedRowHandle = -1;
                Int64 lFocusedJoiaId = -1;

                if (lJoiaId < 1)
                {
                    if (iRowHandle > -1)
                        iFocusedRowHandle = iRowHandle;
                    else
                        iFocusedRowHandle = this.Grid_View.FocusedRowHandle;
                    if (iFocusedRowHandle < 0 || iFocusedRowHandle == GridControl.InvalidRowHandle || !this.Grid_View.IsDataRow(iFocusedRowHandle))
                    {
                        XtraMessageBox.Show("Não foi possível obter os detalhes do joia do sócio selecionada!", "Erro [Joia Selecionada]", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    lFocusedJoiaId = Convert.ToInt64(this.Grid_View.GetRowCellValue(iFocusedRowHandle, this.Grid_View.Columns[_GridColJoiaId]));
                }
                else
                    lFocusedJoiaId = lJoiaId;
                if (lFocusedJoiaId == _SelectedJoia.JoiaId)
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

        #endregion  Grid Methods

        #region     Actions Methods

        /// <versions>07-10-2017(v0.0.4.8)</versions>
        private void Find_Member()
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
                                    Load_Search_Member_Info(objMemberSelected.NUMERO);
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

        /// <versions>07-10-2017(v0.0.4.8)</versions>
        private void Load_Search_Member_Info(Int64 lMemberNumber)
        {
            try
            {
                if (lMemberNumber < 1 || lMemberNumber > new AMFCMember().MaxNumber)
                {
                    Clear_Joia_Details(true);
                    XtraMessageBox.Show("Não foi possível obter o sócio!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                AMFCMemberJoia objMemberJoia = null;
                using (Library_AMFC_SQL obj_AMFC_SQL = new Library_AMFC_SQL())
                    objMemberJoia = obj_AMFC_SQL.Get_Member_Joia_ByMemberNbr(lMemberNumber);
                if (objMemberJoia == null || objMemberJoia.JoiaId < 1)
                {
                    Clear_Joia_Details(true);
                    XtraMessageBox.Show("Não foi possível obter o Id da Joia!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                Load_Grid(false, false, false, false, true, false, -1, objMemberJoia.JoiaId);
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return;
            }
        }

        /// <versions>06-10-2017(v0.0.4.7)</versions>
        private void Load_Member_Joia_Details(Int64 lJoiaId)
        {
            try
            {
                #region     Validate Member Number
                if (lJoiaId < 1)
                {
                    Clear_Joia_Details(true);
                    return;
                }
                #endregion  Validate Member Number

                #region     Get Focused Member
                AMFCMemberJoia objMemberJoia = Get_Selected(-1, lJoiaId);
                if (objMemberJoia == null || objMemberJoia.MemberNumber < 1 || objMemberJoia.MemberNumber > new AMFCMember().MaxNumber || objMemberJoia.JoiaId < 1)
                {
                    Clear_Joia_Details(true);
                    XtraMessageBox.Show("Não foi possível obter joia selecionada!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                #endregion  Get Focused Member

                #region     Set Joia Details Controls
                Set_Joia_Details_Editability(false);

                LayoutControlGroup_Joia_Admin.Visibility = LayoutVisibility.Always;
                LayoutControlGroup_Joia_Add.Visibility = LayoutVisibility.Never;
                LayoutControlGroup_Joia_Edit.Visibility = LayoutVisibility.Never;
                LayoutControlGroup_Payment.Visibility = LayoutVisibility.Always;

                this.LayoutControlItem_Pay.Visibility = (!objMemberJoia.JoiaPaid && objMemberJoia.JoiaValue > 0) ? LayoutVisibility.Always : LayoutVisibility.Never;
                #endregion  Set Joia Details Controls

                #region     Load Joia Details
                if (objMemberJoia != null)
                {
                    if (!SetJoiaDetails(objMemberJoia))
                    {
                        Clear_Joia_Details(true);
                        XtraMessageBox.Show("Não foi possível obter carregar detalhes da joia!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                #endregion  Load Joia Details

                return;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>08-10-2017(v0.0.4.8)</versions>
        private void Clear_Joia_Details(Boolean bClearJoiaId)
        {
            try
            {
                if (bClearJoiaId)
                    this.TextEdit_JoiaId.Text = String.Empty;
                this.TextEdit_MemberNumber.Text = String.Empty;
                this.TextEdit_MemberName.Text = String.Empty;
                this.DateEdit_MemberAdmiDate.DateTime = Program.Default_Date;
                this.TextEdit_JoiaValue.Text = Program.Default_Pay_String;
                if (this.PictureBox_Payment.Image != null)
                {
                    this.PictureBox_Payment.Image.Dispose();
                    this.PictureBox_Payment.Image = null;
                }
                
                this.DateEdit_JoiaInsertDate.DateTime = Program.Default_Date;
                this.TextEdit_Joia_Notas.Text = String.Empty;

                this.TextEdit_Value_ToPay.Text = Program.Default_Pay_String;
                this.TextEdit_Value_Paid.Text = Program.Default_Pay_String;
                this.TextEdit_Value_Missing.Text = Program.Default_Pay_String;

                this.DateEdit_JoiaPayDate.DateTime = Program.Default_Date;
                this.TextEdit_JoiaPaidPerson.Text = String.Empty;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>08-10-2017(v0.0.4.8)</versions>
        private void Set_Joia_Details_Editability(Boolean bCanEdit)
        {
            try
            {
                Boolean bIsReadOnly = !bCanEdit;
                TextEdit_JoiaId.Properties.ReadOnly = true; //always
                TextEdit_MemberNumber.Properties.ReadOnly = true; //always
                TextEdit_MemberName.Properties.ReadOnly = true; //always
                DateEdit_MemberAdmiDate.Properties.ReadOnly = true; //always
                TextEdit_JoiaValue.Properties.ReadOnly = bIsReadOnly;
                
                DateEdit_JoiaInsertDate.Properties.ReadOnly = bIsReadOnly;
                TextEdit_Joia_Notas.Properties.ReadOnly = bIsReadOnly;

                TextEdit_Value_ToPay.Properties.ReadOnly = true;
                TextEdit_Value_Paid.Properties.ReadOnly = true;
                TextEdit_Value_Missing.Properties.ReadOnly = true;
                DateEdit_JoiaPayDate.Properties.ReadOnly = true;
                TextEdit_JoiaPaidPerson.Properties.ReadOnly = true;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return;
            }
        }
        
        /// <versions>08-10-2017(v0.0.4.8)</versions>
        private Int32 CheckJoiaIsValid(AMFCMemberJoia objMemberJoia, Boolean bCheckJoiaId)
        {
            try
            {
                #region     Joia ID
                if (bCheckJoiaId)
                {
                    if (objMemberJoia.JoiaId < 1)
                    {
                        String sError = "O " + "Nº da Joia" + ": " + objMemberJoia.JoiaId + " não é válido! Por favor, modifique.";
                        MessageBox.Show(sError, "Nº da Joia Inválido", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return -1;
                    }
                }
                #endregion  Joia ID

                #region     Nº Sócio
                if (objMemberJoia.MemberNumber < 1 || objMemberJoia.MemberNumber > new AMFCMember().MaxNumber)
                {
                    String sError = "O " + "Nº de Sócio: " + objMemberJoia.MemberNumber + " não é válido! Por favor, modifique.";
                    MessageBox.Show(sError, "Sócio Inválido", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return -1;
                }
                #endregion  Nº Sócio

                #region     Nome
                if (!Program.IsValidTextString(objMemberJoia.MemberName))
                {
                    String sWarning = "O " + "Nome do Sócio" + " (" + objMemberJoia.MemberName.Trim() + ") não é válido! Por favor, modifique.";
                    MessageBox.Show(sWarning, "Nome Sócio" + " Invalído", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return -1;
                }
                #endregion  Nome

                #region     Member Admission Date
                if (objMemberJoia.DtMemberAdmiDate == null)
                {
                    String sWarning = "A " + "data de admissão" + " do Sócio (" + objMemberJoia.MemberAdmiDate.Trim() + ") não é válida! Por favor, modifique.";
                    MessageBox.Show(sWarning, "Data de admissão" + " Invalída", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return -1;
                }
                #endregion  Member Admission Date

                #region     Joia Value
                if (objMemberJoia.JoiaValue <= 0)
                {
                    String sWarning = "O " + "valor da joia" + " (" + objMemberJoia.JoiaValue.ToString() + ") não é válido! Por favor, modifique.";
                    MessageBox.Show(sWarning, "Valor da joia" + " Invalído", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return -1;
                }
                #endregion  Joia Value

                #region     Joia Date Paid
                if (objMemberJoia.DtJoiaDatePaid == null)
                {
                    String sWarning = "A " + "data de pagamento" + " (" + objMemberJoia.JoiaDatePaid.Trim() + ") não é válida! Por favor, modifique.";
                    MessageBox.Show(sWarning, "Data Pagamento" + " Invalída", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return -1;
                }
                #endregion  Joia Date Paid

                #region     Joia Paid Person
                if (!Program.IsValidTextString(objMemberJoia.JoiaPaidPerson))
                {
                    String sWarning = "O " + "nome da pessoa que efetuou o pagamento" + " (" + objMemberJoia.JoiaPaidPerson.Trim() + ") não é válido! Por favor, modifique.";
                    MessageBox.Show(sWarning, "" + "Nome de que pagou invalído", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return -1;
                }
                #endregion  Joia Paid Person

                return 1;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return -1;
            }
        }

        /// <versions>08-10-2017(v0.0.4.8)</versions>
        private AMFCMemberJoia GetJoiaDetailsToAdd(AMFCMember objMember)
        {
            try
            {
                AMFCMemberJoia objMemberJoia = new AMFCMemberJoia();
                #region     Get Joia Max ID
                Int32 iJoiaMaxId = DBF_AMFC_Members_GetMaxJoiaId();
                if (iJoiaMaxId < 1)
                    return null;
                #endregion  Get Joia Max ID
                objMemberJoia.JoiaId = Convert.ToInt64(iJoiaMaxId) + 1;
                if (objMember.NUMERO > 0 && objMember.NUMERO < new AMFCMember().MaxNumber)
                    objMemberJoia.MemberNumber = objMember.NUMERO;
                if (Program.IsValidTextString(objMember.NOME))
                    objMemberJoia.MemberName = objMember.NOME.Trim();
                if (Program.IsValidTextString(objMember.DATAADMI))
                    objMemberJoia.DtMemberAdmiDate = Program.ConvertToValidDateTime(objMember.DATAADMI.Trim());
                if (this.Current_Joia_Value > 0)
                {
                    objMemberJoia.JoiaValue = Program.SetPayCurrencyEuroDoubleValue(this.Current_Joia_Value);
                    objMemberJoia.ValueToPay = Program.SetPayCurrencyEuroDoubleValue(this.Current_Joia_Value);
                    objMemberJoia.ValuePaid = Program.Default_Pay_Value;
                    objMemberJoia.ValueMissing = Program.SetPayCurrencyEuroDoubleValue(this.Current_Joia_Value);
                }
                objMemberJoia.DtJoiaDate = Program.Default_Date;
                objMemberJoia.DtJoiaDatePaid = Program.Default_Date;
                if (Program.IsValidTextString(objMember.NOME))
                    objMemberJoia.JoiaPaidPerson = objMember.NOME.Trim();
                objMemberJoia.JoiaNotas = String.Empty;

                return objMemberJoia;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return null;
            }
        }

        /// <versions>08-10-2017(v0.0.4.8)</versions>
        private void Joia_Edit_Action()
        {
            try
            {
                DialogResult objDialogResult = XtraMessageBox.Show("Deseja editar a joia?", "Editar joia?", MessageBoxButtons.OK, MessageBoxIcon.Question);
                if (objDialogResult == DialogResult.OK)
                    Button_Edit_Joia_Click();
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return;
            }
        }

        /// <versions>08-10-2017(v0.0.4.8)</versions>
        private AMFCMemberJoia GetJoiaDetailsToEdit()
        {
            try
            {
                AMFCMemberJoia objMemberJoia = new AMFCMemberJoia();

                if (!String.IsNullOrEmpty(TextEdit_JoiaId.Text.Trim()) && Convert.ToInt64(TextEdit_JoiaId.Text.Trim()) > 0)
                    objMemberJoia.JoiaId = Convert.ToInt64(TextEdit_JoiaId.Text.Trim());

                if (!String.IsNullOrEmpty(TextEdit_MemberNumber.Text.Trim()) && Convert.ToInt64(TextEdit_MemberNumber.Text.Trim()) > 0 && Convert.ToInt64(TextEdit_MemberNumber.Text.Trim()) < new AMFCMember().MaxNumber)
                    objMemberJoia.MemberNumber = Convert.ToInt64(TextEdit_MemberNumber.Text.Trim());

                if (Program.IsValidTextString(TextEdit_MemberName.Text))
                    objMemberJoia.MemberName = TextEdit_MemberName.Text.Trim();

                objMemberJoia.DtMemberAdmiDate = Program.SetDateTimeValue(DateEdit_MemberAdmiDate.DateTime, -1, -1 );

                if (Program.IsValidCurrencyEuroValue(TextEdit_JoiaValue.Text.Trim()))
                    objMemberJoia.JoiaValue = Program.SetPayCurrencyEuroDoubleValue(TextEdit_JoiaValue.Text.Trim());
                else if (this.Current_Joia_Value > 0)
                    objMemberJoia.JoiaValue = Program.SetPayCurrencyEuroDoubleValue(this.Current_Joia_Value);

                objMemberJoia.DtJoiaDate = Program.SetDateTimeValue(DateEdit_JoiaInsertDate.DateTime, -1, -1);
                
                if (Program.IsValidTextString(TextEdit_Joia_Notas.Text))
                    objMemberJoia.JoiaNotas = TextEdit_Joia_Notas.Text.Trim();

                objMemberJoia.DtJoiaDatePaid = Program.SetDateTimeValue(DateEdit_JoiaPayDate.DateTime, -1, -1);

                if (Program.IsValidTextString(TextEdit_JoiaPaidPerson.Text))
                    objMemberJoia.JoiaPaidPerson = TextEdit_JoiaPaidPerson.Text.Trim();
                else if (Program.IsValidTextString(TextEdit_MemberName.Text))
                    objMemberJoia.MemberName = TextEdit_MemberName.Text.Trim();

                if (Program.IsValidCurrencyEuroValue(TextEdit_Value_ToPay.Text.Trim()))
                    objMemberJoia.ValueToPay = Program.SetPayCurrencyEuroDoubleValue(TextEdit_Value_ToPay.Text.Trim());

                if (Program.IsValidCurrencyEuroValue(TextEdit_Value_Paid.Text.Trim()))
                    objMemberJoia.ValuePaid = Program.SetPayCurrencyEuroDoubleValue(TextEdit_Value_Paid.Text.Trim());

                if (Program.IsValidCurrencyEuroValue(TextEdit_Value_Missing.Text.Trim()))
                    objMemberJoia.ValueMissing = Program.SetPayCurrencyEuroDoubleValue(TextEdit_Value_Missing.Text.Trim());

                return objMemberJoia;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return null;
            }
        }

        /// <versions>08-10-2017(v0.0.4.8)</versions>
        private AMFCMemberJoia GetJoiaDetailsEdited()
        {
            try
            {
                AMFCMemberJoia objMemberJoia = new AMFCMemberJoia();

                if (Convert.ToInt64(TextEdit_JoiaId.Text.Trim()) > 0)
                    objMemberJoia.JoiaId = Convert.ToInt64(TextEdit_JoiaId.Text.Trim());

                if (Convert.ToInt64(TextEdit_MemberNumber.Text.Trim()) > 0 && Convert.ToInt64(TextEdit_MemberNumber.Text.Trim()) < new AMFCMember().MaxNumber)
                    objMemberJoia.MemberNumber = Convert.ToInt64(TextEdit_MemberNumber.Text.Trim());

                if (Program.IsValidTextString(TextEdit_MemberName.Text))
                    objMemberJoia.MemberName = TextEdit_MemberName.Text.Trim();

                objMemberJoia.DtMemberAdmiDate = Program.SetDateTimeValue(DateEdit_MemberAdmiDate.DateTime, -1, -1);

                if (Program.IsValidCurrencyEuroValue(TextEdit_JoiaValue.Text.Trim()))
                    objMemberJoia.JoiaValue = Program.SetPayCurrencyEuroDoubleValue(TextEdit_JoiaValue.Text.Trim());

                objMemberJoia.DtJoiaDate = Program.SetDateTimeValue(DateEdit_JoiaInsertDate.DateTime, -1, -1);

                if (Program.IsValidTextString(TextEdit_Joia_Notas.Text))
                    objMemberJoia.JoiaNotas = TextEdit_Joia_Notas.Text.Trim();

                objMemberJoia.DtJoiaDatePaid = Program.SetDateTimeValue(DateEdit_JoiaPayDate.DateTime, -1, -1);

                if (Program.IsValidTextString(TextEdit_JoiaPaidPerson.Text))
                    objMemberJoia.JoiaPaidPerson = TextEdit_JoiaPaidPerson.Text.Trim();
                else if (Program.IsValidTextString(TextEdit_MemberName.Text))
                    objMemberJoia.JoiaPaidPerson = TextEdit_MemberName.Text.Trim();

                if (Program.IsValidCurrencyEuroValue(TextEdit_Value_ToPay.Text.Trim()))
                    objMemberJoia.ValueToPay = Program.SetPayCurrencyEuroDoubleValue(TextEdit_Value_ToPay.Text.Trim());

                if (Program.IsValidCurrencyEuroValue(TextEdit_Value_Paid.Text.Trim()))
                    objMemberJoia.ValuePaid = Program.SetPayCurrencyEuroDoubleValue(TextEdit_Value_Paid.Text.Trim());

                if (Program.IsValidCurrencyEuroValue(TextEdit_Value_Missing.Text.Trim()))
                    objMemberJoia.ValueMissing = Program.SetPayCurrencyEuroDoubleValue(TextEdit_Value_Missing.Text.Trim());

                if (Program.IsValidTextString(TextEdit_Hidden_Pay_Caixa_ListIDs.Text))
                    objMemberJoia.JoiaListaCaixa = TextEdit_Hidden_Pay_Caixa_ListIDs.Text.Trim();
                
                return objMemberJoia;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return null;
            }
        }

        /// <versions>18-11-2017(GesAMFC-v0.0.4.33)</versions>
        private AMFCMemberJoia GetJoiaDetailsEditedPayment()
        {
            try
            {
                AMFCMemberJoia objMemberJoia = new AMFCMemberJoia();

                if (Convert.ToInt64(TextEdit_JoiaId.Text.Trim()) > 0)
                    objMemberJoia.JoiaId = Convert.ToInt64(TextEdit_JoiaId.Text.Trim());

                if (Convert.ToInt64(TextEdit_MemberNumber.Text.Trim()) > 0 && Convert.ToInt64(TextEdit_MemberNumber.Text.Trim()) < new AMFCMember().MaxNumber)
                    objMemberJoia.MemberNumber = Convert.ToInt64(TextEdit_MemberNumber.Text.Trim());

                if (Program.IsValidTextString(TextEdit_MemberName.Text))
                    objMemberJoia.MemberName = TextEdit_MemberName.Text.Trim();

                objMemberJoia.DtMemberAdmiDate = Program.SetDateTimeValue(DateEdit_MemberAdmiDate.DateTime, -1, -1);

                if (Program.IsValidCurrencyEuroValue(TextEdit_JoiaValue.Text.Trim()))
                    objMemberJoia.JoiaValue = Program.SetPayCurrencyEuroDoubleValue(TextEdit_JoiaValue.Text.Trim());

                objMemberJoia.DtJoiaDate = Program.SetDateTimeValue(DateEdit_JoiaInsertDate.DateTime, -1, -1);

                if (Program.IsValidTextString(TextEdit_Joia_Notas.Text))
                    objMemberJoia.JoiaNotas = TextEdit_Joia_Notas.Text.Trim();

                objMemberJoia.DtJoiaDatePaid = Program.SetDateTimeValue(DateEdit_JoiaPayDate.DateTime, -1, -1);

                if (Program.IsValidTextString(TextEdit_JoiaPaidPerson.Text))
                    objMemberJoia.JoiaPaidPerson = TextEdit_JoiaPaidPerson.Text.Trim();
                else if (Program.IsValidTextString(TextEdit_MemberName.Text))
                    objMemberJoia.JoiaPaidPerson = TextEdit_MemberName.Text.Trim();

                if (Program.IsValidCurrencyEuroValue(TextEdit_Value_Paid.Text.Trim()))
                {
                    objMemberJoia.ValuePaid = Program.SetPayCurrencyEuroDoubleValue(TextEdit_Value_Paid.Text.Trim());
                    objMemberJoia.ValueMissing = Program.SetPayCurrencyEuroDoubleValue(objMemberJoia.GetValueMissing());
                    objMemberJoia.ValueToPay = (objMemberJoia.ValueMissing > 0 && objMemberJoia.ValueMissing < objMemberJoia.JoiaValue) ? Program.SetPayCurrencyEuroDoubleValue(objMemberJoia.ValueMissing) : Program.SetPayCurrencyEuroDoubleValue(objMemberJoia.GetValueToPay());
                }

                return objMemberJoia;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return null;
            }
        }

        /// <versions>18-11-2017(GesAMFC-v0.0.4.33)</versions>
        private void Joia_Edit_Payment()
        {
            try
            {
                Set_Joia_Details_Editability(false);
                LayoutControlGroup_Joia_Edit.Visibility = LayoutVisibility.Never;
                LayoutControlGroup_Joia_Admin.Visibility = LayoutVisibility.Always; 
                LayoutControlGroup_Payment.Visibility = LayoutVisibility.Never;

                #region     Member Joia Info
                AMFCMemberJoia objMemberJoia = GetJoiaDetailsEditedPayment();
                if (objMemberJoia == null)
                {
                    String sError = "Ocorreu um erro a obter os dados de Pagamento da Joia do Sócio " + objMemberJoia.MemberName + " (Nº: " + objMemberJoia.MemberNumber + ")!";
                    MessageBox.Show(sError, "Erro Pagamento Sócio", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                #endregion  Member Joia Info

                #region     Validate Data 
                if (CheckJoiaIsValid(objMemberJoia, true) < 1)
                    return;
                #endregion  Validate Data

                #region     Edit Operation
                if (!DBF_AMFC_Edit_Joia(objMemberJoia, true))
                    return;
                #endregion  Edit Operation

                _SelectedJoia = objMemberJoia;
                Set_Joia_Details_Editability(false);
                LayoutControlGroup_Payment.Visibility = LayoutVisibility.Always;
                LayoutControlGroup_Joia_Add.Visibility = LayoutVisibility.Never;
                LayoutControlGroup_Joia_Admin.Visibility = LayoutVisibility.Always;
                Load_Grid(false, false, false, false, true, false, -1, objMemberJoia.JoiaId);
                Load_Member_Joia_Details(objMemberJoia.JoiaId);
                this.Grid_View.Focus();
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return;
            }
        }

        /// <versions>08-10-2017(v0.0.4.8)</versions>
        private Boolean SetJoiaDetails(AMFCMemberJoia objMemberJoia)
        {
            try
            {
                this.TextEdit_JoiaId.Text = objMemberJoia.JoiaId.ToString();
                this.TextEdit_MemberNumber.Text = objMemberJoia.MemberNumber.ToString();
                this.TextEdit_MemberName.Text = Program.SetTextString(objMemberJoia.MemberName, Program.DefaultStringTextTypes.DEFAULT);
                this.DateEdit_MemberAdmiDate.DateTime = Program.SetDateTimeValue(objMemberJoia.DtMemberAdmiDate, -1, -1);
                this.DateEdit_JoiaInsertDate.DateTime = Program.SetDateTimeValue(objMemberJoia.DtJoiaDate, -1, -1);
                this.TextEdit_Joia_Notas.Text = objMemberJoia.JoiaNotas;
                this.TextEdit_JoiaValue.Text = Program.SetPayCurrencyEuroStringValue(objMemberJoia.JoiaValue);
                this.TextEdit_Value_ToPay.Text = Program.SetPayCurrencyEuroStringValue(objMemberJoia.ValueToPay);
                this.TextEdit_Value_Paid.Text = Program.SetPayCurrencyEuroStringValue(objMemberJoia.ValuePaid);
                this.TextEdit_Value_Missing.Text = Program.SetPayCurrencyEuroStringValue(objMemberJoia.ValueMissing);
                this.DateEdit_JoiaPayDate.DateTime = Program.SetDateTimeValue(objMemberJoia.DtJoiaDatePaid, -1, -1);
                this.TextEdit_JoiaPaidPerson.Text = Program.SetTextString(objMemberJoia.JoiaPaidPerson, Program.DefaultStringTextTypes.DEFAULT);

                Set_PictureBox_Payment(objMemberJoia);
                LayoutControlItem_Pay.Visibility = (objMemberJoia.JoiaPaid || objMemberJoia.JoiaValue <= 0) ? LayoutVisibility.Never : LayoutVisibility.Always;
                LayoutControlItem_Show_Pay.Visibility = (objMemberJoia.JoiaValue > 0 && objMemberJoia.JoiaPaid && Get_Joia_Pay_Last_Id(objMemberJoia) > 0) ? LayoutVisibility.Always : LayoutVisibility.Never;

                JoiaValue_Changed();

                this.TextEdit_Hidden_Pay_Caixa_ListIDs.Text = objMemberJoia.JoiaListaCaixa;

                return true;
            }
            catch (Exception ex)
            {

                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return false;
            }
        }

        /// <versions>25-11-2017(GesAMFC-v0.0.4.37)</versions>
        private void Set_PictureBox_Payment(AMFCMemberJoia objJoia)
        {
            try
            {
                if (objJoia.JoiaPaid)
                {
                    this.PictureBox_Payment.Image = Properties.Resources.carimbo_pago_small;
                }
                else
                {
                    if (objJoia.Pay_State == AMFCMemberJoia.PayState.EM_PAGAMENTO)
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
        private void Reload_Joia_Details()
        {
            try
            {
                AMFCMemberJoia objMemberJoia = null;

                #region     Get Focused Joia
                if (this._SelectedJoia != null && this._SelectedJoia.JoiaId > 1)
                {
                    if (this.Grid_View.FocusedRowHandle > 0)
                    {
                        objMemberJoia = Get_Selected(this.Grid_View.FocusedRowHandle, -1);
                        if (objMemberJoia == null || objMemberJoia.MemberNumber < 1 || objMemberJoia.MemberNumber > new AMFCMember().MaxNumber || objMemberJoia.JoiaId < 1)
                        {
                            Clear_Joia_Details(true);
                            XtraMessageBox.Show("Não foi possível obter joia selecionada!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            Load_Grid(false, false, false, false, true, false, -1, -1);
                            return;
                        }
                    }
                }
                #endregion  Get Focused Joia

                #region     Load Joia Details
                if (objMemberJoia != null)
                {
                    if (!SetJoiaDetails(objMemberJoia))
                    {
                        Clear_Joia_Details(true);
                        XtraMessageBox.Show("Não foi possível obter carregar detalhes da joia!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Load_Grid(false, false, false, false, true, false, -1, -1);
                        return;
                    }
                }
                #endregion  Load Joia Details
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return;
            }
        }

        #region     Add Joia

        /// <versions>07-10-2017(v0.0.4.8)</versions>
        private Int32 DBF_AMFC_Members_GetMaxJoiaId()
        {
            try
            {
                using (Library_AMFC_SQL obj_AMFC_SQL = new Library_AMFC_SQL())
                {
                    Int32 iJoiaMaxId = obj_AMFC_SQL.Get_Joia_Max_Number();
                    if (iJoiaMaxId < 1)
                    {
                        String sWarning = "Não foi possivel obter o número máximo de ID das Joias! Por favor, contacte o programador!";
                        MessageBox.Show(sWarning, "Erro: Nº Joia Máximo", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return -1;
                    }
                    return iJoiaMaxId;
                }
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return -1;
            }
        }

        /// <versions>08-10-2017(v0.0.4.8)</versions>
        private void GetMemberToAddJoia()
        {
            try
            {
                try
                { 
                    Find_Member_Add_Joia objFindJoiaForm = new Find_Member_Add_Joia(null);
                    if (objFindJoiaForm != null)
                    {
                        objFindJoiaForm.FormClosing += delegate
                        {
                            if (!objFindJoiaForm.AllJoiasPaid)
                            {
                                if (objFindJoiaForm.Member_Found)
                                {
                                    AMFCMember objMemberSelected = objFindJoiaForm.MemberSelected;
                                    if (objMemberSelected != null && objMemberSelected.NUMERO > 0 && objMemberSelected.NUMERO < objMemberSelected.MaxNumber)
                                    {
                                        _SelectedMember = objMemberSelected;
                                        Button_Add_Joia_Action(objMemberSelected);
                                    }
                                }
                                else { this.Grid_View.FocusedRowHandle = -1; }
                            }
                        };
                        objFindJoiaForm.Show();
                        objFindJoiaForm.StartPosition = FormStartPosition.CenterParent;
                        objFindJoiaForm.Focus();
                        objFindJoiaForm.BringToFront();
                        objFindJoiaForm.Update();
                    }
                }
                catch { }
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return;
            }
        }

        /// <versions>08-10-2017(v0.0.4.8)</versions>
        private void Button_Add_Joia_Click()
        {
            try
            {
                GetMemberToAddJoia();
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return;
            }
        }

        /// <versions>08-10-2017(v0.0.4.8)</versions>
        private void Button_Add_Joia_Action(AMFCMember objMember)
        {
            try
            {
                #region     Get Member to Add
                if (objMember == null || objMember.NUMERO < 1 || objMember.NUMERO > objMember.MaxNumber)
                {
                    String sError = "Ocorreu um erro na obtenção do sócio para adicionar joia!";
                    MessageBox.Show(sError, "Erro Obtenção Sócio", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                #endregion  Get Member to Add

                #region     Set Controls to Add
                Set_Joia_Details_Editability(true);
                LayoutControlGroup_Joia_Admin.Visibility = LayoutVisibility.Never;
                LayoutControlGroup_Joia_Add.Visibility = LayoutVisibility.Always;
                LayoutControlGroup_Joia_Edit.Visibility = LayoutVisibility.Never;
                LayoutControlGroup_Payment.Visibility = LayoutVisibility.Never;
                #endregion  Set Controls to Add

                #region     Get Member Joia Info to Add
                AMFCMemberJoia objMemberJoia = GetJoiaDetailsToAdd(objMember);
                if (objMemberJoia == null)
                {
                    String sError = "Ocorreu um erro a obter os dados da Joia do Sócio " + objMemberJoia.MemberName + " (Nº: " + objMemberJoia.MemberNumber + ")!";
                    MessageBox.Show(sError, "Erro Criação Sócio", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                #endregion  Get Member Joia Info to Add

                #region     Set Member Joia Info to Add
                if (!SetJoiaDetails(objMemberJoia))
                {
                    Clear_Joia_Details(true);
                    XtraMessageBox.Show("Não foi possível obter carregar detalhes da joia!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Load_Grid(false, false, false, false, true, false, -1, -1);
                    return;
                }
                #endregion  Set Member Joia Info to Add
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
                #region     Get Current Joia Value
                //Double bCurrentJoiaValue = Program.Get_Current_Joia_Admin_Value(); //DEBUG -> nova db table com os valores DB_ADMIN_JOIAS
                //......
                #endregion  Get Current Joia Value

                #region     Member Joia Info
                AMFCMemberJoia objMemberJoia = GetJoiaDetailsEdited();
                if (objMemberJoia == null)
                {
                    String sError = "Ocorreu um erro a obter os dados da Joia do Sócio " + objMemberJoia.MemberName + " (Nº: " + objMemberJoia.MemberNumber + ")!";
                    MessageBox.Show(sError, "Erro Criação Sócio", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                #endregion  Member Joia Info

                #region     Validate Data 
                if (CheckJoiaIsValid(objMemberJoia, true) < 1)
                    return;
                #endregion  Validate Data

                #region     Add Operation
                if (!DBF_AMFC_Add_Joia(objMemberJoia))
                    return;
                _SelectedJoia = objMemberJoia;
                #endregion  Add Operation

                Set_Joia_Details_Editability(false);
                LayoutControlGroup_Payment.Visibility = LayoutVisibility.Always;
                LayoutControlGroup_Joia_Add.Visibility = LayoutVisibility.Never;
                LayoutControlGroup_Joia_Admin.Visibility = LayoutVisibility.Always;
                Load_Grid(false, false, false, false, true, false, -1, objMemberJoia.JoiaId);
                Load_Member_Joia_Details(objMemberJoia.JoiaId);
                this.Grid_View.Focus();

                if (!objMemberJoia.JoiaPaid && objMemberJoia.JoiaValue > 0)
                {
                    DialogResult objResult = XtraMessageBox.Show("Deseja efetuar o Pagamento da Joia!", "Pagamento da Joia", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                    if (objResult == DialogResult.Yes)
                        Pay_Joia();
                }
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
                Clear_Joia_Details(true);
                Set_Joia_Details_Editability(false);
                LayoutControlGroup_Joia_Add.Visibility = LayoutVisibility.Never;
                LayoutControlGroup_Joia_Admin.Visibility = LayoutVisibility.Always;
                LayoutControlGroup_Payment.Visibility = LayoutVisibility.Always;
                Load_Grid(false, false, false, false, true, false, 0, -1);
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
            if (_SelectedMember != null && _SelectedMember.NUMERO > 0 && _SelectedMember.NUMERO < _SelectedMember.MaxNumber)
                Button_Add_Joia_Action(_SelectedMember);
            else 
                Clear_Joia_Details(false);
        }

        /// <versions>08-10-2017(v0.0.4.8)</versions>
        private Boolean DBF_AMFC_Add_Joia(AMFCMemberJoia objMemberJoia)
        {
            try
            {
                Int64 lOpStatus = -1;
                using (Library_AMFC_SQL obj_AMFC_SQL = new Library_AMFC_SQL())
                    lOpStatus = obj_AMFC_SQL.Add_Joia(objMemberJoia);
                if (lOpStatus == 1)
                {
                    String sInfo = "Joia do Sócio " + objMemberJoia.MemberName + " (Nº: " + objMemberJoia.MemberNumber + ") adicionada com sucesso.";
                    MessageBox.Show(sInfo, "Joia adicionada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return true;
                }
                else if (lOpStatus == -1)
                {
                    String sError = "Ocorreu um erro na introdução da Joia do Sócio " + objMemberJoia.MemberName + " (Nº: " + objMemberJoia.MemberNumber + ")!";
                    MessageBox.Show(sError, "Erro Criação Sócio", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    if (!SetJoiaDetails(objMemberJoia))
                    {
                        Clear_Joia_Details(true);
                        XtraMessageBox.Show("Não foi possível obter carregar detalhes da joia!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Load_Grid(false, false, false, false, true, false, 0, -1);
                    }
                    return false;
                }
                else if (lOpStatus == 0)
                {
                    if (!SetJoiaDetails(objMemberJoia))
                    {
                        Clear_Joia_Details(true);
                        XtraMessageBox.Show("Carregar Detalhes da Joia", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Load_Grid(false, false, false, false, true, false, 0, -1);
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

        #endregion  Add Joia

        #region     Edit Joia

        /// <versions>08-10-2017(v0.0.4.9)</versions>
        private void Button_Edit_Joia_Click()
        {
            try
            {
                #region     Check Selected Joia
                if (this.Grid_View.FocusedRowHandle < 0)
                {
                    String sInfo = "Selecione uma joia na grelha para editar!";
                    MessageBox.Show(sInfo, "Joia não selecionada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                #endregion  Check Selected Joia

                #region     Set Controls to Edit
                Set_Joia_Details_Editability(true);
                LayoutControlGroup_Joia_Admin.Visibility = LayoutVisibility.Never;
                LayoutControlGroup_Joia_Add.Visibility = LayoutVisibility.Never;
                LayoutControlGroup_Joia_Edit.Visibility = LayoutVisibility.Always;
                LayoutControlGroup_Payment.Visibility = LayoutVisibility.Never;
                #endregion  Set Controls to Edit

                #region     Get Member Joia Info to Edit
                AMFCMemberJoia objMemberJoia = GetJoiaDetailsToEdit();
                if (objMemberJoia == null)
                {
                    String sError = "Ocorreu um erro a obter os dados da Joia do Sócio " + objMemberJoia.MemberName + " (Nº: " + objMemberJoia.MemberNumber + ")!";
                    MessageBox.Show(sError, "Erro Criação Sócio", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                #endregion  Get Member Joia Info to Edit

                #region     Set Member Joia Info to Edit
                if (!SetJoiaDetails(objMemberJoia))
                {
                    Clear_Joia_Details(true);
                    XtraMessageBox.Show("Não foi possível obter carregar detalhes da joia!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Load_Grid(false, false, false, false, true, false, -1, -1);
                    return;
                }
                #endregion  Set Member Joia Info to Edit
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
                Set_Joia_Details_Editability(false);
                LayoutControlGroup_Joia_Edit.Visibility = LayoutVisibility.Never;
                LayoutControlGroup_Joia_Admin.Visibility = LayoutVisibility.Always;
                LayoutControlGroup_Payment.Visibility = LayoutVisibility.Never;

                #region     Get Current Joia Value to Edit
                //Double bCurrentJoiaValue = Program.Get_Current_Joia_Admin_Value(); //DEBUG -> nova db table com os valores DB_ADMIN_JOIAS
                //......
                #endregion  Get Current Joia Value to Edit

                #region     Member Joia Info
                AMFCMemberJoia objMemberJoia = GetJoiaDetailsEdited();
                if (objMemberJoia == null)
                {
                    String sError = "Ocorreu um erro a obter os dados da Joia do Sócio " + objMemberJoia.MemberName + " (Nº: " + objMemberJoia.MemberNumber + ")!";
                    MessageBox.Show(sError, "Erro Criação Sócio", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                #endregion  Member Joia Info

                #region     Validate Data 
                if (CheckJoiaIsValid(objMemberJoia, true) < 1)
                    return;
                #endregion  Validate Data

                #region     Edit Operation
                if (!DBF_AMFC_Edit_Joia(objMemberJoia, true))
                    return;
                #endregion  Edit Operation

                _SelectedJoia = objMemberJoia;
                Set_Joia_Details_Editability(false);
                LayoutControlGroup_Payment.Visibility = LayoutVisibility.Always;
                LayoutControlGroup_Joia_Add.Visibility = LayoutVisibility.Never;
                LayoutControlGroup_Joia_Admin.Visibility = LayoutVisibility.Always;
                Load_Grid(false, false, false, false, true, false, -1, objMemberJoia.JoiaId);
                Load_Member_Joia_Details(objMemberJoia.JoiaId);
                this.Grid_View.Focus();

                JoiaValue_Changed();
                Value_ToPay_Changed();

                if (!objMemberJoia.JoiaPaid && objMemberJoia.JoiaValue > 0)
                {
                    DialogResult objResult = XtraMessageBox.Show("Deseja efetuar o Pagamento da Joia!", "Pagamento da Joia", MessageBoxButtons.OK, MessageBoxIcon.Question);
                    if (objResult == DialogResult.OK)
                        Pay_Joia();
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
                Reload_Joia_Details();
                Set_Joia_Details_Editability(false);
                LayoutControlGroup_Joia_Edit.Visibility = LayoutVisibility.Never;
                LayoutControlGroup_Joia_Admin.Visibility = LayoutVisibility.Always;
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
                Reload_Joia_Details();
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return;
            }
        }

        /// <versions>08-10-2017(v0.0.4.9)</versions>
        private Boolean DBF_AMFC_Edit_Joia(AMFCMemberJoia objMemberJoia, Boolean bShowMessageDialog)
        {
            try
            {
                Int64 lOpStatus = -1;
                using (Library_AMFC_SQL obj_AMFC_SQL = new Library_AMFC_SQL())
                    lOpStatus = obj_AMFC_SQL.Edit_Joia(objMemberJoia);
                if (lOpStatus == 1 && bShowMessageDialog)
                {
                    String sInfo = "Joia do Sócio " + objMemberJoia.MemberName + " (Nº: " + objMemberJoia.MemberNumber + ") alterada com sucesso.";
                    MessageBox.Show(sInfo, "Joia editada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return true;
                }
                else if (lOpStatus == -1)
                {
                    String sError = "Ocorreu um erro na alteração da Joia do Sócio " + objMemberJoia.MemberName + " (Nº: " + objMemberJoia.MemberNumber + ")!";
                    MessageBox.Show(sError, "Erro Criação Sócio", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    if (!SetJoiaDetails(objMemberJoia))
                    {
                        Clear_Joia_Details(true);
                        XtraMessageBox.Show("Não foi possível obter carregar detalhes da joia!", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Load_Grid(false, false, false, false, true, false, 0, -1);
                    }
                    return false;
                }
                else if (lOpStatus == 0)
                {
                    if (!SetJoiaDetails(objMemberJoia))
                    {
                        Clear_Joia_Details(true);
                        XtraMessageBox.Show("Carregar Detalhes da Joia", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        Load_Grid(false, false, false, false, true, false, 0, -1);
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

        #endregion  Edit Joia

        #region     Del Joia

        /// <versions>08-10-2017(v0.0.4.8)</versions>
        private void Button_Del_Click()
        {
            try
            {
                #region     Del Confirmation
                DialogResult objDialogResult = XtraMessageBox.Show("Deseja eliminar a joia?", "Eliminar joia?", MessageBoxButtons.OK, MessageBoxIcon.Question);
                if (objDialogResult != DialogResult.OK)
                    return;
                #endregion Del Confirmation

                #region     Check Selected Joia
                if (this.Grid_View.FocusedRowHandle < 0)
                {
                    String sInfo = "Selecione uma joia na grelha para eliminar!";
                    MessageBox.Show(sInfo, "Joia não selecionada", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                #endregion  Check Selected Joia

                #region     Get Member Joia ID
                Int64 lJoiaId = -1;
                if (Convert.ToInt64(TextEdit_JoiaId.Text.Trim()) > 0)
                    lJoiaId = Convert.ToInt64(TextEdit_JoiaId.Text.Trim());
                if (lJoiaId < 1)
                {
                    String sError = "Nº Joia inválido: " + lJoiaId;
                    MessageBox.Show(sError, "Nº Inválido", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                #endregion  Get Member Joia ID

                #region     Delete Operation
                if (!DBF_AMFC_Del_Joia(lJoiaId))
                {
                    String sError = "Nº Joia inválido: " + lJoiaId;
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

        /// <versions>08-10-2017(v0.0.4.8)</versions>
        private Boolean DBF_AMFC_Del_Joia(Int64 lJoiaId)
        {
            try
            {
                Int64 lOpStatus = -1;
                using (Library_AMFC_SQL obj_AMFC_SQL = new Library_AMFC_SQL())
                    lOpStatus = obj_AMFC_SQL.Del_Joia(lJoiaId);
                if (lOpStatus == 1)
                {
                    String sInfo = "Nº Joia = " + lJoiaId + " eliminada com sucesso.";
                    MessageBox.Show(sInfo, "Joia Eliminada", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Load_Grid(false, false, false, false, true, false, 0, -1);
                    return true;
                }
                else
                {
                    String sError = "Ocorreu um erro na eliminação da Joia Nº = " + lJoiaId + "!";
                    MessageBox.Show(sError, "Erro Eliminação Joia", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    Clear_Joia_Details(true);
                    Load_Grid(false, false, false, false, true, false, 0, -1);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return false;
            }
        }

        #endregion  Del Joia

        #region     Pay Joia

        /// <versions>12-11-2017(GesAMFC-v0.0.4.32)</versions>
        private AMFCCashPayment GetPayDetails(AMFCMemberJoia objJoia)
        {
            try
            {
                if (objJoia == null)
                    return null;

                AMFCCashPayment objMemberPay = new AMFCCashPayment();

                objMemberPay.Payment_Type = AMFCCashPayment.PaymentTypes.JOIA;

                #region     Get Pay Max ID
                Int32 iPayMaxId = DBF_AMFC_Members_GetMaxPayId();
                if (iPayMaxId < 1)
                    return null;
                #endregion  Get Pay Max ID

                objMemberPay.ID = Convert.ToInt64(iPayMaxId) + 1;
                if (objJoia.MemberNumber > 0 && objJoia.MemberNumber < new AMFCMember().MaxNumber)
                    objMemberPay.SOCIO = objJoia.MemberNumber;
                if (Program.IsValidTextString(objJoia.MemberName))
                    objMemberPay.NOME = objJoia.MemberName.Trim();

                if (Program.IsValidCurrencyEuroValue(TextEdit_Value_Missing.Text.Trim()))
                    objJoia.ValueMissing = Program.SetPayCurrencyEuroDoubleValue(TextEdit_Value_Missing.Text.Trim());
                
                objMemberPay.VALOR = (objJoia.ValueMissing > 0) ? Program.SetPayCurrencyEuroDoubleValue(objJoia.ValueMissing) : Program.SetPayCurrencyEuroDoubleValue(objJoia.JoiaValue);

                objMemberPay.ALTERADO = objMemberPay.DATA;

                objMemberPay.DATA = Program.SetDateTimeValue(DateEdit_JoiaPayDate.DateTime, -1, -1);

                if (Program.IsValidTextString(TextEdit_JoiaPaidPerson.Text))
                    objMemberPay.NOME_PAG = TextEdit_JoiaPaidPerson.Text.Trim();

                objMemberPay.DESIGNACAO = "Pagamento da Joia de Sócio";

                objMemberPay.Payment_State = AMFCCashPayment.PaymentState.ABERTO;

                #region     JOIA
                objMemberPay.HasJOIA = true;
                objMemberPay.JOIADESC = "Joia de admissão";
                objMemberPay.JOIAVAL = objJoia.JoiaValue;
                objMemberPay.DASSOCJOIA = objMemberPay.DATA;
                #endregion  JOIA

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
        private void Pay_Joia()
        {
            try
            {
                using (Library_AMFC_SQL obj_AMFC_SQL = new Library_AMFC_SQL())
                {
                    Form_Caixa objForm_Caixa = new Form_Caixa();
                    if (objForm_Caixa != null)
                    {
                        AMFCMemberJoia objJoia = GetJoiaDetailsEdited();
                        if (objJoia == null || objJoia.MemberNumber < 1 || objJoia.MemberNumber > new AMFCMember().MaxNumber || objJoia.JoiaId < 1)
                            return;

                        this._SelectedMember = Get_DBF_AMFC_Member_By_Number(objJoia.MemberNumber);
                        if (this._SelectedMember == null || this._SelectedMember.NUMERO <= 0 || this._SelectedMember.NUMERO > this._SelectedMember.MaxNumber)
                            return;
                        objForm_Caixa.PaymentMember = this._SelectedMember;

                        AMFCCashPayment objPayment = new AMFCCashPayment();
                        if (obj_AMFC_SQL.Member_Payment_Open_Already_Exist(this._SelectedMember.NUMERO)) //Pagamento em aberto
                        {
                            objPayment = obj_AMFC_SQL.Get_Member_Payment_Open(this._SelectedMember.NUMERO);
                            if (objPayment == null || objPayment.ID < 1)
                                return;
                            objPayment.JOIAVAL = (objJoia.ValueMissing > 0) ? objJoia.ValueMissing : objJoia.JoiaValue;
                            //objPayment.VALOR = 0;
                            objPayment.ALTERADO = Program.SetDateTimeValue(DateTime.Today, -1, -1);
                            objPayment.HasJOIA = true;
                            objPayment.JOIADESC = "Joia Admissão";
                            objPayment.DASSOCJOIA = Program.SetDateTimeValue(DateTime.Today, -1, -1);
                            objPayment.SetItemPayment(AMFCCashPayment.PaymentTypes.JOIA, Program.SetPayCurrencyEuroDoubleValue(objPayment.JOIAVAL), true, false);
                            objForm_Caixa.PaymentOperationType = Library_AMFC_Methods.OperationType.EDIT;
                            objForm_Caixa.CurrentPayment = objPayment;
                            objForm_Caixa.EntitiesPaymentType = objForm_Caixa.GetMultipleSinglePaymentType();
                        }
                        else
                        {
                            objPayment = GetPayDetails(objJoia); //Novo pagamento
                            if (objPayment == null || objPayment.ID < 1)
                                return;
                            objPayment.JOIAVAL = (objJoia.ValueMissing > 0) ? objJoia.ValueMissing : objJoia.JoiaValue;
                            objPayment.SetItemPayment(AMFCCashPayment.PaymentTypes.JOIA, Program.SetPayCurrencyEuroDoubleValue(objPayment.JOIAVAL), true, false);
                            objForm_Caixa.PaymentOperationType = Library_AMFC_Methods.OperationType.ADD;
                            objPayment.Payment_Type = AMFCCashPayment.PaymentTypes.JOIA;
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
                                        objJoia.AddValuePaid(Program.SetPayCurrencyEuroDoubleValue(objForm_Caixa.CurrentPayment.JOIAVAL.ToString().Trim()));
                                        TextEdit_Value_ToPay.Text = Program.SetPayCurrencyEuroStringValue(objJoia.ValueToPay);
                                        TextEdit_Value_Paid.Text = Program.SetPayCurrencyEuroStringValue(objJoia.ValuePaid);
                                        TextEdit_Value_Missing.Text = Program.SetPayCurrencyEuroStringValue(objJoia.ValueMissing);
                                        TextEdit_Value_OnPaying.Text = Program.SetPayCurrencyEuroStringValue(objJoia.ValueOnPaying);
                                    }
                                    else if (objForm_Caixa.CurrentPayment.Payment_State == AMFCCashPayment.PaymentState.ABERTO)
                                    {
                                        objJoia.ValueOnPaying = Program.SetPayCurrencyEuroDoubleValue(objForm_Caixa.CurrentPayment.JOIAVAL.ToString().Trim());
                                        TextEdit_Value_ToPay.Text = Program.SetPayCurrencyEuroStringValue(objJoia.ValueToPay);
                                        TextEdit_Value_Paid.Text = Program.SetPayCurrencyEuroStringValue(objJoia.ValuePaid);
                                        TextEdit_Value_Missing.Text = Program.SetPayCurrencyEuroStringValue(objJoia.ValueMissing);
                                        TextEdit_Value_OnPaying.Text = Program.SetPayCurrencyEuroStringValue(objJoia.ValueOnPaying);
                                    }
                                    DateEdit_JoiaPayDate.DateTime = objForm_Caixa.CurrentPayment.DASSOCJOIA;
                                    TextEdit_JoiaPaidPerson.Text = objForm_Caixa.CurrentPayment.NOME_PAG;

                                    #region     Update Joia Details
                                    objJoia = Get_Joia_Pay_Details(objJoia);
                                    if (objForm_Caixa.CurrentPayment.ID > 0)
                                        objJoia.AddCaixaId(objForm_Caixa.CurrentPayment.ID);
                                    if (objForm_Caixa.CurrentPayment.Payment_State == AMFCCashPayment.PaymentState.FINALIZADO)
                                        objJoia.Pay_State = AMFCMemberJoia.PayState.SIM;
                                    else
                                        objJoia.Pay_State = AMFCMemberJoia.PayState.EM_PAGAMENTO;
                                    if (objJoia == null)
                                        return;
                                    Joia_Pay_Edit_DB(objJoia, false);
                                    #endregion  Update Joia Details
                                }
                                catch { }
                            }
                            else //Delete Payment or Error 
                            {
                                try
                                {
                                    if (objForm_Caixa.CurrentPayment.Payment_State == AMFCCashPayment.PaymentState.CANCELED)
                                    {
                                        objJoia.RemoveValuePaid(Program.SetPayCurrencyEuroDoubleValue(objForm_Caixa.CurrentPayment.JOIAVAL.ToString().Trim()));
                                        TextEdit_Value_ToPay.Text = Program.SetPayCurrencyEuroStringValue(objJoia.ValueToPay);
                                        TextEdit_Value_Paid.Text = Program.SetPayCurrencyEuroStringValue(objJoia.ValuePaid);
                                        TextEdit_Value_Missing.Text = Program.SetPayCurrencyEuroStringValue(objJoia.ValueMissing);
                                        TextEdit_Value_OnPaying.Text = Program.SetPayCurrencyEuroStringValue(objJoia.ValueOnPaying);
                                    }

                                    objJoia = Get_Joia_Pay_Details(objJoia);
                                    if (objForm_Caixa.CurrentPayment.ID > 0)
                                        objJoia.DelCaixaId(objForm_Caixa.CurrentPayment.ID);

                                    if (objJoia == null)
                                        return;
                                    Joia_Pay_Edit_DB(objJoia, false);
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
        private Int64 Get_Joia_Pay_Last_Id(AMFCMemberJoia objJoia)
        {
            try
            {
                Int64 lPayId = -1;
                if (objJoia.JoiaListaCaixaIDs.Count == 0)
                    return -1;
                lPayId = objJoia.GetLastCaixaId();
                return lPayId;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, false, false);
                return -1;
            }
        }

        /// <versions>26-11-2017(GesAMFC-v0.0.4.37)</versions>
        private void Open_Joia()
        {
            try
            {
                Int64 lPayId = -1;
                AMFCMemberJoia objJoia = GetJoiaDetailsEdited();
                if (objJoia == null || objJoia.MemberNumber < 1 || objJoia.MemberNumber > new AMFCMember().MaxNumber || objJoia.JoiaId < 1)
                    return;

                if (objJoia.JoiaListaCaixaIDs.Count == 0)
                {
                    String sWarning = "Não foi possivel obter o ID do Pagamento!";
                    MessageBox.Show(sWarning, "Erro: Nº de " + "Pagamento", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                lPayId = Get_Joia_Pay_Last_Id(objJoia);
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

                        this._SelectedMember = Get_DBF_AMFC_Member_By_Number(objJoia.MemberNumber);
                        if (this._SelectedMember == null || this._SelectedMember.NUMERO <= 0 || this._SelectedMember.NUMERO > this._SelectedMember.MaxNumber)
                            return;
                        objForm_Caixa.PaymentMember = this._SelectedMember;

                        AMFCCashPayment objPayment = obj_AMFC_SQL.Get_Member_Payment_By_Id(this._SelectedMember.NUMERO, lPayId);
                        if (objPayment == null || objPayment.ID < 1)
                            return;

                        objPayment.JOIAVAL = (objJoia.ValueMissing > 0) ? objJoia.ValueMissing : objJoia.JoiaValue;
                        objPayment.VALOR = 0;
                        objPayment.SetItemPayment(AMFCCashPayment.PaymentTypes.JOIA, Program.SetPayCurrencyEuroDoubleValue(objPayment.JOIAVAL), true, false);

                        //objPayment.Payment_Type = AMFCCashPayment.PaymentTypes.JOIA;
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
        private void Joia_Pay_Edit_DB(AMFCMemberJoia objMemberJoia, Boolean bShowMessageDialog)
        {
            try
            {
                Set_Joia_Details_Editability(false);
                LayoutControlGroup_Joia_Edit.Visibility = LayoutVisibility.Never;
                LayoutControlGroup_Joia_Admin.Visibility = LayoutVisibility.Always;
                LayoutControlGroup_Payment.Visibility = LayoutVisibility.Never;

                #region     Member Joia Info
                if (objMemberJoia == null)
                {
                    String sError = "Ocorreu um erro a obter os dados de Pagamento da Joia do Sócio " + objMemberJoia.MemberName + " (Nº: " + objMemberJoia.MemberNumber + ")!";
                    MessageBox.Show(sError, "Erro Pagamento Sócio", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                #endregion  Member Joia Info

                #region     Validate Data 
                if (CheckJoiaIsValid(objMemberJoia, true) < 1)
                    return;
                #endregion  Validate Data

                #region     Edit Operation
                if (!DBF_AMFC_Edit_Joia(objMemberJoia, bShowMessageDialog))
                    return;
                #endregion  Edit Operation

                _SelectedJoia = objMemberJoia;
                Set_PictureBox_Payment(objMemberJoia);
                Set_Joia_Details_Editability(false);
                LayoutControlGroup_Payment.Visibility = LayoutVisibility.Always;
                LayoutControlGroup_Joia_Add.Visibility = LayoutVisibility.Never;
                LayoutControlGroup_Joia_Admin.Visibility = LayoutVisibility.Always;
                Load_Grid(false, false, false, false, true, false, -1, objMemberJoia.JoiaId);
                Load_Member_Joia_Details(objMemberJoia.JoiaId);
                this.Grid_View.Focus();
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return;
            }
        }

        /// <versions>18-11-2017(GesAMFC-v0.0.4.33)</versions>
        private AMFCMemberJoia Get_Joia_Pay_Details(AMFCMemberJoia objMemberJoia)
        {
            try
            {
                if (objMemberJoia.JoiaId <= 1)
                    objMemberJoia.JoiaId = Convert.ToInt64(TextEdit_JoiaId.Text.Trim());

                if (objMemberJoia.MemberNumber < 1 || objMemberJoia.MemberNumber > new AMFCMember().MaxNumber)
                    objMemberJoia.MemberNumber = Convert.ToInt64(TextEdit_MemberNumber.Text.Trim());

                if (Program.IsValidTextString(objMemberJoia.MemberName))
                    objMemberJoia.MemberName = TextEdit_MemberName.Text.Trim();

                objMemberJoia.DtMemberAdmiDate = Program.SetDateTimeValue(DateEdit_MemberAdmiDate.DateTime, -1, -1);

                if (!Program.IsValidCurrencyEuroValue(objMemberJoia.JoiaValue))
                    objMemberJoia.JoiaValue = Program.SetPayCurrencyEuroDoubleValue(TextEdit_JoiaValue.Text.Trim());

                objMemberJoia.DtJoiaDate = Program.SetDateTimeValue(DateEdit_JoiaInsertDate.DateTime, -1, -1);

                if (Program.IsValidTextString(objMemberJoia.JoiaNotas))
                    objMemberJoia.JoiaNotas = TextEdit_Joia_Notas.Text.Trim();

                objMemberJoia.DtJoiaDatePaid = Program.SetDateTimeValue(DateEdit_JoiaPayDate.DateTime, -1, -1);

                if (Program.IsValidTextString(objMemberJoia.JoiaPaidPerson))
                    objMemberJoia.JoiaPaidPerson = TextEdit_JoiaPaidPerson.Text.Trim();

                if (Program.IsValidTextString(objMemberJoia.JoiaListaCaixa))
                    objMemberJoia.JoiaListaCaixa = TextEdit_Hidden_Pay_Caixa_ListIDs.Text.Trim();

                return objMemberJoia;
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

        #endregion  Pay Joia

        #endregion  Actions Methods

        #endregion  Methods

        /// <versions>21-11-2017(GesAMFC-v0.0.4.35)</versions>
        private void DateEdit_MemberAdmiDate_QueryPopUp(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DateEdit dateEdit = sender as DateEdit;
            Program.DateEdit_QueryPopUp_Event(dateEdit, e);
        }

        /// <versions>21-11-2017(GesAMFC-v0.0.4.35)</versions>
        private void DateEdit_JoiaInsertDate_QueryPopUp(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DateEdit dateEdit = sender as DateEdit;
            Program.DateEdit_QueryPopUp_Event(dateEdit, e);
        }

        /// <versions>21-11-2017(GesAMFC-v0.0.4.35)</versions>
        private void DateEdit_JoiaPayDate_QueryPopUp(object sender, System.ComponentModel.CancelEventArgs e)
        {
            DateEdit dateEdit = sender as DateEdit;
            Program.DateEdit_QueryPopUp_Event(dateEdit, e);
        }
    }
}