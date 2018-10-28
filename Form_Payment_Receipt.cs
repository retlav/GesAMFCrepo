using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using DevExpress.XtraEditors;
using DevExpress.XtraRichEdit.API.Native;
using GesAMFC.AMFC_Methods;

namespace GesAMFC
{
    public partial class Form_Payment_Receipt : DevExpress.XtraEditors.XtraForm
    {
        public Library_AMFC_Methods LibAMFC;

        public Form_Payment_Receipt()
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

        public Int64 Receipt_Desginação_Max_Chars = 200;

        #region     Receipt Properties
        public Int64    Receipt_Number              { get; set; }
        public Double   Receipt_Value               { get; set; }
        public String   Receipt_Member_Name         { get; set; }
        public Int64    Receipt_Member_Number       { get; set; }
        public String   Receipt_Member_Lote         { get; set; }
        public String   Receipt_Member_Quantia      { get; set; }
        public String   Receipt_Member_Designacao   { get; set; }
        public String   Receipt_Member_Data         { get; set; }
        #endregion  Receipt Properties

        private void Form_Payment_Receipt_Load(object sender, EventArgs e)
        {
            
            try
            {
                this.WindowState = FormWindowState.Maximized;
                this.Update();
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// use DevExpress.XtraRichEdit.API.Native
        /// </remarks>
        public void Set_Member_Pay_Receipt()
        {
            try
            {
                String sFilePath = "Templates" + "/" + "AMFC-Recibo-Template" + "." + "doc";
                RichEditControl_Receipt.LoadDocument(sFilePath, DevExpress.XtraRichEdit.DocumentFormat.Doc);

                Document document = RichEditControl_Receipt.Document;

                #region     Document Page Properties
                document.Unit = DevExpress.Office.DocumentUnit.Centimeter;
                document.Sections[0].Page.PaperKind = System.Drawing.Printing.PaperKind.A4;
                document.Sections[0].Page.Landscape = false;
                float fMargin = 1.0f;
                document.Sections[0].Margins.Left   = fMargin;
                document.Sections[0].Margins.Top    = fMargin;
                document.Sections[0].Margins.Right  = fMargin;
                document.Sections[0].Margins.Bottom     = fMargin;
                #endregion  Document Page Properties

                //Dim cell As TableCell = table.Cell(0, 0)
                //Dim pp As ParagraphProperties = richEditControl1.Document.BeginUpdateParagraphs(cell.ContentRange)
                //pp.LineSpacingType = ParagraphLineSpacing.Single
                //richEditControl1.Document.EndUpdateParagraphs(pp)
                
                Table tbl01 = document.Tables[0];
                Set_Pay_Receipt_Details(document, tbl01);

                Table tbl02= document.Tables[1];
                Set_Pay_Receipt_Details(document, tbl02);
            }
            catch (Exception ex)
            {
                String serr = ex.Message;
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        public void Set_Pay_Receipt_Details(Document document, Table tbl)
        {
            try
            {
                TableCell objCell_Pay_Receipt_Number = tbl.Rows[0].LastCell;
                if (this.Receipt_Number > 0)
                    document.InsertSingleLineText(objCell_Pay_Receipt_Number.Range.Start, " " + this.Receipt_Number.ToString());

                TableCell objCell_Pay_Receipt_Value = tbl.Rows[1].LastCell;
                if (this.Receipt_Value > 0)
                    document.InsertSingleLineText(objCell_Pay_Receipt_Value.Range.Start, " " + String.Format("{0:C}", this.Receipt_Value));

                TableCell objCell_Pay_Receipt_Member_Name = tbl.Rows[4].LastCell;
                if (!String.IsNullOrEmpty(this.Receipt_Member_Name))
                    document.InsertSingleLineText(objCell_Pay_Receipt_Member_Name.Range.Start, this.Receipt_Member_Name.ToString());

                TableCell objCell_Pay_Receipt_Member_Number = tbl.Rows[5].Cells[2];
                if (this.Receipt_Member_Number > 0)
                    document.InsertSingleLineText(objCell_Pay_Receipt_Member_Number.Range.Start, " " + this.Receipt_Member_Number.ToString());

                TableCell objCell_Pay_Receipt_Member_Lote = tbl.Rows[6].Cells[2];
                if (!String.IsNullOrEmpty(this.Receipt_Member_Lote))
                    document.InsertSingleLineText(objCell_Pay_Receipt_Member_Lote.Range.Start, " " + this.Receipt_Member_Lote.ToString());

                TableCell objCell_Pay_Receipt_Member_Quantia = tbl.Rows[7].Cells[2];
                if (!String.IsNullOrEmpty(this.Receipt_Member_Quantia))
                    document.InsertSingleLineText(objCell_Pay_Receipt_Member_Quantia.Range.Start, " " + this.Receipt_Member_Quantia.ToString());

                TableCell objCell_Pay_Receipt_Member_Designacao = tbl.Rows[8].Cells[2];
                if (!String.IsNullOrEmpty(this.Receipt_Member_Designacao))
                    document.InsertSingleLineText(objCell_Pay_Receipt_Member_Designacao.Range.Start, " " + this.Receipt_Member_Designacao.ToString());

                TableCell objCell_Pay_Receipt_Member_Data = tbl.Rows[10].Cells[3];
                if (!String.IsNullOrEmpty(this.Receipt_Member_Data))
                    document.InsertSingleLineText(objCell_Pay_Receipt_Member_Data.Range.Start, " " + this.Receipt_Member_Data.ToString());
            }
            catch (Exception ex)
            {
                String serr = ex.Message;
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        #region a ver

        ////https://www.devexpress.com/Support/Center/Example/Details/E3242
        //private void richEditControl1_DocumentLoaded(object sender, EventArgs e)
        //{
        //    CreateStyles();
        //}
        //private void btnCreateTable_Click(object sender, EventArgs e)
        //{
        //    CreateTable();
        //    FillTable();
        //    ApplyHeadingStyle();
        //}

        //private void CreateTable()
        //{

        //    Document doc = richEditControl1.Document;
        //    // Clear out the document content
        //    doc.Delete(richEditControl1.Document.Range);
        //    // Set up header information
        //    DocumentPosition pos = doc.Range.Start;
        //    DocumentRange rng = doc.InsertSingleLineText(pos, "Directory Information from C:\\");

        //    CharacterProperties cp_Header = doc.BeginUpdateCharacters(rng);
        //    cp_Header.FontName = "Verdana";
        //    cp_Header.FontSize = 16;
        //    doc.EndUpdateCharacters(cp_Header);
        //    doc.Paragraphs.Insert(rng.End);
        //    doc.Paragraphs.Insert(rng.End);

        //    // Add the table
        //    doc.Tables.Create(rng.End, 1, 3, AutoFitBehaviorType.AutoFitToWindow);
        //    // Format the table
        //    Table tbl = doc.Tables[0];

        //    try
        //    {
        //        tbl.BeginUpdate();

        //        CharacterProperties cp_Tbl = doc.BeginUpdateCharacters(tbl.Range);
        //        cp_Tbl.FontSize = 8;
        //        cp_Tbl.FontName = "Verdana";
        //        doc.EndUpdateCharacters(cp_Tbl);

        //        // Insert header caption and format the columns
        //        doc.InsertSingleLineText(tbl[0, 0].Range.Start, "Name");
        //        doc.InsertSingleLineText(tbl[0, 1].Range.Start, "Size");
        //        ParagraphProperties pp_HeadingSize = doc.BeginUpdateParagraphs(tbl[0, 1].Range);
        //        pp_HeadingSize.Alignment = ParagraphAlignment.Right;
        //        doc.EndUpdateParagraphs(pp_HeadingSize);

        //        doc.InsertSingleLineText(tbl[0, 2].Range.Start, "Modified");
        //        ParagraphProperties pp_HeadingModified = doc.BeginUpdateParagraphs(tbl[0, 2].Range);
        //        pp_HeadingModified.Alignment = ParagraphAlignment.Right;
        //        doc.EndUpdateParagraphs(pp_HeadingModified);
        //        // Apply a style to the table
        //        tbl.Style = doc.TableStyles["MyTableGridNumberEight"];
        //        // Specify right and left paddings equal to 0.08 inches for all cells in a table
        //        tbl.RightPadding = Units.InchesToDocumentsF(0.08f);
        //        tbl.LeftPadding = Units.InchesToDocumentsF(0.08f);
        //    }
        //    finally
        //    {
        //        tbl.EndUpdate();
        //    }
        //}

        //private void CreateStyles()
        //{
        //    // Define basic style
        //    TableStyle tStyleNormal = richEditControl1.Document.TableStyles.CreateNew();
        //    tStyleNormal.LineSpacingType = ParagraphLineSpacing.Single;
        //    tStyleNormal.FontName = "Verdana";
        //    tStyleNormal.Alignment = ParagraphAlignment.Left;
        //    tStyleNormal.Name = "MyTableGridNormal";
        //    richEditControl1.Document.TableStyles.Add(tStyleNormal);

        //    // Define Grid Eight style
        //    TableStyle tStyleGrid8 = richEditControl1.Document.TableStyles.CreateNew();
        //    tStyleGrid8.Parent = tStyleNormal;
        //    TableBorders borders = tStyleGrid8.TableBorders;

        //    borders.Bottom.LineColor = Color.DarkBlue;
        //    borders.Bottom.LineStyle = TableBorderLineStyle.Single;
        //    borders.Bottom.LineThickness = 0.75f;

        //    borders.Left.LineColor = Color.DarkBlue;
        //    borders.Left.LineStyle = TableBorderLineStyle.Single;
        //    borders.Left.LineThickness = 0.75f;

        //    borders.Right.LineColor = Color.DarkBlue;
        //    borders.Right.LineStyle = TableBorderLineStyle.Single;
        //    borders.Right.LineThickness = 0.75f;

        //    borders.Top.LineColor = Color.DarkBlue;
        //    borders.Top.LineStyle = TableBorderLineStyle.Single;
        //    borders.Top.LineThickness = 0.75f;

        //    borders.InsideVerticalBorder.LineColor = Color.DarkBlue;
        //    borders.InsideVerticalBorder.LineStyle = TableBorderLineStyle.Single;
        //    borders.InsideVerticalBorder.LineThickness = 0.75f;

        //    borders.InsideHorizontalBorder.LineColor = Color.DarkBlue;
        //    borders.InsideHorizontalBorder.LineStyle = TableBorderLineStyle.Single;
        //    borders.InsideHorizontalBorder.LineThickness = 0.75f;

        //    tStyleGrid8.CellBackgroundColor = Color.Transparent;
        //    tStyleGrid8.Name = "MyTableGridNumberEight";
        //    richEditControl1.Document.TableStyles.Add(tStyleGrid8);

        //    // Define Headings paragraph style
        //    ParagraphStyle pStyleHeadings = richEditControl1.Document.ParagraphStyles.CreateNew();
        //    pStyleHeadings.Bold = true;
        //    pStyleHeadings.ForeColor = Color.White;
        //    pStyleHeadings.Name = "My Headings Style";
        //    richEditControl1.Document.ParagraphStyles.Add(pStyleHeadings);
        //}

        //private void FillTable()
        //{
        //    // Fill the table with data
        //    Document doc = richEditControl1.Document;
        //    Table tbl = doc.Tables[0];
        //    DirectoryInfo di = new DirectoryInfo("C:\\");

        //    try
        //    {
        //        tbl.BeginUpdate();
        //        foreach (FileInfo fi in di.GetFiles())
        //        {
        //            TableRow row = tbl.Rows.Append();
        //            TableCell cell = row.FirstCell;
        //            doc.InsertSingleLineText(cell.Range.Start, fi.Name);
        //            doc.InsertSingleLineText(cell.Next.Range.Start,
        //                String.Format("{0:N0}", fi.Length));
        //            doc.InsertSingleLineText(cell.Next.Next.Range.Start,
        //                String.Format("{0:g}", fi.LastWriteTime));
        //        }
        //    }
        //    finally
        //    {
        //        tbl.EndUpdate();
        //    }
        //}

        //private void ApplyHeadingStyle()
        //{
        //    Document doc = richEditControl1.Document;
        //    Table tbl = doc.Tables[0];
        //    foreach (TableCell cell in tbl.Rows.First.Cells)
        //    {
        //        cell.BackgroundColor = Color.DarkBlue;
        //    }
        //    ParagraphProperties pp_Headings = doc.BeginUpdateParagraphs(tbl.Rows.First.Range);
        //    pp_Headings.Style = doc.ParagraphStyles["My Headings Style"];
        //    doc.EndUpdateParagraphs(pp_Headings);
        //}

        #endregion
    }
}