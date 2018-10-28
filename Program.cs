using DevExpress.XtraEditors;
using GesAMFC.AMFC_Methods;
using MMI.Libraries;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using static GesAMFC.AMFC_Entities;

namespace GesAMFC
{
    /// <summary>AMFC Admin Library</summary>
    /// <author>Valter Lima</author>
    /// <creation>18-02-2017(v0.0.1.0)</creation>
    /// <versions>02-12-2017(GesAMFC-v0.0.4.41)</versions>
    static class Program
    {
        #region     Program Default Properties

        public static AMFCMembers AllDbMembers;

        #region     User Logged
        public static ApplicationUser   AppUser         { get; set; }
        public static Boolean           UserLogged      { get; set; }
        public static List<String>      AppListAdmins   { get; set; }
        public static List<String>      AppListUsers    { get; set; }
        public static CultureInfo       CurrentCulture  { get; set; }

        public static String XmlConfigFilePath = "GesAMFC_Config.xml";
        #endregion  User Logged

        #region     Form Actions

        public static Boolean Member_Added      { get; set; }
        public static Boolean Member_Edited     { get; set; }
        public static Boolean Member_Deleted    { get; set; }
        public static Boolean Member_Found      { get; set; }

        public enum ErroType { UNDEFINED = 0, ERROR = -1, EXCEPTION = -2 }

        #region     Grids PropertiesGesAMFC-v0.0.4.23
        public static Color FocusedRowBgColor   = Color.FromArgb(255, 242, 180, 82);
        public static Color GreenRowBgColor     = Color.FromArgb(150, 63, 193, 95);
        public static Color RedRowBgColor       = Color.FromArgb(190, 200, 0, 0);
        public static Color BlueRowBgColor      = Color.FromArgb(190, 124, 255, 239);
        public static Color YelloRowBgColor     = Color.FromArgb(255, 255, 216, 0);
        #endregion  Grids Properties

        #endregion  Form Actions

        public static String    DB_Not_Available = "n.d.";

        #region     Dates
        public static String    Date_Format_String = "yyyy-MM-dd";
        public static String    DBF_Insert_Date_Format_String = "dd-MM-yyyy";
        public static String    DBF_Compare_Date_Format_String = "dd-MM-yyyy";
        public static DateTime  DB_Min_Valid_Date = new DateTime(1986, 01, 01);
        public static DateTime  DB_Max_Valid_Date = new DateTime(2030, 12, 31);
        public static DateTime  Default_Date = DateTime.Today;
        public static String    Default_Date_Str = Default_Date.ToString(Program.Date_Format_String, CultureInfo.CurrentCulture);

        public static AMFCYear  DefaultYear     = new AMFCYear(DateTime.Today.Year);
        public static AMFCMonth DefaultMonth    = new AMFCMonth(DateTime.Today.Month);
        #endregion  Dates

        #region     Currency Pay Values
        public static String EuroSymbol = "€";
        public static Int32  Default_Pay_Values_DecimalPlaces = 3;
        public static String FormatString_Double3_DecimalPlaces = "{0:0.000}";
        public static String FormatString_Double3_Euro = "{0:c3}";
        public static String TextExit_Control_MaskType_Euro = "C3";
        public static Double Default_Pay_Value = 0.000;
        public static String Default_Pay_String = "0,000 €";
        public static String Default_Pay_Double_String = "0,000";
        public static Double DB_Min_Pay_Value = Default_Pay_Value;
        public static Double DB_Max_Pay_Value = 999999999.999;

        public static String FormatString_Double2_Euro = "{0:c2}";

        //public static Double OneEuroToEscudos = 200.482;

        #endregion  Currency Pay Values

        #region     Areas Values
        public static Int32  Default_Area_Values_DecimalPlaces = 2;
        public static String FormatString_Double2_DecimalPlaces = "{0:0.000}";
        public static String FormatString_Double2_Area = "{0:n2}";
        public static String TextExit_Control_MaskType_Area = "N2";
        public static Double Default_Area_Value = 0.00;
        public static String Default_Area_String = "0,00";
        public static String Default_Area_Double_String = "0,00";
        public static Double DB_Min_Area_Value = Default_Area_Value;
        public static Double DB_Max_Area_Value = 99999.99;
        #endregion  Areas Values

        #endregion  Program Default Properties

        public static Library_AMFC_Methods LibAMFC = new Library_AMFC_Methods();
        public static Library_AMFC_SQL Lib_AMFC_SQL = new Library_AMFC_SQL();

        #region     Entities Names

        #region     Entity QUOTA
        public static string Entity_QUOTA_Desc_Single       = "Quota";
        public static string Entity_QUOTA_Lower_Single      = "quota";
        public static string Entity_QUOTA_Upper_Single      = "QUOTA";
        public static string Entity_QUOTA_Desc_Plural       = "Quotas";
        public static string Entity_QUOTA_Lower_Plural      = "quotas";
        public static string Entity_QUOTA_Upper_Plural      = "QUOTAS";
        public static string Entity_QUOTA_Abbr_Lower        = "quota";
        public static string Entity_QUOTA_Abbr_Upper        = "QUOTA";
        public static string Entity_QUOTA_Desc_Short_Single     = "Quota";
        public static string Entity_QUOTA_Lower_Short_Single    = "quota";
        public static string Entity_QUOTA_Upper_Short_Single    = "QUOTA";
        public static string Entity_QUOTA_Desc_Short_Plural     = "Quotas";
        public static string Entity_QUOTA_Lower_Short_Plural    = "quotas";
        public static string Entity_QUOTA_Upper_Short_Plural    = "QUOTAS";
        #endregion  Entity QUOTA

        #region     Entity INFRA
        public static string Entity_INFRA_Desc_Single       = "Infraestruturas";
        public static string Entity_INFRA_Lower_Single      = "infraestruturas";
        public static string Entity_INFRA_Upper_Single      = "INFRAESTRUTURAS";
        public static string Entity_INFRA_Desc_Plural       = "Infraestruturas";
        public static string Entity_INFRA_Lower_Plural      = "infraestruturas";
        public static string Entity_INFRA_Upper_Plural      = "INFRAESTRUTURAS";
        public static string Entity_INFRA_Abbr_Lower        = "infra";
        public static string Entity_INFRA_Abbr_Upper        = "INFRA";
        public static string Entity_INFRA_Desc_Short_Single     = "Infraestruturas";
        public static string Entity_INFRA_Lower_Short_Single    = "infraestruturas";
        public static string Entity_INFRA_Upper_Short_Single    = "INFRAESTRUTURAS";
        public static string Entity_INFRA_Desc_Short_Plural     = "Infraestruturas";
        public static string Entity_INFRA_Lower_Short_Plural    = "infraestruturas";
        public static string Entity_INFRA_Upper_Short_Plural    = "INFRAESTRUTURAS";
        #endregion  Entity INFRA

