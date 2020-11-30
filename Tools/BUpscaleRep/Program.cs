using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using MikuMikuLibrary.IO;
using MikuMikuLibrary.Databases;
using MikuMikuLibrary.Objects;
using MikuMikuLibrary.Textures;
using MikuMikuLibrary.Textures.Processing;
using MikuMikuLibrary.Archives;

namespace BUpscaleRep
{
    class Program
    {
        private static void Main(string[] args)
        {

            

            foreach (string path in args)
            {
                if (File.Exists(path))
                {
                    ProcessFile(path, args);
                }
                else if (Directory.Exists(path))
                {
                    ProcessDirectory(path, args);
                }
                else
                {
                    Console.WriteLine("{0} is not a valid file or directory.", path);
                }
            }
        }
        public static void ProcessDirectory(string targetDirectory, string[] oargs)
        {
            string[] fileEntries = Directory.GetFiles(targetDirectory);
            foreach (string fileName in fileEntries)
                ProcessFile(fileName, oargs);
            string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
            foreach (string subdirectory in subdirectoryEntries)
                ProcessDirectory(subdirectory, oargs);
        }

        public static void ProcessFile(string path, string[] oargs)
        {
            bool export = false;
            bool replace = false;

            for (int i = 0; i < oargs.Length; i++)
            {
                string arg = oargs[i];

                if (EqualsAny("-e", "--export"))
                    export = true;

                else if (EqualsAny("-r", "--replace"))
                    replace = false;

                bool EqualsAny(params string[] strings)
                {
                    return strings.Contains(arg, StringComparer.OrdinalIgnoreCase);
                }
            }

            if (export)
            {
                var texDb = BinaryFile.Load<TextureDatabase>(Path.GetDirectoryName(path) + "\\..\\mdata_tex_db.bin");
                var farcArchive = BinaryFile.Load<FarcArchive>(path);
                var dir = Directory.CreateDirectory(Path.GetDirectoryName(path) + "\\export");

                foreach (string filename in farcArchive)
                {
                    if (filename.EndsWith("_obj.bin"))
                    {
                        var sObjSet = farcArchive.Open(filename, EntryStreamMode.MemoryStream);
                        var sTexSet = farcArchive.Open(filename.Replace("_obj.bin", "_tex.bin"), EntryStreamMode.MemoryStream);
                        var lTexSet = BinaryFile.Load<TextureSet>(sTexSet);
                        var nObjSet = new ObjectSet();

                        nObjSet.Load(sObjSet, lTexSet, texDb);

                        foreach (var Tex in nObjSet.TextureSet.Textures)
                        {

                            string newFilePath = Path.GetDirectoryName(path) + "\\..\\export" + "\\" + filename.Replace("_obj.bin", "") + " - " + Tex.Name + ".dds";

                            Console.WriteLine(Tex.Name);
                            TextureDecoder.DecodeToFile(Tex, newFilePath);
                        }

                    }
                }
            }
        }
    }
}
