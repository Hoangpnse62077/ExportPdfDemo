using iTextSharp.text;
using iTextSharp.text.pdf;
using QRCoder;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Color = System.Drawing.Color;
using Font = iTextSharp.text.Font;
using Rectangle = iTextSharp.text.Rectangle;
using Image = iTextSharp.text.Image;
using System.Drawing.Imaging;

namespace ConsoleAppPDF
{
    public static class Class3
    {
        public static int ExportTicket(PurchaseOrderExportInMailModel po)
        {
            #region setfile
            //var stream = new MemoryStream();
            var stream = new FileStream("ORRTest.pdf", FileMode.Create);
            //var mediumFont = FontFactory.GetFont("Calibri", 12, Font.NORMAL);
            //var largeFontBoldBlue = FontFactory.GetFont("Calibri", 14, Font.BOLD, BaseColor.BLUE);
            var pageBackgroundColor = BaseColor.WHITE;

            //4X6: 4x72, 6x72
            var pageSize = new Rectangle(108,108);
            pageSize.BackgroundColor = pageBackgroundColor;

            pageSize.BorderWidthLeft = 1;
            pageSize.BorderWidthRight = 1;
            pageSize.BorderWidthTop = 1;
            pageSize.BorderWidthBottom = 1;
            pageSize.BorderColor = BaseColor.BLACK;

            var document = new Document(pageSize, 1f, 1f, 2f, 2f);
            PdfWriter.GetInstance(document, stream);
            document.Open();
            #endregion

            int page = 1;
            foreach (var pod in po.ListPoDetailInMail)
            {
                //itemLineTable.Rows.Clear();
                document.NewPage();
                AddPageOSR(document, page);
            }

            document.Close();

            //Xoay trang
            //var outStream = new FileStream("ORRTest.pdf", FileMode.Create);
            //PdfReader reader = new PdfReader(stream.ToArray());
            //PdfStamper stamper = new PdfStamper(reader, outStream);

            //for (int i = 1; i <= reader.NumberOfPages; i++)
            //{
            //    PdfDictionary pageDict = reader.GetPageN(i);
            //    int desiredRot = 270; // 90 degrees clockwise
            //    PdfNumber rotation = pageDict.GetAsNumber(PdfName.ROTATE);
            //    if (rotation != null)
            //    {
            //        desiredRot += rotation.IntValue;
            //        desiredRot %= 360; // 0, 90, 180, 270
            //    }
            //    pageDict.Put(PdfName.ROTATE, new PdfNumber(desiredRot));
            //}
            //stamper.Close();

            return 0;
            //return stream.ToArray();
        }

        private static void AddPageOSR(Document document, int page)
        {
            var mediumFont = FontFactory.GetFont("Calibri", 10, Font.NORMAL);
            var mediumFontBold = FontFactory.GetFont("Calibri", 10, Font.BOLD);
            var largeFontBlack = FontFactory.GetFont("Calibri", 18, Font.NORMAL);
            var largeFontBoldBlack = FontFactory.GetFont("Calibri", 18, Font.BOLD);
            var largestFontBlack = FontFactory.GetFont("Calibri", 24, Font.NORMAL);
            var largestFontBoldBlack = FontFactory.GetFont("Calibri", 24, Font.BOLD);

            //table 1:
            {
                var itemLineTable = new PdfPTable(3);
                var phrase = new Phrase();
                var itemLineCell = new PdfPCell(phrase);

                var image = GenerateQRCode("Batch=551_|_Wave=1_|_Start=250_|_End=300", 20);

                var barCodeStream = image.ToByteArray(ImageFormat.Png);
                itemLineTable.WidthPercentage = 100f;
                itemLineTable.DefaultCell.BorderWidth = 0;
                //itemLineTable.SetWidths(new[] { 23f, 19f, 28f, 30f });

                ///row 1, col 1
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk("B-551", mediumFontBold));
                    itemLineCell = new PdfPCell(phrase);
                    itemLineCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    //itemLineCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    itemLineCell.BorderWidthTop = 0;
                    itemLineCell.BorderWidthBottom = 0;
                    itemLineCell.BorderWidthLeft = 0;
                    itemLineCell.BorderWidthRight = 0;
                    //itemLineCell.PaddingTop = 15f;
                    //itemLineCell.PaddingBottom = 5f;
                    //itemLineCell.PaddingLeft = 15f;

                    itemLineTable.AddCell(itemLineCell);
                }
                ///row 1, col 2
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk(""));
                    itemLineCell = new PdfPCell(phrase);
                    itemLineCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    itemLineCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    itemLineCell.BorderWidthTop = 0;
                    itemLineCell.BorderWidthBottom = 0;
                    itemLineCell.BorderWidthLeft = 0;
                    itemLineCell.BorderWidthRight = 0;


