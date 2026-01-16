using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace GameCaro
{
    /// <summary>
    /// Lớp chứa các cấu hình màu sắc cho giao diện hiện đại - LIGHT MODE
    /// </summary>
    public static class ModernColors
    {
        // Background colors - Light Mode
        public static Color DarkBackground = Color.FromArgb(245, 245, 245);  // Xám nhạt
        public static Color CardBackground = Color.FromArgb(255, 255, 255);  // Trắng
        public static Color CardBackgroundLight = Color.FromArgb(250, 250, 250);
        
        // Primary colors (Xanh dương)
        public static Color Primary = Color.FromArgb(33, 150, 243);
        public static Color PrimaryDark = Color.FromArgb(25, 118, 210);
        public static Color PrimaryLight = Color.FromArgb(100, 181, 246);
        
        // Accent colors
        public static Color Accent = Color.FromArgb(255, 64, 129);
        public static Color AccentLight = Color.FromArgb(255, 100, 150);
        
        // Chess colors
        public static Color PlayerO = Color.FromArgb(76, 175, 80);      // Xanh lá - Player O
        public static Color PlayerOLight = Color.FromArgb(129, 199, 132);
        public static Color PlayerX = Color.FromArgb(244, 67, 54);       // Đỏ - Player X
        public static Color PlayerXLight = Color.FromArgb(239, 154, 154);
        
        // Text colors - Light Mode
        public static Color TextPrimary = Color.FromArgb(33, 33, 33);     // Đen
        public static Color TextSecondary = Color.FromArgb(100, 100, 100);
        public static Color TextDisabled = Color.FromArgb(180, 180, 180);
        
        // Board colors - Light Mode
        public static Color BoardBackground = Color.FromArgb(240, 240, 240);  // Xám nhạt
        public static Color BoardCell = Color.FromArgb(255, 255, 255);        // Trắng
        public static Color BoardCellHover = Color.FromArgb(230, 240, 255);   // Xanh nhạt khi hover
        public static Color BoardCellBorder = Color.FromArgb(200, 200, 200);  // Xám border
        
        // Success/Warning
        public static Color Success = Color.FromArgb(76, 175, 80);
        public static Color Warning = Color.FromArgb(255, 193, 7);
        public static Color Error = Color.FromArgb(244, 67, 54);
    }

    /// <summary>
    /// Button hiện đại với hiệu ứng hover và animation
    /// </summary>
    public class ModernButton : Button
    {
        private Color _normalColor = ModernColors.Primary;
        private Color _hoverColor = ModernColors.PrimaryLight;
        private Color _pressColor = ModernColors.PrimaryDark;
        private bool _isHovering = false;
        private bool _isPressed = false;
        private int _cornerRadius = 8;

        public Color NormalColor
        {
            get => _normalColor;
            set { _normalColor = value; Invalidate(); }
        }

        public Color HoverColor
        {
            get => _hoverColor;
            set { _hoverColor = value; Invalidate(); }
        }

        public int CornerRadius
        {
            get => _cornerRadius;
            set { _cornerRadius = value; Invalidate(); }
        }

        public ModernButton()
        {
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            BackColor = Color.Transparent;
            ForeColor = ModernColors.TextPrimary;
            Font = new Font("Segoe UI", 10, FontStyle.Bold);
            Cursor = Cursors.Hand;
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            
            Color currentColor = _isPressed ? _pressColor : (_isHovering ? _hoverColor : _normalColor);
            
            using (GraphicsPath path = CreateRoundedRectangle(new Rectangle(0, 0, Width - 1, Height - 1), _cornerRadius))
            using (SolidBrush brush = new SolidBrush(currentColor))
            {
                e.Graphics.FillPath(brush, path);
            }

            // Draw text
            TextRenderer.DrawText(e.Graphics, Text, Font, ClientRectangle, ForeColor, 
                TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
        }

        private GraphicsPath CreateRoundedRectangle(Rectangle rect, int radius)
        {
            GraphicsPath path = new GraphicsPath();
            int diameter = radius * 2;
            
            path.AddArc(rect.X, rect.Y, diameter, diameter, 180, 90);
            path.AddArc(rect.Right - diameter, rect.Y, diameter, diameter, 270, 90);
            path.AddArc(rect.Right - diameter, rect.Bottom - diameter, diameter, diameter, 0, 90);
            path.AddArc(rect.X, rect.Bottom - diameter, diameter, diameter, 90, 90);
            path.CloseFigure();
            
            return path;
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            _isHovering = true;
            Invalidate();
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            _isHovering = false;
            Invalidate();
            base.OnMouseLeave(e);
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            _isPressed = true;
            Invalidate();
            base.OnMouseDown(e);
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            _isPressed = false;
            Invalidate();
            base.OnMouseUp(e);
        }
    }

    /// <summary>
    /// Ô cờ với hiệu ứng đẹp và animation
    /// </summary>
    public class ChessButton : Button
    {
        private bool _isHovering = false;
        private int _animationStep = 0;
        private Timer _animationTimer;
        private bool _isAnimating = false;
        private int _playerMark = -1; // -1: empty, 0: O, 1: X
        private float _markScale = 0f;

        public int PlayerMark
        {
            get => _playerMark;
            set
            {
                if (_playerMark != value)
                {
                    _playerMark = value;
                    if (value >= 0)
                    {
                        StartMarkAnimation();
                    }
                    else
                    {
                        _markScale = 0f;
                        Invalidate();
                    }
                }
            }
        }

        public ChessButton()
        {
            FlatStyle = FlatStyle.Flat;
            FlatAppearance.BorderSize = 0;
            BackColor = ModernColors.BoardCell;
            Size = new Size(32, 32);  // Default size, sẽ được set lại trong DrawChessBoard
            Cursor = Cursors.Hand;
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);

            _animationTimer = new Timer();
            _animationTimer.Interval = 16; // ~60 FPS
            _animationTimer.Tick += AnimationTimer_Tick;
        }

        private void StartMarkAnimation()
        {
            _markScale = 0f;
            _isAnimating = true;
            _animationTimer.Start();
        }

        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            if (_isAnimating)
            {
                _markScale += 0.4f;  // Tăng tốc animation (0.15 -> 0.4)
                if (_markScale >= 1f)
                {
                    _markScale = 1f;
                    _isAnimating = false;
                    _animationTimer.Stop();
                }
                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            // Draw background - sử dụng BackColor để hỗ trợ hiệu ứng disabled
            Color bgColor;
            if (!Enabled)
            {
                bgColor = BackColor;  // Màu xám khi disabled
            }
            else
            {
                bgColor = _isHovering && _playerMark < 0 ? ModernColors.BoardCellHover : ModernColors.BoardCell;
            }
            using (SolidBrush brush = new SolidBrush(bgColor))
            {
                e.Graphics.FillRectangle(brush, 0, 0, Width, Height);
            }

            // Draw border
            using (Pen pen = new Pen(ModernColors.BoardCellBorder, 1))
            {
                e.Graphics.DrawRectangle(pen, 0, 0, Width - 1, Height - 1);
            }

            // Draw mark with animation
            if (_playerMark >= 0 && _markScale > 0)
            {
                int centerX = Width / 2;
                int centerY = Height / 2;
                int maxSize = Math.Min(Width, Height) - 4;  // Giảm padding để O/X to hơn
                int currentSize = (int)(maxSize * _markScale);
                int offset = currentSize / 2;

                if (_playerMark == 0) // O - Xanh lá
                {
                    DrawO(e.Graphics, centerX, centerY, currentSize);
                }
                else // X - Đỏ
                {
                    DrawX(e.Graphics, centerX, centerY, currentSize);
                }
            }
        }

        private void DrawO(Graphics g, int cx, int cy, int size)
        {
            int thickness = Math.Max(3, size / 6);
            int radius = size / 2 - thickness;
            
            using (Pen pen = new Pen(ModernColors.PlayerO, thickness))
            {
                pen.StartCap = LineCap.Round;
                pen.EndCap = LineCap.Round;
                g.DrawEllipse(pen, cx - radius, cy - radius, radius * 2, radius * 2);
            }
            
            // Glow effect
            using (Pen glowPen = new Pen(Color.FromArgb(50, ModernColors.PlayerOLight), thickness + 4))
            {
                g.DrawEllipse(glowPen, cx - radius, cy - radius, radius * 2, radius * 2);
            }
        }

        private void DrawX(Graphics g, int cx, int cy, int size)
        {
            int thickness = Math.Max(3, size / 6);
            int halfSize = size / 2 - 2;

            using (Pen pen = new Pen(ModernColors.PlayerX, thickness))
            {
                pen.StartCap = LineCap.Round;
                pen.EndCap = LineCap.Round;
                g.DrawLine(pen, cx - halfSize, cy - halfSize, cx + halfSize, cy + halfSize);
                g.DrawLine(pen, cx + halfSize, cy - halfSize, cx - halfSize, cy + halfSize);
            }
            
            // Glow effect
            using (Pen glowPen = new Pen(Color.FromArgb(50, ModernColors.PlayerXLight), thickness + 4))
            {
                g.DrawLine(glowPen, cx - halfSize, cy - halfSize, cx + halfSize, cy + halfSize);
                g.DrawLine(glowPen, cx + halfSize, cy - halfSize, cx - halfSize, cy + halfSize);
            }
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            _isHovering = true;
            Invalidate();
            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            _isHovering = false;
            Invalidate();
            base.OnMouseLeave(e);
        }

        public void ResetMark()
        {
            _playerMark = -1;
            _markScale = 0f;
            _isAnimating = false;
            _animationTimer.Stop();
            BackgroundImage = null;
            Invalidate();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _animationTimer?.Dispose();
            }
            base.Dispose(disposing);
        }
    }

    /// <summary>
    /// Panel với nền gradient đẹp
    /// </summary>
    public class GradientPanel : Panel
    {
        private Color _startColor = ModernColors.DarkBackground;
        private Color _endColor = ModernColors.CardBackground;
        private LinearGradientMode _gradientMode = LinearGradientMode.Vertical;

        public Color StartColor
        {
            get => _startColor;
            set { _startColor = value; Invalidate(); }
        }

        public Color EndColor
        {
            get => _endColor;
            set { _endColor = value; Invalidate(); }
        }

        public LinearGradientMode GradientMode
        {
            get => _gradientMode;
            set { _gradientMode = value; Invalidate(); }
        }

        public GradientPanel()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (Width > 0 && Height > 0)
            {
                using (LinearGradientBrush brush = new LinearGradientBrush(
                    ClientRectangle, _startColor, _endColor, _gradientMode))
                {
                    e.Graphics.FillRectangle(brush, ClientRectangle);
                }
            }
            base.OnPaint(e);
        }
    }

    /// <summary>
    /// TextBox hiện đại với border đẹp
    /// </summary>
    public class ModernTextBox : TextBox
    {
        private Color _borderColor = ModernColors.Primary;
        private int _borderThickness = 2;

        public ModernTextBox()
        {
            BorderStyle = BorderStyle.None;
            BackColor = ModernColors.CardBackground;
            ForeColor = ModernColors.TextPrimary;
            Font = new Font("Segoe UI", 11);
            Padding = new Padding(10);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
        }
    }

    /// <summary>
    /// ProgressBar hiện đại với gradient
    /// </summary>
    public class ModernProgressBar : ProgressBar
    {
        private Color _progressColor = ModernColors.Primary;
        private Color _backgroundColor = ModernColors.CardBackground;

        public Color ProgressColor
        {
            get => _progressColor;
            set { _progressColor = value; Invalidate(); }
        }

        public ModernProgressBar()
        {
            SetStyle(ControlStyles.UserPaint | ControlStyles.AllPaintingInWmPaint | ControlStyles.DoubleBuffer, true);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;

            // Background
            using (SolidBrush bgBrush = new SolidBrush(_backgroundColor))
            {
                e.Graphics.FillRectangle(bgBrush, 0, 0, Width, Height);
            }

            // Progress
            float progressWidth = (float)Value / Maximum * Width;
            if (progressWidth > 0)
            {
                using (LinearGradientBrush progressBrush = new LinearGradientBrush(
                    new Rectangle(0, 0, (int)progressWidth + 1, Height),
                    _progressColor,
                    Color.FromArgb(_progressColor.R / 2, _progressColor.G / 2, _progressColor.B / 2),
                    LinearGradientMode.Vertical))
                {
                    e.Graphics.FillRectangle(progressBrush, 0, 0, progressWidth, Height);
                }
            }

            // Border
            using (Pen borderPen = new Pen(ModernColors.BoardCellBorder, 1))
            {
                e.Graphics.DrawRectangle(borderPen, 0, 0, Width - 1, Height - 1);
            }
        }
    }

    /// <summary>
    /// Label với font hiện đại
    /// </summary>
    public class ModernLabel : Label
    {
        public ModernLabel()
        {
            ForeColor = ModernColors.TextPrimary;
            Font = new Font("Segoe UI", 12, FontStyle.Bold);
            BackColor = Color.Transparent;
        }
    }

    /// <summary>
    /// Hiệu ứng animation helper
    /// </summary>
    public static class AnimationHelper
    {
        public static async Task FadeIn(Control control, int duration = 300)
        {
            control.Visible = true;
            for (double i = 0; i <= 1; i += 0.1)
            {
                await Task.Delay(duration / 10);
                // Note: WinForms doesn't support true opacity for controls
                // This is a simplified version
            }
        }

        public static async Task PulseEffect(Control control, int pulseCount = 2)
        {
            var originalSize = control.Size;
            var originalLocation = control.Location;

            for (int i = 0; i < pulseCount; i++)
            {
                // Scale up
                control.Size = new Size((int)(originalSize.Width * 1.1), (int)(originalSize.Height * 1.1));
                control.Location = new Point(
                    originalLocation.X - (control.Size.Width - originalSize.Width) / 2,
                    originalLocation.Y - (control.Size.Height - originalSize.Height) / 2);
                await Task.Delay(100);

                // Scale back
                control.Size = originalSize;
                control.Location = originalLocation;
                await Task.Delay(100);
            }
        }
    }

    /// <summary>
    /// Custom MenuStrip Renderer với phong cách hiện đại
    /// </summary>
    public class ModernMenuRenderer : ToolStripProfessionalRenderer
    {
        public ModernMenuRenderer() : base(new ModernColorTable()) { }

        protected override void OnRenderMenuItemBackground(ToolStripItemRenderEventArgs e)
        {
            Rectangle rc = new Rectangle(Point.Empty, e.Item.Size);
            Color bgColor = e.Item.Selected ? ModernColors.CardBackgroundLight : ModernColors.CardBackground;
            
            using (SolidBrush brush = new SolidBrush(bgColor))
            {
                e.Graphics.FillRectangle(brush, rc);
            }
        }

        protected override void OnRenderItemText(ToolStripItemTextRenderEventArgs e)
        {
            e.TextColor = ModernColors.TextPrimary;
            base.OnRenderItemText(e);
        }

        protected override void OnRenderToolStripBackground(ToolStripRenderEventArgs e)
        {
            using (SolidBrush brush = new SolidBrush(ModernColors.CardBackground))
            {
                e.Graphics.FillRectangle(brush, e.AffectedBounds);
            }
        }

        protected override void OnRenderToolStripBorder(ToolStripRenderEventArgs e)
        {
            // Không vẽ border
        }

        protected override void OnRenderSeparator(ToolStripSeparatorRenderEventArgs e)
        {
            using (Pen pen = new Pen(ModernColors.BoardCellBorder))
            {
                e.Graphics.DrawLine(pen, 0, e.Item.Height / 2, e.Item.Width, e.Item.Height / 2);
            }
        }

        protected override void OnRenderArrow(ToolStripArrowRenderEventArgs e)
        {
            e.ArrowColor = ModernColors.TextPrimary;
            base.OnRenderArrow(e);
        }
    }

    /// <summary>
    /// Custom Color Table cho MenuStrip
    /// </summary>
    public class ModernColorTable : ProfessionalColorTable
    {
        public override Color MenuItemSelected => ModernColors.CardBackgroundLight;
        public override Color MenuItemSelectedGradientBegin => ModernColors.CardBackgroundLight;
        public override Color MenuItemSelectedGradientEnd => ModernColors.CardBackgroundLight;
        public override Color MenuItemBorder => ModernColors.Primary;
        public override Color MenuBorder => ModernColors.BoardCellBorder;
        
        public override Color ToolStripDropDownBackground => ModernColors.CardBackground;
        public override Color ImageMarginGradientBegin => ModernColors.CardBackground;
        public override Color ImageMarginGradientMiddle => ModernColors.CardBackground;
        public override Color ImageMarginGradientEnd => ModernColors.CardBackground;
        
        public override Color SeparatorDark => ModernColors.BoardCellBorder;
        public override Color SeparatorLight => ModernColors.BoardCellBorder;
    }
}
