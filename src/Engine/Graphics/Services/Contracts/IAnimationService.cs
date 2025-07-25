﻿using Engine.DiskModels.Drawing;
using Engine.Graphics.Models;

namespace Engine.Graphics.Services.Contracts
{
	/// <summary>
	/// Represents a animation service.
	/// </summary>
	public interface IAnimationService
	{
		/// <summary>
		/// Gets the animation.
		/// </summary>
		/// <param name="animationModel">The animation.</param>
		/// <param name="frameWidth">The frame width.</param>
		/// <param name="frameHeight">The frame height.</param>
		/// <returns>The animation.</returns>
		public Animation GetAnimation(AnimationModel animationModel, int frameWidth, int frameHeight);
	}
}
