using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MikuMikuLibrary.IO;
using MikuMikuLibrary.Archives;
using MikuMikuLibrary.Databases;
using MikuMikuLibrary.Textures;
using MikuMikuLibrary.Textures.Processing;

namespace BatchXtoF2
{
    class Program
    {
        static void Main(string[] args)
        {

            var stgFarc = BinaryFile.Load<FarcArchive>(args[0]);

            foreach (string fileName in stgFarc)
            {
                if (fileName.EndsWith(".txd"))
                {
                    MemoryStream stream = new MemoryStream();
                    var oldTexSet = BinaryFile.Load<TextureSet>(stgFarc.Open(fileName, EntryStreamMode.MemoryStream));
                    var newTexSet = new TextureSet();

                    foreach (var tex in oldTexSet.Textures)
                    {
                        Texture newTex = TextureEncoder.EncodeFromFile(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\white.dds", tex.Format, true);
                        newTex.Id = tex.Id;
                        newTex.Name = tex.Name;
                        newTexSet.Textures.Add(newTex);

                    }
                    newTexSet.Endianness = oldTexSet.Endianness;
                    newTexSet.Format = oldTexSet.Format;

                    newTexSet.Save(stream, true);
                    stgFarc.Add(fileName, stream, false, ConflictPolicy.Replace);
                }
            }
            stgFarc.Save(args[0]);
        }
    }
}
