# ElegantSeries.SerialPort
This is a .Net System.IO.Ports extension library, designed to help simplify packet reading.
## Download
Package is  available at NuGet:

[![Nuget](https://img.shields.io/nuget/v/ElegantSeries.SerialPort)](https://www.nuget.org/packages/ElegantSeries.SerialPort/)
## Using the code
- Read fixed length data.
```csharp
SerialPort serialPort = new SerialPort();
byte[] bytes = new byte[10];
// default timeout 5000ms
serialPort.Read(bytes);
// custom timeout 1000ms
serialPort.Read(bytes, 1000);
```
- Read fixed length data, specify the first few bytes.
```csharp
SerialPort serialPort = new SerialPort();
byte[] bytes = new byte[10];
byte[] first = new byte[2];
first[0] = 0x0A;
first[1] = 0x0B;
serialPort.Read(bytes, first);
```
- Read fixed length data, specify the first few bytes and the last few bytes.
```csharp
SerialPort serialPort = new SerialPort();
byte[] bytes = new byte[10];
byte[] first = new byte[2];
first[0] = 0x0A;
first[1] = 0x0B;
byte[] last = new byte[2];
last[0] = 0xA0;
last[1] = 0xB0;
serialPort.Read(bytes, first, last);
```
- Cancel reading data.
```csharp
CancellationTokenSource cts = new CancellationTokenSource();
CancellationToken token = cts.Token;
SerialPort serialPort = new SerialPort();
byte[] bytes = new byte[10];
serialPort.Read(bytes, token:token);

// Execute on other thread
cts.Cancel();
```
## Using the Utilities
- ReuseBuffer Class.\
Multiplex the same packet. e.g., different methods use the same packet for data parsing.
```csharp
ReuseBuffer reuseBuffer = new() { Number = 2 };
SerialPort serialPort = new SerialPort();
byte[] bytes = new byte[10];
serialPort.Read(bytes);
reuseBuffer.Data = bytes;
var bytes1 = reuseBuffer.Data;
var bytes2 = reuseBuffer.Data;
var bytes3 = reuseBuffer.Data;
// bytes1 == bytes2 == bytes, bytes3 == null.
```