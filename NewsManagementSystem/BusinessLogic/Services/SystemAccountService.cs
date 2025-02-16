using AutoMapper;
using BusinessLogic.Interfaces;
using Data.Constants;
using Data.Entities;
using Data.Enum;
using Data.ExceptionCustom;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Repositories.DTOs.SystemAccountDTOs;
using Repositories.Interface;
using Repositories.PaggingItem;

using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;

namespace BusinessLogic.Services
{
    public class SystemAccountService : ISystemAccountService
    {
        private readonly IMapper _mapper;
        private readonly IUOW _unitOfWork;
        private readonly IConfiguration _configuration;

        private const int STAFF = 1;
        private const int LECTURER = 2;
        private const int STARTING_NUMBER = 0;

        public SystemAccountService(IMapper mapper, IUOW unitOfWork, IConfiguration configuration)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        // Get list of system user
        public async Task<PaginatedList<GetSystemAccountDTO>> GetUserAccounts(int index, int pageSize, int? idSearch, string? nameSearch, string? emailSearch, EnumRole? role)
        {
            if (index <= 0 || pageSize <= 0)
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ResponseCodeConstants.BADREQUEST, "Please input index or page size correctly!");
            }

            IQueryable<SystemAccount> query = _unitOfWork.GetRepository<SystemAccount>().Entities;

            // Search by user id
            if (idSearch.HasValue)
            {
                query = query.Where(u => u.AccountId == idSearch);
            }

            // Search by user name
            if (!string.IsNullOrWhiteSpace(nameSearch))
            {
                query = query.Where(u => u.AccountName!.Contains(nameSearch));
            }

            // Search by email
            if (!string.IsNullOrWhiteSpace(emailSearch))
            {
                // query = query.Where(u => u.AccountEmail!.Equals(emailSearch));
                emailSearch = emailSearch.Trim();
                query = query.Where(u => u.AccountEmail!.Trim().ToLower().Contains(emailSearch.ToLower()));
            }

            // Search by role
            switch (role)
            {
                case (EnumRole?)STAFF:
                    query = query.Where(u => u.AccountRole == STAFF);
                    break;
                case (EnumRole?)LECTURER:
                    query = query.Where(u => u.AccountRole == LECTURER);
                    break;
                default:
                    break;
            }

            // Sort by Id
            query = query.OrderBy(u => u.AccountId);

            PaginatedList<SystemAccount> resultQuery = await _unitOfWork.GetRepository<SystemAccount>().GetPagging(query, index, pageSize);

            // Map user entities to user dto
            IReadOnlyCollection<GetSystemAccountDTO> responseItems = resultQuery.Items.Select(item =>
            {
                GetSystemAccountDTO responseItem = _mapper.Map<GetSystemAccountDTO>(item);

                responseItem.RoleName = item.AccountRole == STAFF ? "Staff" : "Lecturer"; 

                return responseItem;
            }).ToList();

            PaginatedList<GetSystemAccountDTO> paginatedList = new(
                responseItems,
                resultQuery.TotalCount,
                resultQuery.PageNumber,
                resultQuery.PageSize
                );

