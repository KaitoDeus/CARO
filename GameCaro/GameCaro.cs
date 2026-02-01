using System;
using System.Drawing;
using System.Net.NetworkInformation;
using System.Threading;
using System.Windows.Forms;
using System.Threading.Tasks;


namespace GameCaro
{
    public partial class GameCaro : Form
    {
        #region Properties
        ChessBoardManager ChessBoard;
        SocketManager socket;
        PlayerSettings playerSettings;
        LanPlayerSettings lanPlayerSettings;
        
        private GameMode currentGameMode = GameMode.LocalMultiplayer;

        private string myChatName = "";
        private AIPlayer aiPlayer;
        #endregion

        public GameCaro()
        {
            try
            {
                InitializeComponent();

                Control.CheckForIllegalCrossThreadCalls = false;

                ChessBoard = new ChessBoardManager(pnlChessBoard, txbPlayerName, pctbMark, lblCountO, lblCountX);
                ChessBoard.EndedGame += ChessBoard_EndedGame;
                ChessBoard.PlayerMarked += ChessBoard_PlayerMarked;

                prcbCoolDown.Step = Cons.COOL_DOWN_STEP;
                prcbCoolDown.Maximum = Cons.COOL_DOWN_TIME;
                prcbCoolDown.Value = 0;

                tmCoolDown.Interval = Cons.COOL_DOWN_INTERVAL;

                socket = new SocketManager();
                socket.ClientConnected += Socket_ClientConnected;

                aiPlayer = new AIPlayer(ChessBoard);

                LoadPlayerSettings();

                cboGameMode.SelectedIndex = 0;
                SetChatEnabled(false);



                NewGame();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khởi động game: " + ex.Message + "\n" + ex.StackTrace, "Lỗi Nghiêm Trọng", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }



        #region Methods
        private void SetChessBoardEnabled(bool enabled)
        {
            pnlChessBoard.Enabled = enabled;
        }

        void UpdatePlayerUI()
        {
            ChessBoard.PlayerName.Text = ChessBoard.Player[ChessBoard.CurrentPlayer].Name;
            ChessBoard.PlayerMark.Image = ChessBoard.Player[ChessBoard.CurrentPlayer].Avatar;
        }

        void EndGame()
        {
            tmCoolDown.Stop();
            SetChessBoardEnabled(false);
        }

        void NewGame()
        {
            prcbCoolDown.Value = 0;
            tmCoolDown.Stop();
            lblCountO.Text = "O:0";
            lblCountX.Text = "X:0";

            ChessBoard.DrawChessBoard();
            UpdateMenuForGameMode();
        }

        void Quit()
        {
            Application.Exit();
        }

        void Undo()
        {
            if (currentGameMode == GameMode.LAN) return;
            
            ChessBoard.Undo();
            
            // Trong chế độ PvC, nếu Undo xong mà đến lượt máy (Player 1)
            // thì Undo thêm lần nữa để về lượt người chơi
            if (currentGameMode == GameMode.PvC && ChessBoard.CurrentPlayer == 1)
            {
                ChessBoard.Undo();
            }

            prcbCoolDown.Value = 0;
        }

        void Redo()
        {
            if (currentGameMode == GameMode.LAN) return;
            ChessBoard.Redo();
            prcbCoolDown.Value = 0;
        }

        void UpdateMenuForGameMode()
        {
            bool allowUndoRedo = currentGameMode != GameMode.LAN;
            bool canUndo = allowUndoRedo && ChessBoard.PlayTimeLine.Count > 0;
            bool canRedo = allowUndoRedo && ChessBoard.RedoTimeLine.Count > 0;
            
            btnUndo.Enabled = canUndo;
            btnRedo.Enabled = canRedo;
        }

        private void ChessBoard_PlayerMarked(object sender, ButtonClickEvent e)
        {
            UpdateMenuForGameMode();
            
            tmCoolDown.Stop();
            prcbCoolDown.Value = 0;
            tmCoolDown.Start();
            
            if (currentGameMode == GameMode.LAN)
            {
                tmCoolDown.Stop();
                prcbCoolDown.Value = 0;
                
                SetChessBoardEnabled(false);

                socket.Send(new SocketData((int)SocketCommand.SEND_POINT, "", e.ClickedPoint));

                Listen();
            }
            
            if (currentGameMode == GameMode.PvC && ChessBoard.CurrentPlayer == 1)
            {
                StartAI();
            }
        }

        private void ChessBoard_EndedGame(object sender, EventArgs e)
        {
            EndGame();
            
            int winner = ChessBoard.CurrentPlayer == 0 ? 1 : 0;
            string winnerName = ChessBoard.Player[winner].Name;
            string winnerMark = winner == 0 ? "O" : "X";
            
            MessageBox.Show(string.Format("Người chơi {0} ({1}) đã thắng!", winnerName, winnerMark), "Kết thúc", MessageBoxButtons.OK, MessageBoxIcon.Information);
            
            if (currentGameMode == GameMode.LAN)
            {
                socket.Send(new SocketData((int)SocketCommand.END_GAME, "", new Point()));
            }
        }

        private void tmCoolDown_Tick(object sender, EventArgs e)
        {
            prcbCoolDown.PerformStep();

            if(prcbCoolDown.Value >= prcbCoolDown.Maximum)
            {
                EndGame();
                
                int loser = ChessBoard.CurrentPlayer;
                int winner = loser == 0 ? 1 : 0;
                string winnerName = ChessBoard.Player[winner].Name;
                string loserName = ChessBoard.Player[loser].Name;
                
                MessageBox.Show(string.Format("Hết giờ! {0} không đánh tiếp.\n{1} thắng!", loserName, winnerName), 
                    "Hết giờ", MessageBoxButtons.OK, MessageBoxIcon.Information);
                
                socket.Send(new SocketData((int)SocketCommand.TIME_OUT, "", new Point()));
            }
        }

        private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewGame();
            socket.Send(new SocketData((int)SocketCommand.NEW_GAME, "", new Point()));
            SetChessBoardEnabled(true);
        }



        private void quitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Quit();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc muốn thoát?", "Thông báo", MessageBoxButtons.OKCancel) != System.Windows.Forms.DialogResult.OK)
            {
                e.Cancel = true;
            }
            else
            {
                try
                {
                    socket.Send(new SocketData((int)SocketCommand.QUIT, "", new Point()));
                }
                catch
                { }
            }
        }

