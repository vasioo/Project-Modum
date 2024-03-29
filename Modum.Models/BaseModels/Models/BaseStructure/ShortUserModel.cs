﻿using Modum.Models.BaseModels.Interfaces;

namespace Modum.Models.BaseModels.Models.BaseStructure
{
    public class ShortUserModel : IEntity
    {
        public int Id { get; set; } = 0;
        public string UserId { get; set; } = "";
        public string ReasonOfBanning { get; set; } = "";
        public DateTime DateOfBan { get; set; } = DateTime.Now;
    }
}
