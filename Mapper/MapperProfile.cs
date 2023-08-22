using AOTableDTOModel;
using AOTableModel;
using AutoMapper;

namespace Mapper
{
    public class MapperProfile:Profile
    {
        public MapperProfile()
        {
            CreateMap<AOTable,AOTableDTO>().ReverseMap();
            
        }


    }
}