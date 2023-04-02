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
    public class PromotionOfPersonalController : ControllerBase
    {
        private readonly DbData _dbContext;
        private readonly IMapper _mapper;


        public PromotionOfPersonalController(DbData dbContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<List<CoursesOfPersonal>>> GetInPromotions()
        {
            var promotions = await _dbContext.promotionsOfPersonals.ToListAsync();

            return Ok(promotions);
        }
        [Authorize(Roles = "Admin")]
        [HttpGet("ForPersonal")]
        public async Task<ActionResult<List<CoursesOfPersonal>>> GetInPromotionsForPersonal(int IdOfPersonal)
        {
            var promotions = await _dbContext.promotionsOfPersonals.Where(p=>p.InformationId==IdOfPersonal).ToListAsync();

            return Ok(promotions);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> AddCourses([FromBody] PromotionOfPersonalForm promotionsForm)
        {

            var promotions = _mapper.Map<PromotionsOfPersonal>(promotionsForm);

            await _dbContext.promotionsOfPersonals.AddAsync(promotions);
            _dbContext.SaveChanges();
            return Ok("تمت اضافة البيانات");
        }
        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> UpdatePromotion(int id, [FromBody] PromotionOfPersonalForm promotionForm)
        {
            var promotion = await _dbContext.promotionsOfPersonals.FindAsync(id);
            if (promotion == null)
            {
                return BadRequest("العنصر المحدد غير موجود");
            }
            promotion.Name = promotionForm.Name;
            promotion.Date=promotionForm.Date;
            promotion.InformationId=promotionForm.InformationId;
           
            await _dbContext.SaveChangesAsync();
            return Ok("تمت عملية التعديل");
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public async Task<IActionResult> DeletePromotion([FromBody] int PromotionID)
        {
            var promotion = await _dbContext.promotionsOfPersonals.FindAsync(PromotionID);
            _dbContext.promotionsOfPersonals.Remove(promotion);


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
                                var promotiom = new PromotionsOfPersonal
                                {
                                    Name = worksheet.Cells[row, 2]?.Value?.ToString(),
                                    Date = DateTime.Parse(worksheet.Cells[row, 3]?.Value?.ToString()),
                                    InformationId = information.InfoId
                                };
                                Console.Write(information);
                                _dbContext.promotionsOfPersonals.Add(promotiom);

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
