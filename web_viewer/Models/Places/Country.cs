using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace web_viewer.Models.Places
{
    public class Country
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Informe o nome do pais")]
        [DisplayName("Nome do pais")]
        public string Label { get; set; }
        public Flag Flag { get; set; }
    }
}