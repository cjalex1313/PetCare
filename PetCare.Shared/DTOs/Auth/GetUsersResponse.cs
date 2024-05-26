namespace PetCare.Shared.DTOs.Auth;

public class GetUsersResponse
{
    public List<UserDTO>? Users { get; set; }

    public GetUsersResponse(List<UserDTO> userDTOs)
    {
        this.Users = userDTOs;
    }
}
