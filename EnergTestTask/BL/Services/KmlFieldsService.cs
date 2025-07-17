using EnergTestTask.BL.Interfaces;
using EnergTestTask.Models;
using NetTopologySuite.Geometries;

namespace EnergTestTask.BL.Services
{
    public class KmlFieldsService : IKmlFieldsService
    {
        private readonly IKmlLoaderService _LoaderService;
        private readonly List<Field> _fields;
        private readonly Dictionary<int, double[]> _centroids;
        public KmlFieldsService(IKmlLoaderService loaderService) 
        {
            _LoaderService = loaderService;
            _fields = loaderService.GetFields();
            _centroids = loaderService.GetCentroids();
        }
        public double GetDistanceFromCenterToPoint(double[] point, int id)
        {
            var center = _centroids.FirstOrDefault(f => f.Key == id).Value;

            var coord1 = new Point(point[1], point[0]);
            var coord2 = new Point(center[1], center[0]);
            return GetDistanceInMeters(point, center);
        }

        private static double GetDistanceInMeters(double[] point1, double[] point2)
        {
            const double R = 6371e3;
            double lat1 = DegreesToRadians(point1[0]);
            double lat2 = DegreesToRadians(point2[0]);
            double deltaLat = DegreesToRadians(point2[0] - point1[0]);
            double deltaLon = DegreesToRadians(point2[1] - point1[1]);

            double a = Math.Sin(deltaLat / 2) * Math.Sin(deltaLat / 2) +
                       Math.Cos(lat1) * Math.Cos(lat2) *
                       Math.Sin(deltaLon / 2) * Math.Sin(deltaLon / 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c;
        }

        private static double DegreesToRadians(double deg) => deg * Math.PI / 180;

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
