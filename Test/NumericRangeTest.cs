/*
   Bitvantage.NumericRange
   Copyright (C) 2024 Michael Crino
   
   This program is free software: you can redistribute it and/or modify
   it under the terms of the GNU Affero General Public License as published by
   the Free Software Foundation, either version 3 of the License, or
   (at your option) any later version.
   
   This program is distributed in the hope that it will be useful,
   but WITHOUT ANY WARRANTY; without even the implied warranty of
   MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
   GNU Affero General Public License for more details.
   
   You should have received a copy of the GNU Affero General Public License
   along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

using Bitvantage;

namespace Test
{
    internal class NumericRangeTest
    {
        [Test]
        public void Add01()
        {
            var range = new NumericRange();
            
            range.Add(10, 20);
            range.Add(25);
            range.Add(27, 30);
            range.Add(33, 53);

            range.Add(25, 40);

            Assert.That(range.ToString(), Is.EqualTo("10-20,25-53"));
        }

        [Test]
        public void Add02()
        {
            var range = new NumericRange();

            range.Add(10, 20);
            range.Add(25);
            range.Add(27, 30);
            range.Add(33, 53);

            range.Add(13, 25);

            Assert.That(range.ToString(), Is.EqualTo("10-25,27-30,33-53"));
        }

        [Test]
        public void Add03()
        {
            var range = new NumericRange();

            range.Add(10, 20);
            range.Add(25);
            range.Add(27, 30);
            range.Add(33, 53);

            range.Add(21, 25);

            Assert.That(range.ToString(), Is.EqualTo("10-25,27-30,33-53"));
        }

        [Test]
        public void Add04()
        {
            var range = new NumericRange();

            range.Add(10, 20);
            range.Add(25);
            range.Add(27, 30);
            range.Add(33, 53);

            range.Add(22, 25);

            Assert.That(range.ToString(), Is.EqualTo("10-20,22-25,27-30,33-53"));
        }

        [Test]
        public void Add05()
        {
            var range = new NumericRange();

            range.Add(10, 20);
            range.Add(25);
            range.Add(27, 30);
            range.Add(33, 53);

            range.Add(20, 25);

            Assert.That(range.ToString(), Is.EqualTo("10-25,27-30,33-53"));
        }

        [Test]
        public void Add06()
        {
            var range = new NumericRange();

            range.Add(10, 20);
            range.Add(25);
            range.Add(27, 30);
            range.Add(33, 53);

            range.Add(19, 25);

            Assert.That(range.ToString(), Is.EqualTo("10-25,27-30,33-53"));
        }

        [Test]
        public void Add07()
        {
            var range = new NumericRange();

            range.Add(10, 20);
            range.Add(25);
            range.Add(27, 30);
            range.Add(33, 53);

            range.Add(10, 25);

            Assert.That(range.ToString(), Is.EqualTo("10-25,27-30,33-53"));
        }

        [Test]
        public void Add08()
        {
            var range = new NumericRange();

            range.Add(10, 20);
            range.Add(25);
            range.Add(27, 30);
            range.Add(33, 53);

            range.Add(09, 25);

            Assert.That(range.ToString(), Is.EqualTo("9-25,27-30,33-53"));
        }

        [Test]
        public void Add09()
        {
            var range = new NumericRange();

            range.Add(0);
            range.Add(100);
            range.Add(ulong.MaxValue);

            Assert.That(range.ToString(), Is.EqualTo("0,100,18446744073709551615"));
        }

        [Test]
        public void Add10()
        {
            var range = new NumericRange();

            //range.Add(0);
            //range.Add(100);

            range.Add(ulong.MaxValue);
            range.Add(ulong.MinValue, ulong.MaxValue);

            Assert.That(range.ToString(), Is.EqualTo("0-18446744073709551615"));
        }


        [Test]
        public void Parse01()
        {
            var range = NumericRange.Parse("10,100,1000,10000-2000000");

            Assert.That(range.ToString(), Is.EqualTo("10,100,1000,10000-2000000"));
        }

        [Test]
        public void Contains01()
        {
            var range = new NumericRange();

            range.Add(10, 20);
            range.Add(ulong.MaxValue);
            range.Add(25);
            range.Add(27, 30);
            range.Add(33, 53);


            Assert.That(range.Contains(0), Is.False);
            Assert.That(range.Contains(10), Is.True);
            Assert.That(range.Contains(11), Is.True);
            Assert.That(range.Contains(12), Is.True);
            Assert.That(range.Contains(13), Is.True);
            Assert.That(range.Contains(14), Is.True);
            Assert.That(range.Contains(15), Is.True);
            Assert.That(range.Contains(16), Is.True);
            Assert.That(range.Contains(17), Is.True);
            Assert.That(range.Contains(18), Is.True);
            Assert.That(range.Contains(19), Is.True);
            Assert.That(range.Contains(20), Is.True);
            Assert.That(range.Contains(21), Is.False);
            Assert.That(range.Contains(22), Is.False);
            Assert.That(range.Contains(23), Is.False);
            Assert.That(range.Contains(23), Is.False);
            Assert.That(range.Contains(24), Is.False);
            Assert.That(range.Contains(25), Is.True);
            Assert.That(range.Contains(26), Is.False);
            Assert.That(range.Contains(27), Is.True);
            Assert.That(range.Contains(28), Is.True);
            Assert.That(range.Contains(29), Is.True);
            Assert.That(range.Contains(30), Is.True);
            Assert.That(range.Contains(31), Is.False);
            Assert.That(range.Contains(32), Is.False);
            Assert.That(range.Contains(33), Is.True);
            Assert.That(range.Contains(34), Is.True);
            Assert.That(range.Contains(35), Is.True);
            Assert.That(range.Contains(36), Is.True);
            Assert.That(range.Contains(37), Is.True);
            Assert.That(range.Contains(38), Is.True);
            Assert.That(range.Contains(39), Is.True);
            Assert.That(range.Contains(40), Is.True);
            Assert.That(range.Contains(41), Is.True);
            Assert.That(range.Contains(42), Is.True);
            Assert.That(range.Contains(43), Is.True);
            Assert.That(range.Contains(44), Is.True);
            Assert.That(range.Contains(45), Is.True);
            Assert.That(range.Contains(46), Is.True);
            Assert.That(range.Contains(47), Is.True);
            Assert.That(range.Contains(48), Is.True);
            Assert.That(range.Contains(49), Is.True);
            Assert.That(range.Contains(50), Is.True);
            Assert.That(range.Contains(51), Is.True);
            Assert.That(range.Contains(52), Is.True);
            Assert.That(range.Contains(53), Is.True);
            Assert.That(range.Contains(54), Is.False);
            Assert.That(range.Contains(55), Is.False);
            Assert.That(range.Contains(56), Is.False);
            Assert.That(range.Contains(57), Is.False);
            Assert.That(range.Contains(58), Is.False);
            Assert.That(range.Contains(59), Is.False);
            Assert.That(range.Contains(60), Is.False);
            Assert.That(range.Contains(long.MaxValue), Is.False);
        }

        [Test]
        public void Contains02()
        {
            var range = new NumericRange();

            Assert.That(range.Contains(0), Is.False);
            range.Add(0);
            Assert.That(range.Contains(0), Is.True);

            range = new NumericRange();
            Assert.That(range.Contains(ulong.MaxValue), Is.False);
            range.Add(ulong.MaxValue);
            Assert.That(range.Contains(ulong.MaxValue), Is.True);

            range = new NumericRange();
            range.Add(ulong.MinValue);
            range.Add(100);
            range.Add(ulong.MaxValue);

            Assert.That(range.Contains(ulong.MinValue));
            Assert.That(range.Contains(100));
            Assert.That(range.Contains(ulong.MaxValue));
        }

        [Test]
        public void Remove01()
        {
            NumericRange range;

            // single value
            range = NumericRange.Parse("10,100,500-5000");
            range.Remove(100);
            Assert.That(range.ToString(), Is.EqualTo("10,500-5000"));

            range = NumericRange.Parse("10,100,500-5000");
            range.Remove(99);
            Assert.That(range.ToString(), Is.EqualTo("10,100,500-5000"));

            range = NumericRange.Parse("10,100,500-5000");
            range.Remove(101);
            Assert.That(range.ToString(), Is.EqualTo("10,100,500-5000"));


            //for (ulong startValue = 99; startValue <= 101; startValue++)
            //for (ulong endValue = 200; endValue <= 201; endValue++)
            //{
            //    range = NumericRange.Parse("10,100,200,500-5000");
            //    range.Remove(startValue, endValue);
            //    Assert.That(range.ToString(), Is.EqualTo("10,500-5000"));
            //}

            //for (ulong startValue = 99; startValue <= 101; startValue++)
            //{
            //    range = NumericRange.Parse("10,100,200,500-5000");
            //    range.Remove(startValue, 199);
            //    Assert.That(range.ToString(), Is.EqualTo("10,500-5000"));
            //}


            range = NumericRange.Parse("10,100,200,500-5000");
            range.Remove(99, 199);
            Assert.That(range.ToString(), Is.EqualTo("10,200,500-5000"));

            range = NumericRange.Parse("10,100,200,500-5000");
            range.Remove(99, 200);
            Assert.That(range.ToString(), Is.EqualTo("10,500-5000"));

            range = NumericRange.Parse("10,100,200,500-5000");
            range.Remove(99, 201);
            Assert.That(range.ToString(), Is.EqualTo("10,500-5000"));

            range = NumericRange.Parse("10,100,200,500-5000");
            range.Remove(100, 199);
            Assert.That(range.ToString(), Is.EqualTo("10,200,500-5000"));

            range = NumericRange.Parse("10,100,200,500-5000");
            range.Remove(100, 200);
            Assert.That(range.ToString(), Is.EqualTo("10,500-5000"));

            range = NumericRange.Parse("10,100,200,500-5000");
            range.Remove(100, 201);
            Assert.That(range.ToString(), Is.EqualTo("10,500-5000"));

            range = NumericRange.Parse("10,100,200,500-5000");
            range.Remove(101, 199);
            Assert.That(range.ToString(), Is.EqualTo("10,100,200,500-5000"));

            range = NumericRange.Parse("10,100,200,500-5000");
            range.Remove(101, 200);
            Assert.That(range.ToString(), Is.EqualTo("10,100,500-5000"));

            range = NumericRange.Parse("10,100,200,500-5000");
            range.Remove(101, 201);
            Assert.That(range.ToString(), Is.EqualTo("10,100,500-5000"));


            range = NumericRange.Parse("100-200,500-5000");
            range.Remove(0, 300);
            Assert.That(range.ToString(), Is.EqualTo("500-5000"));

            range = NumericRange.Parse("10,100-200,500-5000");
            range.Remove(100, 300);
            Assert.That(range.ToString(), Is.EqualTo("10,500-5000"));

            range = NumericRange.Parse("100-200,500-5000");
            range.Remove(0, 200);
            Assert.That(range.ToString(), Is.EqualTo("500-5000"));

            range = NumericRange.Parse("10,100-200,500-5000");
            range.Remove(150);
            Assert.That(range.ToString(), Is.EqualTo("10,100-149,151-200,500-5000"));

        }
    }
}