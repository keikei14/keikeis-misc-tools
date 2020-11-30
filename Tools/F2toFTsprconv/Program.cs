using System;
using System.IO;
using MikuMikuLibrary.IO;
using MikuMikuLibrary.Sprites;

namespace F2toFTsprconv
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            foreach (string path in args)
            {
                if (File.Exists(path))
                {
                    ProcessFile(path);
                }
                else if (Directory.Exists(path))
                {
                    ProcessDirectory(path);
                }
                else
                {
                    Console.WriteLine("{0} is not a valid file or directory.", path);
                }
            }
        }
        public static void ProcessDirectory(string targetDirectory)
        {
            string[] fileEntries = Directory.GetFiles(targetDirectory);
            foreach (string fileName in fileEntries)
                ProcessFile(fileName);

            string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
            foreach (string subdirectory in subdirectoryEntries)
                ProcessDirectory(subdirectory);
        }

        public static void ProcessFile(string path)
        {
            string destinationFileName = path;
            if (path.EndsWith(".spr"))
            {
                destinationFileName = Path.ChangeExtension(destinationFileName, "bin");
                SpriteSet spr = BinaryFile.Load<SpriteSet>(path);
                foreach ( Sprite sprite in spr.Sprites)
                {
                    sprite.ResolutionMode = ResolutionMode.HDTV720;
                    sprite.Name = "MD_IMG";
                }
                spr.Format = BinaryFormat.DT;
                spr.TextureSet.Format = BinaryFormat.DT;
                spr.Endianness = Endianness.Little;
                spr.TextureSet.Endianness = Endianness.Little;
                spr.Save(destinationFileName);
            }
        }
    }
}
