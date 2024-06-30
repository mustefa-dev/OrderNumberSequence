using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OrderNumberSequence.DATA.DTOs.User;
using OrderNumberSequence.Entities;
using OrderNumberSequence.Interface;
using OrderNumberSequence.Repository;

namespace OrderNumberSequence.Services{
    public interface IUserService{
        Task<(UserDto? user, string? error)> Login(LoginForm loginForm);
        Task<(AppUser? user, string? error)> DeleteUser(Guid id);
        Task<(UserDto? UserDto, string? error)> Register(RegisterForm registerForm);
        Task<(AppUser? user, string? error)> UpdateUser(UpdateUserForm updateUserForm, Guid id);

        Task<(UserDto? user, string? error)> GetUserById(Guid id);


        Task<(List<UserDto>? user, int? totalCount, string? error)> GetAll(UserFilter filter);

    }

    public class UserService : IUserService{
        private readonly IRepositoryWrapper _repositoryWrapper;
        private readonly IMapper _mapper;
        private readonly ITokenService _tokenService;

        public UserService(IRepositoryWrapper repositoryWrapper, IMapper mapper, ITokenService tokenService) {
            _repositoryWrapper = repositoryWrapper;
            _mapper = mapper;
            _tokenService = tokenService;
        }

      
        public async Task<(UserDto? user, string? error)> Login(LoginForm loginForm)
        {
            var user = await _repositoryWrapper.User.Get(u => u.Email == loginForm.Email);
            if (user == null) return (null, "المستخدم غير متوفر");
            if (!BCrypt.Net.BCrypt.Verify(loginForm.Password, user.Password)) return (null, "خطاء في الرقم السري");
            var userDto = _mapper.Map<UserDto>(user);
            var TokenDto = _mapper.Map<TokenDTO>(user);
            userDto.Token = _tokenService.CreateToken(TokenDto);
            return (userDto, null);
        }

        public async Task<(AppUser? user, string? error)> DeleteUser(Guid id) {
            var user = await _repositoryWrapper.User.Get(u => u.Id == id);
            if (user == null) return (null, "User not found");
            await _repositoryWrapper.User.SoftDelete(id);
            return (user, null);
        }

        public async Task<(UserDto? UserDto, string? error)> Register(RegisterForm registerForm) 
        {
            var user = await _repositoryWrapper.User.Get(u => u.Email == registerForm.Email);
            if (user != null) return (null, "User already exists");

            var newUser = new AppUser
            {
                Email = registerForm.Email,
                Role = (UserRole)(Enum)Enum.Parse(typeof(UserRole), registerForm.Role),
                FullName = registerForm.FullName,
                Password = BCrypt.Net.BCrypt.HashPassword(registerForm.Password),
            };

            await _repositoryWrapper.User.CreateUser(newUser);

           
            var userDto = _mapper.Map<UserDto>(newUser);
            var TokenDto = _mapper.Map<TokenDTO>(newUser);
            userDto.Token = _tokenService.CreateToken(TokenDto);
            return (userDto, null);
        }
        

        public async Task<(AppUser? user, string? error)> UpdateUser(UpdateUserForm updateUserForm, Guid id) {
            var user = await _repositoryWrapper.User.Get(u => u.Id == id);
            if (user == null) return (null, "User not found");
          
           
            user = _mapper.Map(updateUserForm, user);
            await _repositoryWrapper.User.UpdateUser(user);
            return (user, null);
        }

        public async Task<(UserDto? user, string? error)> GetUserById(Guid id) {
            var user = await _repositoryWrapper.User.Get(u => u.Id == id);
            if (user == null) return (null, "User not found");
            var userDto = _mapper.Map<UserDto>(user);
            return (userDto, null);
        }

        public async Task<(List<UserDto>? user, int? totalCount, string? error)> GetAll(UserFilter filter)
        {
            var (users , totalCount) = await _repositoryWrapper.User.GetAll<UserDto>(
                x => (
                    (filter.FullName == null || x.FullName.Contains(filter.FullName)) &&
                    (filter.Email == null || x.Email.Contains(filter.Email)) &&
                    (filter.IsActive == null || x.IsActive.Equals(filter.IsActive))
                    ) ,
                filter.PageNumber , filter.PageSize);
            return (users , totalCount , null);
        }
    }
}