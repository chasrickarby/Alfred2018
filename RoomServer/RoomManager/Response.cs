///-----------------------------------------------------------------
/// <summary>
/// Response information object.
/// </summary>
///-----------------------------------------------------------------

namespace RoomManager
{
    public class Response
    {
        public Response(string message, bool success)
        {
            Message = message;
            Success = success;
        }

        public string Message { get; private set; }
        public bool Success { get; private set; }
    }
}