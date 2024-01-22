using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyGame.PrimitiveStuff
{
    public struct BlockVertex : IVertexType
    {
        public static readonly int Stride = 40;

        public Vector3 Position;
        public Vector3 Normal;
        public Vector2 TexCoord;
        public Color Color;


        VertexDeclaration IVertexType.VertexDeclaration
        {
            get { return VertexDeclaration; }
        }

        public VertexDeclaration VertexDeclaration = new VertexDeclaration(
            new VertexElement(0, VertexElementFormat.Vector3, VertexElementUsage.Position, 0),
            new VertexElement(12, VertexElementFormat.Vector3, VertexElementUsage.Normal, 0),
            new VertexElement(28, VertexElementFormat.Vector2, VertexElementUsage.TextureCoordinate, 0),
            new VertexElement(36, VertexElementFormat.Color, VertexElementUsage.Color, 0)

            );

        public BlockVertex(Vector3 position, Vector3 normal, Vector2 texCoord, Color color)
        {
            Position = position;
            Normal = normal;
            TexCoord = texCoord; 
            Color = color;
        }
    }
}
