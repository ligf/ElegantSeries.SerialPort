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