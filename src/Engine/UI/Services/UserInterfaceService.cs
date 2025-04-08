﻿using Engine.DiskModels.UI;
using Engine.Drawables.Services.Contracts;
using Engine.Physics.Models;
using Engine.RunTime.Services.Contracts;
using Engine.UI.Models;
using Engine.UI.Models.Constants;
using Engine.UI.Models.Contracts;
using Engine.UI.Models.Elements;
using Engine.UI.Models.Enums;
using Engine.UI.Services.Contracts;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;

namespace Engine.UI.Services
{
	/// <summary>
	/// Represents a user interface service.
	/// </summary>
	/// <remarks>
	/// Initialize the user interface service.
	/// </remarks>
	/// <param name="gameServices">The game services.</param>
	public class UserInterfaceService(GameServiceContainer gameServices) : IUserInterfaceService
	{
		private readonly GameServiceContainer _gameServices = gameServices;

		/// <summary>
		/// Gets the active visibility group id.
		/// </summary>
		public int? ActiveVisibilityGroupId { get; private set; }

		/// <summary>
		/// Gets or sets the user interface groups.
		/// </summary>
		public List<UiGroup> UserInterfaceGroups { get; set; } = [];

		/// <summary>
		/// Toggles the user interface group visibility.
		/// </summary>
		/// <param name="visibilityGroupId">The visibility group id.</param>
		public void ToggleUserInterfaceGroupVisibility(int visibilityGroupId)
		{
			var userInterfaceGroup = this.UserInterfaceGroups.FirstOrDefault(e => e.VisibilityGroupId == visibilityGroupId);
			this.ToggleUserInterfaceGroupVisibility(userInterfaceGroup);
		}

		/// <summary>
		/// Toggles the user interface group visibility.
		/// </summary>
		/// <param name="uiGroup">The user interface group.</param>
		public void ToggleUserInterfaceGroupVisibility(UiGroup uiGroup)
		{
			if (null == uiGroup)
			{
				return;
			}

			var runtimeDrawService = this._gameServices.GetService<IRuntimeDrawService>();

			if (true == this.ActiveVisibilityGroupId.HasValue)
			{
				var activeGroup = this.UserInterfaceGroups.FirstOrDefault(e => e.VisibilityGroupId == this.ActiveVisibilityGroupId);

				foreach (var uiZoneContainer in activeGroup.UiZones)
				{
					runtimeDrawService.RemoveOverlaidDrawable(uiZoneContainer.DrawLayer, uiZoneContainer);
				}
			}

			if (true == uiGroup.UiZones?.Any())
			{
				var animationService = this._gameServices.GetService<IAnimationService>();

				foreach (var uiZoneContainer in uiGroup.UiZones)
				{
					runtimeDrawService.AddOverlaidDrawable(uiZoneContainer.DrawLayer, uiZoneContainer);

					if (true != uiZoneContainer.ElementRows?.Any())
					{
						continue;
					}

					foreach (var uiRow in uiZoneContainer.ElementRows)
					{
						if (true != uiRow.SubElements?.Any())
						{
							continue;
						}

						foreach (var element in uiRow.SubElements)
						{
							if ((element is UiButton button) &&
								(null != button?.ClickAnimation))
							{
								animationService.ResetTriggeredAnimation(button.ClickAnimation);
							}
						}
					}
				}
			}

			this.ActiveVisibilityGroupId = uiGroup.VisibilityGroupId;
		}

