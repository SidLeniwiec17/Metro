using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metro2.Scena.Lights
{
    public class SpotLight : LightSource
    {
        public Vector3 Possition { get; set; }

        public float Falloff { get; set; }

        public Vector3 Direction { get; set; }

        public float ConeAngle { get; set; }
    }
}
