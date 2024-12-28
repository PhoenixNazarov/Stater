using System;
using System.Collections.Generic;
using Avalonia;
using Avalonia.Media;
using Microsoft.Msagl.Core;
using Microsoft.Msagl.Core.Geometry.Curves;
using Microsoft.Msagl.Core.Layout;
using Microsoft.Msagl.Core.Routing;
using Microsoft.Msagl.Drawing;
using Microsoft.Msagl.Layout.Incremental;
using Microsoft.Msagl.Layout.LargeGraphLayout;
using Microsoft.Msagl.Layout.Layered;
using Microsoft.Msagl.Layout.MDS;
using Microsoft.Msagl.Miscellaneous;
using Microsoft.Msagl.Prototype.Ranking;
using Microsoft.Msagl.Routing;
using Stater.Models;
using Stater.Views.Editors;
using Edge = Microsoft.Msagl.Core.Layout.Edge;
using LineSegment = Microsoft.Msagl.Core.Geometry.Curves.LineSegment;
using Node = Microsoft.Msagl.Core.Layout.Node;
using Point = Microsoft.Msagl.Core.Geometry.Point;

namespace Stater.Utils;

public static class GoodGraphView
{
    public class HelperStateClass(Transition t)
    {
        public Transition Transition = t;
    };

    public class HelperTransitionClass(Transition t, bool first)
    {
        public Transition Transition = t;
        public bool IsFirst = first;
    };

    public static StateMachine? ReBuildGraph(StateMachine? stateMachine)
    {
        if (stateMachine == null) return stateMachine;
        var graph = new GeometryGraph();
        foreach (var state in stateMachine.States)
        {
            var nd = new Node(
                CurveFactory.CreateRectangle(
                    state.Width,
                    state.Height,
                    new Point(0, 0)),
                state);
            graph.Nodes.Add(nd);
        }

        foreach (var transition in stateMachine.Transitions)
        {
            var startNode = stateMachine.States.Find(el => el.Guid == transition.Start);
            var endNode = stateMachine.States.Find(el => el.Guid == transition.End);
            if (startNode == null || endNode == null) continue;
            // var helpState = new HelperStateClass(transition);
            // var helpTr1 = new HelperTransitionClass(transition, true);
            // var helpTr2 = new HelperTransitionClass(transition, false);
            // var nd = new Node(
            //     CurveFactory.CreateRectangle(
            //         transition.Name.Length,
            //         1,
            //         new Point(0, 0)),
            //     helpState);
            // graph.Nodes.Add(nd);
            graph.Edges.Add(
                new Edge(
                    graph.FindNodeByUserData(startNode),
                    graph.FindNodeByUserData(endNode),
                    transition.Name.Length,
                    2.0,
                    1.0)
                {
                    Weight = 1,
                    UserData = transition
                });
            // graph.Edges.Add(
            //     new Edge(
            //         graph.FindNodeByUserData(startNode),
            //         graph.FindNodeByUserData(helpState),
            //         transition.Name.Length,
            //         1.0,
            //         1
            //     )
            //     {
            //         Weight = 1,
            //         UserData = helpTr1
            //     });
            // graph.Edges.Add(
            //     new Edge(
            //         graph.FindNodeByUserData(helpState),
            //         graph.FindNodeByUserData(endNode)
            //     )
            //     {
            //         Weight = 1,
            //         UserData = helpTr2
            //     });
        }

        var mode = EdgeRoutingMode.Spline;
        var routSet = new EdgeRoutingSettings
        {
            BundlingSettings = new BundlingSettings(),
            EdgeRoutingMode = mode
        };
        var laySet = new RankingLayoutSettings() { EdgeRoutingSettings = routSet };
        LayoutHelpers.CalculateLayout(graph, laySet, null);

        graph.UpdateBoundingBox();
        graph.Translate(new Point(-graph.Left, -graph.Bottom));

        List<State> newStates = [];
        List<Transition> newTransitions = [];
        // var namePoints = new Dictionary<Guid, Avalonia.Point>();
        // var firstHalfPoints = new Dictionary<Guid, List<Avalonia.Point>>();
        // var secondHalfPoints = new Dictionary<Guid, List<Avalonia.Point>>();
        foreach (var node in graph.Nodes)
        {
            if (node.UserData is State prevState)
            {
                var newState = prevState with
                {
                    CenterPoint = PointToPoint.ToAvaloniaPoint(node.BoundingBox.Center)
                };
                newStates.Add(newState);
            }
            // else if (node.UserData is HelperStateClass helpClass)
            // {
            //     namePoints[helpClass.Transition.Guid] = PointToPoint.ToAvaloniaPoint(node.BoundingBox.Center);
            // }
        }

        foreach (var edge in graph.Edges)
        {
            var newLinePoints = new List<Avalonia.Point>();
            DownloadPoints(newLinePoints, edge);
            // if (edge.UserData is HelperTransitionClass helperTransitionClass)
            // {
            //     if (helperTransitionClass.IsFirst)
            //     {
            //         firstHalfPoints.Add(helperTransitionClass.Transition.Guid, newLinePoints);
            //     }
            //     else
            //     {
            //         secondHalfPoints.Add(helperTransitionClass.Transition.Guid, newLinePoints);
            //     }
            // }
            if (edge.UserData is Transition transition)
            {
                var newTransition = transition with
                {
                    LinePoints = newLinePoints
                };
                newTransitions.Add(newTransition);
            }
        }

        // foreach (var key in namePoints.Keys)
        // {
        //     var namePoint = namePoints[key];
        //     var firstHalfPoint = firstHalfPoints[key];
        //     var secondHalfPoint = secondHalfPoints[key];
        //     var linePoints = UnionPoints(firstHalfPoint, namePoint, secondHalfPoint);
        //     var transition = stateMachine.Transitions.Find(el => el.Guid == key)!;
        //     var newTransition = transition with
        //     {
        //         LinePoints = linePoints,
        //         NamePoint = namePoint
        //     };
        //     newTransitions.Add(newTransition);
        // }

        var newStateMachine = stateMachine with
        {
            States = newStates,
            Transitions = newTransitions,
        };
        return newStateMachine;
    }