                    itemLineTable.AddCell(itemLineCell);
                }
                ///row 1, col 3
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk("W-1", mediumFontBold));
                    itemLineCell = new PdfPCell(phrase);
                    itemLineCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                   // itemLineCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    itemLineCell.BorderWidthTop = 0;
                    itemLineCell.BorderWidthBottom = 0;
                    itemLineCell.BorderWidthLeft = 0;
                    itemLineCell.BorderWidthRight = 0;
                    //itemLineCell.PaddingTop = 15f;
                    //itemLineCell.PaddingBottom = 5f;
                    //itemLineCell.PaddingRight = 15f;
                    itemLineTable.AddCell(itemLineCell);
                }

                //row 2, col 1
                {
                    phrase = new Phrase();
                    var barcodeImage = Image.GetInstance(barCodeStream.ToArray());
                    barcodeImage.ScaleAbsolute(75, 75);
                    phrase.Add(new Chunk(barcodeImage, -1, -20));
                    itemLineCell = new PdfPCell(barcodeImage);
                    itemLineCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    itemLineCell.VerticalAlignment = Element.ALIGN_CENTER;

                    itemLineCell.BorderWidthTop = 0;
                    itemLineCell.BorderWidthBottom = 0;
                    itemLineCell.BorderWidthLeft = 0;
                    itemLineCell.BorderWidthRight = 0;
                    //itemLineCell.PaddingTop = -5;
                    //itemLineCell.PaddingBottom = -2;
                    itemLineCell.Colspan = 3;
                    itemLineTable.AddCell(itemLineCell);
                }

                ///row 3, col 1
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk("255 – 300 ", mediumFontBold));
                    itemLineCell = new PdfPCell(phrase);
                    itemLineCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    // itemLineCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    itemLineCell.BorderWidthTop = 0;
                    itemLineCell.BorderWidthBottom = 0;
                    itemLineCell.BorderWidthLeft = 0;
                    itemLineCell.BorderWidthRight = 0;
                    itemLineCell.Colspan = 3;
                    //itemLineCell.PaddingTop = -5;

                    itemLineTable.AddCell(itemLineCell);
                }
                document.Add(itemLineTable);
            }
            
        }


        public static int ExportTicketLabel(List<TicketExportModel> ticketExportModels)
        {
            #region setfile
            //var stream = new MemoryStream();
            var stream = new FileStream("ORRTest.pdf", FileMode.Create);
            //var mediumFont = FontFactory.GetFont("Calibri", 12, Font.NORMAL);
            //var largeFontBoldBlue = FontFactory.GetFont("Calibri", 14, Font.BOLD, BaseColor.BLUE);
            var pageBackgroundColor = BaseColor.WHITE;

            //4X6: 4x72, 6x72
            var pageSize = new Rectangle(108, 108);
            pageSize.BackgroundColor = pageBackgroundColor;

            pageSize.BorderWidthLeft = 0;
            pageSize.BorderWidthRight = 0;
            pageSize.BorderWidthTop = 0;
            pageSize.BorderWidthBottom = 0;
            //pageSize.BorderColor = BaseColor.BLACK;

            var document = new Document(pageSize, 1f, 1f, 2f, 2f);
            PdfWriter.GetInstance(document, stream);
            document.Open();
            #endregion

            int page = 1;
            foreach (var ticket in ticketExportModels)
            {
                //itemLineTable.Rows.Clear();
                document.NewPage();
                AddTicketLabelPage(document, page, ticket);
            }

            document.Close();

            return 0;
            //return stream.ToArray();

        }

        private static Bitmap GenerateQRCode(string source, int imageWidth = 20)
        {
            QRCodeGenerator qrGenerator = new QRCodeGenerator();
            QRCodeData qrCodeData = qrGenerator.CreateQrCode(source, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(qrCodeData);
            Bitmap qrCodeImage = qrCode.GetGraphic(imageWidth, Color.Black, Color.White, false);
            return qrCodeImage;
        }

        private static void AddTicketLabelPage(Document document, int page,TicketExportModel model)
        {
            var mediumFont_7 = FontFactory.GetFont("Calibri", 7, Font.NORMAL);
            var mediumFont_7point5 = FontFactory.GetFont("Calibri", 7.5f, Font.NORMAL);
            var mediumFont_8 = FontFactory.GetFont("Calibri", 8, Font.NORMAL);
            var mediumFont_8point5 = FontFactory.GetFont("Calibri", 8.2f, Font.NORMAL);
            var mediumFont_9 = FontFactory.GetFont("Calibri", 9, Font.NORMAL);
            var mediumFontBold_8 = FontFactory.GetFont("Calibri", 8, Font.BOLD);
            var largeFontBlack_9 = FontFactory.GetFont("Calibri", 9, Font.NORMAL);
            var largeFontBlack_10 = FontFactory.GetFont("Calibri", 10, Font.NORMAL);
            var largeFontBoldBlack_9 = FontFactory.GetFont("Calibri", 9, Font.BOLD);
            var largeFontBoldBlack_10 = FontFactory.GetFont("Calibri", 10, Font.BOLD);
            var largestFontBlack = FontFactory.GetFont("Calibri", 24, Font.NORMAL);
            var largestFontBoldBlack = FontFactory.GetFont("Calibri", 24, Font.BOLD);


            //table 1:
            {
                var itemLineTable = new PdfPTable(2);
                var phrase = new Phrase();
                var itemLineCell = new PdfPCell(phrase);

                var image = GenerateQRCode("integrer+đaas", 20);

                var barCodeStream = image.ToByteArray(ImageFormat.Png);
                itemLineTable.WidthPercentage = 100f;
                itemLineTable.DefaultCell.BorderWidth = 0;
                //itemLineTable.SetWidths(new[] { 23f, 19f, 28f, 30f });

                //row 1
                {
                    phrase = new Phrase();
                    var barcodeImage = Image.GetInstance(barCodeStream);
                    barcodeImage.ScaleAbsolute(55, 55);

                    phrase.Add(new Chunk(barcodeImage, 0, 0));
                    itemLineCell = new PdfPCell(barcodeImage);
                    itemLineCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    itemLineCell.VerticalAlignment = Element.ALIGN_CENTER;

                    itemLineCell.BorderWidthTop = 0;
                    itemLineCell.BorderWidthBottom = 0;
                    itemLineCell.BorderWidthLeft = 0;
                    itemLineCell.BorderWidthRight = 0;
                    //itemLineCell.PaddingTop = -7f;
                    // itemLineCell.PaddingBottom = 5f;
                    itemLineCell.PaddingLeft = 1;
                    itemLineCell.Rowspan = 4;
                    itemLineTable.AddCell(itemLineCell);
                }


                ///row 1, col 2
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk("013AAE06", largeFontBoldBlack_9));
                    itemLineCell = new PdfPCell(phrase);
                    itemLineCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    //itemLineCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    itemLineCell.BorderWidthTop = 0;
                    itemLineCell.BorderWidthBottom = 0;
                    itemLineCell.BorderWidthLeft = 0;
                    itemLineCell.BorderWidthRight = 0;
                   // itemLineCell.PaddingTop = 15f;
                   // itemLineCell.PaddingBottom = 5f;
                    //itemLineCell.PaddingLeft = 15f;

                    itemLineTable.AddCell(itemLineCell);
                }
                ///row 2, col 2
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk("14-1B", largeFontBoldBlack_10));
                    itemLineCell = new PdfPCell(phrase);
                    itemLineCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    //itemLineCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    itemLineCell.BorderWidthTop = 0;
                    itemLineCell.BorderWidthBottom = 0;
                    itemLineCell.BorderWidthLeft = 0;
                    itemLineCell.BorderWidthRight = 0;
                    // itemLineCell.PaddingTop = 15f;
                    // itemLineCell.PaddingBottom = 5f;
                    //itemLineCell.PaddingLeft = 15f;

                    itemLineTable.AddCell(itemLineCell);
                }

                ///row 3, col 2
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk("NL: 10E", largeFontBlack_10));
                    itemLineCell = new PdfPCell(phrase);
                    itemLineCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    //itemLineCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    itemLineCell.BorderWidthTop = 0;
                    itemLineCell.BorderWidthBottom = 0;
                    itemLineCell.BorderWidthLeft = 0;
                    itemLineCell.BorderWidthRight = 0;
                    // itemLineCell.PaddingTop = 15f;
                    // itemLineCell.PaddingBottom = 5f;
                    //itemLineCell.PaddingLeft = 15f;

                    itemLineTable.AddCell(itemLineCell);
                }

                ///row 4, col 2
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk("FSP, BCPL", mediumFont_9));
                    itemLineCell = new PdfPCell(phrase);
                    itemLineCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    //itemLineCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    itemLineCell.BorderWidthTop = 0;
                    itemLineCell.BorderWidthBottom = 0;
                    itemLineCell.BorderWidthLeft = 0;
                    itemLineCell.BorderWidthRight = 0;
                    // itemLineCell.PaddingTop = 15f;
                    // itemLineCell.PaddingBottom = 5f;
                    //itemLineCell.PaddingLeft = 15f;

                    itemLineTable.AddCell(itemLineCell);
                }



                document.Add(itemLineTable);
            }

            //table 2:
            {
                var itemLineTable = new PdfPTable(3);
                var phrase = new Phrase();
                var itemLineCell = new PdfPCell(phrase);

                itemLineTable.WidthPercentage = 100f;
                itemLineTable.DefaultCell.BorderWidth = 0;

                ///row 1, col 1
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk("tscepicbendy", largeFontBlack_10));
                    itemLineCell = new PdfPCell(phrase);
                    itemLineCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    //itemLineCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    itemLineCell.BorderWidthTop = 0;
                    itemLineCell.BorderWidthBottom = 0;
                    itemLineCell.BorderWidthLeft = 0;
                    itemLineCell.BorderWidthRight = 0;
                    itemLineCell.Colspan = 2;
                    itemLineTable.AddCell(itemLineCell);
                }

                ///row 1, col 2
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk("B-551", largeFontBlack_10));
                    itemLineCell = new PdfPCell(phrase);
                    itemLineCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    //itemLineCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    itemLineCell.BorderWidthTop = 0;
                    itemLineCell.BorderWidthBottom = 0;
                    itemLineCell.BorderWidthLeft = 0;
                    itemLineCell.BorderWidthRight = 0;
                    itemLineCell.Colspan = 3;
                    itemLineTable.AddCell(itemLineCell);
                }

                ///row 2, col 1
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk("G18600B – Hea Metal – 24mo", mediumFont_7point5));
                    itemLineCell = new PdfPCell(phrase);
                    itemLineCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    //itemLineCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    itemLineCell.BorderWidthTop = 0;
                    itemLineCell.BorderWidthBottom = 0;
                    itemLineCell.BorderWidthLeft = 0;
                    itemLineCell.BorderWidthRight = 0;
                    itemLineCell.Colspan = 3;
                    itemLineCell.PaddingTop = 6;
                    itemLineCell.PaddingBottom = 5;
                    itemLineTable.AddCell(itemLineCell);
                }

                ///row 3, col 1
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk("12-25", largeFontBlack_9));
                    itemLineCell = new PdfPCell(phrase);
                    itemLineCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    //itemLineCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    itemLineCell.BorderWidthTop = 0;
                    itemLineCell.BorderWidthBottom = 0;
                    itemLineCell.BorderWidthLeft = 0;
                    itemLineCell.BorderWidthRight = 0;
                    itemLineCell.Colspan = 2;
                    itemLineTable.AddCell(itemLineCell);
                }

                ///row 3, col 2
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk("45/50", largeFontBlack_9));
                    itemLineCell = new PdfPCell(phrase);
                    itemLineCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    //itemLineCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    itemLineCell.BorderWidthTop = 0;
                    itemLineCell.BorderWidthBottom = 0;
                    itemLineCell.BorderWidthLeft = 0;
                    itemLineCell.BorderWidthRight = 0;
                    itemLineCell.Colspan = 3;
                    itemLineTable.AddCell(itemLineCell);
                }


                document.Add(itemLineTable);

            }
        }


        private static byte[] ToByteArray(this System.Drawing.Image image, ImageFormat format)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, format);
                return ms.ToArray();
            }
        }
        public class TicketExportModel
        {
            public string ItemCode { get; set; }
            public string BinId { get; set; }
            public string NeckLabelBinId { get; set; }
            public string PrintLocation { get; set; }
            public string PalletSizeCombination { get; set; }
            public string VendorStyle { get; set; }
            public string ColorAbbreviation { get; set; }
            public string Size { get; set; }
            public string SlaDate { get; set; }
            public string PartnerId { get; set; }
            public string PickWaveTicket { get; set; }
        }
    }
}