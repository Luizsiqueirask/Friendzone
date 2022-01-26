﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using web_viewer.Models.Places;

namespace web_viewer.Models.Perfil
{
    public class Person
    {      
        public int Id { get; set; }
        [Required(ErrorMessage = "Informe nome")]
        [DisplayName("Nome")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Informe sobrenome")]
        [DisplayName("Sobrenome")]
        public string LastName { get; set; }
        [Required(ErrorMessage = "Informe idade")]
        [DisplayName("Idade")]
        public int Age { get; set; }
        [Required(ErrorMessage = "Informe nome")]
        [DisplayName("Data de aniversário")]
        public DateTime Birthday { get; set; }
        [Required(ErrorMessage = "inseri uma foto")]
        [DisplayName("Foto")]
        public Pictures Picture { get; set; }
        public Contacts Contacts { get; set; }
        [Required(ErrorMessage = "Escolha um pais")]
        [DisplayName("Pais")]
        public int CountryId { get; set; }
    }
    public class PersonCountry
    {
        public Country Countries { get; set; }
        public Person People { get; set; }
        public Contacts Contacts { get; set; }
        public SelectListItem CountrySelect { get; set; }
        public IEnumerable<SelectListItem> CountriesSelect { get; set; }
    }
}