using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace GameCaro
{
    /// <summary>
    /// AI Player sử dụng thuật toán heuristic để tìm nước đi tốt nhất.
    /// 
    /// Sử dụng standard Button với BackgroundImage cho chess cells.
    /// </summary>
    public class AIPlayer
    {
        private ChessBoardManager chessBoard;
        
        // Mảng điểm tấn công (Ưu tiên nước đi tạo thế mạnh cho mình)
        private long[] AttackScore = new long[] { 0, 9, 54, 162, 1458, 13122, 118098 };
        
        // Mảng điểm phòng thủ (Ưu tiên chặn nước đi nguy hiểm của đối phương)
        private long[] DefenseScore = new long[] { 0, 3, 27, 243, 2187, 19683, 177147 };

        public AIPlayer(ChessBoardManager chessBoard)
        {
            this.chessBoard = chessBoard;
        }

        public Point GetBestMove()
        {
            Point bestPosition = new Point();
            long maxScore = 0;

            // Duyệt qua toàn bộ bàn cờ
            for (int i = 0; i < Cons.CHESS_BOARD_HEIGHT; i++)
            {
                for (int j = 0; j < Cons.CHESS_BOARD_WIDTH; j++)
                {
                    // Chỉ xét những ô chưa đánh
                    if (chessBoard.Matrix[i][j].BackgroundImage == null) 
                    {
                        long attack = CalculateAttackScore(i, j);
                        long defense = CalculateDefenseScore(i, j);
                        long totalScore = attack + defense;

                        if (totalScore > maxScore)
                        {
                            maxScore = totalScore;
                            bestPosition = new Point(j, i);
                        }
                    }
                }
            }
            
            return bestPosition;
        }

        // Tính điểm tấn công tại vị trí (row, col)
        private long CalculateAttackScore(int row, int col)
        {
            long totalScore = 0;
            // Image của máy (thường là Player 1 - X)
            // Giả định: Người chơi thật là Player 0 (O), Máy là Player 1 (X).
            Image aiMark = chessBoard.Player[1].Mark;
            Image playerMark = chessBoard.Player[0].Mark;

            // Duyệt 4 hướng: Ngang, Dọc, Chéo Chính, Chéo Phụ
            int[] dRow = { 0, 1, 1, 1 };
            int[] dCol = { 1, 0, 1, -1 };

            for (int dir = 0; dir < 4; dir++)
            {
                long score = 0;
                int allyCount = 0;
                int enemyCount = 0;

                // Duyệt về 2 phía của hướng hiện tại
                for (int k = 1; k < 6; k++) // Duyệt tối đa 5 ô
                {
                    int r = row + dRow[dir] * k;
                    int c = col + dCol[dir] * k;
                    if (IsSafe(r, c))
                    {
                        Image cellImage = chessBoard.Matrix[r][c].BackgroundImage;
                        if (cellImage == aiMark)
                            allyCount++;
                        else if (cellImage == playerMark)
                        {
                            enemyCount++;
                            break; // Gặp quân địch thì dừng
                        }
                        else
                            break; // Gặp ô trống thì dừng (để tính chuỗi liên tục)
                    }
                    else break;
                }

                for (int k = 1; k < 6; k++)
                {
                    int r = row - dRow[dir] * k;
                    int c = col - dCol[dir] * k;
                    if (IsSafe(r, c))
                    {
                        Image cellImage = chessBoard.Matrix[r][c].BackgroundImage;
                        if (cellImage == aiMark)
                            allyCount++;
                        else if (cellImage == playerMark)
                        {
                            enemyCount++;
                            break;
                        }
                        else
                            break;
                    }
                    else break;
                }

                // Bị chặn 2 đầu thì giảm điểm hoặc không tính
                if (enemyCount == 2)
                    score = 0;
                else
                    score = AttackScore[allyCount];

                totalScore += score;
            }

            return totalScore;
        }

        // Tính điểm phòng thủ tại vị trí (row, col)
        private long CalculateDefenseScore(int row, int col)
        {
            long totalScore = 0;
            Image aiMark = chessBoard.Player[1].Mark;
            Image playerMark = chessBoard.Player[0].Mark;

            int[] dRow = { 0, 1, 1, 1 };
            int[] dCol = { 1, 0, 1, -1 };

            for (int dir = 0; dir < 4; dir++)
            {
                long score = 0;
                int enemyCount = 0;
                int allyCount = 0;

                for (int k = 1; k < 6; k++)
                {
                    int r = row + dRow[dir] * k;
                    int c = col + dCol[dir] * k;
                    if (IsSafe(r, c))
                    {
                        Image cellImage = chessBoard.Matrix[r][c].BackgroundImage;
                        if (cellImage == playerMark)
                            enemyCount++;
                        else if (cellImage == aiMark)
                        {
                            allyCount++;
                            break;
                        }
                        else
                            break;
                    }
                    else break;
                }

                for (int k = 1; k < 6; k++)
                {
                    int r = row - dRow[dir] * k;
                    int c = col - dCol[dir] * k;
                    if (IsSafe(r, c))
                    {
                        Image cellImage = chessBoard.Matrix[r][c].BackgroundImage;
                        if (cellImage == playerMark)
                            enemyCount++;
                        else if (cellImage == aiMark)
                        {
                            allyCount++;
                            break;
                        }
                        else
                            break;
                    }
                    else break;
                }

                if (allyCount == 2)
                    score = 0;
                else
                    score = DefenseScore[enemyCount];

                totalScore += score;
            }

            return totalScore;
        }

        private bool IsSafe(int row, int col)
        {
            return row >= 0 && row < Cons.CHESS_BOARD_HEIGHT && col >= 0 && col < Cons.CHESS_BOARD_WIDTH;
        }
    }
}
