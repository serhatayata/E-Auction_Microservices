using ESourcing.Core.Repositories;
using ESourcing.Infrastructure.Repository;
using ESourcing.UI.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ESourcing.UI.Controllers
{
    
    public class AuctionController : Controller
    {
        private readonly IUserRepository _userRepository;
        public AuctionController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var userList = await _userRepository.GetAllAsync();
            ViewBag.UserList = userList;
            return View();
        }
        [HttpPost]
        public IActionResult Create(AuctionViewModel model)
        {
            return View(model);
        }
        public IActionResult Detail()
        {
            return View();
        }
    }
}
