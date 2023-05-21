using System.Linq;
using Microsoft.EntityFrameworkCore;
using UniChatApplication.Data;
using UniChatApplication.Models;

namespace UniChatApplication.Daos
{

    public class RoomMessagePinDAOs
    {
        /// <summary>
        /// Get All RoomMessagePin of Room
        /// </summary>
        /// <param name="context"></param>
        /// <returns>List of RoomMessagePin</returns>
        public static IQueryable<RoomMessagePin> GetAll(UniChatDbContext context)
        {
            return context.RoomMessagePins.Include(m => m.RoomMessage);
        }
        /// <summary>
        /// Get All MessagePin of Room
        /// </summary>
        /// <param name="context"></param>
        /// <param name="RoomId"></param>
        /// <returns>List of RoomMessagePin</returns>
        public static IQueryable<RoomMessagePin> GetAllMessagePinOfRoom(UniChatDbContext context, int RoomId)
        {

            return GetAll(context).Where(m => m.RoomMessage.RoomID == RoomId);

        }
        /// <summary>
        /// Get Class MessagePin Of Room
        /// </summary>
        /// <param name="context"></param>
        /// <param name="RoomId"></param>
        /// <returns>RoomMessagePin</returns>
        public static RoomMessagePin GetMessagePinOfRoom(UniChatDbContext context, int RoomId)
        {
            return GetAllMessagePinOfRoom(context, RoomId)
                    .OrderBy(m => m.Time)
                    .LastOrDefault();
        }

    }

}