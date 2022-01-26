﻿
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace web_viewer.Models.Places
{
    public class States
    {
        /*public States(int Id = 0, string Label = null, Flag Flag = null, int CountryId = 0)
        {
            this.Id = Id;
            this.Label = Label;
            this.Flag = Flag;
            this.CountryId = CountryId;
        }*/

        public int Id { get; set; }
        [Required(ErrorMessage = "Informe nome do estado")]
        [DisplayName("Nome do estado")]
        public string Label { get; set; }
        public Flag Flag { get; set; }
        [Required(ErrorMessage = "Pais")]
        [DisplayName("Pais")]
        public int CountryId { get; set; }
    }

    public class StatesCountry
    {
        public States States { get; set; }
        public Country Countries { get; set; }
        public SelectListItem CountrySelect { get; set; }
        public IEnumerable<SelectListItem> CountriesSelect { get; set; }
    }
}