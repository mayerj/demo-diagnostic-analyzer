#region Header
// © 2019 Koninklijke Philips N.V.  All rights reserved.
// Reproduction or transmission in whole or in part, in any form or by any means, 
// electronic, mechanical or otherwise, is prohibited without the prior  written consent of 
// the owner.
#endregion

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Text.RegularExpressions;

namespace DemoAnalyzer.Test
{
	[TestClass]
	public class AvoidTryParseWithoutCultureAnalyzerTest : DiagnosticVerifier
	{
		private const string ClassString = @"
			using System;
			using System.Globalization;
			class Foo 
			{{
				public void Foo()
				{{
					{0}
				}}
			}}
			";

		private const string TestParserDefinition = @"
			class TestParser
			{{
				public static bool TryParse(string s, out TestParser tp)
				{{
					tp = new TestParser();
					return true;
				}}

				public static bool TryParse(string s, NumberStyles numberStyle, IFormatProvider format, out TestParser tp)
				{{
					tp = new TestParser();
					return true;
				}}

				public static bool TryParse(string s, CultureInfo culture, out TestParser tp)
				{{
					tp = new TestParser();
					return true;
				}}

				public static void TryParse() {{}}
			}}";
		protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
		{
			return new AvoidTryParseWithoutCultureAnalyzer();
		}

		[DataTestMethod]
		[DataRow("int.TryParse(\"3\", out int i);")]
		[DataRow("float.TryParse(\"3.00\", out float i);")]
		[DataRow("double.TryParse(\"3.00\", out double i);")]
		public void AvoidTryParseWithoutCultureForValueTypes(string s)
		{
			string code = string.Format(ClassString, s);
			DiagnosticResult expected = new DiagnosticResult
			{
				Id = AvoidTryParseWithoutCultureAnalyzer.DiagnosticId,
				Message = new Regex(".+ "),
				Severity = DiagnosticSeverity.Error,
				Location = new DiagnosticResultLocation("Test0.cs", null, null)
			};

			VerifyCSharpDiagnostic(code, expected);
		}

		[DataTestMethod]
		[DataRow("int.TryParse(\"3\", NumberStyles.Any, CultureInfo.InvariantCulture, out int i);")]
		[DataRow("float.TryParse(\"3.00\", NumberStyles.Any, CultureInfo.InvariantCulture, out float i);")]
		[DataRow("double.TryParse(\"3.00\", NumberStyles.Any, CultureInfo.InvariantCulture, out double i);")]
		public void DoNotFlagTryParseWithCultureForValueTypes(string s)
		{
			string code = string.Format(ClassString, s);
			VerifyCSharpDiagnostic(code);
		}

		[DataTestMethod]
		[DataRow("TestParser.TryParse(\"3\", out TestParser tp);")]
		[DataRow("TestParser.TryParse();")]
		public void AvoidTryParseWithoutCultureForReferenceTypes(string s)
		{
			string editorCode = string.Format(ClassString, s);
			string code = string.Concat(editorCode, TestParserDefinition);
			DiagnosticResult expected = new DiagnosticResult
			{
				Id = AvoidTryParseWithoutCultureAnalyzer.DiagnosticId,
				Message = new Regex(".+ "),
				Severity = DiagnosticSeverity.Error,
				Location = new DiagnosticResultLocation("Test0.cs", null, null)
			};

			VerifyCSharpDiagnostic(code, expected);
		}

		[DataTestMethod]
		[DataRow("TestParser.TryParse(\"3\", NumberStyles.Any, CultureInfo.InvariantCulture, out TestParser tp);")]
		[DataRow("TestParser.TryParse(\"3\", CultureInfo.InvariantCulture, out TestParser tp);")]
		public void DoNotFlagTryParseWithCultureForReferenceTypes(string s)
		{
			string editorCode = string.Format(ClassString, s);
			string code = string.Concat(editorCode, TestParserDefinition);
			VerifyCSharpDiagnostic(code);
		}
	}
}
