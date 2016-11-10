﻿using DotnetSpider.Core.Processor;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xunit;

namespace DotnetSpider.Core.Test.Processor
{
	public class TargetUrlTests
	{
		public class CnblogsProcessor1 : BasePageProcessor
		{
			public CnblogsProcessor1()
			{
				TargetUrlRegions = new HashSet<Selector.ISelector>
				{
					Selector.Selectors.XPath(".//div[@class='pager']")
				};
				TargetUrlPatterns = new HashSet<Regex>
				{
					new Regex("/sitehome/p/\\d+"),new Regex("^http://www\\.cnblogs\\.com/$")
				};
			}

			protected override void Handle(Page page)
			{
				page.ResultItems.AddOrUpdateResultItem("test", true);
			}
		}

		[Fact]
		public void UrlVerifyAndExtract1()
		{
			HttpClient client = new HttpClient();
			var html = client.GetStringAsync("http://www.cnblogs.com").Result;

			Page page = new Page(new Request("http://www.cnblogs.com/", 0, null), ContentType.Html, null);
			page.Content = html;

			CnblogsProcessor1 processor = new CnblogsProcessor1();
			processor.Process(page);

			Assert.True(page.ResultItems.GetResultItem("test"));
			Assert.Equal(12, page.TargetRequests.Count);
			Assert.Equal("http://www.cnblogs.com/", page.TargetRequests.ElementAt(11).Url.ToString());
		}

		public class CnblogsProcessor2 : BasePageProcessor
		{
			public CnblogsProcessor2()
			{
				TargetUrlPatterns = new HashSet<Regex>
				{
					new Regex("/sitehome/p/\\d+"),new Regex("^http://www\\.cnblogs\\.com/$")
				};
			}

			protected override void Handle(Page page)
			{
				page.ResultItems.AddOrUpdateResultItem("test", true);
			}
		}

		[Fact]
		public void UrlVerifyAndExtract2()
		{
			HttpClient client = new HttpClient();
			var html = client.GetStringAsync("http://www.cnblogs.com").Result;

			Page page = new Page(new Request("http://www.cnblogs.com/", 0, null), ContentType.Html, null);
			page.Content = html;

			CnblogsProcessor2 processor = new CnblogsProcessor2();
			processor.Process(page);

			Assert.True(page.ResultItems.GetResultItem("test"));
			Assert.Equal(12, page.TargetRequests.Count);
			Assert.Equal("http://www.cnblogs.com/", page.TargetRequests.ElementAt(11).Url.ToString());
		}

		public class CnblogsProcessor3 : BasePageProcessor
		{
			public CnblogsProcessor3()
			{
				TargetUrlPatterns = new HashSet<Regex>
				{
					new Regex("/sitehome/p/\\d+")
				};
			}

			protected override void Handle(Page page)
			{
				page.ResultItems.AddOrUpdateResultItem("test", true);
			}
		}

		[Fact]
		public void UrlVerifyAndExtract3()
		{
			HttpClient client = new HttpClient();
			var html = client.GetStringAsync("http://www.cnblogs.com").Result;

			Page page = new Page(new Request("http://www.cnblogs.com/", 0, null), ContentType.Html, null);
			page.Content = html;

			CnblogsProcessor3 processor = new CnblogsProcessor3();
			processor.Process(page);

			Assert.Null(page.ResultItems.GetResultItem("test"));
			Assert.Equal(0, page.TargetRequests.Count);
		}

		public class CnblogsProcessor4 : BasePageProcessor
		{
			public CnblogsProcessor4()
			{
				TargetUrlPatterns = new HashSet<Regex>
				{
					new Regex("/sitehome/p/\\d+")
				};
			}

			protected override void Handle(Page page)
			{
				page.ResultItems.AddOrUpdateResultItem("test", true);
			}
		}

		[Fact]
		public void UrlVerifyAndExtract4()
		{
			HttpClient client = new HttpClient();
			var html = client.GetStringAsync("http://www.cnblogs.com/sitehome/p/2/").Result;

			Page page = new Page(new Request("http://www.cnblogs.com/sitehome/p/2/", 0, null), ContentType.Html, null);
			page.Content = html;

			CnblogsProcessor4 processor = new CnblogsProcessor4();
			processor.Process(page);

			Assert.True(page.ResultItems.GetResultItem("test"));
			Assert.Equal(12, page.TargetRequests.Count);
			Assert.Equal("http://www.cnblogs.com/sitehome/p/2/#", page.TargetRequests.ElementAt(0).Url.ToString());
		}
	}
}
