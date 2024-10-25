namespace PetCare.Shared.DTOs.Auth;

public class GetUsersResponse
{
    public List<UserDto>? Users { get; set; }

    public GetUsersResponse(List<UserDto> userDTOs)
    {
        this.Users = userDTOs;
    }
}
