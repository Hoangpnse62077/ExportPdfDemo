using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.IO;
using Color = System.Drawing.Color;
using Font = iTextSharp.text.Font;
using Rectangle = iTextSharp.text.Rectangle;

namespace ConsoleAppPDF
{
    public static class Class1
    {
        
        public static int ExportReceiveToOSRBin(PurchaseOrderExportInMailModel po)
        {
            #region setfile
            //var stream = new MemoryStream();
            var stream = new FileStream("ORRTest.pdf", FileMode.Create);
            //var mediumFont = FontFactory.GetFont("Calibri", 12, Font.NORMAL);
            //var largeFontBoldBlue = FontFactory.GetFont("Calibri", 14, Font.BOLD, BaseColor.BLUE);
            var pageBackgroundColor = BaseColor.WHITE;

            //4X6: 4x72, 6x72
            var pageSize = new Rectangle(288, 432);
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
            var mediumFont = FontFactory.GetFont("Calibri", 12, Font.NORMAL);
            var mediumFontBold = FontFactory.GetFont("Calibri", 12, Font.BOLD);
            var largeFontBlack = FontFactory.GetFont("Calibri", 18, Font.NORMAL);
            var largeFontBoldBlack = FontFactory.GetFont("Calibri", 18, Font.BOLD);
            var largestFontBlack = FontFactory.GetFont("Calibri", 24, Font.NORMAL);
            var largestFontBoldBlack = FontFactory.GetFont("Calibri", 24, Font.BOLD);

            #region content
            //table 1:
            {
                var itemLineTable = new PdfPTable(4);
                var phrase = new Phrase();
                var itemLineCell = new PdfPCell(phrase);

                itemLineTable.WidthPercentage = 100f;
                itemLineTable.DefaultCell.BorderWidth = 0;
                itemLineTable.SetWidths(new[] { 23f, 19f, 28f, 30f });

                //row 1, col 1
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk("Box ID: ", largestFontBlack));
                    phrase.Add(new Chunk("\n"));

                    itemLineCell = new PdfPCell(phrase);
                    itemLineCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    itemLineCell.VerticalAlignment = Element.ALIGN_MIDDLE;

                    itemLineCell.BorderWidthTop = 0;
                    itemLineCell.BorderWidthBottom = 0;
                    itemLineCell.BorderWidthLeft = 0;
                    itemLineCell.BorderWidthRight = 0;
                    itemLineCell.PaddingTop = 15f;
                    itemLineCell.PaddingBottom = 5f;

                    itemLineTable.AddCell(itemLineCell);
                }

                //row 1, col 2
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk("100", largestFontBoldBlack));
                    phrase.Add(new Chunk("\n"));

                    itemLineCell = new PdfPCell(phrase);
                    itemLineCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    itemLineCell.VerticalAlignment = Element.ALIGN_MIDDLE;

                    itemLineCell.BorderWidthTop = 0;
                    itemLineCell.BorderWidthBottom = 0;
                    itemLineCell.BorderWidthLeft = 0;
                    itemLineCell.BorderWidthRight = 0;
                    itemLineCell.PaddingTop = 15f;
                    itemLineCell.PaddingBottom = 5f;

