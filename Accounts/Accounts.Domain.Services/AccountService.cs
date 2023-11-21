using Accounts.Domain.Abstraction.Repositories;
using Accounts.Domain.Abstraction.Services;
using Accounts.Domain.DTOs.Account;
using Accounts.Domain.DTOs.Authentication;
using Accounts.Domain.DTOs.Wallet;
using Accounts.Domain.Enums;
using AutoMapper;

namespace Accounts.Domain.Services
{
    public class AccountService : IAccountService
    {
        private readonly IMapper _mapper;
        private readonly IPasswordService _passwordService;
        private readonly IWalletService _walletService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrencyConverterService _currencyConverterService;

        public AccountService(IUnitOfWork unitOfWork,
                              IPasswordService passwordService,
                              IWalletService walletService,
                              ICurrencyConverterService currencyConverterService,
                              IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _passwordService = passwordService;
            _walletService = walletService;
            _currencyConverterService = currencyConverterService;
            _mapper = mapper;
        }

        public async Task<AccountResponseDto> CreateAsync(RegisterDto registerDto)
        {
            var salt = _passwordService.GenerateSalt();

            var account = _mapper.Map<AccountRequestDto>(registerDto);
            var walletRequest = _mapper.Map<WalletRequestDto>(registerDto);
            var wallet = await _walletService.CreateAsync(walletRequest);

            account.PasswordHash = _passwordService.HashPasword(registerDto.Password, salt);
            account.PasswordSalt = Convert.ToBase64String(salt);
            account.WalletId = wallet.Id;

            AssignRole(wallet, account);

            return await _unitOfWork.AccountRepository.InsertAsync<AccountRequestDto, AccountResponseDto>(account);
        }

        public async Task<AccountResponseDto> GetByIdAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id));
            }

            return await _unitOfWork.AccountRepository.GetByIdAsync<AccountResponseDto>(id);
        }

        public async Task DeleteAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(id));
            }

            await _unitOfWork.AccountRepository.DeleteAsync(id);
        }

        private void AssignRole(WalletResponseDto wallet, AccountRequestDto account)
        {
            decimal sum = wallet.InitialBalance;

            if (wallet.CurrencyCode != CurrencyCode.USD)
            {
                sum = _currencyConverterService.Convert(wallet.CurrencyCode, CurrencyCode.USD, wallet.InitialBalance);
            }

            if (sum > 0)
            {
                if (sum < 1000)
                    account.Role = (int)Role.Regular;
                else if (sum >= 1000 && sum < 5000)
                    account.Role = (int)Role.Special;
                else if (sum >= 5000)
                    account.Role = (int)Role.VIP;
            }
            else
                account.Role = (int)Role.Inactive;
        }
    }
}
