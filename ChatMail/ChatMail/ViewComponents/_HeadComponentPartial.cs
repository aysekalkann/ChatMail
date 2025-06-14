using Microsoft.AspNetCore.Mvc;

namespace ChatMail.ViewComponents
{
    public class _HeadComponentPartial:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
