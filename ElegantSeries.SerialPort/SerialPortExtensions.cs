using ElegantSeries.Utilities;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ElegantSeries
{
    public static class SerialPortExtensions
    {
        public static void Read(this SerialPort serialPort, byte[] readBuffer, int readTimeout = 5000, CancellationToken token = default)
        {
            if (readBuffer == null)
                throw new ArgumentNullException(nameof(readBuffer));

            int readBufferLength = readBuffer.Length;
            long startTicks = Environment.TickCount;
            do
            {
                if (token.IsCancellationRequested)
                    throw new OperationCanceledException();

                if (serialPort.BytesToRead >= readBufferLength)
                {
                    int justRead = serialPort.Read(readBuffer, 0, readBufferLength);
                    if (justRead != readBufferLength)
                    {
                        throw new Exception("Expected and actual values are not equal");
                    }
                    return;
                }
            } while (readTimeout == SerialPort.InfiniteTimeout || readTimeout - (Environment.TickCount - startTicks) > 0);
            throw new TimeoutException();
        }
        public static void Read(this SerialPort serialPort, byte[] readBuffer, byte[] first, int readTimeout = 5000, CancellationToken token = default)
        {
            if (readBuffer == null)
                throw new ArgumentNullException(nameof(readBuffer));
            if (first == null)
                throw new ArgumentNullException(nameof(first));

            int readBufferLength = readBuffer.Length;
            int firstBufferLength = first.Length;

            if (firstBufferLength > readBufferLength)
                throw new ArgumentException($"{nameof(first)} length is more than {nameof(readBuffer)} length");

            long startTicks = Environment.TickCount;
            List<byte> bytes = new List<byte>();
            do
            {
                if (token.IsCancellationRequested)
                    throw new OperationCanceledException();

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
            } while (readTimeout == SerialPort.InfiniteTimeout || readTimeout - (Environment.TickCount - startTicks) > 0);
            throw new TimeoutException();
        }
        public static void Read(this SerialPort serialPort, byte[] readBuffer, byte[] first, byte[] last, int readTimeout = 5000, CancellationToken token = default)
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

            long startTicks = Environment.TickCount;
            List<byte> bytes = new List<byte>();
            do
            {
                if (token.IsCancellationRequested)
                    throw new OperationCanceledException();

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
            } while (readTimeout == SerialPort.InfiniteTimeout || readTimeout - (Environment.TickCount - startTicks) > 0);
            throw new TimeoutException();
        }
        public static void WriteAndRead(this SerialPort serialPort, byte[] writeBuffer, byte[] readBuffer, int readTimeout = 5000, int writeTimeout = 2000, CancellationToken token = default)
        {
            if (writeBuffer == null)
                throw new ArgumentNullException(nameof(writeBuffer));
            if (readBuffer == null)
                throw new ArgumentNullException(nameof(readBuffer));

            serialPort.WriteTimeout = writeTimeout;
            serialPort.Write(writeBuffer, 0, writeBuffer.Length);
            serialPort.Read(readBuffer, readTimeout, token);
        }
        public static void WriteAndRead(this SerialPort serialPort, byte[] writeBuffer, byte[] readBuffer, byte[] first, int readTimeout = 5000, int writeTimeout = 2000, CancellationToken token = default)
        {
            if (writeBuffer == null)
                throw new ArgumentNullException(nameof(writeBuffer));
            if (readBuffer == null)
                throw new ArgumentNullException(nameof(readBuffer));
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (first.Length > readBuffer.Length)
                throw new ArgumentException($"{nameof(first)} length is more than {nameof(readBuffer)} length");

            serialPort.WriteTimeout = writeTimeout;
            serialPort.Write(writeBuffer, 0, writeBuffer.Length);
            serialPort.Read(readBuffer, first, readTimeout, token);
        }
        public static void WriteAndRead(this SerialPort serialPort, byte[] writeBuffer, byte[] readBuffer, byte[] first, byte[] last, int readTimeout = 5000, int writeTimeout = 2000, CancellationToken token = default)
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

            serialPort.WriteTimeout = writeTimeout;
            serialPort.Write(writeBuffer, 0, writeBuffer.Length);
            serialPort.Read(readBuffer, first, last, readTimeout, token);
        }
        public static async Task ReadAsync(this SerialPort serialPort, byte[] readBuffer, int readTimeout, CancellationToken token = default)
        {
            await Task.Run(() => serialPort.Read(readBuffer, readTimeout, token));
        }
        public static async Task ReadAsync(this SerialPort serialPort, byte[] readBuffer, byte[] first, int readTimeout = 5000, CancellationToken token = default)
        {
            await Task.Run(() => serialPort.Read(readBuffer, first, readTimeout, token));
        }
        public static async Task ReadAsync(this SerialPort serialPort, byte[] readBuffer, byte[] first, byte[] last, int readTimeout = 5000, CancellationToken token = default)
        {
            await Task.Run(() => serialPort.Read(readBuffer, first, last, readTimeout, token));
        }
        public static async Task WriteAndReadAsync(this SerialPort serialPort, byte[] writeBuffer, byte[] readBuffer, int readTimeout, int writeTimeout = 2000, CancellationToken token = default)
        {
            await Task.Run(() => serialPort.WriteAndRead(writeBuffer, readBuffer, readTimeout, writeTimeout, token));
        }
        public static async Task WriteAndReadAsync(this SerialPort serialPort, byte[] writeBuffer, byte[] readBuffer, byte[] first, int readTimeout = 5000, int writeTimeout = 2000, CancellationToken token = default)
        {
            await Task.Run(() => serialPort.WriteAndRead(writeBuffer, readBuffer, first, readTimeout, writeTimeout, token));
        }
        public static async Task WriteAndReadAsync(this SerialPort serialPort, byte[] writeBuffer, byte[] readBuffer, byte[] first, byte[] last, int readTimeout = 5000, int writeTimeout = 2000, CancellationToken token = default)
        {
            await Task.Run(() => serialPort.WriteAndRead(writeBuffer, readBuffer, first, last, readTimeout, writeTimeout, token));
        }

        /// <summary>
        /// Match multiple protocols at the same time, and return the first matching protocol.
        /// </summary>
        /// <param name="serialPort"></param>
        /// <param name="protocols"></param>
        /// <param name="readTimeout"></param>
        /// <param name="token"></param>
        /// <returns name="number">Incoming protocol number</returns>
        /// <returns name="data">Received data</returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="OperationCanceledException"></exception>
        /// <exception cref="TimeoutException"></exception>
        public static  (int number, byte[] data) ReadSingle(this SerialPort serialPort, List<Protocol> protocols, int readTimeout = 5000, CancellationToken token = default(CancellationToken))
        {
            if (protocols == null)
            {
                throw new ArgumentNullException("protocolPackages");
            }

            List<ProtocolPackage> protocolPackages = new List<ProtocolPackage>();
            foreach (Protocol protocol in protocols)
            {
                if (protocol.DataLength <= 0)
                {
                    continue;
                }

                if (protocol.First != null)
                {
                    if (protocol.First.Length > protocol.DataLength)
                    {
                        continue;
                    }
                }

                if (protocol.Last != null)
                {
                    if (protocol.Last.Length > protocol.DataLength)
                    {
                        continue;
                    }
                }

                if (protocol.First != null && protocol.Last != null)
                {
                    if (protocol.First.Length + protocol.Last.Length > protocol.DataLength)
                    {
                        continue;
                    }
                }

                var pp = new ProtocolPackage() { Number = protocol.Number, DataLength = protocol.DataLength };
                pp.DataBuffer = new char[protocol.DataLength];
                for (int i = 0; i < pp.DataBuffer.Length; i++)
                {
                    pp.DataBuffer[i] = (char)0x8000;
                }
                //Array.Fill(pp.DataBuffer, (char)0x8000);
                if (protocol.First != null)
                {
                    for (int i = 0; i < protocol.First.Length; i++)
                    {
                        pp.DataBuffer[i] = (char)protocol.First[i];
                    }
                    pp.IsMatch = true;
                }
                if (protocol.Last != null)
                {
                    for (int i = 0; i < protocol.Last.Length; i++)
                    {
                        pp.DataBuffer[protocol.DataLength - protocol.Last.Length + i] = (char)protocol.Last[i];
                    }
                    pp.IsMatch = true;
                }
                protocolPackages.Add(pp);
            }
            if (protocolPackages.Count == 0)
                return (-1, null);

            long tickCount = Environment.TickCount;
            List<byte> list = new List<byte>();
            do
            {
                if (token.IsCancellationRequested)
                {
                    throw new OperationCanceledException();
                }

                if (serialPort.BytesToRead > 0)
                {
                    int num = serialPort.ReadByte();
                    list.Add((byte)num);

                    foreach (var protocolPackage in protocolPackages)
                    {
                        if (protocolPackage.DataLength > list.Count - protocolPackage.Index)
                            continue;

                        bool exist = false;
                        int index = protocolPackage.Index;
                        List<byte> value = new List<byte>();

                        if (!protocolPackage.IsMatch)
                        {
                            for (int i = 0; i < protocolPackage.DataLength; i++)
                            {
                                protocolPackage.DataBuffer[i] = (char)list[i + index];
                                value.Add(list[i + index]);
                            }
                            return (protocolPackage.Number, value.ToArray());
                        }

                        for (int i = 0; i < protocolPackage.DataLength; i++)
                        {
                            bool match = false;
                            int j;
                            for (j = index; j < list.Count; j++)
                            {
                                if (protocolPackage.DataBuffer[i] == (char)0x8000)
                                {
                                    value.Add(list[j]);
                                    index = j + 1;
                                    match = true;
                                    exist = true;
                                    break;
                                }
                                else
                                {
                                    if (protocolPackage.DataBuffer[i] == list[j])
                                    {
                                        if (value.Count == 0)
                                        {
                                            protocolPackage.Index = j;
                                        }
                                        value.Add(list[j]);
                                        index = j + 1;
                                        match = true;
                                        exist = true;
                                        break;
                                    }
                                    else if (exist)
                                    {
                                        break;
                                    }
                                }
                            }

                            if (!match)
                            {
                                if (!exist)
                                {
                                    protocolPackage.Index = j;
                                }
                                else
                                {
                                    if (j < list.Count)
                                    {
                                        protocolPackage.Index++;
                                    }
                                }
                                break;
                            }
                        }
                        if (value.Count == protocolPackage.DataLength)
                        {
                            for (int i = 0; i < value.Count; i++)
                            {
                                protocolPackage.DataBuffer[i] = (char)value[i];
                            }
                            return (protocolPackage.Number, value.ToArray());
                        }
                    }
                }

                //list.Clear();
            }
            while (readTimeout == -1 || readTimeout - (Environment.TickCount - tickCount) > 0);
            throw new TimeoutException();
        }
    }
}