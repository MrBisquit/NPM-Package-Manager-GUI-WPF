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
    /// Interaction logic for ProjectInformation.xaml
    /// </summary>
    public partial class ProjectInformation : Page
    {
        MainWindow mainWindow;
        public ProjectInformation(MainWindow mainWindow)
        {
            InitializeComponent();

            this.mainWindow = mainWindow;

            ProjectName.Text = mainWindow.PackageFile.Name;
            ProjectDescription.Text = mainWindow.PackageFile.Description;
            ProjectAuthor.Text = mainWindow.PackageFile.Author;
            ProjectVersion.Text = mainWindow.PackageFile.Version;

            FetchLicenses();

            bool foundLicense = false;

            for (int i = 0; i < Licenses.Items.Count; i++)
            {
                if (Licenses.Items[i].ToString() == mainWindow.PackageFile.License)
                {
                    Licenses.SelectedIndex = i;
                    foundLicense = true;
                }
            }

            if(!foundLicense)
            {
                Licenses.Items.Add(new ComboBoxItem() { Content = "Custom", IsSelected = true, Visibility = Visibility.Collapsed });
                CustomLicense.Visibility = Visibility.Visible;
                CustomLicense.Text = mainWindow.PackageFile.License;
            }
        }

        private async void FetchLicenses()
        {
            // ChatGPT because I don't know how to do this
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.UserAgent.ParseAdd("NPM Package Manager GUI WPF"); // Probably not the best UserAgent
                var response = await client.GetAsync("https://api.github.com/licenses");
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    List<GitHubLicense> licenses = JsonConvert.DeserializeObject<List<GitHubLicense>>(content);

                    foreach (var license in licenses)
                    {
                        Licenses.Items.Add(license.Name);
                    }
                }
                else
                {
                    MessageBox.Show("Failed to fetch licenses from GitHub API.\n" + await response.Content.ReadAsStringAsync(), "NPM Package Manger GUI WPF - Error");
                }
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.BackPage();
        }
    }
}
