using System;
using System.IO;
using System.Xml;
using System.Xml.Serialization;

using log4net;

namespace RFM.Common.Extensions
{
    public static class XmlExtensions
    {
        private static readonly ILog _logger = LogManager.GetLogger(typeof(XmlExtensions));

        /// <summary>
        /// Deserializes a XML into an object of type T.
        /// </summary>
        /// <typeparam name="T">The output type.</typeparam>
        /// <param name="input">The XML String.</param>
        /// <param name="value">Deserialized Output.</param>
        /// <returns>True if success. False if unsuccessful.</returns>
        public static bool TryXmlDeserialize<T>(this string input, out T value)
        {
            XmlSerializer serializer = null;
            try
            {
                serializer = new XmlSerializer(typeof(T), typeof(T).GetNestedTypes());
                using (TextReader reader = new StringReader(input))
                {
                    value = (T)serializer.Deserialize(reader);
                    if (value != null)
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);

                value = default(T);
                return false;
            }
            return false;
        }

        /// <summary>
        /// Serializes an object to XML string.
        /// </summary>
        /// <typeparam name="T">The input type.</typeparam>
        /// <param name="value">The output.</param>
        /// <returns>A serialized XML on success. An Empty string on failure.</returns>
        public static string ToSerializedXml<T>(this T value)
        {
            if (value == null)
            {
                return string.Empty;
            }
            try
            {
                XmlSerializer xmlserializer = new XmlSerializer(typeof(T));
                StringWriter stringWriter = new StringWriter();
                using (XmlWriter writer = XmlWriter.Create(stringWriter))
                {
                    xmlserializer.Serialize(writer, value);
                    return stringWriter.ToString();
                }
            }
            catch (Exception ex)
            {
                _logger.Error(ex);
                return string.Empty;
            }
        }
    }
}