		/// <summary>
		/// Gets the user interface element at the screen location.
		/// </summary>
		/// <param name="location">The location.</param>
		/// <returns>The user interface element at the location if one is found.</returns>
		public UiElementWithLocation GetUiElementAtScreenLocation(Vector2 location)
		{
			if (true != this.ActiveVisibilityGroupId.HasValue)
			{
				return null;
			}

			var activeUiGroup = this.UserInterfaceGroups.FirstOrDefault(e => e.VisibilityGroupId == this.ActiveVisibilityGroupId);
			var uiZone = activeUiGroup.UiZones.FirstOrDefault(e => e.Area.Contains(location));

			if ((null == uiZone) ||
				(true != uiZone.ElementRows.Any()))
			{
				return null;
			}

			var height = uiZone.ElementRows.Sum(e => e.Height + e.BottomPadding + e.TopPadding);
			var rowVerticalOffset = uiZone.JustificationType switch
			{
				UiZoneJustificationTypes.None => 0,
				UiZoneJustificationTypes.Center => (uiZone.Area.Height - height) / 2,
				UiZoneJustificationTypes.Top => 0,
				UiZoneJustificationTypes.Bottom => uiZone.Area.Height - height,
				_ => 0,
			};

			foreach (var elementRow in uiZone.ElementRows)
			{
				var rowTop = 0f;
				var rowBottom = 0f;

				switch (uiZone.JustificationType)
				{
					case UiZoneJustificationTypes.Bottom:
					case UiZoneJustificationTypes.Center:
					case UiZoneJustificationTypes.None:
					case UiZoneJustificationTypes.Top:
					default:
						rowVerticalOffset += elementRow.TopPadding;
						rowTop = rowVerticalOffset + uiZone.Position.Y;
						rowVerticalOffset += elementRow.Height;
						rowBottom = rowVerticalOffset + uiZone.Position.Y;
						rowVerticalOffset += elementRow.BottomPadding;
						break;
				}

				if ((rowTop <= location.Y) &&
					(rowBottom >= location.Y))
				{
					return this.GetUiElementAtScreenLocationInRow(uiZone.Position, elementRow, rowTop, location);
				}
			}

			return null;
		}

		/// <summary>
		/// Gets the user interface element at the screen location in the row.
		/// </summary>
		/// <param name="position">The position.</param>
		/// <param name="uiRow">The user interface row.</param>
		/// <param name="heightOffset">The height offset.</param>
		/// <param name="location">The location.</param>
		/// <returns>The user interface element at the location if one is found.</returns>
		private UiElementWithLocation GetUiElementAtScreenLocationInRow(Position position, UiRow uiRow, float heightOffset, Vector2 location)
		{
			var width = uiRow.SubElements.Sum(e => e.Area.X + e.LeftPadding + e.RightPadding);
			var elementHorizontalOffset = uiRow.HorizontalJustificationType switch
			{
				UiRowHorizontalJustificationTypes.None => 0,
				UiRowHorizontalJustificationTypes.Center => (uiRow.Width - width) / 2,
				UiRowHorizontalJustificationTypes.Left => 0,
				UiRowHorizontalJustificationTypes.Right => uiRow.Width - width,
				_ => 0,
			};

			var largestHeight = uiRow.SubElements.OrderByDescending(e => e.Area.Y)
												 .FirstOrDefault().Area.Y;

			foreach (var element in uiRow.SubElements)
			{
				var verticallyCenterOffset = 0f;

				switch (uiRow.VerticalJustificationType)
				{
					case UiRowVerticalJustificationTypes.Bottom:
						verticallyCenterOffset = (largestHeight - element.Area.Y);
						break;
					case UiRowVerticalJustificationTypes.Center:
						verticallyCenterOffset = (largestHeight - element.Area.Y) / 2;
						break;
					case UiRowVerticalJustificationTypes.None:
					case UiRowVerticalJustificationTypes.Top:
						break;
				}

				var elementRight = 0f;
				var elementLeft = 0f;

				switch (uiRow.HorizontalJustificationType)
				{
					case UiRowHorizontalJustificationTypes.Right:
					case UiRowHorizontalJustificationTypes.Center:
					case UiRowHorizontalJustificationTypes.None:
					case UiRowHorizontalJustificationTypes.Left:
					default:
						elementHorizontalOffset += element.LeftPadding;
						elementLeft = elementHorizontalOffset + position.X;
						elementHorizontalOffset += element.Area.X;
						elementRight = elementHorizontalOffset + position.X;
						elementHorizontalOffset += element.RightPadding;
						break;
				}

				if ((elementLeft <= location.X) &&
					(elementRight >= location.X))
				{
					var elementTop = verticallyCenterOffset + heightOffset;
					var elementBottom = elementTop + element.Area.Y;

					if ((elementTop <= location.Y) &&
						(elementBottom >= location.Y))
					{
						return new UiElementWithLocation
						{
							Element = element,
							Location = new Vector2(elementLeft, elementTop),
						};
					}
				}
			}

			return null;
		}

		/// <summary>
		/// Gets the user interface group.
		/// </summary>
		/// <param name="uiGroupModel">The user interface group model.</param>
		/// <returns>The user interface group.</returns>
		public UiGroup GetUiGroup(UiGroupModel uiGroupModel)
		{
			var uiZones = new List<UiZone>();

			foreach (var uiZoneElementModel in uiGroupModel.UiZoneElements)
			{
				var uiZoneElement = this.GetUiZone(uiZoneElementModel, uiGroupModel.VisibilityGroupId);

				if (null != uiZoneElement)
				{
					uiZones.Add(uiZoneElement);
				}
			}

			return new UiGroup
			{
				UiGroupName = uiGroupModel.UiGroupName,
				VisibilityGroupId = uiGroupModel.VisibilityGroupId,
				UiZones = uiZones
			};
		}

