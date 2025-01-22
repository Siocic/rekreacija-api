using AutoMapper;
using eRekreacija.Models.Models;
using eRekreacija.Services.Database;
using eRekreacija.Services.Database.enums;
using eRekreacija.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using Newtonsoft.Json;
using System.Text;
using Azure.Messaging;
using System.Security.Claims;
using eRekreacija.Models.DTOs;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using eRekreacija.Services.Database.Context;


namespace eRekreacija.Services.Services
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly IModel _channel;
        private readonly string _host = Environment.GetEnvironmentVariable("RabbitMQ_Host") ?? "localhost";
        private readonly string _username = Environment.GetEnvironmentVariable("RabbitMQ_Username") ?? "guest";
        private readonly string _password = Environment.GetEnvironmentVariable("RabbitMQ_Password") ?? "guest";
        private readonly string _virtualhost = Environment.GetEnvironmentVariable("RabbitMQ_Virtualhost") ?? "/";
        protected readonly IdentityContext _identityContext;

        public AuthService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper, SignInManager<ApplicationUser> signInManager)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _mapper = mapper;
            _signInManager = signInManager;

            var factory = new ConnectionFactory
            {
                HostName = _host,
                UserName = _username,
                Password = _password
            };
            var connection = factory.CreateConnection();
            _channel = connection.CreateModel();
            _channel.QueueDeclare(queue: "registrationQueue",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

        }
        public async Task<IdentityResult> RegisterUser(RegisterRequest request, int flag)
        {

            var checkIfUserExist = await _userManager.FindByEmailAsync(request.Email);
            if (checkIfUserExist != null)
            {
                return IdentityResult.Failed(new IdentityError
                {
                    Code = "UserAlready",
                    Description = "This user already exists. Please try with a different email."
                });
            }
            var user = new ApplicationUser
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                UserName = request.Email,
                Address = request.Address,
                City = request.City,
            };

            if (flag != 0)
            {
                user.isApproved = false;
            }

            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
            {
                return IdentityResult.Failed(result.Errors.ToArray());
            }

            string role = flag == 0 ? Roles.FizickoLice.ToString() : Roles.PravnoLice.ToString();
            await _userManager.AddToRoleAsync(user, role);

            string userEmailMessages;
            if (flag == 0)
            {
                userEmailMessages = "Welcome to Rekreacija! We're excited to have you on board and hope you love using our app!";
            }
            else
            {
                userEmailMessages = "Welcome to AppName! Your registration is successful. Please wait for admin approval. We’ll notify you once it’s done!";
            }

            var message = new
            {
                Email = user.Email,
                Message = userEmailMessages
            };

            var messageJson = JsonConvert.SerializeObject(message);

            var body = Encoding.UTF8.GetBytes(messageJson);

            _channel.BasicPublish(exchange: "", routingKey: "registrationQueue", basicProperties: null, body: body);
            return result;
        }
        public async Task<string> LoginAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
                return "User not found";

            var singInResult = await _signInManager.PasswordSignInAsync(user, password, false, false);
            if (!singInResult.Succeeded)
                return null;

            var roles = await _userManager.GetRolesAsync(user);
            var role = roles.FirstOrDefault() ?? " ";

            var token = GenerateJWTToken.JWTTokenGenerate(user, role);
            return token;
        }
        public async Task<ApplicationUserDTO> GetUser(string userId)
        {
            var user = _userManager.FindByIdAsync(userId);
            var profile = new ApplicationUserDTO
            {
                FirstName = user.Result.FirstName,
                LastName = user.Result.LastName,
                Email = user.Result.Email,
                Address = user.Result.Address,
                City = user.Result.City,
                PhoneNumber = user.Result.PhoneNumber,
                ProfilePicture = user.Result.ProfilePicutre != null
                                    ? Convert.ToBase64String(user.Result.ProfilePicutre)
                                    : null,
            };

            return profile;
        }
        public async Task<bool> EditProfile(ApplicationUserDTO model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email); // Retrieve existing user
            if (user == null)
                return false;

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.Email = model.Email;
            user.Address = model.Address;
            user.City = model.City;
            user.PhoneNumber = model.PhoneNumber;
            if (!string.IsNullOrEmpty(model.ProfilePicture))
            {
                user.ProfilePicutre = Convert.FromBase64String(model.ProfilePicture);
            }
            var result = await _userManager.UpdateAsync(user); 
            return result.Succeeded;
        }
        public async Task<IEnumerable<ApplicationUser>> GetAllUsersAsync()
        {
            return await _userManager.Users.ToListAsync();
        }
        public async Task<IEnumerable<IdentityRole>> GetAllRolesAsync()
        {
            return await _roleManager.Roles.ToListAsync();
        }
    }
}
