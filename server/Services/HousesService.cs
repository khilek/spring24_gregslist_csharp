



namespace csharp_gregslist_api.Services;

public class HousesService
{
  private readonly HousesRepository _repository;
  public HousesService(HousesRepository repository)
  {
    _repository = repository;
  }

  internal House UpdateHouse(int houseId, string userId, House houseData)
  {
    House HouseToUpdate = GetHouseById(houseId);

    if (HouseToUpdate.CreatorId != userId)
    {
      throw new Exception("You are not the creator of this House");
    }

    HouseToUpdate.SqFt = houseData.SqFt ?? HouseToUpdate.SqFt;
    HouseToUpdate.Bathrooms = houseData.Bathrooms ?? HouseToUpdate.Bathrooms;
    HouseToUpdate.Bedrooms = houseData.Bedrooms ?? HouseToUpdate.Bedrooms;
    HouseToUpdate.Description = houseData.Description ?? HouseToUpdate.Description;
    HouseToUpdate.Price = houseData.Price ?? HouseToUpdate.Price;

    House updatedHouse = _repository.UpdateHouse(HouseToUpdate);


    return HouseToUpdate;
  }

  internal string CondemnHouse(int houseId, string userId)
  {
    House HouseToCondemn = GetHouseById(houseId);

    if (HouseToCondemn.CreatorId != userId)
    {
      throw new Exception("NOT YOUR HOUSE POST TO CONDEMN");
    }
    _repository.CondemnHouse(houseId);

    return "HOUSE has been CONDEMNED";
  }



  internal House CreateHouse(House houseData)
  {
    House house = _repository.CreateHouse(houseData);
    return house;
  }

  internal House GetHouseById(int houseId)
  {
    House house = _repository.GetHouseById(houseId);

    if (house == null)
    {
      throw new Exception($"Invalid id: {houseId}");
    }
    return house;
  }


  internal List<House> GetHouses()
  {
    List<House> houses = _repository.GetHouses();
    return houses;
  }


}