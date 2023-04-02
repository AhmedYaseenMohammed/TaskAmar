using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Crypto.Generators;
using testTask.Data;
using testTask.Forms;
using testTask.Healper;
using testTask.Models;

namespace testTask.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly DbData _dbContext;
        private readonly IMapper _mapper;
        private readonly IJwtTokenGenerator _jwt;

        public UsersController(DbData dbContext, IMapper mapper, IJwtTokenGenerator jwtTokenGenerator)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _jwt = jwtTokenGenerator;
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<List<Users>>> Get()
        {
            return Ok(await _dbContext.users.ToListAsync());
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
         public async Task<IActionResult> AddUser([FromBody] UsersFrom userForm)
        {
            if (userForm == null)
            {
                return BadRequest();
            }

            var user = new Users();
            user.Name = userForm.Name;
            user.Email = userForm.Email;
            user.Password = BCrypt.Net.BCrypt.HashPassword(userForm.Password);
            user.role = userForm.role;

           await _dbContext.users.AddAsync(user);
            _dbContext.SaveChanges();

            return Ok("تمت الاضافة بنجاح");
        }
        [AllowAnonymous]
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginForm user)
        {
            var userFromDb = await _dbContext.users.FirstOrDefaultAsync(u => u.Email == user.Email);
            if (userFromDb == null)
            {
                return BadRequest();
            }

            if (!BCrypt.Net.BCrypt.Verify(user.Password, userFromDb.Password))
            {
                return BadRequest();
            }

            return Ok(new
            {
                message = "تمت الاضافة بنجاح",
                token = _jwt.GenerateToken(userFromDb.Id, userFromDb.role)
            });
        }


        [Authorize(Roles = "Admin")]
        [HttpPut]
        public async Task<IActionResult> UpdateUser([FromBody] Users updateUser)
        {
            var user = await _dbContext.users.FindAsync(updateUser.Id);
            if (user == null)
            {
                return BadRequest("user not found.");
            }
            user.Name = updateUser.Name;
            user.Email = updateUser.Email;
            user.Password = updateUser.Password;

            await _dbContext.SaveChangesAsync();
            return Ok("تمت عملية التعديل");
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public async Task<IActionResult> DeletePromotion([FromBody] int UserID)
        {
            var user = await _dbContext.users.FindAsync(UserID);
            _dbContext.users.Remove(user);


            await _dbContext.SaveChangesAsync();
            return Ok("تمت عملية الحذف");
        }

    }
}
