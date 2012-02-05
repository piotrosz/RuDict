using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Win32;
using RuDict.Gramota;
using RuDict.Google;
using RuDict.BabelPoint;
using RuDict.Model;
using RuDict.Interfaces;
using RuDict.Utils;
using RuDict.Exporters;
using RuDict.History;

namespace RuDict
{
    public partial class MainWindow : Window
    {
        private IHistoryManager historyManager = new RavenHistoryManager();
        private IDownloader googleDownloader = new GoogleDownloader();
        private IDownloader gramotaDownloader = new GramotaDownloader();
        private IDownloader babelPointDownloader = new BabelPointDownloader();
        private IScrapper gramotaScrapper = new GramotaScrapper();
        private IScrapper googleScrapper = new GoogleScrapper();
        private IScrapper babelPointScrapper = new BabelPointScrapper();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void TextBox_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key != System.Windows.Input.Key.Enter) return;

            e.Handled = true;
            string word = TextBoxWord.Text;
            
            DownloadAllDefinitions(word);
            
            historyManager.Add(word);
            PopulateListBox();
            TextBoxWord.Text = "";
        }

        private void DownloadAllDefinitions(string word)
        {
            gramotaDownloader.DownloadAsync(
                client_DownloadGramotaProgressChanged,
                client_DownloadGramotaCompleted, word);

            googleDownloader.DownloadAsync(
                client_DownloadGoogleProgressChanged,
                client_DownloadGoogleCompleted, word);

            babelPointDownloader.DownloadAsync(
                 client_DownloadBabelPointProgressChanged,
                client_DownloadBabelPointCompleted, word);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            TextBoxWord.Focus();
            PopulateListBox();
        }

        #region Async handlers
        private void handleCompleted(WebBrowser browser, ProgressBar progress,
            object sender, DownloadStringCompletedEventArgs e, IScrapper scrapper)
        {
            if (e.Error != null)
            {
                browser.NavigateToString("Error occured: " + e.Error.ToString());
            }
            else
            {
                if (e.Cancelled)
                {
                    browser.NavigateToString("Request was cancelled");
                }
                else
                {
                    var searchResults = scrapper.Scrape(e.Result);

                    browser.NavigateToString(GetHtml(searchResults));
                    progress.Value = 0;

                }
            }
        }
        private void handleProgressChanged(object sender, DownloadProgressChangedEventArgs e, ProgressBar progress)
        {
            progress.Value = e.ProgressPercentage;
        }

        public void client_DownloadGramotaCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            handleCompleted(WebBrowserGramota, ProgressBarGramota, sender, e,gramotaScrapper);
        }

        public void client_DownloadGoogleCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            handleCompleted(WebBrowserGoogle, ProgressBarGoogle, sender, e, googleScrapper);
        }

        public void client_DownloadBabelPointCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            handleCompleted(WebBrowserBabelPoint, ProgressBarBabelPoint, sender, e, babelPointScrapper);
        }

        public void client_DownloadGramotaProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            handleProgressChanged(sender, e, ProgressBarGramota);
        }

        public void client_DownloadGoogleProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            handleProgressChanged(sender, e, ProgressBarGoogle);
        }

        public void client_DownloadBabelPointProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            handleProgressChanged(sender, e, ProgressBarBabelPoint);
        }
        #endregion

        private string GetHtml(List<SearchResult> searchResults)
        {
            var sbResult = new StringBuilder();
            sbResult.Append("<html>");
            sbResult.Append("<head>");
            sbResult.Append("<meta http-equiv='Content-Type' content='text/html;charset=UTF-8'>");
            sbResult.Append("</head>");

            foreach (var item in searchResults)
            {
                sbResult.Append("<br/>");
                sbResult.Append(item.Source);
                sbResult.Append("<br/>");
                sbResult.Append(item.Description);
            }
            sbResult.Append("</html>");
            return Utils.Utils.ReplaceHrefBlank(sbResult.ToString());
        }

        private void PopulateListBox()
        {
            ListBoxHistory.Items.Clear();

            foreach (var item in historyManager.GetAll())
                ListBoxHistory.Items.Add(item.Word.ToString());
        }

        private void ListBoxHistory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            object selected = ListBoxHistory.SelectedValue;

            if (selected != null)
            {
                string word = selected.ToString();
                DownloadAllDefinitions(word);
            }
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            Properties.Settings.Default.Save();
        }

        #region Exporters
        private void ExportHtml_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "export.html";
            dlg.DefaultExt = ".html";
            dlg.Filter = "HTML documents (.html)|*.html";
            dlg.Title = "Export to HTML";

            if (dlg.ShowDialog() == true)
            {
                string filename = dlg.FileName;
                new HtmlExporter().Export(historyManager.GetAll(), filename);
            }
        }

        private void ExportXml_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "export.xml";
            dlg.DefaultExt = ".xml";
            dlg.Filter = "XML documents (.xml)|*.xml";
            dlg.Title = "Export to XML";

            if (dlg.ShowDialog() == true)
            {
                string filename = dlg.FileName;
                new XmlExporter().Export(historyManager.GetAll(), filename);
            }
        }

        private void ExportCsv_Click(object sender, RoutedEventArgs e)
        {
            var dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "export.txt";
            dlg.DefaultExt = ".txt";
            dlg.Filter = "Text documents (.txt)|*.txt";
            dlg.Title = "Export to CSV";

            if (dlg.ShowDialog() == true)
            {
                string filename = dlg.FileName;
                new CsvExporter().Export(historyManager.GetAll(), filename);
            }
        }
        #endregion

        #region Menu items click
        //private void Preferences_Click(object sender, RoutedEventArgs e)
        //{
        //    var dlg = new PreferencesWindow();
        //    dlg.Owner = this;
        //    dlg.ShowDialog();
        //}

        private void OpenBabelPoint_Click(object sender, RoutedEventArgs e)
        {
            Utils.Utils.OpenInDefaultBrowser("http://www.babelpoint.org");
        }

        private void OpenGramota_Click(object sender, RoutedEventArgs e)
        {
            Utils.Utils.OpenInDefaultBrowser("http://gramota.ru");
        }

        private void MenuItem_Click_RemoveSelected(object sender, RoutedEventArgs e)
        {
            List<string> items = new List<string>();
            foreach (object item in ListBoxHistory.SelectedItems)
                items.Add(item.ToString());

            if (items.Count > 0)
                historyManager.RemoveMany(items);

            PopulateListBox();
        }

        private void MenuItem_Click_RemoveAll(object sender, RoutedEventArgs e)
        {
            historyManager.RemoveAll();
            PopulateListBox();
        }

        private void MenuItem_Click_About(object sender, RoutedEventArgs e)
        {
            AboutWindow about = new AboutWindow();
            about.Owner = this;
            about.Show();
        }

        private void MenuItem_Click_Exit(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
        #endregion
    }
}
