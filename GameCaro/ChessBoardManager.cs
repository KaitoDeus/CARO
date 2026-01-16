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

        // Sử dụng ChessButton thay vì Button thông thường
        private List<List<ChessButton>> matrix;
        public List<List<ChessButton>> Matrix 
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
            ChessBoard.BackColor = ModernColors.BoardBackground;

            PlayTimeLine = new Stack<PlayInfo>();

            CurrentPlayer = 0;

            ChangePlayer();

            Matrix = new List<List<ChessButton>>();
            
            // Tính kích thước ô để lắp đầy panel
            int cellWidth = ChessBoard.Width / Cons.CHESS_BOARD_WIDTH;
            int cellHeight = ChessBoard.Height / Cons.CHESS_BOARD_HEIGHT;
            
            // Bắt đầu từ góc (0,0) để lắp đầy
            int startX = 0;
            int startY = 0;
            
            for (int i = 0; i < Cons.CHESS_BOARD_HEIGHT; i++)
            {
                Matrix.Add(new List<ChessButton>());
                for (int j = 0; j < Cons.CHESS_BOARD_WIDTH; j++)
                {
                    ChessButton btn = new ChessButton()
                    {
                        Width = cellWidth,
                        Height = cellHeight,
                        Location = new Point(startX + j * cellWidth, startY + i * cellHeight),
                        Tag = i.ToString()
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
            ChessButton btn = sender as ChessButton;

            // Kiểm tra nếu ô đã được đánh
            if (btn.PlayerMark >= 0)
                return;

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
            ChessButton btn = Matrix[point.Y][point.X];
            // Kiểm tra nếu ô đã được đánh
            if (btn.PlayerMark >= 0)
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
            ChessButton btn = matrix[oldPoint.Point.Y][oldPoint.Point.X];

            // Reset ô cờ về trạng thái ban đầu
            btn.ResetMark();

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

        private bool isEndGame(ChessButton btn)
        {
            return isEndHorizontal(btn) || isEndVertical(btn) || isEndPrimary(btn) || isEndSub(btn);
        }

        private Point GetChessPoint(ChessButton btn)
        {
            int vertical = Convert.ToInt32(btn.Tag);
            int horizontal = Matrix[vertical].IndexOf(btn);

            Point point = new Point(horizontal, vertical);

            return point;
        }

        private bool isEndHorizontal(ChessButton btn)
        {
            Point point = GetChessPoint(btn);

            int countLeft = 0;
            for(int i = point.X; i >= 0; i--)
            {
                if (Matrix[point.Y][i].PlayerMark == btn.PlayerMark && btn.PlayerMark >= 0)
                {
                    countLeft++;
                }
                else
                    break;
            }

            int countRight = 0;
            for (int i = point.X+1; i < Cons.CHESS_BOARD_WIDTH; i++)
            {
                if (Matrix[point.Y][i].PlayerMark == btn.PlayerMark && btn.PlayerMark >= 0)
                {
                    countRight++;
                }
                else
                    break;
            }

            return countLeft + countRight == 5;
        }

        private bool isEndVertical(ChessButton btn)
        {
            Point point = GetChessPoint(btn);

            int countTop = 0;
            for (int i = point.Y; i >= 0; i--)
            {
                if (Matrix[i][point.X].PlayerMark == btn.PlayerMark && btn.PlayerMark >= 0)
                {
                    countTop++;
                }
                else
                    break;
            }

            int countBottom = 0;
            for (int i = point.Y + 1; i < Cons.CHESS_BOARD_HEIGHT; i++)
            {
                if (Matrix[i][point.X].PlayerMark == btn.PlayerMark && btn.PlayerMark >= 0)
                {
                    countBottom++;
                }
                else
                    break;
            }

            return countTop + countBottom == 5;
        }

        private bool isEndPrimary(ChessButton btn)
        {
            Point point = GetChessPoint(btn);

            int countTop = 0;
            for (int i = 0; i <= point.X; i++)
            {
                if (point.X - i < 0 || point.Y - i < 0)
                    break;
                if (Matrix[point.Y-i][point.X-i].PlayerMark == btn.PlayerMark && btn.PlayerMark >= 0)
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
                if (Matrix[point.Y + i][point.X + i].PlayerMark == btn.PlayerMark && btn.PlayerMark >= 0)
                {
                    countBottom++;
                }
                else
                    break;
            }

            return countTop + countBottom == 5;
        }

        private bool isEndSub(ChessButton btn)
        {
            Point point = GetChessPoint(btn);

            int countTop = 0;
            for (int i = 0; i <= point.X; i++)
            {
                if (point.X + i > Cons.CHESS_BOARD_WIDTH || point.Y - i < 0)
                    break;
                if (Matrix[point.Y - i][point.X + i].PlayerMark == btn.PlayerMark && btn.PlayerMark >= 0)
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
                if (Matrix[point.Y + i][point.X - i].PlayerMark == btn.PlayerMark && btn.PlayerMark >= 0)
                {
                    countBottom++;
                }
                else
                    break;
            }

            return countTop + countBottom == 5;
        }

        private void Mark(ChessButton btn)
        {
            // Sử dụng PlayerMark property với animation
            btn.PlayerMark = CurrentPlayer;

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
            // Hiển thị hình ảnh player mark từ Resources
            PlayerMark.Image = Player[CurrentPlayer].Mark;
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
