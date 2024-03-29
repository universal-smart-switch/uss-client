﻿using SuperSimpleTcp;
using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using ussclientsandbox.Models;


namespace uss_client_sandbox.Models
{
    public static class NetworkManager
    {
        #region property
        private static IPAddress bridgeAdress;
        private static List<byte> toSend = new List<byte>();
        private static List<byte> received = new List<byte>();
        private static NetworkStream stream;
        private static SimpleTcpClient simpleClient;
        private static TcpClient client;
        private static BCMessage lastMessageReceived;
        private static BCMessage lastMessageSent;
        private static bool dataToRead = false;
        private static bool dataToSend = false;
        private static bool bridgeFound = false;
        private static int port = DefinedInformation.TCPPort;
        private static DateTime lastEchoRequest;
        private static Dictionary<DateTime, TimeSpan> pingHistory = new Dictionary<DateTime, TimeSpan>();
        private static bool connectionError = true;
        private static CancellationTokenSource _canToken = new CancellationTokenSource();

        #endregion

        #region method
        /*
        public static void Connect(CancellationTokenSource ct)
        {
            try
            {
                Search();

                if (bridgeFound)
                {
                    // Prefer a using declaration to ensure the instance is Disposed later.
                    client = new TcpClient(DefinedInformation.BridgeHostName, port);

                    // Get a client stream for reading and writing.
                    stream = client.GetStream();

                    //ThreadPool.QueueUserWorkItem(new WaitCallback(NetworkThread), ct.Token);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("[NM]: ({0}) {1}", e.GetType(), e.Message);
            }
        }
        */
        public static void ConnectFirstTime()
        {
            Connect();
            if (!ConnectionError)
            {
                //NetworkManager.Send(new BCMessage(BCCommand.GetSwitches, "null", 0));
                //Task.Delay(10000);
                //NetworkManager.Send(new BCMessage(BCCommand.GetModes, "null", 0));
            }
        }

        public static void Connect()
        {
            Search();

            if (bridgeFound)
            {
                try
                {
                    if (DefinedInformation.LocalDebugMode)
                    {
                        simpleClient = new SimpleTcpClient("127.0.0.1:" + port);
                    }
                    else { simpleClient = new SimpleTcpClient(bridgeAdress.ToString() + ":" + port); }
                    

                    // set events

                    simpleClient.Events.Connected += Connected;
                    simpleClient.Events.Disconnected += Disconnected;
                    simpleClient.Events.DataReceived += DataReceived;

                    // let's go!
                    simpleClient.Connect();
                    connectionError = false;

                    // try getting switch list 
                    //NetworkManager.Send(new BCMessage(BCCommand.GetSwitches, "null", 0));
                }
                catch (Exception e)
                {
                    connectionError = true;
                    Console.WriteLine("[NM]: Client could not connect: " + e.Message);
                }
                
            }
            else
            {
                Console.WriteLine("[NM]: Bridge not found!");
            }
        }

        public static void Send(BCMessage message)
        {
            /*
            toSend.AddRange(message.FullMessage.ToArray());
            dataToSend = true;
            lastMessageSent = message;*/
            
            simpleClient.Send(message.FullMessage.ToArray());
            Console.WriteLine("[Client sent]:" + message.DataString);
            Debug.WriteLine("[Client sent]: " + " (" + message.Command.ToString()+ ") " + message.DataString);
        }

        public static bool CheckIfIPv4(string IpAddress)
        {
            try
            {
                Regex regex = new Regex(@"((^\s*((([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5])\.){3}([0-9]|[1-9][0-9]|1[0-9]{2}|2[0-4][0-9]|25[0-5]))\s*$)|(^\s*((([0-9A-Fa-f]{1,4}:){7}([0-9A-Fa-f]{1,4}|:))|(([0-9A-Fa-f]{1,4}:){6}(:[0-9A-Fa-f]{1,4}|((25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)(\.(25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)){3})|:))|(([0-9A-Fa-f]{1,4}:){5}(((:[0-9A-Fa-f]{1,4}){1,2})|:((25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)(\.(25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)){3})|:))|(([0-9A-Fa-f]{1,4}:){4}(((:[0-9A-Fa-f]{1,4}){1,3})|((:[0-9A-Fa-f]{1,4})?:((25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)(\.(25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)){3}))|:))|(([0-9A-Fa-f]{1,4}:){3}(((:[0-9A-Fa-f]{1,4}){1,4})|((:[0-9A-Fa-f]{1,4}){0,2}:((25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)(\.(25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)){3}))|:))|(([0-9A-Fa-f]{1,4}:){2}(((:[0-9A-Fa-f]{1,4}){1,5})|((:[0-9A-Fa-f]{1,4}){0,3}:((25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)(\.(25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)){3}))|:))|(([0-9A-Fa-f]{1,4}:){1}(((:[0-9A-Fa-f]{1,4}){1,6})|((:[0-9A-Fa-f]{1,4}){0,4}:((25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)(\.(25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)){3}))|:))|(:(((:[0-9A-Fa-f]{1,4}){1,7})|((:[0-9A-Fa-f]{1,4}){0,5}:((25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)(\.(25[0-5]|2[0-4]\d|1\d\d|[1-9]?\d)){3}))|:)))(%.+)?\s*$))");
                bool flag = false;
                string IPv = string.Empty;
                if (!string.IsNullOrEmpty(IpAddress))
                {
                    if (IpAddress.Count(c => c == '.') == 3)
                    {
                        flag = regex.IsMatch(IpAddress);
                        IPv = "IPv4";
                    }
                    else
                    {
                        IPv = "Version of";
                        flag = false;
                    }
                }

                if (flag)
                {
                    return true;
                }
                else
                {
                    Console.WriteLine("{0} is not a valid {1} IP address", IpAddress, IPv);
                    return false;
                }
            }
            catch (Exception) { return false; }
        }

