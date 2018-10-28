using DevExpress.XtraEditors.Controls;
using DevExpress.Spreadsheet;

using GesAMFC.AMFC_Methods;
using System;
using System.IO;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using System.Globalization;
using System.Diagnostics;

namespace GesAMFC
{
    /// <summary>Set Value To Pay Form</summary>
    /// <creation>23-02-2018(GesAMFC-v1.0.0.3)</creation>
    /// <versions>23-03-2018(GesAMFC-v1.0.0.3)</versions>
    public partial class Form_Folha_Caixa : DevExpress.XtraEditors.XtraForm
    {
        public Library_AMFC_Methods LibAMFC;

        public Library_AMFC_SQL Lib_AMFC_SQL;

        //public AMFCYears ListYears;
        //public AMFCYear YearSelected;
        //public AMFCMonths ListMonths;
        //public AMFCMonth MonthSelected;

        public DateTime DateSelected;

        #region     Constructor

        /// <versions>23-03-2018(GesAMFC-v1.0.0.3)</versions>
        //public Form_Folha_Caixa(AMFCYear objYearSelected, AMFCMonth objMonthSelected)
        public Form_Folha_Caixa(DateTime dtDateSelected)
        {
            this.LibAMFC = new Library_AMFC_Methods();
            try
            {
                this.Lib_AMFC_SQL = new Library_AMFC_SQL();

                InitializeComponent();

                //ListYears = new AMFCYears();
                //ListYears.SetYearList(2000, DateTime.Today.Year);

                //ListMonths = new AMFCMonths();
                //ListMonths.SetMonthList();

                //if (Program.IsValidYear(objYearSelected.Value))
                //    this.YearSelected = objYearSelected;
                //else
                //    this.YearSelected = Program.DefaultYear;

                //if (Program.IsValidMonth(objMonthSelected.Value))
                //    this.MonthSelected = objMonthSelected;
                //else
                //    this.MonthSelected = Program.DefaultMonth;

                if (Program.IsValidDateTime(dtDateSelected))
                    this.DateSelected = dtDateSelected;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        #endregion  Constructor

        #region     Events

        /// <versions>23-03-2018(GesAMFC-v1.0.0.3)</versions>
        private void Form_Folha_Caixa_Load(object sender, EventArgs e)
        {
            try
            {
                //SetYearComboList();
                //SetMonthComboList();
                Load_Folha_Caixa();
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>23-03-2018(GesAMFC-v1.0.0.3)</versions>
        private void Button_Print_Click(object sender, EventArgs e)
        {
            SpreadsheetControl_Folha_Caixa.ShowPrintDialog();
        }

        /// <versions>23-03-2018(GesAMFC-v1.0.0.3)</versions>
        //private void Button_Save_Click(object sender, EventArgs e)
        //{
        //    IWorkbook workbook = SpreadsheetControl_Folha_Caixa.Document;

        //    String sFolha_CaixasDirPath = "Export\\Excel\\FOLHAS_CAIXA";
        //    String sFolha_CaixaFileName = "Folha_Caixa" + "_" + "2018" + "_socio" + "" + "." + "xls";
        //    String sFolha_CaixaFiePath = sFolha_CaixaDirPath + "\\" + sFolha_CaixaFileName;

        //    #region     Save Backup Copy
        //    using (FileStream stream = new FileStream(sFolha_CaixaFiePath, FileMode.Create, FileAccess.ReadWrite))
        //        workbook.SaveDocument(stream, DocumentFormat.Xls);
        //    #endregion  Save Backup Copy

        //    #region     Save Document Where Use 
        //    using (FileStream stream = new FileStream(sFolha_CaixaFiePath, FileMode.Create, FileAccess.ReadWrite))
        //        workbook.SaveDocument(stream, DocumentFormat.Xls);
        //    #endregion  Save Document Where Use 
        //}

        /// <versions>23-03-2018(GesAMFC-v1.0.0.3)</versions>
        private void Button_Create_Folha_Caixa_Click(object sender, EventArgs e)
        {
            try
            {
                if (
                        //!Program.IsValidYear(this.YearSelected.Value)
                        //||
                        //!Program.IsValidMonth(this.MonthSelected.Value)
                        !Program.IsValidDateTime(this.DateSelected)
                    )
                {
                    String sWarning = "Data não é válida!";
                    XtraMessageBox.Show(sWarning, "Data de Caixa Inválida", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                DialogResult objDialogResult = XtraMessageBox.Show("Deseja recarregar os pagamento em caixa para o dia: " + this.DateSelected.ToString("dd/MM/yyyy", CultureInfo.CurrentCulture) + " ?", "Recarregar Pagamentos em Caixa ?", MessageBoxButtons.OKCancel, MessageBoxIcon.Question);
                if (objDialogResult == DialogResult.OK)
                    Load_Folha_Caixa();
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        #endregion  Events

        #region     Methods

        /// <versions>23-03-2018(GesAMFC-v1.0.0.3)</versions>
        //private void SetYearComboList()
        //{
        //    try
        //    {
        //        Int32 iSelecteIndex = 0;
        //        Int32 iIndex = 0;
        //        ComboBoxEdit_Year.Properties.Items.Clear();
        //        foreach (AMFCYear objYear in ListYears.List)
        //        {
        //            ComboboxItem objComboBoxItem = new ComboboxItem(objYear.Value, objYear.Description);
        //            if (objYear.Value == this.YearSelected.Value)
        //                iSelecteIndex = iIndex;
        //            ComboBoxEdit_Year.Properties.Items.Add(objComboBoxItem);
        //            iIndex++;
        //        }
        //        ComboBoxEdit_Year.SelectedIndex = iSelecteIndex;
        //    }
        //    catch (Exception ex)
        //    {
        //        Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
        //    }
        //}

        /// <versions>23-03-2018(GesAMFC-v1.0.0.3)</versions>
        //private void SetMonthComboList()
        //{
        //    try
        //    {
        //        Int32 iSelecteIndex = 0;
        //        Int32 iIndex = 0;
        //        ComboBoxEdit_Month.Properties.Items.Clear();
        //        foreach (AMFCMonth objMonth in ListMonths.List)
        //        {
        //            ComboboxItem objComboBoxItem = new ComboboxItem(objMonth.Value, objMonth.Description);
        //            if (objMonth.Value == this.MonthSelected.Value)
        //                iSelecteIndex = iIndex;
        //            ComboBoxEdit_Month.Properties.Items.Add(objComboBoxItem);
        //            iIndex++;
        //        }
        //        ComboBoxEdit_Month.SelectedIndex = iSelecteIndex;
        //    }
        //    catch (Exception ex)
        //    {
        //        Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
        //    }
        //}

        /// <versions>23-03-2018(GesAMFC-v1.0.0.3)</versions>
        private void Load_Folha_Caixa()
        {
            try
            {
                String sFilePath = "Templates" + "/" + "Folha_Caixa_Minuta" + "." + "xls";
                SpreadsheetControl_Folha_Caixa.LoadDocument(sFilePath, DevExpress.Spreadsheet.DocumentFormat.Xls);

                if (
                        //Program.IsValidYear(this.YearSelected.Value)
                        //&&
                        //Program.IsValidMonth(this.MonthSelected.Value)
                        Program.IsValidDateTime(this.DateSelected)
                    )
                {
                    AMFCCashPayments objAMFCCashPayments = Get_DBF_Date_Folha_Caixa();
                    if (objAMFCCashPayments == null)
                        return;

                    IWorkbook workbook = SpreadsheetControl_Folha_Caixa.Document;
                    Worksheet objWorksheet = workbook.Worksheets[0];
                    objWorksheet.Cells["B2"].Value = "FOLHA DE CAIXA DE " + this.DateSelected.ToString("dd/MM/yyyy", CultureInfo.CurrentCulture);

                    String sCol_Socio_Numero    = "B";
                    String sCol_Socio_Nome      = "C";
                    String sCol_Pay_Descricao   = "D";
                    String sCol_Pay_Valor       = "E";
                    Int32 iRowsValuesStartIdx = 4;
                    Int32 iRowTotalIdx = 30;
                    foreach (AMFCCashPayment objPayment in objAMFCCashPayments.Payments)
                    {
                        objWorksheet.Cells[sCol_Socio_Numero + iRowsValuesStartIdx].Value = objPayment.SOCIO;
                        objWorksheet.Cells[sCol_Socio_Nome + iRowsValuesStartIdx].Value = objPayment.NOME;
                        objWorksheet.Cells[sCol_Pay_Descricao + iRowsValuesStartIdx].Value = objPayment.DESIGNACAO;
                        objWorksheet.Cells[sCol_Pay_Valor + iRowsValuesStartIdx].Value = objPayment.VALOR;
                        iRowsValuesStartIdx++;
                    }
                    if (iRowsValuesStartIdx < iRowTotalIdx)
                        objWorksheet.Rows.Remove(iRowsValuesStartIdx, iRowTotalIdx - iRowsValuesStartIdx - 1);
                    //else //Tinha de se formatrar as linhas tb dp de adicioná-las: tipo de letra, agrupadas e tipo de célula
                    //    objWorksheet.Rows.Insert(iRowsValuesStartIdx - 1, iRowsValuesStartIdx - iRowTotalIdx); //´Qdo for mais de 30 adicionar as linhas no template
                }
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>23-03-2018(GesAMFC-v1.0.0.3)</versions>
        private AMFCCashPayments Get_DBF_Date_Folha_Caixa()
        {
            StackFrame objStackFrame = new StackFrame();
            try
            {
                if (!Program.IsValidDateTime(this.DateSelected))
                    return null;
                AMFCCashPayments objAMFCCashPayments = this.Lib_AMFC_SQL.Get_All_Cash_Payments(this.DateSelected);
                if (objAMFCCashPayments == null)
                {
                    String sErrorMsg = "Não foi possível obter a Lista de Pagamentos!";
                    Program.HandleError(objStackFrame.GetMethod().Name, sErrorMsg, Program.ErroType.ERROR, true, false);
                    return null;
                }
                if (objAMFCCashPayments.Payments.Count == 0)
                {
                    String sWarningMsg = "Não existem Registos na Caixa de Pagamentos para esta Data: " + this.DateSelected.ToString("dd/MM/yyyy", CultureInfo.CurrentCulture);
                    MessageBox.Show(sWarningMsg, "Data Sem Pagamentos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    this.Close();
                    return null;
                }

                return objAMFCCashPayments;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return null;
            }
            finally { objStackFrame = null; }
        }

        #endregion  Methods
    }
}