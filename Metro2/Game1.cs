using Metro2.Scena;
using Metro2.Scena.Lights;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace Metro2
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager _graphics;
        private Camera camera;
        private Scene metroScene;
        private RenderTarget2D _renderTarget;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        protected override void Initialize()
        {
            metroScene = new Scene();
            camera = new Camera
            {
                aspectRatio = (float)_graphics.GraphicsDevice.Viewport.Width / (float)_graphics.GraphicsDevice.Viewport.Height,
                cameraPosition = new Vector3(0, 0, 0),
                sceneSizeX = 40,
                sceneSizeY = 100
            };

            _renderTarget = new RenderTarget2D(GraphicsDevice, GraphicsDevice.PresentationParameters.BackBufferWidth, GraphicsDevice.PresentationParameters.BackBufferHeight,
                false, GraphicsDevice.PresentationParameters.BackBufferFormat, DepthFormat.Depth24);

            CreateLights();
            base.Initialize();
        }

        private void CreateLights()
        {
            metroScene.Lights.Add(new PointLight()
            {
                Possition = new Vector3(0, 38, 0),
                Attenuation = 1000,
                Falloff = 2
            });
        }

        protected override void LoadContent()
        {
            metroScene.Shader = Content.Load<Effect>("shader");
            //tu tworzymy i dodajemu obiekty
            CreateBenches();
            CreateGarbages();
        }
        private void CreateBenches()
        {
            var benchModel = Content.Load<Model>("benches");

            metroScene.SceneElements.Add(new SceneElement()
            {
                Model = benchModel,
                AmbientColor = new Vector3(0.1f, 0.1f, 0.1f),
                DiffuseColor = Color.White.ToVector3(),
                SpecularColor = Color.White.ToVector3(),
                Scale = Matrix.CreateScale(0.05f),
                Shininess = 10,
                Translation = Matrix.CreateTranslation(-18, -40, -5),
                Rotation = Matrix.CreateRotationZ((float)Math.PI / 2)
            });
        }
        private void CreateGarbages()
        {
            var garbageModel = Content.Load<Model>("Garbage_Container_");

            metroScene.SceneElements.Add(new SceneElement()
            {
                Model = garbageModel,
                AmbientColor = new Vector3(0.1f, 0.1f, 0.1f),
                DiffuseColor = Color.White.ToVector3(),
                SpecularColor = Color.White.ToVector3(),
                Scale = Matrix.CreateScale(1.0f),
                Shininess = 10,
                Translation = Matrix.CreateTranslation(-15, 10, -5),
            });
        }
        protected override void UnloadContent()
        {
            Content.Unload();
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            camera.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.SetRenderTarget(_renderTarget);
            GraphicsDevice.DepthStencilState = new DepthStencilState() { DepthBufferEnable = true };
            GraphicsDevice.Clear(Color.Black);
            metroScene.Draw(camera, _graphics);
            GraphicsDevice.SetRenderTarget(null);
            base.Draw(gameTime);
        }
    }
}
