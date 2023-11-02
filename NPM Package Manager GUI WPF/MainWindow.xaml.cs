using Newtonsoft.Json;
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

        public Types.PackageJson PackageFile = new Types.PackageJson();
        public string PackagePath = "";

        public bool ProjectLoaded = false;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            ReloadUI();
            OpenPage(new Pages.NoProject(this));
        }

        private void MMPOP_Click(object sender, RoutedEventArgs e)
        {
            OpenProject();
        }

        public void OpenPage(Page page)
        {
            CurrentPage = page;
            CurrentFrame.Content = CurrentPage.Content;
        }

        public void OpenProject()
        {
            VistaOpenFileDialog ofd = new VistaOpenFileDialog();

            ofd.Title = "Open a package.json file";
            ofd.Filter = "NPM Package File | package.json";

            if ((bool)ofd.ShowDialog())
            {
                PackageFile = JsonConvert.DeserializeObject<Types.PackageJson>(File.ReadAllText(ofd.FileName));
                Title = $"NPM Package Manager GUI WPF - {PackageFile.Name}";
                OpenPage(new Pages.Packages(this));
                PackagePath = ofd.FileName;

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
            } else
            {
                Scripts.SelectedIndex = -1;
                Scripts.IsEnabled = false;
                RunButton.IsEnabled = false;
            }
        }

        private void MMPRP_Click(object sender, RoutedEventArgs e)
        {
            ReloadProject();
            OpenPage(new Pages.Packages(this));
            ReloadUI();
        }
    }
}