using System;

namespace SalesWebMvc.Models.ViewModels
{
    public class ErrorViewModel //Esta classe é instanciada na View Error, que é compartilhada por todo o sistema
    {                           //
        public string RequestId { get; set; } //Id interno da requisição
        public string Message { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId); //Retornar se o Id existe
    }
}