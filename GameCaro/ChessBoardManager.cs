using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Sunny.UI;

namespace GameCaro
{
    /// <summary>
    /// Lớp quản lý bàn cờ và logic chơi game Caro.
    /// Chịu trách nhiệm: vẽ bàn cờ, xử lý đánh cờ, kiểm tra thắng thua,
    /// quản lý Undo/Redo, và theo dõi người chơi hiện tại.
    /// 
    /// [REFACTORED] Cập nhật để tương thích với SunnyUI controls.
    /// Sử dụng standard Button cho chess cells (cần BackgroundImage).
    /// </summary>
    public class ChessBoardManager
    {
        #region Properties

        // Sử dụng Control thay vì Panel để tương thích với UIPanel
        private Control chessBoard;
        public Control ChessBoard
        {
            get { return chessBoard; }
            set { chessBoard = value; }
        }

        private List<Player> _player;
        public List<Player> Player
        {
            get { return _player; }
            set { _player = value; }
        }

        private int currentPlayer;
        public int CurrentPlayer
        {
            get { return currentPlayer; }
            set { currentPlayer = value; }
        }

        // Sử dụng Control thay vì TextBox để tương thích với UITextBox
        private Control playerName;
        public Control PlayerName
        {
            get { return playerName; }
            set { playerName = value; }
        }

        // Sử dụng Control thay vì PictureBox để tương thích với UIAvatar
        private Control playerMark;
        public Control PlayerMark
        {
            get { return playerMark; }
            set { playerMark = value; }
        }

        // Sử dụng standard Button cho các ô cờ (vì cần BackgroundImage)
        private List<List<Button>> matrix;
        public List<List<Button>> Matrix
        {
            get { return matrix; }
            set { matrix = value; }
        }

        private Label labelCountO;
        public Label LabelCountO
        {
            get { return labelCountO; }
            set { labelCountO = value; }
        }

        private Label labelCountX;
        public Label LabelCountX
        {
            get { return labelCountX; }
            set { labelCountX = value; }
        }

        private int countO = 0;
        private int countX = 0;

        #endregion

        #region Events

        private event EventHandler<ButtonClickEvent> playerMarked;
        public event EventHandler<ButtonClickEvent> PlayerMarked
        {
            add { playerMarked += value; }
            remove { playerMarked -= value; }
        }

        private event EventHandler endedGame;
        public event EventHandler EndedGame
        {
            add { endedGame += value; }
            remove { endedGame -= value; }
        }

        #endregion

        #region Undo/Redo Stacks

        private Stack<PlayInfo> playTimeLine;
        public Stack<PlayInfo> PlayTimeLine
        {
            get { return playTimeLine; }
            set { playTimeLine = value; }
        }

