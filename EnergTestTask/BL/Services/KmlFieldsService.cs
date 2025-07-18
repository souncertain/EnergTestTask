using EnergTestTask.BL.Interfaces;
using EnergTestTask.Models;
using NetTopologySuite.Geometries;
using ProjNet;
using ProjNet.CoordinateSystems;

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

        public (int Id, string Name)? IsPointInArea(double[] point)
        {
            var geomFactory = new GeometryFactory(new PrecisionModel(), 4326);

            var wgs84 = GeographicCoordinateSystem.WGS84;
            var utmZone = ProjectedCoordinateSystem.WGS84_UTM(32, true);

            var ctFactory = new ProjNet.CoordinateSystems.Transformations.CoordinateTransformationFactory();
            var transform = ctFactory.CreateFromCoordinateSystems(wgs84, utmZone);

            var testPoint = geomFactory.CreatePoint(new Coordinate(point[1], point[0]));
            var testPointCoords = transform.MathTransform.Transform(new[] { testPoint.X, testPoint.Y });
            var transformedPoint = geomFactory.CreatePoint(new Coordinate(testPointCoords[0], testPointCoords[1]));

            foreach (var field in _fields)
            {
                foreach (var location in field.Locations)
                {
                    var coords = location.Polygon
                        .Select(p => new Coordinate(p[1], p[0]))
                        .Select(c => transform.MathTransform.Transform(new[] { c.X, c.Y }))
                        .Select(c => new Coordinate(c[0], c[1]))
                        .ToArray();

                    if (!coords.First().Equals2D(coords.Last(), 1e-6))
                        coords = coords.Append(coords.First()).ToArray();

                    var ring = geomFactory.CreateLinearRing(coords);
                    var polygon = geomFactory.CreatePolygon(ring);

                    if (polygon.Contains(transformedPoint) || polygon.Intersects(transformedPoint))
                        return (field.Id, field.Name);
                }
            }

            return null;
        }
    }
}
