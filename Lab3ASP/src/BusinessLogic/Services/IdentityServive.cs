using AutoMapper;
using BusinessLogic.Interfaces;
using BusinessLogic.Models;
using Eventus.BusinessLogic.Interfaces;
using Eventus.DAL.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Eventus.BusinessLogic.Services
{
    public class IdentityService : IIdentityService
    {
        private readonly UserManager<UserDto> _userManager;
        private readonly SignInManager<UserDto> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;

        public IdentityService(UserManager<UserDto> userManager, SignInManager<UserDto> signInManager, RoleManager<IdentityRole> roleManager, IMapper mapper)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _mapper = mapper;
        }

        public async Task DeleteAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            await _userManager.DeleteAsync(user);
        }

        public async Task<User> GetByIdAsync(string id)
        {
            var dto = await _userManager.FindByIdAsync(id);
            var user = _mapper.Map<User>(dto);

            return user;
        }

        public async Task<List<User>> GetUsersAsync()
        {
            var dtos = await _userManager.Users.ToListAsync();
            var users = _mapper.Map<List<User>>(dtos);

            return users;
        }

        public async Task SignInAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            var result = await _signInManager.PasswordSignInAsync(user.UserName, password, false, false);

            if (!result.Succeeded)
            {
                throw new ValidationException("Invalid login or password");
            }
        }

        public async Task RegisterAsync(User user, string password)
        {
            var dto = _mapper.Map<UserDto>(user);
            var isEmailTaken = _userManager.Users.Any(usr => usr.Email == user.Email);
            if (isEmailTaken)
            {
                throw new ValidationException("Email already taken");
            }
            var result = await _userManager.CreateAsync(dto, password);
            if (!result.Succeeded)
            {
                var errors = string.Join(" ", result.Errors.Select(x => x.Description));
                throw new ValidationException(errors);
            }
        }

        public async Task SignOutAsync() => await _signInManager.SignOutAsync();

        public async Task UpdateAsync(User user)
        {
            var dto = _mapper.Map<UserDto>(user);
            await _userManager.UpdateAsync(dto);
        }

        public async Task<User> FindByEmailAsync(string email)
        {
            var dto = await _userManager.FindByEmailAsync(email);
            var user = _mapper.Map<User>(dto);

            return user;
        }

        public async Task CreateRoleAsync(IdentityRole role) => await _roleManager.CreateAsync(role);

        public async Task<bool> RoleExistsAsync(string roleName) => await _roleManager.RoleExistsAsync(roleName);

        public async Task AddToRoleAsync(User user, string roleName)
        {
            var dto = await _userManager.FindByIdAsync(user.Id);
            await _userManager.AddToRoleAsync(dto, roleName);
        }

    }
}
