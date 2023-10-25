using Accounts.Domain.Abstraction.Repositories;
using Accounts.Domain.Abstraction.Services;
using Accounts.Domain.DTOs.Account;
using Accounts.Domain.DTOs.User;
using Accounts.Domain.Pagination;
using AutoMapper;

namespace Accounts.Domain.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly IAccountRepository _accountRepository;

        public UserService(IUserRepository userRepository,
                           IAccountRepository accountRepository,
                           IMapper mapper)
        {
            _userRepository = userRepository;
            _accountRepository = accountRepository;
            _mapper = mapper;
        }

        public async Task<UserResponseDto> CreateAsync(RegisterDto registerDto, Guid accountId)
        {
            var user = _mapper.Map<UserRequestDto>(registerDto);
            user.AccountId = accountId;

            return await _userRepository.InsertAsync<UserRequestDto, UserResponseDto>(user);
        }

        public async Task<UserResponseDto> UpdateAsync(Guid id, UserWithoutAccountIdDto user)
        {
            return await _userRepository.UpdateAsync<UserWithoutAccountIdDto, UserResponseDto>(id, user);
        }

        public async Task<UserResponseDto> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id));
            }

            return await _userRepository.GetByIdAsync<UserResponseDto>(id);
        }

        public async Task<PaginatedResult<UserResponseDto>> GetPageAsync(PagingInfo pagingInfo)
        {
            return await _userRepository.GetPageAsync<UserResponseDto>(pagingInfo.PageNumber, pagingInfo.PageSize);
        }

        public async Task DeleteAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id));
            }

            var user = await _userRepository.GetByIdAsync<UserResponseDto>(id);

            await _userRepository.DeleteAsync(id);

            await _accountRepository.DeleteAsync(user.AccountId);
        }
    }
}
