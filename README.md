# Game Caro

Một ứng dụng game Caro (Tic tac toe) cổ điển được xây dựng bằng **C# Windows Forms**. Dự án hỗ trợ chơi offline hai người trên cùng một máy và chơi online qua mạng LAN với tính năng trò chuyện tích hợp.

## Tính Năng

### Chế độ chơi

1.  **Local Multiplayer (2 người / máy):**
    - Hai người chơi thay phiên nhau đánh trên cùng một máy tính.
    - Hỗ trợ tính năng **Undo** (Đi lại) và **Redo** (Khôi phục nước đi).
2.  **LAN Multiplayer (Chơi qua mạng LAN):**
    - Kết nối hai máy tính trong cùng mạng LAN.
    - Cơ chế tự động: Người đầu tiên kết nối sẽ là Server (Host), người sau sẽ kết nối vào như Client.
    - Đồng bộ hóa bàn cờ và lượt đi theo thời gian thực.

### Tiện ích & Giao tiếp

- **Hệ thống Chat:** Nhắn tin trò chuyện trực tiếp giữa hai người chơi trong chế độ LAN.
- **Tùy biến cá nhân:**
  - Đổi tên người chơi.
  - Thay đổi Avatar (ảnh đại diện) tùy ý từ máy tính.
  - Tự động lưu thông tin người chơi cho các lần sau.
- **Luật chơi:**
  - Bên nào đạt **5 quân liên tiếp** (ngang, dọc, chéo) sẽ thắng.
  - Có thanh đếm ngược thời gian cho mỗi lượt đi (Cooldown).

## Công Nghệ Sử Dụng

- **Ngôn ngữ:** C#
- **Framework:** .NET Framework 4.7.2
- **Giao diện:** Windows Forms (WinForms)
- **Mạng (Networking):** TCP/IP Sockets (`System.Net.Sockets`) để truyền tải dữ liệu nước đi và tin nhắn.
- **Serialization:** BinaryFormatter để đóng gói dữ liệu truyền qua mạng.

## Cài đặt và Chạy ứng dụng

### Yêu cầu

- Visual Studio (2019, 2022 hoặc mới hơn).
- .NET Framework 4.7.2 SDK.

### Các bước thực hiện

1.  **Clone dự án** về máy:
    ```bash
    git clone https://github.com/your-username/GameCaro.git
    ```
2.  Mở file giải pháp `GameCaro.sln` bằng Visual Studio.
3.  Nhấn **F5** hoặc chọn **Start** để Build và chạy chương trình.

## Hướng dẫn sử dụng

### 1. Chơi Local (Mặc định)

- Mở ứng dụng, bàn cờ sẽ sẵn sàng ngay lập tức.
- Người chơi 1 đánh **O**, Người chơi 2 đánh **X**.
- Sử dụng menu hoặc phím tắt (`Ctrl+Z`, `Ctrl+Y`) để Undo/Redo.

### 2. Chơi qua LAN

1.  Cả hai người chơi chọn chế độ **LAN** trong hộp chọn chế độ (bên phải).
2.  **Máy Host (Người tạo phòng):**
    - Nhấn nút **LAN** (hoặc đợi đối phương kết nối).
    - Game sẽ tự động nhận diện IP nội bộ và lắng nghe kết nối.
3.  **Máy Client (Người tham gia):**
    - Nhập địa chỉ **IP** của máy Host vào ô IP.
    - Nhấn nút **LAN** để kết nối.
4.  Khi kết nối thành công, nút trạng thái sẽ hiện "Đã kết nối" và hai bên có thể bắt đầu chat và chơi.

## Cấu trúc dự án

```text
GameCaro/
├── GameCaro.sln              # File Solution của Visual Studio
├── README.md                 # Tài liệu hướng dẫn
└── GameCaro/                 # Thư mục mã nguồn chính (Project)
    ├── Program.cs            # Điểm khởi chạy ứng dụng
    ├── GameCaro.cs           # Form chính (Giao diện & Logic game)
    ├── ChessBoardManager.cs  # Logic bàn cờ & kiểm tra thắng thua
    ├── SocketManager.cs      # Quản lý kết nối mạng (LAN)
    ├── SocketData.cs         # Cấu trúc dữ liệu gói tin mạng
    ├── PlayerSettings.cs     # Quản lý cài đặt người chơi
    ├── Cons.cs               # Các hằng số cấu hình game
    ├── GameMode.cs           # Enum định nghĩa chế độ chơi
    ├── Player.cs             # Lớp đối tượng Người chơi
    ├── PlayInfo.cs           # Thông tin lượt chơi hiện tại
    └── Resources/            # Tài nguyên hình ảnh, icon
```
