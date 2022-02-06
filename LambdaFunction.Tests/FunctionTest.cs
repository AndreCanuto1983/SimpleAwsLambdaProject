using Amazon.Lambda.Core;
using Amazon.Lambda.TestUtilities;
using LambdaFunction;
using Moq;
using Xunit;

namespace ExemploFuncaoLambda.Tests
{
    public class FunctionTest
    {
        [Fact]
        public void OnceAValidZipCodeIsEntered_ValidateData()
        {
            var loggerMock = new Mock<ILambdaLogger>();

            var function = new Function(loggerMock.Object);

            var context = new TestLambdaContext();

            var response = function.FunctionHandlerAsync("04011001", context).Result;

            Assert.True(response.Success);
            Assert.NotNull(response.Address.Logradouro);
            Assert.NotNull(response.Address.Localidade);
            Assert.NotNull(response.Address.UF);
            Assert.NotNull(response.Address.Ibge);
            Assert.NotNull(response.Address.Cep);
            Assert.NotNull(response.Address.Bairro);
            Assert.NotNull(response.Address.DDD);
        }

        [Fact]
        public void OnceAInvalidZipCodeIsEntered_ResponseValidate()
        {
            var loggerMock = new Mock<ILambdaLogger>();

            var function = new Function(loggerMock.Object);

            var context = new TestLambdaContext();

            var response = function.FunctionHandlerAsync("14810311", context).Result;

            Assert.True(response.Success);
            Assert.Null(response.Address.Logradouro);
            Assert.Null(response.Address.Localidade);
            Assert.Null(response.Address.UF);
            Assert.Null(response.Address.Ibge);
            Assert.Null(response.Address.Cep);
            Assert.Null(response.Address.Bairro);
            Assert.Null(response.Address.DDD);
        }

        [Fact]
        public void ZIPCodeNonExistent_ResponseValidate()
        {
            var loggerMock = new Mock<ILambdaLogger>();

            var function = new Function(loggerMock.Object);

            var context = new TestLambdaContext();

            var response = function.FunctionHandlerAsync("", context).Result;

            Assert.False(response.Success);
            Assert.Null(response.Address);
        }

        [Fact]
        public void NullRequest_ResponseValidate()
        {
            var loggerMock = new Mock<ILambdaLogger>();            

            var function = new Function(loggerMock.Object);

            var context = new TestLambdaContext();

            var response = function.FunctionHandlerAsync(null, context).Result;

            Assert.False(response.Success);
            Assert.Null(response.Address);
        }

        [Fact]
        public void ForceBadRequestInExternalApi_ResponseValidate()
        {
            var loggerMock = new Mock<ILambdaLogger>();               

            var handler = new Function(loggerMock.Object);

            var context = new TestLambdaContext();

            var response = handler.FunctionHandlerAsync(" " , context).Result;
                        
            Assert.False(response.Success);
            Assert.Equal("Response status code does not indicate success: 400 (Bad Request).", response.Message);
        }
    }
}