		/// <summary>
		/// Gets the user interface zone.
		/// </summary>
		/// <param name="uiZoneModel">The user interface model.</param>
		/// <param name="visibilityGroup">The visibility group of the user interface zone.</param>
		/// <returns>The user interface zone.</returns>
		public UiZone GetUiZone(UiZoneModel uiZoneModel, int visibilityGroup)
		{
			var uiZoneService = this._gameServices.GetService<IUserInterfaceScreenZoneService>();
			var uiElementService = this._gameServices.GetService<IUserInterfaceElementService>();

			if (false == uiZoneService.UserInterfaceScreenZones.TryGetValue((UiScreenZoneTypes)uiZoneModel.UiZoneType, out UiScreenZone uiScreenZone))
			{
				uiScreenZone = uiZoneService.UserInterfaceScreenZones[UiScreenZoneTypes.None];
			}

			var imageService = this._gameServices.GetService<IImageService>();
			var background = imageService.GetImage(uiZoneModel.BackgroundTextureName, (int)uiScreenZone.Area.Width, (int)uiScreenZone.Area.Height);
			var uiZone = new UiZone
			{
				UiZoneName = uiZoneModel.UiZoneName,
				DrawLayer = 0,
				JustificationType = (UiZoneJustificationTypes)uiZoneModel.JustificationType,
				Image = background,
				UserInterfaceScreenZone = uiScreenZone,
				ElementRows = []
			};

			if (true != uiZoneModel.ElementRows?.Any())
			{
				return uiZone;
			}

			var elementRowsFirstPass = new List<UiRow>();

			foreach (var elementRowModel in uiZoneModel.ElementRows)
			{
				var elementRow = this.GetUiRow(elementRowModel, uiScreenZone, visibilityGroup);

				if (null != elementRow)
				{
					elementRowsFirstPass.Add(elementRow);
				}
			}

			var currentTotalHeight = elementRowsFirstPass.Sum(e => e.Height);
			var elementRowsSecondPass = new List<UiRow>();

			foreach (var elementRow in elementRowsFirstPass)
			{
				var rowRealWidth = elementRow.SubElements.Sum(e => e.Area.X + e.LeftPadding + e.RightPadding);

				if ((true == elementRow.Flex) &&
					(currentTotalHeight < uiScreenZone.Area.Height) &&
					(elementRow.Width < rowRealWidth))
				{
					var flexedElementRows = this.GetFlexedUiRows(elementRow);
					var newHeight = (currentTotalHeight - elementRow.Height) + flexedElementRows.Sum(e => e.Height);

					if (newHeight < uiScreenZone.Area.Height)
					{
						currentTotalHeight = newHeight;
						elementRowsSecondPass.AddRange(flexedElementRows?.Where(e => null != e));
						
						continue;
					}
				}

				elementRowsSecondPass.Add(elementRow);
			}

			var fillRows = elementRowsSecondPass.Where(e => e.SubElements.Any(s => s.Area.Y == 0))
												.ToList();

			if (true == fillRows?.Any())
			{
				var fillHeight = (uiScreenZone.Area.Height - currentTotalHeight) / fillRows.Count();

				if (fillHeight < ElementSizesScalars.ExtraSmall.Y / 2)
				{ 
					fillHeight = ElementSizesScalars.ExtraSmall.Y / 2;
				}

				var fillElements = fillRows.SelectMany(e => e.SubElements)
										   .Where(e => e.Area.Y == 0);

				foreach (var fillElement in fillElements)
                {
					uiElementService.UpdateElementHeight(fillElement, fillHeight);
                }

				foreach (var fillRow in fillRows)
				{ 
					fillRow.Height = fillRow.SubElements.OrderByDescending(e => e.Area.Y)
														.FirstOrDefault().Area.Y;
				}
            }

			uiZone.ElementRows = elementRowsSecondPass;

			return uiZone;
		}

