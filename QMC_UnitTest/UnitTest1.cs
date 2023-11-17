using QuineMcCluskey;

namespace QMC_UnitTest
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            //Arrange
            QuineMcCluskeySolver solver = new QuineMcCluskeySolver(1, 3, 5, 10, 11, 12, 13, 14, 15);

            //Act
            solver.Solve();

            //Assert
        }
    }
}