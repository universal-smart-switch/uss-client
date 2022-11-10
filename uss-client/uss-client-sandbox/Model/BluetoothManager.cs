using HashtagChris.DotNetBlueZ;
using HashtagChris.DotNetBlueZ.Extensions;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.IO.Ports;
using System.Net.Sockets;
using System.Text;
using System.Windows;

namespace ussclientsandbox.Model
{
    /// <summary>
    /// Intern B2 communication standart
    /// </summary>

    public class BluetoothManager
    {
        private Adapter _adapter;
        private SerialPort _port;
        private IReadOnlyList<Device> _devices;
        public BluetoothManager()
        {
            _port = new SerialPort(DefinedInformation.BridgeCOM);
        }

        public void StartUp()
        {
            _port.StopBits = DefinedInformation.BridgeCOMStopBits;
            _port.BaudRate = DefinedInformation.BridgeCOMBaud;
            _port.Parity = DefinedInformation.BridgeCOMParity;
            _port.Handshake = DefinedInformation.BridgeCOMHandshake;

            try
            {
                _port.Open();
            }
            catch (Exception)
            {

            }
        }

        public async Task StartUpAsync()
        {
            _adapter = (await BlueZManager.GetAdaptersAsync()).FirstOrDefault();
        }

        // scan for available devices
        public async Task ScanDevices()
        {
            _devices = await _adapter.GetDevicesAsync();
        }
    }
}
