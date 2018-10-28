using DevExpress.Utils;
using DevExpress.XtraEditors;
using DevExpress.XtraEditors.Repository;
using DevExpress.XtraGrid;
using DevExpress.XtraGrid.Columns;
using DevExpress.XtraGrid.Views.Base;
using DevExpress.XtraGrid.Views.Grid;
using DevExpress.XtraGrid.Views.Layout;
using GesAMFC.AMFC_Methods;
using MMI.Libraries;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;

/// <summary>AMFC Admin Library</summary>
/// <author>Valter Lima</author>
/// <creation>18-02-2017(v0.0.1.0)</creation>
/// <versions>02-12-2017(GesAMFC-v0.0.4.41)</versions>
namespace GesAMFC
{
    /// <versions>14-03-2017(v0.0.2.6)</versions>
    public class ApplicationUser
    {
        public enum AMFC_UserTypes { ADMIN = 1, USER = 2 }

        public AMFC_UserTypes UserType { get; set; }

        #region     User Details
        public String Password { get; set; }
        #endregion  User Details

        #region     User Permissions
        public Boolean CanAdd   { get; set; }
        public Boolean CanEdit  { get; set; }
        public Boolean CanDel   { get; set; }
        #endregion  User Permissions
        
        #region     Constructors
        /// <versions>14-03-2017(v0.0.2.6)</versions>
        public ApplicationUser()
        {
            UserType    = AMFC_UserTypes.USER;
            CanAdd      = CanEdit = CanDel = false;
            Password    = String.Empty;
        }
        /// <versions>14-03-2017(v0.0.2.6)</versions>
        public ApplicationUser(AMFC_UserTypes eUserType)
        {
            UserType = eUserType;
            switch (UserType)
            {
                case AMFC_UserTypes.USER:
                    CanAdd = CanEdit = CanDel = false;
                    break;
                case AMFC_UserTypes.ADMIN:
                    CanAdd = CanEdit = CanDel = true;
                    break;
            }
            Password = String.Empty;
        }
        #endregion  Constructors
    }

    #region     Period Dates

    /// <versions>02-12-2017(GesAMFC-v0.0.4.41)</versions>
    public class ComboboxItem
    {
        public object Value { get; set; }
        public string Text { get; set; }

        public ComboboxItem(object objValue, String sText)
        {
            Value = objValue;
            Text = sText;
        }

        public Int32 GetValue()
        {
            return (Int32)Value;
        }

        public String GetText()
        {
            return Text;
        }

        public override String ToString()
        {
            return Text;
        }
    }

    #region     Periods

    public class AMFCPeriod
    {
        public AMFCPeriodYearMonth Start    { get; set; }
        public AMFCPeriodYearMonth End      { get; set; }

        public AMFCPeriod()
        {
            Start   = new AMFCPeriodYearMonth();
            End     = new AMFCPeriodYearMonth();
        }
    }

    public class AMFCPeriodYearMonth
    {
        public AMFCYear     Year    { get; set; }
        public AMFCMonth    Month   { get; set; }

        public AMFCPeriodYearMonth()
        {
            Year    = new AMFCYear();
            Month   = new AMFCMonth();
        }
    }

    public class AMFCPeriodYears
    {
        public AMFCPeriodYear Start { get; set; }
        public AMFCPeriodYear End { get; set; }

        public AMFCPeriodYears()
        {
            Start = new AMFCPeriodYear();
            End = new AMFCPeriodYear();
        }
    }

    public class AMFCPeriodYear
    {
        public AMFCYear Year { get; set; }

        public AMFCPeriodYear()
        {
            Year = new AMFCYear();
        }
    }

    #endregion  Periods

    #region     List Years

    /// <versions>02-12-2017(GesAMFC-v0.0.4.41)</versions>
    public class AMFCYears
    {
        public List<AMFCYear> List { get; set; }

        public AMFCYears()
        {
            List = new List<AMFCYear>();
        }

