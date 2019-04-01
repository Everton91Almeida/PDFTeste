using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using System;
using System.Collections.Generic;
using System.IO;

namespace PDFTeste
{
    public class Program
    {
        static void Main(string[] args)
        {
            foreach (var item in ExtractPages())
            {
                Console.WriteLine("########## INICIO ##########");
                Console.WriteLine(pdfText(item));
                Console.WriteLine("########## FIM ##########");
            }
            Console.Read();
        }

        public static string pdfText(string path)
        {
            PdfReader reader = new PdfReader(path);
            string text = string.Empty;
            for (int page = 1; page <= reader.NumberOfPages; page++)
            {
                text += PdfTextExtractor.GetTextFromPage(reader, page);
            }
            reader.Close();
            return text;
        }


        public static IEnumerable<string> ExtractPages()
        {
            var fileName = "C:\\Totvs\\CONTRACHEQUES.pdf";
            var filePath = "C:\\Totvs\\";
            PdfReader reader = new PdfReader(fileName);
            FileInfo file = new FileInfo(fileName);
            string pdfFileName = file.Name.Substring(0, file.Name.LastIndexOf(".")) + "-";
            for (int pageNumber = 1; pageNumber <= reader.NumberOfPages; pageNumber++)
            {
                string newPdfFileName = $"{pdfFileName + pageNumber.ToString()}";
                SplitAndSaveInterval(fileName, filePath, pageNumber, 1, newPdfFileName);
                yield return $"{filePath + newPdfFileName}.pdf";
            }
        }

        public static void SplitAndSaveInterval(string pdfFilePath, string outputPath, int startPage, int interval, string pdfFileName)
        {
            using (PdfReader reader = new PdfReader(pdfFilePath))
            {
                var document = new Document();
                var copy = new PdfCopy(document, new FileStream($"{outputPath}\\{pdfFileName}.pdf", FileMode.Create));
                document.Open();
                for (int pagenumber = startPage; pagenumber < (startPage + interval); pagenumber++)
                {
                    if (reader.NumberOfPages >= pagenumber)
                        copy.AddPage(copy.GetImportedPage(reader, pagenumber));
                    else
                        break;
                }
                document.Close();
            }
        }
    }
}
