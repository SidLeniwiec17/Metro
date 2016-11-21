using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metro2.Scena
{
    public class SceneElement
    {
        public Matrix Translation { get; set; }
        public Matrix Scale { get; set; }
        public Matrix Rotation { get; set; }
        public Model Model { get; set; }
        public Vector3 AmbientColor { get; set; }
        public Vector3 DiffuseColor { get; set; }
        public Vector3 SpecularColor { get; set; }
        public float Shininess { get; set; }

        public SceneElement()
        {
            Translation = Matrix.Identity;
            Scale = Matrix.Identity;
            Rotation = Matrix.Identity;

        }
        public void Draw(Effect shader, GraphicsDeviceManager graphics)
        {
            foreach (var mesh in Model.Meshes)
            {
                foreach (var part in mesh.MeshParts)
                {
                    part.Effect = shader;
                }
            }
            foreach (var mesh in Model.Meshes)
            {
                foreach (Effect effect in mesh.Effects)
                {
                    effect.Parameters["World"].SetValue(mesh.ParentBone.Transform * Scale * Rotation * Translation);
                    effect.Parameters["DiffuseColor"].SetValue(DiffuseColor);
                    effect.Parameters["AmbientColor"].SetValue(AmbientColor);
                    effect.Parameters["SpecularColor"].SetValue(SpecularColor);
                    effect.Parameters["Shininess"].SetValue(Shininess);
                }

                mesh.Draw();
            }
        }
    }
}
