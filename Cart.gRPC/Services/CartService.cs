using AutoMapper;
using Cart.gRPC.Data;
using Cart.gRPC.Models;
using Cart.gRPC.Protos;
using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Cart.gRPC.Services
{
    [Authorize]
    public class CartService : CartProtoService.CartProtoServiceBase
    {
        private readonly CartContext _cartDbContext;
        private readonly DiscountService _discountService;
        private readonly IMapper _mapper;
        private readonly ILogger<CartService> _logger;

        public CartService(CartContext cartDbContext, IMapper mapper, ILogger<CartService> logger, DiscountService discountService)
        {
            _cartDbContext = cartDbContext;
            _mapper = mapper;
            _logger = logger;
            _discountService = discountService;
        }

        public override async Task<CartModel> GetCart(GetCartRequest request, ServerCallContext context)
        {
            var cart = await _cartDbContext.eCart.FirstOrDefaultAsync(s => s.UserName == request.Username);
            if (cart == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"ShoppingCart with UserName={request.Username} is not found."));
            }

            var cartModel = _mapper.Map<CartModel>(cart);
            return cartModel;
        }

        public override async Task<CartModel> CreateCart(CartModel request, ServerCallContext context)
        {
            var cart = _mapper.Map<eCart>(request);

            var isExist = await _cartDbContext.eCart.AnyAsync(s => s.UserName == cart.UserName);
            if (isExist)
            {
                _logger.LogError("Invalid UserName for Cart creation. UserName : {userName}", cart.UserName);
                throw new RpcException(new Status(StatusCode.NotFound, $"Cart with UserName={request.Username} is already exist."));
            }

            _cartDbContext.eCart.Add(cart);
            await _cartDbContext.SaveChangesAsync();

            _logger.LogInformation("Cart is successfully created.UserName : {userName}", cart.UserName);

            var CartModel = _mapper.Map<CartModel>(cart);
            return CartModel;
        }

        [AllowAnonymous]
        public override async Task<AddItemIntoCartResponse> AddItemIntoCart(IAsyncStreamReader<AddItemIntoCartRequest> requestStream, ServerCallContext context)
        {
            while (await requestStream.MoveNext())
            {
                // Get cart if exist or not
                // Check item if exist in cart or not
                // if item exist +1 quantity
                // if not exist add new item into sc
                // check discount and set the item price

                var Cart = await _cartDbContext.eCart.FirstOrDefaultAsync(s => s.UserName == requestStream.Current.Username);
                if (Cart == null)
                {
                    throw new RpcException(new Status(StatusCode.NotFound, $"Cart with UserName={requestStream.Current.Username} is not found."));
                }

                var newAddedCartItem = _mapper.Map<CartItem>(requestStream.Current.NewCartItem);
                var cartItem = Cart.Items.FirstOrDefault(i => i.ProductId == newAddedCartItem.ProductId);
                if (null != cartItem)
                {
                    cartItem.Quantity++;
                }
                else
                {
                    // GRPC CALL DISCOUNT SERVICE -- check discount and set the item price
                    var discount = await _discountService.GetDiscount(requestStream.Current.DiscountCode);
                    newAddedCartItem.Price -= discount.Amount;

                    Cart.Items.Add(newAddedCartItem);
                }
            }

            var insertCount = await _cartDbContext.SaveChangesAsync();

            var response = new AddItemIntoCartResponse
            {
                Success = insertCount > 0,
                InsertCount = insertCount
            };

            return response;
        }

        [AllowAnonymous]
        public override async Task<RemoveItemFromCartResponse> RemoveItemFromCart(RemoveItemFromCartRequest request, ServerCallContext context)
        {
            // Get cart if exist or not
            // Check item if exist in cart or not
            // Remove item in cart db

            var cart = await _cartDbContext.eCart.FirstOrDefaultAsync(s => s.UserName == request.Username);
            if (cart == null)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"ShoppingCart with UserName={request.Username} is not found."));
            }

            var removeCartItem = cart.Items.FirstOrDefault(i => i.ProductId == request.RemoveCartItem.ProductId);
            if (null == removeCartItem)
            {
                throw new RpcException(new Status(StatusCode.NotFound, $"CartItem with ProductId={request.RemoveCartItem.ProductId} is not found in the ShoppingCart."));
            }

            cart.Items.Remove(removeCartItem);

            var removeCount = await _cartDbContext.SaveChangesAsync();

            var response = new RemoveItemFromCartResponse
            {
                Success = removeCount > 0
            };

            return response;
        }
    }
}
