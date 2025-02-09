# Raw WebSockets in ASP.NET Core

This code sample is a demo web app that uses raw WebSockets in ASP.NET Core. It’s a Visual Studio Solution with an ASP.NET Core Web API project targeting .NET 9.0.

The solution is a basic web application that handles incoming web sockets connections and periodically pushes the server resources information (e.g. CPU, Memory used) to the connected clients. This example also includes a simple Echo request/response message handler that will just send back the message received from the client.

For more context, see the article [Raw WebSocket in ASP.NET Core : 5 Things You Need to Know](https://www.siakabaro.com/raw-websocket-in-csharp-aspnet-core/)  on https://www.siakabaro.com.

