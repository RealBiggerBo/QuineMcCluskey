using QuineMcCluskey;
using System.Data;
using System.Security.Cryptography.X509Certificates;

namespace QMC_UnitTest
{
    //Solutions at: https://www.mathematik.uni-marburg.de/~thormae/lectures/ti1/code/qmc/
    public class QMCSolverTestClass
    {
        [Fact]
        public void DefaultTestCase()
        {
            //Arrange
            IntQuineMcCluskeySolver solver = new IntQuineMcCluskeySolver(1, 3, 5, 10, 11, 12, 13, 14, 15);
            Iteration_Optimised expectedIteration = new Iteration_Optimised(new Value_Optimised("00-1"), new Value_Optimised("0-01"), new Value_Optimised("1-1-"), new Value_Optimised("11--"));

            //Act
            Iteration_Optimised actualResult = solver.Solve();

            //Assert
            Value_Optimised[] expected = expectedIteration.GetValues().ToArray();
            Value_Optimised[] actual = actualResult.GetValues().ToArray();

            for (int i = 0; i < expected.Length; i++)
            {
                Assert.Contains<Value_Optimised>(expected[i], actual);
            }
        }

        [Fact]
        public void BranchingTestCase()
        {
            //Arrange
            IntQuineMcCluskeySolver solver = new IntQuineMcCluskeySolver(1, 2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 33, 41, 43, 47, 53, 59, 61);
            Iteration_Optimised expectedIteration = new Iteration_Optimised(new Value_Optimised("00001-"), new Value_Optimised("0-00-1"),
                new Value_Optimised("101-11"), new Value_Optimised("11-101"), new Value_Optimised("1-1011"), new Value_Optimised("00-011"),
                new Value_Optimised("10-001"), new Value_Optimised("000--1"), new Value_Optimised("0-1101"), new Value_Optimised("01-111"));

            //Act
            Iteration_Optimised actualResult = solver.Solve();

            //Assert
            Value_Optimised[] expected = expectedIteration.GetValues().ToArray();
            Value_Optimised[] actual = actualResult.GetValues().ToArray();

            for (int i = 0; i < expected.Length; i++)
            {
                Assert.Contains<Value_Optimised>(expected[i], actual);
            }
        }
    }

    public class ValueTestClass
    {
        [Fact]
        public void EqualityTestCase()
        {
            //Arrange
            Value_Optimised valA = new Value_Optimised("0000");
            Value_Optimised valB = new Value_Optimised("1111");

            Value_Optimised _valA = new Value_Optimised(0, 4);
            Value_Optimised _valB = new Value_Optimised(15, 4);
            Value_Optimised valC = new Value_Optimised("----");

            //Act


            //Assert
            Assert.True(valA.Equals(_valA));
            Assert.True(valB.Equals(_valB));
            Assert.False(valA.Equals(_valB));
            Assert.False(valB.Equals(_valA));
            Assert.False(valA.Equals(valC));
            Assert.False(valB.Equals(valC));
        }

        [Fact]
        public void IsSimilarTestCase()
        {
            //Arrange
            Value_Optimised valA = new Value_Optimised("0000");//Group 0
            Value_Optimised valAB = new Value_Optimised("000-");
            Value_Optimised valB = new Value_Optimised("0001");//Group 1
            Value_Optimised valBC = new Value_Optimised("00-1");
            Value_Optimised valBD = new Value_Optimised("0-01");
            Value_Optimised valG = new Value_Optimised("111-");//Group 3
            Value_Optimised valH = new Value_Optimised("1101");
            Value_Optimised valHI = new Value_Optimised("11-1");
            Value_Optimised valI = new Value_Optimised("1111");//Group 4


            //Act

            //Assert
            Assert.True(valA.IsSimilar(valB));
            Assert.False(valA.IsSimilar(valBC));
            Assert.False(valA.IsSimilar(valBD));
            Assert.True(valAB.IsSimilar(valB));
            Assert.False(valAB.IsSimilar(valBC));
            Assert.False(valAB.IsSimilar(valBD));
            Assert.False(valG.IsSimilar(valHI));
            Assert.True(valG.IsSimilar(valI));
            Assert.True(valG.IsSimilar(valG));
            Assert.True(valH.IsSimilar(valI));
            Assert.False(valH.IsSimilar(valBD));
            Assert.True(valH.IsSimilar(valHI));
        }

