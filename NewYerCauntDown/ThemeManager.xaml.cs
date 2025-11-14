using NewYerCauntDown;
using System;
using System.Windows;
using System.Windows.Media;
using Microsoft.Win32;
using System.IO;
using System.Windows.Markup;

namespace NewYerCauntDown
{
    /// <summary>
    /// Interaction logic for ThemeManager.xaml
    /// </summary>
    public partial class ThemeManager : Window
    {
        public ThemeManager()
        {
            InitializeComponent();

            bgR.ValueChanged += All_ValueChanged;
            bgG.ValueChanged += All_ValueChanged;
            bgB.ValueChanged += All_ValueChanged;
            fgR.ValueChanged += All_ValueChanged;
            fgG.ValueChanged += All_ValueChanged;
            fgB.ValueChanged += All_ValueChanged;

            Loaded += ThemeManager_Loaded;
        }

        byte ToByte(int? value)
        {
            return (byte)(value ?? 0);
        }

        private void Export_Click(object sender, RoutedEventArgs e)
        {
            int bg = ((bgR.Value ?? 0) << 16) | ((bgG.Value ?? 0) << 8) | (bgB.Value ?? 0);
            int fg = ((fgR.Value ?? 0) << 16) | ((fgG.Value ?? 0) << 8) | (fgB.Value ?? 0);
            string font = fontList.SelectedValue.ToString();

            string[] IniLines = new string[]
            {
                "[Theme]",
                $"bg={bg:X6}",
                $"fg={fg:X6}",
                $"font={font}"
            };

            var dlg = new SaveFileDialog
            {
                FileName = "theme",
                DefaultExt = ".ini",
                Filter = "INI files (*.ini)|*.ini|All files (*.*)|*.*"
            };

            bool? result = dlg.ShowDialog();
            if (result == true)
            {
                File.WriteAllLines(dlg.FileName, IniLines);
            }
        }

        private void Import_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new OpenFileDialog
            {
                DefaultExt = ".ini",
                Filter = "INI files (*.ini)|*.ini|All files (*.*)|*.*"
            };

            bool? result = dlg.ShowDialog();
            if (result != true) return;

            /* yes, im wrapping the entire thing in a try-catch. */
            /* why? i'm lazy */
            /* will this be changed? no. */
            try
            {

                string[] IniLines = File.ReadAllLines(dlg.FileName);

                int bg = 0, fg = 0;
                string font = "";

                foreach (var line in IniLines)
                {
                    if (line.StartsWith("bg="))
                        bg = Convert.ToInt32(line.Substring(3), 16);
                    else if (line.StartsWith("fg="))
                        fg = Convert.ToInt32(line.Substring(3), 16);
                    else if (line.StartsWith("font="))
                        font = line.Substring(5);
                }

                bgR.Value = ((bg >> 16) & 0xFF);
                bgG.Value = ((bg >> 8) & 0xFF);
                bgB.Value = (bg & 0xFF);

                fgR.Value = ((fg >> 16) & 0xFF);
                fgG.Value = ((fg >> 8) & 0xFF);
                fgB.Value = (fg & 0xFF);

                int selectedIndex = -1;
                for (int i = 0; i < fontList.Items.Count; i++)
                {
                    if (fontList.Items[i].ToString() == font)
                    {
                        selectedIndex = i;
                        break;
                    }
                }

                if (selectedIndex >= 0)
                {
                    fontList.SelectedIndex = selectedIndex;
                }
                else
                {
                    MessageBox.Show($"Error loading font '{font}': font not found. Is it installed?", "new year countdown: theme manager - error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Exception caught: {ex.Message}\n\n{ex.ToString()}", "new year countdown: theme manager - error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        private void ThemeManager_Loaded(object sender, RoutedEventArgs e)
        {
            var main = this.Owner as MainWindow;
            if (main == null)
            {
                MessageBox.Show("this.Owner as MainWindow = null", "new yer cauntdown: theme manager - error", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }

            bgR.Value = (int)main.bg.R;
            bgG.Value = (int)main.bg.G;
            bgB.Value = (int)main.bg.B;
            fgR.Value = (int)main.fg.R;
            fgG.Value = (int)main.fg.G;
            fgB.Value = (int)main.fg.B;

            foreach (FontFamily font in Fonts.SystemFontFamilies)
            {
                fontList.Items.Add(font.ToString());
            }

            int selectedIndex = -1;
            for (int i = 0; i < fontList.Items.Count; i++)
            {
                if (fontList.Items[i].ToString() == main.font.ToString())
                {
                    selectedIndex = i;
                    break;
                }
            }

            if (selectedIndex >= 0)
            {
                fontList.SelectedIndex = selectedIndex;
            }

        }
        public void All_ValueChanged(object sender, EventArgs e)
        {
            try
            {
                bgPreviewRect.Fill = new SolidColorBrush(Color.FromRgb(
                    ToByte(bgR.Value),
                    ToByte(bgG.Value),
                    ToByte(bgB.Value)
                ));
                fgPreviewRect.Fill = new SolidColorBrush(Color.FromRgb(
                    ToByte(fgR.Value),
                    ToByte(fgG.Value),
                    ToByte(fgB.Value)
                ));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Exception caught: {ex.Message}\n\n{ex}", "new year countdown: theme manager - error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        // OK button //
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            var main = this.Owner as MainWindow;
            if (main != null)
            {
                try
                {
                    main.bg = Color.FromRgb(
                        ToByte(bgR.Value),
                        ToByte(bgG.Value),
                        ToByte(bgB.Value)
                    );
                    main.fg = Color.FromRgb(
                        ToByte(fgR.Value),
                        ToByte(fgG.Value),
                        ToByte(fgB.Value)
                    );

                    main.font = fontList.SelectedValue.ToString();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Exception caught: {ex.Message}\n\n{ex}", "new year countdown: theme manager - error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                this.Close();
            }
        } 
    }
}
