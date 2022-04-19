using Microsoft.AspNetCore.Mvc;
using OnlineAds.Models;
using System.Linq;
using Microsoft.AspNetCore.Session;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using PagedList.Mvc;
using PagedList;
using cloudscribe.Pagination.Models;


namespace OnlineAds.Controllers
{
    public class AdminController : Controller
    {
        // image upload
        private readonly IWebHostEnvironment _webHostEnvironment;

        public AdminController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        // above is for image upload
        OnlineAdsDBContext db = new OnlineAdsDBContext();

        [HttpGet]
        public IActionResult login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult login(TblAdmin avm)
        {
            TblAdmin ad = db.TblAdmins.Where(x => x.AdUsername == avm.AdUsername && x.AdPassword == avm.AdPassword).SingleOrDefault();
            if(ad != null)
            {
                HttpContext.Session.SetString("ad_id", ad.AdId.ToString());
                return RedirectToAction("Create");
            }
            else
            {
                ViewBag.error = "Invalid username or password";
            }
            return View();
        }
        public IActionResult Create()
        {
            if(HttpContext.Session.GetString("ad_id") == null)
            {
                return RedirectToAction("login");
            }
            return View();
        }

        [HttpPost]
        public IActionResult Create(TblCategory cvm, IFormFile imgfile)
        {
            string path = uploadingfile(imgfile);
            if(path.Equals("-1"))
            {
                ViewBag.error = "Image could not be uploaded....";
            }
            else
            {
                ViewBag.sessionv = HttpContext.Session.GetString("ad_id");
                TblCategory cat = new TblCategory();
                cat.CatName = cvm.CatName;
                cat.CatImage = path;
                cat.CatStatus = 1;
                cat.CatFkAd = Convert.ToInt32(HttpContext.Session.GetString("ad_id"));
                db.TblCategories.Add(cat);
                db.SaveChanges();
                return RedirectToAction("Create");
            }

            return View();
        }



        //public ActionResult ViewCategory(int? page)
        //{
        //    int pagesize = 9, pageindex = 1;
        //    pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
        //    var list = db.TblCategories.Where(x => x.CatStatus == 1).OrderByDescending(x => x.CatId).ToList();
        //    IPagedList<TblCategory> stu = list.ToPagedList(pageindex, pagesize);


        //    return View(stu);

        //}

        public IActionResult ViewCategory(int pageNumber = 1, int pageSize = 2)
        {
            int ExcludeRecords = (pageSize * pageNumber) - pageSize;

            var stu = db.TblCategories.Where(x => x.CatStatus == 1).OrderByDescending(x => x.CatId)
                .Skip(ExcludeRecords)
                .Take(pageSize);

            var result = new PagedResult<TblCategory>
            {
                Data = stu.ToList(),
                TotalItems = db.TblCategories.Count(),
                PageNumber = pageNumber,
                PageSize = pageSize,
            };
            return View(result);
        }


        public ActionResult DeleteCategory(int? id)
        {
            TblCategory p = db.TblCategories.Where(x => x.CatId == id).SingleOrDefault();
            db.TblCategories.Remove(p);
            db.SaveChanges();

            return RedirectToAction("ViewCategory");
        }




        public string uploadingfile(IFormFile file)
        {
            Random r = new Random();
            string path = "-1";
            int random = r.Next();
            if (file != null && file.Length > 0)
            {
                string extension = Path.GetExtension(file.FileName);
                if (extension.ToLower().Equals(".jpg") || extension.ToLower().Equals(".jpeg") || extension.ToLower().Equals(".png"))
                {
                    try
                    {
                        string webRootPath = _webHostEnvironment.WebRootPath;
                        string contentRootPath = _webHostEnvironment.ContentRootPath;

                        path = Path.Combine(webRootPath, "upload");
                        //or path = Path.Combine(contentRootPath , "wwwroot" ,"CSS" );
                        string fileName = Path.GetFileName(file.FileName);
                        
                        using (FileStream stream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                        {
                            file.CopyTo(stream);
                        }
                        
                        path = "~/"+ "upload" + "/" + fileName;
                    }
                    catch (Exception)
                    {
                        path = "-1";
                    }

                }
                else
                {
                    Response.WriteAsync("<script>alert('Only jpg ,jpeg or png formats are acceptable....'); </script>");
                }
            }

            else
            {
                Response.WriteAsync("<script>alert('Please select a file'); </script>");
                path = "-1";
            }
  return path;
        }

    }
}
