using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using Org.BouncyCastle.Utilities;
using System.Drawing;
using testTask.Data;
using testTask.Forms;
using testTask.Models;

namespace testTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InformationController : ControllerBase
    {
        private readonly DbData _dbContext;
        private readonly IMapper _mapper;


        public InformationController(DbData dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<List<Information>>> GetInformation()
        {
            var personalInformatioms = await _dbContext.informations.ToListAsync();

            return Ok(personalInformatioms);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("withCoursesAndPromotion")]
        public async Task<ActionResult<IEnumerable<Information>>> InformationWithCoursesAndPromotion()
        {
            var result = await _dbContext.informations
                .Include(p => p.coursesOfPersonals)
                .Include(p => p.promotionsOfPersonals)
                .Select(p => new
                {
                    p.InfoId,
                    p.StatisticalNumber,
                    p.MilitaryRank,
                    p.FullName,
                    p.Brith,
                    p.CourseNumber,
                    p.CourseDate,
                    p.JoiningDate,
                    p.AcademicAchievement,
                    p.Notes,
                 
                    // Add other properties you want to include from the Information entity...
                    CoursesOfPersonals = p.coursesOfPersonals.Select(c => new
                    {
                        c.Id,
                        c.Name,
                        c.DateFrom,
                        c.DateTo,
                        // Add other properties you want to include from the CourseOfPersonal entity...
                    }),
                    PromotionsOfPersonals = p.promotionsOfPersonals.Select(pr => new
                    {
                        pr.Id,
                        pr.Name,
                        pr.Date,
                        // Add other properties you want to include from the PromotionOfPersonal entity...
                    })
                })
                .ToListAsync();

            return Ok(result);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("withCoursesAndPromotionID")]
        public async Task<ActionResult<IEnumerable<Information>>> InformationWithCoursesAndPromotionID(int ID)
        {
            var result = await _dbContext.informations
                .Where(p => p.InfoId == ID)
                .Include(p => p.coursesOfPersonals)
                .Include(p => p.promotionsOfPersonals)
                .Select(p => new
                {
                    p.StatisticalNumber,
                    p.MilitaryRank,
                    p.FullName,
                    p.Brith,
                    p.CourseNumber,
                    p.CourseDate,
                    p.JoiningDate,
                    p.AcademicAchievement,
                    p.Notes,

                    // Add other properties you want to include from the Information entity...
                    CoursesOfPersonals = p.coursesOfPersonals.Select(c => new
                    {
                        c.Id,
                        c.Name,
                        c.DateFrom,
                        c.DateTo,
                        // Add other properties you want to include from the CourseOfPersonal entity...
                    }),
                    PromotionsOfPersonals = p.promotionsOfPersonals.Select(pr => new
                    {
                        pr.Id,
                        pr.Name,
                        pr.Date,
                        // Add other properties you want to include from the PromotionOfPersonal entity...
                    })
                })
                .ToListAsync();

            return Ok(result);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddInformation([FromBody] InformationForm informatiomForm)
        {
            //if (informatiomForm.FullName == string.Empty
            //    || informatiomForm.Brith == string.Empty
            //    || informatiomForm.CourseNumber == string.Empty
            //    || informatiomForm.JoiningDate == null
            //    || informatiomForm.AcademicAchievement == string.Empty)
            //{
            //    return BadRequest(" يجب ادخال جميع البيانات ");
            //}
            var info = _mapper.Map<Information>(informatiomForm);

            await _dbContext.informations.AddAsync(info);
            _dbContext.SaveChanges();
            return Ok("تمت اضافة البيانات");
        }
        [Authorize(Roles = "Admin")]
        [HttpPost("uploadExcelFile")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("Invalid file");
            }
            
            var existingStatisticalNumbers = new HashSet<string>(); // to keep track of existing statistical numbers
            using (var stream = file.OpenReadStream())
            using (var package = new ExcelPackage(stream))
            {
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
                for (int row = 2; row <= worksheet.Dimension.End.Row; row++)
                {
                    var statisticalNumber = worksheet.Cells[row, 1].Value.ToString();
                    if (existingStatisticalNumbers.Contains(statisticalNumber)) // if statistical number already exists, skip the row
                    {
                        continue;
                    }

                    existingStatisticalNumbers.Add(statisticalNumber); // add the statistical number to the set
                    var information = new Information
                    {
                        StatisticalNumber = statisticalNumber,
                        MilitaryRank = worksheet.Cells[row, 2].Value.ToString(),
                        FullName = worksheet.Cells[row, 3].Value.ToString(),
                        Brith = worksheet.Cells[row, 4].Value.ToString(),
                        CourseNumber = worksheet.Cells[row, 5].Value.ToString(),
                        //CourseDate = DateTime.Parse(worksheet.Cells[row, 6].Value.ToString()),
                        //JoiningDate = DateTime.Parse(worksheet.Cells[row, 7].Value.ToString()),
                        AcademicAchievement = worksheet.Cells[row, 8].Value.ToString(),
                        Notes = worksheet.Cells[row, 9].Value.ToString(),
                    };
                    _dbContext.informations.Add(information);
                    
                }
                await _dbContext.SaveChangesAsync();
            }

            return Ok("تم حفظ البيانات ");
        }


        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> UpdatePersonalInformation(int id, [FromBody] InformationForm updateInfo)
        {
            var info = await _dbContext.informations.FindAsync(id);
            if (info == null)
            {
                return BadRequest("العنصر المحدد غير موجود");
            }
            info.StatisticalNumber= updateInfo.StatisticalNumber;
            info.MilitaryRank= updateInfo.MilitaryRank;
            info.FullName = updateInfo.FullName;
            info.Brith = updateInfo.Brith;
            info.CourseNumber = updateInfo.CourseNumber;
            info.CourseDate= updateInfo.CourseDate;
            info.JoiningDate = updateInfo.JoiningDate;
            info.AcademicAchievement = updateInfo.AcademicAchievement;
            info.Notes = updateInfo.Notes;
            await _dbContext.SaveChangesAsync();
            return Ok("تمت عملية التعديل");
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public async Task<IActionResult> DeletePersonalInformatiom([FromBody] int InfoID)
        {
            var info = await _dbContext.informations.FindAsync(InfoID);
            _dbContext.informations.Remove(info);


            await _dbContext.SaveChangesAsync();
            return Ok("تمت عملية الحذف");
        }

    }
}
