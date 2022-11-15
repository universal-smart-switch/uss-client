using System.Text;

namespace ussclientsandbox.Model
{
    /// <summary>
    /// message between bridge and client
    /// </summary>
    internal class BCMessage
    {
        private byte[] _mark;
        private byte _checkSum;
        private byte _id;
        private byte[] _data;
        private BCCommand _command;
        private byte _commandRaw;
        private List<byte> _fullMessage;
        private List<byte> _messageNoHeader;
        private byte[] _length;
        private bool checkSumCorrect;

        #region ctor
        // create to send
        public BCMessage(BCCommand command, string data, byte id)
        {
            // instanciate 
            _messageNoHeader = new List<byte>();
            _fullMessage = new List<byte>();

            _command = command;
            _data = GetBytesFromString(data);
            _id = Convert.ToByte(id);
            _commandRaw = GetByteFromBCCommand(_command);

            foreach (var item in _data)
            {
                _messageNoHeader.Add(item);
            }

            // save length
            int length = _messageNoHeader.Count;
            byte[] _tempLength = BitConverter.GetBytes(length);
            _length = new byte[] { _tempLength[2], _tempLength[1], _tempLength[0] };

            _messageNoHeader.Add(_commandRaw);
            _messageNoHeader.Add(_id);

            // calculate checksum
            _checkSum = CalcCheckSum(_messageNoHeader.ToArray());

            // create full message
            foreach (var item in _messageNoHeader)
            {
                _fullMessage.Add(item);
            }
            _fullMessage.Add(_checkSum);

            //add length bytes
            //_fullMessage.Add(_length[0]);
            foreach (var item in _length)
            {
                _fullMessage.Add(item);
            }

            // add mark bytes
            _mark = GetBytesFromString(DefinedInformation.BCMark);
            foreach (var item in _mark)
            {
                _fullMessage.Add(item);
            }

        }

        // create from received message
        public BCMessage(byte[] fullMessage)
        {
            // instanciate 
            _messageNoHeader = new List<byte>();
            _fullMessage = new List<byte>();

            _fullMessage.AddRange(fullMessage); //copy list to internal list


            //_messageNoHeader.AddRange(tempNoHeader);
            _fullMessage.Reverse();

            // save mark + remove
            _mark = new byte[] { _fullMessage[0], _fullMessage[1] };    // save mark
            _fullMessage.Remove(_fullMessage[0]);  // remove mark 
            _fullMessage.Remove(_fullMessage[0]);

            // save length + remove
            _length = new byte[] { _fullMessage[0], _fullMessage[1], _fullMessage[2] };    // save length
            _fullMessage.Remove(_fullMessage[0]);  // remove length 
            _fullMessage.Remove(_fullMessage[0]);
            _fullMessage.Remove(_fullMessage[0]);

            // save checkSum + remove
            _checkSum = _fullMessage[0];
            _fullMessage.Remove(_fullMessage[0]);  // remove checkSum

            byte[] tempNoHeader = new byte[fullMessage.Count()];
            _fullMessage.CopyTo(tempNoHeader);

            // save id + remove
            _id = _fullMessage[0];
            _fullMessage.Remove(_fullMessage[0]);  // remove id

            // save command + remove
            _commandRaw = _fullMessage[0];
            _fullMessage.Remove(_fullMessage[0]);  // remove command

            // save data + remove
            _data = new byte[_fullMessage.Count()];
            _fullMessage.Reverse();
            _fullMessage.CopyTo(_data);


            // readd bytes
            _fullMessage.Clear();
            _fullMessage.AddRange(fullMessage); //copy list to internal list

            //validation
            checkSumCorrect = ValidateCheckSum(tempNoHeader, _checkSum);
            _command = GetBCCommandFromByte(_commandRaw);

        }
        #endregion

