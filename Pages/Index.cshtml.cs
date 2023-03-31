using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using przestępny.Forms;
using przestępny.Models;

namespace przestępny.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;

        [BindProperty]
        public YearForm Form { set; get; }

        public List<Model> people = new();

        public string Result = "";
        

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

       
        public void OnGet()
        {
            
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            else
            {
                bool prz = czyPrzestepny();
                Result = Verbose(prz);
                var Data = HttpContext.Session.GetString("Data");
                if (Data != null)
                    people =
                    JsonConvert.DeserializeObject<List<Model>>(Data);
                people.Add(recordCreation(prz));
                HttpContext.Session.SetString("Data",
                JsonConvert.SerializeObject(people));
                return Page();
            }  
        }

        public Model recordCreation(bool p)
        {
            Model record = new Model
            {
                Name = Form.Name,
                Year = Form.Year,
                Przestepny = p
            };

            return record;

        }

        public bool czyPrzestepny()
        {
            //1 gdy przestepny, 0 gdy nie
            return ((Form.Year % 4 == 0 && Form.Year % 100 != 0) || Form.Year % 400 == 0);
        }


        public String Verbose(bool p)
        {
            Result += Form.Name;
            Result += " urodził się w ";
            Result += Form.Year;
            Result += " roku. ";

            if (p)
            {
                Result += "To był rok przestępny.";
            }
            else
            {
                Result += "To nie był rok przestępny.";
            }
            
            return Result;
        }

    }
}