		/// <summary>
		/// Gets the user interface row.
		/// </summary>
		/// <param name="uiRowModel">The user interface row model.</param>
		/// <param name="uiZone">The user interface zone.</param>
		/// <param name="visibilityGroup">The visibility group of the user interface row.</param>
		/// <returns>The user interface row.</returns>
		public UiRow GetUiRow(UiRowModel uiRowModel, UiScreenZone uiZone, int visibilityGroup)
		{
			var uiElementService = this._gameServices.GetService<IUserInterfaceElementService>();
			var imageService = this._gameServices.GetService<IImageService>();
			var numberOfFillElements = 0;
			var subElements = new List<IAmAUiElement>();

			if (true != uiRowModel.SubElements?.Any())
			{
				return null;
			}

			var availableWidth = uiZone.Area.Width;

			foreach (var elementModel in uiRowModel.SubElements)
			{
				var elementMinWidth = elementModel.LeftPadding + elementModel.RightPadding;
				var elementSize = uiElementService.GetElementDimensions(uiZone, elementModel);

				if (true == elementSize.HasValue)
				{
					elementMinWidth += elementSize.Value.X;
				}
				else
				{
					numberOfFillElements++;
				}

				availableWidth -= elementMinWidth;
			}

			var fillWidth = 0 < numberOfFillElements
				? availableWidth / numberOfFillElements
				: availableWidth / uiRowModel.SubElements.Length;

			foreach (var elementModel in uiRowModel.SubElements)
			{
				var element = uiElementService.GetUiElement(elementModel, uiZone, fillWidth, visibilityGroup);

				if (null != element)
				{
					subElements.Add(element);
				}
			}

			var height = subElements.Where(e => null != e)
									.Select(e => e.Area.Y)
									.OrderDescending()
									.FirstOrDefault();
			var image = imageService.GetImage(uiRowModel.BackgroundTextureName, (int)uiZone.Area.Width, (int)height + uiRowModel.TopPadding + uiRowModel.BottomPadding);

			return new UiRow
			{
				UiRowName = uiRowModel.UiRowName,
				Width = uiZone.Area.Width,
				Height = height,
				TopPadding = uiRowModel.TopPadding,
				BottomPadding = uiRowModel.BottomPadding,
				HorizontalJustificationType = (UiRowHorizontalJustificationTypes)uiRowModel.HorizontalJustificationType,
				VerticalJustificationType = (UiRowVerticalJustificationTypes)uiRowModel.VerticalJustificationType,
				Image = image,
				SubElements = subElements
			};
		}

		/// <summary>
		/// Gets the flexed user interface rows.
		/// </summary>
		/// <param name="uiRow">The user interface rows.</param>
		/// <returns>The flexed user interface rows.</returns>
		private IList<UiRow> GetFlexedUiRows(UiRow uiRow)
		{
			if (true != uiRow?.SubElements.Any())
			{
				return [];
			}

			var flexedRows = new List<UiRow>();
			var currentWidth = 0f;
			var currentRow = new UiRow
			{
				UiRowName = uiRow.UiRowName,
				Flex = true,
				Width = uiRow.Width,
				TopPadding = 0,
				BottomPadding = 0,
				HorizontalJustificationType = uiRow.HorizontalJustificationType,
				VerticalJustificationType = uiRow.VerticalJustificationType,
				Image = uiRow.Image,
				SubElements = []
			};

			foreach (var element in uiRow.SubElements)
			{
				var elementWidth = element.Area.X + element.LeftPadding + element.RightPadding;

				if (currentWidth + elementWidth > uiRow.Width)
				{
					currentRow.Height = currentRow.SubElements.OrderByDescending(e => e.Area.Y)
															  .FirstOrDefault().Area.Y;
					currentWidth = 0;
					flexedRows.Add(currentRow);
					currentRow = new UiRow
					{
						UiRowName = uiRow.UiRowName,
						Flex = true,
						Width = uiRow.Width,
						TopPadding = uiRow.TopPadding > uiRow.BottomPadding ?
									 uiRow.BottomPadding :
									 uiRow.TopPadding,
						BottomPadding = 0,
						HorizontalJustificationType = uiRow.HorizontalJustificationType,
						VerticalJustificationType = uiRow.VerticalJustificationType,
						Image = uiRow.Image,
						SubElements = []
					};
				}

				currentRow.SubElements.Add(element);
				currentWidth += elementWidth;
			}

			if (true == currentRow.SubElements.Any())
			{
				currentRow.Height = currentRow.SubElements.OrderByDescending(e => e.Area.Y)
														  .FirstOrDefault().Area.Y;

				flexedRows.Add(currentRow);
			}

			return flexedRows;
		}
	}
}