        #region     Entity CEDEN
        public static string Entity_CEDEN_Desc_Single       = "Cedência de Terreno";
        public static string Entity_CEDEN_Lower_Single      = "cedência de terreno";
        public static string Entity_CEDEN_Upper_Single      = "CEDÊNCIA DE TERRENO";
        public static string Entity_CEDEN_Desc_Plural       = "Cedências de Terrenos";
        public static string Entity_CEDEN_Lower_Plural      = "cedências de terrenos";
        public static string Entity_CEDEN_Upper_Plural      = "CEDÊNCIAS DE TERRENOS";
        public static string Entity_CEDEN_Abbr_Lower        = "ceden";
        public static string Entity_CEDEN_Abbr_Upper        = "CEDEN";
        public static string Entity_CEDEN_Desc_Short_Single     = "Cedências";
        public static string Entity_CEDEN_Lower_Short_Single    = "cedências";
        public static string Entity_CEDEN_Upper_Short_Single    = "CEDÊNCIA";
        public static string Entity_CEDEN_Desc_Short_Plural     = "Cedências";
        public static string Entity_CEDEN_Lower_Short_Plural    = "cedências";
        public static string Entity_CEDEN_Upper_Short_Plural    = "CEDÊNCIAS";
        #endregion  Entity CEDEN

        #region     Entity RECON
        public static string Entity_RECON_Desc_Single       = "Reconversão Urbanística";
        public static string Entity_RECON_Lower_Single      = "reconversão urbanística";
        public static string Entity_RECON_Upper_Single      = "RECONVERSÃO URBANÍSTICA";
        public static string Entity_RECON_Desc_Plural       = "Reconversões Urbanísticas";
        public static string Entity_RECON_Lower_Plural      = "reconversões urbanísticas";
        public static string Entity_RECON_Upper_Plural      = "RECONVERSÕES URBANÍSTICAS";
        public static string Entity_RECON_Abbr_Lower        = "recon";
        public static string Entity_RECON_Abbr_Upper        = "RECONV";
        public static string Entity_RECON_Desc_Short_Single     = "Reconversão";
        public static string Entity_RECON_Lower_Short_Single    = "reconversão";
        public static string Entity_RECON_Upper_Short_Single    = "RECONVERSÃO";
        public static string Entity_RECON_Desc_Short_Plural     = "Reconversões";
        public static string Entity_RECON_Lower_Short_Plural    = "reconversões";
        public static string Entity_RECON_Upper_Short_Plural    = "RECONVERSÕES";
        #endregion  Entity RECON

        #region     Entity ESGOT
        public static string Entity_ESGOT_Desc_Single       = "Ramal de Esgoto";
        public static string Entity_ESGOT_Lower_Single      = "ramal de esgoto";
        public static string Entity_ESGOT_Upper_Single      = "RAMAL DE ESGOTO";
        public static string Entity_ESGOT_Desc_Plural       = "Ramais de Esgotos";
        public static string Entity_ESGOT_Lower_Plural      = "ramais de esgotos";
        public static string Entity_ESGOT_Upper_Plural      = "RAMAIS DE ESGOTOS";
        public static string Entity_ESGOT_Abbr_Lower        = "esgot";
        public static string Entity_ESGOT_Abbr_Upper        = "ESGOT";
        public static string Entity_ESGOT_Desc_Short_Single     = "Esgoto";
        public static string Entity_ESGOT_Lower_Short_Single    = "esgoto";
        public static string Entity_ESGOT_Upper_Short_Single    = "ESGOTO";
        public static string Entity_ESGOT_Desc_Short_Plural     = "Esgotos";
        public static string Entity_ESGOT_Lower_Short_Plural    = "esgotos";
        public static string Entity_ESGOT_Upper_Short_Plural    = "ESGOTOS";
        #endregion  Entity ESGOT
        
        #region     Entity CAIXA
        public static string Entity_CAIXA_Desc_Single       = "Pagamento";
        public static string Entity_CAIXA_Lower_Single      = "pagamento";
        public static string Entity_CAIXA_Upper_Single      = "PAGAMENTO";
        public static string Entity_CAIXA_Desc_Plural       = "Pagamentos";
        public static string Entity_CAIXA_Lower_Plural      = "pagamentos";
        public static string Entity_CAIXA_Upper_Plural      = "PAGAMENTOS";
        public static string Entity_CAIXA_Abbr_Lower        = "pagam";
        public static string Entity_CAIXA_Abbr_Upper        = "PAGAM";
        public static string Entity_CAIXA_Desc_Short_Single     = "Pagamento";
        public static string Entity_CAIXA_Lower_Short_Single    = "pagamento";
        public static string Entity_CAIXA_Upper_Short_Single    = "PAGAMENTO";
        public static string Entity_CAIXA_Desc_Short_Plural     = "Pagamentos";
        public static string Entity_CAIXA_Lower_Short_Plural    = "pagamentos";
        public static string Entity_CAIXA_Upper_Short_Plural    = "PAGAMENTOS";
        #endregion  Entity CAIXA

        #region     Entity RECIB
        public static string Entity_RECIB_Desc_Single       = "Recibo de Pagamento";
        public static string Entity_RECIB_Lower_Single      = "recibo de pagamento";
        public static string Entity_RECIB_Upper_Single      = "RECIBO DE PAGAMENTO";
        public static string Entity_RECIB_Desc_Plural       = "Recibos de Pagamentos";
        public static string Entity_RECIB_Lower_Plural      = "recibos de pagamentos";
        public static string Entity_RECIB_Upper_Plural      = "RECIBOS DE PAGAMENTOS";
        public static string Entity_RECIB_Abbr_Lower        = "recib";
        public static string Entity_RECIB_Abbr_Upper        = "RECIB";
        public static string Entity_RECIB_Desc_Short_Single     = "Recibo";
        public static string Entity_RECIB_Lower_Short_Single    = "recibo";
        public static string Entity_RECIB_Upper_Short_Single    = "RECIBO";
        public static string Entity_RECIB_Desc_Short_Plural     = "Recibos";
        public static string Entity_RECIB_Lower_Short_Plural    = "recibos";
        public static string Entity_RECIB_Upper_Short_Plural    = "RECIBOS";
        #endregion  Entity RECIB

        #endregion  Entities Names

        /// <summary>The main entry point for the application</summary>
        /// <versions>05-05-2017(v0.0.3.2)</versions>
        [STAThread]
        static void Main()
        {
            try
            {
                #region     Culture Info
                CultureInfo culture = CultureInfo.CreateSpecificCulture("pt-PT");
                // The following line provides localization for the application's user interface.  
                Thread.CurrentThread.CurrentUICulture = culture;

                // The following line provides localization for data formats.  
                Thread.CurrentThread.CurrentCulture = culture;

                // Set this culture as the default culture for all threads in this application.  
                // Note: The following properties are supported in the .NET Framework 4.5+ 
                //CultureInfo.DefaultThreadCurrentCulture = culture;
                //CultureInfo.DefaultThreadCurrentUICulture = culture;
                #endregion  Culture Info

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);

                //this.OneEuroToEscudos = Convert.ToDouble(LibAMFC.DBF_AMFC_Euro_To_Escudos.Trim());

                SetAppUsers();
                Application.Run(new GesAMFC_MainForm());
            }
            catch (Exception ex)
            {
                HandleError(ex.TargetSite.Name, ex.Message, ErroType.EXCEPTION, true, false);
                return;
            }
        }

