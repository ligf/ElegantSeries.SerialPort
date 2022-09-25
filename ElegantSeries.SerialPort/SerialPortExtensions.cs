using System.IO.Ports;
namespace ElegantSeries;

public static class SerialPortExtensions
{
    public static void Read(this SerialPort serialPort, byte[] readBuffer, int readTimeout = 5000)
    {
        if (readBuffer == null)
            throw new ArgumentNullException(nameof(readBuffer));

        int readBufferLength = readBuffer.Length;
        long startTicks = Environment.TickCount64;
        do
        {
            if (serialPort.BytesToRead >= readBufferLength)
            {
                int justRead = serialPort.Read(readBuffer, 0, readBufferLength);
                if (justRead != readBufferLength)
                {
                    throw new Exception("Expected and actual values are not equal");
                }
                return;
            }
        } while (readTimeout == SerialPort.InfiniteTimeout || readTimeout - (Environment.TickCount64 - startTicks) > 0);
        throw new TimeoutException();
    }
    public static void Read(this SerialPort serialPort, byte[] readBuffer, byte[] first, int readTimeout = 5000)
    {
        if (readBuffer == null)
            throw new ArgumentNullException(nameof(readBuffer));
        if (first == null)
            throw new ArgumentNullException(nameof(first));

        int readBufferLength = readBuffer.Length;
        int firstBufferLength = first.Length;

        if (firstBufferLength > readBufferLength)
            throw new ArgumentException($"{nameof(first)} length is more than {nameof(readBuffer)} length");

        long startTicks = Environment.TickCount64;
        List<byte> bytes = new();
        do
        {
            if (serialPort.BytesToRead >= readBufferLength)
            {
                int justRead;
                for (int i = 0; i < firstBufferLength; i++)
                {
                    justRead = serialPort.ReadByte();
                    if (justRead == first[i])
                    {
                        bytes.Add((byte)justRead);
                    }
                    else
                    {
                        bytes.Clear();
                        goto Reset;
                    }
                }
                for (int i = 0; i < readBufferLength - firstBufferLength; i++)
                {
                    justRead = serialPort.ReadByte();
                    bytes.Add((byte)justRead);
                }
                
                for (int i = 0; i < bytes.Count; i++)
                {
                    readBuffer[i] = bytes.ElementAt(i);
                }
                return;
            }
        Reset:;
        } while (readTimeout == SerialPort.InfiniteTimeout || readTimeout - (Environment.TickCount64 - startTicks) > 0);
        throw new TimeoutException();
    }
    public static void Read(this SerialPort serialPort, byte[] readBuffer, byte[] first, byte[] last, int readTimeout = 5000)
    {
        if (readBuffer == null)
            throw new ArgumentNullException(nameof(readBuffer));
        if (first == null)
            throw new ArgumentNullException(nameof(first));
        if (last == null)
            throw new ArgumentNullException(nameof(last));

        int readBufferLength = readBuffer.Length;
        int firstBufferLength = first.Length;
        int lastBufferLength = last.Length;

        if (firstBufferLength + lastBufferLength > readBufferLength)
            throw new ArgumentException($"{nameof(first)} length add {nameof(last)} length is more than {nameof(readBuffer)} length");

        long startTicks = Environment.TickCount64;
        List<byte> bytes = new();
        do
        {
            if (serialPort.BytesToRead >= readBufferLength)
            {
                int justRead;
                for (int i = 0; i < firstBufferLength; i++)
                {
                    justRead = serialPort.ReadByte();
                    if (justRead == first[i])
                    {
                        bytes.Add((byte)justRead);
                    }
                    else
                    {
                        bytes.Clear();
                        goto Reset;
                    }
                }
                for (int i = 0; i < readBufferLength - firstBufferLength - lastBufferLength; i++)
                {
                    justRead = serialPort.ReadByte();
                    bytes.Add((byte)justRead);
                }

                for (int i = 0; i < lastBufferLength; i++)
                {
                    justRead = serialPort.ReadByte();
                    if (justRead == last[i])
                    {
                        bytes.Add((byte)justRead);
                    }
                    else
                    {
                        bytes.Clear();
                        goto Reset;
                    }
                }
                for (int i = 0; i < bytes.Count; i++)
                {
                    readBuffer[i] = bytes.ElementAt(i);
                }
                return;
            }
        Reset:;
        } while (readTimeout == SerialPort.InfiniteTimeout || readTimeout - (Environment.TickCount64 - startTicks) > 0);
        throw new TimeoutException();
    }
    public static void WriteAndRead(this SerialPort serialPort, byte[] writeBuffer, byte[] readBuffer, int readTimeout = 5000)
    {
        if (writeBuffer == null)
            throw new ArgumentNullException(nameof(writeBuffer));
        if (readBuffer == null)
            throw new ArgumentNullException(nameof(readBuffer));

        serialPort.Write(writeBuffer, 0, writeBuffer.Length);
        serialPort.Read(readBuffer, readTimeout);
    }
    public static void WriteAndRead(this SerialPort serialPort, byte[] writeBuffer, byte[] readBuffer, byte[] first, int readTimeout = 5000)
    {
        if (writeBuffer == null)
            throw new ArgumentNullException(nameof(writeBuffer));
        if (readBuffer == null)
            throw new ArgumentNullException(nameof(readBuffer));
        if (first == null)
            throw new ArgumentNullException(nameof(first));
        if (first.Length > readBuffer.Length)
            throw new ArgumentException($"{nameof(first)} length is more than {nameof(readBuffer)} length");

        serialPort.Write(writeBuffer, 0, writeBuffer.Length);
        serialPort.Read(readBuffer, first, readTimeout);
    }
    public static void WriteAndRead(this SerialPort serialPort, byte[] writeBuffer, byte[] readBuffer, byte[] first, byte[] last, int readTimeout = 5000)
    {
        if (writeBuffer == null)
            throw new ArgumentNullException(nameof(writeBuffer));
        if (readBuffer == null)
            throw new ArgumentNullException(nameof(readBuffer));
        if (first == null)
            throw new ArgumentNullException(nameof(first));
        if (last == null)
            throw new ArgumentNullException(nameof(last));
        if (first.Length + last.Length > readBuffer.Length)
            throw new ArgumentException($"{nameof(first)} length add {nameof(last)} length is more than {nameof(readBuffer)} length");

        serialPort.Write(writeBuffer, 0, writeBuffer.Length);
        serialPort.Read(readBuffer, first, last, readTimeout);
    }
    public static async Task ReadAsync(this SerialPort serialPort, byte[] readBuffer, int readTimeout)
    {
        await Task.Run(() => serialPort.Read(readBuffer, readTimeout));
    }
    public static async Task ReadAsync(this SerialPort serialPort, byte[] readBuffer, byte[] first, int readTimeout = 5000)
    {
        await Task.Run(() => serialPort.Read(readBuffer, first, readTimeout));
    }
    public static async Task ReadAsync(this SerialPort serialPort, byte[] readBuffer, byte[] first, byte[] last, int readTimeout = 5000)
    {
        await Task.Run(() => serialPort.Read(readBuffer, first, last, readTimeout));
    }
    public static async Task WriteAndReadAsync(this SerialPort serialPort, byte[] writeBuffer, byte[] readBuffer, int readTimeout)
    {
        await Task.Run(() => serialPort.WriteAndRead(writeBuffer, readBuffer, readTimeout));
    }
    public static async Task WriteAndReadAsync(this SerialPort serialPort, byte[] writeBuffer, byte[] readBuffer, byte[] first, int readTimeout = 5000)
    {
        await Task.Run(() => serialPort.WriteAndRead(writeBuffer, readBuffer, first, readTimeout));
    }
    public static async Task WriteAndReadAsync(this SerialPort serialPort, byte[] writeBuffer, byte[] readBuffer, byte[] first, byte[] last, int readTimeout = 5000)
    {
        await Task.Run(() => serialPort.WriteAndRead(writeBuffer, readBuffer, first, last, readTimeout));
    }
}