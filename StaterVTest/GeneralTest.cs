using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StaterV;

namespace StaterVTest
{
    [TestClass]
    public class GeneralTest
    {
        [TestMethod]
        public void FilesExt1Test()
        {
            Assert.AreEqual(".ext", FilesInfo.DetermineExtension("file.ext"));
            Assert.AreEqual("", FilesInfo.DetermineExtension("file"));
            Assert.AreEqual(".htaccess", FilesInfo.DetermineExtension(".htaccess"));
            Assert.AreEqual(FilesInfo.DiagramExtension, FilesInfo.DetermineExtension("auto.cs.xstd"));
            Assert.AreEqual(FilesInfo.ProjectExtension, FilesInfo.DetermineExtension(@"D:\IFMO\project.stp"));
        }
    }
}
