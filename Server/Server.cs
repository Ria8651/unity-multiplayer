using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;

class Server {
    public static int maxPlayers { get; private set; }
    public static int port { get; private set; }
    public static Dictionary <int, Client> clients = new Dictionary <int, Client>();
    public delegate void PacketHandler(int _fromClient, Packet _packet);
    public static Dictionary <int, PacketHandler> packetHandlers;
    public static Map map;

    private static TcpListener tcpListener;
    private static UdpClient udpListener;

    public static void Start(int _maxPlayers, int _port) {
        maxPlayers = _maxPlayers;
        port = _port;

        Console.WriteLine("Starting server...");
        InitializeServerData();

        tcpListener = new TcpListener(IPAddress.Any, port);
        tcpListener.Start();
        tcpListener.BeginAcceptTcpClient(TCPConnectCallback, null);

        udpListener = new UdpClient(port);
        udpListener.BeginReceive(UDPReceiveCallback, null);

        Console.WriteLine($"Server started on port {port}.");
    }

    private static void TCPConnectCallback(IAsyncResult _result) {
        TcpClient _client = tcpListener.EndAcceptTcpClient(_result);
        tcpListener.BeginAcceptTcpClient(TCPConnectCallback, null);
        Console.WriteLine($"Incoming connection from {_client.Client.RemoteEndPoint}...");

        for (int i = 1; i <= maxPlayers; i++) {
            if (clients[i].tcp.socket == null) {
                clients[i].tcp.Connect(_client);
                return;
            }
        }

        Console.WriteLine($"{_client.Client.RemoteEndPoint} failed to connect: Server full!");
    }

    private static void UDPReceiveCallback(IAsyncResult _result) {
        try {
            IPEndPoint _clientEndPoint = new IPEndPoint(IPAddress.Any, 0);
            byte[] _data = udpListener.EndReceive(_result, ref _clientEndPoint);
            udpListener.BeginReceive(UDPReceiveCallback, null);

            if (_data.Length < 4) {
                return;
            }

            using(Packet _packet = new Packet(_data)) {
                int _clientId = _packet.ReadInt();

                if (_clientId == 0) {
                    return;
                }

                if (clients[_clientId].udp.endPoint == null) {
                    clients[_clientId].udp.Connect(_clientEndPoint);
                    return;
                }

                if (clients[_clientId].udp.endPoint.ToString() == _clientEndPoint.ToString()) {
                    clients[_clientId].udp.HandleData(_packet);
                }
            }
        } catch (Exception _ex) {
            Console.WriteLine($"Error receiving UDP data: {_ex}");
        }
    }

    public static void SendUDPData(IPEndPoint _clientEndPoint, Packet _packet) {
        try {
            if (_clientEndPoint != null) {
                udpListener.BeginSend(_packet.ToArray(), _packet.Length(), _clientEndPoint, null, null);
            }
        } catch (Exception _ex) {
            Console.WriteLine($"Error sending data to {_clientEndPoint} via UDP: {_ex}");
        }
    }

    static void InitializeServerData() {
        for (int i = 1; i <= maxPlayers; i++) {
            clients.Add(i, new Client(i));
        }

        map = new Map(0);

        packetHandlers = new Dictionary < int, PacketHandler > () {
            { (int) ClientPackets.welcomeReceived, ServerHandle.WelcomeReceived },
            { (int) ClientPackets.playerData, ServerHandle.PlayerData },
            { (int) ClientPackets.ready, ServerHandle.ClientReady },
            { (int) ClientPackets.infectPlayer, ServerHandle.InfectPlayer },
            { (int) ClientPackets.debug, ServerHandle.Debug }
        };

        Console.WriteLine("Initialized packets.");
    }

    public static Client[] OnlinePlayers() {
        List<Client> onlinePlayers = new List<Client>();
        foreach (Client client in clients.Values) {
            if (client.player != null) {
                onlinePlayers.Add(client);
            }
        }
        return onlinePlayers.ToArray();
    }
}