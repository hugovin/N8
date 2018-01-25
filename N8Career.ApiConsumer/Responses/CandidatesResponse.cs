using N8Career.ApiConsumer.DTO;
using System.Collections.Generic;

namespace N8Career.ApiConsumer.Responses
{
    public class CandidatesResponse : BasicResponse
    {
        public List<Candidate> Candidates;

        public CandidatesResponse()
        {
            Candidates = new List<Candidate>();
        }
    }
}