        /// <versions>02-12-2017(GesAMFC-v0.0.4.41)</versions>
        public Boolean Add(AMFCYear objAMFCYear)
        {
            try
            {
                if (Contains(objAMFCYear))
                    return false;
                List.Add(objAMFCYear);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <versions>02-12-2017(GesAMFC-v0.0.4.41)</versions>
        public Boolean Clear()
        {
            try
            {
                List.Clear();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <versions>02-12-2017(GesAMFC-v0.0.4.41)</versions>
        public Boolean Del(Int32 iYear)
        {
            try
            {
                List<AMFCYear> newList = new List<AMFCYear>();
                foreach (AMFCYear objAMFCYear in List)
                {
                    if (objAMFCYear.Value != iYear)
                    {
                        AMFCYear checkAMFCYear = new AMFCYear(iYear);
                        if (!ListContains(newList, checkAMFCYear))
                        {
                            AMFCYear newAMFCYear = GetItemByValue(iYear);
                            newList.Add(newAMFCYear);
                        }
                    }
                }
                List.Clear();
                List = newList;
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <versions>02-12-2017(GesAMFC-v0.0.4.41)</versions>
        public Boolean Contains(AMFCYear objYear)
        {
            try
            {
                return ListContains(List, objYear);
            }
            catch
            {
                return false;
            }
        }

        /// <versions>02-12-2017(GesAMFC-v0.0.4.41)</versions>
        private Boolean ListContains(List<AMFCYear> objListYears, AMFCYear objYear)
        {
            try
            {
                foreach (AMFCYear objAMFCYear in objListYears)
                {
                    if (objAMFCYear.Value == objYear.Value)
                        return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        /// <versions>02-12-2017(GesAMFC-v0.0.4.41)</versions>
        public Boolean ValidYearItem(AMFCYear objAMFCYear)
        {
            try
            {
                return (Program.IsValidYear(objAMFCYear.Value) && !String.IsNullOrEmpty(objAMFCYear.Description));
            }
            catch
            {
                return false;
            }
        }

        /// <versions>02-12-2017(GesAMFC-v0.0.4.41)</versions>
        public AMFCYear GetItemByDesc(String sYear)
        {
            try
            {
                return Get_Item_By_Desc(List, sYear);
            }
            catch
            {
                return null;
            }
        }

        /// <versions>02-12-2017(GesAMFC-v0.0.4.41)</versions>
        public AMFCYear Get_Item_By_Desc(List<AMFCYear> objListYears, String sYear)
        {
            try
            {
                AMFCYear foundAMFCYear = new AMFCYear();
                foreach (AMFCYear objAMFCYear in objListYears)
                {
                    if (objAMFCYear.Description == sYear)
                    {
                        foundAMFCYear = new AMFCYear(objAMFCYear.Value, objAMFCYear.Description);
                        return foundAMFCYear;
                    }
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        /// <versions>02-12-2017(GesAMFC-v0.0.4.41)</versions>
        public AMFCYear GetItemByValue(Int32 iYear)
        {
            try
            {
                return Get_Item_By_Value(List, iYear);
            }
            catch
            {
                return null;
            }
        }

        /// <versions>02-12-2017(GesAMFC-v0.0.4.41)</versions>
        public AMFCYear Get_Item_By_Value(List<AMFCYear> objListYears, Int32 iYear)
        {
            try
            {
                AMFCYear foundAMFCYear = new AMFCYear();
                foreach (AMFCYear objAMFCYear in objListYears)
                {
                    if (objAMFCYear.Value == iYear)
                    {
                        foundAMFCYear = new AMFCYear(objAMFCYear.Value, objAMFCYear.Description);
                        return foundAMFCYear;
                    }
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        /// <versions>02-12-2017(GesAMFC-v0.0.4.41)</versions>
        public Boolean SetYearList()
        {
            try
            {
                this.List.Clear();
                for (Int32 iYear = Program.DB_Min_Valid_Date.Year; iYear <= Program.DB_Max_Valid_Date.Year; iYear++)
                {
                    AMFCYear objAMFCYear = new AMFCYear(iYear);
                    Add(objAMFCYear);
                }
                return true;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return false;
            }
        }

        /// <versions>02-12-2017(GesAMFC-v0.0.4.41)</versions>
        public Boolean SetYearList(Int32 iYearMin, Int32 iYearMax)
        {
            try
            {
                if (iYearMin < Program.DB_Min_Valid_Date.Year || iYearMax > Program.DB_Max_Valid_Date.Year)
                    return false;
                this.List.Clear();
                for (Int32 iYear = iYearMin; iYear <= iYearMax; iYear++)
                {
                    AMFCYear objAMFCYear = new AMFCYear(iYear);
                    Add(objAMFCYear);
                }
                return true;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return false;
            }
        }
    }

    /// <versions>02-12-2017(GesAMFC-v0.0.4.41)</versions>
    public class AMFCYear
    {
        #region     Fields
        private Int32   _Value = -1;
        private String  _Description = String.Empty;
        #endregion  Fields

        #region     Properties

        /// <versions>02-12-2017(GesAMFC-v0.0.4.41)</versions>
        public Int32 Value
        {
            get
            {
                return _Value;
            }
            set
            {
                if (!Program.IsValidYear(value))
                    return;
                _Value = value;
                _Description = _Value.ToString();
            }
        }

        /// <versions>02-12-2017(GesAMFC-v0.0.4.41)</versions>
        public String Description
        {
            get
            {
                return _Description;
            }
            set
            {
                if (String.IsNullOrEmpty(value))
                    return;
                _Description = value;
                if (!Program.IsValidYear(_Value))
                    _Value = Convert.ToInt32(_Description.ToString());
            }
        }

        #endregion  Properties

        #region     Constructors

        /// <versions>02-12-2017(GesAMFC-v0.0.4.41)</versions>
        public AMFCYear()
        {
            Value = -1;
            Description = String.Empty;
        }

        /// <versions>02-12-2017(GesAMFC-v0.0.4.41)</versions>
        public AMFCYear(Int32 iValue)
        {
            Value = iValue;
        }

        /// <versions>02-12-2017(GesAMFC-v0.0.4.41)</versions>
        public AMFCYear(Int32 iValue, String sDescription)
        {
            Value = iValue;
            Description = sDescription;
        }

        #endregion  Constructors
    }

    #endregion  List Years

    #region     List Months

    /// <versions>02-12-2017(GesAMFC-v0.0.4.41)</versions>
    public class AMFCMonths
    {
        public List<AMFCMonth> List { get; set; }

        public AMFCMonths()
        {
            List = new List<AMFCMonth>();
        }

        /// <versions>02-12-2017(GesAMFC-v0.0.4.41)</versions>
        public Boolean Add(AMFCMonth objAMFCMonth)
        {
            try
            {
                if (Contains(objAMFCMonth))
                    return false;
                List.Add(objAMFCMonth);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <versions>02-12-2017(GesAMFC-v0.0.4.41)</versions>
        public Boolean Clear()
        {
            try
            {
                List.Clear();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <versions>02-12-2017(GesAMFC-v0.0.4.41)</versions>
        public Boolean Del(Int32 iMonth)
        {
            try
            {
                List<AMFCMonth> newList = new List<AMFCMonth>();
                foreach (AMFCMonth objAMFCMonth in List)
                {
                    if (objAMFCMonth.Value != iMonth)
                    {
                        AMFCMonth checkAMFCMonth = new AMFCMonth(iMonth);
                        if (!ListContains(newList, checkAMFCMonth))
                        {
                            AMFCMonth newAMFCMonth = GetItemByValue(iMonth);
                            newList.Add(newAMFCMonth);
                        }
                    }
                }
                List.Clear();
                List = newList;
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <versions>02-12-2017(GesAMFC-v0.0.4.41)</versions>
        public Boolean Contains(AMFCMonth objMonth)
        {
            try
            {
                return ListContains(List, objMonth);
            }
            catch
            {
                return false;
            }
        }

        /// <versions>02-12-2017(GesAMFC-v0.0.4.41)</versions>
        private Boolean ListContains(List<AMFCMonth> objListMonths, AMFCMonth objMonth)
        {
            try
            {
                foreach (AMFCMonth objAMFCMonth in objListMonths)
                {
                    if (objAMFCMonth.Value == objMonth.Value)
                        return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        /// <versions>02-12-2017(GesAMFC-v0.0.4.41)</versions>
        public Boolean ValidMonthItem(AMFCMonth objAMFCMonth)
        {
            try
            {
                return (Program.IsValidMonth(objAMFCMonth.Value) && !String.IsNullOrEmpty(objAMFCMonth.Description));
            }
            catch
            {
                return false;
            }
        }

        /// <versions>02-12-2017(GesAMFC-v0.0.4.41)</versions>
        public AMFCMonth GetItemByDesc(String sMonth)
        {
            try
            {
                return Get_Item_By_Desc(List, sMonth);
            }
            catch
            {
                return null;
            }
        }

        /// <versions>02-12-2017(GesAMFC-v0.0.4.41)</versions>
        public AMFCMonth Get_Item_By_Desc(List<AMFCMonth> objListMonths, String sMonth)
        {
            try
            {
                AMFCMonth foundAMFCMonth = new AMFCMonth();
                foreach (AMFCMonth objAMFCMonth in objListMonths)
                {
                    if (objAMFCMonth.Description == sMonth)
                    {
                        foundAMFCMonth = new AMFCMonth(objAMFCMonth.Value, objAMFCMonth.Description);
                        return foundAMFCMonth;
                    }
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        /// <versions>02-12-2017(GesAMFC-v0.0.4.41)</versions>
        public AMFCMonth GetItemByValue(Int32 iMonth)
        {
            try
            {
                return Get_Item_By_Value(List, iMonth);
            }
            catch
            {
                return null;
            }
        }

        /// <versions>02-12-2017(GesAMFC-v0.0.4.41)</versions>
        public AMFCMonth Get_Item_By_Value(List<AMFCMonth> objListMonths, Int32 iMonth)
        {
            try
            {
                AMFCMonth foundAMFCMonth = new AMFCMonth();
                foreach (AMFCMonth objAMFCMonth in objListMonths)
                {
                    if (objAMFCMonth.Value == iMonth)
                    {
                        foundAMFCMonth = new AMFCMonth(objAMFCMonth.Value, objAMFCMonth.Description);
                        return foundAMFCMonth;
                    }
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        /// <versions>02-12-2017(GesAMFC-v0.0.4.41)</versions>
        public Boolean SetMonthList()
        {
            try
            {
                this.List.Clear();
                for (Int32 iMonth = Program.DB_Min_Valid_Date.Month ; iMonth <= Program.DB_Max_Valid_Date.Month; iMonth++)
                {
                    AMFCMonth objAMFCMonth = new AMFCMonth(iMonth);
                    Add(objAMFCMonth);
                }
                return true;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return false;
            }
        }
    }

    /// <versions>02-12-2017(GesAMFC-v0.0.4.41)</versions>
    public class AMFCMonth
    {
        #region     Fields
        private Int32 _Value = -1;
        private String _Description = String.Empty;
        #endregion  Fields

        #region     Properties

        public Int32 Value
        {
            get
            {
                return _Value;
            }
            set
            {
                if (!Program.IsValidMonth(value))
                    return;
                _Value = value;
                _Description = new DateTime(DateTime.Today.Year, _Value, 1).ToString("MMMM", Program.CurrentCulture);
            }
        }

        public String Description
        {
            get
            {
                return _Description;
            }
            set
            {
                if (String.IsNullOrEmpty(value))
                    return;
                _Description = value;
                if (!Program.IsValidMonth(_Value))
                    _Value = Convert.ToInt32(_Description.ToString());
            }
        }

        #endregion  Properties

        #region     Constructors

        public AMFCMonth()
        {
            Value = -1;
            Description = String.Empty;
        }

        public AMFCMonth(Int32 iValue)
        {
            Value = iValue;
        }

        public AMFCMonth(Int32 iValue, String sDescription)
        {
            Value = iValue;
            Description = sDescription;
        }

        #endregion  Constructors
    }

    #endregion  List Months

    #endregion  Period Dates

    #region     Members

    /// <versions>22-03-2017(v0.0.1.14)</versions>
    public class AMFCMembers
    {
        public List<AMFCMember> Members { get; set; }
        public AMFCMember SelectedMember { get; set; }
        public AMFCMembers()
        {
            Members = new List<AMFCMember>();
            SelectedMember = new AMFCMember();
        }
        public AMFCMember GetMemberByNumber(Int64 lMemberId)
        {
            try
            {
                if (Members == null)
                    return null;
                if (lMemberId == SelectedMember.NUMERO)
                    return SelectedMember;
                foreach (AMFCMember objMember in Members)
                {
                    if (objMember.NUMERO == lMemberId)
                        return objMember;
                }
                return null;
            }
            catch
            {
                return null;
            }
        }
    }

    /// <versions>17-03-2018(GesAMFC-v1.0.0.3)</versions>
    public class AMFCMember
    {
        public Int32 MinNumber = 1;
        public Int32 MaxNumber = 99999;

        #region     DBF Member Fields
        public enum DBFMemberFields
        {
            NUMERO      = 1,
            NOME        = 2,
            MORADA      = 3,
            CPOSTAL     = 4,
            TELEFONE    = 5,
            TELEMOVEL   = 6,
            EMAIL       = 7,
            NUMLOTE     = 8,
            MORLOTE     = 9,
            AREALOTE    = 10,
            PROFISSAO   = 11,
            SECTOR      = 12,
            CASA        = 13,
            GARAGEM     = 14,
            MUROS       = 15,
            POCO        = 16,
            FURO        = 17,
            SANEAMENTO  = 18,
            ELECTRICID  = 19,
            PROJECTO    = 20,
            ESCRITURA   = 21,
            FINANCAS    = 22,
            RESIDENCIA  = 23,
            AGREFAMIL   = 24,
            NUMFILHOS   = 25,
            GAVETO      = 26,
            QUINTINHA   = 27,
            LADOMAIOR   = 28,
            MAIS1FOGO   = 29,
            HABCOLECT   = 30,
            NUMFOGOS    = 31,
            ARECOMERC   = 32,
            DATAADMI    = 33,
            OBSERVACAO  = 34,
            CC          = 35,
            NIF         = 36
        }
        #endregion  DBF Member Fields

        #region     DBF Member Fields Alias
        public List<String> DBFMemberFieldsAlias = new List<String>()
        {
            "Nº Sócio",             //= 1,
            "Nome",                 //= 2,
            "Morada",               //= 3,
            "Cód.Postal",           //= 4,
            "Telefone",             //= 5,
            "Telemóvel",            //= 6,
            "e-mail",               //= 7,
            "Nº Lote",              //= 8,
            "Morada Lote",          //= 9,
            "Área Lote",            //= 10,
            "Profissão",            //= 11,
            "Setor",                //= 12,
            "Casa",                 //= 13,
            "Garagem",              //= 14,
            "Muros",                //= 15,
            "Poço",                 //= 16,
            "Furo",                 //= 17,
            "Sanemaento",           //= 18,
            "Eletricidade",         //= 19,
            "Projeto",              //= 20,
            "Escritura",            //= 21,
            "Finanças",             //= 22,
            "Residência",           //= 23,
            "Agreagado Familiar",   //= 24,
            "Nº Filhos",            //= 25,
            "Gaveto",               //= 26,
            "Quintilha",            //= 27,
            "Lado Maior",           //= 28,
            "Mais Um fogo",         //= 29,
            "Habitação Coletiva",   //= 30,
            "Nº Fogos",             //= 31,
            "Área Comercial",       //= 32,
            "Data Admissão",        //= 33,
            "Observações",          //= 34,
            "CC",                   //= 35,
            "NIF"                   //= 36
        };
        #endregion  DBF Member Fields Alias

        #region     Member Properties
        public Int64    NUMERO      { get; set; }
        public String   NOME        { get; set; }
        public String   MORADA      { get; set; }
        public String   CPOSTAL     { get; set; }       
        public String   TELEFONE    { get; set; }
        public String   TELEMOVEL   { get; set; }
        public String   EMAIL       { get; set; }
        public String   MORLOTE     { get; set; }
        public String   NUMLOTE     { get; set; }
        public Int32    AREALOTE    { get; set; }
        public String   PROFISSAO   { get; set; }
        public String   DATAADMI    { get; set; }
        public String   SECTOR      { get; set; }
        public String   NUMFOGOS    { get; set; }
        public String   NUMFILHOS   { get; set; }
        public String   AGREFAMIL   { get; set; }
        public String   LADOMAIOR   { get; set; }
        public Boolean  CASA        { get; set; }
        public Boolean  GARAGEM     { get; set; }
        public Boolean  MUROS       { get; set; }
        public Boolean  POCO        { get; set; }
        public Boolean  FURO        { get; set; }
        public Boolean  SANEAMENTO  { get; set; }
        public Boolean  ELECTRICID  { get; set; }
        public Boolean  PROJECTO    { get; set; }
        public Boolean  ESCRITURA   { get; set; }
        public Boolean  FINANCAS    { get; set; }
        public Boolean  RESIDENCIA  { get; set; }
        public Boolean  GAVETO      { get; set; }
        public Boolean  QUINTINHA   { get; set; }
        public Boolean  MAIS1FOGO   { get; set; }
        public Boolean  HABCOLECT   { get; set; }
        public Boolean  ARECOMERC   { get; set; }
        public String   OBSERVACAO  { get; set; }
        public String   CC          { get; set; }
        public String   NIF         { get; set; }
        #endregion  Member Properties

        #region     Constructor

        public AMFCMember()
        {
            Set_Empty_Member();

            //SocioQuotas = new AMFCMemberQuotas();
        }

        public AMFCMember(Int64 lNumber)
        {
            Set_Empty_Member();
            NUMERO = lNumber;
        }

        private void Set_Empty_Member()
        {
            #region     Member Properties 
            NUMERO = -1;
            NOME = String.Empty;
            MORADA = String.Empty;
            CPOSTAL = String.Empty;
            TELEFONE = String.Empty;
            TELEMOVEL = String.Empty;
            EMAIL = String.Empty;
            NUMLOTE = String.Empty;
            MORLOTE = String.Empty;
            AREALOTE = -1;
            PROFISSAO = String.Empty;
            DATAADMI = Program.Default_Date_Str;
            OBSERVACAO = String.Empty;
            SECTOR = String.Empty;
            NUMFOGOS = String.Empty;
            NUMFILHOS = String.Empty;
            AGREFAMIL = String.Empty;
            LADOMAIOR = String.Empty;
            CASA = false;
            GARAGEM = false;
            MUROS = false;
            POCO = false;
            FURO = false;
            SANEAMENTO = false;
            ELECTRICID = false;
            PROJECTO = false;
            ESCRITURA = false;
            FINANCAS = false;
            RESIDENCIA = false;
            GAVETO = false;
            QUINTINHA = false;
            MAIS1FOGO = false;
            HABCOLECT = false;
            ARECOMERC = false;
            CC = String.Empty;
            NIF = String.Empty;
            #endregion  Member Properties
        }

        #endregion  Constructor

        #region     Methods
        public Int32 Get_DBFMemberField_Id_ByType(DBFMemberFields eField)
        {
            try
            {
                return Convert.ToInt32(eField);
            }
            catch
            {
                return -1;
            }
        }
        public Int32 Get_DBFMemberField_Idx_ByType(DBFMemberFields eField)
        {
            try
            {
                Int32 iId = Get_DBFMemberField_Id_ByType(eField);
                if (iId < 1)
                    return -1;
                return iId - 1;
            }
            catch
            {
                return -1;
            }
        }
        public String Get_DBFMemberField_Name_ByType(DBFMemberFields eField)
        {
            try
            {
                return Enum.GetName(typeof(DBFMemberFields), eField);
            }
            catch
            {
                return String.Empty;
            }
        }
        public String Get_DBFMemberField_Alias_ByType(DBFMemberFields eField)
        {
            try
            {
                Int32 iIdx = Get_DBFMemberField_Idx_ByType(eField);
                if (iIdx < 0 || iIdx >= DBFMemberFieldsAlias.Count)
                    return String.Empty;
                return DBFMemberFieldsAlias[iIdx];
            }
            catch
            {
                return String.Empty;
            }
        }
        #endregion  Methods
    }

    #endregion  Members

    #region     Members Lotes

    /// <versions>28-02-2018(GesAMFC-v1.0.0.3)</versions>
    public class AMFCMemberLotes
    {
        public List<AMFCMemberLote> Lotes { get; set; }
        public AMFCMember Member { get; set; }
        public AMFCMemberLotes()
        {
            Lotes = new List<AMFCMemberLote>();
            Member = new AMFCMember();
        }

        public AMFCMemberLote GetLoteById(Int64 lLoteId)
        {
            try
            {
                if (lLoteId < 1 || Lotes == null)
                    return null;
                foreach (AMFCMemberLote objLote in Lotes)
                {
                    if (objLote.IDLOTE == lLoteId)
                        return objLote;
                }
                return null;
            }
            catch
            {
                return null;
            }
        }
    }

    /// <remarks>
    /// IDLOTE,N,5,0	SOCNUM,N,5,0	SOCNOME,C,70	OLDNUM,N,5,0	OLDNOME,C,140	MORLOTE,C,140	INDEXLOTE,N,1,0	NUMLOTE,C,10	TOTALLOTES,N,2,0	TOTALFOGOS,N,2,0	AREALOTES,N,5,0	AREAPAGAR,N,5,0	SECTOR,C,6	CASA,C,1	GARAGEM,C,1	MUROS,C,1	POCO,C,1	FURO,C,1	SANEAMENTO,C,1	ELECTRICID,C,1	PROJECTO,C,1	ESCRITURA,C,1	FINANCAS,C,1	RESIDENCIA,C,1	AGREFAMIL,C,2	GAVETO,C,1	QUINTINHA,C,1	LADOMAIOR,C,4	MAIS1FOGO,C,1	HABCOLECT,C,1, ARECOMERC,C,5	OBSERVACAO,C,140	OBSERV2,C,140	OBSERV3,C,140	FICH01,C,140	FICH02,C,140	FICH03,C,140	PROCURADOR,C,140	MORPROC,C,140
    /// </remarks>
    /// <versions>28-02-2018(GesAMFC-v1.0.0.3)</versions>
    public class AMFCMemberLote
    {
        #region     Properties
        public Int64 IDLOTE { get; set; }
        public Int64 SOCNUM { get; set; }
        public String SOCNOME { get; set; }
        public Int64 OLDNUM { get; set; }
        public String OLDNOME { get; set; }
        public String MORLOTE { get; set; }
        public Int32 INDEXLOTE { get; set; }
        public String NUMLOTE { get; set; }
        public Int32 TOTALLOTES { get; set; }
        public Int32 TOTALFOGOS { get; set; }
        public Double AREALOTES { get; set; }
        public Double AREAPAGAR { get; set; }
        public String SECTOR { get; set; }
        public Boolean CASA { get; set; }
        public Boolean GARAGEM { get; set; }
        public Boolean MUROS { get; set; }
        public Boolean POCO { get; set; }
        public Boolean FURO { get; set; }
        public Boolean SANEAMENTO { get; set; }
        public Boolean ELECTRICID { get; set; }
        public Boolean PROJECTO { get; set; }
        public Boolean ESCRITURA { get; set; }
        public Boolean FINANCAS { get; set; }
        public Boolean RESIDENCIA { get; set; }
        public Boolean GAVETO { get; set; }
        public Boolean QUINTINHA { get; set; }
        public String LADOMAIOR { get; set; }
        public Boolean HABCOLECT { get; set; }
        public Boolean ARECOMERC { get; set; }
        public String OBSERVACAO { get; set; }
        public String OBSERV2 { get; set; }
        public String OBSERV3 { get; set; }
        public String FICH01 { get; set; }
        public String FICH02 { get; set; }
        public String FICH03 { get; set; }
        public String PROCURADOR { get; set; }
        public String MORPROC { get; set; }
        
        #endregion  Properties
        
        #region     Constructor

        public AMFCMemberLote()
        {
            Set_Empty_Member();
        }

        private void Set_Empty_Member()
        {
            #region     Properties 
            IDLOTE = -1;
            SOCNUM = -1;
            SOCNOME = String.Empty;
            OLDNUM = -1;
            OLDNOME = String.Empty;
            INDEXLOTE = -1;
            NUMLOTE = String.Empty;
            MORLOTE = String.Empty;
            TOTALLOTES = -1;
            TOTALFOGOS = -1;
            AREALOTES = -1;
            AREAPAGAR = -1;
            OBSERVACAO = String.Empty;
            SECTOR = String.Empty;
            CASA = false;
            GARAGEM = false;
            MUROS = false;
            POCO = false;
            FURO = false;
            SANEAMENTO = false;
            ELECTRICID = false;
            PROJECTO = false;
            ESCRITURA = false;
            FINANCAS = false;
            RESIDENCIA = false;
            GAVETO = false;
            QUINTINHA = false;
            LADOMAIOR = String.Empty;
            HABCOLECT = false;
            ARECOMERC = false;
            OBSERV2 = String.Empty;
            OBSERV3 = String.Empty;
            FICH01 = String.Empty;
            FICH02 = String.Empty;
            FICH03 = String.Empty;
            PROCURADOR = String.Empty;
            MORPROC = String.Empty;
            #endregion  Properties
        }

        #endregion  Constructor
    }

    #region     List Lotes

    /// <versions>15-03-2018(GesAMFC-v1.0.0.3)</versions>
    public class AMFCLotes
    {
        public List<AMFCLote> List { get; set; }

        public AMFCLotes()
        {
            List = new List<AMFCLote>();
        }

        /// <versions>02-12-2017(GesAMFC-v0.0.4.41)</versions>
        public Boolean Add(AMFCLote objAMFCLote)
        {
            try
            {
                if (Contains(objAMFCLote))
                    return false;
                List.Add(objAMFCLote);
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <versions>02-12-2017(GesAMFC-v0.0.4.41)</versions>
        public Boolean Clear()
        {
            try
            {
                List.Clear();
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <versions>02-12-2017(GesAMFC-v0.0.4.41)</versions>
        public Boolean Del(Int32 iLote)
        {
            try
            {
                List<AMFCLote> newList = new List<AMFCLote>();
                foreach (AMFCLote objAMFCLote in List)
                {
                    if (objAMFCLote.Value != iLote)
                    {
                        AMFCLote checkAMFCLote = new AMFCLote();
                        checkAMFCLote.Value = iLote;
                        if (!ListContains(newList, checkAMFCLote))
                        {
                            AMFCLote newAMFCLote = GetItemByValue(iLote);
                            newList.Add(newAMFCLote);
                        }
                    }
                }
                List.Clear();
                List = newList;
                return true;
            }
            catch
            {
                return false;
            }
        }

        /// <versions>02-12-2017(GesAMFC-v0.0.4.41)</versions>
        public Boolean Contains(AMFCLote objLote)
        {
            try
            {
                return ListContains(List, objLote);
            }
            catch
            {
                return false;
            }
        }

        /// <versions>02-12-2017(GesAMFC-v0.0.4.41)</versions>
        private Boolean ListContains(List<AMFCLote> objListLotes, AMFCLote objLote)
        {
            try
            {
                foreach (AMFCLote objAMFCLote in objListLotes)
                {
                    if (objAMFCLote.Value == objLote.Value)
                        return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        /// <versions>02-12-2017(GesAMFC-v0.0.4.41)</versions>
        public AMFCLote GetItemByDesc(String sLote)
        {
            try
            {
                return Get_Item_By_Desc(List, sLote);
            }
            catch
            {
                return null;
            }
        }

        /// <versions>02-12-2017(GesAMFC-v0.0.4.41)</versions>
        public AMFCLote Get_Item_By_Desc(List<AMFCLote> objListLotes, String sLote)
        {
            try
            {
                AMFCLote foundAMFCLote = new AMFCLote();
                foreach (AMFCLote objAMFCLote in objListLotes)
                {
                    if (objAMFCLote.Description == sLote)
                    {
                        foundAMFCLote = new AMFCLote(objAMFCLote.Value, objAMFCLote.Description);
                        return foundAMFCLote;
                    }
                }
                return null;
            }
            catch
            {
                return null;
            }
        }

        /// <versions>02-12-2017(GesAMFC-v0.0.4.41)</versions>
        public AMFCLote GetItemByValue(Int32 iLote)
        {
            try
            {
                return Get_Item_By_Value(List, iLote);
            }
            catch
            {
                return null;
            }
        }

        /// <versions>02-12-2017(GesAMFC-v0.0.4.41)</versions>
        public AMFCLote Get_Item_By_Value(List<AMFCLote> objListLotes, Int32 iLote)
        {
            try
            {
                AMFCLote foundAMFCLote = new AMFCLote();
                foreach (AMFCLote objAMFCLote in objListLotes)
                {
                    if (objAMFCLote.Value == iLote)
                    {
                        foundAMFCLote = new AMFCLote(objAMFCLote.Value, objAMFCLote.Description);
                        return foundAMFCLote;
                    }
                }
                return null;
            }
            catch
            {
                return null;
            }
        }
    }

    /// <versions>15-03-2018(GesAMFC-v1.0.0.3)</versions>
    public class AMFCLote
    {
        #region     Fields
        private Int32 _Value = -1;
        private String _Description = String.Empty;
        #endregion  Fields

        #region     Properties

        /// <versions>02-12-2017(GesAMFC-v0.0.4.41)</versions>
        public Int32 Value
        {
            get
            {
                return _Value;
            }
            set
            {
                if (value < 1)
                    return;
                _Value = value;
            }
        }

        /// <versions>02-12-2017(GesAMFC-v0.0.4.41)</versions>
        public String Description
        {
            get
            {
                return _Description;
            }
            set
            {
                if (String.IsNullOrEmpty(value))
                    return;
                _Description = value;
            }
        }

        #endregion  Properties

        #region     Constructors

        /// <versions>02-12-2017(GesAMFC-v0.0.4.41)</versions>
        public AMFCLote()
        {
            Value = -1;
            Description = String.Empty;
        }

        /// <versions>02-12-2017(GesAMFC-v0.0.4.41)</versions>
        public AMFCLote(Int32 iValue, String sDescription)
        {
            Value = iValue;
            Description = sDescription;
        }

        #endregion  Constructors
    }

    #endregion  List Lotes

    #endregion  Members Lotes

    #region     CC INFRA

    /// <versions>07-03-2018(GesAMFC-v1.0.0.3)</versions>
    /// <remarks>
    /// ID,N,10,0	SOCNUM,N,5,0	SOCNOME,C,70	IDLOTE,N,5,0	NUMLOTE,C,10	NUMPAG,N,2,0	AREA,N,12,2	AREAPAGAR,N,12,2	PRECOM2,N,12,3  VALORPAGAR,N,12,3	DATA,D	PRECOM2P,N,12,3	    AREAPAGO,N,12,2	 VALORESCUD,N,12,3	   VALORPAGO,N,12,3	NOTASPAGO,C,140	PRECOM2F,N,12,3	AREAFALTA,N,12,2	VALORFALTA,N,12,3	NOTASFALTA,C,140	ESTADOLIQ,C,50	NOTASLIQ,C,140	
    /// 01: ID,N,10,0	
    /// 02: SOCNUM,N,5,0	
    /// 03: SOCNOME,C,70	
    /// 04: IDLOTE,N,5,0	
    /// 05: NUMLOTE,C,10	
    /// 06: NUMPAG,N,2,0	
    /// 07: AREA,N,12,2	
    /// 08: AREAPAGAR,N,12,2	
    /// 09: PRECOM2,N,12,3	
    /// 10: VALORPAGAR,N,12,3
    /// 11: DATA,D	
    /// 12: PRECOM2P,N,12,3	
    /// 13: AREAPAGO,N,12,2	
    /// VALORESCUD,N,12,3
    /// 14: VALORPAGO,N,12,3	
    /// 15: NOTASPAGO,C,140	
    /// 16: PRECOM2F,N,12,3	
    /// 17: AREAFALTA,N,12,2	
    /// 18: VALORFALTA,N,12,3	
    /// 19: NOTASFALTA,C,140	
    /// 20: ESTADOLIQ,C,50	
    /// 21: NOTASLIQ,C,140	
    /// </remarks>
    public class AMFC_ContaCorrente_INFRA
    {
        #region     Properties
        public Int64 ID { get; set; }
        public Int64 SOCNUM { get; set; }
        public String SOCNOME { get; set; }
        public Int64 IDLOTE { get; set; }
        public String NUMLOTE { get; set; }
        public Int32 NUMPAG { get; set; }
        public Double AREA { get; set; }
        public Double AREAPAGAR { get; set; }
        public Double PRECOM2 { get; set; }
        public Double VALORPAGAR { get; set; }
        public DateTime DATA { get; set; }
        public Double PRECOM2P { get; set; }
        public Double AREAPAGO { get; set; }
        public Double VALORESCUD { get; set; }
        public Double VALORPAGO { get; set; }
        public String NOTASPAGO { get; set; }
        public Double PRECOM2F { get; set; }
        public Double AREAFALTA { get; set; }
        public Double VALORFALTA { get; set; }
        public String NOTASFALTA { get; set; }
        public String ESTADOLIQ { get; set; }
        public String NOTASLIQ { get; set; }
        #endregion  Properties

        #region     Constructor

        public AMFC_ContaCorrente_INFRA()
        {
            Set_Empty();
        }

        private void Set_Empty()
        {
            ID = -1;
            SOCNUM = -1;
            SOCNOME = "";
            IDLOTE = -1;
            NUMLOTE = "";
            NUMPAG = 0;
            AREA = 0;
            AREAPAGAR = 0;
            PRECOM2 = 0;
            VALORPAGAR = 0;
            DATA = new DateTime();
            PRECOM2P = 0;
            AREAPAGO = 0;
            VALORESCUD = 0;
            VALORPAGO = 0;
            NOTASPAGO = "";
            PRECOM2F = 0;
            AREAFALTA = 0;
            VALORFALTA = 0;
            NOTASFALTA = "";
            ESTADOLIQ = "";
            NOTASLIQ = "";
    }

        #endregion  Constructor
    }

    #endregion  CC INFRA

    #region     CC CEDEN

    /// <versions>07-03-2018(GesAMFC-v1.0.0.3)</versions>
    /// <remarks>
    /// ID,N,10,0	SOCNUM,N,5,0	SOCNOME,C,70	IDLOTE,N,5,0	NUMLOTE,C,10	NUMPAG,N,2,0	AREA,N,12,2	    CEDERPERC,N,4,2	    AREAPAGAR,N,12,2	PRECOM2,N,12,3	VALORPAGAR,N,12,3	DATA,D	PRECOM2P,N,12,3	    AREAPAGO,N,12,2	    VALORESCUD,N,12,3   VALORPAGO,N,12,3	NOTASPAGO,C,140	PRECOM2F,N,12,3	AREAFALTA,N,12,2	VALORFALTA,N,12,3	NOTASFALTA,C,140	ESTADOLIQ,C,50	NOTASLIQ,C,140	
    /// 01: ID,N,10,0	
    /// 02: SOCNUM,N,5,0	
    /// 03: SOCNOME,C,70	
    /// 04: IDLOTE,N,5,0	
    /// 05: NUMLOTE,C,10	
    /// 06: NUMPAG,N,2,0	
    /// 07: AREA,N,12,2	    
    /// 08: CEDERPERC,N,4,2	    
    /// 09: AREAPAGAR,N,12,2	
    /// 10: PRECOM2,N,12,3	
    /// 11: VALORPAGAR,N,12,3	
    /// 12: DATA,D	
    /// 13: PRECOM2P,N,12,3	
    /// 14: AREAPAGO,N,12,2	
    /// VALORESCUD,N,12,3
    /// 15: VALORPAGO,N,12,3	
    /// 16: NOTASPAGO,C,140	
    /// 17: PRECOM2F,N,12,3	
    /// 18: AREAFALTA,N,12,2	
    /// 19: VALORFALTA,N,12,3	
    /// 20: NOTASFALTA,C,140	
    /// 21: ESTADOLIQ,C,50	
    /// 22: NOTASLIQ,C,140	
    /// </remarks>
    public class AMFC_ContaCorrente_CEDEN
    {
        #region     Properties
        public Int64 ID { get; set; }
        public Int64 SOCNUM { get; set; }
        public String SOCNOME { get; set; }
        public Int64 IDLOTE { get; set; }
        public String NUMLOTE { get; set; }
        public Int32 NUMPAG { get; set; }
        public Double AREA { get; set; }
        public Double CEDERPERC { get; set; }
        public Double AREAPAGAR { get; set; }
        public Double PRECOM2 { get; set; }
        public Double VALORPAGAR { get; set; }
        public DateTime DATA { get; set; }
        public Double PRECOM2P { get; set; }
        public Double AREAPAGO { get; set; }
        public Double VALORESCUD { get; set; }
        public Double VALORPAGO { get; set; }
        public String NOTASPAGO { get; set; }
        public Double PRECOM2F { get; set; }
        public Double AREAFALTA { get; set; }
        public Double VALORFALTA { get; set; }
        public String NOTASFALTA { get; set; }
        public String ESTADOLIQ { get; set; }
        public String NOTASLIQ { get; set; }

        #endregion  Properties

        #region     Constructor

        public AMFC_ContaCorrente_CEDEN()
        {
            Set_Empty();
        }

        private void Set_Empty()
        {
            #region     Properties 
            ID = -1;
            SOCNUM = -1;
            SOCNOME = "";
            IDLOTE = -1;
            NUMLOTE = "";
            NUMPAG = 0;
            AREA = 0;
            AREAPAGAR = 0;
            CEDERPERC = 0;
            VALORPAGAR = 0;
            DATA = new DateTime();
            PRECOM2P = 0;
            AREAPAGO = 0;
            VALORESCUD = 0;
            VALORPAGO = 0;
            NOTASPAGO = "";
            PRECOM2F = 0;
            AREAFALTA = 0;
            VALORFALTA = 0;
            NOTASFALTA = "";
            ESTADOLIQ = "";
            NOTASLIQ = "";
            #endregion  Properties
        }

        /// <summary>
        //Até 500 m2 . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .. 25 % 
        //De 501 m2 a 1 500 m2. . . . . . . . . . . . . . . . . . . . . . . . . . 30 % 
        //De 1 501 m2 a 2 500 m2. . . . . . . . . . . . . . . . . . . . . . . . . 35 % 
        //De 2 501 m2 a 3 500 m2. . . . . . . . . . . . . . . . . . . . . . . . . 40 % 
        //De 3 501 m2 a 4 500 m2. . . . . . . . . . . . . . . . . . . . . . . . . 45 % 
        //De 4 501 m2 a 5 500 m2. . . . . . . . . . . . . . . . . . . . . . . . . 50 % 
        //De 5 501 m2 a 10 500 m2. . . . . . . . . . . . . . . . . . . . . . . .  55 % 
        //Mais de 10 500 m2. . . . . . . . . . . . . . . . . . . . . . . . . . .. 60 %
        /// </summary>
        public Double GetCederTotalAreaCeder(Double db_AREALOTE)
        {
            try
            {
                Double dbCederArea = 0;
                if (db_AREALOTE <= 0)
                    return 0;
                if (db_AREALOTE > 0 && db_AREALOTE <= 500)
                    dbCederArea = db_AREALOTE * 0.25;

                else if (db_AREALOTE > 500 && db_AREALOTE <= 1500)
                    dbCederArea = db_AREALOTE * 0.3;

                else if (db_AREALOTE > 1500 && db_AREALOTE <= 2500)
                    dbCederArea = db_AREALOTE * 0.35;

                else if (db_AREALOTE > 2500 && db_AREALOTE <= 3500)
                    dbCederArea = db_AREALOTE * 0.4;

                else if (db_AREALOTE > 3500 && db_AREALOTE <= 4500)
                    dbCederArea = db_AREALOTE * 0.45;

                else if (db_AREALOTE > 4500 && db_AREALOTE <= 5500)
                    dbCederArea = db_AREALOTE * 0.5;

                else if (db_AREALOTE > 5500 && db_AREALOTE <= 10500)
                    dbCederArea = db_AREALOTE * 0.55;

                else if (db_AREALOTE > 10500)
                    dbCederArea = db_AREALOTE * 0.6;

                dbCederArea = Math.Round(dbCederArea, 2);

                return dbCederArea;
            }
            catch { return 0; }
        }

        /// <summary>
        //Até 500 m2 . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .. 25 % 
        //De 501 m2 a 1 500 m2. . . . . . . . . . . . . . . . . . . . . . . . . . 30 % 
        //De 1 501 m2 a 2 500 m2. . . . . . . . . . . . . . . . . . . . . . . . . 35 % 
        //De 2 501 m2 a 3 500 m2. . . . . . . . . . . . . . . . . . . . . . . . . 40 % 
        //De 3 501 m2 a 4 500 m2. . . . . . . . . . . . . . . . . . . . . . . . . 45 % 
        //De 4 501 m2 a 5 500 m2. . . . . . . . . . . . . . . . . . . . . . . . . 50 % 
        //De 5 501 m2 a 10 500 m2. . . . . . . . . . . . . . . . . . . . . . . .  55 % 
        //Mais de 10 500 m2. . . . . . . . . . . . . . . . . . . . . . . . . . .. 60 %
        /// </summary>
        public Double GetCederPercentagem(Double db_AREALOTE)
        {
            try
            {
                Double dbCederPercent = 0;
                if (db_AREALOTE <= 0)
                    return 0;
                if (db_AREALOTE > 0 && db_AREALOTE <= 500)
                    dbCederPercent = 0.25;

                else if (db_AREALOTE > 500 && db_AREALOTE <= 1500)
                    dbCederPercent = 0.3;

                else if (db_AREALOTE > 1500 && db_AREALOTE <= 2500)
                    dbCederPercent = 0.35;

                else if (db_AREALOTE > 2500 && db_AREALOTE <= 3500)
                    dbCederPercent = 0.4;

                else if (db_AREALOTE > 3500 && db_AREALOTE <= 4500)
                    dbCederPercent = 0.45;

                else if (db_AREALOTE > 4500 && db_AREALOTE <= 5500)
                    dbCederPercent = 0.5;

                else if (db_AREALOTE > 5500 && db_AREALOTE <= 10500)
                    dbCederPercent = 0.55;

                else if (db_AREALOTE > 10500)
                    dbCederPercent = db_AREALOTE * 0.6;

                dbCederPercent = Math.Round(dbCederPercent, 2);

                return dbCederPercent;
            }
            catch { return 0; }
        }

        #endregion  Constructor
    }

    #endregion  CC CEDEN

    #region     CC ESGOT

    /// <versions>08-03-2018(GesAMFC-v1.0.0.3)</versions>
    /// <remarks>
    /// ID,N,10,0	SOCNUM,N,5,0	SOCNOME,C,70	IDLOTE,N,5,0	NUMLOTE,C,10	AREAPAGAN,N,10,2	AREAPAGAR,N,10,2	NUMPAG,N,2,0	VALORPAGAR,N,12,3   DATA,D   VALORESCUD,N,12,3   VALORPAGO, N,12,3	NOTASPAGO,C,140	    VALORFALTA,N,12,3	NOTASFALTA,C,140	ESTADOLIQ,C,50	NOTASLIQ,C,140
    /// 01: ID,N,10,0	
    /// 02: SOCNUM,N,5,0	
    /// 03: SOCNOME,C,70	
    /// 04: IDLOTE,N,5,0	
    /// 05: NUMLOTE,C,10	
    /// 06: AREAPAGAN,N,10,2	
    /// 07: AREAPAGAR,N,10,2	
    /// 08: NUMPAG,N,2,0	
    /// 09: VALORPAGAR,N,12,3
    /// 10: DATA,D      
    /// VALORESCUD,N,12,3
    /// 11: VALORPAGO, N,12,3	
    /// 12: NOTASPAGO,C,140	    
    /// 13: VALORFALTA,N,12,3	
    /// 14: NOTASFALTA,C,140	
    /// 15: ESTADOLIQ,C,50	
    /// 16: NOTASLIQ,C,140
    /// </remarks>
    public class AMFC_ContaCorrente_ESGOT
    {
        #region     Properties
        public Int64 ID { get; set; } /// 01: ID,N,10,0	
        public Int64 SOCNUM { get; set; } /// 02: SOCNUM,N,5,0	
        public String SOCNOME { get; set; } /// 03: SOCNOME,C,70
        public Int64 IDLOTE { get; set; } /// 04: IDLOTE,N,5,0	
        public String NUMLOTE { get; set; } /// 05: NUMLOTE,C,10	
        public Double AREAPAGAN { get; set; } /// 06: AREAPAGAN,N,10,2	
        public Double AREAPAGAR { get; set; } /// 07: AREAPAGAR,N,10,2	
        public Int32 NUMPAG { get; set; } /// 08: NUMPAG,N,2,0	
        public Double VALORPAGAR { get; set; } /// 09: VALORPAGAR,N,12,3
        public DateTime DATA { get; set; } /// 10: DATA,D     
        public Double VALORESCUD { get; set; } //VALORESCUD,N,12,3
        public Double VALORPAGO { get; set; } /// 11: VALORPAGO, N,12,3	
        public String NOTASPAGO { get; set; } /// 12: NOTASPAGO,C,140	    
        public Double VALORFALTA { get; set; } /// 13: VALORFALTA,N,12,3
        public String NOTASFALTA { get; set; } /// 14: NOTASFALTA,C,140
        public String ESTADOLIQ { get; set; } /// 15: ESTADOLIQ,C,50
        public String NOTASLIQ { get; set; } /// 16: NOTASLIQ,C,140

        #endregion  Properties

        #region     Constructor

        public AMFC_ContaCorrente_ESGOT()
        {
            Set_Empty();
        }

        private void Set_Empty()
        {
            #region     Properties 
            ID = -1;
            SOCNUM = -1;
            SOCNOME = "";
            IDLOTE = -1;
            NUMLOTE = "";
            NUMPAG = 0;
            AREAPAGAN = 0;
            AREAPAGAR = 0;
            VALORPAGAR = 0;
            DATA = new DateTime();
            VALORESCUD = 0;
            VALORPAGO = 0;
            NOTASPAGO = "";
            VALORFALTA = 0;
            NOTASFALTA = "";
            ESTADOLIQ = "";
            NOTASLIQ = "";
            #endregion  Properties
        }

        #endregion  Constructor
    }

    #endregion  CC ESGOT

    #region     CC RECON

    /// <versions>07-03-2018(GesAMFC-v1.0.0.3)</versions>
    /// <remarks>
    /// ID,N,10,0	SOCNUM,N,5,0	SOCNOME,C,70	IDLOTE,N,5,0	NUMLOTE,C,10	NUMPAG,N,2,0	AREA,N,12,2	    CEDERPERC,N,4,2	    AREAPAGAR,N,12,2	PRECOM2,N,12,3  VALORPAGAR,N,12,3	DATA,D	PRECOM2P,N,12,3	    AREAPAGO,N,12,2	    VALORESCUD,N,12,3 	VALORPAGO,N,12,3	NOTASPAGO,C,140	PRECOM2F,N,12,3	AREAFALTA,N,12,2	VALORFALTA,N,12,3	NOTASFALTA,C,140	ESTADOLIQ,C,50	NOTASLIQ,C,140	
    /// 01: ID,N,10,0	
    /// 02: SOCNUM,N,5,0	
    /// 03: SOCNOME,C,70	
    /// 04: IDLOTE,N,5,0	
    /// 05: NUMLOTE,C,10	
    /// 06: NUMPAG,N,2,0	
    /// 07: AREA,N,12,2	    
    /// 08: CEDERPERC,N,4,2	    //Ingnorar 
    /// 09: AREAPAGAR,N,12,2	
    /// 10: PRECOM2,N,12,3	
    /// 11: VALORPAGAR,N,12,3	
    /// 12: DATA,D	
    /// 13: PRECOM2P,N,12,3	
    /// 14: AREAPAGO,N,12,2	
    /// VALORESCUD,N,12,3	
    /// 15: VALORPAGO,N,12,3	
    /// 16: NOTASPAGO,C,140	
    /// 17: PRECOM2F,N,12,3	
    /// 18: AREAFALTA,N,12,2	
    /// 19: VALORFALTA,N,12,3	
    /// 20: NOTASFALTA,C,140	
    /// 21: ESTADOLIQ,C,50	
    /// 22: NOTASLIQ,C,140	
    /// </remarks>
    public class AMFC_ContaCorrente_RECON
    {
        #region     Properties
        public Int64 ID { get; set; }
        public Int64 SOCNUM { get; set; }
        public String SOCNOME { get; set; }
        public Int64 IDLOTE { get; set; }
        public String NUMLOTE { get; set; }
        public Int32 NUMPAG { get; set; }
        public Double AREA { get; set; }
        //public Double CEDERPERC { get; set; } //Ingnorar 
        public Double AREAPAGAR { get; set; }
        public Double PRECOM2 { get; set; }
        public Double VALORPAGAR { get; set; }
        public DateTime DATA { get; set; }
        public Double PRECOM2P { get; set; }
        public Double AREAPAGO { get; set; }
        public Double VALORESCUD { get; set; }
        public Double VALORPAGO { get; set; }
        public String NOTASPAGO { get; set; }
        public Double PRECOM2F { get; set; }
        public Double AREAFALTA { get; set; }
        public Double VALORFALTA { get; set; }
        public String NOTASFALTA { get; set; }
        public String ESTADOLIQ { get; set; }
        public String NOTASLIQ { get; set; }

        #endregion  Properties

        #region     Constructor

        public AMFC_ContaCorrente_RECON()
        {
            Set_Empty();
        }

        private void Set_Empty()
        {
            #region     Properties 
            ID = -1;
            SOCNUM = -1;
            SOCNOME = "";
            IDLOTE = -1;
            NUMLOTE = "";
            NUMPAG = 0;
            AREA = 0;
            AREAPAGAR = 0;
            //CEDERPERC = 0; //Ingnorar 
            VALORPAGAR = 0;
            DATA = new DateTime();
            PRECOM2P = 0;
            AREAPAGO = 0;
            VALORESCUD = 0;
            VALORPAGO = 0;
            NOTASPAGO = "";
            PRECOM2F = 0;
            AREAFALTA = 0;
            VALORFALTA = 0;
            NOTASFALTA = "";
            ESTADOLIQ = "";
            NOTASLIQ = "";
            #endregion  Properties
        }

        #endregion  Constructor
    }

    #endregion  CC RECON

    #region     Cash Payments

    /// <versions>16-06-2017(v0.0.4.1)</versions>
    public class AMFCCashPayments
    {
        public List<AMFCCashPayment> Payments { get; set; }
        public AMFCCashPayments()
        {
            Payments = new List<AMFCCashPayment>();
        }
        public Boolean Add(AMFCCashPayment objPayment)
        {
            try
            {
                objPayment.Idx = this.Payments.Count;
                if (Contains(objPayment))
                    return false;
                this.Payments.Add(objPayment);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public Boolean Contains(AMFCCashPayment objPayment)
        {
            try
            {
                foreach (AMFCCashPayment objPay in this.Payments)
                {
                    if (objPay.ID == objPayment.ID)
                        return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
        public Int32 GetPaymentIndex(AMFCCashPayment objPayment)
        {
            try
            {
                for (Int32 iIdx = 0; iIdx < this.Payments.Count; iIdx++)
                {
                    AMFCCashPayment objPay = this.Payments[iIdx];
                    if (objPay.ID == objPayment.ID)
                        return iIdx;
                }
                return -1;
            }
            catch
            {
                return -1;
            }
        }
    }

    /// <versions>07-12-2017(GesAMFC-v0.0.5.1)</versions>
    /// <remarks>
    /// ID,N,10,0	
    /// LISTARECNU,C,20	
    /// SOCIO,N,5,0	
    /// NOME,C,70	
    /// DESIGNACAO,C,140	
    /// NOTAS,C,140	
    /// VALOR,N,10,2	
    /// DATA,D
    /// ALTERADO, D
    /// JOIA,C,1	
    /// QUOTAS,C,1	
    /// INFRAEST,C,1	
    /// RECONV,C,1	
    /// CEDENCIAS,C,1	
    /// OUTRO,C,1	
    /// JOIAVAL,N,10,2	
    /// QUOTASVAL,N,10,2	
    /// INFRAVAL,N,10,2	
    /// RECONVAL,N,10,2	
    /// CEDENCVAL,N,10,2	
    /// OUTROSVAL,N,10,2	
    /// ASSOCJOIA, C,1	
    /// DASSOCJOIA,D
    /// JOIADESC, C,140	
    /// ASSOCQUOTA,C,1	
    /// DASSOCQUOT,D
    /// QUOTASDESC, C,140	
    /// ASSOCNFRA,C,1	
    /// DASSOCINFR,D
    /// INFRADESC, C,140	
    /// ASSOCRECON,C,1	
    /// DASSOCRECO,D
    /// RECONDESC, C,140	
    /// ASSOCCEDEN,C,1	
    /// DASSOCCEDE,D
    /// CEDENCDESC, C,140	
    /// ASSOCOUTRO,C,1	
    /// DASSOCOUTR,D
    /// OUTROSDESC, C,140	
    /// ESTADO,C,1	
    /// </remarks>
    public class AMFCCashPayment
    {
        #region     Private Fields

        #region     Payments Types

        public enum PaymentTypes
        {
            UNDEFINED   = -1,
            TOTAL       = 0,
            JOIA        = 1,
            QUOTAS      = 2,
            INFRAS      = 3,	
            RECONV      = 4,
            CEDENC      = 5,
            ESGOT       = 6,
            OUTRO       = 7,
            MULTIPLE    = 8,
        }

        private PaymentTypes _Payment_Type = PaymentTypes.UNDEFINED;

        public PaymentTypes Payment_Type
        {
            get
            {
                return _Payment_Type;
            }
            set
            {
                _Payment_Type = value;
            }
        }

        #endregion  Payments Types

        #region     Payment State

        public enum PaymentState
        {
            UNDEFINED = -1,
            ABERTO = 1,
            FINALIZADO = 2,
            CANCELED = 3
        }

        private String _Payment_State_DB_Value = "";

        private PaymentState _Payment_State = PaymentState.UNDEFINED;

        public String Payment_State_DB_Value
        {
            get
            {
                return _Payment_State_DB_Value;
            }
            set
            {
                if (String.IsNullOrEmpty(value.Trim()) || value.Trim().Length != 1)
                    return;
                _Payment_State_DB_Value = value.Trim().ToUpper();
                switch (_Payment_State_DB_Value)
                {
                    case "A":
                        _Payment_State = PaymentState.ABERTO;
                        break;
                    case "F":
                        _Payment_State = PaymentState.FINALIZADO;
                        break;
                    default:
                        Payment_State = PaymentState.UNDEFINED;
                        break;
                }
            }
        }

        public PaymentState Payment_State
        {
            get
            {
                return _Payment_State;
            }
            set
            {
                _Payment_State = value;
                if (_Payment_State != PaymentState.UNDEFINED)
                    _Payment_State_DB_Value = GetPaymentStateDBvalue(_Payment_State);
            }
        }

        #endregion  Payment State

        #region     Length Max Values
        private Int32 _Max_Length_End_Space_Chars = 5;
        private String _Max_Length_End_Reticencias = " ...";

        private Int32 _Max_Length_LISTARECNU    = 20;
        private Int32 _Max_Length_NOME          = 70;
        private Int32 _Max_Length_NOTAS         = 140;
        private Int32 _Max_Length_JOIADESC      = 140;
        private Int32 _Max_Length_QUOTASDESC    = 140;
        private Int32 _Max_Length_INFRADESC     = 140;
        private Int32 _Max_Length_CEDENCDESC    = 140;
        private Int32 _Max_Length_ESGOTDESC     = 140;
        private Int32 _Max_Length_RECONDESC     = 140;
        private Int32 _Max_Length_OUTROSDESC    = 140;

        private Int32 _Max_Length_EntidadeDESC = 140;

        #endregion  Length Max Values

        private Int64       _Idx = -1;
        private Int64       _ID = -1;
        private List<Int64> _ListaRecibosIDs = new List<Int64>();
        private String      _LISTARECNU = String.Empty;
        private Int64       _SOCIO = -1;
        private String      _NOME = String.Empty;
        private String      _NOME_PAG = String.Empty;
        private String      _DESIGNACAO = String.Empty;
        private String      _NOTAS = String.Empty;

        private Double      _VALOR = Program.Default_Pay_Value;
        private String      _VALOR_Str = String.Empty;

        private DateTime    _DATA = new DateTime();
        private String      _DATA_Str = String.Empty;
        private Int32       _DATAYearInt = -1;
        private String      _DATAYear = String.Empty;
        private Int32       _DATAMonthInt = -1;
        private String      _DATAMonth = String.Empty;
        private String      _DATAMonthYear = String.Empty;
        private DateTime    _ALTERADO = new DateTime();
        private String      _ALTERADO_Str = String.Empty;

        private String      _JOIADESC    = String.Empty;
        private String      _JOIA = String.Empty;
        private Boolean     _HasJOIA = false;
        private Double      _JOIAVAL = Program.Default_Pay_Value;
        private String      _JOIAVAL_Str = String.Empty;
        private String      _ASSOCJOIA = String.Empty;
        private Boolean     _IsASSOCJOIA = false;
        private DateTime    _DASSOCJOIA = new DateTime();
        private String      _DASSOCJOIA_Str = String.Empty;

        private String      _QUOTASDESC  = String.Empty;
        private String      _QUOTAS = String.Empty;
        private Boolean     _HasQUOTAS = false;
        private Double      _QUOTASVAL = Program.Default_Pay_Value;
        private String      _QUOTASVAL_Str = String.Empty;
        private String      _ASSOCQUOTA = String.Empty;
        private Boolean     _IsASSOCQUOTA = false;
        private DateTime    _DASSOCQUOT = new DateTime();
        private String      _DASSOCQUOT_Str = String.Empty;

        private String      _INFRADESC   = String.Empty;
        private String      _INFRAEST = String.Empty;
        private Boolean     _HasINFRAEST = false;
        private Double      _INFRAVAL = Program.Default_Pay_Value;
        private String      _INFRAVAL_Str = String.Empty;
        private String      _ASSOCNFRA = String.Empty;
        private Boolean     _IsASSOCNFRA = false;
        private DateTime    _DASSOCINFR = new DateTime();
        private String      _DASSOCINFR_Str = String.Empty;

        private String      _CEDENCDESC  = String.Empty;
        private String      _CEDENCIAS = String.Empty;
        private Boolean     _HasCEDENCIAS = false;
        private Double      _CEDENCVAL = Program.Default_Pay_Value;
        private String      _CEDENCVAL_Str = String.Empty;
        private String      _ASSOCCEDEN = String.Empty;
        private Boolean     _IsASSOCCEDEN = false;
        private DateTime    _DASSOCCEDE = new DateTime();
        private String      _DASSOCCEDE_Str = String.Empty;

        private String      _ESGOTDESC = String.Empty;
        private String      _ESGOT = String.Empty;
        private Boolean     _HasESGOT = false;
        private Double      _ESGOTVAL = Program.Default_Pay_Value;
        private String      _ESGOTVAL_Str = String.Empty;
        private String      _ASSOCESGOT = String.Empty;
        private Boolean     _IsASSOCESGOT = false;
        private DateTime    _DASSOCESGO = new DateTime();
        private String      _DASSOCESGO_Str = String.Empty;

        private String      _RECONDESC   = String.Empty;
        private String      _RECONV = String.Empty;
        private Boolean     _HasRECONV = false;
        private Double      _RECONVAL = Program.Default_Pay_Value;
        private String      _RECONVAL_Str = String.Empty;
        private String      _ASSOCRECON = String.Empty;
        private Boolean     _IsASSOCRECON = false;
        private DateTime    _DASSOCRECO = new DateTime();
        private String      _DASSOCRECO_Str = String.Empty;

        private String      _OUTROSDESC  = String.Empty;
        private String      _OUTRO = String.Empty;
        private Boolean     _HasOUTRO = false;
        private Double      _OUTROSVAL = Program.Default_Pay_Value;
        private String      _OUTROSVAL_Str = String.Empty;
        private String      _ASSOCOUTRO = String.Empty;
        private Boolean     _IsASSOCOUTRO = false;
        private DateTime    _DASSOCOUTR = new DateTime();
        private String      _DASSOCOUTR_Str = String.Empty;

        private String _EntidadeDESC = String.Empty;
        private String _Entidade = String.Empty;
        private Boolean _HasEntidade = false;
        private Double _EntidadeVAL = Program.Default_Pay_Value;
        private String _EntidadeVAL_Str = String.Empty;
        private String _ASSOCEntidade = String.Empty;
        private Boolean _IsASSOCEntidade = false;
        private DateTime _DASSOCEntidade = new DateTime();
        private String _DASSOCEntidade_Str = String.Empty;

        #region FAlta as propriedades
        private List<Int64> _LISTAJOIAIDs = new List<Int64>();
        private String _LISTAJOIA = String.Empty;

        private List<Int64> LISTAQUOTAIDs = new List<Int64>();
        private String _LISTAQUOTA = String.Empty;

        private List<Int64> _LISTAINFRAIDs = new List<Int64>();
        private String _LISTAINFRA = String.Empty;

        private List<Int64> _LISTACEDENIDs = new List<Int64>();
        private String _LISTACEDEN = String.Empty;

        private List<Int64> _LISTAESGOTIDs = new List<Int64>();
        private String _LISTAESGOT = String.Empty;

        private List<Int64> _LISTARECONIDs = new List<Int64>();
        private String _LISTARECON = String.Empty;

        private List<Int64> _LISTAEntidadeIDs = new List<Int64>();
        private String _LISTAEntidade = String.Empty;

        #endregion FAlta as propriedades

        #endregion  Private Fields

        #region     Properties

        public Int64    Idx
        {
            get { return _Idx; }
            set { _Idx = value; }
        }

        public Int64    ID
        {
            get { return _ID; }
            set
            {
                if (value < 1)
                    return;
                _ID = value;
            }
        }

        public String   LISTARECNU
        {
            get
            {
                try
                {
                    if (String.IsNullOrEmpty(_LISTARECNU))
                        return String.Empty;
                    return _LISTARECNU;
                }
                catch
                {
                    return String.Empty;
                }
            }
            set
            {
                try
                {
                    if (String.IsNullOrEmpty(value))
                        return;
                    _LISTARECNU = value;
                    try
                    {
                        List<String> ListaRecibos = _LISTARECNU.Split(new String[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
                        List<Int64> objListaRecibosIDs = new List<Int64>();
                        foreach (String sId in ListaRecibos)
                        {
                            try
                            {
                                if (!String.IsNullOrEmpty(sId.Trim()))
                                    objListaRecibosIDs.Add(Convert.ToInt64(sId.Trim()));
                            }
                            catch { }
                        }
                        String sSplitList = ",";
                        String _LISTARECNU_LimitString = String.Empty;
                        foreach (Int64 lRecId in objListaRecibosIDs)
                        {
                            if ((_LISTARECNU_LimitString.Length + sSplitList.Length  + lRecId.ToString().Length) > _Max_Length_LISTARECNU)
                                break;
                            if (!String.IsNullOrEmpty(_LISTARECNU_LimitString))
                                _LISTARECNU_LimitString += sSplitList;
                            _LISTARECNU_LimitString += lRecId.ToString();
                        }
                        ListaRecibosIDs = objListaRecibosIDs;
                        _LISTARECNU = _LISTARECNU_LimitString;
                    }
                    catch { }
                }
                catch
                {
                    _LISTARECNU = String.Empty;
                }
            }
        }

        public List<Int64> ListaRecibosIDs
        {
            get { return _ListaRecibosIDs; }
            set { _ListaRecibosIDs = value; }
        }

        public Int64    SOCIO
        {
            get { return _SOCIO; }
            set
            {
                if (value < 1 || value > new AMFCMember().MaxNumber)
                    return;
                _SOCIO = value;
            }
        }

        public String   NOME
        {
            get { return _NOME; }
            set
            {
                _NOME = value;
                if (_NOME.Length > _Max_Length_NOME)
                {
                    _NOME = _NOME.Substring(0, _Max_Length_NOME - _Max_Length_End_Space_Chars - _Max_Length_End_Reticencias.Length);
                    if ((_NOME.Length + _Max_Length_End_Reticencias.Length) < _Max_Length_NOME)
                        _NOME += _Max_Length_End_Reticencias;
                }
            }
        }

        public String   NOME_PAG
        {
            get { return _NOME_PAG; }
            set { _NOME_PAG = value; }
        }

        public String   DESIGNACAO
        {
            get { return _DESIGNACAO; }
            set { _DESIGNACAO = value; }
        }

        public String   NOTAS
        {
            get { return _NOTAS; }
            set
            {
                if (_NOTAS.Length > _Max_Length_NOTAS)
                {
                    _NOTAS = _NOTAS.Substring(0, _Max_Length_NOTAS - _Max_Length_End_Space_Chars - _Max_Length_End_Reticencias.Length);
                    if ((_NOTAS.Length + _Max_Length_End_Reticencias.Length) < _Max_Length_NOTAS)
                        _NOTAS += _Max_Length_End_Reticencias;
                }
                _NOTAS = value;
            }
        }

        #region     DATAS

        public DateTime DATA
        {
            get
            {
                return _DATA;
            }
            set
            {
                if (!Program.IsValidDateTime(value))
                {
                    _DATA = new DateTime();
                    _DATA_Str = Program.Default_Date_Str;
                }
                else
                {
                    _DATA = value;
                    _DATA_Str = _DATA.ToString(Program.Date_Format_String, CultureInfo.CurrentCulture);
                    DATAYearInt = _DATA.Year;
                    DATAMonthInt = _DATA.Month;
                }
            }
        }

        public String   DATA_Str
        {
            get { return _DATA_Str; }
            set
            {
                if (String.IsNullOrEmpty(value))
                    return;
                _DATA_Str = value;
                _DATA = Program.ConvertToValidDateTime(_DATA_Str);
            }
        }

        public Int32    DATAYearInt
        {
            get { return _DATAYearInt; }
            set
            {
                if (!Program.IsValidYear(value))
                {
                    _DATAYearInt = -1;
                    _DATAYear = Program.DB_Not_Available;
                }
                else
                {
                    _DATAYearInt = value;
                    _DATAYear = _DATAYearInt.ToString();
                }
            }
        }

        public String   DATAYear
        {
            get { return _DATAYear; }
            set
            {
                if (String.IsNullOrEmpty(value))
                    return;
                _DATAYear = value;
            }
        }

        public Int32    DATAMonthInt
        {
            get { return _DATAMonthInt; }
            set
            {
                if (value >= 1 && value <= 12)
                {
                    _DATAMonthInt = value;
                    if (!Program.IsValidYear(_DATAYearInt))
                    {
                        if (Program.IsValidDateTime(_DATA))
                            _DATAYearInt = _DATA.Year;
                        else
                            return;
                    }
                    DateTime dtYearMonth = new DateTime(_DATAYearInt, _DATAMonthInt, 1);
                    if (!Program.IsValidTextString(_DATAMonth))
                        _DATAMonth = "[" + dtYearMonth.ToString("MM", Program.CurrentCulture) + "/" + dtYearMonth.ToString("yy", Program.CurrentCulture) + "]" + ": " + dtYearMonth.ToString("MMMM", Program.CurrentCulture) + " / " + dtYearMonth.ToString("yyyy", Program.CurrentCulture);
                }
                else
                {
                    _DATAMonthInt = -1;
                    _DATAMonth = Program.DB_Not_Available;
                    _DATAMonthYear = Program.DB_Not_Available;
                }
            }
        }

        public String   DATAMonth
        {
            get { return _DATAMonth; }
            set { _DATAMonth = value; }
        }

        public String   DATAMonthYear
        {
            get
            {
                if (_DATAYearInt >= 1970 && _DATAYearInt <= 2069 && _DATAMonthInt >= 1 && _DATAMonthInt <= 12)
                {
                    try
                    {
                        _DATAMonthYear = DATA.ToString("MMM", Program.CurrentCulture) + "/" + _DATAYear;
                    }
                    catch
                    {
                        _DATAMonthYear = Program.DB_Not_Available;
                    }
                }
                else
                {
                    _DATAMonthYear = Program.DB_Not_Available;
                }
                return _DATAMonthYear;
            }
            set { _DATAMonthYear = value; }
        }

        public DateTime ALTERADO
        {
            get
            {
                return _ALTERADO;
            }
            set
            {
                if (!Program.IsValidDateTime(value))
                {
                    _ALTERADO = new DateTime();
                    _ALTERADO_Str = Program.Default_Date_Str;
                }
                else
                {
                    _ALTERADO = value;
                    _ALTERADO_Str = _ALTERADO.ToString(Program.Date_Format_String, CultureInfo.CurrentCulture);
                }
            }
        }

        public String   ALTERADO_Str
        {
            get { return _ALTERADO_Str; }
            set
            {
                if (String.IsNullOrEmpty(value))
                    return;
                _ALTERADO_Str = value;
                _ALTERADO = Program.ConvertToValidDateTime(_ALTERADO_Str);
            }
        }

        #endregion  DATAS

        #region     VALOR TOTAL

        public Double VALOR
        {
            get
            {
                return _VALOR;
            }
            set
            {
                if (value < Program.DB_Min_Pay_Value || value > Program.DB_Max_Pay_Value)
                {
                    MessageBox.Show("Valor inválido!", "set > AMFCCashPayment > VALOR", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                _VALOR = value;
                _VALOR_Str = Program.SetPayCurrencyEuroStringValue(_VALOR);
            }
        }

        public String VALOR_Str
        {
            get { return _VALOR_Str; }
            set { _VALOR_Str = value; }
        }

        #endregion  VALOR TOTAL

        #region     JOIA

        public String   JOIA
        {
            get
            {
                return _JOIA;
            }
            set
            {
                _JOIA = value;
                _HasJOIA = Program.ConvertYesOrNoToBoolean(value);
            }
        }

        public Boolean  HasJOIA
        {
            get
            {
                return _HasJOIA;
            }
            set
            {
                _HasJOIA = value;
                _JOIA = Program.ConvertBooleanToYesOrNo(value);
            }
        }

        public String   JOIADESC
        {
            get
            {
                return _JOIADESC;
            }
            set
            {
                _JOIADESC = value;
                if (_JOIADESC.Length > _Max_Length_JOIADESC)
                {
                    _JOIADESC = _JOIADESC.Substring(0, _Max_Length_JOIADESC - _Max_Length_End_Space_Chars - _Max_Length_End_Reticencias.Length);
                    if ((_JOIADESC.Length + _Max_Length_End_Reticencias.Length) < _Max_Length_JOIADESC)
                        _JOIADESC += _Max_Length_End_Reticencias;
                }
            }
        }

        public Double   JOIAVAL
        {
            get
            {
                return _JOIAVAL;
            }
            set
            {
                if (value < Program.DB_Min_Pay_Value || value > Program.DB_Max_Pay_Value)
                {
                    MessageBox.Show("Valor inválido!", "set > AMFCCashPayment > JOIAVAL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                _JOIAVAL = value;
                _JOIAVAL_Str = Program.SetPayCurrencyEuroStringValue(_JOIAVAL);
                HasJOIA = true;
            }
        }

        public String   JOIAVAL_Str
        {
            get { return _JOIAVAL_Str; }
            set { _JOIAVAL_Str = value; }
        }

        public Boolean  IsASSOCJOIA
        {
            get
            {
                return _IsASSOCJOIA;
            }
            set
            {
                _IsASSOCJOIA = value;
                _ASSOCJOIA = Program.ConvertBooleanToYesOrNo(value);
            }
        }

        public String   ASSOCJOIA
        {
            get { return _ASSOCJOIA; }
            set
            {
                _ASSOCJOIA = value;
                _IsASSOCJOIA = Program.ConvertYesOrNoToBoolean(value);
            }
        }

        public DateTime DASSOCJOIA
        {
            get
            {
                return _DASSOCJOIA;
            }
            set
            {
                _DASSOCJOIA = value;
                if (!Program.IsValidDateTime(value))
                    return;
                _DASSOCJOIA_Str = _DASSOCJOIA.ToString(Program.Date_Format_String, CultureInfo.CurrentCulture);
            }
        }

        public String   DASSOCJOIA_Str
        {
            get { return _DASSOCJOIA_Str; }
            set
            {
                if (String.IsNullOrEmpty(value))
                    return;
                _DASSOCJOIA_Str = value;
                _DASSOCJOIA = Program.ConvertToValidDateTime(_DASSOCJOIA_Str);
            }
        }

        #endregion  JOIA

        #region     QUOTAS

        public String   QUOTAS
        {
            get
            {
                return _QUOTAS;
            }
            set
            {
                _QUOTAS = value;
                _HasQUOTAS = Program.ConvertYesOrNoToBoolean(value);
            }
        }

        public Boolean  HasQUOTAS
        {
            get
            {
                return _HasQUOTAS;
            }
            set
            {
                _HasQUOTAS = value;
                _QUOTAS = Program.ConvertBooleanToYesOrNo(value);
            }
        }

        public String   QUOTASDESC
        {
            get
            {
                return _QUOTASDESC;
            }
            set
            {
                _QUOTASDESC = value;
                if (_QUOTASDESC.Length > _Max_Length_QUOTASDESC)
                {
                    _QUOTASDESC = _QUOTASDESC.Substring(0, _Max_Length_QUOTASDESC - _Max_Length_End_Space_Chars - _Max_Length_End_Reticencias.Length);
                    if ((_QUOTASDESC.Length + _Max_Length_End_Reticencias.Length) < _Max_Length_QUOTASDESC)
                        _QUOTASDESC += _Max_Length_End_Reticencias;
                }
            }
        }

        public Double   QUOTASVAL
        {
            get
            {
                return _QUOTASVAL;
            }
            set
            {
                if (value < Program.DB_Min_Pay_Value || value > Program.DB_Max_Pay_Value)
                {
                    MessageBox.Show("Valor inválido! ", "set > AMFCCashPayment > QUOTASVAL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                _QUOTASVAL = value;
                _QUOTASVAL_Str = Program.SetPayCurrencyEuroStringValue(_QUOTASVAL);
                HasQUOTAS = true;
            }
        }

        public String   QUOTASVAL_Str
        {
            get { return _QUOTASVAL_Str; }
            set { _QUOTASVAL_Str = value; }
        }

        public Boolean  IsASSOCQUOTA
        {
            get
            {
                return _IsASSOCQUOTA;
            }
            set
            {
                _IsASSOCQUOTA = value;
                _ASSOCQUOTA = Program.ConvertBooleanToYesOrNo(value);
            }
        }

        public String   ASSOCQUOTA
        {
            get { return _ASSOCQUOTA; }
            set
            {
                _ASSOCQUOTA = value;
                _IsASSOCQUOTA = Program.ConvertYesOrNoToBoolean(value);
            }
        }

        public DateTime DASSOCQUOT
        {
            get
            {
                return _DASSOCQUOT;
            }
            set
            {
                _DASSOCQUOT = value;
                if (!Program.IsValidDateTime(value))
                    return;
                _DASSOCQUOT_Str = _DASSOCQUOT.ToString(Program.Date_Format_String, CultureInfo.CurrentCulture);
            }
        }

        public String   DASSOCQUOT_Str
        {
            get { return _DASSOCQUOT_Str; }
            set { _DASSOCQUOT_Str = value; }
        }

        #endregion  QUOTAS

        #region     INFRAEST

        public String   INFRAEST
        {
            get
            {
                return _INFRAEST;
            }
            set
            {
                _INFRAEST = value;
                _HasINFRAEST = Program.ConvertYesOrNoToBoolean(value);
            }
        }

        public Boolean  HasINFRAEST
        {
            get
            {
                return _HasINFRAEST;
            }
            set
            {
                _HasINFRAEST = value;
                _INFRAEST = Program.ConvertBooleanToYesOrNo(value);
            }
        }

        public String   INFRADESC
        {
            get
            {
                return _INFRADESC;
            }
            set
            {
                _INFRADESC = value;
                if (_INFRADESC.Length > _Max_Length_INFRADESC)
                {
                    _INFRADESC = _INFRADESC.Substring(0, _Max_Length_INFRADESC - _Max_Length_End_Space_Chars - _Max_Length_End_Reticencias.Length);
                    if ((_INFRADESC.Length + _Max_Length_End_Reticencias.Length) < _Max_Length_INFRADESC)
                        _INFRADESC += _Max_Length_End_Reticencias;
                }
            }
        }

        public Double   INFRAVAL
        {
            get
            {
                return _INFRAVAL;
            }
            set
            {
                if (value < Program.DB_Min_Pay_Value || value > Program.DB_Max_Pay_Value)
                {
                    MessageBox.Show("Valor inválido!", "set > AMFCCashPayment > INFRAVAL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                _INFRAVAL = value;
                _INFRAVAL_Str = Program.SetPayCurrencyEuroStringValue(_INFRAVAL);
                HasINFRAEST = true;
            }
        }

        public String   INFRAVAL_Str
        {
            get { return _INFRAVAL_Str; }
            set { _INFRAVAL_Str = value; }
        }

        public Boolean  IsASSOCNFRA
        {
            get
            {
                return _IsASSOCNFRA;
            }
            set
            {
                _IsASSOCNFRA = value;
                _ASSOCNFRA = Program.ConvertBooleanToYesOrNo(value);
            }
        }

        public String   ASSOCNFRA
        {
            get { return _ASSOCNFRA; }
            set
            {
                _ASSOCNFRA = value;
                _IsASSOCNFRA = Program.ConvertYesOrNoToBoolean(value);
            }
        }

        public DateTime DASSOCINFR
        {
            get
            {
                return _DASSOCINFR;
            }
            set
            {
                _DASSOCINFR = value;
                if (!Program.IsValidDateTime(value))
                    return;
                _DASSOCINFR_Str = _DASSOCINFR.ToString(Program.Date_Format_String, CultureInfo.CurrentCulture);
            }
        }

        public String   DASSOCINFR_Str
        {
            get { return _DASSOCINFR_Str; }
            set
            {
                if (String.IsNullOrEmpty(value))
                    return;
                _DASSOCINFR_Str = value;
                _DASSOCINFR = Program.ConvertToValidDateTime(_DASSOCINFR_Str);
            }
        }

        #endregion  INFRAEST

        #region     CEDENCIAS

        public String CEDENCIAS
        {
            get
            {
                return _CEDENCIAS;
            }
            set
            {
                _CEDENCIAS = value;
                _HasCEDENCIAS = Program.ConvertYesOrNoToBoolean(value);
            }
        }

        public Boolean HasCEDENCIAS
        {
            get
            {
                return _HasCEDENCIAS;
            }
            set
            {
                _HasCEDENCIAS = value;
                _CEDENCIAS = Program.ConvertBooleanToYesOrNo(value);
            }
        }

        public String CEDENCDESC
        {
            get
            {
                return _CEDENCDESC;
            }
            set
            {
                _CEDENCDESC = value;
                if (_CEDENCDESC.Length > _Max_Length_CEDENCDESC)
                {
                    _CEDENCDESC = _CEDENCDESC.Substring(0, _Max_Length_CEDENCDESC - _Max_Length_End_Space_Chars - _Max_Length_End_Reticencias.Length);
                    if ((_CEDENCDESC.Length + _Max_Length_End_Reticencias.Length) < _Max_Length_CEDENCDESC)
                        _CEDENCDESC += _Max_Length_End_Reticencias;
                }
            }
        }

        public Double CEDENCVAL
        {
            get
            {
                return _CEDENCVAL;
            }
            set
            {
                if (value < Program.DB_Min_Pay_Value || value > Program.DB_Max_Pay_Value)
                {
                    MessageBox.Show("Valor inválido!", "set > AMFCCashPayment > CEDENCVAL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                _CEDENCVAL = value;
                _CEDENCVAL_Str = Program.SetPayCurrencyEuroStringValue(_CEDENCVAL);
                HasCEDENCIAS = true;
            }
        }

        public String CEDENCVAL_Str
        {
            get { return _CEDENCVAL_Str; }
            set { _CEDENCVAL_Str = value; }
        }

        public Boolean IsASSOCCEDEN
        {
            get
            {
                return _IsASSOCCEDEN;
            }
            set
            {
                _IsASSOCCEDEN = value;
                _ASSOCCEDEN = Program.ConvertBooleanToYesOrNo(value);
            }
        }

        public String ASSOCCEDEN
        {
            get { return _ASSOCCEDEN; }
            set
            {
                _ASSOCCEDEN = value;
                _IsASSOCCEDEN = Program.ConvertYesOrNoToBoolean(value);
            }
        }

        public DateTime DASSOCCEDE
        {
            get
            {
                return _DASSOCCEDE;
            }
            set
            {
                _DASSOCCEDE = value;
                if (!Program.IsValidDateTime(value))
                    return;
                _DASSOCCEDE_Str = _DASSOCCEDE.ToString(Program.Date_Format_String, CultureInfo.CurrentCulture);
            }
        }

        public String DASSOCCEDE_Str
        {
            get { return _DASSOCCEDE_Str; }
            set
            {
                if (String.IsNullOrEmpty(value))
                    return;
                _DASSOCCEDE_Str = value;
                _DASSOCCEDE = Program.ConvertToValidDateTime(_DASSOCCEDE_Str);
            }
        }

        #endregion  CEDENCIAS

        #region     ESGOT

        public String ESGOT
        {
            get
            {
                return _ESGOT;
            }
            set
            {
                _ESGOT = value;
                _HasESGOT = Program.ConvertYesOrNoToBoolean(value);
            }
        }

        public Boolean HasESGOT
        {
            get
            {
                return _HasESGOT;
            }
            set
            {
                _HasESGOT = value;
                _ESGOT = Program.ConvertBooleanToYesOrNo(value);
            }
        }

        public String ESGOTDESC
        {
            get
            {
                return _ESGOTDESC;
            }
            set
            {
                _ESGOTDESC = value;
                if (_ESGOTDESC.Length > _Max_Length_ESGOTDESC)
                {
                    _ESGOTDESC = _ESGOTDESC.Substring(0, _Max_Length_ESGOTDESC - _Max_Length_End_Space_Chars - _Max_Length_End_Reticencias.Length);
                    if ((_ESGOTDESC.Length + _Max_Length_End_Reticencias.Length) < _Max_Length_ESGOTDESC)
                        _ESGOTDESC += _Max_Length_End_Reticencias;
                }
            }
        }

        public Double ESGOTVAL
        {
            get
            {
                return _ESGOTVAL;
            }
            set
            {
                if (value < Program.DB_Min_Pay_Value || value > Program.DB_Max_Pay_Value)
                {
                    MessageBox.Show("Valor inválido!", "set > AMFCCashPayment > ESGOTVAL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                _ESGOTVAL = value;
                _ESGOTVAL_Str = Program.SetPayCurrencyEuroStringValue(_ESGOTVAL);
                HasESGOT = true;
            }
        }

        public String ESGOTVAL_Str
        {
            get { return _ESGOTVAL_Str; }
            set { _ESGOTVAL_Str = value; }
        }

        public Boolean IsASSOCESGOT
        {
            get
            {
                return _IsASSOCESGOT;
            }
            set
            {
                _IsASSOCESGOT = value;
                _ASSOCESGOT = Program.ConvertBooleanToYesOrNo(value);
            }
        }

        public String ASSOCESGOT
        {
            get { return _ASSOCESGOT; }
            set
            {
                _ASSOCESGOT = value;
                _IsASSOCESGOT = Program.ConvertYesOrNoToBoolean(value);
            }
        }

        public DateTime DASSOCESGO
        {
            get
            {
                return _DASSOCESGO;
            }
            set
            {
                _DASSOCESGO = value;
                if (!Program.IsValidDateTime(value))
                    return;
                _DASSOCESGO_Str = _DASSOCESGO.ToString(Program.Date_Format_String, CultureInfo.CurrentCulture);
            }
        }

        public String DASSOCESGO_Str
        {
            get { return _DASSOCESGO_Str; }
            set
            {
                if (String.IsNullOrEmpty(value))
                    return;
                _DASSOCESGO_Str = value;
                _DASSOCESGO = Program.ConvertToValidDateTime(_DASSOCESGO_Str);
            }
        }

        #endregion  ESGOT

        #region     RECONV

        public String   RECONV
        {
            get
            {
                return _RECONV;
            }
            set
            {
                _RECONV = value;
                _HasRECONV = Program.ConvertYesOrNoToBoolean(value);
            }
        }

        public Boolean  HasRECONV
        {
            get
            {
                return _HasRECONV;
            }
            set
            {
                _HasRECONV = value;
                _RECONV = Program.ConvertBooleanToYesOrNo(value);
            }
        }

        public String   RECONDESC
        {
            get
            {
                return _RECONDESC;
            }
            set
            {
                _RECONDESC = value;
                if (_RECONDESC.Length > _Max_Length_RECONDESC)
                {
                    _RECONDESC = _RECONDESC.Substring(0, _Max_Length_RECONDESC - _Max_Length_End_Space_Chars - _Max_Length_End_Reticencias.Length);
                    if ((_RECONDESC.Length + _Max_Length_End_Reticencias.Length) < _Max_Length_RECONDESC)
                        _RECONDESC += _Max_Length_End_Reticencias;
                }
            }
        }

        public Double   RECONVAL
        {
            get
            {
                return _RECONVAL;
            }
            set
            {
                if (value < Program.DB_Min_Pay_Value || value > Program.DB_Max_Pay_Value)
                {
                    MessageBox.Show("Valor inválido!", "set > AMFCCashPayment > RECONVAL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                _RECONVAL = value;
                _RECONVAL_Str = Program.SetPayCurrencyEuroStringValue(_RECONVAL);
                HasRECONV = true;
            }
        }

        public String   RECONVAL_Str
        {
            get { return _RECONVAL_Str; }
            set { _RECONVAL_Str = value; }
        }

        public Boolean  IsASSOCRECON
        {
            get { return _IsASSOCRECON; }
            set
            {
                _IsASSOCRECON = value;
                _ASSOCRECON = Program.ConvertBooleanToYesOrNo(value);
            }
        }

        public String   ASSOCRECON
        {
            get
            {
                return _ASSOCRECON;
            }
            set
            {
                _ASSOCRECON = value;
                _IsASSOCRECON = Program.ConvertYesOrNoToBoolean(value);
            }
        }

        public DateTime DASSOCRECO
        {
            get
            {
                return _DASSOCRECO;
            }
            set
            {
                _DASSOCRECO = value;
                if (!Program.IsValidDateTime(value))
                    return;
                _DASSOCRECO_Str = _DASSOCRECO.ToString(Program.Date_Format_String, CultureInfo.CurrentCulture);
            }
        }

        public String   DASSOCRECO_Str
        {
            get { return _DASSOCRECO_Str; }
            set
            {
                if (String.IsNullOrEmpty(value))
                    return;
                _DASSOCRECO_Str = value;
                _DASSOCRECO = Program.ConvertToValidDateTime(_DASSOCRECO_Str);
            }
        }

        #endregion      RECONV
        
        #region     OUTROS

        public String   OUTRO
        {
            get
            {
                return _OUTRO;
            }
            set
            {
                _OUTRO = value;
                _HasOUTRO = Program.ConvertYesOrNoToBoolean(value);
            }
        }

        public Boolean  HasOUTRO
        {
            get
            {
                return _HasOUTRO;
            }
            set
            {
                _HasOUTRO = value;
                _OUTRO = Program.ConvertBooleanToYesOrNo(value);
            }
        }

        public String   OUTROSDESC
        {
            get
            {
                return _OUTROSDESC;
            }
            set
            {
                _OUTROSDESC = value;
                if (_OUTROSDESC.Length > _Max_Length_OUTROSDESC)
                {
                    _OUTROSDESC = _OUTROSDESC.Substring(0, _Max_Length_OUTROSDESC - _Max_Length_End_Space_Chars - _Max_Length_End_Reticencias.Length);
                    if ((_OUTROSDESC.Length + _Max_Length_End_Reticencias.Length) < _Max_Length_OUTROSDESC)
                        _OUTROSDESC += _Max_Length_End_Reticencias;
                }
            }
        }

        public Double   OUTROSVAL
        {
            get
            {
                return _OUTROSVAL;
            }
            set
            {
                if (value < Program.DB_Min_Pay_Value || value > Program.DB_Max_Pay_Value)
                {
                    MessageBox.Show("Valor inválido!", "set > AMFCCashPayment > OUTROSVAL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                _OUTROSVAL = value;
                _OUTROSVAL_Str = Program.SetPayCurrencyEuroStringValue(_OUTROSVAL);
                HasOUTRO = true;
            }
        }

        public String   OUTROSVAL_Str
        {
            get { return _OUTROSVAL_Str; }
            set { _OUTROSVAL_Str = value; }
        }

        public Boolean  IsASSOCOUTRO
        {
            get
            {
                return _IsASSOCOUTRO;
            }
            set
            {
                _IsASSOCOUTRO = value;
                _ASSOCOUTRO = Program.ConvertBooleanToYesOrNo(value);
            }
        }

        public String   ASSOCOUTRO
        {
            get { return _ASSOCOUTRO; }
            set
            {
                _ASSOCOUTRO = value;
                IsASSOCOUTRO = Program.ConvertYesOrNoToBoolean(value);
            }
        }

        public DateTime DASSOCOUTR
        {
            get
            {
                return _DASSOCOUTR;
            }
            set
            {
                _DASSOCOUTR = value;
                if (!Program.IsValidDateTime(value))
                    return;
                _DASSOCOUTR_Str = _DASSOCOUTR.ToString(Program.Date_Format_String, CultureInfo.CurrentCulture);
            }
        }

        public String   DASSOCOUTR_Str
        {
            get { return _DASSOCOUTR_Str; }
            set
            {
                if (String.IsNullOrEmpty(value))
                    return;
                _DASSOCOUTR_Str = value;
                _DASSOCOUTR = Program.ConvertToValidDateTime(_DASSOCOUTR_Str);
            }
        }

        #endregion  OUTROS

        #region     Entidade

        public String Entidade
        {
            get
            {
                return _Entidade;
            }
            set
            {
                _Entidade = value;
                _HasEntidade = Program.ConvertYesOrNoToBoolean(value);
            }
        }

        public Boolean HasEntidade
        {
            get
            {
                return _HasEntidade;
            }
            set
            {
                _HasEntidade = value;
                _Entidade = Program.ConvertBooleanToYesOrNo(value);
            }
        }

        public String EntidadeDESC
        {
            get
            {
                return _EntidadeDESC;
            }
            set
            {
                _EntidadeDESC = value;
                if (_EntidadeDESC.Length > _Max_Length_EntidadeDESC)
                {
                    _EntidadeDESC = _EntidadeDESC.Substring(0, _Max_Length_EntidadeDESC - _Max_Length_End_Space_Chars - _Max_Length_End_Reticencias.Length);
                    if ((_EntidadeDESC.Length + _Max_Length_End_Reticencias.Length) < _Max_Length_EntidadeDESC)
                        _EntidadeDESC += _Max_Length_End_Reticencias;
                }
            }
        }

        public Double EntidadeVAL
        {
            get
            {
                return _EntidadeVAL;
            }
            set
            {
                if (value < Program.DB_Min_Pay_Value || value > Program.DB_Max_Pay_Value)
                {
                    MessageBox.Show("Valor inválido!", "set > AMFCCashPayment > EntidadeVAL", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                _EntidadeVAL = value;
                _EntidadeVAL_Str = Program.SetPayCurrencyEuroStringValue(_EntidadeVAL);
                HasEntidade = true;
            }
        }

        public String EntidadeVAL_Str
        {
            get { return _EntidadeVAL_Str; }
            set { _EntidadeVAL_Str = value; }
        }

        public Boolean IsASSOCEntidade
        {
            get
            {
                return _IsASSOCEntidade;
            }
            set
            {
                _IsASSOCEntidade = value;
                _ASSOCEntidade = Program.ConvertBooleanToYesOrNo(value);
            }
        }

        public String ASSOCEntidade
        {
            get { return _ASSOCEntidade; }
            set
            {
                _ASSOCEntidade = value;
                _IsASSOCEntidade = Program.ConvertYesOrNoToBoolean(value);
            }
        }

        public DateTime DASSOCEntidade
        {
            get
            {
                return _DASSOCEntidade;
            }
            set
            {
                _DASSOCEntidade = value;
                if (!Program.IsValidDateTime(value))
                    return;
                _DASSOCEntidade_Str = _DASSOCEntidade.ToString(Program.Date_Format_String, CultureInfo.CurrentCulture);
            }
        }

        public String DASSOCEntidade_Str
        {
            get { return _DASSOCEntidade_Str; }
            set
            {
                if (String.IsNullOrEmpty(value))
                    return;
                _DASSOCEntidade_Str = value;
                _DASSOCEntidade = Program.ConvertToValidDateTime(_DASSOCEntidade_Str);
            }
        }

        #endregion  Entidade

        #endregion  Properties

        #region     Methods

        #region     Set TOTAL VALOR

        public Double GetAllPaymentsTotal()
        {
            try
            {
                Double dAllPaymentsTotal = 0;
                dAllPaymentsTotal = (_JOIAVAL > 0)      ? dAllPaymentsTotal + _JOIAVAL      : dAllPaymentsTotal;
                dAllPaymentsTotal = (_QUOTASVAL > 0)    ? dAllPaymentsTotal + _QUOTASVAL    : dAllPaymentsTotal;
                dAllPaymentsTotal = (_INFRAVAL > 0)     ? dAllPaymentsTotal + _INFRAVAL     : dAllPaymentsTotal;
                dAllPaymentsTotal = (_CEDENCVAL > 0)    ? dAllPaymentsTotal + _CEDENCVAL    : dAllPaymentsTotal;
                dAllPaymentsTotal = (_ESGOTVAL > 0)     ? dAllPaymentsTotal + _ESGOTVAL     : dAllPaymentsTotal;
                dAllPaymentsTotal = (_RECONVAL > 0)     ? dAllPaymentsTotal + _RECONVAL     : dAllPaymentsTotal;
                dAllPaymentsTotal = (_OUTROSVAL > 0)    ? dAllPaymentsTotal + _OUTROSVAL    : dAllPaymentsTotal;

                dAllPaymentsTotal = (_EntidadeVAL > 0) ? dAllPaymentsTotal + _EntidadeVAL : dAllPaymentsTotal;
                return dAllPaymentsTotal;
            }
            catch { return 0; }
        }

        public Double GetAllPaymentsTotal(PaymentTypes eExceptPayType)
        {
            try
            {
                Double dAllPaymentsTotal = 0;
                if (eExceptPayType != PaymentTypes.JOIA)
                    dAllPaymentsTotal = (_JOIAVAL > 0) ? dAllPaymentsTotal + _JOIAVAL : dAllPaymentsTotal;
                if (eExceptPayType != PaymentTypes.QUOTAS)
                    dAllPaymentsTotal = (_QUOTASVAL > 0) ? dAllPaymentsTotal + _QUOTASVAL : dAllPaymentsTotal;
                if (eExceptPayType != PaymentTypes.INFRAS)
                    dAllPaymentsTotal = (_INFRAVAL > 0) ? dAllPaymentsTotal + _INFRAVAL : dAllPaymentsTotal;
                if (eExceptPayType != PaymentTypes.CEDENC)
                    dAllPaymentsTotal = (_CEDENCVAL > 0) ? dAllPaymentsTotal + _CEDENCVAL : dAllPaymentsTotal;
                if (eExceptPayType != PaymentTypes.ESGOT)
                    dAllPaymentsTotal = (_ESGOTVAL > 0) ? dAllPaymentsTotal + _ESGOTVAL : dAllPaymentsTotal;
                if (eExceptPayType != PaymentTypes.RECONV)
                    dAllPaymentsTotal = (_RECONVAL > 0) ? dAllPaymentsTotal + _RECONVAL : dAllPaymentsTotal;
                if (eExceptPayType != PaymentTypes.OUTRO)
                    dAllPaymentsTotal = (_OUTROSVAL > 0) ? dAllPaymentsTotal + _OUTROSVAL : dAllPaymentsTotal;

                return dAllPaymentsTotal;
            }
            catch { return 0; }
        }

        public void SetTotalValue(Double dTotalValue)
        {
            try
            {
                VALOR = dTotalValue;
            }
            catch { return; }
        }

        public void SetTotalValue()
        {
            try
            {
                VALOR = GetAllPaymentsTotal();
            }
            catch { return; }
        }

        #endregion  Set TOTAL VALOR

        #region     Set ITEM PAYMENT VALOR

        public void SetItemPayment(PaymentTypes ePaymentType, Double dItemValue, Boolean bCheckTotals, Boolean bSetOtherValue)
        {
            try
            {
                this.Payment_Type = ePaymentType;
                Set_Item_Pay_Value(ePaymentType, dItemValue);
                this.VALOR = GetAllPaymentsTotal();
                if (bCheckTotals)
                {
                    if (this.VALOR > 0)
                    {
                        Double dOtherPaymentsTotal = GetAllPaymentsTotal(ePaymentType);
                        if (dItemValue + dOtherPaymentsTotal > VALOR)
                        {
                            if (bSetOtherValue && OUTROSVAL > 0)
                            {
                                Double dAllTotalExceptTOther = GetAllPaymentsTotal(PaymentTypes.OUTRO);
                                if (VALOR >= dAllTotalExceptTOther)
                                    OUTROSVAL = VALOR - dAllTotalExceptTOther;
                            }
                        }
                        else
                        {
                            if (bSetOtherValue)
                            {
                                Double dAllTotal = GetAllPaymentsTotal();
                                if (OUTROSVAL <= 0 && VALOR > dAllTotal)
                                    OUTROSVAL = VALOR - dAllTotal;
                            }
                        }
                    }
                }
            }
            catch { return; }
        }

        /// <versions>07-12-2017(GesAMFC-v0.0.5.1)</versions>
        private void Set_Item_Pay_Value(PaymentTypes ePaymentType, Double dItemValue)
        {
            try
            {
                switch (ePaymentType)
                {
                    case PaymentTypes.JOIA:         //1
                        JOIAVAL     = dItemValue;
                        break;

                    case PaymentTypes.QUOTAS:       //2
                        QUOTASVAL   = dItemValue;
                        break;

                    case PaymentTypes.INFRAS:       //3
                        INFRAVAL    = dItemValue;
                        break;

                    case PaymentTypes.RECONV:       //4
                        RECONVAL    = dItemValue;
                        break;

                    case PaymentTypes.CEDENC:       //5
                        CEDENCVAL   = dItemValue;
                        break;

                    case PaymentTypes.ESGOT:        //6
                        ESGOTVAL = dItemValue;
                        break;

                    case PaymentTypes.OUTRO:        //7
                        OUTROSVAL   = dItemValue;
                        break;
                }
            }
            catch { return; }
        }

        #endregion  Set ITEM PAYMENT VALOR

        #region     Payment State

        public String GetPaymentStateDBvalue()
        {
            try
            {
                return Get_Payment_State_DB_value(_Payment_State);
            }
            catch { return String.Empty; }
        }

        public String GetPaymentStateDBvalue(PaymentState ePaymentState)
        {
            try
            {
                return Get_Payment_State_DB_value(ePaymentState);
            }
            catch { return String.Empty; }
        }

        public String Get_Payment_State_DB_value(PaymentState ePaymentState)
        {
            try
            {
                String sPaymentStateDesc = Enum.GetName(typeof(PaymentState), ePaymentState);
                if (String.IsNullOrEmpty(sPaymentStateDesc))
                    return "";
                String sPaymentStateDbValue = sPaymentStateDesc.Trim().Substring(0, 1).ToUpper();
                return sPaymentStateDbValue;
            }
            catch { return String.Empty; }
        }

        public String GetPaymentStateDesc()
        {
            try
            {
                return Get_Payment_State_Desc(_Payment_State);
            }
            catch { return String.Empty; }
        }

        public String GetPaymentStateDesc(PaymentState ePaymentState)
        {
            try
            {
                return Get_Payment_State_Desc(ePaymentState);
            }
            catch { return String.Empty; }
        }

        public PaymentState GetPaymentStateType(Int32 iId)
        {
            try
            {
                if (iId < 1 || iId > 2)
                    return PaymentState.UNDEFINED;
                return (PaymentState)Enum.ToObject(typeof(PaymentState), iId);
            }
            catch { return AMFCCashPayment.PaymentState.UNDEFINED; }
        }

        public PaymentState GetPaymentStateType(String sPaymentState)
        {
            try
            {
                if (String.IsNullOrEmpty(sPaymentState.Trim()))
                    return PaymentState.UNDEFINED;
                return (PaymentState)Enum.Parse(typeof(PaymentState), sPaymentState.Trim().ToUpper());
            }
            catch { return AMFCCashPayment.PaymentState.UNDEFINED; }
        }

        private String Get_Payment_State_Desc(PaymentState ePaymentState)
        {
            try
            {
                if (ePaymentState == PaymentState.UNDEFINED)
                    return "";
                String sPaymentStateDesc = Enum.GetName(typeof(PaymentState), ePaymentState);
                return sPaymentStateDesc;
            }
            catch { return String.Empty; }
        }

        public Int32 GetPaymentStateId()
        {
            try
            {
                return Get_Payment_State_Id(_Payment_State);
            }
            catch { return -1; }
        }

        public Int32 GetPaymentStateId(PaymentState ePaymentState)
        {
            try
            {
                return Get_Payment_State_Id(ePaymentState);
            }
            catch { return -1; }
        }

        private Int32 Get_Payment_State_Id(PaymentState ePaymentState)
        {
            try
            {
                if (ePaymentState == PaymentState.UNDEFINED)
                    return -1;
                return (Int32)ePaymentState;
            }
            catch { return -1; }
        }

        public Int32 GetPaymentStateIdx()
        {
            try
            {
                return Get_Payment_State_Idx(_Payment_State);
            }
            catch { return -1; }
        }

        public Int32 GetPaymentStateIdx(PaymentState ePaymentState)
        {
            try
            {
                return Get_Payment_State_Idx(ePaymentState);
            }
            catch { return -1; }
        }

        private Int32 Get_Payment_State_Idx(PaymentState ePaymentState)
        {
            try
            {
                if (ePaymentState == PaymentState.UNDEFINED)
                    return -1;
                Int32 iId = Get_Payment_State_Id(ePaymentState);
                if (iId < 1)
                    return -1;
                return iId - 1;
            }
            catch { return -1; }
        }

        #endregion  Payment State

        #endregion  Methods

        #region     Constructor
        public AMFCCashPayment()
        {
            _Payment_Type = PaymentTypes.UNDEFINED;
            _Payment_State = PaymentState.UNDEFINED;

            Idx = 0;
            ID = -1;
            LISTARECNU = String.Empty;
            SOCIO = -1;
            NOME = String.Empty;
            DESIGNACAO = String.Empty;
            NOTAS = String.Empty;

            VALOR = Program.Default_Pay_Value;
            VALOR_Str = String.Empty;

            DATA = new DateTime();
            DATA_Str = String.Empty;
            DATAYearInt = -1;
            DATAYear = String.Empty;
            DATAMonthInt = -1;
            DATAMonth = String.Empty;
            DATAMonthYear = String.Empty;
            
            JOIA = String.Empty;
            QUOTAS = String.Empty;
            INFRAEST = String.Empty;
            CEDENCIAS = String.Empty;
            ESGOT = String.Empty;
            RECONV = String.Empty;
            OUTRO = String.Empty;
            Entidade = String.Empty;

            HasJOIA = false;
            HasQUOTAS = false;
            HasINFRAEST = false;
            HasCEDENCIAS = false;
            HasESGOT = false;
            HasRECONV = false;
            HasOUTRO = false;
            HasEntidade = false;

            JOIADESC        = String.Empty;
            QUOTASDESC      = String.Empty;
            INFRADESC       = String.Empty;
            CEDENCDESC      = String.Empty;
            ESGOTDESC       = String.Empty;
            RECONDESC       = String.Empty;
            OUTROSDESC      = String.Empty;
            EntidadeDESC = String.Empty;

            JOIAVAL = Program.Default_Pay_Value;
            QUOTASVAL = Program.Default_Pay_Value;
            INFRAVAL = Program.Default_Pay_Value;
            CEDENCVAL = Program.Default_Pay_Value;
            ESGOTVAL = Program.Default_Pay_Value;
            RECONVAL = Program.Default_Pay_Value;
            OUTROSVAL = Program.Default_Pay_Value;
            EntidadeVAL = Program.Default_Pay_Value;

            JOIAVAL_Str = String.Empty;
            QUOTASVAL_Str = String.Empty;
            INFRAVAL_Str = String.Empty;
            CEDENCVAL_Str = String.Empty;
            ESGOTVAL_Str = String.Empty;
            RECONVAL_Str = String.Empty;
            OUTROSVAL_Str = String.Empty;
            EntidadeVAL_Str = String.Empty;

            ASSOCJOIA = String.Empty;
            IsASSOCJOIA = false;
            DASSOCJOIA = new DateTime();
            DASSOCJOIA_Str = String.Empty;

            ASSOCQUOTA = String.Empty;
            IsASSOCQUOTA = false;
            DASSOCQUOT = new DateTime();
            DASSOCQUOT_Str = String.Empty;

            ASSOCNFRA = String.Empty;
            IsASSOCNFRA = false;
            DASSOCINFR = new DateTime();
            DASSOCINFR_Str = String.Empty;

            ASSOCCEDEN = String.Empty;
            IsASSOCCEDEN = false;
            DASSOCCEDE = new DateTime();
            DASSOCCEDE_Str = String.Empty;

            ASSOCESGOT = String.Empty;
            IsASSOCESGOT = false;
            DASSOCESGO = new DateTime();
            DASSOCESGO_Str = String.Empty;

            ASSOCRECON = String.Empty;
            IsASSOCRECON = false;
            DASSOCRECO = new DateTime();
            DASSOCRECO_Str = String.Empty;

            ASSOCOUTRO = String.Empty;
            IsASSOCOUTRO = false;
            DASSOCOUTR = new DateTime();
            DASSOCOUTR_Str = String.Empty;

            ASSOCEntidade = String.Empty;
            IsASSOCEntidade = false;
            DASSOCEntidade = new DateTime();
            DASSOCEntidade_Str = String.Empty;
        }
        #endregion  Constructor
    }

    #endregion  Cash Payments

    #region     Entidades

    /// <versions>25-11-2017(GesAMFC-v0.0.4.37)</versions>
    public class AMFC_Entities
    {
        public enum AMFC_Entidade_Tipo
        {
            UNDEFINED = -1,
            JOIA = 1,
            QUOTAS = 2,
            INFRAEST = 3,
            RECONV = 4,
            CEDENCIAS = 5
        }

        public List<AMFC_Entity> Entidades { get; set; }
        public AMFC_Entities()
        {
            Entidades = new List<AMFC_Entity>();
        }
        public Boolean Add(AMFC_Entity objEntity)
        {
            try
            {
                objEntity.Idx = this.Entidades.Count;
                if (Contains(objEntity))
                    return false;
                this.Entidades.Add(objEntity);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public Boolean Contains(AMFC_Entity objEntity)
        {
            try
            {
                foreach (AMFC_Entity objEntidade in this.Entidades)
                {
                    if (objEntidade.Id == objEntity.Id)
                        return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
        public Int32 GetEntidadeIndex(AMFC_Entity objEntity)
        {
            try
            {
                for (Int32 iIdx = 0; iIdx < this.Entidades.Count; iIdx++)
                {
                    AMFC_Entity objEntidade = this.Entidades[iIdx];
                    if (objEntidade.MemberNumber == objEntity.MemberNumber)
                        return iIdx;
                }
                return -1;
            }
            catch
            {
                return -1;
            }
        }
    }

    /// <versions>25-11-2017(GesAMFC-v0.0.4.37)</versions>
    public class AMFC_Entity
    {
        #region     Private Fields

        private Int64       _MemberNumber       = -1;

        private String _MemberName = "";

        private DateTime    _DtMemberAdmiDate   = new DateTime();
        private String      _MemberAdmiDate     = Program.Date_Format_String;

        private Int64       _Idx                        = -1;
        private Int64       _Id                 = -1;
        private String      _ListaCaixa         = Program.DB_Not_Available;
        private List<Int64> _ListaCaixaIDs = new List<Int64>();
        private String      _ListaRecibos = Program.DB_Not_Available;
        private List<Int64> _ListaRecibosIDs = new List<Int64>();

        private String      _ValueStr = Program.Default_Pay_String;
        private Double      _Value = Program.Default_Pay_Value;

        private DateTime    _DtDate = new DateTime();
        private String      _Date = Program.Default_Date_Str;
        private Int32       _YearInt = -1;
        private String      _Year = Program.DB_Not_Available;
        private Int32       _MonthInt = -1;
        private String      _Month = Program.DB_Not_Available;
        private String      _MonthYear = Program.DB_Not_Available;

        private DateTime    _DtDatePaid = new DateTime();
        private String      _DatePaid = Program.DB_Not_Available;
        private String      _PaidPerson = Program.DB_Not_Available;
        private DateTime    _DtDataPagamentoAgregado = new DateTime();
        private String      _DataPagamentoAgregado = Program.Default_Date_Str;
        private String      _Notas = String.Empty;


        private String _ValueToPayStr = Program.Default_Pay_String;
        private Double _ValueToPay = Program.Default_Pay_Value;

        private String _ValuePaidStr = Program.Default_Pay_String;
        private Double _ValuePaid = Program.Default_Pay_Value;

        private String _ValueMissingStr = Program.Default_Pay_String;
        private Double _ValueMissing = Program.Default_Pay_Value;

        private String _ValueOnPayingStr = Program.Default_Pay_String;
        private Double _ValueOnPaying = Program.Default_Pay_Value;

        private Int32 _AREALOTE = 0;
        private Double _PrecoMetro = Program.Default_Pay_Value;
        private Double _TotalPagar = Program.Default_Pay_Value;
        private Double _TotalPago = Program.Default_Pay_Value;
        private Double _TotalFalta = Program.Default_Pay_Value;

        private Double _CederPrecoMetro = Program.Default_Pay_Value;
        private Double _CederTotalMetros = Program.Default_Pay_Value;
        private Double _CederTotalPagar = Program.Default_Pay_Value;
        private Double _CederTotalPago = Program.Default_Pay_Value;
        private Double _CederTotalFalta = Program.Default_Pay_Value;
        private Double _CederTotalMetrosCedidios = Program.Default_Pay_Value;

        #region     Payment State

        private Boolean     _Paid = false;
        private String      _PaidOrNot = String.Empty;

        public enum PayState
        {
            UNDEFINED = -1,
            SIM = 1,
            NAO = 2,
            EM_PAGAMENTO = 3
        }

        private String _Pay_State_DB_Value = "";

        private PayState _Pay_State = PayState.UNDEFINED;

        public String Pay_State_DB_Value
        {
            get
            {
                return _Pay_State_DB_Value;
            }
            set
            {
                if (String.IsNullOrEmpty(value.Trim()) || value.Trim().Length != 1)
                    return;
                _Pay_State_DB_Value = value.Trim().ToUpper();
                _Pay_State = GetPayStateTypeByInitial(_Pay_State_DB_Value);
            }
        }

        public PayState Pay_State
        {
            get
            {
                return _Pay_State;
            }
            set
            {
                _Pay_State = value;
                if (_Pay_State != PayState.UNDEFINED)
                    _Pay_State_DB_Value = GetPay_StateDBvalue(_Pay_State);
            }
        }

        #endregion  Payment State

        #endregion  Private Fields

        #region     Properties

        public Int64 MemberNumber
        {
            get { return _MemberNumber; }
            set
            {
                if (value < 1 || value > new AMFCMember().MaxNumber)
                    return;
                _MemberNumber = value;
            }
        }

        public String MemberName
        {
            get { return _MemberName; }
            set { _MemberName = value; }
        }

        public DateTime DtMemberAdmiDate
        {
            get { return _DtMemberAdmiDate; }
            set
            {
                if (!Program.IsValidDateTime(value))
                    return;
                _DtMemberAdmiDate = value;
                _MemberAdmiDate = _DtMemberAdmiDate.ToString(Program.Date_Format_String, CultureInfo.CurrentCulture);
            }
        }

        public String MemberAdmiDate
        {
            get { return _MemberAdmiDate; }
            set
            {
                if (String.IsNullOrEmpty(value))
                    return;
                _MemberAdmiDate = value;
                _DtMemberAdmiDate = Program.ConvertToValidDateTime(_MemberAdmiDate);
            }
        }

        public Int64 Idx
        {
            get { return _Idx; }
            set { _Idx = value; }
        }

        public Int64 Id
        {
            get { return _Id; }
            set
            {
                if (value < 1)
                    return;
                _Id = value;
            }
        }

        public String ListaCaixa
        {
            get
            {
                try
                {
                    if (String.IsNullOrEmpty(_ListaCaixa))
                        return String.Empty;
                    return _ListaCaixa;
                }
                catch
                {
                    return String.Empty;
                }
            }
            set
            {
                try
                {
                    if (String.IsNullOrEmpty(value))
                        return;
                    _ListaCaixa = value;
                    try
                    {
                        if (_ListaCaixaIDs == null || _ListaCaixaIDs.Count == 0)
                        {
                            _ListaCaixaIDs = new List<Int64>();
                            if (_ListaCaixa.Trim().ToLower().Contains(Program.DB_Not_Available.Trim().ToLower()))
                            {
                                _ListaCaixa = _ListaCaixa.Replace(Program.DB_Not_Available.Trim().ToLower(), "");
                                _ListaCaixa = _ListaCaixa.Replace(Program.DB_Not_Available.Trim().ToUpper(), "");
                            }
                            List<String> ListListaCaixa = _ListaCaixa.Split(new String[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
                            foreach (String sId in ListListaCaixa)
                            {
                                try
                                {
                                    if (!String.IsNullOrEmpty(sId.Trim()))
                                        _ListaCaixaIDs.Add(Convert.ToInt64(sId.Trim()));
                                }
                                catch { }
                            }
                        }
                    }
                    catch { }
                }
                catch
                {
                    _ListaCaixa = String.Empty;
                }
            }
        }

        public List<Int64> ListaCaixaIDs
        {
            get { return _ListaCaixaIDs; }
            set
            {
                _ListaCaixaIDs = value;
                try
                {
                    if (value == null)
                        return;
                    _ListaCaixaIDs = value;
                    try
                    {
                        if (Program.IsValidTextString(_ListaCaixa.Trim()))
                        {
                            _ListaCaixa = String.Empty;
                            foreach (Int64 lId in _ListaCaixaIDs)
                            {
                                try
                                {
                                    if (!String.IsNullOrEmpty(_ListaCaixa))
                                        _ListaCaixa += ",";
                                    _ListaCaixa += lId.ToString();
                                }
                                catch { }
                            }
                        }
                    }
                    catch { }
                }
                catch
                {
                    _ListaCaixaIDs = new List<Int64>();
                }
            }
        }

        public void AddCaixaId(Int64 lId)
        {
            try
            {
                if (lId <= 0 || ContainsCaixaId(lId))
                    return;
                _ListaCaixaIDs.Add(lId);
                if (_ListaCaixa.Trim().ToLower().Contains(Program.DB_Not_Available.Trim().ToLower()))
                {
                    _ListaCaixa = _ListaCaixa.Replace(Program.DB_Not_Available.Trim().ToLower(), "");
                    _ListaCaixa = _ListaCaixa.Replace(Program.DB_Not_Available.Trim().ToUpper(), "");
                }
                if (!String.IsNullOrEmpty(_ListaCaixa))
                    _ListaCaixa += ",";
                _ListaCaixa += lId.ToString();
            }
            catch { }
        }

        public void DelCaixaId(Int64 lId)
        {
            try
            {
                if (lId <= 0 || !ContainsCaixaId(lId))
                    return;
                _ListaCaixaIDs.Remove(lId);
                if (_ListaCaixa.Trim().ToLower().Contains(Program.DB_Not_Available.Trim().ToLower()))
                {
                    _ListaCaixa = _ListaCaixa.Replace(Program.DB_Not_Available.Trim().ToLower(), "");
                    _ListaCaixa = _ListaCaixa.Replace(Program.DB_Not_Available.Trim().ToUpper(), "");
                }
                if (_ListaCaixa.Contains("," + lId.ToString() + ","))
                    _ListaCaixa = _ListaCaixa.Replace("," + lId.ToString() + ",", ",");
                else if (_ListaCaixa.Contains(lId.ToString() + ","))
                    _ListaCaixa = _ListaCaixa.Replace(lId.ToString() + ",", "");
                else if (_ListaCaixa.Contains("," + lId.ToString() + ","))
                    _ListaCaixa = _ListaCaixa.Replace("," + lId.ToString(), "");
                else if (_ListaCaixa.Contains(lId.ToString()))
                    _ListaCaixa = _ListaCaixa.Replace(lId.ToString(), "");
            }
            catch { }
        }

        public Int64 GetLastCaixaId()
        {
            try
            {
                if (_ListaCaixaIDs == null || _ListaCaixaIDs.Count == 0)
                    return -1;
                Int64 lLastCaixaId = -1;
                foreach (Int64 lCaixaId in _ListaCaixaIDs)
                {
                    if (lCaixaId > lLastCaixaId)
                        lLastCaixaId = lCaixaId;
                }
                return lLastCaixaId;
            }
            catch { return -1; }
        }

        public Boolean ContainsCaixaId(Int64 lId)
        {
            try
            {
                return _ListaCaixaIDs.IndexOf(lId) > -1;
            }
            catch { }
            return false;
        }

        public String ListaRecibos
        {
            get
            {
                try
                {
                    if (String.IsNullOrEmpty(_ListaRecibos))
                        return String.Empty;
                    return _ListaRecibos;
                }
                catch
                {
                    return String.Empty;
                }
            }
            set
            {
                try
                {
                    if (String.IsNullOrEmpty(value))
                        return;
                    _ListaRecibos = value;
                    try
                    {
                        if (_ListaRecibosIDs == null || _ListaRecibosIDs.Count == 0)
                        {
                            _ListaRecibosIDs = new List<Int64>();
                            if (_ListaRecibos.Trim().ToLower().Contains(Program.DB_Not_Available.Trim().ToLower()))
                            {
                                _ListaRecibos = _ListaRecibos.Replace(Program.DB_Not_Available.Trim().ToLower(), "");
                                _ListaRecibos = _ListaRecibos.Replace(Program.DB_Not_Available.Trim().ToUpper(), "");
                            }
                            List<String> ListListaRecibos = _ListaRecibos.Split(new String[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
                            foreach (String sId in ListListaRecibos)
                            {
                                try
                                {
                                    if (!String.IsNullOrEmpty(sId.Trim()))
                                        _ListaRecibosIDs.Add(Convert.ToInt64(sId.Trim()));
                                }
                                catch { }
                            }
                        }
                    }
                    catch { }
                }
                catch
                {
                    _ListaRecibos = String.Empty;
                }
            }
        }

        public List<Int64> ListaRecibosIDs
        {
            get { return _ListaRecibosIDs; }
            set
            {
                _ListaRecibosIDs = value;
                try
                {
                    if (value == null)
                        return;
                    _ListaRecibosIDs = value;
                    try
                    {
                        if (Program.IsValidTextString(_ListaRecibos))
                        {
                            _ListaRecibos = String.Empty;
                            foreach (Int64 lId in _ListaRecibosIDs)
                            {
                                try
                                {
                                    if (!String.IsNullOrEmpty(_ListaRecibos))
                                        _ListaRecibos += ",";
                                    _ListaRecibos += lId.ToString();
                                }
                                catch { }
                            }
                        }
                    }
                    catch { }
                }
                catch
                {
                    _ListaRecibosIDs = new List<Int64>();
                }
            }
        }

        public void AddReciboId(Int64 lId)
        {
            try
            {
                if (lId <= 0 || ContainsReciboId(lId))
                    return;
                _ListaRecibosIDs.Add(lId);
                if (_ListaRecibos.Trim().ToLower().Contains(Program.DB_Not_Available.Trim().ToLower()))
                {
                    _ListaRecibos = _ListaRecibos.Replace(Program.DB_Not_Available.Trim().ToLower(), "");
                    _ListaRecibos = _ListaRecibos.Replace(Program.DB_Not_Available.Trim().ToUpper(), "");
                }
                if (!String.IsNullOrEmpty(_ListaRecibos))
                    _ListaRecibos += ",";
                _ListaRecibos += lId.ToString();
            }
            catch { }
        }

        public Boolean ContainsReciboId(Int64 lId)
        {
            try
            {
                return _ListaRecibosIDs.IndexOf(lId) > -1;
            }
            catch { }
            return false;
        }

        #region     Values

        #region     VALOR

        public Double Value
        {
            get
            {
                return _Value;
            }
            set
            {
                _Value = value;
                _ValueStr = Program.SetPayCurrencyEuroStringValue(_Value);
                ValueToPay = _Value;
            }
        }

        public String ValueStr
        {
            get { return _ValueStr; }
            set { _ValueStr = value; }
        }

        #endregion  VALOR

        #region     Value To Pay

        public Double ValueToPay
        {
            get
            {
                return _ValueToPay;
            }
            set
            {
                if (value < 0 || value != _Value)
                    return;
                _ValueToPay = value;
                _ValueToPayStr = Program.SetPayCurrencyEuroStringValue(_ValueToPay);
                SetPaidState();
            }
        }

        public String ValueToPayStr
        {
            get { return _ValueToPayStr; }
            set { _ValueToPayStr = value; }
        }

        public Double GetValueToPay()
        {
            try
            {
                Double dValueToPay = Program.Default_Pay_Value;
                if (Value <= 0 || ValuePaid > Value)
                    return Value;
                dValueToPay = Value - ValuePaid;
                return dValueToPay;
            }
            catch { return Program.Default_Pay_Value; }
        }

        #endregion  Value To Pay

        #region     Value Paid

        public Double ValuePaid
        {
            get
            {
                return _ValuePaid;
            }
            set
            {
                if (value < 0 || value > _Value)
                    return;
                _ValuePaid = value;
                _ValuePaidStr = Program.SetPayCurrencyEuroStringValue(_ValuePaid);
                if (_Value >= _ValuePaid)
                {
                    _ValueMissing = _Value - _ValuePaid;
                    _ValueMissingStr = Program.SetPayCurrencyEuroStringValue(_ValueMissing);
                }
                SetPaidState();
            }
        }

        public String ValuePaidStr
        {
            get { return _ValuePaidStr; }
            set { _ValuePaidStr = value; }
        }

        public Double GetValuePaid()
        {
            try
            {
                Double dValuePaid = Program.Default_Pay_Value;
                if (Value <= 0 || ValueMissing > Value)
                    return 0;
                dValuePaid = Value - ValueMissing;
                return dValuePaid;
            }
            catch { return Program.Default_Pay_Value; }
        }

        public Double AddValuePaid(Double dAditionalValuePaid)
        {
            try
            {
                Double dValuePaid = _ValuePaid;
                if (dAditionalValuePaid <= 0)
                    return 0;
                dValuePaid = _ValuePaid + dAditionalValuePaid;
                _ValuePaid = dValuePaid;
                if (_ValueToPay > _ValuePaid)
                {
                    _ValueMissing = _ValueToPay - _ValuePaid;
                    _ValueMissingStr = Program.SetPayCurrencyEuroStringValue(_ValueMissing);
                }
                else
                {
                    _ValueToPay = _Value;
                    _ValueMissing = 0;
                    _ValueOnPaying = 0;
                    _ValueMissingStr = Program.SetPayCurrencyEuroStringValue(_ValueMissing);
                    _ValueOnPayingStr = Program.SetPayCurrencyEuroStringValue(_ValueOnPaying);
                }
                return dValuePaid;
            }
            catch { return Program.Default_Pay_Value; }
        }

        public Double RemoveValuePaid(Double dRemoveValuePaid)
        {
            try
            {
                Double dValuePaid = _ValuePaid;
                if (dRemoveValuePaid <= 0 || dRemoveValuePaid > _ValuePaid)
                    return 0;
                dValuePaid = _ValuePaid - dRemoveValuePaid;
                _ValuePaid = dValuePaid;
                if (_ValueToPay > _ValuePaid)
                {
                    _ValueMissing = _ValueToPay - _ValuePaid;
                    _ValueMissingStr = Program.SetPayCurrencyEuroStringValue(_ValueMissing);
                }
                else
                {
                    _ValueToPay = _Value;
                    _ValueMissing = 0;
                    _ValueOnPaying = 0;
                    _ValueMissingStr = Program.SetPayCurrencyEuroStringValue(_ValueMissing);
                    _ValueOnPayingStr = Program.SetPayCurrencyEuroStringValue(_ValueOnPaying);
                }
                return dValuePaid;
            }
            catch { return Program.Default_Pay_Value; }
        }

        #endregion  Value Paid

        #region     Value On Paying

        public Double ValueOnPaying
        {
            get
            {
                return _ValueOnPaying;
            }
            set
            {
                if (value < 0 || value > _Value)
                    return;
                _ValueOnPaying = value;
                _ValueOnPayingStr = Program.SetPayCurrencyEuroStringValue(_ValueOnPaying);
                if (_Value >= _ValuePaid)
                {
                    _ValueMissing = _Value - _ValuePaid;
                    _ValueMissingStr = Program.SetPayCurrencyEuroStringValue(_ValueMissing);
                }
                SetPaidState();
            }
        }

        public String ValueOnPayingStr
        {
            get { return _ValueOnPayingStr; }
            set { _ValueOnPayingStr = value; }
        }

        #endregion  Value On Paying

        #region     Value Missing

        public Double ValueMissing
        {
            get
            {
                return _ValueMissing;
            }
            set
            {
                if (value < 0 || value > (_Value - _ValuePaid))
                    return;
                _ValueMissing = value;
                _ValueMissingStr = Program.SetPayCurrencyEuroStringValue(_ValueMissing);
                SetPaidState();
            }
        }

        public Double GetValueMissing()
        {
            try
            {
                Double dValueMissing = Program.Default_Pay_Value;
                if (Value <= 0 || ValuePaid > Value)
                    return Value;
                dValueMissing = Value - ValuePaid;
                return dValueMissing;
            }
            catch { return Program.Default_Pay_Value; }
        }

        public String ValueMissingStr
        {
            get { return _ValueMissingStr; }
            set { _ValueMissingStr = value; }
        }

        #endregion  Value Missing

        #endregion  Values

        #region     Paid State

        public Boolean Paid
        {
            get
            {
                SetPaidState();
                return _Paid;
            }
            set { _Paid = value; }
        }

        public String PaidOrNot
        {
            get
            {
                _PaidOrNot = _Paid ? "PAGO" : "NÃO PAGO";
                SetPaidState();
                return _PaidOrNot;
            }
            set
            {
                _PaidOrNot = _Paid ? "PAGO" : "NÃO PAGO";
            }
        }

        /// <versions>09-12-2017(GesAMFC-v1.0.0.1)</versions>
        public void SetPaidState()
        {
            if (_AREALOTE > 0 && _TotalPagar > 0 && _PrecoMetro > 0)
            {
                _Paid = Math.Round(_TotalPago, 0) == Math.Round(_TotalPagar, 0) && _TotalFalta == 0;
                _Paid = true; //DEBUG: pROVISORIO ATE TER TODO O HISTORICO DO VALOR DAS INFRA POR M2 -> Depois tirar esta linha
            }

            else if (_AREALOTE > 0 && _CederTotalPagar > 0 && _CederPrecoMetro > 0)
                _Paid = Math.Round(_CederTotalPago, 0) == Math.Round(_CederTotalPagar, 0) && _CederTotalFalta == 0;

            else
                _Paid = (_Value > 0 && _ValueToPay == _ValuePaid && _ValueOnPaying == 0 && _ValueMissing == 0);

            _Pay_State = _Paid ? PayState.SIM : ((_ValueOnPaying > 0 || this.Pay_State_DB_Value == "E") ? PayState.EM_PAGAMENTO : PayState.NAO);
        }

        #endregion  Paid State

        #region     Dates

        public DateTime DtDate
        {
            get
            {
                return _DtDate;
            }
            set
            {
                if (!Program.IsValidDateTime(value))
                {
                    _DtDate = new DateTime();
                    Date = Program.Default_Date_Str;
                }
                else
                {
                    _DtDate = value;
                    _Date = _DtDate.ToString(Program.Date_Format_String, CultureInfo.CurrentCulture);
                    YearInt = _DtDate.Year;
                    MonthInt = _DtDate.Month;
                }
            }
        }

        public String Date
        {
            get { return _Date; }
            set
            {
                if (String.IsNullOrEmpty(value))
                    return;
                _Date = value;
                _DtDate = Program.ConvertToValidDateTime(_Date);
            }
        }

        public Int32 YearInt
        {
            get { return _YearInt; }
            set
            {
                if (!Program.IsValidYear(value))
                {
                    _YearInt = -1;
                    Year = Program.DB_Not_Available;
                }
                else
                {
                    _YearInt = value;
                    Year = _YearInt.ToString();
                }
            }
        }

        public String Year
        {
            get { return _Year; }
            set { _Year = value; }
        }

        public Int32 MonthInt
        {
            get { return _MonthInt; }
            set
            {
                if (value >= 1 && value <= 12)
                {
                    _MonthInt = value;
                    if (Program.IsValidYear(_YearInt))
                        _DtDate = new DateTime(_YearInt, _MonthInt, 1);
                    _Month = "[" + _DtDate.ToString("MM", Program.CurrentCulture) + "/" + _DtDate.ToString("yy", Program.CurrentCulture) + "]" + ": " + _DtDate.ToString("MMMM", Program.CurrentCulture) + " / " + _DtDate.ToString("yyyy", Program.CurrentCulture);
                    _MonthYear = _Year + "/" + _DtDate.ToString("MMM", Program.CurrentCulture);
                }
                else
                {
                    _MonthInt = -1;
                    _Month = Program.DB_Not_Available;
                    _MonthYear = Program.DB_Not_Available;
                }
            }
        }

        public String Month
        {
            get { return _Month; }
            set { _Month = value; }
        }

        public String MonthYear
        {
            get
            {
                return _MonthYear;
            }
            set { _MonthYear = value; }
        }

        public DateTime DtDatePaid
        {
            get { return _DtDatePaid; }
            set
            {
                if (!Program.IsValidDateTime(value))
                    return;
                _DtDatePaid = value;
                _DatePaid = _DtDatePaid.ToString(Program.Date_Format_String, CultureInfo.CurrentCulture);
            }
        }

        public String DatePaid
        {
            get { return _DatePaid; }
            set
            {
                if (String.IsNullOrEmpty(value))
                    return;
                _DatePaid = value;
                _DtDatePaid = Program.ConvertToValidDateTime(_DatePaid);
            }
        }

        public DateTime DtDataPagamentoAgregado
        {
            get { return _DtDataPagamentoAgregado; }
            set
            {
                if (!Program.IsValidDateTime(value))
                    return;
                _DtDataPagamentoAgregado = value;
            }
        }

        public String DataPagamentoAgregado
        {
            get { return _DataPagamentoAgregado; }
            set
            {
                if (String.IsNullOrEmpty(value))
                    return;
                _DataPagamentoAgregado = value;
                _DtDataPagamentoAgregado = Program.ConvertToValidDateTime(_DataPagamentoAgregado);
            }
        }

        #endregion  Dates

        public String PaidPerson
        {
            get { return _PaidPerson; }
            set { _PaidPerson = value; }
        }

        public String Notas
        {
            get { return _Notas; }
            set { _Notas = value; }
        }

        public Int32 AREALOTE
        {
            get { return _AREALOTE; }
            set
            {
                if (value < 1 || value > 50000)
                    return;
                _AREALOTE = value;
                GetTotalPagar();
            }
        }

        //-----

        public Double PrecoMetro
        {
            get
            {
                return _PrecoMetro;
            }
            set
            {
                if (value < 0)
                    return;
                _PrecoMetro = value;
                GetTotalPagar();
            }
        }

        public Double TotalPagar
        {
            get
            {
                GetTotalPagar();
                return _TotalPagar;
            }
            set
            {
                if (value < 0)
                    return;
                _TotalPagar = value;
            }
        }

        public Double TotalPago
        {
            get
            {
                return _TotalPago;
            }
            set
            {
                if (value < 0)
                    return;
                _TotalPago = value;
                GetTotalFalta();
            }
        }

        public Double TotalFalta
        {
            get
            {
                if (_TotalFalta <= 0)
                {
                    GetTotalPagar();
                    GetTotalFalta();
                }
                return _TotalFalta;
            }
            set
            {
                if (value < 0)
                    return;
                _TotalFalta = value;
            }
        }

        public Double GetTotalPagar()
        {
            try
            {
                Double dbTotalPagar = 0;
                if (_AREALOTE > 0 && _PrecoMetro > 0)
                    dbTotalPagar = _AREALOTE * _PrecoMetro;
                if (dbTotalPagar > 0)
                    _TotalPagar = dbTotalPagar;
                else
                    dbTotalPagar = 0;
                return dbTotalPagar;

            }
            catch { return 0; }
        }

        public Double GetTotalFalta()
        {
            try
            {
                Double dbTotalFalta = 0;
                if (_TotalPagar > 0 && _TotalPago >= 0)
                    dbTotalFalta = _TotalPagar - _TotalPago;
                if (dbTotalFalta > 0)
                    _TotalFalta = dbTotalFalta;
                else
                    dbTotalFalta = 0;
                return dbTotalFalta;
            }
            catch { return 0; }
        }

        //-----

        public Double CederPrecoMetro
        {
            get
            {
                return _CederPrecoMetro;
            }
            set
            {
                if (value < 0)
                    return;
                _CederPrecoMetro = value;
                GetCederTotalAreaCeder();
                GetCederTotalPagar();
            }
        }


        /// <summary>
        //Até 500 m2 . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .. 25 % 
        //De 501 m2 a 1 500 m2. . . . . . . . . . . . . . . . . . . . . . . . . . 30 % 
        //De 1 501 m2 a 2 500 m2. . . . . . . . . . . . . . . . . . . . . . . . . 35 % 
        //De 2 501 m2 a 3 500 m2. . . . . . . . . . . . . . . . . . . . . . . . . 40 % 
        //De 3 501 m2 a 4 500 m2. . . . . . . . . . . . . . . . . . . . . . . . . 45 % 
        //De 4 501 m2 a 5 500 m2. . . . . . . . . . . . . . . . . . . . . . . . . 50 % 
        //De 5 501 m2 a 10 500 m2. . . . . . . . . . . . . . . . . . . . . . . .  55 % 
        //Mais de 10 500 m2. . . . . . . . . . . . . . . . . . . . . . . . . . .. 60 %
        /// </summary>
        public Double CederTotalMetros
        {
            get
            {
                return _CederTotalMetros;
            }
            set
            {
                if (value < 0)
                    return;
                _CederTotalMetros = value;
                GetCederTotalAreaCeder();
                GetCederTotalPagar();
            }
        }

        public Double CederTotalPagar
        {
            get
            {
                GetCederTotalAreaCeder();
                GetCederTotalPagar();
                return _CederTotalPagar;
            }
            set
            {
                if (value < 0)
                    return;
                _CederTotalPagar = value;
            }
        }

        public Double CederTotalPago
        {
            get
            {
                GetCederTotalAreaCeder();
                GetCederTotalPagar();
                if (_CederTotalMetrosCedidios > 0)
                    GetCederTotalFaltaByMetros();
                else
                    GetCederTotalFaltaByValor();
                return _CederTotalPago;
            }
            set
            {
                if (value < 0)
                    return;
                _CederTotalPago = value;
                GetCederTotalAreaCeder();
                GetCederTotalPagar();
            }
        }

        public Double CederTotalFalta
        {
            get
            {
                return _CederTotalFalta;
            }
            set
            {
                if (value < 0)
                    return;
                _CederTotalFalta = value;
            }
        }

        
        public Double CederTotalMetrosCedidios
        {
            get
            {
                if (_CederTotalMetrosCedidios <= 0)
                {
                    GetCederTotalAreaCeder();
                    GetCederTotalPagar();
                    GetCederTotalFaltaByMetros();
                }
                return _CederTotalMetrosCedidios;
            }
            set
            {
                if (value < 0)
                    return;
                _CederTotalMetrosCedidios = value;
                GetCederTotalAreaCeder();
                GetCederTotalPagar();
                GetCederTotalFaltaByMetros();
            }
        }

        /// <summary>
        //Até 500 m2 . . . . . . . . . . . . . . . . . . . . . . . . . . . . . .. 25 % 
        //De 501 m2 a 1 500 m2. . . . . . . . . . . . . . . . . . . . . . . . . . 30 % 
        //De 1 501 m2 a 2 500 m2. . . . . . . . . . . . . . . . . . . . . . . . . 35 % 
        //De 2 501 m2 a 3 500 m2. . . . . . . . . . . . . . . . . . . . . . . . . 40 % 
        //De 3 501 m2 a 4 500 m2. . . . . . . . . . . . . . . . . . . . . . . . . 45 % 
        //De 4 501 m2 a 5 500 m2. . . . . . . . . . . . . . . . . . . . . . . . . 50 % 
        //De 5 501 m2 a 10 500 m2. . . . . . . . . . . . . . . . . . . . . . . .  55 % 
        //Mais de 10 500 m2. . . . . . . . . . . . . . . . . . . . . . . . . . .. 60 %
        /// </summary>
        public Double GetCederTotalAreaCeder()
        {
            try
            {
                Double dbCederArea = 0;
                if (_AREALOTE <= 0)
                    return 0;
                if (_AREALOTE > 0 && _AREALOTE <= 500)
                    dbCederArea = _AREALOTE * 0.25;

                else if (_AREALOTE > 500 && _AREALOTE <= 1500)
                    dbCederArea = _AREALOTE * 0.3;

                else if (_AREALOTE > 1500 && _AREALOTE <= 2500)
                    dbCederArea = _AREALOTE * 0.35;

                else if (_AREALOTE > 2500 && _AREALOTE <= 3500)
                    dbCederArea = _AREALOTE * 0.4;

                else if (_AREALOTE > 3500 && _AREALOTE <= 4500)
                    dbCederArea = _AREALOTE * 0.45;

                else if (_AREALOTE > 4500 && _AREALOTE <= 5500)
                    dbCederArea = _AREALOTE * 0.5;

                else if (_AREALOTE > 5500 && _AREALOTE <= 10500)
                    dbCederArea = _AREALOTE * 0.55;

                else if (_AREALOTE > 10500)
                    dbCederArea = _AREALOTE * 0.6;

                dbCederArea = Math.Round(dbCederArea, 0);

                if (dbCederArea > 0)
                    _CederTotalMetros = dbCederArea;
                else
                    _CederTotalMetros = 0;
                return dbCederArea;
            }
            catch { return 0; }
        }

        public Double GetCederTotalPagar()
        {
            try
            {
                Double dbCederTotalPagar = 0;
                if (_CederTotalMetros > 0 && _CederPrecoMetro > 0)
                    dbCederTotalPagar = _CederTotalMetros * _CederPrecoMetro;
                if (dbCederTotalPagar > 0)
                    _CederTotalPagar = dbCederTotalPagar;
                else
                    _CederTotalPagar = 0;
                return dbCederTotalPagar;
            }
            catch { return 0; }
        }

        public Double GetCederTotalFaltaByValor()
        {
            try
            {
                if (Math.Round(_CederTotalMetrosCedidios, 0) == Math.Round(_CederTotalMetros, 0))
                {
                    if (_CederTotalPagar <= 0)
                        GetCederTotalPagar();
                    _CederTotalPago = _CederTotalPagar;
                    _CederTotalFalta = 0;
                    Pay_State = PayState.SIM;
                    Paid = true;
                    return 0; //jã tá tudo pago
                }
                if (_CederTotalFalta > 0 && _CederTotalFalta < _CederTotalPagar)
                    return _CederTotalFalta;
                Double dbCederTotalFalta = 0;
                if (_CederTotalPagar > 0 && _CederTotalPago >= 0)
                    dbCederTotalFalta = _CederTotalPagar - _CederTotalPago;
                dbCederTotalFalta = Math.Round(dbCederTotalFalta, 0);
                if (dbCederTotalFalta > 0)
                    _CederTotalFalta = dbCederTotalFalta;
                else
                    _CederTotalFalta = 0;
                return dbCederTotalFalta;
            }
            catch { return 0; }
        }
        
        public Double GetCederTotalFaltaByMetros()
        {
            try
            {
                if (Math.Round(_CederTotalMetrosCedidios, 0) == Math.Round(_CederTotalMetros, 0))
                {
                    if (_CederTotalPagar <= 0)
                        GetCederTotalPagar();
                    _CederTotalPago = _CederTotalPagar;
                    _CederTotalFalta = 0;
                    _Pay_State = PayState.SIM;
                    return 0; //jã tá tudo pago
                }
                if (_CederTotalFalta > 0 && _CederTotalFalta < _CederTotalPagar)
                    return _CederTotalFalta; //jã tá
                Double dbCederTotalFalta = 0;
                if (_CederTotalPagar > 0 && _CederTotalMetrosCedidios > 0 && _CederPrecoMetro > 0)
                    dbCederTotalFalta = _CederTotalPagar - (_CederTotalMetrosCedidios * _CederPrecoMetro);
                dbCederTotalFalta = Math.Round(dbCederTotalFalta, 0);
                if (dbCederTotalFalta > 0)
                    _CederTotalFalta = dbCederTotalFalta;
                else
                    _CederTotalFalta = 0;
                return dbCederTotalFalta;
            }
            catch { return 0; }
        }

        #endregion  Properties

        #region     Constructor
        public AMFC_Entity()
        {
            Idx = -1;
            Id = -1;
            ListaCaixa = String.Empty;
            ListaCaixaIDs = new List<Int64>();
            ListaRecibos = String.Empty;
            ListaRecibosIDs = new List<Int64>();
            MemberNumber = -1;
            MemberName = String.Empty;
            ValueStr = String.Empty;
            Value = Program.Default_Pay_Value;
            ValueToPay = Program.Default_Pay_Value;
            ValueToPay = Program.Default_Pay_Value;
            ValueMissing = Program.Default_Pay_Value;
            DtDate = new DateTime();
            Date = String.Empty;
            YearInt = -1;
            Year = String.Empty;
            MonthInt = -1;
            Month = String.Empty;
            MonthYear = String.Empty;

            Paid = false;
            PaidOrNot = Paid ? "Pago" : "Não Pago";
            DtDatePaid = new DateTime();
            DatePaid = String.Empty;
            PaidPerson = String.Empty;

            DtDataPagamentoAgregado = new DateTime();
            DataPagamentoAgregado = String.Empty;

            Notas = String.Empty;
        }
        #endregion  Constructor

        #region     Methods

        #region     Payment State

        public String GetPay_StateDBvalue()
        {
            try
            {
                return Get_Pay_State_DB_value(_Pay_State);
            }
            catch { return String.Empty; }
        }

        public String GetPay_StateDBvalue(PayState ePayState)
        {
            try
            {
                return Get_Pay_State_DB_value(ePayState);
            }
            catch { return String.Empty; }
        }

        public String Get_Pay_State_DB_value(PayState ePayState)
        {
            try
            {
                String sPayStateDesc = Enum.GetName(typeof(PayState), ePayState);
                if (String.IsNullOrEmpty(sPayStateDesc))
                    return "";
                String sPayStateDbValue = sPayStateDesc.Trim().Substring(0, 1).ToUpper();
                return sPayStateDbValue;
            }
            catch { return String.Empty; }
        }

        public String GetPayStateDesc()
        {
            try
            {
                return Get_Pay_State_Desc(_Pay_State);
            }
            catch { return String.Empty; }
        }

        public String GetPayStateDesc(PayState ePayState)
        {
            try
            {
                return Get_Pay_State_Desc(ePayState);
            }
            catch { return String.Empty; }
        }

        public PayState GetPayStateType(Int32 iId)
        {
            try
            {
                if (iId < 1 || iId > 2)
                    return PayState.UNDEFINED;
                return (PayState)Enum.ToObject(typeof(PayState), iId);
            }
            catch { return PayState.UNDEFINED; }
        }

        public PayState GetPayStateType(String sPayState)
        {
            try
            {
                if (String.IsNullOrEmpty(sPayState.Trim()))
                    return PayState.UNDEFINED;
                return (PayState)Enum.Parse(typeof(PayState), sPayState.Trim().ToUpper());
            }
            catch { return PayState.UNDEFINED; }
        }

        public PayState GetPayStateTypeByInitial(String sPayStateDB)
        {
            try
            {
                PayState ePayState = PayState.UNDEFINED;
                sPayStateDB = sPayStateDB.Trim().Substring(0, 1).ToUpper();
                switch (sPayStateDB)
                {
                    case "S":
                        ePayState = PayState.SIM;
                        break;
                    case "N":
                        ePayState = PayState.NAO;
                        break;
                    case "E":
                        ePayState = PayState.EM_PAGAMENTO;
                        break;
                    default:
                        ePayState = PayState.UNDEFINED;
                        break;
                }
                return ePayState;
            }
            catch { return PayState.UNDEFINED; }
        }

        private String Get_Pay_State_Desc(PayState ePayState)
        {
            try
            {
                if (ePayState == PayState.UNDEFINED)
                    return "";
                String sPayStateDesc = Enum.GetName(typeof(PayState), ePayState);
                return sPayStateDesc;
            }
            catch { return String.Empty; }
        }

        public Int32 GetPayStateId()
        {
            try
            {
                return Get_Pay_State_Id(_Pay_State);
            }
            catch { return -1; }
        }

        public Int32 GetPayStateId(PayState ePayState)
        {
            try
            {
                return Get_Pay_State_Id(ePayState);
            }
            catch { return -1; }
        }

        private Int32 Get_Pay_State_Id(PayState ePayState)
        {
            try
            {
                if (ePayState == PayState.UNDEFINED)
                    return -1;
                return (Int32)ePayState;
            }
            catch { return -1; }
        }

        public Int32 GetPayStateIdx()
        {
            try
            {
                return Get_Pay_State_Idx(_Pay_State);
            }
            catch { return -1; }
        }

        public Int32 GetPayStateIdx(PayState ePayState)
        {
            try
            {
                return Get_Pay_State_Idx(ePayState);
            }
            catch { return -1; }
        }

        private Int32 Get_Pay_State_Idx(PayState ePayState)
        {
            try
            {
                if (ePayState == PayState.UNDEFINED)
                    return -1;
                Int32 iId = Get_Pay_State_Id(ePayState);
                if (iId < 1)
                    return -1;
                return iId - 1;
            }
            catch { return -1; }
        }

        #endregion  Payment State

        #endregion  Methods
    }

    #endregion  Entidades

    #region     Pagamento Entidades

    /// <versions>21-03-2018(GesAMFC-v1.0.0.3)</versions>
    public class LIST_PAG_ENTIDADE
    {
        public List<PAG_ENTIDADE> List { get; set; }

        public LIST_PAG_ENTIDADE()
        {
            List = new List<PAG_ENTIDADE>();
        }
        public Boolean Add(PAG_ENTIDADE objEnt)
        {
            try
            {
                if (Contains(objEnt))
                    return false;
                this.List.Add(objEnt);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public Boolean Contains(PAG_ENTIDADE objEnt)
        {
            try
            {
                foreach (PAG_ENTIDADE objC in this.List)
                {
                    if (objC.ID == objEnt.ID)
                        return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
        public Int32 GetEntidadeIndex(PAG_ENTIDADE objEnt)
        {
            try
            {
                for (Int32 iIdx = 0; iIdx < this.List.Count; iIdx++)
                {
                    PAG_ENTIDADE objC = this.List[iIdx];
                    if (objC.ID == objEnt.ID)
                        return iIdx;
                }
                return -1;
            }
            catch
            {
                return -1;
            }
        }
    }

    /// <versions>21-03-2018(GesAMFC-v1.0.0.3)</versions>
    public class EntityTypeConfigs
    {
        public PAG_ENTIDADE.EntityTypes Entidade_Tipo { get; set; }
        public String DBF_File_Name { get; set; }
        public AMFCCashPayment.PaymentTypes Pagamento_Tipo { get; set; }
        public Double Entity_Value_Meter { get; set; }
        
        public String Entity_Desc_Single { get; set; }
        public String Entity_Lower_Single { get; set; }
        public String Entity_Upper_Single { get; set; }
        public String Entity_Desc_Plural { get; set; }
        public String Entity_Lower_Plural { get; set; }
        public String Entity_Upper_Plural { get; set; }
        public String Entity_Abbr_Lower { get; set; }
        public String Entity_Abbr_Upper { get; set; }
        public String Entity_Desc_Short_Single { get; set; }
        public String Entity_Lower_Short_Single { get; set; }
        public String Entity_Upper_Short_Single { get; set; }
        public String Entity_Desc_Short_Plural { get; set; }
        public String Entity_Lower_Short_Plural { get; set; }
        public String Entity_Upper_Short_Plural { get; set; }

        public EntityTypeConfigs()
        {
            Entidade_Tipo = PAG_ENTIDADE.EntityTypes.UNDEF;
            Pagamento_Tipo = AMFCCashPayment.PaymentTypes.UNDEFINED;
            DBF_File_Name = "";
            Entity_Value_Meter = 0;

            Entity_Desc_Single = "";
            Entity_Lower_Single = "";
            Entity_Upper_Single = "";
            Entity_Desc_Plural = "";
            Entity_Lower_Plural = "";
            Entity_Upper_Plural = "";
            Entity_Abbr_Lower = "";
            Entity_Abbr_Upper = "";
            Entity_Desc_Short_Single = "";
            Entity_Lower_Short_Single = "";
            Entity_Upper_Short_Single = "";
            Entity_Desc_Short_Plural = "";
            Entity_Lower_Short_Plural = "";
            Entity_Upper_Short_Plural = "";
        }
    }

    /// <versions>21-03-2018(GesAMFC-v1.0.0.3)</versions>
    /// <remarks>
    /// ID,N,10,0	LISTACAIXA,C,20	    LISTARECNU,C,20	    IDLOTE,N,5,0	NUMLOTE,C,10	SOCIO,N,5,0	    NOME,C,70	ANO,N,4,0	MES,N,2,0	DATAPAG,D	DESIGNACAO,C,140	PRECOM2P,N,12,3	    AREAPAGO,N,12,2	    VALORPAGO,N,12,3	ESTADO,C,1	NOTAS,C,140																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																																
    /// 01: ID,N,10,0	
    /// 02: LISTACAIXA,C,20	
    /// 03: LISTARECNU,C,20	
    /// 04: IDLOTE,N,5,0	
    /// 05: NUMLOTE,C,10	
    /// 06: SOCIO,N,5,0	
    /// 07: NOME,C,70	
    /// 08: ANO,N,4,0	
    /// 09: MES,N,2,0	
    /// 10: DATAPAG,D	
    /// 11: DESIGNACAO,C,140	
    /// 12: PRECOM2P,N,12,3	
    /// 13: AREAPAGO,N,12,2	
    /// 14: VALORPAGO,N,12,3	
    /// 15: ESTADO,C,1	
    /// 16: NOTAS,C,140	
    /// 17: PAGNBR,N,2,0
    /// </remarks>
    public class PAG_ENTIDADE
    {
        #region     Properties

        private Library_AMFC_Methods LibAMFC;

        private EntityTypeConfigs Entidade_Configs;
        
        public Int64 ID { get; set; } /// 01: ID,N,10,0	

        #region     02: LISTACAIXA,C,20	
        private String _ListaCaixa = String.Empty;
        private List<Int64> _ListaCaixaIDs = new List<Int64>();
        
        public String LISTACAIXA
        {
            get
            {
                try
                {
                    if (String.IsNullOrEmpty(_ListaCaixa))
                        return String.Empty;
                    return _ListaCaixa;
                }
                catch
                {
                    return String.Empty;
                }
            }
            set
            {
                try
                {
                    if (String.IsNullOrEmpty(value))
                        return;
                    _ListaCaixa = value;
                    try
                    {
                        if (_ListaCaixaIDs == null || _ListaCaixaIDs.Count == 0)
                        {
                            _ListaCaixaIDs = new List<Int64>();
                            if (_ListaCaixa.Trim().ToLower().Contains(Program.DB_Not_Available.Trim().ToLower()))
                            {
                                _ListaCaixa = _ListaCaixa.Replace(Program.DB_Not_Available.Trim().ToLower(), "");
                                _ListaCaixa = _ListaCaixa.Replace(Program.DB_Not_Available.Trim().ToUpper(), "");
                            }
                            List<String> ListListaCaixa = _ListaCaixa.Split(new String[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
                            foreach (String sId in ListListaCaixa)
                            {
                                try
                                {
                                    if (!String.IsNullOrEmpty(sId.Trim()))
                                        _ListaCaixaIDs.Add(Convert.ToInt64(sId.Trim()));
                                }
                                catch { }
                            }
                        }
                    }
                    catch { }
                }
                catch
                {
                    _ListaCaixa = String.Empty;
                }
            }
        }

        public List<Int64> ListaCaixaIDs
        {
            get { return _ListaCaixaIDs; }
            set
            {
                _ListaCaixaIDs = value;
                try
                {
                    if (value == null)
                        return;
                    _ListaCaixaIDs = value;
                    try
                    {
                        if (Program.IsValidTextString(_ListaCaixa.Trim()))
                        {
                            _ListaCaixa = String.Empty;
                            foreach (Int64 lId in _ListaCaixaIDs)
                            {
                                try
                                {
                                    if (!String.IsNullOrEmpty(_ListaCaixa))
                                        _ListaCaixa += ",";
                                    _ListaCaixa += lId.ToString();
                                }
                                catch { }
                            }
                        }
                    }
                    catch { }
                }
                catch
                {
                    _ListaCaixaIDs = new List<Int64>();
                }
            }
        }

        public void AddCaixaId(Int64 lId)
        {
            try
            {
                if (lId <= 0 || ContainsCaixaId(lId))
                    return;
                _ListaCaixaIDs.Add(lId);
                if (_ListaCaixa.Trim().ToLower().Contains(Program.DB_Not_Available.Trim().ToLower()))
                {
                    _ListaCaixa = _ListaCaixa.Replace(Program.DB_Not_Available.Trim().ToLower(), "");
                    _ListaCaixa = _ListaCaixa.Replace(Program.DB_Not_Available.Trim().ToUpper(), "");
                }
                if (!String.IsNullOrEmpty(_ListaCaixa))
                    _ListaCaixa += ",";
                _ListaCaixa += lId.ToString();
            }
            catch { }
        }

        public void DelCaixaId(Int64 lId)
        {
            try
            {
                if (lId <= 0 || !ContainsCaixaId(lId))
                    return;
                _ListaCaixaIDs.Remove(lId);
                if (_ListaCaixa.Trim().ToLower().Contains(Program.DB_Not_Available.Trim().ToLower()))
                {
                    _ListaCaixa = _ListaCaixa.Replace(Program.DB_Not_Available.Trim().ToLower(), "");
                    _ListaCaixa = _ListaCaixa.Replace(Program.DB_Not_Available.Trim().ToUpper(), "");
                }
                if (_ListaCaixa.Contains("," + lId.ToString() + ","))
                    _ListaCaixa = _ListaCaixa.Replace("," + lId.ToString() + ",", ",");
                else if (_ListaCaixa.Contains(lId.ToString() + ","))
                    _ListaCaixa = _ListaCaixa.Replace(lId.ToString() + ",", "");
                else if (_ListaCaixa.Contains("," + lId.ToString() + ","))
                    _ListaCaixa = _ListaCaixa.Replace("," + lId.ToString(), "");
                else if (_ListaCaixa.Contains(lId.ToString()))
                    _ListaCaixa = _ListaCaixa.Replace(lId.ToString(), "");
            }
            catch { }
        }

        public Int64 GetLastCaixaId()
        {
            try
            {
                if (_ListaCaixaIDs == null || _ListaCaixaIDs.Count == 0)
                    return -1;
                Int64 lLastCaixaId = -1;
                foreach (Int64 lCaixaId in _ListaCaixaIDs)
                {
                    if (lCaixaId > lLastCaixaId)
                        lLastCaixaId = lCaixaId;
                }
                return lLastCaixaId;
            }
            catch { return -1; }
        }

        public Boolean ContainsCaixaId(Int64 lId)
        {
            try
            {
                return _ListaCaixaIDs.IndexOf(lId) > -1;
            }
            catch { }
            return false;
        }

        #endregion  02: LISTACAIXA,C,20	

        public String LISTARECNU { get; set; } /// 03: LISTARECNU,C,20	
        public Int64 IDLOTE { get; set; } /// 04: IDLOTE,N,5,0
        public String NUMLOTE { get; set; } /// 05: NUMLOTE,C,10	
        public Int64 SOCIO { get; set; } /// 06: SOCIO,N,5,0	
        public String NOME { get; set; } /// 07: NOME,C,70
        public Int32 ANO { get; set; } /// 08: ANO,N,4,0	
        public Int32 MES { get; set; } /// 09: MES,N,2,0	
        public DateTime DATAPAG { get; set; } /// 10: DATAPAG,D	
        public String DESIGNACAO { get; set; } /// 11: DESIGNACAO,C,140	
        public Double PRECOM2P { get; set; } /// 12: PRECOM2P,N,12,3
        public Double AREAPAGO { get; set; } /// 13: AREAPAGO,N,12,2	
        public Double VALORPAGO { get; set; } /// 14: VALORPAGO,N,12,3
        public String NOTAS { get; set; } /// 16: NOTAS,C,140	
        public Int32 PAGNBR { get; set; } /// 18: PAGNBR,N,2,0

        public Int32 PAGTOTAL { get; set; }


        #region     Entities Types

        public enum EntityTypes
        {
            UNDEF = -1,
            //JOIA = 1,
            //QUOTAS = 2,
            INFRA = 3,
            RECON = 4,
            CEDEN = 5,
            ESGOT = 9
        }

        private EntityTypes _Entity_Type = EntityTypes.UNDEF;

        public EntityTypes Entity_Type
        {
            get
            {
                return _Entity_Type;
            }
            set
            {
                _Entity_Type = value;
            }
        }

        #endregion  Entities Types
        
        #region     Payment State

        public enum PayState
        {
            UNDEFINED = -1,
            SIM = 1,
            NAO = 2,
            EM_PAGAMENTO = 3
        }

        private String ESTADO; /// 15: ESTADO,C,1	

        private PayState _Pay_State = PayState.UNDEFINED;

        public String Pay_State_DB_Value
        {
            get
            {
                return ESTADO;
            }
            set
            {
                if (String.IsNullOrEmpty(value.Trim()) || value.Trim().Length != 1)
                    return;
                ESTADO = value.Trim().ToUpper();
                _Pay_State = GetPayStateTypeByInitial(ESTADO);
            }
        }

        public PayState Pay_State
        {
            get
            {
                return _Pay_State;
            }
            set
            {
                _Pay_State = value;
                if (_Pay_State != PayState.UNDEFINED)
                    ESTADO = GetPay_StateDBvalue(_Pay_State);
            }
        }

        #endregion  Payment State

        #endregion  Properties

        #region     Constructor

        public PAG_ENTIDADE()
        {
            LibAMFC = new Library_AMFC_Methods();

            Entidade_Configs = new EntityTypeConfigs();

            ID = -1; /// 01: ID,N,10,0	
            LISTACAIXA = String.Empty; /// 02: LISTACAIXA,C,20	
            LISTARECNU = String.Empty; /// 03: LISTARECNU,C,20	
            IDLOTE = -1; /// 04: IDLOTE,N,5,0
            NUMLOTE = String.Empty; /// 05: NUMLOTE,C,10	
            SOCIO = -1; /// 06: SOCIO,N,5,0
            NOME = String.Empty; /// 07: NOME,C,70
            ANO = -1; /// 08: ANO,N,4,0	
            MES = -1; /// 09: MES,N,2,0	
            DATAPAG = new DateTime(); /// 10: DATAPAG,D	
            DESIGNACAO = String.Empty; /// 11: DESIGNACAO,C,140	
            PRECOM2P = 0; /// 12: PRECOM2P,N,12,3
            AREAPAGO = 0; /// 13: AREAPAGO,N,12,2	
            VALORPAGO = 0; /// 14: VALORPAGO,N,12,3
            ESTADO = "N"; /// 15: ESTADO,C,1	
            NOTAS = String.Empty; /// 16: NOTAS,C,140	 
            PAGNBR = 0; /// 18: PAGNBR,N,2,0
            PAGTOTAL = 0;
            _Entity_Type = EntityTypes.UNDEF;
        }
        #endregion  Constructor

        #region     Methods

        #region     Payment State

        public String GetPay_StateDBvalue()
        {
            try
            {
                return Get_Pay_State_DB_value(_Pay_State);
            }
            catch { return String.Empty; }
        }

        public String GetPay_StateDBvalue(PayState ePayState)
        {
            try
            {
                return Get_Pay_State_DB_value(ePayState);
            }
            catch { return String.Empty; }
        }

        public String Get_Pay_State_DB_value(PayState ePayState)
        {
            try
            {
                String sPayStateDesc = Enum.GetName(typeof(PayState), ePayState);
                if (String.IsNullOrEmpty(sPayStateDesc))
                    return "";
                String sPayStateDbValue = sPayStateDesc.Trim().Substring(0, 1).ToUpper();
                return sPayStateDbValue;
            }
            catch { return String.Empty; }
        }

        public String GetPayStateDesc()
        {
            try
            {
                return Get_Pay_State_Desc(_Pay_State);
            }
            catch { return String.Empty; }
        }

        public String GetPayStateDesc(PayState ePayState)
        {
            try
            {
                return Get_Pay_State_Desc(ePayState);
            }
            catch { return String.Empty; }
        }

        public PayState GetPayStateType(Int32 iId)
        {
            try
            {
                if (iId < 1 || iId > 2)
                    return PayState.UNDEFINED;
                return (PayState)Enum.ToObject(typeof(PayState), iId);
            }
            catch { return PayState.UNDEFINED; }
        }

        public PayState GetPayStateType(String sPayState)
        {
            try
            {
                if (String.IsNullOrEmpty(sPayState.Trim()))
                    return PayState.UNDEFINED;
                return (PayState)Enum.Parse(typeof(PayState), sPayState.Trim().ToUpper());
            }
            catch { return PayState.UNDEFINED; }
        }

        public PayState GetPayStateTypeByInitial(String sPayStateDB)
        {
            try
            {
                PayState ePayState = PayState.UNDEFINED;
                sPayStateDB = sPayStateDB.Trim().Substring(0, 1).ToUpper();
                switch (sPayStateDB)
                {
                    case "S":
                        ePayState = PayState.SIM;
                        break;
                    case "N":
                        ePayState = PayState.NAO;
                        break;
                    case "E":
                        ePayState = PayState.EM_PAGAMENTO;
                        break;
                    default:
                        ePayState = PayState.UNDEFINED;
                        break;
                }
                return ePayState;
            }
            catch { return PayState.UNDEFINED; }
        }

        private String Get_Pay_State_Desc(PayState ePayState)
        {
            try
            {
                if (ePayState == PayState.UNDEFINED)
                    return "";
                String sPayStateDesc = Enum.GetName(typeof(PayState), ePayState);
                return sPayStateDesc;
            }
            catch { return String.Empty; }
        }

        public Int32 GetPayStateId()
        {
            try
            {
                return Get_Pay_State_Id(_Pay_State);
            }
            catch { return -1; }
        }

        public Int32 GetPayStateId(PayState ePayState)
        {
            try
            {
                return Get_Pay_State_Id(ePayState);
            }
            catch { return -1; }
        }

        private Int32 Get_Pay_State_Id(PayState ePayState)
        {
            try
            {
                if (ePayState == PayState.UNDEFINED)
                    return -1;
                return (Int32)ePayState;
            }
            catch { return -1; }
        }

        public Int32 GetPayStateIdx()
        {
            try
            {
                return Get_Pay_State_Idx(_Pay_State);
            }
            catch { return -1; }
        }

        public Int32 GetPayStateIdx(PayState ePayState)
        {
            try
            {
                return Get_Pay_State_Idx(ePayState);
            }
            catch { return -1; }
        }

        private Int32 Get_Pay_State_Idx(PayState ePayState)
        {
            try
            {
                if (ePayState == PayState.UNDEFINED)
                    return -1;
                Int32 iId = Get_Pay_State_Id(ePayState);
                if (iId < 1)
                    return -1;
                return iId - 1;
            }
            catch { return -1; }
        }

        #endregion  Payment State

        #region     Entity Type Configs

        public EntityTypeConfigs GetEntityTypeConfigs(PAG_ENTIDADE.EntityTypes eEntityType)
        {
            try
            {
                if (eEntityType == PAG_ENTIDADE.EntityTypes.UNDEF)
                {
                    XtraMessageBox.Show("Tipo de Pagamento Inválido!", "Erro [" + "Tipo de Pagamento" + "]", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }

                EntityTypeConfigs objEntityTypeConfigs = new EntityTypeConfigs();
                objEntityTypeConfigs.Entidade_Tipo = eEntityType;

                switch (eEntityType)
                {
                    case PAG_ENTIDADE.EntityTypes.INFRA:
                        objEntityTypeConfigs.DBF_File_Name = LibAMFC.DBF_AMFC_INFRA_FileName;
                        objEntityTypeConfigs.Pagamento_Tipo = AMFCCashPayment.PaymentTypes.INFRAS;
                        objEntityTypeConfigs.Entity_Value_Meter = Program.Get_Current_Parameter_INFRA_Valor_Metro();
                        objEntityTypeConfigs.Entity_Desc_Single = Program.Entity_INFRA_Desc_Single;
                        objEntityTypeConfigs.Entity_Lower_Single = Program.Entity_INFRA_Lower_Single;
                        objEntityTypeConfigs.Entity_Upper_Single = Program.Entity_INFRA_Upper_Single;
                        objEntityTypeConfigs.Entity_Desc_Plural = Program.Entity_INFRA_Desc_Plural;
                        objEntityTypeConfigs.Entity_Lower_Plural = Program.Entity_INFRA_Lower_Plural;
                        objEntityTypeConfigs.Entity_Upper_Plural = Program.Entity_INFRA_Upper_Plural;
                        objEntityTypeConfigs.Entity_Abbr_Lower = Program.Entity_INFRA_Abbr_Lower;
                        objEntityTypeConfigs.Entity_Abbr_Upper = Program.Entity_INFRA_Abbr_Upper;
                        objEntityTypeConfigs.Entity_Desc_Short_Single = Program.Entity_INFRA_Desc_Short_Single;
                        objEntityTypeConfigs.Entity_Lower_Short_Single = Program.Entity_INFRA_Lower_Short_Single;
                        objEntityTypeConfigs.Entity_Upper_Short_Single = Program.Entity_INFRA_Upper_Short_Single;
                        objEntityTypeConfigs.Entity_Desc_Short_Plural = Program.Entity_INFRA_Desc_Short_Plural;
                        objEntityTypeConfigs.Entity_Lower_Short_Plural = Program.Entity_INFRA_Lower_Short_Plural;
                        objEntityTypeConfigs.Entity_Upper_Short_Plural = Program.Entity_INFRA_Upper_Short_Plural;
                        break;

                    case PAG_ENTIDADE.EntityTypes.CEDEN:
                        objEntityTypeConfigs.DBF_File_Name = LibAMFC.DBF_AMFC_CEDEN_FileName;
                        objEntityTypeConfigs.Pagamento_Tipo = AMFCCashPayment.PaymentTypes.CEDENC;
                        objEntityTypeConfigs.Entity_Value_Meter = Program.Get_Current_Parameter_CEDENC_Valor_Metro();
                        objEntityTypeConfigs.Entity_Desc_Single = Program.Entity_CEDEN_Desc_Single;
                        objEntityTypeConfigs.Entity_Lower_Single = Program.Entity_CEDEN_Lower_Single;
                        objEntityTypeConfigs.Entity_Upper_Single = Program.Entity_CEDEN_Upper_Single;
                        objEntityTypeConfigs.Entity_Desc_Plural = Program.Entity_CEDEN_Desc_Plural;
                        objEntityTypeConfigs.Entity_Lower_Plural = Program.Entity_CEDEN_Lower_Plural;
                        objEntityTypeConfigs.Entity_Upper_Plural = Program.Entity_CEDEN_Upper_Plural;
                        objEntityTypeConfigs.Entity_Abbr_Lower = Program.Entity_CEDEN_Abbr_Lower;
                        objEntityTypeConfigs.Entity_Abbr_Upper = Program.Entity_CEDEN_Abbr_Upper;
                        objEntityTypeConfigs.Entity_Desc_Short_Single = Program.Entity_CEDEN_Desc_Short_Single;
                        objEntityTypeConfigs.Entity_Lower_Short_Single = Program.Entity_CEDEN_Lower_Short_Single;
                        objEntityTypeConfigs.Entity_Upper_Short_Single = Program.Entity_CEDEN_Upper_Short_Single;
                        objEntityTypeConfigs.Entity_Desc_Short_Plural = Program.Entity_CEDEN_Desc_Short_Plural;
                        objEntityTypeConfigs.Entity_Lower_Short_Plural = Program.Entity_CEDEN_Lower_Short_Plural;
                        objEntityTypeConfigs.Entity_Upper_Short_Plural = Program.Entity_CEDEN_Upper_Short_Plural;
                        break;

                    case PAG_ENTIDADE.EntityTypes.ESGOT:
                        objEntityTypeConfigs.DBF_File_Name = LibAMFC.DBF_AMFC_ESGOT_FileName;
                        objEntityTypeConfigs.Pagamento_Tipo = AMFCCashPayment.PaymentTypes.ESGOT;

                        //objEntityTypeConfigs.Entity_Value_Meter = Program.Get_Current_Parameter_ESGOT_Valor_Metro();
                        objEntityTypeConfigs.Entity_Value_Meter = 0;

                        objEntityTypeConfigs.Entity_Desc_Single = Program.Entity_ESGOT_Desc_Single;
                        objEntityTypeConfigs.Entity_Lower_Single = Program.Entity_ESGOT_Lower_Single;
                        objEntityTypeConfigs.Entity_Upper_Single = Program.Entity_ESGOT_Upper_Single;
                        objEntityTypeConfigs.Entity_Desc_Plural = Program.Entity_ESGOT_Desc_Plural;
                        objEntityTypeConfigs.Entity_Lower_Plural = Program.Entity_ESGOT_Lower_Plural;
                        objEntityTypeConfigs.Entity_Upper_Plural = Program.Entity_ESGOT_Upper_Plural;
                        objEntityTypeConfigs.Entity_Abbr_Lower = Program.Entity_ESGOT_Abbr_Lower;
                        objEntityTypeConfigs.Entity_Abbr_Upper = Program.Entity_ESGOT_Abbr_Upper;
                        objEntityTypeConfigs.Entity_Desc_Short_Single = Program.Entity_ESGOT_Desc_Short_Single;
                        objEntityTypeConfigs.Entity_Lower_Short_Single = Program.Entity_ESGOT_Lower_Short_Single;
                        objEntityTypeConfigs.Entity_Upper_Short_Single = Program.Entity_ESGOT_Upper_Short_Single;
                        objEntityTypeConfigs.Entity_Desc_Short_Plural = Program.Entity_ESGOT_Desc_Short_Plural;
                        objEntityTypeConfigs.Entity_Lower_Short_Plural = Program.Entity_ESGOT_Lower_Short_Plural;
                        objEntityTypeConfigs.Entity_Upper_Short_Plural = Program.Entity_ESGOT_Upper_Short_Plural;
                        break;

                    case PAG_ENTIDADE.EntityTypes.RECON:
                        objEntityTypeConfigs.DBF_File_Name = LibAMFC.DBF_AMFC_RECON_FileName;
                        objEntityTypeConfigs.Pagamento_Tipo = AMFCCashPayment.PaymentTypes.RECONV;
                        objEntityTypeConfigs.Entity_Value_Meter = Program.Get_Current_Parameter_RECON_Valor_Metro();
                        objEntityTypeConfigs.Entity_Desc_Single = Program.Entity_RECON_Desc_Single;
                        objEntityTypeConfigs.Entity_Lower_Single = Program.Entity_RECON_Lower_Single;
                        objEntityTypeConfigs.Entity_Upper_Single = Program.Entity_RECON_Upper_Single;
                        objEntityTypeConfigs.Entity_Desc_Plural = Program.Entity_RECON_Desc_Plural;
                        objEntityTypeConfigs.Entity_Lower_Plural = Program.Entity_RECON_Lower_Plural;
                        objEntityTypeConfigs.Entity_Upper_Plural = Program.Entity_RECON_Upper_Plural;
                        objEntityTypeConfigs.Entity_Abbr_Lower = Program.Entity_RECON_Abbr_Lower;
                        objEntityTypeConfigs.Entity_Abbr_Upper = Program.Entity_RECON_Abbr_Upper;
                        objEntityTypeConfigs.Entity_Desc_Short_Single = Program.Entity_RECON_Desc_Short_Single;
                        objEntityTypeConfigs.Entity_Lower_Short_Single = Program.Entity_RECON_Lower_Short_Single;
                        objEntityTypeConfigs.Entity_Upper_Short_Single = Program.Entity_RECON_Upper_Short_Single;
                        objEntityTypeConfigs.Entity_Desc_Short_Plural = Program.Entity_RECON_Desc_Short_Plural;
                        objEntityTypeConfigs.Entity_Lower_Short_Plural = Program.Entity_RECON_Lower_Short_Plural;
                        objEntityTypeConfigs.Entity_Upper_Short_Plural = Program.Entity_RECON_Upper_Short_Plural;
                        break;

                    default:
                        XtraMessageBox.Show("Tipo de Pagamento Inválido!", "Erro [" + "Tipo de Pagamento" + "]", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return null;
                }

                this.Entidade_Configs = objEntityTypeConfigs;
                return objEntityTypeConfigs;
            }
            catch
            {
                return null;
            }
        }

        #endregion  Entity Type Configs

        #endregion  Methods
    }

    #endregion  Pagamento Entidades

    #region     Joias

    /// <versions>25-11-2017(GesAMFC-v0.0.4.37)</versions>
    public class AMFCMemberJoias
    {
        public List<AMFCMemberJoia> Joias { get; set; }
        public AMFCMemberJoias()
        {
            Joias = new List<AMFCMemberJoia>();
        }
        public Boolean Add(AMFCMemberJoia objMemberJoia)
        {
            try
            {
                objMemberJoia.Idx = this.Joias.Count;
                if (Contains(objMemberJoia))
                    return false;
                this.Joias.Add(objMemberJoia);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public Boolean Contains(AMFCMemberJoia objMemberJoia)
        {
            try
            {
                foreach (AMFCMemberJoia objJoia in this.Joias)
                {
                    if (objJoia.MemberNumber == objMemberJoia.MemberNumber)
                        return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
        public Int32 GetJoiaIndex(AMFCMemberJoia objMemberJoia)
        {
            try
            {
                for (Int32 iIdx = 0; iIdx < this.Joias.Count; iIdx++)
                {
                    AMFCMemberJoia objJoia = this.Joias[iIdx];
                    if (objJoia.MemberNumber == objMemberJoia.MemberNumber)
                        return iIdx;
                }
                return -1;
            }
            catch
            {
                return -1;
            }
        }
    }

    /// <versions>25-11-2017(GesAMFC-v0.0.4.37)</versions>
    public class AMFCMemberJoia
    {
        #region     Private Fields

        private Int64 _MemberNumber = -1;
        private String _MemberName = Program.DB_Not_Available;
        private DateTime _DtMemberAdmiDate = new DateTime();
        private String _MemberAdmiDate = Program.Date_Format_String;

        private Int64 _Idx = -1;
        private Int64 _JoiaId = -1;
        private String _JoiaListaCaixa = Program.DB_Not_Available;
        private List<Int64> _JoiaListaCaixaIDs = new List<Int64>();
        private String _JoiaListaRecibos = Program.DB_Not_Available;
        private List<Int64> _JoiaListaRecibosIDs = new List<Int64>();
        private String _JoiaValueStr = Program.Default_Pay_String;
        private Double _JoiaValue = Program.Default_Pay_Value;
        private DateTime _DtJoiaDate = new DateTime();
        private String _JoiaDate = Program.Default_Date_Str;
        private Int32 _JoiaYearInt = -1;
        private String _JoiaYear = Program.DB_Not_Available;
        private Int32 _JoiaMonthInt = -1;
        private String _JoiaMonth = Program.DB_Not_Available;
        private String _JoiaMonthYear = Program.DB_Not_Available;
        private Boolean _JoiaPaid = false;
        private DateTime _DtJoiaDatePaid = new DateTime();
        private String _JoiaDatePaid = Program.DB_Not_Available;
        private String _JoiaPaidPerson = Program.DB_Not_Available;
        private DateTime _DtJoiaDataPagamentoAgregado = new DateTime();
        private String _JoiaDataPagamentoAgregado = Program.Default_Date_Str;
        private String _JoiaNotas = String.Empty;
        private String _PaidOrNot = String.Empty;

        private String _ValueToPayStr = Program.Default_Pay_String;
        private Double _ValueToPay = Program.Default_Pay_Value;

        private String _ValuePaidStr = Program.Default_Pay_String;
        private Double _ValuePaid = Program.Default_Pay_Value;

        private String _ValueMissingStr = Program.Default_Pay_String;
        private Double _ValueMissing = Program.Default_Pay_Value;

        private String _ValueOnPayingStr = Program.Default_Pay_String;
        private Double _ValueOnPaying = Program.Default_Pay_Value;

        #region     Payment State

        public enum PayState
        {
            UNDEFINED = -1,
            SIM = 1,
            NAO = 2,
            EM_PAGAMENTO = 3
        }

        private String _Pay_State_DB_Value = "";

        private PayState _Pay_State = PayState.UNDEFINED;

        public String Pay_State_DB_Value
        {
            get
            {
                return _Pay_State_DB_Value;
            }
            set
            {
                if (String.IsNullOrEmpty(value.Trim()) || value.Trim().Length != 1)
                    return;
                _Pay_State_DB_Value = value.Trim().ToUpper();
                _Pay_State = GetPayStateTypeByInitial(_Pay_State_DB_Value);
            }
        }

        public PayState Pay_State
        {
            get
            {
                return _Pay_State;
            }
            set
            {
                _Pay_State = value;
                if (_Pay_State != PayState.UNDEFINED)
                    _Pay_State_DB_Value = GetPay_StateDBvalue(_Pay_State);
            }
        }

        #endregion  Payment State

        #endregion  Private Fields

        #region     Properties

        public Int64 MemberNumber
        {
            get { return _MemberNumber; }
            set
            {
                if (value < 1 || value > new AMFCMember().MaxNumber)
                    return;
                _MemberNumber = value;
            }
        }

        public String MemberName
        {
            get { return _MemberName; }
            set { _MemberName = value; }
        }

        public DateTime DtMemberAdmiDate
        {
            get { return _DtMemberAdmiDate; }
            set
            {
                if (!Program.IsValidDateTime(value))
                    return;
                _DtMemberAdmiDate = value;
                _MemberAdmiDate = _DtMemberAdmiDate.ToString(Program.Date_Format_String, CultureInfo.CurrentCulture);
            }
        }

        public String MemberAdmiDate
        {
            get { return _MemberAdmiDate; }
            set
            {
                if (String.IsNullOrEmpty(value))
                    return;
                _MemberAdmiDate = value;
                _DtMemberAdmiDate = Program.ConvertToValidDateTime(_MemberAdmiDate);
            }
        }

        public Int64 Idx
        {
            get { return _Idx; }
            set { _Idx = value; }
        }

        public Int64 JoiaId
        {
            get { return _JoiaId; }
            set
            {
                if (value < 1)
                    return;
                _JoiaId = value;
            }
        }

        public String JoiaListaCaixa
        {
            get
            {
                try
                {
                    if (String.IsNullOrEmpty(_JoiaListaCaixa))
                        return String.Empty;
                    return _JoiaListaCaixa;
                }
                catch
                {
                    return String.Empty;
                }
            }
            set
            {
                try
                {
                    if (String.IsNullOrEmpty(value))
                        return;
                    _JoiaListaCaixa = value;
                    try
                    {
                        if (_JoiaListaCaixaIDs == null || _JoiaListaCaixaIDs.Count == 0)
                        {
                            _JoiaListaCaixaIDs = new List<Int64>();
                            if (_JoiaListaCaixa.Trim().ToLower().Contains(Program.DB_Not_Available.Trim().ToLower()))
                            {
                                _JoiaListaCaixa = _JoiaListaCaixa.Replace(Program.DB_Not_Available.Trim().ToLower(), "");
                                _JoiaListaCaixa = _JoiaListaCaixa.Replace(Program.DB_Not_Available.Trim().ToUpper(), "");
                            }
                            List<String> ListJoiaListaCaixa = _JoiaListaCaixa.Split(new String[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
                            foreach (String sId in ListJoiaListaCaixa)
                            {
                                try
                                {
                                    if (!String.IsNullOrEmpty(sId.Trim()))
                                        _JoiaListaCaixaIDs.Add(Convert.ToInt64(sId.Trim()));
                                }
                                catch { }
                            }
                        }
                    }
                    catch { }
                }
                catch
                {
                    _JoiaListaCaixa = String.Empty;
                }
            }
        }

        public List<Int64> JoiaListaCaixaIDs
        {
            get { return _JoiaListaCaixaIDs; }
            set
            {
                _JoiaListaCaixaIDs = value;
                try
                {
                    if (value == null)
                        return;
                    _JoiaListaCaixaIDs = value;
                    try
                    {
                        if (Program.IsValidTextString(_JoiaListaCaixa.Trim()))
                        {
                            _JoiaListaCaixa = String.Empty;
                            foreach (Int64 lId in _JoiaListaCaixaIDs)
                            {
                                try
                                {
                                    if (!String.IsNullOrEmpty(_JoiaListaCaixa))
                                        _JoiaListaCaixa += ",";
                                    _JoiaListaCaixa += lId.ToString();
                                }
                                catch { }
                            }
                        }
                    }
                    catch { }
                }
                catch
                {
                    _JoiaListaCaixaIDs = new List<Int64>();
                }
            }
        }

        public void AddCaixaId(Int64 lId)
        {
            try
            {
                if (lId <= 0 || ContainsCaixaId(lId))
                    return;
                _JoiaListaCaixaIDs.Add(lId);
                if (_JoiaListaCaixa.Trim().ToLower().Contains(Program.DB_Not_Available.Trim().ToLower()))
                {
                    _JoiaListaCaixa = _JoiaListaCaixa.Replace(Program.DB_Not_Available.Trim().ToLower(), "");
                    _JoiaListaCaixa = _JoiaListaCaixa.Replace(Program.DB_Not_Available.Trim().ToUpper(), "");
                }
                if (!String.IsNullOrEmpty(_JoiaListaCaixa))
                    _JoiaListaCaixa += ",";
                _JoiaListaCaixa += lId.ToString();
            }
            catch { }
        }

        public void DelCaixaId(Int64 lId)
        {
            try
            {
                if (lId <= 0 || !ContainsCaixaId(lId))
                    return;
                _JoiaListaCaixaIDs.Remove(lId);
                if (_JoiaListaCaixa.Trim().ToLower().Contains(Program.DB_Not_Available.Trim().ToLower()))
                {
                    _JoiaListaCaixa = _JoiaListaCaixa.Replace(Program.DB_Not_Available.Trim().ToLower(), "");
                    _JoiaListaCaixa = _JoiaListaCaixa.Replace(Program.DB_Not_Available.Trim().ToUpper(), "");
                }
                if (_JoiaListaCaixa.Contains("," + lId.ToString() + ","))
                    _JoiaListaCaixa = _JoiaListaCaixa.Replace("," + lId.ToString() + ",", ",");
                else if (_JoiaListaCaixa.Contains(lId.ToString() + ","))
                    _JoiaListaCaixa = _JoiaListaCaixa.Replace(lId.ToString() + ",", "");
                else if (_JoiaListaCaixa.Contains("," + lId.ToString() + ","))
                    _JoiaListaCaixa = _JoiaListaCaixa.Replace("," + lId.ToString(), "");
                else if (_JoiaListaCaixa.Contains(lId.ToString()))
                    _JoiaListaCaixa = _JoiaListaCaixa.Replace(lId.ToString(), "");
            }
            catch { }
        }

        public Int64 GetLastCaixaId()
        {
            try
            {
                if (_JoiaListaCaixaIDs == null || _JoiaListaCaixaIDs.Count == 0)
                    return -1;
                Int64 lLastCaixaId = -1;
                foreach (Int64 lCaixaId in _JoiaListaCaixaIDs)
                {
                    if (lCaixaId > lLastCaixaId)
                        lLastCaixaId = lCaixaId;
                }
                return lLastCaixaId;
            }
            catch { return -1; }
        }

        public Boolean ContainsCaixaId(Int64 lId)
        {
            try
            {
                return _JoiaListaCaixaIDs.IndexOf(lId) > -1;
            }
            catch { }
            return false;
        }

        public String JoiaListaRecibos
        {
            get
            {
                try
                {
                    if (String.IsNullOrEmpty(_JoiaListaRecibos))
                        return String.Empty;
                    return _JoiaListaRecibos;
                }
                catch
                {
                    return String.Empty;
                }
            }
            set
            {
                try
                {
                    if (String.IsNullOrEmpty(value))
                        return;
                    _JoiaListaRecibos = value;
                    try
                    {
                        if (_JoiaListaRecibosIDs == null || _JoiaListaRecibosIDs.Count == 0)
                        {
                            _JoiaListaRecibosIDs = new List<Int64>();
                            if (_JoiaListaRecibos.Trim().ToLower().Contains(Program.DB_Not_Available.Trim().ToLower()))
                            {
                                _JoiaListaRecibos = _JoiaListaRecibos.Replace(Program.DB_Not_Available.Trim().ToLower(), "");
                                _JoiaListaRecibos = _JoiaListaRecibos.Replace(Program.DB_Not_Available.Trim().ToUpper(), "");
                            }
                            List<String> ListJoiaListaRecibos = _JoiaListaRecibos.Split(new String[] { "," }, StringSplitOptions.RemoveEmptyEntries).ToList();
                            foreach (String sId in ListJoiaListaRecibos)
                            {
                                try
                                {
                                    if (!String.IsNullOrEmpty(sId.Trim()))
                                        _JoiaListaRecibosIDs.Add(Convert.ToInt64(sId.Trim()));
                                }
                                catch { }
                            }
                        }
                    }
                    catch { }
                }
                catch
                {
                    _JoiaListaRecibos = String.Empty;
                }
            }
        }

        public List<Int64> JoiaListaRecibosIDs
        {
            get { return _JoiaListaRecibosIDs; }
            set
            {
                _JoiaListaRecibosIDs = value;
                try
                {
                    if (value == null)
                        return;
                    _JoiaListaRecibosIDs = value;
                    try
                    {
                        if (Program.IsValidTextString(_JoiaListaRecibos))
                        {
                            _JoiaListaRecibos = String.Empty;
                            foreach (Int64 lId in _JoiaListaRecibosIDs)
                            {
                                try
                                {
                                    if (!String.IsNullOrEmpty(_JoiaListaRecibos))
                                        _JoiaListaRecibos += ",";
                                    _JoiaListaRecibos += lId.ToString();
                                }
                                catch { }
                            }
                        }
                    }
                    catch { }
                }
                catch
                {
                    _JoiaListaRecibosIDs = new List<Int64>();
                }
            }
        }

        public void AddReciboId(Int64 lId)
        {
            try
            {
                if (lId <= 0 || ContainsReciboId(lId))
                    return;
                _JoiaListaRecibosIDs.Add(lId);
                if (_JoiaListaRecibos.Trim().ToLower().Contains(Program.DB_Not_Available.Trim().ToLower()))
                {
                    _JoiaListaRecibos = _JoiaListaRecibos.Replace(Program.DB_Not_Available.Trim().ToLower(), "");
                    _JoiaListaRecibos = _JoiaListaRecibos.Replace(Program.DB_Not_Available.Trim().ToUpper(), "");
                }
                if (!String.IsNullOrEmpty(_JoiaListaRecibos))
                    _JoiaListaRecibos += ",";
                _JoiaListaRecibos += lId.ToString();
            }
            catch { }
        }

        public Boolean ContainsReciboId(Int64 lId)
        {
            try
            {
                return _JoiaListaRecibosIDs.IndexOf(lId) > -1;
            }
            catch { }
            return false;
        }

        #region     Values

        #region     JOIA VALOR

        public Double JoiaValue
        {
            get
            {
                return _JoiaValue;
            }
            set
            {
                _JoiaValue = value;
                _JoiaValueStr = Program.SetPayCurrencyEuroStringValue(_JoiaValue);
                ValueToPay = _JoiaValue;
            }
        }

        public String JoiaValueStr
        {
            get { return _JoiaValueStr; }
            set { _JoiaValueStr = value; }
        }

        #endregion  JOIA VALOR

        #region     Value To Pay

        public Double ValueToPay
        {
            get
            {
                return _ValueToPay;
            }
            set
            {
                if (value < 0 || value != _JoiaValue)
                    return;
                _ValueToPay = value;
                _ValueToPayStr = Program.SetPayCurrencyEuroStringValue(_ValueToPay);
                SetPaidState();
            }
        }

        public String ValueToPayStr
        {
            get { return _ValueToPayStr; }
            set { _ValueToPayStr = value; }
        }

        public Double GetValueToPay()
        {
            try
            {
                Double dValueToPay = Program.Default_Pay_Value;
                if (JoiaValue <= 0 || ValuePaid > JoiaValue)
                    return JoiaValue;
                dValueToPay = JoiaValue - ValuePaid;
                return dValueToPay;
            }
            catch { return Program.Default_Pay_Value; }
        }

        #endregion  Value To Pay

        #region     Value Paid

        public Double ValuePaid
        {
            get
            {
                return _ValuePaid;
            }
            set
            {
                if (value < 0 || value > _JoiaValue)
                    return;
                _ValuePaid = value;
                _ValuePaidStr = Program.SetPayCurrencyEuroStringValue(_ValuePaid);
                if (_JoiaValue >= _ValuePaid)
                    _ValueMissing = _JoiaValue - _ValuePaid;
                SetPaidState();
            }
        }

        public String ValuePaidStr
        {
            get { return _ValuePaidStr; }
            set { _ValuePaidStr = value; }
        }

        public Double GetValuePaid()
        {
            try
            {
                Double dValuePaid = Program.Default_Pay_Value;
                if (JoiaValue <= 0 || ValueMissing > JoiaValue)
                    return 0;
                dValuePaid = JoiaValue - ValueMissing;
                return dValuePaid;
            }
            catch { return Program.Default_Pay_Value; }
        }

        public Double AddValuePaid(Double dAditionalValuePaid)
        {
            try
            {
                Double dValuePaid = _ValuePaid;
                if (dAditionalValuePaid <= 0)
                    return 0;
                dValuePaid = _ValuePaid + dAditionalValuePaid;
                _ValuePaid = dValuePaid;
                if (_ValueToPay > _ValuePaid)
                    _ValueMissing = _ValueToPay - _ValuePaid;
                else
                {
                    _ValueToPay = _JoiaValue;
                    _ValueMissing = 0;
                    _ValueOnPaying = 0;
                }
                return dValuePaid;
            }
            catch { return Program.Default_Pay_Value; }
        }

        public Double RemoveValuePaid(Double dRemoveValuePaid)
        {
            try
            {
                Double dValuePaid = _ValuePaid;
                if (dRemoveValuePaid <= 0 || dRemoveValuePaid > _ValuePaid)
                    return 0;
                dValuePaid = _ValuePaid - dRemoveValuePaid;
                _ValuePaid = dValuePaid;
                if (_ValueToPay > _ValuePaid)
                    _ValueMissing = _ValueToPay - _ValuePaid;
                else
                {
                    _ValueToPay = _JoiaValue;
                    _ValueMissing = 0;
                    _ValueOnPaying = 0;
                }
                return dValuePaid;
            }
            catch { return Program.Default_Pay_Value; }
        }

        #endregion  Value Paid

        #region     Value On Paying

        public Double ValueOnPaying
        {
            get
            {
                return _ValueOnPaying;
            }
            set
            {
                if (value < 0 || value > _JoiaValue)
                    return;
                _ValueOnPaying = value;
                _ValueOnPayingStr = Program.SetPayCurrencyEuroStringValue(_ValueOnPaying);
                if (_JoiaValue >= _ValuePaid)
                    _ValueMissing = _JoiaValue - _ValuePaid;
                SetPaidState();
            }
        }

        public String ValueOnPayingStr
        {
            get { return _ValueOnPayingStr; }
            set { _ValueOnPayingStr = value; }
        }

        #endregion  Value On Paying

        #region     Value Missing

        public Double ValueMissing
        {
            get
            {
                return _ValueMissing;
            }
            set
            {
                if (value < 0 || value > (_JoiaValue - _ValuePaid))
                    return;
                _ValueMissing = value;
                _ValueMissingStr = Program.SetPayCurrencyEuroStringValue(_ValueMissing);
                SetPaidState();
            }
        }

        public Double GetValueMissing()
        {
            try
            {
                Double dValueMissing = Program.Default_Pay_Value;
                if (JoiaValue <= 0 || ValuePaid > JoiaValue)
                    return JoiaValue;
                dValueMissing = JoiaValue - ValuePaid;
                return dValueMissing;
            }
            catch { return Program.Default_Pay_Value; }
        }

        public String ValueMissingStr
        {
            get { return _ValueMissingStr; }
            set { _ValueMissingStr = value; }
        }

        #endregion  Value Missing

        #endregion  Values

        #region     Joia Paid State

        public void SetPaidState()
        {
            _JoiaPaid = (_JoiaValue > 0 && _ValueToPay == _ValuePaid && _ValueOnPaying == 0 && _ValueMissing == 0);
            _Pay_State = _JoiaPaid ? PayState.SIM : ((_ValueOnPaying > 0 || this.Pay_State_DB_Value == "E") ? PayState.EM_PAGAMENTO : PayState.NAO);
        }

        public Boolean JoiaPaid
        {
            get
            {
                SetPaidState();
                return _JoiaPaid;
            }
            set { _JoiaPaid = value; }
        }

        public String PaidOrNot
        {
            get
            {
                _PaidOrNot = _JoiaPaid ? "Joia  Paga" : "Joia Não Paga";
                SetPaidState();
                return _PaidOrNot;
            }
            set
            {
                _PaidOrNot = _JoiaPaid ? "Joia  Pagas" : "Joia Não Paga";
            }
        }

        #endregion  Joia Paid State

        #region     Dates

        public DateTime DtJoiaDate
        {
            get
            {
                return _DtJoiaDate;
            }
            set
            {
                if (!Program.IsValidDateTime(value))
                {
                    if (_DtMemberAdmiDate != null && Program.IsValidDateTime(_DtMemberAdmiDate))
                    {
                        _DtJoiaDate = _DtMemberAdmiDate;
                        _JoiaDate = _DtJoiaDate.ToString(Program.Date_Format_String, CultureInfo.CurrentCulture);
                    }
                    else
                    {
                        _DtJoiaDate = new DateTime();
                        JoiaDate = Program.Default_Date_Str;
                    }
                }
                else
                {
                    _DtJoiaDate = value;
                    _JoiaDate = _DtJoiaDate.ToString(Program.Date_Format_String, CultureInfo.CurrentCulture);
                    JoiaYearInt = _DtJoiaDate.Year;
                    JoiaMonthInt = _DtJoiaDate.Month;
                }
            }
        }

        public String JoiaDate
        {
            get { return _JoiaDate; }
            set
            {
                if (String.IsNullOrEmpty(value))
                    return;
                _JoiaDate = value;
                _DtJoiaDate = Program.ConvertToValidDateTime(_JoiaDate);
            }
        }

        public Int32 JoiaYearInt
        {
            get { return _JoiaYearInt; }
            set
            {
                if (!Program.IsValidYear(_JoiaYearInt))
                {
                    if (_DtMemberAdmiDate != null && Program.IsValidYear(_DtMemberAdmiDate.Year))
                    {
                        _JoiaYearInt = _DtMemberAdmiDate.Year;
                        JoiaYear = _JoiaYearInt.ToString();
                    }
                    else
                    {
                        _JoiaYearInt = -1;
                        JoiaYear = Program.DB_Not_Available;
                    }
                }
                else
                {
                    _JoiaYearInt = value;
                    JoiaYear = _JoiaYearInt.ToString();
                }
            }
        }

        public String JoiaYear
        {
            get { return _JoiaYear; }
            set { _JoiaYear = value; }
        }

        public Int32 JoiaMonthInt
        {
            get { return _JoiaMonthInt; }
            set
            {
                if (value >= 1 && value <= 12)
                {
                    _JoiaMonthInt = value;
                    if (!Program.IsValidYear(_JoiaYearInt))
                    {
                        if (Program.IsValidDateTime(_DtJoiaDate))
                            _JoiaYearInt = _DtJoiaDate.Year;
                        else
                        {
                            _JoiaMonth = Program.DB_Not_Available;
                            return;
                        }
                    }
                    DateTime dtYearMonth = new DateTime(_JoiaYearInt, _JoiaMonthInt, 1);
                    _JoiaMonth = "[" + dtYearMonth.ToString("MM", Program.CurrentCulture) + "/" + dtYearMonth.ToString("yy", Program.CurrentCulture) + "]" + ": " + dtYearMonth.ToString("MMMM", Program.CurrentCulture) + " / " + dtYearMonth.ToString("yyyy", Program.CurrentCulture);
                }
                else if (_DtMemberAdmiDate != null && Program.IsValidDateTime(_DtMemberAdmiDate))
                    _JoiaMonth = "[" + _DtMemberAdmiDate.ToString("MM", Program.CurrentCulture) + "/" + _DtMemberAdmiDate.ToString("yy", Program.CurrentCulture) + "]" + ": " + _DtMemberAdmiDate.ToString("MMMM", Program.CurrentCulture) + " / " + _DtMemberAdmiDate.ToString("yyyy", Program.CurrentCulture);
                else
                {
                    _JoiaMonthInt = -1;
                    _JoiaMonth = Program.DB_Not_Available;
                    _JoiaMonthYear = Program.DB_Not_Available;
                }
            }
        }

        public String JoiaMonth
        {
            get { return _JoiaMonth; }
            set { _JoiaMonth = value; }
        }

        public String JoiaMonthYear
        {
            get
            {
                if (_JoiaYearInt >= 1970 && _JoiaYearInt <= 2069 && _JoiaMonthInt >= 1 && _JoiaMonthInt <= 12)
                {
                    try
                    {
                        _JoiaMonthYear = _DtJoiaDate.ToString("MMM", Program.CurrentCulture) + "/" + _JoiaYear;
                    }
                    catch
                    {
                        _JoiaMonthYear = Program.DB_Not_Available;
                        if (_MemberAdmiDate != Program.DB_Not_Available)
                            _JoiaMonthYear = _DtMemberAdmiDate.ToString("MMM", Program.CurrentCulture) + "/" + _DtMemberAdmiDate.Year;
                        else
                            _JoiaMonthYear = Program.DB_Not_Available;
                    }
                }
                else
                {
                    _JoiaMonthYear = Program.DB_Not_Available;
                    if (_MemberAdmiDate != Program.DB_Not_Available)
                        _JoiaMonthYear = _DtMemberAdmiDate.ToString("MMM", Program.CurrentCulture) + "/" + _DtMemberAdmiDate.Year;
                    else
                        _JoiaMonthYear = Program.DB_Not_Available;
                }
                return _JoiaMonthYear;
            }
            set { _JoiaMonthYear = value; }
        }

        public DateTime DtJoiaDatePaid
        {
            get { return _DtJoiaDatePaid; }
            set
            {
                if (!Program.IsValidDateTime(value))
                    return;
                _DtJoiaDatePaid = value;
                _JoiaDatePaid = _DtJoiaDatePaid.ToString(Program.Date_Format_String, CultureInfo.CurrentCulture);
            }
        }

        public String JoiaDatePaid
        {
            get { return _JoiaDatePaid; }
            set
            {
                if (String.IsNullOrEmpty(value))
                    return;
                _JoiaDatePaid = value;
                _DtJoiaDatePaid = Program.ConvertToValidDateTime(_JoiaDatePaid);
            }
        }

        public DateTime DtJoiaDataPagamentoAgregado
        {
            get { return _DtJoiaDataPagamentoAgregado; }
            set
            {
                if (!Program.IsValidDateTime(value))
                    return;
                _DtJoiaDataPagamentoAgregado = value;
            }
        }

        public String JoiaDataPagamentoAgregado
        {
            get { return _JoiaDataPagamentoAgregado; }
            set
            {
                if (String.IsNullOrEmpty(value))
                    return;
                _JoiaDataPagamentoAgregado = value;
                _DtJoiaDataPagamentoAgregado = Program.ConvertToValidDateTime(_JoiaDataPagamentoAgregado);
            }
        }

        #endregion  Dates

        public String JoiaPaidPerson
        {
            get { return _JoiaPaidPerson; }
            set { _JoiaPaidPerson = value; }
        }

        public String JoiaNotas
        {
            get { return _JoiaNotas; }
            set { _JoiaNotas = value; }
        }

        #endregion  Properties

        #region     Constructor
        public AMFCMemberJoia()
        {
            Idx = -1;
            JoiaId = -1;
            JoiaListaCaixa = String.Empty;
            JoiaListaCaixaIDs = new List<Int64>();
            JoiaListaRecibos = String.Empty;
            JoiaListaRecibosIDs = new List<Int64>();
            MemberNumber = -1;
            MemberName = String.Empty;
            JoiaValueStr = String.Empty;
            JoiaValue = Program.Default_Pay_Value;
            ValueToPay = Program.Default_Pay_Value;
            ValueToPay = Program.Default_Pay_Value;
            ValueMissing = Program.Default_Pay_Value;
            DtJoiaDate = new DateTime();
            JoiaDate = String.Empty;
            JoiaYearInt = -1;
            JoiaYear = String.Empty;
            JoiaMonthInt = -1;
            JoiaMonth = String.Empty;
            JoiaMonthYear = String.Empty;
            JoiaPaid = false;
            PaidOrNot = JoiaPaid ? "Pago" : "Não Pago";
            DtJoiaDatePaid = new DateTime();
            JoiaDatePaid = String.Empty;
            JoiaPaidPerson = String.Empty;
            DtJoiaDataPagamentoAgregado = new DateTime();
            JoiaDataPagamentoAgregado = String.Empty;
            JoiaNotas = String.Empty;
        }
        #endregion  Constructor

        #region     Methods

        #region     Payment State

        public String GetPay_StateDBvalue()
        {
            try
            {
                return Get_Pay_State_DB_value(_Pay_State);
            }
            catch { return String.Empty; }
        }

        public String GetPay_StateDBvalue(PayState ePayState)
        {
            try
            {
                return Get_Pay_State_DB_value(ePayState);
            }
            catch { return String.Empty; }
        }

        public String Get_Pay_State_DB_value(PayState ePayState)
        {
            try
            {
                String sPayStateDesc = Enum.GetName(typeof(PayState), ePayState);
                if (String.IsNullOrEmpty(sPayStateDesc))
                    return "";
                String sPayStateDbValue = sPayStateDesc.Trim().Substring(0, 1).ToUpper();
                return sPayStateDbValue;
            }
            catch { return String.Empty; }
        }

        public String GetPayStateDesc()
        {
            try
            {
                return Get_Pay_State_Desc(_Pay_State);
            }
            catch { return String.Empty; }
        }

        public String GetPayStateDesc(PayState ePayState)
        {
            try
            {
                return Get_Pay_State_Desc(ePayState);
            }
            catch { return String.Empty; }
        }

        public PayState GetPayStateType(Int32 iId)
        {
            try
            {
                if (iId < 1 || iId > 2)
                    return PayState.UNDEFINED;
                return (PayState)Enum.ToObject(typeof(PayState), iId);
            }
            catch { return PayState.UNDEFINED; }
        }

        public PayState GetPayStateType(String sPayState)
        {
            try
            {
                if (String.IsNullOrEmpty(sPayState.Trim()))
                    return PayState.UNDEFINED;
                return (PayState)Enum.Parse(typeof(PayState), sPayState.Trim().ToUpper());
            }
            catch { return PayState.UNDEFINED; }
        }

        public PayState GetPayStateTypeByInitial(String sPayStateDB)
        {
            try
            {
                PayState ePayState = PayState.UNDEFINED;
                sPayStateDB = sPayStateDB.Trim().Substring(0, 1).ToUpper();
                switch (sPayStateDB)
                {
                    case "S":
                        ePayState = PayState.SIM;
                        break;
                    case "N":
                        ePayState = PayState.NAO;
                        break;
                    case "E":
                        ePayState = PayState.EM_PAGAMENTO;
                        break;
                    default:
                        ePayState = PayState.UNDEFINED;
                        break;
                }
                return ePayState;
            }
            catch { return PayState.UNDEFINED; }
        }

        private String Get_Pay_State_Desc(PayState ePayState)
        {
            try
            {
                if (ePayState == PayState.UNDEFINED)
                    return "";
                String sPayStateDesc = Enum.GetName(typeof(PayState), ePayState);
                return sPayStateDesc;
            }
            catch { return String.Empty; }
        }

        public Int32 GetPayStateId()
        {
            try
            {
                return Get_Pay_State_Id(_Pay_State);
            }
            catch { return -1; }
        }

        public Int32 GetPayStateId(PayState ePayState)
        {
            try
            {
                return Get_Pay_State_Id(ePayState);
            }
            catch { return -1; }
        }

        private Int32 Get_Pay_State_Id(PayState ePayState)
        {
            try
            {
                if (ePayState == PayState.UNDEFINED)
                    return -1;
                return (Int32)ePayState;
            }
            catch { return -1; }
        }

        public Int32 GetPayStateIdx()
        {
            try
            {
                return Get_Pay_State_Idx(_Pay_State);
            }
            catch { return -1; }
        }

        public Int32 GetPayStateIdx(PayState ePayState)
        {
            try
            {
                return Get_Pay_State_Idx(ePayState);
            }
            catch { return -1; }
        }

        private Int32 Get_Pay_State_Idx(PayState ePayState)
        {
            try
            {
                if (ePayState == PayState.UNDEFINED)
                    return -1;
                Int32 iId = Get_Pay_State_Id(ePayState);
                if (iId < 1)
                    return -1;
                return iId - 1;
            }
            catch { return -1; }
        }

        #endregion  Payment State

        #endregion  Methods
    }

    #endregion  Joias

    namespace AMFC_Methods
    {
        /// <versions>02-12-2017(GesAMFC-v0.0.4.41)</versions>
        public class Library_AMFC_Methods : IDisposable
        {
            #region     Program Variables

            public enum MemberOperationType { GET, ADD, EDIT, DEL }

            public enum OperationType { UNDEFINED = -1, OPEN = 1, ADD = 2, EDIT = 3, DEL = 4 }
            public enum PaymentOptions { SINGLE = 1, MULTIPLE = 2 }

            public String AppPath = Path.GetDirectoryName(Application.ExecutablePath);

            #region     SQL DBFs
            public String OLE_DB_Provider;
            public String DBF_AMFC_DirPath;
            public String DBF_AMFC_SOCIO_FileName;
            public String DBF_AMFC_JOIAS_FileName;
            public String DBF_AMFC_QUOTA_FileName;
            public String DBF_AMFC_CAIXA_FileName;
            public String DBF_AMFC_INFRA_FileName;
            public String DBF_AMFC_CEDEN_FileName;
            public String DBF_AMFC_ESGOT_FileName;
            public String DBF_AMFC_RECON_FileName;
            public String DBF_AMFC_RECIB_FileName;
            public String DBF_AMFC_PARAM_FileName;
            #endregion  SQL DBFs
            
            #region     Parâmetros Pagamentos
            public String DBF_AMFC_JOIA_VALOR;

            public String DBF_AMFC_QUOTA_VALOR_MES;
            public String DBF_AMFC_QUOTA_VALOR_ANO;
            public String DBF_AMFC_INFRA_VALOR_METRO;
            public String DBF_AMFC_CEDENC_VALOR_METRO;
            public String DBF_AMFC_ESGOT_VALOR_METRO;
            public String DBF_AMFC_RECONV_VALOR_METRO;

            public String DBF_AMFC_Euro_To_Escudos;

            #endregion  Parâmetros Pagamentos

            #endregion  Program Variables

            /// <versions>28-04-2017(v0.0.2.48)</versions>
            /// <remarks>O nome dos DBFs nao pode ultrapassar os 8 digitos</remarks>
            public Library_AMFC_Methods()
            {
                try
                {
                    using (LibraryXML objLibraryXML = new LibraryXML(Program.XmlConfigFilePath))
                    {
                        #region     SQL DBFs
                        if (!Set_SQL_DB_Settings())
                        {
                            Program.HandleError("Library_AMFC_Methods()", "Set_SQL_DB_Settings() Error!!", Program.ErroType.ERROR, true, false);
                            return;
                        }
                        #endregion  SQL DBFs

                        #region     Parâmetros Pagamentos
                        if (!Set_Payments_Parameters_Settings())
                        {
                            Program.HandleError("Library_AMFC_Methods()", "Set_Payments_Parameters_Settings() Error!!", Program.ErroType.ERROR, true, false);
                            return;
                        }
                        #endregion  Parâmetros Pagamentos
                    }
                }
                catch (Exception ex) 
                { 
                    Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false); 
                }
            }

            /// <versions>02-12-2017(GesAMFC-v0.0.4.41)</versions>
            public Boolean Set_SQL_DB_Settings()
            {
                try
                {
                    using (LibraryXML objLibraryXML = new LibraryXML(Program.XmlConfigFilePath))
                    {
                        OLE_DB_Provider = objLibraryXML.GetXmlConfigFileNodeValue("OLE_DB_Provider").Trim();
                        if (String.IsNullOrEmpty(OLE_DB_Provider))
                            return false;
                        DBF_AMFC_DirPath = objLibraryXML.GetXmlConfigFileNodeValue("DBF_AMFC_DirPath").Trim();
                        if (!DBF_Dir_Exists())
                            return false;

                        String sDBF_AMFC_SOCIO_FileName = Set_DBF_File("DBF_AMFC_SOCIO_FileName");
                        if (String.IsNullOrEmpty(sDBF_AMFC_SOCIO_FileName))
                            return false;
                        DBF_AMFC_SOCIO_FileName = sDBF_AMFC_SOCIO_FileName;

                        String sDBF_AMFC_JOIAS_FileName = Set_DBF_File("DBF_AMFC_JOIAS_FileName");
                        if (String.IsNullOrEmpty(sDBF_AMFC_JOIAS_FileName))
                            return false;
                        DBF_AMFC_JOIAS_FileName = sDBF_AMFC_JOIAS_FileName;

                        String sDBF_AMFC_QUOTA_FileName = Set_DBF_File("DBF_AMFC_QUOTA_FileName");
                        if (String.IsNullOrEmpty(sDBF_AMFC_QUOTA_FileName))
                            return false;
                        DBF_AMFC_QUOTA_FileName = sDBF_AMFC_QUOTA_FileName;

                        String sDBF_AMFC_CAIXA_FileName = Set_DBF_File("DBF_AMFC_CAIXA_FileName");
                        if (String.IsNullOrEmpty(sDBF_AMFC_CAIXA_FileName))
                            return false;
                        DBF_AMFC_CAIXA_FileName = sDBF_AMFC_CAIXA_FileName;

                        String sDBF_AMFC_INFRA_FileName = Set_DBF_File("DBF_AMFC_INFRA_FileName");
                        if (String.IsNullOrEmpty(sDBF_AMFC_INFRA_FileName))
                            return false;
                        DBF_AMFC_INFRA_FileName = sDBF_AMFC_INFRA_FileName;

                        String sDBF_AMFC_CEDEN_FileName = Set_DBF_File("DBF_AMFC_CEDEN_FileName");
                        if (String.IsNullOrEmpty(sDBF_AMFC_CEDEN_FileName))
                            return false;
                        DBF_AMFC_CEDEN_FileName = sDBF_AMFC_CEDEN_FileName;

                        String sDBF_AMFC_ESGOT_FileName = Set_DBF_File("DBF_AMFC_ESGOT_FileName");
                        if (String.IsNullOrEmpty(sDBF_AMFC_ESGOT_FileName))
                            return false;
                        DBF_AMFC_ESGOT_FileName = sDBF_AMFC_ESGOT_FileName;

                        String sDBF_AMFC_RECON_FileName = Set_DBF_File("DBF_AMFC_RECON_FileName");
                        if (String.IsNullOrEmpty(sDBF_AMFC_RECON_FileName))
                            return false;
                        DBF_AMFC_RECON_FileName = sDBF_AMFC_RECON_FileName;

                        String sDBF_AMFC_RECIB_FileName = Set_DBF_File("DBF_AMFC_RECIB_FileName");
                        if (String.IsNullOrEmpty(sDBF_AMFC_RECIB_FileName))
                            return false;
                        DBF_AMFC_RECIB_FileName = sDBF_AMFC_RECIB_FileName;

                        String sDBF_AMFC_PARAM_FileName = Set_DBF_File("DBF_AMFC_PARAM_FileName");
                        if (String.IsNullOrEmpty(sDBF_AMFC_PARAM_FileName))
                            return false;
                        DBF_AMFC_PARAM_FileName = sDBF_AMFC_PARAM_FileName;

                        return true;
                    }
                }
                catch (Exception ex)
                {
                    Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                    return false;
                }
            }

            /// <versions>02-12-2017(GesAMFC-v0.0.4.41)</versions>
            public String Set_DBF_File(String sTagXML)
            {
                try
                {
                    String sDFBFileName = String.Empty;
                    using (LibraryXML objLibraryXML = new LibraryXML(Program.XmlConfigFilePath))
                    {
                        sDFBFileName = objLibraryXML.GetXmlConfigFileNodeValue(sTagXML).Trim();
                        if (!DBF_File_Exists(sDFBFileName))
                            return null;

                        return sDFBFileName;
                    }
                }
                catch (Exception ex)
                {
                    Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                    return null;
                }
            }

            /// <versions>02-12-2017(GesAMFC-v0.0.4.41)</versions>
            public Boolean Set_Payments_Parameters_Settings()
            {
                try
                {
                    using (LibraryXML objLibraryXML = new LibraryXML(Program.XmlConfigFilePath))
                    {
                        DBF_AMFC_JOIA_VALOR = objLibraryXML.GetXmlConfigFileNodeValue("DBF_AMFC_JOIA_VALOR");
                        if (Program.SetPayCurrencyEuroDoubleValue(DBF_AMFC_JOIA_VALOR) <= 0) // NO futuro, criar e verificar em todo o lado  min e max values
                            return false;


                        DBF_AMFC_QUOTA_VALOR_MES = objLibraryXML.GetXmlConfigFileNodeValue("DBF_AMFC_QUOTA_VALOR_MES");
                        if (Program.SetPayCurrencyEuroDoubleValue(DBF_AMFC_QUOTA_VALOR_MES) <= 0) // NO futuro, criar e verificar em todo o lado  min e max values
                            return false;

                        Double dQuotaMonthValue = Program.SetPayCurrencyEuroDoubleValue(DBF_AMFC_QUOTA_VALOR_MES);
                        Double dQuotaYearValue = Convert.ToDouble(Decimal.Multiply(Convert.ToDecimal(dQuotaMonthValue), 12));
                        DBF_AMFC_QUOTA_VALOR_ANO = dQuotaYearValue.ToString();                        

                        DBF_AMFC_INFRA_VALOR_METRO = objLibraryXML.GetXmlConfigFileNodeValue("DBF_AMFC_INFRA_VALOR_METRO");
                        if (Program.SetPayCurrencyEuroDoubleValue(DBF_AMFC_INFRA_VALOR_METRO) <= 0) // NO futuro, criar e verificar em todo o lado  min e max values
                            return false;

                        DBF_AMFC_RECONV_VALOR_METRO = objLibraryXML.GetXmlConfigFileNodeValue("DBF_AMFC_RECONV_VALOR_METRO");
                        if (Program.SetPayCurrencyEuroDoubleValue(DBF_AMFC_RECONV_VALOR_METRO) <= 0) // NO futuro, criar e verificar em todo o lado  min e max values
                            return false;

                        DBF_AMFC_CEDENC_VALOR_METRO = objLibraryXML.GetXmlConfigFileNodeValue("DBF_AMFC_CEDENC_VALOR_METRO");
                        if (Program.SetPayCurrencyEuroDoubleValue(DBF_AMFC_CEDENC_VALOR_METRO) <= 0) // NO futuro, criar e verificar em todo o lado  min e max values
                            return false;

                        DBF_AMFC_ESGOT_VALOR_METRO = objLibraryXML.GetXmlConfigFileNodeValue("DBF_AMFC_ESGOT_VALOR_METRO");
                        if (Program.SetPayCurrencyEuroDoubleValue(DBF_AMFC_CEDENC_VALOR_METRO) < 0) //Esgotos não tem valor/m2 (= 0)
                            return false;

                        DBF_AMFC_Euro_To_Escudos = objLibraryXML.GetXmlConfigFileNodeValue("DBF_AMFC_Euro_To_Escudos");

                        return true;
                    }
                }
                catch (Exception ex)
                {
                    Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                    return false;
                }
            }

            /// <versions>28-04-2017(v0.0.2.48)</versions>
            public void Dispose() { }

            #region     Methods

            #region     SQL

            /// <versions>02-12-2017(GesAMFC-v0.0.4.41)</versions>
            public Boolean DBF_Dir_Exists()
            {
                try
                {
                    if (String.IsNullOrEmpty(this.DBF_AMFC_DirPath))
                        return false;
                    String sDBF_Dir_Path = this.AppPath + "\\" + this.DBF_AMFC_DirPath;
                    return Directory.Exists(sDBF_Dir_Path);
                }
                catch (Exception ex) 
                { 
                    Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                    return false;
                }
            }

            /// <versions>02-12-2017(GesAMFC-v0.0.4.41)</versions>
            public Boolean DBF_File_Exists(String sDBF_File_Name)
            {
                try
                {
                    if (String.IsNullOrEmpty(sDBF_File_Name.Trim()))
                       return false;
                    if (!DBF_Dir_Exists())
                        return false;
                    String sDBF_File_Path = this.DBF_AMFC_DirPath + "\\" + sDBF_File_Name.Trim() + "." + "DBF";
                    return File.Exists(sDBF_File_Path);
                }
                catch (Exception ex)
                {
                    Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                    return false;
                }
            }

            #endregion   SQL

            #region     Grid Methods

            /// <versions>07-05-2017(v0.0.3.6)</versions>
            public void GridConfiguration(GridControl objGridControl, GridView objGridView, Boolean bAllowGroup, Boolean bShowGroupPanel, Boolean bAutoExpandAllGroups, Boolean bAllowMultiSelect, Boolean bShowIndicator)
            {
                try
                {
                    Grid_View_Configuration(objGridControl, objGridView, bAllowGroup, bShowGroupPanel, bAutoExpandAllGroups, bAllowMultiSelect, bShowIndicator);
                }
                catch (Exception ex)
                {
                    Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false); 
                }
            }

            /// <versions>07-05-2017(v0.0.3.6)</versions>
            public void GridConfiguration(GridControl objGridControl, GridView objGridView, Boolean bAllowGroup, Boolean bAllowMultiSelect, Boolean bShowIndicator)
            {
                try
                {
                    Grid_View_Configuration(objGridControl, objGridView, bAllowGroup, false, false, bAllowMultiSelect, bShowIndicator);
                }
                catch (Exception ex) 
                { 
                    Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);  
                }
            }

            /// <versions>07-05-2017(v0.0.3.6)</versions>
            public void Grid_View_Configuration(GridControl objGridControl, GridView objGridView, Boolean bAllowGroup, Boolean bShowGroupPanel, Boolean bAutoExpandAllGroups, Boolean bAllowMultiSelect, Boolean bShowIndicator)
            {
                try
                {
                    objGridView.OptionsBehavior.AllowAddRows = DefaultBoolean.True;
                    objGridView.OptionsBehavior.AllowDeleteRows = DefaultBoolean.True;
                    objGridView.OptionsCustomization.AllowGroup = bAllowGroup;
                    objGridView.OptionsView.ShowGroupPanel = bShowGroupPanel;
                    objGridView.OptionsBehavior.AutoExpandAllGroups = bAutoExpandAllGroups;
                    objGridView.OptionsView.ShowAutoFilterRow = true;
                    objGridView.OptionsBehavior.AllowIncrementalSearch = false;
                    objGridView.OptionsBehavior.SummariesIgnoreNullValues = true;
                    objGridView.OptionsCustomization.AllowColumnMoving = true;
                    objGridView.OptionsNavigation.AutoFocusNewRow = true;
                    objGridView.OptionsNavigation.EnterMoveNextColumn = true;
                    #region Scroll Bars
                    objGridView.OptionsView.ColumnAutoWidth = false; //Need to show horizontal scroll
                    objGridView.ScrollStyle = ScrollStyleFlags.LiveHorzScroll | ScrollStyleFlags.LiveVertScroll;
                    objGridView.HorzScrollVisibility = ScrollVisibility.Always;
                    #endregion Scroll Bars
                    #region Embedded Navigator
                    objGridControl.UseEmbeddedNavigator = true;
                    objGridControl.EmbeddedNavigator.Enabled = true;
                    objGridControl.EmbeddedNavigator.Visible = true;
                    objGridControl.EmbeddedNavigator.ShowToolTips = true;
                    #region Default Buttons Set Invisible
                    objGridControl.EmbeddedNavigator.Buttons.Append.Enabled = false;
                    objGridControl.EmbeddedNavigator.Buttons.Append.Visible = false;
                    objGridControl.EmbeddedNavigator.Buttons.CancelEdit.Enabled = false;
                    objGridControl.EmbeddedNavigator.Buttons.CancelEdit.Visible = false;
                    objGridControl.EmbeddedNavigator.Buttons.Edit.Enabled = false;
                    objGridControl.EmbeddedNavigator.Buttons.Edit.Visible = false;
                    objGridControl.EmbeddedNavigator.Buttons.EndEdit.Enabled = false;
                    objGridControl.EmbeddedNavigator.Buttons.EndEdit.Visible = false;
                    objGridControl.EmbeddedNavigator.Buttons.Remove.Enabled = false;
                    objGridControl.EmbeddedNavigator.Buttons.Remove.Visible = false;
                    #endregion Default Buttons Set Invisible
                    #region Default Buttons Set Visible
                    #region Grid Pagination
                    objGridControl.EmbeddedNavigator.Buttons.First.Visible = true;
                    objGridControl.EmbeddedNavigator.Buttons.PrevPage.Visible = true;
                    objGridControl.EmbeddedNavigator.Buttons.Prev.Visible = true;
                    objGridControl.EmbeddedNavigator.Buttons.Next.Visible = true;
                    objGridControl.EmbeddedNavigator.Buttons.Last.Visible = true;
                    #endregion Grid Pagination
                    #endregion Default Buttons Set Visible
                    #endregion Embedded Navigator
                    if (bAllowGroup)
                        objGridView.GroupFormat = "[#image]{1} {2}";
                    objGridView.OptionsView.ShowAutoFilterRow = true;
                    objGridView.OptionsBehavior.Editable = true;
                    objGridView.OptionsSelection.MultiSelect = bAllowMultiSelect;
                    objGridView.OptionsView.ShowIndicator = bShowIndicator;
                    objGridView.OptionsView.ShowFilterPanelMode = ShowFilterPanelMode.ShowAlways;
                    objGridView.ColumnPanelRowHeight = 30;
                    CleanGrid(objGridControl, objGridView, true, true, true, true);
                }
                catch (Exception ex)
                {
                    Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false); 
                }
            }

            /// <versions>28-04-2017(v0.0.2.48)</versions>
            public void CleanGrid(GridControl objGridControl, GridView objGridView, Boolean bClearCols, Boolean bClearSorting, Boolean bClearFilters, Boolean bClearGrouping)
            {
                try
                {
                    objGridControl.Visible = true;
                    objGridControl.Show();
                    objGridControl.DataSource = null;
                    objGridView.OptionsView.ShowFooter = false;
                    objGridView.BeginUpdate();
                    if (bClearCols)
                        objGridView.Columns.Clear();
                    if (bClearSorting)
                        objGridView.ClearSorting();
                    if (bClearFilters)
                    {
                        objGridView.OptionsFilter.Reset();
                        objGridView.ActiveFilterEnabled = false;
                    }
                    if (bClearGrouping)
                        objGridView.ClearGrouping();
                    objGridView.EndUpdate();
                    objGridControl.Update();
                }
                catch (Exception ex) 
                { 
                    Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);  
                }
            }

            /// <versions>21-10-2017(v0.0.4.18)</versions>
            public void SetGridColumnOptions(GridControl objGridControl, GridView objGridView, String sColName, String sColCaption, String sColToolTip, Boolean bVisible, Int32 iVisibleIndex, Int32 iWidth, Boolean bReadOnly, Boolean bAllowEdit, Boolean bAllowFocus, Boolean bAllowGroup, Boolean bIsCurrencyCol)
            {
                try
                {
                    Set_Grid_Column_Options(objGridControl, objGridView, sColName, sColCaption, sColToolTip, bVisible, iVisibleIndex, iWidth, bReadOnly, bAllowEdit, bAllowFocus, bAllowGroup, -1, HorzAlignment.Center, VertAlignment.Center, HorzAlignment.Default, VertAlignment.Default, bIsCurrencyCol);
                }
                catch (Exception ex) 
                { 
                    Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);  
                }
            }

            /// <versions>21-10-2017(v0.0.4.18)</versions>
            public void SetGridColumnOptions(GridControl objGridControl, GridView objGridView, String sColName, String sColCaption, String sColToolTip, Boolean bVisible, Int32 iVisibleIndex, Int32 iWidth, Boolean bReadOnly, Boolean bAllowEdit, Boolean bAllowFocus, Boolean bAllowGroup)
            {
                try
                {
                    Set_Grid_Column_Options(objGridControl, objGridView, sColName, sColCaption, sColToolTip, bVisible, iVisibleIndex, iWidth, bReadOnly, bAllowEdit, bAllowFocus, bAllowGroup, -1, HorzAlignment.Center, VertAlignment.Center, HorzAlignment.Default, VertAlignment.Default, false);
                }
                catch (Exception ex)
                {
                    Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false); 
                }
            }

            /// <versions>21-10-2017(v0.0.4.18)</versions>
            public void SetGridColumnOptions(GridControl objGridControl, GridView objGridView, String sColName, String sColCaption, String sColToolTip, Boolean bVisible, Int32 iVisibleIndex, Int32 iWidth, Boolean bReadOnly, Boolean bAllowEdit, Boolean bAllowFocus, Boolean bAllowGroup, HorzAlignment eHeaderHAlign, VertAlignment eHeaderVAlign, HorzAlignment eCellHAlign, VertAlignment eCellVAlign)
            {
                try
                {
                    Set_Grid_Column_Options(objGridControl, objGridView, sColName, sColCaption, sColToolTip, bVisible, iVisibleIndex, iWidth, bReadOnly, bAllowEdit, bAllowFocus, bAllowGroup, -1, eHeaderHAlign, eHeaderVAlign, eCellHAlign, eCellVAlign, false);
                }
                catch (Exception ex)
                {
                    Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false); 
                }
            }

            /// <versions>21-10-2017(v0.0.4.18)</versions>
            public void SetGridColumnOptions(GridControl objGridControl, GridView objGridView, String sColName, String sColCaption, String sColToolTip, Boolean bVisible, Int32 iVisibleIndex, Int32 iWidth, Boolean bReadOnly, Boolean bAllowEdit, Boolean bAllowFocus, Boolean bAllowGroup, HorzAlignment eHeaderHAlign, VertAlignment eHeaderVAlign, HorzAlignment eCellHAlign, VertAlignment eCellVAlign, Boolean bIsCurrencyCol)
            {
                try
                {
                    Set_Grid_Column_Options(objGridControl, objGridView, sColName, sColCaption, sColToolTip, bVisible, iVisibleIndex, iWidth, bReadOnly, bAllowEdit, bAllowFocus, bAllowGroup, -1, eHeaderHAlign, eHeaderVAlign, eCellHAlign, eCellVAlign, bIsCurrencyCol);
                }
                catch (Exception ex)
                {
                    Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false); 
                }
            }

            /// <versions>21-10-2017(v0.0.4.18)</versions>
            public void SetGridColumnOptions(GridControl objGridControl, GridView objGridView, String sColName, String sColCaption, String sColToolTip, Boolean bVisible, Int32 iVisibleIndex, Int32 iWidth, Boolean bReadOnly, Boolean bAllowEdit, Boolean bAllowFocus, Boolean bAllowGroup, Int32 iGroupIdx, HorzAlignment eHeaderHAlign, VertAlignment eHeaderVAlign, HorzAlignment eCellHAlign, VertAlignment eCellVAlign)
            {
                try
                {
                    Set_Grid_Column_Options(objGridControl, objGridView, sColName, sColCaption, sColToolTip, bVisible, iVisibleIndex, iWidth, bReadOnly, bAllowEdit, bAllowFocus, bAllowGroup, iGroupIdx, eHeaderHAlign, eHeaderVAlign, eCellHAlign, eCellVAlign, false);
                }
                catch (Exception ex)
                {
                    Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false); 
                }
            }

            /// <versions>21-10-2017(v0.0.4.18)</versions>
            public void SetGridColumnOptions(GridControl objGridControl, GridView objGridView, String sColName, String sColCaption, String sColToolTip, Boolean bVisible, Int32 iVisibleIndex, Int32 iWidth, Boolean bReadOnly, Boolean bAllowEdit, Boolean bAllowFocus, Boolean bAllowGroup, Int32 iGroupIdx, HorzAlignment eHeaderHAlign, VertAlignment eHeaderVAlign, HorzAlignment eCellHAlign, VertAlignment eCellVAlign, Boolean bIsCurrencyCol)
            {
                try
                {
                    Set_Grid_Column_Options(objGridControl, objGridView, sColName, sColCaption, sColToolTip, bVisible, iVisibleIndex, iWidth, bReadOnly, bAllowEdit, bAllowFocus, bAllowGroup, iGroupIdx, eHeaderHAlign, eHeaderVAlign, eCellHAlign, eCellVAlign, bIsCurrencyCol);
                }
                catch (Exception ex)
                {
                    Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false); 
                }
            }

            /// <versions>21-10-2017(v0.0.4.18)</versions>
            public void Set_Grid_Column_Options(GridControl objGridControl, GridView objGridView, String sColName, String sColCaption, String sColToolTip, Boolean bVisible, Int32 iVisibleIndex, Int32 iWidth, Boolean bReadOnly, Boolean bAllowEdit, Boolean bAllowFocus, Boolean bAllowGroup, Int32 iGroupIdx, HorzAlignment eHeaderHAlign, VertAlignment eHeaderVAlign, HorzAlignment eCellHAlign, VertAlignment eCellVAlign, Boolean bIsCurrencyCol)
            {
                try
                {
                    GridColumn objGridColumn = objGridView.Columns[sColName];
                    if (objGridColumn == null)
                        return;
                    objGridColumn.Caption = sColCaption;
                    objGridColumn.ToolTip = sColToolTip;
                    objGridColumn.Visible = bVisible;
                    if (iVisibleIndex > -1)
                        objGridColumn.VisibleIndex = iVisibleIndex;
                    if (iWidth > 0)
                        objGridColumn.Width = iWidth;
                    OptionsColumn objColumnOptions = objGridColumn.OptionsColumn;
                    objColumnOptions.ReadOnly = bReadOnly;
                    objColumnOptions.AllowEdit = bAllowEdit;
                    objColumnOptions.AllowFocus = bAllowFocus;
                    if (bAllowGroup)
                    {
                        objColumnOptions.AllowGroup = DefaultBoolean.True;
                        if (iGroupIdx > -1)
                        {
                            objGridColumn.Visible = false;
                            objGridColumn.GroupIndex = iGroupIdx;
                        }
                    }
                    else
                        objColumnOptions.AllowGroup = DefaultBoolean.False;

                    if (eHeaderHAlign != HorzAlignment.Default)
                        objGridColumn.AppearanceHeader.TextOptions.HAlignment = eHeaderHAlign;
                    else
                        objGridColumn.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
                    if (eHeaderVAlign != VertAlignment.Default)
                        objGridColumn.AppearanceHeader.TextOptions.VAlignment = eHeaderVAlign;
                    else
                        objGridColumn.AppearanceHeader.TextOptions.VAlignment = VertAlignment.Center;

                    if (eCellHAlign != HorzAlignment.Default)
                        objGridColumn.AppearanceCell.TextOptions.HAlignment = eCellHAlign;
                    if (eCellVAlign != VertAlignment.Default)
                        objGridColumn.AppearanceCell.TextOptions.VAlignment = eCellVAlign;

                    if (bIsCurrencyCol)
                    {
                        objGridColumn.DisplayFormat.FormatType = FormatType.Numeric;
                        objGridColumn.DisplayFormat.FormatString = "c2";
                        //RepositoryItemTextEdit objTextEdit = new RepositoryItemTextEdit();
                        //objTextEdit.DisplayFormat.FormatType = FormatType.Numeric;
                        //objTextEdit.DisplayFormat.FormatString = "c2";
                        //objGridColumn.ColumnEdit = objTextEdit;
                    }
                }
                catch (Exception ex)
                {
                    Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false); 
                }
            }

            /// <versions>28-04-2017(v0.0.2.48)</versions>
            //public void SetGridColumOptionsFilter(GridView objGridView, String sColName, Boolean bAllowFilter, Boolean bAllowAutoFilter, AutoFilterCondition eAutoFilterCondition, float fFontSize, Boolean bHeaderBold)
            public void SetGridColumOptionsFilter(GridView objGridView, String sColName, Boolean bAllowFilter, Boolean bAllowAutoFilter, AutoFilterCondition eAutoFilterCondition, float fFontSize)
            {
                try
                {
                    GridColumn objGridColumn = objGridView.Columns[sColName];
                    if (objGridColumn == null)
                        return;
                    OptionsColumnFilter objColumnOptionsFilter = objGridColumn.OptionsFilter;
                    objColumnOptionsFilter.AllowAutoFilter = bAllowAutoFilter;
                    objColumnOptionsFilter.AllowFilter = bAllowFilter;
                    if (eAutoFilterCondition != AutoFilterCondition.Default)
                        objColumnOptionsFilter.AutoFilterCondition = eAutoFilterCondition;
                    if (fFontSize > 0)
                        objGridColumn.AppearanceCell.Font = new Font(objGridColumn.AppearanceCell.Font.FontFamily, fFontSize);
                    //if (bHeaderBold)
                        objGridColumn.AppearanceHeader.Font = new Font(objGridColumn.AppearanceHeader.Font.FontFamily, 10.0f, FontStyle.Bold);
                }
                catch (Exception ex) 
                { 
                    Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);  
                }
            }

            /// <versions>28-04-2017(v0.0.2.48)</versions>
            public int GetRowAt(GridView view, int x, int y)
            {
                return view.CalcHitInfo(new Point(x, y)).RowHandle;
            }

            /// <versions>28-04-2017(v0.0.2.48)</versions>
            public void SelectGridRows(GridView view, String sCheckColName, Int32 iStartRow, Int32 iEndRow)
            {
                try
                {
                    if (iStartRow > -1 && iEndRow > -1)
                    {
                        view.BeginSelection();
                        view.ClearSelection();
                        if (iEndRow < iStartRow)
                        {
                            Int32 iTempRow = iEndRow;
                            iEndRow = iStartRow;
                            iStartRow = iTempRow;
                        }
                        for (Int32 iRow = iStartRow; iRow < iEndRow; iRow++)
                        {
                            view.SelectRow(iRow);
                            view.SetRowCellValue(iRow, view.Columns[sCheckColName], true);
                        }
                        view.EndSelection();
                    }
                }
                catch (Exception ex) 
                { 
                    Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);  
                }
            }

            /// <versions>28-04-2017(v0.0.2.48)</versions>
            public Int32[] GetGridSelectedRows(GridView view, String sSelectFieldName)
            {
                StackFrame objStackFrame = new StackFrame();
                String sErrorMsg = String.Empty;
                try
                {
                    List<Int32> listSelectedRows = new List<Int32>();
                    for (int iRow = 0; iRow < view.DataRowCount; iRow++)
                    {
                        if (Convert.ToBoolean(view.GetRowCellValue(iRow, view.Columns[sSelectFieldName])))
                            listSelectedRows.Add(iRow);
                    }
                    return listSelectedRows.ToArray();
                }
                catch (Exception ex) 
                { 
                    Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, true); 
                    return null; 
                }
                finally { objStackFrame = null; }
            }

            /// <versions>28-04-2017(v0.0.2.48)</versions>
            public Int32 GetRowHandleByColumnsValues(GridView view, List<String> arrColumnFieldNames, List<object> values)
            {
                StackFrame objStackFrame = new StackFrame();
                String sErrorMsg = String.Empty;
                try
                {
                    if (arrColumnFieldNames.Count != values.Count)
                    {
                        sErrorMsg = "Columns Name and Values different!!";
                        Program.HandleError(objStackFrame.GetMethod().Name, sErrorMsg, Program.ErroType.ERROR, true, false);
                        return -1;
                    }
                    Int32 iRowHandle = GridControl.InvalidRowHandle;
                    for (int i = 0; i < view.RowCount; i++)
                    {
                        Boolean bFoundIt = true;
                        for (int j = 0; j < arrColumnFieldNames.Count; j++)
                        {
                            object a = view.GetDataRow(i)[arrColumnFieldNames[j]];
                            object b = values[j];
                            if (view.GetDataRow(i)[arrColumnFieldNames[j]].ToString() != values[j].ToString())
                                bFoundIt = false;
                        }
                        if (bFoundIt)
                            return i;
                    }
                    return iRowHandle;
                }
                catch (Exception ex) 
                {
                    Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, true); 
                    return -1; 
                }
                finally { objStackFrame = null; }
            }

            #endregion  Grid Methods

            #region     Grid LayoutView Methods

            /// <versions>28-04-2017(v0.0.2.48)</versions>
            public void Set_LayoutView_Column_Options(LayoutView objLayoutView, String sColName, String sColCaption, String sColToolTip, Boolean bVisible, Int32 iVisibleIndex, Int32 iWidth, Boolean bReadOnly, Boolean bAllowEdit, Boolean bAllowFocus, Boolean bAllowGroup, Boolean bIsCheckButton)
            {
                try
                {
                    LayoutViewColumn objLayoutViewColumn = objLayoutView.Columns[sColName];
                    if (objLayoutViewColumn == null)
                        return;
                    objLayoutViewColumn.Caption = sColCaption;

                    objLayoutViewColumn.LayoutViewField.TextSize = new Size(120, 20); //Header
                    objLayoutViewColumn.LayoutViewField.OptionsToolTip.ToolTip = sColToolTip;

                    objLayoutViewColumn.LayoutViewField.AppearanceItemCaption.Options.UseFont = true;
                    objLayoutViewColumn.LayoutViewField.AppearanceItemCaption.Options.UseTextOptions = true;
                    objLayoutViewColumn.LayoutViewField.AppearanceItemCaption.TextOptions.HAlignment = HorzAlignment.Near;
                    objLayoutViewColumn.LayoutViewField.AppearanceItemCaption.TextOptions.WordWrap = WordWrap.Wrap;
                    //objLayoutViewColumn.LayoutViewField.AppearanceItemCaption.Font.Style = FontStyle.Regular;
                    objLayoutViewColumn.LayoutViewField.Visibility = bVisible ?  DevExpress.XtraLayout.Utils.LayoutVisibility.Always : DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                    
                     //objLayoutViewColumn.ToolTip = sColToolTip;
                    objLayoutViewColumn.Visible = bVisible;
                    if (iVisibleIndex > -1)
                        objLayoutViewColumn.VisibleIndex = iVisibleIndex;
                    if (iWidth > 0)
                        objLayoutViewColumn.Width = iWidth;
                    OptionsColumn objColumnOptions = objLayoutViewColumn.OptionsColumn;
                    objColumnOptions.ReadOnly = bReadOnly;
                    objColumnOptions.AllowEdit = bAllowEdit;
                    objColumnOptions.AllowFocus = bAllowFocus;
                    //if (bAllowGroup)
                    //    objColumnOptions.AllowGroup = DefaultBoolean.True;
                    //else
                    //    objColumnOptions.AllowGroup = DefaultBoolean.False;

                    if (bIsCheckButton)
                    {
                        RepositoryItemCheckEdit checkEdit = new RepositoryItemCheckEdit();
                        checkEdit.GlyphAlignment = HorzAlignment.Near;
                        checkEdit.Caption = String.Empty;
                        objLayoutViewColumn.ColumnEdit = checkEdit;
                    }
                }
                catch (Exception ex)
                {
                    Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false); 
                }
            }

            #endregion  Grid LayoutView Methods

            #region     CheckedListBox Methods

            /// <versions>28-04-2017(v0.0.2.48)</versions>
            //public void CheckedListBoxControl_Click(CheckedListBoxControl ctrlCheckedList)
            //{
            //    try
            //    {
            //        Boolean bCheckOnSelect = true;
            //        if (!bCheckOnSelect)
            //        {
            //            ctrlCheckedList.SelectedIndex = -1; //Remove Selection
            //            return;
            //        }
            //        Int32 iSelItemIdx = ctrlCheckedList.SelectedIndex;
            //        if (iSelItemIdx > -1)
            //        {
            //            if (!ctrlCheckedList.Items[iSelItemIdx].Enabled)
            //                return;
            //            CheckState eSelItem_CheckState = ctrlCheckedList.Items[iSelItemIdx].CheckState;
            //            ctrlCheckedList.Items[iSelItemIdx].CheckState = (eSelItem_CheckState == CheckState.Checked) ? CheckState.Unchecked : CheckState.Checked;
            //        }
            //    }
            //    catch (Exception ex) 
            //    { 
            //        Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);  
            //    }
            //}

            #endregion  CheckedListBox Methods

            #region     Search Text Methods
            
            /// <versions>03-05-2017(v0.0.2.51)</versions>
            public List<String> SplitStringWords(String sInputString)
            {
                try
                {
                    if (String.IsNullOrEmpty(sInputString))
                        return null;
                    String[] arrListWords = sInputString.Split(new String[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                    List<String> objListWords = new List<String>(arrListWords);
                    return objListWords;
                }
                catch (Exception ex)
                {
                    Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false); 
                    return null;
                }
            }

            /// <versions>03-05-2017(v0.0.2.51)</versions>
            public List<String> RemoveWordsNotToSearch(List<String> objListWords)
            {
                try
                {
                    if (objListWords == null || objListWords.Count == 0)
                        return null;
                    List<String> objListWordNotToSearch = new List<String>() { ",", ".", "de", "da", "do", "das", "dos", "rua", "r", "avenida", "av", "praça", "praceta", "pc", "pctª", "trv" ,"trav", "calçada" };
                    List<String> objListWordsCleaned = new List<String>();
                    objListWordsCleaned = objListWords;
                    List<String> ListWordToRemove = new List<String>();
                    foreach (String sWordInList in objListWordsCleaned)
                    {
                        foreach (String sWordNotToSearch in objListWordNotToSearch)
                        {
                            if (sWordInList.Trim().ToLower() == sWordNotToSearch.Trim().ToLower())
                            {
                                ListWordToRemove.Add(sWordInList);
                                break;
                            }
                        }
                    }
                    if (ListWordToRemove.Count > 0)
                        objListWordsCleaned.RemoveAll(w => ListWordToRemove.Contains(w));
                    return objListWordsCleaned;
                }
                catch (Exception ex)
                {
                    Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false); 
                    return objListWords;
                }
            }

            /// <versions>03-05-2017(v0.0.2.51)</versions>
            public Boolean ContainsMajorityWords(List<String> objListStringWords, List<String> objListWordsToSearch, Double dMinPercToFound, Double dMaxPercToFound)
            {
                try
                {
                    if (objListStringWords == null || objListStringWords.Count == 0 || objListWordsToSearch == null || objListWordsToSearch.Count == 0)
                        return false;

                    #region     Min Percentag eWords To Found Calc
                    Double dMinPercentageWordsToFound = (dMinPercToFound > 0) ? dMinPercToFound : 0.40;
                    Double dMaxPercentageWordsToFound = (dMaxPercToFound > 0) ? dMinPercToFound : 0.80;
                    Double dPercentageWordsToFound = 0.40;
                    if (objListStringWords.Count <= objListWordsToSearch.Count)
                        dPercentageWordsToFound = 0.65;
                    else
                        dPercentageWordsToFound = Math.Round(Convert.ToDouble(Convert.ToDouble(objListWordsToSearch.Count) / Convert.ToDouble(objListStringWords.Count)), 2);
                    if (dPercentageWordsToFound < dMinPercentageWordsToFound)
                        dPercentageWordsToFound = dMinPercentageWordsToFound;
                    if (dPercentageWordsToFound > dMaxPercentageWordsToFound)
                        dPercentageWordsToFound = dMaxPercentageWordsToFound;
                    #endregion Min Percentag eWords To Found Calc

                    #region     Search each word
                    List<String> objListWordsFoundInString = new List<String>();
                    Int32 iWordsFoundCnt = 0;
                    foreach (String sStringWord in objListStringWords)
                    {
                        foreach (String sWordToSearch in objListWordsToSearch)
                        {
                            if (sStringWord.Trim().ToLower() == sWordToSearch.Trim().ToLower())
                            {
                                iWordsFoundCnt++;
                                break;
                            }
                        }
                    }
                    #endregion  Search each word

                    Double dPercentageWordsMatch = Convert.ToDouble(iWordsFoundCnt * 100 / objListStringWords.Count) ;
                    Boolean bItMatches = dPercentageWordsMatch >= dPercentageWordsToFound;
                    return bItMatches;
                }
                catch (Exception ex)
                {
                    Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false); 
                    return false;
                }
            }

            #endregion  Search Text Methods

            #endregion  Methods
        }
    }
}
