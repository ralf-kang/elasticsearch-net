﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

using Nest.Tests.MockData.Domain;

namespace Nest.Tests.Unit.Query
{
	[TestFixture]
	public class RangeQueryJson
	{
		[Test]
		public void RangeStrings()
		{
			var s = new SearchDescriptor<ElasticSearchProject>()
				.From(0)
				.Size(10)
				.Query(ff => ff
					.Range(n => n
						.OnField(f=>f.LOC)
						.From("10")
						.To("20")
						.FromExclusive()
					)
				);
				
			var json = ElasticClient.Serialize(s);
			var expected = @"{ from: 0, size: 10, 
				query : {
						range: {
							""loc.sort"": {
								from: ""10"",
								to: ""20"",
								include_lower: false
							}
						}

					}
			}";
			Assert.True(json.JsonEquals(expected), json);		
		}

		[Test]
		public void RangeInts()
		{
			var s = new SearchDescriptor<ElasticSearchProject>()
				.From(0)
				.Size(10)
				.Query(ff => ff
					.Range(n => n
						.OnField(f => f.LOC)
						.From(10)
						.To(20)
						.FromExclusive()
					)
				);

			var json = ElasticClient.Serialize(s);
			var expected = @"{ from: 0, size: 10, 
				query : {
						range: {
							""loc.sort"": {
								from: 10,
								to: 20,
								include_lower: false
							}
						}

					}
			}";
			Assert.True(json.JsonEquals(expected), json);
		}

		[Test]
		public void RangeDoubles()
		{
			var s = new SearchDescriptor<ElasticSearchProject>()
				.From(0)
				.Size(10)
				.Query(ff => ff
					.Range(n => n
						.OnField(f => f.LOC)
						.From(10.0)
						.To(20.0)
						.FromExclusive()
					)
				);

			var json = ElasticClient.Serialize(s);
			var expected = @"{ from: 0, size: 10, 
				query : {
						range: {
							""loc.sort"": {
								from: 10.0,
								to: 20.0,
								include_lower: false
							}
						}

					}
			}";
			Assert.True(json.JsonEquals(expected), json);
		}
		[Test]
		public void RangeDates()
		{
			var lowerBound = DateTime.UtcNow.AddYears(-1);
			var upperBound = DateTime.UtcNow.AddYears(1);
			var s = new SearchDescriptor<ElasticSearchProject>()
				.From(0)
				.Size(10)
				.Query(ff => ff
					.Range(n => n
						.OnField(f => f.StartedOn)
						.From(lowerBound)
						.To(upperBound)
						.FromExclusive()
					)
				);

			var json = ElasticClient.Serialize(s);
			var expected = @"{ from: 0, size: 10, 
				query : {
						range: {
							""startedOn"": {
								from: """ + lowerBound.ToString("yyyy/MM/dd HH:mm:ss") + @""",
								to: """ + upperBound.ToString("yyyy/MM/dd HH:mm:ss") + @""",
								include_lower: false
							}
						}

					}
			}";
			Assert.True(json.JsonEquals(expected), json + Environment.NewLine + expected);
		}
		[Test]
		public void RangeDatesCustom()
		{
			var format = "yyyy/MM/dd";
			var lowerBound = DateTime.UtcNow.AddYears(-1);
			var upperBound = DateTime.UtcNow.AddYears(1);
			var s = new SearchDescriptor<ElasticSearchProject>()
				.From(0)
				.Size(10)
				.Query(ff => ff
					.Range(n => n
						.OnField(f => f.StartedOn)
						.From(lowerBound, format)
						.To(upperBound, format)
						.FromExclusive()
					)
				);

			var json = ElasticClient.Serialize(s);
			var expected = @"{ from: 0, size: 10, 
				query : {
						range: {
							""startedOn"": {
								from: """ + lowerBound.ToString(format) + @""",
								to: """ + upperBound.ToString(format) + @""",
								include_lower: false
							}
						}

					}
			}";
			Assert.True(json.JsonEquals(expected), json + Environment.NewLine + expected);
		}
	}
}
