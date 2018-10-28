using DevExpress.XtraEditors.Controls;
using DevExpress.Spreadsheet;

using GesAMFC.AMFC_Methods;
using System;
using System.IO;
using System.Windows.Forms;
using DevExpress.XtraEditors;

namespace GesAMFC
{
    /// <summary>Set Value To Pay Form</summary>
    /// <creation>20-02-2018(GesAMFC-v1.0.0.3)</creation>
    /// <versions>20-02-2018(GesAMFC-v1.0.0.3)</versions>
    public partial class Form_Recibo_Quotas : DevExpress.XtraEditors.XtraForm
    {
        public Library_AMFC_Methods LibAMFC;
        public AMFCMember Member;
        public AMFCYears ListYears;
        public AMFCYear YearSelected;

        #region     Constructor

        /// <versions>20-02-2018(GesAMFC-v1.0.0.3)</versions>
        public Form_Recibo_Quotas(AMFCMember objMember, AMFCYear objYearSelected)
        {
            this.LibAMFC = new Library_AMFC_Methods();
            try
            {
                InitializeComponent();

                Member = new AMFCMember();

                ListYears = new AMFCYears();
                ListYears.SetYearList(2000, 2018);

                if (Program.IsValidYear(objYearSelected.Value))
                    this.YearSelected = objYearSelected;
                else
                    this.YearSelected = Program.DefaultYear;

                if (objMember != null && objMember.NUMERO >= objMember.MinNumber && objMember.NUMERO < objMember.MaxNumber && !String.IsNullOrEmpty(objMember.NOME))
                {
                    this.Member = objMember;
                    TextEdit_SOCIO.Text = this.Member.NUMERO.ToString();
                    TextEdit_NOME.Text = this.Member.NOME;
                }
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        #endregion  Constructor

        #region     Events

        /// <versions>20-02-2018(GesAMFC-v1.0.0.3)</versions>
        private void Form_Recibo_Quotas_Load(object sender, EventArgs e)
        {
            try
            {
                SetYearComboList();
                //if (this.Member == null || this.Member.NUMERO < this.Member.MinNumber || this.Member.NUMERO >= this.Member.MaxNumber || String.IsNullOrEmpty(this.Member.NOME))
                //    Find_Member();
                Load_Member_Quotas_Recibo();
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>20-02-2018(GesAMFC-v1.0.0.3)</versions>
        private void Button_Print_Click(object sender, EventArgs e)
        {
            SpreadsheetControl_Recibo_Quotas.ShowPrintDialog();
        }

        /// <versions>20-02-2018(GesAMFC-v1.0.0.3)</versions>
        //private void Button_Save_Click(object sender, EventArgs e)
        //{
        //    IWorkbook workbook = SpreadsheetControl_Recibo_Quotas.Document;

        //    String sRecibosQuotasDirPath = "Export\\Excel\\RECIBOS\\QUOTAS";
        //    String sRecibosQuotasFileName = "Recibo_Quotas" + "_" + "2018" + "_socio" + "" + "." + "xls";
        //    String sRecibosQuotasFiePath = sRecibosQuotasDirPath + "\\" + sRecibosQuotasFileName;

        //    #region     Save Backup Copy
        //    using (FileStream stream = new FileStream(sRecibosQuotasFiePath, FileMode.Create, FileAccess.ReadWrite))
        //        workbook.SaveDocument(stream, DocumentFormat.Xls);
        //    #endregion  Save Backup Copy

        //    #region     Save Document Where Use 
        //    using (FileStream stream = new FileStream(sRecibosQuotasFiePath, FileMode.Create, FileAccess.ReadWrite))
        //        workbook.SaveDocument(stream, DocumentFormat.Xls);
        //    #endregion  Save Document Where Use 
        //}

        /// <versions>20-02-2018(GesAMFC-v1.0.0.3)</versions>
        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Find_Member();
        }

        ///// <versions>20-02-2018(GesAMFC-v1.0.0.3)</versions>
        //private void ComboBoxEdit_Year_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        ComboboxItem objItemYear = (ComboBoxEdit_Year.SelectedItem as ComboboxItem);
        //        this.YearSelected = new AMFCYear(Convert.ToInt32(objItemYear.GetValue()), objItemYear.GetText());
        //        if (!Program.IsValidYear(this.YearSelected.Value))
        //            this.YearSelected = new AMFCYear(DateTime.Today.Year);
        //    }
        //    catch (Exception ex)
        //    {
        //        Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
        //    }
        //}

        /// <versions>20-02-2018(GesAMFC-v1.0.0.3)</versions>
        private void Button_Create_Receipt_Click(object sender, EventArgs e)
        {
            try
            {
                if (
                        !Program.IsValidYear(this.YearSelected.Value)
                        ||
                        (this.Member == null || this.Member.NUMERO < this.Member.MinNumber || this.Member.NUMERO >= this.Member.MaxNumber || String.IsNullOrEmpty(this.Member.NOME))
                    )
                {
                    String sWarning = "Por favor, selecione um sócio!";
                    XtraMessageBox.Show(sWarning, "Sócio não selecionado", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                Load_Member_Quotas_Recibo();
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        #endregion  Events

        #region     Methods

        /// <versions>20-02-2018(GesAMFC-v1.0.0.3)</versions>
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
                        if (objMemberSelected != null && objMemberSelected.NUMERO >= objMemberSelected.MinNumber && objMemberSelected.NUMERO < objMemberSelected.MaxNumber && !String.IsNullOrEmpty(objMemberSelected.NOME))
                        {
                            this.Member = objMemberSelected;
                            TextEdit_SOCIO.Text = this.Member.NUMERO.ToString();
                            TextEdit_NOME.Text = this.Member.NOME;
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

        /// <versions>20-02-2018(GesAMFC-v1.0.0.3)</versions>
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
                    if (objYear.Value == this.YearSelected.Value)
                        iSelecteIndex = iIndex;
                    ComboBoxEdit_Year.Properties.Items.Add(objComboBoxItem);
                    iIndex++;
                }
                ComboBoxEdit_Year.SelectedIndex = iSelecteIndex;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>20-02-2018(GesAMFC-v1.0.0.3)</versions>
        private void Load_Member_Quotas_Recibo()
        {
            try
            {
                String sFilePath = "Templates" + "/" + "Impresso_Quotas_Minuta" + "." + "xls";
                SpreadsheetControl_Recibo_Quotas.LoadDocument(sFilePath, DevExpress.Spreadsheet.DocumentFormat.Xls);

                if (
                        Program.IsValidYear(this.YearSelected.Value)
                        &&
                        (this.Member != null && this.Member.NUMERO >= this.Member.MinNumber && this.Member.NUMERO < this.Member.MaxNumber && !String.IsNullOrEmpty(this.Member.NOME))
                    )
                {
                    Double dQuotaMonthValue = Program.Get_Current_Parameter_QUOTA_Valor_Mes();
                    String sQuotaMonthValue = Program.SetPayCurrencyEuroStringValue(dQuotaMonthValue, Program.FormatString_Double2_Euro);
                    //Double dQuotaYearValue = Convert.ToDouble(Decimal.Multiply(Convert.ToDecimal(dQuotaMonthValue), 12));
                    Double dQuotaYearValue = Program.Get_Current_Parameter_QUOTA_Valor_Ano();
                    String sQuotaYearValue = Program.SetPayCurrencyEuroStringValue(dQuotaYearValue, Program.FormatString_Double2_Euro);

                    IWorkbook workbook = SpreadsheetControl_Recibo_Quotas.Document;
                    Worksheet objWorksheet = workbook.Worksheets[0];
                    //objWorksheet.Rows[1].RowHeight = 1.5;
                    //objWorksheet.Columns[1].ColumnWidth = 1.0;
                    //objWorksheet.Columns[1].Alignment.Vertical = SpreadsheetVerticalAlignment.Center;
                    //objWorksheet.Columns[2].ColumnWidth = 2.5;
                    //objWorksheet.Columns[2].Alignment.WrapText = true;
                    //objWorksheet.Cells["C2"].Formula = "FIELDPICTURE(\"Photo\", \"range\", C2, FALSE, 50)";
                    objWorksheet.Cells["J3"].Value = "Sócio Nº: " + this.Member.NUMERO;
                    objWorksheet.Cells["D7"].Value = this.Member.NOME;
                    objWorksheet.Cells["J4"].Value = sQuotaYearValue;
                    objWorksheet.Cells["F9"].Value = "Quotas  " + this.YearSelected.Value.ToString();
                    objWorksheet.Cells["F12"].Value = "Quotas  " + this.YearSelected.Value.ToString();

                    objWorksheet.Cells["C13"].Value = "Sócio nº: " + this.Member.NUMERO;
                    objWorksheet.Cells["C14"].Value = "Sócio nº: " + this.Member.NUMERO;
                    objWorksheet.Cells["C15"].Value = "Sócio nº: " + this.Member.NUMERO;
                    objWorksheet.Cells["C16"].Value = "Sócio nº: " + this.Member.NUMERO;
                    objWorksheet.Cells["C17"].Value = "Sócio nº: " + this.Member.NUMERO;
                    objWorksheet.Cells["C18"].Value = "Sócio nº: " + this.Member.NUMERO;
                    objWorksheet.Cells["C19"].Value = "Sócio nº: " + this.Member.NUMERO;
                    objWorksheet.Cells["C20"].Value = "Sócio nº: " + this.Member.NUMERO;
                    objWorksheet.Cells["C21"].Value = "Sócio nº: " + this.Member.NUMERO;
                    objWorksheet.Cells["C22"].Value = "Sócio nº: " + this.Member.NUMERO;
                    objWorksheet.Cells["C23"].Value = "Sócio nº: " + this.Member.NUMERO;
                    objWorksheet.Cells["C24"].Value = "Sócio nº: " + this.Member.NUMERO;

                    objWorksheet.Cells["J13"].Value = sQuotaMonthValue;
                    objWorksheet.Cells["J14"].Value = sQuotaMonthValue;
                    objWorksheet.Cells["J15"].Value = sQuotaMonthValue;
                    objWorksheet.Cells["J16"].Value = sQuotaMonthValue;
                    objWorksheet.Cells["J17"].Value = sQuotaMonthValue;
                    objWorksheet.Cells["J18"].Value = sQuotaMonthValue;
                    objWorksheet.Cells["J19"].Value = sQuotaMonthValue;
                    objWorksheet.Cells["J20"].Value = sQuotaMonthValue;
                    objWorksheet.Cells["J21"].Value = sQuotaMonthValue;
                    objWorksheet.Cells["J22"].Value = sQuotaMonthValue;
                    objWorksheet.Cells["J23"].Value = sQuotaMonthValue;
                    objWorksheet.Cells["J24"].Value = sQuotaMonthValue;

                }
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        #endregion  Methods
    }
}