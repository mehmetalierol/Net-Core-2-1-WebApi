using AutoMapper;
using Company.Application.Data.Entities;

namespace Company.Application.Dto.Mapper
{
    /// <summary>
    /// Bu sınıf bizim için Entity ve dto sınıfları arasında mapping tablosu görevi görmektedir. 
    /// Auto mapper buradaki kuralları kontrol ederek veri transferi işlemlerini otomatik yapacak.
    /// </summary>
    public class MappingProfile : Profile
    {
        /// <summary>
        /// Bu yapıcı method içerisinde kurallarımızı tanımlıyoruz.
        /// </summary>
        public MappingProfile()
        {
            //CreateMap metodu ile entity ve dtoları eşleştirip mapliyoruz
            //ReversMap komutu ise bu mappingin iki yönlü olduğunu bildiriyor.
            CreateMap<Organization, OrganizationDto>().ReverseMap();
            CreateMap<Customer, CustomerDto>().ReverseMap();
            CreateMap<Language, LanguageDto>().ReverseMap();
            CreateMap<AppResource, AppResourceDto>().ReverseMap();
            CreateMap<ApplicationRole, ApplicationRoleDto>().ReverseMap();
            CreateMap<ApplicationUser, ApplicationUserDto>().ReverseMap()
                .ForMember(a => a.PasswordHash, b => b.MapFrom(c => c.Password));

            //Hedef ve kaynak arasında property isimleri farklı ise aşağıdaki gibi eşleştirme yapılır
            //.ForMember(a => a.Name, b => b.MapFrom(c => c.ModelName))

            //Bir propertynin görmezden gelinmesi isteniyor ise aşağıdaki gibi bir tanımlama yapılmalıdır.
            //.ForSourceMember(x => x.Name, opt => opt.Ignore());
        }
    }
}
