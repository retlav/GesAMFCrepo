using DevExpress.XtraEditors.Controls;
using GesAMFC.AMFC_Methods;
using System;

namespace GesAMFC
{
    /// <summary>Form Select Lote</summary>
    /// <creation>15-03-2018(GesAMFC-v1.0.0.3</creation>
    /// <versions>15-03-2018(GesAMFC-v1.0.0.3)</versions>
    public partial class Form_Lote : DevExpress.XtraEditors.XtraForm
    {
        public Library_AMFC_Methods LibAMFC;

        public AMFCLotes ListLotes;
        public AMFCLote LoteSelected;
        public Boolean IsLoteSelected;

        #region Constructor

        /// <versions>01-12-2017(GesAMFC-v0.0.4.41)</versions>
        public Form_Lote(AMFCMemberLotes objListLotes)
        {
            this.LibAMFC = new Library_AMFC_Methods();
            try
            {
                InitializeComponent();

                this.ListLotes = new AMFCLotes();
                if (objListLotes != null && objListLotes.Lotes.Count > 0)
                {
                    foreach (AMFCMemberLote objLote in objListLotes.Lotes)
                    {
                        AMFCLote objCBLote = new AMFCLote(Convert.ToInt32(objLote.IDLOTE), objLote.NUMLOTE);
                        this.ListLotes.Add(objCBLote);
                    }
                }
                this.IsLoteSelected = false;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        #endregion Constructor

        #region     Events

        /// <versions>01-12-2017(GesAMFC-v0.0.4.41)</versions>
        private void Form_Lote_Load(object sender, EventArgs e)
        {
            try
            {
                SetLoteComboList();
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>01-12-2017(GesAMFC-v0.0.4.41)</versions>
        private void Form_Lote_FormClosing(object sender, System.Windows.Forms.FormClosingEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>01-12-2017(GesAMFC-v0.0.4.41)</versions>
        private void ComboBoxEdit_Lote_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                ComboboxItem objItemLote = (ComboBoxEdit_Lote.SelectedItem as ComboboxItem);
                this.LoteSelected = new AMFCLote(Convert.ToInt32(objItemLote.GetValue()), objItemLote.GetText());
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
                this.IsLoteSelected = false;
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
            if (Convert.ToInt32(this.LoteSelected.Value) > 0 && !String.IsNullOrEmpty(this.LoteSelected.Description))
                this.IsLoteSelected = true;
            else
                this.IsLoteSelected = false;
            this.Close();
        }

        #endregion  Events

        #region     Private Methods

        /// <versions>01-12-2017(GesAMFC-v0.0.4.41)</versions>
        private void SetLoteComboList()
        {
            try
            {
                Int32 iSelecteIndex = 0;
                Int32 iIndex = 0;
                ComboBoxEdit_Lote.Properties.Items.Clear();
                foreach (AMFCLote objLote in ListLotes.List)
                {
                    ComboboxItem objComboBoxItem = new ComboboxItem(objLote.Value, objLote.Description);
                    if (this.LoteSelected != null && this.LoteSelected.Value > 0 && objLote.Value == this.LoteSelected.Value)
                        iSelecteIndex = iIndex;
                    ComboBoxEdit_Lote.Properties.Items.Add(objComboBoxItem);
                    iIndex++;
                }
                ComboBoxEdit_Lote.SelectedIndex = iSelecteIndex;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        #endregion  Private Methods
    }
}