        private Stack<PlayInfo> redoTimeLine;
        public Stack<PlayInfo> RedoTimeLine
        {
            get { return redoTimeLine; }
            set { redoTimeLine = value; }
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Constructor với tham số Control thay vì types cụ thể để tương thích với SunnyUI.
        /// </summary>
        public ChessBoardManager(Control chessBoard, Control playerName, Control mark, Label lblCountO, Label lblCountX)
        {
            this.ChessBoard = chessBoard;
            this.PlayerName = playerName;
            this.PlayerMark = mark;
            this.LabelCountO = lblCountO;
            this.LabelCountX = lblCountX;

            // Khởi tạo 2 người chơi với hình từ Resources
            this.Player = new List<Player>()
            {
                new Player("Player O", Image.FromFile(Application.StartupPath + "\\Resources\\P1.png")),
                new Player("Player X", Image.FromFile(Application.StartupPath + "\\Resources\\P2.png"))
            };
        }

        #endregion

        #region Public Methods

        public void DrawChessBoard()
        {
            // Bật bàn cờ và xóa các ô cũ
            ChessBoard.Enabled = true;
            ChessBoard.Controls.Clear();

            // Reset các stack Undo/Redo
            PlayTimeLine = new Stack<PlayInfo>();
            RedoTimeLine = new Stack<PlayInfo>();

            // Người chơi O đi trước
            CurrentPlayer = 0;
            ChangePlayer();

            // Khởi tạo ma trận rỗng với standard Button
            Matrix = new List<List<Button>>();

            // Lấy kích thước ô từ hằng số
            int cellWidth = Cons.CHESS_WIDTH;
            int cellHeight = Cons.CHESS_HEIGHT;

            // Lấy số hàng/cột từ hằng số
            int numCols = Cons.CHESS_BOARD_WIDTH;
            int numRows = Cons.CHESS_BOARD_HEIGHT;

            // Bắt đầu vẽ từ góc (5,5) để có padding
            int startX = 5;
            int startY = 5;

            // Tạo từng hàng
            for (int i = 0; i < numRows; i++)
            {
                Matrix.Add(new List<Button>());

                // Tạo từng ô trong hàng
                for (int j = 0; j < numCols; j++)
                {
                    // Màu xen kẽ tạo hiệu ứng bàn cờ
                    bool isEvenCell = (i + j) % 2 == 0;
                    Color cellColor = isEvenCell 
                        ? Color.FromArgb(255, 255, 255)    // Trắng
                        : Color.FromArgb(245, 248, 252);   // Xanh nhạt

                    // Sử dụng standard Button với styling cải tiến
                    Button btn = new Button()
                    {
                        Width = cellWidth,
                        Height = cellHeight,
                        Location = new Point(startX + j * cellWidth, startY + i * cellHeight),
                        Tag = i.ToString(), // Lưu row index vào Tag
                        BackgroundImageLayout = ImageLayout.Stretch,
                        
                        // Modern styling
                        Text = "",
                        FlatStyle = FlatStyle.Flat,
                        BackColor = cellColor,
                        Cursor = Cursors.Hand
                    };

                    // Styling cho flat button với border
                    btn.FlatAppearance.BorderColor = Color.FromArgb(200, 210, 220);
                    btn.FlatAppearance.BorderSize = 1;
                    btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(220, 240, 255);
                    btn.FlatAppearance.MouseDownBackColor = Color.FromArgb(180, 220, 255);

                    // Đăng ký sự kiện click (GIỮ NGUYÊN LOGIC)
                    btn.Click += btn_Click;

                    // Thêm vào panel và ma trận
                    ChessBoard.Controls.Add(btn);
                    Matrix[i].Add(btn);
                }
            }

            // Reset bộ đếm quân
            countO = 0;
            countX = 0;
        }

        /// <summary>
        /// Vẽ bàn cờ với hiệu ứng animation từng hàng từ trên xuống
        /// </summary>
        public async Task DrawChessBoardAnimatedAsync()
        {
            // Bật bàn cờ và xóa các ô cũ
            ChessBoard.Enabled = true;
            ChessBoard.Controls.Clear();

            // Reset các stack Undo/Redo
            PlayTimeLine = new Stack<PlayInfo>();
            RedoTimeLine = new Stack<PlayInfo>();

            // Người chơi O đi trước
            CurrentPlayer = 0;
            ChangePlayer();

            // Khởi tạo ma trận rỗng với standard Button
            Matrix = new List<List<Button>>();

            // Lấy kích thước ô từ hằng số
            int cellWidth = Cons.CHESS_WIDTH;
            int cellHeight = Cons.CHESS_HEIGHT;

            // Lấy số hàng/cột từ hằng số
            int numCols = Cons.CHESS_BOARD_WIDTH;
            int numRows = Cons.CHESS_BOARD_HEIGHT;

            // Bắt đầu vẽ từ góc (5,5) để có padding
            int startX = 5;
            int startY = 5;

            // Tạo từng hàng với animation
            for (int i = 0; i < numRows; i++)
            {
                Matrix.Add(new List<Button>());

                // Tạo từng ô trong hàng
                for (int j = 0; j < numCols; j++)
                {
                    // Màu xen kẽ tạo hiệu ứng bàn cờ
                    bool isEvenCell = (i + j) % 2 == 0;
                    Color cellColor = isEvenCell 
                        ? Color.FromArgb(255, 255, 255)    // Trắng
                        : Color.FromArgb(245, 248, 252);   // Xanh nhạt

                    // Sử dụng standard Button với styling cải tiến
                    Button btn = new Button()
                    {
                        Width = cellWidth,
                        Height = cellHeight,
                        Location = new Point(startX + j * cellWidth, startY + i * cellHeight),
                        Tag = i.ToString(), // Lưu row index vào Tag
                        BackgroundImageLayout = ImageLayout.Stretch,
                        
                        // Modern styling
                        Text = "",
                        FlatStyle = FlatStyle.Flat,
                        BackColor = cellColor,
                        Cursor = Cursors.Hand
                    };

                    // Styling cho flat button với border
                    btn.FlatAppearance.BorderColor = Color.FromArgb(200, 210, 220);
                    btn.FlatAppearance.BorderSize = 1;
                    btn.FlatAppearance.MouseOverBackColor = Color.FromArgb(220, 240, 255);
                    btn.FlatAppearance.MouseDownBackColor = Color.FromArgb(180, 220, 255);

                    // Đăng ký sự kiện click (GIỮ NGUYÊN LOGIC)
                    btn.Click += btn_Click;

                    // Thêm vào panel và ma trận
                    ChessBoard.Controls.Add(btn);
                    Matrix[i].Add(btn);
                }

                // Delay nhỏ sau mỗi hàng để tạo hiệu ứng animation
                await Task.Delay(15); // 15ms delay giữa các hàng
            }

            // Reset bộ đếm quân
            countO = 0;
            countX = 0;
        }

        public void OtherPlayerMark(Point point)
        {
            Button btn = Matrix[point.Y][point.X];

            // Kiểm tra ô đã được đánh chưa
            if (btn.BackgroundImage != null)
                return;

            // Đánh quân vào ô
            Mark(btn);

            // Lưu vào lịch sử
            PlayTimeLine.Push(new PlayInfo(GetChessPoint(btn), CurrentPlayer));

            // Chuyển lượt
            CurrentPlayer = CurrentPlayer == 1 ? 0 : 1;
            ChangePlayer();

            // Kiểm tra kết thúc game
            if (isEndGame(btn))
            {
                EndGame();
            }
        }

        public void EndGame()
        {
            if (endedGame != null)
                endedGame(this, new EventArgs());
        }

        public bool Undo()
        {
            // Kiểm tra có nước đi để undo không
            if (PlayTimeLine.Count <= 0)
                return false;

            // Lấy nước đi cuối từ stack
            PlayInfo oldPoint = PlayTimeLine.Pop();
            
            // Đẩy vào stack Redo để có thể redo
            RedoTimeLine.Push(oldPoint);
            
            // Lấy button tương ứng
            Button btn = matrix[oldPoint.Point.Y][oldPoint.Point.X];

            // Xóa hình trên ô
            btn.BackgroundImage = null;

            // Cập nhật bộ đếm
            if (oldPoint.currentPlayer == 0)
            {
                countO--;
                LabelCountO.Text = "O:" + countO.ToString();
            }
            else
            {
                countX--;
                LabelCountX.Text = "X:" + countX.ToString();
            }

            // Xác định người chơi tiếp theo
            if (PlayTimeLine.Count <= 0)
            {
                // Không còn nước nào -> quay về Player O
                CurrentPlayer = 0;
            }
            else
            {
                // Lấy nước trước đó để xác định lượt
                oldPoint = PlayTimeLine.Peek();
                CurrentPlayer = oldPoint.currentPlayer == 1 ? 0 : 1;
            }

            ChangePlayer();
            return true;
        }

        public bool Redo()
        {
            // Kiểm tra có nước để redo không
            if (RedoTimeLine.Count <= 0)
                return false;

            // Lấy nước từ stack Redo
            PlayInfo redoPoint = RedoTimeLine.Pop();
            Button btn = Matrix[redoPoint.Point.Y][redoPoint.Point.X];

            // Đánh lại nước đã undo
            CurrentPlayer = redoPoint.currentPlayer;
            Mark(btn);
            
            // Đẩy lại vào stack PlayTimeLine
            PlayTimeLine.Push(redoPoint);

            // Chuyển lượt
            CurrentPlayer = CurrentPlayer == 1 ? 0 : 1;
            ChangePlayer();

            return true;
        }

        #endregion

        #region Private Methods - Event Handlers

        private void btn_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;

            // Kiểm tra ô đã được đánh chưa
            if (btn.BackgroundImage != null)
                return;

            // Xóa stack Redo khi có nước đi mới
            // (không thể redo sau khi đánh nước mới)
            RedoTimeLine.Clear();

            // Đánh quân vào ô
            Mark(btn);

            // Lưu vào lịch sử
            PlayTimeLine.Push(new PlayInfo(GetChessPoint(btn), CurrentPlayer));

            // Chuyển lượt
            CurrentPlayer = CurrentPlayer == 1 ? 0 : 1;
            ChangePlayer();

            // Kích hoạt event thông báo đã đánh
            if (playerMarked != null)
                playerMarked(this, new ButtonClickEvent(GetChessPoint(btn)));

            // Kiểm tra kết thúc game
            if (isEndGame(btn))
            {
                EndGame();
            }
        }

