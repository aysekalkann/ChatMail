using Microsoft.AspNetCore.Mvc;

namespace ChatMail.ViewComponents
{
    public class _ScriptComponentPartial:ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View();
        }
    }
}
