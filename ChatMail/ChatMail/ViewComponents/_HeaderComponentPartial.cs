using Microsoft.AspNetCore.Mvc;

namespace ChatMail.ViewComponents
{
    public class _HeaderComponentPartial:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
