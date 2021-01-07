using System.Collections.Generic;

namespace Trill.Web.Core.Stories.Requests
{
    public class SendStoryRequest
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public IEnumerable<string> Tags { get; set; }
    }
}