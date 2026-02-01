namespace GameCaro
{
    partial class GameCaro
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GameCaro));
            this.panel4 = new Sunny.UI.UIPanel();
            this.pctbMark = new System.Windows.Forms.PictureBox();
            this.btnLAN = new Sunny.UI.UIButton();
            this.btnChooseAvatar = new Sunny.UI.UIButton();
            this.txbIP = new Sunny.UI.UITextBox();
            this.txbPlayerName = new Sunny.UI.UITextBox();
            this.lblGameMode = new Sunny.UI.UILabel();
            this.cboGameMode = new Sunny.UI.UIComboBox();
            this.btnUndo = new Sunny.UI.UIButton();
            this.btnRedo = new Sunny.UI.UIButton();
            this.prcbCoolDown = new Sunny.UI.UIProcessBar();
            this.lblCountX = new Sunny.UI.UILabel();
            this.lblCountO = new Sunny.UI.UILabel();
            this.pnlChessBoard = new Sunny.UI.UIPanel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.tmCoolDown = new System.Windows.Forms.Timer(this.components);
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.menuToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newGameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearLocalInfoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.clearLanInfoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.quitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.howToPlayToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.changeNameAvatarToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.sendMessageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.panel1 = new Sunny.UI.UIPanel();
            this.txbMessage = new Sunny.UI.UITextBox();
            this.btnSend = new Sunny.UI.UIButton();
            this.panel2 = new Sunny.UI.UIPanel();
            this.txbLog = new Sunny.UI.UIRichTextBox();
            this.panel4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pctbMark)).BeginInit();
            this.pnlChessBoard.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // panel4
            // 
            this.panel4.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.panel4.Controls.Add(this.pctbMark);
            this.panel4.Controls.Add(this.btnLAN);
            this.panel4.Controls.Add(this.btnChooseAvatar);
            this.panel4.Controls.Add(this.txbIP);
            this.panel4.Controls.Add(this.txbPlayerName);
            this.panel4.Controls.Add(this.lblGameMode);
            this.panel4.Controls.Add(this.cboGameMode);
            this.panel4.Controls.Add(this.btnUndo);
            this.panel4.Controls.Add(this.btnRedo);
            this.panel4.FillColor = System.Drawing.Color.White;
            this.panel4.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.panel4.Location = new System.Drawing.Point(922, 495);
            this.panel4.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel4.MinimumSize = new System.Drawing.Size(1, 1);
            this.panel4.Name = "panel4";
            this.panel4.Radius = 15;
            this.panel4.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.panel4.Size = new System.Drawing.Size(331, 175);
            this.panel4.TabIndex = 3;
            this.panel4.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pctbMark
            // 
            this.pctbMark.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pctbMark.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.pctbMark.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pctbMark.Location = new System.Drawing.Point(211, 14);
            this.pctbMark.Name = "pctbMark";
            this.pctbMark.Size = new System.Drawing.Size(105, 105);
            this.pctbMark.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pctbMark.TabIndex = 3;
            this.pctbMark.TabStop = false;
            // 
            // btnLAN
            // 
            this.btnLAN.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnLAN.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.btnLAN.FillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(125)))), ((int)(((byte)(255)))));
            this.btnLAN.Font = new System.Drawing.Font("Segoe UI Semibold", 10F);
            this.btnLAN.Location = new System.Drawing.Point(15, 130);
            this.btnLAN.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnLAN.Name = "btnLAN";
            this.btnLAN.Radius = 10;
            this.btnLAN.Size = new System.Drawing.Size(85, 35);
            this.btnLAN.TabIndex = 2;
            this.btnLAN.Text = "LAN";
            this.btnLAN.TipsFont = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.btnLAN.Click += new System.EventHandler(this.btnLAN_Click);
            // 
            // btnChooseAvatar
            // 
            this.btnChooseAvatar.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnChooseAvatar.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(72)))), ((int)(((byte)(187)))), ((int)(((byte)(120)))));
            this.btnChooseAvatar.FillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(56)))), ((int)(((byte)(161)))), ((int)(((byte)(100)))));
            this.btnChooseAvatar.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.btnChooseAvatar.Location = new System.Drawing.Point(105, 130);
            this.btnChooseAvatar.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnChooseAvatar.Name = "btnChooseAvatar";
            this.btnChooseAvatar.Radius = 10;
            this.btnChooseAvatar.Size = new System.Drawing.Size(90, 35);
            this.btnChooseAvatar.TabIndex = 9;
            this.btnChooseAvatar.Text = "Avatar";
            this.btnChooseAvatar.TipsFont = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.btnChooseAvatar.Click += new System.EventHandler(this.btnChooseAvatar_Click);
            // 
            // txbIP
            // 
            this.txbIP.ButtonFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txbIP.ButtonStyleInherited = false;
            this.txbIP.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txbIP.DoubleValue = 127001D;
            this.txbIP.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txbIP.Location = new System.Drawing.Point(15, 95);
            this.txbIP.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txbIP.MinimumSize = new System.Drawing.Size(1, 16);
            this.txbIP.Name = "txbIP";
            this.txbIP.Padding = new System.Windows.Forms.Padding(5);
            this.txbIP.Radius = 8;
            this.txbIP.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.txbIP.ShowText = false;
            this.txbIP.Size = new System.Drawing.Size(175, 30);
            this.txbIP.TabIndex = 1;
            this.txbIP.Text = "127.0.0.1";
            this.txbIP.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txbIP.Watermark = "Địa chỉ IP...";
            // 
            // txbPlayerName
            // 
            this.txbPlayerName.ButtonFillColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.txbPlayerName.ButtonStyleInherited = false;
            this.txbPlayerName.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txbPlayerName.Font = new System.Drawing.Font("Segoe UI", 10F);
            this.txbPlayerName.Location = new System.Drawing.Point(15, 10);
            this.txbPlayerName.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txbPlayerName.MinimumSize = new System.Drawing.Size(1, 16);
            this.txbPlayerName.Name = "txbPlayerName";
            this.txbPlayerName.Padding = new System.Windows.Forms.Padding(5);
            this.txbPlayerName.Radius = 8;
            this.txbPlayerName.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.txbPlayerName.ShowText = false;
            this.txbPlayerName.Size = new System.Drawing.Size(175, 30);
            this.txbPlayerName.TabIndex = 0;
            this.txbPlayerName.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txbPlayerName.Watermark = "Tên người chơi...";
            this.txbPlayerName.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txbPlayerName_KeyDown);
            // 
            // lblGameMode
            // 
            this.lblGameMode.AutoSize = true;
            this.lblGameMode.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.lblGameMode.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.lblGameMode.Location = new System.Drawing.Point(14, 43);
            this.lblGameMode.Name = "lblGameMode";
            this.lblGameMode.Size = new System.Drawing.Size(74, 15);
            this.lblGameMode.TabIndex = 10;
            this.lblGameMode.Text = "Chế độ chơi:";
            // 
            // cboGameMode
            // 
            this.cboGameMode.DataSource = null;
            this.cboGameMode.DropDownStyle = Sunny.UI.UIDropDownStyle.DropDownList;
            this.cboGameMode.FillColor = System.Drawing.Color.White;
            this.cboGameMode.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.cboGameMode.ItemHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(155)))), ((int)(((byte)(200)))), ((int)(((byte)(255)))));
            this.cboGameMode.Items.AddRange(new object[] {
            "Chơi với máy",
            "2 người/máy",
            "Chơi qua LAN"});
            this.cboGameMode.ItemSelectForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(243)))), ((int)(((byte)(255)))));
            this.cboGameMode.Location = new System.Drawing.Point(15, 58);
            this.cboGameMode.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.cboGameMode.MinimumSize = new System.Drawing.Size(63, 0);
            this.cboGameMode.Name = "cboGameMode";
            this.cboGameMode.Padding = new System.Windows.Forms.Padding(0, 0, 30, 2);
            this.cboGameMode.Radius = 8;
            this.cboGameMode.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.cboGameMode.Size = new System.Drawing.Size(175, 32);
            this.cboGameMode.SymbolSize = 24;
            this.cboGameMode.TabIndex = 11;
            this.cboGameMode.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.cboGameMode.Watermark = "";
            this.cboGameMode.SelectedIndexChanged += new System.EventHandler(this.cboGameMode_SelectedIndexChanged);
            // 
            // btnUndo
            // 
            this.btnUndo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnUndo.FillColor = System.Drawing.Color.White;
            this.btnUndo.FillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.btnUndo.Font = new System.Drawing.Font("Segoe UI Semibold", 12F);
            this.btnUndo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.btnUndo.Location = new System.Drawing.Point(207, 130);
            this.btnUndo.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnUndo.Name = "btnUndo";
            this.btnUndo.Radius = 8;
            this.btnUndo.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.btnUndo.Size = new System.Drawing.Size(55, 35);
            this.btnUndo.TabIndex = 12;
            this.btnUndo.Text = "↩";
            this.btnUndo.TipsFont = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.btnUndo.Click += new System.EventHandler(this.btnUndo_Click);
            // 
            // btnRedo
            // 
            this.btnRedo.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRedo.FillColor = System.Drawing.Color.White;
            this.btnRedo.FillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(235)))), ((int)(((byte)(245)))), ((int)(((byte)(255)))));
            this.btnRedo.Font = new System.Drawing.Font("Segoe UI Semibold", 12F);
            this.btnRedo.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.btnRedo.Location = new System.Drawing.Point(265, 130);
            this.btnRedo.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnRedo.Name = "btnRedo";
            this.btnRedo.Radius = 8;
            this.btnRedo.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.btnRedo.Size = new System.Drawing.Size(55, 35);
            this.btnRedo.TabIndex = 13;
            this.btnRedo.Text = "↪";
            this.btnRedo.TipsFont = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.btnRedo.Click += new System.EventHandler(this.btnRedo_Click);
            // 
            // prcbCoolDown
            // 
            this.prcbCoolDown.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(240)))), ((int)(((byte)(240)))), ((int)(((byte)(240)))));
            this.prcbCoolDown.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.prcbCoolDown.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.prcbCoolDown.Location = new System.Drawing.Point(525, 20);
            this.prcbCoolDown.MinimumSize = new System.Drawing.Size(3, 3);
            this.prcbCoolDown.Name = "prcbCoolDown";
            this.prcbCoolDown.Size = new System.Drawing.Size(144, 23);
            this.prcbCoolDown.TabIndex = 4;
            // 
            // lblCountX
            // 
            this.lblCountX.AutoSize = true;
            this.lblCountX.Font = new System.Drawing.Font("Courier New", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCountX.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(107)))), ((int)(((byte)(107)))));
            this.lblCountX.Location = new System.Drawing.Point(450, 20);
            this.lblCountX.Name = "lblCountX";
            this.lblCountX.Size = new System.Drawing.Size(43, 22);
            this.lblCountX.TabIndex = 7;
            this.lblCountX.Text = "X:0";
            // 
            // lblCountO
            // 
            this.lblCountO.AutoSize = true;
            this.lblCountO.Font = new System.Drawing.Font("Courier New", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblCountO.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(99)))), ((int)(((byte)(179)))), ((int)(((byte)(237)))));
            this.lblCountO.Location = new System.Drawing.Point(386, 20);
            this.lblCountO.Name = "lblCountO";
            this.lblCountO.Size = new System.Drawing.Size(43, 22);
            this.lblCountO.TabIndex = 6;
            this.lblCountO.Text = "O:0";
            // 
            // pnlChessBoard
            // 
            this.pnlChessBoard.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlChessBoard.Controls.Add(this.flowLayoutPanel1);
            this.pnlChessBoard.FillColor = System.Drawing.Color.White;
            this.pnlChessBoard.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.pnlChessBoard.Location = new System.Drawing.Point(15, 49);
            this.pnlChessBoard.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.pnlChessBoard.MinimumSize = new System.Drawing.Size(1, 1);
            this.pnlChessBoard.Name = "pnlChessBoard";
            this.pnlChessBoard.Padding = new System.Windows.Forms.Padding(5);
            this.pnlChessBoard.Radius = 15;
            this.pnlChessBoard.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.pnlChessBoard.Size = new System.Drawing.Size(901, 620);
            this.pnlChessBoard.TabIndex = 8;
            this.pnlChessBoard.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Location = new System.Drawing.Point(900, 98);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(200, 100);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // tmCoolDown
            // 
            this.tmCoolDown.Tick += new System.EventHandler(this.tmCoolDown_Tick);
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.White;
            this.menuStrip1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menuToolStripMenuItem,
            this.helpToolStripMenuItem,
            this.aboutToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1264, 24);
            this.menuStrip1.TabIndex = 9;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // menuToolStripMenuItem
            // 
            this.menuToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newGameToolStripMenuItem,
            this.clearLocalInfoToolStripMenuItem,
            this.clearLanInfoToolStripMenuItem,
            this.toolStripSeparator1,
            this.quitToolStripMenuItem});
            this.menuToolStripMenuItem.Name = "menuToolStripMenuItem";
            this.menuToolStripMenuItem.Size = new System.Drawing.Size(50, 20);
            this.menuToolStripMenuItem.Text = "Menu";
            // 
            // newGameToolStripMenuItem
            // 
            this.newGameToolStripMenuItem.Name = "newGameToolStripMenuItem";
            this.newGameToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.N)));
            this.newGameToolStripMenuItem.Size = new System.Drawing.Size(225, 22);
            this.newGameToolStripMenuItem.Text = "New game";
            this.newGameToolStripMenuItem.Click += new System.EventHandler(this.newGameToolStripMenuItem_Click);
            // 
            // clearLocalInfoToolStripMenuItem
            // 
            this.clearLocalInfoToolStripMenuItem.Name = "clearLocalInfoToolStripMenuItem";
            this.clearLocalInfoToolStripMenuItem.Size = new System.Drawing.Size(225, 22);
            this.clearLocalInfoToolStripMenuItem.Text = "Xóa thông tin (2 người/máy)";
            this.clearLocalInfoToolStripMenuItem.Click += new System.EventHandler(this.clearLocalInfoToolStripMenuItem_Click);
            // 
            // clearLanInfoToolStripMenuItem
            // 
            this.clearLanInfoToolStripMenuItem.Name = "clearLanInfoToolStripMenuItem";
            this.clearLanInfoToolStripMenuItem.Size = new System.Drawing.Size(225, 22);
            this.clearLanInfoToolStripMenuItem.Text = "Xóa thông tin (LAN)";
            this.clearLanInfoToolStripMenuItem.Click += new System.EventHandler(this.clearLanInfoToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(222, 6);
            // 
            // quitToolStripMenuItem
            // 
            this.quitToolStripMenuItem.Name = "quitToolStripMenuItem";
            this.quitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.quitToolStripMenuItem.Size = new System.Drawing.Size(225, 22);
            this.quitToolStripMenuItem.Text = "Quit";
            this.quitToolStripMenuItem.Click += new System.EventHandler(this.quitToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.howToPlayToolStripMenuItem,
            this.changeNameAvatarToolStripMenuItem,
            this.sendMessageToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(79, 20);
            this.helpToolStripMenuItem.Text = "Hướng dẫn";
            // 
            // howToPlayToolStripMenuItem
            // 
            this.howToPlayToolStripMenuItem.Name = "howToPlayToolStripMenuItem";
            this.howToPlayToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.howToPlayToolStripMenuItem.Text = "Hướng dẫn chơi";
            this.howToPlayToolStripMenuItem.Click += new System.EventHandler(this.howToPlayToolStripMenuItem_Click);
            // 
            // changeNameAvatarToolStripMenuItem
            // 
            this.changeNameAvatarToolStripMenuItem.Name = "changeNameAvatarToolStripMenuItem";
            this.changeNameAvatarToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.changeNameAvatarToolStripMenuItem.Text = "Thay đổi tên && avatar";
            this.changeNameAvatarToolStripMenuItem.Click += new System.EventHandler(this.changeNameAvatarToolStripMenuItem_Click);
            // 
            // sendMessageToolStripMenuItem
            // 
            this.sendMessageToolStripMenuItem.Name = "sendMessageToolStripMenuItem";
            this.sendMessageToolStripMenuItem.Size = new System.Drawing.Size(188, 22);
            this.sendMessageToolStripMenuItem.Text = "Gửi tin nhắn (LAN)";
            this.sendMessageToolStripMenuItem.Click += new System.EventHandler(this.sendMessageToolStripMenuItem_Click);
            // 
            // aboutToolStripMenuItem
            // 
            this.aboutToolStripMenuItem.Name = "aboutToolStripMenuItem";
            this.aboutToolStripMenuItem.Size = new System.Drawing.Size(70, 20);
            this.aboutToolStripMenuItem.Text = "Giới thiệu";
            this.aboutToolStripMenuItem.Click += new System.EventHandler(this.aboutToolStripMenuItem_Click);
            // 
            // panel1
            // 
            this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.panel1.BackgroundImage = global::GameCaro.Properties.Resources.banner;
            this.panel1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.panel1.FillColor = System.Drawing.Color.Transparent;
            this.panel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.panel1.Location = new System.Drawing.Point(921, 49);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel1.MinimumSize = new System.Drawing.Size(1, 1);
            this.panel1.Name = "panel1";
            this.panel1.Radius = 15;
            this.panel1.RectColor = System.Drawing.Color.Transparent;
            this.panel1.Size = new System.Drawing.Size(332, 92);
            this.panel1.TabIndex = 1;
            this.panel1.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txbMessage
            // 
            this.txbMessage.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txbMessage.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.txbMessage.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txbMessage.Location = new System.Drawing.Point(12, 278);
            this.txbMessage.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txbMessage.MinimumSize = new System.Drawing.Size(1, 16);
            this.txbMessage.Name = "txbMessage";
            this.txbMessage.Padding = new System.Windows.Forms.Padding(5);
            this.txbMessage.Radius = 8;
            this.txbMessage.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(200)))), ((int)(((byte)(200)))), ((int)(((byte)(200)))));
            this.txbMessage.ShowText = false;
            this.txbMessage.Size = new System.Drawing.Size(230, 30);
            this.txbMessage.TabIndex = 11;
            this.txbMessage.TextAlignment = System.Drawing.ContentAlignment.MiddleLeft;
            this.txbMessage.Watermark = "Nhập tin nhắn...";
            this.txbMessage.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txbMessage_KeyDown);
            // 
            // btnSend
            // 
            this.btnSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSend.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSend.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.btnSend.FillHoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(65)))), ((int)(((byte)(125)))), ((int)(((byte)(255)))));
            this.btnSend.Font = new System.Drawing.Font("Segoe UI Semibold", 9F);
            this.btnSend.Location = new System.Drawing.Point(248, 277);
            this.btnSend.MinimumSize = new System.Drawing.Size(1, 1);
            this.btnSend.Name = "btnSend";
            this.btnSend.Radius = 8;
            this.btnSend.Size = new System.Drawing.Size(70, 32);
            this.btnSend.TabIndex = 12;
            this.btnSend.Text = "Gửi";
            this.btnSend.TipsFont = new System.Drawing.Font("Microsoft Sans Serif", 9F);
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // panel2
            // 
            this.panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.panel2.Controls.Add(this.btnSend);
            this.panel2.Controls.Add(this.txbMessage);
            this.panel2.Controls.Add(this.txbLog);
            this.panel2.FillColor = System.Drawing.Color.White;
            this.panel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.panel2.Location = new System.Drawing.Point(921, 150);
            this.panel2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.panel2.MinimumSize = new System.Drawing.Size(1, 1);
            this.panel2.Name = "panel2";
            this.panel2.Padding = new System.Windows.Forms.Padding(10);
            this.panel2.Radius = 15;
            this.panel2.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.panel2.Size = new System.Drawing.Size(331, 335);
            this.panel2.TabIndex = 13;
            this.panel2.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txbLog
            // 
            this.txbLog.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(250)))), ((int)(((byte)(250)))), ((int)(((byte)(252)))));
            this.txbLog.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.txbLog.Location = new System.Drawing.Point(12, 8);
            this.txbLog.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.txbLog.MinimumSize = new System.Drawing.Size(1, 1);
            this.txbLog.Name = "txbLog";
            this.txbLog.Padding = new System.Windows.Forms.Padding(2);
            this.txbLog.Radius = 10;
            this.txbLog.ReadOnly = true;
            this.txbLog.RectColor = System.Drawing.Color.FromArgb(((int)(((byte)(220)))), ((int)(((byte)(220)))), ((int)(((byte)(220)))));
            this.txbLog.ShowText = false;
            this.txbLog.Size = new System.Drawing.Size(306, 260);
            this.txbLog.TabIndex = 10;
            this.txbLog.TextAlignment = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // GameCaro
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(1264, 681);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.prcbCoolDown);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.lblCountX);
            this.Controls.Add(this.lblCountO);
            this.Controls.Add(this.pnlChessBoard);
            this.Controls.Add(this.menuStrip1);
            this.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.MinimumSize = new System.Drawing.Size(799, 500);
            this.Name = "GameCaro";
            this.Text = "CARO";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pctbMark)).EndInit();
            this.pnlChessBoard.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        
        // Modern SunnyUI Controls
        private Sunny.UI.UIPanel panel1;
        private Sunny.UI.UIPanel panel4;
        private Sunny.UI.UIButton btnLAN;
        private Sunny.UI.UITextBox txbIP;
        private Sunny.UI.UITextBox txbPlayerName;
        private Sunny.UI.UIProcessBar prcbCoolDown;
        private Sunny.UI.UILabel lblCountO;
        private Sunny.UI.UILabel lblCountX;
        private Sunny.UI.UIPanel pnlChessBoard;
        private System.Windows.Forms.PictureBox pctbMark;
        private System.Windows.Forms.Timer tmCoolDown;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem menuToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newGameToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem quitToolStripMenuItem;
        private Sunny.UI.UIButton btnChooseAvatar;
        private Sunny.UI.UILabel lblGameMode;
        private Sunny.UI.UIComboBox cboGameMode;
        private Sunny.UI.UIButton btnUndo;
        private Sunny.UI.UIButton btnRedo;
        private Sunny.UI.UITextBox txbMessage;
        private Sunny.UI.UIButton btnSend;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private Sunny.UI.UIPanel panel2;
        private Sunny.UI.UIRichTextBox txbLog;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem howToPlayToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem changeNameAvatarToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem sendMessageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearLocalInfoToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem clearLanInfoToolStripMenuItem;
    }
}


