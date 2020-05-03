using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using AutoLotDAL.EF;
using AutoLotDAL.Models;
using AutoLotDAL.Repos;

namespace CarLotMVC.Controllers
{
    public class InventoryController : Controller
    {
        private readonly inventoryRepo repo = new inventoryRepo();

        // GET: Inventory
        public ActionResult Index()
        {
            return View(model: this.repo.GetAll());
        }

        // GET: Inventory/Details/5
        public ActionResult Details(Int32? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(statusCode: HttpStatusCode.BadRequest);
            }

            inventory inventory = this.repo.GetOne(id: id);
            if (inventory == null)
            {
                return HttpNotFound();
            }

            return View(model: inventory);
        }

        // GET: Inventory/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Inventory/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Make,Color,PetName")] inventory inventory)
        {
            if (!ModelState.IsValid) return View(model: inventory);
            try
            {
                this.repo.Add(entity: inventory);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(
                    key: String.Empty,
                    errorMessage: $@"Unable to create record: {ex.Message}");
                // He удается создать запись.
                return View(model: inventory);
            }

            return RedirectToAction(actionName: "Index");
        }

        // GET: Inventory/Edit/5
        public ActionResult Edit(Int32? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(statusCode: HttpStatusCode.BadRequest);
            }

            inventory inventory = this.repo.GetOne(id: id);
            if (inventory == null)
            {
                return HttpNotFound();
            }

            return View(model: inventory);
        }

        // POST: Inventory/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Make,Color,PetName,Timestamp")]
            inventory inventory)
        {
            if (!ModelState.IsValid) return View(model: inventory);
            try
            {
                this.repo.Save(entity: inventory);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                ModelState.AddModelError(
                    key: String.Empty,
                    errorMessage: $@"Unable to save the record. Another user has updated it.{ex.Message}"); // He удается сохранить запись. Другой пользователь обновил ее.
                return View(model: inventory);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(
                    key: String.Empty,
                    errorMessage: $@"Unable to save the record.{ex.Message}"); // He удается сохранить запись.
                return View(model: inventory);
            }

            return RedirectToAction(actionName: "Index");
        }

        // GET: Inventory/Delete/5
        public ActionResult Delete(Int32? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(statusCode: HttpStatusCode.BadRequest);
            }

            inventory inventory = this.repo.GetOne(id: id);
            if (inventory == null)
            {
                return HttpNotFound();
            }

            return View(model: inventory);
        }

        // POST: Inventory/Delete/5
        [HttpPost, ActionName(name: "Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed([Bind(Include = "Id,Timestamp")] inventory inventory)
        {
            try
            {
                this.repo.Delete(entity: inventory);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                ModelState.AddModelError(key: String.Empty, errorMessage: $@"Unable to delete record.Another user updated the record. {ex.Message}"); // He удается удалить запись. Другой пользователь обновил ее.}
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(key: String.Empty, errorMessage: $@"Unable to delete record:{ex.Message}"); // He удается удалить запись."
                return RedirectToAction(actionName: "Index");
            }

            return RedirectToAction(actionName: "Index");
        }

        protected override void Dispose(Boolean disposing)
        {
            if (disposing)
            {
                this.repo.Dispose();
            }

            base.Dispose(disposing: disposing);
        }
    }
}