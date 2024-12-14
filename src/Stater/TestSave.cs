using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using DynamicData;
using Stater.Models;
using Stater.Views.Editors;

namespace Stater;

public class TestSave
{
    public static void main()
    {
        var writer = new System.Xml.Serialization.XmlSerializer(typeof(ExportProject));

        Project project = new("Project", null);

        var stateMachine1 = new StateMachine();

        var state1 = new State();
        state1 = state1 with
        {
            Name = "A",
            Type = StateType.Start
        };

        stateMachine1.States.Add(state1);

        var exportProject = new ExportProject(
            project,
            new List<StateMachine> { stateMachine1 }
        );
        
        Console.WriteLine("YES1");
        var fileName = Path.GetFullPath("./statemachine.xml");
        Console.WriteLine(fileName);
        var sw = new StreamWriter(fileName);
        // Console.WriteLine(sw.);
        writer.Serialize(sw, exportProject);
        Console.WriteLine("YES3");
        sw.Close();
    }
}