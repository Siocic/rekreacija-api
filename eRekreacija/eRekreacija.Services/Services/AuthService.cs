using AutoMapper;
using eRekreacija.Models.Models;
using eRekreacija.Services.Database;
using eRekreacija.Services.Database.enums;
using eRekreacija.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RabbitMQ.Client;
using System.Text;
using System.Threading.Channels;


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

        public AuthService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager,IMapper mapper,SignInManager<ApplicationUser>signInManager)
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
        public async Task<IdentityResult> RegisterUser(RegisterRequest request)
        {
            //var user = _mapper.Map<User>(request);
            var user = new ApplicationUser
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                UserName = request.Email,
                Address = request.Address,
            };
            var result = await _userManager.CreateAsync(user, request.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, Roles.FizickoLice.ToString());
            }
            var userEmail=$"Registration created for {request.Email}";
            var body=Encoding.UTF8.GetBytes(userEmail);
            _channel.BasicPublish(exchange: "", routingKey: "registrationQueue", basicProperties: null,body:body);
            return result;
        }
        public async Task<SignInResult> LoginAsync(string email, string password)
        {
            var user=await _userManager.FindByEmailAsync(email);
            if (user == null) 
                return SignInResult.Failed;

            return await _signInManager.PasswordSignInAsync(user, password, false, false);
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
