using AventStack.ExtentReports.Reporter;
using AventStack.ExtentReports;
using Backend.Framework.BaseActions;
using NUnit.Framework;
using RestSharp;
using Backend.Framework.Utilities;

namespace Backend.Framework.Tests
{
    public abstract class BaseTests
    {
        protected ExtentTest test;
        protected ExtentReports extent;
        protected RestClient restClient;
        protected Actions actions;
        protected ExampleActions exampleActions;

        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            restClient = Actions.NewRestClient();

            extent = new ExtentReports();
            var htmlreporter = new ExtentHtmlReporter(Properties.reporterFileLocation);
            extent.AttachReporter(htmlreporter);
        }

        [SetUp]
        public void SetUp()
        {
            test = extent.CreateTest("Start test");
            actions = new Actions(test);
            exampleActions = new ExampleActions(test);
        }

        [OneTimeTearDown]
        public void ExtentClose()
        {
            extent.Flush();
        }
    }
}
