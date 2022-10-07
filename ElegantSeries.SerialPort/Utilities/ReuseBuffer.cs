namespace ElegantSeries.Utilities
{
    public class ReuseBuffer
    {
        int index = 0;
        object? _Data;

        public int Number { get; set; } = 0;
        public object? Data
        {
            get
            {
                if (index == Number)
                {
                    _Data = null;
                }
                else
                {
                    index++;
                }
                return _Data;
            }
            set
            {
                _Data = value;
                index = 0;
            }
        }
    }
}
