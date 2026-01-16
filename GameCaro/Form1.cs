using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
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
        #endregion
        public Form1()
        {
            InitializeComponent();

            Control.CheckForIllegalCrossThreadCalls = false;

            // Áp dụng giao diện hiện đại
            ApplyModernStyle();

            ChessBoard = new ChessBoardManager(pnlChessBoard, txbPlayerName, pctbMark, lblCountO, lblCountX);
            ChessBoard.EndedGame += ChessBoard_EndedGame;
            ChessBoard.PlayerMarked += ChessBoard_PlayerMarked;

            prcbCoolDown.Step = Cons.COOL_DOWN_STEP;
            prcbCoolDown.Maximum = Cons.COOL_DOWN_TIME;
            prcbCoolDown.Value = 0;

            tmCoolDown.Interval = Cons.COOL_DOWN_INTERVAL;

            socket = new SocketManager();

            NewGame();
        }

        #region Modern UI Setup
        /// <summary>
        /// Áp dụng giao diện hiện đại cho toàn bộ form
        /// </summary>
        private void ApplyModernStyle()
        {
            // Form settings
            this.BackColor = ModernColors.DarkBackground;
            this.ForeColor = ModernColors.TextPrimary;
            this.Font = new Font("Segoe UI", 10);
            
            // Panel chessboard
            pnlChessBoard.BackColor = ModernColors.BoardBackground;
            pnlChessBoard.BorderStyle = BorderStyle.None;
            
            // Panel thông tin bên phải
            panel4.BackColor = ModernColors.CardBackground;
            // Giữ nguyên hình nền panel1 từ Resources
            
            // Labels
            ApplyLabelStyle(label1);
            ApplyLabelStyle(label2);
            ApplyLabelStyle(label5);
            ApplyLabelStyle(lblCountO, ModernColors.PlayerO);
            ApplyLabelStyle(lblCountX, ModernColors.PlayerX);
            
            // TextBox IP
            txbIP.BackColor = ModernColors.CardBackgroundLight;
            txbIP.ForeColor = ModernColors.TextPrimary;
            txbIP.BorderStyle = BorderStyle.FixedSingle;
            txbIP.Font = new Font("Segoe UI", 11);
            
            // TextBox Player Name
            txbPlayerName.BackColor = ModernColors.CardBackgroundLight;
            txbPlayerName.ForeColor = ModernColors.TextPrimary;
            txbPlayerName.BorderStyle = BorderStyle.FixedSingle;
            txbPlayerName.Font = new Font("Segoe UI", 11, FontStyle.Bold);
            
            // Button LAN
            ApplyButtonStyle(btnLAN);
            
            // PictureBox Mark - giữ nguyên để hiển thị hình từ Resources
            pctbMark.BorderStyle = BorderStyle.None;
            
            // ProgressBar - sử dụng custom drawing
            prcbCoolDown.BackColor = ModernColors.CardBackgroundLight;
            prcbCoolDown.ForeColor = ModernColors.Primary;
            
            // MenuStrip
            menuStrip1.BackColor = ModernColors.CardBackground;
            menuStrip1.ForeColor = ModernColors.TextPrimary;
            menuStrip1.RenderMode = ToolStripRenderMode.Professional;
            menuStrip1.Renderer = new ModernMenuRenderer();
        }

        private void ApplyLabelStyle(System.Windows.Forms.Label label, Color? overrideColor = null)
        {
            label.ForeColor = overrideColor ?? ModernColors.TextPrimary;
            label.BackColor = Color.Transparent;
        }

        private void ApplyButtonStyle(Button button)
        {
            button.FlatStyle = FlatStyle.Flat;
            button.FlatAppearance.BorderSize = 0;
            button.BackColor = ModernColors.Primary;
            button.ForeColor = ModernColors.TextPrimary;
            button.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            button.Cursor = Cursors.Hand;
            
            // Hiệu ứng hover
            button.MouseEnter += (s, e) => button.BackColor = ModernColors.PrimaryLight;
            button.MouseLeave += (s, e) => button.BackColor = ModernColors.Primary;
        }

        /// <summary>
        /// Bật/tắt bàn cờ với hiệu ứng visual rõ ràng
        /// </summary>
        private void SetChessBoardEnabled(bool enabled)
        {
            pnlChessBoard.Enabled = enabled;
            
            // Hiệu ứng mờ khi bị vô hiệu hóa
            if (enabled)
            {
                // Bàn cờ sáng - đang chơi được
                pnlChessBoard.BackColor = ModernColors.BoardBackground;
                foreach (Control ctrl in pnlChessBoard.Controls)
                {
                    if (ctrl is ChessButton btn)
                    {
                        btn.Enabled = true;
                        btn.BackColor = ModernColors.BoardCell;  // Màu trắng
                    }
                }
            }
            else
            {
                // Bàn cờ tối/mờ - đang chờ đối thủ
                pnlChessBoard.BackColor = Color.FromArgb(180, 180, 180); // Xám đậm hơn
                foreach (Control ctrl in pnlChessBoard.Controls)
                {
                    if (ctrl is ChessButton btn)
                    {
                        btn.Enabled = false;
                        btn.BackColor = Color.FromArgb(220, 220, 220);  // Xám nhạt cho ô cờ
                    }
                }
            }
            pnlChessBoard.Refresh();
        }
        #endregion

        #region Methods
        void EndGame()
        {
            tmCoolDown.Stop();
            undoToolStripMenuItem.Enabled = false;
            SetChessBoardEnabled(false);
            //MessageBox.Show("Kết thúc");
        }

        void NewGame()
        {
            prcbCoolDown.Value = 0;
            tmCoolDown.Stop();
            undoToolStripMenuItem.Enabled = true;
            lblCountO.Text = "O:0";
            lblCountX.Text = "X:0";

            ChessBoard.DrawChessBoard();
        }

        void Quit()
        {
            Application.Exit();
        }

        void Undo()
        {
            ChessBoard.Undo();
            prcbCoolDown.Value = 0;
        }

        private void ChessBoard_PlayerMarked(object sender, ButtonClickEvent e)
        {
            tmCoolDown.Start();
            SetChessBoardEnabled(false);
            prcbCoolDown.Value = 0;

            socket.Send(new SocketData((int)SocketCommand.SEND_POINT, "", e.ClickedPoint));

            undoToolStripMenuItem.Enabled = false;

            Listen();
        }

        private void ChessBoard_EndedGame(object sender, EventArgs e)
        {
            EndGame();
            socket.Send(new SocketData((int)SocketCommand.END_GAME, "", new Point()));

        }

        private void tmCoolDown_Tick(object sender, EventArgs e)
        {
            prcbCoolDown.PerformStep();

            if(prcbCoolDown.Value >= prcbCoolDown.Maximum)
            {
                EndGame();
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
            socket.IP = txbIP.Text;

            if (!socket.ConnectServer())
            {
                socket.isServer = true;
                SetChessBoardEnabled(true);
                socket.CreateServer();
            }
            else
            {
                socket.isServer = false;
                SetChessBoardEnabled(false);
                Listen();
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

                    ProcessData(data);
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
        #endregion


    }
}
