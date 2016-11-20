using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metro2.Scena
{
    public class Camera
    {
        public float HorizontalRotation { get; set; }
        public float VerticalRotation { get; set; }
        public float ClockRotation { get; set; }
        public Matrix View { get; set; }
        public Matrix Projection { get; set; }
        public Vector3 cameraPosition { get; set; }
        public int sceneSizeX {get; set; }
        public int sceneSizeY { get; set; }
        public float aspectRatio { get; set; }
        
        public bool blokada = true;
        public float rotationSpeed = 0.4f;
        public float moveSpeed = 20.0f;

        public Camera()
        {
            View = Matrix.CreateLookAt(new Vector3(0, 0, 0), new Vector3(0, 1, 0), new Vector3(0, 0, 1));

            float fieldOfView = Microsoft.Xna.Framework.MathHelper.PiOver4;
            Projection = Matrix.CreatePerspectiveFieldOfView(fieldOfView, aspectRatio, 1, 100000f);

            cameraPosition = new Vector3(0, 0, 0);
            HorizontalRotation = 0.0f;
            VerticalRotation = 0.0f;

        }
        public void Update(GameTime gameTime)
        {
            Vector3 moveVector = new Vector3(0, 0, 0);

            ////---------------------LEWO PRAWO GORA DOL
            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                moveVector += new Vector3(-1, 0, 0);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                moveVector += new Vector3(1, 0, 0);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Up))
            {
                moveVector += new Vector3(0, 1, 0);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Down))
            {
                moveVector += new Vector3(0, -1, 0);
            }
            ////----------------------PRZOD TYL
            if (Keyboard.GetState().IsKeyDown(Keys.OemPlus))
            {
                moveVector += new Vector3(0, 0, 1);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.OemMinus))
            {
                moveVector += new Vector3(0, 0, -1);
            }

            ////--------------------OBROTY LEWO PRAWO GORA DOL
            if (Keyboard.GetState().IsKeyDown(Keys.W))
            {
                VerticalRotation += 0.02f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.S))
            {
                VerticalRotation -= 0.02f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.A))
            {
                HorizontalRotation += 0.02f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.D))
            {
                HorizontalRotation -= 0.02f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.Q))
            {
                ClockRotation -= 0.02f;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.E))
            {
                ClockRotation += 0.02f;
            }
            AddToCameraPosition(moveVector, gameTime);
        }

        private void AddToCameraPosition(Vector3 vectorToAdd, GameTime gametime)
        {
            Matrix cameraRotation = Matrix.CreateRotationX(VerticalRotation) * Matrix.CreateRotationY(ClockRotation) * Matrix.CreateRotationZ(HorizontalRotation);

            Vector3 rotatedVector = Vector3.Transform(vectorToAdd, cameraRotation);
            cameraPosition += (moveSpeed* (float)gametime.ElapsedGameTime.TotalSeconds) * rotatedVector;

            cameraPosition= new Vector3(cameraPosition.X, cameraPosition.Y, cameraPosition.Z);

            if (blokada)
            {
                if (cameraPosition.X < -sceneSizeX / 2 + 1)
                    cameraPosition = new Vector3(-sceneSizeX / 2 + 1, cameraPosition.Y, cameraPosition.Z);
                if (cameraPosition.X > sceneSizeX / 2 - 1)
                    cameraPosition = new Vector3(sceneSizeX / 2 - 1, cameraPosition.Y, cameraPosition.Z);

                if (cameraPosition.Y < -sceneSizeY / 2 + 1)
                    cameraPosition = new Vector3(cameraPosition.X, -sceneSizeY / 2 + 1, cameraPosition.Z);

                if (cameraPosition.Y > sceneSizeY / 2 - 1)
                    cameraPosition = new Vector3(cameraPosition.X, sceneSizeY / 2 - 1, cameraPosition.Z);

                if (cameraPosition.Z < -4 && cameraPosition.X < (sceneSizeX / 4))
                    cameraPosition = new Vector3(cameraPosition.X, cameraPosition.Y, -4);
                if (cameraPosition.Z < -7 && cameraPosition.X > (sceneSizeX / 4))
                    cameraPosition = new Vector3(cameraPosition.X, cameraPosition.Y, -7);

                if (cameraPosition.Z > 7)
                    cameraPosition = new Vector3(cameraPosition.X, cameraPosition.Y, 7);
            }
            UpdateViewMatrix();
        }
        private void UpdateViewMatrix()
        {
            Matrix cameraRotation = Matrix.CreateRotationX(VerticalRotation) * Matrix.CreateRotationY(HorizontalRotation);

            Vector3 cameraOriginalTarget = new Vector3(0, 1, 0);
            Vector3 cameraRotatedTarget = Vector3.Transform(cameraOriginalTarget, cameraRotation);
            Vector3 cameraFinalTarget = cameraPosition + cameraRotatedTarget;

            Vector3 cameraOriginalUpVector = new Vector3(0, 0, 1);
            Vector3 cameraRotatedUpVector = Vector3.Transform(cameraOriginalUpVector, cameraRotation);

            View = Matrix.CreateLookAt(cameraPosition, cameraFinalTarget, cameraRotatedUpVector);
        }
    }
}
