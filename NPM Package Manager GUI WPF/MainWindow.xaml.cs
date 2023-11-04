using Newtonsoft.Json;
using NPM_Package_Manager_GUI_WPF.Actions;
using Ookii.Dialogs.Wpf;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NPM_Package_Manager_GUI_WPF
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        public Page CurrentPage;
        public Page LastPage;
        public Pages.SearchResults SearchResults;

        public Types.PackageJson PackageFile = new Types.PackageJson();
        public string PackagePath = "";

        public bool ProjectLoaded = false;

        public Types.Config config = new Types.Config();

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ReloadUI();
            OpenPage(new Pages.NoProject(this));
            UpdateProgress();

            if(Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\NPM Package Manager GUI WPF\\"))
            {
                if(File.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\NPM Package Manager GUI WPF\\config.json"))
                {
                    config = JsonConvert.DeserializeObject<Types.Config>(File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\NPM Package Manager GUI WPF\\config.json"));
                    if(config.LastOpenedPackagePath != string.Empty)
                    {
                        PackageFile = config.LastOpenedPackageFile;
                        PackagePath = config.LastOpenedPackagePath;
                        ProjectLoaded = true;

                        ReloadUI();
                        OpenPage(new Pages.Packages(this));
                    }
                }
            } else
            {
                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\NPM Package Manager GUI WPF\\");
            }
        }

        private void MMPOP_Click(object sender, RoutedEventArgs e)
        {
            OpenProject();
        }

        public void OpenPage(Page page)
        {
            LastPage = CurrentPage;
            CurrentPage = page;
            CurrentFrame.Content = CurrentPage.Content;
        }

        public void BackPage()
        {
            OpenPage(LastPage);
        }

        public void OpenProject()
        {
            VistaOpenFileDialog ofd = new VistaOpenFileDialog();

            ofd.Title = "Open a package.json file";
            ofd.Filter = "NPM Package File | package.json";

            if ((bool)ofd.ShowDialog())
            {
                PackageFile = JsonConvert.DeserializeObject<Types.PackageJson>(File.ReadAllText(ofd.FileName));
                OpenPage(new Pages.Packages(this));
                PackagePath = ofd.FileName;

                config.LastOpenedPackagePath = PackagePath;
                config.LastOpenedPackageFile = PackageFile;

                Actions.CheckProjectState.HasAllPackagesInstalled(PackageFile, PackagePath);

                ProjectLoaded = true;
                ReloadUI();
            }
        }

        public void ReloadProject() => PackageFile = JsonConvert.DeserializeObject<Types.PackageJson>(File.ReadAllText(PackagePath));

        public void ReloadUI()
        {
            if(ProjectLoaded)
            {
                Scripts.IsEnabled = true;
                RunButton.IsEnabled = true;

                Scripts.Items.Clear();

                foreach (var script in PackageFile.Scripts)
                {
                    Scripts.Items.Add(script.Key);
                }

                Scripts.Items.Refresh();

                Scripts.SelectedIndex = 0;

                Title = $"NPM Package Manager GUI WPF - {PackageFile.Name}";

                MMPUP.IsEnabled = true;
                MMPRP.IsEnabled = true;

                MMEPI.IsEnabled = true;

                MMPa.IsEnabled = true;
                MME.IsEnabled = true;
                MMV.IsEnabled = true;

                if(File.Exists(PackagePath + "\\..\\yarn.lock"))
                {
                    SelectedCLI.SelectedIndex = 1;
                    SCLIYarn.IsEnabled = true;
                } else
                {
                    SelectedCLI.SelectedIndex = 0;
                    SCLIYarn.IsEnabled = false;
                }
            } else
            {
                Scripts.SelectedIndex = -1;
                Scripts.IsEnabled = false;
                RunButton.IsEnabled = false;

                Title = $"NPM Package Manager GUI WPF - No Project open";

                MMPUP.IsEnabled = false;
                MMPRP.IsEnabled = false;

                MMEPI.IsEnabled = false;

                MMPa.IsEnabled = false;
                MME.IsEnabled = false;
                MMV.IsEnabled = false;

                SelectedCLI.SelectedIndex = 0;
                SCLIYarn.IsEnabled = false;
            }
        }

        public void UpdateProgress(string? text = null, int? value = null, int? maximum = null)
        {
            if(text == null && value == null && maximum == null)
            {
                SBProgressBar.Visibility = Visibility.Collapsed;
                SBProgressSpinner.Visibility = Visibility.Collapsed;

                SBProgressText.Text = "Ready";
            } else
            {
                SBProgressBar.Visibility = Visibility.Visible;
                SBProgressSpinner.Visibility = Visibility.Visible;

                SBProgressText.Text = text;
                SBProgressBar.Value = (int)value;
                SBProgressBar.Maximum = (int)maximum;
            }
        }

        private void MMPRP_Click(object sender, RoutedEventArgs e)
        {
            ReloadProject();
            OpenPage(new Pages.Packages(this));
            ReloadUI();
        }

        private void MMEPI_Click(object sender, RoutedEventArgs e)
        {
            OpenPage(new Pages.ProjectInformation(this));
        }

        private async void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            ProjectLoaded = false;
            ReloadUI();
            OpenPage(new Pages.NoProject(this));
            if (!Directory.Exists(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\NPM Package Manager GUI WPF\\"))
            {
                Directory.CreateDirectory(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\NPM Package Manager GUI WPF\\");
            }

            File.WriteAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\NPM Package Manager GUI WPF\\config.json", JsonConvert.SerializeObject(config));
        }

        private void MMPUP_Click(object sender, RoutedEventArgs e)
        {
            ProjectLoaded = false;
            ReloadUI();
            OpenPage(new Pages.NoProject(this));

            PackageFile = null;
            PackagePath = null;

            config.LastOpenedPackagePath = string.Empty;
            config.LastOpenedPackageFile = null;
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if(SearchResults == null)
            {
                SearchResults = new Pages.SearchResults(this);
            }

            if(CurrentPage != SearchResults) OpenPage(SearchResults);

            SearchResults.UpdateSearchText(SearchBox.Text);
        }

        private void MMPaRI_Click(object sender, RoutedEventArgs e)
        {
            UpdateProgress("Installing packages...", 0, 1);
            bool IsYarn = SelectedCLI.Items[SelectedCLI.SelectedIndex] == "Yarn";

            Task.Factory.StartNew(() =>
            {
                Actions.RunCommand.RunInstall(PackagePath.Substring(0, PackagePath.Length - "package.json".Length), IsYarn, false);
                Dispatcher.Invoke(() =>
                {
                    UpdateProgress("Installing packages...", 0, 0);
                });
            }).ContinueWith((Task task) =>
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    UpdateProgress();

                    ReloadProject();
                    ReloadUI();
                }));
            });
        }

        public void InstallSpecific(string PackageName)
        {
            UpdateProgress($"Installing {PackageName}...", 0, 1);
            bool IsYarn = SelectedCLI.Items[SelectedCLI.SelectedIndex] == "Yarn";

            Task.Factory.StartNew(() =>
            {
                RunCommand.RunInstallSpecific(PackagePath.Substring(0, PackagePath.Length - "package.json".Length), IsYarn, false, (string)PackageName);
                Dispatcher.Invoke(() =>
                {
                    UpdateProgress($"Installing {PackageName}...", 0, 0);
                });
            }).ContinueWith((Task task) =>
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    UpdateProgress();

                    ReloadProject();
                    ReloadUI();
                }));
            });
        }

        public void RemoveSpecfic(string PackageName)
        {
            UpdateProgress($"Removing {PackageName}...", 0, 1);
            bool IsYarn = SelectedCLI.Items[SelectedCLI.SelectedIndex] == "Yarn";

            Task.Factory.StartNew(() =>
            {
                RunCommand.RunRemoveSpecific(PackagePath.Substring(0, PackagePath.Length - "package.json".Length), IsYarn, false, (string)PackageName);
                Dispatcher.Invoke(() =>
                {
                    UpdateProgress($"Removing {PackageName}...", 0, 0);
                });
            }).ContinueWith((Task task) =>
            {
                Dispatcher.Invoke(new Action(() =>
                {
                    UpdateProgress();

                    ReloadProject();
                    ReloadUI();
                }));
            });
        }
    }
}