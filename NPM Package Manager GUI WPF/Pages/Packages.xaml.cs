using System;
using System.Collections.Generic;
using System.Linq;
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
        Types.PackageJson package;
        string packagePath;
        public Packages(MainWindow mainWindow)
        {
            InitializeComponent();
            package = mainWindow.PackageFile;
            packagePath = mainWindow.PackagePath;

            RefreshList();
        }

        public async void RefreshList()
        {
            await Task.Delay(150);
            List.Items.Clear();
            foreach (var package in package.Dependencies)
            {
                await Task.Delay(5);
                List.Items.Add(package.Key);
            }

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
            LoadPackageInfo((string)List.Items[List.SelectedIndex]);
        }

        private void LoadPackageInfo(string packagename)
        {
            PackageName.Text = packagename;
        }
    }
}
