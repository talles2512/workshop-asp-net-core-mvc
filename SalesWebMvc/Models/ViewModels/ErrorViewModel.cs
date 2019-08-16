using System;

namespace SalesWebMvc.Models.ViewModels
{
    public class ErrorViewModel //Esta classe � instanciada na View Error, que � compartilhada por todo o sistema
    {                           //
        public string RequestId { get; set; } //Id interno da requisi��o
        public string Message { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId); //Retornar se o Id existe
    }
}