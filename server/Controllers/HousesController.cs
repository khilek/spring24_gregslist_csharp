using Microsoft.AspNetCore.Http.HttpResults;

namespace csharp_gregslist_api.Controllers;

[ApiController]
[Route("api/houses")]

public class HousesController : ControllerBase
{
  private readonly HousesService _housesServices;
  private readonly Auth0Provider _auth0Provider;


  public HousesController(HousesService housesService, Auth0Provider auth0Provider)
  {
    _housesServices = housesService;
    _auth0Provider = auth0Provider;
  }

  [Authorize]
  [HttpDelete("{houseId}")]

  public async Task<ActionResult<string>> CondemnHouse(int houseId)
  {
    try
    {
      Account userInfo = await _auth0Provider.GetUserInfoAsync<Account>(HttpContext);
      string message = _housesServices.CondemnHouse(houseId, userInfo.Id);
      return Ok(message);
    }
    catch (Exception exception)
    {

      return BadRequest(exception.Message);
    }
  }



  [Authorize]
  [HttpPut("{houseId}")]
  public async Task<ActionResult<House>> UpdateHouse(int houseId, [FromBody] House houseData)
  {
    try
    {
      Account userInfo = await _auth0Provider.GetUserInfoAsync<Account>(HttpContext);
      House house = _housesServices.UpdateHouse(houseId, userInfo.Id, houseData);
      return Ok(house);
    }
    catch (Exception exception)
    {

      return BadRequest(exception.Message);
    }
  }


  [Authorize]
  [HttpPost]
  public async Task<ActionResult<House>> CreateHouse([FromBody] House houseData)
  {
    try
    {
      Account userInfo = await _auth0Provider.GetUserInfoAsync<Account>(HttpContext);

      houseData.CreatorId = userInfo.Id;

      House house = _housesServices.CreateHouse(houseData);
      return Ok(house);
    }
    catch (Exception exception)
    {
      return BadRequest(exception.Message);
    }
  }

  [Authorize]
  [HttpGet("{houseId}")]
  public ActionResult<House> GetHouseById(int houseId)
  {
    try
    {
      House house = _housesServices.GetHouseById(houseId);
      return Ok(house);
    }
    catch (Exception exception)
    {

      return BadRequest(exception.Message);
    }
  }


  [HttpGet]
  public ActionResult<List<House>> GetHouses()
  {
    try
    {
      List<House> houses = _housesServices.GetHouses();
      return Ok(houses);
    }
    catch (Exception exception)
    {

      return BadRequest(exception.Message);
    }
  }



}