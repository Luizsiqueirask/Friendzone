using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace web_viewer.Models.Perfil
{
    public class Contacts
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Informe e-mail")]
        [DisplayName("E-mail")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Informe nunero mobile")]
        [DisplayName("Mobile")]
        public string Mobile { get; set; }
    }
}