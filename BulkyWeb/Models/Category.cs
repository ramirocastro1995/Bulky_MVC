namespace BulkyWeb.Models
{
    /// <summary>
    /// Category is the table that we want to create in EF,with id/name/display rows
    /// </summary>
    public class Category
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public int DisplayOrder { get; set; }
    }
}
