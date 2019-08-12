using SalesWebMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Services.Exceptions;

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

        public void Update(Seller obj)
        {
            if(!_context.Seller.Any(x => x.Id == obj.Id)) //Procura uma ocorrência que tenha o mesmo Id, ou seja, verifica o registro a ser alterado existe
            {
                throw new NotFoundException("Id not found"); //Lança a exceção referente a não ocorrencia de registro
            }
            try
            {
                _context.Update(obj); //Atualiza o registro de Vendedor passando como parametro o Vendedor alterado
                _context.SaveChanges(); //confirmando a operação
            }
            catch(DbUpdateConcurrencyException e) //Exceção do nivel de acesso a dados
            {
                throw new DbConcurrencyException(e.Message); //Lançando uma exceção para ser tratada no nível de serviços
            }
        }
    }
}
