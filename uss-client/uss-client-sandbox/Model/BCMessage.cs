using System.Security.Permissions;
using System.Text;

namespace ussclientsandbox.Model
{
    /// <summary>
    /// message between bridge and client
    /// </summary>
    public class BCMessage
    {
        private byte[] _markStart;
        private byte[] _markEnd;
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
            _commandRaw = GetByteFromBCCommand(_command);   // also sets date for ping


            // add end.mark bytes
            _markEnd = GetBytesFromString(DefinedInformation.BCMarkEnd);
            foreach (var item in _markEnd)
            {
                _messageNoHeader.Add(item);
            }


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
            _markStart = GetBytesFromString(DefinedInformation.BCMarkStart);
            foreach (var item in _markStart)
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
            _markStart = new byte[] { _fullMessage[0], _fullMessage[1] };    // save mark
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

            byte[] tempNoHeader = new byte[_fullMessage.Count()];
            _fullMessage.CopyTo(tempNoHeader);

            // save id + remove
            _id = _fullMessage[0];
            _fullMessage.Remove(_fullMessage[0]);  // remove id

            // save command + remove
            _commandRaw = _fullMessage[0];
            _fullMessage.Remove(_fullMessage[0]);  // remove command

            // remove enmark
            _fullMessage.Reverse();
            _fullMessage.Remove(_fullMessage[0]);
            _fullMessage.Remove(_fullMessage[0]);
            _fullMessage.Remove(_fullMessage[0]);
            _fullMessage.Reverse();


            // save data + remove
            _data = new byte[_fullMessage.Count()];
            _fullMessage.Reverse();
            _fullMessage.CopyTo(_data);

            string receivedTest = Encoding.UTF8.GetString(_data.ToArray());
            

            if (receivedTest.Contains("\0\08$C"))
            {
                string input = receivedTest;
                int index = input.IndexOf("\"");
                if (index >= 0)
                    input = input.Substring(0, index);

                int test = 2;
            }

            // readd bytes
            _fullMessage.Clear();
            _fullMessage.AddRange(fullMessage); //copy list to internal list

            //validation
            //checkSumCorrect = ValidateCheckSum(tempNoHeader, _checkSum);
            checkSumCorrect = true;
            _command = GetBCCommandFromByte(_commandRaw);

            string testData = this.DataString;

            if (!checkSumCorrect)
            {
                _command = BCCommand.Invalid;
            }

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
        private byte GetByteFromBCCommand(BCCommand type)   // also sets date for ping!
        {
            byte typeRaw;

            switch (type)
            {
                case BCCommand.Invalid:
                    return DefinedInformation.BCCInvalid;
                case BCCommand.EchoReq:
                    return DefinedInformation.BCCEchoReq;
                case BCCommand.EchorRep:
                    return DefinedInformation.BCCEchorRep;
                case BCCommand.GetSwitches:
                    return DefinedInformation.BCCGetSwitches;
                case BCCommand.GetSwitchesRep:
                    return DefinedInformation.BCCGetSwitchesRep;
                case BCCommand.GetModes:
                    return DefinedInformation.BCCGetModes;
                case BCCommand.GetModesRep:
                    return DefinedInformation.BCCGetModesRep;
                case BCCommand.GetSysInfo:
                    return DefinedInformation.BCCGetSysInfo;
                default:
                    return DefinedInformation.BCCInvalid;

            }
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
                case DefinedInformation.BCCEchoReq:
                    return BCCommand.EchoReq;
                    break;
                case DefinedInformation.BCCEchorRep:
                    return BCCommand.EchorRep;
                    break;
                case DefinedInformation.BCCGetSwitchesRep:
                    return BCCommand.GetSwitchesRep;
                    break;
                case DefinedInformation.BCCGetModesRep:
                    return BCCommand.GetModesRep;
                    break;
                case DefinedInformation.BCCGetSysInfoRep:
                    return BCCommand.GetSysInfoRep;
                    break;
                case DefinedInformation.BCCInvalid:
                    return BCCommand.Invalid;
                    break;
                default:
                    return BCCommand.EchoReq;
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
                string test1 =  BitConverter.ToString(_fullMessage.ToArray());
                string test2 = Encoding.UTF8.GetString(_data, 0, _data.Length);
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

        public BCCommand Command { get => _command; set => _command = value; }
        #endregion
    }

    public enum BCCommand
    {
        Invalid,
        EchoReq,
        EchorRep,
        GetSwitches,
        GetSwitchesRep,
        GetModes,
        GetModesRep,
        GetSysInfo,
        GetSysInfoRep
    }
}
