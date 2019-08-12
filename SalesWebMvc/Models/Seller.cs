using System;
using System.Collections.Generic;
using System.Linq;

namespace SalesWebMvc.Models
{
    public class Seller //Classe do Vendedor
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
        public double BaseSalary { get; set; }
        public Department Department { get; set; }
        public int DepartmentId { get; set; }
        public ICollection<SalesRecord> Sales { get; set; } = new List<SalesRecord>(); //Lista de Registro de Vendas do Vendedor

        public Seller()
        {

        }

        public Seller(int id, string name, string email, DateTime birthDate, double baseSalary, Department department)
        {
            Id = id;
            Name = name;
            Email = email;
            BirthDate = birthDate;
            BaseSalary = baseSalary;
            Department = department;
        }

        public void AddSales(SalesRecord sr) //Adicionar um registro de venda
        {
            Sales.Add(sr);
        }

        public void RemoveSales(SalesRecord sr) //Remover um registro de venda
        {
            Sales.Remove(sr);
        }

        public double TotalSales(DateTime initial, DateTime final) //Calcular o total de vendas do vendedor pela lista de registro de vendas dentro do escopo de data
        {
            return Sales.Where(sr => sr.Date >= initial && sr.Date <= final).Sum(sr => sr.Amount);
        }
    }
}
