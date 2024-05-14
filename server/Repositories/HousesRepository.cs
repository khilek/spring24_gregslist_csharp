




namespace csharp_gregslist_api.Repositories;

public class HousesRepository
{
  private readonly IDbConnection _db;

  public HousesRepository(IDbConnection db)
  {
    _db = db;
  }

  internal void CondemnHouse(int houseId)
  {
    string sql = "DELETE FROM houses WHERE id = @houseId;";

    _db.Execute(sql, new { houseId });
  }



  internal House CreateHouse(House houseData)
  {
    string sql = @"
        INSERT INTO
        houses(
          sqft,
          bedrooms,
          bathrooms,
          imgUrl,
          description,
          price,
          creatorId
        )
        VALUES(
          @sqft,
          @bedrooms,
          @bathrooms,
          @imgUrl,
          @description,
          @price,
          @creatorId
        );

        SELECT * FROM houses WHERE id = LAST_INSERT_ID();";

    House house = _db.Query<House>(sql, houseData).FirstOrDefault();

    return house;
  }

  internal House UpdateHouse(House HouseToUpdate)
  {
    string sql = @"
        UPDATE houses
        SET
        sqft = @SqFt,
        bedrooms = @Bedrooms,
        bathrooms = @Bathrooms,
        description = @Description,
        price = @Price
        WHERE id = @Id;

        SELECT
        houses.*,
        accounts.*
        FROM houses
        JOIN accounts ON accounts.id = houses.creatorId
        WHERE houses.id = @Id;";

    House house = _db.Query<House, Account, House>(sql, (house, account) =>
    {
      house.Creator = account;
      return house;
    }, HouseToUpdate).FirstOrDefault();

    return house;
  }



  internal House GetHouseById(int houseId)
  {
    string sql = @"
      SELECT
      houses.*,
      accounts.*
      FROM houses
      JOIN accounts ON accounts.id = houses.creatorId
      WHERE houses.id = @houseId;";

    House house = _db.Query<House, Account, House>(sql, (house, account) =>
    {
      house.Creator = account;
      return house;
    }, new { houseId }).FirstOrDefault();
    return house;
  }

  internal List<House> GetHouses()
  {
    string sql = @"
      SELECT
     houses.*,
     accounts.*
      FROM houses
      JOIN accounts ON accounts.id = houses.creatorId
      ;";

    List<House> houses = _db.Query<House, Account, House>(sql, (house, account) =>
    {
      house.Creator = account;
      return house;
    }).ToList();

    return houses;

  }


}