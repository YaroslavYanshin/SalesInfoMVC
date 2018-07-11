using BL;
using SalesInfoMVC.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace SalesInfoMVC.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        // GET: Home
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Sales()
        {
            var repositoryTransfer = new RepositoryTransfer();
            var salesDTO = repositoryTransfer.GetSales();
            var sales = salesDTO.Select(s => new SaleInfo() { Id = s.Id, Date = s.Date, Manager = s.Manager, Client = s.Client, Product = s.Product, Amount = s.Amount }).ToArray();

            return View(sales);
        }

        public ActionResult SalesList()
        {
            var repositoryTransfer = new RepositoryTransfer();
            var salesDTO = repositoryTransfer.GetSales();
            var sales = salesDTO.Select(s => new SaleInfo() { Id = s.Id, Date = s.Date, Manager = s.Manager, Client = s.Client, Product = s.Product, Amount = s.Amount }).ToArray();

            return PartialView("PartialSalesList", sales);
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult SaleEdit(int id)
        {
            var repositoryTransfer = new RepositoryTransfer();
            var saleDTO = repositoryTransfer.GetSales().FirstOrDefault(s => (s.Id == id));
            var sale = new SaleInfo() { Id = saleDTO.Id, Date = saleDTO.Date, Manager = saleDTO.Manager, Client = saleDTO.Client, Product = saleDTO.Product, Amount = saleDTO.Amount };

            return PartialView("SaleEdit", sale);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult SaleEdit(SaleInfo sale)
        {
            TryValidateModel(sale);
            if (ModelState.IsValid)
            {
                var repositoryTransfer = new RepositoryTransfer();
                var saleDTO = repositoryTransfer.GetSales().FirstOrDefault(s => (s.Id == sale.Id));
                saleDTO.Date = sale.Date;
                saleDTO.Manager = sale.Manager;
                saleDTO.Client = sale.Client;
                saleDTO.Product = sale.Product;
                saleDTO.Amount = sale.Amount;
                repositoryTransfer.UpdateSaleInfo(saleDTO);

                if (Request.IsAjaxRequest())
                {
                    return new EmptyResult();
                }
                else
                {
                    return View("Index");
                }
            }
            else
            {
                return View("Error");
            }
        }

        public ActionResult Managers()
        {
            var repositoryTransfer = new RepositoryTransfer();
            var managersDTO = repositoryTransfer.GetManagers();
            var managers = managersDTO.Select(m => new Manager() { Id = m.Id, SecondName = m.SecondName }).ToArray();

            return View(managers);
        }

        public ActionResult ManagerList()
        {
            var repositoryTransfer = new RepositoryTransfer();
            var managersDTO = repositoryTransfer.GetManagers();
            var managers = managersDTO.Select(m => new Manager() { Id = m.Id, SecondName = m.SecondName }).ToArray();

            return PartialView("PartialManagersList", managers);
        }

        [HttpGet]
        [Authorize(Roles = "admin")]
        public ActionResult ManagerEdit(int id)
        {
            var repositoryTransfer = new RepositoryTransfer();
            var managerDTO = repositoryTransfer.GetManagers().FirstOrDefault(m => (m.Id == id));
            var manager = new Manager() { Id = managerDTO.Id, SecondName = managerDTO.SecondName };

            return PartialView("ManagerEdit", manager);
        }

        [HttpPost]
        [Authorize(Roles = "admin")]
        public ActionResult ManagerEdit(Manager manager)
        {
            TryValidateModel(manager);
            if (ModelState.IsValid)
            {
                var repositoryTransfer = new RepositoryTransfer();
                var existManagerDTO = repositoryTransfer.GetManagers().FirstOrDefault(m => (m.SecondName == manager.SecondName));
                if (existManagerDTO == null)
                {
                    var managerDTO = repositoryTransfer.GetManagers().FirstOrDefault(m => (m.Id == manager.Id));
                    managerDTO.SecondName = manager.SecondName;
                    repositoryTransfer.UpdateManager(managerDTO);

                    if (Request.IsAjaxRequest())
                    {
                        return new EmptyResult();
                    }
                    else
                    {
                        return View("Index");
                    }
                }
                else
                {
                    return View("Error");
                }
            }
            else
            {
                return View("Error");
            }
        }

        public ActionResult ShowManager(int id)
        {
            var repositoryTransfer = new RepositoryTransfer();
            var managers = repositoryTransfer.GetManagers().Where(m => (m.Id == id)).Select(m => (new Manager() { Id = m.Id, SecondName = m.SecondName })).ToArray();

            return PartialView("PartialManagersList", managers);
        }

        public ActionResult ShowSDate(DateTime saleDate)
        {
            var repositoryTransfer = new RepositoryTransfer();
            var sales = repositoryTransfer.GetSales().Where(s => (s.Date == saleDate)).Select(s => (new SaleInfo() { Id = s.Id, Date = s.Date, Manager = s.Manager, Client = s.Client, Product = s.Product, Amount = s.Amount })).ToArray();

            return PartialView("PartialSalesList", sales);
        }

        public ActionResult ShowSManager(string saleManager)
        {
            var repositoryTransfer = new RepositoryTransfer();
            var sales = repositoryTransfer.GetSales().Where(s => (s.Manager == saleManager)).Select(m => (new SaleInfo() { Id = m.Id, Date = m.Date, Manager = m.Manager, Client = m.Client, Product = m.Product, Amount = m.Amount })).ToArray();

            return PartialView("PartialSalesList", sales);
        }

        public ActionResult ShowSClient(string saleClient)
        {
            var repositoryTransfer = new RepositoryTransfer();
            var sales = repositoryTransfer.GetSales().Where(s => (s.Client == saleClient)).Select(m => (new SaleInfo() { Id = m.Id, Date = m.Date, Manager = m.Manager, Client = m.Client, Product = m.Product, Amount = m.Amount })).ToArray();

            return PartialView("PartialSalesList", sales);
        }

        public ActionResult ShowSProduct(string saleProduct)
        {
            var repositoryTransfer = new RepositoryTransfer();
            var sales = repositoryTransfer.GetSales().Where(s => (s.Product == saleProduct)).Select(m => (new SaleInfo() { Id = m.Id, Date = m.Date, Manager = m.Manager, Client = m.Client, Product = m.Product, Amount = m.Amount })).ToArray();

            return PartialView("PartialSalesList", sales);
        }

        [HttpGet]
        public JsonResult GetManagersChartData()
        {
            var repositoryTransfer = new RepositoryTransfer();

            var sales = repositoryTransfer.GetSales()
                                          .GroupBy(s => s.Manager)
                                          .Select(m => new object[] { m.Key, m.Sum(x => x.Amount) })
                                          .ToArray();

            return Json(sales, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ManagersChart()
        {
            return View();
        }
    }
}