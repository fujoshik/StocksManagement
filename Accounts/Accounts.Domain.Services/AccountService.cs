using Accounts.Domain.Abstraction.Repositories;
using Accounts.Domain.Abstraction.Services;
using Accounts.Domain.DTOs.Account;
using Accounts.Domain.DTOs.Authentication;
using Accounts.Domain.DTOs.Wallet;
using AutoMapper;

namespace Accounts.Domain.Services
{
    public class AccountService : IAccountService
    {
        private readonly IMapper _mapper;
        private readonly IPasswordService _passwordService;
        private readonly IWalletService _walletService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAssignRoleService _assignRoleService;

        public AccountService(IUnitOfWork unitOfWork,
                              IPasswordService passwordService,
                              IWalletService walletService,
                              IMapper mapper,
                              IAssignRoleService assignRoleService)
        {
            _unitOfWork = unitOfWork;
            _passwordService = passwordService;
            _walletService = walletService;
            _mapper = mapper;
            _assignRoleService = assignRoleService;
        }

        public async Task<AccountResponseDto> CreateAsync(RegisterDto registerDto)
        {
            if (registerDto == null)
            {
                throw new ArgumentNullException(nameof(registerDto));
            }

            var account = _mapper.Map<AccountRequestDto>(registerDto);
            if (registerDto is RegisterTrialDto)
            {
                account.DateToDelete = DateTime.UtcNow.AddMonths(2).ToString("yyyy-MM-dd HH:mm:ss.fff");
            }

            var walletRequest = _mapper.Map<WalletRequestDto>(registerDto);
            var wallet = await _walletService.CreateAsync(walletRequest);

            GeneratePassword(account, registerDto);
            account.WalletId = wallet.Id;

            var role = _assignRoleService.AssignRole(_mapper.Map<DepositDto>(wallet));
            account.Role = role;

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

        private void GeneratePassword(AccountRequestDto account, RegisterDto registerDto)
        {
            var salt = _passwordService.GenerateSalt();
            account.PasswordHash = _passwordService.HashPasword(registerDto.Password, salt);
            account.PasswordSalt = Convert.ToBase64String(salt);
        }
    }
}
