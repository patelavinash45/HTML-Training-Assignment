using Services.ViewModels.Admin;
using System.Data;

namespace Services.Interfaces.AdminServices
{
    public interface IRecordService
    {
        Records getRecords(Records model);

        EmailSmsLogs getEmailLog(EmailSmsLogs model);

        List<EmailSmsLogTableData> getEmailLogTabledata(EmailSmsLogs model);

        EmailSmsLogs getSMSlLog(EmailSmsLogs model);

        List<EmailSmsLogTableData> getSMSLogTabledata(EmailSmsLogs model);

        DataTable ExportAllRecords();
    }
}
