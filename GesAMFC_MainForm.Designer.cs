namespace GesAMFC
{
    partial class GesAMFC_MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GesAMFC_MainForm));
            this.defaultLookAndFeel1 = new DevExpress.LookAndFeel.DefaultLookAndFeel(this.components);
            this.imageMenuList = new System.Windows.Forms.ImageList(this.components);
            this.barManagerAdexEntities = new DevExpress.XtraBars.BarManager(this.components);
            this.BarStatus = new DevExpress.XtraBars.Bar();
            this.BarStaticItemCopyRight = new DevExpress.XtraBars.BarStaticItem();
            this.BarStaticItemDateTime = new DevExpress.XtraBars.BarStaticItem();
            this.BarMainMenu = new DevExpress.XtraBars.Bar();
            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            this.AMFC_Main_Login_SplitContainer = new System.Windows.Forms.SplitContainer();
            this.AMFC_PictureBox_Logo = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.barManagerAdexEntities)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.AMFC_Main_Login_SplitContainer)).BeginInit();
            this.AMFC_Main_Login_SplitContainer.Panel1.SuspendLayout();
            this.AMFC_Main_Login_SplitContainer.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.AMFC_PictureBox_Logo)).BeginInit();
            this.SuspendLayout();
            // 
            // defaultLookAndFeel1
            // 
            this.defaultLookAndFeel1.LookAndFeel.SkinName = "The Asphalt World";
            // 
            // imageMenuList
            // 
            this.imageMenuList.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageMenuList.ImageStream")));
            this.imageMenuList.TransparentColor = System.Drawing.Color.LightBlue;
            this.imageMenuList.Images.SetKeyName(0, "AdexEntitiesLogo_32x32.png");
            // 
            // barManagerAdexEntities
            // 
            this.barManagerAdexEntities.AllowCustomization = false;
            this.barManagerAdexEntities.AllowMoveBarOnToolbar = false;
            this.barManagerAdexEntities.AllowQuickCustomization = false;
            this.barManagerAdexEntities.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
            this.BarStatus,
            this.BarMainMenu});
            this.barManagerAdexEntities.DockControls.Add(this.barDockControlTop);
            this.barManagerAdexEntities.DockControls.Add(this.barDockControlBottom);
            this.barManagerAdexEntities.DockControls.Add(this.barDockControlLeft);
            this.barManagerAdexEntities.DockControls.Add(this.barDockControlRight);
            this.barManagerAdexEntities.DockWindowTabFont = new System.Drawing.Font("Verdana", 26.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.barManagerAdexEntities.Form = this;
            this.barManagerAdexEntities.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
            this.BarStaticItemCopyRight,
            this.BarStaticItemDateTime});
            this.barManagerAdexEntities.MainMenu = this.BarMainMenu;
            this.barManagerAdexEntities.MaxItemId = 18;
            this.barManagerAdexEntities.StatusBar = this.BarStatus;
            // 
            // BarStatus
            // 
            this.BarStatus.BarName = "Status bar";
            this.BarStatus.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom;
            this.BarStatus.DockCol = 0;
            this.BarStatus.DockRow = 0;
            this.BarStatus.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
            this.BarStatus.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
            new DevExpress.XtraBars.LinkPersistInfo(this.BarStaticItemCopyRight),
            new DevExpress.XtraBars.LinkPersistInfo(this.BarStaticItemDateTime)});
            this.BarStatus.OptionsBar.AllowQuickCustomization = false;
            this.BarStatus.OptionsBar.DrawDragBorder = false;
            this.BarStatus.OptionsBar.UseWholeRow = true;
            this.BarStatus.Text = "Status bar";
            // 
            // BarStaticItemCopyRight
            // 
            this.BarStaticItemCopyRight.AutoSize = DevExpress.XtraBars.BarStaticItemSize.Spring;
            this.BarStaticItemCopyRight.Id = 1;
            this.BarStaticItemCopyRight.Name = "BarStaticItemCopyRight";
            this.BarStaticItemCopyRight.TextAlignment = System.Drawing.StringAlignment.Center;
            this.BarStaticItemCopyRight.Width = 32;
            // 
            // BarStaticItemDateTime
            // 
            this.BarStaticItemDateTime.Id = 2;
            this.BarStaticItemDateTime.Name = "BarStaticItemDateTime";
            this.BarStaticItemDateTime.TextAlignment = System.Drawing.StringAlignment.Center;
            // 
            // BarMainMenu
            // 
            this.BarMainMenu.BarAppearance.Disabled.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.BarMainMenu.BarAppearance.Disabled.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BarMainMenu.BarAppearance.Disabled.Options.UseBackColor = true;
            this.BarMainMenu.BarAppearance.Disabled.Options.UseFont = true;
            this.BarMainMenu.BarAppearance.Hovered.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.BarMainMenu.BarAppearance.Hovered.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.BarMainMenu.BarAppearance.Hovered.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold);
            this.BarMainMenu.BarAppearance.Hovered.Options.UseBackColor = true;
            this.BarMainMenu.BarAppearance.Hovered.Options.UseFont = true;
            this.BarMainMenu.BarAppearance.Normal.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold);
            this.BarMainMenu.BarAppearance.Normal.Options.UseFont = true;
            this.BarMainMenu.BarAppearance.Pressed.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.BarMainMenu.BarAppearance.Pressed.BackColor2 = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.BarMainMenu.BarAppearance.Pressed.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold);
            this.BarMainMenu.BarAppearance.Pressed.Options.UseBackColor = true;
            this.BarMainMenu.BarAppearance.Pressed.Options.UseFont = true;
            this.BarMainMenu.BarName = "MAIN MENU";
            this.BarMainMenu.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Top;
            this.BarMainMenu.DockCol = 0;
            this.BarMainMenu.DockRow = 0;
            this.BarMainMenu.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
            this.BarMainMenu.OptionsBar.AllowQuickCustomization = false;
            this.BarMainMenu.OptionsBar.DrawDragBorder = false;
            this.BarMainMenu.OptionsBar.MultiLine = true;
            this.BarMainMenu.OptionsBar.UseWholeRow = true;
            this.BarMainMenu.Text = "MAIN MENU";
            // 
            // barDockControlTop
            // 
            this.barDockControlTop.CausesValidation = false;
            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
            this.barDockControlTop.Size = new System.Drawing.Size(1015, 25);
            // 
            // barDockControlBottom
            // 
            this.barDockControlBottom.CausesValidation = false;
            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDockControlBottom.Location = new System.Drawing.Point(0, 605);
            this.barDockControlBottom.Size = new System.Drawing.Size(1015, 35);
            // 
            // barDockControlLeft
            // 
            this.barDockControlLeft.CausesValidation = false;
            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
            this.barDockControlLeft.Location = new System.Drawing.Point(0, 25);
            this.barDockControlLeft.Size = new System.Drawing.Size(0, 580);
            // 
            // barDockControlRight
            // 
            this.barDockControlRight.CausesValidation = false;
            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
            this.barDockControlRight.Location = new System.Drawing.Point(1015, 25);
            this.barDockControlRight.Size = new System.Drawing.Size(0, 580);
            // 
            // AMFC_Main_Login_SplitContainer
            // 
            this.AMFC_Main_Login_SplitContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AMFC_Main_Login_SplitContainer.IsSplitterFixed = true;
            this.AMFC_Main_Login_SplitContainer.Location = new System.Drawing.Point(0, 25);
            this.AMFC_Main_Login_SplitContainer.Name = "AMFC_Main_Login_SplitContainer";
            this.AMFC_Main_Login_SplitContainer.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // AMFC_Main_Login_SplitContainer.Panel1
            // 
            this.AMFC_Main_Login_SplitContainer.Panel1.Controls.Add(this.AMFC_PictureBox_Logo);
            this.AMFC_Main_Login_SplitContainer.Size = new System.Drawing.Size(1015, 580);
            this.AMFC_Main_Login_SplitContainer.SplitterDistance = 266;
            this.AMFC_Main_Login_SplitContainer.TabIndex = 11;
            // 
            // AMFC_PictureBox_Logo
            // 
            this.AMFC_PictureBox_Logo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.AMFC_PictureBox_Logo.ErrorImage = null;
            this.AMFC_PictureBox_Logo.Image = global::GesAMFC.Properties.Resources.AMFC_Logo_01;
            this.AMFC_PictureBox_Logo.InitialImage = null;
            this.AMFC_PictureBox_Logo.Location = new System.Drawing.Point(0, 0);
            this.AMFC_PictureBox_Logo.MinimumSize = new System.Drawing.Size(256, 256);
            this.AMFC_PictureBox_Logo.Name = "AMFC_PictureBox_Logo";
            this.AMFC_PictureBox_Logo.Size = new System.Drawing.Size(1015, 266);
            this.AMFC_PictureBox_Logo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.AMFC_PictureBox_Logo.TabIndex = 6;
            this.AMFC_PictureBox_Logo.TabStop = false;
            // 
            // GesAMFC_MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1015, 640);
            this.Controls.Add(this.AMFC_Main_Login_SplitContainer);
            this.Controls.Add(this.barDockControlLeft);
            this.Controls.Add(this.barDockControlRight);
            this.Controls.Add(this.barDockControlBottom);
            this.Controls.Add(this.barDockControlTop);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.IsMdiContainer = true;
            this.LookAndFeel.SkinName = "Office 2010 Blue";
            this.Name = "GesAMFC_MainForm";
            this.Text = "Gestão da Associação Moradores Foros Catrapona";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GesAMFC_MainForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.GesAMFC_MainForm_FormClosed);
            this.Load += new System.EventHandler(this.AdexEntitiesMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.barManagerAdexEntities)).EndInit();
            this.AMFC_Main_Login_SplitContainer.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.AMFC_Main_Login_SplitContainer)).EndInit();
            this.AMFC_Main_Login_SplitContainer.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.AMFC_PictureBox_Logo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.LookAndFeel.DefaultLookAndFeel defaultLookAndFeel1;
        private System.Windows.Forms.ImageList imageMenuList;
        private DevExpress.XtraBars.BarManager barManagerAdexEntities;
        private DevExpress.XtraBars.Bar BarStatus;
        private DevExpress.XtraBars.BarDockControl barDockControlTop;
        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
        private DevExpress.XtraBars.BarDockControl barDockControlRight;
        private DevExpress.XtraBars.BarStaticItem BarStaticItemCopyRight;
        private DevExpress.XtraBars.BarStaticItem BarStaticItemDateTime;
        private DevExpress.XtraBars.Bar BarMainMenu;
        private System.Windows.Forms.SplitContainer AMFC_Main_Login_SplitContainer;
        private System.Windows.Forms.PictureBox AMFC_PictureBox_Logo;
    }
}

