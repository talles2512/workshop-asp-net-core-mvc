using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SalesWebMvc.Services;
using SalesWebMvc.Models;
using SalesWebMvc.Models.ViewModels;

namespace SalesWebMvc.Controllers
{
    public class SellersController : Controller
    {
        private readonly SellerService _sellerService; //Realizando as injeções de dependência
        private readonly DepartmentService _departmentService;

        public SellersController(SellerService sellerService, DepartmentService departmentService)
        {
            _sellerService = sellerService; 
            _departmentService = departmentService;
        }

        public IActionResult Index()
        {
            var list = _sellerService.FindAll(); //Acessando o Serviço de Vendedores para trazer os dados do Banco e inserindo-os em uma lista
            return View(list); //iniciando a View Index jogando a lista de vendedores como argumento
        }

        public IActionResult Create()
        {
            var departments = _departmentService.FindAll(); //Acessando o Serviço de Departamentos para trazer os dados do Banco inserindo-os em uma lista
            var viewModel = new SellerFormViewModel { Departments = departments }; //instanciando um Objeto tipo Vendedor e passando a lista de departamentos joga-los no botão select futuramente
            return View(viewModel); //iniciando a View Create jogando o objeto que tem os dados do vendedor e da lista de departamentos
        }

        [HttpPost] //Indica que é um metodo de postagem
        [ValidateAntiForgeryToken] //Proteção para ataques Forgery Token
        public IActionResult Create(Seller seller) //Metodo de postagem e inserção dos dados preenchidos no formulário/view Create
        {
            _sellerService.Insert(seller); //Jogando os dados 
            return RedirectToAction(nameof(Index)); //retornando da View Create para a View Index
        }

        public IActionResult Delete(int? id) //parametro nullable, podendo ser nulo
        {
            if(id == null) //Caso a busca dê algum tipo de problema
            {
                return NotFound(); //View NotFound
            }

            var obj = _sellerService.FindById(id.Value); // é necessário o .Value para resgatar o valor, pois o argumento pode ser nulo
            if(obj == null) //Caso o retorno do FindById não encontre nenhum registro
            {
                return NotFound();
            }

            return View(obj); //iniciando a View Delete jogando o objeto 
        }

        [HttpPost] //Indica que é um metodo de postagem
        [ValidateAntiForgeryToken] //Proteção para ataques Forgery Token
        public IActionResult Delete(int id) //Metodo de postagem e deleção do registro conforme o id
        {
            _sellerService.Remove(id); //acionando o metódo do Serviço de Vendedor para remover o registro
            return RedirectToAction(nameof(Index)); //Retornando para a View Index
        }

        public IActionResult Details(int? id)
        {
            if (id == null) //Caso a busca dê algum tipo de problema
            {
                return NotFound(); //View NotFound
            }

            var obj = _sellerService.FindById(id.Value); // é necessário o .Value para resgatar o valor, pois o argumento pode ser nulo
            if (obj == null) //Caso o retorno do FindById não encontre nenhum registro
            {
                return NotFound();
            }

            return View(obj); //iniciando a View Details jogando o objeto 
        }
    }
}