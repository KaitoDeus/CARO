# GameCaro

## 1. Giới Thiệu Đề Tài

### Lý Do Chọn Đề Tài

Rèn luyện kỹ năng **lập trình mạng (Socket)** và **Windows Forms** tương tác realtime.

### Bài Toán Đặt Ra

- Kết nối hai người chơi qua mạng LAN không độ trễ.
- Đồng bộ dữ liệu game (nước đi, chat, kết quả) tức thời.
- Tối ưu hiệu năng đồ họa (GDI+).

### Công Nghệ Sử Dụng

- **Ngôn Ngữ**: C# .NET
- **Framework**: Windows Forms (.NET Framework 4.8)
- **UI Library**: SunnyUI v3.9.2
- **Mạng**: `System.Net.Sockets` (TCP/IP)
- **Đóng Gói Dữ Liệu**: `BinaryFormatter`

### Tính Năng Nổi Bật

- **Gameplay Cổ Điển**: Luật chơi chuẩn chỉ - ai xếp được 3 quân thẳng hàng (ngang, dọc, chéo) trước là thắng.
- **Chế Độ Chơi**:
  - **Chơi 2 Người (Local)**: Hai bạn cùng ngồi chung máy đọ sức.
  - **Chơi Qua LAN**: Kết nối mạng nội bộ để chiến với thằng bạn ngồi máy bên cạnh (đồng bộ realtime luôn nhé).
- **Hệ Thống Mạng Thông Minh**:
  - Kiến trúc Server/Client chạy ngầm xịn xò.
  - Tự động nhận diện IP máy local (không cần gõ thủ công).
  - Đồng bộ nước đi và trạng thái game siêu mượt.
- **Chatting**: Tích hợp khung chat để vừa đánh vừa "gáy" với đối thủ trong trận LAN.
- **Tùy Biến Cá Nhân**:
  - Đổi tên nhân vật thoải mái.
  - Import avatar riêng (nhìn cho ngầu).
- **Tiện Ích Khác**:
  - **Undo/Redo**: Đánh nhầm thì xin đi lại (chỉ áp dụng khi chơi Local thôi nhé 😅).
  - **Đếm Ngược**: Thêm chút áp lực thời gian cho kịch tính.

## 2. Kiến Trúc Hệ Thống

Hệ thống của em được chia làm 3 khối chính: Form (Giao diện), Board Manager (Xử lý bàn cờ), và Socket Manager.

```text
+---------------------+           +------------------------+
|      GameCaro       | <-------> |   ChessBoardManager    |
|    (Giao Diện Chính)|           |   (Luật & Vẽ Bàn Cờ)   |
+---------------------+           +------------------------+
          |
          | Sử dụng
          v
+---------------------+           +------------------------+
|    SocketManager    | --------> |       SocketData       |
|    (Kết Nối TCP)    |           |    (Gói Tin Mạng)      |
+---------------------+           +------------------------+
```

Khi chơi LAN, máy **Host** sẽ đóng vai trò Server (Cầm quân O - Player 1), còn máy **Join** sẽ là Client (Cầm quân X - Player 2). Hai bên bắn gói tin `SocketData` qua lại với nhau.

```text
 [ MÁY HOST / SERVER ]                              [ MÁY CLIENT / JOINER ]
        |                                                    |
        | (1) Tạo Server (Start)                             |
        | <------------------ (2) Kết nối -------------------|
        |                                                    |
        | ------------------ (3) Chấp nhận ----------------> |
        |                                                    |
 [ Player O Đánh ]                                           |
        |                                                    |
        | ---------------- (4) Gửi Tọa Độ -----------------> |
        |                                             [ Cập nhật bàn cờ ]
        |                                                    |
        |                                             [ Player X Đánh ]
        |                                                    |
 [ Cập nhật bàn cờ ] --------- (5) Gửi Tọa Độ -------------- |
        |                                                    |
```

## 3. Hướng Dẫn Chơi (Chế Độ LAN)

**Lưu ý: Cả hai máy phải RadminVPN**

1.  **MÁY HOST (Tạo phòng)**:
    - Chọn **Game Mode** -> **LAN**.
    - Nhập IP của máy mình (thường tự nhận diện) -> Bấm **LAN**.
    - Đợi đối thủ kết nối.

2.  **MÁY CLIENT (Vào phòng)**:
    - Chọn **Game Mode** -> **LAN**.
    - Nhập IP của Máy Host.
    - Bấm **LAN** để kết nối.

---

## 5. Thông Tin Tác Giả

Dự án được thực hiện bởi:

- **Họ tên**: Võ Anh Khải
- **Email**: kenkaneki395@gmail.com
- **Github**: [github.com/KaitoDeus](https://github.com/KaitoDeus)
