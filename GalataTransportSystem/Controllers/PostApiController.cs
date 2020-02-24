using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using GalataTransportSystem.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace GalataTransportSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostApiController : ControllerBase
    {


        private IConfiguration _configuration;
        public PostApiController(IConfiguration iconfig)
        {
            _configuration = iconfig;
        }
        [HttpPost]
        public async Task<HttpResponseMessage> PostAsync([FromForm] Customer value)
        {

            if (value == null)
                return new HttpResponseMessage { StatusCode = HttpStatusCode.BadRequest, ReasonPhrase = "POST body is null" };

            try
            {

                Link c = new Link();
                c.Url = _configuration.GetValue<string>("Link:Url");

                var json = JsonConvert.SerializeObject(value);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                var url = "https://postman-echo.com/post";
                using var client = new HttpClient();

                var response = await client.PostAsync(url, data);

                string Token = response.Content.ReadAsStringAsync().Result;

                return new HttpResponseMessage { StatusCode = HttpStatusCode.OK, ReasonPhrase = "Saved" };
           
            }
            catch (Exception ex)
            {
                return new HttpResponseMessage { StatusCode = HttpStatusCode.InternalServerError, ReasonPhrase = $"Document could not be created: {ex.InnerException}" };
            }


        }


    }
}