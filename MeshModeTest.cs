using System;

using Godot;

using SubD;
using SubD.Builders;

using Godot_Util;

[Tool]
public partial class MeshModeTest : Node3D
{
    bool Clean = false;

    readonly BuildFromCubes BFC = new();
    readonly CatmullClarkSubdivider CCS = new();

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
        Action<Cube> cube_mods = cube =>
        {
            EdgeNameUtils.TopEdges.ForEach(x => cube.IsEdgeSharp[x] = true);
            cube.IsEdgeSharp[Cube.EdgeName.FrontLeft] = true;
            cube.IsEdgeSharp[Cube.EdgeName.BackRight] = true;
        };

        CreateCube(
            GetNode<MeshInstance3D>("SurfaceSetSharp"),
            cube_mods,
            Surface.MeshMode.Surface, new MeshOptions {}
        );

        CreateCube(
            GetNode<MeshInstance3D>("SurfaceAngleSharp80Degrees"),
            cube_mods,
            Surface.MeshMode.Surface, new MeshOptions { SplitAngleDegrees = 80 }
        );

        CreateCube(
            GetNode<MeshInstance3D>("SurfaceAngleSharp10Degrees"),
            cube_mods,
            Surface.MeshMode.Surface, new MeshOptions { SplitAngleDegrees = 10 }
        );

        // --

        CreateCube(
            GetNode<MeshInstance3D>("EdgesAll"),
            cube_mods,
            Surface.MeshMode.Edges, new MeshOptions {}
        );

        CreateCube(
            GetNode<MeshInstance3D>("EdgesSetSharp"),
            cube_mods,
            Surface.MeshMode.Edges, new MeshOptions { Edges_IncludeSmooth = false }
        );

        CreateCube(
            GetNode<MeshInstance3D>("EdgesAngleSharp80Degrees"),
            cube_mods,
            Surface.MeshMode.Edges, new MeshOptions { Edges_IncludeSmooth = false, Edges_DetermineSmoothnessFromAngle = true, SplitAngleDegrees = 80 }
        );

        CreateCube(
            GetNode<MeshInstance3D>("EdgesAngleSharp10Degrees"),
            cube_mods,
            Surface.MeshMode.Edges, new MeshOptions { Edges_IncludeSmooth = false, Edges_DetermineSmoothnessFromAngle = true, SplitAngleDegrees = 10}
        );

        CreateCube(
            GetNode<MeshInstance3D>("EdgesAngleSharp10Degrees"),
            cube_mods,
            Surface.MeshMode.Edges, new MeshOptions { Edges_IncludeSmooth = false, Edges_DetermineSmoothnessFromAngle = true, SplitAngleDegrees = 10}
        );

        CreateCube(
            GetNode<MeshInstance3D>("TaggedEdges"),
            cube => {
                cube_mods(cube);
                cube.EdgeTag[Cube.EdgeName.TopFront] = "x";
                cube.EdgeTag[Cube.EdgeName.FrontLeft] = "x";
                cube.EdgeTag[Cube.EdgeName.BottomLeft] = "x";
                cube.EdgeTag[Cube.EdgeName.BottomBack] = "x";
                cube.EdgeTag[Cube.EdgeName.BackRight] = "x";
                cube.EdgeTag[Cube.EdgeName.TopRight] = "x";
            },
            Surface.MeshMode.Edges, new MeshOptions { Edges_Filter = edge => edge.Tag == "x" }
        );

        // --

        CreateCubePair(
            "FaceNormals",
            cube_mods,
            Surface.MeshMode.Normals, new MeshOptions { Normals_IncludeFace = true, DrawNormalsLength = 0.3f },
            Surface.MeshMode.Surface, new MeshOptions { SplitAngleDegrees = 0 }
        );

        CreateCubePair(
            "EdgeNormals",
            cube_mods,
            Surface.MeshMode.Normals, new MeshOptions { Normals_IncludeEdge = true, DrawNormalsLength = 0.3f },
            Surface.MeshMode.Edges, new MeshOptions { }
        );

        CreateCubePair(
            "VertNormals",
            cube_mods,
            Surface.MeshMode.Normals, new MeshOptions { Normals_IncludeVert = true, DrawNormalsLength = 0.3f },
            Surface.MeshMode.Edges, new MeshOptions { }
        );

        CreateCubePair(
            "SplitNormalsFromTagged",
            cube_mods,
            Surface.MeshMode.Normals, new MeshOptions { Normals_IncludeSplitVert = true, DrawNormalsLength = 0.3f },
            Surface.MeshMode.Edges, new MeshOptions { }
        );

        CreateCubePair(
            "SplitNormalsFromAngle80Degrees",
            cube_mods,
            Surface.MeshMode.Normals, new MeshOptions { Normals_IncludeSplitVert = true, DrawNormalsLength = 0.3f, SplitAngleDegrees = 80, Edges_DetermineSmoothnessFromAngle = true },
            Surface.MeshMode.Edges, new MeshOptions { }
        );

        CreateCubePair(
            "SplitNormalsFromAngle10Degrees",
            cube_mods,
            Surface.MeshMode.Normals, new MeshOptions { Normals_IncludeSplitVert = true, DrawNormalsLength = 0.3f, SplitAngleDegrees = 10, Edges_DetermineSmoothnessFromAngle = true },
            Surface.MeshMode.Edges, new MeshOptions { }
        );
    }

    void CreateCubePair(string node, Action<Cube> cube_mods, Surface.MeshMode mode1, MeshOptions options1, Surface.MeshMode mode2, MeshOptions options2)
    {
        CreateCube(GetNode<MeshInstance3D>(node + "/Normals"), cube_mods, mode1, options1);
        CreateCube(GetNode<MeshInstance3D>(node + "/Model"), cube_mods, mode2, options2);
    }


    void CreateCube(MeshInstance3D am, Action<Cube> action, Surface.MeshMode mode, MeshOptions options)
    {
        Cube cube = BFC.AddCube(Vector3I.Zero);

        action(cube);

        Surface surf = BFC.ToSurface();
        surf = CCS.Subdivide(surf);
        surf = CCS.Subdivide(surf);
        surf = CCS.Subdivide(surf);

        am.Mesh = surf.ToMesh(mode, options);

        BFC.Reset();
    }
}
