using System;
using System.Xml;

namespace MMI.Libraries
{
    /// <summary>XML documents handling methods</summary>
    /// <company>Marktest - Markdata</company>
    /// <department>MMI - Markdata Media Internet</department>
    /// <author>Valter Lima</author>
    /// <date created>28-05-2010</date created>
    /// <modifiers>Valter Lima</modifiers>
    /// <dates modified>28-05-2010; 27-10-2011</dates modified>
    public class LibraryXML : System.IDisposable
    {
        #region Variables
        private String _XmlFilePath;
        #endregion Variables

        #region Constructor / Destructor

        /// <summary>Class constructor</summary>
        /// <author>Valter Lima</author>
        /// <date created>28-05-2010</date created>
        /// <modifiers>Valter Lima</modifiers>
        /// <dates modified>27-10-2011</dates modified>
        public LibraryXML(String sXmlFilePath)
        {
            _XmlFilePath = sXmlFilePath;
        }
        
        /// <summary>Disposer</summary>
        /// <author>Valter Lima</author>
        /// <date created>28-05-2010</date created>
        /// <modifiers>Valter Lima</modifiers>
        /// <dates modified>27-10-2011</dates modified>
        public void Dispose()
        {
            _XmlFilePath = null;
        }

        #endregion Constructor / Destructor

        /// <summary>Get the givem tag value from XML config file</summary>
        /// <author>Valter Lima</author>
        /// <date created>28-05-2010</date created>
        /// <modifiers>Valter Lima</modifiers>
        /// <dates modified>28-05-2010</dates modified>
        /// <param name="sConfigTag">Tag description</param>
        /// <returns>Tag value</returns>
        public String GetXmlConfigFileNodeValue(String sXmlNodeTag)
        {
            try
            {
                if (String.IsNullOrEmpty(_XmlFilePath))
                    return null;
                String configTagValue = null;
                XmlDocument objXmlDocument = new XmlDocument();
                objXmlDocument.Load(_XmlFilePath);
                XmlNode objXmlNode = objXmlDocument.DocumentElement.SelectSingleNode("./" + sXmlNodeTag);
                configTagValue = objXmlNode.InnerText;
                return configTagValue;
            }
            catch { return null; }
        }
    }
}
