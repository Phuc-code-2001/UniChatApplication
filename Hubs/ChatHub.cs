using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using UniChatApplication.Data;
using UniChatApplication.Models;
using UniChatApplication.Daos;
using System.Linq;
using Microsoft.AspNetCore.Http;
using UniChatApplication.Controllers;
using Microsoft.EntityFrameworkCore;
using System;
using Microsoft.AspNetCore.Mvc;

namespace UniChatApplication.Hubs
{
    class ChatHub : Hub
    {
    
        /// <summary>
        /// Allow client side join to a room
        /// </summary>
        /// <param name="id"></param>
        public async Task JoinRoom(int id){

            await Groups.AddToGroupAsync(Context.ConnectionId, $"RoomChat-{id}");
            System.Console.WriteLine($"{Context.ConnectionId} Joined RoomChat {id}");
            
        }

        /// <summary>
        /// Allow client side join to a group
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task JoinGroup(int id){

            await Groups.AddToGroupAsync(Context.ConnectionId, $"GroupChat-{id}");
            System.Console.WriteLine($"{Context.ConnectionId} Joined GroupChat {id}");
            
        }

        /// <summary>
        /// Use to send the message to clients in a room
        /// </summary>
        /// <param name="id"></param>
        /// <param name="roomId"></param>
        /// <param name="username"></param>
        /// <param name="avatar"></param>
        /// <param name="message"></param>
        /// <param name="time"></param>
        public async Task SendRoomMessage(int id, int roomId, string username, string avatar, string message, string time)
        {

            await Clients.Group($"RoomChat-{roomId}").SendAsync(
                    "GetRoomMessage",
                    username,
                    message,
                    id,
                    avatar,
                    time
            );
        }

        /// <summary>
        /// Use to send the message to clients in a group
        /// </summary>
        /// <param name="id"></param>
        /// <param name="groupId"></param>
        /// <param name="username"></param>
        /// <param name="avatar"></param>
        /// <param name="message"></param>
        /// <param name="time"></param>
        public async Task SendGroupMessage(int id, int groupId, string username, string avatar, string message, string time)
        {

            await Clients.Group($"GroupChat-{groupId}").SendAsync(
                    "GetGroupMessage",
                    username,
                    message,
                    id,
                    avatar,
                    time
            );
        }
    }
}