using AutoMapper;
using May25.API.Core.Constants;
using May25.API.Core.Contracts.Services;
using May25.API.Core.Contracts.UnitsOfWork;
using May25.API.Core.Exceptions;
using May25.API.Core.Extensions;
using May25.API.Core.Helpers;
using May25.API.Core.Models.Entities;
using May25.API.Core.Models.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace May25.API.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHashService _hashService;
        private readonly ClaimsPrincipal _authUser;
        private readonly IRatingService _ratingService;
        private readonly IEmailService _emailService;
        private readonly IHtmlService _htmlService;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper,
            IHashService hashService, ClaimsPrincipal authUser,
            IRatingService ratingService, IEmailService emailService,
            IHtmlService htmlService)
        {
            _unitOfWork = unitOfWork.ThrowIfNull(nameof(unitOfWork));
            _mapper = mapper.ThrowIfNull(nameof(mapper));
            _hashService = hashService.ThrowIfNull(nameof(hashService));
            _authUser = authUser.ThrowIfNull(nameof(authUser));
            _ratingService = ratingService.ThrowIfNull(nameof(ratingService));
            _emailService = emailService.ThrowIfNull(nameof(emailService));
            _htmlService = htmlService.ThrowIfNull(nameof(htmlService));
        }

        public async Task<CreatedUserDTO> CreateAsync(UserForCreationDTO userForCreation)
        {
            if (await UserExists(userForCreation.Email))
            {
                throw new BadRequestException("The email already exists");
            }

            var roles = await _unitOfWork.Roles.GetAllAsync();

            var user = new User
            {
                Email = userForCreation.Email.ToLower(),
                PasswordHash = _hashService.HashPassword(userForCreation.Password),
                CreatedAt = DateTime.Now,
                Roles = new List<UserRoles>() { new UserRoles { Role = roles.First(x => x.Name.Equals(AppRoles.User)) } },
                EmailConfirmationToken = CryptoHelper.GenerateRandomString(length: 40),
                IsEmailConfirmed = false
            };

            _unitOfWork.Users.Add(user);

            await _unitOfWork.CompleteAsync();

            await _emailService.SendConfirmationEmailAsync(new ConfirmEmailParamsDTO(user.Email, user.EmailConfirmationToken));

            return _mapper.Map<CreatedUserDTO>(user);
        }

        public async Task<bool> UserExists(string email)
        {
            return await _unitOfWork.Users.SingleOrDefaultAsync(x => x.Email.Equals(email.ToLower())) != null;
        }

        public async Task<UserDTO> GetByIdAsync(int id)
        {
            var user = await _unitOfWork.Users.GetAsync(id);

            return _mapper.Map<UserDTO>(user);
        }

        public async Task<IEnumerable<UserDTO>> GetAllAsync()
        {
            var users = await _unitOfWork.Users.GetAllAsync();

            return _mapper.Map<IEnumerable<UserDTO>>(users);
        }

        public async Task<UserDTO> UpdateAsync(int id, UserForUpdateDTO userForUpdate)
        {
            var user = await _unitOfWork.Users.GetAsync(id);

            if (user == null)
            {
                throw new NotFoundException($"User {id} not found");
            }

            if (_authUser.IsInRole(AppRoles.Admin) || _authUser.HasId(id))
            {
                _mapper.Map(userForUpdate, user);

                await _unitOfWork.CompleteAsync();

                return _mapper.Map<UserDTO>(user);
            }
            else
            {
                throw new ForbiddenException("Cannot update another user info");
            }
        }

        public async Task<UserPublicProfileDTO> GetUserPublicProfileAsync(int userId)
        {
            var user = await _unitOfWork.Users.GetAsync(userId);

            if (user == null)
            {
                throw new NotFoundException($"User {userId} not found");
            }

            var publicProfile = _mapper.Map<UserPublicProfileDTO>(user);
            publicProfile.Ratings = await _ratingService.GetUserRatingAsync(userId);

            return publicProfile;
        }

        public async Task<HtmlContentResultDTO> ConfirmEmailAsync(ConfirmEmailParamsDTO data)
        {
            var user = await _unitOfWork.Users.GetByEmailAsync(data.Email);

            if (user == null || !user.EmailConfirmationToken.Equals(data.Token))
            {
                return new HtmlContentResultDTO(false, _htmlService.GetEmailConfirmationError(data.Email));
            }
            else
            {
                user.IsEmailConfirmed = true;
                user.EmailConfirmationToken = null;
                await _unitOfWork.CompleteAsync();

                return new HtmlContentResultDTO(true, _htmlService.GetEmailConfirmationSuccess(data.Email));
            }
        }

        public async Task<HtmlContentResultDTO> UnsubscribeAsync(string email)
        {
            var user = await _unitOfWork.Users.GetByEmailAsync(email);

            if (user != null)
            {
                await _unitOfWork.CompleteAsync();
            }

            return new HtmlContentResultDTO(true, _htmlService.GetEmailUnsubscribed());
        }

        public async Task ChangePasswordAsync(ChangePasswordDTO changePassword)
        {
            var user = await _unitOfWork.Users.GetAsync(_authUser.GetId());
            var currentPasswordIsValid = _hashService.ValidatePassword(changePassword.CurrentPassword, user.PasswordHash);

            if (!currentPasswordIsValid || !changePassword.NewPassword.Equals(changePassword.RepeatNewPassword))
            {
                throw new BadRequestException("Invalid password");
            }

            var newPasswordHash = _hashService.HashPassword(changePassword.NewPassword);

            user.PasswordHash = newPasswordHash;
            await _unitOfWork.CompleteAsync();
        }

        public async Task RequestPasswordResetAsync(string email)
        {
            var user = await _unitOfWork.Users.GetByEmailAsync(email);

            if (user != null)
            {
                var passwordReset = new PasswordReset
                {
                    Email = email,
                    Token = CryptoHelper.GenerateRandomString(length: 40),
                    ValidUntil = DateTime.Now.AddDays(3)
                };

                _unitOfWork.PasswordResets.Add(passwordReset);
                await _unitOfWork.CompleteAsync();

                await _emailService.SendPasswordResetRequestAsync(passwordReset.Email, passwordReset.Token);
            }
        }

        public HtmlContentResultDTO GetPasswordResetForm(string email, string token)
        {
            return new HtmlContentResultDTO(true, _htmlService.GetPasswordResetForm(email, token));
        }

        public async Task ResetPasswordAsync(string email, string token, ResetPasswordDTO data)
        {
            var user = await _unitOfWork.Users.GetByEmailAsync(email);

            if (user == null)
            {
                throw new BadRequestException($"Invalid attempt to reset password");
            }

            var passwordReset = await _unitOfWork.PasswordResets.GetAsync(email, token);

            if (passwordReset == null || !data.Password.Equals(data.RepeatPassword))
            {
                throw new BadRequestException($"Invalid attempt to reset password");
            }

            user.PasswordHash = _hashService.HashPassword(data.Password);

            await _unitOfWork.CompleteAsync();
        }
    }
}
