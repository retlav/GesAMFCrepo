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
    public partial class Form_Envelope : DevExpress.XtraEditors.XtraForm
    {
        public Library_AMFC_Methods LibAMFC;
        private AMFCMember Member;
        private Boolean USE_REMETENTE;

        /// <versions>28-04-2017(v0.0.2.46)</versions>
        public Form_Envelope()
        {
            LibAMFC = new Library_AMFC_Methods();
            try
            {
                InitializeComponent();

                CheckButton_Remetente.Checked = true;
                USE_REMETENTE = CheckButton_Remetente.Checked;

                Config_Member_Letter_Address_Editor(this.RichEditControl_Member_Letter_Address);
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>19-02-2018(GesAMFC-v1.0.0.3</versions>
        private void Form_Envelope_Load(object sender, EventArgs e)
        {
            this.Member = new AMFCMember();

            //#if DEBUG

            //this.Member = Get_DBF_AMFC_Member_By_Number(105); //Debug
            //#endif

            Set_Member_Letter_Address();
            Find_Member();

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
                        if (objMemberSelected != null && objMemberSelected.NUMERO >= objMemberSelected.MinNumber && objMemberSelected.NUMERO < objMemberSelected.MaxNumber && !String.IsNullOrEmpty(objMemberSelected.NOME) && !String.IsNullOrEmpty(objMemberSelected.MORADA) && !String.IsNullOrEmpty(objMemberSelected.CPOSTAL) && !String.IsNullOrEmpty(objMemberSelected.NUMLOTE))
                        {
                            this.Member = objMemberSelected;

                            //Fill_Member_Letter_Address_Editor();
                            Set_Member_Letter_Address();
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

        /// <versions>28-04-2017(v0.0.2.46)</versions>
        private void Button_Member_Envelope_Print_Preview_Click(object sender, EventArgs e)
        {
            Button_Envelope_Print_Preview();
        }
        
        /// <versions>28-04-2017(v0.0.2.46)</versions>
        private void Button_Member_Envelope_Print_Click(object sender, EventArgs e)
        {
            Button_Envelope_Print();
        }

        /// <versions>28-04-2017(v0.0.2.47)</versions>
        private void Button_Member_Envelope_Save_Click(object sender, EventArgs e)
        {
            Button_Envelope_Save();
        }

        /// <versions>28-04-2017(v0.0.2.47)</versions>
        private void FileSaveItem_Envelope_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
        {
            Button_Envelope_Save();
        }

        #region     Member Envelope Editor

        /// <versions>28-04-2017(v0.0.2.46)</versions>
        private void Config_Member_Letter_Address_Editor(RichEditControl objRichEditControl)
        {
            try
            {
                objRichEditControl.Text = String.Empty;

                DevExpress.XtraRichEdit.API.Native.Document document = objRichEditControl.Document;
                document.BeginUpdate();
                document.Text = String.Empty;
                document.Unit = DevExpress.Office.DocumentUnit.Centimeter;
                document.Sections[0].Page.PaperKind = System.Drawing.Printing.PaperKind.DLEnvelope;
                document.Sections[0].Page.Landscape = true;
                float fMargin = 0.5f;
                document.Sections[0].Margins.Left = fMargin;
                document.Sections[0].Margins.Top = fMargin;
                document.Sections[0].Margins.Right = fMargin;
                document.Sections[0].Margins.Bottom = fMargin;
                document.EndUpdate();
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>28-04-2017(v0.0.2.47)</versions>
        private void Button_Envelope_Save()
        {
            try
            {

                String sFileName = "Envelope" + "_" + "Socio" + "_" + this.Member.NUMERO + "_" + DateTime.Now.ToString("yyyyMMddHHmmss");

                String _FileFormat_WordDoc = "doc";

                #region     Save Copy Into Local App Folder
                String sLocalExportMememberDirPath = "Export" + "/" + "Word" + "/" + "SOCIOS";
                String sLocalExportMememberFilePath = sLocalExportMememberDirPath + "/" + sFileName + "." + _FileFormat_WordDoc;
                this.RichEditControl_Member_Letter_Address.SaveDocument(sLocalExportMememberFilePath, DevExpress.XtraRichEdit.DocumentFormat.OpenXml);
                #endregion  Save Copy Into Local App Folder

                #region     Save to Excel
                SaveFileDialog objSaveFileDialog = new SaveFileDialog();
                objSaveFileDialog.Title = "Envelope" + " de " + "Sócio" + " - " + "Export" + " " + "Word";
                objSaveFileDialog.Filter = "Word Documents|*." + _FileFormat_WordDoc;
                objSaveFileDialog.FileName = sFileName;
                DialogResult dialogResult = objSaveFileDialog.ShowDialog(this);
                if (dialogResult == DialogResult.OK)
                {
                    this.RichEditControl_Member_Letter_Address.SaveDocument(objSaveFileDialog.FileName, DevExpress.XtraRichEdit.DocumentFormat.OpenXml);
                    DevExpress.XtraEditors.XtraMessageBox.Show("Envelope" + " do " + "Sócio" + " " + "Nº" + ": " + this.Member.NUMERO + " guardado com sucesso!", "Exportação" + " " + "Envelope" + " " + "Sócio" + " " + "Word", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                #endregion  Save to Excel

                this.RichEditControl_Member_Letter_Address.SaveDocument("teste", DevExpress.XtraRichEdit.DocumentFormat.OpenXml);

            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>28-04-2017(v0.0.2.46)</versions>
        private void Button_Envelope_Print_Preview()
        {
            try
            {
                this.RichEditControl_Member_Letter_Address.ShowPrintPreview();
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>28-04-2017(v0.0.2.46)</versions>
        private void Button_Envelope_Print()
        {
            try
            {
                this.RichEditControl_Member_Letter_Address.ShowPrintDialog();
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>19-02-2018(GesAMFC-v1.0.0.3)</versions>
        //private void Fill_Member_Letter_Address_Editor()
        //{
        //    try
        //    {
        //        DevExpress.XtraRichEdit.API.Native.Document document = this.RichEditControl_Member_Letter_Address.Document;
        //        document.BeginUpdate();

        //        document.Text = String.Empty;

        //        Int32 iCntNbrEmptyRows = 14;
        //        String sMemberLetterAddress = String.Empty;
        //        for (Int32 iCnt = 0; iCnt < iCntNbrEmptyRows; iCnt++)
        //            sMemberLetterAddress += Environment.NewLine;
        //        if (this.Member.NUMERO >= this.Member.MinNumber && this.Member.NUMERO < this.Member.MaxNumber && !String.IsNullOrEmpty(this.Member.NOME) && !String.IsNullOrEmpty(this.Member.MORADA) && !String.IsNullOrEmpty(this.Member.CPOSTAL))
        //        {
        //            sMemberLetterAddress += this.Member.NOME + " " + this.Member.NUMERO + "\r\n";
        //            sMemberLetterAddress += this.Member.MORADA + Environment.NewLine;
        //            sMemberLetterAddress += this.Member.CPOSTAL;
        //        }
        //        this.RichEditControl_Member_Letter_Address.Document.AppendText(sMemberLetterAddress);

        //        Int32 iParagraphsCnt = document.Paragraphs.Count;
        //        for (Int32 iCnt = 0; iCnt < iParagraphsCnt; iCnt++)
        //        {
        //            ParagraphProperties pp = document.BeginUpdateParagraphs(document.Paragraphs[iCnt].Range);

        //            pp.Alignment = ParagraphAlignment.Left;
        //            pp.LeftIndent = 9;
        //            pp.RightIndent = 0.5f;

        //            pp.Style.FontName = "Arial";
        //            pp.Style.FontSize = 10;
        //            document.EndUpdateParagraphs(pp);
        //        }
        //        document.EndUpdate();
        //    }
        //    catch (Exception ex)
        //    {
        //        Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
        //    }
        //}

        /// <versions>19-02-2018(GesAMFC-v1.0.0.3)</versions>
        public void Set_Member_Letter_Address()
        {
            try
            {
                String sFilePath = "Templates" + "/" + "AMFC-Envelope-Template" + "." + "doc";
                RichEditControl_Member_Letter_Address.LoadDocument(sFilePath, DevExpress.XtraRichEdit.DocumentFormat.Doc);

                Document document = RichEditControl_Member_Letter_Address.Document;
               
                Table tbl01 = document.Tables[0];
                //tbl01.Style.LeftIndent = 3.5f;
                tbl01.Style.TopPadding = 0.1f;
                tbl01.Style.FontName = "Verdana";
                tbl01.Style.FontSize = 9;
                if (!USE_REMETENTE)
                {
                    //document.Tables.Remove(tbl01);
                    for (int i = 0; i < tbl01.Rows.Count; i++)
                    {
                        RichEditControl_Member_Letter_Address.Document.Delete(tbl01.Rows[i].Range);
                        document.InsertSingleLineText(tbl01.Rows[i][0].Range.Start, " ");
                        tbl01.Rows[i].HeightType = HeightType.Exact;
                        tbl01.Rows[i].Height = 2.7f;
                    }
                }


                if (this.Member != null && this.Member.NUMERO >= this.Member.MinNumber && this.Member.NUMERO < this.Member.MaxNumber && !String.IsNullOrEmpty(this.Member.NOME) && !String.IsNullOrEmpty(this.Member.MORADA) && !String.IsNullOrEmpty(this.Member.CPOSTAL))
                {
                    Table tbl02 = document.Tables[document.Tables.Count-1];
                    //tbl02.Style.LeftIndent = 0.5f;
                    tbl02.Style.FontName = "Verdana";
                    tbl02.Style.FontSize = 10;

                    document.InsertSingleLineText(tbl02.Rows[0][1].Range.Start, this.Member.NOME.Trim() + " " + this.Member.NUMERO);
                    document.InsertSingleLineText(tbl02.Rows[1][1].Range.Start, this.Member.MORADA.Trim());
                    document.InsertSingleLineText(tbl02.Rows[2][1].Range.Start, this.Member.CPOSTAL);
                }
            }
            catch (Exception ex)
            {
                String serr = ex.Message;
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>28-04-2017(v0.0.2.48)</versions>
        private AMFCMember Get_DBF_AMFC_Member_By_Number(Int64 lMemberNumber)
        {
            try
            {
                using (Library_AMFC_SQL obj_AMFC_SQL = new Library_AMFC_SQL())
                { 
                    return obj_AMFC_SQL.Get_Member_By_Number(lMemberNumber);
                }
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return null;
            }
        }

        #endregion  Member Envelope Editor

        /// <versions>19-02-2018(GesAMFC-v1.0.0.3)</versions>
        private void Button_Member_Find_Click(object sender, EventArgs e)
        {
            Find_Member();
        }

        /// <versions>19-02-2018(GesAMFC-v1.0.0.3)</versions>
        private void CheckButton_Remetente_CheckedChanged(object sender, EventArgs e)
        {
            try
            {
                USE_REMETENTE = CheckButton_Remetente.Checked;
                Set_Member_Letter_Address();
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        private void RibbonControl_Envelope_Click(object sender, EventArgs e)
        {

        }

        private void Label_Envelope_Header_Click(object sender, EventArgs e)
        {

        }
    }
}