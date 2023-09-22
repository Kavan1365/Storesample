using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Drawing;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Wordprocessing;
using Services.Exceptions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Services.Helper.OpenXml
{
    public static class OpenXmlExtensions
    {
        private static void AddPictureToContentControl(MainDocumentPart mainPart, OpenXmlElement item, byte[] pic, string tag)
        {
            Drawing drawing = item.Descendants<Drawing>().FirstOrDefault();
            if (drawing == null)
            {
                return;
            }

            Blip blip = drawing.Descendants<Blip>().FirstOrDefault();
            if (blip != null)
            {
                ImagePart imagePart = mainPart.AddImagePart(ImagePartType.Jpeg, tag);
                using (MemoryStream sourceStream = new MemoryStream(pic))
                {
                    imagePart.FeedData(sourceStream);
                }

                blip.Embed = tag;
            }
        }

        public static void SetControlsValue(this WordprocessingDocument doc, object obj)
        {
            foreach (OpenXmlElement item in doc.ContentControls())
            {
                string text = null;
                try
                {
                    text = item.Elements<SdtProperties>()?.FirstOrDefault().Elements<Tag>()?
                                       .FirstOrDefault()
                                       .Val;
                }
                catch (Exception)
                {

                    continue;

                }

                PropertyInfo property = obj.GetType().GetProperty(text);
                if (property == null)
                {
                    continue;
                }

                object obj2 = property.GetValue(obj);
                if (obj2 == null)
                {
                    continue;
                }

                if (obj2 is IEnumerable && property.PropertyType != typeof(byte[]) && property.PropertyType != typeof(string))
                {
                    DocumentFormat.OpenXml.Wordprocessing.Table table = item.Descendants<SdtContentBlock>().FirstOrDefault()?.Descendants<DocumentFormat.OpenXml.Wordprocessing.Table>().FirstOrDefault();
                    if (table == null)
                    {
                        continue;
                    }

                    OpenXmlElement openXmlElement = table.Last();
                    foreach (object item2 in obj2 as IEnumerable)
                    {
                        OpenXmlElement openXmlElement2 = openXmlElement.CloneNode(deep: true);
                        int num = 0;
                        PropertyInfo[] properties = item2.GetType().GetProperties();
                        foreach (PropertyInfo propertyInfo in properties)
                        {
                            DocumentFormat.OpenXml.Wordprocessing.Text text2 = null;
                            do
                            {
                                text2 = openXmlElement2.ElementAt(num++).Descendants<DocumentFormat.OpenXml.Wordprocessing.Text>().FirstOrDefault();
                            }
                            while (text2 == null && num < openXmlElement2.Count());
                            text2.Text = propertyInfo.GetValue(item2)?.ToString();
                        }

                        table.AppendChild(openXmlElement2);
                    }

                    table.RemoveChild(openXmlElement);
                }
                else if (property.PropertyType == typeof(byte[]))
                {
                    AddPictureToContentControl(doc.MainDocumentPart, item, (byte[])obj2, text);
                }
                else
                {
                    if (property.PropertyType == typeof(DateTime) || property.PropertyType == typeof(DateTime?))
                    {
                        obj2 = ((DateTime)property.GetValue(obj)).ToPersianDate();
                    }
                    else if (property.PropertyType.IsEnum)
                    {
                        obj2 = ((Enum)property.GetValue(obj)).GetDisplay();
                    }
                    else if (property.PropertyType == typeof(long))
                    {
                        obj2 = ((long)property.GetValue(obj)).ToString("N0");
                    }
                    else if (property.PropertyType == typeof(bool))
                    {
                        obj2 = (((bool)property.GetValue(obj)) ? "بله" : "خیر");
                    }

                    item.Descendants<DocumentFormat.OpenXml.Wordprocessing.Text>().FirstOrDefault().Text = obj2?.ToString();
                }
            }
        }

        public static IEnumerable<OpenXmlElement> ContentControls(this OpenXmlPart part)
        {
            return from e in part.RootElement.Descendants()
                   where e is SdtElement
                   select e;
        }

        public static IEnumerable<OpenXmlElement> ContentControls(this WordprocessingDocument doc)
        {
            foreach (OpenXmlElement item in doc.MainDocumentPart.ContentControls())
            {
                yield return item;
            }

            foreach (HeaderPart header in doc.MainDocumentPart.HeaderParts)
            {
                foreach (OpenXmlElement item2 in header.ContentControls())
                {
                    yield return item2;
                }
            }

            foreach (FooterPart footer in doc.MainDocumentPart.FooterParts)
            {
                foreach (OpenXmlElement item3 in footer.ContentControls())
                {
                    yield return item3;
                }
            }

            if (doc.MainDocumentPart.FootnotesPart != null)
            {
                foreach (OpenXmlElement item4 in doc.MainDocumentPart.FootnotesPart.ContentControls())
                {
                    yield return item4;
                }
            }

            if (doc.MainDocumentPart.EndnotesPart == null)
            {
                yield break;
            }

            foreach (OpenXmlElement item5 in doc.MainDocumentPart.EndnotesPart.ContentControls())
            {
                yield return item5;
            }
        }
    }
}
