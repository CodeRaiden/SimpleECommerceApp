//using MailKit.Security;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Extensions.Logging;
//using MimeKit;
//using MimeKit.Text;
//using SharedLibPackage.Common;
//using SharedLibPackage.Common.Response;
//using SharedLibPackage.Helper;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Linq.Expressions;
//using System.Text;
//using System.Threading.Tasks;
//using static SimpleECommerce.Infrastructure.Services.Auth;

//namespace SimpleECommerce.Infrastructure.Services
//{
//    public class Auth
//    {
//        public class UserRepository : IUserRepository
//        {
//            private readonly AuctionIT_DbContext _context;
//            private readonly IConfiguration _config;
//            private readonly ILogger<UserRepository> _logger;

//            public UserRepository(AuctionIT_DbContext context, IConfiguration config, ILogger<UserRepository> logger)
//            {
//                _context = context;
//                _config = config;
//                _logger = logger;
//            }

//            // Private method to get user by email
//            private async Task<User> GetUserByEmailAsync(string email)
//            {
//                // query database for admin with entered user id
//                var userEntity = await _context.Users.FirstOrDefaultAsync(a => a.Email == email);

//                // return admin if found, else return null
//                return userEntity is null ? null! : userEntity;
//            }

//            public async Task<GenericResponse> RegisterUserAsync(User entry, CancellationToken cancellationToken = default)
//            {
//                var response = new GenericResponse();

//                try
//                {

//                    var getUser = await GetUserByEmailAsync(entry.Email);

//                    if (getUser is not null)
//                    {
//                        response = response.Failure("Email already exists");
//                        return response;
//                    }

//                    var userEntity = new User()
//                    {
//                        FirstName = entry.FirstName,
//                        LastName = entry.LastName,
//                        DisplayName = entry.DisplayName,
//                        Email = entry.Email,
//                        Password = BCrypt.Net.BCrypt.HashPassword(entry.Password),
//                        PhoneNumbers = entry.PhoneNumbers,
//                        Addresses = entry.Addresses,
//                        DateOfBirth = entry.DateOfBirth,
//                        AboutMe = entry.AboutMe,
//                        Role = "User",
//                        Gender = entry.Gender,
//                        Country = entry.Country,
//                        State = entry.State,
//                        LGA = entry.LGA,
//                        ProfileImageUrl = entry.ProfileImageUrl,
//                        IsApproved = false,
//                        OnboardingStatus = Domain.Enum.OnboardingStatus.Pending,
//                    };

//                    var addedUser = await _context.Users.AddAsync(userEntity);


//                    // check if the user was successfully registered
//                    if (addedUser.State != EntityState.Added)
//                    {
//                        response = response.Failure("An error occured while registering user... Invalid data provided.");
//                        return response;
//                    }

//                    await _context.SaveChangesAsync();

//                    response = response.Successful("User registeration successful");
//                    return response;
//                }
//                catch (Exception ex)
//                {
//                    // log the original exception
//                    _logger.LogInformation(ex.Message);

//                    // Display scary-free message
//                    response = response.Failure("An error occured while registering user... Please contact site administrator if error persists");
//                    return response;
//                }


//            }

//            public async Task<GenericResponse> LoginUserAsync(User entry, CancellationToken cancellationToken = default)
//            {
//                var response = new GenericResponse();

//                try
//                {

//                    // query database to check if admin exists
//                    var getUser = await GetUserByEmailAsync(entry.Email);

//                    if (getUser is null)
//                    {
//                        response = response.Failure("Invalid user credentials");
//                        return response;
//                    }

//                    // verify admin password using BCrypt
//                    // this returns a boolean
//                    var verifyPassword = BCrypt.Net.BCrypt.Verify(entry.Password, getUser.Password);

//                    // if verification fails return failure response
//                    if (!verifyPassword)
//                    {
//                        response = response.Failure("Invalid user password");
//                        return response;
//                    }

