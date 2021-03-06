﻿using Muzziq.Models.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Muzziq.Models.RoomViewModels
{
    public class CreateRoomViewModel
    {
        public CreateRoomViewModel(List<Song> avalialbeSongs)
        {
            AvailableSongs = avalialbeSongs;
        }
        public List<Song> AvailableSongs { get; set; }
    }
}
