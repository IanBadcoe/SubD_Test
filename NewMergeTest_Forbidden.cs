// #define PROFILE_ON

using Godot;
using SubD;
using SubD.Builders;

using Chickensoft.AutoInject;
using Chickensoft.Introspection;
using System.Collections.Generic;
using Godot_Util;
using System.Linq;
using Geom_Util;
using System.Diagnostics;
using System;

[Tool]
[Meta(typeof(IAutoConnect))]
public partial class NewMergeTest_Forbidden : Node3D
{
    // --------------------------------------------------------------
    // IAutoNode boilerplate
    public override void _Notification(int what) => this.Notify(what);
    // --------------------------------------------------------------

    [Node]
    MeshInstance3D MergedSurf { get; set; }

    [Node]
    MeshInstance3D MergedGrid { get; set; }

    [Node]
    Camera3D Camera { get; set; }

    [Node]
    DirectionalLight3D DirectionalLight { get; set; }

    readonly BuildFromCubes BFC = new();
    readonly CatmullClarkSubdivider CCS = new();

    int TestCaseIdx { get; set; } = 0;

    TestMode CurrentTestMode { get; set; } = TestMode.Basic;

    ulong Timing;
    bool Auto;

    enum TestMode
    {
        Basic,
        Dissect,
        RandomDissect
    }

