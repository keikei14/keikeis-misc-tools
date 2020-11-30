using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MikuMikuLibrary.Archives;
using MikuMikuLibrary.IO;
using MikuMikuLibrary.Objects;

namespace BatchMatFixer
{
    class Program
    {
        static void Main(string[] args)
        {

            bool matFix = false;
            bool texCoordFix = true;
            bool texIdFix = false;
            string sourceFileName = null;

            for (int i = 0; i < args.Length; i++)
            {
                string arg = args[i];

                if (EqualsAny("-m", "--matfix"))
                    matFix = true;

                else if (EqualsAny("-tc=f", "--texcoordfix=f"))
                    texCoordFix = false;

                else if (EqualsAny("-tid", "--texidfix"))
                    texIdFix = true;

                else if (sourceFileName == null)
                    sourceFileName = arg;

                bool EqualsAny(params string[] strings)
                {
                    return strings.Contains(arg, StringComparer.OrdinalIgnoreCase);
                }
            }

            var farcArchive = BinaryFile.Load<FarcArchive>(sourceFileName);

            foreach (var fileName in farcArchive)
            {
                var memoryStream = new MemoryStream();

                if (fileName.EndsWith("_obj.bin"))
                {
                    var source = farcArchive.Open(fileName, EntryStreamMode.MemoryStream);
                    var ObjSet = BinaryFile.Load<ObjectSet>(source);

                    foreach (var obj in ObjSet.Objects)
                    {
                        if (matFix)
                        {
                            foreach (var mat in obj.Materials)
                            {
                                mat.PhongShading = true;
                                mat.LambertShading = true;
                            }
                        }

                        if (texCoordFix)
                        {
                            foreach (var mesh in obj.Meshes)
                            {
                                foreach (var submesh in mesh.SubMeshes)
                                {
                                    submesh.TexCoordIndices[0] = 0;
                                    submesh.TexCoordIndices[1] = 1;
                                    submesh.TexCoordIndices[2] = 0;
                                    submesh.TexCoordIndices[3] = 0;
                                    submesh.TexCoordIndices[4] = 0;
                                    submesh.TexCoordIndices[5] = 0;
                                    submesh.TexCoordIndices[6] = 0;
                                    submesh.TexCoordIndices[7] = 0;
                                }
                            }
                        }
                    }

                    if (texIdFix)
                    {
                        var TexIds = ObjSet.TextureIds;
                        var NewTexIds = new List<uint>();

                        foreach(var Id in TexIds)
                        {
                            NewTexIds.Add(Id + 100000);
                        }
                        ObjSet.TextureIds.Clear();
                        ObjSet.TextureIds.AddRange(NewTexIds);

                        if (ObjSet.Objects.Count != 0)
                        {
                            foreach (var obj in ObjSet.Objects)
                            {
                                foreach (var mat in obj.Materials)
                                {
                                    foreach (var matTex in mat.MaterialTextures)
                                    {
                                        matTex.TextureId += 100000;
                                    }
                                }
                            }
                        }

                    }
                    ObjSet.Save(memoryStream, true);
                    farcArchive.Add(fileName, memoryStream, false, ConflictPolicy.Replace);
                }
            }
            farcArchive.Save(sourceFileName);
        }
    }
}
