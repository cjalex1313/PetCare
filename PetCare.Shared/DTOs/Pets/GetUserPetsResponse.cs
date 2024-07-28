using PetCare.Shared.Entities.Pets;

namespace PetCare.Shared.DTOs.Pets;

public class GetUserPetsResponse : BaseResponse
{
    public IEnumerable<PetDTO> Pets { get; set; }
    public GetUserPetsResponse(IEnumerable<PetDTO> pets)
    {
        Pets = pets;
        Succeeded = true;
        Error = null;
    }
}