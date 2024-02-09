using Newtonsoft.Json;
using NPM_Package_Manager_GUI_WPF.Types;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for Packages.xaml
    /// </summary>
    public partial class Packages : Page
    {
        MainWindow mainWindow;
        Types.PackageJson package;
        string packagePath;
        public Packages(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
            package = mainWindow.PackageFile;
            packagePath = mainWindow.PackagePath;

            RefreshList();
        }

        public async void RefreshList()
        {
            //await Task.Delay(150);
            int i = 0;
            int count = package.Dependencies.Count;
            List.Items.Clear();
            foreach (var package in package.Dependencies)
            {
                i++;
                mainWindow.UpdateProgress("Loading packages...", i, count);
                //await Task.Delay(5);

                if(!package.Key.StartsWith("@types"))
                {
                    List.Items.Add(package.Key);
                }
            }

            mainWindow.UpdateProgress();

            /*foreach (ListViewItem item in List.Items)
            {
                if(Actions.CheckProjectState.SinglePackageInstalled((string)item.Content, packagePath))
                {
                    item.Background = Brushes.Green;
                } else
                {
                    item.Background = Brushes.Red;
                }
            }*/

            List.Items.Refresh();
        }

        private void List_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                LoadPackageInfo((string)List.Items[List.SelectedIndex]);
            }
            catch (Exception ex) { }
        }

        private async void LoadPackageInfo(string packagename)
        {
            string CurrentVersion = "";

            // Blank out data that isn't loaded yet
            PackageAuthor.Text = "";
            PackageDescription.Text = "";

            PackageName.Text = packagename;

            foreach (var package in package.Dependencies)
            {
                if(package.Key == packagename)
                {
                    PackageVersion.Text = package.Value;
                }
            }

            mainWindow.SBProgressText.Text = "Loading package data...";
            mainWindow.SBProgressSpinner.Visibility = Visibility.Visible;

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.UserAgent.ParseAdd("NPM Package Manager GUI WPF"); // Probably not the best UserAgent
                var response = await client.GetAsync($"https://registry.npmjs.org/{packagename}/latest");
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    RegistryPackageVersion PackageData = JsonConvert.DeserializeObject<RegistryPackageVersion>(content);

                    if(PackageData != null && PackageData.Author != null)
                    {
                        //PackageVersion.Text += " ->? " + PackageData.Version;

                        if (PackageVersion.Text.StartsWith("^"))
                        {
                            string CutVersion = PackageVersion.Text.Substring(1);

                            string Version = CutVersion;
                            CurrentVersion = CutVersion;
                            string[] SplitVersion = Version.Split('.');
                            string[] SplitNewVersion = PackageData.Version.Split('.');

                            bool SameVersion = Version == PackageData.Version;

                            bool IsMajor = SplitVersion[0] != SplitNewVersion[0];
                            bool IsMinor = SplitVersion[1] != SplitNewVersion[1];
                            bool IsPatch = SplitVersion[2] != SplitNewVersion[2];

                            if(SameVersion)
                            {
                                Upgrade.IsEnabled = false;
                            } else
                            {
                                Upgrade.IsEnabled = true;
                                PackageVersion.Text += " - " + CutVersion + " -> " + PackageData.Version;

                                if(IsMajor)
                                {
                                    PackageVersion.Text += " (Major)";
                                } else if(IsMinor)
                                {
                                    PackageVersion.Text += " (Minor)";
                                } else if(IsPatch)
                                {
                                    PackageVersion.Text += " (Patch)";
                                }
                            }
                        }
                        PackageAuthor.Text = $"By {PackageData.Author.Name}";
                        PackageDescription.Text = PackageData.Description;
                    } else
                    {
                        PackageAuthor.Text = $"By Unknown";
                        PackageDescription.Text = "";
                    }
                }
                else
                {
                    MessageBox.Show("Failed to fetch package from registry.npmjs.org.\n" + await response.Content.ReadAsStringAsync(), "NPM Package Manger GUI WPF - Error");
                }

                mainWindow.SBProgressText.Text = "Ready";
                mainWindow.SBProgressSpinner.Visibility = Visibility.Collapsed;
            }

            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.UserAgent.ParseAdd("NPM Package Manager GUI WPF"); // Probably not the best UserAgent
                var response = await client.GetAsync($"https://registry.npmjs.org/{packagename}/");
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    RegistryVersions VersionsData = JsonConvert.DeserializeObject<RegistryVersions>(content);

                    Versions.Items.Clear();

                    /*foreach (var version in VersionsData.Versions)
                    {
                        if(version.Value.Deprecated != null && !mainWindow.config.AllowDepricatedPackageVersion)
                        {
                            Versions.Items.Add(new ComboBoxItem() { Content = version.Key, IsEnabled = false });
                        } else
                        {
                            Versions.Items.Add(new ComboBoxItem() { Content = version.Key });
                        }
                    }*/

                    List<string> VersionNames = new List<string>();

                    foreach(var version in VersionsData.Versions)
                    {
                        VersionNames.Add(version.Key);
                    }

                    VersionNames.Reverse();

                    foreach (var version in VersionNames)
                    {
                        ComboBoxItem item = new ComboBoxItem() { Content =  version };

                        RegistryVersion _version = VersionsData.Versions[version];
                        if(_version.Deprecated != null && !mainWindow.config.AllowDepricatedPackageVersion)
                        {
                            item.IsEnabled = false;
                        }

                        Versions.Items.Add(item);
                    }

                    Versions.Items.Refresh();

                    for (int i = 0; i < Versions.Items.Count; i++)
                    {
                        if (((ComboBoxItem)Versions.Items[i]).Content == CurrentVersion)
                        {
                            Versions.SelectedIndex = i;
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Failed to fetch package from registry.npmjs.org.\n" + await response.Content.ReadAsStringAsync(), "NPM Package Manger GUI WPF - Error");
                }

                mainWindow.SBProgressText.Text = "Ready";
                mainWindow.SBProgressSpinner.Visibility = Visibility.Collapsed;
            }
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.RemoveSpecfic((string)List.Items[List.SelectedIndex]);
        }

        private void ChangeVersion_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.ChangeSpecific((string)List.Items[List.SelectedIndex], (string)((ComboBoxItem)Versions.Items[Versions.SelectedIndex]).Content);
        }

        private void Upgrade_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                mainWindow.ChangeSpecific((string)List.Items[List.SelectedIndex], (string)((ComboBoxItem)Versions.Items[Versions.Items.Count - 1]).Content);
            } catch { }
        }

        public void Reload()
        {
            RefreshList();
            try
            {
                LoadPackageInfo((string)List.Items[List.SelectedIndex]);
            } catch { }
        }

        private void ViewPackage_Click(object sender, RoutedEventArgs e)
        {
            ViewPackage vp = new ViewPackage();
            vp.Initialise(PackageName.Text);
            mainWindow.OpenPage(vp);
        }
    }
}
