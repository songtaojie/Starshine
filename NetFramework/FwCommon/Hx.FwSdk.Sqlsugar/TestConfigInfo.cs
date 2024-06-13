using SqlSugar;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hx.FwSdk.Sqlsugar
{
    public class TestConfigInfo : ConfigurationSection
    {
        [ConfigurationProperty("trackers", IsDefaultCollection = false)]
        public trackers Trackers { get { return (trackers)base["trackers"]; } }
        /// <summary>
        /// 获取配置信息
        /// </summary>
        /// <returns></returns>
        public static TestConfigInfo GetConfig()
        {
            return GetConfig("TestConfigInfo");
        }
        /// <summary>
        /// 获取配置信息
        /// </summary>
        /// <param name="sectionName">xml节点名称</param>
        /// <returns></returns>
        public static TestConfigInfo GetConfig(string sectionName)
        {
            TestConfigInfo section = (TestConfigInfo)ConfigurationManager.GetSection(sectionName);
            if (section == null)
                throw new ConfigurationErrorsException("Section " + sectionName + " is not found.");
            return section;
        }
        [ConfigurationProperty("TestName", IsRequired = false)]
        public string TestName
        {
            get { return (string)base["TestName"]; }
            set { base["TestName"] = value; }
        }
        [ConfigurationProperty("TestID", IsRequired = false)]
        public string TestID
        {
            get { return (string)base["TestID"]; }
            set { base["TestID"] = value; }
        }
    }

    public class DbConnectionConfigs : ConfigurationElementCollection
    {
        [ConfigurationProperty("TrackerName", IsRequired = false)]
        public string TrackerName
        {
            get { return (string)base["TrackerName"]; }
            set { base["TrackerName"] = value; }
        }
        protected override ConfigurationElement CreateNewElement()
        {
            return new tracker();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((tracker)element).Host;
        }
    }
    public class DbConnectionConfig : ConfigurationElement
    {
        #region 配置節設置，設定檔中有不能識別的元素、屬性時，使其不報錯

        protected override bool OnDeserializeUnrecognizedAttribute(string name, string value)
        {
            return base.OnDeserializeUnrecognizedAttribute(name, value);

        }

        protected override bool OnDeserializeUnrecognizedElement(string elementName, System.Xml.XmlReader reader)
        {
            return base.OnDeserializeUnrecognizedElement(elementName, reader);

        }
        #endregion
        [ConfigurationProperty("ConfigId", DefaultValue = "hxsqlsugar", IsRequired = true)]
        public string ConfigId
        {
            get { return this["ConfigId"].ToString(); }
        }

        [ConfigurationProperty("DbType", DefaultValue = DbType.Sqlite, IsRequired = true)]
        public DbType DbType
        {
            get 
            {
                if (Enum.IsDefined(typeof(DbType), this["DbType"]))
                {
                    Enum.TryParse(this["DbType"].ToString(), out DbType dbType);
                    return dbType;
                }
                return DbType.Sqlite;
            }
        }

        [ConfigurationProperty("ConnectionString", DefaultValue = DbType.Sqlite, IsRequired = true)]
        public string ConnectionString
        {
            get
            {
                return this["ConnectionString"].ToString();
            }
        }

        [ConfigurationProperty("IsAutoCloseConnection", DefaultValue = true, IsRequired = false)]
        public bool IsAutoCloseConnection
        {
            get
            {
                bool.TryParse(this["IsAutoCloseConnection"].ToString(), out bool result);
                return result;
            }
        }

    }
}
