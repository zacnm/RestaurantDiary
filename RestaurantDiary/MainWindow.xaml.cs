using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Xml.Serialization;
using System.Windows;
using System.Windows.Input;
using System.Collections.ObjectModel;

namespace RestaurantDiary
{
    public partial class MainWindow
    {
        #region Fields

        string saveDirectoryPath =Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\RestaurantDiary";

        ObservableCollection<Table> Tables = new ObservableCollection<Table>();

        List<DateTime> lunchTimesList = new List<DateTime>();
        List<DateTime> dinnerTimesList = new List<DateTime>();
        List<DateTime>[] TimesArray;

        #endregion

        #region Properties

        public ObservableCollection<Booking> Bookings { get; set; } = new ObservableCollection<Booking>();

        #endregion

        #region Constructors

        public MainWindow()
        {
            if (IsAppInstalled()) Load();
            SetupTimesLists();
            DataContext = this;
            InitializeComponent();
            Calendar.SelectedDate = DateTime.Now;
            if (DateTime.Now.Hour < 17) ShiftTimeComboBox.SelectedIndex = 0;
            else ShiftTimeComboBox.SelectedIndex = 1;
        }

        #endregion

        #region Methods

        public bool IsAppInstalled()
        {
            DirectoryInfo saveDirectory = new DirectoryInfo(saveDirectoryPath);

            if (saveDirectory.Exists)
            {
                return true;
            }
            else
            {
                if (MessageBox.Show("Perform first time setup?", "Restaurant Diary Setup", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                {
                    Directory.CreateDirectory(saveDirectoryPath);
                    Save();
                }
                else Environment.Exit(1);
                return false;
            }
        }

        public void SetupTimesLists()
        {
            DateTime time = new DateTime(1, 1, 1, 12, 0, 0);
            lunchTimesList.Add(time);
            for (int i = 0; i < 12; i++)
            {
                time = time.AddMinutes(15);
                lunchTimesList.Add(time);
            }

            time = new DateTime(1, 1, 1, 18, 0, 0);
            dinnerTimesList.Add(time);
            for (int i = 0; i < 14; i++)
            {
                time = time.AddMinutes(15);
                dinnerTimesList.Add(time);
            }

            TimesArray = new List<DateTime>[] { lunchTimesList, dinnerTimesList };
        }

        public void SetupDummyData()
        {

            Tables.Add(new Table(1, 4));
            Tables.Add(new Table(2, 6));
            Tables.Add(new Table(3, 2));
            Tables.Add(new Table(4, 2));
            Tables.Add(new Table(5, 4));
            Tables.Add(new Table(6, 4));
            Tables.Add(new Table(7, 4));
            Tables.Add(new Table(8, 6));
            Tables.Add(new Table(9, 4));
            Tables.Add(new Table(10, 4));
            Tables.Add(new Table(11, 6));
            Tables.Add(new Table(12, 2));
            Tables.Add(new Table(13, 4));
            Tables.Add(new Table(14, 4));
            Tables.Add(new Table(15, 4));
            Tables.Add(new Table(16, 4));
            Tables.Add(new Table(17, 2));

            Bookings.Add(new Booking()
            {
                Name = "Johnson",
                ContactInfo = "123",
                Guests = 2,
                TableID = 1,
                Time = new DateTime(2017, 12, 1, 15, 0, 0)
            });
            Bookings.Add(new Booking()
            {
                Name = "Golino",
                ContactInfo = "324",
                Guests = 3,
                TableID = 2,
                Time = new DateTime(2017, 12, 1, 14, 0, 0),
                Notes = "All food must be welsh"
            });

            Bookings.Add(new Booking()
            {
                Name = "Holley",
                ContactInfo = "666",
                Guests = 4,
                TableID = 3,
                Time = new DateTime(2017, 12, 1, 19, 30, 0)
            });
            Bookings.Add(new Booking()
            {
                Name = "Dobrowolski",
                ContactInfo = "435",
                Guests = 2,
                TableID = 1,
                Time = new DateTime(2017, 12, 1, 20, 0, 0)
            });

            Bookings.Add(new Booking()
            {
                Name = "Tsang",
                ContactInfo = "212",
                Guests = 2,
                TableID = 3,
                Time = new DateTime(2017, 12, 2, 13, 30, 0)
            });
            Bookings.Add(new Booking()
            {
                Name = "Akue",
                ContactInfo = "616",
                Guests = 4,
                TableID = 2,
                Time = new DateTime(2017, 12, 2, 15, 15, 0),
                Notes = "All food seasoned appropriately"
            });

            Bookings.Add(new Booking()
            {
                Name = "Packwood",
                ContactInfo = "111",
                Guests = 2,
                TableID = 3,
                Time = new DateTime(2017, 12, 2, 19, 30, 0)
            });
        } //Defines a set of test data

        ///<summary>
        ///Loads and deserializes the Bookings and Tables lists from bookings.xml and tables.xml.
        ///</summary>
        public void Load()
        {
            TextReader reader = null;
            try
            {
                var serializer = new XmlSerializer(typeof(List<Booking>)); //ObservableCollection is not serializable so must convert to serializable list
                reader = new StreamReader(saveDirectoryPath + @"\bookings.xml");
                Bookings = new ObservableCollection<Booking>(((List<Booking>)serializer.Deserialize(reader)).OrderBy(b => b.Time));
            }
            catch (Exception e)
            {
                MessageBox.Show(this, e.Message, "Load error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                reader?.Close();
            }

            try
            {
                var serializer = new XmlSerializer(typeof(List<Table>));
                reader = new StreamReader(saveDirectoryPath + @"\tables.xml");
                Tables = new ObservableCollection<Table>(((List<Table>)serializer.Deserialize(reader)).OrderBy(t => t.ID));
            }
            catch (Exception e)
            {
                MessageBox.Show(this, e.Message, "Load error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            finally
            {
                reader?.Close();
            }
        }

        ///<summary>
        ///Serializes and saves the Bookings and Tables lists to bookings.xml and tables.xml.
        /// </summary>
        private bool Save()
        {
            bool success = true;
            TextWriter writer = null;
            try
            {
                var serializer = new XmlSerializer(typeof(ObservableCollection<Booking>));
                writer = new StreamWriter(saveDirectoryPath + @"\bookings.xml");
                serializer.Serialize(writer, Bookings);
            }
            catch
            {
                success = false;
            }
            finally
            {
                writer?.Close();
            }

            try
            {
                var serializer = new XmlSerializer(typeof(ObservableCollection<Table>));
                writer = new StreamWriter(saveDirectoryPath + @"\tables.xml");
                serializer.Serialize(writer, Tables);
            }
            catch
            {
                success = false;
            }
            finally
            {
                writer?.Close();
            }
            return success;
        }

        private void RunEditDialog()
        {
            EditBookingDialog editBookingDialog = new EditBookingDialog(Bookings, BookingsListView.SelectedIndex,
                ShiftTimeComboBox.SelectedIndex, Tables, TimesArray[0].Concat(TimesArray[1]).ToList())
            {
                Owner = this,
                WindowStartupLocation = WindowStartupLocation.CenterOwner
            };

            if (editBookingDialog.ShowDialog() == true)
            {
                Bookings.Remove(Bookings[BookingsListView.SelectedIndex]); //Remove edited booking before reinserting it in correct position

                Booking editedBooking = editBookingDialog.EditedBooking;
                int index = Bookings.Select(b => b.Time).ToList().BinarySearch(editedBooking.Time);

                if (index < 0)
                {
                    Bookings.Insert(~index, editedBooking); //Inserts booking in correct position according to Time property
                }
                else
                {
                    MessageBox.Show("Adding edited booking failed - index >= 0"); //Error message which SHOULD never see the light of day
                }
            }
        }

        #endregion

        #region Events

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (Save()) MessageBox.Show("Save successful.", "Save", MessageBoxButton.OK, MessageBoxImage.Information);
            else MessageBox.Show("Save unsuccessful.", "Save", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void AddBookingButton_Click(object sender, RoutedEventArgs e)
        {
            if (Tables.Count > 0)
            {
                NewBookingDialog newBookingDialog = new NewBookingDialog(
                    new ObservableCollection<Booking>(
                        Bookings.Where(b => b.Time.Date == Calendar.SelectedDate).ToList()),
                    Tables, TimesArray[ShiftTimeComboBox.SelectedIndex], (DateTime) Calendar.SelectedDate)
                {
                    Owner = this,
                    WindowStartupLocation = WindowStartupLocation.CenterOwner
                };

                if (newBookingDialog.ShowDialog() == true)
                {
                    Booking newBooking = newBookingDialog.NewBooking;
                    int index = Bookings.Select(b => b.Time).ToList().BinarySearch(newBooking.Time);

                    if (index < 0)
                    {
                        Bookings.Insert(~index, newBooking);
                    }
                    else
                    {
                        MessageBox.Show("Adding new booking failed - index >= 0");
                    }
                }
            }
            else
                MessageBox.Show("Please define tables before creating a booking.", "Cannot add booking",
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }

        private void BookingsListView_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            RunEditDialog();
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if (BookingsListView.SelectedIndex != -1)
            {
                RunEditDialog();
            }
            else
            {
                MessageBox.Show("Please select a booking to edit.", "Select a booking",
                    MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (BookingsListView.SelectedIndex != -1)
            {
                if (MessageBox.Show("Delete this booking?", "Delete", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
                    Bookings.Remove((Booking)BookingsListView.SelectedItem);
            }
            else
            {
                MessageBox.Show("Please select a booking to delete.", "Select a booking", MessageBoxButton.OK, MessageBoxImage.Exclamation);
            }
        }

        private void EditTablesButton_Click(object sender, RoutedEventArgs e)
        {
            EditTablesWindow editTablesWindow = new EditTablesWindow(Tables, Bookings)
            {
                Owner = this
            };
            editTablesWindow.Left = editTablesWindow.Owner.Left - editTablesWindow.Width;
            editTablesWindow.Top = editTablesWindow.Owner.Top;

            editTablesWindow.Show();
        }

        #endregion
    }

}