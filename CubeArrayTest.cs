using Godot;

using Chickensoft.Introspection;
using Chickensoft.AutoInject;

using SubD;
using SubD.Builders;


[Tool]
[Meta(typeof(IAutoConnect))]
public partial class CubeArrayTest : Node3D
{
    // --------------------------------------------------------------
    // IAutoNode boilerplate
    public override void _Notification(int what) => this.Notify(what);
    // --------------------------------------------------------------

    bool Clean = false;

    [Node]
    MeshInstance3D Surface { get; set; }

    [Node]
    MeshInstance3D Mesh { get; set; }

    [Node]
    MeshInstance3D SharpMesh { get; set; }

    public override void _Process(double delta)
    {
        if (!Clean)
        {
            Clean = true;

            Construct();
        }
    }

    void Construct()
    {
        BuildFromCubes bfc = new();
        CatmullClarkSubdivider ccs = new();

        const int size = 5;

        for(int x = -size; x <= size; x+=2)
        {
            for(int y = -size; y <= size; y+=2)
            {
                for(int z = -size; z <= size; z++)
                {
                    Cube cube = bfc.AddCube(new Vector3I(x, y, z), 1);
                    if (z % 2 == 0)
                    {
                        if (y != -size || x != -size)
                        {
                            cube.IsEdgeSharp[Cube.EdgeName.BottomLeft] = true;
                        }
                        if (y != size || x != -size)
                        {
                            cube.IsEdgeSharp[Cube.EdgeName.TopLeft] = true;
                        }
                        if (y != size || x != size)
                        {
                            cube.IsEdgeSharp[Cube.EdgeName.TopRight] = true;
                        }
                        if (y != -size || x != size)
                        {
                            cube.IsEdgeSharp[Cube.EdgeName.BottomRight] = true;
                        }
                    }
                }
            }
        }

        for(int x = -size + 1; x <= size - 1; x+=2)
        {
            for(int y = -size; y <= size; y+=2)
            {
                for(int z = -size; z <= size; z+=2)
                {
                    Cube cube = bfc.AddCube(new Vector3I(x, y, z), 1);
                    if (y != size || z != size)
                    {
                        cube.IsEdgeSharp[Cube.EdgeName.TopFront] = true;
                    }
                    if (y != size || z != -size)
                    {
                        cube.IsEdgeSharp[Cube.EdgeName.TopBack] = true;
                    }
                    if (y != -size || z != size)
                    {
                        cube.IsEdgeSharp[Cube.EdgeName.BottomFront] = true;
                    }
                    if (y != -size || z != -size)
                    {
                        cube.IsEdgeSharp[Cube.EdgeName.BottomBack] = true;
                    }
                }
            }
        }

        for(int x = -size; x <= size; x+=2)
        {
            for(int y = -size + 1; y <= size - 1; y+=2)
            {
                for(int z = -size; z <= size; z+=2)
                {
                    Cube cube = bfc.AddCube(new Vector3I(x, y, z), 1);
                    if (x != -size || z != -size)
                    {
                        cube.IsEdgeSharp[Cube.EdgeName.BackLeft] = true;
                    }
                    if (x != size || z != -size)
                    {
                        cube.IsEdgeSharp[Cube.EdgeName.BackRight] = true;
                    }
                    if (x != -size || z != size)
                    {
                        cube.IsEdgeSharp[Cube.EdgeName.FrontLeft] = true;
                    }
                    if (x != size || z != size)
                    {
                        cube.IsEdgeSharp[Cube.EdgeName.FrontRight] = true;
                    }
                }
            }
        }

        static Vector3 distortion(Vector3 v) => new Vector3(
            // v.X, v.Y * 2, v.Z * 3
            v.X + Mathf.Sin(v.Y + Mathf.Sin(v.Z)) / 2,
            v.Y + Mathf.Sin(v.Z + Mathf.Sin(v.X)) / 2,
            v.Z + Mathf.Sin(v.X + Mathf.Sin(v.Y)) / 2
        );

        Surface surf = bfc.ToSurface();
        surf.Distort(distortion);
        surf = ccs.Subdivide(surf);
        surf = ccs.Subdivide(surf);
        // surf = ccs.Subdivide(surf);

        Surface.Mesh = surf.ToMesh(SubD.Surface.MeshMode.Surface);
        SharpMesh.Mesh = surf.ToMesh(SubD.Surface.MeshMode.Edges, new MeshOptions(){ Edges_Offset = 0.01f, Edges_IncludeSmooth = false });
        Mesh.Mesh = surf.ToMesh(SubD.Surface.MeshMode.Edges, new MeshOptions(){ Edges_Offset = 0.01f, Edges_IncludeSharp = false });
    }
}
