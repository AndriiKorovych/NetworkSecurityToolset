using PacketDotNet;
using SharpPcap;
using System;
using System.Net.Sockets;

class NetworkSniffer
{
    public static void StartSniffing()
    {
        Console.WriteLine("Доступні мережеві інтерфейси:");
        var devices = CaptureDeviceList.Instance;
        for (int i = 0; i < devices.Count; i++)
        {
            Console.WriteLine($"{i}: {devices[i].Description}");
        }

        Console.WriteLine("Оберіть інтерфейс для перехоплення:");
        int deviceIndex = int.Parse(Console.ReadLine());
        var device = devices[deviceIndex];

        device.OnPacketArrival += new PacketArrivalEventHandler(Device_OnPacketArrival);
        device.Open(DeviceMode.Promiscuous);
        Console.WriteLine("Слухаю трафік...");
        device.StartCapture();
    }

    private static void Device_OnPacketArrival(object sender, CaptureEventArgs e)
    {
        var packet = Packet.ParsePacket(e.Packet.LinkLayerType, e.Packet.Data);
        var tcpPacket = packet.Extract<TcpPacket>();
        var ipPacket = packet.Extract<IpPacket>();

        if (tcpPacket != null && ipPacket != null)
        {
            Console.WriteLine($"Отримано пакет від {ipPacket.SourceAddress} до {ipPacket.DestinationAddress}:{tcpPacket.DestinationPort}");

            if (tcpPacket.Syn && !tcpPacket.Ack)
            {
                string alertMessage = $"Виявлено сканування портів з IP: {ipPacket.SourceAddress}";
                Console.WriteLine($"[Загроза] {alertMessage}");
                NotificationManager.NotifyAdmin("Загроза в мережі", alertMessage);
            }
        }
    }
}
