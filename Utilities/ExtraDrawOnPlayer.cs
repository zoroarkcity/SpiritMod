using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using Terraria;
using Terraria.DataStructures;
using Terraria.ModLoader;

namespace SpiritMod.Utilities
{
	/// <summary>
	/// ModPlayer class managing extra additive or alphablend draw calls on top of a player, due to the inflexibility of playerlayers
	/// </summary>
	public class ExtraDrawOnPlayer : ModPlayer
	{
		public enum DrawType
		{
			AlphaBlend,
			Additive
		}

		public delegate void DrawAction(SpriteBatch spriteBatch);

		public IDictionary<DrawAction, DrawType> DrawDict = new Dictionary<DrawAction, DrawType>();

		public override void ResetEffects() => DrawDict = new Dictionary<DrawAction, DrawType>();

		/// <summary>
		/// Check if any of the draw calls on the player have the specified draw type
		/// </summary>
		/// <param name="Type"></param>
		/// <returns></returns>
		public bool AnyOfType(DrawType Type)
		{
			foreach (KeyValuePair<DrawAction, DrawType> kvp in DrawDict)
				if (kvp.Value == Type)
					return true;

			return false;
		}

		/// <summary>
		/// Draw all draw calls on the player with a specified draw type
		/// </summary>
		/// <param name="spriteBatch"></param>
		/// <param name="Type"></param>
		public void DrawAllCallsOfType(SpriteBatch spriteBatch, DrawType Type)
		{
			foreach (KeyValuePair<DrawAction, DrawType> kvp in DrawDict)
				if (kvp.Value == Type)
					kvp.Key.Invoke(spriteBatch);
		}
	}
}