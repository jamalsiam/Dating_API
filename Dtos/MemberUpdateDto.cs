using System;

namespace Api.Dtos
{
    public class MemberUpdateDto
    {
         public string firstName { get; set; }
         public string lastName { get; set; }
         public bool gender { get; set; }
         public DateTime birthdate { get; set; }
         public string city { get; set; }
         public string country { get; set; }
    }
}