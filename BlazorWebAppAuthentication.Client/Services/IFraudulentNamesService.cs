using BlazorWebAppAuthentication.Domain.Entities;

namespace BlazorWebAppAuthentication.Client.Services;

public interface IFraudulentNamesService 
{
    string GetFraudulentName(string name);
    List<FraudulentNames> GetFraudulentNamesById(int id);
    List<FraudulentNames> GetAllFraudulentNames();
}