using System;

using Godot;

using SubD;
using SubD.Builders;

[Tool]
public partial class Floor : MeshInstance3D
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

        for(int i = -10; i <= 10; i++)
        {
            for(int j = -10; j <= 10; j++)
            {
                bfc.AddCube(new Vector3I(i, -1, j), 1);
            }
        }

        var surf = bfc.ToSurface();

        // foreach(Edge edge in surf.Edges.Values)
        // {
        //     edge.IsSharp = true;
        // }

        var sd = new CatmullClarkSubdivider();
        surf = sd.Subdivide(surf);
        surf = sd.Subdivide(surf);
        // surf = sd.Subdivide(surf);
        // // Mesh = surf.ToMeshLines(false);
        Mesh = surf.ToMesh(Surface.MeshMode.Surface);
    }
}
