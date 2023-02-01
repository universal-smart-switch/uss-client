using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Net.NetworkInformation;
using uss_client_sandbox.Models;

namespace ussclientsandbox.Models
{
    public class Characteristic : INotifyPropertyChanged
    {
        private bool _met;
        private bool _invert;
        private CharacteristicType _type;
        private long _value;
        private DateTime _dateValue;

        public Characteristic(CharacteristicType type, int value, bool invert)
        {
            _type = type;
            _value = value;
            _invert = invert;
        }

        public bool Met { get => _met; set => _met = value; }
        public bool Invert { get => _invert; set => _invert = value; }
        public string BindingDateString
        {
            get
            {
                if (Type == CharacteristicType.Date)
                {
                    string dtMin = DateValue.Minute.ToString();
                    string dtHr = DateValue.Hour.ToString();
                    if (DateValue.Minute < 10) { dtMin = "0" + DateValue.Minute; }
                    if (DateValue.Hour < 10) { dtHr = "0" + DateValue.Hour; }

                    string dt = dtHr + ":" + dtMin;
                    return dt;
                }
                else if (Type == CharacteristicType.Temperature)
                {
                    return Value.ToString();
                }
                else return "-";

                
            }

            set
            {
                try
                {
                    if (Type == CharacteristicType.Date)
                    {
                        var spl = value.Split(":");
                        int hr = int.Parse(spl[0]);
                        int min = int.Parse(spl[1]);

                        DateValue = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, hr, min, 0);
                    }
                    else if (Type == CharacteristicType.Temperature)
                    {
                        Value = int.Parse(value);
                    }
                }
                catch (Exception)
                {
                    OnPropertyChanged(nameof(BindingDateString));
                }
                
            }
        }
        public CharacteristicType Type 
        { 
            get => _type; 
            set
            {
                _type = value;
                OnPropertyChanged(nameof(Type));
                OnPropertyChanged(nameof(IsTemp));
                OnPropertyChanged(nameof(IsNotTemp));
                OnPropertyChanged(nameof(IsDate));
                OnPropertyChanged(nameof(IsNotDate));
                OnPropertyChanged(nameof(IsBlank));
                OnPropertyChanged(nameof(IsNotBlank));
                OnPropertyChanged(nameof(BindingDateString));
            }
        }
        public ObservableCollection<string> PossibleCharacteristics { get => LocalBridge.PossibleCharacteristics; }
        public long Value { get => _value; set => _value = value; }

        #region usedforui
        public bool IsBlank
        {
            get
            {
                if (Type == CharacteristicType.Blank) { return true; }
                else return false;
            }
        }
        public bool IsNotBlank { get => !IsBlank; }
        public bool IsTemp
        {
            get
            {
                if (Type == CharacteristicType.Temperature) { return true; }
                else return false;
            }
        }
        public bool IsNotTemp { get => !IsTemp; }
        public bool IsDate
        {
            get
            {
                if (Type == CharacteristicType.Date) { return true; }
                else return false;
            }
        }
        public bool IsNotDate { get => !IsDate; }
        #endregion

        public string DisplayableType
        {
            get => Type.ToString();
            set
            {
                CharacteristicType choice;
                /*
                if (Enum.TryParse("HKEY_LOCAL_MACHINE", out choice))
                {
                    uint aValue = (uint)choice;

                    Type = (CharacteristicType)aValue;

                }
                else { 
                }
                */

                for (int i = 0; i < LocalBridge.PossibleCharacteristics.Count(); i++)
                {
                    if (value == LocalBridge.PossibleCharacteristics[i]) 
                    {
                        var selEnum = (CharacteristicType)i;
                        this.Type = selEnum;
                        break;
                    }
                }

                

            }
        }

        public DateTime DateValue 
        { 
            get
            {
                return DefinedInformation.UnixToDateTime(Value);
            }
            set
            {
                Value = DefinedInformation.DateTimeToUnix(value);
            }
        }

        #region event
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

    }

    public enum CharacteristicType
    {
        Blank,
        Temperature,
        Date,
    }
}
