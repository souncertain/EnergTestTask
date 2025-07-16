namespace EnergTestTask.Models
{
    public class Field
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Size { get; set; }
        public List<Location> Locations { get; set; }
    }
}
