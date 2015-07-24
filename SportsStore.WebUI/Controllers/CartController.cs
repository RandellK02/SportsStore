using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Models;

namespace SportsStore.WebUI.Controllers
{
    public class CartController : Controller
    {
        //
        // GET: /Cart/

        private IProductRepository repository;
        private IOrderProcessor orderProcessor;

        public CartController(IProductRepository repository, IOrderProcessor orderProcessor)
        {
            this.repository = repository;
            this.orderProcessor = orderProcessor;
        }

        public ViewResult Index(Cart cart, String returnUrl)
        {
            return View(new CartIndexViewModel
            {
                Cart = cart,
                ReturnUrl = returnUrl
            });
        }

        public RedirectToRouteResult AddToCart(Cart cart, int productId, string returnUrl)
        {
            Product product = repository.Products.FirstOrDefault(p => p.ProductID == productId);

            if (product != null)
            {
                cart.AddItem(product, 1);
            }

            return RedirectToAction("Index", new { returnUrl });
        }

        public RedirectToRouteResult RemoveFromCart(Cart cart, int productId, string returnUrl)
        {
            Product product = repository.Products.FirstOrDefault(p => p.ProductID == productId);

            if (product != null)
            {
                cart.RemoveLine(product);
            }

            return RedirectToAction("Index", new { returnUrl });
        }

        public PartialViewResult Summary(Cart cart)
        {
            return PartialView( cart );
        }

        public ViewResult Checkout()
        {
            return View( new ShippingDetails() );
        }

        [HttpPost]
        public ViewResult Checkout(Cart cart, ShippingDetails shippingDetails)
        {
            if ( cart.Lines.Count() == 0 )
            {
                ModelState.AddModelError( "", "Sorry, your cart is empty!" );
            }

            if ( ModelState.IsValid )
            {
                orderProcessor.ProcessOrder( cart, shippingDetails );
                cart.Clear();
                return View( "Completed" );
            }
            else
            {
                return View( shippingDetails );
            }
        }

        [Obsolete("Added Cart model binder replaced this method")]
        private Cart GetCart()
        {
            Cart cart = (Cart)Session["Cart"];
            if (cart == null)
            {
                cart = new Cart();
                Session["Cart"] = cart;
            }

            return cart;
        }
    }
}
