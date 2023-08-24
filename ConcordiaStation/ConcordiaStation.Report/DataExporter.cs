using System.Reflection.Metadata;
using iText.Kernel.Pdf;
using iText.Layout;

namespace ConcordiaStation.Report
{
    public static class DataExporter
    {
        public static byte[] GeneratePdf(string content)
        {
            using var memoryStream = new MemoryStream();
            using var writer = new PdfWriter(memoryStream);
            using var pdfDocument = new PdfDocument(writer);
            using var document = new iText.Layout.Document(pdfDocument);
            document.Add(new iText.Layout.Element.Paragraph(content));
            document.Close();

            return memoryStream.ToArray();
        }
    }
}
