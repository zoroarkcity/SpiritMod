﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;

namespace SpiritMod.Prim
{
	public class ChampagnePrimTrail : PrimTrail
	{
		public ChampagnePrimTrail(Projectile projectile, Color color, int width = 12, int cap = 20)
		{
			_projectile = projectile;
			_entitytype = projectile.type;
			_drawtype = PrimTrailManager.DrawProjectile;
			_color = color;
			_width = width;
			_cap = cap;
		}

		public override void SetDefaults() => _alphaValue = 0.8f;

		public override void PrimStructure(SpriteBatch spriteBatch)
		{
			/*if (_noOfPoints <= 1) return; //for easier, but less customizable, drawing
			float colorSin = (float)Math.Sin(_counter / 3f);
			Color c1 = Color.Lerp(Color.White, Color.Cyan, colorSin);
			float widthVar = (float)Math.Sqrt(_points.Count) * _width;
			DrawBasicTrail(c1, widthVar);*/

			if (_noOfPoints <= 6) 
				return;

			for (int i = 0; i < _points.Count; i++) {
				float widthVar;

				if (i == 0) {
					widthVar = _width * (float) Math.Sqrt(i / (float) _points.Count);

					Vector2 normalAhead = CurveNormal(_points, i + 1);
					Vector2 secondUp = _points[i + 1] - normalAhead * widthVar;
					Vector2 secondDown = _points[i + 1] + normalAhead * widthVar;

					AddVertex(_points[i], _color * _alphaValue, Vector2.Zero);
					AddVertex(secondUp, _color * _alphaValue, Vector2.Zero);
					AddVertex(secondDown, _color * _alphaValue, Vector2.Zero);
				}
				else {
					if (i == _points.Count - 1)
						continue;

					widthVar = _width * (float) Math.Sqrt(i / (float) _points.Count);

					Vector2 normal = CurveNormal(_points, i);
					Vector2 normalAhead = CurveNormal(_points, i + 1);
					Vector2 firstUp = _points[i] - normal * widthVar;
					Vector2 firstDown = _points[i] + normal * widthVar;
					Vector2 secondUp = _points[i + 1] - normalAhead * widthVar;
					Vector2 secondDown = _points[i + 1] + normalAhead * widthVar;

					AddVertex(firstDown, _color * _alphaValue, new Vector2(i / (float) _points.Count, 1));
					AddVertex(firstUp, _color * _alphaValue, new Vector2(i / (float) _points.Count, 0));
					AddVertex(secondDown, _color * _alphaValue, new Vector2((i + 1) / (float) _points.Count, 1));

					AddVertex(secondUp, _color * _alphaValue, new Vector2((i + 1) / (float) _points.Count, 0));
					AddVertex(secondDown, _color * _alphaValue, new Vector2((i + 1) / (float) _points.Count, 1));
					AddVertex(firstUp, _color * _alphaValue, new Vector2(i / (float) _points.Count, 0));
				}
			}
		}

		public override void SetShaders() => 
			PrepareShader(SpiritMod.ScreamingSkullTrail, "MainPS", _counter / 2f);

		public override void OnUpdate()
		{
			_counter++;
			_noOfPoints = _points.Count * 6;

			if (_width < 20)
				_width *= 1.01f;
			if (_cap < _noOfPoints / 6)
				_points.RemoveAt(0);

			if (!_projectile.active && _projectile != null || _destroyed)
				OnDestroy();
			else
				_points.Add(_projectile.Center);
		}

		public override void OnDestroy()
		{
			_destroyed = true;

			if (_points.Count < 3)
				Dispose();
			else
				_points.RemoveAt(0);
		}
	}
}