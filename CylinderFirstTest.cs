using System;
using Godot;
using SubD;
using SubD.CylTypes;
using SubD.Builders;

[Tool]
public partial class CylinderFirstTest : Node3D
{
    bool Clean = false;

    public override void _Process(double delta)
    {
        if (!Clean)
        {
            Clean = true;

            BuildFromCylinders bfc = new();
            CatmullClarkSubdivider ccs = new();

            // bfc.AddSection(new CylSection(4, 1, 6, SectionSolidity.Hollow, 2));
            // bfc.AddSection(new CylSection(4, 0.1f, 6, SectionSolidity.Hollow, 2));
            // bfc.AddSection(new CylSection(3.75f, 0, 6, SectionSolidity.Hollow, 1.5f));
            // bfc.AddSection(new CylSection(4, 0.1f, 6, SectionSolidity.Hollow, 2));
            // bfc.AddSection(new CylSection(4, 1, 6, SectionSolidity.Hollow, 2));
            // bfc.AddSection(new CylSection(3, 1, 6));
            // bfc.AddSection(new CylSection(3, 1, 6));
            // bfc.AddSection(new CylSection(3, 1, 6, SectionSolidity.Hollow));
            // bfc.AddSection(new CylSection(3, 1, 6, SectionSolidity.Hollow));
            // bfc.AddSection(new CylSection(4, 0, 6, SectionSolidity.Hollow));
            // bfc.AddSection(new CylSection(4, 1, 6, SectionSolidity.Hollow));
            // bfc.AddSection(new CylSection(3, 0, 6, SectionSolidity.Hollow));
            // bfc.AddSection(new CylSection(3, 1, 6, SectionSolidity.Hollow));

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

            Func<CylSection, int, Topology, VertProps> vert_callback = (c, t, i) => new VertProps();
            // Func<CylSection, int, Topology, BuildFromCylinders.EdgeType, bool> edge_callback
            //     = (c, i, t, et) => t == BuildFromCylinders.Topology.Outside
            //                     && ((et == BuildFromCylinders.EdgeType.Circumferential && (i & 1) == 0)
            //                         || (et == BuildFromCylinders.EdgeType.Coaxial));
            Func<CylSection, int, Topology, EdgeType, EdgeProps> edge_callback =
                (s, i, t, et) =>
                {
                    int where = bfc.Sections.IndexOf(s);
                    string tag = "";
                    bool sharp = false;

                    switch(et)
                    {
                        case EdgeType.Circumferential:
                            tag = "c";
                            break;
                        case EdgeType.Axial:
                            tag = "a";
                            if (i % 2 == 1 || i == 6)
                            {
                                sharp = true;
                            }
                            break;
                        case EdgeType.Hole:
                            tag = "h";
                            break;
                        case EdgeType.HoleEdge:
                            tag = "he";
                            if (where != 1)
                            {
                                sharp = true;
                            }
                            break;
                        case EdgeType.HoleDiagonal:
                            tag = "hd";
                            break;
                    }

                    return new EdgeProps(sharp, tag);
                };

            // Func<CylSection, int, Topology, FaceProps> face_callback = (s, i, t) => new FaceProps(i == -1 ? "x" : null);

            ;
            Func<CylSection, int, SectorProps> sector_callback = (s, i) =>
            {
                HoleProps? hole_props = null;
                int where = bfc.Sections.IndexOf(s);
                if (where == 1 && i == 0)
                {
                    hole_props = new HoleProps(clearance: 1.5f);
                }
                if ((where == 2 || where == 3) && i == 5)
                {
                    hole_props = new HoleProps(clearance: 0.5f);
                }
                if (where == 2 && i == 4)
                {
                    hole_props = new HoleProps(clearance: 0.5f);
                }
                if (where == 4 && i >= 4 && i <= 6)
                {
                    hole_props = new HoleProps(clearance: 0.5f);
                }

                return new SectorProps(hole_props);
            };

            bfc.AddSection(new CylSection(solidity: SectionSolidity.Hollow, radius: 5, length: 4.5f, sectors: 7, thickness: 1, sector_callback: sector_callback, edge_callback: edge_callback, rot_z_degrees: -10));
            bfc.AddSection(new CylSection(solidity: SectionSolidity.Hollow, radius: 6, length: 4.5f, sectors: 7, thickness: 1, sector_callback: sector_callback, edge_callback: edge_callback, rot_z_degrees: -10));
            bfc.AddSection(new CylSection(solidity: SectionSolidity.Hollow, radius: 5, length: 4.5f, sectors: 7, thickness: 1, sector_callback: sector_callback, edge_callback: edge_callback, rot_z_degrees: -10));
            bfc.AddSection(new CylSection(solidity: SectionSolidity.Hollow, radius: 4, length: 4.5f, sectors: 7, thickness: 1, sector_callback: sector_callback, edge_callback: edge_callback, rot_z_degrees: -10));
            bfc.AddSection(new CylSection(solidity: SectionSolidity.Hollow, radius: 3, length: 4.5f, sectors: 7, thickness: 1, sector_callback: sector_callback, edge_callback: edge_callback, rot_z_degrees: 10));
            bfc.AddSection(new CylSection(solidity: SectionSolidity.Hollow, radius: 2, length: 2f, sectors: 7, thickness: 1, sector_callback: sector_callback, edge_callback: edge_callback, rot_z_degrees: 10));
            bfc.AddSection(new CylSection(solidity: SectionSolidity.Hollow, radius: 2, length: 2f, sectors: 7, thickness: 1, sector_callback: sector_callback, edge_callback: edge_callback, rot_z_degrees: 10));
            bfc.AddSection(new CylSection(solidity: SectionSolidity.Hollow, radius: 2, length: 2f, sectors: 7, thickness: 1, sector_callback: sector_callback, edge_callback: edge_callback, rot_z_degrees: 10));
            // bfc.AddSection(new CylSection(solidity: SectionSolidity.Hollow, radius: 12, length: 10, sectors: 5, sector_callback: sector_callback, rot_x_degrees: 15));

            // bfc.AddSection(new CylSection(solidity: SectionSolidity.Hollow, radius: 15, length: 10, sectors: 5, sector_callback: sector_callback, rot_y_degrees: 15, rot_z_degrees: 20));
            // bfc.AddSection(new CylSection(solidity: SectionSolidity.Hollow, radius: 15, length: 10, sectors: 5, sector_callback: sector_callback, rot_y_degrees: 15, rot_z_degrees: 20));
            // bfc.AddSection(new CylSection(solidity: SectionSolidity.Hollow, radius: 15, length: 10, sectors: 5, sector_callback: sector_callback, rot_y_degrees: 15, rot_z_degrees: 20));
            // bfc.AddSection(new CylSection(solidity: SectionSolidity.Hollow, radius: 15, length: 10, sectors: 5, sector_callback: sector_callback, rot_y_degrees: 15, rot_z_degrees: 20));

            // bfc.AddSection(new CylSection(solidity: SectionSolidity.Hollow, radius: 10, length: 10, sectors: 5, sector_callback: sector_callback, rot_x_degrees: -10, rot_z_degrees: -20));
            // bfc.AddSection(new CylSection(solidity: SectionSolidity.Hollow, radius: 5, length: 10, sectors: 5, sector_callback: sector_callback, rot_x_degrees: -10, rot_z_degrees: -20));
            // bfc.AddSection(new CylSection(solidity: SectionSolidity.Hollow, radius: 1, length: 10, sectors: 5, sector_callback: sector_callback, rot_x_degrees: -10, rot_z_degrees: -20));

            // bfc.AddSection(new CylSection(solidity: SectionSolidity.Hollow, radius: 5, length: 10, sectors: 4, sector_callback: sector_callback));
            // bfc.AddSection(new CylSection(solidity: SectionSolidity.Hollow, radius: 5, length: 10, sectors: 4, sector_callback: sector_callback));
            // bfc.AddSection(new CylSection(solidity: SectionSolidity.Hollow, radius: 5, length: 10, sectors: 4, sector_callback: sector_callback));
            // bfc.AddSection(new CylSection(solidity: SectionSolidity.Hollow, radius: 5, length: 10, sectors: 4, sector_callback: sector_callback));
            // bfc.AddSection(new CylSection(solidity: SectionSolidity.Hollow, radius: 5, length: 10, sectors: 4, sector_callback: sector_callback));
            // bfc.AddSection(new CylSection(solidity: SectionSolidity.Hollow, radius: 5, length: 10, sectors: 4, sector_callback: sector_callback));
            // bfc.AddSection(new CylSection(solidity: SectionSolidity.Hollow, radius: 5, length: 10, sectors: 4, sector_callback: sector_callback));

            // bfc.AddSection(new CylSection(solidity: SectionSolidity.Solid, radius: 3, length: 0, face_callback: face_callback));
            // bfc.AddSection(new CylSection(solidity: SectionSolidity.Solid, radius: 3, length: 1, face_callback: face_callback));
            // bfc.AddSection(new CylSection(solidity: SectionSolidity.Solid, radius: 4, length: 0, face_callback: face_callback, vert_callback: vert_callback));
            // bfc.AddSection(new CylSection(solidity: SectionSolidity.Solid, radius: 3, length: 0, face_callback: face_callback));
            // bfc.AddSection(new CylSection(solidity: SectionSolidity.Solid, radius: 3, length: 1, face_callback: face_callback));
            // bfc.AddSection(new CylSection(solidity: SectionSolidity.Solid, radius: 4, length: 0, face_callback: face_callback, vert_callback: vert_callback));
            // bfc.AddSection(new CylSection(solidity: SectionSolidity.Solid, radius: 3, length: 0, face_callback: face_callback));
            // bfc.AddSection(new CylSection(solidity: SectionSolidity.Solid, radius: 3, length: 1, face_callback: face_callback));
            // bfc.AddSection(new CylSection(solidity: SectionSolidity.Solid, radius: 4, length: 0, face_callback: face_callback, vert_callback: vert_callback));
            // bfc.AddSection(new CylSection(solidity: SectionSolidity.Solid, radius: 3, length: 0, face_callback: face_callback));
            // bfc.AddSection(new CylSection(solidity: SectionSolidity.Solid, radius: 3, length: 1, face_callback: face_callback));
            // bfc.AddSection(new CylSection(solidity: SectionSolidity.Solid, radius: 4, length: 0, face_callback: face_callback, vert_callback: vert_callback));
            // bfc.AddSection(new CylSection(solidity: SectionSolidity.Solid, radius: 3, length: 0, face_callback: face_callback));
            // bfc.AddSection(new CylSection(solidity: SectionSolidity.Solid, radius: 3, length: 1, face_callback: face_callback));
            // bfc.AddSection(new CylSection(solidity: SectionSolidity.Solid, radius: 4, length: 0, face_callback: face_callback, vert_callback: vert_callback));
            // bfc.AddSection(new CylSection(solidity: SectionSolidity.Solid, radius: 3, length: 0, face_callback: face_callback));
            // bfc.AddSection(new CylSection(solidity: SectionSolidity.Solid, radius: 3, length: 1, face_callback: face_callback));

            // bfc.AddSection(new CylSection(solidity: SectionSolidity.Hollow, radius: 3, length: 0, vert_callback: vert_callback, edge_callback: edge_callback));
            // bfc.AddSection(new CylSection(solidity: SectionSolidity.Hollow, radius: 3, transform: transform, vert_callback: vert_callback, edge_callback: edge_callback));
            // bfc.AddSection(new CylSection(solidity: SectionSolidity.Hollow, radius: 3, transform: transform, vert_callback: vert_callback, edge_callback: edge_callback));
            // bfc.AddSection(new CylSection(solidity: SectionSolidity.Hollow, radius: 3, transform: transform, vert_callback: vert_callback, edge_callback: edge_callback));

            // bfc.AddSection(new CylSection(solidity: SectionSolidity.Hollow, radius: 3, length: 3));
            // bfc.AddSection(new CylSection(solidity: SectionSolidity.Hollow, radius: 3, transform: transform2));
            // bfc.AddSection(new CylSection(solidity: SectionSolidity.Hollow, radius: 3, transform: transform2));
            // bfc.AddSection(new CylSection(solidity: SectionSolidity.Hollow, radius: 3, transform: transform2));
            // bfc.AddSection(new CylSection(solidity: SectionSolidity.Hollow, radius: 3, length: 5));
            // bfc.AddSection(new CylSection(solidity: SectionSolidity.Hollow, radius: 3, transform: transform));
            // bfc.AddSection(new CylSection(solidity: SectionSolidity.Hollow, radius: 3, transform: transform));
            // bfc.AddSection(new CylSection(solidity: SectionSolidity.Hollow, radius: 3, transform: transform));

            // bfc.AddSection(new CylSection(3, 0, 6));
            // bfc.AddSection(new CylSection(3, 3, 6, offset_angle_degrees: 20));
            // bfc.AddSection(new CylSection(3, 3, 6, offset_angle_degrees: 20));
            // bfc.AddSection(new CylSection(3, 3, 6, offset_angle_degrees: 20));

            // bfc.AddSection(new CylSection(3, 0, 6));
            // bfc.AddSection(new CylSection(3, 0.1f, 6));
            // bfc.AddSection(new CylSection(4, 0, 6));
            // bfc.AddSection(new CylSection(4, 0.1f, 6));
            // bfc.AddSection(new CylSection(4, 3, 6, SectionSolidity.Hollow));
            // bfc.AddSection(new CylSection(4, 0.2f, 6, SectionSolidity.Hollow));
            // bfc.AddSection(new CylSection(4.5f, 0.2f, 6, SectionSolidity.Hollow, 1.5f));
            // bfc.AddSection(new CylSection(4, 0.2f, 6, SectionSolidity.Hollow));
            // bfc.AddSection(new CylSection(4.5f, 0.2f, 6, SectionSolidity.Hollow, 1.5f));
            // bfc.AddSection(new CylSection(4, 0.2f, 6, SectionSolidity.Hollow));
            // bfc.AddSection(new CylSection(4.5f, 0.2f, 6, SectionSolidity.Hollow, 1.5f));
            // bfc.AddSection(new CylSection(4, 0.2f, 6, SectionSolidity.Hollow));
            // bfc.AddSection(new CylSection(4.5f, 0.2f, 6, SectionSolidity.Hollow, 1.5f));
            // bfc.AddSection(new CylSection(4, 0.2f, 6, SectionSolidity.Hollow));
            // bfc.AddSection(new CylSection(4.5f, 0.2f, 6, SectionSolidity.Hollow, 1.5f));
            // bfc.AddSection(new CylSection(4, 0.2f, 6, SectionSolidity.Hollow));


            Surface surf = bfc.ToSurface();
            surf = ccs.Subdivide(surf);
            surf = ccs.Subdivide(surf);
            // surf = ccs.Subdivide(surf);
            // surf = ccs.Subdivide(surf);

            GetNode<MeshInstance3D>("Surface").Mesh = surf.ToMesh(Surface.MeshMode.Surface);
            GetNode<MeshInstance3D>("Mesh").Mesh = surf.ToMesh(Surface.MeshMode.Edges, new MeshOptions{ /* Edges_Filter = x => x.Tag == "he" */ });
            GetNode<MeshInstance3D>("Normals").Mesh = surf.ToMesh(Surface.MeshMode.Normals, new MeshOptions{ DrawNormalsLength = 0.2f, Normals_IncludeFace = true });
        }
    }
}
