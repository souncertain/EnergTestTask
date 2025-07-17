using EnergTestTask.BL.Interfaces;
using EnergTestTask.Models;

namespace EnergTestTask.BL.Services
{
    public class KmlFieldsService : IKmlFieldsService
    {
        private readonly IKmlLoaderService _LoaderService;
        private readonly List<Field> _fields;
        public KmlFieldsService(IKmlLoaderService loaderService) 
        {
            _LoaderService = loaderService;
            _fields = loaderService.GetFields();
        }
        public double GetDistanceFromCenterToPoint(double[] point, int id)
        {
            throw new NotImplementedException();
        }

        public List<Field> GetFields()
        {
            return _fields;
        }

        public int? GetSizeById(int Id)
        {
            return _fields.FirstOrDefault(f => f.Id == Id)?.Size;
        }

        public bool IsPointInArea(double[] point)
        {
            throw new NotImplementedException();
        }
    }
}
