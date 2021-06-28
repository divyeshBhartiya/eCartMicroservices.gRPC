using Cart.gRPC.Protos;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Product.gRPC.Protos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace CartWorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration _config;

        public Worker(ILogger<Worker> logger, IConfiguration config)
        {
            _logger = logger;
            _config = config;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                //0 Get Token from IS4
                //1 Create Cart if not exist
                //2 Retrieve products from product grpc with server stream
                //3 Add sc items into Cart with client stream

                //0 Get Token from IS4
                var token = await GetTokenFromIS4();

                //1 Create Cart if not exist
                using var cartChannel = GrpcChannel.ForAddress(_config.GetValue<string>("WorkerService:CartServerUrl"));
                var cartClient = new CartProtoService.CartProtoServiceClient(cartChannel);

                var cartModel = await GetOrCreateCartAsync(cartClient, token);

                // open cart client stream
                using var cartClientStream = cartClient.AddItemIntoCart();

                //2 Retrieve products from product grpc with server stream
                using var productChannel = GrpcChannel.ForAddress(_config.GetValue<string>("WorkerService:ProductServerUrl"));
                var productClient = new ProductProtoService.ProductProtoServiceClient(productChannel);

                _logger.LogInformation("GetAllProducts started..");
                using var clientData = productClient.GetAllProducts(new GetAllProductsRequest());
                await foreach (var responseData in clientData.ResponseStream.ReadAllAsync())
                {
                    _logger.LogInformation("GetAllProducts Stream Response: {responseData}", responseData);

                    //3 Add cart items into cart with client stream
                    var addNewCartItem = new AddItemIntoCartRequest
                    {
                        Username = _config.GetValue<string>("WorkerService:UserName"),
                        DiscountCode = "CODE_100",
                        NewCartItem = new CartItemModel
                        {
                            ProductId = responseData.ProductId,
                            Productname = responseData.Name,
                            Price = responseData.Price,
                            Color = "Black",
                            Quantity = 1
                        }
                    };

                    await cartClientStream.RequestStream.WriteAsync(addNewCartItem);
                    _logger.LogInformation("Cart Client Stream Added New Item : {addNewScItem}", addNewCartItem);
                }
                await cartClientStream.RequestStream.CompleteAsync();

                var addItemIntoCartResponse = await cartClientStream;
                _logger.LogInformation("AddItemIntoCart Client Stream Response: {addItemIntoCartResponse}", addItemIntoCartResponse);

                await Task.Delay(_config.GetValue<int>("WorkerService:TaskInterval"), stoppingToken);
            }
        }

        private async Task<CartModel> GetOrCreateCartAsync(CartProtoService.CartProtoServiceClient cartClient, string token)
        {
            CartModel cartModel;
            try
            {
                _logger.LogInformation("GetCartAsync started..");

                var headers = new Metadata
                {
                    { "Authorization", $"Bearer {token}" }
                };

                cartModel = await cartClient.GetCartAsync(new GetCartRequest { Username = _config.GetValue<string>("WorkerService:UserName") }, headers);

                _logger.LogInformation("GetCartAsync Response: {CartModel}", cartModel);
            }
            catch (RpcException exception)
            {
                if (exception.StatusCode == StatusCode.NotFound)
                {
                    _logger.LogInformation("CreateCartAsync started..");
                    cartModel = await cartClient.CreateCartAsync(new CartModel { Username = _config.GetValue<string>("WorkerService:UserName") });
                    _logger.LogInformation("CreateCartAsync Response: {cartModel}", cartModel);
                }
                else
                {
                    throw;
                }
            }

            return cartModel;
        }

        private async Task<string> GetTokenFromIS4()
        {
            return string.Empty;
        }
    }
}
