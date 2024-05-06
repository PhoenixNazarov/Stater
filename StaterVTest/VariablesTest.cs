using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StaterV.StateMachine;
using StaterV.Variables;

namespace StaterVTest
{
    [TestClass]
    public class VariablesTest
    {
        [TestMethod]
        public void TestSingle1()
        {
            var v = EditVariablesLogic.ParseLine("int8 v;");

            Assert.AreEqual(typeof(SingleVariable), v.GetType());
            var sv = v as SingleVariable;
            Assert.AreEqual(Variable.TypeList.Int8, sv.Type);
            Assert.AreEqual("v", sv.Name);
            Assert.AreEqual("0", sv.Value);
        }

        [TestMethod]
        public void TestSingle2True()
        {
            var v = EditVariablesLogic.ParseLine("bool flag = true;");

            Assert.AreEqual(typeof(SingleVariable), v.GetType());
            var sv = v as SingleVariable;
            Assert.AreEqual(Variable.TypeList.Bool, sv.Type);
            Assert.AreEqual("flag", sv.Name);
            Assert.AreEqual("true", sv.Value);
        }

        [TestMethod]
        public void TestSingle2False()
        {
            var v = EditVariablesLogic.ParseLine("bool\tflag   =false     ;");

            Assert.AreEqual(typeof(SingleVariable), v.GetType());
            var sv = v as SingleVariable;
            Assert.AreEqual(Variable.TypeList.Bool, sv.Type);
            Assert.AreEqual("flag", sv.Name);
            Assert.AreEqual("false", sv.Value);
        }

        [TestMethod]
        public void TestSingle3True()
        {
            var v = EditVariablesLogic.ParseLine("bool flag=true;");

            Assert.AreEqual(typeof(SingleVariable), v.GetType());
            var sv = v as SingleVariable;
            Assert.AreEqual(Variable.TypeList.Bool, sv.Type);
            Assert.AreEqual("flag", sv.Name);
            Assert.AreEqual("true", sv.Value);
        }

        [TestMethod]
        public void TestArray1()
        {
            var v = EditVariablesLogic.ParseLine("int16 arr\t[5];");
        }

        [TestMethod]
        public void TestArrayW1()
        {
            try
            {
                var v = EditVariablesLogic.ParseLine("bool flags[];");
                Assert.Fail("Must be exception!");
            }
            catch (Exception e)
            {
                Assert.AreEqual(typeof(ArgumentException), e.GetType());
            }
        }
    }
}