        /// <versions>28-04-2017(v0.0.2.48)</versions>
        private static void SetAppUsers()
        {
            try
            {
                UserLogged = false;
                using (LibraryXML objLibraryXML = new LibraryXML(XmlConfigFilePath))
                {
                    #region     Get App Admins
                    String sListAdministrators = objLibraryXML.GetXmlConfigFileNodeValue("Administrators");
                    AppListAdmins = sListAdministrators.Split(new String[] { "," }, StringSplitOptions.RemoveEmptyEntries).Select(pw => pw.Trim()).ToList();
                    #endregion  Get App Admins
                    #region     Get App Users
                    String sListUsers = objLibraryXML.GetXmlConfigFileNodeValue("Users");
                    AppListUsers = sListUsers.Split(new String[] { "," }, StringSplitOptions.RemoveEmptyEntries).Select(pw => pw.Trim()).ToList();
                    #endregion  Get App Users
                }
            }
            catch (Exception ex)
            {
                HandleError(ex.TargetSite.Name, ex.Message, ErroType.EXCEPTION, true, false);
                return;
            }
        }

        /// <versions>28-04-2017(v0.0.2.48)</versions>
        public static void WriteLog(String sMethod, String sMessage, Boolean bWriteTime, Boolean bShowUser, Boolean bLogBrakeLine, Boolean bDetailsBrakeLine)
        {
            try
            {
                //using (StreamWriter objTextStreamLog = File.AppendText(ApplicationDirPath + "\\" + LogsFileName))
                //{
                //    String sLog = String.Empty;
                //    if (bLogBrakeLine)
                //        sLog += "\r\n" + "\r\n";
                //    if (bWriteTime)
                //        sLog += "[" + DateTime.Now.ToString() + "]" + " -> ";
                //    if (bShowUser && ApplicationUser != null)
                //    {
                //        if (!String.IsNullOrEmpty(ApplicationUser.Username))
                //            sLog += ApplicationUser.Username;
                //        if (ApplicationUser.UserInfo != null && !String.IsNullOrEmpty(ApplicationUser.UserInfo.Name))
                //            sLog += " (" + ApplicationUser.UserInfo.Name + ")";
                //        if (!String.IsNullOrEmpty(ApplicationUser.UserIP))
                //            sLog += " [" + ApplicationUser.UserIP + "]";
                //        sLog += " - ";
                //    }
                //    if (!String.IsNullOrEmpty(sMethod)) sLog += sMethod + ": ";
                //    if (bDetailsBrakeLine)
                //        sLog += "\r\n";
                //    if (!String.IsNullOrEmpty(sMessage)) sLog += sMessage;
                //    sLog += "\r\n";
                //    if (!String.IsNullOrEmpty(sLog))
                //        objTextStreamLog.WriteLine(sLog);
                //}
            }
            catch (Exception ex)
            {
                HandleError(ex.TargetSite.Name, ex.Message, ErroType.EXCEPTION, true, false);
                return;
            }
        }

        /// <versions>28-04-2017(v0.0.2.48)</versions>
        public static void Exit_Application()
        {
            try
            {
                //XtraMessageBox.Show("A aplicação vai encerrar ...", "Fechar Programa", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Application.Exit();
            }
            catch (Exception ex)
            {
                HandleError(ex.TargetSite.Name, ex.Message, ErroType.EXCEPTION, true, false);
                Application.Exit();
            }
        }

        /// <versions>28-04-2017(v0.0.2.48)</versions>
        public static Boolean Dialog_Exit_Application()
        {
            try
            {
                DialogResult objDialogResult = XtraMessageBox.Show("Deseja sair da aplicação?", "Fechar Programa ?", MessageBoxButtons.OK, MessageBoxIcon.Question);
                if (objDialogResult == DialogResult.OK)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                HandleError(ex.TargetSite.Name, ex.Message, ErroType.EXCEPTION, true, false);
                return false;
            }
        }

        /// <versions>28-04-2017(v0.0.2.48)</versions>
        public static Boolean Dialog_Exit_Form(String sFormDescription)
        {
            try
            {
                DialogResult objDialogResult = XtraMessageBox.Show("Deseja fechar esta janela de " + "[" + sFormDescription + "]" + " ?", "Fechar Janela ?", MessageBoxButtons.OK, MessageBoxIcon.Question);
                if (objDialogResult == DialogResult.OK)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                HandleError(ex.TargetSite.Name, ex.Message, ErroType.EXCEPTION, true, false);
                return false;
            }
        }

        /// <versions>28-04-2017(v0.0.2.48)</versions>
        public static Int32 LoginUser(String sPasswordCheck)
        {
            try
            {
                UserLogged = false;

                if (AppListAdmins == null || AppListAdmins.Count == 0 || AppListUsers == null || AppListUsers.Count == 0)
                {
                    String sError = "Não foi possível obter as listas de Users e Admins!";
                    HandleError("LoginUser", sError, ErroType.ERROR, true, false);
                    return -1;
                }

                if (String.IsNullOrEmpty(sPasswordCheck))
                {
                    //password vazia
                    return -3;
                }
                String sPassword = sPasswordCheck.Trim().ToLower();

                #region     Check if Is Admin
                foreach (String sPw in AppListAdmins)
                {
                    String sPasswordAuthorized = sPw.Trim().ToLower();
                    if (sPassword == sPasswordAuthorized)
                    {
                        AppUser = new ApplicationUser(ApplicationUser.AMFC_UserTypes.ADMIN);
                        AppUser.Password = sPasswordAuthorized;
                        return 1;
                    }
                }
                #endregion  Check if Is Admin

                #region     Check if Is User
                foreach (String sPw in AppListUsers)
                {
                    String sPasswordAuthorized = sPw.Trim().ToLower();
                    if (sPassword == sPasswordAuthorized)
                    {
                        AppUser = new ApplicationUser(ApplicationUser.AMFC_UserTypes.USER);
                        AppUser.Password = sPasswordAuthorized;
                        return 1;
                    }
                }
                #endregion  Check if Is User

                return -2; //Password inválida
            }
            catch (Exception ex)
            {
                HandleError(ex.TargetSite.Name, ex.Message, ErroType.EXCEPTION, true, false);
                UserLogged = false;
                return -1;
            }
        }

        #region     Methods Boolean Convert Yes/No

        public enum DB_Boolean_Type { N = 0, S = 1 }

