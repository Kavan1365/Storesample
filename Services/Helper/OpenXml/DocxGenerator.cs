using DocumentFormat.OpenXml.Packaging;
using System.IO;


namespace Services.Helper.OpenXml
{
    public static class DocxGenerator
    {
        public static byte[] GenerateDocxFile(object obj, string html, byte[] template)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                memoryStream.Write(template, 0, template.Length);
                memoryStream.Position = 0L;
                using (WordprocessingDocument wordprocessingDocument = WordprocessingDocument.Open(memoryStream, isEditable: true))
                {
                    MainDocumentPart mainDocumentPart = wordprocessingDocument.MainDocumentPart;
                    wordprocessingDocument.SetControlsValue(obj);
                    mainDocumentPart.Document.Save();
                }

                return memoryStream.ToArray();
            }
        }
    }
}