                    itemLineTable.AddCell(itemLineCell);
                }
                
                //row 1, col 3
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk("Qty: ", largestFontBlack));
                    phrase.Add(new Chunk("36", largestFontBoldBlack));
                    phrase.Add(new Chunk("\n"));

                    itemLineCell = new PdfPCell(phrase);
                    itemLineCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    itemLineCell.VerticalAlignment = Element.ALIGN_MIDDLE;

                    itemLineCell.BorderWidthTop = 0;
                    itemLineCell.BorderWidthBottom = 0;
                    itemLineCell.BorderWidthLeft = 0;
                    itemLineCell.BorderWidthRight = 0;
                    itemLineCell.PaddingTop = 15f;
                    itemLineCell.PaddingBottom = 5f;

                    itemLineTable.AddCell(itemLineCell);
                }

                //row 1, col 4: Barcode
                {

                    //string batch = string.Format("M{0}{1}{2}", DateTime.Now.Year.ToString().Substring(2),
                    //                                    DateTime.Now.DayOfYear.ToString().PadLeft(3, '0'),
                    //    (DateTime.Now.Hour * 3600 + DateTime.Now.Minute * 60 + DateTime.Now.Second).ToString().PadLeft(5, '0'));

                    phrase = new Phrase();
                    phrase.Add(new Chunk(" ", mediumFont));

                    #region  Item barcode
                    using (var barCodeStream = new MemoryStream())
                    {
                        //export pdf to stream
                        BarcodeGeneratorService.GenerateBarcodeNoLabelWithBarWidth(barCodeStream, "01236548965269");
                        //BarcodeGeneratorService.GenerateBarcodeNoLabelWithBarWidth(barCodeStream, batch);

                        var barcodeImage = Image.GetInstance(barCodeStream.ToArray());
                        barcodeImage.ScaleAbsolute(110, 50);
                        phrase.Add(new Chunk(barcodeImage, -1, -30));
                    }
                    phrase.Add(new Chunk("\n"));
                    #endregion

                    itemLineCell = new PdfPCell(phrase);
                    itemLineCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    itemLineCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    itemLineCell.BorderWidthTop = 0;
                    itemLineCell.BorderWidthLeft = 0;
                    itemLineCell.BorderWidthBottom = 0;
                    itemLineCell.BorderWidthRight = 0;
                    itemLineCell.PaddingTop = 10f;
                    itemLineCell.PaddingBottom = 15f;
                    itemLineCell.PaddingRight = 10f;

                    itemLineTable.AddCell(itemLineCell);
                }

                //row 2, col 1
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk("Pick Bin: ", largestFontBlack));
                    phrase.Add(new Chunk("\n"));

                    itemLineCell = new PdfPCell(phrase);
                    itemLineCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    itemLineCell.VerticalAlignment = Element.ALIGN_MIDDLE;

                    itemLineCell.BorderWidthTop = 0;
                    itemLineCell.BorderWidthBottom = 0;
                    itemLineCell.BorderWidthLeft = 0;
                    itemLineCell.BorderWidthRight = 0;
                    itemLineCell.PaddingTop = 5f;
                    itemLineCell.PaddingBottom = 2f;

                    itemLineTable.AddCell(itemLineCell);
                }
                //row 2, col 2
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk("2-1E", largestFontBoldBlack));
                    phrase.Add(new Chunk("\n"));

                    itemLineCell = new PdfPCell(phrase);
                    itemLineCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    itemLineCell.VerticalAlignment = Element.ALIGN_MIDDLE;

                    itemLineCell.BorderWidthTop = 0;
                    itemLineCell.BorderWidthBottom = 0;
                    itemLineCell.BorderWidthLeft = 0;
                    itemLineCell.BorderWidthRight = 0;
                    itemLineCell.PaddingTop = 5f;
                    itemLineCell.PaddingBottom = 2f;
                    itemLineCell.Colspan = 3;

                    itemLineTable.AddCell(itemLineCell);
                }

                document.Add(itemLineTable);
            }

            //table 2: 2 col
            {
                var itemLineTable = new PdfPTable(4);
                var phrase = new Phrase();
                var itemLineCell = new PdfPCell(phrase);

                itemLineTable.WidthPercentage = 100f;
                itemLineTable.DefaultCell.BorderWidth = 0;
                itemLineTable.SetWidths(new[] { 23f, 36f, 13f, 28f });

                //itemLineTable.SetWidths(new[] { 23f, 19f, 28f, 30f });


                //row 1, col 3
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk("Brand: ", largeFontBlack));
                    phrase.Add(new Chunk("\n"));

                    itemLineCell = new PdfPCell(phrase);
                    itemLineCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    itemLineCell.VerticalAlignment = Element.ALIGN_MIDDLE;

                    itemLineCell.BorderWidthTop = 0;
                    itemLineCell.BorderWidthBottom = 0;
                    itemLineCell.BorderWidthLeft = 0;
                    itemLineCell.BorderWidthRight = 0;
                    itemLineCell.PaddingTop = 35f;
                    itemLineCell.PaddingBottom = 5f;
                    itemLineCell.Colspan = 3;

                    itemLineTable.AddCell(itemLineCell);
                }

                //row 1, col 4
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk("Bella Canvas", largeFontBoldBlack));
                    phrase.Add(new Chunk("\n"));

                    itemLineCell = new PdfPCell(phrase);
                    itemLineCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    itemLineCell.VerticalAlignment = Element.ALIGN_MIDDLE;

                    itemLineCell.BorderWidthTop = 0;
                    itemLineCell.BorderWidthBottom = 0;
                    itemLineCell.BorderWidthLeft = 0;
                    itemLineCell.BorderWidthRight = 0;
                    itemLineCell.PaddingTop = 35f;
                    itemLineCell.PaddingBottom = 5f;

                    itemLineTable.AddCell(itemLineCell);
                }

                //row 2, col 1
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk("Style: ", largeFontBlack));
                    phrase.Add(new Chunk("\n"));

                    itemLineCell = new PdfPCell(phrase);
                    itemLineCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    itemLineCell.VerticalAlignment = Element.ALIGN_MIDDLE;

                    itemLineCell.BorderWidthTop = 0;
                    itemLineCell.BorderWidthBottom = 0;
                    itemLineCell.BorderWidthLeft = 0;
                    itemLineCell.BorderWidthRight = 0;
                    itemLineCell.PaddingTop = 15f;
                    itemLineCell.PaddingBottom = 5f;

                    itemLineTable.AddCell(itemLineCell);
                }

                //row 2, col 2
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk("G6400", largeFontBoldBlack));
                    phrase.Add(new Chunk("\n"));

                    itemLineCell = new PdfPCell(phrase);
                    itemLineCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    itemLineCell.VerticalAlignment = Element.ALIGN_MIDDLE;

                    itemLineCell.BorderWidthTop = 0;
                    itemLineCell.BorderWidthBottom = 0;
                    itemLineCell.BorderWidthLeft = 0;
                    itemLineCell.BorderWidthRight = 0;
                    itemLineCell.PaddingTop = 15f;
                    itemLineCell.PaddingBottom = 5f;

                    itemLineTable.AddCell(itemLineCell);
                }

                //row 2, col 3
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk("SKU: ", largeFontBlack));
                    phrase.Add(new Chunk("\n"));

                    itemLineCell = new PdfPCell(phrase);
                    itemLineCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    itemLineCell.VerticalAlignment = Element.ALIGN_MIDDLE;

                    itemLineCell.BorderWidthTop = 0;
                    itemLineCell.BorderWidthBottom = 0;
                    itemLineCell.BorderWidthLeft = 0;
                    itemLineCell.BorderWidthRight = 0;
                    itemLineCell.PaddingTop = 15f;
                    itemLineCell.PaddingBottom = 5f;

                    itemLineTable.AddCell(itemLineCell);
                }

                //row 2, col 4
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk("002-016-005", largeFontBoldBlack));
                    phrase.Add(new Chunk("\n"));

                    itemLineCell = new PdfPCell(phrase);
                    itemLineCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    itemLineCell.VerticalAlignment = Element.ALIGN_MIDDLE;

                    itemLineCell.BorderWidthTop = 0;
                    itemLineCell.BorderWidthBottom = 0;
                    itemLineCell.BorderWidthLeft = 0;
                    itemLineCell.BorderWidthRight = 0;
                    itemLineCell.PaddingTop = 15f;
                    itemLineCell.PaddingBottom = 5f;

                    itemLineTable.AddCell(itemLineCell);
                }

                //row 3, col 1
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk("Color: ", largeFontBlack));
                    phrase.Add(new Chunk("\n"));

                    itemLineCell = new PdfPCell(phrase);
                    itemLineCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    itemLineCell.VerticalAlignment = Element.ALIGN_MIDDLE;

                    itemLineCell.BorderWidthTop = 0;
                    itemLineCell.BorderWidthBottom = 0;
                    itemLineCell.BorderWidthLeft = 0;
                    itemLineCell.BorderWidthRight = 0;
                    itemLineCell.PaddingTop = 15f;
                    itemLineCell.PaddingBottom = 5f;

                    itemLineTable.AddCell(itemLineCell);
                }

                //row 3, col 2
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk("AQUA TRIBLEND", largeFontBoldBlack));
                    phrase.Add(new Chunk("\n"));

                    itemLineCell = new PdfPCell(phrase);
                    itemLineCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    itemLineCell.VerticalAlignment = Element.ALIGN_MIDDLE;

                    itemLineCell.BorderWidthTop = 0;
                    itemLineCell.BorderWidthBottom = 0;
                    itemLineCell.BorderWidthLeft = 0;
                    itemLineCell.BorderWidthRight = 0;
                    itemLineCell.PaddingTop = 15f;
                    itemLineCell.PaddingBottom = 5f;

                    itemLineTable.AddCell(itemLineCell);
                }

                //row 3, col 3
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk("PO #: ", largeFontBlack));
                    phrase.Add(new Chunk("\n"));

                    itemLineCell = new PdfPCell(phrase);
                    itemLineCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    itemLineCell.VerticalAlignment = Element.ALIGN_MIDDLE;

                    itemLineCell.BorderWidthTop = 0;
                    itemLineCell.BorderWidthBottom = 0;
                    itemLineCell.BorderWidthLeft = 0;
                    itemLineCell.BorderWidthRight = 0;
                    itemLineCell.PaddingTop = 15f;
                    itemLineCell.PaddingBottom = 5f;

                    itemLineTable.AddCell(itemLineCell);
                }

                //row 3, col 4
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk("37", largeFontBoldBlack));
                    phrase.Add(new Chunk("\n"));

                    itemLineCell = new PdfPCell(phrase);
                    itemLineCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    itemLineCell.VerticalAlignment = Element.ALIGN_MIDDLE;

                    itemLineCell.BorderWidthTop = 0;
                    itemLineCell.BorderWidthBottom = 0;
                    itemLineCell.BorderWidthLeft = 0;
                    itemLineCell.BorderWidthRight = 0;
                    itemLineCell.PaddingTop = 15f;
                    itemLineCell.PaddingBottom = 5f;

                    itemLineTable.AddCell(itemLineCell);
                }

                //row 4, col 1
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk("Size: ", largeFontBlack));
                    phrase.Add(new Chunk("\n"));

                    itemLineCell = new PdfPCell(phrase);
                    itemLineCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    itemLineCell.VerticalAlignment = Element.ALIGN_MIDDLE;

                    itemLineCell.BorderWidthTop = 0;
                    itemLineCell.BorderWidthBottom = 0;
                    itemLineCell.BorderWidthLeft = 0;
                    itemLineCell.BorderWidthRight = 0;
                    itemLineCell.PaddingTop = 15f;
                    itemLineCell.PaddingBottom = 5f;

                    itemLineTable.AddCell(itemLineCell);
                }

                //row 4, col 2
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk("2X", largeFontBoldBlack));
                    phrase.Add(new Chunk("\n"));

                    itemLineCell = new PdfPCell(phrase);
                    itemLineCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    itemLineCell.VerticalAlignment = Element.ALIGN_MIDDLE;

                    itemLineCell.BorderWidthTop = 0;
                    itemLineCell.BorderWidthBottom = 0;
                    itemLineCell.BorderWidthLeft = 0;
                    itemLineCell.BorderWidthRight = 0;
                    itemLineCell.PaddingTop = 15f;
                    itemLineCell.PaddingBottom = 5f;

                    itemLineTable.AddCell(itemLineCell);
                }

                //row 4, col 3
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk("Rcvd: ", largeFontBlack));
                    phrase.Add(new Chunk("\n"));

                    itemLineCell = new PdfPCell(phrase);
                    itemLineCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    itemLineCell.VerticalAlignment = Element.ALIGN_MIDDLE;

                    itemLineCell.BorderWidthTop = 0;
                    itemLineCell.BorderWidthBottom = 0;
                    itemLineCell.BorderWidthLeft = 0;
                    itemLineCell.BorderWidthRight = 0;
                    itemLineCell.PaddingTop = 15f;
                    itemLineCell.PaddingBottom = 5f;

                    itemLineTable.AddCell(itemLineCell);
                }

                //row 4, col 4
                {
                    phrase = new Phrase();
                    phrase.Add(new Chunk("Feb 21", largeFontBoldBlack));
                    phrase.Add(new Chunk("\n"));

                    itemLineCell = new PdfPCell(phrase);
                    itemLineCell.HorizontalAlignment = Element.ALIGN_LEFT;
                    itemLineCell.VerticalAlignment = Element.ALIGN_MIDDLE;

                    itemLineCell.BorderWidthTop = 0;
                    itemLineCell.BorderWidthBottom = 0;
                    itemLineCell.BorderWidthLeft = 0;
                    itemLineCell.BorderWidthRight = 0;
                    itemLineCell.PaddingTop = 15;
                    itemLineCell.PaddingBottom = 5f;

                    itemLineTable.AddCell(itemLineCell);
                }

                document.Add(itemLineTable);
            }
            #endregion
        }

        //private static float CalculatePdfPTableHeight(PdfPTable table)
        //{
        //    using (MemoryStream ms = new MemoryStream())
        //    {

        //        var pageSize = new Rectangle(288, 432);
        //        using (Document doc = new Document(pageSize, 1, 1, 1, 1))
        //        {
        //            using (PdfWriter w = PdfWriter.GetInstance(doc, ms))
        //            {
        //                doc.Open();
        //                doc.Add(table);
        //                doc.Close();
        //                return table.TotalHeight;
        //            }
        //        }
        //    }
        //}
    }
}
