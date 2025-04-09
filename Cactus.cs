using Godot;
using System;
using System.CodeDom.Compiler;
using SubD;
using System.Diagnostics;

[Tool]
public partial class Cactus : MeshInstance3D
{
    bool Clean = false;

    public override void _Process(double delta)
    {
        if (!Clean)
        {
            Clean = true;

            Generate();
        }
    }

    void Generate()
    {
        BuildFromCubes bfc = new();
        bfc.AddCube(new Vector3I(-2, 0, -3));
        bfc.AddCube(new Vector3I(-2, 1, -3));
        bfc.AddCube(new Vector3I(-2, 2, -3));
        bfc.AddCube(new Vector3I(-2, 3, -3));
        bfc.AddCube(new Vector3I(-2, 4, -3));

        bfc.AddCube(new Vector3I(-1, 4, -3));
        bfc.AddCube(new Vector3I(-3, 4, -3));
        bfc.AddCube(new Vector3I(-2, 4, -2));
        bfc.AddCube(new Vector3I(-2, 4, -4));

        bfc.AddCube(new Vector3I(-0, 4, -3));
        bfc.AddCube(new Vector3I(-0, 5, -3));
        bfc.AddCube(new Vector3I(-0, 6, -3));
        bfc.AddCube(new Vector3I(-0, 7, -3));
        bfc.AddCube(new Vector3I(-0, 8, -3));

        bfc.AddCube(new Vector3I(-4, 4, -3));
        bfc.AddCube(new Vector3I(-4, 5, -3));
        bfc.AddCube(new Vector3I(-4, 6, -3));
        bfc.AddCube(new Vector3I(-4, 7, -3));
        bfc.AddCube(new Vector3I(-4, 8, -3));

        bfc.AddCube(new Vector3I(-2, 4, -1));
        bfc.AddCube(new Vector3I(-2, 5, -1));
        bfc.AddCube(new Vector3I(-2, 6, -1));
        bfc.AddCube(new Vector3I(-2, 7, -1));
        bfc.AddCube(new Vector3I(-2, 8, -1));

        bfc.AddCube(new Vector3I(-2, 4, -5));
        bfc.AddCube(new Vector3I(-2, 5, -5));
        bfc.AddCube(new Vector3I(-2, 6, -5));
        bfc.AddCube(new Vector3I(-2, 7, -5));
        bfc.AddCube(new Vector3I(-2, 8, -5));

        Surface surf = bfc.ToSurface();
        var sd = new CatmullClarkSubdivider();
        surf = sd.Subdivide(surf);
        surf = sd.Subdivide(surf);
        surf = sd.Subdivide(surf);
        // Mesh = surf.ToMeshLines(false);
        Mesh = surf.ToMesh();
    }
}
