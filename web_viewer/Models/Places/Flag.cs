using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web;

namespace web_viewer.Models.Places
{
    public class Flag
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Informe nome da bandeira")]
        [DisplayName("Nome do baneira")]
        public string Symbol { get; set; }
        [DisplayName("Bandeira")]
        public string Path { get; set; }
    }
}