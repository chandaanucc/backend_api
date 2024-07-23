namespace Shareplus.DataLayer.Service
{
    public class SendPdfRequest
        {
            public int PdfId { get; set; }
            public List<int> ClientIds { get; set; }
        }

}