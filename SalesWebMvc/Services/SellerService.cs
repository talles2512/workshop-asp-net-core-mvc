using SalesWebMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SalesWebMvc.Services
{
    public class SellerService //Classe de Serviço de Vendedor
    {
        private readonly SalesWebMvcContext _context; //Injeção de dependência da classe de conexão com o banco

        public SellerService(SalesWebMvcContext context)
        {
            _context = context;
        }

        public List<Seller> FindAll() //Listando todos os vendedores
        {
            return _context.Seller.ToList(); //Nota-se aqui que: através do objeto de conexão com o banco, acionando a classe vendedor,                                
        }                                    // é possivel listar todos os vendedores do banco

        public void Insert(Seller obj)
        {
            _context.Add(obj); //adicionando um novo Vendedor
            _context.SaveChanges(); //confirmando a operação
        }

        public Seller FindById(int id) //Buscando por id
        {
            return _context.Seller.Include(obj => obj.Department).FirstOrDefault(obj => obj.Id == id); //FirstOrDefault tenta encontrar o primeiro registro conforme uma condição
        }

        public void Remove(int id)
        {
            var obj = _context.Seller.Find(id); //procurando um vendedor pelo id no banco de dados e instanciando ele
            _context.Seller.Remove(obj); //removendo-o do banco
            _context.SaveChanges(); //confirmando a operação
        }
    }
}
