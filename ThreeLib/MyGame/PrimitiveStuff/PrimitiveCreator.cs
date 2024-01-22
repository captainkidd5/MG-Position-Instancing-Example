using Microsoft.Xna.Framework.Graphics.PackedVector;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;


namespace MyGame.PrimitiveStuff
{
     public enum Faces : byte
    {
        Top = 0,
        Bottom = 1,
        Left = 2,
        Right = 3,
        Front = 4,
        Back = 5
    }
    public partial class BlockBuffer
    {
        private void AddVertex(float x, float y, float z, Vector3 normal, float u, float v, Color color)
        {
            BlockVertex customVertex = new BlockVertex(new Vector3(x, y, z), normal, new Vector2(u, v), color);
            _localShapeVerticies[_localShapeVertexCount] = customVertex;
            _localShapeVertexCount++;
        }

        private void CreateTriangle(ushort indexA, ushort indexB, ushort indexC)
        {
            ushort offSet = (ushort)_currentIndexBufferStart;
            if (indexA + offSet > ushort.MaxValue)
                throw new Exception($"using too much data, check data structures");

            indexA += offSet; indexB += offSet; indexC += offSet;

            _localShapeIndicies[_localShapeIndexCount] = indexA;
            _localShapeIndexCount++;
            _localShapeIndicies[_localShapeIndexCount] = indexB;
            _localShapeIndexCount++;
            _localShapeIndicies[_localShapeIndexCount] = indexC;
            _localShapeIndexCount++;

            _triangleCount++;
        }   

        /// <summary>
        /// Create a quad centered at position
        /// </summary>
        public void CreateQuad(float width, float height, Vector4 uvCoords, Color color)
        {
            Vector3 normal = Vector3.Up;
            //4 verticies, 6 indicies, 2 triangles
            //create first triangle as top right triangle, do clockwise
            float halfWidth = width / 2;
            float halfHeight = height / 2;
            //top left
            AddVertex(-halfWidth, halfHeight, 0, normal, uvCoords.X, uvCoords.Y, color);

            //top right
            AddVertex(halfWidth, halfHeight, 0, normal, uvCoords.Z, uvCoords.Y, color);

            //bottom right
            AddVertex(halfWidth, -halfHeight, 0, normal, uvCoords.Z, uvCoords.W, color);

            //bottom left
            AddVertex(-halfWidth, -halfHeight, 0, normal, uvCoords.X, uvCoords.W, color);
            CreateTriangle(0, 1, 2);
            CreateTriangle(0, 2, 3);


            _vertexBuffer.SetData<BlockVertex>(_currentVertexStart * BlockVertex.Stride, _localShapeVerticies, 0, _localShapeVertexCount, BlockVertex.Stride);
            _currentVertexStart += _localShapeVertexCount;
            _localShapeVertexCount = 0;

            _indexBuffer.SetData<ushort>(_currentIndexBufferStart * iBytes, _localShapeIndicies, 0, _localShapeIndexCount);
            _currentIndexBufferStart += _localShapeIndexCount;
            _localShapeIndexCount = 0;

        }

        enum CV : ushort
        {
            T1 = 0,
            T2 = 1,
            T3 = 2,
            T4 = 3,
            Bo1 = 4,
            Bo2 = 5,
            Bo3 = 6,
            Bo4 = 7,
            L1 = 8,
            L2 = 9,
            L3 = 10,
            L4 = 11,
            R1 = 12,
            R2 = 13,
            R3 = 14,
            R4 = 15,
            F1 = 16,
            F2 = 17,
            F3 = 18,
            F4 = 19,
            Ba1 = 20,
            Ba2 = 21,
            Ba3 = 22,
            Ba4 = 23,
        }

