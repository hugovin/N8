using Moq;
using N8Career.ApiConsumer.DTO;
using N8Career.ApiConsumer.RestClients;
using N8Career.ApiConsumer.Services;
using NUnit.Framework;
using NUnit.Framework.Internal;
using System.Collections.Generic;

namespace N8Career.ApiConsumerTest
{
    [TestFixture]
    public class CandidateServiceTest
    {
        private Mock<IMyApiRestclient> _apiClient;
        private Mock<IEmailService> _emailService;

        [Test, Category("Unit")]
        public void Test_AddCandidate()
        {
            _apiClient = new Mock<IMyApiRestclient>();
            _emailService = new Mock<IEmailService>();

            _apiClient.Setup(m => m.InsertCandidate(It.IsAny<Candidate>())).Returns(true);

            var candidateService = new CandidateService(_apiClient.Object, _emailService.Object);
            Assert.IsTrue(candidateService.InsertCandidate(new Candidate()).Acknowledgement);
        }

        [Test, Category("Unit")]
        public void Test_GetCandidates()
        {
            _apiClient = new Mock<IMyApiRestclient>();
            _emailService = new Mock<IEmailService>();
            _apiClient.Setup(m => m.GetAllCandidates()).Returns(new List<Candidate>() { new Candidate() });
            _emailService.Setup(m => m.SendEmail(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Returns(true);
            var candidateService = new CandidateService(_apiClient.Object, _emailService.Object);
            var response = candidateService.GetCandidates(false);
            Assert.AreEqual(1, response.Candidates.Count);
            Assert.IsTrue(response.Acknowledgement);

        }

        [Test, Category("Unit")]
        public void Test_UpdateCandidate()
        {
            _apiClient = new Mock<IMyApiRestclient>();
            _emailService = new Mock<IEmailService>();

            _apiClient.Setup(m => m.UdpateCandidate(It.IsAny<Candidate>())).Returns(true);

            var candidateService = new CandidateService(_apiClient.Object, _emailService.Object);
            Assert.IsTrue(candidateService.UpdateCandidate(new Candidate()).Acknowledgement);
        }

        [Test, Category("Unit")]
        public void Test_DeleteCandidate()
        {
            _apiClient = new Mock<IMyApiRestclient>();
            _emailService = new Mock<IEmailService>();

            _apiClient.Setup(m => m.DeleteCandidate(It.IsAny<int>())).Returns(true);

            var candidateService = new CandidateService(_apiClient.Object, _emailService.Object);
            Assert.IsTrue(candidateService.DeleteCandidate(10).Acknowledgement);
        }

    }
}
