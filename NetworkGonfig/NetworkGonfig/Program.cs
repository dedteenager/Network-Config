using System.Net.NetworkInformation;
using System.Net.Sockets;


namespace NetworkConfiguration
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Настройка сети");
            Console.WriteLine();
            NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface N in interfaces)
            {
                
                if (N.OperationalStatus == OperationalStatus.Up && N.Supports(NetworkInterfaceComponent.IPv4))
                {
                    Console.WriteLine($"Настройка сетевого интерфейса: {N.Name}");
                    N.EnableDhcp();
                    N.Speed = Int64.MaxValue;

                    var networks = N.GetIPProperties().UnicastAddresses;

                    foreach (var network in networks)
                    {
                        if (!network.IPv4Mask.Equals(System.Net.IPAddress.None))
                        {
                            try
                            {
                                TcpClient client = new TcpClient();
                                client.Connect(network.Address, 80);

                                Console.WriteLine($"Подключено к сети: {network.Address}/{network.IPv4Mask}");
                                break;
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Ошибка подключения к сети: {ex.Message}");
                            }
                        }
                    }

                    Console.WriteLine();
                }
            }

            Console.WriteLine("Настройка завершена.");
            Console.ReadLine();
        }
    }
}
