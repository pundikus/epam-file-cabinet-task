using System.Collections.ObjectModel;

namespace FileCabinetApp
{
    public interface IFileCabinetService
    {
        int CreateRecord(ValueRange parametrs);
        void EditRecord(ValueRange parametrs);
        ReadOnlyCollection<FileCabinetRecord> FindByDateOfBirth(string dateOfBirth);
        ReadOnlyCollection<FileCabinetRecord> FindByFirstName(string firstName);
        ReadOnlyCollection<FileCabinetRecord> FindByLastName(string lastName);
        ReadOnlyCollection<FileCabinetRecord> GetRecords();
        int GetStat();
    }
}