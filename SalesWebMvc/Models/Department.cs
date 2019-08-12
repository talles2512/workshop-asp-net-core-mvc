using System.Collections.Generic;
using System;
using System.Linq;

namespace SalesWebMvc.Models
{
    public class Department //Classe do Departamento
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ICollection<Seller> Sellers { get; set; } = new List<Seller>(); //Lista de Vendedores

        public Department()
        {

        }

        public Department(int id, string name)
        {
            Id = id;
            Name = name;
        }

        public void AddSeller(Seller seller) //Adicionar Vendedor à lista de vendedores do Departamento
        {
            Sellers.Add(seller);
        }

        public double TotalSales(DateTime initial, DateTime final) //Calcular o total das vendas de todos os vendedores dentro de um escopo de data
        {
            return Sellers.Sum(seller => seller.TotalSales(initial, final));
        }
    }
}