        #endregion

        #region Game Logic

        private bool isEndGame(Button btn)
        {
            return isEndHorizontal(btn) || isEndVertical(btn) || isEndPrimary(btn) || isEndSub(btn);
        }

        private Point GetChessPoint(Button btn)
        {
            int vertical = Convert.ToInt32(btn.Tag);  // Lấy hàng từ Tag
            int horizontal = Matrix[vertical].IndexOf(btn);  // Lấy cột từ vị trí trong list

            return new Point(horizontal, vertical);
        }

        private int GetPlayerMark(Button btn)
        {
            if (btn.BackgroundImage == null)
                return -1;

            // So sánh hình với Player marks
            if (btn.BackgroundImage == Player[0].Mark)
                return 0;
            else if (btn.BackgroundImage == Player[1].Mark)
                return 1;

            return -1;
        }

        private bool isEndHorizontal(Button btn)
        {
            Point point = GetChessPoint(btn);
            int playerMark = GetPlayerMark(btn);

            // Đếm sang trái
            int countLeft = 0;
            for (int i = point.X; i >= 0; i--)
            {
                if (GetPlayerMark(Matrix[point.Y][i]) == playerMark && playerMark >= 0)
                {
                    countLeft++;
                }
                else
                    break;
            }

            // Đếm sang phải
            int countRight = 0;
            for (int i = point.X + 1; i < Cons.CHESS_BOARD_WIDTH; i++)
            {
                if (GetPlayerMark(Matrix[point.Y][i]) == playerMark && playerMark >= 0)
                {
                    countRight++;
                }
                else
                    break;
            }

            return countLeft + countRight >= 5;
        }

