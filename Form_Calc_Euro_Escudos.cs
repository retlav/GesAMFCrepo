using DevExpress.XtraEditors.Controls;
using GesAMFC.AMFC_Methods;
using System;

namespace GesAMFC
{
    /// <summary>Form Select Lote</summary>
    /// <creation>15-03-2018(GesAMFC-v1.0.0.3</creation>
    /// <versions>23-03-2018(GesAMFC-v1.0.1.1)</versions>
    public partial class Form_Calc_Euro_Escudos : DevExpress.XtraEditors.XtraForm
    {
        public Library_AMFC_Methods LibAMFC;

        private Double OneEuroToEscudos = 200.482;

        #region Constructor

        /// <versions>23-03-2018(GesAMFC-v1.0.1.1)</versions>
        public Form_Calc_Euro_Escudos()
        {
            this.LibAMFC = new Library_AMFC_Methods();
            try
            {
                InitializeComponent();

                this.OneEuroToEscudos = Convert.ToDouble(LibAMFC.DBF_AMFC_Euro_To_Escudos.Trim());
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        #endregion Constructor

        #region     Events
        
        /// <versions>23-03-2018(GesAMFC-v1.0.1.1)</versions>
        private void Form_Calc_Euro_Escudos_Load(object sender, EventArgs e)
        {
            try
            {
                this.TextEdit_Valor_Escudos.Text = String.Format(Program.FormatString_Double3_DecimalPlaces, this.OneEuroToEscudos);
                this.TextEdit_Valor_Euros.Text = String.Format(Program.FormatString_Double3_DecimalPlaces, Convert.ToDouble("1"));
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>23-03-2018(GesAMFC-v1.0.1.1)</versions>
        private void Form_Calc_Euro_Escudos_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>23-03-2018(GesAMFC-v1.0.1.1)</versions>
        private void Button_Escudos_Para_Euros_Click(object sender, EventArgs e)
        {
            try
            {
                Double dbEscudos = Program.SetAreaDoubleValue(this.TextEdit_Valor_Escudos.Text);
                Double dbEuros = Convert.ToDouble(Decimal.Divide(Convert.ToDecimal(dbEscudos), Convert.ToDecimal(this.OneEuroToEscudos)));
                this.TextEdit_Valor_Euros.Text = String.Format(Program.FormatString_Double3_DecimalPlaces, dbEuros); 
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>23-03-2018(GesAMFC-v1.0.1.1)</versions>
        private void Button_Euros_Para_Escudos_Click(object sender, EventArgs e)
        {
            try
            {
                Double dbEuros = Program.SetAreaDoubleValue(this.TextEdit_Valor_Euros.Text);
                Double dbEscudos = Convert.ToDouble(Decimal.Multiply(Convert.ToDecimal(dbEuros), Convert.ToDecimal(this.OneEuroToEscudos)));
                this.TextEdit_Valor_Escudos.Text = String.Format(Program.FormatString_Double3_DecimalPlaces, dbEscudos);
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        #endregion  Events

        #region     Private Methods


        #endregion  Private Methods
    }
}