using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace RestaurantDiary
{
    public partial class EditBookingDialog
    {
        #region Fields

        public Booking ActiveBooking;
        public Booking EditedBooking;

        #endregion

        #region Properties

        public ObservableCollection<Table> Tables
        {
            get;
            set;
        }
        public List<int> Guests
        {
            get;
            set;
        }
        public List<DateTime> Times
        {
            get;
            set;
        }
        public ObservableCollection<Booking> Bookings
        {
            get;
            set;
        }

        #endregion

        #region Constructors

        public EditBookingDialog(ObservableCollection<Booking> bookings, int bookingIndex, int shiftIndex, ObservableCollection<Table> tables, List<DateTime> times)
        {
            InitializeComponent();
            Bookings = new ObservableCollection<Booking>();
            foreach (Booking b in bookings) Bookings.Add(b); //Defines new List<Booking> so it can be changed without affecting original
            ActiveBooking = Bookings[bookingIndex];
            Bookings.Remove(ActiveBooking); //Removes booking being edited from Bookings list so its original table isn't counted as unavailable
            Tables = tables;
            Times = times;
            Guests = new List<int>();
            for (int i = 1; i <= Tables.Max(t => t.Seats); i++) Guests.Add(i);

            //Defines opening values of each element to match booking being edited
            NameTextBox.Text = ActiveBooking.Name;
            GuestsComboBox.SelectedIndex = ActiveBooking.Guests - 1;
            DateCalendar.SelectedDate = ActiveBooking.Time.Date;
            ShiftComboBox.SelectedIndex = shiftIndex;
            TimeComboBox.SelectedIndex = Times.IndexOf(Times.Find(t => t.TimeOfDay == ActiveBooking.Time.TimeOfDay));
            TableComboBox.SelectedItem = Tables.ToList().Find(t => t.ID == ActiveBooking.TableID);
            ContactInfoTextBox.Text = ActiveBooking.ContactInfo;
            NotesTextBox.Text = ActiveBooking.Notes;

            DataContext = this;
        }

        #endregion

        #region Events

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        //Checks that booking has a name and table assigned before saving is allowed
        {
            if (NameTextBox.Text != "" && TableComboBox.SelectedIndex > -1)
            {
                DateTime selectedTime = (DateTime)TimeComboBox.SelectedItem;
                DateTime selectedDate = (DateTime)DateCalendar.SelectedDate;
                DateTime bookingDateTime = new DateTime(selectedDate.Year, selectedDate.Month, selectedDate.Day, selectedTime.Hour, selectedTime.Minute, 0);

                EditedBooking = new Booking(NameTextBox.Text, (int)GuestsComboBox.SelectedItem, ContactInfoTextBox.Text,
                    ((Table)TableComboBox.SelectedItem).ID, bookingDateTime, NotesTextBox.Text);

                DialogResult = true;
            }
            else MessageBox.Show("Please input name and table.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void ShiftComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //Resets TimeComboBox to earliest available time when shift is changed
        {
            if (ShiftComboBox.SelectedIndex == 1)
            {
                TimeComboBox.SelectedItem = new DateTime(1, 1, 1, 18, 0, 0);
            }
            else
            {
                TimeComboBox.SelectedItem = new DateTime(1, 1, 1, 12, 0, 0);
            }
        }

        private void DateCalendar_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        //If the user changes the date to one where the selected table is not available, the TableComboBox is reset to index -1
        {
            if (DateCalendar.SelectedDate != null && TimeComboBox.SelectedIndex > -1 && TableComboBox.SelectedIndex > -1)
            {
                DateTime selectedDate = (DateTime)DateCalendar.SelectedDate;
                DateTime selectedTime = (DateTime)TimeComboBox.SelectedItem;
                DateTime newDateTime = new DateTime(selectedDate.Year, selectedDate.Month, selectedDate.Day,
                    selectedTime.Hour, selectedTime.Minute, selectedTime.Second);
                Table selectedTable = (Table)TableComboBox.SelectedItem;

                if (Bookings.ToList().Exists(b => newDateTime >= b.Time.AddHours(-2) && newDateTime <= b.Time.AddHours(2) &&
                b.TableID == selectedTable.ID))
                {
                    TableComboBox.SelectedIndex = -1;

                }
            }
        }

        private void GuestsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //If user changes guest number to one higher than the selected table's seats, TableComboBox is reset to index -1
        {
            if (GuestsComboBox.SelectedIndex > -1 && TableComboBox.SelectedIndex > -1)
            {
                if ((int)GuestsComboBox.SelectedItem > ((Table)TableComboBox.SelectedItem).Seats)
                {
                    TableComboBox.SelectedIndex = -1;
                }
            }
        }

        #endregion
    }
}