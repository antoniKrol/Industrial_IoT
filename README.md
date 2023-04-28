# Industrial_IoT
________________________________
1. ## Problem

Company X runs a factory with multiple production lines. Production is handled by machine operators and controlled using different, isolated systems. There’s no system that allows for real-time monitoring of all production linesand most data is gathered using paper documentation.

The company wants to transform their production and as the first step they want to connect their production lines to an Industrial IoT platform. After a series of PoCs and workshops they chose Azure.Your task is to designand develop a Production Monitoring System that will gather data from available productionlines and perform basic analysis, calculations and business logic on that data.

__________________________________
2. ## Technical requirements

The goal of the project is to design and develop the system in two pieces:

  - Agent (may beaconsole app) running on-premise in the factory (can be one app instance per factory orone instance per production line–your choice) 
  - IoT services in Azure that handle data processing, storage, calculations, business logic, etc.
  - 
System must be able to connect multiple production linesworking and transmitting data in parallel, preferably by starting just another instance of the Agent orby changing its configuration (e.g. adding another connection string).
_________________________________
# Antoni Król *396485*
