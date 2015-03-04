using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoMapper.Test.Entitys.Dto
{
public class BookStoreDto  
{  
    public string Name { get; set; }  
    public List<BookDto> Books { get; set; }  
    public AddressDto Address { get; set; }  
}  

}
