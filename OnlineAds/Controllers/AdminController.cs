using Microsoft.AspNetCore.Mvc;
using OnlineAds.Models;
using System.Linq;
using Microsoft.AspNetCore.Http;
using System;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using cloudscribe.Pagination.Models;
using System.Threading.Tasks;
using System.Data.Entity;

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
            Encryption enc = new Encryption();
            string codedPasswrd = enc.Encode(avm.AdPassword);
    
            TblAdmin ad = db.TblAdmins.Where(x => x.AdUsername == avm.AdUsername && x.AdPassword == codedPasswrd).SingleOrDefault();
            if(ad != null)
            {
                HttpContext.Session.SetString("ad_id", ad.AdId.ToString());
                return RedirectToAction("ViewCategory");
            }
            else
            {
                ViewBag.error = "Invalid username or password";
            }
            return View();
        }


        public IActionResult users(int pageNumber = 1, int pageSize = 9)
        {
            if (HttpContext.Session.GetString("ad_id") == null)
            {
                return RedirectToAction("login");
            }

            int ExcludeRecords = (pageSize * pageNumber) - pageSize;

            var stu = db.TblUsers.OrderBy(x => x.UName)
                .Skip(ExcludeRecords)
                .Take(pageSize);

            var result = new PagedResult<TblUser>
            {
                Data = stu.ToList(),
                TotalItems = db.TblUsers.Count(),
                PageNumber = pageNumber,
                PageSize = pageSize,
            };

            return View(result);
        }


        [HttpPost]
        public IActionResult users(string userSearch, int pageNumber = 1, int pageSize = 9)
        {
            int ExcludeRecords = (pageSize * pageNumber) - pageSize;

            var stu = db.TblUsers.Where(x => x.UName.Contains(userSearch) || x.UEmail.Contains(userSearch)).OrderBy(x => x.UName)
                .Skip(ExcludeRecords)
                .Take(pageSize);

            var result = new PagedResult<TblUser>
            {
                Data = stu.ToList(),
                TotalItems = db.TblUsers.Count(),
                PageNumber = pageNumber,
                PageSize = pageSize,
            };

          
            return View(result);
        }


        public IActionResult userDetail(int id)
        {
            var user = db.TblUsers.Where(x => x.UId == id).SingleOrDefault();
            return View(user);
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




        public IActionResult ViewCategory(int pageNumber = 1, int pageSize = 9)
        {
            if (HttpContext.Session.GetString("ad_id") == null)
            {
                return RedirectToAction("login");
            }
            if (HttpContext.Session.GetString("ad_id") == null)
            {
                return RedirectToAction("login");
            }
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


        [HttpGet]
        public IActionResult UpdateCategory(int ?id)
        {
            if (HttpContext.Session.GetString("ad_id") == null)
            {
                return RedirectToAction("login");
            }
            TblCategory cat = db.TblCategories.Where(x => x.CatId == id).SingleOrDefault();
            return View(cat);
        }



        [HttpPost]
        public IActionResult UpdateCategory(int id, TblCategory cvm, IFormFile imgfile)
        {
            TblCategory cat = db.TblCategories.Where(x => x.CatId == id).SingleOrDefault();
            if(cat != null)
            {
                ViewBag.sessionv = HttpContext.Session.GetString("ad_id");
                cat.CatName = cvm.CatName;

                if(imgfile != null)
                {
                    string path = uploadingfile(imgfile);
                    if (path.Equals("-1"))
                    {
                        ViewBag.error = "Image could not be uploaded....";
                    }
                    else
                    {
                        string prevImage = cat.CatImage;
                        cat.CatImage = path;
                        if ((System.IO.File.Exists(prevImage)))
                        {
                            System.IO.File.Delete(prevImage);
                        }
                        else
                        {
                            ViewBag.error = "Previous image is not deleted!!";
                        }
                    }
                }

                cat.CatStatus = cvm.CatStatus;
                cat.CatFkAd = Convert.ToInt32(HttpContext.Session.GetString("ad_id"));
                db.SaveChanges();
                return RedirectToAction("ViewCategory");
            
            }

            return View();
        }

        public IActionResult DeleteCategory(int ?id)
        {
            if (HttpContext.Session.GetString("ad_id") == null)
            {
                return RedirectToAction("login");
            }
            TblCategory cat = db.TblCategories.Where(x => x.CatId == id).SingleOrDefault();
            int count = db.TblProducts.Where(x => x.ProFkCat == id).Count();
            if (count > 0)
            {
                ViewBag.Error = "This Category has active Ads, SO you can not Delete it.";
            }
            return View(cat);
        }

        [HttpPost]
        public IActionResult DeleteCategory(int id)
        {
            TblCategory cat = db.TblCategories.Where(x => x.CatId == id).SingleOrDefault();
            db.TblCategories.Remove(cat);
            db.SaveChanges();
 
            return RedirectToAction("ViewCategory");
        }


        public IActionResult ViewAllAds(int pageNumber = 1, int pageSize = 9)
        {
            if (HttpContext.Session.GetString("ad_id") == null)
            {
                return RedirectToAction("login");
            }
            int ExcludeRecords = (pageSize * pageNumber) - pageSize;

            var stu = db.TblProducts.OrderByDescending(x => x.ProId)
                .Skip(ExcludeRecords)
                .Take(pageSize);

            var result = new PagedResult<TblProduct>
            {
                Data = stu.ToList(),
                TotalItems = db.TblProducts.Count(),
                PageNumber = pageNumber,
                PageSize = pageSize,
            };
            return View(result);
        }


        public IActionResult ViewAdsByCategory(int ?id, int pageNumber = 1, int pageSize = 9)
        {
            if (HttpContext.Session.GetString("ad_id") == null)
            {
                return RedirectToAction("login");
            }
            int ExcludeRecords = (pageSize * pageNumber) - pageSize;

            var stu = db.TblProducts.Where(x => x.ProFkCat == id).OrderByDescending(x => x.ProId)
                .Skip(ExcludeRecords)
                .Take(pageSize);

            var result = new PagedResult<TblProduct>
            {
                Data = stu.ToList(),
                TotalItems = db.TblProducts.Count(),
                PageNumber = pageNumber,
                PageSize = pageSize,
            };
            return View(result);
        }



        public IActionResult ViewAdById(int? id)
        {
            if (HttpContext.Session.GetString("ad_id") == null)
            {
                return RedirectToAction("login");
            }

            Adviewmodel ad = new Adviewmodel();
            TblProduct p = db.TblProducts.Where(x => x.ProId == id).SingleOrDefault();
            ad.pro_id = p.ProId;
            ad.pro_name = p.ProName;
            ad.pro_image = p.ProImage;
            ad.pro_price = p.ProPrice;
            ad.pro_des = p.ProDes;
            TblCategory cat = db.TblCategories.Where(x => x.CatId == p.ProFkCat).SingleOrDefault();
            ad.cat_name = cat.CatName;
            TblUser u = db.TblUsers.Where(x => x.UId == p.ProFkUser).SingleOrDefault();
            ad.u_name = u.UName;
            ad.u_image = u.UImage;
            ad.u_contact = u.UContact;
            ad.pro_fk_user = u.UId;
            return View(ad);
        }


        public ActionResult DeleteAd(int? id)
        {

            TblProduct p = db.TblProducts.Where(x => x.ProId == id).SingleOrDefault();
            db.TblProducts.Remove(p);
            db.SaveChanges();

            return RedirectToAction("ViewAllAds");
        }


        public ActionResult Signout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Index", "Home", new { area = "" });
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
                       

                        path = Path.Combine(webRootPath, "upload");
                        
                        string fileName = random + Path.GetFileName(file.FileName);
                        
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
