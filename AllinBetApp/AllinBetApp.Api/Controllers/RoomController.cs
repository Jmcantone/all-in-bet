using AllinBetApp.Api.Models;
using AllinBetApp.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace AllinBetApp.Api.Controllers
{
    [ApiController]
    [Route("room")]
    public class RoomController : ControllerBase
    {
        private readonly FirebaseService _firebaseService;

        public RoomController(FirebaseService firebaseService)
        {
            _firebaseService = firebaseService;
        }

        [HttpPost("create")]
        public IActionResult CreateRoom([FromBody] Room room)
        {
            if (!string.IsNullOrEmpty(_firebaseService.CreateRoom(room).Result))
            {
                return Ok(new { RoomId = room.Id });
            }
            return BadRequest("No se pudo crear la sala");
        }

        [HttpPost("join")]
        public async Task<IActionResult> JoinRoomAsync([FromBody] string roomId, Player player)
        {
            if (await _firebaseService.JoinRoom(roomId, player))
            {
                return Ok("Jugador unido a la sala");
            }
            return BadRequest("No se pudo unir a la sala");
        }

        [HttpPost("leave")]
        public async Task<IActionResult> LeaveRoom([FromBody] string roomId, Player player)
        {
            if (await _firebaseService.LeaveRoom(roomId, player.Id))
            {
                return Ok("Jugador abandonó la sala");
            }
            return BadRequest("No se pudo abandonar la sala");
        }
    }
}