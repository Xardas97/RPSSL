using AutoMapper;

using Mmicovic.RPSSL.API.Models;
using Mmicovic.RPSSL.Service.Models;

namespace Mmicovic.RPSSL.API.InitializationUtility
{
    public class AutoMapperSetup
    {
        private class MappingProfile : Profile
        {
            public MappingProfile()
            {
                CreateMap<Shape, ShapeDTO>().ReverseMap();
                CreateMap<GameRecord, GameRecordDTO>().ReverseMap();
            }
        }

        public static IMapper CreateMapper()
        {
            var config = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new MappingProfile());
            });
            return config.CreateMapper();
        }
    }
}
