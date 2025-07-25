﻿using Engine.Controls.Enums;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Engine.Controls.Models
{
    /// <summary>
    /// Represents a control state.
    /// </summary>
    public class ControlState
	{
		/// <summary>
		/// Gets or sets the direction.
		/// </summary>
		public float? Direction { get; set; }

		/// <summary>
		/// Gets or sets the mouse state.
		/// </summary>
		public MouseState MouseState { get; set; }

		/// <summary>
		/// Gets or sets the fresh action types. 
		/// </summary>
		public List<ActionTypes> FreshActionTypes { get; set; }

		/// <summary>
		/// Gets or sets the action types.
		/// </summary>
		public List<ActionTypes> ActionTypes { get; set; }
	}
}
