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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NPM_Package_Manager_GUI_WPF.Pages
{
    /// <summary>
    /// Interaction logic for ViewPackage.xaml
    /// </summary>
    public partial class ViewPackage : Page
    {
        public ViewPackage()
        {
            InitializeComponent();
        }

        string PackageName = "";

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Globals.MainWindow.BackPage();
        }

        public void Initialise(string PackageName)
        {
            Versions.IsReadOnly = true;
            DataGridTextColumn versionColumn = new DataGridTextColumn
            {
                Header = "Version",
                Binding = new Binding("Version")
            };
            DataGridTextColumn deprecatedColumn = new DataGridTextColumn
            {
                Header = "Deprecated",
                Binding = new Binding("Deprecated")
            };
            Versions.Columns.Add(versionColumn);
            Versions.Columns.Add(deprecatedColumn);

            this.PackageName = PackageName;

            LoadInfo();
        }

        private async void LoadInfo()
        {
            Globals.MainWindow.SBProgressText.Text = "Loading package data...";
            Globals.MainWindow.SBProgressSpinner.Visibility = Visibility.Visible;

            CurrentPackageName.Text = PackageName;

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.UserAgent.ParseAdd("NPM Package Manager GUI WPF"); // Probably not the best UserAgent
                var response = await client.GetAsync($"https://registry.npmjs.org/{PackageName}/latest");
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    RegistryPackageVersion PackageData = JsonConvert.DeserializeObject<RegistryPackageVersion>(content);

                    LatestPackageVersion.Text = PackageData.NPMVersion;
                    PackageDescription.Text = PackageData.Description;
                    string maintainers = "";
                    for (int i = 0; i < PackageData.Maintainers.Length; i++)
                    {
                        maintainers += $"{PackageData.Maintainers[i].Name}";
                        if(i != PackageData.Maintainers.Length - 1)
                        {
                            maintainers += ", ";
                        }
                    }
                    PackageBy.Text = PackageData.Author.Name + " and maintainers " + maintainers;
                }
            }

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
                        bool deprecated = false;
                        RegistryVersion _version = VersionsData.Versions[version];
                        if (_version.Deprecated != null && !Globals.MainWindow.config.AllowDepricatedPackageVersion)
                        {
                            deprecated = true;
                        }

                        Versions.Items.Add(new { Version = version, Deprecated = deprecated });
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
    }
}
