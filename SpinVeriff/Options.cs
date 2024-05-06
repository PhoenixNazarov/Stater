using System;
using System.Collections.Generic;
using System.Xml.Serialization;


namespace SpinVeriff
{
    [Serializable]
    public class Options
    {
        public Options()
        {
            //ForkSource = ForkEventSource.Environment;
            ForkSource = ForkEventSource.OnlyStarter;
            CompilePanOptions = ECompilePanOptions.DBITSTATE;
            EventSource = EEventSource.Fair;
            TextBefore = TextAfter = "";
            OnlyFSM = false;
            OnlyScriptsMode = false;
            ModelName = SpinPlugin.DefaultModelFile;
            AtomicTransition = true;
        }

        //public string FormulaLTL { get; set; }
        public List<string> FormulaeLTL { get; set; }
        public bool VerifySystem { get; set; }
        public bool VerifyEnteredObjects { get; set; }

        public class ObjectName
        {
            public string Name;
            public string Type;
        }

        public enum ForkEventSource
        {
            OnlyStarter,
            Environment,
        }

        public enum ECompilePanOptions
        {
            Common,
            DCOLLAPSE,
            DCOLLAPSE_DMA144_DSC,
            DMA64,
            DHC,
            DBITSTATE,
        }

        public enum EEventSource
        {
            Random, //Sends random events.
            Fair,   //Sends only events could be processed in this moment.
            None,   //Generate no event source.
        }

        public string TextBefore { get; set; }
        public string TextAfter { get; set; }

        public bool OnlyFSM { get; set; }
        public bool OnlyScriptsMode { get; set; }

        public string ModelName { get; set; }

        public ECompilePanOptions CompilePanOptions { get; set; }

        public ForkEventSource ForkSource { get; set; }

        public EEventSource EventSource { get; set; }

        public bool AtomicTransition { get; set; }

        [XmlArray("EnteredObjects")]
        public List<ObjectName> EnteredObjects { get; set; }

        public bool FillLonelyAutomata { get; set; }
        
        public bool VerifyOnlyOneMachine { get; set; }
        //public StateMachine WhichMachine { get; set; }
        public string WhichMachine { get; set; }

        public void SetOnlyOneMache(bool val)
        {
            if (val)
            {
                VerifySystem = false;
                VerifyEnteredObjects = false;
                FillLonelyAutomata = false;
            }
            VerifyOnlyOneMachine = val;
        }

        public void SetVerifySystem(bool val)
        {
            if (val)
            {
                VerifyOnlyOneMachine = false;
            }
            VerifySystem = val;
        }

        public void SetEnteredObjects(bool val)
        {
            if (val)
            {
                VerifyOnlyOneMachine = false;
            }
            VerifyEnteredObjects = val;
        }

        public void SetFillLonelyAutomatons(bool val)
        {
            if (val)
            {
                VerifyOnlyOneMachine = false;
            }
            FillLonelyAutomata = val;
        }

        //TODO: сделать опцию "Верифицируем один отдельный автомат", которая не совместима с остальными.
    }
}
