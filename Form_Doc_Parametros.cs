using DevExpress.XtraRichEdit;
using DevExpress.XtraRichEdit.API.Native;
using GesAMFC.AMFC_Methods;
using System;
using System.Windows.Forms;

namespace GesAMFC
{
    /// <summary>Form Envelope</summary>
    /// <creation>28-04-2017(v0.0.2.46)</creation>
    /// <versions>19-02-2018(GesAMFC-v1.0.0.3</versions>
    public partial class Form_Doc_Parametros : DevExpress.XtraEditors.XtraForm
    {
        public Library_AMFC_Methods LibAMFC;

        /// <versions>28-04-2017(v0.0.2.46)</versions>
        public Form_Doc_Parametros()
        {
            LibAMFC = new Library_AMFC_Methods();
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>19-02-2018(GesAMFC-v1.0.0.3</versions>
        private void Form_Doc_Parametros_Load(object sender, EventArgs e)
        {
            Load_Document(this.RichEditControl_Parametros);
        }

        /// <versions>20-02-2018(GesAMFC-v1.0.0.3)</versions>
        public void Load_Document(RichEditControl objRichEditControl)
        {
            try
            {
                String sFilePath = "Templates" + "/" + "Tabela_Parametros" + "." + "docx";
                //objRichEditControl.LoadDocument(sFilePath, DevExpress.XtraRichEdit.DocumentFormat.Doc);
                objRichEditControl.LoadDocument(sFilePath);
            }
            catch (Exception ex)
            {
                String serr = ex.Message;
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>20-02-2018(GesAMFC-v1.0.0.3)</versions>
        private void Config_Document(RichEditControl objRichEditControl)
        {
            try
            {
                objRichEditControl.Text = String.Empty;

                DevExpress.XtraRichEdit.API.Native.Document document = objRichEditControl.Document;
                document.BeginUpdate();
                document.Unit = DevExpress.Office.DocumentUnit.Centimeter;
                foreach (DevExpress.XtraRichEdit.API.Native.Section objSection in document.Sections)
                {
                    objSection.Page.PaperKind = System.Drawing.Printing.PaperKind.A4;
                    objSection.Page.Landscape = false;
                    objSection.Margins.Left = 2.5f;
                    objSection.Margins.Top = 2.0f;
                    objSection.Margins.Right = 1.5f;
                    objSection.Margins.Bottom = 2.0f;
                }
                document.EndUpdate();
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }
    }
}