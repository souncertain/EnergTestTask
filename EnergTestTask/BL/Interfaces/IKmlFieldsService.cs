using EnergTestTask.Models;

namespace EnergTestTask.BL.Interfaces
{
    public interface IKmlFieldsService
    {
        public List<Field> GetFields();
        public int? GetSizeById(int Id);
        public double GetDistanceFromCenterToPoint(double[] point, int id);
        public (int Id, string Name)? IsPointInArea(double[] point);
    }
}
