using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GameCaro
{
    /// <summary>
    /// Lớp quản lý kết nối Socket cho chế độ chơi LAN.
    /// Hỗ trợ cả 2 vai trò: Server và Client.
    /// Sử dụng TCP Socket để đảm bảo dữ liệu được truyền đúng thứ tự.
    /// </summary>
    public class SocketManager
    {
        #region Client

        Socket client;

        public bool ConnectServer()
        {
            try
            {
                // Tạo endpoint từ IP và PORT đã cấu hình
                IPEndPoint iep = new IPEndPoint(IPAddress.Parse(IP), PORT);
                
                // Tạo socket TCP
                client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                
                // Thử kết nối đến server
                client.Connect(iep);
                
                return true;
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region Server

        Socket server;
        public event EventHandler ClientConnected;

        public void CreateServer()
        {
            // Tạo endpoint để bind
            IPEndPoint iep = new IPEndPoint(IPAddress.Any, PORT);
            
            // Tạo socket TCP
            server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            // Bind socket vào địa chỉ IP và PORT
            server.Bind(iep);
            
            // Bắt đầu lắng nghe (tối đa 10 kết nối pending)
            server.Listen(10);

            // Tạo thread riêng để chờ client kết nối (tránh block UI)
            Thread acceptClient = new Thread(() =>
            {
                // Accept sẽ block cho đến khi có client kết nối
                client = server.Accept();
                
                // Kích hoạt event để thông báo đã có client
                ClientConnected?.Invoke(this, EventArgs.Empty);
            });
            
            acceptClient.IsBackground = true; // Thread sẽ tự đóng khi app đóng
            acceptClient.Start();
        }

        #endregion

        #region Shared Properties and Methods

        public string IP = "127.0.0.1";
        public int PORT = 9000;
        public const int BUFFER = 2048;
        public bool isServer = true;

        /// <summary>
        /// Gửi dữ liệu qua socket.
        /// Dữ liệu được serialize thành byte[] trước khi gửi.
        /// </summary>
        public bool Send(object data)
        {
            // Kiểm tra kết nối có sẵn sàng không
            if (client == null || !client.Connected)
                return false;

            // Serialize object thành byte array
            byte[] sendData = SerializeData(data);

            // Gửi dữ liệu qua socket
            return SendData(client, sendData);
        }

        /// <summary>
        /// Nhận dữ liệu từ socket.
        /// Dữ liệu nhận được sẽ được deserialize thành object.
        /// </summary>
        /// <returns>Object đã deserialize, null nếu không có kết nối</returns>
        public object Receive()
        {
            // Kiểm tra kết nối có sẵn sàng không
            if (client == null || !client.Connected)
                return null;

            // Tạo buffer để nhận dữ liệu
            byte[] receiveData = new byte[BUFFER];
            
            // Nhận dữ liệu từ socket
            bool isOk = ReceiveData(client, receiveData);

            // Deserialize byte array thành object
            return DeserializeData(receiveData);
        }

        // Gửi mảng byte qua socket.
        private bool SendData(Socket target, byte[] data)
        {
            return target.Send(data) == 1 ? true : false;
        }

        // Nhận mảng byte từ socket.
        private bool ReceiveData(Socket target, byte[] data)
        {
            return target.Receive(data) == 1 ? true : false;
        }

        // Serialize (nén) object thành mảng byte[].
        public byte[] SerializeData(Object o)
        {
            MemoryStream ms = new MemoryStream();
            BinaryFormatter bf1 = new BinaryFormatter(); // Sử dụng BinaryFormatter để chuyển đổi.
            bf1.Serialize(ms, o);
            return ms.ToArray();
        }

        // Deserialize (giải nén) mảng byte[] thành object.
        public object DeserializeData(byte[] theByteArray)
        {
            MemoryStream ms = new MemoryStream(theByteArray);
            BinaryFormatter bf1 = new BinaryFormatter();
            ms.Position = 0;
            return bf1.Deserialize(ms);
        }

        /// <summary>
        /// Lấy địa chỉ IPv4 của card mạng đang hoạt động.
        /// Dùng để tự động điền IP vào textbox.
        /// </summary>
        public string GetLocalIPv4(NetworkInterfaceType _type)
        {
            string output = "";
            
            // Duyệt qua tất cả network interface
            foreach (NetworkInterface item in NetworkInterface.GetAllNetworkInterfaces())
            {
                // Kiểm tra đúng loại và đang hoạt động
                if (item.NetworkInterfaceType == _type && item.OperationalStatus == OperationalStatus.Up)
                {
                    // Lấy danh sách địa chỉ IP
                    foreach (UnicastIPAddressInformation ip in item.GetIPProperties().UnicastAddresses)
                    {
                        // Chỉ lấy địa chỉ IPv4
                        if (ip.Address.AddressFamily == AddressFamily.InterNetwork)
                        {
                            output = ip.Address.ToString();
                        }
                    }
                }
            }
            
            return output;
        }

        #endregion
    }
}
