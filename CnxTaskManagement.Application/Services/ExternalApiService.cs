using CnxTaskManagement.Application.Common;
using CnxTaskManagement.Application.Common.Interfaces;
using CnxTaskManagement.Application.Common.Utils;
using CnxTaskManagement.Application.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace CnxTaskManagement.Application.Services
{
    public class ExternalApiService : IExternalApiService
    {
        private readonly IConfiguration _configuration;
        private readonly IApiService _apiService;
        public ExternalApiService(IConfiguration configuration, IApiService apiService)
        {
            _apiService = apiService;
            _configuration = configuration;
        }

        public async Task<string> FindBadWord(string keyword)
        {
            var apiUrl = "https://api.apilayer.com/bad_words?censor_character=";
            string? apiKey = _configuration["BadwordApiKey"];
            if (apiKey == null)
                throw new Exception("Badword Api key is invalid");

            var response = await _apiService.SendRequestAsync(apiUrl, apiKey, HttpMethod.Post,keyword);
            var result = JsonSerializer.Deserialize<BadWord>(response);
            if (result == null)
                throw new Exception("Can't find badword");

            return result.censored_content.Replace("\"","");
        }
    }
}
