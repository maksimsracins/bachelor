using BlazorWebAppAuthentication.Client.Services.Interfaces;
using BlazorWebAppAuthentication.Database.Interfaces;
using BlazorWebAppAuthentication.Domain.Entities;

namespace BlazorWebAppAuthentication.Client.Services;

public class FraudulentNamesService : IFraudulentNamesService
{
    private readonly IFradulentNamesRepository _fradulentNamesRepository;

    public FraudulentNamesService(IFradulentNamesRepository fradulentNamesRepository)
    {
        _fradulentNamesRepository = fradulentNamesRepository;
    }
    
    public string GetFraudulentName(string name)
    {
        return _fradulentNamesRepository.GetFraudulentName(name);
    }

    public List<FraudulentNames> GetFraudulentNamesById(int id)
    {
        return _fradulentNamesRepository.GetFraudulentNamesById(id);
    }

    public List<FraudulentNames> GetAllFraudulentNames()
    {
        return _fradulentNamesRepository.GetAllFraudulentNames();
    }
}