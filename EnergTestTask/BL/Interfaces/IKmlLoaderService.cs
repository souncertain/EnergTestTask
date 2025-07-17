using EnergTestTask.Models;

namespace EnergTestTask.BL.Interfaces
{
    public interface IKmlLoaderService
    {
        List<Field> GetFields();
        Field? GetFieldById(int Id);
        Dictionary<int, double[]> GetCentroids();
    }
}
