#pragma warning disable 8600, 8602, 8604

using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Text.Json;
using Serilog;
using Gaos.Auth;
using Gaos.Dbo;
using Gaos.Routes.Model.ChatRoomJson;
using Gaos.Dbo.Model;

namespace Gaos.Routes
{

    public static class ChatRoomRoutes
    {
        public static int MAX_NUMBER_OF_MESSAGES_IN_ROOM = 100;

        public static string CLASS_NAME = typeof(ChatRoomRoutes).Name;
        public static RouteGroupBuilder GroupChatRoom(this RouteGroupBuilder group)
        {
            group.MapGet("/hello", (Db db) => "hello");

            group.MapPost("/writeMessage", async (WriteMessageRequest writeMessageRequest, Db db, Gaos.Common.UserService userService) =>
            {
                const string METHOD_NAME = "chatRoom/writeMessage";
                try
                {
                    WriteMessageResponse response;

                    if (writeMessageRequest.Message == null || writeMessageRequest.Message.Trim() == "")
                    {
                        response = new WriteMessageResponse
                        {
                            IsError = true,
                            ErrorMessage = "Message is empty",

                        };
                        return Results.Json(response);
                    }

                    // Verify that ChatRoomId exists
                    ChatRoom chatRoom = await db.ChatRoom.FirstOrDefaultAsync(x => x.Id == writeMessageRequest.ChatRoomId);

                    if (chatRoom == null)
                    {
                        response = new WriteMessageResponse
                        {
                            IsError = true,
                            ErrorMessage = "ChatRoomId - no such chat room",

                        };
                        return Results.Json(response);

                    }

                    using (var transaction = db.Database.BeginTransaction())
                    {
                        try
                        {
                            // Read minimal and maximal message id
                            int? minMessageId = await db.ChatRoomMessage.Where(x => x.ChatRoomId == writeMessageRequest.ChatRoomId).MinAsync(x => x.MessageId);
                            if (minMessageId == null)
                            {
                                minMessageId = 0;
                            }
                            int? maxMessageId = await db.ChatRoomMessage.Where(x => x.ChatRoomId == writeMessageRequest.ChatRoomId).MaxAsync(x => x.MessageId);
                            if (maxMessageId == null)
                            {
                                maxMessageId = 0;
                            }

                            // Remove messages if there are more than MAX_NUMBER_OF_MESSAGES_IN_ROOM 
                            if (maxMessageId - minMessageId > MAX_NUMBER_OF_MESSAGES_IN_ROOM)
                            {
                                await db.Database.ExecuteSqlRawAsync("DELETE FROM ChatRoomMessage WHERE ChatRoomId = {0} AND MesageId < {1}", writeMessageRequest.ChatRoomId, maxMessageId - MAX_NUMBER_OF_MESSAGES_IN_ROOM);
                            }

                            // Verify if user is a member of the chat room
                            ChatRoomMember chatRoomMember = await db.ChatRoomMember.FirstOrDefaultAsync(x => x.ChatRoomId == writeMessageRequest.ChatRoomId && x.UserId == userService.GetUserId());
                            if (chatRoomMember == null)
                            {
                                transaction.Rollback();
                                response = new WriteMessageResponse
                                {
                                    IsError = true,
                                    ErrorMessage = "user is not a member of chat room",

                                };
                                return Results.Json(response);

                            }

                            Gaos.Dbo.Model.User user = userService.GetUser();

                            // Create new message
                            ChatRoomMessage chatRoomMessage = new ChatRoomMessage
                            {
                                ChatRoomId = writeMessageRequest.ChatRoomId,
                                UserId = user.Id,
                                ChatRoomMemberName = user.Name,
                                MessageId = (int)(maxMessageId + 1),
                                Message = writeMessageRequest.Message,
                                CreatedAt = System.DateTime.UtcNow,
                            };

                            // Save message
                            await db.ChatRoomMessage.AddAsync(chatRoomMessage);
                            await db.SaveChangesAsync();

                            // Commit transaction
                            transaction.Commit();

                            // Return response
                            response = new WriteMessageResponse
                            {
                                IsError = false,
                            };
                            return Results.Json(response);


                        }
                        catch (Exception ex)
                        {
                            transaction.Rollback();
                            Log.Error(ex, $"{CLASS_NAME}:{METHOD_NAME}: error: {ex.Message}");
                            response = new WriteMessageResponse
                            {
                                IsError = true,
                                ErrorMessage = "internal error",
                            };
                            return Results.Json(response);
                        }
                    }
                }
                catch (Exception ex) 
                { 
                    Log.Error(ex, $"{CLASS_NAME}:{METHOD_NAME}: error: {ex.Message}");
                    WriteMessageResponse response = new WriteMessageResponse
                    {
                        IsError = true,
                        ErrorMessage = "internal error 1",
                    };
                    return Results.Json(response);
                }
                
            });

            group.MapPost("/readMessages", async (ReadMessagesRequest readMessagesRequest, Db db, Gaos.Common.UserService userService) =>
            {
                const string METHOD_NAME = "chatRoom/readMessages";
                ReadMessagesResponse response;
                try
                {
                    // Verify that ChatRoomId exists
                    ChatRoom chatRoom = await db.ChatRoom.FirstOrDefaultAsync(x => x.Id == readMessagesRequest.ChatRoomId);

                    if (chatRoom == null)
                    {
                        response = new ReadMessagesResponse
                        {
                            IsError = true,
                            ErrorMessage = "ChatRoomId - no such chat room",

                        };
                        return Results.Json(response);

                    }

                    // Verify if user is a member of the chat room
                    ChatRoomMember chatRoomMember = await db.ChatRoomMember.FirstOrDefaultAsync(x => x.ChatRoomId == readMessagesRequest.ChatRoomId && x.UserId == userService.GetUserId());
                    if (chatRoomMember == null)
                    {
                        response = new ReadMessagesResponse
                        {
                            IsError = true,
                            ErrorMessage = "user is not a member of chat room",
                        };
                        return Results.Json(response);
                    }

                    // Read messages
                    ChatRoomMessage[] chatRoomMessages = await db.ChatRoomMessage.Where(x => x.ChatRoomId == readMessagesRequest.ChatRoomId && x.MessageId > readMessagesRequest.LastMessageId).OrderBy(x => x.MessageId).ToArrayAsync();

                    ResponseMessage[] messages = new ResponseMessage[chatRoomMessages.Length];
                    for (int i = 0; i < chatRoomMessages.Length; i++)
                    {
                        messages[i] = new ResponseMessage
                        {
                            MessageId = chatRoomMessages[i].MessageId,
                            Message = chatRoomMessages[i].Message,
                            CreatedAt = chatRoomMessages[i].CreatedAt,
                            UserId = chatRoomMessages[i].UserId,
                            UserName = chatRoomMessages[i].ChatRoomMemberName,
                        };
                    }

                    // Return response
                    response = new ReadMessagesResponse
                    {
                        IsError = false,
                        Messages = messages,
                    };
                    return Results.Json(response);

                    
                }
                catch (Exception ex)
                {
                    Log.Error(ex, $"{CLASS_NAME}:{METHOD_NAME}: error: {ex.Message}");
                    response = new ReadMessagesResponse
                    {
                        IsError = true,
                        ErrorMessage = "internal error",
                    };
                    return Results.Json(response);
                }
            });

            group.MapPost("/readMessagesBackwards", async (ReadMessagesBackwardsRequest readMessagesBackwardsRequest, Db db, Gaos.Common.UserService userService) =>
            {
                const string METHOD_NAME = "chatRoom/readMessagesBackwards";
                ReadMessagesBackwardsResponse response;
                try
                {
                    // Verify that ChatRoomId exists
                    ChatRoom chatRoom = await db.ChatRoom.FirstOrDefaultAsync(x => x.Id == readMessagesBackwardsRequest.ChatRoomId);

                    if (chatRoom == null)
                    {
                        response = new ReadMessagesBackwardsResponse
                        {
                            IsError = true,
                            ErrorMessage = "ChatRoomId - no such chat room",

                        };
                        return Results.Json(response);

                    }

                    // Verify if user is a member of the chat room
                    ChatRoomMember chatRoomMember = await db.ChatRoomMember.FirstOrDefaultAsync(x => x.ChatRoomId == readMessagesBackwardsRequest.ChatRoomId && x.UserId == userService.GetUserId());
                    if (chatRoomMember == null)
                    {
                        response = new ReadMessagesBackwardsResponse
                        {
                            IsError = true,
                            ErrorMessage = "user is not a member of chat room",
                        };
                        return Results.Json(response);
                    }

                    int lastMessageId;
                    if (readMessagesBackwardsRequest.LastMessageId == null || readMessagesBackwardsRequest.LastMessageId <= 0)
                    {
                        lastMessageId = Int32.MaxValue;
                    }
                    else
                    {
                        lastMessageId = readMessagesBackwardsRequest.LastMessageId.Value;
                    }

                    // Read messages
                    ChatRoomMessage[] chatRoomMessages = await db.ChatRoomMessage.Where(x => x.ChatRoomId == readMessagesBackwardsRequest.ChatRoomId && x.MessageId < lastMessageId).OrderByDescending(x => x.MessageId).ToArrayAsync();

                    ResponseMessage[] messages = new ResponseMessage[chatRoomMessages.Length];
                    for (int i = 0; i < chatRoomMessages.Length; i++)
                    {
                        messages[i] = new ResponseMessage
                        {
                            MessageId = chatRoomMessages[i].MessageId,
                            Message = chatRoomMessages[i].Message,
                            CreatedAt = chatRoomMessages[i].CreatedAt,
                            UserId = chatRoomMessages[i].UserId,
                            UserName = chatRoomMessages[i].ChatRoomMemberName,
                        };
                    }

                    // Return response
                    response = new ReadMessagesBackwardsResponse
                    {
                        IsError = false,
                        Messages = messages,
                    };
                    return Results.Json(response);

                    
                }
                catch (Exception ex)
                {
                    Log.Error(ex, $"{CLASS_NAME}:{METHOD_NAME}: error: {ex.Message}");
                    response = new ReadMessagesBackwardsResponse
                    {
                        IsError = true,
                        ErrorMessage = "internal error",
                    };
                    return Results.Json(response);
                }
            });



            return group;

        }
    }
}
