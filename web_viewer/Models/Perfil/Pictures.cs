
using System.ComponentModel;

namespace web_viewer.Models.Perfil
{
    public class Pictures
    {
        public int Id { get; set; }
        [DisplayName("Foto")]
        public string Symbol { get; set; }
        [DisplayName("Local Imagem")]
        public string Path { get; set; }
    }
}