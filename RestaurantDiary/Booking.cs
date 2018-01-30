using System;
using System.ComponentModel;

namespace RestaurantDiary
{
    public class Booking : INotifyPropertyChanged
    {
        #region Fields

        private int guests;
        private int tableID;

        #endregion

        #region Properties

        public string Name { get; set; }

        public int Guests
        {
            get => guests;
            set
            {
                if (value > 0) guests = value;
            }
        }
        public string ContactInfo { get; set; }

        public int TableID
        {
            get => tableID;
            set
            {
                if (value >= 0) tableID = value;
                NotifyPropertyChanged("TableID");
            }
        }
        public DateTime Time { get; set; }

        public string Notes { get; set; }

        public string ShortTime // Converts a DateTime object into a string showing only the time on a 12 hour clock with no AM/PM or excess zeroes
        {
            get
            {
                string temp = Time.ToShortTimeString();
                return temp.Remove(temp.LastIndexOf('M') - 1);
            }
        }

        #endregion

        #region Constructors

        public Booking(string name, int guests, string contactinfo, int tableID, DateTime time) :
            this(name, guests, contactinfo, tableID, time, "")
        {
        }

        public Booking(string name, int guests, string contactinfo, int tableID, DateTime time, string notes)
        {

            Name = name;
            Guests = guests;
            ContactInfo = contactinfo;
            TableID = tableID;
            Time = time;
            Notes = notes;
        }

        public Booking()
        {
        }

        #endregion

        #region INotifyProperptyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
