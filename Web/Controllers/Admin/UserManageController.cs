using Domain.Entities;
using Domain.Entities.UserDto;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Microsoft.AspNetCore.Mvc;


namespace Web.Controllers.Admin
{
    public class UserManageController : Controller
    {
        private readonly IUser _userService;
        private readonly IPasswordHasher _passwordHasher;

        public UserManageController(IUser userService, IPasswordHasher passwordHasher)
        {
            _userService = userService;
            _passwordHasher = passwordHasher;
        }

        [HttpGet]
        [Route("Admin/manageuser")]
        public async Task<IActionResult> ManageUser()
        {
            var users = await _userService.GetAllUsers();
            return View("~/Views/Admin/User/ManageUser.cshtml", users);
        }

        [HttpGet]
        [Route("Admin/EditUser/{id}")]
        public async Task<IActionResult> EditUser(Guid id)
        {
            var user = await _userService.GetUserById(id);
            if (user == null)
            {
                return NotFound();
            }

            var userDto = new UpdateUserDto
            {
                UserId = user.UserId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber
            };

            return View("~/Views/Admin/User/EditUser.cshtml", userDto);
        }

        [HttpPost]
        [Route("Admin/UpdateUser/{id}")]
        public async Task<IActionResult> UpdateUser(UpdateUserDto userDto, Guid id)
        {
            if (ModelState.IsValid)
            {
                var updatedUser = await _userService.UpdateUser(userDto, id);
                if (updatedUser != null)
                {
                    return RedirectToAction("ManageUser");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Failed to update pizza.");
                }
            }

            return View("~/Views/Admin/User/EditUser.cshtml");
        }

        [HttpGet]
        [Route("Admin/AddUser")]
        public IActionResult AddUser()
        {
            return View("~/Views/Admin/User/AddUser.cshtml");
        }

        [HttpPost]
        [Route("Admin/AddUser")]
        public async Task<IActionResult> AddUser(User users)
        {
            if (ModelState.IsValid)
            {
                var userWithPassword = new User()
                {
                    UserId = Guid.NewGuid(),
                    Password = _passwordHasher.Generate(users.Password),
                    FirstName = users.FirstName,
                    LastName = users.LastName,
                    Email = users.Email,
                    PhoneNumber = users.PhoneNumber
                };
                var newUser = await _userService.CreateUser(userWithPassword);
                if (newUser != null)
                {
                    return RedirectToAction("ManageUser");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Failed to add");
                }
            }

            return View("~/Views/Admin/User/AddUser.cshtml");
        }
    }
}