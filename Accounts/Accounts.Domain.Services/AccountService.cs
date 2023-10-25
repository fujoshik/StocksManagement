using Accounts.Domain.Abstraction.Repositories;
using Accounts.Domain.Abstraction.Services;
using Accounts.Domain.DTOs.Account;
using Accounts.Domain.DTOs.Wallet;
using AutoMapper;

namespace Accounts.Domain.Services
{
    public class AccountService : IAccountService
    {
        private readonly IMapper _mapper;
        private readonly IPasswordService _passwordService;
        private readonly IWalletService _walletService;
        private readonly IAccountRepository _accountRepository;

        public AccountService(IAccountRepository accountRepository,
                              IPasswordService passwordService,
                              IWalletService walletService,
                              IMapper mapper)
        {
            _accountRepository = accountRepository;
            _passwordService = passwordService;
            _walletService = walletService;
            _mapper = mapper;
        }

        public async Task<AccountResponseDto> CreateAsync(RegisterDto registerDto)
        {
            var salt = _passwordService.GenerateSalt();

            var account = _mapper.Map<AccountRequestDto>(registerDto);
            var wallet = await _walletService.CreateAsync(new WalletRequestDto());

            account.PasswordHash = _passwordService.HashPasword(registerDto.Password, salt);
            account.PasswordSalt = Convert.ToBase64String(salt);
            account.WalletId = wallet.Id;

            return await _accountRepository.InsertAsync<AccountRequestDto, AccountResponseDto>(account);
        }

        public async Task<AccountResponseDto> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id));
            }

            return await _accountRepository.GetByIdAsync<AccountResponseDto>(id);
        }

        public async Task DeleteAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id));
            }

            await _accountRepository.DeleteAsync(id);
        }
    }
}
