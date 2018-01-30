using System;
using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace RestaurantDiary
{
    /// <summary>
    /// Interaction logic for NewTableDialog.xaml
    /// </summary>
    public partial class NewTableDialog : Window
    {
        public ObservableCollection<Table> Tables
        {
            get;
            set;
        }
        public Table NewTable;

        #region Constructors

        public NewTableDialog(ObservableCollection<Table> tables)
        {
            InitializeComponent();
            Tables = tables;
        }

        #endregion

        #region Events

        private void SaveTableButton_Click(object sender, RoutedEventArgs e)
        {
            int newTableID;
            int newSeats;

            try
            {
                newTableID = int.Parse(TableIDTextBox.Text);
            }
            catch (FormatException)
            {
                MessageBox.Show("Enter numbers only", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            try
            {
                newSeats = int.Parse(SeatsTextBox.Text);
            }
            catch (FormatException)
            {
                MessageBox.Show("Enter numbers only", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            NewTable = new Table(newTableID, newSeats);

            DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void TextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            Regex regex = new Regex(@"[^0-9]");

            e.Handled = regex.IsMatch(e.Text);
        }

        private void TextBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space) e.Handled = true;
        }

        #endregion
    }
}