using DockerMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using System.Diagnostics;
using System.Reflection;

namespace DockerMVC.Controllers
{

    public class HomeController : Controller
    {

        private readonly EmployeeContext _dbContext;
        public HomeController(EmployeeContext dbcontext)  { _dbContext = dbcontext; }
        

        //public IActionResult Listar()
        //{
        //    //return View(_dbContext.Employees.Where(e => e.Nombre.Contains("DA") || e.Nombre.Contains("MU")).OrderBy(e=>e.Nombre));
        //    return View(_dbContext.Employees.OrderBy(e => e.Nombre));
        //}

        //[HttpPost]
        public IActionResult Listar(string filtro="")
        {
            return View(_dbContext.Employees.Where(e => e.Nombre.Contains(filtro)).OrderBy(e => e.Nombre));
        }

        public IActionResult Crear()
        {
            return View(new Employee() { Ingreso = System.DateTime.Now }); ;
        }

        [HttpPost]
        public IActionResult Crear(Employee empleado)
        {
            _dbContext.Employees.Add(empleado);
            _dbContext.SaveChanges();
            return RedirectToAction("Listar");
        }
        public IActionResult Eliminar(int id)
        {
            return View(_dbContext.Employees.FirstOrDefault(e => e.Id == id)); 
        }

        [HttpPost]
        public IActionResult Eliminar(Employee empleado)
        {
            _dbContext.Employees.Remove(empleado);
            _dbContext.SaveChanges();
            return RedirectToAction("Listar");
        }

        public IActionResult Detalle(int id)
        {
            return View(_dbContext.Employees.FirstOrDefault(e => e.Id == id));
        }

        public IActionResult Editar(int id)
        {
            return View(_dbContext.Employees.FirstOrDefault(e => e.Id == id));
        }
        [HttpPost]
        public IActionResult Editar(Employee empleado)
        {
            Employee _empleado = _dbContext.Employees.FirstOrDefault(e => e.Id == empleado.Id);

            if (_empleado != null)
            {

                //_empleado.Nombre = empleado.Nombre;
                //_empleado.DNI = empleado.DNI;
                //_empleado.Edad = empleado.Edad;
                //_empleado.Telefono = empleado.Telefono;
                //_empleado.Correo = empleado.Correo;
                //_empleado.Basico = empleado.Basico;
                //_empleado.Ingreso = empleado.Ingreso;
                //_empleado.Activo = empleado.Activo;

                foreach (PropertyInfo prop in empleado.GetType().GetProperties())  {
                    prop.SetValue(_empleado, (empleado.GetType().GetProperty(prop.Name)).GetValue(empleado, null), null); }

                _dbContext.SaveChanges();
                return RedirectToAction("Listar");
            }
            return View();
        }


        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}