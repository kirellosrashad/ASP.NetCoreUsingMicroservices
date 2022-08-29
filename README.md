# ASP.NetCoreUsingMicroservices
We have a Catalog Service that handles the company’s “Catalog” 
(ex: menu of items in a restaurant), We also have a Service that sends 
Email notifications to users notifying them that an item that was added to the catalog.

Here is what to do : 
1.	Define the Catalog Service and the Email Service using the Microservices Architecture in a .Net Core Solution.
2.	Implement the Controller Service Repository pattern inside the catalog service.
3.	Create CRUD (Create, Read, Update, Delete) Restful APIs for the Catalog Service and store the data in a local database.
4.	The database will contain 1 table called Products.
5.	Products will contain (Name, Price, Cost, Image base64)
6.	Implement the Email Service to Send Email (random address) when a new product is added by using RabbitMQ between the services.
7.	Add an API Gateway to route the requests to the Catalog Microservice
8.	Add an XUnit Test for the catalog service.
9.	Draw the Architecture Diagram of the structure.
10.	Adding a Mediator is a PLUS.
11.	Adding a Custom Authorization Middleware is a PLUS
