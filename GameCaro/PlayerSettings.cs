using System;
using System.Drawing;
using System.IO;

namespace GameCaro
{
    /// <summary>
    /// Lưu trữ và quản lý cài đặt người chơi (tên, avatar)
    /// </summary>
    public class PlayerSettings
    {
        private static string SettingsFilePath => 
            Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "player_settings.txt");

        public string Player1Name { get; set; } = "Player O";
        public string Player2Name { get; set; } = "Player X";
        public string Player1AvatarPath { get; set; } = "";
        public string Player2AvatarPath { get; set; } = "";

        /// <summary>
        /// Lưu cài đặt vào file text
        /// Format: Key=Value, mỗi dòng 1 cặp
        /// </summary>
        public void Save()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(SettingsFilePath))
                {
                    writer.WriteLine($"Player1Name={Player1Name}");
                    writer.WriteLine($"Player2Name={Player2Name}");
                    writer.WriteLine($"Player1AvatarPath={Player1AvatarPath}");
                    writer.WriteLine($"Player2AvatarPath={Player2AvatarPath}");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi! không thể lưu thông tin người chơi: {ex.Message}");
            }
        }

        /// <summary>
        /// Load cài đặt từ file text
        /// </summary>
        public static PlayerSettings Load()
        {
            PlayerSettings settings = new PlayerSettings();
            try
            {
                if (File.Exists(SettingsFilePath))
                {
                    string[] lines = File.ReadAllLines(SettingsFilePath);
                    foreach (string line in lines)
                    {
                        int separatorIndex = line.IndexOf('=');
                        if (separatorIndex > 0)
                        {
                            string key = line.Substring(0, separatorIndex);
                            string value = line.Substring(separatorIndex + 1);

                            switch (key)
                            {
                                case "Player1Name":
                                    settings.Player1Name = value;
                                    break;
                                case "Player2Name":
                                    settings.Player2Name = value;
                                    break;
                                case "Player1AvatarPath":
                                    settings.Player1AvatarPath = value;
                                    break;
                                case "Player2AvatarPath":
                                    settings.Player2AvatarPath = value;
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi! không thể lưu thông tin người chơi: {ex.Message}");
            }
            return settings;
        }

        /// <summary>
        /// Lấy avatar image từ đường dẫn, trả về null nếu không tìm thấy
        /// </summary>
        public static Image LoadAvatarImage(string avatarPath)
        {
            try
            {
                if (!string.IsNullOrEmpty(avatarPath) && File.Exists(avatarPath))
                {
                    using (var stream = new FileStream(avatarPath, FileMode.Open, FileAccess.Read))
                    {
                        return Image.FromStream(stream);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi! không lưu được avatar: {ex.Message}");
            }
            return null;
        }

        /// <summary>
        /// Lưu avatar vào thư mục Resources và trả về đường dẫn mới
        /// </summary>
        public static string SaveAvatarToResources(string sourcePath, int playerIndex)
        {
            try
            {
                string resourcesDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources");
                if (!Directory.Exists(resourcesDir))
                {
                    Directory.CreateDirectory(resourcesDir);
                }

                string extension = Path.GetExtension(sourcePath);
                string avatarFileName = $"Avatar_P{playerIndex + 1}{extension}";
                string destPath = Path.Combine(resourcesDir, avatarFileName);

                File.Copy(sourcePath, destPath, true);

                return destPath;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Lỗi! không lưu được avatar: {ex.Message}");
                return null;
            }
        }
    }
}
