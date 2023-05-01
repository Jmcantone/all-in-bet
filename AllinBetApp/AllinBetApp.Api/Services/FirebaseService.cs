using AllinBetApp.Api.Models;
using Firebase.Database;
using Firebase.Database.Query;
using Newtonsoft.Json;

namespace AllinBetApp.Api.Services
{
    public class FirebaseService
    {
        private readonly FirebaseClient _firebaseClient;

        public FirebaseService(string firebaseUrl)
        {
            _firebaseClient = new FirebaseClient(firebaseUrl);
        }

        public async Task<bool> StartGame(string roomId)
        {
            var roomSnapshot = await _firebaseClient.Child("rooms").Child(roomId).OnceSingleAsync<Room>();
            if (roomSnapshot == null)
            {
                return false;
            }

            var room = roomSnapshot;
            room.GameInProgress = true;
            await _firebaseClient.Child("rooms").Child(roomId).PutAsync(JsonConvert.SerializeObject(room));
            return true;
        }

        public async Task<bool> EndGame(string roomId)
        {
            var roomSnapshot = await _firebaseClient.Child("rooms").Child(roomId).OnceSingleAsync<Room>();
            if (roomSnapshot == null)
            {
                return false;
            }

            var room = roomSnapshot;
            room.GameInProgress = false;
            await _firebaseClient.Child("rooms").Child(roomId).PutAsync(JsonConvert.SerializeObject(room));
            return true;
        }

        public async Task<string> CreateRoom(Room room)
        {
            var roomKey = (await _firebaseClient.Child("rooms").PostAsync(JsonConvert.SerializeObject(room))).Key;
            return roomKey;
        }

        public async Task<bool> JoinRoom(string roomId, Player player)
        {
            var roomSnapshot = await _firebaseClient.Child("rooms").Child(roomId).OnceSingleAsync<Room>();
            if (roomSnapshot == null)
            {
                return false;
            }

            var room = roomSnapshot;
            room.Players.Add(player);
            await _firebaseClient.Child("rooms").Child(roomId).PutAsync(JsonConvert.SerializeObject(room));
            return true;
        }

        public async Task<bool> LeaveRoom(string roomId, string playerId)
        {
            var roomSnapshot = await _firebaseClient.Child("rooms").Child(roomId).OnceSingleAsync<Room>();
            if (roomSnapshot == null)
            {
                return false;
            }

            var room = roomSnapshot;
            room.Players.RemoveAll(p => p.Id == playerId);
            await _firebaseClient.Child("rooms").Child(roomId).PutAsync(JsonConvert.SerializeObject(room));
            return true;
        }
    }
}
