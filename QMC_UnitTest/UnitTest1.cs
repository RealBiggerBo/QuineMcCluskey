using QuineMcCluskey;

namespace QMC_UnitTest
{
    //Solutions at: https://www.mathematik.uni-marburg.de/~thormae/lectures/ti1/code/qmc/
    public class QMCSolverTestClass
    {
        [Fact]
        public void DefaultTestCase()
        {
            //Arrange
            QuineMcCluskeySolver solver = new QuineMcCluskeySolver(1, 3, 5, 10, 11, 12, 13, 14, 15);
            Iteration expectedIteration = new Iteration(new Value("00-1"), new Value("0-01"), new Value("1-1-"), new Value("11--"));

            //Act
            Iteration actualResult = solver.Solve();

            //Assert
            Value[] expected = expectedIteration.GetValues();
            Value[] actual = actualResult.GetValues();

            for (int i = 0; i < expected.Length; i++)
            {
                Assert.Contains<Value>(expected[i], actual);
            }
        }

        [Fact]
        public void BranchingTestCase()
        {
            //Arrange
            QuineMcCluskeySolver solver = new QuineMcCluskeySolver(1, 2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 33, 41, 43, 47, 53, 59, 61);
            Iteration expectedIteration = new Iteration(new Value("00001-"), new Value("0-00-1"),
                new Value("101-11"), new Value("11-101"), new Value("1-1011"), new Value("00-011"),
                new Value("10-001"), new Value("000--1"), new Value("0-1101"), new Value("01-111"));

            //Act
            Iteration actualResult = solver.Solve();

            //Assert
            Value[] expected = expectedIteration.GetValues();
            Value[] actual = actualResult.GetValues();

            for (int i = 0; i < expected.Length; i++)
            {
                Assert.Contains<Value>(expected[i], actual);
            }
        }
    }
}