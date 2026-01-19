using System;
using System.Drawing;
using System.Net.NetworkInformation;
using System.Threading;
using System.Windows.Forms;
using static GameCaro.SocketData;
using static GameCaro.ChessBoardManager;

namespace GameCaro
{
    public partial class Form1 : Form
    {
        #region Properties
        ChessBoardManager ChessBoard;
        SocketManager socket;
        PlayerSettings playerSettings;
        
        // Game mode
        private GameMode currentGameMode = GameMode.LocalMultiplayer;
        #endregion

        public Form1()
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

            // Load player settings (tên và avatar đã lưu)
            LoadPlayerSettings();

            // Khởi tạo ComboBox game mode (mặc định là 2 người chơi)
            cboGameMode.SelectedIndex = 0; // 0: 2 người chơi, 1: LAN

            NewGame();
        }

        #region Methods
        /// <summary>
        /// Bật/tắt bàn cờ
        /// </summary>
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

        /// <summary>
        /// Cập nhật menu và buttons theo chế độ chơi
        /// </summary>
        void UpdateMenuForGameMode()
        {
            bool allowUndoRedo = currentGameMode != GameMode.LAN;
            bool canUndo = allowUndoRedo && ChessBoard.PlayTimeLine.Count > 0;
            bool canRedo = allowUndoRedo && ChessBoard.RedoTimeLine.Count > 0;
            
            // Cập nhật menu items
            undoToolStripMenuItem.Enabled = canUndo;
            redoToolStripMenuItem.Enabled = canRedo;
            
            // Cập nhật buttons trong panel4
            btnUndo.Enabled = canUndo;
            btnRedo.Enabled = canRedo;
        }

        private void ChessBoard_PlayerMarked(object sender, ButtonClickEvent e)
        {
            UpdateMenuForGameMode();
            
            // Dừng timer trước, reset về 0, rồi start lại
            tmCoolDown.Stop();
            prcbCoolDown.Value = 0;
            tmCoolDown.Start();
            
            // Trong chế độ LAN, gửi data qua socket
            if (currentGameMode == GameMode.LAN)
            {
                // Dừng timer của mình vì đến lượt đối phương
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
            
            // Hiển thị thông báo người thắng
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

            if (!socket.ConnectServer())
            {
                socket.isServer = true;
                SetChessBoardEnabled(true);
                socket.CreateServer();
                btnLAN.Text = "Chờ kết nối...";
            }
            else
            {
                socket.isServer = false;
                SetChessBoardEnabled(false);
                Listen();
                btnLAN.Text = "Đã kết nối";
            }
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

                    // Kiểm tra nếu data null (không có kết nối hoặc kết nối bị đóng)
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
                        Undo();
                        prcbCoolDown.Value = 0;
                    break;
                case (int)SocketCommand.END_GAME:
                    MessageBox.Show("Kết thúc vì có 5 quân trên 1 hàng");
                    break;
                case (int)SocketCommand.TIME_OUT:
                    MessageBox.Show("Hết giờ");
                    break;
                case (int)SocketCommand.QUIT:
                    tmCoolDown.Stop();
                    MessageBox.Show("Đối phương đã thoát khỏi trò chơi", "Thông báo");
                    break;
                default:
                    break;
            }

            Listen();
        }

        #region Game Mode and Undo/Redo Button Handlers
        /// <summary>
        /// Xử lý khi người dùng thay đổi chế độ chơi từ ComboBox
        /// </summary>
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
                    btnChooseAvatar.Enabled = false; // Tắt thay avatar ở chế độ LAN
                }
                else
                {
                    SetChessBoardEnabled(true);
                    btnChooseAvatar.Enabled = true; // Bật thay avatar ở chế độ 2 người/máy
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

        /// <summary>
        /// Load tên và avatar người chơi từ settings đã lưu
        /// </summary>
        private void LoadPlayerSettings()
        {
            playerSettings = PlayerSettings.Load();

            // Cập nhật tên người chơi
            ChessBoard.Player[0].Name = playerSettings.Player1Name;
            ChessBoard.Player[1].Name = playerSettings.Player2Name;

            // Load avatar nếu có
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

        /// <summary>
        /// Khi nhấn Enter trong ô tên, lưu tên người chơi
        /// </summary>
        private void txbPlayerName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; // Tránh tiếng beep

                string newName = txbPlayerName.Text.Trim();
                if (!string.IsNullOrEmpty(newName))
                {
                    // Cập nhật tên cho người chơi hiện tại
                    int currentPlayer = ChessBoard.CurrentPlayer;
                    ChessBoard.Player[currentPlayer].Name = newName;

                    // Lưu vào settings
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

        /// <summary>
        /// Mở hộp thoại chọn avatar cho người chơi hiện tại
        /// </summary>
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

                        // Copy avatar vào thư mục Resources
                        string savedPath = PlayerSettings.SaveAvatarToResources(sourcePath, currentPlayer);

                        if (!string.IsNullOrEmpty(savedPath))
                        {
                            // Load avatar mới
                            Image newAvatar = PlayerSettings.LoadAvatarImage(savedPath);
                            if (newAvatar != null)
                            {
                                // Cập nhật avatar cho người chơi (không ảnh hưởng Mark - quân cờ)
                                ChessBoard.Player[currentPlayer].Avatar = newAvatar;
                                pctbMark.Image = newAvatar;

                                // Lưu đường dẫn vào settings
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
        #endregion
    }
}
