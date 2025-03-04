using System;
using System.IO;
using System.Text.Json;
using System.Windows;

namespace ByteCompare
{
    public class Settings
    {
        private static readonly string SettingsFilePath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
            "ByteCompare",
            "settings.json"
        );

        public double WindowWidth { get; set; } = 800;
        public double WindowHeight { get; set; } = 600;
        public double WindowLeft { get; set; } = double.NaN;
        public double WindowTop { get; set; } = double.NaN;
        public WindowState WindowState { get; set; } = WindowState.Normal;
        public bool IsBigEndian { get; set; } = false;

        public static Settings Load()
        {
            try
            {
                if (File.Exists(SettingsFilePath))
                {
                    string json = File.ReadAllText(SettingsFilePath);
                    return JsonSerializer.Deserialize<Settings>(json) ?? new Settings();
                }
            }
            catch (Exception)
            {
                // 如果加载失败，返回默认设置
            }
            return new Settings();
        }

        public void Save()
        {
            try
            {
                string directoryPath = Path.GetDirectoryName(SettingsFilePath);
                if (!Directory.Exists(directoryPath))
                {
                    Directory.CreateDirectory(directoryPath);
                }

                string json = JsonSerializer.Serialize(this, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText(SettingsFilePath, json);
            }
            catch (Exception)
            {
                // 保存失败时不抛出异常
            }
        }
    }
}