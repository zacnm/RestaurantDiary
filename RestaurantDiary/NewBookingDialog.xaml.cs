using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace RestaurantDiary
{
    public partial class NewBookingDialog : Window
    {
        #region Fields

        public Booking NewBooking;

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
        public DateTime Date
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

        public NewBookingDialog(ObservableCollection<Booking> bookings, ObservableCollection<Table> tables, List<DateTime> times, DateTime date)
        {
            InitializeComponent();
            Date = date;
            Bookings = bookings;
            Tables = tables;
            Times = times;
            Guests = new List<int>();
            for (int i = 1; i <= Tables.Max(t => t.Seats); i++) Guests.Add(i);
            DataContext = this;
        }

        #endregion

        #region Events

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (NameTextBox.Text != "" && TableComboBox.SelectedIndex > -1) //Checks that booking has a name and table assigned
            {
                DateTime selectedTime = (DateTime)TimeComboBox.SelectedItem;
                DateTime bookingDateTime = new DateTime(Date.Year, Date.Month, Date.Day, selectedTime.Hour, selectedTime.Minute, 0);

                NewBooking = new Booking(NameTextBox.Text, (int)GuestsComboBox.SelectedItem, ContactInfoTextBox.Text,
                    ((Table)TableComboBox.SelectedItem).ID, bookingDateTime, NotesTextBox.Text);

                DialogResult = true;
            }
            else MessageBox.Show("Please input name and table.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        #endregion
    }
}
