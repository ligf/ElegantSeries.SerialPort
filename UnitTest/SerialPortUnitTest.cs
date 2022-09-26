using ElegantSeries;
using System.IO.Ports;
using System.Threading;

namespace UnitTest
{
    [TestClass]
    public class SerialPortUnitTest
    {
        [TestMethod]
        public void ReadFixedLength()
        {
            SerialPort serialPort = new SerialPort("com1");
            byte[] bytes = new byte[10];
            // default timeout 5000ms
            serialPort.Read(bytes);
            // custom timeout 1000ms
            serialPort.Read(bytes, 1000);
        }
        [TestMethod]
        public async void ReadFixedLengthAsync()
        {
            SerialPort serialPort = new SerialPort("com1");
            byte[] bytes = new byte[10];
            // default timeout 5000ms
            serialPort.Read(bytes);
            // custom timeout 1000ms
            await serialPort.ReadAsync(bytes, 1000);
        }
        [TestMethod]
        public void ReadFixedLengthWithFirst()
        {
            SerialPort serialPort = new SerialPort("com1");
            byte[] bytes = new byte[10];
            byte[] first = new byte[2];
            first[0] = 0x0A;
            first[1] = 0x0B;
            serialPort.Read(bytes, first);
        }
        [TestMethod]
        public void ReadFixedLengthWithFirstLast()
        {
            SerialPort serialPort = new SerialPort("com1");
            byte[] bytes = new byte[10];
            byte[] first = new byte[2];
            first[0] = 0x0A;
            first[1] = 0x0B;
            byte[] last = new byte[2];
            last[0] = 0xA0;
            last[1] = 0xB0;
            serialPort.Read(bytes, first, last);
        }
    }
}