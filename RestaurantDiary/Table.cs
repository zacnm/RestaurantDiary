namespace RestaurantDiary
{
    public class Table
    { 
        #region Properties

        public int ID { get; set; }

        public int Seats { get; set; }

        #endregion

        #region Constructors

        public Table(int id, int seats)
        {
            ID = id;
            Seats = seats;
        }

        public Table()
        {
        }

        #endregion
    }
}