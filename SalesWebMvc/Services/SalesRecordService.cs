using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SalesWebMvc.Models;
using Microsoft.EntityFrameworkCore;

namespace SalesWebMvc.Services
{
    public class SalesRecordService
    {
        private readonly SalesWebMvcContext _context; //Injeção de dependência da classe de conexão com o banco

        public SalesRecordService(SalesWebMvcContext context)
        {
            _context = context;
        }
        
        public async Task<List<SalesRecord>> FindByDateAsync(DateTime? minDate, DateTime? maxDate)
        {
            var result = from obj in _context.SalesRecord select obj;
            if (minDate.HasValue)
            {
                result = result.Where(x => x.Date >= minDate.Value);
            }
            if (maxDate.HasValue)
            {
                result = result.Where(x => x.Date <= maxDate.Value);
            }
            return await result
                .Include(x => x.Seller) //Join com a tabela Vendedores
                .Include(x => x.Seller.Department) //Join com a tabela Departamento
                .OrderByDescending(x => x.Date) //Ordenando decrescentemente pela data
                .ToListAsync();
        }

        public async Task<List<IGrouping<Department,SalesRecord>>> FindByDateGroupingAsync(DateTime? minDate, DateTime? maxDate)
        {
            var result = from obj in _context.SalesRecord select obj;
            if (minDate.HasValue)
            {
                result = result.Where(x => x.Date >= minDate.Value);
            }
            if (maxDate.HasValue)
            {
                result = result.Where(x => x.Date <= maxDate.Value);
            }
            return await result
                .Include(x => x.Seller) //Join com a tabela Vendedores
                .Include(x => x.Seller.Department) //Join com a tabela Departamento
                .OrderByDescending(x => x.Date) //Ordenando decrescentemente pela data
                .GroupBy(x => x.Seller.Department)
                .ToListAsync();
        }
    }
}
