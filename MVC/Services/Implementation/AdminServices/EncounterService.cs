using Repositories.DataModels;
using Repositories.Interfaces;
using Services.Interfaces.AdminServices;
using Services.ViewModels.Admin;

namespace Services.Implementation.AdminServices
{
    public class EncounterService : IEncounterService
    {
        private readonly IEncounterRepository _encounterRepository;

        public EncounterService(IEncounterRepository encounterRepository)
        {
            _encounterRepository = encounterRepository;
        }

        public EncounterForm getEncounterDetails(int requestId,bool type)
        {
            Encounter encounter = _encounterRepository.getEncounter(requestId);
            EncounterForm encounterForm = new EncounterForm();
            if(encounter != null)
            {
                encounterForm = new EncounterForm()
                {
                    IsAdmin = type,
                    FirstName = encounter.FirstName,
                    LastName = encounter.LastName,
                    Location = encounter.Location,
                    Email = encounter.Email,
                    BirthDate = DateTime.Parse(encounter.IntYear + "-" + encounter.StrMonth
                                 + "-" + encounter.IntDate),
                    Date = encounter.Date,
                    Mobile = encounter.PhoneNumber,
                    HistoryOfIllness = encounter.IllnessOrInjury,
                    MedicalHistory = encounter.MedicalHistory,
                    Medications = encounter.Medications,
                    Allergies = encounter.Allergies,
                    Temp = encounter.Temperature,
                    HeartRate = encounter.HeartRate,
                    RespiratoryRate = encounter.RespiratoryRate,
                    BloodPressure1 = encounter.BloodPressure1,
                    BloodPressure2 = encounter.BloodPressure2,
                    O2 = encounter.O2,
                    Pain = encounter.Pain,
                    Heent = encounter.Heent,
                    CV = encounter.Cv,
                    Chest = encounter.Chest,
                    ABD = encounter.Abd,
                    Extra = encounter.Extr,
                    Skin = encounter.Skin,
                    Neuro = encounter.Neuro,
                    Other = encounter.Other,
                    Diagnosis = encounter.Diagnosis,
                    TreatmentPlan = encounter.TreatmentPlan,
                    Dispensed = encounter.MedicationsDispensed,
                    Procedures = encounter.Procedures,
                    FollowUp = encounter.Followup,
                };
            }
            return encounterForm;
        }

        public async Task<bool> updateEncounter(EncounterForm model,int requestId)
        {
            Encounter encounter = _encounterRepository.getEncounter(requestId);
            if (encounter == null)
            {
                Encounter _encounter = new Encounter()
                {
                    RequestId = requestId,
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Location = model.Location,
                    Email = model.Email,
                    IntYear = model.BirthDate.Value.Year,
                    IntDate = model.BirthDate.Value.Day,
                    StrMonth = model.BirthDate.Value.Month.ToString(),
                    Date = model.Date,
                    PhoneNumber = model.Mobile,
                    IllnessOrInjury = model.HistoryOfIllness,
                    MedicalHistory = model.MedicalHistory,
                    Medications = model.Medications,
                    Allergies = model.Allergies,
                    Temperature = model.Temp,
                    HeartRate = model.HeartRate,
                    RespiratoryRate = model.RespiratoryRate,
                    BloodPressure1 = model.BloodPressure1,
                    BloodPressure2 = model.BloodPressure2,
                    O2 = model.O2,
                    Pain = model.Pain,
                    Heent = model.Heent,
                    Cv = model.CV,
                    Chest = model.Chest,
                    Abd = model.ABD,
                    Extr = model.Extra,
                    Skin = model.Skin,
                    Neuro = model.Neuro,
                    Other = model.Other,
                    Diagnosis = model.Diagnosis,
                    TreatmentPlan = model.TreatmentPlan,
                    MedicationsDispensed = model.Dispensed,
                    Procedures = model.Procedures,
                    Followup = model.FollowUp,
                };
                return await _encounterRepository.addEncounter(_encounter);
            }
            else
            {
                encounter.FirstName = model.FirstName;
                encounter.LastName = model.LastName;
                encounter.Location = model.Location;
                encounter.Email = model.Email;
                encounter.IntYear = model.BirthDate.Value.Year;
                encounter.IntDate = model.BirthDate.Value.Day;
                encounter.StrMonth = model.BirthDate.Value.Month.ToString();
                encounter.Date = model.Date;
                encounter.PhoneNumber = model.Mobile;
                encounter.IllnessOrInjury = model.HistoryOfIllness;
                encounter.MedicalHistory = model.MedicalHistory;
                encounter.Medications = model.Medications;
                encounter.Allergies = model.Allergies;
                encounter.Temperature = model.Temp;
                encounter.HeartRate = model.HeartRate;
                encounter.RespiratoryRate = model.RespiratoryRate;
                encounter.BloodPressure1 = model.BloodPressure2;
                encounter.BloodPressure2 = model.BloodPressure2;
                encounter.O2 = model.O2;
                encounter.Pain = model.Pain;
                encounter.Heent = model.Heent;
                encounter.Cv = model.CV;
                encounter.Chest = model.Chest;
                encounter.Abd = model.ABD;
                encounter.Extr = model.Extra;
                encounter.Skin = model.Skin;
                encounter.Neuro = model.Neuro;
                encounter.Other = model.Other;
                encounter.Diagnosis = model.Diagnosis;
                encounter.TreatmentPlan = model.TreatmentPlan;
                encounter.MedicationsDispensed = model.Dispensed;
                encounter.Procedures = model.Procedures;
                encounter.Followup = model.FollowUp;
            }
            return await _encounterRepository.updateEncounter(encounter);
        }
    }
}
