using System.Net.NetworkInformation;
using System.Net.Sockets;


namespace NetworkConfiguration
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Network Configuration");
            Console.WriteLine();
            NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
            foreach (NetworkInterface N in interfaces)
            {
                
                if (N.OperationalStatus == OperationalStatus.Up && N.Supports(NetworkInterfaceComponent.IPv4))
                {
                    Console.WriteLine($"Configuring network interface: {N.Name}");
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

                                Console.WriteLine($"Connected to network: {network.Address}/{network.IPv4Mask}");
                                break;
                            }
                            catch (Exception ex)
                            {
                                Console.WriteLine($"Error connecting to network: {ex.Message}");
                            }
                        }
                    }

                    Console.WriteLine();
                }
            }

            Console.WriteLine("Network configuration completed.");
            Console.ReadLine();
        }
    }
}