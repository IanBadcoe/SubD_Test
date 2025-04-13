using Godot;
using SubD;

[Tool]
public partial class CylinderFirstTest : Node3D
{
    bool Clean = false;

    public override void _Process(double delta)
    {
        if (!Clean)
        {
            Clean = true;

            BuildFromCylinder bfc = new();
            CatmullClarkSubdivider ccs = new();

            // bfc.AddSection(new CylSection(4, 1, 6, CylSection.SectionSolidity.Hollow, 2));
            // bfc.AddSection(new CylSection(4, 0.1f, 6, CylSection.SectionSolidity.Hollow, 2));
            // bfc.AddSection(new CylSection(3.75f, 0, 6, CylSection.SectionSolidity.Hollow, 1.5f));
            // bfc.AddSection(new CylSection(4, 0.1f, 6, CylSection.SectionSolidity.Hollow, 2));
            // bfc.AddSection(new CylSection(4, 1, 6, CylSection.SectionSolidity.Hollow, 2));
            // bfc.AddSection(new CylSection(3, 1, 6));
            // bfc.AddSection(new CylSection(3, 1, 6));
            // bfc.AddSection(new CylSection(3, 1, 6, CylSection.SectionSolidity.Hollow));
            // bfc.AddSection(new CylSection(3, 1, 6, CylSection.SectionSolidity.Hollow));
            // bfc.AddSection(new CylSection(4, 0, 6, CylSection.SectionSolidity.Hollow));
            // bfc.AddSection(new CylSection(4, 1, 6, CylSection.SectionSolidity.Hollow));
            // bfc.AddSection(new CylSection(3, 0, 6, CylSection.SectionSolidity.Hollow));
            // bfc.AddSection(new CylSection(3, 1, 6, CylSection.SectionSolidity.Hollow));

            // bfc.AddSection(new CylSection(3, 3, 6));
            // bfc.AddSection(new CylSection(3, 0, 6));
            // bfc.AddSection(new CylSection(2, -2, 6));
            // bfc.AddSection(new CylSection(2, 0, 6));
            // bfc.AddSection(new CylSection(1, 7, 6));
            // bfc.AddSection(new CylSection(1, 0, 6));
            // // bfc.AddSection(new CylSection(1, 0, 6));
            // // bfc.AddSection(new CylSection(1, 10, 6));

            bfc.AddSection(new CylSection(3, 0, 6));
            bfc.AddSection(new CylSection(3, 3, 6));
            bfc.AddSection(new CylSection(3, 0.1f, 6));
            bfc.AddSection(new CylSection(4, 0, 6));
            bfc.AddSection(new CylSection(4, 0.1f, 6));
            bfc.AddSection(new CylSection(4, 3, 6, CylSection.SectionSolidity.Hollow));
            bfc.AddSection(new CylSection(4, 0.2f, 6, CylSection.SectionSolidity.Hollow));
            bfc.AddSection(new CylSection(4.5f, 0.2f, 6, CylSection.SectionSolidity.Hollow, 1.5f));
            bfc.AddSection(new CylSection(4, 0.2f, 6, CylSection.SectionSolidity.Hollow));
            bfc.AddSection(new CylSection(4.5f, 0.2f, 6, CylSection.SectionSolidity.Hollow, 1.5f));
            bfc.AddSection(new CylSection(4, 0.2f, 6, CylSection.SectionSolidity.Hollow));
            bfc.AddSection(new CylSection(4.5f, 0.2f, 6, CylSection.SectionSolidity.Hollow, 1.5f));
            bfc.AddSection(new CylSection(4, 0.2f, 6, CylSection.SectionSolidity.Hollow));
            bfc.AddSection(new CylSection(4.5f, 0.2f, 6, CylSection.SectionSolidity.Hollow, 1.5f));
            bfc.AddSection(new CylSection(4, 0.2f, 6, CylSection.SectionSolidity.Hollow));
            bfc.AddSection(new CylSection(4.5f, 0.2f, 6, CylSection.SectionSolidity.Hollow, 1.5f));
            bfc.AddSection(new CylSection(4, 0.2f, 6, CylSection.SectionSolidity.Hollow));


            Surface surf = bfc.ToSurface();
            surf = ccs.Subdivide(surf);
            surf = ccs.Subdivide(surf);

            Mesh mesh = surf.ToMesh(Surface.MeshMode.Surface, new Surface.MeshOptions { Normals_IncludePoly = true });

            GetNode<MeshInstance3D>("MeshInstance3D").Mesh = mesh;
        }
    }
}
