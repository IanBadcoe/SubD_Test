using System;
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

            Transform3D transform = Transform3D.Identity;
            transform = transform.RotatedLocal(new Vector3(1, 0, 0), 30 * MathF.PI / 180);
            transform = transform.Translated(new Vector3(0, 3, 0));
            Transform3D transform2 = Transform3D.Identity;
            transform2 = transform2.RotatedLocal(new Vector3(0, 0, 1), 30 * MathF.PI / 180);
            transform2 = transform2.Translated(new Vector3(0, 3, 0));
            bfc.AddSection(new CylSection(solidity: CylSection.SectionSolidity.Hollow, radius: 3, length: 0));
            bfc.AddSection(new CylSection(solidity: CylSection.SectionSolidity.Hollow, radius: 3, transform: transform));
            bfc.AddSection(new CylSection(solidity: CylSection.SectionSolidity.Hollow, radius: 3, transform: transform));
            bfc.AddSection(new CylSection(solidity: CylSection.SectionSolidity.Hollow, radius: 3, transform: transform));
            bfc.AddSection(new CylSection(solidity: CylSection.SectionSolidity.Hollow, radius: 3, length: 3));
            bfc.AddSection(new CylSection(solidity: CylSection.SectionSolidity.Hollow, radius: 3, transform: transform2));
            bfc.AddSection(new CylSection(solidity: CylSection.SectionSolidity.Hollow, radius: 3, transform: transform2));
            bfc.AddSection(new CylSection(solidity: CylSection.SectionSolidity.Hollow, radius: 3, transform: transform2));
            bfc.AddSection(new CylSection(solidity: CylSection.SectionSolidity.Hollow, radius: 3, length: 5));
            bfc.AddSection(new CylSection(solidity: CylSection.SectionSolidity.Hollow, radius: 3, transform: transform));
            bfc.AddSection(new CylSection(solidity: CylSection.SectionSolidity.Hollow, radius: 3, transform: transform));
            bfc.AddSection(new CylSection(solidity: CylSection.SectionSolidity.Hollow, radius: 3, transform: transform));

            // bfc.AddSection(new CylSection(3, 0, 6));
            // bfc.AddSection(new CylSection(3, 3, 6, offset_angle_degrees: 20));
            // bfc.AddSection(new CylSection(3, 3, 6, offset_angle_degrees: 20));
            // bfc.AddSection(new CylSection(3, 3, 6, offset_angle_degrees: 20));

            // bfc.AddSection(new CylSection(3, 0, 6));
            // bfc.AddSection(new CylSection(3, 0.1f, 6));
            // bfc.AddSection(new CylSection(4, 0, 6));
            // bfc.AddSection(new CylSection(4, 0.1f, 6));
            // bfc.AddSection(new CylSection(4, 3, 6, CylSection.SectionSolidity.Hollow));
            // bfc.AddSection(new CylSection(4, 0.2f, 6, CylSection.SectionSolidity.Hollow));
            // bfc.AddSection(new CylSection(4.5f, 0.2f, 6, CylSection.SectionSolidity.Hollow, 1.5f));
            // bfc.AddSection(new CylSection(4, 0.2f, 6, CylSection.SectionSolidity.Hollow));
            // bfc.AddSection(new CylSection(4.5f, 0.2f, 6, CylSection.SectionSolidity.Hollow, 1.5f));
            // bfc.AddSection(new CylSection(4, 0.2f, 6, CylSection.SectionSolidity.Hollow));
            // bfc.AddSection(new CylSection(4.5f, 0.2f, 6, CylSection.SectionSolidity.Hollow, 1.5f));
            // bfc.AddSection(new CylSection(4, 0.2f, 6, CylSection.SectionSolidity.Hollow));
            // bfc.AddSection(new CylSection(4.5f, 0.2f, 6, CylSection.SectionSolidity.Hollow, 1.5f));
            // bfc.AddSection(new CylSection(4, 0.2f, 6, CylSection.SectionSolidity.Hollow));
            // bfc.AddSection(new CylSection(4.5f, 0.2f, 6, CylSection.SectionSolidity.Hollow, 1.5f));
            // bfc.AddSection(new CylSection(4, 0.2f, 6, CylSection.SectionSolidity.Hollow));


            Surface surf = bfc.ToSurface();
            surf = ccs.Subdivide(surf);
            surf = ccs.Subdivide(surf);

            GetNode<MeshInstance3D>("Surface").Mesh = surf.ToMesh(Surface.MeshMode.Surface);
            GetNode<MeshInstance3D>("Mesh").Mesh = surf.ToMesh(Surface.MeshMode.Edges);
            GetNode<MeshInstance3D>("Normals").Mesh = surf.ToMesh(Surface.MeshMode.Normals, new Surface.MeshOptions{ DrawNormalsLength = 0.2f, Normals_IncludePoly = true });
        }
    }
}
