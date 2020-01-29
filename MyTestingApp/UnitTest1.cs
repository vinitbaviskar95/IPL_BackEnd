using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MyUnitTestProject
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            string otp = null;
            bool result = MySecurityLib.Security.GetOTP(out otp);
            Assert.AreEqual(true, result);
            Assert.AreNotEqual(string.Empty, otp);
        }
        [TestMethod]
        public void TestMethod2()
        {
            string data = null;
            bool result = MySecurityLib.Security.Encrypt("pswd", out data);
            Assert.AreEqual(true, result);
            Assert.AreNotEqual(string.Empty, data);
        }
        [TestMethod]
        public void TestMethod3()
        {
            string data = null;
            bool result = MySecurityLib.Security.Decrypt("uonFDG0wiRw =", out data);
            Assert.AreEqual(true, result);
            // Assert.AreNotEqual(string.Empty, res);
        }
    }
}
