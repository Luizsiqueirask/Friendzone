using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace web_viewer.Models.Perfil
{
    public class Contacts
    {
        /*public Contacts()
        {
        }

        public Contacts(int Id, string Email, string Mobile)
        {
            this.Id = Id;
            this.Email = Email;
            this.Mobile = Mobile;
        }*/
        public int Id { get; set; }
        [Required(ErrorMessage = "Informe e-mail")]
        [DisplayName("E-mail")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Informe nunero mobile")]
        [DisplayName("Mobile")]
        public string Mobile { get; set; }
    }
}