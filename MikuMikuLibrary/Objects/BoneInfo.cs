﻿using System;
using System.Numerics;

namespace MikuMikuLibrary.Objects
{
    public class BoneInfo
    {
        public const uint InvalidId = 0xFFFFFFFF;

        public uint Id { get; set; } = InvalidId;
        public Matrix4x4 InverseBindPoseMatrix { get; set; }
        public string Name { get; set; }
        public bool IsEx { get; set; }
        public BoneInfo Parent { get; set; }
    }

    public struct BoneWeight
    {
        public static readonly BoneWeight Empty = new BoneWeight
        {
            Index1 = -1,
            Index2 = -1,
            Index3 = -1,
            Index4 = -1
        };

        public float Weight1, Weight2, Weight3, Weight4;
        public int Index1, Index2, Index3, Index4;

        public void Validate()
        {
            var copy = this;

            Index1 = -1;
            Weight1 = 0;
            Index2 = -1;
            Weight2 = 0;
            Index3 = -1;
            Weight3 = 0;
            Index4 = -1;
            Weight4 = 0;

            if ( copy.Weight1 > 0 )
                AddWeight( copy.Index1, copy.Weight1 );

            if ( copy.Weight2 > 0 )
                AddWeight( copy.Index2, copy.Weight2 );

            if ( copy.Weight3 > 0 )
                AddWeight( copy.Index3, copy.Weight3 );

            if ( copy.Weight4 > 0 )
                AddWeight( copy.Index4, copy.Weight4 );

            Weight4 = 1.0f - Weight1 - Weight2 - Weight3;

            if ( Math.Abs( Weight4 - 1.0f ) < 0.00001f || Math.Abs( Weight4 ) < 0.00001f )
                Weight4 = 0.0f;
        }

        public void AddWeight( int index, float weight )
        {
            if ( Index1 < 0 )
            {
                Index1 = index;
                Weight1 = weight;
            }
            else if ( Index2 < 0 )
            {
                Index2 = index;
                Weight2 = weight;
            }
            else if ( Index3 < 0 )
            {
                Index3 = index;
                Weight3 = weight;
            }
            else if ( Index4 < 0 )
            {
                Index4 = index;
                Weight4 = weight;
            }
        }

        public override string ToString()
        {
            return $"<({Index1}, {Weight1}), ({Index2}, {Weight2}), ({Index3}, {Weight3}), ({Index4}, {Weight4})>";
        }
    }
}