using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eRekreacija.Services.Services
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<Database.User, Models.Models.RegisterRequest>();
            CreateMap<Models.Models.RegisterRequest, Database.User>();
        }
    }
}
