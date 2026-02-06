# GameCaro

---

## Mục Lục

1. [Giới Thiệu Đề Tài](#1-giới-thiệu-đề-tài)
2. [Kiến Trúc Hệ Thống](#2-kiến-trúc-hệ-thống)
3. [Hướng Dẫn Chơi](#3-hướng-dẫn-chơi-chế-độ-lan)
4. [Thông Tin Tác Giả](#4-thông-tin-tác-giả)

---

## 1. Giới Thiệu Đề Tài

### Lý Do Chọn Đề Tài

Rèn luyện kỹ năng **lập trình mạng (Socket)** và **Windows Forms** tương tác realtime.

### Bài Toán Đặt Ra

- Kết nối hai người chơi qua mạng LAN không độ trễ.
- Đồng bộ dữ liệu game (nước đi, chat, kết quả) tức thời.
- Tối ưu hiệu năng đồ họa (GDI+).

### Công Nghệ Sử Dụng

![C#](https://img.shields.io/badge/Language-C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
![.NET](https://img.shields.io/badge/Framework-.NET%204.8-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)
![Windows Forms](https://img.shields.io/badge/UI-Windows%20Forms-0078D7?style=for-the-badge&logo=windows&logoColor=white)
![SunnyUI](https://img.shields.io/badge/Library-SunnyUI%20v3.9.2-orange?style=for-the-badge)
![Socket](https://img.shields.io/badge/Network-TCP%2FIP%20Sockets-blue?style=for-the-badge&logo=socket.io&logoColor=white)

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
  - **Undo/Redo**: Đi lại nước đi vừa rồi.
  - **Đếm Ngược**: Thêm chút áp lực thời gian cho kịch tính.

---

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

---

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

## 4. Thông Tin Tác Giả

Dự án được thực hiện bởi:

- **Họ tên**: Võ Anh Khải
- **Email**: khaivo300605@gmail.com
- **Website**: [gamecaro-uth.vercel.app](https://gamecaro-uth.vercel.app/)
