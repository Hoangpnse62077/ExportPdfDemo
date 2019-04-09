using iTextSharp.text;
using iTextSharp.text.pdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static ConsoleAppPDF.Class3;

namespace ConsoleAppPDF
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                var pod = new PoDetailInMailModel("002-004-004", "BLACK - G6400 - XL", 72, 1.62m, 115.22m);
                List<PoDetailInMailModel> listpod = new List<PoDetailInMailModel>();
                List<TicketExportModel> models = new List<TicketExportModel>();

                for (int i = 1; i < 3; i++)
                {
                    listpod.Add(pod);
                    models.Add(new TicketExportModel());
                }

                var po = new PurchaseOrderExportInMailModel("1000", "1/20/2019", "1/24/2019", "Tiến đẹp trai",
                "Net 30", "Vendor Name", "Address Line 1", "Address Line 2", "City", "State", "Zip Code",
                "0768595768", 507.96m, 507.96m, listpod);



                //var result = Class3.ExportTicket(po);
                var result = Class3.ExportTicketLabel(models);
            }
            catch (Exception ex)
            {

                throw;
            }
            
        }
    }
}
