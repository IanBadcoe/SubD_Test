using System;
using Godot;
using SubD;
using SubD.CylTypes;
using SubD.Builders;

[Tool]
public partial class CylinderWrinklesTest : Node3D
{
    bool Clean = false;

    public override void _Process(double delta)
    {
        if (!Clean)
        {
            Clean = true;

            BuildFromCylinders bfc = new();
            CatmullClarkSubdivider ccs = new();

            Func<CylSection, int, Topology, EdgeType, EdgeProps> edge_callback =
                (c, i, t, et) =>
                {
                    bool sharp = c.Tag == "s" && et == EdgeType.Circumferential;

                    return new EdgeProps(sharp);
                };

            Func<CylSection, int, SectorProps> sector_callback =
                (c, i) =>
                {
                    bool holes = c.Tag == "h";

                    return new SectorProps(holes ? new HoleProps(0, clearance: 0.3f) : null);
                };

            bfc.AddSection(new CylSection(solidity: SectionSolidity.Hollow, radius: 6, length: 0, sectors: 8, thickness: 3, edge_callback: edge_callback, sector_callback: sector_callback, rot_x_degrees: 1, tag: "h"));
            bfc.AddSection(new CylSection(solidity: SectionSolidity.Hollow, radius: 6, length: 3, sectors: 8, thickness: 3, edge_callback: edge_callback, sector_callback: sector_callback, rot_x_degrees: 1));
            bfc.AddSection(new CylSection(solidity: SectionSolidity.Hollow, radius: 5, length: 0, sectors: 8, thickness: 2, edge_callback: edge_callback, sector_callback: sector_callback, rot_x_degrees: 1));
            bfc.AddSection(new CylSection(solidity: SectionSolidity.Hollow, radius: 4, length: -1, sectors: 8, thickness: 1, edge_callback: edge_callback, sector_callback: sector_callback, rot_x_degrees: 1, tag: "s"));
            bfc.AddSection(new CylSection(solidity: SectionSolidity.Hollow, radius: 4.9f, length: 1, sectors: 8, thickness: 2, edge_callback: edge_callback, sector_callback: sector_callback, rot_x_degrees: 1));
            bfc.AddSection(new CylSection(solidity: SectionSolidity.Hollow, radius: 5.5f, length: 0.1f, sectors: 8, thickness: 3, edge_callback: edge_callback, sector_callback: sector_callback, rot_x_degrees: 1));
            bfc.AddSection(new CylSection(solidity: SectionSolidity.Hollow, radius: 5.5f, length: 1, sectors: 8, thickness: 3, edge_callback: edge_callback, sector_callback: sector_callback, rot_x_degrees: 1));
            bfc.AddSection(new CylSection(solidity: SectionSolidity.Hollow, radius: 4.5f, length: 0, sectors: 8, thickness: 2, edge_callback: edge_callback, sector_callback: sector_callback, rot_x_degrees: 1));
            bfc.AddSection(new CylSection(solidity: SectionSolidity.Hollow, radius: 3.5f, length: -1, sectors: 8, thickness: 1, edge_callback: edge_callback, sector_callback: sector_callback, rot_x_degrees: 1, tag: "s"));
            bfc.AddSection(new CylSection(solidity: SectionSolidity.Hollow, radius: 4.5f, length: 1, sectors: 8, thickness: 2, edge_callback: edge_callback, sector_callback: sector_callback, rot_x_degrees: 1));
            bfc.AddSection(new CylSection(solidity: SectionSolidity.Hollow, radius: 5, length: 0, sectors: 8, thickness: 3, edge_callback: edge_callback, sector_callback: sector_callback, rot_x_degrees: 1));
            bfc.AddSection(new CylSection(solidity: SectionSolidity.Hollow, radius: 5, length: 1, sectors: 8, thickness: 3, edge_callback: edge_callback, sector_callback: sector_callback, rot_x_degrees: 1));
            bfc.AddSection(new CylSection(solidity: SectionSolidity.Hollow, radius: 4, length: 0, sectors: 8, thickness: 2, edge_callback: edge_callback, sector_callback: sector_callback, rot_x_degrees: 1));
            bfc.AddSection(new CylSection(solidity: SectionSolidity.Hollow, radius: 3, length: -1, sectors: 8, thickness: 1, edge_callback: edge_callback, sector_callback: sector_callback, rot_x_degrees: 1, tag: "s"));
            bfc.AddSection(new CylSection(solidity: SectionSolidity.Hollow, radius: 4, length: 1, sectors: 8, thickness: 2, edge_callback: edge_callback, sector_callback: sector_callback, rot_x_degrees: 1));
            bfc.AddSection(new CylSection(solidity: SectionSolidity.Hollow, radius: 4.5f, length: 0, sectors: 8, thickness: 3, edge_callback: edge_callback, sector_callback: sector_callback, rot_x_degrees: 1));
            bfc.AddSection(new CylSection(solidity: SectionSolidity.Hollow, radius: 4.5f, length: 1, sectors: 8, thickness: 3, edge_callback: edge_callback, sector_callback: sector_callback, rot_x_degrees: 1));
            bfc.AddSection(new CylSection(solidity: SectionSolidity.Hollow, radius: 3.5f, length: 0, sectors: 8, thickness: 2, edge_callback: edge_callback, sector_callback: sector_callback, rot_x_degrees: 1));
            bfc.AddSection(new CylSection(solidity: SectionSolidity.Hollow, radius: 2.5f, length: -1, sectors: 8, thickness: 1, edge_callback: edge_callback, sector_callback: sector_callback, rot_x_degrees: 1, tag: "s"));
            bfc.AddSection(new CylSection(solidity: SectionSolidity.Hollow, radius: 3.5f, length: 1, sectors: 8, thickness: 2, edge_callback: edge_callback, sector_callback: sector_callback, rot_x_degrees: 1));
            bfc.AddSection(new CylSection(solidity: SectionSolidity.Hollow, radius: 4, length: 0, sectors: 8, thickness: 3, edge_callback: edge_callback, sector_callback: sector_callback, rot_x_degrees: 1));
            bfc.AddSection(new CylSection(solidity: SectionSolidity.Hollow, radius: 2, length: 0, sectors: 8, thickness: 1, edge_callback: edge_callback, sector_callback: sector_callback, rot_x_degrees: 3));
            bfc.AddSection(new CylSection(solidity: SectionSolidity.Hollow, radius: 2, length: 10, sectors: 8, thickness: 1, edge_callback: edge_callback, sector_callback: sector_callback, rot_x_degrees: 1));
            bfc.AddSection(new CylSection(solidity: SectionSolidity.Hollow, radius: 4, length: 0, sectors: 8, thickness: 1, edge_callback: edge_callback, sector_callback: sector_callback, rot_x_degrees: 1, tag: "h"));
            bfc.AddSection(new CylSection(solidity: SectionSolidity.Hollow, radius: 4, length: 3, sectors: 8, thickness: 1, edge_callback: edge_callback, sector_callback: sector_callback, rot_x_degrees: 1));
            bfc.AddSection(new CylSection(solidity: SectionSolidity.Hollow, radius: 2, length: 0.5f, sectors: 8, thickness: 1, edge_callback: edge_callback, sector_callback: sector_callback, rot_x_degrees: 1));

            Surface surf = bfc.ToSurface();
            surf = ccs.Subdivide(surf);
            surf = ccs.Subdivide(surf);
            // surf = ccs.Subdivide(surf);
            // surf = ccs.Subdivide(surf);

            GetNode<MeshInstance3D>("Surface").Mesh = surf.ToMesh(Surface.MeshMode.Surface);
            GetNode<MeshInstance3D>("Mesh").Mesh = surf.ToMesh(Surface.MeshMode.Edges, new MeshOptions{ /* Edges_Filter = x => x.Tag == "hd" */ });
            GetNode<MeshInstance3D>("Normals").Mesh = surf.ToMesh(Surface.MeshMode.Normals, new MeshOptions{ DrawNormalsLength = 0.2f, Normals_IncludeFace = true });
        }
    }
}
