using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Final.Data;
using Final.Models;

namespace Final.Controllers
{
    public class HistoriesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly SessionManager _sessionManager;
        public HistoriesController(ApplicationDbContext context,SessionManager sessionManager)
        {
            _context = context;
            _sessionManager = sessionManager;
        }

        // GET: Histories
        public async Task<IActionResult> Index(int? borrowerId, DateTime? borrowDate, Status? status, int? itemId,int? pageSize,int? pageNumber)
        {
            IQueryable<History> query = _context.History
                .Include(h => h.Borrower)
                .Include(h => h.BorrowItems)
                .ThenInclude(h => h.Item);

            ViewData["BorrowerId"] = new SelectList(_context.Borrowers,"Id", "Name");
            var uniqueStatuses = _context.History.Select(h => h.Status).Distinct().ToList();
            ViewData["Status"] = new SelectList(uniqueStatuses);
            ViewData["ItemId"] = new SelectList(_context.Item,"Id","Title");

            if(borrowerId != null)
            {
                query = query.Where(h => h.BorrowerId == borrowerId);
            }

            if(borrowDate != null)
            {
                query = query.Where(h => h.BorrowDate.Date == borrowDate);
            }

            if(status != null)
            {
                query = query.Where(h => h.Status == status);
            }

            if(itemId != null)
            {
                query = query.Where(h => h.BorrowItems.Any(bi => bi.ItemId == itemId));
            }

            ViewData["BorrowerId1"] = borrowerId;
            ViewData["Status1"] = status;
            ViewData["ItemId1"] = itemId;
            ViewData["BorrowDate1"] = borrowDate?.ToString("MM/dd/yyyy");

            if(!pageNumber.HasValue)
            {
                pageNumber = 1;
            }

            if(!pageSize.HasValue)
            {
                pageSize = 2;
            }

            int totalHistories = await query.CountAsync();
            int totalPages = (int)Math.Ceiling((double)totalHistories / pageSize.Value);
            var histories = await query.Skip((pageNumber.Value - 1) * pageSize.Value)
                .Take(pageSize.Value)
                .ToListAsync();

            ViewData["TotalPages"] = totalPages;
            ViewData["CurrentPage"] = pageNumber;

            return View(histories);
    }

        [HttpGet]
        public IActionResult Return(int historyId,int itemId)
        {
            // Tìm bản ghi History cần trả mục
            var historyRecord = _context.History.FirstOrDefault(history => history.Id == historyId);

            // Tìm bản ghi BorrowItem cần trả
            var borrowedItemRecord = _context.BorrowedItems.FirstOrDefault(item => item.HistoryId == historyId && item.ItemId == itemId);

            if(historyRecord != null && borrowedItemRecord != null)
            {
                // Cộng lại Quantity vào AvailableQuantity
                var itemToReturn = _context.Item.FirstOrDefault(item => item.Id == itemId);
                if(itemToReturn != null)
                {
                    itemToReturn.AvailableQuantity += borrowedItemRecord.Quantity;
                    borrowedItemRecord.ReturnedQuanyity = borrowedItemRecord.Quantity;
                }

                bool isOpened = false;
                foreach(var a in _context.BorrowedItems.Where(item => item.HistoryId == historyId))
                {
                    if(a.ReturnedQuanyity != a.Quantity)
                    {
                        isOpened = true ; break;
                    }
                }

                if(!isOpened)
                {
                    historyRecord.Status = Status.Closed;
                }

                _context.SaveChanges();
            }

            return RedirectToAction("Details",new { id = historyId });
        }

        // GET: Histories/Details/5
        public IActionResult Details(int? id) // id là historyId
        {
            if(id == null)
            {
                return NotFound();
            }

            var history = _context.History.FirstOrDefault(h => h.Id == id);

            if(history == null)
            {
                return NotFound();
            }

            HistoryViewModel viewModel = new HistoryViewModel
            {
                Id = history.Id,
                BorrowDate = history.BorrowDate,
                BorrowerId = history.BorrowerId,
                Status = history.Status,
            };
            viewModel.BorrowItems = _context.BorrowedItems
                .Include(bi => bi.Item)
                .Where(bi => bi.HistoryId == id).ToList();

            return View(viewModel);
        }

        // GET: Histories/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if(id == null || _context.History == null)
            {
                return NotFound();
            }

            var history = await _context.History
                .Include(h => h.Borrower)
                .FirstOrDefaultAsync(m => m.Id == id);
            if(history == null)
            {
                return NotFound();
            }

            return View(history);
        }

        // POST: Histories/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if(_context.History == null)
            {
                return Problem("Entity set 'ApplicationDbContext.History'  is null.");
            }
            var history = await _context.History.FindAsync(id);
            if(history != null)
            {
                _context.History.Remove(history);
            }

            var history1 = await _context.History
                .Include(h => h.BorrowItems)
                .ThenInclude(h => h.Item).FirstOrDefaultAsync(a => a.Id == id);
            foreach(var item in history1.BorrowItems)
            {
                if(item.ReturnedQuanyity < item.Quantity)
                {
                    var b = _context.Item.FirstOrDefault(a => a.Id == item.ItemId);
                    b.AvailableQuantity = b.AvailableQuantity + item.Quantity;
                }
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HistoryExists(int id)
        {
            return (_context.History?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
