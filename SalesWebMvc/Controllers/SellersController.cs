﻿using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SalesWebMvc.Services;
using SalesWebMvc.Models;
using SalesWebMvc.Models.ViewModels;
using System.Collections.Generic;
using SalesWebMvc.Services.Exceptions;
using System.Diagnostics;

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

        public async Task<IActionResult> Index() ////Agora é async
        {
            var list = await _sellerService.FindAllAsync(); //Acessando o Serviço de Vendedores para trazer os dados do Banco e inserindo-os em uma lista
            return View(list); //iniciando a View Index jogando a lista de vendedores como argumento
        }

        public async Task<IActionResult> Create()
        {
            var departments = await _departmentService.FindAllAsync(); //Acessando o Serviço de Departamentos para trazer os dados do Banco inserindo-os em uma lista
            var viewModel = new SellerFormViewModel { Departments = departments }; //instanciando um Objeto tipo Vendedor e passando a lista de departamentos joga-los no botão select futuramente
            return View(viewModel); //iniciando a View Create jogando o objeto que tem os dados do vendedor e da lista de departamentos
        }

        [HttpPost] //Indica que é um metodo de postagem
        [ValidateAntiForgeryToken] //Proteção para ataques Forgery Token
        public async Task<IActionResult> Create(Seller seller) //Metodo de postagem e inserção dos dados preenchidos no formulário/view Create ////Agora é async
        {
            if (!ModelState.IsValid) //Testa se o modelo é valido
            {
                var departments = await _departmentService.FindAllAsync();
                var viewModel = new SellerFormViewModel
                {
                    Seller = seller,
                    Departments = departments
                };
                return View(viewModel); //Caso não seja, o programa retorna para a mesma tela
            }
            await _sellerService.InsertAsync(seller); //Jogando os dados 
            return RedirectToAction(nameof(Index)); //retornando da View Create para a View Index
        }

        public async Task<IActionResult> Delete(int? id) //parametro nullable, podendo ser nulo ////Agora é async
        {
            if(id == null) //Caso a busca dê algum tipo de problema
            {
                return RedirectToAction(nameof(Error), new { message = "Id not provided"}); //Anteriormente retornada uma view padrão View NotFound
            }   //Agora retorna a view Error com uma mensagem personalizada conforme o contexto do erro: Id não fornecido, neste caso. new {} é um objeto anônimo

            var obj = await _sellerService.FindByIdAsync(id.Value); // é necessário o .Value para resgatar o valor, pois o argumento pode ser nulo
            if(obj == null) //Caso o retorno do FindById não encontre nenhum registro
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found" }); //Id não encontrado
            }

            return View(obj); //iniciando a View Delete jogando o objeto 
        }

        [HttpPost] //Indica que é um metodo de postagem
        [ValidateAntiForgeryToken] //Proteção para ataques Forgery Token
        public async Task<IActionResult> Delete(int id) //Metodo de postagem e deleção do registro conforme o id ////Agora é async
        {
            try
            {
                await _sellerService.RemoveAsync(id); //acionando o metódo do Serviço de Vendedor para remover o registro
                return RedirectToAction(nameof(Index)); //Retornando para a View Index
            }
            catch (IntegrityException e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message });
            }
        }

        public async Task<IActionResult> Details(int? id) ////Agora é async
        {
            if (id == null) //Caso a busca dê algum tipo de problema
            {
                return RedirectToAction(nameof(Error), new { message = "Id not provided" });
            }

            var obj = await _sellerService.FindByIdAsync(id.Value); // é necessário o .Value para resgatar o valor, pois o argumento pode ser nulo
            if (obj == null) //Caso o retorno do FindById não encontre nenhum registro
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found" });
            }

            return View(obj); //iniciando a View Details jogando o objeto 
        }

        public async Task<IActionResult> Edit(int? id) ////Agora é async
        {
            if (id == null) //Caso a busca dê algum tipo de problema
            {
                return RedirectToAction(nameof(Error), new { message = "Id not provided" }); //View NotFound
            }

            var obj = await _sellerService.FindByIdAsync(id.Value);
            if(obj == null)
            {
                return RedirectToAction(nameof(Error), new { message = "Id not found" });
            }

            List<Department> departments = await _departmentService.FindAllAsync();
            SellerFormViewModel viewModel = new SellerFormViewModel { Seller = obj, Departments = departments };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Seller seller) ////Agora é async
        {
            if (!ModelState.IsValid) //Testa se o modelo é valido
            {
                var departments = await _departmentService.FindAllAsync();
                var viewModel = new SellerFormViewModel
                {
                    Seller = seller,
                    Departments = departments
                };
                return View(viewModel); //Caso não seja, o programa retorna para a mesma tela
            }  
            if (id != seller.Id) //Caso o id passado seja diferente do Id do Vendedor que deve ser alterado
            {
                return RedirectToAction(nameof(Error), new { message = "Id mismatch" }); //Anteriormente chamava uma view padrão BadRequest. Erro: Id não corresponde
            }
            try
            {
                await _sellerService.UpdateAsync(seller);
                return RedirectToAction(nameof(Index));
            }
            catch (NotFoundException e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message }); //Instanciando o objeto com a mensagem de erro da NotFoundException
            }
            catch (DbConcurrencyException e)
            {
                return RedirectToAction(nameof(Error), new { message = e.Message }); //Instanciando o objeto com a mensagem de erro da DbConcurrencyException
            }

            //catch (ApplicationException e)           Como as exceções acima herdam de ApplicationException, podemos utilizar o polimorfismo(Upcasting), porém não o fiz para ficar mais visível
            //{
            //    return RedirectToAction(nameof(Error), new { message = e.Message }); //Instanciando o objeto com a mensagem de erro da NotFoundExecption
            //}
        }

        //Todas as operações acima são async por realizarem acesso à dados

        public IActionResult Error(string message) //Trazendo a tela de erro
        {
            var viewModel = new ErrorViewModel //Instanciando o objeto que será apresentado na tela
            {
                Message = message, //Definindo a mensagem do objeto pela mensagem recebida como parâmetro
                RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier //pegando o Id interno da requisição
            }; //Current? é um valor opcional, portanto usamos ?? para, caso não exista um valor, a variável Id receberá o Id do HttpContext
            return View(viewModel);
        }
    }
}