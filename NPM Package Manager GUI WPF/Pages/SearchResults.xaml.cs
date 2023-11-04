using Newtonsoft.Json;
using NPM_Package_Manager_GUI_WPF.Actions;
using NPM_Package_Manager_GUI_WPF.Types;
using System;
using System.Collections.Generic;
using System.IO.Packaging;
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
    /// Interaction logic for SearchResults.xaml
    /// </summary>
    public partial class SearchResults : Page
    {
        MainWindow mainWindow;
        SearchResult results;
        public SearchResults(MainWindow mainWindow)
        {
            InitializeComponent();

            this.mainWindow = mainWindow;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.BackPage();
        }

        public async void UpdateSearchText(string text)
        {
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.UserAgent.ParseAdd("NPM Package Manager GUI WPF"); // Probably not the best UserAgent
                var response = await client.GetAsync($"https://registry.npmjs.org/-/v1/search?text={text}");
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                    SearchResult PackagesData = JsonConvert.DeserializeObject<SearchResult>(content);
                    results = PackagesData;
                    RefreshList();
                }
                else
                {
                    MessageBox.Show("Failed to fetch packages from registry.npmjs.org.\n" + await response.Content.ReadAsStringAsync(), "NPM Package Manger GUI WPF - Error");
                }

                mainWindow.SBProgressText.Text = "Ready";
                mainWindow.SBProgressSpinner.Visibility = Visibility.Collapsed;
            }
        }

        private async void List_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            PackageName.Text = "";
            PackageVersion.Text = "";
            PackageAuthor.Text = "";
            PackageDescription.Text = "";

            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.UserAgent.ParseAdd("NPM Package Manager GUI WPF"); // Probably not the best UserAgent
                    var response = await client.GetAsync($"https://registry.npmjs.org/{List.Items[List.SelectedIndex]}/latest");
                    if (response.IsSuccessStatusCode)
                    {
                        string content = await response.Content.ReadAsStringAsync();
                        RegistryPackageVersion PackageData = JsonConvert.DeserializeObject<RegistryPackageVersion>(content);

                        if (PackageData != null && PackageData.Author != null)
                        {
                            PackageName.Text = PackageData.Name;
                            PackageVersion.Text = PackageData.Version;
                            PackageAuthor.Text = $"By {PackageData.Author.Name}";
                            PackageDescription.Text = PackageData.Description;
                        }
                        else
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
            } catch { }
        }

        public void RefreshList()
        {
            List.Items.Clear();
            foreach (var result in results.Objects)
            {
                List.Items.Add(result.Package.Name);
            }
            List.Items.Refresh();
        }

        private void InstallVersion_Click(object sender, RoutedEventArgs e)
        {
            mainWindow.InstallSpecific((string)List.Items[List.SelectedIndex]);
        }
    }
}
