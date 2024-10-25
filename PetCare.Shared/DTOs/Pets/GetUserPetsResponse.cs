using PetCare.Shared.Entities.Pets;

namespace PetCare.Shared.DTOs.Pets;

public class GetUserPetsResponse : BaseResponse
{
    public IEnumerable<PetDto> Pets { get; set; }
    public GetUserPetsResponse(IEnumerable<PetDto> pets)
    {
        Pets = pets;
        Succeeded = true;
        Error = null;
    }
}