using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace web_viewer.Models.Places
{
    public class Country
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Informe o nome do pais")]
        [DisplayName("Pais")]
        public string Label { get; set; }
        public Flag Flag { get; set; }
    }
}