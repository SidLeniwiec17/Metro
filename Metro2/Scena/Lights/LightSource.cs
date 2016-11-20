using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Metro2.Scena.Lights
{
    public abstract class LightSource
    {
        public Vector4 Color { get; set; }
        public LightSource()
        {
            Color = Microsoft.Xna.Framework.Color.White.ToVector4();
        }
    }
}