//                    //// if password verifed is successful, then generate Admin access token
//                    //var token = GenerateToken(getUser);
//                    var token = TokenGenerator<User>.GenerateToken(getUser);

//                    // save admin VerificationToken
//                    getUser.Otp = Utilities.CreateOtp();
//                    getUser.OtpId = Utilities.CreateOtpId();
//                    getUser.OtpLifeSpan = DateTime.UtcNow.AddMinutes(10);
//                    getUser.LastSignInDate = DateTime.UtcNow;
//                    //getAdmin.IsActive = true;

//                    // update admin
//                    var updateUser = await UpdateAsync(getUser);

//                    if (!updateUser.Success)
//                    {
//                        response = updateUser;
//                        return response;
//                    }

//                    //response = response.Successful($"Welcome {getAdmin.FullName}! AccessToken: {token}");
//                    response = response.Successful(token);
//                    return response;
//                }
//                catch (Exception ex)
//                {
//                    // log the original exception
//                    _logger.LogInformation(ex.Message);

//                    // Display scary-free message
//                    response = response.Failure("An error occured while logging in user... Please contact site administrator if error persists");
//                    return response;
//                }

//            }

//            public Task<GenericResponse> CreateAsync(User entity, CancellationToken cancellationToken = default)
//            {
//                throw new NotImplementedException();
//            }

//            public async Task<GenericResponse> DeleteAsync(Guid id, CancellationToken cancellationToken = default)
//            {
//                var response = new GenericResponse();

//                try
//                {
//                    var user = await _context.Users.FindAsync(id);
//                    if (user is null)
//                        return response.Failure("User not found");

//                    _context.Users.Remove(user);
//                    await _context.SaveChangesAsync();
//                    response = response.Successful("User deleted successfully");
//                    return response;
//                }
//                catch (Exception ex)
//                {
//                    // log the original exception
//                    _logger.LogInformation(ex.Message);

//                    // Display scary-free message
//                    response = response.Failure("An error occured while deleting user record... Please contact site administrator if error persists");
//                    return response;
//                }
//            }

//            public async Task<GenericResponse<User>> FindByIdAsync(Guid id, CancellationToken cancellationToken = default)
//            {
//                // initialze response of type GetAdminDto
//                var response = new GenericResponse<User>();

//                try
//                {
//                    // query database for Admin with entered id
//                    var userEntity = await _context.Users.FindAsync(id);

//                    // if null return failure response
//                    if (userEntity is null)
//                    {
//                        response = response.Failure("Invalid user Id");
//                        return response;
//                    }

//                    // else return success
//                    response = response.Successful("User retrieved successfully");
//                    response.Data = userEntity;

//                    return response;

//                }
//                catch (Exception ex)
//                {
//                    // log the original exception
//                    _logger.LogInformation(ex.Message);

//                    // Display scary-free message
//                    response = response.Failure("An error occured while retrieving user record... Please contact site administrator if error persists");
//                    return response;
//                }


//            }

//            public async Task<GenericResponse<IEnumerable<User>>> GetAllAsync(CancellationToken cancellationToken = default)
//            {
//                var response = new GenericResponse<IEnumerable<User>>();

//                try
//                {
//                    var getAllUsers = await _context.Users.AsNoTracking().ToListAsync();

//                    if (getAllUsers is not null && getAllUsers.Any())
//                    {
//                        response = response.Successful("Users retrieved successfully");
//                        response.Data = getAllUsers.AsEnumerable();
//                        return response;
//                    }
//                    else
//                    {
//                        response = response.Successful("Users database is empty");
//                        return response;
//                    }
//                }
//                catch (Exception ex)
//                {
//                    // log the original exception
//                    _logger.LogInformation(ex.Message);

//                    // Display scary-free message
//                    response = response.Failure("An error occured while retrieving users... Please contact site administrator if error persists");
//                    return response;
//                }
//            }

