using BlazorWebAppAuthentication.Domain.Entities;

namespace BlazorWebAppAuthentication.Database.Interfaces;

public interface IFradulentNamesRepository
{
    string GetFraudulentName(string name);
    List<FraudulentNames> GetFraudulentNamesById(int id);
    List<FraudulentNames> GetAllFraudulentNames();
}