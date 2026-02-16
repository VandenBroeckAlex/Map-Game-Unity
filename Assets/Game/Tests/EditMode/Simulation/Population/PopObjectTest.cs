using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace UnitTest.population
{
    public class PopObjectTest
    {
        public static Pop CreatePop()
        {
            GoodRequirement gr = new GoodRequirement(1, 500, 500);
            List<GoodRequirement> goodReq = new List<GoodRequirement>();
            goodReq.Add(gr);
            Pop pop = new Pop(0, 1000, 1, 1, 1, 1, 100, goodReq);
            return pop;
        }

        [Test]
        public static void Population_HaveBasicNeed_ReturnTrue()
        {
            try
            {
                //Arrange - go get variables
                Pop pop = CreatePop();
                //Act 
                bool result = pop.HaveBasicNeed();

                //Assert 
                Assert.IsTrue(result);
            }
            catch (Exception ex) 
            { 
                Console.WriteLine(ex.ToString());
            }
        }
        [Test]
        public static void Population_HaveBasicNeed_ReturnFalse()
        {
            try
            {
                //Arrange - go get variables
                Pop pop = CreatePop();
                //Act 
                pop.GoodList[0].Stockpile = 0;
                bool result = pop.HaveBasicNeed();

                //Assert 
                Assert.IsFalse(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
        }
        [Test]
        public static void Population_GetUnemployedNumber_returnTrue()
        {
            Pop pop = CreatePop();

            int result = pop.GetUnemployedNumber();

            Assert.AreEqual(pop.size, result);
        }
        [Test]
        public static void Population_HireInWorkplace_returnTrue()
        {
            int numberHierd = 500;

            Pop pop = CreatePop();

            pop.HireInWorkplace(1, numberHierd);    

            int result = pop.GetUnemployedNumber();

            Assert.AreEqual((pop.size - numberHierd), result);
        }
        [Test]
        public static void Population_FiredFromWorkplace_returnTrue()
        {
            int numberHierd = 500;

            Pop pop = CreatePop();

            pop.HireInWorkplace(1, numberHierd);

            pop.FiredFromWorkplace(1, 250);

            int result = pop.GetUnemployedNumber();

            Assert.AreEqual((pop.size - 250), result);
        }
        public static void Population_workplace_returnTrue()
        {
            int numberHierd = 500;

            Pop pop = CreatePop();

            pop.HireInWorkplace(1, numberHierd);

            Assert.IsTrue(pop.workplace[0].num == 500);
            Assert.IsTrue(pop.workplace[0].id == 1);
            
        }
    }
}
