using N8Career.ApiConsumer.DTO;
using N8Career.ApiConsumer.Enum;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;

namespace N8Career.ApiConsumer.RestClients
{
    public interface IMyApiRestclient
    {
        List<Candidate> GetAllCandidates();
        bool InsertCandidate(Candidate candidate);
        bool UdpateCandidate(Candidate candidate);
        bool DeleteCandidate(int candidateId);
    }
    public class MyApiRestclient : IMyApiRestclient
    {
        private readonly string _myApiUrl;
        private readonly string _myApiDb;

        public MyApiRestclient()
        {
            _myApiUrl = ConfigurationManager.AppSettings["MyApiUrl"];
            _myApiDb = ConfigurationManager.AppSettings["MyApiDb"];
        }
        public List<Candidate> GetAllCandidates()
        {
            var response = new List<Candidate>();
            try
            {
                using (var client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(HeaderTypeEnum.Json));
                    var task = client.GetAsync(_myApiUrl + _myApiDb + "/Candidate")
                        .ContinueWith((taskwithresponse) =>
                        {
                            var apiResponses = taskwithresponse.Result;
                            var jsonString = apiResponses.Content.ReadAsStringAsync();
                            jsonString.Wait();
                            response = JsonConvert.DeserializeObject<List<Candidate>>(jsonString.Result);

                        });
                    task.Wait();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            return response;

        }

        public bool InsertCandidate(Candidate candidate)
        {
            var response = false;
            try
            {
                using (var client = new HttpClient())
                {
                    var stringContent = new StringContent(JsonConvert.SerializeObject(candidate), Encoding.UTF8, HeaderTypeEnum.Json);
                    var responses = client.PostAsync(_myApiUrl + _myApiDb + "/Candidate", stringContent).Result;
                    response = responses.IsSuccessStatusCode;
                }
            }
            catch (Exception e)
            {
                response = false;
                throw;
            }
            return response;
        }

        public bool UdpateCandidate(Candidate candidate)
        {
            var response = false;
            try
            {
                using (var client = new HttpClient())
                {
                    var stringContent = new StringContent(JsonConvert.SerializeObject(candidate), Encoding.UTF8, HeaderTypeEnum.Json);
                    var responses = client.PutAsync(_myApiUrl + _myApiDb + "/Candidate", stringContent).Result;
                    response = responses.IsSuccessStatusCode;
                }
            }
            catch (Exception e)
            {
                response = false;
                throw;
            }
            return response;
        }

        public bool DeleteCandidate(int candidateId)
        {
            var response = false;
            try
            {
                using (var client = new HttpClient())
                {
                    var responses = client.DeleteAsync(_myApiUrl + _myApiDb + "/Candidate/" + candidateId).Result;
                    response = responses.IsSuccessStatusCode;
                }
            }
            catch (Exception e)
            {
                response = false;
                throw;
            }
            return response;
        }
    }
}
