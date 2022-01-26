
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace web_viewer.Models.Places
{
    public class Flag
    {
        /*public Flag(int Id = 0, string Symbol = null, string Path = null)
        {
            this.Id = Id;
            this.Symbol = Symbol;
            this.Path = Path;
        }*/

        public int Id { get; set; }
        [Required(ErrorMessage = "Informe nome da bandeira")]
        [DisplayName("Nome do baneira")]
        public string Symbol { get; set; }
        [DisplayName("Local do arquivo")]
        public string Path { get; set; }
    }
}