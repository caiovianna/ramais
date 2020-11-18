using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Ramais.AcessoDados;
using Ramais.Models;
namespace Ramais.Controllers
{
    public class RamaisController : Controller
    {
        private RamalContexto db = new RamalContexto();

        // GET: Ramais
        public ActionResult Index()
        {
            return View(db.Ramais.Where(i => !i.Excluido).OrderBy(i => i.Nome).ToList());
        }

        public JsonResult Listar(int current, string searchPhrase, int rowCount = 50)
        {
            var ramais = new List<Ramal>();
            if(String.IsNullOrEmpty(searchPhrase))
            {
                ramais = db.Ramais.Where(i => !i.Excluido).ToList();
            } else
            {
                ramais = db.Ramais.Where(i => !i.Excluido && (i.Nome.Contains(searchPhrase) || i.Matricula.Contains(searchPhrase))).ToList();
            }
            var ramaisPaginados = ramais.OrderBy(i => i.Nome).Skip((current - 1) * rowCount).Take(rowCount);

            var result = new
            {
                rows = ramaisPaginados.ToList(),
                current = current,
                rowCount = rowCount,
                total = ramais.Count()
            };

            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public PartialViewResult Filtrar(string filtro, int pagina, int pageLength = 50)
        {
            var ramais = db.Ramais.Where(i => !i.Excluido && (i.Nome.Contains(filtro) || i.Matricula.Contains(filtro) || i.Setor.Contains(filtro))).ToList();
            var ramaisPaginados = ramais.OrderBy(i => i.Nome).Skip((pagina - 1) * pageLength).Take(pageLength);
            return PartialView("_Listar", ramaisPaginados);
        }

        // GET: Ramais/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ramal ramal = db.Ramais.Find(id);
            if (ramal == null)
            {
                return HttpNotFound();
            }
            return View(ramal);
        }

        // GET: Ramais/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Ramais/Create
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Nome,Numero,Setor,Matricula,UsuarioCriacao,UsuarioAlteracao,Excluido,DataCriacao,DataAlteracao")] Ramal ramal)
        {
            ramal.UsuarioCriacao = User.Identity.Name;
            ramal.UsuarioAlteracao = User.Identity.Name;
            ramal.Excluido = false;
            ramal.DataCriacao = DateTime.Now;
            ramal.DataAlteracao = DateTime.Now;

            if (ModelState.IsValid)
            {
                db.Ramais.Add(ramal);
                Auditoria auditoria = new Auditoria();
                auditoria.Acao = "criou";
                auditoria.Data = DateTime.Now;
                auditoria.Ramal = ramal;
                auditoria.Usuario = User.Identity.Name;
                db.Auditoria.Add(auditoria);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }

            return View(ramal);
        }

        // GET: Ramais/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ramal ramal = db.Ramais.Find(id);
            if (ramal == null)
            {
                return HttpNotFound();
            }
            return View(ramal);
        }

        // POST: Ramais/Edit/5
        // Para se proteger de mais ataques, ative as propriedades específicas a que você quer se conectar. Para 
        // obter mais detalhes, consulte https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Nome,Numero,Setor,Matricula,UsuarioCriacao,UsuarioAlteracao,Excluido,DataCriacao,DataAlteracao")] Ramal ramal)
        {
            ramal.UsuarioAlteracao = User.Identity.Name;
            ramal.DataAlteracao = DateTime.Now;
            if (ModelState.IsValid)
            {
                db.Entry(ramal).State = EntityState.Modified;
                db.SaveChanges();

                Ramal ramalAnterior = db.Ramais.Find(ramal.Id);
                Auditoria auditoria = new Auditoria();
                auditoria.Acao = "editou";
                auditoria.NomeAnterior = ramalAnterior.Nome;
                auditoria.NomeNovo = ramal.Nome;
                auditoria.NumeroAnterior = ramalAnterior.Numero;
                auditoria.NumeroNovo = ramal.Numero;
                auditoria.Data = DateTime.Now;
                auditoria.Ramal = ramal;
                auditoria.Usuario = User.Identity.Name;

                db.Auditoria.Add(auditoria);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            return View(ramal);
        }

        // GET: Ramais/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Ramal ramal = db.Ramais.Find(id);
            if (ramal == null)
            {
                return HttpNotFound();
            }
            return View(ramal);
        }

        // POST: Ramais/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Ramal ramal = db.Ramais.Find(id);
            ramal.Excluido = true;
            ramal.UsuarioAlteracao = User.Identity.Name;
            ramal.DataAlteracao = DateTime.Now;
            db.SaveChanges();
            return RedirectToAction("Index", "Home");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
