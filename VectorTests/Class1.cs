using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using NUnit.Framework;

namespace VectorTests
{
    public class Class1
    {
        [Test]
        public void RoundDigitsTest()
        {
            string s = "M152.57253,0 C171.93097,18.785866 188.64937,39.812946 202.07002,63.639595 L202.0701,96.703814 202.28441,97.289382 C202.5766,98.228772 202.73399,99.227543 202.73399,100.26308 L202.73399,119.22508 C202.73399,124.74792 198.25685,129.22507 192.73399,129.22507 L192.14484,129.22507 191.46355,130.81475 191.46359,144.95671 202.07019,155.56331 202.07022,169.70542 187.92805,183.84758 C187.92805,183.84758 170.44398,186.37769 159.64383,183.84753 155.23122,182.81378 154.62386,178.82755 152.57283,176.77653 152.57283,176.77653 152.57281,155.56339 152.57281,155.56339 L163.17938,144.95683 163.17937,130.81474 162.91249,129.22507 10,129.22507 C4.4771523,129.22507 0,124.74792 0,119.22508 L0,100.26308 C0,94.740229 4.4771523,90.263077 10,90.263076 L124.12514,90.263076 124.12514,129.03909 144.12514,129.03909 144.12514,90.263076 156.73999,90.263076 157.68208,89.739293 C160.55649,87.970379 162.67021,85.444562 164.24011,82.377629 172.84334,66.702972 169.05151,17.814789 152.57253,0 z";

            var regex = new Regex(@"(-?\d+(?:\.\d+)?(?:E-?\d*)?)");
            //var matches = regex.Matches(s);
            //for (int i = 0; i < matches.Count; i++)
            //{
            //    Console.WriteLine("{0} : {1}",i,matches[i].Value);
            //}

            string replace = regex.Replace(s, m => double.Parse(m.Value,CultureInfo.InvariantCulture).ToString("0"));
            Console.WriteLine(replace);
        }
    }
}
