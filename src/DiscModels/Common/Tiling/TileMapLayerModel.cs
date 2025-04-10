﻿using DiscModels.Common.Tiling.Contracts;
using System.Runtime.Serialization;

namespace DiscModels.Common.Tiling
{
    [DataContract(Name = "tileMapLayer")]
    public class TileMapLayerModel
    {
        [DataMember(Name = "layer", Order = 1)]
        public int Layer { get; set; }

        [DataMember(Name = "tiles", Order = 2)]
        public List<IAmATileModel> Tiles { get; set; }
    }
}
