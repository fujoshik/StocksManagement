using Accounts.Domain.Abstraction.Repositories;
using Accounts.Domain.Abstraction.Services;
using Accounts.Domain.DTOs.Account;
using Accounts.Domain.DTOs.User;
using AutoMapper;

namespace Accounts.Domain.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        public async Task<UserResponseDto> CreateAsync(RegisterDto registerDto, Guid accountId)
        {
            var user = _mapper.Map<UserRequestDto>(registerDto);
            user.AccountId = accountId;

            return await _userRepository.InsertAsync<UserRequestDto, UserResponseDto>(user);
        }
    }
}
