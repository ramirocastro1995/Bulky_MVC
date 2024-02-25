using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace BulkyWeb.Models
{
    /// <summary>
    /// Category is the table that we want to create in EF,with id/name/display rows
    /// </summary>
    public class Category
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(30)]
        [DisplayName("Category Name")]
        public string Name { get; set; }
        [DisplayName("Display Order")]
        [Range(1,100, ErrorMessage = "Display order must be between 1 - 100")]
        public int DisplayOrder { get; set; }
    }
}
