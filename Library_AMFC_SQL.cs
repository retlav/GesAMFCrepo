using GesAMFC.AMFC_Methods;
using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Diagnostics;
using System.Globalization;
using System.Windows.Forms;

namespace GesAMFC
{
    /// <summary>AMFC SQL Library</summary>
    /// <author>Valter Lima</author>
    /// <creation>18-02-2017(v0.0.1.0)</creation>
    /// <versions>28-02-2018(GesAMFC-v1.0.0.3)</versions>
    public class Library_AMFC_SQL : IDisposable
    {
        #region     Properties
        public String OLE_DB_ConnectionString;
        public Library_AMFC_Methods LibAMFC;

        #region     Cash Payments Columns

        #region     SQL Cash Payments Columns (DB_CAIXA.DBF)
        public String Payments_DB_Col_ID                = "ID";
        public String Payments_DB_Col_LISTARECNU        = "LISTARECNU";
        public String Payments_DB_Col_SOCIO             = "SOCIO";
        public String Payments_DB_Col_NOME              = "NOME";
        public String Payments_DB_Col_NOME_PAG          = "NOME_PAG";
        public String Payments_DB_Col_DESIGNACAO        = "DESIGNACAO";
        public String Payments_DB_Col_VALOR             = "VALOR";
        public String Payments_DB_Col_DATA              = "DATA";
        public String Payments_DB_Col_ALTERADO          = "ALTERADO";
        public String Payments_DB_Col_NOTAS             = "NOTAS";

        public String Payments_DB_Col_ESTADO            = "ESTADO";

        public String Payments_DB_Col_JOIA              = "JOIA";
        public String Payments_DB_Col_JOIADESC          = "JOIADESC";
        public String Payments_DB_Col_JOIAVAL           = "JOIAVAL";
        public String Payments_DB_Col_ASSOCJOIA         = "ASSOCJOIA";
        public String Payments_DB_Col_DASSOCJOIA        = "DASSOCJOIA";

        public String Payments_DB_Col_QUOTAS            = "QUOTAS";
        public String Payments_DB_Col_QUOTASDESC        = "QUOTASDESC";
        public String Payments_DB_Col_QUOTASVAL         = "QUOTASVAL";
        public String Payments_DB_Col_ASSOCQUOTA        = "ASSOCQUOTA";
        public String Payments_DB_Col_DASSOCQUOT        = "DASSOCQUOT";

        public String Payments_DB_Col_INFRAEST          = "INFRAEST";
        public String Payments_DB_Col_INFRADESC         = "INFRADESC";
        public String Payments_DB_Col_INFRAVAL          = "INFRAVAL";
        public String Payments_DB_Col_ASSOCNFRA         = "ASSOCNFRA";
        public String Payments_DB_Col_DASSOCINFR        = "DASSOCINFR";

        public String Payments_DB_Col_CEDENCIAS         = "CEDENCIAS";
        public String Payments_DB_Col_CEDENCDESC        = "CEDENCDESC";
        public String Payments_DB_Col_CEDENCVAL         = "CEDENCVAL";
        public String Payments_DB_Col_ASSOCCEDEN        = "ASSOCCEDEN";
        public String Payments_DB_Col_DASSOCCEDE        = "DASSOCCEDE";

        public String Payments_DB_Col_ESGOT             = "ESGOT";
        public String Payments_DB_Col_ESGOTDESC         = "ESGOTDESC";
        public String Payments_DB_Col_ESGOTVAL          = "ESGOTVAL";
        public String Payments_DB_Col_ASSOCESGOT        = "ASSOCESGOT";
        public String Payments_DB_Col_DASSOCESGO        = "DASSOCESGO";

        public String Payments_DB_Col_RECONV            = "RECONV";
        public String Payments_DB_Col_RECONDESC         = "RECONDESC";
        public String Payments_DB_Col_RECONVAL          = "RECONVAL";
        public String Payments_DB_Col_ASSOCRECON        = "ASSOCRECON";
        public String Payments_DB_Col_DASSOCRECO        = "DASSOCRECO";

        public String Payments_DB_Col_OUTRO             = "OUTRO";
        public String Payments_DB_Col_OUTROSDESC        = "OUTROSDESC";
        public String Payments_DB_Col_OUTROSVAL         = "OUTROSVAL";
        public String Payments_DB_Col_ASSOCOUTRO        = "ASSOCOUTRO";
        public String Payments_DB_Col_DASSOCOUTR        = "DASSOCOUTR";

        #endregion  SQL Cash Payments Columns (DB_Caixa.dbf)

        #region     Collection Cash Payments Columns
        public String Payments_Col_Idx              = "Idx";
        public String Payments_Col_ID               = "ID";
        public String Payments_Col_LISTARECNU       = "LISTARECNU";
        public String Payments_Col_SOCIO            = "SOCIO";
        public String Payments_Col_NOME             = "NOME";
        public String Payments_Col_NOME_PAG         = "NOME_PAG";
        public String Payments_Col_DESIGNACAO       = "DESIGNACAO";
        public String Payments_Col_NOTAS            = "NOTAS";

        public String Payments_Col_VALOR            = "VALOR";
        public String Payments_Col_VALOR_Str        = "VALOR_Str";

        public String Payments_Col_DATA             = "DATA";
        public String Payments_Col_DATA_Str         = "DATA_Str";
        public String Payments_Col_DATAYearInt      = "DATAYearInt";
        public String Payments_Col_DATAYear         = "DATAYear";
        public String Payments_Col_DATAMonthInt     = "DATAMonthInt";
        public String Payments_Col_DATAMonth        = "DATAMonth"; 
        public String Payments_Col_DATAMonthYear    = "DATAMonthYear";
        public String Payments_Col_ALTERADO         = "ALTERADO";
        public String Payments_Col_ALTERADO_Str     = "ALTERADO_Str";

        public String Payments_Col_ESTADO = "ESTADO";

        public String Payments_Col_JOIADESC         = "JOIADESC";
        public String Payments_Col_JOIA             = "JOIA";
        public String Payments_Col_HasJOIA          = "HasJOIA";
        public String Payments_Col_JOIAVAL          = "JOIAVAL";
        public String Payments_Col_JOIAVAL_Str      = "JOIAVAL_Str";
        public String Payments_Col_ASSOCJOIA        = "ASSOCJOIA";
        public String Payments_Col_IsASSOCJOIA      = "IsASSOCJOIA";
        public String Payments_Col_DASSOCJOIA       = "DASSOCJOIA";
        public String Payments_Col_DASSOCJOIA_Str   = "DASSOCJOIA_Str";

        public String Payments_Col_QUOTASDESC       = "QUOTASDESC";
        public String Payments_Col_QUOTAS           = "QUOTAS";
        public String Payments_Col_HasQUOTAS        = "HasQUOTAS";
        public String Payments_Col_QUOTASVAL        = "QUOTASVAL";
        public String Payments_Col_QUOTASVAL_Str    = "QUOTASVAL_Str";
        public String Payments_Col_ASSOCQUOTA       = "ASSOCQUOTA";
        public String Payments_Col_IsASSOCQUOTA     = "IsASSOCQUOTA";
        public String Payments_Col_DASSOCQUOT       = "DASSOCQUOT";
        public String Payments_Col_DASSOCQUOT_Str   = "DASSOCQUOT_Str"; 
        
        public String Payments_Col_INFRADESC        = "INFRADESC";
        public String Payments_Col_INFRAEST         = "INFRAEST";
        public String Payments_Col_HasINFRAEST      = "HasINFRAEST";
        public String Payments_Col_INFRAVAL         = "INFRAVAL";
        public String Payments_Col_INFRAVAL_Str     = "INFRAVAL_Str";
        public String Payments_Col_ASSOCNFRA        = "ASSOCNFRA";
        public String Payments_Col_IsASSOCNFRA      = "IsASSOCNFRA";
        public String Payments_Col_DASSOCINFR       = "DASSOCINFR";
        public String Payments_Col_DASSOCINFR_Str   = "DASSOCINFR_Str";

        public String Payments_Col_CEDENCDESC       = "CEDENCDESC";
        public String Payments_Col_CEDENCIAS        = "CEDENCIAS";
        public String Payments_Col_HasCEDENCIAS     = "HasCEDENCIAS";
        public String Payments_Col_CEDENCVAL        = "CEDENCVAL";
        public String Payments_Col_CEDENCVAL_Str    = "CEDENCVAL_Str";
        public String Payments_Col_ASSOCCEDEN       = "ASSOCCEDEN";
        public String Payments_Col_IsASSOCCEDEN     = "IsASSOCCEDE";
        public String Payments_Col_DASSOCCEDE       = "DASSOCCEDE";
        public String Payments_Col_DASSOCCEDE_Str   = "DASSOCCEDE_Str";

        public String Payments_Col_ESGOTDESC        = "ESGOTDESC";
        public String Payments_Col_ESGOT            = "ESGOT";
        public String Payments_Col_HasESGOT         = "HasESGOT";
        public String Payments_Col_ESGOTVAL         = "ESGOTVAL";
        public String Payments_Col_ESGOTVAL_Str     = "ESGOTVAL_Str";
        public String Payments_Col_ASSOCESGOT       = "ASSOCESGOT";
        public String Payments_Col_IsASSOCESGO      = "IsASSOCESGO";
        public String Payments_Col_DASSOCESGO       = "DASSOCESGO";
        public String Payments_Col_DASSOCESGO_Str   = "DASSOCESGO_Str";

        public String Payments_Col_RECONDESC        = "RECONDESC";
        public String Payments_Col_RECONV           = "RECONV";
        public String Payments_Col_HasRECONV        = "HasRECONV";
        public String Payments_Col_RECONVAL         = "RECONVAL";
        public String Payments_Col_RECONVAL_Str     = "RECONVAL_Str";
        public String Payments_Col_ASSOCRECON       = "ASSOCRECON";
        public String Payments_Col_IsASSOCRECON     = "IsASSOCRECON";
        public String Payments_Col_DASSOCRECO       = "DASSOCRECO";
        public String Payments_Col_DASSOCRECO_Str   = "DASSOCRECO_Str";

        public String Payments_Col_OUTROSDESC       = "OUTROSDESC";
        public String Payments_Col_OUTRO            = "OUTRO";
        public String Payments_Col_HasOUTRO         = "HasOUTRO";
        public String Payments_Col_OUTROSVAL        = "OUTROSVAL";
        public String Payments_Col_OUTROSVAL_Str    = "OUTROSVAL_Str";
        public String Payments_Col_ASSOCOUTRO       = "ASSOCOUTRO";
        public String Payments_Col_IsASSOCOUTRO     = "IsASSOCOUTRO";
        public String Payments_Col_DASSOCOUTR       = "DASSOCOUTR";
        public String Payments_Col_DASSOCOUTR_Str   = "DASSOCOUTR_Str";

        #endregion  Collection Cash Payments Columns

        #endregion  Cash Payments Columns

        #endregion  Properties

        #region     Constructor

        /// <versions>28-04-2017(v0.0.2.48)</versions>
        public Library_AMFC_SQL()
        {
            try
            {
                OLE_DB_ConnectionString = String.Empty;
                LibAMFC = new Library_AMFC_Methods();
                OLE_DB_Settings();
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>28-04-2017(v0.0.2.48)</versions>
        public void Dispose() 
        {
            LibAMFC.Dispose();
        }

        #endregion  Constructor

        #region     OLE DB Connection

        /// <versions>28-04-2017(v0.0.2.48)</versions>
        public void Set_OLE_DB_ConnectionString()
        {
            try
            {
                if (String.IsNullOrEmpty(LibAMFC.OLE_DB_Provider) || String.IsNullOrEmpty(LibAMFC.DBF_AMFC_DirPath))
                    OLE_DB_Settings();
                if (String.IsNullOrEmpty(OLE_DB_ConnectionString))
                {
                    OLE_DB_ConnectionString = String.Empty;
                    OLE_DB_ConnectionString += @"Provider="     + LibAMFC.OLE_DB_Provider   + ";";
                    OLE_DB_ConnectionString += @"Data Source="  + LibAMFC.DBF_AMFC_DirPath  + ";";
                    OLE_DB_ConnectionString += @"Extended Properties=dBASE III;";
                }
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>02-12-2017(GesAMFC-v0.0.4.41)</versions>
        public Boolean OLE_DB_Settings()
        {
            try
            {
                return LibAMFC.Set_SQL_DB_Settings();
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return false;
            }
        }

        #endregion  OLE DB Connection

        #region     AMFC DB Methods

        #region     Admin Members

        /// <versions>08-10-2017(v0.0.4.8)</versions>
        private AMFCMembers Get_Find_Member_DB_Data(OleDbCommand objOleDbCommand)
        {
            try
            {
                AMFCMembers objListMembers = new AMFCMembers();
                using (OleDbDataReader objOleDbDataReader = objOleDbCommand.ExecuteReader())
                {
                    if (!objOleDbDataReader.HasRows)
                        return new AMFCMembers();
                    while (objOleDbDataReader.Read())
                    {
                        AMFCMember objMember = new AMFCMember();
                        if (objOleDbDataReader["SOCIONUMERO"] == DBNull.Value)
                            continue;
                        objMember.NUMERO = Convert.ToInt64(objOleDbDataReader["SOCIONUMERO"]);
                        if (objMember.NUMERO < new AMFCMember().MinNumber || objMember.NUMERO > objMember.MaxNumber)
                            continue;

                        if (objOleDbDataReader["SOCIONOME"] == DBNull.Value)
                            continue;
                        objMember.NOME = Program.EncodeStringToISO(objOleDbDataReader["SOCIONOME"].ToString());

                        if (objOleDbDataReader["DATAADMI"] != DBNull.Value)
                             objMember.DATAADMI = Program.ConvertToValidDateTime(objOleDbDataReader["DATAADMI"].ToString().Trim()).ToString(Program.Date_Format_String, CultureInfo.CurrentCulture);

                        if (objOleDbDataReader["AREALOTE"] != DBNull.Value)
                            objMember.AREALOTE = Convert.ToInt32(objOleDbDataReader["AREALOTE"].ToString().Trim());

                        objListMembers.Members.Add(objMember);
                    }
                }
                return objListMembers;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return null;
            }
        }

        /// <versions>03-05-2017(v0.0.2.51)</versions>
        public AMFCMembers Get_List_Members()
        {
            String sQueryString = String.Empty;
            try
            {
                AMFCMembers objListMembers = new AMFCMembers();
                Set_OLE_DB_ConnectionString();
                if (String.IsNullOrEmpty(LibAMFC.DBF_AMFC_SOCIO_FileName))
                    OLE_DB_Settings();
                sQueryString = "SELECT * FROM " + LibAMFC.DBF_AMFC_SOCIO_FileName + " ORDER BY " + new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMERO);
                using (OleDbConnection objOleDbConnection = new OleDbConnection(OLE_DB_ConnectionString))
                {
                    objOleDbConnection.Open();
                    using (OleDbCommand objOleDbCommand = new OleDbCommand())
                    {
                        objOleDbCommand.Connection = objOleDbConnection;
                        objOleDbCommand.CommandText = sQueryString;

                        objListMembers = Get_Member_DB_Data(objOleDbCommand, false);
                        if (objListMembers == null || objListMembers.Members == null || objListMembers.Members.Count == 0)
                        {
                            StackFrame objStackFrame = new StackFrame();
                            String sErrorMsg = "Não foi possível obter a Lista de Sócios da Base de Dados!" + " -> " + "QUERY: " + sQueryString;
                            Program.HandleError(objStackFrame.GetMethod().Name, sErrorMsg, Program.ErroType.ERROR, true, true);
                            objStackFrame = null;
                            return null;
                        }
                    }
                }

                return objListMembers;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message + " -> " + "QUERY: " + sQueryString, Program.ErroType.EXCEPTION, true, false);
                return null;
            }
        }

        /// <versions>03-05-2017(v0.0.2.51)</versions>
        public AMFCMember Get_Member_By_Number(Int64 lMemberNumber)
        {
            String sQueryString = String.Empty;
            try
            {
                AMFCMember objMember = new AMFCMember();

                Set_OLE_DB_ConnectionString();
                if (String.IsNullOrEmpty(LibAMFC.DBF_AMFC_SOCIO_FileName))
                    OLE_DB_Settings();

                using (OleDbConnection objOleDbConnection = new OleDbConnection(OLE_DB_ConnectionString))
                {
                    objOleDbConnection.Open();
                    using (OleDbCommand objOleDbCommand = new OleDbCommand())
                    {
                        objOleDbCommand.Connection = objOleDbConnection;

                        #region     Check if there is one single member with this number
                        sQueryString = "SELECT COUNT(*) FROM " + LibAMFC.DBF_AMFC_SOCIO_FileName + " WHERE " + new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMERO) + " = " + lMemberNumber;
                        objOleDbCommand.CommandText = sQueryString;
                        if (Convert.ToInt32(objOleDbCommand.ExecuteScalar()) != 1)
                            return null;
                        #endregion  Check if there is one single member with this number

                        #region     Get the member with this number
                        sQueryString = "SELECT * FROM " + LibAMFC.DBF_AMFC_SOCIO_FileName + " WHERE " + new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMERO) + " = " + lMemberNumber;
                        objOleDbCommand.CommandText = sQueryString;
                        AMFCMembers objListMembers = Get_Member_DB_Data(objOleDbCommand, true);
                        if (objListMembers == null || objListMembers.Members == null || objListMembers.Members.Count > 1)
                        {
                            StackFrame objStackFrame = new StackFrame();
                            String sErrorMsg = "Não foi possível obter o Sócio com o Nº: " + lMemberNumber + " -> " + "QUERY: " + sQueryString;
                            Program.HandleError(objStackFrame.GetMethod().Name, sErrorMsg, Program.ErroType.ERROR, true, true);
                            objStackFrame = null;
                            return null;
                        }
                        if (objListMembers.Members.Count == 1)
                            objMember = objListMembers.Members[0];
                        #endregion     Get the member with this number
                    }
                }

                return objMember;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message + " -> " + "QUERY: " + sQueryString, Program.ErroType.EXCEPTION, true, false);
                return null;
            }
        }

        /// <versions>04-05-2017(v0.0.2.53)</versions>
        public AMFCMembers Find_Members(AMFCMembers objListDBallMemmers, AMFCMember objMemberFieldsToSearch)
        {
            String sQueryString = String.Empty;
            try
            {
                Set_OLE_DB_ConnectionString();
                if (String.IsNullOrEmpty(LibAMFC.DBF_AMFC_SOCIO_FileName))
                    OLE_DB_Settings();

                AMFCMembers objSerarchMembersResult = new AMFCMembers();

                using (OleDbConnection objOleDbConnection = new OleDbConnection(OLE_DB_ConnectionString))
                {
                    objOleDbConnection.Open();
                    using (OleDbCommand objOleDbCommand = new OleDbCommand())
                    {
                        objOleDbCommand.Connection = objOleDbConnection;

                        #region     Get DB Members that match the search fields text

                        #region         Query Where Search Fields Clause
                        String sQueryWhere = String.Empty;
                        if (!String.IsNullOrEmpty(objMemberFieldsToSearch.NOME.Trim()))
                        {
                            if (!String.IsNullOrEmpty(sQueryWhere))
                                sQueryWhere += "OR" + " " + "\n";
                            sQueryWhere += new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NOME) + " LIKE " + "'" + "%" + objMemberFieldsToSearch.NOME.Trim().ToUpper() + "%" + "'" + " " + "\n";
                        }
                        if (!String.IsNullOrEmpty(objMemberFieldsToSearch.MORADA.Trim()))
                        {
                            if (!String.IsNullOrEmpty(sQueryWhere))
                                sQueryWhere += "OR" + " " + "\n";
                            sQueryWhere += new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.MORADA) + " LIKE " + "'" + "%" + objMemberFieldsToSearch.MORADA.Trim().ToUpper() + "%" + "'" + " " + "\n";
                            if (!String.IsNullOrEmpty(sQueryWhere))
                                sQueryWhere += "OR" + " " + "\n";
                            sQueryWhere += new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.MORLOTE) + " LIKE " + "'" + "%" + objMemberFieldsToSearch.MORADA.Trim() + "%" + "'" + " " + "\n";
                            if (!String.IsNullOrEmpty(sQueryWhere))
                                sQueryWhere += "OR" + " " + "\n";
                            sQueryWhere += new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.CPOSTAL) + " LIKE " + "'" + "%" + objMemberFieldsToSearch.MORADA.Trim().ToUpper() + "%" + "'" + " " + "\n";
                        }
                        if (!String.IsNullOrEmpty(objMemberFieldsToSearch.CPOSTAL.Trim()))
                        {
                            if (!String.IsNullOrEmpty(sQueryWhere))
                                sQueryWhere += "OR" + " " + "\n";
                            sQueryWhere += new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.CPOSTAL) + " LIKE " + "'" + "%" + objMemberFieldsToSearch.CPOSTAL.Trim().ToUpper() + "%" + "'" + " " + "\n";
                            if (!String.IsNullOrEmpty(sQueryWhere))
                                sQueryWhere += "OR" + " " + "\n";
                            sQueryWhere += new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.MORADA) + " LIKE " + "'" + "%" + objMemberFieldsToSearch.CPOSTAL.Trim().ToUpper() + "%" + "'" + " " + "\n";
                            if (!String.IsNullOrEmpty(sQueryWhere))
                                sQueryWhere += "OR" + " " + "\n";
                            sQueryWhere += new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.MORLOTE) + " LIKE " + "'" + "%" + objMemberFieldsToSearch.CPOSTAL.Trim() + "%" + "'" + " " + "\n";
                        }
                        if (!String.IsNullOrEmpty(objMemberFieldsToSearch.NUMLOTE.Trim()))
                        {
                            if (!String.IsNullOrEmpty(sQueryWhere))
                                sQueryWhere += "OR" + " " + "\n";
                            sQueryWhere += new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMLOTE) + " LIKE " + "'" + "%" + objMemberFieldsToSearch.NUMLOTE.Trim().ToUpper() + "%" + "'" + " " + "\n";
                            if (!String.IsNullOrEmpty(sQueryWhere))
                                sQueryWhere += "OR" + " " + "\n";
                            sQueryWhere += new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.MORADA) + " LIKE " + "'" + "%" + objMemberFieldsToSearch.NUMLOTE.Trim().ToUpper() + "%" + "'" + " " + "\n";
                            if (!String.IsNullOrEmpty(sQueryWhere))
                                sQueryWhere += "OR" + " " + "\n";
                            sQueryWhere += new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.MORLOTE) + " LIKE " + "'" + "%" + objMemberFieldsToSearch.NUMLOTE.Trim().ToUpper() + "%" + "'" + " " + "\n";
                        }
                        #endregion  Query Where Search Fields Claus

                        #region     Check if any member match the search fields criteria
                        sQueryString  = String.Empty;
                        sQueryString  = "SELECT COUNT(*) " + " "  + "\n";
                        sQueryString += "FROM " + LibAMFC.DBF_AMFC_SOCIO_FileName + " "  + "\n";
                        sQueryString += "WHERE" + " " + "\n";
                        sQueryString += sQueryWhere + " " + "\n";
                        objOleDbCommand.CommandText = sQueryString;
                        Int32 iFoundMembersCnt = Convert.ToInt32(objOleDbCommand.ExecuteScalar());
                        #endregion  Check if any member match the search fields criteria

                        #region     Get members that match the search fields criteria
                        if (iFoundMembersCnt > 0)
                        {
                            sQueryString = String.Empty;
                            sQueryString = "SELECT * " + " " + "\n";
                            sQueryString += "FROM " + LibAMFC.DBF_AMFC_SOCIO_FileName + " " + "\n";
                            sQueryString += "WHERE" + " " + "\n";
                            sQueryString += sQueryWhere + " " + "\n";
                            sQueryString += "ORDER BY" + " " + new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMERO) + " " + "\n";
                            objOleDbCommand.CommandText = sQueryString;
                            objSerarchMembersResult = Get_Member_DB_Data(objOleDbCommand, false);
                            if (objSerarchMembersResult == null || objSerarchMembersResult.Members == null)
                            {
                                StackFrame objStackFrame = new StackFrame();
                                String sErrorMsg = "Não foi possível obter informação dos Sócios da Base de Dados!" + " -> " + "QUERY: " + sQueryString;
                                Program.HandleError(objStackFrame.GetMethod().Name, sErrorMsg, Program.ErroType.ERROR, true, true);
                                objStackFrame = null;
                                return null;
                            }
                            else if (objSerarchMembersResult.Members.Count > 0)
                                return objSerarchMembersResult;
                        }
                        #endregion  Get members that match the search fields criteria
                        
                        #endregion  Get DB Members that match the search fields text

                        #region     Get DB Members that match the majority search fields words

                        #region     Get All DB Members
                        if (objListDBallMemmers == null || objListDBallMemmers.Members == null || objListDBallMemmers.Members.Count == 0)
                            objListDBallMemmers = Get_List_Members();
                        if (objListDBallMemmers == null || objListDBallMemmers.Members == null || objListDBallMemmers.Members.Count == 0)
                            return null;
                        #endregion  Get All DB Members

                        #region     Filter List Member by the Fields Text Words
                        foreach (AMFCMember objMember in objListDBallMemmers.Members)
                        {
                            if (!String.IsNullOrEmpty(objMemberFieldsToSearch.NOME.Trim()))
                            {
                                if (FieldSearchMatch(objMember.NOME.Trim(), objMemberFieldsToSearch.NOME.Trim(), -1, -1))
                                {
                                    objSerarchMembersResult.Members.Add(objMember);
                                    continue;
                                }
                            }
                            if (!String.IsNullOrEmpty(objMemberFieldsToSearch.MORADA.Trim()))
                            {
                                if (FieldSearchMatch(objMember.MORADA.Trim(), objMemberFieldsToSearch.MORADA.Trim(), -1, -1))
                                {
                                    objSerarchMembersResult.Members.Add(objMember);
                                    continue;
                                }
                                if (FieldSearchMatch(objMember.MORLOTE.Trim(), objMemberFieldsToSearch.MORADA.Trim(), -1, -1))
                                {
                                    objSerarchMembersResult.Members.Add(objMember);
                                    continue;
                                }
                                if (FieldSearchMatch(objMember.CPOSTAL, objMemberFieldsToSearch.MORADA.Trim(), 0.4, -1))
                                {
                                    objSerarchMembersResult.Members.Add(objMember);
                                    continue;
                                }
                            }
                            if (!String.IsNullOrEmpty(objMemberFieldsToSearch.CPOSTAL.Trim()))
                            {
                                if (FieldSearchMatch(objMember.CPOSTAL, objMemberFieldsToSearch.CPOSTAL.Trim(), -1, -1))
                                {
                                    objSerarchMembersResult.Members.Add(objMember);
                                    continue;
                                }
                                if (FieldSearchMatch(objMember.MORADA.Trim(), objMemberFieldsToSearch.CPOSTAL.Trim(), 0.1, -1))
                                {
                                    objSerarchMembersResult.Members.Add(objMember);
                                    continue;
                                }
                                if (FieldSearchMatch(objMember.MORLOTE.Trim(), objMemberFieldsToSearch.CPOSTAL.Trim(), 0.1, -1))
                                {
                                    objSerarchMembersResult.Members.Add(objMember);
                                    continue;
                                }
                            }
                            if (!String.IsNullOrEmpty(objMemberFieldsToSearch.NUMLOTE.Trim()))
                            {
                                if (FieldSearchMatch(objMember.NUMLOTE, objMemberFieldsToSearch.NUMLOTE.Trim(), -1, -1))
                                {
                                    objSerarchMembersResult.Members.Add(objMember);
                                    continue;
                                }
                                if (FieldSearchMatch(objMember.MORADA.Trim(), objMemberFieldsToSearch.NUMLOTE.Trim(), 0.1, -1))
                                {
                                    objSerarchMembersResult.Members.Add(objMember);
                                    continue;
                                }
                                if (FieldSearchMatch(objMember.MORLOTE.Trim(), objMemberFieldsToSearch.NUMLOTE.Trim(), 0.1, -1))
                                {
                                    objSerarchMembersResult.Members.Add(objMember);
                                    continue;
                                }
                            }
                        }
                        #endregion  Filter List Member by the Fields Text Words

                        #endregion  Get DB Members that match the majority search fields words
                    }
                }

                return objSerarchMembersResult;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message + " -> " + "QUERY: " + sQueryString, Program.ErroType.EXCEPTION, true, false);
                return null;
            }
        }

        /// <versions>03-05-2017(v0.0.2.51)</versions>
        private AMFCMembers Get_Member_DB_Data(OleDbCommand objOleDbCommand, Boolean bGetSingleRecord)
        {
            try
            {
                AMFCMembers objListMembers = new AMFCMembers();
                using (OleDbDataReader objOleDbDataReader = objOleDbCommand.ExecuteReader())
                {
                    while (objOleDbDataReader.Read())
                    {
                        AMFCMember objMember = new AMFCMember();
                        if (objOleDbDataReader[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMERO)] == DBNull.Value)
                            continue;
                        objMember.NUMERO = Convert.ToInt64(objOleDbDataReader[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMERO)]);
                        if (objMember.NUMERO < objMember.MinNumber || objMember.NUMERO > objMember.MaxNumber)
                            continue;

                        if (objOleDbDataReader[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NOME)] == DBNull.Value)
                            continue;
                        objMember.NOME = Program.EncodeStringToISO(objOleDbDataReader[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NOME)].ToString());

                        if (objOleDbDataReader[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.MORADA)] != DBNull.Value)
                            objMember.MORADA = Program.EncodeStringToISO(objOleDbDataReader[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.MORADA)].ToString());

                        if (objOleDbDataReader[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.CPOSTAL)] != DBNull.Value)
                            objMember.CPOSTAL = Program.EncodeStringToISO(objOleDbDataReader[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.CPOSTAL)].ToString());

                        if (objOleDbDataReader[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.TELEFONE)] != DBNull.Value)
                            objMember.TELEFONE = objOleDbDataReader[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.TELEFONE)].ToString();

                        if (objOleDbDataReader[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.TELEMOVEL)] != DBNull.Value)
                            objMember.TELEMOVEL = objOleDbDataReader[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.TELEMOVEL)].ToString();

                        if (objOleDbDataReader[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.CC)] != DBNull.Value)
                            objMember.CC = objOleDbDataReader[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.CC)].ToString();

                        if (objOleDbDataReader[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NIF)] != DBNull.Value)
                            objMember.NIF = objOleDbDataReader[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NIF)].ToString();

                        if (objOleDbDataReader[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.EMAIL)] != DBNull.Value)
                            objMember.EMAIL = Program.EncodeStringToISO(objOleDbDataReader[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.EMAIL)].ToString());

                        if (objOleDbDataReader[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.MORLOTE)] != DBNull.Value)
                            objMember.MORLOTE = Program.EncodeStringToISO(objOleDbDataReader[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.MORLOTE)].ToString());

                        if (objOleDbDataReader[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMLOTE)] != DBNull.Value)
                            objMember.NUMLOTE = Program.EncodeStringToISO(objOleDbDataReader[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMLOTE)].ToString());

                        if (objOleDbDataReader[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.AREALOTE)] != DBNull.Value)
                            objMember.AREALOTE = Convert.ToInt32(objOleDbDataReader[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.AREALOTE)]);

                        if (objOleDbDataReader[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.PROFISSAO)] != DBNull.Value)
                            objMember.PROFISSAO = Program.EncodeStringToISO(objOleDbDataReader[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.PROFISSAO)].ToString());

                        if (objOleDbDataReader[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.DATAADMI)] != DBNull.Value)
                            objMember.DATAADMI = Program.ConvertToValidDateTime(objOleDbDataReader[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.DATAADMI)].ToString().Trim()).ToString(Program.Date_Format_String, CultureInfo.CurrentCulture);

                        if (objOleDbDataReader[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.OBSERVACAO)] != DBNull.Value)
                            objMember.OBSERVACAO = Program.EncodeStringToISO(objOleDbDataReader[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.OBSERVACAO)].ToString());

                        if (objOleDbDataReader[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.SECTOR)] != DBNull.Value)
                            objMember.SECTOR = objOleDbDataReader[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.SECTOR)].ToString();

                        if (objOleDbDataReader[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMFOGOS)] != DBNull.Value)
                            objMember.NUMFOGOS = objOleDbDataReader[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMFOGOS)].ToString();

                        if (objOleDbDataReader[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMFILHOS)] != DBNull.Value)
                            objMember.NUMFILHOS = objOleDbDataReader[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMFILHOS)].ToString();

                        if (objOleDbDataReader[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.AGREFAMIL)] != DBNull.Value)
                            objMember.AGREFAMIL = objOleDbDataReader[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.AGREFAMIL)].ToString();

                        if (objOleDbDataReader[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.CASA)] != DBNull.Value)
                            objMember.CASA = Program.ConvertYesOrNoToBoolean(objOleDbDataReader[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.CASA)].ToString());

                        if (objOleDbDataReader[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.LADOMAIOR)] != DBNull.Value)
                            objMember.LADOMAIOR = objOleDbDataReader[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.LADOMAIOR)].ToString();

                        if (objOleDbDataReader[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.GARAGEM)] != DBNull.Value)
                            objMember.GARAGEM = Program.ConvertYesOrNoToBoolean(objOleDbDataReader[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.GARAGEM)].ToString());

                        if (objOleDbDataReader[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.MUROS)] != DBNull.Value)
                            objMember.MUROS = Program.ConvertYesOrNoToBoolean(objOleDbDataReader[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.MUROS)].ToString());

                        if (objOleDbDataReader[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.POCO)] != DBNull.Value)
                            objMember.POCO = Program.ConvertYesOrNoToBoolean(objOleDbDataReader[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.POCO)].ToString());

                        if (objOleDbDataReader[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.FURO)] != DBNull.Value)
                            objMember.FURO = Program.ConvertYesOrNoToBoolean(objOleDbDataReader[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.SANEAMENTO)].ToString());

                        if (objOleDbDataReader[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.SANEAMENTO)] != DBNull.Value)
                            objMember.SANEAMENTO = Program.ConvertYesOrNoToBoolean(objOleDbDataReader[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.FURO)].ToString());

                        if (objOleDbDataReader[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.ELECTRICID)] != DBNull.Value)
                            objMember.ELECTRICID = Program.ConvertYesOrNoToBoolean(objOleDbDataReader[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.ELECTRICID)].ToString());

                        if (objOleDbDataReader[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.PROJECTO)] != DBNull.Value)
                            objMember.PROJECTO = Program.ConvertYesOrNoToBoolean(objOleDbDataReader[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.PROJECTO)].ToString());

                        if (objOleDbDataReader[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.ESCRITURA)] != DBNull.Value)
                            objMember.ESCRITURA = Program.ConvertYesOrNoToBoolean(objOleDbDataReader[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.ESCRITURA)].ToString());

                        if (objOleDbDataReader[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.FINANCAS)] != DBNull.Value)
                            objMember.FINANCAS = Program.ConvertYesOrNoToBoolean(objOleDbDataReader[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.FINANCAS)].ToString());

                        if (objOleDbDataReader[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.RESIDENCIA)] != DBNull.Value)
                            objMember.RESIDENCIA = Program.ConvertYesOrNoToBoolean(objOleDbDataReader[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.RESIDENCIA)].ToString());

                        if (objOleDbDataReader[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.GAVETO)] != DBNull.Value)
                            objMember.GAVETO = Program.ConvertYesOrNoToBoolean(objOleDbDataReader[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.GAVETO)].ToString());

                        if (objOleDbDataReader[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.QUINTINHA)] != DBNull.Value)
                            objMember.QUINTINHA = Program.ConvertYesOrNoToBoolean(objOleDbDataReader[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.QUINTINHA)].ToString());

                        if (objOleDbDataReader[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.MAIS1FOGO)] != DBNull.Value)
                            objMember.MAIS1FOGO = Program.ConvertYesOrNoToBoolean(objOleDbDataReader[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.MAIS1FOGO)].ToString());

                        if (objOleDbDataReader[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.HABCOLECT)] != DBNull.Value)
                            objMember.HABCOLECT = Program.ConvertYesOrNoToBoolean(objOleDbDataReader[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.HABCOLECT)].ToString());

                        if (objOleDbDataReader[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.ARECOMERC)] != DBNull.Value)
                            objMember.ARECOMERC = Program.ConvertYesOrNoToBoolean(objOleDbDataReader[objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.ARECOMERC)].ToString());

                        objListMembers.Members.Add(objMember);

                        if (bGetSingleRecord)
                            return objListMembers;
                    }
                }
                return objListMembers;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return null;
            }
        }

        /// <versions>01-05-2017(v0.0.2.50)</versions>
        public Int32 Get_Member_Max_Number()
        {
            String sQueryString = String.Empty;
            try
            {
                var iMemberMaxNumber = -1;

                String sDBf_Filename = LibAMFC.DBF_AMFC_SOCIO_FileName;

                Set_OLE_DB_ConnectionString();
                if (String.IsNullOrEmpty(LibAMFC.DBF_AMFC_SOCIO_FileName))
                    OLE_DB_Settings();

                using (OleDbConnection objOleDbConnection = new OleDbConnection(OLE_DB_ConnectionString))
                {
                    objOleDbConnection.Open();
                    using (OleDbCommand objOleDbCommand = new OleDbCommand())
                    {
                        objOleDbCommand.Connection = objOleDbConnection;

                        objOleDbCommand.CommandText = "SELECT COUNT(*) FROM " + sDBf_Filename + " " + "\n";
                        if (Convert.ToInt32(objOleDbCommand.ExecuteScalar()) <= 0)
                            return 0;

                        objOleDbCommand.CommandText = "SELECT MAX(NUMERO) as MaxId FROM " + sDBf_Filename + ";";
                        iMemberMaxNumber = Convert.ToInt32(objOleDbCommand.ExecuteScalar());
                    }
                }
                return iMemberMaxNumber;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message + " -> " + "QUERY: " + sQueryString, Program.ErroType.EXCEPTION, true, false);
                return -1;
            }
        }

        /// <versions>03-05-2017(v0.0.2.53)</versions>
        public Boolean Member_Already_Exist(AMFCMember objMember, Library_AMFC_Methods.MemberOperationType eOperation)
        {
            String sQueryString = String.Empty;
            try
            {
                AMFCMembers objListMembers = new AMFCMembers();
                
                Set_OLE_DB_ConnectionString();
                if (String.IsNullOrEmpty(LibAMFC.DBF_AMFC_SOCIO_FileName))
                    OLE_DB_Settings();

                #region     Query
                sQueryString += "SELECT COUNT(*) FROM " + LibAMFC.DBF_AMFC_SOCIO_FileName + " " + "\n";
                sQueryString += "WHERE" + " " + "\n";
                switch (eOperation)
                {
                    case Library_AMFC_Methods.MemberOperationType.ADD:
                        sQueryString += "\t" + objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMERO) + " = " + objMember.NUMERO + " " + "\n";
                        sQueryString += "\t" + "OR" + " " + "\n";
                        sQueryString += "\t" + objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NOME) + " = " + "'" + objMember.NOME.Trim().ToUpper() + "'" + " " + "\n";
                        break;
                    case Library_AMFC_Methods.MemberOperationType.EDIT:
                        sQueryString += "\t" + objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NOME) + " = " + "'" + objMember.NOME.Trim().ToUpper() + "'" + " " + "\n";
                        sQueryString += "\t" + "AND" + " " + "\n";
                        sQueryString += "\t" + objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMERO) + " <> " + objMember.NUMERO + " " + "\n";
                        break;
                }
                sQueryString += "; ";
                #endregion  Query

                using (OleDbConnection objOleDbConnection = new OleDbConnection(OLE_DB_ConnectionString))
                {
                    objOleDbConnection.Open();
                    using (OleDbCommand objOleDbCommand = new OleDbCommand())
                    {
                        objOleDbCommand.Connection = objOleDbConnection;
                        objOleDbCommand.CommandText = sQueryString;
                        if (Convert.ToInt32(objOleDbCommand.ExecuteScalar()) > 0)
                            return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message + " -> " + "QUERY: " + sQueryString, Program.ErroType.EXCEPTION, true, false);
                return true;
            }
        }

        /// <versions>03-05-2017(v0.0.2.53)</versions>
        public Int32 Add_Member(AMFCMember objMember, Library_AMFC_Methods.MemberOperationType eOperation)
        {
            String sQueryString = String.Empty;
            try
            {
                AMFCMembers objListMembers = new AMFCMembers();
                
                Set_OLE_DB_ConnectionString();
                if (String.IsNullOrEmpty(LibAMFC.DBF_AMFC_SOCIO_FileName))
                    OLE_DB_Settings();

                #region     Check if Member already exist
                if (Member_Already_Exist(objMember, eOperation))
                {
                    String sWarning = "Já existe um sócio com este Nº Sócio (" + objMember.NUMERO + ") e/ou Nome (" + objMember.NOME + ")! Por favor, modifique.";
                    MessageBox.Show(sWarning, "Sócio já existente", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return -1;
                }
                #endregion  Check if Member already exist

                #region     Query
                sQueryString += "INSERT INTO " + LibAMFC.DBF_AMFC_SOCIO_FileName + " " + "\n";
                sQueryString += "( " + " " + "\n";
                #region     Columns
                sQueryString += "\t" + objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMERO) + "," + " " + "\n";
                sQueryString += "\t" + objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NOME) + "," + " " + "\n";
                sQueryString += "\t" + objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.MORADA) + "," + " " + "\n";
                sQueryString += "\t" + objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.CPOSTAL) + "," + " " + "\n";
                sQueryString += "\t" + objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.TELEFONE) + "," + " " + "\n";
                sQueryString += "\t" + objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.TELEMOVEL) + "," + " " + "\n";
                sQueryString += "\t" + objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.CC) + "," + " " + "\n";
                sQueryString += "\t" + objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NIF) + "," + " " + "\n";
                sQueryString += "\t" + objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.EMAIL) + "," + " " + "\n";
                sQueryString += "\t" + objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.MORLOTE) + "," + " " + "\n";
                sQueryString += "\t" + objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMLOTE) + "," + " " + "\n";
                if (objMember.AREALOTE > 0)
                    sQueryString += "\t" + objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.AREALOTE) + "," + " " + "\n";
                sQueryString += "\t" + objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.PROFISSAO) + "," + " " + "\n";
                if (!String.IsNullOrEmpty(objMember.DATAADMI))
                    sQueryString += "\t" + objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.DATAADMI) + "," + " " + "\n";
                sQueryString += "\t" + objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.OBSERVACAO) + "," + " " + "\n";
                sQueryString += "\t" + objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.SECTOR) + "," + " " + "\n";
                sQueryString += "\t" + objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.CASA) + "," + " " + "\n";
                sQueryString += "\t" + objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.GARAGEM) + "," + " " + "\n";
                sQueryString += "\t" + objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.MUROS) + "," + " " + "\n";
                sQueryString += "\t" + objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.POCO) + "," + " " + "\n";
                sQueryString += "\t" + objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.FURO) + "," + " " + "\n";
                sQueryString += "\t" + objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.SANEAMENTO) + "," + " " + "\n";
                sQueryString += "\t" + objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.ELECTRICID) + "," + " " + "\n";
                sQueryString += "\t" + objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.PROJECTO) + "," + " " + "\n";
                sQueryString += "\t" + objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.ESCRITURA) + "," + " " + "\n";
                sQueryString += "\t" + objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.FINANCAS) + "," + " " + "\n";
                sQueryString += "\t" + objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.RESIDENCIA) + "," + " " + "\n";
                sQueryString += "\t" + objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.AGREFAMIL) + "," + " " + "\n";
                sQueryString += "\t" + objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMFILHOS) + "," + " " + "\n";
                sQueryString += "\t" + objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.GAVETO) + "," + " " + "\n";
                sQueryString += "\t" + objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.QUINTINHA) + "," + " " + "\n";
                sQueryString += "\t" + objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.LADOMAIOR) + "," + " " + "\n";
                sQueryString += "\t" + objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.MAIS1FOGO) + "," + " " + "\n";
                sQueryString += "\t" + objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.HABCOLECT) + "," + " " + "\n";
                sQueryString += "\t" + objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMFOGOS) + "," + " " + "\n";
                sQueryString += "\t" + objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.ARECOMERC) + " " + " " + "\n";
                #endregion  Columns
                sQueryString += ") " + " " + "\n";
                sQueryString += "VALUES" + " " + "\n";
                sQueryString += "( " + " " + "\n";
                #region     Values
                sQueryString += "\t"        + objMember.NUMERO                     + "," + " " + "\n";
                sQueryString += "\t" + "'"  + objMember.NOME.Trim().ToUpper()      + "'"   + "," + " " + "\n";
                sQueryString += "\t" + "'"  + objMember.MORADA.Trim().ToUpper()    + "'"   + "," + " " + "\n";
                sQueryString += "\t" + "'"  + objMember.CPOSTAL.Trim().ToUpper()   + "'"   + "," + " " + "\n";
                sQueryString += "\t" + "'"  + objMember.TELEFONE.Trim()            + "'"   + "," + " " + "\n";
                sQueryString += "\t" + "'"  + objMember.TELEMOVEL.Trim()           + "'"   + "," + " " + "\n";
                sQueryString += "\t" + "'" + objMember.CC.Trim() + "'" + "," + " " + "\n";
                sQueryString += "\t" + "'" + objMember.NIF.Trim() + "'" + "," + " " + "\n";
                sQueryString += "\t" + "'"  + objMember.EMAIL.Trim()               + "'"   + "," + " " + "\n";
                sQueryString += "\t" + "'"  + objMember.MORLOTE.Trim().ToUpper()   + "'"   + "," + " " + "\n";
                sQueryString += "\t" + "'"  + objMember.NUMLOTE.Trim().ToUpper()   + "'"   + "," + " " + "\n";
                if (objMember.AREALOTE > 0)
                    sQueryString += "\t" + objMember.AREALOTE + "," + " " + "\n";
                sQueryString += "\t" + "'" + objMember.PROFISSAO.Trim().ToUpper() + "'" + "," + " " + "\n";
                if (!String.IsNullOrEmpty(objMember.DATAADMI))
                    sQueryString += "\t" + "'" + objMember.DATAADMI.Trim() + "'" + "," + " " + "\n";
                sQueryString += "\t" + "'" + objMember.OBSERVACAO           + "'"   + "," + " " + "\n";
                sQueryString += "\t" + "'" + objMember.SECTOR.ToUpper()     + "'"   + "," + " " + "\n";
                sQueryString += "\t" + "'" + Program.ConvertBooleanToYesOrNo(objMember.CASA).ToUpper()        + "'" + "," + " " + "\n";
                sQueryString += "\t" + "'" + Program.ConvertBooleanToYesOrNo(objMember.GARAGEM).ToUpper()     + "'" + "," + " " + "\n";
                sQueryString += "\t" + "'" + Program.ConvertBooleanToYesOrNo(objMember.MUROS).ToUpper()       + "'" + "," + " " + "\n";
                sQueryString += "\t" + "'" + Program.ConvertBooleanToYesOrNo(objMember.POCO).ToUpper()        + "'" + "," + " " + "\n";
                sQueryString += "\t" + "'" + Program.ConvertBooleanToYesOrNo(objMember.FURO).ToUpper()        + "'" + "," + " " + "\n";
                sQueryString += "\t" + "'" + Program.ConvertBooleanToYesOrNo(objMember.SANEAMENTO).ToUpper()  + "'" + "," + " " + "\n";
                sQueryString += "\t" + "'" + Program.ConvertBooleanToYesOrNo(objMember.ELECTRICID).ToUpper()  + "'" + "," + " " + "\n";
                sQueryString += "\t" + "'" + Program.ConvertBooleanToYesOrNo(objMember.PROJECTO).ToUpper()    + "'" + "," + " " + "\n";
                sQueryString += "\t" + "'" + Program.ConvertBooleanToYesOrNo(objMember.ESCRITURA).ToUpper()   + "'" + "," + " " + "\n";
                sQueryString += "\t" + "'" + Program.ConvertBooleanToYesOrNo(objMember.FINANCAS).ToUpper()    + "'" + "," + " " + "\n";
                sQueryString += "\t" + "'" + Program.ConvertBooleanToYesOrNo(objMember.RESIDENCIA).ToUpper()  + "'" + "," + " " + "\n";
                sQueryString += "\t" + "'" + objMember.AGREFAMIL.Trim() + "'" + "," + " " + "\n"; //string
                sQueryString += "\t" + "'" + objMember.NUMFILHOS.Trim() + "'" + "," + " " + "\n"; //string
                sQueryString += "\t" + "'" + Program.ConvertBooleanToYesOrNo(objMember.GAVETO).ToUpper()      + "'" + "," + " " + "\n";
                sQueryString += "\t" + "'" + Program.ConvertBooleanToYesOrNo(objMember.QUINTINHA).ToUpper()   + "'" + "," + " " + "\n";
                sQueryString += "\t" + "'" + objMember.LADOMAIOR.Trim() + "'" + "," + " " + "\n"; //string
                sQueryString += "\t" + "'" + Program.ConvertBooleanToYesOrNo(objMember.MAIS1FOGO).ToUpper()   + "'" + "," + " " + "\n";
                sQueryString += "\t" + "'" + Program.ConvertBooleanToYesOrNo(objMember.HABCOLECT).ToUpper()   + "'" + "," + " " + "\n";
                sQueryString += "\t" + "'" + objMember.NUMFOGOS.Trim() + "'" + "," + " " + "\n"; //string
                sQueryString += "\t" + "'" + Program.ConvertBooleanToYesOrNo(objMember.ARECOMERC).ToUpper()   + "'" + " " + " " + "\n";
                #endregion  Values
                sQueryString += ") " + " " + "\n";
                #endregion  Query

                using (OleDbConnection objOleDbConnection = new OleDbConnection(OLE_DB_ConnectionString))
                {
                    objOleDbConnection.Open();
                    using (OleDbCommand objOleDbCommand = new OleDbCommand())
                    {
                        objOleDbCommand.Connection = objOleDbConnection;
                        objOleDbCommand.CommandText = sQueryString;
                        if (objOleDbCommand.ExecuteNonQuery() != 1)
                            return -1;
                    }
                }

                return 1;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message + " -> " + "QUERY: " + sQueryString, Program.ErroType.EXCEPTION, true, false);
                return -1;
            }
        }

        /// <versions>03-05-2017(v0.0.2.53)</versions>
        public Int32 Edit_Member(AMFCMember objMember, Library_AMFC_Methods.MemberOperationType eOperation)
        {
            String sQueryString = String.Empty;
            try
            {
                AMFCMembers objListMembers = new AMFCMembers();
                
                Set_OLE_DB_ConnectionString();
                if (String.IsNullOrEmpty(LibAMFC.DBF_AMFC_SOCIO_FileName))
                    OLE_DB_Settings();
                
                #region     Check if Member already exist
                if (Member_Already_Exist(objMember, eOperation))
                {
                    String sWarning = "Já existe um sócio com este Nº Sócio (" + objMember.NUMERO + ") e/ou Nome (" + objMember.NOME + ")! Por favor, modifique.";
                    MessageBox.Show(sWarning, "Sócio já existente", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return -1;
                }
                #endregion  Check if Member already exist

                #region     Query
                sQueryString += "UPDATE " + LibAMFC.DBF_AMFC_SOCIO_FileName + " " + "\n";
                sQueryString += "SET" + " " + "\n";
                #region     Columns Values
                sQueryString += "\t" + objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NOME)            + " = " + "'" + objMember.NOME.Trim().ToUpper()         + "'" + "," + " " + "\n";
                sQueryString += "\t" + objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.MORADA)          + " = " + "'" + objMember.MORADA.Trim().ToUpper()       + "'" + "," + " " + "\n";
                sQueryString += "\t" + objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.CPOSTAL)         + " = " + "'" + objMember.CPOSTAL.Trim().ToUpper()      + "'" + "," + " " + "\n";
                sQueryString += "\t" + objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.TELEFONE)        + " = " + "'" + objMember.TELEFONE.Trim().ToUpper()     + "'" + "," + " " + "\n";
                sQueryString += "\t" + objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.TELEMOVEL)       + " = " + "'" + objMember.TELEMOVEL.Trim().ToUpper()    + "'" + "," + " " + "\n";
                sQueryString += "\t" + objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.CC) + " = " + "'" + objMember.CC.Trim().ToUpper() + "'" + "," + " " + "\n";
                sQueryString += "\t" + objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NIF) + " = " + "'" + objMember.NIF.Trim().ToUpper() + "'" + "," + " " + "\n";
                sQueryString += "\t" + objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.EMAIL)           + " = " + "'" + objMember.EMAIL.Trim().ToUpper()        + "'" + "," + " " + "\n";
                sQueryString += "\t" + objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.MORLOTE)         + " = " + "'" + objMember.MORLOTE.Trim().ToUpper()      + "'" + "," + " " + "\n";
                sQueryString += "\t" + objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMLOTE)         + " = " + "'" + objMember.NUMLOTE.Trim().ToUpper()      + "'" + "," + " " + "\n";
                if (objMember.AREALOTE > 0)
                    sQueryString += "\t" + objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.AREALOTE) + " = " + objMember.AREALOTE + "," + " " + "\n";
                sQueryString += "\t" + objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.PROFISSAO)       + " = " + "'" + objMember.PROFISSAO.Trim().ToUpper()    + "'" + "," + " " + "\n";
                if (!String.IsNullOrEmpty(objMember.DATAADMI))
                    sQueryString += "\t" + objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.DATAADMI)    + " = " + "'" + objMember.DATAADMI.Trim().ToUpper()     + "'" + "," + " " + "\n";
                sQueryString += "\t" + objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.OBSERVACAO)      + " = " + "'" + objMember.OBSERVACAO.Trim()             + "'" + "," + " " + "\n";
                sQueryString += "\t" + objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.SECTOR)          + " = " + "'" + objMember.SECTOR.Trim().ToUpper() + "'" + "," + " " + "\n";
                sQueryString += "\t" + objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.CASA)            + " = " + "'" + Program.ConvertBooleanToYesOrNo(objMember.CASA).ToUpper()         + "'" + "," + " " + "\n";
                sQueryString += "\t" + objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.GARAGEM)         + " = " + "'" + Program.ConvertBooleanToYesOrNo(objMember.GARAGEM).ToUpper()      + "'" + "," + " " + "\n";
                sQueryString += "\t" + objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.MUROS)           + " = " + "'" + Program.ConvertBooleanToYesOrNo(objMember.MUROS).ToUpper()        + "'" + "," + " " + "\n";
                sQueryString += "\t" + objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.POCO)            + " = " + "'" + Program.ConvertBooleanToYesOrNo(objMember.POCO).ToUpper()         + "'" + "," + " " + "\n";
                sQueryString += "\t" + objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.FURO)            + " = " + "'" + Program.ConvertBooleanToYesOrNo(objMember.FURO).ToUpper()         + "'" + "," + " " + "\n";
                sQueryString += "\t" + objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.SANEAMENTO)      + " = " + "'" + Program.ConvertBooleanToYesOrNo(objMember.SANEAMENTO).ToUpper()   + "'" + "," + " " + "\n";
                sQueryString += "\t" + objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.ELECTRICID)      + " = " + "'" + Program.ConvertBooleanToYesOrNo(objMember.ELECTRICID).ToUpper()   + "'" + "," + " " + "\n";
                sQueryString += "\t" + objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.PROJECTO)        + " = " + "'" + Program.ConvertBooleanToYesOrNo(objMember.PROJECTO).ToUpper()     + "'" + "," + " " + "\n";
                sQueryString += "\t" + objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.ESCRITURA)       + " = " + "'" + Program.ConvertBooleanToYesOrNo(objMember.ESCRITURA).ToUpper()    + "'" + "," + " " + "\n";
                sQueryString += "\t" + objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.FINANCAS)        + " = " + "'" + Program.ConvertBooleanToYesOrNo(objMember.FINANCAS).ToUpper()     + "'" + "," + " " + "\n";
                sQueryString += "\t" + objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.RESIDENCIA)      + " = " + "'" + Program.ConvertBooleanToYesOrNo(objMember.RESIDENCIA).ToUpper()   + "'" + "," + " " + "\n";
                sQueryString += "\t" + objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.AGREFAMIL)       + " = " + "'" + objMember.AGREFAMIL.Trim() + "'" + "," + " " + "\n"; //STRING
                sQueryString += "\t" + objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMFILHOS)       + " = " + "'" + objMember.NUMFILHOS.Trim() + "'" + "," + " " + "\n"; //STRING
                sQueryString += "\t" + objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.GAVETO)          + " = " + "'" + Program.ConvertBooleanToYesOrNo(objMember.GAVETO).ToUpper()       + "'" + "," + " " + "\n";
                sQueryString += "\t" + objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.QUINTINHA)       + " = " + "'" + Program.ConvertBooleanToYesOrNo(objMember.QUINTINHA).ToUpper()    + "'" + "," + " " + "\n";
                sQueryString += "\t" + objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.LADOMAIOR)       + " = " + "'" + objMember.LADOMAIOR.Trim() + "'" + "," + " " + "\n"; //STRING
                sQueryString += "\t" + objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.MAIS1FOGO)       + " = " + "'" + Program.ConvertBooleanToYesOrNo(objMember.MAIS1FOGO).ToUpper()    + "'" + "," + " " + "\n";
                sQueryString += "\t" + objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.HABCOLECT)       + " = " + "'" + Program.ConvertBooleanToYesOrNo(objMember.HABCOLECT).ToUpper()    + "'" + "," + " " + "\n";
                sQueryString += "\t" + objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMFOGOS)        + " = " + "'" + objMember.NUMFOGOS.Trim() + "'" + "," + " " + "\n"; //STRING
                sQueryString += "\t" + objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.ARECOMERC)       + " = " + "'" + Program.ConvertBooleanToYesOrNo(objMember.ARECOMERC).ToUpper()    + "'" + " " + " " + "\n";
                #endregion  Columns Values
                sQueryString += "WHERE" + " " + "\n";
                sQueryString += "\t" + objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMERO) + " = " + objMember.NUMERO + " " + "\n";
                #endregion  Query

                using (OleDbConnection objOleDbConnection = new OleDbConnection(OLE_DB_ConnectionString))
                {
                    objOleDbConnection.Open();
                    using (OleDbCommand objOleDbCommand = new OleDbCommand())
                    {
                        objOleDbCommand.Connection = objOleDbConnection;
                        objOleDbCommand.CommandText = sQueryString;
                        if (objOleDbCommand.ExecuteNonQuery() != 1)
                            return -1;
                    }
                }
                return 1;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message + " -> " + "QUERY: " + sQueryString, Program.ErroType.EXCEPTION, true, false);
                return -1;
            }
        }

        /// <versions>01-05-2017(v0.0.2.50)</versions>
        public Boolean Del_Member(AMFCMember objMember)
        {
            String sQueryString = String.Empty;
            try
            {
                AMFCMembers objListMembers = new AMFCMembers();

                Set_OLE_DB_ConnectionString();
                if (String.IsNullOrEmpty(LibAMFC.DBF_AMFC_SOCIO_FileName))
                    OLE_DB_Settings();

                #region     Query
                sQueryString += "DELETE FROM " + LibAMFC.DBF_AMFC_SOCIO_FileName + " " + "\n";
                sQueryString += "WHERE" + " " + objMember.Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMERO) + " = " + objMember.NUMERO + " " + "\n";
                #endregion  Query

                using (OleDbConnection objOleDbConnection = new OleDbConnection(OLE_DB_ConnectionString))
                {
                    objOleDbConnection.Open();
                    using (OleDbCommand objOleDbCommand = new OleDbCommand())
                    {
                        objOleDbCommand.Connection = objOleDbConnection;
                        objOleDbCommand.CommandText = sQueryString;
                        if (objOleDbCommand.ExecuteNonQuery() != 1)
                            return false;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message + " -> " + "QUERY: " + sQueryString, Program.ErroType.EXCEPTION, true, false);
                return false;
            }
        }

        /// <versions>03-05-2017(v0.0.2.51)</versions>
        private Boolean FieldSearchMatch(String sDbText, String sTextToSearch, Double dMinPercToFound, Double dMaxPercToFound)
        {
            try
            { 
                if (String.IsNullOrEmpty(sTextToSearch))
                    return false;

                List<String> objListDbTextWords = LibAMFC.SplitStringWords(sDbText);
                if (objListDbTextWords == null || objListDbTextWords.Count == 0)
                    return false;

                List<String> objListDbTextWordsCleaned = LibAMFC.RemoveWordsNotToSearch(objListDbTextWords);
                if (objListDbTextWordsCleaned == null || objListDbTextWordsCleaned.Count == 0)
                    return false;
                
                List<String> objListSearchWords = LibAMFC.SplitStringWords(sTextToSearch);
                if (objListSearchWords == null || objListSearchWords.Count == 0)
                    return false;
                
                List<String> objListSearchWordsCleaned = LibAMFC.RemoveWordsNotToSearch(objListSearchWords);
                if (objListSearchWordsCleaned == null || objListSearchWordsCleaned.Count == 0)
                    return false;

                Boolean bTextMatch = LibAMFC.ContainsMajorityWords(objListDbTextWordsCleaned, objListSearchWordsCleaned, dMinPercToFound, dMaxPercToFound);
                return bTextMatch;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return false;
            }
        }

        #endregion  Admin Members

        #region     Admin Payments

        /// <versions>23-03-2018(GesAMFC-v1.0.0.3)</versions>
        public AMFCCashPayments Get_All_Cash_Payments(DateTime dtDate)
        {
            try
            {
                AMFCCashPayments objListPayments = new AMFCCashPayments();
                Set_OLE_DB_ConnectionString();

                AMFCMember objMember = new AMFCMember();
                using (OleDbConnection objOleDbConnection = new OleDbConnection(OLE_DB_ConnectionString))
                {
                    objOleDbConnection.Open();
                    using (OleDbCommand objOleDbCommand = new OleDbCommand())
                    {
                        objOleDbCommand.Connection = objOleDbConnection;
                        objOleDbCommand.CommandText = Set_Cash_Payments_Query(-1, -1, dtDate, false);

                        objListPayments = Get_Cash_Payments_DB_Data(objOleDbCommand, false);
                        if (objListPayments == null || objListPayments.Payments == null)
                        {
                            StackFrame objStackFrame = new StackFrame();
                            String sErrorMsg = "Não foi possível obter a Lista da Caixa de Pagamentos da Base de Dados!" + " -> " + "QUERY: " + objOleDbCommand.CommandText;
                            Program.HandleError(objStackFrame.GetMethod().Name, sErrorMsg, Program.ErroType.ERROR, true, true);
                            objStackFrame = null;
                            return null;
                        }
                    }
                }

                return objListPayments;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return null;
            }
        }

        /// <versions>23-03-2018(GesAMFC-v1.0.0.3)</versions>
        /// <remarks>TEste primo MLeitao sócio nº 189</remarks>
        public AMFCCashPayments Get_Cash_Payments_By_MemberNumber(Int64 lMemberNumber, DateTime dtDate)
        {
            try
            {
                AMFCCashPayments objListPayments = new AMFCCashPayments();
                Set_OLE_DB_ConnectionString();

                AMFCMember objMember = new AMFCMember();
                using (OleDbConnection objOleDbConnection = new OleDbConnection(OLE_DB_ConnectionString))
                {
                    objOleDbConnection.Open();
                    using (OleDbCommand objOleDbCommand = new OleDbCommand())
                    {
                        objOleDbCommand.Connection = objOleDbConnection;
                        objOleDbCommand.CommandText = Set_Cash_Payments_Query(lMemberNumber, - 1, dtDate, false);

                        objListPayments = Get_Cash_Payments_DB_Data(objOleDbCommand, false);
                        if (objListPayments == null || objListPayments.Payments == null)
                        {
                            StackFrame objStackFrame = new StackFrame();
                            String sErrorMsg = "Não foi possível obter os Pagamentos relativamente ao Sócio com o Nº: " + lMemberNumber + " -> " + "QUERY: " + objOleDbCommand.CommandText;
                            Program.HandleError(objStackFrame.GetMethod().Name, sErrorMsg, Program.ErroType.ERROR, true, true);
                            objStackFrame = null;
                            return null;
                        }
                    }
                }

                return objListPayments;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return null;
            }
        }

        /// <versions>23-03-2018(GesAMFC-v1.0.0.3)</versions>
        public AMFCCashPayment Get_Cash_Payment_By_Id(Int64 lPaymentId)
        {
            try
            {
                AMFCCashPayment objPayment = new AMFCCashPayment();
                Set_OLE_DB_ConnectionString();

                using (OleDbConnection objOleDbConnection = new OleDbConnection(OLE_DB_ConnectionString))
                {
                    objOleDbConnection.Open();
                    using (OleDbCommand objOleDbCommand = new OleDbCommand())
                    {
                        objOleDbCommand.Connection = objOleDbConnection;
                        objOleDbCommand.CommandText = Set_Cash_Payments_Query(-1, lPaymentId, new DateTime(), true);

                        AMFCCashPayments objListPayments = Get_Cash_Payments_DB_Data(objOleDbCommand, true);
                        if (objListPayments == null || objListPayments.Payments == null || objListPayments.Payments.Count > 1)
                        {
                            StackFrame objStackFrame = new StackFrame();
                            String sErrorMsg = "Não foi possível obter a informação do pagamentos o Nº: " + lPaymentId + " -> " + "QUERY: " + objOleDbCommand.CommandText;
                            Program.HandleError(objStackFrame.GetMethod().Name, sErrorMsg, Program.ErroType.ERROR, true, true);
                            objStackFrame = null;
                            return null;
                        }
                        if (objListPayments.Payments.Count == 1)
                            objPayment = objListPayments.Payments[0];
                    }
                }

                return objPayment;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return null;
            }
        }

        /// <versions>23-03-2018(GesAMFC-v1.0.0.3)</versions>
        public String Set_Cash_Payments_Query(Int64 lMemberNumber, Int64 lPaymentId, DateTime dtDate, Boolean bGetSingleRecord)
        {
            try
            {
                String sQueryString = String.Empty;

                String sDBF_Members_FileName = LibAMFC.DBF_AMFC_SOCIO_FileName;

                String sDBF_FileName = LibAMFC.DBF_AMFC_CAIXA_FileName;

                sQueryString += "SELECT * FROM" + " " + "\n";
                sQueryString += "( " + "\n";

                sQueryString += "SELECT" + " " + "\n";
                sQueryString += "DISTINCT" + " " + "\n";
                sQueryString += sDBF_FileName + "." + Payments_DB_Col_ID                + " AS " + Payments_Col_ID                      + "," + " " + "\n";
                sQueryString += sDBF_FileName + "." + Payments_DB_Col_LISTARECNU        + " AS " + Payments_Col_LISTARECNU              + "," + " " + "\n";
                sQueryString += sDBF_FileName + "." + Payments_DB_Col_SOCIO             + " AS " + Payments_Col_SOCIO                   + "," + " " + "\n";
                sQueryString += sDBF_Members_FileName + "." + Payments_DB_Col_NOME      + " AS " + Payments_Col_NOME                    + "," + " " + "\n";
                sQueryString += sDBF_FileName + "." + Payments_DB_Col_NOME              + " AS " + Payments_Col_NOME_PAG                + "," + " " + "\n";
                sQueryString += sDBF_FileName + "." + Payments_DB_Col_DESIGNACAO        + " AS " + Payments_Col_DESIGNACAO              + "," + " " + "\n";
                sQueryString += sDBF_FileName + "." + Payments_DB_Col_NOTAS             + " AS " + Payments_Col_NOTAS                   + "," + " " + "\n";
                sQueryString += sDBF_FileName + "." + Payments_DB_Col_VALOR             + " AS " + Payments_Col_VALOR                   + "," + " " + "\n";
                sQueryString += sDBF_FileName + "." + Payments_DB_Col_DATA              + " AS " + Payments_Col_DATA                    + "," + " " + "\n";
                sQueryString += "YEAR"  + "(" + sDBF_FileName + "." + Payments_DB_Col_DATA + ")" + " AS " + Payments_Col_DATAYear       + "," + " " + "\n";
                sQueryString += "MONTH" + "(" + sDBF_FileName + "." + Payments_DB_Col_DATA + ")" + " AS " + Payments_Col_DATAMonthInt   + "," + " " + "\n";
                sQueryString += sDBF_FileName + "." + Payments_DB_Col_ALTERADO          + " AS " + Payments_Col_ALTERADO                + "," + " " + "\n";

                sQueryString += sDBF_FileName + "." + Payments_DB_Col_JOIA              + " AS " + Payments_Col_JOIA                    + "," + " " + "\n";
                sQueryString += sDBF_FileName + "." + Payments_DB_Col_JOIADESC          + " AS " + Payments_Col_JOIADESC                + "," + " " + "\n";
                sQueryString += sDBF_FileName + "." + Payments_DB_Col_JOIAVAL           + " AS " + Payments_Col_JOIAVAL                 + "," + " " + "\n";
                sQueryString += sDBF_FileName + "." + Payments_DB_Col_ASSOCJOIA         + " AS " + Payments_Col_ASSOCJOIA               + "," + " " + "\n";
                sQueryString += sDBF_FileName + "." + Payments_DB_Col_DASSOCJOIA        + " AS " + Payments_Col_DASSOCJOIA              + "," + " " + "\n";
                
                sQueryString += sDBF_FileName + "." + Payments_DB_Col_QUOTAS            + " AS " + Payments_Col_QUOTAS                  + "," + " " + "\n";
                sQueryString += sDBF_FileName + "." + Payments_DB_Col_QUOTASDESC        + " AS " + Payments_Col_QUOTASDESC              + "," + " " + "\n";
                sQueryString += sDBF_FileName + "." + Payments_DB_Col_QUOTASVAL         + " AS " + Payments_Col_QUOTASVAL               + "," + " " + "\n";
                sQueryString += sDBF_FileName + "." + Payments_DB_Col_ASSOCQUOTA        + " AS " + Payments_Col_ASSOCQUOTA              + "," + " " + "\n";
                sQueryString += sDBF_FileName + "." + Payments_DB_Col_DASSOCQUOT        + " AS " + Payments_Col_DASSOCQUOT              + "," + " " + "\n";
                
                sQueryString += sDBF_FileName + "." + Payments_DB_Col_INFRAEST          + " AS " + Payments_Col_INFRAEST                + "," + " " + "\n";
                sQueryString += sDBF_FileName + "." + Payments_DB_Col_INFRADESC         + " AS " + Payments_Col_INFRADESC               + "," + " " + "\n";
                sQueryString += sDBF_FileName + "." + Payments_DB_Col_INFRAVAL          + " AS " + Payments_Col_INFRAVAL                + "," + " " + "\n";
                sQueryString += sDBF_FileName + "." + Payments_DB_Col_ASSOCNFRA         + " AS " + Payments_Col_ASSOCNFRA               + "," + " " + "\n";
                sQueryString += sDBF_FileName + "." + Payments_DB_Col_DASSOCINFR        + " AS " + Payments_Col_DASSOCINFR              + "," + " " + "\n";
                
                sQueryString += sDBF_FileName + "." + Payments_DB_Col_CEDENCIAS         + " AS " + Payments_Col_CEDENCIAS               + "," + " " + "\n";
                sQueryString += sDBF_FileName + "." + Payments_DB_Col_CEDENCDESC        + " AS " + Payments_Col_CEDENCDESC              + "," + " " + "\n";
                sQueryString += sDBF_FileName + "." + Payments_DB_Col_CEDENCVAL         + " AS " + Payments_Col_CEDENCVAL               + "," + " " + "\n";
                sQueryString += sDBF_FileName + "." + Payments_DB_Col_ASSOCCEDEN        + " AS " + Payments_Col_ASSOCCEDEN              + "," + " " + "\n";
                sQueryString += sDBF_FileName + "." + Payments_DB_Col_DASSOCCEDE        + " AS " + Payments_Col_DASSOCCEDE              + "," + " " + "\n";
                
                sQueryString += sDBF_FileName + "." + Payments_DB_Col_ESGOT             + " AS " + Payments_Col_ESGOT                   + "," + " " + "\n";
                sQueryString += sDBF_FileName + "." + Payments_DB_Col_ESGOTDESC         + " AS " + Payments_Col_ESGOTDESC               + "," + " " + "\n";
                sQueryString += sDBF_FileName + "." + Payments_DB_Col_ESGOTVAL          + " AS " + Payments_Col_ESGOTVAL                + "," + " " + "\n";
                sQueryString += sDBF_FileName + "." + Payments_DB_Col_ASSOCESGOT        + " AS " + Payments_Col_ASSOCESGOT              + "," + " " + "\n";
                sQueryString += sDBF_FileName + "." + Payments_DB_Col_DASSOCESGO        + " AS " + Payments_Col_DASSOCESGO              + "," + " " + "\n";

                sQueryString += sDBF_FileName + "." + Payments_DB_Col_RECONV            + " AS " + Payments_Col_RECONV                  + "," + " " + "\n";
                sQueryString += sDBF_FileName + "." + Payments_DB_Col_RECONDESC         + " AS " + Payments_Col_RECONDESC               + "," + " " + "\n";
                sQueryString += sDBF_FileName + "." + Payments_DB_Col_RECONVAL          + " AS " + Payments_Col_RECONVAL                + "," + " " + "\n";
                sQueryString += sDBF_FileName + "." + Payments_DB_Col_ASSOCRECON        + " AS " + Payments_Col_ASSOCRECON              + "," + " " + "\n";
                sQueryString += sDBF_FileName + "." + Payments_DB_Col_DASSOCRECO        + " AS " + Payments_Col_DASSOCRECO              + "," + " " + "\n";
                
                sQueryString += sDBF_FileName + "." + Payments_DB_Col_OUTRO             + " AS " + Payments_Col_OUTRO                   + "," + " " + "\n";
                sQueryString += sDBF_FileName + "." + Payments_DB_Col_OUTROSDESC        + " AS " + Payments_Col_OUTROSDESC              + "," + " " + "\n";
                sQueryString += sDBF_FileName + "." + Payments_DB_Col_OUTROSVAL         + " AS " + Payments_Col_OUTROSVAL               + "," + " " + "\n";
                sQueryString += sDBF_FileName + "." + Payments_DB_Col_ASSOCOUTRO        + " AS " + Payments_Col_ASSOCOUTRO              + "," + " " + "\n";
                sQueryString += sDBF_FileName + "." + Payments_DB_Col_DASSOCOUTR        + " AS " + Payments_Col_DASSOCOUTR              + "," + " " + "\n";

                sQueryString += sDBF_FileName + "." + Payments_DB_Col_ESTADO            + " AS " + Payments_Col_ESTADO                  + " " + " " + "\n";

                
                sQueryString += "FROM       " + " " + "\n";
                sQueryString += "\t" + sDBF_FileName + " " + "\n";
                sQueryString += "INNER" + " " + "JOIN" + " " + "\n";
                sQueryString += "\t" + sDBF_Members_FileName + " " + "\n";
                sQueryString += "ON " + " " + sDBF_Members_FileName + "." + new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMERO) + " = " + sDBF_FileName + "." + Payments_DB_Col_SOCIO + " " + "\n";

                if (bGetSingleRecord && lPaymentId > 0)
                {
                    sQueryString += "WHERE" + " " + "\n";
                    sQueryString += sDBF_FileName + "." + Payments_DB_Col_ID + " = " + lPaymentId + "\n";
                }
                else if (lMemberNumber >= new AMFCMember().MinNumber && lMemberNumber <= new AMFCMember().MaxNumber)
                {
                    sQueryString += "WHERE" + " " + "\n";
                    sQueryString += sDBF_Members_FileName + "." + new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMERO) + " = " + lMemberNumber + "\n";

                    if (Program.IsValidDateTime(dtDate))
                    {
                        sQueryString += "AND" + " " + "\n";
                        sQueryString += sDBF_FileName + "." + Payments_DB_Col_DATA + " LIKE " + "'" + dtDate.ToString(Program.DBF_Compare_Date_Format_String) + "'" + " " + "\n";
                    }
                }

                //else if (Program.IsValidYear(iYear))
                //{
                //    sQueryString += "WHERE" + " " + "\n";
                //    sQueryString += "ANO" + " = " + iYear + "\n";
                //    if (Program.IsValidMonth(iMonth))
                //    {
                //        sQueryString += "AND" + " " + "\n";
                //        sQueryString += "MES" + " = " + iMonth + "\n";
                //    }
                //}

                else if (Program.IsValidDateTime(dtDate))
                {
                    sQueryString += "WHERE" + " " + "\n";
                    sQueryString += sDBF_FileName + "." + Payments_DB_Col_DATA + " LIKE " + "'" + dtDate.ToString(Program.DBF_Compare_Date_Format_String) + "'" + " " + "\n";
                }

                sQueryString += ")" + " AS " + "TAB_Payments" + " " + "\n";

                sQueryString += "ORDER BY" + " " + "\n";
                sQueryString += "TAB_Payments" + "." + Payments_Col_SOCIO   + " " + "ASC"   + ", " + "\n";
                sQueryString += "TAB_Payments" + "." + Payments_Col_DATA    + " " + "ASC"   + " " + "\n";
                sQueryString += ";";

                return sQueryString;
            }
            catch
            {
                return null;
            }
        }

        /// <versions>22-10-2017(v0.0.4.22)</versions>
        private AMFCCashPayments Get_Cash_Payments_DB_Data(OleDbCommand objOleDbCommand, Boolean bGetSingleRecord)
        {
            try
            {
                AMFCCashPayments objListPayments = new AMFCCashPayments();

                List<Int64> ListDuplicatedToRemove = new List<Int64>();

                Int64 lIdx = -1;

                using (OleDbDataReader objOleDbDataReader = objOleDbCommand.ExecuteReader())
                {
                    while (objOleDbDataReader.Read())
                    {
                        AMFCCashPayment objPayment = new AMFCCashPayment();

                        lIdx++;
                        objPayment.Idx = lIdx;

                        if (objOleDbDataReader[Payments_Col_ID] != DBNull.Value)
                            objPayment.ID = Convert.ToInt64(objOleDbDataReader[Payments_Col_ID]);

                        if (objOleDbDataReader[Payments_Col_LISTARECNU] != DBNull.Value)
                            objPayment.LISTARECNU = objOleDbDataReader[Payments_Col_LISTARECNU].ToString();

                        if (objOleDbDataReader[Payments_Col_SOCIO] != DBNull.Value)
                            objPayment.SOCIO = Convert.ToInt64(objOleDbDataReader[Payments_Col_SOCIO]);

                        if (objOleDbDataReader[Payments_Col_NOME] != DBNull.Value)
                            objPayment.NOME = Program.EncodeStringToISO(objOleDbDataReader[Payments_Col_NOME].ToString());

                        if (objOleDbDataReader[Payments_Col_NOME_PAG] != DBNull.Value)
                            objPayment.NOME_PAG = Program.EncodeStringToISO(objOleDbDataReader[Payments_Col_NOME_PAG].ToString());

                        if (objOleDbDataReader[Payments_Col_DESIGNACAO] != DBNull.Value)
                            objPayment.DESIGNACAO = Program.EncodeStringToISO(objOleDbDataReader[Payments_Col_DESIGNACAO].ToString());

                        if (objOleDbDataReader[Payments_Col_NOTAS] != DBNull.Value)
                            objPayment.NOTAS = Program.EncodeStringToISO(objOleDbDataReader[Payments_Col_NOTAS].ToString());

                        if (objOleDbDataReader[Payments_Col_ESTADO] != DBNull.Value)
                            objPayment.Payment_State_DB_Value = objOleDbDataReader[Payments_Col_ESTADO].ToString().Trim();

                        #region     VALOR TOTAL
                        if (objOleDbDataReader[Payments_Col_VALOR] != DBNull.Value)
                            objPayment.VALOR = Program.SetPayCurrencyEuroDoubleValue(objOleDbDataReader[Payments_Col_VALOR].ToString().Trim());                       
                        #endregion  VALOR TOTAL

                        #region     PAGAMENTO DATA
                        if (objOleDbDataReader[Payments_Col_DATA] != DBNull.Value)
                            objPayment.DATA = Program.SetDateTimeValue(Program.ConvertToValidDateTime(objOleDbDataReader[Payments_Col_DATA].ToString().Trim()), -1, -1);
                        else
                        {
                            objPayment.DATA = new DateTime();
                            objPayment.DATAYearInt = 0;
                            objPayment.DATAMonthInt = 0;
                        }
                        #endregion  PAGAMENTO DATA

                        #region     DATA ALTERADO
                        if (objOleDbDataReader[Payments_Col_ALTERADO] != DBNull.Value)
                            objPayment.ALTERADO = Program.SetDateTimeValue(Program.ConvertToValidDateTime(objOleDbDataReader[Payments_Col_ALTERADO].ToString().Trim()), -1, -1);
                        #endregion  DATA ALTERADO

                        if (bGetSingleRecord) //Para carregar todos os pagamentos mais rapidament, sõ qdo clica numa linha da grelha é q carrega todo a info
                        {
                            #region     JOIA

                            #region     Has JOIA
                            if (objOleDbDataReader[Payments_Col_JOIA] != DBNull.Value)
                                objPayment.JOIA = Convert.ToString(objOleDbDataReader[Payments_Col_JOIA]);
                            #endregion  Has JOIA

                            #region     JOIA Desc
                            if (objOleDbDataReader[Payments_Col_JOIADESC] != DBNull.Value)
                                objPayment.JOIADESC = Program.SetTextString(Convert.ToString(objOleDbDataReader[Payments_Col_JOIADESC]));
                            #endregion  JOIA Desc

                            #region     JOIA VALOR
                            if (objOleDbDataReader[Payments_Col_JOIAVAL] != DBNull.Value)
                                objPayment.JOIAVAL = Program.SetPayCurrencyEuroDoubleValue(objOleDbDataReader[Payments_Col_JOIAVAL].ToString().Trim());
                            #endregion  JOIA VALOR

                            #region      JOIA Payment Associated
                            if (objOleDbDataReader[Payments_Col_ASSOCJOIA] != DBNull.Value)
                                objPayment.ASSOCJOIA = Convert.ToString(objOleDbDataReader[Payments_Col_ASSOCJOIA]);
                            #endregion   JOIA Payment Associated

                            #region      JOIA Payment Associated Date
                            if (objOleDbDataReader[Payments_Col_DASSOCJOIA] != DBNull.Value)
                                objPayment.DASSOCJOIA = Program.SetDateTimeValue(Program.ConvertToValidDateTime(objOleDbDataReader[Payments_Col_DASSOCJOIA].ToString().Trim()), -1, -1);
                            #endregion   JOIA Payment Associated Date

                            #endregion  JOIA

                            #region     QUOTAS

                            #region     Has QUOTAS
                            if (objOleDbDataReader[Payments_Col_QUOTAS] != DBNull.Value)
                                objPayment.QUOTAS = Convert.ToString(objOleDbDataReader[Payments_Col_QUOTAS]);
                            #endregion  Has QUOTAS

                            #region     QUOTAS Desc
                            if (objOleDbDataReader[Payments_Col_QUOTASDESC] != DBNull.Value)
                                objPayment.QUOTASDESC = Program.SetTextString(Convert.ToString(objOleDbDataReader[Payments_Col_QUOTASDESC]));
                            #endregion  QUOTAS Desc

                            #region     QUOTAS VALOR
                            if (objOleDbDataReader[Payments_Col_QUOTASVAL] != DBNull.Value)
                                objPayment.QUOTASVAL = Program.SetPayCurrencyEuroDoubleValue(objOleDbDataReader[Payments_Col_QUOTASVAL].ToString().Trim());
                            #endregion  QUOTAS VALOR

                            #region      QUOTAS Payment Associated
                            if (objOleDbDataReader[Payments_Col_ASSOCQUOTA] != DBNull.Value)
                                objPayment.ASSOCQUOTA = Convert.ToString(objOleDbDataReader[Payments_Col_ASSOCQUOTA]);
                            #endregion   QUOTAS Payment Associated

                            #region      QUOTAS Payment Associated Date
                            if (objOleDbDataReader[Payments_Col_DASSOCQUOT] != DBNull.Value)
                                objPayment.DASSOCQUOT = Program.SetDateTimeValue(Program.ConvertToValidDateTime(objOleDbDataReader[Payments_Col_DASSOCQUOT].ToString().Trim()), -1, -1);
                            #endregion   QUOTAS Payment Associated Date

                            #endregion  QUOTAS

                            #region     INFRAESTRUTURAS

                            #region     Has INFRAESTRUTURAS
                            if (objOleDbDataReader[Payments_Col_INFRAEST] != DBNull.Value)
                                objPayment.INFRAEST = Convert.ToString(objOleDbDataReader[Payments_Col_INFRAEST]);
                            #endregion  Has INFRAESTRUTURAS

                            #region     INFRAESTRUTURAS Desc
                            if (objOleDbDataReader[Payments_Col_INFRADESC] != DBNull.Value)
                                objPayment.INFRADESC = Program.SetTextString(Convert.ToString(objOleDbDataReader[Payments_Col_INFRADESC]));
                            #endregion  INFRAESTRUTURAS Desc

                            #region     INFRAESTRUTURAS VALOR
                            if (objOleDbDataReader[Payments_Col_INFRAVAL] != DBNull.Value)
                                objPayment.INFRAVAL = Program.SetPayCurrencyEuroDoubleValue(Convert.ToDouble(objOleDbDataReader[Payments_Col_INFRAVAL].ToString().Trim()));
                            #endregion  INFRAESTRUTURAS VALOR

                            #region      INFRAESTRUTURAS Payment Associated
                            if (objOleDbDataReader[Payments_Col_ASSOCNFRA] != DBNull.Value)
                                objPayment.ASSOCNFRA = Convert.ToString(objOleDbDataReader[Payments_Col_ASSOCNFRA]);
                            #endregion   INFRAESTRUTURAS Payment Associated

                            #region      INFRAESTRUTURAS Payment Associated Date
                            if (objOleDbDataReader[Payments_Col_DASSOCINFR] != DBNull.Value)
                                objPayment.DASSOCINFR = Program.SetDateTimeValue(Program.ConvertToValidDateTime(objOleDbDataReader[Payments_Col_DASSOCINFR].ToString().Trim()), -1, -1);
                            #endregion   INFRAESTRUTURAS Payment Associated Date

                            #endregion  INFRAESTRUTURAS


                            #region     CEDENCIAS 

                            #region     Has CEDENCIAS 
                            if (objOleDbDataReader[Payments_Col_CEDENCIAS] != DBNull.Value)
                                objPayment.CEDENCIAS = Convert.ToString(objOleDbDataReader[Payments_Col_CEDENCIAS]);
                            #endregion  Has CEDENCIAS 

                            #region     CEDENCIAS Desc
                            if (objOleDbDataReader[Payments_Col_CEDENCDESC] != DBNull.Value)
                                objPayment.CEDENCDESC = Program.SetTextString(Convert.ToString(objOleDbDataReader[Payments_Col_CEDENCDESC]));
                            #endregion  CEDENCIAS Desc

                            #region     CEDENCIAS VALOR
                            if (objOleDbDataReader[Payments_Col_CEDENCVAL] != DBNull.Value)
                                objPayment.CEDENCVAL = Program.SetPayCurrencyEuroDoubleValue(Convert.ToDouble(objOleDbDataReader[Payments_Col_CEDENCVAL].ToString().Trim()));
                            #endregion  CEDENCIAS VALOR

                            #region     CEDENCIAS Payment Associated
                            if (objOleDbDataReader[Payments_Col_ASSOCCEDEN] != DBNull.Value)
                                objPayment.ASSOCCEDEN = Convert.ToString(objOleDbDataReader[Payments_Col_ASSOCCEDEN]);
                            #endregion  CEDENCIAS Payment Associated

                            #region     CEDENCIAS Payment Associated Date
                            if (objOleDbDataReader[Payments_Col_DASSOCCEDE] != DBNull.Value)
                                objPayment.DASSOCCEDE = Program.SetDateTimeValue(Program.ConvertToValidDateTime(objOleDbDataReader[Payments_Col_DASSOCCEDE].ToString().Trim()), -1, -1);
                            #endregion  CEDENCIAS Payment Associated Date

                            #endregion  CEDENCIAS


                            #region     ESGOT 

                            #region     Has ESGOT 
                            if (objOleDbDataReader[Payments_Col_ESGOT] != DBNull.Value)
                                objPayment.ESGOT = Convert.ToString(objOleDbDataReader[Payments_Col_ESGOT]);
                            #endregion  Has ESGOT 

                            #region     ESGOT Desc
                            if (objOleDbDataReader[Payments_Col_ESGOT] != DBNull.Value)
                                objPayment.ESGOTDESC = Program.SetTextString(Convert.ToString(objOleDbDataReader[Payments_Col_ESGOTDESC]));
                            #endregion  ESGOT Desc

                            #region     ESGOT VALOR
                            if (objOleDbDataReader[Payments_Col_ESGOTVAL] != DBNull.Value)
                                objPayment.ESGOTVAL = Program.SetPayCurrencyEuroDoubleValue(Convert.ToDouble(objOleDbDataReader[Payments_Col_ESGOTVAL].ToString().Trim()));
                            #endregion  ESGOT VALOR

                            #region     ESGOT Payment Associated
                            if (objOleDbDataReader[Payments_Col_ASSOCESGOT] != DBNull.Value)
                                objPayment.ASSOCESGOT = Convert.ToString(objOleDbDataReader[Payments_Col_ASSOCESGOT]);
                            #endregion  ESGOT Payment Associated

                            #region     ESGOT Payment Associated Date
                            if (objOleDbDataReader[Payments_Col_DASSOCESGO] != DBNull.Value)
                                objPayment.DASSOCESGO = Program.SetDateTimeValue(Program.ConvertToValidDateTime(objOleDbDataReader[Payments_Col_DASSOCESGO].ToString().Trim()), -1, -1);
                            #endregion  ESGOT Payment Associated Date

                            #endregion  ESGOT


                            #region     RECONV

                            #region     Has RECONV
                            if (objOleDbDataReader[Payments_Col_RECONV] != DBNull.Value)
                                objPayment.RECONV = Convert.ToString(objOleDbDataReader[Payments_Col_RECONV]);
                            #endregion  Has RECONV

                            #region     RECONV Desc
                            if (objOleDbDataReader[Payments_Col_RECONDESC] != DBNull.Value)
                                objPayment.RECONDESC = Program.SetTextString(Convert.ToString(objOleDbDataReader[Payments_Col_RECONDESC]));
                            #endregion  RECONV Desc

                            #region     RECONV VALOR
                            if (objOleDbDataReader[Payments_Col_RECONVAL] != DBNull.Value)
                                objPayment.RECONVAL = Program.SetPayCurrencyEuroDoubleValue(Convert.ToDouble(objOleDbDataReader[Payments_Col_RECONVAL].ToString().Trim()));
                            #endregion  RECONV VALOR

                            #region      RECONV Payment Associated
                            if (objOleDbDataReader[Payments_Col_ASSOCRECON] != DBNull.Value)
                                objPayment.ASSOCRECON = Convert.ToString(objOleDbDataReader[Payments_Col_ASSOCRECON]);
                            #endregion   RECONV Payment Associated

                            #region      RECONV Payment Associated Date
                            if (objOleDbDataReader[Payments_Col_DASSOCRECO] != DBNull.Value)
                                objPayment.DASSOCRECO = Program.SetDateTimeValue(Program.ConvertToValidDateTime(objOleDbDataReader[Payments_Col_DASSOCRECO].ToString().Trim()), -1, -1);
                            #endregion   RECONV Payment Associated Date

                            #endregion  RECONV

                            #region     OUTRO

                            #region     Has OUTRO
                            if (objOleDbDataReader[Payments_Col_OUTRO] != DBNull.Value)
                                objPayment.OUTRO = Convert.ToString(objOleDbDataReader[Payments_Col_OUTRO]);
                            #endregion  Has OUTRO

                            #region     OUTRO DESC
                            if (objOleDbDataReader[Payments_Col_OUTROSDESC] != DBNull.Value)
                                objPayment.ASSOCJOIA = Program.SetTextString(Convert.ToString(objOleDbDataReader[Payments_Col_OUTROSDESC]));
                            #endregion  OUTRO DESC

                            #region     OUTRO VALOR
                            if (objOleDbDataReader[Payments_Col_OUTROSVAL] != DBNull.Value)
                                objPayment.OUTROSVAL = Program.SetPayCurrencyEuroDoubleValue(Convert.ToDouble(objOleDbDataReader[Payments_Col_OUTROSVAL].ToString().Trim()));
                            #endregion  OUTRO VALOR

                            #region     OUTRO Payment Associated
                            if (objOleDbDataReader[Payments_Col_ASSOCOUTRO] != DBNull.Value)
                                objPayment.ASSOCOUTRO = Convert.ToString(objOleDbDataReader[Payments_Col_ASSOCOUTRO]);
                            #endregion  OUTRO Payment Associated

                            #region     OUTRO Payment Associated Date
                            if (objOleDbDataReader[Payments_Col_DASSOCOUTR] != DBNull.Value)
                                objPayment.DASSOCOUTR = Program.SetDateTimeValue(Program.ConvertToValidDateTime(objOleDbDataReader[Payments_Col_DASSOCOUTR].ToString().Trim()), -1, -1);
                            #endregion  OUTRO Payment Associated Date

                            #endregion  OUTRO
                        }

                        //if (objListPayments.Contains(objPayment))
                        //{
                        //    Int32 iRecordIdx = objListPayments.GetPaymentIndex(objPayment);
                        //    if (iRecordIdx > -1)
                        //    {
                        //        if (objPayment.VALOR > 0 && objListPayments.Payments[iRecordIdx].VALOR <= 0)
                        //        {
                        //            ListDuplicatedToRemove.Add(iRecordIdx);
                        //            objListPayments.Payments.Add(objPayment);
                        //        }
                        //        else
                        //            continue;
                        //    }
                        //}
                        //else
                        objListPayments.Add(objPayment);

                        if (bGetSingleRecord)
                            return objListPayments;
                    }
                }

                #region     Remove Duplicated Regists From Collection
                if (ListDuplicatedToRemove.Count > 0)
                    objListPayments.Payments.RemoveAll(p => ListDuplicatedToRemove.Contains(p.Idx));
                #endregion  Remove Duplicated Regists From Collection

                return objListPayments;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return null;
            }
        }

        /// <versions>29-10-2017(GesAMFC-v0.0.4.29)</versions>
        public Int32 Get_Pay_Max_Number()
        {
            try
            {
                Int32 iMaxPay = -1;

                String sDBf_Filename = LibAMFC.DBF_AMFC_CAIXA_FileName;

                Set_OLE_DB_ConnectionString();
                if (String.IsNullOrEmpty(sDBf_Filename))
                    OLE_DB_Settings();

                using (OleDbConnection objOleDbConnection = new OleDbConnection(OLE_DB_ConnectionString))
                {
                    objOleDbConnection.Open();
                    using (OleDbCommand objOleDbCommand = new OleDbCommand())
                    {
                        objOleDbCommand.Connection = objOleDbConnection;

                        objOleDbCommand.CommandText = "SELECT COUNT(*) FROM " + sDBf_Filename + " " + "\n";
                        if (Convert.ToInt32(objOleDbCommand.ExecuteScalar()) <= 0)
                            return 0;

                        objOleDbCommand.CommandText = "SELECT MAX(" + "ID" + ") AS " + "MaxId" + " FROM " + sDBf_Filename + ";";
                        iMaxPay = Convert.ToInt32(objOleDbCommand.ExecuteScalar());
                    }
                }
                return iMaxPay;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return -1;
            }
        }

        /// <versions>23-10-2017(v0.0.4.23)</versions>
        public Boolean Member_Payment_Already_Exist(AMFCCashPayment objPayment)
        {
            String sQueryString = String.Empty;
            try
            {
                String sDBF_File_Name = LibAMFC.DBF_AMFC_CAIXA_FileName;

                Set_OLE_DB_ConnectionString();
                if (String.IsNullOrEmpty(sDBF_File_Name))
                    OLE_DB_Settings();

                #region     Validate Data
                if (objPayment.SOCIO < new AMFCMember().MinNumber || objPayment.SOCIO > new AMFCMember().MaxNumber)
                {
                    String sError = "Nº de sócio inválido: " + objPayment.SOCIO;
                    MessageBox.Show(sError, "Sócio Inválido", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                if (!Program.IsValidDateTime(objPayment.DATA))
                {
                    String sError = "Data de Pagemento inválida: " + objPayment.DATA.ToString(Program.Date_Format_String, CultureInfo.CurrentCulture);
                    MessageBox.Show(sError, "Sócio Inválido", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                if (objPayment.VALOR <= 0)
                {
                    String sError = "O" + " " + "Pagamento" + " " + "tem de ter um valor monetário maior que zero!";
                    MessageBox.Show(sError, "Valor" + " " + "Pagamento" + " " + "Inválido", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                #endregion  Validate Data

                #region     Query
                sQueryString += "SELECT COUNT(*) FROM " + sDBF_File_Name + " " + "\n";
                sQueryString += "WHERE" + " " + "\n";
                sQueryString += "\t" + Payments_DB_Col_SOCIO + " = " + " " + objPayment.SOCIO                + " " + " " + "\n";
                sQueryString += "AND" + " " + "\n";
                sQueryString += "\t" + Payments_DB_Col_DATA + " LIKE " + "'" + objPayment.DATA.ToString(Program.DBF_Compare_Date_Format_String) + "'" + " " + "\n";
                sQueryString += "AND" + " " + "\n";
                sQueryString += "\t" + Payments_DB_Col_VALOR + " LIKE " + "'" + objPayment.VALOR + "'" + " " + "\n";
                if (!String.IsNullOrEmpty(objPayment.DESIGNACAO.Trim()))
                {
                    sQueryString += "AND" + " " + "\n";
                    sQueryString += "\t" + Payments_DB_Col_DESIGNACAO + " LIKE " + "'" + objPayment.DESIGNACAO.Trim() + "'" + " " + "\n";
                }
                #endregion  Query

                using (OleDbConnection objOleDbConnection = new OleDbConnection(OLE_DB_ConnectionString))
                {
                    objOleDbConnection.Open();
                    using (OleDbCommand objOleDbCommand = new OleDbCommand())
                    {
                        objOleDbCommand.Connection = objOleDbConnection;
                        objOleDbCommand.CommandText = sQueryString;
                        if (Convert.ToInt32(objOleDbCommand.ExecuteScalar()) > 0)
                            return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message + " -> " + "QUERY: " + sQueryString, Program.ErroType.EXCEPTION, true, false);
                return true;
            }
        }

        /// <versions>21-11-2017(GesAMFC-v0.0.4.35)</versions>
        public Boolean Member_Payment_Open_Already_Exist(Int64 lMemberId)
        {
            String sQueryString = String.Empty;
            try
            {
                String sDBF_File_Name = LibAMFC.DBF_AMFC_CAIXA_FileName;

                Set_OLE_DB_ConnectionString();
                if (String.IsNullOrEmpty(sDBF_File_Name))
                    OLE_DB_Settings();

                #region     Validate Data
                if (lMemberId < new AMFCMember().MinNumber || lMemberId > new AMFCMember().MaxNumber)
                {
                    String sError = "Nº de sócio inválido: " + lMemberId;
                    MessageBox.Show(sError, "Sócio Inválido", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
                #endregion  Validate Data

                #region     Query
                sQueryString += "SELECT COUNT(*) FROM " + sDBF_File_Name + " " + "\n";
                sQueryString += "WHERE" + " " + "\n";
                sQueryString += "\t" + Payments_DB_Col_SOCIO + " = " + " " + lMemberId + " " + " " + "\n";
                sQueryString += "AND" + " " + "\n";
                sQueryString += "\t" + Payments_DB_Col_ESTADO + " LIKE " + "'" + new AMFCCashPayment().GetPaymentStateDBvalue(AMFCCashPayment.PaymentState.ABERTO) + "'" + " " + "\n";
                #endregion  Query

                using (OleDbConnection objOleDbConnection = new OleDbConnection(OLE_DB_ConnectionString))
                {
                    objOleDbConnection.Open();
                    using (OleDbCommand objOleDbCommand = new OleDbCommand())
                    {
                        objOleDbCommand.Connection = objOleDbConnection;
                        objOleDbCommand.CommandText = sQueryString;
                        if (Convert.ToInt32(objOleDbCommand.ExecuteScalar()) > 0)
                            return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message + " -> " + "QUERY: " + sQueryString, Program.ErroType.EXCEPTION, true, false);
                return true;
            }
        }

        /// <versions>21-11-2017(GesAMFC-v0.0.4.35)</versions>
        public AMFCCashPayment Get_Member_Payment_Open(Int64 lMemberId)
        {
            String sQueryString = String.Empty;
            try
            {
                AMFCCashPayment objPayment = new AMFCCashPayment();

                String sDBF_File_Name = LibAMFC.DBF_AMFC_CAIXA_FileName;

                Set_OLE_DB_ConnectionString();
                if (String.IsNullOrEmpty(sDBF_File_Name))
                    OLE_DB_Settings();

                #region     Validate Data
                if (lMemberId < new AMFCMember().MinNumber || lMemberId > new AMFCMember().MaxNumber)
                {
                    String sError = "Nº de sócio inválido: " + lMemberId;
                    MessageBox.Show(sError, "Sócio Inválido", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
                #endregion  Validate Data

                #region     Query
                sQueryString += "SELECT " + Payments_DB_Col_ID + " FROM " + sDBF_File_Name + " " + "\n";
                sQueryString += "WHERE" + " " + "\n";
                sQueryString += "\t" + Payments_DB_Col_SOCIO + " = " + " " + lMemberId + " " + " " + "\n";
                sQueryString += "AND" + " " + "\n";
                sQueryString += "\t" + Payments_DB_Col_ESTADO + " LIKE " + "'" + objPayment.GetPaymentStateDBvalue(AMFCCashPayment.PaymentState.ABERTO) + "'" + " " + "\n";
                #endregion  Query

                Int32 iCount = 0;
                using (OleDbConnection objOleDbConnection = new OleDbConnection(OLE_DB_ConnectionString))
                {
                    objOleDbConnection.Open();
                    using (OleDbCommand objOleDbCommand = new OleDbCommand())
                    {
                        objOleDbCommand.Connection = objOleDbConnection;
                        objOleDbCommand.CommandText = sQueryString;
                        using (OleDbDataReader objOleDbDataReader = objOleDbCommand.ExecuteReader())
                        {
                            while (objOleDbDataReader.Read())
                            {
                                iCount++;
                                if (objOleDbDataReader[Payments_DB_Col_ID] != DBNull.Value  )
                                    objPayment.ID = Convert.ToInt64(objOleDbDataReader[Payments_DB_Col_ID]);
                            }
                        }
                    }
                }

                if (iCount != 1 || objPayment.ID < 1)
                {
                    String sError = "Não foi possivel obter o " + "ID" + " do "+ "Pagamento" + " em aberto " + " para o " + " " + "Sócio com o Nº: " + lMemberId + "";
                    if (!String.IsNullOrEmpty(objPayment.NOME))
                        sError += " " + "(" + "Nome: " + objPayment.NOME + ")";
                    MessageBox.Show(sError, "Pagamento Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }

                objPayment = Get_Cash_Payment_By_Id(objPayment.ID);
                if (objPayment == null)
                {
                    String sError = "Não foi possivel obter o " + "Pagamento" + " em aberto " + " para o " + " " + "Sócio com o Nº: " + lMemberId + "";
                    if (!String.IsNullOrEmpty(objPayment.NOME))
                        sError += " " + "(" + "Nome: " + objPayment.NOME + ")";
                    MessageBox.Show(sError, "Pagamento Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }

                return objPayment;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message + " -> " + "QUERY: " + sQueryString, Program.ErroType.EXCEPTION, true, false);
                return null;
            }
        }

        /// <versions>26-11-2017(GesAMFC-v0.0.4.37)</versions>
        public AMFCCashPayment Get_Member_Payment_By_Id(Int64 lMemberId, Int64 lPayId)
        {
            String sQueryString = String.Empty;
            try
            {
                AMFCCashPayment objPayment = new AMFCCashPayment();

                objPayment.ID = lPayId;

                String sDBF_File_Name = LibAMFC.DBF_AMFC_CAIXA_FileName;

                Set_OLE_DB_ConnectionString();
                if (String.IsNullOrEmpty(sDBF_File_Name))
                    OLE_DB_Settings();

                #region     Validate Data
                if (lMemberId < new AMFCMember().MinNumber || lMemberId > new AMFCMember().MaxNumber)
                {
                    String sError = "Nº de sócio inválido: " + lMemberId;
                    MessageBox.Show(sError, "Sócio Inválido", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }
                #endregion  Validate Data

                if (objPayment.ID < 1)
                {
                    String sError = "Não foi possivel obter o " + "ID" + " do " + "Pagamento" + " em aberto " + " para o " + " " + "Sócio com o Nº: " + lMemberId + "";
                    if (!String.IsNullOrEmpty(objPayment.NOME))
                        sError += " " + "(" + "Nome: " + objPayment.NOME + ")";
                    MessageBox.Show(sError, "Pagamento Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }

                objPayment = Get_Cash_Payment_By_Id(objPayment.ID);
                if (objPayment == null)
                {
                    String sError = "Não foi possivel obter o " + "Pagamento" + " em aberto " + " para o " + " " + "Sócio com o Nº: " + lMemberId + "";
                    if (!String.IsNullOrEmpty(objPayment.NOME))
                        sError += " " + "(" + "Nome: " + objPayment.NOME + ")";
                    MessageBox.Show(sError, "Pagamento Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return null;
                }

                return objPayment;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message + " -> " + "QUERY: " + sQueryString, Program.ErroType.EXCEPTION, true, false);
                return null;
            }
        }

        /// <versions>12-11-2017(v0.0.4.32)</versions>
        public Int64 Add_Payment(AMFCCashPayment objPayment)
        {
            String sQueryString = String.Empty;
            try
            {
                String sDBF_File_Name = LibAMFC.DBF_AMFC_CAIXA_FileName;

                Set_OLE_DB_ConnectionString();
                if (String.IsNullOrEmpty(sDBF_File_Name))
                    OLE_DB_Settings();

                #region     Validate Data
                if (objPayment.ID < 1)
                {
                    String sError = "Pagamento" + " " + "ID" + " " + "inválido" + ": " + objPayment.ID;
                    MessageBox.Show(sError, "ID Inválido", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return -1;
                }
                if (objPayment.SOCIO < new AMFCMember().MinNumber || objPayment.SOCIO > new AMFCMember().MaxNumber)
                {
                    String sError = "Nº de sócio inválido: " + objPayment.SOCIO;
                    MessageBox.Show(sError, "Sócio Inválido", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return -1;
                }
                if (!Program.IsValidDateTime(objPayment.DATA))
                {
                    String sError = "Data de Pagemento inválida: " + objPayment.DATA.ToString(Program.Date_Format_String, CultureInfo.CurrentCulture);
                    MessageBox.Show(sError, "Sócio Inválido", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return -1;
                }
                if (objPayment.VALOR <= 0)
                {
                    String sError = "O" + " " + "Pagamento" + " " + "tem de ter um valor monetário maior que zero!";
                    MessageBox.Show(sError, "Valor" + " " + "Pagamento" + " " + "Inválido", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return -1;
                }
                //.... verificar outros no futuro
                #endregion  Validate Data

                #region     Check if Member Payment already exist
                if (Member_Payment_Already_Exist(objPayment))
                {
                    String sWarning = "Já foi" + " " + "adicionado" + " " + "um " + " " + "Pagamento" + " " + "do" + " " + "Sócio com o Nº: " + objPayment.SOCIO + "";
                    if (!String.IsNullOrEmpty(objPayment.NOME))
                        sWarning += " " + "(" + "Nome: " + objPayment.NOME + ")";
                    sWarning += " " + "nesta data, com esta designação!!";
                    sWarning += " " + "Se é o mesmo valor que pretende pagar, por favor, apenas altere a designação do pagamento."; 
                    MessageBox.Show(sWarning, "Pagamento" + " " + "já existente", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return 0;
                }
                #endregion  Check if Member Payment already exist

                #region     Check if Member Payment Open already exist
                if (objPayment.Payment_State == AMFCCashPayment.PaymentState.ABERTO)
                {
                    if (Member_Payment_Open_Already_Exist(objPayment.SOCIO))
                    {
                        String sWarning = "Já existe" + " um " + " " + "Pagamento" + " em aberto " + " para o " + " " + "Sócio com o Nº: " + objPayment.SOCIO + "";
                        if (!String.IsNullOrEmpty(objPayment.NOME))
                            sWarning += " " + "(" + "Nome: " + objPayment.NOME + ")";
                        sWarning += " " + "Se é o mesmo valor que pretende pagar, por favor, apenas altere a designação do pagamento.";
                        MessageBox.Show(sWarning, "Pagamento em aberto já existente", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return -2;
                    }
                }
                #endregion  Check if Member Payment Open already exist

                #region     Query

                sQueryString += "INSERT INTO " + sDBF_File_Name + " " + "\n";
                sQueryString += "( " + " " + "\n";

                #region     Columns

                sQueryString += "\t" + Payments_DB_Col_ID      + "," + " " + "\n";     //Auto-increment: max(ID)+1
                sQueryString += "\t" + Payments_DB_Col_SOCIO   + "," + " " + "\n";

                if (!String.IsNullOrEmpty(objPayment.LISTARECNU) && objPayment.LISTARECNU.Trim().ToLower() != Program.DB_Not_Available.Trim().ToLower())
                    sQueryString += "\t" + Payments_DB_Col_LISTARECNU + "," + " " + "\n";

                if (!String.IsNullOrEmpty(objPayment.NOME) && objPayment.NOME.Trim().ToLower() != Program.DB_Not_Available.Trim().ToLower())
                    sQueryString += "\t" + Payments_DB_Col_NOME + "," + " " + "\n";

                if (!String.IsNullOrEmpty(objPayment.NOME_PAG) && objPayment.NOME_PAG.Trim().ToLower() != Program.DB_Not_Available.Trim().ToLower())
                    sQueryString += "\t" + Payments_DB_Col_NOME_PAG + "," + " " + "\n";

                if (!String.IsNullOrEmpty(objPayment.DESIGNACAO) && objPayment.DESIGNACAO.Trim().ToLower() != Program.DB_Not_Available.Trim().ToLower())
                    sQueryString += "\t" + Payments_DB_Col_DESIGNACAO + "," + " " + "\n";

                if (objPayment.VALOR > 0)
                    sQueryString += "\t" + Payments_DB_Col_VALOR + "," + " " + "\n";

                if (Program.IsValidDateTime(objPayment.DATA))
                    sQueryString += "\t" + Payments_DB_Col_DATA + "," + " " + "\n";

                if (Program.IsValidDateTime(objPayment.ALTERADO))
                    sQueryString += "\t" + Payments_DB_Col_ALTERADO + "," + " " + "\n";

                if (objPayment.Payment_State != AMFCCashPayment.PaymentState.UNDEFINED && !String.IsNullOrEmpty(objPayment.Payment_State_DB_Value))
                    sQueryString += "\t" + Payments_DB_Col_ESTADO + "," + " " + "\n";

                #region     JOIA
                if (
                        (!String.IsNullOrEmpty(objPayment.JOIADESC) && objPayment.JOIADESC.Trim().ToLower() != Program.DB_Not_Available.Trim().ToLower())
                        &&
                        objPayment.JOIAVAL > 0
                    )
                {
                    sQueryString += "\t" + Payments_DB_Col_JOIA + "," + " " + "\n";
                    sQueryString += "\t" + Payments_DB_Col_JOIADESC + "," + " " + "\n";
                    sQueryString += "\t" + Payments_DB_Col_JOIAVAL + "," + " " + "\n";
                    sQueryString += "\t" + Payments_DB_Col_ASSOCJOIA + "," + " " + "\n";
                    if (Program.IsValidDateTime(objPayment.DASSOCJOIA))
                        sQueryString += "\t" + Payments_DB_Col_DASSOCJOIA + "," + " " + "\n";
                }
                #endregion     JOIA

                #region     QUOTAS
                if (
                    (!String.IsNullOrEmpty(objPayment.QUOTASDESC) && objPayment.QUOTASDESC.Trim().ToLower() != Program.DB_Not_Available.Trim().ToLower())
                    &&
                    objPayment.QUOTASVAL > 0
                )
                {
                    sQueryString += "\t" + Payments_DB_Col_QUOTAS + "," + " " + "\n";
                    sQueryString += "\t" + Payments_DB_Col_QUOTASDESC + "," + " " + "\n";
                    sQueryString += "\t" + Payments_DB_Col_QUOTASVAL + "," + " " + "\n";
                    sQueryString += "\t" + Payments_DB_Col_ASSOCQUOTA + "," + " " + "\n";
                    if (Program.IsValidDateTime(objPayment.DASSOCQUOT))
                        sQueryString += "\t" + Payments_DB_Col_DASSOCQUOT + "," + " " + "\n";
                }
                #endregion  QUOTAS

                #region     INFRAESTRUTURAS
                if (
                    (!String.IsNullOrEmpty(objPayment.INFRADESC) && objPayment.INFRADESC.Trim().ToLower() != Program.DB_Not_Available.Trim().ToLower())
                    &&
                    objPayment.INFRAVAL > 0
                )
                {
                    sQueryString += "\t" + Payments_DB_Col_INFRAEST + "," + " " + "\n";
                    sQueryString += "\t" + Payments_DB_Col_INFRADESC + "," + " " + "\n";
                    sQueryString += "\t" + Payments_DB_Col_INFRAVAL + "," + " " + "\n";
                    sQueryString += "\t" + Payments_DB_Col_ASSOCNFRA + "," + " " + "\n";
                    if (Program.IsValidDateTime(objPayment.DASSOCINFR))
                        sQueryString += "\t" + Payments_DB_Col_DASSOCINFR + "," + " " + "\n";
                }
                #endregion  INFRAESTRUTURAS

                #region     CEDENCIAS
                if (
                    (!String.IsNullOrEmpty(objPayment.CEDENCDESC) && objPayment.CEDENCDESC.Trim().ToLower() != Program.DB_Not_Available.Trim().ToLower())
                    &&
                    objPayment.CEDENCVAL > 0
                )
                {
                    sQueryString += "\t" + Payments_DB_Col_CEDENCIAS + "," + " " + "\n";
                    sQueryString += "\t" + Payments_DB_Col_CEDENCDESC + "," + " " + "\n";
                    sQueryString += "\t" + Payments_DB_Col_CEDENCVAL + "," + " " + "\n";
                    sQueryString += "\t" + Payments_DB_Col_ASSOCCEDEN + "," + " " + "\n";
                    if (Program.IsValidDateTime(objPayment.DASSOCCEDE))
                        sQueryString += "\t" + Payments_DB_Col_DASSOCCEDE + "," + " " + "\n";
                }
                #endregion  CEDENCIAS

                #region     ESGOT
                if (
                    (!String.IsNullOrEmpty(objPayment.ESGOTDESC) && objPayment.ESGOTDESC.Trim().ToLower() != Program.DB_Not_Available.Trim().ToLower())
                    &&
                    objPayment.ESGOTVAL > 0
                )
                {
                    sQueryString += "\t" + Payments_DB_Col_ESGOT + "," + " " + "\n";
                    sQueryString += "\t" + Payments_DB_Col_ESGOTDESC + "," + " " + "\n";
                    sQueryString += "\t" + Payments_DB_Col_ESGOTVAL + "," + " " + "\n";
                    sQueryString += "\t" + Payments_DB_Col_ASSOCESGOT + "," + " " + "\n";
                    if (Program.IsValidDateTime(objPayment.DASSOCESGO))
                        sQueryString += "\t" + Payments_DB_Col_DASSOCESGO + "," + " " + "\n";
                }
                #endregion  ESGOT

                #region     RECONVERSAO URBANISTICA
                if (
                    (!String.IsNullOrEmpty(objPayment.RECONDESC) && objPayment.RECONDESC.Trim().ToLower() != Program.DB_Not_Available.Trim().ToLower())
                    &&
                    objPayment.RECONVAL > 0
                )
                {
                    sQueryString += "\t" + Payments_DB_Col_RECONV + "," + " " + "\n";
                    sQueryString += "\t" + Payments_DB_Col_RECONDESC + "," + " " + "\n";
                    sQueryString += "\t" + Payments_DB_Col_RECONVAL + "," + " " + "\n";
                    sQueryString += "\t" + Payments_DB_Col_ASSOCRECON + "," + " " + "\n";
                    if (Program.IsValidDateTime(objPayment.DASSOCRECO))
                        sQueryString += "\t" + Payments_DB_Col_DASSOCRECO + "," + " " + "\n";
                }
                #endregion  RECONVERSAO URBANISTICA

                #region     OUTROS
                if (
                    (!String.IsNullOrEmpty(objPayment.OUTROSDESC) && objPayment.OUTROSDESC.Trim().ToLower() != Program.DB_Not_Available.Trim().ToLower())
                    &&
                    objPayment.OUTROSVAL > 0
                )
                {
                    sQueryString += "\t" + Payments_DB_Col_OUTRO + "," + " " + "\n";
                    sQueryString += "\t" + Payments_DB_Col_OUTROSDESC + "," + " " + "\n";
                    sQueryString += "\t" + Payments_DB_Col_OUTROSVAL + "," + " " + "\n";
                    sQueryString += "\t" + Payments_DB_Col_ASSOCOUTRO + "," + " " + "\n";
                    if (Program.IsValidDateTime(objPayment.DASSOCOUTR))
                        sQueryString += "\t" + Payments_DB_Col_DASSOCOUTR + "," + " " + "\n";
                }
                #endregion  OUTROS

                sQueryString += "\t" + Payments_DB_Col_NOTAS + " " + " " + "\n";

                #endregion  Columns

                sQueryString += ") " + " " + "\n" + "\n";

                sQueryString += "VALUES" + " " + "\n" + "\n";

                sQueryString += "( " + " " + "\n";

                #region     Values

                sQueryString += "\t" + objPayment.ID + "," + " " + "\n";

                sQueryString += "\t" + objPayment.SOCIO + "," + " " + "\n";

                if (!String.IsNullOrEmpty(objPayment.LISTARECNU) && objPayment.LISTARECNU.Trim().ToLower() != Program.DB_Not_Available.Trim().ToLower())
                    sQueryString += "\t" + "'" + objPayment.LISTARECNU.Trim().ToUpper() + "'" + "," + " " + "\n";

                if (!String.IsNullOrEmpty(objPayment.NOME) && objPayment.NOME.Trim().ToLower() != Program.DB_Not_Available.Trim().ToLower())
                    sQueryString += "\t" + "'" + objPayment.NOME.Trim().ToUpper() + "'" + "," + " " + "\n";

                if (!String.IsNullOrEmpty(objPayment.NOME_PAG) && objPayment.NOME_PAG.Trim().ToLower() != Program.DB_Not_Available.Trim().ToLower())
                    sQueryString += "\t" + "'" + objPayment.NOME_PAG.Trim().ToUpper() + "'" + "," + " " + "\n";

                if (!String.IsNullOrEmpty(objPayment.DESIGNACAO) && objPayment.DESIGNACAO.Trim().ToLower() != Program.DB_Not_Available.Trim().ToLower())
                {
                    //sQueryString += "\t" + "'" + objPayment.DESIGNACAO.Trim().ToUpper() + "'" + "," + " " + "\n";
                    sQueryString += "\t" + "'" + objPayment.DESIGNACAO.Trim() + "'" + "," + " " + "\n";
                }

                if (objPayment.VALOR > 0)
                    sQueryString += "\t" + "'" + Program.SetPayDoubleStringValue(objPayment.VALOR) + "'" + "," + " " + "\n";

                if (Program.IsValidDateTime(objPayment.DATA))
                    sQueryString += "\t" + "'" + objPayment.DATA.ToString(Program.DBF_Insert_Date_Format_String) + "'" + "," + " " + "\n";

                if (Program.IsValidDateTime(objPayment.ALTERADO))
                    sQueryString += "\t" + "'" + objPayment.ALTERADO.ToString(Program.DBF_Insert_Date_Format_String) + "'" + "," + " " + "\n";

                if (objPayment.Payment_State != AMFCCashPayment.PaymentState.UNDEFINED && !String.IsNullOrEmpty(objPayment.Payment_State_DB_Value))
                    sQueryString += "\t" + "'" + objPayment.Payment_State_DB_Value + "'" + "," + " " + "\n";

                #region     JOIA
                if (
                        (!String.IsNullOrEmpty(objPayment.JOIADESC) && objPayment.JOIADESC.Trim().ToLower() != Program.DB_Not_Available.Trim().ToLower())
                        &&
                        objPayment.JOIAVAL > 0
                    )
                {
                    sQueryString += "\t" + "'" + Program.ConvertBooleanToYesOrNo(objPayment.HasJOIA) + "'" + "," + " " + "\n";
                    sQueryString += "\t" + "'" + objPayment.JOIADESC + "'" + "," + " " + "\n";
                    sQueryString += "\t" + "'" + Program.SetPayDoubleStringValue(objPayment.JOIAVAL) + "'" + "," + " " + "\n";
                    sQueryString += "\t" + "'" + Program.ConvertBooleanToYesOrNo(objPayment.IsASSOCJOIA) + "'" + "," + " " + "\n";
                    if (Program.IsValidDateTime(objPayment.DASSOCJOIA))
                        sQueryString += "\t" + "'" + objPayment.DASSOCJOIA.ToString(Program.DBF_Insert_Date_Format_String) + "'" + "," + " " + "\n";
                }
                #endregion  JOIA

                #region     QUOTAS
                if (
                    (!String.IsNullOrEmpty(objPayment.QUOTASDESC) && objPayment.QUOTASDESC.Trim().ToLower() != Program.DB_Not_Available.Trim().ToLower())
                    &&
                    objPayment.QUOTASVAL > 0
                )
                {
                    sQueryString += "\t" + "'" + Program.ConvertBooleanToYesOrNo(objPayment.HasQUOTAS) + "'" + "," + " " + "\n";
                    sQueryString += "\t" + "'" + objPayment.QUOTASDESC + "'" + "," + " " + "\n";
                    sQueryString += "\t" + "'" + Program.SetPayDoubleStringValue(objPayment.QUOTASVAL) + "'" +"," + " " + "\n";
                    sQueryString += "\t" + "'" + Program.ConvertBooleanToYesOrNo(objPayment.IsASSOCQUOTA) + "'" + "," + " " + "\n";
                    if (Program.IsValidDateTime(objPayment.DASSOCQUOT))
                        sQueryString += "\t" + "'" + objPayment.DASSOCQUOT.ToString(Program.DBF_Insert_Date_Format_String) + "'" + "," + " " + "\n";
                }
                #endregion  QUOTAS

                #region     INFRAESTRUTURAS
                if (
                    (!String.IsNullOrEmpty(objPayment.INFRADESC) && objPayment.INFRADESC.Trim().ToLower() != Program.DB_Not_Available.Trim().ToLower())
                    &&
                    objPayment.INFRAVAL > 0
                )
                {
                    sQueryString += "\t" + "'" + Program.ConvertBooleanToYesOrNo(objPayment.HasINFRAEST) + "'" + "," + " " + "\n";
                    sQueryString += "\t" + "'" + objPayment.INFRADESC + "'" + "," + " " + "\n";
                    sQueryString += "\t" + "'" + Program.SetPayDoubleStringValue(objPayment.INFRAVAL) + "'" + "," + " " + "\n";
                    sQueryString += "\t" + "'" + Program.ConvertBooleanToYesOrNo(objPayment.IsASSOCNFRA) + "'" + "," + " " + "\n";
                    if (Program.IsValidDateTime(objPayment.DASSOCINFR))
                        sQueryString += "\t" + "'" + objPayment.DASSOCINFR.ToString(Program.DBF_Insert_Date_Format_String) + "'" + "," + " " + "\n";

                }
                #endregion  INFRAESTRUTURAS

                #region     CEDENCIAS
                if (
                    (!String.IsNullOrEmpty(objPayment.CEDENCDESC) && objPayment.CEDENCDESC.Trim().ToLower() != Program.DB_Not_Available.Trim().ToLower())
                    &&
                    objPayment.CEDENCVAL > 0
                )
                {
                    sQueryString += "\t" + "'" + Program.ConvertBooleanToYesOrNo(objPayment.HasCEDENCIAS) + "'" + "," + " " + "\n";
                    sQueryString += "\t" + "'" + objPayment.CEDENCDESC + "'" + "," + " " + "\n";
                    sQueryString += "\t" + "'" + Program.SetPayDoubleStringValue(objPayment.CEDENCVAL) + "'" + "," + " " + "\n";
                    sQueryString += "\t" + "'" + Program.ConvertBooleanToYesOrNo(objPayment.IsASSOCCEDEN) + "'" + "," + " " + "\n";
                    if (Program.IsValidDateTime(objPayment.DASSOCCEDE))
                        sQueryString += "\t" + "'" + objPayment.DASSOCCEDE.ToString(Program.DBF_Insert_Date_Format_String) + "'" + "," + " " + "\n";

                }
                #endregion  CEDENCIAS

                #region     ESGOT
                if (
                    (!String.IsNullOrEmpty(objPayment.ESGOTDESC) && objPayment.ESGOTDESC.Trim().ToLower() != Program.DB_Not_Available.Trim().ToLower())
                    &&
                    objPayment.ESGOTVAL > 0
                )
                {
                    sQueryString += "\t" + "'" + Program.ConvertBooleanToYesOrNo(objPayment.HasESGOT) + "'" + "," + " " + "\n";
                    sQueryString += "\t" + "'" + objPayment.ESGOTDESC + "'" + "," + " " + "\n";
                    sQueryString += "\t" + "'" + Program.SetPayDoubleStringValue(objPayment.ESGOTVAL) + "'" + "," + " " + "\n";
                    sQueryString += "\t" + "'" + Program.ConvertBooleanToYesOrNo(objPayment.IsASSOCESGOT) + "'" + "," + " " + "\n";
                    if (Program.IsValidDateTime(objPayment.DASSOCESGO))
                        sQueryString += "\t" + "'" + objPayment.DASSOCESGO.ToString(Program.DBF_Insert_Date_Format_String) + "'" + "," + " " + "\n";

                }
                #endregion  ESGOT

                #region     RECONVERSAO URBANISTICA
                if (
                    (!String.IsNullOrEmpty(objPayment.RECONDESC) && objPayment.RECONDESC.Trim().ToLower() != Program.DB_Not_Available.Trim().ToLower())
                    &&
                    objPayment.RECONVAL > 0
                )
                {
                    sQueryString += "\t" + "'" + Program.ConvertBooleanToYesOrNo(objPayment.HasRECONV) + "'" + "," + " " + "\n";
                    sQueryString += "\t" + "'" + objPayment.RECONDESC + "'" + "," + " " + "\n";
                    sQueryString += "\t" + "'" + Program.SetPayDoubleStringValue(objPayment.RECONVAL) + "'" + "," + " " + "\n";
                    sQueryString += "\t" + "'" + Program.ConvertBooleanToYesOrNo(objPayment.IsASSOCRECON) + "'" + "," + " " + "\n";
                    if (Program.IsValidDateTime(objPayment.DASSOCRECO))
                        sQueryString += "\t" + "'" + objPayment.DASSOCRECO.ToString(Program.DBF_Insert_Date_Format_String) + "'" + "," + " " + "\n";
                }
                #endregion  RECONVERSAO URBANISTICA

                #region     OUTROS
                if (
                    (!String.IsNullOrEmpty(objPayment.OUTROSDESC) && objPayment.OUTROSDESC.Trim().ToLower() != Program.DB_Not_Available.Trim().ToLower())
                    &&
                    objPayment.OUTROSVAL > 0
                )
                {
                    sQueryString += "\t" + "'" + Program.ConvertBooleanToYesOrNo(objPayment.HasOUTRO) + "'" + "," + " " + "\n";
                    sQueryString += "\t" + "'" + objPayment.OUTROSDESC + "'" + "," + " " + "\n";
                    sQueryString += "\t" + "'" + Program.SetPayDoubleStringValue(objPayment.OUTROSVAL) + "'" + "," + " " + "\n";
                    sQueryString += "\t" + "'" + Program.ConvertBooleanToYesOrNo(objPayment.IsASSOCOUTRO) + "'" + "," + " " + "\n";
                    if (Program.IsValidDateTime(objPayment.DASSOCOUTR))
                        sQueryString += "\t" + "'" + objPayment.DASSOCOUTR.ToString(Program.DBF_Insert_Date_Format_String) + "'" + "," + " " + "\n";
                }
                #endregion  OUTROS

                sQueryString += "\t" + "'" + objPayment.NOTAS.Trim() + "'" + " " + " " + "\n";

                #endregion  Values

                sQueryString += ") " + " " + "\n";
                
                #endregion  Query

                using (OleDbConnection objOleDbConnection = new OleDbConnection(OLE_DB_ConnectionString))
                {
                    objOleDbConnection.Open();
                    using (OleDbCommand objOleDbCommand = new OleDbCommand())
                    {
                        objOleDbCommand.Connection = objOleDbConnection;
                        objOleDbCommand.CommandText = sQueryString;
                        if (objOleDbCommand.ExecuteNonQuery() != 1)
                            return -1;
                    }
                }

                return 1;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message + " -> " + "QUERY: " + sQueryString, Program.ErroType.EXCEPTION, true, false);
                return -1;
            }
        }

        /// <versions>12-11-2017(v0.0.4.32)</versions>
        public Int64 Edit_Payment(AMFCCashPayment objPayment)
        {
            String sQueryString = String.Empty;
            try
            {
                String sDBF_File_Name = LibAMFC.DBF_AMFC_CAIXA_FileName;

                Set_OLE_DB_ConnectionString();
                if (String.IsNullOrEmpty(sDBF_File_Name))
                    OLE_DB_Settings();

                #region     Validate Data
                if (objPayment.ID < 1)
                {
                    String sError = "Pagamento" + " " + "ID" + " " + "inválido" + ": " + objPayment.ID;
                    MessageBox.Show(sError, "ID Inválido", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return -1;
                }
                if (objPayment.SOCIO < new AMFCMember().MinNumber || objPayment.SOCIO > new AMFCMember().MaxNumber)
                {
                    String sError = "Nº de sócio inválido: " + objPayment.SOCIO;
                    MessageBox.Show(sError, "Sócio Inválido", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return -1;
                }
                if (!Program.IsValidDateTime(objPayment.DATA))
                {
                    String sError = "Data de Pagemento inválida: " + objPayment.DATA.ToString(Program.Date_Format_String, CultureInfo.CurrentCulture);
                    MessageBox.Show(sError, "Sócio Inválido", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return -1;
                }
                if (objPayment.VALOR <= 0)
                {
                    String sError = "O" + " " + "Pagamento" + " " + "tem de ter um valor monetário maior que zero!";
                    MessageBox.Show(sError, "Valor" + " " + "Pagamento" + " " + "Inválido", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return -1;
                }
                //.... verificar outros no futuro
                #endregion  Validate Data

                #region     Query
                sQueryString += "UPDATE " + sDBF_File_Name + " " + "\n";
                sQueryString += "SET" + " " + "\n";

                sQueryString += "\t" + Payments_DB_Col_SOCIO + " = " + objPayment.SOCIO + "," + " " + "\n";

                if (!String.IsNullOrEmpty(objPayment.LISTARECNU) && objPayment.LISTARECNU.Trim().ToLower() != Program.DB_Not_Available.Trim().ToLower())
                    sQueryString += "\t" + "'" + objPayment.LISTARECNU.Trim().ToUpper() + "'" + " = " + " " + "'" + objPayment.LISTARECNU.Trim().ToUpper() + "'" + "," + " " + "\n";

                if (!String.IsNullOrEmpty(objPayment.NOME) && objPayment.NOME.Trim().ToLower() != Program.DB_Not_Available.Trim().ToLower())
                    sQueryString += "\t" + Payments_DB_Col_NOME + " = " + "'" + objPayment.NOME.Trim().ToUpper() + "'" + "," + " " + "\n";

                if (!String.IsNullOrEmpty(objPayment.NOME_PAG) && objPayment.NOME_PAG.Trim().ToLower() != Program.DB_Not_Available.Trim().ToLower())
                    sQueryString += "\t" + Payments_DB_Col_NOME_PAG + " = " + "'" + objPayment.NOME_PAG.Trim().ToUpper() + "'" + "," + " " + "\n";

                if (!String.IsNullOrEmpty(objPayment.DESIGNACAO) && objPayment.DESIGNACAO.Trim().ToLower() != Program.DB_Not_Available.Trim().ToLower())
                    sQueryString += "\t" + Payments_DB_Col_DESIGNACAO + " = " + "'" + objPayment.DESIGNACAO.Trim() + "'" + "," + " " + "\n";

                if (objPayment.VALOR > 0)
                    sQueryString += "\t" + Payments_DB_Col_VALOR + " = " + "'" + Program.SetPayDoubleStringValue(objPayment.VALOR) + "'" + "," + " " + "\n";

                if (Program.IsValidDateTime(objPayment.DATA))
                    sQueryString += "\t" + Payments_DB_Col_DATA + " = " + "'" + objPayment.DATA.ToString(Program.DBF_Insert_Date_Format_String) + "'" + "," + " " + "\n";

                if (Program.IsValidDateTime(objPayment.ALTERADO))
                    sQueryString += "\t" + Payments_DB_Col_ALTERADO + " = " + "'" + objPayment.ALTERADO.ToString(Program.DBF_Insert_Date_Format_String) + "'" + "," + " " + "\n";

                if (objPayment.Payment_State != AMFCCashPayment.PaymentState.UNDEFINED && !String.IsNullOrEmpty(objPayment.Payment_State_DB_Value))
                    sQueryString += "\t" + Payments_DB_Col_ESTADO + " = " + "'" + objPayment.Payment_State_DB_Value + "'" + "," + " " + "\n";

                #region     JOIA
                if (
                        (!String.IsNullOrEmpty(objPayment.JOIADESC) && objPayment.JOIADESC.Trim().ToLower() != Program.DB_Not_Available.Trim().ToLower())
                        &&
                        objPayment.JOIAVAL > 0
                    )
                {
                    sQueryString += "\t" + Payments_DB_Col_JOIA + " = " + "'" + Program.ConvertBooleanToYesOrNo(objPayment.HasJOIA) + "'" + "," + " " + "\n";
                    sQueryString += "\t" + Payments_DB_Col_JOIADESC + " = " + "'" + objPayment.JOIADESC + "'" + "," + " " + "\n";
                    sQueryString += "\t" + Payments_DB_Col_JOIAVAL + " = " + "'" + Program.SetPayDoubleStringValue(objPayment.JOIAVAL) + "'" + "," + " " + "\n";
                    sQueryString += "\t" + Payments_DB_Col_ASSOCJOIA + " = " + "'" + Program.ConvertBooleanToYesOrNo(objPayment.IsASSOCJOIA) + "'" + "," + " " + "\n";
                    if (Program.IsValidDateTime(objPayment.DASSOCJOIA))
                        sQueryString += "\t" + Payments_DB_Col_DASSOCJOIA + " = " + "'" + objPayment.DASSOCJOIA.ToString(Program.DBF_Insert_Date_Format_String) + "'" + "," + " " + "\n";
                }
                #endregion  JOIA

                #region     QUOTAS
                if (
                    (!String.IsNullOrEmpty(objPayment.QUOTASDESC) && objPayment.QUOTASDESC.Trim().ToLower() != Program.DB_Not_Available.Trim().ToLower())
                    &&
                    objPayment.QUOTASVAL > 0
                )
                {
                    sQueryString += "\t" + Payments_DB_Col_QUOTAS + " = " + "'" + Program.ConvertBooleanToYesOrNo(objPayment.HasQUOTAS) + "'" + "," + " " + "\n";
                    sQueryString += "\t" + Payments_DB_Col_QUOTASDESC + " = " + "'" + objPayment.QUOTASDESC + "'" + "," + " " + "\n";
                    sQueryString += "\t" + Payments_DB_Col_QUOTASVAL + " = " + "'" + Program.SetPayDoubleStringValue(objPayment.QUOTASVAL) + "'" + "," + " " + "\n";
                    sQueryString += "\t" + Payments_DB_Col_ASSOCQUOTA + " = " + "'" + Program.ConvertBooleanToYesOrNo(objPayment.IsASSOCQUOTA) + "'" + "," + " " + "\n";
                    if (Program.IsValidDateTime(objPayment.DASSOCQUOT))
                        sQueryString += "\t" + Payments_DB_Col_DASSOCQUOT + " = " + "'" + objPayment.DASSOCQUOT.ToString(Program.DBF_Insert_Date_Format_String) + "'" + "," + " " + "\n";
                }
                #endregion  QUOTAS

                #region     INFRAESTRUTURAS
                if (
                    (!String.IsNullOrEmpty(objPayment.INFRADESC) && objPayment.INFRADESC.Trim().ToLower() != Program.DB_Not_Available.Trim().ToLower())
                    &&
                    objPayment.INFRAVAL > 0
                )
                {
                    sQueryString += "\t" + Payments_DB_Col_INFRAEST + " = " + "'" + Program.ConvertBooleanToYesOrNo(objPayment.HasINFRAEST) + "'" + "," + " " + "\n";
                    sQueryString += "\t" + Payments_DB_Col_INFRADESC + " = " + "'" + objPayment.INFRADESC + "'" + "," + " " + "\n";
                    sQueryString += "\t" + Payments_DB_Col_INFRAVAL + " = " + "'" + Program.SetPayDoubleStringValue(objPayment.INFRAVAL) + "'" + "," + " " + "\n";
                    sQueryString += "\t" + Payments_DB_Col_ASSOCNFRA + " = " + "'" + Program.ConvertBooleanToYesOrNo(objPayment.IsASSOCNFRA) + "'" + "," + " " + "\n";
                    if (Program.IsValidDateTime(objPayment.DASSOCINFR))
                        sQueryString += "\t" + Payments_DB_Col_DASSOCINFR + " = " + "'" + objPayment.DASSOCINFR.ToString(Program.DBF_Insert_Date_Format_String) + "'" + "," + " " + "\n";
                }
                #endregion  INFRAESTRUTURAS

                #region     CEDENCIAS
                if (
                    (!String.IsNullOrEmpty(objPayment.CEDENCDESC) && objPayment.CEDENCDESC.Trim().ToLower() != Program.DB_Not_Available.Trim().ToLower())
                    &&
                    objPayment.CEDENCVAL > 0
                )
                {
                    sQueryString += "\t" + Payments_DB_Col_CEDENCIAS + " = " + "'" + Program.ConvertBooleanToYesOrNo(objPayment.HasCEDENCIAS) + "'" + "," + " " + "\n";
                    sQueryString += "\t" + Payments_DB_Col_CEDENCDESC + " = " + "'" + objPayment.CEDENCDESC + "'" + "," + " " + "\n";
                    sQueryString += "\t" + Payments_DB_Col_CEDENCVAL + " = " + "'" + Program.SetPayDoubleStringValue(objPayment.CEDENCVAL) + "'" + "," + " " + "\n";
                    sQueryString += "\t" + Payments_DB_Col_ASSOCCEDEN + " = " + "'" + Program.ConvertBooleanToYesOrNo(objPayment.IsASSOCCEDEN) + "'" + "," + " " + "\n";
                    if (Program.IsValidDateTime(objPayment.DASSOCCEDE))
                        sQueryString += "\t" + Payments_DB_Col_DASSOCCEDE + " = " + "'" + objPayment.DASSOCCEDE.ToString(Program.DBF_Insert_Date_Format_String) + "'" + "," + " " + "\n";
                }
                #endregion  CEDENCIAS

                #region     ESGOT
                if (
                    (!String.IsNullOrEmpty(objPayment.ESGOTDESC) && objPayment.ESGOTDESC.Trim().ToLower() != Program.DB_Not_Available.Trim().ToLower())
                    &&
                    objPayment.ESGOTVAL > 0
                )
                {
                    sQueryString += "\t" + Payments_DB_Col_ESGOT + " = " + "'" + Program.ConvertBooleanToYesOrNo(objPayment.HasESGOT) + "'" + "," + " " + "\n";
                    sQueryString += "\t" + Payments_DB_Col_ESGOTDESC + " = " + "'" + objPayment.ESGOTDESC + "'" + "," + " " + "\n";
                    sQueryString += "\t" + Payments_DB_Col_ESGOTVAL + " = " + "'" + Program.SetPayDoubleStringValue(objPayment.ESGOTVAL) + "'" + "," + " " + "\n";
                    sQueryString += "\t" + Payments_DB_Col_ASSOCESGOT + " = " + "'" + Program.ConvertBooleanToYesOrNo(objPayment.IsASSOCESGOT) + "'" + "," + " " + "\n";
                    if (Program.IsValidDateTime(objPayment.DASSOCESGO))
                        sQueryString += "\t" + Payments_DB_Col_DASSOCESGO + " = " + "'" + objPayment.DASSOCESGO.ToString(Program.DBF_Insert_Date_Format_String) + "'" + "," + " " + "\n";
                }
                #endregion  ESGOT


                #region     RECONVERSAO URBANISTICA
                if (
                    (!String.IsNullOrEmpty(objPayment.RECONDESC) && objPayment.RECONDESC.Trim().ToLower() != Program.DB_Not_Available.Trim().ToLower())
                    &&
                    objPayment.RECONVAL > 0
                )
                {
                    sQueryString += "\t" + Payments_DB_Col_RECONV + " = " + "'" + Program.ConvertBooleanToYesOrNo(objPayment.HasRECONV) + "'" + "," + " " + "\n";
                    sQueryString += "\t" + Payments_DB_Col_RECONDESC + " = " + "'" + objPayment.RECONDESC + "'" + "," + " " + "\n";
                    sQueryString += "\t" + Payments_DB_Col_RECONVAL + " = " + " " + "'" + Program.SetPayDoubleStringValue(objPayment.RECONVAL) + "'" + "," + " " + "\n";
                    sQueryString += "\t" + Payments_DB_Col_ASSOCRECON + " = " + "'" + Program.ConvertBooleanToYesOrNo(objPayment.IsASSOCRECON) + "'" + "," + " " + "\n";
                    if (Program.IsValidDateTime(objPayment.DASSOCRECO))
                        sQueryString += "\t" + Payments_DB_Col_DASSOCRECO + " = " + "'" + objPayment.DASSOCRECO.ToString(Program.DBF_Insert_Date_Format_String) + "'" + "," + " " + "\n";
                }
                #endregion  RECONVERSAO URBANISTICA

                #region     OUTROS
                if (
                    (!String.IsNullOrEmpty(objPayment.OUTROSDESC) && objPayment.OUTROSDESC.Trim().ToLower() != Program.DB_Not_Available.Trim().ToLower())
                    &&
                    objPayment.OUTROSVAL > 0
                )
                {
                    sQueryString += "\t" + Payments_DB_Col_OUTRO + " = " + "'" + Program.ConvertBooleanToYesOrNo(objPayment.HasOUTRO) + "'" + "," + " " + "\n";
                    sQueryString += "\t" + Payments_DB_Col_OUTROSDESC + " = " + "'" + objPayment.OUTROSDESC + "'" + "," + " " + "\n";
                    sQueryString += "\t" + Payments_DB_Col_OUTROSVAL + " = " +  "'" + Program.SetPayDoubleStringValue(objPayment.OUTROSVAL) + "'" + "," + " " + "\n";
                    sQueryString += "\t" + Payments_DB_Col_ASSOCOUTRO + " = " + "'" + Program.ConvertBooleanToYesOrNo(objPayment.IsASSOCOUTRO) + "'" + "," + " " + "\n";
                    if (Program.IsValidDateTime(objPayment.DASSOCOUTR))
                        sQueryString += "\t" + Payments_DB_Col_DASSOCOUTR + " = " + "'" + objPayment.DASSOCOUTR.ToString(Program.DBF_Insert_Date_Format_String) + "'" + "," + " " + "\n";
                }
                #endregion  OUTROS

                sQueryString += "\t" + Payments_DB_Col_NOTAS + " = " + "'" + objPayment.NOTAS.Trim() + "'" + " " + " " + "\n";

                sQueryString += "WHERE " + "ID" + " = " + objPayment.ID.ToString() + "\n";
                #endregion  Query

                using (OleDbConnection objOleDbConnection = new OleDbConnection(OLE_DB_ConnectionString))
                {
                    objOleDbConnection.Open();
                    using (OleDbCommand objOleDbCommand = new OleDbCommand())
                    {
                        objOleDbCommand.Connection = objOleDbConnection;
                        objOleDbCommand.CommandText = sQueryString;
                        if (objOleDbCommand.ExecuteNonQuery() != 1)
                            return -1;
                    }
                }

                return 1;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message + " -> " + "QUERY: " + sQueryString, Program.ErroType.EXCEPTION, true, false);
                return -1;
            }
        }

        /// <versions>04-11-2017(v0.0.4.30)</versions>
        public Int64 Del_Payment(Int64 lPaymentId)
        {
            String sQueryString = String.Empty;
            try
            {
                String sDBF_File_Name = LibAMFC.DBF_AMFC_CAIXA_FileName;

                Set_OLE_DB_ConnectionString();
                if (String.IsNullOrEmpty(sDBF_File_Name))
                    OLE_DB_Settings();

                #region     Validate Data
                if (lPaymentId < 1)
                {
                    String sError = "Nº de " + "Pagamento" + " inválido: " + lPaymentId;
                    MessageBox.Show(sError, "Pagamento" + " Inválido", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return -1;
                }
                #endregion  Validate Data

                #region     Query
                sQueryString = "DELETE FROM " + sDBF_File_Name + " WHERE " + "ID" + " = " + lPaymentId + ";" + "\n";
                #endregion  Query

                using (OleDbConnection objOleDbConnection = new OleDbConnection(OLE_DB_ConnectionString))
                {
                    objOleDbConnection.Open();
                    using (OleDbCommand objOleDbCommand = new OleDbCommand())
                    {
                        objOleDbCommand.Connection = objOleDbConnection;
                        objOleDbCommand.CommandText = sQueryString;
                        if (objOleDbCommand.ExecuteNonQuery() != 1)
                            return -1;
                    }
                }

                return 1;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message + " -> " + "QUERY: " + sQueryString, Program.ErroType.EXCEPTION, true, false);
                return -1;
            }
        }

        #endregion  Admin Payments

        #region     Admin Joias

        /// <versions>07-10-2017(v0.0.4.8)</versions>
        public AMFCMemberJoias Get_All_Member_Joias()
        {
            try
            {
                AMFCMemberJoias objListJoias = new AMFCMemberJoias();
                Set_OLE_DB_ConnectionString();

                AMFCMember objMember = new AMFCMember();
                using (OleDbConnection objOleDbConnection = new OleDbConnection(OLE_DB_ConnectionString))
                {
                    objOleDbConnection.Open();
                    using (OleDbCommand objOleDbCommand = new OleDbCommand())
                    {
                        objOleDbCommand.Connection = objOleDbConnection;
                        objOleDbCommand.CommandText = Set_Member_Joias_Query(-1, -1, false);

                        objListJoias = Get_Members_Joias_DB_Data(objOleDbCommand, false);
                        if (objListJoias == null || objListJoias.Joias == null)
                        {
                            StackFrame objStackFrame = new StackFrame();
                            String sErrorMsg = "Não foi possível obter a Lista de Joias dos Sócios da Base de Dados!" + " -> " + "QUERY: " + objOleDbCommand.CommandText;
                            Program.HandleError(objStackFrame.GetMethod().Name, sErrorMsg, Program.ErroType.ERROR, true, true);
                            objStackFrame = null;
                            return null;
                        }
                    }
                }

                return objListJoias;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return null;
            }
        }

        /// <versions>07-10-2017(v0.0.4.8)</versions>
        public AMFCMemberJoia Get_Member_Joia_ByMemberNbr(Int64 lMemberNumber)
        {
            try
            {
                AMFCMemberJoia objMemberJoia = new AMFCMemberJoia();
                Set_OLE_DB_ConnectionString();

                using (OleDbConnection objOleDbConnection = new OleDbConnection(OLE_DB_ConnectionString))
                {
                    objOleDbConnection.Open();
                    using (OleDbCommand objOleDbCommand = new OleDbCommand())
                    {
                        objOleDbCommand.Connection = objOleDbConnection;
                        objOleDbCommand.CommandText = Set_Member_Joias_Query(lMemberNumber, -1, true);

                        AMFCMemberJoias objListJoias = Get_Members_Joias_DB_Data(objOleDbCommand, true);
                        if (objListJoias == null || objListJoias.Joias == null || objListJoias.Joias.Count > 1)
                        {
                            StackFrame objStackFrame = new StackFrame();
                            String sErrorMsg = "Não foi possível obter a Joia do Sócio com o Nº: " + lMemberNumber + " -> " + "QUERY: " + objOleDbCommand.CommandText;
                            Program.HandleError(objStackFrame.GetMethod().Name, sErrorMsg, Program.ErroType.ERROR, true, true);
                            objStackFrame = null;
                            return null;
                        }
                        if (objListJoias.Joias.Count == 1)
                            objMemberJoia = objListJoias.Joias[0];
                    }
                }

                return objMemberJoia;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return null;
            }
        }

        /// <versions>07-10-2017(v0.0.4.8)</versions>
        public AMFCMemberJoia Get_Member_Joia_ByJoiaId(Int64 lJoiaId)
        {
            try
            {
                AMFCMemberJoia objMemberJoia = new AMFCMemberJoia();
                Set_OLE_DB_ConnectionString();

                using (OleDbConnection objOleDbConnection = new OleDbConnection(OLE_DB_ConnectionString))
                {
                    objOleDbConnection.Open();
                    using (OleDbCommand objOleDbCommand = new OleDbCommand())
                    {
                        objOleDbCommand.Connection = objOleDbConnection;
                        objOleDbCommand.CommandText = Set_Member_Joias_Query(-1, lJoiaId, true);

                        AMFCMemberJoias objListJoias = Get_Members_Joias_DB_Data(objOleDbCommand, true);
                        if (objListJoias == null || objListJoias.Joias == null || objListJoias.Joias.Count > 1)
                        {
                            StackFrame objStackFrame = new StackFrame();
                            String sErrorMsg = "Não foi possível obter a Joia com o ID: " + lJoiaId + " -> " + "QUERY: " + objOleDbCommand.CommandText;
                            Program.HandleError(objStackFrame.GetMethod().Name, sErrorMsg, Program.ErroType.ERROR, true, true);
                            objStackFrame = null;
                            return null;
                        }
                        if (objListJoias.Joias.Count == 1)
                            objMemberJoia = objListJoias.Joias[0];
                    }
                }

                return objMemberJoia;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return null;
            }
        }

        /// <versions>07-10-2017(v0.0.4.8)</versions>
        public String Set_Member_Joias_Query(Int64 lMemberNumber, Int64 lJoiaId, Boolean bGetSingleRecord)
        {
            try
            {
                String sQueryString = String.Empty;
                
                sQueryString += "SELECT"    + " " + "\n";
                sQueryString += LibAMFC.DBF_AMFC_JOIAS_FileName + "." + "ID"                + " AS " + "ID" + "," + " " + "\n";
                sQueryString += LibAMFC.DBF_AMFC_JOIAS_FileName + "." + "LISTACAIXA"        + " AS " + "LISTACAIXA" + "," + " " + "\n";
                sQueryString += LibAMFC.DBF_AMFC_JOIAS_FileName + "." + "LISTARECNU"        + " AS " + "LISTARECNU" + "," + " " + "\n";
                sQueryString += LibAMFC.DBF_AMFC_SOCIO_FileName + "." + "NUMERO"            + " AS " + "SOCIONUMERO" + "," + " " + "\n";
                sQueryString += LibAMFC.DBF_AMFC_SOCIO_FileName + "." + "NOME"              + " AS " + "SOCIONOME" + "," + " " + "\n";

                sQueryString += LibAMFC.DBF_AMFC_SOCIO_FileName + "." + "AREALOTE" + " AS " + "AREALOTE" + "," + " " + "\n";

                sQueryString += LibAMFC.DBF_AMFC_SOCIO_FileName + "." + "DATAADMI"          + " AS " + "DATAADMI" + "," + " " + "\n";
                sQueryString += LibAMFC.DBF_AMFC_JOIAS_FileName + "." + "JOIA"              + " AS " + "JoiaValue" + "," + " " + "\n";
                sQueryString += LibAMFC.DBF_AMFC_JOIAS_FileName + "." + "PAGO"              + " AS " + "PAGO" + "," + " " + "\n";
                sQueryString += LibAMFC.DBF_AMFC_JOIAS_FileName + "." + "FALTA"             + " AS " + "FALTA" + "," + " " + "\n";
                sQueryString += LibAMFC.DBF_AMFC_JOIAS_FileName + "." + "EMPAG"             + " AS " + "EMPAG" + "," + " " + "\n";
                sQueryString += LibAMFC.DBF_AMFC_JOIAS_FileName + "." + "ESTADO"            + " AS " + "ESTADO" + "," + " " + "\n";
                sQueryString += LibAMFC.DBF_AMFC_JOIAS_FileName + "." + "DATA"              + " AS " + "DATA" + "," + " " + "\n";
                sQueryString += LibAMFC.DBF_AMFC_JOIAS_FileName + "." + "DATAPAG"           + " AS " + "DATAPAG" + "," + " " + "\n";
                sQueryString += LibAMFC.DBF_AMFC_JOIAS_FileName + "." + "NOME"              + " AS " + "PAGOUNOME" + "," + " " + "\n";
                sQueryString += "YEAR"  + "(" + LibAMFC.DBF_AMFC_JOIAS_FileName + "." + "DATA" + ")" + " AS " + "ANO" + "," + " " + "\n";
                sQueryString += "MONTH" + "(" + LibAMFC.DBF_AMFC_JOIAS_FileName + "." + "DATA" + ")" + " AS " + "MES" + "," + " " + "\n";
                sQueryString += LibAMFC.DBF_AMFC_JOIAS_FileName + "." + "DATAPAGREG"        + " AS " + "DATAPAGREG" + "," + " " + "\n";
                sQueryString += LibAMFC.DBF_AMFC_JOIAS_FileName + "." + "NOTAS"             + " AS " + "NOTAS" + " " + " " + "\n";
                sQueryString += "FROM       " + " " + "\n";
                sQueryString += "\t" + LibAMFC.DBF_AMFC_SOCIO_FileName + " " + "\n";

                if (bGetSingleRecord)
                    sQueryString += "INNER" + " " + "JOIN" + " " + "\n";
                else
                    sQueryString += "LEFT" + " " + "JOIN" + " " + "\n";

                sQueryString += "\t" + LibAMFC.DBF_AMFC_JOIAS_FileName + " " + "\n";
                sQueryString += "ON " + " " + LibAMFC.DBF_AMFC_SOCIO_FileName + "." + new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMERO) + " = " + LibAMFC.DBF_AMFC_JOIAS_FileName + "." + "SOCIO" + " " + "\n";
                if (bGetSingleRecord && lMemberNumber >= new AMFCMember().MinNumber && lMemberNumber <= new AMFCMember().MaxNumber)
                {
                    sQueryString += "WHERE" + " " + "\n";
                    sQueryString += LibAMFC.DBF_AMFC_SOCIO_FileName + "." + new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMERO) + " = " + lMemberNumber + "\n";
                }
                else if (bGetSingleRecord && lJoiaId > 0)
                {
                    sQueryString += "WHERE" + " " + "\n";
                    sQueryString += LibAMFC.DBF_AMFC_JOIAS_FileName + "." + "ID" + " = " + lJoiaId + "\n";
                }
                else
                {
                    sQueryString += "ORDER BY" + " " + "\n";
                    sQueryString += LibAMFC.DBF_AMFC_JOIAS_FileName + "." + "DATA" + "," + " " + "\n";
                    sQueryString += LibAMFC.DBF_AMFC_SOCIO_FileName + "." + "DATAADMI" + "," + " " + "\n";
                    sQueryString += LibAMFC.DBF_AMFC_SOCIO_FileName + "." + new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMERO) + " " + "\n";
                }
                sQueryString += ";";

                return sQueryString;
            }
            catch
            {
                return null;
            }
        }

        /// <versions>07-10-2017(v0.0.4.8)</versions>
        public String Set_Joias_Pay_Open(Int64 lMemberNumber)
        {
            try
            {
                if (lMemberNumber < new AMFCMember().MinNumber || lMemberNumber > new AMFCMember().MaxNumber)
                    return String.Empty;

                String sQueryString = String.Empty;

                sQueryString += "SELECT" + " " + "\n";
                sQueryString += LibAMFC.DBF_AMFC_JOIAS_FileName + "." + "ID"                + " AS " + "ID" + "," + " " + "\n"; 
                sQueryString += LibAMFC.DBF_AMFC_JOIAS_FileName + "." + "LISTACAIXA"        + " AS " + "LISTACAIXA" + "," + " " + "\n"; 
                sQueryString += LibAMFC.DBF_AMFC_JOIAS_FileName + "." + "LISTARECNU"        + " AS " + "LISTARECNU" + "," + " " + "\n";
                sQueryString += LibAMFC.DBF_AMFC_SOCIO_FileName + "." + "NUMERO"            + " AS " + "SOCIONUMERO" + "," + " " + "\n";
                sQueryString += LibAMFC.DBF_AMFC_SOCIO_FileName + "." + "NOME"              + " AS " + "SOCIONOME" + "," + " " + "\n";
                sQueryString += LibAMFC.DBF_AMFC_SOCIO_FileName + "." + "AREALOTE" + " AS " + "AREALOTE" + "," + " " + "\n";
                sQueryString += LibAMFC.DBF_AMFC_SOCIO_FileName + "." + "DATAADMI"          + " AS " + "DATAADMI" + "," + " " + "\n";
                sQueryString += LibAMFC.DBF_AMFC_JOIAS_FileName + "." + "JOIA"              + " AS " + "JoiaValue" + "," + " " + "\n";
                sQueryString += LibAMFC.DBF_AMFC_JOIAS_FileName + "." + "PAGO"              + " AS " + "PAGO" + "," + " " + "\n";
                sQueryString += LibAMFC.DBF_AMFC_JOIAS_FileName + "." + "FALTA"             + " AS " + "FALTA" + "," + " " + "\n";
                sQueryString += LibAMFC.DBF_AMFC_JOIAS_FileName + "." + "EMPAG"             + " AS " + "EMPAG" + "," + " " + "\n";
                sQueryString += LibAMFC.DBF_AMFC_JOIAS_FileName + "." + "ESTADO"            + " AS " + "ESTADO" + "," + " " + "\n";
                sQueryString += LibAMFC.DBF_AMFC_JOIAS_FileName + "." + "DATA"              + " AS " + "DATA" + "," + " " + "\n";
                sQueryString += LibAMFC.DBF_AMFC_JOIAS_FileName + "." + "DATAPAG"           + " AS " + "DATAPAG" + "," + " " + "\n";
                sQueryString += LibAMFC.DBF_AMFC_JOIAS_FileName + "." + "NOME"              + " AS " + "PAGOUNOME" + "," + " " + "\n";
                sQueryString += "YEAR"  + "(" + LibAMFC.DBF_AMFC_JOIAS_FileName + "." + "DATA" + ")" + " AS " + "ANO" + "," + " " + "\n";
                sQueryString += "MONTH" + "(" + LibAMFC.DBF_AMFC_JOIAS_FileName + "." + "DATA" + ")" + " AS " + "MES" + "," + " " + "\n";
                sQueryString += LibAMFC.DBF_AMFC_JOIAS_FileName + "." + "DATAPAGREG"        + " AS " + "DATAPAGREG" + "," + " " + "\n";
                sQueryString += LibAMFC.DBF_AMFC_JOIAS_FileName + "." + "NOTAS"             + " AS " + "NOTAS" + " " + " " + "\n"; 
                sQueryString += "FROM       " + " " + "\n";
                sQueryString += "\t" + LibAMFC.DBF_AMFC_SOCIO_FileName + " " + "\n";

                sQueryString += "LEFT" + " " + "JOIN" + " " + "\n";

                sQueryString += "\t" + LibAMFC.DBF_AMFC_JOIAS_FileName + " " + "\n";
                sQueryString += "ON " + " " + LibAMFC.DBF_AMFC_SOCIO_FileName + "." + new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMERO) + " = " + LibAMFC.DBF_AMFC_JOIAS_FileName + "." + "SOCIO" + " " + "\n";
                if (lMemberNumber >= new AMFCMember().MinNumber && lMemberNumber <= new AMFCMember().MaxNumber)
                {
                    sQueryString += "WHERE" + " " + "\n";
                    sQueryString += LibAMFC.DBF_AMFC_SOCIO_FileName + "." + new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMERO) + " = " + lMemberNumber + "\n";
                    sQueryString += "AND" + " " + "\n";
                    sQueryString += LibAMFC.DBF_AMFC_JOIAS_FileName + "." + "ESTADO" + " LIKE " + "'" + new AMFCMemberJoia().GetPay_StateDBvalue(AMFCMemberJoia.PayState.EM_PAGAMENTO) + "'" + " " + "\n";
                }
                else
                {
                    sQueryString += "ORDER BY" + " " + "\n";
                    sQueryString += LibAMFC.DBF_AMFC_JOIAS_FileName + "." + "DATA" + "," + " " + "\n";
                    sQueryString += LibAMFC.DBF_AMFC_SOCIO_FileName + "." + "DATAADMI" + "," + " " + "\n";
                    sQueryString += LibAMFC.DBF_AMFC_SOCIO_FileName + "." + new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMERO) + " " + "\n";
                }
                sQueryString += ";";

                return sQueryString;
            }
            catch
            {
                return null;
            }
        }

        /// <versions>27-11-2017(GesAMFC-v0.0.4.38)</versions>
        public AMFCMemberJoias Get_Joias_Pay_Open(Int64 lMemberNumber)
        {
            try
            {
                AMFCMemberJoias objListJoias = new AMFCMemberJoias();
                Set_OLE_DB_ConnectionString();

                AMFCMember objMember = new AMFCMember();
                using (OleDbConnection objOleDbConnection = new OleDbConnection(OLE_DB_ConnectionString))
                {
                    objOleDbConnection.Open();
                    using (OleDbCommand objOleDbCommand = new OleDbCommand())
                    {
                        objOleDbCommand.Connection = objOleDbConnection;
                        objOleDbCommand.CommandText = Set_Joias_Pay_Open(lMemberNumber);

                        objListJoias = Get_Members_Joias_DB_Data(objOleDbCommand, false);
                        if (objListJoias == null || objListJoias.Joias == null)
                        {
                            StackFrame objStackFrame = new StackFrame();
                            String sErrorMsg = "Não foi possível obter a Lista de Joias dos Sócios da Base de Dados!" + " -> " + "QUERY: " + objOleDbCommand.CommandText;
                            Program.HandleError(objStackFrame.GetMethod().Name, sErrorMsg, Program.ErroType.ERROR, true, true);
                            objStackFrame = null;
                            return null;
                        }
                    }
                }

                return objListJoias;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return null;
            }
        }

        /// <versions>06-10-2017(v0.0.4.7)</versions>
        private AMFCMemberJoias Get_Members_Joias_DB_Data(OleDbCommand objOleDbCommand, Boolean bGetSingleRecord)
        {
            try
            {
                AMFCMemberJoias objListJoias = new AMFCMemberJoias();
                
                List<Int64> ListDuplicatedToRemove = new List<Int64>();

                using (OleDbDataReader objOleDbDataReader = objOleDbCommand.ExecuteReader())
                {
                    while (objOleDbDataReader.Read())
                    {
                        AMFCMemberJoia objJoia = new AMFCMemberJoia();

                        if (objOleDbDataReader["ID"] == DBNull.Value)
                            continue;
                        if (objOleDbDataReader["SOCIONUMERO"] == DBNull.Value)
                            continue;
                        if (objOleDbDataReader["SOCIONOME"] == DBNull.Value)
                            continue;

                        objJoia.JoiaId = Convert.ToInt64(objOleDbDataReader["ID"]);
                        objJoia.MemberNumber = Convert.ToInt64(objOleDbDataReader["SOCIONUMERO"]);
                        objJoia.MemberName = objOleDbDataReader["SOCIONOME"].ToString();               

                        if (objOleDbDataReader["LISTACAIXA"] != DBNull.Value)
                            objJoia.JoiaListaCaixa = Program.SetTextString(Convert.ToString(objOleDbDataReader["LISTACAIXA"]));

                        if (objOleDbDataReader["LISTARECNU"] != DBNull.Value)
                            objJoia.JoiaListaRecibos = Program.SetTextString(Convert.ToString(objOleDbDataReader["LISTARECNU"]));

                        if (objOleDbDataReader["DATAADMI"] != DBNull.Value)
                            objJoia.DtMemberAdmiDate = Program.SetDateTimeValue(Program.ConvertToValidDateTime(objOleDbDataReader["DATAADMI"].ToString().Trim()), -1, -1);

                        if (objOleDbDataReader["JoiaValue"] != DBNull.Value)
                            objJoia.JoiaValue = Program.SetPayCurrencyEuroDoubleValue(Convert.ToDouble(objOleDbDataReader["JoiaValue"].ToString().Trim()));

                        if (objOleDbDataReader["PAGO"] != DBNull.Value)
                            objJoia.ValuePaid = Program.SetPayCurrencyEuroDoubleValue(Convert.ToDouble(objOleDbDataReader["PAGO"].ToString().Trim()));

                        if (objOleDbDataReader["FALTA"] != DBNull.Value)
                            objJoia.ValueMissing = Program.SetPayCurrencyEuroDoubleValue(Convert.ToDouble(objOleDbDataReader["FALTA"].ToString().Trim()));

                        if (objOleDbDataReader["EMPAG"] != DBNull.Value)
                            objJoia.ValueOnPaying = Program.SetPayCurrencyEuroDoubleValue(Convert.ToDouble(objOleDbDataReader["EMPAG"].ToString().Trim()));

                        if (objOleDbDataReader["ESTADO"] != DBNull.Value)
                            objJoia.Pay_State_DB_Value = objOleDbDataReader["ESTADO"].ToString().Trim();


                        if (objOleDbDataReader["DATA"] != DBNull.Value)
                        {
                            objJoia.DtJoiaDate = Program.SetDateTimeValue(Program.ConvertToValidDateTime(objOleDbDataReader["DATA"].ToString().Trim()), -1, -1);
                        
                            if (objOleDbDataReader["ANO"] != DBNull.Value)
                                objJoia.JoiaYearInt = Convert.ToInt32(objOleDbDataReader["ANO"].ToString().Trim());
   
                            if (objOleDbDataReader["MES"] != DBNull.Value)
                                objJoia.JoiaMonthInt = Convert.ToInt32(objOleDbDataReader["MES"].ToString().Trim());
                        }

                        if (objJoia.JoiaValue > 0)
                        {
                            if (objOleDbDataReader["DATAPAG"] != DBNull.Value)
                            {
                                objJoia.DtJoiaDatePaid = Program.SetDateTimeValue(Program.ConvertToValidDateTime(objOleDbDataReader["DATAPAG"].ToString().Trim()), -1, -1);

                                if (objOleDbDataReader["PAGOUNOME"] != DBNull.Value)
                                    objJoia.JoiaPaidPerson = Convert.ToString(objOleDbDataReader["PAGOUNOME"]);

                                if (objOleDbDataReader["DATAPAGREG"] != DBNull.Value)
                                    objJoia.DtJoiaDataPagamentoAgregado = Program.ConvertToValidDateTime(objOleDbDataReader["DATAPAGREG"].ToString().Trim());
                            }
                        }

                        if (objOleDbDataReader["NOTAS"] != DBNull.Value)
                            objJoia.JoiaNotas = Program.SetTextString(Convert.ToString(objOleDbDataReader["NOTAS"]));

                        if (objListJoias.Contains(objJoia))
                        {
                            Int32 iRecordIdx = objListJoias.GetJoiaIndex(objJoia);
                            if (iRecordIdx > -1)
                            {
                                if (objJoia.JoiaValue > 0 && objListJoias.Joias[iRecordIdx].JoiaValue <= 0)
                                {
                                    ListDuplicatedToRemove.Add(iRecordIdx);
                                    objListJoias.Joias.Add(objJoia);
                                }
                            }
                        }
                        else
                            objListJoias.Add(objJoia);
                            
                        if (bGetSingleRecord)
                            return objListJoias;
                    }
                }

                #region     Remove Duplicated Regists From Collection
                if (ListDuplicatedToRemove.Count > 0)
                    objListJoias.Joias.RemoveAll(j => ListDuplicatedToRemove.Contains(j.Idx));
                #endregion  Remove Duplicated Regists From Collection

                return objListJoias;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return null;
            }
        }

        /// <versions>06-10-2017(v0.0.4.8)</versions>
        public Boolean Member_Joia_Already_Exist(AMFCMemberJoia objMemberJoia)
        {
            String sQueryString = String.Empty;
            try
            {
                String sDBF_File_Name = LibAMFC.DBF_AMFC_JOIAS_FileName;

                Set_OLE_DB_ConnectionString();
                if (String.IsNullOrEmpty(sDBF_File_Name))
                    OLE_DB_Settings();

                #region     Query
                sQueryString += "SELECT COUNT(*) FROM " + sDBF_File_Name + " " + "\n";
                sQueryString += "WHERE" + " " + "\n";
                sQueryString += "\t" + "SOCIO" + " = " + objMemberJoia.MemberNumber + " " + "\n";
                sQueryString += "; ";
                #endregion  Query

                using (OleDbConnection objOleDbConnection = new OleDbConnection(OLE_DB_ConnectionString))
                {
                    objOleDbConnection.Open();
                    using (OleDbCommand objOleDbCommand = new OleDbCommand())
                    {
                        objOleDbCommand.Connection = objOleDbConnection;
                        objOleDbCommand.CommandText = sQueryString;
                        if (Convert.ToInt32(objOleDbCommand.ExecuteScalar()) > 0)
                            return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message + " -> " + "QUERY: " + sQueryString, Program.ErroType.EXCEPTION, true, false);
                return true;
            }
        }

        /// <versions>07-10-2017(v0.0.4.8)</versions>
        public Int32 Get_Joia_Max_Number()
        {
            String sQueryString = String.Empty;
            try
            {
                Int32 iMaxJoia = -1;

                String sDBf_Filename = LibAMFC.DBF_AMFC_JOIAS_FileName;

                Set_OLE_DB_ConnectionString();
                if (String.IsNullOrEmpty(sDBf_Filename))
                    OLE_DB_Settings();

                using (OleDbConnection objOleDbConnection = new OleDbConnection(OLE_DB_ConnectionString))
                {
                    objOleDbConnection.Open();
                    using (OleDbCommand objOleDbCommand = new OleDbCommand())
                    {
                        objOleDbCommand.Connection = objOleDbConnection;

                        objOleDbCommand.CommandText = "SELECT COUNT(*) FROM " + sDBf_Filename + " " + "\n";
                        if (Convert.ToInt32(objOleDbCommand.ExecuteScalar()) <= 0)
                            return 0;

                        objOleDbCommand.CommandText = "SELECT MAX(" + "ID" + ") AS " + "MaxId" + " FROM " + sDBf_Filename + ";";
                        iMaxJoia = Convert.ToInt32(objOleDbCommand.ExecuteScalar());
                    }
                }
                return iMaxJoia;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message + " -> " + "QUERY: " + sQueryString, Program.ErroType.EXCEPTION, true, false);
                return -1;
            }
        }

        /// <versions>08-10-2017(v0.0.4.8)</versions>
        public String Set_Members_Joia_Not_Paid_Query()
        {
            try
            {
                String sQueryString = String.Empty;

                sQueryString += "SELECT" + " " + "\n";
                sQueryString += LibAMFC.DBF_AMFC_JOIAS_FileName + "." + "ID"        + " AS " + "ID"             + "," + " " + "\n"; 
                sQueryString += LibAMFC.DBF_AMFC_SOCIO_FileName + "." + "NUMERO"    + " AS " + "SOCIONUMERO"    + "," + " " + "\n";
                sQueryString += LibAMFC.DBF_AMFC_SOCIO_FileName + "." + "NOME"      + " AS " + "SOCIONOME"      + "," + " " + "\n";
                sQueryString += LibAMFC.DBF_AMFC_SOCIO_FileName + "." + "AREALOTE" + " AS " + "AREALOTE" + "," + " " + "\n";
                sQueryString += LibAMFC.DBF_AMFC_SOCIO_FileName + "." + "DATAADMI"  + " AS " + "DATAADMI"       + " " + " " + "\n"; 
                sQueryString += "FROM"  + " " + "\n";
                sQueryString += "\t" + LibAMFC.DBF_AMFC_SOCIO_FileName + " " + "\n";

                sQueryString += "LEFT"  + " " + "JOIN" + " " + "\n";

                sQueryString += "\t" + LibAMFC.DBF_AMFC_JOIAS_FileName + " " + "\n";
                sQueryString += "ON "   + " " + LibAMFC.DBF_AMFC_SOCIO_FileName + "." + new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMERO) + " = " + LibAMFC.DBF_AMFC_JOIAS_FileName + "." + "SOCIO" + " " + "\n";
                sQueryString += "WHERE" + " " + "\n";
                sQueryString += LibAMFC.DBF_AMFC_JOIAS_FileName + "." + "ID" + " IS NULL " + "\n";
                sQueryString += "ORDER BY" + " " + "\n";
                sQueryString += LibAMFC.DBF_AMFC_SOCIO_FileName + "." + new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMERO) + " " + "\n";
                sQueryString += ";";

                return sQueryString;
            }
            catch
            {
                return null;
            }
        }

        /// <versions>08-10-2017(v0.0.4.8)</versions>
        public AMFCMembers Get_List_Members_Joia_Not_Paid()
        {
            String sQueryString = String.Empty;
            try
            {
                String sDBF_Filename = LibAMFC.DBF_AMFC_SOCIO_FileName;
                AMFCMembers objListMembers = new AMFCMembers();
                Set_OLE_DB_ConnectionString();
                if (String.IsNullOrEmpty(sDBF_Filename))
                    OLE_DB_Settings();
                sQueryString = Set_Members_Joia_Not_Paid_Query();
                if (String.IsNullOrEmpty(sQueryString))
                {
                    StackFrame objStackFrame = new StackFrame();
                    String sErrorMsg = "Erro no SQL para ober a Lista de Sócios com joia não paga!" + " -> " + "QUERY: " + sQueryString;
                    Program.HandleError(objStackFrame.GetMethod().Name, sErrorMsg, Program.ErroType.ERROR, true, true);
                    objStackFrame = null;
                    return null;
                }
                using (OleDbConnection objOleDbConnection = new OleDbConnection(OLE_DB_ConnectionString))
                {
                    objOleDbConnection.Open();
                    using (OleDbCommand objOleDbCommand = new OleDbCommand())
                    {
                        objOleDbCommand.Connection = objOleDbConnection;
                        objOleDbCommand.CommandText = sQueryString;

                        objListMembers = Get_Find_Member_DB_Data(objOleDbCommand);
                        if (objListMembers == null || objListMembers.Members == null)
                        {
                            StackFrame objStackFrame = new StackFrame();
                            String sErrorMsg = "Não foi possível obter a Lista de Sócios da Base de Dados!" + " -> " + "QUERY: " + sQueryString;
                            Program.HandleError(objStackFrame.GetMethod().Name, sErrorMsg, Program.ErroType.ERROR, true, true);
                            objStackFrame = null;
                            return null;
                        }
                    }
                }

                return objListMembers;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message + " -> " + "QUERY: " + sQueryString, Program.ErroType.EXCEPTION, true, false);
                return null;
            }
        }

        /// <versions>25-11-2017(GesAMFC-v0.0.4.37)</versions>
        /// <remarks>
        /// ID,N,10,0	
        /// LISTACAIXA,C,20	
        /// LISTARECNU,C,20	
        /// SOCIO,N,5,0	JOIA,N,6,2	
        /// JOIA,N,6,2
        /// DATA,D	
        /// DATAPAG,D	
        /// DATAPAGREG,D	
        /// NOME,C,70	
        /// NOTAS,C,140	
        /// PAGO,N,6,2	
        /// FALTA,N,6,2
        /// EMPAG,N,6,2	
        /// ESTADO,C,1
        /// </remarks>
        public Int64 Add_Joia(AMFCMemberJoia objMemberJoia)
        {
            String sQueryString = String.Empty;
            try
            {
                String sDBF_File_Name = LibAMFC.DBF_AMFC_JOIAS_FileName;

                Set_OLE_DB_ConnectionString();
                if (String.IsNullOrEmpty(sDBF_File_Name))
                    OLE_DB_Settings();

                #region     Validate Data
                if (objMemberJoia.JoiaId < 1)
                {
                    String sError = "Joia ID inválido: " + objMemberJoia.MemberNumber;
                    MessageBox.Show(sError, "ID Inválido", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return -1;
                }
                if (objMemberJoia.MemberNumber < new AMFCMember().MinNumber || objMemberJoia.MemberNumber > new AMFCMember().MaxNumber)
                {
                    String sError = "Nº de sócio inválido: " + objMemberJoia.MemberNumber;
                    MessageBox.Show(sError, "Sócio Inválido", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return -1;
                }
                if (objMemberJoia.JoiaValue <= 0)
                {
                    String sError = "A joia tem de ter um valor monetário maior que zero!";
                    MessageBox.Show(sError, "Valor Joia Inválido", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return -1;
                }
                //.... verificar outros no futuro
                #endregion  Validate Data

                #region     Check if Member Joia already exist
                if (Member_Joia_Already_Exist(objMemberJoia))
                {
                    String sWarning = "Já foi adicionada a Joia do Sócio com o Nº: " + objMemberJoia.MemberNumber;
                    if (!String.IsNullOrEmpty(objMemberJoia.MemberName))
                        sWarning += " e Nome: " + objMemberJoia.MemberName + ")";
                    MessageBox.Show(sWarning, "Joia já existente", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return 0;
                }
                #endregion  Check if Member Joia already exist

                #region     Query
                sQueryString += "INSERT INTO " + sDBF_File_Name + " " + "\n";

                #region     Columns
                sQueryString += "( " + " " + "\n";
                sQueryString += "\t" + "ID" + "," + " " + "\n";
                sQueryString += "\t" + "SOCIO" + "," + " " + "\n";
                if (!String.IsNullOrEmpty(objMemberJoia.JoiaListaCaixa) && objMemberJoia.JoiaListaCaixa.Trim().ToLower() != Program.DB_Not_Available.Trim().ToLower())
                    sQueryString += "\t" + "LISTACAIXA" + "," + " " + "\n";
                if (!String.IsNullOrEmpty(objMemberJoia.JoiaListaRecibos) && objMemberJoia.JoiaListaRecibos.Trim().ToLower() != Program.DB_Not_Available.Trim().ToLower())
                    sQueryString += "\t" + "LISTARECNU" + "," + " " + "\n";
                if (objMemberJoia.JoiaValue > 0)
                    sQueryString += "\t" + "JOIA" + "," + " " + "\n";
                if (!String.IsNullOrEmpty(objMemberJoia.JoiaDate) && objMemberJoia.JoiaDate.Trim().ToLower() != Program.DB_Not_Available.Trim().ToLower())
                    sQueryString += "\t" + "DATA" + "," + " " + "\n";
                if (!String.IsNullOrEmpty(objMemberJoia.JoiaDatePaid) && objMemberJoia.JoiaDatePaid.Trim().ToLower() != Program.DB_Not_Available.Trim().ToLower())
                    sQueryString += "\t" + "DATAPAG" + "," + " " + "\n";
                if (!String.IsNullOrEmpty(objMemberJoia.JoiaDataPagamentoAgregado) && objMemberJoia.JoiaDataPagamentoAgregado.Trim().ToLower() != Program.DB_Not_Available.Trim().ToLower())
                    sQueryString += "\t" + "DATAPAGREG" + "," + " " + "\n";
                if (!String.IsNullOrEmpty(objMemberJoia.JoiaPaidPerson) && objMemberJoia.JoiaPaidPerson.Trim().ToLower() != Program.DB_Not_Available.Trim().ToLower())
                    sQueryString += "\t" + "NOME" + "," + " " + "\n";

                if (objMemberJoia.ValuePaid >= 0)
                    sQueryString += "\t" + "PAGO" + "," + " " + "\n";

                if (objMemberJoia.ValueMissing >= 0)
                    sQueryString += "\t" + "FALTA" + "," + " " + "\n";

                if (objMemberJoia.ValueOnPaying >= 0)
                    sQueryString += "\t" + "EMPAG" + "," + " " + "\n";

                if (objMemberJoia.Pay_State != AMFCMemberJoia.PayState.UNDEFINED)
                    sQueryString += "\t" + "ESTADO" + "," + " " + "\n";

                sQueryString += "\t" + "NOTAS" + " " + " " + "\n";
                sQueryString += ") " + " " + "\n";
                #endregion  Columns

                #region     Values
                sQueryString += "VALUES" + " " + "\n";
                sQueryString += "( " + " " + "\n";

                sQueryString += "\t" + objMemberJoia.JoiaId + "," + " " + "\n";

                sQueryString += "\t" + objMemberJoia.MemberNumber + "," + " " + "\n";

                if (!String.IsNullOrEmpty(objMemberJoia.JoiaListaCaixa) && objMemberJoia.JoiaListaCaixa.Trim().ToLower() != Program.DB_Not_Available.Trim().ToLower())
                    sQueryString += "\t" + "'" + objMemberJoia.JoiaListaCaixa.Trim().ToUpper() + "'" + "," + " " + "\n";

                if (!String.IsNullOrEmpty(objMemberJoia.JoiaListaRecibos) && objMemberJoia.JoiaListaRecibos.Trim().ToLower() != Program.DB_Not_Available.Trim().ToLower())
                    sQueryString += "\t" + "'" + objMemberJoia.JoiaListaRecibos.Trim().ToUpper() + "'" + "," + " " + "\n";

                if (objMemberJoia.JoiaValue > 0)
                    sQueryString += "\t" + "'" + Program.SetPayDoubleStringValue(objMemberJoia.JoiaValue) + "'" + "," + " " + "\n";

                if (!String.IsNullOrEmpty(objMemberJoia.JoiaDate) && objMemberJoia.JoiaDate.Trim().ToLower() != Program.DB_Not_Available.Trim().ToLower())
                    sQueryString += "\t" + "'" + objMemberJoia.JoiaDate.Trim() + "'" + "," + " " + "\n";

                if (!String.IsNullOrEmpty(objMemberJoia.JoiaDatePaid) && objMemberJoia.JoiaDatePaid.Trim().ToLower() != Program.DB_Not_Available.Trim().ToLower())
                    sQueryString += "\t" + "'" + objMemberJoia.JoiaDatePaid.Trim() + "'" + "," + " " + "\n";

                if (!String.IsNullOrEmpty(objMemberJoia.JoiaDataPagamentoAgregado) && objMemberJoia.JoiaDataPagamentoAgregado.Trim().ToLower() != Program.DB_Not_Available.Trim().ToLower())
                    sQueryString += "\t" + "'" + objMemberJoia.JoiaDataPagamentoAgregado.Trim() + "'" + "," + " " + "\n";

                if (!String.IsNullOrEmpty(objMemberJoia.JoiaPaidPerson) && objMemberJoia.JoiaPaidPerson.Trim().ToLower() != Program.DB_Not_Available.Trim().ToLower())
                    sQueryString += "\t" + "'" + objMemberJoia.JoiaPaidPerson.Trim().ToUpper() + "'" + "," + " " + "\n";

                if (objMemberJoia.ValuePaid >= 0)
                    sQueryString += "\t" + "'" + Program.SetPayDoubleStringValue(objMemberJoia.ValuePaid) + "'" + "," + " " + "\n";

                if (objMemberJoia.ValueMissing >= 0)
                    sQueryString += "\t" + "'" + Program.SetPayDoubleStringValue(objMemberJoia.ValueMissing) + "'" + "," + " " + "\n";

                if (objMemberJoia.ValueOnPaying >= 0)
                    sQueryString += "\t" + "'" + Program.SetPayDoubleStringValue(objMemberJoia.ValueOnPaying) + "'" + "," + " " + "\n";

                if (objMemberJoia.Pay_State != AMFCMemberJoia.PayState.UNDEFINED)
                    sQueryString += "\t" + "'" + objMemberJoia.Pay_State_DB_Value + "'" + "," + " " + "\n";

                sQueryString += "\t" + "'" + objMemberJoia.JoiaNotas.Trim() + "'" + " " + " " + "\n";

                sQueryString += ") " + " " + "\n";
                #endregion  Values

                #endregion  Query

                using (OleDbConnection objOleDbConnection = new OleDbConnection(OLE_DB_ConnectionString))
                {
                    objOleDbConnection.Open();
                    using (OleDbCommand objOleDbCommand = new OleDbCommand())
                    {
                        objOleDbCommand.Connection = objOleDbConnection;
                        objOleDbCommand.CommandText = sQueryString;
                        if (objOleDbCommand.ExecuteNonQuery() != 1)
                            return -1;
                    }
                }

                return 1;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message + " -> " + "QUERY: " + sQueryString, Program.ErroType.EXCEPTION, true, false);
                return -1;
            }
        }

        /// <versions>25-11-2017(GesAMFC-v0.0.4.37)</versions>
        /// <remarks>
        /// ID,N,10,0	
        /// LISTACAIXA,C,20	
        /// LISTARECNU,C,20	
        /// SOCIO,N,5,0	JOIA,N,6,2	
        /// JOIA,N,6,2
        /// DATA,D	
        /// DATAPAG,D	
        /// DATAPAGREG,D	
        /// NOME,C,70	
        /// NOTAS,C,140	
        /// PAGO,N,6,2	
        /// FALTA,N,6,2
        /// EMPAG,N,6,2	
        /// ESTADO,C,1
        /// </remarks>
        public Int64 Edit_Joia(AMFCMemberJoia objMemberJoia)
        {
            String sQueryString = String.Empty;
            try
            {
                String sDBF_File_Name = LibAMFC.DBF_AMFC_JOIAS_FileName;

                Set_OLE_DB_ConnectionString();
                if (String.IsNullOrEmpty(sDBF_File_Name))
                    OLE_DB_Settings();

                #region     Validate Data
                if (objMemberJoia.JoiaId < 1)
                {
                    String sError = "Joia ID inválido: " + objMemberJoia.MemberNumber;
                    MessageBox.Show(sError, "ID Inválido", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return -1;
                }
                if (objMemberJoia.MemberNumber < new AMFCMember().MinNumber || objMemberJoia.MemberNumber > new AMFCMember().MaxNumber)
                {
                    String sError = "Nº de sócio inválido: " + objMemberJoia.MemberNumber;
                    MessageBox.Show(sError, "Sócio Inválido", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return -1;
                }
                if (objMemberJoia.JoiaValue <= 0)
                {
                    String sError = "A joia tem de ter um valor monetário maior que zero!";
                    MessageBox.Show(sError, "Valor Joia Inválido", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return -1;
                }
                //.... verificar outros no futuro
                #endregion  Validate Data

                #region     Query
                sQueryString += "UPDATE " + sDBF_File_Name + " " + "\n";
                sQueryString += "SET" + " " + "\n";

                if (!String.IsNullOrEmpty(objMemberJoia.JoiaListaCaixa) && objMemberJoia.JoiaListaCaixa.Trim().ToLower() != Program.DB_Not_Available.Trim().ToLower())
                    sQueryString += "\t" + "LISTACAIXA" + " = " + "'" + objMemberJoia.JoiaListaCaixa.Trim().ToUpper() + "'" + "," + " " + "\n";
                if (!String.IsNullOrEmpty(objMemberJoia.JoiaListaRecibos) && objMemberJoia.JoiaListaRecibos.Trim().ToLower() != Program.DB_Not_Available.Trim().ToLower())
                    sQueryString += "\t" + "LISTARECNU" + " = " + "'" + objMemberJoia.JoiaListaRecibos.Trim().ToUpper() + "'" + "," + " " + "\n";
                if (objMemberJoia.JoiaValue > 0)
                    sQueryString += "\t" + "JOIA" + " = " + "'" + Program.SetPayDoubleStringValue(objMemberJoia.JoiaValue) + "'" + "," + " " + "\n";
                if (!String.IsNullOrEmpty(objMemberJoia.JoiaDate) && objMemberJoia.JoiaDate.Trim().ToLower() != Program.DB_Not_Available.Trim().ToLower())
                    sQueryString += "\t" + "DATA" + " = " + "'" + objMemberJoia.JoiaDate.Trim() + "'" + "," + " " + "\n";
                if (!String.IsNullOrEmpty(objMemberJoia.JoiaDatePaid) && objMemberJoia.JoiaDatePaid.Trim().ToLower() != Program.DB_Not_Available.Trim().ToLower())
                    sQueryString += "\t" + "DATAPAG" + " = " + "'" + objMemberJoia.JoiaDatePaid.Trim() + "'" + "," + " " + "\n";
                if (!String.IsNullOrEmpty(objMemberJoia.JoiaDataPagamentoAgregado) && objMemberJoia.JoiaDataPagamentoAgregado.Trim().ToLower() != Program.DB_Not_Available.Trim().ToLower())
                    sQueryString += "\t" + "DATAPAGREG" + " = " + "'" + objMemberJoia.JoiaDataPagamentoAgregado.Trim() + "'" + "," + " " + "\n";
                if (!String.IsNullOrEmpty(objMemberJoia.JoiaPaidPerson) && objMemberJoia.JoiaPaidPerson.Trim().ToLower() != Program.DB_Not_Available.Trim().ToLower())
                    sQueryString += "\t" + "NOME" + " = " + "'" + objMemberJoia.JoiaPaidPerson.Trim().ToUpper() + "'" + "," + " " + "\n";

                if (objMemberJoia.ValuePaid >= 0)
                    sQueryString += "\t" + "PAGO" + " = " + "'" + Program.SetPayDoubleStringValue(objMemberJoia.ValuePaid) + "'" + "," + " " + "\n";

                if (objMemberJoia.ValueMissing >= 0)
                    sQueryString += "\t" + "FALTA" + " = " + "'" + Program.SetPayDoubleStringValue(objMemberJoia.ValueMissing) + "'" + "," + " " + "\n";

                if (objMemberJoia.ValueOnPaying >= 0)
                    sQueryString += "\t" + "EMPAG" + " = " + "'" + Program.SetPayDoubleStringValue(objMemberJoia.ValueOnPaying) + "'" + "," + " " + "\n";

                if (objMemberJoia.Pay_State != AMFCMemberJoia.PayState.UNDEFINED)
                    sQueryString += "\t" + "ESTADO" + " = " + "'" + objMemberJoia.Pay_State_DB_Value + "'" + "," + " " + "\n";

                sQueryString += "\t" + "NOTAS" + " = " + "'" + objMemberJoia.JoiaNotas.Trim() + "'" + " " + " " + "\n";

                sQueryString += "WHERE " + "ID" + " = " + objMemberJoia.JoiaId +  "\n";
                #endregion  Query

                using (OleDbConnection objOleDbConnection = new OleDbConnection(OLE_DB_ConnectionString))
                {
                    objOleDbConnection.Open();
                    using (OleDbCommand objOleDbCommand = new OleDbCommand())
                    {
                        objOleDbCommand.Connection = objOleDbConnection;
                        objOleDbCommand.CommandText = sQueryString;
                        if (objOleDbCommand.ExecuteNonQuery() != 1)
                            return -1;
                    }
                }

                return 1;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message + " -> " + "QUERY: " + sQueryString, Program.ErroType.EXCEPTION, true, false);
                return -1;
            }
        }

        /// <versions>08-10-2017(v0.0.4.9)</versions>
        public Int64 Del_Joia(Int64 lJoiaId)
        {
            String sQueryString = String.Empty;
            try
            {
                String sDBF_File_Name = LibAMFC.DBF_AMFC_JOIAS_FileName;

                Set_OLE_DB_ConnectionString();
                if (String.IsNullOrEmpty(sDBF_File_Name))
                    OLE_DB_Settings();

                #region     Validate Data
                if (lJoiaId < 1)
                {
                    String sError = "Joia ID inválido: " + lJoiaId;
                    MessageBox.Show(sError, "ID Inválido", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return -1;
                }
                #endregion  Validate Data

                #region     Query
                sQueryString = "DELETE FROM " + sDBF_File_Name + " WHERE " + "ID" + " = " + lJoiaId + ";" + "\n";
                #endregion  Query

                using (OleDbConnection objOleDbConnection = new OleDbConnection(OLE_DB_ConnectionString))
                {
                    objOleDbConnection.Open();
                    using (OleDbCommand objOleDbCommand = new OleDbCommand())
                    {
                        objOleDbCommand.Connection = objOleDbConnection;
                        objOleDbCommand.CommandText = sQueryString;
                        if (objOleDbCommand.ExecuteNonQuery() != 1)
                            return -1;
                    }
                }

                return 1;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message + " -> " + "QUERY: " + sQueryString, Program.ErroType.EXCEPTION, true, false);
                return -1;
            }
        }

        #endregion  Admin Joias

        #region     Admin QUOTAS

        /// <versions>28-11-2017(GesAMFC-v0.0.4.39)</versions>
        public AMFC_Entities Get_All_Member_QUOTAS(Int32 iYear)
        {
            try
            {
                AMFC_Entities objEntities = new AMFC_Entities();
                Set_OLE_DB_ConnectionString();

                AMFCMember objMember = new AMFCMember();
                using (OleDbConnection objOleDbConnection = new OleDbConnection(OLE_DB_ConnectionString))
                {
                    objOleDbConnection.Open();
                    using (OleDbCommand objOleDbCommand = new OleDbCommand())
                    {
                        objOleDbCommand.Connection = objOleDbConnection;

                        objOleDbCommand.CommandText = Set_Member_QUOTAS_Query(-1, -1, iYear, false);
                        
                        objEntities = Get_Members_QUOTAS_DB_Data(objOleDbCommand, false);
                        if (objEntities == null || objEntities.Entidades == null)
                        {
                            StackFrame objStackFrame = new StackFrame();
                            String sErrorMsg = "Não foi possível obter a Lista de" + Program.Entity_QUOTA_Desc_Plural + " dos Sócios da Base de Dados!" + " -> " + "QUERY: " + objOleDbCommand.CommandText;
                            Program.HandleError(objStackFrame.GetMethod().Name, sErrorMsg, Program.ErroType.ERROR, true, true);
                            objStackFrame = null;
                            return null;
                        }
                    }
                }

                return objEntities;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return null;
            }
        }

        /// <versions>30-11-2017(GesAMFC-v0.0.4.40)</versions>
        public AMFC_Entities Get_Member_QUOTAS_ByNbr(Int64 lMemberNumber, Int32 iYear)
        {
            try
            {
                AMFC_Entities objEntities = new AMFC_Entities();

                Set_OLE_DB_ConnectionString();

                using (OleDbConnection objOleDbConnection = new OleDbConnection(OLE_DB_ConnectionString))
                {
                    objOleDbConnection.Open();
                    using (OleDbCommand objOleDbCommand = new OleDbCommand())
                    {
                        objOleDbCommand.Connection = objOleDbConnection;
                        objOleDbCommand.CommandText = Set_Member_QUOTAS_Query(lMemberNumber, -1, iYear, false);

                        objEntities = Get_Members_QUOTAS_DB_Data(objOleDbCommand, false);
                        if (objEntities == null || objEntities.Entidades == null)
                        {
                            StackFrame objStackFrame = new StackFrame();
                            String sErrorMsg = "Não foi possível obter o " + "Pagamento" + Program.Entity_QUOTA_Desc_Plural + " do Sócio com o Nº: " + lMemberNumber + " -> " + "QUERY: " + objOleDbCommand.CommandText;
                            Program.HandleError(objStackFrame.GetMethod().Name, sErrorMsg, Program.ErroType.ERROR, true, true);
                            objStackFrame = null;
                            return null;
                        }
                    }
                }

                return objEntities;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return null;
            }
        }

        /// <versions>01-12-2017(GesAMFC-v0.0.4.41)</versions>
        public AMFC_Entities Get_Members_QUOTAS_ByYear(Int32 iYear)
        {
            try
            {
                AMFC_Entities objEntities = new AMFC_Entities();

                Set_OLE_DB_ConnectionString();

                using (OleDbConnection objOleDbConnection = new OleDbConnection(OLE_DB_ConnectionString))
                {
                    objOleDbConnection.Open();
                    using (OleDbCommand objOleDbCommand = new OleDbCommand())
                    {
                        objOleDbCommand.Connection = objOleDbConnection;
                        objOleDbCommand.CommandText = Set_Member_QUOTAS_Query(-1, -1, iYear, false);

                        objEntities = Get_Members_QUOTAS_DB_Data(objOleDbCommand, false);
                        if (objEntities == null || objEntities.Entidades == null)
                        {
                            StackFrame objStackFrame = new StackFrame();
                            String sErrorMsg = "Não foi possível obter " + "Pagamento" + Program.Entity_QUOTA_Desc_Plural + " do Ano: " + iYear + " -> " + "QUERY: " + objOleDbCommand.CommandText;
                            Program.HandleError(objStackFrame.GetMethod().Name, sErrorMsg, Program.ErroType.ERROR, true, true);
                            objStackFrame = null;
                            return null;
                        }
                    }
                }

                return objEntities;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return null;
            }
        }

        /// <versions>28-11-2017(GesAMFC-v0.0.4.39)</versions>
        public AMFC_Entity Get_Member_QUOTA_ById(Int64 lId, Int32 iYear)
        {
            try
            {
                AMFC_Entity objEntity = new AMFC_Entity();
                Set_OLE_DB_ConnectionString();

                using (OleDbConnection objOleDbConnection = new OleDbConnection(OLE_DB_ConnectionString))
                {
                    objOleDbConnection.Open();
                    using (OleDbCommand objOleDbCommand = new OleDbCommand())
                    {
                        objOleDbCommand.Connection = objOleDbConnection;
                        objOleDbCommand.CommandText = Set_Member_QUOTAS_Query(-1, lId, -1, true);

                        AMFC_Entities objEntities = Get_Members_QUOTAS_DB_Data(objOleDbCommand, true);
                        if (objEntities == null || objEntities.Entidades == null || objEntities.Entidades.Count > 1)
                        {
                            StackFrame objStackFrame = new StackFrame();
                            String sErrorMsg = "Não foi possível obter a " + Program.Entity_QUOTA_Desc_Single + " com o ID: " + lId + " -> " + "QUERY: " + objOleDbCommand.CommandText;
                            Program.HandleError(objStackFrame.GetMethod().Name, sErrorMsg, Program.ErroType.ERROR, true, true);
                            objStackFrame = null;
                            return null;
                        }
                        if (objEntities.Entidades.Count == 1)
                            objEntity = objEntities.Entidades[0];
                    }
                }

                return objEntity;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return null;
            }
        }

        /// <versions>28-11-2017(GesAMFC-v0.0.4.39)</versions>
        public String Set_Member_QUOTAS_Query(Int64 lMemberNumber, Int64 lQuotaId, Int32 iYear, Boolean bGetSingleRecord)
        {
            try
            {
                String sQueryString = String.Empty;

                String sDBF_FileName = Program.GetEntityDbfFileName(AMFC_Entities.AMFC_Entidade_Tipo.QUOTAS);

                sQueryString += "SELECT" + " " + "\n";
                sQueryString += sDBF_FileName     + "." + "ID"            + " AS " + "ID"             + "," + " " + "\n"; 
                sQueryString += sDBF_FileName     + "." + "LISTACAIXA"    + " AS " + "ListaCaixa"     + "," + " " + "\n"; 
                sQueryString += sDBF_FileName     + "." + "LISTARECNU"    + " AS " + "LISTARECNU"     + "," + " " + "\n"; 
                sQueryString += LibAMFC.DBF_AMFC_SOCIO_FileName     + "." + "NUMERO"        + " AS " + "SOCIONUMERO"    + "," + " " + "\n";
                sQueryString += LibAMFC.DBF_AMFC_SOCIO_FileName     + "." + "NOME"          + " AS " + "SOCIONOME"      + "," + " " + "\n";
                sQueryString += LibAMFC.DBF_AMFC_SOCIO_FileName + "." + "AREALOTE" + " AS " + "AREALOTE" + "," + " " + "\n";
                sQueryString += sDBF_FileName     + "." + "NOME"          + " AS " + "PAGOUNOME"      + "," + " " + "\n"; 
                sQueryString += LibAMFC.DBF_AMFC_SOCIO_FileName     + "." + "DATAADMI"      + " AS " + "DATAADMI"       + "," + " " + "\n";
                sQueryString += sDBF_FileName     + "." + "VALOR"         + " AS " + "VALOR"          + "," + " " + "\n";
                sQueryString += sDBF_FileName     + "." + "PAGO"          + " AS " + "PAGO"           + "," + " " + "\n";
                sQueryString += sDBF_FileName     + "." + "FALTA"         + " AS " + "FALTA"          + "," + " " + "\n";
                sQueryString += sDBF_FileName     + "." + "EMPAG"         + " AS " + "EMPAG"          + "," + " " + "\n";
                sQueryString += sDBF_FileName     + "." + "ANO"           + " AS " + "ANO"            + "," + " " + "\n";
                sQueryString += sDBF_FileName     + "." + "DATAPAG"       + " AS " + "DATAPAG"        + "," + " " + "\n";
                sQueryString += sDBF_FileName     + "." + "DATAPAGREG"    + " AS " + "DATAPAGREG"     + "," + " " + "\n"; 
                sQueryString += sDBF_FileName     + "." + "ESTADO"        + " AS " + "ESTADO"         + "," + " " + "\n";
                sQueryString += sDBF_FileName     + "." + "NOTAS"         + " AS " + "NOTAS"          + " " + " " + "\n"; 

                sQueryString += "FROM"  + " " + "\n";
                sQueryString += "\t" + LibAMFC.DBF_AMFC_SOCIO_FileName + " " + "\n";

                //if (bGetSingleRecord)
                    sQueryString += "INNER" + " " + "JOIN" + " " + "\n";
                //else
                //sQueryString += "LEFT"  + " " + "JOIN" + " " + "\n";

                sQueryString += "\t" + sDBF_FileName  + " " + "\n";
                sQueryString += "ON " + " " + LibAMFC.DBF_AMFC_SOCIO_FileName + "." + new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMERO) + " = " + sDBF_FileName  + "." + "SOCIO" + " " + "\n";

                if (lMemberNumber >= new AMFCMember().MinNumber && lMemberNumber <= new AMFCMember().MaxNumber)
                {
                    sQueryString += "WHERE" + " " + "\n";
                    sQueryString += LibAMFC.DBF_AMFC_SOCIO_FileName + "." + new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMERO) + " = " + lMemberNumber + "\n";
                    sQueryString += "AND" + " " + "\n";
                    sQueryString += sDBF_FileName + "." + "ID" + " " + "IS NOT NULL" + " " + "\n";
                }
                else if (Program.IsValidYear(iYear))
                {
                    sQueryString += "WHERE" + " " + "\n";
                    sQueryString += "ANO" + " = " + iYear + "\n";
                }
                else if (bGetSingleRecord && lQuotaId > 0)
                {
                    sQueryString += "WHERE" + " " + "\n";
                    sQueryString += sDBF_FileName  + "." + "ID" + " = " + lQuotaId + "\n";
                }
                else
                {
                    sQueryString += "ORDER BY" + " " + "\n";
                    sQueryString += sDBF_FileName + "." + "ANO" + ", " + " " + "\n";
                    sQueryString += LibAMFC.DBF_AMFC_SOCIO_FileName + "." + "NUMERO" + " " + " " + "\n";
                }
                sQueryString += ";";

                return sQueryString;
            }
            catch
            {
                return null;
            }
        }

        /// <versions>28-11-2017(GesAMFC-v0.0.4.39)</versions>
        public String Set_QUOTAS_Pay_Open(Int64 lMemberNumber, Int32 iYear)
        {
            try
            {
                if (lMemberNumber < new AMFCMember().MinNumber || lMemberNumber > new AMFCMember().MaxNumber)
                    return String.Empty;

                String sQueryString = String.Empty;

                String sDBF_FileName = Program.GetEntityDbfFileName(AMFC_Entities.AMFC_Entidade_Tipo.QUOTAS);

                sQueryString += "SELECT" + " " + "\n";
                sQueryString += sDBF_FileName     + "." + "ID"            + " AS " + "ID"             + "," + " " + "\n";
                sQueryString += sDBF_FileName     + "." + "LISTACAIXA"    + " AS " + "LISTACAIXA"     + "," + " " + "\n";
                sQueryString += sDBF_FileName     + "." + "LISTARECNU"    + " AS " + "LISTARECNU"     + "," + " " + "\n";
                sQueryString += LibAMFC.DBF_AMFC_SOCIO_FileName     + "." + "NUMERO"        + " AS " + "SOCIONUMERO"    + "," + " " + "\n";
                sQueryString += LibAMFC.DBF_AMFC_SOCIO_FileName     + "." + "NOME"          + " AS " + "SOCIONOME"      + "," + " " + "\n";
                sQueryString += LibAMFC.DBF_AMFC_SOCIO_FileName + "." + "AREALOTE" + " AS " + "AREALOTE" + "," + " " + "\n";
                sQueryString += sDBF_FileName     + "." + "NOME"          + " AS " + "PAGOUNOME"      + "," + " " + "\n";
                sQueryString += LibAMFC.DBF_AMFC_SOCIO_FileName     + "." + "DATAADMI"      + " AS " + "DATAADMI"       + "," + " " + "\n";
                sQueryString += sDBF_FileName     + "." + "VALOR"         + " AS " + "VALOR"          + "," + " " + "\n";
                sQueryString += sDBF_FileName     + "." + "PAGO"          + " AS " + "PAGO"           + "," + " " + "\n";
                sQueryString += sDBF_FileName     + "." + "FALTA"         + " AS " + "FALTA"          + "," + " " + "\n";
                sQueryString += sDBF_FileName     + "." + "EMPAG"         + " AS " + "EMPAG"          + "," + " " + "\n";
                sQueryString += sDBF_FileName     + "." + "ANO"           + " AS " + "ANO"            + "," + " " + "\n";
                sQueryString += sDBF_FileName     + "." + "DATAPAG"       + " AS " + "DATAPAG"        + "," + " " + "\n";
                sQueryString += sDBF_FileName     + "." + "DATAPAGREG"    + " AS " + "DATAPAGREG"     + "," + " " + "\n";
                sQueryString += sDBF_FileName     + "." + "ESTADO"        + " AS " + "ESTADO"         + "," + " " + "\n";
                sQueryString += sDBF_FileName     + "." + "NOTAS"         + " AS " + "NOTAS"          + " " + " " + "\n";
                sQueryString += "FROM"  + " " + "\n";
                sQueryString += "\t" + LibAMFC.DBF_AMFC_SOCIO_FileName  + " " + "\n";
                
                //sQueryString += "LEFT"  + " " + "JOIN" + " " + "\n";
                sQueryString += "INNER" + " " + "JOIN" + " " + "\n";

                sQueryString += "\t" + sDBF_FileName  + " " + "\n";
                sQueryString += "ON "   + " " + LibAMFC.DBF_AMFC_SOCIO_FileName + "." + new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMERO) + " = " + sDBF_FileName  + "." + "SOCIO" + " " + "\n";
                if (lMemberNumber >= new AMFCMember().MinNumber && lMemberNumber <= new AMFCMember().MaxNumber)
                {
                    sQueryString += "WHERE" + " " + "\n";
                    sQueryString += LibAMFC.DBF_AMFC_SOCIO_FileName + "." + new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMERO) + " = " + lMemberNumber + "\n";
                    sQueryString += "AND" + " " + "\n";
                    sQueryString += sDBF_FileName  + "." + "ESTADO" + " LIKE " + "'" + new AMFC_Entity().GetPay_StateDBvalue(AMFC_Entity.PayState.EM_PAGAMENTO) + "'" + " " + "\n";
                }
                else
                {
                    sQueryString += "ORDER BY" + " " + "\n";
                    sQueryString += sDBF_FileName + "." + "ANO"       + "," + " " + "\n";
                    sQueryString += LibAMFC.DBF_AMFC_SOCIO_FileName + "." + "NUMERO"    + " " + " " + "\n";
                }
                sQueryString += ";";

                return sQueryString;
            }
            catch
            {
                return null;
            }
        }

        /// <versions>28-11-2017(GesAMFC-v0.0.4.39)</versions>
        public AMFC_Entities Get_QUOTAS_Pay_Open(Int64 lMemberNumber, Int32 iYear)
        {
            try
            {
                AMFC_Entities objEntities = new AMFC_Entities();
                Set_OLE_DB_ConnectionString();

                AMFCMember objMember = new AMFCMember();
                using (OleDbConnection objOleDbConnection = new OleDbConnection(OLE_DB_ConnectionString))
                {
                    objOleDbConnection.Open();
                    using (OleDbCommand objOleDbCommand = new OleDbCommand())
                    {
                        objOleDbCommand.Connection = objOleDbConnection;
                        objOleDbCommand.CommandText = Set_QUOTAS_Pay_Open(lMemberNumber, iYear);

                        objEntities = Get_Members_QUOTAS_DB_Data(objOleDbCommand, false);
                        if (objEntities == null || objEntities.Entidades == null)
                        {
                            StackFrame objStackFrame = new StackFrame();
                            String sErrorMsg = "Não foi possível obter a Lista de" + Program.Entity_QUOTA_Desc_Plural + " dos Sócios da Base de Dados!" + " -> " + "QUERY: " + objOleDbCommand.CommandText;
                            Program.HandleError(objStackFrame.GetMethod().Name, sErrorMsg, Program.ErroType.ERROR, true, true);
                            objStackFrame = null;
                            return null;
                        }
                    }
                }

                return objEntities;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return null;
            }
        }

        /// <versions>30-11-2017(GesAMFC-v0.0.4.40)</versions>
        private AMFC_Entities Get_Members_QUOTAS_DB_Data(OleDbCommand objOleDbCommand, Boolean bGetSingleRecord)
        {
            try
            {
                AMFC_Entities objEntities = new AMFC_Entities();

                List<Int64> ListDuplicatedToRemove = new List<Int64>();

                using (OleDbDataReader objOleDbDataReader = objOleDbCommand.ExecuteReader())
                {
                    while (objOleDbDataReader.Read())
                    {
                        
                        AMFC_Entity objQuota = new AMFC_Entity();

                        if (objOleDbDataReader["ID"] == DBNull.Value)
                            continue;
                        if (objOleDbDataReader["SOCIONUMERO"] == DBNull.Value)
                            continue;
                        if (objOleDbDataReader["SOCIONOME"] == DBNull.Value)
                            continue;

                        objQuota.Id = Convert.ToInt64(objOleDbDataReader["ID"]);
                        objQuota.MemberNumber = Convert.ToInt64(objOleDbDataReader["SOCIONUMERO"]);
                        objQuota.MemberName = Program.EncodeStringToISO(objOleDbDataReader["SOCIONOME"].ToString());

                        if (objOleDbDataReader["LISTACAIXA"] != DBNull.Value)
                            objQuota.ListaCaixa = Program.SetTextString(Convert.ToString(objOleDbDataReader["LISTACAIXA"]));

                        if (objOleDbDataReader["LISTARECNU"] != DBNull.Value)
                            objQuota.ListaRecibos = Program.SetTextString(Convert.ToString(objOleDbDataReader["LISTARECNU"]));

                        if (objOleDbDataReader["DATAADMI"] != DBNull.Value)
                            objQuota.DtMemberAdmiDate = Program.SetDateTimeValue(Program.ConvertToValidDateTime(objOleDbDataReader["DATAADMI"].ToString().Trim()), -1, -1);

                        if (objOleDbDataReader["VALOR"] != DBNull.Value)
                            objQuota.Value = Program.SetPayCurrencyEuroDoubleValue(Convert.ToDouble(objOleDbDataReader["VALOR"].ToString().Trim()));

                        if (objOleDbDataReader["PAGO"] != DBNull.Value)
                            objQuota.ValuePaid = Program.SetPayCurrencyEuroDoubleValue(Convert.ToDouble(objOleDbDataReader["PAGO"].ToString().Trim()));

                        if (objOleDbDataReader["FALTA"] != DBNull.Value)
                            objQuota.ValueMissing = Program.SetPayCurrencyEuroDoubleValue(Convert.ToDouble(objOleDbDataReader["FALTA"].ToString().Trim()));

                        if (objOleDbDataReader["EMPAG"] != DBNull.Value)
                            objQuota.ValueOnPaying = Program.SetPayCurrencyEuroDoubleValue(Convert.ToDouble(objOleDbDataReader["EMPAG"].ToString().Trim()));

                        if (objOleDbDataReader["ESTADO"] != DBNull.Value)
                            objQuota.Pay_State_DB_Value = objOleDbDataReader["ESTADO"].ToString().Trim();

                        //#region debug
                        //#if DEBUG
                        //int i = 0;
                        //if (objQuota.Pay_State_DB_Value == "E")
                        //    i = 1;
                        //#endif
                        //#endregion debug

                            //objQuota.Pay_State = objQuota.GetPayStateTypeByInitial(objQuota.Pay_State_DB_Value);


                        if (objOleDbDataReader["ANO"] != DBNull.Value)
                            objQuota.YearInt = Convert.ToInt32(objOleDbDataReader["ANO"].ToString().Trim());

                        objQuota.DtDate = new DateTime(objQuota.YearInt, 1, 1);

                        if (objQuota.Value > 0)
                        {
                            if (objOleDbDataReader["DATAPAG"] != DBNull.Value)
                            {
                                if (objOleDbDataReader["PAGOUNOME"] != DBNull.Value)
                                    objQuota.PaidPerson = Convert.ToString(objOleDbDataReader["PAGOUNOME"]);

                                if (objOleDbDataReader["DATAPAGREG"] != DBNull.Value)
                                    objQuota.DtDataPagamentoAgregado = Program.ConvertToValidDateTime(objOleDbDataReader["DATAPAGREG"].ToString().Trim());

                                objQuota.DtDatePaid = Program.SetDateTimeValue(Program.ConvertToValidDateTime(objOleDbDataReader["DATAPAG"].ToString().Trim()), -1, -1);
                            }
                        }

                        if (objOleDbDataReader["NOTAS"] != DBNull.Value)
                            objQuota.Notas = Program.SetTextString(Convert.ToString(objOleDbDataReader["NOTAS"]));

                        if (objEntities.Contains(objQuota))
                        {
                            Int32 iRecordIdx = objEntities.GetEntidadeIndex(objQuota);
                            if (iRecordIdx > -1)
                            {
                                if (objQuota.Value > 0 && objEntities.Entidades[iRecordIdx].Value <= 0)
                                {
                                    ListDuplicatedToRemove.Add(iRecordIdx);
                                    objEntities.Entidades.Add(objQuota);
                                }
                            }
                        }
                        else
                            objEntities.Add(objQuota);

                        if (bGetSingleRecord)
                            return objEntities;
                    }
                }

                #region     Remove Duplicated Regists From Collection
                if (ListDuplicatedToRemove.Count > 0)
                    objEntities.Entidades.RemoveAll(j => ListDuplicatedToRemove.Contains(j.Idx));
                #endregion  Remove Duplicated Regists From Collection

                return objEntities;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return null;
            }
        }

        /// <versions>28-11-2017(GesAMFC-v0.0.4.39)</versions>
        public Boolean Member_QUOTA_Already_Exist(AMFC_Entity objEntity)
        {
            String sQueryString = String.Empty;
            try
            {
                String sDBF_File_Name = Program.GetEntityDbfFileName(AMFC_Entities.AMFC_Entidade_Tipo.QUOTAS);

                Set_OLE_DB_ConnectionString();
                if (String.IsNullOrEmpty(sDBF_File_Name))
                    OLE_DB_Settings();

                #region     Query
                sQueryString += "SELECT COUNT(*) FROM " + sDBF_File_Name + " " + "\n";
                sQueryString += "WHERE" + " " + "\n";
                sQueryString += "\t" + "SOCIO" + " = " + objEntity.MemberNumber + " " + "\n";
                sQueryString += "AND" + " " + "\n";
                sQueryString += "\t" + "ANO" + " = " + objEntity.YearInt + " " + "\n";
                sQueryString += "; ";
                #endregion  Query

                using (OleDbConnection objOleDbConnection = new OleDbConnection(OLE_DB_ConnectionString))
                {
                    objOleDbConnection.Open();
                    using (OleDbCommand objOleDbCommand = new OleDbCommand())
                    {
                        objOleDbCommand.Connection = objOleDbConnection;
                        objOleDbCommand.CommandText = sQueryString;
                        if (Convert.ToInt32(objOleDbCommand.ExecuteScalar()) > 0)
                            return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message + " -> " + "QUERY: " + sQueryString, Program.ErroType.EXCEPTION, true, false);
                return true;
            }
        }

        /// <versions>28-11-2017(GesAMFC-v0.0.4.39)</versions>
        public Int32 Get_QUOTA_Max_Number()
        {
            String sQueryString = String.Empty;
            try
            {
                Int32 iMaxQuota = -1;

                String sDBf_Filename = Program.GetEntityDbfFileName(AMFC_Entities.AMFC_Entidade_Tipo.QUOTAS);

                Set_OLE_DB_ConnectionString();
                if (String.IsNullOrEmpty(sDBf_Filename))
                    OLE_DB_Settings();

                using (OleDbConnection objOleDbConnection = new OleDbConnection(OLE_DB_ConnectionString))
                {
                    objOleDbConnection.Open();
                    using (OleDbCommand objOleDbCommand = new OleDbCommand())
                    {
                        objOleDbCommand.Connection = objOleDbConnection;

                        objOleDbCommand.CommandText = "SELECT COUNT(*) FROM " + sDBf_Filename + " " + "\n";
                        if (Convert.ToInt32(objOleDbCommand.ExecuteScalar()) <= 0)
                            return 0;

                        objOleDbCommand.CommandText = "SELECT MAX(" + "ID" + ") AS " + "MaxId" + " FROM " + sDBf_Filename + ";";
                        iMaxQuota = Convert.ToInt32(objOleDbCommand.ExecuteScalar());
                    }
                }
                return iMaxQuota;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message + " -> " + "QUERY: " + sQueryString, Program.ErroType.EXCEPTION, true, false);
                return -1;
            }
        }

        /// <versions>28-11-2017(GesAMFC-v0.0.4.39)</versions>
        public String Set_Members_QUOTA_Not_Paid_Query(Int32 iYear)
        {
            try
            {
                String sQueryString = String.Empty;

                String sDBf_Filename = Program.GetEntityDbfFileName(AMFC_Entities.AMFC_Entidade_Tipo.QUOTAS);

                sQueryString += "SELECT" + " " + "\n";
                sQueryString += sDBf_Filename     + "." + "ID"        + " AS " + "ID"             + "," + " " + "\n";
                sQueryString += LibAMFC.DBF_AMFC_SOCIO_FileName     + "." + "NUMERO"    + " AS " + "SOCIONUMERO"    + "," + " " + "\n";
                sQueryString += LibAMFC.DBF_AMFC_SOCIO_FileName     + "." + "NOME"      + " AS " + "SOCIONOME"      + "," + " " + "\n";
                sQueryString += LibAMFC.DBF_AMFC_SOCIO_FileName + "." + "AREALOTE" + " AS " + "AREALOTE" + "," + " " + "\n";
                sQueryString += LibAMFC.DBF_AMFC_SOCIO_FileName     + "." + "DATAADMI"  + " AS " + "DATAADMI"       + " " + " " + "\n";
                sQueryString += "FROM"  + " " + "\n";
                sQueryString += "\t" + LibAMFC.DBF_AMFC_SOCIO_FileName + " " + "\n";
                
                //sQueryString += "LEFT"  + " " + "JOIN" + " " + "\n";
                sQueryString += "INNER" + " " + "JOIN" + " " + "\n";

                sQueryString += "\t" + sDBf_Filename  + " " + "\n";
                sQueryString += "ON "   + " " + LibAMFC.DBF_AMFC_SOCIO_FileName + "." + new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMERO) + " = " + sDBf_Filename  + "." + "SOCIO" + " " + "\n";
                sQueryString += "WHERE" + " " + "\n";
                sQueryString += sDBf_Filename  + "." + "ID" + " IS NULL " + "\n";
                sQueryString += "ORDER BY" + " " + "\n";
                sQueryString += LibAMFC.DBF_AMFC_SOCIO_FileName + "." + new AMFCMember().Get_DBFMemberField_Name_ByType(GesAMFC.AMFCMember.DBFMemberFields.NUMERO) + " " + "\n";
                sQueryString += ";";

                return sQueryString;
            }
            catch
            {
                return null;
            }
        }

        /// <versions>28-11-2017(GesAMFC-v0.0.4.39)</versions>
        public AMFCMembers Get_List_Members_QUOTA_Not_Paid(Int32 iYear)
        {
            String sQueryString = String.Empty;
            try
            {
                String sDBF_Filename = LibAMFC.DBF_AMFC_SOCIO_FileName;
                AMFCMembers objListMembers = new AMFCMembers();
                Set_OLE_DB_ConnectionString();
                if (String.IsNullOrEmpty(sDBF_Filename))
                    OLE_DB_Settings();
                sQueryString = Set_Members_QUOTA_Not_Paid_Query(iYear);
                if (String.IsNullOrEmpty(sQueryString))
                {
                    StackFrame objStackFrame = new StackFrame();
                    String sErrorMsg = "Erro no SQL para ober a Lista de Sócios com ENTID não paga!" + " -> " + "QUERY: " + sQueryString;
                    Program.HandleError(objStackFrame.GetMethod().Name, sErrorMsg, Program.ErroType.ERROR, true, true);
                    objStackFrame = null;
                    return null;
                }
                using (OleDbConnection objOleDbConnection = new OleDbConnection(OLE_DB_ConnectionString))
                {
                    objOleDbConnection.Open();
                    using (OleDbCommand objOleDbCommand = new OleDbCommand())
                    {
                        objOleDbCommand.Connection = objOleDbConnection;
                        objOleDbCommand.CommandText = sQueryString;

                        objListMembers = Get_Find_Member_DB_Data(objOleDbCommand);
                        if (objListMembers == null || objListMembers.Members == null)
                        {
                            StackFrame objStackFrame = new StackFrame();
                            String sErrorMsg = "Não foi possível obter a Lista de Sócios da Base de Dados!" + " -> " + "QUERY: " + sQueryString;
                            Program.HandleError(objStackFrame.GetMethod().Name, sErrorMsg, Program.ErroType.ERROR, true, true);
                            objStackFrame = null;
                            return null;
                        }
                    }
                }

                return objListMembers;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message + " -> " + "QUERY: " + sQueryString, Program.ErroType.EXCEPTION, true, false);
                return null;
            }
        }

        /// <versions>28-11-2017(GesAMFC-v0.0.4.39)</versions>
        public Int64 Add_QUOTA(AMFC_Entity objEntity, Boolean bShowMessage)
        {
            String sQueryString = String.Empty;
            try
            {
                String sDBF_File_Name = Program.GetEntityDbfFileName(AMFC_Entities.AMFC_Entidade_Tipo.QUOTAS);

                Set_OLE_DB_ConnectionString();
                if (String.IsNullOrEmpty(sDBF_File_Name))
                    OLE_DB_Settings();

                #region     Validate Data
                if (objEntity.Id < 1)
                {
                    if (bShowMessage)
                    {
                        String sError = "Pagamento" + " " + "ID" + " " + "inválido" + ": " + objEntity.Id;
                        MessageBox.Show(sError, "ID Inválido", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    return -1;
                }
                if (objEntity.MemberNumber < new AMFCMember().MinNumber || objEntity.MemberNumber > new AMFCMember().MaxNumber)
                {
                    if (bShowMessage)
                    {
                        String sError = "Nº de sócio inválido: " + objEntity.MemberNumber;
                        MessageBox.Show(sError, "Sócio Inválido", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    return -1;
                }
                if (objEntity.Value <= 0)
                {
                    if (bShowMessage)
                    {
                        String sError = "Pagamento" + " de " + Program.Entity_QUOTA_Desc_Plural + " " + "tem de ter um valor monetário maior que zero!";
                        MessageBox.Show(sError, "Valor Inválido", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    return -1;
                }
                //.... verificar outros no futuro
                #endregion  Validate Data

                #region     Check if Member " + Program.Entity_QUOTA_Desc + " already exist
                if (Member_QUOTA_Already_Exist(objEntity))
                {
                    if (bShowMessage)
                    {
                        String sWarning = "Já foi adicionada a " + Program.Entity_QUOTA_Desc_Single + " " + objEntity.Year + " " + "do Sócio com o Nº: " + objEntity.MemberNumber;
                        if (!String.IsNullOrEmpty(objEntity.MemberName))
                            sWarning += " e Nome: " + objEntity.MemberName + "";
                        MessageBox.Show(sWarning, Program.Entity_QUOTA_Desc_Single + " " + "já existente!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    return 0;
                }
                    #endregion  Check if Member already exist

                    #region     Query
                sQueryString += "INSERT INTO " + sDBF_File_Name + " " + "\n";

                #region     Columns
                sQueryString += "( " + " " + "\n";
                sQueryString += "\t" + "ID" + "," + " " + "\n";
                sQueryString += "\t" + "SOCIO" + "," + " " + "\n";

                if (!String.IsNullOrEmpty(objEntity.ListaCaixa) && objEntity.ListaCaixa.Trim().ToLower() != Program.DB_Not_Available.Trim().ToLower())
                    sQueryString += "\t" + "LISTACAIXA" + "," + " " + "\n";

                if (!String.IsNullOrEmpty(objEntity.ListaRecibos) && objEntity.ListaRecibos.Trim().ToLower() != Program.DB_Not_Available.Trim().ToLower())
                    sQueryString += "\t" + "LISTARECNU" + "," + " " + "\n";

                if (!String.IsNullOrEmpty(objEntity.PaidPerson) && objEntity.PaidPerson.Trim().ToLower() != Program.DB_Not_Available.Trim().ToLower())
                    sQueryString += "\t" + "NOME" + "," + " " + "\n";

                if (Program.IsValidYear(objEntity.YearInt))
                    sQueryString += "\t" + "ANO" + "," + " " + "\n";

                if (!String.IsNullOrEmpty(objEntity.DatePaid) && objEntity.DatePaid.Trim().ToLower() != Program.DB_Not_Available.Trim().ToLower())
                    sQueryString += "\t" + "DATAPAG" + "," + " " + "\n";

                if (!String.IsNullOrEmpty(objEntity.DataPagamentoAgregado) && objEntity.DataPagamentoAgregado.Trim().ToLower() != Program.DB_Not_Available.Trim().ToLower())
                    sQueryString += "\t" + "DATAPAGREG" + "," + " " + "\n";
                
                if (objEntity.Value > 0)
                    sQueryString += "\t" + "VALOR" + "," + " " + "\n";

                if (objEntity.ValuePaid >= 0)
                    sQueryString += "\t" + "PAGO" + "," + " " + "\n";

                if (objEntity.ValueMissing >= 0)
                    sQueryString += "\t" + "FALTA" + "," + " " + "\n";

                if (objEntity.ValueOnPaying >= 0)
                    sQueryString += "\t" + "EMPAG" + "," + " " + "\n";

                if (objEntity.Pay_State != AMFC_Entity.PayState.UNDEFINED)
                    sQueryString += "\t" + "ESTADO" + "," + " " + "\n";

                sQueryString += "\t" + "NOTAS" + " " + " " + "\n";
                sQueryString += ") " + " " + "\n";
                #endregion  Columns

                #region     Values
                sQueryString += "VALUES" + " " + "\n";
                sQueryString += "( " + " " + "\n";

                sQueryString += "\t" + objEntity.Id + "," + " " + "\n";

                sQueryString += "\t" + objEntity.MemberNumber + "," + " " + "\n";

                if (!String.IsNullOrEmpty(objEntity.ListaCaixa) && objEntity.ListaCaixa.Trim().ToLower() != Program.DB_Not_Available.Trim().ToLower())
                    sQueryString += "\t" + "'" + objEntity.ListaCaixa.Trim().ToUpper() + "'" + "," + " " + "\n";

                if (!String.IsNullOrEmpty(objEntity.ListaRecibos) && objEntity.ListaRecibos.Trim().ToLower() != Program.DB_Not_Available.Trim().ToLower())
                    sQueryString += "\t" + "'" + objEntity.ListaRecibos.Trim().ToUpper() + "'" + "," + " " + "\n";

                if (!String.IsNullOrEmpty(objEntity.PaidPerson) && objEntity.PaidPerson.Trim().ToLower() != Program.DB_Not_Available.Trim().ToLower())
                    sQueryString += "\t" + "'" + objEntity.PaidPerson.Trim().ToUpper() + "'" + "," + " " + "\n";

                if (Program.IsValidYear(objEntity.YearInt))
                    sQueryString += "\t" + "'" + objEntity.YearInt + "'" + "," + " " + "\n";

                if (!String.IsNullOrEmpty(objEntity.DatePaid) && objEntity.DatePaid.Trim().ToLower() != Program.DB_Not_Available.Trim().ToLower())
                    sQueryString += "\t" + "'" + objEntity.DatePaid.Trim() + "'" + "," + " " + "\n";

                if (!String.IsNullOrEmpty(objEntity.DataPagamentoAgregado) && objEntity.DataPagamentoAgregado.Trim().ToLower() != Program.DB_Not_Available.Trim().ToLower())
                    sQueryString += "\t" + "'" + objEntity.DataPagamentoAgregado.Trim() + "'" + "," + " " + "\n";

                if (objEntity.Value > 0)
                    sQueryString += "\t" + "'" + Program.SetPayDoubleStringValue(objEntity.Value) + "'" + "," + " " + "\n";

                if (objEntity.ValuePaid >= 0)
                    sQueryString += "\t" + "'" + Program.SetPayDoubleStringValue(objEntity.ValuePaid) + "'" + "," + " " + "\n";

                if (objEntity.ValueMissing >= 0)
                    sQueryString += "\t" + "'" + Program.SetPayDoubleStringValue(objEntity.ValueMissing) + "'" + "," + " " + "\n";

                if (objEntity.ValueOnPaying >= 0)
                    sQueryString += "\t" + "'" + Program.SetPayDoubleStringValue(objEntity.ValueOnPaying) + "'" + "," + " " + "\n";

                if (objEntity.Pay_State != AMFC_Entity.PayState.UNDEFINED)
                    sQueryString += "\t" + "'" + objEntity.Pay_State_DB_Value + "'" + "," + " " + "\n";

                sQueryString += "\t" + "'" + objEntity.Notas.Trim() + "'" + " " + " " + "\n";

                sQueryString += ") " + " " + "\n";
                #endregion  Values

                #endregion  Query

                using (OleDbConnection objOleDbConnection = new OleDbConnection(OLE_DB_ConnectionString))
                {
                    objOleDbConnection.Open();
                    using (OleDbCommand objOleDbCommand = new OleDbCommand())
                    {
                        objOleDbCommand.Connection = objOleDbConnection;
                        objOleDbCommand.CommandText = sQueryString;
                        if (objOleDbCommand.ExecuteNonQuery() != 1)
                            return -1;
                    }
                }

                return 1;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message + " -> " + "QUERY: " + sQueryString, Program.ErroType.EXCEPTION, true, false);
                return -1;
            }
        }

        /// <versions>28-11-2017(GesAMFC-v0.0.4.39)</versions>
        public Int64 Edit_QUOTA(AMFC_Entity objEntity)
        {
            String sQueryString = String.Empty;
            try
            {
                String sDBF_File_Name = Program.GetEntityDbfFileName(AMFC_Entities.AMFC_Entidade_Tipo.QUOTAS);

                Set_OLE_DB_ConnectionString();
                if (String.IsNullOrEmpty(sDBF_File_Name))
                    OLE_DB_Settings();

                #region     Validate Data
                if (objEntity.Id < 1)
                {
                    String sError = Program.Entity_QUOTA_Desc_Single + " " + "ID" + " " + "inválido" + ": " + objEntity.Id;
                    MessageBox.Show(sError, "ID Inválido", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return -1;
                }
                if (objEntity.MemberNumber < new AMFCMember().MinNumber || objEntity.MemberNumber > new AMFCMember().MaxNumber)
                {
                    String sError = "Nº de sócio inválido: " + objEntity.MemberNumber;
                    MessageBox.Show(sError, "Sócio Inválido", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return -1;
                }
                if (objEntity.Value <= 0)
                {
                    String sError = "A " + Program.Entity_QUOTA_Desc_Single + " tem de ter um valor monetário maior que zero!";
                    MessageBox.Show(sError, "Valor " + Program.Entity_QUOTA_Desc_Single + " Inválido", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return -1;
                }
                //.... verificar outros no futuro
                #endregion  Validate Data

                #region     Query
                sQueryString += "UPDATE " + sDBF_File_Name + " " + "\n";
                sQueryString += "SET" + " " + "\n";

                if (!String.IsNullOrEmpty(objEntity.ListaCaixa) && objEntity.ListaCaixa.Trim().ToLower() != Program.DB_Not_Available.Trim().ToLower())
                    sQueryString += "\t" + "LISTACAIXA" + " = " + "'" + objEntity.ListaCaixa.Trim().ToUpper() + "'" + "," + " " + "\n";

                if (!String.IsNullOrEmpty(objEntity.ListaRecibos) && objEntity.ListaRecibos.Trim().ToLower() != Program.DB_Not_Available.Trim().ToLower())
                    sQueryString += "\t" + "LISTARECNU" + " = " + "'" + objEntity.ListaRecibos.Trim().ToUpper() + "'" + "," + " " + "\n";

                if (objEntity.Value > 0)
                    sQueryString += "\t" + "VALOR" + " = " + "'" + Program.SetPayDoubleStringValue(objEntity.Value) + "'" + "," + " " + "\n";

                if (!String.IsNullOrEmpty(objEntity.DatePaid) && objEntity.DatePaid.Trim().ToLower() != Program.DB_Not_Available.Trim().ToLower())
                    sQueryString += "\t" + "DATAPAG" + " = " + "'" + objEntity.DatePaid.Trim() + "'" + "," + " " + "\n";

                if (!String.IsNullOrEmpty(objEntity.DataPagamentoAgregado) && objEntity.DataPagamentoAgregado.Trim().ToLower() != Program.DB_Not_Available.Trim().ToLower())
                    sQueryString += "\t" + "DATAPAGREG" + " = " + "'" + objEntity.DataPagamentoAgregado.Trim() + "'" + "," + " " + "\n";

                if (!String.IsNullOrEmpty(objEntity.PaidPerson) && objEntity.PaidPerson.Trim().ToLower() != Program.DB_Not_Available.Trim().ToLower())
                    sQueryString += "\t" + "NOME" + " = " + "'" + objEntity.PaidPerson.Trim().ToUpper() + "'" + "," + " " + "\n";

                if (objEntity.ValuePaid >= 0)
                    sQueryString += "\t" + "PAGO" + " = " + "'" + Program.SetPayDoubleStringValue(objEntity.ValuePaid) + "'" + "," + " " + "\n";

                if (objEntity.ValueMissing >= 0)
                    sQueryString += "\t" + "FALTA" + " = " + "'" + Program.SetPayDoubleStringValue(objEntity.ValueMissing) + "'" + "," + " " + "\n";

                if (objEntity.ValueOnPaying >= 0)
                    sQueryString += "\t" + "EMPAG" + " = " + "'" + Program.SetPayDoubleStringValue(objEntity.ValueOnPaying) + "'" + "," + " " + "\n";

                if (objEntity.Pay_State != AMFC_Entity.PayState.UNDEFINED)
                    sQueryString += "\t" + "ESTADO" + " = " + "'" + objEntity.Pay_State_DB_Value + "'" + "," + " " + "\n";

                sQueryString += "\t" + "NOTAS" + " = " + "'" + objEntity.Notas.Trim() + "'" + " " + " " + "\n";

                sQueryString += "WHERE " + "ID" + " = " + objEntity.Id + "\n";
                #endregion  Query

                using (OleDbConnection objOleDbConnection = new OleDbConnection(OLE_DB_ConnectionString))
                {
                    objOleDbConnection.Open();
                    using (OleDbCommand objOleDbCommand = new OleDbCommand())
                    {
                        objOleDbCommand.Connection = objOleDbConnection;
                        objOleDbCommand.CommandText = sQueryString;
                        if (objOleDbCommand.ExecuteNonQuery() != 1)
                            return -1;
                    }
                }

                return 1;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message + " -> " + "QUERY: " + sQueryString, Program.ErroType.EXCEPTION, true, false);
                return -1;
            }
        }

        /// <versions>28-11-2017(GesAMFC-v0.0.4.39)</versions>
        public Int64 Del_QUOTA(AMFC_Entity objEntity)
        {
            String sQueryString = String.Empty;
            try
            {
                String sDBF_File_Name = Program.GetEntityDbfFileName(AMFC_Entities.AMFC_Entidade_Tipo.QUOTAS);

                Set_OLE_DB_ConnectionString();
                if (String.IsNullOrEmpty(sDBF_File_Name))
                    OLE_DB_Settings();

                #region     Validate Data
                if (objEntity.Id < 1)
                {
                    String sError = Program.Entity_QUOTA_Desc_Single + " " + "ID" + " " + "inválido" + ": " + objEntity.Id;
                    MessageBox.Show(sError, "ID Inválido", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return -1;
                }
                #endregion  Validate Data

                #region     Query
                sQueryString = "DELETE FROM " + sDBF_File_Name + " WHERE " + "ID" + " = " + objEntity.Id + ";" + "\n";
                #endregion  Query

                using (OleDbConnection objOleDbConnection = new OleDbConnection(OLE_DB_ConnectionString))
                {
                    objOleDbConnection.Open();
                    using (OleDbCommand objOleDbCommand = new OleDbCommand())
                    {
                        objOleDbCommand.Connection = objOleDbConnection;
                        objOleDbCommand.CommandText = sQueryString;
                        if (objOleDbCommand.ExecuteNonQuery() != 1)
                            return -1;
                    }
                }

                return 1;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message + " -> " + "QUERY: " + sQueryString, Program.ErroType.EXCEPTION, true, false);
                return -1;
            }
        }

        #endregion  Admin QUOTAS

        #region     Admin ENTID

        /// <versions>15-03-2018(GesAMFC-v1.0.0.3)</versions>
        /// <remarks>
        /// ID,N,10,0	LISTACAIXA,C,20	    LISTARECNU,C,20	    IDLOTE,N,5,0	NUMLOTE,C,10	SOCIO,N,5,0	    NOME,C,70	ANO,N,4,0	MES,N,2,0	DATAPAG,D	DESIGNACAO,C,140	PRECOM2P,N,12,3	    AREAPAGO,N,12,2	    VALORPAGO,N,12,3	ESTADO,C,1	NOTAS,C,140,    PAGNBR,N,2,0
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
        public String Set_Member_ENTID_Query(EntityTypeConfigs objEntityConfigs, Int64 lMemberNumber, Int64 lLoteId, Int64 lENTIDId, Int32 iYear, Int32 iMonth, Boolean bGetSingleRecord)
        {
            try
            {
                String sQueryString = String.Empty;

                String sDBF_FileName = objEntityConfigs.DBF_File_Name;

                sQueryString += "SELECT" + " " + "\n";
                sQueryString += sDBF_FileName + "." + "ID" + "," + " " + "\n"; /// 01: ID,N,10,0	
                sQueryString += sDBF_FileName + "." + "LISTACAIXA" + "," + " " + "\n"; /// 02: LISTACAIXA,C,20	
                sQueryString += sDBF_FileName + "." + "LISTARECNU" + "," + " " + "\n"; /// 03: LISTARECNU,C,20
                sQueryString += sDBF_FileName + "." + "IDLOTE" + "," + " " + "\n"; /// 04: IDLOTE,N,5,0
                sQueryString += sDBF_FileName + "." + "NUMLOTE" + "," + " " + "\n"; /// 05: NUMLOTE,C,10
                sQueryString += sDBF_FileName + "." + "SOCIO" + "," + " " + "\n"; /// 06: SOCIO,N,5,0	
                sQueryString += sDBF_FileName + "." + "NOME" + "," + " " + "\n"; /// 07: NOME,C,70	
                sQueryString += sDBF_FileName + "." + "ANO" + "," + " " + "\n"; /// 08: ANO,N,4,0	
                sQueryString += sDBF_FileName + "." + "MES" + "," + " " + "\n"; /// 09: MES,N,2,0	
                sQueryString += sDBF_FileName + "." + "DATAPAG" + "," + " " + "\n"; /// 10: DATAPAG,D	
                sQueryString += sDBF_FileName + "." + "DESIGNACAO" + "," + " " + "\n"; /// 11: DESIGNACAO,C,140	
                sQueryString += sDBF_FileName + "." + "PRECOM2P" + "," + " " + "\n"; /// 12: PRECOM2P,N,12,3	
                sQueryString += sDBF_FileName + "." + "AREAPAGO" + "," + " " + "\n"; /// 13: AREAPAGO,N,12,2	
                sQueryString += sDBF_FileName + "." + "VALORPAGO" + "," + " " + "\n"; /// 14: VALORPAGO,N,12,3	
                sQueryString += sDBF_FileName + "." + "ESTADO" + "," + " " + "\n"; /// 15: ESTADO,C,1	
                sQueryString += sDBF_FileName + "." + "NOTAS" + ", " + " " + "\n"; /// 16: NOTAS,C,140	
                sQueryString += sDBF_FileName + "." + "PAGNBR" + " " + " " + "\n"; /// 17: PAGNBR,N,2,0

                sQueryString += "FROM" + " " + sDBF_FileName + " " + "\n";

                if (lMemberNumber >= new AMFCMember().MinNumber && lMemberNumber <= new AMFCMember().MaxNumber)
                {
                    sQueryString += "WHERE" + " " + "\n";
                    sQueryString += sDBF_FileName + "." + "SOCIO" + " = " + lMemberNumber + "\n";
                    sQueryString += "AND" + " " + "\n";
                    sQueryString += sDBF_FileName + "." + "ID" + " " + "IS NOT NULL" + " " + "\n";
                    if (lLoteId > 0)
                    {
                        sQueryString += "AND" + " " + "\n";
                        sQueryString += sDBF_FileName + "." + "IDLOTE" + " = " + lLoteId + "\n";
                    }
                }
                else if (Program.IsValidYear(iYear))
                {
                    sQueryString += "WHERE" + " " + "\n";
                    sQueryString += "ANO" + " = " + iYear + "\n";
                    if (Program.IsValidMonth(iMonth))
                    {
                        sQueryString += "AND" + " " + "\n";
                        sQueryString += "MES" + " = " + iMonth + "\n";
                    }
                }
                else if (bGetSingleRecord && lENTIDId > 0)
                {
                    sQueryString += "WHERE" + " " + "\n";
                    sQueryString += sDBF_FileName + "." + "ID" + " = " + lENTIDId + "\n";
                }
                else
                {
                    sQueryString += "ORDER BY" + " " + "\n";
                    sQueryString += sDBF_FileName + "." + "ANO" + "," + " " + "\n";
                    sQueryString += sDBF_FileName + "." + "MES" + "," + " " + "\n";
                    sQueryString += sDBF_FileName + "." + "SOCIO" + " " + " " + "\n";
                }
                sQueryString += ";";

                return sQueryString;
            }
            catch
            {
                return null;
            }
        }

        /// <versions>08-12-2017(GesAMFC-v0.0.5.3)</versions>
        public LIST_PAG_ENTIDADE Get_All_Member_ENTID(EntityTypeConfigs objEntityConfigs, Int32 iYear, Int32 iMonth)
        {
            try
            {
                LIST_PAG_ENTIDADE objListEntidades = new LIST_PAG_ENTIDADE();
                Set_OLE_DB_ConnectionString();

                AMFCMember objMember = new AMFCMember();
                using (OleDbConnection objOleDbConnection = new OleDbConnection(OLE_DB_ConnectionString))
                {
                    objOleDbConnection.Open();
                    using (OleDbCommand objOleDbCommand = new OleDbCommand())
                    {
                        objOleDbCommand.Connection = objOleDbConnection;

                        objOleDbCommand.CommandText = Set_Member_ENTID_Query(objEntityConfigs, -1, -1, -1, iYear, iMonth, false);

                        objListEntidades = Get_Members_ENTID_DB_Data(objEntityConfigs, objOleDbCommand, false);
                        if (objListEntidades == null || objListEntidades.List == null)
                        {
                            StackFrame objStackFrame = new StackFrame();
                            String sErrorMsg = "Não foi possível obter a Lista de" + objEntityConfigs.Entity_Desc_Plural + " dos Sócios da Base de Dados!" + " -> " + "QUERY: " + objOleDbCommand.CommandText;
                            Program.HandleError(objStackFrame.GetMethod().Name, sErrorMsg, Program.ErroType.ERROR, true, true);
                            objStackFrame = null;
                            return null;
                        }
                    }
                }

                return objListEntidades;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return null;
            }
        }

        /// <versions>08-12-2017(GesAMFC-v0.0.5.3)</versions>
        public LIST_PAG_ENTIDADE Get_Member_ENTID_ByNbr(EntityTypeConfigs objEntityConfigs, Int64 lMemberNumber, Int64 lLoteId, Int32 iYear, Int32 iMonth)
        {
            try
            {
                LIST_PAG_ENTIDADE objListEntidades = new LIST_PAG_ENTIDADE();

                Set_OLE_DB_ConnectionString();

                using (OleDbConnection objOleDbConnection = new OleDbConnection(OLE_DB_ConnectionString))
                {
                    objOleDbConnection.Open();
                    using (OleDbCommand objOleDbCommand = new OleDbCommand())
                    {
                        objOleDbCommand.Connection = objOleDbConnection;
                        objOleDbCommand.CommandText = Set_Member_ENTID_Query(objEntityConfigs, lMemberNumber,lLoteId, -1, iYear, iMonth, false);

                        objListEntidades = Get_Members_ENTID_DB_Data(objEntityConfigs, objOleDbCommand, false);
                        if (objListEntidades == null || objListEntidades.List == null)
                        {
                            StackFrame objStackFrame = new StackFrame();
                            String sErrorMsg = "Não foi possível obter a " + objEntityConfigs.Entity_Desc_Single + " do Sócio com o Nº: " + lMemberNumber + " -> " + "QUERY: " + objOleDbCommand.CommandText;
                            Program.HandleError(objStackFrame.GetMethod().Name, sErrorMsg, Program.ErroType.ERROR, true, true);
                            objStackFrame = null;
                            return null;
                        }
                    }
                }

                return objListEntidades;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return null;
            }
        }
        
        /// <versions>08-12-2017(GesAMFC-v0.0.5.3)</versions>
        public LIST_PAG_ENTIDADE Get_Members_ENTID_ByYearMonth(EntityTypeConfigs objEntityConfigs, Int32 iYear, Int32 iMonth)
        {
            try
            {
                LIST_PAG_ENTIDADE objListEntidades = new LIST_PAG_ENTIDADE();

                Set_OLE_DB_ConnectionString();

                using (OleDbConnection objOleDbConnection = new OleDbConnection(OLE_DB_ConnectionString))
                {
                    objOleDbConnection.Open();
                    using (OleDbCommand objOleDbCommand = new OleDbCommand())
                    {
                        objOleDbCommand.Connection = objOleDbConnection;
                        objOleDbCommand.CommandText = Set_Member_ENTID_Query(objEntityConfigs, -1, -1, -1, iYear, iMonth, false);

                        objListEntidades = Get_Members_ENTID_DB_Data(objEntityConfigs, objOleDbCommand, false);
                        if (objListEntidades == null || objListEntidades.List == null)
                        {
                            StackFrame objStackFrame = new StackFrame();
                            String sErrorMsg = "Não foi possível obter a " + objEntityConfigs.Entity_Desc_Single + " do Ano: " + iYear + " e Mês: " + iMonth + " -> " + "QUERY: " + objOleDbCommand.CommandText;
                            Program.HandleError(objStackFrame.GetMethod().Name, sErrorMsg, Program.ErroType.ERROR, true, true);
                            objStackFrame = null;
                            return null;
                        }
                    }
                }

                return objListEntidades;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return null;
            }
        }

        /// <versions>08-12-2017(GesAMFC-v0.0.5.3)</versions>
        public PAG_ENTIDADE Get_Member_ENTID_ById(EntityTypeConfigs objEntityConfigs, Int64 lId)
        {
            try
            {
                PAG_ENTIDADE objEnt = new PAG_ENTIDADE();
                Set_OLE_DB_ConnectionString();

                using (OleDbConnection objOleDbConnection = new OleDbConnection(OLE_DB_ConnectionString))
                {
                    objOleDbConnection.Open();
                    using (OleDbCommand objOleDbCommand = new OleDbCommand())
                    {
                        objOleDbCommand.Connection = objOleDbConnection;
                        objOleDbCommand.CommandText = Set_Member_ENTID_Query(objEntityConfigs, -1, -1, lId, -1, -1, true);

                        LIST_PAG_ENTIDADE objListEntidades = Get_Members_ENTID_DB_Data(objEntityConfigs, objOleDbCommand, true);
                        if (objListEntidades == null || objListEntidades.List == null || objListEntidades.List.Count > 1)
                        {
                            StackFrame objStackFrame = new StackFrame();
                            String sErrorMsg = "Não foi possível obter a " + objEntityConfigs.Entity_Desc_Single + " com o ID: " + lId + " -> " + "QUERY: " + objOleDbCommand.CommandText;
                            Program.HandleError(objStackFrame.GetMethod().Name, sErrorMsg, Program.ErroType.ERROR, true, true);
                            objStackFrame = null;
                            return null;
                        }
                        if (objListEntidades.List.Count == 1)
                            objEnt = objListEntidades.List[0];
                    }
                }

                return objEnt;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return null;
            }
        }

        /// <versions>09-03-2018(GesAMFC-v1.0.0.3)</versions>
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
        private LIST_PAG_ENTIDADE Get_Members_ENTID_DB_Data(EntityTypeConfigs objEntityConfigs, OleDbCommand objOleDbCommand, Boolean bGetSingleRecord)
        {
            try
            {
                LIST_PAG_ENTIDADE objListEntidades = new LIST_PAG_ENTIDADE();

                using (OleDbDataReader objOleDbDataReader = objOleDbCommand.ExecuteReader())
                {
                    while (objOleDbDataReader.Read())
                    {

                        PAG_ENTIDADE objEnt = new PAG_ENTIDADE();

                        if (objOleDbDataReader["ID"] == DBNull.Value)
                            continue;
                        if (objOleDbDataReader["SOCIO"] == DBNull.Value)
                            continue;
                        if (objOleDbDataReader["IDLOTE"] == DBNull.Value)
                            continue;

                        objEnt.ID = Convert.ToInt64(objOleDbDataReader["ID"]); /// 01: ID,N,10,0	

                        if (objOleDbDataReader["LISTACAIXA"] != DBNull.Value) /// 02: LISTACAIXA,C,20	
                            objEnt.LISTACAIXA = Program.SetTextString(Convert.ToString(objOleDbDataReader["LISTACAIXA"]));

                        if (objOleDbDataReader["LISTARECNU"] != DBNull.Value) /// 03: LISTARECNU,C,20	
                            objEnt.LISTARECNU = Program.SetTextString(Convert.ToString(objOleDbDataReader["LISTARECNU"]));

                        if (objOleDbDataReader["IDLOTE"] != DBNull.Value) /// 04: IDLOTE,N,5,0	
                            objEnt.IDLOTE = Convert.ToInt64(objOleDbDataReader["IDLOTE"]);

                        if (objOleDbDataReader["NUMLOTE"] != DBNull.Value) /// 05: NUMLOTE,C,10	
                            objEnt.NUMLOTE = Convert.ToString(objOleDbDataReader["NUMLOTE"]);

                        if (objOleDbDataReader["SOCIO"] != DBNull.Value) /// 06: SOCIO,N,5,0	
                            objEnt.SOCIO = Convert.ToInt64(objOleDbDataReader["SOCIO"]);

                        if (objOleDbDataReader["NOME"] != DBNull.Value) /// 07: NOME,C,70	
                            objEnt.NOME = Program.EncodeStringToISO(objOleDbDataReader["NOME"].ToString());

                        if (objOleDbDataReader["ANO"] != DBNull.Value) /// 08: ANO,N,4,0	
                            objEnt.ANO = Convert.ToInt32(objOleDbDataReader["ANO"].ToString().Trim());

                        if (objOleDbDataReader["MES"] != DBNull.Value) /// 09: MES,N,2,0	
                            objEnt.MES = Convert.ToInt32(objOleDbDataReader["MES"].ToString().Trim());

                        if (objOleDbDataReader["DATAPAG"] != DBNull.Value) /// 10: DATAPAG,D	
                            objEnt.DATAPAG = Program.SetDateTimeValue(Program.ConvertToValidDateTime(objOleDbDataReader["DATAPAG"].ToString().Trim()), -1, -1);

                        if (objOleDbDataReader["DESIGNACAO"] != DBNull.Value) /// 11: DESIGNACAO,C,140	
                            objEnt.DESIGNACAO = Program.EncodeStringToISO(objOleDbDataReader["DESIGNACAO"].ToString());

                        if (objOleDbDataReader["PRECOM2P"] != DBNull.Value) /// 12: PRECOM2P,N,12,3	
                            objEnt.PRECOM2P = Program.SetPayCurrencyEuroDoubleValue(Convert.ToDouble(objOleDbDataReader["PRECOM2P"].ToString().Trim()));

                        if (objOleDbDataReader["AREAPAGO"] != DBNull.Value) /// 13: AREAPAGO,N,12,2	
                            objEnt.AREAPAGO = Program.SetAreaDoubleValue(objOleDbDataReader["AREAPAGO"].ToString());

                        if (objOleDbDataReader["VALORPAGO"] != DBNull.Value) /// 14: VALORPAGO,N,12,3	
                            objEnt.VALORPAGO = Program.SetPayCurrencyEuroDoubleValue(Convert.ToDouble(objOleDbDataReader["VALORPAGO"].ToString().Trim()));
                       
                        if (objOleDbDataReader["ESTADO"] != DBNull.Value) /// 15: ESTADO,C,1	
                            objEnt.Pay_State_DB_Value = objOleDbDataReader["ESTADO"].ToString().Trim();

                        if (objOleDbDataReader["NOTAS"] != DBNull.Value) /// 16: NOTAS,C,140	
                            objEnt.NOTAS = Program.SetTextString(Convert.ToString(objOleDbDataReader["NOTAS"]));

                        objEnt.PAGTOTAL = Get_DB_ENTID_PAGTOTAL_Number(objEntityConfigs, objEnt);

                        /// 18: PAGNBR,N,2,0
                        if (objOleDbDataReader["PAGNBR"] != DBNull.Value)
                            objEnt.PAGNBR = Convert.ToInt32(objOleDbDataReader["PAGNBR"].ToString().Trim());

                        if (!objListEntidades.Contains(objEnt))
                            objListEntidades.Add(objEnt);

                        if (bGetSingleRecord)
                            return objListEntidades;
                    }
                }

                return objListEntidades;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return null;
            }
        }

        /// <versions>08-12-2017(GesAMFC-v0.0.5.3)</versions>
        public Boolean Member_ENTID_Already_Exist(EntityTypeConfigs objEntityConfigs, PAG_ENTIDADE objEnt)
        {
            String sQueryString = String.Empty;
            try
            {
                String sDBF_File_Name = objEntityConfigs.DBF_File_Name;

                Set_OLE_DB_ConnectionString();
                if (String.IsNullOrEmpty(sDBF_File_Name))
                    OLE_DB_Settings();

                #region     Query
                sQueryString += "SELECT COUNT(*) FROM " + sDBF_File_Name + " " + "\n";
                sQueryString += "WHERE" + " " + "\n";
                sQueryString += "\t" + "ID" + " = " + objEnt.ID + " " + "\n";
                sQueryString += "OR" + " " + "\n";
                sQueryString += "(" + " " + "\n";
                sQueryString += "\t" + "SOCIO" + " = " + objEnt.SOCIO + " " + "\n";
                sQueryString += "\t" + "AND" + " " + "\n";
                sQueryString += "\t" + "VALORPAGO" + " LIKE " + "'" + objEnt.VALORPAGO + "'" + " " + "\n";
                sQueryString += "\t" + "AND" + " " + "\n";
                sQueryString += "\t" + "DATAPAG" + " LIKE " + "'" + objEnt.DATAPAG.ToString(Program.DBF_Compare_Date_Format_String) + "'" + " " + "\n";
                sQueryString += ")" + " " + "\n";
                sQueryString += "; ";
                #endregion  Query

                using (OleDbConnection objOleDbConnection = new OleDbConnection(OLE_DB_ConnectionString))
                {
                    objOleDbConnection.Open();
                    using (OleDbCommand objOleDbCommand = new OleDbCommand())
                    {
                        objOleDbCommand.Connection = objOleDbConnection;
                        objOleDbCommand.CommandText = sQueryString;
                        if (Convert.ToInt32(objOleDbCommand.ExecuteScalar()) > 0)
                            return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message + " -> " + "QUERY: " + sQueryString, Program.ErroType.EXCEPTION, true, false);
                return true;
            }
        }

        /// <versions>08-12-2017(GesAMFC-v0.0.5.3)</versions>
        public Int32 Get_ENTID_Max_Number(EntityTypeConfigs objEntityConfigs)
        {
            String sQueryString = String.Empty;
            try
            {
                Int32 iMaxENTID = -1;

                String sDBf_Filename = objEntityConfigs.DBF_File_Name;

                Set_OLE_DB_ConnectionString();
                if (String.IsNullOrEmpty(sDBf_Filename))
                    OLE_DB_Settings();

                using (OleDbConnection objOleDbConnection = new OleDbConnection(OLE_DB_ConnectionString))
                {
                    objOleDbConnection.Open();
                    using (OleDbCommand objOleDbCommand = new OleDbCommand())
                    {
                        objOleDbCommand.Connection = objOleDbConnection;

                        objOleDbCommand.CommandText = "SELECT COUNT(*) FROM " + sDBf_Filename + " " + "\n";
                        if (Convert.ToInt32(objOleDbCommand.ExecuteScalar()) <= 0)
                            return 0;

                        objOleDbCommand.CommandText = "SELECT MAX(" + "ID" + ") AS " + "MaxId" + " FROM " + sDBf_Filename + ";";
                        iMaxENTID = Convert.ToInt32(objOleDbCommand.ExecuteScalar());
                    }
                }
                return iMaxENTID;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message + " -> " + "QUERY: " + sQueryString, Program.ErroType.EXCEPTION, true, false);
                return -1;
            }
        }

        /// <versions>14-03-2018(GesAMFC-v1.0.0.3)</versions>
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
        public Int64 Add_ENTID(EntityTypeConfigs objEntityConfigs, PAG_ENTIDADE objEnt, Boolean bShowMessage)
        {
            String sQueryString = String.Empty;
            try
            {
                String sDBF_File_Name = objEntityConfigs.DBF_File_Name;

                Set_OLE_DB_ConnectionString();
                if (String.IsNullOrEmpty(sDBF_File_Name))
                    OLE_DB_Settings();

                #region     Validate Data
                if (objEnt.ID < 1)
                {
                    if (bShowMessage)
                    {
                        String sError = objEntityConfigs.Entity_Desc_Single + " " + "ID" + " " + "inválido" + ": " + objEnt.ID;
                        MessageBox.Show(sError, "ID Inválido", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    return -1;
                }
                if (objEnt.SOCIO < new AMFCMember().MinNumber || objEnt.SOCIO > new AMFCMember().MaxNumber)
                {
                    if (bShowMessage)
                    {
                        String sError = "Nº de sócio inválido: " + objEnt.SOCIO;
                        MessageBox.Show(sError, "Sócio Inválido", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    return -1;
                }
                if (objEnt.VALORPAGO <= 0)
                {
                    if (bShowMessage)
                    {
                        String sError = "A " + objEntityConfigs.Entity_Desc_Single + " tem de ter um valor monetário maior que zero!";
                        MessageBox.Show(sError, "Valor " + objEntityConfigs.Entity_Desc_Single + " Inválido", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    return -1;
                }
                //.... verificar outros no futuro
                #endregion  Validate Data

                #region     Check if Member " + Program.Entity_ENTID_Desc + " already exist
                if (Member_ENTID_Already_Exist(objEntityConfigs, objEnt))
                {
                    if (bShowMessage)
                    {
                        String sWarning = "Nesta data " + objEnt.DATAPAG.ToString() + " foi adicionada a " + objEntityConfigs.Entity_Desc_Single + " no valor de " + objEnt.VALORPAGO + " " + "do Sócio com o Nº: " + objEnt.SOCIO;
                        if (!String.IsNullOrEmpty(objEnt.NOME))
                            sWarning += " e Nome: " + objEnt.NOME + "";
                        MessageBox.Show(sWarning, objEntityConfigs.Entity_Desc_Single + " " + "já existente!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    return 0;
                }
                #endregion  Check if Member already exist

                #region     Set New PAGNBR
                if (objEnt.PAGNBR < 1)
                {
                    Int32 iMaxPagNbr = Get_DB_ENTID_PAGNBR_Max_Number(objEntityConfigs, objEnt);
                    objEnt.PAGNBR = iMaxPagNbr + 1; 
                }
                #endregion  Set New PAGNBR

                #region     Query
                sQueryString += "INSERT INTO " + sDBF_File_Name + " " + "\n";

                #region     Columns
                sQueryString += "( " + " " + "\n";

                // 01: ID,N,10,0	
                sQueryString += "\t" + "ID" + "," + " " + "\n";

                // 02: LISTACAIXA,C,20	
                if (!String.IsNullOrEmpty(objEnt.LISTACAIXA) && objEnt.LISTACAIXA.Trim().ToLower() != Program.DB_Not_Available.Trim().ToLower())
                    sQueryString += "\t" + "LISTACAIXA" + "," + " " + "\n";

                // 03: LISTARECNU,C,20	
                if (!String.IsNullOrEmpty(objEnt.LISTARECNU) && objEnt.LISTARECNU.Trim().ToLower() != Program.DB_Not_Available.Trim().ToLower())
                    sQueryString += "\t" + "LISTARECNU" + "," + " " + "\n";

                // 04: IDLOTE,N,5,0	
                if (objEnt.IDLOTE > 0)
                    sQueryString += "\t" + "IDLOTE" + "," + " " + "\n";

                // 05: NUMLOTE,C,10	
                if (!String.IsNullOrEmpty(objEnt.NUMLOTE) && objEnt.NUMLOTE.Trim().ToLower() != Program.DB_Not_Available.Trim().ToLower())
                    sQueryString += "\t" + "NUMLOTE" + "," + " " + "\n";

                // 06: SOCIO,N,5,0	
                if (objEnt.SOCIO > 0)
                    sQueryString += "\t" + "SOCIO" + "," + " " + "\n";

                // 07: NOME,C,70	
                if (!String.IsNullOrEmpty(objEnt.NOME) && objEnt.NOME.Trim().ToLower() != Program.DB_Not_Available.Trim().ToLower())
                    sQueryString += "\t" + "NOME" + "," + " " + "\n";

                // 08: ANO,N,4,0	
                if (Program.IsValidYear(objEnt.ANO))
                    sQueryString += "\t" + "ANO" + "," + " " + "\n";

                // 09: MES,N,2,0	
                if (Program.IsValidMonth(objEnt.MES))
                    sQueryString += "\t" + "MES" + "," + " " + "\n";

                // 10: DATAPAG,D	
                if (Program.IsValidDateTime(objEnt.DATAPAG))
                    sQueryString += "\t" + "DATAPAG" + "," + " " + "\n";

                // 11: DESIGNACAO,C,140	
                if (!String.IsNullOrEmpty(objEnt.DESIGNACAO) && objEnt.DESIGNACAO.Trim().ToLower() != Program.DB_Not_Available.Trim().ToLower())
                    sQueryString += "\t" + "DESIGNACAO" + "," + " " + "\n";

                // 12: PRECOM2P,N,12,3	
                if (objEnt.PRECOM2P > 0)
                    sQueryString += "\t" + "PRECOM2P" + "," + " " + "\n";

                // 13: AREAPAGO,N,12,2	
                if (objEnt.AREAPAGO > 0)
                    sQueryString += "\t" + "AREAPAGO" + "," + " " + "\n";

                // 14: VALORPAGO,N,12,3	
                if (objEnt.VALORPAGO > 0)
                    sQueryString += "\t" + "VALORPAGO" + "," + " " + "\n";

                // 15: ESTADO,C,1	
                if (objEnt.Pay_State != PAG_ENTIDADE.PayState.UNDEFINED)
                    sQueryString += "\t" + "ESTADO" + "," + " " + "\n";

                // 16: NOTAS,C,140	
                sQueryString += "\t" + "NOTAS" + "," + " " + "\n";

                // 17: PAGNBR,N,2,0
                sQueryString += "\t" + "PAGNBR" + " " + " " + "\n";

                sQueryString += ") " + " " + "\n";
                #endregion  Columns

                #region     Values
                sQueryString += "VALUES" + " " + "\n";
                sQueryString += "( " + " " + "\n";

                // 01: ID,N,10,0	
                sQueryString += "\t" + objEnt.ID + "," + " " + "\n";

                // 02: LISTACAIXA,C,20	
                if (!String.IsNullOrEmpty(objEnt.LISTACAIXA) && objEnt.LISTACAIXA.Trim().ToLower() != Program.DB_Not_Available.Trim().ToLower())
                    sQueryString += "\t" + "'" + objEnt.LISTACAIXA.Trim() + "'" + "," + " " + "\n";

                // 03: LISTARECNU,C,20	
                if (!String.IsNullOrEmpty(objEnt.LISTARECNU) && objEnt.LISTARECNU.Trim().ToLower() != Program.DB_Not_Available.Trim().ToLower())
                    sQueryString += "\t" + "'" + objEnt.LISTARECNU.Trim() + "'" + "," + " " + "\n";

                // 04: IDLOTE,N,5,0	
                if (objEnt.IDLOTE > 0)
                    sQueryString += "\t" + objEnt.IDLOTE + "," + " " + "\n";

                // 05: NUMLOTE,C,10	
                if (!String.IsNullOrEmpty(objEnt.NUMLOTE) && objEnt.NUMLOTE.Trim().ToLower() != Program.DB_Not_Available.Trim().ToLower())
                    sQueryString += "\t" + "'" + objEnt.NUMLOTE.Trim().ToUpper() + "'" + "," + " " + "\n";

                // 06: SOCIO,N,5,0	
                if (objEnt.SOCIO > 0)
                    sQueryString += "\t" + objEnt.SOCIO + "," + " " + "\n";

                // 07: NOME,C,70	
                if (!String.IsNullOrEmpty(objEnt.NOME) && objEnt.NOME.Trim().ToLower() != Program.DB_Not_Available.Trim().ToLower())
                    sQueryString += "\t" + "'" + objEnt.NOME.Trim().ToUpper() + "'" + "," + " " + "\n";

                // 08: ANO,N,4,0	
                if (Program.IsValidYear(objEnt.ANO))
                    sQueryString += "\t" + objEnt.ANO + "," + " " + "\n";
                
                // 09: MES,N,2,0	
                if (Program.IsValidMonth(objEnt.MES))
                    sQueryString += "\t" + objEnt.MES + "," + " " + "\n";

                // 10: DATAPAG,D	
                if (Program.IsValidDateTime(objEnt.DATAPAG))
                    sQueryString += "\t" + "'" + objEnt.DATAPAG.ToString(Program.Date_Format_String, CultureInfo.CurrentCulture) + "'" + "," + " " + "\n";

                // 11: DESIGNACAO,C,140	
                if (!String.IsNullOrEmpty(objEnt.DESIGNACAO) && objEnt.DESIGNACAO.Trim().ToLower() != Program.DB_Not_Available.Trim().ToLower())
                    sQueryString += "\t" + "'" + objEnt.DESIGNACAO.Trim().ToUpper() + "'" + "," + " " + "\n";

                // 12: PRECOM2P,N,12,3	
                if (objEnt.PRECOM2P > 0)
                    sQueryString += "\t" + "'" + Program.SetPayDoubleStringValue(objEnt.PRECOM2P) + "'" + "," + " " + "\n";

                // 13: AREAPAGO,N,12,2	
                if (objEnt.AREAPAGO > 0)
                    sQueryString += "\t" + "'" + Program.SetAreaDoubleStringValue(objEnt.AREAPAGO) + "'" + "," + " " + "\n";

                // 14: VALORPAGO,N,12,3	
                if (objEnt.VALORPAGO > 0)
                    sQueryString += "\t" + "'" + Program.SetPayDoubleStringValue(objEnt.VALORPAGO) + "'" + "," + " " + "\n";

                // 15: ESTADO,C,1	
                if (objEnt.Pay_State != PAG_ENTIDADE.PayState.UNDEFINED)
                    sQueryString += "\t" + "'" + objEnt.Pay_State_DB_Value + "'" + "," + " " + "\n";

                // 16: NOTAS,C,140	
                sQueryString += "\t" + "'" + objEnt.NOTAS.Trim() + "'" + "," + " " + "\n";

                // 17: PAGNBR,N,2,0
                sQueryString += "\t" + objEnt.PAGNBR + " " + " " + "\n";

                sQueryString += ") " + " " + "\n";
                #endregion  Values

                #endregion  Query

                using (OleDbConnection objOleDbConnection = new OleDbConnection(OLE_DB_ConnectionString))
                {
                    objOleDbConnection.Open();
                    using (OleDbCommand objOleDbCommand = new OleDbCommand())
                    {
                        objOleDbCommand.Connection = objOleDbConnection;
                        objOleDbCommand.CommandText = sQueryString;
                        if (objOleDbCommand.ExecuteNonQuery() != 1)
                            return -1;
                    }
                }

                return 1;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message + " -> " + "QUERY: " + sQueryString, Program.ErroType.EXCEPTION, true, false);
                return -1;
            }
        }

        /// <versions>14-03-2018(GesAMFC-v1.0.0.3)</versions>
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
        public Int64 Edit_ENTID(EntityTypeConfigs objEntityConfigs, PAG_ENTIDADE objEnt)
        {
            String sQueryString = String.Empty;
            try
            {
                String sDBF_File_Name = objEntityConfigs.DBF_File_Name;

                Set_OLE_DB_ConnectionString();
                if (String.IsNullOrEmpty(sDBF_File_Name))
                    OLE_DB_Settings();

                #region     Validate Data
                if (objEnt.ID < 1)
                {
                    String sError = objEntityConfigs.Entity_Desc_Single + " " + "ID" + " " + "inválido" + ": " + objEnt.ID;
                    MessageBox.Show(sError, "ID Inválido", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return -1;
                }
                if (objEnt.SOCIO < new AMFCMember().MinNumber || objEnt.SOCIO > new AMFCMember().MaxNumber)
                {
                    String sError = "Nº de Sócio inválido: " + objEnt.SOCIO;
                    MessageBox.Show(sError, "Sócio Inválido", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return -1;
                }
                if (objEnt.VALORPAGO <= 0)
                {
                    String sError = "A " + objEntityConfigs.Entity_Desc_Single + " tem de ter um valor monetário maior que zero!";
                    MessageBox.Show(sError, "Valor " + objEntityConfigs.Entity_Desc_Single + " Inválido", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return -1;
                }
                //.... verificar outros no futuro
                #endregion  Validate Data

                #region     Query
                sQueryString += "UPDATE " + sDBF_File_Name + " " + "\n";
                sQueryString += "SET" + " " + "\n";
                
                // 02: LISTACAIXA,C,20	
                if (!String.IsNullOrEmpty(objEnt.LISTACAIXA) && objEnt.LISTACAIXA.Trim().ToLower() != Program.DB_Not_Available.Trim().ToLower())
                    sQueryString += "\t" + "LISTACAIXA" + " = " + "'" + objEnt.LISTACAIXA.Trim().ToUpper() + "'" + "," + " " + "\n";

                // 03: LISTARECNU,C,20	
                if (!String.IsNullOrEmpty(objEnt.LISTARECNU) && objEnt.LISTARECNU.Trim().ToLower() != Program.DB_Not_Available.Trim().ToLower())
                    sQueryString += "\t" + "LISTARECNU" + " = " + "'" + objEnt.LISTARECNU.Trim().ToUpper() + "'" + "," + " " + "\n";

                // 04: IDLOTE,N,5,0	
                if (objEnt.IDLOTE > 0)
                    sQueryString += "\t" + "IDLOTE" + " = " + objEnt.IDLOTE + "," + " " + "\n";

                // 05: NUMLOTE,C,10	
                if (!String.IsNullOrEmpty(objEnt.NUMLOTE) && objEnt.NUMLOTE.Trim().ToLower() != Program.DB_Not_Available.Trim().ToLower())
                    sQueryString += "\t" + "NUMLOTE" + " = " + "'" + objEnt.NUMLOTE.Trim().ToUpper() + "'" + "," + " " + "\n";

                // 06: SOCIO,N,5,0	
                if (objEnt.SOCIO > 0)
                    sQueryString += "\t" + "SOCIO" + " = " + objEnt.SOCIO + "," + " " + "\n";

                // 07: NOME,C,70	
                if (!String.IsNullOrEmpty(objEnt.NOME) && objEnt.NOME.Trim().ToLower() != Program.DB_Not_Available.Trim().ToLower())
                    sQueryString += "\t" + "NOME" + " = " + "'" + objEnt.NOME.Trim().ToUpper() + "'" + "," + " " + "\n";

                // 08: ANO,N,4,0	
                if (Program.IsValidYear(objEnt.ANO))
                    sQueryString += "\t" + "ANO" + " = " + objEnt.ANO + "," + " " + "\n";

                // 09: MES,N,2,0	
                if (Program.IsValidMonth(objEnt.MES))
                    sQueryString += "\t" + "MES" + " = " + objEnt.MES + "," + " " + "\n";

                // 10: DATAPAG,D	
                if (Program.IsValidDateTime(objEnt.DATAPAG))
                    sQueryString += "\t" + "DATAPAG" + " = " + "'" + objEnt.DATAPAG.ToString(Program.Date_Format_String, CultureInfo.CurrentCulture) + "'" + "," + " " + "\n";

                // 11: DESIGNACAO,C,140	
                if (!String.IsNullOrEmpty(objEnt.DESIGNACAO) && objEnt.DESIGNACAO.Trim().ToLower() != Program.DB_Not_Available.Trim().ToLower())
                    sQueryString += "\t" + "DESIGNACAO" + " = " + "'" + objEnt.DESIGNACAO.Trim().ToUpper() + "'" + "," + " " + "\n";

                // 12: PRECOM2P,N,12,3	
                if (objEnt.PRECOM2P > 0)
                    sQueryString += "\t" + "PRECOM2P" + " = " + "'" + Program.SetPayDoubleStringValue(objEnt.PRECOM2P) + "'" + "," + " " + "\n";

                // 13: AREAPAGO,N,12,2	
                if (objEnt.AREAPAGO > 0)
                    sQueryString += "\t" + "AREAPAGO" + " = " + "'" + Program.SetAreaDoubleStringValue(objEnt.AREAPAGO) + "'" + "," + " " + "\n";

                // 14: VALORPAGO,N,12,3	
                if (objEnt.VALORPAGO > 0)
                    sQueryString += "\t" + "VALORPAGO" + " = " + "'" + Program.SetPayDoubleStringValue(objEnt.VALORPAGO) + "'" + "," + " " + "\n";

                // 15: ESTADO,C,1	
                if (objEnt.Pay_State != PAG_ENTIDADE.PayState.UNDEFINED)
                    sQueryString += "\t" + "ESTADO" + " = " + "'" + objEnt.Pay_State_DB_Value + "'" + "," + " " + "\n";

                // 16: NOTAS,C,140	
                sQueryString += "\t" + "NOTAS" + " = " + "'" + objEnt.NOTAS.Trim() + "'" + "," + " " + "\n";

                // 17: PAGNBR,N,2,0
                sQueryString += "\t" + "PAGNBR" + " = " + objEnt.PAGNBR + " " + " " + "\n";

                sQueryString += "WHERE " + "ID" + " = " + objEnt.ID + "\n"; // 01: ID,N,10,0
                #endregion  Query

                using (OleDbConnection objOleDbConnection = new OleDbConnection(OLE_DB_ConnectionString))
                {
                    objOleDbConnection.Open();
                    using (OleDbCommand objOleDbCommand = new OleDbCommand())
                    {
                        objOleDbCommand.Connection = objOleDbConnection;
                        objOleDbCommand.CommandText = sQueryString;
                        if (objOleDbCommand.ExecuteNonQuery() != 1)
                            return -1;
                    }
                }

                return 1;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message + " -> " + "QUERY: " + sQueryString, Program.ErroType.EXCEPTION, true, false);
                return -1;
            }
        }

        /// <versions>14-03-2018(GesAMFC-v1.0.0.3)</versions>
        public Int64 Del_ENTID(EntityTypeConfigs objEntityConfigs, PAG_ENTIDADE objEnt)
        {
            String sQueryString = String.Empty;
            try
            {
                String sDBF_File_Name = objEntityConfigs.DBF_File_Name;

                Set_OLE_DB_ConnectionString();
                if (String.IsNullOrEmpty(sDBF_File_Name))
                    OLE_DB_Settings();

                #region     Validate Data
                if (objEnt.ID < 1)
                {
                    String sError = objEntityConfigs.Entity_Desc_Single + " " + "ID" + " " + "inválido" + ": " + objEnt.ID;
                    MessageBox.Show(sError, "ID Inválido", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return -1;
                }
                #endregion  Validate Data

                sQueryString = "DELETE FROM " + sDBF_File_Name + " WHERE " + "ID" + " = " + objEnt.ID + ";" + "\n";
                using (OleDbConnection objOleDbConnection = new OleDbConnection(OLE_DB_ConnectionString))
                {
                    objOleDbConnection.Open();
                    using (OleDbCommand objOleDbCommand = new OleDbCommand())
                    {
                        objOleDbCommand.Connection = objOleDbConnection;
                        objOleDbCommand.CommandText = sQueryString;
                        if (objOleDbCommand.ExecuteNonQuery() != 1)
                            return -1;
                    }
                }

                return 1;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message + " -> " + "QUERY: " + sQueryString, Program.ErroType.EXCEPTION, true, false);
                return -1;
            }
        }

        /// <versions>04-03-2018(GesAMFC-v1.0.0.3)</versions>
        public Int32 Get_DB_ENTID_PAGNBR_Max_Number(EntityTypeConfigs objEntityConfigs, PAG_ENTIDADE objEnt)
        {
            String sQueryString = String.Empty;
            try
            {
                var iMaxNumber = -1;

                Set_OLE_DB_ConnectionString();
                //if (String.IsNullOrEmpty(LibAMFC.DBF_AMFC_SOCIO_FileName))
                //    OLE_DB_Settings();

                String sDBF_File_Name = objEntityConfigs.DBF_File_Name;

                String sColMaxName = "PAGNBR";

                using (OleDbConnection objOleDbConnection = new OleDbConnection(OLE_DB_ConnectionString))
                {
                    objOleDbConnection.Open();
                    using (OleDbCommand objOleDbCommand = new OleDbCommand())
                    {
                        objOleDbCommand.Connection = objOleDbConnection;

                        sQueryString = "SELECT COUNT(*) FROM " + sDBF_File_Name + " " + "\n";
                        sQueryString += "WHERE" + " " + "IDLOTE" + " = " + objEnt.IDLOTE + " " + "\n";
                        sQueryString += "AND" + " " + "SOCIO" + " = " + objEnt.SOCIO + " " + "\n";

                        objOleDbCommand.CommandText = sQueryString;
                        if (Convert.ToInt32(objOleDbCommand.ExecuteScalar()) <= 0)
                            return 0;

                        objOleDbCommand.CommandText = "SELECT MAX(" + sColMaxName + ") AS MaxId FROM " + sDBF_File_Name + ";";
                        iMaxNumber = Convert.ToInt32(objOleDbCommand.ExecuteScalar());
                    }
                }
                return iMaxNumber;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message + " -> " + "QUERY: " + sQueryString, Program.ErroType.EXCEPTION, true, false);
                return -1;
            }
        }

        /// <versions>04-03-2018(GesAMFC-v1.0.0.3)</versions>
        public Int32 Get_DB_ENTID_PAGTOTAL_Number(EntityTypeConfigs objEntityConfigs, PAG_ENTIDADE objEnt)
        {
            String sQueryString = String.Empty;
            try
            {
                Int32 iTotal = -1;

                Set_OLE_DB_ConnectionString();
                //if (String.IsNullOrEmpty(LibAMFC.DBF_AMFC_SOCIO_FileName))
                //    OLE_DB_Settings();

                String sDBF_File_Name = objEntityConfigs.DBF_File_Name;

                using (OleDbConnection objOleDbConnection = new OleDbConnection(OLE_DB_ConnectionString))
                {
                    objOleDbConnection.Open();
                    using (OleDbCommand objOleDbCommand = new OleDbCommand())
                    {
                        objOleDbCommand.Connection = objOleDbConnection;

                        sQueryString = "SELECT COUNT(*) FROM " + sDBF_File_Name + " " + "\n";
                        sQueryString += "WHERE" + " " + "IDLOTE" + " = " + objEnt.IDLOTE + " " + "\n";
                        sQueryString += "AND" + " " + "SOCIO" + " = " + objEnt.SOCIO + " " + "\n";

                        objOleDbCommand.CommandText = sQueryString;
                        iTotal = Convert.ToInt32(objOleDbCommand.ExecuteScalar());
                        if (iTotal <= 0)
                            return 0;                        
                    }
                }
                return iTotal;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message + " -> " + "QUERY: " + sQueryString, Program.ErroType.EXCEPTION, true, false);
                return -1;
            }
        }
        /// <versions>22-03-2018(GesAMFC-v1.0.0.3)</versions>
        public Int32 Get_Member_ENTID_Total_Payments(EntityTypeConfigs objEntityConfigs, Int64 lMemberNumber, Int64 lLoteId, Int32 iYear, Int32 iMonth)
        {
            try
            {
                Int32 iTotalPays = 0;

                Set_OLE_DB_ConnectionString();

                using (OleDbConnection objOleDbConnection = new OleDbConnection(OLE_DB_ConnectionString))
                {
                    objOleDbConnection.Open();
                    using (OleDbCommand objOleDbCommand = new OleDbCommand())
                    {
                        objOleDbCommand.Connection = objOleDbConnection;

                        String sDBF_FileName = objEntityConfigs.DBF_File_Name;
                        objOleDbCommand.CommandText = "SELECT COUNT(*) FROM " + sDBF_FileName + " " + "\n";
                        if (Convert.ToInt32(objOleDbCommand.ExecuteScalar()) <= 0)
                            return 0;

                        objOleDbCommand.CommandText = Get_Member_ENTID_TotalPago(objEntityConfigs, lMemberNumber, lLoteId, iYear, iMonth, "*", "COUNT");
                        using (OleDbDataReader objOleDbDataReader = objOleDbCommand.ExecuteReader())
                        {
                            objOleDbDataReader.Read();
                            if (objOleDbDataReader["TotalPagamentos"] != DBNull.Value)
                                iTotalPays = Convert.ToInt32(objOleDbDataReader["TotalPagamentos"]);
                            if (iTotalPays < 0)
                            {
                                StackFrame objStackFrame = new StackFrame();
                                String sErrorMsg = "Não foi possível obter Total Pagamnetos de " + objEntityConfigs.Entity_Desc_Single + " do Sócio com o Nº: " + lMemberNumber + " -> " + "QUERY: " + objOleDbCommand.CommandText;
                                Program.HandleError(objStackFrame.GetMethod().Name, sErrorMsg, Program.ErroType.ERROR, true, true);
                                objStackFrame = null;
                                return -1;
                            }
                        }
                    }
                }
                return iTotalPays;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return -1;
            }
        }
        
        /// <versions>22-03-2018(GesAMFC-v1.0.0.3)</versions>
        public Double Get_Member_ENTID_Total_Valor_Paid(EntityTypeConfigs objEntityConfigs, Int64 lMemberNumber, Int64 lLoteId, Int32 iYear, Int32 iMonth)
        {
            try
            {
                Double dbTotalPago = 0;

                Set_OLE_DB_ConnectionString();

                using (OleDbConnection objOleDbConnection = new OleDbConnection(OLE_DB_ConnectionString))
                {
                    objOleDbConnection.Open();
                    using (OleDbCommand objOleDbCommand = new OleDbCommand())
                    {
                        objOleDbCommand.Connection = objOleDbConnection;

                        String sDBF_FileName = objEntityConfigs.DBF_File_Name;
                        objOleDbCommand.CommandText = "SELECT COUNT(*) FROM " + sDBF_FileName + " " + "\n";
                        if (Convert.ToInt32(objOleDbCommand.ExecuteScalar()) <= 0)
                            return 0;

                        objOleDbCommand.CommandText = Get_Member_ENTID_TotalPago(objEntityConfigs, lMemberNumber, lLoteId, iYear, iMonth, "VALORPAGO", "SUM");
                        using (OleDbDataReader objOleDbDataReader = objOleDbCommand.ExecuteReader())
                        {
                            objOleDbDataReader.Read();
                            if (objOleDbDataReader["TotalPago"] != DBNull.Value)
                                dbTotalPago = Program.SetPayCurrencyEuroDoubleValue(Convert.ToDouble(objOleDbDataReader["TotalPago"]));
                            if (dbTotalPago < 0)
                            {
                                StackFrame objStackFrame = new StackFrame();
                                String sErrorMsg = "Não foi possível obter Total Valor Pago de " + objEntityConfigs.Entity_Desc_Single + " do Sócio com o Nº: " + lMemberNumber + " -> " + "QUERY: " + objOleDbCommand.CommandText;
                                Program.HandleError(objStackFrame.GetMethod().Name, sErrorMsg, Program.ErroType.ERROR, true, true);
                                objStackFrame = null;
                                return -1;
                            }
                        }
                    }
                }
                return dbTotalPago;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return -1;
            }
        }

        /// <versions>22-03-2018(GesAMFC-v1.0.0.3)</versions>
        public Double Get_Member_ENTID_Total_Area_Paid(EntityTypeConfigs objEntityConfigs, Int64 lMemberNumber, Int64 lLoteId, Int32 iYear, Int32 iMonth)
        {
            try
            {
                Double dbTotalPago = 0;

                Set_OLE_DB_ConnectionString();

                using (OleDbConnection objOleDbConnection = new OleDbConnection(OLE_DB_ConnectionString))
                {
                    objOleDbConnection.Open();
                    using (OleDbCommand objOleDbCommand = new OleDbCommand())
                    {
                        objOleDbCommand.Connection = objOleDbConnection;

                        String sDBF_FileName = objEntityConfigs.DBF_File_Name;
                        objOleDbCommand.CommandText = "SELECT COUNT(*) FROM " + sDBF_FileName + " " + "\n";
                        if (Convert.ToInt32(objOleDbCommand.ExecuteScalar()) <= 0)
                            return 0;

                        objOleDbCommand.CommandText = Get_Member_ENTID_TotalPago(objEntityConfigs, lMemberNumber, lLoteId, iYear, iMonth, "AREAPAGO", "SUM");
                        using (OleDbDataReader objOleDbDataReader = objOleDbCommand.ExecuteReader())
                        {
                            objOleDbDataReader.Read();
                            if (objOleDbDataReader["TotalPago"] != DBNull.Value)
                                dbTotalPago = Convert.ToDouble(objOleDbDataReader["TotalPago"]);
                            if (dbTotalPago < 0)
                            {
                                StackFrame objStackFrame = new StackFrame();
                                String sErrorMsg = "Não foi possível obter Total Área Paga de " + objEntityConfigs.Entity_Desc_Single + " do Sócio com o Nº: " + lMemberNumber + " -> " + "QUERY: " + objOleDbCommand.CommandText;
                                Program.HandleError(objStackFrame.GetMethod().Name, sErrorMsg, Program.ErroType.ERROR, true, true);
                                objStackFrame = null;
                                return -1;
                            }
                        }
                    }
                }
                return dbTotalPago;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return -1;
            }
        }

        /// <versions>22-03-2018(GesAMFC-v1.0.0.3)</versions>
        public String Get_Member_ENTID_TotalPago(EntityTypeConfigs objEntityConfigs, Int64 lMemberNumber, Int64 lLoteId, Int32 iYear, Int32 iMonth, String sDbFieldName, String sOperation)
        {
            try
            {
                String sQueryString = String.Empty;

                String sDBF_FileName = objEntityConfigs.DBF_File_Name;

                sQueryString += "SELECT" + " " + "\n";

                if (sOperation == "SUM")
                    sQueryString += sOperation + "(" + sDBF_FileName + "." + sDbFieldName + ")" + " AS " + "TotalPago" + " " + " " + "\n";
                else if (sOperation == "COUNT")
                    sQueryString += sOperation + "(" + "*" + ")" + " AS " + "TotalPagamentos" + " " + " " + "\n";

                sQueryString += "FROM" + " " + "\n";
                sQueryString += sDBF_FileName + " " + "\n";
                if (lMemberNumber >= new AMFCMember().MinNumber && lMemberNumber <= new AMFCMember().MaxNumber && lLoteId > 0)
                {
                    sQueryString += "WHERE" + " " + "\n";
                    sQueryString += sDBF_FileName + "." + "SOCIO" + " = " + lMemberNumber + "\n";
                    sQueryString += "AND" + " " + "\n";
                    sQueryString += sDBF_FileName + "." + "IDLOTE" + " = " + lLoteId + " " + "\n";
                    if (sOperation == "SUM")
                    {
                        sQueryString += "AND" + " " + "\n";
                        sQueryString += sDBF_FileName + "." + sDbFieldName + " " + "IS NOT NULL" + " " + "\n";
                        sQueryString += "AND" + " " + "\n";
                        sQueryString += sDBF_FileName + "." + sDbFieldName + " " + " > 0" + " " + "\n";
                    }
                    sQueryString += "AND" + " " + "\n";
                    sQueryString += sDBF_FileName + "." + "ID" + " " + "IS NOT NULL" + " " + "\n";
                }
                else if (Program.IsValidYear(iYear))
                {
                    sQueryString += "WHERE" + " " + "\n";
                    sQueryString += "ANO" + " = " + iYear + "\n";
                    if (Program.IsValidMonth(iMonth))
                    {
                        sQueryString += "AND" + " " + "\n";
                        sQueryString += "MES" + " = " + iMonth + "\n";
                    }
                }
                sQueryString += ";";

                return sQueryString;
            }
            catch
            {
                return "";
            }
        }

        /// <versions>08-12-2017(GesAMFC-v0.0.5.3)</versions>
        public LIST_PAG_ENTIDADE Get_ENTID_Pay_Open(EntityTypeConfigs objEntityConfigs, Int64 lMemberNumber, Int64 lLoteId, Int32 iYear)
        {
            try
            {
                LIST_PAG_ENTIDADE objListEntidades = new LIST_PAG_ENTIDADE();
                Set_OLE_DB_ConnectionString();

                AMFCMember objMember = new AMFCMember();
                using (OleDbConnection objOleDbConnection = new OleDbConnection(OLE_DB_ConnectionString))
                {
                    objOleDbConnection.Open();
                    using (OleDbCommand objOleDbCommand = new OleDbCommand())
                    {
                        objOleDbCommand.Connection = objOleDbConnection;
                        objOleDbCommand.CommandText = Set_ENTID_Pay_Open(objEntityConfigs, lMemberNumber, lLoteId, iYear);

                        objListEntidades = Get_Members_ENTID_DB_Data(objEntityConfigs, objOleDbCommand, false);
                        if (objListEntidades == null || objListEntidades.List == null)
                        {
                            StackFrame objStackFrame = new StackFrame();
                            String sErrorMsg = "Não foi possível obter a Lista de" + objEntityConfigs.Entity_Desc_Plural + " dos Sócios da Base de Dados!" + " -> " + "QUERY: " + objOleDbCommand.CommandText;
                            Program.HandleError(objStackFrame.GetMethod().Name, sErrorMsg, Program.ErroType.ERROR, true, true);
                            objStackFrame = null;
                            return null;
                        }
                    }
                }

                return objListEntidades;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return null;
            }
        }

        /// <versions>15-03-2018(GesAMFC-v1.0.0.3)</versions>
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
        public String Set_ENTID_Pay_Open(EntityTypeConfigs objEntityConfigs, Int64 lMemberNumber, Int64 lLoteId, Int32 iYear)
        {
            try
            {
                if (lMemberNumber < new AMFCMember().MinNumber || lMemberNumber > new AMFCMember().MaxNumber || lLoteId < 1)
                    return String.Empty;

                String sQueryString = String.Empty;

                String sDBF_FileName = objEntityConfigs.DBF_File_Name;

                sQueryString += "SELECT" + " " + "\n";
                sQueryString += sDBF_FileName + "." + "ID" + "," + " " + "\n"; /// 01: ID,N,10,0	
                sQueryString += sDBF_FileName + "." + "LISTACAIXA" + "," + " " + "\n"; /// 02: LISTACAIXA,C,20	
                sQueryString += sDBF_FileName + "." + "LISTARECNU" + "," + " " + "\n"; /// 03: LISTARECNU,C,20
                sQueryString += sDBF_FileName + "." + "IDLOTE" + "," + " " + "\n"; /// 04: IDLOTE,N,5,0
                sQueryString += sDBF_FileName + "." + "NUMLOTE" + "," + " " + "\n"; /// 05: NUMLOTE,C,10
                sQueryString += sDBF_FileName + "." + "SOCIO" + "," + " " + "\n"; /// 06: SOCIO,N,5,0	
                sQueryString += sDBF_FileName + "." + "NOME" + "," + " " + "\n"; /// 07: NOME,C,70	
                sQueryString += sDBF_FileName + "." + "ANO" + "," + " " + "\n"; /// 08: ANO,N,4,0	
                sQueryString += sDBF_FileName + "." + "MES" + "," + " " + "\n"; /// 09: MES,N,2,0	
                sQueryString += sDBF_FileName + "." + "DATAPAG" + "," + " " + "\n"; /// 10: DATAPAG,D	
                sQueryString += sDBF_FileName + "." + "DESIGNACAO" + "," + " " + "\n"; /// 11: DESIGNACAO,C,140	
                sQueryString += sDBF_FileName + "." + "PRECOM2P" + "," + " " + "\n"; /// 12: PRECOM2P,N,12,3	
                sQueryString += sDBF_FileName + "." + "AREAPAGO" + "," + " " + "\n"; /// 13: AREAPAGO,N,12,2	
                sQueryString += sDBF_FileName + "." + "VALORPAGO" + "," + " " + "\n"; /// 14: VALORPAGO,N,12,3	
                sQueryString += sDBF_FileName + "." + "ESTADO" + "," + " " + "\n"; /// 15: ESTADO,C,1	
                sQueryString += sDBF_FileName + "." + "NOTAS" + ", " + " " + "\n"; /// 16: NOTAS,C,140	
                sQueryString += sDBF_FileName + "." + "PAGNBR" + " " + " " + "\n"; /// 17: PAGNBR,N,2,0

                sQueryString += "FROM" + " " + "\n";
                sQueryString += sDBF_FileName + " " + "\n";
                if (lMemberNumber >= new AMFCMember().MinNumber && lMemberNumber <= new AMFCMember().MaxNumber && lLoteId > 0)
                {
                    sQueryString += "WHERE" + " " + "\n";
                    sQueryString += sDBF_FileName + "." + "SOCIO" + " = " + lMemberNumber + "\n";

                    sQueryString += "AND" + " " + "\n";
                    sQueryString += sDBF_FileName + "." + "IDLOTE" + " = " + lLoteId + "\n";

                    sQueryString += "AND" + " " + "\n";
                    sQueryString += sDBF_FileName + "." + "ESTADO" + " LIKE " + "'" + new AMFC_Entity().GetPay_StateDBvalue(AMFC_Entity.PayState.EM_PAGAMENTO) + "'" + " " + "\n";
                }
                else
                {
                    sQueryString += "ORDER BY" + " " + "\n";
                    sQueryString += sDBF_FileName + "." + "ANO" + "," + " " + "\n";
                    sQueryString += sDBF_FileName + "." + "MES" + "," + " " + "\n";
                    sQueryString += sDBF_FileName + "." + "SOCIO" + " " + " " + "\n";
                }
                sQueryString += ";";

                return sQueryString;
            }
            catch
            {
                return null;
            }
        }

        #endregion  Admin ENTID

        #region     LOTES

        /// <versions>28-02-2018(GesAMFC-v1.0.0.3)</versions>
        /// <remarks>
        /// IDLOTE,N,5,0	SOCNUM,N,5,0	SOCNOME,C,70	OLDNUM,N,5,0	OLDNOME,C,140	MORLOTE,C,140	INDEXLOTE,N,1,0	NUMLOTE,C,10	TOTALLOTES,N,2,0	TOTALFOGOS,N,2,0	AREALOTES,N,5,0	AREAPAGAR,N,5,0	SECTOR,C,6	CASA,C,1	GARAGEM,C,1	MUROS,C,1	POCO,C,1	FURO,C,1	SANEAMENTO,C,1	ELECTRICID,C,1	PROJECTO,C,1	ESCRITURA,C,1	FINANCAS,C,1	RESIDENCIA,C,1	AGREFAMIL,C,2	GAVETO,C,1	QUINTINHA,C,1	LADOMAIOR,C,4	HABCOLECT,C,1, ARECOMERC,C,5	OBSERVACAO,C,140	OBSERV2,C,140	OBSERV3,C,140	FICH01,C,140	FICH02,C,140	FICH03,C,140	PROCURADOR,C,140	MORPROC,C,140
        /// </remarks>
        public AMFCMemberLotes Get_List_Member_Lotes(Int64 lMemberNumber)
        {
            String sQueryString = String.Empty;
            try
            {
                AMFCMemberLotes objListLotes = new AMFCMemberLotes();
                Set_OLE_DB_ConnectionString();
                if (String.IsNullOrEmpty(LibAMFC.DBF_AMFC_SOCIO_FileName))
                    OLE_DB_Settings();
                sQueryString = "SELECT * FROM " + "DB_LOTES" + " WHERE " + "SOCNUM" + " = " + lMemberNumber;
                sQueryString += " ORDER BY " + "INDEXLOTE";
                using (OleDbConnection objOleDbConnection = new OleDbConnection(OLE_DB_ConnectionString))
                {
                    objOleDbConnection.Open();
                    using (OleDbCommand objOleDbCommand = new OleDbCommand())
                    {
                        objOleDbCommand.Connection = objOleDbConnection;
                        objOleDbCommand.CommandText = sQueryString;

                        objListLotes = Get_Member_DB_Lotes(objOleDbCommand, false);
                        if (objListLotes == null)
                        {
                            StackFrame objStackFrame = new StackFrame();
                            String sErrorMsg = "Não foi possível obter a Lista de Lotes do Sócio Nº: " + lMemberNumber + " -> " + "QUERY: " + sQueryString;
                            Program.HandleError(objStackFrame.GetMethod().Name, sErrorMsg, Program.ErroType.ERROR, true, true);
                            objStackFrame = null;
                            return null;
                        }
                        if (objListLotes.Lotes.Count == 0)
                            return null;
                    }
                }

                return objListLotes;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message + " -> " + "QUERY: " + sQueryString, Program.ErroType.EXCEPTION, true, false);
                return null;
            }
        }

        /// <versions>01-03-2018(GesAMFC-v1.0.0.3)</versions>
        /// <remarks>
        /// IDLOTE,N,5,0	SOCNUM,N,5,0	SOCNOME,C,70	OLDNUM,N,5,0	OLDNOME,C,140	MORLOTE,C,140	INDEXLOTE,N,1,0	NUMLOTE,C,10	TOTALLOTES,N,2,0	AREALOTES,N,5,0	AREAPAGAR,N,5,0	SECTOR,C,6	CASA,C,1	GARAGEM,C,1	MUROS,C,1	POCO,C,1	FURO,C,1	SANEAMENTO,C,1	ELECTRICID,C,1	PROJECTO,C,1	ESCRITURA,C,1	FINANCAS,C,1	RESIDENCIA,C,1	GAVETO,C,1	QUINTINHA,C,1	LADOMAIOR,C,4	HABCOLECT,C,1	TOTALFOGOS,N,2,0	ARECOMERC,C,5	OBSERVACAO,C,140	OBSERV2,C,140	OBSERV3,C,140	FICH01,C,140	FICH02,C,140	FICH03,C,140	PROCURADOR,C,140	MORPROC,C,140
        /// 01: IDLOTE,N,5,0	
        /// 02: SOCNUM,N,5,0	
        /// 03: SOCNOME,C,70	
        /// 04: OLDNUM,N,5,0	
        /// 05: OLDNOME,C,140	
        /// 06: MORLOTE,C,140	
        /// 07: INDEXLOTE,N,1,0	
        /// 08: NUMLOTE,C,10	
        /// 09: TOTALLOTES,N,2,0	
        /// 10: AREALOTES,N,5,0	
        /// 11: AREAPAGAR,N,5,0	
        /// 12: SECTOR,C,6	
        /// 13: CASA,C,1	
        /// 14: GARAGEM,C,1	
        /// 15: MUROS,C,1	
        /// 16: POCO,C,1	
        /// 17: FURO,C,1	
        /// 18: SANEAMENTO,C,1	
        /// 19: ELECTRICID,C,1	
        /// 20: PROJECTO,C,1	
        /// 21: ESCRITURA,C,1	
        /// 22: FINANCAS,C,1	
        /// 23: RESIDENCIA,C,1	
        /// 24: GAVETO,C,1	
        /// 25: QUINTINHA,C,1	
        /// 26: LADOMAIOR,C,4	
        /// 28: HABCOLECT,C,1	
        /// 29: TOTALFOGOS,N,2,0	
        /// 30: ARECOMERC,C,5	
        /// 31: OBSERVACAO,C,140	
        /// 32: OBSERV2,C,140	
        /// 33: OBSERV3,C,140	
        /// 34: FICH01,C,140	
        /// 35: FICH02,C,140	
        /// 36: FICH03,C,140	
        /// 37: PROCURADOR,C,140	
        /// 38: MORPROC,C,140
        /// </remarks>
        private AMFCMemberLotes Get_Member_DB_Lotes(OleDbCommand objOleDbCommand, Boolean bGetSingleRecord)
        {
            try
            {
                AMFCMemberLotes objLotes = new AMFCMemberLotes();
                using (OleDbDataReader objOleDbDataReader = objOleDbCommand.ExecuteReader())
                {
                    while (objOleDbDataReader.Read())
                    {
                        AMFCMemberLote objLote = new AMFCMemberLote();

                        if (objOleDbDataReader["IDLOTE"] == DBNull.Value)
                            continue;
                        if (objOleDbDataReader["SOCNUM"] == DBNull.Value)
                            continue;
                        if (objOleDbDataReader["NUMLOTE"] == DBNull.Value)
                            continue;

                        /// 01: IDLOTE,N,5,0	
                        if (objOleDbDataReader["IDLOTE"] != DBNull.Value) 
                            objLote.IDLOTE = Convert.ToInt64(objOleDbDataReader["IDLOTE"]);

                        /// 02: SOCNUM,N,5,0	
                        if (objOleDbDataReader["SOCNUM"] != DBNull.Value) 
                            objLote.SOCNUM = Convert.ToInt64(objOleDbDataReader["SOCNUM"]);

                        /// 03: SOCNOME,C,70	
                        if (objOleDbDataReader["SOCNOME"] != DBNull.Value)
                            objLote.SOCNOME = Program.EncodeStringToISO(objOleDbDataReader["SOCNOME"].ToString());

                        /// 04: OLDNUM,N,5,0	
                        if (objOleDbDataReader["OLDNUM"] != DBNull.Value)
                            objLote.OLDNUM = Convert.ToInt64(objOleDbDataReader["OLDNUM"]);

                        /// 05: OLDNOME,C,140	
                        if (objOleDbDataReader["OLDNOME"] != DBNull.Value)
                            objLote.OLDNOME = Program.EncodeStringToISO(objOleDbDataReader["OLDNOME"].ToString());

                        /// 06: MORLOTE,C,140	
                        if (objOleDbDataReader["MORLOTE"] != DBNull.Value)
                            objLote.MORLOTE = Program.EncodeStringToISO(objOleDbDataReader["MORLOTE"].ToString());

                        /// 07: INDEXLOTE,N,1,0	
                        if (objOleDbDataReader["INDEXLOTE"] != DBNull.Value)
                            objLote.INDEXLOTE = Convert.ToInt32(objOleDbDataReader["INDEXLOTE"]);

                        /// 08: NUMLOTE,C,10	
                        if (objOleDbDataReader["NUMLOTE"] != DBNull.Value)
                            objLote.NUMLOTE = Program.EncodeStringToISO(objOleDbDataReader["NUMLOTE"].ToString());

                        /// 09: TOTALLOTES,N,2,0	
                        if (objOleDbDataReader["TOTALLOTES"] != DBNull.Value)
                            objLote.TOTALLOTES = Convert.ToInt32(objOleDbDataReader["TOTALLOTES"]);

                        /// 10: AREALOTES,N,5,0	
                        if (objOleDbDataReader["AREALOTES"] != DBNull.Value)
                            objLote.AREALOTES = Convert.ToDouble(objOleDbDataReader["AREALOTES"]);
                        
                        /// 11: AREAPAGAR,N,5,0	
                        if (objOleDbDataReader["AREAPAGAR"] != DBNull.Value)
                            objLote.AREAPAGAR = Convert.ToDouble(objOleDbDataReader["AREAPAGAR"]);
                        if (objLote.AREAPAGAR <= 0 || objLote.AREALOTES > 0)
                            objLote.AREAPAGAR = objLote.AREALOTES;

                        /// 12: SECTOR,C,6	
                        if (objOleDbDataReader["SECTOR"] != DBNull.Value)
                            objLote.SECTOR = objOleDbDataReader["SECTOR"].ToString();

                        /// 13: CASA,C,1	
                        if (objOleDbDataReader["CASA"] != DBNull.Value)
                            objLote.CASA = Program.ConvertYesOrNoToBoolean(objOleDbDataReader["CASA"].ToString());

                        /// 14: GARAGEM,C,1	
                        if (objOleDbDataReader["GARAGEM"] != DBNull.Value)
                            objLote.GARAGEM = Program.ConvertYesOrNoToBoolean(objOleDbDataReader["GARAGEM"].ToString());

                        /// 15: MUROS,C,1	
                        if (objOleDbDataReader["MUROS"] != DBNull.Value)
                            objLote.MUROS = Program.ConvertYesOrNoToBoolean(objOleDbDataReader["MUROS"].ToString());
                        
                        /// 16: POCO,C,1	
                        if (objOleDbDataReader["POCO"] != DBNull.Value)
                            objLote.POCO = Program.ConvertYesOrNoToBoolean(objOleDbDataReader["POCO"].ToString());

                        /// 17: FURO,C,1	
                        if (objOleDbDataReader["FURO"] != DBNull.Value)
                            objLote.FURO = Program.ConvertYesOrNoToBoolean(objOleDbDataReader["SANEAMENTO"].ToString());

                        /// 18: SANEAMENTO,C,1	
                        if (objOleDbDataReader["SANEAMENTO"] != DBNull.Value)
                            objLote.SANEAMENTO = Program.ConvertYesOrNoToBoolean(objOleDbDataReader["FURO"].ToString());

                        /// 19: ELECTRICID,C,1	
                        if (objOleDbDataReader["ELECTRICID"] != DBNull.Value)
                            objLote.ELECTRICID = Program.ConvertYesOrNoToBoolean(objOleDbDataReader["ELECTRICID"].ToString());

                        /// 20: PROJECTO,C,1	
                        if (objOleDbDataReader["PROJECTO"] != DBNull.Value)
                            objLote.PROJECTO = Program.ConvertYesOrNoToBoolean(objOleDbDataReader["PROJECTO"].ToString());

                        /// 21: ESCRITURA,C,1	
                        if (objOleDbDataReader["ESCRITURA"] != DBNull.Value)
                            objLote.ESCRITURA = Program.ConvertYesOrNoToBoolean(objOleDbDataReader["ESCRITURA"].ToString());

                        /// 22: FINANCAS,C,1	
                        if (objOleDbDataReader["FINANCAS"] != DBNull.Value)
                            objLote.FINANCAS = Program.ConvertYesOrNoToBoolean(objOleDbDataReader["FINANCAS"].ToString());

                        /// 23: RESIDENCIA,C,1	
                        if (objOleDbDataReader["RESIDENCIA"] != DBNull.Value)
                            objLote.RESIDENCIA = Program.ConvertYesOrNoToBoolean(objOleDbDataReader["RESIDENCIA"].ToString());

                        /// 24: GAVETO,C,1	
                        if (objOleDbDataReader["GAVETO"] != DBNull.Value)
                            objLote.GAVETO = Program.ConvertYesOrNoToBoolean(objOleDbDataReader["GAVETO"].ToString());

                        /// 25: QUINTINHA,C,1
                        if (objOleDbDataReader["QUINTINHA"] != DBNull.Value)
                            objLote.QUINTINHA = Program.ConvertYesOrNoToBoolean(objOleDbDataReader["QUINTINHA"].ToString());

                        /// 26: LADOMAIOR,C,4	
                        if (objOleDbDataReader["LADOMAIOR"] != DBNull.Value)
                            objLote.LADOMAIOR = objOleDbDataReader["LADOMAIOR"].ToString();

                        /// 28: HABCOLECT,C,1	
                        if (objOleDbDataReader["HABCOLECT"] != DBNull.Value)
                            objLote.HABCOLECT = Program.ConvertYesOrNoToBoolean(objOleDbDataReader["HABCOLECT"].ToString());

                        /// 29: TOTALFOGOS,N,2,0	
                        if (objOleDbDataReader["TOTALFOGOS"] != DBNull.Value)
                            objLote.TOTALFOGOS = Convert.ToInt32(objOleDbDataReader["TOTALFOGOS"]);

                        /// 30: ARECOMERC,C,5	
                        if (objOleDbDataReader["ARECOMERC"] != DBNull.Value)
                            objLote.ARECOMERC = Program.ConvertYesOrNoToBoolean(objOleDbDataReader["ARECOMERC"].ToString());

                        /// 31: OBSERVACAO,C,140	
                        if (objOleDbDataReader["OBSERVACAO"] != DBNull.Value)
                            objLote.OBSERVACAO = Program.EncodeStringToISO(objOleDbDataReader["OBSERVACAO"].ToString());
                                                
                        /// 32: OBSERV2,C,140	
                        if (objOleDbDataReader["OBSERV2"] != DBNull.Value)
                            objLote.OBSERV2 = Program.EncodeStringToISO(objOleDbDataReader["OBSERV2"].ToString());

                        /// 33: OBSERV3,C,140	
                        if (objOleDbDataReader["OBSERV3"] != DBNull.Value)
                            objLote.OBSERV3 = Program.EncodeStringToISO(objOleDbDataReader["OBSERV3"].ToString());

                        /// 34: FICH01,C,140	
                        if (objOleDbDataReader["FICH01"] != DBNull.Value)
                            objLote.FICH01 = Program.EncodeStringToISO(objOleDbDataReader["FICH01"].ToString());

                        /// 35: FICH02,C,140	
                        if (objOleDbDataReader["FICH02"] != DBNull.Value)
                            objLote.FICH02 = Program.EncodeStringToISO(objOleDbDataReader["FICH02"].ToString());

                        /// 36: FICH03,C,140	
                        if (objOleDbDataReader["FICH03"] != DBNull.Value)
                            objLote.FICH03 = Program.EncodeStringToISO(objOleDbDataReader["FICH03"].ToString());

                        /// 37: PROCURADOR,C,140	
                        if (objOleDbDataReader["PROCURADOR"] != DBNull.Value)
                            objLote.PROCURADOR = Program.EncodeStringToISO(objOleDbDataReader["PROCURADOR"].ToString());

                        /// 38: MORPROC,C,140
                        if (objOleDbDataReader["MORPROC"] != DBNull.Value)
                            objLote.MORPROC = Program.EncodeStringToISO(objOleDbDataReader["MORPROC"].ToString());

                        objLotes.Lotes.Add(objLote);

                        if (bGetSingleRecord)
                            return objLotes;
                    }
                }
                return objLotes;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return null;
            }
        }

        /// <versions>01-03-2018(GesAMFC-v1.0.0.3)</versions>
        /// <remarks>
        /// IDLOTE,N,5,0	SOCNUM,N,5,0	SOCNOME,C,70	OLDNUM,N,5,0	OLDNOME,C,140	MORLOTE,C,140	INDEXLOTE,N,1,0	NUMLOTE,C,10	TOTALLOTES,N,2,0	AREALOTES,N,5,0	AREAPAGAR,N,5,0	SECTOR,C,6	CASA,C,1	GARAGEM,C,1	MUROS,C,1	POCO,C,1	FURO,C,1	SANEAMENTO,C,1	ELECTRICID,C,1	PROJECTO,C,1	ESCRITURA,C,1	FINANCAS,C,1	RESIDENCIA,C,1	GAVETO,C,1	QUINTINHA,C,1	LADOMAIOR,C,4	HABCOLECT,C,1	TOTALFOGOS,N,2,0	ARECOMERC,C,5	OBSERVACAO,C,140	OBSERV2,C,140	OBSERV3,C,140	FICH01,C,140	FICH02,C,140	FICH03,C,140	PROCURADOR,C,140	MORPROC,C,140
        /// 01: IDLOTE,N,5,0	
        /// 02: SOCNUM,N,5,0	
        /// 03: SOCNOME,C,70	
        /// 04: OLDNUM,N,5,0	
        /// 05: OLDNOME,C,140	
        /// 06: MORLOTE,C,140	
        /// 07: INDEXLOTE,N,1,0	
        /// 08: NUMLOTE,C,10	
        /// 09: TOTALLOTES,N,2,0	
        /// 10: AREALOTES,N,5,0	
        /// 11: AREAPAGAR,N,5,0	
        /// 12: SECTOR,C,6	
        /// 13: CASA,C,1	
        /// 14: GARAGEM,C,1	
        /// 15: MUROS,C,1	
        /// 16: POCO,C,1	
        /// 17: FURO,C,1	
        /// 18: SANEAMENTO,C,1	
        /// 19: ELECTRICID,C,1	
        /// 20: PROJECTO,C,1	
        /// 21: ESCRITURA,C,1	
        /// 22: FINANCAS,C,1	
        /// 23: RESIDENCIA,C,1	
        /// 24: GAVETO,C,1	
        /// 25: QUINTINHA,C,1	
        /// 26: LADOMAIOR,C,4	
        /// 28: HABCOLECT,C,1	
        /// 29: TOTALFOGOS,N,2,0	
        /// 30: ARECOMERC,C,5	
        /// 31: OBSERVACAO,C,140	
        /// 32: OBSERV2,C,140	
        /// 33: OBSERV3,C,140	
        /// 34: FICH01,C,140	
        /// 35: FICH02,C,140	
        /// 36: FICH03,C,140	
        /// 37: PROCURADOR,C,140	
        /// 38: MORPROC,C,140
        /// </remarks>
        public Boolean Save_Member_DB_Lote(AMFCMemberLote objLote)
        {
            String sQueryString = String.Empty;
            try
            {
                AMFCMembers objListMembers = new AMFCMembers();

                Set_OLE_DB_ConnectionString();
                if (String.IsNullOrEmpty(LibAMFC.DBF_AMFC_SOCIO_FileName))
                    OLE_DB_Settings();

                #region     Delete Previous Exixting Lote ID Info
                if (objLote.IDLOTE > 0)
                {
                    if (!Del_Member_DB_Lote(objLote))
                    {
                        String sError = "Não foi possível atualizar os dados do Lote Nº: " + objLote.NUMLOTE + " (ID=" + objLote.IDLOTE + ") " + " do Sócio: " + objLote.SOCNOME + " Nº: " + objLote.SOCNUM;
                        MessageBox.Show(sError, "Erro Operação", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
                #endregion  Delete Previous Exixting Lote ID Info

                #region     Set New Lote ID
                if (objLote.IDLOTE < 1)
                    objLote.IDLOTE = Get_Lotes_IDs_Max_Number() + 1;
                #endregion  Set New Lote ID

                #region     Check if Member already exist
                if (Lote_Already_Exist(objLote))
                {
                    String sWarning = "Já existe o Lote Nº: " + objLote.NUMLOTE + " (ID=" + objLote.IDLOTE + ") " + " do Sócio: " + objLote.SOCNOME + " Nº: " + objLote.SOCNUM;
                    MessageBox.Show(sWarning, "Lote já existente", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                #endregion  Check if Member already exist

                #region     Trim Observations
                Int32 iEachObsMaxLen = 139;
                String sNotas = objLote.OBSERVACAO;
                if (sNotas.Length >= iEachObsMaxLen)
                {
                    String sObs01 = "", sObs02 = "", sObs03 = "";
                    sObs01 = sNotas.Substring(0, iEachObsMaxLen - 1);
                    sObs02 = sNotas.Replace(sObs01, " ");
                    sNotas = sObs02;
                    if (sObs02.Length >= iEachObsMaxLen)
                    {
                        sObs02 = sNotas.Substring(0, iEachObsMaxLen - 1);
                        sObs03 = sNotas.Replace(sObs02, " ");
                        sNotas = sObs03;
                        if (sObs03.Length >= iEachObsMaxLen)
                            sObs03 = sNotas.Substring(0, iEachObsMaxLen - 1);
                    }
                    objLote.OBSERVACAO = sObs01;
                    objLote.OBSERV2 = sObs02;
                    objLote.OBSERV3 = sObs03;
                }
                #endregion  Trim Observations
                                
                #region     Query

                sQueryString += "INSERT INTO " + "DB_LOTES" + " " + "\n"; 
                sQueryString += "( " + " " + "\n";
                
                #region     Columns
                sQueryString += "\t" + "IDLOTE" + "," + " " + "\n"; /// 01: IDLOTE,N,5,0	
                sQueryString += "\t" + "SOCNUM" + "," + " " + "\n"; /// 02: SOCNUM,N,5,0	
                sQueryString += "\t" + "SOCNOME" + "," + " " + "\n"; /// 03: SOCNOME,C,70	
                sQueryString += "\t" + "OLDNUM" + "," + " " + "\n"; /// 04: OLDNUM,N,5,0	
                sQueryString += "\t" + "OLDNOME" + "," + " " + "\n"; /// 05: OLDNOME,C,140	
                sQueryString += "\t" + "MORLOTE" + "," + " " + "\n"; /// 06: MORLOTE,C,140	
                sQueryString += "\t" + "INDEXLOTE" + "," + " " + "\n"; /// 07: INDEXLOTE,N,1,0	
                sQueryString += "\t" + "NUMLOTE" + "," + " " + "\n"; /// 08: NUMLOTE,C,10	
                sQueryString += "\t" + "TOTALLOTES" + "," + " " + "\n"; /// 09: TOTALLOTES,N,2,0	
                sQueryString += "\t" + "AREALOTES" + "," + " " + "\n"; /// 10: AREALOTES,N,5,0	
                sQueryString += "\t" + "AREAPAGAR" + "," + " " + "\n"; /// 11: AREAPAGAR,N,5,0	
                sQueryString += "\t" + "SECTOR" + "," + " " + "\n"; /// 12: SECTOR,C,6	
                sQueryString += "\t" + "CASA" + "," + " " + "\n"; /// 13: CASA,C,1	
                sQueryString += "\t" + "GARAGEM" + "," + " " + "\n"; /// 14: GARAGEM,C,1	
                sQueryString += "\t" + "MUROS" + "," + " " + "\n"; /// 15: MUROS,C,1
                sQueryString += "\t" + "POCO" + "," + " " + "\n"; /// 16: POCO,C,1	
                sQueryString += "\t" + "FURO" + "," + " " + "\n"; /// 17: FURO,C,1	
                sQueryString += "\t" + "SANEAMENTO" + "," + " " + "\n"; /// 18: SANEAMENTO,C,1	
                sQueryString += "\t" + "ELECTRICID" + "," + " " + "\n"; /// 19: ELECTRICID,C,1	
                sQueryString += "\t" + "PROJECTO" + "," + " " + "\n"; /// 20: PROJECTO,C,1
                sQueryString += "\t" + "ESCRITURA" + "," + " " + "\n"; /// 21: ESCRITURA,C,1	
                sQueryString += "\t" + "FINANCAS" + "," + " " + "\n"; /// 22: FINANCAS,C,1	
                sQueryString += "\t" + "RESIDENCIA" + "," + " " + "\n"; /// 23: RESIDENCIA,C,1	
                sQueryString += "\t" + "GAVETO" + "," + " " + "\n"; /// 24: GAVETO,C,1	
                sQueryString += "\t" + "QUINTINHA" + "," + " " + "\n"; /// 25: QUINTINHA,C,1	
                sQueryString += "\t" + "LADOMAIOR" + "," + " " + "\n"; /// 26: LADOMAIOR,C,4	
                sQueryString += "\t" + "HABCOLECT" + "," + " " + "\n"; /// 28: HABCOLECT,C,1	
                sQueryString += "\t" + "TOTALFOGOS" + "," + " " + "\n"; /// 29: TOTALFOGOS,N,2,0	
                sQueryString += "\t" + "ARECOMERC" + "," + " " + "\n"; /// 30: ARECOMERC,C,5	
                sQueryString += "\t" + "OBSERVACAO" + "," + " " + "\n"; /// 31: OBSERVACAO,C,140
                sQueryString += "\t" + "OBSERV2" + "," + " " + "\n"; /// 32: OBSERV2,C,140	
                sQueryString += "\t" + "OBSERV3" + "," + " " + "\n"; /// 33: OBSERV3,C,140	
                sQueryString += "\t" + "FICH01" + "," + " " + "\n"; /// 34: FICH01,C,140	
                sQueryString += "\t" + "FICH02" + "," + " " + "\n"; /// 35: FICH02,C,140	
                sQueryString += "\t" + "FICH03" + "," + " " + "\n"; /// 36: FICH03,C,140	
                sQueryString += "\t" + "PROCURADOR" + "," + " " + "\n"; /// 37: PROCURADOR,C,140
                sQueryString += "\t" + "MORPROC" + " " + " " + "\n"; /// 38: MORPROC,C,140
                #endregion  Columns

                sQueryString += ") " + " " + "\n";
                sQueryString += "VALUES" + " " + "\n";
                sQueryString += "( " + " " + "\n";

                #region     Values
                sQueryString += "\t" + objLote.IDLOTE + "," + " " + "\n"; /// 01: IDLOTE,N,5,0	
                sQueryString += "\t" + objLote.SOCNUM + "," + " " + "\n";  /// 02: SOCNUM,N,5,0	
                sQueryString += "\t" + "'" + objLote.SOCNOME.Trim().ToUpper() + "'" + "," + " " + "\n"; /// 03: SOCNOME,C,70	
                sQueryString += "\t" + objLote.OLDNUM + "," + " " + "\n"; /// 04: OLDNUM,N,5,0	
                sQueryString += "\t" + "'" + objLote.OLDNOME.Trim().ToUpper() + "'" + "," + " " + "\n"; /// 05: OLDNOME,C,140	
                sQueryString += "\t" + "'" + objLote.MORLOTE.Trim().ToUpper() + "'" + "," + " " + "\n"; /// 06: MORLOTE,C,140	
                sQueryString += "\t" + objLote.INDEXLOTE + "," + " " + "\n"; /// 07: INDEXLOTE,N,1,0	
                sQueryString += "\t" + "'" + objLote.NUMLOTE.Trim().ToUpper() + "'" + "," + " " + "\n"; /// 08: NUMLOTE,C,10	 
                sQueryString += "\t" + (objLote.TOTALLOTES > 0 ? objLote.TOTALLOTES : 1) + "," + " " + "\n"; /// 09: TOTALLOTES,N,2,0
                sQueryString += "\t" + (objLote.AREALOTES > 0 ? objLote.AREALOTES : 0) + "," + " " + "\n"; /// 10: AREALOTES,N,5,0	
                sQueryString += "\t" + (objLote.AREAPAGAR > 0 ? objLote.AREAPAGAR : 0) + "," + " " + "\n"; /// 11: AREAPAGAR,N,5,0	
                sQueryString += "\t" + "'" + objLote.SECTOR.Trim().ToUpper() + "'" + "," + " " + "\n"; /// 12: SECTOR,C,6	
                sQueryString += "\t" + "'" + Program.ConvertBooleanToYesOrNo(objLote.CASA).ToUpper() + "'" + "," + " " + "\n"; /// 13: CASA,C,1
                sQueryString += "\t" + "'" + Program.ConvertBooleanToYesOrNo(objLote.GARAGEM).ToUpper() + "'" + "," + " " + "\n"; /// 14: GARAGEM,C,1	
                sQueryString += "\t" + "'" + Program.ConvertBooleanToYesOrNo(objLote.MUROS).ToUpper() + "'" + "," + " " + "\n"; /// 15: MUROS,C,1	
                sQueryString += "\t" + "'" + Program.ConvertBooleanToYesOrNo(objLote.POCO).ToUpper() + "'" + "," + " " + "\n"; /// 16: POCO,C,1	
                sQueryString += "\t" + "'" + Program.ConvertBooleanToYesOrNo(objLote.FURO).ToUpper() + "'" + "," + " " + "\n"; /// 17: FURO,C,1
                sQueryString += "\t" + "'" + Program.ConvertBooleanToYesOrNo(objLote.SANEAMENTO).ToUpper() + "'" + "," + " " + "\n"; /// 18: SANEAMENTO,C,1	
                sQueryString += "\t" + "'" + Program.ConvertBooleanToYesOrNo(objLote.ELECTRICID).ToUpper() + "'" + "," + " " + "\n"; /// 19: ELECTRICID,C,1	
                sQueryString += "\t" + "'" + Program.ConvertBooleanToYesOrNo(objLote.PROJECTO).ToUpper() + "'" + "," + " " + "\n"; /// 20: PROJECTO,C,1	
                sQueryString += "\t" + "'" + Program.ConvertBooleanToYesOrNo(objLote.ESCRITURA).ToUpper() + "'" + "," + " " + "\n"; /// 21: ESCRITURA,C,1	
                sQueryString += "\t" + "'" + Program.ConvertBooleanToYesOrNo(objLote.FINANCAS).ToUpper() + "'" + "," + " " + "\n"; /// 22: FINANCAS,C,1	
                sQueryString += "\t" + "'" + Program.ConvertBooleanToYesOrNo(objLote.RESIDENCIA).ToUpper() + "'" + "," + " " + "\n"; /// 23: RESIDENCIA,C,1	
                sQueryString += "\t" + "'" + Program.ConvertBooleanToYesOrNo(objLote.GAVETO).ToUpper() + "'" + "," + " " + "\n"; /// 24: GAVETO,C,1	
                sQueryString += "\t" + "'" + Program.ConvertBooleanToYesOrNo(objLote.QUINTINHA).ToUpper() + "'" + "," + " " + "\n"; /// 25: QUINTINHA,C,1	
                sQueryString += "\t" + "'" + objLote.LADOMAIOR.Trim().ToUpper() + "'" + "," + " " + "\n"; /// 26: LADOMAIOR,C,4	
                sQueryString += "\t" + "'" + Program.ConvertBooleanToYesOrNo(objLote.HABCOLECT).ToUpper() + "'" + "," + " " + "\n"; /// 28: HABCOLECT,C,1	
                sQueryString += "\t" + (objLote.TOTALFOGOS > 0 ? objLote.TOTALFOGOS : 1) + "," + " " + "\n"; /// 29: TOTALFOGOS,N,2,0	    
                sQueryString += "\t" + "'" + Program.ConvertBooleanToYesOrNo(objLote.ARECOMERC).ToUpper() + "'" + "," + " " + "\n"; /// 30: ARECOMERC,C,5	
                sQueryString += "\t" + "'" + objLote.OBSERVACAO.Trim() + "'" + "," + " " + "\n"; /// 31: OBSERVACAO,C,140	
                sQueryString += "\t" + "'" + objLote.OBSERV2.Trim() + "'" + "," + " " + "\n"; /// 32: OBSERV2,C,140	
                sQueryString += "\t" + "'" + objLote.OBSERV3.Trim() + "'" + "," + " " + "\n"; /// 33: OBSERV3,C,140	
                sQueryString += "\t" + "'" + objLote.FICH01.Trim() + "'" + "," + " " + "\n"; /// 34: FICH01,C,140	
                sQueryString += "\t" + "'" + objLote.FICH02.Trim() + "'" + "," + " " + "\n"; /// 35: FICH02,C,140	
                sQueryString += "\t" + "'" + objLote.FICH03.Trim() + "'" + "," + " " + "\n"; /// 36: FICH03,C,140	
                sQueryString += "\t" + "'" + objLote.PROCURADOR.Trim().ToUpper() + "'" + "," + " " + "\n"; /// 37: PROCURADOR,C,140	
                sQueryString += "\t" + "'" + objLote.MORPROC.Trim().ToUpper() + "'" + " " + " " + "\n"; /// 38: MORPROC,C,140
                #endregion  Values

                sQueryString += ") " + " " + "\n";
                
                #endregion  Query

                using (OleDbConnection objOleDbConnection = new OleDbConnection(OLE_DB_ConnectionString))
                {
                    objOleDbConnection.Open();
                    using (OleDbCommand objOleDbCommand = new OleDbCommand())
                    {
                        objOleDbCommand.Connection = objOleDbConnection;
                        objOleDbCommand.CommandText = sQueryString;
                        if (objOleDbCommand.ExecuteNonQuery() != 1)
                            return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message + " -> " + "QUERY: " + sQueryString, Program.ErroType.EXCEPTION, true, false);
                return false;
            }
        }

        /// <versions>01-03-2018(GesAMFC-v1.0.0.3)</versions>
        public Boolean Del_Member_DB_Lote(AMFCMemberLote objLote)
        {
            String sQueryString = String.Empty;
            try
            {
                AMFCMembers objListMembers = new AMFCMembers();

                Set_OLE_DB_ConnectionString();
                if (String.IsNullOrEmpty(LibAMFC.DBF_AMFC_SOCIO_FileName))
                    OLE_DB_Settings();

                #region     Query
                sQueryString += "DELETE FROM " + "DB_LOTES" + " " + "\n";
                sQueryString += "WHERE" + " " + "IDLOTE" + " = " + objLote.IDLOTE + " " + "\n";
                sQueryString += "AND"   + " " + "SOCNUM" + " = " + objLote.SOCNUM + " " + "\n";
                #endregion  Query

                using (OleDbConnection objOleDbConnection = new OleDbConnection(OLE_DB_ConnectionString))
                {
                    objOleDbConnection.Open();
                    using (OleDbCommand objOleDbCommand = new OleDbCommand())
                    {
                        objOleDbCommand.Connection = objOleDbConnection;
                        objOleDbCommand.CommandText = sQueryString;
                        if (objOleDbCommand.ExecuteNonQuery() != 1)
                            return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message + " -> " + "QUERY: " + sQueryString, Program.ErroType.EXCEPTION, true, false);
                return false;
            }
        }

        /// <versions>01-03-2018(GesAMFC-v1.0.0.3)</versions>
        public Boolean Lote_Already_Exist(AMFCMemberLote objLote)
        {
            String sQueryString = String.Empty;
            try
            {
                AMFCMembers objListMembers = new AMFCMembers();

                Set_OLE_DB_ConnectionString();
                if (String.IsNullOrEmpty(LibAMFC.DBF_AMFC_SOCIO_FileName))
                    OLE_DB_Settings();

                #region     Query
                sQueryString += "SELECT COUNT(*) FROM " + "DB_LOTES" + " " + "\n";
                sQueryString += "WHERE" + " " + "\n";
                sQueryString += "SOCNUM" + " = " + objLote.SOCNUM + " " + "\n";
                sQueryString += "AND" + " " + "(";
                if (objLote.IDLOTE > 0)
                    sQueryString += "IDLOTE" + " = " + objLote.IDLOTE + " OR " + " " + "\n";
                sQueryString += "NUMLOTE" + " = " + "'" + objLote.NUMLOTE + "'" + ")" + " " + "\n";
                sQueryString += "; ";
                #endregion  Query

                using (OleDbConnection objOleDbConnection = new OleDbConnection(OLE_DB_ConnectionString))
                {
                    objOleDbConnection.Open();
                    using (OleDbCommand objOleDbCommand = new OleDbCommand())
                    {
                        objOleDbCommand.Connection = objOleDbConnection;
                        objOleDbCommand.CommandText = sQueryString;
                        if (Convert.ToInt32(objOleDbCommand.ExecuteScalar()) > 0)
                            return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message + " -> " + "QUERY: " + sQueryString, Program.ErroType.EXCEPTION, true, false);
                return true;
            }
        }

        /// <versions>01-03-2018(GesAMFC-v1.0.0.3)</versions>
        public Int32 Get_Lotes_IDs_Max_Number()
        {
            String sQueryString = String.Empty;
            try
            {
                var iMaxNumber = -1;

                String sDBf_Filename = "DB_LOTES";

                Set_OLE_DB_ConnectionString();
                if (String.IsNullOrEmpty(LibAMFC.DBF_AMFC_SOCIO_FileName))
                    OLE_DB_Settings();

                using (OleDbConnection objOleDbConnection = new OleDbConnection(OLE_DB_ConnectionString))
                {
                    objOleDbConnection.Open();
                    using (OleDbCommand objOleDbCommand = new OleDbCommand())
                    {
                        objOleDbCommand.Connection = objOleDbConnection;

                        objOleDbCommand.CommandText = "SELECT COUNT(*) FROM " + sDBf_Filename + " " + "\n";
                        if (Convert.ToInt32(objOleDbCommand.ExecuteScalar()) <= 0)
                            return 0;

                        objOleDbCommand.CommandText = "SELECT MAX(" + "IDLOTE" + ") AS MaxId FROM " + sDBf_Filename + ";"; ;
                        iMaxNumber = Convert.ToInt32(objOleDbCommand.ExecuteScalar());
                    }
                }
                return iMaxNumber;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message + " -> " + "QUERY: " + sQueryString, Program.ErroType.EXCEPTION, true, false);
                return -1;
            }
        }

        #endregion  LOTES

        #region     CC INFRA

        /// <versions>02-03-2018(GesAMFC-v1.0.0.3)</versions>
        public AMFC_ContaCorrente_INFRA Get_Member_ContaCorrente_INFRA(Int64 lMemberNumber, Int64 lMemberLoteId)
        {
            String sQueryString = String.Empty;
            try
            {
                AMFC_ContaCorrente_INFRA objCCInfra = new AMFC_ContaCorrente_INFRA();
                Set_OLE_DB_ConnectionString();
                if (String.IsNullOrEmpty(LibAMFC.DBF_AMFC_SOCIO_FileName))
                    OLE_DB_Settings();
                sQueryString = "SELECT * FROM " + "CC_INFRA" + " WHERE " + "SOCNUM" + " = " + lMemberNumber + " AND " + "IDLOTE" + " = " + lMemberLoteId;
                using (OleDbConnection objOleDbConnection = new OleDbConnection(OLE_DB_ConnectionString))
                {
                    objOleDbConnection.Open();
                    using (OleDbCommand objOleDbCommand = new OleDbCommand())
                    {
                        objOleDbCommand.Connection = objOleDbConnection;
                        objOleDbCommand.CommandText = sQueryString;

                        objCCInfra = Get_Member_CC_Lote_INFRA(objOleDbCommand, true);
                        if (objCCInfra == null)
                        {
                            StackFrame objStackFrame = new StackFrame();
                            String sErrorMsg = "Não foi possível obter a Conta Corrente de " + "Infraestruturas" + " do Lote ID=" + lMemberLoteId + " do Sócio Nº: " + lMemberNumber + " -> " + "QUERY: " + sQueryString;
                            Program.HandleError(objStackFrame.GetMethod().Name, sErrorMsg, Program.ErroType.ERROR, true, true);
                            objStackFrame = null;
                            return null;
                        }
                    }
                }

                return objCCInfra;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message + " -> " + "QUERY: " + sQueryString, Program.ErroType.EXCEPTION, true, false);
                return null;
            }
        }

        /// <versions>07-03-2018(GesAMFC-v1.0.0.3)</versions>
        /// <remarks>
        /// ID,N,10,0	SOCNUM,N,5,0	SOCNOME,C,70	IDLOTE,N,5,0	NUMLOTE,C,10	NUMPAG,N,2,0	AREA,N,12,2	AREAPAGAR,N,12,2	PRECOM2,N,12,3  VALORESCUD,N,12,3    VALORPAGAR,N,12,3	DATA,D	PRECOM2P,N,12,3	    AREAPAGO,N,12,2	    VALORESCUD,N,12,3	VALORPAGO,N,12,3	NOTASPAGO,C,140	PRECOM2F,N,12,3	AREAFALTA,N,12,2	VALORFALTA,N,12,3	NOTASFALTA,C,140	ESTADOLIQ,C,50	NOTASLIQ,C,140	
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
        /// 14: VALORPAGO,N,12,3	
        /// 15: NOTASPAGO,C,140	
        /// 16: PRECOM2F,N,12,3	
        /// 17: AREAFALTA,N,12,2	
        /// 18: VALORFALTA,N,12,3	
        /// 19: NOTASFALTA,C,140	
        /// 20: ESTADOLIQ,C,50	
        /// 21: NOTASLIQ,C,140	
        /// </remarks>
        private AMFC_ContaCorrente_INFRA Get_Member_CC_Lote_INFRA(OleDbCommand objOleDbCommand, Boolean bGetSingleRecord)
        {
            try
            {
                AMFC_ContaCorrente_INFRA objCCInfra = new AMFC_ContaCorrente_INFRA();
                using (OleDbDataReader objOleDbDataReader = objOleDbCommand.ExecuteReader())
                {
                    while (objOleDbDataReader.Read())
                    {
                        if (objOleDbDataReader["ID"] == DBNull.Value)
                            continue;
                        if (objOleDbDataReader["SOCNUM"] == DBNull.Value)
                            continue;
                        if (objOleDbDataReader["IDLOTE"] == DBNull.Value)
                            continue;
                        if (objOleDbDataReader["NUMLOTE"] == DBNull.Value)
                            continue;
	
                        /// 01: ID,N,10,0	
                        if (objOleDbDataReader["ID"] != DBNull.Value)
                            objCCInfra.ID = Convert.ToInt64(objOleDbDataReader["ID"]);

                        /// 02: SOCNUM,N,5,0
                        if (objOleDbDataReader["SOCNUM"] != DBNull.Value)
                            objCCInfra.SOCNUM = Convert.ToInt64(objOleDbDataReader["SOCNUM"]);

                        /// 03: SOCNOME,C,70	
                        if (objOleDbDataReader["SOCNOME"] != DBNull.Value)
                            objCCInfra.SOCNOME = Convert.ToString(objOleDbDataReader["SOCNOME"]);

                        /// 04: IDLOTE,N,5,0	
                        if (objOleDbDataReader["IDLOTE"] != DBNull.Value)
                            objCCInfra.IDLOTE = Convert.ToInt64(objOleDbDataReader["IDLOTE"]);

                        /// 05: NUMLOTE,C,10	
                        if (objOleDbDataReader["NUMLOTE"] != DBNull.Value)
                            objCCInfra.NUMLOTE = Convert.ToString(objOleDbDataReader["NUMLOTE"]);

                        /// 06: NUMPAG,N,2,0	
                        if (objOleDbDataReader["NUMPAG"] != DBNull.Value)
                            objCCInfra.NUMPAG = Convert.ToInt32(objOleDbDataReader["NUMPAG"]);
                                                          
                        /// 07: AREA,N,12,2	
                        if (objOleDbDataReader["AREA"] != DBNull.Value)
                            objCCInfra.AREA = Convert.ToDouble(objOleDbDataReader["AREA"]);

                        /// 08: AREAPAGAR,N,12,2
                        if (objOleDbDataReader["AREAPAGAR"] != DBNull.Value)
                            objCCInfra.AREAPAGAR = Convert.ToDouble(objOleDbDataReader["AREAPAGAR"]);

                        /// 09: PRECOM2,N,12,3	
                        if (objOleDbDataReader["PRECOM2"] != DBNull.Value)
                            objCCInfra.PRECOM2 = Convert.ToDouble(objOleDbDataReader["PRECOM2"]);

                        /// 10: VALORPAGAR,N,12,3	
                        if (objOleDbDataReader["VALORPAGAR"] != DBNull.Value)
                            objCCInfra.VALORPAGAR = Convert.ToDouble(objOleDbDataReader["VALORPAGAR"]);

                        /// 11: DATA,D	
                        if (objOleDbDataReader["DATA"] != DBNull.Value)
                            objCCInfra.DATA = Program.ConvertToValidDateTime(objOleDbDataReader["DATA"].ToString().Trim());

                        /// 12: PRECOM2P,N,12,3	
                        if (objOleDbDataReader["PRECOM2P"] != DBNull.Value)
                            objCCInfra.PRECOM2P = Convert.ToDouble(objOleDbDataReader["PRECOM2P"]);

                        /// 13: AREAPAGO,N,12,2	
                        if (objOleDbDataReader["AREAPAGO"] != DBNull.Value)
                            objCCInfra.AREAPAGO = Convert.ToDouble(objOleDbDataReader["AREAPAGO"]);

                        //VALORESCUD,N,12,3
                        if (objOleDbDataReader["VALORESCUD"] != DBNull.Value)
                            objCCInfra.VALORESCUD = Convert.ToDouble(objOleDbDataReader["VALORESCUD"]);
                        
                        /// 14: VALORPAGO,N,12,3	
                        if (objOleDbDataReader["VALORPAGO"] != DBNull.Value)
                            objCCInfra.VALORPAGO = Convert.ToDouble(objOleDbDataReader["VALORPAGO"]);

                        /// 15: NOTASPAGO,C,140	
                        if (objOleDbDataReader["NOTASPAGO"] != DBNull.Value)
                            objCCInfra.NOTASPAGO = Convert.ToString(objOleDbDataReader["NOTASPAGO"]);

                        /// 16: PRECOM2F,N,12,3	
                        if (objOleDbDataReader["PRECOM2F"] != DBNull.Value)
                            objCCInfra.PRECOM2F = Convert.ToDouble(objOleDbDataReader["PRECOM2F"]);

                        /// 17: AREAFALTA,N,12,2	
                        if (objOleDbDataReader["AREAFALTA"] != DBNull.Value)
                            objCCInfra.AREAFALTA = Convert.ToDouble(objOleDbDataReader["AREAFALTA"]);

                        /// 18: VALORFALTA,N,12,3	
                        if (objOleDbDataReader["VALORFALTA"] != DBNull.Value)
                            objCCInfra.VALORFALTA = Convert.ToDouble(objOleDbDataReader["VALORFALTA"]);

                        /// 19: NOTASFALTA,C,140	
                        if (objOleDbDataReader["NOTASFALTA"] != DBNull.Value)
                            objCCInfra.NOTASFALTA = Convert.ToString(objOleDbDataReader["NOTASFALTA"]);

                        /// 20: ESTADOLIQ,C,50	
                        if (objOleDbDataReader["ESTADOLIQ"] != DBNull.Value)
                            objCCInfra.ESTADOLIQ = Convert.ToString(objOleDbDataReader["ESTADOLIQ"]);

                        /// 21: NOTASLIQ,C,140
                        if (objOleDbDataReader["NOTASLIQ"] != DBNull.Value)
                            objCCInfra.NOTASLIQ = Convert.ToString(objOleDbDataReader["NOTASLIQ"]);

                        if (bGetSingleRecord)
                            return objCCInfra;
                    }
                }
                return objCCInfra;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return null;
            }
        }

        /// <versions>07-03-2018(GesAMFC-v1.0.0.3)</versions>
        /// <remarks>
        /// ID,N,10,0	SOCNUM,N,5,0	SOCNOME,C,70	IDLOTE,N,5,0	NUMLOTE,C,10	NUMPAG,N,2,0	AREA,N,12,2	AREAPAGAR,N,12,2	PRECOM2,N,12,3  VALORPAGAR,N,12,3	DATA,D	PRECOM2P,N,12,3	    AREAPAGO,N,12,2	    VALORESCUD,N,12,3	VALORPAGO,N,12,3	NOTASPAGO,C,140	PRECOM2F,N,12,3	AREAFALTA,N,12,2	VALORFALTA,N,12,3	NOTASFALTA,C,140	ESTADOLIQ,C,50	NOTASLIQ,C,140	
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
        /// 14: VALORPAGO,N,12,3	
        /// 15: NOTASPAGO,C,140	
        /// 16: PRECOM2F,N,12,3	
        /// 17: AREAFALTA,N,12,2	
        /// 18: VALORFALTA,N,12,3	
        /// 19: NOTASFALTA,C,140	
        /// 20: ESTADOLIQ,C,50	
        /// 21: NOTASLIQ,C,140	
        /// </remarks>
        public Boolean Save_DB_Lote_INFRA(AMFC_ContaCorrente_INFRA objCCInfra)
        {
            String sQueryString = String.Empty;
            try
            {
                Set_OLE_DB_ConnectionString();
                if (String.IsNullOrEmpty(LibAMFC.DBF_AMFC_SOCIO_FileName))
                    OLE_DB_Settings();

                #region     Delete Previous Existing Lote INFRA ID Info
                if (objCCInfra.IDLOTE > 0)
                {
                    if (!Del_DB_Lote_INFRA(objCCInfra))
                    {
                        String sError = "Não foi possível atualizar os dados do Lote Nº: " + objCCInfra.NUMLOTE + " (ID=" + objCCInfra.IDLOTE + ") " + " do Sócio: " + objCCInfra.SOCNOME + " Nº: " + objCCInfra.SOCNUM;
                        MessageBox.Show(sError, "Erro Operação", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
                #endregion  Delete Previous Existing Lote INFRA ID Info

                #region     Set New ID
                if (objCCInfra.ID < 1)
                    objCCInfra.ID = Get_Lote_INFRA_IDs_Max_Number() + 1;
                #endregion  Set New ID

                #region     Check if already exist
                if (Lote_INFRA_Already_Exist(objCCInfra))
                {
                    String sWarning = "Já existe um Registo de " + "Infraestruturas" + " para o Lote Nº: " + objCCInfra.NUMLOTE + " (ID=" + objCCInfra.IDLOTE + ") " + " do Sócio: " + objCCInfra.SOCNOME + " Nº: " + objCCInfra.SOCNUM;
                    MessageBox.Show(sWarning, "Lote já existente", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                #endregion  Check if already exist

                #region     Trim Observations
                Int32 iEachObsMaxLen = 139;
                if (objCCInfra.NOTASPAGO.Trim().Length > iEachObsMaxLen)
                    objCCInfra.NOTASPAGO = objCCInfra.NOTASPAGO.Substring(0, iEachObsMaxLen - 1);
                if (objCCInfra.NOTASFALTA.Trim().Length > iEachObsMaxLen)
                    objCCInfra.NOTASFALTA = objCCInfra.NOTASFALTA.Substring(0, iEachObsMaxLen - 1);
                #endregion  Trim Observations

                #region     Query

                sQueryString += "INSERT INTO " + "CC_INFRA" + " " + "\n";
                sQueryString += "( " + " " + "\n";
                
                #region     Columns
                sQueryString += "\t" + "ID" + "," + " " + "\n"; /// 01: ID,N,10,0	
                sQueryString += "\t" + "SOCNUM" + "," + " " + "\n"; /// 02: SOCNUM,N,5,0	
                sQueryString += "\t" + "SOCNOME" + "," + " " + "\n"; /// 03: SOCNOME,C,70
                sQueryString += "\t" + "IDLOTE" + "," + " " + "\n"; /// 04: IDLOTE,N,5,0	
                sQueryString += "\t" + "NUMLOTE" + "," + " " + "\n"; /// 05: NUMLOTE,C,10	
                sQueryString += "\t" + "NUMPAG" + "," + " " + "\n"; /// 06: NUMPAG,N,2,0	
                sQueryString += "\t" + "AREA" + "," + " " + "\n"; /// 07: AREA,N,12,2	
                sQueryString += "\t" + "AREAPAGAR" + "," + " " + "\n"; /// 08: AREAPAGAR,N,12,2
                sQueryString += "\t" + "PRECOM2" + "," + " " + "\n"; /// 09: PRECOM2,N,12,3	
                sQueryString += "\t" + "VALORPAGAR" + "," + " " + "\n"; /// 10: VALORPAGAR,N,12,3	
                sQueryString += "\t" + "DATA" + "," + " " + "\n"; /// 11: DATA,D	
                sQueryString += "\t" + "PRECOM2P" + "," + " " + "\n"; /// 12: PRECOM2P,N,12,3	
                sQueryString += "\t" + "AREAPAGO" + "," + " " + "\n"; /// 13: AREAPAGO,N,12,2	
                sQueryString += "\t" + "VALORESCUD" + "," + " " + "\n"; //VALORESCUD,N,12,3
                sQueryString += "\t" + "VALORPAGO" + "," + " " + "\n"; /// 14: VALORPAGO,N,12,3	
                sQueryString += "\t" + "NOTASPAGO" + "," + " " + "\n"; /// 15: NOTASPAGO,C,140	
                sQueryString += "\t" + "PRECOM2F" + "," + " " + "\n"; /// 16: PRECOM2F,N,12,3	
                sQueryString += "\t" + "AREAFALTA" + "," + " " + "\n"; /// 17: AREAFALTA,N,12,2	
                sQueryString += "\t" + "VALORFALTA" + "," + " " + "\n"; /// 18: VALORFALTA,N,12,3	
                sQueryString += "\t" + "NOTASFALTA" + "," + " " + "\n"; /// 19: NOTASFALTA,C,140	
                sQueryString += "\t" + "ESTADOLIQ" + "," + " " + "\n"; /// 20: ESTADOLIQ,C,50	
                sQueryString += "\t" + "NOTASLIQ" + " " + " " + "\n"; /// 21: NOTASLIQ,C,140	
                #endregion  Columns

                sQueryString += ") " + " " + "\n";
                sQueryString += "VALUES" + " " + "\n";
                sQueryString += "( " + " " + "\n";
                
                #region     Values
                sQueryString += "\t" + objCCInfra.ID + "," + " " + "\n"; /// 01: ID,N,10,0	
                sQueryString += "\t" + objCCInfra.SOCNUM + "," + " " + "\n";  /// 02: SOCNUM,N,5,0	
                sQueryString += "\t" + "'" + objCCInfra.SOCNOME.Trim().ToUpper() + "'" + "," + " " + "\n"; /// 03: SOCNOME,C,70	
                sQueryString += "\t" + objCCInfra.IDLOTE + "," + " " + "\n"; /// 04: IDLOTE,N,5,0	
                sQueryString += "\t" + "'" +  objCCInfra.NUMLOTE + "'" + "," + " " + "\n"; /// 05: NUMLOTE,C,10	
                sQueryString += "\t" + ((objCCInfra.NUMPAG > 0) ? objCCInfra.NUMPAG : 1) + "," + " " + "\n"; /// 06: NUMPAG,N,2,0	
                sQueryString += "\t" + "'" + ((objCCInfra.AREA > 0) ? Program.SetAreaDoubleStringValue(objCCInfra.AREA) : Program.Default_Area_Double_String) + "'" + "," + " " + "\n"; /// 07: AREA,N,12,2	
                sQueryString += "\t" + "'" + ((objCCInfra.AREAPAGAR > 0) ? Program.SetAreaDoubleStringValue(objCCInfra.AREAPAGAR) : Program.Default_Area_Double_String) + "'" + "," + " " + "\n"; /// 08: AREAPAGAR,N,12,2	
                sQueryString += "\t" + "'" + ((objCCInfra.PRECOM2 > 0) ? Program.SetPayDoubleStringValue(objCCInfra.PRECOM2) : Program.Default_Pay_Double_String) + "'" + "," + " " + "\n"; /// 09: PRECOM2,N,12,3	
                sQueryString += "\t" + "'" + ((objCCInfra.VALORPAGAR > 0) ? Program.SetPayDoubleStringValue(objCCInfra.VALORPAGAR) : Program.Default_Pay_Double_String) + "'" + "," + " " + "\n"; /// 10: VALORPAGAR,N,12,3	
                sQueryString += "\t" + "'" + (Program.IsValidDateTime(objCCInfra.DATA) ? objCCInfra.DATA.ToString(Program.DBF_Insert_Date_Format_String) : Program.Default_Date_Str) + "'" + "," + " " + "\n"; /// 11: DATA,D	

                sQueryString += "\t" + "'" + ((objCCInfra.PRECOM2P > 0) ? Program.SetPayDoubleStringValue(objCCInfra.PRECOM2P) : Program.Default_Pay_Double_String) + "'" + "," + " " + "\n"; /// 12: PRECOM2P,N,12,3	
                sQueryString += "\t" + "'" + ((objCCInfra.AREAPAGO > 0) ? Program.SetAreaDoubleStringValue(objCCInfra.AREAPAGO) : Program.Default_Area_Double_String) + "'" + "," + " " + "\n"; /// 13: AREAPAGO,N,12,2			
                sQueryString += "\t" + "'" + ((objCCInfra.VALORESCUD > 0) ? Program.SetPayDoubleStringValue(objCCInfra.VALORESCUD) : Program.Default_Pay_Double_String) + "'" + "," + " " + "\n"; //VALORESCUD,N,12,3	
                sQueryString += "\t" + "'" + ((objCCInfra.VALORPAGO > 0) ? Program.SetPayDoubleStringValue(objCCInfra.VALORPAGO) : Program.Default_Pay_Double_String) + "'" + "," + " " + "\n"; /// 14: VALORPAGO,N,12,3			
                sQueryString += "\t" + "'" + objCCInfra.NOTASPAGO.Trim() + "'" + ", " + " " + "\n"; /// 15: NOTASPAGO,C,140	

                sQueryString += "\t" + "'" + ((objCCInfra.PRECOM2F > 0) ? Program.SetPayDoubleStringValue(objCCInfra.PRECOM2F) : Program.Default_Pay_Double_String) + "'" + "," + " " + "\n"; /// 16: PRECOM2F,N,12,3	
                sQueryString += "\t" + "'" + ((objCCInfra.AREAFALTA > 0) ? Program.SetAreaDoubleStringValue(objCCInfra.AREAFALTA) : Program.Default_Area_Double_String) + "'" + "," + " " + "\n"; /// 17: AREAFALTA,N,12,2		
                sQueryString += "\t" + "'" + ((objCCInfra.VALORFALTA > 0) ? Program.SetPayDoubleStringValue(objCCInfra.VALORFALTA) : Program.Default_Pay_Double_String) + "'" + "," + " " + "\n"; /// 18: VALORFALTA,N,12,3		
                sQueryString += "\t" + "'" + objCCInfra.NOTASFALTA.Trim() + "'" + ", " + " " + "\n"; /// 19: NOTASFALTA,C,140	

                sQueryString += "\t" + "'" + objCCInfra.ESTADOLIQ.Trim() + "'" + ", " + " " + "\n"; /// 20: ESTADOLIQ,C,50	
                sQueryString += "\t" + "'" + objCCInfra.NOTASLIQ.Trim() + "'" + " " + " " + "\n"; /// 21: NOTASLIQ,C,140	
                #endregion  Values

                sQueryString += ") " + " " + "\n";

                #endregion  Query

                using (OleDbConnection objOleDbConnection = new OleDbConnection(OLE_DB_ConnectionString))
                {
                    objOleDbConnection.Open();
                    using (OleDbCommand objOleDbCommand = new OleDbCommand())
                    {
                        objOleDbCommand.Connection = objOleDbConnection;
                        objOleDbCommand.CommandText = sQueryString;
                        if (objOleDbCommand.ExecuteNonQuery() != 1)
                            return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message + " -> " + "QUERY: " + sQueryString, Program.ErroType.EXCEPTION, true, false);
                return false;
            }
        }

        /// <versions>04-03-2018(GesAMFC-v1.0.0.3)</versions>
        public Boolean Del_DB_Lote_INFRA(AMFC_ContaCorrente_INFRA objCCInfra)
        {
            String sQueryString = String.Empty;
            try
            {
                AMFCMembers objListMembers = new AMFCMembers();

                Set_OLE_DB_ConnectionString();
                if (String.IsNullOrEmpty(LibAMFC.DBF_AMFC_SOCIO_FileName))
                    OLE_DB_Settings();

                #region     Query
                sQueryString += "DELETE FROM " + "CC_INFRA" + " " + "\n";
                sQueryString += "WHERE" + " " + "IDLOTE" + " = " + objCCInfra.IDLOTE + " " + "\n";
                sQueryString += "AND" + " " + "SOCNUM" + " = " + objCCInfra.SOCNUM + " " + "\n";
                #endregion  Query

                using (OleDbConnection objOleDbConnection = new OleDbConnection(OLE_DB_ConnectionString))
                {
                    objOleDbConnection.Open();
                    using (OleDbCommand objOleDbCommand = new OleDbCommand())
                    {
                        objOleDbCommand.Connection = objOleDbConnection;
                        objOleDbCommand.CommandText = sQueryString;
                        if (objOleDbCommand.ExecuteNonQuery() < 0)
                            return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message + " -> " + "QUERY: " + sQueryString, Program.ErroType.EXCEPTION, true, false);
                return false;
            }
        }

        /// <versions>04-03-2018(GesAMFC-v1.0.0.3)</versions>
        public Boolean Lote_INFRA_Already_Exist(AMFC_ContaCorrente_INFRA objCCInfra)
        {
            String sQueryString = String.Empty;
            try
            {
                AMFCMembers objListMembers = new AMFCMembers();

                Set_OLE_DB_ConnectionString();
                if (String.IsNullOrEmpty(LibAMFC.DBF_AMFC_SOCIO_FileName))
                    OLE_DB_Settings();

                #region     Query
                sQueryString += "SELECT COUNT(*) FROM " + "CC_INFRA" + " " + "\n";
                sQueryString += "WHERE" + " " + "\n";
                sQueryString += "SOCNUM" + " = " + objCCInfra.SOCNUM + " " + "\n";
                sQueryString += "AND" + " " + "(";
                if (objCCInfra.IDLOTE > 0)
                    sQueryString += "IDLOTE" + " = " + objCCInfra.IDLOTE + " OR " + " " + "\n";
                sQueryString += "NUMLOTE" + " = " + "'" + objCCInfra.NUMLOTE + "'" + ")" + " " + "\n";
                sQueryString += "; ";
                #endregion  Query

                using (OleDbConnection objOleDbConnection = new OleDbConnection(OLE_DB_ConnectionString))
                {
                    objOleDbConnection.Open();
                    using (OleDbCommand objOleDbCommand = new OleDbCommand())
                    {
                        objOleDbCommand.Connection = objOleDbConnection;
                        objOleDbCommand.CommandText = sQueryString;
                        if (Convert.ToInt32(objOleDbCommand.ExecuteScalar()) > 0)
                            return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message + " -> " + "QUERY: " + sQueryString, Program.ErroType.EXCEPTION, true, false);
                return true;
            }
        }

        /// <versions>04-03-2018(GesAMFC-v1.0.0.3)</versions>
        public Int32 Get_Lote_INFRA_IDs_Max_Number()
        {
            String sQueryString = String.Empty;
            try
            {
                var iMaxNumber = -1;

                Set_OLE_DB_ConnectionString();
                if (String.IsNullOrEmpty(LibAMFC.DBF_AMFC_SOCIO_FileName))
                    OLE_DB_Settings();
                using (OleDbConnection objOleDbConnection = new OleDbConnection(OLE_DB_ConnectionString))
                {
                    objOleDbConnection.Open();
                    using (OleDbCommand objOleDbCommand = new OleDbCommand())
                    {
                        objOleDbCommand.Connection = objOleDbConnection;

                        objOleDbCommand.CommandText = "SELECT COUNT(*) FROM " + "CC_INFRA" + " " + "\n";
                        if (Convert.ToInt32(objOleDbCommand.ExecuteScalar()) <= 0)
                            return 0;

                        objOleDbCommand.CommandText = "SELECT MAX(" + "ID" + ") AS MaxId FROM " + "CC_INFRA" + ";";
                        iMaxNumber = Convert.ToInt32(objOleDbCommand.ExecuteScalar());
                    }
                }
                return iMaxNumber;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message + " -> " + "QUERY: " + sQueryString, Program.ErroType.EXCEPTION, true, false);
                return -1;
            }
        }

        #endregion  CC INFRA

        #region     CC CEDEN

        /// <versions>02-03-2018(GesAMFC-v1.0.0.3)</versions>
        public AMFC_ContaCorrente_CEDEN Get_Member_ContaCorrente_CEDEN(Int64 lMemberNumber, Int64 lMemberLoteId)
        {
            String sQueryString = String.Empty;
            try
            {
                AMFC_ContaCorrente_CEDEN objCCCEDEN = new AMFC_ContaCorrente_CEDEN();
                Set_OLE_DB_ConnectionString();
                if (String.IsNullOrEmpty(LibAMFC.DBF_AMFC_SOCIO_FileName))
                    OLE_DB_Settings();
                sQueryString = "SELECT * FROM " + "CC_CEDEN" + " WHERE " + "SOCNUM" + " = " + lMemberNumber + " AND " + "IDLOTE" + " = " + lMemberLoteId;
                using (OleDbConnection objOleDbConnection = new OleDbConnection(OLE_DB_ConnectionString))
                {
                    objOleDbConnection.Open();
                    using (OleDbCommand objOleDbCommand = new OleDbCommand())
                    {
                        objOleDbCommand.Connection = objOleDbConnection;
                        objOleDbCommand.CommandText = sQueryString;

                        objCCCEDEN = Get_Member_CC_Lote_CEDEN(objOleDbCommand, true);
                        if (objCCCEDEN == null)
                        {
                            StackFrame objStackFrame = new StackFrame();
                            String sErrorMsg = "Não foi possível obter a Conta Corrente de " + "Cedências" + " do Lote ID=" + lMemberLoteId + " do Sócio Nº: " + lMemberNumber + " -> " + "QUERY: " + sQueryString;
                            Program.HandleError(objStackFrame.GetMethod().Name, sErrorMsg, Program.ErroType.ERROR, true, true);
                            objStackFrame = null;
                            return null;
                        }
                    }
                }

                return objCCCEDEN;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message + " -> " + "QUERY: " + sQueryString, Program.ErroType.EXCEPTION, true, false);
                return null;
            }
        }

        /// <versions>07-03-2018(GesAMFC-v1.0.0.3)</versions>
        /// <remarks>
        /// ID,N,10,0	SOCNUM,N,5,0	SOCNOME,C,70	IDLOTE,N,5,0	NUMLOTE,C,10	NUMPAG,N,2,0	AREA,N,12,2	    CEDERPERC,N,4,2	    AREAPAGAR,N,12,2	PRECOM2,N,12,3  VALORPAGAR,N,12,3	DATA,D	PRECOM2P,N,12,3	    AREAPAGO,N,12,2	    VALORESCUD,N,12,3	VALORPAGO,N,12,3	NOTASPAGO,C,140	PRECOM2F,N,12,3	AREAFALTA,N,12,2	VALORFALTA,N,12,3	NOTASFALTA,C,140	ESTADOLIQ,C,50	NOTASLIQ,C,140	
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
        private AMFC_ContaCorrente_CEDEN Get_Member_CC_Lote_CEDEN(OleDbCommand objOleDbCommand, Boolean bGetSingleRecord)
        {
            try
            {
                AMFC_ContaCorrente_CEDEN objCCCEDEN = new AMFC_ContaCorrente_CEDEN();
                using (OleDbDataReader objOleDbDataReader = objOleDbCommand.ExecuteReader())
                {
                    while (objOleDbDataReader.Read())
                    {
                        if (objOleDbDataReader["ID"] == DBNull.Value)
                            continue;
                        if (objOleDbDataReader["SOCNUM"] == DBNull.Value)
                            continue;
                        if (objOleDbDataReader["IDLOTE"] == DBNull.Value)
                            continue;
                        if (objOleDbDataReader["NUMLOTE"] == DBNull.Value)
                            continue;
                                                
                        /// 01: ID,N,10,0	
                        if (objOleDbDataReader["ID"] != DBNull.Value)
                            objCCCEDEN.ID = Convert.ToInt64(objOleDbDataReader["ID"]);

                        /// 02: SOCNUM,N,5,0	
                        if (objOleDbDataReader["SOCNUM"] != DBNull.Value)
                            objCCCEDEN.SOCNUM = Convert.ToInt64(objOleDbDataReader["SOCNUM"]);

                        /// 03: SOCNOME,C,70	
                        if (objOleDbDataReader["SOCNOME"] != DBNull.Value)
                            objCCCEDEN.SOCNOME = Convert.ToString(objOleDbDataReader["SOCNOME"]);

                        /// 04: IDLOTE,N,5,0	
                        if (objOleDbDataReader["IDLOTE"] != DBNull.Value)
                            objCCCEDEN.IDLOTE = Convert.ToInt64(objOleDbDataReader["IDLOTE"]);

                        /// 05: NUMLOTE,C,10	
                        if (objOleDbDataReader["NUMLOTE"] != DBNull.Value)
                            objCCCEDEN.NUMLOTE = Convert.ToString(objOleDbDataReader["NUMLOTE"]);

                        /// 06: NUMPAG,N,2,0	
                        if (objOleDbDataReader["NUMPAG"] != DBNull.Value)
                            objCCCEDEN.NUMPAG = Convert.ToInt32(objOleDbDataReader["NUMPAG"]);

                        /// 07: AREA,N,12,2	    
                        if (objOleDbDataReader["AREA"] != DBNull.Value)
                            objCCCEDEN.AREA = Convert.ToDouble(objOleDbDataReader["AREA"]);

                        /// 08: CEDERPERC,N,4,2	    
                        if (objOleDbDataReader["CEDERPERC"] != DBNull.Value)
                            objCCCEDEN.CEDERPERC = Convert.ToDouble(objOleDbDataReader["CEDERPERC"]);

                        /// 09: AREAPAGAR,N,12,2	
                        if (objOleDbDataReader["AREAPAGAR"] != DBNull.Value)
                            objCCCEDEN.AREAPAGAR = Convert.ToDouble(objOleDbDataReader["AREAPAGAR"]);

                        /// 10: PRECOM2,N,12,3	
                        if (objOleDbDataReader["PRECOM2"] != DBNull.Value)
                            objCCCEDEN.PRECOM2 = Convert.ToDouble(objOleDbDataReader["PRECOM2"]);

                        /// 11: VALORPAGAR,N,12,3	
                        if (objOleDbDataReader["VALORPAGAR"] != DBNull.Value)
                            objCCCEDEN.VALORPAGAR = Convert.ToDouble(objOleDbDataReader["VALORPAGAR"]);

                        /// 12: DATA,D	
                        if (objOleDbDataReader["DATA"] != DBNull.Value)
                            objCCCEDEN.DATA = Program.ConvertToValidDateTime(objOleDbDataReader["DATA"].ToString().Trim());

                        /// 13: PRECOM2P,N,12,3	
                        if (objOleDbDataReader["PRECOM2P"] != DBNull.Value)
                            objCCCEDEN.PRECOM2P = Convert.ToDouble(objOleDbDataReader["PRECOM2P"]);

                        /// 14: AREAPAGO,N,12,2	
                        if (objOleDbDataReader["AREAPAGO"] != DBNull.Value)
                            objCCCEDEN.AREAPAGO = Convert.ToDouble(objOleDbDataReader["AREAPAGO"]);

                        //VALORESCUD
                        if (objOleDbDataReader["VALORESCUD"] != DBNull.Value)
                            objCCCEDEN.VALORESCUD = Convert.ToDouble(objOleDbDataReader["VALORESCUD"]);

                        /// 15: VALORPAGO,N,12,3	
                        if (objOleDbDataReader["VALORPAGO"] != DBNull.Value)
                            objCCCEDEN.VALORPAGO = Convert.ToDouble(objOleDbDataReader["VALORPAGO"]);

                        /// 16: NOTASPAGO,C,140	
                        if (objOleDbDataReader["NOTASPAGO"] != DBNull.Value)
                            objCCCEDEN.NOTASPAGO = Convert.ToString(objOleDbDataReader["NOTASPAGO"]);

                        /// 17: PRECOM2F,N,12,3	
                        if (objOleDbDataReader["PRECOM2F"] != DBNull.Value)
                            objCCCEDEN.PRECOM2F = Convert.ToDouble(objOleDbDataReader["PRECOM2F"]);

                        /// 18: AREAFALTA,N,12,2	
                        if (objOleDbDataReader["AREAFALTA"] != DBNull.Value)
                            objCCCEDEN.AREAFALTA = Convert.ToDouble(objOleDbDataReader["AREAFALTA"]);

                        /// 19: VALORFALTA,N,12,3	
                        if (objOleDbDataReader["VALORFALTA"] != DBNull.Value)
                            objCCCEDEN.VALORFALTA = Convert.ToDouble(objOleDbDataReader["VALORFALTA"]);

                        /// 20: NOTASFALTA,C,140	
                        if (objOleDbDataReader["NOTASFALTA"] != DBNull.Value)
                            objCCCEDEN.NOTASFALTA = Convert.ToString(objOleDbDataReader["NOTASFALTA"]);

                        /// 21: ESTADOLIQ,C,50	
                        if (objOleDbDataReader["ESTADOLIQ"] != DBNull.Value)
                            objCCCEDEN.ESTADOLIQ = Convert.ToString(objOleDbDataReader["ESTADOLIQ"]);

                        /// 22: NOTASLIQ,C,140	
                        if (objOleDbDataReader["NOTASLIQ"] != DBNull.Value)
                            objCCCEDEN.NOTASLIQ = Convert.ToString(objOleDbDataReader["NOTASLIQ"]);

                        if (bGetSingleRecord)
                            return objCCCEDEN;
                    }
                }
                return objCCCEDEN;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return null;
            }
        }

        /// <versions>07-03-2018(GesAMFC-v1.0.0.3)</versions>
        /// <remarks>
        /// ID,N,10,0	SOCNUM,N,5,0	SOCNOME,C,70	IDLOTE,N,5,0	NUMLOTE,C,10	NUMPAG,N,2,0	AREA,N,12,2	    CEDERPERC,N,4,2	    AREAPAGAR,N,12,2	PRECOM2,N,12,3  VALORPAGAR,N,12,3	DATA,D	PRECOM2P,N,12,3	    AREAPAGO,N,12,2	    VALORESCUD,N,12,3	VALORPAGO,N,12,3	NOTASPAGO,C,140	PRECOM2F,N,12,3	AREAFALTA,N,12,2	VALORFALTA,N,12,3	NOTASFALTA,C,140	ESTADOLIQ,C,50	NOTASLIQ,C,140	
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
        public Boolean Save_DB_Lote_CEDEN(AMFC_ContaCorrente_CEDEN objCCCEDEN)
        {
            String sQueryString = String.Empty;
            try
            {
                Set_OLE_DB_ConnectionString();
                if (String.IsNullOrEmpty(LibAMFC.DBF_AMFC_SOCIO_FileName))
                    OLE_DB_Settings();

                #region     Delete Previous Existing Lote CEDEN ID Info
                if (objCCCEDEN.IDLOTE > 0)
                {
                    if (!Del_DB_Lote_CEDEN(objCCCEDEN))
                    {
                        String sError = "Não foi possível atualizar os dados do Lote Nº: " + objCCCEDEN.NUMLOTE + " (ID=" + objCCCEDEN.IDLOTE + ") " + " do Sócio: " + objCCCEDEN.SOCNOME + " Nº: " + objCCCEDEN.SOCNUM;
                        MessageBox.Show(sError, "Erro Operação", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
                #endregion  Delete Previous Existing Lote CEDEN ID Info

                #region     Set New ID
                if (objCCCEDEN.ID < 1)
                    objCCCEDEN.ID = Get_Lote_CEDEN_IDs_Max_Number() + 1;
                #endregion  Set New ID

                #region     Check if already exist
                if (Lote_CEDEN_Already_Exist(objCCCEDEN))
                {
                    String sWarning = "Já existe um Registo de " + "CEDENestruturas" + " para o Lote Nº: " + objCCCEDEN.NUMLOTE + " (ID=" + objCCCEDEN.IDLOTE + ") " + " do Sócio: " + objCCCEDEN.SOCNOME + " Nº: " + objCCCEDEN.SOCNUM;
                    MessageBox.Show(sWarning, "Lote já existente", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                #endregion  Check if already exist

                #region     Trim Observations
                Int32 iEachObsMaxLen = 139;
                if (objCCCEDEN.NOTASPAGO.Trim().Length > iEachObsMaxLen)
                    objCCCEDEN.NOTASPAGO = objCCCEDEN.NOTASPAGO.Substring(0, iEachObsMaxLen - 1);
                if (objCCCEDEN.NOTASFALTA.Trim().Length > iEachObsMaxLen)
                    objCCCEDEN.NOTASFALTA = objCCCEDEN.NOTASFALTA.Substring(0, iEachObsMaxLen - 1);
                #endregion  Trim Observations

                #region     Query

                sQueryString += "INSERT INTO " + "CC_CEDEN" + " " + "\n";
                sQueryString += "( " + " " + "\n";

                #region     Columns
                sQueryString += "\t" + "ID" + "," + " " + "\n"; /// 01: ID,N,10,0	
                sQueryString += "\t" + "SOCNUM" + "," + " " + "\n"; /// 02: SOCNUM,N,5,0
                sQueryString += "\t" + "SOCNOME" + "," + " " + "\n"; /// 03: SOCNOME,C,70	
                sQueryString += "\t" + "IDLOTE" + "," + " " + "\n"; /// 04: IDLOTE,N,5,0
                sQueryString += "\t" + "NUMLOTE" + "," + " " + "\n"; /// 05: NUMLOTE,C,10	
                sQueryString += "\t" + "NUMPAG" + "," + " " + "\n"; /// 06: NUMPAG,N,2,0	
                sQueryString += "\t" + "AREA" + "," + " " + "\n"; /// 07: AREA,N,12,2	  
                sQueryString += "\t" + "CEDERPERC" + "," + " " + "\n"; /// 08: CEDERPERC,N,4,2
                sQueryString += "\t" + "AREAPAGAR" + "," + " " + "\n"; /// 09: AREAPAGAR,N,12,2	
                sQueryString += "\t" + "PRECOM2" + "," + " " + "\n"; /// 10: PRECOM2,N,12,3	
                sQueryString += "\t" + "VALORPAGAR" + "," + " " + "\n"; /// 11: VALORPAGAR,N,12,3
                sQueryString += "\t" + "DATA" + "," + " " + "\n"; /// 12: DATA,D	
                sQueryString += "\t" + "PRECOM2P" + "," + " " + "\n"; /// 13: PRECOM2P,N,12,3	
                sQueryString += "\t" + "AREAPAGO" + "," + " " + "\n"; /// 14: AREAPAGO,N,12,2
                sQueryString += "\t" + "VALORESCUD" + "," + " " + "\n"; //VALORESCUD,N,12,3
                sQueryString += "\t" + "VALORPAGO" + "," + " " + "\n"; /// 15: VALORPAGO,N,12,3
                sQueryString += "\t" + "NOTASPAGO" + "," + " " + "\n"; /// 16: NOTASPAGO,C,140
                sQueryString += "\t" + "PRECOM2F" + "," + " " + "\n"; /// 17: PRECOM2F,N,12,3	
                sQueryString += "\t" + "AREAFALTA" + "," + " " + "\n"; /// 18: AREAFALTA,N,12,2	
                sQueryString += "\t" + "VALORFALTA" + "," + " " + "\n"; /// 19: VALORFALTA,N,12,3	
                sQueryString += "\t" + "NOTASFALTA" + "," + " " + "\n"; /// 20: NOTASFALTA,C,140	
                sQueryString += "\t" + "ESTADOLIQ" + "," + " " + "\n"; /// 21: ESTADOLIQ,C,50	
                sQueryString += "\t" + "NOTASLIQ" + " " + " " + "\n"; /// 22: NOTASLIQ,C,140	
                #endregion  Columns

                sQueryString += ") " + " " + "\n";
                sQueryString += "VALUES" + " " + "\n";
                sQueryString += "( " + " " + "\n";
                
                #region     Values
                sQueryString += "\t" + objCCCEDEN.ID + "," + " " + "\n"; /// 01: ID,N,10,0	
                sQueryString += "\t" + objCCCEDEN.SOCNUM + "," + " " + "\n";  /// 02: SOCNUM,N,5,0	
                sQueryString += "\t" + "'" + objCCCEDEN.SOCNOME.Trim().ToUpper() + "'" + "," + " " + "\n"; /// 03: SOCNOME,C,70	
                sQueryString += "\t" + objCCCEDEN.IDLOTE + "," + " " + "\n"; /// 04: IDLOTE,N,5,0	
                sQueryString += "\t" + "'" + objCCCEDEN.NUMLOTE + "'" + "," + " " + "\n"; /// 05: NUMLOTE,C,10	
                sQueryString += "\t" + ((objCCCEDEN.NUMPAG > 0) ? objCCCEDEN.NUMPAG : 1) + "," + " " + "\n"; /// 06: NUMPAG,N,2,0	
                sQueryString += "\t" + "'" + ((objCCCEDEN.AREA > 0) ? Program.SetAreaDoubleStringValue(objCCCEDEN.AREA) : Program.Default_Area_Double_String) + "'" + "," + " " + "\n"; /// 07: AREA,N,12,2	    
                sQueryString += "\t" + "'" + ((objCCCEDEN.CEDERPERC > 0) ? Program.SetAreaDoubleStringValue(objCCCEDEN.CEDERPERC) : Program.Default_Area_Double_String) + "'" + "," + " " + "\n"; /// 08: CEDERPERC,N,4,2	    
                sQueryString += "\t" + "'" + ((objCCCEDEN.AREAPAGAR > 0) ? Program.SetAreaDoubleStringValue(objCCCEDEN.AREAPAGAR) : Program.Default_Area_Double_String) + "'" + "," + " " + "\n"; /// 09: AREAPAGAR,N,12,2
                sQueryString += "\t" + "'" + ((objCCCEDEN.PRECOM2 > 0) ? Program.SetPayDoubleStringValue(objCCCEDEN.PRECOM2) : Program.Default_Pay_Double_String) + "'" + "," + " " + "\n"; /// 10: PRECOM2,N,12,3
                sQueryString += "\t" + "'" + ((objCCCEDEN.VALORPAGAR > 0) ? Program.SetPayDoubleStringValue(objCCCEDEN.VALORPAGAR) : Program.Default_Pay_Double_String) + "'" + "," + " " + "\n"; /// 11: VALORPAGAR,N,12,3
                sQueryString += "\t" + "'" + (Program.IsValidDateTime(objCCCEDEN.DATA) ? objCCCEDEN.DATA.ToString(Program.DBF_Insert_Date_Format_String) : Program.Default_Date_Str) + "'" + "," + " " + "\n"; /// 12: DATA,D

                sQueryString += "\t" + "'" + ((objCCCEDEN.PRECOM2P > 0) ? Program.SetPayDoubleStringValue(objCCCEDEN.PRECOM2P) : Program.Default_Pay_Double_String) + "'" + "," + " " + "\n"; /// 13: PRECOM2P,N,12,3
                sQueryString += "\t" + "'" + ((objCCCEDEN.AREAPAGO > 0) ? Program.SetAreaDoubleStringValue(objCCCEDEN.AREAPAGO) : Program.Default_Area_Double_String) + "'" + "," + " " + "\n"; /// 14: AREAPAGO,N,12,2	
                sQueryString += "\t" + "'" + ((objCCCEDEN.VALORESCUD > 0) ? Program.SetPayDoubleStringValue(objCCCEDEN.VALORESCUD) : Program.Default_Pay_Double_String) + "'" + "," + " " + "\n"; //VALORESCUD,N,12,3
                sQueryString += "\t" + "'" + ((objCCCEDEN.VALORPAGO > 0) ? Program.SetPayDoubleStringValue(objCCCEDEN.VALORPAGO) : Program.Default_Pay_Double_String) + "'" + "," + " " + "\n"; /// 15: VALORPAGO,N,12,3	
                sQueryString += "\t" + "'" + objCCCEDEN.NOTASPAGO.Trim() + "'" + ", " + " " + "\n"; /// 16: NOTASPAGO,C,140

                sQueryString += "\t" + "'" + ((objCCCEDEN.PRECOM2F > 0) ? Program.SetPayDoubleStringValue(objCCCEDEN.PRECOM2F) : Program.Default_Pay_Double_String) + "'" + "," + " " + "\n"; /// 17: PRECOM2F,N,12,3	 
                sQueryString += "\t" + "'" + ((objCCCEDEN.AREAFALTA > 0) ? Program.SetAreaDoubleStringValue(objCCCEDEN.AREAFALTA) : Program.Default_Area_Double_String) + "'" + "," + " " + "\n"; 	/// 18: AREAFALTA,N,12,2	
                sQueryString += "\t" + "'" + ((objCCCEDEN.VALORFALTA > 0) ? Program.SetPayDoubleStringValue(objCCCEDEN.VALORFALTA) : Program.Default_Pay_Double_String) + "'" + "," + " " + "\n"; /// 19: VALORFALTA,N,12,3
                sQueryString += "\t" + "'" + objCCCEDEN.NOTASFALTA.Trim() + "'" + ", " + " " + "\n"; /// 20: NOTASFALTA,C,140	

                sQueryString += "\t" + "'" + objCCCEDEN.ESTADOLIQ.Trim() + "'" + ", " + " " + "\n"; /// 21: ESTADOLIQ,C,50
                sQueryString += "\t" + "'" + objCCCEDEN.NOTASLIQ.Trim() + "'" + " " + " " + "\n"; /// 22: NOTASLIQ,C,140	
                #endregion  Values

                sQueryString += ") " + " " + "\n";

                #endregion  Query

                using (OleDbConnection objOleDbConnection = new OleDbConnection(OLE_DB_ConnectionString))
                {
                    objOleDbConnection.Open();
                    using (OleDbCommand objOleDbCommand = new OleDbCommand())
                    {
                        objOleDbCommand.Connection = objOleDbConnection;
                        objOleDbCommand.CommandText = sQueryString;
                        if (objOleDbCommand.ExecuteNonQuery() != 1)
                            return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message + " -> " + "QUERY: " + sQueryString, Program.ErroType.EXCEPTION, true, false);
                return false;
            }
        }

        /// <versions>04-03-2018(GesAMFC-v1.0.0.3)</versions>
        public Boolean Del_DB_Lote_CEDEN(AMFC_ContaCorrente_CEDEN objCCCEDEN)
        {
            String sQueryString = String.Empty;
            try
            {
                AMFCMembers objListMembers = new AMFCMembers();

                Set_OLE_DB_ConnectionString();
                if (String.IsNullOrEmpty(LibAMFC.DBF_AMFC_SOCIO_FileName))
                    OLE_DB_Settings();

                #region     Query
                sQueryString += "DELETE FROM " + "CC_CEDEN" + " " + "\n";
                sQueryString += "WHERE" + " " + "IDLOTE" + " = " + objCCCEDEN.IDLOTE + " " + "\n";
                sQueryString += "AND" + " " + "SOCNUM" + " = " + objCCCEDEN.SOCNUM + " " + "\n";
                #endregion  Query

                using (OleDbConnection objOleDbConnection = new OleDbConnection(OLE_DB_ConnectionString))
                {
                    objOleDbConnection.Open();
                    using (OleDbCommand objOleDbCommand = new OleDbCommand())
                    {
                        objOleDbCommand.Connection = objOleDbConnection;
                        objOleDbCommand.CommandText = sQueryString;
                        if (objOleDbCommand.ExecuteNonQuery() < 0)
                            return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message + " -> " + "QUERY: " + sQueryString, Program.ErroType.EXCEPTION, true, false);
                return false;
            }
        }

        /// <versions>04-03-2018(GesAMFC-v1.0.0.3)</versions>
        public Boolean Lote_CEDEN_Already_Exist(AMFC_ContaCorrente_CEDEN objCCCEDEN)
        {
            String sQueryString = String.Empty;
            try
            {
                AMFCMembers objListMembers = new AMFCMembers();

                Set_OLE_DB_ConnectionString();
                if (String.IsNullOrEmpty(LibAMFC.DBF_AMFC_SOCIO_FileName))
                    OLE_DB_Settings();

                #region     Query
                sQueryString += "SELECT COUNT(*) FROM " + "CC_CEDEN" + " " + "\n";
                sQueryString += "WHERE" + " " + "\n";
                sQueryString += "SOCNUM" + " = " + objCCCEDEN.SOCNUM + " " + "\n";
                sQueryString += "AND" + " " + "(";
                if (objCCCEDEN.IDLOTE > 0)
                    sQueryString += "IDLOTE" + " = " + objCCCEDEN.IDLOTE + " OR " + " " + "\n";
                sQueryString += "NUMLOTE" + " = " + "'" + objCCCEDEN.NUMLOTE + "'" + ")" + " " + "\n";
                sQueryString += "; ";
                #endregion  Query

                using (OleDbConnection objOleDbConnection = new OleDbConnection(OLE_DB_ConnectionString))
                {
                    objOleDbConnection.Open();
                    using (OleDbCommand objOleDbCommand = new OleDbCommand())
                    {
                        objOleDbCommand.Connection = objOleDbConnection;
                        objOleDbCommand.CommandText = sQueryString;
                        if (Convert.ToInt32(objOleDbCommand.ExecuteScalar()) > 0)
                            return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message + " -> " + "QUERY: " + sQueryString, Program.ErroType.EXCEPTION, true, false);
                return true;
            }
        }

        /// <versions>04-03-2018(GesAMFC-v1.0.0.3)</versions>
        public Int32 Get_Lote_CEDEN_IDs_Max_Number()
        {
            String sQueryString = String.Empty;
            try
            {
                var iMaxNumber = -1;

                Set_OLE_DB_ConnectionString();
                if (String.IsNullOrEmpty(LibAMFC.DBF_AMFC_SOCIO_FileName))
                    OLE_DB_Settings();
                using (OleDbConnection objOleDbConnection = new OleDbConnection(OLE_DB_ConnectionString))
                {
                    objOleDbConnection.Open();
                    using (OleDbCommand objOleDbCommand = new OleDbCommand())
                    {
                        objOleDbCommand.Connection = objOleDbConnection;

                        objOleDbCommand.CommandText = "SELECT COUNT(*) FROM " + "CC_CEDEN" + " " + "\n";
                        if (Convert.ToInt32(objOleDbCommand.ExecuteScalar()) <= 0)
                            return 0;

                        objOleDbCommand.CommandText = "SELECT MAX(" + "ID" + ") AS MaxId FROM " + "CC_CEDEN" + ";";
                        iMaxNumber = Convert.ToInt32(objOleDbCommand.ExecuteScalar());
                    }
                }
                return iMaxNumber;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message + " -> " + "QUERY: " + sQueryString, Program.ErroType.EXCEPTION, true, false);
                return -1;
            }
        }

        #endregion  CC CEDEN
        
        #region     CC ESGOT

        /// <versions>08-03-2018(GesAMFC-v1.0.0.3)</versions>
        public AMFC_ContaCorrente_ESGOT Get_Member_ContaCorrente_ESGOT(Int64 lMemberNumber, Int64 lMemberLoteId)
        {
            String sQueryString = String.Empty;
            try
            {
                AMFC_ContaCorrente_ESGOT objCCESGOT = new AMFC_ContaCorrente_ESGOT();
                Set_OLE_DB_ConnectionString();
                if (String.IsNullOrEmpty(LibAMFC.DBF_AMFC_SOCIO_FileName))
                    OLE_DB_Settings();
                sQueryString = "SELECT * FROM " + "CC_ESGOT" + " WHERE " + "SOCNUM" + " = " + lMemberNumber + " AND " + "IDLOTE" + " = " + lMemberLoteId;
                using (OleDbConnection objOleDbConnection = new OleDbConnection(OLE_DB_ConnectionString))
                {
                    objOleDbConnection.Open();
                    using (OleDbCommand objOleDbCommand = new OleDbCommand())
                    {
                        objOleDbCommand.Connection = objOleDbConnection;
                        objOleDbCommand.CommandText = sQueryString;

                        objCCESGOT = Get_Member_CC_Lote_ESGOT(objOleDbCommand, true);
                        if (objCCESGOT == null)
                        {
                            StackFrame objStackFrame = new StackFrame();
                            String sErrorMsg = "Não foi possível obter a Conta Corrente de " + "Esgotos" + " do Lote ID=" + lMemberLoteId + " do Sócio Nº: " + lMemberNumber + " -> " + "QUERY: " + sQueryString;
                            Program.HandleError(objStackFrame.GetMethod().Name, sErrorMsg, Program.ErroType.ERROR, true, true);
                            objStackFrame = null;
                            return null;
                        }
                    }
                }

                return objCCESGOT;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message + " -> " + "QUERY: " + sQueryString, Program.ErroType.EXCEPTION, true, false);
                return null;
            }
        }

        /// <versions>08-03-2018(GesAMFC-v1.0.0.3)</versions>
        /// <remarks>
        /// ID,N,10,0	SOCNUM,N,5,0	SOCNOME,C,70	IDLOTE,N,5,0	NUMLOTE,C,10	AREAPAGAN,N,10,2	AREAPAGAR,N,10,2	NUMPAG,N,2,0    VALORPAGAR,N,12,3   DATA,D      VALORESCUD,N,12,3	VALORPAGO, N,12,3	NOTASPAGO,C,140	    VALORFALTA,N,12,3	NOTASFALTA,C,140	ESTADOLIQ,C,50	NOTASLIQ,C,140
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
        private AMFC_ContaCorrente_ESGOT Get_Member_CC_Lote_ESGOT(OleDbCommand objOleDbCommand, Boolean bGetSingleRecord)
        {
            try
            {
                AMFC_ContaCorrente_ESGOT objCCESGOT = new AMFC_ContaCorrente_ESGOT();
                using (OleDbDataReader objOleDbDataReader = objOleDbCommand.ExecuteReader())
                {
                    while (objOleDbDataReader.Read())
                    {
                        if (objOleDbDataReader["ID"] == DBNull.Value)
                            continue;
                        if (objOleDbDataReader["SOCNUM"] == DBNull.Value)
                            continue;
                        if (objOleDbDataReader["IDLOTE"] == DBNull.Value)
                            continue;
                        if (objOleDbDataReader["NUMLOTE"] == DBNull.Value)
                            continue;

                        /// 01: ID,N,10,0	
                        if (objOleDbDataReader["ID"] != DBNull.Value)
                            objCCESGOT.ID = Convert.ToInt64(objOleDbDataReader["ID"]);

                        /// 02: SOCNUM,N,5,0
                        if (objOleDbDataReader["SOCNUM"] != DBNull.Value)
                            objCCESGOT.SOCNUM = Convert.ToInt64(objOleDbDataReader["SOCNUM"]);

                        /// 03: SOCNOME,C,70	
                        if (objOleDbDataReader["SOCNOME"] != DBNull.Value)
                            objCCESGOT.SOCNOME = Convert.ToString(objOleDbDataReader["SOCNOME"]);

                        /// 04: IDLOTE,N,5,0	
                        if (objOleDbDataReader["IDLOTE"] != DBNull.Value)
                            objCCESGOT.IDLOTE = Convert.ToInt64(objOleDbDataReader["IDLOTE"]);

                        /// 05: NUMLOTE,C,10	
                        if (objOleDbDataReader["NUMLOTE"] != DBNull.Value)
                            objCCESGOT.NUMLOTE = Convert.ToString(objOleDbDataReader["NUMLOTE"]);

                        /// 06: AREAPAGAN,N,10,2	
                        if (objOleDbDataReader["AREAPAGAN"] != DBNull.Value)
                            objCCESGOT.AREAPAGAN = Convert.ToDouble(objOleDbDataReader["AREAPAGAN"]);

                        /// 07: AREAPAGAR,N,10,2	
                        if (objOleDbDataReader["AREAPAGAR"] != DBNull.Value)
                            objCCESGOT.AREAPAGAR = Convert.ToDouble(objOleDbDataReader["AREAPAGAR"]);

                        /// 08: NUMPAG,N,2,0
                        if (objOleDbDataReader["NUMPAG"] != DBNull.Value)
                            objCCESGOT.NUMPAG = Convert.ToInt32(objOleDbDataReader["NUMPAG"]);

                        /// 09: VALORPAGAR,N,12,3	
                        if (objOleDbDataReader["VALORPAGAR"] != DBNull.Value)
                            objCCESGOT.VALORPAGAR = Convert.ToDouble(objOleDbDataReader["VALORPAGAR"]);

                        /// 10: DATA,D	
                        if (objOleDbDataReader["DATA"] != DBNull.Value)
                            objCCESGOT.DATA = Program.ConvertToValidDateTime(objOleDbDataReader["DATA"].ToString().Trim());

                        //VALORESCUD,N,12,3
                        if (objOleDbDataReader["VALORESCUD"] != DBNull.Value)
                            objCCESGOT.VALORESCUD = Convert.ToDouble(objOleDbDataReader["VALORESCUD"]);

                        /// 11: VALORPAGO,N,12,3	
                        if (objOleDbDataReader["VALORPAGO"] != DBNull.Value)
                            objCCESGOT.VALORPAGO = Convert.ToDouble(objOleDbDataReader["VALORPAGO"]);

                        /// 12: NOTASPAGO,C,140	
                        if (objOleDbDataReader["NOTASPAGO"] != DBNull.Value)
                            objCCESGOT.NOTASPAGO = Convert.ToString(objOleDbDataReader["NOTASPAGO"]);

                        /// 13: VALORFALTA,N,12,3	
                        if (objOleDbDataReader["VALORFALTA"] != DBNull.Value)
                            objCCESGOT.VALORFALTA = Convert.ToDouble(objOleDbDataReader["VALORFALTA"]);

                        /// 14: NOTASFALTA,C,140	
                        if (objOleDbDataReader["NOTASFALTA"] != DBNull.Value)
                            objCCESGOT.NOTASFALTA = Convert.ToString(objOleDbDataReader["NOTASFALTA"]);

                        /// 15: ESTADOLIQ,C,50	
                        if (objOleDbDataReader["ESTADOLIQ"] != DBNull.Value)
                            objCCESGOT.ESTADOLIQ = Convert.ToString(objOleDbDataReader["ESTADOLIQ"]);

                        /// 16: NOTASLIQ,C,140
                        if (objOleDbDataReader["NOTASLIQ"] != DBNull.Value)
                            objCCESGOT.NOTASLIQ = Convert.ToString(objOleDbDataReader["NOTASLIQ"]);

                        if (bGetSingleRecord)
                            return objCCESGOT;
                    }
                }
                return objCCESGOT;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return null;
            }
        }

        /// <versions>08-03-2018(GesAMFC-v1.0.0.3)</versions>
        /// <remarks>
        /// ID,N,10,0	SOCNUM,N,5,0	SOCNOME,C,70	IDLOTE,N,5,0	NUMLOTE,C,10	AREAPAGAN,N,10,2	AREAPAGAR,N,10,2	NUMPAG,N,2,0    VALORPAGAR,N,12,3   DATA,D      VALORESCUD,N,12,3	VALORPAGO, N,12,3	NOTASPAGO,C,140	    VALORFALTA,N,12,3	NOTASFALTA,C,140	ESTADOLIQ,C,50	NOTASLIQ,C,140
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
        public Boolean Save_DB_Lote_ESGOT(AMFC_ContaCorrente_ESGOT objCCESGOT)
        {
            String sQueryString = String.Empty;
            try
            {
                Set_OLE_DB_ConnectionString();
                if (String.IsNullOrEmpty(LibAMFC.DBF_AMFC_SOCIO_FileName))
                    OLE_DB_Settings();

                #region     Delete Previous Existing Lote ESGOT ID Info
                if (objCCESGOT.IDLOTE > 0)
                {
                    if (!Del_DB_Lote_ESGOT(objCCESGOT))
                    {
                        String sError = "Não foi possível atualizar os dados do Lote Nº: " + objCCESGOT.NUMLOTE + " (ID=" + objCCESGOT.IDLOTE + ") " + " do Sócio: " + objCCESGOT.SOCNOME + " Nº: " + objCCESGOT.SOCNUM;
                        MessageBox.Show(sError, "Erro Operação", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
                #endregion  Delete Previous Existing Lote ESGOT ID Info

                #region     Set New ID
                if (objCCESGOT.ID < 1)
                    objCCESGOT.ID = Get_Lote_ESGOT_IDs_Max_Number() + 1;
                #endregion  Set New ID

                #region     Check if already exist
                if (Lote_ESGOT_Already_Exist(objCCESGOT))
                {
                    String sWarning = "Já existe um Registo de " + "Esgotos" + " para o Lote Nº: " + objCCESGOT.NUMLOTE + " (ID=" + objCCESGOT.IDLOTE + ") " + " do Sócio: " + objCCESGOT.SOCNOME + " Nº: " + objCCESGOT.SOCNUM;
                    MessageBox.Show(sWarning, "Lote já existente", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                #endregion  Check if already exist

                #region     Trim Observations
                Int32 iEachObsMaxLen = 139;
                if (objCCESGOT.NOTASPAGO.Trim().Length > iEachObsMaxLen)
                    objCCESGOT.NOTASPAGO = objCCESGOT.NOTASPAGO.Substring(0, iEachObsMaxLen - 1);
                if (objCCESGOT.NOTASFALTA.Trim().Length > iEachObsMaxLen)
                    objCCESGOT.NOTASFALTA = objCCESGOT.NOTASFALTA.Substring(0, iEachObsMaxLen - 1);
                #endregion  Trim Observations

                #region     Query

                sQueryString += "INSERT INTO " + "CC_ESGOT" + " " + "\n";
                sQueryString += "( " + " " + "\n";

                #region     Columns
                sQueryString += "\t" + "ID" + "," + " " + "\n"; /// 01: ID,N,10,0	
                sQueryString += "\t" + "SOCNUM" + "," + " " + "\n"; /// 02: SOCNUM,N,5,0	
                sQueryString += "\t" + "SOCNOME" + "," + " " + "\n"; /// 03: SOCNOME,C,70
                sQueryString += "\t" + "IDLOTE" + "," + " " + "\n"; /// 04: IDLOTE,N,5,0	
                sQueryString += "\t" + "NUMLOTE" + "," + " " + "\n"; /// 05: NUMLOTE,C,10	
                sQueryString += "\t" + "AREAPAGAN" + "," + " " + "\n"; /// 06: AREAPAGAN,N,10,2	
                sQueryString += "\t" + "AREAPAGAR" + "," + " " + "\n"; /// 07: AREAPAGAR,N,12,2
                sQueryString += "\t" + "NUMPAG" + "," + " " + "\n"; /// 08: NUMPAG,N,2,0	
                sQueryString += "\t" + "VALORPAGAR" + "," + " " + "\n"; /// 09: VALORPAGAR,N,12,3	
                sQueryString += "\t" + "DATA" + "," + " " + "\n"; /// 10: DATA,D	
                sQueryString += "\t" + "VALORESCUD" + "," + " " + "\n"; //VALORESCUD,N,12,3	
                sQueryString += "\t" + "VALORPAGO" + "," + " " + "\n"; /// 11: VALORPAGO,N,12,3	
                sQueryString += "\t" + "NOTASPAGO" + "," + " " + "\n"; /// 12: NOTASPAGO,C,140	
                sQueryString += "\t" + "VALORFALTA" + "," + " " + "\n"; /// 13: VALORFALTA,N,12,3	
                sQueryString += "\t" + "NOTASFALTA" + "," + " " + "\n"; /// 14: NOTASFALTA,C,140	
                sQueryString += "\t" + "ESTADOLIQ" + "," + " " + "\n"; /// 15: ESTADOLIQ,C,50	
                sQueryString += "\t" + "NOTASLIQ" + " " + " " + "\n"; /// 16: NOTASLIQ,C,140	
                #endregion  Columns

                sQueryString += ") " + " " + "\n";
                sQueryString += "VALUES" + " " + "\n";
                sQueryString += "( " + " " + "\n";

                #region     Values
                sQueryString += "\t" + objCCESGOT.ID + "," + " " + "\n"; /// 01: ID,N,10,0	
                sQueryString += "\t" + objCCESGOT.SOCNUM + "," + " " + "\n";  /// 02: SOCNUM,N,5,0	
                sQueryString += "\t" + "'" + objCCESGOT.SOCNOME.Trim().ToUpper() + "'" + "," + " " + "\n"; /// 03: SOCNOME,C,70	
                sQueryString += "\t" + objCCESGOT.IDLOTE + "," + " " + "\n"; /// 04: IDLOTE,N,5,0	
                sQueryString += "\t" + "'" + objCCESGOT.NUMLOTE + "'" + "," + " " + "\n"; /// 05: NUMLOTE,C,10	
                sQueryString += "\t" + "'" + ((objCCESGOT.AREAPAGAN > 0) ? Program.SetAreaDoubleStringValue(objCCESGOT.AREAPAGAN) : Program.Default_Area_Double_String) + "'" + "," + " " + "\n"; /// /// 06: AREAPAGAN,N,10,2	
                sQueryString += "\t" + "'" + ((objCCESGOT.AREAPAGAR > 0) ? Program.SetAreaDoubleStringValue(objCCESGOT.AREAPAGAR) : Program.Default_Area_Double_String) + "'" + "," + " " + "\n"; /// 07: AREAPAGAR,N,12,2	
                sQueryString += "\t" + ((objCCESGOT.NUMPAG > 0) ? objCCESGOT.NUMPAG : 1) + "," + " " + "\n"; /// 08: NUMPAG,N,2,0	
                sQueryString += "\t" + "'" + ((objCCESGOT.VALORPAGAR > 0) ? Program.SetPayDoubleStringValue(objCCESGOT.VALORPAGAR) : Program.Default_Pay_Double_String) + "'" + "," + " " + "\n"; /// 09: VALORPAGAR,N,12,3	
                sQueryString += "\t" + "'" + (Program.IsValidDateTime(objCCESGOT.DATA) ? objCCESGOT.DATA.ToString(Program.DBF_Insert_Date_Format_String) : Program.Default_Date_Str) + "'" + "," + " " + "\n"; /// 10: DATA,D	
                sQueryString += "\t" + "'" + ((objCCESGOT.VALORESCUD > 0) ? Program.SetPayDoubleStringValue(objCCESGOT.VALORESCUD) : Program.Default_Pay_Double_String) + "'" + "," + " " + "\n"; //VALORESCUD,N,12,3	
                sQueryString += "\t" + "'" + ((objCCESGOT.VALORPAGO > 0) ? Program.SetPayDoubleStringValue(objCCESGOT.VALORPAGO) : Program.Default_Pay_Double_String) + "'" + "," + " " + "\n"; /// 11: VALORPAGO,N,12,3			
                sQueryString += "\t" + "'" + objCCESGOT.NOTASPAGO.Trim() + "'" + ", " + " " + "\n"; /// 12: NOTASPAGO,C,140	
                sQueryString += "\t" + "'" + ((objCCESGOT.VALORFALTA > 0) ? Program.SetPayDoubleStringValue(objCCESGOT.VALORFALTA) : Program.Default_Pay_Double_String) + "'" + "," + " " + "\n"; /// 13: VALORFALTA,N,12,3		
                sQueryString += "\t" + "'" + objCCESGOT.NOTASFALTA.Trim() + "'" + ", " + " " + "\n"; /// 14: NOTASFALTA,C,140	
                sQueryString += "\t" + "'" + objCCESGOT.ESTADOLIQ.Trim() + "'" + ", " + " " + "\n"; /// 15: ESTADOLIQ,C,50	
                sQueryString += "\t" + "'" + objCCESGOT.NOTASLIQ.Trim() + "'" + " " + " " + "\n"; /// 16: NOTASLIQ,C,140	
                #endregion  Values

                sQueryString += ") " + " " + "\n";

                #endregion  Query

                using (OleDbConnection objOleDbConnection = new OleDbConnection(OLE_DB_ConnectionString))
                {
                    objOleDbConnection.Open();
                    using (OleDbCommand objOleDbCommand = new OleDbCommand())
                    {
                        objOleDbCommand.Connection = objOleDbConnection;
                        objOleDbCommand.CommandText = sQueryString;
                        if (objOleDbCommand.ExecuteNonQuery() != 1)
                            return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message + " -> " + "QUERY: " + sQueryString, Program.ErroType.EXCEPTION, true, false);
                return false;
            }
        }

        /// <versions>08-03-2018(GesAMFC-v1.0.0.3)</versions>
        public Boolean Del_DB_Lote_ESGOT(AMFC_ContaCorrente_ESGOT objCCESGOT)
        {
            String sQueryString = String.Empty;
            try
            {
                AMFCMembers objListMembers = new AMFCMembers();

                Set_OLE_DB_ConnectionString();
                if (String.IsNullOrEmpty(LibAMFC.DBF_AMFC_SOCIO_FileName))
                    OLE_DB_Settings();

                #region     Query
                sQueryString += "DELETE FROM " + "CC_ESGOT" + " " + "\n";
                sQueryString += "WHERE" + " " + "IDLOTE" + " = " + objCCESGOT.IDLOTE + " " + "\n";
                sQueryString += "AND" + " " + "SOCNUM" + " = " + objCCESGOT.SOCNUM + " " + "\n";
                #endregion  Query

                using (OleDbConnection objOleDbConnection = new OleDbConnection(OLE_DB_ConnectionString))
                {
                    objOleDbConnection.Open();
                    using (OleDbCommand objOleDbCommand = new OleDbCommand())
                    {
                        objOleDbCommand.Connection = objOleDbConnection;
                        objOleDbCommand.CommandText = sQueryString;
                        if (objOleDbCommand.ExecuteNonQuery() < 0)
                            return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message + " -> " + "QUERY: " + sQueryString, Program.ErroType.EXCEPTION, true, false);
                return false;
            }
        }

        /// <versions>08-03-2018(GesAMFC-v1.0.0.3)</versions>
        public Boolean Lote_ESGOT_Already_Exist(AMFC_ContaCorrente_ESGOT objCCESGOT)
        {
            String sQueryString = String.Empty;
            try
            {
                AMFCMembers objListMembers = new AMFCMembers();

                Set_OLE_DB_ConnectionString();
                if (String.IsNullOrEmpty(LibAMFC.DBF_AMFC_SOCIO_FileName))
                    OLE_DB_Settings();

                #region     Query
                sQueryString += "SELECT COUNT(*) FROM " + "CC_ESGOT" + " " + "\n";
                sQueryString += "WHERE" + " " + "\n";
                sQueryString += "SOCNUM" + " = " + objCCESGOT.SOCNUM + " " + "\n";
                sQueryString += "AND" + " " + "(";
                if (objCCESGOT.IDLOTE > 0)
                    sQueryString += "IDLOTE" + " = " + objCCESGOT.IDLOTE + " OR " + " " + "\n";
                sQueryString += "NUMLOTE" + " = " + "'" + objCCESGOT.NUMLOTE + "'" + ")" + " " + "\n";
                sQueryString += "; ";
                #endregion  Query

                using (OleDbConnection objOleDbConnection = new OleDbConnection(OLE_DB_ConnectionString))
                {
                    objOleDbConnection.Open();
                    using (OleDbCommand objOleDbCommand = new OleDbCommand())
                    {
                        objOleDbCommand.Connection = objOleDbConnection;
                        objOleDbCommand.CommandText = sQueryString;
                        if (Convert.ToInt32(objOleDbCommand.ExecuteScalar()) > 0)
                            return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message + " -> " + "QUERY: " + sQueryString, Program.ErroType.EXCEPTION, true, false);
                return true;
            }
        }

        /// <versions>08-03-2018(GesAMFC-v1.0.0.3)</versions>
        public Int32 Get_Lote_ESGOT_IDs_Max_Number()
        {
            String sQueryString = String.Empty;
            try
            {
                var iMaxNumber = -1;

                Set_OLE_DB_ConnectionString();
                if (String.IsNullOrEmpty(LibAMFC.DBF_AMFC_SOCIO_FileName))
                    OLE_DB_Settings();
                using (OleDbConnection objOleDbConnection = new OleDbConnection(OLE_DB_ConnectionString))
                {
                    objOleDbConnection.Open();
                    using (OleDbCommand objOleDbCommand = new OleDbCommand())
                    {
                        objOleDbCommand.Connection = objOleDbConnection;

                        objOleDbCommand.CommandText = "SELECT COUNT(*) FROM " + "CC_ESGOT" + " " + "\n";
                        if (Convert.ToInt32(objOleDbCommand.ExecuteScalar()) <= 0)
                            return 0;

                        objOleDbCommand.CommandText = "SELECT MAX(" + "ID" + ") AS MaxId FROM " + "CC_ESGOT" + ";";
                        iMaxNumber = Convert.ToInt32(objOleDbCommand.ExecuteScalar());
                    }
                }
                return iMaxNumber;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message + " -> " + "QUERY: " + sQueryString, Program.ErroType.EXCEPTION, true, false);
                return -1;
            }
        }

        #endregion  CC ESGOT

        #region     CC RECON

        /// <versions>02-03-2018(GesAMFC-v1.0.0.3)</versions>
        public AMFC_ContaCorrente_RECON Get_Member_ContaCorrente_RECON(Int64 lMemberNumber, Int64 lMemberLoteId)
        {
            String sQueryString = String.Empty;
            try
            {
                AMFC_ContaCorrente_RECON objCCRECON = new AMFC_ContaCorrente_RECON();
                Set_OLE_DB_ConnectionString();
                if (String.IsNullOrEmpty(LibAMFC.DBF_AMFC_SOCIO_FileName))
                    OLE_DB_Settings();
                sQueryString = "SELECT * FROM " + "CC_RECON" + " WHERE " + "SOCNUM" + " = " + lMemberNumber + " AND " + "IDLOTE" + " = " + lMemberLoteId;
                using (OleDbConnection objOleDbConnection = new OleDbConnection(OLE_DB_ConnectionString))
                {
                    objOleDbConnection.Open();
                    using (OleDbCommand objOleDbCommand = new OleDbCommand())
                    {
                        objOleDbCommand.Connection = objOleDbConnection;
                        objOleDbCommand.CommandText = sQueryString;

                        objCCRECON = Get_Member_CC_Lote_RECON(objOleDbCommand, true);
                        if (objCCRECON == null)
                        {
                            StackFrame objStackFrame = new StackFrame();
                            String sErrorMsg = "Não foi possível obter a Conta Corrente de " + "Cedências" + " do Lote ID=" + lMemberLoteId + " do Sócio Nº: " + lMemberNumber + " -> " + "QUERY: " + sQueryString;
                            Program.HandleError(objStackFrame.GetMethod().Name, sErrorMsg, Program.ErroType.ERROR, true, true);
                            objStackFrame = null;
                            return null;
                        }
                    }
                }

                return objCCRECON;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message + " -> " + "QUERY: " + sQueryString, Program.ErroType.EXCEPTION, true, false);
                return null;
            }
        }

        /// <versions>07-03-2018(GesAMFC-v1.0.0.3)</versions>
        /// <remarks>
        /// ID,N,10,0	SOCNUM,N,5,0	SOCNOME,C,70	IDLOTE,N,5,0	NUMLOTE,C,10	NUMPAG,N,2,0	AREA,N,12,2	    AREAPAGAR,N,12,2	PRECOM2,N,12,3  VALORPAGAR,N,12,3	DATA,D	PRECOM2P,N,12,3	    AREAPAGO,N,12,2	    VALORESCUD,N,12,3	VALORPAGO,N,12,3	NOTASPAGO,C,140	PRECOM2F,N,12,3	AREAFALTA,N,12,2	VALORFALTA,N,12,3	NOTASFALTA,C,140	ESTADOLIQ,C,50	NOTASLIQ,C,140	
        /// 01: ID,N,10,0	
        /// 02: SOCNUM,N,5,0	
        /// 03: SOCNOME,C,70	
        /// 04: IDLOTE,N,5,0	
        /// 05: NUMLOTE,C,10	
        /// 06: NUMPAG,N,2,0	
        /// 07: AREA,N,12,2	    
        /// 09: AREAPAGAR,N,12,2	
        /// 10: PRECOM2,N,12,3	
        /// 11: VALORPAGAR,N,12,3	
        /// 12: DATA,D	
        /// 13: PRECOM2P,N,12,3	
        /// 14: AREAPAGO,N,12,2	
        //VALORESCUD,N,12,3
        /// 15: VALORPAGO,N,12,3	
        /// 16: NOTASPAGO,C,140	
        /// 17: PRECOM2F,N,12,3	
        /// 18: AREAFALTA,N,12,2	
        /// 19: VALORFALTA,N,12,3	
        /// 20: NOTASFALTA,C,140	
        /// 21: ESTADOLIQ,C,50	
        /// 22: NOTASLIQ,C,140	
        /// </remarks>
        private AMFC_ContaCorrente_RECON Get_Member_CC_Lote_RECON(OleDbCommand objOleDbCommand, Boolean bGetSingleRecord)
        {
            try
            {
                AMFC_ContaCorrente_RECON objCCRECON = new AMFC_ContaCorrente_RECON();
                using (OleDbDataReader objOleDbDataReader = objOleDbCommand.ExecuteReader())
                {
                    while (objOleDbDataReader.Read())
                    {
                        if (objOleDbDataReader["ID"] == DBNull.Value)
                            continue;
                        if (objOleDbDataReader["SOCNUM"] == DBNull.Value)
                            continue;
                        if (objOleDbDataReader["IDLOTE"] == DBNull.Value)
                            continue;
                        if (objOleDbDataReader["NUMLOTE"] == DBNull.Value)
                            continue;

                        /// 01: ID,N,10,0	
                        if (objOleDbDataReader["ID"] != DBNull.Value)
                            objCCRECON.ID = Convert.ToInt64(objOleDbDataReader["ID"]);

                        /// 02: SOCNUM,N,5,0	
                        if (objOleDbDataReader["SOCNUM"] != DBNull.Value)
                            objCCRECON.SOCNUM = Convert.ToInt64(objOleDbDataReader["SOCNUM"]);

                        /// 03: SOCNOME,C,70	
                        if (objOleDbDataReader["SOCNOME"] != DBNull.Value)
                            objCCRECON.SOCNOME = Convert.ToString(objOleDbDataReader["SOCNOME"]);

                        /// 04: IDLOTE,N,5,0	
                        if (objOleDbDataReader["IDLOTE"] != DBNull.Value)
                            objCCRECON.IDLOTE = Convert.ToInt64(objOleDbDataReader["IDLOTE"]);

                        /// 05: NUMLOTE,C,10	
                        if (objOleDbDataReader["NUMLOTE"] != DBNull.Value)
                            objCCRECON.NUMLOTE = Convert.ToString(objOleDbDataReader["NUMLOTE"]);

                        /// 06: NUMPAG,N,2,0	
                        if (objOleDbDataReader["NUMPAG"] != DBNull.Value)
                            objCCRECON.NUMPAG = Convert.ToInt32(objOleDbDataReader["NUMPAG"]);

                        /// 07: AREA,N,12,2	    
                        if (objOleDbDataReader["AREA"] != DBNull.Value)
                            objCCRECON.AREA = Convert.ToDouble(objOleDbDataReader["AREA"]);

                        /// 09: AREAPAGAR,N,12,2	
                        if (objOleDbDataReader["AREAPAGAR"] != DBNull.Value)
                            objCCRECON.AREAPAGAR = Convert.ToDouble(objOleDbDataReader["AREAPAGAR"]);

                        /// 10: PRECOM2,N,12,3	
                        if (objOleDbDataReader["PRECOM2"] != DBNull.Value)
                            objCCRECON.PRECOM2 = Convert.ToDouble(objOleDbDataReader["PRECOM2"]);

                        /// 11: VALORPAGAR,N,12,3	
                        if (objOleDbDataReader["VALORPAGAR"] != DBNull.Value)
                            objCCRECON.VALORPAGAR = Convert.ToDouble(objOleDbDataReader["VALORPAGAR"]);

                        /// 12: DATA,D	
                        if (objOleDbDataReader["DATA"] != DBNull.Value)
                            objCCRECON.DATA = Program.ConvertToValidDateTime(objOleDbDataReader["DATA"].ToString().Trim());

                        /// 13: PRECOM2P,N,12,3	
                        if (objOleDbDataReader["PRECOM2P"] != DBNull.Value)
                            objCCRECON.PRECOM2P = Convert.ToDouble(objOleDbDataReader["PRECOM2P"]);

                        /// 14: AREAPAGO,N,12,2	
                        if (objOleDbDataReader["AREAPAGO"] != DBNull.Value)
                            objCCRECON.AREAPAGO = Convert.ToDouble(objOleDbDataReader["AREAPAGO"]);

                        //VALORESCUD
                        if (objOleDbDataReader["VALORESCUD"] != DBNull.Value)
                            objCCRECON.VALORESCUD = Convert.ToDouble(objOleDbDataReader["VALORESCUD"]);

                        /// 15: VALORPAGO,N,12,3	
                        if (objOleDbDataReader["VALORPAGO"] != DBNull.Value)
                            objCCRECON.VALORPAGO = Convert.ToDouble(objOleDbDataReader["VALORPAGO"]);

                        /// 16: NOTASPAGO,C,140	
                        if (objOleDbDataReader["NOTASPAGO"] != DBNull.Value)
                            objCCRECON.NOTASPAGO = Convert.ToString(objOleDbDataReader["NOTASPAGO"]);

                        /// 17: PRECOM2F,N,12,3	
                        if (objOleDbDataReader["PRECOM2F"] != DBNull.Value)
                            objCCRECON.PRECOM2F = Convert.ToDouble(objOleDbDataReader["PRECOM2F"]);

                        /// 18: AREAFALTA,N,12,2	
                        if (objOleDbDataReader["AREAFALTA"] != DBNull.Value)
                            objCCRECON.AREAFALTA = Convert.ToDouble(objOleDbDataReader["AREAFALTA"]);

                        /// 19: VALORFALTA,N,12,3	
                        if (objOleDbDataReader["VALORFALTA"] != DBNull.Value)
                            objCCRECON.VALORFALTA = Convert.ToDouble(objOleDbDataReader["VALORFALTA"]);

                        /// 20: NOTASFALTA,C,140	
                        if (objOleDbDataReader["NOTASFALTA"] != DBNull.Value)
                            objCCRECON.NOTASFALTA = Convert.ToString(objOleDbDataReader["NOTASFALTA"]);

                        /// 21: ESTADOLIQ,C,50	
                        if (objOleDbDataReader["ESTADOLIQ"] != DBNull.Value)
                            objCCRECON.ESTADOLIQ = Convert.ToString(objOleDbDataReader["ESTADOLIQ"]);

                        /// 22: NOTASLIQ,C,140	
                        if (objOleDbDataReader["NOTASLIQ"] != DBNull.Value)
                            objCCRECON.NOTASLIQ = Convert.ToString(objOleDbDataReader["NOTASLIQ"]);

                        if (bGetSingleRecord)
                            return objCCRECON;
                    }
                }
                return objCCRECON;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return null;
            }
        }

        /// <versions>07-03-2018(GesAMFC-v1.0.0.3)</versions>
        /// <remarks>
        /// ID,N,10,0	SOCNUM,N,5,0	SOCNOME,C,70	IDLOTE,N,5,0	NUMLOTE,C,10	NUMPAG,N,2,0	AREA,N,12,2	    AREAPAGAR,N,12,2	PRECOM2,N,12,3  VALORPAGAR,N,12,3	DATA,D	PRECOM2P,N,12,3	    AREAPAGO,N,12,2	    VALORESCUD,N,12,3	VALORPAGO,N,12,3	NOTASPAGO,C,140	PRECOM2F,N,12,3	AREAFALTA,N,12,2	VALORFALTA,N,12,3	NOTASFALTA,C,140	ESTADOLIQ,C,50	NOTASLIQ,C,140	
        /// 01: ID,N,10,0	
        /// 02: SOCNUM,N,5,0	
        /// 03: SOCNOME,C,70	
        /// 04: IDLOTE,N,5,0	
        /// 05: NUMLOTE,C,10	
        /// 06: NUMPAG,N,2,0	
        /// 07: AREA,N,12,2	    
        /// 09: AREAPAGAR,N,12,2	
        /// 10: PRECOM2,N,12,3	
        /// 11: VALORPAGAR,N,12,3	
        /// 12: DATA,D	
        /// 13: PRECOM2P,N,12,3	
        /// 14: AREAPAGO,N,12,2	
        ///VALORESCUD,N,12,3
        /// 15: VALORPAGO,N,12,3	
        /// 16: NOTASPAGO,C,140	
        /// 17: PRECOM2F,N,12,3	
        /// 18: AREAFALTA,N,12,2	
        /// 19: VALORFALTA,N,12,3	
        /// 20: NOTASFALTA,C,140	
        /// 21: ESTADOLIQ,C,50	
        /// 22: NOTASLIQ,C,140	
        /// </remarks>
        public Boolean Save_DB_Lote_RECON(AMFC_ContaCorrente_RECON objCCRECON)
        {
            String sQueryString = String.Empty;
            try
            {
                Set_OLE_DB_ConnectionString();
                if (String.IsNullOrEmpty(LibAMFC.DBF_AMFC_SOCIO_FileName))
                    OLE_DB_Settings();

                #region     Delete Previous Existing Lote RECON ID Info
                if (objCCRECON.IDLOTE > 0)
                {
                    if (!Del_DB_Lote_RECON(objCCRECON))
                    {
                        String sError = "Não foi possível atualizar os dados do Lote Nº: " + objCCRECON.NUMLOTE + " (ID=" + objCCRECON.IDLOTE + ") " + " do Sócio: " + objCCRECON.SOCNOME + " Nº: " + objCCRECON.SOCNUM;
                        MessageBox.Show(sError, "Erro Operação", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }
                #endregion  Delete Previous Existing Lote RECON ID Info

                #region     Set New ID
                if (objCCRECON.ID < 1)
                    objCCRECON.ID = Get_Lote_RECON_IDs_Max_Number() + 1;
                #endregion  Set New ID

                #region     Check if already exist
                if (Lote_RECON_Already_Exist(objCCRECON))
                {
                    String sWarning = "Já existe um Registo de " + "RECONestruturas" + " para o Lote Nº: " + objCCRECON.NUMLOTE + " (ID=" + objCCRECON.IDLOTE + ") " + " do Sócio: " + objCCRECON.SOCNOME + " Nº: " + objCCRECON.SOCNUM;
                    MessageBox.Show(sWarning, "Lote já existente", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                #endregion  Check if already exist

                #region     Trim Observations
                Int32 iEachObsMaxLen = 139;
                if (objCCRECON.NOTASPAGO.Trim().Length > iEachObsMaxLen)
                    objCCRECON.NOTASPAGO = objCCRECON.NOTASPAGO.Substring(0, iEachObsMaxLen - 1);
                if (objCCRECON.NOTASFALTA.Trim().Length > iEachObsMaxLen)
                    objCCRECON.NOTASFALTA = objCCRECON.NOTASFALTA.Substring(0, iEachObsMaxLen - 1);
                #endregion  Trim Observations

                #region     Query

                sQueryString += "INSERT INTO " + "CC_RECON" + " " + "\n";
                sQueryString += "( " + " " + "\n";

                #region     Columns
                sQueryString += "\t" + "ID" + "," + " " + "\n"; /// 01: ID,N,10,0	
                sQueryString += "\t" + "SOCNUM" + "," + " " + "\n"; /// 02: SOCNUM,N,5,0
                sQueryString += "\t" + "SOCNOME" + "," + " " + "\n"; /// 03: SOCNOME,C,70	
                sQueryString += "\t" + "IDLOTE" + "," + " " + "\n"; /// 04: IDLOTE,N,5,0
                sQueryString += "\t" + "NUMLOTE" + "," + " " + "\n"; /// 05: NUMLOTE,C,10	
                sQueryString += "\t" + "NUMPAG" + "," + " " + "\n"; /// 06: NUMPAG,N,2,0	
                sQueryString += "\t" + "AREA" + "," + " " + "\n"; /// 07: AREA,N,12,2	  
                sQueryString += "\t" + "AREAPAGAR" + "," + " " + "\n"; /// 09: AREAPAGAR,N,12,2	
                sQueryString += "\t" + "PRECOM2" + "," + " " + "\n"; /// 10: PRECOM2,N,12,3	
                sQueryString += "\t" + "VALORPAGAR" + "," + " " + "\n"; /// 11: VALORPAGAR,N,12,3
                sQueryString += "\t" + "DATA" + "," + " " + "\n"; /// 12: DATA,D	
                sQueryString += "\t" + "PRECOM2P" + "," + " " + "\n"; /// 13: PRECOM2P,N,12,3	
                sQueryString += "\t" + "AREAPAGO" + "," + " " + "\n"; /// 14: AREAPAGO,N,12,2
                sQueryString += "\t" + "VALORESCUD" + "," + " " + "\n"; //VALORESCUD
                sQueryString += "\t" + "VALORPAGO" + "," + " " + "\n"; /// 15: VALORPAGO,N,12,3
                sQueryString += "\t" + "NOTASPAGO" + "," + " " + "\n"; /// 16: NOTASPAGO,C,140
                sQueryString += "\t" + "PRECOM2F" + "," + " " + "\n"; /// 17: PRECOM2F,N,12,3	
                sQueryString += "\t" + "AREAFALTA" + "," + " " + "\n"; /// 18: AREAFALTA,N,12,2	
                sQueryString += "\t" + "VALORFALTA" + "," + " " + "\n"; /// 19: VALORFALTA,N,12,3	
                sQueryString += "\t" + "NOTASFALTA" + "," + " " + "\n"; /// 20: NOTASFALTA,C,140	
                sQueryString += "\t" + "ESTADOLIQ" + "," + " " + "\n"; /// 21: ESTADOLIQ,C,50	
                sQueryString += "\t" + "NOTASLIQ" + " " + " " + "\n"; /// 22: NOTASLIQ,C,140	
                #endregion  Columns

                sQueryString += ") " + " " + "\n";
                sQueryString += "VALUES" + " " + "\n";
                sQueryString += "( " + " " + "\n";

                #region     Values
                sQueryString += "\t" + objCCRECON.ID + "," + " " + "\n"; /// 01: ID,N,10,0	
                sQueryString += "\t" + objCCRECON.SOCNUM + "," + " " + "\n";  /// 02: SOCNUM,N,5,0	
                sQueryString += "\t" + "'" + objCCRECON.SOCNOME.Trim().ToUpper() + "'" + "," + " " + "\n"; /// 03: SOCNOME,C,70	
                sQueryString += "\t" + objCCRECON.IDLOTE + "," + " " + "\n"; /// 04: IDLOTE,N,5,0	
                sQueryString += "\t" + "'" + objCCRECON.NUMLOTE + "'" + "," + " " + "\n"; /// 05: NUMLOTE,C,10	
                sQueryString += "\t" + ((objCCRECON.NUMPAG > 0) ? objCCRECON.NUMPAG : 1) + "," + " " + "\n"; /// 06: NUMPAG,N,2,0	
                sQueryString += "\t" + "'" + ((objCCRECON.AREA > 0) ? Program.SetAreaDoubleStringValue(objCCRECON.AREA) : Program.Default_Area_Double_String) + "'" + "," + " " + "\n"; /// 07: AREA,N,12,2	    
                sQueryString += "\t" + "'" + ((objCCRECON.AREAPAGAR > 0) ? Program.SetAreaDoubleStringValue(objCCRECON.AREAPAGAR) : Program.Default_Area_Double_String) + "'" + "," + " " + "\n"; /// 09: AREAPAGAR,N,12,2
                sQueryString += "\t" + "'" + ((objCCRECON.PRECOM2 > 0) ? Program.SetPayDoubleStringValue(objCCRECON.PRECOM2) : Program.Default_Pay_Double_String) + "'" + "," + " " + "\n"; /// 10: PRECOM2,N,12,3
                sQueryString += "\t" + "'" + ((objCCRECON.VALORPAGAR > 0) ? Program.SetPayDoubleStringValue(objCCRECON.VALORPAGAR) : Program.Default_Pay_Double_String) + "'" + "," + " " + "\n"; /// 11: VALORPAGAR,N,12,3
                sQueryString += "\t" + "'" + (Program.IsValidDateTime(objCCRECON.DATA) ? objCCRECON.DATA.ToString(Program.DBF_Insert_Date_Format_String) : Program.Default_Date_Str) + "'" + "," + " " + "\n"; /// 12: DATA,D

                sQueryString += "\t" + "'" + ((objCCRECON.PRECOM2P > 0) ? Program.SetPayDoubleStringValue(objCCRECON.PRECOM2P) : Program.Default_Pay_Double_String) + "'" + "," + " " + "\n"; /// 13: PRECOM2P,N,12,3
                sQueryString += "\t" + "'" + ((objCCRECON.AREAPAGO > 0) ? Program.SetAreaDoubleStringValue(objCCRECON.AREAPAGO) : Program.Default_Area_Double_String) + "'" + "," + " " + "\n"; /// 14: AREAPAGO,N,12,2	
                sQueryString += "\t" + "'" + ((objCCRECON.VALORESCUD > 0) ? Program.SetPayDoubleStringValue(objCCRECON.VALORESCUD) : Program.Default_Pay_Double_String) + "'" + "," + " " + "\n"; //VALORESCUD,N,12,3
                sQueryString += "\t" + "'" + ((objCCRECON.VALORPAGO > 0) ? Program.SetPayDoubleStringValue(objCCRECON.VALORPAGO) : Program.Default_Pay_Double_String) + "'" + "," + " " + "\n"; /// 15: VALORPAGO,N,12,3	
                sQueryString += "\t" + "'" + objCCRECON.NOTASPAGO.Trim() + "'" + ", " + " " + "\n"; /// 16: NOTASPAGO,C,140

                sQueryString += "\t" + "'" + ((objCCRECON.PRECOM2F > 0) ? Program.SetPayDoubleStringValue(objCCRECON.PRECOM2F) : Program.Default_Pay_Double_String) + "'" + "," + " " + "\n"; /// 17: PRECOM2F,N,12,3	 
                sQueryString += "\t" + "'" + ((objCCRECON.AREAFALTA > 0) ? Program.SetAreaDoubleStringValue(objCCRECON.AREAFALTA) : Program.Default_Area_Double_String) + "'" + "," + " " + "\n"; 	/// 18: AREAFALTA,N,12,2	
                sQueryString += "\t" + "'" + ((objCCRECON.VALORFALTA > 0) ? Program.SetPayDoubleStringValue(objCCRECON.VALORFALTA) : Program.Default_Pay_Double_String) + "'" + "," + " " + "\n"; /// 19: VALORFALTA,N,12,3
                sQueryString += "\t" + "'" + objCCRECON.NOTASFALTA.Trim() + "'" + ", " + " " + "\n"; /// 20: NOTASFALTA,C,140	

                sQueryString += "\t" + "'" + objCCRECON.ESTADOLIQ.Trim() + "'" + ", " + " " + "\n"; /// 21: ESTADOLIQ,C,50
                sQueryString += "\t" + "'" + objCCRECON.NOTASLIQ.Trim() + "'" + " " + " " + "\n"; /// 22: NOTASLIQ,C,140	
                #endregion  Values

                sQueryString += ") " + " " + "\n";

                #endregion  Query

                using (OleDbConnection objOleDbConnection = new OleDbConnection(OLE_DB_ConnectionString))
                {
                    objOleDbConnection.Open();
                    using (OleDbCommand objOleDbCommand = new OleDbCommand())
                    {
                        objOleDbCommand.Connection = objOleDbConnection;
                        objOleDbCommand.CommandText = sQueryString;
                        if (objOleDbCommand.ExecuteNonQuery() != 1)
                            return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message + " -> " + "QUERY: " + sQueryString, Program.ErroType.EXCEPTION, true, false);
                return false;
            }
        }

        /// <versions>04-03-2018(GesAMFC-v1.0.0.3)</versions>
        public Boolean Del_DB_Lote_RECON(AMFC_ContaCorrente_RECON objCCRECON)
        {
            String sQueryString = String.Empty;
            try
            {
                AMFCMembers objListMembers = new AMFCMembers();

                Set_OLE_DB_ConnectionString();
                if (String.IsNullOrEmpty(LibAMFC.DBF_AMFC_SOCIO_FileName))
                    OLE_DB_Settings();

                #region     Query
                sQueryString += "DELETE FROM " + "CC_RECON" + " " + "\n";
                sQueryString += "WHERE" + " " + "IDLOTE" + " = " + objCCRECON.IDLOTE + " " + "\n";
                sQueryString += "AND" + " " + "SOCNUM" + " = " + objCCRECON.SOCNUM + " " + "\n";
                #endregion  Query

                using (OleDbConnection objOleDbConnection = new OleDbConnection(OLE_DB_ConnectionString))
                {
                    objOleDbConnection.Open();
                    using (OleDbCommand objOleDbCommand = new OleDbCommand())
                    {
                        objOleDbCommand.Connection = objOleDbConnection;
                        objOleDbCommand.CommandText = sQueryString;
                        if (objOleDbCommand.ExecuteNonQuery() < 0)
                            return false;
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message + " -> " + "QUERY: " + sQueryString, Program.ErroType.EXCEPTION, true, false);
                return false;
            }
        }

        /// <versions>04-03-2018(GesAMFC-v1.0.0.3)</versions>
        public Boolean Lote_RECON_Already_Exist(AMFC_ContaCorrente_RECON objCCRECON)
        {
            String sQueryString = String.Empty;
            try
            {
                AMFCMembers objListMembers = new AMFCMembers();

                Set_OLE_DB_ConnectionString();
                if (String.IsNullOrEmpty(LibAMFC.DBF_AMFC_SOCIO_FileName))
                    OLE_DB_Settings();

                #region     Query
                sQueryString += "SELECT COUNT(*) FROM " + "CC_RECON" + " " + "\n";
                sQueryString += "WHERE" + " " + "\n";
                sQueryString += "SOCNUM" + " = " + objCCRECON.SOCNUM + " " + "\n";
                sQueryString += "AND" + " " + "(";
                if (objCCRECON.IDLOTE > 0)
                    sQueryString += "IDLOTE" + " = " + objCCRECON.IDLOTE + " OR " + " " + "\n";
                sQueryString += "NUMLOTE" + " = " + "'" + objCCRECON.NUMLOTE + "'" + ")" + " " + "\n";
                sQueryString += "; ";
                #endregion  Query

                using (OleDbConnection objOleDbConnection = new OleDbConnection(OLE_DB_ConnectionString))
                {
                    objOleDbConnection.Open();
                    using (OleDbCommand objOleDbCommand = new OleDbCommand())
                    {
                        objOleDbCommand.Connection = objOleDbConnection;
                        objOleDbCommand.CommandText = sQueryString;
                        if (Convert.ToInt32(objOleDbCommand.ExecuteScalar()) > 0)
                            return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message + " -> " + "QUERY: " + sQueryString, Program.ErroType.EXCEPTION, true, false);
                return true;
            }
        }

        /// <versions>04-03-2018(GesAMFC-v1.0.0.3)</versions>
        public Int32 Get_Lote_RECON_IDs_Max_Number()
        {
            String sQueryString = String.Empty;
            try
            {
                var iMaxNumber = -1;

                Set_OLE_DB_ConnectionString();
                if (String.IsNullOrEmpty(LibAMFC.DBF_AMFC_SOCIO_FileName))
                    OLE_DB_Settings();
                using (OleDbConnection objOleDbConnection = new OleDbConnection(OLE_DB_ConnectionString))
                {
                    objOleDbConnection.Open();
                    using (OleDbCommand objOleDbCommand = new OleDbCommand())
                    {
                        objOleDbCommand.Connection = objOleDbConnection;

                        objOleDbCommand.CommandText = "SELECT COUNT(*) FROM " + "CC_RECON" + " " + "\n";
                        if (Convert.ToInt32(objOleDbCommand.ExecuteScalar()) <= 0)
                            return 0;

                        objOleDbCommand.CommandText = "SELECT MAX(" + "ID" + ") AS MaxId FROM " + "CC_RECON" + ";";
                        iMaxNumber = Convert.ToInt32(objOleDbCommand.ExecuteScalar());
                    }
                }
                return iMaxNumber;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message + " -> " + "QUERY: " + sQueryString, Program.ErroType.EXCEPTION, true, false);
                return -1;
            }
        }

        #endregion  CC RECON


        #endregion  AMFC DB Methods
    }
}
