using Godot;

using SubD;
using SubD.Builders;

using Godot_Util;

using VIdx = SubD.Idx<SubD.Vert>;

[Tool]
public partial class VertSplitTest : Node3D
{
    bool Clean = false;

    void TestCase(bool e1sharp, bool e2sharp, bool e3sharp, bool e4sharp)
    {
        BuildFromCubes bfc = new();
        CatmullClarkSubdivider ccs = new();

        Cube cube = bfc.AddCube(new Vector3I(0, 0, 0));
        EdgeNameUtils.BackEdges.ForEach(x => cube.IsEdgeSharp[x] = true);
        cube.VertTag[Cube.VertName.BottomBackRight] = "xxx";

        // Cube cube1 = bfc.AddCube(new Vector3I(0, 0, 0));
        // cube1.IsEdgeSharp[Cube.EdgeName.TopLeft] = e1sharp;
        // Cube cube2 = bfc.AddCube(new Vector3I(0, 0, -1));
        // cube2.IsEdgeSharp[Cube.EdgeName.TopFront] = e2sharp;
        // Cube cube3 = bfc.AddCube(new Vector3I(-1, 0, -1));
        // cube3.IsEdgeSharp[Cube.EdgeName.TopRight] = e3sharp;
        // Cube cube4 = bfc.AddCube(new Vector3I(-1, 0, 0));
        // cube4.IsEdgeSharp[Cube.EdgeName.TopBack] = e4sharp;

        // Util.Assert(cube1.GetVert(Cube.VertName.TopBackLeft) == cube2.GetVert(Cube.VertName.TopFrontLeft));
        // Util.Assert(cube1.GetVert(Cube.VertName.TopBackLeft) == cube3.GetVert(Cube.VertName.TopFrontRight));
        // Util.Assert(cube1.GetVert(Cube.VertName.TopBackLeft) == cube4.GetVert(Cube.VertName.TopBackRight));

        // Vector3 pos_of_interest = cube1.GetVert(Cube.VertName.TopBackLeft);

        Surface surf = bfc.ToSurface();
        surf = ccs.Subdivide(surf);

        // Vert vert_of_interest = surf.GetVert(pos_of_interest);
        // VIdx? v_idx_of_interest = surf.GetVIdx(pos_of_interest);

        GetNode<MeshInstance3D>("Object").Mesh = surf.ToMesh(Surface.MeshMode.Surface);

        GetNode<MeshInstance3D>("Annotation").Mesh = surf.ToMesh(Surface.MeshMode.Normals,
            new MeshOptions { Normals_IncludeSplitVert = true });

        GetNode<MeshInstance3D>("Annotation2").Mesh = surf.ToMesh(Surface.MeshMode.Edges,
            new MeshOptions { Edges_IncludeSmooth = false });
    }

    public override void _Process(double delta)
    {
        if (!Clean)
        {
            Clean = true;

            TestCase(false, true, false, true);
        }
    }
}
