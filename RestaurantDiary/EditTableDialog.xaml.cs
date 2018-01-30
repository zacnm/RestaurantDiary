using System;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;

namespace RestaurantDiary
{
    /// <summary>
    /// Interaction logic for EditTableDialog.xaml
    /// </summary>
    public partial class EditTableDialog : Window
    {
        public ObservableCollection<Table> Tables
        {
            get;
            set;
        }
        Table ActiveTable;
        public Table EditedTable;
        private ObservableCollection<Booking> Bookings;

        public EditTableDialog(ObservableCollection<Table> tables, Table activeTable, ObservableCollection<Booking> bookings)
        {
            InitializeComponent();
            Tables = new ObservableCollection<Table>();
            Bookings = bookings;
            foreach (Table t in tables) Tables.Add(t);
            Tables.Remove(activeTable);
            ActiveTable = activeTable;
            TableIDTextBox.Text = ActiveTable.ID.ToString();
            SeatsTextBox.Text = ActiveTable.Seats.ToString();
        }

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

            if (Bookings.Any(b => b.Guests > newSeats && b.TableID == ActiveTable.ID))
            {
                if (MessageBox.Show(
                        "Some bookings on this table have more guests than the table's new number of seats. Delete these bookings?",
                        "Edit table", MessageBoxButton.YesNo, MessageBoxImage.Exclamation) == MessageBoxResult.Yes)
                {
                    List<Booking> bookingsToDelete = Bookings.Where(b => b.Guests > newSeats && b.TableID == ActiveTable.ID).ToList();
                    foreach (Booking b in bookingsToDelete)
                    {
                        Bookings.Remove(b);
                    }
                }
            }

            EditedTable = new Table(newTableID, newSeats);

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
    }
}