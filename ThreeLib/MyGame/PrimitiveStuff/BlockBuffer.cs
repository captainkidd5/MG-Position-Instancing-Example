
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyGame.PrimitiveStuff
{
    //https://gamedev.stackexchange.com/questions/58835/how-can-i-actually-understand-instanced-geometry-rendering-so-that-i-can-imple
    //Bind the vertex and index buffers to the device, containing the mesh geomtry
    // Bind the effect (shader) to the device
    // Bind an additional vertex buffer to the device, containing 100 world space matrices
    // Draw the mesh 100 times with instancing
    public partial class BlockBuffer
    {
        const int iBytes = sizeof(ushort); //each index only has a short value. Only a short

        private GraphicsDevice _graphics;
        private BlendState _oldBlendState;
        private RasterizerState _oldRasterizerState;

        private BlockVertex[] _localShapeVerticies;
        private VertexBuffer _vertexBuffer;
        private int _localShapeVertexCount;
        private int _currentVertexStart;

        private ushort[] _localShapeIndicies;
        private IndexBuffer _indexBuffer;
        private int _localShapeIndexCount;
        private int _currentIndexBufferStart;

        private int _triangleCount;

        private Texture2D _texture;
        private Effect _effect;

        public Dictionary<ushort, BlockInfo> Blocks { get; private set; } = new Dictionary<ushort, BlockInfo>();


        //Contains positions for blocks, which are then translated to world matricies in the shader
        private VertexBuffer _instanceVertexBuffer;
        private PositionInstanceVertex[] _positions;

        private ushort _totalInstances;
        private VertexBufferBinding[] _binding;


        public void Initialize(GraphicsDevice graphics)
        {
            _graphics = graphics;


            _localShapeVerticies = new BlockVertex[ushort.MaxValue];
            _localShapeIndicies = new ushort[ushort.MaxValue];
            _triangleCount = 0;

            _positions = new PositionInstanceVertex[ushort.MaxValue];
            _instanceVertexBuffer = new VertexBuffer(_graphics, typeof(BlockVertex), ushort.MaxValue, BufferUsage.WriteOnly);

            _binding = new VertexBufferBinding[2];
        }

        public void Load(Effect effect)
        {
            _effect = effect;
        }

        public bool HasBlockData(ushort gid) => Blocks.ContainsKey(gid);
        public void LoadNewBlock(ushort gid, Rectangle sourceRect)
        {
            if (Blocks.ContainsKey(gid))
                throw new Exception($"Duplicate info load");




            //Rect encompassing all tile textures for tile in tileset
            sourceRect = new Rectangle(sourceRect.X - 16, sourceRect.Y - 32, 48, 64);
            ShapeReference blockShapeReference = CreateCube(1, 1, 1, GetCubeUVFaces(_texture, sourceRect), Color.White);
            Blocks[gid] = new BlockInfo(gid, blockShapeReference);
        }
        private static readonly float _bILLBOARD_ANIM_SCALE = 0.15f;
        private float _billboardAnimationTime = 0f;

        public void Draw(GameTime gameTime, Matrix viewMatrix, Matrix projectionMatrix)
        {
            _oldRasterizerState = _graphics.RasterizerState;
            _oldBlendState = _graphics.BlendState;

            _effect.Parameters["alphaTestThreshold"].SetValue(.95f);

            _effect.Parameters["world"].SetValue(Matrix.Identity);
            _effect.Parameters["view"].SetValue(viewMatrix);
            _effect.Parameters["projection"].SetValue(projectionMatrix);

            _effect.Parameters["animationTime"].SetValue(_billboardAnimationTime);
            _effect.Parameters["animationScaleFactor"].SetValue(_bILLBOARD_ANIM_SCALE);
            _effect.Parameters["alphaTestDirection"].SetValue(1.0f);

            _graphics.BlendState = BlendState.Opaque;
            _graphics.DepthStencilState = DepthStencilState.Default;
            _graphics.RasterizerState = RasterizerState.CullCounterClockwise;

            //set the position data
            _instanceVertexBuffer.SetData<PositionInstanceVertex>(
                0, _positions, 0, _totalInstances, PositionInstanceVertex.Stride);

            _vertexBuffer.SetData<BlockVertex>(0, _localShapeVerticies, 0, _localShapeVertexCount, BlockVertex.Stride);
            _indexBuffer.SetData<ushort>(0, _localShapeIndicies, 0, _localShapeIndexCount);
            _graphics.SetVertexBuffers(_binding);
            
            foreach (EffectPass pass in _effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                _graphics.DrawInstancedPrimitives(PrimitiveType.TriangleList, 0, 0, _localShapeVertexCount, _totalInstances);
            }


            _effect.Parameters["alphaTestDirection"].SetValue(-1.0f);
            _graphics.BlendState = BlendState.NonPremultiplied;
            _graphics.DepthStencilState = DepthStencilState.DepthRead;


            foreach (EffectPass pass in _effect.CurrentTechnique.Passes)
            {
                pass.Apply();
                _graphics.DrawInstancedPrimitives(PrimitiveType.TriangleList, 0, 0, _localShapeVertexCount, _totalInstances);
            }


            _graphics.BlendState = _oldBlendState;
            _graphics.RasterizerState = _oldRasterizerState;
        }


    }
}
