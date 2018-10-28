using DevExpress.XtraEditors.Controls;
using GesAMFC.AMFC_Methods;
using System;
using System.Windows.Forms;

namespace GesAMFC
{
    /// <summary>Select Period Start End Years</summary>
    /// <creation>05-12-2017(GesAMFC-v0.0.4.41)</creation>
    /// <versions>05-12-2017(GesAMFC-v0.0.4.41)</versions>
    public partial class Form_Period_Years : DevExpress.XtraEditors.XtraForm
    {
        public Library_AMFC_Methods LibAMFC;

        public AMFCYears        ListYears;

        public Boolean          IsPeriodSelected;
        public AMFCPeriodYears  PeriodSelected;

        #region Constructor

        //// <versions>05-12-2017(GesAMFC-v0.0.4.41)</versions>
        public Form_Period_Years(AMFCPeriodYears objPeriodSelected)
        {
            this.LibAMFC = new Library_AMFC_Methods();
            try
            {
                InitializeComponent();

                ListYears   = new AMFCYears();
                ListYears.SetYearList();

                this.IsPeriodSelected = false;

                this.PeriodSelected = new AMFCPeriodYears();
                this.PeriodSelected.Start = new AMFCPeriodYear();
                this.PeriodSelected.End = new AMFCPeriodYear();
                if (objPeriodSelected != null)
                {
                    if (objPeriodSelected.Start != null && Program.IsValidYear(objPeriodSelected.Start.Year.Value))
                        this.PeriodSelected.Start = objPeriodSelected.Start;
                    else
                        this.PeriodSelected.Start.Year = Program.DefaultYear;

                    if (objPeriodSelected.End != null && Program.IsValidYear(objPeriodSelected.End.Year.Value))
                        this.PeriodSelected.End = objPeriodSelected.End;
                    else
                        this.PeriodSelected.End.Year = Program.DefaultYear;
                }
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        #endregion Constructor

        #region     Events

        /// <versions>05-12-2017(GesAMFC-v0.0.4.41)</versions>
        private void Form_Period_Years_Load(object sender, EventArgs e)
        {
            try
            {
                SetStartYearComboList();

                SetEndYearComboList();
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>05-12-2017(GesAMFC-v0.0.4.41)</versions>
        private void Form_Period_Years_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            try
            {
                //if (Check_If_Is_Valid_Period(true))
                //    this.IsPeriodSelected = true;
                //else
                //    this.IsPeriodSelected = false;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                this.IsPeriodSelected = false;
            }
        }

        /// <versions>05-12-2017(GesAMFC-v0.0.4.41)</versions>
        private void ComboBoxEdit_Start_Year_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ComboboxItem objItemYear = (ComboBoxEdit_Start_Year.SelectedItem as ComboboxItem);
                this.PeriodSelected.Start.Year = new AMFCYear(Convert.ToInt32(objItemYear.GetValue()), objItemYear.GetText());
                if (!Program.IsValidYear(this.PeriodSelected.Start.Year.Value))
                    this.PeriodSelected.Start.Year = new AMFCYear(DateTime.Today.Year);
                Check_If_Is_Valid_Period(true);
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>05-12-2017(GesAMFC-v0.0.4.41)</versions>
        private void ComboBoxEdit_End_Year_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ComboboxItem objItemYear = (ComboBoxEdit_End_Year.SelectedItem as ComboboxItem);
                this.PeriodSelected.End.Year = new AMFCYear(Convert.ToInt32(objItemYear.GetValue()), objItemYear.GetText());
                if (!Program.IsValidYear(this.PeriodSelected.End.Year.Value))
                    this.PeriodSelected.End.Year = new AMFCYear(DateTime.Today.Year);
                Check_If_Is_Valid_Period(true);
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>05-12-2017(GesAMFC-v0.0.4.41)</versions>
        private void Button_Cancel_Click(object sender, EventArgs e)
        {
            this.IsPeriodSelected = false;
            this.Close();
        }

        /// <versions>05-12-2017(GesAMFC-v0.0.4.41)</versions>

        private void Button_OK_Click(object sender, EventArgs e)
        {
            try
            {
                if (Check_If_Is_Valid_Period(false))
                    this.IsPeriodSelected = true;
                else
                    this.IsPeriodSelected = false;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                this.IsPeriodSelected = false;
            }
            this.Close();
        }

        private Boolean Check_If_Is_Valid_Period(Boolean bShowMessage)
        {
            try
            {
                if (
                        (this.PeriodSelected.Start == null || !Program.IsValidYear(this.PeriodSelected.Start.Year.Value))
                        ||
                        (this.PeriodSelected.End == null || !Program.IsValidYear(this.PeriodSelected.End.Year.Value))
                    )
                    return false;
                DateTime dtDateStart    = new DateTime(this.PeriodSelected.Start.Year.Value,    1, 1);
                DateTime dttDateEnd     = new DateTime(this.PeriodSelected.End.Year.Value,      1, 1);
                if (dtDateStart > dttDateEnd)
                {
                    if (bShowMessage)
                    {
                        String sError = "O Ano/Mês do Início não pode ser superior ao Ano/Mês do Final do Período selecionado!";
                        MessageBox.Show(sError, "Erro Período", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                    else
                    {
                        this.PeriodSelected.Start = this.PeriodSelected.End;
                    }
                    
                }
                return true;

            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return false;
            }
        }
        #endregion  Events

        #region     Private Methods

        /// <versions>05-12-2017(GesAMFC-v0.0.4.41)</versions>
        private void SetStartYearComboList()
        {
            try
            {
                ComboBoxEdit_Start_Year.Properties.Items.Clear();
                Int32 iSelecteIndex = 0;
                Int32 iIndex = 0;
                foreach (AMFCYear objYear in ListYears.List)
                {
                    ComboboxItem objComboBoxItem = new ComboboxItem(objYear.Value, objYear.Description);
                    if (objYear.Value == this.PeriodSelected.Start.Year.Value)
                        iSelecteIndex = iIndex;
                    ComboBoxEdit_Start_Year.Properties.Items.Add(objComboBoxItem);
                    iIndex++;
                }
                ComboBoxEdit_Start_Year.SelectedIndex = iSelecteIndex;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }


        /// <versions>05-12-2017(GesAMFC-v0.0.4.41)</versions>
        private void SetEndYearComboList()
        {
            try
            {
                ComboBoxEdit_End_Year.Properties.Items.Clear();
                Int32 iSelecteIndex = 0;
                Int32 iIndex = 0;
                foreach (AMFCYear objYear in ListYears.List)
                {
                    ComboboxItem objComboBoxItem = new ComboboxItem(objYear.Value, objYear.Description);
                    if (objYear.Value == this.PeriodSelected.End.Year.Value)
                        iSelecteIndex = iIndex;
                    ComboBoxEdit_End_Year.Properties.Items.Add(objComboBoxItem);
                    iIndex++;
                }
                ComboBoxEdit_End_Year.SelectedIndex = iSelecteIndex;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        #endregion  Private Methods
    }
}