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
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IMapper _mapper;
        private readonly IModel _channel;
        private readonly string _host = "localhost";
        private readonly string _username = "guest";
        private readonly string _password = "guest";
        private readonly string _virtualhost = "/";

        public AuthService(UserManager<User> userManager, RoleManager<IdentityRole> roleManager,IMapper mapper,SignInManager<User>signInManager)
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
            var user = new User
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
        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _userManager.Users.ToListAsync();
        }
        public async Task<IEnumerable<IdentityRole>> GetAllRolesAsync()
        {
            return await _roleManager.Roles.ToListAsync();
        }
    }
}
