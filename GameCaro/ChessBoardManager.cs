using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GameCaro
{
    public class ChessBoardManager
    {
        #region Properties
        private Panel chessBoard;
        public Panel ChessBoard
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
            get => currentPlayer; 
            set => currentPlayer = value; 
        }

        private TextBox playerName;
        public TextBox PlayerName 
        { 
            get => playerName; 
            set => playerName = value; 
        }

        private PictureBox playerMark;
        public PictureBox PlayerMark
        { 
            get => playerMark; 
            set => playerMark = value; 
        }

        // Sử dụng Button thông thường
        private List<List<Button>> matrix;
        public List<List<Button>> Matrix 
        { 
            get => matrix;
            set => matrix = value; 
        }

        private Label labelCountO;
        public Label LabelCountO
        {
            get => labelCountO;
            set => labelCountO = value;
        }

        private Label labelCountX;
        public Label LabelCountX
        {
            get => labelCountX;
            set => labelCountX = value;
        }

        private int countO = 0;
        private int countX = 0;

        private event EventHandler<ButtonClickEvent> playerMarked;
        public event EventHandler<ButtonClickEvent> PlayerMarked
        {
            add
            {
                playerMarked += value;
            }
            remove
            {
                playerMarked -= value;
            }
        }

        private event EventHandler endedGame;
        public event EventHandler EndedGame
        {
            add
            {
                endedGame += value;
            }
            remove
            {
                endedGame -= value;
            }
        }

        private Stack<PlayInfo> playTimeLine;
        public Stack<PlayInfo> PlayTimeLine 
        { 
            get => playTimeLine; 
            set => playTimeLine = value; 
        }

        private Stack<PlayInfo> redoTimeLine;
        public Stack<PlayInfo> RedoTimeLine 
        { 
            get => redoTimeLine; 
            set => redoTimeLine = value; 
        }

        #endregion

        #region Initialize
        public ChessBoardManager(Panel chessBoard, TextBox playerName, PictureBox mark, Label lblCountO, Label lblCountX)
        {
            this.ChessBoard = chessBoard;
            this.PlayerName = playerName;
            this.PlayerMark = mark;

            this.LabelCountO = lblCountO;
            this.LabelCountX = lblCountX;

            // Load hình từ Resources
            this.Player = new List<Player>()
            { 
                new Player("Player O", Image.FromFile(Application.StartupPath + "\\Resources\\P1.png")),
                new Player("Player X", Image.FromFile(Application.StartupPath + "\\Resources\\P2.png"))
            };

        }
        #endregion
        #region Methods
        public void DrawChessBoard()
        {
            ChessBoard.Enabled = true;
            ChessBoard.Controls.Clear();

            PlayTimeLine = new Stack<PlayInfo>();
            RedoTimeLine = new Stack<PlayInfo>();

            CurrentPlayer = 0;

            ChangePlayer();

            Matrix = new List<List<Button>>();
            
            // Sử dụng kích thước ô cố định từ Cons
            int cellWidth = Cons.CHESS_WIDTH;
            int cellHeight = Cons.CHESS_HEIGHT;
            
            // Sử dụng số lượng ô cố định từ Cons
            int numCols = Cons.CHESS_BOARD_WIDTH;
            int numRows = Cons.CHESS_BOARD_HEIGHT;
            
            // Bắt đầu từ góc (0,0)
            int startX = 0;
            int startY = 0;
            
            for (int i = 0; i < numRows; i++)
            {
                Matrix.Add(new List<Button>());
                for (int j = 0; j < numCols; j++)
                {
                    Button btn = new Button()
                    {
                        Width = cellWidth,
                        Height = cellHeight,
                        Location = new Point(startX + j * cellWidth, startY + i * cellHeight),
                        Tag = i.ToString(),
                        BackgroundImageLayout = ImageLayout.Stretch
                    };

                    btn.Click += btn_Click;

                    ChessBoard.Controls.Add(btn);

                    Matrix[i].Add(btn);
                }
            }
            
            countO = 0;
            countX = 0;
        }

        void btn_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;

            // Kiểm tra nếu ô đã được đánh (có hình)
            if (btn.BackgroundImage != null)
                return;

            // Clear redo stack khi có nước đi mới
            RedoTimeLine.Clear();

            Mark(btn);

            PlayTimeLine.Push(new PlayInfo(GetChessPoint(btn), CurrentPlayer));

            CurrentPlayer = CurrentPlayer == 1 ? 0 : 1;

            ChangePlayer();

            if (playerMarked != null)
                playerMarked(this, new ButtonClickEvent(GetChessPoint(btn)));

            if (isEndGame(btn))
            {
                EndGame();
            }
        }

        public void OtherPlayerMark(Point point)
        {
            Button btn = Matrix[point.Y][point.X];
            // Kiểm tra nếu ô đã được đánh
            if (btn.BackgroundImage != null)
                return;

            Mark(btn);

            PlayTimeLine.Push(new PlayInfo(GetChessPoint(btn), CurrentPlayer));

            CurrentPlayer = CurrentPlayer == 1 ? 0 : 1;

            ChangePlayer();

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
            if (PlayTimeLine.Count <= 0)
                return false;

            PlayInfo oldPoint = PlayTimeLine.Pop();
            RedoTimeLine.Push(oldPoint); // Lưu để có thể redo
            Button btn = matrix[oldPoint.Point.Y][oldPoint.Point.X];

            // Reset ô cờ về trạng thái ban đầu
            btn.BackgroundImage = null;

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

            if (PlayTimeLine.Count <= 0)
            {
                CurrentPlayer = 0;
            }
            else
            {
                oldPoint = PlayTimeLine.Peek();
                CurrentPlayer = oldPoint.currentPlayer == 1 ? 0 : 1;
            }

            ChangePlayer();

            return true;
        }

        /// <summary>
        /// Redo nước đi đã undo
        /// </summary>
        public bool Redo()
        {
            if (RedoTimeLine.Count <= 0)
                return false;

            PlayInfo redoPoint = RedoTimeLine.Pop();
            Button btn = Matrix[redoPoint.Point.Y][redoPoint.Point.X];

            // Đánh lại nước đã undo
            CurrentPlayer = redoPoint.currentPlayer;
            Mark(btn);
            PlayTimeLine.Push(redoPoint);

            CurrentPlayer = CurrentPlayer == 1 ? 0 : 1;
            ChangePlayer();

            return true;
        }

        private bool isEndGame(Button btn)
        {
            return isEndHorizontal(btn) || isEndVertical(btn) || isEndPrimary(btn) || isEndSub(btn);
        }

        private Point GetChessPoint(Button btn)
        {
            int vertical = Convert.ToInt32(btn.Tag);
            int horizontal = Matrix[vertical].IndexOf(btn);

            Point point = new Point(horizontal, vertical);

            return point;
        }

        // Lấy index của player từ Button (dựa vào hình)
        private int GetPlayerMark(Button btn)
        {
            if (btn.BackgroundImage == null)
                return -1;
            
            // So sánh hình
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

            int countLeft = 0;
            for(int i = point.X; i >= 0; i--)
            {
                if (GetPlayerMark(Matrix[point.Y][i]) == playerMark && playerMark >= 0)
                {
                    countLeft++;
                }
                else
                    break;
            }

            int countRight = 0;
            for (int i = point.X+1; i < Cons.CHESS_BOARD_WIDTH; i++)
            {
                if (GetPlayerMark(Matrix[point.Y][i]) == playerMark && playerMark >= 0)
                {
                    countRight++;
                }
                else
                    break;
            }

            return countLeft + countRight == 5;
        }

        private bool isEndVertical(Button btn)
        {
            Point point = GetChessPoint(btn);
            int playerMark = GetPlayerMark(btn);

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

            return countTop + countBottom == 5;
        }

        private bool isEndPrimary(Button btn)
        {
            Point point = GetChessPoint(btn);
            int playerMark = GetPlayerMark(btn);

            int countTop = 0;
            for (int i = 0; i <= point.X; i++)
            {
                if (point.X - i < 0 || point.Y - i < 0)
                    break;
                if (GetPlayerMark(Matrix[point.Y-i][point.X-i]) == playerMark && playerMark >= 0)
                {
                    countTop++;
                }
                else
                    break;
            }

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

            return countTop + countBottom == 5;
        }

        private bool isEndSub(Button btn)
        {
            Point point = GetChessPoint(btn);
            int playerMark = GetPlayerMark(btn);

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

            return countTop + countBottom == 5;
        }

        private void Mark(Button btn)
        {
            // Hiển thị hình người chơi hiện tại
            btn.BackgroundImage = Player[CurrentPlayer].Mark;

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
            PlayerName.Text = Player[CurrentPlayer].Name;
            // Hiển thị Avatar của player (khác với Mark - quân cờ)
            PlayerMark.Image = Player[CurrentPlayer].Avatar;
        }
        #endregion
    }
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
}
