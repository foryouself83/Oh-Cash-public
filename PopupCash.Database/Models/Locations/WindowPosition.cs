namespace PopupCash.Database.Models.Locations
{
    public class WindowPosition
    {
        public string Id { get; set; }
        public double Left { get; set; }
        public double Top { get; set; }

        public WindowPosition()
        {
            Id = string.Empty;
            Left = 0;
            Top = 0;
        }

        public WindowPosition(string id, double left, double top)
        {
            Id = id;
            Left = left;
            Top = top;
        }
    }
}
