using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Ajax;
using MyAudioApp.Models;
using PagedList;

namespace MyAudioApp.Controllers
{
    public class MyAudioController : Controller
    {
        private MyAudioDBEntities db = new MyAudioDBEntities();
        public ActionResult Index(string sortOrder, string currentFilter, int? page, string searchstring)
        {
            ViewBag.CurrentSort = sortOrder;
            ViewBag.AlbumNameSortParm = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewBag.SongSortParm = sortOrder == "Song" ? "song_desc" : "Song";
           if(searchstring != null)
           {
               page = 1;
           }
           else
           {
               searchstring = currentFilter;
           }
           ViewBag.CurrentFilter = searchstring;

            var audio = from m in db.MyAudioTBs select m;
            if (!String.IsNullOrEmpty(searchstring))
            {
               
                audio = audio.Where(s => s.Song.Contains(searchstring));
            }
            switch (sortOrder)
            {
                case "name_desc":
                    audio = audio.OrderByDescending(s => s.AlbumName);
                    break;
                case "Song":
                    audio = audio.OrderBy(s => s.Song);
                    break;
                case "song_desc":
                    audio = audio.OrderByDescending(s => s.Song);        
                    break;
                case "albumBy":
                    audio = audio.OrderBy(s => s.AlbumBy);
                    break;
                case "label":
                    audio = audio.OrderBy(s => s.Label);
                    break;
                case "filename":
                    audio = audio.OrderBy(s => s.Filename);
                    break;
                default:  // Name ascending 
                    audio = audio.OrderBy(s => s.AlbumName);
                    break;
            }
            int pageSize = 4;
            int pageNumber = (page ?? 1);
            return View(audio.ToPagedList(pageNumber, pageSize));
        }

        //
        // GET: /MyAudio/Details/5

        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            MyAudioTB audio = db.MyAudioTBs.Find(id);
            if(audio == null )
            {
                return HttpNotFound();
            }
            return View(audio);
        }

        //
        // GET: /MyAudio/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /MyAudio/Create
        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create([Bind(Exclude = "id")] MyAudioTB audiotocreate, HttpPostedFileBase file) 
        {
            if(!ModelState.IsValid )
                 return View();

            try
            {
                if (file.ContentLength > 0)
                {
                    var filename = System.IO.Path.GetFileName(file.FileName);        
                    var path = System.IO.Path.Combine(Server.MapPath("~/Audio"), filename);
                    file.SaveAs(path);
                    audiotocreate.Filename = filename;
                    db.MyAudioTBs.Add(audiotocreate);
                    db.SaveChanges();
                  
                }
                ViewBag.Message = "Upload successful";
                return RedirectToAction("Index");
            }
            catch
            {
                ViewBag.Message = "Upload failed";
                return RedirectToAction("Index");
            }
        }
    
        // GET: /MyAudio/Edit/5

        public ActionResult Edit(int id = 0)
        {
            MyAudioTB audiotoedit = db.MyAudioTBs.Find(id);
            if(audiotoedit == null )
            {
                return HttpNotFound();
            }
            return View(audiotoedit);
        }

        //
        // POST: /MyAudio/Edit/5

        [HttpPost]
        public ActionResult Edit(MyAudioTB audiotoedit)
        {
            if (ModelState.IsValid)
            {
                db.Entry(audiotoedit).State = System.Data.EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("index");
            }
            return View(audiotoedit);
        }

        //
        // GET: /MyAudio/Delete/5

        public ActionResult Delete(int? id)
        {
            if(id == null )
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            MyAudioTB audio = db.MyAudioTBs.Find(id);
            if(audio == null )
            {
                return HttpNotFound();
            }

            return View(audio);
        }

        //
        // POST: /MyAudio/Delete/5

        [HttpPost,ActionName ("Delete")]
    [ValidateAntiForgeryToken ]
        public ActionResult DeleteConfirmed(int id)
        {
            MyAudioTB audio = db.MyAudioTBs.Find(id);
            db.MyAudioTBs.Remove(audio);
            var path = System.IO.Path.Combine(Server.MapPath("~/Audio"), audio.Filename);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        //
        // GET: /MyAudio/Player/12

        public ActionResult Player(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.BadRequest);
            }
            MyAudioTB audio = db.MyAudioTBs.Find(id);
            if (audio == null)
            {
                return HttpNotFound();
            }
            return View(audio);
           
        }
      

    }
}
