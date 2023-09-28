using Final.Data;
using Final.Models;
using Final.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Diagnostics;

namespace Final.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private List<Item> borrowedItems = new List<Item>();
        private readonly SessionManager _sessionManager;
        private readonly IBorrowerServices _borrowerServices;
        public HomeController(ApplicationDbContext context,SessionManager sessionManager,IBorrowerServices borrowerServices)
        {
            _context = context;
            borrowedItems = new List<Item>();
            _sessionManager = sessionManager;
            _borrowerServices = borrowerServices;
        }

        public IActionResult Index()
        {
            var items = _context.Item.ToList();
            var ibiVM = new I_BI_ViewModel();
            ibiVM.listItem = items;

            if(!_sessionManager.IsExist("myListKey"))
            {
                _sessionManager.SetSessionData("myListKey",new List<BorrowItemViewModel>());
            }

            List<BorrowItemViewModel> retrievedList = _sessionManager.GetSessionData<BorrowItemViewModel>("myListKey");

            decimal totalCost = retrievedList.Sum(item => item.Cost);
            ViewBag.TotalCost = totalCost;
            ViewData["data"] = retrievedList;

            var borrowers = _borrowerServices.GetAll();
            ViewBag.BorrowerList = new SelectList(borrowers,"Id","Name");

            return View(ibiVM);
        }

        //public JsonResult AddToBorrowedItems(int itemId)
        //{
        //    List<BorrowItemViewModel> retrievedList = _sessionManager.GetSessionData<BorrowItemViewModel>("myListKey");

        //    // Kiểm tra xem mục có tồn tại trong danh sách chưa
        //    var existingItem = retrievedList.FirstOrDefault(item => item.ItemId == itemId);

        //    if(existingItem != null)
        //    {
        //        if(existingItem.Quantity < existingItem.MaxQuantityFromItem)
        //        {
        //            existingItem.Quantity++;
        //            existingItem.Cost = existingItem.Price * existingItem.Quantity;

        //            var itemToReduce = _context.Item.FirstOrDefault(item => item.Id == itemId);

        //            if(itemToReduce != null && itemToReduce.AvailableQuantity > 0)
        //            {
        //                itemToReduce.AvailableQuantity--;
        //                _context.SaveChanges();
        //            }
        //            else
        //            {
        //                TempData["Message"] = "Số lượng tồn kho không đủ.";
        //            }
        //        }
        //        else
        //        {
        //            TempData["Message"] = "Đã đạt tối đa số lượng cho mục này.";
        //        }
        //    }
        //    else
        //    {
        //        var itemToAdd = _context.Item.FirstOrDefault(item => item.Id == itemId);

        //        if(itemToAdd != null)
        //        {
        //            var borrowItemVM = new BorrowItemViewModel
        //            {
        //                ItemId = itemId,
        //                Quantity = 1,
        //                Price = itemToAdd.Price,
        //                Cost = itemToAdd.Price,
        //                ItemName = itemToAdd.Title,
        //                MaxQuantityFromItem = itemToAdd.AvailableQuantity
        //            };

        //            // Kiểm tra xem AvailableQuantity đã còn lớn hơn 0 trước khi giảm
        //            if(itemToAdd.AvailableQuantity >= borrowItemVM.Quantity)
        //            {
        //                retrievedList.Add(borrowItemVM);
        //                itemToAdd.AvailableQuantity -= borrowItemVM.Quantity;

        //                // Lưu thay đổi vào cơ sở dữ liệu
        //                _context.Item.Update(itemToAdd);
        //                _context.SaveChanges();
        //                TempData["Message"] = "Thêm thành công!";
        //            }
        //            else
        //            {
        //                TempData["Message"] = "Số lượng tồn kho không đủ.";
        //            }
        //        }
        //    }

        //    _sessionManager.SetSessionData("myListKey",retrievedList);
        //    //return RedirectToAction("Index");
        //    return Json(new
        //    {
        //        itemId,
        //        message = TempData["Message"],
        //        data = retrievedList,
        //    });
        //}

        //[HttpGet]
        //public IActionResult Editbi(int id)
        //{
        //    List<BorrowItemViewModel> retrievedList = _sessionManager.GetSessionData<BorrowItemViewModel>("myListKey");
        //    var borrowedItem = retrievedList.Find(a => a.ItemId == id);

        //    var bilist = new BorrowItemViewModel {
        //        Id = borrowedItem.Id,
        //        ItemId = borrowedItem.ItemId,
        //        ItemName = borrowedItem.ItemName,
        //        Price = borrowedItem.Price,
        //        Cost = borrowedItem.Cost,
        //        Quantity = borrowedItem.Quantity
        //    };
        //    return View(bilist);
        //}

        //[HttpPost]
        //public IActionResult Editbi(BorrowItemViewModel borrowItem)
        //{
        //    List<BorrowItemViewModel> retrievedList = _sessionManager.GetSessionData<BorrowItemViewModel>("myListKey");
        //    var existingItem = retrievedList.FirstOrDefault(item => item.ItemId == borrowItem.ItemId);

        //    if(existingItem != null)
        //    {
        //        if(borrowItem.Quantity <= existingItem.MaxQuantityFromItem)
        //        {
        //            existingItem.ItemName = borrowItem.ItemName;
        //            var a = _context.Item.FirstOrDefault(item => item.Id == borrowItem.ItemId);
        //            int b = existingItem.Quantity - borrowItem.Quantity;
        //            a.AvailableQuantity = a.AvailableQuantity + b;
        //            existingItem.Quantity = borrowItem.Quantity;
        //            existingItem.Price = borrowItem.Price;
        //            existingItem.Cost = borrowItem.Quantity * borrowItem.Price;
        //            _context.Update(a);
        //            _context.SaveChanges();
        //            _sessionManager.SetSessionData("myListKey",retrievedList);
        //            TempData["Message"] = "Update successful!";
        //            return RedirectToAction("Index");
        //        }
        //        else
        //        {
        //            TempData["Message"] = "The new quantity cannot be greater than the maximum quantity for this item!";
        //        }
        //    }
        //    else
        //    {
        //        TempData["Message"] = "Invalid request!";
        //    }
        //    return RedirectToAction("Index");
        //}

        //public JsonResult Deletebi(int itemId)
        //{
        //    List<BorrowItemViewModel> retrievedList = _sessionManager.GetSessionData<BorrowItemViewModel>("myListKey");

        //    var itemToRemove = retrievedList.FirstOrDefault(item => item.ItemId == itemId);

        //    if(itemToRemove != null)
        //    {
        //        int quantityToRemove = itemToRemove.Quantity;

        //        var itemInDatabase = _context.Item.FirstOrDefault(item => item.Id == itemId);

        //        if(itemInDatabase != null)
        //        {
        //            // Cộng số lượng đã xóa vào AvailableQuantity của mục trong cơ sở dữ liệu
        //            itemInDatabase.AvailableQuantity += quantityToRemove;

        //            // Lưu thay đổi vào cơ sở dữ liệu
        //            _context.Item.Update(itemInDatabase);
        //            _context.SaveChanges();
        //        }

        //        retrievedList.Remove(itemToRemove);

        //        // Lưu danh sách đã cập nhật vào Session
        //        _sessionManager.SetSessionData("myListKey",retrievedList);
        //    }
        //    return Json(new
        //    {
        //        message = "Xóa Item thành công!",
        //        data = retrievedList
        //    });
        //    //return RedirectToAction("Index",retrievedList); // Chuyển hướng về trang Index với danh sách đã cập nhật
        //}

        public JsonResult AddToBorrowedItems(int itemId)
        {
            List<BorrowItemViewModel> retrievedList = _sessionManager.GetSessionData<BorrowItemViewModel>("myListKey");

            // Kiểm tra xem mục có tồn tại trong danh sách chưa
            var existingItem = retrievedList.FirstOrDefault(item => item.ItemId == itemId);

            if(existingItem != null)
            {
                if(existingItem.Quantity < existingItem.MaxQuantityFromItem)
                {
                    existingItem.Quantity++;
                    existingItem.Cost = existingItem.Price * existingItem.Quantity;
                }
                else
                {
                    TempData["Message"] = "Đã đạt tối đa số lượng cho mục này.";
                }
            }
            else
            {
                var itemToAdd = _context.Item.FirstOrDefault(item => item.Id == itemId);

                if(itemToAdd != null)
                {
                    var borrowItemVM = new BorrowItemViewModel
                    {
                        ItemId = itemId,
                        Quantity = 1,
                        Price = itemToAdd.Price,
                        Cost = itemToAdd.Price,
                        ItemName = itemToAdd.Title,
                        MaxQuantityFromItem = itemToAdd.AvailableQuantity
                    };

                    if(itemToAdd.AvailableQuantity >= borrowItemVM.Quantity)
                    {
                        retrievedList.Add(borrowItemVM);
                        TempData["Message"] = "Thêm thành công!";
                    }
                    else
                    {
                        TempData["Message"] = "Số lượng tồn kho không đủ.";
                    }
                }
            }

            _sessionManager.SetSessionData("myListKey",retrievedList);

            return Json(new
            {
                itemId,
                message = TempData["Message"],
                data = retrievedList,
            });
        }

        [HttpGet]
        public IActionResult Editbi(int id)
        {
            List<BorrowItemViewModel> retrievedList = _sessionManager.GetSessionData<BorrowItemViewModel>("myListKey");
            var borrowedItem = retrievedList.Find(a => a.ItemId == id);

            var bilist = new BorrowItemViewModel
            {
                Id = borrowedItem.Id,
                ItemId = borrowedItem.ItemId,
                ItemName = borrowedItem.ItemName,
                Price = borrowedItem.Price,
                Cost = borrowedItem.Cost,
                Quantity = borrowedItem.Quantity
            };
            return View(bilist);
        }

        [HttpPost]
        public IActionResult Editbi(BorrowItemViewModel borrowItem)
        {
            List<BorrowItemViewModel> retrievedList = _sessionManager.GetSessionData<BorrowItemViewModel>("myListKey");
            var existingItem = retrievedList.FirstOrDefault(item => item.ItemId == borrowItem.ItemId);

            if(existingItem != null)
            {
                if(borrowItem.Quantity <= existingItem.MaxQuantityFromItem)
                {
                    existingItem.ItemName = borrowItem.ItemName;
                    existingItem.Quantity = borrowItem.Quantity;
                    existingItem.Price = borrowItem.Price;
                    existingItem.Cost = borrowItem.Quantity * borrowItem.Price;

                    // Không cập nhật cơ sở dữ liệu ở đây

                    _sessionManager.SetSessionData("myListKey",retrievedList);
                    TempData["Message"] = "Chỉnh sửa thành công!";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["Message"] = "The new quantity cannot be greater than the maximum quantity for this item!";
                }
            }
            else
            {
                TempData["Message"] = "Invalid request!";
            }
            return RedirectToAction("Index");
        }

        public JsonResult Deletebi(int itemId)
        {
            List<BorrowItemViewModel> retrievedList = _sessionManager.GetSessionData<BorrowItemViewModel>("myListKey");

            var itemToRemove = retrievedList.FirstOrDefault(item => item.ItemId == itemId);

            if(itemToRemove != null)
            {
                retrievedList.Remove(itemToRemove);

                // Lưu danh sách đã cập nhật vào Session
                _sessionManager.SetSessionData("myListKey",retrievedList);
            }

            return Json(new
            {
                message = "Xóa Item thành công!",
                data = retrievedList
            });
        }

        public JsonResult TotalCostIB()
        {
            List<BorrowItemViewModel> retrievedList = _sessionManager.GetSessionData<BorrowItemViewModel>("myListKey");

            decimal totalCost = retrievedList.Sum(item => item.Cost);

            ViewBag.TotalCost = totalCost;

            //return View("Index",retrievedList);

            return Json(new
            {
                totalCost,
                data = retrievedList
            });
        }

        //[HttpPost]
        //public JsonResult ConfirmBorrow(int SelectedBorrowerId)
        //{
        //    List<BorrowItemViewModel> borrowedItems = _sessionManager.GetSessionData<BorrowItemViewModel>("myListKey");

        //    if(borrowedItems != null && borrowedItems.Any())
        //    {
        //        // Tạo bản ghi trong bảng "History" cho mượn
        //        var historyRecord = new History
        //        {
        //            BorrowDate = DateTime.Now,
        //            Status = Status.Opened,
        //            BorrowerId = SelectedBorrowerId
        //        };
        //        _context.History.Add(historyRecord);
        //        _context.SaveChanges(); // Lưu để có ID cho bản ghi history

        //        // Tạo bản ghi trong bảng "BorrowItem" cho từng mục đã mượn
        //        foreach(var borrowedItem in borrowedItems)
        //        {
        //            var borrowItemRecord = new BorrowItem
        //            {
        //                ItemId = borrowedItem.ItemId,
        //                Quantity = borrowedItem.Quantity,
        //                Cost = borrowedItem.Cost, // Sử dụng Cost từ ViewModel
        //                HistoryId = historyRecord.Id // Sử dụng ID của historyRecord đã lưu
        //            };
        //            _context.BorrowedItems.Add(borrowItemRecord);
        //            _context.SaveChanges();
        //        }

        //        borrowedItems.Clear();
        //        _sessionManager.SetSessionData("myListKey",borrowedItems);

        //        return Json(new
        //        {
        //            message = "Xác nhận mượn thành công!"
        //        });
        //    }
        //    return Json(new
        //    {
        //        message = "Yêu cầu không hợp lệ!"
        //    });
        //    //return RedirectToAction("Index");
        //}

        [HttpPost]
        public JsonResult ConfirmBorrow(int SelectedBorrowerId)
        {
            List<BorrowItemViewModel> borrowedItems = _sessionManager.GetSessionData<BorrowItemViewModel>("myListKey");

            if(borrowedItems != null && borrowedItems.Any())
            {
                // Tạo bản ghi trong bảng "History" cho mượn
                var historyRecord = new History
                {
                    BorrowDate = DateTime.Now,
                    Status = Status.Opened,
                    BorrowerId = SelectedBorrowerId
                };
                _context.History.Add(historyRecord);
                _context.SaveChanges(); // Lưu để có ID cho bản ghi history

                // Tạo bản ghi trong bảng "BorrowItem" cho từng mục đã mượn
                foreach(var borrowedItem in borrowedItems)
                {
                    var borrowItemRecord = new BorrowItem
                    {
                        ItemId = borrowedItem.ItemId,
                        Quantity = borrowedItem.Quantity,
                        Cost = borrowedItem.Cost, // Sử dụng Cost từ ViewModel
                        HistoryId = historyRecord.Id // Sử dụng ID của historyRecord đã lưu
                    };
                    _context.BorrowedItems.Add(borrowItemRecord);
                }

                // Giảm AvailableQuantity và lưu vào cơ sở dữ liệu
                foreach(var borrowedItem in borrowedItems)
                {
                    var itemToReduce = _context.Item.FirstOrDefault(item => item.Id == borrowedItem.ItemId);

                    if(itemToReduce != null && itemToReduce.AvailableQuantity >= borrowedItem.Quantity)
                    {
                        itemToReduce.AvailableQuantity -= borrowedItem.Quantity;
                    }
                }

                _context.SaveChanges(); // Lưu thay đổi của AvailableQuantity vào cơ sở dữ liệu

                borrowedItems.Clear();
                _sessionManager.SetSessionData("myListKey",borrowedItems);

                return Json(new
                {
                    message = "Xác nhận mượn thành công!"
                });
            }

            return Json(new
            {
                message = "Yêu cầu không hợp lệ!"
            });
        }


        [HttpGet]
        public IActionResult LoadBorrowedItems()
        {
            // Load borrowed items
            List<BorrowItemViewModel> retrievedList = _sessionManager.GetSessionData<BorrowItemViewModel>("myListKey");

            // Calculate the TotalCost
            decimal totalCost = retrievedList.Sum(item => item.Cost);

            // Create a combined JSON response
            var response = new
            {
                totalCost,
                borrowedItems = retrievedList,
            };

            return Json(response);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}