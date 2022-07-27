using Application.Contracts.Interfaces;
using Domain.Constants;
using Microsoft.AspNetCore.Mvc;

namespace Vizalys.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocumentDownloadController : ControllerBase
    {
        private readonly IFileuploadServices _fileUploadService;
        private readonly IConfiguration _configuration;

        public DocumentDownloadController(IFileuploadServices fileUploadService, IConfiguration configuration)
        {
            _fileUploadService = fileUploadService;
            _configuration = configuration;
        }
        [HttpGet("{id:int}")]
        public async Task<ActionResult> DownloadFileFromFileSystem(int id, DocType docType)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var result = await _fileUploadService.DownloadFile(id);
                    if (result.IsSuccess)
                    {
                        var file = result.Result;
                        var path = new DirectoryInfo(Environment.CurrentDirectory).Parent.Parent.FullName;
                        string basePath = Path.Combine(path + "\\File\\Error\\");
                        string filedir = _configuration["FilePath"];
                        switch (docType)
                        {
                            case DocType.Invoice:
                                basePath = Path.Combine(path + filedir + "Invoice\\");
                                break;
                            case DocType.Bill:
                                basePath = Path.Combine(path + filedir + "Bill\\");
                                break;
                            case DocType.CreditNote:
                                basePath = Path.Combine(path + filedir + "CreditNote\\");
                                break;
                            case DocType.DebitNote:
                                basePath = Path.Combine(path + filedir + "DebitNote\\");
                                break;
                            case DocType.Payment:
                                basePath = Path.Combine(path + filedir + "Payment\\");
                                break;
                            case DocType.JournalEntry:
                                basePath = Path.Combine(path + filedir + "JournalEntry\\");
                                break;
                        }
                        var filePath = Path.Combine(basePath, file.Name);

                        var memory = new MemoryStream();
                        using (var stream = new FileStream(filePath, FileMode.Open))
                        {
                            await stream.CopyToAsync(memory);
                        }
                        memory.Position = 0;
                        return File(memory, file.FileType, file.Name + file.Extension);
                    }
                    return BadRequest(result);
                }
                return BadRequest("Some properties are not valid"); // Status code : 400
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                    e.Message);
            }
        }
    }
}