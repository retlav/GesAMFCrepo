using DevExpress.XtraBars;
using DevExpress.XtraEditors;
using GesAMFC.AMFC_Methods;
using System;
using System.Windows.Forms;

namespace GesAMFC
{
    /// <summary>AMFC Application Main Form</summary>
    /// <summary>AMFC Admin Library</summary>
    /// <author>Valter Lima</author>
    /// <creation>18-02-2017(v0.0.1.0)</creation>
    /// <versions>09-12-2017(GesAMFC-v1.0.0.1)</versions>
    public partial class GesAMFC_MainForm : XtraForm
    {
        public Library_AMFC_Methods LibAMFC;

        #region     Child Forms
        private Admin_AMFC_Members _FormMembers = null;
        private Boolean _FormMembersClosed = false;
        #endregion  Child Forms


        /// <versions>14-10-2017(v0.0.4.13)</versions>
        private void Open_Start_Module()
        {

            try
            {
#if DEBUG
                //OpenModule("Members");

                //OpenModule("Joias");
                //OpenModule("Quotas");

                //OpenModule("Envelopes");

                //OpenModule("Recibos_Pagamento");
                //OpenModule("Recibos_Quotas");

                //OpenModule("Lotes");

                //OpenModule("Cedências");
                //OpenModule("Infraestruturas");

                //OpenModule("Caixa");
                //OpenModule("CC_CEDEN");
                //OpenModule("CC_RECON");

                OpenModule("CC_ESGOT");
#else

#endif

            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return;
            }
        }

        /// <versions>14-03-2017(v0.0.2.6)</versions>
        public GesAMFC_MainForm()
        {
            LibAMFC = new Library_AMFC_Methods();
            try
            {
                this.LookAndFeel.SkinName = "Office 2010 Blue";
                this.LookAndFeel.UseDefaultLookAndFeel = false;
                InitializeComponent();
                //Bitmap bmp = new Bitmap(imageMenuList.Images[0]);
                //IntPtr Hicon = bmp.GetHicon();
                //Icon logoIcon = Icon.FromHandle(Hicon);
                //this.Icon = logoIcon;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return;
            }
        }

        #region     Events

        #region     Form Events

        /// <versions>05-05-2017(v0.0.3.1)</versions>
        private void AdexEntitiesMain_Load(object sender, EventArgs e)
        {
            try
            {
                AMFC_Main_Login_SplitContainer.Visible = true;

                Program.AppUser = new ApplicationUser();

                #region Check Login
                if (!Program.UserLogged)
                {
                    this.BarMainMenu.Visible = false;
                    Login objFormLogin = new Login();
                    objFormLogin.ShowDialog();
                }
                #endregion Check Login

                if (!Program.UserLogged)
                {
                    //XtraMessageBox.Show("A aplicação vai encerrar ...", "Fechar Programa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    //HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true);
                    return;
                }

                #region Menu Bar
                this.BarMainMenu.Visible = true;
                this.SetMenuOptions();
                #endregion Menu Bar

                #region Bottom Status Bar
                //this.BarStatus.Visible = true;
                //String sApplitcationStartYear = "2010";
                //String sApplitcationCurrentYear = DateTime.Today.Year.ToString();
                //String sCopyrightDates = sApplitcationStartYear + " - " + sApplitcationCurrentYear;
                ////this.BarStaticItemCopyRight.Caption = "© " + sCopyrightDates + " MediaMonitor. All rights reserved. [" + GetApplicationVersion() + "]";
                //String sDateToday = DateTime.Now.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
                //String sUserInfo = String.Empty;
                //if (!String.IsNullOrEmpty(Program.ApplicationUser.Name) && !String.IsNullOrEmpty(Program.ApplicationUser.Username) && Program.ApplicationUser.Name != Program.ApplicationUser.Username)
                //    sUserInfo = Program.ApplicationUser.Name + " (" + Program.ApplicationUser.Username + ")";
                //String sUserLogOnDateInfo = String.Empty;
                //if (!String.IsNullOrEmpty(sUserInfo))
                //    sUserLogOnDateInfo = "Logged on: " + sUserInfo + "  ";
                //if (!String.IsNullOrEmpty(sUserInfo))
                //    sUserLogOnDateInfo += "[" + sDateToday + "]";
                //this.BarStaticItemDateTime.Caption = sUserLogOnDateInfo;
                #endregion Bottom Status Bar

                AMFC_Main_Login_SplitContainer.Visible = false;

                Open_Start_Module();
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return;
            }
        }

        /// <versions>23-04-2017(v0.0.1.34)</versions>
        private void GesAMFC_MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                //if (!Program.Dialog_Exit_Application())
                //    e.Cancel = true;

