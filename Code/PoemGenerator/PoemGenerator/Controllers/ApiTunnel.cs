using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.IO;

using PoemGenerator.Models;

namespace PoemGenerator.Controllers
{
    
    public class ApiTunnelController : ApiController
    {
        public async Task<HttpResponseMessage> PostFormData()
    {
        // Check if the request contains multipart/form-data.
        if (!Request.Content.IsMimeMultipartContent())
        {
            throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
        }

        string root = HttpContext.Current.Server.MapPath("~/App_Data");
        var provider = new MultipartFormDataStreamProvider(root);

        try
        {
            // Read the form data.
            await Request.Content.ReadAsMultipartAsync(provider);

            // This illustrates how to get the file names.
            foreach (MultipartFileData file in provider.FileData)
            {
                    string[] FileText;
                    string poem = "";
                        if (File.Exists(file.LocalFileName))
                        {
                            FileText = File.ReadAllLines(file.LocalFileName);
                        PoemCore poemObj = new PoemCore(FileText);
                        poem=poemObj.GetPoem();

                        var response = new HttpResponseMessage();
                        response.Content = new StringContent("<html><body> <h2>"+poem+"</h2></body></html>");
                        response.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("text/html");
                        return response;
                    }
                        else
                        {
                            throw new FileNotFoundException();
                        }
                    
                    Trace.WriteLine(file.Headers.ContentDisposition.FileName);
                Trace.WriteLine("Server file path: " + file.LocalFileName);
                    Trace.WriteLine(FileText[0]+FileText[1]);
            }
            return Request.CreateResponse(HttpStatusCode.OK);
        }
        catch (System.Exception e)
        {
            return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, e);
        }
    }



    }
}