    void TestCase(int seed, int width, int height, int thickness, TestMode mode, int[] subset = null)
    {
        ClRand Rand = new(seed);

        Vector3I[,,] centres = new Vector3I[width, height, thickness];
        HashSet<(Vector3I, Vector3I)> forbidden_pairs = [];

        Vector3I[] offsets = {
            new(-1,  0,  0),
            new( 1,  0,  0),
            new( 0, -1,  0),
            new( 0,  1,  0),
            new( 0,  0, -1),
            new( 0,  0,  1),
        };

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                for (int k = 0; k < thickness; k++)
                {
                    centres[i, j, k] = new Vector3I(i, j, k);
                }
            }
        }

        ImBounds bounds = new(new ImVec3(0, 0, 0), new ImVec3(width - 1, height - 1, thickness - 1));

        foreach(Vector3I here in centres)
        {
            foreach(Vector3I offset in offsets)
            {
                Vector3I where = here + offset;

                if (bounds.Contains(new ImVec3(where.X, where.Y, where.Z)))
                {
                    if (Rand.Float() < 0.15f)
                    {
                        forbidden_pairs.Add((here, where));
                    }
                }
            }
        }

        // if (subset != null)
        // {
        //     ReducedTestCase(centres, subset);
        // }

        // switch(mode)
        // {
        //     case TestMode.Basic:
                TestCase(centres, forbidden_pairs);
        //         break;

        //     case TestMode.Dissect:
        //         TestCaseDissect(centres);
        //         break;

        //     case TestMode.RandomDissect:
        //         TestCaseRandomDissect(centres, 1000);
        //         break;
        // }
    }

    // private void TestCaseRandomDissect(List<Vector3I> centres, int max_tries_per_size)
    // {
    //     int max_cubes = centres.Count;

    //     ClRand rand = new(73);

    //     for (int num_cubes = 1; num_cubes < max_cubes; num_cubes++)
    //     {
    //         float combinations = 1;

    //         for(int i = 0; i < num_cubes; i++)
    //         {
    //             combinations *= (max_cubes - i);
    //         }

    //         if (combinations < max_tries_per_size)
    //         {
    //             TestCaseDissectOneLength(centres, num_cubes);
    //         }
    //         else
    //         {
    //             for(int attempt = 0; attempt < max_tries_per_size; attempt++)
    //             {
    //                 int?[] state = new int?[num_cubes];

    //                 for(int i = 0; i < num_cubes; i++)
    //                 {
    //                     do
    //                     {
    //                         state[i] = rand.IntRange(max_cubes);
    //                     }
    //                     while(state.Where(x => x.HasValue && x.Value == state[i]).Count() > 1);
    //                 }

    //                 Debug.Print("[" + state.Aggregate("", (x, y) => x + (x != "" ? "," : "") + y.ToString()) + "]");

    //                 ReducedTestCase(centres, [.. state.Select(x => x.Value)]);
    //             }
    //         }
    //     }
    // }

    // private void TestCaseDissect(List<Vector3I> centres)
    // {
    //     int max_cube = centres.Count;

    //     for (int num_cubes = 1; num_cubes <= max_cube; num_cubes++)
    //     {
    //         TestCaseDissectOneLength(centres, num_cubes);
    //     }
    // }

    // private void TestCaseDissectOneLength(List<Vector3I> centres, int num_cubes)
    // {
    //     int max_cube = centres.Count;

    //     int[] state = new int[num_cubes];

    //     for (int i = 0; i < num_cubes; i++)
    //     {
    //         state[i] = i;
    //     }

    //     bool done = false;

    //     while (!done)
    //     {
    //         Debug.Print("[" + state.Aggregate("", (x, y) => x + (x != "" ? "," : "") + y.ToString()) + "]");

    //         ReducedTestCase(centres, state);

    //         do
    //         {
    //             for (int i = num_cubes - 1; i >= 0; i--)
    //             {
    //                 state[i]++;

    //                 if (state[i] < max_cube)
    //                 {
    //                     break;
    //                 }

    //                 // when the first (slowest) index clocks, we are done
    //                 if (i == 0)
    //                 {
    //                     done = true;
    //                     break;
    //                 }

    //                 state[i] = 0;
    //             }
    //         } while (!done && num_cubes > state.Distinct().Count());      // skip any states with duplicate cubes
    //     }
    // }

    // private void ReducedTestCase(List<Vector3I> centres, int[] state, HashSet<(Vector3I, Vector3I)> forbidden_merge_pairs)
    // {
    //     List<Vector3I> reduced_case = [];

    //     foreach (int i in state)
    //     {
    //         reduced_case.Add(centres[i]);
    //     }

    //     TestCase(reduced_case);
    // }

    private void TestCase(Vector3I[,,] centres, HashSet<(Vector3I, Vector3I)> forbidden_merge_pairs)
    {
        BFC.Reset();

        int width = centres.GetUpperBound(0) + 1;
        int height = centres.GetUpperBound(1) + 1;
        int thickness = centres.GetUpperBound(2) + 1;
        Cube[,,] cubes = new Cube[width, height, thickness];

        PoorMansProfiler.Start("Cubes");

        foreach (Vector3I centre in centres)
        {
            cubes[centre.X, centre.Y, centre.Z] = BFC.AddCube(centre, 0);
        }

        foreach((Vector3I i, Vector3I j) in forbidden_merge_pairs)
        {
            BFC.ForbidSpecificMerge(cubes[i.X, i.Y, i.Z], cubes[j.X, j.Y, j.Z]);
        }

        PoorMansProfiler.End("Cubes");

        PoorMansProfiler.Start("ToSurface - separate");
        Surface surf = BFC.ToSurface(false);
        PoorMansProfiler.End("ToSurface - separate");

        // surf = CCS.Subdivide(surf);
        // surf = CCS.Subdivide(surf);
        // surf = CCS.Subdivide(surf);

        PoorMansProfiler.Start("ToMesh - separate");
        MergedGrid.Mesh = surf.ToMesh(Surface.MeshMode.Edges);
        PoorMansProfiler.End("ToMesh - separate");
//        MergedSurf.Mesh = surf.ToMesh(Surface.MeshMode.Surface);

        PoorMansProfiler.Start("ToSurface - merged");
        surf = BFC.ToSurface(true);
        PoorMansProfiler.End("ToSurface - merged");

        PoorMansProfiler.Start("Subdivide");
        // surf = CCS.Subdivide(surf);
        surf = CCS.Subdivide(surf);
        surf = CCS.Subdivide(surf);
        PoorMansProfiler.End("Subdivide");

//        MergedGrid.Mesh = surf.ToMesh(Surface.MeshMode.Edges);
        PoorMansProfiler.Start("ToMesh - merged");
        MergedSurf.Mesh = surf.ToMesh(Surface.MeshMode.Surface);
        PoorMansProfiler.End("ToMesh - merged");

        ImBounds bounds = new(new(0, 0, 0), new(width - 1, height - 1, thickness - 1));

        bounds.ExpandedBy(1);

        Camera.Position = bounds.Centre.ToVector3() + new Vector3(1, 0.5f, 0.25f) * bounds.Size.Length();
        Camera.LookAt(bounds.Centre.ToVector3());

        DirectionalLight.LookAt(bounds.Centre.ToVector3());
    }

    public override void _Ready()
    {
        PoorMansProfiler.Reset();

        PoorMansProfiler.Start("Outer");

        NextTestCase();

        PoorMansProfiler.End("Outer");

        PoorMansProfiler.Dump("profile.txt");

//        SpecificDissectionOfNextTestCase([7,13,8,12,11,6]);
        // TestCase(1, 2, 2, 2);
        // TestCase(2, 2, 2, 2);
        // TestCase(3, 2, 2, 2);
        // TestCase(4, 2, 2, 2);
        // TestCase(5, 2, 2, 2);

        // TestCase(new List<Vector3I>{
        //     new Vector3I(0, 0, 0),
        //     new Vector3I(1, 1, 0),
        //     new Vector3I(0, 1, 0),
        //     new Vector3I(0, 0, 1),
        //     new Vector3I(1, 1, 1),
        // });


        // // BFC.AddCube(new Vector3I(0, 0, 0), 1);
        // // BFC.AddCube(new Vector3I(1, 0, 0), 1);
        // // Cube c1 = BFC.AddCube(new Vector3I(1, 1, 0), 1);
        // // Cube c2 = BFC.AddCube(new Vector3I(0, 1, 0), 1);

        // // //BFC.ForbidSpecificMerge(c1, c2);

        // // BFC.AddCube(new Vector3I(0, 0, 0), 1);
        // // BFC.AddCube(new Vector3I(1, 0, 0), 1);
        // // BFC.AddCube(new Vector3I(2, 0, 0), 1);
        // // BFC.AddCube(new Vector3I(2, 0, 1), 1);
        // // BFC.AddCube(new Vector3I(2, 0, 2), 1);
        // // BFC.AddCube(new Vector3I(1, 0, 2), 1);
        // // BFC.AddCube(new Vector3I(0, 0, 2), 1);
        // // BFC.AddCube(new Vector3I(0, 0, 1), 1);

        // // BFC.AddCube(new Vector3I(1, 0, 1), 1);

        // // BFC.AddCube(new Vector3I(1, 0, 1), 2);
        // // BFC.AddCube(new Vector3I(1, 1, 1), 2);
        // // BFC.AddCube(new Vector3I(2, 1, 1), 2);
        // // BFC.AddCube(new Vector3I(3, 1, 1), 2);
        // // BFC.AddCube(new Vector3I(3, 0, 1), 2);
        // // BFC.AddCube(new Vector3I(3, -1, 1), 2);
        // // BFC.AddCube(new Vector3I(2, -1, 1), 2);
        // // BFC.AddCube(new Vector3I(1, -1, 1), 2);

        // Surface surf = BFC.ToSurface();

        // surf = CCS.Subdivide(surf);
        // // surf = CCS.Subdivide(surf);
        // // surf = CCS.Subdivide(surf);

        // MergedSurf.Mesh = surf.ToMesh(Surface.MeshMode.Surface);
    }

    private void SpecificDissectionOfNextTestCase(int[] subset)
    {
        int size = (TestCaseIdx / 10) + 2;

        TestCase(TestCaseIdx % 10, size, size, size, CurrentTestMode, subset);
    }

    public override void _Process(double delta)
    {
        ulong now = Godot.Time.GetTicksMsec();

        if (!Engine.IsEditorHint())
        {
            if (Input.IsActionJustReleased("Space") || (Auto && Timing + 500 < now))
            {
                NextTestCase();

                Timing = now;
            }

            if (Input.IsActionJustReleased("Auto"))
            {
                Auto = !Auto;
            }
        }
    }

    private void NextTestCase()
    {
        int size = (TestCaseIdx / 10) + 2;

        TestCase(TestCaseIdx % 10, 1, size, size, CurrentTestMode);

        TestCaseIdx++;
    }
}
