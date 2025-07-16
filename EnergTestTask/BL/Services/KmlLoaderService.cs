using EnergTestTask.BL.Interfaces;
using EnergTestTask.Models;

namespace EnergTestTask.BL.Services
{
    public class KmlLoaderService : IKmlLoaderInterface
    {
        private readonly List<Field> _fields;

        private readonly string _fieldsPath = "EnergTestTask/source/fields.kml";
        private readonly string _centroidsPath = "EnergTestTask/source/centroids.kml";

        public KmlLoaderService()
        {
            _fields = Load();
        }

        public Field? GetFieldById(Guid Id)
        {
            return _fields.FirstOrDefault(f => f.Id == Id);
        }

        public IReadOnlyCollection<Field> GetFields() => _fields;
        private List<Field>Load()
        {

        }
    }
}
