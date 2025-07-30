namespace EnergTestTask.Models
{
    public class Organization
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<Departament> Departaments { get; set; }
    }
}
