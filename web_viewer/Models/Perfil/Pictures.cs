
using System.ComponentModel;

namespace web_viewer.Models.Perfil
{
    public class Pictures
    {
        /*public Pictures(int Id=0, string Symbol=null, string Path=null)
        {
            this.Id = Id;
            this.Symbol = Symbol;
            this.Path = Path;
        }*/ 
        public int Id { get; set; }
        [DisplayName("Foto")]
        public string Symbol { get; set; }
        [DisplayName("Local Imagem")]
        public string Path { get; set; }
    }
}