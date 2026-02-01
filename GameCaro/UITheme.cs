using System.Drawing;

namespace GameCaro
{
    /// <summary>
    /// Lớp quản lý màu sắc và style UI tập trung.
    /// Giúp dễ dàng thay đổi theme mà không cần sửa nhiều file.
    /// </summary>
    public static class UITheme
    {
        #region Primary Colors
        
        /// <summary>Màu chính - Xanh dương sáng</summary>
        public static Color PrimaryMain = Color.FromArgb(94, 148, 255);    // #5E94FF
        
        /// <summary>Màu chính đậm - cho hover</summary>
        public static Color PrimaryDark = Color.FromArgb(65, 125, 255);    // #417DFF
        
        /// <summary>Màu chính nhạt - cho highlight</summary>
        public static Color PrimaryLight = Color.FromArgb(230, 245, 255);  // #E6F5FF
        
        #endregion

        #region Neutral Colors
        
        /// <summary>Màu nền chính</summary>
        public static Color Background = Color.White;                          // #FFFFFF
        
        /// <summary>Màu bề mặt Card/Panel</summary>
        public static Color Surface = Color.White;                          // #FFFFFF
        
        /// <summary>Màu viền mặc định</summary>
        public static Color Border = Color.FromArgb(200, 200, 200);        // #C8C8C8
        
        /// <summary>Màu viền nhạt</summary>
        public static Color BorderLight = Color.FromArgb(220, 220, 220);   // #DCDCDC
        
        #endregion

        #region Text Colors
        
        /// <summary>Màu text chính</summary>
        public static Color TextPrimary = Color.FromArgb(50, 50, 50);      // #323232
        
        /// <summary>Màu text phụ</summary>
        public static Color TextSecondary = Color.FromArgb(120, 120, 120); // #787878
        
        /// <summary>Màu text trên nền màu</summary>
        public static Color TextOnPrimary = Color.White;
        
        #endregion

        #region Player Colors
        
        /// <summary>Màu Player O - Xanh dương</summary>
        public static Color PlayerO = Color.FromArgb(99, 179, 237);        // #63B3ED
        
        /// <summary>Màu Player X - Đỏ cam</summary>
        public static Color PlayerX = Color.FromArgb(255, 107, 107);       // #FF6B6B
        
        #endregion

        #region Button States
        
        /// <summary>Màu hover button</summary>
        public static Color ButtonHover = Color.FromArgb(235, 245, 255);   // #EBF5FF
        
        /// <summary>Màu pressed button</summary>
        public static Color ButtonPressed = Color.FromArgb(220, 235, 255); // #DCEBFF
        
        #endregion

        #region Status Colors
        
        /// <summary>Màu thành công - Xanh lá</summary>
        public static Color Success = Color.FromArgb(72, 187, 120);        // #48BB78
        
        /// <summary>Màu cảnh báo - Vàng cam</summary>
        public static Color Warning = Color.FromArgb(237, 137, 54);        // #ED8936
        
        /// <summary>Màu lỗi - Đỏ</summary>
        public static Color Error = Color.FromArgb(245, 101, 101);         // #F56565
        
        #endregion

        #region UI Dimensions
        
        /// <summary>Bán kính bo góc mặc định</summary>
        public const int BorderRadius = 10;
        
        /// <summary>Bán kính bo góc nhỏ</summary>
        public const int BorderRadiusSmall = 6;
        
        /// <summary>Bán kính bo góc lớn</summary>
        public const int BorderRadiusLarge = 15;
        
        /// <summary>Độ dày viền mặc định</summary>
        public const int BorderThickness = 1;
        
        #endregion

        #region Font Settings
        
        /// <summary>Font chính</summary>
        public static Font FontRegular = new Font("Segoe UI", 9F);
        
        /// <summary>Font đậm</summary>
        public static Font FontBold = new Font("Segoe UI Semibold", 9F);
        
        /// <summary>Font tiêu đề</summary>
        public static Font FontTitle = new Font("Segoe UI Semibold", 12F);
        
        /// <summary>Font điểm số</summary>
        public static Font FontScore = new Font("Courier New", 14.25F, FontStyle.Bold);
        
        #endregion
    }
}
