using System.ComponentModel.DataAnnotations;

namespace SalesInfoMVC.Models
{
    public class Manager
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Second name can not be empty!")]
        [StringLength(30, ErrorMessage = "Too many chars")]
        public string SecondName { get; set; }
    }
}