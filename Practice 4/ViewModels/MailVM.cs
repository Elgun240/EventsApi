using Microsoft.CodeAnalysis.Operations;
using Practice_4.Models;

namespace Practice_4.ViewModels
{
    public class MailVM
    {
        public string AppUserId  { get; set; }
        public string To { get; set; }
        public string Email { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public Comment Comment { get; set; }
    }
}