        [Fact]
        public void CombineValuesTestCase()
        {
            //Arrange
            Value_Optimised valA = new Value_Optimised("0000");
            Value_Optimised valB = new Value_Optimised("0001");
            Value_Optimised valC = new Value_Optimised("0011");
            Value_Optimised valD = new Value_Optimised("0101");
            Value_Optimised valE = new Value_Optimised("101-");
            Value_Optimised valF = new Value_Optimised("11-0");
            Value_Optimised valG = new Value_Optimised("111-");
            Value_Optimised valH = new Value_Optimised("1101");
            Value_Optimised valI = new Value_Optimised("1111");

            Value_Optimised valAB = new Value_Optimised("000-");
            Value_Optimised valBC = new Value_Optimised("00-1");
            Value_Optimised valBD = new Value_Optimised("0-01");
            Value_Optimised valDH = new Value_Optimised("-101");
            Value_Optimised valEG = new Value_Optimised("1-1-");
            Value_Optimised valHI = new Value_Optimised("11-1");

            //Act
            Value_Optimised _valAB = new Value_Optimised(valA, valB);
            Value_Optimised _valBA = new Value_Optimised(valB, valA);
            Value_Optimised _valBC = new Value_Optimised(valB, valC);
            Value_Optimised _valCB = new Value_Optimised(valC, valB);
            Value_Optimised _valBD = new Value_Optimised(valB, valD);
            Value_Optimised _valDB = new Value_Optimised(valD, valB);
            Value_Optimised _valDH = new Value_Optimised(valD, valH);
            Value_Optimised _valHD = new Value_Optimised(valH, valD);
            Value_Optimised _valEG = new Value_Optimised(valE, valG);
            Value_Optimised _valGE = new Value_Optimised(valG, valE);
            Value_Optimised _valHI = new Value_Optimised(valH, valI);
            Value_Optimised _valIH = new Value_Optimised(valI, valH);


            //Assert
            Assert.Equal(valAB, _valAB);
            Assert.Equal(valBC, _valBC);
            Assert.Equal(valBD, _valBD);
            Assert.Equal(valDH, _valDH);
            Assert.Equal(valEG, _valEG);
            Assert.Equal(valHI, _valHI);
            Assert.Equal(valAB, _valBA);
            Assert.Equal(valBC, _valCB);
            Assert.Equal(valBD, _valDB);
            Assert.Equal(valDH, _valHD);
            Assert.Equal(valEG, _valGE);
            Assert.Equal(valHI, _valIH);
        }

        [Fact]
        public void GroupIndexTestCase()
        {
            //Arrange
            Value_Optimised valA = new Value_Optimised("0000");
            Value_Optimised valB = new Value_Optimised("0001");
            Value_Optimised valC = new Value_Optimised("0011");
            Value_Optimised valD = new Value_Optimised("0101");
            Value_Optimised valE = new Value_Optimised("101-");
            Value_Optimised valF = new Value_Optimised("11-0");
            Value_Optimised valG = new Value_Optimised("111-");
            Value_Optimised valH = new Value_Optimised("1101");
            Value_Optimised valI = new Value_Optimised("1111");

            Value_Optimised valAB = new Value_Optimised("000-");
            Value_Optimised valBC = new Value_Optimised("00-1");
            Value_Optimised valBD = new Value_Optimised("0-01");
            Value_Optimised valDH = new Value_Optimised("-101");
            Value_Optimised valEG = new Value_Optimised("1-1-");
            Value_Optimised valHI = new Value_Optimised("11-1");

            //Act
            int A = valA.GetGroupIndex();
            int B = valB.GetGroupIndex();
            int C = valC.GetGroupIndex();
            int D = valD.GetGroupIndex();
            int E = valE.GetGroupIndex();
            int F = valF.GetGroupIndex();
            int G = valG.GetGroupIndex();
            int H = valH.GetGroupIndex();
            int I = valI.GetGroupIndex();

            int AB =valAB.GetGroupIndex();
            int BC =valBC.GetGroupIndex();
            int BD =valBD.GetGroupIndex();
            int DH =valDH.GetGroupIndex();
            int EG =valEG.GetGroupIndex();
            int HI =valHI.GetGroupIndex();

            //Assert
            Assert.Equal(0, A);
            Assert.Equal(1, B);
            Assert.Equal(2, C);
            Assert.Equal(2, D);
            Assert.Equal(2, E);
            Assert.Equal(2, F);
            Assert.Equal(3, G);
            Assert.Equal(3, H);
            Assert.Equal(4, I);
            Assert.Equal(0, AB);
            Assert.Equal(1, BC);
            Assert.Equal(1, BD);
            Assert.Equal(2, DH);
            Assert.Equal(2, EG);
            Assert.Equal(3, HI);
        }

        [Fact]
        public void ImpliesTestCase()
        {
            //Arrange
            Value_Optimised[] outputValues = new Value_Optimised[] {
                new Value_Optimised(1, 4),
                new Value_Optimised(3, 4),
                new Value_Optimised(5, 4),
                new Value_Optimised(10, 4),
                new Value_Optimised(11, 4),
                new Value_Optimised(12, 4),
                new Value_Optimised(13, 4),
                new Value_Optimised(14, 4),
                new Value_Optimised(15, 4)
            };
            Value_Optimised[] implicants = new Value_Optimised[] {
                new Value_Optimised("00-1"),
                new Value_Optimised("0-01"),
                new Value_Optimised("-011"),
                new Value_Optimised("-101"),
                new Value_Optimised("1-1-"),
                new Value_Optimised("11--")
            };
            int[] data = new int[] {
                1,1,0,0,0,0,0,0,0,
                1,0,1,0,0,0,0,0,0,
                0,1,0,0,1,0,0,0,0,
                0,0,1,0,0,0,1,0,0,
                0,0,0,1,1,0,0,1,1,
                0,0,0,0,0,1,1,1,1};

            //Act
            Table_Optimised table = new Table_Optimised(outputValues, implicants.ToList());

            //Assert
            for (int y = 0; y < 6; y++)
            {
                for (int x = 0; x < 9; x++)
                {
                    Assert.Equal(data[y * 9 + x] == 1, outputValues[x].Implies(implicants[y]));
                }
            }
        }
    }
}