        #region methods
        private byte CalcCheckSum(byte[] data)
        {

            int fullVal = 0;

            for (int i = 0; i < (data.Length - 2); i++)
            {
                fullVal += Convert.ToInt32(data[i]);
            }

            /*foreach (var item in data)
            {
                fullVal += Convert.ToInt32(item);
            }*/

            for (int i = 0; fullVal > 8; i++)
            {
                fullVal = fullVal / 8;
            }

            return Convert.ToByte(fullVal);
        }

        // create message
        private byte GetByteFromBCCommand(BCCommand type)
        {
            byte typeRaw;

            switch (type)
            {
                case BCCommand.GetSwitches:
                    typeRaw = DefinedInformation.BCCGetSwitches;
                    break;
                case BCCommand.GetModeSwitches:
                    typeRaw = DefinedInformation.BCCGetModeSwitches;
                    break;
                case BCCommand.GetStateSwitch:
                    typeRaw = DefinedInformation.BCCGetStateSwitch;
                    break;
                case BCCommand.SendSwitches:
                    typeRaw = DefinedInformation.BCCSendSwitches;
                    break;
                case BCCommand.SendModeSwitches:
                    typeRaw = DefinedInformation.BCCSendModeSwitches;
                    break;
                case BCCommand.EchoRequest:
                    typeRaw = DefinedInformation.BCCEchoRequest;
                    break;
                case BCCommand.SendStateSwitch:
                    typeRaw = DefinedInformation.BCCSendStateSwitch;
                    break;
                case BCCommand.SetModeSwitch:
                    typeRaw = DefinedInformation.BCCSetModeSwitch;
                    break;
                default:
                    typeRaw = 0b000;
                    break;
            }

            return typeRaw;
        }
        private byte[] GetBytesFromString(string data)
        {
            return Encoding.ASCII.GetBytes(data);
        }

        // save message
        private BCCommand GetBCCommandFromByte(byte type)
        {
            switch (type)
            {
                case DefinedInformation.BCCGetSwitches:
                    //return MessageType.Humidity;
                    return BCCommand.GetSwitches;
                    break;
                case DefinedInformation.BCCGetModeSwitches:
                    //return BCCommand.Temperature;
                    return BCCommand.GetModeSwitches;
                    break;
                case DefinedInformation.BCCGetStateSwitch:
                    //return BCCommand.O3;
                    return BCCommand.GetStateSwitch;
                    break;
                case DefinedInformation.BCCSendSwitches:
                    //return BCCommand.Fan;
                    return BCCommand.SendSwitches;
                    break;
                case DefinedInformation.BCCSendModeSwitches:
                    //return BCCommand.Engine;
                    return BCCommand.SendModeSwitches;
                    break;
                case DefinedInformation.BCCEchoRequest:
                    return BCCommand.EchoRequest;
                    break;
                case DefinedInformation.BCCSendStateSwitch:
                    return BCCommand.SendStateSwitch;
                    break;
                default:
                    return BCCommand.Invalid;
                    break;
            }
        }
        private bool ValidateCheckSum(byte[] _messageNoHeader, byte checkSumUntested)
        {
            int checkSumCalculated = CalcCheckSum(_messageNoHeader);
            if (checkSumUntested == checkSumCalculated)
            {
                return true;
            }
            return false;
        }
        #endregion

        #region fields
        public List<byte> FullMessage { get => _fullMessage; }

        public string DataString
        {
            get
            {
                //return BitConverter.ToString(_fullMessage.ToArray());
                return Encoding.UTF8.GetString(_data.ToArray());
            }
        }
        public int DataInt
        {
            get
            {
                return BitConverter.ToInt32(_fullMessage.ToArray(), 0);
            }
        }
        #endregion
    }

    public enum BCCommand
    {
        Invalid,
        GetSwitches,
        GetModeSwitches,
        GetStateSwitch,
        SendSwitches,
        SendModeSwitches,
        SendStateSwitch,
        EchoRequest,
        EchoResponse,
        SetModeSwitch,
    }
}
