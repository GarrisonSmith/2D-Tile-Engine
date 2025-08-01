﻿using Common.Controls.CursorInteraction.Models;
using Microsoft.Xna.Framework;

namespace Common.Controls.CursorInteraction.Services.Contracts
{
	/// <summary>
	/// Represents a cursor interaction service.
	/// </summary>   
	public interface ICursorInteractionService
	{
		/// <summary>
		/// Gets the hover configuration.
		/// </summary>
		/// <typeparam name="T">The parent type.</typeparam>
		/// <param name="area">The area.</param>
		/// <param name="offset">The offset.</param>
		/// <returns>The hover configuration.</returns>
		public HoverConfiguration<T> GetHoverConfiguration<T>(Vector2 area, Vector2 offset = default);

		/// <summary>
		/// Gets the press configuration.
		/// </summary>
		/// <typeparam name="T">The parent type.</typeparam>
		/// <param name="area">The area.</param>
		/// <param name="offset">The offset.</param>
		/// <returns>The press configuration.</returns>
		public PressConfiguration<T> GetPressConfiguration<T>(Vector2 area, Vector2 offset = default);

		/// <summary>
		/// Gets the click configuration.
		/// </summary>
		/// <typeparam name="T">The parent type.</typeparam>
		/// <param name="area">The area.</param>
		/// <param name="offset">The offset.</param>
		/// <returns>The click configuration.</returns>
		public ClickConfiguration<T> GetClickConfiguration<T>(Vector2 area, Vector2 offset = default);
	}
}