        public static Vector4 GetUVCoords(Rectangle sourceRect, Texture2D texture)
        {
            Vector4 translated = new Vector4(
               sourceRect.X / (float)texture.Width,
                sourceRect.Y / (float)texture.Height,
                (sourceRect.X + sourceRect.Width) / (float)texture.Width,
               (sourceRect.Y + sourceRect.Height) / (float)texture.Height
                );
            return translated;
        }
        /// <summary>
        /// Gets source rectangle in texture space (0-1)
        /// </summary>
        private static Vector4[] GetCubeUVFaces(Texture2D texture, Rectangle sourceRectangle)
        {
            int width = 16;
            Vector4[] rects = new Vector4[6];
            Vector4 top = GetUVCoords(new Rectangle(
              sourceRectangle.X + width,
               sourceRectangle.Y,
                width,
                width), texture);
            Vector4 bottom = GetUVCoords(new Rectangle(
       sourceRectangle.X + width,
       sourceRectangle.Y + width * 2,
        width,
      width), texture);

            Vector4 left = GetUVCoords(new Rectangle(
     sourceRectangle.X,
    sourceRectangle.Y + width * 2,
    width,
  width), texture);

            Vector4 right = GetUVCoords(new Rectangle(
   sourceRectangle.X + width * 2,
     sourceRectangle.Y + width * 2,
    width,
  width), texture);
            Vector4 front = GetUVCoords(new Rectangle(
sourceRectangle.X + width,
sourceRectangle.Y + width * 3,
width,
width), texture);
            Vector4 back = GetUVCoords(new Rectangle(
sourceRectangle.X + width,
sourceRectangle.Y + width,
width,
width), texture);
            rects[(int)Faces.Top] = top;
            rects[(int)Faces.Bottom] = bottom;
            rects[(int)Faces.Left] = left;
            rects[(int)Faces.Right] = right;
            rects[(int)Faces.Front] = front;
            rects[(int)Faces.Back] = back;

            return rects;
        }


        /// <summary>
        /// Create a cube centered at position
        /// </summary>