            return paginatedList;
        }

        // Get 1 user account by id
        public async Task<GetSystemAccountDTO> GetUserAccountById(short id)
        {
            IQueryable<SystemAccount> query = _unitOfWork.GetRepository<SystemAccount>().Entities;

            SystemAccount? user = await query.Where(u => u.AccountId == id).FirstOrDefaultAsync();

            // Validate user
            if (user == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.BADREQUEST, "User not found!");
            }

            // Mapping user entities to dto
            GetSystemAccountDTO responseItem = _mapper.Map<GetSystemAccountDTO>(user);

            responseItem.RoleName = user.AccountRole == STAFF ? "Staff" : "Lecturer";

            return responseItem;
        }

        // Update User Info
        public async Task UpdateUserAccountById(PutSystemAccountDTO updatedUserAccount)
        {
            IQueryable<SystemAccount> query = _unitOfWork.GetRepository<SystemAccount>().Entities;

            SystemAccount? user = await query.Where(u => u.AccountId == updatedUserAccount.AccountId).FirstOrDefaultAsync();

            // Validate user
            if (user == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.BADREQUEST, "User not found!");
            }

            // Validate Account Email
            if (string.IsNullOrWhiteSpace(updatedUserAccount.AccountEmail) || (!updatedUserAccount.AccountEmail.Contains("@") || updatedUserAccount.AccountEmail.Count(c => c == '@') > 1))
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ResponseCodeConstants.BADREQUEST, "Invalid Account Email");
            }

            _mapper.Map(updatedUserAccount, user);
            //user.AccountName = updatedUserAccount.AccountName;
            //user.AccountEmail = updatedUserAccount.AccountEmail;

            await _unitOfWork.GetRepository<SystemAccount>().UpdateAsync(user);
            await _unitOfWork.SaveAsync();
        }

        // Delete User
        public async Task DeleteUserAccountById(short id)
        {
            IQueryable<SystemAccount> query = _unitOfWork.GetRepository<SystemAccount>().Entities;

            SystemAccount? user = await query.Where(u => u.AccountId == id).FirstOrDefaultAsync();

            // Validate user
            if (user == null)
            {
                throw new ErrorException(StatusCodes.Status404NotFound, ResponseCodeConstants.NOT_FOUND, "User not found!");
            }

            await _unitOfWork.GetRepository<SystemAccount>().DeleteAsync(user);
            await _unitOfWork.SaveAsync();
        }

        public async Task CreateUserAccount(PostSystemAccountDTO postSystemAccount)
        {
            // Validate Account Name
            if (string.IsNullOrWhiteSpace(postSystemAccount.AccountName))
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ResponseCodeConstants.BADREQUEST, "Invalid Account Name");
            }

            // Validate Account Email
            if (string.IsNullOrWhiteSpace(postSystemAccount.AccountEmail) || (!postSystemAccount.AccountEmail.Contains("@") || postSystemAccount.AccountEmail.Count(c => c == '@') > 1))
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ResponseCodeConstants.BADREQUEST, "Invalid Account Email");
            }

            // Validate Account Name
            if (string.IsNullOrWhiteSpace(postSystemAccount.AccountPassword))
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ResponseCodeConstants.BADREQUEST, "Invalid Password");
            }

            // Validate Account Name
            if (!postSystemAccount.AccountPassword.Equals(postSystemAccount.ConfirmedPassword))
            {
                throw new ErrorException(StatusCodes.Status400BadRequest, ResponseCodeConstants.BADREQUEST, "Password not matched");
            }

            // Mapping user dto to entities
            SystemAccount newUser = _mapper.Map<SystemAccount>(postSystemAccount);

            // Get Id with biggest number
            int maxId = await _unitOfWork.GetRepository<SystemAccount>()
                                        .Entities
                                        .MaxAsync(u => (short?)u.AccountId) ?? STARTING_NUMBER;

            // Add new account id
            newUser.AccountId = (short)(maxId + 1);

            // Add role to account
            switch (postSystemAccount.Role)
            {
                case EnumRole.Staff:
                    newUser.AccountRole = STAFF;
                    break;
                case EnumRole.Lecturer:
                    newUser.AccountRole = LECTURER;
                    break;
                default:
                    throw new ErrorException(StatusCodes.Status400BadRequest, ResponseCodeConstants.BADREQUEST, "Invalid Role");
            }

            await _unitOfWork.GetRepository<SystemAccount>().InsertAsync(newUser);
            await _unitOfWork.SaveAsync();
        }

        public async Task<string> Login(LoginDTO loginDTO)
        {
            var user = await _unitOfWork.GetRepository<SystemAccount>()
                .Entities
                .FirstOrDefaultAsync(u => u.AccountEmail == loginDTO.AccountEmail);

            if (user == null || loginDTO.AccountPassword != user.AccountPassword)
            {
                throw new ErrorException(StatusCodes.Status401Unauthorized, "401", "Invalid credentials");
            }

            var token = GenerateJwtToken(user);
            return token;
        }

        private string GenerateJwtToken(SystemAccount user)
        {
            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, user.AccountId.ToString()),
        new Claim(ClaimTypes.Name, user.AccountName),
        new Claim(ClaimTypes.Email, user.AccountEmail),
        new Claim(ClaimTypes.Role, user.AccountRole.ToString())
    };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }


    }
}
