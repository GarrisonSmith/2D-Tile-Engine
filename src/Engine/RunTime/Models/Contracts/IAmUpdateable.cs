﻿using Microsoft.Xna.Framework;

namespace Engine.RunTime.Models.Contracts
{
	/// <summary>
	/// Represents something updateable.
	/// </summary>
	public interface IAmUpdateable
	{
		/// <summary>
		/// Gets or sets the update order.
		/// </summary>
		public int UpdateOrder { get; set; }

		/// <summary>
		/// Updates the updateable.
		/// </summary>
		/// <param name="gameTime">The game time.</param>
		/// <param name="gameServices">The game services.</param>
		public void Update(GameTime gameTime, GameServiceContainer gameServices);
	}
}
