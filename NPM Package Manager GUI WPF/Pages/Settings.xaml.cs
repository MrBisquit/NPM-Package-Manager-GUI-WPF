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
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class Settings : Page
    {
        MainWindow mainWindow;
        public Settings(MainWindow mainWindow)
        {
            InitializeComponent();

            this.mainWindow = mainWindow;

            ADPV.IsChecked = mainWindow.config.AllowDepricatedPackageVersion;
            TBOPF.IsChecked = mainWindow.config.TakeBackupsOfProjectFiles;
            DM.IsChecked = mainWindow.config.DebugMode;
        }

        private void ADPV_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.config.AllowDepricatedPackageVersion = (bool)ADPV.IsChecked;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.BackPage();
        }

        private void TBOPF_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.config.TakeBackupsOfProjectFiles = (bool)TBOPF.IsChecked;
        }

        private void DM_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.config.DebugMode = (bool)DM.IsChecked;
        }
    }
}
