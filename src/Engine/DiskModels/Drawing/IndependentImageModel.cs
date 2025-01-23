﻿using Engine.DiskModels.Engine.Physics;
using System.Runtime.Serialization;

namespace Engine.DiskModels.Engine.Drawing
{
	[DataContract(Name = "independentImage")]
	public class IndependentImageModel : ImageModel
	{
		[DataMember(Name = "position", Order = 2)]
		public PositionModel Position { get; set; }
	}
}
