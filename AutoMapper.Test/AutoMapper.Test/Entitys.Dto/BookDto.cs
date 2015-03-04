using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoMapper.Test.Entitys.Dto
{
    public class BookDto
    {

        public string Title { get; set; }
        public string Description { get; set; }
        public string Language { get; set; }
        public decimal Price { get; set; }
        public DateTime? PublishDate { get; set; }
        public string Publisher { get; set; }
        public int? Paperback { get; set; }
        public string FirstAuthorName { get; set; }
        public string FirstAuthorDescription { get; set; }
        public string FirstAuthorEmail { get; set; }
        public string FirstAuthorBlog { get; set; }
        public string FirstAuthorTwitter { get; set; }
        public string SecondAuthorName { get; set; }
        public string SecondAuthorDescription { get; set; }
        public string SecondAuthorEmail { get; set; }
        public string SecondAuthorBlog { get; set; }
        public string SecondAuthorTwitter { get; set; }
    }

}
