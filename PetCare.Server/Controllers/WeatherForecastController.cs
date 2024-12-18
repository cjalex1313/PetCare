using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PetCare.FileService;
using PetCare.Shared.Common;

namespace PetCare.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly IFileService _fileService;

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public WeatherForecastController(IFileService fileService)
        {
            _fileService = fileService;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet("Secure")]
        [Authorize]
        public IEnumerable<WeatherForecast> GetSecure()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpPost("TestGetFile")]
        public async Task<IActionResult> TestGetFile([FromBody] string key)
        {
            var bytes = await _fileService.DownloadFile(key);
            return Ok(bytes);
        }

        [HttpPost("GetFileUrl")]
        public IActionResult GetFileUrl([FromBody] string key)
        {
            var fileUrl = _fileService.GetFileUrl(key);
            return Ok(fileUrl);
        }

        [HttpPost("TestFileUpload")]
        public async Task<IActionResult>  TestFileUpload(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file was uploaded.");
            }

            // Get file name and extension
            var fileName = file.FileName;
            var fileExtension = Path.GetExtension(fileName).Substring(1);

            // Read file into a byte array
            byte[] fileBytes;
            using (var memoryStream = new MemoryStream())
            {
                await file.CopyToAsync(memoryStream);
                fileBytes = memoryStream.ToArray();
            }
            var id = Guid.NewGuid();
            await _fileService.UploadFile(fileBytes, fileExtension, id.ToString());
            // Example: Return file details for testing purposes
            return Ok(new
            {
                FileName = fileName,
                Id = id
            });
        }
    }
}
