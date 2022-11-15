using System.Net;
using System.Net.Sockets;
using ussclientsandbox.Model;

namespace uss_client_sandbox.Model
{
    public static class NetworkManager
    {
        #region property
        private static TcpClient client;
        private static NetworkStream stream;
        private static int port = DefinedInformation.TCPPort;
        private static List<byte> received = new List<byte>();
        private static List<byte> toSend = new List<byte>();
        private static bool dataToRead = false;
        private static bool dataToSend = false;
        private static bool bridgeFound = false;
        private static IPAddress bridgeAdress;
        #endregion

        #region method
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

                    ThreadPool.QueueUserWorkItem(new WaitCallback(NetworkThread), ct.Token);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("[NM]: ({0}) {1}", e.GetType(), e.Message);
            }
        }
        public static void Send(byte[] data)
        {
            toSend.AddRange(data);
            dataToSend = true;
        }
        public static void Search()
        {
            try
            {
                IPAddress[] addresslist = Dns.GetHostAddresses(DefinedInformation.BridgeHostName);
                bridgeAdress = new IPAddress(addresslist[0].GetAddressBytes());
                bridgeFound = true;
            }
            catch (Exception e)
            {
                Console.WriteLine("[NM]: {0}", e.Message);
                bridgeFound = false;
            }
        }
        #endregion

        #region thread
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
                                    complete.RemoveRange(0, sameNumMin);
                                    complete.Reverse();
                                }
                            }
                        }
                        dataToRead = true;
                        received.Clear();
                        received.AddRange(complete);
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
        #endregion

        #region field
        public static bool DataToRead { get => dataToRead; set => dataToRead = value; }
        public static List<byte> ReceivedData { get => received; }
        public static bool BridgeFound { get => bridgeFound; }
        #endregion
    }
}


