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
    }
}