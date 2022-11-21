using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using System.Xml;
using ussclientsandbox.Model;
using System.Linq.Expressions;

namespace uss_client_sandbox.Model
{
    public class Switch
    {
        #region property
        private bool stateOn = false;
        private bool manualOverwrite = false;
        private DateTime lastContacted;
        private string name;
        private string address;
        private string mode;
        #endregion

        public Switch()
        {
            lastContacted = DateTime.Now;
        }

        #region field
        public bool StateOn { get => stateOn; set => stateOn = value; }
        public DateTime LastContacted { get => lastContacted; set => lastContacted = value; }
        public string Name { get => name; set => name = value; }
        public string Address { get => address; set => address = value; }
        public string Mode { get => mode; set => mode = value; }
        public string LastContactedUnix
        {
            get
            {
                return ((DateTimeOffset)LastContacted).ToUnixTimeSeconds().ToString();
            }

        }

        public bool ManualOverwrite { get => manualOverwrite; set => manualOverwrite = value; }
        #endregion
    }

    public class SwitchList : List<Switch>
    {
        private bool _valid = true;

        public void FromXML(string xml)
        {
            this.Clear();

            try
            {
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xml);

                string xpath = "switchList/switch";
                var nodes = xmlDoc.SelectNodes(xpath);
                foreach (XmlNode childrenNode in nodes)
                {
                    Switch sw = new Switch();

                    sw.Name = childrenNode.Attributes["name"].Value;
                    sw.Address = childrenNode.Attributes["address"].Value;
                    sw.Mode = childrenNode.Attributes["mode"].Value;
                    bool stateOn = bool.Parse(childrenNode.Attributes["stateOn"].Value);
                    bool manualOver = bool.Parse(childrenNode.Attributes["manualOverwrite"].Value);
                    sw.StateOn = stateOn;
                    sw.ManualOverwrite = manualOver;
                    this.Add(sw);
                }
                _valid = true;
            }
            catch
            {
                _valid = false;
            }
        }
        public string ToXML()
        {
            using (var stringWriter = new StringWriter())
            {
                XmlTextWriter xmlTextWriter = new XmlTextWriter(stringWriter);
                xmlTextWriter.Formatting = Formatting.Indented;
                xmlTextWriter.WriteStartDocument();

                xmlTextWriter.WriteStartElement("switchList");

                foreach (var item in this)
                {
                    xmlTextWriter.WriteStartElement("switch");
                    xmlTextWriter.WriteAttributeString("name", item.Name);
                    xmlTextWriter.WriteAttributeString("address", item.Address);
                    xmlTextWriter.WriteAttributeString("stateOn", item.StateOn.ToString());
                    xmlTextWriter.WriteAttributeString("lastContacted", item.LastContactedUnix);
                    xmlTextWriter.WriteAttributeString("mode", item.Mode);
                    xmlTextWriter.WriteAttributeString("manualOverwrite", item.ManualOverwrite.ToString());
                    xmlTextWriter.WriteEndElement();
                }

                xmlTextWriter.WriteEndElement();

                return stringWriter.ToString();
            }
        } 
        public bool Valid { get => _valid; set => _valid = value; }
    }
}
