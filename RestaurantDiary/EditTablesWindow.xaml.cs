using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;

namespace RestaurantDiary
{
    public partial class EditTablesWindow
    {
        #region Properties

        public ObservableCollection<Table> Tables
        {
            get;
            set;
        }
        public ObservableCollection<Booking> Bookings;

        #endregion

        #region Constructors

        public EditTablesWindow(ObservableCollection<Table> tables, ObservableCollection<Booking> bookings)
        {
            InitializeComponent();

            Tables = tables;
            Bookings = bookings;
            DataContext = this;
        }

        #endregion

        #region Methods

        private void RunEditTableDialog()
        {
            Table tableToEdit = (Table)TablesListView.SelectedItem;
            EditTableDialog editTableDialog = new EditTableDialog(Tables, tableToEdit, Bookings);

            if (editTableDialog.ShowDialog() == true)
            {
                
                Table editedTable = editTableDialog.EditedTable;

                if (tableToEdit.ID != editedTable.ID)
                {
                    foreach (Booking b in Bookings.Where(b => b.TableID == tableToEdit.ID))
                    {
                        b.TableID = editedTable.ID;
                    }
                }

                Tables.Remove(tableToEdit);
                int index = Tables.Select(t => t.ID).ToList().BinarySearch(editedTable.ID);

                if (index < 0)
                {
                    Tables.Insert(~index, editedTable);
                }
                else
                {
                    MessageBox.Show("Adding edited table failed - index >= 0");
                }
            }
        }

        #endregion

        #region Events

        private void AddTableButton_Click(object sender, RoutedEventArgs e)
        {
            NewTableDialog newTableDialog = new NewTableDialog(Tables);


            if (newTableDialog.ShowDialog() == true)
            {
                Table newTable = newTableDialog.NewTable;
                int index = Tables.Select(t => t.ID).ToList().BinarySearch(newTable.ID);

                if (index < 0)
                {
                    Tables.Insert(~index, newTable);
                }
                else
                {
                    MessageBox.Show("Adding new table failed, index >= 0");
                }
            }
        }

        private void EditTableButton_Click(object sender, RoutedEventArgs e)
        {
            if (TablesListView.SelectedIndex > -1)
            {
                RunEditTableDialog();
            }
        }

        private void DeleteTableButton_Click(object sender, RoutedEventArgs e)
        {
            if (TablesListView.SelectedIndex > -1)
            {
                if (MessageBox.Show("Delete this table? (Deletes all bookings on this table)", "Delete", MessageBoxButton.YesNo, MessageBoxImage.Question)
                    == MessageBoxResult.Yes)
                {
                    Table deletedTable = (Table)TablesListView.SelectedItem;
                    List<Booking> deletedBookings = Bookings.Where(b => b.TableID == deletedTable.ID).ToList();

                    foreach (Booking b in deletedBookings)
                    {
                        Bookings.Remove(b);
                    }

                    Tables.Remove(deletedTable);
                }
                else
                {
                    MessageBox.Show("Please select a table to delete.", "Select a table", MessageBoxButton.OK, MessageBoxImage.Exclamation);
                }
            }
        }

        private void ListViewItem_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            RunEditTableDialog();
        }

        #endregion

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Owner = null; //Prevents MainWindow from closing when this window is closed
        }
    }
}
