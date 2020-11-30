using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MikuMikuLibrary.IO;
using MikuMikuLibrary.Databases;
using MikuMikuLibrary.Hashes;


namespace DbMurmurHasher
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args[0].EndsWith("spr_db.bin"))
            {
                var SpriteDb = BinaryFile.Load<SpriteDatabase>(args[0]);

                foreach (var SpriteSet in SpriteDb.SpriteSets)
                {
                    Console.WriteLine(SpriteSet.Name);
                    SpriteSet.Id = MurmurHash.Calculate(SpriteSet.Name);
                    Console.WriteLine("Murmur Hashed Id: " + SpriteSet.Id +"\n");

                    foreach (var Sprite in SpriteSet.Sprites)
                    {
                        Console.WriteLine(Sprite.Name);
                        Sprite.Id = MurmurHash.Calculate(Sprite.Name);
                        Console.WriteLine("Murmur Hashed Id: " + Sprite.Id + "\n");
                    }
                }
                SpriteDb.Save(args[0]);
            }

            if (args[0].EndsWith("aet_db.bin"))
            {
                var AetDb = BinaryFile.Load<AetDatabase>(args[0]);

                foreach (var AetSet in AetDb.AetSets)
                {
                    AetSet.SpriteSetId = MurmurHash.Calculate(AetSet.Name.Replace("AET_", "SPR_"));
                }
                AetDb.Save(args[0]);
            }
        }
    }
}
