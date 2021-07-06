# eCartMicroservices.gRPC
Developing a real world e-commerce use case with only gRPC communication. We will have 3 gRPC services, Product, Cart and Discount.
Apart form gRPC services we will also use Worker Services for creation of Products and Cart.

Microservices are modern distributed systems so with gRPC in ASP.NET 5, we will develop high-performance, cross-platform applications for building distributed systems and APIs. It’s an ideal choice for communication between backend microservices, internal network applications, or iot devices and services. With the release of ASP.NET 5, Microsoft has added first-class support for creating gRPC services with Asp.Net 5.

## ProductGrpc Server Application
This will be Asp.Net gRPC server web application and expose apis for Product Crud operations.

## Product Worker Service
This product worker service project will be the client of ProductGrpc application and generate products and insert bulk product records into Product database by using client streaming gRPC proto services of ProductGrpc application. This operation will be in a time interval and looping as a service application.

## CartGrpc Server Application
This will be asp.net gRPC server web application and expose apis for Cart and Cart items operations. The grpc services will be create cart and add or remove item into cart.

## Cart Worker Service
This Cart worker service project will be the client of both ProductGrpc and CartGrpc application. This worker service will read the products from ProductGrpc and create cart and add product items into cart by using gRPC proto services of ProductGrpc and CartGrpc application. This operation will be in a time interval and looping as a service application.

## DiscountGrpc Server Application
When adding product item into cart, it will retrieve the discount value and calculate the final price of product. This communication also will be gRPC call with CartgRPC and DiscountgRPC application.

## Cart Authentication, Identity Server
Centralized standalone Authentication Server with implementing IdentityServer4 package and the name of microservice is Identity Server.
Identity Server4 is an open source framework which implements OpenId Connect and OAuth2 protocols for .Net Core.
With IdentityServer, we can provide protect our Cart gRPC services with OAuth 2.0 and JWT tokens. Cart Worker will get the token before send request to Cart gRPC server application.

### Topics Covered:
* gRPC in Microservices with .Net 5
* Working with Protocol Buffers using proto3 Language and apply google well-known types
* gRPC Method Types, RPC life cycles - Unary, Server streaming, Client streaming, Bidirectional streaming
* Develop Protocol Buffer File (protobuf file) for gRPC Contract-First API Development
* Building a high-performance gRPC Inter-service Communication with .Net 5
* Communication between backend microservices with gRPC and AspNet 5
* Background tasks with Worker Service Projects in AspNet Core 5
* Manage long running service apps with AspNet Core Worker Service template
* Consuming a scoped gRPC services in a background task with Worker Service Projects in AspNet Core 5
* Implementation of e-commerce logic with only gRPC communication - Product, ShoppingCart and Discount gRPC services
* Consuming gRPC Server Microservices from Product and ShoppingCart Worker Service in a background task
* Secure the gRPC services with standalone Identity Server microservices with OAuth 2.0 and JWT token
* ProductGrpc Server Expose CRUD operations with gRPC
* Using Entity Framework Core 5 In-Memory Database with Code-First Approach
* Develop Realworld Inter-Service Communication Use Case with Product, ShoppingCart and Discount gRPC services and Consumes from Worker Services
* Use gRPC to implement a fast and distributed microservices systems
* Create Client Console Application for Consuming Grpc Microservices
* Generate Products with ProductFactory class in Product Worker Service Application
* Logging and Exception Handling with Grpc Server Application
* Authenticate gRPC Services with IdentityServer4 Protect ShoppingCartGrpc Method with OAuth 2.0 and JWT Bearer Token

## Courtesy: 
https://www.udemy.com
Mehmet Özkaya: https://medium.com/aspnetrun
