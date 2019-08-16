using System;

namespace SalesWebMvc.Services.Exceptions
{
    public class DbConcurrencyException : ApplicationException //Criando uma exceção personalizada para erros de concorrencia no banco
    {
        public DbConcurrencyException(string message): base(message)
        {

        }
    }
}
