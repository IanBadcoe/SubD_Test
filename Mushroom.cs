using Godot;
using System;
using SubD;
using System.Linq;

[Tool]
public partial class Mushroom : MeshInstance3D
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

        bfc.AddCube(new Vector3I(1, 3, 2));
        bfc.AddCube(new Vector3I(1, 3, 3));
        bfc.AddCube(new Vector3I(1, 3, 4));
        bfc.AddCube(new Vector3I(1, 3, 5));
        bfc.AddCube(new Vector3I(1, 3, 6));
        bfc.AddCube(new Vector3I(2, 3, 2));
        bfc.AddCube(new Vector3I(2, 3, 3));
        bfc.AddCube(new Vector3I(2, 3, 4));
        bfc.AddCube(new Vector3I(2, 3, 5));
        bfc.AddCube(new Vector3I(2, 3, 6));
        bfc.AddCube(new Vector3I(3, 3, 2));
        bfc.AddCube(new Vector3I(3, 3, 3));
        bfc.AddCube(new Vector3I(3, 3, 4));
        bfc.AddCube(new Vector3I(3, 3, 5));
        bfc.AddCube(new Vector3I(3, 3, 6));
        bfc.AddCube(new Vector3I(4, 3, 2));
        bfc.AddCube(new Vector3I(4, 3, 3));
        bfc.AddCube(new Vector3I(4, 3, 4));
        bfc.AddCube(new Vector3I(4, 3, 5));
        bfc.AddCube(new Vector3I(4, 3, 6));
        bfc.AddCube(new Vector3I(5, 3, 2));
        bfc.AddCube(new Vector3I(5, 3, 3));
        bfc.AddCube(new Vector3I(5, 3, 4));
        bfc.AddCube(new Vector3I(5, 3, 5));
        bfc.AddCube(new Vector3I(5, 3, 6));

        bfc.AddCube(new Vector3I(3, 0, 4));
        bfc.AddCube(new Vector3I(3, 1, 4));
        bfc.AddCube(new Vector3I(3, 2, 4));

        Surface surf = bfc.ToSurface();

        Vector3[] ring_verts = [
            new Vector3(3.5f, -0.5f, 4.5f),
            new Vector3(3.5f, -0.5f, 3.5f),
            new Vector3(2.5f, -0.5f, 3.5f),
            new Vector3(2.5f, -0.5f, 4.5f),
        ];

        for (int i = 0; i < 3; i++)
        {
            Vector3[] here_positions = ring_verts.Select(x => x + new Vector3(0, i, 0)).ToArray();

            Vector3 prev_pos = here_positions.Last();

            foreach(var pos in here_positions)
            {
                Edge e = surf.GetEdge(prev_pos, pos);
                e.IsSharp = true;

                prev_pos = pos;
            }
        }

        for(int i = 0; i < 5; i++)
        {
            {
                Vector3 p1 = new(0.5f + i, 2.5f, 1.5f);
                Vector3 p2 = new(1.5f + i, 2.5f, 1.5f);

                Edge e = surf.GetEdge(p1, p2);
                e.IsSharp = true;
            }

            {
                Vector3 p1 = new(0.5f + i, 2.5f, 6.5f);
                Vector3 p2 = new(1.5f + i, 2.5f, 6.5f);

                Edge e = surf.GetEdge(p1, p2);
                e.IsSharp = true;
            }

            {
                Vector3 p1 = new(0.5f, 2.5f, 1.5f + i);
                Vector3 p2 = new(0.5f, 2.5f, 2.5f + i);

                Edge e = surf.GetEdge(p1, p2);
                e.IsSharp = true;
            }

            {
                Vector3 p1 = new(5.5f, 2.5f, 1.5f + i);
                Vector3 p2 = new(5.5f, 2.5f, 2.5f + i);

                Edge e = surf.GetEdge(p1, p2);
                e.IsSharp = true;
            }
        }

        var sd = new CatmullClarkSubdivider();
        surf = sd.Subdivide(surf);
        surf = sd.Subdivide(surf);
        surf = sd.Subdivide(surf);
        // // Mesh = surf.ToMeshLines(false);
        Mesh = surf.ToMesh();
    }
}
