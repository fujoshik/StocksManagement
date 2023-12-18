using Gateway.Domain.Abstraction.Clients.AccountClient;
using Gateway.Domain.DTOs.User;
using Gateway.Domain.Pagination;
using Gateway.Domain.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;

namespace Gateway.Domain.Clients.AccountClient
{
    public class UserAccountClient : BaseAccountClient, IUserAccountClient
    {
        public UserAccountClient(IOptions<HostSettings> hostSettings,
                                 IOptions<AccountSettings> accountSettings,
                                 IOptions<UserSettings> userSettings,
                                 IHttpContextAccessor httpContextAccessor) 
            : base(hostSettings, accountSettings, userSettings, httpContextAccessor)
        {
        }

        public async Task UpdateUserAsync(Guid id, UserWithoutAccountIdDto user)
        {
            AddAuthorizationHeader();

            var updateRoute = _accountApiUrl + _userSettings.GetRoute + id;

            var response = await _httpClient.PutAsJsonAsync(updateRoute, user);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException((int)response.StatusCode + " " + response.ReasonPhrase);
            }
        }

        public async Task<UserResponseDTO> GetUserAsync(Guid id)
        {
            AddAuthorizationHeader();

            var getRoute = _accountApiUrl + _userSettings.GetRoute + id;

            var response = await _httpClient.GetAsync(getRoute);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException((int)response.StatusCode + " " + response.ReasonPhrase);
            }

            return await response.Content.ReadFromJsonAsync<UserResponseDTO>();
        }

        public async Task<PaginatedResult<UserResponseDTO>> GetPageAsync(Paging pagingInfo)
        {
            AddAuthorizationHeader();

            var query = $"?PageNumber={pagingInfo.PageNumber}&PageSize={pagingInfo.PageSize}";

            var getRoute = _accountApiUrl + _userSettings.GetRoute + query;

            var response = await _httpClient.GetAsync(getRoute);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException((int)response.StatusCode + " " + response.ReasonPhrase);
            }

            return await response.Content.ReadFromJsonAsync<PaginatedResult<UserResponseDTO>>();
        }

        public async Task DeleteAsync(Guid id)
        {
            AddAuthorizationHeader();

            var route = _accountApiUrl + _userSettings.GetRoute + id;

            var response = await _httpClient.DeleteAsync(route);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException((int)response.StatusCode + " " + response.ReasonPhrase);
            }
        }
    }
}
