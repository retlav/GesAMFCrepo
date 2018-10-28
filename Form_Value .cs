using DevExpress.XtraEditors.Controls;
using GesAMFC.AMFC_Methods;
using System;

namespace GesAMFC
{
    /// <summary>Set Value To Pay Form</summary>
    /// <creation>08-12-2017(GesAMFC-v0.0.5.2)</creation>
    /// <versions>08-12-2017(GesAMFC-v0.0.5.2)</versions>
    public partial class Form_Value : DevExpress.XtraEditors.XtraForm
    {
        public Library_AMFC_Methods LibAMFC;

        public Double SelectedValue;

        #region Constructor

        /// <versions>01-12-2017(GesAMFC-v0.0.4.41)</versions>
        public Form_Value(Double dbValue)
        {
            this.LibAMFC = new Library_AMFC_Methods();
            try
            {
                InitializeComponent();
                this.SelectedValue = 0;
                if (Program.IsValidCurrencyEuroValue(dbValue) && dbValue > 0)
                    this.SelectedValue = dbValue;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        #endregion Constructor

        #region     Events

        //// <versions>08-12-2017(GesAMFC-v0.0.5.2)</versions>
        private void Form_Value_Load(object sender, EventArgs e)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>08-12-2017(GesAMFC-v0.0.5.2)</versions>
        private void Form_Value_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>08-12-2017(GesAMFC-v0.0.5.2)</versions>
        private void Button_Cancel_Click(object sender, EventArgs e)
        {
            try
            {
                this.SelectedValue = 0;
                this.Close();
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>08-12-2017(GesAMFC-v0.0.5.2)</versions>
        private void Button_OK_Click(object sender, EventArgs e)
        {
            try
            {
                if (Program.IsValidCurrencyEuroValue(this.TextEdit_PayValue.Text.Trim()))
                    this.SelectedValue = Program.SetPayCurrencyEuroDoubleValue(this.TextEdit_PayValue.Text.Trim());
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
            this.Close();
        }

        #endregion  Events

        #region     Private Methods



        #endregion  Private Methods
    }
}