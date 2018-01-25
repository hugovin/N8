using N8Career.ApiConsumer.DTO;
using N8Career.ApiConsumer.Responses;
using N8Career.ApiConsumer.RestClients;
using N8Career.ApiConsumer.Utilities;
using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;

namespace N8Career.ApiConsumer.Services
{
    public interface ICandidateService
    {
        CandidatesResponse GetCandidates(bool sendToEmail);
        BasicResponse InsertCandidate(Candidate candidate);
        BasicResponse UpdateCandidate(Candidate candidate);

        BasicResponse DeleteCandidate(int id);


    }
    public class CandidateService : ICandidateService
    {
        private readonly IMyApiRestclient _myApiRestclient;
        private static Logger _logger;
        private readonly bool _sendToEmail;
        private readonly IEmailService _emailService;

        public CandidateService() : this(new MyApiRestclient(), new EmailService()) { }

        public CandidateService(IMyApiRestclient myApiRestclient, IEmailService emailService)
        {
            _myApiRestclient = myApiRestclient;
            _logger = LogManager.GetCurrentClassLogger();
            _sendToEmail = Convert.ToBoolean(ConfigurationManager.AppSettings["ToEmail"]);
            _emailService = emailService;
        }


        public CandidatesResponse GetCandidates(bool sendToEmail)
        {
            var response = new CandidatesResponse();
            try
            {
                response.Candidates = _myApiRestclient.GetAllCandidates();
                response.Acknowledgement = true;
                response.Message = "Success";
                if (sendToEmail)
                {
                    _emailService.SendEmail(CandidateEmailCreator.CreateEmailContent(Resource.GetAllUsersProcessMessage, response.Candidates), "Candidate Result", "max.madrigal@careerbuilder.com");
                }
            }
            catch (Exception e)
            {
                response.Acknowledgement = false;
                response.Message = "Error: " + e.Message;
            }
            return response;
        }

        public BasicResponse InsertCandidate(Candidate candidate)
        {
            var response = new CandidatesResponse();
            try
            {
                response.Acknowledgement = _myApiRestclient.InsertCandidate(candidate);
                if (!response.Acknowledgement)
                {
                    response.Message = "Error Adding Candidate";
                    return response;
                }
                response.Message = "Success";
            }
            catch (Exception e)
            {
                response.Acknowledgement = false;
                response.Message = "Error: " + e.Message;
            }
            finally
            {
                LogCandidateInformation(response, candidate);
            }
            return response;
        }

        public BasicResponse UpdateCandidate(Candidate candidate)
        {
            var response = new CandidatesResponse();
            try
            {
                response.Acknowledgement = _myApiRestclient.UdpateCandidate(candidate);
                if (!response.Acknowledgement)
                {
                    response.Message = "Error Updating Candidate";
                    return response;
                }
                response.Message = "Success";
            }
            catch (Exception e)
            {
                response.Acknowledgement = false;
                response.Message = "Error: " + e.Message;
            }
            return response;
        }

        public BasicResponse DeleteCandidate(int id)
        {
            var response = new CandidatesResponse();
            try
            {
                response.Acknowledgement = _myApiRestclient.DeleteCandidate(id);
                if (!response.Acknowledgement)
                {
                    response.Message = "Error Deleting Candidate";
                    return response;
                }
                response.Message = "Success";
            }
            catch (Exception e)
            {
                response.Acknowledgement = false;
                response.Message = e.Message;
            }
            return response;
        }

        private void LogCandidateInformation(CandidatesResponse result, Candidate candidate)
        {
            try
            {
                if (_sendToEmail)
                {
                    _emailService.SendEmail(CandidateEmailCreator.CreateEmailContent(Resource.CreateUserProcessMessage + result.Message, new List<Candidate>() { candidate }), "Candidate Result", "max.madrigal@careerbuilder.com");
                    return;
                }
                if (result.Acknowledgement)
                {
                    _logger.Info("Candidate Process: " + result.Message + " candidate Info " + Convert.ToString(candidate));
                }
                else
                {
                    _logger.Error("Candidate Process: " + result.Message + " candidate Info " + Convert.ToString(candidate));
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return;
            }
        }
    }
}
