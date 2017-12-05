using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Markdig;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;

namespace FMLMiddleware
{
	public class FileContentRepository : IContentRepository
	{
		internal string[] ValidExtensions = { ".md", ".html" };
		public string DefaultLayout { get; set; } = "_layouts/Default.html";
		public string BasePath { get; set; } = "";

		public object GetContentForPath(HttpContext context)
		{
			var pipeline = new MarkdownPipelineBuilder()
				.UseAdvancedExtensions()
				.Build();

			var request = context.Request.Path.Value.Trim(new[] { '/' });
			if (string.IsNullOrWhiteSpace(request))
				request = "index.md";

			PhysicalFileProvider pfp = new PhysicalFileProvider(Directory.GetCurrentDirectory());
			var dirContents = pfp.GetDirectoryContents(BasePath);

			StringBuilder sb = new StringBuilder();

			foreach (var file in dirContents)
			{
				if (ValidExtensions.Any(ext => file.Name.EndsWith(ext, StringComparison.InvariantCultureIgnoreCase)))
				{
					if (request == file.Name)
					{

						var fileContents = Markdown.ToHtml(File.ReadAllText(file.PhysicalPath), pipeline);

						sb.AppendLine(fileContents);
					}
				}
			}

			return sb.ToString();
		}
	}
}
