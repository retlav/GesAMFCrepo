using GesAMFC.AMFC_Methods;
using System;
using System.Windows.Forms;

namespace GesAMFC
{
    /// <summary>Admin Conta Corrente</summary>
    /// <author>Valter Lima</author>
    /// <creation>20-01-2018(GesAMFC-v1.0.0.3)</creation>
    /// <versions>01-03-2018(GesAMFC-v1.0.0.3)</versions>
    public partial class Admin_Lotes : DevExpress.XtraEditors.XtraForm
    {
        public Library_AMFC_Methods LibAMFC;

        private AMFCMember Member;
        private AMFCMemberLotes ListMemberLotes;

        #region     Form Constructor 

        /// <versions>05-05-2017(v0.0.3.2)</versions>
        public Admin_Lotes()
        {
            LibAMFC = new Library_AMFC_Methods();

            try
            {
                InitializeComponent();

                LayoutControl_Global_Lotes.Visible = false;

                this.Member = new AMFCMember();
                this.ListMemberLotes = new AMFCMemberLotes();
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        #endregion  Form Constructor

        #region     Events

        #region     Form Events

        /// <versions>20-01-2018(GesAMFC-v1.0.0.3)</versions>
        private void Admin_Form_Load(object sender, EventArgs e)
        {
            try
            {
                #region     Tem de ser feito aqui senão crasha -> No futuro, depois de ter o menu, comentar !!!
                this.WindowState = FormWindowState.Maximized;
                this.Update();
                #endregion

                Load_Member();
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>28-02-2018(GesAMFC-v1.0.0.3)</versions>
        private void Button_Procurar_Socio_Click(object sender, EventArgs e)
        {
            Load_Member();
        }

        private void Button_Socio_Carregar_Click(object sender, EventArgs e)
        {
            Get_Member_Lotes();
        }

        #endregion  Form Events

        #region     Events LOTES

        /// <versions>01-03-2018(GesAMFC-v1.0.0.3)</versions>
        private void SpinEdit_Total_Lotes_EditValueChanged(object sender, EventArgs e)
        {
            Set_Member_Lotes_Total_Number();
        }

        /// <versions>01-03-2018(GesAMFC-v1.0.0.3)</versions>
        private void Button_Guardar_TS_Lote01_Click(object sender, EventArgs e)
        {
            Save_Member_Lote_Info(1);
        }

        /// <versions>01-03-2018(GesAMFC-v1.0.0.3)</versions>
        private void Button_Guardar_TS_Lote02_Click(object sender, EventArgs e)
        {
            Save_Member_Lote_Info(2);
        }

        /// <versions>01-03-2018(GesAMFC-v1.0.0.3)</versions>
        private void Button_Guardar_TS_Lote03_Click(object sender, EventArgs e)
        {
            Save_Member_Lote_Info(3);
        }

        private void Button_Guardar_TS_Lote04_Click(object sender, EventArgs e)
        {
            Save_Member_Lote_Info(4);
        }

        /// <versions>01-03-2018(GesAMFC-v1.0.0.3)</versions>
        private void Button_Guardar_TS_Lote05_Click(object sender, EventArgs e)
        {
            Save_Member_Lote_Info(5);
        }

        #endregion  Events LOTES

        #endregion  Events

        #region     Private Methods

        /// <versions>28-02-2018(GesAMFC-v1.0.0.3)</versions>
        private void Load_Member()
        {
            try
            {
                Find_Member_Form objFindMemberForm = new Find_Member_Form();
                if (objFindMemberForm != null)
                {
                    objFindMemberForm.FormClosing += delegate
                    {
                        if (Program.Member_Found) //trocar por objFindMemberForm.Member_Found e remover Program.Member_Found no futuro
                        {
                            AMFCMember objMemberSelected = objFindMemberForm.MemberSelected;
                            if (objMemberSelected != null && objMemberSelected.NUMERO >= objMemberSelected.MinNumber && objMemberSelected.NUMERO <= objMemberSelected.MaxNumber)
                            {
                                this.Member = objMemberSelected;
                                TextEdit_Lotes_Socio_Numero.Text = objMemberSelected.NUMERO.ToString();
                                TextEdit_Lotes_Socio_Nome.Text = objMemberSelected.NOME;
                                LayoutControl_Global_Lotes.Visible = true;
                                Get_Member_Lotes();
                            }
                        }
                    };
                    objFindMemberForm.Show();
                    objFindMemberForm.StartPosition = FormStartPosition.CenterParent;
                    objFindMemberForm.Focus();
                    objFindMemberForm.BringToFront();
                    objFindMemberForm.Update();
                }
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return;
            }
        }

        private void Clear_All_Controls()
        {
            try
            {
                SpinEdit_Total_Lotes.Value = 1;

                #region     Member Lotes
                TextEdit_TS_Lote01_Numero.Text = "";
                TextEdit_TS_Lote01_Id.Text = "";
                TextEdit_TS_Lote01_Morada.Text = "";
                SpinEdit_TS_Lote01_Lotes_Total.Value = 1;
                SpinEdit_TS_Lote01_Fogos_Total.Value = 1;
                TextEdit_TS_Lote01_Area_Real.Text = "";
                TextEdit_TS_Lote01_Area_Pagar.Text = "";
                CheckEdit_TS_Lote01_Gaveto.Checked = false;
                CheckEdit_TS_Lote01_Quintinha.Checked = false;
                TextEdit_TS_Lote01_Notas.Text = "";
                #endregion  Member Lotes

                LayoutControl_Global_Lotes.Visible = false;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        #region     LOTES

        /// <versions>28-02-2018(GesAMFC-v1.0.0.3)</versions>
        private void Get_Member_Lotes()
        {
            try
            {
                if (this.Member == null || this.Member.NUMERO < this.Member.MinNumber || this.Member.NUMERO > this.Member.MaxNumber)
                {
                    String sErrorMsg = "Não foi obter o Sócio!";
                    Program.HandleError("", sErrorMsg, Program.ErroType.ERROR, true, true);
                    Clear_All_Controls();
                    return;
                }

                ListMemberLotes = Get_DBF_AMFC_Member_Lotes(this.Member.NUMERO);
                if (ListMemberLotes == null || ListMemberLotes.Lotes.Count == 0)
                {
                    String sErrorMsg = "Não foi possível obter a Lista de Lotes do Sócio Nº: " + this.Member.NUMERO;
                    Program.HandleError("", sErrorMsg, Program.ErroType.ERROR, true, true);
                    Clear_All_Controls();
                    return;
                }

                Set_Member_Lotes_Info();
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>28-02-2018(GesAMFC-v1.0.0.3)</versions>
        private AMFCMemberLotes Get_DBF_AMFC_Member_Lotes(Int64 lMemberNumber)
        {
            try
            {
                using (Library_AMFC_SQL obj_AMFC_SQL = new Library_AMFC_SQL())
                    return obj_AMFC_SQL.Get_List_Member_Lotes(lMemberNumber);
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return null;
            }
        }

        /// <versions>01-03-2018(GesAMFC-v1.0.0.3)</versions>
        private void Set_Member_Lotes_Info()
        {
            try
            {
                if (ListMemberLotes == null || ListMemberLotes.Lotes.Count == 0)
                    return;

                LayoutControl_Global_Lotes.Visible = true;

                LayoutControlGroup_TS_Lote01.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                LayoutControlGroup_TS_Lote02.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                LayoutControlGroup_TS_Lote03.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                LayoutControlGroup_TS_Lote04.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                LayoutControlGroup_TS_Lote05.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;

                Int32 iMaxTerrenos = 5;
                if (ListMemberLotes.Lotes.Count > iMaxTerrenos)
                    return;
                SpinEdit_Total_Lotes.Value = ListMemberLotes.Lotes.Count;

                for (Int32 iLote = 1; iLote <= ListMemberLotes.Lotes.Count; iLote++)
                {
                    if (iLote > iMaxTerrenos)
                        break;
                    AMFCMemberLote objLote = ListMemberLotes.Lotes[iLote - 1];

                    switch (iLote)
                    {
                        case 1:
                            LayoutControlGroup_TS_Lote01.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                            LayoutControlGroup_TS_Lote01.Expanded = true;
                            TextEdit_TS_Lote01_Numero.Text = objLote.NUMLOTE;
                            TextEdit_TS_Lote01_Id.Text = objLote.IDLOTE.ToString();
                            TextEdit_TS_Lote01_Morada.Text = objLote.MORLOTE;
                            SpinEdit_TS_Lote01_Lotes_Total.Value = objLote.TOTALLOTES > 0 ? objLote.TOTALLOTES : 1;
                            SpinEdit_TS_Lote01_Fogos_Total.Value = objLote.TOTALFOGOS > 0 ? objLote.TOTALFOGOS : 1;
                            TextEdit_TS_Lote01_Area_Real.Text = objLote.AREALOTES > 0 ? Program.SetAreaDoubleStringValue(objLote.AREALOTES) : "0";
                            if (objLote.AREAPAGAR <= 0)
                                objLote.AREAPAGAR = objLote.AREALOTES;
                            TextEdit_TS_Lote01_Area_Pagar.Text = objLote.AREAPAGAR > 0 ? Program.SetAreaDoubleStringValue(objLote.AREAPAGAR) : "0";
                            CheckEdit_TS_Lote01_Gaveto.Checked = objLote.GAVETO;
                            CheckEdit_TS_Lote01_Quintinha.Checked = objLote.QUINTINHA;
                            TextEdit_TS_Lote01_Notas.Text = objLote.OBSERVACAO + " " + objLote.OBSERV2 + " " + objLote.OBSERV3;
                            break;
                        case 2:
                            LayoutControlGroup_TS_Lote02.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                            LayoutControlGroup_TS_Lote02.Expanded = true;
                            TextEdit_TS_Lote02_Numero.Text = objLote.NUMLOTE;
                            TextEdit_TS_Lote02_Id.Text = objLote.IDLOTE.ToString();
                            TextEdit_TS_Lote02_Morada.Text = objLote.MORLOTE;
                            SpinEdit_TS_Lote02_Lotes_Total.Value = objLote.TOTALLOTES > 0 ? objLote.TOTALLOTES : 1;
                            SpinEdit_TS_Lote02_Fogos_Total.Value = objLote.TOTALFOGOS > 0 ? objLote.TOTALFOGOS : 1;
                            TextEdit_TS_Lote02_Area_Real.Text = objLote.AREALOTES > 0 ? Program.SetAreaDoubleStringValue(objLote.AREALOTES) : "0";
                            if (objLote.AREAPAGAR <= 0)
                                objLote.AREAPAGAR = objLote.AREALOTES;
                            TextEdit_TS_Lote02_Area_Pagar.Text = objLote.AREAPAGAR > 0 ? Program.SetAreaDoubleStringValue(objLote.AREAPAGAR) : "0";
                            CheckEdit_TS_Lote02_Gaveto.Checked = objLote.GAVETO;
                            CheckEdit_TS_Lote02_Quintinha.Checked = objLote.QUINTINHA;
                            TextEdit_TS_Lote02_Notas.Text = objLote.OBSERVACAO + " " + objLote.OBSERV2 + " " + objLote.OBSERV3;
                            break;
                        case 3:
                            LayoutControlGroup_TS_Lote03.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                            LayoutControlGroup_TS_Lote03.Expanded = true;
                            TextEdit_TS_Lote03_Numero.Text = objLote.NUMLOTE;
                            TextEdit_TS_Lote03_Id.Text = objLote.IDLOTE.ToString();
                            TextEdit_TS_Lote03_Morada.Text = objLote.MORLOTE;
                            SpinEdit_TS_Lote03_Lotes_Total.Value = objLote.TOTALLOTES > 0 ? objLote.TOTALLOTES : 1;
                            SpinEdit_TS_Lote03_Fogos_Total.Value = objLote.TOTALFOGOS > 0 ? objLote.TOTALFOGOS : 1;
                            TextEdit_TS_Lote03_Area_Real.Text = objLote.AREALOTES > 0 ? Program.SetAreaDoubleStringValue(objLote.AREALOTES) : "0";
                            if (objLote.AREAPAGAR <= 0)
                                objLote.AREAPAGAR = objLote.AREALOTES;
                            TextEdit_TS_Lote03_Area_Pagar.Text = objLote.AREAPAGAR > 0 ? Program.SetAreaDoubleStringValue(objLote.AREAPAGAR) : "0";
                            CheckEdit_TS_Lote03_Gaveto.Checked = objLote.GAVETO;
                            CheckEdit_TS_Lote03_Quintinha.Checked = objLote.QUINTINHA;
                            TextEdit_TS_Lote03_Notas.Text = objLote.OBSERVACAO + " " + objLote.OBSERV2 + " " + objLote.OBSERV3;
                            break;
                        case 4:
                            LayoutControlGroup_TS_Lote04.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                            LayoutControlGroup_TS_Lote04.Expanded = true;
                            TextEdit_TS_Lote04_Numero.Text = objLote.NUMLOTE;
                            TextEdit_TS_Lote04_Id.Text = objLote.IDLOTE.ToString();
                            TextEdit_TS_Lote04_Morada.Text = objLote.MORLOTE;
                            SpinEdit_TS_Lote04_Lotes_Total.Value = objLote.TOTALLOTES > 0 ? objLote.TOTALLOTES : 1;
                            SpinEdit_TS_Lote04_Fogos_Total.Value = objLote.TOTALFOGOS > 0 ? objLote.TOTALFOGOS : 1;
                            TextEdit_TS_Lote04_Area_Real.Text = objLote.AREALOTES > 0 ? Program.SetAreaDoubleStringValue(objLote.AREALOTES) : "0";
                            if (objLote.AREAPAGAR <= 0)
                                objLote.AREAPAGAR = objLote.AREALOTES;
                            TextEdit_TS_Lote04_Area_Pagar.Text = objLote.AREAPAGAR > 0 ? Program.SetAreaDoubleStringValue(objLote.AREAPAGAR) : "0";
                            CheckEdit_TS_Lote04_Gaveto.Checked = objLote.GAVETO;
                            CheckEdit_TS_Lote04_Quintinha.Checked = objLote.QUINTINHA;
                            TextEdit_TS_Lote04_Notas.Text = objLote.OBSERVACAO + " " + objLote.OBSERV2 + " " + objLote.OBSERV3;
                            break;
                        case 5:
                            LayoutControlGroup_TS_Lote05.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                            LayoutControlGroup_TS_Lote05.Expanded = true;
                            TextEdit_TS_Lote05_Numero.Text = objLote.NUMLOTE;
                            TextEdit_TS_Lote05_Id.Text = objLote.IDLOTE.ToString();
                            TextEdit_TS_Lote05_Morada.Text = objLote.MORLOTE;
                            SpinEdit_TS_Lote05_Lotes_Total.Value = objLote.TOTALLOTES > 0 ? objLote.TOTALLOTES : 1;
                            SpinEdit_TS_Lote05_Fogos_Total.Value = objLote.TOTALFOGOS > 0 ? objLote.TOTALFOGOS : 1;
                            TextEdit_TS_Lote05_Area_Real.Text = objLote.AREALOTES > 0 ? Program.SetAreaDoubleStringValue(objLote.AREALOTES) : "0";
                            if (objLote.AREAPAGAR <= 0)
                                objLote.AREAPAGAR = objLote.AREALOTES;
                            TextEdit_TS_Lote05_Area_Pagar.Text = objLote.AREAPAGAR > 0 ? Program.SetAreaDoubleStringValue(objLote.AREAPAGAR) : "0";
                            CheckEdit_TS_Lote05_Gaveto.Checked = objLote.GAVETO;
                            CheckEdit_TS_Lote05_Quintinha.Checked = objLote.QUINTINHA;
                            TextEdit_TS_Lote05_Notas.Text = objLote.OBSERVACAO + " " + objLote.OBSERV2 + " " + objLote.OBSERV3;
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        /// <versions>01-03-2018(GesAMFC-v1.0.0.3)</versions>
        private void Set_Member_Lotes_Total_Number()
        {
            try
            {
                Int32 iTotalTerrenos = Convert.ToInt32(SpinEdit_Total_Lotes.Value);
                LayoutControlGroup_TS.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                LayoutControlGroup_TS.Expanded = true;
                switch (iTotalTerrenos)
                {
                    case 1:
                        LayoutControlGroup_TS_Lote01.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                        LayoutControlGroup_TS_Lote02.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                        LayoutControlGroup_TS_Lote03.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                        LayoutControlGroup_TS_Lote04.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                        LayoutControlGroup_TS_Lote05.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                        LayoutControlGroup_TS_Lote01.Expanded = true;
                        break;
                    case 2:
                        LayoutControlGroup_TS_Lote01.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                        LayoutControlGroup_TS_Lote02.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                        LayoutControlGroup_TS_Lote03.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                        LayoutControlGroup_TS_Lote04.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                        LayoutControlGroup_TS_Lote05.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                        LayoutControlGroup_TS_Lote01.Expanded = true;
                        LayoutControlGroup_TS_Lote02.Expanded = true;
                        break;
                    case 3:
                        LayoutControlGroup_TS_Lote01.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                        LayoutControlGroup_TS_Lote02.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                        LayoutControlGroup_TS_Lote03.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                        LayoutControlGroup_TS_Lote04.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                        LayoutControlGroup_TS_Lote05.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                        LayoutControlGroup_TS_Lote01.Expanded = true;
                        LayoutControlGroup_TS_Lote02.Expanded = true;
                        LayoutControlGroup_TS_Lote03.Expanded = true;
                        break;
                    case 4:
                        LayoutControlGroup_TS_Lote01.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                        LayoutControlGroup_TS_Lote02.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                        LayoutControlGroup_TS_Lote03.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                        LayoutControlGroup_TS_Lote04.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                        LayoutControlGroup_TS_Lote05.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Never;
                        LayoutControlGroup_TS_Lote01.Expanded = true;
                        LayoutControlGroup_TS_Lote02.Expanded = true;
                        LayoutControlGroup_TS_Lote03.Expanded = true;
                        LayoutControlGroup_TS_Lote04.Expanded = true;
                        break;
                    case 5:
                        LayoutControlGroup_TS_Lote01.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                        LayoutControlGroup_TS_Lote02.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                        LayoutControlGroup_TS_Lote03.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                        LayoutControlGroup_TS_Lote04.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                        LayoutControlGroup_TS_Lote05.Visibility = DevExpress.XtraLayout.Utils.LayoutVisibility.Always;
                        LayoutControlGroup_TS_Lote01.Expanded = true;
                        LayoutControlGroup_TS_Lote02.Expanded = true;
                        LayoutControlGroup_TS_Lote03.Expanded = true;
                        LayoutControlGroup_TS_Lote04.Expanded = true;
                        LayoutControlGroup_TS_Lote05.Expanded = true;
                        break;
                }
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
            }
        }

        private Boolean Save_Member_Lote_Info(Int32 iLoteIdx)
        {
            try
            {
                if (this.Member == null || this.Member.NUMERO < this.Member.MinNumber || this.Member.NUMERO > this.Member.MaxNumber)
                    return false;

                Int64 lLoteID = -1;
                switch (iLoteIdx)
                {
                    case 1:
                        if (!String.IsNullOrEmpty(TextEdit_TS_Lote01_Id.Text.Trim()))
                            lLoteID = Convert.ToInt64(TextEdit_TS_Lote01_Id.Text.Trim());
                        break;
                    case 2:
                        if (!String.IsNullOrEmpty(TextEdit_TS_Lote02_Id.Text.Trim()))
                            lLoteID = Convert.ToInt64(TextEdit_TS_Lote02_Id.Text.Trim());
                        break;
                    case 3:
                        if (!String.IsNullOrEmpty(TextEdit_TS_Lote03_Id.Text.Trim()))
                            lLoteID = Convert.ToInt64(TextEdit_TS_Lote03_Id.Text.Trim());
                        break;
                    case 4:
                        if (!String.IsNullOrEmpty(TextEdit_TS_Lote04_Id.Text.Trim()))
                            lLoteID = Convert.ToInt64(TextEdit_TS_Lote04_Id.Text.Trim());
                        break;
                    case 5:
                        if (!String.IsNullOrEmpty(TextEdit_TS_Lote05_Id.Text.Trim()))
                            lLoteID = Convert.ToInt64(TextEdit_TS_Lote05_Id.Text.Trim());
                        break;
                }

                AMFCMemberLote objLote = this.ListMemberLotes.GetLoteById(lLoteID);
                if (lLoteID < 1 || objLote == null)
                    objLote = new AMFCMemberLote();

                #region     Member Info
                if (objLote.SOCNUM < 1)
                    objLote.SOCNUM = this.Member.NUMERO;
                if (String.IsNullOrEmpty(objLote.SOCNOME))
                    objLote.SOCNOME = this.Member.NOME;
                #endregion  Member Info

                #region     Set Lote Info to Add/Update
                switch (iLoteIdx)
                {
                    case 1:
                        objLote.INDEXLOTE = 1;
                        objLote.NUMLOTE = TextEdit_TS_Lote01_Numero.Text.Trim();
                        objLote.MORLOTE = TextEdit_TS_Lote01_Morada.Text.Trim();
                        objLote.TOTALLOTES = Convert.ToInt32(SpinEdit_TS_Lote01_Lotes_Total.Value);
                        objLote.TOTALFOGOS = Convert.ToInt32(SpinEdit_TS_Lote01_Fogos_Total.Value);
                        objLote.AREALOTES = Program.SetAreaDoubleValue(TextEdit_TS_Lote01_Area_Real.Text.Trim());
                        objLote.AREAPAGAR = Program.SetAreaDoubleValue(TextEdit_TS_Lote01_Area_Pagar.Text.Trim());
                        objLote.GAVETO = CheckEdit_TS_Lote01_Gaveto.Checked;
                        objLote.QUINTINHA = CheckEdit_TS_Lote01_Quintinha.Checked;
                        objLote.OBSERVACAO = TextEdit_TS_Lote01_Notas.Text;
                        break;
                    case 2:
                        objLote.INDEXLOTE = 2;
                        objLote.NUMLOTE = TextEdit_TS_Lote02_Numero.Text.Trim();
                        objLote.MORLOTE = TextEdit_TS_Lote02_Morada.Text.Trim();
                        objLote.TOTALLOTES = Convert.ToInt32(SpinEdit_TS_Lote02_Lotes_Total.Value);
                        objLote.TOTALFOGOS = Convert.ToInt32(SpinEdit_TS_Lote02_Fogos_Total.Value);
                        objLote.AREALOTES = Program.SetAreaDoubleValue(TextEdit_TS_Lote02_Area_Real.Text.Trim());
                        objLote.AREAPAGAR = Program.SetAreaDoubleValue(TextEdit_TS_Lote02_Area_Pagar.Text.Trim());
                        objLote.GAVETO = CheckEdit_TS_Lote02_Gaveto.Checked;
                        objLote.QUINTINHA = CheckEdit_TS_Lote02_Quintinha.Checked;
                        objLote.OBSERVACAO = TextEdit_TS_Lote02_Notas.Text;
                        break;
                    case 3:
                        objLote.INDEXLOTE = 3;
                        objLote.NUMLOTE = TextEdit_TS_Lote03_Numero.Text.Trim();
                        objLote.MORLOTE = TextEdit_TS_Lote03_Morada.Text.Trim();
                        objLote.TOTALLOTES = Convert.ToInt32(SpinEdit_TS_Lote03_Lotes_Total.Value);
                        objLote.TOTALFOGOS = Convert.ToInt32(SpinEdit_TS_Lote03_Fogos_Total.Value);
                        objLote.AREALOTES = Program.SetAreaDoubleValue(TextEdit_TS_Lote03_Area_Real.Text.Trim());
                        objLote.AREAPAGAR = Program.SetAreaDoubleValue(TextEdit_TS_Lote03_Area_Pagar.Text.Trim());
                        objLote.GAVETO = CheckEdit_TS_Lote03_Gaveto.Checked;
                        objLote.QUINTINHA = CheckEdit_TS_Lote03_Quintinha.Checked;
                        objLote.OBSERVACAO = TextEdit_TS_Lote03_Notas.Text;
                        break;
                    case 4:
                        objLote.INDEXLOTE = 4;
                        objLote.NUMLOTE = TextEdit_TS_Lote04_Numero.Text.Trim();
                        objLote.MORLOTE = TextEdit_TS_Lote04_Morada.Text.Trim();
                        objLote.TOTALLOTES = Convert.ToInt32(SpinEdit_TS_Lote04_Lotes_Total.Value);
                        objLote.TOTALFOGOS = Convert.ToInt32(SpinEdit_TS_Lote04_Fogos_Total.Value);
                        objLote.AREALOTES = Program.SetAreaDoubleValue(TextEdit_TS_Lote04_Area_Real.Text.Trim());
                        objLote.AREAPAGAR = Program.SetAreaDoubleValue(TextEdit_TS_Lote04_Area_Pagar.Text.Trim());
                        objLote.GAVETO = CheckEdit_TS_Lote04_Gaveto.Checked;
                        objLote.QUINTINHA = CheckEdit_TS_Lote04_Quintinha.Checked;
                        objLote.OBSERVACAO = TextEdit_TS_Lote04_Notas.Text;
                        break;
                    case 5:
                        objLote.INDEXLOTE = 5;
                        objLote.NUMLOTE = TextEdit_TS_Lote05_Numero.Text.Trim();
                        objLote.MORLOTE = TextEdit_TS_Lote05_Morada.Text.Trim();
                        objLote.TOTALLOTES = Convert.ToInt32(SpinEdit_TS_Lote05_Lotes_Total.Value);
                        objLote.TOTALFOGOS = Convert.ToInt32(SpinEdit_TS_Lote05_Fogos_Total.Value);
                        objLote.AREALOTES = Program.SetAreaDoubleValue(TextEdit_TS_Lote05_Area_Real.Text.Trim());
                        objLote.AREAPAGAR = Program.SetAreaDoubleValue(TextEdit_TS_Lote05_Area_Pagar.Text.Trim());
                        objLote.GAVETO = CheckEdit_TS_Lote05_Gaveto.Checked;
                        objLote.QUINTINHA = CheckEdit_TS_Lote05_Quintinha.Checked;
                        objLote.OBSERVACAO = TextEdit_TS_Lote05_Notas.Text;
                        break;
                }
                #endregion  Set Lote Info to Add/Update

                #region     Add Lote SQL Info
                if (Set_DBF_AMFC_Member_Lote(objLote))
                {
                    String sMessageOK = "Informação guardada com sucesso relativamente ao Lote Nº: " + objLote.NUMLOTE + " (ID=" + objLote.IDLOTE + ")" + " do Sócio: " + objLote.SOCNOME + " Nº: " + objLote.SOCNUM;
                    MessageBox.Show(sMessageOK, "Lote" + " Guardado", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    String sMessageERROR = "Não foi possível guardar a informação relativamente ao Lote Nº: " + objLote.NUMLOTE + " (ID=" + objLote.IDLOTE + ")" + " do Sócio: " + objLote.SOCNOME + " Nº: " + objLote.SOCNUM;
                    MessageBox.Show(sMessageERROR, "Lote" + " Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                #endregion

                #region     Update Member Lotes
                Get_Member_Lotes();
                #endregion

                return true;
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return false;
            }
        }

        /// <versions>28-02-2018(GesAMFC-v1.0.0.3)</versions>
        private Boolean Set_DBF_AMFC_Member_Lote(AMFCMemberLote objLote)
        {
            try
            {
                using (Library_AMFC_SQL obj_AMFC_SQL = new Library_AMFC_SQL())
                    return obj_AMFC_SQL.Save_Member_DB_Lote(objLote);
            }
            catch (Exception ex)
            {
                Program.HandleError(ex.TargetSite.Name, ex.Message, Program.ErroType.EXCEPTION, true, false);
                return false;
            }
        }

        #endregion  LOTES

        #endregion  Private Methods
    }
}