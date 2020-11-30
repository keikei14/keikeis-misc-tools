using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MikuMikuLibrary.Archives;
using MikuMikuLibrary.IO;
using MikuMikuLibrary.Motions;
using MikuMikuLibrary.Databases;
using MikuMikuLibrary.Skeletons;

namespace BakedMotFixer
{
    class Program
    {
        static void Main(string[] args)
        {

            var farcArchive = BinaryFile.Load<FarcArchive>(args[0]);
            var boneDb = BinaryFile.Load<BoneDatabase>(Path.GetDirectoryName(args[0]) + "\\bone_data.bin");
            var motDb = BinaryFile.Load<MotionDatabase>(Path.GetDirectoryName(args[0]) + "\\mot_db.bin");

            foreach (var skele in boneDb.Skeletons)
            {
                if (skele.Name == "MIK")
                {
                    Console.WriteLine(skele.Positions.Count);
                    for (int i = 0; i < skele.Positions.Count; i++)
                    {
                        Console.WriteLine(skele.Bones[i].Name);

                        Console.WriteLine(skele.Positions[i].X);
                        Console.WriteLine(skele.Positions[i].Y);
                        Console.WriteLine(skele.Positions[i].Z);
                        Console.WriteLine("\n");
                        Console.WriteLine(i);
                        Console.WriteLine("\n");
                    }
                }
            }

            /* foreach (var fileName in farcArchive)
            {
                var memoryStream = new MemoryStream();

                if (fileName.EndsWith("FACE_MIK.bin"))
                {
                    var src = farcArchive.Open(fileName, EntryStreamMode.MemoryStream);
                    var motSet = BinaryFile.Load<MotionSet>(src, boneDb.Skeletons[1]);

                    foreach (var mot in motSet.Motions)
                    {
                        foreach ( var keySet in mot.KeySets )
                        {
                            foreach (var key in keySet.Keys)
                            {
                                Console.WriteLine(key.Value * 0.30);
                            }
                        }
                    }

                }

            } */

        }
    }
}
