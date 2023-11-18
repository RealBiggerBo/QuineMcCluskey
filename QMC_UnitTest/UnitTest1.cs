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
            IntIteration expectedIteration = new IntIteration(new IntValue("00-1"), new IntValue("0-01"), new IntValue("1-1-"), new IntValue("11--"));

            //Act
            IntIteration actualResult = solver.Solve();

            //Assert
            IntValue[] expected = expectedIteration.GetValues();
            IntValue[] actual = actualResult.GetValues();

            for (int i = 0; i < expected.Length; i++)
            {
                Assert.Contains<IntValue>(expected[i], actual);
            }
        }

        [Fact]
        public void BranchingTestCase()
        {
            //Arrange
            IntQuineMcCluskeySolver solver = new IntQuineMcCluskeySolver(1, 2, 3, 5, 7, 11, 13, 17, 19, 23, 29, 31, 33, 41, 43, 47, 53, 59, 61);
            IntIteration expectedIteration = new IntIteration(new IntValue("00001-"), new IntValue("0-00-1"),
                new IntValue("101-11"), new IntValue("11-101"), new IntValue("1-1011"), new IntValue("00-011"),
                new IntValue("10-001"), new IntValue("000--1"), new IntValue("0-1101"), new IntValue("01-111"));

            //Act
            IntIteration actualResult = solver.Solve();

            //Assert
            IntValue[] expected = expectedIteration.GetValues();
            IntValue[] actual = actualResult.GetValues();

            for (int i = 0; i < expected.Length; i++)
            {
                Assert.Contains<IntValue>(expected[i], actual);
            }
        }
    }

    public class ValueTestClass
    {
        [Fact]
        public void EqualityTestCase()
        {
            //Arrange
            IntValue valA = new IntValue("0000");
            IntValue valB = new IntValue("1111");

            IntValue _valA = new IntValue(0, 4);
            IntValue _valB = new IntValue(15, 4);
            IntValue valC = new IntValue("----");

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
            IntValue valA = new IntValue("0000");//Group 0
            IntValue valAB = new IntValue("000-");
            IntValue valB = new IntValue("0001");//Group 1
            IntValue valBC = new IntValue("00-1");
            IntValue valBD = new IntValue("0-01");
            IntValue valG = new IntValue("111-");//Group 3
            IntValue valH = new IntValue("1101");
            IntValue valHI = new IntValue("11-1");
            IntValue valI = new IntValue("1111");//Group 4


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
            IntValue valA = new IntValue("0000");
            IntValue valB = new IntValue("0001");
            IntValue valC = new IntValue("0011");
            IntValue valD = new IntValue("0101");
            IntValue valE = new IntValue("101-");
            IntValue valF = new IntValue("11-0");
            IntValue valG = new IntValue("111-");
            IntValue valH = new IntValue("1101");
            IntValue valI = new IntValue("1111");

            IntValue valAB = new IntValue("000-");
            IntValue valBC = new IntValue("00-1");
            IntValue valBD = new IntValue("0-01");
            IntValue valDH = new IntValue("-101");
            IntValue valEG = new IntValue("1-1-");
            IntValue valHI = new IntValue("11-1");

            //Act
            IntValue _valAB = new IntValue(valA, valB);
            IntValue _valBA = new IntValue(valB, valA);
            IntValue _valBC = new IntValue(valB, valC);
            IntValue _valCB = new IntValue(valC, valB);
            IntValue _valBD = new IntValue(valB, valD);
            IntValue _valDB = new IntValue(valD, valB);
            IntValue _valDH = new IntValue(valD, valH);
            IntValue _valHD = new IntValue(valH, valD);
            IntValue _valEG = new IntValue(valE, valG);
            IntValue _valGE = new IntValue(valG, valE);
            IntValue _valHI = new IntValue(valH, valI);
            IntValue _valIH = new IntValue(valI, valH);


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
            IntValue valA = new IntValue("0000");
            IntValue valB = new IntValue("0001");
            IntValue valC = new IntValue("0011");
            IntValue valD = new IntValue("0101");
            IntValue valE = new IntValue("101-");
            IntValue valF = new IntValue("11-0");
            IntValue valG = new IntValue("111-");
            IntValue valH = new IntValue("1101");
            IntValue valI = new IntValue("1111");

            IntValue valAB = new IntValue("000-");
            IntValue valBC = new IntValue("00-1");
            IntValue valBD = new IntValue("0-01");
            IntValue valDH = new IntValue("-101");
            IntValue valEG = new IntValue("1-1-");
            IntValue valHI = new IntValue("11-1");

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
            IntValue[] outputValues = new IntValue[] {
                new IntValue(1, 4),
                new IntValue(3, 4),
                new IntValue(5, 4),
                new IntValue(10, 4),
                new IntValue(11, 4),
                new IntValue(12, 4),
                new IntValue(13, 4),
                new IntValue(14, 4),
                new IntValue(15, 4)
            };
            IntValue[] implicants = new IntValue[] {
                new IntValue("00-1"),
                new IntValue("0-01"),
                new IntValue("-011"),
                new IntValue("-101"),
                new IntValue("1-1-"),
                new IntValue("11--")
            };
            int[] data = new int[] {
                1,1,0,0,0,0,0,0,0,
                1,0,1,0,0,0,0,0,0,
                0,1,0,0,1,0,0,0,0,
                0,0,1,0,0,0,1,0,0,
                0,0,0,1,1,0,0,1,1,
                0,0,0,0,0,1,1,1,1};

            //Act
            IntTable table = new IntTable(outputValues, implicants);

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