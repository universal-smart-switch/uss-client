using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using ussclientsandbox;

namespace ussclientsandbox.Model
{
    internal class Mode
    {
        private string _name;
        private List<Characteristic> _characteristicsToMet;
        private bool _invert;
        private bool _executeMet;
        private bool _onSingle;
        private bool _invalid;

        public Mode(string name, Characteristic firstCharacteristic)
        {
            _name = name;
            _characteristicsToMet = new List<Characteristic>();
            _characteristicsToMet.Add(firstCharacteristic);
        }

        public Mode(string xaml)
        {
            try
            {
                _characteristicsToMet = new List<Characteristic>();
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xaml);

                _name = xmlDoc.SelectSingleNode("/mode/@name").Value;
                _invert = bool.Parse(xmlDoc.SelectSingleNode("/mode/@invert").Value);
                _executeMet = bool.Parse(xmlDoc.SelectSingleNode("/mode/@executemet").Value);
                _onSingle = bool.Parse(xmlDoc.SelectSingleNode("/mode/@onSingle").Value);



                string xpath = "mode/characteristic";
                var nodes = xmlDoc.SelectNodes(xpath);
                foreach (XmlNode childrenNode in nodes)
                {
                    bool invert = bool.Parse(childrenNode.Attributes["invert"].Value);
                    bool met = bool.Parse(childrenNode.Attributes["met"].Value);
                    int value = int.Parse(childrenNode.Attributes["value"].Value);
                    CharacteristicType type = (CharacteristicType)int.Parse(childrenNode.Attributes["type"].Value);
                    Characteristic nodeCh = new Characteristic(type, value, invert);
                    _characteristicsToMet.Add(nodeCh);
                }
                _invalid = false;
            }
            catch (Exception)
            {

                _invalid = true;
            }
            
        }

        

        public string ToXAML()
        {
            //XmlTextWriter xmlTextWriter = new XmlTextWriter("./modes/" + _name + "Message.xml", Encoding.UTF8);

            using (var stringWriter = new StringWriter())
            {
                XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
                xmlTextWriter.Formatting = Formatting.Indented;
                xmlTextWriter.WriteStartDocument();

                xmlTextWriter.WriteStartElement("mode");
                xmlTextWriter.WriteAttributeString("name", _name);
                xmlTextWriter.WriteAttributeString("invert", _invert.ToString());
                xmlTextWriter.WriteAttributeString("executemet", _executeMet.ToString());
                xmlTextWriter.WriteAttributeString("onSingle", _onSingle.ToString());

                foreach (var item in _characteristicsToMet)
                {
                    xmlTextWriter.WriteStartElement("characteristic");
                    xmlTextWriter.WriteAttributeString("type", ((int)item.Type).ToString());
                    xmlTextWriter.WriteAttributeString("value", item.Value.ToString());
                    xmlTextWriter.WriteAttributeString("invert", item.Invert.ToString());
                    xmlTextWriter.WriteAttributeString("met", item.Met.ToString());
                    xmlTextWriter.WriteEndElement();
                }

                xmlTextWriter.WriteEndElement();

                return stringWriter.ToString();
            } 
        }



        public string Name { get => _name; set => _name = value; }
        public bool Invert { get => _invert; set => _invert = value; }
        public bool ExecuteMet { get => _executeMet;}
        public bool OnSingle { get => _onSingle; set => _onSingle = value; }
        internal List<Characteristic> CharacteristicsToMet { get => _characteristicsToMet; set => _characteristicsToMet = value; }
    }
}
