﻿using MagicVilla_VillaAPI.Models.DTO;

namespace MagicVilla_VillaAPI.Data
{
    public class VillaStore
    {
        public static List<VillaDTO> villaList = new List<VillaDTO>
            {
                new VillaDTO {Id=1, Name="Pool View", Occupancy=3, Sqft=300},
                new VillaDTO {Id=2, Name="Beach View", Occupancy = 4, Sqft = 500 }

            };
    }
}
