using Amazon.Lambda.Core;
using LambdaFunction.Converter;
using LambdaFunction.Models;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace LambdaFunction
{
    public class Function
    {
        private static readonly HttpClient client = new HttpClient();
        private readonly ILambdaLogger _lambdaLogger;
        private const string _host = "https://viacep.com.br";

        public Function(ILambdaLogger lambdaLogger)
        {
            _lambdaLogger = lambdaLogger;
        }

        public async Task<FullResponse> FunctionHandlerAsync(string zipCode, ILambdaContext context)
        {
            try
            {
                if (!string.IsNullOrEmpty(zipCode))
                {
                    var streamTask = await client.GetStreamAsync($"{_host}/ws/{zipCode}/json/");
                    var address = await JsonSerializer.DeserializeAsync<Address>(streamTask);
                    return FunctionConverter.ToFullResponse(true, address);
                }
                return FunctionConverter.ToFullResponse(false);
            }
            catch (Exception ex)
            {
                _lambdaLogger.Log(ex.Message);

                return ex.Message.ToExceptionResponse(false);                
            }
        }
    }
}
