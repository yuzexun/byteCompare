using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.ComponentModel;
using Microsoft.Win32;

namespace ByteCompare
{
    public static class UIElementExtensions
    {
        public static T FindParent<T>(this DependencyObject child) where T : DependencyObject
        {
            DependencyObject parentObject = VisualTreeHelper.GetParent(child);

            if (parentObject == null) return null;

            T parent = parentObject as T;
            if (parent != null)
                return parent;
            else
                return FindParent<T>(parentObject);
        }

        public static T FindChild<T>(this DependencyObject parent) where T : DependencyObject
        {
            if (parent == null) return null;

            T foundChild = null;

            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                T childType = child as T;
                if (childType == null)
                {
                    foundChild = FindChild<T>(child);
                    if (foundChild != null) break;
                }
                else
                {
                    foundChild = (T)child;
                    break;
                }
            }
            return foundChild;
        }

        public static IEnumerable<T> FindChildren<T>(this DependencyObject parent) where T : DependencyObject
        {
            if (parent == null) yield break;

            int childrenCount = VisualTreeHelper.GetChildrenCount(parent);
            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(parent, i);
                T childType = child as T;
                if (childType != null)
                {
                    yield return childType;
                }

                foreach (var descendant in FindChildren<T>(child))
                {
                    yield return descendant;
                }
            }
        }
    }
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        private Settings _settings;

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
            Resources.Add("BooleanToBrushConverter", new BooleanToBrushConverter());
            InitializeHexViewerEvents();

            // 加载设置
            _settings = Settings.Load();
            LoadWindowSettings();
        }

        private void LoadWindowSettings()
        {
            if (!double.IsNaN(_settings.WindowLeft) && !double.IsNaN(_settings.WindowTop))
            {
                Left = _settings.WindowLeft;
                Top = _settings.WindowTop;
            }

            Width = _settings.WindowWidth;
            Height = _settings.WindowHeight;
            WindowState = _settings.WindowState;
            IsBigEndian.IsChecked = _settings.IsBigEndian;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            // 保存窗口设置
            _settings.WindowLeft = Left;
            _settings.WindowTop = Top;
            _settings.WindowWidth = Width;
            _settings.WindowHeight = Height;
            _settings.WindowState = WindowState;
            _settings.IsBigEndian = IsBigEndian.IsChecked ?? false;
            _settings.Save();

            base.OnClosing(e);
        }
        private void InitializeHexViewerEvents()
        {
            // 初始化滚动同步
            Viewer1.ScrollChanged += Viewer_ScrollChanged;
            Viewer2.ScrollChanged += Viewer_ScrollChanged;

            // 初始化位置分析面板的默认值
            File1SelectedOffset = "未选择";
            File2SelectedOffset = "未选择";
            File1ByteValue = "未选择";
            File2ByteValue = "未选择";
            File1Value16Bit = "未选择";
            File2Value16Bit = "未选择";
            File1Value32Bit = "未选择";
            File2Value32Bit = "未选择";
            File1Value64Bit = "未选择";
            File2Value64Bit = "未选择";
            File1Utf8Text = "未选择";
            File2Utf8Text = "未选择";
        }

        private string _file1SelectedOffset;
        private string _file2SelectedOffset;
        private string _file1ByteValue;
        private string _file2ByteValue;
        private string _file1Value16Bit;
        private string _file1Value32Bit;
        private string _file1Value64Bit;
        private string _file1Utf8Text;
        private string _file2Value16Bit;
        private string _file2Value32Bit;
        private string _file2Value64Bit;
        private string _file2Utf8Text;

        public string File1SelectedOffset
        {
            get { return _file1SelectedOffset; }
            set
            {
                _file1SelectedOffset = value;
                OnPropertyChanged(nameof(File1SelectedOffset));
            }
        }

        public string File2SelectedOffset
        {
            get { return _file2SelectedOffset; }
            set
            {
                _file2SelectedOffset = value;
                OnPropertyChanged(nameof(File2SelectedOffset));
            }
        }

        public string File1ByteValue
        {
            get { return _file1ByteValue; }
            set
            {
                _file1ByteValue = value;
                OnPropertyChanged(nameof(File1ByteValue));
            }
        }

        public string File2ByteValue
        {
            get { return _file2ByteValue; }
            set
            {
                _file2ByteValue = value;
                OnPropertyChanged(nameof(File2ByteValue));
            }
        }

        public string File1Value16Bit
        {
            get { return _file1Value16Bit; }
            set
            {
                _file1Value16Bit = value;
                OnPropertyChanged(nameof(File1Value16Bit));
            }
        }

        public string File1Value32Bit
        {
            get { return _file1Value32Bit; }
            set
            {
                _file1Value32Bit = value;
                OnPropertyChanged(nameof(File1Value32Bit));
            }
        }

        public string File1Value64Bit
        {
            get { return _file1Value64Bit; }
            set
            {
                _file1Value64Bit = value;
                OnPropertyChanged(nameof(File1Value64Bit));
            }
        }

        public string File1Utf8Text
        {
            get { return _file1Utf8Text; }
            set
            {
                _file1Utf8Text = value;
                OnPropertyChanged(nameof(File1Utf8Text));
            }
        }

        public string File2Value16Bit
        {
            get { return _file2Value16Bit; }
            set
            {
                _file2Value16Bit = value;
                OnPropertyChanged(nameof(File2Value16Bit));
            }
        }

        public string File2Value32Bit
        {
            get { return _file2Value32Bit; }
            set
            {
                _file2Value32Bit = value;
                OnPropertyChanged(nameof(File2Value32Bit));
            }
        }

        public string File2Value64Bit
        {
            get { return _file2Value64Bit; }
            set
            {
                _file2Value64Bit = value;
                OnPropertyChanged(nameof(File2Value64Bit));
            }
        }

        public string File2Utf8Text
        {
            get { return _file2Utf8Text; }
            set
            {
                _file2Utf8Text = value;
                OnPropertyChanged(nameof(File2Utf8Text));
            }
        }

        public class HexByte
        {
            public string Value { get; set; }
            public bool IsDifferent { get; set; }
            public int Position { get; set; }
            public byte ByteValue { get; set; }
            public HexLine Parent { get; set; }
            public int Index { get; set; }
        }

        public class HexLine
        {
            public string Offset { get; set; }
            public ObservableCollection<HexByte> HexBytes { get; set; }
            public string AsciiText { get; set; }
            public byte[] RawBytes { get; set; }
        }

        private void SelectFile1_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == true)
            {
                File1Path.Text = dialog.FileName;
            }
        }

        private void SelectFile2_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == true)
            {
                File2Path.Text = dialog.FileName;
            }
        }

        private void Viewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            var scrollViewer = sender as ScrollViewer;
            if (scrollViewer == null) return;

            var otherViewer = scrollViewer == Viewer1 ? Viewer2 : Viewer1;
            if (otherViewer == null) return;

            otherViewer.ScrollToHorizontalOffset(e.HorizontalOffset);
            otherViewer.ScrollToVerticalOffset(e.VerticalOffset);
        }

        private void HexByte_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var textBlock = sender as TextBlock;
            if (textBlock == null) return;

            var hexByte = textBlock.DataContext as HexByte;
            if (hexByte == null) return;

            var itemsControl = textBlock.FindParent<ItemsControl>();
            if (itemsControl == null) return;

            var hexLine = hexByte.Parent;
            if (hexLine == null) return;

            bool isFile1 = itemsControl.Name == "HexViewer1";

            // 更新选中位置
            if (isFile1)
            {
                File1SelectedOffset = $"0x{hexLine.Offset:X8} + {hexByte.Index}";
                File1ByteValue = $"0x{hexByte.Value:X2} ({hexByte.Value})";
                UpdateMultiByteValues(hexLine.RawBytes, hexByte.Index, true);
            }
            else
            {
                File2SelectedOffset = $"0x{hexLine.Offset:X8} + {hexByte.Index}";
                File2ByteValue = $"0x{hexByte.Value:X2} ({hexByte.Value})";
                UpdateMultiByteValues(hexLine.RawBytes, hexByte.Index, false);
            }

            // 获取另一个文件的相同位置的数据
            var otherItemsControl = isFile1 ? HexViewer2 : HexViewer1;
            var otherHexLines = otherItemsControl.ItemsSource as ObservableCollection<HexLine>;
            if (otherHexLines != null && otherHexLines.Count > 0)
            {
                var lineIndex = hexLine.Offset;
                var otherLine = otherHexLines.FirstOrDefault(l => l.Offset == lineIndex);
                if (otherLine != null && otherLine.HexBytes.Count > hexByte.Index)
                {
                    var otherByte = otherLine.HexBytes[hexByte.Index];
                    if (isFile1)
                    {
                        File2SelectedOffset = $"0x{otherLine.Offset:X8} + {otherByte.Index}";
                        File2ByteValue = $"0x{otherByte.Value:X2} ({otherByte.Value})";
                        UpdateMultiByteValues(otherLine.RawBytes, otherByte.Index, false);
                    }
                    else
                    {
                        File1SelectedOffset = $"0x{otherLine.Offset:X8} + {otherByte.Index}";
                        File1ByteValue = $"0x{otherByte.Value:X2} ({otherByte.Value})";
                        UpdateMultiByteValues(otherLine.RawBytes, otherByte.Index, true);
                    }
                }
            }
        }

        private void UpdateMultiByteValues(byte[] bytes, int startIndex, bool isFile1)
        {
            if (bytes == null || startIndex < 0) return;

            bool isBigEndian = IsBigEndian.IsChecked ?? false;

            // 更新16位值
            if (startIndex + 1 < bytes.Length)
            {
                var bytes16 = new byte[2];
                Array.Copy(bytes, startIndex, bytes16, 0, 2);
                if (!isBigEndian) Array.Reverse(bytes16);
                var value16 = BitConverter.ToUInt16(bytes16, 0);
                if (isFile1)
                    File1Value16Bit = $"0x{value16:X4} ({value16})";
                else
                    File2Value16Bit = $"0x{value16:X4} ({value16})";
            }

            // 更新32位值
            if (startIndex + 3 < bytes.Length)
            {
                var bytes32 = new byte[4];
                Array.Copy(bytes, startIndex, bytes32, 0, 4);
                if (!isBigEndian) Array.Reverse(bytes32);
                var value32 = BitConverter.ToUInt32(bytes32, 0);
                if (isFile1)
                    File1Value32Bit = $"0x{value32:X8} ({value32})";
                else
                    File2Value32Bit = $"0x{value32:X8} ({value32})";
            }

            // 更新64位值
            if (startIndex + 7 < bytes.Length)
            {
                var bytes64 = new byte[8];
                Array.Copy(bytes, startIndex, bytes64, 0, 8);
                if (!isBigEndian) Array.Reverse(bytes64);
                var value64 = BitConverter.ToUInt64(bytes64, 0);
                if (isFile1)
                    File1Value64Bit = $"0x{value64:X16} ({value64})";
                else
                    File2Value64Bit = $"0x{value64:X16} ({value64})";
            }

            // 更新UTF-8字符
            try
            {
                var utf8Bytes = new byte[Math.Min(4, bytes.Length - startIndex)];
                Array.Copy(bytes, startIndex, utf8Bytes, 0, utf8Bytes.Length);
                var utf8Text = System.Text.Encoding.UTF8.GetString(utf8Bytes);
                if (isFile1)
                    File1Utf8Text = utf8Text;
                else
                    File2Utf8Text = utf8Text;
            }
            catch
            {
                if (isFile1)
                    File1Utf8Text = "(无效UTF-8字符)";
                else
                    File2Utf8Text = "(无效UTF-8字符)";
            }
        }

        private void CompareButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(File1Path.Text) || string.IsNullOrEmpty(File2Path.Text))
            {
                MessageBox.Show("请先选择要比较的文件", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                StatusText.Text = "正在比较...";
                var hexLines1 = new ObservableCollection<HexLine>();
                var hexLines2 = new ObservableCollection<HexLine>();

                using (var stream1 = new FileStream(File1Path.Text, FileMode.Open, FileAccess.Read))
                using (var stream2 = new FileStream(File2Path.Text, FileMode.Open, FileAccess.Read))
                {
                    byte[] buffer1 = new byte[16];
                    byte[] buffer2 = new byte[16];
                    long position = 0;

                    while (true)
                    {
                        int count1 = stream1.Read(buffer1, 0, buffer1.Length);
                        int count2 = stream2.Read(buffer2, 0, buffer2.Length);

                        if (count1 == 0 && count2 == 0) break;

                        var hexLine1 = new HexLine
                        {
                            Offset = $"{position:X8}",
                            HexBytes = new ObservableCollection<HexByte>(),
                            AsciiText = ""
                        };

                        var hexLine2 = new HexLine
                        {
                            Offset = $"{position:X8}",
                            HexBytes = new ObservableCollection<HexByte>(),
                            AsciiText = ""
                        };

                        var ascii1 = new System.Text.StringBuilder();
                        var ascii2 = new System.Text.StringBuilder();

                        for (int i = 0; i < 16; i++)
                        {
                            if (i < count1)
                            {
                                hexLine1.HexBytes.Add(new HexByte
                                {
                                    Value = $"{buffer1[i]:X2}",
                                    IsDifferent = i < count2 && buffer1[i] != buffer2[i],
                                    Position = (int)position + i,
                                    ByteValue = buffer1[i],
                                    Parent = hexLine1,
                                    Index = i
                                });
                                ascii1.Append(buffer1[i] >= 32 && buffer1[i] <= 126 ? (char)buffer1[i] : '.');
                            }
                            else
                            {
                                hexLine1.HexBytes.Add(new HexByte { Value = "  ", IsDifferent = false });
                                ascii1.Append(' ');
                            }

                            if (i < count2)
                            {
                                hexLine2.HexBytes.Add(new HexByte
                                {
                                    Value = $"{buffer2[i]:X2}",
                                    IsDifferent = i < count1 && buffer1[i] != buffer2[i],
                                    Position = (int)position + i,
                                    ByteValue = buffer2[i],
                                    Parent = hexLine2,
                                    Index = i
                                });
                                ascii2.Append(buffer2[i] >= 32 && buffer2[i] <= 126 ? (char)buffer2[i] : '.');
                            }
                            else
                            {
                                hexLine2.HexBytes.Add(new HexByte { Value = "  ", IsDifferent = false });
                                ascii2.Append(' ');
                            }
                        }

                        hexLine1.AsciiText = ascii1.ToString();
                        hexLine2.AsciiText = ascii2.ToString();
                        
                        hexLine1.RawBytes = new byte[count1];
                        Array.Copy(buffer1, hexLine1.RawBytes, count1);
                        hexLine2.RawBytes = new byte[count2];
                        Array.Copy(buffer2, hexLine2.RawBytes, count2);

                        hexLines1.Add(hexLine1);
                        hexLines2.Add(hexLine2);
                        position += 16;
                        if (count1 < 16 && count2 < 16) break;
                    }
                }
                HexViewer1.ItemsSource = hexLines1;
                HexViewer2.ItemsSource = hexLines2;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"比较文件时发生错误：{ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                StatusText.Text = "比较失败";
            }
            StatusText.Text = "比较完成";
        }
    }
}