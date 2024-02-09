using Newtonsoft.Json;
using NPM_Package_Manager_GUI_WPF.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace NPM_Package_Manager_GUI_WPF.Popups
{
    /// <summary>
    /// Interaction logic for InstallSpecific.xaml
    /// </summary>
    public partial class InstallSpecific : Window
    {
        public InstallSpecific()
        {
            InitializeComponent();

            if (Globals.MainWindow.WindowState == WindowState.Maximized) Globals.MainWindow.WindowState = WindowState.Normal; // For now

            this.Width = Globals.MainWindow.Width - 10;
            this.Height = Globals.MainWindow.Height - 5 - 30;
            this.Left = Globals.MainWindow.Left + 5;
            this.Top = Globals.MainWindow.Top + 30;

            //if (Globals.MainWindow.WindowState == WindowState.Maximized) WindowState = WindowState.Maximized;

            ShowInTaskbar = false;
        }

        string PackageName = "";

        /*public void Initialise(int width, int height, int Left, int Top)
        {
            this.Width = width;
            this.Height = height;
            this.Left = Left;
            this.Top = Top;
        }*/

        public async void Initialise(string PackageName)
        {
            this.PackageName = PackageName;

            CurrentPackageName.Text = PackageName;

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.UserAgent.ParseAdd("NPM Package Manager GUI WPF"); // Probably not the best UserAgent
                var response = await client.GetAsync($"https://registry.npmjs.org/{PackageName}/");
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    RegistryVersions VersionsData = JsonConvert.DeserializeObject<RegistryVersions>(content);

                    Versions.Items.Clear();

                    List<string> VersionNames = new List<string>();

                    foreach (var version in VersionsData.Versions)
                    {
                        VersionNames.Add(version.Key);
                    }

                    VersionNames.Reverse();

                    foreach (var version in VersionNames)
                    {
                        ComboBoxItem item = new ComboBoxItem() { Content = version };

                        RegistryVersion _version = VersionsData.Versions[version];
                        if (_version.Deprecated != null && !Globals.MainWindow.config.AllowDepricatedPackageVersion)
                        {
                            item.IsEnabled = false;
                        }

                        Versions.Items.Add(item);
                    }

                    Versions.Items.Refresh();
                }
                else
                {
                    MessageBox.Show("Failed to fetch package from registry.npmjs.org.\n" + await response.Content.ReadAsStringAsync(), "NPM Package Manger GUI WPF - Error");
                }

                Globals.MainWindow.SBProgressText.Text = "Ready";
                Globals.MainWindow.SBProgressSpinner.Visibility = Visibility.Collapsed;
            }
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Install_Click(object sender, RoutedEventArgs e)
        {
            ComboBoxItem item = (ComboBoxItem)Versions.Items[Versions.SelectedIndex];
            Globals.MainWindow.ChangeSpecific(PackageName, (string)item.Content);
            Close();
        }
    }
}
