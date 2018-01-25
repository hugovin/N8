using N8Career.ApiConsumer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace N8Career.ApiConsumer.Utilities
{
    public static class CandidateEmailCreator
    {
        public static string CreateEmailContent(string processMessage, List<Candidate> candidates)
        {
            var email = Resource.EmailTemplate.ToString();
            try
            {

                email = email.Replace("<processMessage>", processMessage);
                var mailInfo = new StringBuilder();
                foreach (var candidate in candidates)
                {
                    var sb = new StringBuilder();
                    var candidateInfo = Resource.UserInformationArea;
                    candidateInfo = candidateInfo.Replace("<firstName>", candidate.FirstName);
                    candidateInfo = candidateInfo.Replace("<lastName>", candidate.LastName);
                    candidateInfo = candidateInfo.Replace("<email>", candidate.Email);
                    candidateInfo = candidateInfo.Replace("<phone>", candidate.Phone);
                    if (candidate.Address != null && candidate.Address.Any())
                    {
                        foreach (var address in candidate.Address)
                        {
                            sb.AppendLine("<tr><td>" + address.Country + "</td><td>" + address.StateCode + "</td><td>" + address.City + "</td><td>" + address.Zip + "</td></tr>");
                        }
                        candidateInfo = candidateInfo.Replace("<tableInformation>", sb.ToString());
                    }
                    mailInfo.Append(candidateInfo);
                }
                email = email.Replace("<userInformation>", mailInfo.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            return email;
        }
    }
}
