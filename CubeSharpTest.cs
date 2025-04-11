using Godot;
using System;
using SubD;
using System.Linq;

[Tool]
public partial class CubeSharpTest : Node3D
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
        CreateCube(
            GetNode<MeshInstance3D>("Smooth"),
            cube => {}
        );

        CreateCube(
            GetNode<MeshInstance3D>("EdgesSharp"),
            cube => EdgeNameUtils.AllEdges.ForEach(x => cube.IsEdgeSharp[x] = true)
        );

        CreateCube(
            GetNode<MeshInstance3D>("VertsSharp"),
            cube => VertNameUtils.AllVerts.ForEach(x => cube.IsVertSharp[x] = true)
        );

        // --

        CreateCube(
            GetNode<MeshInstance3D>("TopVertsSharp"),
            cube => VertNameUtils.TopVerts.ForEach(x => cube.IsVertSharp[x] = true)
        );

        CreateCube(
            GetNode<MeshInstance3D>("TopVertsSharp"),
            cube => VertNameUtils.TopVerts.ForEach(x => cube.IsVertSharp[x] = true)
        );

        CreateCube(
            GetNode<MeshInstance3D>("BottomVertsSharp"),
            cube => VertNameUtils.BottomVerts.ForEach(x => cube.IsVertSharp[x] = true)
        );

        CreateCube(
            GetNode<MeshInstance3D>("LeftVertsSharp"),
            cube => VertNameUtils.LeftVerts.ForEach(x => cube.IsVertSharp[x] = true)
        );

        CreateCube(
            GetNode<MeshInstance3D>("RightVertsSharp"),
            cube => VertNameUtils.RightVerts.ForEach(x => cube.IsVertSharp[x] = true)
        );

        CreateCube(
            GetNode<MeshInstance3D>("FrontVertsSharp"),
            cube => VertNameUtils.FrontVerts.ForEach(x => cube.IsVertSharp[x] = true)
        );

        CreateCube(
            GetNode<MeshInstance3D>("BackVertsSharp"),
            cube => VertNameUtils.BackVerts.ForEach(x => cube.IsVertSharp[x] = true)
        );

        // --

        CreateCube(
            GetNode<MeshInstance3D>("TopEdgesSharp"),
            cube => EdgeNameUtils.TopEdges.ForEach(x => cube.IsEdgeSharp[x] = true)
        );

        CreateCube(
            GetNode<MeshInstance3D>("BottomEdgesSharp"),
            cube => EdgeNameUtils.BottomEdges.ForEach(x => cube.IsEdgeSharp[x] = true)
        );

        CreateCube(
            GetNode<MeshInstance3D>("LeftEdgesSharp"),
            cube => EdgeNameUtils.LeftEdges.ForEach(x => cube.IsEdgeSharp[x] = true)
        );

        CreateCube(
            GetNode<MeshInstance3D>("RightEdgesSharp"),
            cube => EdgeNameUtils.RightEdges.ForEach(x => cube.IsEdgeSharp[x] = true)
        );

        CreateCube(
            GetNode<MeshInstance3D>("FrontEdgesSharp"),
            cube => EdgeNameUtils.FrontEdges.ForEach(x => cube.IsEdgeSharp[x] = true)
        );

        CreateCube(
            GetNode<MeshInstance3D>("BackEdgesSharp"),
            cube => EdgeNameUtils.BackEdges.ForEach(x => cube.IsEdgeSharp[x] = true)
        );
    }

    void CreateCube(MeshInstance3D am, Action<Cube> action)
    {
        Cube cube = BFC.AddCube(Vector3I.Zero);

        action(cube);

        Surface surf = BFC.ToSurface();
        surf = CCS.Subdivide(surf);
        surf = CCS.Subdivide(surf);
        surf = CCS.Subdivide(surf);

        am.Mesh = surf.ToMesh(Surface.MeshMode.Surface);
    }
}