        private void clearLocalInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc muốn xóa thông tin người chơi chế độ 2 người/máy về mặc định?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                // Reset Local settings
                playerSettings = new PlayerSettings(); // Reset về mặc định
                playerSettings.Save();

                if (currentGameMode != GameMode.LAN)
                {
                    LoadPlayerSettings();
                    UpdatePlayerUI();
                    // Cập nhật tên hiển thị trên Textbox
                    txbPlayerName.Text = ChessBoard.Player[ChessBoard.CurrentPlayer].Name;
                    // Cập nhật hình ảnh đại diện
                    pctbMark.Image = ChessBoard.Player[ChessBoard.CurrentPlayer].Avatar;
                }

                MessageBox.Show("Đã xóa thông tin chế độ 2 người/máy!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void clearLanInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Bạn có chắc muốn xóa thông tin người chơi chế độ LAN về mặc định?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                // Reset LAN settings
                lanPlayerSettings = new LanPlayerSettings(); // Reset về mặc định
                lanPlayerSettings.Save();

                if (currentGameMode == GameMode.LAN)
                {
                    // Cập nhật UI
                    ChessBoard.Player[0].Name = lanPlayerSettings.PlayerName;
                    
                    LoadLanSettings();
                    UpdatePlayerUI();
                    
                    // Cập nhật tên hiển thị trên Textbox
                    txbPlayerName.Text = ChessBoard.Player[ChessBoard.CurrentPlayer].Name;

                    // Cập nhật hình ảnh đại diện (góc trên bên phải)
                    pctbMark.Image = ChessBoard.Player[0].Avatar;
                }

                MessageBox.Show("Đã xóa thông tin chế độ LAN!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void btnLAN_Click(object sender, EventArgs e)
        {
            btnLAN.Enabled = false;
            txbIP.Enabled = false;

            socket.IP = txbIP.Text;
            
            // Lưu tên LAN hiện tại
            lanPlayerSettings.PlayerName = txbPlayerName.Text.Trim();
            lanPlayerSettings.Save();

            myChatName = lanPlayerSettings.PlayerName;
            if (string.IsNullOrEmpty(myChatName))
            {
                myChatName = "LAN Player";
            }

            if (!socket.ConnectServer())
            {
                socket.isServer = true;
                
                // Server là Player 1 (Index 0)
                ChessBoard.Player[0].Name = lanPlayerSettings.PlayerName;
                // Avatar đã được set trong LoadLanSettings hoặc khi chọn ảnh
                
                // Set tên mặc định cho đối thủ
                ChessBoard.Player[1].Name = "Player X (Client)";
                // Reset avatar đối thủ về mặc định
                ChessBoard.Player[1].Avatar = Image.FromFile(Application.StartupPath + "\\Resources\\P2.png");

                SetChessBoardEnabled(true);
                socket.CreateServer();
                btnLAN.Text = "Chờ kết nối...";
            }
            else
            {
                socket.isServer = false;

                // Client là Player 2 (Index 1)
                ChessBoard.Player[1].Name = lanPlayerSettings.PlayerName;
                
                // Load avatar cho Player 2
                Image avatar = PlayerSettings.LoadAvatarImage(lanPlayerSettings.AvatarPath);
                if (avatar != null)
                {
                    ChessBoard.Player[1].Avatar = avatar;
                }

                // Set tên mặc định cho đối thủ (Server)
                ChessBoard.Player[0].Name = "Player O (Server)";
                // Reset avatar đối thủ về mặc định (tránh hiển thị avatar của mình do LoadLanSettings)
                ChessBoard.Player[0].Avatar = Image.FromFile(Application.StartupPath + "\\Resources\\P1.png");

                SetChessBoardEnabled(false);
                Listen();
                btnLAN.Text = "Đã kết nối";
            }

            txbPlayerName.ReadOnly = true;
            btnChooseAvatar.Enabled = false;
        }

        private void Socket_ClientConnected(object sender, EventArgs e)
        {
            this.Invoke((MethodInvoker)(() =>
            {
                btnLAN.Text = "Đã kết nối";
                Listen();
            }));
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            txbIP.Text = socket.GetLocalIPv4(NetworkInterfaceType.Wireless80211);

            if (string.IsNullOrEmpty(txbIP.Text))
            {
                txbIP.Text = socket.GetLocalIPv4(NetworkInterfaceType.Ethernet);
            }
        }

        void Listen()
        {
            Thread listenThread = new Thread(() =>
            {
                try
                {
                    SocketData data = (SocketData)socket.Receive();

                    if (data != null)
                    {
                        ProcessData(data);
                    }
                }
                catch 
                { 
                
                }
            });
            listenThread.IsBackground = true;
            listenThread.Start();
        }

        private void ProcessData(SocketData data)
        {
            switch (data.Command)
            {
                case (int)SocketCommand.NOTIFY:
                    MessageBox.Show(data.Message);
                    break;
                case (int)SocketCommand.NEW_GAME:
                    this.Invoke((MethodInvoker)(() =>
                    {
                        NewGame();
                        SetChessBoardEnabled(false);
                    }));
                    break;
                case (int)SocketCommand.SEND_POINT:
                    this.Invoke((MethodInvoker)(() =>
                    {
                        prcbCoolDown.Value = 0;
                        SetChessBoardEnabled(true);
                        tmCoolDown.Start();
                        ChessBoard.OtherPlayerMark(data.Point);
                        btnLAN.Text = "Đã kết nối";
                    }));
                    break;
                case (int)SocketCommand.UNDO:
                    this.Invoke((MethodInvoker)(() =>
                    {
                        Undo();
                        prcbCoolDown.Value = 0;
                    }));
                    break;
                case (int)SocketCommand.END_GAME:
                    MessageBox.Show("Kết thúc vì có 5 quân trên 1 hàng");
                    break;
                case (int)SocketCommand.TIME_OUT:
                    MessageBox.Show("Hết giờ");
                    break;
                case (int)SocketCommand.QUIT:
                    this.Invoke((MethodInvoker)(() =>
                    {
                        tmCoolDown.Stop();
                        MessageBox.Show("Đối phương đã thoát khỏi trò chơi", "Thông báo");
                    }));
                    break;
                case (int)SocketCommand.CHAT_MESSAGE:
                    this.Invoke((MethodInvoker)(() =>
                    {
                        string[] parts = data.Message.Split(new char[] { '|' }, 2);
                        string senderName = parts.Length > 1 ? parts[0] : "Player";
                        string chatMessage = parts.Length > 1 ? parts[1] : data.Message;
                        
                        AppendChatMessage(senderName, chatMessage);
                    }));
                    break;
                default:
                    break;
            }

            Listen();
        }

        #region Game Mode and Undo/Redo
        private void cboGameMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            GameMode newMode;
            switch (cboGameMode.SelectedIndex)
            {
                case 0:
                    newMode = GameMode.PvC;
                    break;
                case 1:
                    newMode = GameMode.LocalMultiplayer;
                    break;
                case 2:
                    newMode = GameMode.LAN;
                    break;
                default:
                    newMode = GameMode.PvC;
                    break;
            }

            if (currentGameMode != newMode)
            {
                currentGameMode = newMode;
                NewGame();
                
                if (newMode == GameMode.LAN)
                {
                    SetChessBoardEnabled(false);
                    btnChooseAvatar.Enabled = true;
                    SetChatEnabled(true);
                    
                    LoadLanSettings();
                    UpdatePlayerUI();
                }

                else if (newMode == GameMode.PvC)
                {
                    SetChessBoardEnabled(true);
                    btnChooseAvatar.Enabled = true;
                    SetChatEnabled(false);
                    
                    // Load settings nhưng set tên máy
                    LoadPlayerSettings();
                    ChessBoard.Player[1].Name = "Computer";
                    ChessBoard.Player[1].Avatar = Image.FromFile(Application.StartupPath + "\\Resources\\P2.png");
                    
                    UpdatePlayerUI();
                }
                else
                {
                    SetChessBoardEnabled(true);
                    btnChooseAvatar.Enabled = true;
                    SetChatEnabled(false);
                    
                    LoadPlayerSettings();
                    UpdatePlayerUI();
                }
            }
        }

        private void btnUndo_Click(object sender, EventArgs e)
        {
            Undo();
            UpdateMenuForGameMode();
        }
        private void btnRedo_Click(object sender, EventArgs e)
        {
            Redo();
            UpdateMenuForGameMode();
        }
        #endregion

        private void LoadPlayerSettings()
        {
            playerSettings = PlayerSettings.Load();

            ChessBoard.Player[0].Name = playerSettings.Player1Name;
            ChessBoard.Player[1].Name = playerSettings.Player2Name;

            Image avatar1 = PlayerSettings.LoadAvatarImage(playerSettings.Player1AvatarPath);
            if (avatar1 != null)
            {
                ChessBoard.Player[0].Avatar = avatar1;
            }

            Image avatar2 = PlayerSettings.LoadAvatarImage(playerSettings.Player2AvatarPath);
            if (avatar2 != null)
            {
                ChessBoard.Player[1].Avatar = avatar2;
            }
        }

        private void LoadLanSettings()
        {
            lanPlayerSettings = LanPlayerSettings.Load();

            // Ở màn hình chuẩn bị LAN, ta hiển thị thông tin của người chơi hiện tại vào Player 0 (mặc định)
            // Để người dùng thấy và chỉnh sửa
            ChessBoard.Player[0].Name = lanPlayerSettings.PlayerName;

            Image avatar = PlayerSettings.LoadAvatarImage(lanPlayerSettings.AvatarPath);
            if (avatar != null)
            {
                ChessBoard.Player[0].Avatar = avatar;
            }
        }

        private void txbPlayerName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true;

                string newName = txbPlayerName.Text.Trim();
                if (!string.IsNullOrEmpty(newName))
                {
                    int currentPlayer = ChessBoard.CurrentPlayer;
                    ChessBoard.Player[currentPlayer].Name = newName;

                    if (currentGameMode == GameMode.LAN)
                    {
                        // Chế độ LAN: Lưu vào LanPlayerSettings
                        lanPlayerSettings.PlayerName = newName;
                        lanPlayerSettings.Save();
                    }
                    else
                    {
                        // Chế độ thường: Lưu vào PlayerSettings
                        if (currentPlayer == 0)
                        {
                            playerSettings.Player1Name = newName;
                        }
                        else
                        {
                            playerSettings.Player2Name = newName;
                        }
                        playerSettings.Save();
                    }

                    MessageBox.Show(string.Format("Đã lưu tên: {0}", newName), "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }

        private void btnChooseAvatar_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Image Files|*.png;*.jpg;*.jpeg;*.bmp;";
                openFileDialog.FilterIndex = 1;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        string sourcePath = openFileDialog.FileName;
                        string savedPath = "";

                        if (currentGameMode == GameMode.LAN)
                        {
                            savedPath = PlayerSettings.SaveNamedAvatarToResources(sourcePath, "Avatar_LAN");
                        }
                        else
                        {
                            int currentPlayer = ChessBoard.CurrentPlayer;
                            savedPath = PlayerSettings.SaveAvatarToResources(sourcePath, currentPlayer);
                        }

                        if (!string.IsNullOrEmpty(savedPath))
                        {
                            Image newAvatar = PlayerSettings.LoadAvatarImage(savedPath);
                            if (newAvatar != null)
                            {
                                int currentPlayer = ChessBoard.CurrentPlayer;
                                ChessBoard.Player[currentPlayer].Avatar = newAvatar;
                                pctbMark.Image = newAvatar;

                                if (currentGameMode == GameMode.LAN)
                                {
                                    lanPlayerSettings.AvatarPath = savedPath;
                                    lanPlayerSettings.Save();
                                }
                                else
                                {
                                    if (currentPlayer == 0)
                                    {
                                        playerSettings.Player1AvatarPath = savedPath;
                                    }
                                    else
                                    {
                                        playerSettings.Player2AvatarPath = savedPath;
                                    }
                                    playerSettings.Save();
                                }

                                MessageBox.Show("Đã cập nhật avatar!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Lỗi khi chọn avatar: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        #region Chat Feature
        private void SetChatEnabled(bool enabled)
        {
            txbMessage.Enabled = enabled;
            btnSend.Enabled = enabled;
            txbLog.Enabled = enabled;
            
            if (!enabled)
            {
                txbLog.Text = "Chat chỉ khả dụng khi chơi qua LAN";
            }
            else
            {
                txbLog.Clear();
            }
        }

        private void SendChatMessage()
        {
            if (currentGameMode != GameMode.LAN)
                return;

            string message = txbMessage.Text.Trim();
            if (string.IsNullOrEmpty(message))
                return;

            string chatName = myChatName;

            AppendChatMessage(chatName, message);

            socket.Send(new SocketData((int)SocketCommand.CHAT_MESSAGE, chatName + "|" + message, new Point()));

            txbMessage.Clear();
            txbMessage.Focus();
        }

        private void AppendChatMessage(string sender, string message)
        {
            string timestamp = DateTime.Now.ToString("HH:mm:ss");
            string formattedMessage = string.Format("[{0}] {1}: {2}", timestamp, sender, message);
            
            if (txbLog.Text.Length > 0)
            {
                txbLog.AppendText(Environment.NewLine + formattedMessage);
            }
            else
            {
                txbLog.Text = formattedMessage;
            }

            txbLog.SelectionStart = txbLog.Text.Length;
            txbLog.ScrollToCaret();
        }

        private void btnSend_Click(object sender, EventArgs e)
        {
            SendChatMessage();
        }

        private void txbMessage_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; 
                SendChatMessage();
            }
        }
        #endregion

        #region Help Menu
        private void howToPlayToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string message = "HƯỚNG DẪN CHƠI CARO\n\n" +
                "- Người chơi lần lượt đánh O và X lên bàn cờ.\n\n" +
                "- Bên nào đạt 5 ký hiệu liên tiếp theo hàng, cột hoặc đường chéo sẽ THẮNG.\n\n" +
                "- Mỗi lượt có 10 giây, hết giờ sẽ thua.\n\n" +
                "CHẾ ĐỘ CHƠI:\n" +
                "- 2 người/máy: Chơi trên cùng 1 máy tính\n" +
                "- Chơi qua LAN: Chơi với người khác qua mạng LAN\n\n" +
                "PHÍM TẮT:\n" +
                "- Ctrl+N: Ván mới\n" +
                "- Ctrl+Z: Undo\n" +
                "- Ctrl+Y: Redo";
            
            MessageBox.Show(message, "Hướng dẫn chơi", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void changeNameAvatarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string message = "THAY ĐỔI TÊN:\n\n" +
                "1. Nhập tên mới vào ô 'Player O/X' bên phải màn hình.\n" +
                "2. Nhấn phím Enter để lưu.\n" +
                "3. Tên sẽ được lưu tự động và dùng cho các lần chơi sau.\n\n" +
                "THAY ĐỔI AVATAR:\n\n" +
                "1. Nhấn nút 'Thay Avatar' bên phải màn hình.\n" +
                "2. Chọn file ảnh (PNG, JPG, BMP) từ máy tính.\n" +
                "3. Avatar mới sẽ được hiển thị và lưu tự động.\n\n" +
                "LƯU Ý:\n" +
                "- Không thể đổi tên/avatar khi đang chơi qua LAN.";
            
            MessageBox.Show(message, "Thay đổi tên & avatar", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void sendMessageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string message = "GỬI TIN NHẮN (CHỈ KHẢ DỤNG KHI CHƠI QUA LAN)\n\n" +
                "1. Chọn chế độ 'Chơi qua LAN' trong danh sách.\n\n" +
                "2. Kết nối với đối thủ:\n" +
                "   - Nhập địa chỉ IP của đối thủ.\n" +
                "   - Nhấn nút 'LAN' để kết nối.\n\n" +
                "3. Gửi tin nhắn:\n" +
                "   - Nhập nội dung vào ô chat bên phải.\n" +
                "   - Nhấn Enter hoặc nút 'Gửi tin nhắn'.\n\n" +
                "4. Tin nhắn sẽ hiển thị với thời gian và tên người gửi.";
            
            MessageBox.Show(message, "Gửi tin nhắn", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Tạo Form mới
            Form aboutForm = new Form();
            aboutForm.Text = "Giới thiệu";
            aboutForm.Size = new Size(550, 240);
            aboutForm.StartPosition = FormStartPosition.CenterParent;
            aboutForm.FormBorderStyle = FormBorderStyle.FixedDialog;
            aboutForm.MaximizeBox = false;
            aboutForm.MinimizeBox = false;

            // PictureBox hiển thị avatar
            PictureBox pbAvatar = new PictureBox();
            pbAvatar.Size = new Size(130, 150);
            pbAvatar.Location = new Point(30, 20);
            pbAvatar.SizeMode = PictureBoxSizeMode.StretchImage;
            pbAvatar.BorderStyle = BorderStyle.FixedSingle;
            
            try 
            {
                // Load hình từ thư mục Resources
                string path = Application.StartupPath + "\\Resources\\avatar.jpg";
                if (System.IO.File.Exists(path))
                    pbAvatar.Image = Image.FromFile(path);
            } 
            catch { }
            
            aboutForm.Controls.Add(pbAvatar);

            // Label thông tin
            Label lblInfo = new Label();
            lblInfo.Text = "ĐỒ ÁN LẬP TRÌNH MẠNG - GAME CARO LAN\n\n" +
                           "Sinh viên thực hiện:\n" +
                           "- Họ và tên: Võ Anh Khải\n" +
                           "- Trường Đại học Giao Thông Vận Tải TP.HCM (UTH)";
            lblInfo.Location = new Point(175, 20);
            lblInfo.AutoSize = true;
            lblInfo.Font = new Font("Arial", 10, FontStyle.Regular);
            aboutForm.Controls.Add(lblInfo);
            
            // Nút OK
            Button btnOK = new Button();
            btnOK.Text = "Đóng";
            btnOK.Size = new Size(100, 30);
            btnOK.Location = new Point(410, 150);
            btnOK.DialogResult = DialogResult.OK;
            aboutForm.Controls.Add(btnOK);
            
            aboutForm.AcceptButton = btnOK;

            aboutForm.ShowDialog();
        }
        #endregion

        private async void StartAI()
        {
            // Khóa bàn cờ để người chơi không thể click lung tung trong lúc máy tính
            SetChessBoardEnabled(false);

            // Giảm độ trễ xuống còn 100ms để phản hồi nhanh hơn
            await Task.Delay(100);

            // Kiểm tra lại trạng thái game
            if (currentGameMode != GameMode.PvC || ChessBoard.CurrentPlayer != 1)
            {
                SetChessBoardEnabled(true);
                return;
            }

            // Tính toán nước đi
            Point bestMove = aiPlayer.GetBestMove();
            
            // Thực hiện nước đi
            ChessBoard.OtherPlayerMark(bestMove);

            // Mở lại bàn cờ cho người chơi
            SetChessBoardEnabled(true);
        }

        #endregion
    }
}
