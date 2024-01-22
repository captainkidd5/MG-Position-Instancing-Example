 
   using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace MyGame.PrimitiveStuff
{
  
 public struct BlockInfo
    {
        public ushort Gid;

        public ShapeReference ShapeReference { get; set; }

        public BlockInfo(int gid, ShapeReference shapeReference)
        {
            if (gid > ushort.MaxValue)
                throw new Exception($"Gid too large");
            Gid = (ushort)gid;
            ShapeReference = shapeReference;
        }

    }
}