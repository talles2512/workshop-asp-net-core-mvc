using System;

namespace SalesWebMvc.Services.Exceptions
{
    public class NotFoundException : ApplicationException //Criando uma exceção personalizada para erros de registro não encontrado
    {
        public NotFoundException(string message): base(message)
        {

        }
    }
}
