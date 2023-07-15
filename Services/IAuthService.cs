using tutorial_api_2.Dtos;

namespace tutorial_api_2.Services
{
    public interface IAuthService
    {
        UserLoginDto? Login(LoginDto dto);
        
        SignUpDto? SignUp(SignUpDto dto);
    }
}
