using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace RegionOrebroLan.DirectoryServices.Protocols.UnitTests
{
	[TestClass]
	public class FilterBuilderTest
	{
		#region Methods

		[TestMethod]
		public void Build_IfThereAreManyFilterAndTheOperatorIsAnd_ShouldWorkCorrectly()
		{
			var expectedValue = "(&(attribute=value)(attribute=value)(attribute=value))";
			Assert.AreEqual(expectedValue, new FilterBuilder("attribute=value", "(attribute=value)", "attribute=value") {Operator = FilterOperator.And}.Build());
			Assert.AreEqual(expectedValue, new FilterBuilder("(attribute=value)", "attribute=value", "(attribute=value)") {Operator = FilterOperator.And}.Build());
			Assert.AreEqual(expectedValue, new FilterBuilder(string.Empty, "attribute=value", null, "  ", "(attribute=value)", null, "attribute=value") {Operator = FilterOperator.And}.Build());

			expectedValue = "(&( _ )( _ )( _ ))";
			Assert.AreEqual(expectedValue, new FilterBuilder(" _ ", "( _ )", " _ ") {Operator = FilterOperator.And}.Build());
			Assert.AreEqual(expectedValue, new FilterBuilder("( _ )", " _ ", "( _ )") {Operator = FilterOperator.And}.Build());
			Assert.AreEqual(expectedValue, new FilterBuilder(string.Empty, " _ ", null, "  ", "( _ )", null, " _ ") {Operator = FilterOperator.And}.Build());
		}

		[TestMethod]
		public void Build_IfThereAreManyFilterAndTheOperatorIsNot_ShouldWorkCorrectly()
		{
			var expectedValue = "(!(attribute=value)(attribute=value)(attribute=value))";
			Assert.AreEqual(expectedValue, new FilterBuilder("attribute=value", "(attribute=value)", "attribute=value") {Operator = FilterOperator.Not}.Build());
			Assert.AreEqual(expectedValue, new FilterBuilder("(attribute=value)", "attribute=value", "(attribute=value)") {Operator = FilterOperator.Not}.Build());
			Assert.AreEqual(expectedValue, new FilterBuilder(string.Empty, "attribute=value", null, "  ", "(attribute=value)", null, "attribute=value") {Operator = FilterOperator.Not}.Build());

			expectedValue = "(!( _ )( _ )( _ ))";
			Assert.AreEqual(expectedValue, new FilterBuilder(" _ ", "( _ )", " _ ") {Operator = FilterOperator.Not}.Build());
			Assert.AreEqual(expectedValue, new FilterBuilder("( _ )", " _ ", "( _ )") {Operator = FilterOperator.Not}.Build());
			Assert.AreEqual(expectedValue, new FilterBuilder(string.Empty, " _ ", null, "  ", "( _ )", null, " _ ") {Operator = FilterOperator.Not}.Build());
		}

		[TestMethod]
		public void Build_IfThereAreManyFilterAndTheOperatorIsOr_ShouldWorkCorrectly()
		{
			var expectedValue = "(|(attribute=value)(attribute=value)(attribute=value))";
			Assert.AreEqual(expectedValue, new FilterBuilder("attribute=value", "(attribute=value)", "attribute=value") {Operator = FilterOperator.Or}.Build());
			Assert.AreEqual(expectedValue, new FilterBuilder("(attribute=value)", "attribute=value", "(attribute=value)") {Operator = FilterOperator.Or}.Build());
			Assert.AreEqual(expectedValue, new FilterBuilder(string.Empty, "attribute=value", null, "  ", "(attribute=value)", null, "attribute=value") {Operator = FilterOperator.Or}.Build());

			expectedValue = "(|( _ )( _ )( _ ))";
			Assert.AreEqual(expectedValue, new FilterBuilder(" _ ", "( _ )", " _ ") {Operator = FilterOperator.Or}.Build());
			Assert.AreEqual(expectedValue, new FilterBuilder("( _ )", " _ ", "( _ )") {Operator = FilterOperator.Or}.Build());
			Assert.AreEqual(expectedValue, new FilterBuilder(string.Empty, " _ ", null, "  ", "( _ )", null, " _ ") {Operator = FilterOperator.Or}.Build());
		}

		[TestMethod]
		public void Build_IfThereAreNoFilters_ShouldReturnNull()
		{
			var filterBuilder = new FilterBuilder();
			Assert.IsFalse(filterBuilder.Filters.Any());
			Assert.IsNull(filterBuilder.Build());
		}

		[TestMethod]
		public void Build_IfThereAreOnlyEmptyFilters_ShouldReturnNull()
		{
			Assert.IsNull(new FilterBuilder(null, string.Empty, " ", "  ") {Operator = FilterOperator.And}.Build());
			Assert.IsNull(new FilterBuilder(null, string.Empty, " ", "  ") {Operator = FilterOperator.Not}.Build());
			Assert.IsNull(new FilterBuilder(null, string.Empty, " ", "  ") {Operator = FilterOperator.Or}.Build());
		}

		[TestMethod]
		public void Build_IfThereIsOneFilterAndTheOperatorIsAnd_ShouldWorkCorrectly()
		{
			var expectedValue = "(&(attribute=value))";
			Assert.AreEqual(expectedValue, new FilterBuilder("attribute=value") {Operator = FilterOperator.And}.Build());
			Assert.AreEqual(expectedValue, new FilterBuilder("(attribute=value)") {Operator = FilterOperator.And}.Build());
			Assert.AreEqual(expectedValue, new FilterBuilder(string.Empty, "attribute=value", null, "  ") {Operator = FilterOperator.And}.Build());
			Assert.AreEqual(expectedValue, new FilterBuilder(string.Empty, "(attribute=value)", null, "  ") {Operator = FilterOperator.And}.Build());

			expectedValue = "(&( _ ))";
			Assert.AreEqual(expectedValue, new FilterBuilder(" _ ") {Operator = FilterOperator.And}.Build());
			Assert.AreEqual(expectedValue, new FilterBuilder("( _ )") {Operator = FilterOperator.And}.Build());
			Assert.AreEqual(expectedValue, new FilterBuilder(string.Empty, " _ ", null, "  ") {Operator = FilterOperator.And}.Build());
			Assert.AreEqual(expectedValue, new FilterBuilder(string.Empty, "( _ )", null, "  ") {Operator = FilterOperator.And}.Build());
		}

		[TestMethod]
		public void Build_IfThereIsOneFilterAndTheOperatorIsNot_ShouldWorkCorrectly()
		{
			var expectedValue = "(!(attribute=value))";
			Assert.AreEqual(expectedValue, new FilterBuilder("attribute=value") {Operator = FilterOperator.Not}.Build());
			Assert.AreEqual(expectedValue, new FilterBuilder("(attribute=value)") {Operator = FilterOperator.Not}.Build());
			Assert.AreEqual(expectedValue, new FilterBuilder(string.Empty, "attribute=value", null, "  ") {Operator = FilterOperator.Not}.Build());
			Assert.AreEqual(expectedValue, new FilterBuilder(string.Empty, "(attribute=value)", null, "  ") {Operator = FilterOperator.Not}.Build());

			expectedValue = "(!( _ ))";
			Assert.AreEqual(expectedValue, new FilterBuilder(" _ ") {Operator = FilterOperator.Not}.Build());
			Assert.AreEqual(expectedValue, new FilterBuilder("( _ )") {Operator = FilterOperator.Not}.Build());
			Assert.AreEqual(expectedValue, new FilterBuilder(string.Empty, " _ ", null, "  ") {Operator = FilterOperator.Not}.Build());
			Assert.AreEqual(expectedValue, new FilterBuilder(string.Empty, "( _ )", null, "  ") {Operator = FilterOperator.Not}.Build());
		}

		[TestMethod]
		public void Build_IfThereIsOneFilterAndTheOperatorIsOr_ShouldWorkCorrectly()
		{
			var expectedValue = "(|(attribute=value))";
			Assert.AreEqual(expectedValue, new FilterBuilder("attribute=value") {Operator = FilterOperator.Or}.Build());
			Assert.AreEqual(expectedValue, new FilterBuilder("(attribute=value)") {Operator = FilterOperator.Or}.Build());
			Assert.AreEqual(expectedValue, new FilterBuilder(string.Empty, "attribute=value", null, "  ") {Operator = FilterOperator.Or}.Build());
			Assert.AreEqual(expectedValue, new FilterBuilder(string.Empty, "(attribute=value)", null, "  ") {Operator = FilterOperator.Or}.Build());

			expectedValue = "(|( _ ))";
			Assert.AreEqual(expectedValue, new FilterBuilder(" _ ") {Operator = FilterOperator.Or}.Build());
			Assert.AreEqual(expectedValue, new FilterBuilder("( _ )") {Operator = FilterOperator.Or}.Build());
			Assert.AreEqual(expectedValue, new FilterBuilder(string.Empty, " _ ", null, "  ") {Operator = FilterOperator.Or}.Build());
			Assert.AreEqual(expectedValue, new FilterBuilder(string.Empty, "( _ )", null, "  ") {Operator = FilterOperator.Or}.Build());
		}

		[TestMethod]
		public void Operator_ShouldReturnAndByDefault()
		{
			Assert.AreEqual(FilterOperator.And, new FilterBuilder().Operator);
		}

		#endregion
	}
}