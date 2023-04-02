using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using testTask.Data;
using testTask.Forms;
using testTask.Models;

namespace testTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesOfPersonalController : ControllerBase
    {
        private readonly DbData _dbContext;
        private readonly IMapper _mapper;


        public CoursesOfPersonalController(DbData dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<List<CoursesOfPersonal>>> GetInCourses()
        {
            var courses = await _dbContext.coursesOfPersonals.ToListAsync();

            return Ok(courses);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("ForPersonal")]
        public async Task<ActionResult<List<CoursesOfPersonal>>> GetInCoursesForPersonal(int IdOfPersonal)
        {
            var courses = await _dbContext.coursesOfPersonals.Where(c=>c.InformationId==IdOfPersonal).ToListAsync();

            return Ok(courses);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddCourses([FromBody] CoursesOfPersonalForm CoursesForm)
        {
            
            var couse = _mapper.Map<CoursesOfPersonal>(CoursesForm);

            await _dbContext.coursesOfPersonals.AddAsync(couse);
            _dbContext.SaveChanges();
            return Ok("تمت اضافة البيانات");
        }
        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> UpdateCourse(int id, [FromBody] CoursesOfPersonalForm cuorseForm)
        {
            var course = await _dbContext.coursesOfPersonals.FindAsync(id);
            if (course == null)
            {
                return BadRequest("العنصر المحدد غير موجود");
            }
            course.Name = cuorseForm.Name;
            course.DateFrom = cuorseForm.DateFrom;
            course.DateTo = cuorseForm.DateTo;
            course.InformationId = cuorseForm.InformationId;
            await _dbContext.SaveChangesAsync();
            return Ok("تمت عملية التعديل");
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public async Task<IActionResult> DeleteCourse([FromBody] int CourseID)
        {
            var course = await _dbContext.coursesOfPersonals.FindAsync(CourseID);
            _dbContext.coursesOfPersonals.Remove(course);


            await _dbContext.SaveChangesAsync();
            return Ok("تمت عملية الحذف");
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("uploadExcelFile")]
        public async Task<IActionResult> UploadExcelFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("Invalid file");
            }

            try
            {
                using (var stream = file.OpenReadStream())
                using (var package = new ExcelPackage(stream))
                {
                    ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault();
                    if (worksheet != null)
                    {
                        for (int row = 2; row <= worksheet.Dimension?.End.Row; row++)
                        {
                            var statisticalNumber = worksheet.Cells[row, 1]?.Value?.ToString();
                            if (!string.IsNullOrEmpty(statisticalNumber))
                            {
                                var information = await _dbContext.informations
                                    .FirstOrDefaultAsync(i => i.StatisticalNumber == statisticalNumber);
                                Console.Write(information);
                                if (information == null)
                                {
                                    continue;
                                }
                                var Course = new CoursesOfPersonal
                                {
                                    Name = worksheet.Cells[row, 2]?.Value?.ToString(),
                                    // DateFrom = DateTime.Parse(worksheet.Cells[row, 3]?.Value?.ToString()),
                                    // DateTo = DateTime.Parse(worksheet.Cells[row, 4]?.Value?.ToString()),
                                    InformationId = information.InfoId
                                };
                                Console.Write(information);
                                _dbContext.coursesOfPersonals.Add(Course);

                                
                            }
                        }
                        await _dbContext.SaveChangesAsync();
                    }
                }
                return Ok("File uploaded successfully.");
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

    }
}
