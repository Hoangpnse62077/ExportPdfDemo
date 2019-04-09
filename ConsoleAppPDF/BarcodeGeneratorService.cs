using BarcodeLib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppPDF
{
    public class BarcodeGeneratorService
    {
        public static List<char> ListSpecialCharacter = new List<char>("-");

        private static bool CheckInputInSpeacial(string input)
        {
            return ListSpecialCharacter.Any(input.Contains);
        }

        public static void GenerateBarcodeNoLabelWithBarWidth(Stream stream, string source, TYPE type = TYPE.CODE128, string strImageFormat = "jpeg", AlignmentPositions positions = AlignmentPositions.CENTER)
        {
            try
            {
                Barcode b = new Barcode { BarWidth = 1 };

                if (type != TYPE.UNSPECIFIED)
                {
                    b.IncludeLabel = false;
                    b.Alignment = positions;

                    //===== Encoding performed here =====
                    Image barcodeImage;
                    if (!CheckInputInSpeacial(source.Trim()))
                    {
                        barcodeImage = b.Encode(type, source.Trim());
                    }
                    else
                    {
                        barcodeImage = b.Encode(type, source.Trim());
                    }

                    switch (strImageFormat)
                    {
                        case "gif": barcodeImage.Save(stream, ImageFormat.Gif); break;
                        case "jpeg": barcodeImage.Save(stream, ImageFormat.Jpeg); break;
                        case "png": barcodeImage.Save(stream, ImageFormat.Png); break;
                        case "bmp": barcodeImage.Save(stream, ImageFormat.Bmp); break;
                        case "tiff": barcodeImage.Save(stream, ImageFormat.Tiff); break;
                    }//switch

                }//if
            }//try
            catch (Exception ex)
            {
                throw;
            } //catch

        }

    }

    //Xoay trang
    //public static void RotatePDF(string inputFile, string outputFile)
    //{
    //    using (FileStream outStream = new FileStream(outputFile, FileMode.Create))
    //    {
    //        iTextSharp.text.pdf.PdfReader reader = new iTextSharp.text.pdf.PdfReader(inputFile);
    //        iTextSharp.text.pdf.PdfStamper stamper = new iTextSharp.text.pdf.PdfStamper(reader, outStream);

    //        iTextSharp.text.pdf.PdfDictionary pageDict = reader.GetPageN(desiredPage);
    //        int desiredRot = 90; // 90 degrees clockwise from what it is now
    //        iTextSharp.text.pdf.PdfNumber rotation = pageDict.GetAsNumber(iTextSharp.text.pdf.PdfName.ROTATE);

    //        if (rotation != null)
    //        {
    //            desiredRot += rotation.IntValue;
    //            desiredRot %= 360; // must be 0, 90, 180, or 270
    //        }
    //        pageDict.Put(iTextSharp.text.pdf.PdfName.ROTATE, new iTextSharp.text.pdf.PdfNumber(desiredRot));

    //        stamper.Close();
    //    }
    //}

}
