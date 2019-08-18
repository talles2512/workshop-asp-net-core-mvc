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

        public async Task<List<Seller>> FindAllAsync() //Listando todos os vendedores ////Agora é async
        {                                            //O nome do método foi alterado para estar de acordo com as padronizações do C#
            return await _context.Seller.ToListAsync(); //Nota-se aqui que: através do objeto de conexão com o banco, acionando a classe vendedor,                                
        }                                    // é possivel listar todos os vendedores do banco

        public async Task InsertAsync(Seller obj) ////Agora é async
        {
            _context.Add(obj); //adicionando um novo Vendedor
            await _context.SaveChangesAsync(); //confirmando a operação
        }

        public async Task<Seller> FindByIdAsync(int id) //Buscando por id ////Agora é async
        {
            return await _context.Seller.Include(obj => obj.Department).FirstOrDefaultAsync(obj => obj.Id == id); //FirstOrDefault tenta encontrar o primeiro registro conforme uma condição
        }

        public async Task RemoveAsync(int id) ////Agora é async
        {
            try
            {
                var obj = await _context.Seller.FindAsync(id); //procurando um vendedor pelo id no banco de dados e instanciando ele
                _context.Seller.Remove(obj); //removendo-o do banco
                await _context.SaveChangesAsync(); //confirmando a operação
            }
            catch (DbUpdateException e)
            {
                throw new IntegrityException("Can't delete seller because he/she has sales");
            }
        }

        public async Task UpdateAsync(Seller obj) ////Agora é async
        {
            bool hasAny = await _context.Seller.AnyAsync(x => x.Id == obj.Id);
            if(!hasAny) //Procura uma ocorrência que tenha o mesmo Id, ou seja, verifica o registro a ser alterado existe
            {
                throw new NotFoundException("Id not found"); //Lança a exceção referente a não ocorrencia de registro
            }
            try
            {
                _context.Update(obj); //Atualiza o registro de Vendedor passando como parametro o Vendedor alterado
                await _context.SaveChangesAsync(); //confirmando a operação
            }
            catch(DbUpdateConcurrencyException e) //Exceção do nivel de acesso a dados
            {
                throw new DbConcurrencyException(e.Message); //Lançando uma exceção para ser tratada no nível de serviços
            }
        }
    }
}
