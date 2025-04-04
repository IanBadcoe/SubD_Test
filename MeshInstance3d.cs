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
        bfc.SetCube(new Vector3I(1, 0, 0), 1);
        bfc.SetCube(new Vector3I(0, 1, 0), 1);
        bfc.SetCube(new Vector3I(1, 1, 0), 1);
        bfc.SetCube(new Vector3I(0, 0, 1), 1);
        bfc.SetCube(new Vector3I(1, 0, 1), 1);
        bfc.SetCube(new Vector3I(0, 1, 1), 1);
        bfc.SetCube(new Vector3I(1, 1, 1), 1);
        // bfc.SetCube(new Vector3I(2, 0, 0), 1);
        // bfc.SetCube(new Vector3I(2, 2, 0), 1);
        // bfc.SetCube(new Vector3I(0, 0, 2), 1);
        // bfc.SetCube(new Vector3I(0, -2, 0), 1);
        // bfc.SetCube(new Vector3I(0, 0, -2), 1);
        // bfc.SetCube(new Vector3I(1, 1, 1), 1);

        Surface surf = bfc.ToSurface();
        Mesh = surf.ToMesh();

        // ArrayMesh mesh = new ArrayMesh();

        // Vector3[] verts = new Vector3[] {
        //     new Vector3(-1, -1, -1),
        //     new Vector3( 1, -1, -1),
        //     new Vector3( 1, -1,  1),
        //     new Vector3(-1, -1,  1),
        //     new Vector3(-1,  1, -1),
        //     new Vector3( 1,  1, -1),
        //     new Vector3( 1,  1,  1),
        //     new Vector3(-1,  1,  1),
        // };

        // int[] idxs = new int[] {
        //     0, 2, 1,
        //     0, 3, 2,
        // };

        // var arrays = new Godot.Collections.Array();
        // arrays.Resize((int)Mesh.ArrayType.Max);
        // arrays[(int)Mesh.ArrayType.Vertex] = verts;
        // arrays[(int)Mesh.ArrayType.Index] = idxs;

        // // Create the Mesh.
        // mesh.AddSurfaceFromArrays(Mesh.PrimitiveType.Triangles, arrays);

        // Mesh = mesh;
    }
}
