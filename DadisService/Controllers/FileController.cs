using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace DadisService.Controllers
{
    public class FileController : ApiController
    {

        [HttpPost]
        public int UploadImage (byte[] file)
        {

            return 1;

        }

        [HttpPost]
        public int UploadFile(byte[] file)
        {

            return 1;

        }

    }
}
