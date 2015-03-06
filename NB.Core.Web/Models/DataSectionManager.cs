using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NB.Core.Web.Models
{
    /*
     <DataSection>
    <Ticker name="SPY" MetricsType="P/E">
      <add date="3/5/2015" value="19.85" />
      <add date="1/1/2014" value="18.15" />
    </Ticker>
  </DataSection>
     */
    public class DataSectionManager : ConfigurationSection
    {
        #region Self Management
        
        private const string defaultSectionName = "DataSection";
        public DataSectionManager()
        {

        }

        public DataSectionManager(string sectionName)
        {
            SectionName = sectionName;
        }

        private string _sectionName;
        public string SectionName
        {
            get
            {
                if (string.IsNullOrEmpty(_sectionName))
                {
                    return defaultSectionName;
                }
                return _sectionName;
            }
            set
            {
                _sectionName = value;
            }
        }

        public void CreateDefaultConfigurationSection (string sectionName, DatapointCollection collection)
        {
            DataSectionManager defaultSection = new DataSectionManager(sectionName);
            //DatapointCollection settingsCollection = new DatapointCollection();
            //settingsCollection[0] = new DataElement() { Date = new DateTime(2015, 3, 5), Value = 19.85f };
            //settingsCollection[1] = new DataElement() { Date = new DateTime(2014, 1, 1), Value = 18.3f };
            //settingsCollection[2] = new DataElement() { Date = new DateTime(2013, 1, 1), Value = 16f };
            defaultSection.Entries = collection;
            CreateConfigurationSection(sectionName, defaultSection);
        }

        public void CreateConfigurationSection(ConfigurationSection section)
        {
            CreateConfigurationSection(SectionName, section);
        }

        public void CreateConfigurationSection(string sectionName, ConfigurationSection section)
        {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            if (config.Sections[sectionName] == null)
            {
                config.Sections.Add(sectionName, section);
                section.SectionInformation.ForceSave = true;
                config.Save(ConfigurationSaveMode.Full);
                ConfigurationManager.RefreshSection(sectionName);
            }
        }

        public DataSectionManager GetSection()
        {
            return ConfigurationManager.GetSection(SectionName) as DataSectionManager;
        }

        public void Save()
        {
            Configuration config = ConfigurationManager.OpenExeConfiguration(null);
            var instance = (DataSectionManager)config.Sections[SectionName];
            instance.Entries = this.Entries;
            config.Save(ConfigurationSaveMode.Full);
        }

        #endregion

        [ConfigurationProperty("Entries", IsDefaultCollection=true)]
        public DatapointCollection Entries
        {
            get { return ((DatapointCollection)(base["Entries"])); }
            set { this["Entries"] = value; }
        }

        [ConfigurationProperty("Name")]
        public string Name
        {
            get { return (string)this["Name"]; }
            set { this["Name"] = value; }
        }

        [ConfigurationProperty("MetricsType")]
        public string MetricsType 
        {
            get { return (string)this["MetricsType"]; }
            set { this["MetricsType"] = value; }         
        }
    }

    [ConfigurationCollection(typeof(DataElement))]
    public class DatapointCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new DataElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((DataElement)(element)).Date;
        }

        public DataElement this[int index]
        {
            get
            {
                return (DataElement)BaseGet(index);
            }
            set
            {
                if (Count > index && BaseGet(index) != null)
                {
                    BaseRemoveAt(index);
                }
                BaseAdd(index, value);
            } 
        }
    }

    public class DataElement : ConfigurationElement
    {

        [ConfigurationProperty("date", IsKey = true, IsRequired = true)]
        public DateTime Date
        {
            get { return (DateTime)this["date"]; }
            set { this["date"] = value; }
        }

        [ConfigurationProperty("value", IsKey = false, IsRequired = true)]
        public float Value
        {
            get
            {
                return (float)this["value"];
            }
            set
            {
                this["value"] = value;
            }
        }

        public override bool IsReadOnly()
        {
            return false;
        }
    }
}
