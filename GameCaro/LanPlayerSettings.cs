using System;
using System.IO;

namespace GameCaro
{
    public class LanPlayerSettings
    {
        private static string SettingsFilePath
        {
            get { return Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "player_settings_LAN.txt"); }
        }

        public string PlayerName { get; set; }
        public string AvatarPath { get; set; }

        public LanPlayerSettings()
        {
            PlayerName = "LAN Player";
            AvatarPath = "";
        }

        public void Save()
        {
            try
            {
                using (StreamWriter writer = new StreamWriter(SettingsFilePath))
                {
                    writer.WriteLine("PlayerName=" + PlayerName);
                    writer.WriteLine("AvatarPath=" + AvatarPath);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Lỗi! không thể lưu thông tin LAN: " + ex.Message);
            }
        }

        public static LanPlayerSettings Load()
        {
            LanPlayerSettings settings = new LanPlayerSettings();

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
                                case "PlayerName":
                                    settings.PlayerName = value;
                                    break;
                                case "AvatarPath":
                                    settings.AvatarPath = value;
                                    break;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Lỗi! không thể đọc thông tin người chơi LAN: " + ex.Message);
            }

            return settings;
        }
    }
}
