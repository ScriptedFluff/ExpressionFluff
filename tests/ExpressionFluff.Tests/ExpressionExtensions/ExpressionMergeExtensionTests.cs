namespace ExpressionFluff.Tests.ExpressionExtensions
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using ExpressionFluff.ExpressionExtensions;

    [TestClass]
    public class ExpressionMergeExtensionTests
    {
        [TestMethod]
        public void Can_Merge_Two_Bool_Expressions_Without_Classes()
        {
            var items = new[] { 1, 2, 3 };
            Expression<Func<int, bool>> exp = o => o > 1;

            var result1 = items.Where(exp.Compile());
            Assert.AreEqual(2, result1.Count());

            // Using a different alias 'l' to ensure different alias don't matter
            var result2 = items.Where(exp.Merge(l => l < 3).Compile());
            Assert.AreEqual(1, result2.Count());
        }

        private class Value
        {
            public int IntValue { get; set; }
        }

        private class ClassReference
        {
            public Value Reference { get; set; }
        }

        private class ClassList
        {
            public ClassReference[] References { get; set; }
        }

        [TestMethod]
        public void Can_Merge_Two_Bool_Expressions_With_Multi_Level_ClassList()
        {
            var items = (new[] { 1, 2, 3 }).Select(o => new ClassList
            {
                References = Enumerable
                    .Range(0, o)
                    .Select(num => new ClassReference { Reference = new Value { IntValue = num } })
                    .ToArray()
            });
            Expression<Func<ClassList, bool>> exp = o => o.References.Count() > 1;

            var result1 = items.Where(exp.Compile());
            Assert.AreEqual(2, result1.Count());

            // Using a different alias 'l' to ensure different alias don't matter
            var result2 = items.Where(exp.Merge(l => l.References.Count() < 3).Compile());
            Assert.AreEqual(1, result2.Count());
        }

        [TestMethod]
        public void Can_Merge_Two_Bool_Expressions_With_Multi_Level_Class()
        {
            var items = (new[] { 1, 2, 3 }).Select(o => new ClassReference { Reference = new Value { IntValue = o } });
            Expression<Func<ClassReference, bool>> exp = o => o.Reference.IntValue > 1;

            var result1 = items.Where(exp.Compile());
            Assert.AreEqual(2, result1.Count());

            // Using a different alias 'l' to ensure different alias don't matter
            var result2 = items.Where(exp.Merge(l => l.Reference.IntValue < 3).Compile());
            Assert.AreEqual(1, result2.Count());
        }

        [TestMethod]
        public void Can_Merge_Two_Bool_Expressions_With_Single_Level_Class()
        {
            var items = (new[] { 1, 2, 3 }).Select(o => new Value { IntValue = o });
            Expression<Func<Value, bool>> exp = o => o.IntValue > 1;

            var result1 = items.Where(exp.Compile());
            Assert.AreEqual(2, result1.Count());

            // Using a different alias 'l' to ensure different alias don't matter
            var result2 = items.Where(exp.Merge(l => l.IntValue < 3).Compile());
            Assert.AreEqual(1, result2.Count());
        }
    }
}
