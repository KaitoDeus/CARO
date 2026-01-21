using System;
using System.Drawing;
using System.Net.NetworkInformation;
using System.Threading;
using System.Windows.Forms;
using static GameCaro.SocketData;
using static GameCaro.ChessBoardManager;

namespace GameCaro
{
    public partial class GameCaro : Form
    {
        #region Properties
        ChessBoardManager ChessBoard;
        SocketManager socket;
        PlayerSettings playerSettings;
        
        private GameMode currentGameMode = GameMode.LocalMultiplayer;
        private string myChatName = "";
        #endregion

        public GameCaro()
        {
            InitializeComponent();

            // Control.CheckForIllegalCrossThreadCalls = false;

            ChessBoard = new ChessBoardManager(pnlChessBoard, txbPlayerName, pctbMark, lblCountO, lblCountX);
            ChessBoard.EndedGame += ChessBoard_EndedGame;
            ChessBoard.PlayerMarked += ChessBoard_PlayerMarked;

            prcbCoolDown.Step = Cons.COOL_DOWN_STEP;
            prcbCoolDown.Maximum = Cons.COOL_DOWN_TIME;
            prcbCoolDown.Value = 0;

            tmCoolDown.Interval = Cons.COOL_DOWN_INTERVAL;

            socket = new SocketManager();
            socket.ClientConnected += Socket_ClientConnected;

            LoadPlayerSettings();

            cboGameMode.SelectedIndex = 0;
            SetChatEnabled(false);

            NewGame();
        }

        #region Methods
        private void SetChessBoardEnabled(bool enabled)
        {
            pnlChessBoard.Enabled = enabled;
        }

        void EndGame()
        {
            tmCoolDown.Stop();
            undoToolStripMenuItem.Enabled = false;
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
            
            undoToolStripMenuItem.Enabled = canUndo;
            redoToolStripMenuItem.Enabled = canRedo;
            
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

                undoToolStripMenuItem.Enabled = false;
                redoToolStripMenuItem.Enabled = false;

                Listen();
            }
        }

        private void ChessBoard_EndedGame(object sender, EventArgs e)
        {
            EndGame();
            
            int winner = ChessBoard.CurrentPlayer == 0 ? 1 : 0;
            string winnerName = ChessBoard.Player[winner].Name;
            string winnerMark = winner == 0 ? "O" : "X";
            
            MessageBox.Show($"Người chơi {winnerName} ({winnerMark}) đã thắng!", "Kết thúc", MessageBoxButtons.OK, MessageBoxIcon.Information);
            
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
                
                MessageBox.Show($"Hết giờ! {loserName} không đánh tiếp.\n{winnerName} thắng!", 
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

        private void undoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Undo();
            UpdateMenuForGameMode();
        }

        private void redoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Redo();
            UpdateMenuForGameMode();
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

        private void btnLAN_Click(object sender, EventArgs e)
        {
            btnLAN.Enabled = false;
            txbIP.Enabled = false;

            socket.IP = txbIP.Text;
            
            myChatName = txbPlayerName.Text.Trim();
            if (string.IsNullOrEmpty(myChatName))
            {
                myChatName = socket.isServer ? "Player O" : "Player X";
            }

            if (!socket.ConnectServer())
            {
                socket.isServer = true;
                if (string.IsNullOrEmpty(myChatName) || myChatName == "Player X")
                {
                    myChatName = "Player O";
                }
                SetChessBoardEnabled(true);
                socket.CreateServer();
                btnLAN.Text = "Chờ kết nối...";
            }
            else
            {
                socket.isServer = false;
                if (string.IsNullOrEmpty(myChatName) || myChatName == "Player O")
                {
                    myChatName = "Player X";
                }
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
                        undoToolStripMenuItem.Enabled = true;
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
                    newMode = GameMode.LocalMultiplayer;
                    break;
                case 1:
                    newMode = GameMode.LAN;
                    break;
                default:
                    newMode = GameMode.LocalMultiplayer;
                    break;
            }

            if (currentGameMode != newMode)
            {
                currentGameMode = newMode;
                NewGame();
                
                if (newMode == GameMode.LAN)
                {
                    SetChessBoardEnabled(false);
                    btnChooseAvatar.Enabled = false;
                    SetChatEnabled(true);
                }
                else
                {
                    SetChessBoardEnabled(true);
                    btnChooseAvatar.Enabled = true;
                    SetChatEnabled(false);
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

                    if (currentPlayer == 0)
                    {
                        playerSettings.Player1Name = newName;
                    }
                    else
                    {
                        playerSettings.Player2Name = newName;
                    }
                    playerSettings.Save();

                    MessageBox.Show($"Đã lưu tên: {newName}", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
                        int currentPlayer = ChessBoard.CurrentPlayer;
                        string sourcePath = openFileDialog.FileName;

                        string savedPath = PlayerSettings.SaveAvatarToResources(sourcePath, currentPlayer);

                        if (!string.IsNullOrEmpty(savedPath))
                        {
                            Image newAvatar = PlayerSettings.LoadAvatarImage(savedPath);
                            if (newAvatar != null)
                            {
                                ChessBoard.Player[currentPlayer].Avatar = newAvatar;
                                pctbMark.Image = newAvatar;

                                if (currentPlayer == 0)
                                {
                                    playerSettings.Player1AvatarPath = savedPath;
                                }
                                else
                                {
                                    playerSettings.Player2AvatarPath = savedPath;
                                }
                                playerSettings.Save();

                                MessageBox.Show("Đã cập nhật avatar!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Lỗi khi chọn avatar: {ex.Message}", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            string formattedMessage = $"[{timestamp}] {sender}: {message}";
            
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
        #endregion

        #endregion
    }
}
