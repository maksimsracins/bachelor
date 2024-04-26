using BlazorWebAppAuthentications.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace BlazorWebAppAuthentications.API.Controllers;
[Route("api/[controller]")]
[ApiController]
public class CountryController : Controller
{
    private readonly ICountryRepository _countryRepository;

    public CountryController(ICountryRepository countryRepository)
    {
        _countryRepository = countryRepository;
    }

    [HttpGet]
    public IActionResult GetCountries() => Ok(_countryRepository.GetAllCountries());

    [HttpGet("{id}")]
    public IActionResult GetCountryById(int id) => Ok(_countryRepository.GetCountryById(id));

}