using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineAds.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using cloudscribe.Pagination.Models;
namespace OnlineAds.Controllers
{
    public class UserController : Controller
    {
        // image upload
        private readonly IWebHostEnvironment _webHostEnvironment;

        public UserController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
        }
        // above is for image upload
        OnlineAdsDBContext db = new OnlineAdsDBContext();

        public IActionResult Index(int pageNumber=1, int pageSize=2)
        {
            int ExcludeRecords = (pageSize*pageNumber) - pageSize;

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

        public IActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignUp(TblUser uvm, IFormFile imgfile)
        {
            string path = uploadingfile(imgfile);
            if (path.Equals("-1"))
            {
                ViewBag.error = "Image could not be uploaded....";
            }
            else
            {
                TblUser u = new TblUser();
                u.UName = uvm.UName;
                u.UDob = uvm.UDob;
                u.UGender = uvm.UGender;
                u.UCity = uvm.UCity;
                u.UState = uvm.UState;
                u.UEmail = uvm.UEmail;
                u.UPassword = uvm.UPassword;
                u.UImage = path;
                u.UContact = uvm.UContact;
                db.TblUsers.Add(u);
                db.SaveChanges();
                return RedirectToAction("login");

            }

            return View();
        }

        [HttpGet]
        public IActionResult login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult login(TblUser uvm)
        {
            TblUser user = db.TblUsers.Where(x => x.UEmail == uvm.UEmail && x.UPassword == uvm.UPassword).SingleOrDefault();
            if (user != null)
            {
                HttpContext.Session.SetString("u_id", user.UId.ToString());
                return RedirectToAction("CreateAd");
            }
            else
            {
                ViewBag.error = "Invalid username or password";
            }
            return View();
        }


        [HttpGet]
        public ActionResult CreateAd()
        {
            List<TblCategory> li = db.TblCategories.ToList();
            ViewBag.categorylist = new SelectList(li, "CatId", "CatName");

            return View();
        }

        [HttpPost]
        public ActionResult CreateAd(TblProduct pvm, IFormFile imgfile)
        {
            List<TblCategory> li = db.TblCategories.ToList();
            ViewBag.categorylist = new SelectList(li, "cat_id", "cat_name");


            string path = uploadingfile(imgfile);
            if (path.Equals("-1"))
            {
                ViewBag.error = "Image could not be uploaded....";
            }
            else
            {
                TblProduct p = new TblProduct();
                p.ProName = pvm.ProName;
                p.ProPrice = pvm.ProPrice;
                p.ProImage = path;
                p.ProFkCat = pvm.ProFkCat;
                p.ProDes = pvm.ProDes;
                p.ProFkUser = Convert.ToInt32(HttpContext.Session.GetString("u_id"));
                db.TblProducts.Add(p);
                db.SaveChanges();
                return RedirectToAction("Index");

            }

            return View();
        }



        //public ActionResult Ads(int? id, int? page)
        //{
        //    int pagesize = 9, pageindex = 1;
        //    pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
        //    var list = db.TblProducts.Where(x => x.ProFkCat == id).OrderByDescending(x => x.ProId).ToList();
        //    IPagedList<TblProduct> stu = list.ToPagedList(pageindex, pagesize);


        //    return View(stu);


        //}

        public IActionResult Ads(int ?id, int pageNumber = 1, int pageSize = 2)
        {
            int ExcludeRecords = (pageSize * pageNumber) - pageSize;

            var stu = db.TblProducts.Where(x => x.ProFkCat == id).OrderByDescending(x => x.ProId)
                .Skip(ExcludeRecords)
                .Take(pageSize);

            var result = new PagedResult<TblProduct>
            {
                Data = stu.ToList(),
                TotalItems = db.TblProducts.Where(x => x.ProFkCat == id).Count(),
                PageNumber = pageNumber,
                PageSize = pageSize,
            };
            return View(result);
        }





        [HttpPost]
        public ActionResult Ads(int? id, int? page, string search)
        {
            int pagesize = 9, pageindex = 1;
            pageindex = page.HasValue ? Convert.ToInt32(page) : 1;
            var list = db.TblProducts.Where(x => x.ProName.Contains(search)).OrderByDescending(x => x.ProId).ToList();
            IPagedList<TblProduct> stu = list.ToPagedList(pageindex, pagesize);


            return View(stu);


        }



        public IActionResult ViewAd(int? id)
        {
            
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


        public ActionResult Signout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Index");
        }



        public ActionResult DeleteAd(int? id)
        {

            TblProduct p = db.TblProducts.Where(x => x.ProId == id).SingleOrDefault();
            db.TblProducts.Remove(p);
            db.SaveChanges();

            return RedirectToAction("Index");
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
                        path = "~/" + "upload" + "/" + fileName;
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
