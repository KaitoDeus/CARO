using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameCaro
{
    /// <summary>
    /// Lớp đại diện cho dữ liệu được truyền qua Socket khi chơi LAN.
    /// Được serialize thành byte[] để gửi qua mạng.
    /// </summary>
    [Serializable]
    public class SocketData
    {
        #region Properties
        private int command;
        public int Command
        {
            get => command;
            set => command = value;
        }

        private Point point;
        public Point Point
        {
            get => point;
            set => point = value;
        }

        private string message;
        public string Message
        {
            get => message;
            set => message = value;
        }

        #endregion

        #region Constructor

        /// <summary>
        /// Khởi tạo SocketData với đầy đủ thông tin.
        /// </summary>
        /// <param name="command">Mã lệnh (từ SocketCommand enum)</param>
        /// <param name="message">Nội dung tin nhắn</param>
        /// <param name="point">Vị trí trên bàn cờ</param>
        public SocketData(int command, string message, Point point)
        {
            this.Command = command;
            this.Point = point;
            this.Message = message;
        }

        #endregion

        #region Enums

        /// <summary>
        /// Enum định nghĩa các loại lệnh có thể gửi qua Socket.
        /// </summary>
        public enum SocketCommand
        {
            SEND_POINT,
            NOTIFY,
            NEW_GAME,
            UNDO,
            END_GAME,
            TIME_OUT,
            QUIT,
            CHAT_MESSAGE
        }

        #endregion
    }
}
