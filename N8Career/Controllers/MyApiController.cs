using N8Career.ApiConsumer.DTO;
using N8Career.ApiConsumer.Responses;
using N8Career.ApiConsumer.Services;
using System.Web.Http;

namespace N8Career.Controllers
{
    public class MyApiController : ApiController
    {
        private readonly ICandidateService _candidateService;

        public MyApiController() : this(new CandidateService())
        {
        }

        public MyApiController(ICandidateService candidateService)
        {
            _candidateService = candidateService;
        }

        public CandidatesResponse Get(bool sendToEmail)
        {
            return _candidateService.GetCandidates(sendToEmail);
        }

        public BasicResponse Post(Candidate candidate)
        {
            return _candidateService.InsertCandidate(candidate);
        }

        public BasicResponse Put(Candidate candidate)
        {
            return _candidateService.UpdateCandidate(candidate);
        }

        public BasicResponse Delete(int candidateId)
        {
            return _candidateService.DeleteCandidate(candidateId);
        }

    }
}
