using Microsoft.AspNetCore.Razor.TagHelpers;

namespace FreelanceProject.TagHelpers
{
    [HtmlTargetElement("new-project-button")]
    public class NewProjectButtonTagHelper : TagHelper
    {
        // İsterseniz buton yazısını dışarıdan parametre olarak alabilirsiniz
        public string Text { get; set; } = "New Project";

        // Butonun yönlendireceği url (default: /Jobs/Create)
        public string Url { get; set; } = "/Jobs/Create";

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "button";    // <button> elementi olarak render et
            output.Attributes.SetAttribute("onclick", $"location.href='{Url}'");
            output.Attributes.SetAttribute("class", "btn btn-premium ms-3 d-none d-lg-block");
            output.Content.SetContent(Text);
        }
    }
}
