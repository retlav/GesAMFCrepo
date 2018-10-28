namespace GesAMFC
{
    partial class Login
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Login));
            this.TextEdit_Password = new DevExpress.XtraEditors.TextEdit();
            this.lblUserPassword = new System.Windows.Forms.Label();
            this.TextEdit_Username = new DevExpress.XtraEditors.TextEdit();
            this.lblUserName = new System.Windows.Forms.Label();
            this.Button_Cancel = new DevExpress.XtraEditors.SimpleButton();
            this.ToolTipController_Login = new DevExpress.Utils.ToolTipController(this.components);
            this.Button_OK = new DevExpress.XtraEditors.SimpleButton();
            ((System.ComponentModel.ISupportInitialize)(this.TextEdit_Password.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TextEdit_Username.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // TextEdit_Password
            // 
            this.TextEdit_Password.EditValue = "";
            this.TextEdit_Password.Location = new System.Drawing.Point(148, 44);
            this.TextEdit_Password.Name = "TextEdit_Password";
            this.TextEdit_Password.Properties.PasswordChar = '*';
            this.TextEdit_Password.Size = new System.Drawing.Size(148, 20);
            this.TextEdit_Password.TabIndex = 18;
            this.TextEdit_Password.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextEdit_Password_KeyPress);
            // 
            // lblUserPassword
            // 
            this.lblUserPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUserPassword.Location = new System.Drawing.Point(11, 43);
            this.lblUserPassword.Name = "lblUserPassword";
            this.lblUserPassword.Size = new System.Drawing.Size(130, 21);
            this.lblUserPassword.TabIndex = 22;
            this.lblUserPassword.Text = "Palavra-passe:";
            this.lblUserPassword.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // TextEdit_Username
            // 
            this.TextEdit_Username.EditValue = "";
            this.TextEdit_Username.Location = new System.Drawing.Point(148, 17);
            this.TextEdit_Username.Name = "TextEdit_Username";
            this.TextEdit_Username.Size = new System.Drawing.Size(148, 20);
            this.TextEdit_Username.TabIndex = 17;
            this.TextEdit_Username.Visible = false;
            this.TextEdit_Username.EditValueChanged += new System.EventHandler(this.TextEdit_Username_EditValueChanged);
            this.TextEdit_Username.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.TextEdit_Username_KeyPress);
            // 
            // lblUserName
            // 
            this.lblUserName.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblUserName.Location = new System.Drawing.Point(16, 18);
            this.lblUserName.Name = "lblUserName";
            this.lblUserName.Size = new System.Drawing.Size(126, 19);
            this.lblUserName.TabIndex = 21;
            this.lblUserName.Text = "Utilizador:";
            this.lblUserName.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.lblUserName.Visible = false;
            // 
            // Button_Cancel
            // 
            this.Button_Cancel.Appearance.BackColor = System.Drawing.Color.Red;
            this.Button_Cancel.Appearance.Font = new System.Drawing.Font("Verdana", 7.854546F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Button_Cancel.Appearance.Options.UseBackColor = true;
            this.Button_Cancel.Appearance.Options.UseFont = true;
            this.Button_Cancel.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.Button_Cancel.Location = new System.Drawing.Point(19, 82);
            this.Button_Cancel.LookAndFeel.SkinName = "Blue";
            this.Button_Cancel.Name = "Button_Cancel";
            this.Button_Cancel.Size = new System.Drawing.Size(123, 27);
            this.Button_Cancel.TabIndex = 23;
            this.Button_Cancel.Text = "Cancelar";
            this.Button_Cancel.Click += new System.EventHandler(this.Button_Cancel_Click);
            this.Button_Cancel.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Button_Cancel_KeyPress);
            // 
            // Button_OK
            // 
            this.Button_OK.Appearance.Font = new System.Drawing.Font("Verdana", 7.854546F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Button_OK.Appearance.Options.UseFont = true;
            this.Button_OK.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
            this.Button_OK.Location = new System.Drawing.Point(174, 82);
            this.Button_OK.LookAndFeel.SkinName = "Blue";
            this.Button_OK.Name = "Button_OK";
            this.Button_OK.Size = new System.Drawing.Size(123, 27);
            this.Button_OK.TabIndex = 24;
            this.Button_OK.Text = "OK";
            this.Button_OK.Click += new System.EventHandler(this.Button_OK_Click);
            this.Button_OK.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Button_OK_KeyPress);
            // 
            // Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Gainsboro;
            this.ClientSize = new System.Drawing.Size(317, 121);
            this.Controls.Add(this.Button_OK);
            this.Controls.Add(this.Button_Cancel);
            this.Controls.Add(this.TextEdit_Password);
            this.Controls.Add(this.lblUserPassword);
            this.Controls.Add(this.TextEdit_Username);
            this.Controls.Add(this.lblUserName);
            this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximumSize = new System.Drawing.Size(335, 165);
            this.MinimumSize = new System.Drawing.Size(335, 165);
            this.Name = "Login";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "AUTENTICAÇÃO";
            this.Activated += new System.EventHandler(this.Login_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Login_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Login_FormClosed);
            this.Load += new System.EventHandler(this.Login_Load);
            ((System.ComponentModel.ISupportInitialize)(this.TextEdit_Password.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TextEdit_Username.Properties)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private DevExpress.XtraEditors.TextEdit TextEdit_Password;
        private System.Windows.Forms.Label lblUserPassword;
        private DevExpress.XtraEditors.TextEdit TextEdit_Username;
        private System.Windows.Forms.Label lblUserName;
        private DevExpress.XtraEditors.SimpleButton Button_Cancel;
        private DevExpress.Utils.ToolTipController ToolTipController_Login;
        private DevExpress.XtraEditors.SimpleButton Button_OK;
    }
}