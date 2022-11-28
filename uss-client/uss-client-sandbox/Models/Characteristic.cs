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
        public int Value { get => _value; set => _value = value; }
    }

    public enum CharacteristicType
    {
        Blank,
        Temperature,
        Date,
    }
}
