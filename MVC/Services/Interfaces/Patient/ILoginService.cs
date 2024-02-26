using Repositories.ViewModels;

namespace Services.Interfaces.Patient
{
    public interface ILoginService
    {
        int auth(PatientLogin model);
    }
}
