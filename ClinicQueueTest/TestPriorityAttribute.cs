using Xunit.Abstractions;
using Xunit.Sdk;

namespace ClinicQueueTest
{
    public class TestPriorityAttribute : Attribute
    {
        public int Priority { get; }
        public TestPriorityAttribute(int priority) => Priority = priority;
    }

    public class PriorityOrderer : ITestCaseOrderer
    {
        public IEnumerable<TTestCase> OrderTestCases<TTestCase>(
            IEnumerable<TTestCase> testCases)
            where TTestCase : ITestCase
        {
            var sortedMethods = testCases
                .OrderBy(testCase =>
                    testCase.TestMethod.Method
                        .GetCustomAttributes(typeof(TestPriorityAttribute).AssemblyQualifiedName)
                        .FirstOrDefault()?.GetNamedArgument<int>("Priority") ?? 0);

            return sortedMethods;
        }
    }
}
