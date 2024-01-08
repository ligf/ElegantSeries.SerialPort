namespace ElegantSeries.Utilities
{
    public class Protocol
    {
        /// <summary>
        /// Protocol number. Example: 0, 1, ...
        /// </summary>
        public int Number { get; set; }
        public int DataLength { get; set; }
        public byte[] First { get; set; }
        public byte[] Last { get; set; }
    }

    internal class ProtocolPackage
    {
        /// <summary>
        /// Protocol number
        /// </summary>
        public int Number { get; set; }
        public char[] DataBuffer { get; set; }
        public int DataLength { get; set; }
        public int Index { get; set; }
        public bool IsMatch { get; set; }
    }
}
