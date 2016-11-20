using Metro2.Scena.Lights;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metro2.Scena
{
    public class Scene
    {
        public List<SceneElement> SceneElements { get; set; }
        public Platform Platform { get; set; }
        public Station Station { get; set; }
        public Effect Shader { get; set; }
        public List<LightSource> Lights { get; set; }

        public Scene()
        {
            SceneElements = new List<SceneElement>();
            Lights = new List<LightSource>();
        }
        public void Draw(Camera camera, GraphicsDeviceManager graphics)
        {
            RasterizerState originalRasterizerState = graphics.GraphicsDevice.RasterizerState;
            RasterizerState rasterizerState = new RasterizerState();
            rasterizerState.CullMode = CullMode.None;
            graphics.GraphicsDevice.RasterizerState = rasterizerState;

            RasterizerState rasterizerStateWireframe = new RasterizerState();
            rasterizerStateWireframe.FillMode = FillMode.WireFrame;
            rasterizerStateWireframe.CullMode = CullMode.None;
            
            //Draw stacja
           // Station.Draw();
            //Draw peron
           // Platform.Draw();
            //Draw models
            SetupShader(Shader, camera);
            foreach (var sceneElement in SceneElements)
            {
                sceneElement.Draw(Shader);
            }
        }
        private void SetupShader(Effect shader, Camera camera)
        {
            shader.Parameters["CameraPosition"].SetValue(camera.cameraPosition);
            shader.Parameters["View"].SetValue(camera.View);
            shader.Parameters["Projection"].SetValue(camera.Projection);

            shader.Parameters["LightsCount"].SetValue(Lights.Count);
            shader.Parameters["LightColor"].SetValue(GetLightsColor());
            shader.Parameters["LightType"].SetValue(GetLightsTypes());

            shader.Parameters["LightDirection"].SetValue(GetLightsDirections());

            shader.Parameters["LightPosition"].SetValue(GetLightsPositions());
            shader.Parameters["LightAttenuation"].SetValue(GetLightsAttenuations());
            shader.Parameters["LightFalloff"].SetValue(GetLightsFalloffs());

            shader.Parameters["LightConeAngle"].SetValue(GetLightsConeAngles());
        }
        
        private Vector3[] GetLightsColor()
        {
            var colors = new List<Vector3>();
            Vector4[] tmp2 = Lights.Select(l => l.Color).ToArray();
            for (int i = 0; i < tmp2.Length; i++ )
            {
                Vector3 v = new Vector3(tmp2[i].X, tmp2[i].Y, tmp2[i].Z);
                colors.Add(v);
            }
            return colors.ToArray();
        }

        private Vector3[] GetLightsDirections()
        {
            var directions = new List<Vector3>();
            foreach (var light in Lights)
            {
                if (light is DirectionLight)
                {
                    var directionLight = light as DirectionLight;
                    directions.Add(directionLight.Direction);
                }
                else if (light is SpotLight)
                {
                    var spotLight = light as SpotLight;
                    directions.Add(spotLight.Direction);
                }
                else
                {
                    directions.Add(Vector3.Zero);
                }
            }
            return directions.ToArray();
        }
        private int[] GetLightsTypes()
        {
            var types = new List<int>();
            foreach (var light in Lights)
            {
                if (light is DirectionLight)
                {
                    types.Add(0);
                }
                else if (light is PointLight)
                {
                    types.Add(1);
                }
                else if (light is SpotLight)
                {
                    types.Add(2);
                }
                else
                {
                    types.Add(-1);
                }
            }

            return types.ToArray();
        }
        private Vector3[] GetLightsPositions()
        {
            var positions = new List<Vector3>();
            foreach (var light in Lights)
            {
                if (light is PointLight)
                {
                    var pointLight = light as PointLight;
                    positions.Add(pointLight.Possition);
                }
                else if (light is SpotLight)
                {
                    var spotLight = light as SpotLight;
                    positions.Add(spotLight.Possition);
                }
                else
                {
                    positions.Add(Vector3.Zero);
                }
            }
            return positions.ToArray();
        }
        private float[] GetLightsAttenuations()
        {
            var attenuations = new List<float>();
            foreach (var light in Lights)
            {
                if (light is PointLight)
                {
                    var pointLight = light as PointLight;
                    attenuations.Add(pointLight.Attenuation);
                }
                else
                {
                    attenuations.Add(0);
                }
            }
            return attenuations.ToArray();
        }
        private float[] GetLightsFalloffs()
        {
            var fallOffs = new List<float>();
            foreach (var light in Lights)
            {
                if (light is PointLight)
                {
                    var pointLight = light as PointLight;
                    fallOffs.Add(pointLight.Falloff);
                }
                else if (light is SpotLight)
                {
                    var spotLight = light as SpotLight;
                    fallOffs.Add(spotLight.Falloff);
                }
                else
                {
                    fallOffs.Add(0);
                }
            }
            return fallOffs.ToArray();
        }
        private float[] GetLightsConeAngles()
        {
            var coneAngles = new List<float>();
            foreach (var light in Lights)
            {
                if (light is SpotLight)
                {
                    var spotLight = light as SpotLight;
                    coneAngles.Add(spotLight.Falloff);
                }
                else
                {
                    coneAngles.Add(0);
                }
            }
            return coneAngles.ToArray();
        }

    }
}
