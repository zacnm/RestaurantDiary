using System;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using System.Windows.Markup;
using System.Windows.Media;

namespace RestaurantDiary
{
    public class EditBookingComparisonConverter : MarkupExtension, IMultiValueConverter
    {
        public EditBookingComparisonConverter()
        {
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            string result = null;
            if (values.Length >= 5)
            {

                int guests = (int)values[0];
                DateTime time = (DateTime)values[1];
                ObservableCollection<Booking> bookings = (ObservableCollection<Booking>)values[2];
                Table table = (Table)values[3];
                DateTime date = (DateTime)values[4];
                int seats = table.Seats;

                if (guests > seats || bookings.ToList().Exists(b => b.TableID == table.ID && b.Time.Date == date &&
                (b.Time.TimeOfDay <= time.AddHours(2).TimeOfDay && b.Time.TimeOfDay >= time.AddHours(-2).TimeOfDay)))
                {
                    result = "Unavailable";
                }
                else if (guests == seats)
                {
                    result = "Perfect";
                }
                else
                {
                    result = "Available";
                }
            }

            return result;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }

    public class ShowTimeComboBoxItemConverter : MarkupExtension, IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length >= 2)
            {
                int shiftIndex = (int)values[0];
                DateTime time = (DateTime)values[1];
                TimeSpan start;
                TimeSpan end;

                if (shiftIndex == 0)
                {
                    start = new TimeSpan(12, 0, 0);
                    end = new TimeSpan(15, 0, 0);
                }
                else
                {
                    start = new TimeSpan(18, 0, 0);
                    end = new TimeSpan(22, 0, 0);
                }

                if (time.TimeOfDay >= start && time.TimeOfDay <= end)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return null;
            }

        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }

    public class ShowListViewItemConverter : MarkupExtension, IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length >= 3)
            {
                DateTime date = (DateTime)values[0];
                int timeIndex = (int)values[1];
                Booking booking = (Booking)values[2];

                TimeSpan[,] shiftTimes = new TimeSpan[2, 2] { { new TimeSpan(12, 0, 0), new TimeSpan(15, 0, 0)},
                    { new TimeSpan(18, 0, 0), new TimeSpan(22, 0, 0)} };

                if (booking.Time.Date == date && booking.Time.TimeOfDay >= shiftTimes[timeIndex, 0]
                    && booking.Time.TimeOfDay <= shiftTimes[timeIndex, 1])
                {
                    return true;
                }
                else return false;
            }
            else return null;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }

    public class NewBookingComparisonConverter : MarkupExtension, IMultiValueConverter
    {
        public NewBookingComparisonConverter()
        {
        }

        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            string result = null;
            if (values.Length >= 5)
            {
                int guests = (int)values[0];
                DateTime time = (DateTime)values[1];
                ObservableCollection<Booking> bookings = (ObservableCollection<Booking>)values[2];
                Table table = (Table)values[3];
                DateTime date = (DateTime)values[4];
                int seats = table.Seats;

                if (guests > seats || bookings.ToList().Exists(b => b.TableID == table.ID && b.Time.Date == date &&
                (b.Time.TimeOfDay <= time.AddHours(2).TimeOfDay && b.Time.TimeOfDay >= time.AddHours(-2).TimeOfDay)))
                {
                    result = "Unavailable";
                }
                else if (guests == seats)
                {
                    result = "Perfect";
                }
                else
                {
                    result = "Available";
                }
            }

            return result;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }

    /// <summary>
    /// Converts a DateTime object into a string showing only the time on a 12 hour clock with no AM/PM or excess zeroes
    /// </summary>
    public class DateTimeToTimeStringConverter : MarkupExtension, IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            string result = "";

            if (value != null)
            {
                DateTime time = (DateTime)value;
                string temp = time.ToShortTimeString();
                return temp.Remove(temp.LastIndexOf('M') - 1);
            }

            return result;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }

    public class NewBookingButtonEnabledConverter : MarkupExtension, IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            bool result = false;

            if (values.Length >= 2)
            {
                string nameText = (string)values[0];
                int tableIndex = (int)values[1];

                if (nameText != "" && tableIndex > -1) result = true;
            }

            return result;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }

    public class EditBookingButtonEnabledConverter : MarkupExtension, IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            bool result = false;

            if (values.Length >= 1)
            {
                string nameText = (string)values[0];

                if (nameText != "") result = true;
            }

            return result;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }

    public class NewTableButtonEnabledConverter : MarkupExtension, IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            bool result = false;

            if (values.Length >= 3)
            {
                string tableIDText = (string)values[0];
                string seatsText = (string)values[1];
                SolidColorBrush color = (SolidColorBrush)values[2];

                if (tableIDText != "" && seatsText != "" && color != Brushes.Red) result = true;
            }

            return result;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }

    public class TableIDTextBoxColourConverter : MarkupExtension, IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            bool result = false;

            if (values.Length >= 2)
            {
                string text = (string)values[0];
                int ID;
                ObservableCollection<Table> tables = (ObservableCollection<Table>)values[1];
                if (text != "")
                {
                    try
                    {
                        ID = int.Parse(text);
                    }
                    catch (FormatException)
                    {
                        return result;
                    }

                    if (tables.ToList().Exists(t => t.ID == ID)) result = true;
                }
            }

            return result;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return this;
        }
    }
}
