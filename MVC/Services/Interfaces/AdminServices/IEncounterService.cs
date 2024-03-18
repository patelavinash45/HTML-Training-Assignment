using Services.ViewModels.Admin;

namespace Services.Interfaces.AdminServices
{
    public interface IEncounterService
    {
        EncounterForm getEncounterDetails(int requestId, bool type);

        Task<bool> updateEncounter(EncounterForm model, int requestId);
    }
}
