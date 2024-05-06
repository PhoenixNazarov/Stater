using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpinVeriff;

namespace SpinVeriffTest
{
    [TestClass]
    public class VariablesTest
    {
        [TestMethod]
        public void TestModify1()
        {
            Generator generator = new Generator();
            generator.GenerateFSMWithVariables();

            var newCode = ModelGenerator.ModifyVariables(generator.FSMSystem[0], "b = b + 1;");
            Assert.AreEqual("machine.b=machine.b+1;", newCode);

            newCode = ModelGenerator.ModifyVariables(generator.FSMSystem[0], "b=b+1;");
            Assert.AreEqual("machine.b=machine.b+1;", newCode);
        }

        [TestMethod]
        public void TestArray()
        {
            Generator generator = new Generator();
            generator.GenerateFSMWithVariables();

            var newCode = ModelGenerator.ModifyVariables(generator.FSMSystem[0], "arr[b]=arr[b+1];");
            Assert.AreEqual("machine.arr[machine.b]=machine.arr[machine.b+1];", newCode);

            newCode = ModelGenerator.ModifyVariables(generator.FSMSystem[0], "arr[ b ]   = arr[\tb +\t1];");
            Assert.AreEqual("machine.arr[machine.b]=machine.arr[machine.b+1];", newCode);
        }
    }
}
