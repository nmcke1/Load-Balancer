# Load Balancer Project

## Overview

This project implements a simple TCP-based **Load Balancer** in C#. It distributes incoming client connections across multiple backend dummy servers using a **weighted round-robin** algorithm. The load balancer handles server failures gracefully by removing failed servers dynamically.

## Features

- **Weighted Round Robin:** Distributes requests based on server weights.
- **Dummy Servers:** Simulated backend servers that respond with a greeting message.
- **Dummy Clients:** Simulated clients that can send requests to be load balanced.
- **Dynamic Server Management:** Can gracefully deal with failing servers by removing them dynamically.

## Components

- **RoundRobinList:** Circular list implementing weighted round-robin selection.
- **LoadBalancerService:** Accepts clients and forwards requests to backend servers.
- **Client Handlers:** Manage communication between clients and servers.
- **DummyClient:** Simple TCP client to test server and load balancer responses.
- **DummyServer:** Simple TCP server responding with greetings.

## Getting Started

Navigate to /LoadBalancer/LoadBalancer/ to find the LoadBalancer.csproj file. 
From here run:

```bash
dotnet run
```
To build and run the application.
This will run a demo of the Load Balancer to show the load balancer in action. It will cycle through its list of weighted servers, then when a server is removed you can see how it handles the dynamic removal.
