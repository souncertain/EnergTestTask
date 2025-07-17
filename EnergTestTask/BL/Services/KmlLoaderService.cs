using EnergTestTask.BL.Interfaces;
using EnergTestTask.Models;
using SharpKml.Dom;
using SharpKml.Engine;

namespace EnergTestTask.BL.Services
{
    public class KmlLoaderService : IKmlLoaderService
    {
        private readonly List<Field> _fields;

        private const string FieldsPath = "source/fields.kml";
        private const string CentroidsPath = "source/centroids.kml";

        public KmlLoaderService()
        {
            _fields = LoadAndParseFields();
        }

        public List<Field> GetFields() => _fields;

        public Field? GetFieldById(int id)
            => _fields.FirstOrDefault(f => f.Id == id);

        private List<Field> LoadAndParseFields()
        {
            var centroids = ParseCentroids(CentroidsPath);
            var fields = ParseFields(FieldsPath, centroids);

            return fields;
        }

        private Dictionary<int, double[]> ParseCentroids(string filePath)
        {
            var result = new Dictionary<int, double[]>();

            using var stream = File.OpenRead(filePath);
            var file = KmlFile.Load(stream);
            var placemarks = file?.Root.Flatten().OfType<Placemark>();

            foreach (var placemark in placemarks!)
            {
                var schemaData = placemark.ExtendedData?.SchemaData?.FirstOrDefault();
                if (schemaData == null)
                    continue;

                var fid = Int32.Parse(schemaData.SimpleData
                    .FirstOrDefault(d => d.Name == "fid")?.Text?.Trim());

                if (placemark.Geometry is SharpKml.Dom.Point point)
                {
                    result[fid] = new[]
                    {
                    point.Coordinate.Latitude,
                    point.Coordinate.Longitude
                };
                }
            }

            return result;
        }

        private List<Field> ParseFields(string filePath, Dictionary<int, double[]> centroids)
        {
            var result = new List<Field>();

            using var stream = File.OpenRead(filePath);
            var file = KmlFile.Load(stream);
            var placemarks = file?.Root.Flatten().OfType<Placemark>();

            foreach (var placemark in placemarks!)
            {
                if (placemark.Geometry is SharpKml.Dom.Polygon polygon)
                {
                    var schemaData = placemark.ExtendedData?.SchemaData?.FirstOrDefault();
                    if (schemaData == null)
                        continue;

                    var fid = Int32.Parse(schemaData.SimpleData
                        .FirstOrDefault(d => d.Name == "fid")?.Text?.Trim());

                    var name = placemark.Name;

                    if (!centroids.TryGetValue(fid, out var center))
                        continue;

                    var coordList = polygon.OuterBoundary.LinearRing.Coordinates
                        .Select(c => new double[] { c.Latitude, c.Longitude })
                        .ToList();

                    int size = Int32.Parse(schemaData.SimpleData
                        .FirstOrDefault(sd => sd.Name == "size")!.Text);

                    result.Add(new Field
                    {
                        Id = fid,
                        Name = name,
                        Size = size,
                        Locations = new List<EnergTestTask.Models.Location>
                    {
                        new EnergTestTask.Models.Location
                        {
                            Center = center,
                            Polygon = coordList
                        }
                    }
                    });
                }
            }

            return result;
        }
    }
}