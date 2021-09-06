using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using pdf = PdfSharpCore;
using pdfIO = PdfSharpCore.Pdf.IO;
using PdfSharpCore.Pdf.Security;
using System.Text;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;
using FunctionApp1.Entity;

namespace FunctionApp1
{
    public static class PDFSecure
    {
        [FunctionName("PdfSecure")]
        public static async Task<HttpResponseMessage> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogInformation("C# HTTP trigger function processed a request.");
            try
            {
                string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
                HttpInput inputEntity = JsonConvert.DeserializeObject<HttpInput>(requestBody);

                var imageBytes = Convert.FromBase64String(inputEntity.Pdf);
                MemoryStream mem = new MemoryStream(imageBytes);                
                

                pdf.Pdf.PdfDocument document = pdf.Pdf.IO.PdfReader.Open(mem, pdfIO.PdfDocumentOpenMode.Modify);
                
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                var enc1252 = Encoding.GetEncoding(1252);

                PdfSecuritySettings securitySettings = document.SecuritySettings;

                
                securitySettings.UserPassword = inputEntity.Password;
                securitySettings.OwnerPassword = inputEntity.Password;

                

                // Restrict some rights.
                securitySettings.PermitAccessibilityExtractContent = false;
                securitySettings.PermitAnnotations = false;
                securitySettings.PermitAssembleDocument = false;
                securitySettings.PermitExtractContent = false;
                securitySettings.PermitFormsFill = true;
                securitySettings.PermitFullQualityPrint = false;
                securitySettings.PermitModifyDocument = true;
                securitySettings.PermitPrint = false;
                
                MemoryStream ms = new MemoryStream();
                document.Save(ms);
                ms.Position = 0;
                HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                response.Content = new ByteArrayContent(ms.ToArray());
                response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                {
                    FileName = "PDFDocument.pdf"
                };
                response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/pdf");
                return response;



            }
            catch (Exception ex)
            {
                throw ex;
                    }
        }
    }
}
