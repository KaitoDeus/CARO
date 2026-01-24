using System;
using System.Drawing;
using System.IO;

namespace GameCaro
{
    /// <summary>
    /// Lớp quản lý việc lưu trữ và tải cài đặt người chơi.
    /// Bao gồm: tên người chơi, đường dẫn avatar.
    /// Dữ liệu được lưu vào file text "player_settings.txt".
    /// </summary>
    public class PlayerSettings
    {
        #region Properties
        private static string SettingsFilePath
        {
            get { return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "player_settings.txt"); }
        }

        public string Player1Name { get; set; }
        public string Player2Name { get; set; }

        public string Player1AvatarPath { get; set; }
        public string Player2AvatarPath { get; set; }

        public PlayerSettings()
        {
            Player1Name = "Player O";
            Player2Name = "Player X";
            Player1AvatarPath = "";
            Player2AvatarPath = "";
        }

        #endregion

        #region Save/Load Methods

        /// <summary>
        /// Lưu cài đặt người chơi vào file text.
        /// Format mỗi dòng: Key=Value
        /// </summary>
        public void Save()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(SettingsFilePath))
                {
                    writer.WriteLine("Player1Name=" + Player1Name);
                    writer.WriteLine("Player2Name=" + Player2Name);
                    writer.WriteLine("Player1AvatarPath=" + Player1AvatarPath);
                    writer.WriteLine("Player2AvatarPath=" + Player2AvatarPath);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Lỗi! không thể lưu thông tin người chơi: " + ex.Message);
            }
        }

        /// <summary>
        /// Tải cài đặt người chơi từ file text.
        /// Nếu file không tồn tại, trả về cài đặt mặc định.
        /// </summary>
        public static PlayerSettings Load()
        {
            PlayerSettings settings = new PlayerSettings();
            
            try
            {
                // Kiểm tra file có tồn tại không
                if (File.Exists(SettingsFilePath))
                {
                    string[] lines = File.ReadAllLines(SettingsFilePath);
                    
                    // Đọc từng dòng và parse Key=Value
                    foreach (string line in lines)
                    {
                        int separatorIndex = line.IndexOf('=');
                        
                        if (separatorIndex > 0)
                        {
                            string key = line.Substring(0, separatorIndex);
                            string value = line.Substring(separatorIndex + 1);

                            // Gán giá trị vào property tương ứng
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
                System.Diagnostics.Debug.WriteLine("Lỗi! không thể đọc thông tin người chơi: " + ex.Message);
            }
            
            return settings;
        }

        #endregion

        #region Avatar Methods

        public static Image LoadAvatarImage(string avatarPath)
        {
            try
            {
                // Kiểm tra đường dẫn có hợp lệ và file có tồn tại không
                if (!string.IsNullOrEmpty(avatarPath) && File.Exists(avatarPath))
                {
                    // Mở file stream để đọc ảnh (tránh lock file)
                    using (var stream = new FileStream(avatarPath, FileMode.Open, FileAccess.Read))
                    {
                        return Image.FromStream(stream);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Lỗi! không đọc được avatar: " + ex.Message);
            }
            
            return null;
        }

        public static string SaveNamedAvatarToResources(string sourcePath, string baseName)
        {
            try
            {
                string resourcesDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources");
                if (!Directory.Exists(resourcesDir))
                {
                    Directory.CreateDirectory(resourcesDir);
                }

                // Tạo tên file mới (giữ nguyên extension)
                string extension = Path.GetExtension(sourcePath);
                string avatarFileName = baseName + extension;
                string destPath = Path.Combine(resourcesDir, avatarFileName);

                // Copy file nguồn vào Resources (ghi đè nếu đã tồn tại)
                File.Copy(sourcePath, destPath, true);

                return destPath;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Lỗi! không lưu được avatar: " + ex.Message);
                return null;
            }
        }

        public static string SaveAvatarToResources(string sourcePath, int playerIndex)
        {
            try
            {
                string resourcesDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Resources");
                if (!Directory.Exists(resourcesDir))
                {
                    Directory.CreateDirectory(resourcesDir);
                }

                // Tạo tên file mới (giữ nguyên extension)
                string extension = Path.GetExtension(sourcePath);
                string avatarFileName = "Avatar_P" + (playerIndex + 1) + extension;
                string destPath = Path.Combine(resourcesDir, avatarFileName);

                // Copy file nguồn vào Resources (ghi đè nếu đã tồn tại)
                File.Copy(sourcePath, destPath, true);

                return destPath;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Lỗi! không lưu được avatar: " + ex.Message);
                return null;
            }
        }

        #endregion
    }
}
