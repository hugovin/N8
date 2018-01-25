using System.Collections.Generic;

namespace N8Career.ApiConsumer.DTO
{

    public class Candidate
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public List<Address> Address { get; set; }
    }
}
