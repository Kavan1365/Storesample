using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace BaseCore.Helper.Localization
{
    /// <summary>
    /// پیاده سازی سرویس کار با فایل های چند زبانگی
    /// </summary>
//    public class LocalizationService : ILocalizationService
//    {
//        private readonly IWebHostEnvironment hostingEnvironment;

//        public LocalizationService(IWebHostEnvironment hostingEnvironment)
//        {
//            this.hostingEnvironment = hostingEnvironment;
//        }

//        public async Task<string> GetValueAsync(string Name, string Language, CancellationToken cancellationToken)
//        {
//            if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Language))
//                return string.Empty;

//            var filename = @"Localization\\Resource_" + Language + ".resx";
//            var resourceFile = Path.Combine(hostingEnvironment.WebRootPath, filename);

//            var xml = await File.ReadAllTextAsync(resourceFile, cancellationToken);

//            var elements = XElement.Parse(xml);

//            if (elements.Elements("data").ToList().Any(e => e.Attribute("name").Value == Name))
//                return elements.Elements("data").ToList().First(e => e.Attribute("name").Value == Name).Element("value").Value;

//            return string.Empty;

//        }

//        public async Task UpsertContent(string Name, string Value, string Language, CancellationToken cancellationToken)
//        {
//            var filename = @"Localization\\Resource_" + Language + ".resx";
//            var resourceFile = Path.Combine(hostingEnvironment.WebRootPath, filename);

//            var xml = await File.ReadAllTextAsync(resourceFile, cancellationToken);

//            var elements = XElement.Parse(xml);

//            if (elements.Elements("data").ToList().Any(e => e.Attribute("name").Value == Name))
//            {
//                elements.Elements("data").ToList().First(e => e.Attribute("name").Value == Name).Element("value").Value = Value;
//            }
//            else
//            {
//                elements.Add(new XElement("data", new XElement("value", Value), new XAttribute("name", Name), new XAttribute(XName.Get("xml", "space"), "preserve")));
//            }

//            XmlTextWriter writer = new XmlTextWriter(resourceFile, null)
//            {
//                Formatting = System.Xml.Formatting.Indented
//            };
//            elements.WriteTo(writer);
//            writer.Flush();
//            writer.Close();
//        }

//        public async Task DeleteContent(string Name, string Language, CancellationToken cancellationToken)
//        {
//            var filename = @"Localization\\Resource_" + Language + ".resx";
//            var resourceFile = Path.Combine(hostingEnvironment.WebRootPath, filename);

//            var xml = await File.ReadAllTextAsync(resourceFile, cancellationToken);

//            var elements = XElement.Parse(xml);

//            if (elements.Elements("data").ToList().Any(e => e.Attribute("name").Value == Name))
//            {
//                elements.Elements("data").ToList().First(e => e.Attribute("name").Value == Name).Remove();

//                XmlTextWriter writer = new XmlTextWriter(resourceFile, null)
//                {
//                    Formatting = System.Xml.Formatting.Indented
//                };
//                elements.WriteTo(writer);
//                writer.Flush();
//                writer.Close();
//            }
//        }
//        public string GetResource(string Language)
//        {
//            var filename = @"Localization/Resource_" + Language + ".resx";
//            var resourceFile = Path.Combine(hostingEnvironment.WebRootPath, filename);

//            var xml = File.ReadAllText(resourceFile);

//            var elements = XElement.Parse(xml).Elements("data").ToList();

//            Dictionary<string, string> obj = new Dictionary<string, string>();

//            foreach (var item in elements)
//            {
//                obj.Add(item.Attribute("name").Value, item.Element("value").Value);
//            }

//            string json = JsonConvert.SerializeObject(obj, Newtonsoft.Json.Formatting.Indented);

//            return json;
//        }
//        public void GenerateResourceFile(string Language)
//        {
//            var filename = @"Localization/Resource_" + Language + ".resx";
//            var resourceFile = Path.Combine(hostingEnvironment.WebRootPath, filename);

//            if (!Directory.Exists(resourceFile))
//            {
//                File.WriteAllText(resourceFile, ResxTemplate);
//            }
//        }

//        private string ResxTemplate => @"<?xml version='1.0' encoding='utf-8'?>
//<root>
//  <xsd:schema id='root' xmlns='' xmlns:xsd='http://www.w3.org/2001/XMLSchema' xmlns:msdata='urn:schemas-microsoft-com:xml-msdata'>
//    <xsd:import namespace='http://www.w3.org/XML/1998/namespace' />
//    <xsd:element name='root' msdata:IsDataSet='true'>
//      <xsd:complexType>
//        <xsd:choice maxOccurs='unbounded'>
//          <xsd:element name='metadata'>
//            <xsd:complexType>
//              <xsd:sequence>
//                <xsd:element name='value' type='xsd:string' minOccurs='0' />
//              </xsd:sequence>
//              <xsd:attribute name='name' use='required' type='xsd:string' />
//              <xsd:attribute name='type' type='xsd:string' />
//              <xsd:attribute name='mimetype' type='xsd:string' />
//              <xsd:attribute ref='xml:space' />
//            </xsd:complexType>
//          </xsd:element>
//          <xsd:element name='assembly'>
//            <xsd:complexType>
//              <xsd:attribute name='alias' type='xsd:string' />
//              <xsd:attribute name='name' type='xsd:string' />
//            </xsd:complexType>
//          </xsd:element>
//          <xsd:element name='data'>
//            <xsd:complexType>
//              <xsd:sequence>
//                <xsd:element name='value' type='xsd:string' minOccurs='0' msdata:Ordinal='1' />
//                <xsd:element name='comment' type='xsd:string' minOccurs='0' msdata:Ordinal='2' />
//              </xsd:sequence>
//              <xsd:attribute name='name' type='xsd:string' use='required' msdata:Ordinal='1' />
//              <xsd:attribute name='type' type='xsd:string' msdata:Ordinal='3' />
//              <xsd:attribute name='mimetype' type='xsd:string' msdata:Ordinal='4' />
//              <xsd:attribute ref='xml:space' />
//            </xsd:complexType>
//          </xsd:element>
//          <xsd:element name='resheader'>
//            <xsd:complexType>
//              <xsd:sequence>
//                <xsd:element name='value' type='xsd:string' minOccurs='0' msdata:Ordinal='1' />
//              </xsd:sequence>
//              <xsd:attribute name='name' type='xsd:string' use='required' />
//            </xsd:complexType>
//          </xsd:element>
//        </xsd:choice>
//      </xsd:complexType>
//    </xsd:element>
//  </xsd:schema>
//  <resheader name='resmimetype'>
//    <value>text/microsoft-resx</value>
//  </resheader>
//  <resheader name='version'>
//    <value>2.0</value>
//  </resheader>
//  <resheader name='reader'>
//    <value>System.Resources.ResXResourceReader, System.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</value>
//  </resheader>
//  <resheader name='writer'>
//    <value>System.Resources.ResXResourceWriter, System.Windows.Forms, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089</value>
//  </resheader>
//  <data name='Xbarat' xml:space='preserve'>
//    <value>Xbarat</value>
//  </data>
//</root>";
//    }
}
