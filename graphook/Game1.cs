﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Vector2 = Microsoft.Xna.Framework.Vector2;

namespace graphook
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        float bloomIntensity = 1.25f;
        float baseIntensity = 1.0f;
        float bloomSaturation = 1.5f;
        float baseSaturation = 1.0f;
        int dd;
        Effect bloomCombineEffect;
        public static Texture2D PixelTexture;
        private Random random;
        Texture2D crosshair;
        Effect fireShader;
        Texture2D fireTexture, firePalette;
        RenderTarget2D fireBuffer;
        GraphicsDeviceManager graphics;
        RenderTarget2D crosshairTarget;
        SpriteBatch spriteBatch;
        ParticleSystem particleSystem;
        waterController water;
        
        Collision collision1;
        Entity player;
        List<Collision> collisions = new List<Collision>();
        int mousex;
        int mousey;
        float hue;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            bloomCombineEffect = Content.Load<Effect>("BloomCombine");
            
            Texture2D whiteTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            whiteTexture.SetData(new[] { Microsoft.Xna.Framework.Color.White });
            crosshair = Content.Load<Texture2D>("crosshair2");
            crosshairTarget = new RenderTarget2D(GraphicsDevice, crosshair.Width, crosshair.Height);
            List<Texture2D> textures = new List<Texture2D>();
            List<Texture2D> textures2 = new List<Texture2D>();
            textures.Add(Content.Load<Texture2D>("amogus"));
            textures2.Add(Content.Load<Texture2D>("amogus2"));
            particleSystem = new ParticleSystem(textures, new Vector2(400, 240), 1);
            collisions.Add(new Collision(new Vector2(100, 100), 500, 20));
            water = new waterController(180, 3, whiteTexture);
            int springAmount = 70;
            random = new Random();
            dd = 0;
            
            
            player = new Entity(collisions, textures2);
            
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            KeyboardState newState = Keyboard.GetState();





            water.Update(gameTime, newState, random);
            player.particleSystem.EmitterLocation = new Vector2(player.dcl._a.X + player.dcl.Width / 2, player.dcl._a.Y);
            player.particleSystem.Update();
            particleSystem.EmitterLocation = new Vector2(Mouse.GetState().X, Mouse.GetState().Y);
            particleSystem.Update();
            hue += (float)gameTime.ElapsedGameTime.TotalSeconds * 200f;

            if (hue >= 360f) hue -= 360f;
            player.Update();
            base.Update(gameTime);
        }
        private Color ColorFromHSV(float hue, float saturation, float value)
        {
            int hi = (int)(hue / 60) % 6;
            float f = hue / 60 - (int)(hue / 60);

            float v = value * 255;
            float p = v * (1 - saturation);
            float q = v * (1 - f * saturation);
            float t = v * (1 - (1 - f) * saturation);

            return hi switch
            {
                0 => new Color(v / 255f, t / 255f, p / 255f),
                1 => new Color(q / 255f, v / 255f, p / 255f),
                2 => new Color(p / 255f, v / 255f, t / 255f),
                3 => new Color(p / 255f, q / 255f, v / 255f),
                4 => new Color(t / 255f, p / 255f, v / 255f),
                _ => new Color(v / 255f, p / 255f, q / 255f),
            };
        }
        protected override void Draw(GameTime gameTime)
        {
            
            GraphicsDevice.SetRenderTarget(crosshairTarget);
            GraphicsDevice.Clear(Color.Transparent);
            Color rainbowColor = ColorFromHSV(hue, 1f, 1f);

            spriteBatch.Begin();
            spriteBatch.Draw(crosshair, new Rectangle(0, 0, crosshair.Width, crosshair.Height), rainbowColor);
            spriteBatch.End();

            
            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(Color.Black);

            
            particleSystem.Draw(spriteBatch);

            spriteBatch.Begin();
            for (int i = 0; i < collisions.Count; i++)
            {
                collisions[i].Draw(spriteBatch);
            }
            player.dcl.Draw(spriteBatch);
            water.Draw(spriteBatch);
            mousex = Mouse.GetState().X;
            mousey = Mouse.GetState().Y;

            

            spriteBatch.End();


            bloomCombineEffect.Parameters["BloomIntensity"].SetValue(5.0f);
            bloomCombineEffect.Parameters["BaseIntensity"].SetValue(0.4f);
            bloomCombineEffect.Parameters["BloomSaturation"].SetValue(4.0f);
            bloomCombineEffect.Parameters["BaseSaturation"].SetValue(0.7f);

            bloomCombineEffect.Parameters["BloomSampler"].SetValue(crosshairTarget);
            bloomCombineEffect.Parameters["BaseSampler"].SetValue(crosshairTarget);

            
            spriteBatch.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend);
            bloomCombineEffect.CurrentTechnique.Passes[0].Apply();
            spriteBatch.Draw(crosshairTarget, new Vector2(mousex, mousey), Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }

    }
}