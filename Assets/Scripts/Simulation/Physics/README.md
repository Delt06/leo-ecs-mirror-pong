# sharpPhysics
> *Code is under construction, architecture and codestyle will change along with my experience, my knowledge and available ecs tech*

Simple C# 2D physics engine written on top of [LeoEcsLite](https://github.com/Leopotam/ecslite) framework just for training.

Look at Engine/Game/GameStartup.cs to understand what a hell is going on.  
Use LMB to grab objects, RMB to create attractor, MMB to create repulsor.

Project uses slightly modified versions of different ecslite extensions:  
- [Extended systems](https://github.com/Leopotam/ecslite-extendedsystems) to manage one-frame events
- [Dependency injection](https://github.com/Leopotam/ecslite-di) to manage common pools and shared data

P.S game can be laggy because visuals are rendered by CPU.
![](pictures/preview.png "Example")
