using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpinVeriff;


namespace SpinVeriffTest
{
    [TestClass]
    public class LTLParserTest
    {
        /// <summary>
        /// State and event
        /// </summary>
        [TestMethod]
        public void TestPropParse1()
        {
            Generator generator = new Generator();
            generator.GenerateSimpleFSMSystem();
            Options options = generator.Options;
            options.FormulaeLTL.Add("<>{a1.e1}");
            var fsm = generator.FSMSystem;
            LTLConverter converter = new LTLConverter(options, fsm, null);

            var ltl = converter.ProcessLTL();
            Assert.AreEqual("#define p0 (a1.state == A1_s1)", converter.Propositions[0]);
            Assert.AreEqual("<>p0", ltl[0]);

            Assert.AreEqual("#define p1 (a1.curEvent == e1)", converter.Propositions[1]);
            Assert.AreEqual("<>p1", ltl[1]);
        }

        [TestMethod]
        public void TestPropFunction()
        {
            Generator generator = new Generator();
            generator.GenerateSimpleFSMSystem();
            Options options = generator.Options;
            options.FormulaeLTL.Add("[]{a1.func}");
            LTLConverter converter = new LTLConverter(options, generator.FSMSystem, generator.ForkList);
            var ltl = converter.ProcessLTL();

            Assert.AreEqual("#define p1 (a1.functionCall == A1_func)", converter.Propositions[1]);
            Assert.AreEqual("[]p1", ltl[1]);
        }

        [TestMethod]
        public void TestPropParseNested()
        {
            Generator generator = new Generator();
            generator.GenerateNestedSystem();
            generator.Options.FormulaeLTL.Add("<> {a1->a2}");

            LTLConverter converter = new LTLConverter(generator.Options, generator.FSMSystem, generator.ForkList);
            var ltl = converter.ProcessLTL();
            Assert.AreEqual("#define p0 (a1.nestedMachine == a2)", converter.Propositions[0]);
            Assert.AreEqual("<> p0", ltl[0]);
        }

        [TestMethod]
        public void TestPropParseParallel()
        {
            Generator generator = new Generator();
            generator.GenerateParallelSystem();
            generator.Options.FormulaeLTL.Add("<>{a1||server}");

            LTLConverter converter = new LTLConverter(generator.Options, generator.FSMSystem, generator.ForkList);
            var ltl = converter.ProcessLTL();
            Assert.AreEqual("#define p0 (a1.forkMachine == server)", converter.Propositions[0]);
        }

        [TestMethod]
        public void TestSingleVar()
        {
            Generator generator = new Generator();
            generator.GenerateFSMWithVariables();
            generator.Options.FormulaeLTL.Clear();
            generator.Options.FormulaeLTL.Add("[]{a1.b == 5}");
            LTLConverter converter = new LTLConverter(generator.Options, generator.FSMSystem, generator.ForkList);
            var ltl = converter.ProcessLTL();
            Assert.AreEqual("#define p0 (a1.b == 5)", converter.Propositions[0]);
            Assert.AreEqual("[]p0", ltl[0]);
        }

        /// <summary>
        /// Test an array with constant index.
        /// </summary>
        [TestMethod]
        public void TestArrayConst()
        {
            Generator generator = new Generator();
            generator.GenerateFSMWithVariables();
            generator.Options.FormulaeLTL.Clear();
            generator.Options.FormulaeLTL.Add("[]{a1.arr[5] == 5}");
            LTLConverter converter = new LTLConverter(generator.Options, generator.FSMSystem, generator.ForkList);
            var ltl = converter.ProcessLTL();
            Assert.AreEqual("#define p0 (a1.arr[5] == 5)", converter.Propositions[0]);
            Assert.AreEqual("[]p0", ltl[0]);
        }

        /// <summary>
        /// Variable and variable.
        /// </summary>
        [TestMethod]
        public void TestVarVar()
        {
            Generator generator = new Generator();
            generator.GenerateFSMWithVariables();
            generator.Options.FormulaeLTL.Clear();
            generator.Options.FormulaeLTL.Add("[]{a1.b == a1.c}");

            LTLConverter converter = new LTLConverter(generator.Options, generator.FSMSystem, generator.ForkList);
            var ltl = converter.ProcessLTL();

            Assert.AreEqual("#define p0 (a1.b == a1.c)", converter.Propositions[0]);
            Assert.AreEqual("[]p0", ltl[0]);
        }

        /// <summary>
        ///a1.b >= a1.c
        /// </summary>
        /// 
        [TestMethod]
        public void TestVarVarGE()
        {
            Generator generator = new Generator();
            generator.GenerateFSMWithVariables();
            generator.Options.FormulaeLTL.Clear();
            generator.Options.FormulaeLTL.Add("[]{a1.b >= a1.c}");

            LTLConverter converter = new LTLConverter(generator.Options, generator.FSMSystem, generator.ForkList);
            var ltl = converter.ProcessLTL();

            Assert.AreEqual("#define p0 (a1.b >= a1.c)", converter.Propositions[0]);
            Assert.AreEqual("[]p0", ltl[0]);
        }

        /// <summary>
        /// a1.arr[5] > a1.arr[4]
        /// </summary>
        [TestMethod]
        public void TestArrArr()
        {
            Generator generator = new Generator();
            generator.GenerateFSMWithVariables();
            generator.Options.FormulaeLTL.Clear();
            generator.Options.FormulaeLTL.Add("[]{a1.arr[5] > a1.arr[4]}");
            LTLConverter converter = new LTLConverter(generator.Options, generator.FSMSystem, generator.ForkList);
            var ltl = converter.ProcessLTL();
            Assert.AreEqual("#define p0 (a1.arr[5] > a1.arr[4])", converter.Propositions[0]);
            Assert.AreEqual("[]p0", ltl[0]);
        }

        /// <summary>
        /// a1.arr[5] < a1.c
        /// </summary>
        [TestMethod]
        public void TestVarArr()
        {
            Generator generator = new Generator();
            generator.GenerateFSMWithVariables();
            generator.Options.FormulaeLTL.Clear();
            generator.Options.FormulaeLTL.Add("[]{a1.arr[5] < a1.c}");
            LTLConverter converter = new LTLConverter(generator.Options, generator.FSMSystem, generator.ForkList);
            var ltl = converter.ProcessLTL();
            Assert.AreEqual("#define p0 (a1.arr[5] < a1.c)", converter.Propositions[0]);
            Assert.AreEqual("[]p0", ltl[0]);
        }

        /// <summary>
        /// Regression test.
        /// </summary>
        [TestMethod]
        public void TestBoolVariable()
        {
            Generator generator = new Generator();
            generator.GenerateFSMWithVariables();
            generator.Options.FormulaeLTL.Clear();
            generator.Options.FormulaeLTL.Add("[]{a1.flag == true}");
            LTLConverter converter = new LTLConverter(generator.Options, generator.FSMSystem, generator.ForkList);
            var ltl = converter.ProcessLTL();
            Assert.AreEqual("#define p0 (a1.flag == true)", converter.Propositions[0]);
            Assert.AreEqual("[]p0", ltl[0]);
        }
    }
}
