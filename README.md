# Introduction to Microservice 

This repository is an introduction to .NET microservices
with container based services.
The main service are 2 :
-   Platform service
-   Command service

# Platform service
Expose rest api  under nginx loadbalancer and store the data on SQL Server database using entity framework (with persistence)
and expose an gRPC server

# Command service
Expose rest api  under nginx loadbalancer and store the data on in memory database  using entity framework (No persistence) and using gRPC cleint

The 2 services talk to each other in asynchronous way using  RabbitMQ
and in synchronous way using gRPC and Http rest

# Architecture
![Architecture](https://github.com/FBicsharp/MicroserviceSample/blob/master/Docs/Architecture.jpg)


# Project Behavior
Platform service when a post request is made send an Http request to Command service
and also publish an event to RabbitMQ with the content , all data are save on SQL table, after that expose the entire data via gRPC calling

Command service when start retrive all data via gRPC from Platform service and after that subscribe to RabbitMQ evet exposed.
The command service expose your own data under Http rest api for creating new command based on platform data.


# Technologies stack used
- docker
- kubernetes
- Entity Framework
- SQL Server
- Rest API
- RabbitMQ for asychronous
- gRPC
- a lot of patience