        public ShapeReference CreateCube(float width, float height, float length, Vector4[] uvCoords, Color color)
        {

            //8 verticies, 36 indicies, 12 triangles
            //create first triangle as top right triangle, do clockwise
            float halfWidth = width / 2;
            float halfHeight = height / 2;
            float halfLength = length / 2;


            Vector3 normal = Vector3.Up;
            //T1
            AddVertex(-halfWidth, halfHeight, -halfLength, normal, uvCoords[(int)Faces.Top].X, uvCoords[(int)Faces.Top].Y, color);
            //T2
            AddVertex(halfWidth, halfHeight, -halfLength, normal, uvCoords[(int)Faces.Top].Z, uvCoords[(int)Faces.Top].Y, color);
            //T3
            AddVertex(halfWidth, halfHeight, halfLength, normal, uvCoords[(int)Faces.Top].Z, uvCoords[(int)Faces.Top].W, color);
            //T4
            AddVertex(-halfWidth, halfHeight, halfLength, normal, uvCoords[(int)Faces.Top].X, uvCoords[(int)Faces.Top].W, color);

            normal = Vector3.Down;

            //Bo1
            AddVertex(-halfWidth, -halfHeight, -halfLength, normal, uvCoords[(int)Faces.Bottom].X, uvCoords[(int)Faces.Bottom].Y, color);
            //Bo2
            AddVertex(halfWidth, -halfHeight, -halfLength, normal, uvCoords[(int)Faces.Bottom].Z, uvCoords[(int)Faces.Bottom].Y, color);
            //Bo3
            AddVertex(halfWidth, -halfHeight, halfLength, normal, uvCoords[(int)Faces.Bottom].Z, uvCoords[(int)Faces.Bottom].W, color);
            //Bo4
            AddVertex(-halfWidth, -halfHeight, halfLength, normal, uvCoords[(int)Faces.Bottom].X, uvCoords[(int)Faces.Bottom].W, color);

            normal = Vector3.Left;
            //L1
            AddVertex(-halfWidth, halfHeight, -halfLength, normal, uvCoords[(int)Faces.Left].X, uvCoords[(int)Faces.Left].Y, color);
            //L2
            AddVertex(-halfWidth, halfHeight, halfLength, normal, uvCoords[(int)Faces.Left].Z, uvCoords[(int)Faces.Left].Y, color);
            //L3
            AddVertex(-halfWidth, -halfHeight, halfLength, normal, uvCoords[(int)Faces.Left].Z, uvCoords[(int)Faces.Left].W, color);
            //L4
            AddVertex(-halfWidth, -halfHeight, -halfLength, normal, uvCoords[(int)Faces.Left].X, uvCoords[(int)Faces.Left].W, color);

            normal = Vector3.Right;
            //R1
            AddVertex(halfWidth, halfHeight, halfLength, normal, uvCoords[(int)Faces.Right].X, uvCoords[(int)Faces.Right].Y, color);
            //R2
            AddVertex(halfWidth, halfHeight, -halfLength, normal, uvCoords[(int)Faces.Right].Z, uvCoords[(int)Faces.Right].Y, color);
            //R3
            AddVertex(halfWidth, -halfHeight, -halfLength, normal, uvCoords[(int)Faces.Right].Z, uvCoords[(int)Faces.Right].W, color);
            //R4
            AddVertex(halfWidth, -halfHeight, halfLength, normal, uvCoords[(int)Faces.Right].X, uvCoords[(int)Faces.Right].W, color);

            normal = Vector3.Forward;
            //F1
            AddVertex(-halfWidth, halfHeight, halfLength, normal, uvCoords[(int)Faces.Front].X, uvCoords[(int)Faces.Front].Y, color);
            //F2
            AddVertex(halfWidth, halfHeight, halfLength, normal, uvCoords[(int)Faces.Front].Z, uvCoords[(int)Faces.Front].Y, color);
            //F3
            AddVertex(halfWidth, -halfHeight, halfLength, normal, uvCoords[(int)Faces.Front].Z, uvCoords[(int)Faces.Front].W, color);
            //F4
            AddVertex(-halfWidth, -halfHeight, halfLength, normal, uvCoords[(int)Faces.Front].X, uvCoords[(int)Faces.Front].W, color);

            normal = Vector3.Backward;
            //Ba1
            AddVertex(-halfWidth, halfHeight, -halfLength, normal, uvCoords[(int)Faces.Back].X, uvCoords[(int)Faces.Back].Y, color);
            //Ba2
            AddVertex(halfWidth, halfHeight, -halfLength, normal, uvCoords[(int)Faces.Back].Z, uvCoords[(int)Faces.Back].Y, color);
            //Ba3
            AddVertex(halfWidth, -halfHeight, -halfLength, normal, uvCoords[(int)Faces.Back].Z, uvCoords[(int)Faces.Back].W, color);
            //Ba4
            AddVertex(-halfWidth, -halfHeight, -halfLength, normal, uvCoords[(int)Faces.Back].X, uvCoords[(int)Faces.Back].W, color);


            //top
            CreateTriangle((ushort)CV.T1, (ushort)CV.T2, (ushort)CV.T3);
            CreateTriangle((ushort)CV.T1, (ushort)CV.T3, (ushort)CV.T4);

            //bottom quad
            CreateTriangle((ushort)CV.Bo2, (ushort)CV.Bo1, (ushort)CV.Bo4);
            CreateTriangle((ushort)CV.Bo2, (ushort)CV.Bo4, (ushort)CV.Bo3);

            //left quad
            CreateTriangle((ushort)CV.L1, (ushort)CV.L2, (ushort)CV.L3);
            CreateTriangle((ushort)CV.L1, (ushort)CV.L3, (ushort)CV.L4);

            //right quad
            CreateTriangle((ushort)CV.R1, (ushort)CV.R2, (ushort)CV.R3);
            CreateTriangle((ushort)CV.R1, (ushort)CV.R3, (ushort)CV.R4);

            //front quad
            CreateTriangle((ushort)CV.F1, (ushort)CV.F2, (ushort)CV.F3);
            CreateTriangle((ushort)CV.F1, (ushort)CV.F3, (ushort)CV.F4);


            //back quad
            CreateTriangle((ushort)CV.Ba2, (ushort)CV.Ba1, (ushort)CV.Ba4);
            CreateTriangle((ushort)CV.Ba2, (ushort)CV.Ba4, (ushort)CV.Ba3);
            _vertexBuffer.SetData<BlockVertex>(_currentVertexStart * BlockVertex.Stride, _localShapeVerticies, 0, _localShapeVertexCount, BlockVertex.Stride);
            ShapeReference shapeReference = new ShapeReference(_currentIndexBufferStart, _currentVertexStart, 12);

            _currentVertexStart += _localShapeVertexCount;
            _localShapeVertexCount = 0;

            _indexBuffer.SetData<ushort>(_currentIndexBufferStart * iBytes, _localShapeIndicies, 0, _localShapeIndexCount);

            _currentIndexBufferStart += _localShapeIndexCount;
            _localShapeIndexCount = 0;

            return shapeReference;

        }
    }
    
}
