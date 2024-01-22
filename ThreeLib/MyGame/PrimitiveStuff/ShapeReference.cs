  
  using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyGame.PrimitiveStuff
{
  
  public struct ShapeReference
    {
        public int IndexStart { get; private set; }
        public int VertexStart { get; private set; }
        public byte TriangleCount { get; private set; }
        public ShapeReference(int indexStart, int vertexStart, int triangleCount)
        {
            IndexStart = indexStart;
            VertexStart = vertexStart;

            if (triangleCount > byte.MaxValue)
                throw new Exception($"If you really have a basic shape with this many triangles, may need to change TriangleCount to bigger datatype");

            TriangleCount = (byte)triangleCount;
        }
    }
   

}