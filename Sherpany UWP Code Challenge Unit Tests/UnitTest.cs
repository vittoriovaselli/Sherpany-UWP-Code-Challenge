
using System;
using GalaSoft.MvvmLight;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sherpany_UWP_Code_Challenge.ViewModel;
using Sherpany_UWP_Code_Challenge.ViewModel.Pages;

namespace Sherpany_UWP_Code_Challenge_Unit_Tests
{
    [TestClass]
    public class UnitTest1
    {
        private ViewModelLocator locator;
        private MainPageViewModel viewModel;

        [TestInitialize]
        public void Init()
        {
            locator = new ViewModelLocator();
            viewModel = locator.MainPage;
        }

        [TestMethod]
        public void TestValidPin()
        {
            viewModel.Password = "123456";
            Assert.IsTrue(viewModel.SetPasswordAndNavigateCommand.CanExecute(null));
        }

        [TestMethod]
        public void TestNotDigitPin()
        {
            viewModel.Password = "abcdef";
            Assert.IsFalse(viewModel.SetPasswordAndNavigateCommand.CanExecute(null));
        }

        [TestMethod]
        public void TestInvalidCharacterPin()
        {
            viewModel.Password = "1234c6";
            Assert.IsFalse(viewModel.SetPasswordAndNavigateCommand.CanExecute(null));
        }

        [TestMethod]
        public void TestPinTooLong()
        {
            viewModel.Password = "1234567";
            Assert.IsFalse(viewModel.SetPasswordAndNavigateCommand.CanExecute(null));
        }

        [TestMethod]
        public void TestPinTooShort()
        {
            viewModel.Password = "12345";
            Assert.IsFalse(viewModel.SetPasswordAndNavigateCommand.CanExecute(null));
        }

        [TestMethod]
        public void TestNullStringPin()
        {
            viewModel.Password = null;
            Assert.IsFalse(viewModel.SetPasswordAndNavigateCommand.CanExecute(null));
        }

        [TestMethod]
        public void TestEmptyStringPin()
        {
            viewModel.Password = "";
            Assert.IsFalse(viewModel.SetPasswordAndNavigateCommand.CanExecute(null));
        }
    }
}
