using Accounts.Domain.Abstraction.Repositories;
using Accounts.Domain.Abstraction.Services;
using Accounts.Domain.DTOs.Account;
using AutoMapper;

namespace Accounts.Domain.Services
{
    public class AccountService : IAccountService
    {
        private readonly IMapper _mapper;
        private readonly IPasswordService _passwordService;
        private readonly IAccountRepository _accountRepository;

        public AccountService(IAccountRepository accountRepository,
                              IPasswordService passwordService,
                              IMapper mapper)
        {
            _accountRepository = accountRepository;
            _passwordService = passwordService;
            _mapper = mapper;
        }

        public async Task<AccountResponseDto> CreateAsync(RegisterDto registerDto)
        {
            var salt = _passwordService.GenerateSalt();

            var account = _mapper.Map<AccountRequestDto>(registerDto);

            account.PasswordHash = _passwordService.HashPasword(registerDto.Password, salt);
            account.PasswordSalt = Convert.ToBase64String(salt);

            return await _accountRepository.InsertAsync<AccountRequestDto, AccountResponseDto>(account);
        }

        public async Task<AccountResponseDto> GetByIdAsync(Guid id)
        {
            return await _accountRepository.GetByIdAsync<AccountResponseDto>(id);
        }

        public async Task DeleteAsync(Guid id)
        {
            await _accountRepository.DeleteAsync(id);
        }
    }
}
