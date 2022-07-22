using FirebaseAdmin;
using FirebaseAdmin.Auth;
using Google.Apis.Auth.OAuth2;
using May25.API.Core.Constants;
using May25.API.Core.Contracts.Services;
using May25.API.Core.Contracts.UnitsOfWork;
using May25.API.Core.Exceptions;
using May25.API.Core.Extensions;
using May25.API.Core.Models.Entities;
using May25.API.Core.Models.Resources;
using May25.API.Core.Options;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace May25.API.Core.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHashService _hashService;
        private readonly JwtOptions _jwtOptions;
        private readonly APIKeysOptions _apiKeysOptions;
        private readonly GoogleFirebaseOptions _firebaseOptions;
        private FirebaseApp _firebaseApp;
        private FirebaseAuth _firebaseAuth;

        public AuthService(IUnitOfWork unitOfWork, IHashService hashService,
             IOptions<JwtOptions> jwtOptions, IOptions<APIKeysOptions> apiKeysOptions,
             IOptions<GoogleFirebaseOptions> firebaseOptions)
        {
            _unitOfWork = unitOfWork.ThrowIfNull(nameof(unitOfWork));
            _hashService = hashService.ThrowIfNull(nameof(hashService));
            _jwtOptions = jwtOptions.Value.ThrowIfNull(nameof(jwtOptions));
            _apiKeysOptions = apiKeysOptions.Value.ThrowIfNull(nameof(apiKeysOptions));
            _firebaseOptions = firebaseOptions.Value.ThrowIfNull(nameof(firebaseOptions));

            InitializeFirebaseApp();
        }

        public async Task<UserAuthenticationResultDTO> AuthenticateAsync(UserForAuthenticationDTO userForAuth)
        {
            var user = await _unitOfWork.Users.GetByEmailAsync(userForAuth.Email.ToLower());

            if (!IsValidUser(user, userForAuth.Password))
            {
                throw new BadRequestException("Username or password is incorrect");
            }

            return new UserAuthenticationResultDTO
            {
                Id = user.Id,
                Email = user.Email,
                AccessToken = GenerateJwtToken(user)
            };
        }

        public async Task<UserAuthenticationResultDTO> AuthenticateGoogleAsync(string token)
        {
            var decodedToken = await FirebaseAuth.DefaultInstance.VerifyIdTokenAsync(token);
            var email = decodedToken.Claims["email"].ToString();
            var user = await _unitOfWork.Users.GetByEmailAsync(email);

            if (user == null)
            {
                var roles = await _unitOfWork.Roles.GetAllAsync();

                user = new User
                {
                    Email = email.ToLower(),
                    Picture = decodedToken.Claims["picture"].ToString(),
                    FirstName = decodedToken.Claims["name"].ToString().SplitAndGetPosition(' ', 0),
                    LastName = decodedToken.Claims["name"].ToString().SplitAndGetPosition(' ', 1),
                    PasswordHash = null,
                    CreatedAt = DateTime.Now,
                    Roles = new List<UserRoles>() { new UserRoles { Role = roles.First(x => x.Name.Equals(AppRoles.User)) } },
                    IsEmailConfirmed = true
                };

                _unitOfWork.Users.Add(user);

                await _unitOfWork.CompleteAsync();
            }

            return new UserAuthenticationResultDTO
            {
                Id = user.Id,
                Email = user.Email,
                AccessToken = GenerateJwtToken(user)
            };
        }

        public Task<ClientAuthenticationResultDTO> AuthenticateAsync(ClientForAuthenticationDTO clientForAuth)
        {
            if (_apiKeysOptions.Client1 != clientForAuth.APIKey)
            {
                throw new BadRequestException("Invalid API Key");
            }

            return Task.FromResult(new ClientAuthenticationResultDTO
            {
                AccessToken = GenerateJwtToken(clientForAuth)
            });
        }

        #region Helpers
        private void InitializeFirebaseApp()
        {
            if (FirebaseApp.DefaultInstance != null)
            {
                return;
            }

            var jsonOptions = JsonSerializer.Serialize(_firebaseOptions);

            _firebaseApp = FirebaseApp.Create(new FirebaseAdmin.AppOptions()
            {
                Credential = GoogleCredential.FromJson(jsonOptions).CreateScoped("https://www.googleapis.com/auth/firebase")
            });

            _firebaseAuth = FirebaseAuth.GetAuth(_firebaseApp);
        }

        private bool IsValidUser(User user, string password)
        {
            return user != null && user.PasswordHash != null && _hashService.ValidatePassword(password, user.PasswordHash);
        }

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtOptions.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = GenerateClaims(user),
                Expires = DateTime.UtcNow.AddDays(_jwtOptions.ExpiresInDays),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Audience = _jwtOptions.Audience,
                Issuer = _jwtOptions.Issuer
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        private string GenerateJwtToken(ClientForAuthenticationDTO clientForAuth)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtOptions.Secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = GenerateClaims(clientForAuth),
                Expires = DateTime.UtcNow.AddDays(_jwtOptions.ExpiresInDays),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Audience = _jwtOptions.Audience,
                Issuer = _jwtOptions.Issuer
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        private ClaimsIdentity GenerateClaims(User user)
        {
            var claims = new ClaimsIdentity(new Claim[]
            {
                new Claim(AppClaims.Id, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
            });

            foreach (var userRole in user.Roles)
            {
                claims.AddClaim(new Claim(ClaimTypes.Role, userRole.Role.Name));
            }

            return claims;
        }

        private ClaimsIdentity GenerateClaims(ClientForAuthenticationDTO clientForAuth)
        {
            return new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, "Client1"),
                new Claim(ClaimTypes.Role, AppRoles.ApiClient)
            });
        }
        #endregion
    }
}
