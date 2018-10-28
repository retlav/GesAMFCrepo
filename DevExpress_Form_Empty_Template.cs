using DevExpress.XtraEditors.Controls;
using GesAMFC.AMFC_Methods;
using System;

namespace GesAMFC
{
    /// <summary>Set Value To Pay Form</summary>
    /// <creation>08-12-2017(GesAMFC-v0.0.5.2)</creation>
    /// <versions>08-12-2017(GesAMFC-v0.0.5.2)</versions>
    public partial class DevExpress_Form_Empty_Template : DevExpress.XtraEditors.XtraForm
    {
        public Library_AMFC_Methods LibAMFC;

        #region     Constructor

        /// <versions>01-12-2017(GesAMFC-v0.0.4.41)</versions>
        public DevExpress_Form_Empty_Template(Double dbValue)
        {
            this.LibAMFC = new Library_AMFC_Methods();
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        #endregion  Constructor

        #region     Events

        //// <versions>08-12-2017(GesAMFC-v0.0.5.2)</versions>
        private void DevExpress_Form_Empty_Template_Load(object sender, EventArgs e)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        #endregion  Events

        #region     Methods

        #endregion  Methods
    }
}