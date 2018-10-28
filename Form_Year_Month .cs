using DevExpress.XtraEditors.Controls;
using GesAMFC.AMFC_Methods;
using System;

namespace GesAMFC
{
    /// <summary>Find Member Form</summary>
    /// <creation>20-04-2017(v0.0.1.23)</creation>
    /// <versions>02-12-2017(GesAMFC-v0.0.4.41)</versions>
    public partial class Form_Year_Month : DevExpress.XtraEditors.XtraForm
    {
        public Library_AMFC_Methods LibAMFC;

        public AMFCYears    ListYears;
        public AMFCMonths   ListMonths;

        public Boolean      IsYearMonthSelected;
        public AMFCYear     YearSelected;
        public AMFCMonth    MonthSelected;
        public Boolean      EveryYear;

        #region Constructor

        /// <versions>01-12-2017(GesAMFC-v0.0.4.41)</versions>
        public Form_Year_Month(AMFCYear objYearSelected, AMFCMonth objMonthSelected)
        {
            this.LibAMFC = new Library_AMFC_Methods();
            try
            {
                InitializeComponent();

                ListYears   = new AMFCYears();
                ListYears.SetYearList();
                ListMonths  = new AMFCMonths();
                ListMonths.SetMonthList();

                this.IsYearMonthSelected = false;
                this.EveryYear = false;

                if (Program.IsValidYear(objYearSelected.Value))
                    this.YearSelected = objYearSelected;
                else
                    this.YearSelected = Program.DefaultYear;

                if (Program.IsValidMonth(objMonthSelected.Value))
                    this.MonthSelected = objMonthSelected;
                else
                    this.MonthSelected = Program.DefaultMonth;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        #endregion Constructor

        #region     Events

        /// <versions>01-12-2017(GesAMFC-v0.0.4.41)</versions>
        private void Form_Year_Month_Load(object sender, EventArgs e)
        {
            try
            {
                SetYearComboList();
                SetMonthComboList();
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>01-12-2017(GesAMFC-v0.0.4.41)</versions>
        private void Form_Year_Month_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            try
            {
                //if (!Program.IsValidYear(this.YearSelected.Value) || (!Program.IsValidMonth(this.MonthSelected.Value) && !this.EveryYear))
                //      this.IsYearMonthSelected = false;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>01-12-2017(GesAMFC-v0.0.4.41)</versions>
        private void ComboBoxEdit_Year_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ComboboxItem objItemYear = (ComboBoxEdit_Year.SelectedItem as ComboboxItem);
                this.YearSelected = new AMFCYear(Convert.ToInt32(objItemYear.GetValue()), objItemYear.GetText());
                if (!Program.IsValidYear(this.YearSelected.Value))
                    this.YearSelected = new AMFCYear(DateTime.Today.Year);
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>01-12-2017(GesAMFC-v0.0.4.41)</versions>
        private void ComboBoxEdit_Month_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ComboboxItem objItemMonth = (ComboBoxEdit_Month.SelectedItem as ComboboxItem);
                Int32 iSelectedMonth = Convert.ToInt32(objItemMonth.GetValue());
                if (iSelectedMonth == 0)
                {
                    this.EveryYear = true;
                    this.MonthSelected.Value = -1;
                    return;
                }
                else
                {
                    this.MonthSelected = new AMFCMonth(iSelectedMonth, objItemMonth.Text);
                    if (!Program.IsValidMonth(this.MonthSelected.Value))
                        this.MonthSelected = new AMFCMonth(DateTime.Today.Month);
                    this.EveryYear = false;
                }
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>01-12-2017(GesAMFC-v0.0.4.41)</versions>
        private void Button_Cancel_Click(object sender, EventArgs e)
        {
            try
            {
                this.IsYearMonthSelected = false;
                this.Close();
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>01-12-2017(GesAMFC-v0.0.4.41)</versions>

        private void Button_OK_Click(object sender, EventArgs e)
        {
            if (ListYears.ValidYearItem(this.YearSelected) && ListMonths.ValidMonthItem(this.MonthSelected))
                this.IsYearMonthSelected = true;
            else
                this.IsYearMonthSelected = false;
            this.Close();
        }

        #endregion  Events

        #region     Private Methods

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

        /// <versions>01-12-2017(GesAMFC-v0.0.4.41)</versions>
        private void SetMonthComboList()
        {
            try
            {
                Int32 iSelecteIndex = 0;
                
                ComboBoxEdit_Month.Properties.Items.Clear();

                ComboboxItem objComboBoxItemTodos = new ComboboxItem(0, "-- Todos --");
                ComboBoxEdit_Month.Properties.Items.Add(objComboBoxItemTodos);

                Int32 iIndex = 0;
                foreach (AMFCMonth objMonth in ListMonths.List)
                {
                    ComboboxItem objComboBoxItem = new ComboboxItem(objMonth.Value, objMonth.Description);
                    if (objMonth.Value == this.MonthSelected.Value)
                        iSelecteIndex = iIndex;
                    ComboBoxEdit_Month.Properties.Items.Add(objComboBoxItem);
                    iIndex++;
                }
                ComboBoxEdit_Month.SelectedIndex = iSelecteIndex;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        #endregion  Private Methods
    }
}