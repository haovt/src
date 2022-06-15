using Microsoft.OData.ModelBuilder;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MusicStore.Models
{
    public class Genre
    {
        public int GenreId { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }

        [Contained]
        public List<Album> Albums { get; set; }

        //public BaseModelType BaseModelType { get; set; }
    }
}