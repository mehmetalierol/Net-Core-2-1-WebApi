using Company.Application.Dto;

namespace Company.Application.WebApi.VM
{
    public class TokenModel
    {
        public object Token { get; set; }
        public ApplicationUserDto UserDto { get; set; }
    }
}