        public static Boolean ConvertYesOrNoToBoolean(String sSorN)
        {
            try
            {
                sSorN = sSorN.Trim();
                if (String.IsNullOrEmpty(sSorN) || sSorN.Length != 1)
                    return false;
                try //Se "0" ou "1"
                {
                    String sBoolInt = sSorN.Trim();
                    Int16 iBoolInt = Convert.ToInt16(sBoolInt);
                    switch (iBoolInt)
                    {
                        case 0:
                            return false;
                        case 1:
                            return true;
                    }
                }
                catch //"S" ou "N" 
                {
                    sSorN = sSorN.Trim().ToUpper();
                    if (sSorN != Enum.GetName(typeof(DB_Boolean_Type), DB_Boolean_Type.N) && sSorN != Enum.GetName(typeof(DB_Boolean_Type), DB_Boolean_Type.S))
                        return false;
                    DB_Boolean_Type eBooleanType = (DB_Boolean_Type)Enum.Parse(typeof(DB_Boolean_Type), sSorN);
                    switch (eBooleanType)
                    {
                        case DB_Boolean_Type.N:
                            return false;
                        case DB_Boolean_Type.S:
                            return true;
                    }
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public static String ConvertBooleanTypeToYesOrNo(DB_Boolean_Type eBooleanType)
        {
            try
            {
                if (eBooleanType != DB_Boolean_Type.N && eBooleanType != DB_Boolean_Type.S)
                    return String.Empty;
                String sSorN = Enum.GetName(typeof(DB_Boolean_Type), eBooleanType);
                return sSorN;
            }
            catch
            {
                return String.Empty;
            }
        }

        public static String ConvertBooleanToYesOrNo(Boolean bBool)
        {
            try
            {
                String sSorN = String.Empty;
                if (bBool)
                    sSorN = Enum.GetName(typeof(DB_Boolean_Type), DB_Boolean_Type.S);
                else
                    sSorN = Enum.GetName(typeof(DB_Boolean_Type), DB_Boolean_Type.N);
                return sSorN;
            }
            catch
            {
                return String.Empty;
            }
        }

        #endregion  Methods Boolean Convert Yes/No

        #region     DateTime Methods

        /// <versions>08-05-2017(v0.0.3.9)</versions>
        public static Boolean IsValidDateTime(DateTime dtDateTime)
        {
            try
            {
                if (dtDateTime == null)
                    return false;
                if (dtDateTime <= DB_Min_Valid_Date || dtDateTime >= DB_Max_Valid_Date)
                    return false;
                return true;
            }
            catch (Exception ex)
            {
                HandleError(ex.TargetSite.Name, ex.Message, ErroType.EXCEPTION, true, false);
                return false;
            }
        }

        /// <versions>08-05-2017(v0.0.3.9)</versions>
        public static Boolean IsValidYear(Int32 iDateYear)
        {
            try
            {
                if (iDateYear < DB_Min_Valid_Date.Year || iDateYear > DB_Max_Valid_Date.Year)
                    return false;
                return true;
            }
            catch (Exception ex)
            {
                HandleError(ex.TargetSite.Name, ex.Message, ErroType.EXCEPTION, true, false);
                return false;
            }
        }

        /// <versions>08-05-2017(v0.0.3.9)</versions>
        public static Boolean IsValidMonth(Int32 iDateMonth)
        {
            try
            {
                if (iDateMonth < 1 || iDateMonth > 12)
                    return false;
                return true;
            }
            catch (Exception ex)
            {
                HandleError(ex.TargetSite.Name, ex.Message, ErroType.EXCEPTION, true, false);
                return false;
            }
        }

        /// <versions>08-05-2017(v0.0.3.9)</versions>
        public static DateTime SetDateTimeValue(DateTime dtDateTime, Int32 iYear, Int32 iMonth)
        {
            try
            {
                DateTime dtValidDateTime = dtDateTime;
                if (dtDateTime == null)
                    dtValidDateTime = Default_Date;
                if (dtDateTime.Date == DateTime.Today)
                {
                    if (IsValidYear(iYear) && IsValidMonth(iMonth))
                    {
                        if (iYear != dtDateTime.Year && iMonth != dtDateTime.Month)
                            dtValidDateTime = new DateTime(iYear, iMonth, 1);
                    }
                }
                else
                    dtValidDateTime = IsValidDateTime(dtDateTime) ? dtDateTime : Default_Date;
                return dtValidDateTime;
            }
            catch (Exception ex)
            {
                HandleError(ex.TargetSite.Name, ex.Message, ErroType.EXCEPTION, true, false);
                return Default_Date;
            }
        }

        /// <versions>08-05-2017(v0.0.3.9)</versions>
        public static DateTime ConvertToValidDateTime(String sDateTime)
        {
            try
            {
                DateTime objDateTime = new DateTime();
                if (!IsValidTextString(sDateTime))
                    return new DateTime();
                objDateTime = Convert.ToDateTime(sDateTime.Trim());
                if (!IsValidDateTime(objDateTime))
                {
                    String sYear = objDateTime.Year.ToString().Trim();
                    String sTwoDigitYear = sYear.Length == 2 ? sYear : (sYear.Length == 4 ? sYear.Substring(2, 2) : sYear);
                    Int32 iTwoDigitYear = Convert.ToInt32(sTwoDigitYear);
                    Int32 iFourDigitYear = CultureInfo.CurrentCulture.Calendar.ToFourDigitYear(iTwoDigitYear);
                    DateTime objNewDateTime = new DateTime(iFourDigitYear, objDateTime.Month, objDateTime.Day, objDateTime.Hour, objDateTime.Minute, objDateTime.Second, objDateTime.Millisecond);
                    return objNewDateTime;
                }

                return objDateTime;
            }
            catch (Exception ex)
            {
                HandleError(ex.TargetSite.Name, ex.Message, ErroType.EXCEPTION, true, false);
                return new DateTime();
            }
        }

        /// <versions>20-11-2017(GesAMFC-v0.0.4.35)</versions>
        public static void SetCalendarControl(DateEdit objDateEdit)
        {
            try
            {
                objDateEdit.DateTime = Program.Default_Date;
                objDateEdit.Properties.Mask.Culture = new CultureInfo("pt-PT");
                objDateEdit.Properties.Mask.EditMask = Date_Format_String;
                objDateEdit.Properties.Mask.UseMaskAsDisplayFormat = true;
                objDateEdit.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.DateTime;
                objDateEdit.Properties.MinValue = DB_Min_Valid_Date;
                objDateEdit.Properties.MaxValue = DB_Max_Valid_Date;
            }
            catch (Exception ex)
            {
                HandleError(ex.TargetSite.Name, ex.Message, ErroType.EXCEPTION, true, false);
                return;
            }
        }

        /// <versions>21-11-2017(GesAMFC-v0.0.4.35)</versions>
        public static void DateEdit_QueryPopUp_Event(DateEdit dateEdit, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = !dateEdit.Enabled || dateEdit.Properties.ReadOnly;
        }

        #endregion  DateTime Methods

        #region     Currency Euro Values Methods

        /// <versions>21-11-2017(GesAMFC-v0.0.4.35)</versions>
        public static Boolean IsValidCurrencyEuroValue(String sCurrencyEuroValue)
        {
            try
            {
                Double dCurrencyValue = SetPayCurrencyEuroDoubleValue(sCurrencyEuroValue);
                if (dCurrencyValue < DB_Min_Pay_Value || dCurrencyValue > DB_Max_Pay_Value)
                    return false;
                return true;
            }
            catch (Exception ex)
            {
                HandleError(ex.TargetSite.Name, ex.Message, ErroType.EXCEPTION, true, false);
                return false;
            }
        }

        /// <versions>21-11-2017(GesAMFC-v0.0.4.35)</versions
        public static Boolean IsValidCurrencyEuroValue(Double dCurrencyValue)
        {
            try
            {
                if (dCurrencyValue < DB_Min_Pay_Value || dCurrencyValue > DB_Max_Pay_Value)
                    return false;
                return true;
            }
            catch (Exception ex)
            {
                HandleError(ex.TargetSite.Name, ex.Message, ErroType.EXCEPTION, true, false);
                return false;
            }
        }

        /// <versions>21-11-2017(GesAMFC-v0.0.4.35)</versions>
        public static Double SetPayCurrencyEuroDoubleValue(String sCurrencyEuroValue)
        {
            try
            {
                //CultureInfo objCultureInfo = CultureInfo.CurrentCulture;
                CultureInfo objCultureInfo = new CultureInfo("pt-PT");
                String sCurrencySymbol = objCultureInfo.NumberFormat.CurrencySymbol;
                String sDecimalSymbol = objCultureInfo.NumberFormat.NumberDecimalSeparator;

                Double dCurrencyValue = Default_Pay_Value;

                sCurrencyEuroValue = sCurrencyEuroValue.Trim();
                if (!IsValidTextString(sCurrencyEuroValue))
                    return -1;

                try
                {
                    dCurrencyValue = Double.Parse(sCurrencyEuroValue, objCultureInfo);
                }
                catch
                {
                    try
                    {
                        dCurrencyValue = double.Parse(sCurrencyEuroValue, NumberStyles.Currency);
                    }
                    catch
                    {
                        try
                        {
                            Decimal.Parse(sCurrencyEuroValue, NumberStyles.AllowCurrencySymbol | NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands, objCultureInfo);
                        }
                        catch
                        {

                            dCurrencyValue = 0;
                        }
                    }
                }
                if (dCurrencyValue >= 0)
                    return dCurrencyValue;

                if (sCurrencyEuroValue.Contains(EuroSymbol))
                    sCurrencyEuroValue = sCurrencyEuroValue.Replace(EuroSymbol, "").Trim();

                if (sCurrencyEuroValue.Contains("."))
                    sCurrencyEuroValue = sCurrencyEuroValue.Replace(".", sDecimalSymbol).Trim();

                dCurrencyValue = Math.Round(Convert.ToDouble(sCurrencyEuroValue), Program.Default_Pay_Values_DecimalPlaces);

                if (dCurrencyValue < DB_Min_Pay_Value || dCurrencyValue > DB_Max_Pay_Value)
                    return Default_Pay_Value;

                return dCurrencyValue;
            }
            catch (Exception ex)
            {
                HandleError(ex.TargetSite.Name, ex.Message, ErroType.EXCEPTION, true, false);
                return Default_Pay_Value;
            }
        }

        /// <versions>21-11-2017(GesAMFC-v0.0.4.35)</versions>
        public static Double SetPayCurrencyEuroDoubleValue(Double dCurrencyValue)
        {
            try
            {              
                 if (dCurrencyValue < DB_Min_Pay_Value || dCurrencyValue > DB_Max_Pay_Value)
                    return Default_Pay_Value;

                return dCurrencyValue;
            }
            catch (Exception ex)
            {
                HandleError(ex.TargetSite.Name, ex.Message, ErroType.EXCEPTION, true, false);
                return Default_Pay_Value;
            }
        }

        /// <versions>21-11-2017(GesAMFC-v0.0.4.35)</versions>
        public static String SetPayCurrencyEuroStringValue(Double dCurrencyValue)
        {
            try
            {
                String sCurrencyEuroValue = Default_Pay_String;
                if (dCurrencyValue < 0)
                    return Default_Pay_String;
                sCurrencyEuroValue = String.Format(FormatString_Double3_Euro, dCurrencyValue);
                return sCurrencyEuroValue;
            }
            catch (Exception ex)
            {
                HandleError(ex.TargetSite.Name, ex.Message, ErroType.EXCEPTION, true, false);
                return Default_Pay_String;
            }
        }

        /// <versions>21-11-2017(GesAMFC-v0.0.4.35)</versions>
        public static String SetPayCurrencyEuroStringValue(Double dCurrencyValue, String sStringFormat)
        {
            try
            {
                String sCurrencyEuroValue = Default_Pay_String;
                if (dCurrencyValue < 0)
                    return Default_Pay_String;
                sCurrencyEuroValue = String.Format(sStringFormat, dCurrencyValue);
                return sCurrencyEuroValue;
            }
            catch (Exception ex)
            {
                HandleError(ex.TargetSite.Name, ex.Message, ErroType.EXCEPTION, true, false);
                return Default_Pay_String;
            }
        }

        /// <versions>21-11-2017(GesAMFC-v0.0.4.35)</versions>
        public static String SetPayDoubleStringValue(Double dCurrencyValue)
        {
            try
            {
                String sCurrencyEuroValue = Default_Pay_String;
                if (dCurrencyValue < 0)
                    return Default_Pay_Double_String;
                sCurrencyEuroValue = String.Format(FormatString_Double3_DecimalPlaces, dCurrencyValue);
                return sCurrencyEuroValue;
            }
            catch (Exception ex)
            {
                HandleError(ex.TargetSite.Name, ex.Message, ErroType.EXCEPTION, true, false);
                return Default_Pay_String;
            }
        }

        /// <versions>21-11-2017(GesAMFC-v0.0.4.35)</versions>
        public static void SetPayEditValues(TextEdit objCurrencyEditValue)
        {
            try
            {
                objCurrencyEditValue.Properties.Mask.Culture = new System.Globalization.CultureInfo("pt-PT");
                objCurrencyEditValue.Properties.Mask.EditMask = Program.TextExit_Control_MaskType_Euro;
                objCurrencyEditValue.Properties.Mask.UseMaskAsDisplayFormat = true;
                objCurrencyEditValue.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            }
            catch (Exception ex)
            {
                HandleError(ex.TargetSite.Name, ex.Message, ErroType.EXCEPTION, true, false);
                return;
            }
        }

        #endregion  Currency Euro Values Methods

        #region     Areas Methods

        /// <versions>16-03-2018(GesAMFC-v1.0.0.3)</versions>
        public static Boolean IsValidAreaValue(String sCurrencyEuroValue)
        {
            try
            {
                Double dCurrencyValue = SetAreaDoubleValue(sCurrencyEuroValue);
                if (dCurrencyValue < DB_Min_Area_Value || dCurrencyValue > DB_Max_Area_Value)
                    return false;
                return true;
            }
            catch (Exception ex)
            {
                HandleError(ex.TargetSite.Name, ex.Message, ErroType.EXCEPTION, true, false);
                return false;
            }
        }

        /// <versions>21-11-2017(GesAMFC-v0.0.4.35)</versions>
        public static Double SetAreaDoubleValue(String sAreaValue)
        {
            try
            {
                //CultureInfo objCultureInfo = CultureInfo.CurrentCulture;
                CultureInfo objCultureInfo = new CultureInfo("pt-PT");
                String sDecimalSymbol = objCultureInfo.NumberFormat.NumberDecimalSeparator;

                Double dAreaValue = Default_Area_Value;

                sAreaValue = sAreaValue.Trim();
                if (!IsValidTextString(sAreaValue))
                    return -1;

                try
                {
                    dAreaValue = Double.Parse(sAreaValue, objCultureInfo);
                }
                catch
                {
                    try
                    {
                        Decimal.Parse(sAreaValue, NumberStyles.AllowDecimalPoint | NumberStyles.AllowThousands, objCultureInfo);
                    }
                    catch
                    {

                        dAreaValue = 0;
                    }
                }
                if (dAreaValue >= 0)
                    return dAreaValue;

                if (sAreaValue.Contains("."))
                    sAreaValue = sAreaValue.Replace(".", sDecimalSymbol).Trim();

                dAreaValue = Math.Round(Convert.ToDouble(sAreaValue), Program.Default_Area_Values_DecimalPlaces);

                if (dAreaValue < DB_Min_Area_Value || dAreaValue > DB_Max_Area_Value)
                    return Default_Area_Value;

                return dAreaValue;
            }
            catch (Exception ex)
            {
                HandleError(ex.TargetSite.Name, ex.Message, ErroType.EXCEPTION, true, false);
                return Default_Area_Value;
            }
        }

        /// <versions>21-11-2017(GesAMFC-v0.0.4.35)</versions>
        public static void SetAreaEditValues(TextEdit objAreaEditValue)
        {
            try
            {
                objAreaEditValue.Properties.Mask.Culture = new System.Globalization.CultureInfo("pt-PT");
                objAreaEditValue.Properties.Mask.EditMask = Program.TextExit_Control_MaskType_Area;
                objAreaEditValue.Properties.Mask.UseMaskAsDisplayFormat = true;
                objAreaEditValue.Properties.Mask.MaskType = DevExpress.XtraEditors.Mask.MaskType.Numeric;
            }
            catch (Exception ex)
            {
                HandleError(ex.TargetSite.Name, ex.Message, ErroType.EXCEPTION, true, false);
                return;
            }
        }

        /// <versions>21-11-2017(GesAMFC-v0.0.4.35)</versions>
        public static String SetAreaDoubleStringValue(Double dAreaValue)
        {
            try
            {
                String sAreaValue = Default_Area_String;
                if (dAreaValue < 0)
                    return Default_Area_Double_String;
                sAreaValue = String.Format(FormatString_Double2_DecimalPlaces, dAreaValue);
                return sAreaValue;
            }
            catch (Exception ex)
            {
                HandleError(ex.TargetSite.Name, ex.Message, ErroType.EXCEPTION, true, false);
                return Default_Pay_String;
            }
        }
        
        #endregion  Areas Methods

        #region     Text Strings Methods

        /// <versions>21-11-2017(GesAMFC-v0.0.4.35)</versions
        public static Boolean IsValidTextString(String sTextString)
        {
            try
            {
                return !String.IsNullOrEmpty(sTextString.Trim()) && sTextString.Trim().ToLower() != DB_Not_Available.Trim().ToLower();
            }
            catch (Exception ex)
            {
                HandleError(ex.TargetSite.Name, ex.Message, ErroType.EXCEPTION, true, false);
                return false;
            }
        }

        public enum DefaultStringTextTypes { EMPTY = 1, DEFAULT = 2 }

        public static String SetTextString(String sTextString)
        {
            return Set_Text_String(sTextString, DefaultStringTextTypes.EMPTY);
        }

        /// <versions>21-11-2017(GesAMFC-v0.0.4.35)</versions
        public static String SetTextString(String sTextString, DefaultStringTextTypes eDefaultStringTextType)
        {
            return Set_Text_String(sTextString, eDefaultStringTextType);
        }

        public static String Set_Text_String(String sTextString, DefaultStringTextTypes eDefaultStringTextType)
        {
            try
            {
                String sDefaultStringTextType = eDefaultStringTextType == DefaultStringTextTypes.DEFAULT ? DB_Not_Available.ToLower() : String.Empty;
                return IsValidTextString(sTextString.Trim()) ? sTextString.Trim() : sDefaultStringTextType;
            }
            catch (Exception ex)
            {
                HandleError(ex.TargetSite.Name, ex.Message, ErroType.EXCEPTION, true, false);
                return sTextString;
            }
        }

        #endregion  Text Strings Methods
        
        #region     Handle Errors

        /// <versions>28-04-2017(v0.0.2.48)</versions>
        public static void HandleError(String sMethod, String sErrorMsg, ErroType eErrorType, Boolean bShowPopUp, Boolean bReloadGrid)
        {
            try
            {
                Program.WriteLog(sMethod + " ERROR", sErrorMsg, true, true, true, true);
                if (bShowPopUp)
                    MessageBox.Show(sErrorMsg, Enum.GetName(typeof(ErroType), eErrorType), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch { }
        }

        /// <versions>28-04-2017(v0.0.2.48)</versions>
        public static Boolean CheckIfIsNullOrEmpty(String sEntityName, String sFieldName, String sFieldValue)
        {
            try
            {
                if (String.IsNullOrEmpty(sFieldValue))
                {
                    StackFrame objStackFrame = new StackFrame();
                    String sErrorMsg = sEntityName + " " + sFieldName + " id null or empty!";
                    HandleError(objStackFrame.GetMethod().Name, sErrorMsg, ErroType.ERROR, true, true);
                    objStackFrame = null;
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                HandleError(ex.TargetSite.Name, ex.Message, ErroType.EXCEPTION, true, false);
                return false;
            }
        }

        #endregion  Handle Errors

        #region     JOIA Methods

        /// <versions>27-11-2017(GesAMFC-v0.0.4.38)</versions>
        public static Double Get_Current_Parameter_JOIA_Value()
        {
            try
            {
                return Program.SetPayCurrencyEuroDoubleValue(LibAMFC.DBF_AMFC_JOIA_VALOR);
            }
            catch (Exception ex)
            {
                HandleError(ex.TargetSite.Name, ex.Message, ErroType.EXCEPTION, true, false);
                return -1;
            }
        }

        #endregion  JOIA Methods

        #region     QUOTAS Methods

        /// <versions>27-11-2017(GesAMFC-v0.0.4.38)</versions>
        public static Double Get_Current_Parameter_QUOTA_Valor_Mes()
        {
            try
            {
                return Program.SetPayCurrencyEuroDoubleValue(LibAMFC.DBF_AMFC_QUOTA_VALOR_MES);
            }
            catch (Exception ex)
            {
                HandleError(ex.TargetSite.Name, ex.Message, ErroType.EXCEPTION, true, false);
                return -1;
            }
        }

        /// <versions>27-11-2017(GesAMFC-v0.0.4.38)</versions>
        public static Double Get_Current_Parameter_QUOTA_Valor_Ano()
        {
            try
            {
                return Program.SetPayCurrencyEuroDoubleValue(LibAMFC.DBF_AMFC_QUOTA_VALOR_ANO);
            }
            catch (Exception ex)
            {
                HandleError(ex.TargetSite.Name, ex.Message, ErroType.EXCEPTION, true, false);
                return -1;
            }
        }


        #endregion  QUOTAS Methods

        #region     INFRA Methods


        /// <versions>27-11-2017(GesAMFC-v0.0.4.38)</versions>
        public static Double Get_Current_Parameter_INFRA_Valor_Metro()
        {
            try
            {
                return Program.SetPayCurrencyEuroDoubleValue(LibAMFC.DBF_AMFC_INFRA_VALOR_METRO);
            }
            catch (Exception ex)
            {
                HandleError(ex.TargetSite.Name, ex.Message, ErroType.EXCEPTION, true, false);
                return -1;
            }
        }

        #endregion  INFRA Methods

        #region     CEDENC Methods

        /// <versions>27-11-2017(GesAMFC-v0.0.4.38)</versions>
        public static Double Get_Current_Parameter_CEDENC_Valor_Metro()
        {
            try
            {
                return Program.SetPayCurrencyEuroDoubleValue(LibAMFC.DBF_AMFC_CEDENC_VALOR_METRO);
            }
            catch (Exception ex)
            {
                HandleError(ex.TargetSite.Name, ex.Message, ErroType.EXCEPTION, true, false);
                return -1;
            }
        }

        #endregion  CEDENC Methods

        #region     ESGOT Methods

        /// <versions>27-11-2017(GesAMFC-v0.0.4.38)</versions>
        public static Double Get_Current_Parameter_ESGOT_Valor_Metro()
        {
            try
            {
                return Program.SetPayCurrencyEuroDoubleValue(LibAMFC.DBF_AMFC_ESGOT_VALOR_METRO);
            }
            catch (Exception ex)
            {
                HandleError(ex.TargetSite.Name, ex.Message, ErroType.EXCEPTION, true, false);
                return -1;
            }
        }

        #endregion  ESGOT Methods

        #region     RECONV Methods

        /// <versions>27-11-2017(GesAMFC-v0.0.4.38)</versions>
        public static Double Get_Current_Parameter_RECON_Valor_Metro()
        {
            try
            {
                return Program.SetPayCurrencyEuroDoubleValue(LibAMFC.DBF_AMFC_RECONV_VALOR_METRO);
            }
            catch (Exception ex)
            {
                HandleError(ex.TargetSite.Name, ex.Message, ErroType.EXCEPTION, true, false);
                return -1;
            }
        }


        #endregion  RECONV Methods      

        #region     Payment Methods



        #endregion  Payment Methods

        /// <versions>02-12-2017(GesAMFC-v0.0.4.41)</versions>
        public static String GetEntityDbfFileName(AMFC_Entidade_Tipo eEntityType)
        {
            try
            {
                String sDBF_File_Name = String.Empty;
                switch (eEntityType)
                {
                    case AMFC_Entidade_Tipo.QUOTAS:
                        sDBF_File_Name = LibAMFC.DBF_AMFC_QUOTA_FileName;                           
                        break;
                    case AMFC_Entidade_Tipo.INFRAEST:
                        sDBF_File_Name = LibAMFC.DBF_AMFC_INFRA_FileName;
                        break;
                    case AMFC_Entidade_Tipo.CEDENCIAS:
                        sDBF_File_Name = LibAMFC.DBF_AMFC_CEDEN_FileName;
                        break;
                    default:
                        return String.Empty;
                }
                return sDBF_File_Name;
            }
            catch (Exception ex)
            {
                HandleError(ex.TargetSite.Name, ex.Message, ErroType.EXCEPTION, true, false);
                return String.Empty;
            }
        }

        #region     Quantia Extenso

        // O método toExtenso recebe um valor do tipo decimal
        public static String QuantiaToExtenso(Double valor)
        {
            try
            {
                if (!Program.IsValidCurrencyEuroValue(valor))
                    return valor.ToString();
                else
                {
                    Int32 iParteInteiraMaxChars = 9;
                    String sValor = valor.ToString("000000000.00");
                    String sValorExtenso = String.Empty;

                    Int32 iIncrementLen     = 3;
                    Int32 iDecimalLen       = 2;
                    Int32 iIdxMillions      = 0;
                    Int32 iIdxThousands     = iParteInteiraMaxChars - 6;
                    Int32 iIdxUnits         = iParteInteiraMaxChars - 3;
                    Int32 iIdxDecimal       = iParteInteiraMaxChars;

                    String  sMillionsPart   = sValor.Substring(iIdxMillions, iIncrementLen);
                    Int64   lMillionsPart   = Convert.ToInt64(sMillionsPart);
                    Double  dMillionsPart   = Convert.ToDouble(sMillionsPart);
                    String  sThousandsPart  = sValor.Substring(iIdxThousands, iIncrementLen);
                    Int64   lThousandsPart  = Convert.ToInt64(sThousandsPart);
                    Double  dThousandsPart  = Convert.ToDouble(sThousandsPart);
                    String  sUnitsPart      = sValor.Substring(iIdxUnits, iIncrementLen);
                    Int64   lUnitsPart      = Convert.ToInt64(sUnitsPart);
                    Double  dUnitsPart      = Convert.ToDouble(sUnitsPart);
                    String  sDecimalPart    = sValor.Substring(iIdxDecimal+1, iDecimalLen);
                    Int64   lDecimalPart    = Convert.ToInt64(sDecimalPart);
                    Double  dDecimalPart    = Convert.ToDouble(sDecimalPart);

                    for (int i = 0; i <= iParteInteiraMaxChars; i += iIncrementLen)
                    {
                        String sPartValorExtenso = QuantiaToExtensoDecimal(Convert.ToDouble(sValor.Substring(i, 3)));

                        if (String.IsNullOrEmpty(sPartValorExtenso))
                            continue;

                        #region     Milhões
                        if (i == iIdxMillions & sPartValorExtenso != String.Empty)
                        {
                            if (lMillionsPart == 1)
                                sPartValorExtenso += " MILHÃO" + ((dMillionsPart > 0) ? " E " : String.Empty);
                            else if (lMillionsPart > 1)
                                sPartValorExtenso += " MILHÕES" + ((dMillionsPart > 0) ? " E " : String.Empty);
                        }
                        #endregion  Milhões


                        #region     Milhares
                        else if (i == iIdxThousands & sPartValorExtenso != String.Empty)
                        {
                            if (lThousandsPart > 0)
                            {
                                if (lThousandsPart == 1)
                                    sPartValorExtenso = String.Empty;
                                sPartValorExtenso += " MIL" + ((dThousandsPart > 0) ? " E " : String.Empty);
                            }
                            #endregion  Milhares
                        }

                        #region     Unidades
                        else if (i == iIdxUnits)
                        {
                            if (lUnitsPart == 1)
                                sPartValorExtenso += " EURO";
                            else if (lUnitsPart > 1)
                                sPartValorExtenso += " EUROS";
                            if (lDecimalPart > 0 && sPartValorExtenso != String.Empty)
                                sPartValorExtenso += " E ";
                        }
                        #endregion  Unidades

                        #region     Decimal
                        else if (i == iIdxDecimal)
                        {
                            if (lDecimalPart == 1)
                                sPartValorExtenso += " CÊNTIMO";
                            else if (lDecimalPart > 1)
                                sPartValorExtenso += " CÊNTIMOS";
                        }
                        #endregion  Decimal

                        if (!String.IsNullOrEmpty(sPartValorExtenso))
                            sValorExtenso += sPartValorExtenso;
                    }
                    if (String.IsNullOrEmpty(sValorExtenso))
                        return valor.ToString();
                    return sValorExtenso;
                }
            }
            catch (Exception ex)
            {
                HandleError(ex.TargetSite.Name, ex.Message, ErroType.EXCEPTION, true, false);
                return valor.ToString();
            }
}

        public static String QuantiaToExtensoDecimal(Double valor)
        {
            try
            {
                if (valor <= 0)
                    return String.Empty;
                else
                {
                    String sValorDecimal = String.Empty;
                    if (valor > 0 & valor < 1)
                        valor *= 100;
                    String strValor = valor.ToString("000");
                    int a = Convert.ToInt32(strValor.Substring(0, 1));
                    int b = Convert.ToInt32(strValor.Substring(1, 1));
                    int c = Convert.ToInt32(strValor.Substring(2, 1));

                    if (a == 1) sValorDecimal += (b + c == 0) ? "CEM" : "CENTO";
                    else if (a == 2) sValorDecimal += "DUZENTOS";
                    else if (a == 3) sValorDecimal += "TREZENTOS";
                    else if (a == 4) sValorDecimal += "QUATROCENTOS";
                    else if (a == 5) sValorDecimal += "QUINHENTOS";
                    else if (a == 6) sValorDecimal += "SEISCENTOS";
                    else if (a == 7) sValorDecimal += "SETECENTOS";
                    else if (a == 8) sValorDecimal += "OITOCENTOS";
                    else if (a == 9) sValorDecimal += "NOVECENTOS";

                    if (b == 1)
                    {
                        if (c == 0) sValorDecimal += ((a > 0) ? " E " : String.Empty) + "DEZ";
                        else if (c == 1) sValorDecimal += ((a > 0) ? " E " : String.Empty) + "ONZE";
                        else if (c == 2) sValorDecimal += ((a > 0) ? " E " : String.Empty) + "DOZE";
                        else if (c == 3) sValorDecimal += ((a > 0) ? " E " : String.Empty) + "TREZE";
                        else if (c == 4) sValorDecimal += ((a > 0) ? " E " : String.Empty) + "QUATORZE";
                        else if (c == 5) sValorDecimal += ((a > 0) ? " E " : String.Empty) + "QUINZE";
                        else if (c == 6) sValorDecimal += ((a > 0) ? " E " : String.Empty) + "DEZESSEIS";
                        else if (c == 7) sValorDecimal += ((a > 0) ? " E " : String.Empty) + "DEZESSETE";
                        else if (c == 8) sValorDecimal += ((a > 0) ? " E " : String.Empty) + "DEZOITO";
                        else if (c == 9) sValorDecimal += ((a > 0) ? " E " : String.Empty) + "DEZENOVE";
                    }
                    else if (b == 2) sValorDecimal += ((a > 0) ? " E " : String.Empty) + "VINTE";
                    else if (b == 3) sValorDecimal += ((a > 0) ? " E " : String.Empty) + "TRINTA";
                    else if (b == 4) sValorDecimal += ((a > 0) ? " E " : String.Empty) + "QUARENTA";
                    else if (b == 5) sValorDecimal += ((a > 0) ? " E " : String.Empty) + "CINQUENTA";
                    else if (b == 6) sValorDecimal += ((a > 0) ? " E " : String.Empty) + "SESSENTA";
                    else if (b == 7) sValorDecimal += ((a > 0) ? " E " : String.Empty) + "SETENTA";
                    else if (b == 8) sValorDecimal += ((a > 0) ? " E " : String.Empty) + "OITENTA";
                    else if (b == 9) sValorDecimal += ((a > 0) ? " E " : String.Empty) + "NOVENTA";

                    if (strValor.Substring(1, 1) != "1" & c != 0 & sValorDecimal != String.Empty) sValorDecimal += " E ";

                    if (strValor.Substring(1, 1) != "1")
                        if (c == 1) sValorDecimal += "UM";
                        else if (c == 2) sValorDecimal += "DOIS";
                        else if (c == 3) sValorDecimal += "TRÊS";
                        else if (c == 4) sValorDecimal += "QUATRO";
                        else if (c == 5) sValorDecimal += "CINCO";
                        else if (c == 6) sValorDecimal += "SEIS";
                        else if (c == 7) sValorDecimal += "SETE";
                        else if (c == 8) sValorDecimal += "OITO";
                        else if (c == 9) sValorDecimal += "NOVE";

                    return sValorDecimal;
                }
            }
            catch (Exception ex)
            {
                HandleError(ex.TargetSite.Name, ex.Message, ErroType.EXCEPTION, true, false);
                return String.Empty;
            }
}
        #endregion  Quantia Extenso

        /// <versions>07-12-2017(GesAMFC-v0.0.5.1)</versions>
        public static String EncodeStringToUTF8(String sText)
        {
            try
            {
                byte[] rawData = Encoding.Default.GetBytes(sText);
                String sUtf8Reencoded = Encoding.UTF8.GetString(rawData);
                return sUtf8Reencoded;
            }
            catch (Exception ex)
            {
                HandleError(ex.TargetSite.Name, ex.Message, ErroType.EXCEPTION, true, false);
                return sText;
            }
        }

        public static String EncodeStringToISO(String sText)
        {
            try
            {
                byte[] rawData = Encoding.Default.GetBytes(sText);
                String sUtf8Reencoded = Encoding.GetEncoding("iso-8859-1").GetString(rawData);
                //String sUtf8Reencoded = Encoding.GetEncoding("iso-8859-9").GetString(rawData);
                //String sUtf8Reencoded = Encoding.GetEncoding("iso-8859-15").GetString(rawData);
                sUtf8Reencoded = sUtf8Reencoded.Replace("¦", "Ã");
                return sUtf8Reencoded;
            }
            catch (Exception ex)
            {
                HandleError(ex.TargetSite.Name, ex.Message, ErroType.EXCEPTION, true, false);
                return sText;
            }
        }
    }
}


