using System;
using System.Windows.Forms;
using DevExpress.Utils;
using DevExpress.XtraEditors;
using System.Collections.Generic;

namespace GesAMFC
{
    /// <summary>Application Login Form</summary>
    /// <creation>18-02-2017(v0.0.1.0)</creation>
    /// <versions>16-06-2017(v0.0.4.1)</versions>
    public partial class Login : Form
    {
        #region Constructor

        /// <versions>14-03-2017(v0.0.2.6)</versions
        public Login()
        {
            InitializeComponent();
        }

        #endregion Constructor

        #region Events

        #region Form

        /// <versions>14-03-2017(v0.0.2.6)</versions
        private void Login_Load(object sender, EventArgs e)
        {
            #region DEBUG -> Automatic login
#if DEBUG
            TextEdit_Password.Text = "valter9"; //Debug
            SubmitForm();
#endif
            #endregion DEBUG -> Automatic login
        }

        /// <versions>14-03-2017(v0.0.2.6)</versions
        private void Login_Activated(object sender, EventArgs e)
        {
            TextEdit_Password.Focus();
        }

        /// <versions>14-03-2017(v0.0.2.6)</versions
        private void Login_FormClosing(object sender, FormClosingEventArgs e)
        {
            try
            {
                if (!Program.UserLogged)
                {
                    if (!Program.Dialog_Exit_Application())
                        e.Cancel = true;
                }
            }
            catch
            {
                Program.Exit_Application();
            }
        }

        /// <versions>14-03-2017(v0.0.2.6)</versions
        private void Login_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (!Program.UserLogged)
                Program.Exit_Application();
        }

        #endregion Form

        #region TextBoxes

        /// <versions>14-03-2017(v0.0.2.6)</versions
        private void TextEdit_Username_KeyPress(object sender, KeyPressEventArgs e)
        {
            try { TextEdit_KeyPress(this.TextEdit_Username, ((Int32)e.KeyChar == 13), ((Int32)e.KeyChar == 27)); }
            catch (Exception ex) { Program.WriteLog(ex.TargetSite.Name, ex.Message, true, true, true, true); }
        }

        /// <versions>14-03-2017(v0.0.2.6)</versions
        private void TextEdit_Password_KeyPress(object sender, KeyPressEventArgs e)
        {
            try { TextEdit_KeyPress(this.TextEdit_Password, ((Int32)e.KeyChar == 13), ((Int32)e.KeyChar == 27)); }
            catch (Exception ex) { Program.WriteLog(ex.TargetSite.Name, ex.Message, true, true, true, true); }
        }

        #endregion TextBoxes

        #region Buttons

        /// <versions>14-03-2017(v0.0.2.6)</versions
        private void Button_OK_KeyPress(object sender, KeyPressEventArgs e)
        {
            try { TextEdit_KeyPress(this.TextEdit_Username, ((Int32)e.KeyChar == 13), ((Int32)e.KeyChar == 27)); }
            catch (Exception ex) { Program.WriteLog(ex.TargetSite.Name, ex.Message, true, true, true, true); }
        }

        /// <versions>14-03-2017(v0.0.2.6)</versions
        private void Button_Cancel_KeyPress(object sender, KeyPressEventArgs e)
        {
            try { TextEdit_KeyPress(this.TextEdit_Password, ((Int32)e.KeyChar == 13), ((Int32)e.KeyChar == 27)); }
            catch (Exception ex) { Program.WriteLog(ex.TargetSite.Name, ex.Message, true, true, true, true); }
        }

        /// <versions>14-03-2017(v0.0.2.6)</versions
        private void Button_OK_Click(object sender, EventArgs e) { SubmitForm(); }

        /// <versions>14-03-2017(v0.0.2.6)</versions
        private void Button_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #endregion Buttons

        #endregion Events

        #region Private Methods

        /// <versions>14-03-2017(v0.0.2.6)</versions
        private void TextEdit_KeyPress(TextEdit objTextEdit, Boolean bEnterKeyPressed, Boolean bEscKeyPressed)
        {
            try
            {
                SetToolTipHint(false, objTextEdit);
                if (bEnterKeyPressed) SubmitForm();
                else if (bEscKeyPressed)
                    this.Close();
                else SetToolTipHint(true, objTextEdit);
            }
            catch (Exception ex) { Program.WriteLog(ex.TargetSite.Name, ex.Message, true, true, true, true); }
        }

        /// <versions>14-03-2017(v0.0.2.6)</versions
        private void SetToolTipHint(Boolean bShowHint, TextEdit objTextBox)
        {
            try
            {
                String sCapsOnWarning = "CAPS LOCK ligado!";
                if (bShowHint) { if (Console.CapsLock) ToolTipController_Login.ShowHint(sCapsOnWarning, objTextBox, ToolTipLocation.RightCenter); }
                else
                    ToolTipController_Login.HideHint();
            }
            catch (Exception ex) { Program.WriteLog(ex.TargetSite.Name, ex.Message, true, true, true, true); }
        }

        /// <versions>14-03-2017(v0.0.2.6)</versions
        private void SubmitForm()
        {
            try
            {
                String sPasswordEntered = this.TextEdit_Password.Text.Trim().ToLower();
                Int32 iLoginStatus = Program.LoginUser(sPasswordEntered);
                
                String sCaption = String.Empty;
                String sMessage = String.Empty;
                switch (iLoginStatus)
                {
                    case -1:
                        sCaption = "Erro de autenticação";
                        sMessage = "Ocorreu um erro no processo de autenticação!";
                        sMessage += "\n";
                        sMessage += "Por favor, tente novamente.";
                        sMessage += "\n";
                        sMessage += "Em caso de permanência do erro, contacte o programador. Obrigado!";
                        MessageBox.Show(sMessage, sCaption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        break;
                    case -2:
                        sCaption = "Autenticação Incorreta";
                        sMessage = "Palavra-passe incorrecta!";
                        sMessage += "\n";
                        sMessage += "Por favor, tente novamente.";
                        MessageBox.Show(sMessage, sCaption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        this.TextEdit_Password.Focus();
                        this.TextEdit_Password.SelectAll();
                        break;
                    case -3:
                        sCaption = "Password Incorreta";
                        sMessage = "Palavra-passe vazia!";
                        sMessage += "\n";
                        sMessage += "Por favor, tente novamente.";
                        MessageBox.Show(sMessage, sCaption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        this.TextEdit_Password.Focus();
                        this.TextEdit_Password.SelectAll();
                        break;
                    //case -4:
                    //    sCaption = "Versão da aplicação inválida!";
                    //    sMessage = "Esta versão da aplicação já não se encontra válida!";
                    //    sMessage += "\n";
                    //    sMessage += "Por favor, contacte o programador.";
                    //    MessageBox.Show(sMessage, sCaption, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    //    break;
                    case 1:
                        Program.UserLogged = true;
                        this.Close();
                        break;
                    default:
                        this.Close();
                        break;
                }
            }
            catch (Exception ex)
            {
                Program.WriteLog(ex.TargetSite.Name, ex.Message, true, true, true, true);
            }
        }

        #endregion Private Methods

        private void TextEdit_Username_EditValueChanged(object sender, EventArgs e)
        {

        }
    }
}
