namespace EnergTestTask.Models
{
    public class Field
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Size { get; set; }
        public Organization Organization { get; set; }
        public List<Location> Locations { get; set; }
    }

    public enum Organizations
    {
        Zaria = 1,
        Rassvet = 2
    }

    public enum ZariaDeps
    {
        Dep1 = 1,
        Dep2 = 2,
        Dep3 = 3
    }

    public enum RassvetDeps
    {
        Dep1 = 1
    }
}
