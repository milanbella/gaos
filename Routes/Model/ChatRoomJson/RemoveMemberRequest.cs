﻿#pragma warning disable 8632
using System.Collections.Generic;

namespace Gaos.Routes.Model.ChatRoomJson
{
    [System.Serializable]
    public class RemoveMemberRequest
    {
        public int ChatRoomId { get; set; }
        public int UserId { get; set; }
    }
}