        private bool isEndVertical(Button btn)
        {
            Point point = GetChessPoint(btn);
            int playerMark = GetPlayerMark(btn);

            // Đếm lên trên
            int countTop = 0;
            for (int i = point.Y; i >= 0; i--)
            {
                if (GetPlayerMark(Matrix[i][point.X]) == playerMark && playerMark >= 0)
                {
                    countTop++;
                }
                else
                    break;
            }

            // Đếm xuống dưới
            int countBottom = 0;
            for (int i = point.Y + 1; i < Cons.CHESS_BOARD_HEIGHT; i++)
            {
                if (GetPlayerMark(Matrix[i][point.X]) == playerMark && playerMark >= 0)
                {
                    countBottom++;
                }
                else
                    break;
            }

            return countTop + countBottom >= 5;
        }

        private bool isEndPrimary(Button btn)
        {
            Point point = GetChessPoint(btn);
            int playerMark = GetPlayerMark(btn);

            // Đếm lên trái (↖)
            int countTop = 0;
            for (int i = 0; i <= point.X; i++)
            {
                if (point.X - i < 0 || point.Y - i < 0)
                    break;
                if (GetPlayerMark(Matrix[point.Y - i][point.X - i]) == playerMark && playerMark >= 0)
                {
                    countTop++;
                }
                else
                    break;
            }

            // Đếm xuống phải (↘)
            int countBottom = 0;
            for (int i = 1; i <= Cons.CHESS_BOARD_WIDTH - point.X; i++)
            {
                if (point.Y + i >= Cons.CHESS_BOARD_HEIGHT || point.X + i >= Cons.CHESS_BOARD_WIDTH)
                    break;
                if (GetPlayerMark(Matrix[point.Y + i][point.X + i]) == playerMark && playerMark >= 0)
                {
                    countBottom++;
                }
                else
                    break;
            }

            return countTop + countBottom >= 5;
        }

