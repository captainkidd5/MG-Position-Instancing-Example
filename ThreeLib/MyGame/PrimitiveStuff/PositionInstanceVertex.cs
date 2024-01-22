

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace MyGame.PrimitiveStuff{

    public struct PositionInstanceVertex : IVertexType
    {
    public static readonly int Stride = sizeof(float) * 16;

        public Vector4 M1;
        public Vector4 M2;
        public Vector4 M3;
        public Vector4 M4;



public PositionInstanceVertex(Matrix world)
{

M1 = new Vector4(world.M11, world.M12, world.M13, world.M14);
M2 = new Vector4(world.M21, world.M22, world.M23, world.M24);
M3 = new Vector4(world.M31, world.M32, world.M33, world.M34);
M4 = new Vector4(world.M41, world.M42, world.M43, world.M44);

}
VertexDeclaration declaration = new VertexDeclaration(

new VertexElement(0, VertexElementFormat.Vector4, VertexElementUsage.TextureCoordinate, 0),
new VertexElement(16, VertexElementFormat.Vector4, VertexElementUsage.TextureCoordinate, 0),
new VertexElement(32, VertexElementFormat.Vector4, VertexElementUsage.TextureCoordinate, 0),
new VertexElement(48, VertexElementFormat.Vector4, VertexElementUsage.TextureCoordinate, 0)

);

     VertexDeclaration IVertexType.VertexDeclaration
        {
            get { return declaration; }
        }

    }
}