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
        private readonly IUnitOfWork _unitOfWork;

        public UserService(IUnitOfWork unitOfWork,
                           IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<UserResponseDto> CreateAsync(RegisterDto registerDto, Guid accountId)
        {
            var user = _mapper.Map<UserRequestDto>(registerDto);
            user.AccountId = accountId;

            return await _unitOfWork.UserRepository.InsertAsync<UserRequestDto, UserResponseDto>(user);
        }

        public async Task<UserResponseDto> UpdateAsync(Guid id, UserWithoutAccountIdDto user)
        {
            return await _unitOfWork.UserRepository.UpdateAsync<UserWithoutAccountIdDto, UserResponseDto>(id, user);
        }

        public async Task<UserResponseDto> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id));
            }

            return await _unitOfWork.UserRepository.GetByIdAsync<UserResponseDto>(id);
        }

        public async Task<PaginatedResult<UserResponseDto>> GetPageAsync(PagingInfo pagingInfo)
        {
            return await _unitOfWork.UserRepository.GetPageAsync<UserResponseDto>(pagingInfo.PageNumber, pagingInfo.PageSize);
        }

        public async Task DeleteAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id));
            }

            var user = await _unitOfWork.UserRepository.GetByIdAsync<UserResponseDto>(id);

            await _unitOfWork.UserRepository.DeleteAsync(id);

            await _unitOfWork.AccountRepository.DeleteAsync(user.AccountId);
        }
    }
}
