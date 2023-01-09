using System.Collections.ObjectModel;
using System.Net.NetworkInformation;
using uss_client_sandbox.Models;

namespace ussclientsandbox.Models
{
    public class Characteristic
    {
        private bool _met;
        private bool _invert;
        private CharacteristicType _type;
        private int _value;

        public Characteristic(CharacteristicType type, int value, bool invert)
        {
            _type = type;
            _value = value;
            _invert = invert;
        }

        public bool Met { get => _met; set => _met = value; }
        public bool Invert { get => _invert; set => _invert = value; }
        public CharacteristicType Type { get => _type; set => _type = value; }
        public ObservableCollection<string> PossibleCharacteristics { get => LocalBridge.PossibleCharacteristics; }
        public int Value { get => _value; set => _value = value; }

        public string DisplayableType
        {
            get => Type.ToString();
            set
            {
                CharacteristicType choice;
                if (Enum.TryParse("HKEY_LOCAL_MACHINE", out choice))
                {
                    uint aValue = (uint)choice;

                    Type = (CharacteristicType)aValue;

                }
                else { /* error: the string was not an enum member */ }
            }
        }
    }

    public enum CharacteristicType
    {
        Blank,
        Temperature,
        Date,
    }
}
