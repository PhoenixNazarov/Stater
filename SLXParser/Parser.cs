using System;
using System.IO;
using System.Xml;
using SLXParser.Data;
using SLXParser.Utils;

namespace SLXParser
{
    public class Parser
    {
        private readonly string inFile;
        private readonly string zipPath;

        public Parser(string path)
        {
            if (!File.Exists(path))
            {
                throw new InvalidOperationException("Файл не найден");
            }

            inFile = path;
            var parentDir = Directory.GetParent(inFile);

            if (parentDir == null)
            {
                throw new InvalidOperationException("Не удалось выбрать папку для распаковки");
            }

            zipPath = Path.Combine(parentDir.ToString(), "slx_parser.temp");
        }

        public Stateflow Parse()
        {
            System.IO.Compression.ZipFile.ExtractToDirectory(inFile, zipPath);

            var simulinkPath = Path.Combine(zipPath, "simulink", "stateflow.xml");

            if (!File.Exists(simulinkPath))
            {
                Directory.Delete(zipPath, true);
                throw new InvalidOperationException("Не удалось найти файл от stateflow");
            }

            try
            {
                return ParseFile(simulinkPath);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            finally
            {
                Directory.Delete(zipPath, true);
            }
        }

        private Stateflow ParseFile(string file)
        {
            var xDoc = new XmlDocument();
            xDoc.Load(file);
            var xRoot = xDoc.DocumentElement;
            if (xRoot == null)
            {
                throw new InvalidOperationException("Проблема парсинга stateflow xml");
            }

            if (xRoot.Name != "Stateflow")
            {
                throw new InvalidOperationException("Главный элемент не является Stateflow");
            }

            return ParseStateflow(xRoot);
        }

        private Stateflow ParseStateflow(XmlNode xmlNode)
        {
            var stateflow = new Stateflow();

            foreach (XmlNode node in xmlNode.ChildNodes)
            {
                if (node.Name == "machine")
                {
                    stateflow.Machine = ParseMachine(node);
                }
            }

            return stateflow;
        }

        private Machine ParseMachine(XmlNode xmlNode)
        {
            var machine = new Machine
            {
                Id = ParseId(xmlNode)
            };

            foreach (XmlNode node in xmlNode.ChildNodes)
            {
                switch (node.Name)
                {
                    case "P":
                    {
                        var name = node.Attributes?["Name"].Value;
                        switch (name)
                        {
                            case null:
                                continue;
                            case "created":
                                machine.Created = node.Value;
                                break;
                        }

                        break;
                    }
                    case "Children":
                        foreach (XmlNode childrenNode in xmlNode.ChildNodes)
                        {
                            if (childrenNode.Name == "chart")
                            {
                                machine.Chart = ParseChart(childrenNode);
                            }
                        }

                        break;
                }
            }

            return machine;
        }

        private Chart ParseChart(XmlNode xmlNode)
        {
            var chart = new Chart
            {
                Id = ParseId(xmlNode)
            };

            foreach (XmlNode node in xmlNode.ChildNodes)
            {
                switch (node.Name)
                {
                    case "P":
                    {
                        var name = node.Attributes?["Name"].Value;
                        switch (name)
                        {
                            case null:
                                continue;
                            case "name":
                                chart.Name = node.Value;
                                break;
                            case "windowPosition":
                                chart.WindowPosition = Parse4Point(node.Value);
                                break;
                            case "viewLimits":
                                chart.ViewLimits = Parse4Point(node.Value);
                                break;
                            case "zoomFactor":
                                chart.ZoomFactor = float.Parse(node.Value);
                                break;
                            case "stateColor":
                                chart.StateColor = ParseColor(node.Value);
                                break;
                            case "stateLabelColor":
                                chart.StateLabelColor = ParseColor(node.Value);
                                break;
                            case "transitionColor":
                                chart.TransitionColor = ParseColor(node.Value);
                                break;
                            case "transitionLabelColor":
                                chart.TransitionLabelColor = ParseColor(node.Value);
                                break;
                            case "junctionColor":
                                chart.JunctionColor = ParseColor(node.Value);
                                break;
                            case "chartColor":
                                chart.ChartColor = ParseColor(node.Value);
                                break;
                            case "viewObj":
                                chart.ViewObj = int.Parse(node.Value);
                                break;
                            case "visible":
                                chart.Visible = bool.Parse(node.Value);
                                break;
                        }

                        break;
                    }
                    case "Children":
                        foreach (XmlNode childrenNode in xmlNode.ChildNodes)
                        {
                            switch (childrenNode.Name)
                            {
                                case "state":
                                    Chart.ChildrenState.Add(ParseState(childrenNode));
                                    break;
                                case "data":
                                    Chart.ChildrenData.Add(ParseData(childrenNode));
                                    break;
                            }
                        }

                        break;
                }
            }

            chart.WindowPosition = (new Point2D(2, 2), new Point2D(1, 2));

            return chart;
        }

        private State ParseState(XmlNode xmlNode)
        {
            var state = new State
            {
                Id = ParseId(xmlNode)
            };

            foreach (XmlNode node in xmlNode.ChildNodes)
            {
                switch (node.Name)
                {
                    case "P":
                    {
                        var name = node.Attributes?["Name"].Value;
                        switch (name)
                        {
                            case null:
                                continue;
                            case "labelString":
                                state.LabelString = node.Value;
                                break;
                            case "position":
                                state.Position = Parse4Point(node.Value);
                                break;
                            case "fontSize":
                                state.FontSize = int.Parse(node.Value);
                                break;
                            case "visible":
                                state.Visible = bool.Parse(node.Value);
                                break;
                            case "subviewer":
                                state.Subviewer = int.Parse(node.Value);
                                break;
                            case "type":
                                state.Type = node.Value;
                                break;
                            case "decomposition":
                                state.Decomposition = node.Value;
                                break;
                            case "executionOrder":
                                state.ExecutionOrder = int.Parse(node.Value);
                                break;
                        }

                        break;
                    }
                    case "activeStateOutput":
                    {
                        foreach (XmlNode childrenNode in xmlNode.ChildNodes)
                        {
                            if (childrenNode.Name == "P")
                            {
                                var name = childrenNode.Attributes?["Name"].Value;
                                switch (name)
                                {
                                    case null:
                                        continue;
                                    case "useCustomName":
                                        State.ActiveStateOutput.UseCustomName = bool.Parse(node.Value);
                                        break;
                                    case "customName":
                                        State.ActiveStateOutput.CustomName = node.Value;
                                        break;
                                    case "useCustomEnumTypeName":
                                        State.ActiveStateOutput.UseCustomEnumTypeName = bool.Parse(node.Value);
                                        break;
                                    case "enumTypeName":
                                        State.ActiveStateOutput.EnumTypeName = node.Value;
                                        break;
                                }
                            }
                        }

                        break;
                    }
                    case "Children":
                        foreach (XmlNode childrenNode in xmlNode.ChildNodes)
                        {
                            switch (childrenNode.Name)
                            {
                                case "state":
                                    state.ChildrenState.Add(ParseState(childrenNode));
                                    break;
                                case "transition":
                                    state.ChildrenTransition.Add(ParseTransition(childrenNode));
                                    break;
                            }
                        }

                        break;
                }
            }

            return state;
        }


        private Transition ParseTransition(XmlNode xmlNode)
        {
            var transition = new Transition
            {
                Id = ParseId(xmlNode)
            };

            foreach (XmlNode node in xmlNode.ChildNodes)
            {
                switch (node.Name)
                {
                    case "P":
                    {
                        var name = node.Attributes?["Name"].Value;
                        switch (name)
                        {
                            case null:
                                continue;
                            case "labelString":
                                transition.LabelString = node.Value;
                                break;
                            case "labelPosition":
                                transition.LabelPosition = Parse4Point(node.Value);
                                break;
                            case "fontSize":
                                transition.FontSize = int.Parse(node.Value);
                                break;
                            case "midPoint":
                                transition.MidPoint = ParsePoint(node.Value);
                                break;
                            case "dataLimits":
                                transition.DataLimits = Parse4Point(node.Value);
                                break;
                            case "subviewer":
                                transition.Subviewer = int.Parse(node.Value);
                                break;
                            case "drawStyle":
                                transition.DrawStyle = node.Value;
                                break;
                            case "executionOrder":
                                transition.ExecutionOrder = int.Parse(node.Value);
                                break;
                        }

                        break;
                    }
                    case "src":
                        transition.Src = ParseAddress(node);
                        break;
                    case "dst":
                        transition.Dst = ParseAddress(node);
                        break;
                }
            }

            return transition;
        }

        private static Address ParseAddress(XmlNode xmlNode)
        {
            var address = new Address();
            foreach (XmlNode node in xmlNode.ChildNodes)
            {
                switch (node.Name)
                {
                    case "P":
                    {
                        var name = node.Attributes?["Name"].Value;
                        switch (name)
                        {
                            case null:
                                continue;
                            case "SSID":
                                address.SSID = int.Parse(node.Value);
                                break;
                            case "intersection":
                                address.Intersection = Parse8Point(node.Value);
                                break;
                        }

                        break;
                    }
                }
            }

            return address;
        }


        private static Data.Data ParseData(XmlNode xmlNode)
        {
            var data = new Data.Data
            {
                Id = ParseId(xmlNode),
                Name = ParseName(xmlNode)
            };

            foreach (XmlNode node in xmlNode.ChildNodes)
            {
                switch (node.Name)
                {
                    case "P":
                    {
                        var name = node.Attributes?["Name"].Value;
                        switch (name)
                        {
                            case null:
                                continue;
                            case "scope":
                                data.Scope = node.Value;
                                break;
                            case "dataType":
                                data.DataType = node.Value;
                                break;
                        }

                        break;
                    }
                    case "props":
                        foreach (XmlNode childrenNode in xmlNode.ChildNodes)
                        {
                            switch (childrenNode.Name)
                            {
                                case "P":
                                {
                                    var name = childrenNode.Attributes?["Name"].Value;
                                    switch (name)
                                    {
                                        case null:
                                            continue;
                                        case "frame":
                                            data.Props.Frame = childrenNode.Value;
                                            break;
                                    }

                                    break;
                                }
                                case "type":
                                {
                                    foreach (XmlNode childrenNode2 in childrenNode.ChildNodes)
                                    {
                                        if (childrenNode2.Name == "P")
                                        {
                                            var name = childrenNode2.Attributes?["Name"].Value;
                                            switch (name)
                                            {
                                                case null:
                                                    continue;
                                                case "method":
                                                    data.Props.TypeMethod = childrenNode2.Value;
                                                    break;
                                                case "primitive":
                                                    data.Props.TypePrimitive = childrenNode2.Value;
                                                    break;
                                                case "wordLength":
                                                    data.Props.TypeWordLength = int.Parse(childrenNode2.Value);
                                                    break;
                                            }

                                            break;
                                        }

                                        switch (childrenNode2.Name)
                                        {
                                            case "fixpt":
                                            {
                                                foreach (XmlNode childrenNode3 in childrenNode.ChildNodes)
                                                {
                                                    if (childrenNode3.Name == "P")
                                                    {
                                                        var name = childrenNode3.Attributes?["Name"].Value;
                                                        switch (name)
                                                        {
                                                            case null:
                                                                continue;
                                                            case "scalingMode":
                                                                data.Props.TypeFixptScalingMode = childrenNode3.Value;
                                                                break;
                                                            case "fractionLength":
                                                                data.Props.TypeFixptFractionLength =
                                                                    int.Parse(childrenNode3.Value);
                                                                break;
                                                            case "slope":
                                                                data.Props.TypeFixptSlope = childrenNode3.Value;
                                                                break;
                                                            case "bias":
                                                                data.Props.TypeFixptBias =
                                                                    int.Parse(childrenNode3.Value);
                                                                break;
                                                        }
                                                    }
                                                }

                                                break;
                                            }
                                        }
                                    }

                                    break;
                                }
                                case "unit":
                                {
                                    foreach (XmlNode childrenNode2 in childrenNode.ChildNodes)
                                    {
                                        if (childrenNode2.Name == "P")
                                        {
                                            var name = childrenNode.Attributes?["Name"].Value;
                                            switch (name)
                                            {
                                                case null:
                                                    continue;
                                                case "frame":
                                                    data.Props.UnitName = childrenNode.Value;
                                                    break;
                                            }
                                        }
                                    }

                                    break;
                                }
                            }
                        }

                        break;
                }
            }

            return data;
        }

        private static int ParseId(XmlNode xmlNode)
        {
            var id = xmlNode.Attributes?["id"].Value;
            var ssid = xmlNode.Attributes?["SSID"].Value;
            if (id != null) return int.Parse(id);
            if (ssid != null) return int.Parse(ssid);
            return -1;
        }


        private static string ParseName(XmlNode xmlNode)
        {
            var name = xmlNode.Attributes?["name"].Value;
            return name ?? "";
        }

        private static Point2D ParsePoint(string line)
        {
            line = line.Substring(1, line.Length - 1);
            var numbers = line.Split(' ');

            var x1 = float.Parse(numbers[0]);
            var y1 = float.Parse(numbers[1]);

            return new Point2D(x1, y1);
        }

        private static (Point2D, Point2D) Parse4Point(string line)
        {
            line = line.Substring(1, line.Length - 1);
            var numbers = line.Split(' ');

            var x1 = float.Parse(numbers[0]);
            var y1 = float.Parse(numbers[1]);
            var x2 = float.Parse(numbers[2]);
            var y2 = float.Parse(numbers[3]);

            return (new Point2D(x1, y1), new Point2D(x2, y2));
        }

        private static (Point2D, Point2D, Point2D, Point2D) Parse8Point(string line)
        {
            line = line.Substring(1, line.Length - 1);
            var numbers = line.Split(' ');

            var x1 = float.Parse(numbers[0]);
            var y1 = float.Parse(numbers[1]);
            var x2 = float.Parse(numbers[2]);
            var y2 = float.Parse(numbers[3]);
            var x3 = float.Parse(numbers[2]);
            var y3 = float.Parse(numbers[3]);
            var x4 = float.Parse(numbers[2]);
            var y4 = float.Parse(numbers[3]);

            return (new Point2D(x1, y1), new Point2D(x2, y2), new Point2D(x3, y3), new Point2D(x4, y4));
        }

        private static Color ParseColor(string line)
        {
            line = line.Substring(1, line.Length - 1);
            var numbers = line.Split(' ');

            var r = (int)(double.Parse(numbers[0]) * 256);
            var g = (int)(double.Parse(numbers[1]) * 256);
            var b = (int)(double.Parse(numbers[2]) * 256);

            return new Color(r, g, b);
        }
    }
}