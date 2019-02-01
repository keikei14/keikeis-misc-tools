﻿using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace MikuMikuLibrary.Motions
{
    public class KeySetVector
    {
        public KeySet X { get; set; }
        public KeySet Y { get; set; }
        public KeySet Z { get; set; }

        public IEnumerable<int> Frames
        {
            get
            {
                var enumerable = Enumerable.Empty<int>();
                if ( X != null )
                    enumerable = enumerable.Concat( X.Keys.Select( x => x.Frame ) );
                if ( Y != null )
                    enumerable = enumerable.Concat( Y.Keys.Select( x => x.Frame ) );
                if ( Z != null )
                    enumerable = enumerable.Concat( Z.Keys.Select( x => x.Frame ) );

                return enumerable.Distinct().OrderBy( x => x );
            }
        }

        public Vector3 Interpolate( float frame )
        {
            float x = X?.Interpolate( frame ) ?? 0;
            float y = Y?.Interpolate( frame ) ?? 0;
            float z = Z?.Interpolate( frame ) ?? 0;

            return new Vector3( x, y, z );
        }
    }
}