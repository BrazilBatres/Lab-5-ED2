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
        public async Task<ActionResult> Cypher(string method, [FromForm] CipherInput Key)
        {
            string path = _env.ContentRootPath;
            string OriginalName = Key.File.FileName;
            string uploadPath = path + @"\Cipher\" + OriginalName;
            byte[] FileBytes;

            //try
            //{
                if (Key.File != null)
                {
                    bool okCypher = false;
                    string filename = Key.File.FileName.Substring(0, Key.File.FileName.Length - 4);
                    using (FileStream fs = System.IO.File.Create(uploadPath))
                    {
                        await Key.File.CopyToAsync(fs);
                    }
                    method = method.ToLower();
                    switch (method)
                    {
                        case "cesar":
                            César césar = new César();
                            okCypher = césar.Cipher(uploadPath, out FileBytes, Key.Word);
                            if (okCypher==false)
                            {
                                return StatusCode(500);
                            };
                            return File(FileBytes, "text/plain", filename + ".csr");

                        case "zigzag":
                            ZigZag zigzag = new ZigZag();
                            okCypher = zigzag.Cipher(uploadPath, out FileBytes, Key.Levels);
                            if (okCypher==false)
                            {
                                return StatusCode(500);
                            }
                            return File(FileBytes, "text/plain", filename + ".zz");

                        case "ruta":
                            int[] Size = new int[2];
                            Size[0] = Key.Rows;
                            Size[1] = Key.Columns;
                            Ruta ruta = new Ruta();
                            okCypher = ruta.Cipher(uploadPath, out FileBytes, Size);
                            if (okCypher == false)
                            {
                                return StatusCode(500);
                            }
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



        [HttpPost]
        [Route("decipher")]
        public async Task<ActionResult> Decypher([FromForm] CipherInput Key)
        {
            string path = _env.ContentRootPath;
            string OriginalName = Key.File.FileName;
            string uploadPath = path + @"\Decipher\" + OriginalName;
            string lastChar = OriginalName.Substring(OriginalName.Length - 1, 1);
            byte[] FileBytes;

            try
            {
                if (Key.File != null)
                {
                    bool okCypher = false;
                    using (FileStream fs = System.IO.File.Create(uploadPath))
                    {
                        await Key.File.CopyToAsync(fs);
                    }
                string filename = "";
                    switch (lastChar)
                    {
                        case "r":

                            César césar = new César();
                            okCypher = césar.Decipher(uploadPath, out FileBytes, Key.Word);
                            if (okCypher==false)
                            {
                                return StatusCode(500);
                            }
                            filename = Key.File.FileName.Substring(0, Key.File.FileName.Length - 4);
                            filename += ".txt";
                            return File(FileBytes, "text/plain", filename);

                        case "z":

                            ZigZag zigzag = new ZigZag();
                            okCypher = zigzag.Decipher(uploadPath, out FileBytes, Key.Levels);
                            if (okCypher == false)
                            {
                                return StatusCode(500);
                            }
                            filename = Key.File.FileName.Substring(0, Key.File.FileName.Length - 3);
                            filename += ".txt";
                            return File(FileBytes, "text/plain", filename );

                        case "t":
                            int[] Size = new int[2];
                            Size[0] = Key.Rows;
                            Size[1] = Key.Columns;
                            Ruta ruta = new Ruta();
                            okCypher = ruta.Decipher(uploadPath, out FileBytes, Size);
                            if (okCypher==false)
                            {
                                return StatusCode(500);
                            }
                            filename = Key.File.FileName.Substring(0, Key.File.FileName.Length - 3);
                            filename += ".txt";
                            return File(FileBytes, "text/plain", filename);

                        default:
                            return StatusCode(500);
                    }

                }
                else
                {
                    return StatusCode(500);
                }
            }
            catch (Exception)
            {
                return StatusCode(500);
            }
        }


    }
}