                if (_FormMembers != null)
                {
                    _FormMembers.Close();
                    _FormMembers.Dispose();
                    _FormMembers = null;
                }
                
            }
            catch
            {
                Program.Exit_Application();
            }
        }

        /// <versions>23-04-2017(v0.0.1.34)</versions>
        private void GesAMFC_MainForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            //if (!Program.UserLogged)
            //    Program.Exit_Application();
        }

        /// <versions>14-03-2017(v0.0.2.6)</versions>
        /// <summary>Evita crashs no redimensionamento das janelas!!!!</summary>
        /// <remarks>http://stackoverflow.com/questions/10500993/win32exception-error-creating-window-handle-big-number-of-nested-controls</remarks>
        protected override void OnSizeChanged(EventArgs e)
        {
            try
            {
                if (this.IsHandleCreated)
                {
                        this.BeginInvoke((MethodInvoker)
                        delegate ()
                        {
                            //(MethodInvoker)(() => base.OnSizeChanged(e));
                            base.OnSizeChanged(e);
                        }
                    );
                }
                //base.OnSizeChanged(e);
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return;
            }
        }

        #endregion  Form Events

        #region     Menu Events

        /// <versions>23-04-2017(v0.0.1.34)</versions>
        private void MenuItemClicked(object sender, ItemClickEventArgs e)
        {
            try
            {
                if (e.Item.Tag != null)
                    OpenModule(e.Item.Tag.ToString());
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return;
            }
        }

        #endregion  Menu Events

        #endregion  Events

        #region     Methods

        /// <versions>20-02-2018(GesAMFC-v1.0.0.3)</versions>
        private void SetMenuOptions()
        {
            try
            {
                System.Drawing.Font objFontMenu = new System.Drawing.Font("Verdana", 13);

                System.Drawing.Font objFontSubMenu = new System.Drawing.Font("Verdana", 11);

                #region     Members
                BarSubItem objSubItem_Members = new BarSubItem();
                objSubItem_Members.Appearance.Font = objFontMenu;
                objSubItem_Members.Caption = "SÓCIOS";
                this.BarMainMenu.AddItem(objSubItem_Members);

                #region     Ficha Sócio
                BarButtonItem objButtonItem_MemberFile = new BarButtonItem();
                objButtonItem_MemberFile.Appearance.Font = objFontSubMenu;
                objButtonItem_MemberFile.Caption = "FICHAS SÓCIOS";
                objButtonItem_MemberFile.Tag = "Members";
                objButtonItem_MemberFile.ButtonStyle = BarButtonStyle.Default;
                objButtonItem_MemberFile.ItemClick += new ItemClickEventHandler(MenuItemClicked);
                objSubItem_Members.AddItem(objButtonItem_MemberFile);
                #endregion  Ficha Sócio

                #region     Lotes Sócio
                BarButtonItem objButtonItem_MemberlLotes = new BarButtonItem();
                objButtonItem_MemberlLotes.Appearance.Font = objFontSubMenu;
                objButtonItem_MemberlLotes.Caption = "LOTES SÓCIOS";
                objButtonItem_MemberlLotes.Tag = "Lotes";
                objButtonItem_MemberlLotes.ButtonStyle = BarButtonStyle.Default;
                objButtonItem_MemberlLotes.ItemClick += new ItemClickEventHandler(MenuItemClicked);
                objSubItem_Members.AddItem(objButtonItem_MemberlLotes);
                #endregion  Lotes Sócio

                #endregion  Members

                #region     Payments
                BarSubItem objSubItem_Payments = new BarSubItem();
                objSubItem_Payments.Appearance.Font = objFontMenu;
                objSubItem_Payments.Caption = "PAGAMENTOS";
                this.BarMainMenu.AddItem(objSubItem_Payments);

                #region     Cash On Hand
                BarButtonItem objButtonItem_CashPayments = new BarButtonItem();
                objButtonItem_CashPayments.Appearance.Font = objFontSubMenu;
                objButtonItem_CashPayments.Caption = "CAIXA";
                objButtonItem_CashPayments.Tag = "Caixa";
                objButtonItem_CashPayments.ButtonStyle = BarButtonStyle.Default;
                objButtonItem_CashPayments.ItemClick += new ItemClickEventHandler(MenuItemClicked);
                objSubItem_Payments.AddItem(objButtonItem_CashPayments);
                #endregion  Cash On Hand

                #region     Admission Fee
                BarButtonItem objButtonItem_Joias = new BarButtonItem();
                objButtonItem_Joias.Appearance.Font = objFontSubMenu;
                objButtonItem_Joias.Caption = "JOIAS";
                objButtonItem_Joias.Tag = "Joias";
                objButtonItem_Joias.ButtonStyle = BarButtonStyle.Default;
                objButtonItem_Joias.ItemClick += new ItemClickEventHandler(MenuItemClicked);
                objSubItem_Payments.AddItem(objButtonItem_Joias);
                #endregion  Admission Fee

                #region     Periodic Member Due
                BarButtonItem objButtonItem_Quotas = new BarButtonItem();
                objButtonItem_Quotas.Appearance.Font = objFontSubMenu;
                objButtonItem_Quotas.Caption = "QUOTAS";
                objButtonItem_Quotas.Tag = "Quotas";
                objButtonItem_Quotas.ButtonStyle = BarButtonStyle.Default;
                objButtonItem_Quotas.ItemClick += new ItemClickEventHandler(MenuItemClicked);
                objSubItem_Payments.AddItem(objButtonItem_Quotas);
                #endregion  Periodic Member Due

                #region     Infrastructures
                BarButtonItem objButtonItem_Infrastructures = new BarButtonItem();
                objButtonItem_Infrastructures.Appearance.Font = objFontSubMenu;
                objButtonItem_Infrastructures.Caption = "INFRAESTRUTURAS";
                objButtonItem_Infrastructures.Tag = "Infraestruturas";
                objButtonItem_Infrastructures.ButtonStyle = BarButtonStyle.Default;
                objButtonItem_Infrastructures.ItemClick += new ItemClickEventHandler(MenuItemClicked);
                objSubItem_Payments.AddItem(objButtonItem_Infrastructures);
                #endregion  Infrastructures

                #region     Land Area Transfer
                BarButtonItem objButtonItem_LandAreaTransfer = new BarButtonItem();
                objButtonItem_LandAreaTransfer.Appearance.Font = objFontSubMenu;
                objButtonItem_LandAreaTransfer.Caption = "CEDÊNCIAS TERRENO";
                objButtonItem_LandAreaTransfer.Tag = "Cedências";
                objButtonItem_LandAreaTransfer.ButtonStyle = BarButtonStyle.Default;
                objButtonItem_LandAreaTransfer.ItemClick += new ItemClickEventHandler(MenuItemClicked);
                objSubItem_Payments.AddItem(objButtonItem_LandAreaTransfer);
                #endregion  Land Area Transfer

                #region     Sewers
                BarButtonItem objButtonItem_Sewers = new BarButtonItem();
                objButtonItem_Sewers.Appearance.Font = objFontSubMenu;
                objButtonItem_Sewers.Caption = "ESGOTOS";
                objButtonItem_Sewers.Tag = "Esgotos";
                objButtonItem_Sewers.ButtonStyle = BarButtonStyle.Default;
                objButtonItem_Sewers.ItemClick += new ItemClickEventHandler(MenuItemClicked);
                objSubItem_Payments.AddItem(objButtonItem_Sewers);
                #endregion  Sewers

                #region     Urban Reconversion
                BarButtonItem objButtonItem_UrbanReconversion = new BarButtonItem();
                objButtonItem_UrbanReconversion.Appearance.Font = objFontSubMenu;
                objButtonItem_UrbanReconversion.Caption = "RECONVERSÃO URBANÍSTICA";
                objButtonItem_UrbanReconversion.Tag = "Reconversao";
                objButtonItem_UrbanReconversion.ButtonStyle = BarButtonStyle.Default;
                objButtonItem_UrbanReconversion.ItemClick += new ItemClickEventHandler(MenuItemClicked);
                objSubItem_Payments.AddItem(objButtonItem_UrbanReconversion);
                #endregion  Urban Reconversion

                #region     Other Payments
                //BarButtonItem objButtonItem_OtherPayments = new BarButtonItem();
                //objButtonItem_OtherPayments.Appearance.Font = objFontSubMenu;
                //objButtonItem_OtherPayments.Caption = "Outros pagamentos";
                //objButtonItem_OtherPayments.Tag = "OtherPayments";
                //objButtonItem_OtherPayments.ButtonStyle = BarButtonStyle.Default;
                //objButtonItem_OtherPayments.ItemClick += new ItemClickEventHandler(MenuItemClicked);
                //objSubItem_Payments.AddItem(objButtonItem_OtherPayments);
                #endregion  Other Payments

                #endregion  Payments

                #region     CONTA CORRENTE
                BarSubItem objSubItem_CC = new BarSubItem();
                objSubItem_CC.Caption = "CONTA CORRENTE";
                objSubItem_CC.Appearance.Font = objFontMenu;
                this.BarMainMenu.AddItem(objSubItem_CC);

                #region     CC INFRA
                BarButtonItem objButtonItem_CCINFRA = new BarButtonItem();
                objButtonItem_CCINFRA.Appearance.Font = objFontSubMenu;
                objButtonItem_CCINFRA.Caption = "INFRAESTRUTURAS";
                objButtonItem_CCINFRA.Tag = "CC_INFRA";
                objButtonItem_CCINFRA.ButtonStyle = BarButtonStyle.Default;
                objButtonItem_CCINFRA.ItemClick += new ItemClickEventHandler(MenuItemClicked);
                objSubItem_CC.AddItem(objButtonItem_CCINFRA);
                #endregion  CC INFRA

                #region     CC CEDEN
                BarButtonItem objButtonItem_CCCEDEN = new BarButtonItem();
                objButtonItem_CCCEDEN.Appearance.Font = objFontSubMenu;
                objButtonItem_CCCEDEN.Caption = "CEDÊNCIAS";
                objButtonItem_CCCEDEN.Tag = "CC_CEDEN";
                objButtonItem_CCCEDEN.ButtonStyle = BarButtonStyle.Default;
                objButtonItem_CCCEDEN.ItemClick += new ItemClickEventHandler(MenuItemClicked);
                objSubItem_CC.AddItem(objButtonItem_CCCEDEN);
                #endregion  CC CEDEN

                #region     CC ESGOT
                BarButtonItem objButtonItem_CCESGOT = new BarButtonItem();
                objButtonItem_CCESGOT.Appearance.Font = objFontSubMenu;
                objButtonItem_CCESGOT.Caption = "ESGOTOS";
                objButtonItem_CCESGOT.Tag = "CC_ESGOT";
                objButtonItem_CCESGOT.ButtonStyle = BarButtonStyle.Default;
                objButtonItem_CCESGOT.ItemClick += new ItemClickEventHandler(MenuItemClicked);
                objSubItem_CC.AddItem(objButtonItem_CCESGOT);
                #endregion  CC ESGOT

                #region     CC RECON
                BarButtonItem objButtonItem_CCRECON = new BarButtonItem();
                objButtonItem_CCRECON.Appearance.Font = objFontSubMenu;
                objButtonItem_CCRECON.Caption = "RECONVERSÃO URBANÍSTICA";
                objButtonItem_CCRECON.Tag = "CC_RECON";
                objButtonItem_CCRECON.ButtonStyle = BarButtonStyle.Default;
                objButtonItem_CCRECON.ItemClick += new ItemClickEventHandler(MenuItemClicked);
                objSubItem_CC.AddItem(objButtonItem_CCRECON);
                #endregion  CC RECON

                #endregion  CONTA CORRENTE

                #region     Receipts
                BarSubItem objSubItem_Receipts = new BarSubItem();
                objSubItem_Receipts.Appearance.Font = objFontMenu;
                objSubItem_Receipts.Caption = "RECIBOS";
                this.BarMainMenu.AddItem(objSubItem_Receipts);

                #region     Pay Receipts
                BarButtonItem objButtonItem_PayReceipts = new BarButtonItem();
                objButtonItem_PayReceipts.Appearance.Font = objFontSubMenu;
                objButtonItem_PayReceipts.Caption = "RECIBOS PAGAMENTO";
                objButtonItem_PayReceipts.Tag = "Recibos_Pagamento";
                objButtonItem_PayReceipts.ButtonStyle = BarButtonStyle.Default;
                objButtonItem_PayReceipts.ItemClick += new ItemClickEventHandler(MenuItemClicked);
                objSubItem_Receipts.AddItem(objButtonItem_PayReceipts);
                #endregion  Pay Receipts

                #region     Pay Receipts
                //BarButtonItem objButtonItem_QuotasReceipts = new BarButtonItem();
                //objButtonItem_QuotasReceipts.Appearance.Font = objFontSubMenu;
                //objButtonItem_QuotasReceipts.Caption = "RECIBOS QUOTAS";
                //objButtonItem_QuotasReceipts.Tag = "Recibos_Quotas";
                //objButtonItem_QuotasReceipts.ButtonStyle = BarButtonStyle.Default;
                //objButtonItem_QuotasReceipts.ItemClick += new ItemClickEventHandler(MenuItemClicked);
                //objSubItem_Receipts.AddItem(objButtonItem_QuotasReceipts);
                #endregion  Pay Receipts

                #endregion  Receipts

                #region     Communicated

                BarSubItem objSubItem_Communicated = new BarSubItem();
                objSubItem_Communicated.Appearance.Font = objFontMenu;
                objSubItem_Communicated.Caption = "CARTAS";
                this.BarMainMenu.AddItem(objSubItem_Communicated);

                #region     Envelopes
                BarButtonItem objButtonItem_Envelopes = new BarButtonItem();
                objButtonItem_Envelopes.Appearance.Font = objFontSubMenu;
                objButtonItem_Envelopes.Caption = "ENVELOPES";
                objButtonItem_Envelopes.Tag = "Envelopes";
                objButtonItem_Envelopes.ButtonStyle = BarButtonStyle.Default;
                objButtonItem_Envelopes.ItemClick += new ItemClickEventHandler(MenuItemClicked);
                objSubItem_Communicated.AddItem(objButtonItem_Envelopes);
                #endregion  Envelopes

                //#region     Letters
                //BarButtonItem objButtonItem_Letters = new BarButtonItem();
                //objButtonItem_Letters.Appearance.Font = objFontSubMenu;
                //objButtonItem_Letters.Caption = "Cartas";
                //objButtonItem_Letters.Tag = "Letters";
                //objButtonItem_Letters.ButtonStyle = BarButtonStyle.Default;
                //objButtonItem_Letters.ItemClick += new ItemClickEventHandler(MenuItemClicked);
                //objSubItem_Communicated.AddItem(objButtonItem_Letters);
                //#endregion  Letters

                #endregion  Communicated

                #region     Documents

                BarSubItem objSubItem_Documents = new BarSubItem();
                objSubItem_Documents.Appearance.Font = objFontMenu;
                objSubItem_Documents.Caption = "DOCUMENTOS";
                this.BarMainMenu.AddItem(objSubItem_Documents);

                #region     NIB
                BarButtonItem objButtonItem_NIB = new BarButtonItem();
                objButtonItem_NIB.Appearance.Font = objFontSubMenu;
                objButtonItem_NIB.Caption = "NIB/IBAN ASSOCIAÇÃO";
                objButtonItem_NIB.Tag = "NIB";
                objButtonItem_NIB.ButtonStyle = BarButtonStyle.Default;
                objButtonItem_NIB.ItemClick += new ItemClickEventHandler(MenuItemClicked);
                objSubItem_Documents.AddItem(objButtonItem_NIB);
                #endregion  NIB

                #region     Parametros 
                BarButtonItem objButtonItem_Parametros  = new BarButtonItem();
                objButtonItem_Parametros.Appearance.Font = objFontSubMenu;
                objButtonItem_Parametros.Caption = "PARÂMETROS PAGAMENTOS";
                objButtonItem_Parametros.Tag = "Parametros";
                objButtonItem_Parametros.ButtonStyle = BarButtonStyle.Default;
                objButtonItem_Parametros.ItemClick += new ItemClickEventHandler(MenuItemClicked);
                objSubItem_Documents.AddItem(objButtonItem_Parametros );
                #endregion  Parametros

                #endregion  Documents
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return;
            }
        }

        private void Set_Member_Payment_Receipt()
        {
            try
            {
                Find_Member_Form objFindMemberForm = new Find_Member_Form();
                objFindMemberForm.FormClosing += delegate
                {
                    if (Program.Member_Found)
                    {
                        AMFCMember objMember = objFindMemberForm.MemberSelected;
                        if (objMember != null && objMember.NUMERO > 0 && objMember.NUMERO < objMember.MaxNumber)
                        {
                            Form_Receipt_Details objForm_Receipt_Details = new Form_Receipt_Details(objMember.NUMERO, objMember.NOME, objMember.NUMLOTE, 0);
                            objForm_Receipt_Details.FormClosing += delegate
                            {
                                if (objForm_Receipt_Details.SelectedValue > 0 && !String.IsNullOrEmpty(objForm_Receipt_Details.SelectedDescription))
                                {
                                    Form_Payment_Receipt obj_Form_Payment_Receipt = new Form_Payment_Receipt();
                                    obj_Form_Payment_Receipt.MdiParent = this;
                                    obj_Form_Payment_Receipt.Receipt_Number = 123;
                                    obj_Form_Payment_Receipt.Receipt_Value = objForm_Receipt_Details.SelectedValue;
                                    obj_Form_Payment_Receipt.Receipt_Member_Number = objMember.NUMERO;
                                    obj_Form_Payment_Receipt.Receipt_Member_Name = objMember.NOME;
                                    obj_Form_Payment_Receipt.Receipt_Member_Lote = objMember.NUMLOTE;
                                    obj_Form_Payment_Receipt.Receipt_Member_Quantia = Program.QuantiaToExtenso(obj_Form_Payment_Receipt.Receipt_Value);
                                    obj_Form_Payment_Receipt.Receipt_Member_Designacao = objForm_Receipt_Details.SelectedDescription;
                                    obj_Form_Payment_Receipt.Set_Member_Pay_Receipt();
                                    obj_Form_Payment_Receipt.FormClosing += delegate
                                    {

                                    };
                                    obj_Form_Payment_Receipt.Show();
                                    obj_Form_Payment_Receipt.StartPosition = FormStartPosition.CenterParent;
                                    obj_Form_Payment_Receipt.Focus();
                                    obj_Form_Payment_Receipt.BringToFront();
                                }
                            };
                            objForm_Receipt_Details.Show();
                            objForm_Receipt_Details.StartPosition = FormStartPosition.CenterParent;
                            objForm_Receipt_Details.Focus();
                            objForm_Receipt_Details.BringToFront();
                        }
                    }
                };
                objFindMemberForm.Show();
                objFindMemberForm.StartPosition = FormStartPosition.CenterParent;
                objFindMemberForm.Focus();
                objFindMemberForm.BringToFront();
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return;
            }
        }

        /// <versions>16-06-2017(v0.0.4.1)</versions>
        private void OpenModule(String sTag)
        {
            try
            {
                if (String.IsNullOrEmpty(sTag))
                    return;
                switch (sTag)
                {
                    #region     Members

                    #region     FICHAS SÓCIOS
                    case "Members":
                        if (_FormMembers != null && !_FormMembersClosed)
                        {
                            _FormMembers.Show();
                            _FormMembers.WindowState = FormWindowState.Normal;
                            _FormMembers.BringToFront();
                            break;
                        }
                        _FormMembers = new Admin_AMFC_Members();
                        if (_FormMembers != null)
                        {
                            _FormMembers.MdiParent = this;
                            _FormMembers.WindowState = FormWindowState.Normal;
                            _FormMembers.FormClosing += delegate { _FormMembersClosed = true; };
                            _FormMembers.SetMenuBarEntityClick(sTag);
                            _FormMembers.Show();
                            _FormMembers.BringToFront();
                            //_FormMembers.Location = new System.Drawing.Point(900, 200);
                        }
                        break;
                    #endregion  FICHAS SÓCIOS

                    #region     LOTES SÓCIOS
                    case "Lotes":
                        //if (_FormLotes != null && !_FormLotesClosed)
                        //{
                        //    _FormLotes.Show();
                        //    _FormLotes.WindowState = FormWindowState.Normal;
                        //    _FormLotes.BringToFront();
                        //    break;
                        //}
                        Admin_Lotes _FormLotes = new Admin_Lotes();
                        if (_FormLotes != null)
                        {
                            _FormLotes.MdiParent = this;
                            _FormLotes.WindowState = FormWindowState.Maximized;
                            //_FormLotes.FormClosing += delegate { _FormLotesClosed = true; };
                            //_FormLotes.SetMenuBarEntityClick(sTag);
                            _FormLotes.Show();
                            _FormLotes.BringToFront();
                        }
                        break;
                    #endregion  LOTES SÓCIOS

                    #endregion  Members

                    #region     Payments

                    #region     Cash On Hand
                    //BarButtonItem objButtonItem_CashPayments = new BarButtonItem();
                    //objButtonItem_CashPayments.Caption = "Caixa de Pagamentos";
                    //objButtonItem_CashPayments.Tag = "Caixa";
                    //objButtonItem_CashPayments.ButtonStyle = BarButtonStyle.Default;
                    //objButtonItem_CashPayments.ItemClick += new ItemClickEventHandler(MenuItemClicked);
                    //objSubItem_Payments.AddItem(objButtonItem_CashPayments);
                    case "Caixa":
                        //if (_FormCashPayments != null && !_FormCashPaymentsClosed)
                        //{
                        //    _FormCashPayments.Show();
                        //    _FormCashPayments.WindowState = FormWindowState.Normal;
                        //    _FormCashPayments.BringToFront();
                        //    break;
                        //}
                        Admin_Caixa objXtraForm_Admin_Caixa = new Admin_Caixa();
                        if (objXtraForm_Admin_Caixa != null)
                        {
                            #region     Debug
                            objXtraForm_Admin_Caixa.MdiParent = this;
                            objXtraForm_Admin_Caixa.WindowState = FormWindowState.Maximized;
                            objXtraForm_Admin_Caixa.FormClosing += delegate { };
                            objXtraForm_Admin_Caixa.Show();
                            objXtraForm_Admin_Caixa.BringToFront();
                            #endregion  Debug
                        }
                        break;
                    #endregion  Cash On Hand

                    #region     Admission Fee
                    case "Joias":
                        //if (_FormJoias != null && !_FormJoiasClosed)
                        //{
                        //    _FormJoias.Show();
                        //    _FormJoias.WindowState = FormWindowState.Normal;
                        //    _FormJoias.BringToFront();
                        //    break;
                        //}
                        Admin_Joias objXtraForm_Admin_Joias = new Admin_Joias();
                        if (objXtraForm_Admin_Joias != null)
                        {
                            #region     Debug
                            objXtraForm_Admin_Joias.MdiParent = this;
                            objXtraForm_Admin_Joias.WindowState = FormWindowState.Maximized;
                            objXtraForm_Admin_Joias.FormClosing += delegate { };
                            objXtraForm_Admin_Joias.Show();
                            objXtraForm_Admin_Joias.BringToFront();
                            #endregion  Debug
                        }
                        break;
                    #endregion  Admission Fee

                        
                    #region     Periodic Member Due
                    case "Quotas":
                        //if (_FormQuotas != null && !_FormQuotasClosed)
                        //{
                        //    _FormQuotas.Show();
                        //    _FormQuotas.WindowState = FormWindowState.Normal;
                        //    _FormQuotas.BringToFront();
                        //    break;
                        //}
                        Admin_Quotas objXtraForm_Admin_Quotas = new Admin_Quotas();
                        if (objXtraForm_Admin_Quotas != null)
                        {
                            #region     Debug
                            objXtraForm_Admin_Quotas.MdiParent = this;
                            objXtraForm_Admin_Quotas.WindowState = FormWindowState.Maximized;
                            objXtraForm_Admin_Quotas.FormClosing += delegate { };
                            objXtraForm_Admin_Quotas.Show();
                            objXtraForm_Admin_Quotas.BringToFront();
                            #endregion  Debug
                        }
                        break;
                    #endregion  Periodic Member Due

                    #region     Infraestructures
                    case "Infraestruturas":
                        //if (_FormInfras != null && !_FormInfrasClosed)
                        //{
                        //    _FormInfras.Show();
                        //    _FormInfras.WindowState = FormWindowState.Normal;
                        //    _FormInfras.BringToFront();
                        //    break;
                        //}
                        Admin_Pag objXtraForm_Admin_Pag_INFRA = new Admin_Pag(PAG_ENTIDADE.EntityTypes.INFRA);
                        if (objXtraForm_Admin_Pag_INFRA != null)
                        {
                            #region     Debug
                            objXtraForm_Admin_Pag_INFRA.MdiParent = this;
                            objXtraForm_Admin_Pag_INFRA.WindowState = FormWindowState.Maximized;
                            objXtraForm_Admin_Pag_INFRA.FormClosing += delegate { };
                            objXtraForm_Admin_Pag_INFRA.Show();
                            objXtraForm_Admin_Pag_INFRA.BringToFront();
                            #endregion  Debug
                        }
                        break;
                    #endregion  Infraestructures

                    #region     Cedências
                    case "Cedências":
                        //if (_FormCeden != null && !_FormCedencClosed)
                        //{
                        //    _FormCeden.Show();
                        //    _FormCeden.WindowState = FormWindowState.Normal;
                        //    _FormCeden.BringToFront();
                        //    break;
                        //}
                        Admin_Pag objXtraForm_Admin_Pag_CEDEN = new Admin_Pag(PAG_ENTIDADE.EntityTypes.CEDEN);
                        if (objXtraForm_Admin_Pag_CEDEN != null)
                        {
                            #region     Debug
                            objXtraForm_Admin_Pag_CEDEN.MdiParent = this;
                            objXtraForm_Admin_Pag_CEDEN.WindowState = FormWindowState.Maximized;
                            objXtraForm_Admin_Pag_CEDEN.FormClosing += delegate { };
                            objXtraForm_Admin_Pag_CEDEN.Show();
                            objXtraForm_Admin_Pag_CEDEN.BringToFront();
                            #endregion  Debug
                        }
                        break;
                    #endregion  Cedências

                    #region     Esgotos
                    case "Esgotos":
                        //if (_FormESGOT != null && !_FormESGOTcClosed)
                        //{
                        //    _FormESGOT.Show();
                        //    _FormESGOT.WindowState = FormWindowState.Normal;
                        //    _FormESGOT.BringToFront();
                        //    break;
                        //}
                        Admin_Pag objXtraForm_Admin_Pag_ESGOT = new Admin_Pag(PAG_ENTIDADE.EntityTypes.ESGOT);
                        if (objXtraForm_Admin_Pag_ESGOT != null)
                        {
                            #region     Debug
                            objXtraForm_Admin_Pag_ESGOT.MdiParent = this;
                            objXtraForm_Admin_Pag_ESGOT.WindowState = FormWindowState.Maximized;
                            objXtraForm_Admin_Pag_ESGOT.FormClosing += delegate { };
                            objXtraForm_Admin_Pag_ESGOT.Show();
                            objXtraForm_Admin_Pag_ESGOT.BringToFront();
                            #endregion  Debug
                        }
                        break;
                    #endregion  Esgotos

                    #region     Reconversao
                    case "Reconversao":
                        //if (_FormRECON != null && !_FormRECONcClosed)
                        //{
                        //    _FormRECON.Show();
                        //    _FormRECON.WindowState = FormWindowState.Normal;
                        //    _FormRECON.BringToFront();
                        //    break;
                        //}
                        Admin_Pag objXtraForm_Admin_Pag_RECON = new Admin_Pag(PAG_ENTIDADE.EntityTypes.RECON);
                        if (objXtraForm_Admin_Pag_RECON != null)
                        {
                            #region     Debug
                            objXtraForm_Admin_Pag_RECON.MdiParent = this;
                            objXtraForm_Admin_Pag_RECON.WindowState = FormWindowState.Maximized;
                            objXtraForm_Admin_Pag_RECON.FormClosing += delegate { };
                            objXtraForm_Admin_Pag_RECON.Show();
                            objXtraForm_Admin_Pag_RECON.BringToFront();
                            #endregion  Debug
                        }
                        break;
                    #endregion  Reconversao

                    #endregion  Payments


                    #region     Conta Corrente
                    //case "ContaCorrente":
                    //    Admin_ContaCorrente objXtraForm_ContaCorrente = new Admin_ContaCorrente();
                    //    if (objXtraForm_ContaCorrente != null)
                    //    {
                    //        #region     Debug
                    //        objXtraForm_ContaCorrente.MdiParent = this;
                    //        objXtraForm_ContaCorrente.WindowState = FormWindowState.Maximized;
                    //        objXtraForm_ContaCorrente.FormClosing += delegate { };
                    //        objXtraForm_ContaCorrente.Show();
                    //        objXtraForm_ContaCorrente.BringToFront();
                    //        #endregion  Debug
                    //    }
                    //    break;

                    case "CC_INFRA":
                        Admin_CC_INFRA _Form_CC_INFRA = new Admin_CC_INFRA();
                        if (_Form_CC_INFRA != null)
                        {
                            _Form_CC_INFRA.MdiParent = this;
                            _Form_CC_INFRA.WindowState = FormWindowState.Maximized;
                            _Form_CC_INFRA.FormClosing += delegate { };
                            _Form_CC_INFRA.Show();
                            _Form_CC_INFRA.BringToFront();
                        }
                        break;

                    case "CC_CEDEN":
                        Admin_CC_CEDEN _Form_CC_CEDEN = new Admin_CC_CEDEN();
                        if (_Form_CC_CEDEN != null)
                        {
                            _Form_CC_CEDEN.MdiParent = this;
                            _Form_CC_CEDEN.WindowState = FormWindowState.Maximized;
                            _Form_CC_CEDEN.FormClosing += delegate { };
                            _Form_CC_CEDEN.Show();
                            _Form_CC_CEDEN.BringToFront();
                        }
                        break;

                    case "CC_ESGOT":
                        Admin_CC_ESGOT _Form_CC_ESGOT = new Admin_CC_ESGOT();
                        if (_Form_CC_ESGOT != null)
                        {
                            _Form_CC_ESGOT.MdiParent = this;
                            _Form_CC_ESGOT.WindowState = FormWindowState.Maximized;
                            _Form_CC_ESGOT.FormClosing += delegate { };
                            _Form_CC_ESGOT.Show();
                            _Form_CC_ESGOT.BringToFront();
                        }
                        break;

                    case "CC_RECON":
                        Admin_CC_RECON _Form_CC_RECON = new Admin_CC_RECON();
                        if (_Form_CC_RECON != null)
                        {
                            _Form_CC_RECON.MdiParent = this;
                            _Form_CC_RECON.WindowState = FormWindowState.Maximized;
                            _Form_CC_RECON.FormClosing += delegate { };
                            _Form_CC_RECON.Show();
                            _Form_CC_RECON.BringToFront();
                        }
                        break;

                    #endregion  Conta Corrente


                    #region     Receipts

                    #region     Payment Receipts
                    case "Recibos_Pagamento":
                        //if (_FormPaymentReceipts != null && !_FormPaymentReceiptsClosed)
                        //{
                        //    _FormPaymentReceipts.Show();
                        //    _FormPaymentReceipts.WindowState = FormWindowState.Normal;
                        //    _FormPaymentReceipts.BringToFront();
                        //    break;
                        //}
                        Set_Member_Payment_Receipt();
                        break;
                    #endregion  Payment Receipts

                    #region     Quotas Receipts
                    //case "Recibos_Quotas":
                    //    Form_Recibo_Quotas objFormReciboQuotas = new Form_Recibo_Quotas(null, new AMFCYear(2018));
                    //    if (objFormReciboQuotas != null)
                    //    {
                    //        objFormReciboQuotas.MdiParent = this;
                    //        objFormReciboQuotas.WindowState = FormWindowState.Maximized;
                    //        objFormReciboQuotas.FormClosing += delegate { };
                    //        objFormReciboQuotas.Show();
                    //        objFormReciboQuotas.BringToFront();
                    //    }
                    //    break;
                    #endregion  Quotas Receipts

                    #endregion  Receipts

                        
                    #region     Communicated

                    #region     Envelopes
                    case "Envelopes":
                        //if (_FormEnvelopes != null && !_FormEnvelopesClosed)
                        //{
                        //    _FormEnvelopes.Show();
                        //    _FormEnvelopes.WindowState = FormWindowState.Normal;
                        //    _FormEnvelopes.BringToFront();
                        //    break;
                        //}
                        Form_Envelope objXtraForm_Form_Envelope = new Form_Envelope();
                        if (objXtraForm_Form_Envelope != null)
                        {
                            objXtraForm_Form_Envelope.MdiParent = this;
                            objXtraForm_Form_Envelope.WindowState = FormWindowState.Maximized;
                            objXtraForm_Form_Envelope.FormClosing += delegate { };
                            objXtraForm_Form_Envelope.Show();
                            objXtraForm_Form_Envelope.BringToFront();
                        }
                        break;
                    #endregion  Envelopes

                    #endregion  Communicated


                    #region     Documents

                    #region     NIB
                    case "NIB":
                        //if (_FormNIB != null && !_FormNIBClosed)
                        //{
                        //    _FormNIB.Show();
                        //    _FormNIB.WindowState = FormWindowState.Normal;
                        //    _FormNIB.BringToFront();
                        //    break;
                        //}
                        Form_Iban objFormNIB = new Form_Iban();
                        if (objFormNIB != null)
                        {
                            objFormNIB.MdiParent = this;
                            objFormNIB.WindowState = FormWindowState.Maximized;
                            objFormNIB.FormClosing += delegate { };
                            objFormNIB.Show();
                            objFormNIB.BringToFront();
                        }
                        break;
                    #endregion  NIB

                    #region     Parametros 
                    case "Parametros":
                        //if (_FormParametros != null && !_FormParametrosClosed)
                        //{
                        //    _FormParametros.Show();
                        //    _FormParametros.WindowState = FormWindowState.Normal;
                        //    _FormParametros.BringToFront();
                        //    break;
                        //}
                        Form_Doc_Parametros objFormParametros = new Form_Doc_Parametros();
                        if (objFormParametros != null)
                        {
                            objFormParametros.MdiParent = this;
                            objFormParametros.WindowState = FormWindowState.Maximized;
                            objFormParametros.FormClosing += delegate { };
                            objFormParametros.Show();
                            objFormParametros.BringToFront();
                        }
                        break;
                    #endregion  Parametros

                    #endregion  Documents

                    
                }
            }
            catch (Exception ex)
            {
                #region     Dispose Forms
                _FormMembers = null;
                _FormMembersClosed = false;
                #endregion Dispose Forms
                OpenModule(sTag); //try again ...
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return;
            }
        }

        ///// /// <versions>14-03-2017(v0.0.2.6)</versions>
        //private void HandleError(String sMethod, String sErrorMsg, Program.ErroType eErrorType, Boolean bShowPopUp)
        //{
        //    try
        //    {
        //        Program.WriteLog(sMethod + " ERROR", sErrorMsg, true, true, true, true);
        //        if (bShowPopUp)
        //            MessageBox.Show(sErrorMsg, Enum.GetName(typeof(Program.ErroType), eErrorType), MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //    catch (Exception ex)
        //    {
        //        Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
        //        return;
        //    }
        //}

        ///// /// <versions>14-03-2017(v0.0.2.6)</versions>
        //private String GetApplicationVersion()
        //{
        //    try
        //    {
        //        String sVersion = String.Empty;
        //        sVersion += Assembly.GetExecutingAssembly().GetName().Name + " - ";                 //Application Name
        //        sVersion += "v" + Assembly.GetExecutingAssembly().GetName().Version.ToString();     //Version
        //        String sUriPath = Assembly.GetExecutingAssembly().GetName().CodeBase;
        //        String sExecFile = new Uri(sUriPath).LocalPath; //necessária pra não dar erro de #URI not supportted"
        //        DateTime dtExecFileModifiedTime = File.GetLastWriteTime(sExecFile);
        //        sVersion += " - " + dtExecFileModifiedTime.Day.ToString("00") + "-" + dtExecFileModifiedTime.Month.ToString("00") + "-" + dtExecFileModifiedTime.Year.ToString("00");
        //        return sVersion;
        //    }
        //    catch (Exception ex)
        //    {
        //        Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
        //        return String.Empty; 
        //    }
        //}

        #endregion  Methods
    }
}
