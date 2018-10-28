using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Grid.ViewInfo;
using DevExpress.XtraGrid.Views.Layout;
using GesAMFC.AMFC_Methods;
using System;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace GesAMFC
{
    /// <summary>AMFC Members Admin</summary>
    /// <author>Valter Lima</author>
    /// <creation>18-02-2017(v0.0.1.0)</creation>
    /// <versions>21-10-2017(v0.0.4.18)</versions>
    public partial class Admin_AMFC_Members : XtraForm
    {
        public Library_AMFC_Methods LibAMFC;

        //private const bool _SetMemberDetailsLayoutColumnsAtRunTime = false; //20-04-2017(v0.0.1.25)

        #region     Form Variables
        private DataTable _DataTableSourceListMembers;
        private AMFCMembers _CollectionSourceListMembers;
        private AMFCMember _SelectedMember;
        public Library_AMFC_Methods.MemberOperationType Operation { get; set; }
        #endregion  Form Variables

        #region     Form Constructor 

        /// <versions>28-04-2017(v0.0.2.48)</versions>
        public Admin_AMFC_Members()
        {
            LibAMFC = new Library_AMFC_Methods();

            try
            {
                _SelectedMember = new AMFCMember();
                _DataTableSourceListMembers = new DataTable();
                _CollectionSourceListMembers = new AMFCMembers();
                //_FormDescription = "Administração de Sócios";
                this.Operation = Library_AMFC_Methods.MemberOperationType.GET;

                InitializeComponent();

                #region     User Permissions Set Controls
                String sPermAdmin = "Permissão de Administrador";
                this.SimpleButton_Member_Add.ToolTip = Program.AppUser.CanAdd ? "Adicionar novo sócio" : sPermAdmin;
                this.SimpleButton_Member_Edit.ToolTip = Program.AppUser.CanEdit ? "Editar sócio" : sPermAdmin;
                this.SimpleButton_Member_Del.ToolTip = Program.AppUser.CanDel ? "Apagar sócio" : sPermAdmin;
                this.SimpleButton_Member_Add.Enabled = Program.AppUser.CanAdd;
                this.SimpleButton_Member_Edit.Enabled = Program.AppUser.CanEdit;
                this.SimpleButton_Member_Del.Enabled = Program.AppUser.CanDel;
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

        /// <versions>01-05-2017(v0.0.2.49)</versions>
        private void Admin_AMFC_Members_Load(object sender, EventArgs e)
        {
            try
            {
                #region     Tem de ser feito aqui senão crasha -> No futuro, depois de ter o menu, comentar !!!
                this.WindowState = FormWindowState.Maximized;
                this.Update();
                #endregion

                SetGridMembersControls();

                this.Operation = Library_AMFC_Methods.MemberOperationType.GET;
                Load_Grid_Members(true, true, true, true, true, true, -1, -1);

                //Find_Member();

                //SplitContainer_AMFC_01.SplitterDistance = (this.Size.Width / 2) + 100;
                SplitContainer_AMFC_01.SplitterDistance = (this.Size.Width / 2);
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>21-02-2017(v0.0.1.6)</versions>
        private void Admin_AMFC_Members_FormClosing(object sender, FormClosingEventArgs e)
        {
            //try
            //{
            //    if (!Program.Dialog_Exit_Form(_FormDescription))
            //        e.Cancel = true;
            //}
            //catch
            //{
            //    Program.Exit_Application();
            //}
        }

        #endregion  Form Events

        #region     List Members Events

        /// <versions>20-03-2017(v0.0.1.12)</versions>
        private void GridView_AMFC_Members_Click(object sender, EventArgs e)
        {
            try
            {
                //GridView view = (GridView)sender;
                //GridHitInfo hi = view.CalcHitInfo(e.Location);

                //Int32 iRowHandle = hi.RowHandle;

                //#region     Check if is a valid Data Row Handle
                //if (iRowHandle == GridControl.InvalidRowHandle || !this.GridView_AMFC_Members.IsDataRow(iRowHandle))
                //{
                //    this.GridView_AMFC_Members.ClearSelection();
                //    return;
                //}
                //#endregion  Check if is a valid Data Row Handle

                this.Operation = Library_AMFC_Methods.MemberOperationType.GET;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>22-03-2017(v0.0.1.14)</versions>
        private void GridView_AMFC_Members_MouseDown(object sender, MouseEventArgs e)
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
                    if (iRowHandle == GridControl.InvalidRowHandle || !this.GridView_AMFC_Members.IsDataRow(iRowHandle))
                    {
                        this.GridView_AMFC_Members.ClearSelection();
                        return;
                    }
                    #endregion  Check if is a valid Data Row Handle

                    #region     Prevent the selectionchanged event
                    if (!hi.InRowCell || hi.Column == null)
                    {
                        (e as DXMouseEventArgs).Handled = true;
                        return;
                    }
                    #endregion  Prevent the selectionchanged event

                    this.Operation = Library_AMFC_Methods.MemberOperationType.GET;

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

        //// <versions>22-03-2017(v0.0.1.14)</versions>
        private void GridView_AMFC_Members_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
        {
            try
            {
                Int32 iRowHandle = e.FocusedRowHandle;
                //this.GridView_AMFC_Members.SelectRow(iRowHandle);
                Int32 iMinRowHandle = 1; //> 0 para não carreagar 2x a 1ª linha(iRowHandle= 0) -> 1º Sócio: a 1º linha é focada no load automáticamente;
                GridView_MembersFocusedRow(iRowHandle, iMinRowHandle);
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>23-02-2017(v0.0.1.6)</versions>
        private void Button_Grid_Members_Menu_Click(object sender, EventArgs e)
        {
            this.GridView_AMFC_Members.ColumnsCustomization();
        }

        /// <versions>14-03-2017(v0.0.2.6)</versions>
        private void GridView_AMFC_Members_RowStyle(object sender, RowStyleEventArgs e)
        {
            try
            {
                if (e.RowHandle >= 0 && this.GridView_AMFC_Members.FocusedRowHandle >= 0 && e.RowHandle == this.GridView_AMFC_Members.FocusedRowHandle)
                {
                    //this.GridView_AMFC_Members.Appearance.FocusedCell.BackColor = objColor;
                    //this.GridView_AMFC_Members.Appearance.FocusedRow.BackColor = objColor;
                    e.Appearance.BackColor = Program.FocusedRowBgColor;
                }
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>23-04-2017(v0.0.1.33)</versions>
        private void Button_Grid_Members_FindMember_Click(object sender, EventArgs e)
        {
            Find_Member();
        }

        #region     Export/Print Events

        /// <versions>08-03-2017(v0.0.1.9)</versions>
        private void Button_Grid_Members_Export_Excel_Click(object sender, EventArgs e)
        {
            try
            {
                String sFileName = "Lista" + "_" + LibAMFC.DBF_AMFC_SOCIO_FileName + "_" + DateTime.Now.ToString("yyyyMMddHHmmss");

                #region     Export Options
                DevExpress.XtraPrinting.XlsExportOptions objXlsExportOptions = new DevExpress.XtraPrinting.XlsExportOptions();
                objXlsExportOptions.TextExportMode = DevExpress.XtraPrinting.TextExportMode.Value;
                objXlsExportOptions.ExportMode = DevExpress.XtraPrinting.XlsExportMode.SingleFile;
                objXlsExportOptions.ShowGridLines = true;
                #endregion  Export Options

                #region     Save Copy Into Local App Folder
                String sLocalExportMememberDirPath = "Export/Excel/SOCIOS";
                String sLocalExportMememberFilePath = sLocalExportMememberDirPath + "/" + sFileName + ".xls";
                this.GridControl_AMFC_Members.ExportToXls(sLocalExportMememberFilePath, objXlsExportOptions);
                #endregion  Save Copy Into Local App Folder

                #region     Save to Excel
                SaveFileDialog objSaveFileDialog = new SaveFileDialog();
                objSaveFileDialog.Title = "Lista de Sócios - Export Excel";
                objSaveFileDialog.Filter = "Excel(*.xls)|*.xls"; //Excel Worksheets|*.xls
                objSaveFileDialog.FileName = sFileName;
                DialogResult dialogResult = objSaveFileDialog.ShowDialog(this);
                if (dialogResult == DialogResult.OK)
                {
                    this.GridControl_AMFC_Members.ExportToXls(objSaveFileDialog.FileName, objXlsExportOptions);
                    DevExpress.XtraEditors.XtraMessageBox.Show("Lista de sócios exportada para excel com sucesso!", "Exportação Lista Sócios Excel", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                #endregion  Save to Excel
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }

        }

        /// <versions>21-02-2017(v0.0.1.6)</versions>
        private void Button_Grid_Members_Print_Click(object sender, EventArgs e)
        {
            try
            {
                //this.GridControl_AMFC_Members.ShowPrintPreview();
                this.GridControl_AMFC_Members.PrintDialog();
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        #endregion Export/Print Events

        #endregion  List Members Events
        
        #region     Member Details  Events

        #region     Member File Events

        /// <versions>08-03-2017(v0.0.1.9)</versions>
        private void Button_Member_Details_Export_Excel_Click(object sender, EventArgs e)
        {
            try
            {
                #region     Get Focused Member
                AMFCMember objFocusedMember = Get_Focused_Member();
                if (objFocusedMember == null || objFocusedMember.NUMERO < 1)
                    return;
                #endregion  Get Focused Member

                String sFileName = "Ficha_Socio" + "_" + objFocusedMember.NUMERO + "_" + DateTime.Now.ToString("yyyyMMddHHmmss");

                #region     Export Options
                DevExpress.XtraPrinting.XlsExportOptions objXlsExportOptions = new DevExpress.XtraPrinting.XlsExportOptions();
                objXlsExportOptions.TextExportMode = DevExpress.XtraPrinting.TextExportMode.Value;
                objXlsExportOptions.ExportMode = DevExpress.XtraPrinting.XlsExportMode.SingleFile;
                objXlsExportOptions.ShowGridLines = true;
                #endregion  Export Options

                #region     Save Copy Into Local App Folder
                String sLocalExportMememberDirPath = "Export/Excel/SOCIOS";
                String sLocalExportMememberFilePath = sLocalExportMememberDirPath + "/" + sFileName + ".xls";
                this.GridControl_Member_Details.ExportToXls(sLocalExportMememberFilePath, objXlsExportOptions);
                #endregion  Save Copy Into Local App Folder

                #region     Save to Excel
                SaveFileDialog objSaveFileDialog = new SaveFileDialog();
                objSaveFileDialog.Title = "Ficha de Sócio - Export Excel";
                objSaveFileDialog.Filter = "Excel(*.xls)|*.xls"; //Excel Worksheets|*.xls
                objSaveFileDialog.FileName = sFileName;
                DialogResult dialogResult = objSaveFileDialog.ShowDialog(this);
                if (dialogResult == DialogResult.OK)
                {
                    this.GridControl_Member_Details.ExportToXls(objSaveFileDialog.FileName, objXlsExportOptions);
                    DevExpress.XtraEditors.XtraMessageBox.Show("A Ficha do Sócio Nº: " + objFocusedMember.NUMERO + " foi exportada para excel com sucesso!", "Exportação Ficha Sócio Excel", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                #endregion  Save to Excel   
             
                this.LayoutView_Member_Details.Focus();
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }

        }

        /// <versions>21-02-2017(v0.0.1.6)</versions>
        private void Button_Member_Details_Print_Click(object sender, EventArgs e)
        {
            try
            {
                //this.GridControl_Member_Details.ShowPrintPreview();
                this.GridControl_Member_Details.PrintDialog();
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        #endregion  Member File  Events

        #endregion  Member Details  Events

        #region     Members Admin Events

        /// <versions>23-04-2017(v0.0.1.33)</versions>
        private void SimpleButton_Member_Find_Click(object sender, EventArgs e)
        {
            Find_Member();
        }

        /// <versions>05-05-2017(v0.0.2.56)</versions>
        private void LayoutView_Member_Details_Click(object sender, EventArgs e)
        {
            try
            {
                Member_Details_Click();
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return;
            }
        }

        /// <versions>05-05-2017(v0.0.2.56)</versions>
        private void SimpleButton_Member_Add_Click(object sender, EventArgs e)
        {
            try
            {
                Member_Add_Click();
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>05-05-2017(v0.0.2.56)</versions>
        private void SimpleButton_Member_Edit_Click(object sender, EventArgs e)
        {
            try
            {
                Member_Edit_Click();
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>05-05-2017(v0.0.2.56)</versions>
        private void SimpleButton_Member_Del_Click(object sender, EventArgs e)
        {
            try
            {
                Member_Del_Click();
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return;
            }
        }

        #endregion  Members Admin Events

        #endregion  Events

        #region     Methods

        #region     Public Methods

        /// <summary>MenuBar Entity Click</summary>
        /// <versions>23-04-2017(v0.0.1.34)</versions>
        public void SetMenuBarEntityClick(String sEntity)
        {
            StackFrame objStackFrame = new StackFrame();
            try
            {
                //Program.WriteLog(objStackFrame.GetMethod().Name, "Entity Type: " + sEntity, true, true, true, true);
                //this.Panel_Entity_Type_Header.Visible = true;
                //this.LabelControl_Entity.Text = sHeaderTitle;
                //this.SplitContainer_AMFC_01.Visible = true;
                this.Text = "Gestão de Sócios";
                this.SplitContainer_AMFC_01.SplitterDistance = 1000;
                //CleanFilter();
                //CleanGrid();
                //this.LoadingBox_EntitiesGrid.Hide();
                //this.LoadingBox_EntitiesAdmin.Hide();
                //this.GridControl_Entities.Hide();
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return;
            }
            finally { objStackFrame = null; }
        }

        #endregion  Public Methods

        #region     Private Methods

        #region     List Members

        // <versions>21-02-2017(v0.0.1.6)</versions>
        private void SetGridMembers()
        {
            try
            {
                //this.LoadingBox_dMembers.Hide();
                LibAMFC.GridConfiguration(this.GridControl_AMFC_Members, this.GridView_AMFC_Members, true, true, true);
                //this.GridControl_AMFC_Members.Font.Name =  "Verdana";
                //this.GridControl_AMFC_Members.Font.Size = 6,25;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>22-03-2017(v0.0.1.14)</versions>
        private void Load_Grid_Members(Boolean bSetCols, Boolean bClearSorting, Boolean bClearFilters, Boolean bClearGrouping, Boolean bForceMemberDetailsDataLoad, Boolean bForceMemberOtherDataLoad, Int32 iMemberFocusedRowHandle, Int64 lMemberNumber)
        {
            try
            {
                Grid_AMFC_Members_Clear_Selection();
                Load_AMFC_members_Grid(bSetCols, bClearSorting, bClearFilters, bClearGrouping, true);

                #region     Config Grids Options
                this.GridView_AMFC_Members.OptionsSelection.EnableAppearanceFocusedRow = true;
                this.GridView_AMFC_Members.OptionsSelection.EnableAppearanceFocusedCell = false;
                this.GridView_AMFC_Members.OptionsSelection.EnableAppearanceHideSelection = false;
                this.GridView_AMFC_Members.OptionsSelection.UseIndicatorForSelection = true;
                this.GridView_AMFC_Members.ClearSelection();

                #region     LayoutView Member Details Configs
                this.LayoutView_Member_Details.OptionsBehavior.ScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Always;
                this.LayoutView_Member_Details.OptionsView.ShowCardBorderIfCaptionHidden = true;
                this.LayoutView_Member_Details.OptionsSingleRecordMode.StretchCardToViewWidth = true;
                this.LayoutView_Member_Details.OptionsSingleRecordMode.StretchCardToViewHeight = true;
                this.LayoutView_Member_Details.Appearance.FieldValue.Options.UseFont = true;
                this.LayoutView_Member_Details.Appearance.FieldValue.Font = new Font("Tahoma", 8.5f);
                this.LayoutView_Member_Details.Appearance.FieldCaption.Options.UseFont = true;
                this.LayoutView_Member_Details.Appearance.FieldCaption.Font = new Font("Tahoma", 8.5f);
                //this.LayoutView_Member_Details.CardMinSize = new Size(500, 200);
                //this.LayoutView_Member_Details.TemplateCard.Size = new Size(600, 800);
                //this.LayoutView_Member_Details.Appearance.FieldCaption.TextOptions.HAlignment = HorzAlignment.Near
                #endregion  LayoutView Member Details Configs

                #endregion  Config Grids Options

                #region     Set Focused Member
                if (lMemberNumber > 0)
                {
                    Int32 iRowHandle = this.GridView_AMFC_Members.LocateByValue(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMERO), lMemberNumber);
                    if (iRowHandle != DevExpress.XtraGrid.GridControl.InvalidRowHandle)
                        this.GridView_AMFC_Members.FocusedRowHandle = iRowHandle;
                }
                else if (iMemberFocusedRowHandle > -1)
                {
                    if (iMemberFocusedRowHandle != DevExpress.XtraGrid.GridControl.InvalidRowHandle)
                        this.GridView_AMFC_Members.FocusedRowHandle = iMemberFocusedRowHandle;
                    //this.GridView_AMFC_Members.SelectRow(this.GridView_AMFC_Members.FocusedRowHandle);
                }
                #endregion  Set Focused Member

                if (bForceMemberDetailsDataLoad || bForceMemberOtherDataLoad)
                    Load_Member_Data(bForceMemberDetailsDataLoad, bForceMemberOtherDataLoad, lMemberNumber, bSetCols);
                return;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>21-02-2017(v0.0.1.6)</versions>
        private void Load_AMFC_members_Grid(Boolean bSetCols, Boolean bClearSorting, Boolean bClearFilters, Boolean bClearGrouping, Boolean bWriteLog)
        {
            try
            {
                LibAMFC.CleanGrid(this.GridControl_AMFC_Members, this.GridView_AMFC_Members, bSetCols, bClearSorting, bClearFilters, bClearGrouping);
                this.GridControl_AMFC_Members.Visible = false;
                //this.LoadingBox_Brands.Dock = DockStyle.Fill;
                //this.LoadingBox_Brands.Show();
                this.Update();

                Boolean bLoadDatasource = SetGridMembersDataSource(this.GridControl_AMFC_Members);

                if (bLoadDatasource && bSetCols)
                    SetGrid_AMFC_members_Columns();

                //this.LoadingBox_Brands.Hide();
                this.GridControl_AMFC_Members.Visible = true;
                this.Update();
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>20-03-2017(v0.0.1.12)</versions>
        private void SetGridMembersControls()
        {
            try
            {
                SetGridMembers();
                //SetGridMemberJoias();
                //SetGridMemberQuotas();
                //SetGridMemberCedencias();
                //SetGridMemberContaCorrentes();
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>27-03-2017(v0.0.1.16)</versions>
        private void Load_Member_Data(Boolean bForceMemberDetailsDataLoad, Boolean bForceMemberOtherDataLoad, Int64 lMemberNumber, Boolean bConfigGrids)
        {
            try
            {
                #region     Get Focused Member
                AMFCMember objMember = Get_Selected_Member(-1, lMemberNumber);
                if (objMember == null || objMember.NUMERO < 1)
                {
                    XtraMessageBox.Show("Erro", "Load_Member_Data", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                #endregion  Get Focused Member

                #region     Load Member Details
                if (bForceMemberDetailsDataLoad)
                {
                    LoadMemberDetailsLayoutView(objMember, bConfigGrids, true, true, false);
                    //Set_Member_Letter_Address_Editor(objMember);
                }
                #endregion  Load Member Details

                #region     Load Member Other Info (Payments)
                if (bForceMemberOtherDataLoad)
                {
                    #region     Grid Member Joia
                    //if (bConfigGrids) SetGridMemberJoias();
                    //Load_Grid_Member_Joias(objMember, true, true, true, true);
                    #endregion  Grid Member Joia

                    #region     Grid Member Quotas
                    //if (bConfigGrids) SetGridMemberQuotas();
                    //Load_Grid_Member_Quotas(objMember, true, true, true, true);
                    #endregion  Grid Member Quotas

                    #region     Grid Member Cedencias
                    //if (bConfigGrids) SetGridMemberCedencias();
                    //Load_Grid_Member_Cedencias(objMember, true, true, true, true);
                    #endregion  Grid Member Cedencias

                    #region     Grid Member ContaCorrentes
                    //if (bConfigGrids) SetGridMemberContaCorrentes();
                    //Load_Grid_Member_ContaCorrentes(objMember, true, true, true, true);
                    #endregion  Grid Member ContaCorrentes
                }
                #endregion  Load Member Other Info (Payments)
                return;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>20-04-2017(v0.0.1.26)</versions>
        private AMFCMembers Get_DBF_AMFC_Members()
        {
            try
            {
                using (Library_AMFC_SQL obj_AMFC_SQL = new Library_AMFC_SQL())
                    return obj_AMFC_SQL.Get_List_Members();
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return null;
            }
        }

        /// <versions>21-10-2017(v0.0.4.18)</versions>
        private Boolean SetGridMembersDataSource(GridControl objGridControl)
        {
            StackFrame objStackFrame = new StackFrame();
            String sErrorMsg = String.Empty;
            try
            {
                #region         Members Datasource
                _CollectionSourceListMembers = Get_DBF_AMFC_Members();
                if (_CollectionSourceListMembers == null)
                {
                    sErrorMsg = "Não foi possível obter a Lista de Sócios!";
                    Program.HandleError(objStackFrame.GetMethod().Name, sErrorMsg, Program.ErroType.ERROR, true, false);
                    return false;
                }
                if (_CollectionSourceListMembers.Members.Count == 0)
                {
                    String sWarningMsg = "Não existem Sócios!";
                    MessageBox.Show(sWarningMsg, "No results", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                #endregion  Members Datasource

                #region     TreeDataSet
                _DataTableSourceListMembers = new DataTable("AMFC_Members_List");
                #region     Data Columns Creation
                DataColumn objDataColumn_Member_NUMERO      = new DataColumn(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMERO),    typeof(Int64)); //Int64
                DataColumn objDataColumn_Member_NOME        = new DataColumn(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NOME),      typeof(String));
                DataColumn objDataColumn_Member_MORADA      = new DataColumn(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.MORADA),    typeof(String));
                DataColumn objDataColumn_Member_CPOSTAL     = new DataColumn(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.CPOSTAL),   typeof(String));
                DataColumn objDataColumn_Member_TELEFONE    = new DataColumn(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.TELEFONE),  typeof(String));
                DataColumn objDataColumn_Member_TELEMOVEL   = new DataColumn(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.TELEMOVEL), typeof(String));
                DataColumn objDataColumn_Member_CC = new DataColumn(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.CC), typeof(String));
                DataColumn objDataColumn_Member_NIF = new DataColumn(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NIF), typeof(String));
                DataColumn objDataColumn_Member_EMAIL       = new DataColumn(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.EMAIL),     typeof(String));
                DataColumn objDataColumn_Member_MORLOTE     = new DataColumn(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.MORLOTE),   typeof(String));
                DataColumn objDataColumn_Member_NUMLOTE     = new DataColumn(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMLOTE),   typeof(String));
                DataColumn objDataColumn_Member_AREALOTE    = new DataColumn(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.AREALOTE),  typeof(Int32));  //Int32
                //DataColumn objDataColumn_Member_PROFISSAO   = new DataColumn(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.PROFISSAO), typeof(String));
                //DataColumn objDataColumn_Member_DATAADMI    = new DataColumn(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.DATAADMI), typeof(String));
                //DataColumn objDataColumn_Member_OBSERVACAO  = new DataColumn(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.OBSERVACAO), typeof(String));
                //DataColumn objDataColumn_Member_SECTOR      = new DataColumn(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.SECTOR), typeof(String));
                //DataColumn objDataColumn_Member_CASA        = new DataColumn(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.CASA), typeof(Boolean));
                //DataColumn objDataColumn_Member_GARAGEM     = new DataColumn(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.GARAGEM), typeof(Boolean));
                //DataColumn objDataColumn_Member_MUROS       = new DataColumn(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.MUROS), typeof(Boolean));
                //DataColumn objDataColumn_Member_POCO        = new DataColumn(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.POCO), typeof(Boolean));
                //DataColumn objDataColumn_Member_FURO        = new DataColumn(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.FURO), typeof(Boolean));
                //DataColumn objDataColumn_Member_SANEAMENTO  = new DataColumn(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.SANEAMENTO), typeof(Boolean));
                //DataColumn objDataColumn_Member_ELECTRICID  = new DataColumn(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.ELECTRICID), typeof(Boolean));
                //DataColumn objDataColumn_Member_PROJECTO    = new DataColumn(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.PROJECTO), typeof(Boolean));
                //DataColumn objDataColumn_Member_ESCRITURA    = new DataColumn(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.ESCRITURA), typeof(Boolean));
                //DataColumn objDataColumn_Member_FINANCAS    = new DataColumn(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.FINANCAS), typeof(Boolean));
                //DataColumn objDataColumn_Member_RESIDENCIA  = new DataColumn(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.RESIDENCIA), typeof(Boolean));
                //DataColumn objDataColumn_Member_AGREFAMIL   = new DataColumn(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.AGREFAMIL), typeof(String)); //String
                //DataColumn objDataColumn_Member_NUMFILHOS   = new DataColumn(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMFILHOS), typeof(String)); //String
                //DataColumn objDataColumn_Member_GAVETO      = new DataColumn(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.GAVETO), typeof(Boolean));
                //DataColumn objDataColumn_Member_QUINTINHA   = new DataColumn(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.QUINTINHA), typeof(Boolean));
                //DataColumn objDataColumn_Member_LADOMAIOR   = new DataColumn(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.LADOMAIOR), typeof(String)); //String
                //DataColumn objDataColumn_Member_MAIS1FOGO   = new DataColumn(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.MAIS1FOGO), typeof(Boolean));
                //DataColumn objDataColumn_Member_HABCOLECT   = new DataColumn(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.HABCOLECT), typeof(Boolean));
                //DataColumn objDataColumn_Member_NUMFOGOS    = new DataColumn(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMFOGOS), typeof(String)); //String
                //DataColumn objDataColumn_Member_ARECOMERC   = new DataColumn(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.ARECOMERC), typeof(Boolean));
                #endregion  Data Columns Creation
                #region     Data Table Add Columns
                _DataTableSourceListMembers.Columns.AddRange(
                                            new DataColumn[] {
                                                objDataColumn_Member_NUMERO,
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
                                                objDataColumn_Member_AREALOTE
                                                //objDataColumn_Member_PROFISSAO,
                                                //objDataColumn_Member_DATAADMI,
                                                //objDataColumn_Member_OBSERVACAO,
                                                //objDataColumn_Member_SECTOR,
                                                //objDataColumn_Member_CASA,
                                                //objDataColumn_Member_GARAGEM,
                                                //objDataColumn_Member_MUROS,
                                                //objDataColumn_Member_POCO,
                                                //objDataColumn_Member_FURO,
                                                //objDataColumn_Member_SANEAMENTO,
                                                //objDataColumn_Member_ELECTRICID,
                                                //objDataColumn_Member_PROJECTO,
                                                //objDataColumn_Member_ESCRITURA,
                                                //objDataColumn_Member_FINANCAS,
                                                //objDataColumn_Member_RESIDENCIA,
                                                //objDataColumn_Member_AGREFAMIL,
                                                //objDataColumn_Member_NUMFILHOS,
                                                //objDataColumn_Member_GAVETO,
                                                //objDataColumn_Member_QUINTINHA,
                                                //objDataColumn_Member_LADOMAIOR,
                                                //objDataColumn_Member_MAIS1FOGO,
                                                //objDataColumn_Member_HABCOLECT,
                                                //objDataColumn_Member_NUMFOGOS,
                                                //objDataColumn_Member_ARECOMERC
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
                //objDataColumn_Member_PROFISSAO.Dispose();
                //objDataColumn_Member_DATAADMI.Dispose();
                //objDataColumn_Member_OBSERVACAO.Dispose();
                //objDataColumn_Member_SECTOR.Dispose();
                //objDataColumn_Member_CASA.Dispose();
                //objDataColumn_Member_GARAGEM.Dispose();
                //objDataColumn_Member_MUROS.Dispose();
                //objDataColumn_Member_POCO.Dispose();
                //objDataColumn_Member_FURO.Dispose();
                //objDataColumn_Member_SANEAMENTO.Dispose();
                //objDataColumn_Member_ELECTRICID.Dispose();
                //objDataColumn_Member_PROJECTO.Dispose();
                //objDataColumn_Member_ESCRITURA.Dispose();
                //objDataColumn_Member_FINANCAS.Dispose();
                //objDataColumn_Member_RESIDENCIA.Dispose();
                //objDataColumn_Member_AGREFAMIL.Dispose();
                //objDataColumn_Member_NUMFILHOS.Dispose();
                //objDataColumn_Member_GAVETO.Dispose();
                //objDataColumn_Member_QUINTINHA.Dispose();
                //objDataColumn_Member_LADOMAIOR.Dispose();
                //objDataColumn_Member_MAIS1FOGO.Dispose();
                //objDataColumn_Member_HABCOLECT.Dispose();
                //objDataColumn_Member_NUMFOGOS.Dispose();
                //objDataColumn_Member_ARECOMERC.Dispose();
                #endregion  Data Columns Dispose
                #endregion  TreeDataSet

                #region     Binding Data
                Program.AllDbMembers = _CollectionSourceListMembers;
                foreach (AMFCMember objMember in _CollectionSourceListMembers.Members)
                {
                    if (objMember == null || objMember.NUMERO < 1 || String.IsNullOrEmpty(objMember.NOME))
                        continue;
                    #region     Set Member Row Data
                    DataRow objDataRow = _DataTableSourceListMembers.NewRow();
                    objDataRow[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMERO)]     = objMember.NUMERO.ToString();
                    objDataRow[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NOME)]       = objMember.NOME;
                    objDataRow[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.MORADA)]     = objMember.MORADA;
                    objDataRow[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.CPOSTAL)]    = objMember.CPOSTAL;
                    objDataRow[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.TELEFONE)]   = objMember.TELEFONE;
                    objDataRow[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.TELEMOVEL)]  = objMember.TELEMOVEL;
                    objDataRow[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.CC)] = objMember.CC;
                    objDataRow[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NIF)] = objMember.NIF;
                    objDataRow[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.EMAIL)]      = objMember.EMAIL;
                    objDataRow[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.MORLOTE)]    = objMember.MORLOTE;
                    objDataRow[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMLOTE)]    = objMember.NUMLOTE;
                    objDataRow[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.AREALOTE)]   = objMember.AREALOTE;
                    //objDataRow[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.PROFISSAO)] = objMember.PROFISSAO;
                    //objDataRow[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.DATAADMI)] = objMember.DATAADMI;
                    //objDataRow[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.OBSERVACAO)] = objMember.OBSERVACAO;
                    //objDataRow[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.SECTOR)] = objMember.SECTOR;
                    //objDataRow[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.CASA)] = objMember.CASA;
                    //objDataRow[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.GARAGEM)] = objMember.GARAGEM;
                    //objDataRow[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.MUROS)] = objMember.MUROS;
                    //objDataRow[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.POCO)] = objMember.POCO;
                    //objDataRow[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.FURO)] = objMember.FURO;
                    //objDataRow[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.SANEAMENTO)] = objMember.SANEAMENTO;
                    //objDataRow[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.ELECTRICID)] = objMember.ELECTRICID;
                    //objDataRow[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.PROJECTO)] = objMember.PROJECTO;
                    //objDataRow[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.ESCRITURA)] = objMember.ESCRITURA;
                    //objDataRow[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.FINANCAS)] = objMember.FINANCAS;
                    //objDataRow[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.RESIDENCIA)] = objMember.RESIDENCIA;
                    //objDataRow[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.AGREFAMIL)] = objMember.AGREFAMIL;
                    //objDataRow[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMFILHOS)] = objMember.NUMFILHOS;
                    //objDataRow[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.GAVETO)] = objMember.GAVETO;
                    //objDataRow[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.QUINTINHA)] = objMember.QUINTINHA;
                    //objDataRow[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.LADOMAIOR)] = objMember.LADOMAIOR;
                    //objDataRow[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.MAIS1FOGO)] = objMember.MAIS1FOGO;
                    //objDataRow[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.HABCOLECT)] = objMember.HABCOLECT;
                    //objDataRow[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMFOGOS)] = objMember.NUMFOGOS;
                    //objDataRow[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.ARECOMERC)] = objMember.ARECOMERC;
                    #endregion  Set Member Row Data
                    _DataTableSourceListMembers.Rows.Add(objDataRow);
                }
                objGridControl.DataSource = _DataTableSourceListMembers;
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

        /// <versions>21-02-2017(v0.0.1.6)</versions>
        private void Grid_AMFC_Members_Clear_Selection()
        {
            StackFrame objStackFrame = new StackFrame();
            try
            {
                this.GridView_AMFC_Members.ClearSelection();
                //for (int iRow = 0; iRow < this.GridView_AMFC_Members.DataRowCount; iRow++)
                //    this.GridView_AMFC_Members.SetRowCellValue(iRow, this.GridView_AMFC_Members.Columns[_SelectedColName], false);
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
            finally { objStackFrame = null; }
        }

        /// <versions>21-02-2017(v0.0.1.6)</versions>
        public void SetGrid_AMFC_members_Columns()
        {
            try
            {
                Set_AMFC_members_ColumnsEditability();
                Set_AMFC_members_ColumnsOptionsFilter();
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>21-10-2017(v0.0.4.18)</versions>
        private void Set_AMFC_members_ColumnsEditability()
        {
            try
            {                                                                                                                                                                                                                                                                                                   
                LibAMFC.SetGridColumnOptions(this.GridControl_AMFC_Members, this.GridView_AMFC_Members, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMERO), new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMERO),                "Número de sócio",      true,   1,  70, true, false, true, true, HorzAlignment.Far,     VertAlignment.Center,   HorzAlignment.Far,      VertAlignment.Center);
                LibAMFC.SetGridColumnOptions(this.GridControl_AMFC_Members, this.GridView_AMFC_Members, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NOME), new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.NOME),                    "Nome do sócio",        true,   2, 250, true, false, true, true, HorzAlignment.Near,    VertAlignment.Center,   HorzAlignment.Near,     VertAlignment.Center);
                LibAMFC.SetGridColumnOptions(this.GridControl_AMFC_Members, this.GridView_AMFC_Members, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.MORADA), new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.MORADA),                "Morada do sócio",      true,   4, 300, true, false, true, true, HorzAlignment.Center,  VertAlignment.Center,   HorzAlignment.Near,      VertAlignment.Center);
                LibAMFC.SetGridColumnOptions(this.GridControl_AMFC_Members, this.GridView_AMFC_Members, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.CPOSTAL), new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.CPOSTAL),              "CPOSTAL",              true,   5, 200, true, false, true, true, HorzAlignment.Center,  VertAlignment.Center,   HorzAlignment.Near,     VertAlignment.Center);
                LibAMFC.SetGridColumnOptions(this.GridControl_AMFC_Members, this.GridView_AMFC_Members, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.TELEFONE), new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.TELEFONE),            "TELEFONE",             true,   6,  90, true, false, true, true, HorzAlignment.Center,  VertAlignment.Center,   HorzAlignment.Center,   VertAlignment.Center);
                LibAMFC.SetGridColumnOptions(this.GridControl_AMFC_Members, this.GridView_AMFC_Members, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.TELEMOVEL), new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.TELEMOVEL),          "TELEMOVEL",            true,   7,  90, true, false, true, true, HorzAlignment.Center,  VertAlignment.Center,   HorzAlignment.Center,   VertAlignment.Center);
                LibAMFC.SetGridColumnOptions(this.GridControl_AMFC_Members, this.GridView_AMFC_Members, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.CC), new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.CC), "Cartão Cidadão", true, 8, 90, true, false, true, true, HorzAlignment.Center, VertAlignment.Center, HorzAlignment.Center, VertAlignment.Center);
                LibAMFC.SetGridColumnOptions(this.GridControl_AMFC_Members, this.GridView_AMFC_Members, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NIF), new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.NIF), "NIF", true, 9, 90, true, false, true, true, HorzAlignment.Center, VertAlignment.Center, HorzAlignment.Center, VertAlignment.Center);
                LibAMFC.SetGridColumnOptions(this.GridControl_AMFC_Members, this.GridView_AMFC_Members, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.EMAIL), new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.EMAIL),                  "EMAIL",                true,  10, 220, true, false, true, true, HorzAlignment.Center,  VertAlignment.Center,   HorzAlignment.Center,   VertAlignment.Center);
                LibAMFC.SetGridColumnOptions(this.GridControl_AMFC_Members, this.GridView_AMFC_Members, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.MORLOTE), new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.MORLOTE),              "Morada do lote",       true,  11, 280, true, false, true, true, HorzAlignment.Center,  VertAlignment.Center,   HorzAlignment.Center,   VertAlignment.Center);
                LibAMFC.SetGridColumnOptions(this.GridControl_AMFC_Members, this.GridView_AMFC_Members, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMLOTE), new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMLOTE),              "Lote de sócio",        true,  12,  60, true, false, true, true, HorzAlignment.Center,  VertAlignment.Center,   HorzAlignment.Center,   VertAlignment.Center);
                LibAMFC.SetGridColumnOptions(this.GridControl_AMFC_Members, this.GridView_AMFC_Members, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.AREALOTE), new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.AREALOTE),            "Área do lote",         true,  13,  60, true, false, true, true, HorzAlignment.Center,  VertAlignment.Center,   HorzAlignment.Far,      VertAlignment.Center);
                //LibAMFC.SetGridColumnOptions(this.GridControl_AMFC_Members, this.GridView_AMFC_Members, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.PROFISSAO), new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.PROFISSAO),          "PROFISSAO",            false, -1, 150, true, false, true, true, HorzAlignment.Center,  VertAlignment.Center,   HorzAlignment.Center,   VertAlignment.Center);
                //LibAMFC.SetGridColumnOptions(this.GridControl_AMFC_Members, this.GridView_AMFC_Members, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.DATAADMI), new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.DATAADMI),            "DATAADMI",             false, -1,  50, true, false, true, true, HorzAlignment.Center,  VertAlignment.Center,   HorzAlignment.Center,   VertAlignment.Center);
                //LibAMFC.SetGridColumnOptions(this.GridControl_AMFC_Members, this.GridView_AMFC_Members, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.OBSERVACAO), new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.OBSERVACAO),        "OBSERVACAO",           false, -1,  50, true, false, true, true, HorzAlignment.Center,  VertAlignment.Center,   HorzAlignment.Center,   VertAlignment.Center);
                //LibAMFC.SetGridColumnOptions(this.GridControl_AMFC_Members, this.GridView_AMFC_Members, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.SECTOR), new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.SECTOR),                "SECTOR",               false, -1,  50, true, false, true, true, HorzAlignment.Center, VertAlignment.Center, HorzAlignment.Center, VertAlignment.Center);
                //LibAMFC.SetGridColumnOptions(this.GridControl_AMFC_Members, this.GridView_AMFC_Members, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.CASA), new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.CASA),                    "CASA",                 false, -1,  50, true, false, true, true, HorzAlignment.Center, VertAlignment.Center, HorzAlignment.Center, VertAlignment.Center);
                //LibAMFC.SetGridColumnOptions(this.GridControl_AMFC_Members, this.GridView_AMFC_Members, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.GARAGEM), new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.GARAGEM),              "GARAGEM",              false, -1,  50, true, false, true, true, HorzAlignment.Center, VertAlignment.Center, HorzAlignment.Center, VertAlignment.Center);
                //LibAMFC.SetGridColumnOptions(this.GridControl_AMFC_Members, this.GridView_AMFC_Members, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.MUROS), new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.MUROS),                  "MUROS",                false, -1,  50, true, false, true, true, HorzAlignment.Center, VertAlignment.Center, HorzAlignment.Center, VertAlignment.Center);
                //LibAMFC.SetGridColumnOptions(this.GridControl_AMFC_Members, this.GridView_AMFC_Members, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.POCO), new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.POCO),                    "POCO",                 false, -1,  50, true, false, true, true, HorzAlignment.Center, VertAlignment.Center, HorzAlignment.Center, VertAlignment.Center);
                //LibAMFC.SetGridColumnOptions(this.GridControl_AMFC_Members, this.GridView_AMFC_Members, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.FURO), new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.FURO),                    "FURO",                 false, -1,  50, true, false, true, true, HorzAlignment.Center, VertAlignment.Center, HorzAlignment.Center, VertAlignment.Center);
                //LibAMFC.SetGridColumnOptions(this.GridControl_AMFC_Members, this.GridView_AMFC_Members, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.SANEAMENTO), new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.SANEAMENTO),        "SANEAMENTO",           false, -1,  50, true, false, true, true, HorzAlignment.Center, VertAlignment.Center, HorzAlignment.Center, VertAlignment.Center);
                //LibAMFC.SetGridColumnOptions(this.GridControl_AMFC_Members, this.GridView_AMFC_Members, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.ELECTRICID), new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.ELECTRICID),        "ELECTRICID",           false, -1,  50, true, false, true, true, HorzAlignment.Center, VertAlignment.Center, HorzAlignment.Center, VertAlignment.Center);
                //LibAMFC.SetGridColumnOptions(this.GridControl_AMFC_Members, this.GridView_AMFC_Members, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.PROJECTO), new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.PROJECTO),            "PROJECTO",             false, -1,  50, true, false, true, true, HorzAlignment.Center, VertAlignment.Center, HorzAlignment.Center, VertAlignment.Center);
                //LibAMFC.SetGridColumnOptions(this.GridControl_AMFC_Members, this.GridView_AMFC_Members, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.ESCRITURA), new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.ESCRITURA),          "ESCRITURA",            false, -1,  50, true, false, true, true, HorzAlignment.Center, VertAlignment.Center, HorzAlignment.Center, VertAlignment.Center);
                //LibAMFC.SetGridColumnOptions(this.GridControl_AMFC_Members, this.GridView_AMFC_Members, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.FINANCAS), new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.FINANCAS),            "FINANCAS",             false, -1,  50, true, false, true, true, HorzAlignment.Center, VertAlignment.Center, HorzAlignment.Center, VertAlignment.Center);
                //LibAMFC.SetGridColumnOptions(this.GridControl_AMFC_Members, this.GridView_AMFC_Members, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.RESIDENCIA), new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.RESIDENCIA),        "RESIDENCIA",           false, -1,  50, true, false, true, true, HorzAlignment.Center, VertAlignment.Center, HorzAlignment.Center, VertAlignment.Center);
                //LibAMFC.SetGridColumnOptions(this.GridControl_AMFC_Members, this.GridView_AMFC_Members, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.AGREFAMIL), new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.AGREFAMIL),          "AGREFAMIL",            false, -1,  50, true, false, true, true, HorzAlignment.Center, VertAlignment.Center, HorzAlignment.Center, VertAlignment.Center);
                //LibAMFC.SetGridColumnOptions(this.GridControl_AMFC_Members, this.GridView_AMFC_Members, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMFILHOS), new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMFILHOS),          "NUMFILHOS",            false, -1,  50, true, false, true, true, HorzAlignment.Center, VertAlignment.Center, HorzAlignment.Center, VertAlignment.Center);
                //LibAMFC.SetGridColumnOptions(this.GridControl_AMFC_Members, this.GridView_AMFC_Members, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.GAVETO), new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.GAVETO),                "GAVETO",               false, -1,  50, true, false, true, true, HorzAlignment.Center, VertAlignment.Center, HorzAlignment.Center, VertAlignment.Center);
                //LibAMFC.SetGridColumnOptions(this.GridControl_AMFC_Members, this.GridView_AMFC_Members, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.QUINTINHA), new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.QUINTINHA),          "QUINTINHA",            false, -1,  50, true, false, true, true, HorzAlignment.Center, VertAlignment.Center, HorzAlignment.Center, VertAlignment.Center);
                //LibAMFC.SetGridColumnOptions(this.GridControl_AMFC_Members, this.GridView_AMFC_Members, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.LADOMAIOR), new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.LADOMAIOR),          "LADOMAIOR",            false, -1,  50, true, false, true, true, HorzAlignment.Center, VertAlignment.Center, HorzAlignment.Center, VertAlignment.Center);
                //LibAMFC.SetGridColumnOptions(this.GridControl_AMFC_Members, this.GridView_AMFC_Members, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.MAIS1FOGO), new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.MAIS1FOGO),          "MAIS1FOGO",            false, -1,  50, true, false, true, true, HorzAlignment.Center, VertAlignment.Center, HorzAlignment.Center, VertAlignment.Center);
                //LibAMFC.SetGridColumnOptions(this.GridControl_AMFC_Members, this.GridView_AMFC_Members, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.HABCOLECT), new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.HABCOLECT),          "HABCOLECT",            false, -1,  50, true, false, true, true, HorzAlignment.Center, VertAlignment.Center, HorzAlignment.Center, VertAlignment.Center);
                //LibAMFC.SetGridColumnOptions(this.GridControl_AMFC_Members, this.GridView_AMFC_Members, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMFOGOS), new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMFOGOS),            "NUMFOGOS",             false, -1,  50, true, false, true, true, HorzAlignment.Center, VertAlignment.Center, HorzAlignment.Center, VertAlignment.Center);
                //LibAMFC.SetGridColumnOptions(this.GridControl_AMFC_Members, this.GridView_AMFC_Members, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.ARECOMERC), new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.ARECOMERC),          "ARECOMERC",            false, -1,  50, true, false, true, true, HorzAlignment.Center, VertAlignment.Center, HorzAlignment.Center, VertAlignment.Center);
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>26-03-2017(v0.0.1.15)</versions>
        private void Set_AMFC_members_ColumnsOptionsFilter()
        {
            try
            {
                LibAMFC.SetGridColumOptionsFilter(this.GridView_AMFC_Members, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMERO),       true, true, AutoFilterCondition.Equals,     9.5f);
                LibAMFC.SetGridColumOptionsFilter(this.GridView_AMFC_Members, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NOME),         true, true, AutoFilterCondition.Contains,   9.0f);
                LibAMFC.SetGridColumOptionsFilter(this.GridView_AMFC_Members, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.MORADA),       true, true, AutoFilterCondition.Contains,   8.5f);
                LibAMFC.SetGridColumOptionsFilter(this.GridView_AMFC_Members, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.CPOSTAL),      true, true, AutoFilterCondition.Contains,   8.5f);
                LibAMFC.SetGridColumOptionsFilter(this.GridView_AMFC_Members, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.TELEFONE),     true, true, AutoFilterCondition.Contains,   8.5f);
                LibAMFC.SetGridColumOptionsFilter(this.GridView_AMFC_Members, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.TELEMOVEL),    true, true, AutoFilterCondition.Contains,   8.5f);
                LibAMFC.SetGridColumOptionsFilter(this.GridView_AMFC_Members, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.CC), true, true, AutoFilterCondition.Contains, 8.5f);
                LibAMFC.SetGridColumOptionsFilter(this.GridView_AMFC_Members, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NIF), true, true, AutoFilterCondition.Contains, 8.5f);
                LibAMFC.SetGridColumOptionsFilter(this.GridView_AMFC_Members, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.EMAIL),        true, true, AutoFilterCondition.Contains,   8.5f);
                LibAMFC.SetGridColumOptionsFilter(this.GridView_AMFC_Members, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.MORLOTE),      true, true, AutoFilterCondition.Contains,   8.5f);
                LibAMFC.SetGridColumOptionsFilter(this.GridView_AMFC_Members, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMLOTE),      true, true, AutoFilterCondition.Contains,   8.5f);
                LibAMFC.SetGridColumOptionsFilter(this.GridView_AMFC_Members, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.AREALOTE),     true, true, AutoFilterCondition.Contains,   8.5f);
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>22-03-2017(v0.0.1.14)</versions>
        private Boolean GridView_MembersFocusedRow(Int32 iRowHandle)
        {
            return GridView_Members_Focused_Row(iRowHandle, 0);
        }

        /// <versions>22-03-2017(v0.0.1.14)</versions>
        private Boolean GridView_MembersFocusedRow(Int32 iRowHandle, Int32 iMinRowHandle)
        {
            return GridView_Members_Focused_Row(iRowHandle, iMinRowHandle);
        }

        /// <versions>22-03-2017(v0.0.1.14)</versions>
        private Boolean GridView_Members_Focused_Row(Int32 iRowHandle, Int32 iMinRowHandle)
        {
            try
            {
                StackFrame objStackFrame = new StackFrame();
                #region     Check if is a valid Data Row Handle
                if (iRowHandle < iMinRowHandle || iRowHandle == GridControl.InvalidRowHandle || !this.GridView_AMFC_Members.IsDataRow(iRowHandle))
                {
                    //this.GridView_AMFC_Members.ClearSelection();
                    return false;
                }
                #endregion  Check if is a valid Data Row Handle

                #region     Get Focused Member
                Boolean bIsTheSameMember = IsTheSameSelectedMember(iRowHandle, -1);
                AMFCMember objMember = null;
                if (!bIsTheSameMember)
                    objMember = Get_Selected_Member(iRowHandle, -1);
                else
                    objMember = _SelectedMember;
                if (objMember == null || objMember.NUMERO < 1)
                {
                    XtraMessageBox.Show("Erro", objStackFrame.GetMethod().Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                #endregion  Get Focused Member

                #region     Load Member Data
                if (!bIsTheSameMember)
                    Load_Member_Data(true, true, objMember.NUMERO, false);
                #endregion  Load Member Data

                return true;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return false;
            }
        }

        #endregion  List Members

        #region     Member Details

        /// <versions>21-02-2017(v0.0.1.6)</versions>
        private void LoadMemberDetailsLayoutView(AMFCMember objMember, Boolean bSetCols, Boolean bClearSorting, Boolean bClearFilters, Boolean bWriteLog)
        {
            try
            {
                CleanGridLayoutView(this.GridControl_Member_Details, this.LayoutView_Member_Details, bSetCols, bClearSorting, bClearFilters);
                this.GridControl_Member_Details.Visible = false;
                this.Update();

                Boolean bLoadDatasource = SetMemberDetailsDataSource(this.GridControl_Member_Details, objMember);
                if (bLoadDatasource && bSetCols)
                {
                    //if (_SetMemberDetailsLayoutColumnsAtRunTime)
                    //    Add_Member_LayoutView_Columns(); //Este Bloco atrasa 6 sseg ?!! -> Adicionar as colunas no designer (_SetMemberDetailsLayoutColumnsAtRunTime = false) e depois atribuir apenas o FieldName
                    //else
                        Add_Member_LayoutView_Columns_Options();
                }

                //this.LoadingBox_Brands.Hide();
                this.GridControl_Member_Details.Visible = true;
                this.Update();
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>23-04-2017(v0.0.1.31)</versions>
        private Boolean SetMemberDetailsDataSource(GridControl objGridControl, AMFCMember objMember)
        {
            StackFrame objStackFrame = new StackFrame();
            String sErrorMsg = String.Empty;
            try
            {
                if (objMember == null || objMember.NUMERO < 0 || String.IsNullOrEmpty(objMember.NOME))
                    return false;

                #region     Debug
                //int i = 0;
                //if (objMember.NUMERO != 105)
                //    i = 1;
                #endregion  Debug

                #region     TreeDataSet
                DataTable objDataTable_Member_Details = new DataTable("Member_Details_Table");
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
                DataColumn objDataColumn_Member_AGREFAMIL   = new DataColumn(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.AGREFAMIL),    typeof(String)); //
                DataColumn objDataColumn_Member_NUMFILHOS   = new DataColumn(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMFILHOS),    typeof(String)); //
                DataColumn objDataColumn_Member_GAVETO      = new DataColumn(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.GAVETO),       typeof(Boolean));
                DataColumn objDataColumn_Member_QUINTINHA   = new DataColumn(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.QUINTINHA),    typeof(Boolean));
                DataColumn objDataColumn_Member_LADOMAIOR   = new DataColumn(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.LADOMAIOR),    typeof(String)); //
                DataColumn objDataColumn_Member_MAIS1FOGO   = new DataColumn(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.MAIS1FOGO),    typeof(Boolean));
                DataColumn objDataColumn_Member_HABCOLECT   = new DataColumn(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.HABCOLECT),    typeof(Boolean));
                DataColumn objDataColumn_Member_NUMFOGOS    = new DataColumn(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMFOGOS),     typeof(String)); //
                DataColumn objDataColumn_Member_ARECOMERC   = new DataColumn(new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.ARECOMERC),    typeof(Boolean));
                #endregion
                #region     Data Table Add Columns
                objDataTable_Member_Details.Columns.AddRange(
                                            new DataColumn[] {
                                                objDataColumn_Member_NUMERO,
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

                #region     Set Member Row Data
                DataRow objDataRow = objDataTable_Member_Details.NewRow();
                objDataRow[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMERO)]     = objMember.NUMERO.ToString();
                objDataRow[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NOME)]       = objMember.NOME;
                objDataRow[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.MORADA)]     = objMember.MORADA;
                objDataRow[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.CPOSTAL)]    = objMember.CPOSTAL;
                objDataRow[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.TELEFONE)]   = objMember.TELEFONE;
                objDataRow[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.TELEMOVEL)]  = objMember.TELEMOVEL;
                objDataRow[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.CC)] = objMember.CC;
                objDataRow[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NIF)] = objMember.NIF;
                objDataRow[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.EMAIL)]      = objMember.EMAIL;
                objDataRow[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.MORLOTE)]    = objMember.MORLOTE;
                objDataRow[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMLOTE)]    = objMember.NUMLOTE;
                objDataRow[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.AREALOTE)]   = objMember.AREALOTE;
                objDataRow[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.PROFISSAO)]  = objMember.PROFISSAO;
                objDataRow[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.DATAADMI)]   = objMember.DATAADMI;
                objDataRow[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.OBSERVACAO)] = objMember.OBSERVACAO;
                objDataRow[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.SECTOR)]     = objMember.SECTOR;
                objDataRow[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.CASA)]       = objMember.CASA;
                objDataRow[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.GARAGEM)]    = objMember.GARAGEM;
                objDataRow[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.MUROS)]      = objMember.MUROS;
                objDataRow[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.POCO)]       = objMember.POCO;
                objDataRow[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.FURO)]       = objMember.FURO;
                objDataRow[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.SANEAMENTO)] = objMember.SANEAMENTO;
                objDataRow[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.ELECTRICID)] = objMember.ELECTRICID;
                objDataRow[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.PROJECTO)]   = objMember.PROJECTO;
                objDataRow[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.ESCRITURA)]  = objMember.ESCRITURA;
                objDataRow[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.FINANCAS)]   = objMember.FINANCAS;
                objDataRow[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.RESIDENCIA)] = objMember.RESIDENCIA;
                objDataRow[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.AGREFAMIL)]  = objMember.AGREFAMIL;
                objDataRow[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMFILHOS)]  = objMember.NUMFILHOS;
                objDataRow[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.GAVETO)]     = objMember.GAVETO;
                objDataRow[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.QUINTINHA)]  = objMember.QUINTINHA;
                objDataRow[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.LADOMAIOR)]  = objMember.LADOMAIOR;
                objDataRow[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.MAIS1FOGO)]  = objMember.MAIS1FOGO;
                objDataRow[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.HABCOLECT)]  = objMember.HABCOLECT;
                objDataRow[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMFOGOS)]   = objMember.NUMFOGOS;
                objDataRow[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.ARECOMERC)]  = objMember.ARECOMERC;
                #endregion  Set Member Row Data

                objDataTable_Member_Details.Rows.Add(objDataRow);
                objGridControl.DataSource = objDataTable_Member_Details;

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

        /// <versions>21-02-2017(v0.0.1.6)</versions>
        public void CleanGridLayoutView(GridControl objGridControl, LayoutView objLayoutView, Boolean bClearCols, Boolean bClearSorting, Boolean bClearFilters)
        {
            try
            {
                objGridControl.Visible = true;
                objGridControl.Show();
                objGridControl.DataSource = null;
                //objLayoutView.OptionsView.ShowFooter = false;
                objLayoutView.BeginUpdate();
                if (bClearCols)
                    objLayoutView.Columns.Clear();
                if (bClearSorting)
                    objLayoutView.ClearSorting();
                if (bClearFilters)
                {
                    objLayoutView.OptionsFilter.Reset();
                    objLayoutView.ActiveFilterEnabled = false;
                }
                //if (bClearGrouping)
                //    objLayoutView.ClearGrouping();
                objLayoutView.EndUpdate();
                objGridControl.Update();
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>21-04-2017(v0.0.1.27)</versions>
        public void Add_Member_LayoutView_Columns_Options()
        {
            try
            {
                LibAMFC.Set_LayoutView_Column_Options(this.LayoutView_Member_Details, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMERO),      new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMERO),        "Número de sócio",  true, 1, 60,    true, false, true, false, false);
                LibAMFC.Set_LayoutView_Column_Options(this.LayoutView_Member_Details, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NOME),        new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.NOME),          "Nome do sócio",    true, 2, 60,    true, false, true, false, false);
                LibAMFC.Set_LayoutView_Column_Options(this.LayoutView_Member_Details, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.MORADA),      new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.MORADA),        "Morada do sócio",  true, 3, 60,    true, false, true, false, false);
                LibAMFC.Set_LayoutView_Column_Options(this.LayoutView_Member_Details, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.CPOSTAL),     new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.CPOSTAL),       "CPOSTAL",          true, 4, 60,    true, false, true, false, false);
                LibAMFC.Set_LayoutView_Column_Options(this.LayoutView_Member_Details, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.TELEFONE),    new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.TELEFONE),      "TELEFONE",         true, 5, 60,    true, false, true, false, false);
                LibAMFC.Set_LayoutView_Column_Options(this.LayoutView_Member_Details, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.TELEMOVEL),   new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.TELEMOVEL),     "TELEMOVEL",        true, 6, 60,    true, false, true, false, false);
                LibAMFC.Set_LayoutView_Column_Options(this.LayoutView_Member_Details, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.CC), new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.CC), "CC", true, 6, 60, true, false, true, false, false);
                LibAMFC.Set_LayoutView_Column_Options(this.LayoutView_Member_Details, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NIF), new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.NIF), "NIF", true, 6, 60, true, false, true, false, false);
                LibAMFC.Set_LayoutView_Column_Options(this.LayoutView_Member_Details, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.EMAIL),       new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.EMAIL),         "EMAIL",            true, 7, 60,    true, false, true, false, false);
                LibAMFC.Set_LayoutView_Column_Options(this.LayoutView_Member_Details, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.MORLOTE),     new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.MORLOTE),       "Morada do lote",   true, 8, 60,    true, false, true, false, false);
                LibAMFC.Set_LayoutView_Column_Options(this.LayoutView_Member_Details, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMLOTE),     new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMLOTE),       "Lote de sócio",    true, 9, 60,    true, false, true, false, false);
                LibAMFC.Set_LayoutView_Column_Options(this.LayoutView_Member_Details, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.AREALOTE),    new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.AREALOTE),      "Área do lote",     true, 10, 60,   true, false, true, false, false);
                LibAMFC.Set_LayoutView_Column_Options(this.LayoutView_Member_Details, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.PROFISSAO),   new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.PROFISSAO),     "PROFISSAO",        true, 11, 60,   true, false, true, false, false);
                LibAMFC.Set_LayoutView_Column_Options(this.LayoutView_Member_Details, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.DATAADMI),    new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.DATAADMI),      "DATAADMI",         true, 12, 100,  true, false, true, false, false);
                LibAMFC.Set_LayoutView_Column_Options(this.LayoutView_Member_Details, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.OBSERVACAO),  new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.OBSERVACAO),    "OBSERVACAO",       true, 13, 60,   true, false, true, false, false);
                LibAMFC.Set_LayoutView_Column_Options(this.LayoutView_Member_Details, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.SECTOR),      new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.SECTOR),        "SECTOR",           true, 14, 60,   true, false, true, false, false);
                LibAMFC.Set_LayoutView_Column_Options(this.LayoutView_Member_Details, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.CASA),        new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.CASA),          "CASA",             true, 15, 10,   true, false, true, false, true);
                LibAMFC.Set_LayoutView_Column_Options(this.LayoutView_Member_Details, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.GARAGEM),     new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.GARAGEM),       "GARAGEM",          true, 16, 10,   true, false, true, false, true);
                LibAMFC.Set_LayoutView_Column_Options(this.LayoutView_Member_Details, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.MUROS),       new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.MUROS),         "MUROS",            true, 17, 10,   true, false, true, false, true);
                LibAMFC.Set_LayoutView_Column_Options(this.LayoutView_Member_Details, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.POCO),        new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.POCO),          "POCO",             true, 18, 10,   true, false, true, false, true);
                LibAMFC.Set_LayoutView_Column_Options(this.LayoutView_Member_Details, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.FURO),        new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.FURO),          "FURO",             true, 19, 10,   true, false, true, false, true);
                LibAMFC.Set_LayoutView_Column_Options(this.LayoutView_Member_Details, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.SANEAMENTO),  new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.SANEAMENTO),    "SANEAMENTO",       true, 20, 10,   true, false, true, false, true);
                LibAMFC.Set_LayoutView_Column_Options(this.LayoutView_Member_Details, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.ELECTRICID),  new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.ELECTRICID),    "ELECTRICID",       true, 21, 10,   true, false, true, false, true);
                LibAMFC.Set_LayoutView_Column_Options(this.LayoutView_Member_Details, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.PROJECTO),    new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.PROJECTO),      "PROJECTO",         true, 22, 10,   true, false, true, false, true);
                LibAMFC.Set_LayoutView_Column_Options(this.LayoutView_Member_Details, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.ESCRITURA),   new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.ESCRITURA),     "ESCRITURA",        true, 23, 10,   true, false, true, false, true);
                LibAMFC.Set_LayoutView_Column_Options(this.LayoutView_Member_Details, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.FINANCAS),    new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.FINANCAS),      "FINANCAS",         true, 24, 10,   true, false, true, false, true);
                LibAMFC.Set_LayoutView_Column_Options(this.LayoutView_Member_Details, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.RESIDENCIA),  new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.RESIDENCIA),    "RESIDENCIA",       true, 25, 10,   true, false, true, false, true);
                LibAMFC.Set_LayoutView_Column_Options(this.LayoutView_Member_Details, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.AGREFAMIL),   new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.AGREFAMIL),     "AGREFAMIL",        true, 26, 10,   true, false, true, false, false);//
                LibAMFC.Set_LayoutView_Column_Options(this.LayoutView_Member_Details, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMFILHOS),   new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMFILHOS),     "NUMFILHOS",        true, 27, 10,   true, false, true, false, false);//
                LibAMFC.Set_LayoutView_Column_Options(this.LayoutView_Member_Details, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.GAVETO),      new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.GAVETO),        "GAVETO",           true, 28, 10,   true, false, true, false, true);
                LibAMFC.Set_LayoutView_Column_Options(this.LayoutView_Member_Details, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.QUINTINHA),   new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.QUINTINHA),     "QUINTINHA",        true, 29, 10,   true, false, true, false, true);
                LibAMFC.Set_LayoutView_Column_Options(this.LayoutView_Member_Details, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.LADOMAIOR),   new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.LADOMAIOR),     "LADOMAIOR",        true, 30, 10,   true, false, true, false, false);//
                LibAMFC.Set_LayoutView_Column_Options(this.LayoutView_Member_Details, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.MAIS1FOGO),   new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.MAIS1FOGO),     "MAIS1FOGO",        true, 31, 10,   true, false, true, false, true);
                LibAMFC.Set_LayoutView_Column_Options(this.LayoutView_Member_Details, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.HABCOLECT),   new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.HABCOLECT),     "HABCOLECT",        true, 32, 10,   true, false, true, false, true);
                LibAMFC.Set_LayoutView_Column_Options(this.LayoutView_Member_Details, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMFOGOS),    new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMFOGOS),      "NUMFOGOS",         true, 33, 10,   true, false, true, false, false);//
                LibAMFC.Set_LayoutView_Column_Options(this.LayoutView_Member_Details, new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.ARECOMERC),   new AMFCMember().Get_DBFMemberField_Alias_ByType(GesAMFC.AMFCMember.DBFMemberFields.ARECOMERC),     "ARECOMERC",        true, 34, 10,   true, false, true, false, true);

            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        #endregion  Member Details

        #region     Members Admin

        /// <versions>22-03-2017(v0.0.1.14)</versions>
        private AMFCMember Get_Focused_Member()
        {
            try
            {
                AMFCMember objFocusedMember = null;
                Int32 iFocusedMemberRowHandle = this.GridView_AMFC_Members.FocusedRowHandle;
                if (_SelectedMember == null || _SelectedMember.NUMERO < 1 || String.IsNullOrEmpty(_SelectedMember.NOME))
                {
                    objFocusedMember = Get_Selected_Member(iFocusedMemberRowHandle, -1);
                    if (objFocusedMember == null || objFocusedMember.NUMERO < 1 || String.IsNullOrEmpty(objFocusedMember.NOME))
                    {
                        //Nenhuma linha na grelha de sócios selecionada
                        //No futuro abrir caixa filtro da grelha para selecionar o sócio
                        String sWarning = "Por favor, selecione um sócio da lista (ou seja, selecionar uma linha da tabela de sócios da esquerda). Obrigado!";
                        XtraMessageBox.Show(sWarning, "Sócio não selecionado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return null;
                    }
                }
                else
                    objFocusedMember = _SelectedMember;
                return objFocusedMember;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return null;
            }
        }

        /// <versions>22-03-2017(v0.0.1.14)</versions>
        private AMFCMember Get_Selected_Member(Int32 iRowHandle, Int64 lMemberNumber)
        {
            try
            {
                AMFCMember objMember = null;
                Int32 iFocusedRowHandle     = -1;
                Int64 lFocusedMemberNumber  = -1;

                if (_CollectionSourceListMembers.Members.Count == 0)
                {
                    XtraMessageBox.Show("Lista de Sócios vazia!", "Erro [Get_Selected_Member]", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }

                if (lMemberNumber < 1)
                {
                    if (iRowHandle > -1)
                        iFocusedRowHandle = iRowHandle;
                    else
                        iFocusedRowHandle = this.GridView_AMFC_Members.FocusedRowHandle;
                    if (iFocusedRowHandle < 0 || iFocusedRowHandle == GridControl.InvalidRowHandle || !this.GridView_AMFC_Members.IsDataRow(iFocusedRowHandle))
                    {
                        XtraMessageBox.Show("Não foi possível obter os detalhes do sócio selecionado!", "Erro [Sócio Selecionado]", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return null;
                    }
                    lFocusedMemberNumber  = Convert.ToInt64(this.GridView_AMFC_Members.GetRowCellValue(iFocusedRowHandle, this.GridView_AMFC_Members.Columns[new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMERO)]));
                }
                else
                    lFocusedMemberNumber = lMemberNumber;

                if (lFocusedMemberNumber == _SelectedMember.NUMERO)
                    return _SelectedMember;
                objMember = _CollectionSourceListMembers.GetMemberByNumber(lFocusedMemberNumber);
                if (objMember == null || objMember.NUMERO < 1 || String.IsNullOrEmpty(objMember.NOME))
                {
                    XtraMessageBox.Show("Não foi possível obter os detalhes do sócio selecionado!", "Erro [Sócio Selecionado]", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
                _SelectedMember = objMember;
                _CollectionSourceListMembers.SelectedMember = _SelectedMember;

                return objMember;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return null;
            }
        }

        /// <versions>22-03-2017(v0.0.1.14)</versions>
        private Boolean IsTheSameSelectedMember(Int32 iRowHandle, Int64 lMemberNumber)
        {
            try
            {
                Int32 iFocusedRowHandle = -1;
                Int64 lFocusedMemberNumber = -1;

                if (lMemberNumber < 1)
                {
                    if (iRowHandle > -1)
                        iFocusedRowHandle = iRowHandle;
                    else
                        iFocusedRowHandle = this.GridView_AMFC_Members.FocusedRowHandle;
                    if (iFocusedRowHandle < 0 || iFocusedRowHandle == GridControl.InvalidRowHandle || !this.GridView_AMFC_Members.IsDataRow(iFocusedRowHandle))
                    {
                        XtraMessageBox.Show("Não foi possível obter os detalhes do sócio selecionado!", "Erro [Sócio Selecionado]", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    lFocusedMemberNumber  = Convert.ToInt64(this.GridView_AMFC_Members.GetRowCellValue(iFocusedRowHandle, this.GridView_AMFC_Members.Columns[new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMERO)]));
                }
                else
                    lFocusedMemberNumber = lMemberNumber;
                if (lFocusedMemberNumber == _SelectedMember.NUMERO)
                    return true;

                return false;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return false;
            }
        }

        /// <versions>28-04-2017(v0.0.2.48)</versions>
        private Boolean DBF_AMFC_Del_Member(AMFCMember objMember)
        {
            try
            {
                using (Library_AMFC_SQL obj_AMFC_SQL = new Library_AMFC_SQL())
                { 
                    return obj_AMFC_SQL.Del_Member(objMember);
                }
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return false;
            }
        }

        /// <versions>03-05-2017(v0.0.2.51)</versions>
        private void Find_Member()
        {
            try
            {
                #region     Find Member Form
                Find_Member_Form objFindMemberForm = new Find_Member_Form();
                objFindMemberForm.FormClosing += delegate
                {
                    #region     Member Found
                    if (Program.Member_Found)
                    {
                        #region     Find Member Selected
                        AMFCMember objMemberSelected = objFindMemberForm.MemberSelected;
                        if (objMemberSelected != null && objMemberSelected.NUMERO > 0 && objMemberSelected.NUMERO < objMemberSelected.MaxNumber && !String.IsNullOrEmpty(objMemberSelected.NOME) && !String.IsNullOrEmpty(objMemberSelected.MORADA) && !String.IsNullOrEmpty(objMemberSelected.MORLOTE) && !String.IsNullOrEmpty(objMemberSelected.CPOSTAL) && !String.IsNullOrEmpty(objMemberSelected.NUMLOTE))
                        {
                            _SelectedMember = objMemberSelected;
                            Load_Grid_Members(false, false, false, false, true, true, -1, objMemberSelected.NUMERO);
                        }
                        #endregion  Find Member Selected
                    }
                    #endregion  Member Found
                };
                objFindMemberForm.Show();
                objFindMemberForm.StartPosition = FormStartPosition.CenterParent;
                objFindMemberForm.Focus();
                objFindMemberForm.BringToFront();
                return;
                #endregion  Find Member Form
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return;
            }
        }

        /// <versions>05-05-2017(v0.0.2.56)</versions>
        private void Member_Details_Click()
        {
            try
            {
                #region     Get Focused Member
                AMFCMember objFocusedMember = Get_Focused_Member();
                if (objFocusedMember == null || objFocusedMember.NUMERO < 1)
                    return;
                #endregion  Get Focused Member

                #region     Check if user wants to edit member
                DialogResult objDialogResult1 = XtraMessageBox.Show("Deseja editar o Sócio Nº: " + objFocusedMember.NUMERO + " (" + objFocusedMember.NOME + ")" + "?", "Editar sócio?", MessageBoxButtons.OK, MessageBoxIcon.Question);
                if (objDialogResult1 == DialogResult.OK)
                    Member_Edit_Click();
                #endregion  Check if user wants to edit member
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return;
            }
        }

        /// <versions>05-05-2017(v0.0.2.56)</versions>
        private void Member_Add_Click()
        {
            try
            {
                if (!Program.AppUser.CanAdd)
                {
                    XtraMessageBox.Show("Este utilizador não possui permissões para " + "adicionar" + " sócios! Por favor, sai da aplicação e entre novamente com a password de administrador.", "Utilizador [não adminsitrador]", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }
                this.Operation = Library_AMFC_Methods.MemberOperationType.ADD;
                Member_Form objMember_Form = new Member_Form(null, this.Operation);
                objMember_Form.WindowState = FormWindowState.Maximized;
                objMember_Form.FormClosing += delegate
                {
                    if (Program.Member_Added)
                    {
                        #region     Get New Member
                        AMFCMember objNewMember = objMember_Form.Member;
                        if (objNewMember != null)
                            _SelectedMember = objNewMember;
                        else
                            objNewMember = _SelectedMember;
                        #endregion  Get New Member
                        Load_Grid_Members(false, false, false, false, true, true, -1, objNewMember.NUMERO);
                    }
                };
                objMember_Form.Show();
                objMember_Form.BringToFront();
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>05-05-2017(v0.0.2.56)</versions>
        private void Member_Edit_Click()
        {
            try
            {
                if (!Program.AppUser.CanEdit)
                {
                    XtraMessageBox.Show("Este utilizador não possui permissões para " + "editar" + " sócios! Por favor, sai da aplicação e entre novamente com a password de administrador.", "Utilizador [não adminsitrador]", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                #region     Get Focused Member
                AMFCMember objFocusedMember = Get_Focused_Member();
                if (objFocusedMember == null || objFocusedMember.NUMERO < 1)
                    return;
                #endregion  Get Focused Member

                #region     Open Member Edit Form
                this.Operation = Library_AMFC_Methods.MemberOperationType.EDIT;
                Member_Form objMember_Form = new Member_Form(objFocusedMember, Operation);
                objMember_Form.WindowState = FormWindowState.Normal;
                objMember_Form.FormClosing += delegate
                {
                    if (Program.Member_Edited)
                    {
                        #region     Get Edit Member
                        AMFCMember objEditedMember = objMember_Form.Member;
                        if (objEditedMember != null)
                            _SelectedMember = objEditedMember;
                        else
                            objEditedMember = _SelectedMember;
                        #endregion  Get Edit Member
                        Load_Grid_Members(false, false, false, false, true, false, -1, objEditedMember.NUMERO);
                    }
                };
                objMember_Form.Show();
                objMember_Form.BringToFront();
                objMember_Form.Location = new System.Drawing.Point(900, 200);
                #endregion  Open Member Edit Form
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>05-05-2017(v0.0.2.56)</versions>
        private void Member_Del_Click()
        {
            try
            {
                if (!Program.AppUser.CanDel)
                {
                    XtraMessageBox.Show("Este utilizador não possui permissões para " + "apagar" + " sócios! Por favor, sai da aplicação e entre novamente com a password de administrador.", "Utilizador [não adminsitrador]", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                #region     Get Focused Member
                AMFCMember objFocusedMember = Get_Focused_Member();
                if (objFocusedMember == null || objFocusedMember.NUMERO < 1)
                    return;
                #endregion  Get Focused Member

                #region     Member Delete Confirmation
                DialogResult objDialogResult1 = XtraMessageBox.Show("Eliminar o Sócio Nº: " + objFocusedMember.NUMERO + " (" + objFocusedMember.NOME + ")" + "?", "Deseja eliminar o sócio?", MessageBoxButtons.OK, MessageBoxIcon.Question);
                if (objDialogResult1 == DialogResult.OK)
                {
                    DialogResult objDialogResult2 = MessageBox.Show("Tem a certeza que pretende apagar o Sócio Nº: " + objFocusedMember.NUMERO + " (" + objFocusedMember.NOME + ")" + "?", "Confirmar eliminar o sócio?", MessageBoxButtons.OK, MessageBoxIcon.Question);
                    if (objDialogResult2 != DialogResult.OK)
                        return;
                }
                else
                    return;
                #endregion  Member Delete Confirmation

                this.Operation = Library_AMFC_Methods.MemberOperationType.DEL;

                #region     Delete Operation
                Int32 iFocusedMemberRowHandle = this.GridView_AMFC_Members.FocusedRowHandle;
                if (iFocusedMemberRowHandle > 1)
                    iFocusedMemberRowHandle--;
                else
                    iFocusedMemberRowHandle = 0;
                if (!DBF_AMFC_Del_Member(objFocusedMember))
                {
                    String sWarning = "Não foi possível eliminar o Sócio Nº: " + objFocusedMember.NUMERO;
                    MessageBox.Show(sWarning, "Erro: Eliminar Sócio", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                #endregion  Delete Operation

                Load_Grid_Members(false, false, false, false, true, true, iFocusedMemberRowHandle, -1);
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return;
            }
        }

        #endregion  Members Admin

        #endregion  Private Methods

        #endregion  Methods
    }
}