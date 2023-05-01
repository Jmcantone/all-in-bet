using AllinBetApp.Api.Models;
using AllinBetApp.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace AllinBetApp.Api.Controllers
{
    [ApiController]
    [Route("game")]
    public class GameController : ControllerBase
    {
        private readonly FirebaseService _firebaseService;

        public GameController(FirebaseService firebaseService)
        {
            _firebaseService = firebaseService;
        }

        [HttpPost("start")]
        public async Task<IActionResult> StartGame([FromBody] string roomId)
        {
            if (await _firebaseService.StartGame(roomId))
            {
                return Ok("Juego iniciado");
            }
            return BadRequest("No se pudo iniciar el juego");
        }

        [HttpPost("end")]
        public async Task<IActionResult> EndGame([FromBody] string roomId)
        {
            if (await _firebaseService.EndGame(roomId))
            {
                return Ok("Juego finalizado");
            }
            return BadRequest("No se pudo finalizar el juego");
        }
    }
}