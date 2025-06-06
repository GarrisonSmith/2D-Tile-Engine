﻿using Engine.Physics.Models.Contracts;
using Microsoft.Xna.Framework;
using System.Linq;

namespace Engine.Physics.Models
{
	/// <summary>
	/// Represents a area collection
	/// </summary>
	public class AreaCollection(float width, float height) : IAmAArea
	{	
		/// <summary>
		/// Gets the width.
		/// </summary>
		public float Width { get; } = width;

		/// <summary>
		/// Gets the height.
		/// </summary>
		public float Height { get; } = height;

		/// <summary>
		/// Gets or sets the position.
		/// </summary>
		public Position Position { get; set; }
	
		/// <summary>
		/// Gets or sets the areas.
		/// </summary>
		public OffsetArea[] Areas { get; set; }

		/// <summary>
		/// Determines if a the area contains the coordinate.
		/// </summary>
		/// <param name="coordinate">The coordinate.</param>
		/// <returns>A value indicating whether the area contains the coordinate.</returns>
		public bool Contains(Vector2 coordinate)
		{
			return this.Areas.Any(e => e.Contains(coordinate));
		}
	}
}
