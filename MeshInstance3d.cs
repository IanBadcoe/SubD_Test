using Godot;
using System;
using System.CodeDom.Compiler;
using SubD;
using System.Diagnostics;

[Tool]
public partial class MeshInstance3d : MeshInstance3D
{
    bool Clean = false;

    public override void _Process(double delta)
    {
        if (!Clean)
        {
            Generate();

            Clean = true;
        }
    }

    void Generate()
    {
        BuildFromCubes bfc = new BuildFromCubes();
        bfc.SetCube(new Vector3I(0, 0, 0), 1);
        // bfc.SetCube(new Vector3I(1, 0, 0), 1);
        // bfc.SetCube(new Vector3I(0, 1, 0), 1);
        // bfc.SetCube(new Vector3I(1, 1, 0), 1);
        // bfc.SetCube(new Vector3I(0, 0, 1), 1);
        // bfc.SetCube(new Vector3I(1, 0, 1), 1);
        // bfc.SetCube(new Vector3I(0, 1, 1), 1);
        // bfc.SetCube(new Vector3I(1, 1, 1), 1);

        Surface surf = bfc.ToSurface();

        Vert v = surf.GetVert(new Vector3(0.5f, 0.5f, 0.5f));
        v.IsSharp = true;
        v = surf.GetVert(new Vector3(-0.5f, 0.5f, 0.5f));
        v.IsSharp = true;
        v = surf.GetVert(new Vector3(-0.5f, 0.5f, -0.5f));
        v.IsSharp = true;
        v = surf.GetVert(new Vector3(0.5f, 0.5f, -0.5f));
        v.IsSharp = true;

        Edge e = surf.GetEdge(new Vector3(-0.5f, -0.5f, -0.5f), new Vector3(-0.5f, -0.5f, 0.5f));
        e.IsSharp = true;
        e = surf.GetEdge(new Vector3(-0.5f, -0.5f, 0.5f), new Vector3(0.5f, -0.5f, 0.5f));
        e.IsSharp = true;
        e = surf.GetEdge(new Vector3(0.5f, -0.5f, 0.5f), new Vector3(0.5f, -0.5f, -0.5f));
        e.IsSharp = true;
        e = surf.GetEdge(new Vector3(0.5f, -0.5f, -0.5f), new Vector3(-0.5f, -0.5f, -0.5f));
        e.IsSharp = true;

        // foreach(Edge edge in surf.Edges.Values)
        // {
        //     edge.IsSharp = true;
        // }

        var sd = new CatmullClarkSubdivider();
        surf = sd.Subdivide(surf);
        surf = sd.Subdivide(surf);
        surf = sd.Subdivide(surf);
        surf = sd.Subdivide(surf);
        // Mesh = surf.ToMeshLines(false);
        Mesh = surf.ToMesh();
    }
}