        public static void Search()
        {
            try
            {
                if (!DefinedInformation.LocalDebugMode)
                {
                    IPAddress[] addresslist = Dns.GetHostAddresses(DefinedInformation.BridgeHostName);
                    bridgeAdress = new IPAddress(addresslist[0].GetAddressBytes());
                    
                    foreach (var item in addresslist)
                    {
                        if (CheckIfIPv4(item.ToString()))
                        {
                            bridgeAdress = new IPAddress(item.GetAddressBytes());
                            break;
                        }
                    }
                }
                else { bridgeAdress = new IPAddress(0); }
                bridgeFound = true;
            }
            catch (Exception e)
            {
                Console.WriteLine("[NM]: {0}", e.Message);
                bridgeFound = false;
            }
        }
        #endregion

        #region async
        public static void NetworkThread(Object obj)
        {
            CancellationToken token = (CancellationToken)obj;   // get cancellationtoken

            try
            {
                while (!token.IsCancellationRequested)  // check if cancellation requested
                {

                    // receive data
                    if (stream.DataAvailable)   // check if data received
                    {

                        Thread.Sleep(1000);
                        var complete = new List<byte>();
                        int sameNum = 0;
                        int sameNumMin = 8;
                        while (stream.DataAvailable)
                        {
                            if (sameNum < sameNumMin)
                            {
                                byte rec = (byte)stream.ReadByte();
                                if (complete.Count >= sameNumMin)
                                {
                                    if (rec == complete[sameNum])
                                    {
                                        sameNum++;
                                    }
                                }
                                complete.Add(rec);
                            }
                            else
                            {
                                for (int i = 0; i < sameNumMin; i++)
                                {
                                    complete.Reverse();
                                    complete.RemoveRange(0, 1);
                                    complete.Reverse();
                                }
                                break;
                                stream.Flush();
                            }
                        }
                        received.Clear();
                        received.AddRange(complete);
                        var recMes = new BCMessage(received.ToArray());

                        if (!(recMes.Command == BCCommand.Invalid))
                        {
                            lastMessageReceived = recMes;
                            dataToRead = true;
                        }
                        
                        if (    (int)recMes.Command == DefinedInformation.BCCGetSwitchesRep)
                        {
                            string test = ";";
                        }

                    }
                    // send data
                    else if (dataToSend)    // check if new data to send
                    {
                        stream.Write(toSend.ToArray(), 0, toSend.ToArray().Length);
                        toSend.Clear();
                        dataToSend = false;
                    }

                }

            }
            catch (Exception e)
            {
                Console.WriteLine("[NM]: {0}", e.Message);
            }

        }

        public static async Task ReceiveDataAsync(SuperSimpleTcp.DataReceivedEventArgs e)
        {
            //Console.WriteLine($"[{e.IpPort}] {Encoding.UTF8.GetString(e.Data.Array, 0, e.Data.Count)}");

            var tempData1 = new List<byte>();
            tempData1.AddRange(e.Data.Array);
            tempData1.Reverse();

            for (int i = 0; i < tempData1.Count; i++)
            {
                if ((tempData1[i] == 'E') && (tempData1[i + 1] == 'C') && (tempData1[i + 2] == '$'))
                {
                    tempData1.RemoveRange(i + 3, tempData1.Count - (i + 3));

                    while (tempData1[0] == 0)
                    {
                        tempData1.Remove(0);
                    }
                    tempData1.Reverse();

                    BCMessage test = new BCMessage(tempData1.ToArray());

                    if (!(test.Command == BCCommand.Invalid))
                    {
                        lastMessageReceived = test;
                        MessageController.ReceiveController(test);
                        dataToRead = true;
                    }

                    if ((int)test.Command == DefinedInformation.BCCGetSwitchesRep)
                    {
                        string hello = "test";
                    }

                }
            }


            /*
            var tempData = new List<byte>();
            tempData.AddRange(e.Data.Array);
            for (int i = 0; i < tempData.Count; i++)
            {
                if (    (tempData[i] == 'C')  && (tempData[i+1] == '$')   )
                {
                    tempData.RemoveRange(i+2, tempData.Count - (i+2));
                    BCMessage test = new BCMessage(tempData.ToArray());
                }
            }
            */
        }
        #endregion

        #region events
        static void Connected(object sender, ConnectionEventArgs e)
        {
            Console.WriteLine($"[Bridge {e.IpPort}]: connected");
        }

        static void Disconnected(object sender, ConnectionEventArgs e)
        {
            Console.WriteLine($"[Bridge {e.IpPort}]: disconnected");
        }

        static void DataReceived(object sender, SuperSimpleTcp.DataReceivedEventArgs e)
        {
            ReceiveDataAsync(e);
        }


        #endregion

        #region field
        public static bool DataToRead 
        { 
            get => dataToRead; 
            set
            {
                dataToRead = value;

                if (value == false)
                {
                    received.Clear();
                }
            }
        }
        public static List<byte> ReceivedData { get => received; }
        public static bool BridgeFound { get => bridgeFound; }
        internal static BCMessage LastMessage { get => lastMessageReceived;}
        public static string IpAdress { get => bridgeAdress.ToString(); }
        public static DateTime LastEchoRequest { get => lastEchoRequest; set => lastEchoRequest = value; }
        public static Dictionary<DateTime, TimeSpan> PingHistory { get => pingHistory; set => pingHistory = value; }
        public static bool ConnectionError { get => connectionError; set => connectionError = value; }
        public static CancellationTokenSource CanToken { get => _canToken; set => _canToken = value; }
        #endregion
    }
}


