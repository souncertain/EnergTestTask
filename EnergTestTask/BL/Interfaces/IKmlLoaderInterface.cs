using EnergTestTask.Models;

namespace EnergTestTask.BL.Interfaces
{
    public interface IKmlLoaderInterface
    {
        IReadOnlyCollection<Field> GetFields();
        Field? GetFieldById(Guid Id);
    }
}
