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
using System.Threading.Tasks;
using editor;
using System.ComponentModel;





//are da napraish tupata camera a?
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
        private List<triCol> triCollisions;
        RenderTarget2D renderTarget;
        int virtualWidth = 960;
        int virtualHeight = 540;
        int xoffset;
        int yoffset;
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
        bhcontroller blackhole;
        Texture2D torch;
        List<clsData> cls;
        Texture2D whiteTexture;

        KeyboardState newState = Keyboard.GetState();
        List<Collision> collisions = new List<Collision>();
        int mousex;
        int mousey;
        Texture2D error;
        List<string> Stextures;
        Arrow arrow;
        Grid grid;
        private Texture2D ArrowTexture;
        int xofoffest = 0;
        int yofoffest = 0;
        float hue;
        float smoothXOffset = 0f;
        Texture2D hookTexture;
        float smoothYOffset = 0f;
        float cameraDamping = 0.1f; // smaller = slower, smoother

        Effect saturationEffect;

        public float Speed { get; internal set; }
        Texture2D ropeTexture;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            this.Window.AllowUserResizing = true;
            // Set the preferred resolution before calling base.Initialize()
            graphics.PreferredBackBufferWidth = 960;  // Set width
            graphics.PreferredBackBufferHeight = 540;  // Set height
            graphics.ApplyChanges();
            base.Initialize();
        }


        protected override void LoadContent()
        {
            renderTarget = new RenderTarget2D(GraphicsDevice, virtualWidth, virtualHeight);
            spriteBatch = new SpriteBatch(GraphicsDevice);
            ArrowTexture = Content.Load<Texture2D>("arrow");
            whiteTexture = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            whiteTexture.SetData(new[] { Microsoft.Xna.Framework.Color.White });
            //whiteTexture = Content.Load<Texture2D>("testile");
            crosshair = Content.Load<Texture2D>("cursor");
            hookTexture = Content.Load<Texture2D>("hookt");
            selector = Content.Load<Texture2D>("selector");
            crosshairTarget = new RenderTarget2D(GraphicsDevice, crosshair.Width, crosshair.Height);
            List<Texture2D> textures = new List<Texture2D>();
            List<Texture2D> textures2 = new List<Texture2D>();
            grid = new Grid(0, 0, 60, 34, 16, whiteTexture);
            fireTexture = Content.Load<Texture2D>("fire1");
            ropeTexture = Content.Load<Texture2D>("rope");
            error = Content.Load<Texture2D>("error");
            torch = Content.Load<Texture2D>("torch2");
            textures.Add(fireTexture);
            textures2.Add(Content.Load<Texture2D>("amogus2"));




            particleSystem = new ParticleSystem_sin(textures, new Vector2(400, 240));
            blackhole = new bhcontroller(textures, new Vector2(400, 240));
            fire = new fireSystem(textures, new Vector2(400, 240));
            Debug.WriteLine("Current directory: " + Directory.GetCurrentDirectory());
            string json = File.ReadAllText("collisions.json");
            cls = JsonSerializer.Deserialize<List<clsData>>(json);



            water = new waterController(330, 3, whiteTexture);
            int springAmount = 70;
            random = new Random();
            dd = 0;
            List<Texture2D> Tiles = new List<Texture2D>();
            Stextures = new List<string>();
            triCollisions = [new triCol(new Vector2(0, 0), new Vector2(0, 0), new Vector2(0, 0))];
            player = new Entity(collisions, textures2, spriteBatch, triCollisions);
            player.dcl.player = player;
            /*
            foreach (var cl in cls)
            {
                if (Stextures.Any())
                {
                    bool ExistingTexture = false;
                    for (int i = 0; i < Stextures.Count; i++)
                    {
                        if (cl.Texture == Stextures[i])
                        {
                            try
                            {
                                collisions.Add(new Collision(new Vector2(cl.X, cl.Y), cl.Width, cl.Height, Tiles[i]));
                            }
                            catch
                            {
                                Tiles.Add(error);
                            }


                            ExistingTexture = true;
                        }
                    }
                    if (ExistingTexture == false)
                    {
                        Stextures.Add(cl.Texture);
                        try
                        {
                            Tiles.Add(Content.Load<Texture2D>(Stextures[Stextures.Count() - 1]));
                        }
                        catch
                        {
                            Tiles.Add(error);
                        }

                    }

                }
                else
                {
                    Stextures.Add(cl.Texture);
                    //try
                    //{
                    Tiles.Add(Content.Load<Texture2D>(Stextures[0]));
                    //}
                    //catch
                    //{
                    //    Tiles.Add(Content.Load<Texture2D>("error"));
                    //}
                    collisions.Add(new Collision(new Vector2(cl.X, cl.Y), cl.Width, cl.Height, Tiles[0]));
                }

            }
            */
            Dictionary<string, Texture2D> tileLookup = new Dictionary<string, Texture2D>();

            foreach (var cl in cls)
            {
                if (string.IsNullOrEmpty(cl.Texture))
                {
                    // If texture name is missing, use error texture
                    collisions.Add(new Collision(new Vector2(cl.X, cl.Y), cl.Width, cl.Height, error));
                    continue;
                }

                if (!tileLookup.TryGetValue(cl.Texture, out Texture2D tile))
                {
                    try
                    {
                        tile = Content.Load<Texture2D>(cl.Texture);
                    }
                    catch
                    {
                        tile = error;
                    }
                    tileLookup[cl.Texture] = tile;
                }

                collisions.Add(new Collision(new Vector2(cl.X, cl.Y), cl.Width, cl.Height, tile));
            }

            arrow = new Arrow(new Vector2(0, 0), ArrowTexture, collisions);


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

            mousex = Mouse.GetState().X + xoffset;
            mousey = Mouse.GetState().Y  +yoffset;
            arrow.Update(player.dcl.Position, xoffset, yoffset);
            for (int h = 0; h < 34; h++)
            {
                for (int w = 0; w < 60; w++)
                {
                    grid.lines[h].rows[w].Update(mousex - xoffset, mousey - yoffset, xoffset, yoffset, collisions, newState);
                }
            }
            
            if (newState.IsKeyDown(Keys.R))
            {
                player.dcl.Position = new Vector2(90, 300);
            }
            if (newState.IsKeyDown(Keys.Right))
            {
                xofoffest++;
                xofoffest++;
            }
            if (newState.IsKeyDown(Keys.Left))
            {
                xofoffest--;
                xofoffest--;
            }
            if (newState.IsKeyDown(Keys.Down))
            {
                yofoffest++;
                yofoffest++;
            }
            if (newState.IsKeyDown(Keys.Up))
            {
                yofoffest--;
                yofoffest--;
            }
            
            float targetXOffset = -(player.dcl.Position.X) + 192 + xofoffest;
            float targetYOffset = -(player.dcl.Position.Y) + 324 + yofoffest;
            
            smoothXOffset = MathHelper.Lerp(smoothXOffset, targetXOffset, cameraDamping);
            smoothYOffset = MathHelper.Lerp(smoothYOffset, targetYOffset, cameraDamping);
            
            xoffset = (int)smoothXOffset;
            yoffset = (int)smoothYOffset;

            water.Update(gameTime, newState, random);
            cloudXoffset += 0.3f;
            fire.Update(0);

            hue += (float)gameTime.ElapsedGameTime.TotalSeconds * 200f;

            if (hue >= 360f) hue -= 360f;
            player.Update(xoffset, yoffset);
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
            
            spriteBatch.End();







            GraphicsDevice.SetRenderTarget(renderTarget);
            GraphicsDevice.Clear(new Color(68, 179, 248, 255));
            
            fire.Draw(spriteBatch, xoffset, yoffset);

            particleSystem.EmitterLocation = new Vector2(cloudXoffset + xoffset, 75 + yoffset);
            //particleSystem.Update(0);
            /*
            particleSystem.Draw(spriteBatch);
            particleSystem.EmitterLocation = new Vector2(cloudXoffset + 300, 200);
            particleSystem.Update(0);
            particleSystem.Draw(spriteBatch);
            */



            spriteBatch.Begin();
            spriteBatch.Draw(torch, new Rectangle(xoffset + (int)fire.EmitterLocation.X - 16, yoffset + (int)fire.EmitterLocation.Y - 18, torch.Width, torch.Height
                ), new Color(255, 255, 255));

            
            if (!player.positions.Any() && newState.IsKeyDown(Keys.LeftControl))
            {


                                // Mouse position in world coordinates
                float worldMouseX = Mouse.GetState().X - xoffset;
                float worldMouseY = Mouse.GetState().Y - yoffset;

                float angle = (float)Math.Atan2(worldMouseY - player.dcl.Position.Y,
                                                worldMouseX - player.dcl.Position.X);

                float x = player.dcl.Position.X + 125 * (float)Math.Cos(angle);
                float y = player.dcl.Position.Y + 125 * (float)Math.Sin(angle);
                for (int k = 0; k < 13; k++)
                {
                    double ghX1 = k * 9.5 * Math.Cos(angle);

                    double ghY1 = k * 9.5 * Math.Sin(angle);
                    Vector2 position = new Vector2((float)ghX1 + player.dcl.Position.X, (float)ghY1 + player.dcl.Position.Y);
                    spriteBatch.Draw(selector, new Rectangle((int)position.X + xoffset, (int)position.Y + yoffset, selector.Width, selector.Height),
                        new Color(255, 255, 255));

                }
            }
            
            blackhole.Update(69);

            for (int i = 0; i < collisions.Count; i++)
            {
                collisions[i].Update();
                collisions[i].Draw(spriteBatch, whiteTexture, xoffset, yoffset);

            }
            for (int i = 0; i < triCollisions.Count; i++)
            {

                triCollisions[i].Draw(spriteBatch);
            }
            //blackhole.Draw(spriteBatch);

            //aim !!!

            


            player.dcl.Draw(spriteBatch, xoffset, yoffset);
            //water.Draw(spriteBatch, xoffset, yoffset);
            
            arrow.Draw(spriteBatch, Color.White, xoffset, yoffset);
            spriteBatch.Draw(crosshair, new Rectangle(Mouse.GetState().X, Mouse.GetState().Y, crosshair.Width, crosshair.Height), Color.White);
            for (int i = 0; i < player.positions.Count(); i++)
            {
                //if (i == 0)
                //{
                //    continue;
                //}
                spriteBatch.Draw(ropeTexture, new Vector2(xoffset + (int)player.positions[i].X, yoffset + (int)player.positions[i].Y), new Rectangle(0, 0, fireTexture.Width, fireTexture.Height),
                    Color.White, player.angles[i], new Vector2(ropeTexture.Width / 2f, ropeTexture.Height / 2f), 1f, SpriteEffects.None, 0f);
            }
            if (player.positions.Any())
            {
                Console.WriteLine("uu");
                spriteBatch.Draw(hookTexture, new Vector2(xoffset + (int)player.positions[0].X, yoffset + (int)player.positions[0].Y), new Rectangle(0, 0, hookTexture.Width, hookTexture.Height),
                    Color.White, player.angles[0] + MathHelper.ToRadians(180), new Vector2(hookTexture.Width / 2f, hookTexture.Height / 2f), 1f, SpriteEffects.None, 0f);
            }
            player.positions.Clear();
            spriteBatch.End();





            GraphicsDevice.SetRenderTarget(null); // Set backbuffer

            // Optional: clear screen before final draw
            GraphicsDevice.Clear(Color.Black);

            // Maintain aspect ratio (optional)
            float scaleX = GraphicsDevice.Viewport.Width / (float)virtualWidth;
            float scaleY = GraphicsDevice.Viewport.Height / (float)virtualHeight;
            float scale = Math.Min(scaleX, scaleY);

            int displayWidth = (int)(virtualWidth * scale);
            int displayHeight = (int)(virtualHeight * scale);
            int offsetX = (GraphicsDevice.Viewport.Width - displayWidth) / 2;
            int offsetY = (GraphicsDevice.Viewport.Height - displayHeight) / 2;

            Rectangle dstRect = new Rectangle(offsetX, offsetY, displayWidth, displayHeight);

            spriteBatch.Begin(
                SpriteSortMode.Deferred,
                BlendState.Opaque,
                SamplerState.PointClamp, // no blurring
                DepthStencilState.Default,
                RasterizerState.CullNone
            );

            spriteBatch.Draw(renderTarget, dstRect, Color.White);
            spriteBatch.End();
            base.Draw(gameTime);
        }

    }
}