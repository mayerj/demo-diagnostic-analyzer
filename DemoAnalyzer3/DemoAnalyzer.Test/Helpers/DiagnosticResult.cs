using Microsoft.CodeAnalysis;
using System;
using System.Text.RegularExpressions;

namespace DemoAnalyzer.Test
{
	/// <summary>
	/// Location where the diagnostic appears, as determined by path, line number, and column number.
	/// </summary>
	public struct DiagnosticResultLocation
	{
		public DiagnosticResultLocation(int? line) : this(null, line, null)
		{
		}

		public DiagnosticResultLocation(string path, int? line, int? column)
		{
			if (line.HasValue && line < -1)
			{
				throw new ArgumentOutOfRangeException(nameof(line), "line must be >= -1");
			}

			if (column.HasValue && column < -1)
			{
				throw new ArgumentOutOfRangeException(nameof(column), "column must be >= -1");
			}

			Path = path;
			Line = line;
			Column = column;
		}

		public string Path { get; }
		public int? Line { get; }
		public int? Column { get; }
	}

	/// <summary>
	/// Struct that stores information about a Diagnostic appearing in a source
	/// </summary>
	public struct DiagnosticResult
	{
		private DiagnosticResultLocation[] locations;

		public DiagnosticResultLocation[] Locations
		{
			get
			{
				if (locations == null)
				{
					locations = new DiagnosticResultLocation[] { };
				}
				return locations;
			}

			set
			{
				locations = value;
			}
		}

		public DiagnosticResultLocation Location
		{
			get { return Locations[0]; }
			set
			{
				Locations = new[] { value };
			}
		}

		public DiagnosticSeverity Severity { get; set; }

		public string Id { get; set; }

		public Regex Message { get; set; }

		public string Path
		{
			get
			{
				return Locations.Length > 0 ? Locations[0].Path : "";
			}
		}

		public int? Line
		{
			get
			{
				return Locations.Length > 0 ? Locations[0].Line : -1;
			}
		}

		public int? Column
		{
			get
			{
				return Locations.Length > 0 ? Locations[0].Column : -1;
			}
		}
	}
}
