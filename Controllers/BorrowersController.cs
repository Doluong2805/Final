using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Final.Data;
using Final.Models;
using Final.Service;
using System.Threading.Tasks.Dataflow;

namespace Final.Controllers
{
    public class BorrowersController : Controller
    {
        private readonly IBorrowerServices _borrowerServices;

        public BorrowersController(IBorrowerServices borrowerServices)
        {
            _borrowerServices = borrowerServices;
        }

        // GET: Borrowers
        public async Task<IActionResult> Index(string? name,int? pageSize,int? pageNumber)
        {
            ViewData["Name"] = name;
            var itemquery = _borrowerServices.GetAll();

            if(name != null)
            {
                itemquery = _borrowerServices.Search(itemquery,name);
            }

            if(!Convert.ToBoolean(pageNumber))
            {
                pageNumber = 1;
            }

            if(!Convert.ToBoolean(pageSize))
            {
                pageSize = 2;
            }

            ViewData["TotalPages"] = _borrowerServices.CountPage(itemquery,(int)pageSize);
            ViewData["CurrentPage"] = (int)pageNumber;

            itemquery = _borrowerServices.Pagination(itemquery,(int)pageNumber,(int)pageSize);
            return View(itemquery);
        }


        // GET: Borrowers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var borrower = await _borrowerServices.GetByIdAsync((int)id);
            if(id == null || borrower == null)
            {
                return NotFound();
            }
            return View(borrower);
        }

        // GET: Borrowers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Borrowers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Borrower borrower)
        {
            if(ModelState.IsValid)
            {
                _borrowerServices.Create(borrower);
                return RedirectToAction(nameof(Index));
            }
            return View(borrower);
        }

        // GET: Borrowers/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var borrower = await _borrowerServices.GetByIdAsync((int)id);
            if(id == null || borrower == null)
            {
                return NotFound();
            }

            return View(borrower);
        }

        // POST: Borrowers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,Borrower borrower)
        {
            if(id != borrower.Id)
            {
                return NotFound();
            }

            if(ModelState.IsValid)
            {
                try
                {
                    _borrowerServices.Update(borrower);
                }
                catch(DbUpdateConcurrencyException)
                {
                    if(!BorrowerExists(borrower.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(borrower);
        }

        // GET: Borrowers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var borrower = await _borrowerServices.GetByIdAsync((int)id);
            if(id == null || borrower == null)
            {
                return NotFound();
            }

            return View(borrower);
        }

        // POST: Borrowers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var borrower = await _borrowerServices.GetByIdAsync(id);
                if(borrower == null)
                {
                    return Problem("Entity set 'ApplicationDbContext.Borrowers'  is null.");
                }

                if(borrower != null)
                {
                    _borrowerServices.Delete(id);
                }

                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private bool BorrowerExists(int id)
        {
            return (_borrowerServices.GetAll()?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        //[HttpGet]
        //public JsonResult IsLibraryCardNumberExist(int Id,string LibraryCardNumber)
        //{
        //    // Tìm người mượn có cùng LibraryCardNumber
        //    var existingBorrower = _borrowerServices.GetAll().FirstOrDefault(b => b.LibraryCardNumber == LibraryCardNumber);

        //    if(existingBorrower == null || existingBorrower.Id == Id)
        //    {
        //        // Trường hợp này cho phép sửa vì không có người mượn nào có LibraryCardNumber này
        //        // hoặc nếu có, thì đó là chính người mượn đang sửa thông tin của họ
        //        return Json(true);
        //    }
        //    else
        //    {
        //        // Trường hợp này không cho phép sửa vì đã tồn tại một người mượn khác có LibraryCardNumber tương tự
        //        return Json(false);
        //    }
        //}

        public IActionResult Search(string name)
        {
            return RedirectToAction("Index","Borrowers",new { name });
        }
    }
}
