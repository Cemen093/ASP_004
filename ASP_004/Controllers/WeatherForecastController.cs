using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

namespace ASP_004.Controllers
{
    /*Создать CoreApi для загрузки/файлов c логгированием ip адрессов в отдельную БД*/
    [ApiController]
    [Route("/api/[controller]")]
    public class DonwloadFileController : ControllerBase
    {
        string connStr = new string("Server=tcp:my-server93-2.database.windows.net,1433;Initial Catalog=ASP_004;Persist Security Info=False;User ID=admin93;Password=PASSworld93;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");

        [HttpGet("{fileName}")]
        public IActionResult Get(string fileName)
        {
            string filePath = Directory.GetCurrentDirectory() + $"/Files/{fileName}";
            string fileType = "application/octet-stream";
            byte[] mas = System.IO.File.ReadAllBytes(filePath);

            using (SqlConnection connection = new SqlConnection(connStr))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand($"INSERT INTO [dbo].[Log] VALUES ({HttpContext.Connection.RemoteIpAddress.ToString()}, \'{fileName}\')", connection))
                {
                    command.ExecuteNonQuery();
                }
            }
            return PhysicalFile(filePath, fileType, fileName);
        }
    }
}