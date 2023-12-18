using System;
using System.Collections.Generic;
using System.Configuration;
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
using писец_какой_то.ViewModel;

namespace писец_какой_то.View
{
    /// <summary>
    /// Логика взаимодействия для Settings.xaml
    /// </summary>
    public partial class Settings : Page
    {
        public Settings()
        {
            InitializeComponent();
            FontStyle.SelectionChanged += ThemeChange;
        }
        private void ThemeChange(object sender, SelectionChangedEventArgs e)
        {
            ComboBoxItem selectedItem = (ComboBoxItem)FontStyle.SelectedItem;
            string style = selectedItem.Content.ToString();
            var uri = new Uri($"/Theme/{style}.xaml", UriKind.Relative);
            ResourceDictionary resourceDict = Application.LoadComponent(uri) as ResourceDictionary;
            Application.Current.Resources.Clear();
            Application.Current.Resources.MergedDictionaries.Add(resourceDict);
        }
        private void Theme_Change_Click(object sender, RoutedEventArgs e)
        {
            var currentTheme = Application.Current.Resources.MergedDictionaries[0];

            // Переключаем между темами
            if (currentTheme.Source.OriginalString == "Theme/WhiteTheme.xaml")
            {
                Application.Current.Resources.MergedDictionaries[0] = new ResourceDictionary { Source = new Uri("Theme/BlackTheme.xaml", UriKind.Relative) };
            }
            else
            {
                Application.Current.Resources.MergedDictionaries[0] = new ResourceDictionary { Source = new Uri("Theme/WhiteTheme.xaml", UriKind.Relative) };
            }
        }
    }
}
