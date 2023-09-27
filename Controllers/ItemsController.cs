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

namespace Final.Controllers
{
    public class ItemsController : Controller
    {
        private readonly IItemServices _services;

        public ItemsController(IItemServices itemServices)
        {
            _services = itemServices;
        }

        // GET: Items
        public async Task<IActionResult> Index(string? title, int? pageSize, int? pageNumber)
        {
            ViewData["Title"] = title;
            var itemquery = _services.GetAll();
            //if(!string.IsNullOrWhiteSpace(title))
            //{
                itemquery = _services.Search(itemquery,title);
            //}
            if(!Convert.ToBoolean(pageNumber))
            {
                pageNumber = 1;
            }

            if(!Convert.ToBoolean(pageSize))
            {
                pageSize = 5;
            }

            ViewData["TotalPages"] = _services.CountPage(itemquery, (int)pageSize);
            ViewData["CurrentPage"] = (int)pageNumber;            

            itemquery = _services.Pagination(itemquery,(int)pageNumber,(int)pageSize);
            return View(itemquery);
        }

        public JsonResult ItemListJson()
        {
            var list = _services.GetAll().ToList();
            return Json(list);
        }

        // GET: Items/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            var item = await _services.GetByIdAsync((int)id);
            if(id == null || item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // GET: Items/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Items/Create
        [HttpPost]
        [ActionName("Create")]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Item item)
        {
            item.AvailableQuantity = item.Quantity;

            if(ModelState.IsValid)
            {
                _services.Create(item);
                return RedirectToAction(nameof(Index));
            }
            return View(item);
        }

        // GET: Items/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            var item = await _services.GetByIdAsync((int)id);
            if(id == null || item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // POST: Items/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id,Item item)
        {
            if(id != item.Id)
            {
                return NotFound();
            }

            if(ModelState.IsValid)
            {
                // Lấy phiên bản hiện tại của Item từ cơ sở dữ liệu
                var existingItem = _services.GetById(id);

                if(existingItem == null)
                {
                    return NotFound();
                }

                // Sao chép dữ liệu từ item chỉnh sửa vào existingItem
                existingItem.Type = item.Type;
                existingItem.Title = item.Title;
                existingItem.Author = item.Author;
                existingItem.PublicationDate = item.PublicationDate;
                existingItem.Price = item.Price;
                existingItem.Quantity = existingItem.Quantity;
                existingItem.NumberOfPages = item.NumberOfPages;
                existingItem.RunTime = item.RunTime;

                _services.Update(existingItem);

                return RedirectToAction(nameof(Index));
            }
            return View(item);
        }

        // GET: Items/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            var item = await _services.GetByIdAsync((int)id);
            if(id == null || item == null)
            {
                return NotFound();
            }

            if(item == null)
            {
                return NotFound();
            }

            return View(item);
        }

        // POST: Items/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var item = await _services.GetByIdAsync(id);
                if(item == null)
                {
                    return Problem("Entity set 'ApplicationDbContext.Item'  is null.");
                }

                if(item != null)
                {
                    _services.Delete(id);
                }

                return RedirectToAction(nameof(Index));
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        public IActionResult Search(string title)
        {
            return RedirectToAction("Index","Items",new { title });
        }

        public IActionResult Add(int? id)
        {
            var item = _services.GetAll().FirstOrDefault(m => m.Id == id);
            if(id == null || item == null)
            {
                return NotFound();
            }
            return RedirectToAction("Index","Home");
        }
    }
}
