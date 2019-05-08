#region Header
// © 2019 Koninklijke Philips N.V.  All rights reserved.
// Reproduction or transmission in whole or in part, in any form or by any means, 
// electronic, mechanical or otherwise, is prohibited without the prior  written consent of 
// the owner.
#endregion

using System;
using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Diagnostics;

namespace DemoAnalyzer
{
	[DiagnosticAnalyzer(LanguageNames.CSharp)]
	public class AvoidTryParseWithoutCultureAnalyzer : DiagnosticAnalyzer
	{
		public const string DiagnosticId = @"DA0001";
		private const string Title = @"Do not use TryParse without specifying a culture";
		private const string MessageFormat = @"Do not use TryParse without specifying a culture if such an overload exists.";
		private const string Description = @"Do not use TryParse without specifying a culture if such an overload exists. Failure to do so may result in code not correctly handling localized delimiters (such as commas instead of decimal points).";
		private const string Category = @"Maintainability";

		private static DiagnosticDescriptor Rule = new DiagnosticDescriptor(DiagnosticId, Title, MessageFormat, Category, DiagnosticSeverity.Error, isEnabledByDefault: true, description: Description);

		public override ImmutableArray<DiagnosticDescriptor> SupportedDiagnostics => ImmutableArray.Create(Rule);

		public override void Initialize(AnalysisContext context)
		{
			context.RegisterSyntaxNodeAction(Analyze, SyntaxKind.InvocationExpression);
		}

		private void Analyze(SyntaxNodeAnalysisContext context)
		{
			InvocationExpressionSyntax invocationExpressionSyntax = (InvocationExpressionSyntax)context.Node;
		}
	}
}
