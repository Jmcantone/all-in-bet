using AllinBetApp.Api.Services;
using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;

namespace AllinBetApp.Api.Controllers
{
    [ApiController]
    //[Route("[controller]")]
    public class WebSocketController : ControllerBase
    {
        private readonly RequestDelegate _next;
        private readonly WebSocketService _webSocketService;
        private readonly ILogger<WebSocketController> _logger;

        public WebSocketController(RequestDelegate next, WebSocketService webSocketService, ILogger<WebSocketController> logger)
        {
            _next = next;
            _webSocketService = webSocketService;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            if (context.WebSockets.IsWebSocketRequest)
            {
                WebSocket webSocket = await context.WebSockets.AcceptWebSocketAsync();
                await _webSocketService.HandleWebSocketAsync(webSocket);
            }
            else
            {
                await _next(context);
            }
        }
    }
}