        private bool isEndSub(Button btn)
        {
            Point point = GetChessPoint(btn);
            int playerMark = GetPlayerMark(btn);

            // Đếm lên phải (↗)
            int countTop = 0;
            for (int i = 0; i <= point.X; i++)
            {
                if (point.X + i > Cons.CHESS_BOARD_WIDTH || point.Y - i < 0)
                    break;
                if (GetPlayerMark(Matrix[point.Y - i][point.X + i]) == playerMark && playerMark >= 0)
                {
                    countTop++;
                }
                else
                    break;
            }

            // Đếm xuống trái (↙)
            int countBottom = 0;
            for (int i = 1; i <= Cons.CHESS_BOARD_WIDTH - point.X; i++)
            {
                if (point.Y + i >= Cons.CHESS_BOARD_HEIGHT || point.X - i < 0)
                    break;
                if (GetPlayerMark(Matrix[point.Y + i][point.X - i]) == playerMark && playerMark >= 0)
                {
                    countBottom++;
                }
                else
                    break;
            }

            return countTop + countBottom >= 5;
        }

        private void Mark(Button btn)
        {
            // Hiển thị hình người chơi hiện tại
            btn.BackgroundImage = Player[CurrentPlayer].Mark;

            // Cập nhật bộ đếm
            if (CurrentPlayer == 0)
            {
                countO++;
                LabelCountO.Text = "O:" + countO.ToString();
            }
            else
            {
                countX++;
                LabelCountX.Text = "X:" + countX.ToString();
            }
        }

        private void ChangePlayer()
        {
            // Cập nhật tên - xử lý cho cả TextBox và UITextBox
            if (PlayerName is UITextBox uiTextBox)
            {
                uiTextBox.Text = Player[CurrentPlayer].Name;
            }
            else if (PlayerName is TextBox textBox)
            {
                textBox.Text = Player[CurrentPlayer].Name;
            }
            else
            {
                // Fallback cho bất kỳ control nào có property Text
                PlayerName.Text = Player[CurrentPlayer].Name;
            }
            
            // Cập nhật avatar - xử lý cho cả PictureBox và UIAvatar
            if (PlayerMark is UIAvatar uiAvatar)
            {
                uiAvatar.Image = Player[CurrentPlayer].Avatar;
            }
            else if (PlayerMark is PictureBox pictureBox)
            {
                pictureBox.Image = Player[CurrentPlayer].Avatar;
            }
        }

        #endregion
    }

    #region Event Arguments

    public class ButtonClickEvent : EventArgs
    {
        private Point clickedPoint;
        public Point ClickedPoint
        {
            get { return clickedPoint; }
            set { clickedPoint = value; }
        }

        public ButtonClickEvent(Point point)
        {
            this.ClickedPoint = point;
        }
    }

    #endregion
}