//            public async Task<GenericResponse<User>> GetByAsync(Expression<Func<User, bool>> expression, CancellationToken cancellationToken = default)
//            {
//                var response = new GenericResponse<User>();
//                try
//                {
//                    var userEntity = await _context.Users.Where(expression).FirstOrDefaultAsync();
//                    if (userEntity is not null)
//                    {
//                        response = response.Successful("User retrieved successfully");
//                        response.Data = userEntity;
//                        return response;
//                    }
//                    response = response.Failure("User not found");
//                    return response;
//                }
//                catch (Exception ex)
//                {
//                    // log the original exception
//                    _logger.LogInformation(ex.Message);

//                    // Display scary-free message
//                    response = response.Failure("An error occured while retrieving user... Please contact site administrator if error persists");
//                    return response;
//                }
//            }

//            public async Task<GenericResponse> UpdateAsync(User entity, CancellationToken cancellationToken = default)
//            {
//                var response = new GenericResponse();

//                try
//                {
//                    var getUser = await _context.Users.FindAsync(entity.Id);
//                    if (getUser is null)
//                    {
//                        response = response.Successful("User not found");
//                        return response;
//                    }

//                    var entityToUpdate = new User
//                    {
//                        Id = entity.Id,
//                        FirstName = entity.FirstName is null ? getUser.FirstName : entity.FirstName,
//                        LastName = entity.LastName is null ? getUser.LastName : entity.LastName,
//                        DisplayName = entity.DisplayName is null ? getUser.DisplayName : entity.DisplayName,
//                        Email = entity.Email is null ? getUser.Email : entity.Email,
//                        Password = entity.Password is null ? getUser.Password : entity.Password,
//                        PhoneNumbers = entity.PhoneNumbers is null ? getUser.PhoneNumbers : entity.PhoneNumbers,
//                        ProfileImageUrl = entity.ProfileImageUrl is null ? getUser.ProfileImageUrl : entity.ProfileImageUrl,
//                        Role = "User",
//                        IsApproved = entity.IsApproved is null ? getUser.IsApproved : entity.IsApproved,
//                        DateRegistered = entity.DateRegistered is null ? getUser.DateRegistered : entity.DateRegistered,
//                        OtpId = entity.OtpId is null ? getUser.OtpId : entity.OtpId,
//                        Otp = entity.Otp is null ? getUser.Otp : entity.Otp,
//                        OtpLifeSpan = entity.OtpLifeSpan is null ? getUser.OtpLifeSpan : entity.OtpLifeSpan,
//                        OnboardingStatus = entity.OnboardingStatus is null ? getUser.OnboardingStatus : entity.OnboardingStatus,
//                        ModifiedDate = entity.ModifiedDate is null ? getUser.ModifiedDate : entity.ModifiedDate,
//                        IsActive = entity.IsActive is null ? getUser.IsActive : entity.IsActive,
//                        VerifiedAt = entity.VerifiedAt is null ? getUser.VerifiedAt : entity.VerifiedAt,
//                        PasswordResetCode = entity.PasswordResetCode is null ? getUser.PasswordResetCode : entity.PasswordResetCode,
//                        PasswordResetCodeLifeSpan = entity.PasswordResetCodeLifeSpan is null ? getUser.PasswordResetCodeLifeSpan : entity.PasswordResetCodeLifeSpan,
//                        LastSignInDate = entity.LastSignInDate is null ? getUser.LastSignInDate : entity.LastSignInDate
//                    };

//                    _context.Entry(getUser).State = EntityState.Detached;
//                    _context.Users.Update(entity);
//                    await _context.SaveChangesAsync();
//                    response = response.Successful("User updated successfully");
//                    return response;
//                }
//                catch (Exception ex)
//                {
//                    // log the original exception
//                    _logger.LogInformation(ex.Message);

//                    // Display scary-free message
//                    response = response.Failure("An error occured while updating user... Please contact site administrator if error persists");
//                    return response;
//                }
//            }












//        }
//    }
//}