    private static List<Avalonia.Point> UnionPoints(List<Avalonia.Point> first, Avalonia.Point namePoint,
        List<Avalonia.Point> second)
    {
        first.Add(namePoint);
        first.AddRange(second);

        return first;
    }

    private static void DownloadPoints(List<Avalonia.Point> linePoints, Edge edge)
    {
        if (edge.Curve is LineSegment lineSegment)
        {
            linePoints.Add(PointToPoint.ToAvaloniaPoint(lineSegment.Start));
            linePoints.Add(PointToPoint.ToAvaloniaPoint(lineSegment.End));
        }
        else if (edge.Curve is Curve curve)
        {
            foreach (var segment in curve.Segments)
            {
                // When curve contains a line segment
                if (segment is LineSegment line)
                {
                    if (linePoints.Count == 0)
                        linePoints.Add(PointToPoint.ToAvaloniaPoint(line.Start));
                    linePoints.Add(PointToPoint.ToAvaloniaPoint(line.End));
                }
                // When curve contains a cubic bezier segment
                else if (segment is CubicBezierSegment bezier)
                {
                    linePoints.Add(
                        PointToPoint.ToAvaloniaPoint(new Point(bezier.B(0).X, bezier.B(0).Y)));
                    linePoints.Add(
                        PointToPoint.ToAvaloniaPoint(new Point(bezier.B(1).X, bezier.B(1).Y)));
                    linePoints.Add(
                        PointToPoint.ToAvaloniaPoint(new Point(bezier.B(2).X, bezier.B(2).Y)));
                    linePoints.Add(
                        PointToPoint.ToAvaloniaPoint(new Point(bezier.B(3).X, bezier.B(3).Y)));
                }

                // When curve contains an arc
                else if (segment is Ellipse ellipse)
                {
                    var interval = (ellipse.ParEnd - ellipse.ParStart) / 5.0;
                    for (var i = ellipse.ParStart;
                         i < ellipse.ParEnd;
                         i += interval)
                    {
                        var p = ellipse.Center
                                + (Math.Cos(i) * ellipse.AxisA)
                                + (Math.Sin(i) * ellipse.AxisB);
                        linePoints.Add(PointToPoint.ToAvaloniaPoint(new Point(p.X, p.Y)));
                    }
                }
                else
                {
                }
            }
        }
    }
}