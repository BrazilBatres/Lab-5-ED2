using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.Text;
using System.Text.Json;
using CypherClasses;


namespace API_Interface.Controllers
{
    [ApiController]
    [Route("api")]
    public class CipherController : ControllerBase
    {
        private IWebHostEnvironment _env;
        public CipherController(IWebHostEnvironment env)
        {
            _env = env;
        }

        [HttpPost]
        [Route("cipher/{method}")]
        public async Task<ActionResult> Cypher(string method, [FromForm] IFormFile file, CypherClasses.CipherKey key)
        {
            string path = _env.ContentRootPath;
            string OriginalName = file.FileName;
            string uploadPath = path + @"\Uploads\" + OriginalName;
            byte[] FileBytes;
            
            //try
            //{
                if (file != null)
                {
                    string filename = file.FileName.Substring(0, file.FileName.Length - 4);
                    using (FileStream fs = System.IO.File.Create(uploadPath))
                    {
                        await file.CopyToAsync(fs);
                    }
                    method = method.ToLower();
                    switch (method)
                    {
                        case "cesar":
                            César césar = new César();
                            césar.SetKey(key.Word);
                            césar.Cipher(uploadPath, out FileBytes);
                            return File(FileBytes, "text/plain", filename + ".csr");

                        case "zigzag":
                            ZigZag zigzag = new ZigZag();
                            zigzag.SetLevels(key.Levels);
                            zigzag.Cipher(uploadPath, out FileBytes);
                            return File(FileBytes, "text/plain", filename + ".zz");

                        case "ruta":
                            Ruta ruta = new Ruta();
                            ruta.SetSize(key.Rows, key.Columns);
                            ruta.Cipher(uploadPath, out FileBytes);
                            return File(FileBytes, "text/plain", filename + ".rt");

                        default:
                            return StatusCode(500);
                    }
                    
                }
                else
                {
                    return StatusCode(500);
                }
            //}
            //catch (Exception)
            //{
            //    return StatusCode(500);
            //}
        }
        //[Route("decompress")]
        //public async Task<ActionResult> Decompress([FromForm] IFormFile file)
        //{
        //    string path = _env.ContentRootPath;
        //    string OriginalName = file.FileName;
        //    OriginalName = OriginalName.Substring(0, OriginalName.Length - 4);
        //    string downloadPath = path + @"\Compressions\" + OriginalName;
        //    byte[] FileBytes;
        //    try
        //    {
        //        if (file != null)
        //        {
        //            using (FileStream fs = System.IO.File.Create(downloadPath))
        //            {
        //                await file.CopyToAsync(fs);
        //            }
        //            LZW Compressor = new LZW(downloadPath);
        //            FileBytes = Compressor.Decompress(downloadPath, 100);
        //            return File(FileBytes, "text/plain", Compressor.Name); ;
        //        }
        //        else
        //        {
        //            return StatusCode(500);
        //        }
        //    }
        //    catch (Exception)
        //    {
        //        return StatusCode(500);
        //    }
        //}

    }
}
