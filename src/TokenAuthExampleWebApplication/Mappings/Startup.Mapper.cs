using TokenAuthExampleWebApplication.Authentication;
using TokenAuthExampleWebApplication.AuthModels;

namespace TokenAuthExampleWebApplication
{
    public partial class Startup
    {
        public static void InitMapper()
        {
            AutoMapper.Mapper.CreateMap<BasicUser, User>();
            AutoMapper.Mapper.CreateMap<User, BasicUser>();

            AutoMapper.Mapper.CreateMap<CustomUser, User>();
            AutoMapper.Mapper.CreateMap<User, CustomUser>();
        }
    }
}
