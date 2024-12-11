using System;
using System.Collections.Generic;
using Avalonia;
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
using Node = Microsoft.Msagl.Core.Layout.Node;
using Point = Microsoft.Msagl.Core.Geometry.Point;

namespace Stater.Utils;

public static class GoodGraphView
{
    public static StateMachine? ReBuildGraph(StateMachine? stateMachine)
    {
        if(stateMachine == null) return stateMachine;
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
            graph.Edges.Add(
                new Edge(
                    graph.FindNodeByUserData(startNode),
                    graph.FindNodeByUserData(endNode))
                {
                    Weight = 1,
                    UserData = transition
                });
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
        List<Transition> newtransitions = [];
        
        foreach(var node in graph.Nodes)
        {
            var newState = (node.UserData as State) with
            {
                CenterPoint = PointToPoint.ToAvaloniaPoint(node.BoundingBox.Center)
            };
            newStates.Add(newState);
        }
        newtransitions.AddRange(stateMachine.Transitions);
        var newStateMashine = stateMachine with
        {
            States = newStates,
            Transitions = newtransitions,
        };
        return newStateMashine;
    }
}