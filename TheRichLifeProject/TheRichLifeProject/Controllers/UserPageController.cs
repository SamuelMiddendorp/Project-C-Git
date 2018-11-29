﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheRichLifeProject.Models;
using TheRichLifeProject.ViewModel;

namespace TheRichLifeProject.Controllers
{
    [Authorize]
    public class UserPageController : Controller
    {
        private readonly DatabaseContext _context;
        public UserPageController(DatabaseContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            var userId = User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
            User CurrentUser = _context.Users.Find(Int32.Parse(userId));
            return View(CurrentUser);
        }
        public IActionResult Edit(string username, string adress, string name, string surname, string phonenumber, DateTime birth, string email)
        {
            var userId = User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
            User CurrentUser = _context.Users.Find(Int32.Parse(userId));
            CurrentUser.Adress = adress;
            CurrentUser.Username = username;
            CurrentUser.Name = name;
            CurrentUser.SurName = surname;
            CurrentUser.PhoneNumber = phonenumber;
            CurrentUser.Birth = birth;
            CurrentUser.Email = email;
            _context.SaveChanges();
            return RedirectToAction("Index", "UserPage");
        }
        public IActionResult Orders()
        {
            var orders = _context.Orders.ToList();
            var orderdetails = _context.OrderDetails.ToList();
            var products = _context.Products.ToList();
            var order = new List<Order>();
            string userId = User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
            User CurrentUser = _context.Users.Find(Int32.Parse(userId));
            foreach (var item in orders.Where(x => x.User == CurrentUser)){
                item.OrderDetails = new List<OrderDetail>();
                foreach (var item2 in orderdetails.Where(x => x.Order == item))
                {
                    
                    item.OrderDetails.Add(item2);
                }
                order.Add(item);
            }

            
            return View(order);
        }
    }
}