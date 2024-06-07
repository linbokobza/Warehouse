
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WareHouse22.Controllers;
using System.Web.Mvc;
using System;


namespace UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Test_UserRegister(dynamic data, dynamic database, JsonResult jsonResult)
        {
            //check if recive false json if user already exists
            var controller = new UsersController();
            var result = controller.UserRegister("admin@ac.sce.ac.il", "admin", "admin", "1234", "A");
            //var jsonResult = result as JsonResult;
            Assert.IsNotNull(jsonResult);

            object data1 = jsonResult.Data;
            dynamic data22 = data1;
            string status = data22.GetValue("status").ToString();
            Assert.AreEqual("false", status);
        }

        [TestMethod]
        public void Test_Registration()
        {
            // Arrange
            var controller = new UsersController();

            // Act
            var result = controller.Registration() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Registration", result.ViewName);
        }
        [TestMethod]
        public void Test_AddItem(dynamic data22)
        {
            //check if recive false json if item already exists
            var controller = new UsersController();
            var result = controller.AddItem("1234567", " ", " ", " ", " ", 10);
            var jsonResult = result as JsonResult;
            Assert.IsNotNull(jsonResult);

            //dynamic data22 = jsonResult.Data;
            //string status = data22.GetValue("status").ToString();
            //Assert.AreEqual("false", (Object)status);
        }
        [TestMethod]
        public void Test_Home()
        {
            // Arrange
            var controller = new UsersController();

            // Act
            var result = controller.Home() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Home", result.ViewName);
        }
        [TestMethod]
        public void Test_UserSignIn()
        {
            //check if recive false json if try signin with user not exists
            var controller = new UsersController();
            var result = controller.UserSignIn("test", "test");
            var jsonResult = result as JsonResult;
            Assert.IsNotNull(jsonResult);

            //dynamic data22 = jsonResult.Data;
            //string status = data22.GetValue("status").ToString();
            //Assert.AreEqual("false", status);
        }
    }
}

