using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Transactions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.IO;
using Microsoft.Xna.Framework.Media;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using System.Text.Json;
using NVorbis;
namespace graphook
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        float bloomIntensity = 1.25f;
        float baseIntensity = 1.0f;
        float bloomSaturation = 1.5f;
        double pi = 3.1415926535;
        float baseSaturation = 1.0f;
        int dd;
        Effect bloomCombineEffect;
        public static Texture2D PixelTexture;
        fireSystem fire;

        Texture2D selector;
        private Random random;
        Texture2D crosshair;
        Effect fireShader;
        Texture2D fireTexture, firePalette;
        RenderTarget2D fireBuffer;
        GraphicsDeviceManager graphics;
        RenderTarget2D crosshairTarget;
        SpriteBatch spriteBatch;
        ParticleSystem_sin particleSystem;
        waterController water;
        float cloudXoffset = 0;
        Collision collision1;
        Entity player;
        Texture2D torch;
        List<clsData> cls;
        KeyboardState newState = Keyboard.GetState();
        List<Collision> collisions = new List<Collision>();
        int mousex;
        int mousey;
        float hue;
        Effect saturationEffect;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            // Set the preferred resolution before calling base.Initialize()
            graphics.PreferredBackBufferWidth = 960;  // Set width
            graphics.PreferredBackBufferHeight = 540;  // Set height
            graphics.ApplyChanges();
            base.Initialize();
        }


        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            bloomCombineEffect = Content.Load<Effect>("BloomCombine");
            saturationEffect = Content.Load<Effect>("SaturationShader");
            Texture2D whiteTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            whiteTexture.SetData(new[] { Microsoft.Xna.Framework.Color.White });
            crosshair = Content.Load<Texture2D>("crosshair2");
            selector = Content.Load<Texture2D>("selector");
            crosshairTarget = new RenderTarget2D(GraphicsDevice, crosshair.Width, crosshair.Height);
            List<Texture2D> textures = new List<Texture2D>();
            List<Texture2D> textures2 = new List<Texture2D>();
            fireTexture = Content.Load<Texture2D>("fire1");
            torch = Content.Load<Texture2D>("torch2");
            textures.Add(fireTexture);
            textures2.Add(Content.Load<Texture2D>("amogus2"));
            particleSystem = new ParticleSystem_sin(textures, new Vector2(400, 240));
            fire = new fireSystem(textures, new Vector2(400, 240));
            Debug.WriteLine("Current directory: " + Directory.GetCurrentDirectory());
            string json = File.ReadAllText("collisions.json");
            cls = JsonSerializer.Deserialize<List<clsData>>(json);
            


            water = new waterController(330, 3, whiteTexture);
            int springAmount = 70;
            random = new Random();
            dd = 0;


            player = new Entity(collisions, textures2, spriteBatch);
            player.dcl.player = player;
            foreach (var cl in cls)
            {
                collisions.Add(new Collision(new Vector2(cl.X, cl.Y), cl.Width, cl.Height, player.dcl.Position));
            }
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                this.Exit();
            newState = Keyboard.GetState();

            mousex = Mouse.GetState().X;
            mousey = Mouse.GetState().Y;

            if (newState.IsKeyDown(Keys.R)) {
                player.dcl.Position = new Vector2(90, 300);
            }

            water.Update(gameTime, newState, random);
            cloudXoffset += 0.3f;
            fire.Update(0);

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
            GraphicsDevice.Clear(new Color(68, 179, 248, 255));
            Color rainbowColor = ColorFromHSV(hue, 1f, 1f);

            spriteBatch.Begin();
            spriteBatch.Draw(crosshair, new Rectangle(0, 0, crosshair.Width, crosshair.Height), rainbowColor);
            spriteBatch.End();







            GraphicsDevice.SetRenderTarget(null);
            GraphicsDevice.Clear(new Color(68, 179, 248, 255));

            fire.Draw(spriteBatch);

            particleSystem.EmitterLocation = new Vector2(cloudXoffset, 75);
            //particleSystem.Update(0);
            /*
            particleSystem.Draw(spriteBatch);
            particleSystem.EmitterLocation = new Vector2(cloudXoffset + 300, 200);
            particleSystem.Update(0);
            particleSystem.Draw(spriteBatch);
            */



            spriteBatch.Begin();
            spriteBatch.Draw(torch, new Rectangle((int)fire.EmitterLocation.X - 16, (int)fire.EmitterLocation.Y - 18, torch.Width, torch.Height
                ), new Color(255, 255, 255));

            for (int i = 0; i < player.positions.Count(); i++)
            {
                spriteBatch.Draw(fireTexture, new Rectangle((int)player.positions[i].X, (int)player.positions[i].Y, fireTexture.Width, fireTexture.Height),
                    new Color(random.Next(100, 165), random.Next(40, 110), random.Next(0, 80)));
            }
            if (!player.positions.Any() && newState.IsKeyDown(Keys.LeftControl))
            {


                float angle = (float)Math.Atan2(mousey - player.dcl.Position.Y, mousex - player.dcl.Position.X);
                float x = player.dcl.Position.X + 125 * (float)Math.Cos(angle);
                float y = player.dcl.Position.Y + 125 * (float)Math.Sin(angle);
                for (int k = 0; k < 13; k++)
                {
                    double ghX1 = k * 9.5 * Math.Cos(angle);

                    double ghY1 = k * 9.5 * Math.Sin(angle);
                    Vector2 position = new Vector2((float)ghX1 + player.dcl.Position.X, (float)ghY1 + player.dcl.Position.Y);
                    spriteBatch.Draw(selector, new Rectangle((int)position.X, (int)position.Y, selector.Width, selector.Height),
                        new Color(255, 255, 255));

                }
            }
            player.positions.Clear();

            for (int i = 0; i < collisions.Count; i++)
            {
                
                collisions[i].Draw(spriteBatch);
            }


            //aim !!!




            player.dcl.Draw(spriteBatch);
            water.Draw(spriteBatch);
            



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